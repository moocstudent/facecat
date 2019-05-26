/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.grid;

import facecat.topin.input.*;
import facecat.topin.scroll.*;
import facecat.topin.div.*;
import facecat.topin.core.*;
import java.util.*;

/**
 * 表格控件
 */
public class FCGrid extends FCDiv implements FCEvent {

    /**
     * 创建表格
     */
    public FCGrid() {
        setShowHScrollBar(true);
        setShowVScrollBar(true);
    }

    /**
     * 动画添加的行
     */
    public ArrayList<FCGridRow> m_animateAddRows = new ArrayList<FCGridRow>();

    /**
     * 动画移除的行
     */
    public ArrayList<FCGridRow> m_animateRemoveRows = new ArrayList<FCGridRow>();

    /**
     * 列的集合
     */
    public ArrayList<FCGridColumn> m_columns = new ArrayList<FCGridColumn>();

    /**
     * 正在编辑的单元格
     */
    protected FCGridCell m_editingCell = null;

    /**
     * 正在编辑的行
     */
    public FCGridRow m_editingRow;

    /**
     * 是否包含不可见行
     */
    protected boolean m_hasUnVisibleRow = false;

    /**
     * 是否锁定界面更新
     */
    protected boolean m_lockUpdate = false;

    /**
     * 行的集合
     */
    private FCPoint m_touchDownPoint;

    public ArrayList<FCGridRow> m_rows = new ArrayList<FCGridRow>();

    /**
     * 秒表ID
     */
    private int m_timerID = getNewTimerID();

    protected boolean m_allowDragRow = false;

    /**
     * 获取是否允许拖动行
     */
    public boolean allowDragRow() {
        return m_allowDragRow;
    }

    /**
     * 设置是否允许拖动行
     */
    public void setAllowDragRow(boolean value) {
        m_allowDragRow = value;
    }

    protected boolean m_allowHoveredRow = true;

    /**
     * 获取是否允许悬停行
     */
    public boolean allowHoveredRow() {
        return m_allowHoveredRow;
    }

    /**
     * 设置是否允许悬停行
     */
    public void setAllowHoveredRow(boolean value) {
        m_allowHoveredRow = value;
    }

    /**
     * 获取所有可见列的宽度
     */
    protected int getAllVisibleColumnsWidth() {
        int allVisibleColumnsWidth = 0;
        int columnSize = m_columns.size();
        for (int i = 0; i < columnSize; i++) {
            FCGridColumn column = m_columns.get(i);
            if (column.isVisible()) {
                allVisibleColumnsWidth += column.getWidth();
            }
        }
        return allVisibleColumnsWidth;
    }

    /**
     * 获取所有可见行的高度
     */
    protected int getAllVisibleRowsHeight() {
        int allVisibleRowsHeight = 0;
        int rowSize = m_rows.size();
        for (int i = 0; i < rowSize; i++) {
            if (m_rows.get(i).isVisible()) {
                allVisibleRowsHeight += m_rows.get(i).getHeight();
            }
        }
        return allVisibleRowsHeight;
    }

    protected FCGridRowStyle m_alternateRowStyle = null;

    /**
     * 获取交替行的样式
     */
    public FCGridRowStyle getAlternateRowStyle() {
        return m_alternateRowStyle;
    }

    /**
     * 设置交替行的样式
     */
    public void setAlternateRowStyle(FCGridRowStyle value) {
        m_alternateRowStyle = value;
    }

    protected FCGridCellEditMode m_cellEditMode = FCGridCellEditMode.SingleClick;

    /**
     * 获取单元格编辑模式
     */
    public FCGridCellEditMode getCellEditMode() {
        return m_cellEditMode;
    }

    /**
     * 设置单元格编辑模式
     */
    public void setCellEditMode(FCGridCellEditMode value) {
        m_cellEditMode = value;
    }

    protected FCTextBox m_editTextBox = null;

    /**
     * 获取编辑文本框
     */
    public FCTextBox getEditTextBox() {
        return m_editTextBox;
    }

    protected long m_gridLineColor = FCColor.argb(100, 100, 100);

    /**
     * 获取网格线的颜色
     */
    public long getGridLineColor() {
        return m_gridLineColor;
    }

    /**
     * 设置网格线的颜色
     */
    public void setGridLineColor(long value) {
        m_gridLineColor = value;
    }

    protected boolean m_headerVisible = true;

    /**
     * 获取标题头是否可见
     */
    public boolean isHeaderVisible() {
        return m_headerVisible;
    }

    /**
     * 设置标题头是否可见
     */
    public void setHeaderVisible(boolean value) {
        m_headerVisible = value;
    }

    protected int m_headerHeight = 20;

    /**
     * 获取标题头的高度
     */
    public int getHeaderHeight() {
        return m_headerHeight;
    }

    /**
     * 设置标题头的高度
     */
    public void setHeaderHeight(int value) {
        m_headerHeight = value;
    }

    protected int m_horizontalOffset = 0;

    /**
     * 获取横向滚动距离
     */
    public int getHorizontalOffset() {
        return m_horizontalOffset;
    }

    /**
     * 设置横向滚动距离
     */
    public void setHorizontalOffset(int value) {
        m_horizontalOffset = value;
    }

    protected FCGridCell m_hoveredCell = null;

    /**
     * 获取触摸悬停的单元格
     */
    public FCGridCell getHoveredCell() {
        return m_hoveredCell;
    }

    protected FCGridRow m_hoveredRow = null;

    /**
     * 获取触摸悬停的行
     */
    public FCGridRow getHoveredRow() {
        return m_hoveredRow;
    }

    protected boolean m_multiSelect = false;

    /**
     * 获取是否多选
     */
    public boolean multiSelect() {
        return m_multiSelect;
    }

    /**
     * 设置是否多选
     */
    public void setMultiSelect(boolean value) {
        m_multiSelect = value;
    }

    protected FCGridRowStyle m_rowStyle = new FCGridRowStyle();

    /**
     * 获取行的样式
     */
    public FCGridRowStyle getRowStyle() {
        return m_rowStyle;
    }

    /**
     * 设置行的样式
     */
    public void setRowStyle(FCGridRowStyle value) {
        m_rowStyle = value;
    }

    protected ArrayList<FCGridCell> m_selectedCells = new ArrayList<FCGridCell>();

    /**
     * 获取选中的单元格
     */
    public ArrayList<FCGridCell> getSelectedCells() {
        return m_selectedCells;
    }

    /**
     * 设置选中的单元格
     */
    public void setSelectedCells(ArrayList<FCGridCell> value) {
        m_selectedCells.clear();
        int selectedCellsSize = value.size();
        for (int i = 0; i < selectedCellsSize; i++) {
            m_selectedCells.add(value.get(i));
        }
        onSelectedCellsChanged();
    }

    protected ArrayList<FCGridColumn> m_selectedColumns = new ArrayList<FCGridColumn>();

    /**
     * 获取选中的列
     */
    public ArrayList<FCGridColumn> getSelectedColumns() {
        return m_selectedColumns;
    }

    /**
     * 设置选中的列
     */
    public void setSelectedColumns(ArrayList<FCGridColumn> value) {
        m_selectedColumns.clear();
        int selectedColumnsSize = value.size();
        for (int i = 0; i < selectedColumnsSize; i++) {
            m_selectedColumns.add(value.get(i));
        }
        onSelectedColumnsChanged();
    }

    protected ArrayList<FCGridRow> m_selectedRows = new ArrayList<FCGridRow>();

    /**
     * 获取选中行
     */
    public ArrayList<FCGridRow> getSelectedRows() {
        return m_selectedRows;
    }

    /**
     * 设置选中行
     */
    public void setSelectedRows(ArrayList<FCGridRow> value) {
        m_selectedRows.clear();
        int selectedRowsSize = value.size();
        for (int i = 0; i < selectedRowsSize; i++) {
            m_selectedRows.add(value.get(i));
        }
        onSelectedRowsChanged();
    }

