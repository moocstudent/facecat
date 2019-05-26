#include "..\\..\\stdafx.h"
#include "..\\..\\include\\date\\MonthDiv.h"

namespace FaceCat{
	YearDiv::YearDiv(FCCalendar *calendar){
		m_am_Direction = 0;
		m_am_Tick = 0;
		m_am_TotalTick = 40;
		m_calendar = calendar;
		m_startYear = 0;
		onLoad();
	}

	YearDiv::~YearDiv(){
		m_calendar = 0;
		m_yearButtons.clear();
		m_yearButtons_am.clear();
	}

	FCCalendar* YearDiv::getCalendar(){
		return m_calendar;
	}

	void YearDiv::setCalendar(FCCalendar *calendar){
		m_calendar = calendar;
	}

	int YearDiv::getStartYear(){
		return m_startYear;
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void YearDiv::hide(){
		int yearButtonSize = (int)m_yearButtons.size();
		for(int i = 0; i < yearButtonSize; i++){
			YearButton *yearButton = m_yearButtons.get(i);
			yearButton->setVisible(false);
		}
	}

	void YearDiv::onClick(FCTouchInfo touchInfo){
		FCPoint mp = touchInfo.m_firstPoint;
	    int yearButtonsSize = (int)m_yearButtons.size();
        for (int i = 0; i < yearButtonsSize; i++){
            YearButton *yearButton = m_yearButtons.get(i);
            if (yearButton->isVisible()){
                FCRect bounds = yearButton->getBounds();
                if (mp.x >= bounds.left && mp.x <= bounds.right && mp.y >= bounds.top && mp.y <= bounds.bottom){
                    yearButton->onClick(touchInfo);
                    return;
                }
            }
        }
        int yearFCButtonmSize = (int)m_yearButtons_am.size();
        for (int i = 0; i < yearFCButtonmSize; i++){
            YearButton *yearButton = m_yearButtons_am.get(i);
            if (yearButton->isVisible()){
                FCRect bounds = yearButton->getBounds();
                if (mp.x >= bounds.left && mp.x <= bounds.right && mp.y >= bounds.top && mp.y <= bounds.bottom){
                    yearButton->onClick(touchInfo);
                    return;
                }
            }
        }
	}

	void YearDiv::onLoad(){
		if (m_calendar){
			if(m_yearButtons.size() == 0 || m_yearButtons_am.size() == 0){
				m_yearButtons.clear();
				m_yearButtons_am.clear();
				FCHost *host = m_calendar->getNative()->getHost();
                for (int i = 0; i < 12; i++){
                    YearButton *yearButton = new YearButton(m_calendar);
                    m_yearButtons.add(yearButton);
                    YearButton *yearFCButtonm = new YearButton(m_calendar);
                    yearFCButtonm->setVisible(false);
                    m_yearButtons_am.add(yearFCButtonm);
                }
			}
		}
	}

	void YearDiv::onPaint(FCPaint *paint, const FCRect& clipRect){
	    int yearButtonsSize = (int)m_yearButtons.size();
        for (int i = 0; i < yearButtonsSize; i++){
            YearButton *yearButton = m_yearButtons.get(i);
            if (yearButton->isVisible()){
                FCRect bounds = yearButton->getBounds();
                yearButton->onPaintBackGround(paint, bounds);
                yearButton->onPaintForeground(paint, bounds);
                yearButton->onPaintBorder(paint, bounds);
            }
        }
        int yearFCButtonmSize = (int)m_yearButtons_am.size();
        for (int i = 0; i < yearFCButtonmSize; i++){
            YearButton *yearButton = m_yearButtons_am.get(i);
            if (yearButton->isVisible()){
                FCRect bounds = yearButton->getBounds();
                yearButton->onPaintBackGround(paint, bounds);
                yearButton->onPaintForeground(paint, bounds);
                yearButton->onPaintBorder(paint, bounds);
            }
        }
	}
	
	void YearDiv::onResetDiv(int state){
		if(m_calendar){
			int thisStartYear = m_startYear;
            int lastStartYear = m_startYear - 12;
            int nextStartYear = m_startYear + 12;
            int left = 0;
            int headHeight = m_calendar->getHeadDiv()->getHeight();
            int top = headHeight;
            int width = m_calendar->getWidth();
            int height = m_calendar->getHeight();
			height -= m_calendar->getTimeDiv()->getHeight();
            int yearButtonHeight = height - top;
            if (yearButtonHeight < 1){
                yearButtonHeight = 1;
            }
            int toY = 0;
            ArrayList<YearButton*> yearButtons;
            if (m_am_Direction == 1){
                toY = yearButtonHeight * m_am_Tick / m_am_TotalTick;
                if (state == 1){
                    thisStartYear = nextStartYear;
                    lastStartYear = thisStartYear - 12;
                    nextStartYear = thisStartYear + 12;
                }
            }
            else if (m_am_Direction == 2){
                toY = -yearButtonHeight * m_am_Tick / m_am_TotalTick;
                if (state == 1){
                    thisStartYear = lastStartYear;
                    lastStartYear = thisStartYear - 12;
                    nextStartYear = thisStartYear + 12;
                }
            }
            if (state == 0){
                yearButtons = m_yearButtons;
            }
            else if (state == 1){
                yearButtons = m_yearButtons_am;
            }
            int dheight = yearButtonHeight / 3;
            int buttonSize = (int)yearButtons.size();
            for (int i = 0; i < buttonSize; i++){
                if (i == 8){
                    dheight = height - top;
                }
                YearButton *yearButton = yearButtons.get(i);
                yearButton->setYear(thisStartYear + i);
                int vOffSet = 0;
                if (state == 1){
                    if (m_am_Tick > 0){
                        yearButton->setVisible(true);
                        if (m_am_Direction == 1){
                            vOffSet = toY - yearButtonHeight;
                        }
                        else if (m_am_Direction == 2){
                            vOffSet = toY + yearButtonHeight;
                        }
                    }
                    else{
                        yearButton->setVisible(false);
                        continue;
                    }
                }
                else{
                    vOffSet = toY;
                }
                if ((i + 1) % 4 == 0){
					FCPoint dp = {left, top + vOffSet};
					FCSize ds = {width - left, dheight};
					FCRect bounds = {dp.x, dp.y, dp.x + ds.cx, dp.y + ds.cy};
					yearButton->setBounds(bounds);
                    left = 0;
                    if (i != 0 && i != buttonSize - 1){
                        top += dheight;
                    }
                }
                else{
					FCPoint dp = {left, top + vOffSet};
					FCSize ds = {width / 4 + ((i + 1) % 4) % 2, dheight};
					FCRect bounds = {dp.x, dp.y, dp.x + ds.cx, dp.y + ds.cy};
					yearButton->setBounds(bounds);
                    left += ds.cx;
                }
            }
		}
	}

	void YearDiv::onTimer(){
	    if (m_am_Tick > 0){
            m_am_Tick = (int)((double)m_am_Tick * 2 / 3);
			if(m_calendar){
				m_calendar->update();
				m_calendar->invalidate();
			}
        }
	}

	void YearDiv::selectStartYear(int startYear){
		if(m_calendar){
		    if(m_startYear != startYear){
                if (startYear > m_startYear){
                    m_am_Direction = 1;
                }
                else{
                    m_am_Direction = 2;
                }
				if(m_calendar->useAnimation()){
					m_am_Tick = m_am_TotalTick;
				}
                m_startYear = startYear;
            }
		}
	}


	void YearDiv::show(){
		int yearButtonSize = (int)m_yearButtons.size();
		for(int i = 0; i < yearButtonSize; i++){
			YearButton *yearButton = m_yearButtons.get(i);
			yearButton->setVisible(true);
			yearButton->bringToFront();
		}
	}

	void YearDiv::update(){
		onResetDiv(0);
		onResetDiv(1);
	}
}
