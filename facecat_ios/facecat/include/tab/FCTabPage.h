/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCTABPAGE_H__
#define __FCTABPAGE_H__
#pragma once
#include "stdafx.h"
#include "FCDiv.h"
#include "FCButton.h"
#include "FCTabControl.h"

namespace FaceCat{
    class FCTabControl;
    /**
     * 多页夹控件
     */
    class FCTabPage:public FCDiv{
    protected:
        /**
         * 开始拖动页头按钮事件
         */
        FCEvent m_dragHeaderBeginEvent;
        /**
         * 结束拖动页头按钮事件
         */
        FCEvent m_dragHeaderEndEvent;
        /**
         * 页头按钮拖动中事件
         */
        FCEvent m_draggingHeaderEvent;
        /**
         * 获取或设置页头按钮
         */
        FCButton *m_headerButton;
        /**
         * 获取或设置头部的位置
         */
        FCPoint m_headerLocation;
        /**
         * 页头的触摸按下事件
         */
        FCTouchEvent m_headerTouchDownEvent;
        /**
         * 多页夹控件
         */
        FCTabControl *m_tabControl;
        /**
         * 开始拖动页头按钮方法
         * @param  sender   调用者
         */
        static void dragHeaderBegin(Object sender, Object pInvoke);
        /**
         * 结束拖动页头按钮方法
         */
        static void dragHeaderEnd(Object sender, Object pInvoke);
        /**
         * 页头按钮拖动中方法
         */
        static void draggingHeader(Object sender, Object pInvoke);
        /**
         * 页头触摸按下方法
         * @param  sender  调用者
         * @param  touchInfo  触摸信息
         */
        static void headerTouchDown(Object sender, FCTouchInfo touchInfo, Object pInvoke);
    public:
        /*
         * 创建页
         */
        FCTabPage();
        /*
         * 删除页
         */
        virtual ~FCTabPage();
        /**
         * 获取页头按钮
         */
        virtual FCButton* getHeaderButton();
        /**
         * 设置页头按钮
         */
        virtual void setHeaderButton(FCButton *headerButton);
        /**
         * 获取头部的位置
         */
        virtual FCPoint getHeaderLocation();
        /**
         * 设置头部的位置
         */
        virtual void setHeaderLocation(FCPoint headerLocation);
        /**
         * 获取头部是否可见
         */
        virtual bool isHeaderVisible();
        /**
         * 设置头部是否可见
         */
        virtual void setHeaderVisible(bool visible);
        /**
         * 获取多页夹控件
         */
        virtual FCTabControl* getTabControl();
        /**
         * 设置多页夹控件
         */
        virtual void setTabControl(FCTabControl *tabControl);
    public:
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
         * 添加控件方法
         */
        virtual void onLoad();
        /**
         * 文字改变方法
         */
        virtual void onTextChanged();
        /**
         * 设置属性
         * @param  name  属性名称
         * @param  value  属性值
         */
        virtual void setProperty(const String& name, const String& value);
    };
}

#endif
