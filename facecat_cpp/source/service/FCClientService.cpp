#include "..\\..\\stdafx.h"
#include "..\\..\\include\\service\\FCClientService.h"
#include "..\\..\\include\\core\\FCBinary.h"
#include "..\\..\\include\\sock\\FCClientSockets.h"

namespace FaceCat{
	long long m_downFlowClient = 0;

	long long m_upFlowClient = 0;

	MessageCallBack m_messageCallBackClient;

	map<int, FCClientService*> m_servicesClient;

	int FCClientService::closeClient(int socketID){
		return FCClientSockets::close(socketID);
	}

	int FCClientService::connectToServer(int type, int proxyType, const char *ip, int port, const char *proxyIp, int proxyPort, const char *proxyUserName, const char *proxyUserPwd, const char *proxyDomain, int timeout){
		return FCClientSockets::connect(type, proxyType, ip, port, proxyIp, proxyPort, proxyUserName, proxyUserPwd, proxyDomain, timeout);
	}

	void FCClientService::registerCallBack(MessageCallBack callBack){
		return FCClientSockets::registerRecv(callBack);
	}

	void FCClientService::registerLog(WriteLogCallBack callBack){
		return FCClientSockets::registerLog(callBack);
	}

	int FCClientService::sendByClient(int socketID, const char *str, int len){
		return FCClientSockets::send(socketID, str, len);
	}

	FCClientService::FCClientService(){
		m_compressType = COMPRESSTYPE_GZIP;
		m_groupID = 0;
		m_serviceID = 0;
		m_sessionID = 0;
	}

	FCClientService::~FCClientService(){
		m_lock.lock();
		map<int, FCMessageListener*>::iterator sIter = m_listeners.begin();
		for(; sIter != m_listeners.end(); ++sIter){
			delete sIter->second;
		}
		m_listeners.clear();
		m_waitMessages.clear();
		m_lock.unLock();
	}

	int FCClientService::getCompressType(){
		return m_compressType;
	}

	void FCClientService::setCompressType(int compressType){
		m_compressType = compressType;
	}

	long long FCClientService::getDownFlow(){
		return m_downFlowClient;
	}

	void FCClientService::setDownFlow(long long downFlow){
		m_downFlowClient = downFlow;
	}

	int FCClientService::getGroupID(){
		return m_groupID;
	}

	void FCClientService::setGroupID(int groupID){
		m_groupID = groupID;
	}

	int FCClientService::getServiceID(){
		return m_serviceID;
	}

	void FCClientService::setServiceID(int serviceID){
		m_serviceID = serviceID;
	}

	int FCClientService::getSessionID(){
		return m_sessionID;
	}

	void FCClientService::setSessionID(int sessionID){
		m_sessionID = sessionID;
	}

	long long FCClientService::getUpFlow(){
		return m_upFlowClient;
	}

