/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCWINDOW_H__
#define __FCWINDOW_H__
#pragma once
#include "stdafx.h"
#include "FCButton.h"
#include "FCWindowFrame.h"

namespace FaceCat{
    class FCWindowFrame;
    
    typedef void (*FCWindowClosingEvent)(Object, bool*, Object);
    
    /*
     * 窗体
     */
    class FCWindow : public FCView{
    protected:
        int m_borderWidth;
        /**
         * 是否可以调整尺寸
         */
        bool m_canResize;
        /**
         * 标题栏的高度
         */
        int m_captionHeight;
        /**
         * 窗体的
         */
        FCWindowFrame *m_frame;
        /**
         * 是否会话窗口
         */
        bool m_isDialog;
        /**
         * 调整尺寸的点
         */
        int m_resizePoint;
        /**
         * 阴影的颜色
         */
        Long m_shadowColor;
        /**
         * 阴影的大小
         */
        int m_shadowSize;
        /**
         * 移动开始点
         */
        FCPoint m_startTouchPoint;
        /**
         * 移动开始时的控件矩形
         */
        FCRect m_startRect;
    protected:
        /**
         * 调用事件
         * @param  eventID  事件ID
         * @param  cancel   是否退出
         */
        void callWindowClosingEvents(int eventID, bool *cancel);
        /**
         * 获取触摸状态
         * @param  state  状态值
         */
        FCCursors getResizeCursor(int state);
        /**
         * 获取调整尺寸的点
         */
        ArrayList<FCRect> getResizePoints();
        /**
         * 获取调整尺寸的状态
         */
        int getResizeState();
    public:
        FCWindow();
        virtual ~FCWindow();
        /**
         * 获取边框的宽度
         */
        virtual int getBorderWidth();
        /**
         * 设置边框的宽度
         */
        virtual void setBorderWidth(int borderWidth);
        /**
         * 获取标题栏的高度
         */
        virtual int getCaptionHeight();
        /**
         * 设置标题栏的高度
         */
        virtual void setCaptionHeight(int captionHeight);
        /**
         * 获取是否可以调整尺寸
         */
        virtual bool canResize();
        /**
         * 设置是否可以调整尺寸
         */
        virtual void setCanResize(bool canResize);
        /**
         * 获取窗体的
         */
        virtual FCWindowFrame* getFrame();
        /**
         * 设置窗体的
         */
        virtual void setFrame(FCWindowFrame *frame);
        /**
         * 获取是否会话窗口
         */
        virtual bool isDialog();
        /**
         * 获取阴影的颜色
         */
        virtual Long getShadowColor();
        /**
         * 设置阴影的颜色
         */
        virtual void setShadowColor(Long shadowColor);
        /**
         * 获取阴影的大小
         */
        virtual int getShadowSize();
        /**
         * 设置阴影的大小
         */
        virtual void setShadowSize(int shadowSize);
    public:
        /**
         * 将控件放到最前显示
         */
        virtual void bringToFront();
        /**
         * 关闭窗体
         */
        virtual void close();
        /**
         * 获取控件类型
         */
        virtual String getControlType();
        /**
         * 获取动态绘图区域
         */
        FCRect getDynamicPaintRect();
        /**
         * 获取属性值
         * @param  name   属性名称
         * @param  value   返回属性值
         * @param  type   返回属性类型
         */
        virtual void getProperty(const String& name, String *value, String *type);
        /**
         * 获取属性列表
         */
        virtual ArrayList<String> getPropertyNames();
        /**
         * 滚动开始方法
         */
        virtual bool onDragBegin();
        /**
         * 拖动准备方法
         * @param  startOffset  可以拖动的偏移坐标量
         */
        virtual void onDragReady(FCPoint *startOffset);
        /**
         * 触摸按下方法
         * @param  touchInfo  触摸信息
         */
        virtual void onTouchDown(FCTouchInfo touchInfo);
        /**
         * 触摸移动方法
         * @param  touchInfo  触摸信息
         */
        virtual void onTouchMove(FCTouchInfo touchInfo);
        /**
         * 触摸抬起方法
         * @param  touchInfo  触摸信息
         */
        virtual void onTouchUp(FCTouchInfo touchInfo);
        /**
         * 绘制前景方法
         * @param  paint   绘图对象
         * @param  clipRect  裁剪区域
         */
        virtual void onPaintForeground(FCPaint *paint, const FCRect& clipRect);
        /**
         * 可见状态改变方法
         */
        virtual void onVisibleChanged();
        /**
         * 窗体正在关闭方法
         * @param  cancel  是否退出
         */
        virtual void onWindowClosing(bool *cancel);
        /**
         * 窗体关闭方法
         */
        virtual void onWindowClosed();
        /**
         * 将控件放到最下面显示
         */
        virtual void sendToBack();
        /**
         * 设置属性值
         * @param  name   属性名称
         * @param  value   属性值
         */
        virtual void setProperty(const String& name, const String& value);
        /**
         * 以会话方式显示
         */
        virtual void showDialog();
    };
}
#endif
