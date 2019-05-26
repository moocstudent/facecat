/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCTOOLTIP_H__
#define __FCTOOLTIP_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\Label\\FCLabel.h"

namespace FaceCat{
	/*
	* 提示层
	*/
	class FCToolTip : public FCLabel{
	private:
	    /**
		 * 秒表ID
		 */
		int m_timerID;
	protected:
	    /**
		 * 显示的时间长度
		 */
		int m_autoPopDelay;
		/**
		 * 触摸静止时延迟显示的毫秒数
		 */
		int m_initialDelay;
		/**
		 * 上一次触摸的位置
		 */
		FCPoint m_lastTouchPoint;
		/**
		 * 是否总是显示
		 */
		bool m_showAlways;
		/**
		 * 是否在显示隐藏时使用动画
		 */
		bool m_useAnimation;
	protected:
	    /**
		 * 剩余保留显示毫秒数
		 */
		int m_remainAutoPopDelay;
		/**
		 * 剩余延迟显示毫秒数
		 */
		int m_remainInitialDelay;
	public:
		/*
		* 构造函数
		*/
		FCToolTip();
		/*
		* 析构函数
		*/
		virtual ~FCToolTip();
		/**
		 * 提示显示的时间长度
		 */
		virtual int getAutoPopDelay();
		/**
		 * 保留显示的时间长度
		 */
		virtual void setAutoPopDelay(int autoPopDelay);
		/**
		 * 获取触摸静止时延迟显示的毫秒数
		 */
		virtual int getInitialDelay();
		/**
		 * 设置触摸静止时延迟显示的毫秒数
		 */
		virtual void setInitialDelay(int initialDelay);
		/**
		 * 获取是否总是显示
		 */
		virtual bool showAlways();
		/**
		 * 设置是否总是显示
		 */
		virtual void setShowAlways(bool showAlways);
		/**
		 * 获取是否在显示隐藏时使用动画
		 */
		virtual bool useAnimation();
		/**
		 * 设置是否在显示隐藏时使用动画
		 */
		virtual void setUseAnimation(bool useAnimation);
	public:
	    /**
		 * 获取控件类型
		 */
		virtual String getControlType();
		/**
		* 获取属性值
		* @param  name   属性名称
		* @param  value   返回属性值
		* @param  type   返回属性类型
		*/
		virtual void getProperty(const String& name, String *value, String *type);
		/**
		 * 获取属性名称列表
		 */
		virtual ArrayList<String> getPropertyNames();
		/**
		 * 隐藏控件
		 */
		virtual void hide();
		/**
		 * 添加控件方法
		 */
		virtual void onLoad();
		/**
		* 秒表方法
		* @param  timerID   秒表ID
		*/
		virtual void onTimer(int timerID);
		/**
		 * 可见状态改变方法
		 */
		virtual void onVisibleChanged();
		/**
		 * 显示控件
		 */
		virtual void show();
		/**
		* 设置属性值
		* @param  name   属性名称
		* @param  value   属性值
		*/
		virtual void setProperty(const String& name, const String& value);
	};
}

#endif