#include "..\\..\\stdafx.h"
#include "..\\..\\include\\date\\CYear.h"

namespace FaceCat{
	void CYear::createMonths(){
		for (int i = 1; i <= 12; i++){
			Months.put(i, new CMonth(m_year, i));
		}
	}

	CYear::CYear(int year){
		m_year = year;
		createMonths();
	}

	CYear::~CYear(){
		for(int i = 0; i < Months.size(); i++){
			CMonth *month = Months.getValue(i);
			delete month;
		}
		Months.clear();
	}
}