/*基于捂脸猫FaceCat框架 v1.0
 捂脸猫创始人-矿洞程序员-脉脉KOL-陶德 (微信号:suade1984);
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using FaceCat;
using System.Windows.Forms;
using System.Drawing;

namespace FaceCat
{
    /// <summary>
    /// 属性列表控件
    /// </summary>
    public class PropertyGrid : FCGrid
    {
        /// <summary>
        /// 创建网格
        /// </summary>
        public PropertyGrid()
        {
            BackColor = FCColor.Back;
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
            m_assembly = Assembly.Load("facecat_net");
            String content = "";
            FCFile.read(DataCenter.GetAppPath() + "\\config\\CN_PROPERTIES.txt", ref content);
            String[] strs = content.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            int strsSize = strs.Length;
            for (int i = 0; i < strsSize; i++)
            {
                String[] subStrs = strs[i].Split(new String[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                m_chNames[subStrs[0].ToLower()] = subStrs[1];
            }
        }

        /// <summary>
        /// 程序集
        /// </summary>
        private Assembly m_assembly;

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
        /// 是否正在设置属性
        /// </summary>
        private bool m_settingProperty;

        /// <summary>
        /// 目标列表
        /// </summary>
        private List<FCView> m_targets = new List<FCView>();

        /// <summary>
        /// 属性值列
        /// </summary>
        private FCGridColumn m_valueColumn;

        private CollectionWindow m_collectionWindow;

        /// <summary>
        /// 获取或设置集合编辑窗体
        /// </summary>
        public CollectionWindow CollectionWindow
        {
            get { return m_collectionWindow; }
            set { m_collectionWindow = value; }
        }

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
        /// 复选框状态切换事件
        /// </summary>
        /// <param name="sender">调用者</param>
        private void checkBoxCheckedChanged(object sender)
        {
            if (m_xml != null)
            {
                if (!m_settingProperty)
                {
                    m_settingProperty = true;
                    FCCheckBox checkBox = sender as FCCheckBox;
                    if (checkBox != null)
                    {
                        m_designerDiv.saveUndo();
                        String name = checkBox.Tag.ToString();
                        String value = checkBox.Checked ? "True" : "False";
                        int targetsSize = m_targets.Count;
                        for (int i = 0; i < targetsSize; i++)
                        {
                            FCView target = m_targets[i];
                            m_xml.setProperty(target, name, value);
                            target.update();
                        }
                        Native.update();
                        Native.invalidate();
                    }
                    m_settingProperty = false;
                }
            }
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
                            int targetsSize = m_targets.Count;
                            for (int i = 0; i < targetsSize; i++)
                            {
                                m_xml.setProperty(m_targets[i], cellName, cellValue);
                                if (m_collectionWindow != null)
                                {
                                    m_collectionWindow.onPropertyChanged(cellName, cellValue);
                                }
                            }
                        }
                    }
                }
                editTextBox.Visible = false;
            }
            m_targets.Clear();
        }

        /// <summary>
        /// 下拉列表控件索引改变事件
        /// </summary>
        /// <param name="sender">调用者</param>
        private void comboBoxSelectedIndexChanged(object sender)
        {
            if (m_xml != null)
            {
                if (!m_settingProperty)
                {
                    m_settingProperty = true;
                    FCComboBox comboBox = sender as FCComboBox;
                    if (comboBox != null)
                    {
                        m_designerDiv.saveUndo();
                        String name = comboBox.Tag.ToString();
                        String value = comboBox.SelectedText;
                        int targetsSize = m_targets.Count;
                        for (int i = 0; i < targetsSize; i++)
                        {
                            FCView target = m_targets[i];
                            m_xml.setProperty(target, name, value);
                            target.update();
                        }
                        Native.update();
                        Native.invalidate();
                    }
                    m_settingProperty = false;
                }
            }
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
                List<String> propertiesName = UIXmlEx.getUnionProperties(m_targets);
                Dictionary<String, String> addProperties = new Dictionary<String, String>();
                if (targetsSize == 1)
                {
                    if (target is FCTabControl)
                    {
                        addProperties["TabPages"] = "collection";
                    }
                    else if (target is FCGrid)
                    {
                        addProperties["Columns"] = "collection";
                    }
                    foreach (String addName in addProperties.Keys)
                    {
                        propertiesName.Add(addName);
                    }
                }
                propertiesName.Sort();
                int psize = propertiesName.Count;
                for (int i = 0; i < psize; i++)
                {
                    String name = propertiesName[i];
                    String value = "";
                    String type = "";
                    if (addProperties.ContainsKey(name))
                    {
                        value = "单击以编辑...";
                        type = addProperties[name];
                    }
                    else
                    {
                        target.getProperty(name.ToLower(), ref value, ref type);
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
                    //英文名称
                    FCGridStringCell enNameCell = new FCGridStringCell(name);
                    row.addCell("ENNAME", enNameCell);
                    //属性值
                    //布尔
                    if (type == "bool")
                    {
                        FCGridCheckBoxCell checkBoxCell = new FCGridCheckBoxCell();
                        checkBoxCell.Control = new CheckBoxM();
                        row.addCell("PROPERTYVALUE", checkBoxCell);
                        checkBoxCell.setBool(value.ToLower() == "true" ? true : false);
                        checkBoxCell.CheckBox.Tag = name;
                        checkBoxCell.CheckBox.ButtonAlign = FCHorizontalAlign.Left;
                        checkBoxCell.CheckBox.addEvent(new FCEvent(checkBoxCheckedChanged), FCEventID.CHECKEDCHANGED);
                    }
                    //枚举
                    else if (type.StartsWith("enum:"))
                    {
                        String strEnum = "FaceCat." + type.Replace("enum:", "");
                        String[] names = Enum.GetNames(m_assembly.GetType(strEnum));
                        FCGridComboBoxCell comboBoxCell = new FCGridComboBoxCell();
                        row.addCell("PROPERTYVALUE", comboBoxCell);
                        comboBoxCell.ComboBox.BackColor = FCColor.None;
                        int nameSize = names.Length;
                        for (int j = 0; j < nameSize; j++)
                        {
                            comboBoxCell.ComboBox.DropDownMenu.addItem(new FCMenuItem(names[j]));
                        }
                        comboBoxCell.ComboBox.SelectedText = value;
                        comboBoxCell.ComboBox.ReadOnly = true;
                        comboBoxCell.ComboBox.Tag = name;
                        comboBoxCell.ComboBox.addEvent(new FCEvent(comboBoxSelectedIndexChanged), FCEventID.SELECTEDINDEXCHANGED);
                    }
                    //集合
                    else if (type == "collection")
                    {
                        FCGridButtonCell buttonCell = new FCGridButtonCell();
                        row.addCell("PROPERTYVALUE", buttonCell);
                        buttonCell.setString(value);
                        buttonCell.Button.Tag = name;
                        buttonCell.Button.BackColor = FCColor.None;
                        buttonCell.Button.TextAlign = FCContentAlignment.MiddleLeft;
                        buttonCell.Button.Font = new FCFont("微软雅黑", 12, false, false, false);
                    }
                    //颜色
                    else if (type == "color")
                    {
                        GridColorCell colorCell = new GridColorCell();
                        colorCell.AllowEdit = true;
                        row.addCell("PROPERTYVALUE", colorCell);
                        colorCell.setString(value);
                        colorCell.Button.Font = new FCFont("微软雅黑", 12, true, false, false);
                    }
                    //字体
                    else if (type == "font")
                    {
                        GridFontCell fontCell = new GridFontCell();
                        fontCell.AllowEdit = true;
                        row.addCell("PROPERTYVALUE", fontCell);
                        fontCell.setString(value);
                        fontCell.Button.Font = new FCFont("微软雅黑", 12, true, false, false);
                    }
                    //输入框
                    else
                    {
                        FCGridStringCell textCell = new FCGridStringCell();
                        textCell.AllowEdit = true;
                        row.addCell("PROPERTYVALUE", textCell);
                        textCell.Text = value;
                    }
                }
                propertiesName.Clear();
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
            return "PropertyGrid";
        }

        /// <summary>
        /// 获取Gdi字体
        /// </summary>
        /// <param name="font">字体</param>
        /// <returns>Gdi字体</returns>
        public Font getFont(FCFont font)
        {
            if (font.m_strikeout)
            {
                if (font.m_bold && font.m_underline && font.m_italic)
                {
                    return new Font(font.m_fontFamily, font.m_fontSize, FontStyle.Bold | FontStyle.Underline | FontStyle.Italic | FontStyle.Strikeout, GraphicsUnit.Pixel);
                }
                else if (font.m_bold && font.m_underline)
                {
                    return new Font(font.m_fontFamily, font.m_fontSize, FontStyle.Bold | FontStyle.Underline | FontStyle.Strikeout, GraphicsUnit.Pixel);
                }
                else if (font.m_bold && font.m_italic)
                {
                    return new Font(font.m_fontFamily, font.m_fontSize, FontStyle.Bold | FontStyle.Italic | FontStyle.Strikeout, GraphicsUnit.Pixel);
                }
                else if (font.m_underline && font.m_italic)
                {
                    return new Font(font.m_fontFamily, font.m_fontSize, FontStyle.Underline | FontStyle.Italic | FontStyle.Strikeout, GraphicsUnit.Pixel);
                }
                else if (font.m_bold)
                {
                    return new Font(font.m_fontFamily, font.m_fontSize, FontStyle.Bold | FontStyle.Strikeout, GraphicsUnit.Pixel);
                }
                else if (font.m_underline)
                {
                    return new Font(font.m_fontFamily, font.m_fontSize, FontStyle.Underline | FontStyle.Strikeout, GraphicsUnit.Pixel);
                }
                else if (font.m_italic)
                {
                    return new Font(font.m_fontFamily, font.m_fontSize, FontStyle.Italic | FontStyle.Strikeout, GraphicsUnit.Pixel);
                }
                else
                {
                    return new Font(font.m_fontFamily, font.m_fontSize, FontStyle.Strikeout, GraphicsUnit.Pixel);
                }
            }
            else
            {
                if (font.m_bold && font.m_underline && font.m_italic)
                {
                    return new Font(font.m_fontFamily, font.m_fontSize, FontStyle.Bold | FontStyle.Underline | FontStyle.Italic, GraphicsUnit.Pixel);
                }
                else if (font.m_bold && font.m_underline)
                {
                    return new Font(font.m_fontFamily, font.m_fontSize, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Pixel);
                }
                else if (font.m_bold && font.m_italic)
                {
                    return new Font(font.m_fontFamily, font.m_fontSize, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Pixel);
                }
                else if (font.m_underline && font.m_italic)
                {
                    return new Font(font.m_fontFamily, font.m_fontSize, FontStyle.Underline | FontStyle.Italic, GraphicsUnit.Pixel);
                }
                else if (font.m_bold)
                {
                    return new Font(font.m_fontFamily, font.m_fontSize, FontStyle.Bold, GraphicsUnit.Pixel);
                }
                else if (font.m_underline)
                {
                    return new Font(font.m_fontFamily, font.m_fontSize, FontStyle.Underline, GraphicsUnit.Pixel);
                }
                else if (font.m_italic)
                {
                    return new Font(font.m_fontFamily, font.m_fontSize, FontStyle.Italic, GraphicsUnit.Pixel);
                }
                else
                {
                    return new Font(font.m_fontFamily, font.m_fontSize, GraphicsUnit.Pixel);
                }
            }
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
            if (touchInto.m_firstTouch)
            {
                if (touchInto.m_clicks == 1)
                {
                    if (!cell.AllowEdit && cell is GridColorCell)
                    {
                        GridColorCell colorCell = cell as GridColorCell;
                        ColorDialog colorDialog = new ColorDialog();
                        colorDialog.AllowFullOpen = true;
                        colorDialog.AnyColor = true;
                        colorDialog.SolidColorOnly = false;
                        int a = 0, r = 0, g = 0, b = 0;
                        FCColor.toArgb(Native.Paint, FCStr.convertStrToColor(colorCell.getString()), ref a, ref r, ref g, ref b);
                        colorDialog.Color = Color.FromArgb(a, r, g, b);
                        if (colorDialog.ShowDialog() == DialogResult.OK)
                        {
                            Color newColor = colorDialog.Color;
                            a = newColor.A;
                            r = newColor.R;
                            g = newColor.G;
                            b = newColor.B;
                            colorCell.setString(FCStr.convertColorToStr(FCColor.argb(a, r, g, b)));
                            FCGridCell nameCell = cell.Row.getCell("PROPERTYNAME");
                            if (nameCell != null)
                            {
                                m_designerDiv.saveUndo();
                                String name = nameCell.Name;
                                String value = cell.Text;
                                int targetsSize = m_targets.Count;
                                for (int i = 0; i < targetsSize; i++)
                                {
                                    FCView target = m_targets[i];
                                    m_xml.setProperty(target, name, value);
                                    if (m_collectionWindow != null)
                                    {
                                        m_collectionWindow.onPropertyChanged(name, value);
                                    }
                                    target.update();
                                }
                                //恢复正确的值
                                String rightValue = "", type = "";
                                for (int i = 0; i < targetsSize; i++)
                                {
                                    m_targets[i].getProperty(name.ToLower(), ref rightValue, ref type);
                                }
                                cell.Text = rightValue;
                                Native.update();
                                Native.invalidate();
                            }
                        }
                        colorDialog.Dispose();
                    }
                    //字体单元格
                    else if (!cell.AllowEdit && cell is GridFontCell)
                    {
                        GridFontCell fontCell = cell as GridFontCell;
                        FontDialog fontDialog = new FontDialog();
                        fontDialog.Font = getFont(FCStr.convertStrToFont(fontCell.getString()));
                        if (fontDialog.ShowDialog() == DialogResult.OK)
                        {
                            Font newFont = fontDialog.Font;
                            fontCell.setString(FCStr.convertFontToStr(new FCFont(newFont.Name, newFont.Size, newFont.Bold, newFont.Underline, newFont.Italic, newFont.Strikeout)));
                            FCGridCell nameCell = cell.Row.getCell("PROPERTYNAME");
                            if (nameCell != null)
                            {
                                m_designerDiv.saveUndo();
                                String name = nameCell.Name;
                                String value = cell.Text;
                                int targetsSize = m_targets.Count;
                                for (int i = 0; i < targetsSize; i++)
                                {
                                    FCView target = m_targets[i];
                                    m_xml.setProperty(target, name, value);
                                    if (m_collectionWindow != null)
                                    {
                                        m_collectionWindow.onPropertyChanged(name, value);
                                    }
                                    target.update();
                                }
                                //恢复正确的值
                                String rightValue = "", type = "";
                                for (int i = 0; i < targetsSize; i++)
                                {
                                    m_targets[i].getProperty(name.ToLower(), ref rightValue, ref type);
                                }
                                cell.Text = rightValue;
                                Native.update();
                                Native.invalidate();
                            }
                        }
                    }
                    //单击编辑框
                    else if (cell is FCGridButtonCell)
                    {
                        FCButton cButton = (cell as FCGridButtonCell).Button;
                        if (cButton.Tag != null)
                        {
                            String collectionName = cButton.Tag.ToString();
                            int targetsSize = m_targets.Count;
                            if (targetsSize > 0)
                            {
                                FCView target = m_targets[0];
                                CollectionWindow collectionWindow = new CollectionWindow(m_native);
                                collectionWindow.CollectionName = collectionName;
                                collectionWindow.DesignerDiv = m_designerDiv;
                                collectionWindow.Target = target;
                                collectionWindow.Xml = m_xml;
                                collectionWindow.IsWinForm = false;
                                collectionWindow.showDialog();
                            }
                        }
                    }
                }
            }
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
                if (cell.Row.m_cells.Count > 0)
                {
                    FCGridCell nameCell = cell.Row.getCell("PROPERTYNAME");
                    if (nameCell != null)
                    {
                        m_designerDiv.saveUndo();
                        String name = nameCell.Name;
                        String value = cell.Text;
                        int targetsSize = m_targets.Count;
                        for (int i = 0; i < targetsSize; i++)
                        {
                            FCView target = m_targets[i];
                            m_xml.setProperty(target, name, value);
                            if (m_collectionWindow != null)
                            {
                                m_collectionWindow.onPropertyChanged(name, value);
                            }
                            target.update();
                        }
                        //恢复正确的值
                        String rightValue = "", type = "";
                        for (int i = 0; i < targetsSize; i++)
                        {
                            m_targets[i].getProperty(name.ToLower(), ref rightValue, ref type);
                        }
                        cell.Text = rightValue;
                        Native.update();
                        Native.invalidate();
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
            if (DataCenter.UserCookieService.GetCookie("PROPERTYGRIDCOLUMNS", ref cookie) > 0)
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
                            m_nameColumn.Text = "属性名称";
                            m_nameColumn.Name = "PROPERTYNAME";
                            m_nameColumn.Width = 120;
                            m_nameColumn.AllowResize = true;
                            m_nameColumn.AllowDrag = true;
                            m_nameColumn.BackColor = FCDraw.FCCOLORS_BACKCOLOR;
                            addColumn(m_nameColumn);
                        }
                        break;
                    case '3':
                        //创建属性值列
                        if (m_valueColumn == null)
                        {
                            m_valueColumn = new GridColumnEx();
                            m_valueColumn.TextColor = FCDraw.FCCOLORS_TEXTCOLOR;
                            m_valueColumn.Font = new FCFont("微软雅黑", 12, false, false, false);
                            m_valueColumn.Text = "属性值";
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
                            m_enNameColumn.Name = "ENNAME";
                            m_enNameColumn.Width = 120;
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
            cookie.m_key = "PROPERTYGRIDCOLUMNS";
            cookie.m_value = sortStr;
            DataCenter.UserCookieService.AddCookie(cookie);
        }
    }

    /// <summary>
    /// 颜色单元格
    /// </summary>
    public class GridColorCell : FCGridButtonCell
    {
        /// <summary>
        /// 创建颜色单元格
        /// </summary>
        public GridColorCell()
        {
        }

        /// <summary>
        /// 值
        /// </summary>
        protected String m_value;

        /// <summary>
        /// 获取要设置的文字
        /// </summary>
        /// <returns></returns>
        public override String getPaintText()
        {
            return m_value;
        }

        /// <summary>
        /// 获取文本
        /// </summary>
        /// <returns>值</returns>
        public override String getString()
        {
            return m_value;
        }

        /// <summary>
        /// 绘制控件方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="rect">区域</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void onPaintControl(FCPaint paint, FCRect rect, FCRect clipRect)
        {
            if (m_control != null)
            {
                m_control.Text = "C";
                m_control.BorderColor = FCColor.Border;
                if (m_value != null)
                {
                    m_control.BackColor = FCStr.convertStrToColor(m_value);
                    m_control.TextColor = FCColor.reverse(paint, m_control.BackColor);
                }
                int buttonWidth = 30;
                FCRect bounds = new FCRect(rect.right - 1 - buttonWidth, rect.top + 1, rect.right - 2, rect.bottom - 1);
                m_control.Bounds = bounds;
                m_control.Region = new FCRect(0, 0, bounds.right - bounds.left, bounds.bottom - bounds.top);
            }
        }

        /// <summary>
        /// 控件的鼠标抬起方法
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚轮值</param>
        public override void onControlTouchUp(FCTouchInfo touchInto)
        {
            m_allowEdit = false;
            base.onControlTouchUp(touchInto);
            m_allowEdit = true;
        }

        /// <summary>
        /// 设置文本
        /// </summary>
        /// <param name="value">值</param>
        public override void setString(String value)
        {
            m_value = value;
        }
    }

    /// <summary>
    /// 字体单元格
    /// </summary>
    public class GridFontCell : FCGridButtonCell
    {
        /// <summary>
        /// 创建颜色单元格
        /// </summary>
        public GridFontCell()
        {
        }

        /// <summary>
        /// 值
        /// </summary>
        protected String m_value;

        /// <summary>
        /// 获取要设置的文字
        /// </summary>
        /// <returns></returns>
        public override String getPaintText()
        {
            return m_value;
        }

        /// <summary>
        /// 获取文本
        /// </summary>
        /// <returns>值</returns>
        public override String getString()
        {
            return m_value;
        }

        /// <summary>
        /// 绘制控件方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="rect">区域</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void onPaintControl(FCPaint paint, FCRect rect, FCRect clipRect)
        {
            if (m_control != null)
            {
                m_control.Text = "A";
                m_control.BorderColor = FCColor.Border;
                int buttonWidth = 30;
                FCRect bounds = new FCRect(rect.right - 1 - buttonWidth, rect.top + 1, rect.right - 2, rect.bottom - 1);
                m_control.Bounds = bounds;
                m_control.Region = new FCRect(0, 0, bounds.right - bounds.left, bounds.bottom - bounds.top);
            }
        }

        /// <summary>
        /// 控件的鼠标抬起方法
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚轮值</param>
        public override void onControlTouchUp(FCTouchInfo touchInto)
        {
            m_allowEdit = false;
            base.onControlTouchUp(touchInto);
            m_allowEdit = true;
        }

        /// <summary>
        /// 设置文本
        /// </summary>
        /// <param name="value">值</param>
        public override void setString(String value)
        {
            m_value = value;
        }
    }

    /// <summary>
    /// 表格序号单元格
    /// </summary>
    public class GridNoCell : FCGridIntCell
    {
        /// <summary>
        /// 获取显示文本
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns>输出文本</returns>
        public override String getPaintText()
        {
            return (Row.Index + 1).ToString();
        }
    }

    /// <summary>
    /// 表格列扩展
    /// </summary>
    public class GridColumnEx : FCGridColumn
    {
        /// <summary>
        /// 重绘背景方法
        /// </summary>
        /// <param name="paint">绘图区域</param>
        /// <param name="clipRect">裁剪对象</param>
        public override void onPaintBackground(FCPaint paint, FCRect clipRect)
        {
            int width = Width, height = Height;
            FCRect drawRect = new FCRect(0, 0, width, height);
            paint.fillGradientRect(FCDraw.FCCOLORS_BACKCOLOR, FCDraw.FCCOLORS_BACKCOLOR2, drawRect, 0, 90);
        }
    }
}
