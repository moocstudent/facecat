#include "..\\..\\stdafx.h"
#include "..\\..\\include\\sock\\FCClientSocket.h"
#include "..\\..\\include\\sock\\FCServerSocket.h"

namespace FaceCat{
	DWORD WINAPI recvClient(LPVOID lpParam){
		FCClientSocket *client = (FCClientSocket*)lpParam;
		if(!client) { return 0; }
		SOCKET socket = client->m_hSocket;
		if(socket == SOCKET_ERROR){
			client = 0;
			return 0;
		}
		char *str = 0;
		bool get = false;
		int head = 0, pos = 0, strRemain = 0, bufferRemain = 0, index = 0, copyLen = 0;
		int intSize = sizeof(int);
		int socketID = (int)socket;
		char headStr[4] = {0};
		int headSize = 4;
		while(1){
			char buffer[1024] = {0};
			if(client->m_type == 0){
				copyLen = recv(socket, buffer, 1024, 0);
			}
			else if(client->m_type == 1){
				int sddrLen = sizeof(SOCKADDR);
				copyLen = recvfrom(socket, buffer, 1024, 0, (SOCKADDR*)&client->m_addr, &sddrLen);
			}
			if(copyLen == 0 || copyLen == SOCKET_ERROR) {
				if(client->m_recvEvent){
					string a = "3";
					client->m_recvEvent(socketID, socketID, a.c_str(), (int)a.length()+1);
				}
				client->writeLog(socketID, socketID, 3, "socket error");
				break;
			}
			index = 0;
			while(index < copyLen){
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
				bufferRemain = copyLen - index;
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
					if(copyLen - index == 0 || copyLen - index >= intSize){
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
		closesocket(socket);
		if(client->m_recvEvent){
			string a = "2";
			client->m_recvEvent(socketID, socketID, a.c_str(), (int)a.length()+1);
		}
		client->writeLog((int)socket, (int)socket, 2, "socket exit");
		client = 0;
		return 0;
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	ConnectStatus FCClientSocket::connectStandard(){
		m_addr.sin_family = AF_INET;
		m_addr.sin_addr.S_un.S_addr = inet_addr(m_ip.c_str());
		m_addr.sin_port = htons(m_port);
		//ÉèÖÃÎª·Ç×èÈû
		unsigned long mode = 1;  
		if (ioctlsocket(m_hSocket, FIONBIO, (unsigned long*)&mode) == SOCKET_ERROR){
			writeLog(0, 0, 0, "ioctlsocket error!");
			return CONNECT_SERVER_FAIL;
		}
		::connect(m_hSocket, (LPSOCKADDR)&m_addr, sizeof(m_addr));
		timeval tmv;
		tmv.tv_sec = m_timeout;
		tmv.tv_usec = 0;
		fd_set set;
		FD_ZERO(&set);
		FD_SET(m_hSocket, &set);
		ConnectStatus ret = CONNECT_SERVER_FAIL;
		if (select(0, 0, &set, 0, &tmv) <= 0){
			int error = GetLastError();
			writeLog(0, 0, 0, "connect error!");
		} 
		else{
			ret = SUCCESS;
		}
		mode = 0;
		ioctlsocket(m_hSocket, FIONBIO, (unsigned long*)&mode);
		return ret;
	}

	ConnectStatus FCClientSocket::connectByHttp()  
	{  
		char buf[512];  
		if (m_proxyUserName != "")  { 
			string str = m_proxyUserName + ":" + m_proxyUserPwd;  
			string strBase64 = CBase64::encode((unsigned char*)str.c_str(), (int)str.length());  
			sprintf_s(buf, sizeof(buf), "CONNECT %s:%d HTTP/1.1\r\nHost: %s:%d\r\nAuthorization: Basic %s\r\n\r\nProxy-Authorization: Basic %s\r\n\r\n",   
				m_ip.c_str(), m_port, m_ip.c_str(), m_port, strBase64.c_str(), strBase64.c_str());  
		}  
		else{   
			sprintf_s(buf, sizeof(buf), "CONNECT %s:%d HTTP/1.1\r\nUser-Agent: MyApp/0.1\r\n\r\n", m_ip.c_str(), m_port);  
		}
		::send(m_hSocket, buf, (int)strlen(buf), 0);
		::recv(m_hSocket, buf, sizeof(buf), 0);	  
		if (strstr(buf, "HTTP/1.0 200 Connection established") != 0){  
			return SUCCESS;  
		}  
		else{  
			return CONNECT_SERVER_FAIL;  
		}    
	}  
	  
	ConnectStatus FCClientSocket::connectBySock4(){  
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
	  
	ConnectStatus FCClientSocket::connectBySock5()  
	{  
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
			ret = closesocket(m_hSocket);
			m_hSocket = 0;
		}
		if(!m_hSocket){
			if(m_type == 0){
				m_hSocket = socket(AF_INET, SOCK_STREAM, 0);
			}
			else if(m_type == 1){
				m_hSocket = socket(AF_INET, SOCK_DGRAM, 0);
			}
		}
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	FCClientSocket::FCClientSocket(int type, int proxyType, const string &ip, u_short port, const string &proxyIp, u_short proxyPort, const string &proxyUserName, const string &proxyUserPwd, const string &proxyDomain, int timeout){
		m_blnProxyServerOk = true;
		m_hSocket = 0;
		m_port = port;
		m_proxyDomain = proxyDomain;
		m_proxyIp = proxyIp;
		m_proxyPort = proxyPort;
		m_proxyType = proxyType;
		m_proxyUserName = proxyUserName;
		m_proxyUserPwd = proxyUserPwd;
		m_recvEvent = 0;
		m_timeout = timeout;
		m_type = type;
		m_writeLogEvent = 0;
		FCServerSocket::wSStart();
		m_ip = getHostIP(ip.c_str());
	}

	FCClientSocket::~FCClientSocket(){
		m_recvEvent = 0;
		m_writeLogEvent = 0;
		FCServerSocket::wSStop();
	}

	int FCClientSocket::close(int socketID){
		int ret = 0;
		SOCKET socket = (SOCKET)socketID;
		if(socket){
			ret = closesocket(socket);
			if(m_recvEvent){
				string a = "2";
				m_recvEvent((int)socket, (int)socket, a.c_str(), (int)a.length()+1);
			}
			writeLog((int)socket, (int)socket, 2, "socket exit");
		}
		return ret;
	}

	ConnectStatus FCClientSocket::connect()  {
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
			unsigned long ul = 0;  
			int ret = ioctlsocket(m_hSocket, FIONBIO, (unsigned long*)&ul);	
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
			HANDLE hThread = ::CreateThread(NULL, 0, recvClient, (LPVOID)this, 0, NULL);
			::CloseHandle(hThread);
			char submitStr[1024];
			memset(submitStr, '\0', 1024);
			submitStr[0] = 'm';
			submitStr[1] = 'i';
			submitStr[2] = 'a';
			submitStr[3] = 'o';
			send((int)m_hSocket, submitStr, 1024);
		}
		return status;  
	}

	ConnectStatus FCClientSocket::connectProxyServer(){
		int ret;
		struct timeval timeout ;
		fd_set r;
		string ip;
		u_short port;
		ip = m_proxyIp;
		port = m_proxyPort;
		sockaddr_in servAddr;
		servAddr.sin_family = AF_INET;
		servAddr.sin_addr.S_un.S_addr = inet_addr(ip.c_str());
		servAddr.sin_port = htons(port);
		unsigned long ul = 1;
		ret = ioctlsocket(m_hSocket, FIONBIO, (unsigned long*)&ul);
		if(ret == SOCKET_ERROR) {
			return CONNECT_PROXY_FAIL;
		}
		::connect(m_hSocket, (sockaddr*)&servAddr, sizeof(sockaddr));
		FD_ZERO(&r);
		FD_SET(m_hSocket, &r);
		timeout.tv_sec = m_timeout; 
		timeout.tv_usec = 0;
		ret = select(0, 0, &r, 0, &timeout);
		if (ret <= 0){
			m_blnProxyServerOk = false;
			return CONNECT_PROXY_FAIL;
		}
		else{
			m_blnProxyServerOk = true;
			return SUCCESS;
		}
	}

	string FCClientSocket::getHostIP(const char* ip){
		char szIP[1024] = {0};
		struct hostent *hptr;
		hptr = ::gethostbyname(ip);
		if(hptr){
			switch(hptr->h_addrtype){
				case AF_INET:
				case AF_INET6:{
						sprintf_s(szIP, 1023, "%d.%d.%d.%d", hptr->h_addr_list[0][0]&0x00ff,
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
		else{
			return ip;
		}
	}

	int FCClientSocket::send(int socketID, const char *str, int len){
		SOCKET socket = (SOCKET)socketID;
		fd_set fds;
		FD_ZERO(&fds);
		FD_SET(socket, &fds);
		timeval timeout = {1, 0};
		int res = select(0, 0, &fds, 0, &timeout);
		if (res > 0){
			return ::send(socket, str, len, 0);
		}
		else{
			return -1;
		}
	}

	int FCClientSocket::sendTo(const char *str, int len){
		return sendto(m_hSocket, str, len,0, (SOCKADDR*)&m_addr, sizeof(SOCKADDR));
	}

	void FCClientSocket::writeLog(int socketID, int localSID, int state, const char *log){
		if(m_writeLogEvent)
		{
			m_writeLogEvent(socketID, localSID, state, log);
		}
	}
}