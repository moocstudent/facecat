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

/*
* 客户端套接字连接
*/
public class FCClientSocket implements Runnable {

    public FCClientSocket(int type, long proxyType, String ip, int port, String proxyIp, int proxyPort, String proxyUserName, String proxyUserPwd, String proxyDomain, int timeout) {
        m_blnProxyServerOk = true;
        m_proxyDomain = proxyDomain;
        m_proxyType = proxyType;
        m_ip = ip;
        m_port = port;
        m_proxyIp = proxyIp;
        m_proxyPort = proxyPort;
        m_proxyUserName = proxyUserName;
        m_proxyUserPwd = proxyUserPwd;
        m_timeout = timeout;
        m_type = type;
    }

    protected void finalize() throws Throwable {
        delete();
    }

    private boolean m_blnProxyServerOk = false;
    private boolean m_connected = false;
    public int m_hSocket;
    private DataInputStream m_inputStream = null;
    private String m_ip;
    private boolean m_isDeleted;
    private int m_port;
    private String m_proxyDomain;
    private long m_proxyType;
    private String m_proxyIp;
    private int m_proxyPort;
    private String m_proxyUserName;
    private String m_proxyUserPwd;
    private ArrayList<byte[]> m_sendDatas = new ArrayList<byte[]>();
    private Socket m_socket = null;
    int m_timeout;
    int m_type;
    private DatagramSocket m_udpSocket = null;

    public int close() {
        int ret = -1;
        if (m_socket != null) {
            try {
                m_socket.close();
                ret = 1;
            } catch (Exception ex) {
                byte[] rmsg = new byte[1];
                rmsg[0] = (byte) ((char) 2);
                FCClientSockets.recvClientMsg(m_hSocket, m_hSocket, rmsg, 1);
                FCClientSockets.writeClientLog(m_hSocket, m_hSocket, 2, "socket exit");
                ret = -1;
            }
        }
        if (m_udpSocket != null) {
            try {
                m_udpSocket.close();
                ret = 1;
            } catch (Exception ex) {
                byte[] rmsg = new byte[1];
                rmsg[0] = (byte) ((char) 2);
                FCClientSockets.recvClientMsg(m_hSocket, m_hSocket, rmsg, 1);
                FCClientSockets.writeClientLog(m_hSocket, m_hSocket, 2, "udp exit");
                ret = -1;
            }
        }
        m_connected = false;
        return ret;
    }

    public ConnectStatus connect() {
        return connectStandard();
    }

    private ConnectStatus connectStandard() {
        ConnectStatus status = ConnectStatus.CONNECT_SERVER_FAIL;
        if (m_type == 0) {
            if (m_socket == null) {
                try {
                    m_socket = new Socket();
                    InetSocketAddress socketAddress = new InetSocketAddress(m_ip, m_port);
                    m_socket.connect(socketAddress, m_timeout);
                    m_inputStream = new DataInputStream(m_socket.getInputStream());
                    m_connected = true;
                    status = ConnectStatus.SUCCESS;
                    byte[] submitStr = new byte[1024];
                    submitStr[0] = 'm';
                    submitStr[1] = 'i';
                    submitStr[2] = 'a';
                    submitStr[3] = 'o';
                    send(submitStr, 1024);
                    Thread thread = new Thread(this);
                    thread.start();
                } catch (Exception ex) {
                    status = ConnectStatus.CONNECT_SERVER_FAIL;
                }
            }
        } else if (m_type == 1) {
            if (m_udpSocket != null) {
                try {
                    InetAddress addr = InetAddress.getByName(m_ip);
                    m_udpSocket = new DatagramSocket(m_port, addr);
                } catch (Exception ex) {
                    status = ConnectStatus.CONNECT_SERVER_FAIL;
                }
            }
        }
        if (status == ConnectStatus.SUCCESS) {
            new Thread(new Runnable() {
                @Override
                public void run() {
                    while (m_connected) {
                        try {
                            byte[] sendBytes = null;
                            synchronized (m_sendDatas) {
                                int datasSize = m_sendDatas.size();
                                if (datasSize > 0) {
                                    sendBytes = m_sendDatas.get(0);
                                    m_sendDatas.remove(0);
                                }
                            }
                            if (sendBytes != null) {
                                DataOutputStream outputStream = new DataOutputStream(m_socket.getOutputStream());
                                outputStream.write(sendBytes);
                                outputStream.flush();
                            } else {
                                Thread.sleep(1);
                            }
                        } catch (Exception ex) {

                        }
                    }
                }
            }).start();
        }
        return status;
    }

    private ConnectStatus connectByHttp() {
        return ConnectStatus.SUCCESS;
    }

    private ConnectStatus connectBySock4() {
        return ConnectStatus.SUCCESS;
    }

    private ConnectStatus connectBySock5() {
        return ConnectStatus.SUCCESS;
    }

    private ConnectStatus connectProxyServer() {
        return ConnectStatus.SUCCESS;
    }

