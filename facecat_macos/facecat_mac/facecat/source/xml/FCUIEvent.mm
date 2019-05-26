#include "stdafx.h"
#include "FCUIEvent.h"

namespace FaceCat{
	FCEventInfo::FCEventInfo(){
	}
    
	FCEventInfo::~FCEventInfo(){
		m_functions.clear();
	}
    
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
	void FCEventInfo::addEvent(int eventID, const String& function){
		m_functions.put(eventID, function);
	}
    
	String FCEventInfo::getFunction(int eventID){
        if(m_functions.containsKey(eventID)){
            return m_functions.get(eventID);
        }
		return L"";
	}
    
	void FCEventInfo::removeEvent(int eventID){
        m_functions.remove(eventID);
	}
    
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
	FCUIEvent::FCUIEvent(FCUIXml *xml){
		m_script = 0;
		m_sender = L"";
		m_xml = xml;
	}
    
	FCUIEvent::~FCUIEvent(){
		map<FCView*, FCEventInfo*>::iterator sIter = m_events.begin();
		for(; sIter != m_events.end(); ++sIter){
			delete sIter->second;
		}
		m_events.clear();
		m_script = 0;
		m_xml = 0;
	}
    
	FCUIScript* FCUIEvent::getScript(){
		return m_script;
	}
    
	void FCUIEvent::setScript(FCUIScript *script){
		m_script = script;
	}
    
	String FCUIEvent::getSender(){
		return m_sender;
	}
    
	void FCUIEvent::setSender(const String& sender){
		m_sender = sender;
	}
    
	FCUIXml* FCUIEvent::getXml(){
		return m_xml;
	}
    
