/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __CROSSLINETIP_H__
#define __CROSSLINETIP_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\core\\FCPaint.h"
#include "ChartDiv.h"

namespace FaceCat{
	class ChartDiv;

	/*
	* 十字线提示框
	*/
	class CrossLineTip{
	protected:
	    /**
	     * 是否允许用户绘图
	     */
		bool m_allowUserPaint;
		/**
	     * X轴提示框背景色
	     */
		Long m_backColor;
		/**
	     * X轴提示框文字的字体
	     */
		FCFont *m_font;
		/**
	     * X轴提示框文字色
	     */
		Long m_textColor;
		/**
	     * 是否可见
	     */
		bool m_visible;
	public:
		CrossLineTip();
		virtual ~CrossLineTip();
		/**
	     * 获取是否允许用户绘图
	     */
		virtual bool allowUserPaint();
		/**
	     * 设置是否允许用户绘图
	     */
		virtual void setAllowUserPaint(bool allowUserPaint);
		/**
	     * 获取X轴提示框背景色
	     */
		virtual Long getBackColor();
		/**
	     * 设置X轴提示框背景色
	     */
		virtual void setBackColor(Long backColor);
		/**
	     * 获取X轴提示框文字的字体
	     */
		virtual FCFont* getFont();
		/**
	     * 设置X轴提示框文字的字体
	     */
		virtual void setFont(FCFont *font);
		/**
	     * 获取X轴提示框文字色
	     */
		virtual Long getTextColor();
		/**
	     * 设置X轴提示框文字色
	     */
		virtual void setTextColor(Long textColor);
		/**
	     * 获取是否可见
	     */
		virtual bool isVisible();
		/**
	     * 设置是否可见
	     */
		virtual void setVisible(bool visible);
	public:
	     /**
          * 获取属性值
          * @param name  属性名称
          * @param value 返回属性值
          * @param type  返回属性类型
         */
		virtual void onPaint(FCPaint *paint, ChartDiv *div, const FCRect& rect);
		/**
	     * 获取属性名称列表
	     */
		virtual void getProperty(const String& name, String *value, String *type);
	    /**
         * 重绘方法
         * @param paint  绘图对象
         * @param div    图层
         * @param rect   区域
        */
		virtual ArrayList<String> getPropertyNames();
	    /**
         * 设置属性值
         * @param name  属性名称
         * @param value 属性值
        */
		virtual void setProperty(const String& name, const String& value);
	};
}
#endif