/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

using System;
using System.Collections.Generic;

namespace FaceCat {
    /// <summary>
    /// 图层
    /// </summary>
    public class FCDiv : FCView {
        /// <summary>
        /// 创建支持滚动条的控件
        /// </summary>
        public FCDiv() {
            m_scrollButtonKeyDownEvent = new FCKeyEvent(scrollButtonKeyDown);
            m_scrollButtonTouchWheelEvent = new FCTouchEvent(scrollButtonTouchWheel);
            FCSize size = new FCSize(200, 200);
            Size = size;
        }

        /// <summary>
        /// 是否正在滚动2
        /// </summary>
        private bool m_isDragScrolling2;

        /// <summary>
        /// 是否准备拖动滚动
        /// </summary>
        private bool m_readyToDragScroll;

        /// <summary>
        /// 滚动按钮键盘事件按下事件
        /// </summary>
        private FCKeyEvent m_scrollButtonKeyDownEvent;

        /// <summary>
        /// 滚动按钮触摸滚动事件
        /// </summary>
        private FCTouchEvent m_scrollButtonTouchWheelEvent;

        /// <summary>
        /// 开始移动的位置
        /// </summary>
        private FCPoint m_startMovePoint;

        /// <summary>
        /// 开始移动的横向位置
        /// </summary>
        private int m_startMovePosX;

        /// <summary>
        /// 开始移动的纵向位置
        /// </summary>
        private int m_startMovePosY;

        /// <summary>
        /// 开始移动时间
        /// </summary>
        private DateTime m_startMoveTime;

        protected bool m_allowDragScroll = false;

        /// <summary>
        /// 获取或设置是否允许拖动滚动
        /// </summary>
        public virtual bool AllowDragScroll {
            get { return m_allowDragScroll; }
            set { m_allowDragScroll = value; }
        }

        protected FCHScrollBar m_hScrollBar;

        /// <summary>
        /// 获取横向滚动条
        /// </summary>
        public virtual FCHScrollBar HScrollBar {
            get {
                if (Native != null && m_showHScrollBar) {
                    if (m_hScrollBar == null) {
                        FCHost host = Native.Host;
                        m_hScrollBar = host.createInternalControl(this, "hscrollbar") as FCHScrollBar;
                        addControl(m_hScrollBar);
                        //注册按钮的事件
                        m_hScrollBar.AddButton.addEvent(m_scrollButtonKeyDownEvent, FCEventID.KEYDOWN);
                        m_hScrollBar.AddButton.addEvent(m_scrollButtonTouchWheelEvent, FCEventID.TOUCHWHEEL);
                        m_hScrollBar.BackButton.addEvent(m_scrollButtonKeyDownEvent, FCEventID.KEYDOWN);
                        m_hScrollBar.BackButton.addEvent(m_scrollButtonTouchWheelEvent, FCEventID.TOUCHWHEEL);
                        m_hScrollBar.ReduceButton.addEvent(m_scrollButtonKeyDownEvent, FCEventID.KEYDOWN);
                        m_hScrollBar.ReduceButton.addEvent(m_scrollButtonTouchWheelEvent, FCEventID.TOUCHWHEEL);
                        m_hScrollBar.ScrollButton.addEvent(m_scrollButtonKeyDownEvent, FCEventID.KEYDOWN);
                        m_hScrollBar.ScrollButton.addEvent(m_scrollButtonTouchWheelEvent, FCEventID.TOUCHWHEEL);
                    }
                    return m_hScrollBar;
                }
                return null;
            }
        }

        protected bool m_showHScrollBar = false;

        /// <summary>
        /// 获取或设置是否显示横向滚动条
        /// </summary>
        public virtual bool ShowHScrollBar {
            get { return m_showHScrollBar; }
            set { m_showHScrollBar = value; }
        }

        private bool m_isDragScrolling;

        /// <summary>
        /// 获取是否正在被拖动
        /// </summary>
        public virtual bool IsDragScrolling {
            get { return m_isDragScrolling; }
        }

        protected bool m_showVScrollBar = false;

        /// <summary>
        /// 获取或设置是否显示纵向滚动条
        /// </summary>
        public virtual bool ShowVScrollBar {
            get { return m_showVScrollBar; }
            set { m_showVScrollBar = value; }
        }

