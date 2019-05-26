/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCHSCROLLBAR_H__
#define __FCHSCROLLBAR_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "FCScrollBar.h"

/*
* 横向滚动条
*/
namespace FaceCat{
	class FCHScrollBar:public FCScrollBar{
	protected:
	    /**
		 * 背景按钮的触摸按下事件
		 */
        FCTouchEvent m_backButtonTouchDownEvent;
        /**
		 * 背景按钮的触摸抬起事件
		 */
		FCTouchEvent m_backButtonTouchUpEvent;
		/**
		* 滚动条背景按钮触摸按下回调事件
		* @params sender  调用者
		* @params touchInfo  触摸信息
		*/
		static void backButtonTouchDown(Object sender, FCTouchInfo touchInfo, Object pInvoke);
		/**
		 * 滚动条背景按钮触摸抬起回调事件
		 */
		static void backButtonTouchUp(Object sender, FCTouchInfo touchInfo, Object pInvoke);
	public:
		/*
		* 构造函数
		*/
		FCHScrollBar();
		/*
		* 析构函数
		*/
		virtual ~FCHScrollBar();
		/**
		 * 获取控件类型
		 */
		virtual String getControlType();
		/**
		 * 滚动条背景按钮触摸按下回调方法
		 */
		void onBackButtonTouchDown(FCTouchInfo touchInfo);
		/**
		 * 滚动条背景按钮触摸抬起方法
		 */
		void onBackButtonTouchUp(FCTouchInfo touchInfo);
		/**
		 * 拖动滚动方法
		 */
		virtual void onDragScroll();
		/**
		 * 添加控件方法
		 */
		virtual void onLoad();
		/**
		 * 重新布局方法
		 */
		virtual void update();
	};
}

#endif