#include "..\\..\\stdafx.h"
#include "..\\..\\include\\service\\CFunctionEx.h"
#include "..\\..\\include\\service\\CFunctionBase.h"
#include "..\\..\\include\\service\\CFunctionHttp.h"

static String FUNCTIONS = L"";

namespace FaceCat{
	CFunctionEx::CFunctionEx(FCScript *indicator, int cid, const String& name, FCNative *native){
		m_indicator = indicator;
		m_ID = cid;
		m_name = name;
		m_native = native;
	}

	CFunctionEx::~CFunctionEx(){
		m_indicator = 0;
		m_native = 0;
	}

	/////////////////////////////////////////////////////////////////////////////////////////

	FCScript* CFunctionEx::createIndicator(const String& script, FCNative *native){
		FCScript *indicator = new FCScript();
		FCDataTable *table = new FCDataTable;
		indicator->setDataSource(table);
		int index = 1000000;
		ArrayList<String> functions = FCStr::split(FUNCTIONS, L",");
		int functionsSize = (int)functions.size();
		for (int i = 0; i < functionsSize; i++){
			indicator->addFunction(new CFunctionEx(indicator, index + i, functions.get(i), native));
		}
		CFunctionBase::addFunctions(indicator);
		CFunctionHttp::addFunctions(indicator);
		indicator->setScript(script);
		table->addColumn(0);
		table->set(0, 0, 0);
		indicator->onCalculate(0);
		functions.clear();
		return indicator;
	}

	double CFunctionEx::onCalculate(CVariable *var){
		switch (var->m_functionID){
			case 1000000:
				return 0;
			default:
				return 0;
		}
	}
}
