/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCSERVERSERVICE_H__
#define __FCSERVERSERVICE_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\core\\FCBinary.h"
#include <string>
#include <map>
using namespace std;

#define  COMPRESSTYPE_NONE			  0
#define  COMPRESSTYPE_GZIP			  1

static int m_requestID = 10000;

namespace FaceCat{	
	/**
	 * 消息结构
	 */
	struct FCMessage{
	public:
	    /**
		 * 创建消息
		 */
		FCMessage();
		/**
		* 创建消息
		* @param  groupID  组ID
		* @param  serviceID  服务ID
		* @param  functionID  功能ID
		* @param  sessionID  登录ID
		* @param  requestID  请求ID
		* @param  socketID  连接ID
		* @param  state  状态
		* @param  compressType  压缩类型
		* @param  bodyLength  包体长度
		* @param  body  包体
		*/
		FCMessage(int groupID, int serviceID, int functionID, int sessionID, int requestID, int socketID, int state, int compressType, int bodyLength, char *body);
		int m_groupID;   
		int m_serviceID;   
		int m_functionID;  
		int m_sessionID;   
		int m_requestID;   
		int m_socketID; 
		int m_state;
		int m_compressType;
		int m_bodyLength;  
		char *m_body;
		/**
		 * 复制数据
		 */
		void copy(FCMessage *message)
		{
			m_groupID = message->m_groupID;
			m_serviceID = message->m_serviceID;
			m_functionID = message->m_functionID;
			m_sessionID = message->m_sessionID;
			m_requestID= message->m_requestID;
			m_socketID = message->m_socketID;
			m_state = message->m_state;
			m_compressType = message->m_compressType;
			m_bodyLength = message->m_bodyLength;
			m_body = message->m_body;
		}
	};

	typedef void (__stdcall *MessageCallBack)(int socketID, int localSID, const char *str, int len);

	typedef void (__stdcall *WriteLogCallBack)(int socketID, int localSID, int state, const char *log);

	typedef void (__stdcall *ListenerMessageCallBack)(FCMessage *message, Object pInvoke);

	typedef int (*funcCloseServer)(int);

	typedef void (*funcRegisterCallBack)(MessageCallBack);

	typedef void (*funcRegisterLog)(WriteLogCallBack);

	typedef int (*funcSendByServer)(int, const char*, int);

	typedef int (*funcStartServer)(int, int);

	/*
	* 消息监听
	*/
	class FCMessageListener{
	private:
	    /**
		 * 监听回调列表
		 */
		vector<ListenerMessageCallBack> m_callBacks;
		vector<Object> m_callBackInvokes;
	public:
	    /**
		 * 创建消息监听
		 */
		FCMessageListener();
		virtual ~FCMessageListener();
	public:
	    /**
		 * 添加回调
		 */
		void add(ListenerMessageCallBack callBack, Object pInvoke);
		/**
		 * 回调方法
		 */
		void callBack(FCMessage *message);
		/**
		 * 清除监听
		 */
		void clear();
		/**
		 * 移除回调
		 */
		void remove(ListenerMessageCallBack callBack);
	};

	/*
	* 套接字组
	*/
	class FCSocketArray{
	private:
	    /**
		 * 套接字ID组
		 */
		vector<int> m_sockets;
	public:
		FCSocketArray();
		virtual ~FCSocketArray();
	public:
	    /**
		 * 添加套接字ID
		 */
		void addSocket(int socketID);
		/**
		* 获取套接字列表
		* @param  socketList  套接字列表
		*/
		void getSocketList(vector<int> *socketList);
		/**
		 * 移除套接字ID
		 */
		void removeSocket(int socketID);
	};

