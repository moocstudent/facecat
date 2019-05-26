/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */


#ifndef __FCDIV_H__
#define __FCDIV_H__
#pragma once
#include "stdafx.h"
#include "FCView.h"
#include "FCScrollBar.h"
#include "FCHScrollBar.h"
#include "FCVScrollBar.h"

namespace FaceCat{
    class FCHScrollBar;
    class FCVScrollBar;
    
    /*
     * 图层
     */
    class FCDiv : public FCView{
    protected:
        /**
         * 是否允许拖动滚动
         */
        bool m_allowDragScroll;
        /**
         * 横向滚动条
         */
        FCHScrollBar *m_hScrollBar;
        /**
         * 是否正在被拖动
         */
        bool m_isDragScrolling;
        /**
         * 是否正在滚动2
         */
        bool m_isDragScrolling2;
        /**
         * 是否准备拖动滚动
         */
        bool m_readyToDragScroll;
        /**
         * 滚动按钮键盘事件按下事件
         */
        FCKeyEvent m_scrollButtonKeyDownEvent;
        /**
         * 滚动按钮触摸滚动事件
         */
        FCTouchEvent m_scrollButtonTouchWheelEvent;
        /**
         * 是否显示横向滚动条
         */
        bool m_showHScrollBar;
        /**
         * 是否显示纵向滚动条
         */
        bool m_showVScrollBar;
        /**
         * 开始移动的位置
         */
        FCPoint m_startMovePoint;
        /**
         * 开始移动的横向位置
         */
        int m_startMovePosX;
        /**
         * 开始移动的纵向位置
         */
        int m_startMovePosY;
        /**
         * 开始移动时间
         */
        uint64_t m_startMoveTime;
        /**
         * 纵向滚动条
         */
        FCVScrollBar *m_vScrollBar;
        /*
         * 纵向滚动条事件
         */
        FCEvent m_vScrollBarScrolledEvent;
        /**
         * 滚动按钮键盘事件按下事件
         */
        static void scrollButtonKeyDown(Object sender, char key, Object pInvoke);
        /**
         * 滚动按钮触摸滚动事件
         */
        static void scrollButtonTouchWheel(Object sender, FCTouchInfo touchInfo, Object pInvoke);
    public:
        /*
         * 构造函数
         */
        FCDiv();
        /*
         * 析构函数
         */
        virtual ~FCDiv();
        /**
         * 获取是否允许拖动滚动
         */
        virtual bool allowDragScroll();
        /**
         * 设置是否允许拖动滚动
         */
        virtual void setAllowDragScroll(bool allowDragScroll);
        /**
         * 获取横向滚动条
         */
        FCHScrollBar* getHScrollBar();
        /**
         * 获取是否显示横向滚动条
         */
        virtual bool showHScrollBar();
        /**
         * 设置是否显示横向滚动条
         */
        virtual void setShowHScrollBar(bool showHScrollBar);
        /**
         * 获取是否正在被拖动
         */
        virtual bool isDragScrolling();
        /**
         * 获取是否显示纵向滚动条
         */
        virtual bool showVScrollBar();
        /**
         * 设置是否显示纵向滚动条
         */
        virtual void setShowVScrollBar(bool showVScrollBar);
        /**
         * 获取纵向滚动条
         */
        virtual FCVScrollBar* getVScrollBar();
    public:
        /**
         * 获取内容的高度
         */
        virtual int getContentHeight();
        /**
         * 获取内容的宽度
         */
        virtual int getContentWidth();
        /**
         * 获取控件类型
         */
        virtual String getControlType();
        /**
         * 获取显示偏移坐标
         */
        virtual FCPoint getDisplayOffset();
        /**
         * 获取属性值
         * @param  name   属性名称
         * @param  value   返回属性值
         * @param  type   返回属性类型
         */
        virtual void getProperty(const String& name, String *value, String *type);
        /**
         * 获取属性名称列表
         */
        virtual ArrayList<String> getPropertyNames();
        /**
         * 向下滚动一行
         */
        virtual void lineDown();
        /**
         * 向左滚动一行
         */
        virtual void lineLeft();
        /**
         * 向右滚动一行
         */
        virtual void lineRight();
        /**
         * 向上滚动一行
         */
        virtual void lineUp();
        /**
         * 拖动准备方法
         * @param  startOffset  可以拖动的偏移坐标量
         */
        virtual void onDragReady(FCPoint *startOffset);
        /**
         * 拖动滚动结束
         */
        virtual void onDragScrollEnd();
        /**
         * 拖动滚动中
         */
        virtual void onDragScrolling();
        /**
         * 拖动滚动许可检查
         */
        virtual bool onDragScrollPermit();
        /**
         * 拖动滚动开始
         */
        virtual void onDragScrollStart();
        /**
         * 键盘按下方法
         * @param  key  按键
         */
        virtual void onKeyDown(char key);
        /**
         * 触摸点击方法
         * @param  touchInfo   触摸信息
         */
        virtual void onTouchDown(FCTouchInfo touchInfo);
        /**
         * 触摸移动方法
         * @param  touchInfo   触摸信息
         */
        virtual void onTouchMove(FCTouchInfo touchInfo);
        /**
         * 触摸抬起方法
         * @param  touchInfo   触摸信息
         */
        virtual void onTouchUp(FCTouchInfo touchInfo);
        /**
         * 触摸滚动方法
         * @param  touchInfo   触摸信息
         */
        virtual void onTouchWheel(FCTouchInfo touchInfo);
        /**
         * 预处理触摸事件
         * @param  eventID   事件ID
         * @param  touchInfo   触摸信息
         */
        virtual bool onPreviewsTouchEvent(int eventID, FCTouchInfo touchInfo);
        /**
         * 向下翻一页
         */
        virtual void pageDown();
        /**
         * 向左翻一页
         */
        virtual void pageLeft();
        /**
         * 向右翻一页
         */
        virtual void pageRight();
        /**
         * 向上翻一页
         */
        virtual void pageUp();
        /**
         * 设置属性值
         * @param  name   属性名称
         * @param  value   属性值
         */
        virtual void setProperty(const String& name, const String& value);
        /**
         * 更新布局方法
         */
        virtual void update();
        /**
         * 更新滚动条的布局
         */
        virtual void updateScrollBar();
    };
}

#endif
