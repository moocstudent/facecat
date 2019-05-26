/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
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
using System.Diagnostics;

namespace FaceCat {
    /// <summary>
    /// 消息回调委托
    /// </summary>
    /// <param name="socketID">连接ID</param>
    /// <param name="localSID">本地连接ID</param>
    /// <param name="str">数据</param>
    /// <param name="len">长度</param>
    public delegate void MessageCallBack(int socketID, int localSID, IntPtr str, int len);

    /// <summary>
    /// 监听消息
    /// </summary>
    /// <param name="message">消息</param>
    public delegate void ListenerMessageCallBack(FCMessage message);

    /// <summary>
    /// 日志报告委托
    /// </summary>
    /// <param name="socketID">连接ID</param>
    /// <param name="localSID">本地连接ID</param>
    /// <param name="state">状态</param>
    /// <param name="log">日志</param>
    public delegate void WriteLogCallBack(int socketID, int localSID, int state, String log);

    /// <summary>
    /// 消息结构
    /// </summary>
    public class FCMessage {
        /// <summary>
        /// 创建消息
        /// </summary>
        public FCMessage() {
        }

        /// <summary>
        /// 创建消息
        /// </summary>
        /// <param name="groupID">组ID</param>
        /// <param name="serviceID">服务ID</param>
        /// <param name="functionID">功能ID</param>
        /// <param name="sessionID">登录ID</param>
        /// <param name="requestID">请求ID</param>
        /// <param name="socketID">连接ID</param>
        /// <param name="state">状态</param>
        /// <param name="compressType">压缩类型</param>
        /// <param name="bodyLength">包体长度</param>
        /// <param name="body">包体</param>
        public FCMessage(int groupID, int serviceID, int functionID, int sessionID, int requestID, int socketID, int state, int compressType, int bodyLength, byte[] body) {
            m_groupID = groupID;
            m_serviceID = serviceID;
            m_functionID = functionID;
            m_sessionID = sessionID;
            m_requestID = requestID;
            m_socketID = socketID;
            m_state = state;
            m_compressType = compressType;
            m_bodyLength = bodyLength;
            m_body = body;
        }

        /// <summary>
        /// 组ID
        /// </summary>
        public int m_groupID;

        /// <summary>
        /// 服务ID
        /// </summary>
        public int m_serviceID;

        /// <summary>
        /// 功能ID
        /// </summary>
        public int m_functionID;

        /// <summary>
        /// 登录ID
        /// </summary>
        public int m_sessionID;

        /// <summary>
        /// 请求ID
        /// </summary>
        public int m_requestID;

        /// <summary>
        /// 连接ID
        /// </summary>
        public int m_socketID;

        /// <summary>
        /// 状态
        /// </summary>
        public int m_state;

        /// <summary>
        /// 压缩类型
        /// </summary>
        public int m_compressType;

        /// <summary>
        /// 包体长度
        /// </summary>
        public int m_bodyLength;

        /// <summary>
        /// 包体
        /// </summary>
        public byte[] m_body;

        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="message">消息</param>
        public void copy(FCMessage message) {
            m_groupID = message.m_groupID;
            m_serviceID = message.m_serviceID;
            m_functionID = message.m_functionID;
            m_sessionID = message.m_sessionID;
            m_requestID = message.m_requestID;
            m_socketID = message.m_socketID;
            m_state = message.m_state;
            m_compressType = message.m_compressType;
            m_bodyLength = message.m_bodyLength;
            m_body = message.m_body;
        }
    }

    /// <summary>
    /// 消息监听
    /// </summary>
    public class FCMessageListener {
        /// <summary>
        /// 创建消息监听
        /// </summary>
        public FCMessageListener() {
        }

        /// <summary>
        /// 析构方法
        /// </summary>
        ~FCMessageListener() {
            clear();
        }

        /// <summary>
        /// 监听回调列表
        /// </summary>
        private ArrayList<ListenerMessageCallBack> m_callBacks = new ArrayList<ListenerMessageCallBack>();

        /// <summary>
        /// 添加回调
        /// </summary>
        /// <param name="callBack">回调</param>
        public void add(ListenerMessageCallBack callBack) {
            m_callBacks.Add(callBack);
        }

