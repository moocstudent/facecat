#include "..\\..\\stdafx.h"
#include "..\\..\\include\\service\\CFunctionHttp.h"
#include "..\\..\\include\\service\\FCServerService.h"

static String FUNCTIONS = L"POSTSTRING,QUERYSTRING,WRITE,ADDPORT,GETREQUESTURL,GETREQUESTMETHOD,GETCONTENTTYPE,SETSTATUSCODE,GETSERVICENAME,CHECKSCRIPT,CLOSE,HARDREQUEST,EASYREQUEST,GETREMOTEIP,GETREMOTEPORT";
static String PREFIX = L"HTTP.";
static int STARTINDEX = 4000;

namespace FaceCat{
	CFunctionHttp::CFunctionHttp(FCScript *indicator, int cid, const String& name){
		m_indicator = indicator;
		m_ID = cid;
		m_name = name;
		m_data = 0;
	}

	CFunctionHttp::~CFunctionHttp(){
		m_indicator = 0;
		m_data = 0;
	}

	FCHttpData* CFunctionHttp::getHttpData(){
		return m_data;
	}

	void CFunctionHttp::setHttpData(FCHttpData *data){
		m_data = data;
	}

	/////////////////////////////////////////////////////////////////////////////////////////	
	void CFunctionHttp::addFunctions(FCScript *indicator){
		ArrayList<String> functions = FCStr::split(FUNCTIONS, L",");
		int functionsSize = (int)functions.size();
		for (int i = 0; i < functionsSize; i++){
			indicator->addFunction(new CFunctionHttp(indicator, STARTINDEX + i, PREFIX + functions.get(i)));
		}
		functions.clear();
	}

	double CFunctionHttp::onCalculate(CVariable *var){	
		switch (var->m_functionID){
			case 4000:
				return HTTP_POSTSTRING(var);
			case 4001:
				return HTTP_QUERYSTRING(var);
			case 4002:
				return HTTP_WRITE(var);
			case 4003:
				return HTTP_ADDPORT(var);
			case 4004:
				return HTTP_GETREQUESTURL(var);
			case 4005:
				return HTTP_GETREQUESTMETHOD(var);
			case 4006:
				return HTTP_GETCONTENTTYPE(var);
			case 4007:
				return HTTP_SETSTATUSCODE(var);
			case 4008:
				return HTTP_GETSERVICENAME(var);
			case 4009:
				return HTTP_CHECKSCRIPT(var);
			case 4010:
				return HTTP_CLOSE(var);
			case 4011:
				return HTTP_HARDREQUEST(var);
			case 4012:
				return HTTP_EASYREQUEST(var);
			case 4013:
				return HTTP_GETREMOTEIP(var);
			case 4014:
				return HTTP_GETREMOTEPORT(var);
			default:
				return 0;
		}
	}

	double CFunctionHttp::HTTP_ADDPORT(CVariable *var){
		FCHttpMonitor::getMainMonitor()->setPort((int)m_indicator->getValue(var->m_parameters[0]));
		return 0;
	}

	double CFunctionHttp::HTTP_CHECKSCRIPT(CVariable *var){
		FCHttpMonitor::getMainMonitor()->checkScript();
		return 0;
	}

	double CFunctionHttp::HTTP_CLOSE(CVariable *var){
		m_data->m_close = true;
		return 0;
	}

	double CFunctionHttp::HTTP_EASYREQUEST(CVariable *var){
		String serviceName = m_indicator->getText(var->m_parameters[0]);
		FCHttpEasyService *easyService = FCHttpMonitor::getEasyService(serviceName);
		if(easyService){
			easyService->onReceive(m_data);
		}
		return 0;
	}

	double CFunctionHttp::HTTP_GETCONTENTTYPE(CVariable *var){
        CVariable *newVar = new CVariable;
		newVar->m_indicator = m_indicator;
		String expression = L"'";
		expression = expression.append(m_data->m_contentType);
		expression = expression.append(L"'");
        newVar->m_expression = expression;
        m_indicator->setVariable(var->m_parameters[0], newVar);
		return 0;
	}

