/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

using System;
using System.Collections.Generic;

namespace FaceCat {
    /// <summary>
    /// 超链接控件行为
    /// </summary>
    public enum FCLinkBehavior {
        /// <summary>
        /// 总是显示下划线
        /// </summary>
        AlwaysUnderLine,
        /// <summary>
        /// 触摸悬停时显示下划线
        /// </summary>
        HoverUnderLine,
        /// <summary>
        /// 总是不显示下划线
        /// </summary>
        NeverUnderLine
    }

    /// <summary>
    /// 超链接控件
    /// </summary>
    public partial class FCLinkLabel : FCLabel {
        /// <summary>
        /// 创建超链接控件
        /// </summary>
        public FCLinkLabel() {
            Cursor = FCCursors.Hand;
        }

        /// <summary>
        /// 是否已访问
        /// </summary>
        protected bool m_visited = false;

        protected long m_activeLinkColor = FCColor.argb(255, 0, 0);

        /// <summary>
        /// 获取或设置单击超链接时的颜色
        /// </summary>
        public virtual long ActiveLinkColor {
            get { return m_activeLinkColor; }
            set { m_activeLinkColor = value; }
        }

        protected long m_disabledLinkColor = FCColor.argb(133, 133, 133);

        /// <summary>
        /// 获取或设置超链接被禁用时的颜色
        /// </summary>
        public virtual long DisabledLinkColor {
            get { return m_disabledLinkColor; }
            set { m_disabledLinkColor = value; }
        }

        protected FCLinkBehavior m_linkBehavior = FCLinkBehavior.AlwaysUnderLine;

        /// <summary>
        /// 获取或设置下划线的显示方式
        /// </summary>
        public virtual FCLinkBehavior LinkBehavior {
            get { return m_linkBehavior; }
            set { m_linkBehavior = value; }
        }

        protected long m_linkColor = FCColor.argb(0, 0, 255);

        /// <summary>
        /// 获取或设置超链接处于默认时的颜色
        /// </summary>
        public virtual long LinkColor {
            get { return m_linkColor; }
            set { m_linkColor = value; }
        }

        protected bool m_linkVisited = false;

        /// <summary>
        /// 获取或设置是否按照已访问的样式显示超链接颜色
        /// </summary>
        public virtual bool LinkVisited {
            get { return m_linkVisited; }
            set { m_linkVisited = value; }
        }

        protected long m_visitedLinkColor = FCColor.argb(128, 0, 128);

        /// <summary>
        /// 获取已访问的超链接的颜色
        /// </summary>
        public virtual long VisitedLinkColor {
            get { return m_visitedLinkColor; }
            set { m_visitedLinkColor = value; }
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <returns>控件类型</returns>
        public override String getControlType() {
            return "LinkLabel";
        }

        /// <summary>
        /// 获取要绘制的超链接颜色
        /// </summary>
        /// <returns>前景色</returns>
        protected virtual long getPaintingLinkColor() {
            if (Enabled) {
                FCNative native = Native;
                if (this == native.HoveredControl) {
                    return m_activeLinkColor;
                }
                else if (this == native.PushedControl) {
                    return m_activeLinkColor;
                }
                else {
                    if (m_linkVisited && m_visited) {
                        return m_visitedLinkColor;
                    }
                    else {
                        return m_linkColor;
                    }
                }
            }
            else {
                return m_disabledLinkColor;
            }
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">返回属性值</param>
        /// <param name="type">返回属性类型</param>
        public override void getProperty(String name, ref String value, ref String type) {
            if (name == "activelinkcolor") {
                type = "color";
                value = FCStr.convertColorToStr(ActiveLinkColor);
            }
            else if (name == "disabledlinkcolor") {
                type = "color";
                value = FCStr.convertColorToStr(DisabledLinkColor);
            }
            else if (name == "linkbehavior") {
                type = "enum:FCLinkBehavior";
                FCLinkBehavior linkBehavior = LinkBehavior;
                if (linkBehavior == FCLinkBehavior.AlwaysUnderLine) {
                    value = "AlwaysUnderLine";
                }
                else if (linkBehavior == FCLinkBehavior.HoverUnderLine) {
                    value = "HoverUnderLine";
                }
                else {
                    value = "NeverUnderLine";
                }
            }
            else if (name == "linkcolor") {
                type = "color";
                value = FCStr.convertColorToStr(LinkColor);
            }
            else if (name == "linkvisited") {
                type = "bool";
                value = FCStr.convertBoolToStr(LinkVisited);
            }
            else if (name == "visitedlinkcolor") {
                type = "color";
                value = FCStr.convertColorToStr(VisitedLinkColor);
            }
            else {
                base.getProperty(name, ref value, ref type);
            }
        }

        /// <summary>
        /// 获取属性名称列表
        /// </summary>
        /// <returns>属性名称列表</returns>
        public override ArrayList<String> getPropertyNames() {
            ArrayList<String> propertyNames = base.getPropertyNames();
            propertyNames.AddRange(new String[] { "ActiveLinkColor", "DisabledLinkColor", "LinkBehavior", "LinkColor", "LinkVisited", "VisitedLinkColor" });
            return propertyNames;
        }

        /// <summary>
        /// 触摸点击方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onClick(FCTouchInfo touchInfo) {
            base.onClick(touchInfo);
            if (m_linkVisited) {
                m_visited = true;
            }
        }

        /// <summary>
        /// 触摸按下方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onTouchDown(FCTouchInfo touchInfo) {
            base.onTouchDown(touchInfo);
            invalidate();
        }

