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
using OwLib;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Net;
using System.Web;
using System.IO;
using FaceCat;

namespace FaceCat {
    /// <summary>
    /// 提示方法
    /// </summary>
    public class CFunctionEx : CFunction {
        /// <summary>
        /// 创建方法
        /// </summary>
        /// <param name="indicator">指标</param>
        /// <param name="id">ID</param>
        /// <param name="name">名称</param>
        /// <param name="native">XML对象</param>
        public CFunctionEx(FCScript indicator, int id, String name, FCNative native) {
            m_indicator = indicator;
            m_ID = id;
            m_name = name;
            m_native = native;
        }

        /// <summary>
        /// 指标
        /// </summary>
        public FCScript m_indicator;

        /// <summary>
        /// XML对象
        /// </summary>
        public FCNative m_native;

        /// <summary>
        /// 所有方法
        /// </summary>
        private const String FUNCTIONS = "CREATETHREAD,ISAPPALIVE";

        /// <summary>
        /// 开始索引
        /// </summary>
        private const int STARTINDEX = 1000000;

        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>结果</returns>
        public override double onCalculate(CVariable var) {
            switch (var.m_functionID) {
                case STARTINDEX:
                    return CREATETHREAD(var);
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 创建指标
        /// </summary>
        /// <param name="native">方法库</param>
        /// <param name="script">脚本</param>
        /// <returns>指标</returns>
        public static FCScript CreateScript(String script, FCNative native) {
            FCScript indicator = new FCScript();
            FCDataTable table = new FCDataTable();
            indicator.DataSource = table;
            CFunctionBase.addFunctions(indicator);
            CFunctionHttp.addFunctions(indicator);
            int index = STARTINDEX;
            string[] functions = FUNCTIONS.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            int functionsSize = functions.Length;
            for (int i = 0; i < functionsSize; i++) {
                indicator.addFunction(new CFunctionEx(indicator, index + i, functions[i], native));
            }
            indicator.Script = script;
            table.addColumn(0);
            table.set(0, 0, 0);
            indicator.onCalculate(0);
            return indicator;
        }

        /// <summary>
        /// 启动线程
        /// </summary>
        /// <param name="param">参数</param>
        private void createThread(object param) {
            FCScript indicator = CreateScript(FCHttpMonitor.MainMonitor.Script, m_native);
            indicator.callFunction(param.ToString());
            indicator.delete();
        }

        /// <summary>
        /// 启动线程
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double CREATETHREAD(CVariable var) {
            Thread thread = new Thread(new ParameterizedThreadStart(createThread));
            thread.Start(m_indicator.getText(var.m_parameters[0]));
            return 0;
        }
    }
}
