#include "..\\..\\stdafx.h"
#include "..\\..\\include\\service\\FCHttpMonitor.h"
#include "..\\..\\include\\service\\CFunctionEx.h"
#include "..\\..\\include\\service\\CFunctionHttp.h"
#include "..\\..\\include\\core\\FCBinary.h"

namespace FaceCat{	
	static map<String, FCHttpEasyService*> m_easyServices;


	FCHttpData::FCHttpData(){
		m_body = 0;
		m_close = false;
		m_contentLength = 0;
		m_contentType = L"";
		m_method = L"";
		m_parameters.clear();	
		m_resBytes = 0;
		m_resBytesLength = 0;
		m_resStr = L"";	
		m_remoteIP = L"";
        m_remotePort = 0;
		m_socket = 0;
		m_statusCode = 200;
		m_url = L"";
	}

	FCHttpData::~FCHttpData(){
		m_parameters.clear();
		if(m_body){
			delete m_body;
			m_body = 0;
		}
		if(m_resBytes){
			delete m_resBytes;
			m_resBytes = 0;
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////

	FCHttpMonitor::FCHttpMonitor(string fileName){
		m_fileName = fileName;
		m_hSocket = 0;
		m_indicator = 0;
		m_native = 0;
		m_port = 8080;
		m_script = L"";
		m_useScript = true;
	}

	FCHttpMonitor::~FCHttpMonitor(){
		map<int, FCHttpData*>::iterator sIter = m_httpDatas.begin();
		for(;sIter != m_httpDatas.end(); sIter++){
			delete sIter->second;
		}
		m_httpDatas.clear();
		m_indicator = 0;
		while (!m_indicators.empty()){
			FCScript *indicator = m_indicators.top();
			m_indicators.pop();
			delete indicator;
		}  
		m_native = 0;
	}

	FCHttpEasyService* FCHttpMonitor::getEasyService(const String& name){
		if(m_easyServices.find(name) != m_easyServices.end()){
			return m_easyServices[name];
		}
		else{
			return 0;
		}
	}

	FCScript* FCHttpMonitor::getIndicator(){
		return m_indicator;
	}

	static FCHttpMonitor *m_mainMonitor = 0;

	FCHttpMonitor* FCHttpMonitor::getMainMonitor(){
		return m_mainMonitor;
	}

	FCNative* FCHttpMonitor::getNative(){
		return m_native;
	}

	int FCHttpMonitor::getPort(){
		return m_port;
	}

	void FCHttpMonitor::setPort(int port){
		m_port = port;
	}

	SOCKET FCHttpMonitor::getSocket(){
		return m_hSocket;
	}

	String FCHttpMonitor::getScript(){
		return m_script;
	}

	void FCHttpMonitor::setScript(String script)
	{
		m_script = script;
	}

	bool FCHttpMonitor::getUseScript(){
		return m_useScript;
	}

	void FCHttpMonitor::setHttpData(int socketID, FCHttpData* data){
		m_lockHttpData.lock();
		m_httpDatas.insert(make_pair(socketID, data));
		m_lockHttpData.unLock();
	}

	FCScript* FCHttpMonitor::popCIndicator(){		
		FCScript* indicator = 0;
		m_lock.lock();		
		if (!m_indicators.empty()){
			indicator = m_indicators.top();
			m_indicators.pop();
		}
		m_lock.unLock();
		return indicator;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////

	DWORD WINAPI receiveHandle(LPVOID lpParam){
		FCHttpData *data = (FCHttpData*)lpParam;
		if(!data) return 0;		
		SOCKADDR_IN addr = data->m_addr;	
		const int bufLen = 102400;
		char buffer[bufLen];
		memset(buffer, '\0', bufLen);
        String parameters = L"";
		int contentLength = 0;
		int copyLen = recv(data->m_socket, buffer, bufLen, 0);
		if(copyLen == 0 || copyLen == SOCKET_ERROR) {
			return 0;
		}		
		strstream os(buffer, copyLen);
		while(true){
			char bufferline[bufLen];
			memset(bufferline, '\0', bufLen);
			os.getline(bufferline, bufLen);
			string requestHeader(bufferline);
			int headLen = requestHeader.length();
			if(headLen == 0 || (headLen == 1 && requestHeader[0] == '\r')){
				break;
			}
			int keyLength = 0;
			int begin = 5;
			if(requestHeader.find("GET") == 0){
				int end = (int)requestHeader.find("HTTP/");
				data->m_method = L"GET";
				parameters = FCStr::stringTowstring(requestHeader.substr(begin, end - 6));
			}
			else if(requestHeader.find("POST") == 0){
				int end = (int)requestHeader.find("HTTP/");
				data->m_method = L"POST";
				parameters = FCStr::stringTowstring(requestHeader.substr(begin, end - 6));
			}
			else if(requestHeader.find("Accept:") == 0){
				int acceptIndex = 0;
				keyLength = (int)strlen("Accept:");
				begin = acceptIndex + keyLength;
				data->m_contentType = FCStr::stringTowstring(requestHeader.substr(begin, requestHeader.find(',', begin) - begin));
			}
			else if (requestHeader.find("Host:") == 0){
				int hostIndex = 0;
				keyLength = (int)strlen("Host:");
				begin = hostIndex + keyLength;
				data->m_url = FCStr::stringTowstring(requestHeader.substr(begin, requestHeader.find("\r\n", begin) - begin));
			}
			else if (requestHeader.find("Content-Length:") == 0){
				int contentIndex = 0;
				keyLength = (int)strlen("Content-Length:");
				begin = contentIndex + keyLength;
				string postParamterLength = FCStr::replace(requestHeader.substr(begin), " ", "");;
				contentLength = atoi(postParamterLength.c_str());
			}
		}		
        if (contentLength > 0){
            int idx = 0, ide = 0;
            data->m_body = new char[contentLength];
            while (idx < contentLength){
				int recvData = os.get();
                if (recvData != -1){
                    if (recvData != 0){
                        ide++;
                    }
                    idx++;
                }
                else{
                    break;
                }
            }
            if (ide == 0){
                recv(data->m_socket, data->m_body, contentLength, 0);
            }
            else{
                for (int i = 0; i < contentLength; i++){
                    data->m_body[i] = buffer[copyLen - contentLength + i];
                }
            }
            data->m_contentLength = contentLength;
        }
		if ((int)data->m_method.length() == 0){
			closesocket(data->m_socket);
			delete data;
            return 0;
        }
		int cindex = (int)parameters.find('?');
        if (cindex != -1){
            data->m_url = data->m_url + L"/" + parameters;
			parameters = parameters.substr(cindex + 1);
			ArrayList<String> strParameters = FCStr::split(parameters, L"&");
			int strsSize = (int)strParameters.size();
            for (int i = 0; i < strsSize; i++){
				ArrayList<String> subStrs = FCStr::split(strParameters.get(i), L"=");
				if((int)subStrs.size() > 1){
					string value = FCStr::wstringTostring(subStrs.get(1));
					value = FCStr::urlDecode(value);
					String wValue = FCStr::stringTowstring(value);
					String wPara = subStrs.get(0);
					data->m_parameters.insert(make_pair(wPara, wValue));
				}
            }
        }
        else{
            data->m_url += L"/" + parameters;
        }		
        FCScript *indicator = 0;
		if (m_mainMonitor->getUseScript()){
			indicator = m_mainMonitor->popCIndicator();
			if(indicator == 0){
				indicator = CFunctionEx::createIndicator(m_mainMonitor->getScript(), m_mainMonitor->getNative());
			}
			ArrayList<CFunction*> functions = indicator->getFunctions();
			int functionsSize = (int)functions.size();
            for (int i = 0; i < functionsSize; i++){
				CFunctionHttp *function = dynamic_cast<CFunctionHttp*>(functions.get(i));
                if (function != 0){
					function->setHttpData(data);
                }
            }
        }
		m_mainMonitor->setHttpData((int)data->m_socket, data);
		if(indicator != 0){
			indicator->callFunction(L"ONHTTPREQUEST();");
		}
        if (data->m_close){
			closesocket(data->m_socket);
			delete data;
            return 0;
        }
		int resContentLength = 0;
        if (data->m_resBytes){
            resContentLength = data->m_resBytesLength;
        }
        else{
			string str = FCStr::wstringTostring(data->m_resStr);
            resContentLength = (int)str.length() + 1;
        }
		string retMessage = "";
		char statusCode[10]; 		
		sprintf_s(statusCode, 9, "%d", data->m_statusCode);
		char szContentLength[10]; 		
		sprintf_s(szContentLength, 9, "%d", resContentLength);
		retMessage = retMessage.append("HTTP/1.0 ");
		retMessage = retMessage.append(statusCode);
		retMessage = retMessage.append(" OK\r\n");
		retMessage = retMessage.append("Content-Length:");
		retMessage = retMessage.append(szContentLength);
		retMessage = retMessage.append("\r\n");
		retMessage = retMessage.append("Connection: close\r\n\r\n");
		if (data->m_resBytes){
			int retLength = (int)strlen(retMessage.c_str());
			int ret = send(data->m_socket, retMessage.c_str(), retLength, 0);
			ret = send(data->m_socket, data->m_resBytes, data->m_resBytesLength, 0);
        }
        else{
			string str = FCStr::wstringTostring(data->m_resStr);
			retMessage = retMessage.append(str);
			int ret = send(data->m_socket, retMessage.c_str(), (int)retMessage.length() + 1, 0);
        }
		closesocket(data->m_socket);
		delete data;
		return 0;
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void FCHttpMonitor::checkScript(){
		String wFileName = FCStr::stringTowstring(m_fileName);
        String newScript = L"";
		FCFile::read(wFileName, &newScript);
		if (m_script.compare(newScript) == 0){
			cout << L"检测到脚本被修改...";
            m_script = newScript;			
			m_lock.lock();			
            while (true){
				FCScript *indicator = m_indicators.top();
				m_indicators.pop();
                delete indicator;
            }
			m_lock.unLock();
        }
	}

	int FCHttpMonitor::close(int socketID){
		SOCKET socket = (SOCKET)socketID;
		int ret = 0;
		if(socket){
			ret = closesocket(socket);
		}
		return ret;
	}

	int FCHttpMonitor::start(string fileName){
		String wFileName = FCStr::stringTowstring(m_fileName);
		m_useScript = FCFile::isFileExist(wFileName);
        if (m_useScript){
            m_native = new FCNative;
			FCFile::read(wFileName, &m_script);
			m_indicator = CFunctionEx::createIndicator(m_script, m_native);
			cout << m_script.c_str() << "\r\n";
        }
		if(m_indicator != 0){
			String funcName = L"ONHTTPSERVERSTARTING('";
			funcName = funcName.append(wFileName);
			funcName = funcName.append(L"');");
			m_indicator->callFunction(funcName);
		}		
		int ret = 0;
		m_hSocket = socket(AF_INET, SOCK_STREAM, 0);
		m_addr.sin_addr.S_un.S_addr = INADDR_ANY;
		m_addr.sin_family = AF_INET;
		m_addr.sin_port = htons(m_port);
		ret = bind(m_hSocket, (SOCKADDR*)&m_addr, sizeof(SOCKADDR));
		if(ret != SOCKET_ERROR){
			ret = listen(m_hSocket, 2);
			if(ret != SOCKET_ERROR){
				while(true){
					SOCKADDR_IN addr;
					int addrlen = sizeof(addr);
					SOCKET socket = accept(m_hSocket, (LPSOCKADDR)&addr, &addrlen);
					if(socket == SOCKET_ERROR){
						::Sleep(1);
						continue;
					}
					FCHttpData *data = new FCHttpData;
					data->m_addr = addr;
					data->m_socket = socket;
					HANDLE hThread = ::CreateThread(NULL, 0, receiveHandle, (LPVOID)data, 0, 0);
					::CloseHandle(hThread);
				}
			}
		}
		if(m_hSocket){
			ret = closesocket(m_hSocket);
		}
		return ret;
	}
}