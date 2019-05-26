#include "stdafx.h"
#include "FCView.h"
#include "FCNative.h"

namespace FaceCat{
    FCView* FCNative::findControl(const FCPoint& mp, ArrayList<FCView*> *controls){
        int size = (int)controls->size();
        for(int i = size - 1; i >= 0; i--){
            FCView *control = controls->get(i);
            if(control->isVisible()){
                if(control->containsPoint(mp)){
                    ArrayList<FCView*> subControls;
                    if(!getSortedControls(control, &subControls)){
                        if(control){
                            subControls = control->m_controls;
                        }
                        else{
                            subControls = m_controls;
                        }
                    }
                    FCView *subControl = findControl(mp, &subControls);
                    if(subControl){
                        return subControl;
                    }
                    else{
                        return control;
                    }
                }
            }
        }
        return 0;
    }
    
    FCView* FCNative::findControl(const String& name, ArrayList<FCView*> *controls){
        for(int c = 0; c < controls->size(); c++){
            FCView *control = controls->get(c);
            if(control->getName() == name){
                return control;
            }
            else{
                ArrayList<FCView*> subControls = control->m_controls;
                if(subControls.size() > 0){
                    FCView *fControl = findControl(name, &subControls);
                    if(fControl){
                        return fControl;
                    }
                }
            }
        }
        return 0;
    }
    
    FCView* FCNative::findPreviewsControl(FCView *control){
        if(control->allowPreviewsEvent()){
            return control;
        }
        else{
            FCView *parent = control->getParent();
            if(parent){
                return findPreviewsControl(parent);
            }
            else{
                return control;
            }
        }
    }
    
    FCView* FCNative::findWindow(FCView *control){
        if(control->isWindow()){
            return control;
        }
        else{
            FCView *parent = control->getParent();
            if(parent){
                return findWindow(parent);
            }
            else{
                return control;
            }
        }
    }
    
    float FCNative::getPaintingOpacity(FCView *control){
        float opacity = control->getOpacity();
        FCView *parent = control->getParent();
        if (parent){
            opacity *= getPaintingOpacity(parent);
        }
        else{
            opacity *= m_opacity;
        }
        return opacity;
    }
    
    String FCNative::getPaintingResourcePath(FCView *control){
        String resourcePath = control->getResourcePath();
        if (resourcePath.length() > 0){
            return resourcePath;
        }
        else{
            FCView *parent = control->getParent();
            if (parent){
                return getPaintingResourcePath(parent);
            }
            else{
                return m_resourcePath;
            }
        }
    }
    
    bool FCNative::getSortedControls(FCView *parent, ArrayList<FCView*> *sortedControls){
        ArrayList<FCView*> controls;
        if (parent){
            controls = parent->m_controls;
        }
        else{
            controls = m_controls;
        }
        for(int c = 0; c < controls.size(); c++){
            FCView *control = controls.get(c);
            if (control->isVisible() && control->isTopMost()){
                sortedControls->add(control);
            }
        }
        if (sortedControls->size() > 0){
            int controlsSize = (int)controls.size();
            for(int i = controlsSize - 1; i >= 0; i--){
                FCView *control = controls.get(i);
                if (control->isVisible() && !control->isTopMost()){
                    sortedControls->insert(0, control);
                }
            }
            return true;
        }
        else{
            return false;
        }
    }
    
    
    void FCNative::getTabStopControls(FCView *control, ArrayList<FCView*> *tabStopControls){
        ArrayList<FCView*> controls = control->m_controls;
        for(int c = 0; c < controls.size(); c++){
            FCView *subControl = controls.get(c);
            if (!subControl->isWindow()){
                if (subControl->isEnabled() && subControl->isTabStop()){
                    tabStopControls->add(subControl);
                }
                getTabStopControls(subControl, tabStopControls);
            }
        }
    }
    
    bool FCNative::isPaintEnabled(FCView *control){
        if (control->isEnabled()){
            FCView *parent = control->getParent();
            if (parent){
                return isPaintEnabled(parent);
            }
            else{
                return true;
            }
        }
        else{
            return false;
        }
    }
    
    void FCNative::renderControls(const FCRect& rect, ArrayList<FCView*> *controls, String resourcePath, float opacity){
        for(int c = 0; c < controls->size(); c++){
            FCView *control = controls->get(c);
            control->onPrePaint(m_paint, control->getDisplayRect());
            FCRect destRect;
            int clx = clientX(control);
            int cly = clientY(control);
            FCRect bounds ={clx, cly, clx + control->getWidth(), cly + control->getHeight()};
            if (control->useRegion()){
                FCRect clipRect = control->getRegion();
                bounds.left += clipRect.left;
                bounds.top += clipRect.top;
                bounds.right = bounds.left + clipRect.right - clipRect.left;
                bounds.bottom = bounds.top + clipRect.bottom - clipRect.top;
            }
            if (control->isVisible() && m_host->getIntersectRect(&destRect, &rect, &bounds) > 0){
                FCRect clipRect ={destRect.left - clx, destRect.top - cly,
                    destRect.right - clx, destRect.bottom - cly};
                String newResourcePath = control->getResourcePath();
                if(newResourcePath.length() == 0){
                    newResourcePath = resourcePath;
                }
                float newOpacity = control->getOpacity() * opacity;
                setPaint(clx, cly, clipRect, newResourcePath, newOpacity);
                control->onPaint(m_paint, clipRect);
                ArrayList<FCView*> subControls;
                if(!getSortedControls(control, &subControls)){
                    if(control){
                        subControls = control->m_controls;
                    }
                    else{
                        subControls = m_controls;
                    }
                }
                if (subControls.size() > 0){
                    renderControls(destRect, &subControls, newResourcePath, newOpacity);
                }
                setPaint(clx, cly, clipRect, newResourcePath, newOpacity);
                control->onPaintBorder(m_paint, clipRect);
            }
        }
    }
    
