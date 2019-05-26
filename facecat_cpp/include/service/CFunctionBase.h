/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __CFUNCTIONBASE_H__
#define __CFUNCTIONBASE_H__
#pragma once
#include "..\\..\\stdafx.h"

namespace FaceCat{
	/*
	* 基础方法库
	*/
	class CFunctionBase : public CFunction{
	private:
	    /**
		 * 指标
		 */
		FCScript *m_indicator;
	public:
		/**
		* 创建方法
		* @param  indicator  指标
		* @param  id  ID
		* @param  name  名称
		*/
		CFunctionBase(FCScript *indicator, int cid, const String& name);
		/*
		* 析构函数
		*/
		virtual ~CFunctionBase();
	public:
		/**
		* 添加方法
		* @param  indicator  方法库
		*/
		static void addFunctions(FCScript *indicator);
		/**
		* 计算
		* @param  var  变量
		*/
		virtual double onCalculate(CVariable *var);
	public:
		/**
		* 输入函数
		* @param  var  变量
		*/
		double INPUT(CVariable *var);
		/**
		* 输出函数
		* @param  var  变量
		*/
		double OUTPUT(CVariable *var);
		/**
		* 睡眠
		* @param  var  变量
		*/
		double SLEEP(CVariable *var);
	};
}

#endif

