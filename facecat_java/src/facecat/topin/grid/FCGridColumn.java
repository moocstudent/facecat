/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.grid;

import facecat.topin.btn.*;
import facecat.topin.core.*;
import java.util.*;

/**
 * 表格列
 */
public class FCGridColumn extends FCButton {

    /**
     * 创建列
     */
    public FCGridColumn() {
        setWidth(100);
    }

    /**
     * 创建列
     *
     * @param text 标题
     */
    public FCGridColumn(String text) {
        setText(text);
        setWidth(100);
    }

    /**
     * 起始宽度
     */
    protected int m_beginWidth = 0;

    /**
     * 调整大小状态，1:左侧 2:右侧
     */
    protected FCPoint m_touchDownPoint = new FCPoint();

    /**
     * 触摸按下时的坐标
     */
    protected int m_resizeState;

    protected boolean m_allowResize = false;

    /**
     * 获取是否可以调整大小
     */
    public boolean allowResize() {
        return m_allowResize;
    }

    /**
     * 设置是否可以调整大小
     */
    public void setAllowResize(boolean value) {
        m_allowResize = value;
    }

    protected boolean m_allowSort = true;

    /**
     * 获取是否可以排序
     */
    public boolean allowSort() {
        return m_allowSort;
    }

    /**
     * 设置是否可以排序
     */
    public void setAllowSort(boolean value) {
        m_allowSort = value;
    }

    protected FCHorizontalAlign m_cellAlign = FCHorizontalAlign.Left;

    /**
     * 获取内容的横向排列样式
     */
    public FCHorizontalAlign getCellAlign() {
        return m_cellAlign;
    }

    /**
     * 设置内容的横向排列样式
     */
    public void setCellAlign(FCHorizontalAlign value) {
        m_cellAlign = value;
    }

    protected String m_columnType = "";

    /**
     * 获取列的类型
     */
    public String getColumnType() {
        return m_columnType;
    }

    /**
     * 设置列的类型
     */
    public void setColumnType(String value) {
        m_columnType = value;
    }

    protected boolean m_frozen = false;

    /**
     * 获取是否冻结
     */
    public boolean isFrozen() {
        return m_frozen;
    }

    /**
     * 设置是否冻结
     */
    public void setFrozen(boolean value) {
        m_frozen = value;
    }

    protected FCGrid m_grid = null;

    /**
     * 获取表格
     */
    public FCGrid getGrid() {
        return m_grid;
    }

    /**
     * 设置表格
     */
    public void setGrid(FCGrid value) {
        m_grid = value;
    }

    protected FCRect m_headerRect = new FCRect();

    /**
     * 获取头部的矩形
     */
    public FCRect getHeaderRect() {
        return m_headerRect.clone();
    }

    /**
     * 设置头部的矩形
     */
    public void setHeaderRect(FCRect value) {
        m_headerRect = value.clone();
    }

    protected int m_index = -1;

    /**
     * 获取索引
     */
    public int getIndex() {
        return m_index;
    }

    /**
     * 设置索引
     */
    public void setIndex(int value) {
        m_index = value;
    }

    protected FCGridColumnSortMode m_sortMode = FCGridColumnSortMode.None;

    /**
     * 获取排序状态，0:不排序 1:升序 2:降序
     */
    public FCGridColumnSortMode getSortMode() {
        return m_sortMode;
    }

    /**
     * 设置排序状态，0:不排序 1:升序 2:降序
     */
    public void setSortMode(FCGridColumnSortMode value) {
        m_sortMode = value;
    }

