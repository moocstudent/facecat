#include "stdafx.h"
#include "DayDiv.h"

namespace FaceCat{
    DayDiv::DayDiv(FCCalendar *calendar){
        m_am_ClickRowFrom = 0;
        m_am_ClickRowTo = 0;
        m_am_Direction = 0;
        m_am_Tick = 0;
        m_am_TotalTick = 40;
        m_calendar = calendar;
        onLoad();
    }
    
    DayDiv::~DayDiv(){
        m_calendar = 0;
        m_dayButtons.clear();
        m_dayButtons_am.clear();
    }
    
    FCCalendar* DayDiv::getCalendar(){
        return m_calendar;
    }
    
    void DayDiv::setCalendar(FCCalendar *calendar){
        m_calendar = calendar;
    }
    
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    void DayDiv::hide(){
        int dayButtonSize = (int)m_dayButtons.size();
        for(int i = 0; i < dayButtonSize; i++){
            DayButton *dayButton = m_dayButtons.get(i);
            dayButton->setVisible(false);
        }
    }
    
    void DayDiv::onClick(FCTouchInfo touchInfo){
        FCPoint mp = touchInfo.m_firstPoint;
        int dayButtonsSize = (int)m_dayButtons.size();
        for (int i = 0; i < dayButtonsSize; i++){
            DayButton *dayButton = m_dayButtons.get(i);
            if (dayButton->isVisible()){
                FCRect bounds = dayButton->getBounds();
                if (mp.x >= bounds.left && mp.x <= bounds.right && mp.y >= bounds.top && mp.y <= bounds.bottom){
                    dayButton->onClick(touchInfo);
                    return;
                }
            }
        }
        int dayFCButtonmSize = (int)m_dayButtons_am.size();
        for (int i = 0; i < dayFCButtonmSize; i++){
            DayButton *dayButton = m_dayButtons_am.get(i);
            if (dayButton->isVisible()){
                FCRect bounds = dayButton->getBounds();
                if (mp.x >= bounds.left && mp.x <= bounds.right && mp.y >= bounds.top && mp.y <= bounds.bottom){
                    dayButton->onClick(touchInfo);
                    return;
                }
            }
        }
    }
    
    void DayDiv::onLoad(){
        if (m_calendar){
            if(m_dayButtons.size() == 0 || m_dayButtons_am.size() == 0){
                m_dayButtons.clear();
                m_dayButtons_am.clear();
                FCHost *host = m_calendar->getNative()->getHost();
                for (int i = 0; i < 42; i++){
                    DayButton *dayButton = new DayButton(m_calendar);
                    m_dayButtons.add(dayButton);
                    DayButton *dayFCButtonm = new DayButton(m_calendar);
                    dayFCButtonm->setVisible(false);
                    m_dayButtons_am.add(dayFCButtonm);
                }
            }
        }
    }
    
    void DayDiv::onPaint(FCPaint *paint, const FCRect& clipRect){
        int dayButtonsSize = (int)m_dayButtons.size();
        for (int i = 0; i < dayButtonsSize; i++){
            DayButton *dayButton = m_dayButtons.get(i);
            if (dayButton->isVisible()){
                FCRect bounds = dayButton->getBounds();
                dayButton->onPaintBackGround(paint, bounds);
                dayButton->onPaintForeground(paint, bounds);
                dayButton->onPaintBorder(paint, bounds);
            }
        }
        int dayFCButtonmSize = (int)m_dayButtons_am.size();
        for (int i = 0; i < dayFCButtonmSize; i++){
            DayButton *dayButton = m_dayButtons_am.get(i);
            if (dayButton->isVisible()){
                FCRect bounds = dayButton->getBounds();
                dayButton->onPaintBackGround(paint, bounds);
                dayButton->onPaintForeground(paint, bounds);
                dayButton->onPaintBorder(paint, bounds);
            }
        }
    }
    
