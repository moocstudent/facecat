/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.div;

import facecat.topin.input.*;
import facecat.topin.scroll.*;
import facecat.topin.core.*;
import facecat.topin.grid.*;
import java.util.*;

/*
 * 树控件
 */
public class FCTree extends FCGrid {

    /*
     * 构造函数
     */
    public FCTree() {
        setGridLineColor(FCColor.None);
    }

    /**
     * 正在移动的节点
     */
    protected FCTreeNode m_movingNode;

    /*
     * 节点
     */
    public ArrayList<FCTreeNode> m_nodes = new ArrayList<FCTreeNode>();

    protected boolean m_checkBoxes = false;

    /**
     * 获取是否在节点旁显示复选框
     */
    public boolean hasCheckBoxes() {
        return m_checkBoxes;
    }

    /**
     * 设置是否在节点旁显示复选框
     */
    public void setCheckBoxes(boolean value) {
        m_checkBoxes = value;
    }

    protected FCSize m_checkBoxSize = new FCSize(14, 14);

    /**
     * 获取复选框的大小
     */
    public FCSize getCheckBoxSize() {
        return m_checkBoxSize.clone();
    }

    /**
     * 设置复选框的大小
     */
    public void setCheckBoxSize(FCSize value) {
        m_checkBoxSize = value.clone();
    }

    protected String m_checkedImage;

    /**
     * 获取复选框选中时的图片
     */
    public String getCheckedImage() {
        return m_checkedImage;
    }

    /**
     * 设置复选框选中时的图片
     */
    public void setCheckedImage(String value) {
        m_checkedImage = value;
    }

    protected String m_collapsedNodeImage;

    /**
     * 获取折叠节点的图片
     */
    public String getCollapsedNodeImage() {
        return m_collapsedNodeImage;
    }

    /**
     * 设置折叠节点的图片
     */
    public void setCollapsedNodeImage(String value) {
        m_collapsedNodeImage = value;
    }

    protected String m_expendedNodeImage;

    /**
     * 获取展开节点的图片
     */
    public String getExpendedNodeImage() {
        return m_expendedNodeImage;
    }

    /**
     * 设置展开节点的图片
     */
    public void setExpendedNodeImage(String value) {
        m_expendedNodeImage = value;
    }

    protected FCSize m_nodeSize = new FCSize(14, 14);

    /**
     * 获取节点的大小
     */
    public FCSize getNodeSize() {
        return m_nodeSize.clone();
    }

    /**
     * 设置节点的大小
     */
    public void setNodeSize(FCSize value) {
        m_nodeSize = value.clone();
    }

    /**
     * 获取选中的节点
     */
    public ArrayList<FCTreeNode> getSelectedNodes() {
        ArrayList<FCTreeNode> selectedNodes = new ArrayList<FCTreeNode>();
        ArrayList<FCGridRow> selectedRows = getSelectedRows();
        int selectedRowsSize = selectedRows.size();
        for (int i = 0; i < selectedRowsSize; i++) {
            ArrayList<FCGridCell> cells = selectedRows.get(i).m_cells;
            int cellsSize = cells.size();
            for (int j = 0; j < cellsSize; j++) {
                FCTreeNode node = (FCTreeNode) ((cells.get(i) instanceof FCTreeNode) ? cells.get(j) : null);
                if (node != null) {
                    selectedNodes.add(node);
                    break;
                }
            }
        }
        return selectedNodes;
    }

    /**
     * 设置选中的节点
     */
    public void setSelectedNodes(ArrayList<FCTreeNode> value) {
        int selectedNodesSize = value.size();
        ArrayList<FCGridRow> selectedRows = new ArrayList<FCGridRow>();
        for (int i = 0; i < selectedNodesSize; i++) {
            selectedRows.add(value.get(i).getRow());
        }
        setSelectedRows(selectedRows);
    }

    protected String m_unCheckedImage;

    /**
     * 获取复选框未选中时的图片
     */
    public String getUnCheckedImage() {
        return m_unCheckedImage;
    }

    /**
     * 设置复选框未选中时的图片
     */
    public void setUnCheckedImage(String value) {
        m_unCheckedImage = value;
    }

    /**
     * 添加节点
     */
    public void appendNode(FCTreeNode node) {
        node.setTree(this);
        node.onAddingNode(-1);
        m_nodes.add(node);
    }

    /**
     * 清除所有节点
     */
    public void clearNodes() {
        while (m_nodes.size() > 0) {
            removeNode(m_nodes.get(m_nodes.size() - 1));
        }
    }

    /**
     * 折叠节点
     */
    public void collapse() {
        int nodesSize = m_nodes.size();
        if (m_nodes.size() > 0) {
            for (int i = 0; i < nodesSize; i++) {
                m_nodes.get(i).collapse();
            }
        }
    }

