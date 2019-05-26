#include "stdafx.h"
#include "FCView.h"
#include "FCPaint.h"
#include "FCNative.h"

namespace FaceCat{
    void FCView::callEvents(int eventID){
        if(m_canRaiseEvents){
            if(m_events.size() > 0){
                map<int, vector<Object>*>::iterator sIter = m_events.find(eventID);
                if(sIter != m_events.end()){
                    map<int, vector<Object>*>::iterator sIter2 = m_invokes.find(eventID);
                    vector<Object> *events = sIter->second;
                    vector<Object> *invokes = sIter2->second;
                    int eventSize = (int)events->size();
                    for(int i = 0; i < eventSize; i++){
                        FCEvent func = (FCEvent)(*events)[i];
                        func(this, (*invokes)[i]);
                    }
                }
            }
        }
    }
    
    void FCView::callInvokeEvents(int eventID, Object args){
        if(m_canRaiseEvents){
            if(m_events.size() > 0){
                map<int, vector<Object>*>::iterator sIter = m_events.find(eventID);
                if(sIter != m_events.end()){
                    map<int, vector<Object>*>::iterator sIter2 = m_invokes.find(eventID);
                    vector<Object> *events = sIter->second;
                    vector<Object> *invokes = sIter2->second;
                    int eventSize = (int)events->size();
                    for(int i = 0; i < eventSize; i++){
                        FCInvokeEvent func = (FCInvokeEvent)(*events)[i];
                        func(this, args, (*invokes)[i]);
                    }
                }
            }
        }
    }
    
    void FCView::callKeyEvents(int eventID, char key){
        if(m_canRaiseEvents){
            if(m_events.size() > 0){
                map<int, vector<Object>*>::iterator sIter = m_events.find(eventID);
                if(sIter != m_events.end()){
                    map<int, vector<Object>*>::iterator sIter2 = m_invokes.find(eventID);
                    vector<Object> *events = sIter->second;
                    vector<Object> *invokes = sIter2->second;
                    int eventSize = (int)events->size();
                    for(int i = 0; i < eventSize; i++){
                        FCKeyEvent func = (FCKeyEvent)(*events)[i];
                        func(this, key, (*invokes)[i]);
                    }
                }
            }
        }
    }
    
    void FCView::callTouchEvents(int eventID, FCTouchInfo touchInfo){
        if(m_canRaiseEvents){
            if(m_events.size() > 0){
                map<int, vector<Object>*>::iterator sIter = m_events.find(eventID);
                if(sIter != m_events.end()){
                    map<int, vector<Object>*>::iterator sIter2 = m_invokes.find(eventID);
                    vector<Object> *events = sIter->second;
                    vector<Object> *invokes = sIter2->second;
                    int eventSize = (int)events->size();
                    for(int i = 0; i < eventSize; i++){
                        FCTouchEvent func = (FCTouchEvent)(*events)[i];
                        func(this, touchInfo, (*invokes)[i]);
                    }
                }
            }
        }
    }
    
    void FCView::callPaintEvents(int eventID, FCPaint *paint, const FCRect& clipRect){
        if(m_canRaiseEvents){
            if(m_events.size() > 0){
                map<int, vector<Object>*>::iterator sIter = m_events.find(eventID);
                if(sIter != m_events.end()){
                    map<int, vector<Object>*>::iterator sIter2 = m_invokes.find(eventID);
                    vector<Object> *events = sIter->second;
                    vector<Object> *invokes = sIter2->second;
                    int eventSize = (int)events->size();
                    for(int i = 0; i < eventSize; i++){
                        FCPaintEvent func = (FCPaintEvent)(*events)[i];
                        func(this, paint, clipRect, (*invokes)[i]);
                    }
                }
            }
        }
    }
    
