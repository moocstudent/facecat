/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.div;

import facecat.topin.scroll.*;
import facecat.topin.core.*;
import facecat.topin.grid.*;
import java.util.*;

/**
 * 图层
 */
public class FCDiv extends FCView implements FCEvent {

    /**
     * 创建支持滚动条的控件
     */
    public FCDiv() {
        FCSize size = new FCSize(200, 200);
        setSize(size);
    }

    /**
     * 是否正在滚动2
     */
    private boolean m_isDragScrolling2;

    /**
     * 是否准备拖动滚动
     */
    private boolean m_readyToDragScroll;

    /**
     * 开始移动的位置
     */
    private FCPoint m_startMovePoint;

    /**
     * 开始移动的横向位置
     */
    private int m_startMovePosX;

    /**
     * 开始移动的纵向位置
     */
    private int m_startMovePosY;

    /**
     * 开始移动时间
     */
    private long m_startMoveTime;

    protected boolean m_allowDragScroll;

    /**
     * 获取是否允许拖动滚动
     */
    public boolean allowDragScroll() {
        return m_allowDragScroll;
    }

    /**
     * 设置是否允许拖动滚动
     */
    public void setAllowDragScroll(boolean value) {
        m_allowDragScroll = value;
    }

    protected FCHScrollBar m_hScrollBar = null;

    /**
     * 获取横向滚动条
     */
    public FCHScrollBar getHScrollBar() {
        if (getNative() != null && m_showHScrollBar) {
            if (m_hScrollBar == null) {
                FCHost host = getNative().getHost();
                FCView tempVar = host.createInternalControl(this, "hscrollbar");
                m_hScrollBar = (FCHScrollBar) ((tempVar instanceof FCHScrollBar) ? tempVar : null);
                addControl(m_hScrollBar);
            }
            return m_hScrollBar;
        }
        return null;
    }

    protected boolean m_showHScrollBar = false;

    /**
     * 获取是否显示横向滚动条
     */
    public boolean showHScrollBar() {
        return m_showHScrollBar;
    }

    /**
     * 设置是否显示横向滚动条
     */
    public void setShowHScrollBar(boolean value) {
        m_showHScrollBar = value;
    }

    private boolean m_isDragScrolling;

    /**
     * 获取是否正在被拖动
     */
    public boolean isDragScrolling() {
        return m_isDragScrolling;
    }

    protected boolean m_showVScrollBar = false;

    /**
     * 获取是否显示纵向滚动条
     */
    public boolean showVScrollBar() {
        return m_showVScrollBar;
    }

    /**
     * 设置是否显示纵向滚动条
     */
    public void setShowVScrollBar(boolean value) {
        m_showVScrollBar = value;
    }

    protected FCVScrollBar m_vScrollBar = null;

    /**
     * 获取纵向滚动条
     */
    public FCVScrollBar getVScrollBar() {
        if (getNative() != null && m_showVScrollBar) {
            if (m_vScrollBar == null) {
                FCHost host = getNative().getHost();
                FCView tempVar = host.createInternalControl(this, "vscrollbar");
                m_vScrollBar = (FCVScrollBar) ((tempVar instanceof FCVScrollBar) ? tempVar : null);
                addControl(m_vScrollBar);
            }
            return m_vScrollBar;
        }
        return null;
    }

    public void callEvent(int eventID, Object sender) {

    }

    /**
     * 销毁方法
     */
    @Override
    public void delete() {
        super.delete();
    }

    /**
     * 获取内容的高度
     *
     * @returns 高度
     */
    public int getContentHeight() {
        FCHScrollBar hScrollBar = getHScrollBar();
        FCVScrollBar vScrollBar = getVScrollBar();
        int hmax = 0;
        ArrayList<FCView> controls = getControls();
        int controlSize = controls.size();
        for (int i = 0; i < controlSize; i++) {
            FCView control = controls.get(i);
            if (control.isVisible() && control != hScrollBar && control != vScrollBar) {
                int bottom = control.getBottom();
                if (bottom > hmax) {
                    hmax = bottom;
                }
            }
        }
        return hmax;
    }

