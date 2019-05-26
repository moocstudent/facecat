#include "..\\..\\stdafx.h"
#include "..\\..\\include\\input\\FCDateTimePicker.h"

namespace FaceCat{
	void FCDateTimePicker::dropDownButtonTouchDown(Object sender, FCTouchInfo touchInfo, Object pInvoke){
		FCDateTimePicker *datePicker = (FCDateTimePicker*)pInvoke;
		if(datePicker){
			datePicker->onDropDownOpening();
		}
	}

	void FCDateTimePicker::selectedTimeChanged(Object sender, Object pInvoke){
		FCDateTimePicker *datePicker = (FCDateTimePicker*)pInvoke;
		if(datePicker){
			datePicker->onSelectedTimeChanged();
		}
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	FCDateTimePicker::FCDateTimePicker(){
		m_calendar = 0;
		m_customFormat = L"yyyy-MM-dd";
		m_dropDownButton = 0;
		m_dropDownButtonTouchDownEvent = dropDownButtonTouchDown;
		m_dropDownMenu = 0;
		m_selectedTimeChangedEvent = selectedTimeChanged;
		m_showTime = true;
	}

	FCDateTimePicker::~FCDateTimePicker(){
		if(m_calendar){
			if(m_selectedTimeChangedEvent){
				m_calendar->removeEvent(m_selectedTimeChangedEvent, FCEventID::SELECTEDTIMECHANGED);
				m_selectedTimeChangedEvent = 0;
			}
			m_calendar = 0;
		}
		if(m_dropDownButton){
			if(m_dropDownButtonTouchDownEvent){
				m_dropDownButton->removeEvent(m_dropDownButtonTouchDownEvent, FCEventID::TOUCHDOWN);
				m_dropDownButtonTouchDownEvent = 0;
			}
			m_dropDownButton = 0;
		}
	    if (m_dropDownMenu){
            getNative()->removeControl(m_dropDownMenu);
            delete m_dropDownMenu;
            m_dropDownMenu = 0;
        }
	}

	FCCalendar* FCDateTimePicker::getCalendar(){
		return m_calendar;
	}

	String FCDateTimePicker::getCustomFormat(){
		return m_customFormat;
	}

	void FCDateTimePicker::setCustomFormat(const String& customFormat){
		m_customFormat = customFormat;
	}

	FCButton* FCDateTimePicker::getDropDownButton(){
		return m_dropDownButton;
	}

	FCMenu* FCDateTimePicker::getDropDownMenu(){
		return m_dropDownMenu;
	}

	bool FCDateTimePicker::showTime(){
		return m_showTime;
	}

	void FCDateTimePicker::setShowTime(bool showTime){
		m_showTime = showTime;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	String FCDateTimePicker::getControlType(){
		return L"DateTimePicker";
	}

	void FCDateTimePicker::getProperty(const String& name, String *value, String *type){
		if(name == L"customformat"){
			*type = L"text";
			*value = getCustomFormat();
		}
		else if(name == L"showtime"){
			*type = L"bool";
			*value = FCStr::convertBoolToStr(showTime());
		}
		else{
			FCTextBox::getProperty(name, value, type);
		}
	}

	ArrayList<String> FCDateTimePicker::getPropertyNames(){
		ArrayList<String> propertyNames = FCTextBox::getPropertyNames();
		propertyNames.add(L"CustomFormat");
		propertyNames.add(L"ShowTime");
		return propertyNames;
	}

	void FCDateTimePicker::onDropDownOpening(){
		FCHost *host = getNative()->getHost();
		if (!m_dropDownMenu){
			m_dropDownMenu = dynamic_cast<FCMenu*>(host->createInternalControl(this, L"dropdownmenu"));
			getNative()->addControl(m_dropDownMenu);
			if(!m_calendar){
				m_calendar = new FCCalendar;
				m_dropDownMenu->addControl(m_calendar);
				m_calendar->setSize(m_dropDownMenu->getSize());
				m_calendar->addEvent(m_selectedTimeChangedEvent, FCEventID::SELECTEDTIMECHANGED, this);
			}
		}
		if(m_calendar && !m_showTime){
			m_calendar->getTimeDiv()->setHeight(0);
		}
		m_dropDownMenu->setNative(getNative());
		FCPoint popupPoint = {0, getHeight()};
	    FCPoint nativePoint = pointToNative(popupPoint);
        m_dropDownMenu->setLocation(nativePoint);
        m_dropDownMenu->setVisible(true);
        if (m_calendar){
            m_calendar->setMode(FCCalendarMode_Day);
        }
        m_dropDownMenu->bringToFront();
        m_dropDownMenu->invalidate();
	}
	
	void FCDateTimePicker::onLoad(){
		FCTextBox::onLoad();
		if (!m_dropDownButton){
			FCHost *host = getNative()->getHost();
			m_dropDownButton = dynamic_cast<FCButton*>(host->createInternalControl(this, L"dropdownbutton"));
			addControl(m_dropDownButton);
			m_dropDownButton->addEvent(m_dropDownButtonTouchDownEvent, FCEventID::TOUCHDOWN, this);
		}	
		if(m_dropDownMenu){
			m_dropDownMenu->setNative(getNative());
		}
	}

	void FCDateTimePicker::onSelectedTimeChanged(){
		callEvents(FCEventID::VALUECHANGED);
		if(m_calendar){
			CDay *selectedDay = m_calendar->getSelectedDay();
			if(selectedDay){
				setText(FCStr::getFormatDate(m_customFormat, selectedDay->getYear(), selectedDay->getMonth(), selectedDay->getDay(),
					m_calendar->getTimeDiv()->getHour(), m_calendar->getTimeDiv()->getMinute(), m_calendar->getTimeDiv()->getSecond()));
				invalidate();
			}
		}
	}

	void FCDateTimePicker::setProperty(const String& name, const String& value){
		if(name == L"customformat"){
			setCustomFormat(value);
		}
		else if(name == L"showtime"){
			setShowTime(FCStr::convertStrToBool(value));
		}
		else{
			FCTextBox::setProperty(name, value);
		}
	}

	void FCDateTimePicker::update(){
		int width = getWidth(), height = getHeight(), uBottom = 0;
		if (m_dropDownButton){
			int dWidth = m_dropDownButton->getWidth();
			FCPoint location = {width - dWidth, 0};
			m_dropDownButton->setLocation(location);
			FCSize size = {dWidth, height};
			m_dropDownButton->setSize(size);
			FCPadding padding(0, 0, dWidth, 0);
			setPadding(padding);
		}
	}
}