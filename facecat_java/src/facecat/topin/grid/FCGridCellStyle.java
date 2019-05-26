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
 * 单元格样式
 */
public class FCGridCellStyle {

    protected FCHorizontalAlign m_align = FCHorizontalAlign.Inherit;

    /**
     * 获取内容的横向排列样式
     */
    public FCHorizontalAlign getAlign() {
        return m_align;
    }

    /**
     * 设置内容的横向排列样式
     */
    public void setAlign(FCHorizontalAlign value) {
        m_align = value;
    }

    protected boolean m_autoEllipsis;

    /**
     * 获取是否在文字超出范围时在结尾显示省略号
     */
    public boolean autoEllipsis() {
        return m_autoEllipsis;
    }

    /**
     * 设置是否在文字超出范围时在结尾显示省略号
     */
    public void setAutoEllipsis(boolean value) {
        m_autoEllipsis = value;
    }

    protected long m_backColor = FCColor.None;

    /**
     * 获取
     */
    public long getBackColor() {
        return m_backColor;
    }

    public void setBackColor(long value) {
        m_backColor = value;
    }

    protected FCFont m_font;

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

    protected long m_textColor = FCColor.None;

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