        /// <summary>
        /// 回调方法
        /// </summary>
        public void callBack(FCMessage message) {
            int callBackSize = m_callBacks.Count;
            for (int i = 0; i < callBackSize; i++) {
                m_callBacks.get(i)(message);
            }
        }

        /// <summary>
        /// 清除监听
        /// </summary>
        public void clear() {
            m_callBacks.Clear();
        }

        /// <summary>
        /// 移除回调
        /// </summary>
        /// <param name="callBack">回调</param>
        public void remove(ListenerMessageCallBack callBack) {
            m_callBacks.Remove(callBack);
        }
    }

    /// <summary>
    /// 套接字连接组信息
    /// </summary>
    public class FCSocketArray {
        /// <summary>
        /// 套接字ID组
        /// </summary>
        private ArrayList<int> m_sockets = new ArrayList<int>();

        /// <summary>
        /// 添加套接字ID
        /// </summary>
        /// <param name="socketID">套接字ID</param>
        public void addSocket(int socketID) {
            int socketsSize = m_sockets.Count;
            for (int i = 0; i < socketsSize; i++) {
                if (m_sockets.get(i) == socketID) {
                    return;
                }
            }
            m_sockets.Add(socketID);
        }

        /// <summary>
        /// 获取套接字列表
        /// </summary>
        /// <param name="socketList">套接字列表</param>
        public void getSocketList(ArrayList<int> socketList) {
            int socketsSize = m_sockets.Count;
            for (int i = 0; i < socketsSize; i++) {
                socketList.add(m_sockets.get(i));
            }
        }

        /// <summary>
        /// 移除套接字ID
        /// </summary>
        /// <param name="socketID">套接字ID</param>
        public void removeSocket(int socketID) {
            m_sockets.Remove(socketID);
        }
    }

