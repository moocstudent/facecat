#include "..\\..\\stdafx.h"
#include "..\\..\\include\\grid\\FCGridColumn.h"

namespace FaceCat{
	FCGridColumn::FCGridColumn(){
		m_allowResize = false;
		m_allowSort = true;
		m_beginWidth = 0;
		m_cellAlign = FCHorizontalAlign_Left;
		m_frozen = false;
		m_grid = 0;
		m_headerRect.left = 0;
		m_headerRect.top = 0;
		m_headerRect.right = 0;
		m_headerRect.bottom = 0;
		m_index = -1;
		m_touchDownPoint.x = 0;
		m_touchDownPoint.y = 0;
		m_resizeState = 0;
		m_sortMode = FCGridColumnSortMode_None;
		setWidth(100);
	}

	FCGridColumn::FCGridColumn(const String& text){
		m_allowResize = false;
		m_allowSort = true;
		m_cellAlign = FCHorizontalAlign_Left;
		m_frozen = false;
		m_beginWidth = 0;
		m_grid = 0;
		m_headerRect.left = 0;
		m_headerRect.top = 0;
		m_headerRect.right = 0;
		m_headerRect.bottom = 0;
		m_index = -1;
		m_touchDownPoint.x = 0;
		m_touchDownPoint.y = 0;
		m_resizeState = 0;
		m_sortMode = FCGridColumnSortMode_None;
		setWidth(100);
		setText(text);
	}

	FCGridColumn::~FCGridColumn(){
		m_grid = 0;
	}

	bool FCGridColumn::allowResize(){
		return m_allowResize;
	}

	void FCGridColumn::setAllowResize(bool allowResize){
		m_allowResize = allowResize;
	}

	bool FCGridColumn::allowSort(){
		return m_allowSort;
	}

	void FCGridColumn::setAllowSort(bool allowSort){
		m_allowSort = allowSort;
	}

	FCHorizontalAlign FCGridColumn::getCellAlign(){
		return m_cellAlign;
	}

	void FCGridColumn::setCellAlign(FCHorizontalAlign cellAlign){
		m_cellAlign = cellAlign;
	}

	String FCGridColumn::getColumnType(){
		return m_columnType;
	}

	void FCGridColumn::setColumnType(String columnType){
		m_columnType = columnType;
	}

	bool FCGridColumn::isFrozen(){
		return m_frozen;
	}

	void FCGridColumn::setFrozen(bool frozen){
		m_frozen = frozen;
	}

	FCGrid* FCGridColumn::getGrid(){
		return m_grid;
	}

	void FCGridColumn::setGrid(FCGrid *grid){
		m_grid = grid;
	}

	FCRect FCGridColumn::getHeaderRect(){
		return m_headerRect;
	}

	void FCGridColumn::setHeaderRect(FCRect headerRect){
		m_headerRect = headerRect;
	}

	int FCGridColumn::getIndex(){
		return m_index;
	}

	void FCGridColumn::setIndex(int index){
		m_index = index;
	}

	FCGridColumnSortMode FCGridColumn::getSortMode(){
		return m_sortMode;
	}

