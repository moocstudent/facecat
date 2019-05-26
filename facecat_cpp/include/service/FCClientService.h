/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCCLIENTSERVICE_H__
#define __FCCLIENTSERVICE_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\core\\FCBinary.h"
#include "FCServerService.h"
#include <string>
#include <map>
using namespace std;

#define  DOWNHELPER_AGENTNAME         "SECURITY"  
#define  LEN_OF_BUFFER_FOR_QUERYINFO  128  
#define  MAX_DOWNLOAD_REQUEST_TIME    10   

namespace FaceCat{
	/*
	* 客户端SOCKET服务
	*/
	class FCClientService{
	private:
		FCLock m_lock;
		/**
		 * 压缩类型
		 */
		int m_compressType;
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
	public:
		/**
		* 关闭
		* @param  socketID  连接ID
		*/
		static int closeClient(int socketID);
		/**
		* 连接
		* @param  proxyType  代理类型
		* @param  ip  地址
		* @param  port  端口
		* @param  proxyIp  代理地址
		* @param  proxyPort  代理端口
		* @param  proxyUserName  代理名称
		* @param  proxyUserPwd  代理密码
		* @param  proxyDomain  代理域
		* @param  timeout  超时
		*/
		static int connectToServer(int type, int proxyType, const char *ip, int port, const char *proxyIp, int proxyPort, const char *proxyUserName, const char *proxyUserPwd, const char *proxyDomain, int timeout);
		/**
		 * 注册回调
		 */
		static void registerCallBack(MessageCallBack callBack);
		/**
		 * 注册日志
		 */
		static void registerLog(WriteLogCallBack callBack);
		/**
		* 发送消息
		* @param  socketID  连接ID
		* @param  str   数据
		* @param  len  长度
		*/
		static int sendByClient(int socketID, const char *str, int len);
	public:
		/*
		* 构造函数
		*/
		FCClientService();
		/*
		* 析构函数
		*/
		virtual ~FCClientService();
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
		static void addService(FCClientService *service);
		/**
		* 回调函数
		* @param  socketID  连接ID
		* @param  localSID  本地连接ID
		* @param  str  数据
		* @param  len  长度
		*/
		static void __stdcall callBack(int socketID, int localSID, const char *str, int len);
		/**
		 * 获取请求ID
		 */
		static int getRequestID();
		/**
		 * 获取所有的服务
		 */
		static void getServices(vector<FCClientService*> *services);
		/**
		 * 保持活跃
		 */
		int keepAlive(int socketID);
		/**
		 * 收到消息
		 */
		virtual void onCallBack(FCBinary *binary, int socketID, int localSID, int len);
		/**
		 * 接收数据
		 */
		virtual void onReceive(FCMessage *message);
		/**
		 * 等待消息的处理
		 */
		virtual void onWaitMessageHandle(FCMessage *message);
		/**
		* 注册数据监听
		* @param  requestID 请求ID
		* @param  callBack  回调函数
		*/
		void registerListener(int requestID, ListenerMessageCallBack callBack, Object pInvoke);
		/**
		 * 注册等待
		 */
		void registerWait(int requestID, FCMessage *message);
		/**
		 * 发送消息
		 */
		int send(FCMessage *message);
		/**
		 * 发送到监听者
		 */
		void sendToListener(FCMessage *message);
		/**
		 * 取消注册数据监听
		 */
		void unRegisterListener(int requestID);
		/**
		 * 取消注册监听
		 */
		void unRegisterListener(int requestID, ListenerMessageCallBack callBack);
		/**
		 * 取消注册等待
		 */
		void unRegisterWait(int requestID);
		/**
		 * 等待消息
		 */
		int waitMessage(int requestID, int timeout);
	};
}
#endif