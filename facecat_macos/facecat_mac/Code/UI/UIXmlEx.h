/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __UIXMLEX_H__
#define __UIXMLEX_H__
#pragma once
#include "stdafx.h"
#include "WindowEx.h"
#include "FCUIXml.h"

namespace FaceCat{
    class UIXmlEx : public FCUIXml{
    protected:
        map<String, FCView*> m_controlsMap;
        map<String, FCTabPage*> m_tempTabPages;
    public:
        UIXmlEx();
        virtual ~UIXmlEx();
    public:
        //自动选中第一行
        void autoSelectFirstRow(FCGrid *grid);
        //自动选中最后一行
        void autoSelectLastRow(FCGrid *grid);
        //创建控件
        virtual FCView* createControl(xmlNodePtr node, const String& type);
        //查找控件
        virtual FCView* findControl(const String& name);
        //获取列的索引
        int getColumnsIndex(FCGrid *grid, map<int, FCGridColumn*> *columnsIndex);
        //加载数据
        virtual void loadData();
        //加载文件
        virtual void loadFile(const String& fileName, FCView *control);
        //控件被添加方法
        virtual void onAddControl(FCView *control, xmlNodePtr node);
    };
    
    class WindowXmlEx : public UIXmlEx{
    public:
        FCInvokeEvent m_invokeEvent;
        bool m_isClosing;
        WindowEx *m_window;
    private:
        static void clickButton(void *sender, FCTouchInfo touchInfo, void *pInvoke);
        static void invoke(void *sender, void *args, void *pInvoke);
        void registerEvents(FCView *control);
    public:
        WindowXmlEx();
        virtual ~WindowXmlEx();
        WindowEx* getWindow();
    public:
        virtual void close();
        virtual void load(FCNative *native, String xmlName, String windowName);
        void onInvoke(void *args);
        virtual void show();
    };
    
    class GridColumnEx : public FCGridColumn{
    public:
        virtual void onPaintBackground(FCPaint *paint, const FCRect &clipRect);
    };
}
#endif
