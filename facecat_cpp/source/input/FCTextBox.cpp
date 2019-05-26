#include "..\\..\\stdafx.h"
#include "..\\..\\include\\input\\FCTextBox.h"

namespace FaceCat{
	void FCTextBox::cursorDown(){
		FCHost *host = getNative()->getHost();
        int scol = -1, srow = -1;
        int rangeSize = (int)m_ranges.size();
        int start = m_selectionStart + m_selectionLength < rangeSize - 1 ? m_selectionStart + m_selectionLength : rangeSize - 1;
		if (host->isKeyPress(VK_SHIFT)){
			start = m_stopMovingIndex;
		}
	    else{
            if (m_selectionLength > 0){
                m_selectionLength = 1;
            }
        }
        int lineCount = (int)m_lines.size();
		bool check = false;
        for (int i = 0; i < lineCount; i++){
            WordLine line = m_lines[i];
            for (int j = line.m_start; j <= line.m_end; j++){
				if (j >= start && j < rangeSize){
					int col = j - line.m_start;
					if (j == start){
						if (i != 0 && j == line.m_start){
							check = true;
							srow = i - 1;
							scol = line.m_end + 1;
						}
						else{
							if (i != lineCount - 1){
								check = true;
								int idx = j - line.m_start;
								scol = m_lines[i + 1].m_start + idx + 1;
								srow = i;
								continue;
							}
						}
					}
					if (check){
						if (i == srow + 1){
							if (host->isKeyPress(VK_SHIFT)){
								setMovingIndex(m_startMovingIndex, j);
							}
							else{
								if (scol > line.m_end){
									scol = line.m_end + 1;
								}
								m_selectionStart = scol;
								m_selectionLength = 0;
								m_startMovingIndex = m_selectionStart;
								m_stopMovingIndex = m_selectionStart;
							}
							m_showCursor = true;
							startTimer(m_timerID, TICK);
							return;
						}
					}
				}
            }
        }
	}

	void FCTextBox::cursorEnd(){		
		FCHost *host = getNative()->getHost();
        int rangeSize = (int)m_ranges.size();
        int start = m_selectionStart + m_selectionLength < rangeSize - 1 ? m_selectionStart + m_selectionLength : rangeSize - 1;
		if (host->isKeyPress(VK_SHIFT)){
			start = m_stopMovingIndex;
		}
        int lineCount = (int)m_lines.size();
        for (int i = 0; i < lineCount; i++){
            WordLine line = m_lines[i];
            for (int j = line.m_start; j <= line.m_end; j++){
                if (j == start){
				    if (j == line.m_start && i > 0){
                        line = m_lines[i - 1];
                    }
					if (host->isKeyPress(VK_SHIFT)){
						setMovingIndex(m_startMovingIndex, line.m_end + 1);
					}
					else{
						m_selectionStart = line.m_end + 1;
						m_selectionLength = 0;
						m_startMovingIndex = m_selectionStart;
						m_stopMovingIndex = m_selectionStart;
					}
                    m_showCursor = true;
					startTimer(m_timerID, TICK);
                    return;
                }
            }
        }
	}

	void FCTextBox::cursorHome(){
		FCHost *host = getNative()->getHost();
        int rangeSize = (int)m_ranges.size();
        int start = m_selectionStart < rangeSize - 1? m_selectionStart : rangeSize - 1;
		if (host->isKeyPress(VK_SHIFT)){
			start = m_stopMovingIndex;
		}
        int lineCount = (int)m_lines.size();
        for (int i = 0; i < lineCount; i++){
            WordLine line = m_lines[i];
            for (int j = line.m_start; j <= line.m_end; j++){
                if (j == start){
				    if (j == line.m_start && i > 0){
                        line = m_lines[i - 1];
                    }
					if (host->isKeyPress(VK_SHIFT)){
						setMovingIndex(m_startMovingIndex, line.m_start + 1);
					}
					else{
						m_selectionStart = line.m_start + 1;
						m_selectionLength = 0;
						m_startMovingIndex = m_selectionStart;
						m_stopMovingIndex = m_selectionStart;
					}
                    m_showCursor = true;
					startTimer(m_timerID, TICK);
                    return;
                }
            }
        }
	}

	void FCTextBox::cursorLeft(){
		FCHost *host = getNative()->getHost();
        if (host->isKeyPress(VK_SHIFT)){
			setMovingIndex(m_startMovingIndex, m_stopMovingIndex - 1);
		}
		else{
			if (m_selectionStart > 0){
				m_selectionStart -= 1;
			}
			m_selectionLength = 0;
			m_startMovingIndex = m_selectionStart;
			m_stopMovingIndex = m_selectionStart;
		}
	}

	void FCTextBox::cursorRight(){
		FCHost *host = getNative()->getHost();
        if (host->isKeyPress(VK_SHIFT)){
			setMovingIndex(m_startMovingIndex, m_stopMovingIndex + 1);
		}
		else{
			int rangeSize = (int)m_ranges.size();
			int start = m_selectionStart + m_selectionLength < rangeSize - 1 ? m_selectionStart + m_selectionLength : rangeSize - 1;
			if(start < rangeSize){
				m_selectionStart = start + 1;
			}
			m_selectionLength = 0;
			m_startMovingIndex = m_selectionStart;
			m_stopMovingIndex = m_selectionStart;
		}
	}

