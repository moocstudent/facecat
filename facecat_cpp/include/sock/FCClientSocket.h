/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCCLIENTSOCKET_H__
#define __FCCLIENTSOCKET_H__
#pragma once
#include "..\\..\\stdafx.h"
#pragma comment(lib, "ws2_32.lib")
#include "winsock2.h"
#include <time.h> 
#include <string>  
#include <vector>  
#include "CBase64.h"
#include "FCClientSockets.h"
using namespace std;  

namespace FaceCat{ 
	enum ConnectStatus{  
		SUCCESS,  
		CONNECT_PROXY_FAIL,  
		NOT_CONNECT_PROXY,  
		CONNECT_SERVER_FAIL  
	};

	struct TSock4req1{   
		char VN;   
		char CD;   
		unsigned short Port;   
		unsigned long IPAddr;   
		char other;   
	};   
  
	struct TSock4ans1{   
		char VN;   
		char CD;   
	};  
  
	struct TSock5req1{   
		char Ver;   
		char nMethods;   
		char Methods;   
	};   
  
	struct TSock5ans1{   
		char Ver;   
		char Method;   
	};   
  
	struct TSock5req2{   
		char Ver;   
		char Cmd;   
		char Rsv;   
		char Atyp;   
		char other;   
	};   
  
	struct TSock5ans2{   
		char Ver;   
		char Rep;   
		char Rsv;   
		char Atyp;   
		char other;   
	};   
  
	struct TAuthreq{   
		char Ver;   
		char Ulen;   
		char Name;   
		char PLen;   
		char Pass;   
	};   
  
	struct TAuthans{   
		char Ver;   
		char Status;   
	};   
  
	/*
	* 客户端套接字连接
	*/
	class FCClientSocket{
		bool m_blnProxyServerOk;
		string m_ip;
		u_short m_port;
		string m_proxyDomain;
		int m_proxyType;  
		string m_proxyIp;  
		u_short m_proxyPort; 
		string m_proxyUserName;  
		string m_proxyUserPwd;
		int m_timeout;
	private:
		ConnectStatus connectStandard();
		ConnectStatus connectByHttp();  
		ConnectStatus connectBySock4();  
		ConnectStatus connectBySock5(); 
		void createSocket();
	public:
		SOCKADDR_IN m_addr;
		SOCKET m_hSocket;
		RecvMsg m_recvEvent;
		int m_type;
		WriteClientLog m_writeLogEvent;
	public:
		FCClientSocket(int type, int proxyType, const string &ip, u_short port, const string &proxyIp, u_short proxyPort, const string &proxyUserName, const string &proxyUserPwd, const string &proxyDomain, int timeout);
		virtual ~FCClientSocket();
	public:
		int close(int socketID);
		ConnectStatus connect(); 
		ConnectStatus connectProxyServer();
		static string getHostIP(const char* ip);
		static int send(int socketID, const char *str, int len);
		int sendTo(const char *str, int len);
		void writeLog(int socketID, int localSID, int state, const char *log);
	};  
}

#endif