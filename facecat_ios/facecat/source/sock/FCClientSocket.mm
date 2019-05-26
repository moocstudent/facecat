#include "stdafx.h"
#include "FCClientSocket.h"

namespace FaceCat{
    Object recvClient(Object lpParam){
        FCClientSocket *client = (FCClientSocket*)lpParam;
        if(!client) return 0;
        int socket = client->m_hSocket;
        if(socket == -1){
            client = 0;
            return 0;
        }
        char *str = 0;
        bool get = false;
        int head = 0, pos = 0, strRemain = 0, bufferRemain = 0, index = 0, len = 0;
        int intSize = sizeof(int);
        int socketID = (int)socket;
        char headStr[4] = {0};
        int headSize = 4;
        struct sockaddr_in addr = client->m_addr;
        socklen_t addr_len = sizeof(struct sockaddr_in);
        while(1){
            char buffer[1024] = {0};
            if(client->m_type == 0){
                len = (int)recv(socket, buffer, 1024, 0);
            }
            else{
                len = (int)recvfrom(socket, buffer, 1024, 0, (struct sockaddr*)&addr, &addr_len);
            }
            if(len == 0 || len == -1){
                if(client->m_recvEvent){
                    char rmsg[1] = {(char)3};
                    client->m_recvEvent(socketID, socketID, rmsg, 1);
                }
                client->writeLog(socketID, socketID, 3, "socket error");
                break;
            }
            index = 0;
            while(index < len){
                int diffSize = 0;
                if(!get){
                    diffSize = intSize - headSize;
                    if(diffSize == 0){
                        memcpy(&head, buffer + index, intSize);
                    }
                    else{
                        for(int i = 0; i < diffSize; i++){
                            headStr[headSize + i] = buffer[i];
                        }
                        memcpy(&head, headStr, intSize);
                    }
                    if(str){
                        delete[] str;
                        str = 0;
                    }
                    str = new char[head];
                    memset(str, '\0', head);
                    if(diffSize > 0){
                        for(int i = 0; i < headSize; i++){
                            str[i] = headStr[i];
                        }
                        pos += headSize;
                        headSize = intSize;
                    }
                }
                bufferRemain = len - index;
                strRemain = head - pos;
                get = strRemain > bufferRemain;
                int remain = min(strRemain, bufferRemain);
                memcpy(str + pos, buffer + index, remain);
                pos += remain;
                index += remain;
                if(!get){
                    if(client->m_recvEvent){
                        client->m_recvEvent(socketID, socketID, str, head);
                    }
                    head = 0;
                    pos = 0;
                    if(len - index == 0 || len - index >= intSize){
                        headSize = intSize;
                    }
                    else{
                        headSize = bufferRemain - strRemain;
                        memcpy(headStr, buffer + index, headSize);
                        break;
                    }
                }
            }
        }
        close(socket);
        if(client->m_recvEvent){
            char rmsg[1] = {(char)2};
            client->m_recvEvent(socketID, socketID, rmsg, 1);
        }
        client->writeLog(socketID, socketID, 2, "socket exit");
        client = 0;
        return 0;
    }
    
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    ConnectStatus FCClientSocket::connectStandard(){
        for (res = res0; res; res = res->ai_next){
            if(res->ai_family == AF_INET){
                char ipv4_str_buf[INET_ADDRSTRLEN] = { 0 };
                struct sockaddr_in *v4sa = (struct sockaddr_in *)res->ai_addr;
                v4sa->sin_port = htons(m_port);
                inet_ntop(AF_INET, &(v4sa->sin_addr),
                          ipv4_str_buf, sizeof(ipv4_str_buf));
                int ret = ::connect(m_hSocket, res->ai_addr, res->ai_addrlen);
                if(ret >= 0){
                    return SUCCESS;
                }
                else{
                    return CONNECT_SERVER_FAIL;
                }
            }
            else if(res->ai_family == AF_INET6){
                for (res = res0; res; res = res->ai_next){
                    char ipv6_str_buf[INET6_ADDRSTRLEN] = { 0 };
                    struct sockaddr_in6 *v6sa = (struct sockaddr_in6 *)res->ai_addr;
                    v6sa->sin6_port = htons(m_port);
                    inet_ntop(AF_INET6, &(v6sa->sin6_addr),
                              ipv6_str_buf, sizeof(ipv6_str_buf));
                    int ret = ::connect(m_hSocket, res->ai_addr, res->ai_addrlen);
                    if(ret >= 0){
                        return SUCCESS;
                    }
                    else{
                        return CONNECT_SERVER_FAIL;
                    }
                }
                return CONNECT_SERVER_FAIL;
            }
            
        }
        return CONNECT_SERVER_FAIL;
    }
    
