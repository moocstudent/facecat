#include "..\\..\\stdafx.h"
#include "..\\..\\include\\btn\\FCRadioButton.h"

namespace FaceCat{
	FCRadioButton::FCRadioButton(){
	}

	FCRadioButton::~FCRadioButton(){
	}

	String FCRadioButton::getGroupName(){
		return m_groupName;
	}

	void FCRadioButton::setGroupName(const String& groupName){
		m_groupName = groupName;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	String FCRadioButton::getControlType(){
		return L"RadioButton";
	}

	void FCRadioButton::getProperty(const String& name, String *value, String *type){
		if(name == L"groupname"){
			*type = L"text";
			*value = getGroupName();
		}
		else{
			FCCheckBox::getProperty(name, value, type);
		}
	}

	ArrayList<String> FCRadioButton::getPropertyNames(){
		ArrayList<String> propertyNames = FCButton::getPropertyNames();
		propertyNames.add(L"GroupName");
		return propertyNames;
	}

	void FCRadioButton::onClick(FCTouchInfo touchInfo){
		if(!isChecked()){
			setChecked(!isChecked());
		}
		callTouchEvents(FCEventID::CLICK, touchInfo);
		invalidate();
	}

	void FCRadioButton::onPaintCheckButton(FCPaint *paint, const FCRect& clipRect){
		String bkImage = getPaintingBackImage();
		if (bkImage.length() > 0){
			paint->drawImage(bkImage.c_str(), clipRect);
		}
		else{
			if(isChecked()){
				FCRect innerRect = {clipRect.left + 2, clipRect.top + 2, clipRect.right - 3, clipRect.bottom - 3};
				if (clipRect.right - clipRect.left < 4 || clipRect.bottom - clipRect.top < 4){
					innerRect = clipRect;
				}
				paint->fillEllipse(getPaintingButtonBackColor(), innerRect);
			}
			paint->drawEllipse(getPaintingButtonBorderColor(), 1, 0, clipRect);
		}
	}

	void FCRadioButton::setProperty(const String& name, const String& value){
		if(name == L"groupname"){
			setGroupName(value);
		}
		else{
			FCCheckBox::setProperty(name, value);
		}
	}

	void FCRadioButton::update(){
		if (isChecked()){
			ArrayList<FCView*> controls;
			if (getParent()){
				controls = getParent()->m_controls;
			}
			else{
				controls = getNative()->m_controls;
			}

			for(int c = 0; c < controls.size(); c++){
				 FCRadioButton *radioButton = (FCRadioButton*)dynamic_cast<FCRadioButton*>(controls.get(c));
				 if(radioButton && radioButton != this){
					if (radioButton->getGroupName() == getGroupName() && radioButton->isChecked()){
						radioButton->setChecked(false);
						radioButton->invalidate();
					}
				 }
			}
		}
	}
}