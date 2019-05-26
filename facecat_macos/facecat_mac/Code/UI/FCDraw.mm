#include "stdafx.h"
#include "FCDraw.h"

Long FCDraw::getBlackColor(Long color){
    if (color > FCCOLORS_USERCOLOR){
        if (color == FCColor_Back){
            color = FCColor::argb(100, 0, 0, 0);
        }
        else if (color == FCColor_Border){
            color = FCColor::argb(43, 138, 195);
        }
        else if (color == FCColor_Text){
            color = FCColor::argb(255, 255, 255);
        }
        else if (color == FCColor_DisabledBack){
            color = FCColor::argb(25, 255, 255, 255);
        }
        else if (color == FCColor_DisabledText){
            color = 3289650;
        }
        else if (color == FCColor_Hovered){
            color = FCColor::argb(150, 43, 138, 195);
        }
        else if (color == FCColor_Pushed){
            color = FCColor::argb(100, 43, 138, 195);
        }
    }
    if (color == FCCOLORS_BACKCOLOR){
        color = FCColor::argb(180, 43, 138, 195);
    }
    else if (color == FCCOLORS_BACKCOLOR2){
        color = FCColor::argb(130, 43, 138, 195);
    }
    else if (color == FCCOLORS_BACKCOLOR3){
        color = FCColor::argb(150, 0, 0, 0);
    }
    else if (color == FCCOLORS_BACKCOLOR4){
        color = FCColor::argb(0, 0, 0);
    }
    else if (color == FCCOLORS_BACKCOLOR5){
        color = FCColor::argb(25, 255, 255, 255);
    }
    else if (color == FCCOLORS_BACKCOLOR6){
        color = FCColor::argb(25, 0, 0, 0);
    }
    else if (color == FCCOLORS_BACKCOLOR7){
        color = FCColor::argb(200, 255, 0, 0);
    }
    else if(color == FCCOLORS_BACKCOLOR8){
        color = FCColor::argb(200, 0, 0, 0);
    }
    else if(color == FCCOLORS_BACKCOLOR9){
        color = FCColor::argb(9, 30, 42);
    }
    else if (color == FCCOLORS_FORECOLOR){
        color = FCColor::argb(255, 255, 255);
    }
    else if(color == FCCOLORS_FORECOLOR2){
        color = FCColor::argb(0, 255, 255);
    }
    else if(color == FCCOLORS_FORECOLOR3){
        color = FCColor::argb(255, 200, 0);
    }
    else if (color == FCCOLORS_LINECOLOR){
        color = FCColor::argb(255, 255, 255);
    }
    else if (color == FCCOLORS_LINECOLOR2){
        color = FCColor::argb(9, 30, 42);
    }
    else if(color == FCCOLORS_LINECOLOR3){
        color = FCColor::argb(5, 255, 255, 255);
    }
    else if (color == FCCOLORS_MIDCOLOR){
        color = FCColor::argb(255, 255, 255);
    }
    else if (color == FCCOLORS_UPCOLOR){
        color = FCColor::argb(255, 82, 82);
    }
    else if (color == FCCOLORS_DOWNCOLOR){
        color = FCColor::argb(80, 255, 80);
    }
    else if (color == FCCOLORS_SELECTEDROWCOLOR){
        color = FCColor::argb(200, 43, 138, 195);
    }
    else if (color == FCCOLORS_HOVEREDROWCOLOR){
        color = FCColor::argb(100, 43, 138, 195);
    }
    else if (color == FCCOLORS_WINDOWFORECOLOR){
        color = FCColor::argb(255, 255, 255);
    }
    else if (color == FCCOLORS_WINDOWBACKCOLOR){
        color = FCColor::argb(255, 50, 50, 50);
    }
    else if (color == FCCOLORS_WINDOWBACKCOLOR2){
        color = FCColor::argb(230, 43, 138, 195);
    }
    else if (color == FCCOLORS_WINDOWBACKCOLOR3){
        color = FCColor::argb(230, 43, 138, 195);
    }
    else if (color == FCCOLORS_WINDOWCONTENTBACKCOLOR){
        color = FCColor::argb(235, 9, 30, 42);
    }
    return color;
}

