#include "..\\..\\stdafx.h"
#include "..\\..\\include\\grid\\FCBandedGrid.h"

namespace FaceCat{
    int FCBandedGrid::getAllVisibleBandsWidth(){
        int allVisibleBandsWidth = 0;
        for(int i = 0; i < m_bands.size(); i++){
            FCGridBand *band = m_bands.get(i);
            if (band->isVisible()){
                allVisibleBandsWidth += band->getWidth();
            }
        }
        return allVisibleBandsWidth;
    }
    
    //////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    FCBandedGrid::FCBandedGrid(){
    }
    
    FCBandedGrid::~FCBandedGrid(){
        clearBands();
    }
    
    //////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    void FCBandedGrid::addBand(FCGridBand *band){
        band->setGrid(this);
        m_bands.add(band);
        int bandSize = (int)m_bands.size();
        for(int i = 0; i < bandSize; i++){
            m_bands.get(i)->setIndex(i);
        }
        addControl(band);
    }
    
    void FCBandedGrid::addColumn(FCGridColumn *column){
        FCBandedGridColumn *bandedColumn = dynamic_cast<FCBandedGridColumn*>(column);
        if(bandedColumn){
            column->setGrid(this);
            m_columns.add(column);
            addControl(column);
        }
    }
    
    void FCBandedGrid::clearBands(){
        for(int i = 0; i < m_bands.size(); i++){
            FCGridBand *band = m_bands.get(i);
            removeControl(band);
            delete band;
        }
        m_bands.clear();
    }
    
    void FCBandedGrid::clearColumns(){
    }
    
    ArrayList<FCGridBand*> FCBandedGrid::getBands(){
        return m_bands;
    }
    
    int FCBandedGrid::getContentWidth(){
        FCHScrollBar *hScrollBar = getHScrollBar();
        FCVScrollBar *vScrollBar = getVScrollBar();
        int wmax = 0;
        ArrayList<FCView*> controls = getControls();
		for(int c = 0; c < controls.size(); c++){
            FCView *control = controls.get(c);
            if (control->isVisible() && control != hScrollBar && control != vScrollBar){
                int right = control->getRight();
                if (right > wmax){
                    wmax = right;
                }
            }
        }
        int allVisibleBandsWidth = getAllVisibleBandsWidth();
        return wmax > allVisibleBandsWidth ? wmax : allVisibleBandsWidth;
    }
    
    String FCBandedGrid::getControlType(){
        return L"BandedGrid";
    }
    
    void FCBandedGrid::insertBand(int index, FCGridBand *band){
        band->setGrid(this);
        m_bands.insert(index, band);
        int bandSize = (int)m_bands.size();
        for(int i = 0; i < bandSize; i++){
            m_bands.get(i)->setIndex(i);
        }
        addControl(band);
    }
    
    void FCBandedGrid::onSetEmptyClipRegion(){
        ArrayList<FCView*> controls = getControls();
        FCRect emptyClipRect = {0};
        for(int c = 0; c < controls.size(); c++){
            FCView *control = controls.get(c);
            FCScrollBar *scrollBar = dynamic_cast<FCScrollBar*>(control);
            FCGridColumn *gridColumn = dynamic_cast<FCGridColumn*>(control);
            FCGridBand *gridBand = dynamic_cast<FCGridBand*>(control);
            if (control != getEditTextBox() && !scrollBar && !gridColumn && !gridBand){
                control->setRegion(emptyClipRect);
            }
        }
    }
    
    void FCBandedGrid::removeBand(FCGridBand *band){
        for(int i = 0; i < m_bands.size(); i++){
            if(band == m_bands.get(i)){
                m_bands.removeAt(i);
                int bandSize = (int)m_bands.size();
                for(int i = 0; i < bandSize; i++){
                    m_bands.get(i)->setIndex(i);
                }
                removeControl(band);
                break;
            }
        }
    }
    
    void FCBandedGrid::removeColumn(FCGridColumn *column){
        FCBandedGridColumn *bandedColumn = dynamic_cast<FCBandedGridColumn*>(column);
        if(bandedColumn){
            for(int i = 0; i < m_columns.size(); i++){
                if(column == m_columns.get(i)){
                    m_columns.removeAt(i);
                    removeControl(column);
                    break;
                }
            }
        }
    }
    
    void FCBandedGrid::resetHeaderLayout(){
        int left = 0, top = 0, scrollH = 0;
        FCHScrollBar *hScrollBar = getHScrollBar();
        if (hScrollBar && hScrollBar->isVisible()){
            scrollH = -hScrollBar->getPos();
        }
        int headerHeight = isHeaderVisible() ? getHeaderHeight() : 0;
        int horizontalOffset = getHorizontalOffset();
        int verticalOffset = getVerticalOffset();
        for(int i = 0; i < m_bands.size(); i++){
            FCGridBand *band = m_bands.get(i);
            if (band->isVisible()){
                int bandHeight = headerHeight < band->getHeight() ? headerHeight : band->getHeight();
                FCRect bandRect = {left + horizontalOffset, top + verticalOffset,
                    left + horizontalOffset + band->getWidth(), top + bandHeight + verticalOffset};
                bool hasFrozenColumn = false;
                ArrayList<FCBandedGridColumn*> childColumns = band->getAllChildColumns();
                int childColumnsSize = (int)childColumns.size();
                for (int j = 0; j < childColumnsSize; j++){
                    if (childColumns.get(j)->isFrozen()){
                        hasFrozenColumn = true;
                        break;
                    }
                }
                if (!hasFrozenColumn){
                    bandRect.left += scrollH;
                    bandRect.right += scrollH;
                }
                band->setBounds(bandRect);
                band->resetHeaderLayout();
                left += band->getWidth();
            }
        }
    }
    
    void FCBandedGrid::update(){
        int bandsSize = (int)m_bands.size();
        int col = 0;
        for (int i = 0; i < bandsSize; i++){
            FCGridBand *band = m_bands.get(i);
            ArrayList<FCBandedGridColumn*> childColumns = band->getAllChildColumns();
            int childColumnsSize = (int)childColumns.size();
            for (int j = 0; j < childColumnsSize; j++){
                FCBandedGridColumn *column = childColumns.get(j);
                column->setIndex(col);
                col++;
            }
        }
        FCGrid::update();
    }
}