/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __PEXTEND_H__
#define __PEXTEND_H__
#pragma once
#include "stdafx.h"
#include "FCPlot.h"
#include "FCPlot.h"

namespace FaceCat{
    class FCPlot;
    
    class AndrewSpitchfork:public FCPlot{
    public:
        /**
         * 安德鲁斯干草叉
         */
        AndrewSpitchfork();
        /**
         * 获取动作类型
         */
        virtual ActionType getAction();
        /**
         * 初始化线条
         */
        virtual bool onCreate(const FCPoint& mp);
        /**
         * 开始移动画线工具
         */
        virtual void onMoveStart();
        /**
         * 绘制图像的方法
         */
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class AngleLine:public FCPlot{
    public:
        /**
         * 创建角度线
         */
        AngleLine();
        /**
         * 获取动作类型
         */
        virtual ActionType getAction();
        /**
         * 初始化线条
         */
        virtual bool onCreate(const FCPoint& mp);
        /**
         * 开始移动画线工具
         */
        virtual void onMoveStart();
        /**
         * 绘制图像的方法
         */
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class CircumCircle:public FCPlot{
    public:
        /**
         * 外接圆
         */
        CircumCircle();
        /**
         * 获取动作类型
         */
        virtual ActionType getAction();
        /**
         * 初始化线条
         */
        virtual bool onCreate(const FCPoint& mp);
        /**
         * 开始移动画线工具
         */
        virtual void onMoveStart();
        /**
         * 绘制图像的方法
         */
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class ArrowSegment:public FCPlot{
    public:
        /**
         * 创建箭头线段
         */
        ArrowSegment();
        /**
         * 获取动作类型
         */
        virtual ActionType getAction();
        /**
         * 初始化线条
         */
        virtual bool onCreate(const FCPoint& mp);
        /**
         * 开始移动画线工具
         */
        virtual void onMoveStart();
        /**
         * 绘制图像的方法
         */
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    class DownArrow:public FCPlot{
    protected:
        FCRect getDownArrowRect(float x, float y, float width);
    public:
        /**
         * 创建下跌箭头
         */
        DownArrow();
        /**
         * 获取动作类型
         */
        virtual ActionType getAction();
        /**
         * 初始化线条
         */
        virtual bool onCreate(const FCPoint& mp);
        /**
         * 开始移动画线工具
         */
        virtual void onMoveStart();
        /**
         * 绘制图像的方法
         */
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    class DropLine:public FCPlot{
    protected:
        /**
         *获取参数
         */
        float* getDropLineParams(HashMap<int, PlotMark*> *pList);
    public:
        /**
         * 创建下降45度线
         */
        DropLine();
        /**
         * 获取动作类型
         */
        virtual ActionType getAction();
        /**
         * 初始化线条
         */
        virtual bool onCreate(const FCPoint& mp);
        /**
         * 开始移动画线工具
         */
        virtual void onMoveStart();
        /**
         * 绘制图像的方法
         */
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class Ellipse:public FCPlot{
    public:
        /**
         * 椭圆
         */
        Ellipse();
        /**
         * 获取动作类型
         */
        virtual ActionType getAction();
        /**
         * 初始化线条
         */
        virtual bool onCreate(const FCPoint& mp);
        /**
         * 开始移动画线工具
         */
        virtual void onMoveStart();
        /**
         * 绘制图像的方法
         */
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class FiboEllipse:public FCPlot{
    protected:
        /**
         * 斐波圆的参数
         */
        float* fibonacciEllipseParam(HashMap<int, PlotMark*> *pList);
    public:
        /**
         * 创建斐波圆
         */
        FiboEllipse();
        /**
         * 获取动作类型
         */
        virtual ActionType getAction();
        /**
         * 初始化图形
         */
        virtual bool onCreate(const FCPoint& mp);
        /**
         * 开始移动画线工具
         */
        virtual void onMoveStart();
        /**
         * 绘制图像的残影
         */
        virtual void onPaintGhost(FCPaint *paint);
        /**
         * 绘制图像的方法
         */
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class FiboFanline:public FCPlot{
    public:
        /**
         * 斐波扇面
         */
        FiboFanline();
        /**
         * 获取动作类型
         */
        virtual ActionType getAction();
        /**
         * 初始化线条
         */
        virtual bool onCreate(const FCPoint& mp);
        /**
         * 开始移动画线工具
         */
        virtual void onMoveStart();
        /**
         * 绘制图像的残影
         */
        virtual void onPaintGhost(FCPaint *paint);
        /**
         * 绘制图像的方法
         */
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class FiboTimeZone:public FCPlot{
    protected:
        /**
         * 获取斐波周期线的参数
         */
        ArrayList<int> getFibonacciTimeZonesParam(HashMap<int,PlotMark*> *pList);
    public:
        /**
         * 创建斐波周期线
         */
        FiboTimeZone();
        /**
         * 获取动作类型
         */
        virtual ActionType getAction();
        /**
         * 初始化线条
         */
        virtual bool onCreate(const FCPoint& mp);
        /**
         * 开始移动画线工具
         */
        virtual void onMoveStart();
        /**
         * 绘制图像的方法
         */
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class GannBox:public FCPlot{
    protected:
        FCPoint m_oppositePoint;
    public:
        /**
         * 甘氏箱
         */
        GannBox();
        /**
         * 获取动作类型
         */
        virtual ActionType getAction();
        /**
         * 获取选中状态
         */
        ActionType getClickStatus();
        /**
         * 获取江恩箱的重要点
         */
        FCPoint* getGannBoxPoints(float x1, float y1, float x2, float y2);
        /**
         * 初始化线条
         */
        virtual bool onCreate(const FCPoint& mp);
        /**
         * 开始移动画线工具
         */
        virtual void onMoveStart();
        /**
         * 移动线条
         */
        virtual void onMoving();
        /**
         * 绘制图像的残影
         */
        virtual void onPaintGhost(FCPaint *paint);
        /**
         * 绘制图像的方法
         */
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class GannLine:public FCPlot{
    public:
        /**
         * 创建甘氏线
         */
        GannLine();
        virtual ActionType getAction();
        virtual bool onCreate(const FCPoint& mp);
        virtual void onMoveStart();
        virtual void onPaintGhost(FCPaint *paint);
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class GoldenRatio:public FCPlot{
    public:
        /**
         * 黄金分割线
         */
        GoldenRatio();
        virtual ActionType getAction();
        virtual bool onCreate(const FCPoint& mp);
        virtual void onMoveStart();
        virtual void onMoving();
        virtual void onPaintGhost(FCPaint *paint);
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class HLine:public FCPlot{
    public:
        /**
         * 创建水平线
         */
        HLine();
        virtual ActionType getAction();
        virtual bool onCreate(const FCPoint& mp);
        virtual void onMoveStart();
        virtual void onMoving();
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class LevelGrading:public FCPlot{
    protected:
        /**
         * 获取高低推档的直线参数
         */
        float* levelGradingParams(double value1, double value2);
    public:
        /**
         * 创建高低推档
         */
        LevelGrading();
        virtual ActionType getAction();
        virtual bool onCreate(const FCPoint& mp);
        virtual void onMoveStart();
        virtual void onMoving();
        virtual void onPaintGhost(FCPaint *paint);
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class Line:public FCPlot{
    public:
        /**
         * 创建直线
         */
        Line();
        virtual ActionType getAction();
        virtual bool onCreate(const FCPoint& mp);
        virtual void onMoveStart();
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class LrBand:public FCPlot{
    public:
        /**
         * 创建线性回归带
         */
        LrBand();
        virtual ActionType getAction();
        virtual bool onCreate(const FCPoint& mp);
        virtual void onMoveStart();
        virtual void onMoving();
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class LrChannel:public FCPlot{
    public:
        /**
         * 创建回归通道
         */
        LrChannel();
        virtual ActionType getAction();
        virtual bool onCreate(const FCPoint& mp);
        virtual void onMoveStart();
        virtual void onMoving();
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class LrLine:public FCPlot{
    public:
        /**
         * 创建线性回归
         */
        LrLine();
        virtual ActionType getAction();
        virtual bool onCreate(const FCPoint& mp);
        virtual void onMoveStart();
        virtual void onMoving();
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class NullPoint:public FCPlot{
    protected:
        /**
         * 创建解消点
         */
        void nullPoint(float x1, float y1, float x2, float y2, float *nullX, float *nullY);
        /**
         * 获取解消点的参数
         */
        double* getNullPointParams(HashMap<int,PlotMark*> *pList);
    public:
        /**
         * 创建解消点
         */
        NullPoint();
        virtual ActionType getAction();
        virtual bool onCreate(const FCPoint& mp);
        virtual void onMoveStart();
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class Parallel:public FCPlot{
    protected:
        /**
         * 获取平行直线的参数
         */
        float* getParallelParams(HashMap<int,PlotMark*> *pList);
    public:
        /**
         * 创建平行线
         */
        Parallel();
        virtual ActionType getAction();
        virtual bool onCreate(const FCPoint& mp);
        virtual void onMoveStart();
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class Percent:public FCPlot{
    protected:
        float* getPercentParams(double value1, double value2);
    public:
        /**
         * 创建百分比线
         */
        Percent();
        virtual ActionType getAction();
        virtual bool onCreate(const FCPoint& mp);
        virtual void onMoveStart();
        virtual void onMoving();
        virtual void onPaintGhost(FCPaint *paint);
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class Periodic:public FCPlot{
    protected:
        int m_period;
        int m_beginPeriod;
    public:
        /**
         * 创建周期线
         */
        Periodic();
        virtual ActionType getAction();
        ArrayList<double> getPLParams(HashMap<int,PlotMark*> *pList);
        virtual bool onCreate(const FCPoint& mp);
        virtual void onMoveStart();
        virtual void onMoving();
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class Price:public FCPlot{
    protected:
        FCSize m_textSize;
    public:
        /**
         * 创建价格签条
         */
        Price();
        virtual ActionType getAction();
        virtual bool onCreate(const FCPoint& mp);
        virtual void onMoveStart();
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class RangeRuler:public FCPlot{
    public:
        /**
         * 创建幅度尺
         */
        RangeRuler();
        virtual ActionType getAction();
        virtual bool onCreate(const FCPoint& mp);
        virtual void onMoveStart();
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class RaseLine:public FCPlot{
    public:
        /**
         * 创建上升45度线
         */
        RaseLine();
        float* getRaseLineParams(HashMap<int,PlotMark*> *pList);
        virtual ActionType getAction();
        virtual bool onCreate(const FCPoint& mp);
        virtual void onMoveStart();
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class Ray:public FCPlot{
    public:
        /**
         * 创建射线
         */
        Ray();
        virtual ActionType getAction();
        virtual bool onCreate(const FCPoint& mp);
        virtual void onMoveStart();
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class RectLine:public FCPlot{
    protected:
        FCPoint m_oppositePoint;
    public:
        RectLine();
        virtual ActionType getAction();
        virtual bool onCreate(const FCPoint& mp);
        virtual void onMoveStart();
        virtual void onMoving();
        void onPaintGhost(FCPaint *paint);
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class Segment:public FCPlot{
    public:
        /**
         * 创建线段
         */
        Segment();
        virtual ActionType getAction();
        virtual bool onCreate(const FCPoint& mp);
        virtual void onMoveStart();
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class Sine:public FCPlot{
    public:
        /**
         * 创建正弦线
         */
        Sine();
        virtual ActionType getAction();
        virtual bool onCreate(const FCPoint& mp);
        virtual void onMoveStart();
        virtual void onMoving();
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class SpeedResist:public FCPlot{
    public:
        /**
         * 创建速阻线
         */
        SpeedResist();
        virtual ActionType getAction();
        virtual bool onCreate(const FCPoint& mp);
        virtual void onMoveStart();
        virtual void onPaintGhost(FCPaint *paint);
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class SeChannel:public FCPlot{
    protected:
        double getSEChannelSD(HashMap<int,PlotMark*> *pList);
    public:
        /**
         * 创建标准误差通道
         */
        SeChannel();
        virtual ActionType getAction();
        virtual bool onCreate(const FCPoint& mp);
        virtual void onMoveStart();
        virtual void onMoving();
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class SymmetricLine:public FCPlot{
    public:
        /**
         * 创建对称线
         */
        SymmetricLine();
        virtual ActionType getAction();
        virtual bool onCreate(const FCPoint& mp);
        virtual void onMoveStart();
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class SymmetricTriangle:public FCPlot{
    protected:
        float* getSymmetricTriangleParams(HashMap<int,PlotMark*> *pList);
    public:
        SymmetricTriangle();
        virtual ActionType getAction();
        virtual bool onCreate(const FCPoint& mp);
        virtual void onMoveStart();
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class TimeRuler:public FCPlot{
    protected:
        double* getTimeRulerParams(HashMap<int,PlotMark*> *pList);
    public:
        /**
         * 创建时间尺
         */
        TimeRuler();
        virtual ActionType getAction();
        virtual bool onCreate(const FCPoint& mp);
        virtual void onMoveStart();
        virtual void onMoving();
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class Triangle:public FCPlot{
    public:
        /**
         * 创建三角形
         */
        Triangle();
        virtual ActionType getAction();
        virtual bool onCreate(const FCPoint& mp);
        virtual void onMoveStart();
        virtual void onPaintGhost(FCPaint *paint);
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    class UpArrow:public FCPlot{
    protected:
        FCRect getUpArrowRect(float x, float y, float width);
    public:
        /**
         * 创建上涨箭头
         */
        UpArrow();
        virtual ActionType getAction();
        virtual bool onCreate(const FCPoint& mp);
        virtual void onMoveStart();
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class VLine:public FCPlot{
    public:
        /**
         * 创建垂直线
         */
        VLine();
        virtual ActionType getAction();
        virtual bool onCreate(const FCPoint& mp);
        virtual void onMoveStart();
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class WaveRuler:public FCPlot{
    protected:
        float* getWaveRulerParams(double value1, double value2);
    public:
        /**
         * 创建波浪尺
         */
        WaveRuler();
        virtual ActionType getAction();
        virtual bool onCreate(const FCPoint& mp);
        virtual void onMoveStart();
        virtual void onPaintGhost(FCPaint *paint);
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class TironeLevels:public FCPlot{
    protected:
        double* getTironelLevelsParams(HashMap<int,PlotMark*> *pList);
    public:
        /**
         * 创建泰龙水平线
         */
        TironeLevels();
        virtual ActionType getAction();
        virtual bool onCreate(const FCPoint& mp);
        virtual void onMoveStart();
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class RaffChannel:public FCPlot{
    protected:
        double getRRCRange(HashMap<int,PlotMark*> *pList, float *param);
    public:
        /**
         * 创建拉弗回归通道
         */
        RaffChannel();
        virtual ActionType getAction();
        virtual bool onCreate(const FCPoint& mp);
        virtual void onMoveStart();
        virtual void onMoving();
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class QuadrantLines:public Percent{
    public:
        /**
         * 创建四等分线
         */
        QuadrantLines();
        virtual ActionType getAction();
        virtual bool onCreate(const FCPoint& mp);
        virtual void onMoveStart();
        virtual void onMoving();
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class BoxLine:public RectLine{
    protected:
        ActionType getAction(const FCPoint& mp);
    public:
        /**
         * 创建箱形线
         */
        BoxLine();
        virtual ActionType getAction();
        virtual bool onCreate(const FCPoint& mp);
        virtual void onMoveStart();
        virtual void onMoving();
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class ParallelGram:public FCPlot{
    public:
        ParallelGram();
        virtual ActionType getAction();
        FCPoint* getPLPoints(HashMap<int,PlotMark*> *pList);
        virtual bool onCreate(const FCPoint& mp);
        virtual void onMoveStart();
        virtual void onPaintGhost(FCPaint *paint);
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class Circle:public FCPlot{
    public:
        /**
         * 创建圆
         */
        Circle();
        virtual ActionType getAction();
        virtual bool onCreate(const FCPoint& mp);
        virtual void onMoveStart();
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class PriceChannel:public Parallel{
    protected:
        void getLine3Params(HashMap<int,PlotMark*> *pList, float *k, float *d, float *x4);
        void paintEx(FCPaint *paint, HashMap<int,PlotMark*> *pList,Long lineColor);
    public:
        /**
         * 创建价格通道线
         */
        PriceChannel();
        virtual ActionType getAction();
        virtual void onMoveStart();
        virtual void onPaint(FCPaint *paint);
        virtual void onPaintGhost(FCPaint *paint);
    };
    
    class Gp:public FCPlot{
    public:
        /**
         * 创建黄金分割价位线
         */
        Gp();
        virtual ActionType getAction();
        virtual bool onCreate(const FCPoint& mp);
        virtual void onMoveStart();
        virtual void onMoving();
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class Ga:public Triangle{
    protected:
        float* getGoldenRatioAimParams(HashMap<int,PlotMark*> *pList);
    public:
        /**
         * 创建黄金分割目标线
         */
        Ga();
        virtual ActionType getAction();
        virtual void onMoveStart();
        virtual void onPaint(FCPaint *paint, HashMap<int,PlotMark*> *pList, Long lineColor);
    };
    
    class PFactory{
    public:
        /*
         * 创建线条
         */
        static FCPlot* createPlot(const String& plotType);
    };
}
#endif
