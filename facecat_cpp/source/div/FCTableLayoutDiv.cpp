#include "..\\..\\stdafx.h"
#include "..\\..\\include\\div\\FCTableLayoutDiv.h"

namespace FaceCat{
	FCColumnStyle::FCColumnStyle(){
		m_sizeType = FCSizeType_PercentSize;
		m_width = 0;
	}

	FCColumnStyle::FCColumnStyle(FCSizeType sizeType, float width){
		m_sizeType = sizeType;
		m_width = width;
	}

	FCColumnStyle::~FCColumnStyle(){
	}

	FCSizeType FCColumnStyle::getSizeType(){
		return m_sizeType;
	}

	void FCColumnStyle::setSizeTypeA(FCSizeType  sizeType){
		m_sizeType = sizeType;
	}

	float FCColumnStyle::getWidth(){
		return m_width;
	}

	void FCColumnStyle::setWidth(float width){
		m_width = width;
	}

	void FCColumnStyle::getProperty(const String& name, String *value, String *type){
	    if (name == L"sizetype"){
            *type = L"enum:FCSizeType";
            if (m_sizeType == FCSizeType_AbsoluteSize){
                *value = L"absolutesize";
            }
            else if (m_sizeType == FCSizeType_AutoFill){
                *value = L"autofill";
            }
            else if (m_sizeType == FCSizeType_PercentSize){
                *value = L"percentsize";
            }
        }
        else if (name == L"width"){
            *type = L"float";
			*value = FCStr::convertFloatToStr(getWidth());
        }
	}

	ArrayList<String> FCColumnStyle::getPropertyNames(){
	    ArrayList<String> propertyNames;
        propertyNames.add(L"SizeType");
		propertyNames.add(L"Height");
        return propertyNames;
	}

