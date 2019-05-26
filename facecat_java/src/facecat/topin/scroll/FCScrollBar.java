/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.scroll;

import facecat.topin.btn.*;
import facecat.topin.core.*;

/**
 * 滚动条控件
 */
public class FCScrollBar extends FCView implements FCEvent, FCTouchEvent {

    /**
     * 创建滚动条
     */
    public FCScrollBar() {
        setCanFocus(false);
        setDisplayOffset(false);
        FCSize size = new FCSize(10, 10);
        setSize(size);
        setTopMost(true);
    }

    /**
     * 秒表计数
     */
    private int m_tick;

    protected int m_addSpeed;

    /**
     * 获取当前的加速度
     */
    public int getAddSpeed() {
        return m_addSpeed;
    }

    /**
     * 设置当前的加速度
     */
    public void setAddSpeed(int addSpeed) {
        m_addSpeed = addSpeed;
        if (m_addSpeed != 0) {
            startTimer(m_timerID, 10);
        } else {
            stopTimer(m_timerID);
        }
    }

    protected boolean m_isAdding = false;

    /**
     * 获取是否正在增量
     */
    public boolean isAdding() {
        return m_isAdding;
    }

    /**
     * 设置是否正在增量
     */
    public void setisAdding(boolean value) {
        if (m_isAdding != value) {
            m_isAdding = value;
            m_tick = 0;
            if (m_isAdding) {
                startTimer(m_timerID, 100);
            } else {
                stopTimer(m_timerID);
            }
        }
    }

    protected boolean m_isReducing = false;

    /**
     * 获取是否正在减量
     */
    public boolean isReducing() {
        return m_isReducing;
    }

    /**
     * 设置是否正在减量
     */
    public void setisReducing(boolean value) {
        if (m_isReducing != value) {
            m_isReducing = value;
            m_tick = 0;
            if (m_isReducing) {
                startTimer(m_timerID, 100);
            } else {
                stopTimer(m_timerID);
            }
        }
    }

    /**
     * 秒表ID
     */
    private int m_timerID = getNewTimerID();

    protected FCButton m_addButton = null;

    /**
     * 增量按钮
     */
    public FCButton getAddButton() {
        return m_addButton;
    }

    protected FCButton m_backButton = null;

    /**
     * 获取滚动背景按钮
     */
    public FCButton getBackButton() {
        return m_backButton;
    }

    protected int m_contentSize = 0;

    /**
     * 获取内容尺寸
     */
    public int getContentSize() {
        return m_contentSize;
    }

    /**
     * 设置内容尺寸
     */
    public void setContentSize(int value) {
        m_contentSize = value;
    }

    protected int m_lineSize = 10;

    /**
     * 获取每次滚动的行尺寸
     */
    public int getLineSize() {
        return m_lineSize;
    }

    /**
     * 每次滚动的行尺寸
     */
    public void setLineSize(int value) {
        m_lineSize = value;
    }

    protected int m_pageSize;

    /**
     * 获取页的尺寸
     */
    public int getPageSize() {
        return m_pageSize;
    }

    /**
     * 设置页的尺寸
     */
    public void setPageSize(int value) {
        m_pageSize = value;
    }

    protected int m_pos;

    /**
     * 获取滚动距离
     */
    public int getPos() {
        return m_pos;
    }

    /**
     * 设置滚动距离
     */
    public void setPos(int value) {
        m_pos = value;
        if (m_pos > m_contentSize - m_pageSize) {
            m_pos = m_contentSize - m_pageSize;
        }
        if (m_pos < 0) {
            m_pos = 0;
        }
    }

    protected FCButton m_reduceButton = null;

    /**
     * 减量按钮
     */
    public FCButton getReduceButton() {
        return m_reduceButton;
    }

    protected FCButton m_scrollButton = null;

    /**
     * 获取滚动按钮
     */
    public FCButton getScrollButton() {
        return m_scrollButton;
    }

    public void callEvent(int eventID, Object sender) {
        if (sender == m_scrollButton) {
            if (eventID == FCEventID.DRAGGING) {
                onDragScroll();
            }
        }
    }

