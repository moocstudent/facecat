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
 * 标题
 */
public class ChartTitle implements FCProperty {

    /**
     * 创建标题
     *
     * @param fieldName 字段名称
     * @param fieldText 字段文字
     * @param textColor 文字颜色
     * @param digit 保留小数位数
     * @param visible 是否可见
     */
    public ChartTitle(int fieldName, String fieldText, long textColor, int digit, boolean visible) {
        m_fieldName = fieldName;
        m_fieldText = fieldText;
        m_textColor = textColor;
        m_digit = digit;
        m_visible = visible;
    }

    protected int m_digit = 2;

    /**
     * 获取保留小数的位数
     */
    public int getDigit() {
        return m_digit;
    }

    /**
     * 设置保留小数的位数
     */
    public void setDigit(int value) {
        m_digit = value;
    }

    protected int m_fieldName;

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

    protected String m_fieldText;

    /**
     * 获取字段文字
     */
    public String getFieldText() {
        return m_fieldText;
    }

    /**
     * 设置字段文字
     */
    public void setFieldText(String value) {
        m_fieldText = value;
    }

    protected TextMode m_fieldTextMode = TextMode.Full;

    /**
     * 获取标题显示模式
     */
    public TextMode getFieldTextMode() {
        return m_fieldTextMode;
    }

    /**
     * 设置标题显示模式
     */
    public void setFieldTextMode(TextMode value) {
        m_fieldTextMode = value;
    }

    protected String m_fieldTextSeparator = " ";

    /**
     * 获取标题的分隔符
     */
    public String getFieldTextSeparator() {
        return m_fieldTextSeparator;
    }

    /**
     * 设置标题的分隔符
     */
    public void setFieldTextSeparator(String value) {
        m_fieldTextSeparator = value;
    }

    protected long m_textColor;

    /**
     * 获取文字的颜色
     */
    public long getTextColor() {
        return m_textColor;
    }

    /**
     * 设置文字的颜色
     */
    public void setTextColor(long value) {
        m_textColor = value;
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

    /**
     * 获取属性值
     *
     * @param name 属性名称
     * @param value 返回属性值
     * @param type 返回属性类型
     */
    public void getProperty(String name, RefObject<String> value, RefObject<String> type) {
        if (name.equals("digit")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getDigit());
        } else if (name.equals("fieldname")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getFieldName());
        } else if (name.equals("fieldtext")) {
            type.argvalue = "text";
            value.argvalue = getFieldText();
        } else if (name.equals("fieldtextmode")) {
            type.argvalue = "enum:TextMode";
            TextMode fieldTextMode = getFieldTextMode();
            if (fieldTextMode == TextMode.Field) {
                value.argvalue = "field";
            } else if (fieldTextMode == TextMode.Full) {
                value.argvalue = "full";
            } else if (fieldTextMode == TextMode.None) {
                value.argvalue = "none";
            } else {
                value.argvalue = "value";
            }
        } else if (name.equals("fieldtextseparator")) {
            value.argvalue = getFieldTextSeparator();
        } else if (name.equals("textcolor")) {
            type.argvalue = "color";
            value.argvalue = FCStr.convertColorToStr(getTextColor());
        } else if (name.equals("visible")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(isVisible());
        }
    }

    /**
     * 获取属性名称列表
     */
    public ArrayList<String> getPropertyNames() {
        ArrayList<String> propertyNames = new ArrayList<String>();
        propertyNames.addAll(Arrays.asList(new String[]{"Digit", "FieldName", "FieldText", "FieldTextMode", "FieldTextSeparator", "TextColor", "Visible"}));
        return propertyNames;
    }

    /**
     * 设置属性值
     *
     * @param name 属性名称
     * @param value 属性值
     */
    public void setProperty(String name, String value) {
        if (name.equals("digit")) {
            setDigit(FCStr.convertStrToInt(value));
        } else if (name.equals("fieldname")) {
            setFieldName(FCStr.convertStrToInt(value));
        } else if (name.equals("fieldtext")) {
            setFieldText(value);
        } else if (name.equals("fieldtextmode")) {
            value = value.toLowerCase();
            if (value.equals("field")) {
                setFieldTextMode(TextMode.Field);
            } else if (value.equals("full")) {
                setFieldTextMode(TextMode.Full);
            } else if (value.equals("none")) {
                setFieldTextMode(TextMode.None);
            } else {
                setFieldTextMode(TextMode.None);
            }
        } else if (name.equals("fieldtextseparator")) {
            setFieldTextSeparator(value);
        } else if (name.equals("textcolor")) {
            setTextColor(FCStr.convertStrToColor(value));
        } else if (name.equals("visible")) {
            setVisible(FCStr.convertStrToBool(value));
        }
    }
}
