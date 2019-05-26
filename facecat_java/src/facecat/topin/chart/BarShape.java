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
 * 柱状图
 */
public class BarShape extends BaseShape {

    /**
     * 创建柱状图
     */
    public BarShape() {
        setZOrder(0);
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

    protected long m_downColor = FCColor.argb(82, 255, 255);

    /**
     * 获取阴线的颜色
     */
    public long getDownColor() {
        return m_downColor;
    }

    /**
     * 设置阴线的颜色
     */
    public void setDownColor(long value) {
        m_downColor = value;
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

    protected int m_fieldName2 = FCDataTable.NULLFIELD;

    /**
     * 获取字段名称2
     */
    public int getFieldName2() {
        return m_fieldName2;
    }

    /**
     * 设置字段名称2
     */
    public void setFieldName2(int value) {
        m_fieldName2 = value;
    }

    protected String m_fieldText = "";

    /**
     *  获取显示的标题名称
     */
    public String getFieldText() {
        return m_fieldText;
    }

    /**
     *  设置显示的标题名称
     */
    public void setFieldText(String value) {
        m_fieldText = value;
    }

    protected String m_fieldText2 = "";

    /**
     * 获取显示的标题名称2
     */
    public String getFieldText2() {
        return m_fieldText2;
    }

    /**
     * 设置显示的标题名称2
     */
    public void setFieldText2(String value) {
        m_fieldText2 = value;
    }

    protected int m_lineWidth = 1;

    /**
     * 获取线的宽度
     */
    public int getLineWidth() {
        return m_lineWidth;
    }

    /**
     * 设置线的宽度
     */
    public void setLineWidth(int value) {
        m_lineWidth = value;
    }

    protected BarStyle m_style = BarStyle.Rect;

    /**
     * 获取线条的样式
     */
    public BarStyle getStyle() {
        return m_style;
    }

    /**
     * 设置线条的样式
     */
    public void setStyle(BarStyle value) {
        m_style = value;
    }

    protected int m_styleField = FCDataTable.NULLFIELD;

    /**
     * 获取样式字段
     *
     * @param -10000:不画
     * @param -1:虚线空心矩形
     * @param 0:实心矩形
     * @param 1:实线空心矩形
     * @param 2:线
     */
    public int getStyleField() {
        return m_styleField;
    }

    /**
     * 设置样式字段
     */
    public void setStyleField(int value) {
        m_styleField = value;
    }

    protected long m_upColor = FCColor.argb(255, 82, 82);

    /**
     * 获取阳线的颜色
     */
    public long getUpColor() {
        return m_upColor;
    }

    /**
     * 设置阳线的颜色
     */
    public void setUpColor(long value) {
        m_upColor = value;
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
     *
     * @param fieldName 字段名称
     * @returns 字段标题
     */
    @Override
    public String getFieldText(int fieldName) {
        if (fieldName == m_fieldName) {
            return getFieldText();
        } else if (fieldName == m_fieldName2) {
            return getFieldText2();
        } else {
            return null;
        }
    }

    /**
     * 获取所有字段
     */
    @Override
    public int[] getFields() {
        if (m_fieldName2 == FCDataTable.NULLFIELD) {
            return new int[]{m_fieldName};
        } else {
            return new int[]{m_fieldName, m_fieldName2};
        }
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
        if (name.equals("colorfield")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getColorField());
        } else if (name.equals("downcolor")) {
            type.argvalue = "color";
            value.argvalue = FCStr.convertColorToStr(getDownColor());
        } else if (name.equals("fieldname")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getFieldName());
        } else if (name.equals("fieldname2")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getFieldName2());
        } else if (name.equals("fieldtext")) {
            type.argvalue = "string";
            value.argvalue = getFieldText();
        } else if (name.equals("fieldtext2")) {
            type.argvalue = "string";
            value.argvalue = getFieldText2();
        } else if (name.equals("linewidth")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getLineWidth());
        } else if (name.equals("style")) {
            type.argvalue = "enum:BarStyle";
            BarStyle style = getStyle();
            if (style == BarStyle.Line) {
                value.argvalue = "Line";
            } else {
                value.argvalue = "Rect";
            }
        } else if (name.equals("stylefield")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getStyleField());
        } else if (name.equals("upcolor")) {
            type.argvalue = "double";
            value.argvalue = FCStr.convertColorToStr(getUpColor());
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
        propertyNames.addAll(Arrays.asList(new String[]{"ColorField", "DownColor", "FieldName", "FieldName2", "FieldText", "FieldText2", "LineWidth", "Style", "StyleField", "UpColor"}));
        return propertyNames;
    }

    /**
     * 获取选中点的颜色
     *
     * @returns 颜色
     */
    @Override
    public long getSelectedColor() {
        return m_downColor;
    }
 
    /**
     * 设置属性值
     *
     * @param name 属性名称
     * @param value 属性值
     */
    @Override
    public void setProperty(String name, String value) {
        if (name.equals("colorfield")) {
            setColorField(FCStr.convertStrToInt(value));
        } else if (name.equals("downcolor")) {
            setDownColor(FCStr.convertStrToColor(value));
        } else if (name.equals("fieldname")) {
            setFieldName(FCStr.convertStrToInt(value));
        } else if (name.equals("fieldname2")) {
            setFieldName2(FCStr.convertStrToInt(value));
        } else if (name.equals("fieldtext")) {
            setFieldText(value);
        } else if (name.equals("fieldtext2")) {
            setFieldText2(value);
        } else if (name.equals("linewidth")) {
            setLineWidth(FCStr.convertStrToInt(value));
        } else if (name.equals("style")) {
            value = value.toLowerCase();
            if (value.equals("line")) {
                setStyle(BarStyle.Line);
            } else {
                setStyle(BarStyle.Rect);
            }
        } else if (name.equals("stylefield")) {
            setStyleField(FCStr.convertStrToInt(value));
        } else if (name.equals("upcolor")) {
            setUpColor(FCStr.convertStrToColor(value));
        } else {
            super.setProperty(name, value);
        }
    }
}
