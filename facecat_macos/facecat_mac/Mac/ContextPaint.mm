#include "ContextPaint.h"
#include "FCDraw.h"

void ContextPaint::affectScaleFactor(FCRect *rect){
    if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
        rect->left = (int)(rect->left * m_scaleFactorX);
        rect->top = (int)(rect->top * m_scaleFactorY);
        rect->right = (int)(rect->right * m_scaleFactorX);
        rect->bottom = (int)(rect->bottom * m_scaleFactorY);
    }
}

CGPathRef ContextPaint::cQMPathCreateRoundingRect(CGRect rect, CGFloat blRadius, CGFloat brRadius, CGFloat trRadius, CGFloat tlRadius){
    CGMutablePathRef path = CGPathCreateMutable();
    if(blRadius == 0 && brRadius == 0 && trRadius == 0 && tlRadius == 0){
        CGPathAddRect(path, 0, rect);
    }
    else{
        CGPoint tlPoint = rect.origin;
        CGPoint brPoint = CGPointMake(rect.origin.x + rect.size.width, rect.origin.y + rect.size.height);
        CGPathMoveToPoint(path, 0, tlPoint.x + tlRadius, tlPoint.y);
        CGPathAddArcToPoint(path, 0, brPoint.x, tlPoint.y, brPoint.x, tlPoint.y + trRadius, trRadius);
        CGPathAddArcToPoint(path, 0, brPoint.x, brPoint.y, brPoint.x - brRadius, brPoint.y, brRadius);
        CGPathAddArcToPoint(path, 0, tlPoint.x, brPoint.y, tlPoint.x, brPoint.y - blRadius, blRadius);
        CGPathAddArcToPoint(path, 0, tlPoint.x, tlPoint.y, tlPoint.x + tlRadius, tlPoint.y, tlRadius);
        CGPathCloseSubpath(path);
    }
    return path;
}

NSColor* ContextPaint::getUIColor(Long dwPenColor){
    int a = 0, r = 0, g = 0, b = 0;
    FCColor::toArgb(this, dwPenColor, &a, &r, &g, &b);
    a = (int)(m_opacity * a);
    return [NSColor colorWithRed:r / 255.0 green:g / 255.0 blue:b / 255.0 alpha:a/255.0];
}

