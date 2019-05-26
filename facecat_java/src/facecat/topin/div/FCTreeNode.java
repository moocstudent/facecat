/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.div;

import facecat.topin.scroll.*;
import facecat.topin.core.*;
import facecat.topin.grid.*;
import java.util.*;

/*
 * 树节点
 */
public class FCTreeNode extends FCGridControlCell {

    /*
     * 构造函数
     */
    public FCTreeNode() {
    }

    /*
     * 析构函数
     */
    protected void finalize() throws Throwable {
        m_nodes.clear();
    }

    /**
     * 子节点
     */
    public ArrayList<FCTreeNode> m_nodes = new ArrayList<FCTreeNode>();

    /**
     * 文字
     */
    protected String m_text;

    protected boolean m_allowDragIn = false;

    /**
     * 获取是否可以拖入节点
     */
    public boolean allowDragIn() {
        return m_allowDragIn;
    }

    /**
     * 设置是否可以拖入节点
     */
    public void setAllowDragIn(boolean value) {
        m_allowDragIn = value;
    }

    protected boolean m_allowDragOut = false;

    /**
     * 获取是否可以拖出节点
     */
    public boolean allowDragOut() {
        return m_allowDragOut;
    }

    /**
     * 设置是否可以拖出节点
     */
    public void setAllowDragOut(boolean value) {
        m_allowDragOut = value;
    }

    protected boolean m_checked = false;

    /**
     * 获取复选框是否选中
     */
    public boolean isChecked() {
        return m_checked;
    }

    /**
     * 设置复选框是否选中
     */
    public void setChecked(boolean value) {
        if (m_checked != value) {
            m_checked = value;
            checkChildNodes(m_nodes, m_checked);
        }
    }

    protected boolean m_expended = true;

    /**
     * 获取是否展开节点
     */
    public boolean isExpended() {
        return m_expended;
    }

    /**
     * 设置是否展开节点
     */
    public void setExpended(boolean value) {
        m_expended = value;
    }

    protected FCTreeNode m_parent = null;

    /**
     * 获取父节点
     */
    public FCTreeNode getParent() {
        return m_parent;
    }

    /**
     * 设置父节点
     */
    public void setParent(FCTreeNode value) {
        m_parent = value;
    }

    protected FCGridColumn m_targetColumn;

    /**
     * 获取目标列
     */
    public FCGridColumn getTargetColumn() {
        return m_targetColumn;
    }

    /**
     * 设置目标列
     */
    public void setTargetColumn(FCGridColumn value) {
        m_targetColumn = value;
    }

    protected int m_indent;

    /**
     * 获取文字缩进距离
     */
    public int getIndent() {
        return m_indent;
    }

    protected FCTree m_tree = null;

    /**
     * 获取树控件
     */
    public FCTree getTree() {
        return m_tree;
    }

    /**
     * 设置树控件
     */
    public void setTree(FCTree value) {
        m_tree = value;
    }

    protected String m_value;

    /**
     * 获取值
     */
    public String getValue() {
        return m_value;
    }

    /**
     * 设置值
     */
    public void setValue(String value) {
        m_value = value;
    }

    /**
     * 添加节点
     */
    public void appendNode(FCTreeNode node) {
        node.setParent(this);
        node.setTree(m_tree);
        node.onAddingNode(-1);
        m_nodes.add(node);
    }

    /**
     * 选中所有子节点
     */
    protected void checkChildNodes(ArrayList<FCTreeNode> nodes, boolean isChecked) {
        int nodeSize = m_nodes.size();
        for (int i = 0; i < nodeSize; i++) {
            FCTreeNode node = nodes.get(i);
            node.setChecked(isChecked);
            ArrayList<FCTreeNode> childNodes = node.getChildNodes();
            if (childNodes != null && childNodes.size() > 0) {
                checkChildNodes(childNodes, isChecked);
            }
        }
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
        if (m_nodes.size() > 0) {
            m_expended = false;
            collapseChildNodes(m_nodes, false);
        }
    }

    /**
     * 折叠所有节点
     */
    public void collapseAll() {
        if (m_nodes.size() > 0) {
            m_expended = false;
            collapseChildNodes(m_nodes, true);
        }
    }

    /**
     * 折叠子节点
     */
    protected void collapseChildNodes(ArrayList<FCTreeNode> nodes, boolean collapseAll) {
        int nodeSize = nodes.size();
        for (int i = 0; i < nodeSize; i++) {
            FCTreeNode node = nodes.get(i);
            if (collapseAll) {
                node.setExpended(false);
            }
            node.getRow().setVisible(false);
            ArrayList<FCTreeNode> childNodes = node.getChildNodes();
            if (childNodes != null && childNodes.size() > 0) {
                collapseChildNodes(childNodes, collapseAll);
            }
        }
    }

