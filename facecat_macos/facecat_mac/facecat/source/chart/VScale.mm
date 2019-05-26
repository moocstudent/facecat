#include "stdafx.h"
#include "VScale.h"

namespace FaceCat{
    VScale::VScale(){
        m_allowUserPaint = false;
        m_autoMaxMin = true;
        m_baseField = FCDataTable::NULLFIELD();
        m_crossLineTip = new CrossLineTip;
        m_digit = 2;
        m_textColor = FCColor::argb(255, 82, 82);
        m_textColor2 = FCColor_None;
        m_font = new FCFont(L"Arial", 14, true, false, false);
        m_magnitude = 1;
        m_paddingBottom = 0;
        m_paddingTop = 0;
        m_midValue = 0;
        m_numberStyle = NumberStyle_Standard;
        m_reverse = false;
        m_scaleColor = FCColor::argb(150, 0, 0);
        m_system = VScaleSystem_Standard;
        m_type = VScaleType_EqualDiff;
        m_visibleMax = 0;
        m_visibleMin = 0;
    }
    
    VScale::~VScale(){
        if(m_crossLineTip){
            delete m_crossLineTip;
            m_crossLineTip = 0;
        }
        if(m_font){
            delete m_font;
            m_font = 0;
        }
        m_scaleSteps.clear();
    }
    
    bool VScale::allowUserPaint(){
        return m_allowUserPaint;
    }
    
    void VScale::setAllowUserPaint(bool allowUserPaint){
        m_allowUserPaint = allowUserPaint;
    }
    
    bool VScale::autoMaxMin(){
        return m_autoMaxMin;
    }
    
    void VScale::setAutoMaxMin(bool autoMaxMin){
        m_autoMaxMin = autoMaxMin;
    }
    
    int VScale::getBaseField(){
        return m_baseField;
    }
    
    void VScale::setBaseField(int baseField){
        m_baseField = baseField;
    }
    
    CrossLineTip* VScale::getCrossLineTip(){
        return m_crossLineTip;
    }
    
    int VScale::getDigit(){
        return m_digit;
    }
    
    void VScale::setDigit(int digit){
        m_digit = digit;
    }
    
    FCFont* VScale::getFont(){
        return m_font;
    }
    
    void VScale::setFont(FCFont *font){
        m_font->copy(font);
    }
    
    int VScale::getMagnitude(){
        return m_magnitude;
    }
    
    void VScale::setMagnitude(int magnitude){
        m_magnitude = magnitude;
    }
    
    double VScale::getMidValue(){
        return m_midValue;
    }
    
    void VScale::setMidValue(double midValue){
        m_midValue = midValue;
    }
    
    NumberStyle VScale::getNumberStyle(){
        return m_numberStyle;
    }
    
    void VScale::setNumberStyle(NumberStyle numberStyle){
        m_numberStyle = numberStyle;
    }
    
    int VScale::getPaddingBottom(){
        return m_paddingBottom;
    }
    
    void VScale::setPaddingBottom(int paddingBottom){
        m_paddingBottom = paddingBottom;
    }
    
    int VScale::getPaddingTop(){
        return m_paddingTop;
    }
    
    void VScale::setPaddingTop(int paddingTop){
        m_paddingTop = paddingTop;
    }
    
    bool VScale::isReverse(){
        return m_reverse;
    }
    
    void VScale::setReverse(bool reverse){
        m_reverse = reverse;
    }
    
    Long VScale::getScaleColor(){
        return m_scaleColor;
    }
    
    void VScale::setScaleColor(Long scaleColor){
        m_scaleColor = scaleColor;
    }
    
    VScaleSystem VScale::getSystem(){
        return m_system;
    }
    
    void VScale::setSystem(VScaleSystem system){
        m_system = system;
    }
    
    Long VScale::getTextColor(){
        return m_textColor;
    }
    
    void VScale::setTextColor(Long textColor){
        m_textColor = textColor;
    }
    
    Long VScale::getTextColor2(){
        return m_textColor2;
    }
    
    void VScale::setTextColor2(Long textColor2){
        m_textColor2 = textColor2;
    }
    
    VScaleType VScale::getType(){
        return m_type;
    }
    
    void VScale::setType(VScaleType type){
        m_type = type;
    }
    
    double VScale::getVisibleMax(){
        return m_visibleMax;
    }
    
    void VScale::setVisibleMax(double visibleMax){
        m_visibleMax = visibleMax;
    }
    
    double VScale::getVisibleMin(){
        return m_visibleMin;
    }
    