NSFont* ContextPaint::getUIFont(FCFont *font){
    String identifier = font->m_fontFamily + L"-" + FCStr::convertFloatToStr(font->m_fontSize);
    map<String, NSFont*>::iterator sIter = m_fonts.find(identifier);
    if(sIter != m_fonts.end()){
        return sIter->second;
    }
    else{
        string fstr = FCStr::wstringTostring(font->m_fontFamily);
        NSString *nsstr = [NSString stringWithUTF8String:fstr.c_str()];
        NSFont *uiFont = [NSFont fontWithName:nsstr size:font->m_fontSize];
        if(!uiFont)
        {
            uiFont = [NSFont fontWithName:@"Heiti SC" size:font->m_fontSize];
        }
        m_fonts[identifier] = uiFont;
        return uiFont;
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

CGPoint ContextPaint::getCGPoint(const FCPoint& point){
    return CGPointMake(point.x, point.y);
}

CGRect ContextPaint::getCGRect(const FCRect& rect){
    int rw = rect.right - rect.left;
    int rh = rect.bottom - rect.top;
    if(rw < 0){
        rw = 0;
    }
    if(rh < 0){
        rh = 0;
    }
    return CGRectMake(rect.left, rect.top, rw, rh);
}

CGSize ContextPaint::getCGSize(const FCSize& size){
    return CGSizeMake(size.cx, size.cy);
}

NSString* ContextPaint::getNSString(const wchar_t *str){
    string fstr = FCStr::wstringTostring(str);
    return [NSString stringWithUTF8String:fstr.c_str()];
}

FCPoint ContextPaint::getPoint(CGPoint cgPoint){
    FCPoint point = {(int)cgPoint.x, (int)cgPoint.y};
    return point;
}

FCRect ContextPaint::getRect(CGRect cgRect){
    FCRect rect = {(int)cgRect.origin.x, (int)cgRect.origin.y, (int)(cgRect.origin.x + cgRect.size.width),
        (int)(cgRect.origin.x + cgRect.size.height)};
    return rect;
}

FCSize ContextPaint::getSize(CGSize cgSize){
    FCSize size = {(int)cgSize.width, (int)cgSize.height};
    return size;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////

ContextPaint::ContextPaint(){
    m_clipRect.left = 0;
    m_clipRect.top = 0;
    m_clipRect.right = 0;
    m_clipRect.bottom = 0;
    m_endLineCap = 0;
    m_isClip = false;
    m_isPathStart = false;
    m_offsetX = 0;
    m_offsetY = 0;
    m_opacity = 1;
    m_pRect.left = 0;
    m_pRect.top = 0;
    m_pRect.right = 0;
    m_pRect.bottom = 0;
    m_resourcePath = L"";
    m_rotateAngle = 0;
    m_scaleFactorX = 1;
    m_scaleFactorY = 1;
    m_smoothMode = 0;
    m_startLineCap = 0;
    m_textQuality = 0;
    m_wHeight = 0;
    m_wRect.left = 0;
    m_wRect.top = 0;
    m_wRect.right = 0;
    m_wRect.bottom = 0;
}

ContextPaint::~ContextPaint(){
    clearCaches();
}

void ContextPaint::addArc(const FCRect& rect, float startAngle, float sweepAngle){
    int rw = rect.right - rect.left;
    if (rw < 1) rw = 1;
    int rh = rect.bottom - rect.top;
    if (rh < 1) rh = 1;
    int x = m_offsetX + rect.left + rw / 2;
    int y = m_offsetY + m_wHeight - (rect.top + rh / 2);
    int radius = rw / 2;
    if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
        x = (int)(m_scaleFactorX * x);
        y = (int)(m_scaleFactorY * y);
        radius = (int)(m_scaleFactorX * radius);
    }
    CGPathAddArc(m_path, 0, x, y, radius, startAngle * M_PI / 180, (sweepAngle + startAngle) * M_PI / 180, false);
    m_isPathStart = false;
}

//添加贝塞尔曲线
void ContextPaint::addBezier(FCPoint *apt, int cpt){
    if(cpt < 3) return;
    if(m_isPathStart){
        CGContextMoveToPoint(m_context, apt[0].x + m_offsetX, apt[0].y + m_offsetY);
        m_isPathStart = false;
    }
    for(int i = 1; i < cpt -2; i = i + 2){
        CGPathAddCurveToPoint(m_path, 0, apt[i].x + m_offsetX, m_wHeight - (apt[i].y + m_offsetY), apt[i+1].x + m_offsetX, m_wHeight - (apt[i + 1].y + m_offsetY), apt[i+2].x + m_offsetX, m_wHeight - (apt[i + 2].y + m_offsetY));
    }
}

void ContextPaint::addCurve(FCPoint *apt, int cpt){
}

void ContextPaint::addEllipse(const FCRect& rect){
    FCRect newRect = {rect.left + m_offsetX, rect.top + m_offsetY, rect.right + m_offsetX, rect.bottom + m_offsetY};
    affectScaleFactor(&newRect);
    CGRect cgRect = getCGRect(newRect);
    cgRect.origin.y = m_wHeight - cgRect.origin.y - cgRect.size.height;
    CGPathAddEllipseInRect(m_path, 0, getCGRect(newRect));
    m_isPathStart = false;
}

void ContextPaint::addLine(int x1, int y1, int x2, int y2){
    int lx1 = x1 + m_offsetX;
    int ly1 = y1 + m_offsetY;
    int lx2 = x2 + m_offsetX;
    int ly2 = y2 + m_offsetY;
    if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
        lx1 = (int)(m_scaleFactorX * lx1);
        ly1 = (int)(m_scaleFactorY * ly1);
        lx2 = (int)(m_scaleFactorX * lx2);
        ly2 = (int)(m_scaleFactorY * ly2);
    }
    if(m_isPathStart){
        CGPathMoveToPoint(m_path, 0, lx1, m_wHeight - ly1);
        m_isPathStart = false;
    }
    CGPathAddLineToPoint(m_path, 0, lx2, m_wHeight - ly2);
}

void ContextPaint::addRect(const FCRect& rect){
    FCRect newRect = {rect.left + m_offsetX, rect.top + m_offsetY, rect.right + m_offsetX, rect.bottom + m_offsetY};
    affectScaleFactor(&newRect);
    CGRect cgRect = getCGRect(newRect);
    cgRect.origin.y = m_wHeight - cgRect.origin.y - cgRect.size.height;
    CGPathAddRect(m_path, 0, cgRect);
    m_isPathStart = false;
}

void ContextPaint::addPie(const FCRect& rect, float startAngle, float sweepAngle){
    CGContextAddArc(m_context, (rect.left + rect.right)/2, m_wHeight - (rect.top + rect.bottom/  2),
                    (rect.right - rect.left)/2, startAngle, sweepAngle, 0);
}

void ContextPaint::addText(const wchar_t *strText, FCFont *font, const FCRect& rect){
    
}

void ContextPaint::beginExport(const String& exportPath, const FCRect& rect){
}

void ContextPaint::beginPaint(int hDC, const FCRect& wRect, const FCRect& pRect){
    m_isClip = false;
    m_opacity = 1;
    m_pRect = pRect;
    m_wRect = wRect;
    int width = m_wRect.right - m_wRect.left;
    int height = m_wRect.bottom - m_wRect.top;
    m_wHeight = height;
    m_resourcePath = L"";
    m_context = (CGContextRef)[[NSGraphicsContext currentContext] graphicsPort];
    setSmoothMode(m_smoothMode);
    setTextQuality(m_textQuality);
}

void ContextPaint::beginPath(){
    m_path = CGPathCreateMutable();
    m_isPathStart = true;
}

void ContextPaint::clearCaches(){
    m_fonts.clear();
    m_images.clear();
}

void ContextPaint::clipPath(){
    CGContextClip(m_context);
}

void ContextPaint::closeFigure(){
    CGPathCloseSubpath(m_path);
}

void ContextPaint::closePath(){
    CGPathRelease(m_path);
    m_isPathStart = false;
}

void ContextPaint::drawArc(Long dwPenColor, float width, int style, const FCRect& rect, float startAngle, float sweepAngle){
    if(dwPenColor == FCColor_None) return;
    dwPenColor = getPaintColor(dwPenColor);
    CGContextSetLineWidth(m_context, width);
    CGContextSetStrokeColorWithColor(m_context, getUIColor(dwPenColor).CGColor);
    int rw = rect.right - rect.left;
    if (rw < 1) rw = 1;
    int rh = rect.bottom - rect.top;
    if (rh < 1) rh = 1;
    int x = m_offsetX + rect.left + rw / 2;
    int y = m_offsetY + m_wHeight - (rect.top + rh / 2);
    int radius = rw / 2;
    if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
        x = (int)(m_scaleFactorX * x);
        y = (int)(m_scaleFactorY * y);
        radius = (int)(m_scaleFactorX * radius);
    }
    CGContextAddArc(m_context, x, y, radius, startAngle * M_PI / 180, (sweepAngle + startAngle) * M_PI / 180, false);
    switch (style) {
        case 0:{
            CGContextSetLineDash(m_context, 0, 0, 0);
            break;
        }
        case 1:{
            CGFloat lengths[] = {10, 10};
            CGContextSetLineDash(m_context, 0, lengths, 2);
            break;
        }
        case 2:{
            CGFloat lengths[] = {1, 1};
            CGContextSetLineDash(m_context, 0, lengths, 2);
            break;
        }
    }
    CGContextStrokePath(m_context);
}

