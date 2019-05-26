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
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Xml;
using System.IO;

namespace FaceCat {
    /// <summary>
    /// XML界面生成类
    /// </summary>
    public class FCUIXml {
        /// <summary>
        /// 创建生成类
        /// </summary>
        public FCUIXml() {
            m_event = new FCUIEvent(this);
        }

        /// <summary>
        /// 析构方法
        /// </summary>
        ~FCUIXml() {
            delete();
        }

        /// <summary>
        /// 控件列表
        /// </summary>
        public ArrayList<FCView> m_controls = new ArrayList<FCView>();

        /// <summary>
        /// CSS样式
        /// </summary>
        public HashMap<String, String> m_styles = new HashMap<string, string>();

        protected XmlDocument m_xmlDoc = new XmlDocument();

        /// <summary>
        /// 获取或设置XML文档
        /// </summary>
        public XmlDocument XmlDoc {
            get { return m_xmlDoc; }
            set { m_xmlDoc = value; }
        }

        private FCUIEvent m_event;

        /// <summary>
        /// 获取或设置事件
        /// </summary>
        public FCUIEvent Event {
            get { return m_event; }
            set { m_event = value; }
        }

        private FCUIScript m_script;

        /// <summary>
        /// 获取或设置脚本
        /// </summary>
        public FCUIScript Script {
            get { return m_script; }
            set { m_script = value; }
        }

        private bool m_isDeleted = false;

        /// <summary>
        /// 获取是否被销毁
        /// </summary>
        public bool IsDeleted {
            get { return m_isDeleted; }
        }

        private FCNative m_native;

        /// <summary>
        /// 获取获取设置方法库
        /// </summary>
        public FCNative Native {
            get { return m_native; }
            set { m_native = value; }
        }

        /// <summary>
        /// 创建菜单项
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="menu">菜单</param>
        /// <param name="parentItem">父项</param>
        protected virtual void createMenuItem(XmlNode node, FCMenu menu, FCMenuItem parentItem) {
            FCMenuItem item = new FCMenuItem();
            item.Native = m_native;
            setAttributesBefore(node, item);
            if (parentItem != null) {
                parentItem.addItem(item);
            }
            else {
                menu.addItem(item);
            }
            if (node.ChildNodes != null && node.ChildNodes.Count > 0) {
                foreach (XmlNode subNode in node.ChildNodes) {
                    createMenuItem(subNode, menu, item);
                }
            }
            setAttributesAfter(node, item);
            onAddControl(item, node);
        }