    /**
     * 折叠所有节点
     */
    public void collapseAll() {
        int nodesSize = m_nodes.size();
        if (m_nodes.size() > 0) {
            for (int i = 0; i < nodesSize; i++) {
                m_nodes.get(i).collapseAll();
            }
        }
    }

    /**
     * 销毁数据
     */
    @Override
    public void delete() {
        if (!isDeleted()) {
            m_nodes.clear();
        }
        super.delete();
    }

    /**
     * 展开节点
     */
    public void expend() {
        int nodesSize = m_nodes.size();
        if (m_nodes.size() > 0) {
            for (int i = 0; i < nodesSize; i++) {
                m_nodes.get(i).expend();
            }
        }
    }

    /**
     * 展开所有节点
     */
    public void expendAll() {
        int nodesSize = m_nodes.size();
        if (m_nodes.size() > 0) {
            for (int i = 0; i < nodesSize; i++) {
                m_nodes.get(i).expendAll();
            }
        }
    }

    /**
     * 获取子节点
     */
    public ArrayList<FCTreeNode> getChildNodes() {
        return m_nodes;
    }

    /**
     * 获取控件类型
     */
    @Override
    public String getControlType() {
        return "Tree";
    }

    /**
     * 获取节点的索引
     */
    public int getNodeIndex(FCTreeNode node) {
        int nodeSize = m_nodes.size();
        for (int i = 0; i < nodeSize; i++) {
            if (m_nodes.get(i) == node) {
                return i;
            }
        }
        return -1;
    }

    /**
     * 获取属性值
     *
     * @param name 属性名称
     * @param value 返回属性值
     * @param type 返回属性类型
     */
    @Override
    public void getProperty(String name, RefObject<String> value, RefObject<String> type) {
        if (name.equals("checkboxes")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(hasCheckBoxes());
        } else if (name.equals("checkboxsize")) {
            type.argvalue = "size";
            value.argvalue = FCStr.convertSizeToStr(getCheckBoxSize());
        } else if (name.equals("checkedimage")) {
            type.argvalue = "string";
            value.argvalue = getCheckedImage();
        } else if (name.equals("collapsednodeimage")) {
            type.argvalue = "string";
            value.argvalue = getCollapsedNodeImage();
        } else if (name.equals("expendednodeimage")) {
            type.argvalue = "string";
            value.argvalue = getExpendedNodeImage();
        } else if (name.equals("uncheckedimage")) {
            type.argvalue = "string";
            value.argvalue = getUnCheckedImage();
        } else if (name.equals("nodesize")) {
            type.argvalue = "size";
            value.argvalue = FCStr.convertSizeToStr(getNodeSize());
        } else {
            super.getProperty(name, value, type);
        }
    }

    /**
     * 获取属性名称列表
     */
    @Override
    public ArrayList<String> getPropertyNames() {
        ArrayList<String> propertyNames = super.getPropertyNames();
        propertyNames.addAll(Arrays.asList(new String[]{"CheckBoxes", "CheckBoxSize", "CheckedImage", "CollapsedNodeImage", "ExpendedNodeImage", "UnCheckedImage", "NodeSize"}));
        return propertyNames;
    }

    /**
     * 插入节点
     */
    public void insertNode(int index, FCTreeNode node) {
        int rowIndex = -1;
        if (index == 0) {
            if (node.getParent() != null) {
                rowIndex = node.getParent().getRow().getIndex() + 1;
            } else {
                rowIndex = 0;
            }
        } else {
            if (m_nodes.size() > 0) {
                rowIndex = m_nodes.get(index).getRow().getIndex();
            }
        }
        node.setTree(this);
        node.onAddingNode(rowIndex);
        m_nodes.add(index, node);
    }

    /**
     * 单元格触摸按下方法
     */
    @Override
    public void onCellTouchDown(FCGridCell cell, FCTouchInfo touchInfo) {
        super.onCellTouchDown(cell, touchInfo.clone());
        FCPoint mp = touchInfo.m_firstPoint.clone();
        FCTreeNode node = (FCTreeNode) ((cell instanceof FCTreeNode) ? cell : null);
        if (node != null) {
            int scrollH = 0;
            FCHScrollBar hscrollBar = getHScrollBar();
            if (hscrollBar != null && hscrollBar.isVisible()) {
                scrollH = hscrollBar.getPos();
            }
            FCRect headerRect = node.getTargetColumn().getBounds();
            headerRect.left += getHorizontalOffset() - scrollH;
            headerRect.top += getVerticalOffset() - scrollH;
            int left = headerRect.left;

            if (m_checkBoxes) {
                int cw = m_checkBoxSize.cx;
                if (mp.x >= left && mp.x <= left + cw) {
                    node.setChecked(!node.isChecked());
                    return;
                }
                left += cw + 10;
            }

            ArrayList<FCTreeNode> childNodes = node.getChildNodes();
            if (childNodes != null && childNodes.size() > 0) {
                int nw = m_nodeSize.cx;
                if (mp.x >= left && mp.x <= left + nw) {
                    if (node.isExpended()) {
                        node.collapse();
                    } else {
                        node.expend();
                    }
                    update();
                    return;
                }
            }

            if (node.allowDragOut()) {
                m_movingNode = node;
            }
        }
    }

