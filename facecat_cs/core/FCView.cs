/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

using System;
using System.Collections.Generic;

namespace FaceCat {
    /// <summary>
    /// 控件事件
    /// </summary>
    /// <param name="sender">调用者</param>
    public delegate void FCEvent(object sender);

    /// <summary>
    /// 在线程中调用方法事件
    /// </summary>
    /// <param name="sender">调用者</param>
    /// <param name="args">参数</param>
    public delegate void FCInvokeEvent(object sender, object args);

    /// <summary>
    /// 键盘事件
    /// </summary>
    /// <param name="sender">调用者</param>
    /// <param name="key">按键</param>
    public delegate void FCKeyEvent(object sender, char key);

    /// <summary>
    /// 键盘事件
    /// </summary>
    /// <param name="sender">调用者</param>
    /// <param name="eventID">事件ID</param>
    /// <param name="key">按键</param>
    public delegate bool FCPreviewKeyEvent(object sender, int eventID, char key);

    /// <summary>
    /// 控件触摸事件
    /// </summary>
    /// <param name="sender">调用者</param>
    /// <param name="mp">触摸位置</param>
    /// <param name="button">按钮</param>
    /// <param name="clicks">点击次数</param>
    /// <param name="delta">滚轮值</param>
    public delegate void FCTouchEvent(object sender, FCTouchInfo touchInfo);

    /// <summary>
    /// 控件触摸事件
    /// </summary>
    /// <param name="sender">调用者</param>
    /// <param name="eventID">事件ID</param>
    /// <param name="mp">触摸位置</param>
    /// <param name="button">按钮</param>
    /// <param name="clicks">点击次数</param>
    /// <param name="delta">滚轮值</param>
    public delegate bool FCPreviewTouchEvent(object sender, int eventID, FCTouchInfo touchInfo);

    /// <summary>
    /// 控件重绘事件
    /// </summary>
    /// <param name="sender">调用者</param>
    /// <param name="paint">绘图对象</param>
    /// <param name="clipRect">裁剪区域</param>
    public delegate void FCPaintEvent(object sender, FCPaint paint, FCRect clipRect);

    /// <summary>
    /// 秒表事件
    /// </summary>
    /// <param name="sender">调用者</param>
    /// <param name="timerID">秒表ID</param>
    public delegate void FCTimerEvent(object sender, int timerID);

    /// <summary>
    /// 事件编号
    /// </summary>
    public class FCEventID {
        public const int ADD = 0;
        public const int AUTOSIZECHANGED = 1;
        public const int BACKCOLORCHANGED = 2;
        public const int BACKIMAGECHANGED = 3;
        public const int CHAR = 4;
        public const int CLICK = 5;
        public const int COPY = 6;
        public const int CUT = 7;
        public const int DOCKCHANGED = 8;
        public const int DOUBLECLICK = 9;
        public const int DRAGBEGIN = 10;
        public const int DRAGEND = 11;
        public const int DRAGGING = 12;
        public const int ENABLECHANGED = 13;
        public const int FONTCHANGED = 14;
        public const int TEXTCOLORCHANGED = 15;
        public const int GOTFOCUS = 16;
        public const int INVOKE = 17;
        public const int KEYDOWN = 18;
        public const int KEYUP = 19;
        public const int LOAD = 20;
        public const int LOCATIONCHANGED = 21;
        public const int LOSTFOCUS = 22;
        public const int MARGINCHANGED = 23;
        public const int TOUCHDOWN = 24;
        public const int TOUCHENTER = 25;
        public const int TOUCHLEAVE = 26;
        public const int TOUCHMOVE = 27;
        public const int TOUCHUP = 28;
        public const int TOUCHWHEEL = 29;
        public const int PADDINGCHANGED = 30;
        public const int PARENTCHANGED = 31;
        public const int PAINT = 32;
        public const int PAINTBORDER = 33;
        public const int PASTE = 34;
        public const int REGIONCHANGED = 36;
        public const int REMOVE = 37;
        public const int SIZECHANGED = 38;
        public const int TABINDEXCHANGED = 39;
        public const int TABSTOP = 40;
        public const int TABSTOPCHANGED = 41;
        public const int TEXTCHANGED = 42;
        public const int TIMER = 43;
        public const int VISIBLECHANGED = 44;
        public const int CHECKEDCHANGED = 45;
        public const int SELECTEDTIMECHANGED = 46;
        public const int GRIDCELLCLICK = 47;
        public const int GRIDCELLEDITBEGIN = 48;
        public const int GRIDCELLEDITEND = 49;
        public const int GRIDCELLTOUCHDOWN = 50;
        public const int GRIDCELLTOUCHENTER = 51;
        public const int GRIDCELLTOUCHLEAVE = 52;
        public const int GRIDCELLTOUCHMOVE = 53;
        public const int GRIDCELLTOUCHUP = 54;
        public const int GRIDSELECTEDCELLSCHANGED = 55;
        public const int GRIDSELECTEDCOLUMNSSCHANGED = 56;
        public const int GRIDSELECTEDROWSCHANGED = 57;
        public const int MENUITEMCLICK = 58;
        public const int SELECTEDINDEXCHANGED = 59;
        public const int SELECTEDTABPAGECHANGED = 60;
        public const int SCROLLED = 61;
        public const int VALUECHANGED = 62;
        public const int WINDOWCLOSED = 63;
        public const int WINDOWCLOSING = 64;
        public const int PREVIEWSKEYEVENT = 65;
        public const int PREVIEWSTOUCHEVENT = 66;
        public const int USER = 100000;
    }

    /// <summary>
    /// 自定义集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ArrayList<T> : List<T> {
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="t">数据</param>
        public void add(T t) {
            Add(t);
        }

