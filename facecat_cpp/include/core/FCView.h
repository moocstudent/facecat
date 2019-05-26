/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCVIEW_H__
#define __FCVIEW_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "FCNative.h"
#include "FCPaint.h"
#include "FCStr.h"
#include "FCProperty.h"
#include "FCHost.h"

namespace FaceCat{
	static int timerID = 0;
	class FCNative;
	class FCView;

	typedef void (*FCEvent)(Object, Object);
	typedef void (*FCInvokeEvent)(Object, Object, Object);
	typedef void (*FCKeyEvent)(Object, char, Object);
	typedef void (*FCTouchEvent)(Object, FCTouchInfo, Object);
	typedef void (*FCPaintEvent)(Object, FCPaint*, const FCRect&, Object);
	typedef bool (*FCPreviewsKeyEvent)(Object, int, char, Object);
	typedef bool (*FCPreviewsTouchEvent)(Object, int, FCTouchInfo, Object);
	typedef void (*FCTimerEvent)(Object, int, Object);

    /**
	* 事件编号
	*/
	class FCEventID{
	public:
        static const int ADD = 0;
        static const int AUTOSIZECHANGED = 1;
        static const int BACKCOLORCHANGED = 2;
        static const int BACKIMAGECHANGED = 3;
        static const int CHAR = 4;
        static const int CLICK = 5;
        static const int COPY = 6;
        static const int CUT = 7;
        static const int DOCKCHANGED = 8;
        static const int DOUBLECLICK = 9;
        static const int DRAGBEGIN = 10;
        static const int DRAGEND = 11;
        static const int DRAGGING = 12;
        static const int ENABLECHANGED = 13;
        static const int FONTCHANGED = 14;
        static const int TEXTCOLORCHANGED = 15;
        static const int GOTFOCUS = 16;
        static const int INVOKE = 17;
        static const int KEYDOWN = 18;
        static const int KEYUP = 19;
        static const int LOAD = 20;
        static const int LOCATIONCHANGED = 21;
        static const int LOSTFOCUS = 22;
        static const int MARGINCHANGED = 23;
        static const int TOUCHDOWN = 24;
        static const int TOUCHENTER = 25;
        static const int TOUCHLEAVE = 26;
        static const int TOUCHMOVE = 27;
        static const int TOUCHUP = 28;
        static const int TOUCHWHEEL = 29;
        static const int PADDINGCHANGED = 30;
        static const int PARENTCHANGED = 31;
        static const int PAINT = 32;
        static const int PAINTBORDER = 33;
        static const int PASTE = 34;
        static const int REGIONCHANGED = 36;
        static const int REMOVE = 37;
        static const int SIZECHANGED = 38;
        static const int TABINDEXCHANGED = 39;
		static const int TABSTOP = 40;
        static const int TABSTOPCHANGED = 41;
        static const int TEXTCHANGED = 42;
        static const int TIMER = 43;
        static const int VISIBLECHANGED = 44;
        static const int CHECKEDCHANGED = 45;
        static const int SELECTEDTIMECHANGED = 46;
        static const int GRIDCELLCLICK = 47;
        static const int GRIDCELLEDITBEGIN = 48;
        static const int GRIDCELLEDITEND = 49;
        static const int GRIDCELLTOUCHDOWN = 50;
        static const int GRIDCELLTOUCHENTER = 51;
        static const int GRIDCELLTOUCHLEAVE = 52;
        static const int GRIDCELLTOUCHMOVE = 53;
        static const int GRIDCELLTOUCHUP = 54;
        static const int GRIDSELECTEDCELLSCHANGED = 55;
        static const int GRIDSELECTEDCOLUMNSSCHANGED = 56;
        static const int GRIDSELECTEDROWSCHANGED = 57;
        static const int MENUITEMCLICK = 58;
        static const int SELECTEDINDEXCHANGED = 59;
        static const int SELECTEDTABPAGECHANGED = 60;
        static const int SCROLLED = 61;
        static const int VALUECHANGED = 62;
        static const int WINDOWCLOSED = 63;
        static const int WINDOWCLOSING = 64;
		static const int PREVIEWSKEYEVENT = 65;
        static const int PREVIEWSTOUCHEVENT = 66;
        static const int USER = 100000;
	};

