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
 * 文字
 */
public class TextShape extends BaseShape {

    /**
     * 创建文字
     */
    public TextShape() {
        setZOrder(4);
    }

    protected int m_colorField = FCDataTable.NULLFIELD;

    /**
     * 获取颜色字段
     */
    public int getColorField() {
        return m_colorField;
    }

    /**
     * 设置颜色字段
     */
    public void setColorField(int value) {
        m_colorField = value;
    }

    protected int m_fieldName = FCDataTable.NULLFIELD;

    /**
     * 获取字段
     */
    public int getFieldName() {
        return m_fieldName;
    }

    /**
     * 设置字段
     */
    public void setFieldName(int value) {
        m_fieldName = value;
    }

    protected FCFont m_font = new FCFont();

    /**
     * 获取字体
     */
    public FCFont getFont() {
        return m_font;
    }

    /**
     * 设置字体
     */
    public void setFont(FCFont value) {
        m_font = value;
    }

    protected int m_styleField = FCDataTable.NULLFIELD;

    /**
     * 获取样式字段
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

    protected String m_text;

    /**
     * 获取绘制的文字
     */
    public String getText() {
        return m_text;
    }

    /**
     * 设置绘制的文字
     */
    public void setText(String value) {
        m_text = value;
    }

    protected long m_textColor = FCColor.argb(255, 255, 255);

    /**
     * 获取前景色
     */
    public long getTextColor() {
        return m_textColor;
    }

    /**
     * 设置前景色
     */
    public void setTextColor(long value) {
        m_textColor = value;
    }

    @Override
    /**
     * 获取属性值
     *
     * @param name 属性名称
     * @param value 返回属性值
     * @param type 返回属性类型
     */
    public void getProperty(String name, RefObject<String> value, RefObject<String> type) {
        if (name.equals("colorfield")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getColorField());
        } else if (name.equals("fieldname")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getFieldName());
        } else if (name.equals("font")) {
            type.argvalue = "font";
            value.argvalue = FCStr.convertFontToStr(getFont());
        } else if (name.equals("textcolor")) {
            type.argvalue = "color";
            value.argvalue = FCStr.convertColorToStr(getTextColor());
        } else if (name.equals("stylefield")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getStyleField());
        } else if (name.equals("text")) {
            type.argvalue = "string";
            value.argvalue = getText();
        } else {
            super.getProperty(name, value, type);
        }
    }

    @Override
    /**
     * 获取属性名称列表
     */
    public ArrayList<String> getPropertyNames() {
        ArrayList<String> propertyNames = new ArrayList<String>();
        propertyNames.addAll(Arrays.asList(new String[]{"ColorField", "FieldName", "Font", "StyleField", "Text", "TextColor"}));
        return propertyNames;
    }

    @Override
    /**
     * 设置属性值
     *
     * @param name 属性名称
     * @param value 属性值
     */
    public void setProperty(String name, String value) {
        if (name.equals("colorfield")) {
            setColorField(FCStr.convertStrToInt(value));
        } else if (name.equals("fieldname")) {
            setFieldName(FCStr.convertStrToInt(value));
        } else if (name.equals("font")) {
            setFont(FCStr.convertStrToFont(value));
        } else if (name.equals("textcolor")) {
            setTextColor(FCStr.convertStrToColor(value));
        } else if (name.equals("stylefield")) {
            setStyleField(FCStr.convertStrToInt(value));
        } else if (name.equals("text")) {
            setText(value);
        } else {
            super.setProperty(name, value);
        }
    }
}
