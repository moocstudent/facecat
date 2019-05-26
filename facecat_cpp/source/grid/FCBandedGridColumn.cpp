#include "..\\..\\stdafx.h"
#include "..\\..\\include\\grid\\FCBandedGridColumn.h"

namespace FaceCat{
    FCBandedGridColumn::FCBandedGridColumn(){
        m_band = 0;
    }
    
    FCBandedGridColumn::~FCBandedGridColumn(){
        m_band = 0;
    }
    
    FCGridBand* FCBandedGridColumn::getBand(){
        return m_band;
    }
    
    void FCBandedGridColumn::setBand(FCGridBand *band){
        m_band = band;
    }
    
    //////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    String FCBandedGridColumn::getControlType(){
        return L"FCBandedGridColumn";
    }
    
    bool FCBandedGridColumn::onDragBegin(){
        return m_resizeState == 0;
    }
    
    void FCBandedGridColumn::onTouchDown(FCTouchInfo touchInfo){
        callTouchEvents(FCEventID::TOUCHDOWN, touchInfo);
        if (m_band){
			FCPoint mp = touchInfo.m_firstPoint;
			if (touchInfo.m_firstTouch && touchInfo.m_clicks == 1){
                if (allowResize()){
                    ArrayList<FCBandedGridColumn*> bandColumns = m_band->m_columns;
                    int columnsSize = (int)bandColumns.size();
                    int index = -1;
                    for (int i = 0; i < columnsSize; i++){
                        if (this == bandColumns.get(i)){
                            index = i;
                            break;
                        }
                    }
                    if (index > 0 && mp.x < 5){
                        m_resizeState = 1;
                        m_beginWidth = bandColumns.get(index - 1)->getWidth();
                    }
                    else if (index < columnsSize - 1 && mp.x > getWidth() - 5){
                        m_resizeState = 2;
                        m_beginWidth = getWidth();
                    }
                    m_touchDownPoint = getNative()->getTouchPoint();
                }
            }
        }
        invalidate();
    }
    
    void FCBandedGridColumn::onTouchMove(FCTouchInfo touchInfo){
        callTouchEvents(FCEventID::TOUCHMOVE, touchInfo);
		FCPoint mp = touchInfo.m_firstPoint;
        FCGrid *grid = getGrid();
        if (m_band && grid){
            if (allowResize()){
                ArrayList<FCBandedGridColumn*> bandColumns = m_band->m_columns;
                int columnsSize = (int)bandColumns.size();
                int index = -1;
                int width = getWidth();
                for (int i = 0; i < columnsSize; i++){
                    if (this == bandColumns.get(i)){
                        index = i;
                        break;
                    }
                }
                if (m_resizeState > 0){
                    FCPoint curPoint = getNative()->getTouchPoint();
                    int newWidth = m_beginWidth + (curPoint.x - m_touchDownPoint.x);
                    if (newWidth > 0){
                        if (m_resizeState == 1){
                            FCBandedGridColumn *leftColumn = bandColumns.get(index - 1);
                            int leftWidth = leftColumn->getWidth();
                            leftColumn->setWidth(newWidth);
                            width += leftWidth - newWidth;
                            setWidth(width);
                        }
                        else if (m_resizeState == 2){
                            FCBandedGridColumn *rightColumn = bandColumns.get(index + 1);
                            int rightWidth = rightColumn->getWidth();
                            rightWidth += width - newWidth;
                            setWidth(newWidth);
                            rightColumn->setWidth(rightWidth);
                        }
                    }
                    grid->invalidate();
                    return;
                }
                else{
                    FCCursors oldCursor = getCursor();
                    FCCursors newCursor = oldCursor;
                    if ((index > 0 && mp.x < 5) || (index < columnsSize - 1 && mp.x > width - 5)){
                        newCursor = FCCursors_SizeWE;
                    }
                    else{
                        newCursor = FCCursors_Arrow;
                    }
                    if(oldCursor != newCursor){
                        setCursor(newCursor);
                        invalidate();
                    }
                }
                if(isDragging()){
                    setCursor(FCCursors_Arrow);
                }
            }
        }
    }
}