#include "stdafx.h"
#include "BaseShape.h"

namespace FaceCat{
    CandleShape::CandleShape(){
        m_zOrder = 1;
        m_closeField = FCDataTable::NULLFIELD();
        m_downColor = FCColor::argb(84, 255, 255);
        m_highField = FCDataTable::NULLFIELD();
        m_lowField = FCDataTable::NULLFIELD();
        m_openField = FCDataTable::NULLFIELD();
        m_showMaxMin = true;
        m_style = CandleStyle_Rect;
        m_styleField = FCDataTable::NULLFIELD();
        m_tagColor = FCColor::argb(255, 255, 255);
        m_upColor = FCColor::argb(255, 82, 82);
        m_colorField = FCDataTable::NULLFIELD();
    }
    
    int CandleShape::getCloseField(){
        return m_closeField;
    }
    
    void CandleShape::setCloseField(int closeField){
        m_closeField = closeField;
    }
    
    String CandleShape::getCloseFieldText(){
        return m_closeFieldText;
    }
    
    void CandleShape::setCloseFieldText(const String& closeFieldText){
        m_closeFieldText = closeFieldText;
    }
    
    int CandleShape::getColorField(){
        return m_colorField;
    }
    
    void CandleShape::setColorField(int colorField){
        m_colorField = colorField;
    }
    
    Long CandleShape::getDownColor(){
        return m_downColor;
    }
    
    void CandleShape::setDownColor(Long downColor){
        m_downColor = downColor;
    }
    
    int CandleShape::getHighField(){
        return m_highField;
    }
    
    void CandleShape::setHighField(int highField){
        m_highField = highField;
    }
    
    String CandleShape::getHighFieldText(){
        return m_highFieldText;
    }
    
    void CandleShape::setHighFieldText(const String& highFieldText){
        m_highFieldText = highFieldText;
    }
    
    int CandleShape::getLowField(){
        return m_lowField;
    }
    
    void CandleShape::setLowField(int lowField){
        m_lowField = lowField;
    }
    
    String CandleShape::getLowFieldText(){
        return m_lowFieldText;
    }
    
    void CandleShape::setLowFieldText(const String& lowFieldText){
        m_lowFieldText = lowFieldText;
    }
    
    int CandleShape::getOpenField(){
        return m_openField;
    }
    
    void CandleShape::setOpenField(int openField){
        m_openField = openField;
    }
    
    String CandleShape::getOpenFieldText(){
        return m_openFieldText;
    }
    
    void CandleShape::setOpenFieldText(const String& openFieldText){
        m_openFieldText = openFieldText;
    }
    
    bool CandleShape::getShowMaxMin(){
        return m_showMaxMin;
    }
    
    void CandleShape::setShowMaxMin(bool showMaxMin){
        m_showMaxMin = showMaxMin;
    }
    
    CandleStyle CandleShape::getStyle(){
        return m_style;
    }
    
    void CandleShape::setStyle(CandleStyle style){
        m_style = style;
    }
    
    int CandleShape::getStyleField(){
        return m_styleField;
    }
    
    void CandleShape::setStyleField(int styleField){
        m_styleField = styleField;
    }
    
    Long CandleShape::getTagColor(){
        return m_tagColor;
    }
    
    void CandleShape::setTagColor(Long tagColor){
        m_tagColor = tagColor;
    }
    
    Long CandleShape::getUpColor(){
        return m_upColor;
    }
    
    void CandleShape::setUpColor(Long upColor){
        m_upColor = upColor;
    }
    
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    int CandleShape::getBaseField(){
        return m_closeField;
    }
    
    String CandleShape::getFieldText(int fieldName){
        if(fieldName == m_closeField){
            return m_closeFieldText;
        }
        if(fieldName == m_openField){
            return m_openFieldText;
        }
        if(fieldName == m_highField){
            return m_highFieldText;
        }
        if(fieldName == m_lowField){
            return m_lowFieldText;
        }
        return L"";
    }
    
    int* CandleShape::getFields(int *length){
        *length = 4;
        int *fields = new int[4];
        fields[0] = m_closeField;
        fields[1] = m_openField;
        fields[2] = m_highField;
        fields[3] = m_lowField;
        return fields;
    }
    
