/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCPLOT_H__
#define __FCPLOT_H__
#pragma once
#include "stdafx.h"
#include "FCStr.h"
#include "FCNative.h"
#include "FCPaint.h"
#include "ChartDiv.h"
#include "FCChart.h"
#include "FCDataTable.h"
#include "FCPlot.h"

namespace FaceCat{
    class ChartDiv;
    class FCDataTable;
    class FCChart;
    class FCNative;
    class FCPlot;
    
    /*
     * 各点值的集合
     */
    class PlotMark{
    public:
        /*
         * 创建一个点值
         */
        PlotMark(int index, double key, double value){
            Index = index;
            Key = key;
            Value = value;
        }
        /*
         * 索引
         */
        int Index;
        /*
         * 键
         */
        double Key;
        /*
         * 值
         */
        double Value;
        /*
         * 拷贝
         */
        PlotMark* copy(){
            PlotMark *plotMark = new PlotMark(Index, Key, Value);
            return plotMark;
        }
    };
    
    /*
     * 画线工具的基类
     */
    class FCPlot{
    protected:
        /**
         * 移动类型
         */
        ActionType m_action;
        AttachVScale m_attachVScale;
        /**
         * 线的颜色
         */
        Long m_color;
        /**
         * 数据源
         */
        FCDataTable *m_dataSource;
        /**
         * 所在图层
         */
        ChartDiv *m_div;
        /**
         * 是否画移动残影
         */
        bool m_drawGhost;
        /**
         * 是否可以选中或拖放
         */
        bool m_enabled;
        /**
         * 字体
         */
        FCFont *m_font;
        /**
         * 是否正在绘制阴影
         */
        bool m_isPaintingGhost;
        /**
         * 线的样式，null为实线
         */
        int m_lineStyle;
        /**
         * 线的宽度
         */
        int m_lineWidth;
        /**
         * 关键点
         */
        HashMap<int,PlotMark*> m_marks;
        /**
         * 移动次数
         */
        int m_moveTimes;
        /**
         * 线条的类型
         */
        String m_plotType;
        /**
         * 源字段
         */
        HashMap<String,int> m_sourceFields;
        /**
         * 开始移动时的值
         */
        HashMap<int,PlotMark*> m_startMarks;
        /**
         * 是否被选中
         */
        bool m_selected;
        /**
         * 选中色
         */
        Long m_selectedColor;
        /**
         * 选中点的样式
         */
        SelectPoint m_selectedPoint;
        /**
         * 开始移动的点
         */
        FCPoint m_startPoint;
        /**
         * 显示的文字
         */
        String m_text;
        /**
         * 可见度
         */
        bool m_visible;
        /**
         * 图层顺序
         */
        int m_zOrder;
    protected:
        /**
         * 画椭圆
         * @param paint 绘图对象
         * @param dwPenColor    颜色
         * @param width 宽度
         * @param style 样式
         */
        void drawEllipse(FCPaint *paint, Long dwPenColor, int width, int style, const FCRect& rect);
        void drawEllipse(FCPaint *paint, Long dwPenColor, int width, int style, int left, int top, int right, int bottom);
        void drawEllipse(FCPaint *paint, Long dwPenColor, int width, int style, float left, float top, float right, float bottom);
        /**
         * 画线方法
         * @param paint 绘图对象
         * @param dwPenColor    颜色
         * @param width 宽度
         * @param style 样式
         */
        void drawLine(FCPaint *paint, Long dwPenColor, int width, int style, const FCPoint& x, const FCPoint& y);
        void drawLine(FCPaint *paint, Long dwPenColor, int width, int style, int x1, int y1, int x2, int y2);
        void drawLine(FCPaint *paint, Long dwPenColor, int width, int style, float x1, float y1, float x2, float y2);
        void drawPolygon(FCPaint *paint, Long dwPenColor, int width, int style, FCPoint *apt, int cpt);
        /**
         * 画曲线
         * @param paint 绘图对象
         * @param dwPenColor    颜色
         * @param width 宽度
         * @param style 样式
         * @param apt    点的集合
         */
        void drawPolyline(FCPaint *paint, Long dwPenColor, int width, int style, FCPoint *apt, int cpt);
        /**
         * 画射线
         * @param paint 绘图对象
         * @param dwPenColor    颜色
         * @param width 宽度
         * @param style 样式
         * @param x1    第一个点的横坐标
         * @param y1    第一个点的纵坐标
         * @param x2    第二个点的横坐标
         * @param y2    第二个点的纵坐标
         * @param k     直线参数k
         * @param b     直线参数b
         */
        void drawRay(FCPaint *paint, Long dwPenColor, int width, int style, float x1, float y1, float x2, float y2, float k, float b);
        /**
         * 画矩形
         * @param paint 绘图对象
         * @param dwPenColor    颜色
         * @param width 宽度
         * @param style 样式
         * @param left  横坐标
         * @param top   纵坐标
         * @param right 右侧横坐标
         * @param bottom    右侧纵坐标
         */
        void drawRect(FCPaint *paint, Long dwPenColor, int width, int style, int left, int top, int right, int bottom);
        /**
         * 画矩形
         * @param paint 绘图对象
         * @param dwPenColor    颜色
         * @param width 宽度
         * @param style 样式
         */
        void drawRect(FCPaint *paint, Long dwPenColor, int width, int style, const FCRect& rect);
        /**
         * 绘制选中点
         * @param paint 绘图对象
         * @param dwPenColor    颜色
         * @param x     横坐标
         * @param y     纵坐标
         */
        void drawSelect(FCPaint *paint, Long dwPenColor, int x, int y);
        void drawSelect(FCPaint *paint, Long dwPenColor, float x, float y);
        /**
         * 画文字
         * @param paint 绘图对象
         * @param text  文字
         * @param dwPenColor    颜色
         * @param font  字体
         * @param x     横坐标
         * @param y     纵坐标
         */
        void drawText(FCPaint *paint, const wchar_t *strText, Long dwPenColor, FCFont *font, int left, int top);
        void fillEllipse(FCPaint *paint, Long dwPenColor, const FCRect& rect);
        /**
         * 填充多边形
         * @param paint 绘图对象
         * @param color 颜色
         * @param points   点的集合
         */
        void fillPolygon(FCPaint *paint, Long dwPenColor, FCPoint *apt, int cpt);
        void fillRect(FCPaint *paint, Long dwPenColor, const FCRect& rect);
        /**
         * 获取文字的大小
         * @param paint 绘图对象
         * @param text  文字
         * @param font  字体
         *  @returns  大小
         */
        FCSize textSize(FCPaint *paint, const wchar_t *strText, FCFont *font);
    protected:
        /**
         * 移动画线
         * @param touchY    纵坐标
         * @param startY    开始纵坐标
         * @param startIndex  开始索引
         * @param touchBeginIndex   触摸开始索引
         * @param touchEndIndex     触摸结束索引
         * @param pureV 纵向距离
         * @param max   最大值
         * @param min   最小值
         * @param dataCount 数据条数
         * @param yAddValue 纵向变化值
         * @param newIndex  新的索引
         */
        void movePlot(float touchY, float startY, int startIndex, int touchBeginIndex, int touchEndIndex, float pureV,
                      double max, double min, int dataCount, double *yAddValue, int *newIndex);
    protected:
        void clearMarks(HashMap<int,PlotMark*> *marks);
        FCNative* getNative();
        /**
         * 初始化一个点类型的通用方法
         * @param mp    坐标
         *  @returns  是否初始化成功
         */
        bool createPoint(const FCPoint& mp);
        /**
         * 初始化两个点类型的通用方法
         * @param mp    坐标
         *  @returns  是否初始化成功
         */
        bool create2PointsA(const FCPoint& mp);
        /**
         * 初始化两个点类型的通用方法
         * @param mp    坐标
         *  @returns  是否初始化成功
         */
        bool create2PointsB(const FCPoint& mp);
        /**
         * 初始两个K线点的方法
         * @param mp    坐标
         *  @returns  是否初始化成功
         */
        bool create2CandlePoints(const FCPoint& mp);
        /**
         * 初始化三个点类型的通用方法
         * @param mp    坐标
         *  @returns  是否初始化成功
         */
        bool create3Points(const FCPoint& mp);
        /**
         * 初始化对应K线上的点
         * @param pos   位置
         * @param index 索引
         * @param close 收盘价字段
         */
        void createCandlePoint(int pos, int index, int close);
        /**
         * 初始四个K线点的方法
         * @param mp    坐标
         *  @returns  是否初始化成功
         */
        bool create4CandlePoints(const FCPoint& mp);
        /**
         * 获取K线区域内最高价和最低价
         * @param pList 点阵集合
         *  @returns  幅度尺的参数
         */
        double* getCandleRange(HashMap<int,PlotMark*> *pList);
        FCPoint getTouchOverPoint();
        /**
         * 根据坐标获取索引
         * @param mp    坐标
         *  @returns  索引
         */
        int getIndex(const FCPoint& mp);
        /**
         * 获取直线的参数
         * @param markA 第一个点
         * @param markB 第二个点
         *  @returns  直线的参数
         */
        float* getLineParams(PlotMark *markA, PlotMark *markB);
        /**
         * 获取线性回归带的高低点偏离值
         * @param pList 点阵集合
         * @param param 直线参数
         *  @returns  高低点偏离值
         */
        double* getLRBandRange(HashMap<int,PlotMark*> *marks, float *param);
        /**
         * 获取线性回归的参数
         * @param marks 点阵集合
         *  @returns  点阵数组
         */
        float* getLRParams(HashMap<int,PlotMark*> *marks);
        /**
         * 获取移动坐标
         */
        FCPoint getMovingPoint();
        /**
         * 根据坐标获取数值
         * @param mp    坐标
         *  @returns  数值
         */
        double getNumberValue(const FCPoint& mp);
        /**
         * 获取偏移横坐标
         *  @returns  偏移横坐标
         */
        int getPx();
        /**
         * 获取偏移纵坐标
         *  @returns  偏移纵坐标
         */
        int getPy();
        /**
         * 根据两点获取矩形
         * @param markA 第一个点
         * @param markB 第二个点
         *  @returns  矩形对象
         */
        FCRect getRectangle(PlotMark *markA, PlotMark *markB);
        /**
         * 获取黄金分割线的直线参数
         * @param value1  值1
         * @param value2    值2
         *  @returns  黄金分割线的直线参数
         */
        float* goldenRatioParams(double value1, double value2);
        /**
         * 多条横线的选中方法
         * @param param 参数
         * @param length    长度
         *  @returns  是否选中
         */
        bool hLinesSelect(float *param, int length);
        /**
         * 移动线条
         * @param mp    坐标
         */
        void move(const FCPoint& mp);
        /**
         * 绘图对象
         * @param paint 绘图对象
         */
        virtual void onPaint(FCPaint *paint);
        /**
         * 绘制图像的残影
         */
        virtual void onPaintGhost(FCPaint *paint);
        /**
         * 绘制图像的方法
         * @param paint 绘图对象
         * @param pList 横纵值描述
         * @param lineColor 颜色
         */
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
        /**
         * 获取绘制横坐标
         * @param index 索引
         *  @returns  横坐标
         */
        float pX(int index);
        /**
         * 获取绘制纵坐标
         * @param value 值
         *  @returns  纵坐标
         */
        float pY(double value);
        /**
         * 根据横坐标获取画线工具内部横坐标
         * @param x 横坐标
         *  @returns  内部横坐标
         */
        float pX(float x);
        /**
         * 根据横坐标获取画线工具内部横坐标
         * @param y     纵坐标
         *  @returns  内部纵坐标
         */
        float pY(float y);
        /**
         * 调整大小
         * @param index 索引
         */
        void resize(int index);
        /**
         * 重新调整大小
         * @param mp    坐标
         * @param oppositePoint 点位置
         */
        void resize(const FCPoint& mp, FCPoint oppositePoint);
        /**
         * 判断是否选中了指定的点
         * @param mp    坐标
         * @param x     点的横坐标
         * @param y     点的纵坐标
         *  @returns      是否选中
         */
        bool selectPoint(const FCPoint& mp, float x, float y);
        /**
         * 判断是否选中线
         * @param mp    点的位置
         * @param x     横坐标
         * @param k     直线参数k
         * @param b     直线参数b
         *  @returns      是否选中线
         */
        bool selectLine(const FCPoint& mp, float x, float k, float b);
        /**
         * 判断是否选中线
         * @param mp    点的位置
         * @param x1    第一个点的横坐标
         * @param y1    第一个点的纵坐标
         * @param x2    第二个点的横坐标
         * @param y2    第二个点的纵坐标
         */
        bool selectLine(const FCPoint& mp, float x1, float y1, float x2, float y2);
        /**
         * 判断是否选中射线
         * @param mp    点的位置
         * @param x1    第一个点的横坐标
         * @param y1    第一个点的纵坐标
         * @param x2    第二个点的横坐标
         * @param y2    第二个点的纵坐标
         * @param k     直线参数k
         * @param b     直线参数b
         *  @returns      是否选中射线
         */
        bool selectRay(const FCPoint& mp, float x1, float y1, float x2, float y2, float *pk, float *pb);
        /**
         * 判断是否选中射线
         * @param mp    点的位置
         * @param x1    第一个点的横坐标
         * @param y1    第一个点的纵坐标
         * @param x2    第二个点的横坐标
         * @param y2    第二个点的纵坐标
         *  @returns      是否选中射线
         */
        bool selectRay(const FCPoint& mp, float x1, float y1, float x2, float y2);
        /**
         * 获取选中状态
         * @param mp    点的位置
         * @param markA 点A
         * @param markB 点B
         *  @returns      选中状态
         */
        ActionType selectRect(const FCPoint& mp, PlotMark *markA, PlotMark *markB);
        /**
         * 判断是否选中线段
         * @param mp    点的位置
         * @param x1    第一个点的横坐标
         * @param y1    第一个点的纵坐标
         * @param x2    第二个点的横坐标
         * @param y2    第二个点的纵坐标
         *  @returns      是否选中线段
         */
        bool selectSegment(const FCPoint& mp, float x1, float y1, float x2, float y2);
        /**
         * 判断是否选中正弦线
         * @param mp    点的位置
         * @param x1    第一个点的横坐标
         * @param y1    第一个点的纵坐标
         * @param x2    第二个点的横坐标
         * @param y2    第二个点的纵坐标
         *  @returns      是否选中正弦线
         */
        bool selectSine(const FCPoint& mp,float x1, float y1, float x2, float y2);
        /**
         * 判断是否选中三角形
         * @param mp    点的位置
         * @param x1    第一个点的横坐标
         * @param y1    第一个点的纵坐标
         * @param x2    第二个点的横坐标
         * @param y2    第二个点的纵坐标
         * @param x3    第三个点的横坐标
         * @param y3    第三个点的纵坐标
         *  @returns      是否选中三角形
         */
        bool selectTriangle(const FCPoint& mp, float x1, float y1, float x2, float y2, float x3, float y3);
        /*
         * 设置光标
         * @param cursor 光标
         */
        void setCursor(FCCursors cursor);
    public:
        /*
         * 创建画线
         */
        FCPlot();
        /*
         * 销毁画线
         */
        virtual ~FCPlot();
        /**
         * 获取附着在左轴还是右轴
         */
        virtual AttachVScale getAttachVScale();
        /**
         * 置附着在左轴还是右轴
         */
        virtual void setAttachVScale(AttachVScale attachVScale);
        /**
         * 获取线的颜色
         */
        virtual Long getColor();
        /**
         * 设置线的颜色
         */
        virtual void setColor(Long color);
        /**
         * 获取所在图层
         */
        virtual ChartDiv* getDiv();
        /**
         * 设置所在图层
         */
        virtual void setDiv(ChartDiv *div);
        /**
         * 获取是否画移动残影
         */
        virtual bool drawGhost();
        /**
         * 设置是否画移动残影
         */
        virtual void setDrawGhost(bool drawGhost);
        /**
         * 获取是否可以选中或拖放
         */
        virtual bool isEnabled();
        /**
         * 设置是否可以选中或拖放
         */
        virtual void setEnabled(bool enabled);
        /**
         * 获取字体
         */
        virtual FCFont* getFont();
        /**
         * 设置字体
         */
        virtual void setFont(FCFont *font);
        /**
         * 设置是否已被销毁
         */
        virtual FCChart* getChart();
        /**
         * 获取线的样式，null为实线
         */
        virtual int getLineStyle();
        /**
         * 设置线的样式，null为实线
         */
        virtual void setLineStyle(int lineStyle);
        /**
         * 获取线的宽度
         */
        virtual int getLineWidth();
        /**
         * 设置线的宽度
         */
        virtual void setLineWidth(int lineWidth);
        /**
         * 获取线条的类型
         */
        virtual String getPlotType();
        /**
         * 设置线条的类型
         */
        virtual void setPlotType(const String& plotType);
        /**
         * 获取是否被选中
         */
        virtual bool isSelected();
        /**
         * 设置是否被选中
         */
        virtual void setSelected(bool selected);
        /**
         * 获取选中色
         */
        virtual Long getSelectedColor();
        /**
         * 设置选中色
         */
        virtual void setSelectedColor(Long selectedColor);
        /**
         * 获取选中点的样式
         */
        virtual enum SelectPoint getSelectedPoint();
        /**
         * 设置选中点的样式
         */
        virtual void setSelectedPoint(enum SelectPoint selectedPoint);
        /**
         * 获取显示的文字
         */
        virtual String getText();
        /**
         * 获取显示的文字
         */
        virtual void setText(const String& text);
        /**
         * 获取可见度
         */
        virtual bool isVisible();
        /**
         * 设置可见度
         */
        virtual void setVisible(bool visible);
        /**
         * 获取区域宽度
         *  @returns  宽度
         */
        virtual int getWorkingAreaWidth();
        /**
         * 获取区域高度
         *  @returns  高度
         */
        virtual int getWorkingAreaHeight();
        /**
         * 获取图层顺序
         */
        virtual int getZOrder();
        /**
         * 设置图层顺序
         */
        virtual void setZOrder(int zOrder);
    public:
        /**
         * 获取动作
         *  @returns 动作类型
         */
        virtual ActionType getAction();
        /**
         * 初始化线条
         * @param mp    坐标
         *  @returns  是否初始化成功
         */
        virtual bool onCreate(const FCPoint& mp);
        /*
         * 移动开始
         */
        virtual void onMoveBegin();
        /**
         * 移动结束
         */
        virtual void onMoveEnd();
        /**
         * 移动开始
         */
        virtual void onMoveStart();
        /**
         * 移动
         */
        virtual void onMoving();
        /**
         * 判断选中
         *  @returns  是否选中
         */
        virtual bool onSelect();
        /**
         * 绘制到图像上
         * @param paint 绘图对象
         */
        virtual void render(FCPaint *paint);
    public:
        /*
         * 计算椭圆的参数
         */
        static void ellipseAB(float width,  float height,  float *a,  float *b);
        /*
         * 判断点是否在·椭圆上
         */
        static bool ellipseHasPoint(float x, float y, float oX, float oY, float a, float b);
        /*
         * 根据三个点算圆心
         */
        static void ellipseOR(float x1, float y1, float x2, float y2, float x3, float y3, float *oX, float *oY, float *r);
        /*
         * 获取直线的斜率
         */
        static double lineSlope(float x1,  float y1,  float x2,  float y2,  float oX,  float oY);
        /*
         * 获取直线的参数
         */
        static void lineXY(float x1,  float y1,  float x2,  float y2,  float oX,  float oY,  float *k,  float *b);
        /*
         * 计算平行四边形第四个店
         */
        static void parallelogram(float x1, float y1, float x2, float y2, float x3, float y3, float *x4, float *y4);
        /*
         * 根据两个点计算矩形
         */
        static void rectangleXYWH(int x1, int y1, int x2, int y2, int *x, int *y, int *w, int *h);
    };
}
#endif
