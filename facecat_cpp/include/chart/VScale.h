/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __VSCALE_H__
#define __VSCALE_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\core\\FCPaint.h"
#include "Enums.h"
#include "FCDataTable.h"
#include "CrossLineTip.h"
#include "ChartDiv.h"

namespace FaceCat{
	class ChartDiv;
	class CrossLineTip;

    /**
     * Y轴
     */
	class VScale : public FCProperty{
	protected:
	    /**
	     * 是否允许用户绘图
	     */
		bool m_allowUserPaint;
		/**
	     * 最大值和最小值是否自动计算
	     */
		bool m_autoMaxMin;
		/**
	     * 基础字段
	     */
		int m_baseField;
		/**
	     * 十字线标签
	     */
		CrossLineTip *m_crossLineTip;
		/**
	     * 面板显示数值保留小数的位数
	     */
		int m_digit;
		/**
	     * 左侧Y轴文字的字体
	     */
		FCFont *m_font;
		/**
	     * 量级
	     */
		int m_magnitude;
		/**
	     * 区别涨贴的中间值
	     */
		double m_midValue;
		/**
	     * 数字类型
	     */
		NumberStyle m_numberStyle;
		/**
	     * 最小值上方的间隙比例
	     */
		int m_paddingBottom;
		/**
	     * 最大值上方的间隙比例
	     */
		int m_paddingTop;
		/**
	     * 是否反转
	     */
		bool m_reverse;
		/**
	     * 坐标轴的画笔
	     */
		Long m_scaleColor;
		/*
		* 坐标刻度
		*/
		ArrayList<double> m_scaleSteps;
		/**
	     * 坐标系的类型
	     */
		VScaleSystem m_system;
		/**
	     * Y轴文字的颜色
	     */
	    Long m_textColor;
		/**
	     * Y轴文字的颜色2
	     */
		Long m_textColor2;
		/**
	     * 坐标轴的类型
	     */
		VScaleType m_type;
		/**
	     * 坐标值可见部分的最大值
	     */
		double m_visibleMax;
		/**
	     * 坐标值可见部分的最小值
	     */
		double m_visibleMin;
	public:
		/*
		* 构造函数
		*/
		VScale();
		/*
		* 析构函数
		*/
		virtual ~VScale();
		/**
	     * 获取是否允许用户绘图
	     */
		virtual bool allowUserPaint();
		/**
	     * 设置是否允许用户绘图
	     */
		virtual void setAllowUserPaint(bool allowUserPaint);
		/**
	     * 获取最大值和最小值是否自动计算
	     */
		virtual bool autoMaxMin();
		/**
	     * 设置最大值和最小值是否自动计算
	     */
		virtual void setAutoMaxMin(bool autoMaxMin);
		/**
	     * 获取基础字段
	     */
		virtual int getBaseField();
		/**
	     * 设置基础字段
	     */
		virtual void setBaseField(int baseField);
		/**
	     * 获取十字线标签
	     */
		virtual CrossLineTip* getCrossLineTip();
		/**
	     * 获取面板显示数值保留小数的位数
	     */
		virtual int getDigit();
		/**
	     * 设置面板显示数值保留小数的位数
	     */
		virtual void setDigit(int digit);
		/**
	     * 获取左侧Y轴文字的字体
	     */
		virtual FCFont* getFont();
		/**
	     * 获取左侧Y轴文字的字体
	     */
		virtual void setFont(FCFont *font);
		/**
	     * 获取量级
	     */
		virtual int getMagnitude();
		/**
	     * 设置量级
	     */
		virtual void setMagnitude(int magnitude);
		/**
	     * 获取区别涨贴的中间值
	     */
		virtual double getMidValue();
		/**
	     * 设置区别涨贴的中间值
	     */
		virtual void setMidValue(double midValue);
		/**
	     * 获取数字类型
	     */
		virtual NumberStyle getNumberStyle();
		/**
	     * 设置数字类型
	     */
		virtual void setNumberStyle(NumberStyle numberStyle);
		/**
	     * 获取最小值上方的间隙比例
	     */
		virtual int getPaddingBottom();
		/**
	     * 设置最小值上方的间隙比例
	     */
		virtual void setPaddingBottom(int paddingBottom);
		/**
	     * 获取最大值上方的间隙比例
	     */
		virtual int getPaddingTop();
		/**
	     * 设置最大值上方的间隙比例
	     */
		virtual void setPaddingTop(int paddingTop);
		/**
	     * 获取是否反转
	     */
		virtual bool isReverse();
		/**
	     * 设置是否反转
	     */
		virtual void setReverse(bool reverse);
		/**
	     * 获取坐标轴的画笔
	     */
		virtual Long getScaleColor();
		/**
	     * 设置坐标轴的画笔
	     */
		virtual void setScaleColor(Long scaleColor);
		/**
	     * 获取坐标系的类型
	     */
		virtual VScaleSystem getSystem();
		/**
	     * 设置坐标系的类型
	     */
		virtual void setSystem(VScaleSystem system);
		/**
	     * 获取Y轴文字的颜色
	     */
	    virtual Long getTextColor();
	    /**
	     * 设置Y轴文字的颜色
	     */
		virtual void setTextColor(Long textColor);
		/**
	     * 获取Y轴文字的颜色2
	     */
		virtual Long getTextColor2();
		/**
	     * 设置Y轴文字的颜色2
	     */
		virtual void setTextColor2(Long textColor2);
		/**
	     * 获取坐标轴的类型
	     */
		virtual VScaleType getType();
		/**
	     * 设置坐标轴的类型
	     */
		virtual void setType(VScaleType type);
		/**
	     * 获取坐标值可见部分的最大值
	     */
		virtual double getVisibleMax();
		/**
	     * 设置坐标值可见部分的最大值
	     */
		virtual void setVisibleMax(double visibleMax);
		/**
	     * 获取坐标值可见部分的最小值
	     */
		virtual double getVisibleMin();
		/**
	     * 设置坐标值可见部分的最小值
	     */
		virtual void setVisibleMin(double visibleMin);
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
	     * 获取属性名称列表
	     */
		ArrayList<double> getScaleSteps();
	    /**
         * 重绘方法
         * @param paint  绘图对象
         * @param div    图层
         * @param rect   区域
        */
		virtual void onPaint(FCPaint *paint, ChartDiv *div, const FCRect& rect);
	    /**
         * 设置属性值
         * @param name  属性名称
         * @param value 属性值
        */
		virtual void setProperty(const String& name, const String& value);
		/**
	     * 设置刻度点
	     */
		void setScaleSteps(ArrayList<double> scaleSteps);
	};
}
#endif