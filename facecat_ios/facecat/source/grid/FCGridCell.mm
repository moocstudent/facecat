#include "stdafx.h"
#include "FCGridCell.h"

namespace FaceCat{
    FCGridCellStyle::FCGridCellStyle(){
        m_align = FCHorizontalAlign_Inherit;
        m_autoEllipsis = false;
        m_backColor = FCColor_None;
        m_font = 0;
        m_textColor = FCColor_None;
    }
    
    FCGridCellStyle::~FCGridCellStyle(){
        if(m_font){
            delete m_font;
            m_font = 0;
        }
    }
    
    FCHorizontalAlign FCGridCellStyle::getAlign(){
        return m_align;
    }
    
    void FCGridCellStyle::setAlign(FCHorizontalAlign align){
        m_align = align;
    }
    
    bool FCGridCellStyle::autoEllipsis(){
        return m_autoEllipsis;
    }
    
    void FCGridCellStyle::setAutoEllipsis(bool autoEllipsis){
        m_autoEllipsis = autoEllipsis;
    }
    
    Long FCGridCellStyle::getBackColor(){
        return m_backColor;
    }
    
    void FCGridCellStyle::setBackColor(Long backColor){
        m_backColor = backColor;
    }
    
    FCFont* FCGridCellStyle::getFont(){
        return m_font;
    }
    
