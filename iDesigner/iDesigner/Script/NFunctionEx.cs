/*基于捂脸猫FaceCat框架 v1.0
 捂脸猫创始人-矿洞程序员-脉脉KOL-陶德 (微信号:suade1984);
 */
using System;
using System.Collections.Generic;
using System.Text;
using FaceCat;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace FaceCat
{
    /// <summary>
    /// 提示方法
    /// </summary>
    public class NFunctionEx : CFunction
    {
        /// <summary>
        /// 创建方法
        /// </summary>
        /// <param name="indicator">指标</param>
        /// <param name="id">ID</param>
        /// <param name="name">名称</param>
        /// <param name="withParameters">是否有参数</param>
        public NFunctionEx(FCScript indicator, int id, String name, FCUIXml xml)
        {
            m_indicator = indicator;
            m_ID = id;
            m_name = name;
            m_xml = xml;
        }

        /// <summary>
        /// 指标
        /// </summary>
        public FCScript m_indicator;

        /// <summary>
        /// XML对象
        /// </summary>
        public FCUIXml m_xml;

        private const String FUNCTIONS = "ACTIVETAB,CREATE,COPY,CUT,DELETE,OPEN,PASTE,REDO,SAVE,SAVEALL,SAVEAS,UNDO,VIEWSOURCE,PREVIEW,MIRROR,BUGHOLE,REFRESH,TOPARENT,TOSUB,ALIGN,CHANGESTYLE";

        /// <summary>
        /// 前缀
        /// </summary>
        private static String PREFIX = "DESIGNER.";

        /// <summary>
        /// 开始索引
        /// </summary>
        private const int STARTINDEX = 1000000;

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
                    return DESIGNER_ACTIVETAB(var);
                case STARTINDEX + 1:
                    return DESIGNER_CREATE(var);
                case STARTINDEX + 2:
                    return DESIGNER_COPY(var);
                case STARTINDEX + 3:
                    return DESIGNER_CUT(var);
                case STARTINDEX + 4:
                    return DESIGNER_DELETE(var);
                case STARTINDEX + 5:
                    return DESIGNER_OPEN(var);
                case STARTINDEX + 6:
                    return DESIGNER_PASTE(var);
                case STARTINDEX + 7:
                    return DESIGNER_REDO(var);
                case STARTINDEX + 8:
                    return DESIGNER_SAVE(var);
                case STARTINDEX + 9:
                    return DESIGNER_SAVEALL(var);
                case STARTINDEX + 10:
                    return DESIGNER_SAVEAS(var);
                case STARTINDEX + 11:
                    return DESIGNER_UNDO(var);
                case STARTINDEX + 12:
                    return DESIGNER_VIEWSOURCE(var);
                case STARTINDEX + 13:
                    return DESIGNER_PREVIEW(var);
                case STARTINDEX + 14:
                    return DESIGNER_MIRROR(var);
                case STARTINDEX + 15:
                    return DESIGNER_BUGHOLE(var);
                case STARTINDEX + 16:
                    return DESIGNER_REFRESH(var);
                case STARTINDEX + 17:
                    return DESIGNER_TOPARENT(var);
                case STARTINDEX + 18:
                    return DESIGNER_TOSUB(var);
                case STARTINDEX + 19:
                    return DESIGNER_ALIGN(var);
                case STARTINDEX + 20:
                    return DESIGNER_CHANGESTYLE(var);
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 创建指标
        /// </summary>
        /// <param name="native">方法库</param>
        /// <param name="script">脚本</param>
        /// <param name="xml">XML</param>
        /// <returns>指标</returns>
        public static FCScript createIndicator(String script, FCUIXml xml)
        {
            FCScript indicator = new FCScript();
            FCDataTable table = new FCDataTable();
            indicator.DataSource = table;
            NFunctionBase.addFunctions(indicator, xml.Native);
            NFunctionUI.addFunctions(indicator, xml);
            NFunctionWin.addFunctions(indicator, xml.Native);
            int index = 1000000;
            String[] functions = FUNCTIONS.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            int functionsSize = functions.Length;
            for (int i = 0; i < functionsSize; i++)
            {
                indicator.addFunction(new NFunctionEx(indicator, index + i, PREFIX + functions[i], xml));
            }
            indicator.Script = script;
            table.addColumn(0);
            table.set(0, 0, 0);
            indicator.onCalculate(0);
            return indicator;
        }

        /// <summary>
        /// 激活页
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private int DESIGNER_ACTIVETAB(CVariable var)
        {
            Designer designer = m_xml as Designer;
            designer.showActiveTabsMenu();
            return 0;
        }

        /// <summary>
        /// 布局
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private int DESIGNER_ALIGN(CVariable var)
        {
            Designer designer = m_xml as Designer;
            designer.align((int)m_indicator.getValue(var.m_parameters[0]));
            return 0;
        }

        /// <summary>
        /// 虫洞
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private int DESIGNER_BUGHOLE(CVariable var)
        {
            Designer designer = m_xml as Designer;
            designer.bugHole();
            return 0;
        }

        /// <summary>
        /// 切换分格
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private int DESIGNER_CHANGESTYLE(CVariable var)
        {
            Designer designer = m_xml as Designer;
            designer.changeStyle((int)m_indicator.getValue(var.m_parameters[0]));
            return 0;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private int DESIGNER_CREATE(CVariable var)
        {
            Designer designer = m_xml as Designer;
            designer.create();
            return 0;
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private int DESIGNER_COPY(CVariable var)
        {
            Designer designer = m_xml as Designer;
            designer.Copy();
            return 0;
        }

        /// <summary>
        /// 剪切
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private int DESIGNER_CUT(CVariable var)
        {
            Designer designer = m_xml as Designer;
            designer.cut();
            return 0;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private int DESIGNER_DELETE(CVariable var)
        {
            Designer designer = m_xml as Designer;
            designer.del();
            return 0;
        }

        /// <summary>
        /// 镜像
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private int DESIGNER_MIRROR(CVariable var)
        {
            Designer designer = m_xml as Designer;
            designer.mirror();
            return 0;
        }

        /// <summary>
        /// 打开
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private int DESIGNER_OPEN(CVariable var)
        {
            Designer designer = m_xml as Designer;
            designer.open();
            return 0;
        }

        /// <summary>
        /// 粘贴
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private int DESIGNER_PASTE(CVariable var)
        {
            Designer designer = m_xml as Designer;
            designer.paste();
            return 0;
        }

        /// <summary>
        /// 重复
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private int DESIGNER_REDO(CVariable var)
        {
            Designer designer = m_xml as Designer;
            designer.redo();
            return 0;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private int DESIGNER_REFRESH(CVariable var)
        {
            Designer designer = m_xml as Designer;
            designer.refresh();
            return 0;
        }

        /// <summary>
        /// 前往父级
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private int DESIGNER_TOPARENT(CVariable var)
        {
            Designer designer = m_xml as Designer;
            designer.toParent();
            return 0;
        }

        /// <summary>
        /// 预览
        /// </summary>
        /// <param name="var"></param>
        /// <returns></returns>
        private int DESIGNER_PREVIEW(CVariable var)
        {
            Designer designer = m_xml as Designer;
            designer.preview();
            return 0;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private int DESIGNER_SAVE(CVariable var)
        {
            Designer designer = m_xml as Designer;
            designer.save();
            return 0;
        }

        /// <summary>
        /// 保存全部
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private int DESIGNER_SAVEALL(CVariable var)
        {
            Designer designer = m_xml as Designer;
            designer.saveAll();
            return 0;
        }

        /// <summary>
        /// 另存为
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private int DESIGNER_SAVEAS(CVariable var)
        {
            Designer designer = m_xml as Designer;
            designer.saveAs();
            return 0;
        }

        /// <summary>
        /// 前往子级
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private int DESIGNER_TOSUB(CVariable var)
        {
            Designer designer = m_xml as Designer;
            designer.toSub();
            return 0;
        }

        /// <summary>
        /// 撤销
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private int DESIGNER_UNDO(CVariable var)
        {
            Designer designer = m_xml as Designer;
            designer.undo();
            return 0;
        }

        /// <summary>
        /// 查看源代码
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private int DESIGNER_VIEWSOURCE(CVariable var)
        {
            Designer designer = m_xml as Designer;
            designer.viewSource();
            return 0;
        }
    }
}
