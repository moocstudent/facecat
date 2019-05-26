/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCDATETIMEPICKER_H__
#define __FCDATETIMEPICKER_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "FCTextBox.h"
#include "..\\btn\\FCButton.h"
#include "..\\div\\FCMenu.h"
#include "..\\div\\FCMenuItem.h"
#include "..\\date\\FCCalendar.h"

namespace FaceCat{
	/*
	* 日历选择控件
	*/
	class FCDateTimePicker : public FCTextBox{
	protected:
	    /**
		 * 获取日历
		 */
		FCCalendar *m_calendar;
		/**
		 * 日期格式
		 */
		String m_customFormat;
		/**
		 * 下拉按钮
		 */
		FCButton *m_dropDownButton;
		/**
		 * 下拉按钮点击函数指针
		 */
		FCTouchEvent m_dropDownButtonTouchDownEvent;
		/**
		 * 下拉菜单
		 */
		FCMenu *m_dropDownMenu;
		/**
		 * 选中日期改变函数指针
		 */
		FCEvent m_selectedTimeChangedEvent;
		/**
		 * 是否显示时间
		 */
		bool m_showTime;
		/**
		 * 下拉按钮点击函数指针
		 */
		static void dropDownButtonTouchDown(Object sender, FCTouchInfo touchInfo, Object pInvoke);
		/**
		 * 选中日期改变函数指针
		 */
		static void selectedTimeChanged(Object sender, Object pInvoke);
	public:
		/*
		* 构造函数
		*/
		FCDateTimePicker();
		/*
		* 析构函数
		*/
		virtual ~FCDateTimePicker();
		/**
		 * 获取日历
		 */
		virtual FCCalendar* getCalendar();
		/**
		 * 获取日期格式
		 */
		virtual String getCustomFormat();
		/**
		 * 设置日期格式
		 */
		virtual void setCustomFormat(const String& customFormat);
		/**
		 * 获取下拉按钮
		 */
		virtual FCButton* getDropDownButton();
		/**
		 * 获取下拉菜单
		 */
		virtual FCMenu* getDropDownMenu();
		/**
		 * 获取是否显示时间
		 */
		virtual bool showTime();
		/**
		 * 设置是否显示时间
		 */
		virtual void setShowTime(bool showTime);
	public:
	    /**
		 * 获取控件类型
		 */
		virtual String getControlType();
		/**
		* 获取属性值
		* @param  name  属性名称
		* @param  value  返回属性值
		* @param  type  返回属性类型
		*/
		virtual void getProperty(const String& name, String *value, String *type);
		/**
		 * 获取属性名称列表
		 */
		virtual ArrayList<String> getPropertyNames();
		/**
		 * 下拉菜单显示方法
		 */
		virtual void onDropDownOpening();
		/**
		 * 添加控件方法
		 */
		virtual void onLoad();
		/**
		 * 数值改变方法
		 */
		virtual void onSelectedTimeChanged();
		/**
		* 设置属性
		* @param  name  属性名称
		* @param  value  属性值
		*/
		virtual void setProperty(const String& name, const String& value);
		/**
		 * 更新布局方法
		 */
		virtual void update();
	};
}

#endif