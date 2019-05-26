#include "stdafx.h"
#include "BarrageDiv.h"

Barrage::Barrage(){
    m_color = 0;
    m_font = new FCFont(L"SimSun", 40, true, false, false);
    m_mode = 0;
    m_rect.left = 0;
    m_rect.top = 0;
    m_rect.right = 0;
    m_rect.bottom = 0;
    m_speed = 10;
    m_text = L"";
    m_tick = 200;
}

Barrage::~Barrage(){
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

Long Barrage::getColor(){
    return m_color;
}

void Barrage::setColor(Long color){
    m_color = color;
}

FCFont* Barrage::getFont(){
    return m_font;
}

void Barrage::setFONT(FCFont *font){
    m_font->copy(font);
}

int Barrage::getMode(){
    return m_mode;
}

void Barrage::setMode(int mode){
    m_mode = mode;
}

int Barrage::getSpeed(){
    return m_speed;
}

void Barrage::setSpeed(int speed){
    m_speed = speed;
}

FCRect Barrage::getRect(){
    return m_rect;
}

void Barrage::setRect(const FCRect& rect){
    m_rect = rect;
}

String Barrage::getText(){
    return m_text;
}

void Barrage::setText(const String& text){
    m_text = text;
}

int Barrage::getTick(){
    return m_tick;
}

void Barrage::setTick(int tick){
    m_tick = tick;
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void Barrage::calculate(){
    m_rect.left -= m_speed;
    m_rect.right -= m_speed;
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

BarrageDiv::BarrageDiv(){
    m_sysColors[0] = FCColor::argb(255, 255, 255);
    m_sysColors[1] = FCColor::argb(255,255,0);
    m_sysColors[2] = FCColor::argb(255, 0, 255);
    m_sysColors[3] = FCColor::argb(0, 255, 0);
    m_sysColors[4] = FCColor::argb(82, 255, 255);
    m_sysColors[5] = FCColor::argb(255, 82, 82);
    m_tick = 0;
    m_timerID = getNewTimerID();
    setBackColor(FCColor_None);
}

BarrageDiv::~BarrageDiv(){
    stopTimer(m_timerID);
    m_lock.lock();
    vector<Barrage*>::iterator sIter = m_barrages.begin();
    for(; sIter != m_barrages.end(); ++sIter)
    {
        delete *sIter;
    }
    m_barrages.clear();
    m_lock.unLock();
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void BarrageDiv::addBarrage(Barrage *barrage){
    barrage->setColor(m_sysColors[m_tick % 6]);
    int width = getWidth(), height = getHeight();
    if(height < 100){
        height = 100;
    }
    int mode = barrage->getMode();
    if(barrage->getMode() == 0){
        FCRect rect = {width, rand() % height, width, 0};
        barrage->setRect(rect);
    }
    else{
        int left = 0, top = 0;
        if(width > 200){
            left = rand() % (width - 200);
        }
        if(height > 200){
            top = rand() & (height - 200);
        }
        FCRect rect = {left, top, left, 0};
        barrage->setRect(rect);
    }
    m_lock.lock();
    m_barrages.push_back(barrage);
    m_lock.unLock();
    m_tick++;
}

bool BarrageDiv::containsPoint(const FCPoint& point){
    return false;
}

void BarrageDiv::onLoad(){
    FCView::onLoad();
    startTimer(m_timerID, 1);
}

void BarrageDiv::onPaintBackground(FCPaint *paint, const FCRect& clipRect){
    FCView::onPaintBackground(paint, clipRect);
    m_lock.lock();
    vector<Barrage*>::iterator sIter = m_barrages.begin();
    for(; sIter != m_barrages.end(); ++sIter){
        Barrage *brg = *sIter;
        FCFont* font = brg->getFont();
        FCRect rect = brg->getRect();
        String str = brg->getText();
        FCSize size = paint->textSize(str.c_str(), font);
        rect.right = rect.left + size.cx;
        rect.bottom = rect.top + size.cy;
        brg->setRect(rect);
        Long color = brg->getColor();
        int mode = brg->getMode();
        if (mode == 1){
            int a = 0, r = 0, g = 0, b = 0;
            FCColor::toArgb(0, color, &a, &r, &g, &b);
            a = a * brg->getTick() / 400;
            color = FCColor::argb(a, r, g, b);
        }
        paint->drawText(str.c_str(), color, font, rect);
    }
    m_lock.unLock();
}

void BarrageDiv::onTimer(int timerID){
    FCView::onTimer(timerID);
    if (m_timerID == timerID){
        bool paint = false;
        m_lock.lock();
        int barragesSize = (int)m_barrages.size();
        if(barragesSize > 0){
            int width = getWidth(), height = getHeight();
            for(int i = 0; i < barragesSize; i++){
                Barrage *brg = m_barrages[i];
                int mode = brg->getMode();
                if(mode == 0){
                    if(brg->getRect().right < 0){
                        m_barrages.erase(m_barrages.begin() + i);
                        i--;
                        barragesSize--;
                    }
                    else{
                        brg->calculate();
                    }
                    paint = true;
                }
                else if(mode == 1){
                    int tick = brg->getTick();
                    tick--;
                    if(tick <= 0){
                        m_barrages.erase(m_barrages.begin() + i);
                        i--;
                        barragesSize--;
                        paint = true;
                    }
                    else{
                        brg->setTick(tick);
                    }
                    if(tick % 20 == 0){
                        paint = true;
                    }
                }
            }
        }
        m_lock.unLock();
        if(paint){
            invalidate();
        }
    }
}
