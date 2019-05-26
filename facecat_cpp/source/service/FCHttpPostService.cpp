#include "..\\..\\stdafx.h"
#include "..\\..\\include\\core\\FCBinary.h"
#include "..\\..\\include\\core\\FCStr.h"
#include "..\\..\\include\\service\\FCHttpPostService.h"

namespace FaceCat{
	FCHttpPostService::FCHttpPostService(){
		m_isSyncSend = false;
		m_timeout = 0;
	}

	FCHttpPostService::~FCHttpPostService(){
	}

	bool FCHttpPostService::isSyncSend(){
        return m_isSyncSend;
    }
    
    void FCHttpPostService::setIsSyncSend(bool isSyncSend){
        m_isSyncSend = isSyncSend;
    }
    
    FCLock FCHttpPostService::getLock(){
        return m_lock;
    }
   
	int FCHttpPostService::getTimeout(){
		return m_timeout;
	}

	void FCHttpPostService::setTimeout(int timeout){
		m_timeout = timeout;
	}
   
	string FCHttpPostService::getUrl(){
		return m_url;
	}

	void FCHttpPostService::setUrl(string url){
		m_url = url;
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	DWORD WINAPI FCHttpPostService::asynSend(LPVOID lpParam)
	{
        FCHttpPostService *postService = (FCHttpPostService*)lpParam;
        if(!postService){
            return 0;
        }
		ArrayList<FCMessage*> messages =postService->m_messages;
		FCLock lock = postService->getLock();
        lock.lock();
        FCMessage *message = messages.get(0);
        lock.unLock();
        if(!message){
            return 0;
        }
		return postService->sendRequest(message);
	}

	void FCHttpPostService::onReceive(FCMessage *message){
		FCClientService::onReceive(message);
		FCClientService::sendToListener(message); 	
	}

	String FCHttpPostService::post(const String& url){
		string result = post(url, 0, 0);
		String text = FCStr::stringTowstring(result);
		return text;
	}

	string FCHttpPostService::post(const String& url, char *sendDatas, int sendSize){
		string surl = FCStr::wstringTostring(url);
		string result = ""; //CurlHttp::GetInstance()->Post(surl.c_str(), sendDatas, sendSize);
		return result;
	}

	String FCHttpPostService::post(const String& url, const String &data){
		string sData = FCStr::wstringTostring(data);
		string str = ""; //Post(url, (char*)sData.c_str(), (int)sData.length() + 1);
		String wResult = FCStr::stringTowstring(str);
		return wResult;
	}

	int FCHttpPostService::send(FCMessage *message){
		if(!m_isSyncSend){
			m_lock.lock();
			FCMessage *copyMessage = new FCMessage;
			copyMessage->copy(message);
			char *str = new char[copyMessage->m_bodyLength];
			for(int i = 0; i < copyMessage->m_bodyLength; i++){
				str[i] = message->m_body[i];
			}
			copyMessage->m_body = str;
			m_messages.add(copyMessage);
			m_lock.unLock();
			HANDLE hThread = ::CreateThread(0, 0, asynSend, this, 0, NULL);
			::CloseHandle(hThread);
			return 1;
		}
		else{
			return sendRequest(message);
		}
	}

	int FCHttpPostService::sendRequest(FCMessage *message)
	{
		long long upFlow = FCClientService::getUpFlow();
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
		string result = "";//CurlHttp::GetInstance()->Post(GetUrl().c_str(), str, len);
		int ret = (int)result.length();
		upFlow += ret;
		FCClientService::setUpFlow(upFlow);
		FCClientService::callBack(message->m_socketID, 0, result.c_str(), ret);
		delete message;
		message = 0;
		return 0;
	}
}