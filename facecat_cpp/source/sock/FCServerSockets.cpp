#include "..\\..\\stdafx.h"
#include "..\\..\\include\\sock\\FCServerSocket.h"
#include "..\\..\\include\\sock\\FCServerSockets.h"

namespace FaceCat{
	map<int, FCServerSocket*> m_servers;

	RecvMsg m_recvClientMsgCallBack = 0;

	WriteServerLog m_writeServerLogCallBack = 0;
	
	void FCServerSockets::recvClientMsg(int socketID, int localSID, const char *str, int len){
		if(m_recvClientMsgCallBack){
			m_recvClientMsgCallBack(socketID, localSID, str, len);
		}
	}

	void FCServerSockets::writeLog(int socketID, int localSID, int state, const char *log){
		if(m_writeServerLogCallBack){
			m_writeServerLogCallBack(socketID, localSID, state, log);
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////

	int FCServerSockets::close(int socketID){
		map<int, FCServerSocket*>::iterator sIter = m_servers.find(socketID);
		if(sIter != m_servers.end()){
			FCServerSocket *server = sIter->second;
			int ret = server->close(socketID);
			m_servers.erase(sIter);
			delete server;
			server = 0;
			return ret;
		}
		return SOCKET_ERROR;
	}

	int FCServerSockets::closeClient(int socketID, int clientSocketID){
		map<int, FCServerSocket*>::iterator sIter = m_servers.find(socketID);
		if(sIter != m_servers.end()){
			FCServerSocket *server = sIter->second;
			int ret = server->closeClient(clientSocketID);
			return ret;
		}
		return SOCKET_ERROR;
	}
	
	void FCServerSockets::registerLog(WriteServerLog writeLogCallBack){
		m_writeServerLogCallBack = writeLogCallBack;
	}

	void FCServerSockets::registerRecv(RecvMsg recvMsgCallBack){
		m_recvClientMsgCallBack = recvMsgCallBack;
	}

	int FCServerSockets::send(int socketID, const char *str, int len){
		return FCServerSocket::send(socketID, str, len);
	}

	int FCServerSockets::sendTo(int socketID, const char *str, int len){
		map<int, FCServerSocket*>::iterator sIter = m_servers.find(socketID);
		if(sIter != m_servers.end()){
			FCServerSocket *server = sIter->second;
			return server->sendTo(str, len);
		}
		return -1;
	}

	int FCServerSockets::start(int type, int port){
		FCServerSocket *server = new FCServerSocket;
		int socketID = 0;
		if(type == 0){
			socketID = server->startTCP(port);
		}
		else{
			socketID = server->startUDP(port);
		}
		if(socketID <= 0){
			delete server;
			server = 0;
		}
		else{
			m_servers[socketID] = server;
			server->m_writeLogEvent = writeLog;
			server->m_recvEvent = recvClientMsg;
		}
		return socketID;
	}
}