        /// <summary>
        /// 清除数据
        /// </summary>
        public void clear() {
            Clear();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>数据</returns>
        public T get(int index) {
            return this[index];
        }

        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="t">数据</param>
        public void remove(T t) {
            Remove(t);
        }

        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="index">索引</param>
        public void removeAt(int index) {
            RemoveAt(index);
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="t">数据</param>
        public void set(int index, T t) {
            this[index] = t;
        }

        /// <summary>
        /// 尺寸
        /// </summary>
        /// <returns></returns>
        public int size() {
            return Count;
        }
    }

    /// <summary>
    /// 自定义哈希表
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class HashMap<K, V> : Dictionary<K, V> {
        /// <summary>
        /// 清除数据
        /// </summary>
        public void clear() {
            Clear();
        }

        /// <summary>
        /// 是否包含键
        /// </summary>
        /// <param name="k">键</param>
        /// <returns>是否包含</returns>
        public bool containsKey(K k) {
            return ContainsKey(k);
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="k">键</param>
        /// <returns>值</returns>
        public V get(K k) {
            return this[k];
        }

        /// <summary>
        /// 填充数据
        /// </summary>
        /// <param name="k">键</param>
        /// <param name="v">值</param>
        public void put(K k, V v) {
            this[k] = v;
        }

        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="k">键</param>
        public void remove(K k) {
            Remove(k);
        }

        /// <summary>
        /// 获取尺寸
        /// </summary>
        /// <returns>尺寸</returns>
        public int size() {
            return Count;
        }
    }

    /// <summary>
    /// 图层内置控件的基类
    /// </summary>
    public class FCView : FCProperty {
        /// <summary>
        /// 创建控件
        /// </summary>
        public FCView() {
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~FCView() {
            delete();
        }

        /// <summary>
        /// 控件集合
        /// </summary>
        public ArrayList<FCView> m_controls = new ArrayList<FCView>();

        /// <summary>
        /// 事件集合
        /// </summary>
        protected HashMap<int, ArrayList<object>> m_events;

        /// <summary>
        /// 上次坐标
        /// </summary>
        protected FCPoint m_oldLocation = new FCPoint();

        /// <summary>
        /// 上次尺寸
        /// </summary>
        protected FCSize m_oldSize = new FCSize();

        /// <summary>
        /// 百分比位置
        /// </summary>
        protected FCPointF2 m_percentLocation = null;

        /// <summary>
        /// 百分比尺寸
        /// </summary>
        protected FCSizeF2 m_percentSize = null;

        protected FCHorizontalAlign m_align = FCHorizontalAlign.Left;

        /// <summary>
        /// 获取或设置横向排列方式
        /// </summary>
        public FCHorizontalAlign Align {
            get { return m_align; }
            set { m_align = value; }
        }

        protected bool m_allowDrag;

        /// <summary>
        /// 获取或设置是否可以拖动位置
        /// </summary>
        public virtual bool AllowDrag {
            get { return m_allowDrag; }
            set { m_allowDrag = value; }
        }

        protected bool m_allowPreviewsEvent;

        /// <summary>
        /// 获取或设置是否允许预处理事件
        /// </summary>
        public virtual bool AllowPreviewsEvent {
            get { return m_allowPreviewsEvent; }
            set { m_allowPreviewsEvent = value; }
        }

        protected FCAnchor m_anchor = new FCAnchor(true, true, false, false);

        /// <summary>
        /// 获取或设置锚定信息
        /// </summary>
        public virtual FCAnchor Anchor {
            get { return m_anchor; }
            set { m_anchor = value; }
        }

        protected bool m_autoEllipsis;

        /// <summary>
        /// 获取或设置是否在文字超出范围时在结尾显示省略号
        /// </summary>
        public bool AutoEllipsis {
            get { return m_autoEllipsis; }
            set { m_autoEllipsis = value; }
        }

        protected bool m_autoSize;

        /// <summary>
        /// 获取或设置是否自动调整尺寸
        /// </summary>
        public virtual bool AutoSize {
            get { return m_autoSize; }
            set {
                if (m_autoSize != value) {
                    m_autoSize = value;
                    onAutoSizeChanged();
                }
            }
        }

        protected long m_backColor = FCColor.Back;

        /// <summary>
        /// 获取或设置背景色
        /// </summary>
        public virtual long BackColor {
            get { return m_backColor; }
            set {
                if (m_backColor != value) {
                    m_backColor = value;
                    onBackColorChanged();
                }
            }
        }

        protected String m_backImage;

        /// <summary>
        /// 获取或设置背景图片
        /// </summary>
        public virtual String BackImage {
            get { return m_backImage; }
            set {
                if (m_backImage != value) {
                    m_backImage = value;
                    onBackImageChanged();
                }
            }
        }

        protected long m_borderColor = FCColor.Border;

        /// <summary>
        /// 获取或设置边线的颜色
        /// </summary>
        public virtual long BorderColor {
            get { return m_borderColor; }
            set { m_borderColor = value; }
        }

        /// <summary>
        /// 获取距离下侧的位置
        /// </summary>
        public virtual int Bottom {
            get { return Top + Height; }
        }

        /// <summary>
        /// 获取或设置控件的区域属性
        /// </summary>
        public virtual FCRect Bounds {
            get { return new FCRect(Left, Top, Right, Bottom); }
            set {
                Location = new FCPoint(value.left, value.top);
                int cx = value.right - value.left;
                int cy = value.bottom - value.top;
                Size = new FCSize(cx, cy);
            }
        }

        protected bool m_canFocus = true;

        /// <summary>
        /// 获取或设置是否可以设置焦点
        /// </summary>
        public bool CanFocus {
            get { return m_canFocus; }
            set { m_canFocus = value; }
        }

        protected bool m_canRaiseEvents = true;

        /// <summary>
        /// 获取或设置是否可以触发事件
        /// </summary>
        public virtual bool CanRaiseEvents {
            get { return m_canRaiseEvents; }
            set { m_canRaiseEvents = value; }
        }

        /// <summary>
        /// 获取是否被触摸捕获
        /// </summary>
        public virtual bool Capture {
            get {
                if (m_native != null) {
                    if (m_native.HoveredControl == this || m_native.PushedControl == this) {
                        return true;
                    }
                }
                return false;
            }
        }

        protected int m_cornerRadius;

        /// <summary>
        /// 获取或设置圆角角度
        /// </summary>
        public virtual int CornerRadius {
            get { return m_cornerRadius; }
            set { m_cornerRadius = value; }
        }

        protected FCCursors m_cursor = FCCursors.Arrow;

        /// <summary>
        /// 获取或设置光标
        /// </summary>
        public virtual FCCursors Cursor {
            get { return m_cursor; }
            set { m_cursor = value; }
        }

        protected bool m_displayOffset = true;

        /// <summary>
        /// 获取或设置是否允许偏移显示
        /// </summary>
        public bool DisplayOffset {
            get { return m_displayOffset; }
            set { m_displayOffset = value; }
        }

        /// <summary>
        /// 获取虚拟显示的区域
        /// </summary>
        public virtual FCRect DisplayRect {
            get {
                if (m_useRegion) {
                    return m_region;
                }
                else {
                    return new FCRect(0, 0, Width, Height);
                }
            }
        }

        protected FCDockStyle m_dock = FCDockStyle.None;

        /// <summary>
        /// 获取或设置绑定边缘类型
        /// </summary>
        public virtual FCDockStyle Dock {
            get { return m_dock; }
            set {
                if (m_dock != value) {
                    m_dock = value;
                    onDockChanged();
                }
            }
        }

        protected bool m_enabled = true;

        /// <summary>
        /// 获取或设置控件是否可用
        /// </summary>
        public virtual bool Enabled {
            get { return m_enabled; }
            set {
                if (m_enabled != value) {
                    m_enabled = value;
                    onEnableChanged();
                }
            }
        }

        /// <summary>
        /// 获取或设置是否具有焦点
        /// </summary>
        public virtual bool Focused {
            get {
                if (m_native != null) {
                    if (m_native.FocusedControl == this) {
                        return true;
                    }
                }
                return false;
            }
            set {
                if (m_native != null) {
                    if (value) {
                        m_native.FocusedControl = this;
                    }
                    else {
                        if (m_native.FocusedControl == this) {
                            m_native.FocusedControl = null;
                        }
                    }
                }
            }
        }

        protected FCFont m_font = new FCFont();

        /// <summary>
        /// 获取或设置字体
        /// </summary>
        public virtual FCFont Font {
            get { return m_font; }
            set {
                m_font = value;
                onFontChanged();
            }
        }

        protected bool m_hasPopupMenu;
        /// <summary>
        /// 获取或设置是否有右键菜单
        /// </summary>
        public virtual bool HasPopupMenu {
            get { return m_hasPopupMenu; }
            set { m_hasPopupMenu = value; }
        }


        /// <summary>
        /// 获取或设置控件的高度
        /// </summary>
        public virtual int Height {
            get {
                if (m_percentSize != null && m_percentSize.cy != -1) {
                    FCSize parentSize = (m_parent != null ? m_parent.Size : m_native.DisplaySize);
                    return (int)(parentSize.cy * m_percentSize.cy);
                }
                else {
                    return m_size.cy;
                }
            }
            set {
                if (m_percentSize != null && m_percentSize.cy != -1) {
                    return;
                }
                else {
                    Size = new FCSize(m_size.cx, value);
                }
            }
        }

        protected bool m_isDeleted;

        /// <summary>
        /// 获取或设置是否已被销毁
        /// </summary>
        public virtual bool IsDeleted {
            get { return m_isDeleted; }
        }

        protected bool m_isDragging;

        /// <summary>
        /// 获取或设置是否正被拖动
        /// </summary>
        public virtual bool IsDragging {
            get { return m_isDragging; }
        }

        protected bool m_isWindow;

        /// <summary>
        /// 获取或设置是否为窗体
        /// </summary>
        public virtual bool IsWindow {
            get { return m_isWindow; }
            set { m_isWindow = value; }
        }

        /// <summary>
        /// 获取或设置距离左侧的位置
        /// </summary>
        public virtual int Left {
            get {
                if (m_percentLocation != null && m_percentLocation.x != -1) {
                    FCSize parentSize = (m_parent != null ? m_parent.Size : m_native.DisplaySize);
                    return (int)(parentSize.cx * m_percentLocation.x);
                }
                else {
                    return m_location.x;
                }
            }
            set {
                if (m_percentLocation != null && m_percentLocation.x != -1) {
                    return;
                }
                else {
                    Location = new FCPoint(value, m_location.y);
                }
            }
        }

        protected FCPoint m_location;

        /// <summary>
        /// 获取或设置控件的位置
        /// </summary>
        public virtual FCPoint Location {
            get {
                if (m_percentLocation != null) {
                    FCPoint location = new FCPoint(Left, Top);
                    return location;
                }
                else {
                    return m_location;
                }
            }
            set {
                if (m_location.x != value.x || m_location.y != value.y) {
                    if (m_percentLocation != null) {
                        m_oldLocation = Location;
                        if (m_percentLocation != null && m_percentLocation.x != -1) {
                        }
                        else {
                            m_location.x = value.x;
                        }
                        if (m_percentLocation != null && m_percentLocation.y != -1) {
                        }
                        else {
                            m_location.y = value.y;
                        }
                    }
                    else {
                        m_oldLocation = m_location;
                        m_location = value;
                    }
                    onLocationChanged();
                }
            }
        }

        protected FCPadding m_margin = new FCPadding(0);

        /// <summary>
        /// 获取或设置外边距
        /// </summary>
        public virtual FCPadding Margin {
            get { return m_margin; }
            set {
                m_margin = value;
                onMarginChanged();
            }
        }

        protected FCSize m_maximumSize = new FCSize(100000, 100000);

        /// <summary>
        /// 获取或设置控件的最大尺寸
        /// </summary>
        public virtual FCSize MaximumSize {
            get { return m_maximumSize; }
            set { m_maximumSize = value; }
        }

        protected FCSize m_minimumSize;

        /// <summary>
        /// 获取或设置控件的最小尺寸
        /// </summary>
        public virtual FCSize MinimumSize {
            get { return m_minimumSize; }
            set { m_minimumSize = value; }
        }

        protected String m_name = String.Empty;

        /// <summary>
        /// 获取或设置控件的唯一标识名称
        /// </summary>
        public virtual String Name {
            get { return m_name; }
            set { m_name = value; }
        }

        protected FCNative m_native;

        /// <summary>
        /// 获取或设置按钮所在的图形接口
        /// </summary>
        public virtual FCNative Native {
            get { return m_native; }
            set {
                m_native = value;
                int controlsSize = m_controls.size();
                for (int i = 0; i < controlsSize; i++) {
                    m_controls.get(i).Native = value;
                }
                onLoad();
            }
        }

        protected float m_opacity = 1;

        /// <summary>
        /// 获取或设置透明度
        /// </summary>
        public virtual float Opacity {
            get { return m_opacity; }
            set { m_opacity = value; }
        }

        protected FCPadding m_padding = new FCPadding(0);

        /// <summary>
        /// 获取或设置内边距
        /// </summaryz
        public virtual FCPadding Padding {
            get { return m_padding; }
            set {
                m_padding = value;
                onPaddingChanged();
            }
        }

        protected FCView m_parent;

        /// <summary>
        /// 获取或设置父控件
        /// </summary>
        public virtual FCView Parent {
            get { return m_parent; }
            set {
                if (m_parent != value) {
                    m_parent = value;
                    onParentChanged();
                }
            }
        }

        protected FCRect m_region;

        /// <summary>
        /// 获取或设置裁剪矩形
        /// </summary>
        public virtual FCRect Region {
            get { return m_region; }
            set {
                m_useRegion = true;
                m_region = value;
                onRegionChanged();
            }
        }

        protected String m_resourcePath;

        /// <summary>
        /// 获取或设置资源路径
        /// </summary>
        public virtual String ResourcePath {
            get { return m_resourcePath; }
            set { m_resourcePath = value; }
        }

        /// <summary>
        /// 获取距离右侧的距离
        /// </summary>
        public virtual int Right {
            get { return Left + Width; }
        }

        protected FCSize m_size;

        /// <summary>
        /// 获取或设置尺寸
        /// </summary>
        public virtual FCSize Size {
            get {
                if (m_percentSize != null) {
                    return new FCSize(Width, Height);
                }
                else {
                    return m_size;
                }
            }
            set {
                FCSize newSize = value;
                if (newSize.cx > m_maximumSize.cx) {
                    newSize.cx = m_maximumSize.cx;
                }
                if (newSize.cy > m_maximumSize.cy) {
                    newSize.cy = m_maximumSize.cy;
                }
                if (newSize.cx < m_minimumSize.cx) {
                    newSize.cx = m_minimumSize.cx;
                }
                if (newSize.cy < m_minimumSize.cy) {
                    newSize.cy = m_minimumSize.cy;
                }
                if (m_size.cx != newSize.cx || m_size.cy != newSize.cy) {
                    if (m_percentLocation != null) {
                        m_oldSize = Size;
                        if (m_percentSize != null && m_percentSize.cx != -1) {
                        }
                        else {
                            m_size.cx = newSize.cx;
                        }
                        if (m_percentSize != null && m_percentSize.cy != -1) {
                        }
                        else {
                            m_size.cy = newSize.cy;
                        }
                    }
                    else {
                        m_oldSize = m_size;
                        m_size = newSize;
                    }
                    onSizeChanged();
                    update();
                }
            }
        }

        protected int m_tabIndex;

        /// <summary>
        /// 获取或设置Tab键的顺序
        /// </summary>
        public virtual int TabIndex {
            get { return m_tabIndex; }
            set {
                if (m_tabIndex != value) {
                    m_tabIndex = value;
                    onTabIndexChanged();
                }
            }
        }

        protected bool m_tabStop;

        /// <summary>
        /// 获取或设置是否能用Tab键将焦点放置到该控件上
        /// </summary>
        public virtual bool TabStop {
            get { return m_tabStop; }
            set {
                if (m_tabStop != value) {
                    m_tabStop = value;
                    onTabStopChanged();
                }
            }
        }

        protected object m_tag = null;

        /// <summary>
        /// 获取或设置TAG值
        /// </summary>
        public virtual object Tag {
            get { return m_tag; }
            set { m_tag = value; }
        }

        protected String m_text = String.Empty;

        /// <summary>
        /// 获取或设置文字
        /// </summary>
        public virtual String Text {
            get { return m_text; }
            set {
                if (m_text != value) {
                    m_text = value;
                    onTextChanged();
                }
            }
        }

        protected long m_textColor = FCColor.Text;

        /// <summary>
        /// 获取或设置前景色
        /// </summary>
        public virtual long TextColor {
            get { return m_textColor; }
            set {
                if (m_textColor != value) {
                    m_textColor = value;
                    onTextColorChanged();
                }
            }
        }

        /// <summary>
        /// 获取或设置距离上侧的位置
        /// </summary>
        public virtual int Top {
            get {
                if (m_percentLocation != null && m_percentLocation.y != -1) {
                    FCSize parentSize = (m_parent != null ? m_parent.Size : m_native.DisplaySize);
                    return (int)(parentSize.cy * m_percentLocation.y);
                }
                else {
                    return m_location.y;
                }
            }
            set {
                if (m_percentLocation != null && m_percentLocation.y != -1) {
                    return;
                }
                else {
                    Location = new FCPoint(m_location.x, value);
                }
            }
        }

        protected bool m_topMost = false;

        /// <summary>
        /// 获取或设置是否置顶显示
        /// </summary>
        public virtual bool TopMost {
            get { return m_topMost; }
            set { m_topMost = value; }
        }

        /// <summary>
        /// 获取触摸的实际位置
        /// </summary>
        public virtual FCPoint TouchPoint {
            get {
                FCPoint mp = Native.TouchPoint;
                return pointToControl(mp);
            }
        }

        protected bool m_useRegion = false;

        /// <summary>
        /// 获取或设置是否设置裁剪
        /// </summary>
        public virtual bool UseRegion {
            get { return m_useRegion; }
        }

        protected FCVerticalAlign m_verticalAlign = FCVerticalAlign.Top;

        /// <summary>
        /// 获取或设置纵向排列方式
        /// </summary>
        public virtual FCVerticalAlign VerticalAlign {
            get { return m_verticalAlign; }
            set { m_verticalAlign = value; }
        }

        protected bool m_visible = true;

        /// <summary>
        /// 获取或设置控件是否可见
        /// </summary>
        public virtual bool Visible {
            get { return m_visible; }
            set {
                if (m_visible != value) {
                    m_visible = value;
                    onVisibleChanged();
                }
            }
        }

        /// <summary>
        /// 获取或设置控件的宽度
        /// </summary>
        public virtual int Width {
            get {
                if (m_percentSize != null && m_percentSize.cx != -1) {
                    FCSize parentSize = (m_parent != null ? m_parent.Size : m_native.DisplaySize);
                    return (int)(parentSize.cx * m_percentSize.cx);
                }
                else {
                    return m_size.cx;
                }
            }
            set {
                if (m_percentSize != null && m_percentSize.cx != -1) {
                    return;
                }
                else {
                    Size = new FCSize(value, m_size.cy);
                }
            }
        }

        /// <summary>
        /// 添加控件
        /// </summary>
        /// <param name="control">控件</param>
        public virtual void addControl(FCView control) {
            control.Parent = this;
            control.Native = m_native;
            m_controls.add(control);
            control.onAdd();
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="func">函数指针</param>
        /// <param name="eventID">事件ID</param>
        public virtual void addEvent(object func, int eventID) {
            if (m_events == null) {
                m_events = new HashMap<int, ArrayList<object>>();
            }
            ArrayList<object> eventList = null;
            if (m_events.containsKey(eventID)) {
                eventList = m_events.get(eventID);
            }
            else {
                eventList = new ArrayList<object>();
                m_events.put(eventID, eventList);
            }
            eventList.add(func);
        }

        /// <summary>
        /// 在控件线程中调用方法
        /// </summary>
        /// <param name="args">参数</param>
        public virtual void beginInvoke(object args) {
            if (m_native != null) {
                FCHost host = m_native.Host;
                host.beginInvoke(this, args);
            }
        }

        /// <summary>
        /// 将子控件置于最前
        /// </summary>
        /// <param name="childControl">子控件</param>
        public virtual void bringChildToFront(FCView childControl) {
            if (m_controls != null && m_controls.size() > 0) {
                m_controls.remove(childControl);
                m_controls.add(childControl);
            }
        }

        /// <summary>
        /// 将控件放到最前显示
        /// </summary>
        public virtual void bringToFront() {
            if (m_native != null) {
                m_native.bringToFront(this);
            }
        }

        /// <summary>
        /// 调用事件
        /// </summary>
        /// <param name="eventID">事件ID</param>
        protected void callEvents(int eventID) {
            if (m_canRaiseEvents) {
                if (m_events != null && m_events.containsKey(eventID)) {
                    ArrayList<object> events = m_events.get(eventID);
                    int eventSize = events.size();
                    for (int i = 0; i < eventSize; i++) {
                        FCEvent func = events.get(i) as FCEvent;
                        if (func != null) {
                            func(this);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 调用控件线程方法事件
        /// </summary>
        /// <param name="eventID">事件ID</param>
        /// <param name="args">参数</param>
        protected void callInvokeEvents(int eventID, object args) {
            if (m_canRaiseEvents) {
                if (m_events != null && m_events.containsKey(eventID)) {
                    ArrayList<object> events = m_events.get(eventID);
                    int eventSize = events.size();
                    for (int i = 0; i < eventSize; i++) {
                        FCInvokeEvent func = events.get(i) as FCInvokeEvent;
                        if (func != null) {
                            func(this, args);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 调用键盘事件
        /// </summary>
        /// <param name="eventID">事件ID</param>
        /// <param name="key">按键</param>
        protected void callKeyEvents(int eventID, char key) {
            if (m_canRaiseEvents) {
                if (m_events != null && m_events.containsKey(eventID)) {
                    ArrayList<object> events = m_events.get(eventID);
                    int eventSize = events.size();
                    for (int i = 0; i < eventSize; i++) {
                        FCKeyEvent func = events.get(i) as FCKeyEvent;
                        if (func != null) {
                            func(this, key);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 调用重绘事件
        /// </summary>
        /// <param name="eventID">事件ID</param>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        protected void callPaintEvents(int eventID, FCPaint paint, FCRect clipRect) {
            if (m_canRaiseEvents) {
                if (m_events != null && m_events.containsKey(eventID)) {
                    ArrayList<object> events = m_events.get(eventID);
                    int eventSize = events.size();
                    for (int i = 0; i < eventSize; i++) {
                        FCPaintEvent func = events.get(i) as FCPaintEvent;
                        if (func != null) {
                            func(this, paint, clipRect);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 预处理键盘事件
        /// </summary>
        /// <param name="eventID">事件ID</param>
        /// <param name="tEventID">事件ID2</param>
        /// <param name="key">键</param>
        /// <returns>是否不处理</returns>
        protected bool callPreviewsKeyEvent(int eventID, int tEventID, char key) {
            if (m_canRaiseEvents) {
                if (m_events != null && m_events.containsKey(eventID)) {
                    ArrayList<object> events = m_events.get(eventID);
                    int eventSize = events.size();
                    for (int i = 0; i < eventSize; i++) {
                        FCPreviewKeyEvent func = events.get(i) as FCPreviewKeyEvent;
                        if (func != null) {
                            if (func(this, tEventID, key)) {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 调用触摸事件
        /// </summary>
        /// <param name="eventID">事件ID</param>
        /// <param name="tEventID">事件ID2</param>
        /// <param name="touchInfo">触摸信息</param>
        protected bool callPreviewsTouchEvents(int eventID, int tEventID, FCTouchInfo touchInfo) {
            if (m_canRaiseEvents) {
                if (m_events != null && m_events.containsKey(eventID)) {
                    ArrayList<object> events = m_events.get(eventID);
                    int eventSize = events.size();
                    for (int i = 0; i < eventSize; i++) {
                        FCPreviewTouchEvent func = events.get(i) as FCPreviewTouchEvent;
                        if (func != null) {
                            if (func(this, tEventID, touchInfo)) {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 调用秒表事件
        /// </summary>
        /// <param name="eventID">事件ID</param>
        /// <param name="timerID">秒表编号</param>
        protected void callTimerEvents(int eventID, int timerID) {
            if (m_canRaiseEvents) {
                if (m_events != null && m_events.containsKey(eventID)) {
                    ArrayList<object> events = m_events.get(eventID);
                    int eventSize = events.size();
                    for (int i = 0; i < eventSize; i++) {
                        FCTimerEvent func = events.get(i) as FCTimerEvent;
                        if (func != null) {
                            func(this, timerID);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 调用触摸事件
        /// </summary>
        /// <param name="eventID">事件ID</param>
        /// <param name="touchInfo">触摸信息</param>
        protected void callTouchEvents(int eventID, FCTouchInfo touchInfo) {
            if (m_canRaiseEvents) {
                if (m_events != null && m_events.containsKey(eventID)) {
                    ArrayList<object> events = m_events.get(eventID);
                    int eventSize = events.size();
                    for (int i = 0; i < eventSize; i++) {
                        FCTouchEvent func = events.get(i) as FCTouchEvent;
                        if (func != null) {
                            func(this, touchInfo);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 清除所有控件
        /// </summary>
        public virtual void clearControls() {
            ArrayList<FCView> controls = new ArrayList<FCView>();
            foreach (FCView control in m_controls) {
                controls.add(control);
            }
            foreach (FCView control in controls) {
                control.onRemove();
                control.delete();
            }
            m_controls.clear();
        }

        /// <summary>
        /// 是否包含控件
        /// </summary>
        /// <param name="control">控件</param>
        /// <returns>是否包含</returns>
        public virtual bool containsControl(FCView control) {
            foreach (FCView subControl in m_controls) {
                if (subControl == control) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否包含点
        /// </summary>
        /// <param name="point">坐标</param>
        /// <returns>是否包含</returns>
        public virtual bool containsPoint(FCPoint point) {
            FCPoint cPoint = pointToControl(point);
            FCSize size = Size;
            if (cPoint.x >= 0 && cPoint.x <= size.cx && cPoint.y >= 0 && cPoint.y <= size.cy) {
                if (m_useRegion) {
                    if (cPoint.x >= m_region.left && cPoint.x <= m_region.right
                        && cPoint.y >= m_region.top && cPoint.y <= m_region.bottom) {
                        return true;
                    }
                }
                else {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 销毁资源
        /// </summary>
        public virtual void delete() {
            if (!m_isDeleted) {
                if (m_events != null) {
                    m_events.clear();
                }
                clearControls();
                m_isDeleted = true;
            }
        }

        /// <summary>
        /// 设置焦点
        /// </summary>
        public virtual void focus() {
            Focused = true;
        }

        /// <summary>
        /// 获取控件集合的拷贝
        /// </summary>
        /// <returns>控件集合</returns>
        public virtual ArrayList<FCView> getControls() {
            return m_controls;
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <returns>控件类型</returns>
        public virtual String getControlType() {
            return "BaseControl";
        }

        /// <summary>
        /// 获取显示偏移坐标
        /// </summary>
        /// <returns>坐标</returns>
        public virtual FCPoint getDisplayOffset() {
            return new FCPoint(0, 0);
        }

        /// <summary>
        /// 获取事件名称列表
        /// </summary>
        /// <returns>名称列表</returns>
        public virtual ArrayList<String> getEventNames() {
            ArrayList<String> eventNames = new ArrayList<String>();
            eventNames.AddRange(new String[] { "Add", "BackColorChanged", "BackImageChanged", "Char", "Click", "copy", "Cut", "DockChanged", "DoubleClick", "DragBegin", "DragEnd", "Dragging", "EnableChanged", "FontChanged", "GotFocus", "invoke", "KeyDown", 
            "KeyUp", "Load", "LocationChanged", "LostFocus", "MarginChanged", "TouchDown", "TouchEnter", "TouchLeave", "TouchMove", "TouchUp", "TouchWheel", "PaddingChanged", "ParentChanged", "Paint",
            "PaintBorder", "paste", "RegionChanged", "Remove", "SizeChanged", "TabIndexChanged", "TabStop", "TabStopChanged", "TextChanged", "TextColorChanged", "Timer", "VisibleChanged"});
            return eventNames;
        }

        public static int m_newTimerID = 0;

        /// <summary>
        /// 获取新的秒表编号
        /// </summary>
        /// <returns>新编号</returns>
        public static int getNewTimerID() {
            return m_newTimerID++;
        }

        /// <summary>
        /// 获取或设置的背景色
        /// </summary>
        /// <returns>背景色</returns>
        protected virtual long getPaintingBackColor() {
            if (m_backColor != FCColor.None && FCColor.DisabledBack != FCColor.None) {
                if (!isPaintEnabled(this)) {
                    return FCColor.DisabledBack;
                }
            }
            return m_backColor;
        }

        /// <summary>
        /// 获取要绘制的背景图片
        /// </summary>
        /// <returns>背景图片</returns>
        protected virtual String getPaintingBackImage() {
            return m_backImage;
        }

        /// <summary>
        /// 获取要绘制的边线颜色
        /// </summary>
        /// <returns>边线颜色</returns>
        protected virtual long getPaintingBorderColor() {
            return m_borderColor;
        }

        /// <summary>
        /// 获取要绘制的前景色
        /// </summary>
        /// <returns>前景色</returns>
        protected virtual long getPaintingTextColor() {
            if (m_textColor != FCColor.Text && FCColor.DisabledText != FCColor.None) {
                if (!isPaintEnabled(this)) {
                    return FCColor.DisabledText;
                }
            }
            return m_textColor;
        }

        /// <summary>
        /// 获取弹出菜单上下文
        /// </summary>
        /// <param name="control">当前控件</param>
        /// <returns>控件</returns>
        public virtual FCView getPopupMenuContext(FCView control) {
            if (m_hasPopupMenu) {
                return this;
            }
            else {
                if (m_parent != null) {
                    return getPopupMenuContext(m_parent);
                }
                else {
                    return null;
                }
            }
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">返回属性值</param>
        /// <param name="type">返回属性类型</param>
        public virtual void getProperty(String name, ref String value, ref String type) {
            int len = name.Length;
            switch (len) {
                case 2: {
                        if (name == "id") {
                            type = "text";
                            value = Name;
                        }
                        break;
                    }
                case 3: {
                        if (name == "top") {
                            type = "float";
                            if (m_percentLocation != null && m_percentLocation.y != -1) {
                                value = "%" + FCStr.convertFloatToStr(100 * m_percentLocation.y);
                            }
                            else {
                                value = FCStr.convertIntToStr(Top);
                            }
                        }
                        break;
                    }
                case 4: {
                        if (name == "dock") {
                            type = "enum:FCDockStyle";
                            value = FCStr.convertDockToStr(Dock);
                        }
                        else if (name == "font") {
                            type = "font";
                            value = FCStr.convertFontToStr(Font);
                        }
                        else if (name == "left") {
                            type = "float";
                            if (m_percentLocation != null && m_percentLocation.x != -1) {
                                value = "%" + FCStr.convertFloatToStr(100 * m_percentLocation.x);
                            }
                            else {
                                value = FCStr.convertIntToStr(Left);
                            }
                        }
                        else if (name == "name") {
                            type = "text";
                            value = Name;
                        }
                        else if (name == "size") {
                            type = "size";
                            if (m_percentSize != null) {
                                String pWidth = "", pHeight = "", pType = "";
                                getProperty("width", ref pWidth, ref pType);
                                getProperty("height", ref pHeight, ref pType);
                                value = pWidth + "," + pHeight;
                            }
                            else {
                                value = FCStr.convertSizeToStr(Size);
                            }
                        }
                        else if (name == "text") {
                            type = "text";
                            value = Text;
                        }
                        break;
                    }
                case 5: {
                        if (name == "align") {
                            type = "enum:FCHorizontalAlign";
                            value = FCStr.convertHorizontalAlignToStr(Align);
                        }
                        else if (name == "value") {
                            type = "text";
                            value = Text;
                        }
                        else if (name == "width") {
                            type = "float";
                            if (m_percentSize != null && m_percentSize.cx != -1) {
                                value = "%" + FCStr.convertFloatToStr(100 * m_percentSize.cx);
                            }
                            else {
                                value = FCStr.convertIntToStr(Width);
                            }
                        }
                        break;
                    }
                case 6: {
                        if (name == "anchor") {
                            type = "anchor";
                            value = FCStr.convertAnchorToStr(Anchor);
                        }
                        else if (name == "bounds") {
                            type = "rect";
                            value = FCStr.convertRectToStr(Bounds);
                        }
                        else if (name == "cursor") {
                            type = "enum:FCCursors";
                            value = FCStr.convertCursorToStr(Cursor);
                        }
                        else if (name == "height") {
                            type = "float";
                            if (m_percentSize != null && m_percentSize.cy != -1) {
                                value = "%" + FCStr.convertFloatToStr(100 * m_percentSize.cy);
                            }
                            else {
                                value = FCStr.convertIntToStr(Height);
                            }
                        }
                        else if (name == "margin") {
                            type = "margin";
                            value = FCStr.convertPaddingToStr(Margin);
                        }
                        else if (name == "region") {
                            type = "rect";
                            value = FCStr.convertRectToStr(Region);
                        }
                        break;
                    }
                case 7: {
                        if (name == "enabled") {
                            type = "bool";
                            value = FCStr.convertBoolToStr(Enabled);
                        }
                        else if (name == "focused") {
                            type = "bool";
                            value = FCStr.convertBoolToStr(Focused);
                        }
                        else if (name == "opacity") {
                            type = "float";
                            value = FCStr.convertFloatToStr(Opacity);
                        }
                        else if (name == "padding") {
                            type = "padding";
                            value = FCStr.convertPaddingToStr(Padding);
                        }
                        else if (name == "tabstop") {
                            type = "bool";
                            value = FCStr.convertBoolToStr(TabStop);
                        }
                        else if (name == "topmost") {
                            type = "bool";
                            value = FCStr.convertBoolToStr(TopMost);
                        }
                        else if (name == "visible") {
                            type = "bool";
                            value = FCStr.convertBoolToStr(Visible);
                        }
                        break;
                    }
                case 8: {
                        if (name == "autosize") {
                            type = "bool";
                            value = FCStr.convertBoolToStr(AutoSize);
                        }
                        else if (name == "canfocus") {
                            type = "bool";
                            value = FCStr.convertBoolToStr(CanFocus);
                        }
                        else if (name == "iswindow") {
                            type = "bool";
                            value = FCStr.convertBoolToStr(IsWindow);
                        }
                        else if (name == "location") {
                            type = "point";
                            if (m_percentLocation != null) {
                                String pLeft = "", pTop = "", pType = "";
                                getProperty("left", ref pLeft, ref pType);
                                getProperty("top", ref pTop, ref pType);
                                value = pLeft + "," + pTop;
                            }
                            else {
                                value = FCStr.convertPointToStr(Location);
                            }
                        }
                        else if (name == "tabindex") {
                            type = "int";
                            value = FCStr.convertIntToStr(TabIndex);
                        }
                        break;
                    }
                case 9: {
                        if (name == "allowdrag") {
                            type = "bool";
                            value = FCStr.convertBoolToStr(AllowDrag);
                        }
                        else if (name == "backcolor") {
                            type = "color";
                            value = FCStr.convertColorToStr(BackColor);
                        }
                        else if (name == "backimage") {
                            type = "text";
                            value = BackImage;
                        }
                        else if (name == "textcolor") {
                            type = "color";
                            value = FCStr.convertColorToStr(TextColor);
                        }
                        break;
                    }
                default: {
                        if (name == "allowpreviewsevent") {
                            type = "bool";
                            value = FCStr.convertBoolToStr(AllowPreviewsEvent);
                        }
                        else if (name == "autoellipsis") {
                            type = "bool";
                            value = FCStr.convertBoolToStr(AutoEllipsis);
                        }
                        else if (name == "bordercolor") {
                            type = "color";
                            value = FCStr.convertColorToStr(BorderColor);
                        }

                        else if (name == "canraiseevents") {
                            type = "bool";
                            value = FCStr.convertBoolToStr(CanRaiseEvents);
                        }
                        else if (name == "cornerradius") {
                            type = "int";
                            value = FCStr.convertIntToStr(CornerRadius);
                        }
                        else if (name == "displayoffset") {
                            type = "bool";
                            value = FCStr.convertBoolToStr(DisplayOffset);
                        }
                        else if (name == "haspopupmenu") {
                            type = "bool";
                            value = FCStr.convertBoolToStr(HasPopupMenu);
                        }
                        else if (name == "maximumsize") {
                            type = "size";
                            value = FCStr.convertSizeToStr(MaximumSize);
                        }
                        else if (name == "minimumsize") {
                            type = "size";
                            value = FCStr.convertSizeToStr(MinimumSize);
                        }
                        else if (name == "resourcepath") {
                            type = "text";
                            value = ResourcePath;
                        }
                        else if (name == "vertical-align") {
                            type = "enum:FCVerticalAlign";
                            value = FCStr.convertVerticalAlignToStr(VerticalAlign);
                        }
                        break;
                    }
            }
        }

        /// <summary>
        /// 获取属性名称列表
        /// </summary>
        /// <returns></returns>
        public virtual ArrayList<String> getPropertyNames() {
            ArrayList<String> propertyNames = new ArrayList<String>();
            propertyNames.AddRange(new String[] { "Align", "AllowDrag", "AllowPreviewsEvent", "Anchor", "AutoEllipsis", "AutoSize", "BackColor", "BackImage", "BorderColor", "Bounds", "CanFocus", "CanRaiseEvents", 
            "CornerRadius", "Cursor", "DisplayOffset", "Dock", "Enabled", "Focused", "Font", "HasPopupMenu", "Height", "IsWindow", "Left",
            "Location", "Margin", "MaximumSize", "MinimumSize", "Name", "Opacity", "Padding", "Region", "ResourcePath", "Size", "TabIndex", "TabStop", "Text", "TextColor", 
            "Top", "TopMost", "Value", "Vertical-Align", "Visible", "Width"});
            return propertyNames;
        }

        /// <summary>
        /// 判断是否包含子控件
        /// </summary>
        /// <returns>是否包含子控件</returns>
        public bool hasChildren() {
            return m_controls.size() > 0;
        }

        /// <summary>
        /// 隐藏控件
        /// </summary>
        public virtual void hide() {
            Visible = false;
        }

        /// <summary>
        /// 插入控件
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="control">控件</param>
        public virtual void insertControl(int index, FCView control) {
            m_controls.Insert(index, control);
        }

        /// <summary>
        /// 启动绘制
        /// </summary>
        public virtual void invalidate() {
            if (m_native != null) {
                m_native.invalidate(this);
            }
        }

        /// <summary>
        /// 在控件线程中调用方法
        /// </summary>
        /// <param name="args">参数</param>
        public virtual void invoke(object args) {
            if (m_native != null) {
                FCHost host = m_native.Host;
                host.invoke(this, args);
            }
        }

        /// <summary>
        /// 判断是否绘制可用状态
        /// </summary>
        /// <param name="control">控件</param>
        /// <returns>是否绘制可用状态</returns>
        public bool isPaintEnabled(FCView control) {
            if (control.Enabled) {
                FCView parent = control.Parent;
                if (parent != null) {
                    return isPaintEnabled(parent);
                }
                else {
                    return true;
                }
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// 判断是否绘图时可见
        /// </summary>
        /// <param name="control">控件</param>
        /// <returns>是否可见</returns>
        public bool isPaintVisible(FCView control) {
            if (control.Visible) {
                FCView parent = control.Parent;
                if (parent != null) {
                    return isPaintVisible(parent);
                }
                else {
                    return true;
                }
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// 添加控件方法
        /// </summary>
        public virtual void onAdd() {
            callEvents(FCEventID.ADD);
        }

        /// <summary>
        /// 自动设置尺寸属性改变方法
        /// </summary>
        public virtual void onAutoSizeChanged() {
            callEvents(FCEventID.AUTOSIZECHANGED);
        }

        /// <summary>
        /// 背景色改变方法
        /// </summary>
        public virtual void onBackColorChanged() {
            callEvents(FCEventID.BACKCOLORCHANGED);
        }

        /// <summary>
        /// 背景图片改变方法
        /// </summary>
        public virtual void onBackImageChanged() {
            callEvents(FCEventID.BACKIMAGECHANGED);
        }

        /// <summary>
        /// 字符输入
        /// </summary>
        /// <param name="ch">字符</param>
        public virtual void onChar(char ch) {
            callKeyEvents(FCEventID.CHAR, ch);
        }

        /// <summary>
        /// 触摸点击方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public virtual void onClick(FCTouchInfo touchInfo) {
            callTouchEvents(FCEventID.CLICK, touchInfo);
        }

        /// <summary>
        /// 复制
        /// </summary>
        public virtual void onCopy() {
            callEvents(FCEventID.COPY);
        }

        /// <summary>
        /// 剪切
        /// </summary>
        public virtual void onCut() {
            callEvents(FCEventID.CUT);
        }

        /// <summary>
        /// 悬停改变方法
        /// </summary>
        public virtual void onDockChanged() {
            callEvents(FCEventID.DOCKCHANGED);
        }

        /// <summary>
        /// 触摸双击方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public virtual void onDoubleClick(FCTouchInfo touchInfo) {
            callTouchEvents(FCEventID.DOUBLECLICK, touchInfo);
        }

        /// <summary>
        /// 拖动开始方法
        /// </summary>
        /// <returns>是否拖动</returns>
        public virtual bool onDragBegin() {
            callEvents(FCEventID.DRAGBEGIN);
            return true;
        }

        /// <summary>
        /// 拖动结束方法
        /// </summary>
        public virtual void onDragEnd() {
            m_isDragging = false;
            callEvents(FCEventID.DRAGEND);
        }

        /// <summary>
        /// 正在拖动方法
        /// </summary>
        public virtual void onDragging() {
            m_isDragging = true;
            callEvents(FCEventID.DRAGGING);
        }

        /// <summary>
        /// 拖动准备方法
        /// </summary>
        /// <param name="startOffset">可以拖动的偏移坐标量</param>
        public virtual void onDragReady(ref FCPoint startOffset) {
            startOffset.x = 5;
            startOffset.y = 5;
        }

        /// <summary>
        /// 可用改变方法
        /// </summary>
        public virtual void onEnableChanged() {
            callEvents(FCEventID.ENABLECHANGED);
        }

        /// <summary>
        /// 字体改变方法
        /// </summary>
        public virtual void onFontChanged() {
            callEvents(FCEventID.FONTCHANGED);
        }

        /// <summary>
        /// 获得焦点方法
        /// </summary>
        public virtual void onGotFocus() {
            callEvents(FCEventID.GOTFOCUS);
        }

        /// <summary>
        /// 在控件线程中调用方法
        /// </summary>
        /// <param name="args">参数</param>
        public virtual void onInvoke(object args) {
            callInvokeEvents(FCEventID.INVOKE, args);
        }

        /// <summary>
        /// 控件加载方法
        /// </summary>
        public virtual void onLoad() {
            callEvents(FCEventID.LOAD);
        }

        /// <summary>
        /// 位置改变方法
        /// </summary>
        public virtual void onLocationChanged() {
            callEvents(FCEventID.LOCATIONCHANGED);
        }

        /// <summary>
        /// 丢失焦点方法
        /// </summary>
        public virtual void onLostFocus() {
            callEvents(FCEventID.LOSTFOCUS);
        }

        /// <summary>
        /// 键盘按下方法
        /// </summary>
        /// <param name="key">按键</param>
        public virtual void onKeyDown(char key) {
            callKeyEvents(FCEventID.KEYDOWN, key);
            FCHost host = Native.Host;
            if (host.isKeyPress(0x11)) {
                //复制
                if (key == 67) {
                    onCopy();
                }
                //粘贴
                else if (key == 86) {
                    onPaste();
                }
                //剪切
                else if (key == 88) {
                    onCut();
                }
            }
        }

        /// <summary>
        /// 键盘弹起方法
        /// </summary>
        /// <param name="key">按键</param>
        public virtual void onKeyUp(char key) {
            callKeyEvents(FCEventID.KEYUP, key);
        }

        /// <summary>
        /// 外边距改变方法
        /// </summary>
        public virtual void onMarginChanged() {
            callEvents(FCEventID.MARGINCHANGED);
        }

        /// <summary>
        /// 内边距改变方法
        /// </summary>
        public virtual void onPaddingChanged() {
            callEvents(FCEventID.PADDINGCHANGED);
        }

        /// <summary>
        /// 重绘方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public virtual void onPaint(FCPaint paint, FCRect clipRect) {
            onPaintBackground(paint, clipRect);
            onPaintForeground(paint, clipRect);
            callPaintEvents(FCEventID.PAINT, paint, clipRect);
        }

        /// <summary>
        /// 重绘背景方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public virtual void onPaintBackground(FCPaint paint, FCRect clipRect) {
            FCRect rect = new FCRect(0, 0, Width, Height);
            //绘制背景色
            paint.fillRoundRect(getPaintingBackColor(), rect, m_cornerRadius);
            //绘制背景图
            String bkImage = getPaintingBackImage();
            if (bkImage != null && bkImage.Length > 0) {
                paint.drawImage(bkImage, rect);
            }
        }

        /// <summary>
        /// 重绘边线方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public virtual void onPaintBorder(FCPaint paint, FCRect clipRect) {
            FCRect borderRect = new FCRect(0, 0, Width, Height);
            //绘制边线
            paint.drawRoundRect(getPaintingBorderColor(), 1, 0, borderRect, m_cornerRadius);
            callPaintEvents(FCEventID.PAINTBORDER, paint, clipRect);
        }

        /// <summary>
        /// 重绘前景方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public virtual void onPaintForeground(FCPaint paint, FCRect clipRect) {
        }

        /// <summary>
        /// 父容器改变方法
        /// </summary>
        public virtual void onParentChanged() {
            callEvents(FCEventID.PARENTCHANGED);
        }

        /// <summary>
        /// 复制
        /// </summary>
        public virtual void onPaste() {
            callEvents(FCEventID.PASTE);
        }

        /// <summary>
        /// 预绘图方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public virtual void onPrePaint(FCPaint paint, FCRect clipRect) {

        }

        /// <summary>
        /// 键盘下按方法
        /// </summary>
        /// <param name="eventID">事件ID</param>
        /// <param name="key">按键</param>
        /// <returns>状态</returns>
        public virtual bool onPreviewsKeyEvent(int eventID, char key) {
            return callPreviewsKeyEvent(FCEventID.PREVIEWSKEYEVENT, eventID, key);
        }

        /// <summary>
        /// 预处理触摸事件
        /// </summary>
        /// <param name="eventID">事件ID</param>
        /// <param name="touchInfo">触摸信息</param>
        /// <returns>状态</returns>
        public virtual bool onPreviewsTouchEvent(int eventID, FCTouchInfo touchInfo) {
            return callPreviewsTouchEvents(FCEventID.PREVIEWSTOUCHEVENT, eventID, touchInfo);
        }

        /// <summary>
        /// 移除控件方法
        /// </summary>
        public virtual void onRemove() {
            callEvents(FCEventID.REMOVE);
        }

        /// <summary>
        /// 裁剪区域改变方法
        /// </summary>
        public virtual void onRegionChanged() {
            callEvents(FCEventID.REGIONCHANGED);
        }

        /// <summary>
        /// 尺寸改变方法
        /// </summary>
        public virtual void onSizeChanged() {
            callEvents(FCEventID.SIZECHANGED);
        }

        /// <summary>
        /// TAB索引改变方法
        /// </summary>
        public virtual void onTabIndexChanged() {
            callEvents(FCEventID.TABINDEXCHANGED);
        }

        /// <summary>
        /// 被使用TAB切换方法
        /// </summary>
        public virtual void onTabStop() {
            callEvents(FCEventID.TABSTOP);
        }

        /// <summary>
        /// 是否用TAB切换改变方法
        /// </summary>
        public virtual void onTabStopChanged() {
            callEvents(FCEventID.TABSTOPCHANGED);
        }

        /// <summary>
        /// 文本改变方法
        /// </summary>
        public virtual void onTextChanged() {
            callEvents(FCEventID.TEXTCHANGED);
        }

        /// <summary>
        /// 前景色改变方法
        /// </summary>
        public virtual void onTextColorChanged() {
            callEvents(FCEventID.TEXTCOLORCHANGED);
        }

        /// <summary>
        /// 触摸按下方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public virtual void onTouchDown(FCTouchInfo touchInfo) {
            callTouchEvents(FCEventID.TOUCHDOWN, touchInfo);
        }

        /// <summary>
        /// 触摸进入方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public virtual void onTouchEnter(FCTouchInfo touchInfo) {
            callTouchEvents(FCEventID.TOUCHENTER, touchInfo);
            if (m_autoEllipsis) {
                if (m_text != null && m_text.Length > 0) {
                    m_native.Host.showToolTip(m_text, m_native.TouchPoint);
                }
            }
        }

        /// <summary>
        /// 触摸离开方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public virtual void onTouchLeave(FCTouchInfo touchInfo) {
            callTouchEvents(FCEventID.TOUCHLEAVE, touchInfo);
        }

        /// <summary>
        /// 触摸移动调用方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public virtual void onTouchMove(FCTouchInfo touchInfo) {
            callTouchEvents(FCEventID.TOUCHMOVE, touchInfo);
        }

        /// <summary>
        /// 触摸抬起方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public virtual void onTouchUp(FCTouchInfo touchInfo) {
            callTouchEvents(FCEventID.TOUCHUP, touchInfo);
        }

        /// <summary>
        /// 触摸滚动方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public virtual void onTouchWheel(FCTouchInfo touchInfo) {
            callTouchEvents(FCEventID.TOUCHWHEEL, touchInfo);
        }

        /// <summary>
        /// 秒表回调方法
        /// </summary>
        /// <param name="timerID">编号</param>
        public virtual void onTimer(int timerID) {
            callTimerEvents(FCEventID.TIMER, timerID);
        }

        /// <summary>
        /// 可见状态改变方法
        /// </summary>
        public virtual void onVisibleChanged() {
            callEvents(FCEventID.VISIBLECHANGED);
        }

        /// <summary>
        /// 获取相对于控件的相对坐标
        /// </summary>
        /// <param name="point">坐标</param>
        /// <returns>相对坐标</returns>
        public FCPoint pointToControl(FCPoint point) {
            if (m_native != null) {
                int clientX = m_native.clientX(this);
                int clientY = m_native.clientY(this);
                return new FCPoint(point.x - clientX, point.y - clientY);
            }
            else {
                return point;
            }
        }

        /// <summary>
        /// 获取控件的绝对坐标
        /// </summary>
        /// <param name="point">坐标</param>
        /// <returns>绝对坐标</returns>
        public FCPoint pointToNative(FCPoint point) {
            if (m_native != null) {
                int clientX = m_native.clientX(this);
                int clientY = m_native.clientY(this);
                return new FCPoint(point.x + clientX, point.y + clientY);
            }
            else {
                return point;
            }
        }

        /// <summary>
        /// 移除控件
        /// </summary>
        /// <param name="control">控件</param>
        public virtual void removeControl(FCView control) {
            if (m_native != null) {
                m_native.removeControl(control);
            }
            m_controls.remove(control);
            control.onRemove();
            control.Parent = null;
        }

        /// <summary>
        /// 取消注册事件
        /// </summary>
        /// <param name="func">函数指针</param>
        /// <param name="eventID">事件ID</param>
        public virtual void removeEvent(object func, int eventID) {
            if (m_events != null && m_events.containsKey(eventID)) {
                m_events.get(eventID).remove(func);
            }
        }

        /// <summary>
        /// 将控件置于最后
        /// </summary>
        /// <param name="childControl">子控件</param>
        public virtual void sendChildToBack(FCView childControl) {
            if (m_controls != null && m_controls.size() > 0) {
                m_controls.remove(childControl);
                m_controls.Insert(0, childControl);
            }
        }


        /// <summary>
        /// 将控件放到最下面显示
        /// </summary>
        public virtual void sendToBack() {
            if (m_native != null) {
                m_native.sendToBack(this);
            }
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        public virtual void setProperty(String name, String value) {
            int len = name.Length;
            switch (len) {
                case 2: {
                        if (name == "id") {
                            Name = value;
                        }
                        break;
                    }
                case 3: {
                        if (name == "top") {
                            if (value.IndexOf("%") != -1) {
                                float percentValue = FCStr.convertStrToFloat(value.Replace("%", "")) / 100;
                                if (m_percentLocation == null) {
                                    m_percentLocation = new FCPointF2();
                                    m_percentLocation.x = -1;
                                }
                                m_percentLocation.y = percentValue;
                            }
                            else {
                                if (m_percentLocation != null) {
                                    m_percentLocation.y = -1;
                                }
                                Top = FCStr.convertStrToInt(value);
                            }

                        }
                        break;
                    }
                case 4: {
                        if (name == "dock") {
                            Dock = FCStr.convertStrToDock(value);
                        }
                        else if (name == "font") {
                            Font = FCStr.convertStrToFont(value);
                        }
                        else if (name == "left") {
                            if (value.IndexOf("%") != -1) {
                                float percentValue = FCStr.convertStrToFloat(value.Replace("%", "")) / 100;
                                if (m_percentLocation == null) {
                                    m_percentLocation = new FCPointF2();
                                    m_percentLocation.y = -1;
                                }
                                m_percentLocation.x = percentValue;
                            }
                            else {
                                if (m_percentLocation != null) {
                                    m_percentLocation.x = -1;
                                }
                                Left = FCStr.convertStrToInt(value);
                            }
                        }
                        else if (name == "name") {
                            Name = value;
                        }
                        else if (name == "size") {
                            if (value.IndexOf("%") == -1) {
                                Size = FCStr.convertStrToSize(value);
                            }
                            else {
                                String[] strs = value.Split(',');
                                setProperty("width", strs[0]);
                                setProperty("height", strs[1]);
                            }
                        }
                        else if (name == "text") {
                            Text = value;
                        }
                        break;
                    }
                case 5: {
                        if (name == "align") {
                            Align = FCStr.convertStrToHorizontalAlign(value);
                        }
                        else if (name == "value") {
                            Text = value;
                        }
                        else if (name == "width") {
                            if (value.IndexOf("%") != -1) {
                                float percentValue = FCStr.convertStrToFloat(value.Replace("%", "")) / 100;
                                if (m_percentSize == null) {
                                    m_percentSize = new FCSizeF2();
                                    m_percentSize.cy = -1;
                                }
                                m_percentSize.cx = percentValue;
                            }
                            else {
                                if (m_percentSize != null) {
                                    m_percentSize.cx = -1;
                                }
                                Width = FCStr.convertStrToInt(value);
                            }

                        }
                        break;
                    }
                case 6: {
                        if (name == "anchor") {
                            Anchor = FCStr.convertStrToAnchor(value);
                        }
                        else if (name == "bounds") {
                            Bounds = FCStr.convertStrToRect(value);
                        }
                        else if (name == "cursor") {
                            Cursor = FCStr.convertStrToCursor(value);
                        }
                        else if (name == "height") {
                            if (value.IndexOf("%") != -1) {
                                float percentValue = FCStr.convertStrToFloat(value.Replace("%", "")) / 100;
                                if (m_percentSize == null) {
                                    m_percentSize = new FCSizeF2();
                                    m_percentSize.cx = -1;
                                }
                                m_percentSize.cy = percentValue;
                            }
                            else {
                                if (m_percentSize != null) {
                                    m_percentSize.cy = -1;
                                }
                                Height = FCStr.convertStrToInt(value);
                            }
                        }
                        else if (name == "margin") {
                            Margin = FCStr.convertStrToPadding(value);
                        }
                        else if (name == "region") {
                            Region = FCStr.convertStrToRect(value);
                        }
                        break;
                    }
                case 7: {
                        if (name == "enabled") {
                            Enabled = FCStr.convertStrToBool(value);
                        }
                        else if (name == "focused") {
                            Focused = FCStr.convertStrToBool(value);
                        }
                        else if (name == "opacity") {
                            Opacity = FCStr.convertStrToFloat(value);
                        }
                        else if (name == "padding") {
                            Padding = FCStr.convertStrToPadding(value);
                        }
                        else if (name == "tabstop") {
                            TabStop = FCStr.convertStrToBool(value);
                        }
                        else if (name == "topmost") {
                            TopMost = FCStr.convertStrToBool(value);
                        }
                        else if (name == "visible") {
                            Visible = FCStr.convertStrToBool(value);
                        }
                        break;
                    }
                case 8: {
                        if (name == "autosize") {
                            AutoSize = FCStr.convertStrToBool(value);
                        }
                        else if (name == "canfocus") {
                            CanFocus = FCStr.convertStrToBool(value);
                        }
                        else if (name == "iswindow") {
                            IsWindow = FCStr.convertStrToBool(value);
                        }
                        else if (name == "location") {
                            if (value.IndexOf("%") == -1) {
                                Location = FCStr.convertStrToPoint(value);
                            }
                            else {
                                String[] strs = value.Split(',');
                                setProperty("left", strs[0]);
                                setProperty("top", strs[1]);
                            }
                        }
                        else if (name == "tabindex") {
                            TabIndex = FCStr.convertStrToInt(value);
                        }
                        break;
                    }
                case 9: {
                        if (name == "allowdrag") {
                            AllowDrag = FCStr.convertStrToBool(value);
                        }
                        else if (name == "backcolor") {
                            BackColor = FCStr.convertStrToColor(value);
                        }
                        else if (name == "backimage") {
                            BackImage = value;
                        }
                        else if (name == "textcolor") {
                            TextColor = FCStr.convertStrToColor(value);
                        }
                        break;
                    }
                default: {
                        if (name == "allowpreviewsevent") {
                            AllowPreviewsEvent = FCStr.convertStrToBool(value);
                        }
                        else if (name == "autoellipsis") {
                            AutoEllipsis = FCStr.convertStrToBool(value);
                        }
                        else if (name == "bordercolor") {
                            BorderColor = FCStr.convertStrToColor(value);
                        }
                        else if (name == "canraiseevents") {
                            CanRaiseEvents = FCStr.convertStrToBool(value);
                        }
                        else if (name == "cornerradius") {
                            CornerRadius = FCStr.convertStrToInt(value);
                        }
                        else if (name == "displayoffset") {
                            DisplayOffset = FCStr.convertStrToBool(value);
                        }
                        else if (name == "haspopupmenu") {
                            HasPopupMenu = FCStr.convertStrToBool(value);
                        }
                        else if (name == "maximumsize") {
                            MaximumSize = FCStr.convertStrToSize(value);
                        }
                        else if (name == "minimumsize") {
                            MinimumSize = FCStr.convertStrToSize(value);
                        }
                        else if (name == "resourcepath") {
                            ResourcePath = value;
                        }
                        else if (name == "vertical-align") {
                            VerticalAlign = FCStr.convertStrToVerticalAlign(value);
                        }
                        break;
                    }
            }
        }

        /// <summary>
        /// 显示控件
        /// </summary>
        public virtual void show() {
            Visible = true;
        }

        /// <summary>
        /// 开始秒表
        /// </summary>
        /// <param name="timerID">编号</param>
        /// <param name="interval">间隔</param>
        public virtual void startTimer(int timerID, int interval) {
            if (m_native != null) {
                m_native.startTimer(this, timerID, interval);
            }
        }

        /// <summary>
        /// 停止秒表
        /// </summary>
        /// <param name="timerID">编号</param>
        public virtual void stopTimer(int timerID) {
            if (m_native != null) {
                m_native.stopTimer(timerID);
            }
        }

        /// <summary>
        /// 更新界面
        /// </summary>
        public virtual void update() {
            if (m_native != null) {
                m_native.setAlign(m_controls);
                if (m_oldSize.cx > 0 && m_oldSize.cy > 0) {
                    m_native.setAnchor(m_controls, m_oldSize);
                }
                m_native.setDock(m_controls);
                m_oldLocation = Location;
                m_oldSize = Size;
                int controlsSize = m_controls.size();
                for (int i = 0; i < controlsSize; i++) {
                    m_controls.get(i).update();
                }
            }
        }
    }
}