    /// <summary>
    /// 基础服务类
    /// </summary>
    public class FCServerService {
        /// <summary>
        /// 创建服务
        /// </summary>
        public FCServerService() {
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~FCServerService() {
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
        /// 压缩类型
        /// </summary>
        protected HashMap<int, int> m_compressTypes = new HashMap<int, int>();

        /// <summary>
        /// 监听者集合
        /// </summary>
        private HashMap<int, FCMessageListener> m_listeners = new HashMap<int, FCMessageListener>();

        /// <summary>
        /// 消息回调
        /// </summary>
        private static MessageCallBack m_messageCallBack;

        /// <summary>
        /// 需要等待的请求ID
        /// </summary>
        private HashMap<int, int> m_needWaitRequestIDs = new HashMap<int, int>();

        /// <summary>
        /// 请求
        /// </summary>
        private static int m_requestID = 10000;

        /// <summary>
        /// 所有的服务
        /// </summary>
        private static HashMap<int, FCServerService> m_services = new HashMap<int, FCServerService>();

        /// <summary>
        /// 等待消息队列
        /// </summary>
        private HashMap<int, FCMessage> m_waitMessages = new HashMap<int, FCMessage>();

        /// <summary>
        /// 写日志回调
        /// </summary>
        private static WriteLogCallBack m_writeLogCallBack;

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
            get { return FCServerService.m_downFlow; }
            set { FCServerService.m_downFlow = value; }
        }

        private int m_groupID = 0;

        /// <summary>
        /// 获取或设置组ID
        /// </summary>
        public int GroupID {
            get { return m_groupID; }
            set { m_groupID = value; }
        }

        private int m_serviceID = 0;

        /// <summary>
        /// 获取或设置服务的ID
        /// </summary>
        public int ServiceID {
            get { return m_serviceID; }
            set { m_serviceID = value; }
        }

        private bool m_isDisposed = false;

        /// <summary>
        /// 获取对象是否已被销毁
        /// </summary>
        public bool IsDisposed {
            get { return m_isDisposed; }
        }

        private int m_sessionID = 0;

        /// <summary>
        /// 获取或设置登录ID
        /// </summary>
        public int SessionID {
            get { return m_sessionID; }
            set { m_sessionID = value; }
        }

        private int m_socketID;

        /// <summary>
        /// 获取或设置套接字ID
        /// </summary>
        public int SocketID {
            get { return m_socketID; }
            set { m_socketID = value; }
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
        /// 关闭服务
        /// </summary>
        /// <param name="socketID">套接字ID</param>
        /// <returns>状态</returns>
        public static int closeServer(int socketID) {
            return FCServerSockets.close(socketID);
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="socketID">服务器套接字ID</param>
        /// <param name="localSID">客户端套接字ID</param>
        /// <param name="str">数据</param>
        /// <param name="len">长度</param>
        /// <returns>状态</returns>
        public static int sendByServer(int socketID, int localSID, byte[] str, int len) {
            return FCServerSockets.send(socketID, localSID, str, len);
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="port">端口</param>
        /// <returns>状态</returns>
        public static int startServer(int port) {
            FCServerSockets.setListener(new FCServerSocketListener());
            return FCServerSockets.start(0, port);
        }

        /// <summary>
        /// 添加服务
        /// </summary>
        /// <param name="service">服务</param>
        public static void AddService(FCServerService service) {
            m_services.put(service.ServiceID, service);
        }

        /// <summary>
        /// 回调函数
        /// </summary>
        /// <param name="socketID">连接ID</param>
        /// <param name="localSID">本地连接ID</param>
        /// <param name="bytes">数据</param>
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
            if (!m_isDisposed) {
                m_listeners.Clear();
                m_isDisposed = true;
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
        public static void getServices(ArrayList<FCServerService> services) {
            foreach (FCServerService service in m_services.Values) {
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
            int ret = FCServerSockets.send(m_socketID, socketID, bytes, length);
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
            message.m_body = null;
            body = null;
        }

        /// <summary>
        /// 客户端关闭方法
        /// </summary>
        /// <param name="socketID">连接ID</param>
        /// <param name="localSID">本地连接ID</param>
        public virtual void onClientClose(int socketID, int localSID) {
            if (m_compressTypes.ContainsKey(socketID)) {
                m_compressTypes.Remove(socketID);
            }
        }

        /// <summary>
        /// 客户端连接方法
        /// </summary>
        /// <param name="socketID">连接ID</param>
        /// <param name="localSID">本地连接ID</param>
        public virtual void onClientConnect(int socketID, int localSID, String ip) {
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="message">消息</param>
        public virtual void onReceive(FCMessage message) {
            lock (m_compressTypes) {
                m_compressTypes.put(message.m_socketID, message.m_compressType);
            }
        }

        /// <summary>
        /// 等待消息的处理
        /// </summary>
        /// <param name="message">消息</param>
        public virtual void onWaitMessageHandle(FCMessage message) {
            if (m_waitMessages.Count > 0) {
                lock (m_waitMessages) {
                    if (m_waitMessages.containsKey(message.m_requestID)) {
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
                if (!m_listeners.ContainsKey(requestID)) {
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
            lock (m_compressTypes) {
                if (m_compressTypes.ContainsKey(message.m_socketID)) {
                    message.m_compressType = m_compressTypes.get(message.m_socketID);
                }
            }
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
            int ret = FCServerSockets.send(m_socketID, message.m_socketID, bytes, length);
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
        public virtual void UnRegisterListener(int requestID) {
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
                if (m_listeners.containsKey(requestID)) {
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
        public static void writeServerLog(int socketID, int localSID, int state, String log) {
            if (state == 2 || state == 3) {
                foreach (FCServerService service in m_services.Values) {
                    service.onClientClose(socketID, localSID);
                }
            } else if (state == 1) {
                foreach (FCServerService service in m_services.Values) {
                    service.onClientConnect(socketID, localSID, log);
                }
            }
        }
    }

    /// <summary>
    /// 服务端数据监听
    /// </summary>
    public class FCServerSocketListener : FCSocketListener {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FCServerSocketListener() {

        }

        /// <summary>
        /// 消息回调
        /// </summary>
        /// <param name="socketID"></param>
        /// <param name="localSID"></param>
        /// <param name="str"></param>
        /// <param name="len"></param>
        public void callBack(int socketID, int localSID, byte[] str, int len) {
            FCServerService.callBack(socketID, localSID, str, len);
        }

        /// <summary>
        /// 日志回调
        /// </summary>
        /// <param name="socketID"></param>
        /// <param name="localSID"></param>
        /// <param name="state"></param>
        /// <param name="log"></param>
        public void writeLog(int socketID, int localSID, int state, String log) {
            FCServerService.writeServerLog(socketID, localSID, state, log);
        }
    }
}