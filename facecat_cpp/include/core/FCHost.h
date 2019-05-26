/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCHOST_H__
#define __FCHOST_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "FCView.h"
#include "FCStr.h"
#include "FCPaint.h"
#include "GdiPaint.h"
#include "GdiPlusPaint.h"
#include "FCNative.h"

namespace FaceCat{
	class FCView;
	class FCNative;

    /**
     * 控件管理
     */
	class FCHost{
	public:
		/*
		* 构造函数
		*/
		FCHost();
		/*
		* 析构函数
		*/
		virtual ~FCHost();
		/**
		 * 获取方法库
		 */
		virtual FCNative* getNative();
		/**
		 * 设置方法库
		 */
		virtual void setNative(FCNative *native);
	public:
		/*
		* 激活镜像
		*/
		virtual void activeMirror();
		/**
		 * 是否允许操作
		 */
		virtual bool allowOperate();
		/**
		 * 是否允许局部绘图
		 */
		virtual bool allowPartialPaint();
		/**
		 * 在控件的线程中调用方法
		 */
		virtual void beginInvoke(FCView *control, Object args);
		/**
		 * 复制文本
		 */
		virtual void copy(string text);
		/**
		 * 创建内部控件
		 */
		virtual FCView* createInternalControl(FCView *parent, const String& clsid);
		/*
		* 获取光标形状
		*/
        virtual FCCursors getCursor();
        /**
         * 获取矩形相交区
         *
         * @param lpDestRect    相交矩形
         * @param lpSrc1Rect    矩形1
         * @param lpSrc2Rect    矩形2
         * @returns 是否相交
         */
		virtual int getIntersectRect(LPRECT lpDestRect, const FCRect *lpSrc1Rect, const FCRect *lpSrc2Rect);
        /**
		 * 获取触摸位置
		 */
        virtual FCPoint getTouchPoint();
        /**
		 * 获取尺寸
		 */
        virtual FCSize getSize();
		/*
		* 获取矩形联合区
		*/
		virtual int getUnionRect(LPRECT lpDestRect, const FCRect *lpSrc1Rect, const FCRect *lpSrc2Rect);
		/**
		 * 刷新绘图
		 */
		virtual void invalidate();
	    /**
         *  刷新绘图
         * @param rect 区域
         */
        virtual void invalidate(const FCRect& rect);
        /**
         *  在控件的线程中调用方法
         * @param control 控件
         * @param args  参数
         */
		virtual void invoke(FCView *control, Object args);
		/*
		* 键盘是否被按下
		*/
		virtual bool isKeyPress(char key);
		/**
		 * 获取粘贴文本
		 */
		virtual string paste();
		/**
		 * 设置是否允许操作
		 */
		virtual void setAllowOperate(bool allowOperate);
		/**
		 * 设置是否允许局部绘图
		 */
		virtual void setAllowPartialPaint(bool allowPartialPaint);
		/*
		* 设置鼠标形状
		*/
        virtual void setCursor(FCCursors cursor);
        /**
         *  显示提示框
         * @param text  文字
         * @param mp    位置
         */
		virtual void showToolTip(const String& text, const FCPoint& mp);
	    /**
         *  开启秒表
         * @param timerID   秒表ID
         * @param interval  间隔
         */
        virtual void startTimer(int timerID, int interval);
        /**
         *  停止秒表
         * @param timerID   秒表ID
         */
        virtual void stopTimer(int timerID);
	};
}
#endif