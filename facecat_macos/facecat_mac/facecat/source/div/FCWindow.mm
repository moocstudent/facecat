#include "stdafx.h"
#include "FCWindow.h"

namespace FaceCat{
    void FCWindow::callWindowClosingEvents(int eventID, bool *cancel){
        if(m_events.size() > 0){
            map<int, vector<Object>*>::iterator sIter = m_events.find(eventID);
            if(sIter != m_events.end()){
                map<int, vector<Object>*>::iterator sIter2 = m_invokes.find(eventID);
                vector<Object> *events = sIter->second;
                vector<Object> *invokes = sIter2->second;
                int eventSize = (int)events->size();
                for(int i = 0; i < eventSize; i++){
                    FCWindowClosingEvent func = (FCWindowClosingEvent)(*events)[i];
                    func(this, cancel, (*invokes)[i]);
                }
            }
        }
    }
    
    FCCursors FCWindow::getResizeCursor(int state){
        switch (state){
            case 0:
                return FCCursors_SizeNWSE;
            case 1:
                return FCCursors_SizeNESW;
            case 2:
                return FCCursors_SizeNESW;
            case 3:
                return FCCursors_SizeNWSE;
            case 4:
                return FCCursors_SizeWE;
            case 5:
                return FCCursors_SizeNS;
            case 6:
                return FCCursors_SizeWE;
            case 7:
                return FCCursors_SizeNS;
            default:
                return FCCursors_Arrow;
        }
    }
    
    ArrayList<FCRect> FCWindow::getResizePoints(){
        int width = getWidth(), height = getHeight();
        ArrayList<FCRect> points;
        FCRect rect1 ={0, 0, m_borderWidth * 2, m_borderWidth * 2};
        FCRect rect2 ={0, height - m_borderWidth * 2, m_borderWidth * 2, height};
        FCRect rect3 ={width - m_borderWidth * 2, 0, width, m_borderWidth * 2};
        FCRect rect4 ={width - m_borderWidth * 2, height - m_borderWidth * 2, width, height};
        FCRect rect5 ={0, 0, m_borderWidth, height};
        FCRect rect6 ={0, 0, width, m_borderWidth};
        FCRect rect7 ={width - m_borderWidth, 0, width, height};
        FCRect rect8 ={0, height - m_borderWidth, width, height};
        points.add(rect1);
        points.add(rect2);
        points.add(rect3);
        points.add(rect4);
        points.add(rect5);
        points.add(rect6);
        points.add(rect7);
        points.add(rect8);
        return points;
    }
    