    ConnectStatus FCClientSocket::connectByHttp()
    {
        char buf[512];
        if (m_proxyUserName != ""){
            string str;
            string strBase64;
            str = m_proxyUserName + ":" + m_proxyUserPwd;
            strBase64 = CBase64::encode((unsigned char*)str.c_str(), (int)str.length());
            sprintf(buf, "CONNECT %s:%d HTTP/1.1\r\nHost: %s:%d\r\nAuthorization: Basic %s\r\n\r\nProxy-Authorization: Basic %s\r\n\r\n",
                    m_ip.c_str(), m_port, m_ip.c_str(), m_port, strBase64.c_str(), strBase64.c_str());
        }
        else{
            sprintf(buf, "CONNECT %s:%d HTTP/1.1\r\nUser-Agent: MyApp/0.1\r\n\r\n", m_ip.c_str(), m_port);
        }
        ::send(m_hSocket, buf, (int)strlen(buf), 0);
        recv(m_hSocket, buf, sizeof(buf), 0);
        if (strstr(buf, "HTTP/1.0 200 Connection established") != 0){
            return SUCCESS;
        }
        else{
            return CONNECT_SERVER_FAIL;
        }
    }
    
    ConnectStatus FCClientSocket::connectBySock4()
    {
        char buf[512];
        memset(buf, '\0', sizeof(buf));
        struct TSock4req1 *proxyreq;
        proxyreq = (struct TSock4req1*)buf;
        proxyreq->VN = 4;
        proxyreq->CD = 1;
        proxyreq->Port = ntohs(m_port);
        proxyreq->IPAddr = inet_addr(m_ip.c_str());
        ::send(m_hSocket, buf, 9, 0);
        struct TSock4ans1 *proxyans;
        proxyans = (struct TSock4ans1*)buf;
        memset(buf, '\0', sizeof(buf));
        recv(m_hSocket, buf, sizeof(buf), 0);
        if(proxyans->VN == 0 && proxyans->CD == 90){
            return SUCCESS;
        }
        else{
            return CONNECT_SERVER_FAIL;
        }
    }
    
    ConnectStatus FCClientSocket::connectBySock5(){
        char buf[512];
        struct TSock5req1 *proxyreq1;
        proxyreq1 = (struct TSock5req1 *)buf;
        proxyreq1->Ver = 5;
        proxyreq1->nMethods = 1;
        proxyreq1->Methods = m_proxyUserName != "" ? 2 : 0;
        ::send(m_hSocket, buf, 3, 0);
        struct TSock5ans1 *proxyans1;
        proxyans1 = (struct TSock5ans1 *)buf;
        memset(buf, '\0', sizeof(buf));
        recv(m_hSocket, buf, sizeof(buf), 0);
        if(proxyans1->Ver != 5 || (proxyans1->Method != 0 && proxyans1->Method != 2)){
            return CONNECT_SERVER_FAIL;
        }
        if(proxyans1->Method == 2){
            int nUserLen = (int)m_proxyUserName.length();
            int nPassLen = (int)m_proxyUserPwd.length();
            buf[0] = 1;
            buf[1] = nUserLen;
            memcpy(buf + 2, m_proxyUserName.c_str(), nUserLen);
            buf[2 + nUserLen] = nPassLen;
            memcpy(buf + 3 + nUserLen, m_proxyUserPwd.c_str(), nPassLen);
            ::send(m_hSocket, buf, 3 + nUserLen + nPassLen, 0);
            struct TAuthans *authans;
            authans = (struct TAuthans *)buf;
            memset(buf, '\0', sizeof(buf));
            recv(m_hSocket, buf, sizeof(buf), 0);
            if(authans->Ver != 1 || authans->Status != 0){
                return CONNECT_SERVER_FAIL;
            }
        }
        memset(buf, '\0', sizeof(buf));
        struct TSock5req2 *proxyreq2;
        proxyreq2 = (struct TSock5req2 *)buf;
        proxyreq2->Ver = 5;
        proxyreq2->Cmd = 1;
        proxyreq2->Rsv = 0;
        proxyreq2->Atyp = 1;
        unsigned long tmpLong = inet_addr(m_ip.c_str());
        unsigned short port1 = ntohs(m_port);
        memcpy((char*)&proxyreq2->other, &tmpLong, 4);
        memcpy((char*)(&proxyreq2->other) + 4, &port1, 2);
        ::send(m_hSocket, buf, 10, 0);
        struct TSock5ans2 *proxyans2;
        memset(buf ,'\0', sizeof(buf));
        proxyans2 = (struct TSock5ans2 *)buf;
        recv(m_hSocket, buf, sizeof(buf), 0);
        if(proxyans2->Ver != 5 || proxyans2->Rep != 0){
            return CONNECT_SERVER_FAIL;
        }
        return SUCCESS;
    }
    
