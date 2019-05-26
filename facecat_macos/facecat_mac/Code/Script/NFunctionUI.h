/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __CFUNCTIONUI_H__
#define __CFUNCTIONUI_H__
#pragma once
#include "stdafx.h"
#include "FCUIXml.h"

class NFunctionUI : public CFunction{
private:
    FCScript *m_indicator;
    FCUIXml *m_xml;
public:
    NFunctionUI(FCScript *indicator, int cid, const String& name, FCUIXml *xml);
    virtual ~NFunctionUI();
public:
    static void addFunctions(FCScript *indicator, FCUIXml *xml);
    virtual double onCalculate(CVariable *var);
public:
	double ADDBARRAGE(CVariable *var);
	double ALERT(CVariable *var);
	double CLOSEWINDOW(CVariable *var);
	double GETCOOKIE(CVariable *var);
    double GETPROPERTY(CVariable *var);
    double GETSENDER(CVariable *var);
    double INVALIDATE(CVariable *var);
	double SETCOOKIE(CVariable *var);
	double SETPROPERTY(CVariable *var);
	double SHOWRIGHTMENU(CVariable *var);
    double SHOWWINDOW(CVariable *var);
    double STARTTIMER(CVariable *var);
	double STOPTIMER(CVariable *var);
};

#endif

