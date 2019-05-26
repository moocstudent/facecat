#include "stdafx.h"
#include "FCTabControl.h"

namespace FaceCat{
    void FCTabControl::drawMoving(){
        if (m_animationState > 0){
            bool drawing = false;
            FCTabPage *selectedTabPage = getSelectedTabPage();
            for(int i = 0; i < m_tabPages.size(); i++){
                FCTabPage *tabPage = m_tabPages.get(i);
                if (tabPage == selectedTabPage && m_animationState == 1){
                    continue;
                }
                FCButton *headerButton =  tabPage->getHeaderButton();
                if(headerButton->isVisible()){
                    int moving = headerButton->getLeft();
                    int pos = tabPage->getHeaderLocation().x;
                    if (m_layout == TabPageLayout_Left || m_layout == TabPageLayout_Right){
                        pos = tabPage->getHeaderLocation().y;
                        moving = headerButton->getTop();
                    }
                    if (pos != moving){
                        int relative = moving;
                        int sub = abs(pos - relative);
                        int step = 20;
                        if(m_useAnimation){
                            if (tabPage == selectedTabPage){
                                if (sub > 200){
                                    step = sub / 200 * 100;
                                }
                            }
                            else{
                                step = sub;
                            }
                        }
                        else{
                            step = sub;
                        }
                        if (relative != pos){
                            if (pos > relative + step) relative += step;
                            else if (pos < relative - step) relative -= step;
                            else relative = pos;
                            if (m_layout == TabPageLayout_Left || m_layout == TabPageLayout_Right){
                                headerButton->setTop(relative);
                            }
                            else{
                                headerButton->setLeft(relative);
                            }
                            drawing = true;
                        }
                    }
                }
            }
            if (!drawing){
                if (m_animationState == 2){
                    m_animationState = 0;
                }
            }
            update();
            invalidate();
        }
    }
    
