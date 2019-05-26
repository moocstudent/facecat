/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-捂脸鹿创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

using System;
using System.Collections.Generic;

namespace FaceCat {
    /// <summary>
    /// 单元格事件
    /// </summary>
    /// <param name="sender">调用者</param>
    /// <param name="cell">单元格</param>
    public delegate void FCGridCellEvent(object sender, FCGridCell cell);

    /// <summary>
    /// 单元格触摸事件委托
    /// </summary>
    /// <param name="sender">调用者</param>
    /// <param name="cell">单元格</param>
    /// <param name="touchInfo">触摸信息</param>
    public delegate void FCGridCellTouchEvent(object sender, FCGridCell cell, FCTouchInfo touchInfo);

    /// <summary>
    /// 表格排序类
    /// </summary>
    public class FCGridSort {
        /// <summary>
        /// 表格排序类
        /// </summary>
        public class FCGridRowCompare : IComparer<FCGridRow> {
            /// <summary>
            /// 列索引
            /// </summary>
            public int m_columnIndex;

            /// <summary>
            /// 类型
            /// </summary>
            public int m_type;

            /// <summary>
            /// 比较方法
            /// </summary>
            /// <param name="x">行X</param>
            /// <param name="y">行Y</param>
            /// <returns>结果</returns>
            public int Compare(FCGridRow x, FCGridRow y) {
                FCGridCell cellLeft = x.getCell(m_columnIndex);
                FCGridCell cellRight = y.getCell(m_columnIndex);
                if (m_type == 0) {
                    return cellRight.compareTo(cellLeft);
                }
                else {
                    return cellLeft.compareTo(cellRight);
                }
            }
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="grid">表格</param>
        /// <param name="column">列</param>
        /// <param name="sortMode">排序方式</param>
        public virtual void sortColumn(FCGrid grid, FCGridColumn column, FCGridColumnSortMode sortMode) {
            FCGridRowCompare compare = new FCGridRowCompare();
            compare.m_columnIndex = column.Index;
            if (sortMode == FCGridColumnSortMode.Asc) {
                compare.m_type = 1;
            }
            grid.m_rows.Sort(compare);
        }
    }

    /// <summary>
    /// 表格控件
    /// </summary>
    public class FCGrid : FCDiv {
        /// <summary>
        /// 创建表格
        /// </summary>
        public FCGrid() {
            m_editTextBoxLostFocusEvent = new FCEvent(editTextBoxLostFocus);
            m_editTextBoxKeyDownEvent = new FCKeyEvent(editTextBoxKeyDown);
            ShowHScrollBar = true;
            ShowVScrollBar = true;
        }

        /// <summary>
        /// 动画添加的行
        /// </summary>
        public ArrayList<FCGridRow> m_animateAddRows = new ArrayList<FCGridRow>();

        /// <summary>
        /// 动画移除的行
        /// </summary>
        public ArrayList<FCGridRow> m_animateRemoveRows = new ArrayList<FCGridRow>();

        /// <summary>
        /// 列的集合
        /// </summary>
        public ArrayList<FCGridColumn> m_columns = new ArrayList<FCGridColumn>();

        /// <summary>
        /// 正在编辑的单元格
        /// </summary>
        public FCGridCell m_editingCell;

        /// <summary>
        /// 正在编辑的行
        /// </summary>
        public FCGridRow m_editingRow;

        /// <summary>
        /// 编辑文本框丢失焦点事件
        /// </summary>
        private FCEvent m_editTextBoxLostFocusEvent;

        /// <summary>
        /// 编辑文本框键盘事件
        /// </summary>
        private FCKeyEvent m_editTextBoxKeyDownEvent;

        /// <summary>
        /// 是否包含不可见行
        /// </summary>
        protected bool m_hasUnVisibleRow = false;

        /// <summary>
        /// 是否锁定界面更新
        /// </summary>
        protected bool m_lockUpdate = false;

        /// <summary>
        /// 行的集合
        /// </summary>
        public ArrayList<FCGridRow> m_rows = new ArrayList<FCGridRow>();

        /// <summary>
        /// 秒表ID
        /// </summary>
        private int m_timerID = getNewTimerID();

        /// <summary>
        /// 触摸按下时的位置
        /// </summary>
        private FCPoint m_touchDownPoint;

        protected bool m_allowDragRow;

        /// <summary>
        /// 获取或设置是否允许拖动行
        /// </summary>
        public virtual bool AllowDragRow {
            get { return m_allowDragRow; }
            set { m_allowDragRow = value; }
        }

        /// <summary>
        /// 获取所有可见列的宽度
        /// </summary>
        protected virtual int AllVisibleColumnsWidth {
            get {
                int allVisibleColumnsWidth = 0;
                int columnSize = m_columns.size();
                for (int i = 0; i < columnSize; i++) {
                    FCGridColumn column = m_columns.get(i);
                    if (column.Visible) {
                        allVisibleColumnsWidth += column.Width;
                    }
                }
                return allVisibleColumnsWidth;
            }
        }

        protected bool m_allowHoveredRow = true;

        /// <summary>
        /// 获取或设置是否允许悬停行
        /// </summary>
        public virtual bool AllowHoveredRow {
            get { return m_allowHoveredRow; }
            set { m_allowHoveredRow = value; }
        }

        /// <summary>
        /// 获取所有可见行的高度
        /// </summary>
        protected virtual int AllVisibleRowsHeight {
            get {
                int allVisibleRowsHeight = 0;
                int rowSize = m_rows.size();
                for (int i = 0; i < rowSize; i++) {
                    if (m_rows.get(i).Visible) {
                        allVisibleRowsHeight += m_rows.get(i).Height;
                    }
                }
                return allVisibleRowsHeight;
            }
        }

        protected FCGridRowStyle m_alternateRowStyle;

        /// <summary>
        /// 获取或设置交替行的样式
        /// </summary>
        public virtual FCGridRowStyle AlternateRowStyle {
            get { return m_alternateRowStyle; }
            set { m_alternateRowStyle = value; }
        }

        protected FCGridCellEditMode m_cellEditMode = FCGridCellEditMode.SingleClick;

        /// <summary>
        /// 获取或设置单元格编辑模式
        /// </summary>
        public virtual FCGridCellEditMode CellEditMode {
            get { return m_cellEditMode; }
            set { m_cellEditMode = value; }
        }

        protected FCTextBox m_editTextBox;

        /// <summary>
        /// 获取编辑文本框
        /// </summary>
        public virtual FCTextBox EditTextBox {
            get { return m_editTextBox; }
        }

        protected long m_gridLineColor = FCColor.argb(100, 100, 100);

        /// <summary>
        /// 获取或设置网格线的颜色
        /// </summary>
        public virtual long GridLineColor {
            get { return m_gridLineColor; }
            set { m_gridLineColor = value; }
        }

        protected bool m_headerVisible = true;

        /// <summary>
        /// 获取或设置标题头是否可见
        /// </summary>
        public virtual bool HeaderVisible {
            get { return m_headerVisible; }
            set { m_headerVisible = value; }
        }

        protected int m_headerHeight = 20;

        /// <summary>
        /// 获取或设置标题头的高度
        /// </summary>
        public int HeaderHeight {
            get { return m_headerHeight; }
            set { m_headerHeight = value; }
        }

        protected int m_horizontalOffset = 0;

        /// <summary>
        /// 获取或设置横向滚动距离
        /// </summary>
        public virtual int HorizontalOffset {
            get { return m_horizontalOffset; }
            set { m_horizontalOffset = value; }
        }

        protected FCGridCell m_hoveredCell;

        /// <summary>
        /// 获取或设置触摸悬停的单元格
        /// </summary>
        public virtual FCGridCell HoveredCell {
            get { return m_hoveredCell; }
            set { m_hoveredCell = value; }
        }

        protected FCGridRow m_hoveredRow;

        /// <summary>
        /// 获取触摸悬停的行
        /// </summary>
        public virtual FCGridRow HoveredRow {
            get { return m_hoveredRow; }
        }

        protected bool m_multiSelect = false;

        /// <summary>
        /// 获取或设置是否多选
        /// </summary>
        public virtual bool MultiSelect {
            get { return m_multiSelect; }
            set { m_multiSelect = value; }
        }

        protected FCGridRowStyle m_rowStyle = new FCGridRowStyle();

        /// <summary>
        /// 获取或设置行的样式
        /// </summary>
        public virtual FCGridRowStyle RowStyle {
            get { return m_rowStyle; }
            set { m_rowStyle = value; }
        }

        protected ArrayList<FCGridCell> m_selectedCells = new ArrayList<FCGridCell>();

        /// <summary>
        /// 获取选中的单元格
        /// </summary>
        public virtual ArrayList<FCGridCell> SelectedCells {
            get { return m_selectedCells; }
            set {
                m_selectedCells.clear();
                int selectedCellsSize = value.size();
                for (int i = 0; i < selectedCellsSize; i++) {
                    m_selectedCells.add(value.get(i));
                }
                onSelectedCellsChanged();
            }
        }

        protected ArrayList<FCGridColumn> m_selectedColumns = new ArrayList<FCGridColumn>();

