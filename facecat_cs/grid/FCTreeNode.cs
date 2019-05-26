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

namespace FaceCat {
    /// <summary>
    /// 树节点
    /// </summary>
    public class FCTreeNode : FCGridControlCell {
        /// <summary>
        /// 创建节点
        /// </summary>
        public FCTreeNode() {
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~FCTreeNode() {
            m_nodes.clear();
        }

        /// <summary>
        /// 子节点
        /// </summary>
        public ArrayList<FCTreeNode> m_nodes = new ArrayList<FCTreeNode>();

        /// <summary>
        /// 文字
        /// </summary>
        protected String m_text;

        protected bool m_allowDragIn = false;

        /// <summary>
        /// 获取或设置是否可以拖入节点
        /// </summary>
        public virtual bool AllowDragIn {
            get { return m_allowDragIn; }
            set { m_allowDragIn = value; }
        }

        protected bool m_allowDragOut = false;

        /// <summary>
        /// 获取或设置是否可以拖出节点
        /// </summary>
        public virtual bool AllowDragOut {
            get { return m_allowDragOut; }
            set { m_allowDragOut = value; }
        }

        protected bool m_checked = false;

        /// <summary>
        /// 获取或设置复选框是否选中
        /// </summary>
        public virtual bool Checked {
            get { return m_checked; }
            set {
                if (m_checked != value) {
                    m_checked = value;
                    checkChildNodes(m_nodes, m_checked);
                }
            }
        }

        protected bool m_expended = true;

        /// <summary>
        /// 获取或设置是否展开节点
        /// </summary>
        public virtual bool Expended {
            get { return m_expended; }
            set { m_expended = value; }
        }

        protected FCTreeNode m_parent;

        /// <summary>
        /// 获取或设置父节点
        /// </summary>
        public virtual FCTreeNode Parent {
            get { return m_parent; }
            set { m_parent = value; }
        }

        protected FCGridColumn m_targetColumn;

        /// <summary>
        /// 获取或设置目标列
        /// </summary>
        public virtual FCGridColumn TargetColumn {
            get { return m_targetColumn; }
            set { m_targetColumn = value; }
        }

        protected int m_indent;

        /// <summary>
        /// 获取文字缩进距离
        /// </summary>
        public virtual int Indent {
            get { return m_indent; }
        }

        protected FCTree m_tree;

        /// <summary>
        /// 获取或设置树控件
        /// </summary>
        public virtual FCTree Tree {
            get { return m_tree; }
            set { m_tree = value; }
        }

        protected String m_value;

        /// <summary>
        /// 获取或设置值
        /// </summary>
        public virtual String Value {
            get { return m_value; }
            set { m_value = value; }
        }

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="node">节点</param>
        public void appendNode(FCTreeNode node) {
            node.Parent = this;
            node.Tree = m_tree;
            node.onAddingNode(-1);
            m_nodes.add(node);
        }

        /// <summary>
        /// 选中所有子节点
        /// </summary>
        /// <param name="nodes">节点集合</param>
        /// <param name="isChecked">是否选中</param>
        protected void checkChildNodes(ArrayList<FCTreeNode> nodes, bool isChecked) {
            int nodeSize = m_nodes.size();
            for (int i = 0; i < nodeSize; i++) {
                FCTreeNode node = nodes.get(i);
                node.Checked = isChecked;
                ArrayList<FCTreeNode> childNodes = node.getChildNodes();
                if (childNodes != null && childNodes.size() > 0) {
                    checkChildNodes(childNodes, isChecked);
                }
            }
        }

        /// <summary>
        /// 清除所有节点
        /// </summary>
        public void clearNodes() {
            while (m_nodes.size() > 0) {
                removeNode(m_nodes.get(m_nodes.size() - 1));
            }
        }

        /// <summary>
        /// 折叠节点
        /// </summary>
        public void collapse() {
            if (m_nodes.size() > 0) {
                m_expended = false;
                collapseChildNodes(m_nodes, false);
            }
        }

        /// <summary>
        /// 折叠所有节点
        /// </summary>
        public void collapseAll() {
            if (m_nodes.size() > 0) {
                m_expended = false;
                collapseChildNodes(m_nodes, true);
            }
        }

        /// <summary>
        /// 折叠子节点
        /// </summary>
        /// <param name="nodes">节点集合</param>
        /// <param name="expendAll">是否折叠儿子节点</param>
        protected void collapseChildNodes(ArrayList<FCTreeNode> nodes, bool collapseAll) {
            int nodeSize = nodes.size();
            for (int i = 0; i < nodeSize; i++) {
                FCTreeNode node = nodes.get(i);
                if (collapseAll) {
                    node.Expended = false;
                }
                node.Row.Visible = false;
                ArrayList<FCTreeNode> childNodes = node.getChildNodes();
                if (childNodes != null && childNodes.size() > 0) {
                    collapseChildNodes(childNodes, collapseAll);
                }
            }
        }

        /// <summary>
        /// 展开节点
        /// </summary>
        public void expend() {
            if (m_nodes.size() > 0) {
                m_expended = true;
                expendChildNodes(m_nodes, true, false);
            }
        }

        /// <summary>
        /// 展开所有节点
        /// </summary>
        public void expendAll() {
            if (m_nodes.size() > 0) {
                m_expended = true;
                expendChildNodes(m_nodes, true, true);
            }
        }

        /// <summary>
        /// 展开所有的节点
        /// </summary>
        /// <param name="nodes">节点集合</param>
        /// <param name="parentExpened">父节点是否展开</param>
        /// <param name="expendAll">儿子节点是否展开</param>
        protected void expendChildNodes(ArrayList<FCTreeNode> nodes, bool parentExpened, bool expendAll) {
            int nodeSize = nodes.size();
            for (int i = 0; i < nodeSize; i++) {
                FCTreeNode node = nodes.get(i);
                bool pExpended = parentExpened;
                if (expendAll) {
                    pExpended = true;
                    node.Row.Visible = true;
                    node.Expended = true;
                }
                else {
                    if (parentExpened) {
                        node.Row.Visible = true;
                    }
                    else {
                        node.Row.Visible = false;
                    }
                    if (!node.Expended) {
                        pExpended = false;
                    }
                }
                ArrayList<FCTreeNode> childNodes = node.getChildNodes();
                if (childNodes != null && childNodes.size() > 0) {
                    expendChildNodes(childNodes, pExpended, expendAll);
                }
            }
        }

        /// <summary>
        /// 获取子节点
        /// </summary>
        /// <returns>子节点</returns>
        public ArrayList<FCTreeNode> getChildNodes() {
            return m_nodes;
        }

        /// <summary>
        /// 获取最后节点的索引
        /// </summary>
        /// <param name="nodes">节点</param>
        /// <returns>索引</returns>
        protected FCTreeNode getLastNode(ArrayList<FCTreeNode> nodes) {
            int size = nodes.size();
            if (size > 0) {
                for (int i = size - 1; i >= 0; i--) {
                    FCTreeNode lastNode = nodes.get(i);
                    if (lastNode.Row != null) {
                        ArrayList<FCTreeNode> childNodes = lastNode.getChildNodes();
                        FCTreeNode subLastNode = getLastNode(childNodes);
                        if (subLastNode != null) {
                            return subLastNode;
                        }
                        else {
                            return lastNode;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 获取节点的索引
        /// </summary>
        /// <param name="node">节点</param>
        /// <returns>索引</returns>
        public int getNodeIndex(FCTreeNode node) {
            int nodeSize = m_nodes.size();
            for (int i = 0; i < nodeSize; i++) {
                if (m_nodes.get(i) == node) {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 获取要绘制的文本
        /// </summary>
        /// <returns>要绘制的文本</returns>
        public override String getPaintText() {
            return Text;
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">返回属性值</param>
        /// <param name="type">返回属性类型</param>
        public override void getProperty(String name, ref String value, ref String type) {
            if (name == "allowdragin") {
                type = "bool";
                value = FCStr.convertBoolToStr(AllowDragIn);
            }
            else if (name == "allowdragout") {
                type = "bool";
                value = FCStr.convertBoolToStr(AllowDragOut);
            }
            else if (name == "checked") {
                type = "bool";
                value = FCStr.convertBoolToStr(Checked);
            }
            else if (name == "expended") {
                type = "bool";
                value = FCStr.convertBoolToStr(Expended);
            }
            else if (name == "value") {
                type = "String";
                value = Value;
            }
            else {
                base.getProperty(name, ref value, ref type);
            }
        }

        /// <summary>
        /// 获取属性名称列表
        /// </summary>
        /// <returns>属性名称列表</returns>
        public override ArrayList<String> getPropertyNames() {
            ArrayList<String> propertyNames = base.getPropertyNames();
            propertyNames.AddRange(new String[] { "AllowDragIn", "AllowDragOut", "Checked", "Expended", "Value" });
            return propertyNames;
        }

        /// <summary>
        /// 获取字符型数值
        /// </summary>
        /// <returns>字符型数值</returns>
        public override String getString() {
            return m_text;
        }

        /// <summary>
        /// 插入节点
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="node">节点</param>
        public void insertNode(int index, FCTreeNode node) {
            int rowIndex = -1;
            if (index == 0) {
                if (node.Parent != null) {
                    rowIndex = node.Parent.Row.Index + 1;
                }
                else {
                    rowIndex = 0;
                }
            }
            else {
                if (m_nodes.size() > 0) {
                    rowIndex = m_nodes.get(index).Row.Index;
                }
            }
            node.Tree = m_tree;
            node.Parent = this;
            node.onAddingNode(rowIndex);
            m_nodes.Insert(index, node);
        }

        /// <summary>
        /// 父节点是否可见
        /// </summary>
        /// <param name="node">节点</param>
        /// <returns>是否可见</returns>
        public bool isNodeVisible(FCTreeNode node) {
            FCTreeNode parentNode = node.Parent;
            if (parentNode != null) {
                if (!parentNode.Expended) {
                    return false;
                }
                else {
                    return isNodeVisible(parentNode);
                }
            }
            else {
                return true;
            }
        }

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="index">行索引</param>
        public virtual void onAddingNode(int index) {
            FCGridRow row = Row;
            if (Row == null) {
                //创建行
                row = new FCGridRow();
                FCTreeNode parentNode = m_parent;
                if (parentNode == null) {
                    if (index != -1) {
                        //插入行
                        m_tree.insertRow(index, row);
                        //重置行的索引
                        ArrayList<FCGridRow> rows = m_tree.getRows();
                        int rowSize = rows.size();
                        for (int i = 0; i < rowSize; i++) {
                            rows.get(i).Index = i;
                        }
                    }
                    else {
                        //添加行
                        m_tree.addRow(row);
                        //设置索引
                        ArrayList<FCGridRow> rows = m_tree.getRows();
                        row.Index = rows.size() - 1;
                    }
                    row.addCell(0, this);
                    m_targetColumn = m_tree.getColumn(0);
                }
                else {
                    //获取行索引
                    int rowIndex = parentNode.Row.Index + 1;
                    if (index != -1) {
                        rowIndex = index;
                    }
                    else {
                        //查找上个节点      
                        FCTreeNode lastNode = getLastNode(parentNode.getChildNodes());
                        if (lastNode != null) {
                            if (lastNode.Row == null) {
                                return;
                            }
                            rowIndex = lastNode.Row.Index + 1;
                        }
                    }
                    //插入行
                    m_tree.insertRow(rowIndex, row);
                    ArrayList<FCGridRow> rows = m_tree.getRows();
                    int rowSize = rows.size();
                    //重置索引
                    if (rowIndex == rowSize - 1) {
                        row.Index = rowIndex;
                    }
                    else {
                        for (int i = 0; i < rowSize; i++) {
                            rows.get(i).Index = i;
                        }
                    }
                    row.addCell(0, this);
                    m_targetColumn = m_tree.getColumn(parentNode.m_targetColumn.Index + 1);
                }
                ColSpan = m_tree.getColumns().size();
                //添加子节点
                if (m_nodes != null && m_nodes.size() > 0) {
                    int nodeSize = m_nodes.size();
                    for (int i = 0; i < nodeSize; i++) {
                        m_nodes.get(i).onAddingNode(-1);
                    }
                }
                row.Visible = isNodeVisible(this);
            }
        }

        /// <summary>
        /// 绘制复选框
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="rect">区域</param>
        public virtual void onPaintCheckBox(FCPaint paint, FCRect rect) {
            if (m_checked) {
                if (m_tree.CheckedImage != null && m_tree.CheckedImage.Length > 0) {
                    paint.drawImage(m_tree.CheckedImage, rect);
                }
                else {
                    paint.fillRect(FCColor.argb(0, 0, 0), rect);
                }
            }
            else {
                if (m_tree.UnCheckedImage != null && m_tree.UnCheckedImage.Length > 0) {
                    paint.drawImage(m_tree.UnCheckedImage, rect);
                }
                else {
                    paint.drawRect(FCColor.argb(0, 0, 0), 1, 0, rect);
                }
            }
        }

        /// <summary>
        /// 绘制节点
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="rect">区域</param>
        public virtual void onPaintNode(FCPaint paint, FCRect rect) {
            if (m_expended) {
                if (m_tree.ExpendedNodeImage != null && m_tree.ExpendedNodeImage.Length > 0) {
                    paint.drawImage(m_tree.ExpendedNodeImage, rect);
                    return;
                }
            }
            else {
                if (m_tree.CollapsedNodeImage != null && m_tree.CollapsedNodeImage.Length > 0) {
                    paint.drawImage(m_tree.CollapsedNodeImage, rect);
                    return;
                }
            }
            int width = rect.right - rect.left;
            int height = rect.bottom - rect.top;
            FCPoint[] points = new FCPoint[3];
            //展开
            if (m_expended) {
                points[0] = new FCPoint(rect.left, rect.top);
                points[1] = new FCPoint(rect.left + width, rect.top);
                points[2] = new FCPoint(rect.left + width / 2, rect.top + height);
            }
            //折叠
            else {
                points[0] = new FCPoint(rect.left, rect.top);
                points[1] = new FCPoint(rect.left, rect.top + height);
                points[2] = new FCPoint(rect.left + width, rect.top + height / 2);
            }
            FCGrid grid = Grid;
            paint.fillPolygon(grid.TextColor, points);
        }

        /// <summary>
        /// 重绘方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="rect">矩形</param>
        /// <param name="clipRect">裁剪矩形</param>
        /// <param name="isAlternate">是否交替行</param>
        public override void onPaint(FCPaint paint, FCRect rect, FCRect clipRect, bool isAlternate) {
            int clipW = clipRect.right - clipRect.left;
            int clipH = clipRect.bottom - clipRect.top;
            FCGrid grid = Grid;
            FCGridRow row = Row;
            if (clipW > 0 && clipH > 0 && grid != null && Column != null && row != null && TargetColumn != null) {
                int width = rect.right - rect.left;
                int height = rect.bottom - rect.top;
                int scrollH = 0;
                FCHScrollBar hscrollBar = grid.HScrollBar;
                if (hscrollBar != null && hscrollBar.Visible) {
                    scrollH = hscrollBar.Pos;
                }
                FCFont font = null;
                long backColor = FCColor.None;
                long textColor = FCColor.None;
                bool autoEllipsis = m_tree.AutoEllipsis;
                FCGridCellStyle style = Style;
                if (style != null) {
                    if (style.AutoEllipsis) {
                        autoEllipsis = style.AutoEllipsis;
                    }
                    backColor = style.BackColor;
                    if (style.Font != null) {
                        font = style.Font;
                    }
                    textColor = style.TextColor;
                }
                FCGridRowStyle rowStyle = grid.RowStyle;
                if (isAlternate) {
                    FCGridRowStyle alternateRowStyle = grid.AlternateRowStyle;
                    if (alternateRowStyle != null) {
                        rowStyle = alternateRowStyle;
                    }
                }
                if (rowStyle != null) {
                    bool selected = false;
                    ArrayList<FCGridRow> selectedRows = grid.SelectedRows;
                    int selectedRowsSize = selectedRows.size();
                    for (int i = 0; i < selectedRowsSize; i++) {
                        if (selectedRows[i] == row) {
                            selected = true;
                            break;
                        }
                    }
                    if (backColor == FCColor.None) {
                        //选中
                        if (selected) {
                            backColor = rowStyle.SelectedBackColor;
                        }
                        //悬停
                        else if (Row == Grid.HoveredRow) {
                            backColor = rowStyle.HoveredBackColor;
                        }
                        //普通
                        else {
                            backColor = rowStyle.BackColor;
                        }
                    }
                    if (font == null) {
                        font = rowStyle.Font;
                    }
                    if (textColor == FCColor.None) {
                        //选中
                        if (selected) {
                            textColor = rowStyle.SelectedTextColor;
                        }
                        //悬停
                        else if (Row == Grid.HoveredRow) {
                            textColor = rowStyle.HoveredTextColor;
                        }
                        //普通
                        else {
                            textColor = rowStyle.TextColor;
                        }
                    }
                }
                //绘制背景
                paint.fillRect(backColor, rect);
                FCRect headerRect = TargetColumn.Bounds;
                headerRect.left += Grid.HorizontalOffset - scrollH;
                headerRect.top += Grid.VerticalOffset - scrollH;
                int left = headerRect.left;
                //绘制复选框
                if (m_tree.CheckBoxes) {
                    int cw = m_tree.CheckBoxSize.cx;
                    int ch = m_tree.CheckBoxSize.cy;
                    FCRect checkBoxRect = new FCRect();
                    checkBoxRect.left = left;
                    checkBoxRect.top = rect.top + (height - ch) / 2;
                    checkBoxRect.right = checkBoxRect.left + cw;
                    checkBoxRect.bottom = checkBoxRect.top + ch;
                    onPaintCheckBox(paint, checkBoxRect);
                    left += cw + 10;
                }
                //绘制折叠展开的标志
                int nw = m_tree.NodeSize.cx;
                int nh = m_tree.NodeSize.cy;
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
                //绘制文字
                if (text != null) {
                    FCSize tSize = paint.textSize(text, font);
                    FCRect tRect = new FCRect();
                    tRect.left = left;
                    tRect.top = rect.top + (row.Height - tSize.cy) / 2;
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
                    }
                    else {
                        paint.drawText(text, textColor, font, tRect);
                    }
                }
            }
            onPaintControl(paint, rect, clipRect);
        }

        /// <summary>
        /// 移除节点方法
        /// </summary>
        public virtual void onRemovingNode() {
            m_indent = 0;
            FCGridRow row = Row;
            if (row != null) {
                if (m_nodes != null && m_nodes.size() > 0) {
                    int nodeSize = m_nodes.size();
                    for (int i = 0; i < nodeSize; i++) {
                        m_nodes.get(i).onRemovingNode();
                    }
                }
                m_tree.removeRow(row);
                row.clearCells();
                Row = null;
                ArrayList<FCGridRow> rows = m_tree.getRows();
                int rowSize = rows.size();
                for (int i = 0; i < rowSize; i++) {
                    rows.get(i).Index = i;
                }
                m_targetColumn = null;
            }
        }

        /// <summary>
        /// 移除节点
        /// </summary>
        /// <param name="node">节点</param>
        public void removeNode(FCTreeNode node) {
            node.onRemovingNode();
            m_nodes.remove(node);
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name">属性名</param>
        /// <param name="value">属性值</param>
        public override void setProperty(String name, String value) {
            if (name == "allowdragin") {
                AllowDragIn = FCStr.convertStrToBool(value);
            }
            else if (name == "allowdragout") {
                AllowDragOut = FCStr.convertStrToBool(value);
            }
            else if (name == "checked") {
                Checked = FCStr.convertStrToBool(value);
            }
            else if (name == "expended") {
                Expended = FCStr.convertStrToBool(value);
            }
            else if (name == "value") {
                Value = value;
            }
            else {
                base.setProperty(name, value);
            }
        }

        /// <summary>
        /// 设置字符型数值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setString(String value) {
            m_text = value;
        }
    }
}