        protected FCVScrollBar m_vScrollBar;

        /// <summary>
        /// 获取纵向滚动条
        /// </summary>
        public virtual FCVScrollBar VScrollBar {
            get {
                if (Native != null && m_showVScrollBar) {
                    if (m_vScrollBar == null) {
                        FCHost host = Native.Host;
                        m_vScrollBar = host.createInternalControl(this, "vscrollbar") as FCVScrollBar;
                        addControl(m_vScrollBar);
                        //注册按钮的事件
                        m_vScrollBar.AddButton.addEvent(m_scrollButtonKeyDownEvent, FCEventID.KEYDOWN);
                        m_vScrollBar.AddButton.addEvent(m_scrollButtonTouchWheelEvent, FCEventID.TOUCHWHEEL);
                        m_vScrollBar.BackButton.addEvent(m_scrollButtonKeyDownEvent, FCEventID.KEYDOWN);
                        m_vScrollBar.BackButton.addEvent(m_scrollButtonTouchWheelEvent, FCEventID.TOUCHWHEEL);
                        m_vScrollBar.ReduceButton.addEvent(m_scrollButtonKeyDownEvent, FCEventID.KEYDOWN);
                        m_vScrollBar.ReduceButton.addEvent(m_scrollButtonTouchWheelEvent, FCEventID.TOUCHWHEEL);
                        m_vScrollBar.ScrollButton.addEvent(m_scrollButtonKeyDownEvent, FCEventID.KEYDOWN);
                        m_vScrollBar.ScrollButton.addEvent(m_scrollButtonTouchWheelEvent, FCEventID.TOUCHWHEEL);
                    }
                    return m_vScrollBar;
                }
                return null;
            }
        }

        /// <summary>
        /// 销毁方法
        /// </summary>
        public override void delete() {
            if (!IsDeleted) {
                m_scrollButtonKeyDownEvent = null;
                m_scrollButtonTouchWheelEvent = null;
            }
            base.delete();
        }

        /// <summary>
        /// 获取内容的高度
        /// </summary>
        /// <returns>高度</returns>
        public virtual int getContentHeight() {
            FCHScrollBar hScrollBar = HScrollBar;
            FCVScrollBar vScrollBar = VScrollBar;
            int hmax = 0;
            ArrayList<FCView> controls = m_controls;
            int controlSize = controls.size();
            for (int i = 0; i < controlSize; i++) {
                FCView control = controls[i];
                if (control.Visible && control != hScrollBar && control != vScrollBar) {
                    int bottom = control.Bottom;
                    if (bottom > hmax) {
                        hmax = bottom;
                    }
                }
            }
            return hmax;
        }

