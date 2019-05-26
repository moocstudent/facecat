/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.grid;

import facecat.topin.core.*;
import java.util.*;

/**
 * 单元格
 */
public class FCGridRow implements FCProperty {

    /**
     * 创建行
     */
    public FCGridRow() {
    }

    protected void finalize() throws Throwable {
        delete();
    }

    /**
     * 所有单元格
     */
    public ArrayList<FCGridCell> m_cells = new ArrayList<FCGridCell>();

    /**
     * 编辑状态
     */
    public int m_editState;

    protected boolean m_allowEdit;

    /**
     * 获取是否允许编辑
     */
    public boolean allowEdit() {
        return m_allowEdit;
    }

    /**
     * 设置是否允许编辑
     */
    public void setAllowEdit(boolean allowEdit) {
        m_allowEdit = allowEdit;
    }

    protected FCRect m_bounds = new FCRect();

    /**
     * 获取显示区域
     */
    public FCRect getBounds() {
        return m_bounds.clone();
    }

    /**
     * 设置显示区域
     */
    public void setBounds(FCRect value) {
        m_bounds = value.clone();
    }

    protected FCView m_editButton;

    /**
     * 获取编辑按钮
     */
    public FCView getEditButton() {
        return m_editButton;
    }

    /**
     * 设置编辑按钮
     */
    public void setEditButton(FCView editButton) {
        m_editButton = editButton;
    }

    protected FCGrid m_grid = null;

    /**
     * 获取所在表格
     */
    public FCGrid getGrid() {
        return m_grid;
    }

    /**
     * 设置所在表格
     */
    public void setGrid(FCGrid value) {
        m_grid = value;
    }

    protected int height = 20;

    /**
     * 获取行高
     */
    public int getHeight() {
        return height;
    }

    /**
     * 设置行高
     */
    public void setHeight(int value) {
        height = value;
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

    protected boolean m_isDeleted = false;

    /**
     * 获取或设置是否已被销毁
     */
    public boolean isDeleted() {
        return m_isDeleted;
    }

    protected Object m_tag = null;

    /**
     * 获取TAG值
     */
    public Object getTag() {
        return m_tag;
    }

    /**
     * 设置TAG值
     */
    public void setTag(Object value) {
        m_tag = value;
    }

    protected boolean m_visible = true;

    /**
     * 获取是否可见
     */
    public boolean isVisible() {
        return m_visible;
    }

    /**
     * 设置是否可见
     */
    public void setVisible(boolean value) {
        m_visible = value;
    }

    protected int m_visibleIndex = -1;

    /**
     * 获取可见索引
     */
    public int getVisibleIndex() {
        return m_visibleIndex;
    }

    /**
     * 设置可见索引
     */
    public void setVisibleIndex(int value) {
        m_visibleIndex = value;
    }

    /**
     * 添加单元格
     *
     * @param column 列索引
     * @param cell 单元格
     */
    public void addCell(FCGridColumn column, FCGridCell cell) {
        cell.setGrid(m_grid);
        cell.setColumn(column);
        cell.setRow(this);
        m_cells.add(cell);
        cell.onadd();
    }

    /**
     * 添加单元格
     *
     * @param columnIndex 列索引
     * @param cell 单元格
     */
    public void addCell(int columnIndex, FCGridCell cell) {
        cell.setGrid(m_grid);
        cell.setColumn(m_grid.getColumn(columnIndex));
        cell.setRow(this);
        m_cells.add(cell);
        cell.onadd();
    }

    /**
     * 添加单元格
     *
     * @param columnName 列名
     * @param cell 单元格
     */
    public void addCell(String columnName, FCGridCell cell) {
        cell.setGrid(m_grid);
        cell.setColumn(m_grid.getColumn(columnName));
        cell.setRow(this);
        m_cells.add(cell);
        cell.onadd();
    }

    /**
     * 清除单元格
     */
    public void clearCells() {
        int cellSize = m_cells.size();
        for (int i = 0; i < cellSize; i++) {
            m_cells.get(i).onremove();
            m_cells.get(i).delete();
        }
        m_cells.clear();
    }

    /**
     * 销毁对象
     */
    public void delete() {
        if (!m_isDeleted) {
            m_isDeleted = !m_isDeleted;
            int cellSize = m_cells.size();
            for (int i = 0; i < cellSize; i++) {
                m_cells.get(i).delete();
            }
            m_cells.clear();
        }
    }

    /**
     * 根据列获取单元格
     *
     * @param column 列
     * @param cell 单元格
     */
    public FCGridCell getCell(FCGridColumn column) {
        int cellSize = m_cells.size();
        for (int i = 0; i < cellSize; i++) {
            if (m_cells.get(i).getColumn() == column) {
                return m_cells.get(i);
            }
        }
        return null;
    }

    /**
     * 根据列的索引获取单元格
     */
    public FCGridCell getCell(int columnIndex) {
        int cellsSize = m_cells.size();
        if (cellsSize > 0) {
            if (columnIndex >= 0 && columnIndex < cellsSize) {
                if (m_cells.get(columnIndex).getColumn().getIndex() == columnIndex) {
                    return m_cells.get(columnIndex);
                }
            }
            for (int i = 0; i < cellsSize; i++) {
                if (m_cells.get(i).getColumn().getIndex() == columnIndex) {
                    return m_cells.get(i);
                }
            }
        }
        return null;
    }

    /**
     * 根据列的名称获取单元格
     *
     * @param columnName 列名
     * @param cell 单元格
     */
    public FCGridCell getCell(String columnName) {
        int cellSize = m_cells.size();
        for (int i = 0; i < cellSize; i++) {
            if (m_cells.get(i).getColumn().getName().equals(columnName)) {
                return m_cells.get(i);
            }
        }
        return null;
    }

    /**
     * 获取所有单元格
     */
    public ArrayList<FCGridCell> getCells() {
        return m_cells;
    }

    /**
     * 获取属性值
     *
     * @param name 属性名称
     * @param value 返回属性值
     * @param type 返回属性类型
     */
    public void getProperty(String name, RefObject<String> value, RefObject<String> type) {
        if (name.equals("allowedit")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(allowEdit());
        }
        if (name.equals("height")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getHeight());
        } else if (name.equals("visible")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertBoolToStr(isVisible());
        } else {
            type.argvalue = "undefined";
            value.argvalue = "";
        }
    }

