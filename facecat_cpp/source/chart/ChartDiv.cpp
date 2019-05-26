#include "..\\..\\stdafx.h"
#include "..\\..\\include\\chart\\ChartDiv.h"
#include <iterator>

namespace FaceCat{
	bool ChartDiv::shapesAsc(BaseShape *x, BaseShape *y){
		return x->getZOrder() < y->getZOrder();  
	}  

	bool ChartDiv::shapesDesc(BaseShape *x, BaseShape *y){  
		return x->getZOrder() > y->getZOrder();  
	} 

	bool ChartDiv::plotsAsc(FCPlot *x, FCPlot *y){
		return x->getZOrder() < y->getZOrder();
	}

	bool ChartDiv::plotsDesc(FCPlot *x, FCPlot *y){
		return x->getZOrder() > y->getZOrder();
	}

	ChartDiv::ChartDiv(){
		m_allowUserPaint = false;
		m_backColor = FCColor::argb(0, 0, 0);
		m_borderColor = FCColor_None;
		m_chart = 0;
		m_crossLine = new CrossLine;
		m_font = new FCFont(L"Arial", 12, false, false, false);
		m_hGrid = new ScaleGrid;
		m_hGrid->setVisible(true);
		m_hScale = new HScale;
		m_leftVScale = new VScale;
		m_location.x = 0;
		m_location.y = 0;
		m_rightVScale = new VScale;
		m_size.cx = 0; 
		m_size.cy = 0;
		m_selectArea = new SelectArea;
		m_selected = false;
		m_showSelect = true;
		m_title = new ChartTitleBar;
		m_toolTip = new ChartToolTip;
		m_verticalPercent = 0;
		m_vGrid = new ScaleGrid;
		m_workingAreaHeight = 0;
	}

	ChartDiv::~ChartDiv(){
		if(m_font){
			delete m_font;
			m_font = 0;
		}
		if(m_crossLine){
			delete m_crossLine;
			m_crossLine = 0;
		}
		if(m_hGrid){
			delete m_hGrid;
			m_hGrid = 0;
		}
		if(m_hScale){
			delete m_hScale;
			m_hScale = 0;
		}
		if(m_leftVScale){	
			delete m_leftVScale;
			m_leftVScale = 0;
		}
		if(m_rightVScale){
			delete m_rightVScale;
			m_rightVScale = 0;
		}
		if(m_selectArea){
			delete m_selectArea;
			m_selectArea = 0;
		}
		if(m_title){
			delete m_title;
			m_title = 0;
		}
		if(m_toolTip){
			delete m_toolTip;
			m_toolTip = 0;
		}
		if(m_vGrid){
			delete m_vGrid;
			m_vGrid = 0;
		}
		if(m_plots.size() > 0){
			for(int p = 0; p < m_plots.size(); p++){
				FCPlot *plot = m_plots.get(p);
				delete plot;
			}
			m_plots.clear();
		}
		if(m_shapes.size() > 0){
			for(int b = 0; b < m_shapes.size(); b++){
				BaseShape *shape = m_shapes.get(b);
				delete shape;
			}
			m_shapes.clear();
		}
	}

	bool ChartDiv::allowUserPaint(){
		return m_allowUserPaint;
	}

	void ChartDiv::setAllowUserPaint(bool allowUserPaint){
		m_allowUserPaint = allowUserPaint;
	}

	Long ChartDiv::getBackColor(){
		return m_backColor;
	}

	void ChartDiv::setBackColor(Long backColor){
		m_backColor = backColor;
	}

	Long ChartDiv::getBorderColor(){
		return m_borderColor;
	}

	void ChartDiv::setBorderColor(Long borderColor){
		m_borderColor = borderColor;
	}

	int ChartDiv::getBottom(){
		return m_location.y + m_size.cy;
	}

	FCRect ChartDiv::getBounds(){
		FCRect bounds = {m_location.x, m_location.y, m_location.x + m_size.cx, m_location.y + m_size.cy};
		return bounds;
	}