void ContextPaint::drawBezier(Long dwPenColor, float width, int style, FCPoint *apt, int cpt){
    if(dwPenColor == FCColor_None) return;
    dwPenColor = getPaintColor(dwPenColor);
    if(cpt < 3) return;
    CGContextSetLineWidth(m_context, width);
    CGContextSetStrokeColorWithColor(m_context, getUIColor(dwPenColor).CGColor);
    CGContextMoveToPoint(m_context, apt[0].x + m_offsetX, m_wHeight - (apt[0].y + m_offsetY));
    for(int i = 1; i < cpt -2; i = i + 2){
        CGContextAddCurveToPoint(m_context, apt[i].x + m_offsetX, m_wHeight - (apt[i].y + m_offsetY), apt[i+1].x + m_offsetX, m_wHeight - (apt[i + 1].y + m_offsetY), apt[i+2].x + m_offsetX, m_wHeight - (apt[i + 2].y + m_offsetY));
    }
    switch (style) {
        case 0:{
            CGContextSetLineDash(m_context, 0, 0, 0);
            break;
        }
        case 1:{
            CGFloat lengths[] = {10, 10};
            CGContextSetLineDash(m_context, 0, lengths, 2);
            break;
        }
        case 2:{
            CGFloat lengths[] = {1, 1};
            CGContextSetLineDash(m_context, 0, lengths, 2);
            break;
        }
    }
    CGContextStrokePath(m_context);
}

void ContextPaint::drawCurve(Long dwPenColor, float width, int style, FCPoint *apt, int cpt){
}

void ContextPaint::drawEllipse(Long dwPenColor, float width, int style, const FCRect& rect){
    drawEllipse(dwPenColor, width, style, rect.left, rect.top, rect.right, rect.bottom);
}

void ContextPaint::drawEllipse(Long dwPenColor, float width, int style, int left, int top, int right, int bottom){
    if(dwPenColor == FCColor_None) return;
    dwPenColor = getPaintColor(dwPenColor);
    setSmoothMode(2);
    FCRect newRect = {left + m_offsetX, top + m_offsetY, right + m_offsetX, bottom + m_offsetY};
    affectScaleFactor(&newRect);
    CGContextSetLineWidth(m_context, width);
    CGContextSetStrokeColorWithColor(m_context, getUIColor(dwPenColor).CGColor);
    CGRect cgRect = getCGRect(newRect);
    cgRect.origin.y = m_wHeight - cgRect.origin.y - cgRect.size.height;
    CGContextAddEllipseInRect(m_context, cgRect);
    switch (style) {
        case 0:{
            CGContextSetLineDash(m_context, 0, 0, 0);
            break;
        }
        case 1:{
            CGFloat lengths[] = {10, 10};
            CGContextSetLineDash(m_context, 0, lengths, 2);
            break;
        }
        case 2:{
            CGFloat lengths[] = {1, 1};
            CGContextSetLineDash(m_context, 0, lengths, 2);
            break;
        }
    }
    CGContextStrokePath(m_context);
    setSmoothMode(0);
}

void ContextPaint::drawImage(const wchar_t *imagePath, const FCRect& rect){
    String imageKey = imagePath;
    NSImage *drawImage = 0;
    int rw = rect.right - rect.left;
    if(rw < 1) rw = 1;
    int rh = rect.bottom - rect.top;
    if(rh < 1) rh = 1;
    if (m_images.find(imageKey) != m_images.end()){
        drawImage = m_images[imageKey];
    }
    else{
        drawImage = [NSImage imageNamed:getNSString(imageKey.c_str())];
        if(drawImage){
            m_images[imageKey] = drawImage;
            drawImage = drawImage;
        }
    }
    if(drawImage){
        FCRect newRect = {rect.left + m_offsetX, rect.top + m_offsetY, rect.right + m_offsetX, rect.bottom + m_offsetY};
        affectScaleFactor(&newRect);
        CGRect cgRect = getCGRect(newRect);
        cgRect.origin.y = m_wHeight - cgRect.origin.y - cgRect.size.height;
        [drawImage drawInRect:cgRect];
    }
}

void ContextPaint::drawLine(Long dwPenColor, float width, int style, const FCPoint& x, const FCPoint& y){
    drawLine(dwPenColor, width, style, x.x, x.y, y.x, y.y);
}

void ContextPaint::drawLine(Long dwPenColor, float width, int style, int x1, int y1, int x2, int y2){
    if(dwPenColor == FCColor_None) return;
    dwPenColor = getPaintColor(dwPenColor);
    int lx1 = x1 + m_offsetX;
    int ly1 = y1 + m_offsetY;
    int lx2 = x2 + m_offsetX;
    int ly2 = y2 + m_offsetY;
    if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
        lx1 = (int)(m_scaleFactorX * lx1);
        ly1 = (int)(m_scaleFactorY * ly1);
        lx2 = (int)(m_scaleFactorX * lx2);
        ly2 = (int)(m_scaleFactorY * ly2);
    }
    CGContextSetLineWidth(m_context, width * 0.5);
    CGContextSetStrokeColorWithColor(m_context, getUIColor(dwPenColor).CGColor);
    CGContextMoveToPoint(m_context, lx1, m_wHeight - ly1);
    CGContextAddLineToPoint(m_context, lx2, m_wHeight - ly2);
    switch (style) {
        case 0:{
            CGContextSetLineDash(m_context, 0, 0, 0);
            break;
        }
        case 1:{
            CGFloat lengths[] = {10, 10};
            CGContextSetLineDash(m_context, 0, lengths, 2);
            break;
        }
        case 2:{
            CGFloat lengths[] = {1, 1};
            CGContextSetLineDash(m_context, 0, lengths, 2);
            break;
        }
    }
    CGContextStrokePath(m_context);
}

void ContextPaint::drawPath(Long dwPenColor, float width, int style){
    if(dwPenColor == FCColor_None) return;
    dwPenColor = getPaintColor(dwPenColor);
    CGContextSetLineWidth(m_context, width * 0.5);
    CGContextSetStrokeColorWithColor(m_context, getUIColor(dwPenColor).CGColor);
    switch (style) {
        case 0:{
            CGContextSetLineDash(m_context, 0, 0, 0);
            break;
        }
        case 1:{
            CGFloat lengths[] = {10, 10};
            CGContextSetLineDash(m_context, 0, lengths, 2);
            break;
        }
        case 2:{
            CGFloat lengths[] = {1, 1};
            CGContextSetLineDash(m_context, 0, lengths, 2);
            break;
        }
    }
    CGContextAddPath(m_context, m_path);
    CGContextDrawPath(m_context, kCGPathStroke);
}

