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
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;

namespace FaceCat {
    /// <summary>
    /// 窗体控件管理
    /// </summary>
    public class WinHost : FCHost {
        /// <summary>
        /// 析构函数
        /// </summary>
        ~WinHost() {
            delete();
        }

        private IntPtr m_hImc;
        [DllImport("user32.dll", ExactSpelling = true)]
        private static extern bool GetUpdateRect(IntPtr hWnd, out FCRect lpRect, bool bErase);
        [DllImport("user32.dll", ExactSpelling = true)]
        private static extern int IntersectRect(ref FCRect lpDestRect, ref FCRect lpSrc1Rect, ref FCRect lpSrc2Rect);
        [DllImport("user32.dll", ExactSpelling = true)]
        private static extern int UnionRect(ref FCRect lpDestRect, ref FCRect lpSrc1Rect, ref FCRect lpSrc2Rect);
        [DllImport("Imm32.dll")]
        private static extern IntPtr ImmGetContext(IntPtr hWnd);
        [DllImport("Imm32.dll")]
        private static extern IntPtr ImmAssociateContext(IntPtr hWnd, IntPtr hIMC);
        [DllImport("imm32.dll")]
        static extern int ImmGetCompositionString(IntPtr hIMC, int dwIndex, StringBuilder lpBuf, int dwBufLen);
        private const int WM_ERASEBKGND = 0x0014;
        private const int WM_PAINT = 0x000F;
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_RBUTTONDOWN = 0x0204;
        private const int WM_LBUTTONDBLCLK = 0x0203;
        private const int WM_RBUTTONDBLCLK = 0x0206;
        private const int WM_LBUTTONUP = 0x0202;
        private const int WM_RBUTTONUP = 0x0205;
        private const int WM_MOUSEMOVE = 0x0200;
        private const int WM_MOUSEWHEEL = 0x020A;
        private const int WM_TIMER = 0x0113;
        private const int WM_IME_SETCONTEXT = 0x0281;
        private const int WM_IME_CHAR = 0x0286;
        private const int WM_CHAR = 0x0102;
        private const int WM_IME_COMPOSITION = 0x010F;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYUP = 0x0105;
        private const int GCS_COMPSTR = 0x0008;
        private const int HC_ACTION = 0;
        private const int PM_REMOVE = 0x0001;
        private int GCS_RESULTSTR = 0x0800;
        private delegate void TimerProc(IntPtr hWnd, uint nMsg, int nIDEvent, int dwTime);
        [DllImport("user32")]
        private static extern int SetTimer(IntPtr hWnd, int nIDEvent, int uElapse, TimerProc CB);
        [DllImport("user32")]
        private static extern int KillTimer(IntPtr hWnd, int nIDEvent);
        [DllImport("user32")]
        private static extern int GetKeyState(int nVirtKey);
        [DllImport("user32")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32")]
        private static extern int PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32")]
        private static extern int UpdateWindow(IntPtr hWnd);

        /// <summary>
        /// 是否可以操作
        /// </summary>
        private bool m_allowOperate = true;

        /// <summary>
        /// 是否支持局部绘图
        /// </summary>
        private bool m_allowPartialPaint = true;

        /// <summary>
        /// 容器
        /// </summary>
        private Control m_container;

        /// <summary>
        /// 调用控件线程方法的参数
        /// </summary>
        private HashMap<int, object> m_invokeArgs = new HashMap<int, object>();

        /// <summary>
        /// 调用控件线程方法的控件
        /// </summary>
        private HashMap<int, FCView> m_invokeControls = new HashMap<int, FCView>();

        /// <summary>
        /// 调用控件线程方法的流水号
        /// </summary>
        private int m_invokeSerialID = 0;

        /// <summary>
        /// 获取或设置是否允许操作
        /// </summary>
        public override bool AllowOperate {
            get { return m_allowOperate; }
            set { m_allowOperate = value; }
        }

        /// <summary>
        /// 获取或设置是否允许局部绘图
        /// </summary>
        public override bool AllowPartialPaint {
            get { return m_allowPartialPaint; }
            set { m_allowPartialPaint = value; }
        }

        private IntPtr m_hWnd;

        /// <summary>
        /// 获取或设置图形控件
        /// </summary>
        public virtual IntPtr HWnd {
            get { return m_hWnd; }
            set {
                m_hWnd = value;
                m_container = Control.FromHandle(m_hWnd);
                m_container.SizeChanged += new EventHandler(container_SizeChanged);
                m_hImc = ImmGetContext(m_hWnd);
            }
        }

        protected bool m_isDeleted;

        /// <summary>
        /// 获取是否已被销毁
        /// </summary>
        public override bool IsDeleted {
            get { return m_isDeleted; }
        }

        private FCNative m_native;

        /// <summary>
        /// 获取或设置方法库
        /// </summary>
        public override FCNative Native {
            get { return m_native; }
            set {
                m_native = value;
            }
        }

        private int m_pInvokeMsgID = 0x0401;

        /// <summary>
        /// 获取或设置调用线程方法的消息ID
        /// </summary>
        public virtual int PInvokeMsgID {
            get { return m_pInvokeMsgID; }
            set { m_pInvokeMsgID = value; }
        }

        private FCView m_toolTip;

        /// <summary>
        /// 获取或设置提示框
        /// </summary>
        public virtual FCView ToolTip {
            get { return m_toolTip; }
            set { m_toolTip = value; }
        }

        /// <summary>
        /// 激活镜像
        /// </summary>
        public override void activeMirror() {
            if (m_native.MirrorMode != FCMirrorMode.None) {
                ArrayList<FCView> controls = m_native.getControls();
                int controlsSize = controls.size();
                for (int i = 0; i < controlsSize; i++) {
                    FCView control = controls.get(i);
                    if (control.Native != m_native) {
                        control.Native = m_native;
                    }
                }
            }
        }

        /// <summary>
        /// 在控件的线程中调用方法
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="args">参数</param>
        public override void beginInvoke(FCView control, object args) {
            lock (m_invokeArgs) {
                m_invokeArgs.put(m_invokeSerialID, args);
            }
            lock (m_invokeControls) {
                m_invokeControls.put(m_invokeSerialID, control);
            }
            IntPtr hWnd = m_hWnd;
            if (m_native.MirrorMode != FCMirrorMode.None) {
                WinHost host = m_native.MirrorHost.Host as WinHost;
                hWnd = host.HWnd;
            }
            SendMessage(hWnd, m_pInvokeMsgID, m_invokeSerialID, 0);
            m_invokeSerialID++;
        }

        /// <summary>
        /// 尺寸改变事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="e">参数</param>
        private void container_SizeChanged(object sender, EventArgs e) {
            if (m_native != null) {
                m_native.onResize();
            }
        }

        /// <summary>
        /// 复制文本
        /// </summary>
        /// <param name="text">文本</param>
        public override void copy(String text) {
            Clipboard.SetText(text);
        }

        /// <summary>
        /// 创建内部控件
        /// </summary>
        /// <param name="parent">父控件</param>
        /// <param name="clsid">类型ID</param>
        /// <returns>控件</returns>
        public override FCView createInternalControl(FCView parent, String clsid) {
            //日历控件
            FCCalendar calendar = parent as FCCalendar;
            if (calendar != null) {
                if (clsid == "datetitle") {
                    return new DateTitle(calendar);
                }
                else if (clsid == "headdiv") {
                    HeadDiv headDiv = new HeadDiv(calendar);
                    headDiv.Width = parent.Width;
                    headDiv.Dock = FCDockStyle.Top;
                    return headDiv;
                }
                else if (clsid == "lastbutton") {
                    return new ArrowButton(calendar);
                }
                else if (clsid == "nextbutton") {
                    ArrowButton nextBtn = new ArrowButton(calendar);
                    nextBtn.ToLast = false;
                    return nextBtn;
                }
            }
            //分割层
            FCSplitLayoutDiv splitLayoutDiv = parent as FCSplitLayoutDiv;
            if (splitLayoutDiv != null) {
                if (clsid == "splitter") {
                    FCButton splitter = new FCButton();
                    splitter.Size = new FCSize(5, 5);
                    return splitter;
                }
            }
            //滚动条
            FCScrollBar scrollBar = parent as FCScrollBar;
            if (scrollBar != null) {
                if (clsid == "addbutton") {
                    FCButton addButton = new FCButton();
                    addButton.Size = new FCSize(15, 15);
                    return addButton;
                }
                else if (clsid == "backbutton") {
                    return new FCButton();
                }
                else if (clsid == "scrollbutton") {
                    FCButton scrollButton = new FCButton();
                    scrollButton.AllowDrag = true;
                    return scrollButton;
                }
                else if (clsid == "reducebutton") {
                    FCButton reduceButton = new FCButton();
                    reduceButton.Size = new FCSize(15, 15);
                    return reduceButton;
                }
            }
            //页夹
            FCTabPage tabPage = parent as FCTabPage;
            if (tabPage != null) {
                if (clsid == "headerbutton") {
                    FCButton button = new FCButton();
                    button.AllowDrag = true;
                    FCSize size = new FCSize(100, 20);
                    button.Size = size;
                    return button;
                }
            }
            //下拉列表
            FCComboBox comboBox = parent as FCComboBox;
            if (comboBox != null) {
                if (clsid == "dropdownbutton") {
                    FCButton dropDownButton = new FCButton();
                    dropDownButton.DisplayOffset = false;
                    int width = comboBox.Width;
                    int height = comboBox.Height;
                    FCPoint location = new FCPoint(width - 16, 0);
                    dropDownButton.Location = location;
                    FCSize size = new FCSize(16, height);
                    dropDownButton.Size = size;
                    return dropDownButton;
                }
                else if (clsid == "dropdownmenu") {
                    FCComboBoxMenu comboBoxMenu = new FCComboBoxMenu();
                    comboBoxMenu.ComboBox = comboBox;
                    comboBoxMenu.Popup = true;
                    FCSize size = new FCSize(100, 200);
                    comboBoxMenu.Size = size;
                    return comboBoxMenu;
                }
            }
            //日期选择
            FCDateTimePicker datePicker = parent as FCDateTimePicker;
            if (datePicker != null) {
                if (clsid == "dropdownbutton") {
                    FCButton dropDownButton = new FCButton();
                    dropDownButton.DisplayOffset = false;
                    int width = datePicker.Width;
                    int height = datePicker.Height;
                    FCPoint location = new FCPoint(width - 16, 0);
                    dropDownButton.Location = location;
                    FCSize size = new FCSize(16, height);
                    dropDownButton.Size = size;
                    return dropDownButton;
                }
                else if (clsid == "dropdownmenu") {
                    FCMenu dropDownMenu = new FCMenu();
                    dropDownMenu.Padding = new FCPadding(1);
                    dropDownMenu.Popup = true;
                    FCSize size = new FCSize(200, 200);
                    dropDownMenu.Size = size;
                    return dropDownMenu;
                }
            }
            //数字选择
            FCSpin spin = parent as FCSpin;
            if (spin != null) {
                if (clsid == "downbutton") {
                    FCButton downButton = new FCButton();
                    downButton.DisplayOffset = false;
                    FCSize size = new FCSize(16, 16);
                    downButton.Size = size;
                    return downButton;
                }
                else if (clsid == "upbutton") {
                    FCButton upButton = new FCButton();
                    upButton.DisplayOffset = false;
                    FCSize size = new FCSize(16, 16);
                    upButton.Size = size;
                    return upButton;
                }
            }
            //容器层
            FCDiv div = parent as FCDiv;
            if (div != null) {
                if (clsid == "hscrollbar") {
                    FCHScrollBar hScrollBar = new FCHScrollBar();
                    hScrollBar.Visible = false;
                    hScrollBar.Size = new FCSize(15, 15);
                    return hScrollBar;
                }
                else if (clsid == "vscrollbar") {
                    FCVScrollBar vScrollBar = new FCVScrollBar();
                    vScrollBar.Visible = false;
                    vScrollBar.Size = new FCSize(15, 15);
                    return vScrollBar;
                }
            }
            //表格
            FCGrid grid = parent as FCGrid;
            if (grid != null) {
                if (clsid == "edittextbox") {
                    return new FCTextBox();
                }
            }
            return null;
        }

        /// <summary>
        /// 销毁资源
        /// </summary>
        public override void delete() {
            if (!m_isDeleted) {
                if (m_toolTip != null) {
                    m_native.removeControl(m_toolTip);
                    m_toolTip.delete();
                }
                m_invokeArgs.clear();
                m_invokeControls.clear();
                m_container = null;
                m_isDeleted = true;
            }
        }

        /// <summary>
        /// 获取程序的路径
        /// </summary>
        /// <returns>路径</returns>
        public static String getAppPath() {
            return System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
        }

        /// <summary>
        /// 获取客户端的大小
        /// </summary>
        /// <returns>大小</returns>
        private FCSize getClientSize() {
            int width = 0, height = 0;
            if (m_container != null) {
                width = m_container.ClientSize.Width;
                height = m_container.ClientSize.Height;
            }
            return new FCSize(width, height);
        }

        /// <summary>
        /// 获取光标
        /// </summary>
        /// <returns>光标</returns>
        public override FCCursors getCursor() {
            if (m_container != null) {
                Cursor cursor = m_container.Cursor;
                if (cursor == Cursors.Default || cursor == Cursors.Arrow) {
                    return FCCursors.Arrow;
                }
                else if (cursor == Cursors.AppStarting) {
                    return FCCursors.AppStarting;
                }
                else if (cursor == Cursors.Cross) {
                    return FCCursors.Cross;
                }
                else if (cursor == Cursors.Hand) {
                    return FCCursors.Hand;
                }
                else if (cursor == Cursors.Help) {
                    return FCCursors.Help;
                }
                else if (cursor == Cursors.IBeam) {
                    return FCCursors.IBeam;
                }
                else if (cursor == Cursors.No) {
                    return FCCursors.No;
                }
                else if (cursor == Cursors.SizeAll) {
                    return FCCursors.SizeAll;
                }
                else if (cursor == Cursors.SizeNESW) {
                    return FCCursors.SizeNESW;
                }
                else if (cursor == Cursors.SizeNS) {
                    return FCCursors.SizeNS;
                }
                else if (cursor == Cursors.SizeNWSE) {
                    return FCCursors.SizeNWSE;
                }
                else if (cursor == Cursors.SizeWE) {
                    return FCCursors.SizeWE;
                }
                else if (cursor == Cursors.UpArrow) {
                    return FCCursors.UpArrow;
                }
                else if (cursor == Cursors.WaitCursor) {
                    return FCCursors.WaitCursor;
                }
            }
            return FCCursors.Arrow;
        }

        /// <summary>
        /// 获取矩形相交区
        /// </summary>
        /// <param name="lpDestRect">相交矩形</param>
        /// <param name="lpSrc1Rect">矩形1</param>
        /// <param name="lpSrc2Rect">矩形2</param>
        /// <returns>是否相交</returns>
        public override int getIntersectRect(ref FCRect lpDestRect, ref FCRect lpSrc1Rect, ref FCRect lpSrc2Rect) {
            return IntersectRect(ref lpDestRect, ref lpSrc1Rect, ref lpSrc2Rect);
        }

        /// <summary>
        /// 获取矩形并集区
        /// </summary>
        /// <param name="lpDestRect">并集矩形</param>
        /// <param name="lpSrc1Rect">矩形1</param>
        /// <param name="lpSrc2Rect">矩形2</param>
        /// <returns>是否相交</returns>
        public override int getUnionRect(ref FCRect lpDestRect, ref FCRect lpSrc1Rect, ref FCRect lpSrc2Rect) {
            return UnionRect(ref lpDestRect, ref lpSrc1Rect, ref lpSrc2Rect);
        }

        /// <summary>
        /// 获取触摸位置
        /// </summary>
        /// <returns>坐标</returns>
        public override FCPoint getTouchPoint() {
            int px = 0, py = 0;
            if (m_container != null) {
                Point mp = m_container.PointToClient(Control.MousePosition);
                px = mp.X;
                py = mp.Y;
                if (m_native.AllowScaleSize) {
                    FCSize clientSize = getClientSize();
                    if (clientSize.cx > 0 && clientSize.cy > 0) {
                        FCSize scaleSize = m_native.ScaleSize;
                        px = px * scaleSize.cx / clientSize.cx;
                        py = py * scaleSize.cy / clientSize.cy;
                    }
                }
            }
            return new FCPoint(px, py);
        }

        /// <summary>
        /// 获取尺寸
        /// </summary>
        /// <returns>大小</returns>
        public override FCSize getSize() {
            if (m_native.AllowScaleSize) {
                return m_native.ScaleSize;
            }
            else {
                return getClientSize();
            }
        }

        /// <summary>
        /// 刷新绘图
        /// </summary>
        public override void invalidate() {
            if (m_container != null) {
                m_container.Invalidate();
            }
        }

        /// <summary>
        /// 刷新绘图
        /// </summary>
        /// <param name="rect">区域</param>
        public override void invalidate(FCRect rect) {
            if (m_allowPartialPaint) {
                onPaint(rect);
            }
            else {
                if (m_container != null) {
                    m_container.Invalidate();
                }
            }
        }

        /// <summary>
        /// 在控件的线程中调用方法
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="args">参数</param>
        public override void invoke(FCView control, object args) {
            lock (m_invokeArgs) {
                m_invokeArgs.put(m_invokeSerialID, args);
            }
            lock (m_invokeControls) {
                m_invokeControls.put(m_invokeSerialID, control);
            }
            IntPtr hWnd = m_hWnd;
            if (m_native.MirrorMode != FCMirrorMode.None) {
                WinHost host = m_native.MirrorHost.Host as WinHost;
                hWnd = host.HWnd;
            }
            PostMessage(hWnd, m_pInvokeMsgID, m_invokeSerialID, 0);
            m_invokeSerialID++;
        }

        /// <summary>
        /// 获取按键的状态
        /// </summary>
        /// <param name="key">按键</param>
        /// <returns>状态</returns>
        public override bool isKeyPress(int key) {
            int state = GetKeyState(key) & 0x8000;
            if (state > 0) {
                return true;
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// 调用控件线程的方法
        /// </summary>
        /// <param name="invokeSerialID">消息ID</param>
        public virtual void onInvoke(int invokeSerialID) {
            object args = null;
            FCView control = null;
            lock (m_invokeArgs) {
                if (m_invokeArgs.containsKey(invokeSerialID)) {
                    args = m_invokeArgs.get(invokeSerialID);
                    m_invokeArgs.remove(invokeSerialID);
                }
            }
            lock (m_invokeControls) {
                if (m_invokeControls.containsKey(invokeSerialID)) {
                    control = m_invokeControls.get(invokeSerialID);
                    m_invokeControls.remove(invokeSerialID);
                    m_invokeArgs.put(m_invokeSerialID, control);
                }
            }
            if (control != null) {
                control.onInvoke(args);
            }
        }

        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="m">消息</param>
        /// <returns>是否处理</returns>
        public virtual int onMessage(ref Message m) {
            if (m_native != null) {
                if (m.Msg == WM_IME_SETCONTEXT && m.WParam.ToInt32() == 1) {
                    ImmAssociateContext(m_hWnd, m_hImc);
                }
                if (m.Msg == m_pInvokeMsgID) {
                    onInvoke(m.WParam.ToInt32());
                }
                switch (m.Msg) {
                    case WM_CHAR: {
                            if (m_allowOperate) {
                                KeyEventArgs e = new KeyEventArgs(((Keys)((int)((long)m.WParam))) | Control.ModifierKeys);
                                char key = (char)e.KeyData;
                                if (m_native.onChar(key)) {
                                    return 1;
                                }
                            }
                            break;
                        }
                    case WM_KEYDOWN:
                    case WM_SYSKEYDOWN: {
                            if (m_allowOperate) {
                                KeyEventArgs e = new KeyEventArgs(((Keys)((int)((long)m.WParam))) | Control.ModifierKeys);
                                char key = (char)e.KeyData;
                                bool handle = m_native.onPreviewKeyEvent(FCEventID.KEYDOWN, key);
                                if (handle) {
                                    return 1;
                                }
                                else {
                                    m_native.onKeyDown(key);
                                }
                            }
                            break;
                        }
                    case WM_KEYUP:
                    case WM_SYSKEYUP: {
                            if (m_allowOperate) {
                                KeyEventArgs e = new KeyEventArgs(((Keys)((int)((long)m.WParam))) | Control.ModifierKeys);
                                char key = (char)e.KeyData;
                                m_native.onKeyUp(key);
                            }
                            break;
                        }
                    case WM_ERASEBKGND:
                        return 1;
                    case WM_IME_CHAR: {
                            if (m_allowOperate) {
                                if (m.WParam.ToInt32() == PM_REMOVE) {
                                    FCView control = m_native.FocusedControl;
                                    if (control != null) {
                                        StringBuilder str = new StringBuilder();
                                        int size = ImmGetCompositionString(m_hImc, GCS_COMPSTR, null, 0);
                                        size += sizeof(Char);
                                        ImmGetCompositionString(m_hImc, GCS_RESULTSTR, str, size);
                                    }
                                }
                            }
                            break;
                        }
                    case WM_LBUTTONDBLCLK: {
                            if (m_allowOperate) {
                                FCTouchInfo newTouchInfo = new FCTouchInfo();
                                newTouchInfo.m_firstTouch = true;
                                newTouchInfo.m_clicks = 2;
                                m_native.onMouseDown(newTouchInfo);
                                m_native.onDoubleClick(newTouchInfo);
                            }
                            break;
                        }
                    case WM_LBUTTONDOWN: {
                            if (m_allowOperate) {
                                activeMirror();
                                FCTouchInfo newTouchInfo = new FCTouchInfo();
                                newTouchInfo.m_firstTouch = true;
                                newTouchInfo.m_clicks = 1;
                                m_native.onMouseDown(newTouchInfo);
                            }
                            break;
                        }

                    case WM_LBUTTONUP: {
                            if (m_allowOperate) {
                                FCTouchInfo newTouchInfo = new FCTouchInfo();
                                newTouchInfo.m_firstTouch = true;
                                newTouchInfo.m_clicks = 1;
                                m_native.onMouseUp(newTouchInfo);
                            }
                            break;
                        }
                    case WM_MOUSEMOVE: {
                            if (m_allowOperate) {
                                if ((int)m.WParam == 1) {
                                    FCTouchInfo newTouchInfo = new FCTouchInfo();
                                    newTouchInfo.m_firstTouch = true;
                                    newTouchInfo.m_clicks = 1;
                                    m_native.onMouseMove(newTouchInfo);
                                }
                                else if ((int)m.WParam == 2) {
                                    FCTouchInfo newTouchInfo = new FCTouchInfo();
                                    newTouchInfo.m_secondTouch = true;
                                    newTouchInfo.m_clicks = 1;
                                    m_native.onMouseMove(newTouchInfo);
                                }
                                else {
                                    FCTouchInfo newTouchInfo = new FCTouchInfo();
                                    m_native.onMouseMove(newTouchInfo);
                                }
                            }
                            break;
                        }
                    case WM_MOUSEWHEEL: {
                            if (m_allowOperate) {
                                FCTouchInfo newTouchInfo = new FCTouchInfo();
                                newTouchInfo.m_delta = (int)m.WParam;
                                m_native.onMouseWheel(newTouchInfo);
                            }
                            break;
                        }
                    case WM_PAINT: {
                            FCSize displaySize = m_native.DisplaySize;
                            FCRect paintRect = new FCRect(0, 0, displaySize.cx, displaySize.cy);
                            onPaint(paintRect);
                            break;
                        }
                    case WM_RBUTTONDBLCLK: {
                            if (m_allowOperate) {
                                FCTouchInfo newTouchInfo = new FCTouchInfo();
                                newTouchInfo.m_secondTouch = true;
                                newTouchInfo.m_clicks = 2;
                                m_native.onMouseDown(newTouchInfo);
                                m_native.onDoubleClick(newTouchInfo);
                            }
                            break;
                        }
                    case WM_RBUTTONDOWN: {
                            if (m_allowOperate) {
                                activeMirror();
                                FCTouchInfo newTouchInfo = new FCTouchInfo();
                                newTouchInfo.m_secondTouch = true;
                                newTouchInfo.m_clicks = 1;
                                m_native.onMouseDown(newTouchInfo);
                            }
                            break;
                        }
                    case WM_RBUTTONUP: {
                            if (m_allowOperate) {
                                FCTouchInfo newTouchInfo = new FCTouchInfo();
                                newTouchInfo.m_secondTouch = true;
                                newTouchInfo.m_clicks = 1;
                                m_native.onMouseUp(newTouchInfo);
                            }
                            break;
                        }
                    case WM_TIMER: {
                            if (m.HWnd == m_hWnd) {
                                int timerID = (int)m.WParam;
                                m_native.onTimer(timerID);
                            }
                            break;
                        }
                }
            }
            return 0;
        }

        /// <summary>
        /// 重绘方法
        /// </summary>
        /// <param name="clipRect">裁剪区域</param>
        public void onPaint(FCRect clipRect) {
            lock (this) {
                if (m_container != null) {
                    FCSize displaySize = m_native.DisplaySize;
                    double scaleFactorX = 1, scaleFactorY = 1;
                    FCSize clientSize = getClientSize();
                    if (m_native.AllowScaleSize) {
                        if (clientSize.cx > 0 && clientSize.cy > 0) {
                            FCSize scaleSize = m_native.ScaleSize;
                            scaleFactorX = (double)(clientSize.cx) / scaleSize.cx;
                            scaleFactorY = (double)(clientSize.cy) / scaleSize.cy;
                        }
                    }
                    Graphics g = m_container.CreateGraphics();
                    m_native.Paint.setScaleFactor(scaleFactorX, scaleFactorY);
                    m_native.Paint.beginPaint(g.GetHdc(), new FCRect(0, 0, clientSize.cx, clientSize.cy), clipRect);
                    m_native.onPaint(clipRect);
                    m_native.Paint.endPaint();
                    g.Dispose();
                }
            }
        }

        /// <summary>
        /// 获取粘贴文本
        /// </summary>
        /// <returns>文本</returns>
        public override String paste() {
            return Clipboard.GetText();
        }

        /// <summary>
        /// 设置光标
        /// </summary>
        /// <param name="cursor">光标</param>
        public override void setCursor(FCCursors cursor) {
            if (m_container != null) {
                FCCursors csr = cursor;
                if (csr == FCCursors.Arrow) {
                    m_container.Cursor = Cursors.Arrow;
                }
                else if (csr == FCCursors.AppStarting) {
                    m_container.Cursor = Cursors.AppStarting;
                }
                else if (csr == FCCursors.Cross) {
                    m_container.Cursor = Cursors.Cross;
                }
                else if (csr == FCCursors.Hand) {
                    m_container.Cursor = Cursors.Hand;
                }
                else if (csr == FCCursors.Help) {
                    m_container.Cursor = Cursors.Help;
                }
                else if (csr == FCCursors.IBeam) {
                    m_container.Cursor = Cursors.IBeam;
                }
                else if (csr == FCCursors.No) {
                    m_container.Cursor = Cursors.No;
                }
                else if (csr == FCCursors.SizeAll) {
                    m_container.Cursor = Cursors.SizeAll;
                }
                else if (csr == FCCursors.SizeNESW) {
                    m_container.Cursor = Cursors.SizeNESW;
                }
                else if (csr == FCCursors.SizeNS) {
                    m_container.Cursor = Cursors.SizeNS;
                }
                else if (csr == FCCursors.SizeNWSE) {
                    m_container.Cursor = Cursors.SizeNWSE;
                }
                else if (csr == FCCursors.SizeWE) {
                    m_container.Cursor = Cursors.SizeWE;
                }
                else if (csr == FCCursors.UpArrow) {
                    m_container.Cursor = Cursors.UpArrow;
                }
                else if (csr == FCCursors.WaitCursor) {
                    m_container.Cursor = Cursors.WaitCursor;
                }
            }
        }

        /// <summary>
        /// 显示提示框
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="mp">位置</param>
        public override void showToolTip(String text, FCPoint mp) {
            if (m_toolTip != null) {
                m_toolTip.Native = m_native;
                m_toolTip.Location = mp;
                m_toolTip.Text = text;
                m_toolTip.show();
            }
        }

        /// <summary>
        /// 开启秒表
        /// </summary>
        /// <param name="timerID">秒表ID</param>
        /// <param name="interval">间隔</param>
        public override void startTimer(int timerID, int interval) {
            IntPtr hWnd = m_hWnd;
            if (m_native.MirrorMode != FCMirrorMode.None) {
                WinHost host = m_native.MirrorHost.Host as WinHost;
                hWnd = host.HWnd;
            }
            SetTimer(hWnd, timerID, interval, null);
        }

        /// <summary>
        /// 停止秒表
        /// </summary>
        /// <param name="timerID">秒表ID</param>
        public override void stopTimer(int timerID) {
            IntPtr hWnd = m_hWnd;
            if (m_native.MirrorMode != FCMirrorMode.None) {
                WinHost host = m_native.MirrorHost.Host as WinHost;
                hWnd = host.HWnd;
            }
            KillTimer(hWnd, timerID);
        }
    }
}
