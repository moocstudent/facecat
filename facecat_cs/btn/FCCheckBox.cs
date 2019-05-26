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
    /// 复选框控件
    /// </summary>
    public class FCCheckBox : FCButton {
        /// <summary>
        /// 创建复选框
        /// </summary>
        public FCCheckBox() {
            BackColor = FCColor.None;
            BorderColor = FCColor.None;
        }

        protected FCHorizontalAlign m_buttonAlign = FCHorizontalAlign.Left;

        /// <summary>
        /// 获取或设置内容的横向排列样式
        /// </summary>
        public virtual FCHorizontalAlign ButtonAlign {
            get { return m_buttonAlign; }
            set { m_buttonAlign = value; }
        }

        protected long m_buttonBackColor = FCColor.Border;

        /// <summary>
        /// 获取或设置按钮的背景色
        /// </summary>
        public virtual long ButtonBackColor {
            get { return m_buttonBackColor; }
            set { m_buttonBackColor = value; }
        }

        protected long m_buttonBorderColor = FCColor.Border;

        /// <summary>
        /// 获取或设置按钮的边线颜色
        /// </summary>
        public virtual long ButtonBorderColor {
            get { return m_buttonBorderColor; }
            set { m_buttonBorderColor = value; }
        }

        protected FCSize m_buttonSize = new FCSize(16, 16);

        /// <summary>
        /// 获取或设置按钮的尺寸
        /// </summary>
        public virtual FCSize ButtonSize {
            get { return m_buttonSize; }
            set { m_buttonSize = value; }
        }

        protected bool m_checked;

        /// <summary>
        /// 获取或设置是否被选中
        /// </summary>
        public virtual bool Checked {
            get { return m_checked; }
            set {
                if (m_checked != value) {
                    m_checked = value;
                    onCheckedChanged();
                }
            }
        }

        protected String m_checkedBackImage;

        /// <summary>
        /// 获取或设置选中时的背景图
        /// </summary>
        public virtual String CheckedBackImage {
            get { return m_checkedBackImage; }
            set { m_checkedBackImage = value; }
        }

        protected String m_checkHoveredBackImage;

        /// <summary>
        /// 获取或设置触摸悬停且选中时的背景图片
        /// </summary>
        public virtual String CheckHoveredBackImage {
            get { return m_checkHoveredBackImage; }
            set { m_checkHoveredBackImage = value; }
        }

        protected String m_checkPushedBackImage;

        /// <summary>
        /// 获取或设置触摸按下且选中时的背景图片
        /// </summary>
        public virtual String CheckPushedBackImage {
            get { return m_checkPushedBackImage; }
            set { m_checkPushedBackImage = value; }
        }

        protected String m_disableCheckedBackImage;

        /// <summary>
        /// 获取或设置不可用时的选中背景图片
        /// </summary>
        public virtual String DisableCheckedBackImage {
            get { return m_disableCheckedBackImage; }
            set { m_disableCheckedBackImage = value; }
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <returns>控件类型</returns>
        public override String getControlType() {
            return "CheckBox";
        }

        /// <summary>
        /// 获取事件名称列表
        /// </summary>
        /// <returns>名称列表</returns>
        public override ArrayList<String> getEventNames() {
            ArrayList<String> eventNames = base.getEventNames();
            eventNames.AddRange(new String[] { "CheckedChanged" });
            return eventNames;
        }

        /// <summary>
        /// 获取或设置的背景色
        /// </summary>
        /// <returns>背景色</returns>
        protected override long getPaintingBackColor() {
            long backColor = BackColor;
            if (backColor != FCColor.None && FCColor.DisabledBack != FCColor.None) {
                if (!isPaintEnabled(this)) {
                    return FCColor.DisabledBack;
                }
            }
            return backColor;
        }

        /// <summary>
        /// 获取要绘制的按钮的背景色
        /// </summary>
        /// <returns>按钮的背景色</returns>
        protected virtual long getPaintingButtonBackColor() {
            long buttonBackColor = m_buttonBackColor;
            if (buttonBackColor != FCColor.None && FCColor.DisabledBack != FCColor.None) {
                if (!isPaintEnabled(this)) {
                    return FCColor.DisabledBack;
                }
            }
            return buttonBackColor;
        }

        /// <summary>
        /// 获取要绘制的按钮边线颜色
        /// </summary>
        /// <returns>边线的颜色</returns>
        protected virtual long getPaintingButtonBorderColor() {
            return m_buttonBorderColor;
        }

        /// <summary>
        /// 获取用于绘制的背景图片
        /// </summary>
        protected override String getPaintingBackImage() {
            String backImage = null;
            FCNative native = Native;
            if (m_checked) {
                if (Enabled) {
                    if (this == native.PushedControl) {
                        backImage = m_checkPushedBackImage;
                    }
                    else if (this == native.HoveredControl) {
                        backImage = m_checkHoveredBackImage;
                    }
                    else {
                        backImage = m_checkedBackImage;
                    }
                }
                else {
                    backImage = m_disableCheckedBackImage;
                }
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
            if (name == "buttonalign") {
                type = "enum:FCHorizontalAlign";
                value = FCStr.convertHorizontalAlignToStr(ButtonAlign);
            }
            else if (name == "buttonbackcolor") {
                type = "color";
                value = FCStr.convertColorToStr(ButtonBackColor);
            }
            else if (name == "buttonbordercolor") {
                type = "color";
                value = FCStr.convertColorToStr(ButtonBorderColor);
            }
            else if (name == "buttonsize") {
                type = "size";
                value = FCStr.convertSizeToStr(ButtonSize);
            }
            else if (name == "checked") {
                type = "bool";
                value = FCStr.convertBoolToStr(Checked);
            }
            else if (name == "checkedbackimage") {
                type = "text";
                value = CheckedBackImage;
            }
            else if (name == "checkhoveredbackimage") {
                type = "text";
                value = CheckHoveredBackImage;
            }
            else if (name == "checkpushedbackimage") {
                type = "text";
                value = CheckPushedBackImage;
            }
            else if (name == "disablecheckedbackimage") {
                type = "text";
                value = DisableCheckedBackImage;
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
            propertyNames.AddRange(new String[] { "ButtonAlign", "ButtonBackColor","ButtonBorderColor", "ButtonSize", "Checked", "CheckedBackImage",
            "CheckHoveredBackimage", "CheckPushedBackImage", "DisableCheckedBackImage"});
            return propertyNames;
        }

        /// <summary>
        /// 选中改变方法
        /// </summary>
        public virtual void onCheckedChanged() {
            callEvents(FCEventID.CHECKEDCHANGED);
            update();
        }

        /// <summary>
        /// 点击方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onClick(FCTouchInfo touchInfo) {
            Checked = !Checked;
            callTouchEvents(FCEventID.CLICK, touchInfo);
            invalidate();
        }

        /// <summary>
        /// 重绘背景方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void onPaintBackground(FCPaint paint, FCRect clipRect) {
            FCRect rect = new FCRect(0, 0, Width, Height);
            //绘制背景色
            paint.fillRoundRect(getPaintingBackColor(), rect, m_cornerRadius);
        }

        /// <summary>
        /// 重绘选中按钮方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public virtual void onPaintCheckButton(FCPaint paint, FCRect clipRect) {
            //绘制背景图
            String bkImage = getPaintingBackImage();
            if (bkImage != null && bkImage.Length > 0) {
                paint.drawImage(bkImage, clipRect);
            }
            else {
                if (m_checked) {
                    FCRect innerRect = new FCRect(clipRect.left + 2, clipRect.top + 2, clipRect.right - 3, clipRect.bottom - 3);
                    if (clipRect.right - clipRect.left < 4 || clipRect.bottom - clipRect.top < 4) {
                        innerRect = clipRect;
                    }
                    paint.fillRect(getPaintingButtonBackColor(), innerRect);
                }
                paint.drawRect(getPaintingButtonBorderColor(), 1, 0, clipRect);
            }
        }

        /// <summary>
        /// 重绘前景方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void onPaintForeground(FCPaint paint, FCRect clipRect) {
            String text = Text;
            int width = Width, height = Height;
            if (width > 0 && height > 0) {
                FCRect buttonRect = new FCRect(5, (height - m_buttonSize.cy) / 2, 5 + m_buttonSize.cx, (height + m_buttonSize.cy) / 2);
                FCPoint tLocation = new FCPoint();
                FCSize tSize = new FCSize();
                FCFont font = Font;
                if (text != null && text.Length > 0) {
                    tSize = paint.textSize(text, font);
                    tLocation.x = buttonRect.right + 5;
                    tLocation.y = (height - tSize.cy) / 2;
                }
                //居中
                if (m_buttonAlign == FCHorizontalAlign.Center) {
                    buttonRect.left = (width - m_buttonSize.cx) / 2;
                    buttonRect.right = (width + m_buttonSize.cx) / 2;
                    tLocation.x = buttonRect.right + 5;
                }
                //远离
                else if (m_buttonAlign == FCHorizontalAlign.Right) {
                    buttonRect.left = width - m_buttonSize.cx - 5;
                    buttonRect.right = width - 5;
                    tLocation.x = buttonRect.left - tSize.cx - 5;
                }
                //绘制背景图
                onPaintCheckButton(paint, buttonRect);
                //绘制文字
                if (text != null && text.Length > 0) {
                    FCRect tRect = new FCRect(tLocation.x, tLocation.y, tLocation.x + tSize.cx + 1, tLocation.y + tSize.cy);
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
            if (name == "buttonalign") {
                ButtonAlign = FCStr.convertStrToHorizontalAlign(value);
            }
            else if (name == "buttonbackcolor") {
                ButtonBackColor = FCStr.convertStrToColor(value);
            }
            else if (name == "buttonbordercolor") {
                ButtonBorderColor = FCStr.convertStrToColor(value);
            }
            else if (name == "buttonsize") {
                ButtonSize = FCStr.convertStrToSize(value);
            }
            else if (name == "checked") {
                Checked = FCStr.convertStrToBool(value);
            }
            else if (name == "checkedbackimage") {
                CheckedBackImage = value;
            }
            else if (name == "checkhoveredbackimage") {
                CheckHoveredBackImage = value;
            }
            else if (name == "checkpushedbackimage") {
                CheckPushedBackImage = value;
            }
            else if (name == "disablecheckedbackimage") {
                DisableCheckedBackImage = value;
            }
            else {
                base.setProperty(name, value);
            }
        }
    }
}