        /// <summary>
        /// 获取选中的列
        /// </summary>
        public virtual ArrayList<FCGridColumn> SelectedColumns {
            get { return m_selectedColumns; }
            set {
                m_selectedColumns.clear();
                int selectedColumnsSize = value.size();
                for (int i = 0; i < selectedColumnsSize; i++) {
                    m_selectedColumns.add(value.get(i));
                }
                onSelectedColumnsChanged();
            }
        }

        protected ArrayList<FCGridRow> m_selectedRows = new ArrayList<FCGridRow>();

        /// <summary>
        /// 获取选中行
        /// </summary>
        public virtual ArrayList<FCGridRow> SelectedRows {
            get { return m_selectedRows; }
            set {
                m_selectedRows.clear();
                int selectedRowsSize = value.size();
                for (int i = 0; i < selectedRowsSize; i++) {
                    m_selectedRows.add(value.get(i));
                }
                onSelectedRowsChanged();
            }
        }

        protected FCGridSelectionMode m_selectionMode = FCGridSelectionMode.SelectFullRow;

        /// <summary>
        /// 选中模式
        /// </summary>
        public virtual FCGridSelectionMode SelectionMode {
            get { return m_selectionMode; }
            set { m_selectionMode = value; }
        }

        protected FCGridSort m_sort = new FCGridSort();

        /// <summary>
        /// 获取或设置排序处理类
        /// </summary>
        public virtual FCGridSort Sort {
            get { return m_sort; }
            set { m_sort = value; }
        }

        protected bool m_useAnimation = false;

        /// <summary>
        /// 获取或设置是否使用动画
        /// </summary>
        public virtual bool UseAnimation {
            get { return m_useAnimation; }
            set {
                m_useAnimation = value;
                if (m_useAnimation) {
                    startTimer(m_timerID, 20);
                }
                else {
                    stopTimer(m_timerID);
                }
            }
        }

        protected int m_verticalOffset = 0;

        /// <summary>
        /// 获取或设置纵向滚动距离
        /// </summary>
        public virtual int VerticalOffset {
            get { return m_verticalOffset; }
            set { m_verticalOffset = value; }
        }

        /// <summary>
        /// 添加列
        /// </summary>
        /// <param name="column">列</param>
        public virtual void addColumn(FCGridColumn column) {
            column.Grid = this;
            m_columns.add(column);
            int columnsSize = m_columns.size();
            for (int i = 0; i < columnsSize; i++) {
                m_columns.get(i).Index = i;
            }
            addControl(column);
        }

        /// <summary>
        /// 添加行
        /// </summary>
        /// <param name="row">行</param>
        public void addRow(FCGridRow row) {
            row.Grid = this;
            m_rows.add(row);
            row.onAdd();
            if (m_selectionMode == FCGridSelectionMode.SelectFullRow) {
                int selectedRowsSize = m_selectedRows.size();
                if (selectedRowsSize == 0) {
                    m_selectedRows.add(row);
                    onSelectedRowsChanged();
                }
            }
        }

        /// <summary>
        /// 动画添加行
        /// </summary>
        /// <param name="row">行</param>
        public void animateAddRow(FCGridRow row) {
            row.Grid = this;
            m_rows.add(row);
            row.onAdd();
            if (m_selectionMode == FCGridSelectionMode.SelectFullRow) {
                int selectedRowsSize = m_selectedRows.size();
                if (selectedRowsSize == 0) {
                    m_selectedRows.add(row);
                    onSelectedRowsChanged();
                }
            }
            if (m_useAnimation) {
                m_animateAddRows.add(row);
            }
        }

        /// <summary>
        /// 动画移除行
        /// </summary>
        /// <param name="row">行</param>
        public void animateRemoveRow(FCGridRow row) {
            if (m_useAnimation) {
                m_animateRemoveRows.add(row);
            }
            else {
                removeRow(row);
            }
        }

        /// <summary>
        /// 开始更新
        /// </summary>
        public void beginUpdate() {
            m_lockUpdate = true;
        }

        /// <summary>
        /// 调用单元格事件
        /// </summary>
        /// <param name="eventID">事件ID</param>
        /// <param name="cell">单元格</param>
        protected void callCellEvents(int eventID, FCGridCell cell) {
            if (m_events != null && m_events.containsKey(eventID)) {
                ArrayList<object> events = m_events.get(eventID);
                int eventSize = events.size();
                for (int i = 0; i < eventSize; i++) {
                    FCGridCellEvent func = events.get(i) as FCGridCellEvent;
                    if (func != null) {
                        func(this, cell);
                    }
                }
            }
        }

        /// <summary>
        /// 调用单元格触摸事件
        /// </summary>
        /// <param name="eventID">事件ID</param>
        /// <param name="cell">单元格</param>
        /// <param name="touchInfo">触摸信息</param>
        protected void callCellTouchEvents(int eventID, FCGridCell cell, FCTouchInfo touchInfo) {
            if (m_events != null && m_events.containsKey(eventID)) {
                ArrayList<object> events = m_events.get(eventID);
                int eventSize = events.size();
                for (int i = 0; i < eventSize; i++) {
                    FCGridCellTouchEvent func = events.get(i) as FCGridCellTouchEvent;
                    if (func != null) {
                        func(this, cell, touchInfo);
                    }
                }
            }
        }

        /// <summary>
        /// 请除数据
        /// </summary>
        public void clear() {
            clearRows();
            clearColumns();
        }

        /// <summary>
        /// 清除所有列
        /// </summary>
        public virtual void clearColumns() {
            m_selectedColumns.clear();
            int columnSize = m_columns.size();
            for (int i = 0; i < columnSize; i++) {
                removeControl(m_columns.get(i));
                m_columns.get(i).delete();
            }
            m_columns.clear();
        }

        /// <summary>
        /// 清除所有行
        /// </summary>
        public void clearRows() {
            m_hasUnVisibleRow = false;
            m_hoveredCell = null;
            m_hoveredRow = null;
            m_selectedRows.clear();
            int rowSize = m_rows.size();
            for (int i = 0; i < rowSize; i++) {
                m_rows.get(i).onRemove();
                m_rows.get(i).delete();
            }
            m_rows.clear();
        }

        /// <summary>
        /// 销毁控件的方法
        /// </summary>
        public override void delete() {
            if (!IsDeleted) {
                m_animateAddRows.clear();
                m_animateRemoveRows.clear();
                m_editingCell = null;
                if (m_editTextBox != null) {
                    if (m_editTextBoxLostFocusEvent != null) {
                        m_editTextBox.removeEvent(m_editTextBoxLostFocusEvent, FCEventID.LOSTFOCUS);
                        m_editTextBoxLostFocusEvent = null;
                    }
                    if (m_editTextBoxKeyDownEvent != null) {
                        m_editTextBox.removeEvent(m_editTextBoxKeyDownEvent, FCEventID.KEYDOWN);
                        m_editTextBoxKeyDownEvent = null;
                    }
                    m_editTextBox = null;
                }
                stopTimer(m_timerID);
                clear();
            }
            base.delete();
        }

        /// <summary>
        /// 编辑文本框丢失焦点事件
        /// </summary>
        /// <param name="sender">调用者</param>
        protected void editTextBoxLostFocus(object sender) {
            if (m_editTextBox != null && m_editTextBox.Tag != null) {
                onCellEditEnd(m_editTextBox.Tag as FCGridCell);
            }
        }

        /// <summary>
        /// 编辑文本框键盘事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="key">按键</param>
        protected void editTextBoxKeyDown(object sender, char key) {
            if (key == 13) {
                if (m_editTextBox != null && !m_editTextBox.Multiline) {
                    m_editTextBox.Focused = false;
                }
            }
            else if (key == 27) {
                onCellEditEnd(null);
            }
        }

        /// <summary>
        /// 结束更新
        /// </summary>
        public void endUpdate() {
            if (m_lockUpdate) {
                m_lockUpdate = false;
                update();
            }
        }

        /// <summary>
        /// 获取表格列
        /// </summary>
        /// <param name="columnIndex">列的索引</param>
        /// <returns>表格列</returns>
        public FCGridColumn getColumn(int columnIndex) {
            if (columnIndex >= 0 && columnIndex < m_columns.size()) {
                return m_columns.get(columnIndex);
            }
            return null;
        }

        /// <summary>
        /// 获取表格列
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <returns>表格列</returns>
        public FCGridColumn getColumn(String columnName) {
            int columnsSize = m_columns.size();
            for (int i = 0; i < columnsSize; i++) {
                if (m_columns.get(i).Name == columnName) {
                    return m_columns.get(i);
                }
            }
            return null;
        }

        /// <summary>
        /// 获取所有的列
        /// </summary>
        /// <returns></returns>
        public ArrayList<FCGridColumn> getColumns() {
            return m_columns;
        }

        /// <summary>
        /// 获取内容的高度
        /// </summary>
        /// <returns>高度</returns>
        public override int getContentHeight() {
            int allVisibleRowsHeight = AllVisibleRowsHeight;
            if (allVisibleRowsHeight > 0) {
                if (allVisibleRowsHeight <= Height) {
                    allVisibleRowsHeight += m_headerVisible ? m_headerHeight : 0;
                }
                return allVisibleRowsHeight;
            }
            else {
                return 0;
            }
        }

