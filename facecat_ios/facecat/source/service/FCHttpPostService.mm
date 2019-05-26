#include "FCHttpPostService.h"

namespace FaceCat{
    FCHttpPostService::FCHttpPostService(){
        m_isSyncSend = false;
        m_timeout = 0;
    }
    
    FCHttpPostService::~FCHttpPostService(){
    }
    
    FCLock FCHttpPostService::getLock(){
        return m_lock;
    }
    
    bool FCHttpPostService::isSyncSend(){
        return m_isSyncSend;
    }
    
    void FCHttpPostService::setIsSyncSend(bool isSyncSend){
        m_isSyncSend = isSyncSend;
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
    
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    Object asynSend(Object lpVoid){
        FCHttpPostService *postService = (FCHttpPostService*)lpVoid;
        if(postService == NULL){
            return NULL;
        }
        vector<FCMessage*> messages = postService->m_messages;
        FCLock lock = postService->getLock();
        lock.lock();
        FCMessage *message = NULL;
        if (!messages.empty()){
            message = messages[0];
            messages.erase(messages.begin());
        }
        lock.unLock();
        if(message == NULL){
            return NULL;
        }
        postService->sendRequest(message);
        return NULL;
    }
    
    void FCHttpPostService::onReceive(FCMessage *message){
        FCClientService::onReceive(message);
        sendToListener(message);
    }
    
    String FCHttpPostService::post(const String& url){
        string result = post(url, 0, 0);
        String text = FCStr::stringTowstring(result);
        return text;
    }
    
    string FCHttpPostService::post(const String& url, char *sendDatas, int sendSize){
        string surl = FCStr::wstringTostring(url);
        NSString *nUrl = [NSString stringWithUTF8String: surl.c_str()];
        NSURL *URL = [NSURL URLWithString:nUrl];
        NSMutableURLRequest *request=[NSMutableURLRequest requestWithURL:URL];
        request.HTTPMethod = @"POST";//设置请求方法
        if(sendDatas){
            NSData *nsBody = [NSData dataWithBytes:sendDatas length:sendSize];
            request.HTTPBody = nsBody;
        }
        NSURLResponse * response = nil;
        NSError *error = nil;
        NSData *data = [NSURLConnection sendSynchronousRequest:request
                                             returningResponse:&response
                                                         error:&error];
        if(error){
            return "";
        }
        else{
            char *resData = (char *) [data bytes];
            return resData;
        }
    }
    
    String FCHttpPostService::post( const String& url, const String &data ){
        string sData = FCStr::wstringTostring(data);
        string str = post(url, (char*)sData.c_str(), sData.length() + 1);
        String wResult = FCStr::stringTowstring(str);
        return wResult;
    }
    
    int FCHttpPostService::send(FCMessage *message){
        if(!m_isSyncSend){
            m_lock.lock();
            FCMessage *copyMessage = new FCMessage;
            copyMessage->copy(message);
            copyMessage->m_body = new char[copyMessage->m_bodyLength];
            char *str = new char[copyMessage->m_bodyLength];
            for(int i = 0; i < copyMessage->m_bodyLength; i++){
                str[i] = message->m_body[i];
            }
            copyMessage->m_body = str;
            m_messages.push_back(copyMessage);
            m_lock.unLock();
            pthread_t hThread;
            pthread_create(&hThread, 0, asynSend, this);
            return 1;
        }else{
            return sendRequest(message);
        }
    }
    
    int FCHttpPostService::sendRequest(FCMessage *message){
        Long upFlow = FCClientService::getUpFlow();
        FCBinary binary;
        char *buffer = 0;
        const char *body = message->m_body;
        int bodyLength = message->m_bodyLength;
        int uncBodyLength = bodyLength;
        if (message->m_compressType == COMPRESSTYPE_GZIP){
            //TODO
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
        
        NSString *nUrl = [NSString stringWithUTF8String: getUrl().c_str()];
        NSURL *URL = [NSURL URLWithString:nUrl];
        NSMutableURLRequest *request=[NSMutableURLRequest requestWithURL:URL];
        request.HTTPMethod = @"POST";//设置请求方法
        NSData *nsBody = [NSData dataWithBytes:str length:len];
        request.HTTPBody = nsBody;
        
        NSURLResponse * response = nil;
        NSError *error = nil;
        NSData *data = [NSURLConnection sendSynchronousRequest:request
                                             returningResponse:&response
                                                         error:&error];
        char *resData = (char *) [data bytes];
        int ret = [data length];
        upFlow += ret;
        FCClientService::setUpFlow(upFlow);
        
        FCClientService::callBack(message->m_socketID, 0, resData, ret);
        delete message;
        message = 0;
        return ret;
    }
}
