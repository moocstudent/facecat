/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __CHARTTITLEBAR_H__
#define __CHARTTITLEBAR_H__
#pragma once
#include "stdafx.h"
#include "FCPaint.h"
#include "FCDataTable.h"
#include "Enums.h"
#include "ChartDiv.h"

namespace FaceCat{
    class ChartDiv;
    
    /*
     * 标题栏
     */
    class ChartTitle : public FCProperty{
    protected:
        /**
         * 保留小数的位数
         */
        int m_digit;
        /**
         * 字段名称
         */
        int m_fieldName;
        /**
         * 字段文字
         */
        String m_fieldText;
        /**
         * 标题显示模式
         */
        TextMode m_fieldTextMode;
        /**
         * 标题的分隔符
         */
        String m_fieldTextSeparator;
        /**
         * 文字的颜色
         */
        Long m_textColor;
        /**
         * 是否可见
         */
        bool m_visible;
    public:
        /**
         * 创建标题
         * @param  fieldName  字段名称
         * @param  fieldText  字段文字
         * @param  textColor  文字颜色
         * @param  digit      保留小数位数
         * @param  visible    是否可见
         */
        ChartTitle(int fieldName, const String& fieldText, Long color, int digit, bool visible);
        /**
         * 获取保留小数的位数
         */
        virtual int getDigit();
        /**
         * 设置保留小数的位数
         */
        virtual void setDigit(int digit);
        /**
         * 获取字段名称
         */
        virtual int getFieldName();
        /**
         * 设置字段名称
         */
        virtual void setFieldName(int fieldName);
        /**
         * 获取字段文字
         */
        virtual String getFieldText();
        /**
         * 设置字段文字
         */
        virtual void setFieldText(const String& fieldText);
        /**
         * 获取标题显示模式
         */
        virtual TextMode getFieldTextMode();
        /**
         * 设置标题显示模式
         */
        virtual void setFieldTextMode(TextMode fieldTextMode);
        /**
         * 获取标题的分隔符
         */
        virtual String getFieldTextSeparator();
        /**
         * 设置标题的分隔符
         */
        virtual void setFieldTextSeparator(const String& fieldTextSeparator);
        /**
         * 获取文字的颜色
         */
        virtual Long getTextColor();
        /**
         * 设置文字的颜色
         */
        virtual void setTextColor(Long textColor);
        /**
         * 获取是否可见
         */
        virtual bool isVisible();
        /**
         * 设置是否可见
         */
        virtual void setVisible(bool visible);
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
         * 设置属性值
         * @param name  属性名称
         * @param value 属性值
         */
        virtual void setProperty(const String& name, const String& value);
    };
    
    /*
     * 标题栏
     */
    class ChartTitleBar : public FCProperty{
    protected:
        /**
         * 是否允许用户绘图
         */
        bool m_allowUserPaint;
        /**
         * 字体
         */
        FCFont *m_font;
        /*
         * 文字颜色
         */
        Long m_textColor;
        /**
         * 标题高度
         */
        int m_height;
        /**
         * 最大标题行数
         */
        int m_maxLine;
        /**
         * 是否显示标题下面的线
         */
        bool m_showUnderLine;
        /**
         * 层的标题
         */
        String m_text;
        /**
         * 是否显示标题下面的线的颜色
         */
        Long m_underLineColor;
        /**
         * 是否显示标题
         */
        bool m_visible;
    public:
        /**
         * 图层标题栏
         */
        ChartTitleBar();
        /*
         * 析构函数
         */
        virtual ~ChartTitleBar();
        /*
         * 所有标题
         */
        ArrayList<ChartTitle*> Titles;
        /**
         * 获取是否允许用户绘图
         */
        virtual bool allowUserPaint();
        /**
         * 设置是否允许用户绘图
         */
        virtual void setAllowUserPaint(bool allowUserPaint);
        /**
         * 获取字体
         */
        virtual FCFont* getFont();
        /**
         * 设置字体
         */
        virtual void setFont(FCFont *font);
        /**
         * 获取标题高度
         */
        virtual int getHeight();
        /**
         * 设置标题高度
         */
        virtual void setHeight(int height);
        /**
         * 获取最大标题行数
         */
        virtual int getMaxLine();
        /**
         * 设置最大标题行数
         */
        virtual void setMaxLine(int maxLine);
        /**
         * 获取是否显示标题下面的线
         */
        virtual bool showUnderLine();
        /**
         * 设置是否显示标题下面的线
         */
        virtual void setShowUnderLine(bool showUnderLine);
        /**
         * 获取层的标题
         */
        virtual String getText();
        /**
         * 设置层的标题
         */
        virtual void setText(const String& text);
        /**
         * 获取标题的文字颜色
         */
        virtual Long getTextColor();
        /**
         * 设置标题的文字颜色
         */
        virtual void setTextColor(Long textColor);
        /**
         * 获取是否显示标题下面的线的颜色
         */
        virtual Long getUnderLineColor();
        /**
         * 设置是否显示标题下面的线的颜色
         */
        virtual void setUnderLineColor(Long underLineColor);
        /**
         * 获取是否显示标题
         */
        virtual bool isVisible();
        /**
         * 设置是否显示标题
         */
        virtual void setVisible(bool visible);
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
