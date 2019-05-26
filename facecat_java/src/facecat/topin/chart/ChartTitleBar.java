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
 * 图层标题栏
 */
public class ChartTitleBar implements FCProperty {

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

    protected int m_height = 22;

    /**
     * 获取标题高度
     */
    public int getHeight() {
        return m_height;
    }

    /**
     * 设置标题高度
     */
    public void setHeight(int value) {
        m_height = value;
    }

    protected boolean m_isDeleted = false;

    /**
     * 获取或设置是否已被销毁
     */
    public boolean isDeleted() {
        return m_isDeleted;
    }

    protected int m_maxLine = 3;

    /**
     * 获取最大标题行数
     */
    public int getMaxLine() {
        return m_maxLine;
    }

    /**
     * 设置最大标题行数
     */
    public void setMaxLine(int value) {
        m_maxLine = value;
    }

    protected boolean m_showUnderLine = true;

    /**
     * 获取是否显示标题下面的线
     */
    public boolean showUnderLine() {
        return m_showUnderLine;
    }

    /**
     * 设置是否显示标题下面的线
     */
    public void setShowUnderLine(boolean value) {
        m_showUnderLine = value;
    }

    protected String m_text = "";

    /**
     * 获取层的标题
     */
    public String getText() {
        return m_text;
    }

    /**
     * 设置层的标题
     */
    public void setText(String value) {
        m_text = value;
    }

    protected long m_textColor = FCColor.argb(255, 255, 255);

    /**
     * 获取标题的文字颜色
     */
    public long getTextColor() {
        return m_textColor;
    }

    /**
     * 设置标题的文字颜色
     */
    public void setTextColor(long value) {
        m_textColor = value;
    }

    protected ArrayList<ChartTitle> m_titles = new ArrayList<ChartTitle>();

    /**
     * 获取标题
     */
    public ArrayList<ChartTitle> getTitles() {
        return m_titles;
    }

    /**
     * 设置标题
     */
    public void setTitles(ArrayList<ChartTitle> value) {
        m_titles = value;
    }

    protected long m_underLineColor = FCColor.argb(80, 0, 0);

    /**
     * 获取是否显示标题下面的线的颜色
     */
    public long getUnderLineColor() {
        return m_underLineColor;
    }

    /**
     * 设置是否显示标题下面的线的颜色
     */
    public void setUnderLineColor(long value) {
        m_underLineColor = value;
    }

    protected boolean m_visible = true;

    /**
     * 获取是否显示标题
     */
    public boolean isVisible() {
        return m_visible;
    }

    /**
     * 设置是否显示标题
     */
    public void setVisible(boolean value) {
        m_visible = value;
    }

    /**
     * 销毁资源
     */
    public void delete() {
        m_isDeleted = true;
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
        } else if (name.equals("height")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getHeight());
        } else if (name.equals("maxline")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getMaxLine());
        } else if (name.equals("showunderline")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(showUnderLine());
        } else if (name.equals("text")) {
            type.argvalue = "string";
            value.argvalue = getText();
        } else if (name.equals("textcolor")) {
            type.argvalue = "color";
            value.argvalue = FCStr.convertColorToStr(getTextColor());
        } else if (name.equals("underlinecolor")) {
            type.argvalue = "color";
            value.argvalue = FCStr.convertColorToStr(getUnderLineColor());
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
        propertyNames.addAll(Arrays.asList(new String[]{"allowUserPaint", "Font", "Height", "MaxLine", "ShowUnderLine", "Text", "TextColor", "UnderLineColor", "Visible"}));
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
        } else if (name.equals("font")) {
            setFont(FCStr.convertStrToFont(value));
        } else if (name.equals("textcolor")) {
            setTextColor(FCStr.convertStrToColor(value));
        } else if (name.equals("height")) {
            setHeight(FCStr.convertStrToInt(value));
        } else if (name.equals("maxline")) {
            setMaxLine(FCStr.convertStrToInt(value));
        } else if (name.equals("showunderline")) {
            setShowUnderLine(FCStr.convertStrToBool(value));
        } else if (name.equals("text")) {
            setText(value);
        } else if (name.equals("underlinecolor")) {
            setUnderLineColor(FCStr.convertStrToColor(value));
        } else if (name.equals("visible")) {
            setVisible(FCStr.convertStrToBool(value));
        }
    }
}
