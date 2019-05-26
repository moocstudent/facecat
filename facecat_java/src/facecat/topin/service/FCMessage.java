/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.service;

import java.net.*;

/**
 * 消息结构
 *
 */
public class FCMessage {

    /**
     * 创建消息
     *
     */
    public FCMessage() {
    }

    /**
     * 创建消息
     *
     * @param groupID 组ID
     * @param serviceID 服务ID
     * @param functionID 功能ID
     * @param sessionID 登录ID
     * @param requestID 请求ID
     * @param socketID 连接ID
     * @param state 状态
     * @param compressType 压缩类型
     * @param bodyLength 包体长度
     * @param body 包体
     */
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

    /**
     * 组ID
     *
     */
    public int m_groupID;

    /**
     * 服务ID
     *
     */
    public int m_serviceID;

    /**
     * 功能ID
     *
     */
    public int m_functionID;

    /**
     * 登录ID
     *
     */
    public int m_sessionID;

    /**
     * 请求ID
     *
     */
    public int m_requestID;

    /**
     * 连接ID
     *
     */
    public int m_socketID;

    /**
     * 状态
     *
     */
    public int m_state;

    /**
     * 压缩类型
     *
     */
    public int m_compressType;

    /**
     * 包体长度
     *
     */
    public int m_bodyLength;

    /**
     * 包体
     *
     */
    public byte[] m_body;

    /**
     * 复制数据
     *
     * @param message 消息
     */
    public final void copy(FCMessage message) {
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