    void FCNative::setCursor(FCView *control){
        FCCursors cursor = control->getCursor();
        if(!isPaintEnabled(control)){
            cursor = FCCursors_Arrow;
        }
        if(cursor != getCursor()){
            setCursor(cursor);
        }
    }
    
    void FCNative::setPaint(int offsetX, int offsetY, const FCRect& clipRect, String resourcePath, float opacity){
        FCPoint offset ={offsetX, offsetY};
        m_paint->setOffset(offset);
        m_paint->setClip(clipRect);
        m_paint->setResourcePath(resourcePath);
        m_paint->setOpacity(opacity);
    }
    
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    FCNative::FCNative(){
        m_allowScaleSize = false;
        m_displaySize.cx = 0;
        m_displaySize.cy = 0;
        m_drawBeginPoint.x = 0;
        m_drawBeginPoint.y = 0;
        m_dragBeginRect.left = 0;
        m_dragBeginRect.top = 0;
        m_dragBeginRect.right = 0;
        m_dragBeginRect.bottom = 0;
        m_draggingControl = 0;
        m_dragStartOffset.x = 0;
        m_dragStartOffset.y = 0;
        m_focusedControl = 0;
        m_host = 0;
        m_mirrorHost = 0;
        m_mirrorMode = FCMirrorMode_None;
        m_touchDownControl = 0;
        m_touchDownPoint.x = 0;
        m_touchDownPoint.y = 0;
        m_touchMoveControl = 0;
        m_opacity = 1;
        m_paint = 0;
        m_rotateAngle = 0;
        m_scaleSize.cx = 0;
        m_scaleSize.cy = 0;
    }
    
    FCNative::~FCNative(){
        m_draggingControl = 0;
        m_focusedControl = 0;
        if(m_host){
            delete m_host;
            m_host = 0;
        }
        m_mirrorHost = 0;
        m_mirrors.clear();
        m_touchDownControl = 0;
        m_touchMoveControl = 0;
        m_timers.clear();
        clearControls();
        if(m_paint){
            delete m_paint;
            m_paint = 0;
        }
    }
    
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    bool FCNative::allowScaleSize(){
        return m_allowScaleSize;
    }
    
    void FCNative::setAllowScaleSize(bool allowScaleSize){
        m_allowScaleSize = allowScaleSize;
    }
    
    FCCursors FCNative::getCursor(){
        if(m_host){
            return m_host->getCursor();
        }
        return FCCursors_Arrow;
    }
    
    void FCNative::setCursor(FCCursors cursor){
        if(m_host){
            m_host->setCursor(cursor);
        }
    }
    
    FCSize FCNative::getDisplaySize(){
        return m_displaySize;
    }
    
    void FCNative::setDisplaySize(FCSize displaySize){
        m_displaySize = displaySize;
    }
    
    FCView* FCNative::getFocusedControl(){
        return m_focusedControl;
    }
    
    void FCNative::setFocusedControl(FCView *focusedControl){
        if (m_focusedControl != focusedControl){
            if (m_focusedControl){
                FCView *fControl = m_focusedControl;
                m_focusedControl = 0;
                fControl->onLostFocus();
            }
            m_focusedControl = focusedControl;
            if (m_focusedControl){
                m_focusedControl->onGotFocus();
            }
        }
    }
    
    FCHost* FCNative::getHost(){
        return m_host;
    }
    
    void FCNative::setHost(FCHost *host){
        if(m_host){
            delete m_host;
        }
        m_host = host;
    }
    
    FCView* FCNative::getHoveredControl(){
        return m_touchMoveControl;
    }
    
    FCNative* FCNative::getMirrorHost(){
        return m_mirrorHost;
    }
    
    FCMirrorMode FCNative::getMirrorMode(){
        return m_mirrorMode;
    }
    
    void FCNative::setMirrorMode(FCMirrorMode mirrorMode){
        m_mirrorMode = mirrorMode;
    }
    
    FCPoint FCNative::getTouchPoint(){
        FCPoint mp ={0};
        if(m_host){
            mp = m_host->getTouchPoint();
        }
        return mp;
    }
    
    float FCNative::getOpacity(){
        return m_opacity;
    }
    
    void FCNative::setOpacity(float opacity){
        m_opacity = opacity;
    }
    
    FCPaint* FCNative::getPaint(){
        return m_paint;
    }
    
    void FCNative::setPaint(FCPaint *paint){
        if(m_paint){
            delete m_paint;
        }
        m_paint = paint;
    }
    
    FCView* FCNative::getPushedControl(){
        return m_touchDownControl;
    }
    
    String FCNative::getResourcePath(){
        return m_resourcePath;
    }
    
    void FCNative::setResourcePath(const String& resourcePath){
        m_resourcePath = resourcePath;
    }
    
    int FCNative::getRotateAngle(){
        return m_rotateAngle;
    }
    
    void FCNative::setRotateAngle(int rotateAngle){
        m_rotateAngle = rotateAngle;
    }
    
    FCSize FCNative::getScaleSize(){
        return m_scaleSize;
    }
    
    void FCNative::setScaleSize(FCSize scaleSize){
        m_scaleSize = scaleSize;
    }
    
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    void FCNative::addControl(FCView *control){
        control->setNative(this);
        m_controls.add(control);
        control->onAdd();
    }
    
    void FCNative::addMirror(FCNative *mirrorHost, FCView *control){
        m_mirrorHost = mirrorHost;
        m_mirrorHost->m_mirrors.push_back(this);
        control->setNative(this);
        m_controls.add(control);
    }
    
    
    void FCNative::bringToFront(FCView *control){
        ArrayList<FCView*> controls;
        FCView *parent = control->getParent();
        if(parent){
            parent->bringChildToFront(control);
        }
        else{
            if(m_controls.size() > 0){
                for(int c = 0; c < m_controls.size(); c++){
                    if(m_controls.get(c) == control){
                        m_controls.removeAt(c);
                        break;
                    }
                }
                m_controls.add(control);
            }
        }
    }
    
