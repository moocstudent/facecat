#include "..\\..\\stdafx.h"
#include "..\\..\\include\\chart\\ChartToolTip.h"

namespace FaceCat{
	ChartToolTip::ChartToolTip(){
		m_allowUserPaint = false;
		m_backColor = FCColor::argb(255, 255, 128);
		m_borderColor = FCColor::argb(255, 255, 80);
		m_font = new FCFont;
		m_textColor = FCColor::argb(0, 0, 0);
	}

	ChartToolTip::~ChartToolTip(){
		if(m_font){
			delete m_font;
			m_font = 0;
		}
	}

	bool ChartToolTip::allowUserPaint(){
		return m_allowUserPaint;
	}

	void ChartToolTip::setAllowUserPaint( bool allowUserPaint ){
		m_allowUserPaint = allowUserPaint;
	}

	Long ChartToolTip::getBackColor(){
		return m_backColor;
	}

	void ChartToolTip::setBackColor(Long backColor){
		m_backColor = backColor;
	}

	Long ChartToolTip::getBorderColor(){
		return m_borderColor;
	}

	void ChartToolTip::setBorderColor(Long borderColor){
		m_borderColor = borderColor;
	}

	FCFont* ChartToolTip::getFont(){
		return m_font;
	}

	void ChartToolTip::setFont(FCFont *font){
		m_font->copy(font);
	}

	Long ChartToolTip::getTextColor(){
		return m_textColor;
	}

	void ChartToolTip::setTextColor(Long textColor){
		m_textColor = textColor;
	}

	/////////////////////////////////////////////////////////////////////////////////////////////////////////

	void ChartToolTip::getProperty(const String& name, String *value, String *type){
		if (name == L"allowuserpaint"){
			*type = L"bool";
			*value = FCStr::convertBoolToStr(allowUserPaint());
		}
	    else if (name == L"backcolor"){
            *type = L"color";
			*value = FCStr::convertColorToStr(getBackColor());
        }
        else if (name == L"bordercolor"){
            *type = L"color";
			*value = FCStr::convertColorToStr(getBorderColor());
        }
        else if (name == L"font"){
            *type = L"font";
			*value = FCStr::convertFontToStr(getFont());
        }
        else if (name == L"textcolor"){
            *type = L"color";
			*value = FCStr::convertColorToStr(getTextColor());
        }
	}

	ArrayList<String> ChartToolTip::getPropertyNames(){
		ArrayList<String> propertyNames;
		propertyNames.add(L"AllowUserPaint");
		propertyNames.add(L"BackColor");
		propertyNames.add(L"BorderColor");
		propertyNames.add(L"Font");
		propertyNames.add(L"TextColor");
		return propertyNames;
	}

	void ChartToolTip::onPaint(FCPaint *paint, ChartDiv *div, const FCRect& rect){

	}

	void ChartToolTip::setProperty(const String& name, const String& value){
		if (name == L"allowuserpaint"){
			setAllowUserPaint(FCStr::convertStrToBool(value));
		}
	    else if (name == L"backcolor"){
			setBackColor(FCStr::convertStrToColor(value));
        }
        else if (name == L"bordercolor"){
			setBorderColor(FCStr::convertStrToColor(value));
        }
        else if (name == L"font"){
			setFont(FCStr::convertStrToFont(value));
        }
        else if (name == L"textcolor"){
			setTextColor(FCStr::convertStrToColor(value));
        }
	}
}