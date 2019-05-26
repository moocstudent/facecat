#include "..\\..\\stdafx.h"
#include "..\\..\\include\\sock\\FCClientSocket.h"
#include "..\\..\\include\\sock\\FCClientSockets.h"

namespace FaceCat{
	map<int, FCClientSocket*> m_clients;

	RecvMsg m_recvServerMsgCallBack = 0;

	WriteClientLog m_writeClientLogCallBack = 0;
	
	void FCClientSockets::recvServerMsg(int socketID, int localSID, const char *str, int len){
		if(m_recvServerMsgCallBack){
			m_recvServerMsgCallBack(socketID, localSID, str, len);
		}
	}

	void FCClientSockets::writeLog(int socketID, int localSID, int state, const char *log){
		if(m_writeClientLogCallBack){
			m_writeClientLogCallBack(socketID, localSID, state, log);
		}
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////

	int FCClientSockets::close(int socketID){
		map<int, FCClientSocket*>::iterator sIter = m_clients.find(socketID);
		if(sIter != m_clients.end()){
			FCClientSocket *client = sIter->second;
			int ret = client->close(socketID);
			m_clients.erase(sIter);
			client = 0;
			return ret;
		}
		return SOCKET_ERROR;
	}

	int FCClientSockets::connect(int type, int proxyType, const char *ip, int port, const char *proxyIp, int proxyPort, const char *proxyUserName, const char *proxyUserPwd, const char *proxyDomain, int timeout){
		FCClientSocket *client = new FCClientSocket(type, proxyType, ip, port, proxyIp, proxyPort, proxyUserName, proxyUserPwd, proxyDomain, timeout);
		ConnectStatus ret = client->connect();
		if(ret != CONNECT_SERVER_FAIL){
			int socketID = (int)client->m_hSocket;
			m_clients[socketID] = client;
			client->m_writeLogEvent = writeLog;
			client->m_recvEvent = recvServerMsg;
			return socketID;
		}
		else{
			delete client;
			client = 0;
			return -1;
		}
	}

	void FCClientSockets::registerLog(WriteClientLog writeLogCallBack){
		m_writeClientLogCallBack = writeLogCallBack;
	}

	void FCClientSockets::registerRecv(RecvMsg recvMsgCallBack){
		m_recvServerMsgCallBack = recvMsgCallBack;
	}

	int FCClientSockets::send(int socketID, const char *str, int len){
		return FCClientSocket::send(socketID, str, len);
	}

	int FCClientSockets::sendTo(int socketID, const char *str, int len){
		map<int, FCClientSocket*>::iterator sIter = m_clients.find(socketID);
		if(sIter != m_clients.end()){
			FCClientSocket *client = sIter->second;
			return client->sendTo(str, len);
		}
		return -1;
	}
}