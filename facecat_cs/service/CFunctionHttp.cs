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
using System.Threading;
using System.IO;
using System.Net;
using System.Web;
using FaceCat;
using System.Runtime.InteropServices;

namespace OwLib {
    /// <summary>
    /// HTTP相关的库
    /// </summary>
    public class CFunctionHttp : CFunction {
        /// <summary>
        /// 创建方法
        /// </summary>
        /// <param name="indicator">指标</param>
        /// <param name="id">ID</param>
        /// <param name="name">名称</param>
        public CFunctionHttp(FCScript indicator, int id, String name) {
            m_indicator = indicator;
            m_ID = id;
            m_name = name;
        }

        /// <summary>
        /// HTTP对象
        /// </summary>
        public FCHttpData m_data;

        /// <summary>
        /// 指标
        /// </summary>
        public FCScript m_indicator;

        /// <summary>
        /// 方法
        /// </summary>
        private static string FUNCTIONS =
            "POSTSTRING,QUERYSTRING,WRITE,ADDPORT,GETREQUESTURL,GETREQUESTMETHOD"
            + ",GETCONTENTTYPE,SETSTATUSCODE,GETSERVICENAME,CHECKSCRIPT,CLOSE,HARDREQUEST,EASYREQUEST,GETREMOTEIP,GETREMOTEPORT";

        /// <summary>
        /// 前缀
        /// </summary>
        private static string PREFIX = "HTTP.";

