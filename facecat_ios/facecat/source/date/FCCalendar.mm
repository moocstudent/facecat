#include "stdafx.h"
#include "FCCalendar.h"

namespace FaceCat{
    FCCalendar::FCCalendar(){
        m_dayDiv = 0;
        m_headDiv = 0;
        m_mode = FCCalendarMode_Day;
        m_month = 3;
        m_monthDiv = 0;
        m_years = new CYears;
        m_selectedDay = getMonth()->m_days.get(1);
        m_timeDiv = 0;
        m_timerID = getNewTimerID();
        m_useAnimation = false;
        m_year = 2014;
        m_yearDiv = 0;
        FCSize size ={200, 200};
        setSize(size);
    }
    
    FCCalendar::~FCCalendar(){
        if(m_dayDiv){
            delete m_dayDiv;
            m_dayDiv = 0;
        }
        m_headDiv = 0;
        if(m_monthDiv){
            delete m_monthDiv;
            m_monthDiv = 0;
        }
        m_selectedDay = 0;
        if(m_timeDiv){
            delete m_timeDiv;
            m_timeDiv = 0;
        }
        if(m_yearDiv){
            delete m_yearDiv;
            m_yearDiv = 0;
        }
        if(m_years){
            delete m_years;
            m_years = 0;
        }
        stopTimer(m_timerID);
    }
    
    DayDiv* FCCalendar::getDayDiv(){
        return m_dayDiv;
    }
    
    void FCCalendar::setDayDiv(DayDiv *dayDiv){
        m_dayDiv = dayDiv;
    }
    
    HeadDiv* FCCalendar::getHeadDiv(){
        return m_headDiv;
    }
    
    void FCCalendar::setHeadDiv(HeadDiv *headDiv){
        m_headDiv = headDiv;
    }
    
    FCCalendarMode FCCalendar::getMode(){
        return m_mode;
    }
    
    void FCCalendar::setMode(FCCalendarMode mode){
        if (m_mode != mode){
            FCCalendarMode oldMode = m_mode;
            m_mode = mode;
            if (m_mode == FCCalendarMode_Month){
                if (m_dayDiv){
                    m_dayDiv->hide();
                }
                if (m_yearDiv){
                    m_yearDiv->hide();
                }
                if (!m_monthDiv){
                    m_monthDiv = new MonthDiv(this);
                }
                if(oldMode == FCCalendarMode_Day){
                    m_monthDiv->selectYear(m_year);
                }
                m_monthDiv->show();
            }
            else if (m_mode == FCCalendarMode_Year){
                if (m_dayDiv){
                    m_dayDiv->hide();
                }
                int startYear = m_year;
                if (m_monthDiv){
                    startYear = m_monthDiv->getYear();
                    m_monthDiv->hide();
                }
                if (!m_yearDiv){
                    m_yearDiv = new YearDiv(this);
                }
                m_yearDiv->selectStartYear(startYear);
                m_yearDiv->show();
            }
            else{
                if (m_monthDiv){
                    m_monthDiv->hide();
                }
                if (m_yearDiv){
                    m_yearDiv->hide();
                }
                m_dayDiv->show();
            }
        }
    }
    
    CMonth* FCCalendar::getMonth(){
        return m_years->getYear(m_year)->Months.get(m_month);
    }
    
    void FCCalendar::setMonth(CMonth *month){
        m_year = month->getYear();
        m_month = month->getMonth();
        update();
    }
    
    MonthDiv* FCCalendar::getMonthDiv(){
        return m_monthDiv;
    }
    
    void FCCalendar::setMonthDiv(MonthDiv *monthDiv){
        m_monthDiv = monthDiv;
    }
    
    CDay* FCCalendar::getSelectedDay(){
        return m_selectedDay;
    }
    
    void FCCalendar::setSelectedDay(CDay *day){
        if(m_selectedDay != day){
            m_selectedDay = day;
            if (m_dayDiv){
                m_dayDiv->selectDay(m_selectedDay);
            }
            invalidate();
            onSelectedTimeChanged();
        }
    }
    
    TimeDiv* FCCalendar::getTimeDiv(){
        return m_timeDiv;
    }
    