    void FCNative::cancelDragging(){
        if(m_draggingControl){
            m_draggingControl->setBounds(m_dragBeginRect);
            FCView *draggingControl = m_draggingControl;
            m_draggingControl = 0;
            draggingControl->onDragEnd();
            FCView *parent = draggingControl->getParent();
            if (parent){
                parent->invalidate();
            }
            else{
                invalidate();
            }
        }
    }
    
    void FCNative::clearControls(){
        ArrayList<FCView*> controls;
        int controlsSize = (int)m_controls.size();
        for(int c = 0; c < m_controls.size(); c++){
            controls.add(m_controls.get(c));
        }
        for(int c = 0; c < m_controls.size(); c++){
            FCView *control = m_controls.get(c);
            control->onRemove();
            delete control;
            if(controlsSize != m_controls.size()){
                break;
            }
        }
        m_controls.clear();
    }
    
    int FCNative::clientX(FCView *control){
        if (control){
            FCView *parent = control->getParent();
            int cLeft = control->getLeft();
            if (parent){
                if (m_mirrorMode != FCMirrorMode_None){
                    int controlsSize = (int)m_controls.size();
                    for (int i = 0; i < controlsSize; i++){
                        if (m_controls.get(i) == control){
                            return cLeft;
                        }
                    }
                }
                return cLeft - (control->displayOffset() ? parent->getDisplayOffset().x : 0) +  clientX(parent);
            }
            else{
                return cLeft;
            }
        }
        else{
            return 0;
        }
    }
    
    int FCNative::clientY(FCView *control){
        if (control){
            FCView *parent = control->getParent();
            int cTop = control->getTop();
            if (parent){
                if (m_mirrorMode != FCMirrorMode_None){
                    int controlsSize = (int)m_controls.size();
                    for (int i = 0; i < controlsSize; i++){
                        if (m_controls.get(i) == control){
                            return cTop;
                        }
                    }
                }
                return cTop - (control->displayOffset() ? parent->getDisplayOffset().y : 0) + clientY(parent);
            }
            else{
                return cTop;
            }
        }
        else{
            return 0;
        }
    }
    
    bool FCNative::containsControl(FCView *control){
        if(m_controls.size() > 0){
            for(int c = 0; c < m_controls.size(); c++){
                if(m_controls.get(c) == control){
                    return true;
                }
            }
        }
        return false;
    }
    
    FCView* FCNative::findControl(const FCPoint& mp){
        ArrayList<FCView*> subControls;
        if(!getSortedControls(0, &subControls)){
            subControls = m_controls;
        }
        return findControl(mp, &subControls);
    }
    
    FCView* FCNative::findControl(const FCPoint& mp, FCView *parent){
        ArrayList<FCView*> subControls;
        if(!getSortedControls(parent, &subControls)){
            if(parent){
                subControls = parent->m_controls;
            }
            else{
                subControls = m_controls;
            }
        }
        return findControl(mp, &subControls);
    }
    
    FCView* FCNative::findControl(const String& name){
        return findControl(name, &m_controls);
    }
    
    ArrayList<FCView*> FCNative::getControls(){
        return m_controls;
    }
    
    void FCNative::insertControl(int index, FCView *control){
        m_controls.insert(index, control);
    }
    
    void FCNative::invalidate(){
        if(m_host){
            m_host->invalidate();
        }
    }
    
    void FCNative::invalidate(FCView *control){
        if(m_host){
            int clx = clientX(control);
            int cly = clientY(control);
            FCRect rect ={clx, cly, clx + control->getWidth(), cly + control->getHeight()};
            m_host->invalidate(rect);
            vector<FCNative*> mirrors;
            if (m_mirrorMode == FCMirrorMode_Shadow){
                clx = m_mirrorHost->clientX(control);
                cly = m_mirrorHost->clientY(control);
                FCRect rect2 ={clx, cly, clx + control->getWidth(), cly + control->getHeight()};
                m_mirrorHost->getHost()->invalidate(rect2);
                mirrors = m_mirrorHost->m_mirrors;
            }
            else{
                mirrors = m_mirrors;
            }
            int mirrorsSize = (int)mirrors.size();
            for (int i = 0; i < mirrorsSize; i++){
                if (mirrors[i] != this && mirrors[i]->getMirrorMode() != FCMirrorMode_BugHole){
                    clx = mirrors[i]->clientX(control);
                    cly = mirrors[i]->clientY(control);
                    FCRect rect3 ={clx, cly, clx + control->getWidth(), cly + control->getHeight()};
                    mirrors[i]->getHost()->invalidate(rect3);
                }
            }
        }
    }
    
    bool FCNative::onChar(wchar_t key){
        FCView *focusedControl = getFocusedControl();
        if(focusedControl && isPaintEnabled(focusedControl)){
            if(focusedControl->isTabStop()){
                FCView *window = findWindow(focusedControl);
                if(window){
                    if(!(m_host->isKeyPress(VK_CONTROL)
                         || m_host->isKeyPress(VK_MENU)
                         || m_host->isKeyPress(VK_SHIFT))
                       && key == 9 ){
                        ArrayList<FCView*> tabStopControls;
                        getTabStopControls(window, &tabStopControls);
                        int size = (int)tabStopControls.size();
                        if (size > 0){
                            for (int i = 0; i < size - 1; i++){
                                for (int j = 0; j < size - 1 - i; j++){
                                    FCView *controlLeft = tabStopControls.get(j);
                                    FCView *controlRight = tabStopControls.get(j + 1);
                                    if (controlLeft->getTabIndex() > controlRight->getTabIndex()){
                                        FCView *temp = tabStopControls.get(j + 1);
                                        tabStopControls.set(j + 1, tabStopControls.get(j));
                                        tabStopControls.set(j, temp);
                                    }
                                }
                            }
                            bool change = false;
                            FCView *newFocusedControl = 0;
                            for (int i = 0; i < size; i++){
                                FCView *control = tabStopControls.get(i);
                                if (focusedControl == control){
                                    if (i < size - 1){
                                        newFocusedControl = tabStopControls.get(i + 1);
                                    }
                                    else{
                                        newFocusedControl = tabStopControls.get(0);
                                    }
                                    change = true;
                                    break;
                                }
                            }
                            if (!change){
                                newFocusedControl = tabStopControls.get(0);
                            }
                            if (newFocusedControl != focusedControl){
                                newFocusedControl->setFocused(true);
                                focusedControl = newFocusedControl;
                                focusedControl->onTabStop();
                                window->invalidate();
                                return true;
                            }
                        }
                    }
                }
            }
            focusedControl->onChar(key);
        }
        return false;
    }
    
