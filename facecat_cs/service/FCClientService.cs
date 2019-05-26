/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-捂脸鹿创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO.Compression;

namespace FaceCat {
    /// <summary>
    /// 基础服务类
    /// </summary>
    public class FCClientService {
        /// <summary>
        /// 创建服务
        /// </summary>
        public FCClientService() {
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~FCClientService() {
            lock (m_listeners) {
                m_listeners.Clear();
            }
            lock (m_waitMessages) {
                m_waitMessages.Clear();
            }
        }

        /// <summary>
        /// 无压缩
        /// </summary>
        public static int COMPRESSTYPE_NONE = 0;

        /// <summary>
        /// GZIP压缩
        /// </summary>
        public static int COMPRESSTYPE_GZIP = 1;

        /// <summary>
        /// 监听者集合
        /// </summary>
        private HashMap<int, FCMessageListener> m_listeners = new HashMap<int, FCMessageListener>();

        /// <summary>
        /// 请求ID
        /// </summary>
        private static int m_requestID = 10000;

        /// <summary>
        /// 所有的服务
        /// </summary>
        private static HashMap<int, FCClientService> m_services = new HashMap<int, FCClientService>();

        /// <summary>
        /// 等待消息队列
        /// </summary>
        private HashMap<int, FCMessage> m_waitMessages = new HashMap<int, FCMessage>();

        private int m_compressType = COMPRESSTYPE_NONE;

        /// <summary>
        /// 获取或设置压缩类型
        /// </summary>
        public int CompressType {
            get { return m_compressType; }
            set { m_compressType = value; }
        }

        protected static long m_downFlow;

        /// <summary>
        /// 获取或设置下载流量
        /// </summary>
        public static long DownFlow {
            get { return FCClientService.m_downFlow; }
            set { FCClientService.m_downFlow = value; }
        }

        private int m_groupID = 0;

        /// <summary>
        /// 获取或设置组ID
        /// </summary>
        public int GroupID {
            get { return m_groupID; }
            set { m_groupID = value; }
        }

        private bool m_isDeleted = false;

        /// <summary>
        /// 获取对象是否已被销毁
        /// </summary>
        public bool IsDeleted {
            get { return m_isDeleted; }
        }

        private int m_serviceID = 0;

        /// <summary>
        /// 获取或设置服务的ID
        /// </summary>
        public int ServiceID {
            get { return m_serviceID; }
            set { m_serviceID = value; }
        }

        private int m_sessionID = 0;

        /// <summary>
        /// 获取或设置登录ID
        /// </summary>
        public int SessionID {
            get { return m_sessionID; }
            set { m_sessionID = value; }
        }

        protected static long m_upFlow;

        /// <summary>
        /// 获取或设置上传流量
        /// </summary>
        public static long UpFlow {
            get { return m_upFlow; }
            set { m_upFlow = value; }
        }

        /// <summary>
        /// 连接到服务器
        /// </summary>
        /// <param name="proxyType">代理类型</param>
        /// <param name="ip">IP</param>
        /// <param name="port">端口</param>
        /// <param name="proxyIp">代理IP</param>
        /// <param name="proxyPort">代理端口</param>
        /// <param name="proxyUserName">用户名</param>
        /// <param name="proxyUserPwd">密码</param>
        /// <param name="proxyDomain">域</param>
        /// <param name="timeout">超时</param>
        /// <returns>状态</returns>
        public static int connectToServer(int proxyType, String ip, int port, String proxyIp, int proxyPort, String proxyUserName, String proxyUserPwd, String proxyDomain, int timeout) {
            FCClientSockets.setListener(new FCClientSocketListener());
            return FCClientSockets.connect(0, proxyType, ip, (short)port, proxyIp, (short)proxyPort, proxyUserName, proxyUserPwd, proxyDomain, timeout);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="socketID">套接字ID</param>
        /// <param name="str">数据</param>
        /// <param name="len">长度</param>
        /// <returns>状态</returns>
        public static int sendByClient(int socketID, byte[] str, int len) {
            return FCClientSockets.send(socketID, str, len);
        }

        /// <summary>
        /// 添加服务
        /// </summary>
        /// <param name="service">服务</param>
        public static void addService(FCClientService service) {
            m_services.put(service.ServiceID, service);
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="ip">地址</param>
        /// <param name="port">端口</param>
        public static int connect(String ip, int port) {
            int socketID = FCClientSockets.connect(0, 0, ip, port, "", 0, "", "", "", 30000);
            return socketID;
        }

        /// <summary>
        /// 回调函数
        /// </summary>
        /// <param name="socketID">连接ID</param>
        /// <param name="localSID">本地连接ID</param>
        /// <param name="str">数据</param>
        /// <param name="len">长度</param>
        public static void callBack(int socketID, int localSID, byte[] bytes, int len) {
            m_downFlow += len;
            try {
                if (len > 4) {
                    FCBinary br = new FCBinary();
                    br.write(bytes, len);
                    int head = br.readInt();
                    int groupID = br.readShort();
                    int serviceID = br.readShort();
                    if (m_services.containsKey(serviceID)) {
                        m_services.get(serviceID).onCallBack(br, socketID, localSID, len);
                    }
                    br.close();
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        public virtual void delete() {
            if (!m_isDeleted) {
                m_listeners.Clear();
                m_isDeleted = true;
            }
        }

        /// <summary>
        /// 获取请求ID
        /// </summary>
        /// <returns>请求ID</returns>
        public static int getRequestID() {
            return m_requestID++;
        }

        /// <summary>
        /// 获取所有的服务
        /// </summary>
        /// <param name="services">服务列表</param>
        public static void getServices(ArrayList<FCClientService> services) {
            foreach (FCClientService service in m_services.Values) {
                services.Add(service);
            }
        }

        /// <summary>
        /// 保持活跃
        /// </summary>
        /// <param name="socketID">套接字ID</param>
        /// <returns>状态</returns>
        public virtual int keepAlive(int socketID) {
            FCBinary bw = new FCBinary();
            bw.writeInt((int)4);
            byte[] bytes = bw.getBytes();
            int length = bytes.Length;
            int ret = FCClientSockets.send(socketID, bytes, length);
            bw.close();
            return ret;
        }

        /// <summary>
        /// 收到消息
        /// </summary>
        /// <param name="br">流</param>
        /// <param name="socketID">套接字ID</param>
        /// <param name="localSID">本地套接字ID</param>
        /// <param name="len">长度</param>
        public virtual void onCallBack(FCBinary br, int socketID, int localSID, int len) {
            int headSize = sizeof(int) * 4 + sizeof(short) * 3 + sizeof(byte) * 2;
            int functionID = br.readShort();
            int sessionID = br.readInt();
            int requestID = br.readInt();
            int state = br.readByte();
            int compressType = br.readByte();
            int bodyLength = br.readInt();
            byte[] body = new byte[len - headSize];
            br.readBytes(body);
            if (compressType == COMPRESSTYPE_GZIP) {
                using (MemoryStream dms = new MemoryStream()) {
                    using (MemoryStream cms = new MemoryStream(body)) {
                        using (GZipStream gzip = new GZipStream(cms, CompressionMode.Decompress)) {
                            byte[] buffer = new byte[1024];
                            int size = 0;
                            while ((size = gzip.Read(buffer, 0, buffer.Length)) > 0) {
                                dms.Write(buffer, 0, size);
                            }
                            body = dms.ToArray();
                        }
                    }
                }
            }
            FCMessage message = new FCMessage(GroupID, ServiceID, functionID, sessionID, requestID, socketID, state, compressType, bodyLength, body);
            onReceive(message);
            onWaitMessageHandle(message);
            body = null;
        }

        /// <summary>
        /// 客户端关闭方法
        /// </summary>
        /// <param name="socketID">连接ID</param>
        /// <param name="localSID">本地连接ID</param>
        public virtual void onClientClose(int socketID, int localSID) {
        }

        /// <summary>
        /// 客户端连接方法
        /// </summary>
        /// <param name="socketID">连接ID</param>
        /// <param name="localSID">本地连接ID</param>
        public virtual void onClientConnect(int socketID, int localSID) {
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="message">消息</param>
        public virtual void onReceive(FCMessage message) {
        }

        /// <summary>
        /// 等待消息的处理
        /// </summary>
        /// <param name="message">消息</param>
        public virtual void onWaitMessageHandle(FCMessage message) {
            if (m_waitMessages.Count > 0) {
                lock (m_waitMessages) {
                    if (m_waitMessages.ContainsKey(message.m_requestID)) {
                        FCMessage waitMessage = m_waitMessages.get(message.m_requestID);
                        waitMessage.copy(message);
                    }
                }
            }
        }

        /// <summary>
        /// 注册数据监听
        /// </summary>
        /// <param name="requestID">请求ID</param>
        /// <param name="callBack">回调函数</param>
        public virtual void registerListener(int requestID, ListenerMessageCallBack callBack) {
            lock (m_listeners) {
                FCMessageListener listener = null;
                if (!m_listeners.containsKey(requestID)) {
                    listener = new FCMessageListener();
                    m_listeners.put(requestID, listener);
                } else {
                    listener = m_listeners.get(requestID);
                }
                listener.add(callBack);
            }
        }

        /// <summary>
        /// 注册等待
        /// </summary>
        /// <param name="requestID">请求ID</param>
        /// <param name="message">消息</param>
        public virtual void registerWait(int requestID, FCMessage message) {
            lock (m_waitMessages) {
                m_waitMessages.put(requestID, message);
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息</param>
        public virtual int send(FCMessage message) {
            FCBinary bw = new FCBinary();
            byte[] body = message.m_body;
            int bodyLength = message.m_bodyLength;
            int uncBodyLength = bodyLength;
            if (message.m_compressType == COMPRESSTYPE_GZIP) {
                using (MemoryStream cms = new MemoryStream()) {
                    using (GZipStream gzip = new GZipStream(cms, CompressionMode.Compress)) {
                        gzip.Write(body, 0, body.Length);
                    }
                    body = cms.ToArray();
                    bodyLength = body.Length;
                }
            }
            int len = sizeof(int) * 4 + bodyLength + sizeof(short) * 3 + sizeof(byte) * 2;
            bw.writeInt(len);
            bw.writeShort((short)message.m_groupID);
            bw.writeShort((short)message.m_serviceID);
            bw.writeShort((short)message.m_functionID);
            bw.writeInt(message.m_sessionID);
            bw.writeInt(message.m_requestID);
            bw.writeByte((byte)message.m_state);
            bw.writeByte((byte)message.m_compressType);
            bw.writeInt(uncBodyLength);
            bw.writeBytes(body);
            byte[] bytes = bw.getBytes();
            int length = bytes.Length;
            int ret = FCClientSockets.send(message.m_socketID, bytes, length);
            m_upFlow += ret;
            bw.close();
            return ret;
        }

        /// <summary>
        /// 发送到监听者
        /// </summary>
        /// <param name="message">消息</param>
        public virtual void sendToListener(FCMessage message) {
            FCMessageListener listener = null;
            lock (m_listeners) {
                if (m_listeners.containsKey(message.m_requestID)) {
                    listener = m_listeners.get(message.m_requestID);
                }
            }
            if (listener != null) {
                listener.callBack(message);
            }
        }

        /// <summary>
        /// 取消注册数据监听
        /// </summary>
        /// <param name="requestID">请求ID</param>
        public virtual void unRegisterListener(int requestID) {
            lock (m_listeners) {
                m_listeners.Remove(requestID);
            }
        }

        /// <summary>
        /// 取消注册监听
        /// </summary>
        /// <param name="requestID">请求ID</param>
        /// <param name="callBack">回调函数</param>
        public virtual void unRegisterListener(int requestID, ListenerMessageCallBack callBack) {
            lock (m_listeners) {
                if (m_listeners.ContainsKey(requestID)) {
                    m_listeners.get(requestID).remove(callBack);
                }
            }
        }

        /// <summary>
        /// 取消注册等待
        /// </summary>
        /// <param name="requestID">请求ID</param>
        public virtual void unRegisterWait(int requestID) {
            lock (m_waitMessages) {
                if (m_waitMessages.ContainsKey(requestID)) {
                    m_waitMessages.Remove(requestID);
                }
            }
        }

        /// <summary>
        /// 等待消息
        /// </summary>
        /// <param name="requestID">请求ID</param>
        /// <param name="timeout">超时</param>
        /// <returns>状态</returns>
        public virtual int waitMessage(int requestID, int timeout) {
            int state = 0;
            while (timeout > 0) {
                lock (m_waitMessages) {
                    if (m_waitMessages.containsKey(requestID)) {
                        if (m_waitMessages.get(requestID).m_bodyLength > 0) {
                            state = 1;
                            break;
                        }
                    } else {
                        break;
                    }
                }
                timeout -= 10;
                Thread.Sleep(10);
            }
            unRegisterWait(requestID);
            return state;
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="socketID">连接ID</param>
        /// <param name="localSID">本地连接ID</param>
        /// <param name="state">状态</param>
        /// <param name="log">日志</param>
        public static void writeClientLog(int socketID, int localSID, int state, String log) {
            if (state == 2 || state == 3) {
                foreach (FCClientService service in m_services.Values) {
                    service.onClientClose(socketID, localSID);
                }
            } else if (state == 1) {
                foreach (FCClientService service in m_services.Values) {
                    service.onClientConnect(socketID, localSID);
                }
            }
        }
    }

    /// <summary>
    /// 客户端数据监听
    /// </summary>
    public class FCClientSocketListener : FCSocketListener {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FCClientSocketListener() {
        }

        /// <summary>
        /// 数据回调
        /// </summary>
        /// <param name="socketID"></param>
        /// <param name="localSID"></param>
        /// <param name="str"></param>
        /// <param name="len"></param>
        public void callBack(int socketID, int localSID, byte[] str, int len) {
            FCClientService.callBack(socketID, localSID, str, len);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="socketID"></param>
        /// <param name="localSID"></param>
        /// <param name="state"></param>
        /// <param name="log"></param>
        public void writeLog(int socketID, int localSID, int state, String log) {
            FCClientService.writeClientLog(socketID, localSID, state, log);
        }
    }
}