	/*
	* 控件的基类
	*/
	class FCView : public FCProperty{
	protected:
	    /**
		 * 横向排列方式
		 */
		FCHorizontalAlign m_align;
		/**
		 * 是否可以拖动位置
		 */
		bool m_allowDrag;
		/**
		 * 是否允许预处理事件
		 */
		bool m_allowPreviewsEvent;
		/**
		 * 锚定信息
		 */
		FCAnchor m_anchor;
		/**
		 * 是否在文字超出范围时在结尾显示省略号
		 */
		bool m_autoEllipsis;
		/**
		 * 是否自动调整尺寸
		 */
		bool m_autoSize;
		/**
		 * 背景色
		 */
		Long m_backColor;
		/**
		 * 背景图片
		 */
		String m_backImage;
		/**
		 * 边线的颜色
		 */
		Long m_borderColor;
		/**
		 * 是否可以设置焦点
		 */
		bool m_canFocus;
		/**
		 * 是否可以触发事件
		 */
		bool m_canRaiseEvents;
		/**
		 * 圆角角度
		 */
		int m_cornerRadius;
		/*
		* 鼠标形状
		*/
		FCCursors m_cursor;
		/**
		 * 是否允许偏移显示
		 */
		bool m_displayOffset;
		/**
		 * 绑定边缘类型
		 */
		FCDockStyle m_dock;
		/**
		 * 控件是否可用
		 */
		bool m_enabled;
		/**
		 * 字体
		 */
		FCFont *m_font;
		/**
		 * 是否有右键菜单
		 */
		bool m_hasPopupMenu;
		/**
		 * 或设置是否正被拖动
		 */
		bool m_isDragging;
		/**
		 * 是否为窗体
		 */
		bool m_isWindow;
		/**
		 * 控件的位置
		 */
		FCPoint m_location;
		/**
		 * 外边距
		 */
		FCPadding m_margin;
		/**
		 * 控件的最大尺寸
		 */
		FCSize m_maximumSize;
		/**
		 * 控件的最小尺寸
		 */
		FCSize m_minimumSize;
		/**
		 * 控件的唯一标识名称
		 */
		String m_name;
		/**
		 * 按钮所在的图形接口
		 */
		FCNative *m_native;
		/**
		 * 上次坐标
		 */
		FCPoint m_oldLocation;
		/**
		 * 上次尺寸
		 */
		FCSize m_oldSize;
		/**
		 * 透明度
		 */
		float m_opacity;
		/**
		 * 内边距
		 */
		FCPadding m_padding;
		/**
		 * 父控件
		 */
		FCView *m_parent;
		/**
		 * 百分比位置
		 */
		FCPointF *m_percentLocation;
		/**
		 * 百分比尺寸
		 */
		FCSizeF *m_percentSize;
		/*
		* 区域
		*/
		FCRect m_region;
		/**
		 * 资源路径
		 */
		String m_resourcePath;
		/**
		 * 尺寸
		 */
		FCSize m_size;
		/*
		* Tab索引
		*/
		int m_tabIndex;
		/*
		* 是否支持Tab
		*/
		bool m_tabStop;
		/**
		 * TAG值
		 */
		Object m_tag;
		/**
		 * 文字
		 */
		String m_text;
		/**
		 * 前景色
		 */
		Long m_textColor;
		/**
		 * 是否置顶显示
		 */
		bool m_topMost;
		/**
		 * 是否设置裁剪
		 */
		bool m_useRegion;
		/**
		 * 纵向排列方式
		 */
		FCVerticalAlign m_verticalAlign;
		/**
		 * 控件是否可见
		 */
		bool m_visible;
	protected:
	    /**
		 * 事件集合
		 */
		map<int, vector<Object>*> m_events;
		/*
		* 调用者集合
		*/
		map<int, vector<Object>*> m_invokes;
	    /**
         * 调用事件
         * @param eventID  事件ID
        */
		void callEvents(int eventID);
	    /**
         * 调用控件线程方法事件
         * @param eventID  事件ID
         * @param args     参数
        */
		void callInvokeEvents(int eventID, Object args);
		void callKeyEvents(int eventID, char key);
	    /**
         * 调用键盘事件
         * @param eventID  事件ID
         * @param key      按键
        */
		void callTouchEvents(int eventID, FCTouchInfo touchInfo);
	    /**
         * 调用重绘事件
         * @param eventID  事件ID
         * @param paint    绘图对象
         * @param clipRect 裁剪区域
        */
		void callPaintEvents(int eventID, FCPaint *paint, const FCRect& clipRect);
		/*
		* 调用预键盘事件
		*/
		bool callPreviewsKeyEvent(int eventID, int tEventID, char key);
	    /**
         * 调用重绘事件
         * @param eventID  事件ID
         * @param tEventID 事件ID2
         * @param touchInfo   触摸信息
        */
		bool callPreviewsTouchEvent(int eventID, int tEventID, FCTouchInfo touchInfo);
	    /**
         * 调用秒表事件
         * @param eventID  事件ID
         * @param timerID  秒表编号
        */
		void callTimerEvents(int eventID, int timerID);
	    /**
         * 获取或设置的背景色
         * @returns 背景色
        */
        virtual Long getPaintingBackColor();
        /**
         * 获取要绘制的背景图片
         * @returns 背景图片
        */
        virtual String getPaintingBackImage();
        /**
         * 获取要绘制的边线颜色
         * @returns 边线颜色
        */
        virtual Long getPaintingBorderColor();
        /**
         * 获取要绘制的前景色
         * @returns 前景色
        */
        virtual Long getPaintingTextColor();
	public:
		/*
		* 所有控件
		*/
		ArrayList<FCView*> m_controls;
		/*
		* 构造函数
		*/
		FCView();
		/*
		* 析构函数
		*/
		virtual ~FCView();
		/**
		 * 获取横向排列方式
		 */
		virtual FCHorizontalAlign getAlign();
		/**
		 * 设置横向排列方式
		 */
		virtual void setAlign(FCHorizontalAlign align);
		/**
		 * 获取是否可以拖动位置
		 */
		virtual bool allowDrag();
		/**
		 * 设置是否可以拖动位置
		 */
		virtual void setAllowDrag(bool allowDrag);
		/**
		 * 获取是否允许预处理事件
		 */
		virtual bool allowPreviewsEvent();
		/**
		 * 设置是否允许预处理事件
		 */
		virtual void setAllowPreviewsEvent(bool allowPreviewsEvent);
		/**
		 * 获取锚定信息
		 */
		virtual FCAnchor getAnchor();
		/**
		 * 设置锚定信息
		 */
		virtual void setAnchor(const FCAnchor& anchor);
		/**
		 * 获取是否在文字超出范围时在结尾显示省略号
		 */
		virtual bool autoEllipsis();
		/**
		 * 设置是否在文字超出范围时在结尾显示省略号
		 */
		virtual void setAutoEllipsis(bool autoEllipsis);
		/**
		 * 获取是否自动调整尺寸
		 */
		virtual bool autoSize();
		/**
		 * 设置是否自动调整尺寸
		 */
		virtual void setAutoSize(bool autoSize);
		/**
		 * 获取背景色
		 */
		virtual Long getBackColor();
		/**
		 * 设置背景色
		 */
		virtual void setBackColor(Long backColor);
		/**
		 * 获取背景图片
		 */
		virtual String getBackImage();
		/**
		 * 设置背景图片
		 */
		virtual void setBackImage(const String& backImage);
		/**
		 * 获取边线的颜色
		 */
		virtual Long getBorderColor();
		/**
		 * 设置边线的颜色
		 */
		virtual void setBorderColor(Long borderColor);
		/**
		 * 获取距离下侧的位置
		 */
		virtual int getBottom();
		/**
		 * 获取控件的区域属性
		 */
		virtual FCRect getBounds();
		/**
		 * 设置控件的区域属性
		 */
		virtual void setBounds(const FCRect& rect);
		/**
		 * 获取是否可以设置焦点
		 */
		virtual bool canFocus();
		/**
		 * 设置是否可以设置焦点
		 */
		virtual void setCanFocus(bool canFocus);
		/**
		 * 获取是否可以触发事件
		 */
		virtual bool canRaiseEvents();
		/**
		 * 设置是否可以触发事件
		 */
		virtual void setCanRaiseEvents(bool canRaiseEvents);
		/**
		 * 获取是否被触摸捕获
		 */
		virtual bool isCapture();
		/**
		 * 获取圆角角度
		 */
		virtual int getCornerRadius();
		/**
		 * 设置圆角角度
		 */
		virtual void setCornerRadius(int cornerRadius);
		/*
		* 获取鼠标形状
		*/
		virtual FCCursors getCursor();
		/*
		* 设置鼠标形状
		*/
		virtual void setCursor(FCCursors cursor);
		/**
		 * 获取是否允许偏移显示
		 */
		virtual bool displayOffset();
		/**
		 * 设置是否允许偏移显示
		 */
		virtual void setDisplayOffset(bool displayOffset);
		/**
		 * 获取虚拟显示的区域
		 */
		virtual FCRect getDisplayRect();
		/**
		 * 获取绑定边缘类型
		 */
		virtual FCDockStyle getDock();
		/**
		 * 设置绑定边缘类型
		 */
		virtual void setDock(FCDockStyle dock);
		/**
		 * 获取控件是否可用
		 */
		virtual bool isEnabled();
		/**
		 * 设置控件是否可用
		 */
		virtual void setEnabled(bool enabled);
		/**
		 * 获取是否具有焦点
		 */
		virtual bool isFocused();
		/**
		 * 设置是否具有焦点
		 */
		virtual void setFocused(bool focused);
		/**
		 * 获取字体
		 */
		virtual FCFont* getFont();
		/**
		 * 设置字体
		 */
		virtual void setFont(FCFont *font);
		/**
		 * 获取是否有右键菜单
		 */
		virtual bool hasPopupMenu();
		/**
		 * 设置是否有右键菜单
		 */
		virtual void setHasPopupMenu(bool hasPopupMenu);
		/**
		 * 获取控件的高度
		 */
		virtual int getHeight();
		/**
		 * 设置控件的高度
		 */
		virtual void setHeight(int height);
		/**
		 * 获取或设置是否正被拖动
		 */
		virtual bool isDragging();
		/**
		 * 获取是否为窗体
		 */
		virtual bool isWindow();
		/**
		 * 设置是否为窗体
		 */
		virtual void setWindow(bool isWindow);
		/**
		 * 获取距离左侧的位置
		 */
		virtual int getLeft();
		/**
		 * 设置距离左侧的位置
		 */
		virtual void setLeft(int left);
		/**
		 * 获取控件的位置
		 */
		virtual FCPoint getLocation();
		/**
		 * 设置控件的位置
		 */
		virtual void setLocation(const FCPoint& location);
		/**
		 * 获取外边距
		 */
		virtual FCPadding getMargin();
		/**
		 * 设置外边距
		 */
		virtual void setMargin(const FCPadding& margin);
		/**
		 * 获取控件的最大尺寸
		 */
		virtual FCSize getMaximumSize();
		/**
		 * 设置控件的最大尺寸
		 */
		virtual void setMaximumSize(FCSize maxinumSize);
		/**
		 * 获取控件的最小尺寸
		 */
		virtual FCSize getMinimumSize();
		/**
		 * 设置控件的最小尺寸
		 */
		virtual void setMinimumSize(FCSize minimumSize);
		/*
		* 获取触摸的点
		*/
		virtual FCPoint getTouchPoint();
		/*
		* 获取名称
		*/
		virtual String getName();
		/*
		* 设置名称
		*/
		virtual void setName(const String& name);
		/**
		 * 获取控制器
		 */
		virtual FCNative* getNative();
		/*
		* 设置控制器
		*/
		void setNative(FCNative *native);
		/**
		 * 获取透明度
		 */
		virtual float getOpacity();
		/**
		 * 设置透明度
		 */
		virtual void setOpacity(float opacity);
		/**
		 * 获取内边距
		 */
		virtual FCPadding getPadding();
		/**
		 * 设置内边距
		 */
		virtual void setPadding(const FCPadding& padding);
		/**
		 * 获取父控件
		 */
		virtual FCView* getParent();
		/**
		 * 设置父控件
		 */
		virtual void setParent(FCView *control);
		/**
		 * 获取裁剪矩形
		 */
		virtual FCRect getRegion();
		/**
		 * 设置裁剪矩形
		 */
		virtual void setRegion(const FCRect& region);
		/**
		 * 获取资源路径
		 */
		virtual String getResourcePath();
		/**
		 * 设置资源路径
		 */
		virtual void setResourcePath(const String& resourcePath);
		/**
		 * 获取距离右侧的距离
		 */
		virtual int getRight();
		/**
		 * 获取尺寸
		 */
		virtual FCSize getSize();
		/**
		 * 设置尺寸
		 */
		virtual void setSize(const FCSize& size);
		/*
		* 获取Tab索引
		*/
		virtual int getTabIndex();
		/*
		* 设置Tab索引
		*/
		virtual void setTabIndex(int tabIndex);
		/*
		* 是否支持Tab键
		*/
		virtual bool isTabStop();
		/*
		* 设置是否支持Tab键
		*/
		virtual void setTabStop(bool tabStop);
		/**
		 * 获取TAG值
		 */
		virtual Object getTag();
		/**
		 * 设置TAG值
		 */
		virtual void setTag(Object tag);
		/**
		 * 获取文字
		 */
		virtual String getText();
		/**
		 * 设置文字
		 */
		virtual void setText(const String& text);
		/**
		 * 获取前景色
		 */
		virtual Long getTextColor();
		/**
		 * 设置前景色
		 */
		virtual void setTextColor(Long textColor);
		/**
		 * 获取距离上侧的位置
		 */
		virtual int getTop();
		/**
		 * 设置距离上侧的位置
		 */
		virtual void setTop(int top);
		/**
		 * 获取是否置顶显示
		 */
		virtual bool isTopMost();
		/**
		 * 获取是否置顶显示
		 */
		virtual void setTopMost(bool topMost);
		/**
		 * 获取或设置是否设置裁剪
		 */
		virtual bool useRegion();
		/**
		 * 获取纵向排列方式
		 */
		virtual FCVerticalAlign getVerticalAlign();
		/**
		 * 设置纵向排列方式
		 */
		virtual void setVerticalAlign(FCVerticalAlign verticalAlign);
		/**
		 * 获取控件是否可见
		 */
		virtual bool isVisible();
		/**
		 * 设置控件是否可见
		 */
		virtual void setVisible(bool visible);
		/**
		 * 获取控件的宽度
		 */
		virtual int getWidth();
		/**
		 * 设置控件的宽度
		 */
		virtual void setWidth(int width);
	public:
	    /**
         * 添加控件
         * @param control  控件
        */
		virtual void addControl(FCView *control);
	    /**
         * 在控件线程中调用方法
         * @param args  参数
        */
		virtual void addEvent(Object func, int eventID, Object pInvoke);
	    /**
         * 在控件线程中调用方法
         * @param args  参数
        */
		virtual void beginInvoke(Object args);
	    /**
         * 将子控件置于最前
         * @param childControl  子控件
        */
		virtual void bringChildToFront(FCView *childControl);
		/**
		 * 将控件放到最前显示
		 */
		virtual void bringToFront();
		/**
		 * 清除所有控件
		 */
		virtual void clearControls();
	    /**
         * 是否包含控件
         * @param control   控件
         * @returns  是否包含
        */
		virtual bool containsControl(FCView *control);
	    /**
         * 是否包含控件
         * @param point   坐标
         * @returns  是否包含
        */
		virtual bool containsPoint(const FCPoint& mp);
		/**
		 * 设置焦点
		 */
		virtual void focus();
	    /**
         * 获取控件集合的拷贝
         * @returns 控件集合
        */
		virtual ArrayList<FCView*> getControls();
	    /**
         * 获取控件类型
         * @returns 控件类型
        */
		virtual String getControlType();
	    /**
         * 获取显示偏移坐标
         * @returns 坐标
        */
		virtual FCPoint getDisplayOffset();
	    /**
         * 获取新的秒表编号
         * @returns 新编号
        */
		static int getNewTimerID();
	    /**
         * 获取弹出菜单上下文
         * @param control  当前控件
         * @returns 控件
        */
		virtual FCView* getPopupMenuContext(FCView *control);
	    /**
         * 获取属性值
         * @param name  属性名称
         * @param value 返回属性值
         * @param type  返回属性类型
        */
		virtual void getProperty(const String& name, String *value, String *type);
		/**
		 * 获取属性名称列表
		 */
		virtual ArrayList<String> getPropertyNames();
	    /**
         * 判断是否包含子控件
         * @returns  是否包含子控件
        */
		virtual bool hasChildren();
		/**
		 * 隐藏控件
		 */
		virtual void hide();
	    /**
         * 插入控件
         * @param index  索引
         * @param control  控件
        */
		virtual void insertControl(int index, FCView *control);
		/**
		 * 启动绘制
		 */
		virtual void invalidate();
	    /**
         * 在控件线程中调用方法
         * @param args  参数
        */
		virtual void invoke(Object args);
	    /**
         * 判断是否绘制可用状态
         * @param control   控件
         * @returns  是否绘制可用状态
        */
		bool isPaintEnabled(FCView *control);
	    /**
         * 判断是否绘图时可见
         * @param control   控件
         * @returns  是否可见
        */
		bool isPaintVisible(FCView *control);
		/**
		 * 添加控件方法
		 */
		virtual void onAdd();
		/**
		 * 自动设置尺寸属性改变方法
		 */
		virtual void onAutoSizeChanged();
		/*
		* 背景色变化方法
		*/
		virtual void onBackColorChanged();
		/**
		 * 背景色改变方法
		 */
		virtual void onBackImageChanged();
		/*
		* 输入方法
		*/
		virtual void onChar(wchar_t ch);
		/**
		 * 字符输入
		 */
		virtual void onClick(FCTouchInfo touchInfo);
		/**
		 * 复制
		 */
		virtual void onCopy();
		/**
		 * 剪切
		 */
		virtual void onCut();
		/**
		 * 悬停改变方法
		 */
		virtual void onDockChanged();
	    /**
         * 触摸双击方法
         * @param touchInfo  触摸信息
        */
		virtual void onDoubleClick(FCTouchInfo touchInfo);
	    /**
         *  拖动开始方法
         * @returns  是否拖动
        */
		virtual bool onDragBegin();
		/**
		 * 拖动结束方法
		 */
		virtual void onDragEnd();
		/**
		 * 正在拖动方法
		 */
		virtual void onDragging();
	    /**
         * 拖动准备方法
         * @param startOffset  可以拖动的偏移坐标量
        */
		virtual void onDragReady(FCPoint *startOffset);
		/**
		 * 可用改变方法
		 */
		virtual void onEnableChanged();
		/**
		 * 字体改变方法
		 */
		virtual void onFontChanged();
		/**
		 * 获得焦点方法
		 */
		virtual void onGotFocus();
	    /**
         * 在控件线程中调用方法
         * @param args  参数
        */
		virtual void onInvoke(Object args);
		/**
		 * 控件加载方法
		 */
		virtual void onLoad();
		/**
		 * 位置改变方法
		 */
		virtual void onLocationChanged();
		/**
		 * 丢失焦点方法
		 */
		virtual void onLostFocus();
		/*
		* 键盘按下方法
		*/
		virtual void onKeyDown(char key);
		/*
		* 键盘抬起方法
		*/
		virtual void onKeyUp(char key);
		/**
		 * 外边距改变方法
		 */
		virtual void onMarginChanged();
	    /**
         * 触摸按下方法
         * @param touchInfo  触摸信息
        */
		virtual void onTouchDown(FCTouchInfo touchInfo);
	    /**
         * 触摸进入方法
         * @param touchInfo  触摸信息
        */
		virtual void onTouchEnter(FCTouchInfo touchInfo);
	    /**
         * 触摸离开方法
         * @param touchInfo  触摸信息
        */
		virtual void onTouchLeave(FCTouchInfo touchInfo);
	    /**
         * 触摸移动调用方法
         * @param touchInfo  触摸信息
        */
		virtual void onTouchMove(FCTouchInfo touchInfo);
	    /**
         * 触摸抬起方法
         * @param touchInfo  触摸信息
        */
		virtual void onTouchUp(FCTouchInfo touchInfo);
		/*
		* 触摸滚动方法
		*/
		virtual void onTouchWheel(FCTouchInfo touchInfo);
		/**
		 * 内边距改变方法
		 */
		virtual void onPaddingChanged();
	    /**
         * 重绘方法
         * @param paint  绘图对象
         * @param clipRect   裁剪区域
        */
		virtual void onPaint(FCPaint *paint, const FCRect& clipRect);
	    /**
         * 重绘背景方法
         * @param paint  绘图对象
         * @param clipRect   裁剪区域
        */
		virtual void onPaintBackground(FCPaint *paint, const FCRect& clipRect);
	    /**
         * 重绘边线方法
         * @param paint  绘图对象
         * @param clipRect   裁剪区域
        */
		virtual void onPaintBorder(FCPaint *paint, const FCRect& clipRect);
	    /**
         * 重绘前景方法
         * @param paint  绘图对象
         * @param clipRect   裁剪区域
        */
		virtual void onPaintForeground(FCPaint *paint, const FCRect& clipRect);
		/**
		 * 父容器改变方法
		 */
		virtual void onParentChanged();
		/**
		 * 复制
		 */
		virtual void onPaste();
	    /**
         * 预绘图方法
         * @param paint  绘图对象
         * @param clipRect   裁剪区域
        */
		virtual void onPrePaint(FCPaint *paint, const FCRect& clipRect);
		/*
		* 键盘预处理事件
		*/
		virtual bool onPreviewsKeyEvent(int eventID, char key);
	    /**
         * 键盘下按方法
         * @param eventID  事件ID
         * @param key      按键
         * @returns     状态
        */
		virtual bool onPreviewsTouchEvent(int eventID, FCTouchInfo touchInfo);
		/**
		 * 裁剪区域改变方法
		 */
		virtual void onRegionChanged();
		/**
		 * 移除控件方法
		 */
		virtual void onRemove();
		/**
		 * 尺寸改变方法
		 */
		virtual void onSizeChanged();
		/**
		 * TAB索引改变方法
		 */
		virtual void onTabIndexChanged();
		/**
		 * 被使用TAB切换方法
		 */
		virtual void onTabStop();
		/**
		 * 是否用TAB切换改变方法
		 */
		virtual void onTabStopChanged();
		/**
		 * 文本改变方法
		 */
		virtual void onTextChanged();
		/**
		 * 前景色改变方法
		 */
		virtual void onTextColorChanged();
	    /**
         * 秒表回调方法
         * @param timerID  编号
        */
		virtual void onTimer(int timerID);
		/**
		 * 可见状态改变方法
		 */
		virtual void onVisibleChanged();
		/*
		 * 转为相对坐标
		 */
		virtual FCPoint pointToControl(const FCPoint& mp);
	    /**
         * 获取相对于控件的相对坐标
         * @param point  坐标
         * @returns  相对坐标
        */
		virtual FCPoint pointToNative(const FCPoint& mp);
	    /**
         * 移除控件
         * @param control 控件
        */
		virtual void removeControl(FCView *control);
	    /**
         * 取消注册事件
         * @param func  函数指针
         * @param eventID  事件ID
        */
		virtual void removeEvent(Object func, int eventID);
	    /**
         * 设置属性
         * @param name  属性名称
         * @param value 属性值
        */
		virtual void setProperty(const String& name, const String& value);
		/**
		 * 显示控件
		 */
		virtual void show();
	    /**
         * 开始秒表
         * @param timerID  编号
         * @param interval 间隔
        */
		virtual void startTimer(int timerID, int interval);
	    /**
         * 停止秒表
         * @param timerID  编号
        */
		virtual void stopTimer(int timerID);
	    /**
         * 将控件置于最后
         * @param childControl  子控件
        */
		virtual void sendChildToBack(FCView *childControl);
		/**
		 * 将控件放到最下面显示
		 */
		virtual void sendToBack();
		/**
		 * 更新界面
		 */
		virtual void update();
	};
}
#endif