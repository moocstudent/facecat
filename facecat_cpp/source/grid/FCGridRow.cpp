#include "..\\..\\stdafx.h"
#include "..\\..\\include\\grid\\FCGridRow.h"

namespace FaceCat{
	FCGridRowStyle::FCGridRowStyle(){
		m_backColor = FCColor_Back;
		m_font = new FCFont(L"Simsun", 14, false, false, false);;
		m_textColor = FCColor_Text;
		m_hoveredBackColor = FCColor::argb(150, 150, 150);
		m_hoveredTextColor = FCColor_Text;
		m_selectedBackColor = FCColor::argb(100, 100, 100);
		m_selectedTextColor = FCColor_Text;
	}

	FCGridRowStyle::~FCGridRowStyle(){
		if(m_font){
			delete m_font;
			m_font = 0;
		}
	}

	Long FCGridRowStyle::getBackColor(){
		return m_backColor;
	}

	void FCGridRowStyle::setBackColor(Long backColor){
		m_backColor = backColor;
	}

	FCFont* FCGridRowStyle::getFont(){
		return m_font;
	}

	void FCGridRowStyle::setFont(FCFont *font){
		if(font){
			if(!m_font){
				m_font = new FCFont();
			}
			m_font->copy(font);
		}
		else{
			if(m_font){
				delete m_font;
				m_font = 0;
			}
		}
	}

	Long FCGridRowStyle::getHoveredBackColor(){
		return m_hoveredBackColor;
	}

	void FCGridRowStyle::setHoveredBackColor(Long hoveredBackColor){
		m_hoveredBackColor = hoveredBackColor;
	}

	Long FCGridRowStyle::getHoveredTextColor(){
		return m_hoveredTextColor;
	}

	void FCGridRowStyle::setHoveredTextColor(Long hoveredTextColor){
		m_hoveredTextColor = hoveredTextColor;
	}

	Long FCGridRowStyle::getSelectedBackColor(){
		return m_selectedBackColor;
	}

	void FCGridRowStyle::setSelectedBackColor(Long selectedBackColor){
		m_selectedBackColor = selectedBackColor;
	}

	Long FCGridRowStyle::getSelectedTextColor(){
		return m_selectedTextColor;
	}

	void FCGridRowStyle::setSelectedTextColor(Long selectedTextColor){
		m_selectedTextColor = selectedTextColor;
	}

	Long FCGridRowStyle::getTextColor(){
		return m_textColor;
	}

	void FCGridRowStyle::setTextColor(Long textColor){
		m_textColor = textColor;
	}

