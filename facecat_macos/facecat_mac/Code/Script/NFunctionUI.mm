#include "stdafx.h"
#include "NFunctionUI.h"
#include "FaceCatScript.h"
#include "BarrageDiv.h"
#include "UIXmlEx.h"

static String FUNCTIONS = L"GETPROPERTY,SETPROPERTY,GETSENDER,ALERT,INVALIDATE,SHOWWINDOW,CLOSEWINDOW,STARTTIMER,STOPTIMER,GETCOOKIE,SETCOOKIE,SHOWRIGHTMENU,ADDBARRAGE";
static String PREFIX = L"";
static const int STARTINDEX = 2000;

NFunctionUI::NFunctionUI(FCScript *indicator, int cid, const String& name, FCUIXml *xml){
    m_indicator = indicator;
    m_ID = cid;
    m_name = name;
    m_xml = xml;
}

NFunctionUI::~NFunctionUI(){
    m_indicator = 0;
    m_xml = 0;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void NFunctionUI::addFunctions(FCScript *indicator, FCUIXml *xml){
    ArrayList<String> functions = FCStr::split(FUNCTIONS, L",");
    int functionsSize = (int)functions.size();
    for (int i = 0; i < functionsSize; i++){
        indicator->addFunction(new NFunctionUI(indicator, STARTINDEX + i, PREFIX + functions.get(i), xml));
    }
    functions.clear();
}

double NFunctionUI::onCalculate(CVariable *var){
    switch (var->m_functionID){
        case STARTINDEX + 0:
            return GETPROPERTY(var);
        case STARTINDEX + 1:
            return SETPROPERTY(var);
        case STARTINDEX + 2:
            return GETSENDER(var);
        case STARTINDEX + 3:
            return ALERT(var);
        case STARTINDEX + 4:
            return INVALIDATE(var);
        case STARTINDEX + 5:
            return SHOWWINDOW(var);
        case STARTINDEX + 6:
            return CLOSEWINDOW(var);
        case STARTINDEX + 7:
            return STARTTIMER(var);
        case STARTINDEX + 8:
            return STOPTIMER(var);
        case STARTINDEX + 9:
            return GETCOOKIE(var);
        case STARTINDEX + 10:
            return SETCOOKIE(var);
        case STARTINDEX + 11:
            return SHOWRIGHTMENU(var);
        case STARTINDEX + 12:
            return ADDBARRAGE(var);
        default:
            return 0;
    }
}

double NFunctionUI::ADDBARRAGE(CVariable *var){
	String text = L"";
    int len = var->m_parametersLength;
    for (int i = 0; i < len; i++){
        text += m_indicator->getText(var->m_parameters[i]);
    }
    BarrageDiv *barrageDiv = dynamic_cast<BarrageDiv*>(m_xml->findControl(L"divBarrage"));
    Barrage *barrage = new Barrage;
    barrage->setText(text);
    barrage->setMode(0);
    barrageDiv->addBarrage(barrage);
    return 1;
}

double NFunctionUI::ALERT(CVariable *var){
    return 0;
}

double NFunctionUI::CLOSEWINDOW(CVariable *var){
	WindowXmlEx *windowXmlEx = dynamic_cast<WindowXmlEx*>(m_xml);
	if (windowXmlEx)
	{
		windowXmlEx->close();
	}
	return 0;
}

double NFunctionUI::GETCOOKIE(CVariable *var){
    return 0;
}

double NFunctionUI::GETPROPERTY(CVariable *var){
    FaceCatScript *fScript = dynamic_cast<FaceCatScript*>(m_xml->getScript());
    String name = m_indicator->getText(var->m_parameters[1]);
    String propertyName = m_indicator->getText(var->m_parameters[2]);
    String text = fScript->getProperty(name, propertyName);
    CVariable newVar;
	newVar.m_indicator = m_indicator;
    newVar.m_expression = L"'" + text + L"'";
    m_indicator->setVariable(var->m_parameters[0], &newVar);
    return 0;
}

double NFunctionUI::GETSENDER(CVariable *var){
    FaceCatScript *fScript = dynamic_cast<FaceCatScript*>(m_xml->getScript());
    String text = fScript->getSender();
    CVariable newVar;
	newVar.m_indicator = m_indicator;
    newVar.m_expression = L"'" + text + L"'";
    m_indicator->setVariable(var->m_parameters[0], &newVar);
    return 0;
}

double NFunctionUI::INVALIDATE(CVariable *var){
    if (m_xml){
        int pLen = var->m_parameters ? var->m_parametersLength : 0;
        if (pLen == 0){
            m_xml->getNative()->invalidate();
        }
        else{
            FCView *control = m_xml->findControl(m_indicator->getText(var->m_parameters[0]));
            if (control){
                control->invalidate();
            }
        }
    }
    return 0;
}

double NFunctionUI::SETCOOKIE(CVariable *var){
    return 0;
}

double NFunctionUI::SETPROPERTY(CVariable *var){
    FaceCatScript *fScript = dynamic_cast<FaceCatScript*>(m_xml->getScript());
    String name = m_indicator->getText(var->m_parameters[0]);
    String propertyName = m_indicator->getText(var->m_parameters[1]);
    String propertyValue = m_indicator->getText(var->m_parameters[2]);
    fScript->setProperty(name, propertyName, propertyValue);
    return 0;
}

double NFunctionUI::SHOWRIGHTMENU(CVariable *var){
    FaceCatScript *fScript = dynamic_cast<FaceCatScript*>(m_xml->getScript());
    FCNative *native = m_xml->getNative();
    FCView *control = m_xml->findControl(fScript->getSender());
    int clientX = native->clientX(control);
    int clientY = native->clientY(control);
    FCMenu *menu = m_xml->getMenu(m_indicator->getText(var->m_parameters[0]));
	FCPoint mp = {clientX, clientY + control->getHeight()};
	menu->setLocation(mp);
    menu->setVisible(true);
    menu->setFocused(true);
    menu->bringToFront();
    native->invalidate();
    return 0;
}

double NFunctionUI::SHOWWINDOW(CVariable *var){
    String xmlName = m_indicator->getText(var->m_parameters[0]);
    String windowName = m_indicator->getText(var->m_parameters[1]);
    WindowXmlEx *window = new WindowXmlEx;
    window->load(m_xml->getNative(), xmlName, windowName);
    window->show();
	return 0;
}

double NFunctionUI::STARTTIMER(CVariable *var){
    FCView *control = m_xml->findControl(m_indicator->getText(var->m_parameters[0]));
    control->startTimer((int)m_indicator->getValue(var->m_parameters[1]), (int)m_indicator->getValue(var->m_parameters[2]));
    return 0;
}

double NFunctionUI::STOPTIMER(CVariable *var){
    FCView *control = m_xml->findControl(m_indicator->getText(var->m_parameters[0]));
    control->stopTimer((int)m_indicator->getValue(var->m_parameters[1]));
    return 0;
}
