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
using System.Text;
using System.Net;
using System.IO;
using System.IO.Compression;
using OwLib;

namespace FaceCat {
    /// <summary>
    /// HTTP的GET服务
    /// </summary>
    public class FCHttpGetService {
        /// <summary>
        /// 创建HTTP服务
        /// </summary>
        public FCHttpGetService() {
        }

        /// <summary>
        /// 获取网页数据
        /// </summary>
        /// <param name="url">地址</param>
        /// <returns>页面源码</returns>
        public static String get(String url) {
            String content = "";
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader streamReader = null;
            Stream resStream = null;
            try {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.KeepAlive = false;
                request.Timeout = 10000;
                ServicePointManager.DefaultConnectionLimit = 50;
                response = (HttpWebResponse)request.GetResponse();
                resStream = response.GetResponseStream();
                streamReader = new StreamReader(resStream, Encoding.Default);
                content = streamReader.ReadToEnd();
            }
            catch (Exception ex) {
            }
            finally {
                if (response != null) {
                    response.Close();
                }
                if (resStream != null) {
                    resStream.Close();
                }
                if (streamReader != null) {
                    streamReader.Close();
                }
            }
            return content;
        }
    }
}
