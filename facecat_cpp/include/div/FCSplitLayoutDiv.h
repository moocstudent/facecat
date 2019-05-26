/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCSPLITLAYOUTDIV_H__
#define __FCSPLITLAYOUTDIV_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\btn\\FCButton.h"
#include "FCDiv.h"
#include "FCLayoutDiv.h"
#include "FCTableLayoutDiv.h"

namespace FaceCat{
	/*
	* 分割层
	*/
	class FCSplitLayoutDiv : public FCLayoutDiv{
	protected:
	    /**
		 * 第一个控件
		 */
		FCView *m_firstControl;
		/**
		 * 第二个控件
		 */
		FCView *m_secondControl;
		/**
		 * 分割模式
		 */
		FCSizeType m_splitMode;
		/**
		 * 分割百分比
		 */
		float m_splitPercent;
		/**
		 * 分割按钮
		 */
		FCButton *m_splitter;
		/*
		* 拖动事件
		*/
		FCEvent m_splitterDraggingEvent;
		/*
		* 拖动事件回调
		*/
		static void splitterDragging(Object sender, Object pInvoke);
	public:
		/*
		* 构造函数
		*/
		FCSplitLayoutDiv();
		/*
		* 析构函数
		*/
		virtual ~FCSplitLayoutDiv();
		/**
		 * 获取第一个控件
		 */
		virtual FCView* getFirstControl();
		/**
		 * 设置第一个控件
		 */
		virtual void setFirstControl(FCView *firstControl);
		/**
		 * 获取第二个控件
		 */
		virtual FCView* getSecondControl();
		/**
		 * 设置第二个控件
		 */
		virtual void setSecondControl(FCView *secondControl);
		/**
		 * 获取分割模式
		 */
		virtual FCSizeType getSplitMode();
		/**
		 * 设置分割模式
		 */
		virtual void setSplitMode(FCSizeType splitMode);
		/**
		 * 获取分割按钮
		 */
		virtual FCButton* getSplitter();
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
		/*
		* 拖动方法
		*/
		void onSplitterDragging();
		/**
		 * 添加控件方法
		 */
		virtual void onLoad();
		/**
		 * 重置布局
		 */
		virtual bool onResetLayout();
		/**
		 * 布局更新方法
		 */
		virtual void update();
		/**
		* 设置属性
		* @param  name   属性名称
		* @param  value   返回属性值
		*/
		virtual void setProperty(const String& name, const String& value);
	};
}

#endif