    void DayDiv::onResetDiv(int state){
        if(m_calendar){
            CMonth *thisMonth = m_calendar->getMonth();
            CMonth *lastMonth = m_calendar->getLastMonth(thisMonth->getYear(), thisMonth->getMonth());
            CMonth *nextMonth = m_calendar->getNextMonth(thisMonth->getYear(), thisMonth->getMonth());
            int left = 0;
            int headHeight = m_calendar->getHeadDiv()->getBottom();
            int top = headHeight;
            int width = m_calendar->getWidth();
            int height = m_calendar->getHeight();
            height -= m_calendar->getTimeDiv()->getHeight();
            int dayButtonHeight = height - headHeight;
            if (dayButtonHeight < 1) dayButtonHeight = 1;
            int subH = 0, toY = 0;
            if (m_am_Direction == 1){
                subH = (6 - (m_am_ClickRowTo - m_am_ClickRowFrom)) * (dayButtonHeight / 6);
                toY = -height + subH + headHeight;
                toY = toY * m_am_Tick / m_am_TotalTick;
                if(state == 1){
                    thisMonth = nextMonth;
                    lastMonth = m_calendar->getLastMonth(thisMonth->getYear(), thisMonth->getMonth());
                    nextMonth = m_calendar->getNextMonth(thisMonth->getYear(), thisMonth->getMonth());
                }
            }
            else if(m_am_Direction == 2){
                subH = (6 - (m_am_ClickRowFrom - m_am_ClickRowTo)) * (dayButtonHeight / 6);
                toY = height - subH - headHeight;
                toY = toY * m_am_Tick / m_am_TotalTick;
                if(state == 1){
                    thisMonth = lastMonth;
                    lastMonth = m_calendar->getLastMonth(thisMonth->getYear(), thisMonth->getMonth());
                    nextMonth = m_calendar->getNextMonth(thisMonth->getYear(), thisMonth->getMonth());
                }
            }
            int buttonSize = 0;
            if(state == 0){
                buttonSize = (int)m_dayButtons.size();
            }
            else if(state == 1){
                buttonSize = (int)m_dayButtons_am.size();
            }
            int dheight = dayButtonHeight / 6;
            HashMap<int, CDay*> days = thisMonth->m_days;
            CDay *firstDay = days.get(1);
            int startDayOfWeek = m_calendar->dayOfWeek(firstDay->getYear(), firstDay->getMonth(), firstDay->getDay());
            int todayYear = 0;
            int todayMonth = 0;
            int todayDay = 0;
            int i = 0;
            for (; i < buttonSize; i++){
                DayButton *dayButton = 0;
                if(state == 0){
                    dayButton = m_dayButtons.get(i);
                    buttonSize = (int)m_dayButtons.size();
                }
                else if(state == 1){
                    dayButton = m_dayButtons_am.get(i);
                    buttonSize = (int)m_dayButtons_am.size();
                }
                if (i == 35){
                    dheight = height - top;
                }
                int vOffset = 0;
                if(state == 1){
                    if(m_am_Tick > 0){
                        dayButton->setVisible(true);
                        if(m_am_Direction == 1){
                            vOffset = toY + dayButtonHeight;
                        }
                        else if(m_am_Direction == 2){
                            vOffset = toY - dayButtonHeight;
                        }
                    }
                    else{
                        dayButton->setVisible(false);
                        continue;
                    }
                }
                else{
                    vOffset = toY;
                }
                if ((i + 1) % 7 == 0){
                    FCPoint dp = {left, top + vOffset};
                    FCSize ds = {width - left, dheight};
                    FCRect bounds = {dp.x, dp.y, dp.x + ds.cx, dp.y + ds.cy};
                    dayButton->setBounds(bounds);
                    left = 0;
                    if (i != 0 && i != buttonSize - 1){
                        top += dheight;
                    }
                }
                else{
                    FCPoint dp = {left, top + vOffset};
                    FCSize ds = {width / 7 + ((i + 1) % 7) % 2, dheight};
                    FCRect bounds = {dp.x, dp.y, dp.x + ds.cx, dp.y + ds.cy};
                    dayButton->setBounds(bounds);
                    left += ds.cx;
                }
                CDay *cDay = 0;
                dayButton->setThisMonth(false);
                if (i >= startDayOfWeek && i <= startDayOfWeek + (int)days.size() - 1){
                    cDay = days.get(i - startDayOfWeek + 1);
                    dayButton->setThisMonth(true);
                }
                else if (i < startDayOfWeek){
                    cDay = lastMonth->m_days.get((int)lastMonth->m_days.size() - startDayOfWeek + i + 1);
                }
                else if (i > startDayOfWeek + (int)days.size() - 1){
                    cDay = nextMonth->m_days.get(i - startDayOfWeek - (int)days.size() + 1);
                }
                dayButton->setDay(cDay);
                if (state == 0 && dayButton->getDay() && dayButton->getDay() == m_calendar->getSelectedDay()){
                    dayButton->setSelected(true);
                }
                else{
                    dayButton->setSelected(false);
                }
            }
        }
    }
    