    public void callControlTouchEvent(int eventID, Object sender, FCTouchInfo touchInfo) {
        if (sender == m_addButton) {
            if (eventID == FCEventID.TOUCHDOWN) {
                onAddButtonTouchDown(touchInfo.clone());
            } else if (eventID == FCEventID.TOUCHUP) {
                onAddButtonTouchUp(touchInfo.clone());
            }
        } else if (sender == m_reduceButton) {
            if (eventID == FCEventID.TOUCHDOWN) {
                onReduceButtonTouchDown(touchInfo.clone());
            } else if (eventID == FCEventID.TOUCHUP) {
                onreduceButtonTouchUp(touchInfo.clone());
            }
        }
    }

    /**
     * 销毁方法
     */
    @Override
    public void delete() {
        if (!isDeleted()) {
            stopTimer(m_timerID);
            if (m_addButton != null) {
                m_addButton.removeEvent(this, FCEventID.TOUCHDOWN);
                m_addButton.removeEvent(this, FCEventID.TOUCHUP);
            }
            if (m_scrollButton != null) {
                m_scrollButton.removeEvent(this, FCEventID.DRAGGING);
            }
            if (m_reduceButton != null) {
                m_reduceButton.removeEvent(this, FCEventID.TOUCHDOWN);
                m_reduceButton.removeEvent(this, FCEventID.TOUCHUP);
            }
        }
        super.delete();
    }

    /**
     * 获取控件类型
     */
    @Override
    public String getControlType() {
        return "ScrollBar";
    }

    /**
     * 行变大方法
     */
    public void lineadd() {
        m_pos += m_lineSize;
        if (m_pos > m_contentSize - m_pageSize) {
            m_pos = m_contentSize - m_pageSize;
        }
        update();
        onScrolled();
    }

    /**
     * 行变小方法
     */
    public void linereduce() {
        m_pos -= m_lineSize;
        if (m_pos < 0) {
            m_pos = 0;
        }
        update();
        onScrolled();
    }

    /**
     * 增量滚动按钮按下事件方法
     *
     * @param touchInfo 触摸信息
     */
    public void onAddButtonTouchDown(FCTouchInfo touchInfo) {
        lineadd();
        setisAdding(true);
    }

    /**
     * 增量滚动按钮抬起事件方法
     *
     * @param touchInfo 触摸信息
     */
    public void onAddButtonTouchUp(FCTouchInfo touchInfo) {
        setisAdding(false);
    }

    /**
     * 加速滚动结束
     */
    public void onAddSpeedScrollEnd() {
    }

    /**
     * 自动加速滚动开始
     */
    public void onAddSpeedScrollStart(long startTime, long nowTime, int start, int end) {
        int diff = (int) ((nowTime - startTime) / 1000000);
        if (diff > 0 && diff < 800) {
            int sub = 10000 * (Math.abs(start - end) / 20) / diff;
            if (start > end) {
                setAddSpeed(getAddSpeed() + sub);
            } else if (start < end) {
                setAddSpeed(getAddSpeed() - sub);
            }
        }
    }

    /**
     * 自动加速滚动中
     */
    public int onAddSpeedScrolling() {
        int sub = m_addSpeed / 10;
        if (sub == 0) {
            sub = m_addSpeed > 0 ? 1 : -1;
        }
        return sub;
    }

    /**
     * 拖动滚动方法
     */
    public void onDragScroll() {
        if (m_scrollButton.getLeft() < 0) {
            m_scrollButton.setLeft(0);
        }
        if (m_scrollButton.getTop() < 0) {
            m_scrollButton.setTop(0);
        }
        if (m_scrollButton.getRight() > m_backButton.getWidth()) {
            m_scrollButton.setLeft(m_backButton.getWidth() - m_scrollButton.getWidth());
        }
        if (m_scrollButton.getBottom() > m_backButton.getHeight()) {
            m_scrollButton.setTop(m_backButton.getHeight() - m_scrollButton.getHeight());
        }
    }

