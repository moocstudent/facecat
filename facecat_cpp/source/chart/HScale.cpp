#include "..\\..\\stdafx.h"
#include "..\\..\\include\\chart\\HScale.h"

namespace FaceCat{
	HScale::HScale(){
		m_allowUserPaint = false;
        m_dateColors.put(DateType_Year, FCColor::argb(255, 255, 255));
        m_dateColors.put(DateType_Month, FCColor::argb(150, 0, 0));
        m_dateColors.put(DateType_Day, FCColor::argb(100, 100, 100));
        m_dateColors.put(DateType_Hour, FCColor::argb(0, 0, 255));
        m_dateColors.put(DateType_Minute, FCColor::argb(255, 255, 0));
        m_dateColors.put(DateType_Second, FCColor::argb(255, 0, 255));
        m_dateColors.put(DateType_Millisecond, FCColor::argb(255, 0, 255));
		m_crossLineTip = new CrossLineTip;
		m_font = new FCFont(L"Arial", 14, false, false, false);
		m_textColor = FCColor::argb(255, 255, 255);
		m_height = 0;
		m_interval = 60;
		m_scaleColor = FCColor::argb(150, 0, 0);
		m_visible = true;
		m_hScaleType = HScaleType_Date;
	}

	HScale::~HScale(){
		if(m_font){
			delete m_font;
			m_font = 0;
		}
		if(m_crossLineTip){
			delete m_crossLineTip;
			m_crossLineTip = 0;
		}
		m_dateColors.clear();
		m_scaleSteps.clear();
	}

	bool HScale::allowUserPaint(){
		return m_allowUserPaint;
	}

	void HScale::setAllowUserPaint(bool allowUserPaint){
		m_allowUserPaint = allowUserPaint;
	}

	CrossLineTip* HScale::getCrossLineTip(){
		return m_crossLineTip;
	}

	Long HScale::getDateColor(DateType dateType){
		return m_dateColors.get(dateType);
	}

	void HScale::setDateColor(DateType dateType, Long color){
		m_dateColors.put(dateType, color);
	}

	FCFont* HScale::getFont(){
		return m_font;
	}

	void HScale::setFont(FCFont *font){
		m_font->copy(font);
	}

	int HScale::getHeight(){
		return m_height;
	}

	void HScale::setHeight(int height){
		m_height = height;
	}

	HScaleType HScale::getHScaleType(){
		return m_hScaleType;
	}

	void HScale::setHScaleType(HScaleType hScaleType){
		m_hScaleType = hScaleType;
	}

	int HScale::getInterval(){
		return m_interval;
	}

	void HScale::setInterval(int interval){
		m_interval = interval;
	}

	Long HScale::getScaleColor(){
		return m_scaleColor;
	}

	void HScale::setScaleColor(Long scaleColor){
		m_scaleColor = scaleColor;
	}

	Long HScale::getTextColor(){
		return m_textColor;
	}

	void HScale::setTextColor(Long textColor){
		m_textColor = textColor;
	}

	bool HScale::isVisible(){
		return m_visible;
	}

	void HScale::setVisible(bool visible){
		m_visible = visible;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void HScale::getProperty(const String& name, String *value, String *type){
		if (name == L"allowuserpaint"){
			*type = L"bool";
			*value = FCStr::convertBoolToStr(allowUserPaint());
		}
	    else if (name == L"font"){
            *type = L"font";
			*value = FCStr::convertFontToStr(getFont());
        }
        else if (name == L"textcolor"){
            *type = L"color";
			*value = FCStr::convertColorToStr(getTextColor());
        }
        else if (name == L"height"){
            *type = L"int";
			*value = FCStr::convertIntToStr(getHeight());
        }
        else if (name == L"interval"){
            *type = L"int";
			*value = FCStr::convertIntToStr(getInterval());
        }
        else if (name == L"scalecolor"){
            *type = L"color";
			*value = FCStr::convertColorToStr(getScaleColor());
        }
		else if (name == L"type"){
			*type = L"enum:HScaleType";
			HScaleType hScaleType = getHScaleType();
			if (hScaleType == HScaleType_Date){
				*value = L"Date";
			}
			else{
				*value = L"Number";
			}
		}
        else if (name == L"visible"){
            *type = L"bool";
			*value = FCStr::convertBoolToStr(isVisible());
        }
	}

	ArrayList<String> HScale::getPropertyNames(){
		ArrayList<String> propertyNames;
		propertyNames.add(L"AllowUserPaint");
		propertyNames.add(L"Font");
		propertyNames.add(L"Height");
		propertyNames.add(L"Interval");
		propertyNames.add(L"ScaleColor");
		propertyNames.add(L"TextColor");
		propertyNames.add(L"Type");
		propertyNames.add(L"Visible");
		return propertyNames;
	}

	ArrayList<double> HScale::getScaleSteps(){
		return m_scaleSteps;
	}

	void HScale::onPaint(FCPaint *paint, ChartDiv *div, const FCRect& rect){

	}

	void HScale::setProperty(const String& name, const String& value){
		if (name == L"allowuserpaint"){
			setAllowUserPaint(FCStr::convertStrToBool(value));
		}
	    else if (name == L"font"){
			setFont(FCStr::convertStrToFont(value));
        }
        else if (name == L"textcolor"){
			setTextColor(FCStr::convertStrToColor(value));
        }
        else if (name == L"height"){
			setHeight(FCStr::convertStrToInt(value));
        }
        else if (name == L"interval"){
			setInterval(FCStr::convertStrToInt(value));
        }
        else if (name == L"scalecolor"){
			setScaleColor(FCStr::convertStrToColor(value));
        }
		else if (name == L"type"){
			String lowerStr = FCStr::toLower(value);
			if (lowerStr == L"date"){
				setHScaleType(HScaleType_Date);
			}
			else{
				setHScaleType(HScaleType_Number);
			}
		}
        else if (name == L"visible"){
			setVisible(FCStr::convertStrToBool(value));
        }
	}

	void HScale::setScaleSteps(ArrayList<double> scaleSteps){
		m_scaleSteps = scaleSteps;
	}
}