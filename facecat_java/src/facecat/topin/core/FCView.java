/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.core;

import java.util.*;

/**
 * 图层内置控件的基类
 */
public class FCView implements FCProperty {

    /**
     * 创建控件
     */
    public FCView() {
    }

    protected void finalize() throws Throwable {
        delete();
    }

    /**
     * 控件集合
     */
    protected ArrayList<FCView> m_controls = new ArrayList<FCView>();

    /**
     * 事件集合
     */
    protected java.util.HashMap<Integer, ArrayList<Object>> m_events;

    /**
     * 上次坐标
     */
    protected FCPoint m_oldLocation = new FCPoint();

    /**
     * 上次尺寸
     */
    protected FCSize m_oldSize = new FCSize();

    /**
     * 百分比位置
     */
    protected FCPointF m_percentLocation = null;

    /**
     * 百分比尺寸
     */
    protected FCSizeF m_percentSize = null;

    private static int m_timerID = 10000;

    protected boolean m_allowDrag = false;

    protected FCHorizontalAlign m_align = FCHorizontalAlign.Left;

    /**
     * 获取横向排列方式
     */
    public FCHorizontalAlign getAlign() {
        return m_align;
    }

    /**
     * 设置横向排列方式
     */
    public void setAlign(FCHorizontalAlign align) {
        m_align = align;
    }

    /**
     * 获取是否可以拖动位置
     */
    public boolean allowDrag() {
        return m_allowDrag;
    }

    /**
     * 设置是否可以拖动位置
     */
    public void setAllowDrag(boolean value) {
        m_allowDrag = value;
    }

    protected boolean m_allowPreviewsEvent = false;

    /**
     * 获取是否允许预处理事件
     */
    public boolean allowPreviewsEvent() {
        return m_allowPreviewsEvent;
    }

    /**
     * 设置是否允许预处理事件
     */
    public void setAllowPreviewsEvent(boolean value) {
        m_allowPreviewsEvent = value;
    }

    protected FCAnchor m_anchor = new FCAnchor(true, true, false, false);

    /**
     * 获取锚定信息
     */
    public FCAnchor getAnchor() {
        return m_anchor;
    }

    /**
     * 设置锚定信息
     */
    public void setAnchor(FCAnchor value) {
        m_anchor = value;
    }

    protected boolean m_autoEllipsis = false;

    /**
     * 获取是否在文字超出范围时在结尾显示省略号
     */
    public boolean autoEllipsis() {
        return m_autoEllipsis;
    }

    /**
     * 设置是否在文字超出范围时在结尾显示省略号
     */
    public void setAutoEllipsis(boolean value) {
        m_autoEllipsis = value;
    }

    protected boolean m_autoSize = false;

    /**
     * 获取是否自动调整尺寸
     */
    public boolean autoSize() {
        return m_autoSize;
    }

    /**
     * 设置是否自动调整尺寸
     */
    public void setAutoSize(boolean value) {
        if (m_autoSize != value) {
            m_autoSize = value;
            onAutoSizeChanged();
        }
    }

    protected long m_backColor = FCColor.Back;

    /**
     * 获取背景色
     */
    public long getBackColor() {
        return m_backColor;
    }

    /**
     * 设置背景色
     */
    public void setBackColor(long value) {
        if (m_backColor != value) {
            m_backColor = value;
            onBackColorChanged();
        }
    }

    protected String m_backImage;

    /**
     * 获取背景图片
     */
    public String getBackImage() {
        return m_backImage;
    }

    /**
     * 设置背景图片
     */
    public void setBackImage(String value) {
        m_backImage = value;
        onBackImageChanged();
    }

    protected long m_borderColor = FCColor.Border;

    /**
     * 获取边线的颜色
     */
    public long getBorderColor() {
        return m_borderColor;
    }

    /**
     * 设置边线的颜色
     */
    public void setBorderColor(long value) {
        m_borderColor = value;
    }

    /**
     * 获取距离下侧的位置
     */
    public int getBottom() {
        return getTop() + getHeight();
    }

    public FCRect getBounds() {
        return new FCRect(getLeft(), getTop(), getRight(), getBottom());
    }

    /**
     * 获取或设置控件的区域属性
     */
    public void setBounds(FCRect value) {
        setLocation(new FCPoint(value.left, value.top));
        int cx = value.right - value.left;
        int cy = value.bottom - value.top;
        setSize(new FCSize(cx, cy));
    }

    protected boolean m_canFocus = true;

    /**
     * 获取是否可以设置焦点
     */
    public boolean canFocus() {
        return m_canFocus;
    }

    /**
     * 设置是否可以设置焦点
     */
    public void setCanFocus(boolean value) {
        m_canFocus = value;
    }

    protected boolean m_canRaiseEvents = true;

    /**
     * 获取是否可以触发事件
     */
    public boolean canRaiseEvents() {
        return m_canRaiseEvents;
    }

    /**
     * 设置是否可以触发事件
     */
    public void setcanRaiseEvents(boolean value) {
        m_canRaiseEvents = value;
    }

    /**
     * 获取是否被触摸捕获
     */
    public boolean isCapture() {
        if (m_native != null) {
            if (m_native.getHoveredControl() == this || m_native.getPushedControl() == this) {
                return true;
            }
        }
        return false;
    }

    protected int m_cornerRadius;

    /**
     * 获取圆角角度
     */
    public int getCornerRadius() {
        return m_cornerRadius;
    }

    /**
     * 设置圆角角度
     */
    public void setCornerRadius(int cornerRadius) {
        m_cornerRadius = cornerRadius;
    }

    protected boolean m_displayOffset = true;

    /**
     * 获取是否允许偏移显示
     */
    public boolean displayOffset() {
        return m_displayOffset;
    }

    /**
     * 设置是否允许偏移显示
     */
    public void setDisplayOffset(boolean value) {
        m_displayOffset = value;
    }

    /**
     * 获取虚拟显示的区域
     */
    public FCRect getDisplayRect() {
        if (m_useRegion) {
            return m_region.clone();
        } else {
            return new FCRect(0, 0, getWidth(), getHeight());
        }
    }

    protected FCDockStyle m_dock = FCDockStyle.None;

    /**
     * 获取绑定边缘类型
     */
    public FCDockStyle getDock() {
        return m_dock;
    }

    /**
     * 设置绑定边缘类型
     */
    public void setDock(FCDockStyle value) {
        if (m_dock != value) {
            m_dock = value;
            onDockChanged();
        }
    }

    protected boolean m_enabled = true;

    /**
     * 获取控件是否可用
     */
    public boolean isEnabled() {
        return m_enabled;
    }

    /**
     * 设置控件是否可用
     */
    public void setEnabled(boolean value) {
        if (m_enabled != value) {
            m_enabled = value;
            onEnableChanged();
        }
    }

    /**
     * 获取是否具有焦点
     */
    public boolean isFocused() {
        if (m_native != null) {
            if (m_native.getFocusedControl() == this) {
                return true;
            }
        }
        return false;
    }

