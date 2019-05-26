/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCUIEVENT_H__
#define __FCUIEVENT_H__
#pragma once
#include "stdafx.h"
#include "FCUIScript.h"
#include "FCUIXml.h"

namespace FaceCat{
	class FCUIScript;
	class FCUIXml;
    
    /*
     * 事件信息
     */
	class FCEventInfo{
	private:
        /**
         * 方法
         */
		HashMap<int, String> m_functions;
	public:
        /*
         * 构造函数
         */
		FCEventInfo();
        /*
         * 析构函数
         */
		virtual ~FCEventInfo();
	public:
        /**
         * 添加事件
         * @params eventID  事件ID
         * @params function  方法
         */
		void addEvent(int eventID, const String& function);
        /**
         * 获取方法
         */
		String getFunction(int eventID);
        /**
         * 移除事件
         */
		void removeEvent(int eventID);
	};
    
    /*
     * 事件
     */
	class FCUIEvent{
	private:
        /**
         * 脚本
         */
		FCUIScript *m_script;
        /**
         * 调用者
         */
		String m_sender;
        /**
         * XML对象
         */
		FCUIXml *m_xml;
	public:
        /**
         * 事件集合
         */
		map<FCView*, FCEventInfo*> m_events;
	public:
        /**
         * 创建事件
         */
		FCUIEvent(FCUIXml *xml);
        /*
         * 析构函数
         */
		virtual ~FCUIEvent();
        /**
         * 获取脚本
         */
        FCUIScript* getScript();
        /**
         * 设置脚本
         */
        void setScript(FCUIScript *script);
        /**
         * 获取调用者
         */
        String getSender();
        /**
         * 设置调用者
         */
        void setSender(const String& sender);
        /**
         * 获取XML对象
         */
        FCUIXml* getXml();
        /**
         * 设置XML对象
         */
        void setXml(FCUIXml *xml);
	public:
        /**
         * 添加控件事件
         */
        static void callAdd(Object sender, Object pInvoke);
        /**
         * 添加背景颜色改变事件
         */
        static void callBackColorChanged(Object sender, Object pInvoke);
        /**
         * 添加背景图片改变时间
         */
        static void callBackImageChanged(Object sender, Object pInvoke);
        /**
         * 字符输入事件
         */
        static void callChar(Object sender, char ch, Object pInvoke);
        /**
         * 选中改变事件
         */
        static void callCheckedChanged(Object sender, Object pInvoke);
        /**
         * 触摸点击事件
         */
        static void callClick(Object sender, FCTouchInfo touchInfo, Object pInvoke);
        /**
         * 复制事件
         */
        static void callCopy(Object sender, Object pInvoke);
        /**
         * 剪切事件
         */
        static void callCut(Object sender, Object pInvoke);
        /**
         * Dock改变事件
         */
        static void callDockChanged(Object sender, Object pInvoke);
        /**
         * 双击事件
         */
        static void callDoubleClick(Object sender, Object pInvoke);
        /**
         * 拖动开始事件
         */
        static void callDragBegin(Object sender, Object pInvoke);
        /**
         * 拖动结束事件
         */
        static void callDragEnd(Object sender, Object pInvoke);
        /**
         * 正在拖动事件
         */
        static void callDragging(Object sender, Object pInvoke);
        /**
         * 可用改变事件
         */
        static void callEnableChanged(Object sender, Object pInvoke);
        /*
         * 调用服务
         */
        static String callFunction(Object sender, int eventID, Object pInvoke);
        /**
         * 字体改变事件
         */
        static void callFontChanged(Object sender, Object pInvoke);
        /**
         * 前景色改变事件
         */
        static void callTextColorChanged(Object sender, Object pInvoke);
        /**
         * 获得焦点事件
         */
        static void callGotFocus(Object sender, Object pInvoke);
        /**
         * 单元格点击事件
         */
        static void callGridCellClick(Object sender, Object pInvoke);
        /**
         * 单元格编辑开始事件
         */
        static void callGridCellEditBegin(Object sender, Object pInvoke);
        /**
         * 单元格编辑结束事件
         */
        static void callGridCellEditEnd(Object sender, Object pInvoke);
        /**
         * 单元格触摸按下事件
         */
        static void callGridCellTouchDown(Object sender, Object pInvoke);
        /**
         * 单元格触摸移动事件
         */
        static void callGridCellTouchMove(Object sender, Object pInvoke);
        /**
         * 单元格触摸抬起事件
         */
        static void callGridCellTouchUp(Object sender, Object pInvoke);
        /**
         * 在控件线程中调用事件
         */
        static void callInvoke(Object sender, Object args, Object pInvoke);
        /**
         * 控件加载事件
         */
        static void callLoad(Object sender, Object pInvoke);
        /**
         * 位置改变事件
         */
        static void callLocationChanged(Object sender, Object pInvoke);
        /**
         * 丢失焦点事件
         */
        static void callLostFocus(Object sender, Object pInvoke);
        /**
         * 外边界改变事件
         */
        static void callMarginChanged(Object sender, Object pInvoke);
        /**
         * 键盘按下事件
         */
        static void callKeyDown(Object sender, char key, Object pInvoke);
        /**
         * 键盘弹起事件
         */
        static void callKeyUp(Object sender, char key, Object pInvoke);
        /**
         * 外边界改变事件
         */
        static void callMenuItemClick(Object sender, FCMenuItem *item, FCTouchInfo touchInfo, Object pInvoke);
        /**
         * 触摸按下事件
         */
        static void callTouchDown(Object sender, FCTouchInfo touchInfo, Object pInvoke);
        /**
         * 触摸进入事件
         */
        static void callTouchEnter(Object sender, FCTouchInfo touchInfo, Object pInvoke);
        /**
         * 触摸离开事件
         */
        static void callTouchLeave(Object sender, FCTouchInfo touchInfo, Object pInvoke);
        /**
         * 触摸移动事件
         */
        static void callTouchMove(Object sender, FCTouchInfo touchInfo, Object pInvoke);
        /**
         * 触摸抬起事件
         */
        static void callTouchUp(Object sender, FCTouchInfo touchInfo, Object pInvoke);
        /**
         * 触摸滚动事件
         */
        static void callTouchWheel(Object sender, FCTouchInfo touchInfo, Object pInvoke);
        /**
         * 内边界改变事件
         */
        static void callPaddingChanged(Object sender, FCPaint *paint, const FCRect& clipRect, Object pInvoke);
        /**
         * 重绘事件
         */
        static void callPaint(Object sender, FCPaint *paint, const FCRect& clipRect, Object pInvoke);
        /**
         * 重绘边线事件
         */
        static void callPaintBorder(Object sender, FCPaint *paint, const FCRect& clipRect, Object pInvoke);
        /**
         * 父控件发生变化
         */
        static void callParentChanged(Object sender, Object pInvoke);
        /**
         * 复制事件
         */
        static void callPaste(Object sender, Object pInvoke);
        /**
         * 区域发生变化事件
         */
        static void callRegionChanged(Object sender, Object pInvoke);
        /**
         * 移除控件事件
         */
        static void callRemove(Object sender, Object pInvoke);
        /**
         * 选中日期改变事件
         */
        static void callSelectedTimeChanged(Object sender, Object pInvoke);
        /**
         * 选中索引改变事件
         */
        static void callSelectedIndexChanged(Object sender, Object pInvoke);
        /**
         * 选中页改变事件
         */
        static void callSelectedTabPageChanged(Object sender, Object pInvoke);
        /**
         * 滚动事件
         */
        static void callScrolled(Object sender, Object pInvoke);
        /**
         * 尺寸改变事件
         */
        static void callSizeChanged(Object sender, Object pInvoke);
        /**
         * Tab改变事件
         */
        static void callTabIndexChanged(Object sender, Object pInvoke);
        /**
         * Tab停留事件
         */
        static void callTabStop(Object sender, Object pInvoke);
        /**
         * 文本改变事件
         */
        static void callTextChanged(Object sender, Object pInvoke);
        /**
         * 秒表回调事件
         */
        static void callTimer(Object sender, int timerID, Object pInvoke);
        /**
         * 数值改变事件
         */
        static void callVisibleChanged(Object sender, Object pInvoke);
        /**
         * 可见状态改变事件
         */
        static void callValueChanged(Object sender, Object pInvoke);
        /**
         * 窗体已关闭事件
         */
        static void callWindowClosed(Object sender, Object pInvoke);
        /**
         * 获取事件的ID
         */
        virtual int getEventID(const String& eventName);
        /**
         * 添加事件
         */
        virtual void addEvent(FCView *control, const String& eventName, const String& function);
        
    };
}
#endif
