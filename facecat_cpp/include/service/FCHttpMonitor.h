/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */


#ifndef __FCHTTPMONITOR_H__
#define __FCHTTPMONITOR_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\core\\FCBinary.h"
#include <string>
#include <map>
#pragma comment(lib, "ws2_32.lib")
#include "winsock2.h"
#include "FCHttpEasyService.h"
using namespace std;

namespace FaceCat
{
	class FCHttpEasyService;

    /**
	 * Http数据
	 */
	class FCHttpData
	{
	public:
	    /**
		 * 文件Http服务
		 */
		FCHttpData();
		/*
		* 析构函数
		*/
		~FCHttpData();
		SOCKADDR_IN m_addr;
		char *m_body;
        bool m_close;
		int m_contentLength;
        String m_contentType;
        String m_method;
		map<String, String> m_parameters;
		String m_remoteIP;
        int m_remotePort;
		char* m_resBytes;
		int m_resBytesLength;
		String m_resStr;
		SOCKET m_socket;
		int m_statusCode;	
        String m_url;
	};

	/*
	* HTTP监听
	*/
	class FCHttpMonitor
	{
	public:
		SOCKADDR_IN m_addr;
		/**
		 * 文件名称
		 */
		string m_fileName;
		/*
		* 套接字
		*/
		SOCKET m_hSocket;
		/**
		 * 数据集合
		 */
		map<int, FCHttpData*> m_httpDatas;
		/**
		 * 脚本集合
		 */
		stack<FCScript*> m_indicators;
		/**
		 * 主脚本
		 */
		FCScript *m_indicator;
		/*
		* 全局锁
		*/
		FCLock m_lock;
		/*
		* HTTP锁
		*/
		FCLock m_lockHttpData;
		/**
		 * XML对象
		 */
		FCNative *m_native;
		/**
		 * 端口
		 */
		int m_port;
		/**
		 * 脚本
		 */
		String m_script;
		/**
		 * 使用脚本
		 */
		bool m_useScript;
	public:
		/*
		* 构造函数
		*/
		FCHttpMonitor(string fileName);
		/*
		* 析构函数
		*/
		virtual ~FCHttpMonitor();
		/*
		* 获取简单服务
		*/
		static FCHttpEasyService* getEasyService(const String& name);
		/**
		 * 获取主脚本
		 */
		FCScript* getIndicator();
		/**
		 * 获取主监视器
		 */
		static FCHttpMonitor* getMainMonitor();
		/**
		 * 获取XML对象
		 */
		FCNative* getNative();
		/**
		 * 获取端口
		 */
		int getPort();
		/**
		 * 设置端口
		 */
		void setPort(int port);
		/**
		 * 获取脚本
		 */
		String getScript();
		/**
		 * 设置脚本
		 */
		void setScript(String script);
		/*
		* 获取套接字
		*/
		SOCKET getSocket();
		/**
		 * 是否使用脚本
		 */
		bool getUseScript();
		/*
		* 设置HTTP数据
		*/
		void setHttpData(int socketID, FCHttpData *data);
		/*
		* 弹出指标
		*/
		FCScript* popCIndicator();
	public:
	    /**
		 * 检查脚本
		 */
		void checkScript();
		/*
		* 关闭服务
		*/
		int close(int socketID);
		/**
		 * 启动监听
		 */
		int start(string fileName);
	};
}

#endif