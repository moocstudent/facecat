#include "..\\..\\stdafx.h"
#include "..\\..\\include\\date\\DayButton.h"

namespace FaceCat{
    Long DayButton::getPaintingBackColor(){
		if (m_selected){
            return FCColor_Pushed;
        }
        else{
            if (m_inThisMonth){
                return FCColor_Hovered;
            }
            else{
                return FCColor_Back;
            }
        }
    }

    Long DayButton::getPaintingBorderColor(){
        return FCColor_Border;
    }

    Long DayButton::getPaintingTextColor(){
        return FCColor_Text;
    }

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	DayButton::DayButton(FCCalendar *calendar){
		m_bounds.left = 0;
		m_bounds.top = 0;
		m_bounds.right = 0;
		m_bounds.bottom = 0;
		m_calendar = calendar;
		m_day = 0;
		m_inThisMonth = false;
		m_selected = false;
		m_visible = true;
	}

	DayButton::~DayButton(){
		m_calendar = 0;
	}

	FCRect DayButton::getBounds(){
		return m_bounds;
	}

	void DayButton::setBounds(const FCRect& bounds){
		m_bounds = bounds;
	}

	FCCalendar* DayButton::getCalendar(){
		return m_calendar;
	}

	void DayButton::setCalendar(FCCalendar *calendar){
		m_calendar = calendar;
	}

	CDay* DayButton::getDay(){
		return m_day;
	}

	void DayButton::setDay(CDay *day){
		m_day = day;
	}

	bool DayButton::inThisMonth(){
		return m_inThisMonth;
	}

	void DayButton::setThisMonth(bool inThisMonth){
		m_inThisMonth = inThisMonth;
	}

	bool DayButton::isSelected(){
		return m_selected;
	}

	void DayButton::setSelected(bool selected){
		m_selected = selected;
	}

	bool DayButton::isVisible(){
		return m_visible;
	}

	void DayButton::setVisible(bool visible){
		m_visible = visible;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void DayButton::onClick(FCTouchInfo touchInfo){
		if(m_calendar && m_day){
			m_calendar->setSelectedDay(m_day);
		}
	}

	void DayButton::onPaintBackGround(FCPaint *paint, const FCRect& clipRect){
        Long backColor = getPaintingBackColor();
        paint->fillRect(backColor, m_bounds); 
	}

	void DayButton::onPaintBorder(FCPaint *paint, const FCRect& clipRect){
        Long borderColor = getPaintingBorderColor();
        paint->drawLine(borderColor, 1, 0, m_bounds.left, m_bounds.bottom - 1, m_bounds.right - 1, m_bounds.bottom - 1);
        paint->drawLine(borderColor, 1, 0, m_bounds.right - 1, m_bounds.top, m_bounds.right - 1, m_bounds.bottom - 1);
	}

	void DayButton::onPaintForeground(FCPaint *paint, const FCRect& clipRect){
		if(m_day){
			int width = m_bounds.right - m_bounds.left;
			int height = m_bounds.bottom - m_bounds.top;
			int day = m_day->getDay();
			wchar_t tDay[10] = {0};
			_stprintf_s(tDay, 9, L"%d", day);
			FCFont *font = m_calendar->getFont();
			FCSize textSize = paint->textSize(tDay, font);
			int tLeft = m_bounds.left + (width - textSize.cx) / 2;
			int tTop = m_bounds.top + (height - textSize.cy) / 2;
			FCRect textRect = {tLeft, tTop, tLeft + textSize.cx, tTop + textSize.cy};
			paint->drawText(tDay, getPaintingTextColor(), font, textRect);
		}
	}
}