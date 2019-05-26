/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.chart;

import facecat.topin.core.*;
import facecat.topin.chart.*;
import facecat.topin.plot.*;
import java.util.*;

/**
 * 画线工具的基类
 */
public class FCPlot {

    /**
     * 创建画线
     */
    public FCPlot() {
        m_color = FCColor.argb(255, 255, 255);
        m_selectedColor = FCColor.argb(255, 255, 255);
    }

    /**
     * 析构函数
     */
    protected void finalize() throws Throwable {
        delete();
    }

    /**
     * 移动类型
     */
    protected ActionType m_action = ActionType.NO;

    /**
     * 数据源
     */
    protected FCDataTable m_dataSource = null;

    /**
     * 是否正在绘制阴影
     */
    protected boolean m_isPaintingGhost = false;

    /**
     * 关键点
     */
    protected java.util.HashMap<Integer, PlotMark> m_marks = new java.util.HashMap<Integer, PlotMark>();

    /**
     * 移动次数
     */
    protected int m_moveTimes = 0;

    /**
     * 源字段
     */
    protected java.util.HashMap<String, Integer> m_sourceFields = new java.util.HashMap<String, Integer>();

    /**
     * 开始移动时的值
     */
    protected java.util.HashMap<Integer, PlotMark> m_startMarks = new java.util.HashMap<Integer, PlotMark>();

    /**
     * 开始移动的点
     */
    protected FCPoint m_startPoint = new FCPoint();

    protected AttachVScale m_attachVScale = AttachVScale.Left;

    /**
     * 获取附着在左轴还是右轴
     */
    public AttachVScale getAttachVScale() {
        return m_attachVScale;
    }

    /**
     * 置附着在左轴还是右轴
     */
    public void setAttachVScale(AttachVScale value) {
        m_attachVScale = value;
    }

    protected long m_color;

    /**
     * 获取线的颜色
     */
    public long getColor() {
        return m_color;
    }

    /**
     * 设置线的颜色
     */
    public void setColor(long value) {
        m_color = value;
    }

    protected ChartDiv m_div = null;

    /**
     * 获取所在图层
     */
    public ChartDiv getDiv() {
        return m_div;
    }

    /**
     * 设置所在图层
     */
    public void setDiv(ChartDiv value) {
        m_div = value;
        m_dataSource = m_div.getChart().getDataSource();
    }

    protected boolean m_drawGhost = true;

    /**
     * 获取是否画移动残影
     */
    public boolean getDrawGhost() {
        return m_drawGhost;
    }

    /**
     * 设置是否画移动残影
     */
    public void setDrawGhost(boolean value) {
        m_drawGhost = value;
    }

    protected boolean m_enabled = true;

    /**
     * 获取是否可以选中或拖放
     */
    public boolean isEnabled() {
        return m_enabled;
    }

    /**
     * 设置是否可以选中或拖放
     */
    public void setEnabled(boolean value) {
        if (!value) {
            m_selected = false;
        }
        m_enabled = value;
    }

    protected FCFont m_font = new FCFont();

    /**
     * 获取字体
     */
    public FCFont getFont() {
        return m_font;
    }

    /**
     * 设置字体
     */
    public void setFont(FCFont value) {
        m_font = value;
    }

    protected boolean m_isDeleted = false;

    /**
     * 获取是否已被销毁
     */
    public boolean isDeleted() {
        return m_isDeleted;
    }

    /**
     * 设置是否已被销毁
     */
    public FCChart getChart() {
        return m_div.getChart();
    }

    protected int m_lineStyle = 0;

    /**
     * 获取线的样式，null为实线
     */
    public int getLineStyle() {
        return m_lineStyle;
    }

    /**
     * 设置线的样式，null为实线
     */
    public void setLineStyle(int value) {
        m_lineStyle = value;
    }

    protected int m_lineWidth = 1;

    /**
     * 获取线的宽度
     */
    public int getLineWidth() {
        return m_lineWidth;
    }

    /**
     * 设置线的宽度
     */
    public void setLineWidth(int value) {
        m_lineWidth = value;
    }

    /**
     * 获取方法库
     */
    protected FCNative getNative() {
        return m_div.getChart().getNative();
    }

    protected String m_plotType = "LINE";

    /**
     * 获取线条的类型
     */
    public String getPlotType() {
        return m_plotType;
    }

    /**
     * 设置线条的类型
     */
    public void setPlotType(String value) {
        m_plotType = value;
    }

    protected boolean m_selected = false;

    /**
     * 获取是否被选中
     */
    public boolean isSelected() {
        return m_selected;
    }

    /**
     * 设置是否被选中
     */
    public void setSelected(boolean value) {
        m_selected = value;
    }

    protected long m_selectedColor;

    /**
     * 获取选中色
     */
    public long getSelectedColor() {
        return m_selectedColor;
    }

    /**
     * 设置选中色
     */
    public void setSelectedColor(long value) {
        m_selectedColor = value;
    }

    protected SelectedPoint m_SelectedPoint = SelectedPoint.Rectangle;

    /**
     * 获取选中点的样式
     */
    public SelectedPoint getSelectedPoint() {
        return m_SelectedPoint;
    }

    /**
     * 设置选中点的样式
     */
    public void setSelectedPoint(SelectedPoint value) {
        m_SelectedPoint = value;
    }

    protected String m_text;

    /**
     * 获取显示的文字
     */
    public String getText() {
        return m_text;
    }

    /**
     *设置显示的文字
     */
    public void setText(String value) {
        m_text = value;
    }

    protected boolean m_visible = true;

    /**
     * 获取可见度
     */
    public boolean isVisible() {
        return m_visible;
    }

    /**
     * 设置可见度
     */
    public void setVisible(boolean value) {
        if (!value) {
            m_selected = false;
        }
        m_visible = value;
    }

    /**
     * 获取区域宽度
     *
     * @returns 宽度
     */
    protected int getWorkingAreaWidth() {
        return getChart().getWorkingAreaWidth();
    }

    /**
     * 获取区域高度
     *
     * @returns 高度
     */
    protected int getWorkingAreaHeight() {
        return m_div.getWorkingAreaHeight();
    }

    protected int m_zOrder = 0;

    /**
     * 获取图层顺序
     */
    public int getZOrder() {
        return m_zOrder;
    }

    /**
     * 设置图层顺序
     */
    public void setZOrder(int value) {
        m_zOrder = value;
    }

    /**
     * 初始化一个点类型的通用方法
     *
     * @param mp 坐标
     * @returns 是否初始化成功
     */
    protected boolean createPoint(FCPoint mp) {
        int rIndex = m_dataSource.getRowsCount();
        if (rIndex > 0) {
            // 获取点的位置
            FCChart chart = getChart();
            int touchIndex = chart.getIndex(mp);
            if (touchIndex >= chart.getFirstVisibleIndex() && touchIndex <= chart.getLastVisibleIndex()) {
                double sDate = m_dataSource.getXValue(touchIndex);
                m_marks.clear();
                double y = getNumberValue(mp);
                m_marks.put(0, new PlotMark(0, sDate, y));
                return true;
            }
        }
        return false;
    }

