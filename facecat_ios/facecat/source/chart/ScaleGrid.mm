#include "stdafx.h"
#include "ScaleGrid.h"

namespace FaceCat{
    ScaleGrid::ScaleGrid(){
        m_allowUserPaint = false;
        m_distance = 30;
        m_gridColor = FCColor::argb(80, 0, 0);
        m_lineStyle = 2;
        m_visible = false;
    }
    
    ScaleGrid::~ScaleGrid(){
    }
    
    bool ScaleGrid::allowUserPaint(){
        return m_allowUserPaint;
    }
    
    void ScaleGrid::setAllowUserPaint(bool allowUserPaint){
        m_allowUserPaint = allowUserPaint;
    }
    
    int ScaleGrid::getDistance(){
        return m_distance;
    }
    
    void ScaleGrid::setDistance(int distance){
        m_distance = distance;
    }
    
    Long ScaleGrid::getGridColor(){
        return m_gridColor;
    }
    
    void ScaleGrid::setGridColor(Long gridColor){
        m_gridColor = gridColor;
    }
    
    int ScaleGrid::getLineStyle(){
        return m_lineStyle;
    }
    
    void ScaleGrid::setLineStyle(int lineStyle){
        m_lineStyle = lineStyle;
    }
    
    bool ScaleGrid::isVisible(){
        return m_visible;
    }
    
    void ScaleGrid::setVisible(bool visible){
        m_visible = visible;
    }
    
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    void ScaleGrid::getProperty(const String& name, String *value, String *type){
        if (name == L"allowuserpaint"){
            *type = L"bool";
            *value = FCStr::convertBoolToStr(allowUserPaint());
        }
        else if(name == L"distance"){
            *type = L"int";
            *value = FCStr::convertIntToStr(getDistance());
        }
        else if (name == L"gridcolor"){
            *type = L"color";
            *value = FCStr::convertColorToStr(getGridColor());
        }
        else if (name == L"linestyle"){
            *type = L"int";
            *value = FCStr::convertIntToStr(getLineStyle());
        }
        else if (name == L"visible"){
            *type = L"bool";
            *value = FCStr::convertBoolToStr(isVisible());
        }
    }
    
    ArrayList<String> ScaleGrid::getPropertyNames(){
        ArrayList<String> propertyNames;
        propertyNames.add(L"AllowUserPaint");
        propertyNames.add(L"Distance");
        propertyNames.add(L"GridColor");
        propertyNames.add(L"LineStyle");
        propertyNames.add(L"Visible");
        return propertyNames;
    }
    
    void ScaleGrid::onPaint(FCPaint *paint, ChartDiv *div, const FCRect& rect){
        
    }
    
    void ScaleGrid::setProperty(const String& name, const String& value){
        if (name == L"allowuserpaint"){
            setAllowUserPaint(FCStr::convertStrToBool(value));
        }
        else if(name == L"distance"){
            setDistance(FCStr::convertStrToInt(value));
        }
        else if (name == L"gridcolor"){
            setGridColor(FCStr::convertStrToColor(value));
        }
        else if (name == L"linestyle"){
            setLineStyle(FCStr::convertStrToInt(value));
        }
        else if (name == L"visible"){
            setVisible(FCStr::convertStrToBool(value));
        }
    }
}
