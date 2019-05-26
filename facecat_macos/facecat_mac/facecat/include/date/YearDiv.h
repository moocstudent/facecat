/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __YEARDIV_H__
#define __YEARDIV_H__
#pragma once
#include "stdafx.h"
#include "YearButton.h"
#include "HeadDiv.h"
#include "TimeDiv.h"

namespace FaceCat{
    class FCCalendar;
    class HeadDiv;
    class YearButton;
    class TimeDiv;
    
    /*
     * 年的层
     */
    class YearDiv{
    protected:
        /**
         * 动画的方向
         */
        int m_am_Direction;
        /**
         * 动画当前帧数
         */
        int m_am_Tick;
        /**
         * 动画总帧数
         */
        int m_am_TotalTick;
        /**
         * 日历控件
         */
        FCCalendar *m_calendar;
        /**
         * 开始年份
         */
        int m_startYear;
    public:
        /**
         * 月的按钮
         */
        ArrayList<YearButton*> m_yearButtons;
        /**
         * 月的动画按钮
         */
        ArrayList<YearButton*> m_yearButtons_am;
    public:
        /*
         * 构造函数
         */
        YearDiv(FCCalendar *calendar);
        /*
         * 析构函数
         */
        virtual ~YearDiv();
        /**
         * 获取日历控件
         */
        virtual FCCalendar* getCalendar();
        /**
         * 设置日历控件
         */
        virtual void setCalendar(FCCalendar *calendar);
        /**
         * 获取开始年份
         */
        virtual int getStartYear();
    public:
        /**
         * 隐藏
         */
        void hide();
        /**
         * 触摸点击方法
         * @param  touchInfo   触摸信息
         */
        void onClick(FCTouchInfo touchInfo);
        /**
         * 添加控件方法
         */
        void onLoad();
        /**
         * 重绘方法
         * @param  paint   绘图对象
         * @param  clipRect   裁剪区域
         */
        void onPaint(FCPaint *paint, const FCRect& clipRect);
        /**
         * 重置日期图层
         * @param  state  状态
         */
        void onResetDiv(int state);
        /**
         * 秒表触发方法
         */
        void onTimer();
        /**
         * 选择开始年份
         * @param  startYear  开始年份
         */
        void selectStartYear(int startYear);
        /**
         * 显示
         */
        void show();
        /**
         * 更新布局方法
         */
        void update();
    };
}
#endif
