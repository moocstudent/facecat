#include "..\\..\\stdafx.h"
#include "..\\..\\include\\chart\\SelectArea.h"

namespace FaceCat{
	SelectArea::SelectArea(){
		m_allowUserPaint = false;
		m_backColor = FCColor_None;
		m_canResize = true;
		m_enabled = true;
		m_lineColor = FCColor::argb(255, 255, 255);
		m_visible = false;
	}

	SelectArea::~SelectArea(){
	}

	bool SelectArea::allowUserPaint(){
		return m_allowUserPaint;
	}

	void SelectArea::setAllowUserPaint( bool allowUserPaint ){
		m_allowUserPaint = allowUserPaint;
	}

	Long SelectArea::getBackColor(){
		return m_backColor;
	}

	void SelectArea::setBackColor(Long backColor){
		m_backColor = backColor;
	}

	FCRect SelectArea::getBounds(){
		return m_bounds;
	}

	void SelectArea::setBounds(FCRect bounds){
		m_bounds = bounds;
	}

	bool SelectArea::canResize(){
		return m_canResize;
	}

	void SelectArea::setCanResize(bool canResize){
		m_canResize = canResize;
	}

	bool SelectArea::isEnabled(){
		return m_enabled;
	}

	void SelectArea::setEnabled(bool enabled){
		m_enabled = enabled;
	}

	Long SelectArea::getLineColor(){
		return m_lineColor;
	}

	void SelectArea::setLineColor(Long lineColor){
		m_lineColor = lineColor;
	}

	bool SelectArea::isVisible(){
		return m_visible;
	}

	void SelectArea::setVisible(bool visible){
		m_visible = visible;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void SelectArea::close(){
		m_visible = false;
		m_canResize = false;
	}

	void SelectArea::getProperty(const String& name, String *value, String *type){
		if (name == L"allowuserpaint"){
			*type = L"bool";
			*value = FCStr::convertBoolToStr(allowUserPaint());
		}
	    else if (name == L"enabled"){
            *type = L"bool";
			*value = FCStr::convertBoolToStr(isEnabled());
        }
        else if (name == L"linecolor"){
            *type = L"color";
			*value = FCStr::convertColorToStr(getLineColor());
        }
	}

	ArrayList<String> SelectArea::getPropertyNames(){
		ArrayList<String> propertyNames;
		propertyNames.add(L"AllowUserPaint");
		propertyNames.add(L"Enabled");
		propertyNames.add(L"LineColor");
		return propertyNames;
	}

	void SelectArea::onPaint(FCPaint *paint, ChartDiv *div, const FCRect& rect){

	}

	void SelectArea::setProperty(const String& name, const String& value){
		if (name == L"allowuserpaint"){
			setAllowUserPaint(FCStr::convertStrToBool(value));
		}
		else if (name == L"enabled"){
			setEnabled(FCStr::convertStrToBool(value));
		}
		else if (name == L"linecolor"){
			setLineColor(FCStr::convertStrToColor(value));
		}
	}
}