/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.label;

import facecat.topin.core.*;
import java.util.*;

/**
 * 超链接控件
 */
public class FCLinkLabel extends FCLabel {

    /**
     * 创建超链接控件
     */
    public FCLinkLabel() {
    }

    /**
     * 是否已访问
     */
    protected boolean m_visited = false;

    protected long m_activeLinkColor = FCColor.argb(255, 0, 0);

    /**
     * 获取单击超链接时的颜色
     */
    public final long getActiveLinkColor() {
        return m_activeLinkColor;
    }

    /**
     * 设置单击超链接时的颜色
     */
    public final void setActiveLinkColor(long value) {
        m_activeLinkColor = value;
    }

    protected long m_disabledLinkColor = FCColor.argb(133, 133, 133);

    /**
     * 获取超链接被禁用时的颜色
     */
    public final long getDisabledLinkColor() {
        return m_disabledLinkColor;
    }

    /**
     * 设置超链接被禁用时的颜色
     */
    public final void setDisabledLinkColor(long value) {
        m_disabledLinkColor = value;
    }

    protected FCLinkBehavior m_linkBehavior = FCLinkBehavior.AlwaysUnderLine;

    /**
     * 获取下划线的显示方式
     */
    public final FCLinkBehavior getLinkBehavior() {
        return m_linkBehavior;
    }

    /**
     * 设置下划线的显示方式
     */
    public final void setLinkBehavior(FCLinkBehavior value) {
        m_linkBehavior = value;
    }

    protected long m_linkColor = FCColor.argb(0, 0, 255);

    /**
     * 获取超链接处于默认时的颜色
     */
    public final long getLinkColor() {
        return m_linkColor;
    }

    /**
     * 设置超链接处于默认时的颜色
     */
    public final void setLinkColor(long value) {
        m_linkColor = value;
    }

    protected boolean m_linkVisited = false;

    /**
     * 获取是否按照已访问的样式显示超链接颜色
     */
    public final boolean isLinkVisited() {
        return m_linkVisited;
    }

    /**
     * 设置是否按照已访问的样式显示超链接颜色
     */
    public final void setLinkVisited(boolean value) {
        m_linkVisited = value;
    }

    protected long m_visitedLinkColor = FCColor.argb(128, 0, 128);

    /**
     * 获取已访问的超链接的颜色
     */
    public final long getVisitedLinkColor() {
        return m_visitedLinkColor;
    }

    /**
     * 设置已访问的超链接的颜色
     */
    public final void setVisitedLinkColor(long value) {
        m_visitedLinkColor = value;
    }

    /**
     * 获取控件类型
     */
    @Override
    public String getControlType() {
        return "LinkLabel";
    }

    /**
     * 获取要绘制的超链接颜色
     */
    protected long getPaintingLinkColor() {
        if (isEnabled()) {
            FCNative inative = getNative();
            if (this == inative.getHoveredControl()) {
                return m_activeLinkColor;
            } else if (this == inative.getPushedControl()) {
                return m_activeLinkColor;
            } else {
                if (m_linkVisited && m_visited) {
                    return m_visitedLinkColor;
                } else {
                    return m_linkColor;
                }
            }
        } else {
            return m_disabledLinkColor;
        }
    }

