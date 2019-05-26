/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */


#ifndef __CHARTTOOLTIP_H__
#define __CHARTTOOLTIP_H__
#pragma once
#include "stdafx.h"
#include "FCPaint.h"
#include "ChartDiv.h"

namespace FaceCat{
    class ChartDiv;
    
    /*
     * 提示框
     */
    class ChartToolTip : public FCProperty{
    protected:
        /**
         * 是否允许用户绘图
         */
        bool m_allowUserPaint;
        /**
         * 提示框的背景色
         */
        Long m_backColor;
        /**
         * 边线的颜色
         */
        Long m_borderColor;
        /**
         * 提示框的字体
         */
        FCFont *m_font;
        /**
         * 提示框的字体颜色
         */
        Long m_textColor;
    public:
        ChartToolTip();
        /*
         * 析构函数
         */
        virtual ~ChartToolTip();
        /**
         * 获取是否允许用户绘图
         */
        virtual bool allowUserPaint();
        /**
         * 设置是否允许用户绘图
         */
        virtual void setAllowUserPaint(bool allowUserPaint);
        /**
         * 获取提示框的背景色
         */
        virtual Long getBackColor();
        /**
         * 设置提示框的背景色
         */
        virtual void setBackColor(Long backColor);
        /**
         * 获取边线的颜色
         */
        virtual Long getBorderColor();
        /**
         * 设置边线的颜色
         */
        virtual void setBorderColor(Long borderColor);
        /**
         * 获取提示框的字体
         */
        virtual FCFont* getFont();
        /**
         * 设置提示框的字体
         */
        virtual void setFont(FCFont *font);
        /**
         * 获取提示框的字体颜色
         */
        virtual Long getTextColor();
        /**
         * 设置提示框的字体颜色
         */
        virtual void setTextColor(Long textColor);
    public:
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
         * 重绘方法
         * @param paint  绘图对象
         * @param div    图层
         * @param rect   区域
         */
        virtual void onPaint(FCPaint *paint, ChartDiv *div, const FCRect& rect);
        /**
         * 设置属性值
         * @param name  属性名称
         * @param value 属性值
         */
        virtual void setProperty(const String& name, const String& value);
    };
}
#endif
