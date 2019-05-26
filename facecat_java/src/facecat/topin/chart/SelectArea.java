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
 * 选中区域
 */
public class SelectArea implements FCProperty {

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

    protected long m_backColor = FCColor.None;

    /**
     * 获取背景色
     */
    public long getBackColor() {
        return m_backColor;
    }

    /**
     * 设置背景色
     */
    public void setBackColor(long value) {
        m_backColor = value;
    }

    protected FCRect m_bounds = new FCRect();

    /**
     * 获取选中框的区域
     */
    public FCRect getBounds() {
        return m_bounds.clone();
    }

    /**
     * 设置选中框的区域
     */
    public void setBounds(FCRect value) {
        m_bounds = value.clone();
    }

    protected boolean m_canResize = false;

    /**
     * 获取是否可以改变选中框的大小
     */
    public boolean canresize() {
        return m_canResize;
    }

    /**
     * 设置是否可以改变选中框的大小
     */
    public void setcanresize(boolean value) {
        m_canResize = value;
    }

    protected boolean m_enabled = true;

    /**
     * 获取是否可以出现选中框
     */
    public boolean isEnabled() {
        return m_enabled;
    }

    /**
     * 设置是否可以出现选中框
     */
    public void setEnabled(boolean value) {
        m_enabled = value;
    }

    protected boolean m_isDeleted = false;

    /**
     * 获取或设置是否已被销毁
     */
    public boolean isDeleted() {
        return m_isDeleted;
    }

    protected long m_lineColor = FCColor.argb(255, 255, 255);

    /**
     * 获取选中框的颜色
     */
    public long getLineColor() {
        return m_lineColor;
    }

    /**
     * 设置选中框的颜色
     */
    public void setLineColor(long value) {
        m_lineColor = value;
    }

    protected boolean m_visible;

    /**
     * 获取是否显示选中框
     */
    public boolean isVisible() {
        return m_visible;
    }

    /**
     * 设置是否显示选中框
     */
    public void setVisible(boolean value) {
        m_visible = value;
    }

    /**
     * 关闭
     */
    public void close() {
        m_visible = false;
        m_canResize = false;
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
        } else if (name.equals("enabled")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(isEnabled());
        } else if (name.equals("linecolor")) {
            type.argvalue = "color";
            value.argvalue = FCStr.convertColorToStr(getLineColor());
        }
    }

    /**
     * 获取属性名称列表
     */
    public ArrayList<String> getPropertyNames() {
        ArrayList<String> propertyNames = new ArrayList<String>();
        propertyNames.addAll(Arrays.asList(new String[]{"allowUserPaint", "Enabled", "LineColor"}));
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
        } else if (name.equals("enabled")) {
            setEnabled(FCStr.convertStrToBool(value));
        } else if (name.equals("linecolor")) {
            setLineColor(FCStr.convertStrToColor(value));
        }
    }
}
