#include "stdafx.h"
#include "FCPlot.h"
#include "FCScript.h"

namespace FaceCat{
    FCPlot::FCPlot(){
        m_attachVScale = AttachVScale_Left;
        m_color = FCColor::argb(255, 255, 255);
        m_dataSource = 0;
        m_drawGhost = true;
        m_enabled = true;
        m_font = new FCFont;
        m_isPaintingGhost = false;
        m_lineStyle = 0;
        m_lineWidth = 1;
        m_moveTimes = 0;
        m_selected = false;
        m_selectedColor = FCColor::argb(255, 255, 255);
        m_selectedPoint = SelectPoint_Rect;
        m_startPoint.x = 0;
        m_startPoint.y = 0;
        m_visible = true;
        m_zOrder = 0;
    }
    
    FCPlot::~FCPlot(){
        m_dataSource = 0;
        if(m_font){
            delete m_font;
            m_font = 0;
        }
        clearMarks(&m_marks);
        clearMarks(&m_startMarks);
    }
    
    AttachVScale FCPlot::getAttachVScale(){
        return m_attachVScale;
    }
    
    void FCPlot::setAttachVScale(AttachVScale attachVScale){
        m_attachVScale = attachVScale;
    }
    
    Long FCPlot::getColor(){
        return m_color;
    }
    
    void FCPlot::setColor(Long color){
        m_color = color;
    }
    
    ChartDiv* FCPlot::getDiv(){
        return m_div;
    }
    
    void FCPlot::setDiv(ChartDiv *div){
        m_div = div;
        m_dataSource = m_div->getChart()->getDataSource();
    }
    
    bool FCPlot::drawGhost(){
        return m_drawGhost;
    }
    
    void FCPlot::setDrawGhost(bool drawGhost){
        m_drawGhost = drawGhost;
    }
    
    bool FCPlot::isEnabled(){
        return m_enabled;
    }
    
    void FCPlot::setEnabled(bool enabled){
        m_enabled = enabled;
    }
    
    FCFont* FCPlot::getFont(){
        return m_font;
    }
    
    void FCPlot::setFont(FCFont *font){
        m_font->copy(font);
    }
    
    FCChart* FCPlot::getChart(){
        return m_div->getChart();
    }
    
    int FCPlot::getLineStyle(){
        return m_lineStyle;
    }
    
    void FCPlot::setLineStyle(int lineStyle){
        m_lineStyle = lineStyle;
    }
    
    int FCPlot::getLineWidth(){
        return m_lineWidth;
    }
    
    void FCPlot::setLineWidth(int lineWidth){
        m_lineWidth = lineWidth;
    }
    
    String FCPlot::getPlotType(){
        return m_plotType;
    }
    
    void FCPlot::setPlotType(const String& plotType){
        m_plotType = plotType;
    }
    
    bool FCPlot::isSelected(){
        return m_selected;
    }
    
    void FCPlot::setSelected(bool selected){
        m_selected = selected;
    }
    
    Long FCPlot::getSelectedColor(){
        return m_selectedColor;
    }
    
    void FCPlot::setSelectedColor(Long selectedColor){
        m_selectedColor = selectedColor;
    }
    
    enum SelectPoint FCPlot::getSelectedPoint(){
        return m_selectedPoint;
    }
    
    void FCPlot::setSelectedPoint(enum SelectPoint selectedPoint){
        m_selectedPoint = selectedPoint;
    }
    
    String FCPlot::getText(){
        return m_text;
    }
    
    void FCPlot::setText(const String& text){
        m_text = text;
    }
    
    bool FCPlot::isVisible(){
        return m_visible;
    }
    
    void FCPlot::setVisible(bool visible){
        m_visible = visible;
    }
    
    int FCPlot::getWorkingAreaWidth(){
        return getChart()->getWorkingAreaWidth();
    }
    
    int FCPlot::getWorkingAreaHeight(){
        return m_div->getWorkingAreaHeight();
    }
    
    int FCPlot::getZOrder(){
        return m_zOrder;
    }
    
