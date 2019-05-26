/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

using System;
using System.Collections.Generic;

namespace FaceCat {
    /// <summary>
    /// 树控件
    /// </summary>
    public class FCTree : FCGrid {
        /// <summary>
        /// 创建树
        /// </summary>
        public FCTree() {
            GridLineColor = FCColor.None;
        }

        /// <summary>
        /// 正在移动的节点
        /// </summary>
        protected FCTreeNode m_movingNode;

        /// <summary>
        /// 子节点
        /// </summary>
        public ArrayList<FCTreeNode> m_nodes = new ArrayList<FCTreeNode>();

        protected bool m_checkBoxes;

        /// <summary>
        /// 获取或设置是否在节点旁显示复选框
        /// </summary>
        public virtual bool CheckBoxes {
            get { return m_checkBoxes; }
            set { m_checkBoxes = value; }
        }

        protected FCSize m_checkBoxSize = new FCSize(14, 14);

        /// <summary>
        /// 获取或设置复选框的大小
        /// </summary>
        public virtual FCSize CheckBoxSize {
            get { return m_checkBoxSize; }
            set { m_checkBoxSize = value; }
        }

        protected String m_checkedImage;

        /// <summary>
        /// 获取或设置复选框选中时的图片
        /// </summary>
        public virtual String CheckedImage {
            get { return m_checkedImage; }
            set { m_checkedImage = value; }
        }

        protected String m_collapsedNodeImage;

        /// <summary>
        /// 获取或设置折叠节点的图片
        /// </summary>
        public virtual String CollapsedNodeImage {
            get { return m_collapsedNodeImage; }
            set { m_collapsedNodeImage = value; }
        }

        protected String m_expendedNodeImage;

        /// <summary>
        /// 获取或设置展开节点的图片
        /// </summary>
        public virtual String ExpendedNodeImage {
            get { return m_expendedNodeImage; }
            set { m_expendedNodeImage = value; }
        }

        protected FCSize m_nodeSize = new FCSize(14, 14);

        /// <summary>
        /// 获取或设置节点的大小
        /// </summary>
        public virtual FCSize NodeSize {
            get { return m_nodeSize; }
            set { m_nodeSize = value; }
        }

        /// <summary>
        /// 获取或设置选中的节点
        /// </summary>
        public virtual ArrayList<FCTreeNode> SelectedNodes {
            get {
                ArrayList<FCTreeNode> selectedNodes = new ArrayList<FCTreeNode>();
                ArrayList<FCGridRow> selectedRows = SelectedRows;
                int selectedRowsSize = selectedRows.size();
                for (int i = 0; i < selectedRowsSize; i++) {
                    ArrayList<FCGridCell> cells = selectedRows.get(i).m_cells;
                    int cellsSize = cells.size();
                    for (int j = 0; j < cellsSize; j++) {
                        FCTreeNode node = cells.get(j) as FCTreeNode;
                        if (node != null) {
                            selectedNodes.add(node);
                            break;
                        }
                    }
                }
                return selectedNodes;
            }
            set {
                int selectedNodesSize = value.size();
                ArrayList<FCGridRow> selectedRows = new ArrayList<FCGridRow>();
                for (int i = 0; i < selectedNodesSize; i++) {
                    selectedRows.add(value.get(i).Row);
                }
                SelectedRows = selectedRows;
            }
        }

        protected String m_unCheckedImage;

        /// <summary>
        /// 获取或设置复选框未选中时的图片
        /// </summary>
        public virtual String UnCheckedImage {
            get { return m_unCheckedImage; }
            set { m_unCheckedImage = value; }
        }

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="node">节点</param>
        public void appendNode(FCTreeNode node) {
            node.Tree = this;
            node.onAddingNode(-1);
            m_nodes.add(node);
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
            int nodesSize = m_nodes.size();
            if (nodesSize > 0) {
                for (int i = 0; i < nodesSize; i++) {
                    m_nodes.get(i).collapse();
                }
            }
        }

