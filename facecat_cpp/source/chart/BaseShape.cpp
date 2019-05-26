#include "..\\..\\stdafx.h"
#include "..\\..\\include\\chart\\BaseShape.h"

namespace FaceCat{
	BaseShape::BaseShape(){
		m_allowUserPaint = false;
		m_attachVScale = AttachVScale_Left;
		m_selected = false;
		m_visible = true;
		m_zOrder = 0;
	}

	BaseShape::~BaseShape(){
	}

	bool BaseShape::allowUserPaint(){
		return m_allowUserPaint;
	}

	void BaseShape::setAllowUserPaint(bool allowUserPaint){
		m_allowUserPaint = allowUserPaint;
	}

	AttachVScale BaseShape::getAttachVScale(){
		return m_attachVScale;
	}

	void BaseShape::setAttachVScale(AttachVScale attachVScale){
		m_attachVScale = attachVScale;
	}

	bool BaseShape::isSelected(){
		return m_selected;
	}

	void BaseShape::setSelected(bool selected){
		m_selected = selected;
	}

	bool BaseShape::isVisible(){
		return m_visible;
	}

	void BaseShape::setVisible(bool visible){
		m_visible = visible;
	}

	int BaseShape::getZOrder(){
		return m_zOrder;
	}

	void BaseShape::setZOrder(int zOrder){
		m_zOrder = zOrder;
	}

	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	int BaseShape::getBaseField(){
		return FCDataTable::NULLFIELD();
	}

	String BaseShape::getFieldText(int fieldName){
		return L"";
	}

	int* BaseShape::getFields(int *length){
		return 0;
	}

	void BaseShape::getProperty(const String& name, String *value, String *type){
		if (name == L"allowuserpaint"){
			*type = L"bool";
			*value = FCStr::convertBoolToStr(allowUserPaint());
		}
		else if(name == L"attachvscale"){
			*type = L"enum:AttachVScale";
			AttachVScale attachVScale = getAttachVScale();
			if(attachVScale == AttachVScale_Left){
				*value = L"Left";
			}
			else{
				*value = L"Right";
			}
		}
		else if(name == L"selected"){
			*type = L"bool";
			*value = FCStr::convertBoolToStr(isSelected());
		}
		else if(name == L"visible"){
			*type = L"bool";
			*value = FCStr::convertBoolToStr(isVisible());
		}
		else if(name == L"zorder"){
			*type = L"int";
			*value = FCStr::convertIntToStr(getZOrder());
		}
		else{
            *type = L"undefined";
            *value = L"";
		}
	}

	ArrayList<String> BaseShape::getPropertyNames(){
		ArrayList<String> propertyNames;
		propertyNames.add(L"AllowUserPaint");
		propertyNames.add(L"attachvscale");
		propertyNames.add(L"selected");
		propertyNames.add(L"visible");
		propertyNames.add(L"zorder");
		return propertyNames;
	}

	Long BaseShape::getSelectedColor(){
		return 0;
	}

	void BaseShape::onPaint(FCPaint *paint, ChartDiv *div, const FCRect& rect){

	}

	void BaseShape::setProperty(const String& name, const String& value){
		if (name == L"allowuserpaint"){
			setAllowUserPaint(FCStr::convertStrToBool(value));
		}
		else if(name == L"attachvscale"){
			String lowerStr = FCStr::toLower(value);
			if(lowerStr == L"Left"){
				setAttachVScale(AttachVScale_Left);
			}
			else{
				setAttachVScale(AttachVScale_Right);
			}
		}
		else if(name == L"selected"){
			setSelected(FCStr::convertStrToBool(value));
		}
		else if(name == L"visible"){
			setVisible(FCStr::convertStrToBool(value));
		}
		else if(name == L"zorder"){
			setZOrder(FCStr::convertStrToInt(value));
		}
	}
}