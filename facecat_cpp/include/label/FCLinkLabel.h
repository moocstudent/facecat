/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCLINKLABEL_H__
#define __FCLINKLABEL_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "FCLabel.h"

namespace FaceCat{
    /**
	 * 超链接控件行为
	 */
	enum FCLinkBehavior{
	    /**
		 * 总是显示下划线
		 */
		LinkBehaviorA_AlwaysUnderLine,
		/**
		 * 触摸悬停时显示下划线
		 */
		LinkBehaviorA_HoverUnderLine,
		/**
		 * 总是不显示下划线
		 */
		LinkBehaviorA_NeverUnderLine
	};

	class FCLabel;

	/*
	* 链接标签
	*/
	class FCLinkLabel : public FCLabel{
	protected:
	    /**
		 * 单击超链接时的颜色
		 */
		Long m_activeLinkColor;
		/**
		 * 超链接被禁用时的颜色
		 */
		Long m_disabledLinkColor;
		/**
		 * 下划线的显示方式
		 */
		FCLinkBehavior m_linkBehavior;
		/**
		 * 超链接处于默认时的颜色
		 */
		Long m_linkColor;
		/**
		 * 是否按照已访问的样式显示超链接颜色
		 */
		bool m_linkVisited;
		/**
		 * 是否已访问
		 */
		bool m_visited;
		/**
		 * 已访问的超链接的颜色
		 */
		Long m_visitedLinkColor;
	protected:
	    /**
		 * 获取要绘制的超链接颜色
		 */
		virtual Long getPaintingLinkColor();
	public:
		/*
		* 构造函数
		*/
		FCLinkLabel();
		/*
		* 析构函数
		*/
		virtual ~FCLinkLabel();
		/**
		 * 获取单击超链接时的颜色
		 */
		virtual Long getActiveLinkColor();
		/**
		 * 设置单击超链接时的颜色
		 */
		virtual void setActiveLinkColor(Long activeLinkColor);
		/**
		 * 获取超链接被禁用时的颜色
		 */
		virtual Long getDisabledLinkColor();
		/**
		 * 设置超链接被禁用时的颜色
		 */
		virtual void setDisabledLinkColor(Long disabledLinkColor);
		/**
		 * 获取下划线的显示方式
		 */
		virtual FCLinkBehavior getLinkBehavior();
		/**
		 * 设置下划线的显示方式
		 */
		virtual void setLinkBehavior(FCLinkBehavior linkBehavior);
		/**
		 * 获取超链接处于默认时的颜色
		 */
		virtual Long getLinkColor();
		/**
		 * 设置超链接处于默认时的颜色
		 */
		virtual void setLinkColor(Long linkColor);
		/**
		 * 获取是否按照已访问的样式显示超链接颜色
		 */
		virtual bool isLinkVisited();
		/**
		 * 设置是否按照已访问的样式显示超链接颜色
		 */
		virtual void setLinkVisited(bool linkVisited);
		/**
		 * 获取的超链接的颜色
		 */
		virtual Long getVisitedLinkColor();
		/**
		 * 访问的超链接的颜色
		 */
		virtual void setVisitedLinkColor(Long visitedLinkColor);
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
		 * 触摸点击方法
		 */
		virtual void onClick(FCTouchInfo touchInfo);
		/**
		 * 触摸按下方法
		 */
		virtual void onTouchDown(FCTouchInfo touchInfo);
		/**
		 * 触摸进入方法
		 */
		virtual void onTouchEnter(FCTouchInfo touchInfo);
		/**
		 * 触摸离开方法
		 */
		virtual void onTouchLeave(FCTouchInfo touchInfo);
		/**
		 * 触摸抬起方法
		 */
		virtual void onTouchUp(FCTouchInfo touchInfo);
		/**
		 * 重绘前景方法
		 */
		virtual void onPaintForeground(FCPaint *paint, const FCRect& clipRect);
		/**
		* 设置属性
		* @param  name  属性名称
		* @param  value  属性值
		*/
		virtual void setProperty(const String& name, const String& value);
	};
}
#endif