/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCNATIVE_H__
#define __FCNATIVE_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "FCView.h"
#include "FCStr.h"
#include "FCPaint.h"
#include "GdiPaint.h"
#include "GdiPlusPaint.h"
#include "FCHost.h"
#include "WinHost.h"

namespace FaceCat{
	enum SortType{
		SortType_NONE,
		SortType_ASC,
		SortType_DESC
	};

	class FCView;
	class FCHost;
	class WinHost;
	/*
	* 控制器
	*/
	class FCNative{
	protected:
	    /**
		 * 是否允许使用缩放尺寸
		 */
		bool m_allowScaleSize;
		/**
		 * 显示区域
		 */
		FCSize m_displaySize;
		/*
		* 开始绘图的店
		*/
		FCPoint m_drawBeginPoint;
		/**
		 * 拖动开始时的区域
		 */
		FCRect m_dragBeginRect;
		/**
		 * 可以开始拖动的偏移坐标量
		 */
		FCPoint m_dragStartOffset;
		/*
		* 要导出图片的控件
		*/
		FCView *m_exportingControl;
		/**
		 * 控件管理器
		 */
		FCHost *m_host;
		/*
		* 镜像宿主
		*/
		FCNative *m_mirrorHost;
		/*
		* 镜像模式
		*/
		FCMirrorMode m_mirrorMode;
		/**
		 * 透明度
		 */
		float m_opacity;
		/**
		 * 绘图类
		 */
		FCPaint *m_paint;
		/**
		 * 触摸按下时的坐标
		 */
		FCPoint m_touchDownPoint;
		/**
		 * 资源文件的路径
		 */
		String m_resourcePath;
		/**
		 * 旋转角度
		 */
		int m_rotateAngle;
		/**
		 * 使用缩放尺寸
		 */
		FCSize m_scaleSize;
		/*
		* 秒表
		*/
		map<int, FCView*> m_timers;
	    /**
         * 根据触摸位置获取控件
         * @param mp    坐标
         * @param control 控件
         * @returns 控件对象
        */
		FCView* findControl(const FCPoint& mp, ArrayList<FCView*> *controls);
	    /**
         * 根据名称查找控件
         * @param name  名称
         * @param controls  控件集合
         * @returns 控件
        */
		FCView* findControl(const String& name, ArrayList<FCView*> *controls);
	    /**
         * 根据控件查找预处理事件的控件
         * @param control   控件
         * @returns 控件
        */
		FCView* findPreviewsControl(FCView *control);
	    /**
         * 根据控件查找窗体
         * @param control   控件
         * @returns 窗体
        */
		FCView* findWindow(FCView *control);
	    /**
         * 获取绘制的透明度
         * @param control   控件
         * @returns 透明度
        */
		float getPaintingOpacity(FCView *control);
	    /**
         * 获取绘制的资源路径
         * @param control   控件
         * @returns 路径
        */
		String getPaintingResourcePath(FCView *control);
	    /**
         * 获取排序后的控件集合
         * @param parent    父控件
         * @param sortedControls   排序后的控件
         * @returns 状态
        */
		bool getSortedControls(FCView *parent, ArrayList<FCView*> *sortedControls);
		/*
		* 获取允许Tab键的控件
		*/
		void getTabStopControls(FCView *control, ArrayList<FCView*> *tabStopControls);
	    /**
         * 判断是否绘制可用状态
         * @param control   控件
         * @returns 是否绘制可用状态
        */
		bool isPaintEnabled(FCView *control);
	    /**
         * 绘制控件
         * @param   rect 矩形
         * @param   controls 控件集合
         * @param   resourcePath 资源路径
         * @param   opacity 透明度
        */
		void renderControls(const FCRect& rect, ArrayList<FCView*> *controls, String resourcePath, float opacity);
		void setCursor(FCView *control);
	    /**
         * 设置绘图属性
         * @param offsetX  横向偏移
         * @param offsetY  纵向偏移
         * @param clipRect  裁剪区域
         * @param resourcePath  资源路径
         * @param opacity  透明度
        */
		void setPaint(int offsetX, int offsetY, const FCRect& clipRect, String resourcePath, float opacity);
	public:
	    /**
		 * 正被拖动的控件
		 */
		FCView *m_draggingControl;
		/*
		* 正被焦点的控件
		*/
		FCView *m_focusedControl;
		/**
		 * 正被触摸按下的控件
		 */
		FCView *m_touchDownControl;
		/**
		 * 触摸正在其上方移动的控件
		 */
		FCView *m_touchMoveControl;
	public:
	    /**
		 * 控件
		 */
		ArrayList<FCView*> m_controls;
		/*
		* 镜像
		*/
		vector<FCNative*> m_mirrors;
		/*
		* 构造函数
		*/
		FCNative();
		/*
		* 析构函数
		*/
		virtual ~FCNative();
		/**
		 * 获取是否允许使用缩放尺寸
		 */
		bool allowScaleSize();
		/**
		 * 设置是否允许使用缩放尺寸
		 */
		void setAllowScaleSize(bool allowScaleSize);
		/*
		* 获取鼠标的形状
		*/
		FCCursors getCursor();
		/*
		* 设置鼠标的形状
		*/
		void setCursor(FCCursors cursor);
		/**
		 * 获取显示区域
		 */
		FCSize getDisplaySize();
		/**
		 * 设置显示区域
		 */
		void setDisplaySize(FCSize displaySize);
		/**
		 * 获取选中的控件
		 */
		FCView* getFocusedControl();
		/**
		 * 设置选中的控件
		 */
		void setFocusedControl(FCView *focusedControl);
		/**
		 * 获取控件管理器
		 */
		FCHost* getHost();
		/**
		 * 设置控件管理器
		 */
		void setHost(FCHost *host);
		/**
		 * 获取触摸悬停的控件
		 */
		FCView* getHoveredControl();
		/*
		* 获取镜像宿主
		*/
		FCNative* getMirrorHost();
		/*
		* 获取镜像模式
		*/
		FCMirrorMode getMirrorMode();
		/*
		* 设置镜像模式
		*/
		void setMirrorMode(FCMirrorMode mirrorMode);
		/**
		 * 获取触摸的实际位置
		 */
		FCPoint getTouchPoint();
		/**
		 * 获取透明度
		 */
		float getOpacity();
		/**
		 * 设置透明度
		 */
		void setOpacity(float opacity);
		/**
		 * 获绘图类
		 */
		FCPaint* getPaint();
		/**
		 * 取绘图类
		 */
		void setPaint(FCPaint *paint);
		/**
		 * 获取触摸按住的控件
		 */
		FCView* getPushedControl();
		/**
		 * 获取资源文件的路径
		 */
		String getResourcePath();
		/**
		 * 设置资源文件的路径
		 */
		void setResourcePath(const String& resourcePath);
		/**
		 * 获取旋转角度
		 */
		int getRotateAngle();
		/**
		 * 设置旋转角度
		 */
		void setRotateAngle(int rotateAngle);
		/**
		 * 获取使用缩放尺寸
		 */
		FCSize getScaleSize();
		/**
		 * 设置使用缩放尺寸
		 */
		void setScaleSize(FCSize scaleSize);
	public:
	    /**
         * 添加控件
         * @param control 控件
        */
		void addControl(FCView *control);
		/*
		* 添加镜像
		*/
		void addMirror(FCNative *mirrorHost, FCView *control);
	    /**
         * 将控件放到最前显示
         * @param control 控件
        */
		void bringToFront(FCView *control);
		/**
		 * 退出拖动
		 */
		void cancelDragging();
		/**
		 * 清除所有的控件
		 */
		void clearControls();
	    /**
         * 获取控件的绝对横坐标
         * @param control 控件
        */
		int clientX(FCView *control);
	    /**
         * 获取控件的绝对纵坐标
         * @param control 控件
         * @returns 坐标
        */
		int clientY(FCView *control);
	    /**
         * 是否包含控件
         * @param control 控件
         * @returns 是否包含
        */
		bool containsControl(FCView *control);
		/*
		* 导出为图片
		*/
        void exportToImage(const String& exportPath);
		/*
		* 导出为图片
		*/
        void exportToImage(const String& exportPath, FCView *control);
        /**
         * 根据触摸位置获取控件
         * @param mp    坐标
        */
		FCView* findControl(const FCPoint& mp);
	    /**
         * 根据坐标在控件中查找控件
         * @param mp    坐标
         * @param parent    父控件
         * @returns 控件
        */
		FCView* findControl(const FCPoint& mp, FCView *parent);
	    /**
         * 根据名称查找控件
         * @param name  名称
         * @returns 控件
        */
		FCView* findControl(const String& name);
		/**
		 * 获取控件集合的拷贝
		 */
		ArrayList<FCView*> getControls();
	    /**
         * 插入控件
         * @param index 索引
         * @param control   控件
         */
		void insertControl(int index, FCView *control);
		/**
		 * 使用缓存绘制图象，不重新计算绘图结构
		 */
		void invalidate();
	    /**
         * 局部绘图
         * @param control   控件
         */
		void invalidate(FCView *control);
	    /**
         * 局部绘图
         * @param rect  区域
         */
		void invalidate(const FCRect& rect);
		/**
         * 文字输入
         */
		bool onChar(wchar_t key);
	    /**
         * 双击
         */
		void onDoubleClick(FCTouchInfo touchInfo);
		/**
         * 键盘按下
         */
		void onKeyDown(char key);
		/**
         * 键盘抬起
         */
		void onKeyUp(char key);
		/**
         * 触摸按下
         */
		void onTouchDown(FCTouchInfo touchInfo);
		/**
         * 触摸离开
         */
		void onTouchLeave(FCTouchInfo touchInfo);
	    /**
         * 触摸移动事件
         * @param   ouchInfo    触摸信息
        */
		void onTouchMove(FCTouchInfo touchInfo);
		/**
         * 触摸抬起
         */
		void onTouchUp(FCTouchInfo touchInfo);
		/**
         * 触摸滚动
         */
		void onTouchWheel(FCTouchInfo touchInfo);
	    /**
         * 绘图方法
         * @param clipRect  矩形区域
        */
		void onPaint(const FCRect& rect);
		/*
		* 按键预处理
		*/
		bool onPreviewsKeyEvent(int eventID, char key);
		/*
		* 触摸预处理
		*/
		bool onPreviewsTouchEvent(int eventID, FCView *control, FCTouchInfo touchInfo);
		/**
		 * 处理尺寸改变
		 */
		void onResize();
	    /**
         * 处理秒表
         * @param   timerID 秒表ID
        */
		void onTimer(int timerID);
		/**
		 * 移除控件
		 */
		void removeControl(FCView *control);
		/*
		* 移除镜像
		*/
		void removeMirror(FCView *control);
		/**
		 * 将控件放到最下面显示
		 */
		void sendToBack(FCView *control);
		/**
		 * 设置排列
		 */
		void setAlign(ArrayList<FCView*> *controls);
	    /**
         * 设置锚定信息
         * @param controls  控件集合
         * @param oldSize   原尺寸
        */
		void setAnchor(FCRect *bounds, FCSize *parentSize, FCSize *oldSize, bool anchorLeft, bool anchorTop, bool anchorRight, bool anchorBottom);
		/*
		* 设置锚定信息
		*/
		void setAnchor(ArrayList<FCView*> *controls, FCSize oldSize);
	    /**
         * 设置绑定边缘
         * @param control   控件
        */
		void setDock(FCRect *bounds, FCRect *spaceRect, FCSize *cSize, int dock);
		/*
		* 设置绑定边缘
		*/
		void setDock(ArrayList<FCView*> *controls);
	    /**
         * 启动秒表
         * @param control  控件
         * @param timerID  秒表编号
         * @param interval 间隔
        */
		void startTimer(FCView *control, int timerID, int interval);
		/**
		 * 停止秒表
		 */
		void stopTimer(int timerID);
		/**
		 * 更新布局
		 */
		void update();
	};
}
#endif