    /**
     * 获取控件类型
     */
    @Override
    public String getControlType() {
        return "FCGrid";
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
        if (name.equals("allowresize")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(allowResize());
        } else if (name.equals("allowsort")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(allowSort());
        } else if (name.equals("columntype")) {
            type.argvalue = "text";
            value.argvalue = getColumnType();
        } else if (name.equals("frozen")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(isFrozen());
        } else if (name.equals("cellalign")) {
            type.argvalue = "enum:FCHorizontalAlign";
            value.argvalue = FCStr.convertHorizontalAlignToStr(getCellAlign());
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
        propertyNames.addAll(Arrays.asList(new String[]{"AllowResize", "AllowSort", "CellAlign", "ColumnType", "Frozen"}));
        return propertyNames;
    }

    /**
     * 点击方法
     *
     * @param touchInfo 触摸信息
     */
    @Override
    public void onClick(FCTouchInfo touchInfo) {
        super.onClick(touchInfo.clone());
        if (m_resizeState == 0 && m_allowSort) {
            switch (m_sortMode) {
                case None:
                case Desc:
                    m_grid.sortColumn(m_grid, this, FCGridColumnSortMode.Asc);
                    break;
                case Asc:
                    m_grid.sortColumn(m_grid, this, FCGridColumnSortMode.Desc);
                    break;
            }
            if (getGrid() != null) {
                getGrid().invalidate();
            }
        }
    }

    /**
     * 拖动开始方法
     */
    @Override
    public boolean onDragBegin() {
        return m_resizeState == 0;
    }

    /**
     * 拖动中方法
     */
    @Override
    public void onDragging() {
        super.onDragging();
        if (m_grid != null) {
            ArrayList<FCGridColumn> columns = m_grid.getColumns();
            int columnSize = columns.size();
            for (int i = 0; i < columnSize; i++) {
                FCGridColumn column = columns.get(i);
                if (column == this) {
                    FCGridColumn lastColumn = null;
                    FCGridColumn nextColumn = null;
                    int lastIndex = i - 1;
                    int nextIndex = i + 1;
                    while (lastIndex >= 0) {
                        FCGridColumn thatColumn = columns.get(lastIndex);
                        if (thatColumn.isVisible()) {
                            lastColumn = thatColumn;
                            break;
                        } else {
                            lastIndex--;
                        }
                    }
                    while (nextIndex < columnSize) {
                        FCGridColumn thatColumn = columns.get(nextIndex);
                        if (thatColumn.isVisible()) {
                            nextColumn = thatColumn;
                            break;
                        } else {
                            nextIndex++;
                        }
                    }

                    FCNative inative = getNative();
                    int clientX = inative.clientX(this);
                    if (lastColumn != null) {
                        int lastClientX = inative.clientX(lastColumn);
                        if (clientX < lastClientX + lastColumn.getWidth() / 2) {
                            columns.set(lastIndex, this);
                            columns.set(i, lastColumn);
                            m_grid.update();
                            break;
                        }
                    }
                    if (nextColumn != null) {
                        int nextClientX = inative.clientX(nextColumn);
                        if (clientX + column.getWidth() > nextClientX + nextColumn.getWidth() / 2) {
                            columns.set(nextIndex, this);
                            columns.set(i, nextColumn);
                            m_grid.update();
                            break;
                        }
                    }
                    break;
                }
            }
        }
    }

    /**
     * 触摸按下方法
     *
     * @param touchInfo 触摸信息
     */
    @Override
    public void onTouchDown(FCTouchInfo touchInfo) {
        super.onTouchDown(touchInfo.clone());
        FCPoint mp = touchInfo.m_firstPoint.clone();
        if (touchInfo.m_firstTouch && touchInfo.m_clicks == 1) {
            if (m_allowResize) {
                if (m_index > 0 && mp.x < 5) {
                    m_resizeState = 1;
                    m_beginWidth = getGrid().getColumn(m_index - 1).getWidth();
                } else if (mp.x > getWidth() - 5) {
                    m_resizeState = 2;
                    m_beginWidth = getWidth();
                }
                m_touchDownPoint = getNative().getTouchPoint();
            }
        }
    }

    /**
     * 触摸移动方法
     *
     * @param touchInfo 触摸信息
     */
    @Override
    public void onTouchMove(FCTouchInfo touchInfo) {
        super.onTouchMove(touchInfo.clone());
        if (m_allowResize) {
            if (m_resizeState > 0) {
                FCPoint curPoint = getNative().getTouchPoint();
                int newWidth = m_beginWidth + (curPoint.x - m_touchDownPoint.x);
                if (newWidth > 0) {
                    if (m_resizeState == 1) {
                        getGrid().getColumn(m_index - 1).setWidth(newWidth);
                    } else if (m_resizeState == 2) {
                        setWidth(newWidth);
                    }
                }
                if (m_grid != null) {
                    m_grid.update();
                    m_grid.invalidate();
                }
            }
        }
    }

    /**
     * 触摸抬起方法
     *
     * @param touchInfo 触摸信息
     */
    @Override
    public void onTouchUp(FCTouchInfo touchInfo) {
        super.onTouchUp(touchInfo.clone());
        m_resizeState = 0;
        if (m_grid != null) {
            m_grid.invalidate();
        }
    }

    /**
     * 重绘前景方法
     *
     * @param paint 绘图对象
     * @param clipRect 裁剪区域
     */
    @Override
    public void onPaintForeground(FCPaint paint, FCRect clipRect) {
        super.onPaintForeground(paint, clipRect);
        if (getNative() != null && m_grid != null) {
            FCRect rect = new FCRect(0, 0, getWidth(), getHeight());
            int tLeft = rect.right - 15;
            int midTop = rect.top + (rect.bottom - rect.top) / 2;
            long textColor = getPaintingTextColor();

            if (m_sortMode == FCGridColumnSortMode.Asc) {
                FCPoint[] points = new FCPoint[]{new FCPoint(tLeft + 5, midTop - 5), new FCPoint(tLeft, midTop + 5), new FCPoint(tLeft + 10, midTop + 5)};
                paint.fillPolygon(textColor, points);
            } else if (m_sortMode == FCGridColumnSortMode.Desc) {
                FCPoint[] points = new FCPoint[]{new FCPoint(tLeft + 5, midTop + 5), new FCPoint(tLeft, midTop - 5), new FCPoint(tLeft + 10, midTop - 5)};
                paint.fillPolygon(textColor, points);
            }
        }
    }

    /**
     * 设置属性值
     *
     * @param name 属性名称
     * @param value 属性值
     */
    @Override
    public void setProperty(String name, String value) {
        if (name.equals("allowresize")) {
            setAllowResize(FCStr.convertStrToBool(value));
        } else if (name.equals("allowsort")) {
            setAllowSort(FCStr.convertStrToBool(value));
        } else if (name.equals("cellalign")) {
            setCellAlign(FCStr.convertStrToHorizontalAlign(value));
        } else if (name.equals("columntype")) {
            setColumnType(value);
        } else if (name.equals("frozen")) {
            setFrozen(FCStr.convertStrToBool(value));
        } else {
            super.setProperty(name, value);
        }
    }
}