    /**
     * 获取内容的宽度
     *
     * @returns 宽度
     */
    public int getContentWidth() {
        FCHScrollBar hScrollBar = getHScrollBar();
        FCVScrollBar vScrollBar = getVScrollBar();
        int wmax = 0;
        ArrayList<FCView> controls = getControls();
        int controlSize = controls.size();
        for (int i = 0; i < controlSize; i++) {
            FCView control = controls.get(i);
            if (control.isVisible() && control != hScrollBar && control != vScrollBar) {
                int right = control.getRight();
                if (right > wmax) {
                    wmax = right;
                }
            }
        }
        return wmax;
    }

    /**
     * 获取控件类型
     *
     * @returns 控件类型
     */
    @Override
    public String getControlType() {
        return "Div";
    }

    /**
     * 获取显示偏移坐标
     *
     * @returns 坐标
     */
    @Override
    public FCPoint getDisplayOffset() {
        FCPoint offset = new FCPoint();
        if (isVisible()) {
            offset.x = (m_hScrollBar != null && m_hScrollBar.isVisible()) ? m_hScrollBar.getPos() : 0;
            offset.y = (m_vScrollBar != null && m_vScrollBar.isVisible()) ? m_vScrollBar.getPos() : 0;
        }
        return offset;
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
        if (name.equals("allowdragscroll")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(allowDragScroll());
        }
        if (name.equals("showhscrollbar")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(showHScrollBar());
        } else if (name.equals("showvscrollbar")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(showVScrollBar());
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
        propertyNames.addAll(Arrays.asList(new String[]{"AllowDragScroll", "ShowHScrollBar", "ShowVScrollBar"}));
        return propertyNames;
    }

    /**
     * 向下滚动一行
     */
    public void lineDown() {
        if (m_vScrollBar != null && m_vScrollBar.isVisible()) {
            m_vScrollBar.lineadd();
        }
    }

    /**
     * 向左滚动一行
     */
    public void lineLeft() {
        if (m_hScrollBar != null && m_hScrollBar.isVisible()) {
            m_hScrollBar.linereduce();
        }
    }

    /**
     * 向右滚动一行
     */
    public void lineRight() {
        if (m_hScrollBar != null && m_hScrollBar.isVisible()) {
            m_hScrollBar.lineadd();
        }
    }

    /**
     * 向上滚动一行
     */
    public void lineUp() {
        if (m_vScrollBar != null && m_vScrollBar.isVisible()) {
            m_vScrollBar.linereduce();
        }
    }

    /**
     * 拖动准备方法
     */
    @Override
    public void onDragReady(RefObject<FCPoint> startOffset) {
        startOffset.argvalue.x = 0;
        startOffset.argvalue.y = 0;
    }

    /**
     * 拖动滚动结束
     */
    public void onDragScrollEnd() {
        m_isDragScrolling = false;
        if (m_readyToDragScroll) {
            long nowTime = System.nanoTime();
            FCPoint newPoint = getNative().getTouchPoint();
            if (m_hScrollBar != null && m_hScrollBar.isVisible()) {
                m_hScrollBar.onAddSpeedScrollStart(m_startMoveTime, nowTime, m_startMovePoint.x, newPoint.x);
            }
            if (m_vScrollBar != null && m_vScrollBar.isVisible()) {
                m_vScrollBar.onAddSpeedScrollStart(m_startMoveTime, nowTime, m_startMovePoint.y, newPoint.y);
            }
            m_readyToDragScroll = false;
            invalidate();
        }
    }

    /**
     * 拖动滚动中
     */
    public void onDragScrolling() {
        int width = getWidth(), height = getHeight();
        if (m_allowDragScroll && m_readyToDragScroll) {
            if (!onDragScrollPermit()) {
                m_readyToDragScroll = false;
                return;
            }
            boolean paint = false;
            FCPoint newPoint = getNative().getTouchPoint();
            if (m_hScrollBar != null && m_hScrollBar.isVisible()) {
                if (Math.abs(newPoint.x - m_startMovePoint.x) > width / 10) {
                    m_isDragScrolling2 = true;
                }
                int newPos = m_startMovePosX + m_startMovePoint.x - newPoint.x;
                if (newPos != m_hScrollBar.getPos()) {
                    m_hScrollBar.setPos(m_startMovePosX + m_startMovePoint.x - newPoint.x);
                    m_hScrollBar.update();
                    paint = true;
                }
            }
            if (m_vScrollBar != null && m_vScrollBar.isVisible()) {
                if (Math.abs(newPoint.y - m_startMovePoint.y) > height / 10) {
                    m_isDragScrolling2 = true;
                }
                int newPos = m_startMovePosY + m_startMovePoint.y - newPoint.y;
                if (newPos != m_vScrollBar.getPos()) {
                    m_vScrollBar.setPos(m_startMovePosY + m_startMovePoint.y - newPoint.y);
                    m_vScrollBar.update();
                    paint = true;
                }
            }
            if (paint) {
                m_isDragScrolling = true;
                invalidate();
            }
        }
    }

    /**
     * 拖动滚动许可检查
     */
    public boolean onDragScrollPermit() {
        FCView focusedControl = getNative().getFocusedControl();
        if (focusedControl != null) {
            if (focusedControl.isDragging()) {
                return false;
            }
            FCGridColumn column = (FCGridColumn) ((focusedControl instanceof FCGridColumn) ? focusedControl : null);
            if (column != null) {
                return false;
            }
            if (focusedControl.getParent() != null) {
                FCScrollBar scrollBar = (FCScrollBar) ((focusedControl.getParent() instanceof FCScrollBar) ? focusedControl.getParent() : null);
                if (scrollBar != null) {
                    return false;
                }
            }
        }
        return true;
    }

    /**
     * 拖动滚动开始
     */
    public void onDragScrollstart() {
        m_isDragScrolling = false;
        m_isDragScrolling2 = false;
        FCView focusedControl = getNative().getFocusedControl();
        if (m_hScrollBar != null && m_hScrollBar.isVisible()) {
            if (focusedControl == m_hScrollBar.getAddButton()
                    || focusedControl == m_hScrollBar.getReduceButton()
                    || focusedControl == m_hScrollBar.getBackButton()
                    || focusedControl == m_hScrollBar.getScrollButton()) {
                m_hScrollBar.setAddSpeed(0);
                return;
            }
        }
        if (m_vScrollBar != null && m_vScrollBar.isVisible()) {
            if (focusedControl == m_vScrollBar.getAddButton()
                    || focusedControl == m_vScrollBar.getReduceButton()
                    || focusedControl == m_vScrollBar.getBackButton()
                    || focusedControl == m_vScrollBar.getScrollButton()) {
                m_vScrollBar.setAddSpeed(0);
                return;
            }
        }
        if (m_allowDragScroll) {
            if (m_hScrollBar != null && m_hScrollBar.isVisible()) {
                m_startMovePosX = m_hScrollBar.getPos();
                m_hScrollBar.setAddSpeed(0);
                m_readyToDragScroll = true;
            }
            if (m_vScrollBar != null && m_vScrollBar.isVisible()) {
                m_startMovePosY = m_vScrollBar.getPos();
                m_vScrollBar.setAddSpeed(0);
                m_readyToDragScroll = true;
            }
            if (m_readyToDragScroll) {
                m_startMovePoint = getNative().getTouchPoint();
                m_startMoveTime = System.nanoTime();
            }
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
        if (!m_allowPreviewsEvent) {
            onDragScrollstart();
        }
    }

    /**
     * 触摸按下方法
     *
     * @param touchInfo 触摸信息
     */
    @Override
    public void onTouchMove(FCTouchInfo touchInfo) {
        super.onTouchMove(touchInfo.clone());
        if (!m_allowPreviewsEvent) {
            onDragScrolling();
        }
    }

    /**
     * 触摸抬起方法
     *
     * @param touchInfo 触摸信息
     */
    @Override
    public void onTouchUp(FCTouchInfo touchInfo) {
        super.onTouchUp(touchInfo.clone());
        if (!m_allowPreviewsEvent) {
            onDragScrollEnd();
        }
    }

    /**
     * 预处理触摸事件
     *
     * @param eventID 事件ID
     * @param touchInfo 触摸信息
     * @returns 状态
     */
    @Override
    public boolean onPreviewsTouchEvent(int eventID, FCTouchInfo touchInfo) {
        if (callPreviewsTouchEvents(FCEventID.PREVIEWSTOUCHEVENT, eventID, touchInfo.clone())) {
            return true;
        }
        if (m_allowPreviewsEvent) {
            if (eventID == FCEventID.TOUCHDOWN) {
                onDragScrollstart();
            } else if (eventID == FCEventID.TOUCHMOVE) {
                onDragScrolling();
            } else if (eventID == FCEventID.TOUCHUP) {
                boolean state = m_isDragScrolling;
                onDragScrollEnd();
                if (state && !m_isDragScrolling2) {
                    return false;
                }
            }
        }
        return false;
    }

    /**
     * 向下翻一页
     */
    public void pageDown() {
        if (m_vScrollBar != null && m_vScrollBar.isVisible()) {
            m_vScrollBar.pageadd();
        }
    }

    /**
     * 向左翻一页
     */
    public void pageLeft() {
        if (m_hScrollBar != null && m_hScrollBar.isVisible()) {
            m_hScrollBar.pagereduce();
        }
    }

    /**
     * 向右翻一页
     */
    public void pageRight() {
        if (m_hScrollBar != null && m_hScrollBar.isVisible()) {
            m_hScrollBar.pageadd();
        }
    }

    /**
     * 向上翻一页
     */
    public void pageUp() {
        if (m_vScrollBar != null && m_vScrollBar.isVisible()) {
            m_vScrollBar.pagereduce();
        }
    }

    /**
     * 设置属性值
     *
     * @param name 属性名称
     * @param value 返回属性值
     */
    @Override
    public void setProperty(String name, String value) {
        if (name.equals("allowdragscroll")) {
            setAllowDragScroll(FCStr.convertStrToBool(value));
        }
        if (name.equals("showhscrollbar")) {
            setShowHScrollBar(FCStr.convertStrToBool(value));
        } else if (name.equals("showvscrollbar")) {
            setShowVScrollBar(FCStr.convertStrToBool(value));
        } else {
            super.setProperty(name, value);
        }
    }

    /**
     * 更新布局方法
     */
    @Override
    public void update() {
        super.update();
        updateScrollBar();
    }

    /**
     * 更新滚动条的布局
     */
    public void updateScrollBar() {
        if (getNative() != null) {
            FCHScrollBar hScrollBar = getHScrollBar();
            FCVScrollBar vScrollBar = getVScrollBar();
            if (isVisible()) {
                int width = getWidth(), height = getHeight();
                // 更新滚动条的布局
                int hBarHeight = (hScrollBar != null) ? hScrollBar.getHeight() : 0;
                int vBarWidth = (vScrollBar != null) ? vScrollBar.getWidth() : 0;
                int wmax = getContentWidth(), hmax = getContentHeight();
                if (hScrollBar != null) {
                    hScrollBar.setContentSize(wmax);
                    hScrollBar.setSize(new FCSize(width - vBarWidth, hBarHeight));
                    hScrollBar.setPageSize(width - vBarWidth);
                    hScrollBar.setLocation(new FCPoint(0, height - hBarHeight));
                    if (wmax <= width) {
                        hScrollBar.setVisible(false);
                    } else {
                        hScrollBar.setVisible(true);
                    }
                }
                if (vScrollBar != null) {
                    vScrollBar.setContentSize(hmax);
                    vScrollBar.setSize(new FCSize(vBarWidth, height - hBarHeight));
                    vScrollBar.setPageSize(height - hBarHeight);
                    vScrollBar.setLocation(new FCPoint(width - vBarWidth, 0));
                    int vh = (hScrollBar != null && hScrollBar.isVisible()) ? height - hBarHeight : height;
                    if (hmax <= vh) {
                        vScrollBar.setVisible(false);
                    } else {
                        vScrollBar.setVisible(true);
                    }
                }
                if (hScrollBar != null && vScrollBar != null) {
                    if (hScrollBar.isVisible() && !vScrollBar.isVisible()) {
                        hScrollBar.setWidth(width);
                        hScrollBar.setPageSize(width);
                    } else if (!hScrollBar.isVisible() && vScrollBar.isVisible()) {
                        vScrollBar.setHeight(height);
                        vScrollBar.setPageSize(height);
                    }
                }
                if (hScrollBar != null && hScrollBar.isVisible()) {
                    hScrollBar.update();
                }
                if (vScrollBar != null && vScrollBar.isVisible()) {
                    vScrollBar.update();
                }
            }
        }
    }
}
