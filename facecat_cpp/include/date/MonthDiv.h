/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __MONTHDIV_H__
#define __MONTHDIV_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "MonthButton.h"
#include "HeadDiv.h"
#include "TimeDiv.h"
#include "FCCalendar.h"

namespace FaceCat{
	class MonthButton;
	class HeadDiv;
	class FCCalendar;
	class TimeDiv;

	/*
	* 月的层
	*/
	class MonthDiv{
	protected:
	    /**
		 * 动画的方向
		 */
		int m_am_Direction;
		/**
		 * 动画当前帧数
		 */
		int m_am_Tick;
		/**
		 * 动画总帧数
		 */
		int m_am_TotalTick;
		/**
		 * 日历控件
		 */
		FCCalendar *m_calendar;
		/**
		 * 年份
		 */
		int m_year;
	public:
	    /**
		 * 月的按钮
		 */
		ArrayList<MonthButton*> m_monthButtons;
		/**
		 * 月的动画按钮
		 */
		ArrayList<MonthButton*> m_monthButtons_am;
	public:
		/*
		* 构造函数
		*/
		MonthDiv(FCCalendar *calendar);
		/*
		* 析构函数
		*/
		virtual ~MonthDiv();
		/**
		 * 获取日历控件
		 */
		virtual FCCalendar* getCalendar();
		/**
		 * 设置日历控件
		 */
		virtual void setCalendar(FCCalendar *calendar);
		/**
		 * 获取年份
		 */
		virtual int getYear();
	public:
	    /**
		 * 隐藏
		 */
		virtual void hide();
		/**
		* 触摸点击方法
		* @param  mp   坐标
		* @param  button   按钮
		* @param  clicks   点击次数
		* @param  delta   滚轮数
		*/
		void onClick(FCTouchInfo touchInfo);
		/**
		 * 添加控件方法
		 */
		virtual void onLoad();
		/**
		* 重绘方法
		* @param  paint   绘图对象
		* @param  clipRect   裁剪区域
		*/
		void onPaint(FCPaint *paint, const FCRect& clipRect);
		/**
		* 重置日期图层
		* @param  state   状态
		*/
		void onResetDiv(int state);
		/**
		 * 秒表触发方法
		 */
		virtual void onTimer();
		/**
		* 选择年份
		* @param  year   年份
		*/
		virtual void selectYear(int year);
		/**
		 * 显示
		 */
		virtual void show();
		/**
		 * 更新布局方法
		 */
		void update();
	};
}
#endif