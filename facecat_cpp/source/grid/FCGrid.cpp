#include "..\\..\\stdafx.h"
#include "..\\..\\include\\grid\\FCGrid.h"

namespace FaceCat{
	GridRowCompare::GridRowCompare(){
		m_columnIndex = 0;
		m_type = 0;
	}

	GridRowCompare::~GridRowCompare(){
	}

	bool GridRowCompare::operator()(FCGridRow *x, FCGridRow *y){
		FCGridCell *cellLeft = x->getCell(m_columnIndex);
        FCGridCell *cellRight = y->getCell(m_columnIndex);
        if (m_type == 0){
			return cellRight->compareTo(cellLeft) > 0 ? true : false;
        }
        else{
			return cellLeft->compareTo(cellRight) > 0 ? true : false;
        }
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	FCGridSort::FCGridSort(){
	}

	FCGridSort::~FCGridSort(){
	}

	void FCGridSort::sortColumn(FCGrid *grid, FCGridColumn *column, FCGridColumnSortMode sortMode){
		GridRowCompare compare;
		compare.m_columnIndex = column->getIndex();
        if (sortMode == FCGridColumnSortMode_Asc){
            compare.m_type = 0;
        }
        else{
            compare.m_type = 1;
        }
        vector<FCGridRow*> rows;
        for(int i = 0; i < grid->m_rows.size(); i++){
            rows.push_back(grid->m_rows.get(i));
        }
        sort(rows.begin(), rows.end(), compare);
        grid->m_rows.clear();
        vector<FCGridRow*>::iterator sIter = rows.begin();
        for(; sIter != rows.end(); ++sIter){
            grid->m_rows.add(*sIter);
        }
        rows.clear();
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void FCGrid::callCellEvents(int eventID, FCGridCell *cell){
		if(m_events.size() > 0){
			map<int, vector<Object>*>::iterator sIter = m_events.find(eventID);
			if(sIter != m_events.end()){
				map<int, vector<Object>*>::iterator sIter2 = m_invokes.find(eventID);
				vector<Object> *events = sIter->second;
				vector<Object> *invokes = sIter2->second;
				int eventSize = (int)events->size();
				for(int i = 0; i < eventSize; i++){
					FCGridCellEvent func = (FCGridCellEvent)(*events)[i];
					func(this, cell, (*invokes)[i]);
				}
			}
		}
	}

	void FCGrid::callCellTouchEvents(int eventID, FCGridCell *cell, FCTouchInfo touchInfo){
		if(m_events.size() > 0){
			map<int, vector<Object>*>::iterator sIter = m_events.find(eventID);
			if(sIter != m_events.end()){
				map<int, vector<Object>*>::iterator sIter2 = m_invokes.find(eventID);
				vector<Object> *events = sIter->second;
				vector<Object> *invokes = sIter2->second;
				int eventSize = (int)events->size();
				for(int i = 0; i < eventSize; i++){
					FCGridCellTouchEvent func = (FCGridCellTouchEvent)(*events)[i];
					func(this, cell, touchInfo, (*invokes)[i]);
				}
			}
		}
	}

	void FCGrid::touchEvent(FCTouchInfo touchInfo, int state){
		FCPoint mp = touchInfo.m_firstPoint;
		int height = getHeight();
		int hHeight = m_headerVisible ? m_headerHeight : 0;
		int scrollH = 0, scrollV = 0;
		FCHScrollBar *hScrollBar = getHScrollBar();
		FCVScrollBar *vScrollBar = getVScrollBar();
		FCHost *host = getNative()->getHost();
		if (hScrollBar && hScrollBar->isVisible()){
			scrollH = - hScrollBar->getPos();
		}
		if (vScrollBar && vScrollBar->isVisible()){
			scrollV = - vScrollBar->getPos();
		}
		FCPoint fPoint = {0, hHeight + 1 - scrollV};
		FCPoint ePoint = {0, height - 10 - scrollV};
		FCGridRow *fRow = getRow(fPoint);
		FCGridRow *eRow = getRow(ePoint);
		while (!eRow && ePoint.y > 0){
			ePoint.y -= 10;
			eRow = getRow(ePoint);
		}
		if (fRow && eRow){
			int fIndex = fRow->getIndex();
            int eIndex = eRow->getIndex();
            for (int i = fIndex; i <= eIndex; i++){
				FCGridRow *row = m_rows.get(i);
				if (row->isVisible()){
					FCRect rowRect = row->getBounds();
					rowRect.top += scrollV;
                    rowRect.bottom += scrollV;
					ArrayList<FCGridCell*> cells;
					ArrayList<FCGridCell*> unFrozenCells;
					for (int j = 0; j < 2; j++){
						if (j == 0){
							cells = row->m_cells;
						}
						else{
							cells = unFrozenCells;
						}
						int cellSize = (int)cells.size();
						for (int c = 0; c < cellSize; c++){
							FCGridCell *cell = cells.get(c);
							FCGridColumn *column = cell->getColumn();
							if (column->isVisible()){
								if (j == 0 && !column->isFrozen()){
									unFrozenCells.add(cell);
									continue;
								}
								FCRect headerRect = column->getHeaderRect();
								if(!column->isFrozen()){
									headerRect.left += scrollH;
									headerRect.right += scrollH;
								}
								int cellWidth = column->getWidth();
								int colSpan = cell->getColSpan();
								if (colSpan > 1){
									for (int n = 1; n < colSpan; n++){
										FCGridColumn *spanColumn = getColumn(column->getIndex() + n);
										if(spanColumn && spanColumn->isVisible()){
											cellWidth += spanColumn->getWidth();
										}
									}
								}
								int cellHeight = row->getHeight();
								int rowSpan = cell->getRowSpan();
								if (rowSpan > 1){
									for (int n = 1; n < rowSpan; n++){
										FCGridRow *spanRow = getRow(i + n);
										if (spanRow && spanRow->isVisible()){
											cellHeight += spanRow->getHeight();
										}
									}
								}
								FCRect cellRect = {headerRect.left, rowRect.top + m_verticalOffset,
									headerRect.left + cellWidth, rowRect.top + m_verticalOffset + cellHeight};
								if (mp.x >= cellRect.left && mp.x <= cellRect.right 
								&& mp.y >= cellRect.top && mp.y <= cellRect.bottom){
									if(state == 0){
										bool hoverChanged = false;
										if (m_allowHoveredRow && m_hoveredRow != row){
											m_hoveredRow = row;
											hoverChanged = true;
										}
										if (getNative()->getPushedControl() == this){
                                            if (m_allowDragRow){
                                                if (m_selectionMode == FCGridSelectionMode_SelectFullRow){
                                                    int selectedRowsSize = (int)m_selectedRows.size();
                                                    if (selectedRowsSize == 1){
                                                        if (m_selectedRows.get(0) != row){
                                                            moveRow(m_selectedRows.get(0)->getIndex(), row->getIndex());
                                                            hoverChanged = true;
                                                        }
                                                    }
                                                }
                                            }
                                        }
										if (m_hoveredCell != cell){
                                            if (m_hoveredCell){
                                                onCellTouchLeave(m_hoveredCell, touchInfo);
                                            }
                                            m_hoveredCell = cell;
                                            onCellTouchEnter(m_hoveredCell, touchInfo);
                                        }
										onCellTouchMove(cell, touchInfo);
                                        if (!m_editingRow){
                                            if (row->allowEdit()){
                                                if (getNative()->getPushedControl() == this){
                                                    int selectedRowsSize = (int)m_selectedRows.size();
                                                    if (selectedRowsSize == 1){
                                                        if (m_selectedRows.get(0) == row){
                                                            onRowEditBegin(row);
                                                            hoverChanged = true;
                                                        }
                                                    }
                                                }
                                            }
                                        }
										if (hoverChanged){
											invalidate();
										}
									}
									else{
										if(state == 1){
											m_touchDownPoint = mp;
											onCellTouchDown(cell, touchInfo);
											if(touchInfo.m_firstTouch && touchInfo.m_clicks == 1){
												int multiSelectMode = 0;
												if (m_multiSelect){
													if (host->isKeyPress(VK_SHIFT)){
														multiSelectMode = 1;
													}
													else if (host->isKeyPress(VK_CONTROL)){
														multiSelectMode = 2;
													}
												}
												if (m_selectionMode == FCGridSelectionMode_SelectCell){
													bool contains = false;
													bool selectedChanged = false;
													int selectedCellSize = (int)m_selectedCells.size();
													if (multiSelectMode == 0 || multiSelectMode == 2){
														for(int c = 0; c < m_selectedCells.size(); c++){
															if(m_selectedCells.get(c) == cell){
																contains = true;
																if (multiSelectMode == 2){
																	m_selectedCells.removeAt(c);
																	selectedChanged = true;
																}
																break;
															}
														}
													}
													if (multiSelectMode == 0){
														selectedCellSize = (int)m_selectedCells.size();
														if (!contains || selectedCellSize > 1){
															m_selectedCells.clear();
															m_selectedCells.add(cell);
															selectedChanged = true;
														}
													}
													else if (multiSelectMode == 2){
														if (!contains){
															m_selectedCells.add(cell);
															selectedChanged = true;
														}
													}
													if (selectedChanged){
														onSelectedCellsChanged();
													}
												}
												else if (m_selectionMode == FCGridSelectionMode_SelectFullColumn){
													bool contains = false;
													bool selectedChanged = false;
													int selectedColumnsSize = (int)m_selectedColumns.size();
													if (multiSelectMode == 0 || multiSelectMode == 2){
														for(int c = 0; c < m_selectedColumns.size(); c++){
															if(m_selectedColumns.get(c) == column){
																contains = true;
																if (multiSelectMode == 2){
																	m_selectedColumns.removeAt(c);
																	selectedChanged = true;
																}
																break;
															}
														}
													}
													if (multiSelectMode == 0){
														if (!contains || selectedColumnsSize > 1){
															m_selectedColumns.clear();
															m_selectedColumns.add(column);
															selectedChanged = true;
														}
													}
													else if (multiSelectMode == 2){
														if (!contains){
															m_selectedColumns.add(column);
															selectedChanged = true;
														}
													}
													m_selectedCells.clear();
													m_selectedCells.add(cell);
													if (selectedChanged){
														onSelectedColumnsChanged();
													}
												}
												else if (m_selectionMode == FCGridSelectionMode_SelectFullRow){
													bool contains = false;
													bool selectedChanged = false;
													int selectedRowsSize = (int)m_selectedRows.size();
													if (multiSelectMode == 0 || multiSelectMode == 2){
														for(int g = 0; g < m_selectedRows.size(); g++){
															if(m_selectedRows.get(g) == row){
																contains = true;
																if (multiSelectMode == 2){
																	m_selectedRows.removeAt(g);
																	selectedChanged = true;
																}
																break;
															}
														}
													}
													if (multiSelectMode == 0){
														selectedRowsSize = (int)m_selectedRows.size();
														if (!contains || selectedRowsSize > 1){
															m_selectedRows.clear();
															m_selectedRows.add(row);
															selectedChanged = true;
														}
													}
													else if (multiSelectMode == 1){
														selectedRowsSize = (int)m_selectedRows.size();
														if (selectedRowsSize > 0){
															int firstIndex = m_selectedRows.get(0)->getIndex();
															int newIndex = row->getIndex();
															int minIndex = min(firstIndex, newIndex);
															int maxIndex = max(firstIndex, newIndex);
															m_selectedRows.clear();
															for (int s = minIndex; s <= maxIndex; s++){
																m_selectedRows.add(getRow(s));
															}
														}
														else{
															m_selectedRows.add(row);
														}
													}
													else if (multiSelectMode == 2){
														if (!contains){
															m_selectedRows.add(row);
															selectedChanged = true;
														}
													}
													m_selectedCells.clear();
													m_selectedCells.add(cell);
													if (selectedChanged){
														onSelectedRowsChanged();
													}
												}
											}
										}
										else if(state == 2){
											onCellTouchUp(cell, touchInfo);
										}
										if (state == 2 || (touchInfo.m_clicks == 2 && state == 1)){	
											 if ((int)m_selectedCells.size() > 0 && m_selectedCells.get(0) == cell){
													onCellClick(cell, touchInfo);
													if (touchInfo.m_firstTouch && cell->allowEdit()){
														if ((m_cellEditMode == FCGridCellEditMode_DoubleClick && (touchInfo.m_clicks == 2 && state == 1))
														|| (m_cellEditMode == FCGridCellEditMode_SingleClick && state == 2)){
															onCellEditBegin(cell);
														}
													}
											 }
										}
										invalidate();
									}
									unFrozenCells.clear();
									if (state == 1 && m_editingRow){
                                        onRowEditEnd();
                                    }
									return;
								}
							}
						}
					}
					unFrozenCells.clear();
				}
			}
		}
	    if (state == 1 && m_editingRow){
            onRowEditEnd();
        }
	}

	void FCGrid::editTextBoxLostFocus(Object sender, Object pInvoke){
		FCGrid *grid = (FCGrid*)pInvoke;
		if(grid){
			FCTextBox *textBox = grid->getEditTextBox();
			if (textBox && textBox->getTag()){
				FCGridCell *cell = (FCGridCell*)textBox->getTag();
				grid->onCellEditEnd(cell);
			}
		}
	}

	void FCGrid::editTextBoxKeyDown(Object sender, char key, Object pInvoke){
		FCGrid *grid = (FCGrid*)pInvoke;
		if(grid){
			if(key == 13){
				FCTextBox *editTextBox = grid->getEditTextBox();
				if(editTextBox && !editTextBox->isMultiline()){
					editTextBox->setFocused(false);
				}
			}
			else if(key == 27){
				if(grid){
					grid->onCellEditEnd(0);
				}
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	int FCGrid::getAllVisibleColumnsWidth(){
		int allVisibleColumnsWidth = 0;
		int colSize = (int)m_columns.size();
		for (int i = 0; i < colSize; i++){
			FCGridColumn *column = m_columns.get(i);
			if (column->isVisible()){
				allVisibleColumnsWidth += column->getWidth();
			}
		}
		return allVisibleColumnsWidth;
	}

	int FCGrid::getAllVisibleRowsHeight(){
		int allVisibleRowsHeight = 0;
		int rowSize = (int)m_rows.size();
		for (int i = 0; i < rowSize; i++){
			if (m_rows.get(i)->isVisible()){
				allVisibleRowsHeight += m_rows.get(i)->getHeight();
			}
		}
		return allVisibleRowsHeight;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	FCGrid::FCGrid(){
		m_allowDragRow = false;
		m_allowHoveredRow = true;
		m_alternateRowStyle = 0;
		m_cellEditMode = FCGridCellEditMode_SingleClick;
		m_editingCell = 0;
		m_editingRow = 0;
		m_editTextBoxLostFocusEvent = editTextBoxLostFocus;
		m_editTextBoxKeyDownEvent = editTextBoxKeyDown;
		m_editTextBox = 0;
		m_gridLineColor = FCColor::argb(100, 100, 100);
		m_hasUnVisibleRow = false;
		m_headerVisible = true;
		m_headerHeight = 20;
		m_horizontalOffset = 0;
		m_hoveredCell = 0;
		m_hoveredRow = 0;
		m_lockUpdate = false;
		m_touchDownPoint.x = 0;
		m_touchDownPoint.y = 0;
		m_multiSelect = false;
		m_rowStyle = new FCGridRowStyle;
		m_selectionMode = FCGridSelectionMode_SelectFullRow;
		m_sort = new FCGridSort;
		m_timerID = getNewTimerID();
		m_useAnimation = false;
		m_verticalOffset = 0;
		setShowHScrollBar(true);
        setShowVScrollBar(true);
	}

	FCGrid::~FCGrid(){
		stopTimer(m_timerID);
		m_animateAddRows.clear();
		m_animateRemoveRows.clear();
		if(m_alternateRowStyle){
			delete m_alternateRowStyle;
			m_alternateRowStyle = 0;
		}
		if(m_rowStyle){
			delete m_rowStyle;
			m_rowStyle = 0;
		}
		m_editingCell = 0;
		m_editingRow = 0;
		if(m_editTextBox){
			if (m_editTextBoxLostFocusEvent){
				m_editTextBox->removeEvent(m_editTextBoxLostFocusEvent, FCEventID::LOSTFOCUS);
				m_editTextBoxLostFocusEvent = 0;
			}
			if(m_editTextBoxKeyDownEvent){
				m_editTextBox->removeEvent(m_editTextBoxKeyDownEvent, FCEventID::KEYDOWN);
				m_editTextBoxKeyDownEvent = 0;
			}
			m_editTextBox = 0;
		}
		m_editTextBox = 0;
		if(m_sort){
			delete m_sort;
			m_sort = 0;
		}
		m_hoveredCell = 0;
		m_hoveredRow = 0;
		clear();
	}

	bool FCGrid::allowDragRow(){
		return m_allowDragRow;
	}

	void FCGrid::setAllowDragRow(bool allowDragRow){
		m_allowDragRow = allowDragRow;
	}

	bool FCGrid::allowHoveredRow(){
		return m_allowHoveredRow;
	}

	void FCGrid::setAllowHoveredRow(bool allowHoveredRow){
		m_allowHoveredRow = allowHoveredRow;
	}

	FCGridRowStyle* FCGrid::getAlternateRowStyle(){
		return m_alternateRowStyle;
	}

	void FCGrid::setAlternateRowStyle(FCGridRowStyle *alternateRowStyle){
		if(alternateRowStyle){
			if(!m_alternateRowStyle){
				m_alternateRowStyle = new FCGridRowStyle;
			}
			m_alternateRowStyle->copy(alternateRowStyle);
		}
		else{
			if(m_alternateRowStyle){
				delete m_alternateRowStyle;
				m_alternateRowStyle = 0;
			}
		}
	}

	FCGridCellEditMode FCGrid::getCellEditMode(){
		return m_cellEditMode;
	}

	void FCGrid::setCellEditMode(FCGridCellEditMode cellEditMode){
		m_cellEditMode = cellEditMode;
	}

	FCTextBox* FCGrid::getEditTextBox(){
		return m_editTextBox;
	}

	Long FCGrid::getGridLineColor(){
		return m_gridLineColor;
	}

	void FCGrid::setGridLineColor(Long gridLineColor){
		m_gridLineColor = gridLineColor;
	}

	bool FCGrid::isHeaderVisible(){
		return m_headerVisible;
	}

	void FCGrid::setHeaderVisible(bool headerVisible){
		m_headerVisible = headerVisible;
	}

	int FCGrid::getHeaderHeight(){
		return m_headerHeight;
	}

	void FCGrid::setHeaderHeight(int headerHeight){
		m_headerHeight = headerHeight;
	}

	int FCGrid::getHorizontalOffset(){
		return m_horizontalOffset;
	}

	void FCGrid::setHorizontalOffset(int horizontalOffset){
		m_horizontalOffset = horizontalOffset;
	}

	FCGridCell* FCGrid::getHoveredCell(){
		return m_hoveredCell;
	}

	FCGridRow* FCGrid::getHoveredRow(){
		return m_hoveredRow;
	}

	bool FCGrid::isMultiSelect(){
		return m_multiSelect;
	}

	void FCGrid::setMultiSelect(bool multiSelect){
		m_multiSelect = multiSelect;
	}

	FCGridRowStyle* FCGrid::getRowStyle(){
		return m_rowStyle;
	}

	void FCGrid::setRowStyle(FCGridRowStyle *rowStyle){
		if(rowStyle){
			if(!m_rowStyle){
				m_rowStyle = new FCGridRowStyle;
			}
			m_rowStyle->copy(rowStyle);
		}
		else{
			if(m_rowStyle){
				delete m_rowStyle;
				m_rowStyle = 0;
			}
		}
	}

	ArrayList<FCGridCell*> FCGrid::getSelectedCells(){
		return m_selectedCells;
	}

	void FCGrid::setSelectedCells(ArrayList<FCGridCell*> selectedCells){
        m_selectedCells.clear();
        int selectedCellsSize = (int)selectedCells.size();
        for (int i = 0; i < selectedCellsSize; i++){
			m_selectedCells.add(selectedCells.get(i));
        }
		onSelectedCellsChanged();
	}

	ArrayList<FCGridColumn*> FCGrid::getSelectedColumns(){
		return m_selectedColumns;
	}

	void FCGrid::setSelectedColumns(ArrayList<FCGridColumn*> selectedColumns){
        m_selectedColumns.clear();
        int selectedColumnsSize = (int)selectedColumns.size();
        for (int i = 0; i < selectedColumnsSize; i++){
            m_selectedColumns.add(selectedColumns.get(i));
        }
		onSelectedColumnsChanged();
	}

	ArrayList<FCGridRow*> FCGrid::getSelectedRows(){
		return m_selectedRows;
	}

	void FCGrid::setSelectedRows(ArrayList<FCGridRow*> selectedRows){
        m_selectedRows.clear();
        int selectedRowsSize = (int)selectedRows.size();
        for (int i = 0; i < selectedRowsSize; i++){
            m_selectedRows.add(selectedRows.get(i));
        }
		onSelectedRowsChanged();
	}

	FCGridSelectionMode FCGrid::getSelectionMode(){
		return m_selectionMode;
	}

	void FCGrid::setSelectionMode(FCGridSelectionMode selectionMode){
		m_selectionMode = selectionMode;
	}

	FCGridSort* FCGrid::getSort(){
		return m_sort;
	}

	void FCGrid::setSort(FCGridSort *sort){
		if(m_sort){
			delete m_sort;
		}
		m_sort = sort;
	}

	bool FCGrid::useAnimation(){
		return m_useAnimation;
	}

	void FCGrid::setUseAnimation(bool useAnimation){
		m_useAnimation = useAnimation;
		if(m_useAnimation){
			startTimer(m_timerID, 20);
		}
		else{
			stopTimer(m_timerID);
		}
	}

	int FCGrid::getVerticalOffset(){
		return m_verticalOffset;
	}

	void FCGrid::setVerticalOffset(int verticalOffset){
		m_verticalOffset = verticalOffset;
	}

	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void FCGrid::addColumn(FCGridColumn *column){
		column->setGrid(this);
		m_columns.add(column);
		int columnsSize = (int)m_columns.size();
		for (int i = 0; i < columnsSize; i++){
			m_columns.get(i)->setIndex(i);
		}
		addControl(column);
	}

	void FCGrid::addRow(FCGridRow *row){
		row->setGrid(this);
		m_rows.add(row);
		row->onAdd();
		if (m_selectionMode == FCGridSelectionMode_SelectFullRow){
			int selectedRowsSize = (int)m_selectedRows.size();
			if (selectedRowsSize == 0){
				m_selectedRows.add(row);
				onSelectedRowsChanged();
			}
		}
	}

	void FCGrid::animateAddRow(FCGridRow *row){
		row->setGrid(this);
		m_rows.add(row);
		row->onAdd();
		if (m_selectionMode == FCGridSelectionMode_SelectFullRow){
			int selectedRowsSize = (int)m_selectedRows.size();
			if (selectedRowsSize == 0){
				m_selectedRows.add(row);
				onSelectedRowsChanged();
			}
		}
		if (m_useAnimation){
            m_animateAddRows.add(row);
        }
	}

	void FCGrid::animateRemoveRow(FCGridRow *row){
		if (m_useAnimation){
            m_animateRemoveRows.add(row);
        }
        else{
            removeRow(row);
        }
	}

	void FCGrid::beginUpdate(){
		m_lockUpdate = true;
	}

	void FCGrid::clear(){
		clearRows();
		clearColumns();
	}

	void FCGrid::clearColumns(){
		m_selectedColumns.clear();
		int colSize = (int)m_columns.size();
		for(int i = 0; i < colSize; i++){
			removeControl(m_columns.get(i));
			delete m_columns.get(i);
		}
		m_columns.clear();
	}

	void FCGrid::clearRows(){
		m_hasUnVisibleRow = false;
		m_hoveredCell = 0;
		m_hoveredRow = 0;
		m_selectedRows.clear();
		int rowSize = (int)m_rows.size();
		for (int i = 0; i < rowSize; i++){
			m_rows.get(i)->onRemove();
			delete m_rows.get(i);
		}
		m_rows.clear();
	}

	void FCGrid::endUpdate(){
        if (m_lockUpdate){
            m_lockUpdate = false;
            update();
        }
	}

	FCGridColumn* FCGrid::getColumn(int columnIndex){
		if (columnIndex >= 0 && columnIndex < (int)m_columns.size()){
			return m_columns.get(columnIndex);
		}
		return 0;
	}

	FCGridColumn* FCGrid::getColumn(const String& columnName){
		int colSize = (int)m_columns.size();
		for (int i = 0; i < colSize; i++){
			if (m_columns.get(i)->getName() == columnName){
				return m_columns.get(i);
			}
		}
		return 0;
	}

	ArrayList<FCGridColumn*> FCGrid::getColumns(){
		return m_columns;
	}

	int FCGrid::getContentHeight(){
		int allVisibleRowsHeight = getAllVisibleRowsHeight();
		if(allVisibleRowsHeight > 0){
			if(allVisibleRowsHeight <= getHeight()){
				allVisibleRowsHeight += m_headerVisible ? m_headerHeight : 0;
			}
			return allVisibleRowsHeight;
		}
		else{
			return 0;
		}
	}

	int FCGrid::getContentWidth(){
        return getAllVisibleColumnsWidth();
	}

	String FCGrid::getControlType(){
		return L"Grid";
	}

	FCPoint FCGrid::getDisplayOffset(){
		FCPoint offset = {0};
		return offset;
	}

	void FCGrid::getProperty(const String& name, String *value, String *type){
		if(name == L"allowdragrow"){
			*type = L"bool";
			*value = FCStr::convertBoolToStr(allowDragRow());
		}
		else if (name == L"allowhoveredrow"){
            *type = L"bool";
			*value = FCStr::convertBoolToStr(allowHoveredRow());
        }
	    else if (name == L"celleditmode"){
            *type = L"enum:FCGridCellEditMode";
            FCGridCellEditMode cellEditMode = getCellEditMode();
            if (cellEditMode == FCGridCellEditMode_DoubleClick){
                *value = L"DoubleClick";
            }
            else if (cellEditMode == FCGridCellEditMode_None){
                *value = L"None";
            }
            else{
                *value = L"SingleClick";
            }
        }
		if(name == L"gridlinecolor"){
			*type = L"color";
			*value = FCStr::convertColorToStr(getGridLineColor());
		}
		else if(name == L"headerheight"){
			*type = L"int";
			*value = FCStr::convertIntToStr(getHeaderHeight());
		}
		else if(name == L"headervisible"){
			*type = L"bool";
			*value = FCStr::convertBoolToStr(isHeaderVisible());
		}
		else if(name == L"horizontaloffset"){
			*type = L"int";
			*value = FCStr::convertIntToStr(getHorizontalOffset());
		}
		else if(name == L"multiselect"){
			*type = L"bool";
			*value = FCStr::convertBoolToStr(isMultiSelect());
		}
		else if (name == L"selectionmode"){
			*type = L"enum:FCGridSelectionMode";
            FCGridSelectionMode selectionMode = getSelectionMode();
            if (selectionMode == FCGridSelectionMode_SelectCell){
                *value = L"SelectCell";
            }
            else if (selectionMode == FCGridSelectionMode_SelectFullColumn){
                *value = L"SelectFullColumn";
            }
            else if (selectionMode == FCGridSelectionMode_SelectFullRow){
                *value = L"SelectFullRow";
            }
            else{
                *value = L"none";
            }
        }
		else if(name == L"useanimation"){
			*type = L"bool";
			*value = FCStr::convertBoolToStr(useAnimation());
		}
		else if(name == L"verticaloffset"){
			*type = L"int";
			*value = FCStr::convertIntToStr(getVerticalOffset());
		}
		else{
			FCDiv::getProperty(name, value, type);
		}
	}

	ArrayList<String> FCGrid::getPropertyNames(){
		ArrayList<String> propertyNames = FCDiv::getPropertyNames();
		propertyNames.add(L"AllowDragRow");
		propertyNames.add(L"AllowHoveredRow");
		propertyNames.add(L"CellEditMode");
		propertyNames.add(L"GridLineColor");
		propertyNames.add(L"HeaderHeight");
		propertyNames.add(L"HeaderVisible");
		propertyNames.add(L"HorizontalOffset");
		propertyNames.add(L"MultiSelect");
		propertyNames.add(L"SelectionMode");
		propertyNames.add(L"UseAnimation");
		propertyNames.add(L"VerticalOffset");
		return propertyNames;
	}

	FCGridRow* FCGrid::getRow(const FCPoint& mp){
		if (m_hasUnVisibleRow){
            int rowsSize = (int)m_rows.size();
            for (int i = 0; i < rowsSize; i++){
                FCGridRow *row = m_rows.get(i);
				if (row->isVisible()){
                    FCRect bounds = row->getBounds();
                    if (mp.y >= bounds.top && mp.y <= bounds.bottom){
                        return row;
                    }
                }
            }
        }
		else{
			int begin = 0;
			int end = (int)m_rows.size() - 1;
			int sub = end - begin;
			while (sub >= 0){
				int half = begin + sub / 2;
				FCGridRow *row = m_rows.get(half);
				FCRect bounds = row->getBounds();
				if (half == begin || half == end){
					if (mp.y >= m_rows.get(begin)->getBounds().top && mp.y <= m_rows.get(begin)->getBounds().bottom){
						return m_rows.get(begin);
					}
					if (mp.y >= m_rows.get(end)->getBounds().top && mp.y <= m_rows.get(end)->getBounds().bottom){
						return m_rows.get(end);
					}
					break;
				}
				if (mp.y >= bounds.top && mp.y <= bounds.bottom){
					return row;
				}
				else if (bounds.top > mp.y){
					end = half;
				}
				else if (bounds.bottom < mp.y){
					begin = half;
				}
				sub = end - begin;
			}
		}
        return 0;
	}

	FCGridRow* FCGrid::getRow(int rowIndex){
		if (rowIndex >= 0 && rowIndex < (int)m_rows.size()){
			return m_rows.get(rowIndex);
		}
		return 0;
	}

	ArrayList<FCGridRow*> FCGrid::getRows(){
		return m_rows;
	}

	void FCGrid::getVisibleRowsIndex(double visiblePercent, int *firstVisibleRowIndex, int *lastVisibleRowIndex){
	    *firstVisibleRowIndex = -1;
        *lastVisibleRowIndex = -1;
        int rowsSize = (int)m_rows.size();
        if (rowsSize > 0){
            for (int i = 0; i < rowsSize; i++){
                FCGridRow *row = m_rows.get(i);
                if (isRowVisible(row, visiblePercent)){
                    if (*firstVisibleRowIndex == -1){
                *firstVisibleRowIndex = i;
                    }
                }
                else{
                    if (*firstVisibleRowIndex != -1){
                *lastVisibleRowIndex = i;
                        break;
                    }
                }
            }
            if (*firstVisibleRowIndex != -1 && *lastVisibleRowIndex == -1){
                *lastVisibleRowIndex = *firstVisibleRowIndex;
            }
        }
	}

	void FCGrid::insertRow(int index, FCGridRow *row){
		row->setGrid(this);
		m_rows.insert(index, row);
		row->onAdd();
	}

	bool FCGrid::isRowVisible(FCRect *bounds, int rowHeight, int scrollV, double visiblePercent, int cell, int floor){
        int rowtop = bounds->top + scrollV;
        int rowbottom = bounds->bottom + scrollV;
        if (rowtop < cell){
            rowtop = cell;
        }
        else if (rowtop > floor){
            rowtop = floor;
        }
        if (rowbottom < cell){
            rowbottom = cell;
        }
        else if (rowbottom > floor){
            rowbottom = floor;
        }
        if (rowbottom - rowtop > rowHeight * visiblePercent){
            return true;
        }
        return false;
    }

	bool FCGrid::isRowVisible(FCGridRow *row, double visiblePercent){
		int scrollV = 0;
        FCVScrollBar *vScrollBar = getVScrollBar();
        if (vScrollBar && vScrollBar->isVisible()){
            scrollV = -vScrollBar->getPos();
        }
        int cell = m_headerVisible ? m_headerHeight : 0;
        int floor = getHeight() - cell;
		FCRect bounds = row->getBounds();
		return isRowVisible(&bounds, row->getHeight(), scrollV, visiblePercent, cell, floor);
	}

	void FCGrid::moveRow(int oldIndex, int newIndex){
		int rowsSize = (int)m_rows.size();
        if (rowsSize > 0){
            if (oldIndex >= 0 && oldIndex < rowsSize
                && newIndex >= 0 && newIndex < rowsSize){
                FCGridRow *movingRow = m_rows.get(oldIndex);
                FCGridRow *targetRow = m_rows.get(newIndex);
                if (movingRow != targetRow){
                    m_rows.set(newIndex, movingRow);
                    m_rows.set(oldIndex, targetRow);
                    movingRow->setIndex(newIndex);
                    targetRow->setIndex(oldIndex);
                    FCVScrollBar *vScrollBar = getVScrollBar();
                    if (vScrollBar && vScrollBar->isVisible()){
                        int firstVisibleRowIndex = -1, lastVisibleRowIndex = -1;
                        getVisibleRowsIndex(0.6, &firstVisibleRowIndex, &lastVisibleRowIndex);
						int th = targetRow->getHeight();
                        if (newIndex <= firstVisibleRowIndex){
                            if (newIndex == firstVisibleRowIndex){
								vScrollBar->setPos(vScrollBar->getPos() - th);
                            }
							int count = 0;
                            while (!isRowVisible(targetRow, 0.6)){
								int newPos = vScrollBar->getPos() - th;
                                vScrollBar->setPos(newPos);
								count++;
								if(count > rowsSize || newPos <= vScrollBar->getPos()){
									break;
								}
                            }
                        }
                        else if (newIndex >= lastVisibleRowIndex){
                            if (newIndex == lastVisibleRowIndex){
                                vScrollBar->setPos(vScrollBar->getPos() + th);
                            }
							int count = 0;
                            while (!isRowVisible(targetRow, 0.6)){
								int newPos = vScrollBar->getPos() + th;
                                vScrollBar->setPos(newPos);
								count++;
								if(count > rowsSize || newPos >= vScrollBar->getPos()){
									break;
								}
                            }
                        }
                        vScrollBar->update();
                    }
					update();
                }
            }
        }
	}

	void FCGrid::onCellClick(FCGridCell *cell, FCTouchInfo touchInfo){
		callCellTouchEvents(FCEventID::GRIDCELLCLICK, cell, touchInfo);
	}

	void FCGrid::onCellEditBegin(FCGridCell *cell){
        m_editingCell = cell;
		if (!m_editTextBox){
			FCHost *host = getNative()->getHost();
            m_editTextBox = dynamic_cast<FCTextBox*>(host->createInternalControl(this, L"edittextbox"));
			m_editTextBox->addEvent(m_editTextBoxLostFocusEvent, FCEventID::LOSTFOCUS, this);
			m_editTextBox->addEvent(m_editTextBoxKeyDownEvent, FCEventID::KEYDOWN, this);
            addControl(m_editTextBox);
        }
		m_editTextBox->setFocused(true);
		m_editTextBox->setTag(m_editingCell);
		String text = m_editingCell->getText();
		m_editTextBox->setText(text);
		m_editTextBox->clearRedoUndo();
		m_editTextBox->setVisible(true);
		if(text.length() > 0){
			m_editTextBox->setSelectionStart((int)text.length());
		}
		callCellEvents(FCEventID::GRIDCELLEDITBEGIN, cell);
	}

	void FCGrid::onCellEditEnd(FCGridCell *cell){
		if(cell){
			cell->setText(m_editTextBox->getText());
		}
		m_editTextBox->setTag(0);
		m_editTextBox->setVisible(false);
		m_editingCell = 0;
		callCellEvents(FCEventID::GRIDCELLEDITEND, cell);
		invalidate();
	}

	void FCGrid::onCellTouchDown(FCGridCell *cell, FCTouchInfo touchInfo){
		callCellTouchEvents(FCEventID::GRIDCELLTOUCHDOWN, cell, touchInfo);
	}

	void FCGrid::onCellTouchEnter(FCGridCell *cell, FCTouchInfo touchInfo){
		callCellTouchEvents(FCEventID::GRIDCELLTOUCHENTER, cell, touchInfo);
		if (autoEllipsis() || (cell->getStyle() && cell->getStyle()->autoEllipsis())){
            m_native->getHost()->showToolTip(cell->getPaintText(), m_native->getTouchPoint());
        }
	}

	void FCGrid::onCellTouchLeave(FCGridCell *cell, FCTouchInfo touchInfo){
		callCellTouchEvents(FCEventID::GRIDCELLTOUCHLEAVE, cell, touchInfo);
	}

	void FCGrid::onCellTouchMove(FCGridCell *cell, FCTouchInfo touchInfo){
		callCellTouchEvents(FCEventID::GRIDCELLTOUCHMOVE, cell, touchInfo);
	}

	void FCGrid::onCellTouchUp(FCGridCell *cell, FCTouchInfo touchInfo){
		callCellTouchEvents(FCEventID::GRIDCELLTOUCHUP, cell, touchInfo);
	}

	void FCGrid::onKeyDown(char key){
		FCHost *host = getNative()->getHost();
		if(!host->isKeyPress(VK_CONTROL)
		&& !host->isKeyPress(VK_MENU)
		&& !host->isKeyPress(VK_SHIFT)){
			if (key == 38 || key == 40){
				callKeyEvents(FCEventID::KEYDOWN, key);
				FCGridRow *row = 0;
				int offset = 0;
				if (key == 38){
					row = selectFrontRow();
					if(row){
						offset = -row->getHeight();
					}
				}
				else if (key == 40){
					row = selectNextRow();
					if(row){
						offset = row->getHeight();
					}
				}
				if (row && !isRowVisible(row, 0.6)){
					FCVScrollBar *vScrollBar = getVScrollBar();
					if (vScrollBar && vScrollBar->isVisible()){
						vScrollBar->setPos(vScrollBar->getPos() + offset);
						vScrollBar->update();
					}
				}
				invalidate();   
				return;
			}
		}
		FCDiv::onKeyDown(key);
	}

	void FCGrid::onLoad(){
		FCDiv::onLoad();
		if(m_useAnimation){
			startTimer(m_timerID, 20);
		}
		else{
			stopTimer(m_timerID);
		}
	}

	void FCGrid::onLostFocus(){
		FCDiv::onLostFocus();
		m_hoveredCell = 0;
		m_hoveredRow = 0;
	}

	void FCGrid::onTouchDown(FCTouchInfo touchInfo){
		FCDiv::onTouchDown(touchInfo);
		touchEvent(touchInfo, 1);
	}

	void FCGrid::onTouchLeave(FCTouchInfo touchInfo){
		FCDiv::onTouchLeave(touchInfo);
		if (m_hoveredCell){
            onCellTouchLeave(m_hoveredCell, touchInfo);
            m_hoveredCell = 0;
        }
		m_hoveredRow = 0;
		invalidate();
	}

	void FCGrid::onTouchMove(FCTouchInfo touchInfo){
		FCDiv::onTouchMove(touchInfo);
		touchEvent(touchInfo, 0);
	}

	void FCGrid::onTouchUp(FCTouchInfo touchInfo){
		FCDiv::onTouchUp(touchInfo);
		touchEvent(touchInfo, 2);
	}

	void FCGrid::onPaintForeground(FCPaint *paint, const FCRect& clipRect){
		resetHeaderLayout();
		int width = getWidth(), height = getHeight();
		if (width > 0 && height > 0){
			FCHost *host = getNative()->getHost();
			FCRect rect = {0, 0, width, height};
			int allVisibleColumnsWidth = getAllVisibleColumnsWidth();
			int rowVisibleWidth = 0;
			if (allVisibleColumnsWidth > 0){
				rowVisibleWidth = allVisibleColumnsWidth > width ? width : allVisibleColumnsWidth;
			}
			int hHeight = m_headerVisible ? m_headerHeight : 0;
			int scrollH = 0, scrollV = 0;
			FCHScrollBar *hScrollBar = getHScrollBar();
			FCVScrollBar *vScrollBar = getVScrollBar();
			if (hScrollBar && hScrollBar->isVisible()){
				scrollH = - hScrollBar->getPos();
			}
			if (vScrollBar && vScrollBar->isVisible()){
				scrollV = - vScrollBar->getPos();
			}
			onSetEmptyClipRegion();
			FCPoint fPoint = {0, hHeight + 1 - scrollV};
			FCPoint ePoint = {0, height - 10 - scrollV};
			FCGridRow *fRow = getRow(fPoint);
			FCGridRow *eRow = getRow(ePoint);
			while (!eRow && ePoint.y > 0){
				ePoint.y -= 10;
				eRow = getRow(ePoint);
			}
			if (fRow && eRow){
				int fIndex = fRow->getIndex();
                int eIndex = eRow->getIndex();
                for (int i = fIndex; i <= eIndex; i++){
					FCGridRow *row = m_rows.get(i);
					if (row->isVisible()){
						FCRect rowRect = row->getBounds();
						rowRect.top += scrollV;
                        rowRect.bottom += scrollV;
						row->onPaint(paint, rowRect, row->getVisibleIndex() % 2 == 1);
						FCRect tempRect = {0};
						ArrayList<FCGridCell*> cells;
						ArrayList<FCGridCell*> frozenCells;
						for (int j = 0; j < 2; j++){
							if (j == 0){
								cells = row->m_cells;
							}
							else{
								cells = frozenCells;
							}
							int frozenRight = 0;
							int cellSize = (int)cells.size();
							for (int c = 0; c < cellSize; c++){
								FCGridCell *cell = cells.get(c);
								FCGridColumn *column = cell->getColumn();
								if (column->isVisible()){
									FCRect headerRect = column->getHeaderRect();
									if (j == 0 && column->isFrozen()){
										frozenRight = headerRect.right;
										frozenCells.add(cell);
										continue;
									}
									if(!column->isFrozen()){
										headerRect.left += scrollH;
										headerRect.right += scrollH;
									}
									int cellWidth = column->getWidth();
									int colSpan = cell->getColSpan();
									if (colSpan > 1){
										for (int n = 1; n < colSpan; n++){
											FCGridColumn *spanColumn = getColumn(column->getIndex() + n);
											if(spanColumn && spanColumn->isVisible()){
												cellWidth += spanColumn->getWidth();
											}
										}
									}
									int cellHeight = row->getHeight();
									int rowSpan = cell->getRowSpan();
									if (rowSpan > 1){
										for (int n = 1; n < rowSpan; n++){
											FCGridRow *spanRow = getRow(i + n);
											if (spanRow && spanRow->isVisible()){
												cellHeight += spanRow->getHeight();
											}
										}
									}
									FCRect cellRect = {headerRect.left, rowRect.top + m_verticalOffset, headerRect.left + cellWidth, rowRect.top + m_verticalOffset + cellHeight};
									cellRect.left += row->getHorizontalOffset();
									cellRect.right += row->getHorizontalOffset();
									if (host->getIntersectRect(&tempRect, &rect, &cellRect)){
										if (cell){
											FCRect cellClipRect = cellRect;
											if(!column->isFrozen()){
												if (cellClipRect.left < frozenRight){
													cellClipRect.left = frozenRight;
												}
												if (cellClipRect.right < frozenRight){
													cellClipRect.right = frozenRight;
												}
											}
											cell->onPaint(paint, cellRect, cellClipRect, row->getVisibleIndex() % 2 == 1);
											if (m_editingCell && m_editingCell == cell && m_editTextBox){
												FCRect editClipRect = {cellClipRect.left - cellRect.left, cellClipRect.top - cellRect.top,
													cellClipRect.right - cellRect.left, cellClipRect.bottom - cellRect.top};
												onPaintEditTextBox(cell, paint, cellRect, editClipRect);
											}
											if(m_gridLineColor != FCColor_None){
												if (i == fIndex){
													paint->drawLine(m_gridLineColor, 1, 0, cellClipRect.left, cellClipRect.top, cellClipRect.right - 1, cellClipRect.top);
												}
												if (c == 0){
													paint->drawLine(m_gridLineColor, 1, 0, cellClipRect.left, cellClipRect.top, cellClipRect.left, cellClipRect.bottom - 1);
												}
												paint->drawLine(m_gridLineColor, 1, 0, cellClipRect.left, cellClipRect.bottom - 1, cellClipRect.right - 1, cellClipRect.bottom - 1);
												paint->drawLine(m_gridLineColor, 1, 0, cellClipRect.right - 1, cellClipRect.top, cellClipRect.right - 1, cellClipRect.bottom - 1);
											}
										}
									}
								}
							}
						}
						frozenCells.clear();
						row->onPaintBorder(paint, rowRect, row->getVisibleIndex() % 2 == 1);
					}
				}
			}
		}
	}

	void FCGrid::onPaintEditTextBox(FCGridCell *cell, FCPaint *paint, const FCRect& rect, const FCRect& clipRect){
		m_editTextBox->setRegion(clipRect);
        m_editTextBox->setBounds(rect);
        m_editTextBox->setDisplayOffset(false);
        m_editTextBox->bringToFront();
	}

	void FCGrid::onRowEditBegin(FCGridRow *row){
		FCView *editButton = row->getEditButton();
        if (editButton && !containsControl(editButton)){
            FCPoint mp = getTouchPoint();
            if (mp.x - m_touchDownPoint.x < -10){
                m_editingRow = row;
                addControl(editButton);
                if (m_useAnimation){
					FCPoint location = {-10000, -10000};
					editButton->setLocation(location);
                    m_editingRow->m_editState = 1;
                }
                else{
					m_editingRow->setHorizontalOffset(-editButton->getWidth() - ((m_vScrollBar && m_vScrollBar->isVisible()) ? m_vScrollBar->getWidth() : 0));
                }
            }
        }
	}

	void FCGrid::onRowEditEnd(){
	    if (m_useAnimation){
            m_editingRow->m_editState = 2;
        }
        else{
            m_editingRow->setHorizontalOffset(0);
            removeControl(m_editingRow->getEditButton());
            m_editingRow = 0;
        }
	}

	void FCGrid::onSelectedCellsChanged(){
		callEvents(FCEventID::GRIDSELECTEDCELLSCHANGED);
	}

	void FCGrid::onSelectedColumnsChanged(){
		callEvents(FCEventID::GRIDSELECTEDCOLUMNSSCHANGED);
	}

	void FCGrid::onSelectedRowsChanged(){
		callEvents(FCEventID::GRIDSELECTEDROWSCHANGED);
	}

	void FCGrid::onSetEmptyClipRegion(){
		ArrayList<FCView*> controls = getControls();
		FCRect emptyClipRect = {0};
		for(int c = 0; c < controls.size(); c++){
			FCView *control = controls.get(c);
			if (m_editingRow && control == m_editingRow->getEditButton()){
                continue;
            }
            FCScrollBar *scrollBar = dynamic_cast<FCScrollBar*>(control);
            FCGridColumn *gridColumn = dynamic_cast<FCGridColumn*>(control);
            if (control != m_editTextBox && !scrollBar && !gridColumn){
                control->setRegion(emptyClipRect);
            }
		}
	}

	void FCGrid::onTimer(int timerID){
		FCDiv::onTimer(timerID);
		if (m_timerID == timerID){
			if(m_useAnimation){
				bool paint = false;
				if (m_horizontalOffset != 0 || m_verticalOffset != 0){
					if (m_horizontalOffset != 0){
						m_horizontalOffset = m_horizontalOffset * 2 / 3;
						if (m_horizontalOffset >= -1 && m_horizontalOffset <= 1){
							m_horizontalOffset = 0;
						}
					}
					if (m_verticalOffset != 0){
						m_verticalOffset = m_verticalOffset * 2 / 3;
						if (m_verticalOffset >= -1 && m_verticalOffset <= 1){
							m_verticalOffset = 0;
						}
					}
					paint = true;
				}
				int animateAddRowsSize = (int)m_animateAddRows.size();
                if (animateAddRowsSize > 0){
                    int width = getAllVisibleColumnsWidth();
                    int step = width / 10;
                    if (step < 10){
                        step = 10;
                    }
                    for (int i = 0; i < animateAddRowsSize; i++){
                        FCGridRow *row = m_animateAddRows.get(i);
                        int horizontalOffset = row->getHorizontalOffset();
                        if (horizontalOffset > step){
                            horizontalOffset -= step;
                        }
                        else{
                            horizontalOffset = 0;
                        }
                        row->setHorizontalOffset(horizontalOffset);
                        if (horizontalOffset == 0){
							m_animateAddRows.removeAt(i);
                            animateAddRowsSize--;
                            i--;
                        }
                    }
                    paint = true;
                }
                int animateRemoveRowsSize = (int)m_animateRemoveRows.size();
                if (animateRemoveRowsSize > 0){
                    int width = getAllVisibleColumnsWidth();
                    int step = width / 10;
                    if (step < 10){
                        step = 10;
                    }
                    for (int i = 0; i < animateRemoveRowsSize; i++){
                        FCGridRow *row = m_animateRemoveRows.get(i);
                        int horizontalOffset = row->getHorizontalOffset();
                        if (horizontalOffset <= width){
                            horizontalOffset += step;
                        }
                        row->setHorizontalOffset(horizontalOffset);
                        if (horizontalOffset > width){
							m_animateRemoveRows.removeAt(i);
                            removeRow(row);
                            update();
                            animateRemoveRowsSize--;
                            i--;
                        }
                    }
                    paint = true;
                }
				if (m_editingRow){
                    int scrollH = 0, scrollV = 0;
                    FCHScrollBar *hScrollBar = getHScrollBar();
                    FCVScrollBar *vScrollBar = getVScrollBar();
                    int vScrollBarW = 0;
                    if (hScrollBar && hScrollBar->isVisible()){
                        scrollH = -hScrollBar->getPos();
                    }
                    if (vScrollBar && vScrollBar->isVisible()){
                        scrollV = -vScrollBar->getPos();
                        vScrollBarW = vScrollBar->getWidth();
                    }
                    if (m_editingRow->m_editState == 1){
                        FCView *editButton = m_editingRow->getEditButton();
                        bool isOver = false;
                        int sub = editButton->getWidth() + vScrollBarW + m_editingRow->getHorizontalOffset();
                        if (sub < 2){
                            isOver = true;
                            m_editingRow->setHorizontalOffset(-editButton->getWidth() - vScrollBarW);
                        }
                        else{
                            m_editingRow->setHorizontalOffset(m_editingRow->getHorizontalOffset() - 10);
                        }
						FCPoint newLocation = {getAllVisibleColumnsWidth() + scrollH + m_editingRow->getHorizontalOffset(),
							m_editingRow->getBounds().top + scrollV};
                        editButton->setLocation(newLocation);
                        if (isOver){
                            m_editingRow->m_editState = 0;
                        }
                    }
                    if (m_editingRow->m_editState == 2){
                        FCView *editButton = m_editingRow->getEditButton();
                        bool isOver = false;
                        if (m_editingRow->getHorizontalOffset() < 0){
                            m_editingRow->setHorizontalOffset(m_editingRow->getHorizontalOffset() + 10);
                            if (m_editingRow->getHorizontalOffset() >= 0){
                                m_editingRow->setHorizontalOffset(0);
                                isOver = true;
                            }
                        }
						FCPoint newLocation = {getAllVisibleColumnsWidth() + scrollH + m_editingRow->getHorizontalOffset(),
                        m_editingRow->getBounds().top + scrollV};
						editButton->setLocation(newLocation);
                        if (isOver){
                            removeControl(editButton);
                            m_editingRow->m_editState = 0;
                            m_editingRow = 0;
                        }
                    }
                    paint = true;
                }
                if (paint){
                    invalidate();
                }
			}
		}
	}


	void FCGrid::onVisibleChanged(){
		FCDiv::onVisibleChanged();
		m_hoveredCell = 0;
		m_hoveredRow = 0;
	}

	void FCGrid::removeColumn(FCGridColumn *column){
		bool selectedChanged = false;
		for(int i = 0; i < m_selectedColumns.size(); i++){
			if(m_selectedColumns.get(i) != column){
				m_selectedColumns.removeAt(i);
				selectedChanged = true;
				break;
			}
		}
		for(int i = 0; i < m_columns.size(); i++){
			if(m_columns.get(i) == column){
				m_columns.removeAt(i);
				int columnsSize = (int)m_columns.size();
				for (int i = 0; i < columnsSize; i++){
					m_columns.get(i)->setIndex(i);
				}
				removeControl(column);
				break;
			}
		}
		int rowSize = (int)m_rows.size();
		for (int i = 0; i < rowSize; i++){
			FCGridRow *row = m_rows.get(i);
			row->removeCell(column->getIndex());
		}
		if(selectedChanged){
			onSelectedColumnsChanged();
		}
	}

	void FCGrid::removeRow(FCGridRow *row){
		if (m_editingRow){
                if (containsControl(m_editingRow->getEditButton())){
                    removeControl(m_editingRow->getEditButton());
                }
            m_editingRow->m_editState = 0;
            m_editingRow = 0;
        }
		int animateAddRowsSize = m_animateAddRows.size();
		for(int i = 0; i < animateAddRowsSize; i++){
			if(m_animateAddRows.get(i) ==  row){
				m_animateAddRows.removeAt(i);
				break;
			}
		}
		bool selectedChanged = false;
        bool selected = false;
        int selectedRowsSize = (int)m_selectedRows.size();
        for (int i = 0; i < selectedRowsSize; i++){
            FCGridRow *selectedRow = m_selectedRows.get(i);
            if (selectedRow == row){
                selected = true;
                break;
            }
        }
        if (selected){
            FCGridRow *otherRow = selectFrontRow();
            if (otherRow){
                selectedChanged = true;
            }
            else{
                otherRow = selectNextRow();
				if(otherRow){
					selectedChanged = true;
				}
            }
        }
        if (m_hoveredRow == row){
			m_hoveredCell = 0;
            m_hoveredRow = 0;
        }
		for(int i = 0; i < m_rows.size(); i++){
			if(m_rows.get(i) == row){
				row->onRemove();
				m_rows.removeAt(i);
				break;
			}
		}
		int rowSize = (int)m_rows.size();
		if(rowSize == 0){
			m_selectedCells.clear();
			m_selectedRows.clear();
		}
		int visibleIndex = 0;
		for (int i = 0; i < rowSize; i++){
			FCGridRow *gridRow = m_rows.get(i);
			gridRow->setIndex(i);
			if(gridRow->isVisible()){
				gridRow->setVisibleIndex(visibleIndex);
				visibleIndex++;
			}
		}
		if(selected){
			if(selectedChanged){
				onSelectedRowsChanged();
			}
			else{
				m_selectedCells.clear();
				m_selectedRows.clear();
			}
		}
	}

	void FCGrid::resetHeaderLayout(){
		if (!m_lockUpdate){
			int left = 0, top =0;
			int scrollH = 0, scrollV = 0;
            FCHScrollBar *hScrollBar = getHScrollBar();
            FCVScrollBar *vScrollBar = getVScrollBar();
            int vScrollBarW = 0;
            if (hScrollBar && hScrollBar->isVisible()){
                scrollH = -hScrollBar->getPos();
            }
            if (vScrollBar && vScrollBar->isVisible()){
                scrollV = -vScrollBar->getPos();
                vScrollBarW = vScrollBar->getWidth();
            }
			int headerHeight = m_headerVisible ? m_headerHeight : 0;
			FCGridColumn *draggingColumn = 0;
			int colSize = (int)m_columns.size();
			for (int i = 0; i < colSize; i++){
				FCGridColumn *column = m_columns.get(i);
				if (column->isVisible()){
					FCRect cellRect = {left + m_horizontalOffset, top + m_verticalOffset,
					left + m_horizontalOffset + column->getWidth(), top + headerHeight + m_verticalOffset};
					column->setHeaderRect(cellRect);
					if (column->isDragging()){
						draggingColumn = column;
						FCRect newRect = {column->getLeft(), cellRect.top, column->getRight(), cellRect.bottom};
						column->setBounds(newRect);
					}
					else{
						if (!column->isFrozen()){
							cellRect.left += scrollH;
							cellRect.right += scrollH;
						}
						column->setBounds(cellRect);
					}
					left += column->getWidth();
				}
			}
			for (int i = colSize - 1; i >= 0; i--){
				m_columns.get(i)->bringToFront();
			}
			if(draggingColumn){
				draggingColumn->bringToFront();
			}
		    if (m_editingRow && m_editingRow->m_editState == 0 && m_editingRow->getEditButton()){
                FCView *editButton = m_editingRow->getEditButton();
				FCPoint newLocation = {getAllVisibleColumnsWidth() - editButton->getWidth() + scrollH - vScrollBarW, m_editingRow->getBounds().top + scrollV};
                editButton->setLocation(newLocation);
            }
		}
	}

	FCGridRow* FCGrid::selectFrontRow(){
		int rowsSize = (int)m_rows.size();
	    if (rowsSize == 0){
			m_selectedCells.clear();
            m_selectedRows.clear();
            return 0;
        }
		FCGridRow *frontRow = 0;
		ArrayList<FCGridRow*> selectedRows = getSelectedRows();
		if ((int)selectedRows.size() == 1){
            FCGridRow *selectedRow = selectedRows.get(0);
            int selectedIndex = selectedRow->getIndex();
            for (int i = selectedIndex - 1; i >= 0; i--){
                if (i < rowsSize && m_rows.get(i)->isVisible()){
                    frontRow = m_rows.get(i);
                    break;
                }
            }
			if(m_selectionMode == FCGridSelectionMode_SelectFullRow){
				if (frontRow){
                    m_selectedRows.clear();
                    m_selectedRows.add(frontRow);
					onSelectedRowsChanged();
                }
                else{
                    m_selectedRows.clear();
					frontRow = m_rows.get((int)m_rows.size() - 1);
                    m_selectedRows.add(frontRow);
                    FCVScrollBar *vScrollBar = getVScrollBar();
                    if (vScrollBar && vScrollBar->isVisible()){
                        vScrollBar->scrollToEnd();
                    }
			onSelectedRowsChanged();
                }
			}
		}
		return frontRow;
	}

	FCGridRow* FCGrid::selectNextRow(){
		int rowsSize = (int)m_rows.size();
		if (rowsSize == 0){
			m_selectedCells.clear();
            m_selectedRows.clear();
            return 0;
        }
		FCGridRow *nextRow = 0;
		ArrayList<FCGridRow*> selectedRows = getSelectedRows();
		if ((int)selectedRows.size() == 1){
			FCGridRow *selectedRow = selectedRows.get(0);
            int selectedIndex = selectedRow->getIndex();
            for (int i = selectedIndex + 1; i < rowsSize; i++){
                if (i >= 0 && m_rows.get(i)->isVisible()){
                    nextRow = m_rows.get(i);
                    break;
                }
            }
			if(m_selectionMode == FCGridSelectionMode_SelectFullRow){
					if (nextRow){
                        m_selectedRows.clear();
                        m_selectedRows.add(nextRow);
						onSelectedRowsChanged();
                    }
                    else{
                        m_selectedRows.clear();
						nextRow = m_rows.get(0);
                        m_selectedRows.add(nextRow);
						FCVScrollBar *vScrollBar = getVScrollBar();
						if (vScrollBar && vScrollBar->isVisible()){
							vScrollBar->scrollToBegin();
						}
						onSelectedRowsChanged();
                    }
			}
		}
		return nextRow;
	}

	void FCGrid::setProperty(const String& name, const String& value){
		if(name == L"allowdragrow"){
			setAllowDragRow(FCStr::convertStrToBool(value));
		}
		else if (name == L"allowhoveredrow"){
			setAllowHoveredRow(FCStr::convertStrToBool(value));
        }
	    else if (name == L"celleditmode"){
			String lowerStr = FCStr::toLower(value);
            if (lowerStr == L"doubleclick"){
                setCellEditMode(FCGridCellEditMode_DoubleClick);
            }
            else if (lowerStr == L"none"){
                setCellEditMode(FCGridCellEditMode_None);
            }
            else if (lowerStr == L"singleclick"){
                setCellEditMode(FCGridCellEditMode_SingleClick);
            }
        }
		else if(name == L"gridlinecolor"){
			setGridLineColor(FCStr::convertStrToColor(value));
		}
		else if(name == L"headerheight"){
			setHeaderHeight(FCStr::convertStrToInt(value));
		}
		else if(name == L"headervisible"){
			setHeaderVisible(FCStr::convertStrToBool(value));
		}
		else if(name == L"horizontaloffset"){
			setHorizontalOffset(FCStr::convertStrToInt(value));
		}
		else if(name == L"multiselect"){
			setMultiSelect(FCStr::convertStrToBool(value));
		}
	    else if (name == L"selectionmode"){
			String lowerStr = FCStr::toLower(value);
            if (lowerStr == L"selectcell"){
                setSelectionMode(FCGridSelectionMode_SelectCell);
            }
            else if (lowerStr == L"selectfullcolumn"){
                setSelectionMode(FCGridSelectionMode_SelectFullColumn);
            }
            else if (lowerStr == L"selectfullrow"){
                setSelectionMode(FCGridSelectionMode_SelectFullRow);
            }
            else{
                setSelectionMode(FCGridSelectionMode_SelectNone);
            }
        }
		else if(name == L"useanimation"){
			setUseAnimation(FCStr::convertStrToBool(value));
		}
		else if(name == L"verticaloffset"){
			setVerticalOffset(FCStr::convertStrToInt(value));
		}
		else{
			FCDiv::setProperty(name, value);
		}
	}

	void FCGrid::sortColumn(FCGrid *grid, FCGridColumn *column, FCGridColumnSortMode sortMode){
		if(column->allowSort()){
			int colSize = (int)grid->m_columns.size();
			for (int i = 0; i < colSize; i++){
				if (grid->m_columns.get(i) != column){
					grid->m_columns.get(i)->setSortMode(FCGridColumnSortMode_None);
				}
				else{
					grid->m_columns.get(i)->setSortMode(sortMode);
				}
			}
			if (m_sort){
				m_sort->sortColumn(grid, column, sortMode);
			}
			grid->update();
			grid->invalidate();
		}
	}

	void FCGrid::update(){
		if(getNative()){
			if (!m_lockUpdate){
				FCDiv::update();
				if(isVisible()){
					int colSize = (int)m_columns.size();
					for (int i = 0; i < colSize; i++){
						m_columns.get(i)->setIndex(i);
					}
					int rowSize = (int)m_rows.size();
					int visibleIndex = 0;
					int rowTop = m_headerVisible ? m_headerHeight : 0;
					int allVisibleColumnsWidth = getAllVisibleColumnsWidth();
					m_hasUnVisibleRow = false;
					for (int i = 0; i < rowSize; i++){
						FCGridRow *gridRow = m_rows.get(i);
						gridRow->setIndex(i);
                        if (gridRow->isVisible()){
                            gridRow->setVisibleIndex(i);
							int rowHeight = gridRow->getHeight();
							FCRect rowRect = {0, rowTop, allVisibleColumnsWidth, rowTop + rowHeight};
                            gridRow->setBounds(rowRect);
                            rowTop += rowHeight;
                            visibleIndex++;
                        }
					    else{
                            m_hasUnVisibleRow = true;
                            gridRow->setVisibleIndex(-1);
							FCRect rowRect = {0, rowTop, allVisibleColumnsWidth, rowTop};
                            gridRow->setBounds(rowRect);
                        }
					}
					FCHScrollBar *hScrollBar = getHScrollBar();
					FCVScrollBar *vScrollBar = getVScrollBar();
					if (vScrollBar && vScrollBar->isVisible()){
                        int top = m_headerVisible ? m_headerHeight : 0;
                        vScrollBar->setTop(top);
                        int height = getHeight() - top - ((hScrollBar && hScrollBar->isVisible()) ? hScrollBar->getHeight() : 0);
                        vScrollBar->setHeight(height);
						vScrollBar->setPageSize(height);
						if (rowSize > 0){
                            vScrollBar->setLineSize(getAllVisibleRowsHeight() / rowSize);
                        }
                    }
				}
			}
		}
	}


	void FCGrid::updateSortColumn(){
		int colSize = (int)m_columns.size();
		for (int i = 0; i < colSize; i++){
			if (m_columns.get(i)->getSortMode() != FCGridColumnSortMode_None){
				sortColumn(this, m_columns.get(i), m_columns.get(i)->getSortMode());
				break;
			}
		}
	}
}