    void FCClientSocket::createSocket(){
        int ret = 0;
        if(m_hSocket){
            ret = close(m_hSocket);
            m_hSocket = 0;
        }
        if(!m_hSocket){
            if(m_type == 0){
                for (res = res0; res; res = res->ai_next){
                    m_hSocket = socket(res->ai_family,
                                       res->ai_socktype,
                                       res->ai_protocol);
                    break;
                }
            }
            else if(m_type == 1){
                m_hSocket = socket(AF_INET, SOCK_DGRAM, 0);
            }
            int value = 1;
			setsockopt(m_hSocket, SOL_SOCKET, SO_NOSIGPIPE, &value, sizeof(value));
        }
    }
    
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    FCClientSocket::FCClientSocket(int type, long proxyType, string ip, u_short port, string proxyIp, u_short proxyPort, string proxyUserName, string proxyUserPwd, string proxyDomain, int timeout){
        m_blnProxyServerOk = true;
        m_proxyDomain = proxyDomain;
        m_proxyType = proxyType;
        m_hSocket = 0;
        m_ip = getHostIP(ip.c_str());
        memset(&hints, 0, sizeof(hints));
        hints.ai_family = PF_UNSPEC;
        hints.ai_socktype = SOCK_STREAM;
        hints.ai_flags = AI_DEFAULT;
        getaddrinfo(m_ip.c_str(), NULL, &hints, &res0);
        m_port = port;
        m_proxyIp = proxyIp;
        m_proxyPort = proxyPort;
        m_proxyUserName = proxyUserName;
        m_proxyUserPwd = proxyUserPwd;
        m_recvEvent = 0;
        m_timeout = timeout;
        m_type = type;
        m_writeLogEvent = 0;
    }
    
    FCClientSocket::~FCClientSocket(){
        m_recvEvent = 0;
        m_writeLogEvent = 0;
    }
    
    int FCClientSocket::close(int socketID){
        int ret = 0;
        if(socketID != -1){
            ret = close(socketID);
            if(m_recvEvent){
                char rmsg[1] = {(char)2};
                m_recvEvent(socketID, socketID, rmsg, 1);
            }
            writeLog(socketID, socketID, 2, "socket exit");
        }
        return ret;
    }
    
    ConnectStatus FCClientSocket::connect(){
        createSocket();
        if(m_proxyType > 0){
            connectProxyServer();
        }
        if (!m_blnProxyServerOk){
            return NOT_CONNECT_PROXY;
        }
        ConnectStatus status = CONNECT_SERVER_FAIL;
        if(m_proxyType == 0){
            status = connectStandard();
        }
        else{
            int nTimeout = 5000;
            setsockopt(m_hSocket, SOL_SOCKET, SO_RCVTIMEO, (char *)&nTimeout, sizeof(int));
            fcntl(m_hSocket, F_SETFL, O_NONBLOCK);
            switch(m_proxyType){
                case 1:
                    status = connectByHttp();
                    break;
                case 2:
                    status = connectBySock4();
                    break;
                case 3: 
                    status = connectBySock5();
                    break;
            }
        }
        if(status == SUCCESS){
            pthread_t recvThread;
            pthread_create(&recvThread, 0, recvClient, this);
            char submitStr[1024];
            memset(submitStr, '\0', 1024);
            submitStr[0] = 'm';
            submitStr[1] = 'i';
            submitStr[2] = 'a';
            submitStr[3] = 'o';
            send(m_hSocket, submitStr, 1024);
        }
        return status;
    }
    
    ConnectStatus FCClientSocket::connectProxyServer(){
        return CONNECT_PROXY_FAIL;
    }
    
    string FCClientSocket::getHostIP(const char* ip){
        char szIP[1024] = {0};
        struct hostent *hptr;
        hptr = gethostbyname(ip);
        if(hptr)
        {
            switch(hptr->h_addrtype)
            {
                case AF_INET:
                case AF_INET6:
                {
                    sprintf(szIP, "%d.%d.%d.%d", hptr->h_addr_list[0][0]&0x00ff,
                          hptr->h_addr_list[0][1]&0x00ff,
                          hptr->h_addr_list[0][2]&0x00ff,
                          hptr->h_addr_list[0][3]&0x00ff);
                }
                break;
                default:
                    printf("unknown address type\n");
                break;
            }
            return szIP;
        }
        return ip;
    }
    
    int FCClientSocket::send(int socketID, const char *str, int len){
        return (int)::send(socketID, str, len, 0);
    }
    
    int FCClientSocket::sendTo(const char *str, int len){
        return (int)sendto(m_hSocket, str, len, 0, (struct sockaddr*)&m_addr, sizeof(m_addr));
    }
    
    void FCClientSocket::writeLog(int socketID, int localSID, int state, const char *log){
        if(m_writeLogEvent)
        {
            m_writeLogEvent(socketID, localSID, state, log);
        }
    }
}
