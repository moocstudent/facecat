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
using System.Text;
using System.Net;
using System.IO.Compression;
using OwLib;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace FaceCat
{
    /// <summary>
    /// HTTP的POST服务
    /// </summary>
    public class FCHttpPostService : FCClientService
    {
        /// <summary>
        /// 创建HTTP服务
        /// </summary>
        public FCHttpPostService()
        {
        }

        private bool m_isSyncSend;
        /// <summary>
        /// 获取或者设置是否同步发送
        /// </summary>
        public bool IsSyncSend
        {
            get { return m_isSyncSend; }
            set { m_isSyncSend = value; }
        }

        private int m_timeout = 10;
        /// <summary>
        /// 获取或者设置Timeout时间
        /// </summary>
        public int Timeout
        {
            get { return m_timeout; }
            set { m_timeout = value; }
        }

        private string m_url;

        /// <summary>
        /// 获取或设置地址
        /// </summary>
        public string Url
        {
            get { return m_url; }
            set { m_url = value; }
        }

        /// <summary>
        /// 异步发送数据
        /// </summary>
        /// <param name="obj"></param>
        public void asynSend(Object obj)
        {
            FCMessage message = obj as FCMessage;
            if (message == null)
            {
                return;
            }
            sendRequest(message);
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="message">消息</param>
        public override void onReceive(FCMessage message)
        {
            base.onReceive(message);
            sendToListener(message);
        }

        /// <summary>
        /// 发送POST数据
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="data">数据</param>
        /// <returns>结果</returns>
        public String post(String url)
        {
            return post(url, "");
        }

        /// <summary>
        /// 发送POST数据
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="data">数据</param>
        /// <returns>结果</returns>
        public String post(String url, String data)
        {
            byte[] sendDatas = Encoding.Default.GetBytes(data);
            byte[] bytes = post(url, sendDatas);
            if (bytes != null)
            {
                return Encoding.Default.GetString(bytes);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 发送POST数据
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="data">数据</param>
        /// <returns>结果</returns>
        public byte[] post(String url, byte[] sendDatas)
        {
            HttpWebRequest request = null;
            Stream reader = null;
            HttpWebResponse response = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                //request.Timeout = m_timeout;
                request.ContentType = "application/x-www-form-urlencoded";
                if (sendDatas != null)
                {
                    request.ContentLength = sendDatas.Length;
                    Stream writer = request.GetRequestStream();
                    writer.Write(sendDatas, 0, sendDatas.Length);
                    writer.Close();
                }
                response = (HttpWebResponse)request.GetResponse();
                reader = response.GetResponseStream();
                long contentLength = response.ContentLength;
                byte[] recvDatas = new byte[contentLength];
                for (int i = 0; i < contentLength; i++)
                {
                    recvDatas[i] = (byte)reader.ReadByte();
                }
                return recvDatas;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        /// <summary>
        /// 发送POST数据
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns>返回消息</returns>
        public override int send(FCMessage message)
        {
            if (!m_isSyncSend)
            {
                Thread thread = new Thread(asynSend);
                thread.Start(message);
                return 1;
            }
            else
            {
                return sendRequest(message);
            }
        }

        /// <summary>
        /// 发送POST数据
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns>返回消息</returns>
        public int sendRequest(FCMessage message)
        {
            FCBinary bw = new FCBinary();
            byte[] body = message.m_body;
            int bodyLength = message.m_bodyLength;
            int uncBodyLength = bodyLength;
            if (message.m_compressType == COMPRESSTYPE_GZIP)
            {
                using (MemoryStream cms = new MemoryStream())
                {
                    using (GZipStream gzip = new GZipStream(cms, CompressionMode.Compress))
                    {
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
            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(m_url);
            webReq.Method = "POST";
            webReq.ContentType = "application/x-www-form-urlencoded";
            webReq.ContentLength = bytes.Length;
            if (bytes != null)
            {
                Stream writer = webReq.GetRequestStream();
                writer.Write(bytes, 0, bytes.Length);
                writer.Close();
            }
            HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
            Stream reader = response.GetResponseStream();
            long contentLength = response.ContentLength;
            byte[] dataArray = new byte[contentLength];
            for (int i = 0; i < contentLength; i++)
            {
                dataArray[i] = (byte)reader.ReadByte();
            }
            response.Close();
            reader.Dispose();
            bw.close();
            int ret = dataArray.Length;
            UpFlow += ret;
            FCClientService.callBack(message.m_socketID, 0, dataArray, ret);
            return ret;
        }
    }
}
