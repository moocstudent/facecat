#include "..\\..\\stdafx.h"
#include "..\\..\\include\\chart\\CrossLine.h"

namespace FaceCat{
	CrossLine::CrossLine(){
		m_allowUserPaint = false;
		m_allowDoubleClick = true;
		m_attachVScale = AttachVScale_Left;
		m_lineColor = FCColor::argb(100, 100, 100);
	}

	CrossLine::~CrossLine(){
	}

	bool CrossLine::allowDoubleClick(){
		return m_allowDoubleClick;
	}

	void CrossLine::setAllowDoubleClick(bool allowDoubleClick){
		m_allowDoubleClick = allowDoubleClick;
	}

	bool CrossLine::allowUserPaint(){
		return m_allowUserPaint;
	}

	void CrossLine::setAllowUserPaint(bool allowUserPaint){
		m_allowUserPaint = allowUserPaint;
	}

	AttachVScale CrossLine::getAttachVScale(){
		return m_attachVScale;
	}

	void CrossLine::setAttachVScale(AttachVScale attachVScale){
		m_attachVScale = attachVScale;
	}

	Long CrossLine::getLineColor(){
		return m_lineColor;
	}

	void CrossLine::setLineColor(Long lineColor){
		m_lineColor = lineColor;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void CrossLine::getProperty(const String& name, String *value, String *type){
	    if (name == L"allowdoubleclick"){
            *type = L"bool";
			*value = FCStr::convertBoolToStr(allowDoubleClick());
        }
		else if (name == L"allowuserpaint"){
			*type = L"bool";
			*value = FCStr::convertBoolToStr(allowUserPaint());
		}
        else if (name == L"attachvscale"){
			*type = L"enum:AttachVScale";
			AttachVScale attachVScale = getAttachVScale();
			if(attachVScale == AttachVScale_Left){
				*value = L"Left";
			}
			else{
				*value = L"Right";
			}
        }
        else if (name == L"linecolor"){
            *type = L"color";
			*value = FCStr::convertColorToStr(getLineColor());
        }
	}

	ArrayList<String> CrossLine::getPropertyNames(){
		ArrayList<String> propertyNames;
		propertyNames.add(L"AllowDoubleClick");
		propertyNames.add(L"AllowUserPaint");
		propertyNames.add(L"AttachVScale");
		propertyNames.add(L"LineColor");
		return propertyNames;
	}

	void CrossLine::onPaint(FCPaint *paint, ChartDiv *div, const FCRect& rect){

	}

	void CrossLine::setProperty(const String& name, const String& value){
		if(name == L"allowdoubleclick"){
			setAllowDoubleClick(FCStr::convertStrToBool(value));
		}
		else if (name == L"allowuserpaint"){
			setAllowUserPaint(FCStr::convertStrToBool(value));
		}
		else if(name == L"attachvscale"){
			String lowerStr = FCStr::toLower(value);
			if(lowerStr == L"left"){
				setAttachVScale(AttachVScale_Left);
			}
			else{
				setAttachVScale(AttachVScale_Right);
			}
		}
		else if(name == L"linecolor"){
			setLineColor(FCStr::convertStrToColor(value));
		}
	}
}