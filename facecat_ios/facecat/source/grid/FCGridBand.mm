#include "stdafx.h"
#include "FCGridBand.h"

namespace FaceCat{
    FCGridBand::FCGridBand(){
        m_allowResize = false;
        m_beginWidth = 0;
        m_grid = 0;
        m_index = -1;
        m_touchDownPoint.x = 0;
        m_touchDownPoint.y = 0;
        m_parentBand = 0;
        m_resizeState = 0;
    }
    
    FCGridBand::~FCGridBand(){
        clearBands();
        clearColumns();
        m_grid = 0;
        m_parentBand = 0;
    }
    
    bool FCGridBand::allowResize(){
        return m_allowResize;
    }
    
    void FCGridBand::setAllowResize(bool allowResize){
        m_allowResize = allowResize;
    }
    
    FCBandedGrid* FCGridBand::getGrid(){
        return m_grid;
    }
    
    void FCGridBand::setGrid(FCBandedGrid *grid){
        m_grid = grid;
    }
    
    int FCGridBand::getIndex(){
        return m_index;
    }
    
    void FCGridBand::setIndex(int index){
        m_index = index;
    }
    
    FCGridBand* FCGridBand::getParentBand(){
        return m_parentBand;
    }
    
    void FCGridBand::setParentBand(FCGridBand *parentBand){
        m_parentBand = parentBand;
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    void FCGridBand::addBand(FCGridBand *band){
        band->setGrid(m_grid);
        band->setParentBand(this);
        m_bands.add(band);
        int bandSize = (int)m_bands.size();
        for(int i = 0; i < bandSize; i++){
            m_bands.get(i)->setIndex(i);
        }
        m_grid->addControl(band);
    }
    
    void FCGridBand::addColumn(FCBandedGridColumn *column){
        column->setBand(this);
        m_columns.add(column);
        m_grid->addColumn(column);
    }
    
    void FCGridBand::clearBands(){
        for(int i = 0; i < m_bands.size(); i++){
            FCGridBand *band = m_bands.get(i);
            m_grid->removeControl(band);
            delete band;
        }
        m_bands.clear();
    }
    
    void FCGridBand::clearColumns(){
        for(int i = 0; i < m_columns.size(); i++){
            FCBandedGridColumn *column = m_columns.get(i);
            m_grid->removeColumn(column);
            delete column;
        }
        m_columns.clear();
    }
    
    ArrayList<FCBandedGridColumn*> FCGridBand::getAllChildColumns(){
        ArrayList<FCBandedGridColumn*> columns;
        for(int i = 0; i < m_columns.size(); i++){
            FCBandedGridColumn *column = m_columns.get(i);
            columns.add(column);
        }
        for(int i = 0; i < m_bands.size(); i++){
            FCGridBand *band = m_bands.get(i);
            ArrayList<FCBandedGridColumn*> childColumns = band->getAllChildColumns();
            for(int j = 0; j < childColumns.size(); j++){
                FCBandedGridColumn *childColumn = childColumns.get(j);
                columns.add(childColumn);
            }
        }
        return columns;
    }
    
    ArrayList<FCGridBand*> FCGridBand::getBands(){
        return m_bands;
    }
    
    ArrayList<FCBandedGridColumn*> FCGridBand::getColumns(){
        return m_columns;
    }
    
    String FCGridBand::getControlType(){
        return L"FCGridBand";
    }
    
    void FCGridBand::getProperty(const String& name, String *value, String *type){
        if(name == L"allowresize"){
            *type = L"bool";
            *value = FCStr::convertBoolToStr(allowResize());
        }
        else{
            FCButton::getProperty(name, value, type);
        }
    }
    
    ArrayList<String> FCGridBand::getPropertyNames(){
        ArrayList<String> propertyNames = FCButton::getPropertyNames();
        propertyNames.add(L"AllowResize");
        return propertyNames;
    }
    
    void FCGridBand::insertBand(int index, FCGridBand *band){
        band->setGrid(m_grid);
        band->setParentBand(this);
        m_bands.insert(index, band);
        int bandSize = (int)m_bands.size();
        for(int i = 0; i < bandSize; i++){
            m_bands.get(i)->setIndex(i);
        }
        m_grid->addControl(band);
    }
    
    void FCGridBand::insertColumn(int index, FCBandedGridColumn *column){
        column->setBand(this);
        m_columns.insert(index, column);
        m_grid->addControl(column);
    }
    
    void FCGridBand::onTouchDown(FCTouchInfo touchInfo){
        FCButton::onTouchDown(touchInfo);
        FCPoint mp = touchInfo.m_firstPoint;
        if (touchInfo.m_firstTouch && touchInfo.m_clicks == 1){
            if (m_allowResize){
                ArrayList<FCGridBand*> bands;
                if (m_parentBand){
                    bands = m_parentBand->m_bands;
                }
                else{
                    bands = m_grid->m_bands;
                }
                int bandsSize = (int)bands.size();
                if (m_index > 0 && mp.x < 5){
                    m_resizeState = 1;
                    m_beginWidth = bands.get(m_index - 1)->getWidth();
                }
                else if ((!m_parentBand || m_index < bandsSize -1) && mp.x > getWidth() - 5){
                    m_resizeState = 2;
                    m_beginWidth = getWidth();
                }
                m_touchDownPoint = getNative()->getTouchPoint();
            }
        }
    }
    
    void FCGridBand::onTouchMove(FCTouchInfo touchInfo){
        FCButton::onTouchMove(touchInfo);
        FCPoint mp = touchInfo.m_firstPoint;
        if (m_allowResize){
            ArrayList<FCGridBand*> bands;
            if (m_parentBand){
                bands = m_parentBand->getBands();
            }
            else{
                bands = m_grid->getBands();
            }
            int bandsSize = (int)bands.size();
            int width = getWidth();
            if (m_resizeState > 0){
                FCPoint curPoint = getNative()->getTouchPoint();
                int newWidth = m_beginWidth + (curPoint.x - m_touchDownPoint.x);
                if (newWidth > 0){
                    if (m_resizeState == 1){
                        FCGridBand *leftBand = bands.get(m_index - 1);
                        int leftWidth = leftBand->getWidth();
                        leftBand->setWidth(newWidth);
                        width += leftWidth - newWidth;
                        setWidth(width);
                    }
                    else if (m_resizeState == 2){
                        setWidth(newWidth);
                        if(m_index < bandsSize - 1){
                            FCGridBand *rightBand = bands.get(m_index + 1);
                            int rightWidth = rightBand->getWidth();
                            rightWidth += width - newWidth;
                            rightBand->setWidth(rightWidth);
                        }
                        else{
                            if (m_grid){
                                m_grid->resetHeaderLayout();
                                m_grid->update();
                            }
                        }
                    }
                }
                if (m_grid){
                    m_grid->invalidate();
                }
            }
            else{
                if ((m_index > 0 && mp.x < 5) || ((!m_parentBand || m_index < bandsSize - 1) && mp.x > width - 5)){
                    setCursor(FCCursors_SizeWE);
                }
                else{
                    setCursor(FCCursors_Arrow);
                }
            }
        }
    }
    
    void FCGridBand::onTouchUp(FCTouchInfo touchInfo){
        if(m_resizeState != 0){
            FCButton::onTouchUp(touchInfo);
            setCursor(FCCursors_Arrow);
            m_resizeState = 0;
            if (m_grid){
                m_grid->invalidate();
            }
        }
    }
    
    void FCGridBand::removeBand(FCGridBand *band){
        for(int i = 0; i < m_bands.size(); i++){
            if(band == m_bands.get(i)){
                m_bands.removeAt(i);
                int bandSize = (int)m_bands.size();
                for(int i = 0; i < bandSize; i++){
                    m_bands.get(i)->setIndex(i);
                }
                m_grid->removeControl(band);
                break;
            }
        }
    }
    
    void FCGridBand::removeColumn(FCBandedGridColumn *column){
        for(int i = 0; i < m_columns.size(); i++){
            if(column == m_columns.get(i)){
                m_columns.removeAt(i);
                m_grid->removeColumn(column);
                break;
            }
        }
    }
    
    void FCGridBand::resetHeaderLayout(){
        int bandsSize = (int)m_bands.size();
        FCRect bounds = getBounds();
        int left = bounds.left, width = getWidth();
        if (bandsSize == 0){
            int scrollH = 0;
            FCHScrollBar *hScrollBar = getGrid()->getHScrollBar();
            if (hScrollBar && hScrollBar->isVisible()){
                scrollH = -hScrollBar->getPos();
            }
            int columnsSize = (int)m_columns.size();
            for (int i = 0; i < columnsSize; i++){
                FCBandedGridColumn *column = m_columns.get(i);
                if(column->isVisible()){
                    int columnWidth = column->getWidth();
                    if (i == columnsSize - 1 || left + columnWidth > width + bounds.left){
                        columnWidth = width + bounds.left - left;
                    }
                    FCRect cellRect ={left, bounds.bottom, left + columnWidth, bounds.bottom + column->getHeight()};
                    column->setBounds(cellRect);
                    cellRect.left -= scrollH;
                    cellRect.right -= scrollH;
                    column->setHeaderRect(cellRect);
                    left += columnWidth;
                }
            }
        }
        else{
            for (int i = 0; i < bandsSize; i++){
                FCGridBand *band = m_bands.get(i);
                if(band->isVisible()){
                    int bandWidth = band->getWidth();
                    if (i == bandsSize - 1 || left + bandWidth > width + bounds.left){
                        bandWidth = width + bounds.left - left;
                    }
                    FCRect cellRect ={left, bounds.bottom, left + bandWidth, bounds.bottom + band->getHeight()};
                    band->setBounds(cellRect);
                    band->resetHeaderLayout();
                    left += bandWidth;
                }
            }
        }
    }
    
    void FCGridBand::setProperty(const String& name, const String& value){
        if(name == L"allowresize"){
            setAllowResize(FCStr::convertStrToBool(value));
        }
        else{
            FCButton::setProperty(name, value);
        }
    }
}
