/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.div;

import facecat.topin.btn.*;
import facecat.topin.core.*;
import java.util.*;

/**
 * 分割布局控件
 */
public class FCSplitLayoutDiv extends FCLayoutDiv implements FCEvent {

    /**
     * 创建分割布局控件
     */
    public FCSplitLayoutDiv() {
    }

    protected FCSize m_oldSize;

    /**
     * 分割百分比
     */
    protected float m_splitPercent = -1;

    protected FCView m_firstControl = null;

    /**
     * 获取第一个控件
     */
    public FCView getFirstControl() {
        return m_firstControl;
    }

    /**
     * 设置第一个控件
     */
    public void setFirstControl(FCView value) {
        m_firstControl = value;
    }

    protected FCView m_secondControl = null;

    /**
     * 获取第二个控件
     */
    public FCView getSecondControl() {
        return m_secondControl;
    }

    /**
     * 设置第二个控件
     */
    public void setSecondControl(FCView value) {
        m_secondControl = value;
    }

    protected FCSizeType m_splitMode = FCSizeType.AbsoluteSize;

    /**
     * 获取分割模式
     */
    public FCSizeType getSplitMode() {
        return m_splitMode;
    }

    /**
     * 设置分割模式
     */
    public void setSplitMode(FCSizeType value) {
        m_splitMode = value;
    }

    protected FCButton m_splitter = null;

    public FCButton getSplitter() {
        return m_splitter;
    }

    public void callEvent(int eventID, Object sender) {
        if (eventID == FCEventID.DRAGGING && sender == m_splitter) {
            m_splitPercent = -1;
            update();
            invalidate();
        }
    }

    /**
     * 销毁方法
     */
    @Override
    public void delete() {
        super.delete();
    }