    void FCGridCellStyle::setFont(FCFont *font){
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
    
    Long FCGridCellStyle::getTextColor(){
        return m_textColor;
    }
    
    void FCGridCellStyle::setTextColor(Long textColor){
        m_textColor = textColor;
    }
    
    void FCGridCellStyle::copy(FCGridCellStyle *style){
        setAutoEllipsis(style->autoEllipsis());
        setBackColor(style->getBackColor());
        setFont(style->getFont());
        setTextColor(style->getTextColor());
        setAlign(style->getAlign());
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    
    FCGridCell::FCGridCell(){
        m_allowEdit = false;
        m_colSpan = 1;
        m_column = 0;
        m_grid = 0;
        m_row = 0;
        m_rowSpan = 1;
        m_style = 0;
        m_tag = 0;
    }
    
    FCGridCell::~FCGridCell(){
        m_column = 0;
        m_grid = 0;
        m_row = 0;
        if(m_style){
            delete m_style;
            m_style = 0;
        }
        m_tag = 0;
    }
    
    bool FCGridCell::allowEdit(){
        return m_allowEdit;
    }
    
    void FCGridCell::setAllowEdit(bool allowEdit){
        m_allowEdit = allowEdit;
    }
    
    int FCGridCell::getColSpan(){
        return m_colSpan;
    }
    
    void FCGridCell::setColSpan(int colSpan){
        m_colSpan = colSpan;
    }
    
    FCGridColumn* FCGridCell::getColumn(){
        return m_column;
    }
    
    void FCGridCell::setColumn(FCGridColumn *column){
        m_column = column;
    }
    
    FCGrid* FCGridCell::getGrid(){
        return m_grid;
    }
    
    void FCGridCell::setGrid(FCGrid *grid){
        m_grid = grid;
    }
    
    String FCGridCell::getName(){
        return m_name;
    }
    
    void FCGridCell::setName(const String& name){
        m_name = name;
    }
    
    FCGridRow* FCGridCell::getRow(){
        return m_row;
    }
    
    void FCGridCell::setRow(FCGridRow *row){
        m_row = row;
    }
    
    int FCGridCell::getRowSpan(){
        return m_rowSpan;
    }
    
    void FCGridCell::setRowSpan(int rowSpan){
        m_rowSpan = rowSpan;
    }
    
    FCGridCellStyle* FCGridCell::getStyle(){
        return m_style;
    }
    
    void FCGridCell::setStyle(FCGridCellStyle *style){
        if(style){
            if(!m_style){
                m_style = new FCGridCellStyle;
            }
            m_style->copy(style);
        }
        else{
            if(m_style){
                delete m_style;
                m_style = 0;
            }
        }
    }
    
    Object FCGridCell::getTag(){
        return m_tag;
    }
    
    void FCGridCell::setTag(Object tag){
        m_tag = tag;
    }
    
    String FCGridCell::getText(){
        return getString();
    }
    
    void FCGridCell::setText(const String& text){
        setString(text);
    }
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    int FCGridCell::compareTo(FCGridCell *cell){
        return 0;
    }
    
    bool FCGridCell::getBool(){
        return false;
    }
    
    double FCGridCell::getDouble(){
        return 0;
    }
    
    float FCGridCell::getFloat(){
        return 0;
    }
    
    int FCGridCell::getInt(){
        return 0;
    }
    
    Long FCGridCell::getLong(){
        return 0;
    }
    
    String FCGridCell::getPaintText(){
        return getText();
    }
    
    void FCGridCell::getProperty(const String& name, String *value, String *type){
        if (name == L"align"){
            *type = L"enum:FCHorizontalAlign";
            FCGridCellStyle *style = getStyle();
            if (style){
                *value = FCStr::convertHorizontalAlignToStr(style->getAlign());
            }
        }
        else if(name == L"allowedit"){
            *type = L"bool";
            *value = FCStr::convertBoolToStr(allowEdit());
        }
        else if (name == L"autoellipsis"){
            *type = L"bool";
            FCGridCellStyle *style = getStyle();
            if (style){
                *value = FCStr::convertBoolToStr(style->autoEllipsis());
            }
        }
        else if(name == L"backcolor"){
            *type = L"color";
            FCGridCellStyle *style = getStyle();
            if (style){
                *value = FCStr::convertColorToStr(style->getBackColor());
            }
        }
        else if(name == L"colspan"){
            *type = L"int";
            *value = FCStr::convertIntToStr(getColSpan());
        }
        else if(name == L"font"){
            *type = L"font";
            FCGridCellStyle *style = getStyle();
            if (style && style->getFont()){
                *value = FCStr::convertFontToStr(style->getFont());
            }
        }
        else if(name == L"textcolor"){
            *type = L"color";
            FCGridCellStyle *style = getStyle();
            if (style){
                *value = FCStr::convertColorToStr(style->getTextColor());
            }
        }
        else if(name == L"name"){
            *type = L"text";
            *value = getName();
        }
        else if (name == L"rowspan"){
            *type = L"int";
            *value = FCStr::convertIntToStr(getRowSpan());
        }
        else if(name == L"text"){
            *type = L"text";
            *value = getText();
        }
        else{
            *type = L"undefined";
            *value = L"";
        }
    }
    
    ArrayList<String> FCGridCell::getPropertyNames(){
        ArrayList<String> propertyNames;
        propertyNames.add(L"Align");
        propertyNames.add(L"AllowEdit");
        propertyNames.add(L"AutoEllipsis");
        propertyNames.add(L"BackColor");
        propertyNames.add(L"ColSpan");
        propertyNames.add(L"Font");
        propertyNames.add(L"Name");
        propertyNames.add(L"RowSpan");
        propertyNames.add(L"Text");
        propertyNames.add(L"TextColor");
        return propertyNames;
    }
    
    String FCGridCell::getString(){
        return L"";
    }
    
    void FCGridCell::onAdd(){
    }
    
    void FCGridCell::onPaint(FCPaint *paint, const FCRect& rect, const FCRect& clipRect, bool isAlternate){
        int clipW = clipRect.right - clipRect.left;
        int clipH = clipRect.bottom - clipRect.top;
        if (clipW > 0 && clipH> 0){
            if(m_grid && m_row && m_column){
                String text = getPaintText();
                bool selected = false;
                FCGridSelectionMode selectionMode = m_grid->getSelectionMode();
                if(selectionMode == FCGridSelectionMode_SelectCell){
                    ArrayList<FCGridCell*> selectedCells = m_grid->getSelectedCells();
                    int cellSize = (int)selectedCells.size();
                    for (int i = 0; i < cellSize; i++){
                        if (selectedCells.get(i) == this){
                            selected = true;
                            break;
                        }
                    }
                }
                else if (selectionMode == FCGridSelectionMode_SelectFullColumn){
                    ArrayList<FCGridColumn*> selectedColumns = m_grid->getSelectedColumns();
                    int selectedColumnsSize = (int)selectedColumns.size();
                    for(int i = 0; i < selectedColumnsSize; i++){
                        if(selectedColumns.get(i) == m_column){
                            selected = true;
                            break;
                        }
                    }
                }
                else if (selectionMode == FCGridSelectionMode_SelectFullRow){
                    ArrayList<FCGridRow*> selectedRows = m_grid->getSelectedRows();
                    int selectedRowsSize = (int)selectedRows.size();
                    for(int i = 0; i < selectedRowsSize; i++){
                        if(selectedRows.get(i) == m_row){
                            selected = true;
                            break;
                        }
                    }
                }
                FCFont *font = 0;
                Long backColor = FCColor_None;
                Long textColor = FCColor_None;
                bool autoEllipsis = m_grid->autoEllipsis();
                FCHorizontalAlign horizontalAlign = m_column->getCellAlign();
                if (m_style){
                    if (m_style->autoEllipsis()){
                        autoEllipsis = m_style->autoEllipsis();
                    }
                    backColor = m_style->getBackColor();
                    if (m_style->getFont()){
                        font = m_style->getFont();
                    }
                    textColor = m_style->getTextColor();
                    if (m_style->getAlign() != FCHorizontalAlign_Inherit){
                        horizontalAlign = m_style->getAlign();
                    }
                }
                FCGridRowStyle *rowStyle = m_grid->getRowStyle();
                if (isAlternate){
                    FCGridRowStyle *alternateRowStyle = m_grid->getAlternateRowStyle();
                    if (alternateRowStyle){
                        rowStyle = alternateRowStyle;
                    }
                }
                if(rowStyle){
                    if (backColor == FCColor_None){
                        if (selected){
                            backColor = rowStyle->getSelectedBackColor();
                        }
                        else if (m_row == m_grid->getHoveredRow()){
                            backColor = rowStyle->getHoveredBackColor();
                        }
                        else{
                            backColor = rowStyle->getBackColor();
                        }
                    }
                    if(!font){
                        font = rowStyle->getFont();
                    }
                    if (textColor == FCColor_None){
                        if (selected){
                            textColor = rowStyle->getSelectedTextColor();
                        }
                        else if (m_row == m_grid->getHoveredRow()){
                            textColor = rowStyle->getHoveredTextColor();
                        }
                        else{
                            textColor = rowStyle->getTextColor();
                        }
                    }
                }
                paint->fillRect(backColor, rect);
                FCSize tSize = paint->textSize(text.c_str(), font);
                FCRect tRect = {0};
                tRect.left = rect.left + 1;
                tRect.top = rect.top + (clipH - tSize.cy) / 2;
                int width = rect.right - rect.left;
                if(tSize.cx < width){
                    if (horizontalAlign == FCHorizontalAlign_Center){
                        tRect.left = rect.left + (rect.right - rect.left - tSize.cx) / 2;
                    }
                    else if (horizontalAlign == FCHorizontalAlign_Right){
                        tRect.left = rect.right - tSize.cx - 2;
                    }
                }
                tRect.right = tRect.left + tSize.cx;
                tRect.bottom = tRect.top + tSize.cy;
                if(autoEllipsis && (tRect.right < clipRect.right || tRect.bottom < clipRect.bottom)){
                    if(tRect.right < clipRect.right){
                        tRect.right = clipRect.right;
                    }
                    if(tRect.bottom < clipRect.bottom){
                        tRect.bottom = clipRect.bottom;
                    }
                    paint->drawTextAutoEllipsis(text.c_str(), textColor, font, tRect);
                }
                else{
                    paint->drawText(text.c_str(), textColor, font, tRect);
                }
            }
        }
    }
    
    void FCGridCell::onRemove(){
    }
    
    void FCGridCell::setBool(bool value){
    }
    
    void FCGridCell::setDouble(double value){
    }
    
    void FCGridCell::setFloat(float value){
    }
    
    void FCGridCell::setInt(int value){
    }
    
    void FCGridCell::setLong(Long value){
    }
    
    void FCGridCell::setProperty(const String& name, const String& value){
        if (name == L"align"){
            if (!m_style){
                m_style = new FCGridCellStyle;
            }
            m_style->setAlign(FCStr::convertStrToHorizontalAlign(value));
        }
        else if(name == L"allowedit"){
            setAllowEdit(FCStr::convertStrToBool(value));
        }
        else if (name == L"autoellipsis"){
            if (!m_style){
                m_style = new FCGridCellStyle;
            }
            m_style->setAutoEllipsis(FCStr::convertStrToBool(value));
        }
        else if(name == L"backcolor"){
            if (!m_style){
                m_style = new FCGridCellStyle;
            }
            m_style->setBackColor(FCStr::convertStrToColor(value));
        }
        else if (name == L"colspan"){
            setColSpan(FCStr::convertStrToInt(value));
        }
        else if(name == L"font"){
            if (!m_style){
                m_style = new FCGridCellStyle;
            }
            m_style->setFont(FCStr::convertStrToFont(value));
        }
        else if(name == L"textcolor"){
            if (!m_style){
                m_style = new FCGridCellStyle;
            }
            m_style->setTextColor(FCStr::convertStrToColor(value));
        }
        else if(name == L"name"){
            setName(value);
        }
        else if (name == L"rowspan"){
            setRowSpan(FCStr::convertStrToInt(value));
        }
        else if(name == L"text"){
            setText(value);
        }
    }
    
    void FCGridCell::setString(const String& value){
    }
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    void FCGridControlCell::controlTouchDown(Object sender, FCTouchInfo touchInfo, Object pInvoke){
        FCGridControlCell *gridControlCell = (FCGridControlCell*)pInvoke;
        gridControlCell->onControlTouchDown(touchInfo);
    }
    
    void FCGridControlCell::controlTouchMove(Object sender, FCTouchInfo touchInfo, Object pInvoke){
        FCGridControlCell *gridControlCell = (FCGridControlCell*)pInvoke;
        gridControlCell->onControlTouchMove(touchInfo);
    }
    
    void FCGridControlCell::controlTouchUp(Object sender, FCTouchInfo touchInfo, Object pInvoke){
        FCGridControlCell *gridControlCell = (FCGridControlCell*)pInvoke;
        gridControlCell->onControlTouchUp(touchInfo);
    }
    
    void FCGridControlCell::controlTouchWheel(Object sender, FCTouchInfo touchInfo, Object pInvoke){
        FCGridControlCell *gridControlCell = (FCGridControlCell*)pInvoke;
        gridControlCell->onControlTouchWheel(touchInfo);
    }
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    FCGridControlCell::FCGridControlCell(){
        m_control = 0;
        m_touchDownEvent = &controlTouchDown;
        m_touchMoveEvent = &controlTouchMove;
        m_touchUpEvent = &controlTouchUp;
        m_touchWheelEvent = &controlTouchWheel;
    }
    
    FCGridControlCell::~FCGridControlCell(){
        if(m_control){
            delete m_control;
            m_control = 0;
        }
        m_touchDownEvent = 0;
        m_touchMoveEvent = 0;
        m_touchUpEvent = 0;
        m_touchWheelEvent = 0;
    }
    
    FCView* FCGridControlCell::getControl(){
        return m_control;
    }
    
    void FCGridControlCell::setControl(FCView *control){
        m_control = control;
    }
    
    String FCGridControlCell::getPaintText(){
        return L"";
    }
    
    String FCGridControlCell::getString(){
        if(m_control){
            return m_control->getText();
        }
        else{
            return L"";
        }
    }
    
    void FCGridControlCell::onAdd(){
        FCGrid *grid = getGrid();
        if (m_control && grid){
            grid->addControl(m_control);
            m_control->addEvent((void*)m_touchDownEvent, FCEventID::TOUCHDOWN, this);
            m_control->addEvent((void*)m_touchMoveEvent, FCEventID::TOUCHMOVE, this);
            m_control->addEvent((void*)m_touchUpEvent, FCEventID::TOUCHUP, this);
            m_control->addEvent((void*)m_touchWheelEvent, FCEventID::TOUCHWHEEL, this);
        }
    }
    
    void FCGridControlCell::onControlTouchDown(FCTouchInfo touchInfo){
        FCGrid *grid = getGrid();
        if(grid && m_control){
            FCTouchInfo newTouchInfo = touchInfo;
            newTouchInfo.m_firstPoint = grid->pointToControl(m_control->pointToNative(newTouchInfo.m_firstPoint));
            newTouchInfo.m_secondPoint = grid->pointToControl(m_control->pointToNative(newTouchInfo.m_secondPoint));
            grid->onTouchDown(newTouchInfo);
        }
    }
    
    void FCGridControlCell::onControlTouchMove(FCTouchInfo touchInfo){
        FCGrid *grid = getGrid();
        if(grid && m_control){
            FCTouchInfo newTouchInfo = touchInfo;
            newTouchInfo.m_firstPoint = grid->pointToControl(m_control->pointToNative(newTouchInfo.m_firstPoint));
            newTouchInfo.m_secondPoint = grid->pointToControl(m_control->pointToNative(newTouchInfo.m_secondPoint));
            grid->onTouchMove(newTouchInfo);
        }
    }
    
    void FCGridControlCell::onControlTouchUp(FCTouchInfo touchInfo){
        FCGrid *grid = getGrid();
        if(grid && m_control){
            FCTouchInfo newTouchInfo = touchInfo;
            newTouchInfo.m_firstPoint = grid->pointToControl(m_control->pointToNative(newTouchInfo.m_firstPoint));
            newTouchInfo.m_secondPoint = grid->pointToControl(m_control->pointToNative(newTouchInfo.m_secondPoint));
            grid->onTouchUp(newTouchInfo);
        }
    }
    
    void FCGridControlCell::onControlTouchWheel(FCTouchInfo touchInfo){
        FCGrid *grid = getGrid();
        if(grid && m_control){
            FCTouchInfo newTouchInfo = touchInfo;
            newTouchInfo.m_firstPoint = grid->pointToControl(m_control->pointToNative(newTouchInfo.m_firstPoint));
            newTouchInfo.m_secondPoint = grid->pointToControl(m_control->pointToNative(newTouchInfo.m_secondPoint));
            grid->onTouchWheel(newTouchInfo);
        }
    }
    
    void FCGridControlCell::onPaint(FCPaint *paint, const FCRect& rect, const FCRect& clipRect, bool isAlternate){
        FCGridCell::onPaint(paint, rect, clipRect, isAlternate);
        onPaintControl(paint, rect, clipRect);
    }
    
    void FCGridControlCell::onPaintControl(FCPaint *paint, const FCRect& rect, const FCRect& clipRect){
        if (m_control){
            FCRect bounds = {rect.left + 1, rect.top + 1, rect.right - 1, rect.bottom - 1};
            m_control->setBounds(bounds);
            FCRect newClipRect = clipRect;
            newClipRect.left -= rect.left;
            newClipRect.top -=  rect.top;
            newClipRect.right -= rect.left;
            newClipRect.bottom -= rect.top;
            m_control->setRegion(newClipRect);
        }
    }
    
    void FCGridControlCell::onRemove(){
        FCGrid *grid = getGrid();
        if (m_control && grid){
            m_control->removeEvent((void*)m_touchDownEvent, FCEventID::TOUCHDOWN);
            m_control->removeEvent((void*)m_touchMoveEvent, FCEventID::TOUCHMOVE);
            m_control->removeEvent((void*)m_touchUpEvent, FCEventID::TOUCHUP);
            m_control->removeEvent((void*)m_touchWheelEvent, FCEventID::TOUCHWHEEL);
            grid->removeControl(m_control);
        }
    }
    
    void FCGridControlCell::setString(const String& value){
        if(m_control){
            return m_control->setText(value);
        }
    }
}
