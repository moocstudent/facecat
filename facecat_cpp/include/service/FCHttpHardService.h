/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */


#ifndef __FCHTTPHARDSERVICE_H__
#define __FCHTTPHARDSERVICE_H__
#pragma once
#include "..\\..\\stdafx.h"
#include <string>
#include <map>
#include "FCServerService.h"

#define SERVICEID_HTTPHARD 20
#define FUNCTIONID_HTTPHARD_TEST 0

namespace FaceCat{
    /**
	 * HTTP服务
	 */
	class FCHttpHardService:FCServerService{
	private:
	public:
	    /**
		 * 创建HTTP服务
		 */
		FCHttpHardService();
		/*
		* 析构函数
		*/
		virtual ~FCHttpHardService();
	public:
		/**
		* 接收数据
		* @param  message  消息
		*/
		virtual void onReceive(FCMessage *message);
		/**
		* 发送消息
		* @param  message  消息
		*/
		int send(FCMessage *message);
	};
}

#endif