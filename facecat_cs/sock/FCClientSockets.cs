using System;
using System.Collections.Generic;
using System.Text;

namespace FaceCat {
    public class FCClientSockets {
        private static HashMap<int, FCClientSocket> m_clients = new HashMap<int, FCClientSocket>();

        private static FCSocketListener m_listener;

        private static int m_socketID;

        public static int close(int socketID) {
            int ret = -1;
            if (m_clients.containsKey(socketID)) {
                FCClientSocket client = m_clients.get(socketID);
                ret = client.close();
                m_clients.remove(socketID);
            }
            return ret;
        }

        public static int connect(int type, int proxyType, String ip, int port, String proxyIp, int proxyPort, String proxyUserName, String proxyUserPwd, String proxyDomain, int timeout) {
            FCClientSocket client = new FCClientSocket(type, (long)proxyType, ip, port, proxyIp, proxyPort, proxyUserName, proxyUserPwd, proxyDomain, timeout);
            ConnectStatus ret = client.connect();
            if (ret != ConnectStatus.CONNECT_SERVER_FAIL) {
                m_socketID++;
                int socketID = m_socketID;
                client.m_hSocket = m_socketID;
                m_clients.put(socketID, client);
                return socketID;
            }
            else {
                client.delete();
                return -1;
            }
        }

        public static void recvClientMsg(int socketID, int localSID, byte[] str, int len) {
            m_listener.callBack(socketID, localSID, str, len);
        }

        public static int send(int socketID, byte[] str, int len) {
            int ret = -1;
            if (m_clients.containsKey(socketID)) {
                FCClientSocket client = m_clients.get(socketID);
                ret = client.send(str, len);
            }
            return ret;
        }

        public static int sendTo(int socketID, byte[] str, int len) {
            int ret = -1;
            if (m_clients.containsKey(socketID)) {
                FCClientSocket client = m_clients.get(socketID);
                ret = client.sendTo(str, len);
            }
            return ret;
        }

        public static int setListener(FCSocketListener listener) {
            m_listener = listener;
            return 1;
        }

        public static void writeClientLog(int socketID, int localSID, int state, String log) {
            m_listener.writeLog(socketID, localSID, state, log);
        }
    }
}
