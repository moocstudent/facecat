/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCCLIENTSOCKETS_H__
#define __FCCLIENTSOCKETS_H__
#pragma once
#include "stdafx.h"
using namespace std;

namespace FaceCat{
	typedef void (*RecvMsg)(int socketID, int localSID, const char *str, int len);
    
	typedef void (*WriteClientLog)(int socketID, int localSID, int state, const char *log);
    
    /*
     * 客户端套接字连接管理
     */
	class FCClientSockets{
	public:
		static void recvServerMsg(int socketID, int localSID, const char *str, int len);
		static void writeLog(int socketID, int localSID, int state, const char *log);
	public:
		static int close(int socketID);
		static int connect(int type, int proxyType, const char *ip, int port, const char *proxyIp, int proxyPort, const char *proxyUserName, const char *proxyUserPwd, const char *proxyDomain, int timeout);
		static void registerLog(WriteClientLog writeLogCallBack);
		static void registerRecv(RecvMsg recvMsgCallBack);
		static int send(int socketID, const char *str, int len);
        static int sendTo(int socketID, const char *str, int len);
	};
}

#endif