	void FCColumnStyle::setProperty(const String& name, const String& value){
	    if (name == L"sizetype"){
			String lowerStr = FCStr::toLower(value);
            if (value == L"absolutesize"){
                m_sizeType = FCSizeType_AbsoluteSize;
            }
            else if (value == L"autofill"){
                m_sizeType = FCSizeType_AutoFill;
            }
            else if (value == L"percentsize"){
                m_sizeType = FCSizeType_PercentSize;
            }
        }
        else if (name == L"width"){
			setWidth(FCStr::convertStrToFloat(value));
        }
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	FCRowStyle::FCRowStyle(){
		m_sizeType = FCSizeType_PercentSize;
		m_height = 0;
	}
	
	FCRowStyle::FCRowStyle(FCSizeType sizeType, float height){
		m_height = height;
		m_sizeType = sizeType;
	}

	FCRowStyle::~FCRowStyle(){
	}

	float FCRowStyle::getHeight(){
		return m_height;
	}

	void FCRowStyle::setHeight(float height){
		m_height = height;
	}

	FCSizeType FCRowStyle::getSizeType(){
		return m_sizeType;
	}

	void FCRowStyle::setSizeTypeA(FCSizeType  sizeType){
		m_sizeType = sizeType;
	}

	void FCRowStyle::getProperty(const String& name, String *value, String *type){
	    if (name == L"sizetype"){
            *type = L"enum:FCSizeType";
            if (m_sizeType == FCSizeType_AbsoluteSize){
                *value = L"absolutesize";
            }
            else if (m_sizeType == FCSizeType_AutoFill){
                *value = L"autofill";
            }
            else if (m_sizeType == FCSizeType_PercentSize){
                *value = L"percentsize";
            }
        }
        else if (name == L"height"){
            *type = L"float";
			*value = FCStr::convertFloatToStr(getHeight());
        }
	}

	ArrayList<String> FCRowStyle::getPropertyNames(){
	    ArrayList<String> propertyNames;
        propertyNames.add(L"SizeType");
        propertyNames.add(L"Height");
        return propertyNames;
	}

	void FCRowStyle::setProperty(const String& name, const String& value){
	    if (name == L"sizetype"){
			String lowerStr = FCStr::toLower(value);
            if (value == L"absolutesize"){
                m_sizeType = FCSizeType_AbsoluteSize;
            }
            else if (value == L"autofill"){
                m_sizeType = FCSizeType_AutoFill;
            }
            else if (value == L"percentsize"){
                m_sizeType = FCSizeType_PercentSize;
            }
        }
        else if (name == L"height"){
			setHeight(FCStr::convertStrToFloat(value));
        }
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	FCTableLayoutDiv::FCTableLayoutDiv(){
		m_columnsCount = 0;
		m_rowsCount = 0;
	}

	FCTableLayoutDiv::~FCTableLayoutDiv(){
        m_columns.clear();
        m_columnStyles.clear();
        m_rows.clear();
        m_rowStyles.clear();
        m_tableControls.clear();
	}

	int FCTableLayoutDiv::getColumnsCount(){
		return m_columnsCount;
	}

	void FCTableLayoutDiv::setColumnsCount(int columnsCount){
		m_columnsCount = columnsCount;
	}

	int FCTableLayoutDiv::getRowsCount(){
		return m_rowsCount;
	}

	void FCTableLayoutDiv::setRowsCount(int rowsCount){
		m_rowsCount = rowsCount;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void FCTableLayoutDiv::addControl(FCView *control){
		ArrayList<FCView*> controls = getControls();
        int controlsSize = (int)controls.size();
		FCDiv::addControl(control);
        int column = controlsSize % m_columnsCount;
        int row = controlsSize / m_columnsCount;
        m_columns.add(column);
        m_rows.add(row);
        m_tableControls.add(control);
	}

	void FCTableLayoutDiv::addControl(FCView *control, int column, int row){
		FCDiv::addControl(control);
        m_columns.add(column);
        m_rows.add(row);
        m_tableControls.add(control);
	}

	String FCTableLayoutDiv::getControlType(){
		return L"TableLayoutDiv";
	}

	void FCTableLayoutDiv::getProperty(const String& name, String *value, String *type){
        if (name == L"columnscount"){
            *type = L"int";
			*value = FCStr::convertIntToStr(getColumnsCount());
        }
        else if (name == L"rowscount"){
            *type = L"int";
			*value = FCStr::convertIntToStr(getRowsCount());
        }
		else{
			FCDiv::getProperty(name, value, type);
		}
	}

	ArrayList<String> FCTableLayoutDiv::getPropertyNames(){
		ArrayList<String> propertyNames = FCDiv::getPropertyNames();
		propertyNames.add(L"ColumnsCount");
		propertyNames.add(L"RowsCount");
		return propertyNames;
	}

	bool FCTableLayoutDiv::onResetLayout(){
		if (getNative()){
			if(m_columnsCount > 0 && m_rowsCount > 0 && (int)m_columnStyles.size() > 0 && (int)m_rowStyles.size() > 0){
				int width = getWidth(), height = getHeight();
				int tabControlsSize = m_tableControls.size();
				int *columnWidths = new int[m_columnsCount];
				int *rowHeights = new int[m_rowsCount];
				int allWidth = 0, allHeight = 0;
				for (int i = 0; i < m_columnsCount; i++){
					FCColumnStyle columnStyle = m_columnStyles.get(i);
					int cWidth = 0;
					FCSizeType sizeType = columnStyle.getSizeType();
					float sWidth = columnStyle.getWidth();
					if (sizeType == FCSizeType_AbsoluteSize){
						cWidth = (int)(sWidth);
					}
				    else if (sizeType == FCSizeType_AutoFill){
                        cWidth = width - allWidth;
                    }
					else if (sizeType == FCSizeType_PercentSize){
						cWidth = (int)(width * sWidth);
					}
					columnWidths[i] = cWidth;
					allWidth += cWidth;
				}
				for (int i = 0; i < m_rowsCount; i++){
					FCRowStyle rowStyle = m_rowStyles.get(i);
					int rHeight = 0;
					FCSizeType sizeType = rowStyle.getSizeType();
					float sHeight = rowStyle.getHeight();
					if (sizeType == FCSizeType_AbsoluteSize){
						rHeight = (int)(sHeight);
					}
					else if (sizeType == FCSizeType_AutoFill){
                        rHeight = height - allHeight;
                    }
					else if (sizeType == FCSizeType_PercentSize){
						rHeight = (int)(height * sHeight);
					}
					rowHeights[i] = rHeight;
					allHeight += rHeight;
				}
				for (int i = 0; i < tabControlsSize; i++){
					FCView *control = m_tableControls.get(i);
					int column = m_columns.get(i);
					int row = m_rows.get(i);
					FCPadding margin = control->getMargin();
					int cLeft = 0, cTop = 0;
					for (int j = 0; j < column; j++){
						cLeft += columnWidths[j];
					}
					for (int j = 0; j < row; j++){
						cTop += rowHeights[j];
					}
					int cRight = cLeft + columnWidths[column] - margin.right;
					int cBottom = cTop + rowHeights[row] - margin.bottom;
					cLeft += margin.left;
					cTop += margin.top;
					if (cRight < cLeft){
						cRight = cLeft;
					}
					if (cBottom < cTop){
						cBottom = cTop;
					}
					FCRect bounds = {cLeft, cTop, cRight, cBottom};
					control->setBounds(bounds);
				}
			}
		}
		return true;
	}

	void FCTableLayoutDiv::removeControl(FCView *control){
	    int tabControlsSize = m_tableControls.size();
        int index = -1;
        for (int i = 0; i < tabControlsSize; i++){
            if (control == m_tableControls.get(i)){
                index = i;
                break;
            }
        }
        if (index != -1){
			m_columns.removeAt(index);
            m_rows.removeAt(index);
            m_tableControls.removeAt(index);
        }
		FCDiv::removeControl(control);
	}

	void FCTableLayoutDiv::setProperty(const String& name, const String& value){
        if (name == L"columnscount"){
			setColumnsCount(FCStr::convertStrToInt(value));
        }
        else if (name == L"rowscount"){
			setRowsCount(FCStr::convertStrToInt(value));
        }
        else{
			FCDiv::setProperty(name, value);
        }
	}

	void FCTableLayoutDiv::update(){
		onResetLayout();
		int controlsSize = (int)m_controls.size();
        for (int i = 0; i < controlsSize; i++){
            m_controls.get(i)->update();
        }
		updateScrollBar();
	}
}