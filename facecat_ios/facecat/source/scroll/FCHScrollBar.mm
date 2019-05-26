#include "stdafx.h"
#include "FCHScrollBar.h"

namespace FaceCat{
    void FCHScrollBar::backButtonTouchDown(Object sender, FCTouchInfo touchInfo, Object pInvoke){
        FCHScrollBar *scrollBar = (FCHScrollBar*)pInvoke;
        scrollBar->onBackButtonTouchDown(touchInfo);
    }
    
    void FCHScrollBar::backButtonTouchUp(Object sender, FCTouchInfo touchInfo, Object pInvoke){
        FCHScrollBar *scrollBar = (FCHScrollBar*)pInvoke;
        scrollBar->onBackButtonTouchUp(touchInfo);
    }
    
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    FCHScrollBar::FCHScrollBar(){
        m_backButtonTouchDownEvent = backButtonTouchDown;
        m_backButtonTouchUpEvent = backButtonTouchUp;
    }
    
    FCHScrollBar::~FCHScrollBar(){
        FCButton *backButton = getBackButton();
        if(backButton){
            if(m_backButtonTouchDownEvent){
                backButton->removeEvent((Object)m_backButtonTouchDownEvent, FCEventID::TOUCHDOWN);
                m_backButtonTouchDownEvent = 0;
            }
            if(m_backButtonTouchUpEvent){
                backButton->removeEvent((Object)m_backButtonTouchUpEvent, FCEventID::TOUCHUP);
                m_backButtonTouchUpEvent = 0;
            }
        }
    }
    
    String FCHScrollBar::getControlType(){
        return L"HScrollBar";
    }
    
    void FCHScrollBar::onBackButtonTouchDown(FCTouchInfo touchInfo){
        FCButton *scrollButton = getScrollButton();
        FCPoint mp = touchInfo.m_firstPoint;
        if (mp.x < scrollButton->getLeft()){
            pageReduce();
            setIsReducing(true);
        }
        else if (mp.x > scrollButton->getRight()){
            pageAdd();
            setIsAdding(true);
        }
    }
    
    void FCHScrollBar::onBackButtonTouchUp(FCTouchInfo touchInfo){
        setIsAdding(false);
        setIsReducing(false);
    }
    
    void FCHScrollBar::onDragScroll(){
        bool floatRight = false;
        FCButton *backButton = getBackButton();
        FCButton *scrollButton = getScrollButton();
        int backButtonWidth = backButton->getWidth();
        int contentSize = getContentSize();
        if (scrollButton->getRight() > backButtonWidth){
            floatRight = true;
        }
        FCScrollBar::onDragScroll();
        if(floatRight){
            setPos(contentSize);
        }
        else{
            setPos((int)((Long)contentSize * (Long)scrollButton->getLeft() / backButtonWidth));
        }
        onScrolled();
    }
    
    void FCHScrollBar::onLoad(){
        bool isAdd = false;
        FCButton *backButton = getBackButton();
        if (backButton){
            isAdd = true;
        }
        FCScrollBar::onLoad();
        if (!isAdd){
            backButton = getBackButton();
            backButton->addEvent((Object)m_backButtonTouchDownEvent, FCEventID::TOUCHDOWN, this);
            backButton->addEvent((Object)m_backButtonTouchUpEvent, FCEventID::TOUCHUP, this);
        }
    }
    
    void FCHScrollBar::update(){
        if(!getNative()){
            return;
        }
        FCButton *addButton = getAddButton();
        FCButton *backButton = getBackButton();
        FCButton *reduceButton = getReduceButton();
        FCButton *scrollButton = getScrollButton();
        int width = getWidth(), height = getHeight(), contentSize = getContentSize();
        if (contentSize > 0 && addButton && backButton && reduceButton && scrollButton ){
            int pos = getPos();
            int pageSize = getPageSize();
            if (pos > contentSize - pageSize){
                pos = contentSize - pageSize;
            }
            if (pos < 0){
                pos = 0;
            }
            int abWidth = addButton->isVisible() ? addButton->getWidth() : 0;
            FCSize aSize ={abWidth, height};
            addButton->setSize(aSize);
            FCPoint aPoint ={width - abWidth, 0};
            addButton->setLocation(aPoint);
            int rbWidth = reduceButton->isVisible() ? reduceButton->getWidth() : 0;
            FCSize sSize ={rbWidth, height};
            reduceButton->setSize(sSize);
            FCPoint sPoint ={0, 0};
            reduceButton->setLocation(sPoint);
            int backWidth = width - abWidth - rbWidth;
            FCSize bSize ={backWidth, height};
            backButton->setSize(bSize);
            FCPoint bPoint ={rbWidth, 0};
            backButton->setLocation(bPoint);
            int scrollWidth = backWidth * pageSize / contentSize;
            int scrollPos = (int)((Long)backWidth * (Long)pos / contentSize);
            if(scrollWidth < 10){
                scrollWidth = 10;
                if (scrollPos + scrollWidth > backWidth){
                    scrollPos = backWidth - scrollWidth;
                }
            }
            FCSize scSize ={scrollWidth, height};
            scrollButton->setSize(scSize);
            FCPoint scPoint ={scrollPos, 0};
            scrollButton->setLocation(scPoint);
        }
    }
}
