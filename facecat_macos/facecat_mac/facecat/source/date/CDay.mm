#include "stdafx.h"
#include "CDay.h"

namespace FaceCat{
    CDay::CDay(int year, int month, int day){
        m_year = year;
        m_month = month;
        m_day = day;
    }
    
    CDay::~CDay(){
    }
    
    int CDay::getDay(){
        return m_day;
    }
    
    int CDay::getMonth(){
        return m_month;
    }
    
    int CDay::getYear(){
        return m_year;
    }
}
