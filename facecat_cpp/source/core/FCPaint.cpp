#include "..\\..\\stdafx.h"
#include "..\\..\\include\\core\\FCPaint.h"

namespace FaceCat{
	Long FCColor::argb(int r, int g, int b){
		return ((r | (g << 8)) | (b << 0x10));

	}

	Long FCColor::argb(int a, int r, int g, int b){
		if(a == 255){
			return ((r | (g << 8)) | (b << 0x10));
		}
		else if(a == 0){
			return FCColor_None;
		}
		else{
			int rgb = ((r | (g << 8)) | (b << 0x10));
			Long argb = -((Long)rgb * 1000 + a);
			return argb;
		}
	}

	void FCColor::toArgb(FCPaint *paint, Long dwPenColor, int *a, int *r, int *g, int *b){
		if(paint){
			dwPenColor = paint->getColor(dwPenColor);
		}
		*a = 255;
		if(dwPenColor < 0){
			dwPenColor = -dwPenColor;
			if(dwPenColor < 1){
				*a = 255;
			}
			else{
				*a = (int)(dwPenColor - dwPenColor / 1000 * 1000);
			}
			dwPenColor /= 1000;
		}
		*r = (int)(dwPenColor & 0xff);
		*g = (int)((dwPenColor >> 8) & 0xff);
		*b = (int)((dwPenColor >> 0x10) & 0xff);
	}

	Long FCColor::ratioColor(FCPaint *paint, Long originalColor, double ratio){
		int a = 0, r = 0, g = 0, b = 0;
        toArgb(paint, originalColor, &a, &r, &g, &b);
        r = (int)(r * ratio);
        g = (int)(g * ratio);
        b = (int)(b * ratio);
        if (r > 255) r = 255;
        if (g > 255) g = 255;
        if (b > 255) b = 255;
        return argb(a, r, g, b);
	}

