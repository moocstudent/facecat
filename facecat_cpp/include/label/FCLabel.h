/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCLABEL_H__
#define __FCLABEL_H__
#pragma once
#include "..\\..\\stdafx.h"

namespace FaceCat{
	/*
	* 标签
	*/
	class FCLabel : public FCView{
	protected:
		FCContentAlignment m_textAlign;
	public:
		/*
		* 构造函数
		*/ 
		FCLabel();
		/*
		* 析构函数
		*/
		virtual ~FCLabel();
		/**
		 * 获取文字的布局方式
		 */
		virtual FCContentAlignment getTextAlign();
		/**
		 * 设置文字的布局方式
		 */
		virtual void setTextAlign(FCContentAlignment textAlign);
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
		 * 重绘前景方法
		 */
		virtual void onPaintForeground(FCPaint *paint, const FCRect& clipRect);
		/*
		* 预绘图方法
		*/
		virtual void onPrePaint(FCPaint *paint, const FCRect& clipRect);
		/**
		* 设置属性
		* @param  name  属性名称
		* @param  value  属性值
		*/
		virtual void setProperty(const String& name, const String& value);
	};
}
#endif