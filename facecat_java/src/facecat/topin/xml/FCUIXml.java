/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.xml;

import facecat.topin.div.*;
import facecat.topin.input.*;
import facecat.topin.tab.*;
import facecat.topin.btn.*;
import facecat.topin.core.*;
import facecat.topin.chart.*;
import facecat.topin.grid.*;
import facecat.topin.label.*;

import java.io.*;
import java.util.*;
import org.w3c.dom.*;
import javax.xml.parsers.*;

/**
 * XML界面生成类
 *
 */
public class FCUIXml {

    /**
     * 创建生成类
     */
    public FCUIXml() {
        m_event = new FCUIEvent(this);
    }

    /**
     * 析构方法
     */
    protected void finalize() throws Throwable {
        delete();
    }

    /**
     * 控件列表
     */
    protected ArrayList<FCView> m_controls = new ArrayList<FCView>();

    protected HashMap<String, String> m_styles = new HashMap<String, String>();

    private FCUIEvent m_event;

    /**
     * 获取或设置事件
     */
    public final FCUIEvent getEvent() {
        return m_event;
    }

    public final void setEvent(FCUIEvent value) {
        m_event = value;
    }

    private FCUIScript m_script;

    /**
     * 获取或设置脚本
     */
    public final FCUIScript getScript() {
        return m_script;
    }

    public final void setScript(FCUIScript value) {
        m_script = value;
    }

    private boolean m_isDeleted = false;

    /**
     * 获取是否被销毁
     */
    public final boolean isDeleted() {
        return m_isDeleted;
    }

    private FCNative m_native;

    /**
     * 获取获取设置方法库
     */
    public final FCNative getNative() {
        return m_native;
    }

    public final void setNative(FCNative value) {
        m_native = value;
    }

    /**
     * 创建GridBand
     *
     * @param node 节点
     * @param control 控件
     */
    protected void createBandedGridBands(Node node, FCView control) {
        FCGridBand gridBand = (FCGridBand) ((control instanceof FCGridBand) ? control : null);
        NodeList nodeList = node.getChildNodes();
        int nodeListSize = nodeList.getLength();
        for (int i = 0; i < nodeListSize; i++) {
            Node subNode = nodeList.item(i);
            FCView subControl = createControl(subNode, subNode.getNodeName().toLowerCase());
            FCGridBand band = (FCGridBand) ((subControl instanceof FCGridBand) ? control : null);
            if (band != null) {
                band.setNative(m_native);
                gridBand.addBand(band);
                SetAttributesBefore(subNode, band);
                createBandedGridBands(subNode, band);
                setAttributesAfter(subNode, band);
                onAddControl(band, subNode);
            }

            FCBandedGridColumn bandcolumn = (FCBandedGridColumn) ((subControl instanceof FCBandedGridColumn) ? control : null);
            if (bandcolumn != null) {
                bandcolumn.setNative(m_native);
                gridBand.addColumn(bandcolumn);
                SetAttributesBefore(subNode, bandcolumn);
                setAttributesAfter(subNode, bandcolumn);
                onAddControl(bandcolumn, subNode);
            }
        }
    }

    /**
     * 创建表格列
     *
     * @param node 节点
     * @param control 控件
     */
    protected void createBandedGridColumns(Node node, FCView control) {
        FCBandedGrid bandedGridA = (FCBandedGrid) ((control instanceof FCBandedGrid) ? control : null);
        NodeList nodeList = node.getChildNodes();
        int nodeListSize = nodeList.getLength();
        for (int i = 0; i < nodeListSize; i++) {
            Node subNode = nodeList.item(i);

            FCView subControl = createControl(subNode, subNode.getNodeName().toLowerCase());
            FCGridBand band = (FCGridBand) ((subControl instanceof FCGridBand) ? control : null);
            if (band != null) {
                band.setNative(m_native);
                bandedGridA.addBand(band);
            }
            SetAttributesBefore(subNode, band);
            createBandedGridBands(subNode, band);
            setAttributesAfter(subNode, band);
            onAddControl(band, subNode);
        }
    }

    /**
     * 创建菜单项
     *
     * @param node 节点
     * @param menu 菜单
     * @param parentItem 父项
     */
    protected void createMenuItem(Node node, FCMenu menu, FCMenuItem parentItem) {
        FCMenuItem item = new FCMenuItem();
        item.setNative(m_native);
        SetAttributesBefore(node, item);
        if (parentItem != null) {
            parentItem.addItem(item);
        } else {
            menu.addItem(item);
        }
        NodeList nodeList = node.getChildNodes();
        if (nodeList != null) {
            int nodeListSize = nodeList.getLength();
            for (int i = 0; i < nodeListSize; i++) {
                Node subNode = nodeList.item(i);
                createMenuItem(subNode, menu, item);
            }
        }
        setAttributesAfter(node, item);
        onAddControl(item, node);
    }

