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
 * Y轴
 */
public class VScale implements FCProperty {

    /**
     * 析构函数
     */
    protected void finalize() throws Throwable {
        delete();
    }

    protected boolean m_allowUserPaint = false;

    /**
     * 获取是否允许用户绘图
     */
    public boolean allowUserPaint() {
        return m_allowUserPaint;
    }

    /**
     * 设置是否允许用户绘图
     */
    public void setAllowUserPaint(boolean allowUserPaint) {
        m_allowUserPaint = allowUserPaint;
    }

    protected ArrayList<Double> m_scaleSteps = new ArrayList<Double>();

    protected boolean m_autoMaxMin = true;

    /**
     * 获取最大值和最小值是否自动计算
     */
    public boolean autoMaxMin() {
        return m_autoMaxMin;
    }

    /**
     * 设置最大值和最小值是否自动计算
     */
    public void setAutoMaxMin(boolean value) {
        m_autoMaxMin = value;
    }

    protected int m_baseField = FCDataTable.NULLFIELD;

    /**
     * 获取基础字段
     */
    public int getBaseField() {
        return m_baseField;
    }

    /**
     * 设置基础字段
     */
    public void setBaseField(int value) {
        m_baseField = value;
    }

    protected CrossLineTip m_crossLineTip = new CrossLineTip();

    /**
     * 获取十字线标签
     */
    public CrossLineTip getCrossLineTip() {
        return m_crossLineTip;
    }

    /**
     * 设置十字线标签
     */
    public void setCrossLineTip(CrossLineTip value) {
        m_crossLineTip = value;
    }

    protected int m_digit = 2;

    /**
     * 获取面板显示数值保留小数的位数
     */
    public int getDigit() {
        return m_digit;
    }

    /**
     * 设置面板显示数值保留小数的位数
     */
    public void setDigit(int value) {
        m_digit = value;
    }

    protected FCFont m_font = new FCFont("Arial", 14, true, false, false);

    /**
     * 获取左侧Y轴文字的字体
     */
    public FCFont getFont() {
        return m_font;
    }

    /**
     * 设置左侧Y轴文字的字体
     */
    public void setFont(FCFont value) {
        m_font = value;
    }

    protected boolean m_isDeleted = false;

    /**
     * 获取或设置是否已被销毁
     */
    public boolean isDeleted() {
        return m_isDeleted;
    }

    protected int m_magnitude = 1;

    /**
     * 获取量级
     */
    public int getMagnitude() {
        return m_magnitude;
    }

    /**
     * 设置量级
     */
    public void setMagnitude(int value) {
        m_magnitude = value;
    }

    protected double m_midValue;

    /**
     * 获取区别涨贴的中间值
     */
    public double getMidValue() {
        return m_midValue;
    }

    /**
     * 设置区别涨贴的中间值
     */
    public void setMidValue(double value) {
        m_midValue = value;
    }

    protected NumberStyle m_numberStyle = NumberStyle.Standard;

    /**
     * 获取数字类型
     */
    public NumberStyle getNumberStyle() {
        return m_numberStyle;
    }

    /**
     * 设置数字类型
     */
    public void setNumberStyle(NumberStyle value) {
        m_numberStyle = value;
    }

    protected int m_paddingBottom;

    /**
     * 获取最小值上方的间隙比例
     */
    public int getPaddingBottom() {
        return m_paddingBottom;
    }

    /**
     * 设置最小值上方的间隙比例
     */
    public void setPaddingBottom(int value) {
        m_paddingBottom = value;
    }

    protected int m_paddingTop;

    /**
     * 获取最大值上方的间隙比例
     */
    public int getPaddingTop() {
        return m_paddingTop;
    }

    /**
     * 设置最大值上方的间隙比例
     */
    public void setPaddingTop(int value) {
        m_paddingTop = value;
    }

    protected boolean m_reverse = false;

    /**
     * 获取是否反转
     */
    public boolean isReverse() {
        return m_reverse;
    }

    /**
     * 设置是否反转
     */
    public void setreverse(boolean value) {
        m_reverse = value;
    }

    protected long m_scaleColor = FCColor.argb(150, 0, 0);

    /**
     * 获取坐标轴的画笔
     */
    public long getScaleColor() {
        return m_scaleColor;
    }

    /**
     * 设置坐标轴的画笔
     */
    public void setScaleColor(long value) {
        m_scaleColor = value;
    }