    void DayDiv::onTimer(){
        if (m_am_Tick > 0){
            m_am_Tick = (int)((double)m_am_Tick * 2 / 3);
            if(m_calendar){
                m_calendar->update();
                m_calendar->invalidate();
            }
        }
    }
    
    void DayDiv::selectDay(CDay* selectedDay){
        if(m_calendar){
            CMonth *m = m_calendar->getYears()->getYear(selectedDay->getYear())->Months.get(selectedDay->getMonth());
            CMonth *thisMonth = m_calendar->getMonth();
            if(m != thisMonth){
                if (thisMonth->getYear() * 12 + thisMonth->getMonth() > m->getYear() * 12 + m->getMonth()){
                    m_am_Direction = 1;
                }
                else{
                    m_am_Direction = 2;
                }
                int i = 0;
                int buttonSize = (int)m_dayButtons.size();
                for (i = 0;i < buttonSize; i++){
                    DayButton *dayButton = m_dayButtons.get(i);
                    if ((m_am_Direction == 1 && dayButton->getDay() == thisMonth->getFirstDay())
                        || m_am_Direction == 2 && dayButton->getDay() == thisMonth->getLastDay()){
                        m_am_ClickRowFrom = i / 7;
                        if (i % 7 != 0){
                            m_am_ClickRowFrom += 1;
                        }
                    }
                }
                m_calendar->setMonth(m);
                onResetDiv(0);
                buttonSize = (int)m_dayButtons_am.size();
                for (i = 0; i < buttonSize; i++){
                    DayButton *dayFCButtonm = m_dayButtons_am.get(i);
                    if ((m_am_Direction == 1 && dayFCButtonm->getDay() == m->getLastDay())
                        || (m_am_Direction == 2 && dayFCButtonm->getDay() == m->getFirstDay())){
                        m_am_ClickRowTo = i / 7;
                        if (i % 7 != 0){
                            m_am_ClickRowTo += 1;
                        }
                    }
                }
                if(m_calendar->useAnimation()){
                    m_am_Tick = m_am_TotalTick;
                }
            }
            else{
                int dayButtonsSize = m_dayButtons.size();
                for(int i = 0; i < dayButtonsSize; i++){
                    DayButton *dayButton = m_dayButtons.get(i);
                    if(dayButton->getDay() != selectedDay){
                        dayButton->setSelected(false);
                    }
                }
                m_calendar->setMonth(m);
            }
        }
    }
    
    void DayDiv::show(){
        int dayButtonSize = (int)m_dayButtons.size();
        for(int i = 0; i < dayButtonSize; i++){
            DayButton *dayButton = m_dayButtons.get(i);
            dayButton->setVisible(true);
        }
    }
    
    void DayDiv::update(){
        onResetDiv(0);
        onResetDiv(1);
    }
}
