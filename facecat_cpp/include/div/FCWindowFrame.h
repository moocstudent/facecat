/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCWINDOWFRAME_H__
#define __FCWINDOWFRAME_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "FCWindow.h"

namespace FaceCat
{
	/*
	* 窗体边界
	*/
	class FCWindowFrame : public FCView{
	public:
	    /**
		 * 窗体控件边界
		 */
		FCWindowFrame();
		/*
		* 析构函数
		*/
		virtual ~FCWindowFrame();
	public:
		/**
		* 是否包含坐标
		* @param  point  坐标
		*/
		virtual bool containsPoint(const FCPoint& point);
		/**
		 * 获取控件类型
		 */
		virtual String getControlType();
		/**
		 * 重绘方法
		 */
		virtual void invalidate();
		/**
		* 绘制背景方法
		* @param  paint 绘图对象
		* @param  clipRect   裁剪区域
		*/
		virtual void onPaintBackground(FCPaint *paint, const FCRect& clipRect);
	};
}
#endif