/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.sock;

import java.util.*;

/*
* 客户端套接字连接管理
*/
public class FCClientSockets {

    private static HashMap<Integer, FCClientSocket> m_clients = new HashMap<Integer, FCClientSocket>();

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
        FCClientSocket client = new FCClientSocket(type, (long) proxyType, ip, port, proxyIp, proxyPort, proxyUserName, proxyUserPwd, proxyDomain, timeout);
        FCClientSocket.ConnectStatus ret = client.connect();
        if (ret != FCClientSocket.ConnectStatus.CONNECT_SERVER_FAIL) {
            m_socketID++;
            Integer socketID = (Integer) m_socketID;
            client.m_hSocket = m_socketID;
            m_clients.put(socketID, client);
            return socketID;
        } else {
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