    void FCCalendar::setTimeDiv(TimeDiv *timeDiv){
        m_timeDiv = timeDiv;
    }
    
    bool FCCalendar::useAnimation(){
        return m_useAnimation;
    }
    
    void FCCalendar::setUseAnimation(bool useAnimation){
        m_useAnimation = useAnimation;
        if(m_useAnimation){
            startTimer(m_timerID, 20);
        }
        else{
            stopTimer(m_timerID);
        }
    }
    
    YearDiv* FCCalendar::getYearDiv(){
        return m_yearDiv;
    }
    
    void FCCalendar::setYearDiv(YearDiv *yearDiv){
        m_yearDiv = yearDiv;
    }
    
    CYears* FCCalendar::getYears(){
        return m_years;
    }
    
    void FCCalendar::setYears(CYears *years){
        m_years = years;
        update();
        setSelectedDay(getMonth()->m_days.get(1));
    }
    
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    int FCCalendar::dayOfWeek(int y, int m, int d){
        if (m == 1 || m == 2){
            m += 12;
            y--;
        }
        return (d + 2 * m + 3 * (m + 1) / 5 + y + y / 4 - y / 100 + y / 400) % 7;
    }
    
    String FCCalendar::getControlType(){
        return L"Calendar";
    }
    
    CMonth* FCCalendar::getLastMonth(int year, int month){
        int lastMonth = month - 1;
        int lastYear = year;
        if (lastMonth == 0){
            lastMonth = 12;
            lastYear -= 1;
        }
        return m_years->getYear(lastYear)->Months.get(lastMonth);
    }
    
    CMonth* FCCalendar::getNextMonth(int year, int month){
        int nextMonth = month + 1;
        int nextYear = year;
        if (nextMonth == 13){
            nextMonth = 1;
            nextYear += 1;
        }
        return m_years->getYear(nextYear)->Months.get(nextMonth);
    }
    
    void FCCalendar::getProperty(const String& name, String *value, String *type){
        if(name == L"selectedday"){
            *type = L"string";
            wchar_t szDate[1024] ={0};
            swprintf(szDate, 1023, L"%d-%02d-%02d", m_year, m_month, m_selectedDay->getDay());
            *value = szDate;
        }
        else if(name == L"useanimation"){
            *type = L"bool";
            *value = FCStr::convertBoolToStr(useAnimation());
        }
        else{
            FCView::getProperty(name, value, type);
        }
    }
    
    ArrayList<String> FCCalendar::getPropertyNames(){
        ArrayList<String> propertyNames = FCView::getPropertyNames();
        propertyNames.add(L"SelectedDay");
        propertyNames.add(L"UseAnimation");
        return propertyNames;
    }
    
    void FCCalendar::goLastMonth(){
        setSelectedDay(getLastMonth(m_year,m_month)->m_days.get(1));
    }
    
    void FCCalendar::goNextMonth(){
        setSelectedDay(getNextMonth(m_year,m_month)->m_days.get(1));
    }
    
    void FCCalendar::onClick(FCTouchInfo touchInfo){
        FCView::onClick(touchInfo);
        if (m_dayDiv){
            m_dayDiv->onClick(touchInfo);
        }
        if (m_monthDiv){
            m_monthDiv->onClick(touchInfo);
        }
        if (m_yearDiv){
            m_yearDiv->onClick(touchInfo);
        }
    }
    
