using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace FaceCat {
    public class FCClientSocket {
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

        private bool m_blnProxyServerOk;
        private bool m_connected;
        public int m_hSocket;
        private String m_ip;
        private bool m_isDeleted;
        private int m_port;
        private String m_proxyDomain;
        private long m_proxyType;
        private String m_proxyIp;
        private int m_proxyPort;
        private String m_proxyUserName;
        private String m_proxyUserPwd;
        private Socket m_socket = null;
        private int m_timeout;
        private int m_type;
        private Socket m_udpSocket = null;

        /// <summary>
        /// 析构函数
        /// </summary>
        ~FCClientSocket() {
            delete();
        }

        public int close() {
            int ret = -1;
            if (m_socket != null) {
                try {
                    m_socket.Close();
                    ret = 1;
                }
                catch (Exception ex) {
                    byte[] rmsg = new byte[1];
                    rmsg[0] = (byte)((char)2);
                    //FCClientSockets.recvClientMsg(m_hSocket, m_hSocket, rmsg, 1);
                    //FCClientSockets.writeClientLog(m_hSocket, m_hSocket, 2, "socket exit");
                    ret = -1;
                }
            }
            if (m_udpSocket != null) {
                try {
                    m_udpSocket.Close();
                    ret = 1;
                }
                catch (Exception ex) {
                    byte[] rmsg = new byte[1];
                    rmsg[0] = (byte)((char)2);
                    //FCClientSockets.recvClientMsg(m_hSocket, m_hSocket, rmsg, 1);
                    //FCClientSockets.writeClientLog(m_hSocket, m_hSocket, 2, "udp exit");
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
            IPAddress ip = IPAddress.Parse(m_ip);
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try {
                clientSocket.Connect(new IPEndPoint(ip, m_port));
                status = ConnectStatus.SUCCESS;
                m_connected = true;
                Thread tThread = new Thread(new ThreadStart(run));
                tThread.Start();
            }
            catch {
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
            bool get = false;
            int head = 0, pos = 0, strRemain = 0, bufferRemain = 0, index = 0, len = 0;
            int intSize = 4;
            byte[] headStr = new byte[4];
            int headSize = 4;
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);//定义要发送的计算机的地址
            EndPoint Remote = (EndPoint)(sender);//
            while (m_connected) {
                try {
                    byte[] buffer = new byte[10240];
                    if (m_type == 0) {
                        len = m_socket.Receive(buffer);
                    }
                    else if (m_type == 1) {
                        m_udpSocket.ReceiveFrom(buffer, ref Remote);
                    }
                    if (len == 0 || len == -1) {
                        byte[] rmsg = new byte[1];
                        rmsg[0] = (byte)((char)3);
                        //FCClientSockets.recvClientMsg(m_hSocket, m_hSocket, rmsg, 1);
                        //FCClientSockets.writeClientLog(m_hSocket, m_hSocket, 3, "socket error");
                        break;
                    }
                    index = 0;
                    while (index < len) {
                        int diffSize = 0;
                        if (!get) {
                            diffSize = intSize - headSize;
                            if (diffSize == 0) {
                                head = ((byte)(0xff & buffer[index]) | (byte)(0xff00 & (buffer[index + 1] << 8)) | (byte)(0xff0000 & (buffer[index + 2] << 16)) | (byte)(0xff000000 & (buffer[index + 3] << 24)));
                            }
                            else {
                                for (int i = 0; i < diffSize; i++) {
                                    headStr[headSize + i] = buffer[i];
                                }
                                head = ((byte)(0xff & headStr[0]) | (byte)(0xff00 & (headStr[1] << 8)) | (byte)(0xff0000 & (headStr[2] << 16)) | (byte)(0xff000000 & (headStr[3] << 24)));
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
                        int remain = Math.Min(strRemain, bufferRemain);
                        Array.Copy(buffer, index, str, pos, remain);
                        pos += remain;
                        index += remain;
                        if (!get) {
                            //FCClientSockets.recvClientMsg(m_hSocket, m_hSocket, str, head);
                            head = 0;
                            pos = 0;
                            if (len - index == 0 || len - index >= intSize) {
                                headSize = intSize;
                            }
                            else {
                                headSize = bufferRemain - strRemain;
                                for (int j = 0; j < headSize; j++) {
                                    headStr[j] = buffer[index + j];
                                }
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex) {
                    break;
                }
            }
            byte[] rmsg2 = new byte[1];
            rmsg2[0] = (byte)((char)2);
            //FCClientSockets.recvClientMsg(m_hSocket, m_hSocket, rmsg, 1);
            //FCClientSockets.writeClientLog(m_hSocket, m_hSocket, 2, "socket exit");
            m_connected = false;
        }

        public int send(byte[] str, int len) {
            if (m_socket == null || !m_connected) {
                return -1;
            }
            return m_socket.Send(str);
        }

        public int sendTo(byte[] str, int len) {
            if (m_udpSocket == null || !m_connected) {
                return -1;
            }
            return m_udpSocket.Send(str);
        }
    }
}
