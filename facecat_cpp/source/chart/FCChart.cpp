#include "..\\..\\stdafx.h"
#include "..\\..\\include\\chart\\FCChart.h"
#include <iterator>

namespace FaceCat{
	FCChart::FCChart(){
		m_autoFillHScale = false;
		m_blankSpace = 0;
		m_canMoveShape = true;
		m_canResizeV = true;
		m_canResizeH = true;
		m_canScroll = true;
		m_canZoom = true;
		m_crossLineMoveMode = CrossLineMoveMode_FollowTouch;
		m_crossStopIndex = -1;
		m_cross_y = -1;
		m_dataSource = new FCDataTable();
		m_firstVisibleIndex = -1;
		m_hResizeType = 0;
		m_hScalePixel = 7;
		m_isScrollCross = false;
		m_lastRecordIsVisible = false;
		m_lastUnEmptyIndex = -1;
		m_lastVisibleKey = 0;
		m_lastVisibleIndex = -1;
		m_leftVScaleWidth = 80;
		m_maxVisibleRecord = 0;
		m_movingPlot = 0;
		m_movingShape = 0;
	    m_reverseHScale = false;
	    m_rightVScaleWidth = 80;
	    m_scrollAddSpeed = true;
	    m_scrollStep = 1;
	    m_showCrossLine = false;
		m_showingSelectArea = false;
	    m_showingToolTip = false;
		m_timerID = getNewTimerID();
	    m_tooltip_dely = 1;
	    m_userResizeDiv = 0;
	    m_workingAreaWidth = 0;
		m_cross_y = -1;
		m_scrollStep = 1;
		m_lastTouchMovePoint.x = 0;
		m_lastTouchMovePoint.y = 0;
		m_lastTouchClickPoint.x = 0;
		m_lastTouchClickPoint.y = 0;
		m_lastTouchMoveTime = 0;
		m_isTouchMove = false;
		m_lastTouchMoveTime = 0;
	}

