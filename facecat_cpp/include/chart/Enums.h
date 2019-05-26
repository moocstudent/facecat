/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __ENUMS_H__
#define __ENUMS_H__
#pragma once
#include "..\\..\\stdafx.h"

namespace FaceCat{
	/*
	* 动作类型
	*/
	enum ActionType{
		ActionType_AT1 = 1, //第一个点的动作
		ActionType_AT2 = 2, //第二个点的动作
		ActionType_AT3 = 3, //第三个点的动作
		ActionType_AT4 = 4, //第四个点的动作
		ActionType_MOVE = 0, //移动
		ActionType_NO = -1 //不移动
	};

	/*
	* 纵轴的类型
	*/
	enum AttachVScale{
		AttachVScale_Left, //左轴
		AttachVScale_Right //右轴
	};

	/*
	* 柱状图样式
	*/
	enum BarStyle{
		BarStyle_Line, //线条
		BarStyle_Rect //矩形
	};

	/*
	* 线的样式
	*/
	enum CandleStyle{
		CandleStyle_American, //美国线
		CandleStyle_CloseLine, //收盘线
		CandleStyle_Rect, //矩形
		CandleStyle_Tower //宝塔线
	};

	/*
	* 十字线的移动方式
	*/
	enum CrossLineMoveMode{
		CrossLineMoveMode_AfterClick, //触摸点击后移动
		CrossLineMoveMode_FollowTouch //跟随触摸
	};

	/*
	* 日期的类型
	*/
	enum DateType{
		DateType_Day = 2, //日
		DateType_Hour = 3, //小时
		DateType_Millisecond = 6, //毫秒
		DateType_Minute = 4, //分钟
		DateType_Month = 1, //月
		DateType_Second = 5, //秒
		DateType_Year = 0 //年
	};

	/*
	* X轴的类型
	*/
	enum HScaleType{
		HScaleType_Date, //日期
		HScaleType_Number //数字
	};

	/*
	* 数字的样式
	*/
	enum NumberStyle{
		NumberStyle_Standard, //标准
		NumberStyle_Underline //加下划线数字
	};

	/*
	* 折线的样式
	*/
	enum PolylineStyle{
		PolylineStyle_Cycle, //圆圈
		PolylineStyle_DashLine, //虚线
		PolylineStyle_DotLine, //细点图
		PolylineStyle_SolidLine //实线
	};

	/*
	* 滚动类型
	*/
	enum ScrollType{
		ScrollType_None, //无
		ScrollType_Left, //向左
		ScrollType_Right, //向右
		ScrollType_ZoomIn, //缩小
		ScrollType_ZoomOut //放大
	};

	/*
	* 选中点样式
	*/
	enum SelectPoint{
		SelectPoint_Ellipse, //圆
		SelectPoint_Rect //矩形
	};

	/*
	* 图形标题的模式
	*/
	enum TextMode{
		TextMode_Field, //显示字段
		TextMode_Full, //显示完整
		TextMode_None, //不显示
		TextMode_Value //显示值
	};

	/*
	* 纵轴坐标系
	*/
	enum VScaleSystem{
		VScaleSystem_Logarithmic, //对数坐标
		VScaleSystem_Standard //标准
	};

	/*
	* 纵坐标轴类型
	*/
	enum VScaleType{
		VScaleType_Divide, //等分
		VScaleType_EqualDiff, //等差
		VScaleType_EqualRatio, //等比
		VScaleType_GoldenRatio, //黄金分割
		VScaleType_Percent //黄金分割
	};
}
#endif