    void FCNative::onDoubleClick(FCTouchInfo touchInfo){
        FCView *focusedControl = getFocusedControl();
        if (focusedControl && isPaintEnabled(focusedControl)){
            FCPoint mp = getTouchPoint();
            int clx = clientX(focusedControl);
            int cly = clientY(focusedControl);
            FCPoint cmp ={mp.x - clx, mp.y - cly};
            FCTouchInfo newTouchInfo = touchInfo;
            newTouchInfo.m_firstPoint = cmp;
            newTouchInfo.m_secondPoint = cmp;
            focusedControl->onDoubleClick(newTouchInfo);
        }
    }
    
    void FCNative::onKeyDown(char key){
        FCView *focusedControl = getFocusedControl();
        if(focusedControl && isPaintEnabled(focusedControl)){
            focusedControl->onKeyDown(key);
        }
    }
    
    void FCNative::onKeyUp(char key){
        FCView *focusedControl = getFocusedControl();
        if(focusedControl && isPaintEnabled(focusedControl)){
            focusedControl->onKeyUp(key);
        }
    }
    
    void FCNative::onMouseDown(FCTouchInfo touchInfo){
        m_draggingControl = 0;
        m_touchDownControl = 0;
        FCPoint mp = getTouchPoint();
        m_touchDownPoint = mp;
        ArrayList<FCView*> subControls;
        if(!getSortedControls(0, &subControls)){
            subControls = m_controls;
        }
        FCView *control = findControl(mp, &subControls);
        if(control){
            FCView *window = findWindow(control);
            if(window && window->isWindow()){
                window->bringToFront();
            }
            if(isPaintEnabled(control)){
                int clx = clientX(control);
                int cly = clientY(control);
                FCPoint cmp ={mp.x - clx, mp.y - cly};
                FCView *focusedControl = getFocusedControl();
                m_touchDownControl = control;
                if (focusedControl == getFocusedControl()){
                    if(control->canFocus()){
                        if(touchInfo.m_firstTouch){
                            setFocusedControl(control);
                        }
                    }
                }
                FCTouchInfo newTouchInfo = touchInfo;
                newTouchInfo.m_firstPoint = mp;
                newTouchInfo.m_secondPoint = mp;
                if (onPreviewsTouchEvent(FCEventID::TOUCHDOWN, m_touchDownControl, newTouchInfo)){
                    return;
                }
                newTouchInfo.m_firstPoint = cmp;
                newTouchInfo.m_secondPoint = cmp;
                m_touchDownControl->onTouchDown(newTouchInfo);
                if(m_touchDownControl){
                    m_touchDownControl->onDragReady(&m_dragStartOffset);
                }
            }
        }
    }
    
    void FCNative::onMouseLeave(FCTouchInfo touchInfo){
        if (m_touchMoveControl && isPaintEnabled(m_touchMoveControl)){
            FCPoint mp = getTouchPoint();
            FCPoint cmp ={mp.x - clientX(m_touchMoveControl), mp.y - clientY(m_touchMoveControl)};
            FCView *touchMoverControl = m_touchMoveControl;
            m_touchMoveControl = 0;
            FCTouchInfo newTouchInfo = touchInfo;
            newTouchInfo.m_firstPoint = mp;
            newTouchInfo.m_secondPoint = mp;
            if (onPreviewsTouchEvent(FCEventID::TOUCHLEAVE, m_touchDownControl, touchInfo)){
                return;
            }
            newTouchInfo.m_firstPoint = cmp;
            newTouchInfo.m_secondPoint = cmp;
            touchMoverControl->onTouchLeave(newTouchInfo);
        }
    }
    
