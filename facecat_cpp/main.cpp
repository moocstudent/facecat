#include "stdafx.h"
#include "include\\sock\\FCClientSockets.h"
#include "include\\sock\\FCServerSockets.h"
using namespace FaceCat;
#pragma comment(lib, "ws2_32.lib")
#include "winsock2.h"
#include "include\\core\\FCPaint.h"
using namespace FaceCat;

#ifdef _MANAGED
#pragma managed(push, off)
#endif

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
					 )
{
    return TRUE;
}

#ifdef _MANAGED
#pragma managed(pop)
#endif

typedef void (__stdcall *MessageCallBack)(int socketID, int localSID, const char *str, int len);

typedef void (__stdcall *WriteLogCallBack)(int socketID, int localSID, int state, const char *log);

vector<MessageCallBack> m_clientCallBacks;

vector<WriteLogCallBack> m_clientLogs;

vector<MessageCallBack> m_serverCallBacks;

vector<WriteLogCallBack> m_serverLogs;

void __stdcall onClientMsg(int socketID, int localSID, const char *str, int len)
{
	vector<MessageCallBack>::iterator sIter = m_clientCallBacks.begin();
	for(; sIter != m_clientCallBacks.end(); ++sIter)
	{
		(*sIter)(socketID, localSID, str, len);
	}
}

void __stdcall onClientLog(int socketID, int localSID, int state, const char *log)
{
	cout << socketID << ":" << log << "\r\n";
	vector<WriteLogCallBack>::iterator sIter = m_clientLogs.begin();
	for(; sIter != m_clientLogs.end(); ++sIter)
	{
		(*sIter)(socketID, localSID, state, log);
	}
}

void __stdcall onServerMsg(int socketID, int localSID, const char *str, int len)
{
	vector<MessageCallBack>::iterator sIter = m_serverCallBacks.begin();
	for(; sIter != m_serverCallBacks.end(); ++sIter)
	{
		(*sIter)(socketID, localSID, str, len);
	}
}

void __stdcall onServerLog(int socketID, int localSID, int state, const char *log)
{
	cout << socketID << ":" << log << "\r\n";
	vector<WriteLogCallBack>::iterator sIter = m_serverLogs.begin();
	for(; sIter != m_serverLogs.end(); ++sIter)
	{
		(*sIter)(socketID, localSID, state, log);
	}
}

extern "C" __declspec(dllexport) int closeClient(int socketID)
{
	return FCServerSockets::close(socketID);
}

extern "C" __declspec(dllexport) int closeServerClient(int socketID, int clientSocketID)
{
	return FCServerSockets::closeClient(socketID, clientSocketID);
}

extern "C" __declspec(dllexport) int closeServer(int socketID)
{
	return FCServerSockets::close(socketID);
}

extern "C" __declspec(dllexport) int connectToServer(int type, int proxyType, const char *ip, int port, const char *proxyIp, int proxyPort, const char *proxyUserName, const char *proxyUserPwd, const char *proxyDomain, int timeout)
{
	int socketID = FCClientSockets::connect(type, proxyType, ip, port, proxyIp, proxyPort, proxyUserName, proxyUserPwd, proxyDomain, timeout);
	FCClientSockets::registerLog(&onClientLog);
	FCClientSockets::registerRecv(&onClientMsg);
	return socketID;
}

extern "C" __declspec(dllexport) void registerClientCallBack(MessageCallBack callBack)
{
	m_clientCallBacks.push_back(callBack);
}

extern "C" __declspec(dllexport) void registerClientLog(WriteLogCallBack callBack)
{
	m_clientLogs.push_back(callBack);
}

extern "C" __declspec(dllexport) void registerServerCallBack(MessageCallBack callBack)
{
	m_serverCallBacks.push_back(callBack);
}

extern "C" __declspec(dllexport) void registerServerLog(WriteLogCallBack callBack)
{
	m_serverLogs.push_back(callBack);
}

extern "C" __declspec(dllexport) int sendByClient(int socketID, const char *str, int len)
{
	return FCClientSockets::send(socketID, str, len);
}

extern "C" __declspec(dllexport) int sendByServer(int socketID, const char *str, int len)
{
	return FCServerSockets::send(socketID, str, len);
}

extern "C" __declspec(dllexport) int startServer(int type, int port)
{
	int socketID = FCServerSockets::start(type, port);
	FCServerSockets::registerLog(&onServerLog);
	FCServerSockets::registerRecv(&onServerMsg); 
	return socketID;
}

int main()
{
	int socketID = connectToServer(0, 0, "www.gaiafintech.com", 9962, "", 0, "", "", "", 6);
	startServer(0, 6000);
	system("pause");
	return 0;
}
