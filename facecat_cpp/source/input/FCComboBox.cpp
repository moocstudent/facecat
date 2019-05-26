#include "..\\..\\stdafx.h"
#include "..\\..\\include\\input\\FCComboBox.h"

namespace FaceCat{
	FCComboBoxMenu::FCComboBoxMenu(){
		m_comboBox = 0;
	}

	FCComboBoxMenu::~FCComboBoxMenu(){
		m_comboBox = 0;
	}

	FCComboBox* FCComboBoxMenu::getComboBox(){
		return m_comboBox;
	}

	void FCComboBoxMenu::setComboBox(FCComboBox *comboBox){
		m_comboBox = comboBox;
	}

	bool FCComboBoxMenu::onAutoHide(){
		if(m_comboBox && m_comboBox->isFocused()){
			return false;
		}
		return true;
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void FCComboBox::dropDownButtonTouchDown(Object sender, FCTouchInfo touchInfo, Object pInvoke){
		FCComboBox *comboBox = (FCComboBox*)pInvoke;
		if(comboBox){
			comboBox->onDropDownOpening();
		}
	}

	void FCComboBox::menuItemClick(Object sender, FCMenuItem *item, FCTouchInfo touchInfo, Object pInvoke){
		FCComboBox *comboBox = (FCComboBox*)pInvoke;
		comboBox->setText(item->getText());
		ArrayList<FCMenuItem*> items = comboBox->getItems();
        int itemSize = (int)items.size();
        for (int i = 0; i < itemSize; i++){
            if (items.get(i) == item){
                comboBox->setSelectedIndex(i);
                break;
            }
        }
		comboBox->setSelectionStart((int)comboBox->getText().length());
		comboBox->invalidate();
	}

	void FCComboBox::menuKeyDown(Object sender, char key, Object pInvoke){
		if(key == 13){
			FCComboBox *comboBox = (FCComboBox*)pInvoke;
			if(comboBox){
				comboBox->onSelectedIndexChanged();
			}
		}
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	FCComboBox::FCComboBox(){
		m_dropDownButton = 0;
		m_dropDownButtonTouchDownEvent = dropDownButtonTouchDown;
		m_dropDownMenu = 0;
		m_menuItemClickEvent = menuItemClick;
		m_menuKeyDownEvent = menuKeyDown;
	}

	FCComboBox::~FCComboBox(){
		if(m_dropDownButton){
			if(m_dropDownButtonTouchDownEvent){
				m_dropDownButton->removeEvent(m_dropDownButtonTouchDownEvent, FCEventID::TOUCHDOWN);
				m_dropDownButtonTouchDownEvent = 0;
			}
			m_dropDownButton = 0;
		}
		if(m_dropDownMenu){
			if(m_menuItemClickEvent){
				m_dropDownMenu->removeEvent(m_menuItemClickEvent, FCEventID::MENUITEMCLICK);
				m_menuItemClickEvent = 0;
			}
			if(m_menuKeyDownEvent){
				m_dropDownMenu->removeEvent(m_menuKeyDownEvent, FCEventID::KEYDOWN);
				m_menuKeyDownEvent = 0;
			}
			getNative()->removeControl(m_dropDownMenu);
			delete m_dropDownMenu;
			m_dropDownMenu = 0;
		}
	}

	FCButton* FCComboBox::getDropDownButton(){
		return m_dropDownButton;
	}

	FCComboBoxMenu* FCComboBox::getDropDownMenu(){
		return m_dropDownMenu;
	}

	int FCComboBox::getSelectedIndex(){
		if (m_dropDownMenu){
			ArrayList<FCMenuItem*> items = m_dropDownMenu->m_items;
			int itemSize = (int)items.size();
			for (int i = 0; i < itemSize; i++){
				FCMenuItem *item = items.get(i);
				if (item->isChecked()){
					return i;
				}
			}
		}
		return -1;
	}

	void FCComboBox::setSelectedIndex(int selectedIndex){
		if (m_dropDownMenu){
			ArrayList<FCMenuItem*> items = m_dropDownMenu->m_items;
			int itemSize = (int)items.size();
			bool changed = false;
			for (int i = 0; i < itemSize; i++){
				FCMenuItem *item = items.get(i);
				if (i == selectedIndex){
					 if (!item->isChecked()){
						 item->setChecked(true);
						 changed = true;
					 }
					 setText(item->getText());
				 }
				 else{
					 item->setChecked(false);
				 }
			 }
			if(changed){
				onSelectedIndexChanged();
			}
		}
	}

	String FCComboBox::getSelectedText(){
		if (m_dropDownMenu){
			ArrayList<FCMenuItem*> items = m_dropDownMenu->m_items;
			int itemSize = (int)items.size();
			for (int i = 0; i < itemSize; i++){
				FCMenuItem *item = items.get(i);
				if (item->isChecked()){
					return item->getText();
				}
			}
		}
		return L"";
	}

	void FCComboBox::setSelectedText(const String& selectedText){
		if (m_dropDownMenu){
			ArrayList<FCMenuItem*> items = m_dropDownMenu->m_items;
			int itemSize = (int)items.size();
			bool changed = false;
			for (int i = 0; i < itemSize; i++){
				FCMenuItem *item = items.get(i);
				if (item->getText() == selectedText){
					 if (!item->isChecked()){
						 item->setChecked(true);
						 changed = true;
					 }
					 setText(item->getText());
				 }
				 else{
					 item->setChecked(false);
				 }
			 }
			if(changed){
				onSelectedIndexChanged();
			}
		}
	}

	String FCComboBox::getSelectedValue(){
		if (m_dropDownMenu){
			ArrayList<FCMenuItem*> items = m_dropDownMenu->m_items;
			int itemSize = (int)items.size();
			for (int i = 0; i < itemSize; i++){
				FCMenuItem *item = items.get(i);
				if (item->isChecked()){
					return item->getValue();
				}
			}
		}
		return L"";
	}

	void FCComboBox::setSelectedValue(const String& selectedValue){
		if (m_dropDownMenu){
			ArrayList<FCMenuItem*> items = m_dropDownMenu->m_items;
			int itemSize = (int)items.size();
			bool changed = false;
			for (int i = 0; i < itemSize; i++){
				FCMenuItem *item = items.get(i);
				if (item->getValue() == selectedValue){
					 if (!item->isChecked()){
						 item->setChecked(true);
						 changed = true;
					 }
					 setText(item->getText());
				 }
				 else{
					 item->setChecked(false);
				 }
			 }
			if(changed){
				onSelectedIndexChanged();
			}
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void FCComboBox::addItem(FCMenuItem *item){
		if(m_dropDownMenu){
			m_dropDownMenu->addItem(item);
		}
	}

	void FCComboBox::clearItems(){
		if(m_dropDownMenu){
			m_dropDownMenu->clearItems();
		}
	}

	String FCComboBox::getControlType(){
		return L"ComboBox";
	}

	ArrayList<FCMenuItem*> FCComboBox::getItems(){
		ArrayList<FCMenuItem*> items;
		if(m_dropDownMenu){
			items = m_dropDownMenu->m_items;
		}
		return items;
	}

	void FCComboBox::getProperty(const String& name, String *value, String *type){
		if(name == L"selectedindex"){
			*type = L"int";
			*value = FCStr::convertIntToStr(getSelectedIndex());
		}
		else if(name == L"selectedtext"){
			*type = L"text";
			*value = getSelectedText();
		}
		else if(name == L"selectedvalue"){
			*type = L"text";
			*value = getSelectedValue();
		}
		else{
			FCTextBox::getProperty(name, value, type);
		}
	}

	ArrayList<String> FCComboBox::getPropertyNames(){
		ArrayList<String> propertyNames = FCTextBox::getPropertyNames();
		propertyNames.add(L"SelectedIndex");
		propertyNames.add(L"SelectedText");
		propertyNames.add(L"SelectedValue");
		return propertyNames;
	}

	void FCComboBox::insertItem(int index, FCMenuItem *item){
		if(m_dropDownMenu){
			m_dropDownMenu->insertItem(index, item);
		}
	}

	void FCComboBox::onDropDownOpening(){
		FCComboBoxMenu *menu = getDropDownMenu();
		menu->setNative(getNative());
		FCPoint popupPoint = {0, getHeight()};
		FCPoint nativePoint = pointToNative(popupPoint);
		menu->setLocation(nativePoint);
		FCSize mSize = {getWidth(), menu->getContentHeight()};
		menu->setSize(mSize);
		menu->setVisible(true);
		menu->bringToFront();
		menu->invalidate();
	}

	void FCComboBox::onKeyDown(char key){
		FCTextBox::onKeyDown(key);
		FCHost *host = getNative()->getHost();
		if(!host->isKeyPress(VK_CONTROL)
		&& !host->isKeyPress(VK_MENU)
		&& !host->isKeyPress(VK_SHIFT)){
			if(getLinesCount() <= 1){
				if(key == 13 || key == 38 || key == 40){
					if(m_dropDownMenu){
						m_dropDownMenu->onKeyDown(key);
					}
				}
			}
		}
	}
	
	void FCComboBox::onLoad(){
		FCTextBox::onLoad();
		FCHost *host = getNative()->getHost();
		if (!m_dropDownButton){
			m_dropDownButton = dynamic_cast<FCButton*>(host->createInternalControl(this, L"dropdownbutton"));
			addControl(m_dropDownButton);
			m_dropDownButton->addEvent(m_dropDownButtonTouchDownEvent, FCEventID::TOUCHDOWN, this);
		}
		if (!m_dropDownMenu){
			m_dropDownMenu = dynamic_cast<FCComboBoxMenu*>(host->createInternalControl(this, L"dropdownmenu"));
			getNative()->addControl(m_dropDownMenu);
			m_dropDownMenu->setVisible(false);
			m_dropDownMenu->addEvent(m_menuItemClickEvent, FCEventID::MENUITEMCLICK, this);
			m_dropDownMenu->addEvent(m_menuKeyDownEvent, FCEventID::KEYDOWN, this);
		}
		else{
			m_dropDownMenu->setNative(getNative());
		}
	}

	void FCComboBox::onSelectedIndexChanged(){
		callEvents(FCEventID::SELECTEDINDEXCHANGED);
	}

	void FCComboBox::onTouchWheel(FCTouchInfo touchInfo){
		FCTextBox::onTouchWheel(touchInfo);
		if(getLinesCount() <= 1){
			if(m_dropDownMenu){
				FCTouchInfo newTouchInfo = touchInfo;
                newTouchInfo.m_firstPoint = m_dropDownMenu->getTouchPoint();
                newTouchInfo.m_secondPoint = m_dropDownMenu->getTouchPoint();
				m_dropDownMenu->onTouchWheel(newTouchInfo);
			}
		}
	}

	void FCComboBox::removeItem(FCMenuItem *item){
		if(m_dropDownMenu){
			m_dropDownMenu->removeItem(item);
		}
	}

	void FCComboBox::setProperty(const String& name, const String& value){
		if(name == L"selectedindex"){
			setSelectedIndex(FCStr::convertStrToInt(value));
		}
		else if(name == L"selectedtext"){
			setSelectedText(value);
		}
		else if(name == L"selectedvalue"){
			setSelectedValue(value);
		}
		else{
			FCTextBox::setProperty(name, value);
		}
	}

	void FCComboBox::update(){
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