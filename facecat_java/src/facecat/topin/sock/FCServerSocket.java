/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.sock;

import java.io.*;
import java.net.*;
import java.util.*;
import java.util.concurrent.*;
import java.nio.*;
import java.nio.channels.*;

/*
* 服务端套接字连接
*/
public class FCServerSocket {

    private HashMap<Integer, SOCKDATA> m_datas = new HashMap<Integer, SOCKDATA>();
    public int m_hSocket;
    private Selector m_selector = null;
    private ServerSocket m_socket = null;
    private ServerSocketChannel m_ssc = null;
    private DatagramSocket m_udpSocket = null;

    public int close() {
        if (m_socket != null) {
            try {
                m_socket.close();
                return 1;
            } catch (Exception ex) {

            }
        }
        if (m_udpSocket != null) {
            try {
                m_udpSocket.close();
                return 1;
            } catch (Exception ex) {

            }
        }
        return -1;
    }

    public int closeClient(int socketID) {
        if (m_datas.containsKey(socketID)) {
            try {
                m_datas.get(socketID).m_socket.close();
                return 1;
            } catch (Exception ex) {

            }
        }
        return -1;
    }

    public int recv(SOCKDATA data) {
        if (!data.m_submit) {
            if (data.m_len == 1024 && data.m_buffer[0] == 'm' && data.m_buffer[1] == 'i' && data.m_buffer[2] == 'a' && data.m_buffer[3] == 'o') {
                data.m_submit = true;
                return 1;
            } else {
                return -1;
            }
        }
        int intSize = 4;
        data.m_index = 0;
        while (data.m_index < data.m_len) {
            int diffSize = 0;
            if (!data.m_get) {
                diffSize = intSize - data.m_headSize;
                if (diffSize == 0) {
                    data.m_head = (0xff & data.m_buffer[data.m_index]) | (0xff00 & (data.m_buffer[data.m_index + 1] << 8))
                            | (0xff0000 & (data.m_buffer[data.m_index + 2] << 16)) | (0xff000000 & (data.m_buffer[data.m_index + 3] << 24));
                } else {
                    for (int i = 0; i < diffSize; i++) {
                        data.m_headStr[data.m_headSize + i] = data.m_buffer[i];
                    }
                    data.m_head = (0xff & data.m_headStr[0]) | (0xff00 & (data.m_headStr[1] << 8))
                            | (0xff0000 & (data.m_headStr[2] << 16)) | (0xff000000 & (data.m_headStr[3] << 24));
                }
                if (data.m_str != null) {
                    data.m_str = null;
                }
                data.m_str = new byte[data.m_head];
                if (diffSize > 0) {
                    for (int i = 0; i < data.m_headSize; i++) {
                        data.m_str[i] = data.m_headStr[i];
                    }
                    data.m_pos += data.m_headSize;
                    data.m_headSize = intSize;
                }
            }
            data.m_bufferRemain = data.m_len - data.m_index;
            data.m_strRemain = data.m_head - data.m_pos;
            data.m_get = data.m_strRemain > data.m_bufferRemain;
            int remain = Math.min(data.m_strRemain, data.m_bufferRemain);
            System.arraycopy(data.m_buffer, data.m_index, data.m_str, data.m_pos, remain);
            data.m_pos += remain;
            data.m_index += remain;
            if (!data.m_get) {
                FCServerSockets.recvClientMsg(data.m_hSocket, m_hSocket, data.m_str, data.m_head);
                data.m_head = 0;
                data.m_pos = 0;
                if (data.m_len - data.m_index == 0 || data.m_len - data.m_index >= intSize) {
                    data.m_headSize = intSize;
                } else {
                    data.m_headSize = data.m_bufferRemain - data.m_strRemain;
                    for (int j = 0; j < data.m_headSize; j++) {
                        data.m_headStr[j] = data.m_buffer[data.m_index + j];
                    }
                    break;
                }
            }
        }
        return 1;
    }

