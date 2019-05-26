/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.service;

import facecat.topin.core.*;
import java.io.*;
import java.util.*;
import facecat.topin.sock.*;

/**
 * 基础服务类
 *
 */
public class FCServerService {

    /**
     * 创建服务
     *
     */
    public FCServerService() {
    }

    /**
     * 析构函数
     *
     */
    protected void finalize() throws Throwable {
        synchronized (m_listeners) {
            m_listeners.clear();
        }
        synchronized (m_waitMessages) {
            m_waitMessages.clear();
        }
    }

    /**
     * 无压缩
     *
     */
    public static int COMPRESSTYPE_NONE = 0;

    /**
     * GZIP压缩
     *
     */
    public static int COMPRESSTYPE_GZIP = 1;

    /**
     * 压缩类型
     */
    protected HashMap<Integer, Integer> m_compressTypes = new HashMap<Integer, Integer>();

    /**
     * 监听者集合
     *
     */
    private java.util.HashMap<Integer, FCMessageListener> m_listeners = new java.util.HashMap<Integer, FCMessageListener>();

    /**
     * 请求ID
     *
     */
    private static int m_requestID = 10000;

    /**
     * 所有的服务
     *
     */
    private static java.util.HashMap<Integer, FCServerService> m_services = new java.util.HashMap<Integer, FCServerService>();

    /**
     * 等待消息队列
     *
     */
    private java.util.HashMap<Integer, FCMessage> m_waitMessages = new java.util.HashMap<Integer, FCMessage>();

    private int m_compressType = COMPRESSTYPE_GZIP;

    /**
     * 获取或设置压缩类型
     *
     */
    public final int getCompressType() {
        return m_compressType;
    }

    public final void setCompressType(int value) {
        m_compressType = value;
    }

    protected static long m_downFlow;

    /**
     * 获取或设置下载流量
     *
     */
    public static long getDownFlow() {
        return FCServerService.m_downFlow;
    }

    public static void setDownFlow(long value) {
        FCServerService.m_downFlow = value;
    }

    private int m_groupID = 0;

    /**
     * 获取或设置组ID
     *
     */
    public final int getGroupID() {
        return m_groupID;
    }

    public final void setGroupID(int value) {
        m_groupID = value;
    }

    private boolean m_isDeleted = false;

    /**
     * 获取对象是否已被销毁
     *
     */
    public final boolean isDeleted() {
        return m_isDeleted;
    }

    private int m_serviceID = 0;

    /**
     * 获取或设置服务的ID
     *
     */
    public final int getServiceID() {
        return m_serviceID;
    }

    public final void setServiceID(int value) {
        m_serviceID = value;
    }

    private int m_sessionID = 0;

    /**
     * 获取或设置登录ID
     *
     */
    public final int getSessionID() {
        return m_sessionID;
    }

    public final void detSessionID(int value) {
        m_sessionID = value;
    }

    private int m_socketID = 0;

    public final int getSocketID() {
        return m_socketID;
    }

    public final void setSocketID(int socketID) {
        m_socketID = socketID;
    }

    protected static long m_upFlow;

    /**
     * 获取或设置上传流量
     *
     */
    public static long getUpFlow() {
        return m_upFlow;
    }

    public static void getUpFlow(long value) {
        m_upFlow = value;
    }

    /**
     * 关闭
     *
     * @param socketID 连接ID
     * @return 状态
     */
    public static int closeServer(int socketID) {
        return FCServerSockets.close(socketID);
    }

    /**
     * 发送消息
     *
     * @param socketID 连接ID
     * @param str 数据
     * @param len 长度
     * @return 状态
     */
    public static int sendByServer(int socketID, int localSID, byte[] str, int len) {
        return FCServerSockets.send(socketID, localSID, str, len);
    }

    /**
     * 启动服务
     *
     * @param port 端口号
     * @return 状态
     */
    public static int startServer(int port) {
        FCServerSockets.setListener(new FCServerSocketListener());
        return FCServerSockets.start(0, port);
    }

    /**
     * 添加服务
     *
     * @param service 服务
     */
    public static void addService(FCServerService service) {
        m_services.put(service.getServiceID(), service);
    }

