/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __MONTHBUTTON_H__
#define __MONTHBUTTON_H__
#pragma once
#include "stdafx.h"
#include "FCButton.h"
#include "FCCalendar.h"

namespace FaceCat{
    class FCCalendar;
    
    /*
     * 月的按钮
     */
    class MonthButton:public FCButton{
    protected:
        /**
         * 显示区域
         */
        FCRect m_bounds;
        /**
         * 日历控件
         */
        FCCalendar *m_calendar;
        /**
         * 月
         */
        int m_month;
        /**
         * 是否可见
         */
        bool m_visible;
        /**
         * 年
         */
        int m_year;
    protected:
        /**
         * 获取绘制的背景色
         */
        virtual Long getPaintingBackColor();
        /**
         * 获取绘制的边线颜色
         */
        virtual Long getPaintingBorderColor();
        /**
         * 获取要绘制的前景色
         */
        virtual Long getPaintingTextColor();
    public:
        /*
         * 构造函数
         */
        MonthButton(FCCalendar *calendar);
        /*
         * 析构函数
         */
        virtual ~MonthButton();
        /**
         * 获取日历控件
         */
        virtual FCCalendar* getCalendar();
        /**
         * 设置日历控件
         */
        virtual void setCalendar(FCCalendar *calendar);
        /**
         * 获取显示区域
         */
        virtual FCRect getBounds();
        /**
         * 设置显示区域
         */
        virtual void setBounds(const FCRect& bounds);
        /**
         * 获取月
         */
        virtual int getMonth();
        /**
         * 设置月
         */
        virtual void setMonth(int month);
        /**
         * 获取是否可见
         */
        virtual bool isVisible();
        /**
         * 设置是否可见
         */
        virtual void setVisible(bool visible);
        /**
         * 获取年
         */
        virtual int getYear();
        /**
         * 设置年
         */
        virtual void setYear(int year);
    public:
        /**
         * 获取月的文字
         */
        virtual String getMonthStr();
        /**
         * 触摸点击方法
         * @param  touchInfo   触摸信息
         */
        virtual void onClick(FCTouchInfo touchInfo);
        /**
         * 重绘背景方法
         * @param  paint   绘图对象
         * @param  clipRect   裁剪区域
         */
        virtual void onPaintBackGround(FCPaint *paint, const FCRect& clipRect);
        /**
         * 重绘边线方法
         * @param  paint   绘图对象
         * @param  clipRect   裁剪区域
         */
        virtual void onPaintBorder(FCPaint *paint, const FCRect& clipRect);
        /**
         * 重绘前景方法
         * @param  paint   绘图对象
         * @param  clipRect   裁剪区域
         */
        virtual void onPaintForeground(FCPaint *paint, const FCRect& clipRect);
    };
}
#endif
