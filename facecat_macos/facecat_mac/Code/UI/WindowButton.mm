#include "WindowButton.h"

WindowButton::WindowButton(){
    m_isEllipse = true;
    setTextColor(FCCOLORS_WINDOWBACKCOLOR2);
    FCSize size = {200, 200};
    setSize(size);
    m_style = WindowButtonStyle_Close;
}

WindowButton::~WindowButton(){
}

bool WindowButton::isEllipse(){
    return m_isEllipse;
}

void WindowButton::setEllipse(bool isEllipse){
    m_isEllipse = isEllipse;
}

WindowButtonStyle WindowButton::getStyle(){
    return m_style;
}

void WindowButton::setStyle(WindowButtonStyle style){
    m_style = style;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////

Long WindowButton::getPaintingBackColor(){
    FCNative *native = getNative();
    if (m_style == WindowButtonStyle_Close){
        if (native->getPushedControl() == this){
            return FCColor::argb(255, 0, 0);
        }
        else if (native->getHoveredControl() == this){
            return FCColor::argb(255, 150, 150);
        }
        else{
            return FCColor::argb(255, 80, 80);
        }
    }
    else if (m_style == WindowButtonStyle_Min){
        if (native->getPushedControl() == this){
            return FCColor::argb(0, 255, 0);
        }
        else if (native->getHoveredControl() == this){
            return FCColor::argb(150, 255, 150);
        }
        else{
            return FCColor::argb(80, 255, 80);
        }
    }
    else{
        if (native->getPushedControl() == this){
            return FCColor::argb(255, 255, 0);
        }
        else if (native->getHoveredControl() == this){
            return FCColor::argb(255, 255, 150);
        }
        else{
            return FCColor::argb(255, 255, 80);
        }
    }
}

void WindowButton::onPaintBackground(FCPaint *paint, const FCRect& clipRect){
    int width = getWidth() , height = getHeight();
    float xRate = (float)width / 200;
    float yRate = (float)height / 200;
    FCRect drawRect = {1, 1, width - 1, height - 1};
    if(m_isEllipse){
        paint->fillEllipse(getPaintingBackColor(), drawRect);
    }
    else{
        paint->fillRect(getPaintingBackColor(), drawRect);
    }
    Long foreColor = getPaintingTextColor();
    float lineWidth = 10 * xRate;
    if (m_style == WindowButtonStyle_Close){
        paint->setLineCap(2, 2);
        paint->drawLine(foreColor, lineWidth, 0, (int)(135 * xRate), (int)(70 * yRate), (int)(70 * xRate), (int)(135 * yRate));
        paint->drawLine(foreColor, lineWidth, 0, (int)(70 * xRate), (int)(70 * yRate), (int)(135 * xRate), (int)(135 * yRate));
    }
    else if (m_style == WindowButtonStyle_Max){
        paint->setLineCap(2, 2);
        paint->drawLine(foreColor, lineWidth, 0, (int)(80 * xRate), (int)(80 * yRate), (int)(60 * xRate), (int)(60 * yRate));
        paint->drawLine(foreColor, lineWidth, 0, (int)(125 * xRate), (int)(145 * yRate), (int)(145 * xRate), (int)(145 * yRate));
        paint->drawLine(foreColor, lineWidth, 0, (int)(145 * xRate), (int)(125 * yRate), (int)(145 * xRate), (int)(145 * yRate));
        paint->drawLine(foreColor, lineWidth, 0, (int)(125 * xRate), (int)(125 * yRate), (int)(145 * xRate), (int)(145 * yRate));
        paint->drawLine(foreColor, lineWidth, 0, (int)(60 * xRate), (int)(80 * yRate), (int)(60 * xRate), (int)(60 * yRate));
        paint->drawLine(foreColor, lineWidth, 0, (int)(80 * xRate), (int)(60 * yRate), (int)(60 * xRate), (int)(60 * yRate));
        paint->drawLine(foreColor, lineWidth, 0, (int)(125 * xRate), (int)(80 * yRate), (int)(145 * xRate), (int)(60 * yRate));
        paint->drawLine(foreColor, lineWidth, 0, (int)(145 * xRate), (int)(80 * yRate), (int)(145 * xRate), (int)(60 * yRate));
        paint->drawLine(foreColor, lineWidth, 0, (int)(125 * xRate), (int)(60 * yRate), (int)(145 * xRate), (int)(60 * yRate));
        paint->drawLine(foreColor, lineWidth, 0, (int)(80 * xRate), (int)(125 * yRate), (int)(60 * xRate), (int)(145 * yRate));
        paint->drawLine(foreColor, lineWidth, 0, (int)(60 * xRate), (int)(125 * yRate), (int)(60 * xRate), (int)(145 * yRate));
        paint->drawLine(foreColor, lineWidth, 0, (int)(80 * xRate), (int)(145 * yRate), (int)(60 * xRate), (int)(145 * yRate));
    }
    else if (m_style == WindowButtonStyle_Min){
        paint->setLineCap(2, 2);
        paint->drawLine(foreColor, lineWidth, (int)(0 * xRate), (int)(60 * yRate), (int)(105 * xRate), (int)(135 * xRate), (int)(105 * yRate));
    }
    else if (m_style == WindowButtonStyle_Restore){
        paint->setLineCap(2, 2);
        paint->drawLine(foreColor, lineWidth, (int)(0 * xRate), (int)(90 * yRate), (int)(90 * xRate), (int)(70 * xRate), (int)(70 * yRate));
        paint->drawLine(foreColor, lineWidth, (int)(0 * xRate), (int)(90 * yRate), (int)(90 * xRate), (int)(70 * xRate), (int)(90 * yRate));
        paint->drawLine(foreColor, lineWidth, (int)(0 * xRate), (int)(90 * yRate), (int)(90 * xRate), (int)(90 * xRate), (int)(70 * yRate));
        paint->drawLine(foreColor, lineWidth, (int)(0 * xRate), (int)(115 * yRate), (int)(115 * xRate), (int)(135 * xRate), (int)(135 * yRate));
        paint->drawLine(foreColor, lineWidth, (int)(0 * xRate), (int)(115 * yRate), (int)(115 * xRate), (int)(135 * xRate), (int)(115 * yRate));
        paint->drawLine(foreColor, lineWidth, (int)(0 * xRate), (int)(115 * yRate), (int)(115 * xRate), (int)(115 * xRate), (int)(135 * yRate));
    }
    paint->setLineCap(0, 0);
}

void WindowButton::onPaintBorder(FCPaint * paint, const FCRect& clipRect){
    int width = getWidth(), height = getHeight();
    FCRect drawRect = {1, 1, width - 1, height - 1};
    if(m_isEllipse){
        paint->drawEllipse(getPaintingBorderColor(), 1, 0, drawRect);
    }
    else{
        paint->drawRect(getPaintingBorderColor(), 1, 0, drawRect);
    }
}