    void FCNative::onMouseMove(FCTouchInfo touchInfo){
        FCPoint mp = getTouchPoint();
        if(m_touchDownControl){
            FCTouchInfo newTouchInfo = touchInfo;
            newTouchInfo.m_firstPoint = mp;
            newTouchInfo.m_secondPoint = mp;
            if (onPreviewsTouchEvent(FCEventID::TOUCHMOVE, m_touchDownControl, newTouchInfo)){
                return;
            }
            newTouchInfo.m_firstPoint = mp;
            newTouchInfo.m_secondPoint = mp;
            FCPoint cmp ={mp.x - clientX(m_touchDownControl), mp.y - clientY(m_touchDownControl)};
            m_touchDownControl->onTouchMove(newTouchInfo);
            setCursor(m_touchDownControl);
            if (m_touchDownControl->allowDrag() && touchInfo.m_firstTouch && touchInfo.m_clicks == 1){
                if (abs(mp.x - m_touchDownPoint.x) > m_dragStartOffset.x
                    || abs(mp.y - m_touchDownPoint.y) > m_dragStartOffset.y){
                    if(m_touchDownControl->onDragBegin()){
                        m_dragBeginRect = m_touchDownControl->getBounds();
                        m_drawBeginPoint = m_touchDownPoint;
                        m_draggingControl = m_touchDownControl;
                        m_touchDownControl = 0;
                        FCView *parent = m_draggingControl->getParent();
                        if (parent){
                            parent->invalidate();
                        }
                        else{
                            invalidate();
                        }
                    }
                }
            }
        }
        else if(m_draggingControl){
            FCView *draggingControl = m_draggingControl;
            int offsetX = mp.x - m_drawBeginPoint.x;
            int offsetY = mp.y - m_drawBeginPoint.y;
            FCRect newBounds = m_dragBeginRect;
            newBounds.left += offsetX;
            newBounds.top += offsetY;
            newBounds.right += offsetX;
            newBounds.bottom += offsetY;
            draggingControl->setBounds(newBounds);
            draggingControl->onDragging();
            FCView *parent = draggingControl->getParent();
            if (parent){
                parent->invalidate();
            }
            else{
                invalidate();
            }
        }
        else{
            ArrayList<FCView*> subControls;
            if(!getSortedControls(0, &subControls)){
                subControls = m_controls;
            }
            FCView *control = findControl(mp, &subControls);
            if(control){
                FCTouchInfo newTouchInfo = touchInfo;
                newTouchInfo.m_firstPoint = mp;
                newTouchInfo.m_secondPoint = mp;
                if (onPreviewsTouchEvent(FCEventID::TOUCHMOVE, control, newTouchInfo)){
                    return;
                }
            }
            if(m_touchMoveControl != control){
                if(m_touchMoveControl && isPaintEnabled(m_touchMoveControl)){
                    if(!m_touchDownControl){
                        FCPoint cmp ={mp.x - clientX(m_touchMoveControl), mp.y - clientY(m_touchMoveControl)};
                        FCView *touchMoveControl = m_touchMoveControl;
                        m_touchMoveControl = 0;
                        FCTouchInfo newTouchInfo = touchInfo;
                        newTouchInfo.m_firstPoint = cmp;
                        newTouchInfo.m_secondPoint = cmp;
                        touchMoveControl->onTouchLeave(newTouchInfo);
                    }
                }
                if(control && isPaintEnabled(control)){
                    if(!m_touchDownControl){
                        FCPoint cmp ={mp.x - clientX(control), mp.y - clientY(control)};
                        m_touchMoveControl = control;
                        FCTouchInfo newTouchInfo = touchInfo;
                        newTouchInfo.m_firstPoint = cmp;
                        newTouchInfo.m_secondPoint = cmp;
                        control->onTouchEnter(newTouchInfo);
                        control->onTouchMove(newTouchInfo);
                        setCursor(control);
                    }
                }
            }
            else{
                if(control && isPaintEnabled(control)){
                    FCPoint cmp ={mp.x - clientX(control), mp.y - clientY(control)};
                    m_touchMoveControl = control;
                    FCTouchInfo newTouchInfo = touchInfo;
                    newTouchInfo.m_firstPoint = cmp;
                    newTouchInfo.m_secondPoint = cmp;
                    control->onTouchMove(newTouchInfo);
                    setCursor(control);
                }
            }
        }
    }
    
    void FCNative::onMouseUp(FCTouchInfo touchInfo){
        FCPoint mp = getTouchPoint();
        if(m_touchDownControl){
            ArrayList<FCView*> subControls;
            if(!getSortedControls(0, &subControls)){
                subControls = m_controls;
            }
            FCView *touchDownControl = m_touchDownControl;
            if (onPreviewsTouchEvent(FCEventID::TOUCHUP, touchDownControl, touchInfo)){
                return;
            }
            if(m_touchDownControl){
                FCView *control = findControl(mp, &subControls);
                FCPoint cmp = {mp.x - clientX(m_touchDownControl), mp.y - clientY(m_touchDownControl)};
                FCTouchInfo newTouchInfo = touchInfo;
                newTouchInfo.m_firstPoint = cmp;
                newTouchInfo.m_secondPoint = cmp;
                if(control && control == m_touchDownControl){
                    m_touchDownControl->onClick(newTouchInfo);
                }
                else{
                    m_touchMoveControl = 0;
                }
                if(m_touchDownControl){
                    touchDownControl = m_touchDownControl;
                    m_touchDownControl = 0;
                    touchDownControl->onTouchUp(newTouchInfo);
                }
            }
        }
        else if (m_draggingControl){
            ArrayList<FCView*> subControls;
            if(!getSortedControls(0, &subControls)){
                subControls = m_controls;
            }
            FCPoint cmp ={mp.x - clientX(m_touchDownControl), mp.y - clientY(m_touchDownControl)};
            FCView *draggingControl = m_draggingControl;
            m_draggingControl = 0;
            FCTouchInfo newTouchInfo = touchInfo;
            newTouchInfo.m_firstPoint = mp;
            newTouchInfo.m_secondPoint = mp;
            if (onPreviewsTouchEvent(FCEventID::TOUCHUP, draggingControl, newTouchInfo)){
                return;
            }
            newTouchInfo.m_firstPoint = cmp;
            newTouchInfo.m_secondPoint = cmp;
            draggingControl->onTouchUp(newTouchInfo);
            draggingControl->onDragEnd();
            FCView *parent = draggingControl->getParent();
            if (parent){
                parent->invalidate();
            }
            else{
                invalidate();
            }
        }
    }
    
    void FCNative::onMouseWheel(FCTouchInfo touchInfo){
        FCView *focusedControl = getFocusedControl();
        if(focusedControl && isPaintEnabled(focusedControl)){
            FCPoint mp = getTouchPoint();
            FCTouchInfo newTouchInfo = touchInfo;
            newTouchInfo.m_firstPoint = mp;
            newTouchInfo.m_secondPoint = mp;
            if (onPreviewsTouchEvent(FCEventID::TOUCHWHEEL, focusedControl, newTouchInfo)){
                return;
            }
            mp.x -= clientX(focusedControl);
            mp.y -= clientY(focusedControl);
            newTouchInfo.m_firstPoint = mp;
            newTouchInfo.m_secondPoint = mp;
            focusedControl->onTouchWheel(touchInfo);
        }
    }
    
