#include "..\\..\\stdafx.h"
#include "..\\..\\include\\service\\FCHttpEasyService.h"

namespace FaceCat{
	FCHttpEasyService::FCHttpEasyService(){
	}

	FCHttpEasyService::~FCHttpEasyService(){
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void FCHttpEasyService::onReceive(FCHttpData *data){		
		data->m_resStr = L"Hello World";
	}
}