    /**
     * 回调函数
     *
     * @param socketID 连接ID
     * @param localSID 本地连接ID
     * @param str 数据
     * @param len 长度
     */
    public static void callBack(int socketID, int localSID, byte[] str, int len) {
        m_downFlow += len;
        try {
            if (len > 4) {
                FCBinary br = new FCBinary();
                br.write(str, len);
                int head = br.readInt();
                int groupID = br.readShort();
                int serviceID = br.readShort();
                FCServerService service = null;
                if (m_services.containsKey(serviceID)) {
                    m_services.get(serviceID).onCallBack(br, socketID, localSID, len);
                }
                br.close();
            }
        } catch (Exception ex) {
            System.out.println(ex.getMessage() + "\r\n" + ex.getStackTrace());
        }
    }

    /**
     * 销毁对象
     *
     */
    public void delete() {
        if (!m_isDeleted) {
            m_listeners.clear();
            m_isDeleted = true;
        }
    }

    /**
     * 获取请求ID
     *
     * @return 请求ID
     */
    public static int getRequestID() {
        return m_requestID++;
    }

    /**
     * 获取所有的服务
     *
     * @param services 服务列表
     */
    public static void getServices(ArrayList<FCServerService> services) {
        for (FCServerService service : m_services.values()) {
            services.add(service);
        }
    }

    /**
     * 保持活跃
     *
     * @param socketID 套接字ID
     * @return 状态
     */
    public int keepAlive(int socketID) {
        int ret = -1;
        try {
            FCBinary bw = new FCBinary();
            bw.writeInt((int) 4);
            ret = sendByServer(socketID, m_socketID, bw.getBytes(), 4);
            bw.close();
        } catch (Exception ex) {

        }
        return ret;
    }

    /**
     * 收到消息
     *
     * @param br 流
     * @param socketID 套接字ID
     * @param localSID 本地套接字ID
     * @param len 长度
     */
    public void onCallBack(FCBinary br, int socketID, int localSID, int len) {
        try {
            int headSize = 4 * 4 + 2 * 3 + 1 * 2;
            int functionID = br.readShort();
            int sessionID = br.readInt();
            int requestID = br.readInt();
            int state = (int) br.readChar();
            int compressType = (int) br.readChar();
            int bodyLength = br.readInt();
            byte[] body = new byte[len - headSize];
            br.readBytes(body);
            FCMessage message = null;
            byte[] unzipBody = null;
            if (compressType == COMPRESSTYPE_GZIP) {
                //unzipBody = CStrA.UnGZip(body);
                bodyLength = unzipBody.length;
                message = new FCMessage(getGroupID(), getServiceID(), functionID, sessionID, requestID, socketID, state,
                        compressType, bodyLength, unzipBody);
                // TODO...
            } else {
                message = new FCMessage(getGroupID(), getServiceID(), functionID, sessionID, requestID, socketID, state,
                        compressType, bodyLength, body);
            }

            onReceive(message);
            onWaitMessageHandle(message);
            body = null;
            unzipBody = null;
        } catch (Exception ex) {

        }
    }

    /**
     * 客户端退出方法
     *
     */
    public void onClientClose(int socketID, int localSID) {
        if (m_compressTypes.containsKey(socketID)) {
            m_compressTypes.remove(socketID);
        }
    }

    /**
     * 接收数据
     *
     * @param message 消息
     */
    public void onReceive(FCMessage message) {
        synchronized (m_compressTypes) {
            m_compressTypes.put(message.m_socketID, message.m_compressType);
        }
    }

    /**
     * 等待消息的处理
     *
     * @param message 消息
     */
    public void onWaitMessageHandle(FCMessage message) {
        if (m_waitMessages.size() > 0) {
            synchronized (m_waitMessages) {
                if (m_waitMessages.containsKey(message.m_requestID)) {
                    FCMessage waitMessage = m_waitMessages.get(message.m_requestID);
                    waitMessage.copy(message);
                }
            }
        }
    }

