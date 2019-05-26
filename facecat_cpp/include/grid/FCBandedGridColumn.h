/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCBANDEDGRIDCOLUMN_H__
#define __FCBANDEDGRIDCOLUMN_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "FCGridEnums.h"
#include "FCBandedGrid.h"
#include "FCGridColumn.h"
#include "FCGridBand.h"

namespace FaceCat{
	class FCBandedGrid;
	class FCGridColumn;
	class FCGridBand;

	/*
	* 多列头表格列
	*/
	class FCBandedGridColumn : public FCGridColumn{
	protected:
	    /**
		 * 表格带
		 */
		FCGridBand *m_band;
	public:
		/*
		* 构造函数
		*/
		FCBandedGridColumn();
		/*
		* 析构函数
		*/
		virtual ~FCBandedGridColumn();
		/**
		 * 获取表格带
		 */
		virtual FCGridBand* getBand();
		/**
		 * 设置表格带
		 */
		virtual void setBand(FCGridBand *band);
	public:
	    /**
		 * 获取控件类型
		 */
		virtual String getControlType();
		/**
		 * 拖动开始方法
		 */
		virtual bool onDragBegin();
		/**
		* 触摸按下方法
         * @param  touchInfo   触摸信息
		*/
		virtual void onTouchDown(FCTouchInfo touchInfo);
		/**
		* 触摸移动方法
         * @param  touchInfo   触摸信息
		*/
		virtual void onTouchMove(FCTouchInfo touchInfo);
	};
}

#endif