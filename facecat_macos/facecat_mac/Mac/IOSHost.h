/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __IOSHOST_H__
#define __IOSHOST_H__
#pragma once
#include "stdafx.h"
#include "FCHost.h"
#include "ContextPaint.h"
#include "ViewController.h"
#import <Cocoa/Cocoa.h>

class IOSTimer{
public:
    IOSTimer();
    virtual ~IOSTimer();
    int m_interval;
    int m_tick;
    int m_timerID;
};

class IOSInvoke{
public:
    FCView *m_control;
    void *m_args;
public:
    IOSInvoke(){
        m_control = 0;
        m_args = 0;
    }
    virtual ~IOSInvoke(){
        m_control = 0;
        m_args = 0;
    }
};

class IOSHost : public FCHost{
public:
    bool m_allowOperate;
    bool m_allowPartialPaint;
    bool m_isViewAppear;
    FCLock m_lock;
    FCPoint m_mousePoint;
    FCNative *m_native;
    int m_threadState;
    map<int, IOSTimer*> m_timers;
    NSView *m_view;
public:
    vector<IOSInvoke*> m_invokes;
    FCLock m_invokeLock;
    IOSHost();
    virtual ~IOSHost();
    virtual FCNative* getNative();
    virtual void setNative(FCNative *native);
    virtual NSView* getView();
    virtual void setView(NSView *view);
public:
    static CGPoint getCGPoint(const FCPoint& point);
    static CGRect getCGRect(const FCRect& rect);
    static CGSize getCGSize(const FCSize& size);
    static NSString* getNSString(const wchar_t *str);
    static FCPoint getPoint(CGPoint cgPoint);
    static FCRect getRect(CGRect cgRect);
    static FCSize getSize(CGSize cgSize);
public:
    virtual bool allowOperate();
    virtual bool allowPartialPaint();
    virtual void beginInvoke(FCView *control, void *args);
    virtual void copy(string text);
    virtual FCView* createInternalControl(FCView *parent, const String& clsid);
    FCSize getClientSize();
    virtual FCCursors getCursor();
    virtual int getIntersectRect(FCRect *lpDestRect, const FCRect *lpSrc1Rect, const FCRect *lpSrc2Rect);
    virtual FCPoint getTouchPoint();
    virtual FCSize getSize();
    virtual int getUnionRect(FCRect *lpDestRect, const FCRect *lpSrc1Rect, const FCRect *lpSrc2Rect);
    virtual void invalidate();
    virtual void invalidate(const FCRect& rect);
    virtual void invoke(FCView *control, void *args);
    void onPaint(const FCRect& rect);
    void onTimer();
    virtual string paste();
    virtual void setAllowOperate(bool allowOperate);
    virtual void setAllowPartialPaint(bool allowPartialPaint);
    virtual void setCursor(FCCursors cursor);
    virtual void setTouchPoint(const FCPoint& mp);
    virtual void startTimer(int timerID, int interval);
    virtual void stopTimer(int timerID);
};

#endif