    void FCNative::invalidate(const FCRect& rect){
        if(m_host){
            m_host->invalidate(rect);
        }
    }
    
    void FCNative::onPaint(const FCRect& rect){
        ArrayList<FCView*> subControls;
        if(!getSortedControls(0, &subControls)){
            subControls = m_controls;
        }
        renderControls(rect, &subControls, m_resourcePath, m_opacity);
    }
    
    bool FCNative::onPreviewsKeyEvent(int eventID, char key){
        FCView *focusedControl = getFocusedControl();
        if(focusedControl && isPaintEnabled(focusedControl)){
            FCView *window = findWindow(focusedControl);
            if(window){
                return window->onPreviewsKeyEvent(eventID, key);
            }
        }
        return false;
    }
    
    bool FCNative::onPreviewsTouchEvent(int eventID, FCView *control, FCTouchInfo touchInfo){
        FCView *previewsControl = findPreviewsControl(control);
        if (previewsControl){
            int clx = clientX(previewsControl);
            int cly = clientY(previewsControl);
            FCPoint wcmp = {touchInfo.m_firstPoint.x - clx, touchInfo.m_firstPoint.y - cly};
            FCTouchInfo newTouchInfo = touchInfo;
            newTouchInfo.m_firstPoint = wcmp;
            newTouchInfo.m_secondPoint = wcmp;
            if (previewsControl->onPreviewsTouchEvent(eventID, newTouchInfo)){
                return true;
            }
        }
        return false;
    }
    
    void FCNative::onResize(){
        update();
    }
    
    void FCNative::onTimer(int timerID){
        map<int, FCView*>::iterator sIter = m_timers.find(timerID);
        if(sIter != m_timers.end()){
            (sIter->second)->onTimer(timerID);
        }
        int mirrorsSize = (int)m_mirrors.size();
        if (mirrorsSize > 0){
            for (int i = 0; i < mirrorsSize; i++){
                m_mirrors[i]->onTimer(timerID);
            }
        }
    }
    
    void FCNative::onTouchBegin(FCTouchInfo touchInfo){
        m_draggingControl = 0;
        m_touchDownControl = 0;
        FCPoint mp = getTouchPoint();
        m_touchDownPoint = mp;
        ArrayList<FCView*> subControls;
        if(!getSortedControls(0, &subControls)){
            subControls = m_controls;
        }
        FCView *control = findControl(mp, &subControls);
        if(control){
            if(touchInfo.m_firstTouch && !touchInfo.m_secondTouch){
                FCView *window = findWindow(control);
                if(window && window->isWindow()){
                    window->bringToFront();
                }
            }
            if(isPaintEnabled(control)){
                int clx = clientX(control);
                int cly = clientY(control);
                FCPoint cmp ={mp.x - clx, mp.y - cly};
                FCView *focusedControl = getFocusedControl();
                m_touchDownControl = control;
                if(touchInfo.m_firstTouch && !touchInfo.m_secondTouch){
                    if (focusedControl == getFocusedControl()){
                        if(control->canFocus()){
                            setFocusedControl(control);
                        }
                    }
                    FCTouchInfo newTouchInfo = touchInfo;
                    newTouchInfo.m_firstPoint = mp;
                    newTouchInfo.m_secondPoint = mp;
                    if (onPreviewsTouchEvent(FCEventID::TOUCHDOWN, m_touchDownControl, newTouchInfo)){
                        return;
                    }
                    newTouchInfo.m_firstPoint = cmp;
                    newTouchInfo.m_secondPoint = cmp;
                    m_touchDownControl->onTouchDown(newTouchInfo);
                    if(m_touchDownControl){
                        m_touchDownControl->onDragReady(&m_dragStartOffset);
                    }
                }
            }
        }
    }
    
    void FCNative::onTouchCancel(FCTouchInfo touchInfo){
    }
    
    void FCNative::onTouchEnd(FCTouchInfo touchInfo){
        FCPoint mp = getTouchPoint();
        if(m_touchDownControl){
            FCPoint cmp ={mp.x - clientX(m_touchDownControl), mp.y - clientY(m_touchDownControl)};
            if(touchInfo.m_firstTouch && !touchInfo.m_secondTouch){
                ArrayList<FCView*> subControls;
                if(!getSortedControls(0, &subControls)){
                    subControls = m_controls;
                }
                FCView *touchDownControl = m_touchDownControl;
                FCTouchInfo newTouchInfo = touchInfo;
                newTouchInfo.m_firstPoint = mp;
                newTouchInfo.m_secondPoint = mp;
                if (onPreviewsTouchEvent(FCEventID::TOUCHUP, touchDownControl, newTouchInfo)){
                    return;
                }
                if(m_touchDownControl){
                    FCView *control = findControl(mp, &subControls);
                    newTouchInfo.m_firstPoint = cmp;
                    newTouchInfo.m_secondPoint = cmp;
                    if(control && control == m_touchDownControl){
                        m_touchDownControl->onClick(newTouchInfo);
                    }
                    else{
                        m_touchMoveControl = 0;
                    }
                    if(m_touchDownControl){
                        touchDownControl = m_touchDownControl;
                        m_touchDownControl = 0;
                        touchDownControl->onTouchUp(newTouchInfo);
                    }
                }
            }
        }
        else if (m_draggingControl){
            if(touchInfo.m_firstTouch && !touchInfo.m_secondTouch){
                ArrayList<FCView*> subControls;
                if(!getSortedControls(0, &subControls)){
                    subControls = m_controls;
                }
                FCPoint cmp ={mp.x - clientX(m_touchDownControl), mp.y - clientY(m_touchDownControl)};
                FCView *draggingControl = m_draggingControl;
                m_draggingControl = 0;
                FCTouchInfo newTouchInfo = touchInfo;
                newTouchInfo.m_firstPoint = mp;
                newTouchInfo.m_secondPoint = mp;
                if (onPreviewsTouchEvent(FCEventID::TOUCHUP, draggingControl, newTouchInfo)){
                    return;
                }
                newTouchInfo.m_firstPoint = cmp;
                newTouchInfo.m_secondPoint = cmp;
                draggingControl->onTouchUp(newTouchInfo);
                draggingControl->onDragEnd();
                FCView *parent = draggingControl->getParent();
                if (parent){
                    parent->invalidate();
                }
                else{
                    invalidate();
                }
            }
        }
    }
    
