/*****************************************************************************\
*                                                                             *
* PlotBase.cs -  Plot functions, types, and definitions                       *
*                                                                             *
*               Version 4.00 ★★★★★                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's OwChart. All rights reserved. *
*                                                                             *
*******************************************************************************/

using System;
using System.Collections.Generic;

namespace OwLib
{
    /// <summary>
    /// 动作类型
    /// </summary>
    public enum ActionType
    {
        /// <summary>
        /// 第一个点的动作
        /// </summary>
        AT1 = 1,
        /// <summary>
        /// 第二个点的动作
        /// </summary>
        AT2 = 2,
        /// <summary>
        /// 第三个点的动作
        /// </summary>
        AT3 = 3,
        /// <summary>
        /// 第四个点的动作
        /// </summary>
        AT4 = 4,
        /// <summary>
        /// 移动
        /// </summary>
        MOVE = 0,
        /// <summary>
        /// 不移动
        /// </summary>
        NO = -1
    }

    /// <summary>
    /// 各点值的集合
    /// </summary>
    public class PlotMark
    {
        /// <summary>
        /// 创建一个点值
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public PlotMark(int index, double key, double value)
        {
            Index = index;
            Key = key;
            Value = value;
        }
        /// <summary>
        /// 索引
        /// </summary>
        public int Index;
        /// <summary>
        /// 键
        /// </summary>
        public double Key;
        /// <summary>
        /// 值
        /// </summary>
        public double Value;
    }

