/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __WINDOWEX_H__
#define __WINDOWEX_H__
#pragma once
#include "stdafx.h"
#include "FCDraw.h"
#include "WindowButton.h"
#include "FCWindow.h"

enum FCWindowState{
    FCWindowState_Max,
    FCWindowState_Min,
    FCWindowState_Normal
};

//自定义窗体
class WindowEx : public FCWindow{
private:
    int m_animateDirection;
    bool m_animateMoving;
    int m_animateType;
    WindowButton *m_closeButton;
    bool m_isChildWindow;
    WindowButton *m_maxOrRestoreButton;
    WindowButton *m_minButton;
    FCPoint m_normalLocation;
    FCSize m_normalSize;
    int m_timerID;
    FCWindowState m_windowState;
private:
    static void clickButton(void *sender, FCTouchInfo touchInfo, void *pInvoke);
public:
    WindowEx();
    virtual ~WindowEx();
    //是否动画
    bool isAnimateMoving();
    //关闭按钮
    WindowButton* getCloseButton();
    void setCloseButton(WindowButton* closeButton);
    //是否子窗体
    bool isChildWindow();
    void setChildWindow(bool isChildWindow);
    //最大化按钮
    WindowButton* getMaxOrRestoreButton();
    void setMaxOrRestoreButton(WindowButton *maxOrRestoreButton);
    //最小化按钮
    WindowButton* getMinButton();
    void setMinButton(WindowButton *minButton);
    //窗体状态
    FCWindowState getWindowState();
    void setWindowState(FCWindowState windowState);
public:
    //动画隐藏
    void animateHide();
    //动画显示
    void animateShow(bool showDialog);
    //最大化
    void maxOrRestore();
    //最小化
    void min();
    //控件添加方法
    virtual void onAdd();
    //拖动控件开始方法
    virtual void onDragReady(FCPoint *startOffset);
    //重绘背景方法
    virtual void onPaintBackground(FCPaint *paint, const FCRect& clipRect);
    //秒表方法
    virtual void onTimer(int timerID);
    //控件布局更新方法
    virtual void update();
};
#endif