	void FCUIEvent::setXml(FCUIXml *xml){
		m_xml = xml;
	}
    
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    void FCUIEvent::callAdd(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::ADD, pInvoke);
    }
    
    void FCUIEvent::callBackColorChanged(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::BACKCOLORCHANGED, pInvoke);
    }
    
    void FCUIEvent::callBackImageChanged(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::BACKIMAGECHANGED, pInvoke);
    }
    
    void FCUIEvent::callChar(Object sender, char ch, Object pInvoke){
        callFunction(sender, FCEventID::CHAR, pInvoke);
    }
    
    void FCUIEvent::callCheckedChanged(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::CHECKEDCHANGED, pInvoke);
    }
    
    void FCUIEvent::callClick(Object sender, FCTouchInfo touchInfo, Object pInvoke){
        callFunction(sender, FCEventID::CLICK, pInvoke);
    }
    
    void FCUIEvent::callCopy(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::COPY, pInvoke);
    }
    
    void FCUIEvent::callCut(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::CUT, pInvoke);
    }
    
    void FCUIEvent::callDockChanged(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::DOCKCHANGED, pInvoke);
    }
    
    void FCUIEvent::callDoubleClick(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::DOUBLECLICK, pInvoke);
    }
    
    void FCUIEvent::callDragBegin(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::DRAGBEGIN, pInvoke);
    }
    
    void FCUIEvent::callDragEnd(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::DRAGEND, pInvoke);
    }
    
    void FCUIEvent::callDragging(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::DRAGGING, pInvoke);
    }
    
    void FCUIEvent::callEnableChanged(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::ENABLECHANGED, pInvoke);
    }
    
    String FCUIEvent::callFunction(Object sender, int eventID, Object pInvoke){
        FCUIEvent *uiEvent = (FCUIEvent*)pInvoke;
        if(uiEvent){
            FCView *control = (FCView*)sender;
            map<FCView*, FCEventInfo*>::iterator sIter = uiEvent->m_events.find(control);
            if(sIter != uiEvent->m_events.end()){
                FCEventInfo *eventInfo = sIter->second;
                String function = eventInfo->getFunction(eventID);
                if (function.length() > 0){
                    FCUIScript *script = uiEvent->getXml()->getScript();
                    if (script){
                        uiEvent->setSender(control->getName());
                        String result = script->callFunction(function);
                        uiEvent->setSender(L"");
                        return result;
                    }
                }
            }
        }
        return L"";
    }
    
    
    void FCUIEvent::callFontChanged(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::FONTCHANGED, pInvoke);
    }
    
    void FCUIEvent::callGotFocus(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::GOTFOCUS, pInvoke);
    }
    
    void FCUIEvent::callGridCellClick(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::GRIDCELLCLICK, pInvoke);
    }
    
    void FCUIEvent::callGridCellEditBegin(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::GRIDCELLEDITBEGIN, pInvoke);
    }
    
    void FCUIEvent::callGridCellEditEnd(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::GRIDCELLEDITEND, pInvoke);
    }
    
    void FCUIEvent::callGridCellTouchDown(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::GRIDCELLTOUCHDOWN, pInvoke);
    }
    
    void FCUIEvent::callGridCellTouchMove(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::GRIDCELLTOUCHMOVE, pInvoke);
    }
    
    void FCUIEvent::callGridCellTouchUp(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::GRIDCELLTOUCHUP, pInvoke);
    }
    
    void FCUIEvent::callInvoke(Object sender, Object agrs, Object pInvoke){
        callFunction(sender, FCEventID::INVOKE, pInvoke);
    }
    
    void FCUIEvent::callKeyDown(Object sender, char key, Object pInvoke){
        callFunction(sender, FCEventID::KEYDOWN, pInvoke);
    }
    
    void FCUIEvent::callKeyUp(Object sender, char key, Object pInvoke){
        callFunction(sender, FCEventID::KEYUP, pInvoke);
    }
    
    void FCUIEvent::callLoad(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::LOAD, pInvoke);
    }
    
    void FCUIEvent::callLocationChanged(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::LOCATIONCHANGED, pInvoke);
    }
    
    void FCUIEvent::callLostFocus(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::LOSTFOCUS, pInvoke);
    }
    
    void FCUIEvent::callMarginChanged(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::MARGINCHANGED, pInvoke);
    }
    
    void FCUIEvent::callMenuItemClick(Object sender, FCMenuItem *item, FCTouchInfo touchInfo, Object pInvoke){
        callFunction(sender, FCEventID::MENUITEMCLICK, pInvoke);
    }
    
    void FCUIEvent::callTouchDown(Object sender, FCTouchInfo touchInfo, Object pInvoke){
        callFunction(sender, FCEventID::TOUCHDOWN, pInvoke);
    }
    
    void FCUIEvent::callTouchEnter(Object sender, FCTouchInfo touchInfo, Object pInvoke){
        callFunction(sender, FCEventID::TOUCHENTER, pInvoke);
    }
    
    void FCUIEvent::callTouchLeave(Object sender, FCTouchInfo touchInfo, Object pInvoke){
        callFunction(sender, FCEventID::TOUCHLEAVE, pInvoke);
    }
    
    void FCUIEvent::callTouchMove(Object sender, FCTouchInfo touchInfo, Object pInvoke){
        callFunction(sender, FCEventID::TOUCHMOVE, pInvoke);
    }
    
    void FCUIEvent::callTouchUp(Object sender, FCTouchInfo touchInfo, Object pInvoke){
        callFunction(sender, FCEventID::TOUCHUP, pInvoke);
    }
    
    void FCUIEvent::callTouchWheel(Object sender, FCTouchInfo touchInfo, Object pInvoke){
        callFunction(sender, FCEventID::TOUCHWHEEL, pInvoke);
    }
    
    void FCUIEvent::callPaddingChanged(Object sender, FCPaint *paint, const FCRect& clipRect, Object pInvoke){
        callFunction(sender, FCEventID::PADDINGCHANGED, pInvoke);
    }
    
    void FCUIEvent::callPaint(Object sender, FCPaint *paint, const FCRect& clipRect, Object pInvoke){
        callFunction(sender, FCEventID::PAINT, pInvoke);
    }
    
    void FCUIEvent::callPaintBorder(Object sender, FCPaint *paint, const FCRect& clipRect, Object pInvoke){
        callFunction(sender, FCEventID::PAINTBORDER, pInvoke);
    }
    
    void FCUIEvent::callParentChanged(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::PARENTCHANGED, pInvoke);
    }
    
    void FCUIEvent::callPaste(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::PASTE, pInvoke);
    }
    
    void FCUIEvent::callRegionChanged(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::REGIONCHANGED, pInvoke);
    }
    
    void FCUIEvent::callRemove(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::REMOVE, pInvoke);
    }
    
    void FCUIEvent::callScrolled(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::SCROLLED, pInvoke);
    }
    
    void FCUIEvent::callSelectedTimeChanged(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::SELECTEDTIMECHANGED, pInvoke);
    }
    
    void FCUIEvent::callSelectedIndexChanged(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::SELECTEDINDEXCHANGED, pInvoke);
    }
    
    void FCUIEvent::callSelectedTabPageChanged(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::SELECTEDTABPAGECHANGED, pInvoke);
    }
    
    void FCUIEvent::callSizeChanged(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::SIZECHANGED, pInvoke);
    }
    
    void FCUIEvent::callTabIndexChanged(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::TABINDEXCHANGED, pInvoke);
    }
    
    void FCUIEvent::callTabStop(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::TABSTOP, pInvoke);
    }
    
    void FCUIEvent::callTextChanged(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::TEXTCHANGED, pInvoke);
    }
    
    void FCUIEvent::callTextColorChanged(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::TEXTCOLORCHANGED, pInvoke);
    }
    
    void FCUIEvent::callTimer(Object sender, int timerID, Object pInvoke){
        callFunction(sender, FCEventID::TIMER, pInvoke);
    }
    
    void FCUIEvent::callVisibleChanged(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::VISIBLECHANGED, pInvoke);
    }
    
    void FCUIEvent::callValueChanged(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::VALUECHANGED, pInvoke);
    }
    
    void FCUIEvent::callWindowClosed(Object sender, Object pInvoke){
        callFunction(sender, FCEventID::WINDOWCLOSED, pInvoke);
    }
    
    int FCUIEvent::getEventID(const String& eventName){
        String lowerName = FCStr::toLower(eventName);
        if (lowerName == L"onadd") return FCEventID::ADD;
        else if (lowerName == L"onbackcolorchanged") return FCEventID::BACKCOLORCHANGED;
        else if (lowerName == L"onbackimagechanged") return FCEventID::BACKIMAGECHANGED;
        else if (lowerName == L"onchar") return FCEventID::CHAR;
        else if (lowerName == L"oncheckedchanged") return FCEventID::CHECKEDCHANGED;
        else if (lowerName == L"onclick") return FCEventID::CLICK;
        else if (lowerName == L"oncopy") return FCEventID::COPY;
        else if (lowerName == L"oncut") return FCEventID::CUT;
        else if (lowerName == L"ondockchanged") return FCEventID::DOCKCHANGED;
        else if (lowerName == L"ondoubleclick") return FCEventID::DOUBLECLICK;
        else if (lowerName == L"ondragbegin") return FCEventID::DRAGBEGIN;
        else if (lowerName == L"ondragend") return FCEventID::DRAGEND;
        else if (lowerName == L"ondragging") return FCEventID::DRAGGING;
        else if (lowerName == L"onenablechanged") return FCEventID::ENABLECHANGED;
        else if (lowerName == L"onfontchanged") return FCEventID::FONTCHANGED;
        else if (lowerName == L"ontextcolorchanged") return FCEventID::TEXTCOLORCHANGED;
        else if (lowerName == L"ongotfocus") return FCEventID::GOTFOCUS;
        else if (lowerName == L"ongridcellclick") return FCEventID::GRIDCELLCLICK;
        else if (lowerName == L"ongridcelleditbegin") return FCEventID::GRIDCELLEDITBEGIN;
        else if (lowerName == L"ongridcelleditend") return FCEventID::GRIDCELLEDITEND;
        else if (lowerName == L"ongridcelltouchdown") return FCEventID::GRIDCELLTOUCHDOWN;
        else if (lowerName == L"ongridcelltouchmove") return FCEventID::GRIDCELLTOUCHMOVE;
        else if (lowerName == L"ongridcelltouchup") return FCEventID::GRIDCELLTOUCHUP;
        else if (lowerName == L"oninvoke") return FCEventID::INVOKE;
        else if (lowerName == L"onkeydown") return FCEventID::KEYDOWN;
        else if (lowerName == L"onkeyup") return FCEventID::KEYUP;
        else if (lowerName == L"onload") return FCEventID::LOAD;
        else if (lowerName == L"onlocationchanged") return FCEventID::LOCATIONCHANGED;
        else if (lowerName == L"onlostfocus") return FCEventID::LOSTFOCUS;
        else if (lowerName == L"onmarginchanged") return FCEventID::MARGINCHANGED;
        else if (lowerName == L"onmenuitemclick") return FCEventID::MENUITEMCLICK;
        else if (lowerName == L"ontouchdown") return FCEventID::TOUCHDOWN;
        else if (lowerName == L"ontouchenter") return FCEventID::TOUCHENTER;
        else if (lowerName == L"ontouchleave") return FCEventID::TOUCHLEAVE;
        else if (lowerName == L"ontouchmove") return FCEventID::TOUCHMOVE;
        else if (lowerName == L"ontouchup") return FCEventID::TOUCHUP;
        else if (lowerName == L"ontouchwheel") return FCEventID::TOUCHWHEEL;
        else if (lowerName == L"onpaddingchanged") return FCEventID::PADDINGCHANGED;
        else if (lowerName == L"onpaint") return FCEventID::PAINT;
        else if (lowerName == L"onpaintborder") return FCEventID::PAINTBORDER;
        else if (lowerName == L"onparentchanged") return FCEventID::PARENTCHANGED;
        else if (lowerName == L"onpaste") return FCEventID::PASTE;
        else if (lowerName == L"onregionchanged") return FCEventID::REGIONCHANGED;
        else if (lowerName == L"onremove") return FCEventID::REMOVE;
        else if (lowerName == L"onselectedtimechanged") return FCEventID::SELECTEDTIMECHANGED;
        else if (lowerName == L"onselectedindexchanged") return FCEventID::SELECTEDINDEXCHANGED;
        else if (lowerName == L"onselectedtabpagechanged") return FCEventID::SELECTEDTABPAGECHANGED;
        else if (lowerName == L"onsizechanged") return FCEventID::SIZECHANGED;
        else if (lowerName == L"ontabindexchanged") return FCEventID::TABINDEXCHANGED;
        else if (lowerName == L"ontabstop") return FCEventID::TABSTOP;
        else if (lowerName == L"ontextchanged") return FCEventID::TEXTCHANGED;
        else if (lowerName == L"ontimer") return FCEventID::TIMER;
        else if (lowerName == L"onvaluechanged") return FCEventID::VALUECHANGED;
        else if (lowerName == L"onvisiblechanged") return FCEventID::VISIBLECHANGED;
        else if (lowerName == L"onscrolled") return FCEventID::SCROLLED;
        else if (lowerName == L"onwindowclosed") return FCEventID::WINDOWCLOSED;
        return -1;
    }
    
    void FCUIEvent::addEvent(FCView *control, const String& eventName, const String& function){
        int eventID = getEventID(eventName);
        if (eventID != -1){
            FCEventInfo *eventInfo = 0;
            map<FCView*, FCEventInfo*>::iterator sIter = m_events.find(control);
            if(sIter != m_events.end()){
                eventInfo = sIter->second;
            }
            else{
                eventInfo = new FCEventInfo();
                m_events[control] = eventInfo;
            }
            eventInfo->addEvent(eventID, function);
            switch (eventID){
                case FCEventID::ADD:{
                    FCEvent cEvent = &callAdd;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::BACKCOLORCHANGED:{
                    FCEvent cEvent = &callBackColorChanged;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::BACKIMAGECHANGED:{
                    FCEvent cEvent = &callBackImageChanged;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::CHAR:{
                    FCKeyEvent cEvent= &callChar;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::CHECKEDCHANGED:{
                    FCEvent cEvent = &callCheckedChanged;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::CLICK:{
                    FCTouchEvent cEvent= &callClick;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::COPY:{
                    FCEvent cEvent = &callCopy;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::CUT:{
                    FCEvent cEvent = &callCut;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::DOCKCHANGED:{
                    FCEvent cEvent = &callDockChanged;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::DOUBLECLICK:{
                    FCEvent cEvent = &callDoubleClick;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::DRAGBEGIN:{
                    FCEvent cEvent = &callDragBegin;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::DRAGEND:{
                    FCEvent cEvent = &callDragEnd;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::DRAGGING:{
                    FCEvent cEvent = &callDragging;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::ENABLECHANGED:{
                    FCEvent cEvent = &callEnableChanged;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::FONTCHANGED:{
                    FCEvent cEvent = &callFontChanged;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::TEXTCOLORCHANGED:{
                    FCEvent cEvent = &callTextColorChanged;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::GOTFOCUS:{
                    FCEvent cEvent = &callGotFocus;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::GRIDCELLCLICK:{
                    FCEvent cEvent = &callGridCellClick;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::GRIDCELLEDITBEGIN:{
                    FCEvent cEvent = &callGridCellEditBegin;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::GRIDCELLEDITEND:{
                    FCEvent cEvent = &callGridCellEditEnd;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::GRIDCELLTOUCHDOWN:{
                    FCEvent cEvent = &callGridCellTouchDown;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::GRIDCELLTOUCHMOVE:{
                    FCEvent cEvent = &callGridCellTouchMove;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::GRIDCELLTOUCHUP:{
                    FCEvent cEvent = &callGridCellTouchUp;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::INVOKE:{
                    FCInvokeEvent cEvent = &callInvoke;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::KEYDOWN:{
                    FCKeyEvent cEvent= &callKeyDown;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::KEYUP:{
                    FCKeyEvent cEvent= &callKeyUp;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::LOAD:{
                    FCEvent cEvent = &callLoad;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::LOCATIONCHANGED:{
                    FCEvent cEvent = &callLocationChanged;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::LOSTFOCUS:{
                    FCEvent cEvent = &callLostFocus;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::MARGINCHANGED:{
                    FCEvent cEvent = &callMarginChanged;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::TOUCHDOWN:{
                    FCTouchEvent cEvent= &callTouchDown;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::TOUCHENTER:{
                    FCTouchEvent cEvent= &callTouchEnter;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::TOUCHLEAVE:{
                    FCTouchEvent cEvent= &callTouchLeave;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::TOUCHMOVE:{
                    FCTouchEvent cEvent= &callTouchMove;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::TOUCHUP:{
                    FCTouchEvent cEvent= &callTouchUp;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::TOUCHWHEEL:{
                    FCTouchEvent cEvent= &callTouchWheel;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::PADDINGCHANGED:{
                    FCPaintEvent cEvent = &callPaddingChanged;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::PAINT:{
                    FCPaintEvent cEvent = &callPaint;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::PAINTBORDER:{
                    FCPaintEvent cEvent = &callPaintBorder;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::PARENTCHANGED:{
                    FCEvent cEvent = &callParentChanged;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::PASTE:{
                    FCEvent cEvent = &callPaste;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::REGIONCHANGED:{
                    FCEvent cEvent = &callRegionChanged;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::REMOVE:{
                    FCEvent cEvent = &callRemove;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::SCROLLED:{
                    FCEvent cEvent = &callScrolled;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::SELECTEDTIMECHANGED:{
                    FCEvent cEvent = &callSelectedTimeChanged;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::SELECTEDINDEXCHANGED:{
                    FCEvent cEvent = &callSelectedIndexChanged;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::SELECTEDTABPAGECHANGED:{
                    FCEvent cEvent = &callSelectedTabPageChanged;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::SIZECHANGED:{
                    FCEvent cEvent = &callSizeChanged;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::TABINDEXCHANGED:{
                    FCEvent cEvent = &callTabIndexChanged;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::TABSTOP:{
                    FCEvent cEvent = &callTabStop;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::TEXTCHANGED:{
                    FCEvent cEvent = &callTextChanged;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::TIMER:{
                    FCTimerEvent cEvent = &callTimer;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::VALUECHANGED:{
                    FCEvent cEvent = &callVisibleChanged;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::VISIBLECHANGED:{
                    FCEvent cEvent = &callVisibleChanged;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
                case FCEventID::WINDOWCLOSED:{
                    FCEvent cEvent = &callWindowClosed;
                    control->addEvent((Object)cEvent, eventID, this);
                    break;
                }
            }
        }
	}
}