    /// <summary>
    /// 画线工具的基类，Lord2012/7/1
    /// </summary>
    public class PlotBase
    {
        #region Lord  2012/7/1
        /// <summary>
        /// 创建画线
        /// </summary>
        public PlotBase()
        {
            m_color = COLOR.ARGB(255, 255, 255);
            m_selectedColor = COLOR.ARGB(255, 255, 255);
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~PlotBase()
        {
            Dispose();
        }

        /// <summary>
        /// 移动类型
        /// </summary>
        protected ActionType m_action = ActionType.NO;

        /// <summary>
        /// 数据源
        /// </summary>
        protected CTable m_dataSource;

        /// <summary>
        /// 是否正在绘制阴影
        /// </summary>
        protected bool m_isPaintingGhost;

        /// <summary>
        /// 关键点
        /// </summary>
        protected Dictionary<int, PlotMark> m_marks = new Dictionary<int, PlotMark>();

        /// <summary>
        /// 移动次数
        /// </summary>
        protected int m_moveTimes = 0;

        /// <summary>
        /// 源字段
        /// </summary>
        protected Dictionary<String, int> m_sourceFields = new Dictionary<String, int>();

        /// <summary>
        /// 开始移动时的值
        /// </summary>
        protected Dictionary<int, PlotMark> m_startMarks = new Dictionary<int, PlotMark>();

        /// <summary>
        /// 开始移动的点
        /// </summary>
        protected POINT m_startPoint = new POINT();

        protected AttachVScale m_attachVScale = AttachVScale.Left;

        /// <summary>
        /// 获取或设置附着在左轴还是右轴
        /// </summary>
        public AttachVScale AttachVScale
        {
            get { return m_attachVScale; }
            set { m_attachVScale = value; }
        }

        protected long m_color;

        /// <summary>
        /// 获取或设置线的颜色
        /// </summary>
        public long Color
        {
            get { return m_color; }
            set { m_color = value; }
        }

        /// <summary>
        /// 设置鼠标形状
        /// </summary>
        protected CursorsA Cursor
        {
            get { return m_div.Chart.Cursor; }
            set { m_div.Chart.Cursor = value; }
        }

        protected CDiv m_div;

        /// <summary>
        /// 获取或设置所在图层
        /// </summary>
        public CDiv Div
        {
            get { return m_div; }
            set
            {
                m_div = value;
                m_dataSource = m_div.Chart.DataSource;
            }
        }

        protected bool m_drawGhost = true;

        /// <summary>
        /// 获取或设置是否画移动残影
        /// </summary>
        public bool DrawGhost
        {
            get { return m_drawGhost; }
            set { m_drawGhost = value; }
        }

        protected bool m_enabled = true;

        /// <summary>
        /// 获取或设置是否可以选中或拖放
        /// </summary>
        public bool Enabled
        {
            get { return m_enabled; }
            set
            {
                if (!value)
                {
                    m_selected = false;
                }
                m_enabled = value;
            }
        }

        protected FONT m_font = new FONT();

        /// <summary>
        /// 获取或设置字体
        /// </summary>
        public FONT Font
        {
            get { return m_font; }
            set { m_font = value; }
        }

        private bool m_isDisposed;

        /// <summary>
        /// 获取或设置是否已被销毁
        /// </summary>
        public virtual bool IsDisposed
        {
            get { return m_isDisposed; }
        }

        /// <summary>
        /// 获取所在布局
        /// </summary>
        public ChartA Chart
        {
            get { return m_div.Chart; }
        }

        protected int m_lineStyle = 0;

        /// <summary>
        /// 获取或设置线的样式，null为实线
        /// </summary>
        public int LineStyle
        {
            get { return m_lineStyle; }
            set { m_lineStyle = value; }
        }

        protected int m_lineWidth = 1;

        /// <summary>
        /// 获取或设置线的宽度
        /// </summary>
        public int LineWidth
        {
            get { return m_lineWidth; }
            set { m_lineWidth = value; }
        }

        /// <summary>
        /// 获取方法库
        /// </summary>
        protected NativeBase Native
        {
            get { return m_div.Chart.Native; }
        }

        protected String m_plotType = "LINE";

        /// <summary>
        /// 获取或设置线条的类型
        /// </summary>
        public String PlotType
        {
            get { return m_plotType; }
            set { m_plotType = value; }
        }

        protected bool m_selected;

        /// <summary>
        /// 获取或设置是否被选中
        /// </summary>
        public bool Selected
        {
            get { return m_selected; }
            set { m_selected = value; }
        }

        protected long m_selectedColor;

        /// <summary>
        /// 获取或设置选中色
        /// </summary>
        public long SelectedColor
        {
            get { return m_selectedColor; }
            set { m_selectedColor = value; }
        }

        protected SelectedPoint m_selectedPoint = SelectedPoint.Rectangle;

        /// <summary>
        /// 获取或设置选中点的样式
        /// </summary>
        public SelectedPoint SelectedPoint
        {
            get { return m_selectedPoint; }
            set { m_selectedPoint = value; }
        }

        protected String m_text;

        /// <summary>
        /// 获取或设置显示的文字
        /// </summary>
        public String Text
        {
            get { return m_text; }
            set { m_text = value; }
        }

        protected bool m_visible = true;

        /// <summary>
        /// 获取或设置可见度
        /// </summary>
        public bool Visible
        {
            get { return m_visible; }
            set
            {
                if (!value)
                {
                    m_selected = false;
                }
                m_visible = value;
            }
        }

        /// <summary>
        /// 获取区域宽度
        /// </summary>
        /// <returns>宽度</returns>
        protected int WorkingAreaWidth
        {
            get { return Chart.WorkingAreaWidth; }
        }

        /// <summary>
        /// 获取区域高度
        /// </summary>
        /// <returns>高度</returns>
        protected int WorkingAreaHeight
        {
            get { return m_div.WorkingAreaHeight; }
        }

        private int m_zOrder = 0;

        /// <summary>
        /// 获取或设置图层顺序
        /// </summary>
        public int ZOrder
        {
            get { return m_zOrder; }
            set { m_zOrder = value; }
        }

        /// <summary>
        /// 初始化一个点类型的通用方法
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        protected bool CreatePoint(POINT mp)
        {
            int rIndex = m_dataSource.RowsCount;
            if (rIndex > 0)
            {
                //获取点的位置
                ChartA chart = Chart;
                int mouseIndex = chart.GetIndex(mp);
                if (mouseIndex >= chart.FirstVisibleIndex && mouseIndex <= chart.LastVisibleIndex)
                {
                    double sDate = m_dataSource.GetXValue(mouseIndex);
                    m_marks.Clear();
                    double y = GetNumberValue(mp);
                    m_marks[0] = new PlotMark(0, sDate, y);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 初始化两个点类型的通用方法
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        protected bool Create2PointsA(POINT mp)
        {
            int rIndex = m_dataSource.RowsCount;
            if (rIndex > 0)
            {
                //获取点的位置
                ChartA chart = Chart;
                int mouseIndex = chart.GetIndex(mp);
                if (mouseIndex >= chart.FirstVisibleIndex && mouseIndex <= chart.LastVisibleIndex)
                {
                    int eIndex = mouseIndex;
                    int bIndex = eIndex - 1;
                    if (bIndex >= 0)
                    {
                        double fDate = m_dataSource.GetXValue(bIndex);
                        double sDate = m_dataSource.GetXValue(eIndex);
                        m_marks.Clear();
                        double y = GetNumberValue(mp);
                        m_marks[0] = new PlotMark(0, fDate, y);
                        m_marks[1] = new PlotMark(1, sDate, y);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 初始化两个点类型的通用方法
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        protected bool Create2PointsB(POINT mp)
        {
            int rIndex = m_dataSource.RowsCount;
            if (rIndex > 0)
            {
                ChartA chart = Chart;
                int mouseIndex = chart.GetIndex(mp);
                if (mouseIndex >= chart.FirstVisibleIndex && mouseIndex <= chart.LastVisibleIndex)
                {
                    double date = m_dataSource.GetXValue(mouseIndex);
                    m_marks.Clear();
                    double y = GetNumberValue(mp);
                    m_marks[0] = new PlotMark(0, date, y);
                    m_marks[1] = new PlotMark(1, date, y);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 初始两个K线点的方法
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        protected bool Create2CandlePoints(POINT mp)
        {
            ChartA chart = Chart;
            int rIndex = m_dataSource.RowsCount;
            if (rIndex > 0)
            {
                List<BaseShape> shapesCopy = m_div.GetShapes(SortType.DESC);
                foreach (BaseShape bs in shapesCopy)
                {
                    if (bs.Visible)
                    {
                        CandleShape cs = bs as CandleShape;
                        if (cs != null)
                        {
                            int mouseIndex = chart.GetIndex(mp);
                            if (mouseIndex >= chart.FirstVisibleIndex && mouseIndex <= chart.LastVisibleIndex)
                            {
                                int eIndex = mouseIndex;
                                int bIndex = eIndex - 1;
                                if (bIndex >= 0)
                                {
                                    double fDate = m_dataSource.GetXValue(bIndex);
                                    double sDate = m_dataSource.GetXValue(eIndex);
                                    m_marks.Clear();
                                    double y = GetNumberValue(mp);
                                    m_marks[0] = new PlotMark(0, fDate, y);
                                    m_marks[1] = new PlotMark(1, sDate, y);
                                    m_sourceFields["CLOSE"] = cs.CloseField;
                                    m_sourceFields["OPEN"] = cs.OpenField;
                                    m_sourceFields["HIGH"] = cs.HighField;
                                    m_sourceFields["LOW"] = cs.LowField;
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 初始化三个点类型的通用方法
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        protected bool Create3Points(POINT mp)
        {
            int rIndex = m_dataSource.RowsCount;
            if (rIndex > 0)
            {
                //获取点的位置
                ChartA chart = Chart;
                int mouseIndex = chart.GetIndex(mp);
                if (mouseIndex >= chart.FirstVisibleIndex && mouseIndex <= chart.LastVisibleIndex)
                {
                    int eIndex = mouseIndex;
                    int bIndex = eIndex - 1;
                    if (bIndex >= 0)
                    {
                        double fDate = m_dataSource.GetXValue(bIndex);
                        double sDate = m_dataSource.GetXValue(eIndex);
                        m_marks.Clear();
                        double y = GetNumberValue(mp);
                        m_marks[0] = new PlotMark(0, fDate, y);
                        m_marks[1] = new PlotMark(1, sDate, y);
                        if (m_div.GetVScale(m_attachVScale) != null && m_div.GetVScale(m_attachVScale).VisibleMax != m_div.GetVScale(m_attachVScale).VisibleMin)
                        {
                            m_marks[2] = new PlotMark(2, fDate, y + (m_div.GetVScale(m_attachVScale).VisibleMax - m_div.GetVScale(m_attachVScale).VisibleMin) / 4);
                        }
                        else
                        {
                            m_marks[2] = new PlotMark(2, fDate, y);
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 初始化对应K线上的点
        /// </summary>
        /// <param name="pos">位置</param>
        /// <param name="index">索引</param>
        /// <param name="close">收盘价字段</param>
        protected void CreateCandlePoint(int pos, int index, int close)
        {
            if (index >= 0)
            {
                if (index > m_dataSource.RowsCount - 1)
                {
                    index = m_dataSource.RowsCount - 1;
                }
                double date = m_dataSource.GetXValue(index);
                double yValue = 0;
                if (!double.IsNaN(m_dataSource.Get2(index, close)))
                {
                    yValue = m_dataSource.Get2(index, close);
                }
                m_marks[pos] = new PlotMark(pos, date, yValue);
            }
        }

        /// <summary>
        /// 初始四个K线点的方法
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        protected bool Create4CandlePoints(POINT mp)
        {
            ChartA chart = Chart;
            int rIndex = m_dataSource.RowsCount;
            if (rIndex > 0)
            {
                List<BaseShape> shapesCopy = m_div.GetShapes(SortType.ASC);
                foreach (BaseShape bs in shapesCopy)
                {
                    if (bs.Visible && bs is CandleShape)
                    {
                        CandleShape cs = bs as CandleShape;
                        int mouseIndex = chart.GetIndex(mp);
                        if (mouseIndex >= chart.FirstVisibleIndex && mouseIndex <= chart.LastVisibleIndex)
                        {
                            int closeField = cs.CloseField;
                            CreateCandlePoint(0, mouseIndex, closeField);
                            CreateCandlePoint(1, mouseIndex + 1, closeField);
                            CreateCandlePoint(2, mouseIndex + 2, closeField);
                            CreateCandlePoint(3, mouseIndex + 3, closeField);
                            m_sourceFields["CLOSE"] = closeField;
                            m_sourceFields["HIGH"] = cs.HighField;
                            m_sourceFields["LOW"] = cs.LowField;
                            m_sourceFields["OPEN"] = cs.OpenField;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        ///  销毁资源方法
        /// </summary>
        public virtual void Dispose()
        {
            if (!m_isDisposed)
            {
                if (m_marks != null)
                {
                    m_marks.Clear();
                }
                if (m_startMarks != null)
                {
                    m_startMarks.Clear();
                }
                m_isDisposed = true;
            }
        }

        /// <summary>
        /// 画椭圆
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="left">横坐标</param>
        /// <param name="top">纵坐标</param>
        /// <param name="right">右侧横坐标</param>
        /// <param name="bottom">右侧纵坐标</param>
        protected void DrawEllipse(CPaint paint, long dwPenColor, int width, int style, float left, float top, float right, float bottom)
        {
            left += GetPx();
            top += GetPy();
            right += GetPx();
            bottom += GetPy();
            RECT rect = new RECT(left, top, right, bottom);
            paint.DrawEllipse(dwPenColor, width, style, rect);
            if (paint.SupportTransparent())
            {
                ChartA chart = Chart;
                POINT mp = chart.MousePoint;
                if (!m_isPaintingGhost && (mp.y >= m_div.Top && mp.y <= m_div.Bottom) && 
                (chart.MovingPlot == this ||
                (chart == Native.HoveredControl 
                && !chart.IsOperating() && OnSelect())))
                {
                    int a = 0, r = 0, g = 0, b = 0;
                    COLOR.ToARGB(paint, dwPenColor, ref a, ref r, ref g, ref b);
                    if (a == 255)
                    {
                        a = 50;
                    }
                    dwPenColor = COLOR.ARGB(a, r, g, b);
                    width += 10;
                    paint.DrawEllipse(dwPenColor, width, 0, rect);
                }
            }
        }

        /// <summary>
        /// 画线方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="x1">第一个点的横坐标</param>
        /// <param name="y1">第一个点的纵坐标</param>
        /// <param name="x2">第二个点的横坐标</param>
        /// <param name="y2">第二个点的纵坐标</param>
        protected void DrawLine(CPaint paint, long dwPenColor, int width, int style, float x1, float y1, float x2, float y2)
        {
            x1 += GetPx();
            y1 += GetPy();
            x2 += GetPx();
            y2 += GetPy();
            paint.DrawLine(dwPenColor, width, style, (int)x1, (int)y1, (int)x2, (int)y2);
            if (paint.SupportTransparent())
            {
                ChartA chart = Chart;
                POINT mp = chart.MousePoint;
                if (!m_isPaintingGhost && (mp.y >= m_div.Top && mp.y <= m_div.Bottom) &&
                (chart.MovingPlot == this ||
                (chart == Native.HoveredControl
                && !chart.IsOperating() && OnSelect())))
                {
                    int a = 0, r = 0, g = 0, b = 0;
                    COLOR.ToARGB(paint, dwPenColor, ref a, ref r, ref g, ref b);
                    if (a == 255)
                    {
                        a = 50;
                    }
                    dwPenColor = COLOR.ARGB(a, r, g, b);
                    width += 10;
                    paint.DrawLine(dwPenColor, width, 0, (int)x1, (int)y1, (int)x2, (int)y2);
                }
            }
        }

        /// <summary>
        /// 画射线
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="x1">第一个点的横坐标</param>
        /// <param name="y1">第一个点的纵坐标</param>
        /// <param name="x2">第二个点的横坐标</param>
        /// <param name="y2">第二个点的纵坐标</param>
        /// <param name="k">直线参数k</param>
        /// <param name="b">直线参数b</param>
        protected void DrawRay(CPaint paint, long dwPenColor, int width, int style, float x1, float y1, float x2, float y2, float k, float b)
        {
            //非垂直时
            if (k != 0 || b != 0)
            {
                float leftX = 0;
                float leftY = leftX * k + b;
                float rightX = WorkingAreaWidth;
                float rightY = rightX * k + b;
                if (x1 >= x2)
                {
                    DrawLine(paint, dwPenColor, width, style, leftX, leftY, x1, y1);
                }
                else
                {
                    DrawLine(paint, dwPenColor, width, style, x1, y1, rightX, rightY);
                }
            }
            //垂直时
            else
            {
                if (y1 >= y2)
                {
                    DrawLine(paint, dwPenColor, width, style, x1, y1, x1, 0);
                }
                else
                {
                    DrawLine(paint, dwPenColor, width, style, x1, y1, x1, WorkingAreaHeight);
                }
            }
        }

        /// <summary>
        /// 画矩形
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="left">横坐标</param>
        /// <param name="top">纵坐标</param>
        /// <param name="right">右侧横坐标</param>
        /// <param name="bottom">右侧纵坐标</param>
        protected void DrawRect(CPaint paint, long dwPenColor, int width, int style, int left, int top, int right, int bottom)
        {
            left += GetPx();
            top += GetPy();
            right += GetPx();
            bottom += GetPy();
            RECT rect = new RECT(left, top, right, bottom);
            if (paint.SupportTransparent())
            {
                paint.DrawRect(dwPenColor, width, style, rect);
                ChartA chart = Chart;
                POINT mp = chart.MousePoint;
                if (!m_isPaintingGhost && (mp.y >= m_div.Top && mp.y <= m_div.Bottom) &&
                (chart.MovingPlot == this ||
                (chart == Native.HoveredControl
                && !chart.IsOperating() && OnSelect())))
                {
                    int a = 0, r = 0, g = 0, b = 0;
                    COLOR.ToARGB(paint, dwPenColor, ref a, ref r, ref g, ref b);
                    if (a == 255)
                    {
                        a = 50;
                    }
                    dwPenColor = COLOR.ARGB(a, r, g, b);
                    width += 10;
                    paint.DrawRect(dwPenColor, width, 0, rect);
                }
            }
        }

        /// <summary>
        /// 画矩形
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="left">横坐标</param>
        /// <param name="top">纵坐标</param>
        /// <param name="right">右侧横坐标</param>
        /// <param name="bottom">右侧纵坐标</param>
        protected void DrawRect(CPaint paint, long dwPenColor, int width, int style, float left, float top, float right, float bottom)
        {
            left += GetPx();
            top += GetPy();
            right += GetPx();
            bottom += GetPy();
            RECT rect = new RECT(left, top, right, bottom);
            paint.DrawRect(dwPenColor, width, style, rect);
            if (paint.SupportTransparent())
            {
                ChartA chart = Chart;
                POINT mp = chart.MousePoint;
                if (!m_isPaintingGhost && (mp.y >= m_div.Top && mp.y <= m_div.Bottom) &&
                (chart.MovingPlot == this ||
                (chart == Native.HoveredControl
                && !chart.IsOperating() && OnSelect())))
                {
                    int a = 0, r = 0, g = 0, b = 0;
                    COLOR.ToARGB(paint, dwPenColor, ref a, ref r, ref g, ref b);
                    if (a == 255)
                    {
                        a = 50;
                    }
                    dwPenColor = COLOR.ARGB(a, r, g, b);
                    width += 10;
                    paint.DrawRect(dwPenColor, width, 0, rect);
                }
            }
        }

        /// <summary>
        /// 画曲线
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="points">点的集合</param>
        protected void DrawPolyline(CPaint paint, long dwPenColor, int width, int style, POINT[] points)
        {
            int px = GetPx();
            int py = GetPy();
            for (int i = 0; i < points.Length; i++)
            {
                POINT point = new POINT(px + points[i].x, py + points[i].y);
                points[i] = point;
            }
            paint.DrawPolyline(dwPenColor, width, style, points);
            if (paint.SupportTransparent())
            {
                ChartA chart = Chart;
                POINT mp = chart.MousePoint;
                if (!m_isPaintingGhost && (mp.y >= m_div.Top && mp.y <= m_div.Bottom) &&
                (chart.MovingPlot == this ||
                (chart == Native.HoveredControl
                && !chart.IsOperating() && OnSelect())))
                {
                    int a = 0, r = 0, g = 0, b = 0;
                    COLOR.ToARGB(paint, dwPenColor, ref a, ref r, ref g, ref b);
                    if (a == 255)
                    {
                        a = 50;
                    }
                    dwPenColor = COLOR.ARGB(a, r, g, b);
                    width += 10;
                    paint.DrawPolyline(dwPenColor, width, 0, points);
                }
            }
        }

        /// <summary>
        /// 绘制选中点
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="x">横坐标</param>
        /// <param name="y">纵坐标</param>
        protected void DrawSelect(CPaint paint, long dwPenColor, float x, float y)
        {
            x += GetPx();
            y += GetPy();
            int sub = m_lineWidth * 3;
            RECT rect = new RECT(x - sub, y - sub, x + sub, y + sub);
            if (SelectedPoint == SelectedPoint.Ellipse)
            {
                paint.FillEllipse(dwPenColor, rect);
            }
            else if (SelectedPoint == SelectedPoint.Rectangle)
            {
                paint.FillRect(dwPenColor, rect);
            }
        }

        /// <summary>
        /// 画文字
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="text">文字</param>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="font">字体</param>
        /// <param name="x">横坐标</param>
        /// <param name="y">纵坐标</param>
        protected void DrawText(CPaint paint, String text, long dwPenColor, FONT font, int x, int y)
        {
            x += GetPx();
            y += GetPy();
            SIZE tSize = paint.TextSize(text, font);
            RECT tRect = new RECT(x, y, x + tSize.cx, y + tSize.cy);
            paint.DrawText(text, dwPenColor, font, tRect);
        }

        /// <summary>
        /// 画文字
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="text">文字</param>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="font">字体</param>
        /// <param name="x">横坐标</param>
        /// <param name="y">纵坐标</param>
        protected void DrawText(CPaint paint, String text, long dwPenColor, FONT font, float x, float y)
        {
            x += GetPx();
            y += GetPy();
            SIZE tSize = paint.TextSize(text, font);
            RECT tRect = new RECT(x, y, x + tSize.cx, y + tSize.cy);
            paint.DrawText(text, dwPenColor, font, tRect);
        }

        /// <summary>
        /// 填充多边形
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="color">颜色</param>
        /// <param name="points">点的集合</param>
        protected void FillPolygon(CPaint paint, long dwPenColor, POINT[] points)
        {
            int px = GetPx();
            int py = GetPy();
            for (int i = 0; i < points.Length; i++)
            {
                POINT point = new POINT(px + points[i].x, py + points[i].y);
                points[i] = point;
            }
            paint.FillPolygon(dwPenColor, points);
        }

        /// <summary>
        /// 获取K线区域内最高价和最低价
        /// </summary>
        /// <param name="pList">点阵集合</param>
        /// <returns>幅度尺的参数</returns>
        protected double[] GetCandleRange(Dictionary<int, PlotMark> pList)
        {
            if (pList.Count == 0 || m_sourceFields.Count == 0)
            {
                return null;
            }
            if (!m_sourceFields.ContainsKey("HIGH") || !m_sourceFields.ContainsKey("LOW"))
            {
                return null;
            }
            int bRecord = m_dataSource.GetRowIndex(pList[0].Key);
            int eRecord = m_dataSource.GetRowIndex(pList[1].Key);
            double[] highlist = null;
            double[] lowlist = null;
            if (eRecord >= bRecord)
            {
                highlist = m_dataSource.DATA_ARRAY(m_sourceFields["HIGH"], eRecord, eRecord - bRecord + 1);
                lowlist = m_dataSource.DATA_ARRAY(m_sourceFields["LOW"], eRecord, eRecord - bRecord + 1);
            }
            else
            {
                highlist = m_dataSource.DATA_ARRAY(m_sourceFields["HIGH"], bRecord, bRecord - eRecord + 1);
                lowlist = m_dataSource.DATA_ARRAY(m_sourceFields["LOW"], bRecord, bRecord - eRecord + 1);
            }
            double nHigh = 0, nLow = 0;
            nHigh = PMathLib.M009(highlist, highlist.Length);
            nLow = PMathLib.M008(lowlist, lowlist.Length);
            return new double[] { nHigh, nLow };
        }

        /// <summary>
        /// 获取动作
        /// </summary>
        /// <returns>动作类型</returns>
        public virtual ActionType GetAction()
        {
            return ActionType.NO;
        }

        /// <summary>
        /// 获取瞄准的坐标
        /// </summary>
        /// <returns></returns>
        protected POINT GetMouseOverPoint()
        {
            ChartA chart = Chart;
            POINT mp = chart.MousePoint;
            mp.x -= chart.LeftVScaleWidth;
            mp.y = mp.y - m_div.Top - m_div.TitleBar.Height;
            return mp;
        }

        /// <summary>
        /// 根据坐标获取索引
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>索引</returns>
        protected int GetIndex(POINT mp)
        {
            ChartA chart = Chart;
            mp.x += chart.LeftVScaleWidth;
            mp.y += m_div.Top + m_div.TitleBar.Height;
            return chart.GetIndex(mp);
        }

        /// <summary>
        /// 获取直线的参数
        /// </summary>
        /// <param name="markA">第一个点</param>
        /// <param name="markB">第二个点</param>
        /// <returns>直线的参数</returns>
        protected float[] GetLineParams(PlotMark markA, PlotMark markB)
        {
            float y1 = PY(markA.Value);
            float y2 = PY(markB.Value);
            int bIndex = m_dataSource.GetRowIndex(markA.Key);
            int eIndex = m_dataSource.GetRowIndex(markB.Key);
            float x1 = PX(bIndex);
            float x2 = PX(eIndex);
            float a = 0, b = 0;
            if (x2 - x1 != 0)
            {
                PMathLib.M107(x1, y1, x2, y2, 0, 0, ref a, ref b);
                return new float[] { a, b };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取线性回归带的高低点偏离值
        /// </summary>
        /// <param name="pList">点阵集合</param>
        /// <param name="param">直线参数</param>
        /// <returns>高低点偏离值</returns>
        protected double[] GetLRBandRange(Dictionary<int, PlotMark> pList, float[] param)
        {
            if (m_sourceFields != null && m_sourceFields.Count > 0 && m_sourceFields.ContainsKey("HIGH")
                && m_sourceFields.ContainsKey("LOW"))
            {
                float a = param[0];
                float b = param[1];
                List<double> upList = new List<double>();
                List<double> downList = new List<double>();
                int bIndex = m_dataSource.GetRowIndex(pList[0].Key);
                int eIndex = m_dataSource.GetRowIndex(pList[1].Key);
                for (int i = bIndex; i <= eIndex; i++)
                {
                    double high = m_dataSource.Get2(i, m_sourceFields["HIGH"]);
                    double low = m_dataSource.Get2(i, m_sourceFields["LOW"]);
                    if (!double.IsNaN(high) && !double.IsNaN(low))
                    {
                        double midValue = (i - bIndex + 1) * a + b;
                        upList.Add(high - midValue);
                        downList.Add(midValue - low);
                    }
                }
                double upSubValue = PMathLib.M009(upList.ToArray(), upList.Count);
                double downSubValue = PMathLib.M009(downList.ToArray(), downList.Count);
                return new double[] { upSubValue, downSubValue };
            }
            return null;
        }

        /// <summary>
        /// 获取线性回归的参数
        /// </summary>
        /// <param name="marks">点阵集合</param>
        /// <returns>点阵数组</returns>
        protected float[] GetLRParams(Dictionary<int, PlotMark> marks)
        {
            if (m_sourceFields != null && m_sourceFields.ContainsKey("CLOSE"))
            {
                int bIndex = m_dataSource.GetRowIndex(marks[0].Key);
                int eIndex = m_dataSource.GetRowIndex(marks[1].Key);
                if (bIndex != -1 && eIndex != -1)
                {
                    List<double> closeVList = new List<double>();
                    for (int i = bIndex; i <= eIndex; i++)
                    {
                        double value = m_dataSource.Get2(i, m_sourceFields["CLOSE"]);
                        if (!double.IsNaN(value))
                        {
                            closeVList.Add(value);
                        }
                    }
                    if (closeVList.Count > 0)
                    {
                        float a = 0;
                        float b = 0;
                        PMathLib.M014(closeVList.ToArray(), closeVList.Count, ref a, ref b);
                        return new float[] { a, b };
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 获取移动坐标
        /// </summary>
        /// <returns></returns>
        protected POINT GetMovingPoint()
        {
            POINT mp = GetMouseOverPoint();
            if (mp.x < 0)
            {
                mp.x = 0;
            }
            else if (mp.x > WorkingAreaWidth)
            {
                mp.x = WorkingAreaWidth;
            }
            if (mp.y < 0)
            {
                mp.y = 0;
            }
            else if (mp.y > WorkingAreaHeight)
            {
                mp.y = WorkingAreaHeight;
            }
            return mp;
        }

        /// <summary>
        /// 根据坐标获取数值
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>数值</returns>
        protected double GetNumberValue(POINT mp)
        {
            ChartA chart = Chart;
            mp.x += chart.LeftVScaleWidth;
            mp.y += m_div.Top + m_div.TitleBar.Height;
            return chart.GetNumberValue(m_div, mp, m_attachVScale);
        }

        /// <summary>
        /// 获取偏移横坐标
        /// </summary>
        /// <returns>偏移横坐标</returns>
        protected int GetPx()
        {
            ChartA chart = Chart;
            return chart.LeftVScaleWidth;
        }

        /// <summary>
        /// 获取偏移纵坐标
        /// </summary>
        /// <returns>偏移纵坐标</returns>
        protected int GetPy()
        {
            return m_div.TitleBar.Height;
        }

        /// <summary>
        /// 根据两点获取矩形
        /// </summary>
        /// <param name="markA">第一个点</param>
        /// <param name="markB">第二个点</param>
        /// <returns>矩形对象</returns>
        protected RECT GetRectangle(PlotMark markA, PlotMark markB)
        {
            double aValue = markA.Value;
            double bValue = markB.Value;
            int aIndex = m_dataSource.GetRowIndex(markA.Key);
            int bIndex = m_dataSource.GetRowIndex(markB.Key);
            float x = PX(aIndex);
            float y = PY(aValue);
            float xS = PX(bIndex);
            float yS = PY(bValue);
            float width = Math.Abs(xS - x);
            if (width < 4)
            {
                width = 4;
            }
            float height = Math.Abs(yS - y);
            if (height < 4)
            {
                height = 4;
            }
            float rX = x <= xS ? x : xS;
            float rY = y <= yS ? y : yS;
            return new RECT(rX, rY, rX + width, rY + height);
        }

        /// <summary>
        /// 获取黄金分割线的直线参数
        /// </summary>
        /// <param name="value1">值1</param>
        /// <param name="value2">值2</param>
        /// <returns>黄金分割线的直线参数</returns>
        protected float[] GoldenRatioParams(double value1, double value2)
        {
            float y1 = PY(value1);
            float y2 = PY(value2);
            float y0 = 0, yA = 0, yB = 0, yC = 0, yD = 0, y100 = 0;
            y0 = y1;
            yA = y1 <= y2 ? y1 + (y2 - y1) * 0.236f : y2 + (y1 - y2) * (1 - 0.236f);
            yB = y1 <= y2 ? y1 + (y2 - y1) * 0.382f : y2 + (y1 - y2) * (1 - 0.382f);
            yC = y1 <= y2 ? y1 + (y2 - y1) * 0.5f : y2 + (y1 - y2) * (1 - 0.5f);
            yD = y1 <= y2 ? y1 + (y2 - y1) * 0.618f : y2 + (y1 - y2) * (1 - 0.618f);
            y100 = y2;
            return new float[] { y0, yA, yB, yC, yD, y100 };
        }

        /// <summary>
        /// 多条横线的选中方法
        /// </summary>
        /// <param name="param">参数</param>
        /// <param name="length">长度</param>
        /// <returns>是否选中</returns>
        protected bool HLinesSelect(float[] param,int length)
        {
            POINT mp = GetMouseOverPoint();
            //获取上下区间
            float top = 0;
            float bottom = WorkingAreaHeight;
            if (mp.y >= top && mp.y <= bottom)
            {
                foreach (float p in param)
                {
                    //判断每个点
                    if (mp.y >= p - m_lineWidth * 2.5f && mp.y <= p + m_lineWidth * 2.5f)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 移动线条
        /// </summary>
        /// <param name="mp">坐标</param>
        protected void Move(POINT mp)
        {
            VScale vScale = m_div.GetVScale(m_attachVScale);
            //循环处理
            int startMarkSize = m_startMarks.Count;
            for (int i = 0; i < startMarkSize; i++)
            {
                int newIndex = 0;
                double yAddValue = 0;
                int startIndex = m_dataSource.GetRowIndex(m_startMarks[i].Key);
                //调用函数
                MovePlot(mp.y, m_startPoint.y, startIndex, GetIndex(m_startPoint), GetIndex(mp), WorkingAreaHeight, vScale.VisibleMax,
                vScale.VisibleMin, m_dataSource.RowsCount, ref yAddValue, ref newIndex);
                //纵轴反转
                if (vScale.Reverse)
                {
                    m_marks[i] = new PlotMark(i, m_dataSource.GetXValue(newIndex), m_startMarks[i].Value - yAddValue);
                }
                else
                {
                    m_marks[i] = new PlotMark(i, m_dataSource.GetXValue(newIndex), m_startMarks[i].Value + yAddValue);
                }
            }
        }

        /// <summary>
        /// 移动画线
        /// </summary>
        /// <param name="mouseY">纵坐标</param>
        /// <param name="startY">开始纵坐标</param>
        /// <param name="startIndex">开始索引</param>
        /// <param name="mouseBeginIndex">鼠标开始索引</param>
        /// <param name="mouseEndIndex">鼠标结束索引</param>
        /// <param name="pureV">纵向距离</param>
        /// <param name="max">最大值</param>
        /// <param name="min">最小值</param>
        /// <param name="dataCount">数据条数</param>
        /// <param name="yAddValue">纵向变化值</param>
        /// <param name="newIndex">新的索引</param>
        private void MovePlot(float mouseY,float startY,int startIndex,int mouseBeginIndex,int mouseEndIndex,float pureV,
	    double max,double min,int dataCount,ref double yAddValue,ref int newIndex)
	    {
		    float subY = mouseY - startY;
		    yAddValue = ((min - max) * subY / pureV);
		    newIndex = startIndex + (mouseEndIndex - mouseBeginIndex);
		    if(newIndex<0)
		    {
			    newIndex = 0;
		    }
		    else if(newIndex > dataCount - 1)
		    {
			    newIndex = dataCount - 1;
		    }
	    }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public virtual bool OnCreate(POINT mp)
        {
            return false;
        }

        /// <summary>
        /// 移动结束
        /// </summary>
        public virtual void OnMoveEnd()
        {
        }

        /// <summary>
        /// 移动开始
        /// </summary>
        public virtual void OnMoveStart()
        {
        }

        /// <summary>
        /// 移动
        /// </summary>
        public virtual void OnMoving()
        {
            POINT mp = GetMovingPoint();
            //根据不同类型作出动作
            switch (m_action)
            {
                case ActionType.MOVE:
                    Move(mp);
                    break;
                case ActionType.AT1:
                    Resize(0);
                    break;
                case ActionType.AT2:
                    Resize(1);
                    break;
                case ActionType.AT3:
                    Resize(2);
                    break;
                case ActionType.AT4:
                    Resize(3);
                    break;
            }
        }

        /// <summary>
        /// 绘图对象
        /// </summary>
        /// <param name="paint">绘图对象</param>
        public virtual void OnPaint(CPaint paint)
        {
            ChartA chart = Chart;
            POINT mp = chart.MousePoint;
            if ((mp.y >= m_div.Top && mp.y <= m_div.Bottom) && 
                (chart.MovingPlot == this ||
                (chart == Native.HoveredControl 
                && !chart.IsOperating() && OnSelect())))
            {
                Paint(paint, m_marks, m_selectedColor);
            }
            else
            {
                Paint(paint, m_marks, m_color);
            }
        }

        /// <summary>
        /// 绘制图像的残影
        /// </summary>
        /// <param name="paint">绘图对象</param>
        public virtual void OnPaintGhost(CPaint paint)
        {
            m_isPaintingGhost = true;
            Paint(paint, m_startMarks, m_color);
            m_isPaintingGhost = false;
        }

        /// <summary>
        /// 判断选中
        /// </summary>
        /// <returns>是否选中</returns>
        public virtual bool OnSelect()
        {
            ActionType action = GetAction();
            if (action != ActionType.NO)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected virtual void Paint(CPaint paint, Dictionary<int,PlotMark> pList, long lineColor)
        {

        }

        /// <summary>
        /// 获取绘制横坐标
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>横坐标</returns>
        protected float PX(int index)
        {
            ChartA chart = Chart;
            float x = chart.GetX(index);
            return x - chart.LeftVScaleWidth;
        }

        /// <summary>
        /// 获取绘制纵坐标
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>纵坐标</returns>
        protected float PY(double value)
        {
            ChartA chart = Chart;
            float y = chart.GetY(m_div, value, m_attachVScale);
            return y - m_div.TitleBar.Height;
        }

        /// <summary>
        /// 根据横坐标获取画线工具内部横坐标
        /// </summary>
        /// <param name="x">横坐标</param>
        /// <returns>内部横坐标</returns>
        protected float PX(float x)
        {
            ChartA chart = Chart;
            return x - chart.LeftVScaleWidth;
        }

        /// <summary>
        /// 根据纵坐标获取画线工具内部纵坐标
        /// </summary>
        /// <param name="y">纵坐标</param>
        /// <returns>内部纵坐标</returns>
        protected float PY(float y)
        {
            return y - m_div.Top - m_div.TitleBar.Height;
        }

        /// <summary>
        /// 绘制到图像上
        /// </summary>
        /// <param name="paint">绘图对象</param>
        public virtual void Render(CPaint paint)
        {
            ChartA chart = Chart;
            if (m_drawGhost && chart.MovingPlot != null && chart.MovingPlot == this)
            {
                OnPaintGhost(paint);
            }
            OnPaint(paint);
        }

        /// <summary>
        /// 调整大小
        /// </summary>
        /// <param name="index">索引</param>
        protected void Resize(int index)
        {
            ChartA chart = Chart;
            int mouseIndex = chart.GetMouseOverIndex();
            double y = GetNumberValue(GetMovingPoint());
            if (mouseIndex < 0)
            {
                mouseIndex = 0;
            }
            if (mouseIndex > chart.LastVisibleIndex)
            {
                mouseIndex = chart.LastVisibleIndex;
            }
            m_marks[index] = new PlotMark(index, m_dataSource.GetXValue(mouseIndex), y);
        }

        /// <summary>
        /// 重新调整大小
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <param name="oppositePoint">点位置</param>
        protected void Resize(POINT mp, POINT oppositePoint)
        {
            double sValue = GetNumberValue(new POINT(oppositePoint.x, oppositePoint.y));
            double eValue = GetNumberValue(mp);
            int iS = GetIndex(new POINT(oppositePoint.x, oppositePoint.y));
            int iE = GetIndex(mp);
            double topValue = 0;
            double bottomValue = 0;
            VScale vScale = m_div.GetVScale(m_attachVScale);
            if (sValue >= eValue)
            {
                if (vScale.Reverse)
                {
                    topValue = eValue;
                    bottomValue = sValue;
                }
                else
                {
                    topValue = sValue;
                    bottomValue = eValue;
                }
            }
            else
            {
                if (vScale.Reverse)
                {
                    topValue = sValue;
                    bottomValue = eValue;
                }
                else
                {
                    topValue = eValue;
                    bottomValue = sValue;
                }
            }
            double sDate = 0;
            double eDate = 0;
            if (iS < 0)
            {
                iS = 0;
            }
            else if (iS > m_dataSource.RowsCount - 1)
            {
                iS = m_dataSource.RowsCount - 1;
            }
            if (iE < 0)
            {
                iE = 0;
            }
            else if (iE > m_dataSource.RowsCount - 1)
            {
                iE = m_dataSource.RowsCount - 1;
            }
            if (iS >= iE)
            {
                sDate = m_dataSource.GetXValue(iE);
                eDate = m_dataSource.GetXValue(iS);
            }
            else
            {
                sDate = m_dataSource.GetXValue(iS);
                eDate = m_dataSource.GetXValue(iE);
            }
            //横轴反转
            m_marks[0] = new PlotMark(0, sDate, topValue);
            m_marks[1] = new PlotMark(1, eDate, bottomValue);
        }

        /// <summary>
        /// 判断是否选中了指定的点
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <param name="x">点的横坐标</param>
        /// <param name="y">点的纵坐标</param>
        /// <returns>是否选中</returns>
        protected bool SelectPoint(POINT mp, float x, float y)
        {
            if (mp.x >= x - m_lineWidth * 6 && mp.x <= x + m_lineWidth * 6
                    && mp.y >= y - m_lineWidth * 6 && mp.y <= y + m_lineWidth * 6)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断是否选中线
        /// </summary>
        /// <param name="mp">点的位置</param>
        /// <param name="x">横坐标</param>
        /// <param name="k">直线参数k</param>
        /// <param name="b">直线参数b</param>
        /// <returns>是否选中线</returns>
        protected bool SelectLine(POINT mp, float x, float k, float b)
        {
            if (!(k == 0 && b == 0))
            {
                if (mp.y / (mp.x * k + b) >= 0.95 && mp.y / (mp.x * k + b) <= 1.05)
                {
                    return true;
                }
            }
            else
            {
                if (mp.x >= x - m_lineWidth * 5 && mp.x <= x + m_lineWidth * 5)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断是否选中线
        /// </summary>
        /// <param name="mp">点的位置</param>
        /// <param name="x1">第一个点的横坐标</param>
        /// <param name="y1">第一个点的纵坐标</param>
        /// <param name="x2">第二个点的横坐标</param>
        /// <param name="y2">第二个点的纵坐标</param>
        /// <returns>是否选中线</returns>
        protected bool SelectLine(POINT mp, float x1, float y1, float x2, float y2)
        {
            float k = 0, b = 0;
            //获取直线参数
            PMathLib.M107(x1, y1, x2, y2, 0, 0, ref k, ref b);
            return SelectLine(mp, x1, k, b);
        }


        /// <summary>
        /// 判断是否选中射线
        /// </summary>
        /// <param name="mp">点的位置</param>
        /// <param name="x1">第一个点的横坐标</param>
        /// <param name="y1">第一个点的纵坐标</param>
        /// <param name="x2">第二个点的横坐标</param>
        /// <param name="y2">第二个点的纵坐标</param>
        /// <param name="k">直线参数k</param>
        /// <param name="b">直线参数b</param>
        /// <returns>是否选中射线</returns>
        protected bool SelectRay(POINT mp, float x1, float y1, float x2, float y2, ref float k, ref float b)
        {
            //获取直线参数
            PMathLib.M107(x1, y1, x2, y2, 0, 0, ref k, ref b);
            if (!(k == 0 && b == 0))
            {
                if (mp.y / (mp.x * k + b) >= 0.95 && mp.y / (mp.x * k + b) <= 1.05)
                {
                    if (x1 >= x2)
                    {
                        if (mp.x > x1 + 5) return false;
                    }
                    else if (x1 < x2)
                    {
                        if (mp.x < x1 - 5) return false;
                    }
                    return true;
                }
            }
            else
            {
                if (mp.x >= x1 - m_lineWidth * 5 && mp.x <= x1 + m_lineWidth * 5)
                {
                    if (y1 >= y2)
                    {
                        if (mp.y <= y1 - 5)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (mp.y >= y1 - 5)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 判断是否选中射线
        /// </summary>
        /// <param name="mp">点的位置</param>
        /// <param name="x1">第一个点的横坐标</param>
        /// <param name="y1">第一个点的纵坐标</param>
        /// <param name="x2">第二个点的横坐标</param>
        /// <param name="y2">第二个点的纵坐标</param>
        /// <returns>是否选中射线</returns>
        protected bool SelectRay(POINT mp, float x1, float y1, float x2, float y2)
        {
            float k = 0, b = 0;
            return SelectRay(mp, x1, y1, x2, y2, ref k, ref b);
        }

        /// <summary>
        /// 获取选中状态
        /// </summary>
        /// <param name="mp">点的位置</param>
        /// <param name="markA">点A</param>
        /// <param name="markB">点B</param>
        /// <returns>选中状态</returns>
        protected ActionType SelectRect(POINT mp, PlotMark markA, PlotMark markB)
        {
            RECT rect = GetRectangle(markA, markB);
            int x1 = rect.left;
            int y1 = rect.top;
            int x2 = rect.right;
            int y2 = rect.top;
            int x3 = rect.left;
            int y3 = rect.bottom;
            int x4 = rect.right;
            int y4 = rect.bottom;
            //判断是否选中点
            if (SelectPoint(mp, x1, y1))
            {
                return ActionType.AT1;
            }
            else if (SelectPoint(mp, x2, y2))
            {
                return ActionType.AT2;
            }
            else if (SelectPoint(mp, x3, y3))
            {
                return ActionType.AT3;
            }
            else if (SelectPoint(mp, x4, y4))
            {
                return ActionType.AT4;
            }
            else
            {
                int sub = (int)(m_lineWidth * 2.5);
                RECT bigRect = new RECT(rect.left - sub, rect.top - sub, rect.right + sub, rect.bottom + sub);
                if (mp.x >= bigRect.left && mp.x <= bigRect.right && mp.y >= bigRect.top && mp.y <= bigRect.bottom)
                {
                    if (rect.right - rect.left <= 4 || rect.bottom - rect.top <= 4)
                    {
                        return ActionType.MOVE;
                    }
                    else
                    {
                        RECT smallRect = new RECT(rect.left + sub, rect.top + sub, rect.right - sub, rect.bottom - sub);
                        if (!(mp.x >= smallRect.left && mp.x <= smallRect.right && mp.y >= smallRect.top && mp.y <= smallRect.bottom))
                        {
                            return ActionType.MOVE;
                        }
                    }
                }
            }
            return ActionType.NO;
        }

        /// <summary>
        /// 判断是否选中线段
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <param name="x1">第一个点的横坐标</param>
        /// <param name="y1">第一个点的纵坐标</param>
        /// <param name="x2">第二个点的横坐标</param>
        /// <param name="y2">第二个点的纵坐标</param>
        /// <returns>是否选中线段</returns>
        protected bool SelectSegment(POINT mp, float x1, float y1, float x2, float y2)
        {
            float k = 0;
            float b = 0;
            //获取直线参数
            PMathLib.M107(x1, y1, x2, y2, 0, 0, ref k, ref b);
            float smallX = x1 <= x2 ? x1 : x2;
            float smallY = y1 <= y2 ? y1 : y2;
            float bigX = x1 > x2 ? x1 : x2;
            float bigY = y1 > y2 ? y1 : y2;
            if (mp.x >= smallX - 2 && mp.x <= bigX + 2 && mp.y >= smallY - 2 && mp.y <= bigY + 2)
            {
                if (!(k == 0 && b == 0))
                {
                    if (mp.y / (mp.x * k + b) >= 0.95 && mp.y / (mp.x * k + b) <= 1.05)
                    {
                        return true;
                    }
                }
                else
                {
                    if (mp.x >= x1 - m_lineWidth * 5 && mp.x <= x1 + m_lineWidth * 5)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 判断是否选中正弦线
        /// </summary>
        /// <param name="mp">点的位置</param>
        /// <param name="x1">第一个点的横坐标</param>
        /// <param name="y1">第一个点的纵坐标</param>
        /// <param name="x2">第二个点的横坐标</param>
        /// <param name="y2">第二个点的纵坐标</param>
        /// <returns>是否选中正弦线</returns>
        protected bool SelectSine(POINT mp, float x1, float y1, float x2, float y2)
        {
            double f = 2.0 * Math.PI / ((x2 - x1) * 4);
            int len = WorkingAreaWidth;
            if (len > 0)
            {
                float lastX = 0, lastY = 0;
                for (int i = 0; i < len; i++)
                {
                    float x = -x1 + i;
                    float y = (float)(0.5 * (y2 - y1) * Math.Sin(x * f) * 2);
                    float px = x + x1, py = y + y1;
                    if (i == 0)
                    {
                        if (SelectPoint(mp, px, py))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        float rectLeft = lastX - 2;
                        float rectTop = lastY <= py ? lastY : py - 2;
                        float rectRight = rectLeft + Math.Abs(px - lastX) + 4;
                        float rectBottom = rectTop + Math.Abs(py - lastY) + 4;
                        if (mp.x >= rectLeft && mp.x <= rectRight && mp.y >= rectTop && mp.y <= rectBottom)
                        {
                            return true;
                        }
                    }
                    lastX = px;
                    lastY = py;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断是否选中三角形
        /// </summary>
        /// <param name="mp">点的位置</param>
        /// <param name="x1">第一个点的横坐标</param>
        /// <param name="y1">第一个点的纵坐标</param>
        /// <param name="x2">第二个点的横坐标</param>
        /// <param name="y2">第二个点的纵坐标</param>
        /// <param name="x3">第三个点的横坐标</param>
        /// <param name="y3">第三个点的纵坐标</param>
        /// <returns>是否选中三角形</returns>
        protected bool SelectTriangle(POINT mp, float x1, float y1, float x2, float y2, float x3, float y3)
        {
            bool selected = SelectSegment(mp, x1, y1, x2, y2);
            if (selected) return true;
            selected = SelectSegment(mp, x1, y1, x3, y3);
            if (selected) return true;
            selected = SelectSegment(mp, x2, y2, x3, y3);
            if (selected) return true;
            return false;
        }

        /// <summary>
        /// 获取文字的大小
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="text">文字</param>
        /// <param name="font">字体</param>
        /// <returns>大小</returns>
        protected SIZE TextSize(CPaint paint, String text, FONT font)
        {
            return paint.TextSize(text, font);
        }
        #endregion
    }
}