    protected VScaleSystem m_system = VScaleSystem.Standard;

    /**
     * 获取坐标系的类型
     */
    public VScaleSystem getSystem() {
        return m_system;
    }

    /**
     * 设置坐标系的类型
     */
    public void setSystem(VScaleSystem value) {
        m_system = value;
    }

    protected long m_textColor = FCColor.argb(255, 82, 82);

    /**
     * 获取Y轴文字的颜色
     */
    public long getTextColor() {
        return m_textColor;
    }

    /**
     * 设置Y轴文字的颜色
     */
    public void setTextColor(long value) {
        m_textColor = value;
    }

    protected long m_textColor2 = FCColor.None;

    /**
     * 获取Y轴文字的颜色2
     */
    public long getTextColor2() {
        return m_textColor2;
    }

    /**
     * 设置Y轴文字的颜色2
     */
    public void setTextColor2(long value) {
        m_textColor2 = value;
    }

    protected VScaleType m_type = VScaleType.EqualDiff;

    /**
     * 获取坐标轴的类型
     */
    public VScaleType getType() {
        return m_type;
    }

    /**
     * 设置坐标轴的类型
     */
    public void setType(VScaleType value) {
        m_type = value;
    }

    protected double visibleMax;

    /**
     * 获取坐标值可见部分的最大值
     */
    public double getVisibleMax() {
        return visibleMax;
    }

    /**
     * 设置坐标值可见部分的最大值
     */
    public void setVisibleMax(double value) {
        visibleMax = value;
    }

    protected double m_visibleMin;

    /**
     * 获取坐标值可见部分的最小值
     */
    public double getVisibleMin() {
        return m_visibleMin;
    }

    /**
     * 设置坐标值可见部分的最小值
     */
    public void setVisibleMin(double value) {
        m_visibleMin = value;
    }

    /**
     * 销毁资源
     */
    public void delete() {
        if (!m_isDeleted) {
            if (m_crossLineTip != null) {
                m_crossLineTip.delete();
                m_crossLineTip = null;
            }
            m_scaleSteps.clear();
            m_isDeleted = true;
        }
    }

