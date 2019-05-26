#include "..\\..\\stdafx.h"
#include "..\\..\\include\\btn\\FCCheckBox.h"

namespace FaceCat{
	Long FCCheckBox::getPaintingBackColor(){
		Long backColor = getBackColor();
		if (backColor != FCColor_None && FCColor_DisabledBack != FCColor_None){
            if (!isPaintEnabled(this)){
				return FCColor_DisabledBack;
            }
        }
        return backColor;
	}

	Long FCCheckBox::getPaintingButtonBackColor(){
	    Long buttonBackColor = m_buttonBackColor;
		if (buttonBackColor != FCColor_None && FCColor_DisabledBack != FCColor_None){
            if (!isPaintEnabled(this)){
				return FCColor_DisabledBack;
            }
        }
        return buttonBackColor;
	}

	Long FCCheckBox::getPaintingButtonBorderColor(){
		return m_buttonBorderColor;
	}

	String FCCheckBox::getPaintingBackImage(){
		String backImage;
		if (m_checked){
			if (isEnabled()){
				FCNative *native = getNative();
				if (this == native->getPushedControl()){
					backImage = m_checkPushedBackImage;
				}
				else if (this == native->getHoveredControl()){
					backImage = m_checkHoveredBackImage;
				}
				else{
					backImage = m_checkedBackImage;
				}
			}
			else{
				backImage = m_disableCheckedBackImage;
			}
		}

		if(backImage.length() > 0){
			return backImage;
		}
		else
		{
			return FCButton::getPaintingBackImage();
		}
	}


	//////////////////////////////////////////////////////////////////////////////////////////////////////

	FCCheckBox::FCCheckBox(){
		m_buttonAlign = FCHorizontalAlign_Left;
		m_buttonBackColor = FCColor_Border;
		m_buttonBorderColor = FCColor_Border;
		m_buttonSize.cx = 16;
		m_buttonSize.cy = 16;
		m_checked = false;
		setBackColor(FCColor_None);
		setBorderColor(FCColor_None);
	}

	FCCheckBox::~FCCheckBox(){
	}

	FCHorizontalAlign FCCheckBox::getButtonAlign(){
		return m_buttonAlign;
	}

	void FCCheckBox::setButtonAlign(FCHorizontalAlign buttonAlign){
		m_buttonAlign = buttonAlign;
	}

	Long FCCheckBox::getButtonBackColor(){
		return m_buttonBackColor;
	}

	void FCCheckBox::setButtonBackColor(Long buttonBackColor){
		m_buttonBackColor = buttonBackColor;
	}

	Long FCCheckBox::getButtonBorderColor(){
		return m_buttonBorderColor;
	}

	void FCCheckBox::setButtonBorderColor(Long buttonBorderColor){
		m_buttonBorderColor = buttonBorderColor;
	}

	FCSize FCCheckBox::getButtonSize(){
		return m_buttonSize;
	}

	void FCCheckBox::setButtonSize(FCSize buttonSize){
		m_buttonSize = buttonSize;
	}

	bool FCCheckBox::isChecked(){
		return m_checked;
	}

	void FCCheckBox::setChecked(bool checked){
		if(m_checked != checked){
			 m_checked = checked;
			 onCheckedChanged();
		}
	}

	String FCCheckBox::getCheckedBackImage(){
		return m_checkedBackImage;
	}

	void FCCheckBox::setCheckedBackImage(const String& checkedBackImage){
		m_checkedBackImage = checkedBackImage;
	}

	String FCCheckBox::getCheckHoveredBackImage(){
		return m_checkHoveredBackImage;
	}

	void FCCheckBox::setCheckHoveredBackImage(const String& checkHoveredBackImage){
		m_checkHoveredBackImage = checkHoveredBackImage;
	}

	String FCCheckBox::getCheckPushedBackImage(){
		return m_checkPushedBackImage;
	}

	void FCCheckBox::setCheckPushedBackImage(const String& checkPushedBackImage){
		m_checkPushedBackImage = checkPushedBackImage;
	}

	String FCCheckBox::getDisableCheckedBackImage(){
		return m_disableCheckedBackImage;
	}