    /**
     * 是否包含控件
     *
     * @param control 控件
     * @return 是否包含
     */
    public boolean containsControl(FCView control) {
        for (FCView subControl : m_controls) {
            if (subControl == control) {
                return true;
            }
        }
        return false;
    }

    /**
     * 创建控件
     *
     * @param node 节点
     * @param type 类型
     * @return 控件
     */
    public FCView createControl(Node node, String type) {
        if (type.equals("band")) {
            return new FCGridBand();
        } else if (type.equals("bandcolumn")) {
            return new FCBandedGridColumn();
        } else if (type.equals("bandedgrid")) {
            return new FCBandedGrid();
        } else if (type.equals("button")) {
            return new FCButton();
        } else if (type.equals("calendar")) {
            return new FCView();
        } else if (type.equals("chart")) {
            return new FCChart();
        } else if (type.equals("checkbox")) {
            return new FCCheckBox();
        } else if (type.equals("column") || type.equals("th")) {
            return new FCGridColumn();
        } else if (type.equals("combobox") || type.equals("select")) {
            return new FCComboBox();
        } else if (type.equals("datetimepicker")) {
            return new FCTextBox();
        } else if (type.equals("div")) {
            HashMap<String, String> attributes = getAttributes(node);
            if (attributes.containsKey("type")) {
                String inputType = attributes.get("type");
                if (inputType.equals("groupbox")) {
                    return new FCGroupBox();
                } else if (inputType.equals("layout")) {
                    return new FCLayoutDiv();
                } else if (inputType.equals("splitlayout")) {
                    return new FCSplitLayoutDiv();
                } else if (inputType.equals("tabcontrol")) {
                    return new FCTabControl();
                } else if (inputType.equals("tabpage")) {
                    return new FCTabPage();
                } else if (inputType.equals("tablelayout")) {
                    return new FCTableLayoutDiv();
                } else if (inputType.equals("usercontrol")) {
                    return createUserControl(node);
                }
            }
            return new FCDiv();
        } else if (type.equals("grid") || type.equals("table")) {
            return new FCGrid();
        } else if (type.equals("groupbox")) {
            return new FCGroupBox();
        } else if (type.equals("input")) {
            HashMap<String, String> attributes = getAttributes(node);
            if (attributes.containsKey("type")) {
                String inputType = attributes.get("type");
                if (inputType.equals("button")) {
                    return new FCButton();
                } else if (inputType.equals("checkbox")) {
                    return new FCCheckBox();
                } else if (inputType.equals("datetime")) {
                    return new FCTextBox();
                } else if (inputType.equals("radio")) {
                    return new FCRadioButton();
                } else if (inputType.equals("range")) {
                    return new FCSpin();
                } else if (inputType.equals("text")) {
                    return new FCTextBox();
                } else if (inputType.equals("usercontrol")) {
                    return createUserControl(node);
                }
            }
            attributes.clear();
        } else if (type.equals("label")) {
            return new FCLabel();
        } else if (type.equals("layoutdiv")) {
            return new FCLayoutDiv();
        } else if (type.equals("linklabel") || type.equals("a")) {
            return new FCLinkLabel();
        } else if (type.equals("menu")) {
            return new FCMenu();
        } else if (type.equals("splitlayoutdiv")) {
            return new FCSplitLayoutDiv();
        } else if (type.equals("radiobutton")) {
            return new FCRadioButton();
        } else if (type.equals("spin")) {
            return new FCSpin();
        } else if (type.equals("tabcontrol")) {
            return new FCTabControl();
        } else if (type.equals("tablelayoutdiv")) {
            return new FCTableLayoutDiv();
        } else if (type.equals("textbox")) {
            return new FCTextBox();
        } else if (type.equals("tree")) {
            return new FCTree();
        } else if (type.equals("usercontrol")) {
            return createUserControl(node);
        } else if (type.equals("window")) {
            return new FCWindow();
        }
        return null;
    }

