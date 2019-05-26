/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __CFUNCTIONHTTP_H__
#define __CFUNCTIONHTTP_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "FCHttpMonitor.h"
#include "FCHttpEasyService.h"

namespace FaceCat{
	/*
	* HTTP库
	*/
	class CFunctionHttp : public CFunction{
	private:
	    /**
		 * HTTP对象
		 */
		FCHttpData *m_data;
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
		CFunctionHttp(FCScript *indicator, int cid, const String& name);
		/*
		* 析构函数
		*/
		virtual ~CFunctionHttp();
		/**
		 * 获取HTTP对象
		 */
		FCHttpData* getHttpData();
		/**
		 * 设置HTTP对象
		 */
		void setHttpData(FCHttpData *data);
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
		 * 添加前缀
		 */
		double HTTP_ADDPORT(CVariable *var);
		/**
		 * 检查脚本
		 */
		double HTTP_CHECKSCRIPT(CVariable *var);
		/**
		 * HTTP关闭
		 */
		double HTTP_CLOSE(CVariable *var);
		/**
		 * 接受GET请求
		 */
		double HTTP_EASYREQUEST(CVariable *var);
		/**
		 * 获取内容类型方法
		 */
		double HTTP_GETCONTENTTYPE(CVariable *var);
		/**
		 * 获取IP
		 */
		double HTTP_GETREMOTEIP(CVariable *var);
		/**
		 * 获取Port
		 */
		double HTTP_GETREMOTEPORT(CVariable *var);
		/**
		 * 获取请求方法
		 */
		double HTTP_GETREQUESTMETHOD(CVariable *var);
		/**
		 * 获取请求URL
		 */
		double HTTP_GETREQUESTURL(CVariable *var);
		/**
		 * 获取服务名称
		 */
		double HTTP_GETSERVICENAME(CVariable *var);
		/**
		 * 接受流数据
		 */
		double HTTP_HARDREQUEST(CVariable *var);
		/**
		 * 接受流数据
		 */
		double HTTP_POSTREQUEST(CVariable *var);
		/**
		 * HTTP获取POST请求的参数
		 */
		double HTTP_POSTSTRING(CVariable *var);
		/**
		 * HTTP获取GET请求的参数
		 */
		double HTTP_QUERYSTRING(CVariable *var);
		/**
		 * 设置响应状态码
		 */
		double HTTP_SETSTATUSCODE(CVariable *var);
		/**
		 * HTTP响应写流
		 */
		double HTTP_WRITE(CVariable *var);
	};
}
#endif

