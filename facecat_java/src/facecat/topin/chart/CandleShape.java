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
 * K线
 */
public class CandleShape extends BaseShape {

    /**
     * 创建K线
     */
    public CandleShape() {
        setZOrder(1);
    }

    protected int m_closeField = FCDataTable.NULLFIELD;

    /**
     * 获取收盘价字段
     */
    public int getCloseField() {
        return m_closeField;
    }

    /**
     * 设置收盘价字段
     */
    public void setCloseField(int value) {
        m_closeField = value;
    }

    protected int m_colorField = FCDataTable.NULLFIELD;

    /**
     *  获取颜色的字段
     */
    public int getColorField() {
        return m_colorField;
    }

    /**
     *  设置颜色的字段
     */
    public void setColorField(int value) {
        m_colorField = value;
    }

    protected String m_closeFieldText;

    /**
     * 获取收盘价的显示文字
     */
    public String getCloseFieldText() {
        return m_closeFieldText;
    }

    /**
     * 设置收盘价的显示文字
     */
    public void setCloseFieldText(String value) {
        m_closeFieldText = value;
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

    protected int m_highField = FCDataTable.NULLFIELD;

    /**
     * 获取最高价字段
     */
    public int getHighField() {
        return m_highField;
    }

    /**
     * 设置最高价字段
     */
    public void setHighField(int value) {
        m_highField = value;
    }

    protected String m_highFieldText;

    /**
     * 获取最高价的显示文字
     */
    public String getHighFieldText() {
        return m_highFieldText;
    }

    /**
     * 设置最高价的显示文字
     */
    public void setHighFieldText(String value) {
        m_highFieldText = value;
    }

    protected int m_lowField = FCDataTable.NULLFIELD;

    /**
     * 获取最低价字段
     */
    public int getLowField() {
        return m_lowField;
    }

    /**
     * 设置最低价字段
     */
    public void setLowField(int value) {
        m_lowField = value;
    }

    protected String m_lowFieldText;

    /**
     * 获取最低价的显示文字
     */
    public String getLowFieldText() {
        return m_lowFieldText;
    }

    /**
     * 设置最低价的显示文字
     */
    public void setLowFieldText(String value) {
        m_lowFieldText = value;
    }

    protected int m_openField = FCDataTable.NULLFIELD;

    /**
     * 获取开盘价字段
     */
    public int getOpenField() {
        return m_openField;
    }

    /**
     * 设置开盘价字段
     */
    public void setOpenField(int value) {
        m_openField = value;
    }

    protected String m_openFieldText;

    /**
     * 获取开盘价的显示文字
     */
    public String getOpenFieldText() {
        return m_openFieldText;
    }

    /**
     * 设置开盘价的显示文字
     */
    public void setOpenFieldText(String value) {
        m_openFieldText = value;
    }

    protected boolean m_showMaxMin = true;

    /**
     * 获取是否显示最大最小值
     */
    public boolean showMaxMin() {
        return m_showMaxMin;
    }

    /**
     * 设置是否显示最大最小值
     */
    public void setShowMaxMin(boolean value) {
        m_showMaxMin = value;
    }

    protected CandleStyle m_style = CandleStyle.Rect;

    /**
     * 获取线柱的类型
     */
    public CandleStyle getStyle() {
        return m_style;
    }

    /**
     * 设置线柱的类型
     */
    public void setStyle(CandleStyle value) {
        m_style = value;
    }

    protected int m_styleField = FCDataTable.NULLFIELD;

    /**
     * 获取样式字段
     *
     * @param -10000:不画
     * @param 0:阳线空心阴线实心
     * @param 1:空心矩形
     * @param 2:实心矩形
     * @param 3:美国线
     * @param 4:收盘线
     * @param 5:宝塔线
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

    protected long m_tagColor = FCColor.argb(255, 255, 255);

    /**
     * 获取最大最小值标签的颜色
     */
    public long getTagColor() {
        return m_tagColor;
    }

    /**
     * 设置最大最小值标签的颜色
     */
    public void setTagColor(long value) {
        m_tagColor = value;
    }

    protected long m_upColor = FCColor.argb(255, 82, 82);

    /**
     * 获取阳线颜色
     */
    public long getUpColor() {
        return m_upColor;
    }

    /**
     * 设置阳线颜色
     */
    public void setUpColor(long value) {
        m_upColor = value;
    }

    /**
     * 获取基础字段
     */
    @Override
    public int getBaseField() {
        return m_closeField;
    }
    
    /**
     * 由字段名称获取字段标题
     *
     * @param fieldName 字段名称
     * @returns 字段标题
     */
    @Override
    public String getFieldText(int fieldName) {
        if (fieldName == m_closeField) {
            return getCloseFieldText();
        } else if (fieldName == m_highField) {
            return getHighFieldText();
        } else if (fieldName == m_lowField) {
            return getLowFieldText();
        } else if (fieldName == m_openField) {
            return getOpenFieldText();
        } else {
            return null;
        }
    }

    /**
     * 获取所有字段
     */
    @Override
    public int[] getFields() {
        return new int[]{m_closeField, m_highField, m_lowField, m_openField};
    }
  
    /**
     * 设置属性值
     *
     * @param name 属性名称
     * @param value 返回属性值
     * @param type 返回属性类型
     */
    @Override
    public void getProperty(String name, RefObject<String> value, RefObject<String> type) {
        if (name.equals("closefield")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getCloseField());
        } else if (name.equals("colorfield")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getColorField());
        } else if (name.equals("closefieldtext")) {
            type.argvalue = "string";
            value.argvalue = getCloseFieldText();
        } else if (name.equals("downcolor")) {
            type.argvalue = "color";
            value.argvalue = FCStr.convertColorToStr(getDownColor());
        } else if (name.equals("highfield")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getHighField());
        } else if (name.equals("highfieldtext")) {
            type.argvalue = "string";
            value.argvalue = getHighFieldText();
        } else if (name.equals("lowfield")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getLowField());
        } else if (name.equals("lowfieldtext")) {
            type.argvalue = "string";
            value.argvalue = getLowFieldText();
        } else if (name.equals("openfield")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getOpenField());
        } else if (name.equals("openfieldtext")) {
            type.argvalue = "string";
            value.argvalue = getOpenFieldText();
        } else if (name.equals("showmaxmin")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(showMaxMin());
        } else if (name.equals("style")) {
            type.argvalue = "enum:CandleStyle";
            CandleStyle style = getStyle();
            if (style == CandleStyle.American) {
                value.argvalue = "American";
            } else if (style == CandleStyle.CloseLine) {
                value.argvalue = "CloseLine";
            } else if (style == CandleStyle.Tower) {
                value.argvalue = "Tower";
            } else {
                value.argvalue = "Rect";
            }
        } else if (name.equals("stylefield")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getStyleField());
        } else if (name.equals("tagcolor")) {
            type.argvalue = "double";
            value.argvalue = FCStr.convertDoubleToStr(getTagColor());
        } else if (name.equals("upcolor")) {
            type.argvalue = "color";
            value.argvalue = FCStr.convertDoubleToStr(getUpColor());
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
        propertyNames.addAll(Arrays.asList(new String[]{"CloseField", "ColorField", "CloseFieldText", "DownColor", "DownColor", "HighFieldText", "LowField", "LowFieldText", "OpenField", "OpenFieldText", "ShowMaxMin", "Style", "StyleField", "TagColor", "UpColor"}));
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
     * 获取属性值
     *
     * @param name 属性名称
     * @param value 属性值
     */
     @Override
    public void setProperty(String name, String value) {
        if (name.equals("closefield")) {
            setCloseField(FCStr.convertStrToInt(value));
        } else if (name.equals("colorfield")) {
            setColorField(FCStr.convertStrToInt(value));
        } else if (name.equals("closefieldtext")) {
            setCloseFieldText(value);
        } else if (name.equals("downcolor")) {
            setDownColor(FCStr.convertStrToColor(value));
        } else if (name.equals("highfield")) {
            setHighField(FCStr.convertStrToInt(value));
        } else if (name.equals("highfieldtext")) {
            setHighFieldText(value);
        } else if (name.equals("lowfield")) {
            setLowField(FCStr.convertStrToInt(value));
        } else if (name.equals("lowfieldtext")) {
            setLowFieldText(value);
        } else if (name.equals("openfield")) {
            setOpenField(FCStr.convertStrToInt(value));
        } else if (name.equals("openfieldtext")) {
            setOpenFieldText(value);
        } else if (name.equals("showmaxmin")) {
            setShowMaxMin(FCStr.convertStrToBool(value));
        } else if (name.equals("style")) {
            value = value.toLowerCase();
            if (value.equals("american")) {
                setStyle(CandleStyle.American);
            } else if (value.equals("closeline")) {
                setStyle(CandleStyle.CloseLine);
            } else if (value.equals("tower")) {
                setStyle(CandleStyle.Tower);
            } else {
                setStyle(CandleStyle.Rect);
            }
        } else if (name.equals("stylefield")) {
            setStyleField(FCStr.convertStrToInt(value));
        } else if (name.equals("tagcolor")) {
            setTagColor(FCStr.convertStrToColor(value));
        } else if (name.equals("upcolor")) {
            setUpColor(FCStr.convertStrToColor(value));
        } else {
            super.setProperty(name, value);
        }
    }
}
