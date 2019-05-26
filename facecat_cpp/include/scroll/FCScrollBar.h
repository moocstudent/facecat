/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCSCROLLBAR_H__
#define __FCSCROLLBAR_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\btn\\FCButton.h"

namespace FaceCat{
	/*
	* 滚动条
	*/
	class FCScrollBar:public FCView{
	private:
        /**
		 * 秒表ID
		 */
		int m_timerID;
		/**
		 * 秒表计数
		 */
		int m_tick;
	protected:
		FCButton *m_addButton;
		/**
		 * 当前的加速度
		 */
		int m_addSpeed;
		/**
		 * 滚动背景按钮
		 */
		FCButton *m_backButton;
		/**
		 * 内容尺寸
		 */
		int m_contentSize;
		/**
		 * 是否正在增量
		 */
		bool m_isAdding;
		/**
		 * 是否正在减量
		 */
		bool m_isReducing;
		/**
		 * 滚动的行尺寸
		 */
		int m_lineSize;
		/**
		 * 页的尺寸
		 */
		int m_pageSize;
		/**
		 * 滚动距离
		 */
		int m_pos;
		/**
		 * 减量按钮
		 */
		FCButton *m_reduceButton;
		/**
		 * 滚动按钮
		 */
		FCButton *m_scrollButton;
	protected:
	    /**
		 * 增量按钮的按下事件
		 */
		FCTouchEvent m_addButtonTouchDownEvent;
		/**
		 * 增量按钮的抬起事件
		 */
		FCTouchEvent m_addButtonTouchUpEvent;
		/**
		 * 减量按钮的按下事件
		 */
		FCTouchEvent m_reduceButtonTouchDownEvent;
		/**
		 * 减量按钮的抬起事件
		 */
		FCTouchEvent m_reduceButtonTouchUpEvent;
		/**
		 * 滚动按钮拖拽事件
		 */
		FCEvent m_scrollButtonDraggingEvent;
		/**
		* 增量滚动按钮按下事件回调事件
		* @params sender  调用者
		* @params touchInfo  触摸信息
		*/
		static void addButtonTouchDown(Object sender, FCTouchInfo touchInfo, Object pInvoke);
		/**
		 * 增量滚动按钮抬起事件回调事件
		 */
		static void addButtonTouchUp(Object sender, FCTouchInfo touchInfo, Object pInvoke);
		/**
		 * 减量滚动按钮按下事件回调方法
		 */
		static void reduceButtonTouchDown(Object sender, FCTouchInfo touchInfo, Object pInvoke);
		/**
		 * 减量滚动按钮抬起事件回调方法
		 */
		static void reduceButtonTouchUp(Object sender, FCTouchInfo touchInfo, Object pInvoke);
		/**
		 * 滚动按钮的拖动事件回调方法
		 */
		static void scrollButtonDragging(Object sender, Object pInvoke);
	public:
		/*
		* 构造函数
		*/
		FCScrollBar();
		/*
		* 析构函数
		*/
		virtual ~FCScrollBar();
		/**
		 * 增量按钮
		 */
		virtual FCButton* getAddButton();
		/**
		 * 获取当前的加速度
		 */
		virtual int getAddSpeed();
		/**
		 * 设置当前的加速度
		 */
		virtual void setAddSpeed(int addSpeed);
		/**
		 * 获取滚动背景按钮
		 */
		virtual FCButton* getBackButton();
		/**
		 * 获取内容尺寸
		 */
		virtual int getContentSize();
		/**
		 * 设置内容尺寸
		 */
		virtual void setContentSize(int contentWidth);
		/**
		 * 获取是否正在增量
		 */
		virtual bool isAdding();
		/**
		 * 设置是否正在增量
		 */
		virtual void setIsAdding(bool isAdding);
		/**
		 * 获取是否正在减量
		 */
		virtual bool isReducing();
		/**
		 * 设置是否正在减量
		 */
		virtual void setIsReducing(bool isReducing);
		/**
		 * 每次滚动的行尺寸
		 */
		virtual int getLineSize();
		virtual void setLineSize(int lineSize);
		/**
		 * 获取页的尺寸
		 */
		virtual int getPageSize();
		/**
		 * 设置页的尺寸
		 */
		virtual void setPageSize(int pageSize);
		/**
		 * 获取滚动距离
		 */
		virtual int getPos();
		/**
		 * 设置滚动距离
		 */
		virtual void setPos(int pos);
		/**
		 * 减量按钮
		 */
		virtual FCButton* getReduceButton();
		/**
		 * 获取滚动按钮
		 */
		virtual FCButton* getScrollButton();
	public:
	    /**
		 * 获取控件类型
		 */
		virtual String getControlType();
		/**
		 * 行变大方法
		 */
		virtual void lineAdd();
		/**
		 * 行变小方法
		 */
		virtual void lineReduce();
		/**
		* 增量滚动按钮按下事件方法
		* @params touchInfo  触摸信息
		*/
		void onAddButtonTouchDown(FCTouchInfo touchInfo);
		/**
		 * 增量滚动按钮抬起事件方法
		 */
		void onAddButtonTouchUp(FCTouchInfo touchInfo);
		/**
		 * 加速滚动结束
		 */
		virtual void onAddSpeedScrollEnd();
		/**
		 * 自动加速滚动开始
		 */
		virtual void onAddSpeedScrollStart(DWORD startTime, DWORD nowTime, int start, int end);
		/**
		 * 自动加速滚动中
		 */
		virtual int onAddSpeedScrolling();
		/**
		 * 拖动滚动方法
		 */
		virtual void onDragScroll();
		/**
		 * 控件添加方法
		 */
		virtual void onLoad();
		/**
		 * 减量滚动按钮按下事件方法
		 */
		void onReduceButtonTouchDown(FCTouchInfo touchInfo);
		/**
		 * 减量滚动按钮抬起事件回调方法
		 */
		void onReduceButtonTouchUp(FCTouchInfo touchInfo);
		/**
		 * 滚动方法
		 */
		virtual void onScrolled();
		/**
		 * 可见状态改变方法
		 */
		virtual void onVisibleChanged();
		/**
		 * 页变大方法
		 */
		virtual void pageAdd();
		/**
		 * 页变小方法
		 */
		virtual void pageReduce();
		/**
		 * 滚动到开始
		 */
		virtual void scrollToBegin();
		/**
		 * 滚动到结束
		 */
		virtual void scrollToEnd();
		/**
		 * 秒表回调方法
		 */
		virtual void onTimer(int timerID);
	};
}

#endif