    void FCNative::onTouchMove(FCTouchInfo touchInfo){
        FCPoint mp = getTouchPoint();
        if(m_touchDownControl){
            if(touchInfo.m_firstTouch && !touchInfo.m_secondTouch){
                FCTouchInfo newTouchInfo = touchInfo;
                newTouchInfo.m_firstPoint = mp;
                newTouchInfo.m_secondPoint = mp;
                if (onPreviewsTouchEvent(FCEventID::TOUCHMOVE, m_touchDownControl, newTouchInfo)){
                    return;
                }
                FCPoint cmp ={mp.x - clientX(m_touchDownControl), mp.y - clientY(m_touchDownControl)};
                newTouchInfo.m_firstPoint = cmp;
                newTouchInfo.m_secondPoint = cmp;
                m_touchDownControl->onTouchMove(newTouchInfo);
                setCursor(m_touchDownControl);
                if (m_touchDownControl->allowDrag()){
                    if (abs(mp.x - m_touchDownPoint.x) > m_dragStartOffset.x
                        || abs(mp.y - m_touchDownPoint.y) > m_dragStartOffset.y){
                        if(m_touchDownControl->onDragBegin()){
                            m_dragBeginRect = m_touchDownControl->getBounds();
                            m_drawBeginPoint = m_touchDownPoint;
                            m_draggingControl = m_touchDownControl;
                            m_touchDownControl = 0;
                            FCView *parent = m_draggingControl->getParent();
                            if (parent){
                                parent->invalidate();
                            }
                            else{
                                invalidate();
                            }
                        }
                    }
                }
            }
        }
        else if(m_draggingControl){
            if(touchInfo.m_firstTouch && !touchInfo.m_secondTouch){
                FCView *draggingControl = m_draggingControl;
                int offsetX = mp.x - m_drawBeginPoint.x;
                int offsetY = mp.y - m_drawBeginPoint.y;
                FCRect newBounds = m_dragBeginRect;
                newBounds.left += offsetX;
                newBounds.top += offsetY;
                newBounds.right += offsetX;
                newBounds.bottom += offsetY;
                draggingControl->setBounds(newBounds);
                draggingControl->onDragging();
                FCView *parent = draggingControl->getParent();
                if (parent){
                    parent->invalidate();
                }
                else{
                    invalidate();
                }
            }
        }
    }
    
    void FCNative::removeControl(FCView *control){
        if(control == m_draggingControl){
            m_draggingControl = 0;
        }
        if(control == m_focusedControl){
            m_focusedControl = 0;
        }
        if(control == m_touchDownControl){
            m_touchDownControl = 0;
        }
        if(control == m_touchMoveControl){
            m_touchMoveControl = 0;
        }
        map<int, FCView*>::iterator sIter = m_timers.begin();
        vector<int> removeTimers;
        for(; sIter != m_timers.end(); ++ sIter){
            if(sIter->second == control){
                removeTimers.push_back(sIter->first);
            }
        }
        vector<int>::iterator sIter2 = removeTimers.begin();
        for(; sIter2 != removeTimers.end(); ++sIter2){
            stopTimer(*sIter2);
        }
        if(!control->getParent()){
            for(int c = 0; c < m_controls.size(); c++){
                if(m_controls.get(c) == control){
                    m_controls.removeAt(c);
                    control->onRemove();
                    return;
                }
            }
        }
    }
    
    void FCNative::removeMirror(FCView *control){
        vector<FCNative*>::iterator sIter  = m_mirrorHost->m_mirrors.begin();
        for(;sIter != m_mirrorHost->m_mirrors.end(); ++sIter){
            if(*sIter == this){
                m_mirrorHost->m_mirrors.erase(sIter);
                break;
            }
        }
        for(int c = 0; c < m_controls.size(); c++){
            if(m_controls.get(c) == control){
                m_controls.removeAt(c);
                break;
            }
        }
        control->setNative(m_mirrorHost);
    }
    
    void FCNative::sendToBack(FCView *control){
        ArrayList<FCView*> controls;
        FCView *parent = control->getParent();
        if(parent){
            parent->sendChildToBack(control);
        }
        else{
            if(m_controls.size() > 0){
                for(int c = 0; c < m_controls.size(); c++){
                    if(m_controls.get(c) == control){
                        m_controls.removeAt(c);
                        break;
                    }
                }
                m_controls.insert(0, control);
            }
        }
    }
    
    void FCNative::setAlign(ArrayList<FCView*> *controls){
        int controlSize = (int)controls->size();
        for (int i = 0; i < controlSize; i++){
            FCView *control = controls->get(i);
            if (control->displayOffset()){
                FCSize parentSize = m_displaySize;
                FCView *parent = control->getParent();
                if (parent){
                    if(!(m_mirrorMode == FCMirrorMode_BugHole && controls == &m_controls)){
                        parentSize = parent->getSize();
                    }
                }
                FCSize size = control->getSize();
                FCPadding margin = control->getMargin();
                FCPadding padding(0);
                if(parent){
                    padding = parent->getPadding();
                }
                if (control->getAlign() == FCHorizontalAlign_Center){
                    control->setLeft((parentSize.cx - size.cx) / 2);
                }
                else if (control->getAlign() == FCHorizontalAlign_Right){
                    control->setLeft(parentSize.cx - size.cx - margin.right - padding.right);
                }
                if (control->getVerticalAlign() == FCVerticalAlign_Bottom){
                    control->setTop(parentSize.cy - size.cy - margin.bottom - padding.bottom);
                }
                else if (control->getVerticalAlign() == FCVerticalAlign_Middle){
                    control->setTop((parentSize.cy - size.cy) / 2);
                }
            }
        }
    }
    
