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
using System.Text;

namespace FaceCat {
    /// <summary>
    /// 控件事件
    /// </summary>
    /// <param name="sender">调用者</param>
    /// <param name="cancel">是否退出</param>
    public delegate void FCWindowClosingEvent(object sender, ref bool cancel);

    /// <summary>
    /// 窗体控件
    /// </summary>
    public class FCWindow : FCView {
        /// <summary>
        /// 创建窗体控件
        /// </summary>
        public FCWindow() {
            AllowDrag = true;
            IsWindow = true;
            Visible = false;
        }

        /// <summary>
        /// 调整尺寸的点
        /// </summary>
        private int m_resizePoint = -1;

        /// <summary>
        /// 移动开始点
        /// </summary>
        private FCPoint m_startTouchPoint;

        /// <summary>
        /// 移动开始时的控件矩形
        /// </summary>
        private FCRect m_startRect;

        protected int m_borderWidth = 2;

        /// <summary>
        /// 获取或设置边框的宽度
        /// </summary>
        public virtual int BorderWidth {
            get { return m_borderWidth; }
            set { m_borderWidth = value; }
        }

        protected bool m_canResize = false;

        /// <summary>
        /// 获取或设置是否可以调整尺寸
        /// </summary>
        public virtual bool CanResize {
            get { return m_canResize; }
            set { m_canResize = value; }
        }

        protected int m_captionHeight = 20;

        /// <summary>
        /// 获取或设置标题栏的高度
        /// </summary>
        public virtual int CaptionHeight {
            get { return m_captionHeight; }
            set { m_captionHeight = value; }
        }

        /// <summary>
        /// 获取客户端的区域
        /// </summary>
        public virtual FCRect ClientSize {
            get {
                int width = Width, height = Height - m_captionHeight;
                if (height < 0) {
                    height = 0;
                }
                return new FCRect(0, m_captionHeight, width, height);
            }
        }

        protected FCWindowFrame m_frame;

        /// <summary>
        /// 获取或设置窗体的
        /// </summary>
        public virtual FCWindowFrame Frame {
            get { return m_frame; }
            set { m_frame = value; }
        }

        protected bool m_isDialog;

        /// <summary>
        /// 获取或设置是否会话窗口
        /// </summary>
        public virtual bool IsDialog {
            get { return m_isDialog; }
        }

        protected long m_shadowColor = FCColor.argb(25, 255, 255, 255);

        /// <summary>
        /// 获取或设置阴影的颜色
        /// </summary>
        public virtual long ShadowColor {
            get { return m_shadowColor; }
            set { m_shadowColor = value; }
        }

        protected int m_shadowSize = 10;

        /// <summary>
        /// 获取或设置阴影的大小
        /// </summary>
        public virtual int ShadowSize {
            get { return m_shadowSize; }
            set { m_shadowSize = value; }
        }

        /// <summary>
        /// 将控件放到最前显示
        /// </summary>
        public override void bringToFront() {
            base.bringToFront();
            if (m_frame != null) {
                m_frame.bringToFront();
            }
        }

