/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCGRIDBAND_H__
#define __FCGRIDBAND_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "FCGridEnums.h"
#include "FCBandedGrid.h"
#include "FCBandedGridColumn.h"
#include "..\\btn\\FCButton.h"

namespace FaceCat{
	class FCBandedGrid;
	class FCBandedGridColumn;

	/*
	* 表格带
	*/
	class FCGridBand : public FCButton{
	protected:
	    /**
		 * 是否可以调整大小
		 */
		bool m_allowResize;
		/**
		 * 表格
		 */
		FCBandedGrid *m_grid;
		/**
		 * 索引
		 */
		int m_index;
		/**
		 * 父表格带
		 */
		FCGridBand *m_parentBand;
	protected:
	    /**
		 * 起始宽度
		 */
		int m_beginWidth;
		/**
		 * 触摸按下时的坐标
		 */
        FCPoint m_touchDownPoint;
        /**
		 * 调整大小状态，1:左侧 2:右侧
		 */
		int m_resizeState;
	public:
	    /**
		 * 子表格带
		 */
		ArrayList<FCGridBand*> m_bands;
		/**
		 * 子表格列
		 */
		ArrayList<FCBandedGridColumn*> m_columns;
		/*
		* 构造函数
		*/
		FCGridBand();
		/*
		* 析构函数
		*/
		virtual ~FCGridBand();
		/**
		 * 获取是否可以调整大小
		 */
		virtual bool allowResize();
		/**
		 * 设置是否可以调整大小
		 */
		virtual void setAllowResize(bool allowResize);
		/**
		 * 获取表格
		 */
		virtual FCBandedGrid* getGrid();
		/**
		 * 设置表格
		 */
		virtual void setGrid(FCBandedGrid *grid);
		/**
		 * 获取索引
		 */
		virtual int getIndex();
		/**
		 * 设置索引
		 */
		virtual void setIndex(int index);
		/**
		 * 获取父表格带
		 */
		virtual FCGridBand* getParentBand();
		/**
		 * 设置父表格带
		 */
		virtual void setParentBand(FCGridBand *parentBand);
	public:
	    /**
		 * 添加表格带
		 */
		void addBand(FCGridBand *band);
		/**
		 * 添加表格列
		 */
		void addColumn(FCBandedGridColumn *column);
		/**
		 * 清除表格带
		 */
		void clearBands();
		/**
		 * 清除列
		 */
		void clearColumns();
		/**
		 * 获取所有的子表格列
		 */
		ArrayList<FCBandedGridColumn*> getAllChildColumns();
		/**
		 * 获取表格带列表
		 */
		ArrayList<FCGridBand*> getBands();
		/**
		 * 获取列头
		 */
		ArrayList<FCBandedGridColumn*> getColumns();
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
		* 插入表格带
		* @param  index   索引
		* @param  band   表格带
		*/
		void insertBand(int index, FCGridBand *band);
		/**
		* 插入表格列
		* @param  index   索引
		* @param  band   表格带
		*/
		void insertColumn(int index, FCBandedGridColumn *column);
		/**
		* 触摸按下方法
         * @param  touchInfo    触摸信息
		*/
		virtual void onTouchDown(FCTouchInfo touchInfo);
		/**
		* 触摸移动方法
         * @param  touchInfo    触摸信息
		*/
		virtual void onTouchMove(FCTouchInfo touchInfo);
		/**
		* 触摸抬起方法
         * @param  touchInfo    触摸信息
		*/
		virtual void onTouchUp(FCTouchInfo touchInfo);
		/**
		 * 移除表格带
		 */
		void removeBand(FCGridBand *band);
		/**
		 * 移除表格列
		 */
		void removeColumn(FCBandedGridColumn *column);
		/**
		 * 重置列头布局
		 */
		virtual void resetHeaderLayout();
		/**
		* 设置属性
		* @param  name  属性名称
		* @param  value  属性值
		*/
		virtual void setProperty(const String& name, const String& value);
	};
}

#endif