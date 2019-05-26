#include "stdafx.h"
#include "FCToolTip.h"

namespace FaceCat{
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    FCToolTip::FCToolTip(){
        m_timerID = FCView::getNewTimerID();
        m_autoPopDelay = false;
        m_initialDelay = false;
        m_lastTouchPoint.x = 0;
        m_lastTouchPoint.y = 0;
        m_remainAutoPopDelay = 0;
        m_remainInitialDelay = 0;
        m_showAlways = false;
        m_useAnimation = false;
        setAutoSize(true);
        setBackColor(FCColor::argb(255, 255, 40));
        setBorderColor(FCColor_Border);
        setTopMost(true);
        setVisible(false);
    }
    
    FCToolTip::~FCToolTip(){
        stopTimer(m_timerID);
    }
    
    int FCToolTip::getAutoPopDelay(){
        return m_autoPopDelay;
    }
    
    void FCToolTip::setAutoPopDelay(int autoPopDelay){
        m_autoPopDelay = autoPopDelay;
    }
    
    int FCToolTip::getInitialDelay(){
        return m_initialDelay;
    }
    
    void FCToolTip::setInitialDelay(int initialDelay){
        m_initialDelay = initialDelay;
    }
    
    bool FCToolTip::showAlways(){
        return m_showAlways;
    }
    
    void FCToolTip::setShowAlways(bool showAlways){
        m_showAlways = showAlways;
    }
    
    bool FCToolTip::useAnimation(){
        return m_useAnimation;
    }
    
    void FCToolTip::setUseAnimation(bool useAnimation){
        m_useAnimation = useAnimation;
    }
    
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    String FCToolTip::getControlType(){
        return L"ToolTip";
    }
    
    void FCToolTip::getProperty(const String& name, String *value, String *type){
        if (name == L"autopopupdelay"){
            *type = L"int";
            *value = FCStr::convertIntToStr(getAutoPopDelay());
        }
        else if (name == L"initialdelay"){
            *type = L"int";
            *value = FCStr::convertIntToStr(getInitialDelay());
        }
        else if (name == L"showalways"){
            *type = L"bool";
            *value = FCStr::convertBoolToStr(showAlways());
        }
        else if (name == L"useanimation"){
            *type = L"bool";
            *value = FCStr::convertBoolToStr(useAnimation());
        }
        else{
            FCLabel::getProperty(name, value, type);
        }
    }
    
    ArrayList<String> FCToolTip::getPropertyNames(){
        ArrayList<String> propertyNames = FCLabel::getPropertyNames();
        propertyNames.add(L"AutoPopupDelay");
        propertyNames.add(L"InitialDelay");
        propertyNames.add(L"ShowAlways");
        propertyNames.add(L"UseAnimation");
        return propertyNames;
    }
    
    void FCToolTip::hide(){
        setVisible(false);
    }
    
    void FCToolTip::onLoad(){
        FCLabel::onLoad();
        m_lastTouchPoint = getTouchPoint();
        startTimer(m_timerID, 10);
    }
    
    void FCToolTip::onTimer(int timerID){
        FCLabel::onTimer(timerID);
        if (m_timerID == timerID){
            FCPoint mp = getTouchPoint();
            if(!m_showAlways){
                if (m_lastTouchPoint.x != mp.x || m_lastTouchPoint.y != mp.y){
                    setVisible(false);
                }
            }
            m_lastTouchPoint = mp;
            if (m_remainAutoPopDelay > 0){
                m_remainAutoPopDelay -= 10;
                if (m_remainAutoPopDelay <= 0){
                    setVisible(false);
                }
            }
            if (m_remainInitialDelay > 0){
                m_remainInitialDelay -= 10;
                if (m_remainInitialDelay <= 0){
                    setVisible(true);
                }
            }
        }
    }
    
    void FCToolTip::onVisibleChanged(){
        FCLabel::onVisibleChanged();
        if(m_native){
            if (isVisible()){
                m_native->addControl(this);
                m_remainAutoPopDelay = m_autoPopDelay;
                m_remainInitialDelay = 0;
            }
            else{
                m_native->removeControl(this);
                startTimer(m_timerID, 10);
                m_remainAutoPopDelay = 0;
                m_remainInitialDelay = 0;
            }
            m_native->invalidate();
        }
    }
    
    void FCToolTip::setProperty(const String& name, const String& value){
        if (name == L"autopopupdelay"){
            setAutoPopDelay(FCStr::convertStrToInt(value));
        }
        else if (name == L"initialdelay"){
            setInitialDelay(FCStr::convertStrToInt(value));
        }
        else if (name == L"showalways"){
            setShowAlways(FCStr::convertStrToBool(value));
        }
        else if (name == L"useanimation"){
            setUseAnimation(FCStr::convertStrToBool(value));
        }
        else{
            FCLabel::setProperty(name, value);
        }
    }
    
    void FCToolTip::show(){
        m_remainAutoPopDelay = 0;
        m_remainInitialDelay = m_initialDelay;
        setVisible(m_initialDelay == 0);
        m_native->invalidate();
    }
}
