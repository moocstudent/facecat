#include "..\\..\\stdafx.h"
#include "..\\..\\include\\date\\CYears.h"

namespace FaceCat{
	CYears::CYears(){
	}

	CYears::~CYears(){
		for(int i = 0; i < Years.size(); i++){
			CYear *year = Years.getValue(i);
			delete year;
		}
		Years.clear();
	}

	CYear* CYears::getYear(int year){
		CYear *cy = 0;
		if(!Years.containsKey(year)){
			cy = new CYear(year);
			Years.put(year, cy);
		}
		else{
			cy = Years.get(year);
		}
		return cy;
	}
}