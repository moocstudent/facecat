#include "stdafx.h"
#include "ChartTitleBar.h"

namespace FaceCat{
    ChartTitle::ChartTitle(int fieldName, const String& fieldText, Long color, int digit, bool visible){
        m_digit = digit;
        m_fieldName = fieldName;
        m_fieldText = fieldText;
        m_fieldTextMode = TextMode_Full;
        m_fieldTextSeparator = L" ";
        m_textColor = color;
        m_visible = visible;
    }
    
    int ChartTitle::getDigit(){
        return m_digit;
    }
    
    void ChartTitle::setDigit(int digit){
        m_digit = digit;
    }
    
    int ChartTitle::getFieldName(){
        return m_fieldName;
    }
    
    void ChartTitle::setFieldName(int fieldName){
        m_fieldName = fieldName;
    }
    
    String ChartTitle::getFieldText(){
        return m_fieldText;
    }
    
    void ChartTitle::setFieldText(const String& fieldText){
        m_fieldText = fieldText;
    }
    
    TextMode ChartTitle::getFieldTextMode(){
        return m_fieldTextMode;
    }
    
    void ChartTitle::setFieldTextMode(TextMode fieldTextMode){
        m_fieldTextMode = fieldTextMode;
    }
    
    String ChartTitle::getFieldTextSeparator(){
        return m_fieldTextSeparator;
    }
    
    void ChartTitle::setFieldTextSeparator(const String& fieldTextSeparator){
        m_fieldTextSeparator = fieldTextSeparator;
    }
    
    Long ChartTitle::getTextColor(){
        return m_textColor;
    }
    
    void ChartTitle::setTextColor(Long textColor){
        m_textColor = textColor;
    }
    
    bool ChartTitle::isVisible(){
        return m_visible;
    }
    
    void ChartTitle::setVisible(bool visible){
        m_visible = visible;
    }
    
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    void ChartTitle::getProperty(const String& name, String *value, String *type){
        if (name == L"digit"){
            *type = L"int";
            *value = FCStr::convertIntToStr(getDigit());
        }
        else if (name == L"fieldname"){
            *type = L"int";
            *value = FCStr::convertIntToStr(getFieldName());
        }
        else if (name == L"fieldtext"){
            *type = L"text";
            *value = getFieldText();
        }
        else if (name == L"fieldtextmode"){
            *type = L"enum:TextMode";
            TextMode fieldTextMode = getFieldTextMode();
            if (fieldTextMode == TextMode_Field){
                *value = L"field";
            }
            else if (fieldTextMode == TextMode_Full){
                *value = L"full";
            }
            else if (fieldTextMode == TextMode_None){
                *value = L"none";
            }
            else{
                *value = L"value";
            }
        }
        else if (name == L"fieldtextseparator"){
            *type = L"text";
            *value = getFieldTextSeparator();
        }
        else if (name == L"textcolor"){
            *type = L"color";
            *value = FCStr::convertColorToStr(getTextColor());
        }
        else if(name == L"visible"){
            *type = L"bool";
            *value = FCStr::convertBoolToStr(isVisible());
        }
    }
    
    ArrayList<String> ChartTitle::getPropertyNames(){
        ArrayList<String> propertyNames;
        propertyNames.add(L"Digit");
        propertyNames.add(L"FieldName");
        propertyNames.add(L"FieldText");
        propertyNames.add(L"FieldTextMode");
        propertyNames.add(L"FieldTextSeparator");
        propertyNames.add(L"TextColor");
        propertyNames.add(L"Visible");
        return propertyNames;
    }
    
    void ChartTitle::setProperty(const String& name, const String& value){
        if (name == L"digit"){
            setDigit(FCStr::convertStrToInt(value));
        }
        else if (name == L"fieldname"){
            setFieldName(FCStr::convertStrToInt(value));
        }
        else if (name == L"fieldtext"){
            setFieldText(value);
        }
        else if (name == L"fieldtextmode"){
            if (value == L"field"){
                setFieldTextMode(TextMode_Field);
            }
            else if (value == L"full"){
                setFieldTextMode(TextMode_Full);
            }
            else if (value == L"none"){
                setFieldTextMode(TextMode_None);
            }
            else{
                setFieldTextMode(TextMode_None);
            }
        }
        else if (name == L"fieldtextseparator"){
            setFieldTextSeparator(value);
        }
        else if (name == L"textcolor"){
            setTextColor(FCStr::convertStrToColor(value));
        }
        else if(name == L"visible"){
            setVisible(FCStr::convertStrToBool(value));
        }
    }
    
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    ChartTitleBar::ChartTitleBar(){
        m_allowUserPaint = false;
        m_font = new FCFont(L"Arial", 12, false, false, false);
        m_textColor = FCColor::argb(255, 255, 255);
        m_height = 20;
        m_maxLine = 5;
        m_showUnderLine = true;
        m_underLineColor = FCColor::argb(80, 0, 0);
        m_visible = true;
    }
    
