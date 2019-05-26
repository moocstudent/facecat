/*基于捂脸猫FaceCat框架 v1.0
 捂脸猫创始人-矿洞程序员-脉脉KOL-陶德 (微信号:suade1984);
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;
using FaceCat;

namespace FaceCat
{
    /// <summary>
    /// 属性列表控件
    /// </summary>
    public class EventGrid : FCGrid
    {
        /// <summary>
        /// 创建网格
        /// </summary>
        public EventGrid()
        {
            BackColor = FCColor.Back;
            BorderColor = FCColor.None;
            GridLineColor = FCColor.Border;
            RowStyle.HoveredBackColor = FCDraw.FCCOLORS_HOVEREDROWCOLOR;
            RowStyle.SelectedBackColor = FCDraw.FCCOLORS_SELECTEDROWCOLOR;
            RowStyle.SelectedTextColor = FCDraw.FCCOLORS_TEXTCOLOR4;
            RowStyle.Font = new FCFont("微软雅黑", 12, false, false, false);
            FCGridRowStyle alternateRowStyle = new FCGridRowStyle();
            alternateRowStyle.BackColor = FCDraw.FCCOLORS_ALTERNATEROWCOLOR;
            alternateRowStyle.HoveredBackColor = FCDraw.FCCOLORS_HOVEREDROWCOLOR;
            alternateRowStyle.SelectedBackColor = FCDraw.FCCOLORS_SELECTEDROWCOLOR;
            alternateRowStyle.SelectedTextColor = FCDraw.FCCOLORS_TEXTCOLOR4;
            alternateRowStyle.Font = new FCFont("微软雅黑", 12, false, false, false);
            AlternateRowStyle = alternateRowStyle;
            String content = "";
            FCFile.read(DataCenter.GetAppPath() + "\\config\\CN_EVENTS.txt", ref content);
            String[] strs = content.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            int strsSize = strs.Length;
            for (int i = 0; i < strsSize; i++)
            {
                String[] subStrs = strs[i].Split(new String[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                m_chNames[subStrs[0].ToLower()] = subStrs[1];
            }
        }

        /// <summary>
        /// 中文名称
        /// </summary>
        private Dictionary<String, String> m_chNames = new Dictionary<String, String>();

        /// <summary>
        /// 英文名称列
        /// </summary>
        private FCGridColumn m_enNameColumn;

        /// <summary>
        /// 属性名称列
        /// </summary>
        private FCGridColumn m_nameColumn;

        /// <summary>
        /// 序号列
        /// </summary>
        private FCGridColumn m_orderColumn;

        /// <summary>
        /// 目标列表
        /// </summary>
        private List<FCView> m_targets = new List<FCView>();

        /// <summary>
        /// 属性值列
        /// </summary>
        private FCGridColumn m_valueColumn;

        private DesignerDiv m_designerDiv;

        /// <summary>
        /// 获取或设置编辑视图
        /// </summary>
        public DesignerDiv DesignerDiv
        {
            get { return m_designerDiv; }
            set { m_designerDiv = value; }
        }

        private UIXmlEx m_xml;

        /// <summary>
        /// 获取或设置XML解析
        /// </summary>
        public UIXmlEx Xml
        {
            get { return m_xml; }
            set { m_xml = value; }
        }

        /// <summary>
        /// 添加目标
        /// </summary>
        /// <param name="targets">目标列表</param>
        public void addTargets(List<FCView> targets)
        {
            int targetsSize = targets.Count;
            for (int i = 0; i < targetsSize; i++)
            {
                m_targets.Add(targets[i]);
            }
            createProperties();
        }

        /// <summary>
        /// 清除目标
        /// </summary>
        public void clearTargets()
        {
            //保存上次编辑的控件属性
            FCTextBox editTextBox = EditTextBox;
            if (editTextBox != null)
            {
                if (editTextBox.Tag != null)
                {
                    FCGridCell editingCell = EditTextBox.Tag as FCGridCell;
                    if (editingCell != null)
                    {
                        FCGridCell nameCell = editingCell.Row.getCell("PROPERTYNAME");
                        if (nameCell != null)
                        {
                            String cellName = nameCell.Name;
                            String cellValue = editTextBox.Text;
                            String eventName = "on" + cellName.ToLower();
                            int targetsSize = m_targets.Count;
                            for (int i = 0; i < targetsSize; i++)
                            {
                                FCView target = m_targets[i];
                                if (cellValue == null || cellValue.Trim().Length == 0)
                                {
                                    m_xml.removeProperty(target, eventName);
                                }
                                else
                                {
                                    m_xml.setProperty(target, eventName, cellValue);
                                }
                                target.update();
                            }
                        }
                    }
                }
                editTextBox.Visible = false;
            }
            m_targets.Clear();
        }

        /// <summary>
        /// 创建属性
        /// </summary>
        public void createProperties()
        {
            WinHostEx host = Native.Host as WinHostEx;
            host.LoadingDesigner = true;
            int rowSize = m_rows.Count;
            if (rowSize > 0)
            {
                //清除所有行
                for (int i = 0; i < rowSize; i++)
                {
                    m_rows[i].clearCells();
                    m_rows[i].delete();
                }
                m_rows.Clear();
            }
            int targetsSize = m_targets.Count;
            if (targetsSize > 0)
            {
                FCView target = m_targets[0];
                //获取属性名称
                List<String> eventNames = target.getEventNames();
                Dictionary<String, String> attributes = m_xml.getAttributes(m_xml.Nodes[target]);
                eventNames.Sort();
                int psize = eventNames.Count;
                for (int i = 0; i < psize; i++)
                {
                    String name = eventNames[i];
                    String eventName = "on" + name.ToLower();
                    String value = "";
                    if (attributes.ContainsKey(eventName))
                    {
                        value = attributes[eventName];
                    }
                    String text = name;
                    if (m_chNames.ContainsKey(name.ToLower()))
                    {
                        text = m_chNames[name.ToLower()];
                    }
                    if (value == null) value = "";
                    FCGridRow row = new FCGridRow();
                    addRow(row);
                    //序号
                    GridNoCell orderCell = new GridNoCell();      
                    row.addCell("NO", orderCell);
                    //属性名称
                    FCGridStringCell nameCell = new FCGridStringCell(text);
                    nameCell.Name = name;
                    row.addCell("PROPERTYNAME", nameCell);
                    FCGridStringCell enNameCell = new FCGridStringCell(name);
                    row.addCell("ENNAME", enNameCell);
                    FCGridStringCell textCell = new FCGridStringCell();
                    textCell.AllowEdit = true;
                    row.addCell("PROPERTYVALUE", textCell);
                    textCell.Text = value;
                }
                eventNames.Clear();
                update();
                invalidate();
            }
            host.LoadingDesigner = false;
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <returns>控件类型</returns>
        public override String getControlType()
        {
            return "EventGrid";
        }

        /// <summary>
        /// 获取目标列表
        /// </summary>
        /// <returns>目标列表</returns>
        public List<FCView> getTargets()
        {
            List<FCView> targets = new List<FCView>();
            int targetsSize = m_targets.Count;
            for (int i = 0; i < targetsSize; i++)
            {
                targets.Add(m_targets[i]);
            }
            return targets;
        }

        /// <summary>
        /// 单元格开始编辑
        /// </summary>
        /// <param name="cell">单元格</param>
        public override void onCellEditBegin(FCGridCell cell)
        {
            base.onCellEditBegin(cell);
        }

        /// <summary>
        /// 单元格编辑结束
        /// </summary>
        /// <param name="cell">单元格</param>
        public override void onCellEditEnd(FCGridCell cell)
        {
            base.onCellEditEnd(cell);
            if (cell != null)
            {
                FCGridCell nameCell = cell.Row.getCell("PROPERTYNAME");
                if (nameCell != null)
                {
                    m_designerDiv.saveUndo();
                    String name = nameCell.Name;
                    String eventName = "on" + name.ToLower();
                    String value = cell.Text;
                    int targetsSize = m_targets.Count;
                    for (int i = 0; i < targetsSize; i++)
                    {
                        FCView target = m_targets[i];
                        if (value == null || value.Trim().Length == 0)
                        {
                            m_xml.removeProperty(target, eventName);
                        }
                        else
                        {
                            m_xml.setProperty(target, eventName, value);
                        }
                        target.update();
                    }
                    //恢复正确的值
                    String rightValue = "";
                    for (int i = 0; i < targetsSize; i++)
                    {
                        FCView target = m_targets[i];
                        Dictionary<String, String> attributes = m_xml.getAttributes(m_xml.Nodes[target]);
                        if (attributes.ContainsKey(eventName))
                        {
                            rightValue = attributes[eventName];
                        }
                    }
                    cell.Text = rightValue;
                    Native.update();
                    Native.invalidate();
                }
            }
        }

        /// <summary>
        /// 单元格点击事件
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚轮值</param>
        public override void onCellClick(FCGridCell cell, FCTouchInfo touchInto)
        {
            base.onCellClick(cell, touchInto);
            List<FCGridRow> rows = m_rows;
            int rowsSize = rows.Count;
            for (int i = 0; i < rowsSize; i++)
            {
                FCGridRow row = rows[i];
                List<FCGridCell> cells = row.getCells();
                int cellsSize = cells.Count;
                for (int j = 0; j < cellsSize; j++)
                {
                    FCGridControlCell cCell = cells[j] as FCGridControlCell;
                    if (cCell != null)
                    {
                        if (row == cell.Row)
                        {
                            cCell.Control.TextColor = FCDraw.FCCOLORS_TEXTCOLOR4;
                        }
                        else
                        {
                            cCell.Control.TextColor = FCColor.Text;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 添加控件方法
        /// </summary>
        public override void onLoad()
        {
            base.onLoad();
            String sortStr = "1432";
            UserCookie cookie = new UserCookie();
            if (DataCenter.UserCookieService.GetCookie("EVENTGRIDCOLUMNS", ref cookie) > 0)
            {
                sortStr = cookie.m_value;
            }
            foreach (char ch in sortStr)
            {
                switch (ch)
                {
                    case '1':
                        //创建序号列
                        if (m_orderColumn == null)
                        {
                            m_orderColumn = new GridColumnEx();
                            m_orderColumn.TextColor = FCDraw.FCCOLORS_TEXTCOLOR;
                            m_orderColumn.Font = new FCFont("微软雅黑", 12, false, false, false);
                            m_orderColumn.Text = "序号";
                            m_orderColumn.Name = "NO";
                            m_orderColumn.Width = 30;
                            m_orderColumn.AllowResize = true;
                            m_orderColumn.AllowSort = false;
                            m_orderColumn.AllowDrag = true;
                            m_orderColumn.BackColor = FCDraw.FCCOLORS_BACKCOLOR;
                            addColumn(m_orderColumn);
                        }
                        break;
                    case '2':
                        //创建名称列
                        if (m_nameColumn == null)
                        {
                            m_nameColumn = new GridColumnEx();
                            m_nameColumn.TextColor = FCDraw.FCCOLORS_TEXTCOLOR;
                            m_nameColumn.Font = new FCFont("微软雅黑", 12, false, false, false);
                            m_nameColumn.Text = "事件名称";
                            m_nameColumn.Name = "PROPERTYNAME";
                            m_nameColumn.Width = 120;
                            m_nameColumn.AllowResize = true;
                            m_nameColumn.AllowDrag = true;
                            m_nameColumn.BackColor = FCDraw.FCCOLORS_BACKCOLOR;
                            addColumn(m_nameColumn);
                        }
                        break;
                    case '3':
                        //创建事件脚本列
                        if (m_valueColumn == null)
                        {
                            m_valueColumn = new GridColumnEx();
                            m_valueColumn.TextColor = FCDraw.FCCOLORS_TEXTCOLOR;
                            m_valueColumn.Font = new FCFont("微软雅黑", 12, false, false, false);
                            m_valueColumn.Text = "脚本";
                            m_valueColumn.Name = "PROPERTYVALUE";
                            m_valueColumn.Width = 120;
                            m_valueColumn.AllowResize = true;
                            m_valueColumn.AllowDrag = true;
                            m_valueColumn.BackColor = FCDraw.FCCOLORS_BACKCOLOR;
                            addColumn(m_valueColumn);
                        }
                        break;
                    case '4':
                        //创建英文属性列
                        if (m_enNameColumn == null)
                        {
                            m_enNameColumn = new GridColumnEx();
                            m_enNameColumn.TextColor = FCDraw.FCCOLORS_TEXTCOLOR;
                            m_enNameColumn.Font = new FCFont("微软雅黑", 12, false, false, false);
                            m_enNameColumn.Text = "英文名称";
                            m_enNameColumn.Width = 120;
                            m_enNameColumn.Name = "ENNAME";
                            m_enNameColumn.AllowResize = true;
                            m_enNameColumn.AllowDrag = true;
                            m_enNameColumn.BackColor = FCDraw.FCCOLORS_BACKCOLOR;
                            addColumn(m_enNameColumn);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        public void save()
        {
            List<FCGridColumn> columns = m_columns;
            int columnsSize = columns.Count;
            String sortStr = "";
            for (int i = 0; i < columnsSize; i++)
            {
                FCGridColumn column = columns[i];
                if (column == m_orderColumn)
                {
                    sortStr += "1";
                }
                else if (column == m_nameColumn)
                {
                    sortStr += "2";
                }
                else if (column == m_valueColumn)
                {
                    sortStr += "3";
                }
                else if (column == m_enNameColumn)
                {
                    sortStr += "4";
                }
            }
            UserCookie cookie = new UserCookie();
            cookie.m_key = "EVENTGRIDCOLUMNS";
            cookie.m_value = sortStr;
            DataCenter.UserCookieService.AddCookie(cookie);
        }
    }
}