    /**
     * 创建表格列
     *
     * @param node 节点
     * @param control 控件
     */
    protected void createGridColumns(Node node, FCView control) {
        FCGrid grid = (FCGrid) ((control instanceof FCGrid) ? control : null);
        NodeList nodeList = node.getChildNodes();
        int nodeListSize = nodeList.getLength();
        for (int i = 0; i < nodeListSize; i++) {
            Node subNode = nodeList.item(i);
            FCView subControl = createControl(subNode, subNode.getNodeName().toLowerCase());
            if (subControl != null) {
                FCGridColumn column = (FCGridColumn) ((subControl instanceof FCGridColumn) ? subControl : null);
                if (column != null) {
                    column.setNative(m_native);
                    grid.addColumn(column);
                }
                SetAttributesBefore(subNode, subControl);
                readChildNodes(subNode, subControl);
                setAttributesAfter(subNode, subControl);
                if (subNode.getFirstChild() != null) {
                    if (subNode.getFirstChild().getNodeValue() != null) {
                        column.setText(subNode.getFirstChild().getNodeValue());
                    }
                }
                onAddControl(subControl, subNode);
            }
        }
    }

    /**
     * 创建表格行
     *
     * @param node 节点
     * @param control 控件
     */
    protected void createGridRow(Node node, FCView control) {
        FCGrid grid = (FCGrid) ((control instanceof FCGrid) ? control : null);
        FCGridRow row = new FCGridRow();
        grid.addRow(row);
        SetAttributesBefore(node, row);
        //单元格
        int col = 0;
        NodeList nodeList2 = node.getChildNodes();
        int nodeListSize2 = nodeList2.getLength();
        for (int j = 0; j < nodeListSize2; j++) {
            Node node3 = nodeList2.item(j);
            String subNodeName = node3.getNodeName().toLowerCase();
            if (subNodeName.equals("cell") || subNodeName.equals("td")) {
                String subNodeValue = node3.getFirstChild().getNodeValue();
                String cellType = "string";
                HashMap<String, String> attributes = getAttributes(node3);
                if (attributes.containsKey("type")) {
                    cellType = attributes.get("type");
                }
                attributes.clear();
                FCGridCell cell = null;
                if (cellType.equals("bool")) {
                    cell = new FCGridBoolCell();
                } else if (cellType.equals("button")) {
                    cell = new FCGridButtonCell();
                } else if (cellType.equals("checkbox")) {
                    cell = new FCGridCheckBoxCell();
                } else if (cellType.equals("combobox")) {
                    cell = new FCGridComboBoxCell();
                } else if (cellType.equals("double")) {
                    cell = new FCGridDoubleCell();
                } else if (cellType.equals("float")) {
                    cell = new FCGridFloatCell();
                } else if (cellType.equals("string")) {
                    cell = new FCGridStringCell();
                } else if (cellType.equals("int")) {
                    cell = new FCGridIntCell();
                } else if (cellType.equals("long")) {
                    cell = new FCGridLongCell();
                } else if (cellType.equals("textbox")) {
                    cell = new FCGridTextBoxCell();
                }
                row.addCell(col, cell);
                SetAttributesBefore(node3, cell);
                cell.setString(subNodeValue);
                setAttributesAfter(node3, cell);
                col++;
            }
        }
        setAttributesAfter(node, row);
    }

    protected void createGridRows(Node node, FCView control) {
        NodeList nodeList = node.getChildNodes();
        int nodeListSize = nodeList.getLength();
        for (int i = 0; i < nodeListSize; i++) {
            Node node2 = nodeList.item(i);
            if (node2.getNodeName().toLowerCase().equals("row")
                    || node2.getNodeName().toLowerCase().equals("tr")) {
                createGridRow(node2, control);
            }
        }
    }

    /**
     * 创建控件框架
     */
    public final void createNative() {
        m_native = new FCNative();
    }

    /**
     * 创建分割布局控件
     *
     * @param node 节点
     * @param splitLayoutDiv 控件
     */
    protected void createSplitLayoutSubProperty(Node node, FCSplitLayoutDiv splitLayoutDiv) {
        int oldCount = splitLayoutDiv.getControls().size();
        SetAttributesBefore(node, splitLayoutDiv);
        //读取子节点
        readChildNodes(node, splitLayoutDiv);
        ArrayList<FCView> newControls = splitLayoutDiv.getControls();
        if (newControls.size() - oldCount >= 2) {
            splitLayoutDiv.setFirstControl(newControls.get(newControls.size() - 2));
            splitLayoutDiv.setSecondControl(newControls.get(newControls.size() - 1));
        }
        setAttributesAfter(node, splitLayoutDiv);
        splitLayoutDiv.update();
        onAddControl(splitLayoutDiv, node);
    }

