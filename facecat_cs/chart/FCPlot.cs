/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

using System;
using System.Collections.Generic;

namespace FaceCat {
    /// <summary>
    /// 动作类型
    /// </summary>
    public enum ActionType {
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
    public class PlotMark {
        /// <summary>
        /// 创建一个点值
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public PlotMark(int index, double key, double value) {
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
    /// 画线工具的基类
    /// </summary>
    public class FCPlot {
        /// <summary>
        /// 创建画线
        /// </summary>
        public FCPlot() {
            m_color = FCColor.argb(255, 255, 255);
            m_selectedColor = FCColor.argb(255, 255, 255);
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~FCPlot() {
            delete();
        }

        /// <summary>
        /// 移动类型
        /// </summary>
        protected ActionType m_action = ActionType.NO;

        /// <summary>
        /// 数据源
        /// </summary>
        protected FCDataTable m_dataSource;

        /// <summary>
        /// 是否正在绘制阴影
        /// </summary>
        protected bool m_isPaintingGhost;

        /// <summary>
        /// 关键点
        /// </summary>
        protected HashMap<int, PlotMark> m_marks = new HashMap<int, PlotMark>();

        /// <summary>
        /// 移动次数
        /// </summary>
        protected int m_moveTimes = 0;

        /// <summary>
        /// 源字段
        /// </summary>
        protected HashMap<String, int> m_sourceFields = new HashMap<String, int>();

        /// <summary>
        /// 开始移动时的值
        /// </summary>
        protected HashMap<int, PlotMark> m_startMarks = new HashMap<int, PlotMark>();

        /// <summary>
        /// 开始移动的点
        /// </summary>
        protected FCPoint m_startPoint = new FCPoint();

        protected AttachVScale m_attachVScale = AttachVScale.Left;

        /// <summary>
        /// 获取或设置附着在左轴还是右轴
        /// </summary>
        public AttachVScale AttachVScale {
            get { return m_attachVScale; }
            set { m_attachVScale = value; }
        }

        protected long m_color;

        /// <summary>
        /// 获取或设置线的颜色
        /// </summary>
        public long Color {
            get { return m_color; }
            set { m_color = value; }
        }

        /// <summary>
        /// 设置触摸形状
        /// </summary>
        protected FCCursors Cursor {
            get { return m_div.Chart.Cursor; }
            set { m_div.Chart.Cursor = value; }
        }

        protected ChartDiv m_div;

        /// <summary>
        /// 获取或设置所在图层
        /// </summary>
        public ChartDiv Div {
            get { return m_div; }
            set {
                m_div = value;
                m_dataSource = m_div.Chart.DataSource;
            }
        }

        protected bool m_drawGhost = true;

        /// <summary>
        /// 获取或设置是否画移动残影
        /// </summary>
        public bool DrawGhost {
            get { return m_drawGhost; }
            set { m_drawGhost = value; }
        }

        protected bool m_enabled = true;

        /// <summary>
        /// 获取或设置是否可以选中或拖放
        /// </summary>
        public bool Enabled {
            get { return m_enabled; }
            set {
                if (!value) {
                    m_selected = false;
                }
                m_enabled = value;
            }
        }

        protected FCFont m_font = new FCFont();

        /// <summary>
        /// 获取或设置字体
        /// </summary>
        public FCFont Font {
            get { return m_font; }
            set { m_font = value; }
        }

        private bool m_isDeleted;

        /// <summary>
        /// 获取或设置是否已被销毁
        /// </summary>
        public virtual bool IsDeleted {
            get { return m_isDeleted; }
        }

        /// <summary>
        /// 获取所在布局
        /// </summary>
        public FCChart Chart {
            get { return m_div.Chart; }
        }

        protected int m_lineStyle = 0;

        /// <summary>
        /// 获取或设置线的样式，null为实线
        /// </summary>
        public int LineStyle {
            get { return m_lineStyle; }
            set { m_lineStyle = value; }
        }

        protected int m_lineWidth = 1;

        /// <summary>
        /// 获取或设置线的宽度
        /// </summary>
        public int LineWidth {
            get { return m_lineWidth; }
            set { m_lineWidth = value; }
        }

        /// <summary>
        /// 获取方法库
        /// </summary>
        protected FCNative Native {
            get { return m_div.Chart.Native; }
        }

        protected String m_plotType = "LINE";

        /// <summary>
        /// 获取或设置线条的类型
        /// </summary>
        public String PlotType {
            get { return m_plotType; }
            set { m_plotType = value; }
        }

        protected bool m_selected;

        /// <summary>
        /// 获取或设置是否被选中
        /// </summary>
        public bool Selected {
            get { return m_selected; }
            set { m_selected = value; }
        }

        protected long m_selectedColor;

        /// <summary>
        /// 获取或设置选中色
        /// </summary>
        public long SelectedColor {
            get { return m_selectedColor; }
            set { m_selectedColor = value; }
        }

        protected SelectedPoint m_selectedPoint = SelectedPoint.Rectangle;

        /// <summary>
        /// 获取或设置选中点的样式
        /// </summary>
        public SelectedPoint SelectedPoint {
            get { return m_selectedPoint; }
            set { m_selectedPoint = value; }
        }

        protected String m_text;

        /// <summary>
        /// 获取或设置显示的文字
        /// </summary>
        public String Text {
            get { return m_text; }
            set { m_text = value; }
        }

        protected bool m_visible = true;

        /// <summary>
        /// 获取或设置可见度
        /// </summary>
        public bool Visible {
            get { return m_visible; }
            set {
                if (!value) {
                    m_selected = false;
                }
                m_visible = value;
            }
        }

        /// <summary>
        /// 获取区域宽度
        /// </summary>
        /// <returns>宽度</returns>
        protected int WorkingAreaWidth {
            get { return Chart.WorkingAreaWidth; }
        }

        /// <summary>
        /// 获取区域高度
        /// </summary>
        /// <returns>高度</returns>
        protected int WorkingAreaHeight {
            get { return m_div.WorkingAreaHeight; }
        }

        private int m_zOrder = 0;

        /// <summary>
        /// 获取或设置图层顺序
        /// </summary>
        public int ZOrder {
            get { return m_zOrder; }
            set { m_zOrder = value; }
        }

        /// <summary>
        /// 初始化一个点类型的通用方法
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        protected bool createPoint(FCPoint mp) {
            int rIndex = m_dataSource.RowsCount;
            if (rIndex > 0) {
                //获取点的位置
                FCChart chart = Chart;
                int touchIndex = chart.getIndex(mp);
                if (touchIndex >= chart.FirstVisibleIndex && touchIndex <= chart.LastVisibleIndex) {
                    double sDate = m_dataSource.getXValue(touchIndex);
                    m_marks.clear();
                    double y = getNumberValue(mp);
                    m_marks.put(0, new PlotMark(0, sDate, y));
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
        protected bool create2PointsA(FCPoint mp) {
            int rIndex = m_dataSource.RowsCount;
            if (rIndex > 0) {
                //获取点的位置
                FCChart chart = Chart;
                int touchIndex = chart.getIndex(mp);
                if (touchIndex >= chart.FirstVisibleIndex && touchIndex <= chart.LastVisibleIndex) {
                    int eIndex = touchIndex;
                    int bIndex = eIndex - 1;
                    if (bIndex >= 0) {
                        double fDate = m_dataSource.getXValue(bIndex);
                        double sDate = m_dataSource.getXValue(eIndex);
                        m_marks.clear();
                        double y = getNumberValue(mp);
                        m_marks.put(0, new PlotMark(0, fDate, y));
                        m_marks.put(1, new PlotMark(1, sDate, y));
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
        protected bool create2PointsB(FCPoint mp) {
            int rIndex = m_dataSource.RowsCount;
            if (rIndex > 0) {
                FCChart chart = Chart;
                int touchIndex = chart.getIndex(mp);
                if (touchIndex >= chart.FirstVisibleIndex && touchIndex <= chart.LastVisibleIndex) {
                    double date = m_dataSource.getXValue(touchIndex);
                    m_marks.clear();
                    double y = getNumberValue(mp);
                    m_marks.put(0, new PlotMark(0, date, y));
                    m_marks.put(1, new PlotMark(1, date, y));
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
        protected bool create2CandlePoints(FCPoint mp) {
            FCChart chart = Chart;
            int rIndex = m_dataSource.RowsCount;
            if (rIndex > 0) {
                ArrayList<BaseShape> shapesCopy = m_div.getShapes(SortType.DESC);
                foreach (BaseShape bs in shapesCopy) {
                    if (bs.Visible) {
                        CandleShape cs = bs as CandleShape;
                        if (cs != null) {
                            int touchIndex = chart.getIndex(mp);
                            if (touchIndex >= chart.FirstVisibleIndex && touchIndex <= chart.LastVisibleIndex) {
                                int eIndex = touchIndex;
                                int bIndex = eIndex - 1;
                                if (bIndex >= 0) {
                                    double fDate = m_dataSource.getXValue(bIndex);
                                    double sDate = m_dataSource.getXValue(eIndex);
                                    m_marks.clear();
                                    double y = getNumberValue(mp);
                                    m_marks.put(0, new PlotMark(0, fDate, y));
                                    m_marks.put(1, new PlotMark(1, sDate, y));
                                    m_sourceFields.put("CLOSE", cs.CloseField);
                                    m_sourceFields.put("OPEN", cs.OpenField);
                                    m_sourceFields.put("HIGH", cs.HighField);
                                    m_sourceFields.put("LOW", cs.LowField);
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
        protected bool create3Points(FCPoint mp) {
            int rIndex = m_dataSource.RowsCount;
            if (rIndex > 0) {
                //获取点的位置
                FCChart chart = Chart;
                int touchIndex = chart.getIndex(mp);
                if (touchIndex >= chart.FirstVisibleIndex && touchIndex <= chart.LastVisibleIndex) {
                    int eIndex = touchIndex;
                    int bIndex = eIndex - 1;
                    if (bIndex >= 0) {
                        double fDate = m_dataSource.getXValue(bIndex);
                        double sDate = m_dataSource.getXValue(eIndex);
                        m_marks.clear();
                        double y = getNumberValue(mp);
                        m_marks.put(0, new PlotMark(0, fDate, y));
                        m_marks.put(1, new PlotMark(1, sDate, y));
                        if (m_div.getVScale(m_attachVScale) != null && m_div.getVScale(m_attachVScale).VisibleMax != m_div.getVScale(m_attachVScale).VisibleMin) {
                            m_marks.put(2, new PlotMark(2, fDate, y + (m_div.getVScale(m_attachVScale).VisibleMax - m_div.getVScale(m_attachVScale).VisibleMin) / 4));
                        }
                        else {
                            m_marks.put(2, new PlotMark(2, fDate, y));
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
        protected void createCandlePoint(int pos, int index, int close) {
            if (index >= 0) {
                if (index > m_dataSource.RowsCount - 1) {
                    index = m_dataSource.RowsCount - 1;
                }
                double date = m_dataSource.getXValue(index);
                double yValue = 0;
                if (!double.IsNaN(m_dataSource.get2(index, close))) {
                    yValue = m_dataSource.get2(index, close);
                }
                m_marks.put(pos, new PlotMark(pos, date, yValue));
            }
        }

        /// <summary>
        /// 初始四个K线点的方法
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        protected bool create4CandlePoints(FCPoint mp) {
            FCChart chart = Chart;
            int rIndex = m_dataSource.RowsCount;
            if (rIndex > 0) {
                ArrayList<BaseShape> shapesCopy = m_div.getShapes(SortType.ASC);
                foreach (BaseShape bs in shapesCopy) {
                    if (bs.Visible && bs is CandleShape) {
                        CandleShape cs = bs as CandleShape;
                        int touchIndex = chart.getIndex(mp);
                        if (touchIndex >= chart.FirstVisibleIndex && touchIndex <= chart.LastVisibleIndex) {
                            int closeField = cs.CloseField;
                            createCandlePoint(0, touchIndex, closeField);
                            createCandlePoint(1, touchIndex + 1, closeField);
                            createCandlePoint(2, touchIndex + 2, closeField);
                            createCandlePoint(3, touchIndex + 3, closeField);
                            m_sourceFields.put("CLOSE", closeField);
                            m_sourceFields.put("HIGH", cs.HighField);
                            m_sourceFields.put("LOW", cs.LowField);
                            m_sourceFields.put("OPEN", cs.OpenField);
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
        public virtual void delete() {
            if (!m_isDeleted) {
                if (m_marks != null) {
                    m_marks.clear();
                }
                if (m_startMarks != null) {
                    m_startMarks.clear();
                }
                m_isDeleted = true;
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
        protected void drawEllipse(FCPaint paint, long dwPenColor, int width, int style, float left, float top, float right, float bottom) {
            left += getPx();
            top += getPy();
            right += getPx();
            bottom += getPy();
            FCRect rect = new FCRect(left, top, right, bottom);
            paint.drawEllipse(dwPenColor, width, style, rect);
            if (paint.supportTransparent()) {
                FCChart chart = Chart;
                FCPoint mp = chart.TouchPoint;
                if (!m_isPaintingGhost && (mp.y >= m_div.Top && mp.y <= m_div.Bottom) &&
                (chart.MovingPlot == this ||
                (chart == Native.HoveredControl
                && !chart.isOperating() && onSelect()))) {
                    int a = 0, r = 0, g = 0, b = 0;
                    FCColor.toArgb(paint, dwPenColor, ref a, ref r, ref g, ref b);
                    if (a == 255) {
                        a = 50;
                    }
                    dwPenColor = FCColor.argb(a, r, g, b);
                    width += 10;
                    paint.drawEllipse(dwPenColor, width, 0, rect);
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
        protected void drawLine(FCPaint paint, long dwPenColor, int width, int style, float x1, float y1, float x2, float y2) {
            x1 += getPx();
            y1 += getPy();
            x2 += getPx();
            y2 += getPy();
            paint.drawLine(dwPenColor, width, style, (int)x1, (int)y1, (int)x2, (int)y2);
            if (paint.supportTransparent()) {
                FCChart chart = Chart;
                FCPoint mp = chart.TouchPoint;
                if (!m_isPaintingGhost && (mp.y >= m_div.Top && mp.y <= m_div.Bottom) &&
                (chart.MovingPlot == this ||
                (chart == Native.HoveredControl
                && !chart.isOperating() && onSelect()))) {
                    int a = 0, r = 0, g = 0, b = 0;
                    FCColor.toArgb(paint, dwPenColor, ref a, ref r, ref g, ref b);
                    if (a == 255) {
                        a = 50;
                    }
                    dwPenColor = FCColor.argb(a, r, g, b);
                    width += 10;
                    paint.drawLine(dwPenColor, width, 0, (int)x1, (int)y1, (int)x2, (int)y2);
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
        protected void drawRay(FCPaint paint, long dwPenColor, int width, int style, float x1, float y1, float x2, float y2, float k, float b) {
            //非垂直时
            if (k != 0 || b != 0) {
                float leftX = 0;
                float leftY = leftX * k + b;
                float rightX = WorkingAreaWidth;
                float rightY = rightX * k + b;
                if (x1 >= x2) {
                    drawLine(paint, dwPenColor, width, style, leftX, leftY, x1, y1);
                }
                else {
                    drawLine(paint, dwPenColor, width, style, x1, y1, rightX, rightY);
                }
            }
            //垂直时
            else {
                if (y1 >= y2) {
                    drawLine(paint, dwPenColor, width, style, x1, y1, x1, 0);
                }
                else {
                    drawLine(paint, dwPenColor, width, style, x1, y1, x1, WorkingAreaHeight);
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
        protected void drawRect(FCPaint paint, long dwPenColor, int width, int style, int left, int top, int right, int bottom) {
            left += getPx();
            top += getPy();
            right += getPx();
            bottom += getPy();
            FCRect rect = new FCRect(left, top, right, bottom);
            if (paint.supportTransparent()) {
                paint.drawRect(dwPenColor, width, style, rect);
                FCChart chart = Chart;
                FCPoint mp = chart.TouchPoint;
                if (!m_isPaintingGhost && (mp.y >= m_div.Top && mp.y <= m_div.Bottom) &&
                (chart.MovingPlot == this ||
                (chart == Native.HoveredControl
                && !chart.isOperating() && onSelect()))) {
                    int a = 0, r = 0, g = 0, b = 0;
                    FCColor.toArgb(paint, dwPenColor, ref a, ref r, ref g, ref b);
                    if (a == 255) {
                        a = 50;
                    }
                    dwPenColor = FCColor.argb(a, r, g, b);
                    width += 10;
                    paint.drawRect(dwPenColor, width, 0, rect);
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
        protected void drawRect(FCPaint paint, long dwPenColor, int width, int style, float left, float top, float right, float bottom) {
            left += getPx();
            top += getPy();
            right += getPx();
            bottom += getPy();
            FCRect rect = new FCRect(left, top, right, bottom);
            paint.drawRect(dwPenColor, width, style, rect);
            if (paint.supportTransparent()) {
                FCChart chart = Chart;
                FCPoint mp = chart.TouchPoint;
                if (!m_isPaintingGhost && (mp.y >= m_div.Top && mp.y <= m_div.Bottom) &&
                (chart.MovingPlot == this ||
                (chart == Native.HoveredControl
                && !chart.isOperating() && onSelect()))) {
                    int a = 0, r = 0, g = 0, b = 0;
                    FCColor.toArgb(paint, dwPenColor, ref a, ref r, ref g, ref b);
                    if (a == 255) {
                        a = 50;
                    }
                    dwPenColor = FCColor.argb(a, r, g, b);
                    width += 10;
                    paint.drawRect(dwPenColor, width, 0, rect);
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
        protected void drawPolyline(FCPaint paint, long dwPenColor, int width, int style, FCPoint[] points) {
            int px = getPx();
            int py = getPy();
            for (int i = 0; i < points.Length; i++) {
                FCPoint point = new FCPoint(px + points[i].x, py + points[i].y);
                points[i] = point;
            }
            paint.drawPolyline(dwPenColor, width, style, points);
            if (paint.supportTransparent()) {
                FCChart chart = Chart;
                FCPoint mp = chart.TouchPoint;
                if (!m_isPaintingGhost && (mp.y >= m_div.Top && mp.y <= m_div.Bottom) &&
                (chart.MovingPlot == this ||
                (chart == Native.HoveredControl
                && !chart.isOperating() && onSelect()))) {
                    int a = 0, r = 0, g = 0, b = 0;
                    FCColor.toArgb(paint, dwPenColor, ref a, ref r, ref g, ref b);
                    if (a == 255) {
                        a = 50;
                    }
                    dwPenColor = FCColor.argb(a, r, g, b);
                    width += 10;
                    paint.drawPolyline(dwPenColor, width, 0, points);
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
        protected void drawSelect(FCPaint paint, long dwPenColor, float x, float y) {
            x += getPx();
            y += getPy();
            int sub = m_lineWidth * 3;
            FCRect rect = new FCRect(x - sub, y - sub, x + sub, y + sub);
            if (SelectedPoint == SelectedPoint.Ellipse) {
                paint.fillEllipse(dwPenColor, rect);
            }
            else if (SelectedPoint == SelectedPoint.Rectangle) {
                paint.fillRect(dwPenColor, rect);
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
        protected void drawText(FCPaint paint, String text, long dwPenColor, FCFont font, int x, int y) {
            x += getPx();
            y += getPy();
            FCSize tSize = paint.textSize(text, font);
            FCRect tRect = new FCRect(x, y, x + tSize.cx, y + tSize.cy);
            paint.drawText(text, dwPenColor, font, tRect);
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
        protected void drawText(FCPaint paint, String text, long dwPenColor, FCFont font, float x, float y) {
            x += getPx();
            y += getPy();
            FCSize tSize = paint.textSize(text, font);
            FCRect tRect = new FCRect(x, y, x + tSize.cx, y + tSize.cy);
            paint.drawText(text, dwPenColor, font, tRect);
        }

        /// <summary>
        /// 填充多边形
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="color">颜色</param>
        /// <param name="points">点的集合</param>
        protected void fillPolygon(FCPaint paint, long dwPenColor, FCPoint[] points) {
            int px = getPx();
            int py = getPy();
            for (int i = 0; i < points.Length; i++) {
                FCPoint point = new FCPoint(px + points[i].x, py + points[i].y);
                points[i] = point;
            }
            paint.fillPolygon(dwPenColor, points);
        }

        /// <summary>
        /// 获取K线区域内最高价和最低价
        /// </summary>
        /// <param name="pList">点阵集合</param>
        /// <returns>幅度尺的参数</returns>
        protected double[] getCandleRange(HashMap<int, PlotMark> pList) {
            if (pList.size() == 0 || m_sourceFields.size() == 0) {
                return null;
            }
            if (!m_sourceFields.containsKey("HIGH") || !m_sourceFields.containsKey("LOW")) {
                return null;
            }
            int bRecord = m_dataSource.getRowIndex(pList.get(0).Key);
            int eRecord = m_dataSource.getRowIndex(pList.get(1).Key);
            double[] highlist = null;
            double[] lowlist = null;
            if (eRecord >= bRecord) {
                highlist = m_dataSource.DATA_ARRAY(m_sourceFields.get("HIGH"), eRecord, eRecord - bRecord + 1);
                lowlist = m_dataSource.DATA_ARRAY(m_sourceFields.get("LOW"), eRecord, eRecord - bRecord + 1);
            }
            else {
                highlist = m_dataSource.DATA_ARRAY(m_sourceFields.get("HIGH"), bRecord, bRecord - eRecord + 1);
                lowlist = m_dataSource.DATA_ARRAY(m_sourceFields.get("LOW"), bRecord, bRecord - eRecord + 1);
            }
            double nHigh = 0, nLow = 0;
            nHigh = FCScript.maxValue(highlist, highlist.Length);
            nLow = FCScript.minValue(lowlist, lowlist.Length);
            return new double[] { nHigh, nLow };
        }

        /// <summary>
        /// 获取动作
        /// </summary>
        /// <returns>动作类型</returns>
        public virtual ActionType getAction() {
            return ActionType.NO;
        }

        /// <summary>
        /// 根据坐标获取索引
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>索引</returns>
        protected int getIndex(FCPoint mp) {
            FCChart chart = Chart;
            mp.x += chart.LeftVScaleWidth;
            mp.y += m_div.Top + m_div.TitleBar.Height;
            return chart.getIndex(mp);
        }

        /// <summary>
        /// 获取直线的参数
        /// </summary>
        /// <param name="markA">第一个点</param>
        /// <param name="markB">第二个点</param>
        /// <returns>直线的参数</returns>
        protected float[] getLineParams(PlotMark markA, PlotMark markB) {
            float y1 = py(markA.Value);
            float y2 = py(markB.Value);
            int bIndex = m_dataSource.getRowIndex(markA.Key);
            int eIndex = m_dataSource.getRowIndex(markB.Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            float a = 0, b = 0;
            if (x2 - x1 != 0) {
                lineXY(x1, y1, x2, y2, 0, 0, ref a, ref b);
                return new float[] { a, b };
            }
            else {
                return null;
            }
        }

        /// <summary>
        /// 获取线性回归带的高低点偏离值
        /// </summary>
        /// <param name="pList">点阵集合</param>
        /// <param name="param">直线参数</param>
        /// <returns>高低点偏离值</returns>
        protected double[] getLRBandRange(HashMap<int, PlotMark> pList, float[] param) {
            if (m_sourceFields != null && m_sourceFields.size() > 0 && m_sourceFields.containsKey("HIGH")
                && m_sourceFields.containsKey("LOW")) {
                float a = param[0];
                float b = param[1];
                ArrayList<double> upList = new ArrayList<double>();
                ArrayList<double> downList = new ArrayList<double>();
                int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
                int eIndex = m_dataSource.getRowIndex(pList.get(1).Key);
                for (int i = bIndex; i <= eIndex; i++) {
                    double high = m_dataSource.get2(i, m_sourceFields.get("HIGH"));
                    double low = m_dataSource.get2(i, m_sourceFields.get("LOW"));
                    if (!double.IsNaN(high) && !double.IsNaN(low)) {
                        double midValue = (i - bIndex + 1) * a + b;
                        upList.add(high - midValue);
                        downList.add(midValue - low);
                    }
                }
                double upSubValue = FCScript.maxValue(upList.ToArray(), upList.size());
                double downSubValue = FCScript.maxValue(downList.ToArray(), downList.size());
                return new double[] { upSubValue, downSubValue };
            }
            return null;
        }

        /// <summary>
        /// 获取线性回归的参数
        /// </summary>
        /// <param name="marks">点阵集合</param>
        /// <returns>点阵数组</returns>
        protected float[] getLRParams(HashMap<int, PlotMark> marks) {
            if (m_sourceFields != null && m_sourceFields.containsKey("CLOSE")) {
                int bIndex = m_dataSource.getRowIndex(marks.get(0).Key);
                int eIndex = m_dataSource.getRowIndex(marks.get(1).Key);
                if (bIndex != -1 && eIndex != -1) {
                    ArrayList<double> closeVList = new ArrayList<double>();
                    for (int i = bIndex; i <= eIndex; i++) {
                        double value = m_dataSource.get2(i, m_sourceFields.get("CLOSE"));
                        if (!double.IsNaN(value)) {
                            closeVList.add(value);
                        }
                    }
                    if (closeVList.size() > 0) {
                        float a = 0;
                        float b = 0;
                        FCScript.linearRegressionEquation(closeVList.ToArray(), closeVList.size(), ref a, ref b);
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
        protected FCPoint getMovingPoint() {
            FCPoint mp = getTouchOverPoint();
            if (mp.x < 0) {
                mp.x = 0;
            }
            else if (mp.x > WorkingAreaWidth) {
                mp.x = WorkingAreaWidth;
            }
            if (mp.y < 0) {
                mp.y = 0;
            }
            else if (mp.y > WorkingAreaHeight) {
                mp.y = WorkingAreaHeight;
            }
            return mp;
        }

        /// <summary>
        /// 根据坐标获取数值
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>数值</returns>
        protected double getNumberValue(FCPoint mp) {
            FCChart chart = Chart;
            mp.x += chart.LeftVScaleWidth;
            mp.y += m_div.Top + m_div.TitleBar.Height;
            return chart.getNumberValue(m_div, mp, m_attachVScale);
        }

        /// <summary>
        /// 获取偏移横坐标
        /// </summary>
        /// <returns>偏移横坐标</returns>
        protected int getPx() {
            FCChart chart = Chart;
            return chart.LeftVScaleWidth;
        }

        /// <summary>
        /// 获取偏移纵坐标
        /// </summary>
        /// <returns>偏移纵坐标</returns>
        protected int getPy() {
            return m_div.TitleBar.Height;
        }

        /// <summary>
        /// 根据两点获取矩形
        /// </summary>
        /// <param name="markA">第一个点</param>
        /// <param name="markB">第二个点</param>
        /// <returns>矩形对象</returns>
        protected FCRect getRectangle(PlotMark markA, PlotMark markB) {
            double aValue = markA.Value;
            double bValue = markB.Value;
            int aIndex = m_dataSource.getRowIndex(markA.Key);
            int bIndex = m_dataSource.getRowIndex(markB.Key);
            float x = px(aIndex);
            float y = py(aValue);
            float xS = px(bIndex);
            float yS = py(bValue);
            float width = Math.Abs(xS - x);
            if (width < 4) {
                width = 4;
            }
            float height = Math.Abs(yS - y);
            if (height < 4) {
                height = 4;
            }
            float rX = x <= xS ? x : xS;
            float rY = y <= yS ? y : yS;
            return new FCRect(rX, rY, rX + width, rY + height);
        }

        /// <summary>
        /// 获取瞄准的坐标
        /// </summary>
        /// <returns></returns>
        protected FCPoint getTouchOverPoint() {
            FCChart chart = Chart;
            FCPoint mp = chart.TouchPoint;
            mp.x -= chart.LeftVScaleWidth;
            mp.y = mp.y - m_div.Top - m_div.TitleBar.Height;
            return mp;
        }

        /// <summary>
        /// 获取黄金分割线的直线参数
        /// </summary>
        /// <param name="value1">值1</param>
        /// <param name="value2">值2</param>
        /// <returns>黄金分割线的直线参数</returns>
        protected float[] GoldenRatioParams(double value1, double value2) {
            float y1 = py(value1);
            float y2 = py(value2);
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
        protected bool hLinesSelect(float[] param, int length) {
            FCPoint mp = getTouchOverPoint();
            //获取上下区间
            float top = 0;
            float bottom = WorkingAreaHeight;
            if (mp.y >= top && mp.y <= bottom) {
                foreach (float p in param) {
                    //判断每个点
                    if (mp.y >= p - m_lineWidth * 2.5f && mp.y <= p + m_lineWidth * 2.5f) {
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
        protected void move(FCPoint mp) {
            VScale vScale = m_div.getVScale(m_attachVScale);
            //循环处理
            int startMarkSize = m_startMarks.size();
            for (int i = 0; i < startMarkSize; i++) {
                int newIndex = 0;
                double yAddValue = 0;
                int startIndex = m_dataSource.getRowIndex(m_startMarks.get(i).Key);
                //调用函数
                movePlot(mp.y, m_startPoint.y, startIndex, getIndex(m_startPoint), getIndex(mp), WorkingAreaHeight, vScale.VisibleMax,
                vScale.VisibleMin, m_dataSource.RowsCount, ref yAddValue, ref newIndex);
                //纵轴反转
                if (vScale.Reverse) {
                    m_marks.put(i, new PlotMark(i, m_dataSource.getXValue(newIndex), m_startMarks.get(i).Value - yAddValue));
                }
                else {
                    m_marks.put(i, new PlotMark(i, m_dataSource.getXValue(newIndex), m_startMarks.get(i).Value + yAddValue));
                }
            }
        }

        /// <summary>
        /// 移动画线
        /// </summary>
        /// <param name="touchY">纵坐标</param>
        /// <param name="startY">开始纵坐标</param>
        /// <param name="startIndex">开始索引</param>
        /// <param name="touchBeginIndex">触摸开始索引</param>
        /// <param name="touchEndIndex">触摸结束索引</param>
        /// <param name="pureV">纵向距离</param>
        /// <param name="max">最大值</param>
        /// <param name="min">最小值</param>
        /// <param name="dataCount">数据条数</param>
        /// <param name="yAddValue">纵向变化值</param>
        /// <param name="newIndex">新的索引</param>
        private void movePlot(float touchY, float startY, int startIndex, int touchBeginIndex, int touchEndIndex, float pureV,
        double max, double min, int dataCount, ref double yAddValue, ref int newIndex) {
            float subY = touchY - startY;
            yAddValue = ((min - max) * subY / pureV);
            newIndex = startIndex + (touchEndIndex - touchBeginIndex);
            if (newIndex < 0) {
                newIndex = 0;
            }
            else if (newIndex > dataCount - 1) {
                newIndex = dataCount - 1;
            }
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public virtual bool onCreate(FCPoint mp) {
            return false;
        }

        /// <summary>
        /// 移动结束
        /// </summary>
        public virtual void onMoveEnd() {
        }

        /// <summary>
        /// 移动开始
        /// </summary>
        public virtual void onMoveStart() {
        }

        /// <summary>
        /// 移动
        /// </summary>
        public virtual void onMoving() {
            FCPoint mp = getMovingPoint();
            //根据不同类型作出动作
            switch (m_action) {
                case ActionType.MOVE:
                    move(mp);
                    break;
                case ActionType.AT1:
                    resize(0);
                    break;
                case ActionType.AT2:
                    resize(1);
                    break;
                case ActionType.AT3:
                    resize(2);
                    break;
                case ActionType.AT4:
                    resize(3);
                    break;
            }
        }

        /// <summary>
        /// 绘图对象
        /// </summary>
        /// <param name="paint">绘图对象</param>
        public virtual void onPaint(FCPaint paint) {
            FCChart chart = Chart;
            FCPoint mp = chart.TouchPoint;
            if ((mp.y >= m_div.Top && mp.y <= m_div.Bottom) &&
                (chart.MovingPlot == this ||
                (chart == Native.HoveredControl
                && !chart.isOperating() && onSelect()))) {
                onPaint(paint, m_marks, m_selectedColor);
            }
            else {
                onPaint(paint, m_marks, m_color);
            }
        }

        /// <summary>
        /// 绘制图像的残影
        /// </summary>
        /// <param name="paint">绘图对象</param>
        public virtual void onPaintGhost(FCPaint paint) {
            m_isPaintingGhost = true;
            onPaint(paint, m_startMarks, m_color);
            m_isPaintingGhost = false;
        }

        /// <summary>
        /// 判断选中
        /// </summary>
        /// <returns>是否选中</returns>
        public virtual bool onSelect() {
            ActionType action = getAction();
            if (action != ActionType.NO) {
                return true;
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected virtual void onPaint(FCPaint paint, HashMap<int, PlotMark> pList, long lineColor) {

        }

        /// <summary>
        /// 获取绘制横坐标
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>横坐标</returns>
        protected float px(int index) {
            FCChart chart = Chart;
            float x = chart.getX(index);
            return x - chart.LeftVScaleWidth;
        }

        /// <summary>
        /// 获取绘制纵坐标
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>纵坐标</returns>
        protected float py(double value) {
            FCChart chart = Chart;
            float y = chart.getY(m_div, value, m_attachVScale);
            return y - m_div.TitleBar.Height;
        }

        /// <summary>
        /// 根据横坐标获取画线工具内部横坐标
        /// </summary>
        /// <param name="x">横坐标</param>
        /// <returns>内部横坐标</returns>
        protected float px(float x) {
            FCChart chart = Chart;
            return x - chart.LeftVScaleWidth;
        }

        /// <summary>
        /// 根据纵坐标获取画线工具内部纵坐标
        /// </summary>
        /// <param name="y">纵坐标</param>
        /// <returns>内部纵坐标</returns>
        protected float py(float y) {
            return y - m_div.Top - m_div.TitleBar.Height;
        }

        /// <summary>
        /// 绘制到图像上
        /// </summary>
        /// <param name="paint">绘图对象</param>
        public virtual void render(FCPaint paint) {
            FCChart chart = Chart;
            if (m_drawGhost && chart.MovingPlot != null && chart.MovingPlot == this) {
                onPaintGhost(paint);
            }
            onPaint(paint);
        }

        /// <summary>
        /// 调整大小
        /// </summary>
        /// <param name="index">索引</param>
        protected void resize(int index) {
            FCChart chart = Chart;
            int touchIndex = chart.getTouchOverIndex();
            double y = getNumberValue(getMovingPoint());
            if (touchIndex < 0) {
                touchIndex = 0;
            }
            if (touchIndex > chart.LastVisibleIndex) {
                touchIndex = chart.LastVisibleIndex;
            }
            m_marks.put(index, new PlotMark(index, m_dataSource.getXValue(touchIndex), y));
        }

        /// <summary>
        /// 重新调整大小
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <param name="oppositePoint">点位置</param>
        protected void resize(FCPoint mp, FCPoint oppositePoint) {
            double sValue = getNumberValue(new FCPoint(oppositePoint.x, oppositePoint.y));
            double eValue = getNumberValue(mp);
            int iS = getIndex(new FCPoint(oppositePoint.x, oppositePoint.y));
            int iE = getIndex(mp);
            double topValue = 0;
            double bottomValue = 0;
            VScale vScale = m_div.getVScale(m_attachVScale);
            if (sValue >= eValue) {
                if (vScale.Reverse) {
                    topValue = eValue;
                    bottomValue = sValue;
                }
                else {
                    topValue = sValue;
                    bottomValue = eValue;
                }
            }
            else {
                if (vScale.Reverse) {
                    topValue = sValue;
                    bottomValue = eValue;
                }
                else {
                    topValue = eValue;
                    bottomValue = sValue;
                }
            }
            double sDate = 0;
            double eDate = 0;
            if (iS < 0) {
                iS = 0;
            }
            else if (iS > m_dataSource.RowsCount - 1) {
                iS = m_dataSource.RowsCount - 1;
            }
            if (iE < 0) {
                iE = 0;
            }
            else if (iE > m_dataSource.RowsCount - 1) {
                iE = m_dataSource.RowsCount - 1;
            }
            if (iS >= iE) {
                sDate = m_dataSource.getXValue(iE);
                eDate = m_dataSource.getXValue(iS);
            }
            else {
                sDate = m_dataSource.getXValue(iS);
                eDate = m_dataSource.getXValue(iE);
            }
            //横轴反转
            m_marks.put(0, new PlotMark(0, sDate, topValue));
            m_marks.put(1, new PlotMark(1, eDate, bottomValue));
        }

        /// <summary>
        /// 判断是否选中了指定的点
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <param name="x">点的横坐标</param>
        /// <param name="y">点的纵坐标</param>
        /// <returns>是否选中</returns>
        protected bool selectPoint(FCPoint mp, float x, float y) {
            if (mp.x >= x - m_lineWidth * 6 && mp.x <= x + m_lineWidth * 6
                    && mp.y >= y - m_lineWidth * 6 && mp.y <= y + m_lineWidth * 6) {
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
        protected bool selectLine(FCPoint mp, float x, float k, float b) {
            if (!(k == 0 && b == 0)) {
                if (mp.y / (mp.x * k + b) >= 0.95 && mp.y / (mp.x * k + b) <= 1.05) {
                    return true;
                }
            }
            else {
                if (mp.x >= x - m_lineWidth * 5 && mp.x <= x + m_lineWidth * 5) {
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
        protected bool selectLine(FCPoint mp, float x1, float y1, float x2, float y2) {
            float k = 0, b = 0;
            //获取直线参数
            lineXY(x1, y1, x2, y2, 0, 0, ref k, ref b);
            return selectLine(mp, x1, k, b);
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
        protected bool selectRay(FCPoint mp, float x1, float y1, float x2, float y2, ref float k, ref float b) {
            //获取直线参数
            lineXY(x1, y1, x2, y2, 0, 0, ref k, ref b);
            if (!(k == 0 && b == 0)) {
                if (mp.y / (mp.x * k + b) >= 0.95 && mp.y / (mp.x * k + b) <= 1.05) {
                    if (x1 >= x2) {
                        if (mp.x > x1 + 5) return false;
                    }
                    else if (x1 < x2) {
                        if (mp.x < x1 - 5) return false;
                    }
                    return true;
                }
            }
            else {
                if (mp.x >= x1 - m_lineWidth * 5 && mp.x <= x1 + m_lineWidth * 5) {
                    if (y1 >= y2) {
                        if (mp.y <= y1 - 5) {
                            return true;
                        }
                    }
                    else {
                        if (mp.y >= y1 - 5) {
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
        protected bool selectRay(FCPoint mp, float x1, float y1, float x2, float y2) {
            float k = 0, b = 0;
            return selectRay(mp, x1, y1, x2, y2, ref k, ref b);
        }

        /// <summary>
        /// 获取选中状态
        /// </summary>
        /// <param name="mp">点的位置</param>
        /// <param name="markA">点A</param>
        /// <param name="markB">点B</param>
        /// <returns>选中状态</returns>
        protected ActionType selectRect(FCPoint mp, PlotMark markA, PlotMark markB) {
            FCRect rect = getRectangle(markA, markB);
            int x1 = rect.left;
            int y1 = rect.top;
            int x2 = rect.right;
            int y2 = rect.top;
            int x3 = rect.left;
            int y3 = rect.bottom;
            int x4 = rect.right;
            int y4 = rect.bottom;
            //判断是否选中点
            if (selectPoint(mp, x1, y1)) {
                return ActionType.AT1;
            }
            else if (selectPoint(mp, x2, y2)) {
                return ActionType.AT2;
            }
            else if (selectPoint(mp, x3, y3)) {
                return ActionType.AT3;
            }
            else if (selectPoint(mp, x4, y4)) {
                return ActionType.AT4;
            }
            else {
                int sub = (int)(m_lineWidth * 2.5);
                FCRect bigRect = new FCRect(rect.left - sub, rect.top - sub, rect.right + sub, rect.bottom + sub);
                if (mp.x >= bigRect.left && mp.x <= bigRect.right && mp.y >= bigRect.top && mp.y <= bigRect.bottom) {
                    if (rect.right - rect.left <= 4 || rect.bottom - rect.top <= 4) {
                        return ActionType.MOVE;
                    }
                    else {
                        FCRect smallRect = new FCRect(rect.left + sub, rect.top + sub, rect.right - sub, rect.bottom - sub);
                        if (!(mp.x >= smallRect.left && mp.x <= smallRect.right && mp.y >= smallRect.top && mp.y <= smallRect.bottom)) {
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
        protected bool selectSegment(FCPoint mp, float x1, float y1, float x2, float y2) {
            float k = 0;
            float b = 0;
            //获取直线参数
            lineXY(x1, y1, x2, y2, 0, 0, ref k, ref b);
            float smallX = x1 <= x2 ? x1 : x2;
            float smallY = y1 <= y2 ? y1 : y2;
            float bigX = x1 > x2 ? x1 : x2;
            float bigY = y1 > y2 ? y1 : y2;
            if (mp.x >= smallX - 2 && mp.x <= bigX + 2 && mp.y >= smallY - 2 && mp.y <= bigY + 2) {
                if (!(k == 0 && b == 0)) {
                    if (mp.y / (mp.x * k + b) >= 0.95 && mp.y / (mp.x * k + b) <= 1.05) {
                        return true;
                    }
                }
                else {
                    if (mp.x >= x1 - m_lineWidth * 5 && mp.x <= x1 + m_lineWidth * 5) {
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
        protected bool selectSine(FCPoint mp, float x1, float y1, float x2, float y2) {
            double f = 2.0 * Math.PI / ((x2 - x1) * 4);
            int len = WorkingAreaWidth;
            if (len > 0) {
                float lastX = 0, lastY = 0;
                for (int i = 0; i < len; i++) {
                    float x = -x1 + i;
                    float y = (float)(0.5 * (y2 - y1) * Math.Sin(x * f) * 2);
                    float px = x + x1, py = y + y1;
                    if (i == 0) {
                        if (selectPoint(mp, px, py)) {
                            return true;
                        }
                    }
                    else {
                        float rectLeft = lastX - 2;
                        float rectTop = lastY <= py ? lastY : py - 2;
                        float rectRight = rectLeft + Math.Abs(px - lastX) + 4;
                        float rectBottom = rectTop + Math.Abs(py - lastY) + 4;
                        if (mp.x >= rectLeft && mp.x <= rectRight && mp.y >= rectTop && mp.y <= rectBottom) {
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
        protected bool selectTriangle(FCPoint mp, float x1, float y1, float x2, float y2, float x3, float y3) {
            bool selected = selectSegment(mp, x1, y1, x2, y2);
            if (selected) return true;
            selected = selectSegment(mp, x1, y1, x3, y3);
            if (selected) return true;
            selected = selectSegment(mp, x2, y2, x3, y3);
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
        protected FCSize textSize(FCPaint paint, String text, FCFont font) {
            return paint.textSize(text, font);
        }


        public static void ellipseAB(float width, float height, ref float a, ref float b) {
            a = width / 2;
            b = height / 2;
        }

        public static bool ellipseHasPoint(float x, float y, float oX, float oY, float a, float b) {
            x -= oX;
            y -= oY;
            if (a == 0 && b == 0 && x == 0 && y == 0) {
                return true;
            }
            if (a == 0) {
                if (x == 0 && y >= -b && y <= b) {
                    return false;
                }
            }
            if (b == 0) {
                if (y == 0 && x >= -a && x <= a) {
                    return true;
                }
            }
            if ((x * x) / (a * a) + (y * y) / (b * b) >= 0.8 && (x * x) / (a * a) + (y * y) / (b * b) <= 1.2) {
                return true;
            }
            return false;
        }

        public static void ellipseOR(float x1, float y1, float x2, float y2, float x3, float y3, ref float oX, ref float oY, ref float r) {
            oX = ((y3 - y1) * (y2 * y2 - y1 * y1 + x2 * x2 - x1 * x1) + (y2 - y1) * (y1 * y1 - y3 * y3 + x1 * x1 - x3 * x3))
                / (2 * (x2 - x1) * (y3 - y1) - 2 * (x3 - x1) * (y2 - y1));
            oY = ((x3 - x1) * (x2 * x2 - x1 * x1 + y2 * y2 - y1 * y1) + (x2 - x1) * (x1 * x1 - x3 * x3 + y1 * y1 - y3 * y3))
                / (2 * (y2 - y1) * (x3 - x1) - 2 * (y3 - y1) * (x2 - x1));
            r = (float)Math.Sqrt((x1 - oX) * (x1 - oX) + (y1 - oY) * (y1 - oY));
        }

        public static double lineSlope(float x1, float y1, float x2, float y2, float oX, float oY) {
            if ((x1 - oX) != (x2 - oX)) {
                return ((y2 - oY) - (y1 - oY)) / ((x2 - oX) - (x1 - oX));
            }
            return 0;
        }

        public static void lineXY(float x1, float y1, float x2, float y2, float oX, float oY, ref float k, ref float b) {
            k = 0;
            b = 0;
            if ((x1 - oX) != (x2 - oX)) {
                k = ((y2 - oY) - (y1 - oY)) / ((x2 - oX) - (x1 - oX));
                b = (y1 - oY) - k * (x1 - oX);
            }
        }

        public static void parallelogram(float x1, float y1, float x2, float y2, float x3, float y3, ref float x4, ref float y4) {
            x4 = x1 + x3 - x2;
            y4 = y1 + y3 - y2;
        }

        public static void rectangleXYWH(int x1, int y1, int x2, int y2, ref int x, ref int y, ref int w, ref int h) {
            x = x1 < x2 ? x1 : x2;
            y = y1 < y2 ? y1 : y2;
            w = Math.Abs(x1 - x2);
            h = Math.Abs(y1 - y2);
            if (w <= 0) {
                w = 4;
            }
            if (h <= 0) {
                h = 4;
            }
        }
    }
}