	void FCTextBox::cursorUp(){
		FCHost *host = getNative()->getHost();
		int scol = -1, srow = -1;
        int rangeSize = (int)m_ranges.size();
        int start = m_selectionStart < rangeSize - 1 ? m_selectionStart : rangeSize - 1;
		if (host->isKeyPress(VK_SHIFT)){
			start = m_stopMovingIndex;
		}
	    else{
            if (m_selectionLength > 0){
                m_selectionLength = 1;
            }
        }
        int lineCount = (int)m_lines.size();
		bool check = false;
        for (int i = lineCount - 1; i >= 0; i--){
            WordLine line = m_lines[i];
            for (int j = line.m_end; j >= line.m_start; j--){
				if (j >= 0 && j <= start){
					int col = j - line.m_start;
					if (i != 0 && j == start){
						check = true;
						if (i != lineCount - 1 && j == line.m_start){
							srow = i;
							scol = m_lines[i - 1].m_start;
						}
						else{
							int idx = j - line.m_start;
							scol = m_lines[i - 1].m_start + idx - 1;
							if (scol < 0){
								scol = 0;
							}
							srow = i;
						}
						continue;
					}
					if (check){
						if (i == srow - 1 && col <= scol){
							if (host->isKeyPress(VK_SHIFT)){
								setMovingIndex(m_startMovingIndex, j);
							}
							else{
								if (scol > line.m_end){
									scol = line.m_end + 1;
								}
								m_selectionStart = scol;
								m_selectionLength = 0;
								m_startMovingIndex = m_selectionStart;
								m_stopMovingIndex = m_selectionStart;
							}
							m_showCursor = true;
							startTimer(m_timerID, TICK);
							return;
						}
					}
				}
            }
        }
	}

	void FCTextBox::deleteWord(){
		String text = getText();
		int textLength = (int)text.length();
		if(textLength > 0){
			int oldLines = (int)m_lines.size();
			String left, right;
			int rightIndex = -1;
			if (m_selectionLength > 0){
				left = m_selectionStart > 0 ? text.substr(0, m_selectionStart) : L"";
				rightIndex = m_selectionStart + m_selectionLength;
			}
			else{
				left = m_selectionStart > 0 ? text.substr(0, m_selectionStart - 1) : L"";
				rightIndex = m_selectionStart + m_selectionLength;
				if (m_selectionStart > 0){
					m_selectionStart -= 1;
				}
			}
			if (rightIndex < textLength){
				right = text.substr(rightIndex);
			}
			m_text = left + right;
			String newText = left + right;
			m_text = newText;
			textLength = (int)newText.length();
			if(textLength == 0){
				m_selectionStart = 0;
			}
			else{
				if (m_selectionStart > textLength){
					m_selectionStart = textLength;
				}
			}
			m_selectionLength = 0;
		}
	}

	void FCTextBox::insertWord(const String& str){
		String text = getText();
		if ((int)text.length() == 0 || m_selectionStart == (int)text.length()){
			text = text + str;
			m_text = text;
			m_selectionStart = (int)text.length();
		}
		else{
			int sIndex = m_selectionStart > (int)text.length() ? (int)text.length() : m_selectionStart;
			String left = sIndex > 0 ? text.substr(0, sIndex) : L"";
			String right;
			int rightIndex = m_selectionStart + (m_selectionLength == 0 ? 0 : m_selectionLength);
			if (rightIndex < (int)text.length()){
				right = text.substr(rightIndex);
			}
			text = left + str + right;
			m_text = text;
			m_selectionStart += (int)str.length();
			if (m_selectionStart > (int)text.length()){
				m_selectionStart = (int)text.length();
			}
			m_selectionLength = 0;
		}
	}

	bool FCTextBox::isLineVisible(int indexTop, int indexBottom, int cell, int floor, int lineHeight, double visiblePercent){
        if (indexTop < cell){
            indexTop = cell;
        }
        else if (indexTop > floor){
            indexTop = floor;
        }
        if (indexBottom < cell){
            indexBottom = cell;
        }
        else if (indexBottom > floor){
            indexBottom = floor;
        }
        if (indexBottom - indexTop > lineHeight * visiblePercent){
            return TRUE;
        }
        return FALSE;
    }

	bool FCTextBox::isLineVisible(int index, double visiblePercent){
		int rangeSize = (int)m_ranges.size();
        if (rangeSize > 0){
            if (index >= 0 && index < rangeSize){
                int top = 0, scrollV = 0, sch = 0;
                FCHScrollBar *hScrollBar = getHScrollBar();
                FCVScrollBar *vScrollBar = getVScrollBar();
                if (hScrollBar && hScrollBar->isVisible()){
                    sch = hScrollBar->getHeight();
                }
                if (vScrollBar && vScrollBar->isVisible()){
                    scrollV = -vScrollBar->getPos();
                }
                top = scrollV;
                int cell = 1;
                int floor = getHeight() - cell - sch - 1;
                FCRectF indexRect = m_ranges[index];
				int indexTop = (int)indexRect.top + scrollV;
                int indexBottom = (int)indexRect.bottom + scrollV;
				return isLineVisible(indexTop, indexBottom, cell, floor, m_lineHeight, visiblePercent);
            }
        }
        return false;
	}