    /**
     * 创建控件
     *
     * @param node 节点
     * @param tableLayoutDiv 控件
     */
    protected void createTableLayoutSubProperty(Node node, FCTableLayoutDiv tableLayoutDiv) {
        SetAttributesBefore(node, tableLayoutDiv);
        NodeList nodeList = node.getChildNodes();
        int nodeListSize = nodeList.getLength();
        for (int i = 0; i < nodeListSize; i++) {
            Node node2 = nodeList.item(i);
            String nodeName2 = node2.getNodeName().toLowerCase();
            if (nodeName2.equals("columnstyles")) {
                NodeList nodeSmallList = node2.getChildNodes();
                int smallSize = nodeSmallList.getLength();
                for (int j = 0; j < smallSize; j++) {
                    Node node3 = nodeSmallList.item(j);

                    String nodeName3 = node3.getNodeName().toLowerCase();
                    if (nodeName3.equals("columnstyle")) {
                        FCColumnStyle column = new FCColumnStyle(FCSizeType.PercentSize, 0.0f);
                        ArrayList<FCColumnStyle> styles = tableLayoutDiv.getColumnStyles();
                        styles.add(column);
                        tableLayoutDiv.setColumnStyles(styles);
                        HashMap<String, String> dic = getAttributes(node3);
                        for (String str : dic.keySet()) {
                            column.setProperty(str, dic.get(str));
                        }
                        dic.clear();
                    }
                }
            } else if (nodeName2.equals("rowstyles")) {
                NodeList nodeSmallList = node2.getChildNodes();
                int smallSize = nodeSmallList.getLength();
                for (int j = 0; j < smallSize; j++) {
                    Node node3 = nodeSmallList.item(j);

                    String nodeName3 = node3.getNodeName().toLowerCase();
                    if (nodeName3.equals("rowstyle")) {
                        FCRowStyle row = new FCRowStyle(FCSizeType.PercentSize, 0.0f);
                        ArrayList<FCRowStyle> styles = tableLayoutDiv.getRowStyles();
                        styles.add(row);
                        tableLayoutDiv.setRowStyles(styles);

                        HashMap<String, String> dic = getAttributes(node3);
                        for (String str : dic.keySet()) {
                            row.setProperty(str, dic.get(str));
                        }
                        dic.clear();
                    }
                }
            } else if (nodeName2.equals("childs")) {
                //读取子节点
                readChildNodes(node2, tableLayoutDiv);
            }
        }
        setAttributesAfter(node, tableLayoutDiv);
        tableLayoutDiv.update();
        onAddControl(tableLayoutDiv, node);
    }

    /**
     * 创建多页夹的页
     *
     * @param node 节点
     * @param control 控件
     */
    protected void createTabPage(Node node, FCView control) {
        FCTabControl tabControl = (FCTabControl) ((control instanceof FCTabControl) ? control : null);
        if (tabControl != null) {
            FCTabPage tabPage = new FCTabPage();
            tabPage.setNative(m_native);
            tabControl.addControl(tabPage);
            SetAttributesBefore(node, tabPage);
            readChildNodes(node, tabPage);
            setAttributesAfter(node, tabPage);
            onAddControl(tabPage, node);
        }
    }

    /**
     * 创建树的节点
     *
     * @param node 节点
     * @param control 控件
     * @param treeNode 树节点
     */
    protected void createTreeNode(Node node, FCView control, FCTreeNode treeNode) {
        FCTree tree = (FCTree) ((control instanceof FCTree) ? control : null);
        if (tree != null) {
            FCTreeNode appendNode = new FCTreeNode();
            if (treeNode == null) {
                tree.appendNode(appendNode);
            } else {
                treeNode.appendNode(appendNode);
            }
            SetAttributesBefore(node, appendNode);
            NodeList nodeList = node.getChildNodes();
            int nodeListSize = nodeList.getLength();
            for (int i = 0; i < nodeListSize; i++) {
                Node subNode = nodeList.item(i);
                if (subNode.getNodeName().toLowerCase().equals("node")) {
                    createTreeNode(subNode, control, appendNode);
                }
            }
            setAttributesAfter(node, appendNode);
        }
    }

    /**
     * 创建树的节点
     *
     * @param node 节点
     * @param control 控件
     */
    protected void createTreeNodes(Node node, FCView control) {
        NodeList nodeList = node.getChildNodes();
        int nodeListSize = nodeList.getLength();
        for (int i = 0; i < nodeListSize; i++) {
            Node subNode = nodeList.item(i);
            createTreeNode(subNode, control, null);
        }
        control.update();
    }