	double CFunctionHttp::HTTP_GETREMOTEIP(CVariable *var){
        CVariable *newVar = new CVariable;
		newVar->m_indicator = m_indicator;
		String expression = L"'";
		expression = expression.append(m_data->m_remoteIP);
		expression = expression.append(L"'");
        newVar->m_expression = expression;
        m_indicator->setVariable(var->m_parameters[0], newVar);
		return 0;
	}

	double CFunctionHttp::HTTP_GETREMOTEPORT(CVariable *var){
		return m_data->m_remotePort;
	}

	double CFunctionHttp::HTTP_GETREQUESTMETHOD(CVariable *var){
        CVariable *newVar = new CVariable;
		newVar->m_indicator = m_indicator;
		String expression = L"'";
		expression = expression.append(m_data->m_method);
		expression = expression.append(L"'");
        newVar->m_expression = expression;
        m_indicator->setVariable(var->m_parameters[0], newVar);
		return 0;
	}

	double CFunctionHttp::HTTP_GETREQUESTURL(CVariable *var){
        CVariable *newVar = new CVariable;
		newVar->m_indicator = m_indicator;
		String expression = L"'";
		expression = expression.append(m_data->m_url);
		expression = expression.append(L"'");
        newVar->m_expression = expression;
        m_indicator->setVariable(var->m_parameters[0], newVar);
		return 0;
	}

	double CFunctionHttp::HTTP_GETSERVICENAME(CVariable *var){
        String url = m_data->m_url;
        int sindex = url.rfind(L'/');
        int eindex = url.find(L'?');
        String text = L"";
        if (eindex != -1){
            text = url.substr(sindex + 1, eindex - sindex - 1);
        }
        else{
            if (sindex + 1 == (int)url.length()){
                url = url.substr(0, (int)url.length() - 1);
                sindex = url.rfind('/');
            }
            text = url.substr(sindex + 1);
        }
		CVariable *newVar = new CVariable;
		newVar->m_indicator = m_indicator;
		String expression = L"'";
		expression = expression.append(text);
		expression = expression.append(L"'");
		newVar->m_expression = expression;
		m_indicator->setVariable(var->m_parameters[0], newVar);
		return 0;
	}

	double CFunctionHttp::HTTP_HARDREQUEST(CVariable *var){
		int length = (int)strlen(m_data->m_body);
		FCServerService::callBack((int)m_data->m_socket, 0, m_data->m_body, length);
		return 0;
	}

	double CFunctionHttp::HTTP_POSTREQUEST(CVariable *var){
		int length = (int)strlen(m_data->m_body);
		FCServerService::callBack((int)m_data->m_socket, 0, m_data->m_body, length);	
		return 0;
	}

	double CFunctionHttp::HTTP_POSTSTRING(CVariable *var){
        String wText = L"";
        if (m_data->m_body != 0)
        {
			string text(m_data->m_body);
			wText = FCStr::stringTowstring(text);
        }
		CVariable *newVar = new CVariable;
		newVar->m_indicator = m_indicator;
		String expression = L"'";
		expression = expression.append(wText);
		expression = expression.append(L"'");
		newVar->m_expression = expression;
        m_indicator->setVariable(var->m_parameters[0], newVar);
		return 0;
	}

	double CFunctionHttp::HTTP_QUERYSTRING(CVariable *var){
		String name = FCStr::toLower(m_indicator->getText(var->m_parameters[1]));
        String text = L"";
		map<String, String>::iterator sIter = m_data->m_parameters.find(name);
		if(sIter != m_data->m_parameters.end()){
			text = sIter->second;
		}
		CVariable *newVar = new CVariable;
		newVar->m_indicator = m_indicator;
		String expression = L"'";
		expression = expression.append(text);
		expression = expression.append(L"'");
		newVar->m_expression = expression;
        m_indicator->setVariable(var->m_parameters[0], newVar);
		return 0;
	}

	double CFunctionHttp::HTTP_SETSTATUSCODE(CVariable *var){
        m_data->m_statusCode = (int)m_indicator->getValue(var->m_parameters[0]);
		return 0;
	}

	double CFunctionHttp::HTTP_WRITE(CVariable *var){
		int len = var->m_parametersLength;
        for (int i = 0; i < len; i++){
            String text = m_indicator->getText(var->m_parameters[i]);
            m_data->m_resStr += text;
        }
        return 0;
	}
}