    /**
     * 获取控件类型
     */
    @Override
    public String getControlType() {
        return "SplitLayoutDiv";
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
        if (name.equals("candragsplitter")) {
            type.argvalue = "bool";
            if (m_splitter != null) {
                value.argvalue = FCStr.convertBoolToStr(m_splitter.allowDrag());
            } else {
                value.argvalue = "False";
            }
        } else if (name.equals("splitmode")) {
            type.argvalue = "enum:FCSizeType";
            if (getSplitMode() == FCSizeType.AbsoluteSize) {
                value.argvalue = "AbsoluteSize";
            } else {
                value.argvalue = "PercentSize";
            }
        } else if (name.indexOf("splitter-") != -1) {
            if (m_splitter != null) {
                m_splitter.getProperty(name.substring(9), value, type);
            }
        } else if (name.equals("splitterposition")) {
            type.argvalue = "str";
            if (m_splitter != null) {
                if (m_layoutStyle == FCLayoutStyle.TopToBottom || m_layoutStyle == FCLayoutStyle.BottomToTop) {
                    value.argvalue = FCStr.convertIntToStr(m_splitter.getTop());
                    if (m_splitter.getHeight() > 0) {
			value.argvalue = value.argvalue + "," + FCStr.convertIntToStr(m_splitter.getHeight());
                    }
                } else {
			value.argvalue = FCStr.convertIntToStr(m_splitter.getLeft());
                    if (m_splitter.getWidth() > 0) {
			value.argvalue = value.argvalue + "," + FCStr.convertIntToStr(m_splitter.getWidth());
                    }
                }
            }
        } else if (name.equals("splittervisible")) {
            type.argvalue = "bool";
            if (m_splitter != null) {
                value.argvalue = FCStr.convertBoolToStr(m_splitter.isVisible());
            } else {
                value.argvalue = "False";
            }
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
        propertyNames.addAll(Arrays.asList(new String[]{"CanDragSplitter", "SplitMode", "Splitter", "SplitterPosition", "SplitterVisible"}));
        return propertyNames;
    }

    /**
     * 添加控件方法
     */
    @Override
    public void onLoad() {
        super.onLoad();
        if (m_splitter == null) {
            FCHost host = getNative().getHost();
            FCView tempVar = host.createInternalControl(this, "splitter");
            m_splitter = (FCButton) ((tempVar instanceof FCButton) ? tempVar : null);
            m_splitter.addEvent(this, FCEventID.DRAGGING);
            addControl(m_splitter);
        }
        m_oldSize = getSize();
    }

    /**
     * 重置布局
     */
    @Override
    public boolean onResetLayout() {
        boolean reset = false;
        if (getNative() != null && m_splitter != null && m_firstControl != null && m_secondControl != null) {
            FCRect splitRect = new FCRect();
            int width = getWidth(), height = getHeight();
            FCRect fRect = new FCRect();
            FCRect sRect = new FCRect();
            FCSize splitterSize = new FCSize(0, 0);
            if (m_splitter.isVisible()) {
                splitterSize.cx = m_splitter.getWidth();
                splitterSize.cy = m_splitter.getHeight();
            }
            FCLayoutStyle layoutStyle = getLayoutStyle();
            switch (layoutStyle) {
                case BottomToTop:
                    if (m_splitMode == FCSizeType.AbsoluteSize || m_oldSize.cy == 0) {
                        splitRect.left = 0;
                        splitRect.top = height - (m_oldSize.cy - m_splitter.getTop());
                        splitRect.right = width;
                        splitRect.bottom = splitRect.top + splitterSize.cy;
                    } else if (m_splitMode == FCSizeType.PercentSize) {
                        splitRect.left = 0;
                        if (m_splitPercent == -1) {
                            m_splitPercent = (float) m_splitter.getTop() / m_oldSize.cy;
                        }
                        splitRect.top = (int) (height * m_splitPercent);
                        splitRect.right = width;
                        splitRect.bottom = splitRect.top + splitterSize.cy;
                    }
                    fRect.left = 0;
                    fRect.top = splitRect.bottom;
                    fRect.right = width;
                    fRect.bottom = height;
                    sRect.left = 0;
                    sRect.top = 0;
                    sRect.right = width;
                    sRect.bottom = splitRect.top;
                    break;
                case LeftToRight:
                    if (m_splitMode == FCSizeType.AbsoluteSize || m_oldSize.cx == 0) {
                        splitRect.left = m_splitter.getLeft();
                        splitRect.top = 0;
                        splitRect.right = m_splitter.getLeft() + splitterSize.cx;
                        splitRect.bottom = height;
                    } else if (m_splitMode == FCSizeType.PercentSize) {
                        if (m_splitPercent == -1) {
                            m_splitPercent = (float) m_splitter.getLeft() / m_oldSize.cx;
                        }
                        splitRect.left = (int) (width * m_splitPercent);
                        splitRect.top = 0;
                        splitRect.right = splitRect.left + splitterSize.cx;
                        splitRect.bottom = height;
                    }
                    fRect.left = 0;
                    fRect.top = 0;
                    fRect.right = splitRect.left;
                    fRect.bottom = height;
                    sRect.left = splitRect.right;
                    sRect.top = 0;
                    sRect.right = width;
                    sRect.bottom = height;
                    break;
                case RightToLeft:
                    if (m_splitMode == FCSizeType.AbsoluteSize || m_oldSize.cx == 0) {
                        splitRect.left = width - (m_oldSize.cx - m_splitter.getLeft());
                        splitRect.top = 0;
                        splitRect.right = splitRect.left + splitterSize.cx;
                        splitRect.bottom = height;
                    } else if (m_splitMode == FCSizeType.PercentSize) {
                        if (m_splitPercent == -1) {
                            m_splitPercent = (float) m_splitter.getLeft() / m_oldSize.cx;
                        }
                        splitRect.left = (int) (width * m_splitPercent);
                        splitRect.top = 0;
                        splitRect.right = splitRect.left + splitterSize.cx;
                        splitRect.bottom = height;
                    }
                    fRect.left = splitRect.right;
                    fRect.top = 0;
                    fRect.right = width;
                    fRect.bottom = height;
                    sRect.left = 0;
                    sRect.top = 0;
                    sRect.right = splitRect.left;
                    sRect.bottom = height;
                    break;
                case TopToBottom:
                    if (m_splitMode == FCSizeType.AbsoluteSize || m_oldSize.cy == 0) {
                        splitRect.left = 0;
                        splitRect.top = m_splitter.getTop();
                        splitRect.right = width;
                        splitRect.bottom = splitRect.top + splitterSize.cy;
                    } else if (m_splitMode == FCSizeType.PercentSize) {
                        splitRect.left = 0;
                        if (m_splitPercent == -1) {
                            m_splitPercent = (float) m_splitter.getTop() / m_oldSize.cy;
                        }
                        splitRect.top = (int) (height * m_splitPercent);
                        splitRect.right = width;
                        splitRect.bottom = splitRect.top + splitterSize.cy;
                    }
                    fRect.left = 0;
                    fRect.top = 0;
                    fRect.right = width;
                    fRect.bottom = splitRect.top;
                    sRect.left = 0;
                    sRect.top = splitRect.bottom;
                    sRect.right = width;
                    sRect.bottom = height;
                    break;
            }
            if (m_splitter.isVisible()) {
                FCRect spRect = m_splitter.getBounds();
                if (spRect.left != splitRect.left || spRect.top != splitRect.top || spRect.right != splitRect.right || spRect.bottom != splitRect.bottom) {
                    m_splitter.setBounds(splitRect);
                    reset = true;
                }
                if (m_splitter.allowDrag()) {
                    m_splitter.bringToFront();
                }
            }
            FCRect fcRect = m_firstControl.getBounds();
            if (fcRect.left != fRect.left || fcRect.top != fRect.top || fcRect.right != fRect.right || fcRect.bottom != fRect.bottom) {
                reset = true;
                m_firstControl.setBounds(fRect);
                m_firstControl.update();
            }
            FCRect scRect = m_secondControl.getBounds();
            if (scRect.left != sRect.left || scRect.top != sRect.top || scRect.right != sRect.right || scRect.bottom != sRect.bottom) {
                reset = true;
                m_secondControl.setBounds(sRect);
                m_secondControl.update();
            }
        }
        m_oldSize = getSize();
        return reset;
    }

    /**
     * 设置属性
     *
     * @param name 属性名称
     * @param value 属性值
     */
    @Override
    public void setProperty(String name, String value) {
        if (name.equals("candragsplitter")) {
            if (m_splitter != null) {
                m_splitter.setAllowDrag(FCStr.convertStrToBool(value));
            }
        } else if (name.equals("splitmode")) {
            value = value.toLowerCase();
            if (value.equals("absolutesize")) {
                setSplitMode(FCSizeType.AbsoluteSize);
            } else {
                setSplitMode(FCSizeType.PercentSize);
            }
        } else if (name.indexOf("splitter-") != -1) {
            if (m_splitter != null) {
                m_splitter.setProperty(name.substring(9), value);
            }
        } else if (name.equals("splitterposition")) {
            if (m_splitter != null) {
                String[] strs = value.split("[,]");
                if (strs.length == 4) {
                        m_splitter.setBounds(FCStr.convertStrToRect(value));
                } else if (strs.length <= 2) {
                        int pos = FCStr.convertStrToInt(strs[0]);
                        int lWidth = 0;
                        if (strs.length == 2) {
                                lWidth = FCStr.convertStrToInt(strs[1]);
                        }
                        int width = getWidth(), height = getHeight();
                        if (m_layoutStyle == FCLayoutStyle.TopToBottom || m_layoutStyle == FCLayoutStyle.BottomToTop) {
                                FCRect bounds = new FCRect(0, pos, width, pos + lWidth);
                                m_splitter.setBounds(bounds);
                        } else {
                                FCRect bounds = new FCRect(pos, 0, pos + lWidth, height);
                                m_splitter.setBounds(bounds);
                        }
                }
            }
        } else if (name.equals("splittervisible")) {
            if (m_splitter != null) {
                m_splitter.setVisible(FCStr.convertStrToBool(value));
            }
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
    }
}