	void FCCheckBox::setDisableCheckedBackImage(const String& disableCheckedBackImage){
		m_disableCheckedBackImage = disableCheckedBackImage;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////
	String FCCheckBox::getControlType(){
		return L"CheckBox";
	}

	void FCCheckBox::getProperty(const String& name, String *value, String *type){
		if (name == L"buttonalign"){
            *type = L"enum:FCHorizontalAlign";
			*value = FCStr::convertHorizontalAlignToStr(getButtonAlign());
        }
		else if(name == L"buttonsize"){
			*type = L"size";
			*value = FCStr::convertSizeToStr(getButtonSize());
		}
		else if(name == L"checked"){
			*type = L"bool";
			*value = FCStr::convertBoolToStr(isChecked());
		}
		else if(name == L"checkedbackimage"){
			*type = L"text";
			*value = getCheckedBackImage();
		}
		else if(name == L"checkhoveredbackimage"){
			*type = L"text";
			*value = getCheckHoveredBackImage();
		}
		else if(name == L"checkpushedbackimage"){
			*type = L"text";
			*value = getCheckPushedBackImage();
		}
		else if(name == L"disablecheckedbackimage"){
			*type = L"text";
			*value = getDisableCheckedBackImage();
		}
		else{
			FCButton::getProperty(name, value, type);
		}
	}

	ArrayList<String> FCCheckBox::getPropertyNames(){
		ArrayList<String> propertyNames = FCButton::getPropertyNames();
		propertyNames.add(L"FCButtonlign");
		propertyNames.add(L"ButtonSize");
		propertyNames.add(L"Checked");
		propertyNames.add(L"CheckedBackImage");
		propertyNames.add(L"CheckHoveredBackimage");
		propertyNames.add(L"CheckPushedBackImage");
		propertyNames.add(L"DisableCheckedBackImage");
		return propertyNames;
	}

	void FCCheckBox::onCheckedChanged(){
		callEvents(FCEventID::CHECKEDCHANGED);
		update();
	}

	void FCCheckBox::onClick(FCTouchInfo touchInfo){
		setChecked(!isChecked());
		callTouchEvents(FCEventID::CLICK, touchInfo);
		invalidate();
	}

	void FCCheckBox::onPaintBackground(FCPaint *paint, const FCRect& clipRect){
		FCRect rect = {0, 0, getWidth(), getHeight()};
		paint->fillRoundRect(getPaintingBackColor(), rect, m_cornerRadius);
	}

	void FCCheckBox::onPaintCheckButton(FCPaint *paint, const FCRect& clipRect){
		String bkImage = getPaintingBackImage();
		if (bkImage.length() > 0){
			paint->drawImage(bkImage.c_str(), clipRect);
		}
		else{
			if(m_checked){
				FCRect innerRect = {clipRect.left + 2, clipRect.top + 2, clipRect.right - 2, clipRect.bottom - 2};
				if (clipRect.right - clipRect.left < 4 || clipRect.bottom - clipRect.top < 4){
					innerRect = clipRect;
				}
				paint->fillRect(getPaintingButtonBackColor(), innerRect);
			}
            paint->drawRect(getPaintingButtonBorderColor(), 1, 0, clipRect);
		}
	}

	void FCCheckBox::onPaintForeground(FCPaint *paint, const FCRect& clipRect){
        String text = getText();
        int width = getWidth(), height = getHeight();
		if(width > 0 && height > 0){
			FCRect buttonRect = {5, (height - m_buttonSize.cy) / 2, 5 + m_buttonSize.cx, (height + m_buttonSize.cy) / 2};
			FCPoint tLocation = {0};
			FCSize tSize = {0};
			FCFont *font = getFont();
			if (text.length() > 0){
				tSize = paint->textSize(text.c_str(), font);
				tLocation.x = buttonRect.right + 5;
				tLocation.y = (height - tSize.cy) / 2;
			}
			if (m_buttonAlign == FCHorizontalAlign_Center){
				buttonRect.left = (width - m_buttonSize.cx) / 2;
				buttonRect.right = (width + m_buttonSize.cx) / 2;
				tLocation.x = buttonRect.right + 5;
			}
			else if (m_buttonAlign == FCHorizontalAlign_Right){
				buttonRect.left = width - m_buttonSize.cx - 5;
				buttonRect.right = width - 5;
				tLocation.x = buttonRect.left - tSize.cx - 5;
			}
			onPaintCheckButton(paint, buttonRect);

			if (text.length() > 0){
				FCRect tRect = {tLocation.x, tLocation.y, tLocation.x + tSize.cx + 1, tLocation.y + tSize.cy};
				Long textColor = getPaintingTextColor();
				if(autoEllipsis() && (tRect.right < clipRect.right || tRect.bottom < clipRect.bottom)){
					if(tRect.right < clipRect.right){
						tRect.right = clipRect.right;
					}
					if(tRect.bottom < clipRect.bottom){
						tRect.bottom = clipRect.bottom;
					}
					paint->drawTextAutoEllipsis(text.c_str(), textColor, font, tRect);
				}else
				{
					paint->drawText(text.c_str(), textColor, font, tRect);
				}
			}
		}
	}

	void FCCheckBox::setProperty(const String& name, const String& value){
		if (name == L"buttonalign"){
			setButtonAlign(FCStr::convertStrToHorizontalAlign(value));
        }
		else if(name == L"buttonsize"){
			setButtonSize(FCStr::convertStrToSize(value));
		}
		else if(name == L"checked"){
			setChecked(FCStr::convertStrToBool(value));
		}
		else if(name == L"checkedbackimage"){
			setCheckedBackImage(value);
		}
		else if(name == L"checkhoveredbackimage"){
			setCheckHoveredBackImage(value);
		}
		else if(name == L"checkpushedbackimage"){
			setCheckPushedBackImage(value);
		}
		else if(name == L"disablecheckedbackimage"){
			setDisableCheckedBackImage(value);
		}
		else{
			FCButton::setProperty(name, value);
		}
	}
}