	void ChartDiv::setBounds(FCRect bounds){
		m_location.x = bounds.left;
		m_location.y = bounds.top;
		m_size.cx = bounds.right - bounds.left;
		m_size.cy = bounds.bottom - bounds.top;
	}

	CrossLine* ChartDiv::getCrossLine(){
		return m_crossLine;
	}

	void ChartDiv::setCrossLine( CrossLine *crossLine ){
		if (m_crossLine){
			delete m_crossLine;
		}
		m_crossLine = crossLine;
	}

	FCFont* ChartDiv::getFont(){
		return m_font;
	}

	void ChartDiv::setFont(FCFont *font){
		m_font->copy(font);
	}

	int ChartDiv::getHeight(){
		return m_size.cy;
	}

	ScaleGrid* ChartDiv::getHGrid(){
		return m_hGrid;
	}

	void ChartDiv::setHGrid(ScaleGrid *hGrid){
		if (m_hGrid){
			delete m_hGrid;
		}
		m_hGrid = hGrid;
	}

	HScale* ChartDiv::getHScale(){
		return m_hScale;
	}

	void ChartDiv::setHScale(HScale *hScale){
		if (m_hScale){
			delete m_hScale;
		}
		m_hScale = hScale;
	}

	FCChart* ChartDiv::getChart(){
		return m_chart;
	}

	void ChartDiv::setChart(FCChart* chart){
		m_chart = chart;
	}

	int ChartDiv::getLeft(){
		return m_location.x;
	}

	VScale* ChartDiv::getLeftVScale(){
		return m_leftVScale;
	}

	void ChartDiv::setLeftVScale(VScale *leftVScale){
		if (m_leftVScale){
			delete m_leftVScale;
		}
		m_leftVScale = leftVScale;
	}

	FCPoint ChartDiv::getLocation(){
		return m_location;
	}

	int ChartDiv::getRight(){
		return m_location.x + m_size.cx;
	}

	VScale* ChartDiv::getRightVScale(){
		return m_rightVScale;
	}

	void ChartDiv::setRightVScale(VScale *rightVScale){
		if (m_rightVScale){
			delete m_rightVScale;
		}
		m_rightVScale = rightVScale;
	}

	SelectArea* ChartDiv::getSelectArea(){
		return m_selectArea;
	}

	void ChartDiv::setSelectArea(SelectArea *selectArea){
		if (m_selectArea){
			delete m_selectArea;
		}
		m_selectArea = selectArea;
	}

	bool ChartDiv::isSelected(){
		return m_selected;
	}

	void ChartDiv::setSelected(bool selected){
		m_selected = selected;
	}

	bool ChartDiv::isShowSelect(){
		return m_showSelect;
	}

	void ChartDiv::setShowSelect(bool showSelect){
		m_showSelect = showSelect;
	}

	ChartTitleBar* ChartDiv::getTitleBar(){
		return m_title;
	}

	void ChartDiv::setTitleBar(ChartTitleBar *title){
		if (m_title){
			delete m_title;
		}
		m_title = title;
	}

	ChartToolTip* ChartDiv::getToolTip(){
		return m_toolTip;
	}

	void ChartDiv::setToolTip(ChartToolTip *toolTip){
		if (m_toolTip){
			delete m_toolTip;
		}
		m_toolTip = toolTip;
	}

	int ChartDiv::getTop(){
		return m_location.y;
	}

	int ChartDiv::getVerticalPercent(){
		return m_verticalPercent;
	}

	void ChartDiv::setVerticalPercent(int verticalPercent){
		m_verticalPercent = verticalPercent;
	}

	ScaleGrid* ChartDiv::getVGrid(){
		return m_vGrid;
	}

	void ChartDiv::setVGrid(ScaleGrid *vGrid){
		if (m_vGrid){
			delete m_vGrid;
		}
		m_vGrid = vGrid;
	}

	int ChartDiv::getWidth(){
		return m_size.cx;
	}

	int ChartDiv::getWorkingAreaHeight(){
		return m_workingAreaHeight;
	}

	void ChartDiv::setWorkingAreaHeight(int workingAreaHeight){
		m_workingAreaHeight = workingAreaHeight;
	}

