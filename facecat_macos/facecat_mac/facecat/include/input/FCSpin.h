/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCSPIN_H__
#define __FCSPIN_H__
#pragma once
#include "stdafx.h"
#include "FCStr.h"
#include "FCTextBox.h"
#include "FCButton.h"

namespace FaceCat{
    /*
     * 数值选择
     */
    class FCSpin : public FCTextBox{
    private:
        /**
         * 秒表ID
         */
        int m_timerID;
        /**
         * TICK值
         */
        int m_tick;
    protected:
        /**
         * 是否自动格式化
         */
        bool m_autoFormat;
        /**
         * 保留小数的位数
         */
        int m_digit;
        /**
         * 向下按钮
         */
        FCButton *m_downButton;
        /*
         * 下拉按钮点击触摸按下事件
         */
        FCTouchEvent m_downButtonTouchDownEvent;
        /*
         * 下拉按钮点击触摸抬起事件
         */
        FCTouchEvent m_downButtonTouchUpEvent;
        /**
         * 是否正在增量
         */
        bool m_isAdding;
        /**
         * 是否正在减量
         */
        bool m_isReducing;
        /**
         * 最大值
         */
        double m_maximum;
        /**
         * 最小值
         */
        double m_minimum;
        /**
         * 是否显示千分位
         */
        bool m_showThousands;
        /**
         * 数值增减幅度
         */
        double m_step;
        /**
         * 获取向上按钮
         */
        FCButton *m_upButton;
        /**
         * 向上按钮的触摸按下函数
         */
        FCTouchEvent m_upButtonTouchDownEvent;
        /**
         * 向上按钮的触摸抬起函数
         */
        FCTouchEvent m_upButtonTouchUpEvent;
        /**
         * 向下按钮的触摸按下事件
         */
        static void downButtonTouchDown(Object sender, FCTouchInfo touchInfo, Object pInvoke);
        /**
         * 向下按钮的触摸抬起事件
         */
        static void downButtonTouchUp(Object sender, FCTouchInfo touchInfo, Object pInvoke);
        /**
         * 向上按钮的触摸按下事件
         */
        static void upButtonTouchDown(Object sender, FCTouchInfo touchInfo, Object pInvoke);
        /**
         * 向上按钮的触摸抬起事件
         */
        static void upButtonTouchUp(Object sender, FCTouchInfo touchInfo, Object pInvoke);
    public:
        /*
         * 构造函数
         */
        FCSpin();
        /*
         * 析构函数
         */
        virtual ~FCSpin();
        /**
         * 获取是否自动格式化
         */
        virtual bool autoFormat();
        /**
         * 设置是否自动格式化
         */
        virtual void setAutoFormat(bool autoFormat);
        /**
         * 获取保留小数的位数
         */
        virtual int getDigit();
        /**
         * 设置保留小数的位数
         */
        virtual void setDigit(int digit);
        /**
         * 获取向下按钮
         */
        virtual FCButton* getDownButton();
        /**
         * 获取是否正在增量
         */
        virtual bool isAdding();
        /**
         * 设置是否正在增量
         */
        virtual void setIsAdding(bool isAdding);
        /**
         * 获取是否正在减量
         */
        virtual bool isReducing();
        /**
         * 设置是否正在减量
         */
        virtual void setIsReducing(bool isReducing);
        /**
         * 获取最大值
         */
        virtual double getMaximum();
        /**
         * 设置最大值
         */
        virtual void setMaximum(double maximum);
        /**
         * 获取最小值
         */
        virtual double getMinimum();
        /**
         * 设置最小值
         */
        virtual void setMinimum(double minimum);
        /**
         * 获取是否显示千分位
         */
        virtual bool showThousands();
        /**
         * 设置是否显示千分位
         */
        virtual void setShowThousands(bool showThousands);
        /**
         * 获取数值增减幅度
         */
        virtual double getStep();
        /**
         * 设置数值增减幅度
         */
        virtual void setStep(double step);
        /**
         * 设置文本
         */
        virtual void setText(const String& text);
        /**
         * 获取向上按钮
         */
        virtual FCButton* getUpButton();
        /**
         * 获取当的数值
         */
        virtual double getValue();
        /**
         * 设置当的数值
         */
        virtual void setValue(double value);
    public:
        /**
         *
         * 增加指定幅度的数值
         */
        void add();
        /**
         * 将文本转化为千分位显示
         */
        String formatNum(String inputText);
        /**
         * 获取控件类型
         */
        virtual String getControlType();
        /**
         * 获取属性值
         * @param  name  属性名称
         * @param  value  返回属性值
         * @param  type  返回属性类型
         */
        virtual void getProperty(const String& name, String *value, String *type);
        /**
         * 获取属性名称列表
         */
        virtual ArrayList<String> getPropertyNames();
        /**
         * 根据保留小数的位置将double型转化为String型
         */
        String getValueByDigit(double value, int digit);
        /**
         * 输入文字方法
         */
        virtual void onChar(wchar_t ch);
        /**
         * 键盘按下方法
         */
        virtual void onKeyDown(char key);
        /**
         * 添加控件方法
         */
        virtual void onLoad();
        /**
         * 触摸滚动方法
         */
        virtual void onTouchWheel(FCTouchInfo touchInfo);
        /**
         * 粘贴方法
         */
        virtual void onPaste();
        /**
         * 秒表事件
         */
        virtual void onTimer(int timerID);
        /**
         * 数值改变方法
         */
        virtual void onValueChanged();
        /**
         * 减少指定幅度的数值
         */
        void reduce();
        /**
         * 设置属性
         * @param  name  属性名称
         * @param  value  属性值
         */
        virtual void setProperty(const String& name, const String& value);
        /**
         * 设置区域
         */
        void setRegion();
        /**
         * 更新布局方法
         */
        virtual void update();
    };
}

#endif