    /**
     * 控件添加方法
     */
    @Override
    public void onLoad() {
        super.onLoad();
        // 添加按钮
        FCHost host = getNative().getHost();
        if (m_addButton == null) {
            FCView tempVar = host.createInternalControl(this, "addbutton");
            m_addButton = (FCButton) ((tempVar instanceof FCButton) ? tempVar : null);
            m_addButton.addEvent(this, FCEventID.TOUCHDOWN);
            m_addButton.addEvent(this, FCEventID.TOUCHUP);
            addControl(m_addButton);
        }
        if (m_backButton == null) {
            FCView tempVar2 = host.createInternalControl(this, "backbutton");
            m_backButton = (FCButton) ((tempVar2 instanceof FCButton) ? tempVar2 : null);
            addControl(m_backButton);
        }
        if (m_reduceButton == null) {
            FCView tempVar3 = host.createInternalControl(this, "reducebutton");
            m_reduceButton = (FCButton) ((tempVar3 instanceof FCButton) ? tempVar3 : null);
            m_reduceButton.addEvent(this, FCEventID.TOUCHDOWN);
            m_reduceButton.addEvent(this, FCEventID.TOUCHUP);
            addControl(m_reduceButton);
        }
        if (m_scrollButton == null) {
            FCView tempVar4 = host.createInternalControl(this, "scrollbutton");
            m_scrollButton = (FCButton) ((tempVar4 instanceof FCButton) ? tempVar4 : null);
            m_scrollButton.setAllowDrag(true);
            m_scrollButton.addEvent(this, FCEventID.DRAGGING);
            m_backButton.addControl(m_scrollButton);
        }
    }

    /**
     * 减量滚动按钮按下事件方法
     *
     * @param touchInfo 触摸信息
     */
    public void onReduceButtonTouchDown(FCTouchInfo touchInfo) {
        linereduce();
        setisReducing(true);
    }

    /**
     * 减量滚动按钮抬起事件回调方法
     *
     * @param touchInfo 触摸信息
     */
    public void onreduceButtonTouchUp(FCTouchInfo touchInfo) {
        setisReducing(false);
    }

    /**
     * 滚动方法
     */
    public void onScrolled() {
        callEvents(FCEventID.SCROLLED);
        FCView parent = getParent();
        if (parent != null) {
            parent.invalidate();
        }
    }

    /**
     * 秒表回调方法
     *
     * @param timerID 秒表ID
     */
    @Override
    public void onTimer(int timerID) {
        super.onTimer(timerID);
        if (m_timerID == timerID) {
            if (m_isAdding) {
                if (m_tick > 5) {
                    pageadd();
                } else {
                    lineadd();
                }
            }
            if (m_isReducing) {
                if (m_tick > 5) {
                    pagereduce();
                } else {
                    linereduce();
                }
            }
            if (m_addSpeed != 0) {
                int sub = onAddSpeedScrolling();
                setPos(getPos() + sub);
                update();
                onScrolled();
                m_addSpeed -= sub;
                if (Math.abs(m_addSpeed) < 3) {
                    m_addSpeed = 0;
                }
                if (m_addSpeed == 0) {
                    onAddSpeedScrollEnd();
                    stopTimer(m_timerID);
                    if (getParent() != null) {
                        getParent().invalidate();
                    }
                }
            }
            m_tick++;
        }
    }

    /**
     * 可见状态改变方法
     */
    @Override
    public void onVisibleChanged() {
        super.onVisibleChanged();
        if (!isVisible()) {
            m_pos = 0;
        }
    }

    /**
     * 页变大方法
     */
    public void pageadd() {
        m_pos += m_pageSize;
        if (m_pos > m_contentSize - m_pageSize) {
            m_pos = m_contentSize - m_pageSize;
        }
        update();
        onScrolled();
    }

    /**
     * 页变小方法
     */
    public void pagereduce() {
        m_pos -= m_pageSize;
        if (m_pos < 0) {
            m_pos = 0;
        }
        update();
        onScrolled();
    }

    /**
     * 减量滚动按钮按下事件回调方法
     *
     * @param sender 调用者
     * @param touchInfo 触摸信息
     */
    public void reduceButtonTouchDown(Object sender, FCTouchInfo touchInfo) {
        onReduceButtonTouchDown(touchInfo.clone());
    }

    /**
     * 减量滚动按钮抬起事件回调方法
     *
     * @param sender 调用者
     * @param touchInfo 触摸信息
     */
    public void reduceButtonTouchUp(Object sender, FCTouchInfo touchInfo) {
        onreduceButtonTouchUp(touchInfo.clone());
    }

    /**
     * 滚动到开始
     */
    public void scrollToBegin() {
        m_pos = 0;
        update();
        onScrolled();
    }

    /**
     * 滚动到结束
     */
    public void scrollToEnd() {
        m_pos = m_contentSize - m_pageSize;
        if (m_pos < 0) {
            m_pos = 0;
        }
        update();
        onScrolled();
    }
}