    protected void readData(SelectionKey key) throws IOException {
        SocketChannel socketChannel = (SocketChannel) key.channel();
        Socket socket = socketChannel.socket();
        ByteBuffer receiveBuffer = ByteBuffer.allocate(1024);
        int len = -1;
        try {
            len = socketChannel.read(receiveBuffer);
        } catch (Exception ex) {
        }
        if (len > 0) {
            byte[] buffer = receiveBuffer.array();
            int socketID = socket.hashCode();
            SOCKDATA data = m_datas.get(socketID);
            if (data != null) {
                data.m_buffer = buffer;
                data.m_len = len;
                int state = recv(data);
                if (state == -1) {
                    socketChannel.close();
                    FCServerSockets.writeServerLog(socketID, m_hSocket, 2, "socket exit");
                    m_datas.put(socketID, null);
                }
            }
        } else {
            int socketID = socket.hashCode();
            socketChannel.close();
            FCServerSockets.writeServerLog(socketID, m_hSocket, 2, "socket exit");
        }
        receiveBuffer.clear();
    }

    public int send(int socketID, byte[] str, int len) {
        int ret = -1;
        try {
            SOCKDATA data = m_datas.get(socketID);
            if (data != null) {
                ByteBuffer sendBuffer = ByteBuffer.wrap(str);
                data.m_socket.getChannel().write(sendBuffer);
                sendBuffer.clear();
                ret = len;
            }
        } catch (Exception ex) {
            ret = -1;
        }
        return ret;

    }

    public int sendTo(byte[] str, int len) {
        int ret = -1;
        try {
            DatagramPacket sendPacket = new DatagramPacket(str, len);
            m_udpSocket.send(sendPacket);
            ret = len;
        } catch (Exception ex) {
            ret = -1;
        }
        return ret;
    }

    public int startTCP(int port) throws IOException {
        try {
            int number = 1;
            m_ssc = ServerSocketChannel.open();
            m_ssc.configureBlocking(false);
            m_socket = m_ssc.socket();
            m_selector = Selector.open();
            m_socket.bind(new InetSocketAddress(port));
            m_ssc.register(m_selector, SelectionKey.OP_ACCEPT);
            for (int i = 0; i < number; i++) {
                Thread thread = new Thread() {
                    public void run() {
                        while (true) {
                            try {
                                if (m_selector.select() == 0) {
                                    continue;
                                }
                                Iterator<SelectionKey> iter = m_selector.selectedKeys().iterator();
                                while (iter.hasNext()) {
                                    SelectionKey key = iter.next();
                                    iter.remove();
                                    if (key.isAcceptable()) {
                                        SocketChannel channel = m_ssc.accept();
                                        channel.configureBlocking(false);
                                        channel.register(m_selector, SelectionKey.OP_READ);
                                        SOCKDATA data = new SOCKDATA();
                                        data.m_socket = channel.socket();
                                        data.m_hSocket = data.m_socket.hashCode();
                                        m_datas.put(data.m_hSocket, data);
                                        FCServerSockets.writeServerLog(data.m_hSocket, m_hSocket, 1, "accept:" + channel.getRemoteAddress());
                                    } else if (key.isReadable()) {
                                        readData(key);
                                    }
                                }
                            } catch (Exception ex) {

                            }
                        }
                    }
                };
                thread.start();
            }
            return 1;
        } catch (Exception ex) {

        }
        return -1;
    }

    public int startUDP(int port) throws IOException {
        try {
            m_udpSocket = new DatagramSocket(port);
            SOCKDATA data = new SOCKDATA();
            m_datas.put(0, data);
            Thread thread = new Thread() {
                public void run() {
                    while (true) {
                        try {
                            byte[] buffer = new byte[1024];
                            DatagramPacket recvPacket = new DatagramPacket(buffer, buffer.length);
                            m_udpSocket.receive(recvPacket);
                            int len = recvPacket.getLength();
                            if (len > 0) {
                                SOCKDATA data = m_datas.get(0);
                                if (data != null) {
                                    data.m_buffer = buffer;
                                    data.m_len = len;
                                    int state = recv(data);
                                    if (state == -1) {
                                    }
                                }
                            }
                        } catch (Exception ex) {
                        }
                    }
                }
            };
            thread.start();
            return 1;
        } catch (Exception ex) {
            return -1;
        }
    }
}
