/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCCALENDAR_H__
#define __FCCALENDAR_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "CDay.h"
#include "CYears.h"
#include "CMonth.h"
#include "ArrowButton.h"
#include "DateTitle.h"
#include "DayButton.h"
#include "DayDiv.h"
#include "HeadDiv.h"
#include "MonthButton.h"
#include "MonthDiv.h"
#include "TimeDiv.h"
#include "YearButton.h"
#include "YearDiv.h"

namespace FaceCat{
	class ArrowButton;
	class DateTitle;
	class DayButton;
	class DayDiv;
	class HeadDiv;
	class MonthButton;
	class MonthDiv;
	class TimeDiv;
	class YearButton;
	class YearDiv;

	/*
	* 日历模式
	*/
	enum FCCalendarMode{
		FCCalendarMode_Day, //选择日
		FCCalendarMode_Month, //选择月
		FCCalendarMode_Year //选择年
	};

	/*
	* 日历控件
	*/
	class FCCalendar : public FCView{
	private:
	    /**
		 * 秒表ID
		 */
		int m_timerID;;
	protected:
	    /**
		 * 日期层
		 */
		DayDiv *m_dayDiv;
		/**
		 * 头部层
		 */
		HeadDiv *m_headDiv;
		/**
		 * 日历的模式
		 */
		FCCalendarMode m_mode;
		/**
		 * 当前的月份
		 */
		int m_month;
		/**
		 * 月份层
		 */
		MonthDiv *m_monthDiv;
		/**
		 * 选中日期
		 */
		CDay *m_selectedDay;
		/**
		 * 时间层
		 */
		TimeDiv *m_timeDiv;
		/**
		 * 是否使用动画
		 */
		bool m_useAnimation;
		/**
		 * 年份
		 */
		int m_year;
		/**
		 * 年份层
		 */
		YearDiv *m_yearDiv;
		/**
		 * 日历
		 */
		CYears *m_years;
	public:
		/*
		* 构造函数
		*/
		FCCalendar();
		/*
		* 析构函数
		*/
		virtual ~FCCalendar();
		/**
		 * 获取日期层
		 */
		virtual DayDiv* getDayDiv();
		/**
		 * 设置日期层
		 */
		virtual void setDayDiv(DayDiv *dayDiv);
		/**
		 * 获取头部层
		 */
		virtual HeadDiv* getHeadDiv();
		/**
		 * 设置头部层
		 */
		virtual void setHeadDiv(HeadDiv *headDiv);
		/**
		 * 获取日历的模式
		 */
		virtual FCCalendarMode getMode();
		/**
		 * 设置日历的模式
		 */
		virtual void setMode(FCCalendarMode mode);
		/**
		 * 获取月份
		 */
		virtual CMonth* getMonth();
		/**
		 * 设置月份
		 */
		virtual void setMonth(CMonth *month);
		/**
		 * 获取月份层
		 */
		virtual MonthDiv* getMonthDiv();
		/**
		 * 设置月份层
		 */
		virtual void setMonthDiv(MonthDiv *monthDiv);
		/**
		 * 获取选中日期
		 */
		virtual CDay* getSelectedDay();
		/**
		 * 设置选中日期
		 */
		virtual void setSelectedDay(CDay *day);
		/**
		 * 获取时间层
		 */
		virtual TimeDiv* getTimeDiv();
		/**
		 * 设置时间层
		 */
		virtual void setTimeDiv(TimeDiv *timeDiv);
		/**
		 * 获取是否使用动画
		 */
		virtual bool useAnimation();
		/**
		 * 设置是否使用动画
		 */
		virtual void setUseAnimation(bool useAnimation);
		/**
		 * 获取年份层
		 */
		virtual YearDiv* getYearDiv();
		/**
		 * 设置年份层
		 */
		virtual void setYearDiv(YearDiv *yearDiv);
		/**
		 * 获取日历
		 */
		virtual CYears* getYears();
		/**
		 * 设置日历
		 */
		virtual void setYears(CYears *years);
	public:
		/*
		* 获取星期
		*/
		int dayOfWeek(int y, int m, int d);
		/**
		 * 获取控件类型
		 */
		virtual String getControlType();
		/**
		* 根据年月获取上个月
		* @param  year   年
		* @param  month   月
		*/
		CMonth* getLastMonth(int year, int month);
		/**
		* 获取下个月
		* @param  year   年
		* @param  month   月
		*/
		CMonth* getNextMonth(int year, int month);
		/**
		* 获取属性值
		* @param  name   属性名称
		* @param  value  属性值
		* @param  type   类型
		*/
		virtual void getProperty(const String& name, String *value, String *type);
		/**
		 * 获取属性名称列表
		 */
		virtual ArrayList<String> getPropertyNames();
		/**
		 * 回到上个月
		 */
		void goLastMonth();
		/**
		 * 前往下个月
		 */
		void goNextMonth();
		/**
		* 触摸点击方法
		* @param  touchInfo   触摸信息
		*/
		virtual void onClick(FCTouchInfo touchInfo);
		/**
		 * 控件被添加事件
		 */
		virtual void onLoad();
		/**
		* 键盘方法
		* @param  key  按键
		*/
		virtual void onKeyDown(char key);
		/**
		* 重绘背景方法
		* @param  paint  绘图对象
		* @param  clipRect  裁剪区域
		*/
		virtual void onPaintBackground(FCPaint *paint, const FCRect& clipRect);
		/**
		 * 选中日期更改方法
		 */
		virtual void onSelectedTimeChanged();
		/**
		* 秒表事件
		* @param  timerID  秒表编号
		*/
		virtual void onTimer(int timerID);
		/**
		* 设置属性值
		* @param  name  属性名称
		* @param  value  属性值
		*/
		virtual void setProperty(const String& name, const String& value);
		/**
		 * 重新布局
		 */
		virtual void update();
	};
}
#endif