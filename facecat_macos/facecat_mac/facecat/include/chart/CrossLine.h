/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */


#ifndef __CROSSLINE_H__
#define __CROSSLINE_H__
#pragma once
#include "stdafx.h"
#include "Enums.h"
#include "ChartDiv.h"

namespace FaceCat{
    class ChartDiv;
    /**
     * 十字线
     */
    class CrossLine : FCProperty{
    protected:
        /**
         * 是否允许用户绘图
         */
        bool m_allowUserPaint;
        /**
         * 是否允许双击操作
         */
        bool m_allowDoubleClick;
        /**
         * 依附于左轴还是右轴
         */
        AttachVScale m_attachVScale;
        /**
         * 十字线的颜色
         */
        Long m_lineColor;
    public:
        /*
         * 构造函数
         */
        CrossLine();
        /*
         * 析构函数
         */
        virtual ~CrossLine();
        /**
         * 获取是否允许双击操作
         */
        virtual bool allowDoubleClick();
        /**
         * 设置是否允许双击操作
         */
        virtual void setAllowDoubleClick(bool allowDoubleClick);
        /**
         * 获取是否允许用户绘图
         */
        virtual bool allowUserPaint();
        /**
         * 设置是否允许用户绘图
         */
        virtual void setAllowUserPaint(bool allowUserPaint);
        /**
         * 获取依附于左轴还是右轴
         */
        virtual AttachVScale getAttachVScale();
        /**
         * 设置依附于左轴还是右轴
         */
        virtual void setAttachVScale(AttachVScale attachVScale);
        /**
         * 获取十字线的颜色
         */
        virtual Long getLineColor();
        /**
         * 设置十字线的颜色
         */
        virtual void setLineColor(Long lineColor);
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
