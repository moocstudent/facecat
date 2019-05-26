#include "FCWindowFrame.h"

namespace FaceCat{
    FCWindowFrame::FCWindowFrame(){
        setBackColor(FCColor_None);
        setBorderColor(FCColor_None);
        setDock(FCDockStyle_Fill);
    }
    
    FCWindowFrame::~FCWindowFrame(){
    }
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    bool FCWindowFrame::containsPoint(const FCPoint& point){
        ArrayList<FCView*> controls = getControls();
        for(int c = 0; c < controls.size(); c++){
            FCWindow *window = dynamic_cast<FCWindow*>(controls.get(c));
            if (window){
                if (window->isDialog() && window->getFrame() == this){
                    return true;
                }
                else{
                    return window->containsPoint(point);
                }
            }
        }
        return false;
    }
    
    String FCWindowFrame::getControlType(){
        return L"WindowFrame";
    }
    
    void FCWindowFrame::invalidate(){
        if(m_native){
            ArrayList<FCView*> controls = getControls();
            for(int c = 0; c < controls.size(); c++){
                FCWindow *window = dynamic_cast<FCWindow*>(controls.get(c));
                if (window){
                    m_native->invalidate(window->getDynamicPaintRect());
                    break;
                }
            }
        }
    }
    
    void FCWindowFrame::onPaintBackground(FCPaint *paint, const FCRect& clipRect){
        FCView::onPaintBackground(paint, clipRect);
        if(paint->supportTransparent()){
            ArrayList<FCView*> controls = getControls();
            for(int c = 0; c < controls.size(); c++){
                FCWindow *window = dynamic_cast<FCWindow*>(controls.get(c));
                if (window){
                    Long shadowColor = window->getShadowColor();
                    int shadowSize = window->getShadowSize();
                    if (shadowColor != FCColor_None && shadowSize > 0 && window->isDialog() && window->getFrame() == this){
                        FCRect bounds = window->getBounds();
                        FCRect leftShadow ={bounds.left - shadowSize, bounds.top - shadowSize, bounds.left, bounds.bottom + shadowSize};
                        paint->fillRect(shadowColor, leftShadow);
                        FCRect rightShadow ={bounds.right, bounds.top - shadowSize, bounds.right + shadowSize, bounds.bottom + shadowSize};
                        paint->fillRect(shadowColor, rightShadow);
                        FCRect topShadow ={bounds.left, bounds.top - shadowSize, bounds.right, bounds.top};
                        paint->fillRect(shadowColor, topShadow);
                        FCRect bottomShadow ={bounds.left, bounds.bottom, bounds.right, bounds.bottom + shadowSize};
                        paint->fillRect(shadowColor, bottomShadow);
                    }
                }
            }
        }
    }
}