    void CandleShape::getProperty(const String& name, String *value, String *type){
        if (name == L"closefield"){
            *type = L"int";
            *value = FCStr::convertIntToStr(getCloseField());
        }
        else if (name == L"colorfield"){
            *type = L"int";
            *value = FCStr::convertIntToStr(getColorField());
        }
        else if (name == L"closefieldtext"){
            *type = L"text";
            *value = getCloseFieldText();
        }
        else if (name == L"downcolor"){
            *type = L"color";
            *value = FCStr::convertColorToStr(getDownColor());
        }
        else if (name == L"highfield"){
            *type = L"int";
            *value = FCStr::convertIntToStr(getHighField());
        }
        else if (name == L"highfieldtext"){
            *type = L"text";
            *value = getHighFieldText();
        }
        else if (name == L"lowfield"){
            *type = L"int";
            *value = FCStr::convertIntToStr(getLowField());
        }
        else if (name == L"lowfieldtext"){
            *type = L"text";
            *value = getLowFieldText();
        }
        else if (name == L"openfield"){
            *type = L"int";
            *value = FCStr::convertIntToStr(getOpenField());
        }
        else if (name == L"openfieldtext"){
            *type = L"text";
            *value = getOpenFieldText();
        }
        else if (name == L"showmaxmin"){
            *type = L"bool";
            *value = FCStr::convertBoolToStr(getShowMaxMin());
        }
        else if (name == L"style"){
            *type = L"enum:CandleStyle";
            CandleStyle style = getStyle();
            if (style == CandleStyle_American){
                *value = L"American";
            }
            else if (style == CandleStyle_CloseLine){
                *value = L"CloseLine";
            }
            else if (style == CandleStyle_Tower){
                *value = L"Tower";
            }
            else{
                *value = L"Rect";
            }
        }
        else if (name == L"stylefield"){
            *type = L"int";
            *value = FCStr::convertIntToStr(getStyleField());
        }
        else if (name == L"tagcolor"){
            *type = L"color";
            *value = FCStr::convertColorToStr(getTagColor());
        }
        else if (name == L"upcolor"){
            *type = L"color";
            *value = FCStr::convertColorToStr(getUpColor());
        }
        else{
            BaseShape::getProperty(name, value, type);
        }
    }
    
    ArrayList<String> CandleShape::getPropertyNames(){
        ArrayList<String> propertyNames = BaseShape::getPropertyNames();
        propertyNames.add(L"CloseField");
        propertyNames.add(L"ColorField");
        propertyNames.add(L"CloseFieldText");
        propertyNames.add(L"DownColor");
        propertyNames.add(L"DownColor");
        propertyNames.add(L"HighFieldText");
        propertyNames.add(L"LowField");
        propertyNames.add(L"LowFieldText");
        propertyNames.add(L"OpenField");
        propertyNames.add(L"OpenFieldText");
        propertyNames.add(L"ShowMaxMin");
        propertyNames.add(L"Style");
        propertyNames.add(L"StyleField");
        propertyNames.add(L"TagColor");
        propertyNames.add(L"UpColor");
        return propertyNames;
    }
    
    Long CandleShape::getSelectedColor(){
        return m_downColor;
    }
    
    void CandleShape::setProperty(const String& name, const String& value){
        if (name == L"closefield"){
            setCloseField(FCStr::convertStrToInt(value));
        }
        else if (name == L"colorfield"){
            setColorField(FCStr::convertStrToInt(value));
        }
        else if (name == L"closefieldtext"){
            setCloseFieldText(value);
        }
        else if (name == L"downcolor"){
            setDownColor(FCStr::convertStrToColor(value));
        }
        else if (name == L"highfield"){
            setHighField(FCStr::convertStrToInt(value));
        }
        else if (name == L"highfieldtext"){
            setHighFieldText(value);
        }
        else if (name == L"lowfield"){
            setLowField(FCStr::convertStrToInt(value));
        }
        else if (name == L"lowfieldtext"){
            setLowFieldText(value);
        }
        else if (name == L"openfield"){
            setOpenField(FCStr::convertStrToInt(value));
        }
        else if (name == L"openfieldtext"){
            setOpenFieldText(value);
        }
        else if (name == L"showmaxmin"){
            setShowMaxMin(FCStr::convertStrToBool(value));
        }
        else if (name == L"style"){
            String lowerStr = FCStr::toLower(value);
            if (lowerStr == L"american"){
                setStyle(CandleStyle_American);
            }
            else if (lowerStr == L"closeline"){
                setStyle(CandleStyle_CloseLine);
            }
            else if (lowerStr == L"tower"){
                setStyle(CandleStyle_Tower);
            }
            else{
                setStyle(CandleStyle_Rect);
            }
        }
        else if (name == L"stylefield"){
            setStyleField(FCStr::convertStrToInt(value));
        }
        else if (name == L"tagcolor"){
            setTagColor(FCStr::convertStrToColor(value));
        }
        else if (name == L"upcolor"){
            setUpColor(FCStr::convertStrToColor(value));
        }
        else{
            BaseShape::setProperty(name, value);
        }
    }
}