        /// <summary>
        /// 触摸进入方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onTouchEnter(FCTouchInfo touchInfo) {
            base.onTouchEnter(touchInfo);
            invalidate();
        }

        /// <summary>
        /// 触摸离开方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onTouchLeave(FCTouchInfo touchInfo) {
            base.onTouchLeave(touchInfo);
            invalidate();
        }

        /// <summary>
        /// 触摸抬起方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onTouchUp(FCTouchInfo touchInfo) {
            base.onTouchUp(touchInfo);
            invalidate();
        }

        /// <summary>
        /// 重绘前景方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void onPaintForeground(FCPaint paint, FCRect clipRect) {
            String text = Text;
            //绘制文字
            if (text != null && text.Length > 0) {
                int width = Width, height = Height;
                if (width > 0 && height > 0) {
                    FCFont font = Font;
                    FCSize tSize = paint.textSize(text, font);
                    long linkColor = getPaintingLinkColor();
                    FCPoint tPoint = new FCPoint((width - tSize.cx) / 2, (height - tSize.cy) / 2);
                    FCPadding padding = Padding;
                    switch (m_textAlign) {
                        case FCContentAlignment.BottomCenter:
                            tPoint.y = height - tSize.cy;
                            break;
                        case FCContentAlignment.BottomLeft:
                            tPoint.x = padding.left;
                            tPoint.y = height - tSize.cy - padding.bottom;
                            break;
                        case FCContentAlignment.BottomRight:
                            tPoint.x = width - tSize.cx - padding.right;
                            tPoint.y = height - tSize.cy - padding.bottom;
                            break;
                        case FCContentAlignment.MiddleLeft:
                            tPoint.x = padding.left;
                            break;
                        case FCContentAlignment.MiddleRight:
                            tPoint.x = width - tSize.cx - padding.right;
                            break;
                        case FCContentAlignment.TopCenter:
                            tPoint.y = padding.top;
                            break;
                        case FCContentAlignment.TopLeft:
                            tPoint.x = padding.left;
                            tPoint.y = padding.top;
                            break;
                        case FCContentAlignment.TopRight:
                            tPoint.x = width - tSize.cx - padding.right;
                            tPoint.y = padding.top;
                            break;
                    }
                    FCRect tRect = new FCRect(tPoint.x, tPoint.y, tPoint.x + tSize.cx, tPoint.y + tSize.cy);
                    if (AutoEllipsis && (tRect.right > clipRect.right || tRect.bottom > clipRect.bottom)) {
                        if (tRect.right > clipRect.right) {
                            tRect.right = clipRect.right;
                        }
                        if (tRect.bottom > clipRect.bottom) {
                            tRect.bottom = clipRect.bottom;
                        }
                        paint.drawTextAutoEllipsis(text, linkColor, font, tRect);
                    }
                    else {
                        paint.drawText(text, linkColor, font, tRect);
                    }
                    //画下划线
                    FCNative native = Native;
                    if (m_linkBehavior == FCLinkBehavior.AlwaysUnderLine || (m_linkBehavior == FCLinkBehavior.HoverUnderLine && (this == native.PushedControl || this == native.HoveredControl))) {
                        paint.drawLine(linkColor, 1, 0, tPoint.x, tPoint.y + tSize.cy, tPoint.x + tSize.cx, tPoint.y + tSize.cy);
                    }
                }
            }
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name">属性名</param>
        /// <param name="value">属性值</param>
        public override void setProperty(String name, String value) {
            if (name == "activelinkcolor") {
                ActiveLinkColor = FCStr.convertStrToColor(value);
            }
            else if (name == "disabledlinkcolor") {
                DisabledLinkColor = FCStr.convertStrToColor(value);
            }
            else if (name == "linkbehavior") {
                value = value.ToLower();
                if (value == "alwaysunderline") {
                    LinkBehavior = FCLinkBehavior.AlwaysUnderLine;
                }
                else if (value == "hoverunderline") {
                    LinkBehavior = FCLinkBehavior.HoverUnderLine;
                }
                else {
                    LinkBehavior = FCLinkBehavior.NeverUnderLine;
                }
            }
            else if (name == "linkcolor") {
                LinkColor = FCStr.convertStrToColor(value);
            }
            else if (name == "linkvisited") {
                LinkVisited = FCStr.convertStrToBool(value);
            }
            else if (name == "visitedlinkcolor") {
                VisitedLinkColor = FCStr.convertStrToColor(value);
            }
            else {
                base.setProperty(name, value);
            }
        }
    }
}
