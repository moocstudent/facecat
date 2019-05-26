/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCSERVERSOCKET_H__
#define __FCSERVERSOCKET_H__
#pragma once
#include "..\\..\\stdafx.h"
#pragma comment(lib, "ws2_32.lib")
#include "winsock2.h"
#include "FCServerSockets.h"
#pragma comment (lib,"winmm")

namespace FaceCat{
	typedef enum{
		RECV_POSTED
	}OPERATION_TYPE;

	typedef struct
	{
		WSAOVERLAPPED  overlap;
		WSABUF         Buffer;
		char           szMessage[1024];
		DWORD          NumberOfBytesRecvd;
		DWORD          Flags;
		OPERATION_TYPE OperationType;
	}PER_IO_OPERATION_DATA, *LPPER_IO_OPERATION_DATA;

	class SOCKDATA{
	public:
		SOCKDATA();
		~SOCKDATA();
		char *m_buffer;
		int m_bufferRemain;
		bool m_get;
		int m_head;
		int m_headSize;
		char m_headStr[4];
		int m_index;
		int m_len;
		int m_pos;
		SOCKET m_socket;
		char *m_str;
		int m_strRemain;
		bool m_submit;
	};

	/*
	* 服务端套接字连接
	*/
	class FCServerSocket{
	public:
		SOCKADDR_IN m_addr;
		HANDLE m_completionPort;
		vector<SOCKDATA*> m_datas;
		SOCKET m_hSocket;
		int m_port;
		RecvMsg m_recvEvent;
		WriteServerLog m_writeLogEvent;
	public:
		FCServerSocket();
		virtual ~FCServerSocket();
		int close(int socketID);
		int closeClient(int socketID);
		int recv(SOCKDATA *data);
		static int send(int socketID, const char *str, int len);
		int sendTo(const char *str, int len);
		int startTCP(int port);
		int startUDP(int port);
		void writeLog(int socketID, int localSID, int state, const char *log);
	public:
		static void wSStart();
		static void wSStop();
	};
}
#endif