	void FCClientService::setUpFlow(long long upFlow){
		m_upFlowClient = upFlow;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void FCClientService::addService(FCClientService *service){
		if (!m_messageCallBackClient){
			MessageCallBack messageCallBack = &callBack;
			m_messageCallBackClient = messageCallBack;
			registerCallBack(messageCallBack);
		}
		m_servicesClient[service->getServiceID()] = service;
	}

	void FCClientService::callBack(int socketID, int localSID, const char *str, int len){
		m_downFlowClient += len;
		if (len > 4){
			FCBinary binary;
			binary.write(str, len);
			int head = binary.readInt();
			int groupID = (int)binary.readShort();
			int serviceID = (int)binary.readShort();
			FCClientService *service = 0;
			if(m_servicesClient.find(serviceID) != m_servicesClient.end()){
				m_servicesClient[serviceID]->onCallBack(&binary, socketID, localSID, len);
			}
		}
	}

	int FCClientService::getRequestID(){
		return m_requestID ++;
	}

	void FCClientService::getServices(vector<FCClientService*> *services){
		map<int, FCClientService*>::iterator sIter = m_servicesClient.begin();
		for(; sIter != m_servicesClient.end(); ++sIter){
			services->push_back(sIter->second);
		}
	}

	int FCClientService::keepAlive(int socketID){
		FCBinary binary;
		binary.initialize(4, false);
		binary.writeInt(4);
		char *str = (char*)binary.getBytes();
		return sendByClient(socketID, str, 4);
	}

	void FCClientService::onCallBack(FCBinary *binary, int socketID, int localSID, int len){
		int headSize = sizeof(int) * 4 + sizeof(short) * 3 + sizeof(byte) * 2;
		int functionID = (int)binary->readShort();
		int sessionID = binary->readInt();
		int requestID = binary->readInt();
		int state = (int)binary->readChar();
		int compressType = (int)binary->readChar();
		int uncBodyLength = binary->readInt();
		int bodyLength = len - headSize;
		char *body = new char[bodyLength];
		memset(body, '\0', bodyLength);
		binary->read(body, bodyLength);
		if (compressType == COMPRESSTYPE_GZIP){
			//uLong deLen = uncBodyLength;
			//char *buffer = new char[deLen];
			//memset(buffer, '\0', deLen);
			//CStrA::GZDeCompress((Byte*)body, bodyLength, (Byte*)buffer, &deLen);
			//delete[] body;
			//body = buffer;
		}
		FCMessage message(getGroupID(), getServiceID(), functionID, sessionID, requestID, socketID, state, compressType, uncBodyLength, body);
		onReceive(&message);
		onWaitMessageHandle(&message);
		if(message.m_body){
			delete[] body;
		}
		body = 0;
	}

	void FCClientService::onReceive(FCMessage *message){
	}

	void FCClientService::onWaitMessageHandle(FCMessage *message){
		if((int)m_waitMessages.size() > 0){
			m_lock.lock();
			map<int, FCMessage*>::iterator sIter = m_waitMessages.find(message->m_requestID);
			if(sIter != m_waitMessages.end()){
				FCMessage *waitMessage = sIter->second;
				waitMessage->copy(message);
				char *newBody = new char[message->m_bodyLength];
				for (int i = 0; i < message->m_bodyLength; i++){
					newBody[i] = message->m_body[i];
				}
				waitMessage->m_body = newBody;
			}
			m_lock.unLock();
		}
	}

	void FCClientService::registerListener(int requestID, ListenerMessageCallBack callBack, Object pInvoke){
		m_lock.lock();
		FCMessageListener *listener = 0;
		map<int, FCMessageListener*>::iterator sIter = m_listeners.find(requestID);
		if(sIter != m_listeners.end()){
			listener = sIter->second;
		}
		else{
			listener = new FCMessageListener;
			m_listeners[requestID] = listener;
		}
		listener->add(callBack, pInvoke);
		m_lock.unLock();
	}

	void FCClientService::registerWait(int requestID, FCMessage *message){
		m_lock.lock();
		m_waitMessages[requestID] = message;
		m_lock.unLock();
	}

	int FCClientService::send(FCMessage *message){
		FCBinary binary;
		char *buffer = 0;
		const char *body = message->m_body;
		int bodyLength = message->m_bodyLength;
		int uncBodyLength = bodyLength;
		if (message->m_compressType == COMPRESSTYPE_GZIP){
			//uLong cLen = 128;
			//if(uncBodyLength > cLen)
			//{
			//	cLen = uncBodyLength;
			//}
			//buffer = new char[cLen];
			//memset(buffer, '\0', cLen);
			//CStrA::GZCompress((Byte*)body, bodyLength, (Byte*)buffer, &cLen);
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
		int ret = sendByClient(message->m_socketID, str, len);
		m_upFlowClient += ret;
		return ret;
	}

	void FCClientService::sendToListener(FCMessage *message){
		FCMessageListener *listener = 0;
		Object pInvoke = 0;
		m_lock.lock();
		map<int, FCMessageListener*>::iterator sIter = m_listeners.find(message->m_requestID);
		if(sIter != m_listeners.end()){
			listener = sIter->second;
		}
		m_lock.unLock();
		if(listener){
			listener->callBack(message);
		}
	}

	void FCClientService::unRegisterListener(int requestID){
		m_lock.lock();
		map<int, FCMessageListener*>::iterator sIter = m_listeners.find(requestID);
		if(sIter != m_listeners.end()){
			delete sIter->second;
			m_listeners.erase(sIter);
		}
		m_lock.unLock();
	}

	void FCClientService::unRegisterListener(int requestID, ListenerMessageCallBack callBack){
		m_lock.lock();
		map<int, FCMessageListener*>::iterator sIter = m_listeners.find(requestID);
		if(sIter != m_listeners.end()){
			sIter->second->remove(callBack);
		}
		m_lock.unLock();
	}

	void FCClientService::unRegisterWait(int requestID){
		m_lock.lock();
		map<int, FCMessage*>::iterator sIter = m_waitMessages.find(requestID);
		if(sIter != m_waitMessages.end()){
			m_waitMessages.erase(sIter);
		}
		m_lock.unLock();
	}

	int FCClientService::waitMessage(int requestID, int timeout){
		int state = 0;
		while(timeout > 0){
			m_lock.lock();
			map<int, FCMessage*>::iterator sIter = m_waitMessages.find(requestID);
			if(sIter != m_waitMessages.end()){
				FCMessage *message = sIter->second;
				if(message->m_bodyLength > 0){
					state = 1;
					m_lock.unLock();
					break;
				}
			}
			else{
				m_lock.unLock();
				break;
			}
			m_lock.unLock();
			timeout -= 10;
			Sleep(10);
		}
		unRegisterWait(requestID);
		return state;
	}
}