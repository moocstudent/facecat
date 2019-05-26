#include "stdafx.h"
#include "FCMenu.h"

namespace FaceCat{
    void FCMenu::adjust(FCMenu *menu){
        FCNative *native = getNative();
        if (autoSize()){
            int contentHeight = menu->getContentHeight();
            int maximumHeight = getMaximumSize().cy;
            menu->setHeight(min(contentHeight, maximumHeight));
        }
        FCPoint mPoint = menu->getLocation();
        FCSize mSize = menu->getSize();
        FCSize nSize = native->getDisplaySize();
        if (mPoint.x < 0){
            mPoint.x = 0;
        }
        if (mPoint.y < 0){
            mPoint.y = 0;
        }
        if (mPoint.x + mSize.cx > nSize.cx){
            mPoint.x = nSize.cx - mSize.cx;
        }
        if (mPoint.y + mSize.cy > nSize.cy){
            mPoint.y = nSize.cy - mSize.cy;
        }
        menu->setLocation(mPoint);
        menu->update();
    }
    
    bool FCMenu::checkDivFocused(ArrayList<FCMenuItem*> items){
        int itemSize = (int)items.size();
        for (int i = 0; i < itemSize; i++){
            FCMenuItem *item = items.get(i);
            FCMenu *dropDownMenu = item->getDropDownMenu();
            if (dropDownMenu){
                if (checkFocused(dropDownMenu)){
                    return true;
                }
            }
            ArrayList<FCMenuItem*> subItems = item->m_items;
            bool focused = checkDivFocused(subItems);
            if (focused){
                return true;
            }
        }
        return false;
    }
    
    bool FCMenu::checkFocused(FCView *control){
        if (control->isFocused()){
            return true;
        }
        else{
            ArrayList<FCView*> subControls = control->m_controls;
            for(int c = 0; c < subControls.size(); c++){
                FCView *subControl = subControls.get(c);
                bool focused = checkFocused(subControl);
                if(focused){
                    return true;
                }
            }
            return false;
        }
    }
    
    bool FCMenu::closeMenus(ArrayList<FCMenuItem*> items){
        int itemSize = (int)items.size();
        bool close = false;
        for (int i = 0; i < itemSize; i++){
            FCMenuItem *item = items.get(i);
            ArrayList<FCMenuItem*> subItems = item->m_items;
            if(closeMenus(subItems)){
                close = true;
            }
            FCMenu *dropDownMenu = item->getDropDownMenu();
            if (dropDownMenu && dropDownMenu->isVisible()){
                dropDownMenu->hide();
                close = true;
            }
        }
        return close;
    }
    
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    void FCMenu::callMenuItemTouchEvent(int eventID, FCMenuItem *item, FCTouchInfo touchInfo){
        if(m_events.size() > 0){
            map<int, vector<Object>*>::iterator sIter = m_events.find(eventID);
            if(sIter != m_events.end()){
                map<int, vector<Object>*>::iterator sIter2 = m_invokes.find(eventID);
                vector<Object> *events = sIter->second;
                vector<Object> *invokes = sIter2->second;
                int eventSize = (int)events->size();
                for(int i = 0; i < eventSize; i++){
                    FCMenuItemTouchEvent func = (FCMenuItemTouchEvent)(*events)[i];
                    func(this, item, touchInfo, (*invokes)[i]);
                }
            }
        }
    }
    
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    FCMenu::FCMenu(){
        m_autoHide = true;
        m_parentItem = 0;
        m_popup = false;
        m_timerID = getNewTimerID();
        setAutoSize(true);
        setLayoutStyle(FCLayoutStyle_TopToBottom);
        FCSize maximumSize ={2000, 500};
        setMaximumSize(maximumSize);
        setShowHScrollBar(true);
        setShowVScrollBar(true);
        setTopMost(true);
        FCSize size ={200, 200};
        setSize(size);
    }
    
    FCMenu::~FCMenu(){
        m_parentItem = 0;
        stopTimer(m_timerID);
        clearItems();
    }
    
    bool FCMenu::autoHide(){
        return m_autoHide;
    }
    
    void FCMenu::setAutoHide(bool autoHide){
        m_autoHide = autoHide;
    }
    
    FCMenuItem* FCMenu::getParentItem(){
        return m_parentItem;
    }
    
    void FCMenu::setParentItem(FCMenuItem *parentItem){
        m_parentItem = parentItem;
    }
    
    bool FCMenu::isPopup(){
        return m_popup;
    }
    
    void FCMenu::setPopup(bool popup){
        m_popup = popup;
        if(isVisible()){
            setVisible(false);
        }
    }
    
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    void FCMenu::addItem(FCMenuItem *item){
        item->setParentMenu(this);
        item->onAddingItem(-1);
        m_items.add(item);
    }
    
    void FCMenu::clearItems(){
        ArrayList<FCMenuItem*> itemsCopy;
        int itemSize = (int)m_items.size();
        for (int i = 0; i < itemSize; i++){
            itemsCopy.add(m_items.get(i));
        }
        int copySize = (int)itemsCopy.size();
        for (int i = 0; i < copySize; i++){
            itemsCopy.get(i)->onRemovingItem();
            delete itemsCopy.get(i);
        }
        m_items.clear();
    }
    
