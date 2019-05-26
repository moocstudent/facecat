#include "stdafx.h"
#include "FCButton.h"

namespace FaceCat{
    Long FCButton::getPaintingBackColor(){
        Long backColor = FCView::getPaintingBackColor();
        if (backColor != FCColor_None && isPaintEnabled(this)){
            FCNative *native = getNative();
            if (this == native->getPushedControl()){
                backColor = FCColor_Pushed;
            }
            else if (this == native->getHoveredControl()){
                backColor = FCColor_Hovered;
            }
        }
        return backColor;
    }
    
    String FCButton::getPaintingBackImage(){
        String backImage;
        if(isPaintEnabled(this)){
            FCNative *native = getNative();
            if (this == native->getPushedControl()){
                backImage = m_pushedBackImage;
            }
            else if (this == native->getHoveredControl()){
                backImage = m_hoveredBackImage;
            }
        }
        else{
            backImage = m_disabledBackImage;
        }
        if(backImage.length() > 0){
            return backImage;
        }
        else{
            return FCView::getPaintingBackImage();
        }
    }
    
    /////////////////////////////////////////////////////////////////////////////////////////
    
    FCButton::FCButton(){
        m_textAlign = FCContentAlignment_MiddleCenter;
        FCSize size ={60, 20};
        setSize(size);
    }
    
    FCButton::~FCButton(){
    }
    
    String FCButton::getDisabledBackImage(){
        return m_disabledBackImage;
    }
    
    void FCButton::setDisabledBackImage(const String& disabledBackImage){
        m_disabledBackImage = disabledBackImage;
    }
    
    String FCButton::getHoveredBackImage(){
        return m_hoveredBackImage;
    }
    
    void FCButton::setHoveredBackImage(const String& hoveredBackImage){
        m_hoveredBackImage = hoveredBackImage;
    }
    
    String FCButton::getPushedBackImage(){
        return m_pushedBackImage;
    }
    
    void FCButton::setPushedBackImage(const String& pushedBackImage){
        m_pushedBackImage = pushedBackImage;
    }
    
    FCContentAlignment FCButton::getTextAlign(){
        return m_textAlign;
    }
    
    void FCButton::setTextAlign(FCContentAlignment textAlign){
        m_textAlign = textAlign;
    }
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    String FCButton::getControlType(){
        return L"Button";
    }
    
    void FCButton::getProperty(const String& name, String *value, String *type){
        if(name == L"disabledbackimage"){
            *type = L"text";
            *value = getDisabledBackImage();
        }
        else if(name == L"hoveredbackimage"){
            *type = L"text";
            *value = getHoveredBackImage();
        }
        else if(name == L"pushedbackimage"){
            *type = L"text";
            *value = getPushedBackImage();
        }
        else if (name == L"textalign"){
            *type = L"enum:FCContentAlignment";
            *value = FCStr::convertContentAlignmentToStr(getTextAlign());
        }
        else{
            FCView::getProperty(name, value, type);
        }
    }
    
    ArrayList<String> FCButton::getPropertyNames(){
        ArrayList<String> propertyNames = FCView::getPropertyNames();
        propertyNames.add(L"DisabledBackImage");
        propertyNames.add(L"HoveredBackImage");
        propertyNames.add(L"PushedBackImage");
        propertyNames.add(L"TextAlign");
        return propertyNames;
    }
    
    void FCButton::onTouchDown(FCTouchInfo touchInfo){
        FCView::onTouchDown(touchInfo);
        invalidate();
    }
    
    void FCButton::onTouchEnter(FCTouchInfo touchInfo){
        FCView::onTouchEnter(touchInfo);
        invalidate();
    }
    
    void FCButton::onTouchLeave(FCTouchInfo touchInfo){
        FCView::onTouchLeave(touchInfo);
        invalidate();
    }
    
    void FCButton::onTouchUp(FCTouchInfo touchInfo){
        FCView::onTouchUp(touchInfo);
        invalidate();
    }
    
    void FCButton::onPaintForeground(FCPaint *paint, const FCRect& clipRect){
        String text = getText();
        if (text.length() > 0){
            int width = getWidth(), height = getHeight();
            if(width > 0 && height > 0){
                FCFont *font = getFont();
                FCSize tSize = paint->textSize(text.c_str(), font);
                FCPoint tPoint ={(width - tSize.cx) / 2, (height - tSize.cy) / 2};
                FCPadding padding = getPadding();
                switch (m_textAlign){
                    case FCContentAlignment_BottomCenter:
                        tPoint.y = height - tSize.cy;
                        break;
                    case FCContentAlignment_BottomLeft:
                        tPoint.x = padding.left;
                        tPoint.y = height - tSize.cy - padding.bottom;
                        break;
                    case FCContentAlignment_BottomRight:
                        tPoint.x = width - tSize.cx - padding.right;
                        tPoint.y = height - tSize.cy - padding.bottom;
                        break;
                    case FCContentAlignment_MiddleLeft:
                        tPoint.x = padding.left;
                        break;
                    case FCContentAlignment_MiddleRight:
                        tPoint.x = width - tSize.cx - padding.right;
                        break;
                    case FCContentAlignment_TopCenter:
                        tPoint.y = padding.top;
                        break;
                    case FCContentAlignment_TopLeft:
                        tPoint.x = padding.left;
                        tPoint.y = padding.top;
                        break;
                    case FCContentAlignment_TopRight:
                        tPoint.x = width - tSize.cx - padding.right;
                        tPoint.y = padding.top;
                        break;
                }
                FCRect tRect ={tPoint.x, tPoint.y, tPoint.x + tSize.cx, tPoint.y + tSize.cy};
                Long textColor = getPaintingTextColor();
                if(autoEllipsis() && (tRect.right < clipRect.right || tRect.bottom < clipRect.bottom)){
                    if(tRect.right < clipRect.right){
                        tRect.right = clipRect.right;
                    }
                    if(tRect.bottom < clipRect.bottom){
                        tRect.bottom = clipRect.bottom;
                    }
                    paint->drawTextAutoEllipsis(text.c_str(), textColor, font, tRect);
                }
                else{
                    paint->drawText(text.c_str(), textColor, font, tRect);
                }
            }
        }
    }
    
    void FCButton::setProperty(const String& name, const String& value){
        if(name == L"disabledbackimage"){
            setDisabledBackImage(value);
        }
        else if(name == L"hoveredbackimage"){
            setHoveredBackImage(value);
        }
        else if(name == L"pushedbackimage"){
            setPushedBackImage(value);
        }
        else if (name == L"textalign"){
            setTextAlign(FCStr::convertStrToContentAlignment(value));
        }
        else{
            FCView::setProperty(name, value);
        }
    }
}
