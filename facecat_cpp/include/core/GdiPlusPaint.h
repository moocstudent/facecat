/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __GDIPLUSPAINT_H__
#define __GDIPLUSPAINT_H__
#pragma once
#include "..\\..\\stdafx.h"
#include <comdef.h>
#include "gdiplus.h"
#pragma comment(lib, "gdiplus.lib")
using namespace Gdiplus;
#include "FCPaint.h"

namespace FaceCat{
	/*
	* GDI+绘图类
	*/
	class GdiPlusPaint : public FCPaint{
	protected:
	    /**
		 * 位图
		 */
		Bitmap *m_bitmap;
		/**
		 * 画刷
		 */
		SolidBrush *m_brush;
		/**
		 * 画刷的颜色
		 */
		Long m_brushColor;
		/**
		 * 空的字符串格式
		 */
		StringFormat *m_emptyStringFormat;
		/**
		 * 直线结束样式
		 */
		int m_endLineCap;
		/**
		 * 导出路径
		 */
		String m_exportPath;
		/**
		 * 绘图画面
		 */
		Graphics *m_g;
		/**
		 * 控件的HDC
		 */
		HDC m_hDC;
		/**
		 * 图像缓存
		 */
		map<String, Bitmap*> m_images;
		/**
		 * 横向偏移
		 */
		int m_offsetX;
		/**
		 * 纵向偏移
		 */
		int m_offsetY;
		/**
		 * 透明度
		 */
		float m_opacity;
		/**
		 * 绘图路径
		 */
		GraphicsPath *m_path;
		/**
		 * 画笔
		 */
		Pen *m_pen;
		/**
		 * 刷新区域
		 */
		FCRect m_pRect;
		/**
		 *  画笔的颜色
		 */
		Long m_penColor;
		/**
		 * 画笔的宽度
		 */
		float m_penWidth;
		/**
		 *  画笔的样式
		 */
		int m_penStyle;
		/**
		 * 资源路径
		 */
		String m_resourcePath;
		/**
		 * 旋转角度
		 */
		int m_rotateAngle;
		/**
		 * 横向缩放因子
		 */
		double m_scaleFactorX;
		/**
		 * 纵向缩放因子
		 */
		double m_scaleFactorY;
		/**
		 * 平滑程度
		 */
		int m_smoothMode;
		/**
		 * 直线开始样式
		 */
		int m_startLineCap;
		/**
		 * 文字的质量
		 */
		int m_textQuality;
		/**
		 * 窗口大小
		 */
		FCRect m_wRect;
	protected:
	    /**
         * 缩放因子生效
         * @param gdiplusRect 矩形
        */
		void affectScaleFactor(Rect *gdiplusRect);
	    /**
         * 缩放因子生效
         * @param gdiplusRect 矩形
        */
		void affectScaleFactor(RectF *gdiplusRect);
	    /**
         * 获取画刷
         * @param dwPenColor 颜色
        */
		SolidBrush* getBrush(Long dwPenColor);
	    /**
         * 获取颜色
         * @param dwPenColor 整型颜色
        */
		Color getGdiPlusColor(Long dwPenColor);
		int getEncoderClsid(const WCHAR *format, CLSID *pClsid);
	    /**
         * 获取Gdi字体
         * @param font   Gdi字体
        */
		Gdiplus::Font* getFont(FCFont *font);
	    /**
         * 获取画笔
         * @param dwPenColor 整型颜色
         * @param width  宽度
         * @param style  样式
        */
		Pen* getPen(Long dwPenColor, float width, int style);
	    /**
         * 获取绘制圆形矩形的路径
         * @param gdiPlusRect 矩形
         * @param cornerRadius  边角半径
        */
		GraphicsPath* getRoundRectPath(Rect gdiPlusRect, int cornerRadius);
	public:
		/*
		* 构造函数
		*/
		GdiPlusPaint();
		/*
		* 析构函数
		*/
		virtual ~GdiPlusPaint();
	public:
	    /**
         * 添加曲线
         * @param rect 矩形区域
         * @param startAngle  从 x 轴到弧线的起始点沿顺时针方向度量的角（以度为单位）
         * @param sweepAngle  从 startAngle 参数到弧线的结束点沿顺时针方向度量的角（以度为单位）
        */
		virtual void addArc(const FCRect& rect, float startAngle, float sweepAngle);
	    /**
         * 添加贝赛尔曲线
         * @param points  点阵
        */
		virtual void addBezier(FCPoint *apt, int cpt);
	    /**
         * 添加曲线
         * @param points  点阵
        */
		virtual void addCurve(FCPoint *apt, int cpt);
	    /**
         * 添加椭圆
         * @param rect  矩形
        */
		virtual void addEllipse(const FCRect& rect);
	    /**
         * 添加直线
         * @param x1 第一个点的横坐标
         * @param y1 第一个点的纵坐标（以度为单位）
         * @param x2  第二个点的横坐标
         * @param y2  第二个点的纵坐标
        */
		virtual void addLine(int x1, int y1, int x2, int y2);
	    /**
         * 添加矩形
         * @param rect  区域
        */
		virtual void addRect(const FCRect& rect);
	    /**
         * 添加扇形
         * @param rect 矩形区域
         * @param startAngle  从 x 轴到弧线的起始点沿顺时针方向度量的角（以度为单位）
         * @param sweepAngle  从 startAngle 参数到弧线的结束点沿顺时针方向度量的角（以度为单位）
        */
		virtual void addPie(const FCRect& rect, float startAngle, float sweepAngle);
	    /**
         * 添加文字
         * @param text 文字
         * @param font 字体
         * @param rect 区域
        */
		virtual void addText(const wchar_t *strText, FCFont *font, const FCRect& rect);
	    /**
         * 开始导出
         * @param exportPath  路径
         * @param rect 区域
        */
		virtual void beginExport(const String& exportPath, const FCRect& rect);
	    /**
         * 开始绘图
         * @param hdc HDC
         * @param wRect 窗体区域
         * @param pRect 刷新区域
        */
		virtual void beginPaint(HDC hDC, const FCRect& wRect, const FCRect& pRect);
		/**
		 * 开始一段路径
		 */
		virtual void beginPath();
		/**
		 * 裁剪路径
		 */
		virtual void clipPath();
		/**
		 * 清除缓存
		 */
		virtual void clearCaches();
		/**
		 * 闭合路径
		 */
		virtual void closeFigure();
		/**
		 * 结束一段路径
		 */
		virtual void closePath();
	    /**
         * 绘制弧线
         * @param dwPenColor 颜色
         * @param width  宽度
         * @param style  样式
         * @param rect 矩形区域
         * @param startAngle  从 x 轴到弧线的起始点沿顺时针方向度量的角（以度为单位）
         * @param sweepAngle  从 startAngle 参数到弧线的结束点沿顺时针方向度量的角（以度为单位）
        */
		virtual void drawArc(Long dwPenColor, float width, int style, const FCRect& rect, float startAngle, float sweepAngle);
	    /**
         * 设置贝赛尔曲线
         * @param dwPenColor 颜色
         * @param width  宽度
         * @param style  样式
         * @param points  坐标阵1
        */
		virtual void drawBezier(Long dwPenColor, float width, int style, FCPoint *apt, int cpt);
	    /**
         * 绘制曲线
         * @param dwPenColor 颜色
         * @param width  宽度
         * @param style  样式
         * @param points  坐标阵
        */
		virtual void drawCurve(Long dwPenColor, float width, int style, FCPoint *apt, int cpt);
	    /**
         * 绘制矩形
         * @param dwPenColor 颜色
         * @param width  宽度
         * @param style  样式
         * @param rect  矩形区域
        */
		virtual void drawEllipse(Long dwPenColor, float width, int style, const FCRect& rect);
	    /**
         * 绘制矩形
         * @param dwPenColor 颜色
         * @param width  宽度
         * @param style  样式
         * @param left  左侧坐标
         * @param top 顶部左标
         * @param right  右侧坐标
         * @param bottom  底部坐标
        */
		virtual void drawEllipse(Long dwPenColor, float width, int style, int left, int top, int right, int bottom);
	    /**
         * 绘制图片
         * @param imagePath  图片路径
         * @param rect  绘制区域
        */
		virtual void drawImage(const wchar_t *imagePath, const FCRect& rect);
	    /**
         * 绘制直线
         * @param dwPenColor 颜色
         * @param width  宽度
         * @param style  样式
         * @param x1  第一个点的横坐标
         * @param y1  第一个点的纵坐标
         * @param x2  第二个点的横坐标
         * @param y2  第二个点的纵坐标
        */
		virtual void drawLine(Long dwPenColor, float width, int style, const FCPoint& x, const FCPoint& y);
	    /**
         * 绘制直线
         * @param dwPenColor 颜色
         * @param width  宽度
         * @param style  样式
         * @param x  第一个点的坐标
         * @param y  第二个点的坐标
        */
		virtual void drawLine(Long dwPenColor, float width, int style, int x1, int y1, int x2, int y2);
	    /**
         * 绘制直线
         * @param dwPenColor 颜色
         * @param width  宽度
         * @param style  样式
        */
		virtual void drawPath(Long dwPenColor, float width, int style);
	    /**
         * 绘制扇形
         * @param dwPenColor 颜色
         * @param width  宽度
         * @param style  样式
         * @param rect 矩形区域
         * @param startAngle  从 x 轴到弧线的起始点沿顺时针方向度量的角（以度为单位）
         * @param sweepAngle  从 startAngle 参数到弧线的结束点沿顺时针方向度量的角（以度为单位）
        */
		virtual void drawPie(Long dwPenColor, float width, int style, const FCRect& rect, float startAngle, float sweepAngle);
	    /**
         * 绘制多边形
         * @param dwPenColor 颜色
         * @param width  宽度
         * @param style  样式
         * @param points  点的数组
        */
		virtual void drawPolygon(Long dwPenColor, float width, int style, FCPoint *apt, int cpt);
	    /**
         * 绘制大量直线
         * @param dwPenColor 颜色
         * @param width  宽度
         * @param style  样式
         * @param points  点集
        */
		virtual void drawPolyline(Long dwPenColor, float width, int style, FCPoint *apt, int cpt);
	    /**
         * 绘制矩形
         * @param dwPenColor 颜色
         * @param width  宽度
         * @param style  样式
         * @param rect   矩形区域
        */
		virtual void drawRect(Long dwPenColor, float width, int style, int left, int top, int right, int bottom);
	    /**
         * 绘制矩形
         * @param dwPenColor 颜色
         * @param width  宽度
         * @param style  样式
         * @param left  左侧坐标
         * @param top 顶部左标
         * @param right  右侧坐标
         * @param bottom  底部坐标
        */
		virtual void drawRect(Long dwPenColor, float width, int style, const FCRect& rect);
	    /**
         * 绘制圆角矩形
         * @param dwPenColor 颜色
         * @param width  宽度
         * @param style  样式
         * @param rect   矩形区域
         * @param cornerRadius   边角半径
        */
		virtual void drawRoundRect(Long dwPenColor, float width, int style, const FCRect& rect, int cornerRadius);
	    /**
         * 绘制文字
         * @param text 文字
         * @param dwPenColor  颜色
         * @param font  字体
         * @param rect   矩形区域
        */
		virtual void drawText(const wchar_t *strText, Long dwPenColor, FCFont *font, const FCRect& rect);
	    /**
         * 绘制文字
         * @param text 文字
         * @param dwPenColor  颜色
         * @param font  字体
         * @param rect   矩形区域
        */
		virtual void drawText(const wchar_t *strText, Long dwPenColor, FCFont *font, const FCRectF& rect);
	    /**
         * 绘制自动省略结尾的文字
         * @param text 文字
         * @param dwPenColor  颜色
         * @param font  字体
         * @param rect   矩形区域
        */
		virtual void drawTextAutoEllipsis(const wchar_t *strText, Long dwPenColor, FCFont *font, const FCRect& rect);
		/**
		 * 结束导出
		 */
		virtual void endExport();
		/**
		 * 结束绘图
		 */
		virtual void endPaint();
		/**
		 * 反裁剪路径
		 */
		virtual void excludeClipPath();
	    /**
         * 填充椭圆
         * @param dwPenColor  颜色
         * @param rect   矩形区域
        */
		virtual void fillEllipse(Long dwPenColor, const FCRect& rect);
	    /**
         * 绘制渐变椭圆
         * @param dwFirst 开始颜色
         * @param dwSecond  结束颜色
         * @param rect   矩形区域
         * @param angle   角度
        */
		virtual void fillGradientEllipse(Long dwFirst, Long dwSecond, const FCRect& rect, int angle);
	    /**
         * 填充渐变路径
         * @param dwFirst 开始颜色
         * @param dwSecond  开始颜色
         * @param points   点的集合
         * @param angle   角度
        */
		virtual void fillGradientPath(Long dwFirst, Long dwSecond, const FCRect& rect, int angle);
	    /**
         * 绘制渐变的多边形
         * @param dwFirst 开始颜色
         * @param dwSecond  开始颜色
         * @param points   点的集合
         * @param angle   角度
        */
		virtual void fillGradientPolygon(Long dwFirst, Long dwSecond, FCPoint *apt, int cpt, int angle);
	    /**
         * 绘制渐变矩形
         * @param dwFirst 开始颜色
         * @param dwSecond  开始颜色
         * @param rect  矩形
         * @param cornerRadius   边角半径
         * @param angle   角度
        */
		virtual void fillGradientRect(Long dwFirst, Long dwSecond, const FCRect& rect, int cornerRadius, int angle);
	    /**
         * 填充路径
         * @param dwPenColor 颜色
        */
		virtual void fillPath(Long dwPenColor);
	    /**
         * 绘制扇形
         * @param dwPenColor 颜色
         * @param rect 矩形区域
         * @param startAngle  从 x 轴到弧线的起始点沿顺时针方向度量的角（以度为单位）
         * @param sweepAngle  从 startAngle 参数到弧线的结束点沿顺时针方向度量的角（以度为单位）
        */
		virtual void fillPie(Long dwPenColor, const FCRect& rect, float startAngle, float sweepAngle);
	    /**
         * 填充多边形
         * @param dwPenColor 颜色
         * @param points 点的数组
        */
		virtual void fillPolygon(Long dwPenColor, FCPoint *apt, int cpt);
	    /**
         * 填充矩形
         * @param dwPenColor 颜色
         * @param rect 矩形区域
        */
		virtual void fillRect(Long dwPenColor, const FCRect& rect);
	    /**
         * 填充矩形
         * @param dwPenColor 颜色
         * @param left  左侧坐标
         * @param top 顶部左标
         * @param right  右侧坐标
         * @param bottom  底部坐标
        */
		virtual void fillRect(Long dwPenColor, int left, int top, int right, int bottom);
	    /**
         * 填充圆角矩形
         * @param dwPenColor 颜色
         * @param rect  矩形区域
         * @param cornerRadius 边角半径
        */
		virtual void fillRoundRect(Long dwPenColor, const FCRect& rect, int cornerRadius);
	    /**
         * 获取颜色
         * @param dwPenColor 输入颜色
        */
		virtual Long getColor(Long dwPenColor);
	    /**
         * 获取要绘制的颜色
         * @param dwPenColor 输入颜色
        */
		virtual Long getPaintColor(Long dwPenColor);
		/**
		 * 获取偏移
		 */
		virtual FCPoint getOffset();
	    /**
         * 旋转角度
         * @param op 圆心坐标
         * @param mp  点的坐标
         * @param angle   角度
        */
		virtual FCPoint rotate(const FCPoint& op, const FCPoint& mp, int angle);
	    /**
         * 设置裁剪区域
         * @param rect 区域
        */
		virtual void setClip(const FCRect& rect);
	    /**
         * 设置直线两端的样式
         * @param startLineCap 开始的样式
         * @param endLineCap  结束的样式
        */
		virtual void setLineCap(int startLineCap, int endLineCap);
	    /**
         * 设置偏移
         * @param mp  偏移坐标
        */
		virtual void setOffset(const FCPoint& offset);
	    /**
         * 设置透明度
         * @param opacity  透明度
        */
		virtual void setOpacity(float opacity);
	    /**
         * 设置资源的路径
         * @param resourcePath  资源的路径
        */
        virtual void setResourcePath(const String& resourcePath);
	    /**
         * 设置旋转角度
         * @param rotateAngle  旋转角度
        */
		virtual void setRotateAngle(int rotateAngle);
	    /**
         * 设置缩放因子
         * @param scaleFactorX 横向因子
         * @param scaleFactorY  纵向因子
        */
		virtual void setScaleFactor(double scaleFactorX, double scaleFactorY);
	    /**
         * 设置平滑模式
         * @param smoothMode  平滑模式
        */
		virtual void setSmoothMode(int smoothMode);
	    /**
         * 设置文字的质量
         * @param textQuality  文字质量
        */
		virtual void setTextQuality(int textQuality);
		/**
		 * 设置是否支持透明色
		 */
		virtual bool supportTransparent();
	    /**
         * 获取文字大小
         * @param text 文字
         * @param font  字体
        */
		virtual FCSize textSize(const wchar_t *strText, FCFont *font);
	    /**
         * 获取文字大小
         * @param text 文字
         * @param font  字体
        */
		virtual FCSizeF textSizeF(const wchar_t *strText, FCFont *font);
	};
}

#endif