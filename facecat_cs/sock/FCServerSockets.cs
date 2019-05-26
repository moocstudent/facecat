using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace FaceCat {
    public class FCServerSockets {
        private static FCSocketListener m_listener;

        private static HashMap<int, FCServerSocket> m_servers = new HashMap<int, FCServerSocket>();

        private static int m_socketID;

        public static int close(int socketID) {
            int ret = -1;
            if (m_servers.containsKey(socketID)) {
                FCServerSocket server = m_servers.get(socketID);
                ret = server.close();
                m_servers.remove(socketID);
            }
            return ret;
        }

        public static int closeClient(int socketID) {
            int ret = -1;
            if (m_servers.containsKey(socketID)) {
                FCServerSocket server = m_servers.get(socketID);
                ret = server.closeClient(socketID);
                m_servers.remove(socketID);
            }
            return ret;
        }

        public static void recvClientMsg(int socketID, int localSID, byte[] str, int len) {
            m_listener.callBack(socketID, localSID, str, len);
        }

        public static int send(int socketID, int localSID, byte[] str, int len) {
            int ret = -1;
            if (m_servers.containsKey(localSID)) {
                FCServerSocket server = m_servers.get(localSID);
                ret = server.send(socketID, str, len);
            }
            return ret;
        }

        public static int sendTo(int localSID, byte[] str, int len) {
            return -1;
        }

        public static int setListener(FCSocketListener listener) {
            m_listener = listener;
            return 1;
        }

        public static int start(int type, int port) {
            try {
                FCServerSocket server = new FCServerSocket();
                if (type == 0) {
                    server.startTCP(port);
                }
                m_socketID++;
                int socketID = m_socketID;
                server.m_hSocket = m_socketID;
                m_servers.put(socketID, server);
                return m_socketID;
            }
            catch (Exception ex) {
                return -1;
            }
        }

        public static void writeServerLog(int socketID, int localSID, int state, String log) {
            m_listener.writeLog(socketID, localSID, state, log);
        }
    }
}
