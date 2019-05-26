#include "..\\..\\stdafx.h"
#include "..\\..\\include\\service\\FCHttpHardService.h"
#include "..\\..\\include\\service\\FCHttpMonitor.h"

namespace FaceCat{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	FCHttpHardService::FCHttpHardService(){
		setServiceID(SERVICEID_HTTPHARD);
	}

	FCHttpHardService::~FCHttpHardService(){
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////	

	void FCHttpHardService::onReceive(FCMessage *message){
		FCServerService::onReceive(message);
		FCServerService::sendToListener(message); 	
	}

	int FCHttpHardService::send(FCMessage *message){
		FCBinary binary;
		char *buffer = 0;
		const char *body = message->m_body;
		int bodyLength = message->m_bodyLength;
		int uncBodyLength = bodyLength;
		m_lock.lock();
		map<int, int>::iterator sIter = m_compressTypes.find(message->m_socketID);
		if(sIter != m_compressTypes.end()){
			message->m_compressType = sIter->second;
		}
		m_lock.unLock();
		if (message->m_compressType == COMPRESSTYPE_GZIP){
			//uLong cLen = bodyLength;
			//buffer = new char[cLen];
			//memset(buffer, '\0', cLen);
			////FCStr::GZCompress((Byte*)body, bodyLength, (Byte*)buffer, &cLen);
			//body = buffer;
			//bodyLength = (int)cLen;
		}
		int len = sizeof(int) * 4 + bodyLength + sizeof(short) * 3 + sizeof(char) * 2;
		binary.initialize(len, false);
		binary.writeInt(len);
		binary.writeShort((short)message->m_groupID);
		binary.writeShort((short)message->m_serviceID);
		binary.writeShort((short)message->m_functionID);
		binary.writeInt(message->m_sessionID);
		binary.writeInt(message->m_requestID);
		binary.writeChar((char)message->m_state);
		binary.writeChar((char)message->m_compressType);
		binary.writeInt(uncBodyLength);
		binary.write(body, bodyLength);
		char *str = (char*)binary.getBytes();
		if(buffer){
			delete[] buffer;
			buffer = 0;
		}
		int ret = -1;
		FCHttpMonitor *nodeService = FCHttpMonitor::getMainMonitor();
		nodeService->m_lockHttpData.lock();
		if(nodeService->m_httpDatas.find(message->m_socketID) != nodeService->m_httpDatas.end()){
			nodeService->m_httpDatas[message->m_socketID]->m_resBytes = str;
			nodeService->m_httpDatas[message->m_socketID]->m_resBytesLength = len;
			ret = 1;
		}
		nodeService->m_lockHttpData.unLock();
		return ret;
	}
}