//绘制扇形
void ContextPaint::drawPie(Long dwPenColor, float width, int style, const FCRect& rect, float startAngle, float sweepAngle){
    if(dwPenColor == FCColor_None) return;
    dwPenColor = getPaintColor(dwPenColor);
}

void ContextPaint::drawPolygon(Long dwPenColor, float width, int style, FCPoint *apt, int cpt){
    if(dwPenColor == FCColor_None) return;
    dwPenColor = getPaintColor(dwPenColor);
    CGContextSetLineWidth(m_context, width);
    CGContextSetStrokeColorWithColor(m_context, getUIColor(dwPenColor).CGColor);
    int fx = 0, fy = 0;
    for(int i = 0; i< cpt; i++){
        int x = apt[i].x + m_offsetX;
        int y = apt[i].y + m_offsetY;
        if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
            x = (int)(m_scaleFactorX * x);
            y = (int)(m_scaleFactorY * y);
        }
        if(i == 0){
            fx = x;
            fy = y;
            CGContextMoveToPoint(m_context, x, m_wHeight - y);
        }
        else{
            CGContextAddLineToPoint(m_context, x, m_wHeight - y);
        }
    }
    CGContextAddLineToPoint(m_context, fx, m_wHeight - fy);
    switch (style) {
        case 0:{
            CGContextSetLineDash(m_context, 0, 0, 0);
            break;
        }
        case 1:{
            CGFloat lengths[] = {10, 10};
            CGContextSetLineDash(m_context, 0, lengths, 2);
            break;
        }
        case 2:{
            CGFloat lengths[] = {1, 1};
            CGContextSetLineDash(m_context, 0, lengths, 2);
            break;
        }
    }
    CGContextStrokePath(m_context);
}

void ContextPaint::drawPolyline(Long dwPenColor, float width, int style, FCPoint *apt, int cpt){
    if(dwPenColor == FCColor_None) return;
    dwPenColor = getPaintColor(dwPenColor);
    CGContextSetLineWidth(m_context, width);
    CGContextSetStrokeColorWithColor(m_context, getUIColor(dwPenColor).CGColor);
    for(int i = 0; i< cpt; i++){
        int x = apt[i].x + m_offsetX;
        int y = apt[i].y + m_offsetY;
        if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
            x = (int)(m_scaleFactorX * x);
            y = (int)(m_scaleFactorY * y);
        }
        if(i == 0){
            CGContextMoveToPoint(m_context, x, m_wHeight - y);
        }
        else{
            CGContextAddLineToPoint(m_context, x, m_wHeight - y);
        }
    }
    switch (style) {
        case 0:{
            CGContextSetLineDash(m_context, 0, 0, 0);
            break;
        }
        case 1:{
            CGFloat lengths[] = {10, 10};
            CGContextSetLineDash(m_context, 0, lengths, 2);
            break;
        }
        case 2:{
            CGFloat lengths[] = {1, 1};
            CGContextSetLineDash(m_context, 0, lengths, 2);
            break;
        }
    }
    CGContextStrokePath(m_context);
}

void ContextPaint::drawRect(Long dwPenColor, float width, int style, int left, int top, int right, int bottom){
    if(bottom == top){
        drawLine(dwPenColor, 1, 0, left, top, right, top);
    }
    else if(left == right){
        drawLine(dwPenColor, 1, 0, left, top, left, bottom);
    }
	else{
	    if(dwPenColor == FCColor_None) return;
        dwPenColor = getPaintColor(dwPenColor);
        setSmoothMode(2);
        FCRect newRect = {left + m_offsetX, top + m_offsetY, right + m_offsetX, bottom + m_offsetY};
        affectScaleFactor(&newRect);
        CGContextSetLineWidth(m_context, width * 0.5);
        CGContextSetStrokeColorWithColor(m_context, getUIColor(dwPenColor).CGColor);
        CGRect cgRect = getCGRect(newRect);
        cgRect.origin.y = m_wHeight - cgRect.origin.y - cgRect.size.height;
        CGContextAddRect(m_context, cgRect);
        switch (style) {
            case 0:{
                CGContextSetLineDash(m_context, 0, 0, 0);
                break;
            }
            case 1:{
                CGFloat lengths[] = {10, 10};
                CGContextSetLineDash(m_context, 0, lengths, 2);
                break;
            }
            case 2:{
                CGFloat lengths[] = {1, 1};
                CGContextSetLineDash(m_context, 0, lengths, 2);
                break;
            }
        }
        CGContextStrokePath(m_context);
        setSmoothMode(0);
	}
}

void ContextPaint::drawRect(Long dwPenColor, float width, int style, const FCRect& rect){
    drawRect(dwPenColor, width, style, rect.left, rect.top, rect.right, rect.bottom);
}

void ContextPaint::drawRoundRect(Long dwPenColor, float width, int style, const FCRect& rect, int cornerRadius){
    if(cornerRadius == 0){
        drawRect(dwPenColor, width, style, rect);
    }
    else{
        if(dwPenColor == FCColor_None) return;
        dwPenColor = getPaintColor(dwPenColor);
        setSmoothMode(2);
        FCRect newRect = {rect.left + m_offsetX, rect.top + m_offsetY, rect.right + m_offsetX, rect.bottom + m_offsetY};
        affectScaleFactor(&newRect);
        CGContextSetLineWidth(m_context, width * 0.5);
        CGContextSetStrokeColorWithColor(m_context, getUIColor(dwPenColor).CGColor);
        CGRect cgRect = getCGRect(newRect);
        cgRect.origin.y = m_wHeight - cgRect.origin.y - cgRect.size.height;
        CGPathRef path = cQMPathCreateRoundingRect(cgRect, cornerRadius, cornerRadius, cornerRadius, cornerRadius);
        CGContextAddPath(m_context, path);
        switch (style) {
            case 0:{
                CGContextSetLineDash(m_context, 0, 0, 0);
                break;
            }
            case 1:{
                CGFloat lengths[] = {10, 10};
                CGContextSetLineDash(m_context, 0, lengths, 2);
                break;
            }
            case 2:{
                CGFloat lengths[] = {1, 1};
                CGContextSetLineDash(m_context, 0, lengths, 2);
                break;
            }
        }
        CGContextStrokePath(m_context);
        CGPathRelease(path);
        setSmoothMode(0);
    }
}