	Long FCColor::reverse(FCPaint *paint, Long originalColor){
		int a = 0, r = 0, g = 0, b = 0;
		toArgb(paint, originalColor, &a, &r, &g, &b);
		return argb(a, 255 - r, 255 - g, 255 - b);
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	FCPaint::FCPaint(){
	}

	FCPaint::~FCPaint(){
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void FCPaint::addArc(const FCRect& rect, float startAngle, float sweepAngle){
	}
	
	void FCPaint::addBezier(FCPoint *apt, int cpt){
	}
	
	void FCPaint::addCurve(FCPoint *apt, int cpt){
	}
	
	void FCPaint::addEllipse(const FCRect& rect){
	}
	
	void FCPaint::addLine(int x1, int y1, int x2, int y2){
	}
	
	void FCPaint::addRect(const FCRect& rect){
	}
	
	void FCPaint::addPie(const FCRect& rect, float startAngle, float sweepAngle){
	}
	
	void FCPaint::addText(const wchar_t *strText, FCFont *font, const FCRect& rect){
	}

	void FCPaint::beginExport(const String& exportPath, const FCRect& rect){
	}
    
	void FCPaint::beginPaint(HDC hDC, const FCRect& wRect, const FCRect& pRect){
        
	}
	
	void FCPaint::beginPath(){
	}
    
	void FCPaint::clearCaches(){
	}

	void FCPaint::clipPath(){
	}
	
	void FCPaint::closeFigure(){
	}
	
	void FCPaint::closePath(){
	}
	
	void FCPaint::drawArc(Long dwPenColor, float width, int style, const FCRect& rect, float startAngle, float sweepAngle){
	}
	
	void FCPaint::drawBezier(Long dwPenColor, float width, int style, FCPoint *apt, int cpt){
	}
	
	void FCPaint::drawCurve(Long dwPenColor, float width, int style, FCPoint *apt, int cpt){
	}
    
	void FCPaint::drawEllipse(Long dwPenColor, float width, int style, const FCRect& rect){
	}
    
	void FCPaint::drawEllipse(Long dwPenColor, float width, int style, int left, int top, int right, int bottom){
	}
    
	void FCPaint::drawImage(const wchar_t *imagePath, const FCRect& rect){
	}
    
	void FCPaint::drawLine(Long dwPenColor, float width, int style, const FCPoint& x, const FCPoint& y){
	}
    
	void FCPaint::drawLine(Long dwPenColor, float width, int style, int x1, int y1, int x2, int y2){
	}
	
	void FCPaint::drawPath(Long dwPenColor, float width, int style){
	}
	
	void FCPaint::drawPie(Long dwPenColor, float width, int style, const FCRect& rect, float startAngle, float sweepAngle){
	}
    
	void FCPaint::drawPolygon(Long dwPenColor, float width, int style, FCPoint *apt, int cpt){
	}
    
	void FCPaint::drawPolyline(Long dwPenColor, float width, int style, FCPoint *apt, int cpt){
	}
    
	void FCPaint::drawRect(Long dwPenColor, float width, int style, int left, int top, int right, int bottom){
	}
    
	void FCPaint::drawRect(Long dwPenColor, float width, int style, const FCRect& rect){
	}
    
	void FCPaint::drawRoundRect(Long dwPenColor, float width, int style, const FCRect& rect, int cornerRadius){
	}
    
	void FCPaint::drawText(const wchar_t *strText, Long dwPenColor, FCFont *font, const FCRect& rect){
	}
    
	void FCPaint::drawText(const wchar_t *strText, Long dwPenColor, FCFont *font, const FCRectF& rect){
	}
    
	void FCPaint::drawTextAutoEllipsis(const wchar_t *strText, Long dwPenColor, FCFont *font, const FCRect& rect){
	}

	void FCPaint::endExport(){
	}
    
	void FCPaint::endPaint(){
	}

	void FCPaint::excludeClipPath(){
	}
    
	void FCPaint::fillEllipse(Long dwPenColor, const FCRect& rect){
	}
	
	void FCPaint::fillGradientEllipse(Long dwFirst, Long dwSecond, const FCRect& rect, int angle){
	}

	void FCPaint::fillGradientPath(Long dwFirst, Long dwSecond, const FCRect& rect, int angle){
	}

	void FCPaint::fillGradientPolygon(Long dwFirst, Long dwSecond, FCPoint *apt, int cpt, int angle){
	}
    
	void FCPaint::fillGradientRect(Long dwFirst, Long dwSecond, const FCRect& rect, int cornerRadius, int angle){
	}
	
	void FCPaint::fillPath(Long dwPenColor){
	}
	
	void FCPaint::fillPie(Long dwPenColor, const FCRect& rect, float startAngle, float sweepAngle){
	}
    
	void FCPaint::fillPolygon(Long dwPenColor, FCPoint *apt, int cpt){
	}
    
	void FCPaint::fillRect(Long dwPenColor, const FCRect& rect){
	}
    
	void FCPaint::fillRect(Long dwPenColor, int left, int top, int right, int bottom){
	}
    
	void FCPaint::fillRoundRect(Long dwPenColor, const FCRect& rect, int cornerRadius){
	}
    
	Long FCPaint::getColor(Long dwPenColor){
		return 0;
	}
    
	Long FCPaint::getPaintColor(Long dwPenColor){
		return 0;
	}
    
	FCPoint FCPaint::getOffset(){
		FCPoint offset = {0};
		return offset;
	}
    
    FCPoint FCPaint::rotate(const FCPoint& op, const FCPoint& mp, int angle){
		FCPoint pt = {0};
		return pt;
	}
    
	void FCPaint::setClip(const FCRect& rect){
	}
	
	void FCPaint::setLineCap(int startLineCap, int endLineCap){
	}
    
	void FCPaint::setOffset(const FCPoint& offset){
        
	}
    
	void FCPaint::setOpacity(float opacity){
	}
    
	void FCPaint::setResourcePath(const String& resourcePath){
	}
    
    void FCPaint::setRotateAngle(int rotateAngle){
        
    }
    
	void FCPaint::setScaleFactor(double scaleFactorX, double scaleFactorY){
	}

	void FCPaint::setSmoothMode(int smoothMode){
	}

	void FCPaint::setTextQuality(int textQuality){
	}
    
	bool FCPaint::supportTransparent(){
		return false;
	}
    
	FCSize FCPaint::textSize(const wchar_t *strText, FCFont *font){
		FCSize size = {0};
		return size;
	}
    
	FCSizeF FCPaint::textSizeF(const wchar_t *strText, FCFont *font){
		FCSizeF size = {0};
		return size;
	}
}