	//////////////////////////////////////////////////
	void ChartDiv::addPlot(FCPlot *plot){
		m_plots.add(plot);
	}

	void ChartDiv::addShape(BaseShape *shape){
		m_shapes.add(shape);
	}

	bool ChartDiv::containsShape(BaseShape *shape){
		if(m_shapes.size()>0){
			for(int b = 0; b < m_shapes.size(); b++){
				if(shape == m_shapes.get(b)){
					return true;
				}
			}
		}
		return false;
	}

	ArrayList<FCPlot*> ChartDiv::getPlots(SortType sortType){
        ArrayList<FCPlot*> list;
        vector<FCPlot*> plots;
        for(int i = 0; i < m_plots.size(); i++){
            plots.push_back(m_plots.get(i));
        }
        if(sortType == SortType_ASC){
            sort(plots.begin(), plots.end(), plotsAsc);
        }
        else if(sortType == SortType_DESC){
            sort(plots.begin(), plots.end(), plotsDesc);
        }
        return list;
	}

	void ChartDiv::getProperty(const String& name, String *value, String *type){
		if (name == L"allowuserpaint"){
			*type = L"bool";
			*value = FCStr::convertBoolToStr(allowUserPaint());
		}
	    else if (name == L"backcolor"){
            *type = L"color";
			*value = FCStr::convertColorToStr(getBackColor());
        }
        else if (name == L"bordercolor"){
            *type = L"color";
			*value = FCStr::convertColorToStr(getBorderColor());
        }
        else if (name == L"showselect"){
            *type = L"bool";
			*value = FCStr::convertBoolToStr(isShowSelect());
        }
	}

	ArrayList<String> ChartDiv::getPropertyNames(){
		ArrayList<String> propertyNames;
		propertyNames.add(L"AllowUserPaint");
		propertyNames.add(L"BackColor");
		propertyNames.add(L"BorderColor");
		propertyNames.add(L"ShowSelect");
		return propertyNames;
	}

	ArrayList<BaseShape*> ChartDiv::getShapes(SortType sortType){
        ArrayList<BaseShape*> list;
        vector<BaseShape*> shapes;
        for(int i = 0; i < m_shapes.size(); i++){
            shapes.push_back(m_shapes.get(i));
        }
        if(sortType == SortType_ASC){
            sort(shapes.begin(), shapes.end(), shapesAsc);
        }
        else if(sortType == SortType_DESC){
            sort(shapes.begin(), shapes.end(), shapesDesc);
        }
        vector<BaseShape*>::iterator sIter = shapes.begin();
        for(; sIter != shapes.end(); ++sIter){
            list.add(*sIter);
        }
        shapes.clear();
        return list;
	}

	VScale* ChartDiv::getVScale(AttachVScale attachVScale){
		if(attachVScale == AttachVScale_Left){
			return m_leftVScale;
		}
		else{
			return m_rightVScale;
		}
	}
	
	void ChartDiv::onPaint(FCPaint *paint, const FCRect& rect){

	}

	void ChartDiv::removePlot(FCPlot *plot){
		if(m_plots.size()>0){
			for(int p = 0; p < m_plots.size(); p++){
				if(plot == m_plots.get(p)){
					m_plots.removeAt(p);
					break;
				}
			}
		}
	}

	void ChartDiv::removeShape(BaseShape *shape){
		if(m_shapes.size()>0){
			for(int b = 0; b < m_shapes.size(); b++){
				if(shape == m_shapes.get(b)){
					m_shapes.removeAt(b);
					break;
				}
			}
		}
	}

	void ChartDiv::setProperty(const String& name, const String& value){
		if (name == L"allowuserpaint"){
			setAllowUserPaint(FCStr::convertStrToBool(value));
		}
	    else if (name == L"backcolor"){
			setBackColor(FCStr::convertStrToColor(value));
        }
        else if (name == L"bordercolor"){
			setBorderColor(FCStr::convertStrToColor(value));
        }
        else if (name == L"showselect"){
			setShowSelect(FCStr::convertStrToBool(value));
        }
	}

}