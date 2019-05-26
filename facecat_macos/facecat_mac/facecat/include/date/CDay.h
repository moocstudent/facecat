/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __CDAY_H__
#define __CDAY_H__
#pragma once
#include "stdafx.h"

namespace FaceCat{
    /**
     * 创建日
     * @param  year   年
     * @param  month   月
     * @param  day   日
     */
    class CDay{
    protected:
        int m_day;
        int m_month;
        int m_year;
    public:
        /*
         * 构造函数
         */
        CDay(int year, int month, int day);
        /*
         * 析构函数
         */
        virtual ~CDay();
        /**
         * 获取日
         */
        int getDay();
        /**
         * 获取月
         */
        int getMonth();
        /**
         * 获取年
         */
        int getYear();
    };
}
#endif
