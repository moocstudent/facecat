#include "..\\..\\stdafx.h"
#include "..\\..\\include\\service\\FCServerService.h"
#include "..\\..\\include\\core\\FCBinary.h"
#include "..\\..\\include\\sock\\FCServerSockets.h"

namespace FaceCat{
	long long m_downFlow = 0;

	long long m_upFlow = 0;

	MessageCallBack m_messageCallBack;

	map<int, FCServerService*> m_services;

	WriteLogCallBack m_writeLogCallBack;

	FCMessage::FCMessage(){
		m_groupID = 0;
		m_serviceID = 0;
		m_functionID = 0;
		m_sessionID = 0;
		m_requestID = 0;
		m_socketID = 0;
		m_state = 0;
		m_compressType = 0;
		m_bodyLength = 0;
		m_body = 0;
	}

	FCMessage::FCMessage(int groupID, int serviceID, int functionID, int sessionID, int requestID, int socketID, int state, int compressType, int bodyLength, char *body){
		m_groupID = groupID;
		m_serviceID = serviceID;
		m_functionID = functionID;
		m_sessionID = sessionID;
		m_requestID = requestID;
		m_socketID = socketID;
		m_state = state;
		m_compressType = compressType;
		m_bodyLength = bodyLength;
		m_body = body;
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////

	FCMessageListener::FCMessageListener(){
	}

	FCMessageListener::~FCMessageListener(){
		clear();
	}

	void FCMessageListener::add(ListenerMessageCallBack callBack, Object pInvoke){
		m_callBacks.push_back(callBack);
		m_callBackInvokes.push_back(pInvoke);
	}

	void FCMessageListener::callBack(FCMessage *message){
		int callBacksSize = (int)m_callBacks.size();
		for(int i = 0; i < callBacksSize; i++){
			m_callBacks[i](message, m_callBackInvokes[i]);
		}
	}

	void FCMessageListener::clear(){
		m_callBacks.clear();
		m_callBackInvokes.clear();
	}

	void FCMessageListener::remove(ListenerMessageCallBack callBack){
		Object pInvoke = 0;
		int pos = 0;
		vector<ListenerMessageCallBack>::iterator sIter = m_callBacks.begin();
		for(; sIter != m_callBacks.end(); ++sIter){
			if(callBack == *sIter)
			{
				m_callBacks.erase(sIter);
				pInvoke = m_callBackInvokes[pos];
				break;
			}
			pos++;
		}
		vector<Object>::iterator sIter2 = m_callBackInvokes.begin();
		for(; sIter2 != m_callBackInvokes.end(); ++sIter2){
			if(pInvoke == *sIter2){
				m_callBackInvokes.erase(sIter2);
				break;
			}
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	FCSocketArray::FCSocketArray(){
	}

	FCSocketArray::~FCSocketArray(){
	}

	void FCSocketArray::addSocket(int socketID){
		vector<int>::iterator sIter = m_sockets.begin();
		for(; sIter != m_sockets.end(); ++sIter){
			if(socketID == *sIter)
			{
				return;
			}
		}
		m_sockets.push_back(socketID);
	}

	void FCSocketArray::getSocketList(vector<int> *socketList){
		vector<int>::iterator sIter = m_sockets.begin();
		for(; sIter != m_sockets.end(); ++sIter){
			socketList->push_back(*sIter);
		}
	}

	void FCSocketArray::removeSocket(int socketID){
		vector<int>::iterator sIter = m_sockets.begin();
		for(; sIter != m_sockets.end(); ++sIter){
			if(socketID == *sIter){
				m_sockets.erase(sIter);
				return;
			}
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	int FCServerService::closeServer(int socketID){
		return FCServerSockets::close(socketID);
	}

	void FCServerService::registerCallBack(MessageCallBack callBack){
		FCServerSockets::registerRecv(callBack);
	}

	void FCServerService::registerLog(WriteLogCallBack callBack){
		FCServerSockets::registerLog(callBack);
	}

	int FCServerService::sendByServer(int socketID, const char *str, int len){
		return FCServerSockets::send(socketID, str, len);
	}

	int FCServerService::startServer(int type, int port){
		return FCServerSockets::start(type, port);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	FCServerService::FCServerService(){
		m_compressType = COMPRESSTYPE_GZIP;
		m_groupID = 0;
		m_serviceID = 0;
		m_sessionID = 0;
	}

	FCServerService::~FCServerService(){
		m_lock.lock();
		map<int, FCMessageListener*>::iterator sIter = m_listeners.begin();
		for(; sIter != m_listeners.end(); ++sIter){
			delete sIter->second;
		}
		m_listeners.clear();
		m_waitMessages.clear();
		m_lock.unLock();
	}

	int FCServerService::getCompressType(){
		return 1;
	}

	void FCServerService::setCompressType(int compressType){
		m_compressType = compressType;
	}

	long long FCServerService::getDownFlow(){
		return m_downFlow;
	}

	void FCServerService::setDownFlow(long long downFlow){
		m_downFlow = downFlow;
	}

	int FCServerService::getGroupID(){
		return m_groupID;
	}

	void FCServerService::setGroupID(int groupID){
		m_groupID = groupID;
	}

	int FCServerService::getServiceID(){
		return m_serviceID;
	}

	void FCServerService::setServiceID(int serviceID){
		m_serviceID = serviceID;
	}

	int FCServerService::getSessionID(){
		return m_sessionID;
	}

	void FCServerService::setSessionID(int sessionID){
		m_sessionID = sessionID;
	}

	long long FCServerService::getUpFlow(){
		return m_upFlow;
	}

	void FCServerService::setUpFlow(long long upFlow){
		m_upFlow = upFlow;
	}

	/////////////////////////////////////////////////////////////////////////////////////////////////////////

	void FCServerService::addService(FCServerService *service){
		if (!m_messageCallBack){
			MessageCallBack messageCallBack = &callBack;
			m_messageCallBack = messageCallBack;
			registerCallBack(messageCallBack);
			WriteLogCallBack writeLogCallBack = &writeServerLog;
			m_writeLogCallBack = writeLogCallBack;
			registerLog(m_writeLogCallBack);
		}
		m_services[service->getServiceID()] = service;
	}

	void FCServerService::callBack(int socketID, int localSID, const char *str, int len){
		m_downFlow += len;
		if (len > 4){
			FCBinary binary;
			binary.write(str, len);
			int head = binary.readInt();
			int groupID = (int)binary.readShort();
			int serviceID = (int)binary.readShort();
			FCServerService *service = 0;
			if(m_services.find(serviceID) != m_services.end()){
				m_services[serviceID]->onCallBack(&binary, socketID, localSID, len);
			}
		}
	}

	int FCServerService::getRequestID(){
		return m_requestID ++;
	}

	void FCServerService::getServices(vector<FCServerService*> *services){
		map<int, FCServerService*>::iterator sIter = m_services.begin();
		for(; sIter != m_services.end(); ++sIter){
			services->push_back(sIter->second);
		}
	}

	int FCServerService::keepAlive(int socketID){
		FCBinary binary;
		binary.initialize(4, false);
		binary.writeInt(4);
		char *str = (char*)binary.getBytes();
		return sendByServer(socketID, str, 4);
	}

	void FCServerService::onCallBack(FCBinary *binary, int socketID, int localSID, int len){
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
			//uLong cLen = 128;
			//if(uncBodyLength > cLen)
			//{
			//	cLen = uncBodyLength;
			//}
			//char *buffer = new char[cLen];
			//memset(buffer, '\0', cLen);
			//FCStr::GZDeCompress((Byte*)body, bodyLength, (Byte*)buffer, &cLen);
			//delete[] body;
			//body = buffer;
			//uncBodyLength = cLen;
		}
		FCMessage message(getGroupID(), getServiceID(), functionID, sessionID, requestID, socketID, state, compressType, uncBodyLength, body);
		onReceive(&message);
		onWaitMessageHandle(&message);
		if(message.m_body){
			delete[] body;
		}
		body = 0;
	}

	void FCServerService::onClientClose(int socketID, int localSID){
	}

	void FCServerService::onReceive(FCMessage *message){
		m_lock.lock();
		m_compressTypes[message->m_socketID] = message->m_compressType;
		m_lock.unLock();
	}

	void FCServerService::onWaitMessageHandle(FCMessage *message){
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

	void FCServerService::registerWait(int requestID, FCMessage *message){
		m_lock.lock();
		m_waitMessages[requestID] = message;
		m_lock.unLock();
	}

	void FCServerService::registerListener(int requestID, ListenerMessageCallBack callBack, Object pInvoke){
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

	int FCServerService::send(FCMessage *message){
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
		int ret = sendByServer(message->m_socketID, str, len);
		m_upFlow += ret;
		return ret;
	}

	void FCServerService::sendToListener(FCMessage *message){
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

	void FCServerService::unRegisterListener(int requestID){
		m_lock.lock();
		map<int, FCMessageListener*>::iterator sIter = m_listeners.find(requestID);
		if(sIter != m_listeners.end()){
			delete sIter->second;
			m_listeners.erase(sIter);
		}
		m_lock.unLock();
	}

	void FCServerService::unRegisterListener(int requestID, ListenerMessageCallBack callBack){
		m_lock.lock();
		map<int, FCMessageListener*>::iterator sIter = m_listeners.find(requestID);
		if(sIter != m_listeners.end()){
			sIter->second->remove(callBack);
		}
		m_lock.unLock();
	}

	void FCServerService::unRegisterWait(int requestID){
		m_lock.lock();
		map<int, FCMessage*>::iterator sIter = m_waitMessages.find(requestID);
		if(sIter != m_waitMessages.end()){
			m_waitMessages.erase(sIter);
		}
		m_lock.unLock();
	}

	int FCServerService::waitMessage(int requestID, int timeout){
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

	void FCServerService::writeServerLog(int socketID, int localSID, int state, const char *log){
		if(state == 2){
			map<int, FCServerService*>::iterator sIter = m_services.begin();
			for(; sIter != m_services.end(); ++sIter){
				FCServerService *service = sIter->second;
				service->onClientClose(socketID, localSID);
			}
		}
	}
}

