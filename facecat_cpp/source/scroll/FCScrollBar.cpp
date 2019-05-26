#include "..\\..\\stdafx.h"
#include "..\\..\\include\\scroll\\FCScrollBar.h"

namespace FaceCat
{
	void FCScrollBar::addButtonTouchDown(Object sender, FCTouchInfo touchInfo, Object pInvoke){
		FCScrollBar *scrollBar = (FCScrollBar*)pInvoke;
		if(scrollBar){
			scrollBar->onAddButtonTouchDown(touchInfo);
		}
	}

	void FCScrollBar::addButtonTouchUp(Object sender, FCTouchInfo touchInfo, Object pInvoke){
		FCScrollBar *scrollBar = (FCScrollBar*)pInvoke;
		if(scrollBar){
			scrollBar->onAddButtonTouchUp(touchInfo);
		}
	}

	void FCScrollBar::reduceButtonTouchDown(Object sender, FCTouchInfo touchInfo, Object pInvoke){
		FCScrollBar *scrollBar = (FCScrollBar*)pInvoke;
		if(scrollBar){
			scrollBar->onReduceButtonTouchDown(touchInfo);
		}
	}

	void FCScrollBar::reduceButtonTouchUp(Object sender, FCTouchInfo touchInfo, Object pInvoke){
		FCScrollBar *scrollBar = (FCScrollBar*)pInvoke;
		if(scrollBar){
			scrollBar->onReduceButtonTouchUp(touchInfo);
		}
	}

	void FCScrollBar::scrollButtonDragging(Object sender, Object pInvoke){
		FCScrollBar *scrollBar = (FCScrollBar*)pInvoke;
		if(scrollBar){
			scrollBar->onDragScroll();
		}
	}

	/////////////////////////////////////////////////////////////////////////////////////////////////////

	FCScrollBar::FCScrollBar(){
		m_addButton = 0;
		m_addSpeed = 0;
		m_backButton = 0;
		m_contentSize = 0;
		m_isAdding = false;
		m_isReducing = false;
		m_lineSize = 10;
		m_pageSize = 0;
		m_pos = 0;
		m_reduceButton = 0;
		m_scrollButton = 0;
		m_tick = 0;
		m_timerID = FCView::getNewTimerID();
        m_addButtonTouchDownEvent = addButtonTouchDown;
        m_addButtonTouchUpEvent = addButtonTouchUp;
        m_scrollButtonDraggingEvent = scrollButtonDragging;
        m_reduceButtonTouchDownEvent = reduceButtonTouchDown;
        m_reduceButtonTouchUpEvent = reduceButtonTouchUp;
		setCanFocus(false);
		setDisplayOffset(false);
		FCSize size = {20, 20};
		setSize(size);
		setTopMost(true);
	}

	FCScrollBar::~FCScrollBar(){
        stopTimer(m_timerID);
        if (m_addButton){
            if (m_addButtonTouchDownEvent){
				m_addButton->removeEvent(m_addButtonTouchDownEvent, FCEventID::TOUCHDOWN);
                m_addButtonTouchDownEvent = 0;
            }
            if (m_addButtonTouchUpEvent){
				m_addButton->removeEvent(m_addButtonTouchUpEvent, FCEventID::TOUCHUP);
                m_addButtonTouchUpEvent = 0;
            }
        }
        if (m_scrollButton){
            if (m_scrollButtonDraggingEvent){
				m_scrollButton->removeEvent(m_scrollButtonDraggingEvent, FCEventID::DRAGGING);
                m_scrollButtonDraggingEvent = 0;
            }
        }
        if (m_reduceButton){
            if (m_reduceButtonTouchDownEvent){
				m_reduceButton->removeEvent(m_reduceButtonTouchDownEvent, FCEventID::TOUCHDOWN);
                m_reduceButtonTouchDownEvent = 0;
            }
            if (m_reduceButtonTouchUpEvent){
				m_reduceButton->removeEvent(m_reduceButtonTouchUpEvent, FCEventID::TOUCHUP);
                m_reduceButtonTouchUpEvent = 0;
            }
        }
		m_addButton = 0;
		m_backButton = 0;
		m_reduceButton = 0;
		m_scrollButton = 0;
	}

