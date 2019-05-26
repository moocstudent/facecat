/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __HSCALE_H__
#define __HSCALE_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\core\\FCPaint.h"
#include "Enums.h"
#include "CrossLineTip.h"
#include "ChartDiv.h"

namespace FaceCat{
	class ChartDiv;
	class CrossLineTip;

	/*
	* 横轴
	*/
	class HScale : public FCProperty{
	protected:
	    /**
	     * 是否允许用户绘图
	     */
		bool m_allowUserPaint;
		/**
	     * 十字线标签
	     */
		CrossLineTip *m_crossLineTip;
		/**
	     * 日期文字的颜色
	     */
		HashMap<int, Long> m_dateColors;
		/**
	     * X轴文字的字体
	     */
		FCFont *m_font;
		/**
	     * X轴的高度
	     */
		int m_height;
		/**
	     * 横轴的数据类型
	     */
		HScaleType m_hScaleType;
		/**
	     * 日期文字间隔
	     */
		int m_interval;
		/**
	     * X轴的线条颜色
	     */
		Long m_scaleColor;
		/**
	     * 刻度点
	     */
		ArrayList<double> m_scaleSteps;
		/**
	     * 文本颜色
	     */
		Long m_textColor;
		/**
	     * 显示X轴
	     */
		bool m_visible;
	public:
		HScale();
		virtual ~HScale();
		/**
	     * 获取是否允许用户绘图
	     */
		virtual bool allowUserPaint();
		/**
	     * 设置是否允许用户绘图
	     */
		virtual void setAllowUserPaint(bool allowUserPaint);
		/**
	     * 获取十字线标签
	     */
		virtual CrossLineTip* getCrossLineTip();
		/**
	     *  获取日期文字的颜色
	     */
		virtual Long getDateColor(DateType dateType);
		/**
	     * 设置日期文字的颜色
	     */
		virtual void setDateColor(DateType dateType, Long color);
		/**
	     * 获取X轴文字的字体
	     */
		virtual FCFont* getFont();
		/**
	     * 设置X轴文字的字体
	     */
		virtual void setFont(FCFont *font);
		/**
	     * 获取X轴的高度
	     */
		virtual int getHeight();
		/**
	     * 设置X轴的高度
	     */
		virtual void setHeight(int height);
		/**
	     * 获取横轴的数据类型
	     */
		virtual HScaleType getHScaleType();
		/**
	     * 设置横轴的数据类型
	     */
		virtual void setHScaleType(HScaleType hScaleType);
		/**
	     * 获取日期文字间隔
	     */
		virtual int getInterval();
		/**
	     * 设置日期文字间隔
	     */
		virtual void setInterval(int interval);
		/**
	     * 获取X轴的线条颜色
	     */
		virtual Long getScaleColor();
		/**
	     * 设置X轴的线条颜色
	     */
		virtual void setScaleColor(Long scaleColor);
		/**
	     * 获取文本颜色
	     */
		virtual Long getTextColor();
		/**
	     * 设置文本颜色
	     */
		virtual void setTextColor(Long textColor);
		/**
	     * 获取显示X轴
	     */
		virtual bool isVisible();
		/**
	     * 设置显示X轴
	     */
		virtual void setVisible(bool visible);
	public:
	     /**
          * 获取属性值
          * @param name  属性名称
          * @param value 返回属性值
          * @param type  返回属性类型
         */
		virtual void getProperty(const String& name, String *value, String *type);
		/**
	     * 获取属性名称列表
	     */
		virtual ArrayList<String> getPropertyNames();
		/**
	     * 获取刻度点
	     */
		ArrayList<double> getScaleSteps();
		/**
	     * 重绘方法
	     */
		virtual void onPaint(FCPaint *paint, ChartDiv *div, const FCRect& rect);
		/**
	     * 设置属性
	     */
		virtual void setProperty(const String& name, const String& value);
		/**
	     * 设置刻度点
	     */
		void setScaleSteps(ArrayList<double> scaleSteps);
	};
}
#endif