void ContextPaint::drawText(const wchar_t *strText, Long dwPenColor, FCFont *font, const FCRect& rect){
    if(dwPenColor == FCColor_None) return;
    dwPenColor = getPaintColor(dwPenColor);
    setSmoothMode(2);
    NSString *nsstr = getNSString(strText);
    if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
        int strX = (int)(m_scaleFactorX * (rect.left + m_offsetX));
        int strY = (int)(m_scaleFactorY * (rect.top + m_offsetY));
        int strBottom = (int)(m_scaleFactorY * (rect.bottom + m_offsetY));
        float fontSize = (float)(font->m_fontSize * (m_scaleFactorX + m_scaleFactorY) / 2);
        FCFont scaleFont(font->m_fontFamily, fontSize, font->m_bold, font->m_underline, font->m_italic);
        NSFont *nsFont = getUIFont(&scaleFont);
        CGContextSetFillColorWithColor(m_context, getUIColor(dwPenColor).CGColor);
        UniChar *characters;
        CGGlyph *glyphs;
        CFIndex count;
        CTFontRef ctFont = CTFontCreateWithName(CFSTR("Heiti SC"), scaleFont.m_fontSize, 0);
        CTFontDescriptorRef ctFontDesRef = CTFontCopyFontDescriptor(ctFont);
        CGFontRef cgFont = CTFontCopyGraphicsFont(ctFont, &ctFontDesRef );
        CGContextSetFont(m_context, cgFont);
        CFNumberRef pointSizeRef = (CFNumberRef)CTFontDescriptorCopyAttribute(ctFontDesRef,kCTFontSizeAttribute);
        CGContextSetFontSize(m_context, scaleFont.m_fontSize);
        count = CFStringGetLength((CFStringRef)nsstr);
        characters = (UniChar *)malloc(sizeof(UniChar) * count);
        glyphs = (CGGlyph *)malloc(sizeof(CGGlyph) * count);
        CFStringGetCharacters((CFStringRef)nsstr, CFRangeMake(0, count), characters);
        CTFontGetGlyphsForCharacters(ctFont, characters, glyphs, count);
        CGContextShowGlyphsAtPoint(m_context, strX, m_wHeight - (strBottom - strY) - strY, glyphs, nsstr.length);
        if(characters){
            free(characters);
        }
        if(glyphs){
            free(glyphs);
        }
        CFRelease(ctFont);
    }
    else{
        NSFont *nsFont = getUIFont(font);
        CGContextSetFillColorWithColor(m_context, getUIColor(dwPenColor).CGColor);
        UniChar *characters = 0;
        CGGlyph *glyphs = 0;
        CFIndex count = 0;
        CTFontRef ctFont = CTFontCreateWithName(CFSTR("Heiti SC"), font->m_fontSize, 0);
        CTFontDescriptorRef ctFontDesRef = CTFontCopyFontDescriptor(ctFont);
        CGFontRef cgFont = CTFontCopyGraphicsFont(ctFont, &ctFontDesRef);
        CGContextSetFont(m_context, cgFont);
        CFNumberRef pointSizeRef = (CFNumberRef)CTFontDescriptorCopyAttribute(ctFontDesRef,kCTFontSizeAttribute);
        CGContextSetFontSize(m_context, font->m_fontSize);
        count = CFStringGetLength((CFStringRef)nsstr);
        characters = (UniChar *)malloc(sizeof(UniChar) * count);
        glyphs = (CGGlyph *)malloc(sizeof(CGGlyph) * count);
        CFStringGetCharacters((CFStringRef)nsstr, CFRangeMake(0, count), characters);
        CTFontGetGlyphsForCharacters(ctFont, characters, glyphs, count);
        int tSize = rect.bottom - rect.top;
        CGContextShowGlyphsAtPoint(m_context, rect.left + m_offsetX, m_wHeight - tSize - (rect.top + m_offsetY) + 2, glyphs, nsstr.length);
        CFRelease(ctFont);
        if(characters){
            free(characters);
        }
        if(glyphs){
            free(glyphs);
        }
    }
    setSmoothMode(0);
}

void ContextPaint::drawText(const wchar_t *strText, Long dwPenColor, FCFont *font, const FCRectF& rect){
    FCRect rc = {(int)rect.left, (int)rect.top, (int)rect.right, (int)rect.bottom};
    drawText(strText, dwPenColor, font, rc);
}

void ContextPaint::drawTextAutoEllipsis(const wchar_t *strText, Long dwPenColor, FCFont *font, const FCRect& rect){
    drawText(strText, dwPenColor, font, rect);
}

void ContextPaint::endExport(){
}

void ContextPaint::endPaint(){
    int left = m_pRect.left;
    int top = m_pRect.top;
    int width = m_pRect.right - m_pRect.left;
    int height = m_pRect.bottom - m_pRect.top;
    if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
        left = (int)(m_scaleFactorX * left);
        top = (int)(m_scaleFactorY * top);
        width = (int)(m_scaleFactorX * width);
        height = (int)(m_scaleFactorY * height);
    }
    if(m_isClip){
        CGContextRestoreGState(m_context);
        m_isClip = false;
    }
    m_context = 0;
    //BitBlt(m_wndHDC, left, top, width, height, m_hDC, left, top, SRCCOPY);
    m_offsetX = 0;
    m_offsetY = 0;
    m_resourcePath = L"";
}