    int FCTabControl::getTabPageIndex(FCTabPage *tabPage){
        int i = 0;
        for(int i = 0; i < m_tabPages.size(); i++){
            if(tabPage == m_tabPages.get(i)){
                return i;
            }
        }
        return -1;
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    FCTabControl::FCTabControl(){
        m_animationState = 0;
        m_layout = TabPageLayout_Top;
        m_selectedIndex = -1;
        m_timerID = FCView::getNewTimerID();
        m_useAnimation = false;
    }
    
    FCTabControl::~FCTabControl(){
        stopTimer(m_timerID);
    }
    
    TabPageLayout FCTabControl::getLayout(){
        return m_layout;
    }
    
    void FCTabControl::setLayout(TabPageLayout layout){
        m_layout = layout;
    }
    
    int FCTabControl::getSelectedIndex(){
        return m_selectedIndex;
    }
    
    void FCTabControl::setSelectedIndex(int selectedIndex){
        int tabPageSize = (int)m_tabPages.size();
        if(tabPageSize > 0){
            if(selectedIndex >= 0 && selectedIndex < tabPageSize){
                m_selectedIndex = selectedIndex;
                setSelectedTabPage(m_tabPages.get(m_selectedIndex));
            }
        }
    }
    
    FCTabPage* FCTabControl::getSelectedTabPage(){
        int tabPageSize = (int)m_tabPages.size();
        if(tabPageSize > 0){
            if(m_selectedIndex >= 0 && m_selectedIndex < tabPageSize){
                return m_tabPages.get(m_selectedIndex);
            }
        }
        return 0;
    }
    
    void FCTabControl::setSelectedTabPage(FCTabPage *selectedTabPage){
        int index = -1;
        int i = 0;
        int tabPageSize = (int)m_tabPages.size();
        if(selectedTabPage && tabPageSize > 0){
            FCTabPage* oldSelectedTabPage = getSelectedTabPage();
            for(int i = 0; i < m_tabPages.size(); i++){
                FCTabPage *tabPage = m_tabPages.get(i);
                if (tabPage == selectedTabPage){
                    index = i;
                    tabPage->setVisible(true);
                }
                else{
                    tabPage->setVisible(false);
                }
            }
            if (index != -1){
                m_selectedIndex = index;
            }
            else{
                if (m_tabPages.size() > 0){
                    m_selectedIndex = 0;
                }
            }
            FCTabPage* newSelectedTabPage = getSelectedTabPage();
            if(oldSelectedTabPage != newSelectedTabPage){
                onSelectedTabPageChanged();
            }
            newSelectedTabPage->bringToFront();
            newSelectedTabPage->getHeaderButton()->bringToFront();
        }
        else{
            m_selectedIndex = -1;
        }
    }
    
    bool FCTabControl::useAnimation(){
        return m_useAnimation;
    }
    
    void FCTabControl::setUseAnimation(bool useAnimation){
        m_useAnimation = useAnimation;
        if(m_useAnimation){
            startTimer(m_timerID, 20);
        }
        else{
            stopTimer(m_timerID);
        }
    }
    
    ///////////////////////////////////////////////////////////////////////////////////////////////
    
    void FCTabControl::addControl(FCView *control){
        FCTabPage *tabPage = dynamic_cast<FCTabPage*>(control);
        if(tabPage){
            tabPage->setTabControl(this);
        }
        FCDiv::addControl(control);
        if(tabPage){
            m_tabPages.add(tabPage);
            setSelectedTabPage(tabPage);
        }
    }
    
    String FCTabControl::getControlType(){
        return L"TabControl";
    }
    
    void FCTabControl::clearControls(){
        m_tabPages.clear();
        m_selectedIndex = -1;
        FCDiv::clearControls();
    }
    
    void FCTabControl::getProperty(const String& name, String *value, String *type){
        if(name == L"layout"){
            *type = L"enum:FCTabPageLayout";
            TabPageLayout layout = getLayout();
            if(layout == TabPageLayout_Left){
                *value = L"Left";
            }
            else if(layout == TabPageLayout_Right){
                *value = L"Right";
            }
            else if(layout == TabPageLayout_Bottom){
                *value = L"Bottom";
            }
            else{
                *value = L"Top";
            }
        }
        else if(name == L"selectedindex"){
            *type = L"int";
            *value = FCStr::convertIntToStr(getSelectedIndex());
        }
        else if(name == L"useanimation"){
            *type = L"bool";
            *value = FCStr::convertBoolToStr(useAnimation());
        }
        else{
            FCDiv::getProperty(name, value, type);
        }
    }
    
    ArrayList<String> FCTabControl::getPropertyNames(){
        ArrayList<String> propertyNames = FCDiv::getPropertyNames();
        propertyNames.add(L"Layout");
        propertyNames.add(L"SelectedIndex");
        propertyNames.add(L"UseAnimation");
        return propertyNames;
    }
    
    void FCTabControl::insertControl(int index, FCView *control){
        FCTabPage *tabPage = dynamic_cast<FCTabPage*>(control);
        if(tabPage){
            tabPage->setTabControl(this);
        }
        FCDiv::addControl(control);
        if(tabPage){
            m_tabPages.insert(index, tabPage);
            setSelectedTabPage(tabPage);
        }
    }
    
    void FCTabControl::onDragTabHeaderBegin(FCTabPage *tabPage){
        m_animationState = 1;
        tabPage->getHeaderButton()->bringToFront();
    }
    
    void FCTabControl::onDragTabHeaderEnd(FCTabPage *tabPage){
        if (m_animationState == 1){
            m_animationState = 2;
            drawMoving();
        }
        else{
            update();
            invalidate();
        }
    }
    
    void FCTabControl::onDraggingTabHeader(FCTabPage *tabPage){
        FCButton *headerButton = tabPage->getHeaderButton();
        int moving = headerButton->getLeft();
        if (m_layout == TabPageLayout_Left || m_layout == TabPageLayout_Right){
            moving = headerButton->getTop();
        }
        int tabPageSize = (int)m_tabPages.size();
        for (int i = 0; i < tabPageSize; i++){
            FCTabPage *page = m_tabPages.get(i);
            if (page != tabPage){
                FCButton *tpHeader = page->getHeaderButton();
                if(tpHeader->isVisible()){
                    int pos = page->getHeaderLocation().x;
                    int size = tpHeader->getWidth();
                    int sSize = headerButton->getWidth();
                    if (m_layout == TabPageLayout_Left || m_layout == TabPageLayout_Right){
                        pos = page->getHeaderLocation().y;
                        size = tpHeader->getHeight();
                        sSize = headerButton->getHeight();
                    }
                    bool instead = false;
                    if (moving > pos){
                        if (moving > pos
                            && moving < pos + size / 2){
                            instead = true;
                        }
                    }
                    if (moving < pos){
                        if (moving + sSize > pos + size / 2
                            && moving + sSize < pos + size){
                            instead = true;
                        }
                    }
                    if (instead){
                        FCPoint sLocation = tabPage->getHeaderLocation();
                        if (m_layout == TabPageLayout_Left || m_layout == TabPageLayout_Right){
                            FCPoint tabPageLocation ={tabPage->getHeaderLocation().x, pos};
                            tabPage->setHeaderLocation(tabPageLocation);
                            FCPoint pageLocation ={page->getHeaderLocation().x, sLocation.y};
                            page->setHeaderLocation(pageLocation);
                        }
                        else{
                            FCPoint tabPageLocation ={pos, tabPage->getHeaderLocation().y};
                            tabPage->setHeaderLocation(tabPageLocation);
                            FCPoint pageLocation ={sLocation.x, page->getHeaderLocation().y};
                            page->setHeaderLocation(pageLocation);
                        }
                        int oIndex = getTabPageIndex(tabPage);
                        int nIndex = getTabPageIndex(page);
                        m_tabPages.set(oIndex, page);
                        m_tabPages.set(nIndex, tabPage);
                        m_selectedIndex = nIndex;
                        break;
                    }
                }
            }
        }
        drawMoving();
    }
    
    void FCTabControl::onLoad(){
        FCDiv::onLoad();
        if(m_useAnimation){
            startTimer(m_timerID, 20);
        }
        else{
            stopTimer(m_timerID);
        }
    }
    
    void FCTabControl::onSelectedTabPageChanged(){
        callEvents(FCEventID::SELECTEDTABPAGECHANGED);
    }
    
    void FCTabControl::onTimer(int timerID){
        FCDiv::onTimer(timerID);
        if(m_timerID == timerID){
            drawMoving();
        }
    }
    
    void FCTabControl::removeControl(FCView *control){
        FCTabPage *tabPage = dynamic_cast<FCTabPage*>(control);
        if(tabPage){
            int tabPageSize = (int)m_tabPages.size();
            if(tabPageSize > 0){
                FCTabPage *selectedPage = m_tabPages.get(m_selectedIndex);
                if (selectedPage == tabPage){
                    if (m_selectedIndex > 0){
                        if (m_selectedIndex < tabPageSize - 1){
                            selectedPage = m_tabPages.get(m_selectedIndex + 1);
                        }
                        else{
                            selectedPage = m_tabPages.get(m_selectedIndex - 1);
                        }
                    }
                    else{
                        if (tabPageSize > 1){
                            selectedPage = m_tabPages.get(m_selectedIndex + 1);
                        }
                    }
                }
                for(int i = 0; i < m_tabPages.size(); i++){
                    if(tabPage == m_tabPages.get(i)){
                        m_tabPages.removeAt(i);
                        break;
                    }
                }
                FCDiv::removeControl(tabPage->getHeaderButton());
                FCDiv::removeControl(tabPage);
                setSelectedTabPage(selectedPage);
            }
        }
        else{
            FCDiv::removeControl(control);
        }
    }
    
    void FCTabControl::setProperty(const String& name, const String& value){
        if(name == L"layout"){
            String lowerStr = FCStr::toLower(value);
            if(lowerStr == L"left"){
                setLayout(TabPageLayout_Left);
            }
            else if(lowerStr == L"top"){
                setLayout(TabPageLayout_Top);
            }
            else if(lowerStr == L"right"){
                setLayout(TabPageLayout_Right);
            }
            else if(lowerStr == L"bottom"){
                setLayout(TabPageLayout_Bottom);
            }
        }
        else if(name == L"selectedindex"){
            setSelectedIndex(FCStr::convertStrToInt(value));
        }
        else if(name == L"useanimation"){
            setUseAnimation(FCStr::convertStrToBool(value));
        }
        else{
            FCDiv::setProperty(name, value);
        }
    }
    
    static void updataTabLayout(int layoutStyle, FCRect *bounds, FCPadding *padding,
                        FCPadding *margin, int left, int top, int width, int height,
                        int tw, int th, FCPoint *headerLocation){
        switch(layoutStyle){
            case 0:
                bounds->left = padding->left;
                bounds->top = padding->top;
                bounds->right = width;
                bounds->bottom = height - th;
                headerLocation->x = left;
                headerLocation->y = height - th;
                break;
            case 1:
                bounds->left = tw;
                bounds->top = padding->top;
                bounds->right = width;
                bounds->bottom = height;
                headerLocation->x = padding->left;
                headerLocation->y = top;
                break;
            case 2:
                bounds->left = padding->left;
                bounds->top = padding->top;
                bounds->right = width - tw;
                bounds->bottom = height;
                headerLocation->x = width - tw;
                headerLocation->y = top;
                break;
            case 3:
                bounds->left = padding->left;
                bounds->top = th;
                bounds->right = width;
                bounds->bottom = height;
                headerLocation->x = left;
                headerLocation->y = padding->top;
                break;
        }
    }
    
    void FCTabControl::update(){
        if(!getNative()){
            return;
        }
        FCDiv::update();
        FCPadding padding = getPadding();
        int left = padding.left, top = padding.top;
        int width = getWidth() - padding.left - padding.right;
        int height = getHeight() - padding.top - padding.bottom;
        for(int i = 0; i < m_tabPages.size(); i++){
            FCTabPage *tabPage = m_tabPages.get(i);
            FCButton *headerButton = tabPage->getHeaderButton();
            if(headerButton->isVisible()){
                FCPadding margin = headerButton->getMargin();
                int tw = headerButton->getWidth() + margin.left + margin.right;
                int th = headerButton->getHeight() + margin.top + margin.bottom;
                FCRect bounds ={0};
                FCPoint headerLocation ={0};
                int layout = 0;
                switch(m_layout){
                    case TabPageLayout_Bottom:
                        layout = 0;
                        break;
                    case TabPageLayout_Left:
                        layout = 1;
                        break;
                    case TabPageLayout_Right:
                        layout = 2;
                        break;
                    case TabPageLayout_Top:
                        layout = 3;
                        break;
                }
                updataTabLayout(layout, &bounds, &padding,
                               &margin, left, top, width, height,
                               tw, th, &headerLocation);
                tabPage->setBounds(bounds);
                tabPage->setHeaderLocation(headerLocation);
                if(!m_useAnimation){
                    tabPage->getHeaderButton()->setLocation(headerLocation);
                }
                
                if (m_animationState > 0){
                    if (m_layout == TabPageLayout_Left || m_layout == TabPageLayout_Right){
                        headerLocation.y = headerButton->getTop();
                    }
                    else if (m_layout == TabPageLayout_Bottom || m_layout == TabPageLayout_Top){
                        headerLocation.x = headerButton->getLeft();
                    }
                }
                headerButton->setLocation(headerLocation);
                left += tw;
                top += th;
            }
            else{
                tabPage->setVisible(false);
            }
        }
    }
}
