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
public class FCGridCell implements FCProperty {

    /**
     * 创建单元格
     */
    public FCGridCell() {
    }

    protected void finalize() throws Throwable {
        delete();
    }

    protected boolean m_allowEdit = false;

    /**
     * 获取是否可编辑
     */
    public boolean allowEdit() {
        return m_allowEdit;
    }

    /**
     * 设置是否可编辑
     */
    public void setAllowEdit(boolean value) {
        m_allowEdit = value;
    }

    protected int colSpan = 1;

    /**
     * 获取跨越的列数
     */
    public int getColSpan() {
        return colSpan;
    }

    /**
     * 设置跨越的列数
     */
    public void setColSpan(int value) {
        colSpan = value;
    }

    protected FCGridColumn m_column = null;

    /**
     * 获取所在列
     */
    public FCGridColumn getColumn() {
        return m_column;
    }

    /**
     * 设置所在列
     */
    public void setColumn(FCGridColumn value) {
        m_column = value;
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

    protected boolean m_isDeleted;

    /**
     * 获取或设置是否已被销毁
     */
    public boolean isDeleted() {
        return m_isDeleted;
    }

    protected String m_name;

    /**
     * 获取名称
     */
    public String getName() {
        return m_name;
    }

    /**
     * 设置名称
     */
    public void setName(String value) {
        m_name = value;
    }

    protected FCGridRow m_row = null;

    /**
     * 获取所在行
     */
    public FCGridRow getRow() {
        return m_row;
    }

    /**
     * 设置所在行
     */
    public void setRow(FCGridRow value) {
        m_row = value;
    }

    protected int rowSpan = 1;

    /**
     * 获取跨越的行数
     */
    public int getRowSpan() {
        return rowSpan;
    }

    /**
     * 设置跨越的行数
     */
    public void setRowSpan(int value) {
        rowSpan = value;
    }

    protected FCGridCellStyle m_style = null;

    /**
     * 获取样式
     */
    public FCGridCellStyle getStyle() {
        return m_style;
    }

    /**
     * 设置样式
     */
    public void setStyle(FCGridCellStyle value) {
        m_style = value;
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

    /**
     * 获取文字
     */
    public String getText() {
        return getString();
    }

    /**
     * 设置文字
     */
    public void setText(String value) {
        setString(value);
    }

    /**
     * 单元格大小比较，用于排序
     *
     * @param cell 比较单元格
     * @returns 1:较大 0:相等 -1:较小
     */
    public int compareTo(FCGridCell cell) {
        return 0;
    }

    /**
     * 销毁资源
     */
    public void delete() {
        m_isDeleted = true;
    }

    /**
     * 获取布尔型数值
     */
    public boolean getBool() {
        return false;
    }

    /**
     * 获取双精度浮点值
     */
    public double getDouble() {
        return 0;
    }

    /**
     * 获取单精度浮点值
     */
    public float getFloat() {
        return 0;
    }

    /**
     * 获取整型数值
     */
    public int getInt() {
        return 0;
    }

    /**
     * 获取长整型数值
     */
    public long getLong() {
        return 0;
    }

    /**
     * 获取要绘制的文字
     */
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
    public void getProperty(String name, RefObject<String> value, RefObject<String> type) {
        if (name.equals("align")) {
            type.argvalue = "enum:FCHorizontalAlign";
            FCGridCellStyle style = getStyle();
            if (style != null) {
                value.argvalue = FCStr.convertHorizontalAlignToStr(style.getAlign());
            }
        } else if (name.equals("allowedit")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(allowEdit());
        } else if (name.equals("autoellipsis")) {
            type.argvalue = "bool";
            FCGridCellStyle style = getStyle();
            if (style != null) {
                value.argvalue = FCStr.convertBoolToStr(style.autoEllipsis());
            }
        } else if (name.equals("backcolor")) {
            type.argvalue = "color";
            FCGridCellStyle style = getStyle();
            if (style != null) {
                value.argvalue = FCStr.convertColorToStr(style.getBackColor());
            }
        } else if (name.equals("colspan")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getColSpan());
        } else if (name.equals("font")) {
            type.argvalue = "font";
            FCGridCellStyle style = getStyle();
            if (style != null && style.getFont() != null) {
                value.argvalue = FCStr.convertFontToStr(style.getFont());
            }
        } else if (name.equals("textcolor")) {
            type.argvalue = "color";
            FCGridCellStyle style = getStyle();
            if (style != null) {
                value.argvalue = FCStr.convertColorToStr(style.getTextColor());
            }
        } else if (name.equals("name")) {
            type.argvalue = "string";
            value.argvalue = getName();
        } else if (name.equals("rowspan")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getRowSpan());
        } else if (name.equals("text")) {
            type.argvalue = "string";
            value.argvalue = getText();
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
        propertyNames.addAll(Arrays.asList(new String[]{"Align", "AllowEdit", "AutoEllipsis", "BackColor", "ColSpan", "Font", "Name", "RowSpan", "Text", "TextColor"}));
        return propertyNames;
    }

    /**
     * 获取字符型数值
     */
    public String getString() {
        return "";
    }

    /**
     * 添加单元格方法
     */
    public void onadd() {
    }

    /**
     * 重绘方法
     *
     * @param paint 绘图对象
     * @param rect 矩形
     * @param clipRect 裁剪矩形
     * @param isAlternate 是否交替行
     */
    public void onPaint(FCPaint paint, FCRect rect, FCRect clipRect, boolean isAlternate) {
        int clipW = clipRect.right - clipRect.left;
        int clipH = clipRect.bottom - clipRect.top;
        if (clipW > 0 && clipH > 0) {
            if (m_grid != null && m_row != null && m_column != null) {
                // 判断选中
                String text = getPaintText();
                boolean selected = false;
                if (m_grid.getSelectionMode() == FCGridSelectionMode.SelectCell) {
                    ArrayList<FCGridCell> selectedCells = m_grid.getSelectedCells();
                    int selectedCellSize = selectedCells.size();
                    for (int i = 0; i < selectedCellSize; i++) {
                        if (selectedCells.get(i) == this) {
                            selected = true;
                            break;
                        }
                    }
                } else if (m_grid.getSelectionMode() == FCGridSelectionMode.SelectFullColumn) {
                    ArrayList<FCGridColumn> selectedColumns = m_grid.getSelectedColumns();
                    int selectedColumnsSize = selectedColumns.size();
                    for (int i = 0; i < selectedColumnsSize; i++) {
                        if (selectedColumns.get(i) == m_column) {
                            selected = true;
                            break;
                        }
                    }
                } else if (m_grid.getSelectionMode() == FCGridSelectionMode.SelectFullRow) {
                    ArrayList<FCGridRow> selectedRows = m_grid.getSelectedRows();
                    int selectedRowsSize = selectedRows.size();
                    for (int i = 0; i < selectedRowsSize; i++) {
                        if (selectedRows.get(i) == m_row) {
                            selected = true;
                            break;
                        }
                    }
                }
                // 获取颜色
                FCFont font = null;
                long backColor = FCColor.None;
                long textColor = FCColor.None;
                boolean autoEllipsis = m_grid.autoEllipsis();
                FCHorizontalAlign horizontalAlign = m_column.getCellAlign();
                if (m_style != null) {
                    if (m_style.autoEllipsis()) {
                        autoEllipsis = m_style.autoEllipsis();
                    }
                    backColor = m_style.getBackColor();
                    if (m_style.getFont() != null) {
                        font = m_style.getFont();
                    }
                    textColor = m_style.getTextColor();
                    if (m_style.getAlign() != FCHorizontalAlign.Inherit) {
                        horizontalAlign = m_style.getAlign();
                    }
                }
                FCGridRowStyle rowStyle = m_grid.getRowStyle();
                if (isAlternate) {
                    FCGridRowStyle alternateRowStyle = m_grid.getAlternateRowStyle();
                    if (alternateRowStyle != null) {
                        rowStyle = alternateRowStyle;
                    }
                }
                if (rowStyle != null) {
                    if (backColor == FCColor.None) {
                        if (selected) {
                            backColor = rowStyle.getSelectedBackColor();
                        } else if (m_row == m_grid.getHoveredRow()) {
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
                        } else if (m_row == m_grid.getHoveredRow()) {
                            textColor = rowStyle.getHoveredTextColor();
                        } else {
                            textColor = rowStyle.getTextColor();
                        }
                    }
                }
                paint.fillRect(backColor, rect);
                FCSize tSize = paint.textSize(text, font);
                FCPoint tPoint = new FCPoint(rect.left + 1, rect.top + clipH / 2 - tSize.cy / 2);
                if (tSize.cx < clipW) {
                    if (horizontalAlign == FCHorizontalAlign.Center) {
                        tPoint.x = rect.left + (rect.right - rect.left - tSize.cx) / 2;
                    } else if (horizontalAlign == FCHorizontalAlign.Right) {
                        tPoint.x = rect.right - tSize.cx - 2;
                    }
                }
                FCRect tRect = new FCRect(tPoint.x, tPoint.y, tPoint.x + tSize.cx, tPoint.y + tSize.cy);
                if (autoEllipsis && (tRect.right > clipRect.right || tRect.bottom > clipRect.bottom)) {
                    if (tRect.right > clipRect.right) {
                        tRect.right = clipRect.right;
                    }
                    if (tRect.bottom > clipRect.bottom) {
                        tRect.bottom = clipRect.bottom;
                    }
                    paint.drawTextAutoEllipsis(text, textColor, font, tRect);
                } else {
                    paint.drawText(text, textColor, font, tRect);
                }
            }
        }
    }

    /**
     * 移除单元格方法
     */
    public void onremove() {
    }

    /**
     * 设置布尔型数值
     *
     * @param value 数值
     */
    public void setBool(boolean value) {
    }

    /**
     * 设置双精度浮点值
     *
     * @param value 数值
     */
    public void setDouble(double value) {
    }

    /**
     * 设置单精度浮点值
     *
     * @param value 数值
     */
    public void setFloat(float value) {
    }

    /**
     * 设置整型数值
     *
     * @param value 数值
     */
    public void setInt(int value) {
    }

    /**
     * 设置长整型数值
     *
     * @param value 数值
     */
    public void setLong(long value) {
    }

    /**
     * 设置属性
     *
     * @param name 属性名称
     * @param value 属性值
     */
    public void setProperty(String name, String value) {
        if (name.equals("align")) {
            if (m_style == null) {
                m_style = new FCGridCellStyle();
            }
            m_style.setAlign(FCStr.convertStrToHorizontalAlign(value));
        } else if (name.equals("allowedit")) {
            setAllowEdit(FCStr.convertStrToBool(value));
        } else if (name.equals("autoellipsis")) {
            if (m_style == null) {
                m_style = new FCGridCellStyle();
            }
            m_style.setAutoEllipsis(FCStr.convertStrToBool(value));
        } else if (name.equals("backcolor")) {
            if (m_style == null) {
                m_style = new FCGridCellStyle();
            }
            m_style.setBackColor(FCStr.convertStrToColor(value));
        } else if (name.equals("colspan")) {
            setColSpan(FCStr.convertStrToInt(value));
        } else if (name.equals("font")) {
            if (m_style == null) {
                m_style = new FCGridCellStyle();
            }
            m_style.setFont(FCStr.convertStrToFont(value));
        } else if (name.equals("textcolor")) {
            if (m_style == null) {
                m_style = new FCGridCellStyle();
            }
            m_style.setTextColor(FCStr.convertStrToColor(value));
        } else if (name.equals("name")) {
            setName(value);
        } else if (name.equals("rowspan")) {
            setRowSpan(FCStr.convertStrToInt(value));
        } else if (name.equals("text")) {
            setText(value);
        }
    }

    /**
     * 设置字符型数值
     *
     * @param value 数值
     */
    public void setString(String value) {
    }
}