void ContextPaint::excludeClipPath(){
    
}

void ContextPaint::fillEllipse(Long dwPenColor, const FCRect& rect){
    if(dwPenColor == FCColor_None) return;
    dwPenColor = getPaintColor(dwPenColor);
    setSmoothMode(2);
    FCRect newRect = {rect.left + m_offsetX, rect.top + m_offsetY, rect.right + m_offsetX, rect.bottom + m_offsetY};
    affectScaleFactor(&newRect);
    CGContextSetFillColorWithColor(m_context, getUIColor(dwPenColor).CGColor);
    CGRect cgRect = getCGRect(newRect);
    cgRect.origin.y = m_wHeight - cgRect.origin.y - cgRect.size.height;
    CGContextFillEllipseInRect(m_context, cgRect);
    setSmoothMode(0);
}

void ContextPaint::fillGradientEllipse(Long dwFirst, Long dwSecond, const FCRect& rect, int angle){
    if(dwFirst == FCColor_None) return;
    if(dwSecond == FCColor_None) return;
    dwFirst = getPaintColor(dwFirst);
    dwSecond = getPaintColor(dwSecond);
    setSmoothMode(2);
    FCRect newRect = {rect.left + m_offsetX, rect.top + m_offsetY, rect.right + m_offsetX, rect.bottom + m_offsetY};
    affectScaleFactor(&newRect);
    CGRect cgRect = getCGRect(newRect);
    cgRect.origin.y = m_wHeight - cgRect.origin.y - cgRect.size.height;
    CGContextSaveGState(m_context);
    CGColorSpaceRef colorSpace = CGColorSpaceCreateDeviceRGB();
    CGFloat locations[] = { 0.0, 1.0 };
    NSArray *colors = [NSArray arrayWithObjects:(__bridge id)getUIColor(dwFirst).CGColor, (__bridge id)getUIColor(dwSecond).CGColor, nil];
    CGGradientRef gradient = CGGradientCreateWithColors(colorSpace,
                                                        (CFArrayRef) colors, locations);
    CGPoint startPoint = CGPointMake(CGRectGetMidX(cgRect), CGRectGetMinY(cgRect));
    CGPoint endPoint = CGPointMake(CGRectGetMidX(cgRect), CGRectGetMaxY(cgRect));
    CGContextAddEllipseInRect(m_context, cgRect);
    CGContextDrawLinearGradient(m_context, gradient, startPoint, endPoint, 0);
    CGContextRestoreGState(m_context);
    CGGradientRelease(gradient);
    CGColorSpaceRelease(colorSpace);
    setSmoothMode(0);
}

void ContextPaint::fillGradientPath(Long dwFirst, Long dwSecond, const FCRect& rect, int angle){
    if(dwFirst == FCColor_None) return;
    if(dwSecond == FCColor_None) return;
    dwFirst = getPaintColor(dwFirst);
    dwSecond = getPaintColor(dwSecond);
    FCRect newRect = {rect.left + m_offsetX, rect.top + m_offsetY, rect.right + m_offsetX, rect.bottom + m_offsetY};
    affectScaleFactor(&newRect);
    CGRect cgRect = getCGRect(newRect);
    cgRect.origin.y = m_wHeight - cgRect.origin.y - cgRect.size.height;
    CGContextSaveGState(m_context);
    CGColorSpaceRef colorSpace = CGColorSpaceCreateDeviceRGB();
    CGFloat locations[] = { 0.0, 1.0 };
    NSArray *colors = [NSArray arrayWithObjects:(__bridge id)getUIColor(dwFirst).CGColor, (__bridge id)getUIColor(dwSecond).CGColor, nil];
    CGGradientRef gradient = CGGradientCreateWithColors(colorSpace,
                                                        (CFArrayRef) colors, locations);
    CGPoint startPoint = CGPointMake(CGRectGetMidX(cgRect), CGRectGetMinY(cgRect));
    CGPoint endPoint = CGPointMake(CGRectGetMidX(cgRect), CGRectGetMaxY(cgRect));
    CGPathRef path = cQMPathCreateRoundingRect(cgRect, 0, 0, 0, 0);
    CGContextAddPath(m_context, path);
    CGContextClip(m_context);
    CGContextDrawLinearGradient(m_context, gradient, startPoint, endPoint, 0);
    CGContextRestoreGState(m_context);
    CGGradientRelease(gradient);
    CGColorSpaceRelease(colorSpace);
    CGPathRelease(path);
}

void ContextPaint::fillGradientPolygon(Long dwFirst, Long dwSecond, FCPoint *apt, int cpt, int angle){
    if(dwFirst == FCColor_None) return;
    if(dwSecond == FCColor_None) return;
    dwFirst = getPaintColor(dwFirst);
    dwSecond = getPaintColor(dwSecond);
    CGContextSaveGState(m_context);
    int left = 0, top = 0,  right =0, bottom = 0;
    int fx = 0, fy = 0;
    for(int i = 0; i< cpt; i++){
        int x = apt[i].x + m_offsetX;
        int y = apt[i].y + m_offsetY;
        if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
            x = (int)(m_scaleFactorX * x);
            y = (int)(m_scaleFactorY * y);
        }
        if(i == 0){
            fx = x;
            fy = y;
            left = x;
            top = y;
            right =x;
            bottom = y;
            CGContextMoveToPoint(m_context, x, m_wHeight - y);
        }
        if(left > x){
            left = x;
        }
        if (top > y){
            top = y;
        }
        if (right < x){
            right = x;
        }
        if (bottom < y){
            bottom = y;
        }
        CGContextAddLineToPoint(m_context, x, m_wHeight - y);
    }
    CGContextAddLineToPoint(m_context, fx, m_wHeight - fy);
    CGContextClip(m_context);
    CGColorSpaceRef colorSpace = CGColorSpaceCreateDeviceRGB();
    NSArray *colors = [NSArray arrayWithObjects:(__bridge id)getUIColor(dwFirst).CGColor, (__bridge id)getUIColor(dwSecond).CGColor, nil];
    CGGradientRef gradient = CGGradientCreateWithColors(colorSpace, (CFArrayRef) colors, 0);
    CGPoint startPoint = CGPointMake(left, m_wHeight - top);
    CGPoint endPoint = CGPointMake(right, m_wHeight - bottom);
    CGContextDrawLinearGradient(m_context, gradient, startPoint, endPoint, 0);
    CGContextRestoreGState(m_context);
    CGGradientRelease(gradient);
    CGColorSpaceRelease(colorSpace);
}