	void FCGridColumn::setSortMode(FCGridColumnSortMode sortMode){
		m_sortMode = sortMode;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////

	String FCGridColumn::getControlType(){
		return L"FCGridColumn";
	}

	void FCGridColumn::getProperty(const String& name, String *value, String *type){
		if(name == L"allowresize"){
			*type = L"bool";
			*value = FCStr::convertBoolToStr(allowResize());
		}
		else if(name == L"allowsort"){
			*type = L"bool";
			*value = FCStr::convertBoolToStr(allowSort());
		}
		else if (name == L"cellalign"){
            *type = L"enum:FCHorizontalAlign";
			*value = FCStr::convertHorizontalAlignToStr(getCellAlign());
        }
		else if(name == L"columntype"){
			*type = L"text";
			*value = getColumnType();
		}
		else if(name == L"frozen"){
			*type = L"bool";
			*value = FCStr::convertBoolToStr(isFrozen());
		}

		else{
			FCButton::getProperty(name, value, type);
		}
	}

	ArrayList<String> FCGridColumn::getPropertyNames(){
		ArrayList<String> propertyNames = FCButton::getPropertyNames();
		propertyNames.add(L"AllowResize");
		propertyNames.add(L"AllowSort");
		propertyNames.add(L"CellAlign");
		propertyNames.add(L"ColumnType");
		propertyNames.add(L"Frozen");
		return propertyNames;
	}

	void FCGridColumn::onClick(FCTouchInfo touchInfo){
		FCButton::onClick(touchInfo);
		if (m_resizeState == 0){
			switch (m_sortMode){
				case FCGridColumnSortMode_None:
				case FCGridColumnSortMode_Desc:
					m_grid->sortColumn(m_grid, this, FCGridColumnSortMode_Asc);
					break;
				case FCGridColumnSortMode_Asc:
					m_grid->sortColumn(m_grid, this, FCGridColumnSortMode_Desc);
					break;
			}
		}
	}

	bool FCGridColumn::onDragBegin(){
		return m_resizeState == 0;
	}

	void FCGridColumn::onDragging(){
		FCView::onDragging();
        if (m_grid){
			ArrayList<FCGridColumn*> columns = m_grid->m_columns;
            int count = (int)columns.size();
            for (int i = 0; i < count; i++){
                FCGridColumn *column = columns.get(i);
                if (column == this){
                    FCGridColumn *lastColumn = 0;
                    FCGridColumn *nextColumn = 0;
                    int lastIndex = i - 1;
					int nextIndex = i + 1;
					while(lastIndex >= 0){
						FCGridColumn *thatColumn = columns.get(lastIndex);
						if(thatColumn->isVisible()){
							lastColumn = thatColumn;
							break;
						}
						else{
							lastIndex--;
						}
					}
					while(nextIndex < count){
						FCGridColumn *thatColumn = columns.get(nextIndex);
						if(thatColumn->isVisible()){
							nextColumn = thatColumn;
							break;
						}
						else{
							nextIndex++;
						}
					}
					FCNative *native = getNative();
                    int clientX = native->clientX(this);
                    if (lastColumn){
                        int lastClientX = native->clientX(lastColumn);
                        if (clientX < lastClientX + lastColumn->getWidth() / 2){
                            m_grid->m_columns.set(lastIndex, this);
                            m_grid->m_columns.set(i, lastColumn);
                            m_grid->update();
                            break;
                        }
                    }
                    if (nextColumn){
                        int nextClientX = native->clientX(nextColumn);
                        if (clientX + column->getWidth() > nextClientX + nextColumn->getWidth() / 2){
                            m_grid->m_columns.set(nextIndex, this);
                            m_grid->m_columns.set(i, nextColumn);
                            m_grid->update();
                            break;
                        }
                    }
                    break;
                }
            }
        }
	}

	void FCGridColumn::onTouchDown(FCTouchInfo touchInfo){
		FCButton::onTouchDown(touchInfo);
		FCPoint mp = touchInfo.m_firstPoint;
		if (touchInfo.m_firstTouch && touchInfo.m_clicks == 1){
            if (m_allowResize){
                if (m_index > 0 && mp.x < 5){
                    m_resizeState = 1;
                    m_beginWidth = getGrid()->getColumn(m_index - 1)->getWidth();
                }
                else if (mp.x > getWidth() - 5){
                    m_resizeState = 2;
                    m_beginWidth = getWidth();
                }
                m_touchDownPoint = getNative()->getTouchPoint();
            }
        }
	}

	void FCGridColumn::onTouchMove(FCTouchInfo touchInfo){
		FCButton::onTouchMove(touchInfo);
		FCPoint mp = touchInfo.m_firstPoint;
		if (m_allowResize){
			if (m_resizeState > 0){
				FCPoint curPoint = getNative()->getTouchPoint();
				int newWidth = m_beginWidth + (curPoint.x - m_touchDownPoint.x);
				if (newWidth > 0){
					if (m_resizeState == 1){
						getGrid()->getColumn(m_index - 1)->setWidth(newWidth);
					}
					else if (m_resizeState == 2){
						setWidth(newWidth);
					}
				}
				if (m_grid){
					m_grid->update();
					m_grid->invalidate();
				}
			}
			else{
				if ((m_index > 0 && mp.x < 5) || mp.x > getWidth() - 5){
					setCursor(FCCursors_SizeWE);
				}
				else{
					setCursor(FCCursors_Arrow);
				}
			}
			if(isDragging()){
				setCursor(FCCursors_Arrow);
			}
		}
	}

	void FCGridColumn::onTouchUp(FCTouchInfo touchInfo){
		FCButton::onTouchUp(touchInfo);
		setCursor(FCCursors_Arrow);
        m_resizeState = 0;
		if(m_grid){
			m_grid->invalidate();
		}
	}

	void FCGridColumn::onPaintForeground(FCPaint *paint, const FCRect& clipRect){	
		FCButton::onPaintForeground(paint, clipRect);
		if(m_grid && getNative()){
			FCRect rect = {0, 0, getWidth(), getHeight()};
			int tLeft = rect.right - 15;
			int midTop = rect.top + (rect.bottom - rect.top) / 2;
			Long textColor = getPaintingTextColor();
			if (m_sortMode == FCGridColumnSortMode_Asc){
				FCPoint *points = new FCPoint[3];
				FCPoint point1 = {tLeft + 5, midTop - 5};
				FCPoint point2 = {tLeft, midTop + 5};
				FCPoint point3 = {tLeft + 10, midTop + 5};
				points[0] = point1;
				points[1] = point2;
				points[2] = point3;
				paint->fillPolygon(textColor, points, 3);
				if(points){
					delete[] points;
					points = 0;
				}
			}
			else if (m_sortMode == FCGridColumnSortMode_Desc){
				FCPoint *points = new FCPoint[3];
				FCPoint point1 = {tLeft + 5, midTop + 5};
				FCPoint point2 = {tLeft, midTop - 5};
				FCPoint point3 = {tLeft + 10, midTop - 5};
				points[0] = point1;
				points[1] = point2;
				points[2] = point3;
				paint->fillPolygon(textColor, points, 3);
				if(points){
					delete[] points;
					points = 0;
				}
			}
		}
	}

	void FCGridColumn::setProperty(const String& name, const String& value){
		if(name == L"allowresize"){
			setAllowResize(FCStr::convertStrToBool(value));
		}
		else if(name == L"allowsort"){
			setAllowSort(FCStr::convertStrToBool(value));
		}
		else if (name == L"cellalign"){
			setCellAlign(FCStr::convertStrToHorizontalAlign(value));
        }
		else if(name == L"columntype"){
			setColumnType(value);
		}
		else if(name == L"frozen"){
			setFrozen(FCStr::convertStrToBool(value));
		}
		else{
			FCButton::setProperty(name, value);
		}
	}
}
