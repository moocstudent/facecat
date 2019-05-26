#include "..\\..\\stdafx.h"
#include "..\\..\\include\\input\\FCSpin.h"

namespace FaceCat{
	void FCSpin::downButtonTouchDown(Object sender, FCTouchInfo touchInfo, Object pInvoke){
		FCSpin *upDown = (FCSpin*)pInvoke;
		if(upDown){
			upDown->reduce();
			upDown->setIsReducing(true);
			upDown->invalidate();
		}
	}

	void FCSpin::downButtonTouchUp(Object sender, FCTouchInfo touchInfo, Object pInvoke){
		FCSpin *upDown = (FCSpin*)pInvoke;
		if(upDown){
			upDown->setIsReducing(false);
		}
	}

	void FCSpin::upButtonTouchDown(Object sender, FCTouchInfo touchInfo, Object pInvoke){
		FCSpin *upDown = (FCSpin*)pInvoke;
		if(upDown){
			upDown->add();
			upDown->setIsAdding(true);
			upDown->invalidate();
		}
	}

	void FCSpin::upButtonTouchUp(Object sender, FCTouchInfo touchInfo, Object pInvoke){
		FCSpin *upDown = (FCSpin*)pInvoke;
		if(upDown){
			upDown->setIsAdding(false);
		}
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	FCSpin::FCSpin(){
		m_autoFormat = true;
		m_digit = 0;
		m_downButton = 0;
		m_downButtonTouchDownEvent = downButtonTouchDown;
		m_downButtonTouchUpEvent = downButtonTouchUp;
		m_isAdding = false;
		m_isReducing = false;
		m_maximum = 100;
		m_minimum = 0;
		m_showThousands = false;
		m_step = 1;
		m_tick = 0;
		m_timerID = FCView::getNewTimerID();
		m_upButton = 0;
		m_upButtonTouchDownEvent = upButtonTouchDown;
		m_upButtonTouchUpEvent = upButtonTouchUp;
	}

	FCSpin::~FCSpin(){
		if (m_downButton){
			if(m_downButtonTouchDownEvent){
				m_downButton->removeEvent(m_downButtonTouchDownEvent, FCEventID::TOUCHDOWN);
				m_downButtonTouchDownEvent = 0;
			}
			if(m_downButtonTouchUpEvent){
				m_downButton->removeEvent(m_downButtonTouchUpEvent, FCEventID::TOUCHUP);
				m_downButtonTouchUpEvent = 0;
			}
			m_downButton = 0;
		}
		if(m_upButton){
			if(m_upButtonTouchDownEvent){
				m_upButton->removeEvent(m_upButtonTouchDownEvent, FCEventID::TOUCHDOWN);
				m_upButtonTouchDownEvent = 0;
			}
			if(m_upButtonTouchUpEvent){
				m_upButton->removeEvent(m_upButtonTouchUpEvent, FCEventID::TOUCHUP);
				m_upButtonTouchUpEvent = 0;
			}
			m_upButton = 0;
		}
	}

	bool FCSpin::autoFormat(){
		return m_autoFormat;
	}

	void FCSpin::setAutoFormat(bool autoFormat){
		m_autoFormat = autoFormat;
	}

	int FCSpin::getDigit(){
		return m_digit;
	}

	void FCSpin::setDigit(int digit){
		m_digit = digit;
		if (m_autoFormat){
			if (m_text == L""){
				m_text = L"0";
			}
			double text = FCStr::convertStrToDouble(m_text);
			setText(getValueByDigit(getValue(), m_digit));
		}
	}

	FCButton* FCSpin::getDownButton(){
		return m_downButton;
	}

	bool FCSpin::isAdding(){
		return m_isAdding;
	}

	void FCSpin::setIsAdding(bool isAdding){
		if(m_isAdding != isAdding){
			m_isAdding = isAdding;
			m_tick = 0;
			if(m_isAdding){
				startTimer(m_timerID, 10);
			}
			else{
				stopTimer(m_timerID);
			}
		}
	}

	bool FCSpin::isReducing(){
		return m_isReducing;
	}

	void FCSpin::setIsReducing(bool isReducing){
		if(m_isReducing != isReducing){
			m_isReducing = isReducing;
			m_tick = 0;
			if(m_isReducing){
				startTimer(m_timerID, 10);
			}
			else{
				stopTimer(m_timerID);
			}
		}
	}

	double FCSpin::getMaximum(){
		return m_maximum;
	}

	void FCSpin::setMaximum(double maximum){
		m_maximum = maximum;
		if (getValue() > maximum){
            		setValue(maximum);
        	}
	}

	double FCSpin::getMinimum(){
		return m_minimum;
	}

	void FCSpin::setMinimum(double minimum){
		m_minimum = minimum;
        	if (getValue() < minimum){
            		setValue(minimum);
        	}
	}

	bool FCSpin::showThousands(){
		return m_showThousands;
	}

	void FCSpin::setShowThousands(bool showThousands){
		m_showThousands = showThousands;
	}

	double FCSpin::getStep(){
		return m_step;
	}

	void FCSpin::setStep(double step){
		m_step = step;
	}

	void FCSpin::setText(const String& text){
		FCView::setText(formatNum(FCStr::replace(text, L",", L"")));
		onValueChanged();
	}

	FCButton* FCSpin::getUpButton(){
		return m_upButton;
	}

	double FCSpin::getValue(){
		return FCStr::convertStrToDouble(FCStr::replace(getText(), L",", L""));
	}

	void FCSpin::setValue(double value){
		if(value > m_maximum){
			value = m_maximum;
		}
		if(value < m_minimum){
			value = m_minimum;
		}
		double oldValue = getValue();
            setText(formatNum(getValueByDigit(value, m_digit)));
            onValueChanged();
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void FCSpin::add(){
		setValue(getValue() + m_step);
	}

	String FCSpin::formatNum(String inputText){
		if (m_showThousands){
			inputText = FCStr::replace(inputText, L",", L"");
            String theNewText = L"";
            int pos = 0;
            bool hasMinusSign = false;
            if (inputText.find(L"-") == 0){
                hasMinusSign = true;
                inputText = inputText.substr(1);
            }
            String textAfterDot = L"";
            bool hasDot = false;
			if (inputText.find(L".") != -1){
                hasDot = true;
				textAfterDot = inputText.substr(inputText.find(L".") + 1);
				inputText.erase(inputText.begin() + inputText.find(L"."));
            }
			pos = (int)inputText.length();
            while (pos >= 0){
                int logicPos = (int)inputText.length() - pos;
                if ((logicPos % 3) == 0 && logicPos > 1){
                    if (theNewText == L""){
                        theNewText = inputText.substr(pos, 3);
                    }
                    else{
                        theNewText = inputText.substr(pos, 3) + L"," + theNewText;
                    }
                }
                else{
                    if (pos == 0){
                        if (theNewText == L""){
                            theNewText = inputText.substr(pos, (logicPos % 3));
                        }
                        else{
                            theNewText = inputText.substr(pos, (logicPos % 3)) + L"," + theNewText;
                        }

                    }
                }
                --pos;
            }
            if (hasMinusSign){
                theNewText = L"-" + theNewText;
            }
            if (hasDot){
                theNewText = theNewText + L"." + textAfterDot;
            }
			if (theNewText.find(L".") == 0){
                theNewText = L"0" + theNewText;
            }
			if (theNewText.find(L"-.") == 0){
				theNewText = theNewText.insert(1, L"0");
            }
            return theNewText;
        }
        else{
            return inputText;
        }
	}

	String FCSpin::getControlType(){
		return L"Spin";
	}

	void FCSpin::getProperty(const String& name, String *value, String *type){
	    if (name == L"autoformat"){
            *type = L"int";
			*value = FCStr::convertBoolToStr(autoFormat());
        }
		else if(name == L"digit"){
			*type = L"int";
			*value = FCStr::convertDoubleToStr(getDigit());
		}
		else if(name == L"maximum"){
			*type = L"double";
			*value = FCStr::convertDoubleToStr(getMaximum());
		}
		else if(name == L"minimum"){
			*type = L"double";
			*value = FCStr::convertDoubleToStr(getMinimum());
		}
	    else if (name == L"showthousands"){
            *type = L"bool";
			*value = FCStr::convertBoolToStr(showThousands());
        }
		else if(name == L"step"){
			*type = L"double";
			*value = FCStr::convertDoubleToStr(getStep());
		}
		else if(name == L"value"){
			*type = L"double";
			*value = FCStr::convertDoubleToStr(getValue());
		}
		else{
			FCTextBox::getProperty(name, value, type);
		}
	}

	ArrayList<String> FCSpin::getPropertyNames(){
		ArrayList<String> propertyNames = FCTextBox::getPropertyNames();
		propertyNames.add(L"AutoFormat");
		propertyNames.add(L"Digit");
		propertyNames.add(L"Maximum");
		propertyNames.add(L"Minimum");
		propertyNames.add(L"ShowThousands");
		propertyNames.add(L"Step");
		return propertyNames;
	}

	String FCSpin::getValueByDigit(double value, int digit){
		wchar_t sz[1024] = {0};
		FCStr::getValueByDigit(value, digit, sz);
		return sz;
	}

	void FCSpin::onChar(wchar_t ch){
		if (m_autoFormat){
			callEvents(FCEventID::CHAR);
			FCHost *host = getNative()->getHost();
			if (!host->isKeyPress(0x11)){
				wchar_t szTemp[2];
				szTemp[0] = ch;
				szTemp[1] = '\0';
				String str = szTemp;
				bool flag = false;
				if (m_selectionLength > 0){
					flag = true;
				}
				int curPos = m_selectionStart;
				bool cursorIsLast = false;
				if (curPos == (int)m_text.length()){
					cursorIsLast = true;
				}
				int exCount = 0;
				if (ch == 8){
					if (m_selectionLength > 0){
						if (m_selectionStart > 0 && m_text[m_selectionStart - 1] == ','){
							setSelectionStart(m_selectionStart - 1);
							setSelectionLength(m_selectionLength + 1);
							curPos = m_selectionLength;
						}
						//insertWord(str);
						setText(formatNum(m_text));
						setSelectionStart(curPos);
					}
					else{
						if (m_selectionStart > 0 && m_text[m_selectionStart - 1] == ','){
							setSelectionStart(m_selectionStart - 1);
							curPos = m_selectionStart;
						}

						if (m_selectionStart > 0 && m_text[m_selectionStart - 1] == '.'){
							String sub = m_text.substr(m_selectionStart);
							exCount = (int)sub.length() / 3;
						}
						int length = (int)m_text.length() - curPos;
						//insertWord(str);
						String oriText = m_text;
						int diff = 0;
						if (oriText.find(L",") == 0){
							oriText = oriText.substr(1);
							diff = 1;
						}
						setText(formatNum(m_text));
						if ((int)m_text.length() + diff - exCount - length > 0){
							setSelectionStart((int)m_text.length() + diff - exCount - length);
						}
					}
					invalidate();
					return;
				}
				if (ch == 46){
					if (m_text.find(L".") != -1){
						if (m_text.find(L".") != (int)m_text.length() - 1){
							setSelectionStart(m_text.find(L".") + 1);
						}
						invalidate();
						return;
					}
				}
				int backLength = (int)m_text.length() - curPos;
				if (ch == 46){
					String sub = m_text.substr(m_selectionStart);
					exCount = (int)sub.length() - (int)FCStr::replace(sub, L",", L"").length();
					insertWord(str);
					onTextChanged();
					onValueChanged();
					invalidate();
					return;
				}
				bool removeCharFlag = false;
				if (m_selectionLength > 0 && m_selectionLength == (int)m_text.length()){
            		m_text = L"";
            		setSelectionLength(0);
        		}
				else if (m_text.find(L"0.") == 0){
					if (m_selectionStart == 0){
						removeCharFlag = true;
						m_text = m_text.substr(1);
						setSelectionLength(0);
					}
				}
				String oldText = m_text;
				int index = (int)m_text.find(L".");
                    		if (m_selectionLength == 0 && index < getSelectionStart()){
                        		m_selectionLength = 1;
                    		}
				insertWord(str);
				int selectionStart = m_selectionStart;
				setRegion();
				m_selectionLength = 0;
                    		String newText = getText();
                    		if ((int)newText.length() > 0 && selectionStart > (int)newText.length()){
                        		setSelectionStart((int)newText.length());
                    		}
				if (((m_text.find(L".") != -1) && ((int)m_text.find(L".") < m_selectionStart)) || flag){
					setSelectionStart(selectionStart);
				}
				else{
					if (cursorIsLast == true){
						setSelectionStart((int)m_text.length());
					}
					else{
						if ((int)m_text.length() + exCount - backLength >= 0){
							setSelectionStart((int)m_text.length() + exCount - backLength);
						}
					}
				}
				if (removeCharFlag){
					setSelectionStart(m_selectionStart + 1);
				}
				m_showCursor = true;
				onTextChanged();
				onValueChanged();
				invalidate();
			}
		}
		else{
			FCTextBox::onChar(ch);
		}
	}

	void FCSpin::onKeyDown(char key){
		if (m_autoFormat){
			if (key == 8 || key == 46){
				String text = getText();
				if ((int)text.length() > 0){
					int curPos = m_selectionStart;
					if (text.find(L".") != -1 && text.find(L".") == curPos - 1 && m_selectionLength <= 1){
						setSelectionStart(curPos - 1);
						curPos = m_selectionStart;
					}
					if (m_selectionStart == 0){
						int len = m_selectionLength + 1;
						if (len > (int)text.length()){
							len = (int)text.length();
						}
						m_text = text.substr(len);
					}
					else{
						deleteWord();
					}
					bool deleteAll = (int)m_text.length() == 0;
					setRegion();
					if (curPos > 0){
						setSelectionStart(curPos - 1);
					}
					else{
						setSelectionStart(curPos);
					}
					if (deleteAll){
						setSelectionStart((int)m_text.length());
						setSelectionLength(0);
					}
					else if(curPos == 0 && m_text.find(L"0.") == 0){
						setSelectionStart((int)m_text.length());
					}
					m_showCursor = true;
					onTextChanged();
					onValueChanged();
					invalidate();
					return;
				}
			}
		}
		FCTextBox::onKeyDown(key);
        FCHost *host = getNative()->getHost();
        if (!host->isKeyPress(0x10) && !host->isKeyPress(0x11) && !host->isKeyPress(0x12)){
            if (key == 38){
                add();
                invalidate();
            }
            else if (key == 40){
                reduce();
                invalidate();
            }
        }
	}

	void FCSpin::onLoad(){
		FCTextBox::onLoad();
		FCHost *host = getNative()->getHost();
		if (!m_downButton){
			m_downButton = dynamic_cast<FCButton*>(host->createInternalControl(this, L"downbutton"));
			m_downButton->addEvent(m_downButtonTouchDownEvent, FCEventID::TOUCHDOWN, this);
			m_downButton->addEvent(m_downButtonTouchUpEvent, FCEventID::TOUCHUP, this);
			addControl(m_downButton);
		}
		if (!m_upButton){
			m_upButton = dynamic_cast<FCButton*>(host->createInternalControl(this, L"upbutton"));
			m_upButton->addEvent(m_upButtonTouchDownEvent, FCEventID::TOUCHDOWN, this);
			m_upButton->addEvent(m_upButtonTouchUpEvent, FCEventID::TOUCHUP, this);
			addControl(m_upButton);
		}
		update();
	}

	void FCSpin::onTouchWheel(FCTouchInfo touchInfo){
		FCTextBox::onTouchWheel(touchInfo); 
        if (touchInfo.m_delta > 0){
            add();
        }
        else{
            reduce();
        }
        invalidate();
	}

	void FCSpin::onPaste(){
		if (m_autoFormat){
			callEvents(FCEventID::PASTE);
			FCHost *host = getNative()->getHost();
			String insert = FCStr::stringTowstring(host->paste());
			if ((int)insert.length() > 0){
				insertWord(insert);
				setText(formatNum(getValueByDigit(getValue(), m_digit)));
				onTextChanged();
				onValueChanged();
				invalidate();
			}      
		}
		else{
			FCTextBox::onPaste();
		}
	}

	void FCSpin::onTimer(int timerID){
		FCTextBox::onTimer(timerID);
		if (timerID == m_timerID){
			if (m_tick > 20){
				if (m_tick > 50 || m_tick % 3 == 1){
					if (m_isAdding){
						add();
						invalidate();
					}
					else if (m_isReducing){
						reduce();
						invalidate();
					}
				}
			}
			m_tick++;
		}
	}

	void FCSpin::onValueChanged(){
		callEvents(FCEventID::VALUECHANGED);
	}

	void FCSpin::reduce(){
		setValue(getValue() - m_step);
	}

	void FCSpin::setProperty(const String& name, const String& value){
		if (name == L"autoformat"){
			setAutoFormat(FCStr::convertStrToBool(value));
        }
		else if(name == L"digit"){
			setDigit(FCStr::convertStrToInt(value));
		}
		else if(name == L"maximum"){
			setMaximum(FCStr::convertStrToDouble(value));
		}
		else if(name == L"minimum"){
			setMinimum(FCStr::convertStrToDouble(value));
		}
		else if (name == L"showthousands"){
			setShowThousands(FCStr::convertStrToBool(value));
        }
		else if(name == L"step"){
			setStep(FCStr::convertStrToDouble(value));
		}
		else if(name == L"value"){
			setValue(FCStr::convertStrToDouble(value));
		}
		else{
			FCTextBox::setProperty(name, value);
		}
	}

	void FCSpin::setRegion(){
		String textValue = FCStr::replace(m_text, L",", L"");
        if ((int)textValue.length() == 0){
            textValue = L"0";
        }
        if (textValue.find(L".") != -1 && textValue.find(L".") == 0){
            textValue = L"0" + textValue;
        }
		double value = FCStr::convertStrToDouble(textValue);
        if (value > getMaximum()){
            value = getMaximum();
        }
        if (value < getMinimum()){
            value = getMinimum();
        }
        setText(formatNum(getValueByDigit(value, m_digit)));
	}

	void FCSpin::update(){
		int width = getWidth(), height = getHeight(), uBottom = 0;
		if (m_upButton && m_upButton->isVisible()){
			int uWidth = m_upButton->getWidth();		
			FCPoint location = {width - uWidth, 0};
			m_upButton->setLocation(location);
			FCSize size = {uWidth, height / 2};
			m_upButton->setSize(size);
			uBottom = m_upButton->getBottom();
			FCPadding oldPadding = getPadding();
			FCPadding padding(oldPadding.left, oldPadding.top, uWidth + 3, oldPadding.bottom);
			setPadding(padding);
		}
		if (m_downButton && m_downButton->isVisible()){
			int dWidth = m_downButton->getWidth();
			FCPoint location = {width - dWidth, uBottom};
			m_downButton->setLocation(location);
			FCSize size = {m_upButton->getWidth(), height - uBottom};
			m_downButton->setSize(size);
		}
	}
}