    /**
     * 创建子属性
     *
     * @param node 节点
     * @param control 控件
     */
    public void createSubProperty(Node node, FCView control) {
        String name = node.getNodeName().toLowerCase();
        String controlType = null;
        if (control != null) {
            controlType = control.getControlType();
        }
        if (name.equals("bands")) {
            if (controlType.equals("BandedGrid")) {
                createBandedGridColumns(node, control);
            }
        }
        // 下拉项
        else if (name.equals("columns")) {
            if (controlType.equals("Grid") || controlType.equals("Tree")) {
                createGridColumns(node, control);
            }
        }
        // 下拉列表
        else if (name.equals("item") || name.equals("option")) {
            // 下拉列表
            if (controlType.equals("ComboBox")) {
                FCComboBox comboBox = (FCComboBox) ((control instanceof FCComboBox) ? control : null);
                if (comboBox != null) {
                    createMenuItem(node, comboBox.getDropDownMenu(), null);
                }
            }
            // 菜单
            else if (controlType.equals("Menu")) {
                FCMenu menu = (FCMenu) ((control instanceof FCMenu) ? control : null);
                if (menu != null) {
                    createMenuItem(node, menu, null);
                }
            }
        }
        // 树节点
        else if (name.equals("nodes")) {
            if (controlType.equals("Tree")) {
                createTreeNodes(node, control);
            }
        }
        // 行
        else if (name.equals("rows")) {
            if (controlType.equals("Grid")) {
                createGridRows(node, control);
            } else if (controlType.equals("BandedGrid")) {
                createGridRows(node, control);
            }
        }
        // 多页夹
        else if (name.equals("tabpage")) {
            if (controlType.equals("TabControl")) {
                createTabPage(node, control);
            }
        } else if (name.equals("tr")) {
            FCGrid grid = (FCGrid) ((control instanceof FCGrid) ? control : null);
            if (grid != null) {
                if (grid.m_columns.size() == 0) {
                    createGridColumns(node, control);
                } else {
                    createGridRow(node, control);
                }
            }
        }
    }

    /**
     * 创建用户控件
     *
     * @param node 节点
     */
    protected FCView createUserControl(Node node) {
        // 用户控件
        FCView userControl = null;
        HashMap<String, String> attributes = getAttributes(node);
        if (attributes.containsKey("cid")) {
            userControl = createControl(node, attributes.get("cid"));
        }
        attributes.clear();
        if (userControl != null) {
            userControl.setNative(m_native);
            return userControl;
        } else {
            return null;
        }
    }

    /**
     * 销毁方法
     *
     */
    public void delete() {
        if (!m_isDeleted) {
            if (m_event != null) {
                m_event.delete();
                m_event = null;
            }
            if (m_script != null) {
                m_script = null;
            }
            m_controls.clear();
            m_styles.clear();
            m_isDeleted = true;
        }
    }

    /**
     * 查找控件
     *
     * @param name 控件名称
     * @return 控件
     */
    public FCView findControl(String name) {
        FCView findControl = null;
        for (FCView control : m_controls) {
            if (name.equals(control.getName())) {
                findControl = control;
            }
        }
        if (findControl == null) {
            findControl = m_native.findControl(name);
        }
        return findControl;
    }

    /**
     * 获取属性
     *
     * @param node 节点
     * @return 属性键值结合
     */
    public final HashMap<String, String> getAttributes(Node node) {
        HashMap<String, String> attributes = new HashMap<String, String>();
        NamedNodeMap nodeMap = node.getAttributes();
        if (nodeMap != null) {
            int nodeMapSize = nodeMap.getLength();
            for (int i = 0; i < nodeMapSize; i++) {
                Node attribute = nodeMap.item(i);
                String name = attribute.getNodeName().toLowerCase();
                String value = attribute.getNodeValue();
                attributes.put(name, value);
            }
        }
        return attributes;
    }

    /**
     * 获取所有的控件
     *
     * @return 控件集合
     */
    public final ArrayList<FCView> getControls() {
        return m_controls;
    }

    /**
     * 获取按钮
     *
     * @param name 控件名称
     * @return 按钮
     */
    public final FCButton getButton(String name) {
        FCView control = findControl(name);
        if (control != null) {
            return (FCButton) ((control instanceof FCButton) ? control : null);
        }
        return null;
    }