    void FCNative::setAnchor(FCRect *bounds, FCSize *parentSize, FCSize *oldSize, bool anchorLeft, bool anchorTop, bool anchorRight, bool anchorBottom){
        if(anchorRight && !anchorLeft){
            bounds->left = bounds->left + parentSize->cx - oldSize->cx;
        }
        if(anchorBottom && !anchorTop){
            bounds->top = bounds->top + parentSize->cy - oldSize->cy;
        }
        if(anchorRight){
            bounds->right = bounds->right + parentSize->cx - oldSize->cx;
        }
        if(anchorBottom){
            bounds->bottom = bounds->bottom + parentSize->cy - oldSize->cy;
        }
    }
    
    void FCNative::setAnchor(ArrayList<FCView*> *controls, FCSize oldSize){
        if(oldSize.cx != 0 && oldSize.cy != 0){
            for(int c = 0; c < controls->size(); c++){
                FCView *control = controls->get(c);
                FCSize parentSize = m_displaySize;
                FCView *parent = control->getParent();
                if (parent){
                    if(!(m_mirrorMode == FCMirrorMode_BugHole && controls == &m_controls)){
                        parentSize = parent->getSize();
                    }
                }
                FCAnchor anchor = control->getAnchor();
                FCRect bounds = control->getBounds();
                setAnchor(&bounds, &parentSize, &oldSize, anchor.left, anchor.top, anchor.right, anchor.bottom);
                control->setBounds(bounds);
            }
        }
    }
    
    void FCNative::setDock(FCRect *bounds, FCRect *spaceRect, FCSize *cSize, int dock){
        if (dock == 0){
            bounds->left = spaceRect->left;
            bounds->top = spaceRect->top;
            bounds->right = spaceRect->right;
            bounds->bottom = spaceRect->bottom;
        }
        else if (dock == 1){
            bounds->left = spaceRect->left;
            bounds->top = spaceRect->top;
            bounds->right = bounds->left + cSize->cx;
            bounds->bottom = spaceRect->bottom;
        }
        else if (dock == 2){
            bounds->left = spaceRect->left;
            bounds->top = spaceRect->top;
            bounds->right = spaceRect->right;
            bounds->bottom = bounds->top + cSize->cy;
        }
        else if (dock == 3){
            bounds->left = spaceRect->right - cSize->cx;
            bounds->top = spaceRect->top;
            bounds->right = spaceRect->right;
            bounds->bottom = spaceRect->bottom;
        }
        else if (dock == 4){
            bounds->left = spaceRect->left;
            bounds->top = spaceRect->bottom - cSize->cy;
            bounds->right = spaceRect->right;
            bounds->bottom = spaceRect->bottom;
        }
    }
    
    void FCNative::setDock(ArrayList<FCView*> *controls){
        for(int c = 0; c < controls->size(); c++){
            FCView *control = controls->get(c);
            FCSize parentSize = m_displaySize;
            FCView *parent = control->getParent();
            if (parent){
                if(!(m_mirrorMode == FCMirrorMode_BugHole && controls == &m_controls)){
                    parentSize = parent->getSize();
                }
            }
            FCDockStyle dock = control->getDock();
            if (dock != FCDockStyle_None){
                FCPadding padding(0);
                if(parent){
                    padding = parent->getPadding();
                }
                FCPadding margin = control->getMargin();
                FCSize cSize = control->getSize();
                FCRect spaceRect ={0};
                spaceRect.left = padding.left + margin.left;
                spaceRect.top = padding.top + margin.top;
                spaceRect.right = parentSize.cx - padding.right - margin.right;
                spaceRect.bottom = parentSize.cy - padding.bottom - margin.bottom;
                FCRect bounds ={0};
                int dockStyle = -1;
                if (dock == FCDockStyle_Bottom){
                    dockStyle = 4;
                }
                else if (dock == FCDockStyle_Fill){
                    dockStyle = 0;
                }
                else if (dock == FCDockStyle_Left){
                    dockStyle = 1;
                }
                else if (dock == FCDockStyle_Right){
                    dockStyle = 3;
                }
                else if (dock == FCDockStyle_Top){
                    dockStyle = 2;
                }
                setDock(&bounds, &spaceRect, &cSize, dockStyle);
                control->setBounds(bounds);
            }
        }
    }
    
    void FCNative::startTimer(FCView *control, int timerID, int interval){
        m_timers[timerID] = control;
        if(m_host){
            m_host->startTimer(timerID, interval);
        }
    }
    
    void FCNative::stopTimer(int timerID){
        map<int, FCView*>::iterator sIter = m_timers.find(timerID);
        if(sIter != m_timers.end()){
            if(m_host){
                m_host->stopTimer(timerID);
            }
            m_timers.erase(sIter);
        }
    }
    
    void FCNative::update(){
        if(m_host){
            FCSize size = m_host->getSize();
            FCSize oldSize = m_displaySize;
            m_displaySize = size;
            if(m_displaySize.cx != 0 && m_displaySize.cy != 0){
                setAlign(&m_controls);
                setAnchor(&m_controls, oldSize);
                setDock(&m_controls);
                int controlsSize = (int)m_controls.size();
                for (int i = 0; i < controlsSize; i++){
                    m_controls.get(i)->update();
                }
            }
        }
    }
}