void ContextPaint::fillGradientRect(Long dwFirst, Long dwSecond, const FCRect& rect, int cornerRadius, int angle){
    if(dwFirst == FCColor_None) return;
    if(dwSecond == FCColor_None) return;
    dwFirst = getPaintColor(dwFirst);
    dwSecond = getPaintColor(dwSecond);
    CGContextSaveGState(m_context);
    if(cornerRadius != 0){
        setSmoothMode(2);
    }
    FCRect newRect = {rect.left + m_offsetX, rect.top + m_offsetY, rect.right + m_offsetX, rect.bottom + m_offsetY};
    affectScaleFactor(&newRect);
    CGRect cgRect = getCGRect(newRect);
    cgRect.origin.y = m_wHeight - cgRect.origin.y - cgRect.size.height;
    CGColorSpaceRef colorSpace = CGColorSpaceCreateDeviceRGB();
    CGFloat locations[] = { 0.0, 1.0 };
    NSArray *colors = [NSArray arrayWithObjects:(__bridge id)getUIColor(dwFirst).CGColor, (__bridge id)getUIColor(dwSecond).CGColor, nil];
    CGGradientRef gradient = CGGradientCreateWithColors(colorSpace,
                                                        (CFArrayRef) colors, locations);
    CGPoint startPoint = CGPointMake(CGRectGetMidX(cgRect), CGRectGetMinY(cgRect));
    CGPoint endPoint = CGPointMake(CGRectGetMidX(cgRect), CGRectGetMaxY(cgRect));
    CGPathRef path = cQMPathCreateRoundingRect(cgRect, cornerRadius, cornerRadius, cornerRadius, cornerRadius);
    CGContextAddPath(m_context, path);
    CGContextClip(m_context);
    CGContextDrawLinearGradient(m_context, gradient, startPoint, endPoint, 0);
    CGGradientRelease(gradient);
    CGColorSpaceRelease(colorSpace);
    CGPathRelease(path);
    setSmoothMode(0);
    CGContextRestoreGState(m_context);
}

void ContextPaint::fillPath(Long dwPenColor){
    if(dwPenColor == FCColor_None) return;
    dwPenColor = getPaintColor(dwPenColor);
    CGContextSetFillColorWithColor(m_context, getUIColor(dwPenColor).CGColor);
    CGContextAddPath(m_context, m_path);
    CGContextFillPath(m_context);
}

void ContextPaint::fillPie(Long dwPenColor, const FCRect& rect, float startAngle, float sweepAngle){
    if(dwPenColor == FCColor_None) return;
    dwPenColor = getPaintColor(dwPenColor);
    CGContextSetFillColorWithColor(m_context, getUIColor(dwPenColor).CGColor);
    CGContextSetStrokeColorWithColor(m_context, getUIColor(dwPenColor).CGColor);
    CGContextMoveToPoint(m_context, (rect.left + rect.right)/2, m_wHeight - (rect.top + rect.bottom / 2));
    CGContextAddArc(m_context, (rect.left + rect.right)/2, m_wHeight - (rect.top + rect.bottom / 2),
                    (rect.right - rect.left)/2, startAngle, sweepAngle, 0);
    CGContextClosePath(m_context);
    CGContextDrawPath(m_context, kCGPathFill);
}

void ContextPaint::fillPolygon(Long dwPenColor, FCPoint *apt, int cpt){
    if(dwPenColor == FCColor_None) return;
    dwPenColor = getPaintColor(dwPenColor);
    CGContextSetFillColorWithColor(m_context, getUIColor(dwPenColor).CGColor);
    for(int i = 0; i< cpt; i++){
        int x = apt[i].x + m_offsetX;
        int y = apt[i].y + m_offsetY;
        if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
            x = (int)(m_scaleFactorX * x);
            y = (int)(m_scaleFactorY * y);
        }
        if(i == 0){
            CGContextMoveToPoint(m_context, x, m_wHeight - y);
        }
        else{
            CGContextAddLineToPoint(m_context, x, m_wHeight - y);
        }
    }
    CGContextFillPath(m_context);
}

void ContextPaint::fillRect(Long dwPenColor, const FCRect& rect){
    if(rect.bottom == rect.top){
        drawLine(dwPenColor, 1, 0, rect.left, rect.top, rect.right, rect.top);
    }
    else if(rect.left == rect.right){
        drawLine(dwPenColor, 1, 0, rect.left, rect.top, rect.left, rect.bottom);
    }
	else{
	    if(dwPenColor == FCColor_None) return;
        dwPenColor = getPaintColor(dwPenColor);
        FCRect newRect = {rect.left + m_offsetX, rect.top + m_offsetY, rect.right + m_offsetX, rect.bottom + m_offsetY};
        affectScaleFactor(&newRect);
        CGContextSetFillColorWithColor(m_context, getUIColor(dwPenColor).CGColor);
        CGRect cgRect = getCGRect(newRect);
        cgRect.origin.y = m_wHeight - cgRect.origin.y - cgRect.size.height;
        CGContextFillRect(m_context, cgRect);
	}
}

void ContextPaint::fillRect(Long dwPenColor, int left, int top, int right, int bottom){
    FCRect newRect = {left, top, right, bottom};
    fillRect(dwPenColor, newRect);
}