    protected FCGridSelectionMode m_selectionMode = FCGridSelectionMode.SelectFullRow;

    /**
     * 选中模式
     */
    public FCGridSelectionMode getSelectionMode() {
        return m_selectionMode;
    }

    public void setSelectionMode(FCGridSelectionMode value) {
        m_selectionMode = value;
    }

    protected FCGridSort m_sort = new FCGridSort();

    /**
     * 获取排序处理类
     */
    public FCGridSort getSort() {
        return m_sort;
    }

    /**
     * 设置排序处理类
     */
    public void setSort(FCGridSort sort) {
        m_sort = sort;
    }

    protected boolean m_useAnimation = false;

    /**
     * 获取是否使用动画
     */
    public boolean useAnimation() {
        return m_useAnimation;
    }

    /**
     * 设置是否使用动画
     */
    public void setuseAnimation(boolean value) {
        m_useAnimation = value;
        if (m_useAnimation) {
            startTimer(m_timerID, 20);
        } else {
            stopTimer(m_timerID);
        }
    }

    protected int m_verticalOffset = 0;

    /**
     * 获取纵向滚动距离
     */
    public int getVerticalOffset() {
        return m_verticalOffset;
    }

    /**
     * 设置纵向滚动距离
     */
    public void setVerticalOffset(int value) {
        m_verticalOffset = value;
    }

    /**
     * 添加列
     *
     * @param column 列
     */
    public void addColumn(FCGridColumn column) {
        column.setGrid(this);
        m_columns.add(column);
        int columnsSize = m_columns.size();
        for (int i = 0; i < columnsSize; i++) {
            m_columns.get(i).setIndex(i);
        }
        addControl(column);
    }

    /**
     * 添加行
     *
     * @param row 行
     */
    public void addRow(FCGridRow row) {
        row.setGrid(this);
        m_rows.add(row);
        row.onadd();
        if (m_selectionMode == FCGridSelectionMode.SelectFullRow) {
            int selectedRowsSize = m_selectedRows.size();
            if (selectedRowsSize == 0) {
                m_selectedRows.add(row);
                onSelectedRowsChanged();
            }
        }
    }

