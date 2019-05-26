/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.btn;

import facecat.topin.core.*;
import java.util.*;

/**
 * 复选框控件
 */
public class FCCheckBox extends FCButton {

    /**
     * 创建复选框
     */
    public FCCheckBox() {
        setBackColor(FCColor.None);
        setBorderColor(FCColor.None);
    }

    protected FCHorizontalAlign m_buttonAlign = FCHorizontalAlign.Left;

    /**
     * 获取内容的横向排列样式
     */
    public FCHorizontalAlign getButtonAlign() {
        return m_buttonAlign;
    }

    /**
     * 设置内容的横向排列样式
     */
    public void setButtonAlign(FCHorizontalAlign value) {
        m_buttonAlign = value;
    }

    protected long m_buttonBackColor = FCColor.Border;

    /**
     * 获取按钮的背景色
     */
    public long getButtonBackColor() {
        return m_buttonBackColor;
    }

    /**
     * 设置按钮的背景色
     */
    public void setButtonBackColor(long value) {
        m_buttonBackColor = value;
    }

    protected long m_buttonBorderColor = FCColor.Border;

    /**
     *  获取按钮的边线颜色
     */
    public long getButtonBorderColor() {
        return m_buttonBorderColor;
    }

    /**
     * 设置按钮的边线颜色
     */
    public void setButtonBorderColor(long value) {
        m_buttonBorderColor = value;
    }

    protected FCSize m_buttonSize = new FCSize(16, 16);

    /**
     * 获取按钮的尺寸
     */
    public FCSize getButtonSize() {
        return m_buttonSize.clone();
    }

    /**
     * 设置按钮的尺寸
     */
    public void setButtonSize(FCSize value) {
        m_buttonSize = value.clone();
    }

    protected boolean m_checked = false;

    /**
     * 获取是否被选中
     */
    public boolean isChecked() {
        return m_checked;
    }

    /**
     * 设置是否被选中
     */
    public void setChecked(boolean value) {
        if (m_checked != value) {
            m_checked = value;
            onCheckedChanged();
        }
    }

    protected String m_checkedBackImage;

    /**
     * 获取选中时的背景图
     */
    public String getCheckedBackImage() {
        return m_checkedBackImage;
    }

    /**
     * 设置选中时的背景图
     */
    public void setCheckedBackImage(String value) {
        m_checkedBackImage = value;
    }

    protected String m_checkHoveredBackImage;

    /**
     * 获取触摸悬停且选中时的背景图片
     */
    public String getCheckHoveredBackImage() {
        return m_checkHoveredBackImage;
    }

    /**
     * 设置触摸悬停且选中时的背景图片
     */
    public void setCheckHoveredBackImage(String value) {
        m_checkHoveredBackImage = value;
    }

    protected String m_checkPushedBackImage;

    /**
     * 获取触摸按下且选中时的背景图片
     */
    public String getCheckPushedBackImage() {
        return m_checkPushedBackImage;
    }

    /**
     * 设置触摸按下且选中时的背景图片
     */
    public void setCheckPushedBackImage(String value) {
        m_checkPushedBackImage = value;
    }

    protected String m_disableCheckedBackImage;

    /**
     * 获取不可用时的选中背景图片
     */
    public String getDisableCheckedBackImage() {
        return m_disableCheckedBackImage;
    }

    /**
     * 设置不可用时的选中背景图片
     */
    public void setDisableCheckedBackImage(String value) {
        m_disableCheckedBackImage = value;
    }

    /**
     * 获取控件类型
     */
    @Override
    public String getControlType() {
        return "CheckBox";
    }
    
    /**
     * 获取或设置的背景色
     */
    @Override
    protected long getPaintingBackColor() {
        long backColor = getBackColor();
        if (backColor != FCColor.None && FCColor.DisabledBack != FCColor.None) {
            if (!isPaintEnabled(this)) {
                return FCColor.DisabledBack;
            }
        }
        return backColor;
    }

