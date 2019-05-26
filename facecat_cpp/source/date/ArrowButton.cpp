#include "..\\..\\stdafx.h"
#include "..\\..\\include\\date\\ArrowButton.h"

namespace FaceCat{
	ArrowButton::ArrowButton(FCCalendar *calendar){
		m_calendar = calendar;
		m_toLast = true;
		setBackColor(FCColor_None);
		setBorderColor(FCColor_None);
		FCSize size = {16, 16};
		setSize(size);
	}

	ArrowButton::~ArrowButton(){
		m_calendar = 0;
	}

	FCCalendar* ArrowButton::getCalendar(){
		return m_calendar;
	}

	void ArrowButton::setCalendar(FCCalendar *calendar){
		m_calendar = calendar;
	}

	bool ArrowButton::isToLast(){
		return m_toLast;
	}

	void ArrowButton::setToLast(bool toLast){
		m_toLast = toLast;
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	String ArrowButton::getControlType(){
		return L"ArrowButton";
	}

	void ArrowButton::onClick(FCTouchInfo touchInfo){
		FCButton::onClick(touchInfo);
		if (m_calendar){
			FCCalendarMode mode = m_calendar->getMode();
            if (mode == FCCalendarMode_Day){
                if (m_toLast){
                    m_calendar->goLastMonth();
                }
                else{
                    m_calendar->goNextMonth();
                }
            }
            else if (mode == FCCalendarMode_Month){
                MonthDiv *monthDiv = m_calendar->getMonthDiv();
				if(monthDiv){
					int year = monthDiv->getYear();
					if (m_toLast){
						monthDiv->selectYear(year - 1);
					}
					else{
						monthDiv->selectYear(year + 1);
					}
				}
            }
            else if (mode == FCCalendarMode_Year){
                YearDiv *yearDiv = m_calendar->getYearDiv();
				if(yearDiv){
					int year = yearDiv->getStartYear();
					if (m_toLast){
						yearDiv->selectStartYear(year - 12);
					}
					else{
						yearDiv->selectStartYear(year + 12);
					}
				}
            }
		m_calendar->invalidate();
        }
	}

	void ArrowButton::onPaintForeground(FCPaint *paint, const FCRect& clipRect){
		int width = getWidth(), height = getHeight();
		FCPoint p1 = {0,0}, p2 = {0,0}, p3 = {0,0};
		if(m_toLast){
			p1.x = 0;
			p1.y = height / 2;
			p2.x = width;
			p2.y = 0;
			p3.x = width;
			p3.y = height;
		}
		else{
			p1.x = 0;
			p1.y = 0;
			p2.x = 0;
			p2.y = height;
			p3.x = width;
			p3.y = height / 2;
		}
		FCPoint *points = new FCPoint[3];
		points[0] = p1;
		points[1] = p2;
		points[2] = p3;
		paint->fillPolygon(getPaintingTextColor(), points, 3);
	}
}