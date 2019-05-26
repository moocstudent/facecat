/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.chart;

import facecat.topin.core.*;
import java.util.*;

/**
 * 曲线
 */
public class PolylineShape extends BaseShape {

    /**
     * 创建曲线
     */
    public PolylineShape() {
        setZOrder(2);
    }

    protected long m_color = FCColor.argb(255, 255, 0);

    /**
     * 获取线的颜色
     */
    public long getColor() {
        return m_color;
    }

    /**
     * 设置线的颜色
     */
    public void setColor(long value) {
        m_color = value;
    }

    protected int m_colorField = FCDataTable.NULLFIELD;

    /**
     * 获取颜色的字段
     */
    public int getColorField() {
        return m_colorField;
    }

    /**
     * 设置颜色的字段
     */
    public void setColorField(int value) {
        m_colorField = value;
    }

    protected int m_fieldName = FCDataTable.NULLFIELD;

    /**
     * 获取字段名称
     */
    public int getFieldName() {
        return m_fieldName;
    }

    /**
     * 设置字段名称
     */
    public void setFieldName(int value) {
        m_fieldName = value;
    }

    protected String m_fieldText = "";

    /**
     * 获取显示的标题名称
     */
    public String getFieldText() {
        return m_fieldText;
    }

    /**
     * 设置显示的标题名称
     */
    public void setFieldText(String value) {
        m_fieldText = value;
    }

    protected long m_fillColor = FCColor.None;

    /**
     * 获取填充色
     */
    public long getFillColor() {
        return m_fillColor;
    }

    /**
     * 设置填充色
     */
    public void setFillColor(long value) {
        m_fillColor = value;
    }

    protected PolylineStyle m_style = PolylineStyle.SolidLine;

    /**
     * 获取样式
     */
    public PolylineStyle getStyle() {
        return m_style;
    }

    /**
     * 设置样式
     */
    public void setStyle(PolylineStyle value) {
        m_style = value;
    }

    protected float m_width = 1;

    /**
     * 获取线的宽度
     */
    public float getWidth() {
        return m_width;
    }

    /**
     * 设置线的宽度
     */
    public void setWidth(float value) {
        m_width = value;
    }

    /**
     * 获取基础字段
     */
    @Override
    public int getBaseField() {
        return m_fieldName;
    }

    /**
     * 由字段名称获取字段标题
     */
    @Override
    public String getFieldText(int fieldName) {
        if (fieldName == m_fieldName) {
            return getFieldText();
        } else {
            return null;
        }
    }

    /**
     * 获取所有字段
     */
    @Override
    public int[] getFields() {
        return new int[]{m_fieldName};
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
        if (name.equals("color")) {
            type.argvalue = "color";
            value.argvalue = FCStr.convertColorToStr(getColor());
        } else if (name.equals("colorfield")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getColorField());
        } else if (name.equals("fieldname")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getFieldName());
        } else if (name.equals("fieldtext")) {
            type.argvalue = "string";
            value.argvalue = getFieldText();
        } else if (name.equals("fillcolor")) {
            type.argvalue = "color";
            value.argvalue = FCStr.convertColorToStr(getFillColor());
        } else if (name.equals("style")) {
            type.argvalue = "enum:PolylineStyle";
            PolylineStyle style = getStyle();
            if (style == PolylineStyle.Cycle) {
                value.argvalue = "Cycle";
            } else if (style == PolylineStyle.DashLine) {
                value.argvalue = "DashLine";
            } else if (style == PolylineStyle.DotLine) {
                value.argvalue = "DotLine";
            } else {
                value.argvalue = "SolidLine";
            }
        } else if (name.equals("width")) {
            type.argvalue = "float";
            value.argvalue = FCStr.convertFloatToStr(getWidth());
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
        propertyNames.addAll(Arrays.asList(new String[]{"Color", "ColorField", "FieldName", "FieldText", "FillColor", "Style", "Width"}));
        return propertyNames;
    }

    /**
     * 获取选中点的颜色
     */
    @Override
    public long getSelectedColor() {
        return m_color;
    }

    /**
     * 设置属性
     */
    @Override
    public void setProperty(String name, String value) {
        if (name.equals("color")) {
            setColor(FCStr.convertStrToColor(value));
        } else if (name.equals("colorfield")) {
            setColorField(FCStr.convertStrToInt(value));
        } else if (name.equals("fieldname")) {
            setFieldName(FCStr.convertStrToInt(value));
        } else if (name.equals("fieldtext")) {
            setFieldText(value);
        } else if (name.equals("fillcolor")) {
            setFillColor(FCStr.convertStrToColor(value));
        } else if (name.equals("style")) {
            value = value.toLowerCase();
            if (value.equals("cyle")) {
                setStyle(PolylineStyle.Cycle);
            } else if (value.equals("dashline")) {
                setStyle(PolylineStyle.DashLine);
            } else if (value.equals("dotline")) {
                setStyle(PolylineStyle.DotLine);
            } else {
                setStyle(PolylineStyle.SolidLine);
            }
        } else if (name.equals("width")) {
            setWidth(FCStr.convertStrToFloat(value));
        } else {
            super.setProperty(name, value);
        }
    }
}