    /**
     * 获取属性值
     *
     * @param name 属性名称
     * @param value 返回属性值
     * @param type 返回属性类型
     */
    @Override
    public void getProperty(String name, RefObject<String> value, RefObject<String> type) {
        if (name.equals("activelinkcolor")) {
            type.argvalue = "color";
            value.argvalue = FCStr.convertColorToStr(getActiveLinkColor());
        } else if (name.equals("disabledlinkcolor")) {
            type.argvalue = "color";
            value.argvalue = FCStr.convertColorToStr(getDisabledLinkColor());
        } else if (name.equals("linkbehavior")) {
            type.argvalue = "enum:FCLinkBehavior";
            FCLinkBehavior linkBehavior = getLinkBehavior();
            if (linkBehavior == FCLinkBehavior.AlwaysUnderLine) {
                value.argvalue = "AlwaysUnderLine";
            } else if (linkBehavior == FCLinkBehavior.HoverUnderLine) {
                value.argvalue = "HoverUnderLine";
            } else {
                value.argvalue = "NeverUnderLine";
            }
        } else if (name.equals("linkcolor")) {
            type.argvalue = "color";
            value.argvalue = FCStr.convertColorToStr(getLinkColor());
        } else if (name.equals("linkvisited")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(isLinkVisited());
        } else if (name.equals("visitedlinkcolor")) {
            type.argvalue = "color";
            value.argvalue = FCStr.convertColorToStr(getVisitedLinkColor());
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
        propertyNames.addAll(Arrays.asList(new String[]{"ActiveLinkColor", "DisabledLinkColor", "LinkBehavior", "LinkColor", "LinkVisited", "VisitedLinkColor"}));
        return propertyNames;
    }

    /**
     * 触摸点击方法
     *
     * @param touchInfo 触摸信息
     */
    @Override
    public void onClick(FCTouchInfo touchInfo) {
        super.onClick(touchInfo.clone());
        if (m_linkVisited) {
            m_visited = true;
        }
    }

    /**
     * 触摸按下方法
     *
     * @param touchInfo 触摸信息
     */
    @Override
    public void onTouchDown(FCTouchInfo touchInfo) {
        super.onTouchDown(touchInfo.clone());
        invalidate();
    }

    /**
     * 触摸进入方法
     *
     * @param touchInfo 触摸信息
     */
    @Override
    public void onTouchEnter(FCTouchInfo touchInfo) {
        super.onTouchEnter(touchInfo.clone());
        invalidate();
    }

    /**
     * 触摸离开方法
     *
     * @param touchInfo 触摸信息
     */
    @Override
    public void onTouchLeave(FCTouchInfo touchInfo) {
        super.onTouchLeave(touchInfo.clone());
        invalidate();
    }

    /**
     * 触摸抬起方法
     *
     * @param touchInfo 触摸信息
     */
    @Override
    public void onTouchUp(FCTouchInfo touchInfo) {
        super.onTouchUp(touchInfo.clone());
        invalidate();
    }

    /**
     * 重绘前景方法
     *
     * @param paint 绘图对象
     * @param clipRect 裁剪区域
     */
    @Override
    public void onPaintForeground(FCPaint paint, FCRect clipRect) {
        String text = getText();
        // 绘制文字
        if (text != null && text.length() > 0) {
            int width = getWidth(), height = getHeight();
            if (width > 0 && height > 0) {
                FCFont font = getFont();
                FCSize tSize = paint.textSize(text, font);
                long linkColor = getPaintingLinkColor();
                FCPoint tPoint = new FCPoint((width - tSize.cx) / 2, (height - tSize.cy) / 2);
                FCPadding padding = getPadding();
                switch (m_textAlign) {
                    case BottomCenter:
                        tPoint.y = height - tSize.cy;
                        break;
                    case BottomLeft:
                        tPoint.x = padding.left;
                        tPoint.y = height - tSize.cy - padding.bottom;
                        break;
                    case BottomRight:
                        tPoint.x = width - tSize.cx - padding.right;
                        tPoint.y = height - tSize.cy - padding.bottom;
                        break;
                    case MiddleLeft:
                        tPoint.x = padding.left;
                        break;
                    case MiddleRight:
                        tPoint.x = width - tSize.cx - padding.right;
                        break;
                    case TopCenter:
                        tPoint.y = padding.top;
                        break;
                    case TopLeft:
                        tPoint.x = padding.left;
                        tPoint.y = padding.top;
                        break;
                    case TopRight:
                        tPoint.x = width - tSize.cx - padding.right;
                        tPoint.y = padding.top;
                        break;
                }
                FCRect tRect = new FCRect(tPoint.x, tPoint.y, tPoint.x + tSize.cx, tPoint.y + tSize.cy);
                if (autoEllipsis() && (tRect.right > clipRect.right || tRect.bottom > clipRect.bottom)) {
                    if (tRect.right > clipRect.right) {
                        tRect.right = clipRect.right;
                    }
                    if (tRect.bottom > clipRect.bottom) {
                        tRect.bottom = clipRect.bottom;
                    }
                    paint.drawTextAutoEllipsis(text, linkColor, font, tRect);
                } else {
                    paint.drawText(text, linkColor, font, tRect);
                }
                // 画下划线
                FCNative inative = getNative();
                if (m_linkBehavior == FCLinkBehavior.AlwaysUnderLine || (m_linkBehavior == FCLinkBehavior.HoverUnderLine && (this == inative.getPushedControl() || this == inative.getHoveredControl()))) {
                    paint.drawLine(linkColor, 1, 0, tPoint.x, tPoint.y + tSize.cy, tPoint.x + tSize.cx, tPoint.y + tSize.cy);
                }
            }
        }
    }

    /**
     * 设置属性值
     *
     * @param name 属性名称
     * @param value 属性值
     */
    @Override
    public void setProperty(String name, String value) {
        if (name.equals("activelinkcolor")) {
            setActiveLinkColor(FCStr.convertStrToColor(value));
        } else if (name.equals("disabledlinkcolor")) {
            setDisabledLinkColor(FCStr.convertStrToColor(value));
        } else if (name.equals("linkbehavior")) {
            value = value.toLowerCase();
            if (value.equals("alwaysunderline")) {
                setLinkBehavior(FCLinkBehavior.AlwaysUnderLine);
            } else if (value.equals("hoverunderline")) {
                setLinkBehavior(FCLinkBehavior.HoverUnderLine);
            } else {
                setLinkBehavior(FCLinkBehavior.NeverUnderLine);
            }
        } else if (name.equals("linkcolor")) {
            setLinkColor(FCStr.convertStrToColor(value));
        } else if (name.equals("linkvisited")) {
            setLinkVisited(FCStr.convertStrToBool(value));
        } else if (name.equals("visitedlinkcolor")) {
            setVisitedLinkColor(FCStr.convertStrToColor(value));
        } else {
            super.setProperty(name, value);
        }
    }
}