        /// <summary>
        /// 开始索引
        /// </summary>
        private const int STARTINDEX = 4000;

        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>结果</returns>
        public override double onCalculate(CVariable var) {
            switch (var.m_functionID) {
                case STARTINDEX:
                    return HTTP_POSTSTRING(var);
                case STARTINDEX + 1:
                    return HTTP_QUERYSTRING(var);
                case STARTINDEX + 2:
                    return HTTP_WRITE(var);
                case STARTINDEX + 3:
                    return HTTP_ADDPORT(var);
                case STARTINDEX + 4:
                    return HTTP_GETREQUESTURL(var);
                case STARTINDEX + 5:
                    return HTTP_GETREQUESTMETHOD(var);
                case STARTINDEX + 6:
                    return HTTP_GETCONTENTTYPE(var);
                case STARTINDEX + 7:
                    return HTTP_SETSTATUSCODE(var);
                case STARTINDEX + 8:
                    return HTTP_GETSERVICENAME(var);
                case STARTINDEX + 9:
                    return HTTP_CHECKSCRIPT(var);
                case STARTINDEX + 10:
                    return HTTP_CLOSE(var);
                case STARTINDEX + 11:
                    return HTTP_HARDREQUEST(var);
                case STARTINDEX + 12:
                    return HTTP_EASYREQUEST(var);
                case STARTINDEX + 13:
                    return HTTP_GETREMOTEIP(var);
                case STARTINDEX + 14:
                    return HTTP_GETREMOTEPORT(var);
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 添加方法
        /// </summary>
        /// <param name="indicator">方法库</param>
        /// <returns>指标</returns>
        public static void addFunctions(FCScript indicator) {
            string[] functions = FUNCTIONS.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            int functionsSize = functions.Length;
            for (int i = 0; i < functionsSize; i++) {
                indicator.addFunction(new CFunctionHttp(indicator, STARTINDEX + i, PREFIX + functions[i]));
            }
        }

        /// <summary>
        /// 添加前缀
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double HTTP_ADDPORT(CVariable var) {
            FCHttpMonitor.MainMonitor.Port = (int)m_indicator.getValue(var.m_parameters[0]);
            return 0;
        }

        /// <summary>
        /// 检查脚本
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double HTTP_CHECKSCRIPT(CVariable var) {
            FCHttpMonitor.MainMonitor.checkScript();
            return 0;
        }

        /// <summary>
        /// HTTP关闭
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double HTTP_CLOSE(CVariable var) {
            m_data.m_close = true;
            return 0;
        }

        /// <summary>
        /// 接受GET请求
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double HTTP_EASYREQUEST(CVariable var) {
            return FCHttpMonitor.m_easyServices.get(m_indicator.getText(var.m_parameters[0])).onReceive(m_data);
        }

        /// <summary>
        /// 获取内容类型方法
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double HTTP_GETCONTENTTYPE(CVariable var) {
            CVariable newVar = new CVariable(m_indicator);
            newVar.m_expression = "'" + m_data.m_contentType + "'";
            m_indicator.setVariable(var.m_parameters[0], newVar);
            return 0;
        }

        /// <summary>
        /// 获取IP
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double HTTP_GETREMOTEIP(CVariable var) {
            CVariable newVar = new CVariable(m_indicator);
            newVar.m_expression = "'" + m_data.m_remoteIP + "'";
            m_indicator.setVariable(var.m_parameters[0], newVar);
            return 0;
        }

        /// <summary>
        /// 获取Port
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>Port</returns>
        private double HTTP_GETREMOTEPORT(CVariable var) {
            return m_data.m_remotePort;
        }

        /// <summary>
        /// 获取请求方法
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double HTTP_GETREQUESTMETHOD(CVariable var) {
            CVariable newVar = new CVariable(m_indicator);
            newVar.m_expression = "'" + m_data.m_method + "'";
            m_indicator.setVariable(var.m_parameters[0], newVar);
            return 0;
        }

        /// <summary>
        /// 获取请求URL
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double HTTP_GETREQUESTURL(CVariable var) {
            CVariable newVar = new CVariable(m_indicator);
            newVar.m_expression = "'" + m_data.m_url + "'";
            m_indicator.setVariable(var.m_parameters[0], newVar);
            return 0;
        }

        /// <summary>
        /// 获取服务名称
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double HTTP_GETSERVICENAME(CVariable var) {
            String url = m_data.m_url;
            int sindex = url.LastIndexOf('/');
            int eindex = url.IndexOf('?');
            String text = "";
            if (eindex != -1) {
                text = url.Substring(sindex + 1, eindex - sindex - 1);
            }
            else {
                if (sindex + 1 == url.Length) {
                    url = url.Substring(0, url.Length - 1);
                    sindex = url.LastIndexOf('/');
                }
                text = url.Substring(sindex + 1);
            }
            CVariable newVar = new CVariable(m_indicator);
            newVar.m_expression = "'" + text + "'";
            m_indicator.setVariable(var.m_parameters[0], newVar);
            return 0;
        }

        /// <summary>
        /// 接受流数据
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double HTTP_HARDREQUEST(CVariable var) {
            FCServerService.callBack(m_data.m_socketID, 0, m_data.m_body, m_data.m_body.Length);
            return 0;
        }

        /// <summary>
        /// 接受流数据
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double HTTP_POSTREQUEST(CVariable var) {
            FCServerService.callBack(m_data.m_socketID, 0, m_data.m_body, m_data.m_body.Length);
            return 0;
        }

        /// <summary>
        /// HTTP获取POST请求的参数
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double HTTP_POSTSTRING(CVariable var) {
            String text = "";
            if (m_data.m_body != null) {
                text = Encoding.Default.GetString(m_data.m_body);
            }
            CVariable newVar = new CVariable(m_indicator);
            newVar.m_expression = "'" + text + "'";
            m_indicator.setVariable(var.m_parameters[0], newVar);
            return 0;
        }

        /// <summary>
        /// HTTP获取GET请求的参数
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double HTTP_QUERYSTRING(CVariable var) {
            String name = m_indicator.getText(var.m_parameters[1]).ToLower();
            string text = "";
            if (m_data.m_parameters.ContainsKey(name)) {
                text = m_data.m_parameters[name];
            }
            CVariable newVar = new CVariable(m_indicator);
            newVar.m_expression = "'" + text + "'";
            m_indicator.setVariable(var.m_parameters[0], newVar);
            return 0;
        }

        /// <summary>
        /// 设置响应状态码
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double HTTP_SETSTATUSCODE(CVariable var) {
            m_data.m_statusCode = (int)m_indicator.getValue(var.m_parameters[0]);
            return 0;
        }

        /// <summary>
        /// HTTP响应写流
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double HTTP_WRITE(CVariable var) {
            int len = var.m_parameters.Length;
            for (int i = 0; i < len; i++) {
                String text = m_indicator.getText(var.m_parameters[i]);
                m_data.m_resStr += text;
            }
            return 0;
        }
    }
}