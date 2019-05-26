#include "stdafx.h"
#include "FaceCatScript.h"
#include "NFunctionEx.h"

FaceCatScript::FaceCatScript(FCUIXml *xml) : FCUIScript(xml)
{
    m_script = 0;
    m_xml = xml;
}

FaceCatScript::~FaceCatScript()
{
    if(m_script)
    {
        delete m_script;
        m_script = 0;
    }
    m_xml = 0;
}

String FaceCatScript::callFunction(const String& function)
{
    double value = m_script->callFunction(function);
    return FCStr::convertDoubleToStr(value);
}

String FaceCatScript::getProperty(const String& name, const String& propertyName)
{
    if (m_xml)
    {
        FCView *control = m_xml->findControl(name);
        if (control)
        {
            String value = L"", type = L"";
            control->getProperty(propertyName, &value, &type);
            return value;
        }
    }
    return L"";
}

String FaceCatScript::getSender()
{
    if (m_xml)
    {
        FCUIEvent *uiEvent = m_xml->getEvent();
        if (uiEvent)
        {
            return uiEvent->getSender();
        }
    }
    return 0;
}

void FaceCatScript::setProperty(const String& name, const String& propertyName, const String& propertyValue)
{
    if (m_xml)
    {
        FCView *control = m_xml->findControl(name);
        if (control)
        {
            control->setProperty(propertyName, propertyValue);
        }
    }
}

void FaceCatScript::setText(const String& text)
{
    m_script = NFunctionEx::createIndicator(text, m_xml);
}