    void VScale::setVisibleMin(double visibleMin){
        m_visibleMin = visibleMin;
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    void VScale::getProperty(const String& name, String *value, String *type){
        if (name == L"allowuserpaint"){
            *type = L"bool";
            *value = FCStr::convertBoolToStr(allowUserPaint());
        }
        else if(name == L"automaxmin"){
            *type = L"bool";
            *value = FCStr::convertBoolToStr(autoMaxMin());
        }
        else if (name == L"basefield"){
            *type = L"int";
            *value = FCStr::convertIntToStr(getBaseField());
        }
        else if (name == L"digit"){
            *type = L"int";
            *value = FCStr::convertIntToStr(getDigit());
        }
        else if (name == L"font"){
            *type = L"font";
            *value = FCStr::convertFontToStr(getFont());
        }
        else if (name == L"textcolor"){
            *type = L"color";
            *value = FCStr::convertColorToStr(getTextColor());
        }
        else if (name == L"textcolor2"){
            *type = L"color";
            *value = FCStr::convertColorToStr(getTextColor2());
        }
        else if (name == L"magnitude"){
            *type = L"int";
            *value = FCStr::convertIntToStr(getMagnitude());
        }
        else if(name == L"midvalue"){
            *type = L"double";
            *value = FCStr::convertDoubleToStr(getMidValue());
        }
        else if (name == L"numberstyle"){
            *type = L"enum:NumberStyle";
            NumberStyle style = getNumberStyle();
            if (style == NumberStyle_Standard){
                *value = L"Standard";
            }
            else{
                *value = L"UnderLine";
            }
        }
        else if (name == L"paddingbottom"){
            *type = L"int";
            *value = FCStr::convertIntToStr(getPaddingBottom());
        }
        else if (name == L"paddingtop"){
            *type = L"int";
            *value = FCStr::convertIntToStr(getPaddingTop());
        }
        else if (name == L"reverse"){
            *type = L"bool";
            *value = FCStr::convertBoolToStr(isReverse());
        }
        else if (name == L"scalecolor"){
            *type = L"color";
            *value = FCStr::convertColorToStr(getScaleColor());
        }
        else if (name == L"system"){
            *type = L"enum:VScaleSystem";
            VScaleSystem system = getSystem();
            if (system == VScaleSystem_Logarithmic){
                *value = L"Log";
            }
            else{
                *value = L"Standard";
            }
        }
        else if (name == L"type"){
            *type = L"enum:VScaleType";
            VScaleType vScaleType = getType();
            if (vScaleType == VScaleType_Divide){
                *value = L"Divide";
            }
            else if (vScaleType == VScaleType_EqualDiff){
                *value = L"EqualDiff";
            }
            else if (vScaleType == VScaleType_EqualRatio){
                *value = L"EqualRatio";
            }
            else if (vScaleType == VScaleType_GoldenRatio){
                *value = L"GoldenRatio";
            }
            else{
                *value = L"percent";
            }
        }
    }
    
    ArrayList<String> VScale::getPropertyNames(){
        ArrayList<String> propertyNames;
        propertyNames.add(L"AllowUserPaint");
        propertyNames.add(L"AutoMaxMin");
        propertyNames.add(L"BaseField");
        propertyNames.add(L"Digit");
        propertyNames.add(L"Font");
        propertyNames.add(L"Magnitude");
        propertyNames.add(L"MidValue");
        propertyNames.add(L"NumberStyle");
        propertyNames.add(L"PaddingBottom");
        propertyNames.add(L"PaddingTop");
        propertyNames.add(L"Reverse");
        propertyNames.add(L"ScaleColor");
        propertyNames.add(L"System");
        propertyNames.add(L"TextColor");
        propertyNames.add(L"TextColor2");
        propertyNames.add(L"Type");
        return propertyNames;
    }
    
    void VScale::onPaint(FCPaint *paint, ChartDiv *div, const FCRect& rect){
        
    }
    
    void VScale::setProperty(const String& name, const String& value){
        if (name == L"allowuserpaint"){
            setAllowUserPaint(FCStr::convertStrToBool(value));
        }
        else if(name == L"automaxmin"){
            setAutoMaxMin(FCStr::convertStrToBool(value));
        }
        else if (name == L"basefield"){
            setBaseField(FCStr::convertStrToInt(value));
        }
        else if (name == L"digit"){
            setDigit(FCStr::convertStrToInt(value));
        }
        else if (name == L"font"){
            setFont(FCStr::convertStrToFont(value));
        }
        else if (name == L"textcolor"){
            setTextColor(FCStr::convertStrToColor(value));
        }
        else if (name == L"textcolor2"){
            setTextColor2(FCStr::convertStrToColor(value));
        }
        else if (name == L"magnitude"){
            setMagnitude(FCStr::convertStrToInt(value));
        }
        else if (name == L"midvalue"){
            setMidValue(FCStr::convertStrToDouble(value));
        }
        else if (name == L"numberstyle"){
            String lowerStr = FCStr::toLower(value);
            if(value == L"standard"){
                setNumberStyle(NumberStyle_Standard);
            }
            else{
                setNumberStyle(NumberStyle_Underline);
            }
        }
        else if (name == L"paddingbottom"){
            setPaddingBottom(FCStr::convertStrToInt(value));
        }
        else if (name == L"paddingtop"){
            setPaddingTop(FCStr::convertStrToInt(value));
        }
        else if (name == L"reverse"){
            setReverse(FCStr::convertStrToBool(value));
        }
        else if (name == L"scalecolor"){
            setScaleColor(FCStr::convertStrToColor(value));
        }
        else if (name == L"system"){
            String lowerStr = FCStr::toLower(value);
            if (value == L"log"){
                setSystem(VScaleSystem_Logarithmic);
            }
            else{
                setSystem(VScaleSystem_Standard);
            }
        }
        else if (name == L"type"){
            String lowerStr = FCStr::toLower(value);
            if (value == L"divide"){
                setType(VScaleType_Divide);
            }
            else if (value == L"equaldiff"){
                setType(VScaleType_EqualDiff);
            }
            else if (value == L"equalratio"){
                setType(VScaleType_EqualRatio);
            }
            else if (value == L"goldenratio"){
                setType(VScaleType_GoldenRatio);
            }
            else{
                setType(VScaleType_Percent);
            }
        }
    }
    
    ArrayList<double> VScale::getScaleSteps(){
        return m_scaleSteps;
    }
    
    void VScale::setScaleSteps(ArrayList<double> scaleSteps){
        m_scaleSteps = scaleSteps;
    }
}
