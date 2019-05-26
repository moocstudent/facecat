#include "stdafx.h"
#include "FCDiv.h"
#include "FCGridColumn.h"
#import <mach/mach_time.h>

namespace FaceCat{
    void FCDiv::scrollButtonKeyDown(Object sender, char key, Object pInvoke){
        FCDiv *div =(FCDiv*)pInvoke;
        if(div){
            div->onKeyDown(key);
        }
    }
    
    void FCDiv::scrollButtonTouchWheel(Object sender, FCTouchInfo touchInfo, Object pInvoke){
        FCDiv *div =(FCDiv*)pInvoke;
        if(div){
            FCTouchInfo newTouchInfo = touchInfo;
            newTouchInfo.m_firstPoint = div->getTouchPoint();
            newTouchInfo.m_secondPoint = div->getTouchPoint();
            div->onTouchWheel(newTouchInfo);
        }
    }
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    FCDiv::FCDiv(){
        m_allowDragScroll = false;
        m_hScrollBar = 0;
        m_isDragScrolling = false;
        m_isDragScrolling2 = false;
        m_readyToDragScroll = false;
        m_scrollButtonKeyDownEvent = scrollButtonKeyDown;
        m_scrollButtonTouchWheelEvent = scrollButtonTouchWheel;
        m_showHScrollBar = false;
        m_showVScrollBar = false;
        m_startMovePoint.x = 0;
        m_startMovePoint.y = 0;
        m_startMovePosX = 0;
        m_startMovePosY = 0;
        m_startMoveTime = 0;
        m_vScrollBar = 0;
        FCSize size ={200, 200};
        setSize(size);
    }
    
    FCDiv::~FCDiv(){
        m_scrollButtonKeyDownEvent = 0;
        m_scrollButtonTouchWheelEvent = 0;
        m_hScrollBar = 0;
        m_vScrollBar = 0;
    }
    
    bool FCDiv::allowDragScroll(){
        return m_allowDragScroll;
    }
    
    void FCDiv::setAllowDragScroll(bool allowDragScroll){
        m_allowDragScroll = allowDragScroll;
    }
    
    FCHScrollBar* FCDiv::getHScrollBar(){
        if (getNative() && m_showHScrollBar){
            if (!m_hScrollBar){
                FCHost *host = getNative()->getHost();
                m_hScrollBar = dynamic_cast<FCHScrollBar*>(host->createInternalControl(this, L"hscrollbar"));
                addControl(m_hScrollBar);
                m_hScrollBar->getAddButton()->addEvent((Object)m_scrollButtonKeyDownEvent, FCEventID::KEYDOWN, this);
                m_hScrollBar->getAddButton()->addEvent((Object)m_scrollButtonTouchWheelEvent, FCEventID::TOUCHWHEEL, this);
                m_hScrollBar->getBackButton()->addEvent((Object)m_scrollButtonKeyDownEvent, FCEventID::KEYDOWN, this);
                m_hScrollBar->getBackButton()->addEvent((Object)m_scrollButtonTouchWheelEvent, FCEventID::TOUCHWHEEL, this);
                m_hScrollBar->getReduceButton()->addEvent((Object)m_scrollButtonKeyDownEvent, FCEventID::KEYDOWN, this);
                m_hScrollBar->getReduceButton()->addEvent((Object)m_scrollButtonTouchWheelEvent, FCEventID::TOUCHWHEEL, this);
                m_hScrollBar->getScrollButton()->addEvent((Object)m_scrollButtonKeyDownEvent, FCEventID::KEYDOWN, this);
                m_hScrollBar->getScrollButton()->addEvent((Object)m_scrollButtonTouchWheelEvent, FCEventID::TOUCHWHEEL, this);
            }
            return m_hScrollBar;
        }
        return 0;
    }
    
    bool FCDiv::showHScrollBar(){
        return m_showHScrollBar;
    }
    
    void FCDiv::setShowHScrollBar(bool showHScrollBar){
        m_showHScrollBar = showHScrollBar;
    }
    
    bool FCDiv::isDragScrolling(){
        return m_isDragScrolling;
    }
    
    bool FCDiv::showVScrollBar(){
        return m_showVScrollBar;
    }
    
    void FCDiv::setShowVScrollBar(bool showVScrollBar){
        m_showVScrollBar = showVScrollBar;
    }
    