    /**
     * 展开节点
     */
    public void expend() {
        if (m_nodes.size() > 0) {
            m_expended = true;
            expendChildNodes(m_nodes, true, false);
        }
    }

    /**
     * 展开所有节点
     */
    public void expendAll() {
        if (m_nodes.size() > 0) {
            m_expended = true;
            expendChildNodes(m_nodes, true, true);
        }
    }

    /**
     * 展开所有的节点
     */
    protected void expendChildNodes(ArrayList<FCTreeNode> nodes, boolean parentExpened, boolean expendAll) {
        int nodeSize = nodes.size();
        for (int i = 0; i < nodeSize; i++) {
            FCTreeNode node = nodes.get(i);
            boolean pExpended = parentExpened;
            if (expendAll) {
                pExpended = true;
                node.getRow().setVisible(true);
                node.setExpended(true);
            } else {
                if (parentExpened) {
                    node.getRow().setVisible(true);
                } else {
                    node.getRow().setVisible(false);
                }
                if (!node.isExpended()) {
                    pExpended = false;
                }
            }
            ArrayList<FCTreeNode> childNodes = node.getChildNodes();
            if (childNodes != null && childNodes.size() > 0) {
                expendChildNodes(childNodes, pExpended, expendAll);
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
     * 获取最后节点的索引
     */
    protected FCTreeNode getLastNode(ArrayList<FCTreeNode> nodes) {
        int size = nodes.size();
        if (size > 0) {
            for (int i = size - 1; i >= 0; i--) {
                FCTreeNode lastNode = nodes.get(i);
                if (lastNode.getRow() != null) {
                    ArrayList<FCTreeNode> childNodes = lastNode.getChildNodes();
                    FCTreeNode subLastNode = getLastNode(childNodes);
                    if (subLastNode != null) {
                        return subLastNode;
                    } else {
                        return lastNode;
                    }
                }
            }
        }
        return null;
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
     * 获取要绘制的文本
     */
    @Override
    public String getPaintText() {
        return getText();
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
        if (name.equals("allowdragin")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(allowDragIn());
        } else if (name.equals("allowdragout")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(allowDragOut());
        } else if (name.equals("checked")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(isChecked());
        } else if (name.equals("expended")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(isExpended());
        } else if (name.equals("value")) {
            type.argvalue = "string";
            value.argvalue = getValue();
        } else {
            super.getProperty(name, value, type);
        }
    }

    @Override
    public ArrayList<String> getPropertyNames() {
        ArrayList<String> propertyNames = super.getPropertyNames();
        propertyNames.addAll(Arrays.asList(new String[]{"AllowDragIn", "AllowDragOut", "Checked", "Expended", "Value"}));
        return propertyNames;
    }

    /**
     * 获取字符型数值
     */
    @Override
    public String getString() {
        return m_text;
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
        node.setTree(m_tree);
        node.setParent(this);
        node.onAddingNode(rowIndex);
        m_nodes.add(index, node);
    }

    /**
     * 父节点是否可见
     */
    public boolean isNodeVisible(FCTreeNode node) {
        FCTreeNode parentNode = node.getParent();
        if (parentNode != null) {
            if (!parentNode.isExpended()) {
                return false;
            } else {
                return isNodeVisible(parentNode);
            }
        } else {
            return true;
        }
    }

    /**
     * 添加节点
     */
    public void onAddingNode(int index) {
        FCGridRow row = getRow();
        if (getRow() == null) {
            row = new FCGridRow();
            FCTreeNode parentNode = m_parent;
            if (parentNode == null) {
                if (index != -1) {
                    m_tree.insertRow(index, row);
                    ArrayList<FCGridRow> rows = m_tree.getRows();
                    int rowSize = rows.size();
                    for (int i = 0; i < rowSize; i++) {
                        rows.get(i).setIndex(i);
                    }
                } else {
                    m_tree.addRow(row);
                    ArrayList<FCGridRow> rows = m_tree.getRows();
                    row.setIndex(rows.size() - 1);
                }
                row.addCell(0, this);
                m_targetColumn = m_tree.getColumn(0);
            } else {
                int rowIndex = parentNode.getRow().getIndex() + 1;
                if (index != -1) {
                    rowIndex = index;
                } else {
                    FCTreeNode lastNode = getLastNode(parentNode.getChildNodes());
                    if (lastNode != null) {
                        if (lastNode.getRow() == null) {
                            return;
                        }
                        rowIndex = lastNode.getRow().getIndex() + 1;
                    }
                }

                m_tree.insertRow(rowIndex, row);
                ArrayList<FCGridRow> rows = m_tree.getRows();
                int rowSize = rows.size();
                if (rowIndex == rowSize - 1) {
                    row.setIndex(rowIndex);
                } else {
                    for (int i = 0; i < rowSize; i++) {
                        rows.get(i).setIndex(i);
                    }
                }
                row.addCell(0, this);
                m_targetColumn = m_tree.getColumn(parentNode.m_targetColumn.getIndex() + 1);
            }
            setColSpan(m_tree.getColumns().size());
            if (m_nodes != null && m_nodes.size() > 0) {
                int nodeSize = m_nodes.size();
                for (int i = 0; i < nodeSize; i++) {
                    m_nodes.get(i).onAddingNode(-1);
                }
            }
            row.setVisible(isNodeVisible(this));
        }
    }

    /**
     * 绘制复选框
     */
    public void onPaintCheckBox(FCPaint paint, FCRect rect) {
        if (m_checked) {
            if (m_tree.getCheckedImage() != null && m_tree.getCheckedImage().length() > 0) {
                paint.drawImage(m_tree.getCheckedImage(), rect);
            } else {
                paint.fillRect(FCColor.argb(0, 0, 0), rect);
            }
        } else {
            if (m_tree.getUnCheckedImage() != null && m_tree.getUnCheckedImage().length() > 0) {
                paint.drawImage(m_tree.getUnCheckedImage(), rect);
            } else {
                paint.drawRect(FCColor.argb(0, 0, 0), 1, 0, rect);
            }
        }
    }

    /**
     * 绘制节点
     */
    public void onPaintNode(FCPaint paint, FCRect rect) {
        if (m_expended) {
            if (m_tree.getExpendedNodeImage() != null && m_tree.getExpendedNodeImage().length() > 0) {
                paint.drawImage(m_tree.getExpendedNodeImage(), rect);
                return;
            }
        } else {
            if (m_tree.getCollapsedNodeImage() != null && m_tree.getCollapsedNodeImage().length() > 0) {
                paint.drawImage(m_tree.getCollapsedNodeImage(), rect);
                return;
            }
        }
        int width = rect.right - rect.left;
        int height = rect.bottom - rect.top;
        FCPoint[] points = new FCPoint[3];

        if (m_expended) {
            points[0] = new FCPoint(rect.left, rect.top);
            points[1] = new FCPoint(rect.left + width, rect.top);
            points[2] = new FCPoint(rect.left + width / 2, rect.top + height);
        } else {
            points[0] = new FCPoint(rect.left, rect.top);
            points[1] = new FCPoint(rect.left, rect.top + height);
            points[2] = new FCPoint(rect.left + width, rect.top + height / 2);
        }
        FCGrid grid = getGrid();
        paint.fillPolygon(grid.getTextColor(), points);
    }

    /**
     * 重绘方法
     */
    @Override
    public void onPaint(FCPaint paint, FCRect rect, FCRect clipRect, boolean isAlternate) {
        int clipW = clipRect.right - clipRect.left;
        int clipH = clipRect.bottom - clipRect.top;
        FCGrid grid = getGrid();
        FCGridRow row = getRow();
        if (clipW > 0 && clipH > 0 && grid != null && getColumn() != null && row != null && getTargetColumn() != null) {
            int width = rect.right - rect.left;
            int height = rect.bottom - rect.top;
            int scrollH = 0;
            FCHScrollBar hscrollBar = grid.getHScrollBar();
            if (hscrollBar != null && hscrollBar.isVisible()) {
                scrollH = hscrollBar.getPos();
            }
            FCFont font = null;
            long backColor = FCColor.None;
            long textColor = FCColor.None;
            boolean autoEllipsis = m_tree.autoEllipsis();
            FCGridCellStyle style = getStyle();
            if (style != null) {
                if (style.autoEllipsis()) {
                    autoEllipsis = style.autoEllipsis();
                }
                backColor = style.getBackColor();
                if (style.getFont() != null) {
                    font = style.getFont();
                }
                textColor = style.getTextColor();
            }
            FCGridRowStyle rowStyle = grid.getRowStyle();
            if (isAlternate) {
                FCGridRowStyle alternateRowStyle = grid.getAlternateRowStyle();
                if (alternateRowStyle != null) {
                    rowStyle = alternateRowStyle;
                }
            }
            if (rowStyle != null) {
                boolean selected = false;
                ArrayList<FCGridRow> selectedRows = grid.getSelectedRows();
                int selectedRowsSize = selectedRows.size();
                for (int i = 0; i < selectedRowsSize; i++) {
                    if (selectedRows.get(i) == row) {
                        selected = true;
                        break;
                    }
                }
                if (backColor == FCColor.None) {
                    if (selected) {
                        backColor = rowStyle.getSelectedBackColor();
                    } else if (getRow() == getGrid().getHoveredRow()) {
                        backColor = rowStyle.getHoveredBackColor();
                    } else {
                        backColor = rowStyle.getBackColor();
                    }
                }
                if (font == null) {
                    font = rowStyle.getFont();
                }
                if (textColor == FCColor.None) {
                    if (selected) {
                        textColor = rowStyle.getSelectedTextColor();
                    } else if (getRow() == getGrid().getHoveredRow()) {
                        textColor = rowStyle.getHoveredTextColor();
                    } else {
                        textColor = rowStyle.getTextColor();
                    }
                }
            }

            paint.fillRect(backColor, rect);
            FCRect headerRect = getTargetColumn().getBounds();
            headerRect.left += getGrid().getHorizontalOffset() - scrollH;
            headerRect.top += getGrid().getVerticalOffset() - scrollH;
            int left = headerRect.left;
            if (m_tree.hasCheckBoxes()) {
                int cw = m_tree.getCheckBoxSize().cx;
                int ch = m_tree.getCheckBoxSize().cy;
                FCRect checkBoxRect = new FCRect();
                checkBoxRect.left = left;
                checkBoxRect.top = rect.top + (height - ch) / 2;
                checkBoxRect.right = checkBoxRect.left + cw;
                checkBoxRect.bottom = checkBoxRect.top + ch;
                onPaintCheckBox(paint, checkBoxRect);
                left += cw + 10;
            }

            int nw = m_tree.getNodeSize().cx;
            int nh = m_tree.getNodeSize().cy;
            if (m_nodes.size() > 0) {
                FCRect nodeRect = new FCRect();
                nodeRect.left = left;
                nodeRect.top = rect.top + (height - nh) / 2;
                nodeRect.right = nodeRect.left + nw;
                nodeRect.bottom = nodeRect.top + nh;
                onPaintNode(paint, nodeRect);
            }
            left += nw + 10;
            m_indent = left;
            String text = getPaintText();
            if (text != null) {
                FCSize tSize = paint.textSize(text, font);
                FCRect tRect = new FCRect();
                tRect.left = left;
                tRect.top = rect.top + (row.getHeight() - tSize.cy) / 2;
                tRect.right = tRect.left + tSize.cx;
                tRect.bottom = tRect.top + tSize.cy;
                if (autoEllipsis && (tRect.right < clipRect.right || tRect.bottom < clipRect.bottom)) {
                    if (tRect.right < clipRect.right) {
                        tRect.right = clipRect.right;
                    }
                    if (tRect.bottom < clipRect.bottom) {
                        tRect.bottom = clipRect.bottom;
                    }
                    paint.drawTextAutoEllipsis(text, textColor, font, tRect);
                } else {
                    paint.drawText(text, textColor, font, tRect);
                }
            }
        }
        onPaintControl(paint, rect, clipRect);
    }

    /**
     * 移除节点方法
     */
    public void onRemovingNode() {
        m_indent = 0;
        FCGridRow row = getRow();
        if (row != null) {
            if (m_nodes != null && m_nodes.size() > 0) {
                int nodeSize = m_nodes.size();
                for (int i = 0; i < nodeSize; i++) {
                    m_nodes.get(i).onRemovingNode();
                }
            }
            m_tree.removeRow(row);
            row.clearCells();
            setRow(null);
            ArrayList<FCGridRow> rows = m_tree.getRows();
            int rowSize = rows.size();
            for (int i = 0; i < rowSize; i++) {
                rows.get(i).setIndex(i);
            }
            m_targetColumn = null;
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
     *
     * @param name 属性名称
     * @param value 属性值
     */
    @Override
    public void setProperty(String name, String value) {
        if (name.equals("allowdragin")) {
            setAllowDragIn(FCStr.convertStrToBool(value));
        } else if (name.equals("allowdragout")) {
            setAllowDragOut(FCStr.convertStrToBool(value));
        } else if (name.equals("checked")) {
            setChecked(FCStr.convertStrToBool(value));
        } else if (name.equals("expended")) {
            setExpended(FCStr.convertStrToBool(value));
        } else if (name.equals("value")) {
            setValue(value);
        } else {
            super.setProperty(name, value);
        }
    }

    /**
     * 设置字符型数值
     */
    @Override
    public void setString(String value) {
        m_text = value;
    }
}