    /**
     * 设置是否具有焦点
     */
    public void setFocused(boolean value) {
        if (m_native != null) {
            if (value) {
                m_native.setFocusedControl(this);
            } else {
                if (m_native.getFocusedControl() == this) {
                    m_native.setFocusedControl(null);
                }
            }
        }
    }

    protected FCFont m_font = new FCFont();

    /**
     * 获取字体
     */
    public FCFont getFont() {
        return m_font;
    }

    /**
     * 设置字体
     */
    public void setFont(FCFont value) {
        m_font = value;
        onFontChanged();
    }

    protected boolean m_hasPopupMenu;

    /**
     * 获取是否有右键菜单
     */
    public boolean getHasPopupMenu() {
        return m_hasPopupMenu;
    }

    /**
     * 设置是否有右键菜单
     */
    public void setHasPopupMenu(boolean hasPopupMenu) {
        m_hasPopupMenu = hasPopupMenu;
    }

    /**
     * 获取控件的高度
     */
    public int getHeight() {
        if (m_percentSize != null && m_percentSize.cy != -1) {
            FCSize parentSize = (m_parent != null ? m_parent.getSize() : m_native.getDisplaySize());
            return (int) (parentSize.cy * m_percentSize.cy);
        } else {
            return m_size.cy;
        }
    }

    /**
     * 设置控件的高度
     */
    public void setHeight(int value) {
        if (m_percentSize != null && m_percentSize.cy != -1) {
            return;
        } else {
            setSize(new FCSize(m_size.cx, value));
        }
    }

    protected boolean m_isDeleted;

    /**
     * 获取或设置是否已被销毁
     */
    public boolean isDeleted() {
        return m_isDeleted;
    }

    protected boolean m_isDragging;

    /**
     * 获取或设置是否正被拖动
     */
    public boolean isDragging() {
        return m_isDragging;
    }

    protected boolean m_isWindow;

    /**
     * 获取是否为窗体
     */
    public boolean isWindow() {
        return m_isWindow;
    }

    /**
     * 设置是否为窗体
     */
    public void setIsWindow(boolean value) {
        m_isWindow = value;
    }

    /**
     * 获取距离左侧的位置
     */
    public int getLeft() {
        if (m_percentLocation != null && m_percentLocation.x != -1) {
            FCSize parentSize = (m_parent != null ? m_parent.getSize() : m_native.getDisplaySize());
            return (int) (parentSize.cx * m_percentLocation.x);
        } else {
            return m_location.x;
        }
    }

    /**
     * 设置距离左侧的位置
     */
    public void setLeft(int value) {
        if (m_percentLocation != null && m_percentLocation.x != -1) {
            return;
        } else {
            setLocation(new FCPoint(value, m_location.y));
        }
    }

    protected FCPoint m_location = new FCPoint(0, 0);

    /**
     * 获取控件的位置
     */
    public FCPoint getLocation() {
        if (m_percentLocation != null) {
            FCPoint location = new FCPoint(getLeft(), getTop());
            return location;
        } else {
            return m_location.clone();
        }
    }

    /**
     * 设置控件的位置
     */
    public void setLocation(FCPoint value) {
        if (m_location.x != value.x || m_location.y != value.y) {
            if (m_percentLocation != null) {
                m_oldLocation = getLocation().clone();
                if (m_percentLocation.x != -1) {
                } else {
                    m_location.x = value.x;
                }
                if (m_percentLocation.y != -1) {
                } else {
                    m_location.y = value.y;
                }
            } else {
                m_oldLocation = m_location.clone();
                m_location = value.clone();
            }
            onLocationChanged();
        }
    }

    protected FCPadding m_margin = new FCPadding(0);

    /**
     * 获取外边距
     */
    public FCPadding getMargin() {
        return m_margin.clone();
    }

    /**
     * 设置外边距
     */
    public void setMargin(FCPadding value) {
        m_margin = value.clone();
        onMarginChanged();
    }

    protected FCSize m_maximumSize = new FCSize(100000, 100000);

    /**
     * 获取控件的最大尺寸
     */
    public FCSize getMaximumSize() {
        return m_maximumSize.clone();
    }

    /**
     * 设置控件的最大尺寸
     */
    public void setMaximumSize(FCSize value) {
        m_maximumSize = value.clone();
    }

    protected FCSize m_minimumSize = new FCSize(0, 0);

    /**
     * 获取控件的最小尺寸
     */
    public FCSize getMinimumSize() {
        return m_minimumSize.clone();
    }

    /**
     * 设置控件的最小尺寸
     */
    public void setMinimumSize(FCSize value) {
        m_minimumSize = value.clone();
    }

    public FCPoint getTouchPoint() {
        FCPoint mp = getNative().getTouchPoint();
        return pointToControl(mp);
    }

    protected String m_name = "";

    /**
     * 获取控件的唯一标识名称
     */
    public String getName() {
        return m_name;
    }

    /**
     * 设置控件的唯一标识名称
     */
    public void setName(String value) {
        m_name = value;
    }

    protected FCNative m_native = null;

    /**
     * 获取按钮所在的图形接口
     */
    public FCNative getNative() {
        return m_native;
    }

    /**
     * 设置按钮所在的图形接口
     */
    public void setNative(FCNative value) {
        m_native = value;
        int controlsSize = m_controls.size();
        for (int i = 0; i < controlsSize; i++) {
            m_controls.get(i).setNative(value);
        }
        onLoad();
    }

    protected float m_opacity = 1;

    /**
     * 获取透明度
     */
    public float getOpacity() {
        return m_opacity;
    }

    /**
     * 设置透明度
     */
    public void setOpacity(float value) {
        m_opacity = value;
    }

    protected FCPadding m_padding = new FCPadding(0);

    /**
     * 获取内边距
     */
    public FCPadding getPadding() {
        return m_padding.clone();
    }

    /**
     * 设置内边距
     */
    public void setPadding(FCPadding value) {
        m_padding = value.clone();
        onPaddingChanged();
    }

    protected FCView m_parent = null;

    /**
     * 获取父控件
     */
    public FCView getParent() {
        return m_parent;
    }

    /**
     * 设置父控件
     */
    public void setParent(FCView value) {
        if (m_parent != value) {
            m_parent = value;
            onParentChanged();
        }
    }

    protected FCRect m_region = new FCRect();

    /**
     * 获取裁剪矩形
     */
    public FCRect getRegion() {
        return m_region.clone();
    }

    /**
     * 设置裁剪矩形
     */
    public void setRegion(FCRect value) {
        m_useRegion = true;
        m_region = value.clone();
        onRegionChanged();
    }

    protected String m_resourcePath;

    /**
     * 获取资源路径
     */
    public String getResourcePath() {
        return m_resourcePath;
    }

    /**
     * 设置资源路径
     */
    public void setResourcePath(String value) {
        m_resourcePath = value;
    }

    /**
     * 获取距离右侧的距离
     */
    public int getRight() {
        return getLeft() + getWidth();
    }