    /**
     * 获取属性值
     *
     * @param name 属性名称
     * @param value 返回属性值
     * @param type 返回属性类型
     */
    public void getProperty(String name, RefObject<String> value, RefObject<String> type) {
        if (name.equals("allowUserPaint")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(allowUserPaint());
        } else if (name.equals("automaxmin")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(autoMaxMin());
        } else if (name.equals("basefield")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getBaseField());
        } else if (name.equals("digit")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getDigit());
        } else if (name.equals("font")) {
            type.argvalue = "font";
            value.argvalue = FCStr.convertFontToStr(getFont());
        } else if (name.equals("textcolor")) {
            type.argvalue = "color";
            value.argvalue = FCStr.convertColorToStr(getTextColor());
        } else if (name.equals("textcolor2")) {
            type.argvalue = "color";
            value.argvalue = FCStr.convertColorToStr(getTextColor2());
        } else if (name.equals("magnitude")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getMagnitude());
        } else if (name.equals("midvalue")) {
            type.argvalue = "double";
            value.argvalue = FCStr.convertDoubleToStr(getMidValue());
        } else if (name.equals("numberstyle")) {
            type.argvalue = "enum:NumberStyle";
            NumberStyle style = getNumberStyle();
            if (style == NumberStyle.Standard) {
                value.argvalue = "Standard";
            } else {
                value.argvalue = "UnderLine";
            }
        } else if (name.equals("paddingbottom")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getPaddingBottom());
        } else if (name.equals("paddingtop")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getPaddingTop());
        } else if (name.equals("reverse")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(isReverse());
        } else if (name.equals("scalecolor")) {
            type.argvalue = "color";
            value.argvalue = FCStr.convertColorToStr(getScaleColor());
        } else if (name.equals("system")) {
            type.argvalue = "enum:VScaleSystem";
            VScaleSystem system = getSystem();
            if (system == VScaleSystem.Logarithmic) {
                value.argvalue = "Log";
            } else {
                value.argvalue = "Standard";
            }
        } else if (name.equals("type")) {
            type.argvalue = "enum:VScaleType";
            VScaleType vScaleType = getType();
            if (vScaleType == VScaleType.Divide) {
                value.argvalue = "Divide";
            } else if (vScaleType == VScaleType.EqualDiff) {
                value.argvalue = "EqualDiff";
            } else if (vScaleType == VScaleType.EqualRatio) {
                value.argvalue = "EqualRatio";
            } else if (vScaleType == VScaleType.GoldenRatio) {
                value.argvalue = "GoldenRatio";
            } else {
                value.argvalue = "Percent";
            }
        } else if (name.equals("visiblemax")) {
            type.argvalue = "double";
            value.argvalue = FCStr.convertDoubleToStr(getVisibleMax());
        } else if (name.equals("visiblemin")) {
            type.argvalue = "double";
            value.argvalue = FCStr.convertDoubleToStr(getVisibleMin());
        }
    }

    /**
     * 获取属性名称列表
     */
    public ArrayList<String> getPropertyNames() {
        ArrayList<String> propertyNames = new ArrayList<String>();
        propertyNames.addAll(Arrays.asList(new String[]{"allowUserPaint", "AutoMaxMin", "BaseField", "Digit", "Font", "Magnitude", "MidValue", "NumberStyle", "PaddingBottom", "PaddingTop", "Reverse", "ScaleColor", "System", "TextColor", "TextColor2", "Type", "VisibleMax", "VisibleMin"}));
        return propertyNames;
    }

    /**
     * 重绘方法
     *
     * @param paint 绘图对象
     * @param div 图层
     * @param rect 区域
     */
    public void onPaint(FCPaint paint, ChartDiv div, FCRect rect) {

    }

    /**
     * 设置属性值
     *
     * @param name 属性名称
     * @param value 属性值
     */
    public void setProperty(String name, String value) {
        if (name.equals("allowUserPaint")) {
            setAllowUserPaint(FCStr.convertStrToBool(value));
        } else if (name.equals("automaxmin")) {
            setAutoMaxMin(FCStr.convertStrToBool(value));
        } else if (name.equals("basefield")) {
            setBaseField(FCStr.convertStrToInt(value));
        } else if (name.equals("digit")) {
            setDigit(FCStr.convertStrToInt(value));
        } else if (name.equals("font")) {
            setFont(FCStr.convertStrToFont(value));
        } else if (name.equals("textcolor")) {
            setTextColor(FCStr.convertStrToColor(value));
        } else if (name.equals("textcolor2")) {
            setTextColor2(FCStr.convertStrToColor(value));
        } else if (name.equals("magnitude")) {
            setMagnitude(FCStr.convertStrToInt(value));
        } else if (name.equals("midvalue")) {
            setMidValue(FCStr.convertStrToDouble(value));
        } else if (name.equals("numberstyle")) {
            value = value.toLowerCase();
            if (value.equals("standard")) {
                setNumberStyle(NumberStyle.Standard);
            } else {
                setNumberStyle(NumberStyle.UnderLine);
            }
        } else if (name.equals("paddingbottom")) {
            setPaddingBottom(FCStr.convertStrToInt(value));
        } else if (name.equals("paddingtop")) {
            setPaddingTop(FCStr.convertStrToInt(value));
        } else if (name.equals("reverse")) {
            setreverse(FCStr.convertStrToBool(value));
        } else if (name.equals("scalecolor")) {
            setScaleColor(FCStr.convertStrToColor(value));
        } else if (name.equals("system")) {
            if (value.equals("log")) {
                setSystem(VScaleSystem.Logarithmic);
            } else {
                setSystem(VScaleSystem.Standard);
            }
        } else if (name.equals("type")) {
            if (value.equals("Divide")) {
                setType(VScaleType.Divide);
            } else if (value.equals("equaldiff")) {
                setType(VScaleType.EqualDiff);
            } else if (value.equals("equalratio")) {
                setType(VScaleType.EqualRatio);
            } else if (value.equals("goldenratio")) {
                setType(VScaleType.GoldenRatio);
            } else {
                setType(VScaleType.Percent);
            }
        } else if (name.equals("visiblemax")) {
            setVisibleMax(FCStr.convertStrToDouble(value));
        } else if (name.equals("visiblemin")) {
            setVisibleMin(FCStr.convertStrToDouble(value));
        }
    }

    /**
     * 获取属性名称列表
     */
    public ArrayList<Double> getScaleSteps() {
        return m_scaleSteps;
    }

    /**
     * 设置刻度点
     */
    public void setScaleSteps(ArrayList<Double> scaleSteps) {
        m_scaleSteps = scaleSteps;
    }
}
