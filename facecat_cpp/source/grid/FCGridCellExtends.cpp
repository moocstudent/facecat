#include "..\\..\\stdafx.h"
#include "..\\..\\include\\grid\\FCGridCellExtends.h"
#include "..\\..\\include\\btn\\FCButton.h"
#include "..\\..\\include\\btn\\FCCheckBox.h"
#include "..\\..\\include\\input\\FCComboBox.h"
#include "..\\..\\include\\input\\FCDateTimePicker.h"
#include "..\\..\\include\\div\\FCDiv.h"
#include "..\\..\\include\\Label\\FCLabel.h"
#include "..\\..\\include\\btn\\FCRadioButton.h"
#include "..\\..\\include\\input\\FCSpin.h"
#include "..\\..\\include\\input\\FCTextBox.h"

namespace FaceCat{
	FCGridBoolCell::FCGridBoolCell(){
		m_value = false;
	}

	FCGridBoolCell::FCGridBoolCell(bool value){
		m_value = value;
	}

	FCGridBoolCell::~FCGridBoolCell(){
	}

	int FCGridBoolCell::compareTo(FCGridCell *cell){
		bool value = getBool();
		bool target = cell->getBool();
		if(value && !target){
			return 1;
		}
		else if(!value && target){
			return -1;
		}
		else{
			return 0;
		}
	}

	bool FCGridBoolCell::getBool(){
		return m_value;
	}

	double FCGridBoolCell::getDouble(){
		return m_value ? 1: 0;
	}

	float FCGridBoolCell::getFloat(){
		return (float)(m_value ? 1: 0);
	}

	int FCGridBoolCell::getInt(){
		return m_value ? 1: 0;
	}

	Long FCGridBoolCell::getLong(){
		return m_value ? 1: 0;
	}

	String FCGridBoolCell::getString(){
		return m_value ? L"true": L"false";
	}

	void FCGridBoolCell::setBool(bool value){
		m_value = value;
	}

    void FCGridBoolCell::setDouble(double value){
		m_value = value > 0  ? true : false;
	}

    void FCGridBoolCell::setFloat(float value){
		m_value = value > 0  ? true : false;
	}

    void FCGridBoolCell::setInt(int value){
		m_value = value > 0  ? true : false;
	}

    void FCGridBoolCell::setLong(Long value){
		m_value = value > 0  ? true : false;
	}

