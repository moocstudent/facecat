/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.div;

import facecat.topin.core.*;
import java.util.*;

/**
 * 表格布局控件
 */
public class FCTableLayoutDiv extends FCDiv {

    /**
     * 创建布局控件
     */
    public FCTableLayoutDiv() {
    }

    /**
     * 列的集合
     */
    protected ArrayList<Integer> m_columns = new ArrayList<Integer>();

    /**
     * 行的集合
     */
    protected ArrayList<Integer> m_rows = new ArrayList<Integer>();

    /**
     * 表格控件
     */
    protected ArrayList<FCView> m_tableControls = new ArrayList<FCView>();

    protected int m_columnsCount;

    /**
     * 获取列的数量
     */
    public final int getColumnsCount() {
        return m_columnsCount;
    }

    /**
     * 设置列的数量
     */
    public final void setColumnsCount(int value) {
        m_columnsCount = value;
    }

    public ArrayList<FCColumnStyle> m_columnStyles = new ArrayList<FCColumnStyle>();

    /**
     * 获取列的样式
     */
    public final ArrayList<FCColumnStyle> getColumnStyles() {
        return m_columnStyles;
    }

    /**
     * 设置列的样式
     */
    public final void setColumnStyles(ArrayList<FCColumnStyle> value) {
        m_columnStyles = value;
    }

    protected int m_rowsCount;

    /**
     * 获取行的数量
     */
    public final int getRowsCount() {
        return m_rowsCount;
    }

    /**
     * 获设置行的数量
     */
    public final void setRowsCount(int value) {
        m_rowsCount = value;
    }

    public ArrayList<FCRowStyle> m_rowStyles = new ArrayList<FCRowStyle>();

    /**
     * 获取行的样式
     */
    public final ArrayList<FCRowStyle> getRowStyles() {
        return m_rowStyles;
    }

    /**
     * 设置行的样式
     */
    public final void setRowStyles(ArrayList<FCRowStyle> value) {
        m_rowStyles = value;
    }

    /**
     * 添加控件
     *
     * @param control 控件
     */
    @Override
    public void addControl(FCView control) {
        ArrayList<FCView> controls = getControls();
        int controlsSize = controls.size();
        super.addControl(control);
        int column = controlsSize % m_columnsCount;
        int row = controlsSize / m_columnsCount;
        m_columns.add(column);
        m_rows.add(row);
        m_tableControls.add(control);
    }

    /**
     * 添加控件
     *
     * @param control 控件
     * @param column 列
     * @param row 行
     */
    public void addControl(FCView control, int column, int row) {
        super.addControl(control);
        m_columns.add(column);
        m_rows.add(row);
        m_tableControls.add(control);
    }

    /**
     * 销毁资源方法
     */
    @Override
    public void delete() {
        if (!isDeleted()) {
            m_columns.clear();
            m_columnStyles.clear();
            m_rows.clear();
            m_rowStyles.clear();
            m_tableControls.clear();
        }
        super.delete();
    }

    /**
     * 获取控件类型
     */
    @Override
    public String getControlType() {
        return "TableLayoutDiv";
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
        if (name.equals("columnscount")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getColumnsCount());
        } else if (name.equals("rowscount")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getRowsCount());
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
        propertyNames.add("ColumnsCount");
        propertyNames.add("RowsCount");
        return propertyNames;
    }

    /**
     * 重置布局
     */
    public boolean onResetLayout() {
        if (getNative() != null) {
            if (m_columnsCount > 0 && m_rowsCount > 0 && m_columnStyles.size() > 0 && m_rowStyles.size() > 0) {
                int width = getWidth(), height = getHeight();
                int tabControlsSize = m_tableControls.size();
                //获取行列的宽度
                int[] columnWidths = new int[m_columnsCount];
                int[] rowHeights = new int[m_rowsCount];
                //获取列的宽度
                int allWidth = 0, allHeight = 0;
                for (int i = 0; i < m_columnsCount; i++) {
                    FCColumnStyle columnStyle = m_columnStyles.get(i);
                    int cWidth = 0;
                    FCSizeType sizeType = columnStyle.getSizeType();
                    float sWidth = columnStyle.getWidth();
                    if (sizeType == FCSizeType.AbsoluteSize) {
                        cWidth = (int) (sWidth);
                    } else if (sizeType == FCSizeType.AutoFill) {
                        cWidth = width - allWidth;
                    } else if (sizeType == FCSizeType.PercentSize) {
                        cWidth = (int) (width * sWidth);
                    }
                    columnWidths[i] = cWidth;
                    allWidth += cWidth;
                }
                for (int i = 0; i < m_rowsCount; i++) {
                    FCRowStyle rowStyle = m_rowStyles.get(i);
                    int rHeight = 0;
                    FCSizeType sizeType = rowStyle.getSizeType();
                    float sHeight = rowStyle.getHeight();
                    if (sizeType == FCSizeType.AbsoluteSize) {
                        rHeight = (int) (sHeight);
                    } else if (sizeType == FCSizeType.AutoFill) {
                        rHeight = height - allHeight;
                    } else if (sizeType == FCSizeType.PercentSize) {
                        rHeight = (int) (height * sHeight);
                    }
                    rowHeights[i] = rHeight;
                    allHeight += rHeight;
                }

                for (int i = 0; i < tabControlsSize; i++) {
                    FCView control = m_tableControls.get(i);
                    int column = m_columns.get(i);
                    int row = m_rows.get(i);
                    FCPadding margin = control.getMargin();

                    int cLeft = 0, cTop = 0;
                    for (int j = 0; j < column; j++) {
                        cLeft += columnWidths[j];
                    }
                    for (int j = 0; j < row; j++) {
                        cTop += rowHeights[j];
                    }
                    int cRight = cLeft + columnWidths[column] - margin.right;
                    int cBottom = cTop + rowHeights[row] - margin.bottom;
                    cLeft += margin.left;
                    cTop += margin.top;
                    if (cRight < cLeft) {
                        cRight = cLeft;
                    }
                    if (cBottom < cTop) {
                        cBottom = cTop;
                    }
                    control.setBounds(new FCRect(cLeft, cTop, cRight, cBottom));
                }
            }
        }
        return true;
    }

    /**
     * 移除控件
     *
     * @param control 控件
     */
    @Override
    public void removeControl(FCView control) {
        int tabControlsSize = m_tableControls.size();
        int index = -1;
        for (int i = 0; i < tabControlsSize; i++) {
            if (control == m_tableControls.get(i)) {
                index = i;
                break;
            }
        }
        if (index != -1) {
            m_columns.remove(index);
            m_rows.remove(index);
            m_tableControls.remove(index);
        }
        super.removeControl(control);
    }

    /**
     * 设置属性
     *
     * @param name 属性名称
     * @param value 属性值
     */
    @Override
    public void setProperty(String name, String value) {
        if (name.equals("columnscount")) {
            setColumnsCount(FCStr.convertStrToInt(value));
        } else if (name.equals("rowscount")) {
            setRowsCount(FCStr.convertStrToInt(value));
        } else {
            super.setProperty(name, value);
        }
    }

    /**
     * 布局更新方法
     */
    @Override
    public void update() {
        onResetLayout();
        int controlsSize = m_controls.size();
        for (int i = 0; i < controlsSize; i++) {
            m_controls.get(i).update();
        }
        updateScrollBar();
    }
}
