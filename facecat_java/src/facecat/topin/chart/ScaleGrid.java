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
 * 网格线
 */
public class ScaleGrid implements FCProperty {

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

    protected int m_distance = 30;

    /**
     * 获取距离
     */
    public int getDistance() {
        return m_distance;
    }

    /**
     * 设置距离
     */
    public void setDistance(int value) {
        m_distance = value;
    }

    protected long m_gridColor = FCColor.argb(80, 0, 0);

    /**
     * 获取网格线的颜色
     */
    public long getGridColor() {
        return m_gridColor;
    }

    /**
     * 设置网格线的颜色
     */
    public void setGridColor(long value) {
        m_gridColor = value;
    }

    protected int m_lineStyle = 2;

    /**
     * 获取获取或设置横向网格线的样式
     */
    public int getLineStyle() {
        return m_lineStyle;
    }

    /**
     * 设置获取或设置横向网格线的样式
     */
    public void setLineStyle(int value) {
        m_lineStyle = value;
    }

    protected boolean m_isDeleted = false;

    /**
     * 获取或设置是否已被销毁
     */
    public boolean isDeleted() {
        return m_isDeleted;
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
        } else if (name.equals("distance")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getDistance());
        } else if (name.equals("gridcolor")) {
            type.argvalue = "color";
            value.argvalue = FCStr.convertColorToStr(getGridColor());
        } else if (name.equals("linestyle")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getLineStyle());
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
        propertyNames.addAll(Arrays.asList(new String[]{"allowUserPaint", "Distance", "GridColor", "LineStyle", "Visible"}));
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
        } else if (name.equals("distance")) {
            setDistance(FCStr.convertStrToInt(value));
        } else if (name.equals("gridcolor")) {
            setGridColor(FCStr.convertStrToColor(value));
        } else if (name.equals("linestyle")) {
            setLineStyle(FCStr.convertStrToInt(value));
        } else if (name.equals("visible")) {
            setVisible(FCStr.convertStrToBool(value));
        }
    }
}