    void FCGridBoolCell::setString(const String& value){
		m_value = value == L"true" ? true : false;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////

	FCGridButtonCell::FCGridButtonCell(){
		FCButton *button = new FCButton;
		button->setBorderColor(FCColor_None);
		button->setDisplayOffset(false);
		setControl(button);
	}

	FCGridButtonCell::~FCGridButtonCell(){
	}

	FCButton* FCGridButtonCell::getButton(){
		FCView *control = getControl();
		if(control){
			return dynamic_cast<FCButton*>(control);
		}
		else{
			return 0;
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////

	FCGridCheckBoxCell::FCGridCheckBoxCell(){
		FCCheckBox *checkBox = new FCCheckBox;
		checkBox->setDisplayOffset(false);
		setControl(checkBox);
	}

	FCGridCheckBoxCell::~FCGridCheckBoxCell(){
	}

	FCCheckBox* FCGridCheckBoxCell::getCheckBox(){
		FCView *control = getControl();
		if(control){
			return dynamic_cast<FCCheckBox*>(control);
		}
		else{
			return 0;
		}
	}

	bool FCGridCheckBoxCell::getBool(){
		FCCheckBox *checkBox = getCheckBox();
		if (checkBox){
			return checkBox->isChecked();
		}
		else{
			return false;
		}
	}

	double FCGridCheckBoxCell::getDouble(){
		FCCheckBox *checkBox = getCheckBox();
		if (checkBox){
			return checkBox->isChecked() ? 1 : 0;
		}
		return 0;
	}

	float FCGridCheckBoxCell::getFloat(){
		FCCheckBox *checkBox = getCheckBox();
		if (checkBox){
			return (float)(checkBox->isChecked() ? 1 : 0);
		}
		return 0;
	}

	int FCGridCheckBoxCell::getInt(){
		FCCheckBox *checkBox = getCheckBox();
		if (checkBox){
			return checkBox->isChecked() ? 1 : 0;
		}
		return 0;
	}

	Long FCGridCheckBoxCell::getLong(){
		FCCheckBox *checkBox = getCheckBox();
		if (checkBox){
			return checkBox->isChecked() ? 1 : 0;
		}
		return 0;
	}

	String FCGridCheckBoxCell::getString(){
		FCCheckBox *checkBox = getCheckBox();
		if (checkBox){
			return checkBox->isChecked() ? L"true" : L"false";
		}
		return L"false";
	}

	void FCGridCheckBoxCell::setBool(bool value){
		FCCheckBox *checkBox = getCheckBox();
	    if (checkBox){
            checkBox->setChecked(value);
        }
	}

	void FCGridCheckBoxCell::setDouble(double value){
		FCCheckBox *checkBox = getCheckBox();
	    if (checkBox){
            checkBox->setChecked(value > 0  ? true : false);
        }
	}

    void FCGridCheckBoxCell::setFloat(float value){
		FCCheckBox *checkBox = getCheckBox();
	    if (checkBox){
            checkBox->setChecked(value > 0  ? true : false);
        }
	}

    void FCGridCheckBoxCell::setInt(int value){
		FCCheckBox *checkBox = getCheckBox();
	    if (checkBox){
            checkBox->setChecked(value > 0  ? true : false);
        }
	}

    void FCGridCheckBoxCell::setLong(Long value){
		FCCheckBox *checkBox = getCheckBox();
	    if (checkBox){
            checkBox->setChecked(value > 0  ? true : false);
        }
	}

    void FCGridCheckBoxCell::setString(const String& value){
		FCCheckBox *checkBox = getCheckBox();
	    if (checkBox){
			checkBox->setChecked(value == L"true"  ? true : false);
        }
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////

	FCGridComboBoxCell::FCGridComboBoxCell(){
		FCComboBox *comboBox = new FCComboBox;
		comboBox->setBorderColor(FCColor_None);
		comboBox->setDisplayOffset(false);
		setControl(comboBox);
	}

	FCGridComboBoxCell::~FCGridComboBoxCell(){
	}

	FCComboBox* FCGridComboBoxCell::getComboBox(){
		FCView *control = getControl();
		if(control){
			return dynamic_cast<FCComboBox*>(control);
		}
		else{
			return 0;
		}
	}

	bool FCGridComboBoxCell::getBool(){
		FCComboBox *comboBox = getComboBox();
		if(comboBox){
			return comboBox->getSelectedIndex() > 0;
		}
		else{
			return false;
		}
	}

	double FCGridComboBoxCell::getDouble(){
		FCComboBox *comboBox = getComboBox();
		if(comboBox){
			return (double)comboBox->getSelectedIndex();
		}
		else{
			return false;
		}
	}

	float FCGridComboBoxCell::getFloat(){
		FCComboBox *comboBox = getComboBox();
		if(comboBox){
			return (float)comboBox->getSelectedIndex();
		}
		else{
			return false;
		}
	}

	int FCGridComboBoxCell::getInt(){
		FCComboBox *comboBox = getComboBox();
		if(comboBox){
			return (int)comboBox->getSelectedIndex();
		}
		else{
			return false;
		}
	}

	Long FCGridComboBoxCell::getLong(){
		FCComboBox *comboBox = getComboBox();
		if(comboBox){
			return (Long)comboBox->getSelectedIndex();
		}
		else{
			return false;
		}
	}

	String FCGridComboBoxCell::getString(){
		FCComboBox *comboBox = getComboBox();
		if(comboBox){
			return comboBox->getSelectedValue();
		}
		else{
			return L"";
		}
	}

	void FCGridComboBoxCell::setBool(bool value){
		FCComboBox *comboBox = getComboBox();
		if(comboBox){
			comboBox->setSelectedIndex((int)value);
		}
	}

	void FCGridComboBoxCell::setDouble(double value){
		FCComboBox *comboBox = getComboBox();
		if(comboBox){
			comboBox->setSelectedIndex((int)value);
		}
	}

    void FCGridComboBoxCell::setFloat(float value){
		FCComboBox *comboBox = getComboBox();
		if(comboBox){
			comboBox->setSelectedIndex((int)value);
		}
	}

    void FCGridComboBoxCell::setInt(int value){
		FCComboBox *comboBox = getComboBox();
		if(comboBox){
			comboBox->setSelectedIndex(value);
		}
	}

    void FCGridComboBoxCell::setLong(Long value){
		FCComboBox *comboBox = getComboBox();
		if(comboBox){
			comboBox->setSelectedIndex((int)value);
		}
	}

    void FCGridComboBoxCell::setString(const String& value){
		FCComboBox *comboBox = getComboBox();
		if(comboBox){
			comboBox->setSelectedValue(value);
		}
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	FCGridDateTimePickerCell::FCGridDateTimePickerCell(){
		FCDateTimePicker *datePicker = new FCDateTimePicker;
		datePicker->setBorderColor(FCColor_None);
		datePicker->setDisplayOffset(false);
		setControl(datePicker);
	}

	FCGridDateTimePickerCell::~FCGridDateTimePickerCell(){
	}

	FCDateTimePicker* FCGridDateTimePickerCell::getDatePicker(){
		FCView *control = getControl();
		if(control){
			return dynamic_cast<FCDateTimePicker*>(control);
		}
		else{
			return 0;
		}
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	FCGridDivCell::FCGridDivCell(){
		FCDiv *div = new FCDiv;
		div->setBorderColor(FCColor_None);
		div->setDisplayOffset(false);
		setControl(div);
	}

	FCGridDivCell::~FCGridDivCell(){
	}

	FCDiv* FCGridDivCell::getDiv(){
		FCView *control = getControl();
		if(control){
			return dynamic_cast<FCDiv*>(control);
		}
		else{
			return 0;
		}
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	FCGridDoubleCell::FCGridDoubleCell(){
		m_value = 0;
	}

	FCGridDoubleCell::FCGridDoubleCell(double value){
		m_value = value;
	}

	FCGridDoubleCell::~FCGridDoubleCell(){
	}

	int FCGridDoubleCell::compareTo(FCGridCell *cell){
		double value = getDouble();
		double target = cell->getDouble();
		if(value > target){
			return 1;
		}
		else if(value < target){
			return -1;
		}
		else{
			return 0;
		}
	}

	bool FCGridDoubleCell::getBool(){
		return m_value == 0 ? false : true;
	}

	double FCGridDoubleCell::getDouble(){
		return m_value;
	}

	float FCGridDoubleCell::getFloat(){
		return (float)m_value;
	}

	int FCGridDoubleCell::getInt(){
		return m_value ? 1: 0;
	}

	Long FCGridDoubleCell::getLong(){
		return m_value ? 1: 0;
	}

	String FCGridDoubleCell::getString(){
		wchar_t str[100] = {0};
		_stprintf_s(str, 99, L"%lf", m_value);
		return str;
	}

	void FCGridDoubleCell::setBool(bool value){
		m_value = value ? 1 : 0;
	}

    void FCGridDoubleCell::setDouble(double value){
		m_value = value;
	}

    void FCGridDoubleCell::setFloat(float value){
		m_value = value;
	}

    void FCGridDoubleCell::setInt(int value){
		m_value = value;
	}

    void FCGridDoubleCell::setLong(Long value){
		m_value = (double)value;
	}

    void FCGridDoubleCell::setString(const String& value){
		m_value = FCStr::convertStrToDouble(value.c_str());
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	FCGridFloatCell::FCGridFloatCell(){
		m_value = 0;
	}

	FCGridFloatCell::FCGridFloatCell(float value){
		m_value = value;
	}

	FCGridFloatCell::~FCGridFloatCell(){
	}

	int FCGridFloatCell::compareTo(FCGridCell *cell){
		float value = getFloat();
		float target = cell->getFloat();
		if(value > target){
			return 1;
		}
		else if(value < target){
			return -1;
		}
		else{
			return 0;
		}
	}

	bool FCGridFloatCell::getBool(){
		return m_value == 0 ? false : true;
	}

	double FCGridFloatCell::getDouble(){
		return m_value;
	}

	float FCGridFloatCell::getFloat(){
		return m_value;
	}

	int FCGridFloatCell::getInt(){
		return (int)m_value;
	}

	Long FCGridFloatCell::getLong(){
		return (Long)m_value;
	}

	String FCGridFloatCell::getString(){
		wchar_t str[100] = {0};
		_stprintf_s(str, 99, L"%f", m_value);
		return str;
	}

	void FCGridFloatCell::setBool(bool value){
		m_value = (float)(value ? 1 : 0);
	}

    void FCGridFloatCell::setDouble(double value){
		m_value = (float)value;
	}

    void FCGridFloatCell::setFloat(float value){
		m_value = value;
	}

    void FCGridFloatCell::setInt(int value){
		m_value = (float)value;
	}

    void FCGridFloatCell::setLong(Long value){
		m_value = (float)value;
	}

    void FCGridFloatCell::setString(const String& value){
		m_value = (float)FCStr::convertStrToDouble(value.c_str());
	}

	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	FCGridIntCell::FCGridIntCell(){
		m_value = 0;
	}

	FCGridIntCell::FCGridIntCell(int value){
		m_value = value;
	}

	FCGridIntCell::~FCGridIntCell(){
	}

	int FCGridIntCell::compareTo(FCGridCell *cell){
		int value = getInt();
		int target = cell->getInt();
		if(value > target){
			return 1;
		}
		else if(value < target){
			return -1;
		}
		else{
			return 0;
		}
	}

	bool FCGridIntCell::getBool(){
		return m_value == 0 ? false : true;
	}

	double FCGridIntCell::getDouble(){
		return m_value;
	}

	float FCGridIntCell::getFloat(){
		return (float)m_value;
	}

	int FCGridIntCell::getInt(){
		return (int)m_value;
	}

	Long FCGridIntCell::getLong(){
		return (Long)m_value;
	}

	String FCGridIntCell::getString(){
		wchar_t str[100] = {0};
		_stprintf_s(str, 99, L"%d", m_value);
		return str;
	}

	void FCGridIntCell::setBool(bool value){
		m_value = value ? 1 : 0;
	}

    void FCGridIntCell::setDouble(double value){
		m_value = (int)value;
	}

    void FCGridIntCell::setFloat(float value){
		m_value = (int)value;
	}

    void FCGridIntCell::setInt(int value){
		m_value = value;
	}

    void FCGridIntCell::setLong(Long value){
		m_value = (int)value;
	}

    void FCGridIntCell::setString(const String& value){
		m_value = FCStr::convertStrToInt(value.c_str());
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	FCGridLabelCell::FCGridLabelCell(){
		FCLabel *label = new FCLabel;
		label->setBorderColor(FCColor_None);
		label->setDisplayOffset(false);
		setControl(label);
	}

	FCGridLabelCell::~FCGridLabelCell(){
	}

	FCLabel* FCGridLabelCell::getLabel(){
		FCView *control = getControl();
		if(control){
			return dynamic_cast<FCLabel*>(control);
		}
		else{
			return 0;
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	FCGridLongCell::FCGridLongCell(){
		m_value = 0;
	}

	FCGridLongCell::FCGridLongCell(Long value){
		m_value = value;
	}

	FCGridLongCell::~FCGridLongCell(){
	}

	int FCGridLongCell::compareTo(FCGridCell *cell){
		Long value = getLong();
		Long target = cell->getLong();
		if(value > target){
			return 1;
		}
		else if(value < target){
			return -1;
		}
		else{
			return 0;
		}
	}

	bool FCGridLongCell::getBool(){
		return m_value == 0 ? false : true;
	}

	double FCGridLongCell::getDouble(){
		return (double)m_value;
	}

	float FCGridLongCell::getFloat(){
		return (float)m_value;
	}

	int FCGridLongCell::getInt(){
		return (int)m_value;
	}

	Long FCGridLongCell::getLong(){
		return m_value;
	}

	String FCGridLongCell::getString(){
		wchar_t str[100] = {0};
		_stprintf_s(str, 99, L"%ld", m_value);
		return str;
	}

	void FCGridLongCell::setBool(bool value){
		m_value = value ? 1 : 0;
	}

    void FCGridLongCell::setDouble(double value){
		m_value = (Long)value;
	}

    void FCGridLongCell::setFloat(float value){
		m_value = (Long)value;
	}

    void FCGridLongCell::setInt(int value){
		m_value = value;
	}

    void FCGridLongCell::setLong(Long value){
		m_value = value;
	}

    void FCGridLongCell::setString(const String& value){
		m_value = FCStr::convertStrToInt(value.c_str());
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	FCGridRadioButtonCell::FCGridRadioButtonCell(){
		FCRadioButton *radioButton = new FCRadioButton;
		radioButton->setBorderColor(FCColor_None);
		radioButton->setDisplayOffset(false);
		setControl(radioButton);
	}

	FCGridRadioButtonCell::~FCGridRadioButtonCell(){
	}

	FCRadioButton* FCGridRadioButtonCell::getRadioButton(){
		FCView *control = getControl();
		if(control){
			return dynamic_cast<FCRadioButton*>(control);
		}
		else{
			return 0;
		}
	}

	bool FCGridRadioButtonCell::getBool(){
		FCRadioButton *radioButton = getRadioButton();
	    if (radioButton){
			return radioButton->isChecked();
		}
		else{
			return false;
		}
	}

	double FCGridRadioButtonCell::getDouble(){
		FCRadioButton *radioButton = getRadioButton();
	    if (radioButton){
			return radioButton->isChecked() ? 1 : 0;
		}
		return 0;
	}

	float FCGridRadioButtonCell::getFloat(){
		FCRadioButton *radioButton = getRadioButton();
	    if (radioButton){
			return (float)(radioButton->isChecked() ? 1 : 0);
		}
		return 0;
	}

	int FCGridRadioButtonCell::getInt(){
		FCRadioButton *radioButton = getRadioButton();
	    if (radioButton){
			return radioButton->isChecked() ? 1 : 0;
		}
		return 0;
	}

	Long FCGridRadioButtonCell::getLong(){
		FCRadioButton *radioButton = getRadioButton();
	    if (radioButton){
			return radioButton->isChecked() ? 1 : 0;
		}
		return 0;
	}

	String FCGridRadioButtonCell::getString(){
		FCRadioButton *radioButton = getRadioButton();
	    if (radioButton){
			return radioButton->isChecked() ? L"true" : L"false";
		}
		return L"false";
	}

	void FCGridRadioButtonCell::setBool(bool value){
		FCRadioButton *radioButton = getRadioButton();
	    if (radioButton){
            radioButton->setChecked(value);
        }
	}

	void FCGridRadioButtonCell::setDouble(double value){
		FCRadioButton *radioButton = getRadioButton();
	    if (radioButton){
            radioButton->setChecked(value > 0  ? true : false);
        }
	}

    void FCGridRadioButtonCell::setFloat(float value){
		FCRadioButton *radioButton = getRadioButton();
	    if (radioButton){
            radioButton->setChecked(value > 0  ? true : false);
        }
	}

    void FCGridRadioButtonCell::setInt(int value){
		FCRadioButton *radioButton = getRadioButton();
	    if (radioButton){
            radioButton->setChecked(value > 0  ? true : false);
        }
	}

    void FCGridRadioButtonCell::setLong(Long value){
		FCRadioButton *radioButton = getRadioButton();
	    if (radioButton){
            radioButton->setChecked(value > 0  ? true : false);
        }
	}

    void FCGridRadioButtonCell::setString(const String& value){
		FCRadioButton *radioButton = getRadioButton();
	    if (radioButton){
			radioButton->setChecked(value == L"true"  ? true : false);
        }
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	FCGridSpinCell::FCGridSpinCell(){
		FCSpin *spin = new FCSpin;
		spin->setBorderColor(FCColor_None);
		spin->setDisplayOffset(false);
		setControl(spin);
	}

	FCGridSpinCell::~FCGridSpinCell(){
	}

	FCSpin* FCGridSpinCell::getSpin(){
		FCView *control = getControl();
		if(control){
			return dynamic_cast<FCSpin*>(control);
		}
		else{
			return 0;
		}
	}

	bool FCGridSpinCell::getBool(){
		FCSpin *spin = getSpin();
	    if (spin){
			return spin->getValue() > 0;
		}
		else{
			return false;
		}
	}

	double FCGridSpinCell::getDouble(){
		FCSpin *spin = getSpin();
	    if (spin){
			return (double)spin->getValue();
		}
		else{
			return 0;
		}
	}

	float FCGridSpinCell::getFloat(){
		FCSpin *spin = getSpin();
	    if (spin){
			return (float)spin->getValue();
		}
		else{
			return 0;
		}
	}

	int FCGridSpinCell::getInt(){
		FCSpin *spin = getSpin();
	    if (spin){
			return (int)spin->getValue();
		}
		else{
			return 0;
		}
	}

	Long FCGridSpinCell::getLong(){
		FCSpin *spin = getSpin();
	    if (spin){
			return (Long)spin->getValue();
		}
		else{
			return 0;
		}
	}

	void FCGridSpinCell::setBool(bool value){
		FCSpin *spin = getSpin();
	    if (spin){
			spin->setValue((double)value);
		}
	}

	void FCGridSpinCell::setDouble(double value){
		FCSpin *spin = getSpin();
	    if (spin){
			spin->setValue(value);
		}
	}

    void FCGridSpinCell::setFloat(float value){
		FCSpin *spin = getSpin();
	    if (spin){
			spin->setValue((double)value);
		}
	}

    void FCGridSpinCell::setInt(int value){
		FCSpin *spin = getSpin();
	    if (spin){
			spin->setValue((double)value);
		}
	}

    void FCGridSpinCell::setLong(Long value){
		FCSpin *spin = getSpin();
	    if (spin){
			spin->setValue((double)value);
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	FCGridStringCell::FCGridStringCell(){
	}

	FCGridStringCell::FCGridStringCell(const String& value){
		m_value = value;
	}

	FCGridStringCell::~FCGridStringCell(){
	}

	int FCGridStringCell::compareTo(FCGridCell *cell){
		String target = cell->getString();
		String value = getString();
		if(value > target){
			return 1;
		}
		else if(value < target){
			return -1;
		}
		else{
			return 0;
		}
	}

	bool FCGridStringCell::getBool(){
		return false;
	}

	double FCGridStringCell::getDouble(){
		return 0;
	}

	float FCGridStringCell::getFloat(){
		return 0;
	}

	int FCGridStringCell::getInt(){
		return 0;
	}

	Long FCGridStringCell::getLong(){
		return 0;
	}

	String FCGridStringCell::getString(){
		return m_value;
	}

	void FCGridStringCell::setString(const String& value){
		m_value = value;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	FCGridPasswordCell::FCGridPasswordCell(){
	}

	FCGridPasswordCell::~FCGridPasswordCell(){
	}

	String FCGridPasswordCell::getPaintText(){
		return L"******";
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	FCGridTextBoxCell::FCGridTextBoxCell(){
		FCTextBox *textBox = new FCTextBox;
		textBox->setBorderColor(FCColor_None);
		textBox->setDisplayOffset(false);
		setControl(textBox);
	}

	FCGridTextBoxCell::~FCGridTextBoxCell(){
	}

	FCTextBox* FCGridTextBoxCell::getTextBox(){
		FCView *control = getControl();
		if(control){
			return dynamic_cast<FCTextBox*>(control);
		}
		else{
			return 0;
		}
	}
}