    /**
     * 单元格触摸移动方法
     */
    @Override
    public void onCellTouchMove(FCGridCell cell, FCTouchInfo touchInfo) {
        super.onCellTouchMove(cell, touchInfo.clone());
        if (m_movingNode != null) {
            invalidate();
        }
    }

    /**
     * 单元格触摸抬起方法
     */
    @Override
    public void onCellTouchUp(FCGridCell cell, FCTouchInfo touchInfo) {
        super.onCellTouchUp(cell, touchInfo.clone());
        FCPoint mp = touchInfo.m_firstPoint.clone();
        if (m_movingNode != null) {
            FCGridRow curRow = getRow(mp.clone());

            if (curRow != null) {
                FCGridCell tempVar = curRow.getCell(0);
                FCTreeNode curNode = (FCTreeNode) ((tempVar instanceof FCTreeNode) ? tempVar : null);
                if (curNode.allowDragIn() && m_movingNode != curNode) {
                    FCTreeNode curNodeParent = curNode.getParent();
                    FCTreeNode movingNodeParent = m_movingNode.getParent();
                    if (movingNodeParent != null) {
                        movingNodeParent.removeNode(m_movingNode);
                    } else {
                        removeNode(m_movingNode);
                    }

                    if (curNodeParent != null) {
                        if (movingNodeParent == curNodeParent) {
                            curNodeParent.insertNode(curNodeParent.getNodeIndex(curNode), m_movingNode);
                        } else {
                            curNode.appendNode(m_movingNode);
                        }
                    } else {
                        if (movingNodeParent == curNodeParent) {
                            insertNode(getNodeIndex(curNode), m_movingNode);
                        } else {
                            curNode.appendNode(m_movingNode);
                        }
                    }
                    curNode.expend();
                }
            }
            m_movingNode = null;
            update();
        }
    }

    /**
     * 重绘前景方法
     */
    @Override
    public void onPaintForeground(FCPaint paint, FCRect clipRect) {
        super.onPaintForeground(paint, clipRect);

        if (m_movingNode != null) {
            FCFont font = getFont();
            FCPoint mp = getTouchPoint();
            FCSize tSize = paint.textSize(m_movingNode.getText(), font);
            FCRect tRect = new FCRect(mp.x, mp.y, mp.x + tSize.cx, mp.y + tSize.cy);
            paint.drawText(m_movingNode.getText(), getTextColor(), font, tRect);
        }
    }

    /**
     * 绘制编辑文本框
     */
    @Override
    public void onPaintEditTextBox(FCGridCell cell, FCPaint paint, FCRect rect, FCRect clipRect) {
        FCTextBox editTextBox = getEditTextBox();
        if (editTextBox != null) {
            FCTreeNode node = (FCTreeNode) ((cell instanceof FCTreeNode) ? cell : null);
            if (node != null) {
                int indent = node.getIndent();
                rect.left += indent;
                if (rect.right < rect.left) {
                    rect.right = rect.left;
                }
                editTextBox.setBounds(rect);
                editTextBox.setDisplayOffset(false);
                editTextBox.bringToFront();
            } else {
                super.onPaintEditTextBox(cell, paint, rect, clipRect);
            }
        }
    }

    /**
     * 移除节点
     */
    public void removeNode(FCTreeNode node) {
        node.onRemovingNode();
        m_nodes.remove(node);
    }

    /**
     * 设置属性
     */
    @Override
    public void setProperty(String name, String value) {
        if (name.equals("checkboxes")) {
            setCheckBoxes(FCStr.convertStrToBool(value));
        } else if (name.equals("checkboxsize")) {
            setCheckBoxSize(FCStr.convertStrToSize(value));
        } else if (name.equals("checkedimage")) {
            setCheckedImage(value);
        } else if (name.equals("collapsednodeimage")) {
            setCollapsedNodeImage(value);
        } else if (name.equals("expendednodeimage")) {
            setExpendedNodeImage(value);
        } else if (name.equals("uncheckedimage")) {
            setUnCheckedImage(value);
        } else if (name.equals("nodesize")) {
            setNodeSize(FCStr.convertStrToSize(value));
        } else {
            super.setProperty(name, value);
        }
    }
}