	FCButton* FCScrollBar::getAddButton(){
		return m_addButton;
	}

	int FCScrollBar::getAddSpeed(){
		return m_addSpeed;
	}

	void FCScrollBar::setAddSpeed(int addSpeed){
		m_addSpeed = addSpeed;
		if (m_addSpeed != 0){
            startTimer(m_timerID, 10);
        }
        else{
            stopTimer(m_timerID);
        }
	}

	FCButton* FCScrollBar::getBackButton(){
		return m_backButton;
	}

	int FCScrollBar::getContentSize(){
		return m_contentSize;
	}

	void FCScrollBar::setContentSize(int contentWidth){
		m_contentSize = contentWidth;
	}

	bool FCScrollBar::isAdding(){
		return m_isAdding;
	}

	void FCScrollBar::setIsAdding(bool isAdding){
		if(m_isAdding != isAdding){
			m_isAdding = isAdding;
			m_tick = 0;
			if(m_isAdding){
				startTimer(m_timerID, 100);
			}
			else{
				stopTimer(m_timerID);
			}
		}
	}

	bool FCScrollBar::isReducing(){
		return m_isReducing;
	}

	void FCScrollBar::setIsReducing(bool isReducing){
		if(m_isReducing != isReducing){
			m_isReducing = isReducing;
			m_tick = 0;
			if(m_isReducing){
				startTimer(m_timerID, 100);
			}
			else{
				stopTimer(m_timerID);
			}
		}
	}

	int FCScrollBar::getLineSize(){
		return m_lineSize;
	}

	void FCScrollBar::setLineSize(int lineSize){
		m_lineSize = lineSize;
	}

	int FCScrollBar::getPageSize(){
		return m_pageSize;
	}

	void FCScrollBar::setPageSize(int pageSize){
		m_pageSize = pageSize;
	}

	int FCScrollBar::getPos(){
		return m_pos;
	}

	void FCScrollBar::setPos(int pos){
		m_pos = pos;
		if (m_pos > m_contentSize - m_pageSize){
			m_pos = m_contentSize - m_pageSize;
		}
		if (m_pos < 0){
			m_pos = 0;
		}
	}

	FCButton* FCScrollBar::getReduceButton(){
		return m_reduceButton;
	}

