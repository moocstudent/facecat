#include "..\\..\\stdafx.h"
#include "..\\..\\include\\scroll\\FCVScrollBar.h"

namespace FaceCat{
	void FCVScrollBar::backButtonTouchDown(Object sender, FCTouchInfo touchInfo, Object pInvoke){
		FCVScrollBar *scrollBar = (FCVScrollBar*)pInvoke;
		scrollBar->onBackButtonTouchDown(touchInfo);
	}

	void FCVScrollBar::backButtonTouchUp(Object sender, FCTouchInfo touchInfo, Object pInvoke){
		FCVScrollBar *scrollBar = (FCVScrollBar*)pInvoke;
		scrollBar->onBackButtonTouchUp(touchInfo);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	FCVScrollBar::FCVScrollBar(){
		m_backButtonTouchDownEvent = backButtonTouchDown;
		m_backButtonTouchUpEvent = backButtonTouchUp;
	}

	FCVScrollBar::~FCVScrollBar(){
		FCButton *backButton = getBackButton();
		if(backButton){
			if(m_backButtonTouchDownEvent){
				backButton->removeEvent(m_backButtonTouchDownEvent, FCEventID::TOUCHDOWN);
				m_backButtonTouchDownEvent = 0;
			}
			if(m_backButtonTouchUpEvent){
				backButton->removeEvent(m_backButtonTouchUpEvent, FCEventID::TOUCHUP);
				m_backButtonTouchUpEvent = 0;
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	String FCVScrollBar::getControlType(){
		return L"VScrollBar";
	}

	void FCVScrollBar::onBackButtonTouchDown(FCTouchInfo touchInfo){
		FCButton *scrollButton = getScrollButton();
		FCPoint mp = touchInfo.m_firstPoint;
		if (mp.y < scrollButton->getTop()){
			pageReduce();
			setIsReducing(true);
		}
		else if (mp.y > scrollButton->getBottom()){
			pageAdd();
			setIsAdding(true);
		}
	}

	void FCVScrollBar::onBackButtonTouchUp(FCTouchInfo touchInfo){
		 setIsAdding(false);
         setIsReducing(false);
	}

	void FCVScrollBar::onDragScroll(){
		bool floatRight = false;
		FCButton *backButton = getBackButton();
		FCButton *scrollButton = getScrollButton();
		int backButtonHeight = backButton->getHeight();
		int contentSize = getContentSize();
		if(scrollButton->getBottom() > backButtonHeight){
			floatRight = true;
		}
		FCScrollBar::onDragScroll();
		if(floatRight){
			setPos(contentSize);
		}
		else{
			setPos((int)((Long)contentSize * (Long)scrollButton->getTop() / backButtonHeight));
		}
		onScrolled();
	}

	void FCVScrollBar::onLoad(){
		bool isAdd = false;
		FCButton *backButton = getBackButton();
		if (backButton){
			isAdd = true;
		}
		FCScrollBar::onLoad();
		if (!isAdd){
			backButton = getBackButton();
			backButton->addEvent(m_backButtonTouchDownEvent, FCEventID::TOUCHDOWN, this);
			backButton->addEvent(m_backButtonTouchUpEvent, FCEventID::TOUCHUP, this);
		}
	}

	void FCVScrollBar::update(){
		if(!getNative()){
			return;
		}
		FCButton *addButton = getAddButton();
		FCButton *backButton = getBackButton();
		FCButton *reduceButton = getReduceButton();
		FCButton *scrollButton = getScrollButton();
		int width = getWidth(), height = getHeight();
		int contentSize = getContentSize();
		if (contentSize > 0 && addButton && backButton && reduceButton && scrollButton){
			int pos = getPos();
            int pageSize = getPageSize();
            if (pos > contentSize - pageSize){
                pos = contentSize - pageSize;
            }
            if (pos < 0){
                pos = 0;
            }
			int abHeight = addButton->isVisible() ? addButton->getHeight() : 0;
			FCSize aSize = {width, abHeight};
			addButton->setSize(aSize);
			FCPoint aPoint = {0, height - abHeight};
			addButton->setLocation(aPoint);
			int rbHeight = reduceButton->isVisible() ? reduceButton->getHeight() : 0;
			FCSize sSize = {width, rbHeight};
			reduceButton->setSize(sSize);
			FCPoint sPoint = {0, 0};
			reduceButton->setLocation(sPoint);
			int backHeight = height - abHeight - rbHeight;
			FCSize bSize = {width, backHeight};
			backButton->setSize(bSize);
			FCPoint bPoint = {0, rbHeight};
			backButton->setLocation(bPoint);
			int scrollHeight = backHeight * pageSize / contentSize;
			int scrollPos = (int)((Long)backHeight * (Long)pos / contentSize);
			if(scrollHeight < 10){
				scrollHeight = 10;
				if (scrollPos + scrollHeight > backHeight){
                    scrollPos = backHeight - scrollHeight;
                }
			}
			FCSize scSize = {width, scrollHeight};
			scrollButton->setSize(scSize);
			FCPoint scPoint = {0, scrollPos};
			scrollButton->setLocation(scPoint);
		}
	}
}