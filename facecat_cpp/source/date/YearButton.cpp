#include "..\\..\\stdafx.h"
#include "..\\..\\include\\date\\YearButton.h"

namespace FaceCat{
	Long YearButton::getPaintingBackColor(){
        return FCColor_Back;
    }

    Long YearButton::getPaintingBorderColor(){
        return FCColor_Border;
    }

    Long YearButton::getPaintingTextColor(){
        return FCColor_Text;
    }

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	YearButton::YearButton(FCCalendar *calendar){
		m_bounds.left = 0;
		m_bounds.top = 0;
		m_bounds.right = 0;
		m_bounds.bottom = 0;
		m_calendar = calendar;
		m_visible = true;
		m_year = 0;
	}

	YearButton::~YearButton(){
		m_calendar = 0;
	}

	FCRect YearButton::getBounds(){
		return m_bounds;
	}

	void YearButton::setBounds(const FCRect& bounds){
		m_bounds = bounds;
	}

	FCCalendar* YearButton::getCalendar(){
		return m_calendar;
	}

	void YearButton::setCalendar(FCCalendar *calendar){
		m_calendar = calendar;
	}

	bool YearButton::isVisible(){
		return m_visible;
	}

	void YearButton::setVisible(bool visible){
		m_visible = visible;
	}

	int YearButton::getYear(){
		return m_year;
	}

	void YearButton::setYear(int year){
		m_year = year;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void YearButton::onClick(FCTouchInfo touchInfo){
	    if (m_calendar){
            m_calendar->setMode(FCCalendarMode_Month);
			m_calendar->getMonthDiv()->selectYear(m_year);
			m_calendar->update();
			m_calendar->invalidate();
        }
	}

	void YearButton::onPaintBackGround(FCPaint *paint, const FCRect& clipRect){
        Long backColor = getPaintingBackColor();
        paint->fillRect(backColor, m_bounds); 
	}

	void YearButton::onPaintBorder(FCPaint *paint, const FCRect& clipRect){
        Long borderColor = getPaintingBorderColor();
        paint->drawLine(borderColor, 1, 0, m_bounds.left, m_bounds.bottom - 1, m_bounds.right - 1, m_bounds.bottom - 1);
        paint->drawLine(borderColor, 1, 0, m_bounds.right - 1, m_bounds.top, m_bounds.right - 1, m_bounds.bottom - 1);
	}

	void YearButton::onPaintForeground(FCPaint *paint, const FCRect& clipRect){
		int width = m_bounds.right - m_bounds.left;
		int height = m_bounds.bottom - m_bounds.top;
		wchar_t tYear[10] = {0};
		_stprintf_s(tYear, 9, L"%d", m_year);
		FCFont *font = getFont();
		FCSize textSize = paint->textSize(tYear, font);
		int tLeft = m_bounds.left + (width - textSize.cx) / 2;
		int tTop = m_bounds.top + (height - textSize.cy) / 2;
		FCRect textRect = {tLeft, tTop, tLeft + textSize.cx, tTop + textSize.cy};
		paint->drawText(tYear, getPaintingTextColor(), font, textRect);
	}
}