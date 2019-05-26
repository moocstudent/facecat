#include "stdafx.h"
#include "BaseShape.h"

namespace FaceCat{
    PolylineShape::PolylineShape(){
        m_color = FCColor::argb(255, 255, 255);
        m_colorField = FCDataTable::NULLFIELD();
        m_fieldName = FCDataTable::NULLFIELD();
        m_fillColor = FCColor_None;
        m_width = 1;
        m_style = PolylineStyle_SolidLine;
        m_zOrder = 2;
    }
    
    Long PolylineShape::getColor(){
        return m_color;
    }
    
    void PolylineShape::setColor(Long color){
        m_color = color;
    }
    
    int PolylineShape::getColorField(){
        return m_colorField;
    }
    
    void PolylineShape::setColorField(int colorField){
        m_colorField = colorField;
    }
    
    int PolylineShape::getFieldName(){
        return m_fieldName;
    }
    
    void PolylineShape::setFieldName(int fieldName){
        m_fieldName = fieldName;
    }
    
    String PolylineShape::getFieldText(){
        return m_fieldText;
    }
    
    void PolylineShape::setFieldText(const String& fieldText){
        m_fieldText = fieldText;
    }
    
    Long PolylineShape::getFillColor(){
        return m_fillColor;
    }
    
    void PolylineShape::setFillColor(Long fillColor){
        m_fillColor = fillColor;
    }
    
    PolylineStyle PolylineShape::getStyle(){
        return m_style;
    }
    
    void PolylineShape::setStyle(PolylineStyle style){
        m_style = style;
    }
    
    float PolylineShape::getWidth(){
        return m_width;
    }
    
    void PolylineShape::setWidth(float width){
        m_width = width;
    }
    
    //////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    int PolylineShape::getBaseField(){
        return m_fieldName;
    }
    
    String PolylineShape::getFieldText(int fieldName){
        if(fieldName == m_fieldName){
            return m_fieldText;
        }
        return L"";
    }
    
    int* PolylineShape::getFields(int *length){
        *length = 1;
        int *fields = new int[1];
        fields[0] = m_fieldName;
        return fields;
    }
    
    void PolylineShape::getProperty(const String& name, String *value, String *type){
        if (name == L"color"){
            *type = L"color";
            *value = FCStr::convertColorToStr(getColor());
        }
        else if (name == L"colorfield"){
            *type = L"int";
            *value = FCStr::convertIntToStr(getColorField());
        }
        else if (name == L"fieldname"){
            *type = L"int";
            *value = FCStr::convertIntToStr(getFieldName());
        }
        else if (name == L"fieldtext"){
            *type = L"text";
            *value = getFieldText();
        }
        else if (name == L"fillcolor"){
            *type = L"color";
            *value = FCStr::convertColorToStr(getFillColor());
        }
        else if (name == L"style"){
            *type = L"enum:PolylineStyle";
            PolylineStyle style = getStyle();
            if (style == PolylineStyle_Cycle){
                *value = L"Cycle";
            }
            else if (style == PolylineStyle_DashLine){
                *value = L"DashLine";
            }
            else if (style == PolylineStyle_DotLine){
                *value = L"DotLine";
            }
            else{
                *value = L"SolidLine";
            }
        }
        else if (name == L"width"){
            *type = L"float";
            *value = FCStr::convertFloatToStr(getWidth());
        }
        else{
            BaseShape::getProperty(name, value, type);
        }
    }
    
    ArrayList<String> PolylineShape::getPropertyNames(){
        ArrayList<String> propertyNames = BaseShape::getPropertyNames();
        propertyNames.add(L"color");
        propertyNames.add(L"ColorField");
        propertyNames.add(L"FieldName");
        propertyNames.add(L"FieldText");
        propertyNames.add(L"FillColor");
        propertyNames.add(L"Style");
        propertyNames.add(L"Width");
        return propertyNames;
    }
    
    Long PolylineShape::getSelectedColor(){
        return m_color;
    }
    
    void PolylineShape::setProperty(const String& name, const String& value){
        if (name == L"color"){
            setColor(FCStr::convertStrToColor(value));
        }
        else if (name == L"colorfield"){
            setColorField(FCStr::convertStrToInt(value));
        }
        else if (name == L"fieldname"){
            setFieldName(FCStr::convertStrToInt(value));
        }
        else if (name == L"fieldtext"){
            setFieldText(value);
        }
        else if (name == L"fillcolor"){
            setFillColor(FCStr::convertStrToColor(value));
        }
        else if (name == L"style"){
            String lowerStr = FCStr::toLower(value);
            if (lowerStr == L"cycle"){
                setStyle(PolylineStyle_Cycle);
            }
            else if (lowerStr == L"dashline"){
                setStyle(PolylineStyle_DashLine);
            }
            else if (lowerStr == L"dotline"){
                setStyle(PolylineStyle_DotLine);
            }
            else{
                setStyle(PolylineStyle_SolidLine);
            }
        }
        else if (name == L"width"){
            setWidth(FCStr::convertStrToFloat(value));
        }
        else{
            BaseShape::setProperty(name, value);
        }
    }
}
