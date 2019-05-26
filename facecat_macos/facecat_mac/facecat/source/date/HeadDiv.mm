#include "stdafx.h"
#include "HeadDiv.h"

namespace FaceCat{
    HeadDiv::HeadDiv(FCCalendar *calendar){
        m_calendar = calendar;
        m_dateTitle = 0;
        m_lastBtn = 0;
        m_nextBtn = 0;
        m_weekStrings[0] = L"周日";
        m_weekStrings[1] = L"周一";
        m_weekStrings[2] = L"周二";
        m_weekStrings[3] = L"周三";
        m_weekStrings[4] = L"周四";
        m_weekStrings[5] = L"周五";
        m_weekStrings[6] = L"周六";
        FCFont font(L"宋体", 14, true, false, false);
        setFont(&font);
        setHeight(55);
    }
    
    HeadDiv::~HeadDiv(){
        m_calendar = 0;
        m_dateTitle = 0;
        m_lastBtn = 0;
        m_nextBtn = 0;
    }
    
    FCCalendar* HeadDiv::getCalendar(){
        return m_calendar;
    }
    
    void HeadDiv::setCalendar(FCCalendar *calendar){
        m_calendar = calendar;
    }
    
    DateTitle* HeadDiv::getDateTitle(){
        return m_dateTitle;
    }
    
    void HeadDiv::setDateTitle(DateTitle *dateTitle){
        m_dateTitle = dateTitle;
    }
    
    ArrowButton* HeadDiv::getLastBtn(){
        return m_lastBtn;
    }
    
    void HeadDiv::setLastBtn(ArrowButton *lastBtn){
        m_lastBtn = lastBtn;
    }
    
    ArrowButton* HeadDiv::getNextBtn(){
        return m_nextBtn;
    }
    
    void HeadDiv::setNextBtn(ArrowButton *nextBtn){
        m_nextBtn = nextBtn;
    }
    
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    String HeadDiv::getControlType(){
        return L"HeadDiv";
    }
    
    void HeadDiv::onLoad(){
        FCHost *host = getNative()->getHost();
        if(!m_dateTitle){
            m_dateTitle = dynamic_cast<DateTitle*>(host->createInternalControl(m_calendar, L"datetitle"));
            addControl(m_dateTitle);
        }
        if(!m_lastBtn){
            m_lastBtn = dynamic_cast<ArrowButton*>(host->createInternalControl(m_calendar, L"lastbutton"));
            addControl(m_lastBtn);
        }
        if(!m_nextBtn){
            m_nextBtn = dynamic_cast<ArrowButton*>(host->createInternalControl(m_calendar, L"nextbutton"));
            addControl(m_nextBtn);
        }
    }
    
    void HeadDiv::onPaintBackground(FCPaint *paint, const FCRect& clipRect){
        int width = getWidth(), height = getHeight();
        FCRect rect ={0, 0, width, height};
        paint->fillRect(getPaintingBackColor(), rect);
    }
    
    void HeadDiv::onPaintBorder(FCPaint *paint, const FCRect& clipRect){
        int width = getWidth(), height = getHeight();
        paint->drawLine(getPaintingBorderColor(), 1, 0, 0, height, width, height);
    }
    
    void HeadDiv::onPaintForeground(FCPaint *paint, const FCRect& clipRect){
        int width = getWidth(), height = getHeight();
        FCCalendarMode mode = m_calendar->getMode();
        if(mode == FCCalendarMode_Day){
            int left = 0;
            FCSize weekDaySize ={0};
            FCFont *font = getFont();
            Long textColor = getPaintingTextColor();
            for(int i = 0;i < 7; i++){
                weekDaySize = paint->textSize(m_weekStrings[i].c_str(), font);
                int textX = left + (width / 7) / 2 - weekDaySize.cx / 2;
                int textY = height - weekDaySize.cy - 2;
                FCRect weekRect ={textX, textY, textX + weekDaySize.cx, textY + weekDaySize.cy};
                paint->drawText(m_weekStrings[i].c_str(), textColor, font, weekRect);
                left += width / 7;
            }
        }
    }
    
    void HeadDiv::update(){
        FCView::update();
        int width = getWidth(), height = getHeight();
        if (m_dateTitle){
            FCPoint location ={(width - m_dateTitle->getWidth()) / 2, (height - m_dateTitle->getHeight()) / 2};
            m_dateTitle->setLocation(location);
        }
        if (m_lastBtn){
            FCPoint location ={2, (height - m_lastBtn->getHeight()) / 2};
            m_lastBtn->setLocation(location);
        }
        if (m_nextBtn){
            FCPoint location ={width - m_nextBtn->getWidth() - 2, (height - m_nextBtn->getHeight()) / 2};
            m_nextBtn->setLocation(location);
        }
    }
}
