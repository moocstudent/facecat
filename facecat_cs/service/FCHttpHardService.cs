/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using FaceCat;
using System.IO.Compression;

namespace FaceCat {
    /// <summary>
    /// HTTP服务
    /// </summary>
    public class FCHttpHardService : FCServerService {
        /// <summary>
        /// 创建HTTP服务
        /// </summary>
        public FCHttpHardService() {
            ServiceID = SERVICEID_HTTPHARD;
        }

        /// <summary>
        /// 用户会话服务ID
        /// </summary>
        public const int SERVICEID_HTTPHARD = 20;

        /// <summary>
        /// POST方法ID
        /// </summary>
        public const int FUNCTIONID_HTTPHARD_TEST = 0;

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="message">消息</param>
        public override void onReceive(FCMessage message) {
            base.onReceive(message);
            sendToListener(message);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息</param>
        public override int send(FCMessage message) {
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
            lock (FCHttpMonitor.MainMonitor.m_httpDatas) {
                FCHttpMonitor.MainMonitor.m_httpDatas.get(message.m_socketID).m_resBytes = bytes;
            }
            int ret = bytes.Length;
            UpFlow += ret;
            bw.close();
            onClientClose(message.m_socketID, 0);
            return ret;
        }
    }
}
