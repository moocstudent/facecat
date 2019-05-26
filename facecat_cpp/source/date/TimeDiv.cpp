#include "..\\..\\stdafx.h"
#include "..\\..\\include\\date\\TimeDiv.h"

namespace FaceCat{
	void TimeDiv::selectedTimeChanged(Object sender, Object pInvoke){
		TimeDiv *timeDiv = (TimeDiv*)pInvoke;
		timeDiv->onSelectedTimeChanged();
	}

	Long TimeDiv::getPaintingBackColor(){
		return FCColor_Back;
	}

	Long TimeDiv::getPaintingBorderColor(){
		return FCColor_Border;
	}

	Long TimeDiv::getPaintingTextColor(){
		return FCColor_Text;
	}


	///////////////////////////////////////////////////////////////////////////////////////////////////

	TimeDiv::TimeDiv(FCCalendar *calendar){
		m_calendar = calendar;
		m_height = 40;
		m_spinHour = 0;
		m_spinMinute = 0;
		m_spinSecond = 0;
		onLoad();
	}

	TimeDiv::~TimeDiv(){
		m_calendar = 0;
		m_spinHour = 0;
		m_spinMinute = 0;
		m_spinSecond = 0;
	}

	FCCalendar* TimeDiv::getCalendar(){
		return m_calendar;
	}

	void TimeDiv::setCalendar(FCCalendar *calendar){
		m_calendar = calendar;
	}

	int TimeDiv::getHeight(){
		return m_height;
	}

	void TimeDiv::setHeight(int height){
		m_height = height;
	}

	int TimeDiv::getHour(){
		if (m_spinHour){
            return (int)m_spinHour->getValue();
        }
        else{
            return 0;
        }
	}

	void TimeDiv::setHour(int hour){
        if (m_spinHour){
            m_spinHour->setValue(hour);
        }
	}

	int TimeDiv::getMinute(){
		if (m_spinMinute){
            return (int)m_spinMinute->getValue();
        }
        else{
            return 0;
        }
	}

	void TimeDiv::setMinute(int minute){
		if (m_spinMinute){
            m_spinMinute->setValue(minute);
        }
	}

	int TimeDiv::getSecond(){
		if (m_spinSecond){
            return (int)m_spinSecond->getValue();
        }
        else{
            return 0;
        }
	}

	void TimeDiv::setSecond(int second){
		if (m_spinSecond){
            m_spinSecond->setValue(second);
        }
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////

	void TimeDiv::onLoad(){
		if (m_calendar){
            if (!m_spinHour){
                m_spinHour = new FCSpin();
                m_spinHour->setMaximum(23);
				m_spinHour->setTextAlign(FCHorizontalAlign_Right);
                m_calendar->addControl(m_spinHour);
				m_spinHour->addEvent(&selectedTimeChanged, FCEventID::VALUECHANGED, this);
            }
            if (!m_spinMinute){
                m_spinMinute = new FCSpin();
                m_spinMinute->setMaximum(59);
				m_spinMinute->setTextAlign(FCHorizontalAlign_Right);
                m_calendar->addControl(m_spinMinute);
				m_spinMinute->addEvent(&selectedTimeChanged, FCEventID::VALUECHANGED, this);
            }
            if (!m_spinSecond){
                m_spinSecond = new FCSpin();
                m_spinSecond->setMaximum(59);
				m_spinSecond->setTextAlign(FCHorizontalAlign_Right);
                m_calendar->addControl(m_spinSecond);
				m_spinSecond->addEvent(&selectedTimeChanged, FCEventID::VALUECHANGED, this);
            }
        }
	}

	void TimeDiv::onPaint(FCPaint *paint, const FCRect& clipRect){
		int width = m_calendar->getWidth(), height = m_calendar->getHeight();
        int top = height - m_height;
		FCRect rect = {0, height - m_height, width, height};
        paint->fillRect(getPaintingBackColor(), rect);
        if (m_height > 0){
            Long textColor = getPaintingTextColor();
            FCFont *font = m_calendar->getFont();
            FCSize tSize = paint->textSize(L"时", font);
			FCRect tRect = {0};
            tRect.left = width / 3 - tSize.cx;
            tRect.top = top + m_height / 2 - tSize.cy / 2;
            tRect.right = tRect.left + tSize.cx;
            tRect.bottom = tRect.top + tSize.cy;
            paint->drawText(L"时", textColor, font, tRect);
            tSize = paint->textSize(L"分", font);
            tRect.left = width * 2 / 3 - tSize.cx;
            tRect.top = top + m_height / 2 - tSize.cy / 2;
            tRect.right = tRect.left + tSize.cx;
            tRect.bottom = tRect.top + tSize.cy;
            paint->drawText(L"分", textColor, font, tRect);
            tSize = paint->textSize(L"秒", font);
            tRect.left = width - tSize.cx - 5;
            tRect.top = top + m_height / 2 - tSize.cy / 2;
            tRect.right = tRect.left + tSize.cx;
            tRect.bottom = tRect.top + tSize.cy;
            paint->drawText(L"秒", textColor, font, tRect);
        }
	}

	void TimeDiv::onSelectedTimeChanged(){
		if (m_calendar){
            m_calendar->onSelectedTimeChanged();
        }
	}

	void TimeDiv::onTimer(){
	}

	void TimeDiv::update(){
		if (m_height > 0){
            int width = m_calendar->getWidth(), height = m_calendar->getHeight();
            int top = height - m_height;
            int left = 5;
            if (m_spinHour){
                m_spinHour->setVisible(true);
				FCPoint location = {left, top + m_height / 2 - m_spinHour->getHeight() / 2};
                m_spinHour->setLocation(location);
                m_spinHour->setWidth((width - 15) / 3 - 20);
            }
            if (m_spinMinute){
                m_spinMinute->setVisible(true);
				FCPoint location = {width / 3 + 5, top + m_height / 2 - m_spinMinute->getHeight() / 2};
                m_spinMinute->setLocation(location);
                m_spinMinute->setWidth((width - 15) / 3 - 20);
            }
            if (m_spinSecond){
                m_spinSecond->setVisible(true);
				FCPoint location = {width * 2 / 3 + 5, top + m_height / 2 - m_spinSecond->getHeight() / 2};
                m_spinSecond->setLocation(location);
                m_spinSecond->setWidth((width - 15) / 3 - 20);

            }
        }
        else{
            if (m_spinHour){
                m_spinHour->setVisible(false);
            }
            if (m_spinMinute){
                m_spinMinute->setVisible(false);
            }
            if (m_spinSecond){
                m_spinSecond->setVisible(false);
            }
        }
	}
}
