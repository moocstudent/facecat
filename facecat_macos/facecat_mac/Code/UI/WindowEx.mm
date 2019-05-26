#include "stdafx.h"
#include "WindowEx.h"

void WindowEx::clickButton(void *sender, FCTouchInfo touchInfo, void *pInvoke){
    if (touchInfo.m_firstTouch && touchInfo.m_clicks == 1){
        FCView *control = (FCView*)sender;
        WindowEx *window = (WindowEx*)pInvoke;
        String name = control->getName();
        if (name == L"btnMaxOrRestore"){
            window->maxOrRestore();
        }
        else if (name == L"btnMin"){
            window->min();
        }
    }
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

WindowEx::WindowEx(){
    m_animateDirection = -1;
    m_animateMoving = false;
    m_animateType = 0;
    m_closeButton = 0;
    m_isChildWindow = false;
    m_maxOrRestoreButton = 0;
    m_minButton = 0;
    m_normalLocation.x = 0;
    m_normalLocation.y = 0;
    m_normalSize.cx = 0;
    m_normalSize.cy = 0;
    m_timerID =  FCView::getNewTimerID();
    m_windowState = FCWindowState_Normal;
    setBackColor(FCColor_None);
    setBorderColor(FCColor_None);
    setCaptionHeight(23);
    FCFont wFont(L"SimSun", 14, true, false, false);
    setFont(&wFont);
    setTextColor(FCColor_None);
    setOpacity(0);
    setShadowSize(0);
}

WindowEx::~WindowEx(){
    m_animateMoving = false;
    m_closeButton = 0;
    m_maxOrRestoreButton = 0;
    m_minButton = 0;
    stopTimer(m_timerID);
}

bool WindowEx::isAnimateMoving(){
    return m_animateMoving;
}

WindowButton* WindowEx::getCloseButton(){
    return m_closeButton;
}

void WindowEx::setCloseButton(WindowButton* closeButton){
    m_closeButton = closeButton;
}

bool WindowEx::isChildWindow(){
    return m_isChildWindow;
}

void WindowEx::setChildWindow(bool isChildWindow){
    m_isChildWindow = isChildWindow;
}

WindowButton* WindowEx::getMaxOrRestoreButton(){
    return m_maxOrRestoreButton;
}

void WindowEx::setMaxOrRestoreButton(WindowButton *maxOrRestoreButton){
    m_maxOrRestoreButton = maxOrRestoreButton;
}

WindowButton* WindowEx::getMinButton(){
    return m_minButton;
}

void WindowEx::setMinButton(WindowButton *minButton){
    m_minButton = minButton;
}

FCWindowState WindowEx::getWindowState(){
    return m_windowState;
}

void WindowEx::setWindowState(FCWindowState windowState){
    m_windowState = windowState;
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void WindowEx::animateHide(){
    m_animateType = 1;
    FCNative *native = getNative();
    FCHost *host = native->getHost();
    m_animateDirection = rand() % 4;
    startTimer(m_timerID, 10);
    m_animateMoving = true;
    host->setAllowOperate(false);
}

void WindowEx::animateShow(bool showDialog){
    m_animateType = 0;
    FCNative *native = getNative();
    FCHost *host = native->getHost();
    host->setAllowOperate(false);
    FCSize nativeSize = native->getDisplaySize();
    int width = getWidth(), height = getHeight(), mx = (nativeSize.cx - width) / 2, my = (nativeSize.cy - height) / 2, x = mx, y = my;
    m_animateDirection = rand() % 4;
    switch (m_animateDirection){
        case 0:
            x = -width;
            break;
        case 1:
            x = nativeSize.cx;
            break;
        case 2:
            y = -height;
            break;
        case 3:
            y = nativeSize.cy;
            break;
    }
    FCPoint location = {x, y};
    setLocation(location);
    if(showDialog){
        this->showDialog();
        getFrame()->setBackColor(FCCOLORS_BACKCOLOR4);
    }
    else{
        show();
    }
    update();
    startTimer(m_timerID, 10);
    m_animateMoving = true;
}

void WindowEx::maxOrRestore(){
    if (m_windowState == FCWindowState_Normal){
        m_normalLocation = getLocation();
        m_normalSize = getSize();
        setDock(FCDockStyle_Fill);
        m_windowState = FCWindowState_Max;
        FCPoint maxLocation = {0, 0};
        setLocation(maxLocation);
        FCSize maxSize = getNative()->getDisplaySize();
        setSize(maxSize);
        m_maxOrRestoreButton->setStyle(WindowButtonStyle_Restore);
        getNative()->update();
        getNative()->invalidate();
    }
    else{
        setDock(FCDockStyle_None);
        m_windowState = FCWindowState_Normal;
        setLocation(m_normalLocation);
        setSize(m_normalSize);
        m_maxOrRestoreButton->setStyle(WindowButtonStyle_Max);
        getNative()->update();
        getNative()->invalidate();
    }
}

void WindowEx::min(){
    m_normalLocation = getLocation();
    m_normalSize = getSize();
    setDock(FCDockStyle_None);
    m_windowState = FCWindowState_Min;
    m_maxOrRestoreButton->setStyle(WindowButtonStyle_Restore);
    FCSize minSize = {150, getCaptionHeight()};
    setSize(minSize);
    getNative()->update();
    getNative()->invalidate();
}

void WindowEx::onAdd(){
    FCView::onAdd();
    if (!m_closeButton){
        m_closeButton = new WindowButton;
        m_closeButton->setName(L"btnClose");
        FCSize buttonSize = {20, 20};
        m_closeButton->setSize(buttonSize);
        addControl(m_closeButton);
    }
    if (!m_maxOrRestoreButton){
        m_maxOrRestoreButton = new WindowButton;
        m_maxOrRestoreButton->setName(L"btnMaxOrRestore");
        m_maxOrRestoreButton->setStyle(WindowButtonStyle_Max);
        FCSize buttonSize = {20, 20};
        m_maxOrRestoreButton->setSize(buttonSize);
        addControl(m_maxOrRestoreButton);
        m_maxOrRestoreButton->addEvent((void*)clickButton, FCEventID::CLICK, this);
    }
    if (!m_minButton){
        m_minButton = new WindowButton;
        m_minButton->setName(L"btnMin");
        m_minButton->setStyle(WindowButtonStyle_Min);
        FCSize buttonSize = {20, 20};
        m_minButton->setSize(buttonSize);
        addControl(m_minButton);
        m_minButton->addEvent((void*)clickButton, FCEventID::CLICK, this);
    }
}

void WindowEx::onDragReady(FCPoint *startOffset){
    startOffset->x = 0;
    startOffset->y = 0;
}

void WindowEx::onPaintBackground(FCPaint *paint, const FCRect& clipRect){
    int width = getWidth();
    int height = getHeight();
    FCRect rect = {0, 0, width, height};
    Long backColor = FCCOLORS_WINDOWBACKCOLOR3;
    Long borderColor = FCCOLORS_LINECOLOR;
    Long foreColor = FCCOLORS_WINDOWFORECOLOR;
    if (paint->supportTransparent()){
        if(m_isChildWindow){
            backColor = FCCOLORS_WINDOWBACKCOLOR2;
        }
        else{
            backColor = FCCOLORS_WINDOWBACKCOLOR3;
        }
    }
    int captionHeight = getCaptionHeight();
    FCRect contentRect = rect;
    contentRect.top += captionHeight;
    contentRect.bottom -= 6;
    contentRect.left += 6;
    contentRect.right -= 6;
    paint->beginPath();
    paint->addRect(contentRect);
    paint->excludeClipPath();
    paint->closePath();
    paint->fillRoundRect(backColor, rect, 6);
    paint->drawRoundRect(borderColor, 1, 0, rect, 6);
    paint->setClip(clipRect);
    if (contentRect.right - contentRect.left > 0 && contentRect.bottom - contentRect.top > 0){
        contentRect.top -= 1;
        contentRect.left -= 1;
        contentRect.right += 1;
        contentRect.bottom += 1;
        paint->fillRect(FCCOLORS_WINDOWCONTENTBACKCOLOR, contentRect);
    }
    FCPoint location = {5, 3};
    FCDraw::drawText(paint, getText().c_str(), foreColor, getFont(), location.x, location.y);
}

void WindowEx::onTimer(int timerID){
    FCView::onTimer(timerID);
    if (m_timerID == timerID){
        FCNative *native = getNative();
        FCHost *host = native->getHost();
        FCSize nativeSize = native->getDisplaySize();
        int x = getLeft(), y = getTop(), width = getWidth(), height = getHeight();
        if(m_animateType == 0){
            int xSub = nativeSize.cx / 2;
            int ySub = nativeSize.cy / 2;
            int mx = (nativeSize.cx - width) / 2;
            int my = (nativeSize.cy - height) / 2;
            float opacity = getOpacity();
            opacity += 0.1F;
            if(opacity > 1){
                opacity = 1;
            }
            setOpacity(opacity);
            bool stop = false;
            switch (m_animateDirection){
                case 0:
                    if (x + xSub >= mx){
                        x = mx;
                        stop = true;
                    }
                    else{
                        x += xSub;
                    }
                    break;
                case 1:
                    if (x - xSub <= mx){
                        x = mx;
                        stop = true;
                    }
                    else{
                        x -= xSub;
                    }
                    break;
                case 2:
                    if (y + ySub >= my){
                        y = my;
                        stop = true;
                    }
                    else{
                        y += ySub;
                    }
                    break;
                case 3:
                    if (y - ySub <= my){
                        y = my;
                        stop = true;
                    }
                    else{
                        y -= ySub;
                    }
                    break;
            }
            if (stop){
                setOpacity(1);
                m_animateMoving = false;
                stopTimer(m_timerID);
                host->setAllowOperate(true);
            }
        }
        else{
            int xSub = nativeSize.cx / 2;
            int ySub = nativeSize.cy / 2;
            bool stop = false;
            float opacity = getOpacity();
            opacity -= 0.1F;
            if(opacity < 0){
                opacity = 0;
            }
            setOpacity(opacity);
            switch (m_animateDirection){
                case 0:
                    if (x - xSub <= -width){
                        x = 0;
                        stop = true;
                    }
                    else{
                        x -= xSub;
                    }
                    break;
                case 1:
                    if (x +xSub >= nativeSize.cx){
                        x = 0;
                        stop = true;
                    }
                    else{
                        x += xSub;
                    }
                    break;
                case 2:
                    if (y - ySub <= -height){
                        y = 0;
                        stop = true;
                    }
                    else{
                        y -= ySub;
                    }
                    break;
                case 3:
                    if (y + ySub >= nativeSize.cy){
                        y = 0;
                        stop = true;
                    }
                    else{
                        y += ySub;
                    }
                    break;
            }
            if (stop){
                setOpacity(0);
                m_animateMoving = false;
                stopTimer(m_timerID);
                host->setAllowOperate(true);
                hide();
            }
        }
        FCPoint location = {x, y};
        setLocation(location);
        native->invalidate();
    }
}

void WindowEx::update(){
    FCView::update();
    int width = getWidth();
    if (m_closeButton){
        FCPoint location = {width - 26, 2};
        m_closeButton->setLocation(location);
    }
    if (m_maxOrRestoreButton){
        FCPoint location = {width - 48, 2};
        m_maxOrRestoreButton->setLocation(location);
    }
    if (m_minButton){
        FCPoint location = {width - 70, 2};
        m_minButton->setLocation(location);
    }
}