    /**
     * 获取要绘制的按钮的背景色
     */
    protected long getPaintingButtonBackColor() {
        long buttonBackColor = m_buttonBackColor;
        if (buttonBackColor != FCColor.None && FCColor.DisabledBack != FCColor.None) {
            if (!isPaintEnabled(this)) {
                return FCColor.DisabledBack;
            }
        }
        return buttonBackColor;
    }

    /**
     * 获取要绘制的按钮边线颜色
     */
    protected long getPaintingButtonBorderColor() {
        return m_buttonBorderColor;
    }

    /**
     *  获取用于绘制的背景图片
     */
    @Override
    protected String getPaintingBackImage() {
        String backImage = null;
        FCNative inative = getNative();
        if (m_checked) {
            if (isEnabled()) {
                if (this == inative.getPushedControl()) {
                    backImage = m_checkPushedBackImage;
                } else if (this == inative.getHoveredControl()) {
                    backImage = m_checkHoveredBackImage;
                } else {
                    backImage = m_checkedBackImage;
                }
            } else {
                backImage = m_disableCheckedBackImage;
            }
        }
        if (backImage != null) {
            return backImage;
        } else {
            return super.getPaintingBackImage();
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
        if (name.equals("buttonalign")) {
            type.argvalue = "enum:FCHorizontalAlign";
            value.argvalue = FCStr.convertHorizontalAlignToStr(getButtonAlign());
        } else if (name.equals("buttonbackcolor")) {
            type.argvalue = "color";
            value.argvalue = FCStr.convertColorToStr(getButtonBackColor());
        } else if (name.equals("buttonbordercolor")) {
            type.argvalue = "color";
            value.argvalue = FCStr.convertColorToStr(getButtonBorderColor());
        } else if (name.equals("buttonsize")) {
            type.argvalue = "size";
            value.argvalue = FCStr.convertSizeToStr(getButtonSize());
        } else if (name.equals("checked")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(isChecked());
        } else if (name.equals("checkedbackimage")) {
            type.argvalue = "text";
            value.argvalue = getCheckedBackImage();
        } else if (name.equals("checkhoveredbackimage")) {
            type.argvalue = "text";
            value.argvalue = getCheckHoveredBackImage();
        } else if (name.equals("checkpushedbackimage")) {
            type.argvalue = "text";
            value.argvalue = getCheckPushedBackImage();
        } else if (name.equals("disablecheckedbackimage")) {
            type.argvalue = "text";
            value.argvalue = getDisableCheckedBackImage();
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
        propertyNames.addAll(Arrays.asList(new String[]{"ButtonAlign", "ButtonBackColor", "ButtonBorderColor", "ButtonSize", "Checked", "CheckedBackImage", "CheckHoveredBackimage", "CheckPushedBackImage", "DisableCheckedBackImage"}));
        return propertyNames;
    }

    /**
     * 选中改变方法
     */
    public void onCheckedChanged() {
        callEvents(FCEventID.CHECKEDCHANGED);
        update();
    }

    /**
     * 点击方法
     *
     * @param touchInfo 触摸信息
     */
    @Override
    public void onClick(FCTouchInfo touchInfo) {
        setChecked(!isChecked());
        callTouchEvents(FCEventID.CLICK, touchInfo.clone());
        invalidate();
    }
    
    /**
     * 重绘背景方法
     *
     * @param paint 绘图对象
     * @param clipRect 裁剪区域
     */
    @Override
    public void onPaintBackground(FCPaint paint, FCRect clipRect) {
        FCRect rect = new FCRect(0, 0, getWidth(), getHeight());
        paint.fillRoundRect(getPaintingBackColor(), rect, m_cornerRadius);
    }

    /**
     * 重绘选中按钮方法
     *
     * @param paint 绘图对象
     * @param clipRect 裁剪区域
     */
    public void onPaintCheckButton(FCPaint paint, FCRect clipRect) {
        String bkImage = getPaintingBackImage();
        if (bkImage != null && bkImage.length() > 0) {
            paint.drawImage(bkImage, clipRect);
        } else {
            if (m_checked) {
                FCRect innerRect = new FCRect(clipRect.left + 2, clipRect.top + 2, clipRect.right - 2, clipRect.bottom - 2);
                if (clipRect.right - clipRect.left < 4 || clipRect.bottom - clipRect.top < 4) {
                    innerRect = clipRect.clone();
                }
                paint.fillRect(getPaintingButtonBackColor(), innerRect);
            }
            paint.drawRect(getPaintingButtonBorderColor(), 1, 0, clipRect);
        }
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
        int width = getWidth(), height = getHeight();
        if (width > 0 && height > 0) {
            FCRect buttonRect = new FCRect(5, (height - m_buttonSize.cy) / 2, 5 + m_buttonSize.cx, (height + m_buttonSize.cy) / 2);
            FCPoint tLocation = new FCPoint();
            FCSize tSize = new FCSize();
            FCFont font = getFont();
            if (text != null && text.length() > 0) {
                tSize = paint.textSize(text, font);
                tLocation.x = buttonRect.right + 5;
                tLocation.y = (height - tSize.cy) / 2;
            }
            // 居中
            if (m_buttonAlign == FCHorizontalAlign.Center) {
                buttonRect.left = (width - m_buttonSize.cx) / 2;
                buttonRect.right = (width + m_buttonSize.cx) / 2;
                tLocation.x = buttonRect.right + 5;
            }
            // 远离
            else if (m_buttonAlign == FCHorizontalAlign.Right) {
                buttonRect.left = width - m_buttonSize.cx - 5;
                buttonRect.right = width - 5;
                tLocation.x = buttonRect.left - tSize.cx - 5;
            }
             // 绘制背景图
            onPaintCheckButton(paint, buttonRect);
            // 绘制文字
            if (text != null && text.length() > 0) {
                FCRect tRect = new FCRect(tLocation.x, tLocation.y, tLocation.x + tSize.cx + 1, tLocation.y + tSize.cy);
                long textColor = getPaintingTextColor();
                if (autoEllipsis() && (tRect.right > clipRect.right || tRect.bottom > clipRect.bottom)) {
                    if (tRect.right > clipRect.right) {
                        tRect.right = clipRect.right;
                    }
                    if (tRect.bottom > clipRect.bottom) {
                        tRect.bottom = clipRect.bottom;
                    }
                    paint.drawTextAutoEllipsis(text, textColor, font, tRect);
                } else {
                    paint.drawText(text, textColor, font, tRect);
                }
            }
        }
    }
 
    /**
     * 设置属性
     *
     * @param name 属性名称
     * @param value 属性值
     */
    @Override
    public void setProperty(String name, String value) {
        if (name.equals("buttonalign")) {
            setButtonAlign(FCStr.convertStrToHorizontalAlign(value));
        } else if (name.equals("buttonbackcolor")) {
            setButtonBackColor(FCStr.convertStrToColor(value));
        } else if (name.equals("buttonbordercolor")) {
            setButtonBorderColor(FCStr.convertStrToColor(value));
        } else if (name.equals("buttonsize")) {
            setButtonSize(FCStr.convertStrToSize(value));
        } else if (name.equals("checked")) {
            setChecked(FCStr.convertStrToBool(value));
        } else if (name.equals("checkedbackimage")) {
            setCheckedBackImage(value);
        } else if (name.equals("checkhoveredbackimage")) {
            setCheckHoveredBackImage(value);
        } else if (name.equals("checkpushedbackimage")) {
            setCheckPushedBackImage(value);
        } else if (name.equals("disablecheckedbackimage")) {
            setDisableCheckedBackImage(value);
        } else {
            super.setProperty(name, value);
        }
    }
}
