/*基于捂脸猫FaceCat框架 v1.0
 捂脸猫创始人-矿洞程序员-脉脉KOL-陶德 (微信号:suade1984);
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using FaceCat;

namespace FaceCat
{
    /// <summary>
    /// 界面相关的库
    /// </summary>
    public class NFunctionWin : CFunction
    {
        /// <summary>
        /// 创建方法
        /// </summary>
        /// <param name="indicator">指标</param>
        /// <param name="id">ID</param>
        /// <param name="name">名称</param>
        public NFunctionWin(FCScript indicator, int id, String name)
        {
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
        private static String FUNCTIONS = "BEEP,EXECUTE";

        /// <summary>
        /// 前缀
        /// </summary>
        private static String PREFIX = "WIN.";

        /// <summary>
        /// 开始索引
        /// </summary>
        private const int STARTINDEX = 3000;

        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>结果</returns>
        public override double onCalculate(CVariable var)
        {
            switch (var.m_functionID)
            {
                case STARTINDEX + 0:
                    return WIN_BEEP(var);
                case STARTINDEX + 1:
                    return WIN_EXECUTE(var);
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 添加方法
        /// </summary>
        /// <param name="indicator">方法库</param>
        /// <param name="native">脚本</param>
        /// <param name="xml">XML</param>
        /// <returns>指标</returns>
        public static void addFunctions(FCScript indicator, FCNative native)
        {
            String[] functions = FUNCTIONS.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            int functionsSize = functions.Length;
            for (int i = 0; i < functionsSize; i++)
            {
                indicator.addFunction(new NFunctionWin(indicator, STARTINDEX + i, PREFIX + functions[i]));
            }
        }


        /// <summary>
        /// Windows下主板响
        /// </summary_
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double WIN_BEEP(CVariable var)
        {
            int frequency = 0, duration = 0;
            int vlen = var.m_parameters.Length;
            if (vlen >= 1)
            {
                frequency = (int)m_indicator.getValue(var.m_parameters[0]);
            }
            if (vlen >= 2)
            {
                duration = (int)m_indicator.getValue(var.m_parameters[1]);
            }
            Console.Beep(frequency, duration);
            return 0;
        }

        /// <summary>
        /// Windows下执行程序
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double WIN_EXECUTE(CVariable var)
        {
            WinHostEx.execute(m_indicator.getText(var.m_parameters[0]));
            return 1;
        }
    }
}