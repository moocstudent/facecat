/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __SELECTAREA_H__
#define __SELECTAREA_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "ChartDiv.h"

namespace FaceCat{
	class ChartDiv;

    /**
     * 选中区域
     */
	class SelectArea : public FCProperty{
	protected:
	    /**
	     * 是否允许用户绘图
	     */
		bool m_allowUserPaint;
		/**
	     * 背景色
	     */
		Long m_backColor;
		/**
	     * 选中框的区域
	     */
		FCRect m_bounds;
		/**
	     * 是否可以改变选中框的大小
	     */
		bool m_canResize;
		/**
	     * 是否可以出现选中框
	     */
		bool m_enabled;
		/**
	     * 选中框的颜色
	     */
		Long m_lineColor;
		/**
	     * 是否显示选中框
	     */
		bool m_visible;
	public:
		/*
		* 构造函数
		*/
		SelectArea();
		/*
		* 析构函数
		*/
		virtual ~SelectArea();
		/**
	     * 获取是否允许用户绘图
	     */
		virtual bool allowUserPaint();
		/**
	     * 设置是否允许用户绘图
	     */
		virtual void setAllowUserPaint(bool allowUserPaint);
		/**
	     * 获取背景色
	     */
		virtual Long getBackColor();
		/**
	     * 设置背景色
	     */
		virtual void setBackColor(Long backColor);
		/**
	     * 获取选中框的区域
	     */
		virtual FCRect getBounds();
		/**
	     * 设置选中框的区域
	     */
		virtual void setBounds(FCRect bounds);
		/**
	     * 获取是否可以改变选中框的大小
	     */
		virtual bool canResize();
		/**
	     * 设置是否可以改变选中框的大小
	     */
		virtual void setCanResize(bool canResize);
		/**
	     * 获取是否可以出现选中框
	     */
		virtual bool isEnabled();
		/**
	     * 设置是否可以出现选中框
	     */
		virtual void setEnabled(bool enabled);
		/**
	     * 获取选中框的颜色
	     */
		virtual Long getLineColor();
		/**
	     * 设置选中框的颜色
	     */
		virtual void setLineColor(Long lineColor);
		/**
	     * 获取是否显示选中框
	     */
		virtual bool isVisible();
		/**
	     * 设置是否显示选中框
	     */
		virtual void setVisible(bool visible);
	public:
	    /**
	     * 关闭
	     */
		void close();
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

	};
}
#endif