    public void delete() {
        if (!m_isDeleted) {
            close();
            m_connected = false;
            m_isDeleted = true;
        }
    }

    public void run() {
        byte[] str = null;
        boolean get = false;
        int head = 0, pos = 0, strRemain = 0, bufferRemain = 0, index = 0, len = 0;
        int intSize = 4;
        byte[] headStr = new byte[4];
        int headSize = 4;
        while (m_connected) {
            try {
                byte[] buffer = new byte[10240];
                if (m_type == 0) {
                    len = m_inputStream.read(buffer);
                } else if (m_type == 1) {
                    DatagramPacket recvPacket = new DatagramPacket(buffer, buffer.length);
                    m_udpSocket.receive(recvPacket);
                }
                if (len == 0 || len == -1) {
                    byte[] rmsg = new byte[1];
                    rmsg[0] = (byte) ((char) 3);
                    FCClientSockets.recvClientMsg(m_hSocket, m_hSocket, rmsg, 1);
                    FCClientSockets.writeClientLog(m_hSocket, m_hSocket, 3, "socket error");
                    break;
                }
                index = 0;
                while (index < len) {
                    int diffSize = 0;
                    if (!get) {
                        diffSize = intSize - headSize;
                        if (diffSize == 0) {
                            head = (0xff & buffer[index]) | (0xff00 & (buffer[index + 1] << 8))
                                    | (0xff0000 & (buffer[index + 2] << 16)) | (0xff000000 & (buffer[index + 3] << 24));
                        } else {
                            for (int i = 0; i < diffSize; i++) {
                                headStr[headSize + i] = buffer[i];
                            }
                            head = (0xff & headStr[0]) | (0xff00 & (headStr[1] << 8))
                                    | (0xff0000 & (headStr[2] << 16)) | (0xff000000 & (headStr[3] << 24));
                        }
                        if (str != null) {
                            str = null;
                        }
                        str = new byte[head];
                        if (diffSize > 0) {
                            for (int i = 0; i < headSize; i++) {
                                str[i] = headStr[i];
                            }
                            pos += headSize;
                            headSize = intSize;
                        }
                    }
                    bufferRemain = len - index;
                    strRemain = head - pos;
                    get = strRemain > bufferRemain;
                    int remain = Math.min(strRemain, bufferRemain);
                    System.arraycopy(buffer, index, str, pos, remain);
                    pos += remain;
                    index += remain;
                    if (!get) {
                        FCClientSockets.recvClientMsg(m_hSocket, m_hSocket, str, head);
                        head = 0;
                        pos = 0;
                        if (len - index == 0 || len - index >= intSize) {
                            headSize = intSize;
                        } else {
                            headSize = bufferRemain - strRemain;
                            for (int j = 0; j < headSize; j++) {
                                headStr[j] = buffer[index + j];
                            }
                            break;
                        }
                    }
                }
            } catch (Exception ex) {
                break;
            }
        }
        byte[] rmsg = new byte[1];
        rmsg[0] = (byte) ((char) 2);
        FCClientSockets.recvClientMsg(m_hSocket, m_hSocket, rmsg, 1);
        FCClientSockets.writeClientLog(m_hSocket, m_hSocket, 2, "socket exit");
        m_connected = false;
    }

    public int send(byte[] str, int len) {
        if (m_socket == null || !m_connected) {
            return -1;
        }
        int ret = -1;
        try {
            synchronized (m_sendDatas) {
                m_sendDatas.add(str);
            }
            ret = len;
        } catch (Exception ex) {
            ret = -1;
        }
        return ret;
    }

    public int sendTo(byte[] str, int len) {
        if (m_socket == null || !m_connected) {
            return -1;
        }
        int ret = -1;
        try {
            synchronized (m_sendDatas) {
                m_sendDatas.add(str);
            }
            new Thread(new Runnable() {
                @Override
                public void run() {
                    try {
                        byte[] sendBytes = null;
                        synchronized (m_sendDatas) {
                            int datasSize = m_sendDatas.size();
                            if (datasSize > 0) {
                                sendBytes = m_sendDatas.get(0);
                                m_sendDatas.remove(0);
                            }
                        }
                        if (sendBytes != null) {
                            DatagramPacket sendPacket = new DatagramPacket(sendBytes, sendBytes.length);
                            m_udpSocket.send(sendPacket);
                        }

                    } catch (Exception ex) {

                    }
                }
            }).start();
            ret = len;
        } catch (Exception ex) {
            ret = -1;
        }
        return ret;
    }

    public static enum ConnectStatus {

        SUCCESS,
        CONNECT_PROXY_FAIL,
        NOT_CONNECT_PROXY,
        CONNECT_SERVER_FAIL;

        public int getValue() {
            return this.ordinal();
        }

        public static ConnectStatus forValue(int value) {
            return ConnectStatus.values()[value];
        }
    }
}
