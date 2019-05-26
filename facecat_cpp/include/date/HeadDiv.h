/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __HEADDIV_H__
#define __HEADDIV_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "CDay.h"
#include "FCCalendar.h"
#include "DateTitle.h"

namespace FaceCat{
	class ArrowButton;
	class DateTitle;

	/*
	* 头部层
	*/
	class HeadDiv:public FCView{
	protected:
	    /**
		 * 日历
		 */
		FCCalendar *m_calendar;
		/**
		 * 日期标题
		 */
		DateTitle *m_dateTitle;
		/**
		 * 上个周期的按钮
		 */
		ArrowButton *m_lastBtn;
		/**
		 * 下个周期的按钮
		 */
		ArrowButton *m_nextBtn;
		/*
		* 日期名称
		*/
		String m_weekStrings[7];
	public:
		/*
		* 构造函数
		*/
		HeadDiv(FCCalendar *calendar);
		/*
		* 析构函数
		*/
		virtual ~HeadDiv();
		/**
		 * 获取日历
		 */
		virtual FCCalendar* getCalendar();
		/**
		 * 设置日历
		 */
		virtual void setCalendar(FCCalendar *calendar);
		/**
		 * 获取日期标题
		 */
		virtual DateTitle* getDateTitle();
		/**
		 * 设置日期标题
		 */
		virtual void setDateTitle(DateTitle *dateTitle);
		/**
		 * 获取到上个周期的按钮
		 */
		virtual ArrowButton* getLastBtn();
		/**
		 * 设置到上个周期的按钮
		 */
		virtual void setLastBtn(ArrowButton *lastBtn);
		/**
		 * 获取到下个周期的按钮
		 */
		virtual ArrowButton* getNextBtn();
		/**
		 * 设置到下个周期的按钮
		 */
		virtual void setNextBtn(ArrowButton *nextBtn);
	public:
	    /**
		 * 获取控件类型
		 */
		virtual String getControlType();
		/**
		 * 添加控件方法
		 */
		virtual void onLoad();
		/**
		* 重绘背景方法
		* @param  paint   绘图对象
		* @param  clipRect   裁剪区域
		*/
		virtual void onPaintBackground(FCPaint *paint, const FCRect& clipRect);
		/**
		* 重绘边线方法
		* @param  paint   绘图对象
		* @param  clipRect   裁剪区域
		*/
		virtual void onPaintBorder(FCPaint *paint, const FCRect& clipRect);
		/**
		* 重绘前景方法
		* @param  paint   绘图对象
		* @param  clipRect   裁剪区域
		*/
		virtual void onPaintForeground(FCPaint *paint, const FCRect& clipRect);
		/**
		 * 布局更新方法
		 */
		virtual void update();
	};
}

#endif