        /// <summary>
        /// 是否包含控件
        /// </summary>
        /// <param name="control">控件</param>
        /// <returns>是否包含</returns>
        public virtual bool containsControl(FCView control) {
            foreach (FCView subControl in m_controls) {
                if (subControl == control) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 创建FCGridBand
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="control">控件</param>
        protected virtual void createBandedGridBands(XmlNode node, FCView control) {
            FCGridBand gridBand = control as FCGridBand;
            foreach (XmlNode subNode in node.ChildNodes) {
                FCView subControl = createControl(subNode, subNode.Name.ToLower());
                FCGridBand band = subControl as FCGridBand;
                if (band != null) {
                    band.Native = m_native;
                    gridBand.AddBand(band);
                    setAttributesBefore(subNode, band);
                    createBandedGridBands(subNode, band);
                    setAttributesAfter(subNode, band);
                    onAddControl(band, subNode);
                }

                FCBandedFCGridColumn bandcolumn = subControl as FCBandedFCGridColumn;
                if (bandcolumn != null) {
                    bandcolumn.Native = m_native;
                    gridBand.AddColumn(bandcolumn);
                    setAttributesBefore(subNode, bandcolumn);
                    setAttributesAfter(subNode, bandcolumn);
                    onAddControl(bandcolumn, subNode);
                }
            }
        }

        /// <summary>
        /// 创建表格列
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="control">控件</param>
        protected virtual void createBandedGridColumns(XmlNode node, FCView control) {
            FCBandedGrid bandedGridA = control as FCBandedGrid;
            foreach (XmlNode subNode in node.ChildNodes) {
                FCView subControl = createControl(subNode, subNode.Name.ToLower());
                FCGridBand band = subControl as FCGridBand;
                if (band != null) {
                    band.Native = m_native;
                    bandedGridA.addBand(band);
                }
                setAttributesBefore(subNode, band);
                createBandedGridBands(subNode, band);
                setAttributesAfter(subNode, band);
                onAddControl(band, subNode);
            }
        }

        /// <summary>
        /// 创建控件
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="type">类型</param>
        /// <returns>控件</returns>
        public virtual FCView createControl(XmlNode node, String type) {
            if (type == "band") {
                return new FCGridBand();
            }
            else if (type == "bandcolumn") {
                return new FCBandedFCGridColumn();
            }
            else if (type == "bandedgrid") {
                return new FCBandedGrid();
            }
            else if (type == "button") {
                return new FCButton();
            }
            else if (type == "calendar") {
                return new FCCalendar();
            }
            else if (type == "chart") {
                return new FCChart();
            }
            else if (type == "checkbox") {
                return new FCCheckBox();
            }
            else if (type == "column" || type == "th") {
                return new FCGridColumn();
            }
            else if (type == "combobox" || type == "select") {
                return new FCComboBox();
            }
            else if (type == "datetimepicker") {
                return new FCDateTimePicker();
            }
            else if (type == "div") {
                HashMap<String, String> attributes = getAttributes(node);
                if (attributes.containsKey("type")) {
                    String inputType = attributes.get("type");
                    if (inputType == "groupbox") {
                        return new FCGroupBox();
                    }
                    else if (inputType == "layout") {
                        return new FCLayoutDiv();
                    }
                    else if (inputType == "splitlayout") {
                        return new FCSplitLayoutDiv();
                    }
                    else if (inputType == "tabcontrol") {
                        return new FCTabControl();
                    }
                    else if (inputType == "tabpage") {
                        return new FCTabPage();
                    }
                    else if (inputType == "tablelayout") {
                        return new FCTableLayoutDiv();
                    }
                    else if (inputType == "usercontrol") {
                        return createUserControl(node);
                    }
                }
                return new FCDiv();
            }
            else if (type == "grid" || type == "table") {
                return new FCGrid();
            }
            else if (type == "groupbox") {
                return new FCGroupBox();
            }
            else if (type == "input") {
                HashMap<String, String> attributes = getAttributes(node);
                if (attributes.containsKey("type")) {
                    String inputType = attributes.get("type");
                    if (inputType == "button") {
                        return new FCButton();
                    }
                    else if (inputType == "checkbox") {
                        return new FCCheckBox();
                    }
                    else if (inputType == "datetime") {
                        return new FCDateTimePicker();
                    }
                    else if (inputType == "radio") {
                        return new FCRadioButton();
                    }
                    else if (inputType == "range") {
                        return new FCSpin();
                    }
                    else if (inputType == "text") {
                        return new FCTextBox();
                    }
                    else if (inputType == "usercontrol") {
                        return createUserControl(node);
                    }
                }
                attributes.clear();
            }
            else if (type == "label") {
                return new FCLabel();
            }
            else if (type == "layoutdiv") {
                return new FCLayoutDiv();
            }
            else if (type == "linklabel" || type == "a") {
                return new FCLinkLabel();
            }
            else if (type == "menu") {
                return new FCMenu();
            }
            else if (type == "splitlayoutdiv") {
                return new FCSplitLayoutDiv();
            }
            else if (type == "radiobutton") {
                return new FCRadioButton();
            }
            else if (type == "spin") {
                return new FCSpin();
            }
            else if (type == "tabcontrol") {
                return new FCTabControl();
            }
            else if (type == "tablelayoutdiv") {
                return new FCTableLayoutDiv();
            }
            else if (type == "textbox") {
                return new FCTextBox();
            }
            else if (type == "tree") {
                return new FCTree();
            }
            else if (type == "usercontrol") {
                return createUserControl(node);
            }
            else if (type == "window") {
                return new FCWindow();
            }
            return null;
        }

        /// <summary>
        /// 创建图形控件属性
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="chart">图形控件</param>
        protected virtual void createChartSubProperty(XmlNode node, FCChart chart) {
            foreach (XmlNode node2 in node.ChildNodes) {
                String nodeName2 = node2.Name.ToLower();
                if (nodeName2 == "div") {
                    ChartDiv div = chart.addDiv();
                    setAttributesBefore(node2, div);
                    foreach (XmlNode node3 in node2.ChildNodes) {
                        String nodeName3 = node3.Name.ToLower();
                        if (nodeName3 == "titlebar") {
                            ChartTitleBar titleBar = div.TitleBar;
                            setAttributesBefore(node3, titleBar);
                            setAttributesAfter(node3, titleBar);
                        }
                    }
                    setAttributesAfter(node2, div);
                }
            }
        }

        /// <summary>
        /// 创建表格列
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="control">控件</param>
        protected virtual void createGridColumns(XmlNode node, FCView control) {
            FCGrid grid = control as FCGrid;
            foreach (XmlNode subNode in node.ChildNodes) {
                FCView subControl = createControl(subNode, subNode.Name.ToLower());
                FCGridColumn column = subControl as FCGridColumn;
                if (column != null) {
                    column.Native = m_native;
                    grid.addColumn(column);
                }
                setAttributesBefore(subNode, subControl);
                readChildNodes(subNode, subControl);
                setAttributesAfter(subNode, subControl);
                if (subNode.InnerText.Length > 0) {
                    column.Text = subNode.InnerText;
                }
                onAddControl(subControl, subNode);
            }
        }

        /// <summary>
        /// 创建表格行
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="control">控件</param>
        protected virtual void createGridRow(XmlNode node, FCView control) {
            FCGrid grid = control as FCGrid;
            FCGridRow row = new FCGridRow();
            grid.addRow(row);
            setAttributesBefore(node, row);
            //单元格
            int col = 0;
            foreach (XmlNode node3 in node.ChildNodes) {
                String subNodeName = node3.Name.ToLower();
                String subNodeValue = node3.InnerText;
                if (subNodeName == "cell" || subNodeName == "td") {
                    String cellType = "string";
                    HashMap<String, String> attributes = getAttributes(node3);
                    if (attributes.containsKey("type")) {
                        cellType = attributes.get("type");
                    }
                    attributes.clear();
                    FCGridCell cell = null;
                    if (cellType == "bool") {
                        cell = new FCGridBoolCell();
                    }
                    else if (cellType == "button") {
                        cell = new FCGridButtonCell();
                    }
                    else if (cellType == "checkbox") {
                        cell = new FCGridCheckBoxCell();
                    }
                    else if (cellType == "combobox") {
                        cell = new FCGridComboBoxCell();
                    }
                    else if (cellType == "double") {
                        cell = new FCGridDoubleCell();
                    }
                    else if (cellType == "float") {
                        cell = new FCGridFloatCell();
                    }
                    else if (cellType == "string") {
                        cell = new FCGridStringCell();
                    }
                    else if (cellType == "int") {
                        cell = new FCGridIntCell();
                    }
                    else if (cellType == "long") {
                        cell = new FCGridLongCell();
                    }
                    else if (cellType == "textbox") {
                        cell = new FCGridTextBoxCell();
                    }
                    row.addCell(col, cell);
                    setAttributesBefore(node3, cell);
                    cell.setString(subNodeValue);
                    setAttributesAfter(node3, cell);
                    col++;
                }
            }
            setAttributesAfter(node, row);
        }

        /// <summary>
        /// 创建表格行
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="control">控件</param>
        protected virtual void createGridRows(XmlNode node, FCView control) {
            foreach (XmlNode node2 in node.ChildNodes) {
                if (node2.Name.ToLower() == "row" || node2.Name.ToLower() == "tr") {
                    createGridRow(node2, control);
                }
            }
        }

        /// <summary>
        /// 创建控件框架
        /// </summary>
        /// <param name="hWnd">句柄</param>
        public void createNative() {
            m_native = new FCNative();
        }

        /// <summary>
        /// 创建分割布局控件
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="control">控件</param>
        protected virtual void createSplitLayoutSubProperty(XmlNode node, FCSplitLayoutDiv splitLayoutDiv) {
            int oldCount = splitLayoutDiv.getControls().size();
            setAttributesBefore(node, splitLayoutDiv);
            //读取子节点
            readChildNodes(node, splitLayoutDiv);
            ArrayList<FCView> newControls = splitLayoutDiv.getControls();
            if (newControls.size() - oldCount >= 2) {
                splitLayoutDiv.FirstControl = newControls.get(newControls.size() - 2);
                splitLayoutDiv.SecondControl = newControls.get(newControls.size() - 1);
            }
            setAttributesAfter(node, splitLayoutDiv);
            splitLayoutDiv.update();
            onAddControl(splitLayoutDiv, node);
        }

        /// <summary>
        /// 创建控件
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="control">控件</param>
        protected virtual void createTableLayoutSubProperty(XmlNode node, FCTableLayoutDiv tableLayoutDiv) {
            setAttributesBefore(node, tableLayoutDiv);
            foreach (XmlNode node2 in node.ChildNodes) {
                String nodeName2 = node2.Name.ToLower();
                if (nodeName2 == "columnstyles") {
                    foreach (XmlNode node3 in node2.ChildNodes) {
                        String nodeName3 = node3.Name.ToLower();
                        if (nodeName3 == "columnstyle") {
                            FCColumnStyle column = new FCColumnStyle(FCSizeType.PercentSize, 0.0f);
                            tableLayoutDiv.ColumnStyles.add(column);
                            HashMap<String, String> dic = getAttributes(node3);
                            foreach (String str in dic.Keys) {
                                column.setProperty(str, dic.get(str));
                            }
                        }
                    }
                }
                else if (nodeName2 == "rowstyles") {
                    foreach (XmlNode node3 in node2.ChildNodes) {
                        String nodeName3 = node3.Name.ToLower();
                        if (nodeName3 == "rowstyle") {
                            FCRowStyle row = new FCRowStyle(FCSizeType.PercentSize, 0.0f);
                            tableLayoutDiv.RowStyles.add(row);
                            HashMap<String, String> dic = getAttributes(node3);
                            foreach (String str in dic.Keys) {
                                row.setProperty(str, dic.get(str));
                            }
                        }
                    }
                }
                else if (nodeName2 == "childs") {
                    //读取子节点
                    readChildNodes(node2, tableLayoutDiv);
                }
            }
            setAttributesAfter(node, tableLayoutDiv);
            tableLayoutDiv.update();
            onAddControl(tableLayoutDiv, node);
        }

        /// <summary>
        /// 创建多页夹的页
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="control">控件</param>
        protected virtual void createTabPage(XmlNode node, FCView control) {
            FCTabControl tabControl = control as FCTabControl;
            if (tabControl != null) {
                FCTabPage tabPage = new FCTabPage();
                tabPage.Native = m_native;
                tabControl.addControl(tabPage);
                setAttributesBefore(node, tabPage);
                readChildNodes(node, tabPage);
                setAttributesAfter(node, tabPage);
                onAddControl(tabPage, node);
            }
        }

        /// <summary>
        /// 创建树的节点
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="control">控件</param>
        /// <param name="treeNode">树节点</param>
        protected virtual void createTreeNode(XmlNode node, FCView control, FCTreeNode treeNode) {
            FCTree tree = control as FCTree;
            if (tree != null) {
                FCTreeNode appendNode = new FCTreeNode();
                if (treeNode == null) {
                    tree.appendNode(appendNode);
                }
                else {
                    treeNode.appendNode(appendNode);
                }
                setAttributesBefore(node, appendNode);
                foreach (XmlNode subNode in node.ChildNodes) {
                    if (subNode.Name.ToLower() == "node") {
                        createTreeNode(subNode, control, appendNode);
                    }
                }
                setAttributesAfter(node, appendNode);
            }
        }

        /// <summary>
        /// 创建树的节点
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="control">控件</param>
        protected virtual void createTreeNodes(XmlNode node, FCView control) {
            foreach (XmlNode subNode in node.ChildNodes) {
                createTreeNode(subNode, control, null);
            }
            control.update();
        }

        /// <summary>
        /// 创建子属性
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="control">控件</param>
        public virtual void createSubProperty(XmlNode node, FCView control) {
            String name = node.Name.ToLower();
            String controlType = null;
            if (control != null) {
                controlType = control.getControlType();
            }
            if (name == "bands") {
                if (control is FCBandedGrid) {
                    createBandedGridColumns(node, control);
                }
            }
            else if (name == "columns") {
                if (control is FCGrid) {
                    createGridColumns(node, control);
                }
            }
            //下拉项
            else if (name == "item" || name == "option") {
                //下拉列表
                if (control is FCComboBox) {
                    FCComboBox comboBox = control as FCComboBox;
                    if (comboBox != null) {
                        createMenuItem(node, comboBox.DropDownMenu, null);
                    }
                }
                //菜单
                else if (control is FCMenu) {
                    FCMenu menu = control as FCMenu;
                    if (menu != null) {
                        createMenuItem(node, menu, null);
                    }
                }
            }
            //树节点
            else if (name == "nodes") {
                if (control is FCTree) {
                    createTreeNodes(node, control);
                }
            }
            //行
            else if (name == "rows") {
                if (control is FCGrid) {
                    createGridRows(node, control);
                }
            }
            //多页夹
            else if (name == "tabpage") {
                if (control is FCTabControl) {
                    createTabPage(node, control);
                }
            }
            else if (name == "tr") {
                if (control is FCGrid) {
                    FCGrid grid = control as FCGrid;
                    if (grid.m_columns.size() == 0) {
                        createGridColumns(node, control);
                    }
                    else {
                        createGridRow(node, control);
                    }
                }
            }
        }

        /// <summary>
        /// 创建用户控件
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="cid">标识</param>
        protected virtual FCView createUserControl(XmlNode node) {
            //用户控件
            FCView userControl = null;
            HashMap<String, String> attributes = getAttributes(node);
            if (attributes.containsKey("cid")) {
                userControl = createControl(node, attributes.get("cid"));
            }
            attributes.clear();
            if (userControl != null) {
                userControl.Native = m_native;
                return userControl;
            }
            else {
                return null;
            }
        }

        /// <summary>
        /// 销毁方法
        /// </summary>
        public virtual void delete() {
            if (!m_isDeleted) {
                if (m_event != null) {
                    m_event.delete();
                    m_event = null;
                }
                if (m_script != null) {
                    m_script.delete();
                    m_script = null;
                }
                m_controls.clear();
                m_styles.clear();
                m_isDeleted = true;
            }
        }

        /// <summary>
        /// 查找控件
        /// </summary>
        /// <param name="name">控件名称</param>
        /// <returns>控件</returns>
        public FCView findControl(String name) {
            foreach (FCView control in m_controls) {
                if (control.Name == name) {
                    return control;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="node">节点</param>
        /// <returns>属性键值结合</returns>
        public HashMap<String, String> getAttributes(XmlNode node) {
            HashMap<String, String> attributes = new HashMap<String, String>();
            if (node.Attributes != null && node.Attributes.Count > 0) {
                foreach (XmlAttribute attribute in node.Attributes) {
                    String name = attribute.Name.ToLower();
                    String value = attribute.Value;
                    attributes.put(name, value);
                }
            }
            return attributes;
        }

        /// <summary>
        /// 获取所有的控件
        /// </summary>
        /// <returns>控件集合</returns>
        public ArrayList<FCView> getControls() {
            return m_controls;
        }

        /// <summary>
        /// 获取按钮
        /// </summary>
        /// <param name="name">控件名称</param>
        /// <returns>按钮</returns>
        public FCButton getButton(String name) {
            FCView control = findControl(name);
            if (control != null) {
                return control as FCButton;
            }
            return null;
        }

        /// <summary>
        /// 获取图形控件
        /// </summary>
        /// <param name="name">控件名称</param>
        /// <returns>复选框</returns>
        public FCChart getChart(String name) {
            FCView control = findControl(name);
            if (control != null) {
                return control as FCChart;
            }
            return null;
        }

        /// <summary>
        /// 获取复选框
        /// </summary>
        /// <param name="name">控件名称</param>
        /// <returns>复选框</returns>
        public FCCheckBox getCheckBox(String name) {
            FCView control = findControl(name);
            if (control != null) {
                return control as FCCheckBox;
            }
            return null;
        }

        /// <summary>
        /// 获取下拉控件
        /// </summary>
        /// <param name="name">控件名称</param>
        /// <returns>下拉控件</returns>
        public FCComboBox getComboBox(String name) {
            FCView control = findControl(name);
            if (control != null) {
                return control as FCComboBox;
            }
            return null;
        }

        /// <summary>
        /// 获取日期控件
        /// </summary>
        /// <param name="name">控件名称</param>
        /// <returns>日期控件</returns>
        public FCDateTimePicker getDatePicker(String name) {
            FCView control = findControl(name);
            if (control != null) {
                return control as FCDateTimePicker;
            }
            return null;
        }

        /// <summary>
        /// 获取图层
        /// </summary>
        /// <param name="name">控件名称</param>
        /// <returns>按钮</returns>
        public FCDiv getDiv(String name) {
            FCView control = findControl(name);
            if (control != null) {
                return control as FCDiv;
            }
            return null;
        }

        /// <summary>
        /// 获取表格
        /// </summary>
        /// <param name="name">控件名称</param>
        /// <returns>表格</returns>
        public FCGrid getGrid(String name) {
            FCView control = findControl(name);
            if (control != null) {
                return control as FCGrid;
            }
            return null;
        }

        /// <summary>
        /// 获取组控件
        /// </summary>
        /// <param name="name">控件名称</param>
        /// <returns>组控件</returns>
        public FCGroupBox getGroupBox(String name) {
            FCView control = findControl(name);
            if (control != null) {
                return control as FCGroupBox;
            }
            return null;
        }

        /// <summary>
        /// 获取标签
        /// </summary>
        /// <param name="name">控件名称</param>
        /// <returns>按钮</returns>
        public FCLabel getLabel(String name) {
            FCView control = findControl(name);
            if (control != null) {
                return control as FCLabel;
            }
            return null;
        }

        /// <summary>
        /// 获取布局层
        /// </summary>
        /// <param name="name">控件名称</param>
        /// <returns>布局层</returns>
        public FCLayoutDiv getLayoutDiv(String name) {
            FCView control = findControl(name);
            if (control != null) {
                return control as FCLayoutDiv;
            }
            return null;
        }

        /// <summary>
        /// 获取名称相似控件
        /// </summary>
        /// <param name="name">控件名称</param>
        /// <returns>名称相似控件</returns>
        public ArrayList<FCView> getLikeControls(String name) {
            ArrayList<FCView> controls = new ArrayList<FCView>();
            foreach (FCView control in m_controls) {
                if (control.Name.StartsWith(name)) {
                    controls.add(control);
                }
            }
            return controls;
        }

        /// <summary>
        /// 获取菜单控件
        /// </summary>
        /// <param name="name">控件名称</param>
        /// <returns>菜单控件</returns>
        public FCMenu getMenu(String name) {
            FCView control = findControl(name);
            if (control != null) {
                return control as FCMenu;
            }
            return null;
        }

        /// <summary>
        /// 获取菜单项控件
        /// </summary>
        /// <param name="name">控件名称</param>
        /// <returns>菜单项控件</returns>
        public FCMenuItem getMenuItem(String name) {
            FCView control = findControl(name);
            if (control != null) {
                return control as FCMenuItem;
            }
            return null;
        }

        /// <summary>
        /// 获取单选按钮
        /// </summary>
        /// <param name="name">控件名称</param>
        /// <returns>单选按钮</returns>
        public FCRadioButton getRadioButton(String name) {
            FCView control = findControl(name);
            if (control != null) {
                return control as FCRadioButton;
            }
            return null;
        }

        /// <summary>
        /// 获取数值控件
        /// </summary>
        /// <param name="name">控件名称</param>
        /// <returns>数值控件</returns>
        public FCSpin getSpin(String name) {
            FCView control = findControl(name);
            if (control != null) {
                return control as FCSpin;
            }
            return null;
        }

        /// <summary>
        /// 获取分割层
        /// </summary>
        /// <param name="name">控件名称</param>
        /// <returns>分割层</returns>
        public FCSplitLayoutDiv getSplitLayoutDiv(String name) {
            FCView control = findControl(name);
            if (control != null) {
                return control as FCSplitLayoutDiv;
            }
            return null;
        }

        /// <summary>
        /// 获取多页夹控件
        /// </summary>
        /// <param name="name">控件名称</param>
        /// <returns>多页夹控件</returns>
        public FCTabControl getTabControl(String name) {
            FCView control = findControl(name);
            if (control != null) {
                return control as FCTabControl;
            }
            return null;
        }

        /// <summary>
        /// 获取夹控件
        /// </summary>
        /// <param name="name">控件名称</param>
        /// <returns>多页夹控件</returns>
        public FCTabPage getTabPage(String name) {
            FCView control = findControl(name);
            if (control != null) {
                return control as FCTabPage;
            }
            return null;
        }

        /// <summary>
        /// 获取表格层
        /// </summary>
        /// <param name="name">控件名称</param>
        /// <returns>表格层</returns>
        public FCTableLayoutDiv getTableLayoutDiv(String name) {
            FCView control = findControl(name);
            if (control != null) {
                return control as FCTableLayoutDiv;
            }
            return null;
        }

        /// <summary>
        /// 获取文本框
        /// </summary>
        /// <param name="name">控件名称</param>
        /// <returns>多页夹控件</returns>
        public FCTextBox getTextBox(String name) {
            FCView control = findControl(name);
            if (control != null) {
                return control as FCTextBox;
            }
            return null;
        }

        /// <summary>
        /// 获取树控件
        /// </summary>
        /// <param name="name">控件名称</param>
        /// <returns>树控件</returns>
        public FCTree getTree(String name) {
            FCView control = findControl(name);
            if (control != null) {
                return control as FCTree;
            }
            return null;
        }

        /// <summary>
        /// 获取窗体
        /// </summary>
        /// <param name="name">控件名称</param>
        /// <returns>窗体</returns>
        public FCWindow getWindow(String name) {
            FCView control = findControl(name);
            if (control != null) {
                return control as FCWindow;
            }
            return null;
        }

        /// <summary>
        /// 判断是否后设置属性
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <returns>是否后设置属性</returns>
        public virtual bool isAfterSetingAttribute(String name) {
            if (name == "selectedindex"
                  || name == "selectedtext"
                  || name == "selectedvalue"
              || name == "value") {
                return true;
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// 读取文件，加载到控件中
        /// </summary>
        /// <param name="fileName">文件的路径</param>
        /// <param name="control">控件</param>
        public virtual void loadFile(String fileName, FCView control) {
            m_controls.clear();
            m_styles.clear();
            m_xmlDoc.Load(fileName);
            XmlNode root = m_xmlDoc.DocumentElement;
            foreach (XmlNode node in root.ChildNodes) {
                String nodeName = node.Name.ToLower();
                if (nodeName == "body") {
                    readBody(node, control);
                }
                else if (nodeName == "head") {
                    readHead(node, control);
                }
            }
        }

        /// <summary>
        /// 读取字符串，加载到控件中
        /// </summary>
        /// <param name="xml">字符串</param>
        /// <param name="control">控件</param>
        public virtual void loadXml(String xml, FCView control) {
            m_controls.clear();
            m_styles.clear();
            m_xmlDoc.LoadXml(xml);
            XmlNode root = m_xmlDoc.DocumentElement;
            foreach (XmlNode node in root.ChildNodes) {
                String nodeName = node.Name.ToLower();
                if (nodeName == "body") {
                    readBody(node, control);
                }
                else if (nodeName == "head") {
                    readHead(node, control);
                }
            }
        }

        /// <summary>
        /// 添加控件
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="node">节点</param>
        public virtual void onAddControl(FCView control, XmlNode node) {
            m_controls.add(control);
            //设置事件
            setEvents(node, control);
            m_event.callFunction(control, FCEventID.LOAD);
        }

        /// <summary>
        /// 读取文件体
        /// </summary>
        /// <param name="node">XML节点</param>
        /// <param name="control">控件</param>
        public virtual void readBody(XmlNode node, FCView control) {
            foreach (XmlNode subNode in node.ChildNodes) {
                readNode(subNode, control);
            }
        }

        /// <summary>
        /// 读取子节点
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="control">控件</param>
        public virtual void readChildNodes(XmlNode node, FCView control) {
            if (node.ChildNodes != null && node.ChildNodes.Count > 0) {
                foreach (XmlNode subNode in node.ChildNodes) {
                    readNode(subNode, control);
                }
            }
        }

        /// <summary>
        /// 读取头部
        /// </summary>
        /// <param name="node">XML节点</param>
        /// <param name="control">控件</param>
        public virtual void readHead(XmlNode node, FCView control) {
            foreach (XmlNode childNode in node.ChildNodes) {
                if (childNode.Name == "script") {
                    if (m_script != null) {
                        m_script.setText(childNode.InnerText);
                    }
                }
                else if (childNode.Name == "style") {
                    readStyle(childNode, control);
                }
            }
        }

        /// <summary>
        /// 读取XML
        /// </summary>
        /// <param name="node">XML节点</param>
        /// <param name="parent">父控件</param>
        public virtual FCView readNode(XmlNode node, FCView parent) {
            String type = node.Name.ToLower();
            FCView control = createControl(node, type);
            if (control != null) {
                control.Native = m_native;
                if (parent != null) {
                    parent.addControl(control);
                }
                else {
                    m_native.addControl(control);
                }
                //前设置属性
                setAttributesBefore(node, control);
                if (control is FCSplitLayoutDiv) {
                    //创建分割层
                    createSplitLayoutSubProperty(node, control as FCSplitLayoutDiv);
                }
                else if (control is FCChart) {
                    createChartSubProperty(node, control as FCChart);
                }
                else if (control is FCTableLayoutDiv) {
                    createTableLayoutSubProperty(node, control as FCTableLayoutDiv);
                }
                else {
                    //加载子节点
                    readChildNodes(node, control);
                }
                //后设置属性
                setAttributesAfter(node, control);
                control.update();
                onAddControl(control, node);
            }
            else {
                //创建子属性
                createSubProperty(node, parent);
            }
            return control;
        }

        /// <summary>
        /// 后设置属性
        /// </summary>
        /// <param name="node">XML节点</param>
        /// <param name="control">属性对象</param>
        public virtual void setAttributesAfter(XmlNode node, FCProperty control) {
            HashMap<String, String> attributes = getAttributes(node);
            //读取属性
            foreach (String name in attributes.Keys) {
                if (isAfterSetingAttribute(name)) {
                    control.setProperty(name, attributes.get(name));
                }
                else if (name == "class") {
                    if (m_styles.containsKey(attributes.get(name))) {
                        setStyle(m_styles.get(attributes.get(name)), control);
                    }
                }
                else if (name == "style") {
                    setStyle(attributes.get(name), control);
                }
            }
            attributes.clear();
        }

        /// <summary>
        /// 读取样式
        /// </summary>
        /// <param name="node">XML节点</param>
        /// <param name="control">属性对象</param>
        public virtual void readStyle(XmlNode node, FCProperty control) {
            String styles = node.InnerText;
            bool isStr = false;
            String str = "";
            foreach (char ch in styles) {
                if (ch == '\'') {
                    isStr = !isStr;
                }
                if (!isStr) {
                    if (ch == '}') {
                        int idx = str.IndexOf('{');
                        String className = str.Substring(1, idx - 1).ToLower();
                        String style = str.Substring(idx + 1);
                        m_styles.put(className, style);
                        continue;
                    }
                    else if (ch == ' ' || ch == '\r' || ch == '\n' || ch == '\t') {
                        continue;
                    }
                }
                str += ch.ToString();
            }
        }

        /// <summary>
        /// 前设置属性
        /// </summary>
        /// <param name="node">XML节点</param>
        /// <param name="control">属性对象</param>
        public virtual void setAttributesBefore(XmlNode node, FCProperty control) {
            HashMap<String, String> attributes = getAttributes(node);
            //读取属性
            foreach (String name in attributes.Keys) {
                if (!isAfterSetingAttribute(name)) {
                    control.setProperty(name, attributes.get(name));
                }
            }
            attributes.clear();
        }

        /// <summary>
        /// 设置事件
        /// </summary>
        /// <param name="node">XML节点</param>
        /// <param name="control">属性对象</param>
        public virtual void setEvents(XmlNode node, FCProperty control) {
            FCView baseControl = control as FCView;
            if (baseControl != null) {
                HashMap<String, String> attributes = getAttributes(node);
                foreach (String strEvent in attributes.Keys) {
                    m_event.registerEvent(baseControl, strEvent, attributes.get(strEvent));
                }
                attributes.clear();
            }
        }

        /// <summary>
        /// 设置CSS样式
        /// </summary>
        /// <param name="style">样式</param>
        /// <param name="control">控件</param>
        public virtual void setStyle(String style, FCProperty control) {
            bool isStr = false;
            String str = "";
            foreach (char ch in style) {
                if (ch == '\'') {
                    isStr = !isStr;
                }
                if (!isStr) {
                    if (ch == ';') {
                        int idx = str.IndexOf(':');
                        String pName = str.Substring(0, idx);
                        String pValue = str.Substring(idx + 1);
                        control.setProperty(pName.ToLower(), pValue);
                        str = "";
                        continue;
                    }
                    else if (ch == ' ') {
                        continue;
                    }
                }
                str += ch.ToString();
            }
        }
    }
}