#include "stdafx.h"
#include "NFunctionBase.h"

static String FUNCTIONS = L"IN,OUT,SLEEP";
static String PREFIX = L"";
static const int STARTINDEX = 1000;

NFunctionBase::NFunctionBase(FCScript *indicator, int cid, const String& name){
    m_indicator = indicator;
    m_ID = cid;
    m_name = name;
}

NFunctionBase::~NFunctionBase(){
    m_indicator = 0;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void NFunctionBase::addFunctions(FCScript *indicator){
    ArrayList<String> functions = FCStr::split(FUNCTIONS, L",");
    int functionsSize = (int)functions.size();
    for (int i = 0; i < functionsSize; i++){
        indicator->addFunction(new NFunctionBase(indicator, STARTINDEX + i, PREFIX + functions.get(i)));
    }
    functions.clear();
}

double NFunctionBase::onCalculate(CVariable *var){
    switch (var->m_functionID){
        case STARTINDEX + 0:
            return IN(var);
        case STARTINDEX + 1:
            return OUT(var);
        case STARTINDEX + 2:
            return SLEEP(var);
        default: return 0;
    }
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

double NFunctionBase::IN(CVariable *var){
    return 0;
}

double NFunctionBase::OUT(CVariable *var){
    return 0;
}

double NFunctionBase::SLEEP(CVariable *var){
    return 1;
}
