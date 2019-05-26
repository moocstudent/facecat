/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __DATETITLE_H__
#define __DATETITLE_H__
#pragma once
#include "stdafx.h"
#include "FCButton.h"
#include "FCCalendar.h"

namespace FaceCat{
    class FCCalendar;
    
    /*
     * 日历标题栏
     */
    class DateTitle : public FCButton{
    protected:
        FCCalendar *m_calendar;
    public:
        /**
         * 日期标题
         */
        DateTitle(FCCalendar *calendar);
        /*
         * 析构函数
         */
        virtual ~DateTitle();
        /**
         * 获取日历
         */
        virtual FCCalendar* getCalendar();
        /**
         * 设置日历
         */
        virtual void setCalendar(FCCalendar *calendar);
    public:
        /**
         * 获取控件类型
         */
        virtual String getControlType();
        /**
         * 触摸点击方法
         * @param  touchInfo   触摸信息
         */
        virtual void onClick(FCTouchInfo touchInfo);
        /**
         * 重绘前景方法
         * @param  paint 绘图对象
         * @param  clipRect      裁剪区域
         */
        virtual void onPaintForeground(FCPaint *paint, const FCRect& clipRect);
    };
}
#endif
