/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCGRIDROW_H__
#define __FCGRIDROW_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\core\\FCProperty.h"
#include "FCGridCell.h"
#include "FCGrid.h"
#include "FCGridColumn.h"

namespace FaceCat{
	class FCGridCell;
	class FCGrid;
	class FCGridColumn;

	/*
	* 表格行的央视
	*/
	class FCGridRowStyle{
	protected:
	    /**
		 * 表格行的样式
		 */
		Long m_backColor;
		/**
		 * 字体
		 */
		FCFont *m_font;
		/**
		 * 触摸悬停行的背景色
		 */
		Long m_hoveredBackColor;
		/**
		 * 触摸悬停行的前景色
		 */
		Long m_hoveredTextColor;
		/**
		 * 选中行的背景色
		 */
		Long m_selectedBackColor;
		/**
		 * 选中行的前景色
		 */
		Long m_selectedTextColor;
		/**
		 * 前景色
		 */
		Long m_textColor;
	public:
		/*
		* 构造函数
		*/
		FCGridRowStyle();
		/*
		* 析构函数
		*/
		virtual ~FCGridRowStyle();
		/*
		* 获取背景色
		*/
		virtual Long getBackColor();
		/*
		* 设置背景色
		*/
		virtual void setBackColor(Long backColor);
		/**
		 * 获取字体
		 */
		virtual FCFont* getFont();
		/**
		 * 设置字体
		 */
		virtual void setFont(FCFont *font);
		/**
		 * 获取前景色
		 */
		virtual Long getTextColor();
		/**
		 * 设置前景色
		 */
		virtual void setTextColor(Long textColor);
		/**
		 * 获取触摸悬停行的背景色
		 */
		virtual Long getHoveredBackColor();
		/**
		 * 设置触摸悬停行的背景色
		 */
		virtual void setHoveredBackColor(Long hoveredBackColor);
		/**
		 * 获取触摸悬停行的前景色
		 */
		virtual Long getHoveredTextColor();
		/**
		 * 设置触摸悬停行的前景色
		 */
		virtual void setHoveredTextColor(Long hoveredTextColor);
		/**
		 * 获取选中行的背景色
		 */
		virtual Long getSelectedBackColor();
		/**
		 * 设置选中行的背景色
		 */
		virtual void setSelectedBackColor(Long selectedBackColor);
		/**
		 * 获取选中行的前景色
		 */
		virtual Long getSelectedTextColor();
		/**
		 * 设置选中行的前景色
		 */
		virtual void setSelectedTextColor(Long selectedTextColor);
	public:
		void copy(FCGridRowStyle *style);
	};

	/*
	* 表格行
	*/
	class FCGridRow : public FCProperty{
	protected:
	    /**
		 * 是否允许编辑
		 */
		bool m_allowEdit;
		/**
		 * 显示区域
		 */
		FCRect m_bounds;
		/**
		 * 编辑按钮
		 */
		FCView *m_editButton;
		/**
		 * 所在表格
		 */
		FCGrid *m_grid;
		int m_height;
		/**
		 * 横向滚动距离
		 */
		int m_horizontalOffset;
		/**
		 * 索引
		 */
		int m_index;
		/**
		 * TAG值
		 */
		Object m_tag;
		/**
		 * 是否可见
		 */
		bool m_visible;
		/**
		 * 可见索引
		 */
		int m_visibleIndex;
	public:
		/*
		* 单元格
		*/
		ArrayList<FCGridCell*> m_cells;
		/*
		* 编辑状态
		*/
		int m_editState;
		/*
		* 构造函数
		*/
		FCGridRow();
		/*
		* 析构函数
		*/
		virtual ~FCGridRow();
		/**
		 * 获取是否允许编辑
		 */
		virtual bool allowEdit();
		/**
		 * 设置是否允许编辑
		 */
		virtual void setAllowEdit(bool allowEdit);
		/**
		 * 获取显示区域
		 */
		virtual FCRect getBounds();
		/**
		 * 设置显示区域
		 */
		virtual void setBounds(FCRect bounds);
		/**
		 * 获取编辑按钮
		 */
		virtual FCView* getEditButton();
		/**
		 * 设置编辑按钮
		 */
		virtual void setEditButton(FCView *editButton);
		/**
		 * 获取所在表格
		 */
		virtual FCGrid* getGrid();
		/**
		 * 设置所在表格
		 */
		virtual void setGrid(FCGrid *grid);
		/**
		 * 获取行高
		 */
		virtual int getHeight();
		/**
		 * 设置行高
		 */
		virtual void setHeight(int height);
		/**
		 * 获取横向滚动距离
		 */
		virtual int getHorizontalOffset();
		/**
		 * 设置横向滚动距离
		 */
		virtual void setHorizontalOffset(int horizontalOffset);
		/**
		 * 获取索引
		 */
		virtual int getIndex();
		/**
		 * 设置索引
		 */
		virtual void setIndex(int index);
		/**
		 * 获取TAG值
		 */
		virtual Object getTag();
		/**
		 * 设置TAG值
		 */
		virtual void setTag(Object tag);
		/**
		 * 获取是否可见
		 */
		virtual bool isVisible();
		/**
		 * 设置是否可见
		 */
		virtual void setVisible(bool visible);
		/**
		 * 获取可见索引
		 */
		virtual int getVisibleIndex();
		/**
		 * 设置可见索引
		 */
		virtual void setVisibleIndex(int visibleIndex);
	public:
	    /**
		 * 添加单元格
		 */
		void addCell(FCGridColumn *column, FCGridCell *cell);
		/**
		 * 添加单元格
		 */
		void addCell(int columnIndex, FCGridCell *cell);
		/**
		 * 添加单元格
		 */
		void addCell(const String& columnName, FCGridCell *cell);
		/**
		 * 清除单元格
		 */
		void clearCells();
		/**
		 * 获取所有单元格
		 */
		ArrayList<FCGridCell*> getCells();
		/**
		 * 根据列的名称获取单元格
		 */
		FCGridCell* getCell(FCGridColumn *column);
		/**
		 * 根据列的索引获取单元格
		 */
		FCGridCell* getCell(int columnIndex);
		/**
		 * 根据列的名称获取单元格
		 */
		FCGridCell* getCell(const String& columnName);
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
		 * 添加行方法
		 */
		virtual void onAdd();
		/**
		 * 重绘方法
		 */
		virtual void onPaint(FCPaint *paint, const FCRect& clipRect, bool isAlternate);
		/**
		 * 重绘边线方法
		 */
		virtual void onPaintBorder(FCPaint *paint, const FCRect& clipRect, bool isAlternate);
		/**
		 * 移除行方法
		 */
		virtual void onRemove();
		/**
		 * 移除单元格
		 */
		void removeCell(FCGridColumn *column);
		/**
		 * 移除单元格
		 */
		void removeCell(int columnIndex);
		/**
		 * 移除单元格
		 */
		void removeCell(const String& columnName);
		/**
		* 设置属性
		* @param  name  属性名称
		* @param  value  属性值
		*/
		virtual void setProperty(const String& name, const String& value);
	};
}

#endif