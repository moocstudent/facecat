#include "stdafx.h"
#include "MonthDiv.h"

namespace FaceCat{
    MonthDiv::MonthDiv(FCCalendar *calendar){
        m_am_Direction = 0;
        m_am_Tick = 0;
        m_am_TotalTick = 40;
        m_year = 0;
        m_calendar = calendar;
        onLoad();
    }
    
    MonthDiv::~MonthDiv(){
        m_calendar = 0;
        m_monthButtons.clear();
        m_monthButtons_am.clear();
    }
    
    FCCalendar* MonthDiv::getCalendar(){
        return m_calendar;
    }
    
    void MonthDiv::setCalendar(FCCalendar *calendar){
        m_calendar = calendar;
    }
    
    int MonthDiv::getYear(){
        return m_year;
    }
    
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    void MonthDiv::hide(){
        int monthButtonSize = (int)m_monthButtons.size();
        for(int i = 0; i < monthButtonSize; i++){
            MonthButton *monthButton = m_monthButtons.get(i);
            monthButton->setVisible(false);
        }
    }
    
    void MonthDiv::onClick(FCTouchInfo touchInfo){
        FCPoint mp = touchInfo.m_firstPoint;
        int monthButtonsSize = (int)m_monthButtons.size();
        for (int i = 0; i < monthButtonsSize; i++){
            MonthButton *monthButton = m_monthButtons.get(i);
            if (monthButton->isVisible()){
                FCRect bounds = monthButton->getBounds();
                if (mp.x >= bounds.left && mp.x <= bounds.right && mp.y >= bounds.top && mp.y <= bounds.bottom){
                    monthButton->onClick(touchInfo);
                    return;
                }
            }
        }
        int monthButtonAmSize = (int)m_monthButtons_am.size();
        for (int i = 0; i < monthButtonAmSize; i++){
            MonthButton *monthButton = m_monthButtons_am.get(i);
            if (monthButton->isVisible()){
                FCRect bounds = monthButton->getBounds();
                if (mp.x >= bounds.left && mp.x <= bounds.right && mp.y >= bounds.top && mp.y <= bounds.bottom){
                    monthButton->onClick(touchInfo);
                    return;
                }
            }
        }
    }
    
    void MonthDiv::onLoad(){
        if (m_calendar){
            if(m_monthButtons.size() == 0 || m_monthButtons_am.size() == 0){
                m_monthButtons.clear();
                m_monthButtons_am.clear();
                FCHost *host = m_calendar->getNative()->getHost();
                for (int i = 0; i < 12; i++){
                    MonthButton *monthButton = new MonthButton(m_calendar);
                    monthButton->setMonth(i + 1);
                    m_monthButtons.add(monthButton);
                    MonthButton *monthButtonAm = new MonthButton(m_calendar);
                    monthButtonAm->setMonth(i + 1);
                    monthButtonAm->setVisible(false);
                    m_monthButtons_am.add(monthButtonAm);
                }
            }
        }
    }
    
    void MonthDiv::onPaint(FCPaint *paint, const FCRect& clipRect){
        int monthButtonsSize = (int)m_monthButtons.size();
        for (int i = 0; i < monthButtonsSize; i++){
            MonthButton *monthButton = m_monthButtons.get(i);
            if (monthButton->isVisible()){
                FCRect bounds = monthButton->getBounds();
                monthButton->onPaintBackGround(paint, bounds);
                monthButton->onPaintForeground(paint, bounds);
                monthButton->onPaintBorder(paint, bounds);
            }
        }
        int monthButtonAmSize = (int)m_monthButtons_am.size();
        for (int i = 0; i < monthButtonAmSize; i++){
            MonthButton *monthButton = m_monthButtons_am.get(i);
            if (monthButton->isVisible()){
                FCRect bounds = monthButton->getBounds();
                monthButton->onPaintBackGround(paint, bounds);
                monthButton->onPaintForeground(paint, bounds);
                monthButton->onPaintBorder(paint, bounds);
            }
        }
    }
    