    protected FCSize m_size = new FCSize();

    /**
     * 获取尺寸
     */
    public FCSize getSize() {
        if (m_percentSize != null) {
            FCSize size = new FCSize(getWidth(), getHeight());
            return size;
        } else {
            return m_size.clone();
        }
    }

    /**
     * 设置尺寸
     */
    public void setSize(FCSize value) {
        FCSize newSize = value.clone();
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
            if (m_percentSize != null) {
                m_oldSize = getSize();
                if (m_percentSize.cx != -1) {
                } else {
                    m_size.cx = newSize.cx;
                }
                if (m_percentSize.cy != -1) {
                } else {
                    m_size.cy = newSize.cy;
                }
            } else {
                m_oldSize = m_size.clone();
                m_size = newSize;
            }
            onSizeChanged();
            update();
        }
    }

    protected Object m_tag = null;

    /**
     * 获取TAG值
     */
    public Object getTag() {
        return m_tag;
    }

    /**
     * 设置TAG值
     */
    public void setTag(Object value) {
        m_tag = value;
    }

    protected String m_text = "";

    /**
     * 获取文字
     */
    public String getText() {
        return m_text;
    }

    /**
     * 设置文字
     */
    public void setText(String value) {
        if (!m_text.equals(value)) {
            m_text = value;
            onTextChanged();
        }
    }

    protected long m_textColor = FCColor.Text;

    /**
     * 获取前景色
     */
    public long getTextColor() {
        return m_textColor;
    }

    /**
     * 设置前景色
     */
    public void setTextColor(long value) {
        if (m_textColor != value) {
            m_textColor = value;
            onTextColorChanged();
        }
    }

    /**
     * 获取距离上侧的位置
     */
    public int getTop() {
        if (m_percentLocation != null && m_percentLocation.y != -1) {
            FCSize parentSize = (m_parent != null ? m_parent.getSize() : m_native.getDisplaySize());
            return (int) (parentSize.cy * m_percentLocation.y);
        } else {
            return m_location.y;
        }
    }

    /**
     * 设置距离上侧的位置
     */
    public void setTop(int value) {
        if (m_percentLocation != null && m_percentLocation.y != -1) {
            return;
        } else {
            setLocation(new FCPoint(m_location.x, value));
        }
    }

    protected boolean m_topMost = false;

    /**
     * 获取是否置顶显示
     */
    public boolean isTopMost() {
        return m_topMost;
    }

    /**
     * 获取是否置顶显示
     */
    public void setTopMost(boolean value) {
        m_topMost = value;
    }

    protected boolean m_useRegion = false;

    /**
     * 获取或设置是否设置裁剪
     */
    public boolean useRegion() {
        return m_useRegion;
    }

    protected FCVerticalAlign m_verticalAlign = FCVerticalAlign.Top;

    /**
     * 获取纵向排列方式
     */
    public FCVerticalAlign getVerticalAlign() {
        return m_verticalAlign;
    }

    /**
     * 设置纵向排列方式
     */
    public void setVerticalAlign(FCVerticalAlign verticalAlign) {
        m_verticalAlign = verticalAlign;
    }

    protected boolean m_visible = true;

    /**
     * 获取控件是否可见
     */
    public boolean isVisible() {
        return m_visible;
    }

    /**
     * 设置控件是否可见
     */
    public void setVisible(boolean value) {
        if (m_visible != value) {
            m_visible = value;
            onVisibleChanged();
        }
    }

    /**
     * 获取控件的宽度
     */
    public int getWidth() {
        if (m_percentSize != null && m_percentSize.cx != -1) {
            FCSize parentSize = (m_parent != null ? m_parent.getSize() : m_native.getDisplaySize());
            return (int) (parentSize.cx * m_percentSize.cx);
        } else {
            return m_size.cx;
        }
    }

    /**
     * 设置控件的宽度
     */
    public void setWidth(int value) {
        if (m_percentSize != null && m_percentSize.cx != -1) {
            return;
        } else {
            setSize(new FCSize(value, m_size.cy));
        }
    }

    /**
     * 添加控件
     *
     * @param control 控件
     */
    public void addControl(FCView control) {
        control.setParent(this);
        control.setNative(m_native);
        m_controls.add(control);
        control.onAdd();
    }

    /**
     * 在控件线程中调用方法
     *
     * @param args 参数
     */
    public void beginInvoke(Object args) {
        if (m_native != null) {
            FCHost host = m_native.getHost();
            host.beginInvoke(this, args);
        }
    }

    /**
     * 注册事件
     *
     * @param func 函数指针
     * @param eventID 事件ID
     */
    public void addEvent(Object func, int eventID) {
        if (m_events == null) {
            m_events = new java.util.HashMap<Integer, ArrayList<Object>>();
        }
        ArrayList<Object> eventList = null;
        if (m_events.containsKey(eventID)) {
            eventList = m_events.get(eventID);
        } else {
            eventList = new ArrayList<Object>();
            m_events.put(eventID, eventList);
        }
        eventList.add(func);
    }

    /**
     * 将子控件置于最前
     *
     * @param childControl 子控件
     */
    public void bringChildToFront(FCView childControl) {
        if (m_controls != null && m_controls.size() > 0) {
            m_controls.remove(childControl);
            m_controls.add(childControl);
        }
    }

    /**
     * 将控件放到最前显示
     */
    public void bringToFront() {
        if (m_native != null) {
            m_native.bringToFront(this);
        }
    }

    /**
     * 调用事件
     *
     * @param eventID 事件ID
     */
    protected void callEvents(int eventID) {
        if (m_canRaiseEvents) {
            if (m_events != null && m_events.containsKey(eventID)) {
                ArrayList<Object> events = m_events.get(eventID);
                int eventSize = events.size();
                for (int i = 0; i < eventSize; i++) {
                    FCEvent func = (FCEvent) ((events.get(i) instanceof FCEvent) ? events.get(i) : null);
                    if (func != null) {
                        func.callEvent(eventID, this);
                    }
                }
            }
        }
    }

    /**
     * 调用控件线程方法事件
     *
     * @param eventID 事件ID
     * @param args 参数
     */
    protected void callInvokeEvents(int eventID, Object args) {
        if (m_canRaiseEvents) {
            if (m_events != null && m_events.containsKey(eventID)) {
                ArrayList<Object> events = m_events.get(eventID);
                int eventSize = events.size();
                for (int i = 0; i < eventSize; i++) {
                    FCInvokeEvent func = (FCInvokeEvent) ((events.get(i) instanceof FCInvokeEvent) ? events.get(i) : null);
                    if (func != null) {
                        func.callControlInvokeEvent(eventID, this, args);
                    }
                }
            }
        }
    }

    /**
     * 调用触摸事件
     *
     * @param eventID 事件ID
     * @param touchInfo 触摸信息
     */
    protected void callTouchEvents(int eventID, FCTouchInfo touchInfo) {
        if (m_canRaiseEvents) {
            if (m_events != null && m_events.containsKey(eventID)) {
                ArrayList<Object> events = m_events.get(eventID);
                int eventSize = events.size();
                for (int i = 0; i < eventSize; i++) {
                    FCTouchEvent func = (FCTouchEvent) ((events.get(i) instanceof FCTouchEvent) ? events.get(i) : null);
                    if (func != null) {
                        func.callControlTouchEvent(eventID, this, touchInfo.clone());
                    }
                }
            }
        }
    }

    /**
     * 调用重绘事件
     *
     * @param eventID 事件ID
     * @param paint 绘图对象
     * @param clipRect 裁剪区域
     */
    protected void callPaintEvents(int eventID, FCPaint paint, FCRect clipRect) {
        if (m_canRaiseEvents) {
            if (m_events != null && m_events.containsKey(eventID)) {
                ArrayList<Object> events = m_events.get(eventID);
                int eventSize = events.size();
                for (int i = 0; i < eventSize; i++) {
                    FCPaintEvent func = (FCPaintEvent) ((events.get(i) instanceof FCPaintEvent) ? events.get(i) : null);
                    if (func != null) {
                        func.callControlPaintEvent(eventID, this, paint, clipRect);
                    }
                }
            }
        }
    }

    /**
     * 调用重绘事件
     *
     * @param eventID 事件ID
     * @param tEventID 事件ID2
     * @param touchInfo 触摸信息
     */
    protected boolean callPreviewsTouchEvents(int eventID, int tEventID, FCTouchInfo touchInfo) {
        if (m_canRaiseEvents) {
            if (m_events != null && m_events.containsKey(eventID)) {
                ArrayList<Object> events = m_events.get(eventID);
                int eventSize = events.size();
                for (int i = 0; i < eventSize; i++) {
                    FCPreviewsTouchEvent func = (FCPreviewsTouchEvent) ((events.get(i) instanceof FCTouchEvent) ? events.get(i) : null);
                    if (func != null) {
                        if (func.callPreviewsControlTouchEvent(eventID, tEventID, this, touchInfo.clone())) {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    /**
     * 调用秒表事件
     *
     * @param eventID 事件ID
     * @param timerID 秒表编号
     */
    protected void callTimerEvents(int eventID, int timerID) {
        if (m_canRaiseEvents) {
            if (m_events != null && m_events.containsKey(eventID)) {
                ArrayList<Object> events = m_events.get(eventID);
                int eventSize = events.size();
                for (int i = 0; i < eventSize; i++) {
                    FCTimerEvent func = (FCTimerEvent) ((events.get(i) instanceof FCTimerEvent) ? events.get(i) : null);
                    if (func != null) {
                        func.callControlTimerEvent(eventID, this, timerID);
                    }
                }
            }
        }
    }

    /**
     * 清除所有控件
     */
    public void clearControls() {
        ArrayList<FCView> controls = new ArrayList<FCView>();
        for (FCView control : m_controls) {
            controls.add(control);
        }
        for (FCView control : controls) {
            control.onRemove();
            control.delete();
        }
        m_controls.clear();
    }

    /**
     * 是否包含控件
     *
     * @param control 控件
     * @returns 是否包含
     */
    public boolean containsControl(FCView control) {
        for (FCView subControl : m_controls) {
            if (subControl == control) {
                return true;
            }
        }
        return false;
    }

    /**
     * 是否包含控件
     *
     * @param point 坐标
     * @returns 是否包含
     */
    public boolean containsPoint(FCPoint point) {
        FCPoint cPoint = pointToControl(point);
        FCSize size = getSize();
        if (cPoint.x >= 0 && cPoint.x <= size.cx && cPoint.y >= 0 && cPoint.y <= size.cy) {
            if (m_useRegion) {
                if (cPoint.x >= m_region.left && cPoint.x <= m_region.right && cPoint.y >= m_region.top && cPoint.y <= m_region.bottom) {
                    return true;
                }
            } else {
                return true;
            }
        }
        return false;
    }

    /**
     * 销毁资源
     */
    public void delete() {
        if (!m_isDeleted) {
            if (m_events != null) {
                m_events.clear();
            }
            clearControls();
            m_isDeleted = true;
        }
    }

    /**
     * 设置焦点
     */
    public void focus() {
        setFocused(true);
    }

    /**
     * 获取控件集合的拷贝
     *
     * @returns 控件集合
     */
    public ArrayList<FCView> getControls() {
        return m_controls;
    }

    /**
     * 获取控件类型
     *
     * @returns 控件类型
     */
    public String getControlType() {
        return "BaseControl";
    }

    /**
     * 获取显示偏移坐标
     *
     * @returns 坐标
     */
    public FCPoint getDisplayOffset() {
        return new FCPoint(0, 0);
    }

    /**
     * 获取新的秒表编号
     *
     * @returns 新编号
     */
    public static int getNewTimerID() {
        return m_timerID++;
    }

    /**
     * 获取或设置的背景色
     *
     * @returns 背景色
     */
    protected long getPaintingBackColor() {
        if (m_backColor != FCColor.None && FCColor.DisabledBack != FCColor.None) {
            if (!isPaintEnabled(this)) {
                return FCColor.DisabledBack;
            }
        }
        return m_backColor;
    }

    /**
     * 获取要绘制的背景图片
     *
     * @returns 背景图片
     */
    protected String getPaintingBackImage() {
        return m_backImage;
    }

    /**
     * 获取要绘制的边线颜色
     *
     * @returns 边线颜色
     */
    protected long getPaintingBorderColor() {
        return m_borderColor;
    }

    /**
     * 获取要绘制的前景色
     *
     * @returns 前景色
     */
    protected long getPaintingTextColor() {
        if (m_textColor != FCColor.Text && FCColor.DisabledText != FCColor.None) {
            if (!isPaintEnabled(this)) {
                return FCColor.DisabledText;
            }
        }
        return m_textColor;
    }

    /**
     * 获取弹出菜单上下文
     *
     * @param control 当前控件
     * @returns 控件
     */
    public FCView getPopupMenuContext(FCView control) {
        if (m_hasPopupMenu) {
            return this;
        } else {
            if (m_parent != null) {
                return getPopupMenuContext(m_parent);
            } else {
                return null;
            }
        }
    }

    /**
     * 获取属性值
     *
     * @param name 属性名称
     * @param value 返回属性值
     * @param type 返回属性类型
     */
    public void getProperty(String name, RefObject<String> value, RefObject<String> type) {
        int len = name.length();
        switch (len) {
            case 2: {
                if (name.equals("id")) {
                    type.argvalue = "text";
                    value.argvalue = getName();
                }
            }
            case 3: {
                if (name.equals("top")) {
                    type.argvalue = "float";
                    if (m_percentLocation != null && m_percentLocation.y != -1) {
                        value.argvalue = "%" + FCStr.convertFloatToStr(100 * m_percentLocation.y);
                    } else {
                        value.argvalue = FCStr.convertIntToStr(getTop());
                    }
                }
                break;
            }
            case 4: {
                if (name.equals("dock")) {
                    type.argvalue = "enum:FCDockStyle";
                    value.argvalue = FCStr.convertDockToStr(getDock());
                } else if (name.equals("font")) {
                    type.argvalue = "font";
                    value.argvalue = FCStr.convertFontToStr(getFont());
                } else if (name.equals("left")) {
                    type.argvalue = "float";
                    if (m_percentLocation != null && m_percentLocation.x != -1) {
                        value.argvalue = "%" + FCStr.convertFloatToStr(100 * m_percentLocation.x);
                    } else {
                        value.argvalue = FCStr.convertIntToStr(getLeft());
                    }
                } else if (name.equals("name")) {
                    type.argvalue = "text";
                    value.argvalue = getName();
                } else if (name.equals("size")) {
                    type.argvalue = "size";
                    if (m_percentSize != null) {
                        String pWidth = "", pHeight = "", pType = "";
                        RefObject<String> refPWidth = new RefObject<String>(pWidth);
                        RefObject<String> refPHeight = new RefObject<String>(pHeight);
                        RefObject<String> refPType = new RefObject<String>(pType);
                        getProperty("width", refPWidth, refPType);
                        getProperty("height", refPHeight, refPType);
                        value.argvalue = refPWidth.argvalue + "," + refPHeight.argvalue;
                    } else {
                        value.argvalue = FCStr.convertSizeToStr(getSize());
                    }
                } else if (name.equals("text")) {
                    type.argvalue = "text";
                    value.argvalue = getText();
                }
                break;
            }
            case 5: {
                if (name.equals("align")) {
                    type.argvalue = "enum:FCHorizontalAlign";
                    value.argvalue = FCStr.convertHorizontalAlignToStr(getAlign());
                } else if (name.equals("value")) {
                    type.argvalue = "text";
                    value.argvalue = getText();
                } else if (name.equals("width")) {
                    type.argvalue = "float";
                    if (m_percentSize != null && m_percentSize.cx != -1) {
                        value.argvalue = "%" + FCStr.convertFloatToStr(100 * m_percentSize.cx);
                    } else {
                        value.argvalue = FCStr.convertIntToStr(getWidth());
                    }
                }
                break;
            }
            case 6: {
                if (name.equals("anchor")) {
                    type.argvalue = "anchor";
                    value.argvalue = FCStr.convertAnchorToStr(getAnchor());
                } else if (name.equals("bounds")) {
                    type.argvalue = "rect";
                    value.argvalue = FCStr.convertRectToStr(getBounds());
                } else if (name.equals("height")) {
                    type.argvalue = "float";
                    if (m_percentSize != null && m_percentSize.cy != -1) {
                        value.argvalue = "%" + FCStr.convertFloatToStr(100 * m_percentSize.cy);
                    } else {
                        value.argvalue = FCStr.convertIntToStr(getHeight());
                    }
                } else if (name.equals("margin")) {
                    type.argvalue = "margin";
                    value.argvalue = FCStr.convertPaddingToStr(getMargin());
                } else if (name.equals("region")) {
                    type.argvalue = "rect";
                    value.argvalue = FCStr.convertRectToStr(getRegion());
                }
                break;
            }
            case 7: {
                if (name.equals("enabled")) {
                    type.argvalue = "bool";
                    value.argvalue = FCStr.convertBoolToStr(isEnabled());
                } else if (name.equals("focused")) {
                    type.argvalue = "bool";
                    value.argvalue = FCStr.convertBoolToStr(isFocused());
                } else if (name.equals("opacity")) {
                    type.argvalue = "float";
                    value.argvalue = FCStr.convertFloatToStr(getOpacity());
                } else if (name.equals("padding")) {
                    type.argvalue = "padding";
                    value.argvalue = FCStr.convertPaddingToStr(getPadding());
                } else if (name.equals("topmost")) {
                    type.argvalue = "bool";
                    value.argvalue = FCStr.convertBoolToStr(isTopMost());
                } else if (name.equals("visible")) {
                    type.argvalue = "bool";
                    value.argvalue = FCStr.convertBoolToStr(isVisible());
                }
                break;
            }
            case 8: {
                if (name.equals("autosize")) {
                    type.argvalue = "bool";
                    value.argvalue = FCStr.convertBoolToStr(autoSize());
                } else if (name.equals("canfocus")) {
                    type.argvalue = "bool";
                    value.argvalue = FCStr.convertBoolToStr(canFocus());
                } else if (name.equals("iswindow")) {
                    type.argvalue = "bool";
                    value.argvalue = FCStr.convertBoolToStr(isWindow());
                } else if (name.equals("location")) {
                    type.argvalue = "FCPoint";
                    if (m_percentLocation != null) {
                        String pLeft = "", pTop = "", pType = "";
                        RefObject<String> refPLeft = new RefObject<String>(pLeft);
                        RefObject<String> refPTop = new RefObject<String>(pTop);
                        RefObject<String> refPType = new RefObject<String>(pType);
                        getProperty("left", refPLeft, refPType);
                        getProperty("top", refPTop, refPType);
                        value.argvalue = refPLeft.argvalue + "," + refPTop.argvalue;
                    } else {
                        value.argvalue = FCStr.convertPointToStr(getLocation());
                    }
                }
                break;
            }
            case 9: {
                if (name.equals("allowdrag")) {
                    type.argvalue = "bool";
                    value.argvalue = FCStr.convertBoolToStr(allowDrag());
                } else if (name.equals("backcolor")) {
                    type.argvalue = "color";
                    value.argvalue = FCStr.convertColorToStr(getBackColor());
                } else if (name.equals("backimage")) {
                    type.argvalue = "text";
                    value.argvalue = getBackImage();
                } else if (name.equals("textcolor")) {
                    type.argvalue = "color";
                    value.argvalue = FCStr.convertColorToStr(getTextColor());
                }
                break;
            }
            default: {
                if (name.equals("allowpreviewsevent")) {
                    type.argvalue = "bool";
                    value.argvalue = FCStr.convertBoolToStr(allowPreviewsEvent());
                } else if (name.equals("autoellipsis")) {
                    type.argvalue = "bool";
                    value.argvalue = FCStr.convertBoolToStr(autoEllipsis());
                } else if (name.equals("bordercolor")) {
                    type.argvalue = "color";
                    value.argvalue = FCStr.convertColorToStr(getBorderColor());
                } else if (name.equals("canraiseevents")) {
                    type.argvalue = "bool";
                    value.argvalue = FCStr.convertBoolToStr(canRaiseEvents());
                } else if (name.equals("cornerradius")) {
                    type.argvalue = "int";
                    value.argvalue = FCStr.convertIntToStr(getCornerRadius());
                } else if (name.equals("displayoffset")) {
                    type.argvalue = "bool";
                    value.argvalue = FCStr.convertBoolToStr(displayOffset());
                } else if (name.equals("haspopupmenu")) {
                    type.argvalue = "bool";
                    value.argvalue = FCStr.convertBoolToStr(getHasPopupMenu());
                } else if (name.equals("maximumsize")) {
                    type.argvalue = "size";
                    value.argvalue = FCStr.convertSizeToStr(getMaximumSize());
                } else if (name.equals("minimumsize")) {
                    type.argvalue = "size";
                    value.argvalue = FCStr.convertSizeToStr(getMinimumSize());
                } else if (name.equals("resourcepath")) {
                    type.argvalue = "text";
                    value.argvalue = getResourcePath();
                } else if (name.equals("vertical-align")) {
                    type.argvalue = "enum:FCVerticalAlign";
                    value.argvalue = FCStr.convertVerticalAlignToStr(getVerticalAlign());
                }
                break;
            }
        }
    }

    /**
     * 获取属性名称列表
     */
    public ArrayList<String> getPropertyNames() {
        ArrayList<String> propertyNames = new ArrayList<String>();
        propertyNames.addAll(Arrays.asList(new String[]{
            "Align", "AllowDrag", "AllowPreviewsEvent", "Anchor", "AutoEllipsis", "AutoSize", "BackColor",
            "BackImage", "BorderColor", "Bounds", "CanFocus", "CanRaiseEvents", "CornerRadius", "Cursor", "DisplayOffset", "Dock", "Enabled", "Focused", "Font",
            "HasPopupMenu", "Height", "IsWindow", "Left", "Location", "Margin", "MaximumSize", "MinimumSize", "Name", "Opacity", "Padding", "Region", "ResourcePath", "Size", "TabIndex", "TabStop", "Text", "TextColor", "Top", "TopMost", "Value", "Vertical-Align", "Visible", "Width"}));
        return propertyNames;
    }

    /**
     * 判断是否包含子控件
     *
     * @returns 是否包含子控件
     */
    public boolean hasChildren() {
        return m_controls.size() > 0;
    }

    /**
     * 隐藏控件
     */
    public void hide() {
        setVisible(false);
    }

    /**
     * 插入控件
     *
     * @param index 索引
     * @param control 控件
     */
    public void insertControl(int index, FCView control) {
        m_controls.add(index, control);
    }

    /**
     * 启动绘制
     */
    public void invalidate() {
        if (m_native != null) {
            m_native.invalidate(this);
        }
    }

    /**
     * 在控件线程中调用方法
     *
     * @param args 参数
     */
    public void invoke(Object args) {
        if (m_native != null) {
            FCHost host = m_native.getHost();
            host.invoke(this, args);
        }
    }

    /**
     * 判断是否绘制可用状态
     *
     * @param control 控件
     * @returns 是否绘制可用状态
     */
    public boolean isPaintEnabled(FCView control) {
        if (control.isEnabled()) {
            FCView parent = control.getParent();
            if (parent != null) {
                return isPaintEnabled(parent);
            } else {
                return true;
            }
        } else {
            return false;
        }
    }

    /**
     * 判断是否绘图时可见
     *
     * @param control 控件
     * @returns 是否可见
     */
    public boolean isPaintVisible(FCView control) {
        if (control.isEnabled()) {
            FCView parent = control.getParent();
            if (parent != null) {
                return isPaintVisible(parent);
            } else {
                return true;
            }
        } else {
            return false;
        }
    }

    /**
     * 添加控件方法
     */
    public void onAdd() {
        callEvents(FCEventID.ADD);
    }

    /**
     * 自动设置尺寸属性改变方法
     */
    public void onAutoSizeChanged() {
        callEvents(FCEventID.AUTOSIZECHANGED);
    }

    public void onBackColorChanged() {
        callEvents(FCEventID.BACKCOLORCHANGED);
    }

    /**
     * 背景色改变方法
     */
    public void onBackImageChanged() {
        callEvents(FCEventID.BACKIMAGECHANGED);
    }

    /**
     * 字符输入
     */
    public void onClick(FCTouchInfo touchInfo) {
        callTouchEvents(FCEventID.CLICK, touchInfo.clone());
    }

    /**
     * 复制
     */
    public void onCopy() {
        callEvents(FCEventID.COPY);
    }

    /**
     * 剪切
     */
    public void onCut() {
        callEvents(FCEventID.CUT);
    }

    /**
     * 悬停改变方法
     */
    public void onDockChanged() {
        callEvents(FCEventID.DOCKCHANGED);
    }

    /**
     * 触摸双击方法
     *
     * @param touchInfo 触摸信息
     */
    public void onDoubleClick(FCTouchInfo touchInfo) {
        callTouchEvents(FCEventID.DOUBLECLICK, touchInfo.clone());
    }

    /**
     * 拖动开始方法
     *
     * @returns 是否拖动
     */
    public boolean onDragBegin() {
        callEvents(FCEventID.DRAGBEGIN);
        return true;
    }

    /**
     * 拖动结束方法
     */
    public void onDragEnd() {
        m_isDragging = false;
        callEvents(FCEventID.DRAGEND);
    }

    /**
     * 正在拖动方法
     */
    public void onDragging() {
        m_isDragging = true;
        callEvents(FCEventID.DRAGGING);
    }

    /**
     * 拖动准备方法
     *
     * @param startOffset 可以拖动的偏移坐标量
     */
    public void onDragReady(RefObject<FCPoint> startOffset) {
        startOffset.argvalue.x = 5;
        startOffset.argvalue.y = 5;
    }

    /**
     * 可用改变方法
     */
    public void onEnableChanged() {
        callEvents(FCEventID.ENABLECHANGED);
    }

    /**
     * 字体改变方法
     */
    public void onFontChanged() {
        callEvents(FCEventID.FONTCHANGED);
    }

    /**
     * 获得焦点方法
     */
    public void onGotFocus() {
        callEvents(FCEventID.GOTFOCUS);
    }

    /**
     * 在控件线程中调用方法
     *
     * @param args 参数
     */
    public void onInvoke(Object args) {
        callInvokeEvents(FCEventID.INVOKE, args);
    }

    /**
     * 控件加载方法
     */
    public void onLoad() {
        callEvents(FCEventID.LOAD);
    }

    /**
     * 位置改变方法
     */
    public void onLocationChanged() {
        callEvents(FCEventID.LOCATIONCHANGED);
    }

    /**
     * 丢失焦点方法
     */
    public void onLostfocus() {
        callEvents(FCEventID.LOSTFOCUS);
    }

    /**
     * 外边距改变方法
     */
    public void onMarginChanged() {
        callEvents(FCEventID.MARGINCHANGED);
    }

    /**
     * 触摸按下方法
     *
     * @param touchInfo 触摸信息
     */
    public void onTouchDown(FCTouchInfo touchInfo) {
        callTouchEvents(FCEventID.TOUCHDOWN, touchInfo.clone());
    }

    /**
     * 触摸进入方法
     *
     * @param touchInfo 触摸信息
     */
    public void onTouchEnter(FCTouchInfo touchInfo) {
        callTouchEvents(FCEventID.TOUCHENTER, touchInfo.clone());
    }

    /**
     * 触摸离开方法
     *
     * @param touchInfo 触摸信息
     */
    public void onTouchLeave(FCTouchInfo touchInfo) {
        callTouchEvents(FCEventID.TOUCHLEAVE, touchInfo.clone());
    }

    /**
     * 触摸移动调用方法
     *
     * @param touchInfo 触摸信息
     */
    public void onTouchMove(FCTouchInfo touchInfo) {
        callTouchEvents(FCEventID.TOUCHMOVE, touchInfo.clone());
    }

    /**
     * 触摸抬起方法
     *
     * @param touchInfo 触摸信息
     */
    public void onTouchUp(FCTouchInfo touchInfo) {
        callTouchEvents(FCEventID.TOUCHUP, touchInfo.clone());
    }

    /**
     * 内边距改变方法
     */
    public void onPaddingChanged() {
        callEvents(FCEventID.PADDINGCHANGED);
    }

    /**
     * 重绘方法
     *
     * @param paint 绘图对象
     * @param clipRect 裁剪区域
     */
    public void onPaint(FCPaint paint, FCRect clipRect) {
        onPaintBackground(paint, clipRect);
        onPaintForeground(paint, clipRect);
        callPaintEvents(FCEventID.PAINT, paint, clipRect);
    }

    /**
     * 重绘背景方法
     *
     * @param paint 绘图对象
     * @param clipRect 裁剪区域
     */
    public void onPaintBackground(FCPaint paint, FCRect clipRect) {
        FCRect rect = new FCRect(0, 0, getWidth(), getHeight());
        //绘制背景色
        paint.fillRoundRect(getPaintingBackColor(), rect, m_cornerRadius);
        //绘制背景图
        String bkImage = getPaintingBackImage();
        if (bkImage != null && bkImage.length() > 0) {
            paint.drawImage(bkImage, rect);
        }
    }

    /**
     * 重绘边线方法
     *
     * @param paint 绘图对象
     * @param clipRect 裁剪区域
     */
    public void onPaintBorder(FCPaint paint, FCRect clipRect) {
        FCRect borderRect = new FCRect(0, 0, getWidth(), getHeight());
        //绘制边线
        paint.drawRoundRect(getPaintingBorderColor(), 1, 0, borderRect, m_cornerRadius);
        callPaintEvents(FCEventID.PAINTBORDER, paint, clipRect);
    }

    /**
     * 重绘前景方法
     *
     * @param paint 绘图对象
     * @param clipRect 裁剪区域
     */
    public void onPaintForeground(FCPaint paint, FCRect clipRect) {
    }

    /**
     * 父容器改变方法
     */
    public void onParentChanged() {
        callEvents(FCEventID.PARENTCHANGED);
    }

    /**
     * 复制
     */
    public void onPaste() {
        callEvents(FCEventID.PASTE);
    }

    /**
     * 预绘图方法
     *
     * @param paint 绘图对象
     * @param clipRect 裁剪区域
     */
    public void onPrePaint(FCPaint paint, FCRect clipRect) {
    }

    /**
     * 键盘下按方法
     *
     * @param eventID 事件ID
     * @param key 按键
     * @returns 状态
     */
    public boolean onPreviewsTouchEvent(int eventID, FCTouchInfo touchInfo) {
        return callPreviewsTouchEvents(FCEventID.PREVIEWSTOUCHEVENT, eventID, touchInfo.clone());
    }

    /**
     * 移除控件方法
     */
    public void onRemove() {
        callEvents(FCEventID.REMOVE);
    }

    /**
     * 裁剪区域改变方法
     */
    public void onRegionChanged() {
        callEvents(FCEventID.REGIONCHANGED);
    }

    /**
     * 尺寸改变方法
     */
    public void onSizeChanged() {
        callEvents(FCEventID.SIZECHANGED);
    }

    /**
     * TAB索引改变方法
     */
    public void onTabIndexChanged() {
        callEvents(FCEventID.TABINDEXCHANGED);
    }

    /**
     * 被使用TAB切换方法
     */
    public void onTabStop() {
        callEvents(FCEventID.TABSTOP);
    }

    /**
     * 是否用TAB切换改变方法
     */
    public void onTabStopChanged() {
        callEvents(FCEventID.TABSTOPCHANGED);
    }

    /**
     * 文本改变方法
     */
    public void onTextChanged() {
        callEvents(FCEventID.TEXTCHANGED);
    }

    /**
     * 前景色改变方法
     */
    public void onTextColorChanged() {
        callEvents(FCEventID.TEXTCOLORCHANGED);
    }

    /**
     * 秒表回调方法
     *
     * @param timerID 编号
     */
    public void onTimer(int timerID) {
        callTimerEvents(FCEventID.TIMER, timerID);
    }

    /**
     * 可见状态改变方法
     */
    public void onVisibleChanged() {
        callEvents(FCEventID.VISIBLECHANGED);
    }

    public FCPoint pointToControl(FCPoint FCPoint) {
        if (m_native != null) {
            int clientX = m_native.clientX(this);
            int clientY = m_native.clientY(this);
            return new FCPoint(FCPoint.x - clientX, FCPoint.y - clientY);
        } else {
            return FCPoint.clone();
        }
    }

    /**
     * 获取相对于控件的相对坐标
     *
     * @param point 坐标
     * @returns 相对坐标
     */
    public FCPoint pointToNative(FCPoint point) {
        if (m_native != null) {
            int clientX = m_native.clientX(this);
            int clientY = m_native.clientY(this);
            return new FCPoint(point.x + clientX, point.y + clientY);
        } else {
            return point.clone();
        }
    }

    /**
     * 移除控件
     *
     * @param control 控件
     */
    public void removeControl(FCView control) {
        if (m_native != null) {
            m_native.removeControl(control);
        }
        m_controls.remove(control);
        control.onRemove();
        control.setParent(null);
    }

    /**
     * 取消注册事件
     *
     * @param func 函数指针
     * @param eventID 事件ID
     */
    public void removeEvent(Object func, int eventID) {
        if (m_events != null && m_events.containsKey(eventID)) {
            m_events.get(eventID).remove(func);
        }
    }

    /**
     * 将控件置于最后
     *
     * @param childControl 子控件
     */
    public void sendChildToBack(FCView childControl) {
        if (m_controls != null && m_controls.size() > 0) {
            m_controls.remove(childControl);
            m_controls.add(0, childControl);
        }
    }

    /**
     * 将控件放到最下面显示
     */
    public void sendToBack() {
        if (m_native != null) {
            m_native.sendToBack(this);
        }
    }

    /**
     * 设置属性
     *
     * @param name 属性名称
     * @param value 属性值
     */
    public void setProperty(String name, String value) {
        int len = name.length();
        switch (len) {
            case 2: {
                if (name.equals("id")) {
                    setName(value);
                }
            }
            case 3: {
                if (name.equals("top")) {
                    if (value.indexOf("%") != -1) {
                        float percentValue = FCStr.convertStrToFloat(value.replace("%", "")) / 100;
                        if (m_percentLocation == null) {
                            m_percentLocation = new FCPointF();
                            m_percentLocation.x = -1;
                        }
                        m_percentLocation.y = percentValue;
                    } else {
                        if (m_percentLocation != null) {
                            m_percentLocation.y = -1;
                        }
                        setTop(FCStr.convertStrToInt(value));
                    }
                }
                break;
            }
            case 4: {
                if (name.equals("dock")) {
                    setDock(FCStr.convertStrToDock(value));
                } else if (name.equals("font")) {
                    setFont(FCStr.convertStrToFont(value));
                } else if (name.equals("left")) {
                    if (value.indexOf("%") != -1) {
                        float percentValue = FCStr.convertStrToFloat(value.replace("%", "")) / 100;
                        if (m_percentLocation == null) {
                            m_percentLocation = new FCPointF();
                            m_percentLocation.y = -1;
                        }
                        m_percentLocation.x = percentValue;
                    } else {
                        setLeft(FCStr.convertStrToInt(value));
                        if (m_percentLocation != null) {
                            m_percentLocation.x = -1;
                        }
                    }
                } else if (name.equals("name")) {
                    setName(value);
                } else if (name.equals("size")) {
                    setSize(FCStr.convertStrToSize(value));
                } else if (name.equals("text")) {
                    setText(value);
                }
                break;
            }
            case 5: {
                if (name.equals("align")) {
                    setAlign(FCStr.convertStrToHorizontalAlign(value));
                } else if (name.equals("value")) {
                    setText(value);
                } else if (name.equals("width")) {
                    if (value.indexOf("%") != -1) {
                        float percentValue = FCStr.convertStrToFloat(value.replace("%", "")) / 100;
                        if (m_percentSize == null) {
                            m_percentSize = new FCSizeF();
                            m_percentSize.cy = -1;
                        }
                        m_percentSize.cx = percentValue;
                    } else {
                        setWidth(FCStr.convertStrToInt(value));
                        if (m_percentSize != null) {
                            m_percentSize.cx = -1;
                        }
                    }
                }
                break;
            }
            case 6: {
                if (name.equals("anchor")) {
                    setAnchor(FCStr.convertStrToAnchor(value));
                } else if (name.equals("bounds")) {
                    setBounds(FCStr.convertStrToRect(value));
                } else if (name.equals("height")) {
                    if (value.indexOf("%") != -1) {
                        float percentValue = FCStr.convertStrToFloat(value.replace("%", "")) / 100;
                        if (m_percentSize == null) {
                            m_percentSize = new FCSizeF();
                            m_percentSize.cx = -1;
                        }
                        m_percentSize.cy = percentValue;
                    } else {
                        if (m_percentSize != null) {
                            m_percentSize.cy = -1;
                        }
                        setHeight(FCStr.convertStrToInt(value));
                    }
                } else if (name.equals("margin")) {
                    setMargin(FCStr.convertStrToPadding(value));
                } else if (name.equals("region")) {
                    setRegion(FCStr.convertStrToRect(value));
                }
                break;
            }
            case 7: {
                if (name.equals("enabled")) {
                    setEnabled(FCStr.convertStrToBool(value));
                } else if (name.equals("focused")) {
                    setFocused(FCStr.convertStrToBool(value));
                } else if (name.equals("opacity")) {
                    setOpacity(FCStr.convertStrToFloat(value));
                } else if (name.equals("padding")) {
                    setPadding(FCStr.convertStrToPadding(value));
                } else if (name.equals("topmost")) {
                    setTopMost(FCStr.convertStrToBool(value));
                } else if (name.equals("visible")) {
                    setVisible(FCStr.convertStrToBool(value));
                }
                break;
            }
            case 8: {
                if (name.equals("autosize")) {
                    setAutoSize(FCStr.convertStrToBool(value));
                } else if (name.equals("canfocus")) {
                    setCanFocus(FCStr.convertStrToBool(value));
                } else if (name.equals("iswindow")) {
                    setIsWindow(FCStr.convertStrToBool(value));
                } else if (name.equals("location")) {
                    setLocation(FCStr.convertStrToPoint(value));
                }
                break;
            }
            case 9: {
                if (name.equals("allowdrag")) {
                    setAllowDrag(FCStr.convertStrToBool(value));
                } else if (name.equals("backcolor")) {
                    setBackColor(FCStr.convertStrToColor(value));
                } else if (name.equals("backimage")) {
                    setBackImage(value);
                } else if (name.equals("textcolor")) {
                    setTextColor(FCStr.convertStrToColor(value));
                }
                break;
            }
            default: {
                if (name.equals("allowpreviewsevent")) {
                    setAllowPreviewsEvent(FCStr.convertStrToBool(value));
                } else if (name.equals("autoellipsis")) {
                    setAutoEllipsis(FCStr.convertStrToBool(value));
                } else if (name.equals("bordercolor")) {
                    setBorderColor(FCStr.convertStrToColor(value));
                } else if (name.equals("canraiseevents")) {
                    setcanRaiseEvents(FCStr.convertStrToBool(value));
                } else if (name.equals("cornerradius")) {
                    setCornerRadius(FCStr.convertStrToInt(value));
                } else if (name.equals("displayoffset")) {
                    setDisplayOffset(FCStr.convertStrToBool(value));
                } else if (name.equals("haspopupmenu")) {
                    setHasPopupMenu(FCStr.convertStrToBool(value));
                } else if (name.equals("maximumsize")) {
                    setMaximumSize(FCStr.convertStrToSize(value));
                } else if (name.equals("minimumsize")) {
                    setMinimumSize(FCStr.convertStrToSize(value));
                } else if (name.equals("resourcepath")) {
                    setResourcePath(value);
                } else if (name.equals("vertical-align")) {
                    setVerticalAlign(FCStr.convertStrToVerticalAlign(value));
                }
                break;
            }
        }
    }

    /**
     * 显示控件
     */
    public void show() {
        setVisible(true);
    }

    /**
     * 开始秒表
     *
     * @param timerID 编号
     * @param interval 间隔
     */
    public void startTimer(int timerID, int interval) {
        if (m_native != null) {
            m_native.startTimer(this, timerID, interval);
        }
    }

    /**
     * 停止秒表
     *
     * @param timerID 编号
     */
    public void stopTimer(int timerID) {
        if (m_native != null) {
            m_native.stopTimer(timerID);
        }
    }

    /**
     * 更新界面
     */
    public void update() {
        if (m_native != null) {
            m_native.setAlign(m_controls);
            if (m_oldSize.cx > 0 && m_oldSize.cy > 0) {
                m_native.setAnchor(m_controls, m_oldSize);
            }
            m_native.setDock(m_controls);
            m_oldLocation = getLocation();
            m_oldSize = getSize();
            int controlsSize = m_controls.size();
            for (int i = 0; i < controlsSize; i++) {
                m_controls.get(i).update();
            }
        }
    }
}
