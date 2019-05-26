/*基于捂脸猫FaceCat框架 v1.0
 捂脸猫创始人-矿洞程序员-脉脉KOL-陶德 (微信号:suade1984);
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using FaceCat;

namespace FaceCat {
    /// <summary>
    /// 界面相关的库
    /// </summary>
    public class NFunctionBase : CFunction {
        /// <summary>
        /// 创建方法
        /// </summary>
        /// <param name="indicator">指标</param>
        /// <param name="id">ID</param>
        /// <param name="name">名称</param>
        /// <param name="withParameters">是否有参数</param>
        public NFunctionBase(FCScript indicator, int id, String name) {
            m_indicator = indicator;
            m_ID = id;
            m_name = name;
        }

        /// <summary>
        /// 指标
        /// </summary>
        public FCScript m_indicator;

        /// <summary>
        /// 方法
        /// </summary>
        private static String FUNCTIONS = "IN,OUT,SLEEP";

        /// <summary>
        /// 前缀
        /// </summary>
        private static String PREFIX = "";

        /// <summary>
        /// 开始索引
        /// </summary>
        private const int STARTINDEX = 1000;

        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>结果</returns>
        public override double onCalculate(CVariable var) {
            switch (var.m_functionID) {
                case STARTINDEX + 0:
                    return IN(var);
                case STARTINDEX + 1:
                    return OUT(var);
                case STARTINDEX + 2:
                    return SLEEP(var);
                default: return 0;
            }
        }

        /// <summary>
        /// 添加方法
        /// </summary>
        /// <param name="indicator">方法库</param>
        /// <param name="native">脚本</param>
        /// <param name="xml">XML</param>
        /// <returns>指标</returns>
        public static void addFunctions(FCScript indicator, FCNative native) {
            String[] functions = FUNCTIONS.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            int functionsSize = functions.Length;
            for (int i = 0; i < functionsSize; i++) {
                indicator.addFunction(new NFunctionBase(indicator, STARTINDEX + i, PREFIX + functions[i]));
            }
        }

        /// <summary>
        /// 输入函数
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double IN(CVariable var) {
            CVariable newVar = new CVariable(m_indicator);
            newVar.m_expression = "'" + Console.ReadLine() + "'";
            m_indicator.setVariable(var.m_parameters[0], newVar);
            return 0;
        }

        /// <summary>
        /// 输出函数
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double OUT(CVariable var) {
            int len = var.m_parameters.Length;
            for (int i = 0; i < len; i++) {
                String text = m_indicator.getText(var.m_parameters[i]);
                Console.Write(text);
            }
            Console.WriteLine("");
            return 0;
        }

        /// <summary>
        /// 睡眠
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double SLEEP(CVariable var) {
            Thread.Sleep((int)m_indicator.getValue(var.m_parameters[0]));
            return 1;
        }
    }
}