	FCChart::~FCChart(){
		stopTimer(m_timerID);
		removeAll();
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	bool FCChart::autoFillHScale(){
		return m_autoFillHScale;
	}

	void FCChart::setAutoFillHScale(bool autoFillHScale){
		m_autoFillHScale = autoFillHScale;
	}

	int FCChart::getBlankSpace(){
		return m_blankSpace;
	}

	void FCChart::setBlankSpace(int blankSpace){
		m_blankSpace = blankSpace;
	}

	bool FCChart::canMoveShape(){
		return m_canMoveShape;
	}

	void FCChart::setCanMoveShape(bool canMoveShape){
		m_canMoveShape = canMoveShape;
	}

	bool FCChart::canResizeV(){
		return m_canResizeV;
	}

	void FCChart::setCanResizeV(bool canResizeV){
		m_canResizeV = canResizeV;
	}

	bool FCChart::canResizeH(){
		return m_canResizeH;
	}

	void FCChart::setCanResizeH(bool canResizeH){
		m_canResizeH = canResizeH;
	}

	bool FCChart::canScroll(){
		return m_canScroll;
	}

	void FCChart::setCanScroll(bool canScroll){
		m_canScroll = canScroll;
	}

	bool FCChart::canZoom(){
		return m_canZoom;
	}

	void FCChart::setCanZoom(bool canZoom){
		m_canZoom = canZoom;
	}

	CrossLineMoveMode FCChart::getCrossLineMoveMode(){
		return m_crossLineMoveMode;
	}

	void FCChart::setCrossLineMoveMode(CrossLineMoveMode crossLineMoveMode){
		m_crossLineMoveMode = crossLineMoveMode;
	}

	int FCChart::getCrossStopIndex(){
		return m_crossStopIndex;
	}

	void FCChart::setCrossStopIndex(int crossStopIndex){
		m_crossStopIndex = crossStopIndex;
	}

	FCDataTable* FCChart::getDataSource(){
		return m_dataSource;
	}

	void FCChart::setDataSource(FCDataTable *dataSource){
		m_dataSource = dataSource;
	}

	int FCChart::getFirstVisibleIndex(){
		return m_firstVisibleIndex;
	}

	void FCChart::setFirstVisibleIndex(int firstVisibleIndex){
		m_firstVisibleIndex = firstVisibleIndex;
	}

	String FCChart::getHScaleFieldText(){
		return m_hScaleFieldText;
	}

	void FCChart::setHScaleFieldText(const String& hScaleFieldText){
		m_hScaleFieldText = hScaleFieldText;
	}

	double FCChart::getHScalePixel(){
		return m_hScalePixel;
	}

	void FCChart::setHScalePixel(double hScalePixel){
		m_hScalePixel = hScalePixel;
		if (m_hScalePixel > 1) m_hScalePixel = (int)m_hScalePixel;
        if (m_hScalePixel > 1 && (int)m_hScalePixel % 2 == 0){
            m_hScalePixel += 1;
        }
	}

	int FCChart::getLastVisibleIndex(){
		return m_lastVisibleIndex;
	}

	void FCChart::setLastVisibleIndex(int lastVisibleIndex){
		m_lastVisibleIndex = lastVisibleIndex;
	}

	int FCChart::getLeftVScaleWidth(){
		return m_leftVScaleWidth;
	}

	void FCChart::setLeftVScaleWidth(int leftVScaleWidth){
		m_leftVScaleWidth = leftVScaleWidth;
	}

	int FCChart::getMaxVisibleRecord(){
		return m_maxVisibleRecord;
	}

	void FCChart::setMaxVisibleRecord(int maxVisibleRecord){
		m_maxVisibleRecord = maxVisibleRecord;
	}

	FCPlot* FCChart::getMovingPlot(){
		return m_movingPlot;
	}

	BaseShape* FCChart::getMovingShape(){
		return m_movingShape;
	}

	bool FCChart::isReverseHScale(){
		return m_reverseHScale;
	}

	void FCChart::setReverseHScale(bool reverseHScale){
		m_reverseHScale = reverseHScale;
	}

	int FCChart::getRightVScaleWidth(){
		return m_rightVScaleWidth;
	}

	void FCChart::setRightVScaleWidth(int rightVScaleWidth){
		m_rightVScaleWidth = rightVScaleWidth;
	}

	bool FCChart::isScrollAddSpeed(){
		return m_scrollAddSpeed;
	}

	void FCChart::setScrollAddSpeed(bool scrollAddSpeed){
		m_scrollAddSpeed = scrollAddSpeed;
	}

	BaseShape* FCChart::getSelectedShape(){
		ArrayList<ChartDiv*> divsCopy = getDivs();
		for(int d = 0; d < divsCopy.size(); d++){
			ChartDiv *div = divsCopy.get(d);
			ArrayList<BaseShape*> shapesCopy = div->getShapes(SortType_NONE);
			for(int b = 0; b < shapesCopy.size(); b++){
				BaseShape *shape = shapesCopy.get(b);
				if(shape->isSelected()){
					return shape;
				}
			}
		}
		return 0;
	}

	void FCChart::setSelectedShape(BaseShape *baseShape){
		ArrayList<ChartDiv*> divsCopy = getDivs();
        for(int d = 0; d < divsCopy.size(); d++){
			ChartDiv *div = divsCopy.get(d);
			ArrayList<BaseShape*> shapesCopy = div->getShapes(SortType_NONE);
			for(int b = 0; b < shapesCopy.size(); b++){
				BaseShape *shape = shapesCopy.get(b);
				if(shape == baseShape){
					shape->setSelected(true);
				}
				else{
					shape->setSelected(false);
				}
			}
		}
	}

	FCPlot* FCChart::getSelectedPlot(){
		ArrayList<ChartDiv*> divsCopy = getDivs();
        for(int d = 0; d < divsCopy.size(); d++){
			ChartDiv *div = divsCopy.get(d);
			ArrayList<FCPlot*> plotsCopy = div->getPlots(SortType_NONE); 
			for(int p = 0; p < plotsCopy.size(); p++){
				FCPlot *plot = plotsCopy.get(p);
				if(plot->isVisible() && plot->isSelected()){
					return plot;
				}
			}
		}
		return 0;
	}

	void FCChart::setSelectedPlot(FCPlot *selectedPlot){
		ArrayList<ChartDiv*> divsCopy = getDivs();
		for(int d = 0; d < divsCopy.size(); d++){
			ChartDiv *div = divsCopy.get(d);
			ArrayList<FCPlot*> plotsCopy = div->getPlots(SortType_NONE); 
			for(int p = 0; p < plotsCopy.size(); p++){
				FCPlot *plot = plotsCopy.get(p);
				if(plot == selectedPlot){
					plot->setSelected(true);
				}
				else{
					plot->setSelected(false);
				}
			}
		}
	}

	ChartDiv* FCChart::getSelectedDiv(){
		ArrayList<ChartDiv*> divsCopy = getDivs();
        for(int d = 0; d < divsCopy.size(); d++){
			ChartDiv *div = divsCopy.get(d);
			if(div->isSelected()){
				return div;
			}
		}
		return 0;
	}

	void FCChart::setSelectedDiv(ChartDiv *selectedDiv){
		ArrayList<ChartDiv*> divsCopy = getDivs();
        for(int d = 0; d < divsCopy.size(); d++){
			ChartDiv *div = divsCopy.get(d);
			if(div == selectedDiv){
				div->setSelected(true);
			}
			else{
				div->setSelected(false);
			}
		}
	}

	bool FCChart::isShowCrossLine(){
		return m_showCrossLine;
	}

	void FCChart::setShowCrossLine(bool showCrossLine){
		m_showCrossLine = showCrossLine;
	}

	int FCChart::getWorkingAreaWidth(){
		return m_workingAreaWidth;
	}

	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	ChartDiv* FCChart::addDiv(int vPercent){
		if(vPercent <= 0){
			return 0;
		}
		ChartDiv *cDiv = new ChartDiv;
		cDiv->setVerticalPercent(vPercent);
		cDiv->setChart(this);
		m_divs.add(cDiv);
		update();
		return cDiv;
	}

	ChartDiv* FCChart::addDiv(){
		ArrayList<ChartDiv*> divsCopy = getDivs();
		int pNum = (int)divsCopy.size() + 1;
		return addDiv(100 / pNum);
	}

	void FCChart::addPlot(FCPlot* bpl, const FCPoint& mp, ChartDiv *div){
        if (div && m_dataSource->rowsCount() >= 2){
            int rIndex = getIndex(mp);
            if (rIndex < 0 || rIndex > m_lastVisibleIndex){
                return;
            }
            if (bpl){
                bpl->setDiv(div);
                bpl->setSelected(true);
				ArrayList<FCPlot*> plots = div->getPlots(SortType_NONE);
				double *zorders = new double[plots.size()];
				int i = 0;
				for(int p = 0; p < plots.size(); p++){
					FCPlot *plot = plots.get(p);
					zorders[i] = plot->getZOrder();
					i++;
				}
                bpl->setZOrder((int)FCScript::maxValue(zorders, (int)plots.size()) + 1);
                bool flag = bpl->onCreate(mp);
                if (flag){
                    div->addPlot(bpl);
                    m_movingPlot = bpl;
                    m_movingPlot->onSelect();
                }
				delete[] zorders;
				zorders = 0;
            }
            closeSelectArea();
        }
    }

	void FCChart::adjust(){
        if (m_workingAreaWidth > 0){
			m_lastUnEmptyIndex = -1;
            if (m_firstVisibleIndex < 0 || m_lastVisibleIndex > m_dataSource->rowsCount() - 1){
                return;
            }
            ArrayList<ChartDiv*> divsCopy = getDivs();
			for (int d = 0; d < divsCopy.size(); d++){
				ChartDiv *cDiv = divsCopy.get(d);
				VScale *leftVScale = cDiv->getLeftVScale();
				VScale *rightVScale = cDiv->getRightVScale();
				cDiv->setWorkingAreaHeight(cDiv->getHeight() - cDiv->getHScale()->getHeight() - cDiv->getTitleBar()->getHeight() - 1);
                ArrayList<BaseShape*> shapesCopy = cDiv->getShapes(SortType_NONE);
                double leftMax = 0, leftMin = 0, rightMax = 0, rightMin = 0;
				bool leftMaxInit = false, leftMinInit = false, rightMaxInit = false, rightMinInit = false;
                if (m_dataSource->rowsCount() > 0){
					for(int b = 0; b < shapesCopy.size(); b++){
						BaseShape *bs = shapesCopy.get(b);
						if(!bs->isVisible()){
							continue;
						}
                        BarShape *bar = dynamic_cast<BarShape*>(bs);
						int fieldsLength = 0;
                        int *fields = bs->getFields(&fieldsLength);
						for (int f = 0; f < fieldsLength; f++){
							int field = m_dataSource->getColumnIndex(fields[f]);
							for (int m = m_firstVisibleIndex; m <= m_lastVisibleIndex; m++){
								double fieldValue = m_dataSource->get3(m, field);
								if (!FCDataTable::isNaN(fieldValue)){
									m_lastUnEmptyIndex = m;
									if (bs->getAttachVScale() == AttachVScale_Left){
										if (fieldValue > leftMax || !leftMaxInit){
											leftMaxInit = true;
											leftMax = fieldValue;
										}
										if (fieldValue < leftMin || !leftMinInit){
											leftMinInit = true;
											leftMin = fieldValue;
										}
									}
									else{
										if (fieldValue > rightMax || !rightMaxInit){
											rightMaxInit = true;
											rightMax = fieldValue;
										}
										if (fieldValue < rightMin || !rightMinInit){
											rightMinInit = true;
											rightMin = fieldValue;
										}
									}
								}
							}
                        }
						if (bar){
                            double midValue = 0;
							if(bar->getFieldName2() == FCDataTable::NULLFIELD()){
								if (bs->getAttachVScale() == AttachVScale_Left){
									if (midValue > leftMax || !leftMaxInit){
										leftMaxInit = true;
										leftMax = midValue;
									}
									if (midValue < leftMin || !leftMinInit){
										leftMinInit = true;
										leftMin = midValue;
									}
								}
								else{
									if (midValue > rightMax|| !rightMaxInit){
										rightMaxInit = true;
										rightMax = midValue;
									}
									if (midValue < rightMin || !rightMinInit){
										rightMinInit = true;
										rightMin = midValue;
									}
								}
							}
                        }
						if(fields){
							delete[] fields;
							fields = 0;
						}
                    }
					if(leftMax == leftMin){
						leftMax = leftMax * 1.01;
						leftMin = leftMin * 0.99;
					}
					if(rightMax == rightMin){
						rightMax = rightMax * 1.01;
						rightMin = rightMin * 0.99;
					}
                }
				if(leftVScale->autoMaxMin()){
					leftVScale->setVisibleMax(leftMax);
					leftVScale->setVisibleMin(leftMin);
				}
				if(rightVScale->autoMaxMin()){
					rightVScale->setVisibleMax(rightMax);
					rightVScale->setVisibleMin(rightMin);
				}
				if (leftVScale->autoMaxMin() && leftVScale->getVisibleMax() == 0 && leftVScale->getVisibleMin() == 0){
					leftVScale->setVisibleMax(rightVScale->getVisibleMax());
					leftVScale->setVisibleMin(rightVScale->getVisibleMin());
				}
				if (rightVScale->autoMaxMin() && rightVScale->getVisibleMax() == 0 && rightVScale->getVisibleMin() == 0){
					rightVScale->setVisibleMax(leftVScale->getVisibleMax());
					rightVScale->setVisibleMin(leftVScale->getVisibleMin());
				}
            }
        }
    }

	void FCChart::changeChart(ScrollType scrollType, int limitStep){
        ArrayList<ChartDiv*> divsCopy = getDivs();
        if (divsCopy.size() == 0 || m_dataSource->rowsCount() == 0){
            return;
        }
        int fIndex = m_firstVisibleIndex;
        int lIndex = m_lastVisibleIndex;
        double axis = m_hScalePixel;
        bool flag = false;
        bool locateCrossHairFlag = false;
        switch (scrollType){
            case ScrollType_Left:
                if (m_canScroll){
                    flag = true;
                    if (m_showCrossLine){
                        if (limitStep > m_scrollStep){
                            scrollCrossLineLeft(limitStep);
                        }
                        else{
                            scrollCrossLineLeft(m_scrollStep);
                        }
                        locateCrossHairFlag = true;
                    }
                    else{
                        if (limitStep > m_scrollStep){
                            scrollLeft(limitStep);
                        }
                        else{
                            scrollLeft(m_scrollStep);
                        }
                    }
                }
                break;
            case ScrollType_Right:
                if (m_canScroll){
                    flag = true;
                    if (m_showCrossLine){
                        if (limitStep > m_scrollStep){
                            scrollCrossLineRight(limitStep);
                        }
                        else{
                            scrollCrossLineRight(m_scrollStep);
                        }
                        locateCrossHairFlag = true;
                    }
                    else{
                        if (limitStep > m_scrollStep){
                            scrollRight(limitStep);
                        }
                        else{
                            scrollRight(m_scrollStep);
                        }
                    }
                }
                break;
            case ScrollType_ZoomIn:
                if (m_canZoom){
                    flag = true;
                    zoomIn();
                }
                break;
            case ScrollType_ZoomOut:
                if (m_canZoom){
                    flag = true;
                    zoomOut();
                }
                break;
        }
        if (flag){
            int fIndex_after = m_firstVisibleIndex;
            int lIndex_after = m_lastVisibleIndex;
            double axis_after = m_hScalePixel;
            correctVisibleRecord(m_dataSource->rowsCount(), &m_firstVisibleIndex, &m_lastVisibleIndex);
            if (fIndex != fIndex_after || lIndex != lIndex_after){
                adjust();
            }
            resetCrossOverIndex();
            if (locateCrossHairFlag){
                locateCrossLine();
            }
            if (fIndex == fIndex_after && lIndex == lIndex_after && axis == axis_after){
				invalidate();
            }
            else{
				update();
				invalidate();
            }
        }
        if (m_scrollAddSpeed && (scrollType == ScrollType_Left || scrollType == ScrollType_Right)){
            if (m_scrollStep < 50){
                m_scrollStep += 5;
            }
        }
        else{
            m_scrollStep = 1;
        }
    }

	void FCChart::checkLastVisibleIndex(){
        if (m_lastVisibleIndex > m_dataSource->rowsCount() - 1){
            m_lastVisibleIndex = m_dataSource->rowsCount() - 1;
        }
        if (m_dataSource->rowsCount() > 0){
            m_lastVisibleKey = m_dataSource->getXValue(m_lastVisibleIndex);
            if (m_lastVisibleIndex == m_dataSource->rowsCount() - 1){
                m_lastRecordIsVisible = true;
            }
            else{
                m_lastRecordIsVisible = false;
            }
        }
        else{
            m_lastVisibleKey = 0;
            m_lastRecordIsVisible = true;
        }
	}

	void FCChart::checkToolTip(){
		SYSTEMTIME t; 
		GetLocalTime(&t); 
		double nowTime = FCStr::getDateNum(t.wYear, t.wMonth, t.wDay, t.wHour, t.wMinute, t.wSecond, t.wMilliseconds);
		if (m_lastTouchMoveTime + m_tooltip_dely <= nowTime){
			if (m_isTouchMove){
				bool show = true;
				if (isOperating()){
					show = false;
				}
				ArrayList<ChartDiv*> divsCopy = getDivs();
				for(int d = 0; d < divsCopy.size(); d++){
					ChartDiv *div = divsCopy.get(d);
					if (div->getSelectArea()->isVisible()){
						show = false;
					}
				}
				if (show){
					int curRecord = getTouchOverIndex();
					BaseShape *bs = selectShape(curRecord, 0);
					if (bs){
						m_showingToolTip = true;
						invalidate();
					}
				}
				m_isTouchMove = false;
			}
		}
    }

	void FCChart::clear(){
		clearSelectedShape();
		clearSelectedPlot();
        ArrayList<ChartDiv*> divsCopy = getDivs();
		for(int d = 0; d < divsCopy.size(); d++){
			ChartDiv *div = divsCopy.get(d);
			ArrayList<FCPlot*> plotsCopy = div->getPlots(SortType_NONE); 
			for(int p = 0; p < plotsCopy.size(); p++){
				FCPlot *plot = plotsCopy.get(p);
				div->removePlot(plot);
				delete plot;
			}
        }
        closeSelectArea();
        m_dataSource->clear();
        m_firstVisibleIndex = -1;
        m_lastVisibleIndex = -1;
        m_lastRecordIsVisible = true;
        m_lastVisibleIndex = 0;
        m_lastVisibleKey = 0;
        m_showCrossLine = false;
    }

    void FCChart::clearSelectedShape(){
        ArrayList<ChartDiv*> divsCopy = getDivs();
		for(int d = 0; d < divsCopy.size(); d++){
			ChartDiv *div = divsCopy.get(d);
			ArrayList<BaseShape*> shapesCopy = div->getShapes(SortType_NONE);
			for(int b = 0; b < shapesCopy.size(); b++){
				BaseShape *bs = shapesCopy.get(b);
				bs->setSelected(false);
			}
		}
		m_movingShape = 0;
	}

	void FCChart::clearSelectedPlot(){
        ArrayList<ChartDiv*> divsCopy = getDivs();
		for(int d = 0; d < divsCopy.size(); d++){
			ChartDiv *div = divsCopy.get(d);
			ArrayList<FCPlot*> plotsCopy = div->getPlots(SortType_NONE);
			for(int p = 0; p < plotsCopy.size(); p++){
				FCPlot *plot = plotsCopy.get(p);
                plot->setSelected(false);
            }
         }
		m_movingPlot = 0;
    }

	void FCChart::clearSelectedDiv(){
	    ArrayList<ChartDiv*> divsCopy = getDivs();
		for(int d = 0; d < divsCopy.size(); d++){
			ChartDiv *div = divsCopy.get(d);
			div->setSelected(false);
	    }
	}

	void FCChart::closeSelectArea(){
		ArrayList<ChartDiv*> divsCopy = getDivs();
		for(int d = 0; d < divsCopy.size(); d++){
			ChartDiv *cDiv = divsCopy.get(d);
			cDiv->getSelectArea()->close();
		}
		m_showingSelectArea = false;
	}

	ChartDiv* FCChart::findDiv(const FCPoint& mp){
        ArrayList<ChartDiv*> divsCopy = getDivs();
		for(int d = 0; d < divsCopy.size(); d++){
			ChartDiv *cDiv = divsCopy.get(d);
            if (mp.y >= cDiv->getTop() && mp.y <= cDiv->getTop() + cDiv->getHeight()){
                return cDiv;
            }
        }
        return 0;
    }

	ChartDiv* FCChart::findDiv(BaseShape *shape){
		ArrayList<ChartDiv*> divsCopy = getDivs();
		for(int d = 0; d < divsCopy.size(); d++){
			ChartDiv *div = divsCopy.get(d);
			ArrayList<BaseShape*> shapesCopy = div->getShapes(SortType_NONE);
			for(int b = 0; b < shapesCopy.size(); b++){
				BaseShape *bs = shapesCopy.get(b);
				if(bs == shape){
					return div;
				}
			}
		}
		return 0;
	}

	String FCChart::getControlType(){
		return L"Chart";
	}
	
	void FCChart::getHScaleDateString(double date, double lDate, DateType *dateType, wchar_t *str){
		int year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0, msecond = 0, lyear = 0, lmonth = 0, lday = 0, lhour = 0, lminute = 0, lsecond = 0, lmsecond = 0;	    
		FCStr::getDateByNum(date, &year, &month, &day, &hour, &minute, &second, &msecond);
		FCStr::getDateByNum(lDate, &lyear, &lmonth, &lday, &lhour, &lminute, &lsecond, &lmsecond);
        if (year > lyear){
            *dateType = DateType_Year;
			_stprintf_s(str, 19, L"%d", year);
			return;
        }
        if (month > lmonth){
            *dateType = DateType_Month;;
			_stprintf_s(str, 19, L"%d", month);
			return;
        }
        if (day > lday){
            *dateType = DateType_Day;
 			_stprintf_s(str, 19, L"%d", day);
			return;
        }
        if (hour > lhour || minute > lminute){
            *dateType = DateType_Minute;
			_stprintf_s(str, 19, L"%d:%02d", hour, minute);
			return;
        }
        if (second > lsecond){
            *dateType = DateType_Second;;
			_stprintf_s(str, 19, L"%d", second);
			return;
        }
        if (msecond > lmsecond){
            *dateType = DateType_Millisecond;
			_stprintf_s(str, 19, L"%d", msecond);
        }
    }

	int FCChart::getIndex(const FCPoint& mp){
		int x = mp.x;
	    if (m_reverseHScale){
            x = m_workingAreaWidth - (x - m_leftVScaleWidth) + m_leftVScaleWidth;
        }
        double pixel = m_hScalePixel;
		int index = getChartIndex(x, m_leftVScaleWidth, pixel, m_firstVisibleIndex);
        if (index < 0){
            index = 0;
        }
        if (index > m_lastVisibleIndex){
            index = m_lastVisibleIndex;
        }
        return index;
	}

	int FCChart::getMaxVisibleCount(double hScalePixel, int pureH){
		return (int)(pureH / hScalePixel);
	}

	ChartDiv* FCChart::getTouchOverDiv(){
		FCPoint mp = getTouchPoint();
		ArrayList<ChartDiv*> divsCopy = getDivs();
		for(int d = 0; d < divsCopy.size(); d++){
			ChartDiv *cDiv = divsCopy.get(d);
			if (mp.y >= cDiv->getTop() && mp.y <= cDiv->getTop() + cDiv->getHeight()){
                return cDiv;
            }
		}
		return 0;
	}

	int FCChart::getTouchOverIndex(){
	    FCPoint mp = getTouchPoint();
        if (m_reverseHScale){
            mp.x = m_workingAreaWidth - (mp.x - m_leftVScaleWidth) + m_leftVScaleWidth;
        }
        double pixel = m_hScalePixel;
		return getChartIndex(mp.x, m_leftVScaleWidth, pixel, m_firstVisibleIndex);
	}

	double FCChart::getNumberValue(ChartDiv *div,const FCPoint& mp,AttachVScale attachVScale){
		VScale *vScale = div->getVScale(attachVScale);
		VScale *leftVScale = div->getLeftVScale();
		VScale *rightVScale = div->getRightVScale();
        int vHeight = div->getWorkingAreaHeight() - vScale->getPaddingTop() - vScale->getPaddingBottom();
        int cY = mp.y - div->getTop() - div->getTitleBar()->getHeight() - vScale->getPaddingTop();
        if (vScale->isReverse()){
            cY = vHeight - cY;
        }
        if (vHeight > 0){
            double max = 0;
            double min = 0;
            bool isLog = false;
            if (attachVScale == AttachVScale_Left){
                max = leftVScale->getVisibleMax();
                min = leftVScale->getVisibleMin();
                if (max == 0 && min == 0){
                    max = rightVScale->getVisibleMax();
                    min = rightVScale->getVisibleMin();
                }
                isLog = leftVScale->getSystem() == VScaleSystem_Logarithmic;
            }
            else if (attachVScale == AttachVScale_Right){
                max = rightVScale->getVisibleMax();
                min = rightVScale->getVisibleMin();
                if (max == 0 && min == 0){
                    max = leftVScale->getVisibleMax();
                    min = leftVScale->getVisibleMin();
                }
                isLog = rightVScale->getSystem() == VScaleSystem_Logarithmic;
            }
            if (isLog){
                if (max >= 0){
                    max = log10(max);
                }
                else{
                    max = -log10(abs(max));
                }
                if (min >= 0){
                    min = log10(min);
                }
                else{
                    min = -log10(abs(min));
                }
				double value  = getVScaleValue(cY, max, min, (float)vHeight);
				return pow(10, value);
            }
            else{
				return getVScaleValue(cY, max, min, (float)vHeight);
            }
        }
        return 0;
	}

	void FCChart::getProperty(const String& name, String *value, String *type){
		if(name == L"autofillhscale"){
			*type = L"bool";
			*value = FCStr::convertBoolToStr(autoFillHScale());
		}
		else if(name == L"blankspace"){
			*type = L"int";
			*value = FCStr::convertIntToStr(getBlankSpace());
		}
		else if(name == L"canmoveshape"){
			*type = L"bool";
			*value = FCStr::convertBoolToStr(canMoveShape());
		}
		else if(name == L"canresizeh"){
			*type = L"bool";
			*value = FCStr::convertBoolToStr(canResizeH());
		}
		else if(name == L"canresizev"){
			*type = L"bool";
			*value = FCStr::convertBoolToStr(canResizeV());
		}
		else if(name == L"canscroll"){
			*type = L"bool";
			*value = FCStr::convertBoolToStr(canScroll());
		}
		else if(name == L"canzoom"){
			*type = L"bool";
			*value = FCStr::convertBoolToStr(canZoom());
		}
		else if(name == L"crosslinemovemode"){
			*type = L"enum:CrossLineMoveMode";
			CrossLineMoveMode crossLineMoveMode = getCrossLineMoveMode();
			if(crossLineMoveMode == CrossLineMoveMode_AfterClick){
				*value = L"AfterClick";
			}
			else{
				*value = L"FollowTouch";
			}
		}
		else if(name == L"hscalefieldtext"){
			*type = L"text";
			*value = getHScaleFieldText();
		}
		else if(name == L"hscalepixel"){
			*type = L"double";
			*value = FCStr::convertDoubleToStr(getHScalePixel());
		}
		else if(name == L"leftvscalewidth"){
			*type = L"int";
			*value = FCStr::convertIntToStr(getLeftVScaleWidth());
		}
		else if(name == L"reversehscale"){
			*type = L"bool";
			*value = FCStr::convertBoolToStr(isReverseHScale());
		}
		else if(name == L"rightvscalewidth"){
			*type = L"int";
			*value = FCStr::convertIntToStr(getRightVScaleWidth());
		}
		else if(name == L"scrolladdspeed"){
			*type = L"bool";
			*value = FCStr::convertBoolToStr(isScrollAddSpeed());
		}
		else if(name == L"showcrossline"){
			*type = L"bool";
			*value = FCStr::convertBoolToStr(isShowCrossLine());
		}
		else{
			FCView::getProperty(name, value, type);
		}
	}

	ArrayList<String> FCChart::getPropertyNames(){
		ArrayList<String> propertyNames = FCView::getPropertyNames();
		propertyNames.add(L"AutoFillHScale");
		propertyNames.add(L"BlankSpace");
		propertyNames.add(L"CanMoveShape");
		propertyNames.add(L"CanResizeH");
		propertyNames.add(L"CanResizeV");
		propertyNames.add(L"CanScroll");
		propertyNames.add(L"CanZoom");
		propertyNames.add(L"CrossLineMoveMode");
		propertyNames.add(L"HScaleFieldText");
		propertyNames.add(L"HScalePixel");
		propertyNames.add(L"LeftVScaleWidth");
		propertyNames.add(L"ReverseHScale");
		propertyNames.add(L"RightVScaleWidth");
		propertyNames.add(L"ScrollAddSpeed");
		propertyNames.add(L"ShowCrossLine");
		return propertyNames;
	}

	int FCChart::getShapesCount(int field){
		int count=0;
		ArrayList<ChartDiv*> divsCopy = getDivs();
		for(int d = 0; d < divsCopy.size(); d++){
			ChartDiv *cDiv = divsCopy.get(d);
			ArrayList<BaseShape*> shapesCopy = cDiv->getShapes(SortType_NONE);
			for(int b = 0; b < shapesCopy.size(); b++){
				BaseShape *shape = shapesCopy.get(b);
				int length = 0;
				int *fields = shape->getFields(&length);
				for(int f = 0; f < length; f++){
					if(fields[f] == field){
						count++;
						break;
					}
				}
				if(fields){
					delete[] fields;
					fields = 0;
				}
			}
		}
		return count;
	}

	float FCChart::getX(int index){
	    float x = 0;
        x = (float)(m_leftVScaleWidth + (index - m_firstVisibleIndex) * m_hScalePixel + m_hScalePixel / 2 + 1);
        if (m_reverseHScale){
            return m_workingAreaWidth - (x - m_leftVScaleWidth) + m_leftVScaleWidth + m_blankSpace;
        }
        else{
            return x;
        }
	}

	float FCChart::getY(ChartDiv *div, double value, AttachVScale attach){
		if (div){
            VScale *scale = div->getVScale(attach);
            double max = scale->getVisibleMax();
            double min = scale->getVisibleMin();
            if (scale->getSystem() == VScaleSystem_Logarithmic){
                if (value > 0){
                    value = log10(value);
                }
                else if(value < 0){
                    value = -log10(abs(value));
                }
                if (max > 0){
                    max = log10(max);
                }
                else if(max < 0){
                    max = -log10(abs(max));
                }
                if (min > 0){
                    min = log10(min);
                }
                else if(min < 0){
                    min = -log10(abs(min));
                }
            }

            if (max != min){
                int wHeight = div->getWorkingAreaHeight() - scale->getPaddingTop() - scale->getPaddingBottom();
				ChartTitleBar *titleBar = div->getTitleBar();
                if (wHeight > 0){
					float y = (float)((max - value) / (max - min) * wHeight);
                    if (scale->isReverse()){

						return titleBar->getHeight() + div->getWorkingAreaHeight() - scale->getPaddingBottom() - y;
                    }
                    else{
						return titleBar->getHeight() + scale->getPaddingTop() + y;
                    }
                }
            }
        }
        return 0;
	}

	int FCChart::getVScaleBaseField(ChartDiv *div, VScale *vScale){
		int baseField = vScale->getBaseField();
		if (baseField == FCDataTable::NULLFIELD()){
			ArrayList<BaseShape*> shapesCopy = div->getShapes(SortType_DESC);
			for(int b = 0; b < shapesCopy.size(); b++){
				BaseShape *shape = shapesCopy.get(b);
				baseField = shape->getBaseField();
				break;
			}
		}
		return baseField;
	}

	int FCChart::gridScale(double min, double max, int yLen, int maxSpan, int minSpan, int defCount, double *step, int *digit){
        double sub = max - min;
		int nMinCount = (int)ceil((double)yLen / maxSpan);
		int nMaxCount = (int)floor((double)yLen / minSpan);
		int nCount = defCount;
		double logStep = sub / nCount;
		BOOLEAN start = FALSE;
		double divisor = 0;
		int i = 0, nTemp = 0;
		*step = 0;
		*digit = 0;
		nCount = max(nMinCount, nCount);
		nCount = min(nMaxCount, nCount);
		nCount = max(nCount, 1);
		for (i = 15; i >= -6; i--)
		{
			divisor = pow(10.0, (double)i); 
			if (divisor < 1) 
			{
				*digit++;
			}
			nTemp = (int)floor(logStep / divisor);
			if (start)
			{
				if (nTemp < 4)
				{
					if (*digit > 0)
					{
						*digit--;
					}
				}
				else if (nTemp >= 4 && nTemp <= 6)
				{
					nTemp = 5;
					*step += nTemp * divisor;
				}
				else 
				{
					*step += 10 * divisor;
					if (*digit > 0)
					{
						*digit--;
					}
				}
				break;
			}
			else if (nTemp > 0)
			{
				*step = nTemp * divisor + *step;
				logStep -= *step;
				start = TRUE;
			}
		}
		return 0;
    }

	double FCChart::getVScaleBaseValue(ChartDiv *div, VScale *vScale, int i){
		double baseValue = 0;
		int baseField = getVScaleBaseField(div, vScale);
        if (baseField != FCDataTable::NULLFIELD() && m_dataSource->rowsCount() > 0){
            if (i >= m_firstVisibleIndex && i <= m_lastVisibleIndex){
                double value = m_dataSource->get2(i, baseField);
				if (!FCDataTable::isNaN(value)){
                    baseValue = value;
                }
            }
        }
		else{
			baseValue = vScale->getMidValue();
		}
        return baseValue;
	}

	bool FCChart::isOperating(){
	    if (m_movingPlot || m_movingShape
        || m_hResizeType != 0 || m_userResizeDiv){
            return true;
        }
        return false;
	}

	void FCChart::locateCrossLine(){
		if (m_dataSource->rowsCount() > 0){
            ArrayList<ChartDiv*> divsCopy = getDivs();
			for(int d = 0; d < divsCopy.size(); d++){
				ChartDiv *div = divsCopy.get(d);
                if (m_cross_y >= div->getTop() && m_cross_y <= div->getTop() + div->getHeight()){
                    if (div->getWorkingAreaHeight() > 0 && m_crossStopIndex >= 0 && m_crossStopIndex < m_dataSource->rowsCount()){
						ArrayList<BaseShape*> shapesCopy = div->getShapes(SortType_DESC);
						for(int b = 0; b < shapesCopy.size(); b++){
							BaseShape *tls = shapesCopy.get(b);
                            if (tls->isVisible()){
                                double value = m_dataSource->get2(m_crossStopIndex, tls->getBaseField());
                                if (!FCDataTable::isNaN(value)){
                                    m_cross_y = (int)getY(div, value, tls->getAttachVScale()) + div->getTop();
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }
	}

	void FCChart::moveShape(ChartDiv* cDiv, BaseShape *shape){
		ArrayList<ChartDiv*> divsCopy = getDivs();
		for(int d = 0; d < divsCopy.size(); d++){
			ChartDiv *div = divsCopy.get(d);
			div->removeShape(shape);
		}
		if(cDiv){
			cDiv->addShape(shape);
		}
	}

	double FCChart::divMaxOrMin(int index, ChartDiv *div, int flag){
		if (index < 0){
            return 0;
        }
        if (div){
			ArrayList<BaseShape*> shapesCopy = div->getShapes(SortType_NONE);
			double max = 0, min = 0;
			bool isEmpty = true;
			for(int b = 0; b < shapesCopy.size(); b++){
				BaseShape *bs = shapesCopy.get(b);
                if (!bs->isVisible()){
                    continue;
                }
				int fieldsLength = 0;
                int *fields = bs->getFields(&fieldsLength);
                for (int i = 0; i < fieldsLength; i++){
                    double value = m_dataSource->get2(index, fields[i]);
                    if (!FCDataTable::isNaN(value)){
						if(isEmpty){
							max = value;
							min = value;
							isEmpty = false;
						}
						else{
							if(flag == 0 && max < value){
								max = value;
							}
							if(flag == 1 && min > value){
								min = value;
							}
						}
                    }
                }
				if(fields){
					delete fields;
					fields = 0;
				}
            }
            if (flag == 0){
                return max;
            }
            else{
                return min;
            }
        }
        return 0;
    }

	int FCChart::getDateType(DateType dateType){
		switch(dateType){
		case DateType_Year:
			return 0;
		case DateType_Month:
			return 1;
		case DateType_Day:
			return 2;
		case DateType_Hour:
			return 3;
		case DateType_Minute:
			return 4;
		case DateType_Second:
			return 5;
		case DateType_Millisecond:
			return 6;
		}
		return 3;
	}

	DateType FCChart::getDateType(int dateType){
		switch(dateType){
		case 0:
			return DateType_Year;
		case 1:
			return DateType_Month;
		case 2:
			return DateType_Day;
		case 3:
			return DateType_Hour;
		case 4:
			return DateType_Minute;
		case 5:
			return DateType_Second;
		case 6:
			return DateType_Millisecond;
		}
		return DateType_Day;
	}

	ArrayList<ChartDiv*> FCChart::getDivs(){
		return m_divs;
	}

	void FCChart::resetCrossOverIndex(){
        if (m_showCrossLine){
            m_crossStopIndex = resetCrossOverIndex(m_dataSource->rowsCount(), m_maxVisibleRecord,
                m_crossStopIndex, m_firstVisibleIndex, m_lastVisibleIndex);
        }
        m_isScrollCross = true;
    }

	void FCChart::removeAll(){
		clear();
		clearSelectedDiv();
		for(int d = 0; d < m_divs.size(); d++){
			ChartDiv *div = m_divs.get(d);
			delete div;
		}
		m_divs.clear();
        m_dataSource->clear();
        m_cross_y = -1;
        m_showingToolTip = false;
	}

	bool FCChart::resizeDiv(){
		int width = getWidth(), height = getHeight();
        if (m_hResizeType > 0){
            FCPoint mp = getTouchPoint();
            if (m_hResizeType == 1){
                if (mp.x > 0 && mp.x < width - m_rightVScaleWidth - 50){
                    m_leftVScaleWidth = mp.x;
                }
            }
            else if (m_hResizeType == 2){
                if (mp.x > m_leftVScaleWidth + 50 && mp.x < width){
                    m_rightVScaleWidth = width - mp.x;
                }
            }
            m_hResizeType = 0;
            update();
            return true;
        }
        if (m_userResizeDiv){
            FCPoint mp = getTouchPoint();
            ChartDiv *nextCP = 0;
            bool rightP = false;
			ArrayList<ChartDiv*> divsCopy = getDivs();
			for(int d = 0; d < divsCopy.size(); d++){
				ChartDiv *cDiv = divsCopy.get(d);
                if (rightP){
                    nextCP = cDiv;
                    break;
                }
                if (cDiv == m_userResizeDiv){
                    rightP = true;
                }
            }
            int sumPercent = 0;
			for(int cd = 0; cd < divsCopy.size(); cd++){
				ChartDiv *div = divsCopy.get(cd);
                sumPercent += div->getVerticalPercent();
            }
            int originalVP = m_userResizeDiv->getVerticalPercent();
			FCRect uRect = m_userResizeDiv->getBounds();
            if (mp.x >= uRect.left && mp.x <= uRect.right && mp.y >= uRect.top && mp.y <= uRect.bottom){
                m_userResizeDiv->setVerticalPercent(sumPercent * (mp.y - m_userResizeDiv->getTop()) / height);
                if (m_userResizeDiv->getVerticalPercent() < 1){
                    m_userResizeDiv->setVerticalPercent(1);
                }
                if (nextCP){
					nextCP->setVerticalPercent(nextCP->getVerticalPercent()+originalVP - m_userResizeDiv->getVerticalPercent());
                }
            }
            else{
				if(nextCP){
					FCRect nRect = nextCP->getBounds();
					if (nextCP && mp.x >= nRect.left && mp.x <= nRect.right && mp.y >= nRect.top && mp.y <= nRect.bottom){
						m_userResizeDiv->setVerticalPercent(sumPercent * (mp.y - m_userResizeDiv->getTop()) / height);
						if (m_userResizeDiv->getVerticalPercent() >= originalVP + nextCP->getVerticalPercent()){
							m_userResizeDiv->setVerticalPercent(m_userResizeDiv->getVerticalPercent()-1);
						}
						nextCP->setVerticalPercent(originalVP + nextCP->getVerticalPercent() - m_userResizeDiv->getVerticalPercent());
					}
				}
            }
            m_userResizeDiv = 0;
            update();
            return true;
        }
        return false;
	}

	void FCChart::removeDiv(ChartDiv *div){
		for(int d = 0; d < m_divs.size(); d++){
			ChartDiv *cDiv = m_divs.get(d);
			if(cDiv == div){
				m_divs.removeAt(d);
				break;
			}
	    }
        update();
    }

	void FCChart::reset(){
		if(isVisible()){
			resetVisibleRecord();
			adjust();
			resetCrossOverIndex();
		}
	}

	void FCChart::resetVisibleRecord(){
		ArrayList<ChartDiv*> divs = getDivs();
        if (divs.size() > 0){
			int rowsCount = m_dataSource->rowsCount();
            if (m_autoFillHScale){
                if (m_workingAreaWidth > 0 && rowsCount > 0){
                    m_hScalePixel = (double)m_workingAreaWidth / rowsCount;
                    m_maxVisibleRecord = rowsCount;
                    m_firstVisibleIndex = 0;
                    m_lastVisibleIndex = rowsCount - 1;
                }
            }
			else{
				m_maxVisibleRecord = getMaxVisibleCount(m_hScalePixel, m_workingAreaWidth);
				if (rowsCount == 0){
					m_firstVisibleIndex = -1;
					m_lastVisibleIndex = -1;
				}
				else{
					if (rowsCount < m_maxVisibleRecord){
						m_lastVisibleIndex = rowsCount - 1;
						m_firstVisibleIndex = 0;
					}
					else{
						if (m_firstVisibleIndex != -1 && m_lastVisibleIndex != -1 && !m_lastRecordIsVisible){
							int index = m_dataSource->getRowIndex(m_lastVisibleKey);
							if (index != -1){
								m_lastVisibleIndex = index;
							}
							m_firstVisibleIndex = m_lastVisibleIndex - m_maxVisibleRecord + 1;
							if (m_firstVisibleIndex < 0){
								m_firstVisibleIndex = 0;
								m_lastVisibleIndex = m_firstVisibleIndex + m_maxVisibleRecord;
								checkLastVisibleIndex();
							}
						}
						else{
							m_lastVisibleIndex = rowsCount - 1;
							m_firstVisibleIndex = m_lastVisibleIndex - m_maxVisibleRecord + 1;
							if (m_firstVisibleIndex > m_lastVisibleIndex){
								m_firstVisibleIndex = m_lastVisibleIndex;
							}
						}
					}
				}
			}
        }
    }

	void FCChart::scrollLeft(int step){
		if(!m_autoFillHScale){
			scrollLeft(step, m_dataSource->rowsCount(), m_hScalePixel, m_workingAreaWidth,
				&m_firstVisibleIndex, &m_lastVisibleIndex);
			checkLastVisibleIndex();
		}
	}

	void FCChart::scrollLeftToBegin(){
		if (!m_autoFillHScale && m_dataSource->rowsCount() > 0){
            m_firstVisibleIndex = 0;
            m_lastVisibleIndex = m_maxVisibleRecord - 1;
            checkLastVisibleIndex();
            m_crossStopIndex = m_firstVisibleIndex;
        }
	}

	void FCChart::scrollRight(int step){
		if(!m_autoFillHScale){
			scrollRight(step, m_dataSource->rowsCount(), m_hScalePixel, m_workingAreaWidth,
				&m_firstVisibleIndex, &m_lastVisibleIndex);
			checkLastVisibleIndex();
		}
	}

	void FCChart::scrollRightToEnd(){
	    if (!m_autoFillHScale && m_dataSource->rowsCount() > 0){
            m_lastVisibleIndex = m_dataSource->rowsCount() - 1;
            checkLastVisibleIndex();
            m_firstVisibleIndex = m_lastVisibleIndex - m_maxVisibleRecord + 1;
            if (m_firstVisibleIndex < 0){
                m_firstVisibleIndex = 0;
            }
            m_crossStopIndex = m_lastVisibleIndex;
        }
	}

	void FCChart::scrollCrossLineLeft(int step){
        int currentIndex = m_crossStopIndex;
        m_crossStopIndex = currentIndex - step;
        if (m_crossStopIndex < 0){
            m_crossStopIndex = 0;
        }
        if (currentIndex <= m_firstVisibleIndex){
            scrollLeft(step);
        }
    }

	void FCChart::scrollCrossLineRight(int step){
        int currentIndex = m_crossStopIndex;
        m_crossStopIndex = currentIndex + step;
        if (m_dataSource->rowsCount() < m_maxVisibleRecord){
            if (m_crossStopIndex >= m_maxVisibleRecord - 1){
                m_crossStopIndex = m_maxVisibleRecord - 1;
            }
        }
        if (currentIndex >= m_lastVisibleIndex){
            scrollRight(step);
        }
    }

	BaseShape* FCChart::selectShape(int curIndex, int state){
        BaseShape *selectObj = 0;
		FCPoint mp = getTouchPoint();
        ArrayList<ChartDiv*> divsCopy = getDivs();
		for(int d = 0; d < divsCopy.size(); d++){
			ChartDiv *div = divsCopy.get(d);
			ArrayList<BaseShape*> shapesCopy = div->getShapes(SortType_DESC);
			for(int b = 0; b < shapesCopy.size(); b++){
				BaseShape *bShape = shapesCopy.get(b);
				if(bShape->isVisible()){
					if (selectObj){
						if (state == 1){
							bShape->setSelected(false);
						}
					}
					else{
						if (m_firstVisibleIndex == -1 || m_lastVisibleIndex == -1){
							if (state == 1){
								bShape->setSelected(false);
							}
							continue;
						}
						bool isSelect = false;
						PolylineShape *tls = dynamic_cast<PolylineShape*>(bShape);
						CandleShape *cs = dynamic_cast<CandleShape*>(bShape);
						BarShape *barS = dynamic_cast<BarShape*>(bShape);
						if (tls){
							isSelect = selectPolyline(div, mp, tls->getBaseField(), tls->getWidth(), tls->getAttachVScale(), curIndex);
						}
						else if (cs){
							if (cs->getStyle() == CandleStyle_CloseLine){
								isSelect = selectPolyline(div, mp, cs->getCloseField(), 1, cs->getAttachVScale(), curIndex);
							}
							else{
								isSelect = selectCandle(div, (float)mp.y, cs->getHighField(), cs->getLowField(), cs->getStyleField(), cs->getAttachVScale(), curIndex);
							}
						}
						else if (barS){
							isSelect = selectBar(div, (float)mp.y, barS->getFieldName(), barS->getFieldName2(), barS->getStyleField(), barS->getAttachVScale(), curIndex);
						}
						if (isSelect){
							selectObj = bShape;
							if (state == 1){
								bShape->setSelected(true);
							}
						}
						else{
							if (state == 1){
								bShape->setSelected(false);
							}
						}
					}
				}
            }
        }
        return selectObj;
    }

	bool FCChart::selectBar(ChartDiv *div, float mpY, int fieldName, int fieldName2, int styleField, AttachVScale attachVScale, int curIndex){
		int style = 1;
		if (styleField != FCDataTable::NULLFIELD()){
            double defineStyle = m_dataSource->get2(curIndex, styleField);
            if (!FCDataTable::isNaN(defineStyle)){
                style = (int)defineStyle;
            }
        }
        if (style == -10000 || curIndex < 0 || curIndex > m_lastVisibleIndex || FCDataTable::isNaN(m_dataSource->get2(curIndex, fieldName))){
            return false;
        }
		double midValue = 0;
		if(fieldName2 != FCDataTable::NULLFIELD()){
			midValue = m_dataSource->get2(curIndex, fieldName2);
		}
        double volumn = m_dataSource->get2(curIndex, fieldName);
        float y = getY(div, volumn, attachVScale);
        float midY = getY(div, midValue, attachVScale);
		int divTop = div->getTop();
        float topY = y + divTop;
        float bottomY = midY + divTop;
        if (topY > bottomY){
            topY = midY + divTop;
            bottomY = y + divTop;
        }
        topY -= 1;
        bottomY += 1;
        if (topY >= divTop && bottomY <= divTop + div->getHeight()
            && mpY >= topY && mpY <= bottomY){
            return true;
        }
        return false;
    }

	bool FCChart::selectCandle(ChartDiv *div, float mpY, int highField, int lowField, int styleField, AttachVScale attachVScale, int curIndex){
		int style = 1;
		if (styleField != FCDataTable::NULLFIELD()){
            double defineStyle = m_dataSource->get2(curIndex, styleField);
            if (!FCDataTable::isNaN(defineStyle)){
                style = (int)defineStyle;
            }
        }
        double highValue = 0;
        double lowValue = 0;
        if (style == -10000 || curIndex < 0 || curIndex > m_lastVisibleIndex){
            return false;
        }
        else{
            highValue = m_dataSource->get2(curIndex, highField);
            lowValue = m_dataSource->get2(curIndex, lowField);
            if (FCDataTable::isNaN(highValue) || FCDataTable::isNaN(lowValue)){
                return false;
            }
        }
        float highY = getY(div, highValue, attachVScale);
        float lowY = getY(div, lowValue, attachVScale);
		int divTop = div->getTop();
        float topY = highY + divTop;
        float bottomY = lowY + divTop;
        if (topY > bottomY){
            float temp = topY;
            topY = bottomY;
            bottomY = temp;
        }
        topY -= 1;
        bottomY += 1;
        if (topY >= divTop && bottomY <= divTop + div->getHeight()
            && mpY >= topY && mpY <= bottomY){
            return true;
        }
        return false;
    }

	bool FCChart::selectPolyline(ChartDiv *div, const FCPoint& mp, int fieldName, float lineWidth, AttachVScale attachVScale, int curIndex){
        if (curIndex < 0 || curIndex > m_lastVisibleIndex || (FCDataTable::isNaN(m_dataSource->get2(curIndex, fieldName)))){
            return false;
        }
        double lineValue = m_dataSource->get2(curIndex, fieldName);
        float topY = getY(div, lineValue, attachVScale) + div->getTop();
        if (m_hScalePixel <= 1){
            if (topY >= div->getTop() && topY <= div->getTop() + div->getHeight()
            && mp.y >= topY - 8 && mp.y <= topY + 8){
                return true;
            }
        }
        else{
            int index = curIndex;
			float scaleX = getX(index);
            float judgeTop = 0;
            float judgeScaleX = scaleX;
            if (mp.x >= scaleX){
                int leftIndex = curIndex + 1;
                if (curIndex < m_lastVisibleIndex && (!FCDataTable::isNaN(m_dataSource->get2(leftIndex, fieldName)))){
                    double rightValue = m_dataSource->get2(leftIndex, fieldName);
                    judgeTop = getY(div, rightValue, attachVScale) + div->getTop();
                    if (judgeTop > div->getTop() + div->getHeight() - div->getHScale()->getHeight() || judgeTop < div->getTop() + div->getTitleBar()->getHeight()){
                        return false;
                    }
                }
                else{
                    judgeTop = topY;
                }
            }
            else{
                judgeScaleX = scaleX - (int)m_hScalePixel;
                int rightIndex = curIndex - 1;
                if (curIndex > 0 && (!FCDataTable::isNaN(m_dataSource->get2(rightIndex, fieldName)))){
                    double leftValue = m_dataSource->get2(rightIndex, fieldName);
                    judgeTop = getY(div, leftValue, attachVScale) + div->getTop();
                    if (judgeTop > div->getTop() + div->getHeight() - div->getHScale()->getHeight() || judgeTop < div->getTop() + div->getTitleBar()->getHeight()){
                        return false;
                    }
                }
                else{
                    judgeTop = topY;
                }
            }
			float judgeX = 0, judgeY = 0, judgeW = 0, judgeH = 0;
            if (judgeTop >= topY){
				judgeX = judgeScaleX;
				judgeY = topY - 2 - lineWidth;
				judgeW = (float)m_hScalePixel;
				judgeH = judgeTop - topY + lineWidth < 4 ? 4 : judgeTop - topY + 4 + lineWidth;
            }
            else{
				judgeX = judgeScaleX;
				judgeY = judgeTop - 2 - lineWidth / 2;
				judgeW = (float)m_hScalePixel;
				judgeH = topY - judgeTop + lineWidth < 4 ? 4 : topY - judgeTop + 4 + lineWidth;
            }
            if (mp.x >= judgeX && mp.x <= judgeX + judgeW && mp.y >= judgeY && mp.y <= judgeY + judgeH){
                return true;
            }
        }
        return false;
    }

	void FCChart::setProperty(const String& name, const String& value){
		if(name == L"autofillhscale"){
			setAutoFillHScale(FCStr::convertStrToBool(value));
		}
		else if(name == L"blankspace"){
			setBlankSpace(FCStr::convertStrToInt(value));
		}
		else if(name == L"canmoveshape"){
			setCanMoveShape(FCStr::convertStrToBool(value));
		}
		else if(name == L"canresizeh"){
			setCanResizeH(FCStr::convertStrToBool(value));
		}
		else if(name == L"canresizev"){
			setCanResizeV(FCStr::convertStrToBool(value));
		}
		else if(name == L"canscroll"){
			setCanScroll(FCStr::convertStrToBool(value));
		}
		else if(name == L"canzoom"){
			setCanZoom(FCStr::convertStrToBool(value));
		}
		else if(name == L"crosslinemovemode"){
			String lowerStr = FCStr::toLower(value);
			if(lowerStr == L"afterclick"){
				setCrossLineMoveMode(CrossLineMoveMode_AfterClick);
			}
			else{
				setCrossLineMoveMode(CrossLineMoveMode_FollowTouch);
			}
		}
		else if(name == L"hscalefieldtext"){
			setHScaleFieldText(value);
		}
		else if(name == L"hscalepixel"){
			setHScalePixel(FCStr::convertStrToDouble(value));
		}
		else if(name == L"leftvscalewidth"){
			setLeftVScaleWidth(FCStr::convertStrToInt(value));
		}
		else if(name == L"reversehscale"){
			setReverseHScale(FCStr::convertStrToBool(value));
		}
		else if(name == L"rightvscalewidth"){
			setRightVScaleWidth(FCStr::convertStrToInt(value));
		}
		else if(name == L"scrolladdspeed"){
			setScrollAddSpeed(FCStr::convertStrToBool(value));
		}
		else if(name == L"showcrossline"){
			setShowCrossLine(FCStr::convertStrToBool(value));
		}
		else{
			FCView::setProperty(name, value);
		}
	}
	
	void FCChart::setVisibleIndex(int firstVisibleIndex, int lastVisibleIndex){
		double xScalePixel = (double)m_workingAreaWidth / (m_lastVisibleIndex - m_firstVisibleIndex + 1);
        if (xScalePixel < 1000000){
            m_firstVisibleIndex = firstVisibleIndex;
            m_lastVisibleIndex = lastVisibleIndex;
            if (m_lastVisibleIndex != m_dataSource->rowsCount() - 1){
                m_lastRecordIsVisible = false;
            }
            else{
                m_lastRecordIsVisible = true;
            }
			setHScalePixel(xScalePixel);
            m_maxVisibleRecord = getMaxVisibleCount(m_hScalePixel, m_workingAreaWidth);
            checkLastVisibleIndex();
        }
	}

	void FCChart::zoomIn(){
		if(!m_autoFillHScale){
			double hp = m_hScalePixel;
			zoomIn(m_workingAreaWidth, m_dataSource->rowsCount(), &m_firstVisibleIndex, &m_lastVisibleIndex, &hp);
			setHScalePixel(hp);
			m_maxVisibleRecord = getMaxVisibleCount(m_hScalePixel, m_workingAreaWidth);
			checkLastVisibleIndex();
		}
	}

	void FCChart::zoomOut(){
		if(!m_autoFillHScale){
			double hp = m_hScalePixel;
			zoomOut(m_workingAreaWidth, m_dataSource->rowsCount(), &m_firstVisibleIndex, &m_lastVisibleIndex, &hp);
			setHScalePixel(hp);
			m_maxVisibleRecord = getMaxVisibleCount(m_hScalePixel, m_workingAreaWidth);
			checkLastVisibleIndex();
		}
	}

	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void FCChart::drawThinLine(FCPaint *paint, Long dwPenColor, int width, int x1, int y1, int x2, int y2){
		if(width > 1 || getNative()->allowScaleSize()){
			paint->drawLine(dwPenColor, (float)width, 0, x1, y1, x2, y2);
		}
		else{
			int left = x1 < x2 ? x1 : x2;
			int top = y1 < y2 ? y1 : y2;
			int w = abs(x1 - x2);
			int h = abs(y1 - y2);
			if (w < 1) w = 1;
			if (h < 1) h = 1;
			if ((w > 1 && h == 1) || (w == 1 && h > 1)){
				FCRect rect = {left, top, left + w, top + h};
				paint->fillRect(dwPenColor, rect);
            }
            else{
				paint->drawLine(dwPenColor, (float)width, 0, x1, y1, x2, y2);
            }
		}
	}

	void FCChart::drawText(FCPaint *paint, const wchar_t *strText, Long dwPenColor, FCFont *font, int left, int top){
		FCSize textSize = paint->textSize(strText, font);
		FCRect rect = {left, top, left + textSize.cx, top + textSize.cy};
		paint->drawText(strText, dwPenColor, font, rect);
	}
	
	ArrayList<double> FCChart::getVScaleStep(double max, double min, ChartDiv *div, VScale *vScale){
        ArrayList<double> scaleStepList;
        if (vScale->getType() == VScaleType_EqualDiff || vScale->getType() == VScaleType_Percent){
			double step = 0;
			int distance = div->getVGrid()->getDistance();
            int digit = 0, gN = div->getWorkingAreaHeight() / distance;
            if (gN == 0){
                gN = 1;
            }
			gridScale(min, max, div->getWorkingAreaHeight(), distance, distance / 2, gN, &step, &digit);
            if (step > 0){
                double start = 0;
                if (min >= 0){
                    while (start + step < min){
                        start += step;
                    }
                }
                else{
                    while (start - step > min){
                        start -= step;
                    }
                }
                while (start <= max){
                    scaleStepList.add(start);
                    start += step;
                }
            }
        }
        else if (vScale->getType() == VScaleType_EqualRatio){
            int baseField = getVScaleBaseField(div, vScale);
            double bMax = 0;
            double bMin = 0;
			bool isNullBMax = true,isNullBMin = true;
            if (baseField != FCDataTable::NULLFIELD()){
                for (int i = 0; i < m_dataSource->rowsCount(); i++){
                    double value = m_dataSource->get2(i, baseField);
                    if (!FCDataTable::isNaN(value)){
						 if(isNullBMax){
							bMax = value;
							isNullBMax = false;
						}
                        else if (value > bMax){
                            bMax = value;
                        }
						if(isNullBMin){
							bMin = value;
							isNullBMin = false;
						}
                        else if (value < bMin){
                            bMin = value;
                        }
                    }
                }
                if (!isNullBMax && !isNullBMin && bMin > 0 && bMax > 0 && bMin < bMax){
                    double num = bMin;
                    while (num < bMax){
                        num = num * 1.1;
                        if (num >= min && num <= max){
                            scaleStepList.add(num);
                        }
                    }
                }
            }
        }
        else if (vScale->getType() == VScaleType_Divide){
            scaleStepList.add(min + (max - min) * 0.25);
            scaleStepList.add(min + (max - min) * 0.5);
            scaleStepList.add(min + (max - min) * 0.75);
        }
        else if (vScale->getType() == VScaleType_GoldenRatio){
            scaleStepList.add(min);
            scaleStepList.add(min + (max - min) * 0.191);
            scaleStepList.add(min + (max - min) * 0.382);
            scaleStepList.add(min + (max - min) * 0.5);
            scaleStepList.add(min + (max - min) * 0.618);
            scaleStepList.add(min + (max - min) * 0.809);
        }
        if ((max != min) && scaleStepList.size() == 0){
	    if (!FCDataTable::isNaN(min)){
            	scaleStepList.add(min);
            }
        }
        return scaleStepList;
    }

	void FCChart::onPaintBar(FCPaint *paint, ChartDiv *div, BarShape *bs){
		int ciFieldName1 = m_dataSource->getColumnIndex(bs->getFieldName());
		int ciFieldName2 = m_dataSource->getColumnIndex(bs->getFieldName2());
		int ciClr = m_dataSource->getColumnIndex(bs->getColorField());
		int ciStyle = m_dataSource->getColumnIndex(bs->getStyleField());
		int defaultLineWidth = 1;
        if (!isOperating() && m_crossStopIndex != -1){
            if (selectBar(div, (float)getTouchPoint().y, bs->getFieldName(), bs->getFieldName2(), bs->getStyleField(), bs->getAttachVScale(), m_crossStopIndex)){
                defaultLineWidth = 2;
            }
        }
		for (int i = m_firstVisibleIndex; i <= m_lastVisibleIndex; i++){
			int thinLineWidth = 1;
			if(m_crossStopIndex == i){
				thinLineWidth = defaultLineWidth;
			}
			int style = -10000;
			switch (bs->getStyle()){
				case BarStyle_Line:
					style = 2;
					break;
				case BarStyle_Rect:
					style = 0;
					break;
			}
			if (ciStyle != -1){
				double defineStyle = m_dataSource->get3(i, ciStyle);
				if (!FCDataTable::isNaN(defineStyle)){
					style = (int)defineStyle;
				}
			}
			if(style == -10000){
				continue;
			}
			double value = m_dataSource->get3(i, ciFieldName1); 
			int scaleX = (int)getX(i);
			double midValue = 0;
			if(ciFieldName2 != -1){
				midValue = m_dataSource->get3(i, ciFieldName2); 
			}
			int midY = (int)getY(div, midValue, bs->getAttachVScale());
			if (!FCDataTable::isNaN(value)){
				float barY = getY(div, value, bs->getAttachVScale());
				int startPX = scaleX;
				int startPY = (int)midY;
				int endPX = scaleX;
				int endPY = (int)barY;
				if (bs->getStyle() == BarStyle_Rect){
					if (startPY == div->getHeight() - div->getHScale()->getHeight()){
						startPY = div->getHeight() - div->getHScale()->getHeight() + 1;
					}
				}
				int x = 0, y = 0, width = 0, height = 0;
				width = (int)(m_hScalePixel * 2 / 3);
				if (width % 2 == 0){
					width += 1;
				}
				if (width < 3){
					width = 1;
				}
				x = scaleX - width / 2;
				if (startPY >= endPY){
					y = endPY;
				}
				else{
					y = startPY;
				}
				height = abs(startPY - endPY);
				if (height < 1){
					height = 1;
				}
				Long barColor = FCColor_None;
				if (ciClr != -1){
					double defineColor = m_dataSource->get3(i, ciClr);
					if(!FCDataTable::isNaN(defineColor)){
						barColor = (Long)defineColor;
					}
				}
				if(barColor == FCColor_None){
					if (startPY >= endPY){
						barColor = bs->getUpColor();
					}
					else{
						barColor = bs->getDownColor();
					}
				}
				switch (style){
					case -1:
						if (m_hScalePixel <= 3){
							drawThinLine(paint, barColor, thinLineWidth, startPX, y, startPX, y + height);
						}
						else{
							FCRect rect = {x, y, x + width,y + height};
							paint->fillRect(div->getBackColor(), rect);
							paint->drawRect(barColor, (float)thinLineWidth, 1, rect);
						}
						break;
					case 0:
						if (m_hScalePixel <= 3){
							drawThinLine(paint, barColor, thinLineWidth, startPX, y, startPX, y + height);
						}
						else{
							FCRect rect = {x, y, x + width,y + height};
							paint->fillRect(barColor, rect);
							if (thinLineWidth > 1){
								if (startPY >= endPY){
									paint->drawRect(bs->getDownColor(), (float)thinLineWidth, 0, rect);
								}
								else{
									paint->drawRect(bs->getUpColor(), (float)thinLineWidth, 0, rect);
								}
							}
						}
						break;
					case 1:
						if (m_hScalePixel <= 3){
							drawThinLine(paint, barColor, thinLineWidth, startPX, y, startPX, y + height);
						}
						else{
							FCRect rect = {x, y, x + width,y + height};
							paint->fillRect(div->getBackColor(), rect);
							paint->drawRect(barColor, (float)thinLineWidth, 0, rect);
						}
						break;
					case 2:
                        if (startPY <= 0){
                            startPY = 0;
                        }
                        if (startPY >= div->getHeight()){
                            startPY = div->getHeight();
                        }
                        if (endPY <= 0){
                            endPY = 0;
                        }
                        if (endPY >= div->getHeight()){
                            endPY = div->getHeight();
                        }
                        if (bs->getLineWidth() <= 1){
                            drawThinLine(paint, barColor, thinLineWidth, startPX, startPY, endPX, endPY);
                        }
                        else{
                            int lineWidth = (int)bs->getLineWidth();
                            if (lineWidth > m_hScalePixel){
                                lineWidth = (int)m_hScalePixel;
                            }
                            if (lineWidth < 1){
                                lineWidth = 1;
                            }
							paint->drawLine(barColor, (float)(lineWidth + thinLineWidth - 1), 0, startPX, startPY, endPX, endPY);
                        }
						break;
				}
				if(bs->isSelected()){
					int kPInterval = m_maxVisibleRecord / 30;
					if (kPInterval < 2){
						kPInterval = 2;
					}
					if (i % kPInterval == 0){
						if (barY >= div->getTitleBar()->getHeight()
							&& barY <= div->getHeight() - div->getHScale()->getHeight()){
							FCRect sRect = {scaleX - 3, (int)barY - 4, scaleX + 4, (int)barY + 3};
							paint->fillRect(bs->getSelectedColor(), sRect);
						}
					}
				}
			}
			if (i == m_lastVisibleIndex && div->getVScale(bs->getAttachVScale())->getVisibleMin() < 0){
				if (m_reverseHScale){
					int left = (int)(m_leftVScaleWidth + m_workingAreaWidth - (m_lastVisibleIndex - m_firstVisibleIndex + 1) * m_hScalePixel);
					paint->drawLine(bs->getDownColor(), 1, 0, m_leftVScaleWidth + m_workingAreaWidth, (int)midY, left, (int)midY);
				}
				else{
					int right = (int)(m_leftVScaleWidth + (m_lastVisibleIndex - m_firstVisibleIndex + 1) * m_hScalePixel);
					paint->drawLine(bs->getDownColor(), 1, 0, m_leftVScaleWidth, midY, right, midY);
				}
			}
		}
	}

	void FCChart::onPaintCandle(FCPaint *paint, ChartDiv *div, CandleShape *cs){
		int x = 0, y = 0;
		vector<FCPoint> points;
		int visibleMaxIndex = 0, visibleMinIndex = 0;
		double visibleMax = 0, visibleMin = 0;
		int ciHigh = m_dataSource->getColumnIndex(cs->getHighField());
		int ciLow = m_dataSource->getColumnIndex(cs->getLowField());
		int ciOpen = m_dataSource->getColumnIndex(cs->getOpenField());
		int ciClose = m_dataSource->getColumnIndex(cs->getCloseField());
		int ciClr = m_dataSource->getColumnIndex(cs->getColorField());
		int ciStyle = m_dataSource->getColumnIndex(cs->getStyleField());
		int defaultLineWidth = 1;
        if (!isOperating() && m_crossStopIndex != -1){
            if (selectCandle(div, (float)getTouchPoint().y, cs->getHighField(), cs->getLowField(), cs->getStyleField(), cs->getAttachVScale(), m_crossStopIndex)){
                defaultLineWidth = 2;
            }
        }
		for (int i = m_firstVisibleIndex; i <= m_lastVisibleIndex; i++){
			int thinLineWidth = 1;
			if(i == m_crossStopIndex){
				thinLineWidth = defaultLineWidth;
			}
			int style = -10000;
			switch (cs->getStyle()){
				case CandleStyle_Rect:
					style = 0;
					break;
				case CandleStyle_American:
					style = 3;
					break;
				case CandleStyle_CloseLine:
					style = 4;
					break;
				case CandleStyle_Tower:
					style = 5;
					break;
			}
			if (ciStyle != -1){
				double defineStyle = m_dataSource->get3(i, ciStyle);
				if (!FCDataTable::isNaN(defineStyle)){
					style = (int)defineStyle;
				}
			}
			if (style == -10000){
				continue;
			}
			double open = m_dataSource->get3(i, ciOpen);
			double high = m_dataSource->get3(i, ciHigh);
			double low = m_dataSource->get3(i, ciLow);
			double close = m_dataSource->get3(i, ciClose); 
			if (FCDataTable::isNaN(open) || FCDataTable::isNaN(high) || FCDataTable::isNaN(low) || FCDataTable::isNaN(close)){
                if (i != m_lastVisibleIndex || style != 4){
                    continue;
                }
			}
			int scaleX = (int)getX(i);
			if (cs->getShowMaxMin()){
				if (i == m_firstVisibleIndex){
					visibleMaxIndex = i;
					visibleMinIndex = i;
					visibleMax = high;
					visibleMin = low ;
				}
				else{
					if (high > visibleMax){
						visibleMax = high;
						visibleMaxIndex = i;
					}
					if (low < visibleMin){
						visibleMin = low;
						visibleMinIndex = i;
					}
				}
			}
			int highY = (int)getY(div, high, cs->getAttachVScale());
			int openY = (int)getY(div, open, cs->getAttachVScale());
			int lowY = (int)getY(div, low, cs->getAttachVScale());
			int closeY = (int)getY(div, close, cs->getAttachVScale());
			int cwidth = (int)(m_hScalePixel * 2 / 3);
			if (cwidth % 2 == 0){
				cwidth += 1;
			}
			if (cwidth < 3){
				cwidth = 1;
			}
			int xsub = cwidth / 2;
			if(xsub < 1){
				xsub = 1;
			}
			switch(style){
			case 3:{
					Long color = cs->getUpColor();
					if(open > close){
						color = cs->getDownColor();
					}
					if (ciClr != -1){
						double defineColor = m_dataSource->get3(i, ciClr);
						if(!FCDataTable::isNaN(defineColor)){
							color = (Long)defineColor;
						}
					}
					if ((int)highY != (int)lowY){
						if (m_hScalePixel <= 3){
							drawThinLine(paint, color, thinLineWidth, scaleX, highY, scaleX, lowY);
						}
						else{
							drawThinLine(paint, color, thinLineWidth, scaleX, highY, scaleX, lowY);
							drawThinLine(paint, color, thinLineWidth, scaleX - xsub, openY, scaleX, openY);
							drawThinLine(paint, color, thinLineWidth, scaleX, closeY, scaleX + xsub, closeY);
						}
					}
					else{
						drawThinLine(paint, color, thinLineWidth, scaleX - xsub, closeY, scaleX + xsub, closeY);
					}
					break;
				}
			case 4:
				onPaintPolyline(paint, div, cs->getUpColor(), FCColor_None, ciClr, (float)defaultLineWidth, PolylineStyle_SolidLine, close, cs->getAttachVScale(), scaleX, (int)closeY, i, &points, &x, &y);
				break;
			default:
				if (open <= close){
					float recth = getUpCandleHeight(close, open, div->getVScale(cs->getAttachVScale())->getVisibleMax(), div->getVScale(cs->getAttachVScale())->getVisibleMin(), (float)div->getWorkingAreaHeight() - div->getVScale(cs->getAttachVScale())->getPaddingBottom() - div->getVScale(cs->getAttachVScale())->getPaddingTop());
					if (recth < 1){
						recth = 1;
					}
					int rcUpX = scaleX - xsub, rcUpTop = (int)openY, rcUpBottom = (int)openY, rcUpW = cwidth, rcUpH = (int)recth;
					if (openY < closeY){
						rcUpTop = (int)openY;
						rcUpBottom = (int)closeY;
					}
					Long upColor = FCColor_None;
					int colorField = cs->getColorField();
					if (colorField != FCDataTable::NULLFIELD()){
						double defineColor = m_dataSource->get2(i, colorField);
						if(!FCDataTable::isNaN(defineColor)){
							upColor = (Long)defineColor;
						}
					}
					if(upColor == FCColor_None){
						upColor = cs->getUpColor();
					}
					switch (style){
						case 0:
						case 1:
						case 2:
							if ((int)highY != (int)lowY){
								drawThinLine(paint, upColor, thinLineWidth, scaleX, highY, scaleX, lowY);
								if (m_hScalePixel > 3){
									if ((int)openY == (int)closeY){
										drawThinLine(paint, upColor, thinLineWidth, rcUpX, rcUpBottom, rcUpX + rcUpW, rcUpBottom);
									}
									else{
										FCRect rcUp = {rcUpX, rcUpTop, rcUpX + rcUpW, rcUpBottom};
										if (style == 0 || style == 1){
											if (rcUpW > 0 && rcUpH > 0 && m_hScalePixel > 3){
												paint->fillRect(div->getBackColor(), rcUp);
											}
											paint->drawRect(upColor, (float)thinLineWidth, 0, rcUp);
										}
										else if (style == 2){
											paint->fillRect(upColor, rcUp);
											if (thinLineWidth > 1){
                                                paint->drawRect(upColor, (float)thinLineWidth, 0, rcUp);
                                            }
										}
									}
								}
							}
							else{
								drawThinLine(paint, upColor, thinLineWidth, scaleX - xsub, closeY, scaleX + xsub, closeY);
							}
							break;
						case 5:{
								double lOpen = m_dataSource->get3(i - 1, ciOpen);
								double lClose = m_dataSource->get3(i - 1, ciClose);
								double lHigh = m_dataSource->get3(i - 1, ciHigh);
								double lLow = m_dataSource->get3(i - 1, ciLow);
								int top = highY;
								int bottom = lowY;
								if ((int)highY > (int)lowY){
									top = lowY;
									bottom = highY;
								}
								if (i == 0 || FCDataTable::isNaN(lOpen) || FCDataTable::isNaN(lClose) || FCDataTable::isNaN(lHigh) || FCDataTable::isNaN(lLow)){
									if (m_hScalePixel <= 3){
										drawThinLine(paint, upColor, thinLineWidth, rcUpX, top, rcUpX, bottom);
									}
									else{
										int rcUpHeight = (int)abs(bottom - top == 0 ? 1 : bottom - top);
										if (rcUpW > 0 && rcUpHeight > 0){
											FCRect rcUp = {rcUpX, top, rcUpX + rcUpW, top + rcUpHeight};
											paint->fillRect(upColor, rcUp);
											if (thinLineWidth > 1){
                                                paint->drawRect(upColor, (float)thinLineWidth, 0, rcUp);
                                            }
										}
									}
								}
								else{
									if (m_hScalePixel <= 3){
										drawThinLine(paint, upColor, thinLineWidth, rcUpX, top, rcUpX, bottom);
									}
									else{
										int rcUpHeight = (int)abs(bottom - top == 0 ? 1 : bottom - top);
										if (rcUpW > 0 && rcUpHeight> 0){
											FCRect rcUp = {rcUpX, top, rcUpX + rcUpW, top + rcUpHeight};
											paint->fillRect(upColor, rcUp);
											if (thinLineWidth > 1){
                                                paint->drawRect(upColor, (float)thinLineWidth, 0, rcUp);
                                            }
										}
									}
									if (lClose < lOpen && low < lHigh){
										int tx = rcUpX;
										int ty = (int)getY(div, lHigh, cs->getAttachVScale());
										if (high < lHigh){
											ty = (int)highY;
										}
										int width = rcUpW;
										int height = (int)lowY - ty;
										if (height > 0){
											if (m_hScalePixel <= 3){
												drawThinLine(paint, cs->getDownColor(), thinLineWidth, tx, ty, tx, ty + height);
											}
											else{
												if (width > 0 && height > 0){
													FCRect rect = {tx, ty, tx + width, ty + height};
													paint->fillRect(cs->getDownColor(), rect);
													if (thinLineWidth > 1){
														paint->drawRect(cs->getDownColor(), (float)thinLineWidth, 0, rect);
													}
												}
											}
										}
									}
								}
								break;
							}
					}
				}
				else{
					float recth = getDownCandleHeight(close, open, div->getVScale(cs->getAttachVScale())->getVisibleMax(), div->getVScale(cs->getAttachVScale())->getVisibleMin(), (float)div->getWorkingAreaHeight() - div->getVScale(cs->getAttachVScale())->getPaddingBottom() - div->getVScale(cs->getAttachVScale())->getPaddingTop());
					if (recth < 1){
						recth = 1;
					}
					int rcDownX = scaleX - xsub, rcDownTop = (int)openY, rcDownBottom = (int)closeY, rcDownW = cwidth, rcDownH = (int)recth;
					if (closeY < openY){
						rcDownTop = (int)closeY;
						rcDownBottom = (int)openY;
					}
					Long downColor = FCColor_None;
					if (ciClr != -1){
						double defineColor = m_dataSource->get3(i, ciClr);
						if(!FCDataTable::isNaN(defineColor)){
							downColor = (Long)defineColor;
						}
					}
					if(downColor == FCColor_None){
						downColor = cs->getDownColor();
					}
					switch (style){
						case 0:
						case 1:
						case 2:
							if ((int)highY != (int)lowY){
								drawThinLine(paint, downColor, thinLineWidth, scaleX, highY, scaleX, lowY);
								if (m_hScalePixel > 3){
									FCRect rcDown = {rcDownX, rcDownTop, rcDownX + rcDownW, rcDownBottom};
									if (style == 1){
										if (rcDownW > 0 && rcDownH > 0 && m_hScalePixel > 3){
											paint->fillRect(div->getBackColor(), rcDown);
										}
										paint->drawRect(downColor, (float)thinLineWidth, 0, rcDown);
									}
									else if (style == 0 || style == 1){
										paint->fillRect(downColor, rcDown);
										if (thinLineWidth > 1){
											paint->drawRect(downColor, (float)thinLineWidth, 0, rcDown);
										}
									}
								}
							}
							else{
								drawThinLine(paint, downColor, thinLineWidth, scaleX - xsub, closeY, scaleX + xsub, closeY);
							}
							break;
						case 5:
							double lOpen = m_dataSource->get3(i - 1, ciOpen);
							double lClose = m_dataSource->get3(i - 1, ciClose);
							double lHigh = m_dataSource->get3(i - 1, ciHigh);
							double lLow = m_dataSource->get3(i - 1, ciLow);
							int top = highY;
							int bottom = lowY;
							if ((int)highY > (int)lowY){
								top = lowY;
								bottom = highY;
							}
							if (i == 0 || FCDataTable::isNaN(lOpen) || FCDataTable::isNaN(lClose) || FCDataTable::isNaN(lHigh) || FCDataTable::isNaN(lLow)){
								if (m_hScalePixel <= 3){
									drawThinLine(paint, downColor, thinLineWidth, rcDownX, top, rcDownX, bottom);
								}
								else{
									int rcDownHeight = (int)abs(bottom - top == 0 ? 1 : bottom - top);
									if (rcDownW > 0 && rcDownHeight > 0){
										FCRect rcDown = {rcDownX, top, rcDownX + rcDownW, rcDownBottom};
										paint->fillRect(downColor,rcDown);
										if (thinLineWidth > 1){
											paint->drawRect(downColor, (float)thinLineWidth, 0, rcDown);
										}
									}
								}
							}
							else{
								if (m_hScalePixel <= 3){
									drawThinLine(paint, downColor, thinLineWidth, rcDownX, top, rcDownX, bottom);
								}
								else{
									int rcDownHeight = (int)abs(bottom - top == 0 ? 1 : bottom - top);
									if (rcDownW > 0 && rcDownHeight > 0){
										 FCRect rcDown = {rcDownX, top, rcDownX + rcDownW, rcDownBottom};
										 paint->fillRect(downColor, rcDown);
										 if (thinLineWidth > 1){
											 paint->drawRect(downColor, (float)thinLineWidth, 0, rcDown);
										 }
									}
								}
								if (lClose >= lOpen && high > lLow){
									int tx = rcDownX;
									int ty = (int)highY;
									int width = rcDownW;
									int height = (int)abs(getY(div, lLow, cs->getAttachVScale()) - ty);
									if (low > lLow){
										height = (int)lowY - ty;
									}
									if (height > 0){
										if (m_hScalePixel <= 3){
											drawThinLine(paint, cs->getUpColor(), thinLineWidth, tx, ty, tx, ty + height);
										}
										else{
											if (width > 0 && height > 0){
												FCRect rect = {tx, ty, tx + width, ty + height};
												paint->fillRect(cs->getUpColor(), rect);
												if (thinLineWidth > 1){
													paint->drawRect(cs->getUpColor(), (float)thinLineWidth, 0, rect);
												}
											}
										}
									}
								}
							}
							break;
					}
				}
				break;
			}
			if(cs->isSelected()){
				int kPInterval = m_maxVisibleRecord / 30;
				if (kPInterval < 2){
					kPInterval = 3;
				}
				if (i % kPInterval == 0){
					if (!FCDataTable::isNaN(open) && !FCDataTable::isNaN(high) && !FCDataTable::isNaN(low) && !FCDataTable::isNaN(close)){
						if (closeY >= div->getTitleBar()->getHeight()
							&& closeY <= div->getHeight() - div->getHScale()->getHeight()){
							FCRect sRect = {scaleX - 3, closeY - 4, scaleX + 4, closeY + 3};
							paint->fillRect(cs->getSelectedColor(), sRect);
						}
					}
				}
			}
		}
		onPaintCandleEx(paint, div, cs, visibleMaxIndex, visibleMax, visibleMinIndex, visibleMin);
	}

	void FCChart::onPaintCandleEx(FCPaint *paint, ChartDiv *div, CandleShape *cs, int visibleMaxIndex, double visibleMax, int visibleMinIndex, double visibleMin){
		if (m_dataSource->rowsCount() > 0){
			if (visibleMaxIndex != -1 && visibleMinIndex != -1 && cs->getShowMaxMin()){
                double max = visibleMax;
                double min = visibleMin;
                float scaleYMax = getY(div, max, cs->getAttachVScale());
                float scaleYMin = getY(div, min, cs->getAttachVScale());
                int scaleXMax = (int)getX(visibleMaxIndex);
                int digit = div->getVScale(cs->getAttachVScale())->getDigit();
				wchar_t maxStr[100] = {0};
				FCStr::getValueByDigit(max, digit, maxStr);
				FCFont *font = div->getFont();
				FCSize maxSize = paint->textSize(maxStr, font);
				int width = getWidth();
                float maxPX = 0, maxPY = 0;
                float strY = 0;
                if (scaleYMax > scaleYMin){
					getCandleMinStringPoint((float)scaleXMax, scaleYMax, (float)maxSize.cx, (float)maxSize.cy, width, m_leftVScaleWidth, m_rightVScaleWidth, &maxPX, &maxPY);
					strY = maxPY + maxSize.cy;
                }
                else{
					getCandleMaxStringPoint((float)scaleXMax, scaleYMax, (float)maxSize.cx, (float)maxSize.cy, width, m_leftVScaleWidth, m_rightVScaleWidth,  &maxPX, &maxPY);
					strY = maxPY;
                }
				Long tagColor = cs->getTagColor();
				drawText(paint, maxStr, tagColor, font, (int)maxPX, (int)maxPY);
                paint->drawLine(tagColor, 1, 0, scaleXMax, (int)scaleYMax, (int)(maxPX + maxSize.cx / 2), (int)strY);
				wchar_t minStr[100] = {0};
				FCStr::getValueByDigit(min, digit, minStr);
				FCSize minSize = paint->textSize(minStr, font);
                int scaleXMin = (int)getX(visibleMinIndex);
                float minPX = 0, minPY = 0;
                if (scaleYMax > scaleYMin){
					getCandleMaxStringPoint((float)scaleXMin, scaleYMin, (float)minSize.cx, (float)minSize.cy, width, m_leftVScaleWidth, m_rightVScaleWidth, &minPX, &minPY);
					strY = minPY;
                }
                else{
					getCandleMinStringPoint((float)scaleXMin, scaleYMin, (float)minSize.cx, (float)minSize.cy, width, m_leftVScaleWidth, m_rightVScaleWidth, &minPX, &minPY);
					strY = minPY + minSize.cy;
                }
				drawText(paint, minStr, tagColor, font, (int)minPX, (int)minPY);
                paint->drawLine(tagColor, 1, 0, scaleXMin, (int)scaleYMin, (int)(minPX + minSize.cx / 2), (int)strY);
            }
        }
	}

	void FCChart::onPaintCrossLine(FCPaint *paint, ChartDiv *div){
        FCPoint touchPoint = getTouchPoint();
        if (m_cross_y != -1){
			int divWidth = div->getWidth();
			int divHeight = div->getHeight();
            int titleBarHeight = div->getTitleBar()->getHeight();
            int hScaleHeight = div->getHScale()->getHeight();
			int mpY = m_cross_y - div->getTop();
            if (m_dataSource->rowsCount() > 0 && m_hResizeType == 0 && !m_userResizeDiv){
                if (mpY >= titleBarHeight && mpY <= divHeight - hScaleHeight){
					VScale *leftVScale = div->getLeftVScale();
					CrossLineTip *crossLineTip = leftVScale->getCrossLineTip();
                    if (m_leftVScaleWidth != 0 && crossLineTip->isVisible()){
						if(crossLineTip->allowUserPaint()){
							FCRect clipRect = {0, 0, m_leftVScaleWidth, divHeight - hScaleHeight};
							crossLineTip->onPaint(paint, div, clipRect);
						}
						else{
							double lValue = getNumberValue(div, touchPoint, AttachVScale_Left);
							wchar_t leftValue[100] = {0};
							FCStr::getValueByDigit(lValue, leftVScale->getDigit(), leftValue);
							FCSize leftYTipFontSize = paint->textSize(leftValue, crossLineTip->getFont());
							if (leftYTipFontSize.cx > 0 && leftYTipFontSize.cy > 0){
								int lRtX = m_leftVScaleWidth - leftYTipFontSize.cx - 1;
								int lRtY = mpY - leftYTipFontSize.cy / 2;
								int lRtW = leftYTipFontSize.cx;
								int lRtH = leftYTipFontSize.cy;
								if (lRtW > 0 && lRtH > 0){
									FCRect lRtL = {lRtX, lRtY, lRtX + lRtW, lRtY + lRtH};
									paint->fillRect(crossLineTip->getBackColor(),lRtL);
									paint->drawRect(crossLineTip->getTextColor(), 1, 0, lRtL);
								}
								drawText(paint, leftValue, crossLineTip->getTextColor(), crossLineTip->getFont(), lRtX, lRtY);
							}
						}
                    }
					VScale *rightVScale = div->getRightVScale();
					crossLineTip = rightVScale->getCrossLineTip();
                    if (m_rightVScaleWidth != 0 && crossLineTip->isVisible()){
						if(crossLineTip->allowUserPaint()){
							FCRect clipRect = {divWidth - m_rightVScaleWidth, 0, divWidth, divHeight - hScaleHeight};
							crossLineTip->onPaint(paint, div, clipRect);
						}
						else{
							double rValue = getNumberValue(div, touchPoint, AttachVScale_Right);
							wchar_t rightValue[100] = {0};
							FCStr::getValueByDigit(rValue, rightVScale->getDigit(), rightValue);
							FCSize rightYTipFontSize = paint->textSize(rightValue, crossLineTip->getFont());
							if (rightYTipFontSize.cx > 0 && rightYTipFontSize.cy > 0){
								int rRtX = getWidth() - m_rightVScaleWidth + 1;
								int rRtY = mpY - rightYTipFontSize.cy / 2;
								int rRtW = rightYTipFontSize.cx;
								int rRtH = rightYTipFontSize.cy;
								if (rRtW > 0 && rRtH > 0){
									FCRect rRtL = {rRtX, rRtY, rRtX + rRtW, rRtY + rRtH};
									paint->fillRect(crossLineTip->getBackColor(), rRtL);
									paint->drawRect(crossLineTip->getTextColor(), 1, 0, rRtL);
								}
								drawText(paint, rightValue, crossLineTip->getTextColor(), crossLineTip->getFont(), rRtX, rRtY);
							}
						}
                    }
                }
            }
            int verticalX = 0;
			if (m_crossStopIndex >= m_firstVisibleIndex && m_crossStopIndex <= m_lastVisibleIndex){
                verticalX = (int)getX(m_crossStopIndex);
            }
            if (!m_isScrollCross){
                verticalX = touchPoint.x;
            }
			CrossLine *crossLine = div->getCrossLine();
            if (crossLine->allowUserPaint()){
                FCRect clRect = {0, 0, divWidth, divHeight};
                crossLine->onPaint(paint, div, clRect);
            }
            else{
				if (m_showCrossLine){
					if (mpY >= titleBarHeight && mpY <= divHeight - hScaleHeight){
						paint->drawLine(crossLine->getLineColor(), 1, 0, m_leftVScaleWidth, mpY, getWidth() - m_rightVScaleWidth, mpY);
					}
				}
				if (m_crossStopIndex == -1 || m_crossStopIndex < m_firstVisibleIndex || m_crossStopIndex > m_lastVisibleIndex){
					if (m_showCrossLine){
						int x = touchPoint.x;
						if (x > m_leftVScaleWidth && x < m_leftVScaleWidth + m_workingAreaWidth){
							paint->drawLine(crossLine->getLineColor(), 1, 0, x, titleBarHeight, x, divHeight - hScaleHeight);
						}
					}
					return;
				}
				if (m_showCrossLine){
					paint->drawLine(crossLine->getLineColor(), 1, 0, verticalX, titleBarHeight, verticalX, divHeight - hScaleHeight);
				}
			}
            if (m_hResizeType == 0 && !m_userResizeDiv){
				HScale *hScale = div->getHScale();
				CrossLineTip *hScaleCrossLineTip = hScale->getCrossLineTip();
				if (hScale->isVisible() && hScaleCrossLineTip->isVisible()){
					if(hScaleCrossLineTip->allowUserPaint()){
						FCRect clipRect = {0, divHeight - hScaleHeight, divWidth, divHeight};
						hScaleCrossLineTip->onPaint(paint, div, clipRect);
					}
					else{
						wchar_t *tip = 0;
						wchar_t formatDate[100] = {0};
						wchar_t ctip[100] = {0};
						tip = L"";
						if (hScale->getHScaleType() == HScaleType_Date){
							FCStr::getFormatDate(m_dataSource->getXValue(m_crossStopIndex), formatDate);
							tip = formatDate;
						}
						else if (hScale->getHScaleType() == HScaleType_Number){
							_stprintf_s(ctip, 99, L"%d", (int)m_dataSource->getXValue(m_crossStopIndex));
							tip = ctip;
						}
						FCSize xTipFontSize = paint->textSize(tip, hScaleCrossLineTip->getFont());
						int xRtX = verticalX - xTipFontSize.cx / 2;
						int xRtY = divHeight - hScaleHeight + 2;
						int xRtW = xTipFontSize.cx + 2;
						int xRtH = xTipFontSize.cy;
						if (xRtX < m_leftVScaleWidth){
							xRtX = m_leftVScaleWidth;
							xRtY = divHeight - hScaleHeight + 2;
						}
						if (xRtX + xRtW > divWidth - m_rightVScaleWidth){
							xRtX = divWidth - m_rightVScaleWidth - xTipFontSize.cx - 1;
							xRtY = divHeight - hScaleHeight + 2;
						}                             
						if (xRtW > 0 && xRtH > 0){
							FCRect xRtL = {xRtX, xRtY, xRtX + xRtW, xRtY + xRtH};
							paint->fillRect(hScaleCrossLineTip->getBackColor(), xRtL);
							paint->drawRect(hScaleCrossLineTip->getTextColor(),1 , 0, xRtL);
							drawText(paint, tip, hScaleCrossLineTip->getTextColor(), hScaleCrossLineTip->getFont(), xRtX, xRtY);
						}
					}
                }
            }
        }
    }

	void FCChart::onPaintDivBackGround(FCPaint *paint, ChartDiv *div){
        int width = div->getWidth();
        int height = div->getHeight();
        if (width < 1){
            width = 1;
        }
        if (height < 1){
            height = 1;
        }
        if (width > 0 && height > 0){
			FCRect rect = {0, 0, width, height};
			if(div->allowUserPaint()){
				div->onPaint(paint, rect);
			}
			else{
				if (div->getBackColor() != getBackColor()){
					paint->fillRect(div->getBackColor(), rect);
				}
			}
        }
    }

	void FCChart::onPaintDivBorder(FCPaint *paint, ChartDiv *div){
        int y = 0;
        int width = div->getWidth();
        int height = div->getHeight();
        if (width < 1){
            width = 1;
        }
        if (height < 1){
            height = 1;
        }
        if (width > 0 && height > 0){
            ChartDiv *lDiv = 0;
            ArrayList<ChartDiv*> divsCopy = getDivs();
			for(int d = 0; d < divsCopy.size(); d++){
				ChartDiv *cDiv = divsCopy.get(d);
                if (div != cDiv){
                    lDiv = cDiv;
                }
                else{
                    break;
                }
            }
            if (lDiv){
                if (!lDiv->getHScale()->isVisible()){
                    paint->drawLine(div->getHScale()->getScaleColor(), 1, 0, m_leftVScaleWidth, y + 1, width - m_rightVScaleWidth, y + 1);
                }
                else{
                    paint->drawLine(div->getHScale()->getScaleColor(), 1, 0, 0, y + 1, width, y + 1);
                }
            }
            if (div->isShowSelect() && div->isSelected()){
                if (m_leftVScaleWidth > 0){
					int leftRectX = 1;
					int leftRectY = 0;
					int leftRectW = m_leftVScaleWidth - 1;
					int leftRectH = div->getHeight() - div->getHScale()->getHeight();
                    if (leftRectW > 0 && leftRectH > 0){
						FCRect leftRect = {leftRectX,leftRectY,leftRectX + leftRectW,leftRectY + leftRectH};
						paint->drawRect(div->getLeftVScale()->getScaleColor(), 1, 0, leftRect);
                    }
                }
                if (m_rightVScaleWidth > 0){
					int rightRectX = getWidth() - m_rightVScaleWidth + 1;
					int rightRectY = 0;
					int rightRectW = m_rightVScaleWidth - 1;
					int rightRectH = div->getHeight() - div->getHScale()->getHeight();
                    if (rightRectW > 0 && rightRectH> 0){
						FCRect rightRect = {rightRectX,rightRectY,rightRectX + rightRectW,rightRectY + rightRectH};
                        paint->drawRect(div->getRightVScale()->getScaleColor(),1 , 0, rightRect);
                    }
                }
            }
            if (div->getBorderColor() != FCColor_None){
            	if (width > 0 && height > 0){
					FCRect rect = {0, y, width, height};
                	paint->drawRect(div->getBorderColor(), 1, 0, rect);
                }
			}
        }
    }
		
	void FCChart::onPaintHScale(FCPaint *paint, ChartDiv *div){
		HScale *hScale = div->getHScale();
		ScaleGrid *vGrid = div->getVGrid();
		int width = div->getWidth(), height = div->getHeight();
		int scaleHeight = hScale->getHeight();
        if ((hScale->isVisible() || vGrid->isVisible()) && height >= scaleHeight){
			FCRect hRect = {0, height - scaleHeight, width, height};
			if(hScale->allowUserPaint()){
				hScale->onPaint(paint, div, hRect);
			}
			if(vGrid->allowUserPaint()){
				vGrid->onPaint(paint, div, hRect);
			}
			if(hScale->allowUserPaint() && vGrid->allowUserPaint()){
				return;
			}
            int divBottom = div->getHeight();
            if (hScale->isVisible() && !hScale->allowUserPaint()){
                paint->drawLine(hScale->getScaleColor(), 1, 0, 0, divBottom - scaleHeight+ 1, width, divBottom - scaleHeight + 1);
            }
            if (m_firstVisibleIndex >= 0){
                double xScaleWordRight = 0;
                int pureH = m_workingAreaWidth;
				ArrayList<double> scaleSteps = hScale->getScaleSteps();
				int scaleStepsSize = scaleSteps.size();
                HashMap<double, int> scaleStepsMap;
                for (int i = 0; i < scaleStepsSize; i++){
                    scaleStepsMap.put(scaleSteps.get(i), 0);
                }
                if (hScale->getHScaleType() == HScaleType_Number){
                    for (int i = m_firstVisibleIndex; i <= m_lastVisibleIndex; i++){
						double xValue = m_dataSource->getXValue(i);
						if (scaleStepsSize > 0 && !scaleStepsMap.containsKey(xValue)){
                            continue;
                        }
						wchar_t xValueStr[20] = {0};
						_stprintf_s(xValueStr, 19, L"%d",(int)m_dataSource->getXValue(i));
                        int scaleX = (int)getX(i);
                        FCSize xSize = paint->textSize(xValueStr, hScale->getFont());
                        if (scaleX - xSize.cx / 2 - hScale->getInterval() > xScaleWordRight){
                            if (hScale->isVisible() && !hScale->allowUserPaint()){
                                drawThinLine(paint, hScale->getScaleColor(), 1, scaleX, divBottom - scaleHeight + 1,
                                scaleX, divBottom - scaleHeight + 4);
                                drawText(paint, xValueStr, hScale->getTextColor(), hScale->getFont(),
                                    scaleX - xSize.cx / 2, divBottom - scaleHeight + 6);
                            }
                            xScaleWordRight = scaleX + xSize.cx / 2;
                            if (vGrid->isVisible() && !vGrid->allowUserPaint()){
								paint->drawLine(vGrid->getGridColor(), 1, vGrid->getLineStyle(), scaleX, div->getTitleBar()->getHeight(),
                                    scaleX, div->getHeight() - scaleHeight);
                            }
                        }
                    }
                }
                else{
					ArrayList<int> xList;
                    for (int i = m_firstVisibleIndex; i <= m_lastVisibleIndex; i++){
						if (scaleStepsSize > 0){
							if(scaleStepsMap.containsKey(m_dataSource->getXValue(i))){
								scaleStepsMap.remove(m_dataSource->getXValue(i));
								scaleStepsSize = (int)scaleStepsMap.size();
							    xList.add(i);
                                if (scaleStepsSize == 0){
                                    break;
                                }
							}
							else{
								continue;
							}
                        }
                        xList.add(i);
                    }
                    int interval = hScale->getInterval();
                    ArrayList<int> lasts;
                    for (int p = 0; p < 7; p++){
                        int count = 0;
                        int xListSize = (int)xList.size();
                        for (int i = 0; i < xListSize; i++){
                            int pos = xList.get(i);
                            double date = m_dataSource->getXValue(pos);
							DateType dateType = getDateType(p);
                            double lDate = 0;
                            if (pos > 0){
                                lDate = m_dataSource->getXValue(pos - 1);
                            }
							wchar_t xValueStr[20] = {0};
							xValueStr[0] = L'\0';
                            getHScaleDateString(date, lDate, &dateType, xValueStr);
                            int scaleX = (int)getX(pos);
                            if (getDateType(dateType) == p){
                                count++;
                                bool show = true;
                                if (scaleStepsSize == 0){
									int lastsSize = (int)lasts.size();
									for (int j = 0; j < lastsSize; j++){
										int index = lasts.get(j);
                                        int getx = (int)getX(index);
                                        if (index > pos){
                                            if (m_reverseHScale){
                                                if (getx + interval * 2 > scaleX){
                                                    show = false;
                                                    break;
                                                }
                                            }
                                            else{
                                                if (getx - interval * 2 < scaleX){
                                                    show = false;
                                                    break;
                                                }
                                            }
                                        }
                                        else if (index < pos){
                                            if (m_reverseHScale){
                                                if (getx - interval * 2 < scaleX){
                                                    show = false;
                                                    break;
                                                }
                                            }
                                            else{
                                                if (getx + interval * 2 > scaleX){
                                                    show = false;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                if (show){
									lasts.add(pos);
									if (hScale->isVisible() && !hScale->allowUserPaint()){
										FCSize xSize = paint->textSize(xValueStr, hScale->getFont());
										drawThinLine(paint, hScale->getScaleColor(), 1, scaleX, divBottom - scaleHeight + 1,
										scaleX, divBottom - scaleHeight + 4);
										Long dateColor = hScale->getDateColor(getDateType(p));
										drawText(paint, xValueStr, dateColor, hScale->getFont(), scaleX - xSize.cx / 2, divBottom - scaleHeight + 6);
									}
									if (vGrid->isVisible() && !vGrid->allowUserPaint()){
										paint->drawLine(vGrid->getGridColor(), 1, vGrid->getLineStyle(), scaleX, div->getTitleBar()->getHeight(),
											scaleX, div->getHeight() - scaleHeight);
									}
                                    xList.removeAt(i);
                                    i--;
                                    xListSize--;
                                }
                            }
                        }
                        if (count > pureH / 40){
                            break;
                        }
                    }
                    lasts.clear();
					xList.clear();
                }
            }
        }
        if (div->getTitleBar()->showUnderLine()){
            FCSize sizeTitle = paint->textSize(L" ", div->getTitleBar()->getFont());
            paint->drawLine(div->getTitleBar()->getUnderLineColor(), 1, 2, m_leftVScaleWidth, 5 + sizeTitle.cy,
                width - m_rightVScaleWidth, 5 + sizeTitle.cy);
        }
    }

	void FCChart::onPaintIcon(FCPaint *paint){
        if (m_movingShape){
            ChartDiv *div = findDiv(m_movingShape);
            if (div){
				FCPoint actualPoint = getTouchPoint();
                int x = actualPoint.x;
                int y = actualPoint.y;
				if (m_lastTouchClickPoint.x != -1 && m_lastTouchClickPoint.y != -1 &&
                    abs(x - m_lastTouchClickPoint.x) > 5 && abs(y - m_lastTouchClickPoint.y) > 5){
                    FCSize sizeK;
					sizeK.cx = 15;
					sizeK.cy = 16;
					int rectCsX = x - sizeK.cx;
					int rectCsY = y - sizeK.cy;
					int rectCsH = sizeK.cy;
					BarShape *bs = dynamic_cast<BarShape*>(m_movingShape);
					CandleShape *cs = dynamic_cast<CandleShape*>(m_movingShape);
					PolylineShape *tls = dynamic_cast<PolylineShape*>(m_movingShape);
                    if (bs){
						FCRect bsRectA = {rectCsX + 1, rectCsY + 10, rectCsX + 4, rectCsY + rectCsH - 1};
                        paint->fillRect(bs->getUpColor(), bsRectA);
						FCRect bsRectB = {rectCsX + 6, rectCsY + 3, rectCsX + 9, rectCsY + rectCsH - 1};
                        paint->fillRect(bs->getUpColor(), bsRectB);
						FCRect bsRectC = {rectCsX + 11, rectCsY + 8, rectCsX + 14, rectCsY + rectCsH - 1};
                        paint->fillRect(bs->getUpColor(), bsRectC);
                    }
                    else if (cs){
                        paint->drawLine(cs->getDownColor(), 1, 0, rectCsX + 4, rectCsY + 6, rectCsX + 4, rectCsY + rectCsH - 2);
                        paint->drawLine(cs->getUpColor(), 1, 0, rectCsX + 9, rectCsY + 2, rectCsX + 9, rectCsY + rectCsH - 4);
						FCRect csRectA = {rectCsX + 3, rectCsY + 8, rectCsX + 6, rectCsY + 13};
                        paint->fillRect(cs->getDownColor(), csRectA);
						FCRect csRectB = {rectCsX + 8, rectCsY + 4, rectCsX + 11, rectCsY + 9};
                        paint->fillRect(cs->getUpColor(), csRectB);
                    }
                    else if (tls){
                        paint->drawLine(tls->getColor(), 1, 0, rectCsX + 2, rectCsY + 5, rectCsX + 12, rectCsY + 1);
                        paint->drawLine(tls->getColor(), 1, 0, rectCsX + 2, rectCsY + 10, rectCsX + 12, rectCsY + 6);
                        paint->drawLine(tls->getColor(), 1, 0, rectCsX + 2, rectCsY + 15, rectCsX + 12, rectCsY + 11);
                    }
                }
            }
        }
    }

	void FCChart::onPaintPlots(FCPaint *paint, ChartDiv *div){
        ArrayList<FCPlot*> plotsCopy = div->getPlots(SortType_ASC);
        if (plotsCopy.size() > 0){
            int wX = m_workingAreaWidth;
            int wY = div->getWorkingAreaHeight();
            if (wX > 0 && wY > 0){
				FCRect clipRect = {0};
                clipRect.left = m_leftVScaleWidth;
                clipRect.top = div->getTitleBar()->isVisible() ? div->getTitleBar()->getHeight() : 0;
                clipRect.right = clipRect.left + wX;
                clipRect.bottom = clipRect.top + wY;
				for(int p = 0; p < plotsCopy.size(); p++){
					FCPlot *pl = plotsCopy.get(p);
					if(pl->isVisible()){
						paint->setClip(clipRect);
						pl->render(paint);
					}
				}
            }
        }
    }

	void FCChart::onPaintPolyline(FCPaint *paint, ChartDiv *div, PolylineShape *ls){
		int x = 0, y = 0;
		vector<FCPoint> points;
		int ciFieldName = m_dataSource->getColumnIndex(ls->getBaseField());
		int ciClr = m_dataSource->getColumnIndex(ls->getColorField());
		float defaultLineWidth = ls->getWidth();
        if (!isOperating() && m_crossStopIndex != -1){
            if (selectPolyline(div, getTouchPoint(), ls->getBaseField(), ls->getWidth(), ls->getAttachVScale(), m_crossStopIndex)){
                defaultLineWidth += 1;
            }
        }
		for (int i = m_firstVisibleIndex; i <= m_lastVisibleIndex; i++){
			int scaleX = (int)getX(i);
			double value = m_dataSource->get3(i, ciFieldName); 
			if (!FCDataTable::isNaN(value)){
				int lY = (int)getY(div, value, ls->getAttachVScale());
				onPaintPolyline(paint, div, ls->getColor(), ls->getFillColor(), ciClr, defaultLineWidth, ls->getStyle(), value, ls->getAttachVScale(), scaleX, lY, i, &points, &x, &y);
				if(ls->isSelected()){
					int kPInterval = m_maxVisibleRecord / 30;
					if (kPInterval < 2){
						kPInterval = 3;
					}
					if (i % kPInterval == 0){
						if (lY >= div->getTitleBar()->getHeight()
						   && lY <= div->getHeight() - div->getHScale()->getHeight()){
							int lineWidth = (int)ls->getWidth();
							int rl = scaleX - 3 - (lineWidth - 1);
							int rt = lY - 4 - (lineWidth - 1);
							FCRect sRect = {rl, rt, rl + 7 + (lineWidth - 1) * 2, rt + 7 + (lineWidth - 1) * 2};
							paint->fillRect(ls->getSelectedColor(), sRect);
						}
					}
				}
			}
			else{
				onPaintPolyline(paint, div, ls->getColor(), ls->getFillColor(), ciClr, defaultLineWidth, ls->getStyle(), value, ls->getAttachVScale(), scaleX, 0, i, &points, &x, &y);
			}
		}
		points.clear();
	}


	void FCChart::onPaintPolyline(FCPaint *paint, ChartDiv *div, Long lineColor, Long fillColor, int ciClr,
            float lineWidth, PolylineStyle lineStyle, double value, AttachVScale attachVScale,
            int scaleX, int lY, int i, vector<FCPoint> *points, int*x, int *y){
		if (!FCDataTable::isNaN(value)){
			if (m_dataSource->rowsCount() == 1){
				int cwidth = (int)(m_hScalePixel / 4);
				FCPoint startP = {scaleX - cwidth, lY};
				FCPoint endP = {scaleX - cwidth + cwidth * 2 + 1, lY};
				points->push_back(startP);
				points->push_back(endP);
			}
			else{
				int newX = 0;
				int newY = 0;
				if (i == m_firstVisibleIndex){
					*x = scaleX;
					*y = lY;
				}
				newX = scaleX;
				newY = lY;
				if (*y <= div->getHeight() - div->getHScale()->getHeight() + 1
				 && *y >= div->getTitleBar()->getHeight() - 1
				 && newY < div->getHeight() - div->getHScale()->getHeight() + 1
				 && newY >= div->getTitleBar()->getHeight() - 1){
					if (*x != newX || *y != newY){
						if(points->size()==0){
							FCPoint startP = {*x, *y};
							points->push_back(startP);
						}
						FCPoint endP = {newX, newY};
						points->push_back(endP);
					}
				}
				*x = newX;
				*y = newY;
			}
			if (ciClr != -1){
				double defineColor = m_dataSource->get3(i, ciClr);
				if(!FCDataTable::isNaN(defineColor)){
					lineColor = (Long)defineColor;
				}
			}
		}
		if (points->size() > 0){
			Long lColor = lineColor;
			if (ciClr != -1 && i > 0){
				double defineColor = m_dataSource->get2(i - 1, ciClr);
				if(!FCDataTable::isNaN(defineColor)){
					lColor = (Long)defineColor;
				}
			}
            if (lineColor != lColor || i == m_lastVisibleIndex){
				FCPoint *pts = new FCPoint[points->size()];
				int psize = (int)points->size();
				for(int j = 0;j < psize;j++){
					pts[j] = (*points)[j];
				}
				Long drawColor = lineColor;
				int width = (int)(m_hScalePixel / 2);
                if (lColor != lineColor){
					drawColor = lColor;
                }
				switch (lineStyle){
                    case PolylineStyle_Cycle:{
							int ew = (width - 1) > 0 ? (width - 1) : 1;
							int psize = (int)points->size();
							for (int j = 0; j < psize; j++){
								FCPoint point = (*points)[j];
								FCRect pRect = {point.x - ew / 2,
									point.y - ew / 2, point.x + ew / 2, point.y + ew / 2};
								paint->drawEllipse(drawColor, 1, 0, pRect);
							}
							break;
						}
                    case PolylineStyle_DashLine:{
							paint->drawPolyline(lColor, (float)lineWidth, 1, pts, (int)points->size());
                            break;
                        }
                    case PolylineStyle_DotLine:{
							paint->drawPolyline(lColor, (float)lineWidth, 2, pts, (int)points->size());
                            break;
                        }
                    case PolylineStyle_SolidLine:{
							paint->drawPolyline(lColor, (float)lineWidth, 0, pts, (int)points->size());
                            break;
                        }
                }
				delete[] pts;
				pts = 0;
				if (fillColor != FCColor_None){
                    int zy = (int)getY(div, 0, attachVScale);
                    int th = div->getTitleBar()->isVisible() ? div->getTitleBar()->getHeight() : 0;
                    int hh = div->getHScale()->isVisible() ? div->getHScale()->getHeight() : 0;
                    if (zy < th) zy = th;
                    else if (zy > div->getHeight() - hh) zy = div->getHeight() - hh;
					FCPoint ps = {(*points)[0].x, zy};
					points->insert(points->begin(), ps);
					FCPoint pe = {points->at((int)points->size() - 1).x, zy};
					points->push_back(pe);
					int psize = (int)points->size();
					FCPoint *ptsf = new FCPoint[psize];
					for(int j = 0; j < psize; j++){
						ptsf[j] = (*points)[j];
					}
                    paint->fillPolygon(fillColor, ptsf, psize);
					delete[] ptsf;
					ptsf =  0;
                }
                points->clear();
            }
        }
	}

	void FCChart::onPaintResizeLine(FCPaint *paint){
        if (m_hResizeType > 0){
            FCPoint mp = getTouchPoint();
            ArrayList<ChartDiv*> divsCopy = getDivs();
			for(int d = 0; d < divsCopy.size(); d++){
				ChartDiv *div = divsCopy.get(d);
                if (mp.x < 0) mp.x = 0;
                if (mp.x > getWidth()) mp.x = getWidth();
				paint->drawLine(FCColor::reverse(paint, div->getBackColor()), 1, 2, mp.x, 0, mp.x, getWidth());
            }
        }
        if (m_userResizeDiv){
            FCPoint mp = getTouchPoint();
            ChartDiv *nextCP = 0;
            bool rightP = false;
            ArrayList<ChartDiv*> divsCopy = getDivs();
			for(int d = 0; d < divsCopy.size(); d++){
				ChartDiv *cDiv = divsCopy.get(d);
                if (rightP){
                    nextCP = cDiv;
                    break;
                }
                if (cDiv == m_userResizeDiv){
                    rightP = true;
                }
            }
            bool drawFlag = false;
			FCRect uRect = m_userResizeDiv->getBounds();
            if (mp.x >= uRect.left && mp.x <= uRect.right && mp.y >= uRect.top && mp.y <= uRect.bottom){
                drawFlag = true;
            }
            else{
                if (nextCP){
					FCRect nRect = nextCP->getBounds();
					if (mp.x >= nRect.left && mp.x <= nRect.right && mp.y >= nRect.top && mp.y <= nRect.bottom){
                    	drawFlag = true;
					}
                }
            }
            if (drawFlag){
                paint->drawLine(FCColor::reverse(paint, m_userResizeDiv->getBackColor()), 1, 2, 0, mp.y, getWidth(), mp.y);
            }
        }
    }

	void FCChart::onPaintSelectArea(FCPaint *paint, ChartDiv *div){
		SelectArea *selectArea = div->getSelectArea();
        if (selectArea->isVisible()){
            FCRect bounds = selectArea->getBounds();
			if(selectArea->allowUserPaint()){
				selectArea->onPaint(paint, div, bounds);
			}
			else{
				if ((bounds.right - bounds.left) >= 5 && (bounds.bottom - bounds.top) >= 5){
					paint->fillRect(selectArea->getBackColor(), bounds);
					paint->drawRect(selectArea->getLineColor(), 1, 0, bounds);
				}
			}
        }
    }

	void FCChart::onPaintShapes(FCPaint *paint, ChartDiv *div){
		ArrayList<BaseShape*> sortedBs = div->getShapes(SortType_ASC);
		for(int b = 0; b < sortedBs.size(); b++){
			BaseShape *bShape = sortedBs.get(b);
			if (!bShape->isVisible() || (div->getVScale(bShape->getAttachVScale())->getVisibleMax() - div->getVScale(bShape->getAttachVScale())->getVisibleMin()) == 0){
                continue;
            }
			if(bShape->allowUserPaint()){
				FCRect rect = {0, 0, div->getWidth(), div->getHeight()};
				bShape->onPaint(paint, div, rect);
			}
			else{
				BarShape *bs = dynamic_cast<BarShape*>(bShape);
				CandleShape *cs = dynamic_cast<CandleShape*>(bShape);
				PolylineShape *ls = dynamic_cast<PolylineShape*>(bShape);
				TextShape *ts = dynamic_cast<TextShape*>(bShape);
				if (ls){
					onPaintPolyline(paint, div, ls);
				}
				else if(ts){
					onPaintText(paint, div, ts);
				}
				else if(bs){
					onPaintBar(paint, div, bs);
				}
				else if (cs){
					onPaintCandle(paint, div, cs);
				}
			}
        }
    }

	void FCChart::onPaintText(FCPaint *paint, ChartDiv *div, TextShape *ts){
		String drawText = ts->getText();
		if (drawText.length() == 0){
            return;
        }
		int ciFieldName = m_dataSource->getColumnIndex(ts->getFieldName());
		int ciClr = m_dataSource->getColumnIndex(ts->getColorField());
		int ciStyle = m_dataSource->getColumnIndex(ts->getStyleField());
		for (int i = m_firstVisibleIndex; i <= m_lastVisibleIndex; i++){
			int style = 0;
			if (ciStyle != -1){
				double defineStyle = m_dataSource->get3(i, ciStyle);
				if (!FCDataTable::isNaN(defineStyle)){
					style = (int)defineStyle;
				}
			}
			if (style == -10000){
				continue;
			}
			double value = m_dataSource->get3(i, ciFieldName);
			if (!FCDataTable::isNaN(value)){
				int scaleX = (int)getX(i);
				int y = (int)getY(div, value, ts->getAttachVScale());
				Long textColor = ts->getTextColor();
				if (ciClr != -1){
					double defineColor = m_dataSource->get3(i, ciClr);
					if(!FCDataTable::isNaN(defineColor)){
						textColor = (Long)defineColor;
					}
				}
				FCSize tSize = paint->textSize(drawText.c_str(), ts->getFont());
				this->drawText(paint, drawText.c_str(), textColor, ts->getFont(), scaleX - tSize.cx / 2, y - tSize.cy / 2);
			}
		}
	}

	void FCChart::onPaintTitle(FCPaint *paint, ChartDiv *div){
		ChartTitleBar *titleBar = div->getTitleBar();
		int width = div->getWidth(), height = div->getHeight();
        if (titleBar->isVisible()){
			FCRect tbRect = {0, 0, width, height};
			if(titleBar->allowUserPaint()){
				titleBar->onPaint(paint, div, tbRect);
			}
			else{
				int titleLeftPadding = m_leftVScaleWidth;
				int rightPadding = width - m_rightVScaleWidth - 2;
				FCSize divNameSize = paint->textSize(titleBar->getText().c_str(), titleBar->getFont());
				if (titleLeftPadding + divNameSize.cx <= width - m_rightVScaleWidth){
					drawText(paint, titleBar->getText().c_str(), titleBar->getTextColor(), titleBar->getFont(), titleLeftPadding, 2);
				}
				titleLeftPadding += divNameSize.cx + 2;
				if (m_firstVisibleIndex >= 0 && m_lastVisibleIndex >= 0){
					int displayIndex = m_lastVisibleIndex;
					if(displayIndex > m_lastUnEmptyIndex){
						displayIndex = m_lastUnEmptyIndex;
					}
					if (m_showCrossLine){
						if (m_crossStopIndex <= m_lastVisibleIndex){
							displayIndex = m_crossStopIndex;
						}
						if (m_crossStopIndex < 0){
							displayIndex = 0;
						}
						if (m_crossStopIndex >= m_lastVisibleIndex){
							displayIndex = m_lastVisibleIndex;
						}
					}
					int curLength = 1;
					int tTop = 2;
					ArrayList<ChartTitle*> titles = titleBar->Titles;
					wchar_t *showTitle = 0;
					for(int t = 0; t < titles.size(); t++){
						ChartTitle *title = titles.get(t);
						if (!title->isVisible() || title->getFieldTextMode() == TextMode_None){
							continue;
						}
						showTitle = L"";
						wchar_t p[100] = {0};
						wchar_t digitStr[100] = {0};
						wchar_t p2[100] = {0};
						double value = m_dataSource->get2(displayIndex, title->getFieldName());
						if (FCDataTable::isNaN(value)){
							value = 0;
						}
						if (title->getFieldTextMode() != TextMode_Value){
							FCStr::contact(p, title->getFieldText().c_str(), title->getFieldTextSeparator().c_str(), L"");
							showTitle = p;
						}
						if (title->getFieldTextMode() != TextMode_Field){
							if (!FCDataTable::isNaN(value)){
								int digit = title->getDigit();
								FCStr::getValueByDigit(value, digit, digitStr);
								FCStr::contact(p2, showTitle, digitStr, L"");
								showTitle = p2;
							}
						}
						FCSize conditionSize = paint->textSize(showTitle, titleBar->getFont());
						if (titleLeftPadding + conditionSize.cx + 8 > rightPadding){
							curLength++;
							if (curLength <= titleBar->getMaxLine()){
								tTop += conditionSize.cy + 2;
								titleLeftPadding = m_leftVScaleWidth;
								rightPadding = width - m_rightVScaleWidth;
							}
							else{
								break;
							}
							if (tTop + conditionSize.cy >= div->getHeight() - div->getHScale()->getHeight()){
								break;
							}
						}
						if (titleLeftPadding <= rightPadding){
							drawText(paint, showTitle, title->getTextColor(), titleBar->getFont(), titleLeftPadding, tTop);
							titleLeftPadding += conditionSize.cx + 8;
						}
					}
				}
			}
        }
    }

    void FCChart::onPaintToolTip(FCPaint *paint){
        if (!m_showingToolTip){
            return;
        }
		BaseShape *bs = selectShape(getTouchOverIndex(), 0);
        if (bs){
            FCPoint touchP = getTouchPoint();
            ChartDiv *touchOverDiv = getTouchOverDiv();
            int digit = touchOverDiv->getVScale(bs->getAttachVScale())->getDigit();
            if (!touchOverDiv) return;
            int index = getIndex(touchP);
            if (index < 0) return;
			ChartToolTip *toolTip = touchOverDiv->getToolTip();
			if(toolTip->allowUserPaint()){
				FCRect toolRect = {0, 0, getWidth(), getHeight()};
				toolTip->onPaint(paint, touchOverDiv, toolRect);
				return;
			}
            int pWidth = 0;
            int pHeight = 0;
			FCFont *toolTipFont = toolTip->getFont();
            double xValue = m_dataSource->getXValue(index);
			int sLeft = touchOverDiv->getLeft(), sTop = touchOverDiv->getTop();
			for(int t = 0; t < 2; t++){
				int x = touchP.x + 10;
				int y = touchP.y + 2;
				if(t == 0){
					sLeft = x + 2;
					sTop = y;
				}
				FCSize xValueSize = {0};
				if (touchOverDiv->getHScale()->getHScaleType() == HScaleType_Date){
					wchar_t formatDate[100] = {0};
					FCStr::getFormatDate(xValue, formatDate);
					wchar_t p1[100] = {0};
					FCStr::contact(p1, m_hScaleFieldText.c_str(), L":", formatDate);
					xValueSize = paint->textSize(p1, toolTipFont);
					pHeight = xValueSize.cy;
					if(t == 1){
						drawText(paint, p1, toolTip->getTextColor(), toolTipFont, sLeft, sTop);
					}
				}
				else if (touchOverDiv->getHScale()->getHScaleType() == HScaleType_Number){
					wchar_t p2[100] = {0};
					wchar_t xValueStr[20] = {0};
					_stprintf_s(xValueStr, 19, L"%d", (int)xValue);
					FCStr::contact(p2,m_hScaleFieldText.c_str(), L":", xValueStr);
					xValueSize = paint->textSize(p2, toolTipFont);
					pHeight = xValueSize.cy;
					if(t == 1){
						drawText(paint, p2, toolTip->getTextColor(), toolTipFont, sLeft ,sTop);
					}
				}
				sTop += xValueSize.cy;
				int fieldsLength = 0;
				int *fields = bs->getFields(&fieldsLength);
				if (fieldsLength > 0){
					for (int i = 0; i < fieldsLength; i++){
						String fieldText = bs->getFieldText(fields[i]);
						double value = 0;
						if(index>=0){
							value = m_dataSource->get2(index, fields[i]);
						}
						wchar_t valueDigit[100] = {0};
						FCStr::getValueByDigit(value, digit, valueDigit);
						wchar_t p[100] = {0};
						FCStr::contact(p, fieldText.c_str(), L":", valueDigit);
						if(t == 1){
							drawText(paint, p, toolTip->getTextColor(), toolTipFont, sLeft, sTop);
						}
						FCSize strSize = paint->textSize(p, toolTipFont);
						if(i == 0){
							pWidth = xValueSize.cx;
						}
						if(xValueSize.cx > pWidth){
							pWidth = xValueSize.cx;
						}
						if(strSize.cx > pWidth){
							pWidth = strSize.cx;
						}
						sTop += strSize.cy;
						pHeight += strSize.cy;
					}
				}
				if(fields){
					delete[] fields;
					fields = 0;
				}
				if(t == 0){
					int width = getWidth(), height = getHeight();
					pWidth += 4;
					pHeight += 1;
					if (x + pWidth > width){
						x = width - pWidth;
						if (x < 0){
							x = 0;
						}
					}
					if (y + pHeight > height){
						y = height - pHeight;
						if (y < 0){
							y = 0;
						}
					}
					sLeft = x;
					sTop = y;
					int rectPX = x, rectPY = y, rectPW = pWidth, rectPH = pHeight;
					if (rectPW > 0 && rectPH > 0){
						FCRect rectP = {rectPX, rectPY, rectPX + rectPW, rectPY + rectPH};
						paint->fillRect(toolTip->getBackColor(), rectP);
						paint->drawRect(toolTip->getBorderColor(), 1, 0, rectP);
					}
				}
			}
        }
    }

	void FCChart::onPaintVScale(FCPaint *paint, ChartDiv *div){
		int divBottom = div->getHeight();
		vector<int> gridYList;
        bool leftGridIsShow = false;
		int width = getWidth();
        if (m_leftVScaleWidth > 0){
			VScale *leftVScale = div->getLeftVScale();
			ScaleGrid *hGrid = div->getHGrid();
			bool paintV = true, paintG = true;
			if(leftVScale->allowUserPaint()){
				FCRect leftVRect = {0, 0, m_leftVScaleWidth, divBottom};
				leftVScale->onPaint(paint, div, leftVRect);
				paintV = false;
			}
			if(hGrid->allowUserPaint()){
				FCRect hGridRect = {0, 0, width, divBottom};
				hGrid->onPaint(paint, div, hGridRect);
				paintG = false;
			}
			if(paintV || paintG){
				if (paintV && m_leftVScaleWidth <= width){
					paint->drawLine(leftVScale->getScaleColor(), 1, 0, m_leftVScaleWidth, 0, m_leftVScaleWidth, divBottom - div->getHScale()->getHeight());
				}
				FCFont *leftYFont = leftVScale->getFont();
				FCSize leftYSize = paint->textSize(L" ", leftYFont);
				double min = leftVScale->getVisibleMin();
				double max = leftVScale->getVisibleMax();
				if (min == 0 && max == 0){
					VScale *rightVScale = div->getRightVScale();
					if (rightVScale->getVisibleMin() != 0 || rightVScale->getVisibleMax() != 0){
						min = rightVScale->getVisibleMin();
						max = rightVScale->getVisibleMax();
						FCPoint point1 = {0, div->getTop() + div->getTitleBar()->getHeight() };
						double value1 = getNumberValue(div, point1, AttachVScale_Right);
						FCPoint point2 = {0, div->getBottom() - div->getHScale()->getHeight()};
						double value2 = getNumberValue(div, point2, AttachVScale_Right);
						max = max(value1, value2);
						min = min(value1, value2);
					}
				}
				else{
					FCPoint point1 = {0, div->getTop() + div->getTitleBar()->getHeight() };
					double value1 = getNumberValue(div, point1, AttachVScale_Left);
					FCPoint point2 = {0, div->getBottom() - div->getHScale()->getHeight()};
					double value2 = getNumberValue(div, point2, AttachVScale_Left);
					max = max(value1, value2);
					min = min(value1, value2);
				}
				ArrayList<double> scaleStepList = leftVScale->getScaleSteps();
				if(scaleStepList.size() == 0){
					scaleStepList = getVScaleStep(max, min, div, leftVScale);
				}
				float lY = -1;
				int stepSize = (int)scaleStepList.size();
				for (int i = 0; i < stepSize; i++){
					double lValue = scaleStepList.get(i) / leftVScale->getMagnitude();
					if (lValue != 0 && leftVScale->getType() == VScaleType_Percent){
						double baseValue = getVScaleBaseValue(div, leftVScale, m_firstVisibleIndex) / leftVScale->getMagnitude();
						lValue = 100 * (lValue - baseValue * leftVScale->getMagnitude()) / lValue;
					}
					wchar_t *numberStr = 0;
					wchar_t number[100] = {0};
					FCStr::getValueByDigit(lValue, leftVScale->getDigit(), number);
					numberStr = number;
					wchar_t p[100] = {0};
					if (leftVScale->getType() == VScaleType_Percent){
						FCStr::contact(p, numberStr, L"%", L"");
						numberStr = p;
					}
					int y = (int)getY(div, scaleStepList.get(i), AttachVScale_Left);
					leftYSize = paint->textSize(numberStr, leftYFont);
					if (y - leftYSize.cy / 2 < 0 || y + leftYSize.cy / 2 > divBottom){
						continue;
					}
					if (hGrid->isVisible() && paintG){
						leftGridIsShow = true;
						if(find(gridYList.begin(), gridYList.end(), y) == gridYList.end()){
							gridYList.push_back(y);
							paint->drawLine(hGrid->getGridColor(), 1, hGrid->getLineStyle(), m_leftVScaleWidth, y, width - m_rightVScaleWidth, y);
						}
					}
					if(paintV){
						drawThinLine(paint, leftVScale->getScaleColor(), 1, m_leftVScaleWidth - 4, y, m_leftVScaleWidth, y);
						if (leftVScale->isReverse()){
							if (lY != -1 && y - leftYSize.cy / 2 < lY){
								continue;
							}
							lY = y + (float)leftYSize.cy / 2;
						}
						else{
							if (lY != -1 && y + leftYSize.cy / 2 > lY){
								continue;
							}
							lY = y - (float)leftYSize.cy / 2;
						}
						Long scaleTextColor = leftVScale->getTextColor();
						Long scaleTextColor2 = leftVScale->getTextColor2();
						if (leftVScale->getType() != VScaleType_Percent){
							if (scaleTextColor2 != FCColor_None && lValue < 0){
                                			
								scaleTextColor = scaleTextColor2;
                    
						       }
						}
						else{
							if (scaleTextColor2 != FCColor_None && scaleStepList.get(i) < leftVScale->getMidValue()){
								scaleTextColor = scaleTextColor2;
							}
						}
						if (leftVScale->getType() != VScaleType_Percent && leftVScale->getNumberStyle() == NumberStyle_Underline){
							ArrayList<String> splits = FCStr::split(numberStr, L".");
							if (splits.size() >= 1){
								drawText(paint, splits.get(0).c_str(), scaleTextColor, leftYFont, m_leftVScaleWidth - 10 - leftYSize.cx, y - leftYSize.cy / 2);
							}
							if (splits.size() >= 2){
								FCSize decimalSize = paint->textSize(splits.get(0).c_str(), leftYFont);
								FCSize size2 = paint->textSize(splits.get(1).c_str(), leftYFont);
								drawText(paint, splits.get(1).c_str(), scaleTextColor, leftYFont, m_leftVScaleWidth - 10 - leftYSize.cx
									+ decimalSize.cx, y - leftYSize.cy / 2);
								drawThinLine(paint, scaleTextColor, 1, m_leftVScaleWidth - 10 - leftYSize.cx
								+ decimalSize.cx, y + leftYSize.cy / 2,
								m_leftVScaleWidth - 10 - leftYSize.cx + decimalSize.cx + size2.cx - 1, y + leftYSize.cy / 2);
							}
						}
						else{
							drawText(paint, numberStr, scaleTextColor, leftYFont, m_leftVScaleWidth - 10 - leftYSize.cx, y - leftYSize.cy / 2);
						}
						if (leftVScale->getType() == VScaleType_GoldenRatio){
							String goldenRatio;
							if (i == 1) goldenRatio = L"19.1%";
							else if (i == 2) goldenRatio = L"38.2%";
							else if (i == 4) goldenRatio = L"61.8%";
							else if (i == 5) goldenRatio = L"80.9%";
							if ((int)goldenRatio.length() > 0){
								FCSize goldenRatioSize = paint->textSize(goldenRatio.c_str(), leftYFont);
								drawText(paint, goldenRatio.c_str(), scaleTextColor, leftYFont, m_leftVScaleWidth - 10 - goldenRatioSize.cx, y + leftYSize.cy / 2);
							}
						}
					}
				}
				if (leftVScale->getMagnitude() != 1 && paintV){
					wchar_t magnitude[100] = {0};
					_stprintf_s(magnitude, 99, L"%d", leftVScale->getMagnitude());
					wchar_t str[100] = {0};
					FCStr::contact(str, L"X", magnitude, L"");
					FCSize sizeF = paint->textSize(str, leftYFont);
					int x = m_leftVScaleWidth - sizeF.cx - 6;
					int y = div->getHeight() - div->getHScale()->getHeight() - sizeF.cy - 2;
					FCRect rectL = {x - 1, y - 1, x + sizeF.cx, y + sizeF.cy};
					paint->drawRect(leftVScale->getScaleColor(), 1, 0, rectL);
					drawText(paint, str, leftVScale->getTextColor(), leftYFont, x, y);
				}
			}
        }
        if (m_rightVScaleWidth > 0){
			VScale *rightVScale = div->getRightVScale();
			ScaleGrid *hGrid = div->getHGrid();
			bool paintV = true, paintG = true;
			if(rightVScale->allowUserPaint()){
				FCRect leftVRect = {width - m_rightVScaleWidth, 0, width, divBottom};
				rightVScale->onPaint(paint, div, leftVRect);
				paintV = false;
			}
			if(hGrid->allowUserPaint()){
				FCRect hGridRect = {0, 0, width, divBottom};
				hGrid->onPaint(paint, div, hGridRect);
				paintG = false;
			}
			if(paintV || paintG){
				if (paintV && width - m_rightVScaleWidth >= m_leftVScaleWidth){
					paint->drawLine(rightVScale->getScaleColor(), 1, 0, width - m_rightVScaleWidth, 0,width - m_rightVScaleWidth, divBottom - div->getHScale()->getHeight());
				}
				FCFont *rightYFont = rightVScale->getFont();
				FCSize rightYSize = paint->textSize(L" ", rightYFont);
				double min = rightVScale->getVisibleMin();
				double max = rightVScale->getVisibleMax();
				if (min == 0 && max == 0){
					VScale *leftVScale = div->getLeftVScale();
					if (leftVScale->getVisibleMin() != 0 || leftVScale->getVisibleMax() != 0){
						min = leftVScale->getVisibleMin();
						max = leftVScale->getVisibleMax();
						FCPoint point1 = {0, div->getTop() + div->getTitleBar()->getHeight() };
						double value1 = getNumberValue(div, point1, AttachVScale_Left);
						FCPoint point2 = {0, div->getBottom() - div->getHScale()->getHeight()};
						double value2 = getNumberValue(div, point2, AttachVScale_Left);
						max = max(value1, value2);
						min = min(value1, value2);
					}
				}
				else{
					FCPoint point1 = {0, div->getTop() + div->getTitleBar()->getHeight() };
					double value1 = getNumberValue(div, point1, AttachVScale_Right);
					FCPoint point2 = {0, div->getBottom() - div->getHScale()->getHeight()};
					double value2 = getNumberValue(div, point2, AttachVScale_Right);
					max = max(value1, value2);
					min = min(value1, value2);
				}
				ArrayList<double> scaleStepList = rightVScale->getScaleSteps();
				if(scaleStepList.size() == 0){
					scaleStepList = getVScaleStep(max, min, div, rightVScale);
				}
				float lY = -1;
				int stepSize = (int)scaleStepList.size();
				for (int i = 0; i < stepSize; i++){
					double rValue = scaleStepList.get(i) / rightVScale->getMagnitude();
					if (rValue != 0 && rightVScale->getType() == VScaleType_Percent){
						double baseValue = getVScaleBaseValue(div, rightVScale, m_lastVisibleIndex) / rightVScale->getMagnitude();
						rValue = 100 * (rValue - baseValue * rightVScale->getMagnitude()) / rValue;
					}
					wchar_t *numberStr = 0;
					wchar_t number[100] = {0};
					FCStr::getValueByDigit(rValue, rightVScale->getDigit(), number);
					numberStr = number;
					wchar_t p[100] = {0};
					if (rightVScale->getType() == VScaleType_Percent){
						FCStr::contact(p, numberStr, L"%", L"");
						numberStr = p;
					}
					int y = (int)getY(div, scaleStepList.get(i), AttachVScale_Right);
					rightYSize = paint->textSize(numberStr, rightYFont);
					if (y - rightYSize.cy / 2 < 0 || y + rightYSize.cy / 2 > divBottom){
						continue;
					}
					if (hGrid->isVisible() && paintG && !leftGridIsShow){
						if(find(gridYList.begin(), gridYList.end(), y) == gridYList.end()){
							gridYList.push_back(y);
							paint->drawLine(hGrid->getGridColor(), 1, hGrid->getLineStyle(), m_leftVScaleWidth, y, width - m_rightVScaleWidth, y);
						}
					}
					if(paintV){
						drawThinLine(paint, rightVScale->getScaleColor(), 1, width - m_rightVScaleWidth, y, width - m_rightVScaleWidth + 4, y);
						if (rightVScale->isReverse()){
							if (lY != -1 && y - rightYSize.cy / 2 < lY){
								continue;
							}
							lY = y + (float)rightYSize.cy / 2;
						}
						else{
							if (lY != -1 && y + rightYSize.cy / 2 > lY){
								continue;
							}
							lY = y - (float)rightYSize.cy / 2;
						}
						Long scaleTextColor = rightVScale->getTextColor();
						Long scaleTextColor2 = rightVScale->getTextColor2();
						if (rightVScale->getType() != VScaleType_Percent){
							if (scaleTextColor2 != FCColor_None && rValue < 0){
                                			
								scaleTextColor = scaleTextColor2;
                    
						       }
						}
						else{
							if (scaleTextColor2 != FCColor_None && scaleStepList.get(i) < rightVScale->getMidValue()){
								scaleTextColor = scaleTextColor2;
							}
						}
						if (rightVScale->getType() != VScaleType_Percent && rightVScale->getNumberStyle() == NumberStyle_Underline){
							ArrayList<String> splits = FCStr::split(numberStr, L".");
							if (splits.size() >= 1){
								drawText(paint, splits.get(0).c_str(), scaleTextColor, rightYFont, width - m_rightVScaleWidth + 10,
								y - rightYSize.cy / 2);
							}
							if (splits.size() >= 2){
								FCSize decimalSize = paint->textSize(splits.get(0).c_str(), rightYFont);
								FCSize size2 = paint->textSize(splits.get(1).c_str(), rightYFont);
								drawText(paint, splits.get(1).c_str(), scaleTextColor, rightYFont, width - m_rightVScaleWidth + 10
									+ decimalSize.cx, y - rightYSize.cy / 2);
								drawThinLine(paint, scaleTextColor, 1, width - m_rightVScaleWidth + 10
								+ decimalSize.cx, y + rightYSize.cy / 2,
								width - m_rightVScaleWidth + 10 + decimalSize.cx + size2.cx - 1, y + rightYSize.cy / 2);
							}
						}
						else{
							drawText(paint, numberStr, scaleTextColor, rightYFont, width - m_rightVScaleWidth + 10,
							y - rightYSize.cy / 2);
						}
						if (rightVScale->getType() == VScaleType_GoldenRatio){
							String goldenRatio;
							if (i == 1) goldenRatio = L"19.1%";
							else if (i == 2) goldenRatio = L"38.2%";
							else if (i == 4) goldenRatio = L"61.8%";
							else if (i == 5) goldenRatio = L"80.9%";
							if ((int)goldenRatio.length() > 0){
								drawText(paint, goldenRatio.c_str(), scaleTextColor, rightYFont, width - m_rightVScaleWidth + 10,
								y + rightYSize.cy / 2);
							}
						}
					}
				}
				if (rightVScale->getMagnitude() != 1 && paintV){
					wchar_t magnitude[100] = {0};
					_stprintf_s(magnitude, 99, L"%d", rightVScale->getMagnitude());
					wchar_t str[100] = {0};
					FCStr::contact(str, L"X", magnitude, L"");
					FCSize sizeF = paint->textSize(str, rightYFont);
					int x = width - m_rightVScaleWidth + 6;
					int y = div->getHeight() - div->getHScale()->getHeight() - sizeF.cy - 2;
					FCRect rectR = {x - 1, y - 1, x + sizeF.cx, y + sizeF.cy};
					paint->drawRect(rightVScale->getScaleColor(), 1, 0, rectR);
					drawText(paint, str, rightVScale->getTextColor(), rightYFont, x, y);
				}
			}
        }
    }

	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	void FCChart::correctVisibleRecord(int dataCount, int *first, int *last){
		if (dataCount > 0){
			if (*first == -1){
				*first = 0;
			}
			if (*last == -1){
				*last = 0;
			}
			if (*first > *last){
				*first = *last;
			}
			if (*last < *first){
				*last = *first;
			}
		}
		else{
			*first = -1;
			*last = -1;
		}
	}

	void FCChart::getCandleMaxStringPoint(float scaleX, float scaleY, float stringWidth, float stringHeight, int actualWidth,
		int leftVScaleWidth, int rightVScaleWidth, float *x, float *y){
		if(scaleX < leftVScaleWidth + stringWidth){
			*x = scaleX;
		}
		else if(scaleX > actualWidth - rightVScaleWidth - stringWidth){
			*x = scaleX - stringWidth;
		}
		else{
			if(scaleX < actualWidth / 2){
				*x = scaleX - stringWidth;
			}
			else{
				*x = scaleX;
			}
		}
		*y = scaleY + stringHeight / 2;
	}


	void FCChart::getCandleMinStringPoint(float scaleX, float scaleY, float stringWidth, float stringHeight, int actualWidth,
		int leftVScaleWidth, int rightVScaleWidth, float *x, float *y){
		if(scaleX < leftVScaleWidth + stringWidth){
			*x = scaleX;
		}
		else if(scaleX > actualWidth - rightVScaleWidth - stringWidth){
			*x = scaleX - stringWidth;
		}
		else{
			if(scaleX < actualWidth / 2){
				*x = scaleX - stringWidth;
			}
			else{
				*x = scaleX;
			}
		}
		*y = scaleY - stringHeight * 3 / 2;
	}

	int FCChart::getChartIndex(int x, int leftScaleWidth, double hScalePixel, int firstVisibleIndex){
		return (int)((x - leftScaleWidth) / hScalePixel + firstVisibleIndex);
	}

	float FCChart::getUpCandleHeight(double close, double open, double max, double min, float divPureV){
		if(close - open == 0){
			return 1;
		}
		else{
			return (float)((close - open) / (max - min) * divPureV);
		}
	}

	float FCChart::getDownCandleHeight(double close, double open, double max, double min, float divPureV){
		if(close - open == 0){
			return 1;
		}
		else{
			return (float)((open - close) / (max - min) * divPureV);
		}
	}

	void FCChart::scrollLeft(int step, int dateCount, double hScalePixel, int pureH, int *fIndex, int *lIndex){
		int max = getMaxVisibleCount(hScalePixel, pureH);
		int right = -1;
		if(dateCount > max){
			right = max - (*lIndex - *fIndex);
			if(right > 1){
				*fIndex = *lIndex - max + 1;
				if(*fIndex > *lIndex){
					*fIndex = *lIndex;
				}
			}
			else{
				if(*fIndex-step >= 0){
					*fIndex = *fIndex - step;
					*lIndex = *lIndex - step;
				}
				else{
					if(*fIndex != 0){
						*lIndex = *lIndex - *fIndex;
						*fIndex = 0;
					}
				}
			}
		}
	}

	void FCChart::scrollRight(int step, int dataCount, double hScalePixel, int pureH, int *fIndex, int *lIndex){
		int max = getMaxVisibleCount(hScalePixel, pureH);
		if(dataCount > max){
			if(*lIndex < dataCount-1){
				if(*lIndex + step>dataCount-1){
					*fIndex = *fIndex + dataCount - *lIndex;
					*lIndex = dataCount-1;
				}
				else{
					*fIndex = *fIndex + step;
					*lIndex = *lIndex + step;
				}
			}
			else{
				*fIndex = *lIndex - (int)(max * 0.9);
				if(*fIndex > *lIndex){
					*fIndex = *lIndex;
				}
			}
		}
	}

	double FCChart::getVScaleValue(int y, double max, double min, float vHeight){
		double every = (max - min) / vHeight;
		return max - y * every;
	}

	int FCChart::resetCrossOverIndex(int dataCount, int maxVisibleRecord, int crossStopIndex, int firstL, int lastL){
		if(dataCount > 0 && dataCount >= maxVisibleRecord){
			if(crossStopIndex < firstL){
				crossStopIndex = firstL;
			}
			if(crossStopIndex > lastL){
				crossStopIndex = lastL;
			}
		}
		return crossStopIndex;
	}

	void FCChart::update(){
		if(!getNative()){
			return;
		}
		FCView::update();
		FCRect bounds = getBounds();
		int width = bounds.right - bounds.left;
		int height = bounds.bottom - bounds.top;
		m_workingAreaWidth = width - m_leftVScaleWidth - m_rightVScaleWidth - m_blankSpace - 1;
		if(m_workingAreaWidth < 0){
			m_workingAreaWidth = 0;
		}
        int locationY = 0;
        float sumPercent = 0;
        ArrayList<ChartDiv*> divsCopy = getDivs();
		for(int d = 0; d < divsCopy.size(); d++){
			ChartDiv *div = divsCopy.get(d);
            sumPercent += div->getVerticalPercent();
        }
        if (sumPercent > 0){
			for(int d = 0; d < divsCopy.size(); d++){
				ChartDiv *cDiv = divsCopy.get(d);
				FCRect rect = {0, locationY, width, locationY + (int)(height * cDiv->getVerticalPercent() / sumPercent)};
				cDiv->setBounds(rect);
                cDiv->setWorkingAreaHeight(cDiv->getHeight() - cDiv->getHScale()->getHeight() - cDiv->getTitleBar()->getHeight() - 1);
                locationY += (int)(height * cDiv->getVerticalPercent() / sumPercent);
            }
        }
		reset();
	}


	void FCChart::zoomIn(int pureH, int dataCount, int *findex, int *lindex, double *hScalePixel){
		int max = -1;
		if(*hScalePixel > 1){
			*hScalePixel -=2 ;
		}
		else{
			*hScalePixel = *hScalePixel * 2 / 3;
		}
		max = getMaxVisibleCount(*hScalePixel, pureH);
		if(max >= dataCount){
			if(*hScalePixel < 1){
				*hScalePixel = (double)pureH / max;
			}
			*findex = 0;
			*lindex = dataCount - 1;
		}
		else{
			*findex = *lindex - max + 1;
			if(*findex < 0){
				*findex = 0;
			}
		}
	}

	void FCChart::zoomOut(int pureH, int dataCount, int *findex, int *lindex, double *hScalePixel){
		int oriMax = -1, max = -1, deal = 0;
		if(*hScalePixel < 30){
			oriMax = getMaxVisibleCount(*hScalePixel,  pureH);
			if(dataCount < oriMax){
				deal = 1;
			}
			if(*hScalePixel >= 1){
				*hScalePixel += 2;
			}
			else{
				*hScalePixel = *hScalePixel * 1.5;
				if(*hScalePixel > 1){
					*hScalePixel = 1;
				}
			}
			max = getMaxVisibleCount(*hScalePixel, pureH);
			if(dataCount >= max){
				if(deal == 1){
					*lindex = dataCount - 1;
				}
				*findex = *lindex - max + 1;
				if(*findex < 0){
					*findex = 0;
				}
			}
		}
	}

	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void FCChart::onKeyDown(char key){
		FCView::onKeyDown(key);
		FCHost *host = getNative()->getHost();
		if(!host->isKeyPress(VK_CONTROL)
		&& !host->isKeyPress(VK_MENU)
		&& !host->isKeyPress(VK_SHIFT)){
			m_isTouchMove = false;
			m_showingToolTip = false;
			ScrollType operatorType = ScrollType_None;
			switch (key){
				case 37:
					if(m_reverseHScale)
						operatorType = ScrollType_Right;
					else
						operatorType = ScrollType_Left;
					break;
				case 39:
					if(m_reverseHScale)
						operatorType = ScrollType_Left;
					else
						operatorType = ScrollType_Right;
					break;
				case 38:
					operatorType = ScrollType_ZoomOut;
					break;
				case 40:
					operatorType = ScrollType_ZoomIn;
					break;
				case 27:
					ArrayList<ChartDiv*> divsCopy = getDivs();
					if (divsCopy.size() > 0){
						clearSelectedDiv();
						invalidate();
					}
					break;
			}
			if (operatorType != ScrollType_None){
				changeChart(operatorType, 1);
				return;
			}
		}
	}

	void FCChart::onLoad(){
		FCView::onLoad();
		startTimer(m_timerID, 50);
		if(!m_dataSource){
			m_dataSource = new FCDataTable();
		}
	}

	void FCChart::onTouchDown(FCTouchInfo touchInfo){
		FCView::onTouchDown(touchInfo); 
		if(touchInfo.m_firstTouch && touchInfo.m_clicks == 2){
			clearSelectedShape();
			m_showCrossLine = !m_showCrossLine;
			invalidate();
			return;
		}
		FCPoint mp = touchInfo.m_firstPoint;
		int width = getWidth();
        m_userResizeDiv = 0;
        int shapeCount = !getSelectedShape() ? 0 : 1;
		ArrayList<ChartDiv*> divsCopy = getDivs();
        m_hResizeType = 0;
		if (touchInfo.m_firstTouch){
            clearSelectedPlot();
			ChartDiv *touchOverDiv = getTouchOverDiv();
			for(int d = 0; d < divsCopy.size(); d++){
				ChartDiv *div = divsCopy.get(d);
                if (div == touchOverDiv){
                    div->setSelected(true);
                }
                else{
                    div->setSelected(false);
                }
            }
			if (touchInfo.m_clicks == 1){
                closeSelectArea();
                m_crossStopIndex = getTouchOverIndex();
                m_cross_y = mp.y;
				if (m_showCrossLine && m_crossLineMoveMode == CrossLineMoveMode_AfterClick){
                    m_crossStopIndex = getTouchOverIndex();
                    m_cross_y = mp.y;
                    m_isScrollCross = false;
                }
                if (m_canResizeH){
                    if (mp.x >= m_leftVScaleWidth - 4 && mp.x <= m_leftVScaleWidth + 4){
                        m_hResizeType = 1;
                        goto OutLoop;
                    }
                    else if (mp.x >= width - m_rightVScaleWidth - 4 && mp.x <= width - m_rightVScaleWidth + 4){
                        m_hResizeType = 2;
                        goto OutLoop;
                    }
                }
                if (m_canResizeV){
                    int pIndex = 0;
					for(int cd = 0; cd < divsCopy.size(); cd++){
						ChartDiv *cDiv = divsCopy.get(cd);
                        pIndex++;
                        if (pIndex == (int)divsCopy.size()){
                            break;
                        }
						FCRect resizeRect = {0, cDiv->getBottom() - 4, cDiv->getWidth(), cDiv->getBottom() + 4};
                        if (mp.x >= resizeRect.left && mp.x <= resizeRect.right
                            && mp.y >= resizeRect.top && mp.y <= resizeRect.bottom){
                            m_userResizeDiv = cDiv;
                            goto OutLoop;
                        }
                    }
                }
                if ((mp.x >= m_leftVScaleWidth && mp.x <= width - m_rightVScaleWidth)){
                    if (touchOverDiv){
						ArrayList<FCPlot*> plotsCopy = touchOverDiv->getPlots(SortType_DESC);
						for(int p = 0; p < plotsCopy.size(); p++){
							FCPlot *lsb = plotsCopy.get(p);
                            if (lsb->isEnabled() && lsb->isVisible() && lsb->onSelect()){
                                m_movingPlot = lsb;
								lsb->onMoveStart();
								double *zorders = new double[plotsCopy.size()];
								ArrayList<FCPlot*> plots = touchOverDiv->getPlots(SortType_DESC);
								int lstSize = (int)plotsCopy.size();
								for (int j = 0;j < lstSize; j++){
									FCPlot *plot = plots.get(j);
									zorders[j] = plot->getZOrder();
								}
								lsb->setZOrder((int)FCScript::maxValue(zorders, (int)plotsCopy.size()) + 1);
								delete[] zorders;
								zorders = 0;
                            }
                        }
                    }
                    if (m_movingPlot){
                        m_movingPlot->setSelected(true);
                        if (shapeCount != 0){
                            clearSelectedShape();
                        }
                        goto OutLoop;
                    }
                    else{
                        BaseShape *bs = selectShape(m_crossStopIndex, 1);
                        ChartDiv *div = 0;
                        if (!bs){
                            div = touchOverDiv;
                            if (div && div->getSelectArea()->isEnabled()){
                                if (mp.y >= div->getTop() + 2 && mp.y <= div->getBottom() - div->getHScale()->getHeight() - 2){
									m_showingSelectArea = true;
                                }
                            }
                        }
                    }
                }
            OutLoop: ;
            }
        }
        else{
            m_isTouchMove=true;
            m_showingToolTip = false;
        }
        m_lastTouchClickPoint = mp;
		if(m_canMoveShape){
        if (getSelectedShape()){
            m_movingShape = getSelectedShape();
        }
}
		invalidate();
	}

	void FCChart::onTouchMove(FCTouchInfo touchInfo){
		FCView::onTouchMove(touchInfo); 
		FCPoint mp = touchInfo.m_firstPoint;
		if (mp.x != m_lastTouchMovePoint.x || mp.y != m_lastTouchMovePoint.y){
			int width = getWidth();
            m_isTouchMove = true;
            m_showingToolTip = false;
			ArrayList<ChartDiv*> divsCopy = getDivs();
			for(int d = 0; d < divsCopy.size(); d++){
				ChartDiv *div = divsCopy.get(d);
                bool resize = false;
                if (div->getSelectArea()->isVisible() && div->getSelectArea()->canResize()){
                    resize = true;
                }
                else{
                    if (m_showingSelectArea){
						if (touchInfo.m_firstTouch){
                            int subX = m_lastTouchClickPoint.x - m_lastTouchMovePoint.x;
                            int subY = m_lastTouchMovePoint.y - m_lastTouchClickPoint.y;
                            if (abs(subX) > m_hScalePixel * 2 || abs(subY) > m_hScalePixel * 2){
                                m_showingSelectArea = false;
                                div->getSelectArea()->setVisible(true);
                                div->getSelectArea()->setCanResize(true);
                                resize = true;
                            }
                        }
                    }
                }
				if (resize && touchInfo.m_firstTouch){
                    int x1 = m_lastTouchClickPoint.x;
                    int y1 = m_lastTouchClickPoint.y;
                    int x2 = mp.x;
                    int y2 = mp.y;
                    if (x2 < m_leftVScaleWidth){
                        x2 = m_leftVScaleWidth;
					}
                    else if (x2 > width - m_rightVScaleWidth){
                        x2 = width - m_rightVScaleWidth;
					}
                    if (y2 > div->getBottom() - div->getHScale()->getHeight()){
                        y2 = div->getBottom() - div->getHScale()->getHeight();
					}
                    else if (y2 < div->getTop() + div->getTitleBar()->getHeight()){
                        y2 = div->getTop() + div->getTitleBar()->getHeight();
					}
					int bx = 0, by = 0, bwidth = 0, bheight = 0;
					FCPlot::rectangleXYWH(x1, y1 - div->getTop(), x2, y2 - div->getTop(), &bx, &by, &bwidth, &bheight);
					FCRect bounds = {bx, by, bx + bwidth, by + bheight};
					div->getSelectArea()->setBounds(bounds);
                    invalidate();
                    m_lastTouchMovePoint = mp;
                    return;
                }
                if (div->getSelectArea()->isVisible()){
                    return;
                }
            }
			SYSTEMTIME t; 
			GetLocalTime(&t); 
			m_lastTouchMoveTime = FCStr::getDateNum(t.wYear,t.wMonth,t.wDay,t.wHour,t.wMinute,t.wSecond,t.wMilliseconds);
			if (m_movingPlot && touchInfo.m_firstTouch){
                m_movingPlot->onMoving();
            }
            else{
				if (m_canResizeH){
                    if (m_hResizeType == 0){
                        if ((mp.x >= m_leftVScaleWidth - 4 && mp.x <= m_leftVScaleWidth + 4) ||
                        (mp.x >= width - m_rightVScaleWidth - 4 && mp.x <= width - m_rightVScaleWidth + 4)){
							setCursor(FCCursors_SizeWE);
                            goto OutLoop;
                        }
                    }
                    else{
                        setCursor(FCCursors_SizeWE);
                        goto OutLoop;
                    }
                }
                if (m_canResizeV){
                    int pIndex = 0;
                    ArrayList<ChartDiv*> divsCopy = getDivs();
					for(int d = 0; d < divsCopy.size(); d++){
						ChartDiv *cDiv = divsCopy.get(d);
                        pIndex++;
                        if (pIndex == (int)divsCopy.size()){
                            break;
                        }
						FCRect resizeRect = {0, cDiv->getBottom() - 4, width, cDiv->getBottom() + 4};
                        if (mp.x >= resizeRect.left && mp.x <= resizeRect.right && mp.y >= resizeRect.top && mp.y <= resizeRect.bottom){
							setCursor(FCCursors_SizeNS);
                            goto OutLoop;
						}
                    }
                }
				setCursor(FCCursors_Arrow);
				OutLoop: ;
            }
            m_crossStopIndex = getTouchOverIndex();
            m_cross_y = mp.y;
            if (m_showCrossLine && m_crossLineMoveMode == CrossLineMoveMode_FollowTouch){
                m_isScrollCross = false;
            }
            invalidate();
        }
        m_lastTouchMovePoint = mp;
	}

	void FCChart::onTouchUp(FCTouchInfo touchInfo){
		FCView::onTouchUp(touchInfo); 
		bool needUpdate = false;
        if (m_movingShape){
            m_movingShape = 0;
        }
        ArrayList<ChartDiv*> divsCopy = getDivs();
		for(int d = 0; d < divsCopy.size(); d++){
			ChartDiv *div = divsCopy.get(d);
            if (div->getSelectArea()->isVisible()){
                div->getSelectArea()->setCanResize(false);
                invalidate();
                return;
            }
        }
		FCPoint mp = touchInfo.m_firstPoint;
        setCursor(FCCursors_Arrow);
		BaseShape *selectedShape = getSelectedShape();
		if (m_hResizeType == 0 && m_userResizeDiv == 0 && touchInfo.m_firstTouch && m_canMoveShape && selectedShape){
            ChartDiv *curDiv = findDiv(selectedShape);
			for(int cd = 0; cd < divsCopy.size(); cd++){
				ChartDiv *cDiv = divsCopy.get(cd);
                if (mp.y >= cDiv->getTop() && mp.y <= cDiv->getBottom()){
                    if (cDiv == curDiv){
                        break;
                    }
					if(!cDiv->containsShape(selectedShape)){
						int dsize = (int)divsCopy.size();
                        for (int j = 0; j < dsize; j++){
							ChartDiv *div = divsCopy.get(j);
							if(div->containsShape(selectedShape)){
                                div->removeShape(selectedShape);
                                break;
                            }
                        }
                        cDiv->addShape(selectedShape);
                        needUpdate = true;
                    }
                }
            }
        }
        if (m_movingPlot){
            m_movingPlot = 0;
        }
        if (resizeDiv()){
            needUpdate = true;
        }
        if (needUpdate){
            update();
        }
		invalidate();
	}

	void FCChart::onTouchWheel(FCTouchInfo touchInfo){
		FCView::onTouchWheel(touchInfo); 
        if (touchInfo.m_delta > 0){
            changeChart(ScrollType_ZoomOut, 1);
        }
        else{
            changeChart(ScrollType_ZoomIn, 1);
        }
	}

	void FCChart::onKeyUp(char key){
		FCView::onKeyUp(key);
		if (m_scrollStep != 1){
            bool redraw = false;
            if (m_scrollStep > 6){
                redraw = true;
            }
            m_scrollStep = 1;
            if (redraw){
                update();
				invalidate();
            }
        }
	}

	void FCChart::onPaintForeground(FCPaint *paint, const FCRect& clipRect){
		FCPoint offset = paint->getOffset();
		ArrayList<ChartDiv*> divsCopy = getDivs();
		for(int d = 0; d < divsCopy.size(); d++){
			ChartDiv *div = divsCopy.get(d);
			int offsetX = offset.x + div->getLeft();
            int offsetY = offset.y + div->getTop();
			FCPoint newOffset = {offsetX, offsetY};
			paint->setOffset(newOffset);
			FCRect divClipRect = {0, 0, div->getWidth(), div->getHeight()};
			paint->setClip(divClipRect);
			onPaintDivBackGround(paint, div);
			onPaintHScale(paint, div);
			onPaintVScale(paint, div);
			onPaintShapes(paint, div);
			onPaintDivBorder(paint, div);
			onPaintCrossLine(paint, div);
			onPaintTitle(paint, div);
			onPaintSelectArea(paint, div);
			onPaintPlots(paint, div);
		}
		paint->setOffset(offset);
		paint->setClip(clipRect);
		onPaintResizeLine(paint);
		onPaintToolTip(paint);
		onPaintIcon(paint);
	}

	void FCChart::onTimer(int timerID){
		FCView::onTimer(timerID);
		if(isVisible() && this == getNative()->getHoveredControl() && timerID == m_timerID){
			checkToolTip();
		}
	}
}