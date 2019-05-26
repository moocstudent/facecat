#include "..\\..\\stdafx.h"
#include "..\\..\\include\\xml\\FCUIScript.h"

namespace FaceCat{
	FCUIScript::FCUIScript(FCUIXml *xml){
		m_xml = xml;
	}

	FCUIScript::~FCUIScript(){
		m_xml = 0;
	}

	FCUIXml* FCUIScript::getXml(){
		return m_xml;
	}

	void FCUIScript::setXml(FCUIXml *xml){
		m_xml = xml;
	}

	String FCUIScript::callFunction(const String& function){
		return L"";
	}

	void FCUIScript::setText(const String& text){
	}
}