    /**
     * 获取图形控件
     *
     * @param name 控件名称
     * @return 复选框
     */
    public final FCChart getChart(String name) {
        FCView control = findControl(name);
        if (control != null) {
            return (FCChart) ((control instanceof FCChart) ? control : null);
        }
        return null;
    }

    /**
     * 获取复选框
     *
     * @param name 控件名称
     * @return 复选框
     */
    public final FCCheckBox getCheckBox(String name) {
        FCView control = findControl(name);
        if (control != null) {
            return (FCCheckBox) ((control instanceof FCCheckBox) ? control : null);
        }
        return null;
    }

    /**
     * 获取下拉控件
     *
     * @param name 控件名称
     * @return 下拉控件
     */
    public final FCComboBox getComboBox(String name) {
        FCView control = findControl(name);
        if (control != null) {
            return (FCComboBox) ((control instanceof FCComboBox) ? control : null);
        }
        return null;
    }

    /**
     * 获取图层
     *
     * @param name 控件名称
     * @return 按钮
     */
    public final FCDiv getDiv(String name) {
        FCView control = findControl(name);
        if (control != null) {
            return (FCDiv) ((control instanceof FCDiv) ? control : null);
        }
        return null;
    }

    /**
     * 获取表格
     *
     * @param name 控件名称
     * @return 表格
     */
    public final FCGrid getGrid(String name) {
        FCView control = findControl(name);
        if (control != null) {
            return (FCGrid) ((control instanceof FCGrid) ? control : null);
        }
        return null;
    }

    /**
     * 获取组控件
     *
     * @param name 控件名称
     * @return 组控件
     */
    public final FCGroupBox getGroupBox(String name) {
        FCView control = findControl(name);
        if (control != null) {
            return (FCGroupBox) ((control instanceof FCGroupBox) ? control : null);
        }
        return null;
    }

    /**
     * 获取标签
     *
     * @param name 控件名称
     * @return 按钮
     */
    public final FCLabel getLabel(String name) {
        FCView control = findControl(name);
        if (control != null) {
            return (FCLabel) ((control instanceof FCLabel) ? control : null);
        }
        return null;
    }

    /**
     * 获取布局层
     *
     * @param name 控件名称
     * @return 布局层
     */
    public final FCLayoutDiv getLayoutDiv(String name) {
        FCView control = findControl(name);
        if (control != null) {
            return (FCLayoutDiv) ((control instanceof FCLayoutDiv) ? control : null);
        }
        return null;
    }

    /**
     * 获取名称相似控件
     *
     * @param name 控件名称
     * @return 名称相似控件
     */
    public final ArrayList<FCView> getLikeControls(String name) {
        ArrayList<FCView> controls = new ArrayList<FCView>();
        for (FCView control : m_controls) {
            if (control.getName().startsWith(name)) {
                controls.add(control);
            }
        }
        return controls;
    }

    /**
     * 获取菜单控件
     *
     * @param name 控件名称
     * @return 菜单控件
     */
    public final FCMenu getMenu(String name) {
        FCView control = findControl(name);
        if (control != null) {
            return (FCMenu) ((control instanceof FCMenu) ? control : null);
        }
        return null;
    }

    /**
     * 获取菜单项控件
     *
     * @param name 控件名称
     * @return 菜单项控件
     */
    public final FCMenuItem getMenuItem(String name) {
        FCView control = findControl(name);
        if (control != null) {
            return (FCMenuItem) ((control instanceof FCMenuItem) ? control : null);
        }
        return null;
    }

    /**
     * 获取单选框控件
     *
     * @param name 控件名称
     * @return 单选框控件
     */
    public FCRadioButton getRadioButton(String name) {
        FCView control = findControl(name);
        if (control != null) {
            return (FCRadioButton) control;
        }
        return null;
    }

    /**
     * 获取数值控件
     *
     * @param name 控件名称
     * @return 数值控件
     */
    public final FCSpin getSpin(String name) {
        FCView control = findControl(name);
        if (control != null) {
            return (FCSpin) ((control instanceof FCSpin) ? control : null);
        }
        return null;
    }

    /**
     * 获取分割层
     *
     * @param name 控件名称
     * @return 分割层
     */
    public final FCSplitLayoutDiv getSplitLayoutDiv(String name) {
        FCView control = findControl(name);
        if (control != null) {
            return (FCSplitLayoutDiv) ((control instanceof FCSplitLayoutDiv) ? control : null);
        }
        return null;
    }

