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
 * 所有线条的父类
 */
public class BaseShape implements FCProperty {

    /**
     * 析构函数
     */
    protected void finalize() throws Throwable {
        delete();
    }

    protected boolean m_allowUserPaint = false;

    public boolean allowUserPaint() {
        return m_allowUserPaint;
    }

    /**
     * 获取或设置是否允许用户绘图
     */
    public void setAllowUserPaint(boolean allowUserPaint) {
        m_allowUserPaint = allowUserPaint;
    }

    protected AttachVScale m_attachVScale = AttachVScale.Left;

    /**
     * 获取依附于左轴还是右轴
     */
    public AttachVScale getAttachVScale() {
        return m_attachVScale;
    }

    /**
     * 设置依附于左轴还是右轴
     */
    public void setAttachVScale(AttachVScale value) {
        m_attachVScale = value;
    }

    protected boolean isDisposed = false;

    /**
     * 获取或设置是否已被销毁
     */
    public boolean isDeleted() {
        return isDisposed;
    }

    protected boolean m_selected = false;

    /**
     *  获取是否被选中
     */
    public boolean isSelected() {
        return m_selected;
    }

    /**
     *  设置是否被选中
     */
    public void setSelected(boolean value) {
        m_selected = value;
    }

    protected boolean m_visible = true;

    /**
     * 获取图形是否可见
     */
    public boolean isVisible() {
        return m_visible;
    }

    /**
     * 设置图形是否可见
     */
    public void setVisible(boolean value) {
        m_visible = value;
    }

    protected int m_zOrder;

    /**
     * 获取绘图顺序
     */
    public int getZOrder() {
        return m_zOrder;
    }

    /**
     * 设置绘图顺序
     */
    public void setZOrder(int value) {
        m_zOrder = value;
    }

    /**
     * 销毁资源的方法
     */
    public void delete() {
        if (!isDisposed) {
            isDisposed = true;
        }
    }

    /**
     * 获取基础字段
     */
    public int getBaseField() {
        return -1;
    }

    /**
     * 由字段名称获取字段标题
     *
     * @param fieldName 字段名称
     * @returns 字段标题
     */
    public String getFieldText(int fieldName) {
        return "";
    }

    /**
     * 获取所有字段
     */
    public int[] getFields() {
        return null;
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
        } else if (name.equals("attachvscale")) {
            type.argvalue = "enum:AttachVScale";
            if (getAttachVScale() == AttachVScale.Left) {
                value.argvalue = "Left";
            } else {
                value.argvalue = "Right";
            }
        } else if (name.equals("selected")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(isSelected());
        } else if (name.equals("visible")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(isVisible());
        } else if (name.equals("zorder")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getZOrder());
        } else {
            type.argvalue = "undefined";
            value.argvalue = "";
        }
    }

    /**
     * 获取属性名称列表
     */
    public ArrayList<String> getPropertyNames() {
        ArrayList<String> propertyNames = new ArrayList<String>();
        propertyNames.addAll(Arrays.asList(new String[]{"allowUserPaint", "AttachVScale", "Selected", "Visible", "ZOrder"}));
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
        } else if (name.equals("attachvscale")) {
            value = value.toLowerCase();
            if (value.equals("left")) {
                setAttachVScale(AttachVScale.Left);
            } else {
                setAttachVScale(AttachVScale.Right);
            }
        } else if (name.equals("selected")) {
            setSelected(FCStr.convertStrToBool(value));
        } else if (name.equals("visible")) {
            setVisible(FCStr.convertStrToBool(value));
        } else if (name.equals("zorder")) {
            setZOrder(FCStr.convertStrToInt(value));
        }
    }

    /**
     * 获取选中点的颜色
     */
    public long getSelectedColor() {
        return 0;
    }
}