        /// <summary>
        /// 调用事件
        /// </summary>
        /// <param name="eventID">事件ID</param>
        /// <param name="cancel">是否退出</param>
        protected void callWindowClosingEvents(int eventID, ref bool cancel) {
            if (m_events != null && m_events.containsKey(eventID)) {
                ArrayList<object> events = m_events[eventID];
                int eventSize = events.size();
                for (int i = 0; i < eventSize; i++) {
                    FCWindowClosingEvent func = events.get(i) as FCWindowClosingEvent;
                    if (func != null) {
                        func(this, ref cancel);
                    }
                }
            }
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        public virtual void close() {
            bool cancel = false;
            onWindowClosing(ref cancel);
            if (!cancel) {
                if (m_frame != null) {
                    m_frame.removeControl(this);
                    Native.removeControl(m_frame);
                    m_frame.delete();
                    m_frame = null;
                    Parent = null;
                }
                else {
                    Native.removeControl(this);
                }
                onWindowClosed();
            }
        }

        /// <summary>
        /// 销毁控件方法
        /// </summary>
        public override void delete() {
            if (!IsDeleted) {
                if (m_frame != null) {
                    m_frame.removeControl(this);
                    Native.removeControl(m_frame);
                    m_frame.delete();
                    m_frame = null;
                }
            }
            base.delete();
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <returns></returns>
        public override String getControlType() {
            return "Window";
        }

        /// <summary>
        /// 获取动态绘图区域
        /// </summary>
        /// <returns>区域</returns>
        public FCRect getDynamicPaintRect() {
            FCSize oldSize = m_oldSize;
            if (oldSize.cx == 0 && oldSize.cy == 0) {
                oldSize = Size;
            }
            FCRect oldRect = new FCRect(m_oldLocation.x, m_oldLocation.y, m_oldLocation.x + oldSize.cx, m_oldLocation.y + oldSize.cy);
            FCRect rect = new FCRect(m_location.x, m_location.y, m_location.x + Width, m_location.y + Height);
            FCRect paintRect = new FCRect(Math.Min(oldRect.left, rect.left) - m_shadowSize - 10,
            Math.Min(oldRect.top, rect.top) - m_shadowSize - 10,
            Math.Max(oldRect.right, rect.right) + m_shadowSize + 10,
            Math.Max(oldRect.bottom, rect.bottom) + m_shadowSize + 10);
            return paintRect;
        }

        /// <summary>
        /// 获取事件名称列表
        /// </summary>
        /// <returns>名称列表</returns>
        public override ArrayList<String> getEventNames() {
            ArrayList<String> eventNames = base.getEventNames();
            eventNames.AddRange(new String[] { "WindowClosed", "WindowClosing" });
            return eventNames;
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        /// <param name="type">类型</param>
        public override void getProperty(String name, ref String value, ref String type) {
            if (name == "borderwidth") {
                type = "int";
                value = FCStr.convertIntToStr(BorderWidth);
            }
            else if (name == "canresize") {
                type = "bool";
                value = FCStr.convertBoolToStr(CanResize);
            }
            else if (name == "captionheight") {
                type = "int";
                value = FCStr.convertIntToStr(CaptionHeight);
            }
            else if (name == "shadowcolor") {
                type = "color";
                value = FCStr.convertColorToStr(ShadowColor);
            }
            else if (name == "shadowsize") {
                type = "int";
                value = FCStr.convertIntToStr(ShadowSize);
            }
            else {
                base.getProperty(name, ref value, ref type);
            }
        }

        /// <summary>
        /// 获取属性列表
        /// </summary>
        /// <returns>属性列表</returns>
        public override ArrayList<String> getPropertyNames() {
            ArrayList<String> propertyNames = base.getPropertyNames();
            propertyNames.AddRange(new String[] { "BorderWidth", "CanResize", "CaptionHeight", "ShadowColor", "ShadowSize" });
            return propertyNames;
        }

        /// <summary>
        /// 获取触摸状态
        /// </summary>
        /// <param name="state">状态值</param>
        /// <returns>触摸状态</returns>
        protected FCCursors getResizeCursor(int state) {
            switch (state) {
                case 0:
                    return FCCursors.SizeNWSE;
                case 1:
                    return FCCursors.SizeNESW;
                case 2:
                    return FCCursors.SizeNESW;
                case 3:
                    return FCCursors.SizeNWSE;
                case 4:
                    return FCCursors.SizeWE;
                case 5:
                    return FCCursors.SizeNS;
                case 6:
                    return FCCursors.SizeWE;
                case 7:
                    return FCCursors.SizeNS;
                default:
                    return FCCursors.Arrow;
            }
        }

        /// <summary>
        /// 获取调整尺寸的点
        /// </summary>
        /// <returns>矩形集合</returns>
        protected FCRect[] getResizePoints() {
            int width = Width, height = Height;
            FCRect[] points = new FCRect[8];
            //左上
            points[0] = new FCRect(0, 0, m_borderWidth * 2, m_borderWidth * 2);
            //左下
            points[1] = new FCRect(0, height - m_borderWidth * 2, m_borderWidth * 2, height);
            //右上
            points[2] = new FCRect(width - m_borderWidth * 2, 0, width, m_borderWidth * 2);
            //右下
            points[3] = new FCRect(width - m_borderWidth * 2, height - m_borderWidth * 2, width, height);
            //左
            points[4] = new FCRect(0, 0, m_borderWidth, height);
            //上
            points[5] = new FCRect(0, 0, width, m_borderWidth);
            //右
            points[6] = new FCRect(width - m_borderWidth, 0, width, height);
            //下
            points[7] = new FCRect(0, height - m_borderWidth, width, height);
            return points;
        }

        /// <summary>
        /// 获取调整尺寸的状态
        /// </summary>
        /// <returns>状态</returns>
        protected int getResizeState() {
            FCPoint mp = TouchPoint;
            FCRect[] pRects = getResizePoints();
            int rsize = pRects.Length;
            for (int i = 0; i < rsize; i++) {
                FCRect rect = pRects[i];
                if (mp.x >= rect.left && mp.x <= rect.right
                    && mp.y >= rect.top && mp.y <= rect.bottom) {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 滚动开始方法
        /// </summary>
        /// <returns>是否已处理</returns>
        public override bool onDragBegin() {
            FCPoint mp = TouchPoint;
            int width = Width, height = Height;
            if (mp.y > m_captionHeight) {
                return false;
            }
            if (m_resizePoint != -1) {
                return false;
            }
            return base.onDragBegin();
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
        /// 触摸按下方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onTouchDown(FCTouchInfo touchInfo) {
            base.onTouchDown(touchInfo);
            //选中点
            if (touchInfo.m_firstTouch && touchInfo.m_clicks == 1) {
                if (m_canResize) {
                    m_resizePoint = getResizeState();
                    Cursor = getResizeCursor(m_resizePoint);
                    m_startTouchPoint = Native.TouchPoint;
                    m_startRect = Bounds;
                }
            }
            invalidate();
        }

        /// <summary>
        /// 触摸移动方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onTouchMove(FCTouchInfo touchInfo) {
            base.onTouchMove(touchInfo);
            if (m_canResize) {
                FCPoint nowPoint = Native.TouchPoint;
                if (m_resizePoint != -1) {
                    int left = m_startRect.left, top = m_startRect.top, right = m_startRect.right, bottom = m_startRect.bottom;
                    windowResize(m_resizePoint, ref left, ref top, ref right, ref bottom, ref nowPoint, ref m_startTouchPoint);
                    FCRect bounds = new FCRect(left, top, right, bottom);
                    Bounds = bounds;
                    Native.invalidate();
                }
                else {
                    Cursor = getResizeCursor(getResizeState());
                }
            }
        }

        /// <summary>
        /// 触摸抬起方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onTouchUp(FCTouchInfo touchInfo) {
            base.onTouchUp(touchInfo);
            m_resizePoint = -1;
            invalidate();
        }

        /// <summary>
        /// 绘制前景方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void onPaintForeground(FCPaint paint, FCRect clipRect) {
            String text = Text;
            if (text != null && text.Length > 0) {
                int width = Width;
                FCFont font = Font;
                FCSize tSize = paint.textSize(text, font);
                FCPoint strPoint = new FCPoint();
                strPoint.x = 5;
                strPoint.y = (m_captionHeight - tSize.cy) / 2;
                FCRect tRect = new FCRect(strPoint.x, strPoint.y, strPoint.x + tSize.cx, strPoint.y + tSize.cy);
                paint.drawText(text, getPaintingTextColor(), font, tRect);
            }
        }

        /// <summary>
        /// 可见状态改变方法
        /// </summary>
        public override void onVisibleChanged() {
            base.onVisibleChanged();
            FCNative native = Native;
            if (native != null) {
                if (Visible) {
                    if (m_frame == null) {
                        m_frame = new FCWindowFrame();
                    }
                    native.removeControl(this);
                    native.addControl(m_frame);
                    m_frame.Size = native.DisplaySize;
                    if (!m_frame.containsControl(this)) {
                        m_frame.addControl(this);
                    }
                }
                else {
                    if (m_frame != null) {
                        m_frame.removeControl(this);
                        native.removeControl(m_frame);
                    }
                }
            }
        }

        /// <summary>
        /// 窗体正在关闭方法
        /// </summary>
        /// <param name="cancel">是否退出</param>
        public virtual void onWindowClosing(ref bool cancel) {
            callWindowClosingEvents(FCEventID.WINDOWCLOSING, ref cancel);
        }

        /// <summary>
        /// 窗体关闭方法
        /// </summary>
        public virtual void onWindowClosed() {
            callEvents(FCEventID.WINDOWCLOSED);
        }

        /// <summary>
        /// 将控件放到最下面显示
        /// </summary>
        public override void sendToBack() {
            base.sendToBack();
            if (m_frame != null) {
                m_frame.sendToBack();
            }
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        public override void setProperty(String name, String value) {
            if (name == "borderwidth") {
                BorderWidth = FCStr.convertStrToInt(value);
            }
            else if (name == "canresize") {
                CanResize = FCStr.convertStrToBool(value);
            }
            else if (name == "captionheight") {
                CaptionHeight = FCStr.convertStrToInt(value);
            }
            else if (name == "shadowcolor") {
                ShadowColor = FCStr.convertStrToColor(value);
            }
            else if (name == "shadowsize") {
                ShadowSize = FCStr.convertStrToInt(value);
            }
            else {
                base.setProperty(name, value);
            }
        }

        /// <summary>
        /// 以会话方式显示
        /// </summary>
        public virtual void showDialog() {
            m_isDialog = true;
            show();
        }

        /// <summary>
        /// 窗体尺寸改变
        /// </summary>
        /// <param name="resizePoint"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        /// <param name="nowPoint"></param>
        /// <param name="startTouchPoint"></param>
        public virtual void windowResize(int resizePoint, ref int left, ref int top, ref int right, ref int bottom, ref FCPoint nowPoint, ref FCPoint startTouchPoint) {
            switch (resizePoint) {
                case 0:
                    left = left + nowPoint.x - startTouchPoint.x;
                    top = top + nowPoint.y - startTouchPoint.y;
                    break;
                case 1:
                    left = left + nowPoint.x - startTouchPoint.x;
                    bottom = bottom + nowPoint.y - startTouchPoint.y;
                    break;
                case 2:
                    right = right + nowPoint.x - startTouchPoint.x;
                    top = top + nowPoint.y - startTouchPoint.y;
                    break;
                case 3:
                    right = right + nowPoint.x - startTouchPoint.x;
                    bottom = bottom + nowPoint.y - startTouchPoint.y;
                    break;
                case 4:
                    left = left + nowPoint.x - startTouchPoint.x;
                    break;
                case 5:
                    top = top + nowPoint.y - startTouchPoint.y;
                    break;
                case 6:
                    right = right + nowPoint.x - startTouchPoint.x;
                    break;
                case 7:
                    bottom = bottom + nowPoint.y - startTouchPoint.y;
                    break;
            }
        }
    }
}