	FCButton* FCScrollBar::getScrollButton(){
		return m_scrollButton;
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////

	String FCScrollBar::getControlType(){
		return L"ScrollBar";
	}

	void FCScrollBar::lineAdd(){
		m_pos += m_lineSize;
		if (m_pos > m_contentSize - m_pageSize){
			m_pos = m_contentSize - m_pageSize;
		}
		update();
		onScrolled();
	}

	void FCScrollBar::lineReduce(){
		m_pos -= m_lineSize;
		if (m_pos < 0){
			m_pos = 0;
		}
		update();
		onScrolled();
	}

	void FCScrollBar::onAddButtonTouchDown(FCTouchInfo touchInfo){
	    lineAdd();
        setIsAdding(true);
	}

	void FCScrollBar::onAddButtonTouchUp(FCTouchInfo touchInfo){
	    setIsAdding(false);
	}

	void FCScrollBar::onAddSpeedScrollEnd(){
	}

	void FCScrollBar::onAddSpeedScrollStart(DWORD startTime, DWORD nowTime, int start, int end){
	    int diff = (int)((nowTime - startTime) / 10000);
        if (diff > 0 && diff < 800){
            int sub = 5000 * (abs(start - end) / 20) / diff;
            if (start > end){
                setAddSpeed(getAddSpeed() + sub);
            }
            else if (start < end){
                setAddSpeed(getAddSpeed() - sub);
            }
        }
	}

	int FCScrollBar::onAddSpeedScrolling(){
            int sub = m_addSpeed / 10;
            if (sub == 0){
                sub = m_addSpeed > 0 ? 1 : -1;
            }
            return sub;
	}

	void FCScrollBar::onDragScroll(){
		if (m_scrollButton->getLeft() < 0){
			m_scrollButton->setLeft(0);
		}
		if (m_scrollButton->getTop() < 0){
			m_scrollButton->setTop(0);
		}
		if (m_scrollButton->getRight() > m_backButton->getWidth()){
			m_scrollButton->setLeft(m_backButton->getWidth() - m_scrollButton->getWidth());
		}
		if (m_scrollButton->getBottom() > m_backButton->getHeight()){
			m_scrollButton->setTop(m_backButton->getHeight() - m_scrollButton->getHeight());
		}
	}

	void FCScrollBar::onLoad(){
		FCView::onLoad();
		FCHost *host = getNative()->getHost();
		if (!m_addButton){
			m_addButton = dynamic_cast<FCButton*>(host->createInternalControl(this, L"addbutton"));
			m_addButton->addEvent(m_addButtonTouchDownEvent, FCEventID::TOUCHDOWN, this);
			m_addButton->addEvent(m_addButtonTouchUpEvent, FCEventID::TOUCHUP, this);
			addControl(m_addButton);
		}
		if (!m_backButton){
			m_backButton = dynamic_cast<FCButton*>(host->createInternalControl(this, L"backbutton"));
			addControl(m_backButton);
		}
		if (!m_reduceButton){
			m_reduceButton = dynamic_cast<FCButton*>(host->createInternalControl(this, L"reducebutton"));
			m_reduceButton->addEvent(m_reduceButtonTouchDownEvent, FCEventID::TOUCHDOWN, this);
			m_reduceButton->addEvent(m_reduceButtonTouchUpEvent, FCEventID::TOUCHUP, this);
			addControl(m_reduceButton);
		}
		if (!m_scrollButton){
			m_scrollButton = dynamic_cast<FCButton*>(host->createInternalControl(this, L"scrollbutton"));
			m_scrollButton->addEvent(m_scrollButtonDraggingEvent, FCEventID::DRAGGING, this);
			m_backButton->addControl(m_scrollButton);
		}
	}

	void FCScrollBar::onReduceButtonTouchDown(FCTouchInfo touchInfo){
	    lineReduce();
        setIsReducing(true);
	}

	void FCScrollBar::onReduceButtonTouchUp(FCTouchInfo touchInfo){
	   setIsReducing(false);
	}

	void FCScrollBar::onScrolled(){
		callEvents(FCEventID::SCROLLED);
		FCView *parent = getParent();
        if (parent){
            parent->invalidate();
        }
	}

	void FCScrollBar::onVisibleChanged(){
		FCView::onVisibleChanged();
		if(!isVisible()){
			m_pos = 0;
		}
	}

	void FCScrollBar::pageAdd(){
		m_pos += m_pageSize;
		if (m_pos > m_contentSize - m_pageSize){
			m_pos = m_contentSize - m_pageSize;
		}
		update();
		onScrolled();
	}

	void FCScrollBar::pageReduce(){
		m_pos -= m_pageSize;
		if (m_pos < 0){
			m_pos = 0;
		}
		update();
		onScrolled();
	}

	void FCScrollBar::scrollToBegin(){
		m_pos = 0;
		update();
		onScrolled();
	}

	void FCScrollBar::scrollToEnd(){
		m_pos = m_contentSize - m_pageSize;
		if(m_pos < 0){
			m_pos = 0;
		}
		update();
		onScrolled();
	}

	void FCScrollBar::onTimer(int timerID){
		FCView::onTimer(timerID);
        if (m_timerID == timerID){
            if (m_isAdding){
                if (m_tick > 5){
                    pageAdd();
                }
                else{
                    lineAdd();
                }
            }
            if (m_isReducing){
                if (m_tick > 5){
                    pageReduce();
                }
                else{
                    lineReduce();
                }
            }
            if (m_addSpeed != 0){
                int sub = onAddSpeedScrolling();
                setPos(getPos() + sub);
                update();
                onScrolled();
                m_addSpeed -= sub;
                if (abs(m_addSpeed) < 3){
                    m_addSpeed = 0;
                }
                if (m_addSpeed == 0){
					onAddSpeedScrollEnd();
                    stopTimer(m_timerID);
					if(getParent()){
						getParent()->invalidate();
					}
                }
            }
            m_tick++;
        }
	}
}