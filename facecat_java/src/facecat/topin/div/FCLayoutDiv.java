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
 * 布局控件
 */
public class FCLayoutDiv extends FCDiv {

    /**
     * 创建布局控件
     */
    public FCLayoutDiv() {
    }

    protected boolean m_autoWrap = false;

    /**
     * 获取是否自动换行
     */
    public boolean autoWrap() {
        return m_autoWrap;
    }

    /**
     * 设置是否自动换行
     */
    public void setAutoWrap(boolean autoWrap) {
        m_autoWrap = autoWrap;
    }

    protected FCLayoutStyle m_layoutStyle = FCLayoutStyle.LeftToRight;

    /**
     * 获取排列模式
     */
    public FCLayoutStyle getLayoutStyle() {
        return m_layoutStyle;
    }

    /**
     * 设置排列模式
     */
    public void setLayoutStyle(FCLayoutStyle value) {
        m_layoutStyle = value;
    }

    /**
     * 获取控件类型
     */
    @Override
    public String getControlType() {
        return "LayoutDiv";
    }

    /**
     * 设置属性值
     *
     * @param name 属性名称
     * @param value 返回属性值
     * @param type 返回属性类型
     */
    @Override
    public void getProperty(String name, RefObject<String> value, RefObject<String> type) {
        if (name.equals("autowrap")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(autoWrap());
        } else if (name.equals("layoutstyle")) {
            type.argvalue = "enum:FCLayoutStyle";
            value.argvalue = FCStr.convertLayoutStyleToStr(getLayoutStyle());
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
        propertyNames.addAll(Arrays.asList(new String[]{"AutoWrap", "LayoutStyle"}));
        return propertyNames;
    }

    /**
     * 重置布局
     */
    public boolean onResetLayout() {
        boolean reset = false;
        if (getNative() != null) {
            FCPadding padding = getPadding();
            int left = padding.left, top = padding.top;
            int width = getWidth() - padding.left - padding.right;
            int height = getHeight() - padding.top - padding.bottom;
            int controlSize = m_controls.size();
            for (int i = 0; i < controlSize; i++) {
                FCView control = m_controls.get(i);
                if (control.isVisible() && control != getHScrollBar() && control != getVScrollBar()) {
                    FCSize size = control.getSize();
                    int cLeft = control.getLeft(), cTop = control.getTop(), cWidth = size.cx, cHeight = size.cy;
                    int nLeft = cLeft, nTop = cTop, nWidth = cWidth, nHeight = cHeight;
                    FCPadding margin = control.getMargin();
                    if (m_layoutStyle == FCLayoutStyle.BottomToTop) {
                        if (i == 0) {
                            top = padding.top + height;
                        }
                        int lWidth = 0;
                        if (m_autoWrap) {
                            lWidth = size.cx;
                            int lTop = top - margin.top - cHeight - margin.bottom;
                            if (lTop < padding.top) {
                                left += cWidth + margin.left;
                                top = height - padding.top;
                            }
                        } else {
                            lWidth = width - margin.left - margin.right;
                        }
                        top -= cHeight + margin.bottom;
                        nLeft = left + margin.left;
                        nWidth = lWidth;
                        nTop = top;
                    } else if (m_layoutStyle == FCLayoutStyle.LeftToRight) {
                        int lHeight = 0;
                        if (m_autoWrap) {
                            lHeight = size.cy;
                            int lRight = left + margin.left + cWidth + margin.right;
                            if (lRight > width) {
                                left = padding.left;
                                top += cHeight + margin.top;
                            }
                        } else {
                            lHeight = height - margin.top - margin.bottom;
                        }
                        left += margin.left;
                        nLeft = left;
                        nTop = top + margin.top;
                        nHeight = lHeight;
                        left += cWidth + margin.right;
                    } else if (m_layoutStyle == FCLayoutStyle.RightToLeft) {
                        if (i == 0) {
                            left = width - padding.left;
                        }
                        int lHeight = 0;
                        if (m_autoWrap) {
                            lHeight = size.cy;
                            int lLeft = left - margin.left - cWidth - margin.right;
                            if (lLeft < padding.left) {
                                left = width - padding.left;
                                top += cHeight + margin.top;
                            }
                        } else {
                            lHeight = height - margin.top - margin.bottom;
                        }
                        left -= cWidth + margin.left;
                        nLeft = left;
                        nTop = top + margin.top;
                        nHeight = lHeight;
                    } else if (m_layoutStyle == FCLayoutStyle.TopToBottom) {
                        int lWidth = 0;
                        if (m_autoWrap) {
                            lWidth = size.cx;
                            int lBottom = top + margin.top + cHeight + margin.bottom;
                            if (lBottom > height) {
                                left += cWidth + margin.left + margin.right;
                                top = padding.top;
                            }
                        } else {
                            lWidth = width - margin.left - margin.right;
                        }
                        top += margin.top;
                        nTop = top;
                        nLeft = left + margin.left;
                        nWidth = lWidth;
                        top += cHeight + margin.bottom;
                    }
                    if (cLeft != nLeft || cTop != nTop || cWidth != nWidth || cHeight != nHeight) {
                        FCRect rect = new FCRect(nLeft, nTop, nLeft + nWidth, nTop + nHeight);
                        control.setBounds(rect);
                        reset = true;
                    }
                }
            }
        }
        return reset;
    }

    /**
     * 设置属性值
     *
     * @param name 属性名称
     * @param value 返回属性值
     */
    @Override
    public void setProperty(String name, String value) {
        if (name.equals("autowrap")) {
            setAutoWrap(FCStr.convertStrToBool(value));
        } else if (name.equals("layoutstyle")) {
            setLayoutStyle(FCStr.convertStrToLayoutStyle(value));
        } else {
            super.setProperty(name, value);
        }
    }

    /**
     * 布局更新方法
     */
    @Override
    public void update() {
        onResetLayout();
        int controlsSize = m_controls.size();
        for (int i = 0; i < controlsSize; i++) {
            m_controls.get(i).update();
        }
        updateScrollBar();
    }
}