    /**
     * 获取多页夹控件
     *
     * @param name 控件名称
     * @return 多页夹控件
     */
    public final FCTabControl getTabControl(String name) {
        FCView control = findControl(name);
        if (control != null) {
            return (FCTabControl) ((control instanceof FCTabControl) ? control : null);
        }
        return null;
    }

    /**
     * 获取夹控件
     *
     * @param name 控件名称
     * @return 多页夹控件
     */
    public final FCTabPage getTabPage(String name) {
        FCView control = findControl(name);
        if (control != null) {
            return (FCTabPage) ((control instanceof FCTabPage) ? control : null);
        }
        return null;
    }

    /**
     * 获取表格层
     *
     * @param name 控件名称
     * @return 表格层
     */
    public final FCTableLayoutDiv getTableLayoutDiv(String name) {
        FCView control = findControl(name);
        if (control != null) {
            return (FCTableLayoutDiv) ((control instanceof FCTableLayoutDiv) ? control : null);
        }
        return null;
    }

    /**
     * 获取文本框
     *
     * @param name 控件名称
     * @return 多页夹控件
     */
    public final FCTextBox getTextBox(String name) {
        FCView control = findControl(name);
        if (control != null) {
            return (FCTextBox) ((control instanceof FCTextBox) ? control : null);
        }
        return null;
    }

    /**
     * 获取树控件
     *
     * @param name 控件名称
     * @return 树控件
     */
    public final FCTree getTree(String name) {
        FCView control = findControl(name);
        if (control != null) {
            return (FCTree) ((control instanceof FCTree) ? control : null);
        }
        return null;
    }

    /**
     * 获取窗体
     *
     * @param name 控件名称
     * @return 窗体
     */
    public final FCWindow getWindow(String name) {
        FCView control = findControl(name);
        if (control != null) {
            return (FCWindow) ((control instanceof FCWindow) ? control : null);
        }
        return null;
    }

    /**
     * 判断是否后设置属性
     *
     * @param name 属性名称
     * @return 是否后设置属性
     */
    public boolean isAfterSetingAttribute(String name) {
        if (name.equals("selectedindex") || name.equals("selectedtext") || name.equals("selectedvalue") || name.equals("value")) {
            return true;
        } else {
            return false;
        }
    }

    /**
     * 读取字符串，加载到控件中
     *
     * @param xml 文件的路径
     * @param control 控件
     */
    public void loadXml(String xml, FCView control) {
        m_controls.clear();
        m_styles.clear();
        try {
            DocumentBuilderFactory factory = DocumentBuilderFactory.newInstance();
            DocumentBuilder builder = factory.newDocumentBuilder();
            InputStream is = new ByteArrayInputStream(xml.getBytes());
            Document doc = builder.parse(is);
            Node root = doc.getDocumentElement();
            NodeList nodeList = root.getChildNodes();
            int nodeListSize = nodeList.getLength();
            for (int i = 0; i < nodeListSize; i++) {
                Node node = nodeList.item(i);
                String nodeName = node.getNodeName().toLowerCase();
                if (nodeName.equals("body")) {
                    readBody(node, control);
                } else if (nodeName.equals("head")) {
                    readHead(node, control);
                }
            }
        } catch (Exception ex) {
        }
    }

    /**
     * 添加控件
     *
     * @param control 控件
     * @param node 节点
     */
    public void onAddControl(FCView control, Node node) {
        m_controls.add(control);
        //设置事件
        setEvents(node, control);
        m_event.callFunction(control, FCEventID.LOAD);
    }

    /**
     * 读取文件体
     *
     * @param node XML节点
     * @param control 控件
     */
    public void readBody(Node node, FCView control) {
        NodeList nodeList = node.getChildNodes();
        int nodeListSize = nodeList.getLength();
        for (int i = 0; i < nodeListSize; i++) {
            Node subNode = nodeList.item(i);
            readNode(subNode, control);
        }
    }

    /**
     * 读取子节点
     *
     * @param node 节点
     * @param control 控件
     */
    public void readChildNodes(Node node, FCView control) {
        NodeList nodeList = node.getChildNodes();
        int nodeListSize = nodeList.getLength();
        for (int i = 0; i < nodeListSize; i++) {
            Node subNode = nodeList.item(i);
            readNode(subNode, control);
        }
    }

    /**
     * 读取头部
     *
     * @param node XML节点
     * @param control 控件
     */
    public void readHead(Node node, FCView control) {
        NodeList nodeList = node.getChildNodes();
        int nodeListSize = nodeList.getLength();
        for (int i = 0; i < nodeListSize; i++) {
            Node childNode = nodeList.item(i);
            if (childNode.getNodeName().equals("script")) {
                m_script.setText(childNode.getNodeValue());
            } else if (childNode.getNodeName().equals("style")) {
                readStyle(childNode, control);
            }
        }
    }