    bool FCView::callPreviewsKeyEvent(int eventID, int tEventID, char key){
        if(m_canRaiseEvents){
            if(m_events.size() > 0){
                map<int, vector<Object>*>::iterator sIter = m_events.find(eventID);
                if(sIter != m_events.end()){
                    map<int, vector<Object>*>::iterator sIter2 = m_invokes.find(eventID);
                    vector<Object> *events = sIter->second;
                    vector<Object> *invokes = sIter2->second;
                    int eventSize = (int)events->size();
                    for(int i = 0; i < eventSize; i++){
                        FCPreviewsKeyEvent func = (FCPreviewsKeyEvent)(*events)[i];
                        if(func(this, tEventID, key, (*invokes)[i])){
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
    
    bool FCView::callPreviewsTouchEvent(int eventID, int tEventID, FCTouchInfo touchInfo){
        if(m_canRaiseEvents){
            if(m_events.size() > 0){
                map<int, vector<Object>*>::iterator sIter = m_events.find(eventID);
                if(sIter != m_events.end()){
                    map<int, vector<Object>*>::iterator sIter2 = m_invokes.find(eventID);
                    vector<Object> *events = sIter->second;
                    vector<Object> *invokes = sIter2->second;
                    int eventSize = (int)events->size();
                    for(int i = 0; i < eventSize; i++){
                        FCPreviewsTouchEvent func = (FCPreviewsTouchEvent)(*events)[i];
                        if(func(this, tEventID, touchInfo, (*invokes)[i])){
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
    
    void FCView::callTimerEvents(int eventID, int timerID){
        if(m_canRaiseEvents){
            if(m_events.size() > 0){
                map<int, vector<Object>*>::iterator sIter = m_events.find(eventID);
                if(sIter != m_events.end()){
                    map<int, vector<Object>*>::iterator sIter2 = m_invokes.find(eventID);
                    vector<Object> *events = sIter->second;
                    vector<Object> *invokes = sIter2->second;
                    int eventSize = (int)events->size();
                    for(int i = 0; i < eventSize; i++){
                        FCTimerEvent func = (FCTimerEvent)(*events)[i];
                        func(this, timerID, (*invokes)[i]);
                    }
                }
            }
        }
    }
    
    Long FCView::getPaintingBackColor(){
        if (m_backColor != FCColor_None && FCColor_DisabledBack != FCColor_None){
            if (!isPaintEnabled(this)){
                return FCColor_DisabledBack;
            }
        }
        return m_backColor;
    }
    
    String FCView::getPaintingBackImage(){
        return m_backImage;
    }
    
    Long FCView::getPaintingBorderColor(){
        return m_borderColor;
    }
    
    Long FCView::getPaintingTextColor(){
        if (m_textColor != FCColor_Text && FCColor_DisabledText != FCColor_None){
            if (!isPaintEnabled(this)){
                return FCColor_DisabledText;
            }
        }
        return m_textColor;
    }
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    FCView::FCView(){
        m_align = FCHorizontalAlign_Left;
        m_allowDrag = false;
        m_allowPreviewsEvent = false;
        m_autoEllipsis = false;
        m_autoSize = false;
        m_backColor = FCColor_Back;
        m_borderColor = FCColor_Border;
        m_canFocus = true;
        m_canRaiseEvents = true;
        m_cornerRadius = 0;
        m_cursor = FCCursors_Arrow;
        m_displayOffset = true;
        m_dock = FCDockStyle_None;
        m_enabled = true;
        m_font = new FCFont();
        m_textColor = FCColor_Text;
        m_hasPopupMenu = false;
        m_isDragging = false;
        m_isWindow = false;
        m_location.x = 0;
        m_location.y = 0;
        m_maximumSize.cx = 100000;
        m_maximumSize.cy = 100000;
        m_minimumSize.cx = 0;
        m_minimumSize.cy = 0;
        m_native = 0;
        m_oldLocation.x = 0;
        m_oldLocation.y = 0;
        m_oldSize.cx = 0;
        m_oldSize.cy = 0;
        m_opacity = 1;
        m_parent = 0;
        m_percentLocation = 0;
        m_percentSize = 0;
        m_region.left = 0;
        m_region.top = 0;
        m_region.right = 0;
        m_region.bottom = 0;
        m_size.cx = 0;
        m_size.cy = 0;
        m_tabIndex = 0;
        m_tabStop = false;
        m_tag = 0;
        m_topMost = false;
        m_useRegion = false;
        m_verticalAlign = FCVerticalAlign_Top;
        m_visible = true;
    }
    
    FCView::~FCView(){
        m_tag = 0;
        if(m_font){
            delete m_font;
            m_font = 0;
        }
        map<int, vector<Object>*>::iterator sIter = m_events.begin();
        for(; sIter != m_events.end(); ++sIter){
            delete sIter->second;
        }
        m_events.clear();
        map<int, vector<Object>*>::iterator sIter2 = m_invokes.begin();
        for(; sIter2 != m_invokes.end(); ++sIter2){
            delete sIter2->second;
        }
        m_invokes.clear();
        if(m_percentLocation){
            delete m_percentLocation;
            m_percentLocation = 0;
        }
        if(m_percentSize){
            delete m_percentSize;
            m_percentSize = 0;
        }
        clearControls();
        m_native = 0;
        m_parent = 0;
    }
    
    FCHorizontalAlign FCView::getAlign(){
        return m_align;
    }
    
    void FCView::setAlign(FCHorizontalAlign align){
        m_align = align;
    }
    
    bool FCView::allowDrag(){
        return m_allowDrag;
    }
    
    void FCView::setAllowDrag(bool allowDrag){
        m_allowDrag = allowDrag;
    }
    
    bool FCView::allowPreviewsEvent(){
        return m_allowPreviewsEvent;
    }
    
    void FCView::setAllowPreviewsEvent(bool allowPreviewsEvent){
        m_allowPreviewsEvent = allowPreviewsEvent;
    }
    
    FCAnchor FCView::getAnchor(){
        return m_anchor;
    }
    
    void FCView::setAnchor(const FCAnchor& anchor){
        m_anchor = anchor;
    }
    
    bool FCView::autoEllipsis(){
        return m_autoEllipsis;
    }
    
    void FCView::setAutoEllipsis(bool autoEllipsis){
        m_autoEllipsis = autoEllipsis;
    }
    
    bool FCView::autoSize(){
        return m_autoSize;
    }
    
    void FCView::setAutoSize(bool autoSize){
        if(m_autoSize != autoSize){
            m_autoSize = autoSize;
            onAutoSizeChanged();
        }
    }
    
    Long FCView::getBackColor(){
        return m_backColor;
    }
    
    void FCView::setBackColor(Long backColor){
        if(m_backColor != backColor){
            m_backColor = backColor;
            onBackColorChanged();
        }
    }
    
    String FCView::getBackImage(){
        return m_backImage;
    }
    
    void FCView::setBackImage(const String& backImage){
        if(m_backImage != backImage){
            m_backImage = backImage;
            onBackImageChanged();
        }
    }
    
    Long FCView::getBorderColor(){
        return m_borderColor;
    }
    
    void FCView::setBorderColor(Long borderColor){
        m_borderColor = borderColor;
    }
    
    int FCView::getBottom(){
        return getTop() + getHeight();
    }
    
    FCRect FCView::getBounds(){
        FCRect rect ={getLeft(), getTop(), getRight(), getBottom()};
        return rect;
    }
    
    void FCView::setBounds(const FCRect& rect){
        FCPoint location ={rect.left, rect.top};
        setLocation(location);
        int cx = rect.right - rect.left;
        int cy = rect.bottom - rect.top;
        FCSize size ={cx, cy};
        setSize(size);
    }
    
    bool FCView::canFocus(){
        return m_canFocus;
    }
    
    void FCView::setCanFocus(bool canFocus){
        m_canFocus = canFocus;
    }
    
    bool FCView::canRaiseEvents(){
        return m_canRaiseEvents;
    }
    
    void FCView::setCanRaiseEvents(bool canRaiseEvents){
        m_canRaiseEvents = canRaiseEvents;
    }
    
    bool FCView::isCapture(){
        if (m_native){
            if (m_native->getHoveredControl() == this || m_native->getPushedControl() == this){
                return true;
            }
        }
        return false;
    }
    
    int FCView::getCornerRadius(){
        return m_cornerRadius;
    }
    
    void FCView::setCornerRadius(int cornerRadius){
        m_cornerRadius = cornerRadius;
    }
    
    FCCursors FCView::getCursor(){
        return m_cursor;
    }
    
    void FCView::setCursor(FCCursors cursor){
        m_cursor = cursor;
    }
    
    bool FCView::displayOffset(){
        return m_displayOffset;
    }
    
    void FCView::setDisplayOffset(bool displayOffset){
        m_displayOffset = displayOffset;
    }
    
    FCRect FCView::getDisplayRect(){
        if (m_useRegion){
            return m_region;
        }
        else{
            FCRect displayRect ={0, 0, getWidth(), getHeight()};
            return displayRect;
        }
    }
    
    FCDockStyle FCView::getDock(){
        return m_dock;
    }
    
    void FCView::setDock(FCDockStyle dock){
        if(m_dock != dock){
            m_dock = dock;
            onDockChanged();
        }
    }
    
    bool FCView::isEnabled(){
        return m_enabled;
    }
    
    void FCView::setEnabled(bool enabled){
        if(m_enabled != enabled){
            m_enabled = enabled;
            onEnableChanged();
        }
    }
    
    bool FCView::isFocused(){
        if (m_native){
            if (m_native->getFocusedControl() == this){
                return true;
            }
        }
        return false;
    }
    
    void FCView::setFocused(bool focused){
        if (m_native){
            if (focused){
                m_native->setFocusedControl(this);
            }
            else{
                if (m_native->getFocusedControl() == this){
                    m_native->setFocusedControl(0);
                }
            }
        }
    }
    
    FCFont* FCView::getFont(){
        return m_font;
    }
    
    void FCView::setFont(FCFont *font){
        m_font->copy(font);
        onFontChanged();
    }
    
    bool FCView::hasPopupMenu(){
        return m_hasPopupMenu;
    }
    
    void FCView::setHasPopupMenu(bool hasPopupMenu){
        m_hasPopupMenu = hasPopupMenu;
    }
    
    int FCView::getHeight(){
        if(m_percentSize && m_percentSize->cy != -1){
            FCSize parentSize = m_parent ? m_parent->getSize() : m_native->getDisplaySize();
            return (int)(parentSize.cy * m_percentSize->cy);
        }
        else{
            return m_size.cy;
        }
    }
    
    void FCView::setHeight(int height){
        if(m_percentSize && m_percentSize->cy != -1){
            return;
        }
        else{
            FCSize size ={m_size.cx, height};
            setSize(size);
        }
    }
    
    bool FCView::isDragging(){
        return m_isDragging;
    }
    
    bool FCView::isWindow(){
        return m_isWindow;
    }
    
    void FCView::setWindow(bool isWindow){
        m_isWindow = isWindow;
    }
    
    int FCView::getLeft(){
        if(m_percentLocation && m_percentLocation->x != -1){
            FCSize parentSize = m_parent ? m_parent->getSize() : m_native->getDisplaySize();
            return (int)(parentSize.cx * m_percentLocation->x);
        }
        else{
            return m_location.x;
        }
    }
    
    void FCView::setLeft(int left){
        if(m_percentLocation && m_percentLocation->x != -1){
            return;
        }
        else{
            FCPoint location ={left, m_location.y};
            setLocation(location);
        }
    }
    
    FCPoint FCView::getLocation(){
        if(m_percentLocation){
            FCPoint location ={getLeft(), getTop()};
            return location;
        }
        else{
            return m_location;
        }
    }
    
    void FCView::setLocation(const FCPoint& location){
        if(location.x != m_location.x || location.y != m_location.y){
            if(m_percentLocation){
                m_oldLocation = getLocation();
                if(m_percentLocation->x != -1){
                }
                else{
                    m_location.x = location.x;
                }
                if(m_percentLocation->y != -1){
                }
                else{
                    m_location.y = location.y;
                }
            }
            else{
                m_oldLocation = m_location;
                m_location = location;
            }
            onLocationChanged();
        }
    }
    
    FCPadding FCView::getMargin(){
        return m_margin;
    }
    
    void FCView::setMargin(const FCPadding& margin){
        m_margin = margin;
        onMarginChanged();
    }
    
    FCSize FCView::getMaximumSize(){
        return m_maximumSize;
    }
    
    void FCView::setMaximumSize(FCSize maxinumSize){
        m_maximumSize = maxinumSize;
    }
    
    FCSize FCView::getMinimumSize(){
        return m_minimumSize;
    }
    
    void FCView::setMinimumSize(FCSize minimumSize){
        m_minimumSize = minimumSize;
    }
    
    FCPoint FCView::getTouchPoint(){
        if(m_native){
            FCPoint mp = m_native->getTouchPoint();
            return pointToControl(mp);
        }
        else{
            FCPoint mp ={0};
            return mp;
        }
    }
    
    String FCView::getName(){
        return m_name;
    }
    
    void FCView::setName(const String& name){
        m_name = name;
    }
    
    FCNative* FCView::getNative(){
        return m_native;
    }
    
    void FCView::setNative(FCNative *native){
        m_native = native;
        int controlsSize = (int)m_controls.size();
        for(int i = 0; i < controlsSize; i++){
            m_controls.get(i)->setNative(native);
        }
        onLoad();
    }
    
    float FCView::getOpacity(){
        return m_opacity;
    }
    
    void FCView::setOpacity(float opacity){
        m_opacity = opacity;
    }
    
    FCPadding FCView::getPadding(){
        return m_padding;
    }
    
    void FCView::setPadding(const FCPadding& padding){
        m_padding = padding;
        onPaddingChanged();
    }
    
    FCView* FCView::getParent(){
        return m_parent;
    }
    
    void FCView::setParent(FCView *control){
        if(m_parent != control){
            m_parent = control;
            onParentChanged();
        }
    }
    
    FCRect FCView::getRegion(){
        return m_region;
    }
    
    void FCView::setRegion(const FCRect& region){
        m_useRegion = true;
        m_region = region;
        onRegionChanged();
    }
    
    String FCView::getResourcePath(){
        return m_resourcePath;
    }
    
    void FCView::setResourcePath(const String& resourcePath){
        m_resourcePath = resourcePath;
    }
    
    int FCView::getRight(){
        return getLeft() + getWidth();
    }
    
    FCSize FCView::getSize(){
        if(m_percentSize){
            FCSize size ={getWidth(), getHeight()};
            return size;
        }
        else{
            return m_size;
        }
    }
    
    void FCView::setSize(const FCSize& size){
        FCSize newSize = size;
        if (newSize.cx > m_maximumSize.cx){
            newSize.cx = m_maximumSize.cx;
        }
        if (newSize.cy > m_maximumSize.cy){
            newSize.cy = m_maximumSize.cy;
        }
        if (newSize.cx < m_minimumSize.cx){
            newSize.cx = m_minimumSize.cx;
        }
        if (newSize.cy < m_minimumSize.cy){
            newSize.cy = m_minimumSize.cy;
        }
        if(newSize.cx != m_size.cx || newSize.cy != m_size.cy){
            if(m_percentSize){
                m_oldSize = getSize();
                if(m_percentSize->cx != -1){
                }
                else{
                    m_size.cx = newSize.cx;
                }
                if(m_percentSize->cy != -1){
                }
                else{
                    m_size.cy = newSize.cy;
                }
            }
            else{
                m_oldSize = m_size;
                m_size = size;
            }
            onSizeChanged();
            update();
        }
    }
    
    int FCView::getTabIndex(){
        return m_tabIndex;
    }
    
    void FCView::setTabIndex(int tabIndex){
        if(m_tabIndex != tabIndex){
            m_tabIndex = tabIndex;
            onTabIndexChanged();
        }
    }
    
    bool FCView::isTabStop(){
        return m_tabStop;
    }
    
    void FCView::setTabStop(bool tabStop){
        if(m_tabStop != tabStop){
            m_tabStop = tabStop;
            onTabStopChanged();
        }
    }
    
    Object FCView::getTag(){
        return m_tag;
    }
    
    void FCView::setTag(Object tag){
        m_tag = tag;
    }
    
    String FCView::getText(){
        return m_text;
    }
    
    void FCView::setText(const String& text){
        if(m_text != text){
            m_text = text;
            onTextChanged();
        }
    }
    
    Long FCView::getTextColor(){
        return m_textColor;
    }
    
    void FCView::setTextColor(Long textColor){
        if(m_textColor != textColor){
            m_textColor = textColor;
            onTextColorChanged();
        }
    }
    
    int FCView::getTop(){
        if(m_percentLocation && m_percentLocation->y != -1){
            FCSize parentSize = m_parent ? m_parent->getSize() : m_native->getDisplaySize();
            return (int)(parentSize.cy * m_percentLocation->y);
        }
        else{
            return m_location.y;
        }
    }
    
    void FCView::setTop(int top){
        if(m_percentLocation && m_percentLocation->y != -1){
            return;
        }
        else{
            FCPoint location ={m_location.x, top};
            setLocation(location);
        }
    }
    
    bool FCView::isTopMost(){
        return m_topMost;
    }
    
    void FCView::setTopMost(bool topMost){
        m_topMost = topMost;
    }
    
    bool FCView::useRegion(){
        return m_useRegion;
    }
    
    FCVerticalAlign FCView::getVerticalAlign(){
        return m_verticalAlign;
    }
    
    void FCView::setVerticalAlign(FCVerticalAlign verticalAlign){
        m_verticalAlign = verticalAlign;
    }
    
    bool FCView::isVisible(){
        return m_visible;
    }
    
    void FCView::setVisible(bool visible){
        if(m_visible != visible){
            m_visible = visible;
            onVisibleChanged();
        }
    }
    
    int FCView::getWidth(){
        if(m_percentSize && m_percentSize->cx != -1){
            FCSize parentSize = m_parent ? m_parent->getSize() : m_native->getDisplaySize();
            return (int)(parentSize.cx * m_percentSize->cx);
        }
        else{
            return m_size.cx;
        }
    }
    
    void FCView::setWidth(int width){
        if(m_percentSize && m_percentSize->cx != -1){
            return;
        }
        else{
            FCSize size ={width, m_size.cy};
            setSize(size);
        }
    }
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    void FCView::addControl(FCView *control){
        control->setParent(this);
        control->setNative(m_native);
        m_controls.add(control);
        control->onAdd();
    }
    
    void FCView::addEvent(Object func, int eventID, Object pInvoke){
        map<int, vector<Object>*>::iterator sIter = m_events.find(eventID);
        vector<Object> *events = 0;
        vector<Object> *invokes = 0;
        if(sIter != m_events.end()){
            events = sIter->second;
            invokes = m_invokes[eventID];
        }
        else{
            events = new vector<Object>;
            m_events[eventID] = events;
            invokes = new vector<Object>;
            m_invokes[eventID] = invokes;
        }
        events->push_back(func);
        invokes->push_back(pInvoke);
    }
    
    void FCView::beginInvoke(Object args){
        if (m_native){
            FCHost *host = m_native->getHost();
            host->beginInvoke(this, args);
        }
    }
    
    void FCView::bringChildToFront(FCView *childControl){
        if(m_controls.size() > 0){
            for(int c = 0; c < m_controls.size(); c++){
                if(m_controls.get(c) == childControl){
                    m_controls.removeAt(c);
                    break;
                }
            }
            m_controls.add(childControl);
        }
    }
    
    void FCView::bringToFront(){
        if(m_native){
            m_native->bringToFront(this);
        }
    }
    
    void FCView::clearControls(){
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
    
    bool FCView::containsControl(FCView *control){
        if(m_controls.size() > 0){
            for(int c = 0; c < m_controls.size(); c++){
                if(m_controls.get(c) == control){
                    return true;
                }
            }
        }
        return false;
    }
    
    bool FCView::containsPoint(const FCPoint& mp){
        FCPoint cPoint = pointToControl(mp);
        FCSize size = getSize();
        if (cPoint.x >= 0 && cPoint.x <= size.cx && cPoint.y >= 0 && cPoint.y <= size.cy){
            if (m_useRegion){
                if (cPoint.x >= m_region.left && cPoint.x <= m_region.right
                    && cPoint.y >= m_region.top && cPoint.y <= m_region.bottom){
                    return true;
                }
            }
            else{
                return true;
            }
        }
        return false;
    }
    
    void FCView::focus(){
        setFocused(true);
    }
    
    ArrayList<FCView*> FCView::getControls(){
        return m_controls;
    }
    
    String FCView::getControlType(){
        return L"FCView";
    }
    
    FCPoint FCView::getDisplayOffset(){
        FCPoint offset ={0, 0};
        return offset;
    }
    
    static int m_timerID = 10000;
    
    int FCView::getNewTimerID(){
        return m_timerID++;
    }
    
    FCView* FCView::getPopupMenuContext(FCView *control){
        if (m_hasPopupMenu){
            return this;
        }
        else{
            if (m_parent){
                return getPopupMenuContext(m_parent);
            }
            else{
                return 0;
            }
        }
    }
    
    void FCView::getProperty(const String& name, String *value, String *type){
        int len = (int)name.length();
        switch (len){
            case 2:{
                if(name == L"id"){
                    *type = L"text";
                    *value = getName();
                }
                break;
            }
            case 3:{
                if (name == L"top"){
                    *type = L"float";
                    if(m_percentLocation && m_percentLocation->y != -1){
                        *value = L"%" + FCStr::convertFloatToStr(100 * m_percentLocation->y);
                    }
                    else{
                        *value = FCStr::convertIntToStr(getTop());
                    }
                }
                break;
            }
            case 4:{
                if (name == L"dock"){
                    *type = L"enum:FCDockStyle";
                    *value = FCStr::convertDockToStr(getDock());
                }
                else if (name == L"font"){
                    *type = L"font";
                    *value = FCStr::convertFontToStr(getFont());
                }
                else if (name == L"left"){
                    *type = L"float";
                    if(m_percentLocation && m_percentLocation->x != -1){
                        *value = L"%" + FCStr::convertFloatToStr(100 * m_percentLocation->x);
                    }
                    else{
                        *value = FCStr::convertIntToStr(getLeft());
                    }
                }
                else if (name == L"name"){
                    *type = L"text";
                    *value = getName();
                }
                else if (name == L"size"){
                    *type = L"size";
                    if (m_percentSize){
                        String pWidth, pHeight, pType;
                        getProperty(L"width", &pWidth, &pType);
                        getProperty(L"height", &pHeight, &pType);
                        *value = pWidth + L"," + pHeight;
                    }
                    else{
                        *value = FCStr::convertSizeToStr(getSize());
                    }
                }
                else if (name == L"text"){
                    *type = L"text";
                    *value = getText();
                }
                break;
            }
            case 5:{
                if (name == L"align"){
                    *type = L"enum:FCHorizontalAlign";
                    *value = FCStr::convertHorizontalAlignToStr(getAlign());
                }
                else if(name == L"value"){
                    *type = L"text";
                    *value = getText();
                }
                else if (name == L"width"){
                    *type = L"float";
                    if(m_percentSize && m_percentSize->cx != -1){
                        *value = L"%" + FCStr::convertFloatToStr(100 * m_percentSize->cx);
                    }
                    else{
                        *value = FCStr::convertIntToStr(getWidth());
                    }
                }
                break;
            }
            case 6:{
                if (name == L"anchor"){
                    *type = L"anchor";
                    *value = FCStr::convertAnchorToStr(getAnchor());
                }
                else if (name == L"bounds"){
                    *type = L"rect";
                    *value = FCStr::convertRectToStr(getBounds());
                }
                else if (name == L"cursor"){
                    *type = L"enum:FCCursors";
                    *value = FCStr::convertCursorToStr(getCursor());
                }
                else if (name == L"height"){
                    *type = L"float";
                    if(m_percentSize && m_percentSize->cy != -1){
                        *value = L"%" + FCStr::convertFloatToStr(100 * m_percentSize->cy);
                    }
                    else{
                        *value = FCStr::convertIntToStr(getHeight());
                    }
                }
                else if (name == L"margin"){
                    *type = L"padding";
                    *value = FCStr::convertPaddingToStr(getMargin());
                }
                else if (name == L"region"){
                    *type = L"rect";
                    *value = FCStr::convertRectToStr(getRegion());
                }
                break;
            }
            case 7:{
                if (name == L"enabled"){
                    *type = L"bool";
                    *value = FCStr::convertBoolToStr(isEnabled());
                }
                else if (name == L"focused"){
                    *type = L"bool";
                    *value = FCStr::convertBoolToStr(isFocused());
                }
                else if (name == L"opacity"){
                    *type = L"float";
                    *value = FCStr::convertFloatToStr(getOpacity());
                }
                else if (name == L"padding"){
                    *type = L"padding";
                    *value = FCStr::convertPaddingToStr(getPadding());
                }
                else if (name == L"tabstop"){
                    *type = L"bool";
                    *value = FCStr::convertBoolToStr(isTabStop());
                }
                else if (name == L"topmost"){
                    *type = L"bool";
                    *value = FCStr::convertBoolToStr(isTopMost());
                }
                else if (name == L"visible"){
                    *type = L"bool";
                    *value = FCStr::convertBoolToStr(isVisible());
                }
                break;
            }
            case 8:{
                if (name == L"autosize"){
                    *type = L"bool";
                    *value = FCStr::convertBoolToStr(autoSize());
                }
                else if (name == L"canfocus"){
                    *type = L"bool";
                    *value = FCStr::convertBoolToStr(canFocus());
                }
                else if (name == L"iswindow"){
                    *type = L"bool";
                    *value = FCStr::convertBoolToStr(isWindow());
                }
                else if (name == L"location"){
                    *type = L"point";
                    if (m_percentLocation){
                        String pLeft, pTop, pType;
                        getProperty(L"left", &pLeft, &pType);
                        getProperty(L"top", &pTop, &pType);
                        *value = pLeft + L"," + pTop;
                    }
                    else{
                        *value = FCStr::convertPointToStr(getLocation());
                    }
                }
                else if (name == L"tabindex"){
                    *type = L"int";
                    *value = FCStr::convertIntToStr(getTabIndex());
                }
                break;
            }
            case 9:{
                if (name == L"allowdrag"){
                    *type = L"bool";
                    *value = FCStr::convertBoolToStr(allowDrag());
                }
                else if (name == L"backcolor"){
                    *type = L"color";
                    *value = FCStr::convertColorToStr(getBackColor());
                }
                else if (name == L"backimage"){
                    *type = L"text";
                    *value = getBackImage();
                }
                else if (name == L"textcolor"){
                    *type = L"color";
                    *value = FCStr::convertColorToStr(getTextColor());
                }
                break;
            }
            default:{
                if (name == L"allowpreviewsevent"){
                    *type = L"bool";
                    *value = FCStr::convertBoolToStr(allowPreviewsEvent());
                }
                else if (name == L"autoellipsis"){
                    *type = L"bool";
                    *value = FCStr::convertBoolToStr(autoEllipsis());
                }
                else if (name == L"bordercolor"){
                    *type = L"color";
                    *value = FCStr::convertColorToStr(getBorderColor());
                }
                else if (name == L"canraiseevents"){
                    *type = L"bool";
                    *value = FCStr::convertBoolToStr(canRaiseEvents());
                }
                else if(name == L"cornerradius"){
                    *type = L"int";
                    *value = FCStr::convertIntToStr(getCornerRadius());
                }
                else if (name == L"displayoffset"){
                    *type = L"bool";
                    *value = FCStr::convertBoolToStr(displayOffset());
                }
                else if (name == L"haspopupmenu"){
                    *type = L"bool";
                    *value = FCStr::convertBoolToStr(hasPopupMenu());
                }
                else if (name == L"maximumsize"){
                    *type = L"size";
                    *value = FCStr::convertSizeToStr(getMaximumSize());
                }
                else if (name == L"minimumsize"){
                    *type = L"size";
                    *value = FCStr::convertSizeToStr(getMinimumSize());
                }
                else if (name == L"resourcepath"){
                    *type = L"text";
                    *value = getResourcePath();
                }
                else if (name == L"vertical-align"){
                    *type = L"enum:FCVerticalAlign";
                    *value = FCStr::convertVerticalAlignToStr(getVerticalAlign());
                }
                break;
            }
        }
    }
    
    ArrayList<String> FCView::getPropertyNames(){
        ArrayList<String> propertyNames;
        propertyNames.add(L"Align");
        propertyNames.add(L"AllowDrag");
        propertyNames.add(L"AllowPreviewsEvent");
        propertyNames.add(L"Anchor");
        propertyNames.add(L"AutoEllipsis");
        propertyNames.add(L"AutoSize");
        propertyNames.add(L"BackColor");
        propertyNames.add(L"BackImage");
        propertyNames.add(L"BorderColor");
        propertyNames.add(L"Bounds");
        propertyNames.add(L"CanFocus");
        propertyNames.add(L"CanRaiseEvents");
        propertyNames.add(L"CornerRadius");
        propertyNames.add(L"Cursor");
        propertyNames.add(L"DisplayOffset");
        propertyNames.add(L"Dock");
        propertyNames.add(L"Enabled");
        propertyNames.add(L"Focused");
        propertyNames.add(L"Font");
        propertyNames.add(L"HasPopupMenu");
        propertyNames.add(L"Height");
        propertyNames.add(L"IsWindow");
        propertyNames.add(L"Left");
        propertyNames.add(L"Location");
        propertyNames.add(L"Margin");
        propertyNames.add(L"MaximumSize");
        propertyNames.add(L"MinimumSize");
        propertyNames.add(L"Name");
        propertyNames.add(L"Opacity");
        propertyNames.add(L"Padding");
        propertyNames.add(L"Region");
        propertyNames.add(L"ResourcePath");
        propertyNames.add(L"Size");
        propertyNames.add(L"TabIndex");
        propertyNames.add(L"TabStop");
        propertyNames.add(L"Text");
        propertyNames.add(L"TextColor");
        propertyNames.add(L"Top");
        propertyNames.add(L"TopMost");
        propertyNames.add(L"Value");
        propertyNames.add(L"Vertical-Align");
        propertyNames.add(L"Visible");
        propertyNames.add(L"Width");
        return propertyNames;
    }
    
    
    bool FCView::hasChildren(){
        return m_controls.size() > 0;
    }
    
    void FCView::hide(){
        setVisible(false);
    }
    
    void FCView::insertControl(int index, FCView *control){
        m_controls.insert(index, control);
    }
    
    void FCView::invalidate(){
        if(m_native){
            m_native->invalidate(this);
        }
    }
    
    void FCView::invoke(Object args){
        if (m_native){
            FCHost *host = m_native->getHost();
            host->invoke(this, args);
        }
    }
    
    bool FCView::isPaintEnabled(FCView *control){
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
    
    bool FCView::isPaintVisible(FCView *control){
        if (control->isEnabled()){
            FCView *parent = control->getParent();
            if (parent){
                return isPaintVisible(parent);
            }
            else{
                return true;
            }
        }
        else{
            return false;
        }
    }
    
    void FCView::onAdd(){
        callEvents(FCEventID::ADD);
    }
    
    void FCView::onAutoSizeChanged(){
        callEvents(FCEventID::AUTOSIZECHANGED);
    }
    
    void FCView::onBackColorChanged(){
        callEvents(FCEventID::BACKCOLORCHANGED);
    }
    
    void FCView::onBackImageChanged(){
        callEvents(FCEventID::BACKIMAGECHANGED);
    }
    
    void FCView::onChar(wchar_t ch){
    }
    
    void FCView::onClick(FCTouchInfo touchInfo){
        callTouchEvents(FCEventID::CLICK, touchInfo);
    }
    
    void FCView::onCopy(){
        callEvents(FCEventID::COPY);
    }
    
    void FCView::onCut(){
        callEvents(FCEventID::CUT);
    }
    
    void FCView::onDockChanged(){
        callEvents(FCEventID::DOCKCHANGED);
    }
    
    void FCView::onDoubleClick(FCTouchInfo touchInfo){
        callTouchEvents(FCEventID::DOUBLECLICK, touchInfo);
    }
    
    bool FCView::onDragBegin(){
        m_isDragging = true;
        callEvents(FCEventID::DRAGBEGIN);
        return true;
    }
    
    void FCView::onDragEnd(){
        m_isDragging = false;
        callEvents(FCEventID::DRAGEND);
    }
    
    void FCView::onDragging(){
        m_isDragging = true;
        callEvents(FCEventID::DRAGGING);
    }
    
    void FCView::onDragReady(FCPoint *startOffset){
        startOffset->x = 5;
        startOffset->y = 5;
    }
    
    void FCView::onEnableChanged(){
        callEvents(FCEventID::ENABLECHANGED);
    }
    
    void FCView::onFontChanged(){
        callEvents(FCEventID::FONTCHANGED);
    }
    
    void FCView::onGotFocus(){
        callEvents(FCEventID::GOTFOCUS);
    }
    
    void FCView::onInvoke(Object args){
        callInvokeEvents(FCEventID::INVOKE, args);
    }
    
    void FCView::onLoad(){
        callEvents(FCEventID::LOAD);
    }
    
    void FCView::onLocationChanged(){
        callEvents(FCEventID::LOCATIONCHANGED);
    }
    
    void FCView::onLostFocus(){
        callEvents(FCEventID::LOSTFOCUS);
    }
    
    void FCView::onKeyDown(char key){
        callKeyEvents(FCEventID::KEYDOWN, key);
    }
    
    void FCView::onKeyUp(char key){
        callKeyEvents(FCEventID::KEYUP, key);
    }
    
    void FCView::onMarginChanged(){
        callEvents(FCEventID::MARGINCHANGED);
    }
    
    void FCView::onTouchDown(FCTouchInfo touchInfo){
        callTouchEvents(FCEventID::TOUCHDOWN, touchInfo);
    }
    
    void FCView::onTouchEnter(FCTouchInfo touchInfo){
        callTouchEvents(FCEventID::TOUCHENTER, touchInfo);
        if (m_autoEllipsis){
            if ((int)m_text.length() > 0){
                m_native->getHost()->showToolTip(m_text, m_native->getTouchPoint());
            }
        }
    }
    
    void FCView::onTouchLeave(FCTouchInfo touchInfo){
        callTouchEvents(FCEventID::TOUCHLEAVE, touchInfo);
    }
    
    void FCView::onTouchMove(FCTouchInfo touchInfo){
        callTouchEvents(FCEventID::TOUCHMOVE, touchInfo);
    }
    
    void FCView::onTouchUp(FCTouchInfo touchInfo){
        callTouchEvents(FCEventID::TOUCHUP, touchInfo);
    }
    
    void FCView::onTouchWheel(FCTouchInfo touchInfo){
        callTouchEvents(FCEventID::TOUCHWHEEL, touchInfo);
    }
    
    void FCView::onPaddingChanged(){
        callEvents(FCEventID::PADDINGCHANGED);
    }
    
    void FCView::onPaint(FCPaint *paint, const FCRect& clipRect){
        onPaintBackground(paint, clipRect);
        onPaintForeground(paint, clipRect);
        callPaintEvents(FCEventID::PAINT, paint, clipRect);
    }
    
    void FCView::onPaintBackground(FCPaint *paint, const FCRect& clipRect){
        FCRect rect ={0, 0, getWidth(), getHeight()};
        paint->fillRoundRect(getPaintingBackColor(), rect, m_cornerRadius);
        String bkImage = getPaintingBackImage();
        if (bkImage.length() > 0){
            paint->drawImage(bkImage.c_str(), rect);
        }
    }
    
    void FCView::onPaintBorder(FCPaint *paint, const FCRect& clipRect){
        FCRect borderRect ={0, 0, getWidth(), getHeight()};
        paint->drawRoundRect(getPaintingBorderColor(), 1, 0, borderRect, m_cornerRadius);
        callPaintEvents(FCEventID::PAINTBORDER, paint, clipRect);
    }
    
    void FCView::onPaintForeground(FCPaint *paint, const FCRect& clipRect){
    }
    
    void FCView::onParentChanged(){
        callEvents(FCEventID::PARENTCHANGED);
    }
    
    void FCView::onPaste(){
        callEvents(FCEventID::PASTE);
    }
    
    void FCView::onPrePaint(FCPaint *paint, const FCRect& clipRect){
    }
    
    bool FCView::onPreviewsKeyEvent(int eventID, char key){
        return callPreviewsKeyEvent(FCEventID::PREVIEWSKEYEVENT, eventID, key);
    }
    
    bool FCView::onPreviewsTouchEvent(int eventID, FCTouchInfo touchInfo){
        return callPreviewsTouchEvent(FCEventID::PREVIEWSTOUCHEVENT, eventID, touchInfo);
    }
    
    void FCView::onRegionChanged(){
        callEvents(FCEventID::REGIONCHANGED);
    }
    
    void FCView::onRemove(){
        callEvents(FCEventID::REMOVE);
    }
    
    void FCView::onSizeChanged(){
        callEvents(FCEventID::SIZECHANGED);
        update();
    }
    
    void FCView::onTabIndexChanged(){
        callEvents(FCEventID::TABINDEXCHANGED);
    }
    
    void FCView::onTabStop(){
        callEvents(FCEventID::TABSTOP);
    }
    
    void FCView::onTabStopChanged(){
        callEvents(FCEventID::TABSTOPCHANGED);
    }
    
    void FCView::onTextChanged(){
        callEvents(FCEventID::TEXTCHANGED);
    }
    
    void FCView::onTextColorChanged(){
        callEvents(FCEventID::TEXTCOLORCHANGED);
    }
    
    void FCView::onTimer(int timerID){
        callTimerEvents(FCEventID::TIMER, timerID);
    }
    
    void FCView::onVisibleChanged(){
        callEvents(FCEventID::VISIBLECHANGED);
    }
    
    FCPoint FCView::pointToControl(const FCPoint& mp){
        if (m_native){
            int clientX = m_native->clientX(this);
            int clientY = m_native->clientY(this);
            FCPoint point ={mp.x - clientX, mp.y - clientY};
            return point;
        }
        else{
            return mp;
        }
    }
    
    FCPoint FCView::pointToNative(const FCPoint& mp){
        if (m_native){
            int clientX = m_native->clientX(this);
            int clientY = m_native->clientY(this);
            FCPoint point ={mp.x + clientX, mp.y + clientY};
            return point;
        }
        else{
            return mp;
        }
    }
    
    void FCView::removeControl(FCView *control){
        if(m_native){
            m_native->removeControl(control);
        }
        for(int c = 0; c < m_controls.size(); c++){
            FCView *baseControl = m_controls.get(c);
            if(baseControl == control){
                baseControl->onRemove();
                m_controls.removeAt(c);
                control->setParent(0);
                return;
            }
        }
    }
    
    void FCView::removeEvent(Object func, int eventID){
        map<int, vector<Object>*>::iterator sIter = m_events.find(eventID);
        if(sIter != m_events.end()){
            map<int, vector<Object>*>::iterator sIter2 = m_invokes.find(eventID);
            vector<Object> *events = sIter->second;
            int eventSize = (int)events->size();
            int removeIndex = -1;
            for(int i = 0; i < eventSize; i++){
                if((*events)[i] == func){
                    removeIndex = i;
                    break;
                }
            }
            if(removeIndex >= 0){
                vector<Object> *invokes = sIter2->second;
                vector<Object>::iterator sIter3 = events->begin();
                int pos = 0;
                for(; sIter3 != events->end(); ++sIter3){
                    if(pos == removeIndex){
                        events->erase(sIter3);
                        break;
                    }
                    pos++;
                }
                pos = 0;
                vector<Object>::iterator sIter4 = invokes->begin();
                for(; sIter4 != invokes->end(); ++sIter4){
                    if(pos == removeIndex){
                        invokes->erase(sIter4);
                        break;
                    }
                    pos++;
                }
            }
        }
    }
    
    void FCView::sendChildToBack(FCView *childControl){
        if(m_controls.size() > 0){
            for(int c = 0; c < m_controls.size(); c++){
                if(m_controls.get(c) ==  childControl){
                    m_controls.removeAt(c);
                    break;
                }
            }
            m_controls.insert(0, childControl);
        }
    }
    
    void FCView::setProperty(const String& name, const String& value){
        int len = (int)name.length();
        switch (len){
            case 2:{
                if(name == L"id"){
                    setName(value);
                }
            }
            case 3:{
                if (name == L"top"){
                    if(value.find(L"%") != -1){
                        float percentValue = FCStr::convertStrToFloat(FCStr::replace(value, L"%", L"")) / 100;
                        if(!m_percentLocation){
                            m_percentLocation = new FCPointF();
                            m_percentLocation->x = -1;
                        }
                        m_percentLocation->y = percentValue;
                    }
                    else{
                        if(m_percentLocation){
                            m_percentLocation->y = -1;
                        }
                        setTop(FCStr::convertStrToInt(value));
                    }
                }
                break;
            }
            case 4:{
                if (name == L"dock"){
                    setDock(FCStr::convertStrToDock(value));
                }
                else if (name == L"font"){
                    setFont(FCStr::convertStrToFont(value));
                }
                else if (name == L"left"){
                    if(value.find(L"%") != -1){
                        float percentValue = FCStr::convertStrToFloat(FCStr::replace(value, L"%", L"")) / 100;
                        if(!m_percentLocation){
                            m_percentLocation = new FCPointF();
                            m_percentLocation->y = -1;
                        }
                        m_percentLocation->x = percentValue;
                    }
                    else{
                        if(m_percentLocation){
                            m_percentLocation->x = -1;
                        }
                        setLeft(FCStr::convertStrToInt(value));
                    }
                }
                else if (name == L"name"){
                    setName(value);
                }
                else if (name == L"size"){
                    setSize(FCStr::convertStrToSize(value));
                }
                else if (name == L"text"){
                    setText(value);
                }
                break;
            }
            case 5:{
                if (name == L"align"){
                    setAlign(FCStr::convertStrToHorizontalAlign(value));
                }
                else if(name == L"value"){
                    setText(value);
                }
                else if (name == L"width"){
                    if(value.find(L"%") != -1){
                        float percentValue = FCStr::convertStrToFloat(FCStr::replace(value, L"%", L"")) / 100;
                        if(!m_percentSize){
                            m_percentSize = new FCSizeF();
                            m_percentSize->cy = -1;
                        }
                        m_percentSize->cx = percentValue;
                    }
                    else{
                        if(m_percentSize){
                            m_percentSize->cx = -1;
                        }
                        setWidth(FCStr::convertStrToInt(value));
                    }
                }
                break;
            }
            case 6:{
                if (name == L"anchor"){
                    setAnchor(FCStr::convertStrToAnchor(value));
                }
                else if (name == L"bounds"){
                    setBounds(FCStr::convertStrToRect(value));
                }
                else if (name == L"cursor"){
                    setCursor(FCStr::convertStrToCursor(value));
                }
                else if (name == L"height"){
                    if(value.find(L"%") != -1){
                        float percentValue = FCStr::convertStrToFloat(FCStr::replace(value, L"%", L"")) / 100;
                        if(!m_percentSize){
                            m_percentSize = new FCSizeF();
                            m_percentSize->cx = -1;
                        }
                        m_percentSize->cy = percentValue;
                    }
                    else{
                        if(m_percentSize){
                            m_percentSize->cy = -1;
                        }
                        setHeight(FCStr::convertStrToInt(value));
                    }
                }
                else if (name == L"margin"){
                    setMargin(FCStr::convertStrToPadding(value));
                }
                else if (name == L"region"){
                    setRegion(FCStr::convertStrToRect(value));
                }
                break;
            }
            case 7:{
                if (name == L"enabled"){
                    setEnabled(FCStr::convertStrToBool(value));
                }
                else if (name == L"focused"){
                    setFocused(FCStr::convertStrToBool(value));
                }
                else if (name == L"opacity"){
                    setOpacity(FCStr::convertStrToFloat(value));
                }
                else if (name == L"padding"){
                    setPadding(FCStr::convertStrToPadding(value));
                }
                else if (name == L"tabstop"){
                    setTabStop(FCStr::convertStrToBool(value));
                }
                else if (name == L"topmost"){
                    setTopMost(FCStr::convertStrToBool(value));
                }
                else if (name == L"visible"){
                    setVisible(FCStr::convertStrToBool(value));
                }
                break;
            }
            case 8:{
                if (name == L"autosize"){
                    setAutoSize(FCStr::convertStrToBool(value));
                }
                else if (name == L"canfocus"){
                    setCanFocus(FCStr::convertStrToBool(value));
                }
                else if (name == L"iswindow"){
                    setWindow(FCStr::convertStrToBool(value));
                }
                else if (name == L"location"){
                    setLocation(FCStr::convertStrToPoint(value));
                }
                else if (name == L"tabindex"){
                    setTabIndex(FCStr::convertStrToInt(value));
                }
                break;
            }
            case 9:{
                if (name == L"allowdrag"){
                    setAllowDrag(FCStr::convertStrToBool(value));
                }
                else if (name == L"backcolor"){
                    setBackColor(FCStr::convertStrToColor(value));
                }
                else if (name == L"backimage"){
                    setBackImage(value);
                }
                else if (name == L"textcolor"){
                    setTextColor(FCStr::convertStrToColor(value));
                }
                break;
            }
            default:{
                if (name == L"allowpreviewsevent"){
                    setAllowPreviewsEvent(FCStr::convertStrToBool(value));
                }
                else if (name == L"autoellipsis"){
                    setAutoEllipsis(FCStr::convertStrToBool(value));
                }
                else if (name == L"bordercolor"){
                    setBorderColor(FCStr::convertStrToColor(value));
                }
                else if (name == L"canraiseevents"){
                    setCanRaiseEvents(FCStr::convertStrToBool(value));
                }
                else if(name == L"cornerradius"){
                    setCornerRadius(FCStr::convertStrToInt(value));
                }
                else if (name == L"displayoffset"){
                    setDisplayOffset(FCStr::convertStrToBool(value));
                }
                else if (name == L"haspopupmenu"){
                    setHasPopupMenu(FCStr::convertStrToBool(value));
                }
                else if (name == L"maximumsize"){
                    setMaximumSize(FCStr::convertStrToSize(value));
                }
                else if (name == L"minimumsize"){
                    setMinimumSize(FCStr::convertStrToSize(value));
                }
                else if (name == L"resourcepath"){
                    setResourcePath(value);
                }
                else if (name == L"vertical-align"){
                    setVerticalAlign(FCStr::convertStrToVerticalAlign(value));
                }
                break;
            }
        }
    }
    
    void FCView::sendToBack(){
        if(m_native){
            m_native->sendToBack(this);
        }
    }
    
    void FCView::show(){
        setVisible(true);
    }
    
    void FCView::startTimer(int timerID, int interval){
        if(m_native){
            m_native->startTimer(this, timerID, interval);
        }
    }
    
    void FCView::stopTimer(int timerID){
        if(m_native){
            m_native->stopTimer(timerID);
        }
    }
    
    void FCView::update(){
        if(m_native){
            m_native->setAlign(&m_controls);
            if (m_oldSize.cx > 0 && m_oldSize.cy > 0){
                m_native->setAnchor(&m_controls, m_oldSize);
            }
            m_native->setDock(&m_controls);
            m_oldLocation = getLocation();
            m_oldSize = getSize();
            int controlsSize = (int)m_controls.size();
            for (int i = 0; i < controlsSize; i++){
                m_controls.get(i)->update();
            }
        }
    }
}
