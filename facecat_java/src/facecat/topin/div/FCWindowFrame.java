/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.div;

import facecat.topin.core.*;
import java.util.*;

/**
 * 窗体控件边界
 */
public class FCWindowFrame extends FCView {

    /**
     * 创建窗体控件
     */
    public FCWindowFrame() {
        setBackColor(FCColor.None);
        setBorderColor(FCColor.None);
        setDock(FCDockStyle.Fill);
    }

    /**
     * 是否包含坐标
     *
     * @param point 坐标
     * @returns 是否包含
     */
    @Override
    public boolean containsPoint(FCPoint FCPoint) {
        ArrayList<FCView> controls = getControls();
        int controlsSize = controls.size();
        for (int i = 0; i < controlsSize; i++) {
            FCWindow window = (FCWindow) ((controls.get(i) instanceof FCWindow) ? controls.get(i) : null);
            if (window != null && window.getFrame() == this) {
                if (window.isDialog()) {
                    return true;
                } else {
                    return window.containsPoint(FCPoint);
                }
            }
        }
        return false;
    }

    /**
     * 获取控件类型
     */
    @Override
    public String getControlType() {
        return "WindowFrame";
    }

    /**
     * 重绘方法
     */
    @Override
    public void invalidate() {
        if (m_native != null) {
            ArrayList<FCView> controls = getControls();
            int controlsSize = controls.size();
            for (int i = 0; i < controlsSize; i++) {
                FCWindow window = (FCWindow) ((controls.get(i) instanceof FCWindow) ? controls.get(i) : null);
                if (window != null) {
                    m_native.invalidate(window.getDynamicPaintRect());
                    break;
                }
            }
        }
    }

    /**
     * 绘制背景方法
     *
     * @param paint 绘图对象
     * @param clipRect 裁剪区域
     */
    @Override
    public void onPaintBackground(FCPaint paint, FCRect clipRect) {
        super.onPaintBackground(paint, clipRect);
        if (paint.supportTransparent()) {
            ArrayList<FCView> controls = getControls();
            int controlsSize = controls.size();
            for (int i = 0; i < controlsSize; i++) {
                FCWindow window = (FCWindow) ((controls.get(i) instanceof FCWindow) ? controls.get(i) : null);
                if (window != null) {
                    long shadowColor = window.getShadowColor();
                    int shadowSize = window.getShadowSize();
                    if (shadowColor != FCColor.None && shadowSize > 0 && window.isDialog() && window.getFrame() == this) {
                        FCRect bounds = window.getBounds();
                        FCRect leftShadow = new FCRect(bounds.left - shadowSize, bounds.top - shadowSize, bounds.left, bounds.bottom + shadowSize);
                        paint.fillRect(shadowColor, leftShadow);
                        FCRect rightShadow = new FCRect(bounds.right, bounds.top - shadowSize, bounds.right + shadowSize, bounds.bottom + shadowSize);
                        paint.fillRect(shadowColor, rightShadow);
                        FCRect topShadow = new FCRect(bounds.left, bounds.top - shadowSize, bounds.right, bounds.top);
                        paint.fillRect(shadowColor, topShadow);
                        FCRect bottomShadow = new FCRect(bounds.left, bounds.bottom, bounds.right, bounds.bottom + shadowSize);
                        paint.fillRect(shadowColor, bottomShadow);
                    }
                }
            }
        }
    }
}
