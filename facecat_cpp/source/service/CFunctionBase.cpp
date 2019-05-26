#include "..\\..\\stdafx.h"
#include "..\\..\\include\\service\\CFunctionBase.h"

static String FUNCTIONS = L"IN,OUT,SLEEP";
static String PREFIX = L"";
static int STARTINDEX = 1000;

namespace FaceCat
{
	CFunctionBase::CFunctionBase(FCScript *indicator, int cid, const String& name){
		m_indicator = indicator;
		m_ID = cid;
		m_name = name;
	}

	CFunctionBase::~CFunctionBase(){
		m_indicator = 0;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void CFunctionBase::addFunctions(FCScript *indicator){
		ArrayList<String> functions = FCStr::split(FUNCTIONS, L",");
		int functionsSize = (int)functions.size();
		for (int i = 0; i < functionsSize; i++){
			indicator->addFunction(new CFunctionBase(indicator, STARTINDEX + i, PREFIX + functions.get(i)));
		}
		functions.clear();
	}

	double CFunctionBase::onCalculate(CVariable *var){
		switch (var->m_functionID){
			case 1000:
				return INPUT(var);
			case 1001:
				return OUTPUT(var);
			case 1002:
				return SLEEP(var);
			default: return 0;
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	double CFunctionBase::INPUT(CVariable *var){
		return 0;
	}

	double CFunctionBase::OUTPUT(CVariable *var){
		int len = var->m_parametersLength;
        for (int i = 0; i < len; i++){
            String text = m_indicator->getText(var->m_parameters[i]);
			string sText = FCStr::wstringTostring(text);
			cout << sText.c_str();
        }
		cout << "\r\n";
        return 0;
	}

	double CFunctionBase::SLEEP(CVariable *var){
		::Sleep((int)m_indicator->getValue(var->m_parameters[0]));
		return 1;
	}
}
