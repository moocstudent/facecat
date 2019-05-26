/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __CONTEXTPAINT_H__
#define __CONTEXTPAINT_H__
#pragma once
#include "stdafx.h"
#include "FCPaint.h"
#include "FCStr.h"
#import <UIKit/UIKit.h>
using namespace FaceCat;

class ContextPaint: public FCPaint{
protected:
    FCRect m_clipRect;
    CGContextRef m_context;
    int m_endLineCap;
    map<String, UIFont*> m_fonts;
    map<String, UIImage*> m_images;
    bool m_isClip;
    bool m_isPathStart;
    int m_offsetX;
    int m_offsetY;
    float m_opacity;
    CGMutablePathRef m_path;
    FCRect m_pRect;
    String m_resourcePath;
    int m_rotateAngle;
    double m_scaleFactorX;
    double m_scaleFactorY;
    int m_smoothMode;
    int m_startLineCap;
    int m_textQuality;
    FCRect m_wRect;
    void affectScaleFactor(FCRect *rect);
    CGPathRef cQMPathCreateRoundingRect(CGRect rect, CGFloat blRadius, CGFloat brRadius, CGFloat trRadius, CGFloat tlRadius);
    UIColor* getUIColor(Long dwPenColor);
    UIFont* getUIFont(FCFont *font);
public:
    static CGPoint getCGPoint(const FCPoint& point);
    static CGRect getCGRect(const FCRect& rect);
    static CGSize getCGSize(const FCSize& size);
    static NSString* getNSString(const wchar_t *str);
    static FCPoint getPoint(CGPoint cgPoint);
    static FCRect getRect(CGRect cgRect);
    static FCSize getSize(CGSize cgSize);
public:
    ContextPaint();
    virtual ~ContextPaint();
    virtual void addArc(const FCRect& rect, float startAngle, float sweepAngle);
    virtual void addBezier(FCPoint *apt, int cpt);
    virtual void addCurve(FCPoint *apt, int cpt);
    virtual void addEllipse(const FCRect& rect);
    virtual void addLine(int x1, int y1, int x2, int y2);
    virtual void addRect(const FCRect& rect);
    virtual void addPie(const FCRect& rect, float startAngle, float sweepAngle);
    virtual void addText(const wchar_t *strText, FCFont *font, const FCRect& rect);
    virtual void beginExport(const String& exportPath, const FCRect& rect);
    virtual void beginPaint(int hDC, const FCRect& wRect, const FCRect& pRect);
    virtual void beginPath();
    virtual void clearCaches();
    virtual void clipPath();
    virtual void closeFigure();
    virtual void closePath();
    virtual void drawArc(Long dwPenColor, float width, int style, const FCRect& rect, float startAngle, float sweepAngle);
    virtual void drawBezier(Long dwPenColor, float width, int style, FCPoint *apt, int cpt);
    virtual void drawCurve(Long dwPenColor, float width, int style, FCPoint *apt, int cpt);
    virtual void drawEllipse(Long dwPenColor, float width, int style, const FCRect& rect);
    virtual void drawEllipse(Long dwPenColor, float width, int style, int left, int top, int right, int bottom);
    virtual void drawImage(const wchar_t *imagePath, const FCRect& rect);
    virtual void drawLine(Long dwPenColor, float width, int style, const FCPoint& x, const FCPoint& y);
    virtual void drawLine(Long dwPenColor, float width, int style, int x1, int y1, int x2, int y2);
    virtual void drawPath(Long dwPenColor, float width, int style);
    virtual void drawPie(Long dwPenColor, float width, int style, const FCRect& rect, float startAngle, float sweepAngle);
    virtual void drawPolygon(Long dwPenColor, float width, int style, FCPoint *apt, int cpt);
    virtual void drawPolyline(Long dwPenColor, float width, int style, FCPoint *apt, int cpt);
    virtual void drawRect(Long dwPenColor, float width, int style, int left, int top, int right, int bottom);
    virtual void drawRect(Long dwPenColor, float width, int style, const FCRect& rect);
    virtual void drawRoundRect(Long dwPenColor, float width, int style, const FCRect& rect, int cornerRadius);
    virtual void drawText(const wchar_t *strText, Long dwPenColor, FCFont *font, const FCRect& rect);
    virtual void drawText(const wchar_t *strText, Long dwPenColor, FCFont *font, const FCRectF& rect);
    virtual void drawTextAutoEllipsis(const wchar_t *strText, Long dwPenColor, FCFont *font, const FCRect& rect);
    virtual void endExport();
    virtual void endPaint();
    virtual void excludeClipPath();
    virtual void fillEllipse(Long dwPenColor, const FCRect& rect);
    virtual void fillGradientEllipse(Long dwFirst, Long dwSecond, const FCRect& rect, int angle);
    virtual void fillGradientPath(Long dwFirst, Long dwSecond, const FCRect& rect, int angle);
    virtual void fillGradientPolygon(Long dwFirst, Long dwSecond, FCPoint *apt, int cpt, int angle);
    virtual void fillGradientRect(Long dwFirst, Long dwSecond, const FCRect& rect, int cornerRadius, int angle);
    virtual void fillPath(Long dwPenColor);
    virtual void fillPie(Long dwPenColor, const FCRect& rect, float startAngle, float sweepAngle);
    virtual void fillPolygon(Long dwPenColor, FCPoint *apt, int cpt);
    virtual void fillRect(Long dwPenColor, const FCRect& rect);
    virtual void fillRect(Long dwPenColor, int left, int top, int right, int bottom);
    virtual void fillRoundRect(Long dwPenColor, const FCRect& rect, int cornerRadius);
    virtual Long getColor(Long dwPenColor);
    virtual Long getPaintColor(Long dwPenColor);
    virtual FCPoint getOffset();
    virtual FCPoint rotate(const FCPoint& op, const FCPoint& mp, int angle);
    virtual void saveImage(String key, UIImage* value);
    virtual void setClip(const FCRect& rect);
    virtual void setLineCap(int startLineCap, int endLineCap);
    virtual void setOffset(const FCPoint& offset);
    virtual void setOpacity(float opacity);
    virtual void setResourcePath(const String& resourcePath);
    virtual void setRotateAngle(int rotateAngle);
    virtual void setScaleFactor(double scaleFactorX, double scaleFactorY);
    virtual void setSmoothMode(int smoothMode);
    virtual void setTextQuality(int textQuality);
    virtual bool supportTransparent();
    virtual FCSize textSize(const wchar_t *strText, FCFont *font);
    virtual FCSizeF textSizeF(const wchar_t *strText, FCFont *font);
};

#endif
