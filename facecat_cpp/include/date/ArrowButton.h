/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __ARROWBUTTON_H__
#define __ARROWBUTTON_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\btn\\FCButton.h"
#include "FCCalendar.h"
#include "YearDiv.h"

namespace FaceCat{
	class FCCalendar;

	/*
	* 箭头按钮
	*/
	class ArrowButton:public FCButton{
	protected:
	    /**
		 * 日历控件
		 */
		FCCalendar *m_calendar;
		/**
		 * 是否前往上个月
		 */
		bool m_toLast;
	public:
		/*
		* 构造函数
		*/
		ArrowButton(FCCalendar *calendar);
		/*
		* 析构函数
		*/
		virtual ~ArrowButton();
		/**
		 * 获取日历控件
		 */
		virtual FCCalendar* getCalendar();
		/**
		 * 设置日历控件
		 */
		virtual void setCalendar(FCCalendar *calendar);
		/**
		 * 获取是否前往上个月
		 */
		virtual bool isToLast();
		/**
		 * 设置是否前往上个月
		 */
		virtual void setToLast(bool toLast);
	public:
	    /**
		 * 获取控件类型
		 */
		virtual String getControlType();
    	/**
    		* 触摸点击事件
    		* @param  touchInfo   触摸信息
    	*/
		virtual void onClick(FCTouchInfo touchInfo);
    	/**
    		* 重绘背景方法
    		* @param  paint   绘图对象
    		* @param  clipRect   裁剪区域
    	*/
		virtual void onPaintForeground(FCPaint *paint, const FCRect& clipRect);
	};
}
#endif