    /**
     * 读取XML
     *
     * @param node XML节点
     * @param parent 父控件
     */
    public FCView readNode(Node node, FCView parent) {
        String name = node.getNodeName().toLowerCase();
        FCView control = createControl(node, node.getNodeName().toLowerCase());
        if (control != null) {
            control.setNative(m_native);
            if (parent != null) {
                parent.addControl(control);
            } else {
                m_native.addControl(control);
            }
            // 前设置属性
            SetAttributesBefore(node, control);
            FCSplitLayoutDiv splitLayoutDiv = (FCSplitLayoutDiv) ((control instanceof FCSplitLayoutDiv) ? control : null);
            FCTableLayoutDiv tableLayoutDiv = (FCTableLayoutDiv) ((control instanceof FCTableLayoutDiv) ? control : null);
            if (splitLayoutDiv != null) {
                createSplitLayoutSubProperty(node, splitLayoutDiv);
            } else if (tableLayoutDiv != null) {
                createTableLayoutSubProperty(node, tableLayoutDiv);
            } else {
                // 加载子节点
                readChildNodes(node, control);
            }
            // 后设置属性
            setAttributesAfter(node, control);
            control.update();
            onAddControl(control, node);
        } else {
            // 创建子属性
            createSubProperty(node, parent);
        }
        return control;
    }

    public void readStyle(Node node, FCProperty control) {
        String styles = node.getNodeValue();
        boolean isStr = false;
        String str = "";
        for (char ch : styles.toCharArray()) {
            if (ch == '\'') {
                isStr = !isStr;
            }
            if (!isStr) {
                if (ch == '}') {
                    int idx = str.indexOf('{');
                    String className = str.substring(1, idx).toLowerCase();
                    String style = str.substring(idx + 1);
                    m_styles.put(className, style);
                    continue;
                } else if (ch == ' ' || ch == '\r' || ch == '\n' || ch == '\t') {
                    continue;
                }
            }
            str += ch;
        }
    }

    /**
     * 后设置属性
     *
     * @param node XML节点
     * @param control 属性对象
     */
    public void setAttributesAfter(Node node, FCProperty control) {
        if (control != null) {
            HashMap<String, String> attributes = getAttributes(node);
            // 读取属性
            for (String name : attributes.keySet()) {
                if (isAfterSetingAttribute(name)) {
                    control.setProperty(name, attributes.get(name));
                } else if (name.equals("class")) {
                    if (m_styles.containsKey(attributes.get(name))) {
                        setStyle(m_styles.get(attributes.get(name)), control);
                    }
                } else if (name.equals("style")) {
                    setStyle(attributes.get(name), control);
                }
            }
            attributes.clear();
        }
    }

    /**
     * 前设置属性
     *
     * @param node XML节点
     * @param control 属性对象
     */
    public void SetAttributesBefore(Node node, FCProperty control) {
        if (control != null) {
            HashMap<String, String> attributes = getAttributes(node);
            // 读取属性
            for (String name : attributes.keySet()) {
                if (!isAfterSetingAttribute(name)) {
                    control.setProperty(name, attributes.get(name));
                }
            }
            attributes.clear();
        }
    }

    /**
     * 设置事件
     *
     * @param node XML节点
     * @param control 属性对象
     */
    public void setEvents(Node node, FCProperty control) {
        FCView baseControl = (FCView) ((control instanceof FCView) ? control : null);
        if (baseControl != null) {
            HashMap<String, String> attributes = getAttributes(node);
            for (String strEvent : attributes.keySet()) {
                m_event.registerEvent(baseControl, strEvent, attributes.get(strEvent));
            }
            attributes.clear();
        }
    }

    public void setStyle(String style, FCProperty control) {
        boolean isStr = false;
        String str = "";
        for (char ch : style.toCharArray()) {
            if (ch == '\'') {
                isStr = !isStr;
            }
            if (!isStr) {
                if (ch == ';') {
                    int idx = str.indexOf(':');
                    String pName = str.substring(0, idx);
                    String pValue = str.substring(idx + 1);
                    control.setProperty(pName.toLowerCase(), pValue);
                    str = "";
                    continue;
                } else if (ch == ' ') {
                    continue;
                }
            }
            str += ch;
        }
    }
}
