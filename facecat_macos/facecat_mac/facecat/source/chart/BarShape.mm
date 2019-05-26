#include "stdafx.h"
#include "BaseShape.h"

namespace FaceCat{
    BarShape::BarShape(){
        m_zOrder = 0;
        m_downColor = FCColor::argb(84, 255, 255);
        m_fieldName = FCDataTable::NULLFIELD();
        m_fieldName2 = FCDataTable::NULLFIELD();
        m_lineWidth = 1;
        m_style = BarStyle_Rect;
        m_styleField = FCDataTable::NULLFIELD();
        m_upColor = FCColor::argb(255, 82, 82);
        m_colorField = FCDataTable::NULLFIELD();
    }
    
    int BarShape::getColorField(){
        return m_colorField;
    }
    
    void BarShape::setColorField(int colorField){
        m_colorField = colorField;
    }
    
    Long BarShape::getDownColor(){
        return m_downColor;
    }
    
    void BarShape::setDownColor(Long downColor){
        m_downColor = downColor;
    }
    
    int BarShape::getFieldName(){
        return m_fieldName;
    }
    
    void BarShape::setFieldName(int fieldName){
        m_fieldName = fieldName;
    }
    
    int BarShape::getFieldName2(){
        return m_fieldName2;
    }
    
    void BarShape::setFieldName2(int fieldName2){
        m_fieldName2 = fieldName2;
    }
    
    String BarShape::getFieldText(){
        return m_fieldText;
    }
    
    void BarShape::setFieldText(const String& fieldText){
        m_fieldText = fieldText;
    }
    
    String BarShape::getFieldText2(){
        return m_fieldText2;
    }
    
    void BarShape::setFieldText2(const String& fieldText2){
        m_fieldText2 = fieldText2;
    }
    
    float BarShape::getLineWidth(){
        return m_lineWidth;
    }
    
    void BarShape::setLineWidth(float lineWidth){
        m_lineWidth = lineWidth;
    }
    
    BarStyle BarShape::getStyle(){
        return m_style;
    }
    
    void BarShape::setStyle(BarStyle style){
        m_style = style;
    }
    
    int BarShape::getStyleField(){
        return m_styleField;
    }
    
    void BarShape::setStyleField(int styleField){
        m_styleField = styleField;
    }
    
    Long BarShape::getUpColor(){
        return m_upColor;
    }
    
    void BarShape::setUpColor(Long upColor){
        m_upColor = upColor;
    }
    
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    int BarShape::getBaseField(){
        return m_fieldName;
    }
    
    String BarShape::getFieldText(int fieldName){
        if(fieldName == m_fieldName){
            return m_fieldText;
        }
        else if(fieldName == m_fieldName2){
            return m_fieldText2;
        }
        return L"";
    }
    
    int* BarShape::getFields(int *length){
        if(m_fieldName2 == FCDataTable::NULLFIELD()){
            *length = 1;
            int *fields = new int[1];
            fields[0] = m_fieldName;
            return fields;
        }
        else{
            *length = 2;
            int *fields = new int[2];
            fields[0] = m_fieldName;
            fields[1] = m_fieldName2;
            return fields;
        }
    }
    
    void BarShape::getProperty(const String& name, String *value, String *type){
        if (name == L"colorfield"){
            *type = L"int";
            *value = FCStr::convertIntToStr(getColorField());
        }
        else if(name == L"downcolor"){
            *type = L"color";
            *value = FCStr::convertColorToStr(getDownColor());
        }
        else if(name == L"fieldname"){
            *type = L"int";
            *value = FCStr::convertIntToStr(getFieldName());
        }
        else if (name == L"fieldname2"){
            *type = L"int";
            *value = FCStr::convertIntToStr(getFieldName2());
        }
        else if(name == L"fieldtext"){
            *type = L"text";
            *value = getFieldText();
        }
        else if(name == L"fieldtext2"){
            *type = L"text";
            *value = getFieldText2();
        }
        else if(name == L"linewidth"){
            *type = L"float";
            *value = FCStr::convertFloatToStr(getLineWidth());
        }
        else if(name == L"style"){
            *type = L"enum:BarStyle";
            BarStyle style = getStyle();
            if (style == BarStyle_Line){
                *value = L"Line";
            }
            else{
                *value = L"Rect";
            }
        }
        else if(name == L"stylefield"){
            *type = L"int";
            *value = FCStr::convertIntToStr(getStyleField());
        }
        else if(name == L"upcolor"){
            *type = L"color";
            *value = FCStr::convertColorToStr(getUpColor());
        }
        else{
            BaseShape::getProperty(name, value, type);
        }
    }
    
    ArrayList<String> BarShape::getPropertyNames(){
        ArrayList<String> propertyNames = BaseShape::getPropertyNames();
        propertyNames.add(L"ColorField");
        propertyNames.add(L"DownColor");
        propertyNames.add(L"FieldName");
        propertyNames.add(L"FieldName2");
        propertyNames.add(L"FieldText");
        propertyNames.add(L"FieldText2");
        propertyNames.add(L"LineWidth");
        propertyNames.add(L"Style");
        propertyNames.add(L"StyleField");
        propertyNames.add(L"UpColor");
        return propertyNames;
    }
    
    Long BarShape::getSelectedColor(){
        return m_downColor;
    }
    
    void BarShape::setProperty(const String& name, const String& value){
        if (name == L"colorfield"){
            setColorField(FCStr::convertStrToInt(value));
        }
        else if (name == L"downcolor"){
            setDownColor(FCStr::convertStrToColor(value));
        }
        else if (name == L"fieldname"){
            setFieldName(FCStr::convertStrToInt(value));
        }
        else if (name == L"fieldname2"){
            setFieldName2(FCStr::convertStrToInt(value));
        }
        else if (name == L"fieldtext"){
            setFieldText(value);
        }
        else if (name == L"fieldtext2"){
            setFieldText2(value);
        }
        else if (name == L"linewidth"){
            setLineWidth(FCStr::convertStrToFloat(value));
        }
        else if (name == L"style"){
            String lowerStr = FCStr::toLower(value);
            if (lowerStr == L"line"){
                setStyle(BarStyle_Line);
            }
            else{
                setStyle(BarStyle_Rect);
            }
        }
        else if (name == L"stylefield"){
            setStyleField(FCStr::convertStrToInt(value));
        }
        else if (name == L"upcolor"){
            setUpColor(FCStr::convertStrToColor(value));
        }
        else{
            BaseShape::setProperty(name, value);
        }
    }
}