	void FCGridRowStyle::copy(FCGridRowStyle *style){
		setBackColor(style->getBackColor());
		setFont(style->getFont());
		setTextColor(style->getTextColor());
		setHoveredBackColor(style->getHoveredBackColor());
		setHoveredTextColor(style->getHoveredTextColor());
		setSelectedBackColor(style->getSelectedBackColor());
		setSelectedTextColor(style->getSelectedTextColor());
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	FCGridRow::FCGridRow(){
		m_allowEdit = false;
		m_bounds.left = 0;
		m_bounds.top = 0;
		m_bounds.right = 0;
		m_bounds.bottom = 0;
		m_editButton = 0;
		m_editState = 0;
		m_grid = 0;
		m_height = 20;
		m_horizontalOffset = 0;
		m_index = -1;
		m_tag = 0;
		m_visible = true;
		m_visibleIndex = -1;
	}

	FCGridRow::~FCGridRow(){
		for(int i = 0; i < m_cells.size(); i++){
			delete m_cells.get(i);
		}
		m_cells.clear();
		m_editButton = 0;
		m_grid = 0;
		m_tag = 0;
	}

	bool FCGridRow::allowEdit(){
		return m_allowEdit;
	}

	void FCGridRow::setAllowEdit(bool allowEdit){
		m_allowEdit = allowEdit;
	}

	FCRect FCGridRow::getBounds(){
		return m_bounds;
	}

	void FCGridRow::setBounds(FCRect bounds){
		m_bounds = bounds;
	}

	FCView* FCGridRow::getEditButton(){
		return m_editButton;
	}

	void FCGridRow::setEditButton(FCView *editButton){
		m_editButton = editButton;
	}

	FCGrid* FCGridRow::getGrid(){
		return m_grid;
	}

	void FCGridRow::setGrid(FCGrid *grid){
		m_grid = grid;
	}

	int FCGridRow::getHeight(){
		return m_height;
	}

	void FCGridRow::setHeight(int height){
		m_height = height;
	}

	int FCGridRow::getHorizontalOffset(){
		return m_horizontalOffset;
	}

	void FCGridRow::setHorizontalOffset(int horizontalOffset){
		m_horizontalOffset = horizontalOffset;
	}

	int FCGridRow::getIndex(){
		return m_index;
	}

	void FCGridRow::setIndex(int index){
		m_index = index;
	}

	Object FCGridRow::getTag(){
		return m_tag;
	}

	void FCGridRow::setTag(Object tag){
		m_tag = tag;
	}

	bool FCGridRow::isVisible(){
		return m_visible;
	}

	void FCGridRow::setVisible(bool visible){
		m_visible = visible;
	}

	int FCGridRow::getVisibleIndex(){
		return m_visibleIndex;
	}

	void FCGridRow::setVisibleIndex(int visibleIndex){
		m_visibleIndex = visibleIndex;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void FCGridRow::addCell(FCGridColumn *column, FCGridCell *cell){
		cell->setGrid(m_grid);
		cell->setColumn(column);
		cell->setRow(this);
		m_cells.add(cell);
		cell->onAdd();
	}

	void FCGridRow::addCell(const String& columnName, FCGridCell *cell){
		cell->setGrid(m_grid);
		cell->setColumn(m_grid->getColumn(columnName));
		cell->setRow(this);
		m_cells.add(cell);
		cell->onAdd();
	}


	void FCGridRow::addCell(int columnIndex, FCGridCell *cell){
		cell->setGrid(m_grid);
		cell->setColumn(m_grid->getColumn(columnIndex));
		cell->setRow(this);
		m_cells.add(cell);
		cell->onAdd();
	}

	void FCGridRow::clearCells(){
		for(int i = 0; i < m_cells.size(); i++){
			m_cells.get(i)->onRemove();
			delete m_cells.get(i);
		}
		m_cells.clear();
	}

	FCGridCell* FCGridRow::getCell(FCGridColumn *column){
		for(int i = 0; i < m_cells.size(); i++){
			FCGridCell *cell = m_cells.get(i);
			if(cell->getColumn() == column){
				return cell;
			}
		}
		return 0;
	}

	FCGridCell* FCGridRow::getCell(int columnIndex){
		int cellsSize = (int)m_cells.size();
		if(cellsSize > 0){
			if(columnIndex >= 0 && columnIndex < cellsSize){
				if(m_cells.get(columnIndex)->getColumn()->getIndex() == columnIndex){
					return m_cells.get(columnIndex);
				}
			}
			for(int i = 0; i < m_cells.size(); i++){
				FCGridCell *cell = m_cells.get(i);
				if(cell->getColumn()->getIndex() == columnIndex){
					return cell;
				}
			}
		}
		return 0;
	}

	FCGridCell* FCGridRow::getCell(const String& columnName){
		for(int i = 0; i < m_cells.size(); i++){
			FCGridCell *cell = m_cells.get(i);
			if(cell->getColumn()->getName() == columnName){
				return cell;
			}
		}
		return 0;
	}

	ArrayList<FCGridCell*> FCGridRow::getCells(){
		return m_cells;
	}

	void FCGridRow::getProperty(const String& name, String *value, String *type){
		if (name == L"allowedit"){
            *type = L"bool";
			*value = FCStr::convertBoolToStr(allowEdit());
        }
		else if (name == L"height"){
			*type = L"int";
            *value = FCStr::convertIntToStr(getHeight());
        }
        else if (name == L"visible"){
			*type = L"int";
            *value = FCStr::convertBoolToStr(isVisible());

        }
        else{
			*type = L"undefined";
            *value = L"";
        }
	}

	ArrayList<String> FCGridRow::getPropertyNames(){
		ArrayList<String> propertyNames;
		propertyNames.add(L"AllowEdit");
		propertyNames.add(L"Height");
		propertyNames.add(L"Visible");
		return propertyNames;
	}

	void FCGridRow::onAdd(){
		for(int i = 0; i < m_cells.size(); i++){
			m_cells.get(i)->onAdd();
		}
	}

	void FCGridRow::onPaint(FCPaint *paint, const FCRect& clipRect, bool isAlternate){
	}

	void FCGridRow::onPaintBorder(FCPaint *paint, const FCRect& clipRect, bool isAlternate){
	}

	void FCGridRow::onRemove(){
		for(int i = 0; i < m_cells.size(); i++){
			m_cells.get(i)->onRemove();
		}
	}

	void FCGridRow::removeCell(FCGridColumn *column){
		for(int i = 0; i < m_cells.size(); i++){
			FCGridCell *cell = m_cells.get(i);
			if(cell->getColumn() == column){
				m_cells.removeAt(i);
				cell->onRemove();
				break;
			}
		}
	}

	void FCGridRow::removeCell(int columnIndex){
		int cellSize = (int)m_cells.size();
		if(columnIndex >= 0 && columnIndex < cellSize){
			FCGridCell *cell = m_cells.get(columnIndex);
			if(cell->getColumn()->getIndex() == columnIndex){
				m_cells.removeAt(columnIndex);
				return;
			}
			for(int i = 0; i < m_cells.size(); i++){
				cell = m_cells.get(i);
				if(cell->getColumn()->getIndex() == columnIndex){
					m_cells.removeAt(i);
					cell->onRemove();
					break;
				}
			}
		}
	}

	void FCGridRow::removeCell(const String& columnName){
		for(int i = 0; i < m_cells.size(); i++){
			FCGridCell *cell = m_cells.get(i);
			if(cell->getColumn()->getName() == columnName){
				m_cells.removeAt(i);
				cell->onRemove();
				break;
			}
		}
	}

	void FCGridRow::setProperty(const String& name, const String& value){
		if (name == L"allowedit"){
			setAllowEdit(FCStr::convertStrToBool(value));
        }
		else if (name == L"height"){
            setHeight(FCStr::convertStrToInt(value));
        }
        else if (name == L"visible"){
            setVisible(FCStr::convertStrToBool(value));
        }
	}
}