    /**
     * 初始化两个点类型的通用方法
     *
     * @param mp 坐标
     * @returns 是否初始化成功
     */
    protected boolean create2PointsA(FCPoint mp) {
        int rIndex = m_dataSource.getRowsCount();
        if (rIndex > 0) {
            // 获取点的位置
            FCChart chart = getChart();
            int touchIndex = chart.getIndex(mp);
            if (touchIndex >= chart.getFirstVisibleIndex() && touchIndex <= chart.getLastVisibleIndex()) {
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

    /**
     * 初始化两个点类型的通用方法
     *
     * @param mp 坐标
     * @returns 是否初始化成功
     */
    protected boolean create2PointsB(FCPoint mp) {
        int rIndex = m_dataSource.getRowsCount();
        if (rIndex > 0) {
            FCChart chart = getChart();
            int touchIndex = chart.getIndex(mp);
            if (touchIndex >= chart.getFirstVisibleIndex() && touchIndex <= chart.getLastVisibleIndex()) {
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

    /**
     * 初始两个K线点的方法
     *
     * @param mp 坐标
     * @returns 是否初始化成功
     */
    protected boolean create2CandlePoints(FCPoint mp) {
        FCChart chart = getChart();
        int rIndex = m_dataSource.getRowsCount();
        if (rIndex > 0) {
            ArrayList<BaseShape> shapesCopy = m_div.getShapes(SortType.DESC);
            for (BaseShape bs : shapesCopy) {
                if (bs.isVisible()) {
                    CandleShape cs = (CandleShape) ((bs instanceof CandleShape) ? bs : null);
                    if (cs != null) {
                        int touchIndex = chart.getIndex(mp);
                        if (touchIndex >= chart.getFirstVisibleIndex() && touchIndex <= chart.getLastVisibleIndex()) {
                            int eIndex = touchIndex;
                            int bIndex = eIndex - 1;
                            if (bIndex >= 0) {
                                double fDate = m_dataSource.getXValue(bIndex);
                                double sDate = m_dataSource.getXValue(eIndex);
                                m_marks.clear();
                                double y = getNumberValue(mp);
                                m_marks.put(0, new PlotMark(0, fDate, y));
                                m_marks.put(1, new PlotMark(1, sDate, y));
                                m_sourceFields.put("CLOSE", cs.getCloseField());
                                m_sourceFields.put("OPEN", cs.getOpenField());
                                m_sourceFields.put("HIGH", cs.getHighField());
                                m_sourceFields.put("LOW", cs.getLowField());
                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }

    /**
     * 初始化三个点类型的通用方法
     *
     * @param mp 坐标
     * @returns 是否初始化成功
     */
    protected boolean create3Points(FCPoint mp) {
        int rIndex = m_dataSource.getRowsCount();
        if (rIndex > 0) {
            // 获取点的位置
            FCChart chart = getChart();
            int touchIndex = chart.getIndex(mp);
            if (touchIndex >= chart.getFirstVisibleIndex() && touchIndex <= chart.getLastVisibleIndex()) {
                int eIndex = touchIndex;
                int bIndex = eIndex - 1;
                if (bIndex >= 0) {
                    double fDate = m_dataSource.getXValue(bIndex);
                    double sDate = m_dataSource.getXValue(eIndex);
                    m_marks.clear();
                    double y = getNumberValue(mp);
                    m_marks.put(0, new PlotMark(0, fDate, y));
                    m_marks.put(1, new PlotMark(1, sDate, y));
                    if (m_div.getVScale(m_attachVScale) != null && m_div.getVScale(m_attachVScale).getVisibleMax() != m_div.getVScale(m_attachVScale).getVisibleMin()) {
                        m_marks.put(2, new PlotMark(2, fDate, y + (m_div.getVScale(m_attachVScale).getVisibleMax() - m_div.getVScale(m_attachVScale).getVisibleMin()) / 4));
                    } else {
                        m_marks.put(2, new PlotMark(2, fDate, y));
                    }
                    return true;
                }
            }
        }
        return false;
    }

    /**
     * 初始化对应K线上的点
     *
     * @param pos 位置
     * @param index 索引
     * @param close 收盘价字段
     */
    protected void createCandlePoint(int pos, int index, int close) {
        if (index >= 0) {
            if (index > m_dataSource.getRowsCount() - 1) {
                index = m_dataSource.getRowsCount() - 1;
            }
            double date = m_dataSource.getXValue(index);
            double yValue = 0;
            if (!Double.isNaN(m_dataSource.get2(index, close))) {
                yValue = m_dataSource.get2(index, close);
            }
            m_marks.put(pos, new PlotMark(pos, date, yValue));
        }
    }

    /**
     * 初始四个K线点的方法
     *
     * @param mp 坐标
     * @returns 是否初始化成功
     */
    protected boolean create4CandlePoints(FCPoint mp) {
        FCChart chart = getChart();
        int rIndex = m_dataSource.getRowsCount();
        if (rIndex > 0) {
            ArrayList<BaseShape> shapesCopy = m_div.getShapes(SortType.ASC);
            for (BaseShape bs : shapesCopy) {
                if (bs.isVisible() && bs instanceof CandleShape) {
                    CandleShape cs = (CandleShape) ((bs instanceof CandleShape) ? bs : null);
                    int touchIndex = chart.getIndex(mp);
                    if (touchIndex >= chart.getFirstVisibleIndex() && touchIndex <= chart.getLastVisibleIndex()) {
                        int closeField = cs.getCloseField();
                        createCandlePoint(0, touchIndex, closeField);
                        createCandlePoint(1, touchIndex + 1, closeField);
                        createCandlePoint(2, touchIndex + 2, closeField);
                        createCandlePoint(3, touchIndex + 3, closeField);
                        m_sourceFields.put("CLOSE", closeField);
                        m_sourceFields.put("HIGH", cs.getHighField());
                        m_sourceFields.put("LOW", cs.getLowField());
                        m_sourceFields.put("OPEN", cs.getOpenField());
                        return true;
                    }
                }
            }
        }
        return false;
    }

    /**
     * 销毁资源方法
     */
    public void delete() {
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

    /**
     * 画椭圆
     *
     * @param paint 绘图对象
     * @param dwPenColor 颜色
     * @param width 宽度
     * @param style 样式
     * @param left 横坐标
     * @param top 纵坐标
     * @param right 右侧横坐标
     * @param bottom 右侧纵坐标
     */
    protected void drawEllipse(FCPaint paint, long dwPenColor, int width, int style, float left, float top, float right, float bottom) {
        left += getPx();
        top += getPy();
        right += getPx();
        bottom += getPy();
        FCRect rect = new FCRect(left, top, right, bottom);
        paint.drawEllipse(dwPenColor, width, style, rect);
        if (paint.supportTransparent()) {
            FCChart chart = getChart();
            FCPoint mp = chart.getTouchPoint();
            if (!m_isPaintingGhost && (mp.y >= m_div.getTop() && mp.y <= m_div.getBottom()) && (chart.getMovingPlot() == this || (chart == getNative().getHoveredControl() && !chart.isOperating() && onSelect()))) {
                int a = 0, r = 0, g = 0, b = 0;
                RefObject<Integer> tempRef_a = new RefObject<Integer>(a);
                RefObject<Integer> tempRef_r = new RefObject<Integer>(r);
                RefObject<Integer> tempRef_g = new RefObject<Integer>(g);
                RefObject<Integer> tempRef_b = new RefObject<Integer>(b);
                FCColor.toArgb(paint, dwPenColor, tempRef_a, tempRef_r, tempRef_g, tempRef_b);
                a = tempRef_a.argvalue;
                r = tempRef_r.argvalue;
                g = tempRef_g.argvalue;
                b = tempRef_b.argvalue;
                if (a == 255) {
                    a = 50;
                }
                dwPenColor = FCColor.argb(a, r, g, b);
                width += 10;
                paint.drawEllipse(dwPenColor, width, 0, rect);
            }
        }
    }

    /**
     * 画线方法
     *
     * @param paint 绘图对象
     * @param dwPenColor 颜色
     * @param width 宽度
     * @param style 样式
     * @param x1 第一个点的横坐标
     * @param y1 第一个点的纵坐标
     * @param x2 第二个点的横坐标
     * @param y2 第二个点的纵坐标
     */
    protected void drawLine(FCPaint paint, long dwPenColor, int width, int style, float x1, float y1, float x2, float y2) {
        x1 += getPx();
        y1 += getPy();
        x2 += getPx();
        y2 += getPy();
        paint.drawLine(dwPenColor, width, style, (int) x1, (int) y1, (int) x2, (int) y2);
        if (paint.supportTransparent()) {
            FCChart chart = getChart();
            FCPoint mp = chart.getTouchPoint();
            if (!m_isPaintingGhost && (mp.y >= m_div.getTop() && mp.y <= m_div.getBottom()) && (chart.getMovingPlot() == this || (chart == getNative().getHoveredControl() && !chart.isOperating() && onSelect()))) {
                int a = 0, r = 0, g = 0, b = 0;
                RefObject<Integer> tempRef_a = new RefObject<Integer>(a);
                RefObject<Integer> tempRef_r = new RefObject<Integer>(r);
                RefObject<Integer> tempRef_g = new RefObject<Integer>(g);
                RefObject<Integer> tempRef_b = new RefObject<Integer>(b);
                FCColor.toArgb(paint, dwPenColor, tempRef_a, tempRef_r, tempRef_g, tempRef_b);
                a = tempRef_a.argvalue;
                r = tempRef_r.argvalue;
                g = tempRef_g.argvalue;
                b = tempRef_b.argvalue;
                if (a == 255) {
                    a = 50;
                }
                dwPenColor = FCColor.argb(a, r, g, b);
                width += 10;
                paint.drawLine(dwPenColor, width, 0, (int) x1, (int) y1, (int) x2, (int) y2);
            }
        }
    }

    /**
     * 画射线
     *
     * @param paint 绘图对象
     * @param dwPenColor 颜色
     * @param width 宽度
     * @param style 样式
     * @param x1 第一个点的横坐标
     * @param y1 第一个点的纵坐标
     * @param x2 第二个点的横坐标
     * @param y2 第二个点的纵坐标
     * @param k 直线参数k
     * @param b 直线参数b
     */
    protected void drawRay(FCPaint paint, long dwPenColor, int width, int style, float x1, float y1, float x2, float y2, float k, float b) {
        // 非垂直时
        if (k != 0 || b != 0) {
            float leftX = 0;
            float leftY = leftX * k + b;
            float rightX = getWorkingAreaWidth();
            float rightY = rightX * k + b;
            if (x1 >= x2) {
                drawLine(paint, dwPenColor, width, style, leftX, leftY, x1, y1);
            } else {
                drawLine(paint, dwPenColor, width, style, x1, y1, rightX, rightY);
            }
        }
        // 垂直时
        else {
            if (y1 >= y2) {
                drawLine(paint, dwPenColor, width, style, x1, y1, x1, 0);
            } else {
                drawLine(paint, dwPenColor, width, style, x1, y1, x1, getWorkingAreaHeight());
            }
        }
    }

    /**
     * 画矩形
     *
     * @param paint 绘图对象
     * @param dwPenColor 颜色
     * @param width 宽度
     * @param style 样式
     * @param left 横坐标
     * @param top 纵坐标
     * @param right 右侧横坐标
     * @param bottom 右侧纵坐标
     */
    protected void drawRect(FCPaint paint, long dwPenColor, int width, int style, int left, int top, int right, int bottom) {
        left += getPx();
        top += getPy();
        right += getPx();
        bottom += getPy();
        FCRect rect = new FCRect(left, top, right, bottom);
        if (paint.supportTransparent()) {
            paint.drawRect(dwPenColor, width, style, rect);
            FCChart chart = getChart();
            FCPoint mp = chart.getTouchPoint();
            if (!m_isPaintingGhost && (mp.y >= m_div.getTop() && mp.y <= m_div.getBottom()) && (chart.getMovingPlot() == this || (chart == getNative().getHoveredControl() && !chart.isOperating() && onSelect()))) {
                int a = 0, r = 0, g = 0, b = 0;
                RefObject<Integer> tempRef_a = new RefObject<Integer>(a);
                RefObject<Integer> tempRef_r = new RefObject<Integer>(r);
                RefObject<Integer> tempRef_g = new RefObject<Integer>(g);
                RefObject<Integer> tempRef_b = new RefObject<Integer>(b);
                FCColor.toArgb(paint, dwPenColor, tempRef_a, tempRef_r, tempRef_g, tempRef_b);
                a = tempRef_a.argvalue;
                r = tempRef_r.argvalue;
                g = tempRef_g.argvalue;
                b = tempRef_b.argvalue;
                if (a == 255) {
                    a = 50;
                }
                dwPenColor = FCColor.argb(a, r, g, b);
                width += 10;
                paint.drawRect(dwPenColor, width, 0, rect);
            }
        }
    }

    /**
     * 画矩形
     *
     * @param paint 绘图对象
     * @param dwPenColor 颜色
     * @param width 宽度
     * @param style 样式
     * @param left 横坐标
     * @param top 纵坐标
     * @param right 右侧横坐标
     * @param bottom 右侧纵坐标
     */
    protected void drawRect(FCPaint paint, long dwPenColor, int width, int style, float left, float top, float right, float bottom) {
        left += getPx();
        top += getPy();
        right += getPx();
        bottom += getPy();
        FCRect rect = new FCRect(left, top, right, bottom);
        paint.drawRect(dwPenColor, width, style, rect);
        if (paint.supportTransparent()) {
            FCChart chart = getChart();
            FCPoint mp = chart.getTouchPoint();
            if (!m_isPaintingGhost && (mp.y >= m_div.getTop() && mp.y <= m_div.getBottom()) && (chart.getMovingPlot() == this || (chart == getNative().getHoveredControl() && !chart.isOperating() && onSelect()))) {
                int a = 0, r = 0, g = 0, b = 0;
                RefObject<Integer> tempRef_a = new RefObject<Integer>(a);
                RefObject<Integer> tempRef_r = new RefObject<Integer>(r);
                RefObject<Integer> tempRef_g = new RefObject<Integer>(g);
                RefObject<Integer> tempRef_b = new RefObject<Integer>(b);
                FCColor.toArgb(paint, dwPenColor, tempRef_a, tempRef_r, tempRef_g, tempRef_b);
                a = tempRef_a.argvalue;
                r = tempRef_r.argvalue;
                g = tempRef_g.argvalue;
                b = tempRef_b.argvalue;
                if (a == 255) {
                    a = 50;
                }
                dwPenColor = FCColor.argb(a, r, g, b);
                width += 10;
                paint.drawRect(dwPenColor, width, 0, rect);
            }
        }
    }

    /**
     * 画曲线
     *
     * @param paint 绘图对象
     * @param dwPenColor 颜色
     * @param width 宽度
     * @param style 样式
     * @param points 点的集合
     */
    protected void drawPolyline(FCPaint paint, long dwPenColor, int width, int style, FCPoint[] points) {
        int px = getPx();
        int py = getPy();
        for (int i = 0; i < points.length; i++) {
            FCPoint point = new FCPoint(px + points[i].x, py + points[i].y);
            points[i] = point;
        }
        paint.drawPolyline(dwPenColor, width, style, points);
        if (paint.supportTransparent()) {
            FCChart chart = getChart();
            FCPoint mp = chart.getTouchPoint();
            if (!m_isPaintingGhost && (mp.y >= m_div.getTop() && mp.y <= m_div.getBottom()) && (chart.getMovingPlot() == this || (chart == getNative().getHoveredControl() && !chart.isOperating() && onSelect()))) {
                int a = 0, r = 0, g = 0, b = 0;
                RefObject<Integer> tempRef_a = new RefObject<Integer>(a);
                RefObject<Integer> tempRef_r = new RefObject<Integer>(r);
                RefObject<Integer> tempRef_g = new RefObject<Integer>(g);
                RefObject<Integer> tempRef_b = new RefObject<Integer>(b);
                FCColor.toArgb(paint, dwPenColor, tempRef_a, tempRef_r, tempRef_g, tempRef_b);
                a = tempRef_a.argvalue;
                r = tempRef_r.argvalue;
                g = tempRef_g.argvalue;
                b = tempRef_b.argvalue;
                if (a == 255) {
                    a = 50;
                }
                dwPenColor = FCColor.argb(a, r, g, b);
                width += 10;
                paint.drawPolyline(dwPenColor, width, 0, points);
            }
        }
    }

    /**
     * 绘制选中点
     *
     * @param paint 绘图对象
     * @param dwPenColor 颜色
     * @param x 横坐标
     * @param y 纵坐标
     */
    protected void drawSelect(FCPaint paint, long dwPenColor, float x, float y) {
        x += getPx();
        y += getPy();
        int sub = m_lineWidth * 3;
        FCRect rect = new FCRect(x - sub, y - sub, x + sub, y + sub);
        if (getSelectedPoint() == SelectedPoint.Ellipse) {
            paint.fillEllipse(dwPenColor, rect);
        } else if (getSelectedPoint() == SelectedPoint.Rectangle) {
            paint.fillRect(dwPenColor, rect);
        }
    }

    /**
     * 画文字
     *
     * @param paint 绘图对象
     * @param text 文字
     * @param dwPenColor 颜色
     * @param font 字体
     * @param x 横坐标
     * @param y 纵坐标
     */
    protected void drawText(FCPaint paint, String text, long dwPenColor, FCFont font, int x, int y) {
        x += getPx();
        y += getPy();
        FCSize tSize = paint.textSize(text, font);
        FCRect tRect = new FCRect(x, y, x + tSize.cx, y + tSize.cy);
        paint.drawText(text, dwPenColor, font, tRect);
    }

    /**
     * 画文字
     *
     * @param paint 绘图对象
     * @param text 文字
     * @param dwPenColor 颜色
     * @param font 字体
     * @param x 横坐标
     * @param y 纵坐标
     */
    protected void drawText(FCPaint paint, String text, long dwPenColor, FCFont font, float x, float y) {
        x += getPx();
        y += getPy();
        FCSize tSize = paint.textSize(text, font);
        FCRect tRect = new FCRect(x, y, x + tSize.cx, y + tSize.cy);
        paint.drawText(text, dwPenColor, font, tRect);
    }

    public static void ellipseAB(float width, float height, RefObject<Float> a, RefObject<Float> b) {
        a.argvalue = width / 2;
        b.argvalue = height / 2;
    }

    public static boolean ellipseHasPoint(float x, float y, float oX, float oY, float a, float b) {
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

    public static void ellipseOR(float x1, float y1, float x2, float y2, float x3, float y3, RefObject<Float> oX, RefObject<Float> oY, RefObject<Float> r) {
        oX.argvalue = ((y3 - y1) * (y2 * y2 - y1 * y1 + x2 * x2 - x1 * x1) + (y2 - y1) * (y1 * y1 - y3 * y3 + x1 * x1 - x3 * x3))
                / (2 * (x2 - x1) * (y3 - y1) - 2 * (x3 - x1) * (y2 - y1));
        oY.argvalue = ((x3 - x1) * (x2 * x2 - x1 * x1 + y2 * y2 - y1 * y1) + (x2 - x1) * (x1 * x1 - x3 * x3 + y1 * y1 - y3 * y3))
                / (2 * (y2 - y1) * (x3 - x1) - 2 * (y3 - y1) * (x2 - x1));
        r.argvalue = (float) Math.sqrt((x1 - oX.argvalue) * (x1 - oX.argvalue) + (y1 - oY.argvalue) * (y1 - oY.argvalue));
    }

    /**
     * 填充多边形
     *
     * @param paint 绘图对象
     * @param color 颜色
     * @param points 点的集合
     */
    protected void fillPolygon(FCPaint paint, long dwPenColor, FCPoint[] points) {
        int px = getPx();
        int py = getPy();
        for (int i = 0; i < points.length; i++) {
            FCPoint point = new FCPoint(px + points[i].x, py + points[i].y);
            points[i] = point;
        }
        paint.fillPolygon(dwPenColor, points);
    }

    /**
     * 获取K线区域内最高价和最低价
     *
     * @param pList 点阵集合
     * @returns 幅度尺的参数
     */
    protected double[] getCandleRange(java.util.HashMap<Integer, PlotMark> pList) {
        if (pList.isEmpty() || m_sourceFields.isEmpty()) {
            return null;
        }
        if (!m_sourceFields.containsKey("HIGH") || !m_sourceFields.containsKey("LOW")) {
            return null;
        }
        int bRecord = m_dataSource.getRowIndex(pList.get(0).getKey());
        int eRecord = m_dataSource.getRowIndex(pList.get(1).getKey());
        double[] highlist = null;
        double[] lowlist = null;
        if (eRecord >= bRecord) {
            highlist = m_dataSource.DATA_ARRAY(m_sourceFields.get("HIGH"), eRecord, eRecord - bRecord + 1);
            lowlist = m_dataSource.DATA_ARRAY(m_sourceFields.get("LOW"), eRecord, eRecord - bRecord + 1);
        } else {
            highlist = m_dataSource.DATA_ARRAY(m_sourceFields.get("HIGH"), bRecord, bRecord - eRecord + 1);
            lowlist = m_dataSource.DATA_ARRAY(m_sourceFields.get("LOW"), bRecord, bRecord - eRecord + 1);
        }
        double nHigh = 0, nLow = 0;
        nHigh = FCScript.maxValue(highlist, highlist.length);
        nLow = FCScript.minValue(lowlist, lowlist.length);
        return new double[]{nHigh, nLow};
    }

    /**
     * 获取动作
     *
     * @returns 动作类型
     */
    public ActionType getAction() {
        return ActionType.NO;
    }

    protected FCPoint getTouchOverPoint() {
        FCChart chart = getChart();
        FCPoint mp = chart.getTouchPoint();
        mp.x -= chart.getLeftVScaleWidth();
        mp.y = mp.y - m_div.getTop() - m_div.getTitleBar().getHeight();
        return mp;
    }

    /**
     * 根据坐标获取索引
     *
     * @param mp 坐标
     * @returns 索引
     */
    protected int getIndex(FCPoint mp) {
        FCChart chart = getChart();
        mp.x += chart.getLeftVScaleWidth();
        mp.y += m_div.getTop() + m_div.getTitleBar().getHeight();
        return chart.getIndex(mp);
    }

    /**
     * 获取直线的参数
     *
     * @param markA 第一个点
     * @param markB 第二个点
     * @returns 直线的参数
     */
    protected float[] getLineParams(PlotMark markA, PlotMark markB) {
        float y1 = pY(markA.getValue());
        float y2 = pY(markB.getValue());
        int bIndex = m_dataSource.getRowIndex(markA.getKey());
        int eIndex = m_dataSource.getRowIndex(markB.getKey());
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float a = 0, b = 0;
        if (x2 - x1 != 0) {
            RefObject<Float> tempRef_a = new RefObject<Float>(a);
            RefObject<Float> tempRef_b = new RefObject<Float>(b);
            lineXY(x1, y1, x2, y2, 0, 0, tempRef_a, tempRef_b);
            a = tempRef_a.argvalue;
            b = tempRef_b.argvalue;
            return new float[]{a, b};
        } else {
            return null;
        }
    }

    /**
     * 获取线性回归带的高低点偏离值
     *
     * @param pList 点阵集合
     * @param param 直线参数
     * @returns 高低点偏离值
     */
    protected double[] getLRBandRange(java.util.HashMap<Integer, PlotMark> pList, float[] param) {
        if (m_sourceFields != null && m_sourceFields.size() > 0 && m_sourceFields.containsKey("HIGH") && m_sourceFields.containsKey("LOW")) {
            float a = param[0];
            float b = param[1];
            ArrayList<Double> upList = new ArrayList<Double>();
            ArrayList<Double> downList = new ArrayList<Double>();
            int bIndex = m_dataSource.getRowIndex(pList.get(0).getKey());
            int eIndex = m_dataSource.getRowIndex(pList.get(1).getKey());
            for (int i = bIndex; i <= eIndex; i++) {
                double high = m_dataSource.get2(i, m_sourceFields.get("HIGH"));
                double low = m_dataSource.get2(i, m_sourceFields.get("LOW"));
                if (!Double.isNaN(high) && !Double.isNaN(low)) {
                    double midValue = (i - bIndex + 1) * a + b;
                    upList.add(high - midValue);
                    downList.add(midValue - low);
                }
            }
            int upListSize = upList.size();
            double upListArray[] = new double[upListSize];
            for (int i = 0; i < upListSize; i++) {
                upListArray[i] = upList.get(i);
            }
            double upSubValue = FCScript.maxValue(upListArray, upListSize);
            int downListSize = downList.size();
            double downListArray[] = new double[downListSize];
            for (int i = 0; i < downListSize; i++) {
                downListArray[i] = downList.get(i);
            }
            double downSubValue = FCScript.maxValue(downListArray, downList.size());
            return new double[]{upSubValue, downSubValue};
        }
        return null;
    }

    /**
     * 获取线性回归的参数
     *
     * @param marks 点阵集合
     * @returns 点阵数组
     */
    protected float[] getLRParams(java.util.HashMap<Integer, PlotMark> marks) {
        if (m_sourceFields != null && m_sourceFields.containsKey("CLOSE")) {
            int bIndex = m_dataSource.getRowIndex(marks.get(0).getKey());
            int eIndex = m_dataSource.getRowIndex(marks.get(1).getKey());
            if (bIndex != -1 && eIndex != -1) {
                ArrayList<Double> closeVList = new ArrayList<Double>();
                for (int i = bIndex; i <= eIndex; i++) {
                    double value = m_dataSource.get2(i, m_sourceFields.get("CLOSE"));
                    if (!Double.isNaN(value)) {
                        closeVList.add(value);
                    }
                }
                if (closeVList.size() > 0) {
                    float a = 0;
                    float b = 0;
                    RefObject<Float> tempRef_a = new RefObject<Float>(a);
                    RefObject<Float> tempRef_b = new RefObject<Float>(b);
                    int closeVListSize = closeVList.size();
                    double list[] = new double[closeVListSize];
                    for (int i = 0; i < closeVListSize; i++) {
                        list[i] = closeVList.get(i);
                    }
                    FCScript.linearRegressionEquation(list, closeVListSize, tempRef_a, tempRef_b);
                    a = tempRef_a.argvalue;
                    b = tempRef_b.argvalue;
                    return new float[]{a, b};
                }
            }
        }
        return null;
    }

    /**
     * 获取移动坐标
     */
    protected FCPoint getMovingPoint() {
        FCPoint mp = getTouchOverPoint();
        if (mp.x < 0) {
            mp.x = 0;
        } else if (mp.x > getWorkingAreaWidth()) {
            mp.x = getWorkingAreaWidth();
        }
        if (mp.y < 0) {
            mp.y = 0;
        } else if (mp.y > getWorkingAreaHeight()) {
            mp.y = getWorkingAreaHeight();
        }
        return mp;
    }

    /**
     * 根据坐标获取数值
     *
     * @param mp 坐标
     * @returns 数值
     */
    protected double getNumberValue(FCPoint mp) {
        FCChart chart = getChart();
        mp.x += chart.getLeftVScaleWidth();
        mp.y += m_div.getTop() + m_div.getTitleBar().getHeight();
        return chart.getNumberValue(m_div, mp, m_attachVScale);
    }

    /**
     * 获取偏移横坐标
     *
     * @returns 偏移横坐标
     */
    protected int getPx() {
        FCChart chart = getChart();
        return chart.getLeftVScaleWidth();
    }

    /**
     * 获取偏移纵坐标
     *
     * @returns 偏移纵坐标
     */
    protected int getPy() {
        return m_div.getTitleBar().getHeight();
    }

    /**
     * 根据两点获取矩形
     *
     * @param markA 第一个点
     * @param markB 第二个点
     * @returns 矩形对象
     */
    protected FCRect getRectangle(PlotMark markA, PlotMark markB) {
        double aValue = markA.getValue();
        double bValue = markB.getValue();
        int aIndex = m_dataSource.getRowIndex(markA.getKey());
        int bIndex = m_dataSource.getRowIndex(markB.getKey());
        float x = pX(aIndex);
        float y = pY(aValue);
        float xS = pX(bIndex);
        float yS = pY(bValue);
        float width = Math.abs(xS - x);
        if (width < 4) {
            width = 4;
        }
        float height = Math.abs(yS - y);
        if (height < 4) {
            height = 4;
        }
        float rX = x <= xS ? x : xS;
        float rY = y <= yS ? y : yS;
        return new FCRect(rX, rY, rX + width, rY + height);
    }

    /**
     * 获取黄金分割线的直线参数
     *
     * @param value1 值1
     * @param value2 值2
     * @returns 黄金分割线的直线参数
     */
    protected float[] goldenRatioParams(double value1, double value2) {
        float y1 = pY(value1);
        float y2 = pY(value2);
        float y0 = 0, yA = 0, yB = 0, yC = 0, yD = 0, y100 = 0;
        y0 = y1;
        yA = y1 <= y2 ? y1 + (y2 - y1) * 0.236f : y2 + (y1 - y2) * (1 - 0.236f);
        yB = y1 <= y2 ? y1 + (y2 - y1) * 0.382f : y2 + (y1 - y2) * (1 - 0.382f);
        yC = y1 <= y2 ? y1 + (y2 - y1) * 0.5f : y2 + (y1 - y2) * (1 - 0.5f);
        yD = y1 <= y2 ? y1 + (y2 - y1) * 0.618f : y2 + (y1 - y2) * (1 - 0.618f);
        y100 = y2;
        return new float[]{y0, yA, yB, yC, yD, y100};
    }

    /**
     * 多条横线的选中方法
     *
     * @param param 参数
     * @param length 长度
     * @returns 是否选中
     */
    protected boolean hLinesSelect(float[] param, int length) {
        FCPoint mp = getTouchOverPoint();
        float top = 0;
        float bottom = getWorkingAreaHeight();
        if (mp.y >= top && mp.y <= bottom) {
            for (float p : param) {
                if (mp.y >= p - m_lineWidth * 2.5f && mp.y <= p + m_lineWidth * 2.5f) {
                    return true;
                }
            }
        }
        return false;
    }

    public static double lineSlope(float x1, float y1, float x2, float y2, float oX, float oY) {
        if ((x1 - oX) != (x2 - oX)) {
            return ((y2 - oY) - (y1 - oY)) / ((x2 - oX) - (x1 - oX));
        }
        return 0;
    }

    public static void lineXY(float x1, float y1, float x2, float y2, float oX, float oY, RefObject<Float> k, RefObject<Float> b) {
        k.argvalue = 0.0f;
        b.argvalue = 0.0f;
        if ((x1 - oX) != (x2 - oX)) {
            k.argvalue = ((y2 - oY) - (y1 - oY)) / ((x2 - oX) - (x1 - oX));
            b.argvalue = (y1 - oY) - k.argvalue * (x1 - oX);
        }
    }

    /**
     * 移动线条
     *
     * @param mp 坐标
     */
    protected void move(FCPoint mp) {
        VScale vScale = m_div.getVScale(m_attachVScale);
        // 循环处理
        int startMarkSize = m_startMarks.size();
        for (int i = 0; i < startMarkSize; i++) {
            int newIndex = 0;
            double yAddValue = 0;
            int startIndex = m_dataSource.getRowIndex(m_startMarks.get(i).getKey());
            RefObject<Double> tempRef_yAddValue = new RefObject<Double>(yAddValue);
            RefObject<Integer> tempRef_newIndex = new RefObject<Integer>(newIndex);
            movePlot(mp.y, m_startPoint.y, startIndex, getIndex(m_startPoint), getIndex(mp), getWorkingAreaHeight(), vScale.getVisibleMax(), vScale.getVisibleMin(), m_dataSource.getRowsCount(), tempRef_yAddValue, tempRef_newIndex);
            yAddValue = tempRef_yAddValue.argvalue;
            newIndex = tempRef_newIndex.argvalue;
            if (vScale.isReverse()) {
                m_marks.put(i, new PlotMark(i, m_dataSource.getXValue(newIndex), m_startMarks.get(i).getValue() - yAddValue));
            } else {
                m_marks.put(i, new PlotMark(i, m_dataSource.getXValue(newIndex), m_startMarks.get(i).getValue() + yAddValue));
            }
        }
    }

    /**
     * 移动画线
     *
     * @param touchY 纵坐标
     * @param startY 开始纵坐标
     * @param startIndex 开始索引
     * @param touchBeginIndex 触摸开始索引
     * @param touchEndIndex 触摸结束索引
     * @param pureV 纵向距离
     * @param max 最大值
     * @param min 最小值
     * @param dataCount 数据条数
     * @param yAddValue 纵向变化值
     * @param newIndex 新的索引
     */
    private void movePlot(float touchY, float startY, int startIndex, int touchBeginIndex, int touchEndIndex, float pureV, double max, double min, int dataCount, RefObject<Double> yAddValue, RefObject<Integer> newIndex) {
        float subY = touchY - startY;
        yAddValue.argvalue = ((min - max) * subY / pureV);
        newIndex.argvalue = startIndex + (touchEndIndex - touchBeginIndex);
        if (newIndex.argvalue < 0) {
            newIndex.argvalue = 0;
        } else if (newIndex.argvalue > dataCount - 1) {
            newIndex.argvalue = dataCount - 1;
        }
    }

    /**
     * 初始化线条
     *
     * @param mp 坐标
     * @returns 是否初始化成功
     */
    public boolean onCreate(FCPoint mp) {
        return false;
    }

    /**
     * 移动结束
     */
    public void onMoveEnd() {
    }

    /**
     * 移动开始
     */
    public void onMoveStart() {
    }

    /**
     * 移动
     */
    public void onMoving() {
        FCPoint mp = getMovingPoint();
        // 根据不同类型作出动作
        switch (m_action) {
            case MOVE:
                move(mp);
                break;
            case AT1:
                resize(0);
                break;
            case AT2:
                resize(1);
                break;
            case AT3:
                resize(2);
                break;
            case AT4:
                resize(3);
                break;
        }
    }

    /**
     * 绘图对象
     *
     * @param paint 绘图对象
     */
    public void onPaint(FCPaint paint) {
        FCChart chart = getChart();
        FCPoint mp = chart.getTouchPoint();
        if ((mp.y >= m_div.getTop() && mp.y <= m_div.getBottom()) && (chart.getMovingPlot() == this || (chart == getNative().getHoveredControl() && !chart.isOperating() && onSelect()))) {
            paint(paint, m_marks, m_selectedColor);
        } else {
            paint(paint, m_marks, m_color);
        }
    }

    /**
     * 绘制图像的残影
     *
     * @param paint 绘图对象
     */
    public void onPaintGhost(FCPaint paint) {
        m_isPaintingGhost = true;
        paint(paint, m_startMarks, m_color);
        m_isPaintingGhost = false;
    }

    /**
     * 判断选中
     *
     * @returns 是否选中
     */
    public boolean onSelect() {
        ActionType action = getAction();
        if (action != ActionType.NO) {
            return true;
        } else {
            return false;
        }
    }

    public static void parallelogram(float x1, float y1, float x2, float y2, float x3, float y3, RefObject<Float> x4, RefObject<Float> y4) {
        x4.argvalue = x1 + x3 - x2;
        y4.argvalue = y1 + y3 - y2;
    }

    /**
     * 绘制图像的方法
     *
     * @param paint 绘图对象
     * @param pList 横纵值描述
     * @param lineColor 颜色
     */
    protected void paint(FCPaint paint, java.util.HashMap<Integer, PlotMark> pList, long lineColor) {

    }

    /**
     * 获取绘制横坐标
     *
     * @param index 索引
     * @returns 横坐标
     */
    protected float pX(int index) {
        FCChart chart = getChart();
        float x = chart.getX(index);
        return x - chart.getLeftVScaleWidth();
    }

    /**
     * 获取绘制纵坐标
     *
     * @param value 值
     * @returns 纵坐标
     */
    protected float pY(double value) {
        FCChart chart = getChart();
        float y = chart.getY(m_div, value, m_attachVScale);
        return y - m_div.getTitleBar().getHeight();
    }

    /**
     * 根据横坐标获取画线工具内部横坐标
     *
     * @param x 横坐标
     * @returns 内部横坐标
     */
    protected float pX(float x) {
        FCChart chart = getChart();
        return x - chart.getLeftVScaleWidth();
    }

    /**
     * 根据横坐标获取画线工具内部横坐标
     *
     * @param y 纵坐标
     * @returns 内部纵坐标
     */
    protected float pY(float y) {
        return y - m_div.getTop() - m_div.getTitleBar().getHeight();
    }

    public static void rectangleXYWH(int x1, int y1, int x2, int y2, RefObject<Integer> x, RefObject<Integer> y, RefObject<Integer> w, RefObject<Integer> h) {
        x.argvalue = x1 < x2 ? x1 : x2;
        y.argvalue = y1 < y2 ? y1 : y2;
        w.argvalue = Math.abs(x1 - x2);
        h.argvalue = Math.abs(y1 - y2);
        if (w.argvalue <= 0) {
            w.argvalue = 4;
        }
        if (h.argvalue <= 0) {
            h.argvalue = 4;
        }
    }

    /**
     * 绘制到图像上
     *
     * @param paint 绘图对象
     */
    public void render(FCPaint paint) {
        FCChart chart = getChart();
        if (m_drawGhost && chart.getMovingPlot() != null && chart.getMovingPlot() == this) {
            onPaintGhost(paint);
        }
        onPaint(paint);
    }

    /**
     * 调整大小
     *
     * @param index 索引
     */
    protected void resize(int index) {
        FCChart chart = getChart();
        int touchIndex = chart.getTouchOverIndex();
        double y = getNumberValue(getMovingPoint());
        if (touchIndex < 0) {
            touchIndex = 0;
        }
        if (touchIndex > chart.getLastVisibleIndex()) {
            touchIndex = chart.getLastVisibleIndex();
        }
        m_marks.put(index, new PlotMark(index, m_dataSource.getXValue(touchIndex), y));
    }

    /**
     * 重新调整大小
     *
     * @param mp 坐标
     * @param oppositePoint 点位置
     */
    protected void resize(FCPoint mp, FCPoint oppositePoint) {
        double sValue = getNumberValue(new FCPoint(oppositePoint.x, oppositePoint.y));
        double eValue = getNumberValue(mp);
        int iS = getIndex(new FCPoint(oppositePoint.x, oppositePoint.y));
        int iE = getIndex(mp);
        double topValue = 0;
        double bottomValue = 0;
        VScale vScale = m_div.getVScale(m_attachVScale);
        if (sValue >= eValue) {
            if (vScale.isReverse()) {
                topValue = eValue;
                bottomValue = sValue;
            } else {
                topValue = sValue;
                bottomValue = eValue;
            }
        } else {
            if (vScale.isReverse()) {
                topValue = sValue;
                bottomValue = eValue;
            } else {
                topValue = eValue;
                bottomValue = sValue;
            }
        }
        double sDate = 0;
        double eDate = 0;
        if (iS < 0) {
            iS = 0;
        } else if (iS > m_dataSource.getRowsCount() - 1) {
            iS = m_dataSource.getRowsCount() - 1;
        }
        if (iE < 0) {
            iE = 0;
        } else if (iE > m_dataSource.getRowsCount() - 1) {
            iE = m_dataSource.getRowsCount() - 1;
        }
        if (iS >= iE) {
            sDate = m_dataSource.getXValue(iE);
            eDate = m_dataSource.getXValue(iS);
        } else {
            sDate = m_dataSource.getXValue(iS);
            eDate = m_dataSource.getXValue(iE);
        }
        // 横轴反转
        m_marks.put(0, new PlotMark(0, sDate, topValue));
        m_marks.put(1, new PlotMark(1, eDate, bottomValue));
    }

    /**
     * 判断是否选中了指定的点
     *
     * @param mp 坐标
     * @param x 点的横坐标
     * @param y 点的纵坐标
     * @returns 是否选中
     */
    protected boolean selectPoint(FCPoint mp, float x, float y) {
        if (mp.x >= x - m_lineWidth * 6 && mp.x <= x + m_lineWidth * 6 && mp.y >= y - m_lineWidth * 6 && mp.y <= y + m_lineWidth * 6) {
            return true;
        }
        return false;
    }

    /**
     * 判断是否选中线
     *
     * @param mp 点的位置
     * @param x 横坐标
     * @param k 直线参数k
     * @param b 直线参数b
     * @returns 是否选中线
     */
    protected boolean selectLine(FCPoint mp, float x, float k, float b) {
        if (!(k == 0 && b == 0)) {
            if (mp.y / (mp.x * k + b) >= 0.95 && mp.y / (mp.x * k + b) <= 1.05) {
                return true;
            }
        } else {
            if (mp.x >= x - m_lineWidth * 5 && mp.x <= x + m_lineWidth * 5) {
                return true;
            }
        }
        return false;
    }

    /**
     * 判断是否选中线
     *
     * @param mp 点的位置
     * @param x1 第一个点的横坐标
     * @param y1 第一个点的纵坐标
     * @param x2 第二个点的横坐标
     * @param y2 第二个点的纵坐标
     */
    protected boolean selectLine(FCPoint mp, float x1, float y1, float x2, float y2) {
        float k = 0, b = 0;
        RefObject<Float> tempRef_k = new RefObject<Float>(k);
        RefObject<Float> tempRef_b = new RefObject<Float>(b);
        lineXY(x1, y1, x2, y2, 0, 0, tempRef_k, tempRef_b);
        k = tempRef_k.argvalue;
        b = tempRef_b.argvalue;
        return selectLine(mp, x1, k, b);
    }

    /**
     * 判断是否选中射线
     *
     * @param mp 点的位置
     * @param x1 第一个点的横坐标
     * @param y1 第一个点的纵坐标
     * @param x2 第二个点的横坐标
     * @param y2 第二个点的纵坐标
     * @param k 直线参数k
     * @param b 直线参数b
     * @returns 是否选中射线
     */
    protected boolean selectRay(FCPoint mp, float x1, float y1, float x2, float y2, RefObject<Float> k, RefObject<Float> b) {
        // 获取直线参数
        lineXY(x1, y1, x2, y2, 0, 0, k, b);
        if (!(k.argvalue == 0 && b.argvalue == 0)) {
            if (mp.y / (mp.x * k.argvalue + b.argvalue) >= 0.95 && mp.y / (mp.x * k.argvalue + b.argvalue) <= 1.05) {
                if (x1 >= x2) {
                    if (mp.x > x1 + 5) {
                        return false;
                    }
                } else if (x1 < x2) {
                    if (mp.x < x1 - 5) {
                        return false;
                    }
                }
                return true;
            }
        } else {
            if (mp.x >= x1 - m_lineWidth * 5 && mp.x <= x1 + m_lineWidth * 5) {
                if (y1 >= y2) {
                    if (mp.y <= y1 - 5) {
                        return true;
                    }
                } else {
                    if (mp.y >= y1 - 5) {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    /**
     * 判断是否选中射线
     *
     * @param mp 点的位置
     * @param x1 第一个点的横坐标
     * @param y1 第一个点的纵坐标
     * @param x2 第二个点的横坐标
     * @param y2 第二个点的纵坐标
     * @returns 是否选中射线
     */
    protected boolean selectRay(FCPoint mp, float x1, float y1, float x2, float y2) {
        float k = 0, b = 0;
        RefObject<Float> tempRef_k = new RefObject<Float>(k);
        RefObject<Float> tempRef_b = new RefObject<Float>(b);
        boolean tempVar = selectRay(mp, x1, y1, x2, y2, tempRef_k, tempRef_b);
        k = tempRef_k.argvalue;
        b = tempRef_b.argvalue;
        return tempVar;
    }

    /**
     * 获取选中状态
     *
     * @param mp 点的位置
     * @param markA 点A
     * @param markB 点B
     * @returns 选中状态
     */
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
        // 判断是否选中点
        if (selectPoint(mp, x1, y1)) {
            return ActionType.AT1;
        } else if (selectPoint(mp, x2, y2)) {
            return ActionType.AT2;
        } else if (selectPoint(mp, x3, y3)) {
            return ActionType.AT3;
        } else if (selectPoint(mp, x4, y4)) {
            return ActionType.AT4;
        } else {
            int sub = (int) (m_lineWidth * 2.5);
            FCRect bigRect = new FCRect(rect.left - sub, rect.top - sub, rect.right + sub, rect.bottom + sub);
            if (mp.x >= bigRect.left && mp.x <= bigRect.right && mp.y >= bigRect.top && mp.y <= bigRect.bottom) {
                if (rect.right - rect.left <= 4 || rect.bottom - rect.top <= 4) {
                    return ActionType.MOVE;
                } else {
                    FCRect smallRect = new FCRect(rect.left + sub, rect.top + sub, rect.right - sub, rect.bottom - sub);
                    if (!(mp.x >= smallRect.left && mp.x <= smallRect.right && mp.y >= smallRect.top && mp.y <= smallRect.bottom)) {
                        return ActionType.MOVE;
                    }
                }
            }
        }
        return ActionType.NO;
    }

    /**
     * 判断是否选中线段
     *
     * @param mp 点的位置
     * @param x1 第一个点的横坐标
     * @param y1 第一个点的纵坐标
     * @param x2 第二个点的横坐标
     * @param y2 第二个点的纵坐标
     * @returns 是否选中线段
     */
    protected boolean selectSegment(FCPoint mp, float x1, float y1, float x2, float y2) {
        float k = 0;
        float b = 0;
        RefObject<Float> tempRef_k = new RefObject<Float>(k);
        RefObject<Float> tempRef_b = new RefObject<Float>(b);
        // 获取直线参数
        lineXY(x1, y1, x2, y2, 0, 0, tempRef_k, tempRef_b);
        k = tempRef_k.argvalue;
        b = tempRef_b.argvalue;
        float smallX = x1 <= x2 ? x1 : x2;
        float smallY = y1 <= y2 ? y1 : y2;
        float bigX = x1 > x2 ? x1 : x2;
        float bigY = y1 > y2 ? y1 : y2;
        if (mp.x >= smallX - 2 && mp.x <= bigX + 2 && mp.y >= smallY - 2 && mp.y <= bigY + 2) {
            if (!(k == 0 && b == 0)) {
                if (mp.y / (mp.x * k + b) >= 0.95 && mp.y / (mp.x * k + b) <= 1.05) {
                    return true;
                }
            } else {
                if (mp.x >= x1 - m_lineWidth * 5 && mp.x <= x1 + m_lineWidth * 5) {
                    return true;
                }
            }
        }
        return false;
    }

    /**
     * 判断是否选中正弦线
     *
     * @param mp 点的位置
     * @param x1 第一个点的横坐标
     * @param y1 第一个点的纵坐标
     * @param x2 第二个点的横坐标
     * @param y2 第二个点的纵坐标
     * @returns 是否选中正弦线
     */
    protected boolean selectSine(FCPoint mp, float x1, float y1, float x2, float y2) {
        double f = 2.0 * Math.PI / ((x2 - x1) * 4);
        int len = getWorkingAreaWidth();
        if (len > 0) {
            float lastX = 0, lastY = 0;
            for (int i = 0; i < len; i++) {
                float x = -x1 + i;
                float y = (float) (0.5 * (y2 - y1) * Math.sin(x * f) * 2);
                float px = x + x1, py = y + y1;
                if (i == 0) {
                    if (selectPoint(mp, px, py)) {
                        return true;
                    }
                } else {
                    float rectLeft = lastX - 2;
                    float rectTop = lastY <= py ? lastY : py - 2;
                    float rectRight = rectLeft + Math.abs(px - lastX) + 4;
                    float rectBottom = rectTop + Math.abs(py - lastY) + 4;
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

    /**
     * 判断是否选中三角形
     *
     * @param mp 点的位置
     * @param x1 第一个点的横坐标
     * @param y1 第一个点的纵坐标
     * @param x2 第二个点的横坐标
     * @param y2 第二个点的纵坐标
     * @param x3 第三个点的横坐标
     * @param y3 第三个点的纵坐标
     * @returns 是否选中三角形
     */
    protected boolean selectTriangle(FCPoint mp, float x1, float y1, float x2, float y2, float x3, float y3) {
        boolean selected = selectSegment(mp, x1, y1, x2, y2);
        if (selected) {
            return true;
        }
        selected = selectSegment(mp, x1, y1, x3, y3);
        if (selected) {
            return true;
        }
        selected = selectSegment(mp, x2, y2, x3, y3);
        if (selected) {
            return true;
        }
        return false;
    }

    /**
     * 获取文字的大小
     *
     * @param paint 绘图对象
     * @param text 文字
     * @param font 字体
     * @returns 大小
     */
    protected FCSize textSize(FCPaint paint, String text, FCFont font) {
        return paint.textSize(text, font);
    }
}
