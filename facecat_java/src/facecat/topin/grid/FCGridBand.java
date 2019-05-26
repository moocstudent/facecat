/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.grid;

import facecat.topin.scroll.*;
import facecat.topin.btn.*;
import facecat.topin.core.*;
import java.util.*;

/**
 * 表格带
 */
public class FCGridBand extends FCButton {

    /**
     * 创建表格带
     */
    public FCGridBand() {
        setWidth(100);
    }

    /**
     * 子表格带
     */
    public ArrayList<FCGridBand> m_bands = new ArrayList<FCGridBand>();

    /**
     * 子表格列
     */
    public ArrayList<FCBandedGridColumn> m_columns = new ArrayList<FCBandedGridColumn>();

    /**
     * 起始宽度
     */
    protected int m_beginWidth = 0;

    /**
     * 触摸按下时的坐标
     */
    protected FCPoint m_touchDownPoint = new FCPoint();

    protected int m_resizeState;

    protected boolean m_allowResize = false;

    /**
     * 获取是否可以调整大小
     */
    public boolean allowresize() {
        return m_allowResize;
    }

    /**
     * 设置是否可以调整大小
     */
    public void setAllowResize(boolean value) {
        m_allowResize = value;
    }

    protected FCBandedGrid m_grid = null;

    /**
     * 获取置表格
     */
    public FCBandedGrid getGrid() {
        return m_grid;
    }

    /**
     * 设置表格
     */
    public void setGrid(FCBandedGrid value) {
        m_grid = value;
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

    protected FCGridBand m_parentBand = null;

    /**
     * 获取父表格带
     */
    public FCGridBand getParentBand() {
        return m_parentBand;
    }

    /**
     * 设置父表格带
     */
    public void setParentBand(FCGridBand value) {
        m_parentBand = value;
    }

    /**
     * 添加表格带
     *
     * @param band 表格带
     */
    public void addBand(FCGridBand band) {
        band.setGrid(m_grid);
        band.setParentBand(this);
        m_bands.add(band);
        int bandSize = m_bands.size();
        for (int i = 0; i < bandSize; i++) {
            m_bands.get(i).setIndex(i);
        }
        m_grid.addControl(band);
    }

    /**
     * 添加表格列
     *
     * @param column 表格列
     */
    public void addColumn(FCBandedGridColumn column) {
        column.setBand(this);
        m_columns.add(column);
        m_grid.addColumn(column);
    }

    /**
     * 清除表格带
     */
    public void clearBands() {
        int bandsSize = m_bands.size();
        for (int i = 0; i < bandsSize; i++) {
            FCGridBand band = m_bands.get(i);
            m_grid.removeControl(band);
            band.delete();
        }
        m_bands.clear();
    }

    /**
     * 清除列
     */
    public void clearColumns() {
        int columnsSize = m_columns.size();
        for (int i = 0; i < columnsSize; i++) {
            FCBandedGridColumn column = m_columns.get(i);
            m_grid.removeColumn(column);
            column.delete();
        }
        m_columns.clear();
    }

    /**
     * 获取所有的子表格列
     *
     * @returns 列头集合
     */
    public ArrayList<FCBandedGridColumn> getAllChildColumns() {
        ArrayList<FCBandedGridColumn> columns = new ArrayList<FCBandedGridColumn>();
        int columnsSize = m_columns.size();
        for (int i = 0; i < columnsSize; i++) {
            FCBandedGridColumn column = m_columns.get(i);
            columns.add(column);
        }
        int bandsSize = m_bands.size();
        for (int i = 0; i < bandsSize; i++) {
            FCGridBand band = m_bands.get(i);
            ArrayList<FCBandedGridColumn> childColumns = band.getAllChildColumns();
            int childColumnsSize = childColumns.size();
            for (int j = 0; j < childColumnsSize; j++) {
                FCBandedGridColumn childColumn = childColumns.get(j);
                columns.add(childColumn);
            }
        }
        return columns;
    }

    /**
     * 获取表格带列表
     *
     * @returns 表格带集合
     */
    public ArrayList<FCGridBand> getBands() {
        return m_bands;
    }

    /**
     * 获取列头
     *
     * @returns 列头集合
     */
    public ArrayList<FCBandedGridColumn> getColumns() {
        return m_columns;
    }

    /**
     * 获取控件类型
     *
     * @returns 控件类型
     */
    @Override
    public String getControlType() {
        return "FCGridBand";
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
            value.argvalue = FCStr.convertBoolToStr(allowresize());
        }
    }

    /**
     * 获取属性名称列表
     */
    @Override
    public ArrayList<String> getPropertyNames() {
        ArrayList<String> propertyNames = super.getPropertyNames();
        propertyNames.addAll(Arrays.asList(new String[]{"AllowResize"}));
        return propertyNames;
    }

    /**
     * 销毁控件
     */
    @Override
    public void delete() {
        clearBands();
        clearColumns();
        super.delete();
    }

