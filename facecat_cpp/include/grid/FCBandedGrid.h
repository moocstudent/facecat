/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCBANDEDGRID_H__
#define __FCBANDEDGRID_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "FCGridEnums.h"
#include "FCBandedGridColumn.h"
#include "FCGridBand.h"
#include "FCGrid.h"

namespace FaceCat{
	class FCBandedGridColumn;
	class FCGridBand;

	/*
	* 带状表格
	*/
	class FCBandedGrid : public FCGrid{
	protected:
	    /**
		 * 获取所有可见带的宽度
		 */
		int getAllVisibleBandsWidth();
	public:
	    /**
		 * 表格带
		 */
		ArrayList<FCGridBand*> m_bands;
		/*
		* 构造函数
		*/
		FCBandedGrid();
		/*
		* 析构函数
		*/
		virtual ~FCBandedGrid();
	public:
		/**
		* 添加表格带
		* @param  band  表格带
		*/
		void addBand(FCGridBand *band);
		/**
		* 添加列
		* @param  column  列
		*/
		virtual void addColumn(FCGridColumn *column);
		/**
		 * 清除表格带
		 */
		void clearBands();
		/**
		 * 清除所有的列
		 */
		virtual void clearColumns();
        /**
		 * 获取表格带列表
		 */
		ArrayList<FCGridBand*> getBands();
		/**
		 * 获取内容的宽度
		 */
		virtual int getContentWidth();
		/**
		 * 获取控件类型
		 */
		virtual String getControlType();
		/**
		* 插入表格带
		* @param  index  索引
		* @param  band  表格带
		*/
		void insertBand(int index, FCGridBand *band);
		/**
		 * 设置控件不可见方法
		 */
		virtual void onSetEmptyClipRegion();
		/**
		* 移除表格带
		* @param  band  表格带
		*/
		void removeBand(FCGridBand *band);
		/**
		* 移除列头
		* @param  column  列头
		*/
		virtual void removeColumn(FCGridColumn *column);
		/**
		 * 重置列头布局
		 */
		virtual void resetHeaderLayout();
		/**
		 * 重新布局
		 */
		virtual void update();
	};
}

#endif