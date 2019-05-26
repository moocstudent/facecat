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
    /// 滚动条控件
    /// </summary>
    public class FCScrollBar : FCView {
        /// <summary>
        /// 创建滚动条
        /// </summary>
        public FCScrollBar() {
            m_addButtonTouchDownEvent = new FCTouchEvent(addButtonTouchDown);
            m_addButtonTouchUpEvent = new FCTouchEvent(addButtonTouchUp);
            m_scrollButtonDraggingEvent = new FCEvent(scrollButtonDragging);
            m_reduceButtonTouchDownEvent = new FCTouchEvent(reduceButtonTouchDown);
            m_reduceButtonTouchUpEvent = new FCTouchEvent(reduceButtonTouchUp);
            CanFocus = false;
            DisplayOffset = false;
            FCSize size = new FCSize(10, 10);
            Size = size;
            TopMost = true;
        }

        /// <summary>
        /// 增量按钮的按下事件
        /// </summary>
        private FCTouchEvent m_addButtonTouchDownEvent;

        /// <summary>
        /// 增量按钮的抬起事件
        /// </summary>
        private FCTouchEvent m_addButtonTouchUpEvent;

        protected int m_addSpeed;

        /// <summary>
        /// 获取或设置当前的加速度
        /// </summary>
        public virtual int AddSpeed {
            get { return m_addSpeed; }
            set {
                m_addSpeed = value;
                if (m_addSpeed != 0) {
                    startTimer(m_timerID, 10);
                }
                else {
                    stopTimer(m_timerID);
                }
            }
        }

        protected bool m_isAdding;

        /// <summary>
        /// 获取或设置是否正在增量
        /// </summary>
        public virtual bool IsAdding {
            get { return m_isAdding; }
            set {
                m_isAdding = value;
                m_tick = 0;
                if (m_isAdding) {
                    startTimer(m_timerID, 100);
                }
                else {
                    stopTimer(m_timerID);
                }
            }
        }

        protected bool m_isReducing;

        /// <summary>
        /// 获取或设置是否正在减量
        /// </summary>
        public virtual bool IsReducing {
            get { return m_isReducing; }
            set {
                m_isReducing = value;
                m_tick = 0;
                if (m_isReducing) {
                    startTimer(m_timerID, 100);
                }
                else {
                    stopTimer(m_timerID);
                }
            }
        }

        /// <summary>
        /// 减量按钮的按下事件
        /// </summary>
        private FCTouchEvent m_reduceButtonTouchDownEvent;

        /// <summary>
        /// 减量按钮的抬起事件
        /// </summary>
        private FCTouchEvent m_reduceButtonTouchUpEvent;

        /// <summary>
        /// 滚动按钮拖拽事件
        /// </summary>
        private FCEvent m_scrollButtonDraggingEvent;

        /// <summary>
        /// 秒表计数
        /// </summary>
        private int m_tick;

        /// <summary>
        /// 秒表ID
        /// </summary>
        private int m_timerID = getNewTimerID();

        protected FCButton m_addButton;

        /// <summary>
        /// 增量按钮
        /// </summary>
        public virtual FCButton AddButton {
            get { return m_addButton; }
        }

        protected FCButton m_backButton;

        /// <summary>
        /// 获取滚动背景按钮
        /// </summary>
        public virtual FCButton BackButton {
            get { return m_backButton; }
        }

        protected int m_contentSize = 0;

        /// <summary>
        /// 获取或设置内容尺寸
        /// </summary>
        public virtual int ContentSize {
            get { return m_contentSize; }
            set { m_contentSize = value; }
        }

        protected int m_lineSize = 10;

        /// <summary>
        /// 每次滚动的行尺寸
        /// </summary>
        public virtual int LineSize {
            get { return m_lineSize; }
            set { m_lineSize = value; }
        }

        protected int m_pageSize;

        /// <summary>
        /// 获取或设置页的尺寸
        /// </summary>
        public virtual int PageSize {
            get { return m_pageSize; }
            set { m_pageSize = value; }
        }

        protected int m_pos;

        /// <summary>
        /// 获取或设置滚动距离
        /// </summary>
        public virtual int Pos {
            get { return m_pos; }
            set {
                m_pos = value;
                if (m_pos > m_contentSize - m_pageSize) {
                    m_pos = m_contentSize - m_pageSize;
                }
                if (m_pos < 0) {
                    m_pos = 0;
                }
            }
        }

        protected FCButton m_reduceButton;

        /// <summary>
        /// 减量按钮
        /// </summary>
        public virtual FCButton ReduceButton {
            get { return m_reduceButton; }
        }

        protected FCButton m_scrollButton;

        /// <summary>
        /// 获取滚动按钮
        /// </summary>
        public virtual FCButton ScrollButton {
            get { return m_scrollButton; }
        }

        /// <summary>
        /// 增量滚动按钮按下事件回调事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="touchInfo">触摸信息</param>
        public void addButtonTouchDown(object sender, FCTouchInfo touchInfo) {
            onAddButtonTouchDown(touchInfo);
        }

        /// <summary>
        /// 增量滚动按钮抬起事件回调事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="touchInfo">触摸信息</param>
        public void addButtonTouchUp(object sender, FCTouchInfo touchInfo) {
            onAddButtonTouchUp(touchInfo);
        }

        /// <summary>
        /// 销毁方法
        /// </summary>
        public override void delete() {
            if (!IsDeleted) {
                stopTimer(m_timerID);
                if (m_addButton != null) {
                    if (m_addButtonTouchDownEvent != null) {
                        m_addButton.removeEvent(m_addButtonTouchDownEvent, FCEventID.TOUCHDOWN);
                        m_addButtonTouchDownEvent = null;
                    }
                    if (m_addButtonTouchUpEvent != null) {
                        m_addButton.removeEvent(m_addButtonTouchUpEvent, FCEventID.TOUCHUP);
                        m_addButtonTouchUpEvent = null;
                    }
                }
                if (m_scrollButton != null) {
                    if (m_scrollButtonDraggingEvent != null) {
                        m_scrollButton.removeEvent(m_scrollButtonDraggingEvent, FCEventID.DRAGGING);
                        m_scrollButtonDraggingEvent = null;
                    }
                }
                if (m_reduceButton != null) {
                    if (m_reduceButtonTouchDownEvent != null) {
                        m_reduceButton.removeEvent(m_reduceButtonTouchDownEvent, FCEventID.TOUCHDOWN);
                        m_reduceButtonTouchDownEvent = null;
                    }
                    if (m_reduceButtonTouchUpEvent != null) {
                        m_reduceButton.removeEvent(m_reduceButtonTouchUpEvent, FCEventID.TOUCHUP);
                        m_reduceButtonTouchUpEvent = null;
                    }
                }
            }
            base.delete();
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <returns>控件类型</returns>
        public override String getControlType() {
            return "ScrollBar";
        }

        /// <summary>
        /// 获取事件名称列表
        /// </summary>
        /// <returns>名称列表</returns>
        public override ArrayList<String> getEventNames() {
            ArrayList<String> eventNames = base.getEventNames();
            eventNames.AddRange(new String[] { "Scrolled" });
            return eventNames;
        }

        /// <summary>
        /// 行变大方法
        /// </summary>
        public virtual void lineAdd() {
            m_pos += m_lineSize;
            if (m_pos > m_contentSize - m_pageSize) {
                m_pos = m_contentSize - m_pageSize;
            }
            update();
            onScrolled();
        }

        /// <summary>
        /// 行变小方法
        /// </summary>
        public virtual void lineReduce() {
            m_pos -= m_lineSize;
            if (m_pos < 0) {
                m_pos = 0;
            }
            update();
            onScrolled();
        }

        /// <summary>
        /// 增量滚动按钮按下事件方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public void onAddButtonTouchDown(FCTouchInfo touchInfo) {
            lineAdd();
            IsAdding = true;
        }

        /// <summary>
        /// 增量滚动按钮抬起事件方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public void onAddButtonTouchUp(FCTouchInfo touchInfo) {
            IsAdding = false;
        }

        /// <summary>
        /// 加速滚动结束
        /// </summary>
        public virtual void onAddSpeedScrollEnd() {
        }

        /// <summary>
        /// 自动加速滚动开始
        /// </summary>
        public virtual void onAddSpeedScrollStart(long startTime, long nowTime, int start, int end) {
            int diff = (int)((nowTime - startTime) / 10000);
            if (diff > 0 && diff < 800) {
                int sub = 5000 * (Math.Abs(start - end) / 20) / diff;
                if (start > end) {
                    AddSpeed += sub;
                }
                else if (start < end) {
                    AddSpeed += -sub;
                }
            }
        }

        /// <summary>
        /// 自动加速滚动中
        /// </summary>
        public virtual int onAddSpeedScrolling() {
            int sub = m_addSpeed / 10;
            if (sub == 0) {
                sub = m_addSpeed > 0 ? 1 : -1;
            }
            return sub;
        }

        /// <summary>
        /// 拖动滚动方法
        /// </summary>
        public virtual void onDragScroll() {
            if (m_scrollButton.Left < 0) {
                m_scrollButton.Left = 0;
            }
            if (m_scrollButton.Top < 0) {
                m_scrollButton.Top = 0;
            }
            if (m_scrollButton.Right > m_backButton.Width) {
                m_scrollButton.Left = m_backButton.Width - m_scrollButton.Width;
            }
            if (m_scrollButton.Bottom > m_backButton.Height) {
                m_scrollButton.Top = m_backButton.Height - m_scrollButton.Height;
            }
        }

        /// <summary>
        /// 控件添加方法
        /// </summary>
        public override void onLoad() {
            base.onLoad();
            //添加按钮
            FCHost host = Native.Host;
            if (m_addButton == null) {
                m_addButton = host.createInternalControl(this, "addbutton") as FCButton;
                m_addButton.addEvent(m_addButtonTouchDownEvent, FCEventID.TOUCHDOWN);
                m_addButton.addEvent(m_addButtonTouchUpEvent, FCEventID.TOUCHUP);
                addControl(m_addButton);
            }
            if (m_backButton == null) {
                m_backButton = host.createInternalControl(this, "backbutton") as FCButton;
                addControl(m_backButton);
            }
            if (m_reduceButton == null) {
                m_reduceButton = host.createInternalControl(this, "reducebutton") as FCButton;
                m_reduceButton.addEvent(m_reduceButtonTouchDownEvent, FCEventID.TOUCHDOWN);
                m_reduceButton.addEvent(m_reduceButtonTouchUpEvent, FCEventID.TOUCHUP);
                addControl(m_reduceButton);
            }
            if (m_scrollButton == null) {
                m_scrollButton = host.createInternalControl(this, "scrollbutton") as FCButton;
                m_scrollButton.AllowDrag = true;
                m_scrollButton.addEvent(m_scrollButtonDraggingEvent, FCEventID.DRAGGING);
                m_backButton.addControl(m_scrollButton);
            }
        }

        /// <summary>
        /// 减量滚动按钮按下事件方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public void onReduceButtonTouchDown(FCTouchInfo touchInfo) {
            lineReduce();
            IsReducing = true;
        }

        /// <summary>
        /// 减量滚动按钮抬起事件回调方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public void onReduceButtonTouchUp(FCTouchInfo touchInfo) {
            IsReducing = false;
        }

        /// <summary>
        /// 滚动方法
        /// </summary>
        public virtual void onScrolled() {
            callEvents(FCEventID.SCROLLED);
            FCView parent = Parent;
            if (parent != null) {
                parent.invalidate();
            }
        }

        /// <summary>
        /// 秒表回调方法
        /// </summary>
        /// <param name="timerID">秒表ID</param>
        public override void onTimer(int timerID) {
            base.onTimer(timerID);
            if (m_timerID == timerID) {
                if (m_isAdding) {
                    if (m_tick > 5) {
                        pageAdd();
                    }
                    else {
                        lineAdd();
                    }
                }
                if (m_isReducing) {
                    if (m_tick > 5) {
                        pageReduce();
                    }
                    else {
                        lineReduce();
                    }
                }
                if (m_addSpeed != 0) {
                    int sub = onAddSpeedScrolling();
                    Pos += sub;
                    update();
                    onScrolled();
                    m_addSpeed -= sub;
                    if (Math.Abs(m_addSpeed) < 3) {
                        m_addSpeed = 0;
                    }
                    if (m_addSpeed == 0) {
                        onAddSpeedScrollEnd();
                        stopTimer(m_timerID);
                        if (Parent != null) {
                            Parent.invalidate();
                        }
                    }
                }
                m_tick++;
            }
        }

        /// <summary>
        /// 可见状态改变方法
        /// </summary>
        public override void onVisibleChanged() {
            base.onVisibleChanged();
            if (!Visible) {
                m_pos = 0;
            }
        }

        /// <summary>
        /// 页变大方法
        /// </summary>
        public virtual void pageAdd() {
            m_pos += m_pageSize;
            if (m_pos > m_contentSize - m_pageSize) {
                m_pos = m_contentSize - m_pageSize;
            }
            update();
            onScrolled();
        }

        /// <summary>
        /// 页变小方法
        /// </summary>
        public virtual void pageReduce() {
            m_pos -= m_pageSize;
            if (m_pos < 0) {
                m_pos = 0;
            }
            update();
            onScrolled();
        }

        /// <summary>
        /// 减量滚动按钮按下事件回调方法
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="touchInfo">触摸信息</param>
        public void reduceButtonTouchDown(object sender, FCTouchInfo touchInfo) {
            onReduceButtonTouchDown(touchInfo);
        }

        /// <summary>
        /// 减量滚动按钮抬起事件回调方法
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="touchInfo">触摸信息</param>
        public void reduceButtonTouchUp(object sender, FCTouchInfo touchInfo) {
            onReduceButtonTouchUp(touchInfo);
        }

        /// <summary>
        /// 滚动按钮的拖动事件回调方法
        /// </summary>
        /// <param name="sender">调用者</param>
        public void scrollButtonDragging(object sender) {
            onDragScroll();
        }

        /// <summary>
        /// 滚动到开始
        /// </summary>
        public void scrollToBegin() {
            m_pos = 0;
            update();
            onScrolled();
        }

        /// <summary>
        /// 滚动到结束
        /// </summary>
        public void scrollToEnd() {
            m_pos = m_contentSize - m_pageSize;
            if (m_pos < 0) {
                m_pos = 0;
            }
            update();
            onScrolled();
        }
    }
}
