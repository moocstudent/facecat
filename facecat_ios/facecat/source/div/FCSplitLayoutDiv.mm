#include "stdafx.h"
#include "FCSplitLayoutDiv.h"

namespace FaceCat{
    void FCSplitLayoutDiv::splitterDragging(Object sender, Object pInvoke){
        FCSplitLayoutDiv *layout = (FCSplitLayoutDiv*)pInvoke;
        if(layout){
            layout->onSplitterDragging();
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    FCSplitLayoutDiv::FCSplitLayoutDiv(){
        m_firstControl = 0;
        m_oldSize.cx = 0;
        m_oldSize.cy = 0;
        m_secondControl = 0;
        m_splitMode = FCSizeType_AbsoluteSize;
        m_splitPercent = -1;
        m_splitter = 0;
        m_splitterDraggingEvent = splitterDragging;
    }
    
    FCSplitLayoutDiv::~FCSplitLayoutDiv(){
        m_firstControl = 0;
        m_secondControl = 0;
        if(m_splitterDraggingEvent){
            if(m_splitter){
                m_splitter->removeEvent((Object)m_splitterDraggingEvent, FCEventID::DRAGGING);
                m_splitterDraggingEvent = 0;
            }
        }
        m_splitter = 0;
    }
    
    FCView* FCSplitLayoutDiv::getFirstControl(){
        return m_firstControl;
    }
    
    void FCSplitLayoutDiv::setFirstControl(FCView *firstControl){
        m_firstControl = firstControl;
    }
    
    FCView* FCSplitLayoutDiv::getSecondControl(){
        return m_secondControl;
    }
    
    void FCSplitLayoutDiv::setSecondControl(FCView *secondControl){
        m_secondControl = secondControl;
    }
    
    FCSizeType FCSplitLayoutDiv::getSplitMode(){
        return m_splitMode;
    }
    
    void FCSplitLayoutDiv::setSplitMode(FCSizeType splitMode){
        m_splitMode = splitMode;
    }
    
    FCButton* FCSplitLayoutDiv::getSplitter(){
        return m_splitter;
    }
    
    ///////////////////////////////////////////////////////////////////////////////////////////////////////
    
    String FCSplitLayoutDiv::getControlType(){
        return L"SplitLayoutDiv";
    }
    
    void FCSplitLayoutDiv::getProperty(const String& name, String *value, String *type){
        if(name == L"candragsplitter"){
            *type = L"bool";
            if (m_splitter){
                *value = FCStr::convertBoolToStr(m_splitter->allowDrag());
            }
            else{
                *value = L"False";
            }
        }
        else if (name == L"splitmode"){
            *type = L"enum:FCSizeType";
            if (getSplitMode() == FCSizeType_AbsoluteSize){
                *value = L"AbsoluteSize";
            }
            else{
                *value = L"PercentSize";
            }
        }
        else if ((int)name.find(L"splitter-") != -1){
            if (m_splitter){
                m_splitter->getProperty(name.substr(9), value, type);
            }
        }
        else if (name == L"splitterposition"){
            *type = L"str";
            if (m_splitter) {
                if (m_layoutStyle == FCLayoutStyle_TopToBottom || m_layoutStyle == FCLayoutStyle_BottomToTop) {
                    *value = FCStr::convertIntToStr(m_splitter->getTop());
                    if (m_splitter->getHeight() > 0) {
                        *value = *value + L"," + FCStr::convertIntToStr(m_splitter->getHeight());
                    }
                } else {
                    *value = FCStr::convertIntToStr(m_splitter->getLeft());
                    if (m_splitter->getWidth() > 0) {
                        *value = *value + L"," + FCStr::convertIntToStr(m_splitter->getWidth());
                    }
                }
            }
        }
        else if (name == L"splittervisible"){
            *type = L"bool";
            if (m_splitter){
                *value = FCStr::convertBoolToStr(m_splitter->isVisible());
            }
            else{
                *value = L"False";
            }
        }
        else{
            FCLayoutDiv::getProperty(name, value, type);
        }
    }
    
    ArrayList<String> FCSplitLayoutDiv::getPropertyNames(){
        ArrayList<String> propertyNames = FCDiv::getPropertyNames();
        propertyNames.add(L"CanDragSplitter");
        propertyNames.add(L"SplitMode");
        propertyNames.add(L"Splitter");
        propertyNames.add(L"SplitterPosition");
        propertyNames.add(L"SplitterVisible");
        return propertyNames;
    }
    
    void FCSplitLayoutDiv::onSplitterDragging(){
        m_splitPercent = -1;
        update();
        invalidate();
    }
    
    void FCSplitLayoutDiv::onLoad(){
        FCLayoutDiv::onLoad();
        if (!m_splitter){
            FCHost *host = getNative()->getHost();
            m_splitter = dynamic_cast<FCButton*>(host->createInternalControl(this, L"splitter"));
            m_splitter->addEvent((Object)m_splitterDraggingEvent, FCEventID::DRAGGING, this);
            addControl(m_splitter);
        }
    }
    
    bool FCSplitLayoutDiv::onResetLayout(){
        bool reset = false;
        if(getNative() && m_splitter && m_firstControl && m_secondControl){
            FCRect splitRect ={0};
            int width = getWidth(), height = getHeight();
            FCRect fRect ={0};
            FCRect sRect ={0};
            FCSize splitterSize ={0};
            if(m_splitter->isVisible()){
                splitterSize.cx = m_splitter->getWidth();
                splitterSize.cy = m_splitter->getHeight();
            }
            FCLayoutStyle layoutStyle = getLayoutStyle();
            if(layoutStyle == FCLayoutStyle_BottomToTop){
                if (m_splitMode == FCSizeType_AbsoluteSize || m_oldSize.cy == 0){
                    splitRect.left = 0;
                    splitRect.top = height - (m_oldSize.cy - m_splitter->getTop());
                    splitRect.right = width;
                    splitRect.bottom = splitRect.top + splitterSize.cy;
                }
                else if (m_splitMode == FCSizeType_PercentSize){
                    splitRect.left = 0;
                    if(m_splitPercent == -1){
                        m_splitPercent = (float)m_splitter->getTop() / m_oldSize.cy;
                    }
                    splitRect.top = (int)height * m_splitPercent;
                    splitRect.right = width;
                    splitRect.bottom = splitRect.top + splitterSize.cy;
                }
                fRect.left = 0;
                fRect.top = splitRect.bottom;
                fRect.right = width;
                fRect.bottom = height;
                sRect.left = 0;
                sRect.top = 0;
                sRect.right = width;
                sRect.bottom = splitRect.top;
            }
            else if(layoutStyle == FCLayoutStyle_LeftToRight){
                if (m_splitMode == FCSizeType_AbsoluteSize || m_oldSize.cx == 0){
                    splitRect.left = m_splitter->getLeft();
                    splitRect.top = 0;
                    splitRect.right = splitRect.left + splitterSize.cx;
                    splitRect.bottom = height;
                }
                else if (m_splitMode == FCSizeType_PercentSize){
                    if(m_splitPercent == -1){
                        m_splitPercent = (float)m_splitter->getLeft() / m_oldSize.cx;
                    }
                    splitRect.left = (int)width * m_splitPercent;
                    splitRect.top = 0;
                    splitRect.right = splitRect.left + splitterSize.cx;
                    splitRect.bottom = height;
                }
                fRect.left = 0;
                fRect.top = 0;
                fRect.right = splitRect.left;
                fRect.bottom = height;
                sRect.left = splitRect.right;
                sRect.top = 0;
                sRect.right = width;
                sRect.bottom = height;
            }
            else if(layoutStyle == FCLayoutStyle_RightToLeft){
                if (m_splitMode == FCSizeType_AbsoluteSize || m_oldSize.cx == 0){
                    splitRect.left = width - (m_oldSize.cx - m_splitter->getLeft());
                    splitRect.top = 0;
                    splitRect.right = splitRect.left + splitterSize.cx;
                    splitRect.bottom = height;
                }
                else if (m_splitMode == FCSizeType_PercentSize){
                    if(m_splitPercent == -1){
                        m_splitPercent = (float)m_splitter->getLeft() / m_oldSize.cx;
                    }
                    splitRect.left = (int)width * m_splitPercent;
                    splitRect.top = 0;
                    splitRect.right = splitRect.left + splitterSize.cx;
                    splitRect.bottom = height;
                }
                fRect.left = splitRect.right;
                fRect.top = 0;
                fRect.right = width;
                fRect.bottom = height;
                sRect.left = 0;
                sRect.top = 0;
                sRect.right = splitRect.left;
                sRect.bottom = height;
            }
            else if(layoutStyle == FCLayoutStyle_TopToBottom){
                if (m_splitMode == FCSizeType_AbsoluteSize || m_oldSize.cy == 0){
                    splitRect.left = 0;
                    splitRect.top = m_splitter->getTop();
                    splitRect.right = width;
                    splitRect.bottom = splitRect.top + splitterSize.cy;
                }
                else if (m_splitMode == FCSizeType_PercentSize){
                    splitRect.left = 0;
                    if(m_splitPercent == -1){
                        m_splitPercent = (float)m_splitter->getTop() / m_oldSize.cy;
                    }
                    splitRect.top = (int)height * m_splitPercent;
                    splitRect.right = width;
                    splitRect.bottom = splitRect.top + splitterSize.cy;
                }
                fRect.left = 0;
                fRect.top = 0;
                fRect.right = width;
                fRect.bottom = splitRect.top;
                sRect.left = 0;
                sRect.top = splitRect.bottom;
                sRect.right = width;
                sRect.bottom = height;
            }
            if(m_splitter->isVisible()){
                FCRect spRect = m_splitter->getBounds();
                if (spRect.left != splitRect.left || spRect.top != splitRect.top || spRect.right != splitRect.right || spRect.bottom != splitRect.bottom){
                    m_splitter->setBounds(splitRect);
                    reset = true;
                }
                if(m_splitter->allowDrag()){
                    if(layoutStyle == FCLayoutStyle_LeftToRight || layoutStyle == FCLayoutStyle_RightToLeft){
                        m_splitter->setCursor(FCCursors_SizeWE);
                    }
                    else{
                        m_splitter->setCursor(FCCursors_SizeNS);
                    }
                    m_splitter->bringToFront();
                }
            }
            FCRect fcRect = m_firstControl->getBounds();
            if (fcRect.left != fRect.left || fcRect.top != fRect.top || fcRect.right != fRect.right || fcRect.bottom != fRect.bottom){
                reset = true;
                m_firstControl->setBounds(fRect);
                m_firstControl->update();
            }
            FCRect scRect = m_secondControl->getBounds();
            if (scRect.left != sRect.left || scRect.top != sRect.top || scRect.right != sRect.right || scRect.bottom != sRect.bottom){
                reset = true;
                m_secondControl->setBounds(sRect);
                m_secondControl->update();
            }
        }
        m_oldSize = getSize();
        return reset;
    }
    
    void FCSplitLayoutDiv::setProperty(const String& name, const String& value){
        if(name == L"candragsplitter"){
            if (m_splitter){
                m_splitter->setAllowDrag(FCStr::convertStrToBool(value));
            }
        }
        else if (name == L"splitmode"){
            String lowerStr = FCStr::toLower(value);
            if (lowerStr == L"absolutesize"){
                setSplitMode(FCSizeType_AbsoluteSize);
            }
            else{
                setSplitMode(FCSizeType_PercentSize);
            }
        }
        else if ((int)name.find(L"splitter-") != -1){
            if (m_splitter){
                m_splitter->setProperty(name.substr(9), value);
            }
        }
        else if (name == L"splitterposition"){
            if (m_splitter){
                ArrayList<String> strs = FCStr::split(value, L",");
                if (strs.size() == 4) {
                    m_splitter->setBounds(FCStr::convertStrToRect(value));
                } else if (strs.size() <= 2) {
                    int pos = FCStr::convertStrToInt(strs.get(0));
                    int lWidth = 0;
                    if (strs.size() == 2) {
                        lWidth = FCStr::convertStrToInt(strs.get(1));
                    }
                    int width = getWidth(), height = getHeight();
                    if (m_layoutStyle == FCLayoutStyle_TopToBottom || m_layoutStyle == FCLayoutStyle_BottomToTop) {
                        FCRect bounds = {0, pos, width, pos + lWidth};
                        m_splitter->setBounds(bounds);
                    } else {
                        FCRect bounds = {pos, 0, pos + lWidth, height};
                        m_splitter->setBounds(bounds);
                    }
                }
            }
        }
        else if (name == L"splittervisible"){
            if (m_splitter){
                m_splitter->setVisible(FCStr::convertStrToBool(value));
            }
        }
        else{
            FCLayoutDiv::setProperty(name, value);
        }
    }
    
    void FCSplitLayoutDiv::update(){
        onResetLayout();
        int controlsSize = (int)m_controls.size();
        for (int i = 0; i < controlsSize; i++){
            m_controls.get(i)->update();
        }
    }
}