    void FCPlot::setZOrder(int zOrder){
        m_zOrder = zOrder;
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    void FCPlot::drawEllipse(FCPaint *paint, Long dwPenColor, int width, int style, const FCRect& rect){
        drawEllipse(paint, dwPenColor, width, style, rect.left, rect.top, rect.right, rect.bottom);
    }
    
    void FCPlot::drawEllipse(FCPaint *paint, Long dwPenColor, int width, int style, int left, int top, int right, int bottom){
        int px = getPx();
        int py = getPy();
        left += px;
        top += py;
        right += px;
        bottom += py;
        paint->drawEllipse(dwPenColor, width, style, left, top, right, bottom);
        if(paint->supportTransparent()){
            FCChart *chart = getChart();
            FCPoint mp = chart->getTouchPoint();
            FCNative *native = getNative();
            if (!m_isPaintingGhost && (chart->getMovingPlot() == this ||  (mp.y >= m_div->getTop() && mp.y <= m_div->getBottom() &&  (chart == native->getHoveredControl() && !chart->isOperating() && onSelect())))){
                int a = 0, r = 0, g = 0, b = 0;
                FCColor::toArgb(paint, dwPenColor, &a, &r, &g, &b);
                if(a == 255){
                    a = 50;
                }
                dwPenColor = FCColor::argb(a, r, g, b);
                width += 10;
                paint->drawEllipse(dwPenColor, width, 0, left, top, right, bottom);
            }
        }
    }
    
    void FCPlot::drawEllipse(FCPaint *paint, Long dwPenColor, int width, int style, float left, float top, float right, float bottom){
        drawEllipse(paint, dwPenColor, width, style, (int)left, (int)top, (int)right, (int)bottom);
    }
    
    void FCPlot::drawLine(FCPaint *paint, Long dwPenColor, int width, int style, const FCPoint& x, const FCPoint& y){
        drawLine(paint, dwPenColor, width, style, x.x, x.y, y.x, y.y);
    }
    
    void FCPlot::drawLine(FCPaint *paint, Long dwPenColor, int width, int style, int x1, int y1, int x2, int y2){
        int px = getPx();
        int py = getPy();
        x1 += px;
        y1 += py;
        x2 += px;
        y2 += py;
        paint->drawLine(dwPenColor, width, style, x1, y1, x2, y2);
        if(paint->supportTransparent()){
            FCChart *chart = getChart();
            FCPoint mp = chart->getTouchPoint();
            FCNative *native = getNative();
            if (!m_isPaintingGhost && (chart->getMovingPlot() == this ||  (mp.y >= m_div->getTop() && mp.y <= m_div->getBottom() &&  (chart == native->getHoveredControl() && !chart->isOperating() && onSelect())))){
                int a = 0, r = 0, g = 0, b = 0;
                FCColor::toArgb(paint, dwPenColor, &a, &r, &g, &b);
                if(a == 255){
                    a = 50;
                }
                dwPenColor = FCColor::argb(a, r, g, b);
                width += 10;
                paint->drawLine(dwPenColor, width, 0, x1, y1, x2, y2);
            }
        }
    }
    
    void FCPlot::drawLine(FCPaint *paint, Long dwPenColor, int width, int style, float x1, float y1, float x2, float y2){
        drawLine(paint, dwPenColor, width, style, (int)x1, (int)y1, (int)x2, (int)y2);
    }
    
    void FCPlot::drawPolygon(FCPaint *paint, Long dwPenColor, int width, int style, FCPoint *apt, int cpt){
        int px = getPx();
        int py = getPy();
        for(int i = 0;i < cpt;i++){
            apt[i].x += px;
            apt[i].y += py;
        }
        paint->drawPolygon(dwPenColor, width, style, apt, cpt);
        if(paint->supportTransparent()){
            FCChart *chart = getChart();
            FCPoint mp = chart->getTouchPoint();
            FCNative *native = getNative();
            if (!m_isPaintingGhost && (chart->getMovingPlot() == this ||  (mp.y >= m_div->getTop() && mp.y <= m_div->getBottom() &&  (chart == native->getHoveredControl() && !chart->isOperating() && onSelect())))){
                int a = 0, r = 0, g = 0, b = 0;
                FCColor::toArgb(paint, dwPenColor, &a, &r, &g, &b);
                if(a == 255){
                    a = 50;
                }
                dwPenColor = FCColor::argb(a, r, g, b);
                width += 10;
                paint->drawPolygon(dwPenColor, width, 0, apt, cpt);
            }
        }
    }
    
    void FCPlot::drawPolyline(FCPaint *paint, Long dwPenColor, int width, int style, FCPoint *apt, int cpt){
        int px = getPx();
        int py = getPy();
        for(int i = 0;i < cpt;i++){
            apt[i].x += px;
            apt[i].y += py;
        }
        paint->drawPolyline(dwPenColor, width, style, apt, cpt);
        if(paint->supportTransparent()){
            FCChart *chart = getChart();
            FCPoint mp = chart->getTouchPoint();
            FCNative *native = getNative();
            if (!m_isPaintingGhost && (chart->getMovingPlot() == this ||  (mp.y >= m_div->getTop() && mp.y <= m_div->getBottom() &&  (chart == native->getHoveredControl() && !chart->isOperating() && onSelect())))){
                int a = 0, r = 0, g = 0, b = 0;
                FCColor::toArgb(paint, dwPenColor, &a, &r, &g, &b);
                if(a == 255){
                    a = 50;
                }
                dwPenColor = FCColor::argb(a, r, g, b);
                width += 10;
                paint->drawPolyline(dwPenColor, width, 0, apt, cpt);
            }
        }
    }
    
    void FCPlot::drawRay(FCPaint *paint, Long dwPenColor, int width, int style, float x1, float y1, float x2, float y2, float k, float b){
        if (k != 0 || b != 0){
            float leftX = 0;
            float leftY = leftX * k + b;
            float rightX = (float)getWorkingAreaWidth();
            float rightY = rightX * k + b;
            if (x1 >= x2){
                drawLine(paint, dwPenColor, width, style, leftX, leftY, x1, y1);
            }
            else{
                drawLine(paint, dwPenColor, width, style, x1, y1, rightX, rightY);
            }
        }
        else{
            if (y1 >= y2){
                drawLine(paint, dwPenColor, width, style, x1, y1, x1, (float)0);
            }
            else{
                drawLine(paint, dwPenColor, width, style, x1, y1, x1, (float)getWorkingAreaHeight());
            }
        }
    }
    
    void FCPlot::drawRect(FCPaint *paint, Long dwPenColor, int width, int style, int left, int top, int right, int bottom){
        int px = getPx();
        int py = getPy();
        left += px;
        top += py;
        right += px;
        bottom += py;
        paint->drawRect(dwPenColor, width, style, left, top, right, bottom);
        if(paint->supportTransparent()){
            FCChart *chart = getChart();
            FCPoint mp = chart->getTouchPoint();
            FCNative *native = getNative();
            if (!m_isPaintingGhost && (chart->getMovingPlot() == this ||  (mp.y >= m_div->getTop() && mp.y <= m_div->getBottom() &&  (chart == native->getHoveredControl() && !chart->isOperating() && onSelect())))){
                int a = 0, r = 0, g = 0, b = 0;
                FCColor::toArgb(paint, dwPenColor, &a, &r, &g, &b);
                if(a == 255){
                    a = 50;
                }
                dwPenColor = FCColor::argb(a, r, g, b);
                width += 10;
                paint->drawRect(dwPenColor, width, 0, left, top, right, bottom);
            }
        }
    }
    
    void FCPlot::drawRect(FCPaint *paint, Long dwPenColor, int width, int style, const FCRect& rect){
        paint->drawRect(dwPenColor, width, style, rect.left, rect.top, rect.right, rect.bottom);
    }
    
    void FCPlot::drawSelect(FCPaint *paint, Long dwPenColor, int x, int y){
        int sub = m_lineWidth * 3;
        FCRect ellipseRect ={x - sub, y - sub, x + sub, y + sub};
        if (m_selectedPoint == SelectPoint_Ellipse){
            fillEllipse(paint, dwPenColor, ellipseRect);
        }
        else if (m_selectedPoint == SelectPoint_Rect){
            fillRect(paint, dwPenColor, ellipseRect);
        }
    }
    
    void FCPlot::drawSelect(FCPaint *paint, Long dwPenColor, float x, float y){
        drawSelect(paint, dwPenColor, (int)x, (int)y);
    }
    
    void FCPlot::drawText(FCPaint *paint, const wchar_t *strText, Long dwPenColor, FCFont *font, int left, int top){
        left = left + getPx();
        top = top + getPy();
        FCSize textSize = this->textSize(paint, strText, font);
        FCRect rect ={left, top, left + textSize.cx, top + textSize.cy};
        paint->drawText(strText, dwPenColor, font, rect);
    }
    
    void FCPlot::fillEllipse(FCPaint *paint, Long dwPenColor, const FCRect& rect){
        int px = getPx();
        int py = getPy();
        FCRect newRect = rect;
        newRect.left += px;
        newRect.top += py;
        newRect.right += px;
        newRect.bottom += py;
        paint->fillEllipse(dwPenColor, newRect);
    }
    
    void FCPlot::fillPolygon(FCPaint *paint, Long dwPenColor, FCPoint *apt, int cpt){
        int px = getPx();
        int py = getPy();
        for(int i = 0;i < cpt;i++){
            apt[i].x += px;
            apt[i].y += py;
        }
        paint->fillPolygon(dwPenColor, apt, cpt);
    }
    
    void FCPlot::fillRect(FCPaint *paint, Long dwPenColor, const FCRect& rect){
        int px = getPx();
        int py = getPy();
        FCRect newRect = rect;
        newRect.left += px;
        newRect.top += py;
        newRect.right += px;
        newRect.bottom += py;
        paint->fillRect(dwPenColor, newRect);
    }
    
    FCSize FCPlot::textSize(FCPaint *paint, const wchar_t *strText, FCFont *font){
        return paint->textSize(strText, font);
    }
    
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    void FCPlot::movePlot(float touchY, float startY, int startIndex, int touchBeginIndex, int touchEndIndex, float pureV,
                            double max, double min, int dataCount, double *yAddValue, int *newIndex){
        float subY = touchY - startY;
        *yAddValue = ((min - max) * subY / pureV);
        *newIndex = startIndex + (touchEndIndex - touchBeginIndex);
        if(*newIndex < 0){
            *newIndex = 0;
        }
        else if(*newIndex > dataCount - 1){
            *newIndex = dataCount - 1;
        }
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    FCNative* FCPlot::getNative(){
        return getChart()->getNative();
    }
    
    int FCPlot::getIndex(const FCPoint& mp){
        FCChart *chart = getChart();
        FCPoint newp = mp;
        newp.x += chart->getLeftVScaleWidth();
        newp.y += m_div->getTop() + m_div->getTitleBar()->getHeight();
        return chart->getIndex(newp);
    }
    
    FCPoint FCPlot::getTouchOverPoint(){
        FCChart *chart = getChart();
        FCPoint mp = chart->getTouchPoint();
        mp.x -= chart->getLeftVScaleWidth();
        mp.y = mp.y - m_div->getTop() - m_div->getTitleBar()->getHeight();
        return mp;
    }
    
    void FCPlot::clearMarks(HashMap<int,PlotMark*> *marks){
        for(int i = 0; i < marks->size(); i++){
            PlotMark *mark = marks->getValue(i);
            delete mark;
        }
        marks->clear();
    }
    
    bool FCPlot::createPoint(const FCPoint& mp){
        FCChart *chart = getChart();
        int rIndex = m_dataSource->rowsCount();
        if (rIndex > 0){
            int touchIndex = chart->getIndex(mp);
            if (touchIndex >= chart->getFirstVisibleIndex() && touchIndex <= chart->getLastVisibleIndex()){
                double sDate = m_dataSource->getXValue(touchIndex);
                clearMarks(&m_marks);
                double y = getNumberValue(mp);
                m_marks.put(0, new PlotMark(0, sDate, y));
                return true;
            }
        }
        return false;
    }
    
    bool FCPlot::create2PointsA(const FCPoint& mp){
        FCChart *chart = getChart();
        int rIndex = m_dataSource->rowsCount();
        if (rIndex > 0){
            int touchIndex = chart->getIndex(mp);
            if (touchIndex >= chart->getFirstVisibleIndex() && touchIndex <= chart->getLastVisibleIndex()){
                int eIndex = touchIndex;
                int bIndex = eIndex - 1;
                double fDate = m_dataSource->getXValue(bIndex);
                double sDate = m_dataSource->getXValue(eIndex);
                clearMarks(&m_marks);
                double y = getNumberValue(mp);
                m_marks.put(0, new PlotMark(0, fDate, y));
                m_marks.put(1, new PlotMark(1, sDate, y));
                return true;
            }
        }
        return false;
    }
    
    bool FCPlot::create2PointsB(const FCPoint& mp){
        FCChart *chart = getChart();
        int rIndex = m_dataSource->rowsCount();
        if (rIndex > 0){
            int touchIndex = chart->getIndex(mp);
            if (touchIndex >= chart->getFirstVisibleIndex() && touchIndex <= chart->getLastVisibleIndex()){
                double date = m_dataSource->getXValue(touchIndex);
                clearMarks(&m_marks);
                double y = getNumberValue(mp);
                m_marks.put(0, new PlotMark(0, date, y));
                m_marks.put(1, new PlotMark(1, date, y));
                return true;
            }
        }
        return false;
    }
    
    bool FCPlot::create2CandlePoints(const FCPoint& mp){
        FCChart *chart = getChart();
        int rIndex = m_dataSource->rowsCount();
        if (rIndex > 0){
            ArrayList<BaseShape*> shapesCopy = m_div->getShapes(SortType_DESC);
            for (int b = 0; b < shapesCopy.size(); b++){
                BaseShape *bs = shapesCopy.get(b);
                if(bs->isVisible()){
                    CandleShape *cs = dynamic_cast<CandleShape*>(bs);
                    if(cs){
                        int touchIndex = chart->getIndex(mp);
                        if (touchIndex >= 0 && touchIndex <= chart->getLastVisibleIndex()){
                            int eIndex = touchIndex;
                            int bIndex = eIndex - 1;
                            if (bIndex >= 0){
                                double fDate = m_dataSource->getXValue(bIndex);
                                double sDate = m_dataSource->getXValue(eIndex);
                                clearMarks(&m_marks);
                                double y = getNumberValue(mp);
                                m_marks.put(0, new PlotMark(0, fDate, y));
                                m_marks.put(1, new PlotMark(1, sDate, y));
                                m_sourceFields.put(L"CLOSE", cs->getCloseField());
                                m_sourceFields.put(L"OPEN", cs->getOpenField());
                                m_sourceFields.put(L"HIGH", cs->getHighField());
                                m_sourceFields.put(L"LOW", cs->getLowField());
                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }
    
    bool FCPlot::create3Points(const FCPoint& mp){
        FCChart *chart = getChart();
        int rIndex = m_dataSource->rowsCount();
        if (rIndex > 0){
            int touchIndex = chart->getIndex(mp);
            if (touchIndex >= chart->getFirstVisibleIndex() && touchIndex <= chart->getLastVisibleIndex()){
                int eIndex = touchIndex;
                int bIndex = eIndex - 1;
                if (bIndex >= 0){
                    double fDate = m_dataSource->getXValue(bIndex);
                    double sDate = m_dataSource->getXValue(eIndex);
                    clearMarks(&m_marks);
                    double y = getNumberValue(mp);
                    m_marks.put(0, new PlotMark(0, fDate, y));
                    m_marks.put(1, new PlotMark(1, sDate, y));
                    if (m_div->getVScale(m_attachVScale) && m_div->getVScale(m_attachVScale)->getVisibleMax() != m_div->getVScale(m_attachVScale)->getVisibleMin()){
                        m_marks.put(2, new PlotMark(2, fDate, y + (m_div->getVScale(m_attachVScale)->getVisibleMax() - m_div->getVScale(m_attachVScale)->getVisibleMin()) / 4));
                    }
                    else{
                        m_marks.put(2, new PlotMark(2, fDate, y));
                    }
                    return true;
                }
            }
        }
        return false;
    }
    
    void FCPlot::createCandlePoint(int pos, int index, int close){
        if (index >= 0){
            if (index > m_dataSource->rowsCount() - 1){
                index = m_dataSource->rowsCount() - 1;
            }
            double date = m_dataSource->getXValue(index);
            double yValue = 0;
            if (!FCDataTable::isNaN(m_dataSource->get2(index, close))){
                yValue = m_dataSource->get2(index, close);
            }
            m_marks.put(pos, new PlotMark(pos, date, yValue));
        }
    }
    
    bool FCPlot::create4CandlePoints(const FCPoint& mp){
        FCChart *chart = getChart();
        int rIndex = m_dataSource->rowsCount();
        if (rIndex > 0){
            ArrayList<BaseShape*> shapesCopy = m_div->getShapes(SortType_None);
            for (int b = 0; b < shapesCopy.size(); b++){
                BaseShape *bs = shapesCopy.get(b);
                if(bs->isVisible()){
                    CandleShape *cs = dynamic_cast<CandleShape*>(bs);
                    if(cs){
                        int touchIndex = chart->getIndex(mp);
                        if (touchIndex >= 0 && touchIndex <= chart->getLastVisibleIndex()){
                            int closeField = cs->getCloseField();
                            createCandlePoint(0, touchIndex, closeField);
                            createCandlePoint(1, touchIndex + 1, closeField);
                            createCandlePoint(2, touchIndex + 2, closeField);
                            createCandlePoint(3, touchIndex + 3, closeField);
                            m_sourceFields.put(L"CLOSE", closeField);
                            m_sourceFields.put(L"HIGH", cs->getHighField());
                            m_sourceFields.put(L"LOW", cs->getLowField());
                            m_sourceFields.put(L"OPEN", cs->getOpenField());
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
    
    double* FCPlot::getCandleRange(HashMap<int,PlotMark*> *pList){
        if (pList->size() == 0){
            return 0;
        }
        if(m_sourceFields.containsKey(L"HIGH") || m_sourceFields.containsKey(L"LOW")){
            return 0;
        }
        int bRecord = m_dataSource->getRowIndex(pList->get(0)->Key);
        int eRecord = m_dataSource->getRowIndex(pList->get(1)->Key);
        double *highlist = 0;
        double *lowlist = 0;
        int highlistL = 0, lowlistL = 0;
        if (eRecord >= bRecord){
            highlist = m_dataSource->DATA_ARRAY(m_sourceFields.get(L"HIGH"), eRecord, eRecord - bRecord + 1, &highlistL);
            lowlist = m_dataSource->DATA_ARRAY(m_sourceFields.get(L"LOW"), eRecord, eRecord - bRecord + 1, &lowlistL);
        }
        else{
            highlist = m_dataSource->DATA_ARRAY(m_sourceFields.get(L"HIGH"), bRecord, bRecord - eRecord + 1, &highlistL);
            lowlist = m_dataSource->DATA_ARRAY(m_sourceFields.get(L"LOW"), bRecord, bRecord - eRecord + 1, &lowlistL);
        }
        double nHigh = 0, nLow = 0;
        nHigh = FCScript::maxValue(highlist, highlistL);
        nLow = FCScript::minValue(lowlist, lowlistL);
        if(highlist){
            delete[] highlist;
            highlist = 0;
        }
        if(lowlist){
            delete[] lowlist;
            lowlist = 0;
        }
        double *result = new double[2];
        result[0] = nHigh;
        result[1] = nLow;
        return result;
    }
    
    float* FCPlot::getLineParams(PlotMark *markA, PlotMark *markB){
        float y1 = pY(markA->Value);
        float y2 = pY(markB->Value);
        int bIndex = m_dataSource->getRowIndex(markA->Key);
        int eIndex = m_dataSource->getRowIndex(markB->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        if (x2 - x1 != 0){
            float a = 0,b = 0;
            lineXY(x1, y1, x2, y2, 0, 0, &a, &b);
            float *list = new float[2];
            list[0] = a;
            list[1] = b;
            return list;
        }
        else{
            return 0;
        }
    }
    
    double* FCPlot::getLRBandRange(HashMap<int,PlotMark*> *marks, float *param){
        if (m_sourceFields.containsKey(L"HIGH") && m_sourceFields.containsKey(L"LOW") && param){
            float a = param[0];
            float b = param[1];
            int bIndex = m_dataSource->getRowIndex(marks->get(0)->Key);
            int eIndex = m_dataSource->getRowIndex(marks->get(1)->Key);
            double upSubValue = 0;
            double downSubValue = 0;
            int pos = 0;
            for (int i = bIndex; i <= eIndex; i++){
                double high = m_dataSource->get2(i, m_sourceFields.get(L"HIGH"));
                double low = m_dataSource->get2(i, m_sourceFields.get(L"LOW"));
                if (!FCDataTable::isNaN(high) && !FCDataTable::isNaN(low)){
                    double midValue = (i - bIndex + 1) * a + b;
                    if (pos == 0){
                        upSubValue = high - midValue;
                        downSubValue = midValue - low;
                    }
                    else{
                        if (high - midValue > upSubValue){
                            upSubValue = high - midValue;
                        }
                        if (midValue - low > downSubValue){
                            downSubValue = midValue - low;
                        }
                    }
                    pos++;
                }
            }
            double *list = new double[2];
            list[0] = upSubValue;
            list[1] = downSubValue;
            return list;
        }
        return 0;
    }
    
    float* FCPlot::getLRParams(HashMap<int,PlotMark*> *marks){
        if (m_sourceFields.containsKey(L"CLOSE")){
            int bIndex = m_dataSource->getRowIndex(marks->get(0)->Key);
            int eIndex = m_dataSource->getRowIndex(marks->get(1)->Key);
            if (bIndex != -1 && eIndex != -1){
                double *closeVList = new double[eIndex - bIndex + 1];
                int length = 0;
                for (int i = bIndex; i <= eIndex; i++){
                    double value = m_dataSource->get2(i, m_sourceFields.get(L"CLOSE"));
                    if (!FCDataTable::isNaN(value)){
                        closeVList[length] = value;
                        length++;
                    }
                }
                if (length > 0){
                    float a = 0, b = 0;
                    FCScript::linearRegressionEquation(closeVList, length, &a, &b);
                    delete[] closeVList;
                    closeVList = 0;
                    float *list = new float[2];
                    list[0] = a;
                    list[1] = b;
                    return list;
                }
                else{
                    delete[] closeVList;
                    closeVList = 0;
                }
            }
        }
        return 0;
    }
    
    FCPoint FCPlot::getMovingPoint(){
        FCPoint mp = getTouchOverPoint();
        if (mp.x < 0){
            mp.x = 0;
        }
        else if (mp.x > getWorkingAreaWidth()){
            mp.x = getWorkingAreaWidth();
        }
        if (mp.y < 0){
            mp.y = 0;
        }
        else if (mp.y > getWorkingAreaHeight()){
            mp.y = getWorkingAreaHeight();
        }
        return mp;
    }
    
    double FCPlot::getNumberValue(const FCPoint& mp){
        FCChart *chart = getChart();
        FCPoint newp = mp;
        newp.x += chart->getLeftVScaleWidth();
        newp.y += m_div->getTop() + m_div->getTitleBar()->getHeight();
        return chart->getNumberValue(m_div, newp, m_attachVScale);
    }
    
    int FCPlot::getPx(){
        FCChart *chart = getChart();
        return chart->getLeftVScaleWidth();
    }
    
    int FCPlot::getPy(){
        return m_div->getTitleBar()->getHeight();
    }
    
    FCRect FCPlot::getRectangle(PlotMark *markA, PlotMark *markB){
        double aValue = markA->Value;
        double bValue = markB->Value;
        int aIndex = m_dataSource->getRowIndex(markA->Key);
        int bIndex = m_dataSource->getRowIndex(markB->Key);
        float x = pX(aIndex);
        float y = pY(aValue);
        float xS = pX(bIndex);
        float yS = pY(bValue);
        float width = abs(xS - x);
        if (width < 4){
            width = 4;
        }
        float height = abs(yS - y);
        if (height < 4){
            height = 4;
        }
        float rX = x <= xS ? x : xS;
        float rY = y <= yS ? y : yS;
        FCRect rect ={(int)rX, (int)rY, (int)(rX + width), (int)(rY + height)};
        return rect;
    }
    
    float* FCPlot::goldenRatioParams(double value1, double value2){
        float y1 = pY(value1);
        float y2 = pY(value2);
        float y0 = 0, yA = 0, yB = 0, yC = 0, yD = 0, y100 = 0;
        y0 = y1;
        yA = y1 <= y2 ? y1 + (y2 - y1) * 0.236f : y2 + (y1 - y2) * (1 - 0.236f);
        yB = y1 <= y2 ? y1 + (y2 - y1) * 0.382f : y2 + (y1 - y2) * (1 - 0.382f);
        yC = y1 <= y2 ? y1 + (y2 - y1) * 0.5f : y2 + (y1 - y2) * (1 - 0.5f);
        yD = y1 <= y2 ? y1 + (y2 - y1) * 0.618f : y2 + (y1 - y2) * (1 - 0.618f);
        y100 = y2;
        float *list = new float[6];
        list[0] = y0;
        list[1] = yA;
        list[2] = yB;
        list[3] = yC;
        list[4] = yD;
        list[5] = y100;
        return list;
    }
    
    bool FCPlot::hLinesSelect(float *param, int length){
        FCPoint mp = getTouchOverPoint();
        float top = 0;
        float bottom = (float)getWorkingAreaHeight();
        if (mp.y >= top && mp.y <= bottom){
            for(int i=0;i<length;i++){
                float p = param[i];
                if (mp.y >= p - m_lineWidth * 2.5f && mp.y <= p + m_lineWidth * 2.5f){
                    return true;
                }
            }
        }
        return false;
    }
    
    void FCPlot::move(const FCPoint& mp){
        VScale *vScale = m_div->getVScale(m_attachVScale);
        clearMarks(&m_marks);
        int msize = (int)m_startMarks.size();
        for (int i = 0; i < msize; i++){
            int startIndex = m_dataSource->getRowIndex(m_startMarks.get(i)->Key);
            int newIndex = 0;
            double yAddValue = 0;
            movePlot((float)mp.y, (float)m_startPoint.y, startIndex, getIndex(m_startPoint), getIndex(mp), (float)getWorkingAreaHeight(), vScale->getVisibleMax(),
                     vScale->getVisibleMin(), m_dataSource->rowsCount(), &yAddValue, &newIndex);
            if (vScale->isReverse()){
                m_marks.put(i, new PlotMark(i, m_dataSource->getXValue(newIndex), m_startMarks.get(i)->Value - yAddValue));
            }
            else{
                m_marks.put(i, new PlotMark(i, m_dataSource->getXValue(newIndex), m_startMarks.get(i)->Value + yAddValue));
            }
        }
    }
    
    void FCPlot::onPaint(FCPaint *paint){
        FCChart *chart = getChart();
        FCPoint mp = chart->getTouchPoint();
        FCNative *native = getNative();
        if (chart->getMovingPlot() == this ||  (mp.y >= m_div->getTop() && mp.y <= m_div->getBottom() &&  (chart == native->getHoveredControl() && !chart->isOperating() && onSelect()))){
            onPaint(paint, &m_marks, m_selectedColor);
        }
        else{
            onPaint(paint, &m_marks, m_color);
        }
    }
    
    void FCPlot::onPaintGhost(FCPaint *paint){
        m_isPaintingGhost = true;
        onPaint(paint, &m_startMarks, m_selectedColor);
        m_isPaintingGhost = false;
    }
    
    void FCPlot::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList,Long lineColor){
    }
    
    float FCPlot::pX(int index){
        FCChart *chart = getChart();
        float x = chart->getX(index);
        return x - chart->getLeftVScaleWidth();
    }
    
    float FCPlot::pY(double value){
        FCChart *chart = getChart();
        float y = chart->getY(m_div, value, m_attachVScale);
        return y - m_div->getTitleBar()->getHeight();
    }
    
    float FCPlot::pX(float x){
        return x - getChart()->getLeftVScaleWidth();
    }
    
    float FCPlot::pY(float y){
        return y - m_div->getTop() - m_div->getTitleBar()->getHeight();
    }
    
    void FCPlot::resize(int index){
        FCChart *chart = getChart();
        int touchIndex = chart->getTouchOverIndex();
        double y = getNumberValue(getMovingPoint());
        if (touchIndex < 0){
            touchIndex = 0;
        }
        if (touchIndex > chart->getLastVisibleIndex()){
            touchIndex = chart->getLastVisibleIndex();
        }
        if(m_marks.get(index)){
            delete m_marks.get(index);
        }
        m_marks.put(index, new PlotMark(index, m_dataSource->getXValue(touchIndex), y));
    }
    
    void FCPlot::resize(const FCPoint& mp, FCPoint oppositePoint){
        double sValue = getNumberValue(oppositePoint);
        double eValue = getNumberValue(mp);
        int iS = getIndex(oppositePoint);
        int iE = getIndex(mp);
        double topValue = 0;
        double bottomValue = 0;
        VScale *vScale = m_div->getVScale(m_attachVScale);
        if (sValue >= eValue){
            if (vScale->isReverse()){
                topValue = eValue;
                bottomValue = sValue;
            }
            else{
                topValue = sValue;
                bottomValue = eValue;
            }
        }
        else{
            if (vScale->isReverse()){
                topValue = sValue;
                bottomValue = eValue;
            }
            else{
                topValue = eValue;
                bottomValue = sValue;
            }
        }
        double sDate = 0;
        double eDate = 0;
        if (iS < 0){
            iS = 0;
        }
        else if (iS > m_dataSource->rowsCount() - 1){
            iS = m_dataSource->rowsCount() - 1;
        }
        if (iE < 0){
            iE = 0;
        }
        else if (iE > m_dataSource->rowsCount() - 1){
            iE = m_dataSource->rowsCount() - 1;
        }
        if (iS >= iE){
            sDate = m_dataSource->getXValue(iE);
            eDate = m_dataSource->getXValue(iS);
        }
        else{
            sDate = m_dataSource->getXValue(iS);
            eDate = m_dataSource->getXValue(iE);
        }
        clearMarks(&m_marks);
        m_marks.put(0, new PlotMark(0, sDate, topValue));
        m_marks.put(1, new PlotMark(1, eDate, bottomValue));
    }
    
    bool FCPlot::selectPoint(const FCPoint& mp, float x, float y){
        if (mp.x >= x - m_lineWidth * 6 && mp.x <= x + m_lineWidth * 6
            && mp.y >= y - m_lineWidth * 6 && mp.y <= y + m_lineWidth * 6){
            return true;
        }
        return false;
    }
    
    bool FCPlot::selectLine(const FCPoint& mp, float x, float k, float b){
        if (!(k == 0 && b == 0)){
            if (mp.y / (mp.x * k + b) >= 0.95 && mp.y / (mp.x * k + b) <= 1.05){
                return true;
            }
        }
        else{
            if (mp.x >= x - m_lineWidth * 5 && mp.x <= x + m_lineWidth * 5){
                return true;
            }
        }
        return false;
    }
    
    bool FCPlot::selectLine(const FCPoint& mp, float x1, float y1, float x2, float y2){
        float k = 0, b = 0;
        lineXY(x1, y1, x2, y2, 0, 0, &k, &b);
        return selectLine(mp, x1, k, b);
    }
    
    bool FCPlot::selectRay(const FCPoint& mp, float x1, float y1, float x2, float y2, float *pk, float *pb){
        lineXY(x1, y1, x2, y2, 0, 0, pk, pb);
        float k = *pk, b = *pb;
        if (!(k == 0 && b == 0)){
            if (mp.y / (mp.x * k + b) >= 0.95 && mp.y / (mp.x * k + b) <= 1.05){
                if (x1 >= x2){
                    if (mp.x > x1 + 5) return false;
                }
                else if (x1 < x2){
                    if (mp.x < x1 - 5) return false;
                }
                return true;
            }
        }
        else{
            if (mp.x >= x1 - m_lineWidth * 5 && mp.x <= x1 + m_lineWidth * 5){
                if (y1 >= y2){
                    if (mp.y <= y1 - 5){
                        return true;
                    }
                }
                else{
                    if (mp.y >= y1 - 5){
                        return true;
                    }
                }
            }
        }
        return false;
    }
    
    bool FCPlot::selectRay(const FCPoint& mp, float x1, float y1, float x2, float y2){
        float k = 0 ,b = 0;
        return selectRay(mp, x1, y1, x2, y2, &k, &b);
    }
    
    ActionType FCPlot::selectRect(const FCPoint& mp, PlotMark *markA, PlotMark *markB){
        FCRect rect = getRectangle(markA, markB);
        int x1 = rect.left;
        int y1 = rect.top;
        int x2 = rect.right;
        int y2 = rect.top;
        int x3 = rect.left;
        int y3 = rect.bottom;
        int x4 = rect.right;
        int y4 = rect.bottom;
        if (selectPoint(mp, (float)x1, (float)y1)){
            return ActionType_AT1;
        }
        else if (selectPoint(mp, (float)x2, (float)y2)){
            return ActionType_AT2;
        }
        else if (selectPoint(mp, (float)x3, (float)y3)){
            return ActionType_AT3;
        }
        else if (selectPoint(mp, (float)x4, (float)y4)){
            return ActionType_AT4;
        }
        else{
            int sub = (int)(m_lineWidth * 2.5);
            FCRect bigRect ={rect.left - sub, rect.top - sub, rect.right + sub, rect.bottom + sub};
            if (mp.x >= bigRect.left && mp.x <= bigRect.right && mp.y >= bigRect.top && mp.y <= bigRect.bottom){
                if (rect.right - rect.left <= 4 || rect.bottom - rect.top <= 4){
                    return ActionType_MOVE;
                }
                else{
                    FCRect smallRect ={rect.left + sub, rect.top + sub, rect.right - sub, rect.bottom - sub};
                    if (!(mp.x >= smallRect.left && mp.x <= smallRect.right && mp.y >= smallRect.top && mp.y <= smallRect.bottom)){
                        return ActionType_MOVE;
                    }
                }
            }
        }
        return ActionType_NO;
    }
    
    bool FCPlot::selectSegment(const FCPoint& mp, float x1, float y1, float x2, float y2){
        float k = 0,b = 0;
        lineXY(x1, y1, x2, y2, 0, 0, &k, &b);
        float smallX = x1 <= x2 ? x1 : x2;
        float smallY = y1 <= y2 ? y1 : y2;
        float bigX = x1 > x2 ? x1 : x2;
        float bigY = y1 > y2 ? y1 : y2;
        if (mp.x >= smallX - 2 && mp.x <= bigX + 2 && mp.y >= smallY - 2 && mp.y <= bigY + 2){
            if (!(k == 0 && b == 0)){
                if (mp.y / (mp.x * k + b) >= 0.95 && mp.y / (mp.x * k + b) <= 1.05){
                    return true;
                }
            }
            else{
                if (mp.x >= x1 - m_lineWidth * 5 && mp.x <= x1 + m_lineWidth * 5){
                    return true;
                }
            }
        }
        return false;
    }
    
    bool FCPlot::selectSine(const FCPoint& mp, float x1, float y1, float x2, float y2){
        double f = 2.0 * 3.14159 / ((x2 - x1) * 4);
        int len = getWorkingAreaWidth();
        if (len > 0){
            float lastX = 0, lastY = 0;
            for (int i = 0; i < len; i++){
                float x = -x1 + i;
                float y = (float)(0.5 * (y2 - y1) * sin(x * f) * 2);
                float px = x + x1, py = y + y1;
                if (i == 0){
                    if (selectPoint(mp, px, py)){
                        return true;
                    }
                }
                else{
                    float rectLeft = lastX - 2;
                    float rectTop = lastY <= py ? lastY : py - 2;
                    float rectRight = rectLeft + abs(px - lastX) + 4;
                    float rectBottom = rectTop + abs(py - lastY) + 4;
                    if (mp.x >= rectLeft && mp.x <= rectRight && mp.y >= rectTop && mp.y <= rectBottom){
                        return true;
                    }
                }
                lastX = px;
                lastY = py;
            }
        }
        return false;
    }
    
    bool FCPlot::selectTriangle(const FCPoint& mp, float x1, float y1, float x2, float y2, float x3, float y3){
        bool selected = selectSegment(mp, x1, y1, x2, y2);
        if (selected) return true;
        selected = selectSegment(mp, x1, y1, x3, y3);
        if (selected) return true;
        selected = selectSegment(mp, x2, y2, x3, y3);
        if (selected) return true;
        return false;
    }
    
    void FCPlot::setCursor(FCCursors cursor){
        m_div->getChart()->setCursor(cursor);
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    ActionType FCPlot::getAction(){
        return ActionType_NO;
    }
    
    bool FCPlot::onCreate(const FCPoint& mp){
        return false;
    }
    
    void FCPlot::onMoveBegin(){
    }
    
    void FCPlot::onMoveEnd(){
    }
    
    void FCPlot::onMoveStart(){
    }
    
    void FCPlot::onMoving(){
        FCPoint mp = getMovingPoint();
        switch (m_action){
            case ActionType_MOVE:
                move(mp);
                break;
            case ActionType_AT1:
                resize(0);
                break;
            case ActionType_AT2:
                resize(1);
                break;
            case ActionType_AT3:
                resize(2);
                break;
            case ActionType_AT4:
                resize(3);
                break;
        }
    }
    
    bool FCPlot::onSelect(){
        ActionType action = getAction();
        if(action != ActionType_NO){
            return true;
        }
        else{
            return false;
        }
    }
    
    void FCPlot::render(FCPaint *paint){
        FCChart *chart = getChart();
        if(m_drawGhost && chart->getMovingPlot() && chart->getMovingPlot() == this){
            onPaintGhost(paint);
        }
        onPaint(paint);
    }
    
    void FCPlot::ellipseAB(float width,  float height,  float *a,  float *b){
        *a = width / 2;
        *b = height / 2;
    }
    
    bool FCPlot::ellipseHasPoint(float x, float y, float oX, float oY, float a, float b){
        x -= oX;
        y -= oY;
        if(a == 0 && b == 0 && x == 0 && y == 0)
        {
            return TRUE;
        }
        if(a == 0)
        {
            if(x == 0 && y >= -b && y <= b)
            {
                return FALSE;
            }
        }
        if(b == 0)
        {
            if(y == 0 && x >= -a && x <= a)
            {
                return TRUE;
            }
        }
        if((x * x) / (a * a) + (y * y) / (b * b) >= 0.8 && (x * x)/(a * a)+(y * y)/(b * b) <= 1.2)
        {
            return TRUE;
        }
        return FALSE;
    }
    
    void FCPlot::ellipseOR(float x1, float y1, float x2, float y2, float x3, float y3, float *oX, float *oY, float *r){
        *oX = ((y3 - y1) * (y2 * y2 - y1 * y1 + x2 * x2 - x1 * x1) + (y2 - y1) * (y1 * y1 - y3 * y3 + x1 * x1 - x3 * x3))
        / (2 * (x2 - x1) * (y3 - y1) - 2 * (x3 - x1) * (y2 - y1));
        *oY = ((x3 - x1) * (x2 * x2 - x1 * x1 + y2 * y2 - y1 * y1) + (x2 - x1) * (x1 * x1 - x3 * x3 + y1 * y1 - y3 * y3))
        / (2 * (y2 - y1) * (x3 - x1) - 2 * (y3 - y1) * (x2 - x1));
        *r = (float)sqrt((x1 - *oX) * (x1 - *oX) + (y1 - *oY) * (y1 - *oY));
    }
    
    double FCPlot::lineSlope(float x1,  float y1,  float x2,  float y2,  float oX,  float oY){
        if((x1 - oX) != (x2 - oX)){
            return ((y2 - oY) - (y1 - oY)) / ((x2 - oX) - (x1 - oX));
        }
        return 0;
    }
    
    void FCPlot::lineXY(float x1,  float y1,  float x2,  float y2,  float oX,  float oY,  float *k,  float *b){
        *k = 0;
        *b = 0;
        if((x1 - oX) != (x2 - oX)){
            *k = ((y2 - oY) - (y1 - oY)) / ((x2 - oX) - (x1 - oX));
            *b = (y1 - oY) - *k * (x1 - oX);
        }
    }
    
    void FCPlot::parallelogram(float x1, float y1, float x2, float y2, float x3, float y3, float *x4, float *y4){
        *x4 = x1 + x3 - x2;
        *y4 = y1 + y3 - y2;
    }
    
    void FCPlot::rectangleXYWH(int x1, int y1, int x2, int y2, int *x, int *y, int *w, int *h){
        *x = x1 < x2 ? x1 : x2;
        *y = y1 < y2 ? y1 : y2;
        *w = abs(x1 - x2);
        *h = abs(y1 - y2);
        if ((*w) <= 0){
            *w = 4;
        }
        if ((*h) <= 0){
            *h = 4;
        }
    }
}