        /// <summary>
        /// 获取内容的宽度
        /// </summary>
        /// <returns>宽度</returns>
        public override int getContentWidth() {
            return AllVisibleColumnsWidth;
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <returns>控件类型</returns>
        public override String getControlType() {
            return "Grid";
        }

        /// <summary>
        /// 获取显示偏移坐标
        /// </summary>
        /// <returns></returns>
        public override FCPoint getDisplayOffset() {
            return new FCPoint(0, 0);
        }

        /// <summary>
        /// 获取事件名称列表
        /// </summary>
        /// <returns>名称列表</returns>
        public override ArrayList<String> getEventNames() {
            ArrayList<String> eventNames = base.getEventNames();
            eventNames.AddRange(new String[] { "FCGridCellClick", "FCGridCellEditBegin", "FCGridCellEditEnd", "FCGridCellTouchDown", 
            "FCGridCellTouchMove", "FCGridCellTouchUp", "GridSelectedCellsChanged", "GridSelectedColumnsChanged", "GridSelectedRowsChanged"});
            return eventNames;
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">返回属性值</param>
        /// <param name="type">返回属性类型</param>
        public override void getProperty(String name, ref String value, ref String type) {
            if (name == "allowdragrow") {
                type = "bool";
                value = FCStr.convertBoolToStr(AllowDragRow);
            }
            else if (name == "allowhoveredrow") {
                type = "bool";
                value = FCStr.convertBoolToStr(AllowHoveredRow);
            }
            else if (name == "celleditmode") {
                type = "enum:FCGridCellEditMode";
                FCGridCellEditMode cellEditMode = CellEditMode;
                if (cellEditMode == FCGridCellEditMode.DoubleClick) {
                    value = "DoubleClick";
                }
                else if (cellEditMode == FCGridCellEditMode.None) {
                    value = "None";
                }
                else {
                    value = "SingleClick";
                }
            }
            else if (name == "gridlinecolor") {
                type = "color";
                value = FCStr.convertColorToStr(GridLineColor);
            }
            else if (name == "headerheight") {
                type = "int";
                value = FCStr.convertIntToStr(HeaderHeight);
            }
            else if (name == "headervisible") {
                type = "bool";
                value = FCStr.convertBoolToStr(HeaderVisible);
            }
            else if (name == "horizontaloffset") {
                type = "int";
                value = FCStr.convertIntToStr(HorizontalOffset);
            }
            else if (name == "multiselect") {
                type = "bool";
                value = FCStr.convertBoolToStr(MultiSelect);
            }
            else if (name == "selectionmode") {
                type = "enum:FCGridSelectionMode";
                FCGridSelectionMode selectionMode = SelectionMode;
                if (selectionMode == FCGridSelectionMode.SelectCell) {
                    value = "SelectCell";
                }
                else if (selectionMode == FCGridSelectionMode.SelectFullColumn) {
                    value = "SelectFullColumn";
                }
                else if (selectionMode == FCGridSelectionMode.SelectFullRow) {
                    value = "SelectFullRow";
                }
                else {
                    value = "None";
                }
            }
            else if (name == "useanimation") {
                type = "bool";
                value = FCStr.convertBoolToStr(UseAnimation);
            }
            else if (name == "verticaloffset") {
                type = "int";
                value = FCStr.convertIntToStr(VerticalOffset);
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
            propertyNames.AddRange(new String[] { "AllowDragRow", "AllowHoveredRow", "CellEditMode", "GridLineColor", "HeaderHeight", "HeaderVisible", "HorizontalOffset", "MultiSelect", 
                "SelectionMode", "UseAnimation", "VerticalOffset"});
            return propertyNames;
        }

        /// <summary>
        /// 根据坐标获取行
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>表格行</returns>
        public FCGridRow getRow(FCPoint mp) {
            if (m_hasUnVisibleRow) {
                int rowsSize = m_rows.size();
                for (int i = 0; i < rowsSize; i++) {
                    FCGridRow row = m_rows.get(i);
                    if (row.Visible) {
                        FCRect bounds = row.Bounds;
                        if (mp.y >= bounds.top && mp.y <= bounds.bottom) {
                            return row;
                        }
                    }
                }
            }
            else {
                int begin = 0;
                int end = m_rows.size() - 1;
                int sub = end - begin;
                while (sub >= 0) {
                    int half = begin + sub / 2;
                    FCGridRow row = m_rows.get(half);
                    FCRect bounds = row.Bounds;
                    if (half == begin || half == end) {
                        if (mp.y >= m_rows.get(begin).Bounds.top && mp.y <= m_rows.get(begin).Bounds.bottom) {
                            return m_rows.get(begin);
                        }
                        if (mp.y >= m_rows.get(end).Bounds.top && mp.y <= m_rows.get(end).Bounds.bottom) {
                            return m_rows.get(end);
                        }
                        break;
                    }
                    if (mp.y >= bounds.top && mp.y <= bounds.bottom) {
                        return row;
                    }
                    else if (bounds.top > mp.y) {
                        end = half;
                    }
                    else if (bounds.bottom <= mp.y) {
                        begin = half;
                    }
                    sub = end - begin;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取表格行
        /// </summary>
        /// <param name="rowIndex">行的索引</param>
        /// <returns>表格行</returns>
        public FCGridRow getRow(int rowIndex) {
            if (rowIndex >= 0 && rowIndex < m_rows.size()) {
                return m_rows.get(rowIndex);
            }
            return null;
        }

        /// <summary>
        /// 获取所有的行
        /// </summary>
        /// <returns></returns>
        public ArrayList<FCGridRow> getRows() {
            return m_rows;
        }

        /// <summary>
        /// 获取可见的行索引
        /// </summary>
        /// <param name="visiblePercent">可见百分比</param>
        /// <param name="firstVisibleRowIndex">首先可见的行索引</param>
        /// <param name="lastVisibleRowIndex">最后可见的行索引</param>
        public void getVisibleRowsIndex(double visiblePercent, ref int firstVisibleRowIndex, ref int lastVisibleRowIndex) {
            firstVisibleRowIndex = -1;
            lastVisibleRowIndex = -1;
            int rowsSize = m_rows.size();
            if (rowsSize > 0) {
                for (int i = 0; i < rowsSize; i++) {
                    FCGridRow row = m_rows.get(i);
                    if (isRowVisible(row, visiblePercent)) {
                        if (firstVisibleRowIndex == -1) {
                            firstVisibleRowIndex = i;
                        }
                    }
                    else {
                        if (firstVisibleRowIndex != -1) {
                            lastVisibleRowIndex = i;
                            break;
                        }
                    }
                }
                if (firstVisibleRowIndex != -1 && lastVisibleRowIndex == -1) {
                    lastVisibleRowIndex = firstVisibleRowIndex;
                }
            }
        }

        /// <summary>
        /// 插入行
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="row">行</param>
        public void insertRow(int index, FCGridRow row) {
            row.Grid = this;
            m_rows.Insert(index, row);
            row.onAdd();
        }

        /// <summary>
        /// 判断行是否可见
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="visiblePercent">可见百分比</param>
        /// <returns>是否可见</returns>
        public bool isRowVisible(FCGridRow row, double visiblePercent) {
            int scrollV = 0;
            FCVScrollBar vScrollBar = VScrollBar;
            if (vScrollBar != null && vScrollBar.Visible) {
                scrollV = -vScrollBar.Pos;
            }
            int cell = (m_headerVisible ? m_headerHeight : 0);
            int floor = Height - cell;
            FCRect bounds = row.Bounds;
            return rowVisible(ref bounds, row.Height, scrollV, visiblePercent, cell, floor);
        }

        /// <summary>
        /// 移动行
        /// </summary>
        /// <param name="oldIndex">旧行</param>
        /// <param name="newIndex">新行</param>
        public void moveRow(int oldIndex, int newIndex) {
            int rowsSize = m_rows.size();
            if (rowsSize > 0) {
                if (oldIndex >= 0 && oldIndex < rowsSize
                    && newIndex >= 0 && newIndex < rowsSize) {
                    FCGridRow movingRow = m_rows.get(oldIndex);
                    FCGridRow targetRow = m_rows.get(newIndex);
                    if (movingRow != targetRow) {
                        //交换行
                        m_rows.set(newIndex, movingRow);
                        m_rows.set(oldIndex, targetRow);
                        movingRow.Index = newIndex;
                        targetRow.Index = oldIndex;
                        FCVScrollBar vScrollBar = VScrollBar;
                        if (vScrollBar != null && vScrollBar.Visible) {
                            int firstVisibleRowIndex = -1, lastVisibleRowIndex = -1;
                            getVisibleRowsIndex(0.6, ref firstVisibleRowIndex, ref lastVisibleRowIndex);
                            int th = targetRow.Height;
                            if (newIndex <= firstVisibleRowIndex) {
                                if (newIndex == firstVisibleRowIndex) {
                                    vScrollBar.Pos -= th;
                                }
                                int count = 0;
                                while (!isRowVisible(targetRow, 0.6)) {
                                    int newPos = vScrollBar.Pos - th;
                                    vScrollBar.Pos = newPos;
                                    count++;
                                    if (count > rowsSize || newPos <= vScrollBar.Pos) {
                                        break;
                                    }
                                }
                            }
                            else if (newIndex >= lastVisibleRowIndex) {
                                if (newIndex == lastVisibleRowIndex) {
                                    vScrollBar.Pos += th;
                                }
                                int count = 0;
                                while (!isRowVisible(targetRow, 0.6)) {
                                    int newPos = vScrollBar.Pos + th;
                                    vScrollBar.Pos = newPos;
                                    count++;
                                    if (count > rowsSize || newPos >= vScrollBar.Pos) {
                                        break;
                                    }
                                }
                            }
                            vScrollBar.update();
                        }
                        update();
                    }
                }
            }
        }

        /// <summary>
        /// 单元格触摸点击方法
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="touchInfo">触摸信息</param>
        public virtual void onCellClick(FCGridCell cell, FCTouchInfo touchInfo) {
            callCellTouchEvents(FCEventID.GRIDCELLCLICK, cell, touchInfo);
        }

        /// <summary>
        /// 单元格编辑开始
        /// </summary>
        /// <param name="cell">单元格</param>
        public virtual void onCellEditBegin(FCGridCell cell) {
            m_editingCell = cell;
            //创建编辑文本框
            if (m_editTextBox == null) {
                FCHost host = Native.Host;
                m_editTextBox = host.createInternalControl(this, "edittextbox") as FCTextBox;
                m_editTextBox.addEvent(m_editTextBoxLostFocusEvent, FCEventID.LOSTFOCUS);
                m_editTextBox.addEvent(m_editTextBoxKeyDownEvent, FCEventID.KEYDOWN);
                addControl(m_editTextBox);
            }
            m_editTextBox.Focused = true;
            String text = m_editingCell.Text;
            m_editTextBox.Tag = m_editingCell;
            m_editTextBox.Text = text;
            m_editTextBox.clearRedoUndo();
            m_editTextBox.Visible = true;
            if (text != null && text.Length > 0) {
                m_editTextBox.SelectionStart = text.Length;
            }
            callCellEvents(FCEventID.GRIDCELLEDITBEGIN, cell);
        }

        /// <summary>
        /// 单元格编辑结束
        /// </summary>
        /// <param name="cell">单元格</param>
        public virtual void onCellEditEnd(FCGridCell cell) {
            if (cell != null) {
                cell.Text = m_editTextBox.Text;
            }
            m_editTextBox.Tag = null;
            m_editTextBox.Visible = false;
            m_editingCell = null;
            callCellEvents(FCEventID.GRIDCELLEDITEND, cell);
            invalidate();
        }

        /// <summary>
        /// 单元格触摸按下方法
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="touchInfo">触摸信息</param>
        public virtual void onCellTouchDown(FCGridCell cell, FCTouchInfo touchInfo) {
            callCellTouchEvents(FCEventID.GRIDCELLTOUCHDOWN, cell, touchInfo);
        }

        /// <summary>
        /// 单元格触摸进入方法
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="touchInfo">触摸信息</param>
        public virtual void onCellTouchEnter(FCGridCell cell, FCTouchInfo touchInfo) {
            callCellTouchEvents(FCEventID.GRIDCELLTOUCHENTER, cell, touchInfo);
            if (AutoEllipsis || (cell.Style != null && cell.Style.AutoEllipsis)) {
                m_native.Host.showToolTip(cell.getPaintText(), m_native.TouchPoint);
            }
        }

        /// <summary>
        /// 单元格触摸移出方法
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="touchInfo">触摸信息</param>
        public virtual void onCellTouchLeave(FCGridCell cell, FCTouchInfo touchInfo) {
            callCellTouchEvents(FCEventID.GRIDCELLTOUCHLEAVE, cell, touchInfo);
        }

        /// <summary>
        /// 单元格触摸移动方法
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="touchInfo">触摸信息</param>
        public virtual void onCellTouchMove(FCGridCell cell, FCTouchInfo touchInfo) {
            callCellTouchEvents(FCEventID.GRIDCELLTOUCHMOVE, cell, touchInfo);
        }

        /// <summary>
        /// 单元格触摸抬起方法
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="touchInfo">触摸信息</param>
        public virtual void onCellTouchUp(FCGridCell cell, FCTouchInfo touchInfo) {
            callCellTouchEvents(FCEventID.GRIDCELLTOUCHUP, cell, touchInfo);
        }

        /// <summary>
        /// 键盘方法
        /// </summary>
        /// <param name="key">键盘参数</param>
        public override void onKeyDown(char key) {
            FCHost host = Native.Host;
            if (!host.isKeyPress(0x10) && !host.isKeyPress(0x11) && !host.isKeyPress(0x12)) {
                if (key == 38 || key == 40) {
                    callKeyEvents(FCEventID.KEYDOWN, key);
                    FCGridRow row = null;
                    int offset = 0;
                    if (key == 38) {
                        row = selectFrontRow();
                        if (row != null) {
                            offset = -row.Height;
                        }
                    }
                    else if (key == 40) {
                        row = selectNextRow();
                        if (row != null) {
                            offset = row.Height;
                        }
                    }
                    if (row != null && !isRowVisible(row, 0.6)) {
                        FCVScrollBar vScrollBar = VScrollBar;
                        if (vScrollBar != null && vScrollBar.Visible) {
                            vScrollBar.Pos += offset;
                            vScrollBar.update();
                        }
                        onSelectedRowsChanged();
                    }
                    invalidate();
                    return;
                }
            }
            base.onKeyDown(key);
        }

        /// <summary>
        /// 控件添加方法
        /// </summary>
        public override void onLoad() {
            base.onAdd();
            if (m_useAnimation) {
                startTimer(m_timerID, 20);
            }
            else {
                stopTimer(m_timerID);
            }
        }

        /// <summary>
        /// 丢失焦点方法
        /// </summary>
        public override void onLostFocus() {
            base.onLostFocus();
            m_hoveredCell = null;
            m_hoveredRow = null;
        }

        /// <summary>
        /// 触摸按下方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onTouchDown(FCTouchInfo touchInfo) {
            base.onTouchDown(touchInfo);
            touchEvent(touchInfo, 1);
        }

        /// <summary>
        /// 触摸离开方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onTouchLeave(FCTouchInfo touchInfo) {
            base.onTouchLeave(touchInfo);
            if (m_hoveredCell != null) {
                onCellTouchLeave(m_hoveredCell, touchInfo);
                m_hoveredCell = null;
            }
            m_hoveredRow = null;
            invalidate();
        }

        /// <summary>
        /// 触摸移动方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onTouchMove(FCTouchInfo touchInfo) {
            base.onTouchMove(touchInfo);
            touchEvent(touchInfo, 0);
        }

        /// <summary>
        /// 触摸抬起方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onTouchUp(FCTouchInfo touchInfo) {
            base.onTouchUp(touchInfo);
            touchEvent(touchInfo, 2);
        }

        /// <summary>
        /// 重绘前景方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void onPaintForeground(FCPaint paint, FCRect clipRect) {
            resetHeaderLayout();
            int width = Width, height = Height;
            if (width > 0 && height > 0) {
                FCNative native = Native;
                FCRect drawRect = new FCRect(0, 0, width, height);
                int allVisibleColumnsWidth = AllVisibleColumnsWidth;
                //行的可见宽度
                int rowVisibleWidth = 0;
                if (allVisibleColumnsWidth > 0) {
                    rowVisibleWidth = allVisibleColumnsWidth > width ? width : allVisibleColumnsWidth;
                }
                //绘制标题头
                int hHeight = m_headerVisible ? m_headerHeight : 0;
                int scrollH = 0, scrollV = 0;
                FCHScrollBar hScrollBar = HScrollBar;
                FCVScrollBar vScrollBar = VScrollBar;
                if (hScrollBar != null && hScrollBar.Visible) {
                    scrollH = -hScrollBar.Pos;
                }
                if (vScrollBar != null && vScrollBar.Visible) {
                    scrollV = -vScrollBar.Pos;
                }
                onSetEmptyClipRegion();
                //获取显示的第一行和最后一行
                FCPoint fPoint = new FCPoint(0, hHeight + 1 - scrollV);
                FCPoint ePoint = new FCPoint(0, height - 10 - scrollV);
                FCGridRow fRow = getRow(fPoint);
                FCGridRow eRow = getRow(ePoint);
                while (eRow == null && ePoint.y > 0) {
                    ePoint.y -= 10;
                    eRow = getRow(ePoint);
                }
                if (fRow != null && eRow != null) {
                    int fIndex = fRow.Index;
                    int eIndex = eRow.Index;
                    for (int i = fIndex; i <= eIndex; i++) {
                        FCGridRow row = m_rows.get(i);
                        if (row.Visible) {
                            FCRect rowRect = row.Bounds;
                            rowRect.top += scrollV;
                            rowRect.bottom += scrollV;
                            row.onPaint(paint, rowRect, row.VisibleIndex % 2 == 1);
                            //画选中行
                            FCRect tempRect = new FCRect();
                            ArrayList<FCGridCell> cells = null;
                            ArrayList<FCGridCell> frozenCells = new ArrayList<FCGridCell>();
                            for (int j = 0; j < 2; j++) {
                                if (j == 0) {
                                    cells = row.getCells();
                                }
                                else {
                                    cells = frozenCells;
                                }
                                int frozenRight = 0;
                                int cellSize = cells.size();
                                for (int c = 0; c < cellSize; c++) {
                                    FCGridCell cell = cells.get(c);
                                    FCGridColumn column = cell.Column;
                                    if (column.Visible) {
                                        FCRect headerRect = column.HeaderRect;
                                        if (j == 0 && column.Frozen) {
                                            frozenRight = headerRect.right;
                                            frozenCells.add(cell);
                                            continue;
                                        }
                                        if (!column.Frozen) {
                                            headerRect.left += scrollH;
                                            headerRect.right += scrollH;
                                        }
                                        int cellWidth = column.Width;
                                        int colSpan = cell.ColSpan;
                                        if (colSpan > 1) {
                                            for (int n = 1; n < colSpan; n++) {
                                                FCGridColumn spanColumn = getColumn(column.Index + n);
                                                if (spanColumn != null && spanColumn.Visible) {
                                                    cellWidth += spanColumn.Width;
                                                }
                                            }
                                        }
                                        int cellHeight = row.Height;
                                        int rowSpan = cell.RowSpan;
                                        if (rowSpan > 1) {
                                            for (int n = 1; n < rowSpan; n++) {
                                                FCGridRow spanRow = getRow(i + n);
                                                if (spanRow != null && spanRow.Visible) {
                                                    cellHeight += spanRow.Height;
                                                }
                                            }
                                        }
                                        FCRect cellRect = new FCRect(headerRect.left, rowRect.top + m_verticalOffset, headerRect.left + cellWidth, rowRect.top + m_verticalOffset + cellHeight);
                                        cellRect.left += row.HorizontalOffset;
                                        cellRect.right += row.HorizontalOffset;
                                        if (native.Host.getIntersectRect(ref tempRect, ref cellRect, ref drawRect) > 0) {
                                            if (cell != null) {
                                                FCRect cellClipRect = cellRect;
                                                if (!column.Frozen) {
                                                    if (cellClipRect.left < frozenRight) {
                                                        cellClipRect.left = frozenRight;
                                                    }
                                                    if (cellClipRect.right < frozenRight) {
                                                        cellClipRect.right = frozenRight;
                                                    }
                                                }
                                                cell.onPaint(paint, cellRect, cellClipRect, row.VisibleIndex % 2 == 1);
                                                if (m_editingCell != null && m_editingCell == cell && m_editTextBox != null) {
                                                    FCRect editClipRect = new FCRect(cellClipRect.left - cellRect.left, cellClipRect.top - cellRect.top,
                                                        cellClipRect.right - cellRect.left, cellClipRect.bottom - cellRect.top);
                                                    onPaintEditTextBox(cell, paint, cellRect, editClipRect);
                                                }
                                                if (m_gridLineColor != FCColor.None) {
                                                    if (i == fIndex) {
                                                        paint.drawLine(m_gridLineColor, 1, 0, cellClipRect.left, cellClipRect.top, cellClipRect.right - 1, cellClipRect.top);
                                                    }
                                                    if (c == 0) {
                                                        paint.drawLine(m_gridLineColor, 1, 0, cellClipRect.left, cellClipRect.top, cellClipRect.left, cellClipRect.bottom - 1);
                                                    }
                                                    paint.drawLine(m_gridLineColor, 1, 0, cellClipRect.left, cellClipRect.bottom - 1, cellClipRect.right - 1, cellClipRect.bottom - 1);
                                                    paint.drawLine(m_gridLineColor, 1, 0, cellClipRect.right - 1, cellClipRect.top, cellClipRect.right - 1, cellClipRect.bottom - 1);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            frozenCells.clear();
                            row.onPaintBorder(paint, rowRect, row.VisibleIndex % 2 == 1);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 绘制编辑文本框
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="paint">绘图对象</param>
        /// <param name="rect">区域</param>
        /// <param name="clipRect">裁剪区域</param>
        public virtual void onPaintEditTextBox(FCGridCell cell, FCPaint paint, FCRect rect, FCRect clipRect) {
            m_editTextBox.Region = clipRect;
            m_editTextBox.Bounds = rect;
            m_editTextBox.DisplayOffset = false;
            m_editTextBox.bringToFront();
        }

        /// <summary>
        /// 行编辑开始
        /// </summary>
        /// <param name="row">行</param>
        public virtual void onRowEditBegin(FCGridRow row) {
            FCView editButton = row.EditButton;
            if (editButton != null && !containsControl(editButton)) {
                FCPoint mp = TouchPoint;
                if (mp.x - m_touchDownPoint.x < -10) {
                    m_editingRow = row;
                    addControl(editButton);
                    if (m_useAnimation) {
                        editButton.Location = new FCPoint(-10000, -10000);
                        m_editingRow.m_editState = 1;
                    }
                    else {
                        m_editingRow.HorizontalOffset = -editButton.Width - ((m_vScrollBar != null && m_vScrollBar.Visible) ? m_vScrollBar.Width : 0);
                    }
                }
            }
        }

        /// <summary>
        /// 行编辑结束
        /// </summary>
        public virtual void onRowEditEnd() {
            if (m_useAnimation) {
                m_editingRow.m_editState = 2;
            }
            else {
                m_editingRow.HorizontalOffset = 0;
                removeControl(m_editingRow.EditButton);
                m_editingRow = null;
            }
        }

        /// <summary>
        /// 选中单元格改变方法
        /// </summary>
        public virtual void onSelectedCellsChanged() {
            callEvents(FCEventID.GRIDSELECTEDCELLSCHANGED);
        }

        /// <summary>
        /// 选中列改变方法
        /// </summary>
        public virtual void onSelectedColumnsChanged() {
            callEvents(FCEventID.GRIDSELECTEDCOLUMNSSCHANGED);
        }

        /// <summary>
        /// 选中行改变方法
        /// </summary>
        public virtual void onSelectedRowsChanged() {
            callEvents(FCEventID.GRIDSELECTEDROWSCHANGED);
        }

        /// <summary>
        /// 设置控件不可见方法
        /// </summary>
        public virtual void onSetEmptyClipRegion() {
            //隐藏控件
            ArrayList<FCView> controls = m_controls;
            int controlsSize = controls.size();
            FCRect emptyClipRect = new FCRect();
            for (int i = 0; i < controlsSize; i++) {
                FCView control = controls.get(i);
                if (m_editingRow != null && control == m_editingRow.EditButton) {
                    continue;
                }
                FCScrollBar scrollBar = control as FCScrollBar;
                FCGridColumn gridColumn = control as FCGridColumn;
                if (control != m_editTextBox && scrollBar == null && gridColumn == null) {
                    control.Region = emptyClipRect;
                }
            }
        }

        /// <summary>
        /// 秒表方法
        /// </summary>
        /// <param name="timerID">秒表ID</param>
        public override void onTimer(int timerID) {
            base.onTimer(timerID);
            if (m_timerID == timerID) {
                if (m_useAnimation) {
                    bool paint = false;
                    if (m_horizontalOffset != 0 || m_verticalOffset != 0) {
                        if (m_horizontalOffset != 0) {
                            m_horizontalOffset = m_horizontalOffset * 2 / 3;
                            if (m_horizontalOffset >= -1 && m_horizontalOffset <= 1) {
                                m_horizontalOffset = 0;
                            }
                        }
                        if (m_verticalOffset != 0) {
                            m_verticalOffset = m_verticalOffset * 2 / 3;
                            if (m_verticalOffset >= -1 && m_verticalOffset <= 1) {
                                m_verticalOffset = 0;
                            }
                        }
                        paint = true;
                    }
                    int animateAddRowsSize = m_animateAddRows.size();
                    if (animateAddRowsSize > 0) {
                        int width = AllVisibleColumnsWidth;
                        int step = width / 10;
                        if (step < 10) {
                            step = 10;
                        }
                        for (int i = 0; i < animateAddRowsSize; i++) {
                            FCGridRow row = m_animateAddRows.get(i);
                            int horizontalOffset = row.HorizontalOffset;
                            if (horizontalOffset > step) {
                                horizontalOffset -= step;
                            }
                            else {
                                horizontalOffset = 0;
                            }
                            row.HorizontalOffset = horizontalOffset;
                            if (horizontalOffset == 0) {
                                m_animateAddRows.removeAt(i);
                                animateAddRowsSize--;
                                i--;
                            }
                        }
                        paint = true;
                    }
                    int animateRemoveRowsSize = m_animateRemoveRows.size();
                    if (animateRemoveRowsSize > 0) {
                        int width = AllVisibleColumnsWidth;
                        int step = width / 10;
                        if (step < 10) {
                            step = 10;
                        }
                        for (int i = 0; i < animateRemoveRowsSize; i++) {
                            FCGridRow row = m_animateRemoveRows.get(i);
                            int horizontalOffset = row.HorizontalOffset;
                            if (horizontalOffset <= width) {
                                horizontalOffset += step;
                            }
                            row.HorizontalOffset = horizontalOffset;
                            if (horizontalOffset > width) {
                                m_animateRemoveRows.removeAt(i);
                                removeRow(row);
                                update();
                                animateRemoveRowsSize--;
                                i--;
                            }
                        }
                        paint = true;
                    }
                    if (m_editingRow != null) {
                        int scrollH = 0, scrollV = 0;
                        FCHScrollBar hScrollBar = HScrollBar;
                        FCVScrollBar vScrollBar = VScrollBar;
                        int vScrollBarW = 0;
                        if (hScrollBar != null && hScrollBar.Visible) {
                            scrollH = -hScrollBar.Pos;
                        }
                        if (vScrollBar != null && vScrollBar.Visible) {
                            scrollV = -vScrollBar.Pos;
                            vScrollBarW = vScrollBar.Width;
                        }
                        if (m_editingRow.m_editState == 1) {
                            FCView editButton = m_editingRow.EditButton;
                            bool isOver = false;
                            int sub = editButton.Width + vScrollBarW + m_editingRow.HorizontalOffset;
                            if (sub < 2) {
                                isOver = true;
                                m_editingRow.HorizontalOffset = -editButton.Width - vScrollBarW;
                            }
                            else {
                                m_editingRow.HorizontalOffset -= 10;
                            }
                            editButton.Location = new FCPoint(AllVisibleColumnsWidth + scrollH + m_editingRow.HorizontalOffset,
                            m_editingRow.Bounds.top + scrollV);
                            if (isOver) {
                                m_editingRow.m_editState = 0;
                            }
                        }
                        if (m_editingRow.m_editState == 2) {
                            FCView editButton = m_editingRow.EditButton;
                            bool isOver = false;
                            if (m_editingRow.HorizontalOffset < 0) {
                                m_editingRow.HorizontalOffset += 10;
                                if (m_editingRow.HorizontalOffset >= 0) {
                                    m_editingRow.HorizontalOffset = 0;
                                    isOver = true;
                                }
                            }
                            editButton.Location = new FCPoint(AllVisibleColumnsWidth + scrollH + m_editingRow.HorizontalOffset,
                            m_editingRow.Bounds.top + scrollV);
                            if (isOver) {
                                removeControl(editButton);
                                m_editingRow.m_editState = 0;
                                m_editingRow = null;
                            }
                        }
                        paint = true;
                    }
                    if (paint) {
                        invalidate();
                    }
                }
            }
        }

        /// <summary>
        /// 可见状态改变方法
        /// </summary>
        public override void onVisibleChanged() {
            base.onVisibleChanged();
            m_hoveredCell = null;
            m_hoveredRow = null;
        }

        /// <summary>
        /// 移除列
        /// </summary>
        /// <param name="column">列</param>
        public virtual void removeColumn(FCGridColumn column) {
            bool selectedChanged = false;
            int selectedColumnsSize = m_selectedColumns.size();
            for (int i = 0; i < selectedColumnsSize; i++) {
                if (m_selectedColumns.get(i) == column) {
                    m_selectedColumns.remove(column);
                    selectedChanged = true;
                    break;
                }
            }
            m_columns.remove(column);
            int columnsSize = m_columns.size();
            for (int i = 0; i < columnsSize; i++) {
                m_columns.get(i).Index = i;
            }
            removeControl(column);
            int rowSize = m_rows.size();
            for (int i = 0; i < rowSize; i++) {
                FCGridRow row = m_rows.get(i);
                row.removeCell(column.Index);
            }
            if (selectedChanged) {
                onSelectedColumnsChanged();
            }
        }

        /// <summary>
        /// 移除行
        /// </summary>
        /// <param name="row">行</param>
        public void removeRow(FCGridRow row) {
            if (m_editingRow != null) {
                if (containsControl(m_editingRow.EditButton)) {
                    removeControl(m_editingRow.EditButton);
                }
                m_editingRow.m_editState = 0;
                m_editingRow = null;
            }
            if (m_animateAddRows.Contains(row)) {
                m_animateAddRows.remove(row);
            }
            bool selectedChanged = false;
            bool selected = false;
            int selectedRowsSize = (int)m_selectedRows.size();
            for (int i = 0; i < selectedRowsSize; i++) {
                FCGridRow selectedRow = m_selectedRows.get(i);
                if (selectedRow == row) {
                    selected = true;
                    break;
                }
            }
            if (selected) {
                FCGridRow otherRow = selectFrontRow();
                if (otherRow != null) {
                    selectedChanged = true;
                }
                else {
                    otherRow = selectNextRow();
                    if (otherRow != null) {
                        selectedChanged = true;
                    }
                }
            }
            if (m_hoveredRow == row) {
                m_hoveredCell = null;
                m_hoveredRow = null;
            }
            row.onRemove();
            m_rows.remove(row);
            int rowSize = m_rows.size();
            if (rowSize == 0) {
                m_selectedCells.clear();
                m_selectedRows.clear();
            }
            int visibleIndex = 0;
            for (int i = 0; i < rowSize; i++) {
                FCGridRow gridRow = m_rows.get(i);
                gridRow.Index = i;
                if (gridRow.Visible) {
                    gridRow.VisibleIndex = visibleIndex;
                    visibleIndex++;
                }
            }
            if (selected) {
                if (selectedChanged) {
                    onSelectedRowsChanged();
                }
                else {
                    m_selectedCells.clear();
                    m_selectedRows.clear();
                }
            }
        }

        /// <summary>
        /// 调整列的布局
        /// </summary>
        public virtual void resetHeaderLayout() {
            if (!m_lockUpdate) {
                int left = 0, top = 0;
                int scrollH = 0, scrollV = 0;
                FCHScrollBar hScrollBar = HScrollBar;
                FCVScrollBar vScrollBar = VScrollBar;
                int vScrollBarW = 0;
                if (hScrollBar != null && hScrollBar.Visible) {
                    scrollH = -hScrollBar.Pos;
                }
                if (vScrollBar != null && vScrollBar.Visible) {
                    scrollV = -vScrollBar.Pos;
                    vScrollBarW = vScrollBar.Width;
                }
                int headerHeight = m_headerVisible ? m_headerHeight : 0;
                //计算列的区域
                FCGridColumn draggingColumn = null;
                int columnSize = m_columns.size();
                for (int i = 0; i < columnSize; i++) {
                    FCGridColumn column = m_columns.get(i);
                    if (column.Visible) {
                        FCRect cellRect = new FCRect(left + m_horizontalOffset, top + m_verticalOffset,
                        left + m_horizontalOffset + column.Width, top + headerHeight + m_verticalOffset);
                        column.HeaderRect = cellRect;
                        if (column.IsDragging) {
                            draggingColumn = column;
                            column.Bounds = new FCRect(column.Left, cellRect.top, column.Right, cellRect.bottom);
                        }
                        else {
                            if (!column.Frozen) {
                                cellRect.left += scrollH;
                                cellRect.right += scrollH;
                            }
                            column.Bounds = cellRect;
                        }
                        left += column.Width;
                    }
                }
                //将控件置于最前
                for (int i = columnSize - 1; i >= 0; i--) {
                    m_columns.get(i).bringToFront();
                }
                if (draggingColumn != null) {
                    draggingColumn.bringToFront();
                }
                if (m_editingRow != null && m_editingRow.m_editState == 0 && m_editingRow.EditButton != null) {
                    FCView editButton = m_editingRow.EditButton;
                    editButton.Location = new FCPoint(AllVisibleColumnsWidth - editButton.Width + scrollH - vScrollBarW, m_editingRow.Bounds.top + scrollV);
                }
            }
        }

        /// <summary>
        /// 行是否可见
        /// </summary>
        /// <param name="bounds"></param>
        /// <param name="rowHeight"></param>
        /// <param name="scrollV"></param>
        /// <param name="visiblePercent"></param>
        /// <param name="cell"></param>
        /// <param name="floor"></param>
        /// <returns></returns>
        public virtual bool rowVisible(ref FCRect bounds, int rowHeight, int scrollV, double visiblePercent, int cell, int floor) {
            int rowtop = bounds.top + scrollV;
            int rowbottom = bounds.bottom + scrollV;
            if (rowtop < cell) {
                rowtop = cell;
            }
            else if (rowtop > floor) {
                rowtop = floor;
            }
            if (rowbottom < cell) {
                rowbottom = cell;
            }
            else if (rowbottom > floor) {
                rowbottom = floor;
            }
            if (rowbottom - rowtop > rowHeight * visiblePercent) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 选中上一行
        /// </summary>
        public FCGridRow selectFrontRow() {
            int rowsSize = m_rows.size();
            if (rowsSize == 0) {
                m_selectedRows.clear();
                m_selectedCells.clear();
                return null;
            }
            FCGridRow frontRow = null;
            ArrayList<FCGridRow> selectedRows = SelectedRows;
            if (selectedRows.size() == 1) {
                //获取向上的行
                FCGridRow selectedRow = selectedRows.get(0);
                int selectedIndex = selectedRow.Index;
                for (int i = selectedIndex - 1; i >= 0; i--) {
                    if (i < rowsSize && m_rows.get(i).Visible) {
                        frontRow = m_rows.get(i);
                        break;
                    }
                }
                //向上
                if (m_selectionMode == FCGridSelectionMode.SelectFullRow) {
                    if (frontRow != null) {
                        m_selectedRows.clear();
                        m_selectedRows.add(frontRow);
                        onSelectedRowsChanged();
                    }
                    else {
                        m_selectedRows.clear();
                        frontRow = m_rows.get(m_rows.size() - 1);
                        m_selectedRows.add(frontRow);
                        FCVScrollBar vScrollBar = VScrollBar;
                        if (vScrollBar != null && vScrollBar.Visible) {
                            vScrollBar.scrollToEnd();
                        }
                        onSelectedRowsChanged();
                    }
                }
            }
            return frontRow;
        }

        /// <summary>
        /// 选中下一行
        /// </summary>
        public FCGridRow selectNextRow() {
            int rowsSize = m_rows.size();
            if (rowsSize == 0) {
                m_selectedRows.clear();
                m_selectedCells.clear();
                return null;
            }
            FCGridRow nextRow = null;
            ArrayList<FCGridRow> selectedRows = SelectedRows;
            if (selectedRows.size() == 1) {
                FCGridRow selectedRow = selectedRows.get(0);
                int selectedIndex = selectedRow.Index;
                for (int i = selectedIndex + 1; i < rowsSize; i++) {
                    if (i >= 0 && m_rows.get(i).Visible) {
                        nextRow = m_rows.get(i);
                        break;
                    }
                }
                //向下
                if (m_selectionMode == FCGridSelectionMode.SelectFullRow) {
                    if (nextRow != null) {
                        m_selectedRows.clear();
                        m_selectedRows.add(nextRow);
                        onSelectedRowsChanged();
                    }
                    else {
                        m_selectedRows.clear();
                        nextRow = m_rows.get(0);
                        m_selectedRows.add(nextRow);
                        FCVScrollBar vScrollBar = VScrollBar;
                        if (vScrollBar != null && vScrollBar.Visible) {
                            vScrollBar.scrollToBegin();
                        }
                        onSelectedRowsChanged();
                    }
                }
            }
            return nextRow;
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        public override void setProperty(String name, String value) {
            if (name == "allowdragrow") {
                AllowDragRow = FCStr.convertStrToBool(value);
            }
            else if (name == "allowhoveredrow") {
                AllowHoveredRow = FCStr.convertStrToBool(value);
            }
            else if (name == "celleditmode") {
                value = value.ToLower();
                if (value == "doubleclick") {
                    CellEditMode = FCGridCellEditMode.DoubleClick;
                }
                else if (value == "none") {
                    CellEditMode = FCGridCellEditMode.None;
                }
                else if (value == "singleclick") {
                    CellEditMode = FCGridCellEditMode.SingleClick;
                }
            }
            else if (name == "gridlinecolor") {
                GridLineColor = FCStr.convertStrToColor(value);
            }
            else if (name == "headerheight") {
                HeaderHeight = FCStr.convertStrToInt(value);
            }
            else if (name == "headervisible") {
                HeaderVisible = FCStr.convertStrToBool(value);
            }
            else if (name == "horizontaloffset") {
                HorizontalOffset = FCStr.convertStrToInt(value);
            }
            else if (name == "multiselect") {
                MultiSelect = FCStr.convertStrToBool(value);
            }
            else if (name == "selectionmode") {
                value = value.ToLower();
                if (value == "selectcell") {
                    SelectionMode = FCGridSelectionMode.SelectCell;
                }
                else if (value == "selectfullcolumn") {
                    SelectionMode = FCGridSelectionMode.SelectFullColumn;
                }
                else if (value == "selectfullrow") {
                    SelectionMode = FCGridSelectionMode.SelectFullRow;
                }
                else {
                    SelectionMode = FCGridSelectionMode.SelectNone;
                }
            }
            else if (name == "useanimation") {
                UseAnimation = FCStr.convertStrToBool(value);
            }
            else if (name == "verticaloffset") {
                VerticalOffset = FCStr.convertStrToInt(value);
            }
            else {
                base.setProperty(name, value);
            }
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="grid">表格</param>
        /// <param name="column">列</param>
        /// <param name="sortMode">排序方式</param>
        public virtual void sortColumn(FCGrid grid, FCGridColumn column, FCGridColumnSortMode sortMode) {
            if (column.AllowSort) {
                //取消原有排序
                int colSize = grid.m_columns.size();
                for (int i = 0; i < colSize; i++) {
                    if (grid.m_columns.get(i) != column) {
                        grid.m_columns.get(i).SortMode = FCGridColumnSortMode.None;
                    }
                    else {
                        grid.m_columns.get(i).SortMode = sortMode;
                    }
                }
                //重新排序，要改成二叉树排序
                if (m_sort != null) {
                    m_sort.sortColumn(this, column, sortMode);
                }
                grid.update();
                grid.invalidate();
            }
        }

        /// <summary>
        /// 触摸事件
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        /// <param name="state">0:移动 1:按下 2:抬起</param>
        protected void touchEvent(FCTouchInfo touchInfo, int state) {
            FCPoint mp = touchInfo.m_firstPoint;
            int height = Height;
            int hHeight = m_headerVisible ? m_headerHeight : 0;
            int scrollH = 0, scrollV = 0;
            FCHost host = Native.Host;
            FCHScrollBar hScrollBar = HScrollBar;
            FCVScrollBar vScrollBar = VScrollBar;
            if (hScrollBar != null && hScrollBar.Visible) {
                scrollH = -hScrollBar.Pos;
            }
            if (vScrollBar != null && vScrollBar.Visible) {
                scrollV = -vScrollBar.Pos;
            }
            FCPoint fPoint = new FCPoint(0, hHeight + 1 - scrollV);
            FCPoint ePoint = new FCPoint(0, height - 10 - scrollV);
            FCGridRow fRow = getRow(fPoint);
            FCGridRow eRow = getRow(ePoint);
            while (eRow == null && ePoint.y > 0) {
                ePoint.y -= 10;
                eRow = getRow(ePoint);
            }
            if (fRow != null && eRow != null) {
                int fIndex = fRow.Index;
                int eIndex = eRow.Index;
                for (int i = fIndex; i <= eIndex; i++) {
                    FCGridRow row = m_rows.get(i);
                    if (row.Visible) {
                        FCRect rowRect = row.Bounds;
                        rowRect.top += scrollV;
                        rowRect.bottom += scrollV;
                        ArrayList<FCGridCell> cells = null;
                        ArrayList<FCGridCell> unFrozenCells = new ArrayList<FCGridCell>();
                        for (int j = 0; j < 2; j++) {
                            if (j == 0) {
                                cells = row.getCells();
                            }
                            else {
                                cells = unFrozenCells;
                            }
                            int cellSize = cells.size();
                            for (int c = 0; c < cellSize; c++) {
                                FCGridCell cell = cells.get(c);
                                FCGridColumn column = cell.Column;
                                if (column.Visible) {
                                    if (j == 0 && !column.Frozen) {
                                        unFrozenCells.add(cell);
                                        continue;
                                    }
                                    //获取单元格的矩形
                                    FCRect headerRect = column.HeaderRect;
                                    if (!column.Frozen) {
                                        headerRect.left += scrollH;
                                        headerRect.right += scrollH;
                                    }
                                    int cellWidth = column.Width;
                                    int colSpan = cell.ColSpan;
                                    if (colSpan > 1) {
                                        for (int n = 1; n < colSpan; n++) {
                                            FCGridColumn spanColumn = getColumn(column.Index + n);
                                            if (spanColumn != null && spanColumn.Visible) {
                                                cellWidth += spanColumn.Width;
                                            }
                                        }
                                    }
                                    int cellHeight = row.Height;
                                    int rowSpan = cell.RowSpan;
                                    if (rowSpan > 1) {
                                        for (int n = 1; n < rowSpan; n++) {
                                            FCGridRow spanRow = getRow(i + n);
                                            if (spanRow != null && spanRow.Visible) {
                                                cellHeight += spanRow.Height;
                                            }
                                        }
                                    }
                                    FCRect cellRect = new FCRect(headerRect.left, rowRect.top + m_verticalOffset, headerRect.left + cellWidth, rowRect.top + m_verticalOffset + cellHeight);
                                    if (mp.x >= cellRect.left && mp.x <= cellRect.right
                                        && mp.y >= cellRect.top && mp.y <= cellRect.bottom) {
                                        if (state == 0) {
                                            bool hoverChanged = false;
                                            if (m_allowHoveredRow && m_hoveredRow != row) {
                                                m_hoveredRow = row;
                                                hoverChanged = true;
                                            }
                                            if (Native.PushedControl == this) {
                                                if (m_allowDragRow) {
                                                    if (m_selectionMode == FCGridSelectionMode.SelectFullRow) {
                                                        int selectedRowsSize = m_selectedRows.size();
                                                        if (selectedRowsSize == 1) {
                                                            if (m_selectedRows.get(0) != row) {
                                                                moveRow(m_selectedRows.get(0).Index, row.Index);
                                                                hoverChanged = true;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            if (m_hoveredCell != cell) {
                                                if (m_hoveredCell != null) {
                                                    onCellTouchLeave(m_hoveredCell, touchInfo);
                                                }
                                                m_hoveredCell = cell;
                                                onCellTouchEnter(m_hoveredCell, touchInfo);
                                            }
                                            onCellTouchMove(cell, touchInfo);
                                            //检查行的编辑
                                            if (m_editingRow == null) {
                                                if (row.AllowEdit) {
                                                    if (Native.PushedControl == this) {
                                                        int selectedRowsSize = m_selectedRows.size();
                                                        if (selectedRowsSize == 1) {
                                                            if (m_selectedRows.get(0) == row) {
                                                                onRowEditBegin(row);
                                                                hoverChanged = true;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            if (hoverChanged) {
                                                Native.invalidate();
                                            }
                                        }
                                        else {
                                            //触摸按下
                                            if (state == 1) {
                                                onCellTouchDown(cell, touchInfo);
                                                m_touchDownPoint = mp;
                                                if (touchInfo.m_firstTouch && touchInfo.m_clicks == 1) {
                                                    int multiSelectMode = 0;
                                                    //判断是否多选
                                                    if (m_multiSelect) {
                                                        if (host.isKeyPress(0x10)) {
                                                            multiSelectMode = 1;
                                                        }
                                                        else if (host.isKeyPress(0x11)) {
                                                            multiSelectMode = 2;
                                                        }
                                                    }
                                                    //选中单元格
                                                    if (m_selectionMode == FCGridSelectionMode.SelectCell) {
                                                        bool contains = false;
                                                        bool selectedChanged = false;
                                                        int selectedCellSize = m_selectedCells.size();
                                                        if (multiSelectMode == 0 || multiSelectMode == 2) {
                                                            for (int m = 0; m < selectedCellSize; m++) {
                                                                if (m_selectedCells.get(m) == cell) {
                                                                    contains = true;
                                                                    if (multiSelectMode == 2) {
                                                                        m_selectedCells.remove(cell);
                                                                        selectedChanged = true;
                                                                    }
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                        if (multiSelectMode == 0) {
                                                            selectedCellSize = m_selectedCells.size();
                                                            if (!contains || selectedCellSize > 1) {
                                                                m_selectedCells.clear();
                                                                m_selectedCells.add(cell);
                                                                selectedChanged = true;
                                                            }
                                                        }
                                                        else if (multiSelectMode == 2) {
                                                            if (!contains) {
                                                                m_selectedCells.add(cell);
                                                                selectedChanged = true;
                                                            }
                                                        }
                                                        if (selectedChanged) {
                                                            onSelectedCellsChanged();
                                                        }
                                                    }
                                                    //选中全列
                                                    else if (m_selectionMode == FCGridSelectionMode.SelectFullColumn) {
                                                        bool contains = false;
                                                        bool selectedChanged = false;
                                                        int selectedColumnsSize = m_selectedColumns.size();
                                                        if (multiSelectMode == 0 || multiSelectMode == 2) {
                                                            for (int m = 0; m < selectedColumnsSize; m++) {
                                                                if (m_selectedColumns.get(m) == column) {
                                                                    contains = true;
                                                                    if (multiSelectMode == 2) {
                                                                        m_selectedColumns.remove(column);
                                                                        selectedChanged = true;
                                                                    }
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                        if (multiSelectMode == 0) {
                                                            if (!contains || selectedColumnsSize > 1) {
                                                                m_selectedColumns.clear();
                                                                m_selectedColumns.add(column);
                                                                selectedChanged = true;
                                                            }
                                                        }
                                                        else if (multiSelectMode == 2) {
                                                            if (!contains) {
                                                                m_selectedColumns.add(column);
                                                                selectedChanged = true;
                                                            }
                                                        }
                                                        //单元格必选中
                                                        m_selectedCells.clear();
                                                        m_selectedCells.add(cell);
                                                        if (selectedChanged) {
                                                            onSelectedColumnsChanged();
                                                        }
                                                    }
                                                    //选中全行
                                                    else if (m_selectionMode == FCGridSelectionMode.SelectFullRow) {
                                                        bool contains = false;
                                                        bool selectedChanged = false;
                                                        int selectedRowsSize = m_selectedRows.size();
                                                        if (multiSelectMode == 0 || multiSelectMode == 2) {
                                                            for (int m = 0; m < selectedRowsSize; m++) {
                                                                if (m_selectedRows.get(m) == row) {
                                                                    contains = true;
                                                                    if (multiSelectMode == 2) {
                                                                        m_selectedRows.remove(row);
                                                                        selectedChanged = true;
                                                                    }
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                        if (multiSelectMode == 0) {
                                                            selectedRowsSize = m_selectedRows.size();
                                                            if (!contains || selectedRowsSize > 1) {
                                                                m_selectedRows.clear();
                                                                m_selectedRows.add(row);
                                                                selectedChanged = true;
                                                            }
                                                        }
                                                        else if (multiSelectMode == 1) {
                                                            selectedRowsSize = m_selectedRows.size();
                                                            if (selectedRowsSize > 0) {
                                                                int firstIndex = m_selectedRows.get(0).Index;
                                                                int newIndex = row.Index;
                                                                int minIndex = Math.Min(firstIndex, newIndex);
                                                                int maxIndex = Math.Max(firstIndex, newIndex);
                                                                m_selectedRows.clear();
                                                                for (int s = minIndex; s <= maxIndex; s++) {
                                                                    m_selectedRows.add(getRow(s));
                                                                }
                                                            }
                                                            else {
                                                                m_selectedRows.add(row);
                                                            }
                                                        }
                                                        else if (multiSelectMode == 2) {
                                                            if (!contains) {
                                                                m_selectedRows.add(row);
                                                                selectedChanged = true;
                                                            }
                                                        }
                                                        //单元格必选中
                                                        m_selectedCells.clear();
                                                        m_selectedCells.add(cell);
                                                        if (selectedChanged) {
                                                            onSelectedRowsChanged();
                                                        }
                                                    }
                                                }
                                            }
                                            //触摸抬起
                                            else if (state == 2) {
                                                onCellTouchUp(cell, touchInfo);
                                            }
                                            if (state == 2 || (touchInfo.m_clicks == 2 && state == 1)) {
                                                if (m_selectedCells.size() > 0 && m_selectedCells.get(0) == cell) {
                                                    onCellClick(cell, touchInfo);
                                                    if (touchInfo.m_firstTouch && cell.AllowEdit) {
                                                        if ((m_cellEditMode == FCGridCellEditMode.DoubleClick && (touchInfo.m_clicks == 2 && state == 1))
                                                            || (m_cellEditMode == FCGridCellEditMode.SingleClick && state == 2)) {
                                                            onCellEditBegin(cell);
                                                        }
                                                    }
                                                }
                                            }
                                            invalidate();
                                        }
                                        unFrozenCells.clear();
                                        if (state == 1 && m_editingRow != null) {
                                            //取消行的编辑
                                            onRowEditEnd();
                                        }
                                        return;
                                    }
                                }
                            }
                        }
                        unFrozenCells.clear();
                    }
                }
            }
            if (state == 1 && m_editingRow != null) {
                //取消行的编辑
                onRowEditEnd();
            }
        }

        /// <summary>
        /// 重新布局
        /// </summary>
        public override void update() {
            if (Native != null) {
                if (!m_lockUpdate) {
                    base.update();
                    if (Visible) {
                        int colSize = m_columns.size();
                        for (int i = 0; i < colSize; i++) {
                            m_columns.get(i).Index = i;
                        }
                        int rowSize = m_rows.size();
                        int visibleIndex = 0;
                        int rowTop = m_headerVisible ? m_headerHeight : 0;
                        int allVisibleColumnsWidth = AllVisibleColumnsWidth;
                        m_hasUnVisibleRow = false;
                        for (int i = 0; i < rowSize; i++) {
                            FCGridRow gridRow = m_rows.get(i);
                            gridRow.Index = i;
                            if (gridRow.Visible) {
                                gridRow.VisibleIndex = i;
                                int rowHeight = gridRow.Height;
                                FCRect rowRect = new FCRect(0, rowTop, allVisibleColumnsWidth, rowTop + rowHeight);
                                gridRow.Bounds = rowRect;
                                rowTop += rowHeight;
                                visibleIndex++;
                            }
                            else {
                                m_hasUnVisibleRow = true;
                                gridRow.VisibleIndex = -1;
                                FCRect rowRect = new FCRect(0, rowTop, allVisibleColumnsWidth, rowTop);
                                gridRow.Bounds = rowRect;
                            }
                        }
                        FCHScrollBar hScrollBar = HScrollBar;
                        FCVScrollBar vScrollBar = VScrollBar;
                        if (vScrollBar != null && vScrollBar.Visible) {
                            int top = m_headerVisible ? m_headerHeight : 0;
                            vScrollBar.Top = top;
                            int height = Height - top - ((hScrollBar != null && hScrollBar.Visible) ? hScrollBar.Height : 0);
                            vScrollBar.Height = height;
                            vScrollBar.PageSize = height;
                            if (rowSize > 0) {
                                vScrollBar.LineSize = AllVisibleRowsHeight / rowSize;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 更新列排序
        /// </summary>
        public virtual void updateSortColumn() {
            //重新排序
            int columnSize = m_columns.size();
            for (int i = 0; i < columnSize; i++) {
                if (m_columns.get(i).SortMode != FCGridColumnSortMode.None) {
                    sortColumn(this, m_columns.get(i), m_columns.get(i).SortMode);
                    break;
                }
            }
        }
    }
}
