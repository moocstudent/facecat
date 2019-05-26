#include "..\\..\\stdafx.h"
#include "..\\..\\include\\date\\CMonth.h"

namespace FaceCat{
	void CMonth::createDays(){
		int daysInMonth = getDaysInMonth();
		for (int i = 1; i <= daysInMonth; i++){
			m_days.put(i, new CDay(m_year, m_month, i));
		}
	}

	CMonth::CMonth(int year, int month){
		m_year = year;
		m_month = month;
		createDays();
	}

	CMonth::~CMonth(){
		for (int i = 0; i < m_days.size(); i++){
			CDay *day = m_days.getValue(i);
			delete day;
		}
		m_days.clear();
	}

	int CMonth::getDaysInMonth(){
		switch(m_month){
			case 1:
			case 3:
			case 5:
			case 7:
			case 8:
			case 10:
			case 12:
				return 31;
			case 4:
			case 6:
			case 9:
			case 11:
				return 30;
			case 2:
			if((m_year % 4 == 0 && m_year % 100 != 0)|| m_year % 400 == 0)
				return 29;
			else
				return 28;
		}
		return 0;
	}

	CDay* CMonth::getFirstDay(){
		return m_days.get(1);
	}

	CDay* CMonth::getLastDay(){
		return m_days.get((int)m_days.size());
	}


	int CMonth::getMonth(){
		return m_month;
	}

	int CMonth::getYear(){
		return m_year;
	}
}