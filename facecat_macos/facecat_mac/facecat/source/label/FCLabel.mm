#include "stdafx.h"
#include "FCLabel.h"

namespace FaceCat{
    FCLabel::FCLabel(){
        m_textAlign = FCContentAlignment_TopLeft;
        setAutoSize(true);
        setBackColor(FCColor_None);
        setBorderColor(FCColor_None);
        FCSize size ={100, 20};
        setSize(size);
    }
    
    FCLabel::~FCLabel(){
    }
    
    FCContentAlignment FCLabel::getTextAlign(){
        return m_textAlign;
    }
    
    void FCLabel::setTextAlign(FCContentAlignment textAlign){
        m_textAlign = textAlign;
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    String FCLabel::getControlType(){
        return L"Label";
    }
    
    void FCLabel::getProperty(const String& name, String *value, String *type){
        if (name == L"textalign"){
            *type = L"enum:FCContentAlignment";
            *value = FCStr::convertContentAlignmentToStr(getTextAlign());
        }
        else{
            FCView::getProperty(name, value, type);
        }
    }
    
    ArrayList<String> FCLabel::getPropertyNames(){
        ArrayList<String> propertyNames = FCView::getPropertyNames();
        propertyNames.add(L"TextAlign");
        return propertyNames;
    }
    
    void FCLabel::onPaintForeground(FCPaint *paint, const FCRect& clipRect){
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
    
    void FCLabel::onPrePaint(FCPaint *paint, const FCRect& clipRect){
        FCView::onPrePaint(paint, clipRect);
        if (autoSize()){
            String text = getText();
            int width = getWidth(), height = getHeight();
            if(width > 0 && height > 0){
                FCFont *font = getFont();
                FCSize tSize = paint->textSize(getText().c_str(), font);
                int newW = tSize.cx + 4;
                int newH = tSize.cy + 4;
                if (newW != width || newH != height){
                    FCSize newSize ={newW, newH};
                    setSize(newSize);
                    width = newW;
                    height = newH;
                }
            }
        }
    }
    
    void FCLabel::setProperty(const String& name, const String& value){
        if (name == L"textalign"){
            setTextAlign(FCStr::convertStrToContentAlignment(value));
        }
        else{
            FCView::setProperty(name, value);
        }
    }
}
