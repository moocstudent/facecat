#include "..\\..\\stdafx.h"
#include "..\\..\\include\\core\\WinHost.h"
#include "..\\..\\include\\core\\FCLib.h"

namespace FaceCat{
	FCSize WinHost::getClientSize(){
		FCSize size = {0};
		if(m_native){
			FCRect wRect;
			::GetClientRect(m_hWnd, &wRect);
			size.cx = wRect.right - wRect.left;
			size.cy = wRect.bottom - wRect.top;
		}
		return size;
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	WinHost::WinHost(){
		m_allowOperate = true;
		m_allowPartialPaint = true;
		m_hImc = 0;
		m_hWnd = 0;
		m_invokeSerialID = 0;
		m_native = 0;
		m_pInvokeMsgID = 0x0401;
		m_toolTip = 0;
		::InitializeCriticalSection(&_csLock);
	}

	WinHost::~WinHost(){
        if (m_toolTip){
            m_native->removeControl(m_toolTip);
            delete m_toolTip;
			m_toolTip = 0;
        }
		m_hImc = 0;
		m_hWnd = 0;
		m_native = 0;
		::DeleteCriticalSection(&_csLock);
	}

	HWND WinHost::getHWnd(){
		return m_hWnd;
	}

	void WinHost::setHWnd(HWND hWnd){
		m_hWnd = hWnd;
		m_hImc = ImmGetContext(m_hWnd);
		if(m_native){
			m_native->onResize();
		}
	}

	FCNative* WinHost::getNative(){
		return m_native;
	}

	void WinHost::setNative(FCNative *native){
		m_native = native;
	}

	int WinHost::getPInvokeMsgID(){
		return m_pInvokeMsgID;
	}

	void WinHost::setPInvokeMsgID(int pInvokeMsgID){
		m_pInvokeMsgID = pInvokeMsgID;
	}

	FCView* WinHost::getToolTip(){
		return m_toolTip;
	}

	void WinHost::setToolTip(FCView *toolTip){
		m_toolTip = toolTip;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void WinHost::activeMirror(){
		if (m_native->getMirrorMode() != FCMirrorMode_None){
            ArrayList<FCView*> controls = m_native->getControls();
            int controlsSize = (int)controls.size();
            for (int i = 0; i < controlsSize; i++){
                FCView *control = controls.get(i);
                if (control->getNative() != m_native){
                    control->setNative(m_native);
                }
            }
        }
	}

	bool WinHost::allowOperate(){
		return m_allowOperate;
	}

	bool WinHost::allowPartialPaint(){
		return m_allowPartialPaint;
	}

	void WinHost::beginInvoke(FCView *control, Object args){
		lock();
		m_invokeArgs[m_invokeSerialID] = args;
		m_invokeControls[m_invokeSerialID] = control;
		unLock();
		HWND hWnd = m_hWnd;
		if (m_native->getMirrorMode() != FCMirrorMode_None){
            WinHost *host = dynamic_cast<WinHost*>(m_native->getMirrorHost()->getHost());
			hWnd = host->getHWnd();
        }
        ::SendMessage(hWnd, m_pInvokeMsgID, m_invokeSerialID, 0);
        m_invokeSerialID++;
	}

	void WinHost::copy(string text){
		if (::OpenClipboard(0)){
			int textLength = (int)text.length();
			if(textLength > 0){
				HANDLE hClip;  
				char *pBuf;  
				EmptyClipboard();
				hClip = GlobalAlloc(GMEM_MOVEABLE, text.length() + 1);  
				pBuf = (char*)GlobalLock(hClip);  
				strcpy_s(pBuf, text.length() + 1, text.c_str());  
				GlobalUnlock(hClip);
				::SetClipboardData(CF_TEXT, hClip);
				CloseClipboard();
			}
		} 
	}

	FCView* WinHost::createInternalControl(FCView *parent, const String& clsid){
		FCCalendar *calendar = dynamic_cast<FCCalendar*>(parent);
        if (calendar){
            if (clsid == L"datetitle"){
                return new DateTitle(calendar);
            }
            else if (clsid == L"headdiv"){
                HeadDiv *headDiv = new HeadDiv(calendar);
                headDiv->setWidth(parent->getWidth());
                headDiv->setDock(FCDockStyle_Top);
                return headDiv;
            }
            else if (clsid == L"lastbutton"){
                return new ArrowButton(calendar);
            }
            else if (clsid == L"nextbutton"){
                ArrowButton *nextBtn = new ArrowButton(calendar);
                nextBtn->setToLast(false);
                return nextBtn;
            }
        }
        FCSplitLayoutDiv *splitLayoutDiv = dynamic_cast<FCSplitLayoutDiv*>(parent);
        if (splitLayoutDiv){
            if (clsid == L"splitter"){
                FCButton *splitter = new FCButton;
                FCSize size = {5, 5};
                splitter->setSize(size);
                return splitter;
            }
        }
        FCScrollBar *scrollBar = dynamic_cast<FCScrollBar*>(parent);
        if (scrollBar){
            if (clsid == L"addbutton"){
                FCButton *addButton = new FCButton;
                FCSize size = {15, 15};
                addButton->setSize(size);
                return addButton;
            }
            else if (clsid == L"backbutton"){
                return new FCButton();
            }
            else if (clsid == L"scrollbutton"){
                FCButton *scrollButton = new FCButton;
                scrollButton->setAllowDrag(true);
                return scrollButton;
            }
            else if (clsid == L"reducebutton"){
                FCButton *reduceButton = new FCButton;
                FCSize size = {15, 15};
                reduceButton->setSize(size);
                return reduceButton;
            }
        }
        FCTabPage *tabPage = dynamic_cast<FCTabPage*>(parent);
        if (tabPage){
            if (clsid == L"headerbutton"){
                FCButton *button = new FCButton;
                button->setAllowDrag(true);
                FCSize size = {100, 20};
                button->setSize(size);
                return button;
            }
        }
        FCComboBox *comboBox = dynamic_cast<FCComboBox*>(parent);
        if (comboBox){
            if (clsid == L"dropdownbutton"){
                FCButton *dropDownButton = new FCButton;
				dropDownButton->setDisplayOffset(false);
                int width = comboBox->getWidth();
                int height = comboBox->getHeight();
                FCPoint location = {width - 16, 0};
                dropDownButton->setLocation(location);
                FCSize size = {16, height};
                dropDownButton->setSize(size);
                return dropDownButton;
            }
            else if (clsid == L"dropdownmenu"){
                FCComboBoxMenu *comboBoxMenu = new FCComboBoxMenu;
                comboBoxMenu->setComboBox(comboBox);
                comboBoxMenu->setPopup(true);
                FCSize size = {100, 200};
                comboBoxMenu->setSize(size);
                return comboBoxMenu;
            }
        }
        FCDateTimePicker *datePicker = dynamic_cast<FCDateTimePicker*>(parent);
        if (datePicker){
            if (clsid == L"dropdownbutton"){
                FCButton *dropDownButton = new FCButton;
                dropDownButton->setDisplayOffset(false);
                int width = datePicker->getWidth();
                int height = datePicker->getHeight();
                FCPoint location = {width - 16, 0};
                dropDownButton->setLocation(location);
                FCSize size = {16, height};
                dropDownButton->setSize(size);
                return dropDownButton;
            }
            else if (clsid == L"dropdownmenu"){
                FCMenu *dropDownMenu = new FCMenu();
                FCPadding padding(1);
                dropDownMenu->setPadding(padding);
                dropDownMenu->setPopup(true);
                FCSize size = {200, 200};
                dropDownMenu->setSize(size);
                return dropDownMenu;
            }
        }
        FCSpin *spin = dynamic_cast<FCSpin*>(parent);
        if (spin){
            if (clsid == L"downbutton"){
                FCButton *downButton = new FCButton;
				downButton->setDisplayOffset(false);
                FCSize size = {16, 16};
                downButton->setSize(size);
                return downButton;
            }
            else if (clsid == L"upbutton"){
                FCButton *upButton = new FCButton;
				upButton->setDisplayOffset(false);
                FCSize size = {16, 16};
                upButton->setSize(size);
                return upButton;
            }
        }
		FCDiv *div = dynamic_cast<FCDiv*>(parent);
        if (div){
            if (clsid == L"hscrollbar"){
                FCHScrollBar *hScrollBar = new FCHScrollBar;
                hScrollBar->setVisible(false);
                FCSize size = {15, 15};
                hScrollBar->setSize(size);
                return hScrollBar;
            }
            else if (clsid == L"vscrollbar"){
                FCVScrollBar *vScrollBar = new FCVScrollBar;
                vScrollBar->setVisible(false);
                FCSize size = {15, 15};
                vScrollBar->setSize(size);
                return vScrollBar;
            }
        }
		FCGrid *grid = dynamic_cast<FCGrid*>(parent);
		if(grid){
			if(clsid == L"edittextbox"){
				return new FCTextBox;
			}
		}
        return 0;
	}

	string WinHost::getAppPath(){
		char exeFullPath[MAX_PATH]; 
		string strPath = "";
		GetModuleFileNameA(0, exeFullPath, MAX_PATH);
		strPath = (string)exeFullPath; 
		int pos = (int)strPath.rfind('\\', strPath.length());
		return strPath.substr(0, pos);
	}

	FCCursors WinHost::getCursor(){
		HCURSOR cursor = ::GetCursor();
		if(cursor == ::LoadCursor(0, IDC_ARROW)){
			return FCCursors_Arrow;
		}
		else if(cursor == ::LoadCursor(0, IDC_APPSTARTING)){
			return FCCursors_AppStarting;
		}
		else if(cursor == ::LoadCursor(0, IDC_CROSS)){
			return FCCursors_Cross;
		}
		else if(cursor == ::LoadCursor(0, IDC_HAND)){
			return FCCursors_Hand;
		}
		else if(cursor == ::LoadCursor(0, IDC_HELP)){
			return FCCursors_Help;
		}
		else if(cursor == ::LoadCursor(0, IDC_IBEAM)){
			return FCCursors_IBeam;
		}
		else if(cursor == ::LoadCursor(0, IDC_NO)){
			return FCCursors_No;
		}
		else if(cursor == ::LoadCursor(0, IDC_SIZEALL)){
			return FCCursors_SizeAll;
		}
		else if(cursor == ::LoadCursor(0, IDC_SIZENESW)){
			return FCCursors_SizeNESW;
		}
		else if(cursor == ::LoadCursor(0, IDC_SIZENS)){
			return FCCursors_SizeNS;
		}
		else if(cursor == ::LoadCursor(0, IDC_SIZENWSE)){
			return FCCursors_SizeNWSE;
		}
		else if(cursor == ::LoadCursor(0, IDC_SIZEWE)){
			return FCCursors_SizeWE;
		}
		else if(cursor == ::LoadCursor(0, IDC_UPARROW)){
			return FCCursors_UpArrow;
		}
		else if(cursor == ::LoadCursor(0, IDC_WAIT)){
			return FCCursors_WaitCursor;
		}
		return FCCursors_Arrow;
	}

	int WinHost::getIntersectRect(LPRECT lpDestRect, const FCRect *lpSrc1Rect, const FCRect *lpSrc2Rect){
		return ::IntersectRect(lpDestRect, lpSrc1Rect, lpSrc2Rect);
	}

	FCPoint WinHost::getTouchPoint(){
		FCPoint mp = {0}; 
		if(m_hWnd){
			GetCursorPos(&mp);
			::ScreenToClient(m_hWnd, &mp);
			if (m_native->allowScaleSize()){
				FCSize clientSize = getClientSize();
				if (clientSize.cx > 0 && clientSize.cy > 0){
					FCSize scaleSize = m_native->getScaleSize();
					mp.x = mp.x * scaleSize.cx / clientSize.cx;
					mp.y = mp.y * scaleSize.cy / clientSize.cy;
				}
			}
		}
		return mp;
	}

	FCSize WinHost::getSize(){
        if (m_native->allowScaleSize()){
            return m_native->getScaleSize();
        }
        else{
            return getClientSize();
        }
	}

	int WinHost::getUnionRect(LPRECT lpDestRect, const FCRect *lpSrc1Rect, const FCRect *lpSrc2Rect){
		return ::UnionRect(lpDestRect, lpSrc1Rect, lpSrc2Rect);
	}

	void WinHost::invalidate(){
		if(m_hWnd){
			FCSize clientSize = getClientSize();
			FCRect bounds = {0, 0, clientSize.cx, clientSize.cy};
			::InvalidateRect(m_hWnd, &bounds, false);
		}
	}

	void WinHost::invalidate(const FCRect& rect){
		if(m_allowPartialPaint){
			HDC hDC = GetDC(m_hWnd);
			onPaint(hDC, rect);
			::ReleaseDC(m_hWnd, hDC);
		}
		else{
			invalidate();
		}
	}

	void WinHost::invoke(FCView *control, Object args){
		lock();
		m_invokeArgs[m_invokeSerialID] = args;
		m_invokeControls[m_invokeSerialID] = control;
		unLock();
		HWND hWnd = m_hWnd;
		if (m_native->getMirrorMode() != FCMirrorMode_None){
            WinHost *host = dynamic_cast<WinHost*>(m_native->getMirrorHost()->getHost());
			hWnd = host->getHWnd();
        }
		::PostMessage(hWnd, m_pInvokeMsgID, m_invokeSerialID, 0);
        m_invokeSerialID++;
	}

	bool WinHost::isKeyPress(char key){
		return (::GetKeyState(key) & (int)0x8000) > 0 ? 1 : 0;
	}

	void WinHost::lock(){
		::EnterCriticalSection(&_csLock);
	}

	void WinHost::onInvoke(int invokeSerialID){
        Object args = 0;
        FCView *control = 0;
		lock();
		map<int, Object>::iterator sIter = m_invokeArgs.find(invokeSerialID);
		if(sIter != m_invokeArgs.end()){
			args = sIter->second;
			m_invokeArgs.erase(sIter);
		}
		map<int, FCView*>::iterator sIter2 = m_invokeControls.find(invokeSerialID);
		if(sIter2 != m_invokeControls.end()){
			control = sIter2->second;
			m_invokeControls.erase(sIter2);
		}
		unLock();
		if(control){
			control->onInvoke(args);
		}
	}

	int WinHost::onMessage(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam){
		if(m_native){
			if(message == WM_IME_SETCONTEXT && wParam == 1){
				::ImmAssociateContext(m_hWnd, m_hImc);
			}
			if(message == m_pInvokeMsgID){
				onInvoke((int)wParam);
			}
			switch(message){
			case WM_CHAR:{
					if(m_allowOperate){
						wchar_t key = (wchar_t)(int)wParam;
						if(m_native->onChar(key)){
							return 1;
						}
					}
					break;
				}
			case WM_ERASEBKGND:
				return 1;
			case WM_IME_CHAR:{
					if(m_allowOperate){
						if (wParam == PM_REMOVE){
							FCView *focusedControl = m_native->getFocusedControl();
							if(focusedControl){
								int size = ::ImmGetCompositionString(m_hImc, GCS_COMPSTR, 0, 0);
								size += sizeof(char);
								String wstr;
								::ImmGetCompositionStringW(m_hImc, GCS_RESULTSTR, &wstr, size);
							}
						}
					}
					break;
				}
			case WM_LBUTTONDBLCLK:
				if(m_allowOperate){
					FCTouchInfo newTouchInfo;
                    newTouchInfo.m_firstTouch = true;
                    newTouchInfo.m_clicks = 2;
					m_native->onTouchDown(newTouchInfo);
					m_native->onDoubleClick(newTouchInfo);
				}
				break;
			case WM_LBUTTONDOWN:
				if(m_allowOperate){
					activeMirror();
					FCTouchInfo newTouchInfo;
                    newTouchInfo.m_firstTouch = true;
                    newTouchInfo.m_clicks = 1;
					m_native->onTouchDown(newTouchInfo);
				}
				break;
			case WM_LBUTTONUP:
				if(m_allowOperate){
					FCTouchInfo newTouchInfo;
                    newTouchInfo.m_firstTouch = true;
                    newTouchInfo.m_clicks = 1;
					m_native->onTouchUp(newTouchInfo);
				}
				break;
			case WM_KEYDOWN:
			case WM_SYSKEYDOWN:
				if(m_allowOperate){
					char key = (char)(int)wParam;
					bool handle = m_native->onPreviewsKeyEvent(FCEventID::KEYDOWN, key);
					if(handle){
						return 1;
					}
					else{
						m_native->onKeyDown(key);
					}
				}
				break;
			case WM_KEYUP:
			case WM_SYSKEYUP:
				if(m_allowOperate){
					char key = (char)(int)wParam;
					m_native->onKeyUp(key);
				}
				break;
			case WM_MOUSEMOVE:
				if(m_allowOperate){
					if(wParam == 1){
						FCTouchInfo newTouchInfo;
                        newTouchInfo.m_firstTouch = true;
                        newTouchInfo.m_clicks = 1;
						m_native->onTouchMove(newTouchInfo);
					}
					else if(wParam == 2){
						FCTouchInfo newTouchInfo;
                        newTouchInfo.m_secondTouch = true;
                        newTouchInfo.m_clicks = 1;
						m_native->onTouchMove(newTouchInfo);
					}
					else{
						FCTouchInfo newTouchInfo;
						m_native->onTouchMove(newTouchInfo);
					}
				}
				break;
			case WM_MOUSEWHEEL:{
					if(m_allowOperate){
						FCTouchInfo newTouchInfo;
                        newTouchInfo.m_delta = (int)wParam;
						m_native->onTouchWheel(newTouchInfo);
					}
					break;
				}
			case WM_PAINT:{
					PAINTSTRUCT ps;
					HDC hDC = BeginPaint(m_hWnd, &ps);
					FCSize displaySize = m_native->getDisplaySize();
					FCRect paintRect = {0, 0, displaySize.cx, displaySize.cy};
					onPaint(hDC, paintRect);
					EndPaint(m_hWnd, &ps);
					return 1;
				}
			case WM_RBUTTONDBLCLK:{
					if(m_allowOperate){
						FCTouchInfo newTouchInfo;
                        newTouchInfo.m_secondTouch = true;
                        newTouchInfo.m_clicks = 2;
						m_native->onTouchDown(newTouchInfo);
						m_native->onDoubleClick(newTouchInfo);
					}
					break;
				}
			case WM_RBUTTONDOWN:{
					if(m_allowOperate){
						activeMirror();
						FCTouchInfo newTouchInfo;
                        newTouchInfo.m_secondTouch = true;
                        newTouchInfo.m_clicks = 1;
						m_native->onTouchDown(newTouchInfo);
					}
					break;
				}
			case WM_RBUTTONUP:{
					if(m_allowOperate){
						FCTouchInfo newTouchInfo;
                        newTouchInfo.m_secondTouch = true;
                        newTouchInfo.m_clicks = 1;
						m_native->onTouchUp(newTouchInfo);
					}
					break;
				}
			case WM_SIZE:{
					int wmId = 0;
					wmId = LOWORD(wParam);
					if(wmId == SIZE_MINIMIZED){
					}
					else{
						m_native->onResize();
					}
					break;
				}
			case WM_TIMER:{
					if(hWnd && m_hWnd == hWnd){
						int timerID = (int)wParam;
						m_native->onTimer(timerID);
					}
					break;
				}
			}
		}
		return 0;
	}

	void WinHost::onPaint(HDC hDC, const FCRect& rect){
		FCSize displaySize = m_native->getDisplaySize();
		double scaleFactorX = 1, scaleFactorY = 1;
		FCSize clientSize = getClientSize();
        if (m_native->allowScaleSize()){
            if (clientSize.cx > 0 && clientSize.cy > 0){
				FCSize scaleSize = m_native->getScaleSize();
                scaleFactorX = (double)(clientSize.cx) / scaleSize.cx;
                scaleFactorY = (double)(clientSize.cy) / scaleSize.cy;
            }
        }
		FCPaint *paint = m_native->getPaint();
		FCRect wRect = {0, 0, clientSize.cx, clientSize.cy};
		paint->setScaleFactor(scaleFactorX, scaleFactorY);
		paint->beginPaint(hDC, wRect, rect);
		m_native->onPaint(rect);
		paint->endPaint();
	}

	string WinHost::paste(){
		if (OpenClipboard(0)){  
			if (IsClipboardFormatAvailable(CF_TEXT)){  
				HANDLE hClip;  
				char *pBuf;    
				hClip = GetClipboardData(CF_TEXT);  
				pBuf = (char*)GlobalLock(hClip);  
				GlobalUnlock(hClip);  
				CloseClipboard();
				string str = pBuf;
				if(str.length() > 0){
					return str;
				}
			}  
		}  
		return "";
	}

	void WinHost::setAllowOperate(bool allowOperate){
		m_allowOperate = allowOperate;
	}

	void WinHost::setAllowPartialPaint(bool allowPartialPaint){
		m_allowPartialPaint = allowPartialPaint;
	}

	void WinHost::setCursor(FCCursors cursor){
		if(cursor == FCCursors_Arrow){
			::SetCursor(::LoadCursor(0, IDC_ARROW));
		}
		else if(cursor == FCCursors_AppStarting){
			::SetCursor(::LoadCursor(0, IDC_APPSTARTING));
		}
		else if(cursor == FCCursors_Cross){
			::SetCursor(::LoadCursor(0, IDC_CROSS));
		}
		else if(cursor == FCCursors_Hand){
			::SetCursor(::LoadCursor(0, IDC_HAND));
		}
		else if(cursor == FCCursors_Help){
			::SetCursor(::LoadCursor(0, IDC_HELP));
		}
		else if(cursor == FCCursors_IBeam){
			::SetCursor(::LoadCursor(0, IDC_IBEAM));
		}
		else if(cursor == FCCursors_No){
			::SetCursor(::LoadCursor(0, IDC_NO));
		}
		else if(cursor == FCCursors_SizeAll){
			::SetCursor(::LoadCursor(0, IDC_SIZEALL));
		}
		else if(cursor == FCCursors_SizeNESW){
			::SetCursor(::LoadCursor(0, IDC_SIZENESW));
		}
		else if(cursor == FCCursors_SizeNS){
			::SetCursor(::LoadCursor(0, IDC_SIZENS));
		}
		else if(cursor == FCCursors_SizeNWSE){
			::SetCursor(::LoadCursor(0, IDC_SIZENWSE));
		}
		else if(cursor == FCCursors_SizeWE){
			::SetCursor(::LoadCursor(0, IDC_SIZEWE));
		}
		else if(cursor == FCCursors_UpArrow){
			::SetCursor(::LoadCursor(0, IDC_UPARROW));
		}
		else if(cursor == FCCursors_WaitCursor){
			::SetCursor(::LoadCursor(0, IDC_WAIT));
		}
	}

	void WinHost::showToolTip(const String& text, const FCPoint& mp){
		if (m_toolTip){
            m_toolTip->setNative(m_native);
			m_toolTip->setLocation(mp);
            m_toolTip->setText(text);
            m_toolTip->show();
        }
	}

	void WinHost::startTimer(int timerID, int interval){
		HWND hWnd = m_hWnd;
		if (m_native->getMirrorMode() != FCMirrorMode_None){
            WinHost *host = dynamic_cast<WinHost*>(m_native->getMirrorHost()->getHost());
			hWnd = host->getHWnd();
        }
		::SetTimer(hWnd, timerID, interval, 0);
	}

	void WinHost::stopTimer(int timerID){
		HWND hWnd = m_hWnd;
		if (m_native->getMirrorMode() != FCMirrorMode_None){
            WinHost *host = dynamic_cast<WinHost*>(m_native->getMirrorHost()->getHost());
			hWnd = host->getHWnd();
        }
		::KillTimer(hWnd, timerID);
	}

	void WinHost::unLock(){
		::LeaveCriticalSection(&_csLock);
	}
}