    /**
     * 动画添加行
     *
     * @param row 行
     */
    public void animateAddRow(FCGridRow row) {
        row.setGrid(this);
        m_rows.add(row);
        row.onadd();
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

    /**
     * 动画移除行
     *
     * @param row 行
     */
    public void animateRemoveRow(FCGridRow row) {
        if (m_useAnimation) {
            m_animateRemoveRows.add(row);
        } else {
            removeRow(row);
        }
    }

    /**
     * 开始更新
     */
    public void beginUpdate() {
        m_lockUpdate = true;
    }

    /**
     * 调用单元格事件
     *
     * @param eventID 事件ID
     * @param cell 单元格
     */
    protected void callCellEvents(int eventID, FCGridCell cell) {
        if (m_events != null && m_events.containsKey(eventID)) {
            ArrayList<Object> events = m_events.get(eventID);
            int eventSize = events.size();
            for (int i = 0; i < eventSize; i++) {
                FCGridCellEvent func = (FCGridCellEvent) ((events.get(i) instanceof FCGridCellEvent) ? events.get(i) : null);
                if (func != null) {
                    func.callGridCellEvent(eventID, this, cell);
                }
            }
        }
    }

    /**
     * 调用单元格触摸事件
     *
     * @param eventID 事件ID
     * @param cell 单元格
     * @param touchInfo 触摸信息
     */
    protected void callCellTouchEvents(int eventID, FCGridCell cell, FCTouchInfo touchInfo) {
        if (m_events != null && m_events.containsKey(eventID)) {
            ArrayList<Object> events = m_events.get(eventID);
            int eventSize = events.size();
            for (int i = 0; i < eventSize; i++) {
                FCGridCellTouchEvent func = (FCGridCellTouchEvent) ((events.get(i) instanceof FCGridCellTouchEvent) ? events.get(i) : null);
                if (func != null) {
                    func.callGridCellTouchEvent(eventID, this, cell, touchInfo.clone());
                }
            }
        }
    }

    public void callEvent(int eventID, Object sender) {
        if (eventID == FCEventID.LOSTFOCUS && sender == m_editTextBox) {
            if (m_editTextBox != null && m_editTextBox.getTag() != null) {
                Object tempVar = m_editTextBox.getTag();
                onCellEditEnd((FCGridCell) ((tempVar instanceof FCGridCell) ? tempVar : null));
            }
        }
    }

    /**
     * 请除数据
     */
    public void clear() {
        clearRows();
        clearColumns();
    }

    /**
     * 清除所有列
     */
    public void clearColumns() {
        m_selectedColumns.clear();
        int columnSize = m_columns.size();
        for (int i = 0; i < columnSize; i++) {
            removeControl(m_columns.get(i));
            m_columns.get(i).delete();
        }
        m_columns.clear();
    }

    /**
     * 清除所有行
     */
    public void clearRows() {
        m_hasUnVisibleRow = false;
        m_hoveredCell = null;
        m_hoveredRow = null;
        m_selectedRows.clear();
        int rowSize = m_rows.size();
        for (int i = 0; i < rowSize; i++) {
            m_rows.get(i).onremove();
            m_rows.get(i).delete();
        }
        m_rows.clear();
    }

    /**
     * 销毁控件的方法
     */
    @Override
    public void delete() {
        if (!isDeleted()) {
            m_editingCell = null;
            if (m_editTextBox != null) {
                m_editTextBox.removeEvent(this, FCEventID.LOSTFOCUS);
                m_editTextBox = null;
            }
            stopTimer(m_timerID);
            clear();
        }
        super.delete();
    }

    /**
     * 结束更新
     */
    public void endUpdate() {
        if (m_lockUpdate) {
            m_lockUpdate = false;
            update();
        }
    }

    /**
     * 获取表格列
     *
     * @param columnIndex 列的索引
     * @returns 表格列
     */
    public FCGridColumn getColumn(int columnIndex) {
        if (columnIndex >= 0 && columnIndex < m_columns.size()) {
            return m_columns.get(columnIndex);
        }
        return null;
    }

    /**
     * 获取表格列
     *
     * @param columnName 列名
     * @returns 表格列
     */
    public FCGridColumn getColumn(String columnName) {
        int colSize = m_columns.size();
        for (int i = 0; i < colSize; i++) {
            if (m_columns.get(i).getName().equals(columnName)) {
                return m_columns.get(i);
            }
        }
        return null;
    }

    /**
     * 获取所有的列
     */
    public ArrayList<FCGridColumn> getColumns() {
        return m_columns;
    }

    /**
     * 获取内容的高度
     */
    @Override
    public int getContentHeight() {
        int allVisibleRowsHeight = getAllVisibleRowsHeight();
        if (allVisibleRowsHeight > 0) {
            if (allVisibleRowsHeight <= getHeight()) {
                allVisibleRowsHeight += m_headerVisible ? m_headerHeight : 0;
            }
            return allVisibleRowsHeight;
        } else {
            return 0;
        }
    }

    /**
     * 获取内容的宽度
     */
    @Override
    public int getContentWidth() {
        return getAllVisibleColumnsWidth();
    }

    /**
     * 获取控件类型
     */
    @Override
    public String getControlType() {
        return "Grid";
    }

    /**
     * 获取显示偏移坐标
     */
    @Override
    public FCPoint getDisplayOffset() {
        return new FCPoint(0, 0);
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
        if (name.equals("allowdragrow")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(allowDragRow());
        } else if (name.equals("allowhoveredrow")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(allowHoveredRow());
        } else if (name.equals("celleditmode")) {
            type.argvalue = "enum:GridCellEditMode";
            FCGridCellEditMode cellEditMode = getCellEditMode();
            if (cellEditMode == FCGridCellEditMode.DoubleClick) {
                value.argvalue = "DoubleClick";
            } else if (cellEditMode == FCGridCellEditMode.None) {
                value.argvalue = "None";
            } else {
                value.argvalue = "SingleClick";
            }
        } else if (name.equals("gridlinecolor")) {
            type.argvalue = "color";
            value.argvalue = FCStr.convertColorToStr(getGridLineColor());
        } else if (name.equals("headerheight")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getHeaderHeight());
        } else if (name.equals("headervisible")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(isHeaderVisible());
        } else if (name.equals("horizontaloffset")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getHorizontalOffset());
        } else if (name.equals("multiselect")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(multiSelect());
        } else if (name.equals("selectionmode")) {
            type.argvalue = "enum:GridSelectionMode";
            FCGridSelectionMode selectionMode = getSelectionMode();
            if (selectionMode == FCGridSelectionMode.SelectCell) {
                value.argvalue = "SelectCell";
            } else if (selectionMode == FCGridSelectionMode.SelectFullColumn) {
                value.argvalue = "SelectFullColumn";
            } else if (selectionMode == FCGridSelectionMode.SelectFullRow) {
                value.argvalue = "SelectFullRow";
            } else {
                value.argvalue = "None";
            }
        } else if (name.equals("useanimation")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(useAnimation());
        } else if (name.equals("verticaloffset")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getVerticalOffset());
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
        propertyNames.addAll(Arrays.asList(new String[]{"AllowDragRow", "AllowHoveredRow", "CellEditMode", "GridLineColor", "HeaderHeight", "HeaderVisible", "HorizontalOffset", "MultiSelect", "SelectionMode", "UseAnimation", "VerticalOffset"}));
        return propertyNames;
    }

    /**
     * 根据坐标获取行
     *
     * @param mp 坐标
     * @returns 表格行
     */
    public FCGridRow getRow(FCPoint mp) {
        if (m_hasUnVisibleRow) {
            int rowsSize = m_rows.size();
            for (int i = 0; i < rowsSize; i++) {
                FCGridRow row = m_rows.get(i);
                if (row.isVisible()) {
                    FCRect bounds = row.getBounds();
                    if (mp.y >= bounds.top && mp.y <= bounds.bottom) {
                        return row;
                    }
                }
            }
        } else {
            int begin = 0;
            int end = m_rows.size() - 1;
            int sub = end - begin;
            while (sub >= 0) {
                int half = begin + sub / 2;
                FCGridRow row = m_rows.get(half);
                FCRect bounds = row.getBounds();
                if (half == begin || half == end) {
                    if (mp.y >= m_rows.get(begin).getBounds().top && mp.y <= m_rows.get(begin).getBounds().bottom) {
                        return m_rows.get(begin);
                    }
                    if (mp.y >= m_rows.get(end).getBounds().top && mp.y <= m_rows.get(end).getBounds().bottom) {
                        return m_rows.get(end);
                    }
                    break;
                }
                if (mp.y >= bounds.top && mp.y <= bounds.bottom) {
                    return row;
                } else if (bounds.top > mp.y) {
                    end = half;
                } else if (bounds.bottom < mp.y) {
                    begin = half;
                }
                sub = end - begin;
            }
        }
        return null;
    }

    /**
     * 获取表格行
     *
     * @param rowIndex 行的索引
     * @returns 表格行
     */
    public FCGridRow getRow(int rowIndex) {
        if (rowIndex >= 0 && rowIndex < m_rows.size()) {
            return m_rows.get(rowIndex);
        }
        return null;
    }

    /**
     * 获取所有的行
     */
    public ArrayList<FCGridRow> getRows() {
        return m_rows;
    }

    /**
     * 获取可见的行索引
     *
     * @param visiblePercent 可见百分比
     * @param firstVisibleRowIndex 首先可见的行索引
     * @param lastVisibleRowIndex 最后可见的行索引
     */
    public void getVisibleRowsIndex(double visiblePercent, RefObject<Integer> firstVisibleRowIndex, RefObject<Integer> lastVisibleRowIndex) {
        firstVisibleRowIndex.argvalue = -1;
        lastVisibleRowIndex.argvalue = -1;
        int rowsSize = m_rows.size();
        if (rowsSize > 0) {
            for (int i = 0; i < rowsSize; i++) {
                FCGridRow row = m_rows.get(i);
                if (isRowVisible(row, visiblePercent)) {
                    if (firstVisibleRowIndex.argvalue == -1) {
                        firstVisibleRowIndex.argvalue = i;
                    }
                } else {
                    if (firstVisibleRowIndex.argvalue != -1) {
                        lastVisibleRowIndex.argvalue = i;
                        break;
                    }
                }
            }
            if (firstVisibleRowIndex.argvalue != -1 && lastVisibleRowIndex.argvalue == -1) {
                lastVisibleRowIndex.argvalue = firstVisibleRowIndex.argvalue;
            }
        }
    }

    /**
     * 插入行
     *
     * @param index 索引
     * @param row 行
     */
    public void insertRow(int index, FCGridRow row) {
        row.setGrid(this);
        m_rows.add(index, row);
        row.onadd();
    }

    /**
     * 判断行是否可见
     *
     * @param row 行
     * @param visiblePercent 可见百分比
     * @returns 是否可见
     */
    public boolean isRowVisible(FCGridRow row, double visiblePercent) {
        int scrollV = 0;
        FCVScrollBar vScrollBar = getVScrollBar();
        if (vScrollBar != null && vScrollBar.isVisible()) {
            scrollV = -vScrollBar.getPos();
        }
        int cell = (m_headerVisible ? m_headerHeight : 0);
        int floor = getHeight() - cell;
        FCRect bounds = row.getBounds();
        int rowtop = bounds.top + scrollV;
        int rowbottom = bounds.bottom + scrollV;
        if (rowtop < cell) {
            rowtop = cell;
        } else if (rowtop > floor) {
            rowtop = floor;
        }
        if (rowbottom < cell) {
            rowbottom = cell;
        } else if (rowbottom > floor) {
            rowbottom = floor;
        }
        if (rowbottom - rowtop > row.getHeight() * visiblePercent) {
            return true;
        }
        return false;
    }

    protected void touchEvent(FCTouchInfo touchInfo, int state) {
        FCPoint mp = touchInfo.m_firstPoint.clone();
        int height = getHeight();
        int hHeight = m_headerVisible ? m_headerHeight : 0;
        int scrollH = 0, scrollV = 0;
        FCHost host = getNative().getHost();
        FCHScrollBar hScrollBar = getHScrollBar();
        FCVScrollBar vScrollBar = getVScrollBar();
        if (hScrollBar != null && hScrollBar.isVisible()) {
            scrollH = -hScrollBar.getPos();
        }
        if (vScrollBar != null && vScrollBar.isVisible()) {
            scrollV = -vScrollBar.getPos();
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
            int fIndex = fRow.getIndex();
            int eIndex = eRow.getIndex();
            for (int i = fIndex; i <= eIndex; i++) {
                FCGridRow row = m_rows.get(i);
                if (row.isVisible()) {
                    FCRect rowRect = row.getBounds();
                    rowRect.top += scrollV;
                    rowRect.bottom += scrollV;
                    ArrayList<FCGridCell> cells = null;
                    ArrayList<FCGridCell> unFrozenCells = new ArrayList<FCGridCell>();
                    for (int j = 0; j < 2; j++) {
                        if (j == 0) {
                            cells = row.getCells();
                        } else {
                            cells = unFrozenCells;
                        }
                        int cellSize = cells.size();
                        for (int c = 0; c < cellSize; c++) {
                            FCGridCell cell = cells.get(c);
                            FCGridColumn column = cell.getColumn();
                            if (column.isVisible()) {
                                if (j == 0 && !column.isFrozen()) {
                                    unFrozenCells.add(cell);
                                    continue;
                                }

                                FCRect headerRect = column.getHeaderRect();
                                if (!column.isFrozen()) {
                                    headerRect.left += scrollH;
                                    headerRect.right += scrollH;
                                }
                                int cellWidth = column.getWidth();
                                int colSpan = cell.getColSpan();
                                if (colSpan > 1) {
                                    for (int n = 1; n < colSpan; n++) {
                                        FCGridColumn spanColumn = getColumn(column.getIndex() + n);
                                        if (spanColumn != null && spanColumn.isVisible()) {
                                            cellWidth += spanColumn.getWidth();
                                        }
                                    }
                                }
                                int cellHeight = row.getHeight();
                                int rowSpan = cell.getRowSpan();
                                if (rowSpan > 1) {
                                    for (int n = 1; n < rowSpan; n++) {
                                        FCGridRow spanRow = getRow(i + n);
                                        if (spanRow != null && spanRow.isVisible()) {
                                            cellHeight += spanRow.getHeight();
                                        }
                                    }
                                }
                                FCRect cellRect = new FCRect(headerRect.left, rowRect.top + m_verticalOffset, headerRect.left + cellWidth, rowRect.top + m_verticalOffset + cellHeight);
                                if (mp.x >= cellRect.left && mp.x <= cellRect.right && mp.y >= cellRect.top && mp.y <= cellRect.bottom) {
                                    if (state == 0) {
                                        boolean hoverChanged = false;
                                        if (m_allowHoveredRow && m_hoveredRow != row) {
                                            m_hoveredRow = row;
                                            hoverChanged = true;
                                        }
                                        if (getNative().getPushedControl() == this) {
                                            if (m_allowDragRow) {
                                                if (m_selectionMode == FCGridSelectionMode.SelectFullRow) {
                                                    int selectedRowsSize = m_selectedRows.size();
                                                    if (selectedRowsSize == 1) {
                                                        if (m_selectedRows.get(0) != row) {
                                                            moveRow(m_selectedRows.get(0).getIndex(), row.getIndex());
                                                            hoverChanged = true;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (m_hoveredCell != cell) {
                                            if (m_hoveredCell != null) {
                                                onCellTouchLeave(m_hoveredCell, touchInfo.clone());
                                            }
                                            m_hoveredCell = cell;
                                            onCellTouchEnter(m_hoveredCell, touchInfo.clone());
                                        }
                                        if (m_editingRow == null) {
                                            if (row.allowEdit()) {
                                                if (getNative().getPushedControl() == this) {
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
                                        onCellTouchMove(cell, touchInfo.clone());
                                        if (hoverChanged) {
                                            invalidate();
                                        }
                                    } else {
                                        if (state == 1) {
                                            onCellTouchDown(cell, touchInfo.clone());
                                            m_touchDownPoint = mp;
                                            if (touchInfo.m_firstTouch && touchInfo.m_clicks == 1) {
                                                int multiSelectMode = 0;
                                                if (m_selectionMode == FCGridSelectionMode.SelectCell) {
                                                    boolean contains = false;
                                                    boolean selectedChanged = false;
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
                                                    } else if (multiSelectMode == 2) {
                                                        if (!contains) {
                                                            m_selectedCells.add(cell);
                                                            selectedChanged = true;
                                                        }
                                                    }
                                                    if (selectedChanged) {
                                                        onSelectedCellsChanged();
                                                    }
                                                } else if (m_selectionMode == FCGridSelectionMode.SelectFullColumn) {
                                                    boolean contains = false;
                                                    boolean selectedChanged = false;
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
                                                    } else if (multiSelectMode == 2) {
                                                        if (!contains) {
                                                            m_selectedColumns.add(column);
                                                            selectedChanged = true;
                                                        }
                                                    }
                                                    m_selectedCells.clear();
                                                    m_selectedCells.add(cell);
                                                    if (selectedChanged) {
                                                        onSelectedColumnsChanged();
                                                    }
                                                } else if (m_selectionMode == FCGridSelectionMode.SelectFullRow) {
                                                    boolean contains = false;
                                                    boolean selectedChanged = false;
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
                                                    } else if (multiSelectMode == 1) {
                                                        selectedRowsSize = m_selectedRows.size();
                                                        if (selectedRowsSize > 0) {
                                                            int firstIndex = m_selectedRows.get(0).getIndex();
                                                            int newIndex = row.getIndex();
                                                            int minIndex = Math.min(firstIndex, newIndex);
                                                            int maxIndex = Math.max(firstIndex, newIndex);
                                                            m_selectedRows.clear();
                                                            for (int s = minIndex; s <= maxIndex; s++) {
                                                                m_selectedRows.add(getRow(s));
                                                            }
                                                        } else {
                                                            m_selectedRows.add(row);
                                                        }
                                                    } else if (multiSelectMode == 2) {
                                                        if (!contains) {
                                                            m_selectedRows.add(row);
                                                            selectedChanged = true;
                                                        }
                                                    }

                                                    m_selectedCells.clear();
                                                    m_selectedCells.add(cell);
                                                    if (selectedChanged) {
                                                        onSelectedRowsChanged();
                                                    }
                                                }
                                            }
                                        } else if (state == 2) {
                                            onCellTouchUp(cell, touchInfo.clone());
                                        }
                                        if (state == 2 || (touchInfo.m_clicks == 2 && state == 1)) {
                                            if (m_selectedCells.size() > 0 && m_selectedCells.get(0) == cell) {
                                                onCellClick(cell, touchInfo.clone());
                                                if (touchInfo.m_firstTouch && cell.allowEdit()) {
                                                    if ((m_cellEditMode == FCGridCellEditMode.DoubleClick && (touchInfo.m_clicks == 2 && state == 1)) || (m_cellEditMode == FCGridCellEditMode.SingleClick && state == 2)) {
                                                        onCellEditBegin(cell);
                                                    }
                                                }
                                            }
                                        }
                                        invalidate();
                                    }
                                    unFrozenCells.clear();
                                    if (state == 1 && m_editingRow != null) {
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
            onRowEditEnd();
        }
    }

    /**
     * 移动行
     *
     * @param oldIndex 旧行
     * @param newIndex 新行
     */
    public void moveRow(int oldIndex, int newIndex) {
        int rowsSize = m_rows.size();
        if (rowsSize > 0) {
            if (oldIndex >= 0 && oldIndex < rowsSize && newIndex >= 0 && newIndex < rowsSize) {
                FCGridRow movingRow = m_rows.get(oldIndex);
                FCGridRow targetRow = m_rows.get(newIndex);
                if (movingRow != targetRow) {
                    // 交换行
                    m_rows.set(newIndex, movingRow);
                    m_rows.set(oldIndex, targetRow);
                    movingRow.setIndex(newIndex);
                    targetRow.setIndex(oldIndex);
                    FCVScrollBar vScrollBar = getVScrollBar();
                    if (vScrollBar != null && vScrollBar.isVisible()) {
                        int firstVisibleRowIndex = -1, lastVisibleRowIndex = -1;
                        RefObject<Integer> tempRef_firstVisibleRowIndex = new RefObject<Integer>(firstVisibleRowIndex);
                        RefObject<Integer> tempRef_lastVisibleRowIndex = new RefObject<Integer>(lastVisibleRowIndex);
                        getVisibleRowsIndex(0.6, tempRef_firstVisibleRowIndex, tempRef_lastVisibleRowIndex);
                        firstVisibleRowIndex = tempRef_firstVisibleRowIndex.argvalue;
                        lastVisibleRowIndex = tempRef_lastVisibleRowIndex.argvalue;
                        int th = targetRow.getHeight();
                        if (newIndex <= firstVisibleRowIndex) {
                            if (newIndex == firstVisibleRowIndex) {
                                vScrollBar.setPos(vScrollBar.getPos() - th);
                            }
                            int count = 0;
                            while (!isRowVisible(targetRow, 0.6)) {
                                int newPos = vScrollBar.getPos() - th;
                                vScrollBar.setPos(newPos);
                                count++;
                                if (count > rowsSize || newPos <= vScrollBar.getPos()) {
                                    break;
                                }
                            }
                        } else if (newIndex >= lastVisibleRowIndex) {
                            if (newIndex == lastVisibleRowIndex) {
                                vScrollBar.setPos(vScrollBar.getPos() + th);
                            }
                            int count = 0;
                            while (!isRowVisible(targetRow, 0.6)) {
                                int newPos = vScrollBar.getPos() + th;
                                vScrollBar.setPos(newPos);
                                count++;
                                if (count > rowsSize || newPos >= vScrollBar.getPos()) {
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

    /**
     * 单元格触摸点击方法
     *
     * @param cell 单元格
     * @param touchInfo 触摸信息
     */
    public void onCellClick(FCGridCell cell, FCTouchInfo touchInfo) {
        callCellTouchEvents(FCEventID.GRIDCELLCLICK, cell, touchInfo.clone());
    }

    /**
     * 单元格编辑开始
     *
     * @param cell 单元格
     */
    public void onCellEditBegin(FCGridCell cell) {
        m_editingCell = cell;
        //创建编辑文本框
        if (m_editTextBox == null) {
            FCHost host = getNative().getHost();
            FCView tempVar = host.createInternalControl(this, "edittextbox");
            m_editTextBox = (FCTextBox) ((tempVar instanceof FCTextBox) ? tempVar : null);
            m_editTextBox.addEvent(this, FCEventID.LOSTFOCUS);
            addControl(m_editTextBox);
        }
        m_editTextBox.setFocused(true);
        String text = m_editingCell.getText();
        m_editTextBox.setTag(m_editingCell);
        m_editTextBox.setText(text);
        m_editTextBox.clearRedoUndo();
        m_editTextBox.setVisible(true);
        if (text != null && text.length() > 0) {
            m_editTextBox.setSelectionStart(text.length());
        }
        callCellEvents(FCEventID.GRIDCELLEDITBEGIN, cell);
    }

    /**
     * 单元格编辑结束
     *
     * @param cell 单元格
     */
    public void onCellEditEnd(FCGridCell cell) {
        if (cell != null) {
            cell.setText(m_editTextBox.getText());
        }
        m_editTextBox.setTag(null);
        m_editTextBox.setVisible(false);
        m_editingCell = null;
        callCellEvents(FCEventID.GRIDCELLEDITEND, cell);
        invalidate();
    }

    /**
     * 单元格触摸按下方法
     *
     * @param cell 单元格
     * @param touchInfo 触摸信息
     */
    public void onCellTouchDown(FCGridCell cell, FCTouchInfo touchInfo) {
        callCellTouchEvents(FCEventID.GRIDCELLTOUCHDOWN, cell, touchInfo.clone());
    }

    /**
     * 单元格触摸进入方法
     *
     * @param cell 单元格
     * @param touchInfo 触摸信息
     */
    public void onCellTouchEnter(FCGridCell cell, FCTouchInfo touchInfo) {
        callCellTouchEvents(FCEventID.GRIDCELLTOUCHENTER, cell, touchInfo.clone());
        if (autoEllipsis() || (cell.getStyle() != null && cell.getStyle().autoEllipsis())) {
            m_native.getHost().showToolTip(cell.getPaintText(), m_native.getTouchPoint());
        }
    }

    /**
     * 单元格触摸移出方法
     *
     * @param cell 单元格
     * @param touchInfo 触摸信息
     */
    public void onCellTouchLeave(FCGridCell cell, FCTouchInfo touchInfo) {
        callCellTouchEvents(FCEventID.GRIDCELLTOUCHLEAVE, cell, touchInfo.clone());
    }

    /**
     * 单元格触摸移出方法
     *
     * @param cell 单元格
     * @param touchInfo 触摸信息
     */
    public void onCellTouchMove(FCGridCell cell, FCTouchInfo touchInfo) {
        callCellTouchEvents(FCEventID.GRIDCELLTOUCHMOVE, cell, touchInfo.clone());
    }

    /**
     * 单元格触摸抬起方法
     *
     * @param cell 单元格
     * @param touchInfo 触摸信息
     */
    public void onCellTouchUp(FCGridCell cell, FCTouchInfo touchInfo) {
        callCellTouchEvents(FCEventID.GRIDCELLTOUCHUP, cell, touchInfo.clone());
    }

    /**
     * 控件添加方法
     */
    @Override
    public void onLoad() {
        super.onAdd();
        if (m_useAnimation) {
            startTimer(m_timerID, 20);
        } else {
            stopTimer(m_timerID);
        }
    }

    /**
     * 丢失焦点方法
     */
    @Override
    public void onLostfocus() {
        super.onLostfocus();
        m_hoveredCell = null;
        m_hoveredRow = null;
    }

    /**
     * 触摸按下方法
     *
     * @param touchInfo 触摸信息
     */
    @Override
    public void onTouchDown(FCTouchInfo touchInfo) {
        super.onTouchDown(touchInfo.clone());
        touchEvent(touchInfo.clone(), 1);
    }

    /**
     * 触摸离开方法
     *
     * @param touchInfo 触摸信息
     */
    @Override
    public void onTouchLeave(FCTouchInfo touchInfo) {
        super.onTouchLeave(touchInfo.clone());
        if (m_hoveredCell != null) {
            onCellTouchLeave(m_hoveredCell, touchInfo.clone());
            m_hoveredCell = null;
        }
        m_hoveredRow = null;
        invalidate();
    }

    /**
     * 触摸移动方法
     *
     * @param touchInfo 触摸信息
     */
    @Override
    public void onTouchMove(FCTouchInfo touchInfo) {
        super.onTouchMove(touchInfo.clone());
        touchEvent(touchInfo.clone(), 0);
    }

    /**
     * 触摸抬起方法
     *
     * @param touchInfo 触摸信息
     */
    @Override
    public void onTouchUp(FCTouchInfo touchInfo) {
        super.onTouchUp(touchInfo.clone());
        touchEvent(touchInfo.clone(), 2);
    }

    /**
     * 重绘前景方法
     *
     * @param paint 绘图对象
     * @param clipRect 裁剪区域
     */
    @Override
    public void onPaintForeground(FCPaint paint, FCRect clipRect) {
        resetHeaderLayout();
        int width = getWidth(), height = getHeight();
        if (width > 0 && height > 0) {
            FCNative inative = getNative();
            FCRect drawRect = new FCRect(0, 0, width, height);
            int allVisibleColumnsWidth = getAllVisibleColumnsWidth();
            /**
             * 行的可见宽度
             */
            int rowVisibleWidth = 0;
            if (allVisibleColumnsWidth > 0) {
                rowVisibleWidth = allVisibleColumnsWidth > width ? width : allVisibleColumnsWidth;
            }

            /**
             * 绘制标题头
             */
            int hHeight = m_headerVisible ? m_headerHeight : 0;
            int scrollH = 0, scrollV = 0;
            FCHScrollBar hScrollBar = getHScrollBar();
            FCVScrollBar vScrollBar = getVScrollBar();
            if (hScrollBar != null && hScrollBar.isVisible()) {
                scrollH = -hScrollBar.getPos();
            }
            if (vScrollBar != null && vScrollBar.isVisible()) {
                scrollV = -vScrollBar.getPos();
            }
            onSetEmptyClipRegion();
            /**
             * 获取显示的第一行和最后一行
             */
            FCPoint fPoint = new FCPoint(0, hHeight + 1 - scrollV);
            FCPoint ePoint = new FCPoint(0, height - 10 - scrollV);
            FCGridRow fRow = getRow(fPoint);
            FCGridRow eRow = getRow(ePoint);
            while (eRow == null && ePoint.y > 0) {
                ePoint.y -= 10;
                eRow = getRow(ePoint);
            }
            if (fRow != null && eRow != null) {
                int fIndex = fRow.getIndex();
                int eIndex = eRow.getIndex();
                for (int i = fIndex; i <= eIndex; i++) {
                    FCGridRow row = m_rows.get(i);
                    if (row.isVisible()) {
                        // 画选中行
                        FCRect rowRect = row.getBounds();
                        rowRect.top += scrollV;
                        rowRect.bottom += scrollV;
                        row.onPaint(paint, rowRect.clone(), row.getVisibleIndex() % 2 == 1);
                        FCRect tempRect = new FCRect();
                        ArrayList<FCGridCell> cells = null;
                        ArrayList<FCGridCell> frozenCells = new ArrayList<FCGridCell>();
                        for (int j = 0; j < 2; j++) {
                            if (j == 0) {
                                cells = row.getCells();
                            } else {
                                cells = frozenCells;
                            }
                            int frozenRight = 0;
                            int cellSize = cells.size();
                            for (int c = 0; c < cellSize; c++) {
                                FCGridCell cell = cells.get(c);
                                FCGridColumn column = cell.getColumn();
                                if (column.isVisible()) {
                                    FCRect headerRect = column.getHeaderRect();
                                    if (j == 0 && column.isFrozen()) {
                                        frozenRight = headerRect.right;
                                        frozenCells.add(cell);
                                        continue;
                                    }
                                    if (!column.isFrozen()) {
                                        headerRect.left += scrollH;
                                        headerRect.right += scrollH;
                                    }
                                    int cellWidth = column.getWidth();
                                    int colSpan = cell.getColSpan();
                                    if (colSpan > 1) {
                                        for (int n = 1; n < colSpan; n++) {
                                            FCGridColumn spanColumn = getColumn(column.getIndex() + n);
                                            if (spanColumn != null && spanColumn.isVisible()) {
                                                cellWidth += spanColumn.getWidth();
                                            }
                                        }
                                    }
                                    int cellHeight = row.getHeight();
                                    int rowSpan = cell.getRowSpan();
                                    if (rowSpan > 1) {
                                        for (int n = 1; n < rowSpan; n++) {
                                            FCGridRow spanRow = getRow(i + n);
                                            if (spanRow != null && spanRow.isVisible()) {
                                                cellHeight += spanRow.getHeight();
                                            }
                                        }
                                    }
                                    FCRect cellRect = new FCRect(headerRect.left, rowRect.top + m_verticalOffset, headerRect.left + cellWidth, rowRect.top + m_verticalOffset + cellHeight);
                                    cellRect.left += row.getHorizontalOffset();
                                    cellRect.right += row.getHorizontalOffset();
                                    RefObject<FCRect> tempRef_tempRect = new RefObject<FCRect>(tempRect);
                                    RefObject<FCRect> tempRef_cellRect = new RefObject<FCRect>(cellRect);
                                    RefObject<FCRect> tempRef_drawRect = new RefObject<FCRect>(drawRect);
                                    boolean tempVar = inative.getHost().getIntersectRect(tempRef_tempRect, tempRef_cellRect, tempRef_drawRect) > 0;
                                    if (tempVar) {
                                        if (cell != null) {
                                            FCRect cellClipRect = cellRect.clone();
                                            if (!column.isFrozen()) {
                                                if (cellClipRect.left < frozenRight) {
                                                    cellClipRect.left = frozenRight;
                                                }
                                                if (cellClipRect.right < frozenRight) {
                                                    cellClipRect.right = frozenRight;
                                                }
                                            }
                                            cell.onPaint(paint, cellRect, cellClipRect, row.getVisibleIndex() % 2 == 1);
                                            if (m_editingCell != null && m_editingCell == cell && m_editTextBox != null) {
                                                FCRect editClipRect = new FCRect(cellClipRect.left - cellRect.left, cellClipRect.top - cellRect.top, cellClipRect.right - cellRect.left, cellClipRect.bottom - cellRect.top);
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
                        row.onPaintBorder(paint, rowRect.clone(), row.getVisibleIndex() % 2 == 1);
                    }
                }
            }
        }
    }

    /**
     * 绘制编辑文本框
     *
     * @param cell 单元格
     * @param paint 绘图对象
     * @param rect 区域
     * @param clipRect 裁剪区域
     */
    public void onPaintEditTextBox(FCGridCell cell, FCPaint paint, FCRect rect, FCRect clipRect) {
        m_editTextBox.setRegion(clipRect);
        m_editTextBox.setBounds(rect);
        m_editTextBox.setDisplayOffset(false);
        m_editTextBox.bringToFront();
    }

    /**
     * 行编辑开始
     *
     * @param row 行
     */
    public void onRowEditBegin(FCGridRow row) {
        FCView editButton = row.getEditButton();
        if (editButton != null && !containsControl(editButton)) {
            FCPoint mp = getTouchPoint();
            if (mp.x - m_touchDownPoint.x < -10) {
                m_editingRow = row;
                addControl(editButton);
                if (m_useAnimation) {
                    editButton.setLocation(new FCPoint(-10000, -10000));
                    m_editingRow.m_editState = 1;
                } else {
                    m_editingRow.setHorizontalOffset(-editButton.getWidth() - ((m_vScrollBar != null && m_vScrollBar.isVisible()) ? m_vScrollBar.getWidth() : 0));
                }
            }
        }
    }

    /**
     * 行编辑结束
     */
    public void onRowEditEnd() {
        if (m_useAnimation) {
            m_editingRow.m_editState = 2;
        } else {
            m_editingRow.setHorizontalOffset(0);
            removeControl(m_editingRow.getEditButton());
            m_editingRow = null;
        }
    }

    /**
     * 选中单元格改变方法
     */
    public void onSelectedCellsChanged() {
        callEvents(FCEventID.GRIDSELECTEDCELLSCHANGED);
    }

    /**
     * 选中列改变方法
     */
    public void onSelectedColumnsChanged() {
        callEvents(FCEventID.GRIDSELECTEDCOLUMNSSCHANGED);
    }

    /**
     * 选中行改变方法
     */
    public void onSelectedRowsChanged() {
        callEvents(FCEventID.GRIDSELECTEDROWSCHANGED);
    }

    /**
     * 设置控件不可见方法
     */
    public void onSetEmptyClipRegion() {
        // 隐藏控件
        ArrayList<FCView> controls = getControls();
        int controlsSize = controls.size();
        FCRect emptyClipRect = new FCRect();
        for (int i = 0; i < controlsSize; i++) {
            FCView control = controls.get(i);
            if (m_editingRow != null && control == m_editingRow.getEditButton()) {
                continue;
            }
            FCScrollBar scrollBar = (FCScrollBar) ((control instanceof FCScrollBar) ? control : null);
            FCGridColumn gridColumn = (FCGridColumn) ((control instanceof FCGridColumn) ? control : null);
            if (control != m_editTextBox && scrollBar == null && gridColumn == null) {
                control.setRegion(emptyClipRect);
            }
        }
    }

    /**
     * 秒表方法
     *
     * @param timerID 秒表ID
     */
    @Override
    public void onTimer(int timerID) {
        super.onTimer(timerID);
        if (m_timerID == timerID) {
            if (m_useAnimation) {
                boolean paint = false;
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
                    int width = getAllVisibleColumnsWidth();
                    int step = width / 10;
                    if (step < 10) {
                        step = 10;
                    }
                    for (int i = 0; i < animateAddRowsSize; i++) {
                        FCGridRow row = m_animateAddRows.get(i);
                        int horizontalOffset = row.getHorizontalOffset();
                        if (horizontalOffset > step) {
                            horizontalOffset -= step;
                        } else {
                            horizontalOffset = 0;
                        }
                        row.setHorizontalOffset(horizontalOffset);
                        if (horizontalOffset == 0) {
                            m_animateAddRows.remove(i);
                            animateAddRowsSize--;
                            i--;
                        }
                    }
                    paint = true;
                }
                int animateRemoveRowsSize = m_animateRemoveRows.size();
                if (animateRemoveRowsSize > 0) {
                    int width = getAllVisibleColumnsWidth();
                    int step = width / 10;
                    if (step < 10) {
                        step = 10;
                    }
                    for (int i = 0; i < animateRemoveRowsSize; i++) {
                        FCGridRow row = m_animateRemoveRows.get(i);
                        int horizontalOffset = row.getHorizontalOffset();
                        if (horizontalOffset <= width) {
                            horizontalOffset += step;
                        }
                        row.setHorizontalOffset(horizontalOffset);
                        if (horizontalOffset > width) {
                            m_animateRemoveRows.remove(i);
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
                    FCHScrollBar hScrollBar = getHScrollBar();
                    FCVScrollBar vScrollBar = getVScrollBar();
                    int vScrollBarW = 0;
                    if (hScrollBar != null && hScrollBar.isVisible()) {
                        scrollH = -hScrollBar.getPos();
                    }
                    if (vScrollBar != null && vScrollBar.isVisible()) {
                        scrollV = -vScrollBar.getPos();
                        vScrollBarW = vScrollBar.getWidth();
                    }
                    if (m_editingRow.m_editState == 1) {
                        FCView editButton = m_editingRow.getEditButton();
                        boolean isOver = false;
                        int sub = editButton.getWidth() + m_editingRow.getHorizontalOffset() + vScrollBarW;
                        if (sub < 2) {
                            isOver = true;
                            m_editingRow.setHorizontalOffset(-editButton.getWidth() - vScrollBarW);
                        } else {
                            m_editingRow.setHorizontalOffset(m_editingRow.getHorizontalOffset() - 10);
                        }
                        editButton.setLocation(new FCPoint((getAllVisibleColumnsWidth()) + scrollH + m_editingRow.getHorizontalOffset(),
                                m_editingRow.getBounds().top + scrollV));
                        if (isOver) {
                            m_editingRow.m_editState = 0;
                        }
                    }
                    if (m_editingRow.m_editState == 2) {
                        FCView editButton = m_editingRow.getEditButton();
                        boolean isOver = false;
                        if (m_editingRow.getHorizontalOffset() < 0) {
                            m_editingRow.setHorizontalOffset(m_editingRow.getHorizontalOffset() + 10);
                            if (m_editingRow.getHorizontalOffset() >= 0) {
                                m_editingRow.setHorizontalOffset(0);
                                isOver = true;
                            }
                        }
                        editButton.setLocation(new FCPoint(getAllVisibleColumnsWidth() + scrollH + m_editingRow.getHorizontalOffset(),
                                m_editingRow.getBounds().top + scrollV));
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

    /**
     * 可见状态改变方法
     */
    @Override
    public void onVisibleChanged() {
        super.onVisibleChanged();
        m_hoveredCell = null;
        m_hoveredRow = null;
    }

    /**
     * 移除列
     *
     * @param column 列
     */
    public void removeColumn(FCGridColumn column) {
        boolean selectedChanged = false;
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
            m_columns.get(i).setIndex(i);
        }
        removeControl(column);
        int rowSize = m_rows.size();
        for (int i = 0; i < rowSize; i++) {
            FCGridRow row = m_rows.get(i);
            row.removeCell(column.getIndex());
        }
        if (selectedChanged) {
            onSelectedColumnsChanged();
        }
    }

    /**
     * 移除行
     *
     * @param row 行
     */
    public void removeRow(FCGridRow row) {
        if (m_editingRow != null) {
            if (containsControl(m_editingRow.getEditButton())) {
                removeControl(m_editingRow.getEditButton());
            }
            m_editingRow.m_editState = 0;
            m_editingRow = null;
        }
        boolean selectedChanged = false;
        boolean selected = false;
        int selectedRowsSize = m_selectedRows.size();
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
            } else {
                if (otherRow != null) {
                    selectedChanged = true;
                }
            }
        }
        if (m_hoveredRow == row) {
            m_hoveredCell = null;
            m_hoveredRow = null;
        }
        row.onremove();
        m_rows.remove(row);
        int rowSize = m_rows.size();
        if (rowSize == 0) {
            m_selectedCells.clear();
            m_selectedRows.clear();
        }
        int visibleIndex = 0;
        for (int i = 0; i < rowSize; i++) {
            FCGridRow gridRow = m_rows.get(i);
            gridRow.setIndex(i);
            if (gridRow.isVisible()) {
                gridRow.setVisibleIndex(visibleIndex);
                visibleIndex++;
            }
        }
        if (selected) {
            if (selectedChanged) {
                onSelectedRowsChanged();
            } else {
                m_selectedCells.clear();
                m_selectedRows.clear();
            }
        }
    }

    /**
     * 调整列的布局
     */
    public void resetHeaderLayout() {
        if (!m_lockUpdate) {
            int left = 0, top = 0;
            int scrollH = 0, scrollV = 0;
            FCHScrollBar hScrollBar = getHScrollBar();
            FCVScrollBar vScrollBar = getVScrollBar();
            int vScrollBarW = 0;
            if (hScrollBar != null && hScrollBar.isVisible()) {
                scrollH = -hScrollBar.getPos();
            }
            if (vScrollBar != null && vScrollBar.isVisible()) {
                scrollV = -vScrollBar.getPos();
                vScrollBarW = vScrollBar.getWidth();
            }
            int headerHeight = m_headerVisible ? m_headerHeight : 0;
            // 计算列的区域
            FCGridColumn draggingColumn = null;
            int columnSize = m_columns.size();
            for (int i = 0; i < columnSize; i++) {
                FCGridColumn column = m_columns.get(i);
                if (column.isVisible()) {
                    FCRect cellRect = new FCRect(left + m_horizontalOffset, top + m_verticalOffset, left + m_horizontalOffset + column.getWidth(), top + headerHeight + m_verticalOffset);
                    column.setHeaderRect(cellRect);
                    if (column.isDragging()) {
                        draggingColumn = column;
                        column.setBounds(new FCRect(column.getLeft(), cellRect.top, column.getRight(), cellRect.bottom));
                    } else {
                        if (!column.isFrozen()) {
                            cellRect.left += scrollH;
                            cellRect.right += scrollH;
                        }
                        column.setBounds(cellRect);
                    }
                    left += column.getWidth();
                }
            }
            // 将控件置于最前
            for (int i = columnSize - 1; i >= 0; i--) {
                m_columns.get(i).bringToFront();
            }
            if (draggingColumn != null) {
                draggingColumn.bringToFront();
            }
            if (m_editingRow != null && m_editingRow.m_editState == 0 && m_editingRow.getEditButton() != null) {
                FCView editButton = m_editingRow.getEditButton();
                editButton.setLocation(new FCPoint(getAllVisibleColumnsWidth() - editButton.getWidth() + scrollH - vScrollBarW, m_editingRow.getBounds().top + scrollV));
            }
        }
    }

    /**
     * 选中上一行
     */
    public FCGridRow selectFrontRow() {
        int rowsSize = m_rows.size();
        if (rowsSize == 0) {
            m_selectedRows.clear();
            m_selectedCells.clear();
            return null;
        }
        FCGridRow frontRow = null;
        ArrayList<FCGridRow> selectedRows = getSelectedRows();
        if (selectedRows.size() == 1) {
            // 获取向上的行
            FCGridRow selectedRow = selectedRows.get(0);
            int selectedIndex = selectedRow.getIndex();
            for (int i = selectedIndex - 1; i >= 0; i--) {
                if (i < rowsSize && m_rows.get(i).isVisible()) {
                    frontRow = m_rows.get(i);
                    break;
                }
            }
            // 向上
            if (m_selectionMode == FCGridSelectionMode.SelectFullRow) {
                if (frontRow != null) {
                    m_selectedRows.clear();
                    m_selectedRows.add(frontRow);
                    onSelectedRowsChanged();
                } else {
                    m_selectedRows.clear();
                    frontRow = m_rows.get(m_rows.size() - 1);
                    m_selectedRows.add(frontRow);
                    FCVScrollBar vScrollBar = getVScrollBar();
                    if (vScrollBar != null && vScrollBar.isVisible()) {
                        vScrollBar.scrollToEnd();
                    }
                    onSelectedRowsChanged();
                }
            }
        }
        return frontRow;
    }

    /**
     * 选中下一行
     */
    public FCGridRow selectNextRow() {
        int rowsSize = m_rows.size();
        if (rowsSize == 0) {
            m_selectedRows.clear();
            m_selectedCells.clear();
            return null;
        }
        FCGridRow nextRow = null;
        ArrayList<FCGridRow> selectedRows = getSelectedRows();
        if (selectedRows.size() == 1) {
            FCGridRow selectedRow = selectedRows.get(0);
            int selectedIndex = selectedRow.getIndex();
            for (int i = selectedIndex + 1; i < rowsSize; i++) {
                if (i >= 0 && m_rows.get(i).isVisible()) {
                    nextRow = m_rows.get(i);
                    break;
                }
            }
            // 向下
            if (m_selectionMode == FCGridSelectionMode.SelectFullRow) {
                if (nextRow != null) {
                    m_selectedRows.clear();
                    m_selectedRows.add(nextRow);
                    onSelectedRowsChanged();
                } else {
                    m_selectedRows.clear();
                    nextRow = m_rows.get(0);
                    m_selectedRows.add(nextRow);
                    FCVScrollBar vScrollBar = getVScrollBar();
                    if (vScrollBar != null && vScrollBar.isVisible()) {
                        vScrollBar.scrollToBegin();
                    }
                    onSelectedRowsChanged();
                }
            }
        }
        return nextRow;
    }

    /**
     * 设置属性
     *
     * @param name 属性名称
     * @param value 属性值
     */
    @Override
    public void setProperty(String name, String value) {
        if (name.equals("allowdragrow")) {
            setAllowDragRow(FCStr.convertStrToBool(value));
        } else if (name.equals("allowhoveredrow")) {
            setAllowHoveredRow(FCStr.convertStrToBool(value));
        } else if (name.equals("celleditmode")) {
            value = value.toLowerCase();
            if (value.equals("doubleclick")) {
                setCellEditMode(FCGridCellEditMode.DoubleClick);
            } else if (value.equals("none")) {
                setCellEditMode(FCGridCellEditMode.None);
            } else if (value.equals("singleclick")) {
                setCellEditMode(FCGridCellEditMode.SingleClick);
            }
        } else if (name.equals("gridlinecolor")) {
            setGridLineColor(FCStr.convertStrToColor(value));
        } else if (name.equals("headerheight")) {
            setHeaderHeight(FCStr.convertStrToInt(value));
        } else if (name.equals("headervisible")) {
            setHeaderVisible(FCStr.convertStrToBool(value));
        } else if (name.equals("horizontaloffset")) {
            setHorizontalOffset(FCStr.convertStrToInt(value));
        } else if (name.equals("multiselect")) {
            setMultiSelect(FCStr.convertStrToBool(value));
        } else if (name.equals("selectionmode")) {
            value = value.toLowerCase();
            if (value.equals("selectcell")) {
                setSelectionMode(FCGridSelectionMode.SelectCell);
            } else if (value.equals("selectfullcolumn")) {
                setSelectionMode(FCGridSelectionMode.SelectFullColumn);
            } else if (value.equals("selectfullrow")) {
                setSelectionMode(FCGridSelectionMode.SelectFullRow);
            } else {
                setSelectionMode(FCGridSelectionMode.SelectNone);
            }
        } else if (name.equals("useanimation")) {
            setuseAnimation(FCStr.convertStrToBool(value));
        } else if (name.equals("verticaloffset")) {
            setVerticalOffset(FCStr.convertStrToInt(value));
        } else {
            super.setProperty(name, value);
        }
    }

    /**
     * 排序
     *
     * @param grid 表格
     * @param column 列
     * @param sortMode 排序方式
     */
    public void sortColumn(FCGrid grid, FCGridColumn column, FCGridColumnSortMode sortMode) {
        if (column.allowSort()) {
            // 取消原有排序
            int colSize = grid.m_columns.size();
            for (int i = 0; i < colSize; i++) {
                if (grid.m_columns.get(i) != column) {
                    grid.m_columns.get(i).setSortMode(FCGridColumnSortMode.None);
                } else {
                    grid.m_columns.get(i).setSortMode(sortMode);
                }
            }
            // 重新排序，要改成二叉树排序
            if (m_sort != null) {
                m_sort.sortColumn(grid, column, sortMode);
            }
            grid.update();
            grid.invalidate();
        }
    }

    /**
     * 重新布局
     */
    @Override
    public void update() {
        if (getNative() != null) {
            if (!m_lockUpdate) {
                super.update();
                if (isVisible()) {
                    int colSize = m_columns.size();
                    for (int i = 0; i < colSize; i++) {
                        m_columns.get(i).setIndex(i);
                    }
                    int rowSize = m_rows.size();
                    int visibleIndex = 0;
                    int rowTop = m_headerVisible ? m_headerHeight : 0;
                    int allVisibleColumnsWidth = getAllVisibleColumnsWidth();
                    m_hasUnVisibleRow = false;
                    for (int i = 0; i < rowSize; i++) {
                        FCGridRow gridRow = m_rows.get(i);
                        gridRow.setIndex(i);
                        if (gridRow.isVisible()) {
                            gridRow.setVisibleIndex(i);
                            int rowHeight = gridRow.getHeight();
                            FCRect rowRect = new FCRect(0, rowTop, allVisibleColumnsWidth, rowTop + rowHeight);
                            gridRow.setBounds(rowRect);
                            rowTop += rowHeight;
                            visibleIndex++;
                        } else {
                            gridRow.setVisibleIndex(-1);
                            FCRect rowRect = new FCRect(0, rowTop, allVisibleColumnsWidth, rowTop);
                            gridRow.setBounds(rowRect);
                        }
                    }
                    FCHScrollBar hScrollBar = getHScrollBar();
                    FCVScrollBar vScrollBar = getVScrollBar();
                    if (vScrollBar != null && vScrollBar.isVisible()) {
                        int top = m_headerVisible ? m_headerHeight : 0;
                        vScrollBar.setTop(top);
                        int height = getHeight() - top - ((hScrollBar != null && hScrollBar.isVisible()) ? hScrollBar.getHeight() : 0);
                        vScrollBar.setHeight(height);
                        vScrollBar.setPageSize(height);
                        if (rowSize > 0) {
                            vScrollBar.setLineSize(getAllVisibleRowsHeight() / rowSize);
                        }
                    }
                }
            }
        }
    }

    /**
     * 更新列排序
     */
    public void updateSortColumn() {
        // 重新排序
        int columnSize = m_columns.size();
        for (int i = 0; i < columnSize; i++) {
            if (m_columns.get(i).getSortMode() != FCGridColumnSortMode.None) {
                sortColumn(this, m_columns.get(i), m_columns.get(i).getSortMode());
                break;
            }
        }
    }
}
