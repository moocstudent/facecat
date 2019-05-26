#include "stdafx.h"
#include "FCPlot.h"
#include "PExtend.h"
#include "FCScript.h"

namespace FaceCat{
    AndrewSpitchfork::AndrewSpitchfork(){
        m_plotType = L"ANDREWSPITCHFORK";
    }
    
    ActionType AndrewSpitchfork::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        if(!m_sourceFields.containsKey(L"CLOSE")){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        int aIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int bIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
        int cIndex = m_dataSource->getRowIndex(m_marks.get(2)->Key);
        int dIndex = m_dataSource->getRowIndex(m_marks.get(3)->Key);
        float x1 = pX(aIndex);
        float x2 = pX(bIndex);
        float x3 = pX(cIndex);
        float x4 = pX(dIndex);
        float y1 = pY(m_dataSource->get2(aIndex, m_sourceFields.get(L"CLOSE")));
        float y2 = pY(m_marks.get(1)->Value);
        float y3 = pY(m_dataSource->get2(cIndex, m_sourceFields.get(L"CLOSE")));
        float y4 = pY(m_dataSource->get2(dIndex, m_sourceFields.get(L"CLOSE")));
        bool selected = selectPoint(mp, x1, y1);
        if (selected){
            action = ActionType_AT1;
            return action;
        }
        selected = selectPoint(mp, x2, y2);
        if (selected){
            action = ActionType_AT2;
            return action;
        }
        selected = selectPoint(mp, x3, y3);
        if (selected){
            action = ActionType_AT3;
            return action;
        }
        selected = selectPoint(mp, x4, y4);
        if (selected){
            action = ActionType_AT4;
            return action;
        }
        float k = 0, b = 0;
        selected = selectRay(mp, x1, y1, x2, y2, &k, &b);
        if (selected){
            action = ActionType_MOVE;
            return action;
        }
        int wWidth = getWorkingAreaWidth();
        if (k != 0 || b != 0){
            float x3_newx = (float)wWidth;
            if (bIndex < aIndex){
                x3_newx = 0;
            }
            b = y3 - x3 * k;
            float x3_newy = k * x3_newx + b;
            selected = selectRay(mp, x3, y3, x3_newx, x3_newy);
            if (selected){
                action = ActionType_MOVE;
                return action;
            }
            float x4_newx = (float)wWidth;
            if (bIndex < aIndex){
                x4_newx = 0;
            }
            b = y4 - x4 * k;
            float x4_newy = k * x4_newx + b;
            selected = selectRay(mp, x4, y4, x4_newx, x4_newy);
            if (selected){
                action = ActionType_MOVE;
                return action;
            }
        }
        else{
            if (y1 >= y2){
                selected = selectRay(mp, x3, y3, x3, 0);
                if (selected){
                    action = ActionType_MOVE;
                    return action;
                }
                selected = selectRay(mp, x4, y4, x4, 0);
                if (selected){
                    action = ActionType_MOVE;
                    return action;
                }
            }
            else{
                int wHeight = getWorkingAreaHeight();
                selected = selectRay(mp, x3, y3, x3, (float)wHeight);
                if (selected){
                    action = ActionType_MOVE;
                    return action;
                }
                selected = selectRay(mp, x4, y4, x4, (float)wHeight);
                if (selected){
                    action = ActionType_MOVE;
                    return action;
                }
            }
        }
        return action;
    }
    
    bool AndrewSpitchfork::onCreate(const FCPoint& mp){
        bool create = create4CandlePoints(mp);
        if(create){
            m_marks.put(1, new PlotMark(m_marks.get(1)->Index, m_marks.get(1)->Key, getNumberValue(mp)));
        }
        return create;
    }
    
