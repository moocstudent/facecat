#include "stdafx.h"
#include "CrossLineTip.h"

namespace FaceCat{
    CrossLineTip::CrossLineTip(){
        m_allowUserPaint = false;
        m_backColor = FCColor::argb(255, 0, 0);
        m_font = new FCFont(L"Arial", 12, false, false, false);
        m_textColor = FCColor::argb(255, 255, 255);
        m_visible = true;
    }
    
    CrossLineTip::~CrossLineTip(){
        if(m_font){
            delete m_font;
            m_font = 0;
        }
    }
    
    bool CrossLineTip::allowUserPaint(){
        return m_allowUserPaint;
    }
    
    void CrossLineTip::setAllowUserPaint(bool allowUserPaint){
        m_allowUserPaint = allowUserPaint;
    }
    
    Long CrossLineTip::getBackColor(){
        return m_backColor;
    }
    
    void CrossLineTip::setBackColor(Long backColor){
        m_backColor = backColor;
    }
    
    FCFont* CrossLineTip::getFont(){
        return m_font;
    }
    
    void CrossLineTip::setFont(FCFont *font){
        m_font->copy(font);
    }
    
    Long CrossLineTip::getTextColor(){
        return m_textColor;
    }
    
    void CrossLineTip::setTextColor(Long textColor){
        m_textColor = textColor;
    }
    
    bool CrossLineTip::isVisible(){
        return m_visible;
    }
    
    void CrossLineTip::setVisible(bool visible){
        m_visible = visible;
    }
    
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    void CrossLineTip::getProperty(const String& name, String *value, String *type){
        if (name == L"allowuserpaint"){
            *type = L"bool";
            *value = FCStr::convertBoolToStr(allowUserPaint());
        }
        else if (name == L"backcolor"){
            *type = L"color";
            *value = FCStr::convertColorToStr(getBackColor());
        }
        else if (name == L"font"){
            *type = L"font";
            *value = FCStr::convertFontToStr(getFont());
        }
        else if (name == L"textcolor"){
            *type = L"color";
            *value = FCStr::convertColorToStr(getTextColor());
        }
        else if (name == L"visible"){
            *type = L"bool";
            *value = FCStr::convertBoolToStr(isVisible());
        }
    }
    
    ArrayList<String> CrossLineTip::getPropertyNames(){
        ArrayList<String> propertyNames;
        propertyNames.add(L"AllowUserPaint");
        propertyNames.add(L"BackColor");
        propertyNames.add(L"Font");
        propertyNames.add(L"TextColor");
        propertyNames.add(L"Visible");
        return propertyNames;
    }
    
    void CrossLineTip::onPaint(FCPaint *paint, ChartDiv *div, const FCRect& rect){
    }
    
    void CrossLineTip::setProperty(const String& name, const String& value){
        if (name == L"allowuserpaint"){
            setAllowUserPaint(FCStr::convertStrToBool(value));
        }
        else if (name == L"backcolor"){
            setBackColor(FCStr::convertStrToColor(value));
        }
        else if (name == L"font"){
            setFont(FCStr::convertStrToFont(value));
        }
        else if (name == L"textcolor"){
            setTextColor(FCStr::convertStrToColor(value));
        }
        else if (name == L"visible"){
            setVisible(FCStr::convertStrToBool(value));
        }
    }
}