    ChartTitleBar::~ChartTitleBar(){
        if(m_font){
            delete m_font;
            m_font = 0;
        }
        if(Titles.size() > 0){
            for(int i = 0; i < Titles.size(); i++){
                delete Titles.get(i);
            }
            Titles.clear();
        }
    }
    
    bool ChartTitleBar::allowUserPaint(){
        return m_allowUserPaint;
    }
    
    void ChartTitleBar::setAllowUserPaint(bool allowUserPaint){
        m_allowUserPaint = allowUserPaint;
    }
    
    FCFont* ChartTitleBar::getFont(){
        return m_font;
    }
    
    void ChartTitleBar::setFont(FCFont *font){
        m_font->copy(font);
    }
    
    int ChartTitleBar::getHeight(){
        return m_height;
    }
    
    void ChartTitleBar::setHeight(int height){
        m_height = height;
    }
    
    int ChartTitleBar::getMaxLine(){
        return m_maxLine;
    }
    
    void ChartTitleBar::setMaxLine(int maxLine){
        m_maxLine = maxLine;
    }
    
    bool ChartTitleBar::showUnderLine(){
        return m_showUnderLine;
    }
    
    void ChartTitleBar::setShowUnderLine(bool showUnderLine){
        m_showUnderLine = showUnderLine;
    }
    
    String ChartTitleBar::getText(){
        return m_text;
    }
    
    void ChartTitleBar::setText(const String& text){
        m_text = text;
    }
    
    Long ChartTitleBar::getTextColor(){
        return m_textColor;
    }
    
    void ChartTitleBar::setTextColor(Long textColor){
        m_textColor = textColor;
    }
    
    Long ChartTitleBar::getUnderLineColor(){
        return m_underLineColor;
    }
    
    void ChartTitleBar::setUnderLineColor(Long underLineColor){
        m_underLineColor = underLineColor;
    }
    
    bool ChartTitleBar::isVisible(){
        return m_visible;
    }
    
    void ChartTitleBar::setVisible(bool visible){
        m_visible = visible;
    }
    
    //////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    void ChartTitleBar::getProperty(const String& name, String *value, String *type){
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
        else if (name == L"maxline"){
            *type = L"int";
            *value = FCStr::convertIntToStr(getMaxLine());
        }
        else if (name == L"showunderline"){
            *type = L"bool";
            *value = FCStr::convertBoolToStr(showUnderLine());
        }
        else if (name == L"text"){
            *type = L"text";
            *value = getText();
        }
        else if (name == L"underlinecolor"){
            *type = L"color";
            *value = FCStr::convertColorToStr(getUnderLineColor());
        }
        else if (name == L"visible"){
            *type = L"bool";
            *value = FCStr::convertBoolToStr(isVisible());
        }
    }
    
    ArrayList<String> ChartTitleBar::getPropertyNames(){
        ArrayList<String> propertyNames;
        propertyNames.add(L"AllowUserPaint");
        propertyNames.add(L"Font");
        propertyNames.add(L"Height");
        propertyNames.add(L"MaxLine");
        propertyNames.add(L"ShowUnderLine");
        propertyNames.add(L"Text");
        propertyNames.add(L"TextColor");
        propertyNames.add(L"UnderLineColor");
        propertyNames.add(L"Visible");
        return propertyNames;
    }
    
    void ChartTitleBar::onPaint(FCPaint *paint, ChartDiv *div, const FCRect& rect){
        
    }
    
    void ChartTitleBar::setProperty(const String& name, const String& value){
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
        else if (name == L"maxline"){
            setMaxLine(FCStr::convertStrToInt(value));
        }
        else if (name == L"showunderline"){
            setShowUnderLine(FCStr::convertStrToBool(value));
        }
        else if (name == L"text"){
            setText(value);
        }
        else if (name == L"underlinecolor"){
            setUnderLineColor(FCStr::convertStrToColor(value));
        }
        else if (name == L"visible"){
            setVisible(FCStr::convertStrToBool(value));
        }
    }
}
