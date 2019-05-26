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
import java.util.*;
import facecat.topin.chart.*;

/*
 HTTP流服务
 */
public class FCHttpHardService extends FCServerService {
    /*
     创建服务
     */

    public FCHttpHardService() {
        setServiceID(SERVICEID_HTTPPOST);
    }

    public static final int SERVICEID_HTTPPOST = 20;

    public static final int FUNCTIONID_HTTPPOST_TEST = 0;

    @Override
    public void onReceive(FCMessage message) {
        super.onReceive(message);
        sendToListener(message);
    }

    @Override
    public int send(FCMessage message) {
        int ret = -1;
        try {
            FCBinary bw = new FCBinary();
            byte[] body = message.m_body;
            int bodyLength = message.m_bodyLength;
            int uncBodyLength = bodyLength;
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
            synchronized (FCHttpMonitor.getMainMonitor().m_httpDatas) {
                FCHttpMonitor.getMainMonitor().m_httpDatas.get(message.m_socketID).m_resBytes = bytes;
            }
            ret = bytes.length;
            bw.close();
        } catch (Exception ex) {

        }
        return ret;
    }
}
