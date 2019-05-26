#include "..\\..\\stdafx.h"
#include "..\\..\\include\\date\\DateTitle.h"

namespace FaceCat{
	DateTitle::DateTitle(FCCalendar *calendar){
		m_calendar = calendar;
        setBackColor(FCColor_None);
        setBorderColor(FCColor_None);
		FCFont font(L"宋体", 22, true, false, false);
        setFont(&font);
		FCSize size = {180, 30};
		setSize(size);
	}

	DateTitle::~DateTitle(){
		m_calendar = 0;
	}

	FCCalendar* DateTitle::getCalendar(){
		return m_calendar;
	}

	void DateTitle::setCalendar(FCCalendar *calendar){
		m_calendar = calendar;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	String DateTitle::getControlType(){
		return L"DateTitle";
	}

	void DateTitle::onClick(FCTouchInfo touchInfo){
		FCButton::onClick(touchInfo);
        if (m_calendar){
            FCCalendarMode mode = m_calendar->getMode();
            if (mode == FCCalendarMode_Day){
                m_calendar->setMode(FCCalendarMode_Month);
            }
            else if (mode == FCCalendarMode_Month){
                m_calendar->setMode(FCCalendarMode_Year);
            }
			m_calendar->update();
            m_calendar->invalidate();
        }
	}

	void DateTitle::onPaintForeground(FCPaint *paint, const FCRect& clipRect){
		if(m_calendar){
			int width = getWidth(), height = getHeight();
			FCFont *font = getFont();
			String text;
			FCCalendarMode mode = m_calendar->getMode();
			wchar_t yMText[100] = {0};
			if (mode == FCCalendarMode_Day){
				CMonth *month = m_calendar->getMonth();				
				_stprintf_s(yMText, 99, L"%d年%d月", month->getYear(), month->getMonth());
			}
			else if(mode == FCCalendarMode_Month){
				_stprintf_s(yMText, 99, L"%d年", m_calendar->getMonthDiv()->getYear());
			}
			else if(mode == FCCalendarMode_Year){
				int startYear = m_calendar->getYearDiv()->getStartYear();
				_stprintf_s(yMText, 99, L"%d年-%d年", startYear, startYear + 12);
			}				
			text = yMText;
			FCSize tSize = paint->textSize(text.c_str(), font);
			FCRect tRect = {0};
			tRect.left = (width - tSize.cx) / 2;
			tRect.top = (height - tSize.cy) / 2;
			tRect.right = tRect.left + tSize.cx + 1;
			tRect.bottom = tRect.top + tSize.cy + 1;
			paint->drawText(text.c_str(), getPaintingTextColor(), font, tRect);
		}
	}
}