    void MonthDiv::onResetDiv(int state){
        if(m_calendar){
            int thisYear = m_year;
            int lastYear = m_year - 1;
            int nextYear = m_year + 1;
            int left = 0;
            int headHeight = m_calendar->getHeadDiv()->getHeight();
            int top = headHeight;
            int width = m_calendar->getWidth();
            int height = m_calendar->getHeight();
            height -= m_calendar->getTimeDiv()->getHeight();
            int monthButtonHeight = height - top;
            if (monthButtonHeight < 1){
                monthButtonHeight = 1;
            }
            int toY = 0;
            ArrayList<MonthButton*> monthButtons;
            if (m_am_Direction == 1){
                toY = monthButtonHeight * m_am_Tick / m_am_TotalTick;
                if (state == 1){
                    thisYear = nextYear;
                    lastYear = thisYear - 1;
                    nextYear = thisYear + 1;
                }
            }
            else if (m_am_Direction == 2){
                toY = -monthButtonHeight * m_am_Tick / m_am_TotalTick;
                if (state == 1){
                    thisYear = lastYear;
                    lastYear = thisYear - 1;
                    nextYear = thisYear + 1;
                }
            }
            if (state == 0){
                monthButtons = m_monthButtons;
            }
            else if (state == 1){
                monthButtons = m_monthButtons_am;
            }
            int dheight = monthButtonHeight / 3;
            int buttonSize = (int)monthButtons.size();
            for (int i = 0; i < buttonSize; i++){
                if (i == 8){
                    dheight = height - top;
                }
                MonthButton *monthButton = monthButtons.get(i);
                monthButton->setYear(thisYear);
                int vOffSet = 0;
                if (state == 1){
                    if (m_am_Tick > 0){
                        monthButton->setVisible(true);
                        if (m_am_Direction == 1){
                            vOffSet = toY - monthButtonHeight;
                        }
                        else if (m_am_Direction == 2){
                            vOffSet = toY + monthButtonHeight;
                        }
                    }
                    else{
                        monthButton->setVisible(false);
                        continue;
                    }
                }
                else{
                    vOffSet = toY;
                }
                if ((i + 1) % 4 == 0){
                    FCPoint dp ={left, top + vOffSet};
                    FCSize ds ={width - left, dheight};
                    FCRect bounds ={dp.x, dp.y, dp.x + ds.cx, dp.y + ds.cy};
                    monthButton->setBounds(bounds);
                    left = 0;
                    if (i != 0 && i != buttonSize - 1){
                        top += dheight;
                    }
                }
                else{
                    FCPoint dp ={left, top + vOffSet};
                    FCSize ds ={width / 4 + ((i + 1) % 4) % 2, dheight};
                    FCRect bounds ={dp.x, dp.y, dp.x + ds.cx, dp.y + ds.cy};
                    monthButton->setBounds(bounds);
                    left += ds.cx;
                }
            }
        }
    }
    
    void MonthDiv::onTimer(){
        if (m_am_Tick > 0){
            m_am_Tick = (int)((double)m_am_Tick * 2 / 3);
            if(m_calendar){
                m_calendar->update();
                m_calendar->invalidate();
            }
        }
    }
    
    void MonthDiv::selectYear(int year){
        if(m_calendar){
            if(m_year != year){
                if (year > m_year){
                    m_am_Direction = 1;
                }
                else{
                    m_am_Direction = 2;
                }
                if(m_calendar->useAnimation()){
                    m_am_Tick = m_am_TotalTick;
                }
                m_year = year;
            }
        }
    }
    
    
    void MonthDiv::show(){
        int monthButtonSize = (int)m_monthButtons.size();
        for(int i = 0; i < monthButtonSize; i++){
            MonthButton *monthButton = m_monthButtons.get(i);
            monthButton->setVisible(true);
            monthButton->bringToFront();
        }
    }
    
    void MonthDiv::update(){
        onResetDiv(0);
        onResetDiv(1);
    }
}