    /**
     * 获取属性名称列表
     */
    public ArrayList<String> getPropertyNames() {
        ArrayList<String> propertyNames = new ArrayList<String>();
        propertyNames.addAll(Arrays.asList(new String[]{"AllowEdit", "Height", "Visible"}));
        return propertyNames;
    }

    /**
     * 添加行方法
     */
    public void onadd() {
        int cellSize = m_cells.size();
        for (int i = 0; i < cellSize; i++) {
            FCGridCell cell = m_cells.get(i);
            cell.onadd();
        }
    }

    /**
     * 重绘方法
     *
     * @param paint 绘图对象
     * @param clipRect 裁剪矩形
     * @param isAlternate 是否交替行
     */
    public void onPaint(FCPaint paint, FCRect clipRect, boolean isAlternate) {

    }

    /**
     * 重绘边线方法
     *
     * @param paint 绘图对象
     * @param clipRect 裁剪矩形
     * @param isAlternate 是否交替行
     */
    public void onPaintBorder(FCPaint paint, FCRect clipRect, boolean isAlternate) {

    }

    /**
     * 移除行方法
     */
    public void onremove() {
        int cellSize = m_cells.size();
        for (int i = 0; i < cellSize; i++) {
            FCGridCell cell = m_cells.get(i);
            cell.onremove();
        }
    }

    /**
     * 移除单元格
     *
     * @param column 列
     */
    public void removeCell(FCGridColumn column) {
        int cellSize = m_cells.size();
        for (int i = 0; i < cellSize; i++) {
            FCGridCell cell = m_cells.get(i);
            if (cell.getColumn() == column) {
                m_cells.remove(cell);
                cell.onremove();
                break;
            }
        }
    }

    /**
     * 移除单元格
     *
     * @param columnIndex 列索引
     */
    public void removeCell(int columnIndex) {
        int cellSize = m_cells.size();
        if (columnIndex >= 0 && columnIndex < cellSize) {
            FCGridCell cell = m_cells.get(columnIndex);
            if (cell.getColumn().getIndex() == columnIndex) {
                m_cells.remove(cell);
                cell.onremove();
                return;
            }
            for (int i = 0; i < cellSize; i++) {
                cell = m_cells.get(i);
                if (cell.getColumn().getIndex() == columnIndex) {
                    m_cells.remove(cell);
                    cell.onremove();
                    break;
                }
            }
        }
    }

    /**
     * 移除单元格
     *
     * @param columnName 列名
     */
    public void removeCell(String columnName) {
        int cellSize = m_cells.size();
        for (int i = 0; i < cellSize; i++) {
            FCGridCell cell = m_cells.get(i);
            if (cell.getColumn().getName().equals(columnName)) {
                m_cells.remove(cell);
                cell.onremove();
                break;
            }
        }
    }

    /**
     * 设置属性值
     *
     * @param name 属性名称
     * @param value 属性值
     */
    public void setProperty(String name, String value) {
        if (name.equals("allowedit")) {
            setAllowEdit(FCStr.convertStrToBool(value));
        } else if (name.equals("height")) {
            setHeight(FCStr.convertStrToInt(value));
        } else if (name.equals("visible")) {
            setVisible(FCStr.convertStrToBool(value));
        }
    }
}