        /// <summary>
        /// 获取内容的宽度
        /// </summary>
        /// <returns>宽度</returns>
        public virtual int getContentWidth() {
            FCHScrollBar hScrollBar = HScrollBar;
            FCVScrollBar vScrollBar = VScrollBar;
            int wmax = 0;
            ArrayList<FCView> controls = m_controls;
            int controlSize = controls.size();
            for (int i = 0; i < controlSize; i++) {
                FCView control = controls[i];
                if (control.Visible && control != hScrollBar && control != vScrollBar) {
                    int right = control.Right;
                    if (right > wmax) {
                        wmax = right;
                    }
                }
            }
            return wmax;
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <returns>控件类型</returns>
        public override String getControlType() {
            return "Div";
        }

        /// <summary>
        /// 获取显示偏移坐标
        /// </summary>
        /// <returns>坐标</returns>
        public override FCPoint getDisplayOffset() {
            FCPoint offset = new FCPoint();
            if (Visible) {
                offset.x = (m_hScrollBar != null && m_hScrollBar.Visible) ? m_hScrollBar.Pos : 0;
                offset.y = (m_vScrollBar != null && m_vScrollBar.Visible) ? m_vScrollBar.Pos : 0;
            }
            return offset;
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">返回属性值</param>
        /// <param name="type">返回属性类型</param>
        public override void getProperty(String name, ref String value, ref String type) {
            if (name == "allowdragscroll") {
                type = "bool";
                value = FCStr.convertBoolToStr(AllowDragScroll);
            }
            else if (name == "showhscrollbar") {
                type = "bool";
                value = FCStr.convertBoolToStr(ShowHScrollBar);
            }
            else if (name == "showvscrollbar") {
                type = "bool";
                value = FCStr.convertBoolToStr(ShowVScrollBar);
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
            propertyNames.AddRange(new String[] { "AllowDragScroll", "ShowHScrollBar", "ShowVScrollBar" });
            return propertyNames;
        }

        /// <summary>
        /// 向下滚动一行
        /// </summary>
        public virtual void lineDown() {
            if (m_vScrollBar != null && m_vScrollBar.Visible) {
                m_vScrollBar.lineAdd();
            }
        }

        /// <summary>
        /// 向左滚动一行
        /// </summary>
        public virtual void lineLeft() {
            if (m_hScrollBar != null && m_hScrollBar.Visible) {
                m_hScrollBar.lineReduce();
            }
        }

        /// <summary>
        /// 向右滚动一行
        /// </summary>
        public virtual void LineRight() {
            if (m_hScrollBar != null && m_hScrollBar.Visible) {
                m_hScrollBar.lineAdd();
            }
        }

        /// <summary>
        /// 向上滚动一行
        /// </summary>
        public virtual void lineUp() {
            if (m_vScrollBar != null && m_vScrollBar.Visible) {
                m_vScrollBar.lineReduce();
            }
        }

        /// <summary>
        /// 拖动准备方法
        /// </summary>
        /// <param name="startOffset">可以拖动的偏移坐标量</param>
        public override void onDragReady(ref FCPoint startOffset) {
            startOffset.x = 0;
            startOffset.y = 0;
        }

        /// <summary>
        /// 拖动滚动结束
        /// </summary>
        public virtual void onDragScrollEnd() {
            m_isDragScrolling = false;
            if (m_readyToDragScroll) {
                DateTime nowTime = DateTime.Now;
                FCPoint newPoint = Native.TouchPoint;
                if (m_hScrollBar != null && m_hScrollBar.Visible) {
                    m_hScrollBar.onAddSpeedScrollStart(m_startMoveTime.Ticks, nowTime.Ticks, m_startMovePoint.x, newPoint.x);
                }
                if (m_vScrollBar != null && m_vScrollBar.Visible) {
                    m_vScrollBar.onAddSpeedScrollStart(m_startMoveTime.Ticks, nowTime.Ticks, m_startMovePoint.y, newPoint.y);
                }
                m_readyToDragScroll = false;
                invalidate();
            }
        }

        /// <summary>
        /// 拖动滚动中
        /// </summary>
        public virtual void onDragScrolling() {
            int width = Width, height = Height;
            if (m_allowDragScroll && m_readyToDragScroll) {
                if (!onDragScrollPermit()) {
                    m_readyToDragScroll = false;
                    return;
                }
                bool paint = false;
                FCPoint newPoint = Native.TouchPoint;
                if (m_hScrollBar != null && m_hScrollBar.Visible) {
                    if (Math.Abs(newPoint.x - m_startMovePoint.x) > width / 10) {
                        m_isDragScrolling2 = true;
                    }
                    int newPos = m_startMovePosX + m_startMovePoint.x - newPoint.x;
                    if (newPos != m_hScrollBar.Pos) {
                        m_hScrollBar.Pos = newPos;
                        m_hScrollBar.update();
                        paint = true;
                    }
                }
                if (m_vScrollBar != null && m_vScrollBar.Visible) {
                    if (Math.Abs(newPoint.y - m_startMovePoint.y) > height / 10) {
                        m_isDragScrolling2 = true;
                    }
                    int newPos = m_startMovePosY + m_startMovePoint.y - newPoint.y;
                    if (newPos != m_vScrollBar.Pos) {
                        m_vScrollBar.Pos = newPos;
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

        /// <summary>
        /// 拖动滚动许可检查
        /// </summary>
        public virtual bool onDragScrollPermit() {
            FCView focusedControl = Native.FocusedControl;
            if (focusedControl != null) {
                if (focusedControl.IsDragging) {
                    return false;
                }
                if (focusedControl is FCGridColumn) {
                    return false;
                }
                if (focusedControl.Parent != null) {
                    if (focusedControl.Parent is FCScrollBar) {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 拖动滚动开始
        /// </summary>
        public virtual void onDragScrollStart() {
            m_isDragScrolling = false;
            m_isDragScrolling2 = false;
            FCView focusedControl = Native.FocusedControl;
            if (m_hScrollBar != null && m_hScrollBar.Visible) {
                if (focusedControl == m_hScrollBar.AddButton
                    || focusedControl == m_hScrollBar.ReduceButton
                    || focusedControl == m_hScrollBar.BackButton
                    || focusedControl == m_hScrollBar.ScrollButton) {
                    m_hScrollBar.AddSpeed = 0;
                    return;
                }
            }
            if (m_vScrollBar != null && m_vScrollBar.Visible) {
                if (focusedControl == m_vScrollBar.AddButton
             || focusedControl == m_vScrollBar.ReduceButton
             || focusedControl == m_vScrollBar.BackButton
             || focusedControl == m_vScrollBar.ScrollButton) {
                    m_vScrollBar.AddSpeed = 0;
                    return;
                }
            }
            if (m_allowDragScroll) {
                if (m_hScrollBar != null && m_hScrollBar.Visible) {
                    m_startMovePosX = m_hScrollBar.Pos;
                    m_hScrollBar.AddSpeed = 0;
                    m_readyToDragScroll = true;
                }
                if (m_vScrollBar != null && m_vScrollBar.Visible) {
                    m_startMovePosY = m_vScrollBar.Pos;
                    m_vScrollBar.AddSpeed = 0;
                    m_readyToDragScroll = true;
                }
                if (m_readyToDragScroll) {
                    m_startMovePoint = Native.TouchPoint;
                    m_startMoveTime = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// 键盘按下方法
        /// </summary>
        /// <param name="key">按键</param>
        public override void onKeyDown(char key) {
            base.onKeyDown(key);
            FCHost host = Native.Host;
            if (!host.isKeyPress(0x10) && !host.isKeyPress(0x11) && !host.isKeyPress(0x12)) {
                //向上
                if (key == 38) {
                    lineUp();
                }
                //向下
                else if (key == 40) {
                    lineDown();
                }
                invalidate();
            }
        }

        /// <summary>
        /// 触摸按下方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onTouchDown(FCTouchInfo touchInfo) {
            base.onTouchDown(touchInfo);
            if (!m_allowPreviewsEvent) {
                onDragScrollStart();
            }
        }

        /// <summary>
        /// 触摸移动方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onTouchMove(FCTouchInfo touchInfo) {
            base.onTouchMove(touchInfo);
            if (!m_allowPreviewsEvent) {
                onDragScrolling();
            }
        }

        /// <summary>
        /// 触摸抬起方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onTouchUp(FCTouchInfo touchInfo) {
            base.onTouchUp(touchInfo);
            if (!m_allowPreviewsEvent) {
                onDragScrollEnd();
            }
        }

        /// <summary>
        /// 触摸滚动方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onTouchWheel(FCTouchInfo touchInfo) {
            base.onTouchWheel(touchInfo);
            if (touchInfo.m_delta > 0) {
                lineUp();
                invalidate();
            }
            else if (touchInfo.m_delta < 0) {
                lineDown();
                invalidate();
            }
        }

        /// <summary>
        /// 预处理触摸事件
        /// </summary>
        /// <param name="eventID">事件ID</param>
        /// <param name="touchInfo">触摸信息</param>
        /// <returns>状态</returns>
        public override bool onPreviewsTouchEvent(int eventID, FCTouchInfo touchInfo) {
            if (callPreviewsTouchEvents(FCEventID.PREVIEWSTOUCHEVENT, eventID, touchInfo)) {
                return true;
            }
            if (m_allowPreviewsEvent) {
                if (eventID == FCEventID.TOUCHDOWN) {
                    onDragScrollStart();
                }
                else if (eventID == FCEventID.TOUCHMOVE) {
                    onDragScrolling();
                }
                else if (eventID == FCEventID.TOUCHUP) {
                    bool state = m_isDragScrolling;
                    onDragScrollEnd();
                    if (state && !m_isDragScrolling2) {
                        return false;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 向下翻一页
        /// </summary>
        public virtual void pageDown() {
            if (m_vScrollBar != null && m_vScrollBar.Visible) {
                m_vScrollBar.pageAdd();
            }
        }

        /// <summary>
        /// 向左翻一页
        /// </summary>
        public virtual void pageLeft() {
            if (m_hScrollBar != null && m_hScrollBar.Visible) {
                m_hScrollBar.pageReduce();
            }
        }

        /// <summary>
        /// 向右翻一页
        /// </summary>
        public virtual void pageRight() {
            if (m_hScrollBar != null && m_hScrollBar.Visible) {
                m_hScrollBar.pageAdd();
            }
        }

        /// <summary>
        /// 向上翻一页
        /// </summary>
        public virtual void pageUp() {
            if (m_vScrollBar != null && m_vScrollBar.Visible) {
                m_vScrollBar.pageReduce();
            }
        }

        /// <summary>
        /// 滚动按钮键盘按下事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="key">按键</param>
        protected void scrollButtonKeyDown(object sender, char key) {
            onKeyDown(key);
        }

        /// <summary>
        /// 滚动按钮触摸滚动事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="touchInfo">触摸信息</param>
        protected void scrollButtonTouchWheel(object sender, FCTouchInfo touchInfo) {
            FCTouchInfo newTouchInfo = touchInfo.clone();
            newTouchInfo.m_firstPoint = TouchPoint;
            newTouchInfo.m_secondPoint = TouchPoint;
            onTouchWheel(newTouchInfo);
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        public override void setProperty(String name, String value) {
            if (name == "allowdragscroll") {
                AllowDragScroll = FCStr.convertStrToBool(value);
            }
            else if (name == "showhscrollbar") {
                ShowHScrollBar = FCStr.convertStrToBool(value);
            }
            else if (name == "showvscrollbar") {
                ShowVScrollBar = FCStr.convertStrToBool(value);
            }
            else {
                base.setProperty(name, value);
            }
        }

        /// <summary>
        /// 更新布局方法
        /// </summary>
        public override void update() {
            base.update();
            updateScrollBar();
        }

        /// <summary>
        /// 更新滚动条的布局
        /// </summary>
        public virtual void updateScrollBar() {
            if (Native != null) {
                FCHScrollBar hScrollBar = HScrollBar;
                FCVScrollBar vScrollBar = VScrollBar;
                if (Visible) {
                    int width = Width, height = Height;
                    //滚动条尺寸         
                    int hBarHeight = (hScrollBar != null) ? hScrollBar.Height : 0;
                    int vBarWidth = (vScrollBar != null) ? vScrollBar.Width : 0;
                    int wmax = getContentWidth(), hmax = getContentHeight();
                    if (hScrollBar != null) {
                        hScrollBar.ContentSize = wmax;
                        hScrollBar.Size = new FCSize(width - vBarWidth, hBarHeight);
                        hScrollBar.PageSize = width - vBarWidth;
                        hScrollBar.Location = new FCPoint(0, height - hBarHeight);
                        if (wmax <= width) {
                            hScrollBar.Visible = false;
                        }
                        else {
                            hScrollBar.Visible = true;
                        }
                    }
                    if (vScrollBar != null) {
                        vScrollBar.ContentSize = hmax;
                        vScrollBar.Size = new FCSize(vBarWidth, height - hBarHeight);
                        vScrollBar.PageSize = height - hBarHeight;
                        vScrollBar.Location = new FCPoint(width - vBarWidth, 0);
                        int vh = (hScrollBar != null && hScrollBar.Visible) ? height - hBarHeight : height;
                        if (hmax <= vh) {
                            vScrollBar.Visible = false;
                        }
                        else {
                            vScrollBar.Visible = true;
                        }
                    }
                    //修改尺寸
                    if (hScrollBar != null && vScrollBar != null) {
                        if (hScrollBar.Visible && !vScrollBar.Visible) {
                            hScrollBar.Width = width;
                            hScrollBar.PageSize = width;
                        }
                        else if (!hScrollBar.Visible && vScrollBar.Visible) {
                            vScrollBar.Height = height;
                            vScrollBar.PageSize = height;
                        }
                    }
                    if (hScrollBar != null && hScrollBar.Visible) {
                        hScrollBar.update();
                    }
                    if (vScrollBar != null && vScrollBar.Visible) {
                        vScrollBar.update();
                    }
                }
            }
        }
    }
}
