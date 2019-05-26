#include "stdafx.h"
#include "FCGroupBox.h"

namespace FaceCat{
    FCGroupBox::FCGroupBox(){
    }
    
    FCGroupBox::~FCGroupBox(){
    }
    
    String FCGroupBox::getControlType(){
        return L"GroupBox";
    }
    
    void FCGroupBox::onPaintBorder(FCPaint *paint, const FCRect& clipRect){
        FCFont *font = getFont();
        int width = getWidth(), height = getHeight();
        String text = getText();
        FCSize tSize ={0};
        if (text.length() > 0){
            tSize = paint->textSize(text.c_str(), font);
        }
        else{
            tSize = paint->textSize(L"0", font);
            tSize.cx = 0;
        }
        FCPoint pts[6];
        int tMid = tSize.cy / 2;
        int padding = 2;
        FCPoint pt1 ={10, tMid};
        FCPoint pt2 ={padding, tMid};
        FCPoint pt3 ={padding, height - padding};
        FCPoint pt4 ={width - padding, height - padding};
        FCPoint pt5 ={width - padding, tMid};
        FCPoint pt6 ={14 + tSize.cx, tMid};
        pts[0] = pt1;
        pts[1] = pt2;
        pts[2] = pt3;
        pts[3] = pt4;
        pts[4] = pt5;
        pts[5] = pt6;
        paint->drawPolyline(getPaintingBorderColor(), 1, 0, pts, 6);
        callPaintEvents(FCEventID::PAINTBORDER, paint, clipRect);
    }
    
    void FCGroupBox::onPaintForeground(FCPaint *paint, const FCRect& clipRect){
        String text = getText();
        if (text.length() > 0){
            FCFont *font = getFont();
            FCSize tSize = paint->textSize(text.c_str(), font);
            FCRect tRect ={12, 0, 12 + tSize.cx, tSize.cy};
            paint->drawText(text.c_str(), getPaintingTextColor(), font, tRect);
        }
    }
}
