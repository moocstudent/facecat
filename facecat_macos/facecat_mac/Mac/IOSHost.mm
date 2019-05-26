#include "stdafx.h"
#include "IOSHost.h"
#include "RibbonButton.h"

IOSTimer::IOSTimer(){
    m_interval = 1000;
    m_tick = 0;
    m_timerID = 0;
}

IOSTimer::~IOSTimer(){
    
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void* start(void *lpParam){
    IOSHost *iOSHost = (IOSHost*)lpParam;
    iOSHost->m_threadState = 1;
    while(iOSHost->m_threadState == 1){
        if((int)iOSHost->m_invokes.size() > 0){
            iOSHost->m_invokeLock.lock();
            dispatch_sync(dispatch_get_main_queue(), ^{
                int size = (int)iOSHost->m_invokes.size();
                for(int i = 0; i < size; i++){
                    if(iOSHost->m_threadState == 1){
                        IOSInvoke *invoke = iOSHost->m_invokes[i];
                        invoke->m_control->onInvoke(invoke->m_args);
                        delete invoke;
                        invoke = 0;
                    }
                }
                iOSHost->m_invokes.clear();
            });
            iOSHost->m_invokeLock.unLock();
            continue;
        }
        iOSHost->onTimer();
        usleep(10000);
    }
    iOSHost->m_threadState = 3;
    return 0;
}

IOSHost::IOSHost(){
    m_allowOperate = true;
    m_allowPartialPaint = true;
    m_isViewAppear = true;
    m_mousePoint.x = 0;
    m_mousePoint.y = 0;
    m_native = 0;
    m_threadState = 0;
    m_view = 0;
    pthread_t invokeThread;
    pthread_create(&invokeThread, 0, start, this);
}

IOSHost::~IOSHost(){
    m_threadState = 2;
    int tick = 0;
    while(m_threadState == 2 && tick < 50){
        usleep(1000);
        tick++;
    }
    m_native = 0;
    m_lock.lock();
    map<int, IOSTimer*>::iterator sIter = m_timers.begin();
    for(; sIter != m_timers.end(); ++sIter){
        delete sIter->second;
    }
    m_timers.clear();
    m_lock.unLock();
    m_invokeLock.lock();
    int size = (int)m_invokes.size();
    for(int i = 0; i < size; i++){
        IOSInvoke *invoke = m_invokes[i];
        delete invoke;
        invoke = 0;
    }
    m_invokes.clear();
    m_invokeLock.unLock();
    m_view = 0;
    
}

FCNative* IOSHost::getNative(){
    return m_native;
}

void IOSHost::setNative(FCNative *native)
{
    m_native = native;
}

NSView* IOSHost::getView(){
    return m_view;
}

void IOSHost::setView(NSView *view){
    m_view = view;
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

CGPoint IOSHost::getCGPoint(const FCPoint& point){
    return CGPointMake(point.x, point.y);
}

CGRect IOSHost::getCGRect(const FCRect& rect){
    int rw = rect.right - rect.left;
    int rh = rect.bottom - rect.top;
    if(rw < 0){
        rw = 0;
    }
    if(rh < 0){
        rh = 0;
    }
    return CGRectMake(rect.left, rect.top, rw, rh);
}

CGSize IOSHost::getCGSize(const FCSize& size){
    return CGSizeMake(size.cx, size.cy);
}

NSString* IOSHost::getNSString(const wchar_t *str){
    string fstr = FCStr::wstringTostring(str);
    return [NSString stringWithUTF8String:fstr.c_str()];
}

FCPoint IOSHost::getPoint(CGPoint cgPoint){
    FCPoint point = {(int)cgPoint.x, (int)cgPoint.y};
    return point;
}

FCRect IOSHost::getRect(CGRect cgRect){
    FCRect rect = {(int)cgRect.origin.x, (int)cgRect.origin.y, (int)(cgRect.origin.x + cgRect.size.width),
        (int)(cgRect.origin.x + cgRect.size.height)};
    return rect;
}

FCSize IOSHost::getSize(CGSize cgSize){
    FCSize size = {(int)cgSize.width, (int)cgSize.height};
    return size;
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

bool IOSHost::allowOperate(){
    return m_allowOperate;
}

bool IOSHost::allowPartialPaint(){
    return m_allowPartialPaint;
}

void IOSHost::beginInvoke(FCView *control, void *args){
    IOSInvoke *invoke = new IOSInvoke;
    invoke->m_control = control;
    invoke->m_args = args;
    m_invokeLock.lock();
    m_invokes.push_back(invoke);
    m_invokeLock.unLock();
}

void IOSHost::copy(string text){
}

FCView* IOSHost::createInternalControl(FCView *parent, const String& clsid){
    FCCalendar *calendar = dynamic_cast<FCCalendar*>(parent);
    if (calendar)
    {
        if (clsid == L"datetitle"){
            return new DateTitle(calendar);
        }
        else if (clsid == L"headdiv"){
            HeadDiv *headDiv = new HeadDiv(calendar);
            headDiv->setWidth(parent->getWidth());
            headDiv->setDock(FCDockStyle_Top);
            return headDiv;
        }
        else if (clsid == L"lastbutton"){
            return new ArrowButton(calendar);
        }
        else if (clsid == L"nextbutton"){
            ArrowButton *nextBtn = new ArrowButton(calendar);
            nextBtn->setToLast(false);
            return nextBtn;
        }
    }
    FCSplitLayoutDiv *splitLayoutDiv = dynamic_cast<FCSplitLayoutDiv*>(parent);
    if (splitLayoutDiv){
        if (clsid == L"splitter"){
            FCButton *splitter = new FCButton;
            splitter->setBackColor(FCColor_Border);
            splitter->setBorderColor(FCColor_Border);
            FCSize size = {5, 5};
            splitter->setSize(size);
            return splitter;
        }
    }
    FCScrollBar *scrollBar = dynamic_cast<FCScrollBar*>(parent);
    if (scrollBar){
        scrollBar->setBorderColor(FCColor_None);
        scrollBar->setBackColor(FCColor_None);
        if (clsid == L"addbutton"){
            RibbonButton *addButton = new RibbonButton;
            FCSize size = {10, 10};
            addButton->setSize(size);
            if (dynamic_cast<FCScrollBar*>(scrollBar)){
                addButton->setArrowType(2);
            }
            else{
                addButton->setArrowType(4);
            }
            return addButton;
        }
        else if (clsid == L"backbutton"){
            FCButton *backButton = new FCButton;
            backButton->setBackColor(FCColor_None);
            backButton->setBorderColor(FCColor_None);
            return backButton;
        }
        else if (clsid == L"scrollbutton"){
            RibbonButton *scrollButton = new RibbonButton;
            scrollButton->setAllowDrag(true);
            if(dynamic_cast<FCVScrollBar*>(scrollBar)){
                scrollButton->setAngle(0);
            }
            return scrollButton;
        }
        else if (clsid == L"reducebutton"){
            RibbonButton *reduceButton = new RibbonButton;
            FCSize size = {10, 10};
            reduceButton->setSize(size);
            if (dynamic_cast<FCHScrollBar*>(scrollBar)){
                reduceButton->setArrowType(1);
            }
            else{
                reduceButton->setArrowType(3);
            }
            return reduceButton;
        }
    }
    FCTabPage *tabPage = dynamic_cast<FCTabPage*>(parent);
    if (tabPage){
        if (clsid == L"headerbutton"){
            RibbonButton *button = new RibbonButton;
            button->setAllowDrag(true);
            FCSize size = {100, 20};
            button->setSize(size);
            return button;
        }
    }
    FCComboBox *comboBox = dynamic_cast<FCComboBox*>(parent);
    if (comboBox){
        if (clsid == L"dropdownbutton"){
            RibbonButton *dropDownButton = new RibbonButton;
            dropDownButton->setArrowType(4);
            dropDownButton->setDisplayOffset(false);
            int width = comboBox->getWidth();
            int height = comboBox->getHeight();
            FCPoint location = {width - 20, 0};
            dropDownButton->setLocation(location);
            FCSize size = {20, height};
            dropDownButton->setSize(size);
            return dropDownButton;
        }
        else if (clsid == L"dropdownmenu"){
            FCComboBoxMenu *comboBoxMenu = new FCComboBoxMenu;
            comboBoxMenu->setComboBox(comboBox);
            comboBoxMenu->setPopup(true);
            FCSize size = {100, 200};
            comboBoxMenu->setSize(size);
            return comboBoxMenu;
        }
    }
    FCDateTimePicker *datePicker = dynamic_cast<FCDateTimePicker*>(parent);
    if (datePicker){
        if (clsid == L"dropdownbutton"){
            RibbonButton *dropDownButton = new RibbonButton;
            dropDownButton->setArrowType(4);
            dropDownButton->setDisplayOffset(false);
            int width = datePicker->getWidth();
            int height = datePicker->getHeight();
            FCPoint location = {width - 16, 0};
            dropDownButton->setLocation(location);
            FCSize size = {16, height};
            dropDownButton->setSize(size);
            return dropDownButton;
        }
        else if (clsid == L"dropdownmenu"){
            FCMenu *dropDownMenu = new FCMenu();
            FCPadding padding(1);
            dropDownMenu->setPadding(padding);
            dropDownMenu->setPopup(true);
            FCSize size = {200, 200};
            dropDownMenu->setSize(size);
            return dropDownMenu;
        }
    }
    FCSpin *spin = dynamic_cast<FCSpin*>(parent);
    if (spin){
        if (clsid == L"downbutton"){
            RibbonButton *downButton = new RibbonButton;
            downButton->setArrowType(4);
            downButton->setDisplayOffset(false);
            FCSize size = {16, 16};
            downButton->setSize(size);
            return downButton;
        }
        else if (clsid == L"upbutton"){
            RibbonButton *upButton = new RibbonButton;
            upButton->setArrowType(3);
            upButton->setDisplayOffset(false);
            FCSize size = {16, 16};
            upButton->setSize(size);
            return upButton;
        }
    }
    FCDiv *div = dynamic_cast<FCDiv*>(parent);
    if (div){
        if (clsid == L"hscrollbar"){
            FCHScrollBar *hScrollBar = new FCHScrollBar;
            hScrollBar->setVisible(false);
            FCSize size = {10, 10};
            hScrollBar->setSize(size);
            return hScrollBar;
        }
        else if (clsid == L"vscrollbar"){
            FCVScrollBar *vScrollBar = new FCVScrollBar;
            vScrollBar->setVisible(false);
            FCSize size = {10, 10};
            vScrollBar->setSize(size);
            return vScrollBar;
        }
    }
    FCGrid *grid = dynamic_cast<FCGrid*>(parent);
    if(grid){
        if(clsid == L"edittextbox"){
            FCTextBox *editTextBox = new FCTextBox;
            editTextBox->setBackColor(FCCOLORS_BACKCOLOR4);
            return editTextBox;
        }
    }
    return 0;
}

FCSize IOSHost::getClientSize(){
    FCSize size ={0};
    if(m_view){
        size = getSize(m_view.frame.size);
    }
    return size;
}

FCCursors IOSHost::getCursor(){
    FCCursors retCursorsStyle = FCCursors_Arrow;
    NSCursor *currentCursor = [NSCursor currentCursor];
    if(currentCursor == [NSCursor arrowCursor]){
        //
    }
    else if(currentCursor == [NSCursor closedHandCursor]){
        retCursorsStyle = FCCursors_ClosedHand;
    }
    else if(currentCursor == [NSCursor crosshairCursor]){
        retCursorsStyle = FCCursors_Cross;
    }
    else if(currentCursor == [NSCursor disappearingItemCursor]){
        retCursorsStyle = FCCursors_DisappearingItem;
    }
    else if(currentCursor == [NSCursor dragCopyCursor]){
        retCursorsStyle = FCCursors_DragCopy;
    }
    else if(currentCursor == [NSCursor dragLinkCursor]){
        retCursorsStyle = FCCursors_DragLink;
    }
    else if(currentCursor == [NSCursor openHandCursor]){
        retCursorsStyle = FCCursors_Hand;
    }
    else if(currentCursor == [NSCursor IBeamCursor]){
        retCursorsStyle = FCCursors_IBeam;
    }
    else if(currentCursor == [NSCursor IBeamCursorForVerticalLayout]){
        retCursorsStyle = FCCursors_IBeamCursorForVerticalLayout;
    }
    else if(currentCursor == [NSCursor operationNotAllowedCursor]){
        retCursorsStyle = FCCursors_No;
    }
    else if(currentCursor == [NSCursor pointingHandCursor]){
        retCursorsStyle = FCCursors_PointingHand;
    }
    else if(currentCursor == [NSCursor resizeDownCursor]){
        retCursorsStyle = FCCursors_SizeDown;
    }
    else if(currentCursor == [NSCursor resizeLeftCursor]){
        retCursorsStyle = FCCursors_SizeLeft;
    }
    else if(currentCursor == [NSCursor resizeLeftRightCursor]){
        retCursorsStyle = FCCursors_SizeRight;
    }
    else if(currentCursor == [NSCursor resizeUpCursor]){
        retCursorsStyle = FCCursors_SizeUp;
    }
    else if(currentCursor == [NSCursor resizeLeftRightCursor]){
        retCursorsStyle = FCCursors_SizeWE;
    }
    else if(currentCursor == [NSCursor resizeUpDownCursor]){
        retCursorsStyle = FCCursors_SizeNS;
    }
    else if(currentCursor == [[NSCursor class] performSelector:@selector(_windowResizeNorthEastSouthWestCursor)]){
        retCursorsStyle = FCCursors_SizeNESW;
    }
    else if(currentCursor == [[NSCursor class] performSelector:@selector(_windowResizeNorthWestSouthEastCursor)]){
        retCursorsStyle = FCCursors_SizeNWSE;
    }
    else if(currentCursor == [[NSCursor class] performSelector:@selector(_waitCursor)]){
        retCursorsStyle = FCCursors_WaitCursor;
    }
    return retCursorsStyle;
}

int IOSHost::getIntersectRect(FCRect *lpDestRect, const FCRect *lpSrc1Rect, const FCRect *lpSrc2Rect)
{
    lpDestRect->left = max(lpSrc1Rect->left, lpSrc2Rect->left);
    lpDestRect->right = min(lpSrc1Rect->right, lpSrc2Rect->right);
    lpDestRect->top = max(lpSrc1Rect->top, lpSrc2Rect->top);
    lpDestRect->bottom = min(lpSrc1Rect->bottom, lpSrc2Rect->bottom);
    if(lpDestRect->right >= lpDestRect->left && lpDestRect->bottom >= lpDestRect->top){
        return 1;
    }
    else{
        lpDestRect->left = 0;
        lpDestRect->right = 0;
        lpDestRect->top = 0;
        lpDestRect->bottom = 0;
        return 0;
    }
}

FCPoint IOSHost::getTouchPoint(){
    FCPoint mp = m_mousePoint;
    if (m_native->allowScaleSize()){
        FCSize clientSize = getClientSize();
        if (clientSize.cx > 0 && clientSize.cy > 0){
            FCSize scaleSize = m_native->getScaleSize();
            mp.x = mp.x * scaleSize.cx / clientSize.cx;
            mp.y = mp.y * scaleSize.cy / clientSize.cy;
        }
    }
    return mp;
}

FCSize IOSHost::getSize(){
    if (m_native->allowScaleSize()){
        return m_native->getScaleSize();
    }
    else{
        return getClientSize();
    }
}

int IOSHost::getUnionRect(FCRect *lpDestRect, const FCRect *lpSrc1Rect, const FCRect *lpSrc2Rect){
    return 0;
}

void IOSHost::invalidate(){
    [m_view setNeedsDisplay:true];
}

void IOSHost::invalidate(const FCRect& rect){
    if(m_allowPartialPaint){
        FCSize displaySize = m_native->getDisplaySize();
        double scaleFactorX = 1, scaleFactorY = 1;
        FCSize clientSize = getClientSize();
        if (m_native->allowScaleSize()){
            if (clientSize.cx > 0 && clientSize.cy > 0){
                FCSize scaleSize = m_native->getScaleSize();
                scaleFactorX = (double)(clientSize.cx) / scaleSize.cx;
                scaleFactorY = (double)(clientSize.cy) / scaleSize.cy;
            }
        }
        FCRect newRect = rect;
        if(scaleFactorX > 0 && scaleFactorY > 0){
            newRect.left = (int)(newRect.left * scaleFactorX);
            newRect.top = (int)(newRect.top * scaleFactorY);
            newRect.right = (int)(newRect.right * scaleFactorX);
            newRect.bottom = (int)(newRect.bottom * scaleFactorY);
        }
        CGRect drawRect = getCGRect(newRect);
        drawRect.origin.y = clientSize.cy - newRect.top - (newRect.bottom - newRect.top);
        [m_view setNeedsDisplayInRect:drawRect];
    }
    else{
        invalidate();
    }
}

void IOSHost::invoke(FCView *control, void *args){
    IOSInvoke *invoke = new IOSInvoke;
    invoke->m_control = control;
    invoke->m_args = args;
    m_invokes.push_back(invoke);
}


void IOSHost::onPaint(const FCRect& rect){
    FCSize displaySize = m_native->getDisplaySize();
    double scaleFactorX = 1, scaleFactorY = 1;
    FCSize clientSize = getClientSize();
    if (m_native->allowScaleSize()){
        if (clientSize.cx > 0 && clientSize.cy > 0){
            FCSize scaleSize = m_native->getScaleSize();
            scaleFactorX = (double)(clientSize.cx) / scaleSize.cx;
            scaleFactorY = (double)(clientSize.cy) / scaleSize.cy;
        }
    }
    FCPaint *paint = m_native->getPaint();
    FCRect wRect = {0, 0, clientSize.cx, clientSize.cy};
    paint->setScaleFactor(scaleFactorX, scaleFactorY);
    paint->beginPaint(0, wRect, rect);
    m_native->onPaint(rect);
    paint->endPaint();
}

void IOSHost::onTimer(){
    if(m_native){
        if(m_isViewAppear && m_threadState == 1){
            int timersSize = (int)m_timers.size();
            if(timersSize > 0){
                vector<int> runningTimerIDs;
                m_lock.lock();
                map<int, IOSTimer*>::iterator sIter = m_timers.begin();
                for(; sIter != m_timers.end(); ++sIter){
                    IOSTimer *timer = sIter->second;
                    int interval = timer->m_interval / 10;
                    if(interval < 1){
                        interval = 1;
                    }
                    if (timer->m_tick % interval == 0){
                        runningTimerIDs.push_back(timer->m_timerID);
                    }
                    timer->m_tick++;
                }
                m_lock.unLock();
                if(m_threadState == 1){
                    int runningTimerIDsSize = (int)runningTimerIDs.size();
                    for(int i = 0; i < runningTimerIDsSize; i++){
                        if([NSThread isMainThread] == YES){
                            if(m_threadState == 1){
                                m_native->onTimer(runningTimerIDs[i]);
                            }
                        }
                        else{
                            dispatch_sync(dispatch_get_main_queue(), ^{
                                if(m_threadState == 1){
                                    m_native->onTimer(runningTimerIDs[i]);
                                }
                            });
                        }
                    }
                    runningTimerIDs.clear();
                }
            }
        }
    }
    
}

string IOSHost::paste(){
    return "";
}

void IOSHost::setAllowOperate(bool allowOperate){
    m_allowOperate = allowOperate;
}

void IOSHost::setAllowPartialPaint(bool allowPartialPaint){
    m_allowPartialPaint = allowPartialPaint;
}

void IOSHost::setCursor(FCCursors cursor){
    NSCursor *targetCursor = nil;
    switch (cursor) {
        case FCCursors_Arrow:
            targetCursor = [NSCursor arrowCursor];
            break;
        case FCCursors_ClosedHand:
            targetCursor = [NSCursor closedHandCursor];
            break;
        case FCCursors_Cross:
            targetCursor = [NSCursor crosshairCursor];
            break;
        case FCCursors_DisappearingItem:
            targetCursor = [NSCursor disappearingItemCursor];
            break;
        case FCCursors_DragCopy:
            targetCursor = [NSCursor dragCopyCursor];
            break;
        case FCCursors_DragLink:
            targetCursor = [NSCursor dragLinkCursor];
            break;
        case FCCursors_Hand:
            targetCursor = [NSCursor openHandCursor];
            break;
        case FCCursors_IBeam:
            targetCursor = [NSCursor IBeamCursor];
            break;
        case FCCursors_IBeamCursorForVerticalLayout:
            targetCursor = [NSCursor IBeamCursorForVerticalLayout];
            break;
        case FCCursors_No:
            targetCursor = [NSCursor operationNotAllowedCursor];
            break;
        case FCCursors_PointingHand:
            targetCursor = [NSCursor pointingHandCursor];
            break;
        case FCCursors_SizeDown:
            targetCursor = [NSCursor resizeDownCursor];
            break;
        case FCCursors_SizeLeft:
            targetCursor = [NSCursor resizeLeftCursor];
            break;
        case FCCursors_SizeRight:
            targetCursor = [NSCursor resizeLeftRightCursor];
            break;
        case FCCursors_SizeUp:
            targetCursor = [NSCursor resizeUpCursor];
            break;
        case FCCursors_SizeWE:
            targetCursor = [NSCursor resizeLeftRightCursor];
            break;
        case FCCursors_SizeNS:
            targetCursor = [NSCursor resizeUpDownCursor];
            break;
        case FCCursors_SizeNESW:
            targetCursor = [[NSCursor class] performSelector:@selector(_windowResizeNorthEastSouthWestCursor)];
            break;
        case FCCursors_SizeNWSE:
            targetCursor = [[NSCursor class] performSelector:@selector(_windowResizeNorthWestSouthEastCursor)];
            break;
        case FCCursors_WaitCursor:
            targetCursor = [[NSCursor class] performSelector:@selector(_waitCursor)];
            break;
    }
    NSCursor *currentCursor = [NSCursor currentCursor];
    if(targetCursor && targetCursor != currentCursor){
        [targetCursor set];
    }
}

void IOSHost::setTouchPoint(const FCPoint& mp){
    m_mousePoint = mp;
}

void IOSHost::startTimer(int timerID, int interval)
{
    m_lock.lock();
    map<int, IOSTimer*>::iterator sIter = m_timers.find(timerID);
    if(sIter != m_timers.end()){
        sIter->second->m_interval = interval;
        sIter->second->m_tick = 0;
    }
    else{
        IOSTimer *timer = new IOSTimer;
        timer->m_interval = interval;
        timer->m_timerID = timerID;
        m_timers[timerID] = timer;
    }
    m_lock.unLock();
}

void IOSHost::stopTimer(int timerID){
    m_lock.lock();
    map<int, IOSTimer*>::iterator sIter = m_timers.find(timerID);
    if(sIter != m_timers.end()){
        delete sIter->second;
        m_timers.erase(sIter);
    }
    m_lock.unLock();
}
