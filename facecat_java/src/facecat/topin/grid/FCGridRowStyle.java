/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.grid;

import facecat.topin.core.*;

/**
 * 表格行的样式
 */
public class FCGridRowStyle {

    protected long m_backColor = FCColor.Back;

    /**
     * 获取
     */
    public long getBackColor() {
        return m_backColor;
    }

    public void setBackColor(long value) {
        m_backColor = value;
    }

    protected FCFont m_font = new FCFont("Simsun", 14, false, false, false);

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

    protected long m_hoveredBackColor = FCColor.argb(150, 150, 150);

    /**
     * 获取触摸悬停行的背景色
     */
    public long getHoveredBackColor() {
        return m_hoveredBackColor;
    }

    /**
     * 设置触摸悬停行的背景色
     */
    public void setHoveredBackColor(long value) {
        m_hoveredBackColor = value;
    }

    protected long m_hoveredTextColor = FCColor.Text;

    /**
     * 获取触摸悬停行的前景色
     */
    public long getHoveredTextColor() {
        return m_hoveredTextColor;
    }

    /**
     * 设置触摸悬停行的前景色
     */
    public void setHoveredTextColor(long value) {
        m_hoveredTextColor = value;
    }

    protected long m_selectedBackColor = FCColor.argb(100, 100, 100);

    /**
     * 获取选中行的背景色
     */
    public long getSelectedBackColor() {
        return m_selectedBackColor;
    }

    /**
     * 设置选中行的背景色
     */
    public void setSelectedBackColor(long value) {
        m_selectedBackColor = value;
    }

    protected long m_selectedTextColor = FCColor.Text;

    /**
     * 获取选中行的前景色
     */
    public long getSelectedTextColor() {
        return m_selectedTextColor;
    }

    /**
     * 设置选中行的前景色
     */
    public void setSelectedTextColor(long value) {
        m_selectedTextColor = value;
    }

    protected long m_textColor = FCColor.Text;

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
}