    FCMenu* FCMenu::createDropDownMenu(){
        FCMenu *menu = new FCMenu;
        menu->setPopup(true);
        menu->setShowHScrollBar(true);
        menu->setShowVScrollBar(true);
        return menu;
    }
    
    String FCMenu::getControlType(){
        return L"Menu";
    }
    
    ArrayList<FCMenuItem*> FCMenu::getItems(){
        return m_items;
    }
    
    void FCMenu::getProperty(const String& name, String *value, String *type){
        if(name == L"popup"){
            *type = L"bool";
            *value = FCStr::convertBoolToStr(isPopup());
        }
        else{
            FCLayoutDiv::getProperty(name, value, type);
        }
    }
    
    ArrayList<String> FCMenu::getPropertyNames(){
        ArrayList<String> propertyNames = FCLayoutDiv::getPropertyNames();
        propertyNames.add(L"Popup");
        return propertyNames;
    }
    
    void FCMenu::insertItem(int index, FCMenuItem *item){
        item->setParentMenu(this);
        item->onAddingItem(index);
        m_items.insert(index, item);
    }
    
    bool FCMenu::onAutoHide(){
        return true;
    }
    
    void FCMenu::onLoad(){
        FCLayoutDiv::onLoad();
    }
    
    void FCMenu::onMenuItemClick(FCMenuItem *item, FCTouchInfo touchInfo){
        if (item->m_items.size() == 0){
            callMenuItemTouchEvent(FCEventID::MENUITEMCLICK, item, touchInfo);
            bool close = closeMenus(m_items);
            if(m_popup){
                hide();
            }
            else{
                getNative()->invalidate();
            }
        }
        else{
            onMenuItemTouchMove(item, touchInfo);
        }
    }
    
    void FCMenu::onMenuItemTouchMove(FCMenuItem *item, FCTouchInfo touchInfo){
        FCNative *native = getNative();
        ArrayList<FCMenuItem*> items;
        FCMenuItem *parentItem = item->getParentItem();
        if (parentItem){
            items = parentItem->m_items;
        }
        else{
            items = m_items;
        }
        bool close = closeMenus(items);
        if (item->m_items.size() > 0){
            FCMenu *dropDownMenu = item->getDropDownMenu();
            if (dropDownMenu){
                dropDownMenu->setNative(native);
                FCLayoutStyle layoutStyle = getLayoutStyle();
                FCPoint location ={native->clientX(item) + item->getWidth(), native->clientY(item)};
                if (layoutStyle == FCLayoutStyle_LeftToRight || layoutStyle == FCLayoutStyle_RightToLeft){
                    location.x = native->clientX(item);
                    location.y = native->clientY(item) + item->getHeight();
                }
                dropDownMenu->setLocation(location);
                dropDownMenu->setOpacity(getOpacity());
                dropDownMenu->bringToFront();
                dropDownMenu->focus();
                dropDownMenu->show();
                adjust(dropDownMenu);
            }
        }
        getNative()->invalidate();
    }
    
    void FCMenu::onTouchDown(FCTouchInfo touchInfo){
        FCLayoutDiv::onTouchDown(touchInfo);
        bool close = closeMenus(m_items);
        getNative()->invalidate();
    }
    
    void FCMenu::onTimer(int timerID){
        FCLayoutDiv::onTimer(timerID);
        if (m_timerID == timerID){
            if (m_autoHide && !m_parentItem && isVisible()){
                if (!checkFocused(this) && !checkDivFocused(m_items) && onAutoHide()){
                    bool close = closeMenus(m_items);
                    if (m_popup){
                        hide();
                    }
                    else{
                        getNative()->invalidate();
                    }
                }
            }
        }
    }
    
    void FCMenu::onVisibleChanged(){
        FCLayoutDiv::onVisibleChanged();
        if (isVisible()){
            if(m_popup){
                FCHScrollBar *hScrollBar = getHScrollBar();
                FCVScrollBar *vScrollBar = getVScrollBar();
                if (hScrollBar){
                    hScrollBar->setPos(0);
                }
                if (vScrollBar){
                    vScrollBar->setPos(0);
                }
                focus();
                adjust(this);
            }
            startTimer(m_timerID, 10);
        }
        else{
            stopTimer(m_timerID);
            bool close = closeMenus(m_items);
            FCNative *native = getNative();
            if(native){
                native->invalidate();
            }
        }
    }
    
    void FCMenu::removeItem(FCMenuItem *item){
        item->onRemovingItem();
        for(int i = 0; i < m_items.size(); i++){
            if(item == m_items.get(i)){
                m_items.removeAt(i);
                return;
            }
        }
    }
    
    void FCMenu::setProperty(const String& name, const String& value){
        if(name == L"popup"){
            setPopup(FCStr::convertStrToBool(value));
        }
        else{
            FCLayoutDiv::setProperty(name, value);
        }
    }
}
