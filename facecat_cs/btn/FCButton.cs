/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-捂脸鹿创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
using System;
using System.Collections.Generic;

namespace FaceCat {
    /// <summary>
    /// 按钮控件
    /// </summary>
    public class FCButton : FCView {
        /// <summary>
        /// 创建按钮
        /// </summary>
        public FCButton() {
            FCSize size = new FCSize(60, 20);
            Size = size;
        }

        protected String m_disabledBackImage;

        /// <summary>
        /// 获取或设置不可用时的背景图片
        /// </summary>
        public virtual String DisabledBackImage {
            get { return m_disabledBackImage; }
            set { m_disabledBackImage = value; }
        }

        protected String m_hoveredBackImage;

        /// <summary>
        /// 获取或设置触摸悬停时的背景图片
        /// </summary>
        public virtual String HoveredBackImage {
            get { return m_hoveredBackImage; }
            set { m_hoveredBackImage = value; }
        }

        private String m_pushedBackImage;

        /// <summary>
        /// 获取或设置触摸按下时的背景图片
        /// </summary>
        public virtual String PushedBackImage {
            get { return m_pushedBackImage; }
            set { m_pushedBackImage = value; }
        }

        protected FCContentAlignment m_textAlign = FCContentAlignment.MiddleCenter;

        /// <summary>
        /// 获取或设置文字的布局方式
        /// </summary>
        public virtual FCContentAlignment TextAlign {
            get { return m_textAlign; }
            set { m_textAlign = value; }
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <returns>控件类型</returns>
        public override String getControlType() {
            return "Button";
        }

        /// <summary>
        /// 获取要绘制的背景色
        /// </summary>
        /// <returns>背景色</returns>
        protected override long getPaintingBackColor() {
            long backColor = base.getPaintingBackColor();
            if (backColor != FCColor.None && isPaintEnabled(this)) {
                FCNative native = Native;
                if (this == native.PushedControl) {
                    backColor = FCColor.Pushed;
                }
                else if (this == native.HoveredControl) {
                    backColor = FCColor.Hovered;
                }
            }
            return backColor;
        }

        /// <summary>
        /// 获取用于绘制的背景图片
        /// </summary>
        protected override String getPaintingBackImage() {
            String backImage = null;
            if (isPaintEnabled(this)) {
                FCNative native = Native;
                if (this == native.PushedControl) {
                    backImage = m_pushedBackImage;
                }
                else if (this == native.HoveredControl) {
                    backImage = m_hoveredBackImage;
                }
            }
            else {
                backImage = m_disabledBackImage;
            }
            if (backImage != null) {
                return backImage;
            }
            else {
                return base.getPaintingBackImage();
            }
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">返回属性值</param>
        /// <param name="type">返回属性类型</param>
        public override void getProperty(String name, ref String value, ref String type) {
            if (name == "disabledbackimage") {
                type = "text";
                value = DisabledBackImage;
            }
            else if (name == "hoveredbackimage") {
                type = "text";
                value = HoveredBackImage;
            }
            else if (name == "pushedbackimage") {
                type = "text";
                value = PushedBackImage;
            }
            else if (name == "textalign") {
                type = "enum:FCContentAlignment";
                value = FCStr.convertContentAlignmentToStr(TextAlign);
            }
            else {
                base.getProperty(name, ref value, ref type);
            }
        }

        /// <summary>
        /// 获取属性名称列表
        /// </summary>
        /// <returns></returns>
        public override ArrayList<String> getPropertyNames() {
            ArrayList<String> propertyNames = base.getPropertyNames();
            propertyNames.AddRange(new String[] { "DisabledBackImage", "HoveredBackImage", "PushedBackImage", "TextAlign" });
            return propertyNames;
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
        /// 触摸移动方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onTouchMove(FCTouchInfo touchInfo) {
            base.onTouchMove(touchInfo);
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
                    long textColor = getPaintingTextColor();
                    if (AutoEllipsis && (tRect.right > clipRect.right || tRect.bottom > clipRect.bottom)) {
                        if (tRect.right > clipRect.right) {
                            tRect.right = clipRect.right;
                        }
                        if (tRect.bottom > clipRect.bottom) {
                            tRect.bottom = clipRect.bottom;
                        }
                        paint.drawTextAutoEllipsis(text, textColor, font, tRect);
                    }
                    else {
                        paint.drawText(text, textColor, font, tRect);
                    }
                }
            }
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        public override void setProperty(String name, String value) {
            if (name == "disabledbackimage") {
                DisabledBackImage = value;
            }
            else if (name == "hoveredbackimage") {
                HoveredBackImage = value;
            }
            else if (name == "pushedbackimage") {
                PushedBackImage = value;
            }
            else if (name == "textalign") {
                TextAlign = FCStr.convertStrToContentAlignment(value);
            }
            else {
                base.setProperty(name, value);
            }
        }
    }
}