Long FCDraw::getWhiteColor(Long color){
    if (color < FCColor_None){
        if (color > FCCOLORS_USERCOLOR){
            if (color == FCColor_Back){
                color = FCColor::argb(255, 255, 255);
            }
            else if (color == FCColor_Border){
                color = FCColor::argb(200, 200, 200);
            }
            else if (color == FCColor_Text){
                color = FCColor::argb(0, 0, 0);
            }
            else if (color == FCColor_DisabledBack){
                color = FCColor::argb(25, 255, 255, 255);
            }
            else if (color == FCColor_DisabledText){
                color = 3289650;
            }
            else if (color == FCColor_Hovered){
                color = FCColor::argb(150, 200, 200, 200);
            }
            else if (color == FCColor_Pushed){
                color = FCColor::argb(150, 150, 150, 150);
            }
        }
        else if (color == FCCOLORS_BACKCOLOR){
            color = FCColor::argb(255, 255, 255);
        }
        else if (color == FCCOLORS_BACKCOLOR2){
            color = FCColor::argb(230, 230, 230);
        }
        else if (color == FCCOLORS_BACKCOLOR3){
            color = FCColor::argb(25, 100, 100, 100);
        }
        else if (color == FCCOLORS_BACKCOLOR4){
            color = FCColor::argb(25, 0, 0, 0);
        }
        else if (color == FCCOLORS_BACKCOLOR5){
            color = FCColor::argb(75, 51, 153, 255);
        }
        else if (color == FCCOLORS_BACKCOLOR6){
            color = FCColor::argb(50, 51, 153, 255);
        }
        else if (color == FCCOLORS_BACKCOLOR7){
            color = FCColor::argb(100, 255, 255, 255);
        }
        else if (color == FCCOLORS_BACKCOLOR8){
            color = FCColor::argb(50, 105, 217);
        }
        else if (color == FCCOLORS_BACKCOLOR9){
            color = FCColor::argb(75, 215, 99);
        }
        else if (color == FCCOLORS_FORECOLOR){
            color = FCColor::argb(0, 0, 0);
        }
        else if (color == FCCOLORS_FORECOLOR2){
            color = FCColor::argb(112, 112, 112);
        }
        else if (color == FCCOLORS_FORECOLOR3){
            color = FCColor::argb(100, 255, 255, 255);
        }
        else if (color == FCCOLORS_FORECOLOR4){
            color = FCColor::argb(255, 255, 255);
        }
        else if (color == FCCOLORS_LINECOLOR){
            color = FCColor::argb(100, 100, 100);
        }
        else if (color == FCCOLORS_LINECOLOR2){
            color = FCColor::argb(0, 105, 217);
        }
        else if (color == FCCOLORS_UPCOLOR){
            color = FCColor::argb(255, 82, 82);
        }
        else if (color == FCCOLORS_DOWNCOLOR){
            color = FCColor::argb(80, 255, 80);
        }
        else if (color == FCCOLORS_LINECOLOR3){
            color = FCColor::argb(5, 255, 255, 255);
        }
        else if (color == FCCOLORS_SELECTEDROWCOLOR){
            color = FCColor::argb(0, 105, 217);
        }
        else if (color == FCCOLORS_HOVEREDROWCOLOR){
            color = FCColor::argb(200, 240, 240, 240);
        }
        else if (color == FCCOLORS_ALTERNATEROWCOLOR){
            color = FCColor::argb(200, 245, 245, 245);
        }
        else if (color == FCCOLORS_WINDOWFORECOLOR){
            color = FCColor::argb(0, 0, 0);
        }
        else if (color == FCCOLORS_WINDOWBACKCOLOR){
            color = FCColor::argb(255, 255, 255);
        }
        else if (color == FCCOLORS_WINDOWBACKCOLOR2){
            color = FCColor::argb(100, 255, 255, 255);
        }
        else if (color == FCCOLORS_WINDOWBACKCOLOR3){
            color = FCColor::argb(230, 255, 255, 255);
        }
        else if (color == FCCOLORS_WINDOWCROSSLINECOLOR){
            color = FCColor::argb(100, 100, 100);
        }
        else if (color == FCCOLORS_WINDOWCROSSLINECOLOR2){
            color = FCColor::argb(10, 255, 255, 255);
        }
        else if (color == FCCOLORS_WINDOWCONTENTBACKCOLOR){
            color = FCColor::argb(235, 255, 255, 255);
        }
    }
    return color;
}

void FCDraw::drawText(FCPaint *paint, const wchar_t *strText, Long dwPenColor, FCFont *font, int x, int y){
    FCSize textSize = paint->textSize(strText, font);
    FCRect rect = {x, y, x + textSize.cx, y + textSize.cy};
    paint->drawText(strText, dwPenColor, font, rect);
}

int FCDraw::drawUnderLineNum(FCPaint *paint, double value, int digit, FCFont *font, int fontColor, bool zeroAsEmpty, int x, int y)
{
    if(zeroAsEmpty && value == 0){
        String text = L"-";
        FCSize size = paint->textSize(text.c_str(), font);
        FCRect tRect = {x, y, x + size.cx, y + size.cy};
        paint->drawText(text.c_str(), fontColor, font, tRect);
        return size.cx;
    }
    else{
        wchar_t strValue[100] = {0};
        FCStr::getValueByDigit(value, digit, strValue);
        ArrayList<String> nbs = FCStr::split(strValue, L".");
        if(nbs.size() == 1){
            FCSize size = paint->textSize(nbs.get(0).c_str(), font);
            FCRect tRect = {x, y, x + size.cx, y + size.cy};
            paint->drawText(nbs.get(0).c_str(), fontColor, font, tRect);
            return size.cx;
        }
        else{
            FCSize decimalSize = paint->textSize(nbs.get(0).c_str(), font);
            FCSize size = paint->textSize(nbs.get(1).c_str(), font);
            FCDraw::drawText(paint, nbs.get(0).c_str(), fontColor, font, x, y);
            FCDraw::drawText(paint, nbs.get(1).c_str(), fontColor, font, x
                            + decimalSize.cx + 1, y);
            paint->drawLine(fontColor, 1, 0, x
                            + decimalSize.cx + 1, y + decimalSize.cy,
                            x + decimalSize.cx + size.cx, y + decimalSize.cy);
            return decimalSize.cx + size.cx;
        }
    }
}

Long FCDraw::getPriceColor(double price, double comparePrice){
    if(price != 0){
        if(price > comparePrice){
            return FCCOLORS_UPCOLOR;
        }
        else if(price < comparePrice){
            return FCCOLORS_DOWNCOLOR;
        }
    }
    return FCCOLORS_MIDCOLOR;
}