    /**
     * 注册数据监听
     *
     * @param requestID 请求ID
     * @param callBack 回调函数
     */
    public void registerListener(int requestID, FCListenerMessageCallBack callBack) {
        synchronized (m_listeners) {
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

    /**
     * 注册等待
     *
     * @param requestID 请求ID
     * @param message 消息
     */
    public void registerWait(int requestID, FCMessage message) {
        synchronized (m_waitMessages) {
            m_waitMessages.put(requestID, message);
        }
    }

    /**
     * 发送消息
     *
     * @param message 消息
     */
    public int send(FCMessage message) {
        int ret = -1;
        try {
            FCBinary bw = new FCBinary();
            byte[] body = message.m_body;
            int bodyLength = message.m_bodyLength;
            int uncBodyLength = bodyLength;
            synchronized (m_compressTypes) {
                if (m_compressTypes.containsKey(message.m_socketID)) {
                    message.m_compressType = m_compressTypes.get(message.m_socketID);
                }
            }
            byte[] zipBody = null;
            if (message.m_compressType == COMPRESSTYPE_GZIP) {
                zipBody = FCStr.gZip(body);
                bodyLength = zipBody.length;
            }
            int len = 4 * 4 + bodyLength + 2 * 3 + 1 * 2;
            bw.writeInt(len);
            bw.writeShort((short) message.m_groupID);
            bw.writeShort((short) message.m_serviceID);
            bw.writeShort((short) message.m_functionID);
            bw.writeInt(message.m_sessionID);
            bw.writeInt(message.m_requestID);
            bw.writeChar((char) message.m_state);
            bw.writeChar((char) message.m_compressType);
            bw.writeInt(uncBodyLength);
            if (message.m_compressType == COMPRESSTYPE_GZIP) {
                bw.writeBytes(zipBody);
            } else {
                bw.writeBytes(body);
            }
            byte[] bytes = bw.getBytes();
            ret = sendByServer(message.m_socketID, m_socketID, bytes, bytes.length);
            bw.close();
        } catch (Exception ex) {

        }
        return ret;
    }

    /**
     * 发送到监听者
     *
     * @param message 消息
     */
    public void sendToListener(FCMessage message) {
        FCMessageListener listener = null;
        synchronized (m_listeners) {
            if (m_listeners.containsKey(message.m_requestID)) {
                listener = m_listeners.get(message.m_requestID);
            }
        }
        if (listener != null) {
            listener.callBack(message);
        }
    }

    /**
     * 取消注册数据监听
     *
     * @param requestID 请求ID
     */
    public void unRegisterListener(int requestID) {
        synchronized (m_listeners) {
            m_listeners.remove(requestID);
        }
    }

    /**
     * 取消注册监听
     *
     * @param requestID 请求ID
     * @param callBack 回调函数
     */
    public void unRegisterListener(int requestID, FCListenerMessageCallBack callBack) {
        synchronized (m_listeners) {
            if (m_listeners.containsKey(requestID)) {
                m_listeners.get(requestID).remove(callBack);
            }
        }
    }

    /**
     * 取消注册等待
     *
     * @param requestID 请求ID
     */
    public void unRegisterWait(int requestID) {
        synchronized (m_waitMessages) {
            if (m_waitMessages.containsKey(requestID)) {
                m_waitMessages.remove(requestID);
            }
        }
    }

    /**
     * 等待消息
     *
     * @param requestID 请求ID
     * @param timeout 超时
     * @return 状态
     */
    public int waitMessage(int requestID, int timeout) {
        int state = 0;
        try {
            while (timeout > 0) {
                synchronized (m_waitMessages) {
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
                Thread.sleep(10);
            }
            unRegisterWait(requestID);
        } catch (Exception ex) {
        }
        return state;
    }

    /**
     * 等待消息
     *
     * @param socketID 目标SocketID
     * @param localSID 本地SocketID
     * @param state 状态
     * @param log 日志
     * @return 状态
     */
    public static void writeLog(int socketID, int localSID, int state, String log) {
        if (state == 2) {
            for (FCServerService service : m_services.values()) {
                service.onClientClose(socketID, localSID);
            }
        }
    }
}