    FCVScrollBar* FCDiv::getVScrollBar(){
        if (getNative() && m_showVScrollBar){
            if (!m_vScrollBar){
                FCHost *host = getNative()->getHost();
                m_vScrollBar = dynamic_cast<FCVScrollBar*>(host->createInternalControl(this, L"vscrollbar"));
                addControl(m_vScrollBar);
                m_vScrollBar->getAddButton()->addEvent((Object)m_scrollButtonKeyDownEvent, FCEventID::KEYDOWN, this);
                m_vScrollBar->getAddButton()->addEvent((Object)m_scrollButtonTouchWheelEvent, FCEventID::TOUCHWHEEL, this);
                m_vScrollBar->getBackButton()->addEvent((Object)m_scrollButtonKeyDownEvent, FCEventID::KEYDOWN, this);
                m_vScrollBar->getBackButton()->addEvent((Object)m_scrollButtonTouchWheelEvent, FCEventID::TOUCHWHEEL, this);
                m_vScrollBar->getReduceButton()->addEvent((Object)m_scrollButtonKeyDownEvent, FCEventID::KEYDOWN, this);
                m_vScrollBar->getReduceButton()->addEvent((Object)m_scrollButtonTouchWheelEvent, FCEventID::TOUCHWHEEL, this);
                m_vScrollBar->getScrollButton()->addEvent((Object)m_scrollButtonKeyDownEvent, FCEventID::KEYDOWN, this);
                m_vScrollBar->getScrollButton()->addEvent((Object)m_scrollButtonTouchWheelEvent, FCEventID::TOUCHWHEEL, this);
            }
            return m_vScrollBar;
        }
        return 0;
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    int FCDiv::getContentHeight(){
        FCHScrollBar *hScrollBar = getHScrollBar();
        FCVScrollBar *vScrollBar = getVScrollBar();
        int hmax = 0;
        ArrayList<FCView*> controls = getControls();
        for(int c = 0; c < controls.size(); c++){
            FCView *control = controls.get(c);
            if (control->isVisible() && control != hScrollBar && control != vScrollBar){
                int bottom = control->getBottom();
                if (bottom > hmax){
                    hmax = bottom;
                }
            }
        }
        return hmax;
    }
    
    int FCDiv::getContentWidth(){
        FCHScrollBar *hScrollBar = getHScrollBar();
        FCVScrollBar *vScrollBar = getVScrollBar();
        int wmax = 0;
        ArrayList<FCView*> controls = getControls();
        for(int c = 0; c < controls.size(); c++){
            FCView *control = controls.get(c);
            if (control->isVisible() && control != hScrollBar && control != vScrollBar){
                int right = control->getRight();
                if (right > wmax){
                    wmax = right;
                }
            }
        }
        return wmax;
    }
    
    String FCDiv::getControlType(){
        return L"Div";
    }
    
    FCPoint FCDiv::getDisplayOffset(){
        FCPoint offset ={0};
        if(isVisible()){
            offset.x = (m_hScrollBar && m_hScrollBar->isVisible()) ? m_hScrollBar->getPos() : 0;
            offset.y = (m_vScrollBar && m_vScrollBar->isVisible()) ? m_vScrollBar->getPos() : 0;
        }
        return offset;
    }
    
    void FCDiv::getProperty(const String& name, String *value, String *type){
        if (name == L"allowdragscroll"){
            *type = L"bool";
            *value = FCStr::convertBoolToStr(allowDragScroll());
        }
        else if (name == L"showhscrollbar"){
            *type = L"bool";
            *value = FCStr::convertBoolToStr(showHScrollBar());
        }
        else if(name == L"showvscrollbar"){
            *type = L"bool";
            *value = FCStr::convertBoolToStr(showVScrollBar());
        }
        else{
            FCView::getProperty(name, value, type);
        }
    }
    
    ArrayList<String> FCDiv::getPropertyNames(){
        ArrayList<String> propertyNames = FCView::getPropertyNames();
        propertyNames.add(L"AllowDragScroll");
        propertyNames.add(L"ShowHScrollBar");
        propertyNames.add(L"ShowVScrollBar");
        return propertyNames;
    }
    
    void FCDiv::lineDown(){
        if (m_vScrollBar && m_vScrollBar->isVisible()){
            m_vScrollBar->lineAdd();
        }
    }
    
    void FCDiv::lineLeft(){
        if (m_hScrollBar && m_hScrollBar->isVisible()){
            m_hScrollBar->lineReduce();
        }
    }
    
    void FCDiv::lineRight(){
        if (m_hScrollBar && m_hScrollBar->isVisible()){
            m_hScrollBar->lineAdd();
        }
    }
    
    void FCDiv::lineUp(){
        if (m_vScrollBar && m_vScrollBar->isVisible()){
            m_vScrollBar->lineReduce();
        }
    }
    
    void FCDiv::onDragReady(FCPoint *startOffset){
        startOffset->x = 0;
        startOffset->y = 0;
    }
    
    void FCDiv::onDragScrollEnd(){
        m_isDragScrolling = false;
        if (m_readyToDragScroll){
            long nowTime = mach_absolute_time();
            FCPoint newPoint = getNative()->getTouchPoint();
            if (m_hScrollBar && m_hScrollBar->isVisible()){
                m_hScrollBar->onAddSpeedScrollStart(m_startMoveTime, nowTime, m_startMovePoint.x, newPoint.x);
            }
            if (m_vScrollBar && m_vScrollBar->isVisible()){
                m_vScrollBar->onAddSpeedScrollStart(m_startMoveTime, nowTime, m_startMovePoint.y, newPoint.y);
            }
            m_readyToDragScroll = false;
            invalidate();
        }
    }
    
    void FCDiv::onDragScrolling(){
        int width = getWidth(), height = getHeight();
        if (m_allowDragScroll && m_readyToDragScroll){
            if (!onDragScrollPermit()){
                m_readyToDragScroll = false;
                return;
            }
            bool paint = false;
            FCPoint newPoint = getNative()->getTouchPoint();
            if (m_hScrollBar && m_hScrollBar->isVisible()){
                if(abs(newPoint.x - m_startMovePoint.x) > width / 10){
                    m_isDragScrolling2 = true;
                }
                int newPos = m_startMovePosX + m_startMovePoint.x - newPoint.x;
                if(newPos != m_hScrollBar->getPos()){
                    m_hScrollBar->setPos(newPos);
                    m_hScrollBar->update();
                    paint = true;
                }
            }
            if (m_vScrollBar && m_vScrollBar->isVisible()){
                if (abs(newPoint.y - m_startMovePoint.y) > height / 10){
                    m_isDragScrolling2 = true;
                }
                int newPos = m_startMovePosY + m_startMovePoint.y - newPoint.y;
                if(newPos != m_vScrollBar->getPos()){
                    m_vScrollBar->setPos(newPos);
                    m_vScrollBar->update();
                    paint = true;
                }
            }
            if (paint){
                m_isDragScrolling = true;
                invalidate();
            }
        }
    }
    
    bool FCDiv::onDragScrollPermit(){
        FCView *focusedControl = getNative()->getFocusedControl();
        if (focusedControl){
            if (focusedControl->isDragging()){
                return false;
            }
            if (dynamic_cast<FCGridColumn*>(focusedControl)){
                return false;
            }
            if (focusedControl->getParent()){
                if (dynamic_cast<FCScrollBar*>(focusedControl->getParent())){
                    return false;
                }
            }
        }
        return true;
    }
    
    void FCDiv::onDragScrollStart(){
        m_isDragScrolling = false;
        m_isDragScrolling2 = false;
        FCView *focusedControl = getNative()->getFocusedControl();
        if (m_hScrollBar && m_hScrollBar->isVisible()){
            if (focusedControl == m_hScrollBar->getAddButton()
                || focusedControl == m_hScrollBar->getReduceButton()
                || focusedControl == m_hScrollBar->getBackButton()
                || focusedControl == m_hScrollBar->getScrollButton()){
                m_hScrollBar->setAddSpeed(0);
                return;
            }
        }
        if (m_vScrollBar && m_vScrollBar->isVisible()){
            if (focusedControl == m_vScrollBar->getAddButton()
                || focusedControl == m_vScrollBar->getReduceButton()
                || focusedControl == m_vScrollBar->getBackButton()
                || focusedControl == m_vScrollBar->getScrollButton()){
                m_vScrollBar->setAddSpeed(0);
                return;
            }
        }
        if (m_allowDragScroll){
            if (m_hScrollBar && m_hScrollBar->isVisible()){
                m_startMovePosX = m_hScrollBar->getPos();
                m_hScrollBar->setAddSpeed(0);
                m_readyToDragScroll = true;
            }
            if (m_vScrollBar && m_vScrollBar->isVisible()){
                m_startMovePosY = m_vScrollBar->getPos();
                m_vScrollBar->setAddSpeed(0);
                m_readyToDragScroll = true;
            }
            if (m_readyToDragScroll){
                m_startMovePoint = getNative()->getTouchPoint();
                m_startMoveTime = mach_absolute_time();
            }
        }
    }
    
    void FCDiv::onKeyDown(char key){
        FCView::onKeyDown(key);
        FCHost *host = getNative()->getHost();
        if(!host->isKeyPress(VK_CONTROL)
           && !host->isKeyPress(VK_MENU)
           && !host->isKeyPress(VK_SHIFT)){
            if (key == 38){
                lineUp();
            }
            else if (key == 40){
                lineDown();
            }
            invalidate();
        }
    }
    
    void FCDiv::onTouchDown(FCTouchInfo touchInfo){
        FCView::onTouchDown(touchInfo);
        if (!m_allowPreviewsEvent){
            onDragScrollStart();
        }
    }
    
    void FCDiv::onTouchMove(FCTouchInfo touchInfo){
        FCView::onTouchMove(touchInfo);
        if (!m_allowPreviewsEvent){
            onDragScrolling();
        }
    }
    
    void FCDiv::onTouchUp(FCTouchInfo touchInfo){
        FCView::onTouchUp(touchInfo);
        if (!m_allowPreviewsEvent){
            onDragScrollEnd();
        }
    }
    
    void FCDiv::onTouchWheel(FCTouchInfo touchInfo){
        FCView::onTouchWheel(touchInfo);
        if (touchInfo.m_delta > 0){
            lineUp();
            invalidate();
        }
        else if (touchInfo.m_delta < 0){
            lineDown();
            invalidate();
        }
    }
    
    bool FCDiv::onPreviewsTouchEvent(int eventID, FCTouchInfo touchInfo){
        if(callPreviewsTouchEvent(FCEventID::PREVIEWSTOUCHEVENT, eventID, touchInfo)){
            return true;
        }
        if (m_allowPreviewsEvent){
            if (eventID == FCEventID::TOUCHDOWN){
                onDragScrollStart();
            }
            else if (eventID == FCEventID::TOUCHMOVE){
                onDragScrolling();
            }
            else if (eventID == FCEventID::TOUCHUP){
                bool state = m_isDragScrolling;
                onDragScrollEnd();
                if (state && !m_isDragScrolling2){
                    return false;
                }
            }
        }
        return false;
    }
    
    void FCDiv::pageDown(){
        if (m_vScrollBar && m_vScrollBar->isVisible()){
            m_vScrollBar->pageAdd();
        }
    }
    
    void FCDiv::pageLeft(){
        if (m_hScrollBar && m_hScrollBar->isVisible()){
            m_hScrollBar->pageReduce();
        }
    }
    
    void FCDiv::pageRight(){
        if (m_hScrollBar && m_hScrollBar->isVisible()){
            m_hScrollBar->pageAdd();
        }
    }
    
    void FCDiv::pageUp(){
        if (m_vScrollBar && m_vScrollBar->isVisible()){
            m_vScrollBar->pageReduce();
        }
    }
    
    void FCDiv::setProperty(const String& name, const String& value){
        if(name == L"allowdragscroll"){
            setAllowDragScroll(FCStr::convertStrToBool(value));
        }
        else if (name == L"showhscrollbar"){
            setShowHScrollBar(FCStr::convertStrToBool(value));
        }
        else if(name == L"showvscrollbar"){
            setShowVScrollBar(FCStr::convertStrToBool(value));
        }
        else{
            FCView::setProperty(name, value);
        }
    }
    
    void FCDiv::update(){
        FCView::update();
        updateScrollBar();
    }
    
    void FCDiv::updateScrollBar(){
        if(getNative()){
            FCHScrollBar *hScrollBar = getHScrollBar();
            FCVScrollBar *vScrollBar = getVScrollBar();
            if(isVisible()){
                int width = getWidth(), height = getHeight();
                int hBarHeight = hScrollBar ? hScrollBar->getHeight() : 0;
                int vBarWidth = vScrollBar ? vScrollBar->getWidth() : 0;
                int wmax = getContentWidth(), hmax = getContentHeight();
                if(hScrollBar){
                    hScrollBar->setContentSize(wmax);
                    FCSize hSize ={width - vBarWidth, hBarHeight};
                    hScrollBar->setSize(hSize);
                    hScrollBar->setPageSize(width - vBarWidth);
                    FCPoint hLocation ={0, height - hBarHeight};
                    hScrollBar->setLocation(hLocation);
                    if (wmax <= width){
                        hScrollBar->setVisible(false);
                    }
                    else{
                        hScrollBar->setVisible(true);
                    }
                }
                if(vScrollBar){
                    vScrollBar->setContentSize(hmax);
                    FCSize vSize ={vBarWidth, height - hBarHeight};
                    vScrollBar->setSize(vSize);
                    vScrollBar->setPageSize(height - hBarHeight);
                    FCPoint vLocation ={width - vBarWidth, 0};
                    vScrollBar->setLocation(vLocation);
                    int vh = (hScrollBar && hScrollBar->isVisible()) ? height - hBarHeight : height;
                    if (hmax <= vh){
                        vScrollBar->setVisible(false);
                    }
                    else{
                        vScrollBar->setVisible(true);
                    }
                }
                if (hScrollBar && vScrollBar){
                    if (hScrollBar->isVisible() && !vScrollBar->isVisible()){
                        hScrollBar->setWidth(width);
                        hScrollBar->setPageSize(width);
                    }
                    else if (!hScrollBar->isVisible() && vScrollBar->isVisible()){
                        vScrollBar->setHeight(height);
                        vScrollBar->setPageSize(height);
                    }
                }
                if(hScrollBar && hScrollBar->isVisible()){
                    hScrollBar->update();
                }
                if(vScrollBar && vScrollBar->isVisible()){
                    vScrollBar->update();
                }
            }
        }
    }
}
