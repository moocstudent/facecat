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
 * 标签控件
 */
public class FCLabel extends FCView {

    /**
     * 创建标签控件
     */
    public FCLabel() {
        setAutoSize(true);
        setBackColor(FCColor.None);
        setBorderColor(FCColor.None);
        FCSize size = new FCSize(100, 20);
        setSize(size);
    }

    protected FCContentAlignment m_textAlign = FCContentAlignment.TopLeft;

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
     */
    @Override
    public String getControlType() {
        return "Label";
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
        if (name.equals("textalign")) {
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
        propertyNames.addAll(Arrays.asList(new String[]{"TextAlign"}));
        return propertyNames;
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
     * 预绘图方法
     *
     * @param paint 绘图对象
     * @param clipRect 裁剪区域
     */
    @Override
    public void onPrePaint(FCPaint paint, FCRect clipRect) {
        if (autoSize()) {
            String text = getText();
            FCFont font = getFont();
            FCSize tSize = paint.textSize(text, font);
            int newW = tSize.cx + 4;
            int newH = tSize.cy + 4;
            int width = getWidth(), height = getHeight();
            if (newW != width || newH != height) {
                setSize(new FCSize(newW, newH));
                width = newW;
                height = newH;
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
        if (name.equals("textalign")) {
            setTextAlign(FCStr.convertStrToContentAlignment(value));
        } else {
            super.setProperty(name, value);
        }
    }
}