    void FCCalendar::onKeyDown(char key){
        FCView::onKeyDown(key);
        FCHost *host = getNative()->getHost();
        if(!host->isKeyPress(VK_CONTROL)
           && !host->isKeyPress(VK_MENU)
           && !host->isKeyPress(VK_SHIFT)){
            CMonth *thisMonth = getMonth();
            CMonth *lastMonth = getLastMonth(m_year,m_month);
            CMonth *nextMonth = getNextMonth(m_year,m_month);
            int today = m_selectedDay->getDay();
            if(key >= 37 && key <= 40){
                switch(key){
                    case 37:
                        if (m_selectedDay == thisMonth->getFirstDay()){
                            setSelectedDay(lastMonth->getLastDay());
                        }
                        else{
                            setSelectedDay(thisMonth->m_days.get(today - 1));
                        }
                        break;
                    case 38:
                        if (today <= 7){
                            setSelectedDay(lastMonth->m_days.get((int)lastMonth->m_days.size() - (7 - today)));
                        }
                        else{
                            setSelectedDay(thisMonth->m_days.get(m_selectedDay->getDay() - 7));
                        }
                        break;
                    case 39:
                        if (m_selectedDay == thisMonth->getLastDay()){
                            setSelectedDay(nextMonth->getFirstDay());
                        }
                        else{
                            setSelectedDay(thisMonth->m_days.get(today + 1));
                        }
                        break;
                    case 40:
                        if (today > (int)thisMonth->m_days.size() - 7){
                            setSelectedDay(nextMonth->m_days.get(7 - ((int)thisMonth->m_days.size() - today)));
                        }
                        else{
                            setSelectedDay(thisMonth->m_days.get(today + 7));
                        }
                        break;
                }
            }
        }
    }
    
    void FCCalendar::onLoad(){
        FCView::onLoad();
        if(!m_dayDiv){
            m_dayDiv = new DayDiv(this);
        }
        if(!m_timeDiv){
            m_timeDiv = new TimeDiv(this);
        }
        if(!m_headDiv){
            FCHost *host = getNative()->getHost();
            m_headDiv = dynamic_cast<HeadDiv*>(host->createInternalControl(this, L"headdiv"));
            addControl(m_headDiv);
        }
        if(m_useAnimation){
            startTimer(m_timerID, 20);
        }
        else{
            stopTimer(m_timerID);
        }
        if (m_years && m_year == 0 && m_month == 0){
            time_t tn;
            struct tm* t = gmtime(&tn);
            m_year = t->tm_year + 1900;
            m_month = t->tm_mon + 1;
            setSelectedDay(m_years->getYear(m_year)->Months.get(m_month)->m_days.get(t->tm_mday));
        }
    }
    
    void FCCalendar::onPaintBackground(FCPaint *paint, const FCRect& clipRect){
        FCView::onPaintBackground(paint, clipRect);
        if (m_dayDiv){
            m_dayDiv->onPaint(paint, clipRect);
        }
        if (m_monthDiv){
            m_monthDiv->onPaint(paint, clipRect);
        }
        if (m_yearDiv){
            m_yearDiv->onPaint(paint, clipRect);
        }
        if(m_timeDiv){
            m_timeDiv->onPaint(paint, clipRect);
        }
    }
    
    void FCCalendar::onSelectedTimeChanged(){
        callEvents(FCEventID::SELECTEDTIMECHANGED);
    }
    
    void FCCalendar::onTimer(int timerID){
        FCView::onTimer(timerID);
        if(m_timerID == timerID){
            if(m_dayDiv){
                m_dayDiv->onTimer();
            }
            if(m_monthDiv){
                m_monthDiv->onTimer();
            }
            if(m_yearDiv){
                m_yearDiv->onTimer();
            }
            if(m_timeDiv){
                m_timeDiv->onTimer();
            }
        }
    }
    
    void FCCalendar::setProperty(const String& name, const String& value){
        if(name == L"selectedday"){
            ArrayList<String> strs = FCStr::split(value, L"-");
            m_year = FCStr::convertStrToInt(strs.get(0));
            m_month = FCStr::convertStrToInt(strs.get(1));
            if(m_selectedDay){
                delete m_selectedDay;
                m_selectedDay = 0;
            }
            setSelectedDay(new CDay(m_year, m_month, FCStr::convertStrToInt(strs.get(2))));
            strs.clear();
        }
        else if(name == L"useanimation"){
            setUseAnimation(FCStr::convertStrToBool(value));
        }
        else{
            FCView::setProperty(name, value);
        }
    }
    
    void FCCalendar::update(){
        FCView::update();
        if(m_dayDiv){
            m_dayDiv->update();
        }
        if(m_headDiv){
            m_headDiv->bringToFront();
            m_headDiv->update();
        }
        if(m_monthDiv){
            m_monthDiv->update();
        }
        if(m_yearDiv){
            m_yearDiv->update();
        }
        if(m_timeDiv){
            m_timeDiv->update();
        }
    }
}
