#include "..\\..\\stdafx.h"
#include "..\\..\\include\\core\\FCLock.h"

namespace FaceCat
{
	FCLock::FCLock()
	{
		::InitializeCriticalSection(&m_cs);
	}

	FCLock::~FCLock()
	{
		::DeleteCriticalSection(&m_cs);
	}

	void FCLock::lock()
	{
		::EnterCriticalSection(&m_cs);
	}

	void FCLock::unLock()
	{
		::LeaveCriticalSection(&m_cs);
	}
}