	/*
	* 服务端SOCKET服务
	*/
	class FCServerService{
	protected:
		/*
		* 压缩类型
		*/
		int m_compressType;
		/**
		 * 压缩类型集合
		 */
		map<int, int> m_compressTypes;
		/**
		 * 组ID
		 */
		int m_groupID;
		/**
		 * 监听者集合
		 */
		map<int, FCMessageListener*> m_listeners;
		/**
		 * 服务的ID
		 */
		int m_serviceID;
		/**
		 * 登录ID
		 */
		int m_sessionID;
		/**
		 * 等待消息队列
		 */
		map<int, FCMessage*> m_waitMessages;
	protected:
		FCLock m_lock;
	public:
		/**
		* 关闭
		* @param  socketID  连接ID
		*/
		static int closeServer(int socketID);
		/**
		* 注册回调
		* @param  callBack  回调函数
		*/
		static void registerCallBack(MessageCallBack callBack);
		/**
		* 注册日志
		* @param  callBack  回调函数
		*/
		static void registerLog(WriteLogCallBack callBack);
		/**
		* 发送消息
		* @param  socketID  连接ID
		* @param  str  数据
		* @param  len  长度
		*/
		static int sendByServer(int socketID, const char *str, int len);
		/**
		* 启动
		* @param  port  端口
		*/
		static int startServer(int type, int port);
	public:
		FCServerService();
		virtual ~FCServerService();
		/**
		 * 获取压缩类型
		 */
		int getCompressType();
		/**
		 * 设置压缩类型
		 */
		void setCompressType(int compressType);
		/**
		 * 获取下载流量
		 */
		static Long getDownFlow();
		/**
		 * 设置下载流量
		 */
		static void setDownFlow(Long downFlow);
		/**
		 * 获取组ID
		 */
		int getGroupID();
		/**
		 * 设置组ID
		 */
		void setGroupID(int groupID);
		/**
		 * 获取服务的ID
		 */
		int getServiceID();
		/**
		 * 设置服务的ID
		 */
		void setServiceID(int serviceID);
		/**
		 * 获取登录ID
		 */
		int getSessionID();
		/**
		 * 设置登录ID
		 */
		void setSessionID(int sessionID);
		/**
		 * 获取上传流量
		 */
		static Long getUpFlow();
		/**
		 * 设置上传流量
		 */
		static void setUpFlow(Long upFlow);
	public:
		/**
		* 添加服务
		* @param  service  服务
		*/
		static void addService(FCServerService *service);
		/*
		* 数据回调
		*/
		static void __stdcall callBack(int socketID, int localSID, const char *str, int len);
		/**
		 * 获取请求ID
		 */
		static int getRequestID();
		/**
		* 获取所有的服务
		* @param  service  服务
		*/
		static void getServices(vector<FCServerService*> *services);
		/**
		* 保持活跃
		* @param  socketID  套接字ID
		*/
		virtual int keepAlive(int socketID);
		/**
		* 收到消息
		* @param  br  流
		* @param  socketID  套接字ID
		* @param  localSID  本地套接字ID
		* @param  len  长度
		*/
		virtual void onCallBack(FCBinary *binary, int socketID, int localSID, int len);
		/**
		* 客户端关闭方法
		* @param  socketID  连接ID
		* @param  localSID  本地连接ID
		*/
		virtual void onClientClose(int socketID, int localSID);
		/**
		* 接收数据
		* @param  message  消息
		*/
		virtual void onReceive(FCMessage *message);
		/**
		 * 等待消息的处理
		 */
		virtual void onWaitMessageHandle(FCMessage *message);
		/**
		 * 注册数据监听
		 */
		virtual void registerListener(int requestID, ListenerMessageCallBack callBack, Object pInvoke);
		/**
		 * 注册等待
		 */
		virtual void registerWait(int requestID, FCMessage *message);
		/**
		 * 发送消息
		 */
		virtual int send(FCMessage *message);
		/**
		 * 发送到监听者
		 */
		virtual void sendToListener(FCMessage *message);
		/**
		 * 取消注册数据监听
		 */
		virtual void unRegisterListener(int requestID);
		/**
		 * 取消注册监听
		 */
		virtual void unRegisterListener(int requestID, ListenerMessageCallBack callBack);
		/**
		 * 取消注册等待
		 */
		virtual void unRegisterWait(int requestID);
		/**
		 * 等待消息
		 */
		virtual int waitMessage(int requestID, int timeout);
		/**
		* 写日志
		* @param  socketID  连接ID
		* @param  localSID  本地连接ID
		* @param  state  状态
		* @param  log  日志
		*/
		static void __stdcall writeServerLog(int socketID, int localSID, int state, const char *log);
	};
}

#endif