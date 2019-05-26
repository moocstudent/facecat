/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __TIMEDIV_H__
#define __TIMEDIV_H__
#pragma once
#include "stdafx.h"
#include "FCCalendar.h"
#include "FCSpin.h"

namespace FaceCat{
    class FCCalendar;
    
    /*
     * 时间层
     */
    class TimeDiv{
    protected:
        /*
         * 选择时间改变事件
         */
        static void selectedTimeChanged(Object sender, Object pInvoke);
    protected:
        /**
         * 日历控件
         */
        FCCalendar *m_calendar;
        /**
         * 高度
         */
        int m_height;
        /**
         * 小时
         */
        FCSpin *m_spinHour;
        /**
         * 分钟
         */
        FCSpin *m_spinMinute;
        /**
         * 秒
         */
        FCSpin *m_spinSecond;
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
        TimeDiv(FCCalendar *calendar);
        /*
         * 析构函数
         */
        virtual ~TimeDiv();
        /**
         * 获取日历控件
         */
        virtual FCCalendar* getCalendar();
        /**
         * 设置日历控件
         */
        virtual void setCalendar(FCCalendar *calendar);
        /**
         * 获取或设置高度
         */
        virtual int getHeight();
        /**
         * 获取或设置高度
         */
        virtual void setHeight(int height);
        /**
         * 获取或设置小时
         */
        virtual int getHour();
        /**
         * 获取或设置小时
         */
        virtual void setHour(int hour);
        /**
         * 获取或设置分钟
         */
        virtual int getMinute();
        /**
         * 获取或设置分钟
         */
        virtual void setMinute(int minute);
        /**
         *获取秒
         */
        virtual int getSecond();
        /**
         * 设置秒
         */
        virtual void setSecond(int second);
    public:
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
         * 数值修改事件
         */
        void onSelectedTimeChanged();
        /**
         * 秒表触发方法
         */
        void onTimer();
        /**
         * 更新布局方法
         */
        void update();
    };
}
#endif