    int FCWindow::getResizeState(){
        FCPoint mp = getTouchPoint();
        ArrayList<FCRect> pRects = getResizePoints();
        int rsize = pRects.size();
        for (int i = 0; i < rsize; i++){
            FCRect rect = pRects.get(i);
            if (mp.x >= rect.left && mp.x <= rect.right
                && mp.y >= rect.top && mp.y <= rect.bottom){
                return i;
            }
        }
        return -1;
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    FCWindow::FCWindow(){
        m_borderWidth = 2;
        m_canResize = false;
        m_captionHeight = 20;
        m_frame = 0;
        m_isDialog = false;
        m_resizePoint = -1;
        m_shadowColor = FCColor::argb(25, 255, 255, 255);
        m_shadowSize = 10;
        m_startTouchPoint.x = 0;
        m_startTouchPoint.y = 0;
        m_startRect.left = 0;
        m_startRect.top = 0;
        m_startRect.right = 0;
        m_startRect.bottom = 0;
        setAllowDrag(true);
        setVisible(false);
    }
    
    FCWindow::~FCWindow(){
        if (m_frame){
            m_frame->removeControl(this);
            getNative()->removeControl(m_frame);
            delete m_frame;
            m_frame = 0;
        }
    }
    
    int FCWindow::getBorderWidth(){
        return m_borderWidth;
    }
    
    void FCWindow::setBorderWidth(int borderWidth){
        m_borderWidth = borderWidth;
    }
    
    int FCWindow::getCaptionHeight(){
        return m_captionHeight;
    }
    
    void FCWindow::setCaptionHeight(int captionHeight){
        m_captionHeight = captionHeight;
    }
    
    bool FCWindow::canResize(){
        return m_canResize;
    }
    
    void FCWindow::setCanResize(bool canResize){
        m_canResize = canResize;
    }
    
    FCWindowFrame* FCWindow::getFrame(){
        return m_frame;
    }
    
    void FCWindow::setFrame(FCWindowFrame *frame){
        m_frame = frame;
    }
    
    bool FCWindow::isDialog(){
        return m_isDialog;
    }
    
    Long FCWindow::getShadowColor(){
        return m_shadowColor;
    }
    
    void FCWindow::setShadowColor(Long shadowColor){
        m_shadowColor = shadowColor;
    }
    
    int FCWindow::getShadowSize(){
        return m_shadowSize;
    }
    
    void FCWindow::setShadowSize(int shadowSize){
        m_shadowSize = shadowSize;
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    void FCWindow::bringToFront(){
        FCView::bringToFront();
        if(m_frame){
            m_frame->bringToFront();
        }
    }
    
    void FCWindow::close(){
        bool cancel = false;
        onWindowClosing(&cancel);
        if (!cancel){
            if (m_frame){
                m_frame->removeControl(this);
                getNative()->removeControl(m_frame);
                delete m_frame;
                m_frame = 0;
                setParent(0);
            }
            else{
                getNative()->removeControl(this);
            }
            onWindowClosed();
        }
    }
    
    String FCWindow::getControlType(){
        return L"Window";
    }
    
    FCRect FCWindow::getDynamicPaintRect(){
        FCSize oldSize = m_oldSize;
        if (oldSize.cx == 0 && oldSize.cy == 0){
            oldSize = getSize();
        }
        FCRect oldRect ={m_oldLocation.x, m_oldLocation.y, m_oldLocation.x + oldSize.cx,  m_oldLocation.y + oldSize.cy};
        FCRect rect ={m_location.x, m_location.y, m_location.x + getWidth(), m_location.y + getHeight()};
        FCRect paintRect ={min(oldRect.left, rect.left) - m_shadowSize - 10,
            min(oldRect.top, rect.top) - m_shadowSize - 10,
            max(oldRect.right, rect.right) + m_shadowSize + 10,
            max(oldRect.bottom, rect.bottom) + m_shadowSize + 10};
        return paintRect;
    }
    
    void FCWindow::getProperty(const String& name, String *value, String *type){
        if (name == L"borderwidth"){
            *type = L"int";
            *value = FCStr::convertIntToStr(getBorderWidth());
        }
        else if (name == L"canresize"){
            *type = L"bool";
            *value = FCStr::convertBoolToStr(canResize());
        }
        else if (name == L"captionheight"){
            *type = L"int";
            *value = FCStr::convertIntToStr(getCaptionHeight());
        }
        else if (name == L"shadowcolor"){
            *type = L"color";
            *value = FCStr::convertColorToStr(getShadowColor());
        }
        else if (name == L"shadowsize"){
            *type = L"int";
            *value = FCStr::convertIntToStr(getShadowSize());
        }
        else{
            FCView::getProperty(name, value, type);
        }
    }
    
    ArrayList<String> FCWindow::getPropertyNames(){
        ArrayList<String> propertyNames = FCView::getPropertyNames();
        propertyNames.add(L"BorderWidth");
        propertyNames.add(L"CanResize");
        propertyNames.add(L"CaptionHeight");
        propertyNames.add(L"ShadowColor");
        propertyNames.add(L"ShadowSize");
        return propertyNames;
    }
    
    bool FCWindow::onDragBegin(){
        FCPoint mp = getTouchPoint();
        if (mp.y > m_captionHeight){
            return false;
        }
        if (m_resizePoint != -1){
            return false;
        }
        return FCView::onDragBegin();
    }
    
    void FCWindow::onDragReady(FCPoint *startOffset){
        startOffset->x = 0;
        startOffset->y = 0;
    }
    
    void FCWindow::onTouchDown(FCTouchInfo touchInfo){
        FCView::onTouchDown(touchInfo);
        if (touchInfo.m_firstTouch && touchInfo.m_clicks == 1){
            if(m_canResize){
                m_resizePoint = getResizeState();
                setCursor(getResizeCursor(m_resizePoint));
                m_startTouchPoint = getNative()->getTouchPoint();
                m_startRect = getBounds();
            }
        }
        invalidate();
    }
    
    static void windowResize(int resizePoint, int *left, int *top, int *right, int *bottom, FCPoint *nowPoint, FCPoint *startTouchPoint){
        switch (resizePoint){
            case 0:
                *left = *left + nowPoint->x - startTouchPoint->x;
                *top = *top + nowPoint->y - startTouchPoint->y;
                break;
            case 1:
                *left = *left + nowPoint->x - startTouchPoint->x;
                *bottom = *bottom + nowPoint->y - startTouchPoint->y;
                break;
            case 2:
                *right = *right + nowPoint->x - startTouchPoint->x;
                *top = *top + nowPoint->y - startTouchPoint->y;
                break;
            case 3:
                *right = *right + nowPoint->x - startTouchPoint->x;
                *bottom = *bottom + nowPoint->y - startTouchPoint->y;
                break;
            case 4:
                *left = *left + nowPoint->x - startTouchPoint->x;
                break;
            case 5:
                *top = *top + nowPoint->y - startTouchPoint->y;
                break;
            case 6:
                *right = *right + nowPoint->x - startTouchPoint->x;
                break;
            case 7:
                *bottom = *bottom + nowPoint->y - startTouchPoint->y;
                break;
        }
    }
    
    void FCWindow::onTouchMove(FCTouchInfo touchInfo){
        FCView::onTouchMove(touchInfo);
        if(m_canResize){
            FCPoint nowPoint = getNative()->getTouchPoint();
            if (m_resizePoint != -1){
                int left = m_startRect.left, top = m_startRect.top, right = m_startRect.right, bottom = m_startRect.bottom;
                windowResize(m_resizePoint, &left, &top, &right, &bottom, &nowPoint, &m_startTouchPoint);
                FCRect bounds ={left, top, right, bottom};
                setBounds(bounds);
                getNative()->invalidate();
            }
            else{
                setCursor(getResizeCursor(getResizeState()));
            }
        }
    }
    
    void FCWindow::onTouchUp(FCTouchInfo touchInfo){
        FCView::onTouchUp(touchInfo);
        m_resizePoint = -1;
        invalidate();
    }
    
    void FCWindow::onPaintForeground(FCPaint *paint, const FCRect& clipRect){
        String text = getText();
        if (text.length() > 0){
            FCFont *font = getFont();
            FCSize tSize = paint->textSize(text.c_str(), font);
            FCRect tRect ={0};
            tRect.left = 5;
            tRect.top = (m_captionHeight - tSize.cy) / 2;
            tRect.right = tRect.left + tSize.cx;
            tRect.bottom = tRect.top + tSize.cy;
            paint->drawText(text.c_str(), getPaintingTextColor(), font, tRect);
        }
    }
    
    void FCWindow::onVisibleChanged(){
        FCView::onVisibleChanged();
        FCNative *native = getNative();
        if (native){
            if (isVisible()){
                if (!m_frame){
                    m_frame = new FCWindowFrame();
                }
                native->removeControl(this);
                native->addControl(m_frame);
                m_frame->setSize(native->getDisplaySize());
                if (!m_frame->containsControl(this)){
                    m_frame->addControl(this);
                }
            }
            else{
                if (m_frame){
                    m_frame->removeControl(this);
                    native->removeControl(m_frame);
                }
            }
        }
    }
    
    void FCWindow::onWindowClosing(bool *cancel){
        callWindowClosingEvents(FCEventID::WINDOWCLOSING, cancel);
    }
    
    void FCWindow::onWindowClosed(){
        callEvents(FCEventID::WINDOWCLOSED);
    }
    
    void FCWindow::sendToBack(){
        FCView::sendToBack();
        if(m_frame){
            m_frame->sendToBack();
        }
    }
    
    void FCWindow::setProperty(const String& name, const String& value){
        if (name == L"borderwidth"){
            setBorderWidth(FCStr::convertStrToInt(value));
        }
        else if (name == L"canresize"){
            setCanResize(FCStr::convertStrToBool(value));
        }
        else if (name == L"captionheight"){
            setCaptionHeight(FCStr::convertStrToInt(value));
        }
        else if (name == L"shadowcolor"){
            setShadowColor(FCStr::convertStrToColor(value));
        }
        else if (name == L"shadowsize"){
            setShadowSize(FCStr::convertStrToInt(value));
        }
        else{
            FCView::setProperty(name, value);
        }
    }
    
    void FCWindow::showDialog(){
        m_isDialog = true;
        show();
    }
}