        /// <summary>
        /// 折叠节点
        /// </summary>
        public void CollapseAll() {
            int nodesSize = m_nodes.size();
            if (nodesSize > 0) {
                for (int i = 0; i < nodesSize; i++) {
                    m_nodes.get(i).collapseAll();
                }
            }
        }

        /// <summary>
        /// 销毁资源
        /// </summary>
        public override void delete() {
            if (!IsDeleted) {
                m_nodes.clear();
            }
            base.delete();
        }

        /// <summary>
        /// 展开节点
        /// </summary>
        public void expend() {
            int nodesSize = m_nodes.size();
            if (nodesSize > 0) {
                for (int i = 0; i < nodesSize; i++) {
                    m_nodes.get(i).expend();
                }
            }
        }

        /// <summary>
        /// 展开节点
        /// </summary>
        public void expendAll() {
            int nodesSize = m_nodes.size();
            if (nodesSize > 0) {
                for (int i = 0; i < nodesSize; i++) {
                    m_nodes.get(i).expendAll();
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
        /// 获取控件类型
        /// </summary>
        /// <returns>控件类型</returns>
        public override String getControlType() {
            return "Tree";
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
        /// 获取属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">返回属性值</param>
        /// <param name="type">返回属性类型</param>
        public override void getProperty(String name, ref String value, ref String type) {
            if (name == "checkboxes") {
                type = "bool";
                value = FCStr.convertBoolToStr(CheckBoxes);
            }
            else if (name == "checkboxsize") {
                type = "size";
                value = FCStr.convertSizeToStr(CheckBoxSize);
            }
            else if (name == "checkedimage") {
                type = "String";
                value = CheckedImage;
            }
            else if (name == "collapsednodeimage") {
                type = "String";
                value = CollapsedNodeImage;
            }
            else if (name == "expendednodeimage") {
                type = "String";
                value = ExpendedNodeImage;
            }
            else if (name == "uncheckedimage") {
                type = "String";
                value = UnCheckedImage;
            }
            else if (name == "nodesize") {
                type = "size";
                value = FCStr.convertSizeToStr(NodeSize);
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
            propertyNames.AddRange(new String[] { "CheckBoxes", "CheckBoxSize", "CheckedImage", "CollapsedNodeImage", "ExpendedNodeImage", "UnCheckedImage", "NodeSize" });
            return propertyNames;
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
            node.Tree = this;
            node.onAddingNode(rowIndex);
            m_nodes.Insert(index, node);
        }

        /// <summary>
        /// 单元格触摸按下方法
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="touchInfo">触摸信息</param>
        public override void onCellTouchDown(FCGridCell cell, FCTouchInfo touchInfo) {
            base.onCellTouchDown(cell, touchInfo);
            FCPoint mp = touchInfo.m_firstPoint;
            FCTreeNode node = cell as FCTreeNode;
            if (node != null) {
                int scrollH = 0;
                FCHScrollBar hscrollBar = HScrollBar;
                if (hscrollBar != null && hscrollBar.Visible) {
                    scrollH = hscrollBar.Pos;
                }
                FCRect headerRect = node.TargetColumn.Bounds;
                headerRect.left += HorizontalOffset - scrollH;
                headerRect.top += VerticalOffset - scrollH;
                int left = headerRect.left;
                //复选框
                if (m_checkBoxes) {
                    int cw = m_checkBoxSize.cx;
                    if (mp.x >= left && mp.x <= left + cw) {
                        node.Checked = !node.Checked;
                        return;
                    }
                    left += cw + 10;
                }
                //折叠节点
                ArrayList<FCTreeNode> childNodes = node.getChildNodes();
                if (childNodes != null && childNodes.size() > 0) {
                    int nw = m_nodeSize.cx;
                    if (mp.x >= left && mp.x <= left + nw) {
                        if (node.Expended) {
                            node.collapse();
                        }
                        else {
                            node.expend();
                        }
                        update();
                        return;
                    }
                }
                //移动
                if (node.AllowDragOut) {
                    m_movingNode = node;
                }
            }
        }

        /// <summary>
        /// 单元格触摸移动方法
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="touchInfo">触摸信息</param>
        public override void onCellTouchMove(FCGridCell cell, FCTouchInfo touchInfo) {
            base.onCellTouchMove(cell, touchInfo);
            if (m_movingNode != null) {
                invalidate();
            }
        }

        /// <summary>
        /// 单元格触摸抬起方法
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="touchInfo">触摸信息</param>
        public override void onCellTouchUp(FCGridCell cell, FCTouchInfo touchInfo) {
            base.onCellTouchUp(cell, touchInfo);
            FCPoint mp = touchInfo.m_firstPoint;
            if (m_movingNode != null) {
                FCGridRow curRow = getRow(mp);
                //移动
                if (curRow != null) {
                    FCTreeNode curNode = curRow.getCell(0) as FCTreeNode;
                    if (curNode.AllowDragIn && m_movingNode != curNode) {
                        FCTreeNode curNodeParent = curNode.Parent;
                        FCTreeNode movingNodeParent = m_movingNode.Parent;
                        if (movingNodeParent != null) {
                            movingNodeParent.removeNode(m_movingNode);
                        }
                        else {
                            removeNode(m_movingNode);
                        }
                        //有父节点
                        if (curNodeParent != null) {
                            if (movingNodeParent == curNodeParent) {
                                curNodeParent.insertNode(curNodeParent.getNodeIndex(curNode), m_movingNode);
                            }
                            else {
                                curNode.appendNode(m_movingNode);
                            }
                        }
                        //无父节点
                        else {
                            if (movingNodeParent == curNodeParent) {
                                insertNode(getNodeIndex(curNode), m_movingNode);
                            }
                            else {
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

        /// <summary>
        /// 重绘前景方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void onPaintForeground(FCPaint paint, FCRect clipRect) {
            base.onPaintForeground(paint, clipRect);
            //绘制移动的节点
            if (m_movingNode != null) {
                FCFont font = Font;
                FCPoint mp = TouchPoint;
                FCSize tSize = paint.textSize(m_movingNode.Text, font);
                FCRect tRect = new FCRect(mp.x, mp.y, mp.x + tSize.cx, mp.y + tSize.cy);
                paint.drawText(m_movingNode.Text, TextColor, font, tRect);
            }
        }

        /// <summary>
        /// 绘制编辑文本框
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="paint">绘图对象</param>
        /// <param name="rect">区域</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void onPaintEditTextBox(FCGridCell cell, FCPaint paint, FCRect rect, FCRect clipRect) {
            FCTextBox editTextBox = EditTextBox;
            if (editTextBox != null) {
                FCTreeNode node = cell as FCTreeNode;
                if (node != null) {
                    int indent = node.Indent;
                    rect.left += indent;
                    if (rect.right < rect.left) {
                        rect.right = rect.left;
                    }
                    editTextBox.Bounds = rect;
                    editTextBox.DisplayOffset = false;
                    editTextBox.bringToFront();
                }
                else {
                    base.onPaintEditTextBox(cell, paint, rect, clipRect);
                }
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
            if (name == "checkboxes") {
                CheckBoxes = FCStr.convertStrToBool(value);
            }
            else if (name == "checkboxsize") {
                CheckBoxSize = FCStr.convertStrToSize(value);
            }
            else if (name == "checkedimage") {
                CheckedImage = value;
            }
            else if (name == "collapsednodeimage") {
                CollapsedNodeImage = value;
            }
            else if (name == "expendednodeimage") {
                ExpendedNodeImage = value;
            }
            else if (name == "uncheckedimage") {
                UnCheckedImage = value;
            }
            else if (name == "nodesize") {
                NodeSize = FCStr.convertStrToSize(value);
            }
            else {
                base.setProperty(name, value);
            }
        }
    }
}
