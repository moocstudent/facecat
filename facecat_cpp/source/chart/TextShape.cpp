#include "..\\..\\stdafx.h"
#include "..\\..\\include\\chart\\BaseShape.h"

namespace FaceCat{
	TextShape::TextShape(){
		m_zOrder = 4;
		m_colorField = FCDataTable::NULLFIELD();
		m_fieldName = FCDataTable::NULLFIELD();;
		m_font =  new FCFont;
		m_textColor = FCColor::argb(255, 255, 255);
		m_styleField = FCDataTable::NULLFIELD();;
	}

	TextShape::~TextShape(){
		if(m_font){
			delete m_font;
			m_font = 0;
		}
	}

	int TextShape::getColorField(){
		return m_colorField;
	}

	void TextShape::setColorField(int colorField){
		m_colorField = colorField;
	}

	int TextShape::getFieldName(){
		return m_fieldName;
	}

	void TextShape::setFieldName(int fieldName){
		m_fieldName = fieldName;
	}

	FCFont* TextShape::getFont(){
		return m_font;
	}

	void TextShape::setFont(FCFont *font){
		m_font->copy(font);
	}

	int TextShape::getStyleField(){
		return m_styleField;
	}

	void TextShape::setStyleField(int styleField){
		m_styleField = styleField;
	}

	String TextShape::getText(){
		return m_text;
	}

	void TextShape::setText(const String& text){
		m_text = text;
	}

	Long TextShape::getTextColor(){
		return m_textColor;
	}

	void TextShape::setTextColor(Long textColor){
		m_textColor = textColor;
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void TextShape::getProperty(const String& name, String *value, String *type){
	    if (name == L"colorfield"){
            *type = L"int";
			*value = FCStr::convertIntToStr(getColorField());
        }
        else if (name == L"fieldname"){
            *type = L"int";
			*value = FCStr::convertIntToStr(getFieldName());
        }
        else if (name == L"font"){
            *type = L"font";
			*value = FCStr::convertFontToStr(getFont());
        }
        else if (name == L"textcolor"){
            *type = L"color";
			*value = FCStr::convertColorToStr(getTextColor());
        }
        else if (name == L"stylefield"){
            *type = L"int";
			*value = FCStr::convertIntToStr(getStyleField());
        }
        else if (name == L"text"){
            *type = L"text";
            *value = getText();
        }
        else{
			BaseShape::getProperty(name, value, type);
        }
	}

	ArrayList<String> TextShape::getPropertyNames(){
		ArrayList<String> propertyNames;
		propertyNames.add(L"ColorField");
		propertyNames.add(L"FieldName");
		propertyNames.add(L"Font");
		propertyNames.add(L"StyleField");
		propertyNames.add(L"Text");
		propertyNames.add(L"TextColor");
		return propertyNames;
	}

	void TextShape::setProperty(const String& name, const String& value){
	    if (name == L"colorfield"){
			setColorField(FCStr::convertStrToInt(value));
        }
        else if (name == L"fieldname"){
			setFieldName(FCStr::convertStrToInt(value));
        }
        else if (name == L"font"){
			setFont(FCStr::convertStrToFont(value));
        }
        else if (name == L"textcolor"){
			setTextColor(FCStr::convertStrToColor(value));
        }
        else if (name == L"stylefield"){
			setStyleField(FCStr::convertStrToInt(value));
        }
        else if (name == L"text"){
            setText(value);
        }
        else{
			BaseShape::setProperty(name, value);
        }
	}
}