	void FCTextBox::setMovingIndex(int sIndex, int eIndex){
		int textSize = (int)(getText().length());
		if(textSize > 0){
			if(sIndex < 0){
				sIndex = 0;
			}
			if(sIndex > textSize){
				sIndex = textSize;
			}
			if(eIndex < 0){
				eIndex = 0;
			}
			if(eIndex > textSize){
				eIndex = textSize;
			}
			m_startMovingIndex = sIndex;
			m_stopMovingIndex = eIndex;
			m_selectionStart = min(m_startMovingIndex, m_stopMovingIndex);
			m_selectionLength = abs(m_startMovingIndex - m_stopMovingIndex);
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////

	FCTextBox::FCTextBox(){
		m_isKeyDown = false;
		m_isTouchDown = false;
		m_lineHeight = 20;
		m_multiline = false;
		m_offsetX = 0;
		m_passwordChar = 0;
		m_readOnly = false;
		m_showCursor = false;
		m_rightToLeft = false;
		m_selectionBackColor = FCColor::argb(10, 36, 106);
		m_selectionTextColor = FCColor::argb(255, 255, 255);
		m_selectionLength = 0;
		m_selectionStart = -1;
		m_startMovingIndex = -1;
		m_stopMovingIndex = -1;
		m_tempTextColor = FCColor_DisabledText;
		m_textAlign = FCHorizontalAlign_Left;
		m_textChanged = false;
		m_tick = 0;
		TICK = 500;
		m_timerID = FCView::getNewTimerID();
		m_wordWrap = false;
		setCursor(FCCursors_IBeam);
		FCSize size = {100, 20};
        setSize(size);
		setTabStop(true);
	}

	FCTextBox::~FCTextBox(){
		stopTimer(m_timerID);
		m_lines.clear();
		m_ranges.clear();
		m_wordsSize.clear();
		while(!m_redoStack.empty()){
			m_redoStack.pop();
		}
		while(!m_undoStack.empty()){
			m_undoStack.pop();
		}
	}

	int FCTextBox::getLinesCount(){
		return (int)m_lines.size();
	}

	int FCTextBox::getLineHeight(){
		return m_lineHeight;
	}

	void FCTextBox::setLineHeight(int lineHeight){
		m_lineHeight = lineHeight;
	}

	vector<WordLine> FCTextBox::getLines(){
		return m_lines;
	}

	bool FCTextBox::isMultiline(){
		return m_multiline;
	}

	void FCTextBox::setMultiline(bool multiline){
		if(m_multiline != multiline){
			m_multiline = multiline;
			m_textChanged = true;
		}
		setShowVScrollBar(m_multiline);
	}

	wchar_t FCTextBox::getPasswordChar(){
		return m_passwordChar;
	}

	void FCTextBox::setPasswordChar(wchar_t passwordChar){
		m_passwordChar = passwordChar;
		m_textChanged = true;
	}

	bool FCTextBox::isReadOnly(){
		return m_readOnly;
	}

	void FCTextBox::setReadOnly(bool readOnly){
		m_readOnly = readOnly;
	}

	bool FCTextBox::isRightToLeft(){
		return m_rightToLeft;
	}

	void FCTextBox::setRightToLeft(bool rightToLeft){
		m_rightToLeft = rightToLeft;
		m_textChanged = true;
	}

	Long FCTextBox::getSelectionBackColor(){
		return m_selectionBackColor;
	}

	void FCTextBox::setSelectionBackColor(Long selectionBackColor){
		m_selectionBackColor = selectionBackColor;
	}

	Long FCTextBox::getSelectionTextColor(){
		return m_selectionTextColor;
	}

	void FCTextBox::setSelectionTextColor(Long selectionTextColor){
		m_selectionTextColor = selectionTextColor;
	}

	int FCTextBox::getSelectionLength(){
		return m_selectionLength;
	}

	void FCTextBox::setSelectionLength(int selectionLength){
		m_selectionLength = selectionLength;
	}

	int FCTextBox::getSelectionStart(){
		return m_selectionStart;
	}

	void FCTextBox::setSelectionStart(int selectionStart){
		m_selectionStart = selectionStart;
	}

	String FCTextBox::getTempText(){
		return m_tempText;
	}

	void FCTextBox::setTempText(const String& tempText){
		m_tempText = tempText;
	}

	Long FCTextBox::getTempTextColor(){
		return m_tempTextColor;
	}

	void FCTextBox::setTempTextColor(Long tempTextColor){
		m_tempTextColor = tempTextColor;
	}

	FCHorizontalAlign FCTextBox::getTextAlign(){
		return m_textAlign;
	}

	void FCTextBox::setTextAlign(FCHorizontalAlign textAlign){
		m_textAlign = textAlign;
	}

	bool FCTextBox::isWordWrap(){
		return m_wordWrap;
	}

	void FCTextBox::setWordWrap(bool wordWrap){
		if(m_wordWrap != wordWrap){
			m_wordWrap = wordWrap;
			m_textChanged = true;
		}
		setShowHScrollBar(!m_wordWrap);
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	bool FCTextBox::canRedo(){
	    if ((int)m_redoStack.size() > 0){
            return true;
        }
        return false;
	}

	bool FCTextBox::canUndo(){
	    if ((int)m_undoStack.size() > 0){
            return true;
        }
        return false;
	}

	void FCTextBox::clearRedoUndo(){
		while(!m_redoStack.empty()){
			m_redoStack.pop();
		}
		while(!m_undoStack.empty()){
			m_undoStack.pop();
		}
	}

	int FCTextBox::getContentHeight(){
		int hmax = FCDiv::getContentHeight();
        int cheight = 0;
		int rangeSize = (int)m_ranges.size();
        for (int i = 0; i < rangeSize; i++){
            if (cheight < m_ranges[i].bottom){
                cheight = (int)m_ranges[i].bottom;
            }
        }
        return hmax > cheight ? hmax : cheight;
	}

	int FCTextBox::getContentWidth(){
		int wmax = FCDiv::getContentWidth();
        int cwidth = 0;
		int rangeSize = (int)m_ranges.size();
        for (int i = 0; i < rangeSize; i++){
            if (cwidth < m_ranges[i].right){
                cwidth = (int)m_ranges[i].right;
            }
        }
        return wmax > cwidth ? wmax : cwidth;
	}

	String FCTextBox::getControlType(){
		return L"TextBox";
	}

	void FCTextBox::getProperty(const String& name, String *value, String *type){
		if(name == L"lineheight"){
			*type = L"int";
			*value = FCStr::convertIntToStr(getLineHeight());
		}
		else if(name == L"multiline"){
			*type = L"bool";
			*value = FCStr::convertBoolToStr(isMultiline());
		}
		else if(name == L"passwordchar"){
			*type = L"text";
			wchar_t str[2] = {0};
			str[0] = getPasswordChar();
			str[1] = 0;
			*value = str;
		}
		else if(name == L"readonly"){
			*type = L"bool";
			*value = FCStr::convertBoolToStr(isReadOnly());
		}
		else if(name == L"righttoleft"){
			*type = L"bool";
			*value = FCStr::convertBoolToStr(isRightToLeft());
		}
		else if(name == L"selectionbackcolor"){
			*type = L"color";
			*value = FCStr::convertColorToStr(getSelectionBackColor());
		}
		else if(name == L"selectionTextColor"){
			*type = L"color";
			*value = FCStr::convertColorToStr(getSelectionTextColor());
		}
		else if (name == L"temptext"){
            *type = L"text";
            *value = getTempText();
        }
        else if (name == L"temptextcolor"){
            *type = L"color";
			*value = FCStr::convertColorToStr(getTempTextColor());
        }
		else if (name == L"textalign"){
            *type = L"enum:FCHorizontalAlign";
			*value = FCStr::convertHorizontalAlignToStr(getTextAlign());
        }
		else if(name == L"wordwrap"){
			*type = L"bool";
			*value = FCStr::convertBoolToStr(isWordWrap());
		}
		else{
			FCDiv::getProperty(name, value, type);
		}
	}

	ArrayList<String> FCTextBox::getPropertyNames(){
		ArrayList<String> propertyNames = FCDiv::getPropertyNames();
		propertyNames.add(L"LineHeight");
		propertyNames.add(L"Multiline");
		propertyNames.add(L"PasswordChar");
		propertyNames.add(L"ReadOnly");
		propertyNames.add(L"RightToLeft");
		propertyNames.add(L"SelectionBackColor");
		propertyNames.add(L"selectionTextColor");
		propertyNames.add(L"TempText");
		propertyNames.add(L"TempTextColor");
		propertyNames.add(L"TextAlign");
		propertyNames.add(L"WordWrap");
		return propertyNames;
	}

	String FCTextBox::getSelectionText(){
		String text = getText();
		int textLength = (int)text.length();
		if (textLength > 0 && m_selectionStart != textLength){
			String selectionText = text.substr(m_selectionStart, m_selectionLength);
			return selectionText;
		}
		return L"";
	}

	void FCTextBox::onChar(wchar_t ch){
		if (!m_readOnly){
			FCHost *host = getNative()->getHost();
			if (!host->isKeyPress(VK_CONTROL)){
				int oldLines = (int)m_lines.size();
				if (ch != 8 || (!m_multiline && ch == 13)){
					wchar_t szTemp[2];
					szTemp[0] = ch;
					szTemp[1] = '\0';
					String str = szTemp;
					String redotext = getText();
					insertWord(str);
					onTextChanged();
					if (m_textChanged){
						m_undoStack.push(redotext);
					}
				}
				invalidate();
				int newLines = (int)m_lines.size();
				if (newLines != oldLines){
					FCVScrollBar *vScrollBar = getVScrollBar();
					if(vScrollBar){
						vScrollBar->setPos(vScrollBar->getPos() + m_lineHeight * (newLines - oldLines));
						invalidate();
					}
				}
			}
		}
	}

	void FCTextBox::onCopy(){
		FCHost *host = getNative()->getHost();
		String text = getSelectionText();
		if(text.length() > 0){
			string str = FCStr::wstringTostring(text);
			host->copy(str);
			FCDiv::onCopy();
		}
	}

	void FCTextBox::onCut(){
		if (!m_readOnly){
			onCopy();
			int oldLines = (int)m_lines.size();
			String redotext = getText();
			deleteWord();
			onTextChanged();
			if (m_textChanged){
				m_undoStack.push(redotext);
			}
			invalidate();
			int newLines = (int)m_lines.size();
			if (newLines != oldLines){
				FCVScrollBar *vScrollBar = getVScrollBar();
				if(vScrollBar){
					vScrollBar->setPos(vScrollBar->getPos() + m_lineHeight * (newLines - oldLines));
					invalidate();
				}
			}
			FCDiv::onCut();
		}
	}

	void FCTextBox::onGotFocus(){
		FCDiv::onGotFocus();
		m_showCursor = true;
		invalidate();
		startTimer(m_timerID, TICK);
	}

	void FCTextBox::onKeyDown(char key){
		m_isKeyDown = true;
		FCHost *host = getNative()->getHost();
		if (host->isKeyPress(VK_CONTROL)){
			FCDiv::onKeyDown(key);
			if(key == 65){
				selectAll();
			}
            else if (key == 89){
                redo();
            }
            else if (key == 90){
                undo();
            }
		}
		else{
			if (key >= 35 && key <= 40){
				if(key == 38 || key == 40){
					callKeyEvents(FCEventID::KEYDOWN, key);
					if(m_lines.size() > 1){
						int offset = 0;
						if (key == 38){
							cursorUp();
							if (!isLineVisible(m_stopMovingIndex, 0.6)){
								offset = -m_lineHeight;
							}
						}
						else if (key == 40){
							cursorDown();
							if (!isLineVisible(m_stopMovingIndex, 0.6)){
								offset = m_lineHeight;
							}
						}
						FCVScrollBar *vScrollBar = getVScrollBar();
						if (vScrollBar && vScrollBar->isVisible()){
							vScrollBar->setPos(vScrollBar->getPos() + offset);
							vScrollBar->update();
						}
					}
				}
				else{
					FCDiv::onKeyDown(key);
					if (key == 35){
                        cursorEnd();
                    }
                    else if (key == 36){
                        cursorHome();
                    }
					else if(key == 37){
						cursorLeft();
					}
					else if(key == 39){
						cursorRight();
					}
				}
			}
			else{
				FCDiv::onKeyDown(key);
				if(key == 27){
					setFocused(false);
				}
				else if(key == 8 || key == 46){
					int oldLines = (int)m_lines.size();
					String redotext = getText();
					deleteWord();
					onTextChanged();
					if (m_textChanged){
						m_undoStack.push(redotext);
					}
					invalidate();
					int newLines = (int)m_lines.size();
					if (newLines != oldLines){
						FCVScrollBar *vScrollBar = getVScrollBar();
						if(vScrollBar){
							vScrollBar->setPos(vScrollBar->getPos() + m_lineHeight * (newLines - oldLines));
							invalidate();
						}
					}
				}
			}
		}
		invalidate();
	}

	void FCTextBox::onKeyUp(char key){
		FCDiv::onKeyUp(key);
		FCHost *host = getNative()->getHost();
		if(!host->isKeyPress(VK_SHIFT) && !m_isTouchDown){
			m_startMovingIndex = m_selectionStart;
			m_stopMovingIndex = m_selectionStart;
		}
		m_isKeyDown = false;
	}

	void FCTextBox::onLostFocus(){
		FCDiv::onLostFocus();
		stopTimer(m_timerID);
		m_isKeyDown = false;
		m_showCursor = false;
		m_selectionLength = 0;
		invalidate();
	}

	void FCTextBox::onTouchDown(FCTouchInfo touchInfo){
		FCDiv::onTouchDown(touchInfo);
		FCPoint mp = touchInfo.m_firstPoint;
		if(touchInfo.m_firstTouch){
			if (touchInfo.m_clicks == 1){
				int sIndex = -1;
				int linesCount = (int)m_lines.size();
				m_selectionLength = 0;
				m_startMovingIndex = -1;
				m_stopMovingIndex = -1;
				if (linesCount > 0){
					FCHScrollBar *hScrollBar = getHScrollBar();
					FCVScrollBar *vScrollBar = getVScrollBar();
					int scrollH = (hScrollBar && hScrollBar->isVisible()) ? hScrollBar->getPos() : 0;
					int scrollV = (vScrollBar && vScrollBar->isVisible()) ? vScrollBar->getPos() : 0;
					scrollH += m_offsetX;
					int x = mp.x + scrollH, y = mp.y + scrollV;
					FCRectF lastRange = {0};
					int rangeSize = (int)m_ranges.size();
					if (rangeSize > 0){
						lastRange = m_ranges[rangeSize - 1];
					}
					for (int i = 0; i < linesCount; i++){
						WordLine line = m_lines[i];
						for (int j = line.m_start; j <= line.m_end; j++){
							FCRectF rect = m_ranges[j];
							if (i == linesCount - 1){
								rect.bottom += 3;
							}
							if (y >= rect.top && y <= rect.bottom){
								float sub = (rect.right - rect.left) / 2;
								if ((x >= rect.left - sub && x <= rect.right - sub)
									|| (j == line.m_start && x <= rect.left + sub)
									|| (j == line.m_end &&x >= rect.right - sub)){
									if (j == line.m_end && x >= rect.right - sub){
										sIndex = j + 1;
									}
									else{
										sIndex = j;
									}
									break;
								}
							}
						}
						if (sIndex != -1){
							break;
						}
					}
					if (sIndex == -1){
						if ((y >= lastRange.top && y <= lastRange.bottom && x > lastRange.right) || (y >= lastRange.bottom)){
							sIndex = rangeSize;
						}
					}
				}
				if (sIndex != -1){
					m_selectionStart = sIndex;
				}
				else{
					m_selectionStart = 0;
				}
				m_startMovingIndex = m_selectionStart;
				m_stopMovingIndex = m_selectionStart;
            }
			else if (touchInfo.m_clicks == 2){
                if (!m_multiline){
                    selectAll();
                }
            }
		}
		m_isTouchDown = true;
		m_showCursor = true;
		startTimer(m_timerID, TICK);
		invalidate();
	}

	void FCTextBox::onTouchMove(FCTouchInfo touchInfo){
		FCDiv::onTouchMove(touchInfo);
		FCPoint mp = touchInfo.m_firstPoint;
		if (m_isTouchDown){
			int linesCount = (int)m_lines.size();
            if (linesCount > 0){
                int eIndex = -1;
                FCHScrollBar *hScrollBar = getHScrollBar();
                FCVScrollBar *vScrollBar = getVScrollBar();
                int scrollH = (hScrollBar && hScrollBar->isVisible()) ? hScrollBar->getPos() : 0;
                int scrollV = (vScrollBar && vScrollBar->isVisible()) ? vScrollBar->getPos() : 0;
                scrollH += m_offsetX;
				FCPoint point = mp;
                if (point.x < 0){
                    point.x = 0;
                }
                if (point.y < 0){
                    point.y = 0;
                }
                int x = point.x + scrollH, y = point.y + scrollV;
				FCRectF lastRange = {0};
                int rangeSize = (int)m_ranges.size();
                if (rangeSize > 0){
                    lastRange = m_ranges[rangeSize - 1];
                }
                for (int i = 0; i < linesCount; i++){
                    WordLine line = m_lines[i];
                    for (int j = line.m_start; j <= line.m_end; j++){
                        FCRectF rect = m_ranges[j];
						if (i == linesCount - 1){
                            rect.bottom += 3;
                        }
                        if (eIndex == -1){
                            if (y >= rect.top && y <= rect.bottom){
                                int sub = (int)(rect.right - rect.left) / 2;
                                if ((x >= rect.left - sub && x <= rect.right - sub)
                                    || (j == line.m_start && x <= rect.left + sub)
                                    || (j == line.m_end && x >= rect.right - sub)){
                                    if (j == line.m_end && x >= rect.right - sub){
                                        eIndex = j + 1;
                                    }
                                    else{
                                        eIndex = j;
                                    }
                                }
                            }
                        }
                    }
                    if (eIndex != -1){
                        break;
                    }
                }
			    if (eIndex != -1){
                    m_stopMovingIndex = eIndex;
                }
                if (m_startMovingIndex == m_stopMovingIndex){
                    m_selectionStart = m_startMovingIndex;
                    m_selectionLength = 0;
                }
                else{
                    m_selectionStart = m_startMovingIndex < m_stopMovingIndex ? m_startMovingIndex : m_stopMovingIndex;
                    m_selectionLength = abs(m_startMovingIndex - m_stopMovingIndex);
                }
                invalidate();
            }
        }
	}

	void FCTextBox::onTouchUp(FCTouchInfo touchInfo){
		m_isTouchDown = false;
		FCDiv::onTouchUp(touchInfo);
	}

	void FCTextBox::onPaintForeground(FCPaint *paint, const FCRect& clipRect){
		int width = getWidth(), height = getHeight();
		if(width > 0 && height > 0){
			int lineHeight = m_multiline ? m_lineHeight : height; 
			FCPadding padding = getPadding();
			FCHost *host = getNative()->getHost();
			FCRect rect = {0, 0, width, height};
			FCFont *font = getFont();
			FCSizeF tSize = paint->textSizeF(L" ", font);
			FCHScrollBar *hScrollBar = getHScrollBar();
			FCVScrollBar *vScrollBar = getVScrollBar();
			int vWidth = (vScrollBar && vScrollBar->isVisible()) ? (vScrollBar->getWidth() - padding.left - padding.right): 0;
			int scrollH = ((hScrollBar && hScrollBar->isVisible()) ? hScrollBar->getPos() : 0);
			int scrollV = ((vScrollBar && vScrollBar->isVisible()) ? vScrollBar->getPos() : 0);
			float strX = (float)(padding.left + 1);
			String text = getText();
			int length = (int)text.length();
			FCSizeF bSize = paint->textSizeF(L"0", font);
			if(m_textChanged){
				int line = 0, count = 0;
				m_textChanged = !m_textChanged;
				m_lines.clear();
				m_ranges.clear();
				m_wordsSize.clear();
				for(int i = 0; i < length; i++){
					if(i == 0){
						count = 0;
						line ++;
						strX = (float)(padding.left + 1);
						WordLine line(i, i);
						m_lines.push_back(line);
					}
					wchar_t ch[2] = {0};
					ch[0] = text[i];
					ch[1] = '\0';
					if (ch[0] == 9){                  
						int addCount = 4 - count % 4;
						tSize.cx = bSize.cx * addCount;
						tSize.cy = bSize.cy;
						count += addCount;
					}
					else{
						if (m_passwordChar > 0){
							ch[0] = m_passwordChar;
						}
						tSize = paint->textSizeF(ch, font);
						if (ch[0] == 10){
							tSize.cx = 0;
						}
						count ++;
					}
					if (m_multiline){
						bool isNextLine = false;
						if (ch[0] == 13){
							isNextLine = true;
							tSize.cx = 0;
						}
						else{
							if (m_wordWrap){
								if (strX + tSize.cx > width - vWidth){
									isNextLine = true;
								}
							}
						}
						if (isNextLine){
							count = 0;
							line ++;
							strX = (float)(padding.left + 1);
							WordLine line(i, i);
							m_lines.push_back(line);
						}
						else{
							WordLine wl(m_lines[line - 1].m_start, i);
							m_lines[line - 1] = wl;
						}
					}
					else{
						WordLine wl(m_lines[line - 1].m_start, i);
						m_lines[line - 1] = wl;
					}
                        		if (m_text[i] > 1000){
                            			tSize.cx += 1; 
                        		}
					FCRectF rangeRect = {strX, (float)((line - 1) * lineHeight + padding.top), (float)(strX + tSize.cx), (float)(line * lineHeight + padding.top)};
					m_ranges.push_back(rangeRect);
					m_wordsSize.push_back(tSize);
                    strX = rangeRect.right;
				}
				if (m_rightToLeft){
					int lcount = (int)m_lines.size();
					for (int i = 0; i < lcount; i++){
						WordLine ln = m_lines[i];
						float lw = width - vWidth - (m_ranges[ln.m_end].right - m_ranges[ln.m_start].left) - 2;
						if (lw > 0){
							for (int j = ln.m_start; j <= ln.m_end; j++){
								FCRectF rangeRect = m_ranges[j];
								rangeRect.left += lw;
								rangeRect.right += lw;
								m_ranges[j] = rangeRect;
							}
						}
					}
				}
				update();
			}
			scrollH += m_offsetX;
			FCRect tempRect = {0};
			int rangesSize = (int)m_ranges.size();
			int offsetX = m_offsetX;
			if (!m_multiline){
				FCRectF firstRange = {0};
				FCRectF lastRange = {0};
				if(rangesSize > 0){
					firstRange = m_ranges[0];
					lastRange = m_ranges[rangesSize - 1];
				}
				scrollH -= offsetX;
				if (m_textAlign == FCHorizontalAlign_Center){
					offsetX = -(int)(width - padding.right - (lastRange.right - firstRange.left)) / 2;
				}
				else if (m_textAlign == FCHorizontalAlign_Right){
					offsetX = -(int)(width - padding.right - (lastRange.right - firstRange.left) - 3);
				}
				else{
					if (lastRange.right > width - padding.right){
						int alwaysVisibleIndex = m_selectionStart + m_selectionLength;
						if (m_startMovingIndex != -1){
							alwaysVisibleIndex = m_startMovingIndex;
						}
						if (m_stopMovingIndex != -1){
							alwaysVisibleIndex = m_stopMovingIndex;
						}
						if (alwaysVisibleIndex > rangesSize - 1){
							alwaysVisibleIndex = rangesSize - 1;
						}
						if (alwaysVisibleIndex != -1){
							FCRectF alwaysVisibleRange = m_ranges[alwaysVisibleIndex];
							int cw = width - padding.left - padding.right;
							if (alwaysVisibleRange.left - offsetX > cw - 10){
								offsetX = (int)alwaysVisibleRange.right - cw + 3;
								if (offsetX < 0){
									offsetX = 0;
								}
							}
							else if (alwaysVisibleRange.left - offsetX < 10){
								offsetX -= (int)bSize.cx * 4;
								if (offsetX < 0){
									offsetX = 0;
								}
							}
							if (offsetX > lastRange.right - cw){
								offsetX = (int)lastRange.right - cw + 3;
							}
						}
					}
					else{
						if (m_textAlign == FCHorizontalAlign_Right){
							offsetX = -(int)(width - padding.right - (lastRange.right - firstRange.left) - 3);
						}
						else{
							offsetX = 0;
						}
					}
				}
				m_offsetX = offsetX;
				scrollH += m_offsetX;
			}
			int lineCount = (int)m_lines.size();
			vector<FCRectF> selectedRanges;
			vector<FCRect> selectedWordsRanges;
			vector<wchar_t> selectedWords;
			for (int i = 0; i < lineCount; i++){
				WordLine line = m_lines[i];
				for (int j = line.m_start; j <= line.m_end; j++){
					wchar_t ch = text[j];
					if(ch != 9){
						if (m_passwordChar > 0){
							ch = m_passwordChar;
						}
					}
					FCRectF rangeRect = m_ranges[j];
					rangeRect.left -= scrollH;
					rangeRect.top -= scrollV;
					rangeRect.right -= scrollH;
					rangeRect.bottom -= scrollV;
					FCRect rRect = {(int)rangeRect.left, (int)(rangeRect.top + (lineHeight - m_wordsSize[j].cy) / 2), (int)rangeRect.right, (int)(rangeRect.top + (lineHeight + m_wordsSize[j].cy) / 2)};
					if (rRect.right == rRect.left){
						rRect.right = rRect.left + 1;
					}
					if (host->getIntersectRect(&tempRect, &rRect, &rect) > 0){
						if(m_selectionLength > 0){
							if (j >= m_selectionStart && j < m_selectionStart + m_selectionLength){
								selectedWordsRanges.push_back(rRect);
                                selectedRanges.push_back(rangeRect);
								selectedWords.push_back(ch);
								continue;
							}
						}				
						wchar_t str[2] = {0};
						str[0] = ch;
						str[1] = '\0'; 
						paint->drawText(str, getPaintingTextColor(), font, rRect);
					}
				}
			}
			int selectedRangesSize = (int)selectedRanges.size();
			if (selectedRangesSize > 0){
				int sIndex = 0;
				float right = 0;
				for (int i = 0; i < selectedRangesSize; i++){
					FCRectF rRect = selectedRanges[i];
					FCRectF sRect = selectedRanges[sIndex];
					bool newLine = rRect.top != sRect.top;
					if (newLine || i == selectedRangesSize - 1){
						int eIndex = (i == selectedRangesSize - 1) ? i : i - 1;
						FCRectF eRect = selectedRanges[eIndex];
						FCRect unionRect = {(int)sRect.left, (int)sRect.top, (int)eRect.right + 1, (int)sRect.bottom + 1};
						if(newLine){
							unionRect.right = (int)right;
						}
						paint->fillRect(m_selectionBackColor, unionRect);
						for (int j = sIndex; j <= eIndex; j++){
							wchar_t str[2] = {0};
							str[0] = selectedWords[j];
							str[1] = '\0'; 
							paint->drawText(str, m_selectionTextColor, font, selectedWordsRanges[j]);
						}
						sIndex = i;
					}
					right = rRect.right;
				}
				selectedRanges.clear();
				selectedWords.clear();
				selectedWordsRanges.clear();
			}
			if (isFocused() && !m_readOnly && m_selectionLength == 0 && (m_isKeyDown || m_showCursor)){
				int index = m_selectionStart;
				if(index < 0){
					index = 0;
				}
				if (index > length){
					index = length;
				}
				int cursorX = offsetX;
				int cursorY = 0;
				if (length > 0){
					if (index == 0){
						if (rangesSize > 0){
							cursorX = (int)m_ranges[0].left;
							cursorY = (int)m_ranges[0].top;
						}
					}
					else{
						cursorX = (int)ceil(m_ranges[index - 1].right) + 2;
						cursorY = (int)ceil(m_ranges[index - 1].top);
					}
					cursorY += (lineHeight - (int)tSize.cy) / 2;
				}
				else{
					cursorY = (lineHeight - (int)tSize.cy) / 2;
				}
				FCRect cRect = {cursorX - 1 - scrollH, cursorY - scrollV, cursorX + 1 - scrollH, cursorY + (int)tSize.cy - scrollV};
				paint->fillRect(getPaintingTextColor(), cRect);
			}
			else{
				if (!isFocused() && (int)text.length() == 0){
                    if ((int)m_tempText.length() > 0){
                        FCSize pSize = paint->textSize(m_tempText.c_str(), font);
						FCRect pRect = {0};
                        pRect.left = padding.left;
                        pRect.top = (lineHeight - pSize.cy) / 2;
                        pRect.right = pRect.left + pSize.cx;
                        pRect.bottom = pRect.top + pSize.cy;
                        paint->drawText(m_tempText.c_str(), m_tempTextColor, font, pRect);
                    }
                }
			}
		}
	}

	void FCTextBox::onPaste(){
		if (!m_readOnly){
			FCHost *host = getNative()->getHost();
			string str = host->paste();
			if(str.length() > 0){
				int oldLines = (int)m_lines.size();
				String insert = FCStr::stringTowstring(str);
				String redotext = getText();
				insertWord(insert);
				onTextChanged();
				if (m_textChanged){
					m_undoStack.push(redotext);
				}
				int newLines = (int)m_lines.size();
				if (newLines != oldLines){
					FCVScrollBar *vScrollBar = getVScrollBar();
					if(vScrollBar){
						vScrollBar->setPos(vScrollBar->getPos() + m_lineHeight * (newLines - oldLines));
						invalidate();
					}
				}
				update();
				FCDiv::onPaste();
			}	
		}
	}

	void FCTextBox::onTabStop(){
		FCDiv::onTabStop();
		if (!m_multiline){
			int textSize = (int)(getText().size());
			if (textSize > 0){
                m_selectionStart = 0;
                m_selectionLength = textSize;
				onTimer(m_timerID);
			}
		}
	}

	void FCTextBox::onSizeChanged(){
		FCDiv::onSizeChanged();
		if (m_wordWrap){
			m_textChanged = true;
			invalidate();
		}
	}

	void FCTextBox::onTextChanged(){
		m_textChanged = true;
		FCDiv::onTextChanged();
	}

	void FCTextBox::onTimer(int timerID){
		FCDiv::onTimer(timerID);
		if (m_timerID == timerID){
			if(isVisible() && isFocused() && !m_textChanged){
				m_showCursor = !m_showCursor;
				invalidate();
			}
		}
	}

	void FCTextBox::redo(){
		if (canRedo()){
			m_undoStack.push(getText());
			setText(m_redoStack.top());
			m_redoStack.pop();
		}
	}

	void FCTextBox::selectAll(){
		m_selectionStart = 0;
		m_selectionLength = (int)getText().length();
	}

	void FCTextBox::setProperty(const String& name, const String& value){
		if(name == L"lineheight"){
			setLineHeight(FCStr::convertStrToInt(value));
		}
		else if(name == L"multiline"){
			setMultiline(FCStr::convertStrToBool(value));
		}
		else if(name == L"passwordchar"){
			if(value.length() > 0){
				setPasswordChar(value.c_str()[0]);
			}
			else{
				setPasswordChar(0);
			}
		}
		else if(name == L"readonly"){
			setReadOnly(FCStr::convertStrToBool(value));
		}
		else if(name == L"righttoleft"){
			setRightToLeft(FCStr::convertStrToBool(value));
		}
		else if(name == L"selectionbackcolor"){
			setSelectionBackColor(FCStr::convertStrToColor(value));
		}
		else if(name == L"selectionTextColor"){
			setSelectionTextColor(FCStr::convertStrToColor(value));
		}
	    else if (name == L"temptext"){
            setTempText(value);
        }
        else if (name == L"temptextcolor"){
			setTempTextColor(FCStr::convertStrToColor(value));
        }
		else if (name == L"textalign"){
			setTextAlign(FCStr::convertStrToHorizontalAlign(value));
        }
		else if(name == L"wordwrap"){
			setWordWrap(FCStr::convertStrToBool(value));
		}
		else{
			FCDiv::setProperty(name, value);
		}
	}

	void FCTextBox::undo(){
	    if (canUndo()){
            m_redoStack.push(getText());
            setText(m_undoStack.top());
			m_undoStack.pop();
        }
	}

	void FCTextBox::update(){
		FCDiv::update();
		FCNative *native = getNative();
		if(native){
			FCVScrollBar *scrollBar = getVScrollBar();
			if(scrollBar){
				scrollBar->setLineSize(m_lineHeight);
			}
		}
	}
}