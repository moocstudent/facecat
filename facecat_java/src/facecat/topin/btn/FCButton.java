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
 * 按钮控件
 */
public class FCButton extends FCView {

    /**
     * 创建按钮
     */
    public FCButton() {
        FCSize size = new FCSize(60, 20);
        setSize(size);
    }

    protected String m_disabledBackImage;

    /**
     * 获取不可用时的背景图片
     */
    public final String getDisabledBackImage() {
        return m_disabledBackImage;
    }

    /**
     * 设置不可用时的背景图片
     */
    public final void setDisabledBackImage(String value) {
        m_disabledBackImage = value;
    }

    protected String m_hoveredBackImage;

    /**
     * 获取触摸悬停时的背景图片
     */
    public final String getHoveredBackImage() {
        return m_hoveredBackImage;
    }

    /**
     * 设置触摸悬停时的背景图片
     */
    public final void setHoveredBackImage(String value) {
        m_hoveredBackImage = value;
    }

    protected String m_pushedBackImage;

    /**
     * 获取触摸按下时的背景图片
     */
    public final String getPushedBackImage() {
        return m_pushedBackImage;
    }

    /**
     * 设置触摸按下时的背景图片
     */
    public final void setPushedBackImage(String value) {
        m_pushedBackImage = value;
    }

    protected FCContentAlignment m_textAlign = FCContentAlignment.MiddleCenter;

    /**
     * 获取文字的布局方式
     */
    public final FCContentAlignment getTextAlign() {
        return m_textAlign;
    }

    /**
     * 设置文字的布局方式
     */
    public final void setTextAlign(FCContentAlignment value) {
        m_textAlign = value;
    }

    /**
     * 获取控件类型
     *
     * @return 控件类型
     */
    @Override
    public String getControlType() {
        return "Button";
    }

    /**
     * 获取要绘制的背景色
     *
     * @return 背景色
     */
    @Override
    protected long getPaintingBackColor() {
        long backColor = super.getPaintingBackColor();
        if (backColor != FCColor.None && isPaintEnabled(this)) {
            FCNative inative = getNative();
            if (this == inative.getPushedControl()) {
                backColor = FCColor.Pushed;
            } else if (this == inative.getHoveredControl()) {
                backColor = FCColor.Hovered;
            }
        }
        return backColor;
    }

    /**
     * 获取用于绘制的背景图片
     */
    @Override
    protected String getPaintingBackImage() {
        String backImage = null;
        if (isPaintEnabled(this)) {
            FCNative inative = getNative();
            if (this == inative.getPushedControl()) {
                backImage = m_pushedBackImage;
            } else if (this == inative.getHoveredControl()) {
                backImage = m_hoveredBackImage;
            }
        } else {
            backImage = m_disabledBackImage;
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
        if (name.equals("disabledbackimage")) {
            type.argvalue = "text";
            value.argvalue = getDisabledBackImage();
        } else if (name.equals("hoveredbackimage")) {
            type.argvalue = "text";
            value.argvalue = getHoveredBackImage();
        } else if (name.equals("pushedbackimage")) {
            type.argvalue = "text";
            value.argvalue = getPushedBackImage();
        } else if (name.equals("textalign")) {
            type.argvalue = "enum:FCContentAlignment";
            value.argvalue = FCStr.convertContentAlignmentToStr(getTextAlign());
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
        propertyNames.addAll(Arrays.asList(new String[]{"DisabledBackImage", "HoveredBackImage", "PushedBackImage", "TextAlign"}));
        return propertyNames;
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
     * 触摸移动方法
     *
     * @param touchInfo 触摸信息
     */
    @Override
    public void onTouchMove(FCTouchInfo touchInfo) {
        super.onTouchMove(touchInfo.clone());
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
        if (text != null && text.length() > 0) {
            int width = getWidth(), height = getHeight();
            if (width > 0 && height > 0) {
                FCFont font = getFont();
                FCSize tSize = paint.textSize(text, font);
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
        if (name.equals("disabledbackimage")) {
            setDisabledBackImage(value);
        } else if (name.equals("hoveredbackimage")) {
            setHoveredBackImage(value);
        } else if (name.equals("pushedbackimage")) {
            setPushedBackImage(value);
        } else if (name.equals("textalign")) {
            setTextAlign(FCStr.convertStrToContentAlignment(value));
        } else {
            super.setProperty(name, value);
        }
    }
}