void ContextPaint::fillRoundRect(Long dwPenColor, const FCRect& rect, int cornerRadius){
    if(cornerRadius == 0)
    {
        fillRect(dwPenColor, rect);
    }
    else
    {
        if(dwPenColor == FCColor_None) return;
        dwPenColor = getPaintColor(dwPenColor);
        FCRect newRect = {rect.left + m_offsetX, rect.top + m_offsetY, rect.right + m_offsetX, rect.bottom + m_offsetY};
        affectScaleFactor(&newRect);
        CGContextSetFillColorWithColor(m_context, getUIColor(dwPenColor).CGColor);
        CGRect cgRect = getCGRect(newRect);
        cgRect.origin.y = m_wHeight - cgRect.origin.y - cgRect.size.height;
        CGPathRef path = cQMPathCreateRoundingRect(cgRect, cornerRadius, cornerRadius, cornerRadius, cornerRadius);
        CGContextAddPath(m_context, path);
        CGContextFillPath(m_context);
        CGPathRelease(path);
    }
}

Long ContextPaint::getColor(Long dwPenColor){
    if (dwPenColor < FCColor_None){
        if (dwPenColor == FCColor_Back){
            dwPenColor = 16777215;
        }
        else if (dwPenColor == FCColor_Border){
            dwPenColor = 3289650;
        }
        else if (dwPenColor == FCColor_Text){
            dwPenColor = 0;
        }
        else if (dwPenColor == FCColor_DisabledBack){
            dwPenColor = 13158600;
        }
        else if (dwPenColor == FCColor_DisabledText){
            dwPenColor = 3289650;
        }
        else if(dwPenColor == FCColor_Hovered){
            dwPenColor = 13158600;
        }
        else if(dwPenColor == FCColor_Pushed){
            dwPenColor = 9868950;
        }
    }
    return dwPenColor;
}

Long ContextPaint::getPaintColor(Long dwPenColor){
    return FCDraw::getWhiteColor(dwPenColor);
}

FCPoint ContextPaint::getOffset(){
    FCPoint offset = {m_offsetX, m_offsetY};
    return offset;
}

FCPoint ContextPaint::rotate(const FCPoint& op, const FCPoint& mp, int angle){
    float PI = 3.14159265f;
    FCPoint pt = {0};
    pt.x = (int)((mp.x - op.x) * cos(angle * PI / 180) - (mp.y - op.y) * sin(angle * PI / 180) + op.x);
    pt.y = (int)((mp.x - op.x) * sin(angle * PI / 180) + (mp.y - op.y) * cos(angle * PI / 180) + op.y);
    return pt;
}

void ContextPaint::setClip(const FCRect& rect){
    if(m_isClip){
        CGContextRestoreGState(m_context);
    }
    FCRect newRect = {rect.left + m_offsetX, rect.top + m_offsetY, rect.right + m_offsetX, rect.bottom + m_offsetY};
    if (m_scaleFactorX != 1 || m_scaleFactorY != 1){
        newRect.left = (int)floor(newRect.left * m_scaleFactorX);
        newRect.top = (int)floor(newRect.top * m_scaleFactorY);
        newRect.right = (int)ceil(newRect.right * m_scaleFactorX);
        newRect.bottom = (int)ceil(newRect.bottom * m_scaleFactorY);
    }
    m_clipRect = newRect;
    CGContextSaveGState(m_context);
    CGRect cgRect = getCGRect(m_clipRect);
    cgRect.origin.y = m_wHeight - cgRect.origin.y - cgRect.size.height;
    CGContextClipToRect(m_context, cgRect);
    m_isClip = true;
}

void ContextPaint::setLineCap(int startLineCap, int endLineCap){
    m_startLineCap = startLineCap;
    if(m_startLineCap == 0){
        CGContextSetLineCap(m_context, kCGLineCapButt);
    }
    else if(m_startLineCap == 1){
        CGContextSetLineCap(m_context, kCGLineCapRound);
    }
    else if(m_startLineCap == 2){
        CGContextSetLineCap(m_context, kCGLineCapSquare);
    }
    m_endLineCap = endLineCap;
    if(m_endLineCap == 0){
        CGContextSetLineCap(m_context, kCGLineCapButt);
    }
    else if(m_endLineCap == 1){
        CGContextSetLineCap(m_context, kCGLineCapRound);
    }
    else if(m_endLineCap == 2){
        CGContextSetLineCap(m_context, kCGLineCapSquare);
    }
}

void ContextPaint::setOffset(const FCPoint& offset){
    m_offsetX = offset.x;
    m_offsetY = offset.y;
}

void ContextPaint::setOpacity(float opacity){
    m_opacity = opacity;
}

void ContextPaint::setResourcePath(const String& resourcePath){
    m_resourcePath = resourcePath;
}

void ContextPaint::setRotateAngle(int rotateAngle){
    m_rotateAngle = rotateAngle;
}

void ContextPaint::setScaleFactor(double scaleFactorX, double scaleFactorY){
    m_scaleFactorX = scaleFactorX;
    m_scaleFactorY = scaleFactorY;
}

void ContextPaint::setSmoothMode(int smoothMode){
    m_smoothMode = smoothMode;
    if(m_smoothMode > 0){
        CGContextSetShouldAntialias(m_context, true);
    }
    else{
        CGContextSetShouldAntialias(m_context, false);
    }
}

void ContextPaint::setTextQuality(int textQuality){
    m_textQuality = textQuality;
}

bool ContextPaint::supportTransparent(){
    return true;
}

FCSize ContextPaint::textSize(const wchar_t *strText, FCFont *font){
    NSFont *uiFont = getUIFont(font);
    NSString *nsstr = getNSString(strText);
    CGSize cgSize = [nsstr boundingRectWithSize:CGSizeMake(100000, 0) options:NSStringDrawingUsesLineFragmentOrigin attributes:@{NSFontAttributeName: uiFont} context:nil].size;
    FCSize size = { (int)cgSize.width, (int)cgSize.height };
    return size;
}

FCSizeF ContextPaint::textSizeF(const wchar_t *strText, FCFont *font){
    FCSize tSize = textSize(strText, font);
    FCSizeF size = { (float)tSize.cx, (float)tSize.cy };
    return size;
}
