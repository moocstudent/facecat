/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */


#ifndef __CHARTDIV_H__
#define __CHARTDIV_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\core\\FCPaint.h"
#include "Enums.h"
#include "CrossLine.h"
#include "ScaleGrid.h"
#include "FCChart.h"
#include "VScale.h"
#include "SelectArea.h"
#include "ChartToolTip.h"
#include "ChartTitleBar.h"
#include "HScale.h"
#include "FCPlot.h"
#include "BaseShape.h"

namespace FaceCat{
	class FCChart;
	class CrossLine;
	class ChartToolTip;
	class FCPlot;
	class BaseShape;
	class HScale;
	class VScale;
	class FCNative;
	class ChartTitleBar;
	class SelectArea;
	class ScaleGrid;
	/*
	* 图层，用来承载坐标轴，图形，画线工具，指标等内容。
	*/
	class ChartDiv : public FCProperty{
	protected:
		/*
		* 是否允许用户绘图
		*/
		bool m_allowUserPaint;
		/*
		* 背景色
		*/
		Long m_backColor;
		/*
		* 边线色
		*/
		Long m_borderColor;
		/*
		* K线图
		*/
		FCChart *m_chart;
		/*
		* 十字线
		*/
		CrossLine *m_crossLine;
		/*
		* 字体
		*/
		FCFont *m_font;
		/*
		* 横向网格线
		*/
		ScaleGrid *m_hGrid;
		/*
		* 横轴
		*/
		HScale *m_hScale;
		/*
		* 左侧的纵轴
		*/
		VScale *m_leftVScale;
		/*
		* 位置
		*/
		FCPoint m_location;
		/*
		* 画线工具
		*/
		ArrayList<FCPlot*> m_plots;
		/*
		* 右侧的纵轴
		*/
		VScale *m_rightVScale;
		/*
		* 选中区域
		*/
		SelectArea *m_selectArea;
		/*
		* 是否选中
		*/
		bool m_selected;
		/*
		* 图形
		*/
		ArrayList<BaseShape*> m_shapes;
		/*
		* 显示选中
		*/
		bool m_showSelect;
		/*
		* 大小
		*/
		FCSize m_size;
		/*
		* 标题栏
		*/
		ChartTitleBar *m_title;
		/*
		* 提示标签
		*/
		ChartToolTip *m_toolTip;
		/*
		* 纵向百分比
		*/
		int m_verticalPercent;
		/*
		* 纵向网格线
		*/
		ScaleGrid *m_vGrid;
		/*
		* 工作区的高度
		*/
		int m_workingAreaHeight;
	public:
		/*
		* 图形升序
		*/
		static bool shapesAsc(BaseShape *x, BaseShape *y);
		/*
		* 图形降序
		*/
		static bool shapesDesc(BaseShape *x, BaseShape *y);
		/*
		* 画线工具升序
		*/
		static bool plotsAsc(FCPlot *x, FCPlot *y);
		/*
		* 画线工具降序
		*/
		static bool plotsDesc(FCPlot *x, FCPlot *y);
	public:
		/*
		* 创建图层
		*/
		ChartDiv();
		/*
		* 析构函数
		*/
		virtual ~ChartDiv();
		/*
		* 获取是否允许用户绘图
		*/
		virtual bool allowUserPaint();
		/*
		* 设置是否允许用户绘图
		*/
		virtual void setAllowUserPaint(bool allowUserPaint);
		/*
		* 获取背景色
		*/
		virtual Long getBackColor();
		/*
		* 设置背景色
		*/
		virtual void setBackColor(Long backColor);
		/*
		* 获取边线色
		*/
		virtual Long getBorderColor();
		/*
		* 设置边线色
		*/
		virtual void setBorderColor(Long borderColor);
		/*
		* 获取距离下侧的位置
		*/
		virtual int getBottom();
		/*
		* 获取层的区域
		*/
		virtual FCRect getBounds();
		/*
		* 设置层的区域
		*/
		virtual void setBounds(FCRect bounds);
		/*
		* 获取十字线
		*/
		virtual CrossLine* getCrossLine();
		/*
		* 设置十字线
		*/
		virtual void setCrossLine(CrossLine *crossLine);
		/*
		* 获取字体
		*/
		virtual FCFont* getFont();
		/*
		* 设置字体
		*/
		virtual void setFont(FCFont *font);
		/*
		* 获取高度
		*/
		virtual int getHeight();
		/*
		* 获取横向网格线
		*/
		virtual ScaleGrid* getHGrid();
		/*
		* 设置横向网格线
		*/
		virtual void setHGrid(ScaleGrid *hGrid);
		/*
		* 获取横轴
		*/
		virtual HScale* getHScale();
		/*
		* 设置横轴
		*/
		virtual void setHScale(HScale *hScale);
		/*
		* 获取K线图
		*/
		virtual FCChart* getChart();
		/*
		* 设置K线图
		*/
		virtual void setChart(FCChart *chart);
		/*
		* 获取左侧的距离
		*/
		virtual int getLeft();
		/*
		* 获取左侧的纵轴
		*/
		virtual VScale* getLeftVScale();
		/*
		* 设置左侧的纵轴
		*/
		virtual void setLeftVScale(VScale *leftVScale);
		/*
		* 获取位置
		*/
		virtual FCPoint getLocation();
		/*
		* 获取距离右侧的距离
		*/
		virtual int getRight();
		/*
		* 获取右侧的纵轴
		*/
		virtual VScale* getRightVScale();
		/*
		* 设置右侧的纵轴
		*/
		virtual void setRightVScale(VScale *rightVScale);
		/*
		* 获取选中区域
		*/
		virtual SelectArea* getSelectArea();
		/*
		* 设置选中区域
		*/
		virtual void setSelectArea(SelectArea *selectArea);
		/*
		* 获取是否选中
		*/
		virtual bool isSelected();
		/*
		* 设置是否选中
		*/
		virtual void setSelected(bool selected);
		/*
		* 获取是否要显示选中
		*/
		virtual bool isShowSelect();
		/*
		* 设置是否要显示选中
		*/
		virtual void setShowSelect(bool showSelect);
		/*
		* 获取标题栏
		*/
		virtual ChartTitleBar* getTitleBar();
		/*
		* 设置标题栏
		*/
		virtual void setTitleBar(ChartTitleBar *title);
		/*
		* 获取提示标签
		*/
		virtual ChartToolTip* getToolTip();
		/*
		* 设置提示标签
		*/
		virtual void setToolTip(ChartToolTip *toolTip);
		/*
		* 获取距离顶部的距离
		*/
		virtual int getTop();
		/*
		* 获取纵向百分比
		*/
		virtual int getVerticalPercent();
		/*
		* 设置纵向百分比
		*/
		virtual void setVerticalPercent(int verticalPercent);
		/*
		* 获取纵向网格线
		*/
		virtual ScaleGrid* getVGrid();
		/*
		* 设置纵向网格线
		*/
		virtual void setVGrid(ScaleGrid *vGrid);
		/*
		* 获取宽度
		*/
		virtual int getWidth();
		/*
		* 获取工作区的高度
		*/
		virtual int getWorkingAreaHeight();
		/*
		* 设置工作区的高度
		*/
		virtual void setWorkingAreaHeight(int workingAreaHeight);
	public:
		/**
         * 添加画线
         * @param plot 画线
        */
		void addPlot(FCPlot *plot);
		/**
         * 添加图形
         * @param shape 图形
        */
		void addShape(BaseShape *shape);
		/**
         * 是否包含图形
         * @param shape 图形
        */
		bool containsShape(BaseShape *shape);
		/**
         * 获取画线的集合
         * @param sortType 排序类型
        */
		ArrayList<FCPlot*> getPlots(SortType sortType);
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
         * 获取图形的集合
         * @param sortType 排序类型
        */
		ArrayList<BaseShape*> getShapes(SortType sortType);
		/**
         * 获取纵轴
         * @param attachVScale 左侧或右侧
        */
		VScale* getVScale(AttachVScale attachVScale);
		/**
         * 重绘方法
         * @param paint  绘图对象
         * @param rect 矩形
        */
		virtual void onPaint(FCPaint *paint, const FCRect& rect);
		/**
		 * 移除画线
		 * @param plot 画线
        */
		void removePlot(FCPlot *plot);
		/**
         * 移除图形
         * @param shape 图形
        */
		void removeShape(BaseShape *shape);
		/**
         * 设置属性
         * @param name  属性名称
         * @param value 属性值
        */
		virtual void setProperty(const String& name, const String& value);
	};
}
#endif