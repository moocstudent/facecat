/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCCHART_H__
#define __FCCHART_H__
#pragma once
#include "stdafx.h"
#include "FCStr.h"
#include "FCDataTable.h"
#include "ChartDiv.h"
#include "FCPlot.h"
#include "BaseShape.h"
#include "VScale.h"

namespace FaceCat{
    class ChartDiv;
    class FCPlot;
    class BaseShape;
    class BarShape;
    class PointShape;
    class CandleShape;
    class PolylineShape;
    class TextShape;
    class FCView;
    class VScale;
    /*
     * 图表
     */
    class FCChart:public FCView{
    private:
        /*
         * 秒表ID
         */
        int m_timerID;
    protected:
        /**
         * 数据是否
         */
        bool m_autoFillHScale;
        /**
         * 空白空间
         */
        int m_blankSpace;
        /**
         * 是否可以拖动线条
         */
        bool m_canMoveShape;
        /**
         * 是否可纵向改变大小
         */
        bool m_canResizeV;
        /**
         * 是可横向改变大小
         */
        bool m_canResizeH;
        /**
         * 是否可以执行滚动操作
         */
        bool m_canScroll;
        /**
         * 是否可以执行缩放操作
         */
        bool m_canZoom;
        CrossLineMoveMode m_crossLineMoveMode;
        /**
         * 十字线当前停留的记录索引
         */
        int m_crossStopIndex;
        int m_cross_y;
        /**
         * 数据源
         */
        FCDataTable *m_dataSource;
        /*
         * 所有图层
         */
        ArrayList<ChartDiv*> m_divs;
        /**
         * 首先可见记录号
         */
        int m_firstVisibleIndex;
        /*
         * 横向大小调整方式
         */
        int m_hResizeType;
        /**
         * 横轴字段的显示文字
         */
        String m_hScaleFieldText;
        /**
         * 每条数据在X轴上所占的空间
         */
        double m_hScalePixel;
        /**
         * 触摸是否正在移动
         */
        bool m_isTouchMove;
        /**
         * 滚动十字线标识
         */
        bool m_isScrollCross;
        /**
         * 上次移动的点位
         */
        FCPoint m_lastTouchMovePoint;
        /**
         * 上次点击的位置
         */
        FCPoint m_lastTouchClickPoint;
        /**
         * 触摸最后一次移动的事件
         */
        double m_lastTouchMoveTime;
        /**
         * 当前最后一条记录是否可见
         */
        bool m_lastRecordIsVisible;
        /*
         * 最后可见的索引
         */
        int m_lastUnEmptyIndex;
        /**
         * 最后一条非空索引
         */
        double m_lastVisibleKey;
        /**
         * 获取最后可见的记录号
         */
        int m_lastVisibleIndex;
        /**
         * 获取左侧Y轴的宽度
         */
        int m_leftVScaleWidth;
        /**
         * 获取显示最大记录数
         */
        int m_maxVisibleRecord;
        /*
         * 正在被移动的画线
         */
        FCPlot *m_movingPlot;
        /**
         * 获取正在被移动的图形
         */
        BaseShape *m_movingShape;
        /**
         * 获取是否反转横轴
         */
        bool m_reverseHScale;
        /**
         * 获取右侧Y轴的宽度
         */
        int m_rightVScaleWidth;
        /**
         * 获取是否启用加速效果
         */
        bool m_scrollAddSpeed;
        /**
         * 按键滚动的幅度
         */
        int m_scrollStep;
        /**
         * 是否显示十字线
         */
        bool m_showCrossLine;
        /**
         * 显示选中区域
         */
        bool m_showingSelectArea;
        /**
         * 是否正在显示提示框
         */
        bool m_showingToolTip;
        /**
         * 提示框显示延迟tick值
         */
        int m_tooltip_dely;
        /*
         * 正在调整大小的图层
         */
        ChartDiv *m_userResizeDiv;
        /*
         * 横向工作区域
         */
        int m_workingAreaWidth;
    public:
        /**
         * 画细线，只能是水平线或垂直线
         * @param paint 绘图对象
         * @param color 颜色
         * @param width 宽度
         * @param x1    第一个点的横坐标
         * @param y1    第一个点的纵坐标
         * @param x2    第二个点的横坐标
         * @param y2    第二个点的纵坐标
         */
        virtual void drawThinLine(FCPaint *paint, Long dwPenColor, int width, int x1, int y1, int x2, int y2);
        /**
         * 绘制文字
         * @param paint 绘图对象
         * @param text  文字
         * @param dwPenColor  颜色
         * @param font  字体
         * @param point 坐标
         */
        virtual void drawText(FCPaint *paint, const wchar_t *strText, Long dwPenColor,FCFont *font, int left, int top);
        /**
         * 获取纵轴的刻度
         * @param max   最大值
         * @param min   最小值
         * @param vScale    坐标轴
         *  @returns  刻度集合
         */
        virtual ArrayList<double> getVScaleStep(double max, double min, ChartDiv *div, VScale *vScale);
        /**
         * 绘制成交量
         * @param paint 绘图对象
         * @param div   要绘制的层
         * @param bs    线条对象
         */
        virtual void onPaintBar(FCPaint *paint, ChartDiv *div, BarShape *bs);
        /**
         * 绘制K线
         * @param paint 绘图对象
         * @param div   要绘制的层
         * @param cs    K线
         */
        virtual void onPaintCandle(FCPaint *paint, ChartDiv *div, CandleShape *cs);
        /**
         * 绘制K线的扩展属性
         * @param paint 绘图对象
         * @param div   要绘制的层
         * @param cs    K线
         * @param visibleMaxIndex   最大值索引
         * @param visibleMax    最大值
         * @param visibleMinIndex   最小值索引
         * @param visibleMin    最小值
         */
        virtual void onPaintCandleEx(FCPaint *paint, ChartDiv *div, CandleShape *cs, int visibleMaxIndex, double visibleMax, int visibleMinIndex, double visibleMin);
        /**
         * 绘制十字线
         * @param paint 绘图对象
         * @param div   要绘制的层
         */
        virtual void onPaintCrossLine(FCPaint *paint, ChartDiv *div);
        /**
         * 绘制层背景
         * @param paint 绘图对象
         * @param div   要绘制的层
         */
        virtual void onPaintDivBackGround(FCPaint *paint, ChartDiv *div);
        /**
         * 绘制层边框
         * @param paint 绘图对象
         * @param div   要绘制的层
         */
        virtual void onPaintDivBorder(FCPaint *paint, ChartDiv *div);
        /**
         * 绘制横坐标轴
         * @param paint 绘图对象
         * @param div   要绘制的层
         */
        virtual void onPaintHScale(FCPaint *paint, ChartDiv *div);
        /**
         * 绘制图形的图标
         * @param paint 绘图对象
         */
        virtual void onPaintIcon(FCPaint *paint);
        /**
         * 绘制画线工具
         * @param paint 绘图对象
         * @param div   要绘制的层
         */
        virtual void onPaintPlots(FCPaint *paint, ChartDiv *div);
        /**
         * 绘制趋势线
         * @param paint 绘图对象
         * @param div   要绘制的层
         * @param ls    线条对象
         */
        virtual void onPaintPolyline(FCPaint *paint, ChartDiv *div, PolylineShape *ls);
        /**
         * 绘制趋势线
         * @param paint 绘图对象
         * @param div   图层
         * @param lineColor 线的颜色
         * @param fillColor 填充色
         * @param ciClr 颜色字段
         * @param lineWidth 线的宽度
         * @param lineStyle 线的样式
         * @param value 点的值
         * @param attachVScale  依附坐标轴
         * @param scaleX  横坐标
         * @param lY    纵坐标
         * @param i     索引
         * @param points    点集合
         * @param x     横坐标
         * @param y     纵坐标
         */
        virtual void onPaintPolyline(FCPaint *paint, ChartDiv *div, Long lineColor, Long fillColor, int ciClr,
                                     float lineWidth, PolylineStyle lineStyle, double value, AttachVScale attachVScale,
                                     int scaleX, int lY, int i, vector<FCPoint> *points, int *x, int *y);
        /**
         * 绘制拖动的边线
         * @param paint 绘图对象
         */
        virtual void onPaintResizeLine(FCPaint *paint);
        /**
         * 绘制选中块
         * @param paint 绘图对象
         * @param div   要绘制的层
         */
        virtual void onPaintSelectArea(FCPaint *paint, ChartDiv *div);
        /**
         * 绘制K线，成交量，趋势线等等
         * @param paint 绘图对象
         * @param div   要绘制的层
         */
        virtual void onPaintShapes(FCPaint *paint, ChartDiv *div);
        /**
         * 绘制文字
         * @param paint 绘图对象
         * @param div   图层
         * @param ts    文字
         */
        virtual void onPaintText(FCPaint *paint, ChartDiv *div, TextShape *ts);
        /**
         *  绘制标题
         * @param paint 绘图对象
         * @param div   要绘制的层
         */
        virtual void onPaintTitle(FCPaint *paint, ChartDiv *div);
        /**
         *  绘制提示框
         * @param paint 绘图对象
         */
        virtual void onPaintToolTip(FCPaint *paint);
        /**
         *  绘制纵坐标轴
         * @param paint 绘图对象
         * @param div   要绘制的层
         */
        virtual void onPaintVScale(FCPaint *paint, ChartDiv *div);
    protected:
        /**
         *  修正可见索引
         * @param dataCount 数据条数
         * @param first     首先可见索引号
         * @param last   最后可见索引号
         */
        virtual void correctVisibleRecord(int dataCount, int *first, int *last);
        /**
         *  获取K线最大值的显示位置
         * @param scaleX    横坐标
         * @param scaleY    纵坐标
         * @param stringWidth   文字的宽度
         * @param stringHeight  文字的高度
         * @param actualWidth   横向宽度
         * @param leftVScaleWidth  左侧纵轴宽度
         * @param rightVScaleWidth  右侧纵轴宽度
         * @param x 最大值的横坐标
         * @param y 最大值的纵坐标
         */
        virtual void getCandleMaxStringPoint(float scaleX, float scaleY, float stringWidth, float stringHeight, int actualWidth,
                                             int leftVScaleWidth, int rightVScaleWidth, float *x, float *y);
        /**
         * 获取K线最小值的显示位置
         * @param scaleX    横坐标
         * @param scaleY    纵坐标
         * @param stringWidth   文字的宽度
         * @param stringHeight  文字的高度
         * @param actualWidth   横向宽度
         * @param leftVScaleWidth  左侧纵轴宽度
         * @param rightVScaleWidth  右侧纵轴宽度
         * @param x 最大值的横坐标
         * @param y 最大值的纵坐标
         */
        virtual void getCandleMinStringPoint(float scaleX, float scaleY, float stringWidth, float stringHeight, int actualWidth,
                                             int leftVScaleWidth, int rightVScaleWidth, float *x, float *y);
        /**
         * 获取某坐标对应的索引
         * @param x 横坐标
         * @param leftVScaleWidth   左侧纵轴的高度
         * @param hScalePixel   数据间隔
         * @param firstVisibleIndex 首先可见索引
         * @returns  坐标
         */
        virtual int getChartIndex(int x, int leftScaleWidth, double hScalePixel, int firstVisibleIndex);
        /**
         * 获取阳K线的高度
         * @param close 收盘价
         * @param open  开盘价
         * @param max   最高价
         * @param min   最低价
         * @param divPureV  层高度
         * @returns  高度
         */
        virtual float getUpCandleHeight(double close, double open, double max, double min, float divPureV);
        /**
         * 获取阴K线的高度
         * @param close 收盘价
         * @param open  开盘价
         * @param max   最高价
         * @param min   最低价
         * @param divPureV  层高度
         *  @returns  高度
         */
        virtual float getDownCandleHeight(double close, double open, double max, double min, float divPureV);
        /**
         * 左滚
         * @param step  步长
         * @param dateCount 数据条数
         * @param hScalePixel   数据间隔
         * @param pureH 横向宽度
         * @param fIndex    首先可见索引号
         * @param lIndex    最后可见索引号
         */
        virtual void scrollLeft(int step, int dateCount, double hScalePixel, int pureH, int *fIndex, int *lIndex);
        /**
         * 右滚
         * @param step  步长
         * @param dateCount 数据条数
         * @param hScalePixel   数据间隔
         * @param pureH 横向宽度
         * @param fIndex    首先可见索引号
         * @param lIndex    最后可见索引号
         */
        virtual void scrollRight(int step, int dataCount, double hScalePixel, int pureH, int *fIndex, int *lIndex);
        /**
         *  获取纵轴某坐标的值
         * @param y 纵坐标
         * @param max   最大值
         * @param min   最小值
         * @param vHeight  层高度
         *  @returns  数值
         */
        virtual double getVScaleValue(int y, double max, double min, float vHeight);
        /**
         *  重置十字线索引
         * @param dataCount 数据条数
         * @param maxVisibleRecord  最大显示记录数
         * @param crossStopIndex    十字线索引
         * @param firstL            首先可见索引号
         * @param lastL             最后可见索引号
         * @returns  修正后的十字线索引
         */
        virtual int resetCrossOverIndex(int dataCount, int maxVisibleRecord, int crossStopIndex, int firstL, int lastL);
        /**
         * 缩小
         * @param pureH 横向宽度
         * @param dataCount 数据条数
         * @param findex    首先可见索引号
         * @param lindex    最后可见索引号
         * @param hScalePixel   数据间隔
         */
        virtual void zoomIn(int pureH, int dataCount, int *findex, int *lindex, double *hScalePixel);
        /**
         * 放大
         * @param pureH 横向宽度
         * @param dataCount 数据条数
         * @param findex    首先可见索引号
         * @param lindex    最后可见索引号
         * @param hScalePixel   数据间隔
         */
        virtual void zoomOut(int pureH,int dataCount, int *findex, int *lindex, double *hScalePixel);
    public:
        /*
         * 创建图表
         */
        FCChart();
        /*
         * 析构函数
         */
        virtual ~FCChart();
        /**
         * 获取数据是否
         */
        virtual bool autoFillHScale();
        /**
         * 设置数据是否
         */
        virtual void setAutoFillHScale(bool autoFillHScale);
        /**
         * 获取空白空间
         */
        virtual int getBlankSpace();
        /**
         * 设置空白空间
         */
        virtual void setBlankSpace(int blankSpace);
        /**
         *获取是否可以拖动线条
         */
        virtual bool canMoveShape();
        /**
         * 设置是否可以拖动线条
         */
        virtual void setCanMoveShape(bool canMoveShape);
        /**
         * 获取是否可纵向改变大小
         */
        virtual bool canResizeV();
        /**
         * 设置是否可纵向改变大小
         */
        virtual void setCanResizeV(bool canResizeV);
        /**
         * 获取是可横向改变大小
         */
        virtual bool canResizeH();
        /**
         * 设置是可横向改变大小
         */
        virtual void setCanResizeH(bool canResizeH);
        /**
         * 获取是否可以执行滚动操作
         */
        virtual bool canScroll();
        /**
         * 设置是否可以执行滚动操作
         */
        virtual void setCanScroll(bool canScroll);
        /**
         * 获取是否可以执行缩放操作
         */
        virtual bool canZoom();
        /**
         * 设置是否可以执行缩放操作
         */
        virtual void setCanZoom(bool canZoom);
        /**
         * 获取十字线的移动方式
         */
        virtual CrossLineMoveMode getCrossLineMoveMode();
        /**
         * 设置十字线的移动方式
         */
        virtual void setCrossLineMoveMode(CrossLineMoveMode crossLineMoveMode);
        /**
         * 获取十字线当前停留的记录索引
         */
        virtual int getCrossStopIndex();
        /**
         * 设置十字线当前停留的记录索引
         */
        virtual void setCrossStopIndex(int crossStopIndex);
        /**
         * 获取数据源
         */
        virtual FCDataTable* getDataSource();
        /**
         * 设置数据源
         */
        virtual void setDataSource(FCDataTable *dataSource);
        /**
         * 获取首先可见记录号
         */
        virtual int getFirstVisibleIndex();
        /**
         * 设置首先可见记录号
         */
        virtual void setFirstVisibleIndex(int firstVisibleIndex);
        /**
         * 获取横轴字段的显示文字
         */
        virtual String getHScaleFieldText();
        /**
         * 设置横轴字段的显示文字
         */
        virtual void setHScaleFieldText(const String& hScaleFieldText);
        /**
         * 获取每条数据在X轴上所占的空间
         */
        virtual double getHScalePixel();
        /**
         * 设置每条数据在X轴上所占的空间
         */
        virtual void setHScalePixel(double hScalePixel);
        /**
         * 获取最后可见的记录号
         */
        virtual int getLastVisibleIndex();
        /**
         * 设置最后可见的记录号
         */
        virtual void setLastVisibleIndex(int lastVisibleIndex);
        /**
         * 获取左侧Y轴的宽度
         */
        virtual int getLeftVScaleWidth();
        /**
         * 设置左侧Y轴的宽度
         */
        virtual void setLeftVScaleWidth(int leftVScaleWidth);
        /**
         * 获取显示最大记录数
         */
        virtual int getMaxVisibleRecord();
        /**
         * 设置显示最大记录数
         */
        virtual void setMaxVisibleRecord(int maxVisibleRecord);
        /**
         * 获取正在移动的画线工具
         */
        virtual FCPlot* getMovingPlot();
        /**
         * 获取正在被移动的图形
         */
        virtual BaseShape* getMovingShape();
        /**
         * 获取是否反转横轴
         */
        virtual bool isReverseHScale();
        /**
         * 设置是否反转横轴
         */
        virtual void setReverseHScale(bool reverseHScale);
        /**
         * 获取右侧Y轴的宽度
         */
        virtual int getRightVScaleWidth();
        /**
         * 设置右侧Y轴的宽度
         */
        virtual void setRightVScaleWidth(int rightVScaleWidth);
        /**
         * 获取是否启用加速效果
         */
        virtual bool isScrollAddSpeed();
        /**
         * 设置是否启用加速效果
         */
        virtual void setScrollAddSpeed(bool scrollAddSpeed);
        /**
         * 获取当前选中的线条
         */
        virtual BaseShape* getSelectedShape();
        /**
         * 设置当前选中的线条
         */
        virtual void setSelectedShape(BaseShape *baseShape);
        /**
         * 获取当前选中的画线工具
         */
        virtual FCPlot* getSelectedPlot();
        /**
         * 设置当前选中的画线工具
         */
        virtual void setSelectedPlot(FCPlot *selectedPlot);
        /**
         * 获取当前选中的层
         */
        virtual ChartDiv* getSelectedDiv();
        /**
         * 设置当前选中的层
         */
        virtual void setSelectedDiv(ChartDiv *selectedDiv);
        /**
         * 获取是否显示十字线
         */
        virtual bool isShowCrossLine();
        /**
         * 设置是否显示十字线
         */
        virtual void setShowCrossLine(bool showCrossLine);
        /**
         * 获取层去掉坐标轴宽度后的横向宽度
         *  @returns 宽度
         */
        virtual int getWorkingAreaWidth();
    public:
        /*
         * 添加层
         */
        virtual ChartDiv* addDiv(int vPercent);
        /*
         * 添加层
         */
        virtual ChartDiv* addDiv();
        /**
         * 可见部分的最大值和最小值
         */
        virtual void adjust();
        /**
         * 在指定层的指定位置添加画线工具
         */
        virtual void addPlot(FCPlot *bpl, const FCPoint& mp, ChartDiv *div);
        /**
         * 清除图像上的数据，但不改变图形结构
         */
        virtual void clear();
        /**
         * 对图像进行操作
         */
        virtual void changeChart(ScrollType scrollType, int limitStep);
        /**
         * 检查最后可见索引
         */
        virtual void checkLastVisibleIndex();
        /**
         * 检查并弹出提示框
         */
        virtual void checkToolTip();
        /**
         * 取消选中所有图形，包括K线，柱状图，线等
         */
        virtual void clearSelectedShape();
        /**
         * 取消选中所有的画线工具
         */
        virtual void clearSelectedPlot();
        /**
         * 取消选中所有的层
         */
        virtual void clearSelectedDiv();
        /**
         * 隐藏选中框
         */
        virtual void closeSelectArea();
        /**
         * 根据记录号获取层极值
         */
        virtual double divMaxOrMin(int index, ChartDiv *div, int flag);
        /**
         * 由坐标获取层对象，返回图层对象
         */
        virtual ChartDiv* findDiv(const FCPoint& mp);
        /**
         * 由图形名称获取包含它的层，返回图层对象
         */
        virtual ChartDiv* findDiv(BaseShape *shape);
        /**
         * 获取控件类型
         */
        virtual String getControlType();
        /*
         * 获取日期类型
         */
        virtual int getDateType(DateType dateType);
        /*
         * 设置日期类型
         */
        virtual DateType getDateType(int dateType);
        /**
         * 获取图层集合的拷备
         */
        virtual ArrayList<ChartDiv*> getDivs();
        /**
         * 获取横轴的文字
         */
        virtual void getHScaleDateString(double date, double lDate, DateType *dateType, wchar_t *str);
        /**
         * 由坐标点获取它对应的索引
         */
        virtual int getIndex(const FCPoint& mp);
        /**
         * 获取最大显示条数
         */
        virtual int getMaxVisibleCount(double hScalePixel, int pureH);
        /*
         * 获取触摸的层
         */
        virtual ChartDiv* getTouchOverDiv();
        /**
         * 获取触摸所在横向记录索引
         */
        virtual int getTouchOverIndex();
        /**
         * 获取坐标轴的值
         */
        double getNumberValue(ChartDiv *div, const FCPoint& mp, AttachVScale attachVScale);
        /**
         * 获取属性值
         */
        virtual void getProperty(const String& name, String *value, String *type);
        /**
         * 获取属性名称列表
         */
        virtual ArrayList<String> getPropertyNames();
        /**
         * 由字段获取所有的图形
         */
        virtual int getShapesCount(int field);
        /**
         * 获取指定索引的横坐标
         */
        virtual float getX(int index);
        /**
         * 获取某一值对应的纵坐标
         */
        virtual float getY(ChartDiv *div,double value,AttachVScale attach);
        /**
         * 获取纵轴的基础字段
         */
        virtual int getVScaleBaseField(ChartDiv *div, VScale *vScale);
        /**
         * 获取纵轴的基准值
         */
        virtual double getVScaleBaseValue(ChartDiv *div, VScale *vScale, int i);
        /*
         * 计算坐标轴的参数
         */
        virtual int gridScale(double min, double max, int yLen, int maxSpan, int minSpan, int defCount, double *step, int *digit);
        /**
         * 判断是否正在操作图形
         */
        virtual bool isOperating();
        /**
         * 定位十字线
         */
        virtual void locateCrossLine();
        /**
         * 将图形移动到另一个层中
         */
        virtual void moveShape(ChartDiv *div, BaseShape *shape);
        /**
         * 重置十字线穿越的记录号
         */
        virtual void resetCrossOverIndex();
        /**
         *  重置图像，删除所有的数据，层，指标和画线工具等
         */
        virtual void removeAll();
        /**
         * 移除图层
         */
        virtual bool resizeDiv();
        /**
         * 拖动图层改变大小
         */
        virtual void removeDiv(ChartDiv *div);
        /**
         * 重置
         */
        virtual void reset();
        /**
         * 自动设置首先可见和最后可见的记录号
         */
        virtual void resetVisibleRecord();
        /**
         * 左滚画面
         */
        virtual void scrollLeft(int step);
        /**
         * 立即向左滚动到显示第一条数据为止
         */
        virtual void scrollLeftToBegin();
        /**
         * 右滚画面
         */
        virtual void scrollRight(int step);
        /**
         * 立即向右滚动到显示最后一条数据为止
         */
        virtual void scrollRightToEnd();
        /**
         * 左滚十字线
         */
        virtual void scrollCrossLineLeft(int step);
        /**
         * 右滚十字线
         */
        virtual void scrollCrossLineRight(int step);
        /**
         * 选中线条的方法
         */
        virtual BaseShape* selectShape(int curIndex, int state);
        /**
         * 判断是否选中柱状图
         */
        virtual bool selectBar(ChartDiv *div, float mpY, int fieldName, int fieldName2, int styleField, AttachVScale attachVScale, int curIndex);
        /**
         * 判断是否选中柱状图
         */
        virtual bool selectCandle(ChartDiv *div, float mpY, int highField, int lowField, int styleField, AttachVScale attachVScale, int curIndex);
        /**
         * 判断是否选中线
         */
        virtual bool selectPolyline(ChartDiv *div, const FCPoint& mp, int fieldName, float lineWidth, AttachVScale attachVScale, int curIndex);
        /**
         * 设置属性
         */
        virtual void setProperty(const String& name, const String& value);
        /**
         * 设置图形首先可见的索引号和最后可见的索引号
         */
        virtual void setVisibleIndex(int firstVisibleIndex, int lastVisibleIndex);
        /**
         * 重新布局
         */
        virtual void update();
        /**
         * 缩小
         */
        virtual void zoomIn();
        /**
         * 放大
         */
        virtual void zoomOut();
    public:
        /*
         * 键盘按下方法
         */
        virtual void onKeyDown(char key);
        /**
         * 控件添加方法
         */
        virtual void onLoad();
        /**
         * 触摸离开的方法
         */
        virtual void onTouchDown(FCTouchInfo touchInfo);
        /**
         * 触摸抬起的方法
         */
        virtual void onTouchMove(FCTouchInfo touchInfo);
        /**
         * 触摸按下的方法
         */
        virtual void onTouchUp(FCTouchInfo touchInfo);
        /*
         *  触摸滚动方法
         */
        virtual void onTouchWheel(FCTouchInfo touchInfo);
        /*
         * 键盘抬起的方法
         */
        virtual void onKeyUp(char key);
        /**
         * 重绘前景方法
         */
        virtual void onPaintForeground(FCPaint *paint, const FCRect& clipRect);
        /**
         * 秒表回调方法
         */
        virtual void onTimer(int timerID);
    };
}
#endif