    void AndrewSpitchfork::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_MOVE){
                setCursor(FCCursors_Hand);
            }
            else{
                setCursor(FCCursors_SizeWE);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
            m_startMarks.put(2, m_marks.get(2)->copy());
            m_startMarks.put(3, m_marks.get(3)->copy());
        }
    }
    
    void AndrewSpitchfork::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        if(!m_sourceFields.containsKey(L"CLOSE") ){
            return;
        }
        int aIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int bIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
        int cIndex = m_dataSource->getRowIndex(m_marks.get(2)->Key);
        int dIndex = m_dataSource->getRowIndex(m_marks.get(3)->Key);
        float x1 = pX(aIndex);
        float x2 = pX(bIndex);
        float x3 = pX(cIndex);
        float x4 = pX(dIndex);
        float y1 = pY(m_dataSource->get2(aIndex, m_sourceFields.get(L"CLOSE")));
        float y2 = pY(m_marks.get(1)->Value);
        float y3 = pY(m_dataSource->get2(cIndex, m_sourceFields.get(L"CLOSE")));
        float y4 = pY(m_dataSource->get2(dIndex, m_sourceFields.get(L"CLOSE")));
        float k = 0, b = 0;
        lineXY(x1, y1, x2, y2, 0, 0, &k, &b);
        drawRay(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2, k, b);
        if (k != 0 || b != 0){
            float x3_newx = (float)getWorkingAreaWidth();
            if (bIndex < aIndex){
                x3_newx = 0;
            }
            b = y3 - x3 * k;
            float x3_newy = k * x3_newx + b;
            drawRay(paint, lineColor, m_lineWidth, m_lineStyle, x3, y3, x3_newx, x3_newy, k, b);
            float x4_newx = (float)getWorkingAreaWidth();
            if (bIndex < aIndex){
                x4_newx = 0;
            }
            b = y4 - x4 * k;
            float x4_newy = k * x4_newx + b;
            drawRay(paint, lineColor, m_lineWidth, m_lineStyle, x4, y4, x4_newx, x4_newy, k, b);
        }
        else{
            if (y1 >= y2){
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x3, y3, x3, (float)0);
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x4, y4, x4, (float)0);
            }
            else{
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x3, y3, x3, (float)getWorkingAreaHeight());
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x4, y4, x4, (float)getWorkingAreaHeight());
            }
        }
        if (m_selected){
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x2, y2);
            drawSelect(paint, lineColor, x3, y3);
            drawSelect(paint, lineColor, x4, y4);
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    AngleLine::AngleLine(){
        m_plotType = L"ANGLELINE";
    }
    
    ActionType AngleLine::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        for (int i = 0; i < 2; i++){
            PlotMark *markA = m_marks.get(0);
            PlotMark *markB = m_marks.get(i + 1);
            int bIndex = m_dataSource->getRowIndex(markA->Key);
            int eIndex = m_dataSource->getRowIndex(markB->Key);
            float x1 = pX(bIndex);
            float x2 = pX(eIndex);
            float y1 = pY(markA->Value);
            float y2 = pY(markB->Value);
            if (selectPoint(mp, x1, y1)){
                action = ActionType_AT1;
                return action;
            }
            else if (selectPoint(mp, x2, y2)){
                if (i == 0){
                    action = ActionType_AT2;
                    return action;
                }
                else{
                    action = ActionType_AT3;
                    return action;
                }
            }
            float k = 0;
            float b = 0;
            lineXY(x1, y1, x2, y2, 0, 0, &k, &b);
            if (!(k == 0 && b == 0)){
                if (mp.y / (mp.x * k + b) >= 0.9 && mp.y / (mp.x * k + b) <= 1.1){
                    if (x1 >= x2){
                        if (mp.x > x1 + 5){
                            ActionType action = ActionType_NO;
                            return action;
                        }
                    }
                    else if (x1 < x2){
                        if (mp.x < x1 - 5){
                            ActionType action = ActionType_NO;
                            return action;
                        }
                    }
                    action = ActionType_MOVE;
                    return action;
                }
            }
            else{
                if (mp.x >= x1 - m_lineWidth * 5 && mp.x <= x1 + m_lineWidth * 5){
                    if (y1 >= y2){
                        if (mp.y <= y1 - 5){
                            action = ActionType_MOVE;
                            return action;
                        }
                    }
                    else{
                        if (mp.y >= y1 - 5){
                            action = ActionType_MOVE;
                            return action;
                        }
                    }
                }
            }
        }
        return action;
    }
    
    bool AngleLine::onCreate(const FCPoint& mp){
        return create3Points(mp);
    }
    
    void AngleLine::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_MOVE){
                setCursor(FCCursors_Hand);
            }
            else{
                setCursor(FCCursors_SizeWE);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
            m_startMarks.put(2, m_marks.get(2)->copy());
        }
    }
    
    void AngleLine::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        for (int i = 0; i < 2; i++){
            PlotMark *markA = pList->get(0);
            PlotMark *markB = pList->get(i + 1);
            float y1 = pY(markA->Value);
            float y2 = pY(markB->Value);
            int bIndex = m_dataSource->getRowIndex(markA->Key);
            int eIndex = m_dataSource->getRowIndex(markB->Key);
            float *param = getLineParams(markA, markB);
            float a = 0;
            float b = 0;
            float x1 = pX(bIndex);
            float x2 = pX(eIndex);
            if (param){
                a = param[0];
                b = param[1];
                float leftX = 0;
                float leftY = leftX * a + b;
                float rightX = (float)getWorkingAreaWidth();
                float rightY = rightX * a + b;
                if (x1 >= x2){
                    drawLine(paint, lineColor, m_lineWidth, m_lineStyle, leftX, leftY, x2, y2);
                }
                else{
                    drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, rightX, rightY);
                }
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
                delete[] param;
                param = 0;
            }
            else{
                if (y1 >= y2){
                    drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, (float)0);
                }
                else{
                    drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, (float)getWorkingAreaHeight());
                }
            }
            if (m_selected){
                drawSelect(paint, lineColor, x1, y1);
                drawSelect(paint, lineColor, x2, y2);
            }
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    CircumCircle::CircumCircle(){
        m_plotType = L"CIRCUMCIRCLE";
    }
    
    ActionType CircumCircle::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        float y1 = pY(m_marks.get(0)->Value);
        float y2 = pY(m_marks.get(1)->Value);
        float y3 = pY(m_marks.get(2)->Value);
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
        int pIndex = m_dataSource->getRowIndex(m_marks.get(2)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float x3 = pX(pIndex);
        float Ox = 0, Oy = 0, r = 0;
        ellipseOR(x1, y1, x2, y2, x3, y3, &Ox, &Oy, &r);
        float clickX = mp.x - Ox;
        float clickY = mp.y - Oy;
        double ellipseValue = clickX * clickX + clickY * clickY;
        if (ellipseValue >= r * r * 0.8 && ellipseValue <= r * r * 1.2){
            if (selectPoint(mp, x1, y1)){
                action = ActionType_AT1;
            }
            else if (selectPoint(mp, x2, y2)){
                action = ActionType_AT2;
            }
            else if (selectPoint(mp, x3, y3)){
                action = ActionType_AT3;
            }
            else{
                action = ActionType_MOVE;
            }
        }
        return action;
    }
    
    bool CircumCircle::onCreate(const FCPoint& mp){
        return create3Points(mp);
    }
    
    void CircumCircle::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_MOVE){
                setCursor(FCCursors_Hand);
            }
            else{
                setCursor(FCCursors_SizeWE);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
            m_startMarks.put(2, m_marks.get(2)->copy());
        }
    }
    
    void CircumCircle::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        float y1 = pY(pList->get(0)->Value);
        float y2 = pY(pList->get(1)->Value);
        float y3 = pY(pList->get(2)->Value);
        int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(pList->get(1)->Key);
        int pIndex = m_dataSource->getRowIndex(pList->get(2)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float x3 = pX(pIndex);
        float Ox = 0, Oy = 0, r = 0;
        ellipseOR(x1, y1, x2, y2, x3, y3, &Ox, &Oy, &r);
        drawEllipse(paint, lineColor, m_lineWidth, m_lineStyle, Ox - r, Oy - r, Ox + r, Oy + r);
        if (m_selected){
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x2, y2, x3, y3);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x3, y3, x1, y1);
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x2, y2);
            drawSelect(paint, lineColor, x3, y3);
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    ArrowSegment::ArrowSegment(){
        m_plotType = L"ARROWSEGMENT";
    }
    
    ActionType ArrowSegment::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        float y1 = pY(m_marks.get(0)->Value);
        float y2 = pY(m_marks.get(1)->Value);
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float *param = getLineParams(m_marks.get(0), m_marks.get(1));
        if (param){
            delete[] param;
            param = 0;
            if (mp.x >= x1 - m_lineWidth * 2.5f && mp.x <= x1 + m_lineWidth * 2.5f){
                action = ActionType_AT1;
                return action;
            }
            else if (mp.x >= x2 - m_lineWidth * 2.5f && mp.x <= x2 + m_lineWidth * 2.5f){
                action = ActionType_AT2;
                return action;
            }
        }
        else{
            if (mp.y >= y1 - m_lineWidth * 5 && mp.y <= y1 + m_lineWidth * 5){
                action = ActionType_AT1;
                return action;
            }
            else if (mp.y >= y2 - m_lineWidth * 5 && mp.y <= y2 + m_lineWidth * 5){
                action = ActionType_AT2;
                return action;
            }
        }
        if (selectSegment(mp, x1, y1, x2, y2)){
            action = ActionType_MOVE;
        }
        return action;
    }
    
    bool ArrowSegment::onCreate(const FCPoint& mp){
        return create2PointsB(mp);
    }
    
    void ArrowSegment::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_MOVE){
                setCursor(FCCursors_Hand);
            }
            else{
                setCursor(FCCursors_SizeWE);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
        }
    }
    
    void ArrowSegment::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        float y1 = pY(pList->get(0)->Value);
        float y2 = pY(pList->get(1)->Value);
        int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(pList->get(1)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
        const float ARROW_Size = 8;
        double slopy, cosy , siny;
        slopy = atan2((double)(y1 - y2), (double)(x1 - x2));
        cosy = cos(slopy);
        siny = sin(slopy);
        FCPoint ptPoint;
        ptPoint.x = (int)x2;
        ptPoint.y = (int)y2;
        FCPoint pts[3];
        pts[0] = ptPoint;
        pts[1].x = ptPoint.x + (int)(ARROW_Size * cosy - (ARROW_Size / 2.0 * siny) + 0.5);
        pts[1].y = ptPoint.y + (int)(ARROW_Size * siny + (ARROW_Size / 2.0 * cosy) + 0.5);
        pts[2].x = ptPoint.x + (int)(ARROW_Size * cosy + ARROW_Size / 2.0 * siny + 0.5);
        pts[2].y = ptPoint.y - (int)(ARROW_Size / 2.0 * cosy - ARROW_Size * siny + 0.5);
        fillPolygon(paint, lineColor, pts, 3);
        if (m_selected){
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x2, y2);
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    FCRect DownArrow::getDownArrowRect(float x, float y, float width){
        if(width>10){
            width = 14;
        }
        int mleft = (int)(x - width / 2);
        int mtop = (int)(y - width * 3 / 2);
        FCRect markRect ={(int)mleft, (int)mtop, (int)(mleft + width), (int)(mtop + width * 3 / 2)};
        return markRect;
    }
    
    DownArrow::DownArrow(){
        m_plotType = L"DOWNARROW";
    }
    
    ActionType DownArrow::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        double fValue = m_marks.get(0)->Value;
        int aIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        float x1 = pX(aIndex);
        float y1 = pY(fValue);
        FCRect rect ={(int)x1 - 5, (int)y1 - 10, (int)x1 + 5, (int)y1};
        FCPoint mp = getTouchOverPoint();
        if (mp.x >= rect.left && mp.x <= rect.right && mp.y >= rect.top && mp.y <= rect.bottom){
            action = ActionType_MOVE;
        }
        return action;
    }
    
    bool DownArrow::onCreate(const FCPoint& mp){
        return createPoint(mp);
    }
    
    void DownArrow::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            setCursor(FCCursors_Hand);
            m_startMarks.put(0, m_marks.get(0)->copy());
        }
    }
    
    void DownArrow::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        double fValue = pList->get(0)->Value;
        int aIndex = m_dataSource->getRowIndex(pList->get(0)->Key) ;
        float x1 = pX(aIndex);
        float y1 = pY(fValue);
        int width = 10;
        FCPoint points[7];
        FCPoint p1 ={(int)x1, (int)y1};
        FCPoint p2 ={(int)(x1 + width / 2), (int)(y1 - width)};
        FCPoint p3 ={(int)(x1 + width / 4), (int)(y1 - width)};
        FCPoint p4 ={(int)(x1 + width / 4), (int)(y1 - width * 3 / 2)};
        FCPoint p5 ={(int)(x1 - width / 4), (int)(y1 - width * 3 / 2)};
        FCPoint p6 ={(int)(x1 - width / 4), (int)(y1 - width)};
        FCPoint p7 ={(int)(x1 - width / 2), (int)(y1 - width)};
        points[0] = p1;
        points[1] = p2;
        points[2] = p3;
        points[3] = p4;
        points[4] = p5;
        points[5] = p6;
        points[6] = p7;
        fillPolygon(paint, lineColor, points, 7);
        if (m_selected){
            drawSelect(paint, lineColor, x1, y1);
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    float* DropLine::getDropLineParams(HashMap<int, PlotMark*> *pList){
        if (pList->size() == 0){
            return 0;
        }
        float y1 = pY(pList->get(0)->Value);
        int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        float x1 = pX(bIndex);
        float a = 1;
        float b = y1 - x1;
        float *param = new float[2];
        param[0] = a;
        param[1] = b;
        return param;
    }
    
    DropLine::DropLine(){
        m_plotType = L"DROPLINE";
    }
    
    ActionType DropLine::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        float *param = getDropLineParams(&m_marks);
        if (param){
            if (mp.y - param[0] * mp.x - param[1] >= m_lineWidth * -5 && mp.y - param[0] * mp.x - param[1] <= m_lineWidth * 5){
                action = ActionType_MOVE;
            }
            delete[] param;
            param = 0;
        }
        return action;
    }
    
    bool DropLine::onCreate(const FCPoint& mp){
        return createPoint(mp);
    }
    
    void DropLine::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            setCursor(FCCursors_Hand);
            m_startMarks.put(0, m_marks.get(0)->copy());
        }
    }
    
    void DropLine::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        float *param = getDropLineParams(pList);
        if(param){
            float a = param[0];
            float b = param[1];
            delete[] param;
            param = 0;
            float leftX = 0;
            float leftY = leftX * a + b;
            float rightX = (float)getWorkingAreaWidth();
            float rightY = rightX * a + b;
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, leftX, leftY, rightX, rightY);
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    Ellipse::Ellipse(){
        m_plotType = L"ELLIPSE";
    }
    
    ActionType Ellipse::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        double fValue = m_marks.get(0)->Value;
        double eValue = m_marks.get(1)->Value;
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float y1 = pY(fValue);
        float y2 = pY(eValue);
        float x = 0;
        float y = 0;
        if (x1 >= x2){
            x = x2;
        }
        else{
            x = x2 - (x2 - x1) * 2;
        }
        if(y1>=y2){
            y = y1 - (y1 - y2) * 2;
        }
        else{
            y = y1;
        }
        if (selectPoint(mp, x1, y1)){
            action = ActionType_AT1;
            return action;
        }
        else if (selectPoint(mp, x2, y2)){
            action = ActionType_AT2;
            return action;
        }
        float width = abs((x1 - x2) * 2);
        float height = abs((y1 - y2) * 2);
        float oX = x + width / 2;
        float oY = y + height / 2;
        float a = 0, b = 0;
        ellipseAB(width, height, &a, &b);
        if (a != 0 && b != 0){
            float clickX = mp.x - oX;
            float clickY = mp.y - oY;
            double ellipseValue = clickX * clickX / (a * a) + clickY * clickY / (b * b);
            if (ellipseValue >= 0.8 && ellipseValue <= 1.2){
                action = ActionType_MOVE;
            }
        }
        return action;
    }
    
    bool Ellipse::onCreate(const FCPoint& mp){
        return create2PointsB(mp);
    }
    
    void Ellipse::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_AT1){
                setCursor(FCCursors_SizeNS);
            }
            else if (m_action == ActionType_AT2){
                setCursor(FCCursors_SizeWE);
            }
            else{
                setCursor(FCCursors_Hand);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
        }
    }
    
    void Ellipse::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        double fValue = pList->get(0)->Value;
        double eValue = pList->get(1)->Value;
        int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(pList->get(1)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float y1 = pY(fValue);
        float y2 = pY(eValue);
        float x = x1 - (x1 - x2);
        float y = 0;
        float width = (x1 - x2) * 2;
        float height = 0;
        if (y1 >= y2)
            height = (y1 - y2) * 2;
        else
            height = (y2 - y1) * 2;
        y = y2 - height / 2;
        if (width == 0)
            width = 1;
        if (height == 0)
            height = 1;
        if (width == 1 && height == 1){
            drawEllipse(paint, lineColor, m_lineWidth, m_lineStyle, x - 2, y - 2, x + 2, y + 2);
        }
        else{
            drawEllipse(paint, lineColor, m_lineWidth, m_lineStyle,  x, y, x + width, y + height);
        }
        if (m_selected){
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x2, y2);
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    float* FiboEllipse::fibonacciEllipseParam(HashMap<int,PlotMark*> *pList){
        if (pList->size() == 0){
            return 0;
        }
        int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(pList->get(1)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float y1 = pY(pList->get(0)->Value);
        float y2 = pY(pList->get(1)->Value);
        float r1 = (float)(sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1)));
        float r2 = r1 * 0.236f;
        float r3 = r1 * 0.382f;
        float r4 = r1 * 0.5f;
        float r5 = r1 * 0.618f;
        float *list = new float[9];
        list[0] = x1;
        list[1] = y1;
        list[2] = x2;
        list[3] = y2;
        list[4] = r1;
        list[5] = r2;
        list[6] = r3;
        list[7] = r4;
        list[8] = r5;
        return list;
    }
    
    FiboEllipse::FiboEllipse(){
        m_plotType = L"FIBOELLIPSE";
    }
    
    ActionType FiboEllipse::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        float *param = fibonacciEllipseParam(&m_marks);
        float x1 = param[0];
        float y1 = param[1];
        float x2 = param[2];
        float y2 = param[3];
        if (selectPoint(mp, x1, y1) || m_moveTimes == 1){
            action = ActionType_AT1;
            delete[] param;
            param = 0;
            return action;
        }
        else if (selectPoint(mp, x2, y2)){
            action = ActionType_AT2;
            delete[] param;
            param = 0;
            return action;
        }
        if (selectSegment(mp, x1, y1, x2, y2)){
            action = ActionType_MOVE;
            delete[] param;
            param = 0;
            return action;
        }
        FCPoint p ={mp.x - (int)x1, mp.y - (int)y1};
        float round = (float)(p.x * p.x + p.y * p.y);
        for (int i = 4; i < 9; i++){
            float r = param[i];
            if (round / (r * r) >= 0.9 && round / (r * r) <= 1.1){
                action = ActionType_MOVE;
                return action;
            }
        }
        delete[] param;
        param = 0;
        return action;
    }
    
    bool FiboEllipse::onCreate(const FCPoint& mp){
        return create2PointsB(mp);
    }
    
    void FiboEllipse::onMoveStart(){
        m_moveTimes++;
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_MOVE){
                setCursor(FCCursors_Hand);
            }
            else{
                setCursor(FCCursors_SizeNS);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
        }
    }
    
    void FiboEllipse::onPaintGhost(FCPaint *paint){
        if (m_moveTimes > 1){
            onPaint(paint, &m_startMarks, m_selectedColor);
        }
    }
    
    void FiboEllipse::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        float *param = fibonacciEllipseParam(pList);
        float x1 = param[0];
        float y1 = param[1];
        float x2 = param[2];
        float y2 = param[3];
        drawLine(paint, lineColor, m_lineWidth, 2, x1, y1, x2, y2);
        float r1 = param[4] >= 4 ? param[4] : 4;
        float r2 = param[5] >= 4 ? param[5] : 4;
        float r3 = param[6] >= 4 ? param[6] : 4;
        float r4 = param[7] >= 4 ? param[7] : 4;
        float r5 = param[8] >= 4 ? param[8] : 4;
        delete[] param;
        param = 0;
        drawEllipse(paint, lineColor, m_lineWidth, m_lineStyle, x1 - r1, y1 - r1, x1 + r1, y1 + r1);
        drawEllipse(paint, lineColor, m_lineWidth, m_lineStyle, x1 - r2, y1 - r2, x1 + r2, y1 + r2);
        drawEllipse(paint, lineColor, m_lineWidth, m_lineStyle, x1 - r3, y1 - r3, x1 + r3, y1 + r3);
        drawEllipse(paint, lineColor, m_lineWidth, m_lineStyle, x1 - r4, y1 - r4, x1 + r4, y1 + r4);
        drawEllipse(paint, lineColor, m_lineWidth, m_lineStyle, x1 - r5, y1 - r5, x1 + r5, y1 + r5);
        if (m_selected){
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x2, y2);
        }
        if (r5 > 20){
            FCSize sizeF = textSize(paint, L"23.6%", m_font);
            FCPlot::drawText(paint, L"23.6%", lineColor, m_font, (int)(x1 - sizeF.cx / 2), (int)(y1 - r2 - sizeF.cy));
            sizeF = textSize(paint, L"38.2%", m_font);
            FCPlot::drawText(paint, L"38.2%", lineColor, m_font, (int)(x1 - sizeF.cx / 2), (int)(y1 - r3 - sizeF.cy));
            sizeF = textSize(paint, L"50.0%", m_font);
            FCPlot::drawText(paint, L"50.0%", lineColor, m_font, (int)(x1 - sizeF.cx / 2), (int)(y1 - r4 - sizeF.cy));
            sizeF = textSize(paint, L"61.8%", m_font);
            FCPlot::drawText(paint, L"61.8%", lineColor, m_font, (int)(x1 - sizeF.cx / 2), (int)(y1 - r5 - sizeF.cy));
            sizeF = textSize(paint, L"100%" ,m_font);
            FCPlot::drawText(paint, L"100%", lineColor, m_font, (int)(x1 - sizeF.cx / 2), (int)(y1 - r1 - sizeF.cy));
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    FiboFanline::FiboFanline(){
        m_plotType = L"FIBOFANLINE";
    }
    
    ActionType FiboFanline::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        float y1 = pY(m_marks.get(0)->Value);
        float y2 = pY(m_marks.get(1)->Value);
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        if (selectPoint(mp, x1, y1) || m_moveTimes == 1){
            action = ActionType_AT1;
            return action;
        }
        else if (selectPoint(mp, x2, y2)){
            action = ActionType_AT2;
            return action;
        }
        FCPoint firstP ={(int)x2, (int)(y2 - (y2 - y1) * 0.382f)};
        FCPoint secondP ={(int)x2, (int)(y2 - (y2 - y1) * 0.5f)};
        FCPoint thirdP ={(int)x2, (int)(y2 - (y2 - y1) * 0.618f)};
        FCPoint startP ={(int)x1, (int)y1};
        bool selected = selectSegment(mp, x1, y1, x2, y2);
        if (selected){
            action = ActionType_MOVE;
            return action;
        }
        if ((x2 > x1 && mp.x >= x1 - 2) || (mp.x <= x1 + 2 && x2 < x1)){
            if (selectRay(mp, (float)startP.x, (float)startP.y, (float)firstP.x, (float)firstP.y)
                || selectRay(mp, (float)startP.x, (float)startP.y, (float)secondP.x ,(float)secondP.y)
                || selectRay(mp, (float)startP.x, (float)startP.y, (float)thirdP.x, (float)thirdP.y)){
                action = ActionType_MOVE;
            }
        }
        return action;
    }
    
    bool FiboFanline::onCreate(const FCPoint& mp){
        return create2PointsB(mp);
    }
    
    void FiboFanline::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_MOVE){
                setCursor(FCCursors_Hand);
            }
            else{
                setCursor(FCCursors_SizeNS);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
        }
    }
    
    void FiboFanline::onPaintGhost(FCPaint *paint){
        if (m_moveTimes > 1){
            onPaint(paint, &m_startMarks, m_selectedColor);
        }
    }
    
    void FiboFanline::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        float y1 = pY(pList->get(0)->Value);
        float y2 = pY(pList->get(1)->Value);
        int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(pList->get(1)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        drawLine(paint, lineColor, m_lineWidth, 2, x1, y1, x2, y2);
        if (m_selected || (x1 == x2)){
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x2, y2);
        }
        if (x1 != x2 && y1 != y2){
            FCPoint firstP ={(int)x2, (int)(y2 - (y2 - y1) * 0.382f)};
            FCPoint secondP ={(int)x2, (int)(y2 - (y2 - y1) * 0.5f)};
            FCPoint thirdP ={(int)x2, (int)(y2 - (y2 - y1) * 0.618f)};
            FCPoint startP ={(int)x1, (int)y1};
            FCPoint listP[3];
            listP[0] = firstP;
            listP[1] = secondP;
            listP[2] = thirdP;
            for (int i = 0; i < 3; i++){
                float k = 0, b = 0;
                lineXY((float)startP.x, (float)startP.y, (float)listP[i].x, (float)listP[i].y, 0, 0, &k, &b);
                float newX = 0;
                float newY = 0;
                if (x2 > x1){
                    newY = k * getWorkingAreaWidth() + b;
                    newX = (float)getWorkingAreaWidth();
                }
                else{
                    newY = b;
                    newX = 0;
                }
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, (float)startP.x, (float)startP.y, newX, newY);
            }
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    ArrayList<int> FiboTimeZone::getFibonacciTimeZonesParam(HashMap<int,PlotMark*> *pList){
        ArrayList<int> fValueList;
        if (pList->size() == 0){
            return fValueList;
        }
        FCChart *chart = getChart();
        int lastVisibleIndex = chart->getLastVisibleIndex();
        int aIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        int fibonacciValue = 1;
        fValueList.add(aIndex);
        int pos = 1;
        while (aIndex + fibonacciValue <= lastVisibleIndex){
            fibonacciValue = FCScript::fibonacciValue(pos);
            fValueList.add(aIndex + fibonacciValue);
            pos++;
        }
        return fValueList;
    }
    
    FiboTimeZone::FiboTimeZone(){
        m_plotType = L"FIBOTIMEZONE";
    }
    
    ActionType FiboTimeZone::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        FCChart *chart = getChart();
        ArrayList<int> param = getFibonacciTimeZonesParam(&m_marks);
        int psize = (int)param.size();
        if(psize > 0){
            for (int i = 0; i < psize; i++){
                int rI = (int)param.get(i);
                if (rI >= chart->getFirstVisibleIndex() && rI <= chart->getLastVisibleIndex()){
                    float x1 = pX(rI);
                    if (mp.x >= x1 - m_lineWidth * 2.5f && mp.x <= x1 + m_lineWidth * 2.5f){
                        action = ActionType_MOVE;
                        return action;
                    }
                }
            }
        }
        return action;
    }
    
    bool FiboTimeZone::onCreate(const FCPoint& mp){
        return createPoint(mp);
    }
    
    void FiboTimeZone::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            setCursor(FCCursors_Hand);
            m_startMarks.put(0, m_marks.get(0)->copy());
        }
    }
    
    void FiboTimeZone::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        FCChart *chart = getChart();
        ArrayList<int> param = getFibonacciTimeZonesParam(pList);
        int psize = (int)param.size();
        if(psize > 0){
            for (int i = 0; i < psize; i++){
                int rI = (int)param.get(i);
                if (rI >= chart->getFirstVisibleIndex() && rI <= chart->getLastVisibleIndex()){
                    float x1 = pX(rI);
                    drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, (float)0, x1, (float)getWorkingAreaHeight());
                    if (i == 0 && m_selected){
                        drawSelect(paint, lineColor, (float)x1, (float)getWorkingAreaHeight() / 2);
                    }
                    int fValue = FCScript::fibonacciValue(i);
                    wchar_t strValue[100] ={0};
                    swprintf(strValue, 99, L"%d", fValue);
                    FCPlot::drawText(paint, strValue, lineColor, m_font, (int)x1, 0);
                }
            }
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    GannBox::GannBox(){
        m_plotType = L"GANNBOX";
        m_oppositePoint.x = 0;
        m_oppositePoint.y = 0;
    }
    
    ActionType GannBox::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        action = getClickStatus();
        return action;
    }
    
    ActionType GannBox::getClickStatus(){
        FCPoint mp = getTouchOverPoint();
        FCRect rect = getRectangle(m_marks.get(0), m_marks.get(1));
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
            FCRect bigRect ={(int)(rect.left - sub), (int)(rect.top - sub), (int)(rect.right + sub), (int)(rect.bottom + sub)};
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
                x1 = rect.left;
                y1 = rect.bottom;
                x2 = rect.right;
                y2 = rect.top;
                FCPoint startP ={x1, y1};
                FCPoint *listP = getGannBoxPoints((float)x1, (float)y1, (float)x2, (float)y2);
                bool selected;
                int i;
                for (i = 0; i < 8; i++){
                    selected = selectLine(mp, (float)startP.x, (float)startP.y, (float)listP[i].x, (float)listP[i].y);
                    if (selected){
                        delete[] listP;
                        listP = 0;
                        return ActionType_MOVE;
                    }
                }
                delete[] listP;
                listP = 0;
                selected = selectLine(mp, (float)startP.x, (float)startP.y, (float)x2, (float)y2);
                if (selected) return ActionType_MOVE;
                x1 = rect.left;
                y1 = rect.top;
                x2 = rect.right;
                y2 = rect.bottom;
                listP = getGannBoxPoints((float)x1, (float)y1, (float)x2, (float)y2);
                for (i = 0; i < 8; i++){
                    selected = selectLine(mp, (float)startP.x, (float)startP.y, (float)listP[i].x, (float)listP[i].y);
                    if (selected){
                        delete[] listP;
                        listP = 0;
                        return ActionType_MOVE;
                    }
                }
                delete[] listP;
                listP = 0;
                startP.x = x1;
                startP.y = y1;
                selected = selectLine(mp, (float)startP.x, (float)startP.y, (float)x2, (float)y2);
                if (selected) return ActionType_MOVE;
            }
        }
        return ActionType_NO;
    }
    
    FCPoint* GannBox::getGannBoxPoints(float x1, float y1, float x2, float y2){
        FCPoint firstP ={(int)x2, (int)(y2 - (y2 - y1) * 0.875f)};
        FCPoint secondP ={(int)x2, (int)(y2 - (y2 - y1) * 0.75f)};
        FCPoint thirdP ={(int)x2, (int)(y2 - (y2 - y1) * 0.67f)};
        FCPoint forthP ={(int)x2, (int)(y2 - (y2 - y1) * 0.5f)};
        FCPoint fifthP ={(int)(x2 - (x2 - x1) * 0.875f), (int)y2};
        FCPoint sixthP ={(int)(x2 - (x2 - x1) * 0.75f), (int)y2};
        FCPoint seventhP ={(int)(x2 - (x2 - x1) * 0.67f), (int)y2};
        FCPoint eighthP ={(int)(x2 - (x2 - x1) * 0.5f), (int)y2};
        FCPoint *list = new FCPoint[8];
        list[0] = firstP;
        list[1] = secondP;
        list[2] = thirdP;
        list[3] = forthP;
        list[4] = fifthP;
        list[5] = sixthP;
        list[6] = seventhP;
        list[7] = eighthP;
        return list;
    }
    
    bool GannBox::onCreate(const FCPoint& mp){
        return create2PointsB(mp);
    }
    
    void GannBox::onMoveStart(){
        m_moveTimes++;
        m_action = getAction();
        FCRect rect = getRectangle(m_marks.get(0), m_marks.get(1));
        int x1 = rect.left;
        int y1 = rect.top;
        int x2 = rect.right;
        int y2 = rect.top;
        int x3 = rect.left;
        int y3 = rect.bottom;
        int x4 = rect.right;
        int y4 = rect.bottom;
        switch (m_action){
            case ActionType_AT1:
                m_oppositePoint.x = x4;
                m_oppositePoint.y = y4;
                break;
            case ActionType_AT2:
                m_oppositePoint.x = x3;
                m_oppositePoint.y = y3;
                break;
            case ActionType_AT3:
                m_oppositePoint.x = x2;
                m_oppositePoint.y = y2;
                break;
            case ActionType_AT4:
                m_oppositePoint.x = x1;
                m_oppositePoint.y = y1;
                break;
        }
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_MOVE){
                setCursor(FCCursors_Hand);
            }
            else{
                setCursor(FCCursors_SizeNS);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
        }
    }
    
    void GannBox::onMoving(){
        FCPoint mp = getMovingPoint();
        switch(m_action){
            case ActionType_MOVE:
                move(mp);
                break;
            case ActionType_AT1:
            case ActionType_AT2:
            case ActionType_AT3:
            case ActionType_AT4:
                resize(mp, m_oppositePoint);
                break;
        }
    }
    
    void GannBox::onPaintGhost(FCPaint *paint){
        if (m_moveTimes > 1){
            onPaint(paint, &m_startMarks, m_selectedColor);
        }
    }
    
    void GannBox::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        FCRect rect = getRectangle(pList->get(0), pList->get(1));
        int leftVScaleWidth = getChart()->getLeftVScaleWidth();
        int titleHeight = getDiv()->getTitleBar()->getHeight();
        FCRect clipRect ={rect.left + leftVScaleWidth, rect.top + titleHeight, rect.right + leftVScaleWidth, rect.bottom + titleHeight};
        paint->setClip(clipRect);
        if (rect.right - rect.left >= 0 && rect.bottom - rect.top >= 0){
            drawRect(paint, lineColor, m_lineWidth, m_lineStyle, rect.left, rect.top, rect.right, rect.bottom);
        }
        FCPoint oP ={rect.left, rect.top};
        int x1 = rect.left;
        int y1 = rect.bottom;
        int x2 = rect.right;
        int y2 = rect.top;
        if (x1 != x2 && y1 != y2){
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
            FCPoint startP ={x1, y1};
            FCPoint *listP = getGannBoxPoints((float)x1, (float)y1, (float)x2, (float)y2);
            int i;
            for (i = 0; i < 8; i++){
                float k = 0, b = 0;
                lineXY((float)startP.x, (float)startP.y, (float)listP[i].x, (float)listP[i].y, 0, 0, &k, &b);
                float newX = 0;
                float newY = 0;
                if (x2 > x1){
                    newY = k * x2 + b;
                    newX = (float)x2;
                }
                else{
                    newY = b;
                    newX = (float)x1;
                }
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, (int)(startP.x), (int)(startP.y), (int)(newX), (int)(newY));
            }
            delete[] listP;
            listP = 0;
            x1 = rect.left;
            y1 = rect.top;
            x2 = rect.right;
            y2 = rect.bottom;
            listP = getGannBoxPoints((float)x1, (float)y1, (float)x2, (float)y2);
            startP.x = x1;
            startP.y = y1;
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
            for (i = 0; i < 8; i++){
                float k = 0, b = 0;
                lineXY((float)startP.x, (float)startP.y, (float)listP[i].x, (float)listP[i].y, 0, 0, &k, &b);
                float newX = 0;
                float newY = 0;
                if (x2 > x1){
                    newY = k * x2 + b;
                    newX = (float)x2;
                }
                else{
                    newY = b;
                    newX = (float)x1;
                }
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, (int)(startP.x), (int)(startP.y), (int)(newX), (int)(newY));
            }
            delete[] listP;
            listP = 0;
        }
        if (m_selected){
            drawSelect(paint, lineColor, rect.left, rect.top);
            drawSelect(paint, lineColor, rect.right, rect.top);
            drawSelect(paint, lineColor, rect.left, rect.bottom);
            drawSelect(paint, lineColor, rect.right, rect.bottom);
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    
    GannLine::GannLine(){
        m_plotType = L"GANNLINE";
    }
    
    ActionType GannLine::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        m_moveTimes++;
        float y1 = pY(m_marks.get(0)->Value);
        float y2 = pY(m_marks.get(1)->Value);
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        if (selectPoint(mp, x1, y1)){
            action = ActionType_AT1;
            return action;
        }
        else if (selectPoint(mp, x2, y2)){
            action = ActionType_AT2;
            return action;
        }
        bool selected = selectSegment(mp, x1, y1, x2, y2);
        if (selected){
            action = ActionType_MOVE;
            return action;
        }
        FCPoint firstP ={(int)x2, (int)(y2 - (y2 - y1) * 0.875f)};
        FCPoint secondP ={(int)x2, (int)(y2 - (y2 - y1) * 0.75f)};
        FCPoint thirdP ={(int)x2, (int)(y2 - (y2 - y1) * 0.67f)};
        FCPoint forthP ={(int)x2, (int)(y2 - (y2 - y1) * 0.5f)};
        FCPoint fifthP ={(int)(x2 - (x2 - x1) * 0.875f), (int)y2};
        FCPoint sixthP ={(int)(x2 - (x2 - x1) * 0.75f), (int)y2};
        FCPoint seventhP ={(int)(x2 - (x2 - x1) * 0.67f), (int)y2};
        FCPoint eighthP ={(int)(x2 - (x2 - x1) * 0.5f), (int)y2};
        FCPoint startP ={(int)x1, (int)y1};
        FCPoint listP[8];
        listP[0] = firstP;
        listP[1] = secondP;
        listP[2] = thirdP;
        listP[3] = forthP;
        listP[4] = fifthP;
        listP[5] = sixthP;
        listP[6] = seventhP;
        listP[7] = eighthP;
        if ((x2 > x1 && mp.x >= x1 - 2) || (mp.x <= x1 + 2 && x2 < x1)){
            for (int i = 0; i < 8; i++){
                selected = selectLine(mp, (float)startP.x, (float)startP.y, (float)listP[i].x, (float)listP[i].y);
                if (selected){
                    action = ActionType_MOVE;
                    return action;
                }
            }
        }
        return action;
    }
    
    bool GannLine::onCreate(const FCPoint& mp){
        return create2PointsB(mp);
    }
    
    void GannLine::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_AT1){
                setCursor(FCCursors_SizeNS);
            }
            else if (m_action == ActionType_AT2){
                setCursor(FCCursors_SizeNS);
            }
            else{
                setCursor(FCCursors_Hand);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
        }
    }
    
    void GannLine::onPaintGhost(FCPaint *paint){
        if (m_moveTimes > 1){
            onPaint(paint, &m_startMarks, m_selectedColor);
        }
    }
    
    void GannLine::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        float y1 = pY(pList->get(0)->Value);
        float y2 = pY(pList->get(1)->Value);
        int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(pList->get(1)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        drawLine(paint, lineColor, m_lineWidth, 2, x1, y1, x2, y2);
        if (m_selected || (x1 == x2)){
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x2, y2);
        }
        if (x1 != x2 && y1 != y2){
            FCPoint firstP ={(int)x2, (int)(y2 - (y2 - y1) * 0.875f)};
            FCPoint secondP ={(int)x2, (int)(y2 - (y2 - y1) * 0.75f)};
            FCPoint thirdP ={(int)x2, (int)(y2 - (y2 - y1) * 0.67f)};
            FCPoint forthP ={(int)x2, (int)(y2 - (y2 - y1) * 0.5f)};
            FCPoint fifthP ={(int)(x2 - (x2 - x1) * 0.875f), (int)y2};
            FCPoint sixthP ={(int)(x2 - (x2 - x1) * 0.75f), (int)y2};
            FCPoint seventhP ={(int)(x2 - (x2 - x1) * 0.67f), (int)y2};
            FCPoint eighthP ={(int)(x2 - (x2 - x1) * 0.5f), (int)y2};
            FCPoint startP ={(int)x1, (int)y1};
            FCPoint listP[8];
            listP[0] = firstP;
            listP[1] = secondP;
            listP[2] = thirdP;
            listP[3] = forthP;
            listP[4] = fifthP;
            listP[5] = sixthP;
            listP[6] = seventhP;
            listP[7] = eighthP;
            for (int i = 0; i < 8; i++){
                float k = 0, b = 0;
                lineXY((float)startP.x, (float)startP.y, (float)listP[i].x, (float)listP[i].y, 0, 0, &k, &b);
                float newX = 0;
                float newY = 0;
                if (x2 > x1){
                    newY = k * getWorkingAreaWidth() + b;
                    newX = (float)getWorkingAreaWidth();
                }
                else{
                    newY = b;
                    newX = 0;
                }
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, (float)startP.x, (float)startP.y, newX, newY);
            }
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    GoldenRatio::GoldenRatio(){
        m_plotType = L"GOLDENRATIO";
    }
    
    ActionType GoldenRatio::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        m_moveTimes++;
        FCPoint mp = getTouchOverPoint();
        float y1 = pY(m_marks.get(0)->Value);
        float y2 = pY(m_marks.get(1)->Value);
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        float x1 = pX(bIndex);
        if (m_moveTimes == 1){
            action = ActionType_AT1;
            return action;
        }
        if (mp.x >= x1 - m_lineWidth * 2.5f && mp.x <= x1 +  m_lineWidth * 2.5f){
            if (mp.y >= y1 -  m_lineWidth * 2.5f && mp.y <= y1 +  m_lineWidth * 2.5f){
                action = ActionType_AT1;
                return action;
            }
            else if (mp.y >= y2 -  m_lineWidth * 2.5f && mp.y <= y2 +  m_lineWidth * 2.5f){
                action = ActionType_AT1;
                return action;
            }
        }
        if (hLinesSelect(goldenRatioParams(m_marks.get(0)->Value, m_marks.get(1)->Value),6)){
            action = ActionType_MOVE;
        }
        return action;
    }
    
    bool GoldenRatio::onCreate(const FCPoint& mp){
        return create2PointsB(mp);
    }
    
    void GoldenRatio::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_MOVE){
                setCursor(FCCursors_Hand);
            }
            else{
                setCursor(FCCursors_SizeWE);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
        }
    }
    
    void GoldenRatio::onMoving(){
        FCPoint mp = getMovingPoint();
        switch (m_action){
            case ActionType_AT1:
                resize(0);
                break;
            case ActionType_AT2:
                resize(1);
                break;
            case ActionType_MOVE:
                double subY = mp.y - m_startPoint.y;
                double maxValue = m_div->getVScale(m_attachVScale)->getVisibleMax();
                double minValue = m_div->getVScale(m_attachVScale)->getVisibleMin();
                double yAddValue = subY / getWorkingAreaHeight() * (minValue - maxValue);
                clearMarks(&m_marks);
                m_marks.put(0, new PlotMark(0, m_startMarks.get(0)->Key, m_startMarks.get(0)->Value + yAddValue));
                m_marks.put(1, new PlotMark(1, m_startMarks.get(1)->Key, m_startMarks.get(1)->Value + yAddValue));
                break;
        }
    }
    
    void GoldenRatio::onPaintGhost(FCPaint *paint){
        if (m_moveTimes > 1){
            onPaint(paint, &m_startMarks, m_selectedColor);
        }
    }
    
    void GoldenRatio::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        float y1 = pY(pList->get(0)->Value);
        float y2 = pY(pList->get(1)->Value);
        int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        float x1 = pX(bIndex);
        float *lineParam = goldenRatioParams(pList->get(0)->Value, pList->get(1)->Value);
        wchar_t *str[6] ={L"0.00%", L"23.60%", L"38.20%", L"50.00%", L"61.80%", L"100.00%" };
        for (int i = 0; i < 6; i++){
            FCSize sizeF = textSize(paint, str[i], m_font);
            float yP = lineParam[i];
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, (float)0, yP, (float)getWorkingAreaWidth(), yP);
            FCPlot::drawText(paint, str[i], lineColor, m_font, (int)(getWorkingAreaWidth() - sizeF.cx), (int)(yP - sizeF.cy));
        }
        delete[] lineParam;
        lineParam = 0;
        if (m_selected){
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x1, y2);
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    HLine::HLine(){
        m_plotType = L"HLINE";
    }
    
    ActionType HLine::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        float y1 = pY(m_marks.get(0)->Value);
        if (mp.y >= y1 - m_lineWidth * 2.5f && mp.y <= y1 + m_lineWidth * 2.5f){
            action = ActionType_MOVE;
        }
        return action;
    }
    
    bool HLine::onCreate(const FCPoint& mp){
        int rIndex = m_dataSource->rowsCount();
        if (rIndex > 0){
            m_marks.clear();
            double y = getNumberValue(mp);
            m_marks.put(0, new PlotMark(0, 0, y));
            return true;
        }
        return false;
    }
    
    void HLine::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            setCursor(FCCursors_Hand);
            m_startMarks.put(0, m_marks.get(0)->copy());
        }
    }
    
    void HLine::onMoving(){
        FCPoint mp = getMovingPoint();
        clearMarks(&m_marks);
        m_marks.put(0, new PlotMark(0, 0, getNumberValue(mp)));
    }
    
    void HLine::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        float y1 = pY(pList->get(0)->Value);
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, (float)0, y1, (float)getWorkingAreaWidth(), y1);
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    float* LevelGrading::levelGradingParams(double value1, double value2){
        float y1 = pY(value1);
        float y2 = pY(value2);
        float yA = 0, yB = 0, yC = 0, yD = 0, yE = 0;
        yA = y1;
        yB = y2;
        yC = y1 <= y2 ? y2 + (y2 - y1) * 0.382f : y2 - (y1 - y2) * 0.382f;
        yD = y1 <= y2 ? y2 + (y2 - y1) * 0.618f : y2 - (y1 - y2) * 0.618f;
        yE = y1 <= y2 ? y2 + (y2 - y1) : y2 - (y1 - y2);
        float *list = new float[5];
        list[0] = yA;
        list[1] = yB;
        list[2] = yC;
        list[3] = yD;
        list[4] = yE;
        return list;
    }
    
    LevelGrading::LevelGrading(){
        m_plotType = L"LEVELGRADING";
    }
    
    ActionType LevelGrading::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        float y1 = pY(m_marks.get(0)->Value);
        float y2 = pY(m_marks.get(1)->Value);
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        float x1 = pX(bIndex);
        if (m_moveTimes == 1){
            action = ActionType_AT1;
            return action;
        }
        if (mp.x >= x1 - m_lineWidth * 2.5f && mp.x <= x1 + m_lineWidth * 2.5f){
            if (mp.y >= y1 - m_lineWidth * 2.5f && mp.y <= y1 + m_lineWidth * 2.5f){
                action = ActionType_AT1;
                return action;
            }
            else if (mp.y >= y2 - m_lineWidth * 2.5f && mp.y <= y2 + m_lineWidth * 2.5f){
                action = ActionType_AT2;
                return action;
            }
        }
        if (hLinesSelect(levelGradingParams(m_marks.get(0)->Value, m_marks.get(1)->Value),5)){
            action = ActionType_MOVE;
        }
        return action;
    }
    
    bool LevelGrading::onCreate(const FCPoint& mp){
        return create2PointsB(mp);
    }
    
    void LevelGrading::onMoveStart(){
        m_moveTimes++;
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_MOVE){
                setCursor(FCCursors_Hand);
            }
            else{
                setCursor(FCCursors_SizeWE);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
        }
    }
    
    void LevelGrading::onMoving(){
        FCPoint mp = getMovingPoint();
        switch (m_action){
            case ActionType_AT1:
                resize(0);
                break;
            case ActionType_AT2:
                resize(1);
                break;
            case ActionType_MOVE:
                double subY = mp.y - m_startPoint.y;
                double maxValue = m_div->getVScale(m_attachVScale)->getVisibleMax();
                double minValue = m_div->getVScale(m_attachVScale)->getVisibleMin();
                double yAddValue = subY / getWorkingAreaHeight() * (minValue - maxValue);
                clearMarks(&m_marks);
                m_marks.put(0, new PlotMark(0, m_startMarks.get(0)->Key, m_startMarks.get(0)->Value + yAddValue));
                m_marks.put(1, new PlotMark(1, m_startMarks.get(1)->Key, m_startMarks.get(1)->Value + yAddValue));
                break;
        }
    }
    
    void LevelGrading::onPaintGhost(FCPaint *paint){
        if (m_moveTimes > 1){
            onPaint(paint, &m_startMarks, m_selectedColor);
        }
    }
    
    void LevelGrading::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        float y1 = pY(pList->get(0)->Value);
        float y2 = pY(pList->get(1)->Value);
        int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        float x1 = pX(bIndex);
        float *lineParam = levelGradingParams(pList->get(0)->Value, pList->get(1)->Value);
        wchar_t *strs[5] ={L"-100%", L"0.00%", L"38.20%",L"61.80%", L"100%"};
        wchar_t *strs2[5] ={L"100%", L"0.00%", L"-38.20%", L"-61.80%", L"-100%"};
        for (int i = 0; i < 5; i++){
            wchar_t *str = y1 >= y2 ? strs[i] : strs2[i];
            FCSize sizeF = textSize(paint, str, m_font);
            float yP = lineParam[i];
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, (float)0, yP, (float)getWorkingAreaWidth(), yP);
            FCPlot::drawText(paint, str, lineColor, m_font, (int)(getWorkingAreaWidth() - sizeF.cx), (int)(yP - sizeF.cy));
        }
        delete[] lineParam;
        lineParam = 0;
        if (m_selected){
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x1, y2);
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    Line::Line(){
        m_plotType = L"LINE";
    }
    
    ActionType Line::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
        float y1 = pY(m_marks.get(0)->Value);
        float y2 = pY(m_marks.get(1)->Value);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        if (selectPoint(mp, x1, y1)){
            action = ActionType_AT1;
        }
        else if (selectPoint(mp, x2, y2)){
            action = ActionType_AT2;
        }
        else if (selectLine(mp, x1, y1, x2, y2)){
            action = ActionType_MOVE;
        }
        return action;
    }
    
    bool Line::onCreate(const FCPoint& mp){
        return create2PointsA(mp);
    }
    
    void Line::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_MOVE){
                setCursor(FCCursors_Hand);
            }
            else{
                setCursor(FCCursors_SizeWE);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
        }
    }
    
    void Line::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        float y1 = pY(pList->get(0)->Value);
        float y2 = pY(pList->get(1)->Value);
        int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(pList->get(1)->Key);
        float *param = getLineParams(pList->get(0), pList->get(1));
        float a = 0;
        float b = 0;
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        if (param){
            a = param[0];
            b = param[1];
            float leftX = 0;
            float leftY = leftX * a + b;
            float rightX = (float)getWorkingAreaWidth();
            float rightY = rightX * a + b;
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, leftX, leftY, rightX , rightY);
            delete[] param;
            param = 0;
        }
        else{
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, (float)0, x1, (float)getWorkingAreaHeight());
        }
        if (m_selected){
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x2, y2);
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    LrBand::LrBand(){
        m_plotType = L"LRBAND";
    }
    
    ActionType LrBand::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        FCChart *chart = getChart();
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float *param = getLRParams(&m_marks);
        if (param){
            float a = param[0];
            float b = param[1];
            double leftValue = a + b;
            double rightValue = (eIndex - bIndex + 1) * a + b;
            float y1 = pY(leftValue);
            float y2 = pY(rightValue);
            if (selectPoint(mp, x1, y1)){
                action = ActionType_AT1;
                delete[] param;
                param = 0;
                return action;
            }
            else if (selectPoint(mp, x2, y2)){
                action = ActionType_AT2;
                delete[] param;
                param = 0;
                return action;
            }
            int touchIndex = chart->getTouchOverIndex();
            if (touchIndex >= bIndex && touchIndex <= eIndex){
                double yValue = a * ((touchIndex - bIndex) + 1) + b;
                float y = pY(yValue);
                float x = pX(touchIndex);
                if (mp.x >= x - 5 && mp.x <= x + 5 && mp.y >= y - 5 && mp.y <= y + 5){
                    action = ActionType_MOVE;
                    delete[] param;
                    param = 0;
                    return action;
                }
                double *parallel = getLRBandRange(&m_marks, param);
                delete[] param;
                param = 0;
                yValue = a * ((touchIndex - bIndex) + 1) + b + parallel[0];
                y = pY(yValue);
                x = pX(touchIndex);
                if (mp.x >= x - 5 && mp.x <= x + 5 && mp.y >= y - 5 && mp.y <= y + 5){
                    action = ActionType_MOVE;
                    delete[] parallel;
                    parallel = 0;
                    return action;
                }
                yValue = a * ((touchIndex - bIndex) + 1) + b - parallel[1];
                y = pY(yValue);
                x = pX(touchIndex);
                if (mp.x >= x - 5 && mp.x <= x + 5 && mp.y >= y - 5 && mp.y <= y + 5){
                    action = ActionType_MOVE;
                    delete[] parallel;
                    parallel = 0;
                    return action;
                }
                delete[] parallel;
                parallel = 0;
            }
            else{
                delete[] param;
                param = 0;
            }
        }
        return action;
    }
    
    bool LrBand::onCreate(const FCPoint& mp){
        return create2CandlePoints(mp);
    }
    
    void LrBand::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_MOVE){
                setCursor(FCCursors_Hand);
            }
            else{
                setCursor(FCCursors_SizeWE);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
        }
    }
    
    void LrBand::onMoving(){
        FCPoint mp = getMovingPoint();
        FCChart *chart = getChart();
        int touchIndex = chart->getTouchOverIndex();
        if (touchIndex < 0){
            touchIndex = 0;
        }
        if (touchIndex > chart->getLastVisibleIndex()){
            touchIndex = chart->getLastVisibleIndex();
        }
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
        switch (m_action){
            case ActionType_MOVE:
                move(mp);
                break;
            case ActionType_AT1:
                if (touchIndex < eIndex){
                    resize(0);
                }
                break;
            case ActionType_AT2:
                if (touchIndex > bIndex){
                    resize(1);
                }
                break;
        }
    }
    
    void LrBand::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        float *param = getLRParams(pList);
        if (param){
            int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
            int eIndex = m_dataSource->getRowIndex(pList->get(1)->Key);
            float x1 = pX(bIndex);
            float x2 = pX(eIndex);
            float a = param[0];
            float b = param[1];
            double leftValue = a + b;
            double rightValue = (eIndex - bIndex + 1) * a + b;
            float y1 = pY(leftValue);
            float y2 = pY(rightValue);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
            double *parallel = getLRBandRange(pList,param);
            double leftTop = leftValue + parallel[0];
            double rightTop = rightValue + parallel[0];
            double leftBottom = leftValue - parallel[1];
            double rightBottom = rightValue - parallel[1];
            delete[] param;
            param = 0;
            delete[] parallel;
            parallel = 0;
            float leftTopY = pY(leftTop);
            float rightTopY = pY(rightTop);
            float leftBottomY = pY(leftBottom);
            float rightBottomY = pY(rightBottom);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, leftTopY, x2, rightTopY);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, leftBottomY, x2, rightBottomY);
            if (m_selected){
                drawSelect(paint, lineColor, x1, y1);
                drawSelect(paint, lineColor, x2, y2);
            }
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    LrChannel::LrChannel(){
        m_plotType = L"LRCHANNEL";
    }
    
    ActionType LrChannel::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        FCChart *chart = getChart();
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float *param = getLRParams(&m_marks);
        if (param){
            float a = param[0];
            float b = param[1];
            double leftValue = a + b;
            double rightValue = (eIndex - bIndex + 1) * a + b;
            float y1 = pY(leftValue);
            float y2 = pY(rightValue);
            if (selectPoint(mp, x1, y1)){
                action = ActionType_AT1;
                delete[] param;
                param = 0;
                return action;
            }
            else if (selectPoint(mp, x2, y2)){
                action = ActionType_AT2;
                delete[] param;
                param = 0;
                return action;
            }
            int touchIndex = chart->getTouchOverIndex();
            if (touchIndex >= bIndex && touchIndex <= chart->getLastVisibleIndex()){
                double yValue = a * ((touchIndex - bIndex) + 1) + b;
                float y = pY(yValue);
                float x = pX(touchIndex);
                if (selectPoint(mp, x, y)){
                    action = ActionType_MOVE;
                    return action;
                }
                double *parallel = getLRBandRange(&m_marks, param);
                delete[] param;
                param = 0;
                yValue = a * ((touchIndex - bIndex) + 1) + b + parallel[0];
                y = pY(yValue);
                x = pX(touchIndex);
                if (selectPoint(mp, x, y)){
                    action = ActionType_MOVE;
                    delete[] parallel;
                    parallel = 0;
                    return action;
                }
                yValue = a * ((touchIndex - bIndex) + 1) + b - parallel[1];
                y = pY(yValue);
                x = pX(touchIndex);
                if (selectPoint(mp, x, y)){
                    action = ActionType_MOVE;
                    delete[] parallel;
                    parallel = 0;
                    return action;
                }
                delete[] parallel;
                parallel = 0;
            }
            else{
                delete[] param;
                param = 0;
            }
        }
        return action;
    }
    
    bool LrChannel::onCreate(const FCPoint& mp){
        return create2CandlePoints(mp);
    }
    
    void LrChannel::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_MOVE){
                setCursor(FCCursors_Hand);
            }
            else{
                setCursor(FCCursors_SizeWE);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
        }
    }
    
    void LrChannel::onMoving(){
        FCPoint mp = getMovingPoint();
        FCChart *chart = getChart();
        int touchIndex = chart->getTouchOverIndex();
        if (touchIndex < 0){
            touchIndex = 0;
        }
        if (touchIndex > chart->getLastVisibleIndex()){
            touchIndex = chart->getLastVisibleIndex();
        }
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
        switch (m_action){
            case ActionType_MOVE:
                move(mp);
                break;
            case ActionType_AT1:
                if (touchIndex < eIndex){
                    resize(0);
                }
                break;
            case ActionType_AT2:
                if (touchIndex > bIndex){
                    resize(1);
                }
                break;
        }
    }
    
    void LrChannel::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        float *param = getLRParams(pList);
        if (param){
            FCChart *chart = getChart();
            int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
            int eIndex = m_dataSource->getRowIndex(pList->get(1)->Key);
            float x1 = pX(bIndex);
            float x2 = pX(eIndex);
            float a = param[0];
            float b = param[1];
            double leftValue = a + b;
            double rightValue = (eIndex - bIndex + 1) * a + b;
            float y1 = pY(leftValue);
            float y2 = pY(rightValue);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
            double *parallel = getLRBandRange(pList, param);
            delete[] param;
            param = 0;
            double leftTop = leftValue + parallel[0];
            double rightTop = rightValue + parallel[0];
            double leftBottom = leftValue - parallel[1];
            double rightBottom = rightValue - parallel[1];
            float leftTopY = pY(leftTop);
            float rightTopY = pY(rightTop);
            float leftBottomY = pY(leftBottom);
            float rightBottomY = pY(rightBottom);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, leftTopY, x2, rightTopY);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, leftBottomY, x2, rightBottomY);
            rightValue = (chart->getLastVisibleIndex() + 1 - bIndex) * a + b;
            float x3 = (float)((chart->getLastVisibleIndex() - chart->getFirstVisibleIndex() + 1) * chart->getHScalePixel() + chart->getHScalePixel() / 2);
            double dashTop = rightValue + parallel[0];
            double dashBottom = rightValue - parallel[1];
            delete[] parallel;
            parallel = 0;
            float mValueY = pY(rightValue);
            float dashTopY = pY(dashTop);
            float dashBottomY = pY(dashBottom);
            drawLine(paint, lineColor, m_lineWidth, 2, x2, rightTopY, x3, dashTopY);
            drawLine(paint, lineColor, m_lineWidth, 2, x2, rightBottomY, x3, dashBottomY);
            drawLine(paint, lineColor, m_lineWidth, 2, x2, y2, x3, mValueY);
            if (m_selected){
                drawSelect(paint, lineColor, x1, y1);
                drawSelect(paint, lineColor, x2, y2);
            }
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    LrLine::LrLine(){
        m_plotType = L"LRLINE";
    }
    
    ActionType LrLine::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        FCChart *chart = getChart();
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float *param = getLRParams(&m_marks);
        if (param){
            float a = param[0];
            float b = param[1];
            delete[] param;
            param = 0;
            double leftValue = a + b;
            double rightValue = (eIndex - bIndex + 1) * a + b;
            float y1 = pY(leftValue);
            float y2 = pY(rightValue);
            if (selectPoint(mp, x1, y1)){
                action = ActionType_AT1;
                return action;
            }
            else if (selectPoint(mp, x2, y2)){
                action = ActionType_AT2;
                return action;
            }
            int touchIndex = chart->getTouchOverIndex();
            if (touchIndex >= bIndex && touchIndex <= eIndex){
                double yValue = a * ((touchIndex-bIndex) + 1) + b;
                float y = pY(yValue);
                float x = pX(touchIndex);
                if (selectPoint(mp, x, y)){
                    action = ActionType_MOVE;
                    return action;
                }
            }
        }
        return action;
    }
    
    bool LrLine::onCreate(const FCPoint& mp){
        return create2CandlePoints(mp);
    }
    
    void LrLine::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_MOVE){
                setCursor(FCCursors_Hand);
            }
            else{
                setCursor(FCCursors_SizeWE);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
        }
    }
    
    void LrLine::onMoving(){
        FCPoint mp = getMovingPoint();
        FCChart *chart = getChart();
        int touchIndex = chart->getTouchOverIndex();
        if (touchIndex < 0){
            touchIndex = 0;
        }
        if (touchIndex > chart->getLastVisibleIndex()){
            touchIndex = chart->getLastVisibleIndex();
        }
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
        switch (m_action){
            case ActionType_MOVE:
                move(mp);
                break;
            case ActionType_AT1:
                if (touchIndex < eIndex){
                    resize(0);
                }
                break;
            case ActionType_AT2:
                if (touchIndex > bIndex){
                    resize(1);
                }
                break;
        }
    }
    
    void LrLine::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        float *param = getLRParams(pList);
        if (param){
            int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
            int eIndex = m_dataSource->getRowIndex(pList->get(1)->Key);
            float x1 = pX(bIndex);
            float x2 = pX(eIndex);
            float a = param[0];
            float b = param[1];
            delete[] param;
            param = 0;
            double leftValue = a + b;
            double rightValue = (eIndex - bIndex + 1) * a + b;
            float y1 = pY(leftValue);
            float y2 = pY(rightValue);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
            if (m_selected){
                drawSelect(paint, lineColor, x1, y1);
                drawSelect(paint, lineColor, x2, y2);
            }
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    void NullPoint::nullPoint(float x1, float y1, float x2, float y2, float *nullX, float *nullY){
        float k1 = 0, k2 = 0, b1 = 0, b2 = 0;
        double a = 45, b = 60;
        if(y1>=y2){
            k1 = -(float)tan(a);
            k2 = -(float)tan(b);
            b1 = y1 - k1 * x1;
            b2 = y2 - k2 * x2;
        }
        else{
            k1 = (float)tan(a);
            k2 = (float)tan(b);
            b1 = y1 - k1 * x1;
            b2 = y2 - k2 * x2;
        }
        *nullX = (b2 - b1) / (k1 - k2);
        *nullY = *nullX * k1 + b1;
    }
    
    double* NullPoint::getNullPointParams(HashMap<int,PlotMark*> *pList){
        if (pList->size() == 0){
            return 0;
        }
        int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(pList->get(1)->Key);
        if(!m_sourceFields.containsKey(L"CLOSE") ){
            return 0;
        }
        double leftClose = 0, rightClose = 0;
        if (eIndex >= bIndex){
            leftClose = m_dataSource->get2(bIndex, m_sourceFields.get(L"CLOSE"));
            rightClose = m_dataSource->get2(eIndex, m_sourceFields.get(L"CLOSE"));
        }
        else{
            leftClose = m_dataSource->get2(eIndex, m_sourceFields.get(L"CLOSE"));
            rightClose = m_dataSource->get2(bIndex, m_sourceFields.get(L"CLOSE"));
        }
        double *list = new double[2];
        list[0] = leftClose;
        list[1] = rightClose;
        return list;
    }
    
    NullPoint::NullPoint(){
        m_plotType = L"NULLFCPoint";
    }
    
    ActionType NullPoint::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
        double *closeParam = getNullPointParams(&m_marks);
        double leftClose = closeParam[1];
        double rightClose = closeParam[0];
        float y1 = pY(leftClose);
        float y2 = pY(rightClose);
        float x1 = pX(bIndex >= eIndex ? bIndex : eIndex);
        float x2 = pX(bIndex >= eIndex ? eIndex : bIndex);
        float *param = getLineParams(m_marks.get(0), m_marks.get(1));
        if (param){
            delete[] param;
            param = 0;
            if (mp.x >= x1 - m_lineWidth * 2.5f && mp.x <= x1 + m_lineWidth * 2.5f){
                action = ActionType_AT1;
                return action;
            }
            else if (mp.x >= x2 - m_lineWidth * 2.5f && mp.x <= x2 + m_lineWidth * 2.5f){
                action = ActionType_AT2;
                return action;
            }
        }
        else{
            if (mp.y >= y1 - m_lineWidth * 5 && mp.y <= y1 + m_lineWidth * 5){
                action = ActionType_AT1;
                return action;
            }
            else if (mp.y >= y2 - m_lineWidth * 5 && mp.y <= y2 + m_lineWidth * 5){
                action = ActionType_AT2;
                return action;
            }
        }
        float x3 = 0, y3 = 0;
        if (y1 != y2){
            nullPoint(x1, y1, x2, y2, &x3, &y3);
        }
        if(selectTriangle(mp, x1, y1, x2, y2, x3, y3)){
            action = ActionType_MOVE;
            return action;
        }
        return action;
    }
    
    bool NullPoint::onCreate(const FCPoint& mp){
        return create2CandlePoints(mp);
    }
    
    void NullPoint::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_MOVE){
                setCursor(FCCursors_Hand);
            }
            else{
                setCursor(FCCursors_SizeWE);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
        }
    }
    
    void NullPoint::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        int aIndex = m_dataSource->getRowIndex(pList->get(1)->Key);
        double *closeParam = getNullPointParams(pList);
        double leftClose = closeParam[1];
        double rightClose = closeParam[0];
        delete[] closeParam;
        closeParam = 0;
        float y1 = pY(leftClose);
        float y2 = pY(rightClose);
        float x1 = pX(bIndex >= aIndex ? bIndex : aIndex);
        float x2 = pX(bIndex >= aIndex ? aIndex : bIndex);
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
        if (y1 != y2){
            float nullX = 0, nullY = 0;
            nullPoint(x1, y1, x2, y2, &nullX, &nullY);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, nullX, nullY);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x2, y2, nullX, nullY);
        }
        if (m_selected){
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x2, y2);
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    float* Parallel::getParallelParams(HashMap<int,PlotMark*> *pList){
        if (pList->size() == 0){
            return 0;
        }
        float y1 = pY(pList->get(0)->Value);
        float y2 = pY(pList->get(1)->Value);
        float y3 = pY(pList->get(2)->Value);
        int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(pList->get(1)->Key);
        int pIndex = m_dataSource->getRowIndex(pList->get(2)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float x3 = pX(pIndex);
        float a = 0;
        if (x2 - x1 != 0){
            a = (y2 - y1) / (x2 - x1);
            float b = y1 - a * x1;
            float c = y3 - a * x3;
            float *list = new float[3];
            list[0] = a;
            list[1] = b;
            list[2] = c;
            return list;
        }
        else{
            return 0;
        }
    }
    
    Parallel::Parallel(){
        m_plotType = L"PARALLEL";
    }
    
    ActionType Parallel::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        float y1 = pY(m_marks.get(0)->Value);
        float y2 = pY(m_marks.get(1)->Value);
        float y3 = pY(m_marks.get(2)->Value);
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
        int pIndex = m_dataSource->getRowIndex(m_marks.get(2)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float x3 = pX(pIndex);
        float *param = getParallelParams(&m_marks);
        if (param){
            if (selectPoint(mp, x1, y1)){
                action = ActionType_AT1;
                delete[] param;
                param = 0;
                return action;
            }
            else if (selectPoint(mp, x2, y2)){
                action = ActionType_AT2;
                delete[] param;
                param = 0;
                return action;
            }
            else if (selectPoint(mp, x3, y3)){
                action = ActionType_AT3;
                delete[] param;
                param = 0;
                return action;
            }
            if (mp.y - param[0] * mp.x - param[1] >= m_lineWidth * -5 && mp.y - param[0] * mp.x - param[1] <= m_lineWidth * 5){
                action = ActionType_MOVE;
                delete[] param;
                param = 0;
                return action;
            }
            if (mp.y - param[0] * mp.x - param[2] >= m_lineWidth * -5 && mp.y - param[0] * mp.x - param[2] <= m_lineWidth * 5){
                action = ActionType_MOVE;
                delete[] param;
                param = 0;
                return action;
            }
            delete[] param;
            param = 0;
        }
        else{
            if (mp.y >= y1 - m_lineWidth * 5 && mp.y <= y1 + m_lineWidth * 5){
                action = ActionType_AT1;
                return action;
            }
            else if (mp.y >= y2 - m_lineWidth * 5 && mp.y <= y2 + m_lineWidth * 5){
                action = ActionType_AT2;
                return action;
            }
            else if (mp.y >= y3 - m_lineWidth * 5 && mp.y <= y3 + m_lineWidth * 5){
                action = ActionType_AT3;
                return action;
            }
            if (mp.x >= x1 - m_lineWidth * 5 && mp.x <= x1 + m_lineWidth * 5){
                action = ActionType_MOVE;
                return action;
            }
            if (mp.x >= x3 - m_lineWidth * 5 && mp.x <= x3 + m_lineWidth * 5){
                action = ActionType_MOVE;
                return action;
            }
        }
        return action;
    }
    
    bool Parallel::onCreate(const FCPoint& mp){
        return create3Points(mp);
    }
    
    void Parallel::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_MOVE){
                setCursor(FCCursors_Hand);
            }
            else{
                setCursor(FCCursors_SizeWE);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
            m_startMarks.put(2, m_marks.get(2)->copy());
        }
    }
    
    void Parallel::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        float y1 = pY(pList->get(0)->Value);
        float y2 = pY(pList->get(1)->Value);
        float y3 = pY(pList->get(2)->Value);
        int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(pList->get(1)->Key);
        int pIndex = m_dataSource->getRowIndex(pList->get(2)->Key);
        float *param = getParallelParams(pList);
        float a = 0;
        float b = 0;
        float c = 0;
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float x3 = pX(pIndex);
        if (param){
            a = param[0];
            b = param[1];
            c = param[2];
            delete[] param;
            param = 0;
            float leftX = 0;
            float leftY = leftX * a + b;
            float rightX = (float)getWorkingAreaWidth();
            float rightY = rightX * a + b;
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, leftX, leftY, rightX, rightY);
            leftY = leftX * a + c;
            rightY = rightX * a + c;
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, leftX, leftY, rightX, rightY);
        }
        else{
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, (float)0, x1, (float)getWorkingAreaHeight());
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x3, (float)0, x3, (float)getWorkingAreaHeight());
        }
        if (m_selected){
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x2, y2);
            drawSelect(paint, lineColor, x3, y3);
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    float* Percent::getPercentParams(double value1, double value2){
        float y1 = pY(value1);
        float y2 = pY(value2);
        float y0 = 0, y25 = 0, y50 = 0, y75 = 0, y100 = 0;
        y0 = y1;
        y25 = y1 <= y2 ? y1 + (y2 - y1) / 4 : y2 + (y1 - y2) * 3 / 4;
        y50 = y1 <= y2 ? y1 + (y2 - y1) / 2 : y2 + (y1 - y2) / 2;
        y75 = y1 <= y2 ? y1 + (y2 - y1) * 3 / 4 : y2 + (y1 - y2) / 4;
        y100 = y2;
        float *list = new float[5];
        list[0] = y0;
        list[1] = y25;
        list[2] = y50;
        list[3] = y75;
        list[4] = y100;
        return list;
    }
    
    Percent::Percent(){
        m_plotType = L"PERCENT";
    }
    
    ActionType Percent::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        m_moveTimes++;
        FCPoint mp = getTouchOverPoint();
        float y1 = pY(m_marks.get(0)->Value);
        float y2 = pY(m_marks.get(1)->Value);
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        float x1 = pX(bIndex);
        if (m_moveTimes == 1){
            action = ActionType_AT1;
            return action;
        }
        if (mp.x >= x1 - m_lineWidth * 2.5f && mp.x <= x1 + m_lineWidth * 2.5f){
            if (mp.y >= y1 - m_lineWidth * 2.5f && mp.y <= y1 + m_lineWidth * 2.5f){
                action = ActionType_AT1;
                return action;
            }
            else if (mp.y >= y2 - m_lineWidth * 2.5f && mp.y <= y2 + m_lineWidth * 2.5f){
                action = ActionType_AT2;
                return action;
            }
        }
        float *param = getPercentParams(m_marks.get(0)->Value, m_marks.get(1)->Value);
        if (hLinesSelect(param, 5)){
            action = ActionType_MOVE;
        }
        delete[] param;
        param = 0;
        return action;
    }
    
    
    bool Percent::onCreate(const FCPoint& mp){
        return create2PointsB(mp);
    }
    
    void Percent::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_MOVE){
                setCursor(FCCursors_Hand);
            }
            else{
                setCursor(FCCursors_SizeWE);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
        }
    }
    
    void Percent::onMoving(){
        FCPoint mp = getMovingPoint();
        switch (m_action){
            case ActionType_AT1:
                resize(0);
                break;
            case ActionType_AT2:
                resize(1);
                break;
            case ActionType_MOVE:
                double subY = mp.y - m_startPoint.y;
                double maxValue = m_div->getVScale(m_attachVScale)->getVisibleMax();
                double minValue = m_div->getVScale(m_attachVScale)->getVisibleMin();
                double yAddValue = subY / getWorkingAreaHeight() * (minValue - maxValue);
                clearMarks(&m_marks);
                m_marks.put(0, new PlotMark(0, m_startMarks.get(0)->Key, m_startMarks.get(0)->Value + yAddValue));
                m_marks.put(1, new PlotMark(1, m_startMarks.get(1)->Key, m_startMarks.get(1)->Value + yAddValue));
                break;
        }
    }
    
    void Percent::onPaintGhost(FCPaint *paint){
        if (m_moveTimes > 1){
            onPaint(paint, &m_startMarks, m_selectedColor);
        }
    }
    
    void Percent::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        float y1 = pY(pList->get(0)->Value);
        float y2 = pY(pList->get(1)->Value);
        int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        float x1 = pX(bIndex);
        float *lineParam = getPercentParams(pList->get(0)->Value, pList->get(1)->Value);
        wchar_t *str[5] ={L"0.00%", L"25.00%", L"50.00%", L"75.00%", L"100.00%"};
        for (int i = 0; i < 5; i++){
            FCSize sizeF = textSize(paint, str[i], m_font);
            float yP = lineParam[i];
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, (float)0, yP, (float)getWorkingAreaWidth(), yP);
            FCPlot::drawText(paint, str[i], lineColor, m_font, (int)(getWorkingAreaWidth() - sizeF.cx), (int)(yP - sizeF.cy));
        }
        delete[] lineParam;
        lineParam = 0;
        if (m_selected){
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x1, y2);
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    Periodic::Periodic(){
        m_plotType = L"PERIODIC";
        m_period = 5;
        m_beginPeriod = 1;
    }
    
    ActionType Periodic::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        ArrayList<double> param = getPLParams(&m_marks);
        int psize = (int)param.size();
        if(psize > 0){
            float y = (float)getWorkingAreaHeight() / 2;
            for (int i = 0; i < psize; i++){
                int rI = (int)param.get(i);
                float x1 = pX(rI);
                if (selectPoint(mp, x1, y)){
                    action = ActionType_AT1;
                    clearMarks(&m_marks);
                    m_marks.put(0, new PlotMark(0, m_dataSource->getXValue(rI), 0));
                    m_beginPeriod = m_period;
                    return action;
                }
                if (mp.x >= x1 - m_lineWidth * 2.5f && mp.x <= x1 + m_lineWidth * 2.5f){
                    action = ActionType_MOVE;
                    return action;
                }
            }
        }
        return action;
    }
    
    ArrayList<double> Periodic::getPLParams(HashMap<int,PlotMark*> *pList){
        ArrayList<double> list;
        if(pList->size()==0){
            return list;
        }
        FCChart *chart = getChart();
        int aIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        int fIndex = chart->getFirstVisibleIndex();
        int lIndex = chart->getLastVisibleIndex();
        for (int i = fIndex; i < lIndex; i++){
            int value = (i - aIndex>=0) ? (i - aIndex):(aIndex - i);
            if (value % m_period == 0){
                list.add(i);
            }
        }
        return list;
    }
    
    bool Periodic::onCreate(const FCPoint& mp){
        FCChart *chart = getChart();
        int rIndex = m_dataSource->rowsCount();
        if (rIndex > 0){
            int currentIndex = getIndex(mp);
            double y = getNumberValue(mp);
            double date = m_dataSource->getXValue(currentIndex);
            clearMarks(&m_marks);
            m_marks.put(0, new PlotMark(0, date, y));
            m_period = chart->getMaxVisibleRecord() / 10;
            if (m_period < 1){
                m_period = 1;
            }
            return true;
        }
        return false;
    }
    
    void Periodic::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_AT1){
                setCursor(FCCursors_SizeNS);
            }
            else{
                setCursor(FCCursors_Hand);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
        }
    }
    
    void Periodic::onMoving(){
        FCPoint mp = getMovingPoint();
        FCChart *chart = getChart();
        int touchIndex = chart->getTouchOverIndex();
        if (touchIndex < 0){
            touchIndex = 0;
        }
        if (touchIndex > chart->getLastVisibleIndex()){
            touchIndex = chart->getLastVisibleIndex();
        }
        int bI = getIndex(m_startPoint);
        int eI = getIndex(mp);
        switch (m_action){
            case ActionType_MOVE:
                move(mp);
                break;
            case ActionType_AT1:
                m_period = m_beginPeriod + (eI - bI);
                if (m_period < 1){
                    m_period = 1;
                }
                break;
        }
    }
    
    void Periodic::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        ArrayList<double> param = getPLParams(pList);
        int psize = (int)param.size();
        for (int i = 0; i < psize; i++){
            int rI = (int)param.get(i);
            float x1 = pX(rI);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, (float)0, x1, (float)getWorkingAreaHeight());
            if (m_selected){
                drawSelect(paint, lineColor, (int)x1, getWorkingAreaHeight() / 2);
            }
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    Price::Price(){
        m_plotType = L"PRICE";
    }
    
    ActionType Price::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        double fValue = m_marks.get(0)->Value;
        int aIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        float x1 = pX(aIndex);
        float y1 = pY(fValue);
        FCRect rect ={(int)x1, (int)y1, (int)x1 + m_textSize.cx, (int)y1 + m_textSize.cy};
        if (mp.x >= rect.left && mp.x <= rect.right && mp.y >= rect.top && mp.y <= rect.bottom){
            action = ActionType_MOVE;
        }
        return action;
    }
    
    bool Price::onCreate(const FCPoint& mp){
        return createPoint(mp);
    }
    
    void Price::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            setCursor(FCCursors_Hand);
            m_startMarks.put(0, m_marks.get(0)->copy());
        }
    }
    
    void Price::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        FCChart *chart = getChart();
        int wX = getWorkingAreaWidth();
        int wY = getWorkingAreaHeight();
        if (wX > 0 && wY > 0){
            double fValue = pList->get(0)->Value;
            int aIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
            float x1 = pX(aIndex);
            float y1 = pY(fValue);
            wchar_t word[100] ={0};
            FCStr::getValueByDigit(fValue, chart->getLeftVScaleWidth() > 0 ? m_div->getLeftVScale()->getDigit() : m_div->getRightVScale()->getDigit(), word);
            FCPlot::drawText(paint, word, lineColor, m_font, (int)x1, (int)y1);
            m_textSize = textSize(paint, word, m_font);
            if (m_selected){
                if (m_textSize.cx > 0 && m_textSize.cy > 0){
                    drawRect(paint, lineColor, m_lineWidth, m_lineStyle, (int)x1, (int)y1, (int)x1 + m_textSize.cx, (int)y1 + m_textSize.cy);
                }
            }
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    RangeRuler::RangeRuler(){
        m_plotType = L"RANGERULER";
    }
    
    ActionType RangeRuler::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        double *param = getCandleRange(&m_marks);
        double nHigh = param[0];
        double nLow = param[1];
        delete[] param;
        param = 0;
        float highY = pY(nHigh);
        float lowY = pY(nLow);
        float smallX = x1 > x2 ? x2 : x1;
        float bigX = x1 > x2 ? x1 : x2;
        if ((mp.y >= highY - m_lineWidth * 2.5f && mp.y <= highY + m_lineWidth * 2.5f)
            || (mp.y >= lowY - m_lineWidth * 2.5f && mp.y <= lowY + m_lineWidth * 2.5f)){
            if (mp.x >= x1 - m_lineWidth * 2.5f && mp.x <= x1 + m_lineWidth * 2.5f){
                action = ActionType_AT1;
                return action;
            }
            else if (mp.x >= x2 - m_lineWidth * 2.5f && mp.x <= x2 + m_lineWidth * 2.5f){
                action = ActionType_AT2;
                return action;
            }
        }
        if (mp.y >= highY - m_lineWidth * 2.5f && mp.y <= highY + m_lineWidth * 2.5f){
            if (mp.x >= smallX - m_lineWidth * 2.5f && mp.x <= bigX + m_lineWidth * 2.5f){
                action = ActionType_MOVE;
                return action;
            }
        }
        else if (mp.y >= lowY - m_lineWidth * 2.5f && mp.y <= lowY + m_lineWidth * 2.5f){
            if (mp.x >= smallX - m_lineWidth * 2.5f && mp.x <= bigX + m_lineWidth * 2.5f){
                action = ActionType_MOVE;
                return action;
            }
        }
        float mid = x1 >= x2 ? (x2 + (x1 - x2) / 2) : (x1 + (x2 - x1) / 2);
        if (mp.x >= mid - m_lineWidth * 2.5f && mp.x <= mid + m_lineWidth * 2.5f){
            if (mp.y >= highY - m_lineWidth * 2.5f && mp.y <= lowY + m_lineWidth * 2.5f){
                action = ActionType_MOVE;
                return action;
            }
        }
        return action;
    }
    
    bool RangeRuler::onCreate(const FCPoint& mp){
        return create2CandlePoints(mp);
    }
    
    void RangeRuler::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_MOVE){
                setCursor(FCCursors_Hand);
            }
            else{
                setCursor(FCCursors_SizeNS);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
        }
    }
    
    void RangeRuler::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(pList->get(1)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float smallX = x1 > x2 ? x2 : x1;
        float bigX = x1 > x2 ? x1 : x2;
        double *param = getCandleRange(pList);
        double nHigh = param[0];
        double nLow = param[1];
        delete[] param;
        param = 0;
        float highY = pY(nHigh);
        float lowY = pY(nLow);
        float mid = x1 >= x2 ? (x2 + (x1 - x2) / 2) : (x1 + (x2 - x1) / 2);
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, highY, x2, highY);
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, lowY, x2, lowY);
        drawLine(paint, lineColor, m_lineWidth, 2, mid, lowY, mid, highY);
        if (nHigh != nLow){
            double diff = abs(nLow - nHigh);
            double range = 0;
            if(nHigh != 0){
                range = diff / nHigh;
            }
            wchar_t diffString[100] ={0};
            FCStr::getValueByDigit(diff, m_div->getVScale(m_attachVScale)->getDigit(), diffString);
            wchar_t rangeString[100] ={0};
            swprintf(rangeString, 99, L"%.2f", range);
            wchar_t rangeString2[100] ={0};
            FCStr::contact(rangeString2, rangeString, L"%", L"");
            FCSize diffSize = textSize(paint, diffString, m_font);
            FCSize rangeSize = textSize(paint, rangeString2, m_font);
            FCPlot::drawText(paint, diffString, lineColor, m_font, (int)(bigX - diffSize.cx), (int)(highY + 2));
            FCPlot::drawText(paint, rangeString2, lineColor, m_font, (int)(bigX - rangeSize.cx), (int)(lowY - rangeSize.cy));
        }
        if (m_selected){
            drawSelect(paint, lineColor, smallX, highY);
            drawSelect(paint, lineColor, smallX, lowY);
            drawSelect(paint, lineColor, bigX, highY);
            drawSelect(paint, lineColor, bigX, lowY);
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    RaseLine::RaseLine(){
        m_plotType = L"RASELINE";
    }
    
    ActionType RaseLine::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        float *param = getRaseLineParams(&m_marks);
        if (param){
            if (mp.y - param[0] * mp.x - param[1] >= m_lineWidth * -5 && mp.y - param[0] * mp.x - param[1] <= m_lineWidth * 5){
                action = ActionType_MOVE;
            }
            delete[] param;
            param = 0;
        }
        return action;
    }
    
    float* RaseLine::getRaseLineParams(HashMap<int,PlotMark*> *pList){
        if (pList->size() == 0){
            return 0;
        }
        float y1 = pY(pList->get(0)->Value);
        int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        float x1 = pX(bIndex);
        float a = -1;
        float b = y1 + x1;
        float *param = new float[2];
        param[0] = a;
        param[1] = b;
        return param;
    }
    
    bool RaseLine::onCreate(const FCPoint& mp){
        return createPoint(mp);
    }
    
    void RaseLine::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            setCursor(FCCursors_Hand);
            m_startMarks.put(0, m_marks.get(0)->copy());
        }
    }
    
    void RaseLine::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        float *param = getRaseLineParams(pList);
        if(param){
            float a = param[0];
            float b = param[1];
            delete[] param;
            param = 0;
            float leftX = 0;
            float leftY = leftX * a + b;
            float rightX = (float)getWorkingAreaWidth();
            float rightY = rightX * a + b;
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, leftX, leftY , rightX, rightY);
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    Ray::Ray(){
        m_plotType = L"RAY";
    }
    
    ActionType Ray::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float y1 = pY(m_marks.get(0)->Value);
        float y2 = pY(m_marks.get(1)->Value);
        float *param = getLineParams(m_marks.get(0), m_marks.get(1));
        if (param){
            delete[] param;
            param = 0;
            if (mp.x >= x1 - m_lineWidth * 2.5f && mp.x <= x1 + m_lineWidth * 2.5f){
                action = ActionType_AT1;
                return action;
            }
            else if (mp.x >= x2 - m_lineWidth * 2.5f && mp.x <= x2 + m_lineWidth * 2.5f){
                action = ActionType_AT2;
                return action;
            }
        }
        else{
            if (mp.y >= y1 - m_lineWidth * 5 && mp.y <= y1 + m_lineWidth * 5){
                action = ActionType_AT1;
                return action;
            }
            else if (mp.y >= y2 - m_lineWidth * 5 && mp.y <= y2 + m_lineWidth * 5){
                action = ActionType_AT2;
                return action;
            }
        }
        if (selectRay(mp, x1, y1, x2, y2)){
            action = ActionType_MOVE;
        }
        return action;
    }
    
    bool Ray::onCreate(const FCPoint& mp){
        return create2PointsA(mp);
    }
    
    void Ray::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_MOVE){
                setCursor(FCCursors_Hand);
            }
            else{
                setCursor(FCCursors_SizeWE);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
        }
    }
    
    void Ray::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        float y1 = pY(pList->get(0)->Value);
        float y2 = pY(pList->get(1)->Value);
        int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(pList->get(1)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float k = 0, b = 0;
        lineXY(x1, y1, x2, y2, 0, 0, &k, &b);
        drawRay(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2, k, b);
        if (m_selected){
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x2, y2);
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    RectLine::RectLine(){
        m_plotType = L"FCRect";
    }
    
    ActionType RectLine::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        action = selectRect(mp, m_marks.get(0), m_marks.get(1));
        return action;
    }
    
    bool RectLine::onCreate(const FCPoint& mp){
        return create2PointsB(mp);
    }
    
    void RectLine::onMoveStart(){
        m_moveTimes++;
        m_action = getAction();
        if (m_action != ActionType_MOVE && m_action != ActionType_NO){
            FCRect rect = getRectangle(m_marks.get(0), m_marks.get(1));
            int x1 = rect.left;
            int y1 = rect.top;
            int x2 = rect.right;
            int y2 = rect.top;
            int x3 = rect.left;
            int y3 = rect.bottom;
            int x4 = rect.right;
            int y4 = rect.bottom;
            switch (m_action){
                case ActionType_AT1:
                    m_oppositePoint.x = x4;
                    m_oppositePoint.y = y4;
                    break;
                case ActionType_AT2:
                    m_oppositePoint.x = x3;
                    m_oppositePoint.y = y3;
                    break;
                case ActionType_AT3:
                    m_oppositePoint.x = x2;
                    m_oppositePoint.y = y2;
                    break;
                case ActionType_AT4:
                    m_oppositePoint.x = x1;
                    m_oppositePoint.y = y1;
                    break;
            }
        }
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_MOVE){
                setCursor(FCCursors_Hand);
            }
            else{
                setCursor(FCCursors_SizeNS);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
        }
    }
    
    void RectLine::onMoving(){
        FCPoint mp = getMovingPoint();
        switch (m_action){
            case ActionType_MOVE:
                move(mp);
                break;
            case ActionType_AT1:
            case ActionType_AT2:
            case ActionType_AT3:
            case ActionType_AT4:
                resize(mp, m_oppositePoint);
                break;
        }
    }
    
    void RectLine::onPaintGhost(FCPaint *paint){
        if (m_moveTimes > 1){
            onPaint(paint, &m_startMarks, m_selectedColor);
        }
    }
    
    void RectLine::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        FCRect rect = getRectangle(pList->get(0), pList->get(1));
        if (rect.right - rect.left >= 0 && rect.bottom - rect.top >= 0){
            drawRect(paint, lineColor, m_lineWidth, m_lineStyle,  rect.left, rect.top, rect.right, rect.bottom);
        }
        if (m_selected){
            drawSelect(paint, lineColor, rect.left, rect.top);
            drawSelect(paint, lineColor, rect.right, rect.top);
            drawSelect(paint, lineColor, rect.left, rect.bottom);
            drawSelect(paint, lineColor, rect.right, rect.bottom);
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    Segment::Segment(){
        m_plotType = L"SEGMENT";
    }
    
    ActionType Segment::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        float y1 = pY(m_marks.get(0)->Value);
        float y2 = pY(m_marks.get(1)->Value);
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float *param = getLineParams(m_marks.get(0), m_marks.get(1));
        if (param){
            delete[] param;
            param = 0;
            if (mp.x >= x1 - m_lineWidth * 2.5f && mp.x <= x1 + m_lineWidth * 2.5f){
                action = ActionType_AT1;
                return action;
            }
            else if (mp.x >= x2 - m_lineWidth * 2.5f && mp.x <= x2 + m_lineWidth * 2.5f){
                action = ActionType_AT2;
                return action;
            }
        }
        else{
            if (mp.y >= y1 - m_lineWidth * 5 && mp.y <= y1 + m_lineWidth * 5){
                action = ActionType_AT1;
                return action;
            }
            else if (mp.y >= y2 - m_lineWidth * 5 && mp.y <= y2 + m_lineWidth * 5){
                action = ActionType_AT2;
                return action;
            }
        }
        if (selectSegment(mp, x1, y1, x2, y2)){
            action = ActionType_MOVE;
        }
        return action;
    }
    
    bool Segment::onCreate(const FCPoint& mp){
        return create2PointsB(mp);
    }
    
    void Segment::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_MOVE){
                setCursor(FCCursors_Hand);
            }
            else{
                setCursor(FCCursors_SizeWE);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
        }
    }
    
    void Segment::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        float y1 = pY(pList->get(0)->Value);
        float y2 = pY(pList->get(1)->Value);
        int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(pList->get(1)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
        if (m_selected){
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x2, y2);
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    Sine::Sine(){
        m_plotType = L"SINE";
    }
    
    ActionType Sine::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        float y1 = pY(m_marks.get(0)->Value);
        float y2 = pY(m_marks.get(1)->Value);
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        if (selectPoint(mp, x1, y1)){
            action = ActionType_AT1;
            return action;
        }
        else if (selectPoint(mp, x2, y2)){
            action = ActionType_AT2;
            return action;
        }
        if (selectSine(mp, x1, y1, x2, y2)){
            action = ActionType_MOVE;
        }
        return action;
    }
    
    bool Sine::onCreate(const FCPoint& mp){
        FCChart *chart = getChart();
        int rIndex = m_dataSource->rowsCount();
        if (rIndex > 0){
            int touchIndex = chart->getTouchOverIndex();
            if (touchIndex >= 0 && touchIndex <= chart->getLastVisibleIndex()){
                int eIndex = touchIndex;
                int bIndex = eIndex - chart->getMaxVisibleRecord() / 10;
                if (bIndex >= 0 && eIndex != bIndex){
                    double fDate = m_dataSource->getXValue(bIndex);
                    double sDate = m_dataSource->getXValue(eIndex);
                    m_marks.clear();
                    double y = getNumberValue(mp);
                    clearMarks(&m_marks);
                    m_marks.put(0, new PlotMark(0, fDate, y + (m_div->getVScale(m_attachVScale)->getVisibleMax() - m_div->getVScale(m_attachVScale)->getVisibleMin()) / 4));
                    m_marks.put(1, new PlotMark(1, sDate, y));
                    return true;
                }
            }
        }
        return false;
    }
    
    void Sine::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_AT1){
                setCursor(FCCursors_SizeNS);
            }
            else if (m_action == ActionType_AT2){
                setCursor(FCCursors_SizeWE);
            }
            else{
                setCursor(FCCursors_Hand);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
        }
    }
    
    void Sine::onMoving(){
        FCPoint mp = getMovingPoint();
        FCChart *chart = getChart();
        int touchIndex = chart->getTouchOverIndex();
        if (touchIndex < 0){
            touchIndex = 0;
        }
        if (touchIndex > chart->getLastVisibleIndex()){
            touchIndex = chart->getLastVisibleIndex();
        }
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
        switch (m_action){
            case ActionType_MOVE:
                move(mp);
                break;
            case ActionType_AT1:
                if (touchIndex < eIndex){
                    resize(0);
                }
                break;
            case ActionType_AT2:
                if (touchIndex > bIndex){
                    resize(1);
                }
                break;
        }
    }
    
    void Sine::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        double fValue = pList->get(0)->Value;
        double eValue = pList->get(1)->Value;
        int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(pList->get(1)->Key);
        int x1 = (int)pX(bIndex);
        float x2 = pX(eIndex);
        float y1 = pY(fValue);
        float y2 = pY(eValue);
        double f = 2.0 * 3.14159 / ((x2 - x1) * 4);
        if (x1 != x2){
            int len = getWorkingAreaWidth();
            if(len > 0){
                FCPoint *pf = new FCPoint[len];
                for(int i = 0; i < len; i++){
                    int x = -x1 + i;
                    float y = (float)(0.5 * (y2 - y1) * sin(x * f) * 2);
                    FCPoint pt ={(int)(x + x1), (int)(y + y1)};
                    pf[i] = pt;
                }
                drawPolyline(paint, lineColor, m_lineWidth, m_lineStyle, pf, len);
                delete[] pf;
                pf = 0;
            }
        }
        if (m_selected){
            drawSelect(paint, lineColor, (float)x1, y1);
            drawSelect(paint, lineColor, x2, y2);
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    SpeedResist::SpeedResist(){
        m_plotType = L"SPEEDRESIST";
    }
    
    ActionType SpeedResist::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        float y1 = pY(m_marks.get(0)->Value);
        float y2 = pY(m_marks.get(1)->Value);
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        if (selectPoint(mp,x1,y1) || m_moveTimes == 1){
            action = ActionType_AT1;
            return action;
        }
        else if (selectPoint(mp,x2,y2)){
            action = ActionType_AT2;
            return action;
        }
        FCPoint firstP ={(int)x2, (int)(y2 - (y2 - y1) / 3)};
        FCPoint secondP ={(int)x2, (int)(y2 - (y2 - y1) * 2 / 3)};
        FCPoint startP ={(int)x1, (int)y1};
        float oK = 0, oB = 0, fK = 0, fB = 0, sK = 0, sB = 0;
        lineXY(x1, y1, x2, y2, 0, 0, &oK, &oB);
        lineXY((float)startP.x, (float)startP.y, (float)firstP.x, (float)firstP.y, 0, 0, &fK, &fB);
        lineXY((float)startP.x, (float)startP.y, (float)secondP.x, (float)secondP.y, 0, 0, &sK, &sB);
        float smallX = x1 <= x2 ? x1 : x2;
        float smallY = y1 <= y2 ? y1 : y2;
        float bigX = x1 > x2 ? x1 : x2;
        float bigY = y1 > y2 ? y1 : y2;
        if (mp.x >= smallX - 2 && mp.x <= bigX + 2 && mp.y >= smallY - 2 && mp.y <= bigY + 2){
            if (!(oK == 0 && oB == 0)){
                if (mp.y / (mp.x * oK + oB) >= 0.9 && mp.y / (mp.x * oK + oB) <= 1.1){
                    action = ActionType_MOVE;
                    return action;
                }
            }
            else{
                action = ActionType_MOVE;
                return action;
            }
        }
        if ((x2 > x1 && mp.x >= x1 - 2) || (mp.x <= x1 + 2 && x2 < x1)){
            if (!(fK == 0 && fB == 0)){
                if (mp.y / (mp.x * fK + fB) >= 0.9 && mp.y / (mp.x * fK + fB) <= 1.1){
                    action = ActionType_MOVE;
                    return action;
                }
            }
            if (!(sK == 0 && sB == 0)){
                if (mp.y / (mp.x * sK + sB) >= 0.9 && mp.y / (mp.x * sK + sB) <= 1.1){
                    action = ActionType_MOVE;
                    return action;
                }
            }
        }
        return action;
    }
    
    bool SpeedResist::onCreate(const FCPoint& mp){
        return create2PointsA(mp);
    }
    
    void SpeedResist::onMoveStart(){
        m_moveTimes++;
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_MOVE){
                setCursor(FCCursors_Hand);
            }
            else{
                setCursor(FCCursors_SizeNS);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
        }
    }
    
    void SpeedResist::onPaintGhost(FCPaint *paint){
        if (m_moveTimes > 1){
            onPaint(paint, &m_startMarks, m_selectedColor);
        }
    }
    
    void SpeedResist::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        float y1 = pY(pList->get(0)->Value);
        float y2 = pY(pList->get(1)->Value);
        int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(pList->get(1)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        drawLine(paint, lineColor, m_lineWidth, 2, x1, y1, x2, y2);
        if (m_selected || (x1 == x2)){
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x2, y2);
        }
        if (x1 != x2 && y1 != y2){
            FCPoint firstP ={(int)x2, (int)(y2 - (y2 - y1) / 3)};
            FCPoint secondP ={(int)x2, (int)(y2 - (y2 - y1) * 2 / 3)};
            FCPoint startP ={(int)x1, (int)y1};
            float fK = 0, fB = 0, sK = 0, sB = 0;
            lineXY((float)startP.x, (float)startP.y, (float)firstP.x, (float)firstP.y, 0, 0, &fK, &fB);
            lineXY((float)startP.x, (float)startP.y, (float)secondP.x, (float)secondP.y, 0, 0, &sK, &sB);
            float newYF = 0, newYS = 0;
            float newX = 0;
            if (x2 > x1){
                newYF = fK * getWorkingAreaWidth() + fB;
                newYS = sK * getWorkingAreaWidth() + sB;
                newX = (float)getWorkingAreaWidth();
            }
            else{
                newYF = fB;
                newYS = sB;
                newX = 0;
            }
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, (float)startP.x, (float)startP.y, newX, newYF);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, (float)startP.x, (float)startP.y, newX, newYS);
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    double SeChannel::getSEChannelSD(HashMap<int,PlotMark*> *pList){
        if (m_sourceFields.containsKey(L"CLOSE")){
            int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
            int eIndex = m_dataSource->getRowIndex(pList->get(1)->Key);
            int len = eIndex - bIndex + 1;
            if(len > 0){
                double *ary = new double[len];
                for(int i = 0; i < len; i++){
                    double close = m_dataSource->get2(i + bIndex, m_sourceFields.get(L"CLOSE"));
                    if (!FCDataTable::isNaN(close)){
                        ary[i] = close;
                    }
                }
                double avg = FCScript::avgValue(ary, len);
                double sd = FCScript::standardDeviation(ary, len, avg, 2);
                delete[] ary;
                ary = 0;
                return sd;
            }
        }
        return 0;
    }
    
    SeChannel::SeChannel(){
        m_plotType = L"SECHANNEL";
    }
    
    ActionType SeChannel::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        FCChart *chart = getChart();
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float *param = getLRParams(&m_marks);
        if (param){
            float a = param[0];
            float b = param[1];
            delete[] param;
            param = 0;
            double leftValue = a + b;
            double rightValue = (eIndex - bIndex + 1) * a + b;
            float y1 = pY(leftValue);
            float y2 = pY(rightValue);
            if (selectPoint(mp, x1, y1)){
                action = ActionType_AT1;
                return action;
            }
            else if (selectPoint(mp, x2, y2)){
                action = ActionType_AT2;
                return action;
            }
            int touchIndex = chart->getTouchOverIndex();
            if (touchIndex >= bIndex && touchIndex <= chart->getLastVisibleIndex()){
                double yValue = a * ((touchIndex - bIndex) + 1) + b;
                float y = pY(yValue);
                float x = pX(touchIndex);
                if (selectPoint(mp, x, y)){
                    action = ActionType_MOVE;
                    return action;
                }
                double sd = getSEChannelSD(&m_marks);
                yValue = a * ((touchIndex - bIndex) + 1) + b + sd;
                y = pY(yValue);
                x = pX(touchIndex);
                if (selectPoint(mp, x, y)){
                    action = ActionType_MOVE;
                    return action;
                }
                yValue = a * ((touchIndex - bIndex) + 1) + b - sd;
                y = pY(yValue);
                x = pX(touchIndex);
                if (selectPoint(mp, x, y)){
                    action = ActionType_MOVE;
                    return action;
                }
            }
        }
        return action;
    }
    
    bool SeChannel::onCreate(const FCPoint& mp){
        return create2CandlePoints(mp);
    }
    
    void SeChannel::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_MOVE){
                setCursor(FCCursors_Hand);
            }
            else{
                setCursor(FCCursors_SizeWE);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
        }
    }
    
    void SeChannel::onMoving(){
        FCPoint mp = getMovingPoint();
        FCChart *chart = getChart();
        int touchIndex = chart->getTouchOverIndex();
        if (touchIndex < 0){
            touchIndex = 0;
        }
        if (touchIndex > chart->getLastVisibleIndex()){
            touchIndex = chart->getLastVisibleIndex();
        }
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
        switch (m_action){
            case ActionType_MOVE:
                move(mp);
                break;
            case ActionType_AT1:
                if (touchIndex < eIndex){
                    resize(0);
                }
                break;
            case ActionType_AT2:
                if (touchIndex > bIndex){
                    resize(1);
                }
                break;
        }
    }
    
    void SeChannel::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        FCChart *chart = getChart();
        int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(pList->get(1)->Key);
        float x1 = pX(chart->getX(bIndex));
        float x2 = pX(chart->getX(eIndex));
        float *param = getLRParams(pList);
        if (param){
            float a = param[0];
            float b = param[1];
            delete[] param;
            param = 0;
            double leftValue = a + b;
            double rightValue = (eIndex - bIndex + 1) * a + b;
            float y1 = pY(leftValue);
            float y2 = pY(rightValue);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
            double sd = getSEChannelSD(pList);
            double leftTop = leftValue + sd;
            double rightTop = rightValue + sd;
            double leftBottom = leftValue - sd;
            double rightBottom = rightValue - sd;
            float leftTopY = pY(leftTop);
            float rightTopY = pY(rightTop);
            float leftBottomY = pY(leftBottom);
            float rightBottomY = pY(rightBottom);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, leftTopY, x2, rightTopY);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, leftBottomY, x2, rightBottomY);
            rightValue = (chart->getLastVisibleIndex() + 1 - bIndex) * a + b;
            float x3 = (float)((chart->getLastVisibleIndex() - chart->getFirstVisibleIndex()) * chart->getHScalePixel() + chart->getHScalePixel() / 2);
            double dashTop = rightValue + sd;
            double dashBottom = rightValue - sd;
            float mValueY = pY(rightValue);
            float dashTopY = pY(dashTop);
            float dashBottomY = pY(dashBottom);
            drawLine(paint, lineColor, m_lineWidth, 2, x2, rightTopY, x3, dashTopY);
            drawLine(paint, lineColor, m_lineWidth, 2, x2, rightBottomY, x3, dashBottomY);
            drawLine(paint, lineColor, m_lineWidth, 2, x2, y2, x3, mValueY);
            if (m_selected){
                drawSelect(paint, lineColor, x1, y1);
                drawSelect(paint, lineColor, x2, y2);
            }
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    SymmetricLine::SymmetricLine(){
        m_plotType = L"SYMMETRICLINE";
    }
    
    ActionType SymmetricLine::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float y1 = pY(m_marks.get(0)->Value);
        float y2 = pY(m_marks.get(1)->Value);
        if (selectPoint(mp, x1, y1)){
            action = ActionType_AT1;
            return action;
        }
        else if (selectPoint(mp, x2, y2)){
            action = ActionType_AT2;
            return action;
        }
        int cIndex = 0;
        if (x2 >= x1){
            cIndex = bIndex - (eIndex - bIndex);
        }
        else{
            cIndex = bIndex + (bIndex - eIndex);
        }
        if (cIndex > m_dataSource->rowsCount() - 1){
            cIndex = m_dataSource->rowsCount() - 1;
        }
        else if (cIndex < 0){
            cIndex = 0;
        }
        float x3 = pX(cIndex);
        if ((mp.x >= x1 - m_lineWidth * 5 && mp.y <= x1 + m_lineWidth * 5)
            || (mp.x >= x2 - m_lineWidth * 5 && mp.y <= x2 + m_lineWidth * 5)
            || (mp.x >= x3 - m_lineWidth * 5 && mp.y <= x3 + m_lineWidth * 5)){
            action = ActionType_MOVE;
        }
        return action;
    }
    
    bool SymmetricLine::onCreate(const FCPoint& mp){
        return create2PointsA(mp);
    }
    
    void SymmetricLine::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_MOVE){
                setCursor(FCCursors_Hand);
            }
            else{
                setCursor(FCCursors_SizeWE);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
        }
    }
    
    void SymmetricLine::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(pList->get(1)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float y1 = pY(pList->get(0)->Value);
        float y2 = pY(pList->get(1)->Value);
        int cIndex = -1;
        if (x2 >= x1){
            cIndex = bIndex - (eIndex - bIndex);
        }
        else{
            cIndex = bIndex + (bIndex - eIndex);
        }
        if (cIndex > m_dataSource->rowsCount() - 1){
            cIndex = m_dataSource->rowsCount() - 1;
        }
        else if (cIndex < 0){
            cIndex = 0;
        }
        float x3 = pX(cIndex);
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, (float)0, x1, (float)getWorkingAreaHeight());
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x2, (float)0, x2, (float)getWorkingAreaHeight());
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x3, (float)0, x3, (float)getWorkingAreaHeight());
        if (m_selected){
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x2, y2);
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    float* SymmetricTriangle::getSymmetricTriangleParams(HashMap<int,PlotMark*> *pList){
        if (pList->size() == 0){
            return 0;
        }
        float y1 = pY(pList->get(0)->Value);
        float y2 = pY(pList->get(1)->Value);
        float y3 = pY(pList->get(2)->Value);
        int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(pList->get(1)->Key);
        int pIndex = m_dataSource->getRowIndex(pList->get(2)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float x3 = pX(pIndex);
        float a = 0;
        if (x2 - x1 != 0){
            a = (y2 - y1) / (x2 - x1);
            float b = y1 - a * x1;
            float c = -a;
            float d = y3 - c * x3;
            float *list = new float[4];
            list[0] = a;
            list[1] = b;
            list[2] = c;
            list[3] = d;
            return list;
        }
        else{
            return 0;
        }
    }
    
    SymmetricTriangle::SymmetricTriangle(){
        m_plotType = L"SYMMETRICTRIANGLE";
    }
    
    ActionType SymmetricTriangle::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        float y1 = pY(m_marks.get(0)->Value);
        float y2 = pY(m_marks.get(1)->Value);
        float y3 = pY(m_marks.get(2)->Value);
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
        int pIndex = m_dataSource->getRowIndex(m_marks.get(2)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float x3 = pX(pIndex);
        float *param = getSymmetricTriangleParams(&m_marks);
        if (param){
            if (selectPoint(mp, x1, y1)){
                action = ActionType_AT1;
                delete[] param;
                param = 0;
                return action;
            }
            else if (selectPoint(mp, x2, y2)){
                action = ActionType_AT2;
                delete[] param;
                param = 0;
                return action;
            }
            else if (selectPoint(mp, x3, y3)){
                action = ActionType_AT3;
                delete[] param;
                param = 0;
                return action;
            }
            if (mp.y - param[0] * mp.x - param[1] >= m_lineWidth * -5 && mp.y - param[0] * mp.x - param[1] <= m_lineWidth * 5){
                action = ActionType_MOVE;
                delete[] param;
                param = 0;
                return action;
            }
            if (mp.y - param[2] * mp.x - param[3] >= m_lineWidth * -5 && mp.y - param[2] * mp.x - param[3] <= m_lineWidth * 5){
                action = ActionType_MOVE;
                delete[] param;
                param = 0;
                return action;
            }
            delete[] param;
            param = 0;
        }
        else{
            if (mp.y >= y1 - m_lineWidth * 5 && mp.y <= y1 + m_lineWidth * 5){
                action = ActionType_AT1;
                return action;
            }
            else if (mp.y >= y2 - m_lineWidth * 5 && mp.y <= y2 + m_lineWidth * 5){
                action = ActionType_AT2;
                return action;
            }
            else if (mp.y >= y3 - m_lineWidth * 5 && mp.y <= y3 + m_lineWidth * 5){
                action = ActionType_AT3;
                return action;
            }
            if (mp.x >= x1 - m_lineWidth * 5 && mp.x <= x1 + m_lineWidth * 5){
                action = ActionType_MOVE;
                return action;
            }
            if (mp.x >= x3 - m_lineWidth * 5 && mp.x <= x3 + m_lineWidth * 5){
                action = ActionType_MOVE;
                return action;
            }
        }
        return action;
    }
    
    bool SymmetricTriangle::onCreate(const FCPoint& mp){
        return create3Points(mp);
    }
    
    void SymmetricTriangle::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_MOVE){
                setCursor(FCCursors_Hand);
            }
            else{
                setCursor(FCCursors_SizeWE);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
            m_startMarks.put(2, m_marks.get(2)->copy());
        }
    }
    
    void SymmetricTriangle::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        float y1 = pY(pList->get(0)->Value);
        float y2 = pY(pList->get(1)->Value);
        float y3 = pY(pList->get(2)->Value);
        int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(pList->get(1)->Key);
        int pIndex = m_dataSource->getRowIndex(pList->get(2)->Key);
        float *param = getSymmetricTriangleParams(pList);
        float a = 0;
        float b = 0;
        float c = 0;
        float d = 0;
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float x3 = pX(pIndex);
        if (param){
            a = param[0];
            b = param[1];
            c = param[2];
            d = param[3];
            delete[] param;
            param = 0;
            float leftX = 0;
            float leftY = leftX * a + b;
            float rightX = (float)getWorkingAreaWidth();
            float rightY = rightX * a + b;
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, leftX, leftY, rightX, rightY);
            leftY = leftX * c + d;
            rightY = rightX * c + d;
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, leftX, leftY, rightX, rightY );
        }
        else{
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, (float)0, x1, (float)getWorkingAreaHeight());
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x3, (float)0, x3, (float)getWorkingAreaHeight());
        }
        if (m_selected){
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x2, y2);
            drawSelect(paint, lineColor, x3, y3);
        }
    }
    
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    
    double* TimeRuler::getTimeRulerParams(HashMap<int,PlotMark*> *pList){
        if (pList->size() == 0){
            return 0;
        }
        FCChart *chart = getChart();
        int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(pList->get(1)->Key);
        double bHigh = chart->divMaxOrMin(bIndex, m_div, 0);
        double bLow = chart->divMaxOrMin(bIndex, m_div, 1);
        double eHigh = chart->divMaxOrMin(eIndex, m_div, 0);
        double eLow = chart->divMaxOrMin(eIndex, m_div, 1);
        double *list = new double[4];
        list[0] = bHigh;
        list[1] = bLow;
        list[2] = eHigh;
        list[3] = eLow;
        return list;
    }
    
    TimeRuler::TimeRuler(){
        m_plotType = L"TIMERULER";
    }
    
    ActionType TimeRuler::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        float y1 = pY(m_marks.get(0)->Value);
        float y2 = pY(m_marks.get(1)->Value);
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        if (selectPoint(mp,x1,y1)){
            action = ActionType_AT1;
            return action;
        }
        else if (selectPoint(mp, x2, y2)){
            action = ActionType_AT2;
            return action;
        }
        double *param = getTimeRulerParams(&m_marks);
        float yBHigh = pY(param[0]);
        float yBLow = pY(param[1]);
        float yEHigh = pY(param[2]);
        float yELow = pY(param[3]);
        delete[] param;
        param = 0;
        if (y1 < yBHigh){
            yBHigh = y1;
        }
        if (y1 > yBLow){
            yBLow = y1;
        }
        if (y2 < yEHigh){
            yEHigh = y2;
        }
        if (y2 > yELow){
            yELow = y2;
        }
        if (mp.x >= x1 - m_lineWidth * 2.5f && mp.x <= x1 + m_lineWidth * 2.5f){
            if (mp.y >= yBHigh - 2 && mp.y <= yBLow + 2){
                action = ActionType_MOVE;
                return action;
            }
        }
        if (mp.x >= x2 - m_lineWidth * 2.5f && mp.x <= x2 + m_lineWidth * 2.5f){
            if (mp.y >= yEHigh - m_lineWidth * 2.5f && mp.y <= yELow + m_lineWidth * 2.5f){
                action = ActionType_MOVE;
                return action;
            }
        }
        if (mp.y >= y1 - m_lineWidth * 2.5f && mp.y <= y1 + m_lineWidth * 2.5f){
            float bigX = x1 >= x2 ? x1 : x2;
            float smallX = x1 < x2 ? x1 : x2;
            if (mp.x >= smallX - m_lineWidth * 2.5f && mp.x <= bigX + m_lineWidth * 2.5f){
                action = ActionType_MOVE;
                return action;
            }
        }
        return action;
    }
    
    bool TimeRuler::onCreate(const FCPoint& mp){
        return create2PointsB(mp);
    }
    
    void TimeRuler::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_MOVE){
                setCursor(FCCursors_Hand);
            }
            else{
                setCursor(FCCursors_SizeNS);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
        }
    }
    
    void TimeRuler::onMoving(){
        FCPoint mp = getMovingPoint();
        FCChart *chart = getChart();
        int touchIndex = chart->getTouchOverIndex();
        double y = getNumberValue(mp);
        if (touchIndex < 0){
            touchIndex = 0;
        }
        if (touchIndex > chart->getLastVisibleIndex()){
            touchIndex = chart->getLastVisibleIndex();
        }
        switch (m_action){
            case ActionType_MOVE:
                move(mp);
                break;
            case ActionType_AT1:{
                double oldKey2 = m_marks.get(1)->Key;
                clearMarks(&m_marks);
                m_marks.put(0, new PlotMark(0, m_dataSource->getXValue(touchIndex), y));
                m_marks.put(1, new PlotMark(1, oldKey2, y));
                break;
            }
            case ActionType_AT2:{
                double oldKey1 = m_marks.get(0)->Key;
                clearMarks(&m_marks);
                m_marks.put(1, new PlotMark(1, m_dataSource->getXValue(touchIndex), y));
                m_marks.put(0, new PlotMark(0, oldKey1, y));
                break;
            }
        }
    }
    
    void TimeRuler::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        float y1 = pY(pList->get(0)->Value);
        float y2 = pY(pList->get(1)->Value);
        int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(pList->get(1)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        double *param = getTimeRulerParams(pList);
        float yBHigh = pY(param[0]);
        float yBLow = pY(param[1]);
        float yEHigh = pY(param[2]);
        float yELow = pY(param[3]);
        delete[] param;
        param = 0;
        if (y1 < yBHigh){
            yBHigh = y1;
        }
        if (y1 > yBLow){
            yBLow = y1;
        }
        if (y2 < yEHigh){
            yEHigh = y2;
        }
        if (y2 > yELow){
            yELow = y2;
        }
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, yBHigh, x1, yBLow);
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x2, yEHigh, x2, yELow);
        int subRecord = abs(eIndex - bIndex) + 1;
        wchar_t subRecordStr[100] ={0};
        swprintf(subRecordStr, 99, L"%d", subRecord);
        wchar_t recordStr[100] ={0};
        FCStr::contact(recordStr, subRecordStr,L"(T)", L"");
        FCSize sizeF = textSize(paint, recordStr, m_font);
        FCPlot::drawText(paint, recordStr, lineColor, m_font, (int)((x2 + x1) / 2 - sizeF.cx / 2), (int)(y1 - sizeF.cy / 2));
        if (abs(x1 - x2) > sizeF.cx){
            if (x2 >= x1){
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, (x2 + x1) / 2 - sizeF.cx / 2, y1);
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, (x2 + x1) / 2 + sizeF.cx / 2, y1, x2, y1);
            }
            else{
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x2, y1, (x2 + x1) / 2 - sizeF.cx / 2, y1);
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, (x2 + x1) / 2 + sizeF.cx / 2, y1, x1, y1);
            }
        }
        if (m_selected){
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x2, y2);
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    Triangle::Triangle(){
        m_plotType = L"TRIANGLE";
    }
    
    ActionType Triangle::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        float y1 = pY(m_marks.get(0)->Value);
        float y2 = pY(m_marks.get(1)->Value);
        float y3 = pY(m_marks.get(2)->Value);
        int aIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int bIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
        int cIndex = m_dataSource->getRowIndex(m_marks.get(2)->Key);
        float x1 = pX(aIndex);
        float x2 = pX(bIndex);
        float x3 = pX(cIndex);
        FCPoint mp = getTouchOverPoint();
        if (m_moveTimes == 1){
            action = ActionType_AT3;
            return action;
        }
        else{
            if (selectPoint(mp, x1, y1) || m_moveTimes == 1){
                action = ActionType_AT1;
                return action;
            }
            else if (selectPoint(mp, x2, y2)){
                action = ActionType_AT2;
                return action;
            }
            else if (selectPoint(mp, x3, y3)){
                action = ActionType_AT3;
                return action;
            }
        }
        if (selectTriangle(mp, x1, y1, x2, y2, x3, y3)){
            action = ActionType_MOVE;
        }
        return action;
    }
    
    bool Triangle::onCreate(const FCPoint& mp){
        FCChart *chart = getChart();
        int rIndex = m_dataSource->rowsCount();
        if (rIndex > 0){
            int currentIndex = getIndex(mp);
            double y = getNumberValue(mp);
            double date = m_dataSource->getXValue(currentIndex);
            m_marks.put(0, new PlotMark(0, date, y));
            int si = currentIndex + 10;
            if (si > chart->getLastVisibleIndex()){
                si = chart->getLastVisibleIndex();
            }
            m_marks.put(1, new PlotMark(1, m_dataSource->getXValue(si), y));
            m_marks.put(2, new PlotMark(2, date, y));
            return true;
        }
        return false;
    }
    
    void Triangle::onMoveStart(){
        m_moveTimes++;
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_MOVE){
                setCursor(FCCursors_Hand);
            }
            else{
                setCursor(FCCursors_SizeWE);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
            m_startMarks.put(2, m_marks.get(2)->copy());
        }
    }
    
    void Triangle::onPaintGhost(FCPaint *paint){
        if (m_moveTimes > 1){
            onPaint(paint, &m_startMarks, m_selectedColor);
        }
    }
    
    void Triangle::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        float y1 = pY(pList->get(0)->Value);
        float y2 = pY(pList->get(1)->Value);
        float y3 = pY(pList->get(2)->Value);
        int aIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        int bIndex = m_dataSource->getRowIndex(pList->get(1)->Key);
        int cIndex = m_dataSource->getRowIndex(pList->get(2)->Key);
        float x1 = pX(aIndex);
        float x2 = pX(bIndex);
        float x3 = pX(cIndex);
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x3, y3);
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x2, y2, x3, y3);
        if (m_selected || (x1 == x2 && x2 == x3 && y1 == y2 && y2 == y3)){
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x2, y2);
            drawSelect(paint, lineColor, x3, y3);
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    FCRect UpArrow::getUpArrowRect(float x, float y, float width){
        if(width>10){
            width = 14;
        }
        int mleft = (int)(x - width / 2);
        int mtop = (int)y;
        FCRect markRect ={(int)mleft, (int)mtop, (int)(mleft + width), (int)(mtop + width * 3 / 2)};
        return markRect;
    }
    
    UpArrow::UpArrow(){
        m_plotType = L"UPARROW";
    }
    
    ActionType UpArrow::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        double fValue = m_marks.get(0)->Value;
        int aIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        float x1 = pX(aIndex);
        float y1 = pY(fValue);
        FCRect rect ={(int)x1 - 5, (int)y1, (int)x1 + 5, (int)y1 + 10};
        FCPoint mp = getTouchOverPoint();
        if (mp.x > rect.left && mp.x <= rect.right && mp.y >= rect.top && mp.y <= rect.bottom){
            action = ActionType_MOVE;
        }
        return action;
    }
    
    bool UpArrow::onCreate(const FCPoint& mp){
        return createPoint(mp);
    }
    
    void UpArrow::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            setCursor(FCCursors_Hand);
            m_startMarks.put(0, m_marks.get(0)->copy());
        }
    }
    
    void UpArrow::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        double fValue = pList->get(0)->Value;
        int aIndex = m_dataSource->getRowIndex(pList->get(0)->Key) ;
        float x1 = pX(aIndex);
        float y1 = pY(fValue);
        int width = 10;
        FCPoint points[7];
        FCPoint p1 ={(int)x1, (int)y1};
        FCPoint p2 ={(int)(x1 + width / 2), (int)(y1 + width)};
        FCPoint p3 ={(int)(x1 + width / 4), (int)(y1 + width)};
        FCPoint p4 ={(int)(x1 + width / 4), (int)(y1 + width * 3 / 2)};
        FCPoint p5 ={(int)(x1 - width / 4), (int)(y1 + width * 3 / 2)};
        FCPoint p6 ={(int)(x1 - width / 4), (int)(y1 + width)};
        FCPoint p7 ={(int)(x1 - width / 2), (int)(y1 + width)};
        points[0] = p1;
        points[1] = p2;
        points[2] = p3;
        points[3] = p4;
        points[4] = p5;
        points[5] = p6;
        points[6] = p7;
        fillPolygon(paint, lineColor, points, 7);
        if (m_selected){
            drawSelect(paint, lineColor, x1, y1);
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    VLine::VLine(){
        m_plotType = L"VLINE";
    }
    
    ActionType VLine::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        float x1 = pX(bIndex);
        if (mp.x >= x1 - m_lineWidth * 2.5f && mp.x <= x1 + m_lineWidth * 2.5f){
            action = ActionType_MOVE;
        }
        return action;
    }
    
    bool VLine::onCreate(const FCPoint& mp){
        return createPoint(mp);
    }
    
    void VLine::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            setCursor(FCCursors_Hand);
            m_startMarks.put(0, m_marks.get(0)->copy());
        }
    }
    
    void VLine::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        float x1 = pX(bIndex);
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, (float)0, x1, (float)getWorkingAreaHeight());
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    float* WaveRuler::getWaveRulerParams(double value1, double value2){
        float y1 = pY(value1);
        float y2 = pY(value2);
        float y0 = 0, yA = 0, yB = 0, yC = 0, yD = 0, yE = 0, yF = 0, yG = 0, yH = 0, yI = 0, yMax = 0;
        y0 = y1;
        yA = y1 <= y2 ? y1 + (y2 - y1) * (0.236f / 2.618f) : y2 + (y1 - y2) * (1 - 0.236f / 2.618f);
        yB = y1 <= y2 ? y1 + (y2 - y1) * (0.362f / 2.618f) : y2 + (y1 - y2) * (1 - 0.362f / 2.618f);
        yC = y1 <= y2 ? y1 + (y2 - y1) * (0.5f / 2.618f) : y2 + (y1 - y2) * (1 - 0.5f / 2.618f);
        yD = y1 <= y2 ? y1 + (y2 - y1) * (0.618f / 2.618f) : y2 + (y1 - y2) * (1 - 0.618f / 2.618f);
        yE = y1 <= y2 ? y1 + (y2 - y1) * (1 / 2.618f) : y2 + (y1 - y2) * (1 - 1 / 2.618f);
        yF = y1 <= y2 ? y1 + (y2 - y1) * (1.382f / 2.618f) : y2 + (y1 - y2) * (1 - 1.382f / 2.618f);
        yG = y1 <= y2 ? y1 + (y2 - y1) * (1.618f / 2.618f) : y2 + (y1 - y2) * (1 - 1.618f / 2.618f);
        yH = y1 <= y2 ? y1 + (y2 - y1) * (2 / 2.618f) : y2 + (y1 - y2) * (1 - 2 / 2.618f);
        yI = y1 <= y2 ? y1 + (y2 - y1) * (2.382f / 2.618f) : y2 + (y1 - y2) * (1 - 2.382f / 2.618f);
        yMax = y2;
        float *list = new float[11];
        list[0] = y0;
        list[1] = yA;
        list[2] = yB;
        list[3] = yC;
        list[4] = yD;
        list[5] = yE;
        list[6] = yF;
        list[7] = yG;
        list[8] = yH;
        list[9] = yI;
        list[10] = yMax;
        return list;
    }
    
    WaveRuler::WaveRuler(){
        m_plotType = L"WAVERULER";
    }
    
    ActionType WaveRuler::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        float *param = getWaveRulerParams(m_marks.get(0)->Value,m_marks.get(1)->Value);
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float y1 = pY(m_marks.get(0)->Value);
        float y2 = param[10];
        if (selectPoint(mp, x1, y1) || m_moveTimes == 1){
            action = ActionType_AT1;
            delete[] param;
            param = 0;
            return action;
        }
        else if (selectPoint(mp, x2, y2)){
            action = ActionType_AT2;
            delete[] param;
            param = 0;
            return action;
        }
        float smallY = param[0] < param[10] ? param[0] : param[10];
        float bigY = param[0] >= param[10] ? param[0] : param[10];
        float mid = x1 >= x2 ? (x2 + (x1 - x2) / 2) : (x1 + (x2 - x1) / 2);
        if (mp.x >= mid - m_lineWidth * 2.5f && mp.x <= mid + m_lineWidth * 2.5f && mp.y >= smallY - m_lineWidth * 2.5f && mp.y <= bigY + m_lineWidth * 2.5f){
            action = ActionType_MOVE;
            delete[] param;
            param = 0;
            return action;
        }
        float top = 0;
        float bottom = (float)getWorkingAreaWidth();
        if (mp.y >= top && mp.y <= bottom){
            for(int i=0;i<10;i++){
                if (mp.x >= 0 && mp.x <= getWorkingAreaWidth() && mp.y >= param[i] - m_lineWidth * 2.5f && mp.y <= param[i] + m_lineWidth * 2.5f){
                    action = ActionType_MOVE;
                    delete[] param;
                    param = 0;
                    return action;
                }
            }
        }
        delete[] param;
        param = 0;
        return action;
    }
    
    bool WaveRuler::onCreate(const FCPoint& mp){
        return create2PointsB(mp);
    }
    
    void WaveRuler::onMoveStart(){
        m_moveTimes++;
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_MOVE){
                setCursor(FCCursors_Hand);
            }
            else{
                setCursor(FCCursors_SizeWE);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
        }
    }
    
    void WaveRuler::onPaintGhost(FCPaint *paint){
        if (m_moveTimes > 1){
            onPaint(paint, &m_startMarks, m_selectedColor);
        }
    }
    
    void WaveRuler::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        float y1 = pY(pList->get(0)->Value);
        float y2 = pY(pList->get(1)->Value);
        int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(pList->get(1)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float *lineParam = getWaveRulerParams(pList->get(0)->Value, pList->get(1)->Value);
        wchar_t *str[11] ={ L"0.00%", L"23.60%", L"38.20%", L"50.00%", L"61.80%", L"100.00%", L"138.20%", L"161.80%", L"200%", L"238.20%", L"261.80%"};
        float mid = x1 >= x2 ? (x2 + (x1 - x2) / 2) : (x1 + (x2 - x1) / 2);
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, mid, lineParam[0] , mid, lineParam[10]);
        for (int i = 0; i < 11; i++){
            FCSize sizeF = textSize(paint, str[i], m_font);
            float yP = lineParam[i];
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, yP, x2, yP); ;
            FCPlot::drawText(paint, str[i], lineColor, m_font, (int)mid, (int)(yP - sizeF.cy));
        }
        delete[] lineParam;
        lineParam = 0;
        if (m_selected){
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x2, y2);
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    double* TironeLevels::getTironelLevelsParams(HashMap<int,PlotMark*> *pList){
        double *hl = getCandleRange(pList);
        if (hl){
            double nHigh = hl[0];
            double nLow = hl[1];
            delete[] hl;
            hl = 0;
            double *list = new double[5];
            list[0] = nHigh;
            list[1] = nHigh - (nHigh - nLow) / 3;
            list[2] = nHigh - (nHigh - nLow) / 2;
            list[3] = nHigh - 2 * (nHigh - nLow) / 3;
            list[4] = nLow;
            return list;
        }
        else{
            return 0;
        }
    }
    
    TironeLevels::TironeLevels(){
        m_plotType = L"TIRONELEVELS";
    }
    
    ActionType TironeLevels::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        double *param = getTironelLevelsParams(&m_marks);
        double nHigh = param[0];
        double nLow = param[4];
        float highY = pY(nHigh);
        float lowY = pY(nLow);
        float smallX = x1 > x2 ? x2 : x1;
        float bigX = x1 > x2 ? x1 : x2;
        if ((mp.y >= highY - m_lineWidth * 2.5f && mp.y <= highY + m_lineWidth * 2.5f)
            || (mp.y >= lowY - m_lineWidth * 2.5f && mp.y <= lowY + m_lineWidth * 2.5f)){
            if (mp.x >= x1 - m_lineWidth * 2.5f && mp.x <= x1 + m_lineWidth * 2.5f){
                action = ActionType_AT1;
                delete[] param;
                param = 0;
                return action;
            }
            else if (mp.x >= x2 - m_lineWidth * 2.5f && mp.x <= x2 + m_lineWidth * 2.5f){
                action = ActionType_AT2;
                delete[] param;
                param = 0;
                return action;
            }
        }
        if (mp.y >= highY - m_lineWidth * 2.5f && mp.y <= highY + m_lineWidth * 2.5f){
            if (mp.x >= smallX - m_lineWidth * 2.5f && mp.x <= bigX + m_lineWidth * 2.5f){
                action = ActionType_MOVE;
                delete[] param;
                param = 0;
                return action;
            }
        }
        else if (mp.y >= lowY - m_lineWidth * 2.5f && mp.y <= lowY + m_lineWidth * 2.5f){
            if (mp.x >= smallX - m_lineWidth * 2.5f && mp.x <= bigX + m_lineWidth * 2.5f){
                action = ActionType_MOVE;
                delete[] param;
                param = 0;
                return action;
            }
        }
        for (int i = 1; i < 3; i++){
            float y = pY(param[i]);
            if (mp.y >= y - m_lineWidth * 2.5f && mp.y <= y + m_lineWidth * 2.5f){
                action = ActionType_MOVE;
                delete[] param;
                param = 0;
                return action;
            }
        }
        delete[] param;
        param = 0;
        return action;
    }
    
    bool TironeLevels::onCreate(const FCPoint& mp){
        return create2CandlePoints(mp);
    }
    
    void TironeLevels::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_MOVE){
                setCursor(FCCursors_Hand);
            }
            else{
                setCursor(FCCursors_SizeNS);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
        }
    }
    
    void TironeLevels::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(pList->get(1)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float smallX = x1 > x2 ? x2 : x1;
        float bigX = x1 > x2 ? x1 : x2;
        double *param = getTironelLevelsParams(pList);
        double nHigh = param[0];
        double nLow = param[4];
        float highY = pY(nHigh);
        float lowY = pY(nLow);
        float mid = x1 >= x2 ? (x2 + (x1 - x2) / 2) : (x1 + (x2 - x1) / 2);
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, highY, x2, highY);
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, lowY, x2, lowY);
        drawLine(paint, lineColor, m_lineWidth, 2, mid, lowY, mid, highY);
        for (int i = 1; i < 3; i++){
            float y = pY(param[i]);
            drawLine(paint, lineColor, m_lineWidth, 2, (float)0, y, (float)getWorkingAreaWidth(), y);
            wchar_t str[100] ={0};
            swprintf(str, 99, L"%d", i);
            wchar_t str2[10] ={0};
            if(i == 2){
                FCStr::contact(str2, str, L"/2", L"");
            }
            else{
                FCStr::contact(str2, str, L"/3", L"");
            }
            FCSize sizeF = textSize(paint, str, m_font);
            FCPlot::drawText(paint, str, lineColor, m_font, (int)(getWorkingAreaWidth() - sizeF.cx), (int)(y - sizeF.cy));
        }
        delete[] param;
        param = 0;
        if (m_selected){
            drawSelect(paint, lineColor, smallX, highY);
            drawSelect(paint, lineColor, smallX, lowY);
            drawSelect(paint, lineColor, bigX, highY);
            drawSelect(paint, lineColor, bigX, lowY);
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    double RaffChannel::getRRCRange(HashMap<int,PlotMark*> *pList, float *param){
        if (m_sourceFields.containsKey(L"HIGH") && m_sourceFields.containsKey(L"LOW") && param){
            float a = param[0];
            float b = param[1];
            int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
            int eIndex = m_dataSource->getRowIndex(pList->get(1)->Key);
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
            if (upSubValue >= downSubValue){
                return upSubValue;
            }
            else{
                return downSubValue;
            }
        }
        return 0;
    }
    
    RaffChannel::RaffChannel(){
        m_plotType = L"RAFFCHANNEL";
    }
    
    ActionType RaffChannel::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        FCChart *chart = getChart();
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float *param = getLRParams(&m_marks);
        if (param){
            float a = param[0];
            float b = param[1];
            double leftValue = a + b;
            double rightValue = (eIndex - bIndex + 1) * a + b;
            float y1 = pY(leftValue);
            float y2 = pY(rightValue);
            if (selectPoint(mp, x1, y1)){
                action = ActionType_AT1;
                delete[] param;
                param = 0;
                return action;
            }
            else if (selectPoint(mp, x2, y2)){
                action = ActionType_AT2;
                delete[] param;
                param = 0;
                return action;
            }
            int touchIndex = chart->getTouchOverIndex();
            if (touchIndex >= chart->getFirstVisibleIndex() && touchIndex <= chart->getLastVisibleIndex()){
                double yValue = a * ((touchIndex - bIndex) + 1) + b;
                float y = pY(yValue);
                float x = pX(touchIndex);
                if (mp.x >= x - 5 && mp.x <= x + 5 && mp.y >= y - 5 && mp.y <= y + 5){
                    action = ActionType_MOVE;
                    delete[] param;
                    param = 0;
                    return action;
                }
                double parallel = getRRCRange(&m_marks, param);
                delete[] param;
                param = 0;
                yValue = a * ((touchIndex - bIndex) + 1) + b + parallel;
                y = pY(yValue);
                x = pX(touchIndex);
                if (mp.x >= x - 5 && mp.x <= x + 5 && mp.y >= y - 5 && mp.y <= y + 5){
                    action = ActionType_MOVE;
                    return action;
                }
                yValue = a * ((touchIndex - bIndex) + 1) + b - parallel;
                y = pY(yValue);
                x = pX(touchIndex);
                if (mp.x >= x - 5 && mp.x <= x + 5 && mp.y >= y - 5 && mp.y <= y + 5){
                    action = ActionType_MOVE;
                    return action;
                }
            }
            else{
                delete[] param;
                param = 0;
            }
        }
        return action;
    }
    
    bool RaffChannel::onCreate(const FCPoint& mp){
        return create2CandlePoints(mp);
    }
    
    void RaffChannel::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_MOVE){
                setCursor(FCCursors_Hand);
            }
            else{
                setCursor(FCCursors_SizeWE);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
        }
    }
    
    void RaffChannel::onMoving(){
        FCPoint mp = getMovingPoint();
        FCChart *chart = getChart();
        int touchIndex = chart->getTouchOverIndex();
        if (touchIndex < 0){
            touchIndex = 0;
        }
        if (touchIndex > chart->getLastVisibleIndex()){
            touchIndex = chart->getLastVisibleIndex();
        }
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
        switch (m_action){
            case ActionType_MOVE:
                move(mp);
                break;
            case ActionType_AT1:
                if (touchIndex < eIndex){
                    resize(0);
                }
                break;
            case ActionType_AT2:
                if (touchIndex > bIndex){
                    resize(1);
                }
                break;
        }
    }
    
    void RaffChannel::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        float *param = getLRParams(pList);
        if (param){
            float a = param[0];
            float b = param[1];
            int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
            int eIndex = m_dataSource->getRowIndex(pList->get(1)->Key);
            float x1 = pX(bIndex);
            float x2 = pX(eIndex);
            double leftValue = a + b;
            double rightValue = (eIndex - bIndex + 1) * a + b;
            float y1 = pY(leftValue);
            float y2 = pY(rightValue);
            PlotMark plotMarkA(0, pList->get(0)->Key, leftValue);
            PlotMark plotMarkB(1, pList->get(1)->Key, rightValue);
            float *param2 = getLineParams(&plotMarkA, &plotMarkB);
            if (param2){
                a = param2[0];
                b = param2[1];
                float leftX = 0;
                float leftY = leftX * a + b;
                float rightX = (float)getWorkingAreaWidth();
                float rightY = rightX * a + b;
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, leftX, leftY, rightX, rightY);
                double parallel = getRRCRange(pList, param);
                double leftTop = leftValue + parallel;
                double rightTop = rightValue + parallel;
                delete[] param2;
                param2 = 0;
                PlotMark plotMarkC(0, pList->get(0)->Key, leftTop);
                PlotMark plotMarkD(1, pList->get(1)->Key, rightTop);
                param2 = getLineParams(&plotMarkC, &plotMarkD);
                if (param2){
                    a = param2[0];
                    b = param2[1];
                    leftX = 0;
                    leftY = leftX * a + b;
                    rightX = (float)getWorkingAreaWidth();
                    rightY = rightX * a + b;
                    drawLine(paint, lineColor, m_lineWidth, m_lineStyle, leftX, leftY, rightX, rightY);
                    delete[] param2;
                    param2 = 0;
                }
                double leftBottom = leftValue - parallel;
                double rightBottom = rightValue - parallel;
                PlotMark plotMarkE(0, pList->get(0)->Key, leftBottom);
                PlotMark plotMarkF(1, pList->get(1)->Key, rightBottom);
                param2 = getLineParams(&plotMarkE, &plotMarkF);
                if (param2){
                    a = param2[0];
                    b = param2[1];
                    leftX = 0;
                    leftY = leftX * a + b;
                    rightX = (float)getWorkingAreaWidth();
                    rightY = rightX * a + b;
                    drawLine(paint, lineColor, m_lineWidth, m_lineStyle, leftX, leftY, rightX, rightY);
                    delete[] param2;
                    param2 = 0;
                }
            }
            if (m_selected){
                drawSelect(paint, lineColor, x1, y1);
                drawSelect(paint, lineColor, x2, y2);
            }
            delete[] param;
            param = 0;
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    QuadrantLines::QuadrantLines(){
        m_plotType = L"QUADRANTLINES";
    }
    
    ActionType QuadrantLines::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        FCChart *chart = getChart();
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float *param = getLRParams(&m_marks);
        if (param){
            float a = param[0];
            float b = param[1];
            delete[] param;
            param = 0;
            double leftValue = a + b;
            double rightValue = (eIndex - bIndex + 1) * a + b;
            float y1 = pY(leftValue);
            float y2 = pY(rightValue);
            if (selectPoint(mp, x1, y1)){
                action = ActionType_AT1;
                return action;
            }
            else if (selectPoint(mp, x2, y2)){
                action = ActionType_AT2;
                return action;
            }
            int touchIndex = chart->getTouchOverIndex();
            if (touchIndex >= bIndex && touchIndex <= eIndex){
                double yValue = a * ((touchIndex - bIndex) + 1) + b;
                float y = pY(yValue);
                float x = pX(touchIndex);
                if (mp.x >= x - 5 && mp.x <= x + 5 && mp.y >= y - 5 && mp.y <= y + 5){
                    action = ActionType_MOVE;
                    return action;
                }
                double *candleRegion = getCandleRange(&m_marks);
                if (candleRegion){
                    float *percents = getPercentParams(candleRegion[0], candleRegion[1]);
                    for (int i = 0; i < 5; i++){
                        if (selectRay(mp, x1, percents[i], x2, percents[i])){
                            action = ActionType_MOVE;
                            return action;
                        }
                    }
                    delete[] candleRegion;
                    candleRegion = 0;
                    delete[] percents;
                    percents = 0;
                }
            }
        }
        return action;
    }
    
    bool QuadrantLines::onCreate(const FCPoint& mp){
        return create2CandlePoints(mp);
    }
    
    void QuadrantLines::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_MOVE){
                setCursor(FCCursors_Hand);
            }
            else{
                setCursor(FCCursors_SizeWE);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
        }
    }
    
    void QuadrantLines::onMoving(){
        FCPoint mp = getMovingPoint();
        FCChart *chart = getChart();
        int touchIndex = chart->getTouchOverIndex();
        if (touchIndex < 0){
            touchIndex = 0;
        }
        if (touchIndex > chart->getLastVisibleIndex()){
            touchIndex = chart->getLastVisibleIndex();
        }
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
        switch (m_action){
            case ActionType_MOVE:
                move(mp);
                break;
            case ActionType_AT1:
                if (touchIndex < eIndex){
                    resize(0);
                }
                break;
            case ActionType_AT2:
                if (touchIndex > bIndex){
                    resize(1);
                }
                break;
        }
    }
    
    void QuadrantLines::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        float *param = getLRParams(pList);
        if (param){
            int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
            int eIndex = m_dataSource->getRowIndex(pList->get(1)->Key);
            float x1 = pX(bIndex);
            float x2 = pX(eIndex);
            float a = param[0];
            float b = param[1];
            delete[] param;
            param = 0;
            double leftValue = a + b;
            double rightValue = (eIndex - bIndex + 1) * a + b;
            float y1 = pY(leftValue);
            float y2 = pY(rightValue);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
            double *candleRegion = getCandleRange(pList);
            if (candleRegion){
                float *percents = getPercentParams(candleRegion[0], candleRegion[1]);
                for (int i = 0; i < 5; i++){
                    float yp = percents[i];
                    if (i == 2){
                        drawLine(paint, lineColor, m_lineWidth, 2, x1, yp, x2, yp);
                    }
                    else{
                        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, yp, x2, yp);
                    }
                }
                delete[] candleRegion;
                candleRegion = 0;
                delete[] percents;
                percents = 0;
            }
            if (m_selected){
                drawSelect(paint, lineColor, x1, y1);
                drawSelect(paint, lineColor, x2, y2);
            }
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    ActionType BoxLine::getAction(const FCPoint& mp){
        double *param = getCandleRange(&m_marks);
        double nHigh = param[0];
        double nLow = param[1];
        if (param){
            PlotMark plotMarkA(0, m_marks.get(0)->Key, nHigh);
            PlotMark plotMarkB(1, m_marks.get(1)->Key, nLow);
            ActionType action = selectRect(mp, &plotMarkA, &plotMarkB);
            delete[] param;
            param = 0;
            return action;
        }
        return ActionType_NO;
    }
    
    BoxLine::BoxLine(){
        m_plotType = L"BOXLINE";
    }
    
    ActionType BoxLine::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        action = getAction(mp);
        if (action == ActionType_AT4){
            action = ActionType_AT2;
        }
        return action;
    }
    
    bool BoxLine::onCreate(const FCPoint& mp){
        return create2CandlePoints(mp);
    }
    
    void BoxLine::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_MOVE){
                setCursor(FCCursors_Hand);
            }
            else{
                setCursor(FCCursors_SizeWE);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
        }
    }
    
    void BoxLine::onMoving(){
        FCPoint mp = getMovingPoint();
        FCChart *chart = getChart();
        int touchIndex = chart->getTouchOverIndex();
        if (touchIndex < 0){
            touchIndex = 0;
        }
        if (touchIndex > chart->getLastVisibleIndex()){
            touchIndex = chart->getLastVisibleIndex();
        }
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
        switch (m_action){
            case ActionType_MOVE:
                move(mp);
                break;
            case ActionType_AT1:
                if (touchIndex < eIndex){
                    resize(0);
                }
                break;
            case ActionType_AT2:
                if (touchIndex > bIndex){
                    resize(1);
                }
                break;
        }
    }
    
    void BoxLine::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(pList->get(1)->Key);
        double *param = getCandleRange(pList);
        double nHigh = param[0];
        double nLow = param[1];
        if (param){
            delete[] param;
            param = 0;
            PlotMark plotMarkA(0, pList->get(0)->Key, nHigh);
            PlotMark plotMarkB(1, pList->get(1)->Key, nLow);
            FCRect rect = getRectangle(&plotMarkA, &plotMarkB);
            int x1 = rect.left;
            int y1 = rect.top;
            int x2 = rect.right;
            int y2 = rect.bottom;
            if (rect.right - rect.left >= 0 && rect.bottom - rect.top >= 0){
                int rwidth = rect.right-rect.left;
                int rheight = rect.bottom-rect.top;
                drawRect(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, rect.right, rect.bottom);
                int count = abs(bIndex - eIndex) + 1;
                wchar_t cStr[100] ={0};
                swprintf(cStr, 99, L"%d", count);
                wchar_t cStr2[100] ={0};
                FCStr::contact(cStr2, L"COUNT:", cStr, L"");
                FCPlot::drawText(paint, cStr2, lineColor, m_font, x1 + 2, y1 + 2);
                double diff = nLow - nHigh;
                double range = 0;
                if(nHigh != 0){
                    nHigh = 100 * diff / nHigh;
                }
                wchar_t diffString[100] ={0};
                swprintf(diffString, 99, L"%.2f", diff);
                wchar_t rangeString[100] ={0};
                swprintf(rangeString, 99, L"%.2f", range);
                wchar_t rangeString2[100] ={0};
                FCStr::contact(rangeString2, rangeString,L"%", L"");
                FCSize rangeSize = textSize(paint, rangeString2, m_font);
                FCPlot::drawText(paint, diffString, lineColor, m_font, x1 + rwidth + 2, y1 + 2);
                FCPlot::drawText(paint, rangeString2, lineColor, m_font, x1 + rwidth + 2, y1 + rheight - rangeSize.cy - 2);
                if (m_sourceFields.containsKey(L"CLOSE")){
                    int aryL = 0;
                    double *ary = m_dataSource->DATA_ARRAY(m_sourceFields.get(L"CLOSE"), eIndex, eIndex - bIndex + 1, &aryL);
                    double avg = FCScript::avgValue(ary, aryL);
                    if(ary){
                        delete[] ary;
                        ary = 0;
                    }
                    float yAvg = pY(avg);
                    drawLine(paint, lineColor, m_lineWidth, 2, (float)x1, yAvg, (float)x2, yAvg);
                    wchar_t avgString[100] ={0};
                    swprintf(avgString, 99, L"%.2f", avg);
                    wchar_t avgString2[100] ={0};
                    FCStr::contact(avgString2, L"AVG:", avgString, L"");
                    FCSize avgSize = textSize(paint, avgString2, m_font);
                    FCPlot::drawText(paint, avgString2, lineColor, m_font, (int)(x1 + 2), (int)(yAvg - avgSize.cy - 2));
                }
            }
            if (m_selected){
                drawSelect(paint, lineColor, (float)x1, (float)y1);
                drawSelect(paint, lineColor, (float)x2, (float)y2);
            }
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    ParallelGram::ParallelGram(){
        m_plotType = L"PARALLELOGRAM";
    }
    
    ActionType ParallelGram::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        if (m_moveTimes == 1){
            action = ActionType_AT3;
            return action;
        }
        else{
            float y1 = pY(m_marks.get(0)->Value);
            float y2 = pY(m_marks.get(1)->Value);
            float y3 = pY(m_marks.get(2)->Value);
            int aIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
            int bIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
            int cIndex = m_dataSource->getRowIndex(m_marks.get(2)->Key);
            float x1 = pX(aIndex);
            float x2 = pX(bIndex);
            float x3 = pX(cIndex);
            if (selectPoint(mp, x1, y1) || m_moveTimes == 1){
                action = ActionType_AT1;
                return action;
            }
            else if (selectPoint(mp, x2, y2)){
                action = ActionType_AT2;
                return action;
            }
            else if (selectPoint(mp, x3, y3)){
                action = ActionType_AT3;
                return action;
            }
        }
        FCPoint *points = getPLPoints(&m_marks);
        for (int i = 0; i < 4; i++){
            int start = i;
            int end = i + 1;
            if (start == 3){
                end = 0;
            }
            if (selectRay(mp, (float)points[start].x, (float)points[start].y, (float)points[end].x, (float)points[end].y)){
                action = ActionType_MOVE;
                delete[] points;
                points = 0;
                return action;
            }
        }
        delete[] points;
        points = 0;
        return action;
    }
    
    FCPoint* ParallelGram::getPLPoints(HashMap<int,PlotMark*> *pList){
        FCPoint point1 ={(int)pX(m_dataSource->getRowIndex(m_marks.get(0)->Key)), (int)pY(m_marks.get(0)->Value)};
        FCPoint point2 ={(int)pX(m_dataSource->getRowIndex(m_marks.get(1)->Key)), (int)pY(m_marks.get(1)->Value)};
        FCPoint point3 ={(int)pX(m_dataSource->getRowIndex(m_marks.get(2)->Key)), (int)pY(m_marks.get(2)->Value)};
        float x1 = 0, y1 = 0, x2 = 0, y2 = 0, x3 = 0, y3 = 0, x4 = 0, y4 = 0;
        x1 = (float)point1.x;
        y1 = (float)point1.y;
        x2 = (float)point2.x;
        y2 = (float)point2.y;
        x3 = (float)point3.x;
        y3 = (float)point3.y;
        parallelogram(x1, y1, x2, y2, x3, y3, &x4, &y4);
        FCPoint point4 ={(int)x4, (int)y4};
        FCPoint *list = new FCPoint[4];
        list[0] = point1;
        list[1] = point2;
        list[2] = point3;
        list[3] = point4;
        return list;
    }
    
    bool ParallelGram::onCreate(const FCPoint& mp){
        FCChart *chart = getChart();
        int rIndex = m_dataSource->rowsCount();
        if (rIndex > 0){
            int currentIndex = getIndex(mp);
            double y = getNumberValue(mp);
            double date = m_dataSource->getXValue(currentIndex);
            m_marks.put(0, new PlotMark(0, date, y));
            int si = currentIndex + 10;
            if (si > chart->getLastVisibleIndex()){
                si = chart->getLastVisibleIndex();
            }
            m_marks.put(1, new PlotMark(1, m_dataSource->getXValue(si), y));
            m_marks.put(2, new PlotMark(2, date, y));
            return true;
        }
        return false;
    }
    
    void ParallelGram::onMoveStart(){
        m_moveTimes++;
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_MOVE){
                setCursor(FCCursors_Hand);
            }
            else{
                setCursor(FCCursors_SizeWE);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
            m_startMarks.put(2, m_marks.get(2)->copy());
        }
    }
    
    void ParallelGram::onPaintGhost(FCPaint *paint){
        if (m_moveTimes > 1){
            onPaint(paint, &m_startMarks, m_selectedColor);
        }
    }
    
    void ParallelGram::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        FCPoint *points = getPLPoints(pList);
        for (int i = 0; i < 4; i++){
            int start = i;
            int end = i + 1;
            if (start == 3){
                end = 0;
            }
            float x1 = (float)points[start].x;
            float y1 = (float)points[start].y;
            float x2 = (float)points[end].x;
            float y2 = (float)points[end].y;
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
            if (m_selected && i < 3){
                drawSelect(paint, lineColor, x1, y1);
            }
        }
        delete[] points;
        points = 0;
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    Circle::Circle(){
        m_plotType = L"CIRCLE";
    }
    
    ActionType Circle::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        float y1 = pY(m_marks.get(0)->Value);
        float y2 = pY(m_marks.get(1)->Value);
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(m_marks.get(1)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        if (selectPoint(mp, x1, y1)){
            action = ActionType_AT1;
        }
        else if (selectPoint(mp, x2, y2)){
            action = ActionType_AT2;
        }
        else{
            float r = (float)(sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1)));
            FCPoint p ={(int)(mp.x - x1), (int)(mp.y - y1)};
            float round = (float)(p.x * p.x + p.y * p.y);
            if (round / (r * r) >= 0.9 && round / (r * r) <= 1.1){
                action = ActionType_MOVE;
            }
        }
        return action;
    }
    
    bool Circle::onCreate(const FCPoint& mp){
        return create2PointsA(mp);
    }
    
    void Circle::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_MOVE){
                setCursor(FCCursors_Hand);
            }
            else{
                setCursor(FCCursors_SizeWE);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
        }
    }
    
    void Circle::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        float y1 = pY(pList->get(0)->Value);
        float y2 = pY(pList->get(1)->Value);
        int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        int eIndex = m_dataSource->getRowIndex(pList->get(1)->Key);
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float r = (float)sqrt(abs((x2 - x1)*(x2 - x1) + (y2 - y1) * (y2 - y1)));
        drawEllipse(paint, lineColor, m_lineWidth, m_lineStyle,  x1 - r, y1 - r, x1 + r, y1 + r);
        if (m_selected){
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x2, y2);
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    void PriceChannel::getLine3Params(HashMap<int,PlotMark*> *pList, float *k, float *d, float *x4){
        int bIndex = m_dataSource->getRowIndex(m_marks.get(0)->Key);
        int pIndex = m_dataSource->getRowIndex(m_marks.get(2)->Key);
        float x1 = pX(bIndex);
        float x3 = pX(pIndex);
        float *param = getParallelParams(&m_marks);
        if (param){
            *k = param[0];
            float b = param[1];
            float c = param[2];
            delete[] param;
            param = 0;
            *d = b >= c ? b + (b - c) : b - (c - b);
        }
        else{
            *x4 = 0;
            if (x3 > x1){
                *x4 = x1 - (x3 - x1);
            }
            else{
                *x4 = x1 + (x1 - x3);
            }
        }
    }
    
    void PriceChannel::paintEx(FCPaint *paint, HashMap<int,PlotMark*> *pList,Long lineColor){
        if (pList->size() == 0)
            return;
        float k = 0, d = 0, x4 = 0;
        getLine3Params(&m_marks, &k, &d, &x4);
        if (k == 0 && d == 0){
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x4, (float)0, x4, (float)getWorkingAreaHeight());
        }
        else{
            float leftX = 0;
            float leftY = leftX * k + d;
            float rightX = (float)getWorkingAreaWidth();
            float rightY = rightX * k + d;
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, leftX, leftY, rightX, rightY);
        }
    }
    
    PriceChannel::PriceChannel(){
        m_plotType = L"PRICECHANNEL";
    }
    
    ActionType PriceChannel::getAction(){
        ActionType action = ActionType_NO;
        action = Parallel::getAction();
        if (action != ActionType_NO){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        float k = 0, d = 0, x4 = 0;
        getLine3Params(&m_marks, &k, &d, &x4);
        if (k == 0 && d == 0){
            if (mp.x >= x4 - m_lineWidth * 5 && mp.x <= x4 + m_lineWidth * 5){
                action = ActionType_MOVE;
            }
        }
        else{
            if (selectLine(mp, pX(mp.x), k, d)){
                action = ActionType_MOVE;
            }
        }
        return action;
    }
    
    
    
    void PriceChannel::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            setCursor(FCCursors_Hand);
            m_startMarks.put(0, m_marks.get(0)->copy());
            m_startMarks.put(1, m_marks.get(1)->copy());
            m_startMarks.put(2, m_marks.get(2)->copy());
        }
    }
    
    void PriceChannel::onPaint(FCPaint *paint){
        paintEx(paint, &m_marks, m_color);
        FCPlot::onPaint(paint);
    }
    
    void PriceChannel::onPaintGhost(FCPaint *paint){
        paintEx(paint, &m_startMarks, m_selectedColor);
        FCPlot::onPaintGhost(paint);
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    Gp::Gp(){
        m_plotType = L"GP";
    }
    
    ActionType Gp::getAction(){
        ActionType action = ActionType_NO;
        if (m_marks.size() == 0){
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        float y1 = pY(m_marks.get(0)->Value);
        if (mp.y >= y1 - m_lineWidth * 2.5f && mp.y <= y1 + m_lineWidth * 2.5f){
            action = ActionType_MOVE;
        }
        else{
            double list[11] ={ 0.236, 0.382, 0.5, 0.618, 0.819, 1.191, 1.382, 1.6180, 2, 2.382, 2.618 };
            for (int i = 0; i < 11; i++){
                float yP = pY(list[i] * m_marks.get(0)->Value);
                if (mp.y >= yP - m_lineWidth * 2.5f && mp.y <= yP + m_lineWidth * 2.5f){
                    action = ActionType_MOVE;
                    break;
                }
            }
        }
        return action;
    }
    
    bool Gp::onCreate(const FCPoint& mp){
        return createPoint(mp);
    }
    
    void Gp::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType_NO){
            if (m_action == ActionType_MOVE){
                setCursor(FCCursors_Hand);
            }
            else{
                setCursor(FCCursors_SizeWE);
            }
            m_startMarks.put(0, m_marks.get(0)->copy());
        }
    }
    
    void Gp::onMoving(){
        FCPoint mp = getMovingPoint();
        switch (m_action){
            case ActionType_MOVE:
                move(mp);
                break;
        }
    }
    
    void Gp::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        float y1 = pY(pList->get(0)->Value);
        int bIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        float x1 = pX(bIndex);
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, (float)0, y1, (float)getWorkingAreaWidth(), y1);
        double list[11] ={ 0.236, 0.382, 0.5, 0.618, 0.819, 1.191, 1.382, 1.6180, 2, 2.382, 2.618 };
        for (int i = 0; i < 11; i++){
            float yP = pY(list[i] * pList->get(0)->Value);
            wchar_t str[100] ={0};
            swprintf(str, 99, L"%.2f", list[i] * 100);
            wchar_t str2[100] ={0};
            FCStr::contact(str2, str, L"%", L"");
            FCSize sizeF = textSize(paint, str2, m_font);
            drawLine(paint, lineColor, m_lineWidth, 2, (float)0, yP, (float)getWorkingAreaWidth(), yP);
            FCPlot::drawText(paint, str2, lineColor, m_font, (int)(getWorkingAreaWidth() - sizeF.cx), (int)(yP - sizeF.cy));
        }
        if (m_selected){
            drawSelect(paint, lineColor, x1, y1);
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    float* Ga::getGoldenRatioAimParams(HashMap<int,PlotMark*> *pList){
        double baseValue = pList->get(0)->Value;
        if (pList->get(1)->Value >= pList->get(2)->Value){
            return goldenRatioParams(baseValue, baseValue + pList->get(1)->Value - pList->get(2)->Value);
        }
        else{
            return goldenRatioParams(baseValue + pList->get(1)->Value - pList->get(2)->Value, baseValue);
        }
    }
    
    Ga::Ga(){
        m_plotType = L"GA";
    }
    
    ActionType Ga::getAction(){
        ActionType action = ActionType_NO;
        action = Triangle::getAction();
        if(action != ActionType_NO){
            return action;
        }
        else{
            if (hLinesSelect(getGoldenRatioAimParams(&m_marks), 6)){
                m_action = ActionType_MOVE;
            }
        }
        return m_action;
    }
    
    void Ga::onMoveStart(){
        m_action = getAction();
        clearMarks(&m_startMarks);
        m_startPoint = getTouchOverPoint();
        setCursor(FCCursors_Hand);
        m_startMarks.put(0, m_marks.get(0)->copy());
        m_startMarks.put(1, m_marks.get(1)->copy());
        m_startMarks.put(2, m_marks.get(2)->copy());
    }
    
    void Ga::onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor){
        if (pList->size() == 0)
            return;
        float y1 = pY(pList->get(0)->Value);
        float y2 = pY(pList->get(1)->Value);
        float y3 = pY(pList->get(2)->Value);
        int aIndex = m_dataSource->getRowIndex(pList->get(0)->Key);
        int bIndex = m_dataSource->getRowIndex(pList->get(1)->Key);
        int cIndex = m_dataSource->getRowIndex(pList->get(2)->Key);
        float x1 = pX(aIndex);
        float x2 = pX(bIndex);
        float x3 = pX(cIndex);
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x2, y2, x3, y3);
        float *lineParam = getGoldenRatioAimParams(pList);
        wchar_t *str[6] ={L"0.00%", L"23.60%", L"38.20%", L"50.00%", L"61.80%", L"100.00%"};
        for (int i = 0; i < 6; i++){
            FCSize sizeF = textSize(paint, str[i], m_font);
            float yP = lineParam[i];
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, (float)0, yP, (float)getWorkingAreaWidth(), yP);
            FCPlot::drawText(paint, str[i], lineColor, m_font, (int)(getWorkingAreaWidth() - sizeF.cx), (int)(yP - sizeF.cy));
        }
        delete[] lineParam;
        lineParam = 0;
        if (m_selected || (x1 == x2 && x2 == x3 && y1 == y2 && y2 == y3)){
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x2, y2);
            drawSelect(paint, lineColor, x3, y3);
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    FCPlot* PFactory::createPlot(const String& plotType){
        if(plotType == L"ANDREWSPITCHFORK") return new AndrewSpitchfork();
        else if(plotType == L"ANGLELINE") return new AngleLine();
        else if(plotType == L"CIRCUMCIRCLE") return new CircumCircle();
        else if(plotType == L"ARROWSEGMENT") return new ArrowSegment();
        else if(plotType == L"DOWNARROW") return new DownArrow();
        else if(plotType == L"DROPLINE") return new DropLine();
        else if(plotType == L"ELLIPSE") return new Ellipse();
        else if(plotType == L"FIBOELLIPSE") return new FiboEllipse();
        else if(plotType == L"FIBOFANLINE") return new FiboFanline();
        else if(plotType == L"FIBOTIMEZONE") return new FiboTimeZone();
        else if(plotType == L"GANNBOX") return new GannBox();
        else if(plotType == L"GANNLINE") return new GannLine();
        else if(plotType == L"GOLDENRATIO") return new GoldenRatio();
        else if(plotType == L"HLINE") return new HLine();
        else if(plotType == L"LEVELGRADING") return new LevelGrading();
        else if(plotType == L"LINE") return new Line();
        else if(plotType == L"LRBAND") return new LrBand();
        else if(plotType == L"LRCHANNEL") return new LrChannel();
        else if(plotType == L"LRLINE") return new LrLine();
        else if(plotType == L"NULLFCPoint") return new NullPoint();
        else if(plotType == L"PARALLEL") return new Parallel();
        else if(plotType == L"PERCENT") return new Percent();
        else if(plotType == L"PERIODIC") return new Periodic();
        else if(plotType == L"PRICE") return new Price();
        else if(plotType == L"RANGERULER") return new RangeRuler();
        else if(plotType == L"RASELINE") return new RaseLine();
        else if(plotType == L"RAY") return new Ray();
        else if(plotType == L"FCRect") return new RectLine();
        else if(plotType == L"SEGMENT") return new Segment();
        else if(plotType == L"SINE") return new Sine();
        else if(plotType == L"SPEEDRESIST") return new SpeedResist();
        else if(plotType == L"SECHANNEL") return new SeChannel();
        else if(plotType == L"SYMMETRICLINE") return new SymmetricLine();
        else if(plotType == L"SYMMETRICTRIANGLE") return new SymmetricTriangle();
        else if(plotType == L"TIMERULER") return new TimeRuler();
        else if(plotType == L"TRIANGLE") return new Triangle();
        else if(plotType == L"UPARROW") return new UpArrow();
        else if(plotType == L"VLINE") return new VLine();
        else if(plotType == L"WAVERULER") return new WaveRuler();
        else if(plotType == L"TIRONELEVELS") return new TironeLevels();
        else if(plotType == L"RAFFCHANNEL") return new RaffChannel();
        else if(plotType == L"QUADRANTLINES") return new QuadrantLines();
        else if(plotType == L"BOXLINE") return new BoxLine();
        else if(plotType == L"PARALLELOGRAM") return new ParallelGram();
        else if(plotType == L"CIRCLE") return new Circle();
        else if(plotType == L"PRICECHANNEL") return new PriceChannel();
        else if(plotType == L"GP") return new Gp();
        else if(plotType == L"GA") return new Ga();
        else return 0;
    }
}
