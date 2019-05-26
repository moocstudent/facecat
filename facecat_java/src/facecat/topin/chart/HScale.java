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
 * 横轴
 */
public class HScale implements FCProperty {

    /**
     * 创建横轴
     */
    public HScale() {
        // 设置日期的自定义颜色
        m_dateColors.put(DateType.Year, FCColor.argb(255, 255, 255));
        m_dateColors.put(DateType.Month, FCColor.argb(150, 0, 0));
        m_dateColors.put(DateType.Day, FCColor.argb(100, 100, 100));
        m_dateColors.put(DateType.Hour, FCColor.argb(82, 82, 255));
        m_dateColors.put(DateType.Minute, FCColor.argb(255, 255, 0));
        m_dateColors.put(DateType.Second, FCColor.argb(255, 0, 255));
        m_dateColors.put(DateType.Millisecond, FCColor.argb(255, 0, 255));
    }

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

    /**
     * 日期文字的颜色
     */
    protected java.util.HashMap<DateType, Long> m_dateColors = new java.util.HashMap<DateType, Long>();

    /**
     * 刻度点
     */
    protected ArrayList<Double> m_scaleSteps = new ArrayList<Double>();

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

    protected FCFont m_font = new FCFont();

    /**
     * 获取X轴文字的字体
     */
    public FCFont getFont() {
        return m_font;
    }

    /**
     * 设置X轴文字的字体
     */
    public void setFont(FCFont value) {
        m_font = value;
    }

    protected int m_height = 25;

    /**
     * 获取X轴的高度
     */
    public int getHeight() {
        return m_height;
    }

    /**
     * 设置X轴的高度
     */
    public void setHeight(int value) {
        m_height = value;
    }

    protected HScaleType m_hScaleType = HScaleType.Date;

    /**
     * 获取横轴的数据类型
     */
    public HScaleType getHScaleType() {
        return m_hScaleType;
    }

    /**
     * 设置横轴的数据类型
     */
    public void setHScaleType(HScaleType value) {
        m_hScaleType = value;
    }

    protected int m_interval = 60;

    /**
     * 获取日期文字间隔
     */
    public int getInterval() {
        return m_interval;
    }

    /**
     * 设置日期文字间隔
     */
    public void setInterval(int value) {
        m_interval = value;
    }

    protected boolean m_isDeleted = false;

    /**
     * 获取或设置是否已被销毁
     */
    public boolean isDeleted() {
        return m_isDeleted;
    }

    protected long m_scaleColor = FCColor.argb(150, 0, 0);

    /**
     * 获取X轴的线条颜色
     */
    public long getScaleColor() {
        return m_scaleColor;
    }

    /**
     * 设置X轴的线条颜色
     */
    public void setScaleColor(long value) {
        m_scaleColor = value;
    }

    protected long m_textColor = FCColor.argb(255, 255, 255);

    /**
     * 获取文本颜色
     */
    public long getTextColor() {
        return m_textColor;
    }

    /**
     * 设置文本颜色
     */
    public void setTextColor(long value) {
        m_textColor = value;
    }

    protected boolean m_visible = true;

    /**
     * 获取显示X轴
     */
    public boolean isVisible() {
        return m_visible;
    }

    /**
     * 设置显示X轴
     */
    public void setVisible(boolean value) {
        m_visible = value;
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
            if (m_dateColors != null) {
                m_dateColors.clear();
                m_dateColors = null;
            }
            m_isDeleted = true;
        }
    }

    /**
     *  获取日期文字的颜色
     */
    public long getDateColor(DateType dateType) {
        return m_dateColors.get(dateType);
    }

    /**
     * 设置日期文字的颜色
     */
    public void setDateColor(DateType dateType, long color) {
        m_dateColors.put(dateType, color);
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
        } else if (name.equals("font")) {
            type.argvalue = "font";
            value.argvalue = FCStr.convertFontToStr(getFont());
        } else if (name.equals("textcolor")) {
            type.argvalue = "color";
            value.argvalue = FCStr.convertColorToStr(getTextColor());
        } else if (name.equals("height")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getHeight());
        } else if (name.equals("type")) {
            type.argvalue = "enum:HScaleType";
            HScaleType hScaleType = getHScaleType();
            if (hScaleType == HScaleType.Date) {
                value.argvalue = "Date";
            } else {
                value.argvalue = "Number";
            }
        } else if (name.equals("interval")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getInterval());
        } else if (name.equals("scalecolor")) {
            type.argvalue = "color";
            value.argvalue = FCStr.convertColorToStr(getScaleColor());
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
        propertyNames.addAll(Arrays.asList(new String[]{"allowUserPaint", "Font", "Height", "Type", "Interval", "ScaleColor", "TextColor", "Visible"}));
        return propertyNames;
    }

    /**
     * 重绘方法
     */
    public void onPaint(FCPaint paint, ChartDiv div, FCRect rect) {

    }

    /**
     * 设置属性
     */
    public void setProperty(String name, String value) {
        if (name.equals("allowUserPaint")) {
            setAllowUserPaint(FCStr.convertStrToBool(value));
        } else if (name.equals("font")) {
            setFont(FCStr.convertStrToFont(value));
        } else if (name.equals("textcolor")) {
            setTextColor(FCStr.convertStrToColor(value));
        } else if (name.equals("height")) {
            setHeight(FCStr.convertStrToInt(value));
        } else if (name.equals("type")) {
            value = value.toLowerCase();
            if (value.equals("date")) {
                setHScaleType(HScaleType.Date);
            } else {
                setHScaleType(HScaleType.Number);
            }
        } else if (name.equals("interval")) {
            setInterval(FCStr.convertStrToInt(value));
        } else if (name.equals("scalecolor")) {
            setScaleColor(FCStr.convertStrToColor(value));
        } else if (name.equals("visible")) {
            setVisible(FCStr.convertStrToBool(value));
        }
    }

    /**
     * 获取刻度点
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
