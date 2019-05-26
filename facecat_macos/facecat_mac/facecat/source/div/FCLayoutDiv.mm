#include "stdafx.h"
#include "FCLayoutDiv.h"

namespace FaceCat{
    FCLayoutDiv::FCLayoutDiv(){
        m_autoWrap = false;
        m_layoutStyle = FCLayoutStyle_LeftToRight;
    }
    
    FCLayoutDiv::~FCLayoutDiv(){
    }
    
    bool FCLayoutDiv::autoWrap(){
        return m_autoWrap;
    }
    
    void FCLayoutDiv::setAutoWrap(bool autoWrap){
        m_autoWrap = autoWrap;
    }
    
    FCLayoutStyle FCLayoutDiv::getLayoutStyle(){
        return m_layoutStyle;
    }
    
    void FCLayoutDiv::setLayoutStyle(FCLayoutStyle layoutStyle){
        m_layoutStyle = layoutStyle;
    }
    
    ///////////////////////////////////////////////////////////////////////////////////////////////////////
    
    String FCLayoutDiv::getControlType(){
        return L"LayoutDiv";
    }
    
    void FCLayoutDiv::getProperty(const String& name, String *value, String *type){
        if (name == L"autowrap"){
            *type = L"bool";
            *value = FCStr::convertBoolToStr(autoWrap());
        }
        else if (name == L"layoutstyle"){
            *type = L"enum:FCLayoutStyle";
            *value = FCStr::convertLayoutStyleToStr(getLayoutStyle());
        }
        else{
            FCDiv::getProperty(name, value, type);
        }
    }
    
    ArrayList<String> FCLayoutDiv::getPropertyNames(){
        ArrayList<String> propertyNames = FCDiv::getPropertyNames();
        propertyNames.add(L"AutoWrap");
        propertyNames.add(L"LayoutStyle");
        return propertyNames;
    }
    
    bool FCLayoutDiv::onResetLayout(){
        bool reset = false;
        if (getNative()){
            FCPadding padding = getPadding();
            int left = padding.left, top = padding.top;
            int width = getWidth() - padding.left - padding.right;
            int height = getHeight() - padding.top - padding.bottom;
            int controlSize = (int)m_controls.size();
            for (int i = 0; i < controlSize; i++){
                FCView *control = m_controls.get(i);
                if(control->isVisible() && control != getHScrollBar() && control != getVScrollBar()){
                    FCSize size = control->getSize();
                    int cLeft = control->getLeft(), cTop = control->getTop(), cWidth = size.cx, cHeight = size.cy;
                    int nLeft = cLeft, nTop = cTop, nWidth = cWidth, nHeight = cHeight;
                    FCPadding margin = control->getMargin();
                    switch (m_layoutStyle){
                            //自下而上
                        case FCLayoutStyle_BottomToTop:{
                            if (i == 0){
                                top = padding.top + height;
                            }
                            int lWidth = 0;
                            if (m_autoWrap){
                                lWidth = size.cx;
                                int lTop = top - margin.top - cHeight - margin.bottom;
                                if (lTop < padding.top){
                                    left += cWidth + margin.left;
                                    top = height - padding.top;
                                }
                            }
                            else{
                                lWidth = width - margin.left - margin.right;
                            }
                            top -= cHeight + margin.bottom;
                            nLeft = left + margin.left;
                            nWidth = lWidth;
                            nTop = top;
                            break;
                        }
                            //从左向右
                        case FCLayoutStyle_LeftToRight:{
                            int lHeight = 0;
                            if (m_autoWrap){
                                lHeight = size.cy;
                                int lRight = left + margin.left + cWidth + margin.right;
                                if (lRight > width){
                                    left = padding.left;
                                    top += cHeight + margin.top;
                                }
                            }
                            else{
                                lHeight = height - margin.top - margin.bottom;
                            }
                            left += margin.left;
                            nLeft = left;
                            nTop = top + margin.top;
                            nHeight = lHeight;
                            left += cWidth + margin.right;
                            break;
                        }
                            //从右向左
                        case FCLayoutStyle_RightToLeft:{
                            if (i == 0){
                                left = width - padding.left;
                            }
                            int lHeight = 0;
                            if (m_autoWrap){
                                lHeight = size.cy;
                                int lLeft = left - margin.left - cWidth - margin.right;
                                if (lLeft < padding.left){
                                    left = width - padding.left;
                                    top += cHeight + margin.top;
                                }
                            }
                            else{
                                lHeight = height - margin.top - margin.bottom;
                            }
                            left -= cWidth + margin.left;
                            nLeft = left;
                            nTop = top + margin.top;
                            nHeight = lHeight;
                            break;
                        }
                            //自上而下
                        case FCLayoutStyle_TopToBottom:{
                            int lWidth = 0;
                            if (m_autoWrap){
                                lWidth = size.cx;
                                int lBottom = top + margin.top + cHeight + margin.bottom;
                                if (lBottom > height){
                                    left += cWidth + margin.left + margin.right;
                                    top = padding.top;
                                }
                            }
                            else{
                                lWidth = width - margin.left - margin.right;
                            }
                            top += margin.top;
                            nTop = top;
                            nLeft = left + margin.left;
                            nWidth = lWidth;
                            top += cHeight + margin.bottom;
                            break;
                        }
                    }
                    if (cLeft != nLeft || cTop != nTop || cWidth != nWidth || cHeight != nHeight){
                        FCRect rect ={nLeft, nTop, nLeft + nWidth, nTop + nHeight};
                        control->setBounds(rect);
                        reset = true;
                    }
                }
            }
        }
        return reset;
    }
    
    void FCLayoutDiv::setProperty(const String& name, const String& value){
        if (name == L"autowrap"){
            setAutoWrap(FCStr::convertStrToBool(value));
        }
        else if (name == L"layoutstyle"){
            setLayoutStyle(FCStr::convertStrToLayoutStyle(value));
        }
        else{
            FCDiv::setProperty(name, value);
        }
    }
    
    void FCLayoutDiv::update(){
        onResetLayout();
        int controlsSize = (int)m_controls.size();
        for (int i = 0; i < controlsSize; i++){
            m_controls.get(i)->update();
        }
        updateScrollBar();
    }
}