    /**
     * 插入表格带
     *
     * @param index 索引
     * @param band 表格带
     */
    public void insertBand(int index, FCGridBand band) {
        band.setGrid(m_grid);
        band.setParentBand(this);
        m_bands.add(index, band);
        int bandSize = m_bands.size();
        for (int i = 0; i < bandSize; i++) {
            m_bands.get(i).setIndex(i);
        }
        m_grid.addControl(band);
    }

    /**
     * 插入表格列
     *
     * @param index 索引
     * @param column 表格列
     */
    public void insertColumn(int index, FCBandedGridColumn column) {
        column.setBand(this);
        m_columns.add(index, column);
        m_grid.addColumn(column);
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
                ArrayList<FCGridBand> bands = null;
                if (m_parentBand != null) {
                    bands = m_parentBand.getBands();
                } else {
                    bands = m_grid.getBands();
                }
                int bandsSize = bands.size();
                if (m_index > 0 && mp.x < 5) {
                    m_resizeState = 1;
                    m_beginWidth = bands.get(m_index - 1).getWidth();
                } else if ((m_parentBand == null || m_index < bandsSize - 1) && mp.x > getWidth() - 5) {
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
            ArrayList<FCGridBand> bands = null;
            if (m_parentBand != null) {
                bands = m_parentBand.getBands();
            } else {
                bands = m_grid.getBands();
            }
            int bandsSize = bands.size();
            int width = getWidth();
            if (m_resizeState > 0) {
                FCPoint curPoint = getNative().getTouchPoint();
                int newWidth = m_beginWidth + (curPoint.x - m_touchDownPoint.x);
                if (newWidth > 0) {
                    if (m_resizeState == 1) {
                        FCGridBand leftBand = bands.get(m_index - 1);
                        int leftWidth = leftBand.getWidth();
                        leftBand.setWidth(newWidth);
                        width += leftWidth - newWidth;
                        setWidth(width);
                    } else if (m_resizeState == 2) {
                        setWidth(newWidth);
                        if (m_index < bandsSize - 1) {
                            FCGridBand rightBand = bands.get(m_index + 1);
                            int rightWidth = rightBand.getWidth();
                            rightWidth += width - newWidth;
                            rightBand.setWidth(rightWidth);
                        } else {
                            if (m_grid != null) {
                                m_grid.resetHeaderLayout();
                                m_grid.update();
                            }
                        }
                    }
                }
                if (m_grid != null) {
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
        if (m_resizeState != 0) {
            m_resizeState = 0;
            if (m_grid != null) {
                m_grid.invalidate();
            }
        }
    }

    /**
     * 移除表格带
     *
     * @param band 表格带
     */
    public void removeBand(FCGridBand band) {
        if (m_bands.contains(band)) {
            m_bands.remove(band);
            int bandSize = m_bands.size();
            for (int i = 0; i < bandSize; i++) {
                m_bands.get(i).setIndex(i);
            }
            m_grid.removeControl(band);
        }
    }

    /**
     * 移除表格列
     *
     * @param column 表格列
     */
    public void removeColumn(FCBandedGridColumn column) {
        if (m_columns.contains(column)) {
            m_columns.remove(column);
            m_grid.removeColumn(column);
        }
    }

    /**
     * 重置列头布局
     */
    public void resetHeaderLayout() {
        int bandsSize = m_bands.size();
        FCRect bounds = getBounds();
        int left = bounds.left;
        int width = getWidth();
        if (bandsSize == 0) {
            int scrollH = 0;
            FCHScrollBar hScrollBar = getGrid().getHScrollBar();
            if (hScrollBar != null && hScrollBar.isVisible()) {
                scrollH = -hScrollBar.getPos();
            }
            int columnsSize = m_columns.size();
            for (int i = 0; i < columnsSize; i++) {
                FCBandedGridColumn column = m_columns.get(i);
                if (column.isVisible()) {
                    int columnWidth = column.getWidth();
                    if (i == columnsSize - 1 || left + columnWidth > width + bounds.left) {
                        columnWidth = width + bounds.left - left;
                    }
                    FCRect cellRect = new FCRect(left, bounds.bottom, left + columnWidth, bounds.bottom + column.getHeight());
                    column.setBounds(cellRect);
                    cellRect.left -= scrollH;
                    cellRect.right -= scrollH;
                    column.setHeaderRect(cellRect);
                    left += columnWidth;
                }
            }
        } else {
            for (int i = 0; i < bandsSize; i++) {
                FCGridBand band = m_bands.get(i);
                if (band.isVisible()) {
                    int bandWidth = band.getWidth();
                    if (i == bandsSize - 1 || left + bandWidth > width + bounds.left) {
                        bandWidth = width + bounds.left - left;
                    }
                    FCRect cellRect = new FCRect(left, bounds.bottom, left + bandWidth, bounds.bottom + band.getHeight());
                    band.setBounds(cellRect);
                    band.resetHeaderLayout();
                    left += bandWidth;
                }
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
        } else {
            super.setProperty(name, value);
        }
    }
}
