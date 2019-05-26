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
import java.text.*;
import java.util.*;

/**
 * 股票布局控件
 */
public class FCChart extends FCView {

    /**
     * 创建控件
     */
    public FCChart() {
        m_timerID = getNewTimerID();
    }

    /**
     * 析构函数
     */
    protected void finalize() throws Throwable {
        delete();
    }

    /**
     * 图层
     */
    protected ArrayList<ChartDiv> m_divs = new ArrayList<ChartDiv>();

    /**
     * 十字线定位y坐标
     */
    protected int m_cross_y = -1;

    /**
     * 横向重置大小标识
     */
    protected int m_hResizeType = 0;

    /**
     * 最后一条非空索引
     */
    protected int m_lastUnEmptyIndex = -1;

    /**
     * 当前最后一条记录是否可见
     */
    protected boolean m_lastRecordIsVisible;

    /**
     * 最后一条可见记录的时间
     */
    protected double m_lastVisibleKey;

    /**
     * 触摸是否正在移动
     */
    protected boolean m_isTouchMove = true;

    /**
     * 滚动十字线标识
     */
    protected boolean m_isScrollCross = false;

    /**
     * 上次点击的位置
     */
    protected FCPoint m_lastTouchClickPoint = new FCPoint(-1, -1);

    /**
     * 上次移动的点位
     */
    protected FCPoint m_lastTouchMovePoint = new FCPoint();

    /**
     * 触摸最后一次移动的事件
     */
    protected Calendar m_lastTouchMoveTime = Calendar.getInstance();

    /**
     * 按键滚动的幅度
     */
    protected int m_scrollStep = 1;

    /**
     * 显示选中区域
     */
    protected boolean m_showingSelectArea = false;

    /**
     * 是否正在显示提示框
     */
    protected boolean m_showingToolTip = false;

    /**
     * 秒表编号
     */
    private int m_timerID;

    /**
     * 提示框显示延迟tick值
     */
    protected static int m_tooltip_dely = 2500000;

    /**
     * 获取或设置正在改变大小的图层
     */
    protected ChartDiv m_userResizeDiv = null;

    protected boolean m_autoFillHScale = false;

    /**
     * 获取数据是否
     */
    public boolean getAutoFillHScale() {
        return m_autoFillHScale;
    }

    /**
     * 设置数据是否
     */
    public void setAutoFillHScale(boolean value) {
        m_autoFillHScale = value;
    }

    protected int m_blankSpace;

    /**
     * 获取空白空间
     */
    public int getBlankSpace() {
        return m_blankSpace;
    }

    /**
     * 设置空白空间
     */
    public void setBlankSpace(int value) {
        m_blankSpace = value;
    }

    protected boolean m_canResizeV = true;

    /**
     * 获取是否可纵向改变大小
     */
    public boolean canResizeV() {
        return m_canResizeV;
    }

    /**
     * 设置是否可纵向改变大小
     */
    public void setCanResizeV(boolean value) {
        m_canResizeV = value;
    }

    protected boolean m_canResizeH = true;

    /**
     * 获取是可横向改变大小
     */
    public boolean canResizeH() {
        return m_canResizeH;
    }

    /**
     * 设置是可横向改变大小
     */
    public void setCanResizeH(boolean value) {
        m_canResizeH = value;
    }

    protected boolean m_canMoveShape = true;

    /**
     *获取是否可以拖动线条
     */
    public boolean canmoveShape() {
        return m_canMoveShape;
    }

    /**
     * 设置是否可以拖动线条
     */
    public void setCanMoveShape(boolean value) {
        m_canMoveShape = value;
    }

    protected CrossLineMoveMode m_crossLineMoveMode = CrossLineMoveMode.FollowTouch;

    /**
     * 获取十字线的移动方式
     */
    public CrossLineMoveMode getCrossLineMoveMode() {
        return m_crossLineMoveMode;
    }

    /**
     * 设置十字线的移动方式
     */
    public void setCrossLineMoveMode(CrossLineMoveMode value) {
        m_crossLineMoveMode = value;
    }

    protected boolean m_canScroll = true;

    /**
     * 获取是否可以执行滚动操作
     */
    public boolean getCanScroll() {
        return m_canScroll;
    }

    /**
     * 设置是否可以执行滚动操作
     */
    public void setCanScroll(boolean value) {
        m_canScroll = value;
    }

    protected boolean m_canZoom = true;

    /**
     * 获取是否可以执行缩放操作
     */
    public boolean canZoom() {
        return m_canZoom;
    }

    /**
     * 设置是否可以执行缩放操作
     */
    public void setCanZoom(boolean value) {
        m_canZoom = value;
    }

    protected int m_crossStopIndex;

    /**
     * 获取十字线当前停留的记录索引
     */
    public int getCrossStopIndex() {
        return m_crossStopIndex;
    }

    /**
     * 设置十字线当前停留的记录索引
     */
    public void setCrossStopIndex(int value) {
        m_crossStopIndex = value;
    }

    protected FCDataTable m_dataSource;

    /**
     * 获取数据源
     */
    public FCDataTable getDataSource() {
        return m_dataSource;
    }

    protected int m_firstVisibleIndex = -1;

    /**
     * 获取首先可见记录号
     */
    public int getFirstVisibleIndex() {
        return m_firstVisibleIndex;
    }

    protected String m_hScaleFieldText;

    /**
     * 获取横轴字段的显示文字
     */
    public String getHScaleFieldText() {
        return m_hScaleFieldText;
    }

    /**
     * 设置横轴字段的显示文字
     */
    public void setHScaleFieldText(String value) {
        m_hScaleFieldText = value;
    }

    protected double m_hScalePixel = 7;

    /**
     * 获取每条数据在X轴上所占的空间
     */
    public double getHScalePixel() {
        return m_hScalePixel;
    }

    /**
     * 设置每条数据在X轴上所占的空间
     */
    public void setHScalePixel(double value) {
        m_hScalePixel = value;
        if (m_hScalePixel > 1) {
            m_hScalePixel = (int) m_hScalePixel;
        }
        if (m_hScalePixel > 1 && m_hScalePixel % 2 == 0) {
            m_hScalePixel += 1;
        }
    }

    protected int m_lastVisibleIndex = -1;

    /**
     * 获取最后可见的记录号
     */
    public int getLastVisibleIndex() {
        return m_lastVisibleIndex;
    }

    protected int m_leftVScaleWidth = 80;

    /**
     * 获取左侧Y轴的宽度
     */
    public int getLeftVScaleWidth() {
        return m_leftVScaleWidth;
    }

    /**
     * 设置左侧Y轴的宽度
     */
    public void setLeftVScaleWidth(int value) {
        m_leftVScaleWidth = value;
    }

    protected int m_maxVisibleRecord;

    /**
     * 获取显示最大记录数
     */
    public int getMaxVisibleRecord() {
        return m_maxVisibleRecord;
    }

    protected BaseShape m_movingShape = null;

    /**
     * 获取正在被移动的图形
     */
    public BaseShape getMovingShape() {
        return m_movingShape;
    }

    /**
     * 设置正在被移动的图形
     */
    public void setMovingShape(BaseShape value) {
        m_movingShape = value;
    }

    protected FCPlot m_movingPlot = null;

    /**
     * 获取正在移动的画线工具
     */
    public FCPlot getMovingPlot() {
        return m_movingPlot;
    }

    /**
     * 设置正在移动的画线工具
     */
    public void setMovingPlot(FCPlot value) {
        m_movingPlot = value;
    }

    protected boolean m_reverseHScale = false;

    /**
     * 获取是否反转横轴
     */
    public boolean getReverseHScale() {
        return m_reverseHScale;
    }

    /**
     * 设置是否反转横轴
     */
    public void setReverseHScale(boolean value) {
        m_reverseHScale = value;
    }

    protected int m_rightVScaleWidth = 0;

    /**
     * 获取右侧Y轴的宽度
     */
    public int getRightVScaleWidth() {
        return m_rightVScaleWidth;
    }

    /**
     * 设置右侧Y轴的宽度
     */
    public void setRightVScaleWidth(int value) {
        m_rightVScaleWidth = value;
    }

    protected boolean m_scrollAddSpeed = false;

    /**
     * 获取是否启用加速效果
     */
    public boolean scrollAddSpeed() {
        return m_scrollAddSpeed;
    }

    /**
     * 设置是否启用加速效果
     */
    public void setscrollAddSpeed(boolean value) {
        m_scrollAddSpeed = value;
    }

    /**
     * 获取当前选中的线条
     */
    public BaseShape getSelectedShape() {
        ArrayList<ChartDiv> divsCopy = getDivs();
        for (ChartDiv cDiv : divsCopy) {
            ArrayList<BaseShape> shapesCopy = cDiv.getShapes(SortType.NONE);
            for (BaseShape bs : shapesCopy) {
                if (bs.isSelected()) {
                    return bs;
                }
            }
        }
        return null;
    }

    /**
     * 设置当前选中的线条
     */
    public void setSelectedShape(BaseShape value) {
        ArrayList<ChartDiv> divsCopy = getDivs();
        for (ChartDiv cDiv : divsCopy) {
            ArrayList<BaseShape> shapesCopy = cDiv.getShapes(SortType.ASC);
            for (BaseShape bs : shapesCopy) {
                if (bs == value) {
                    bs.setSelected(true);
                } else {
                    bs.setSelected(false);
                }
            }
        }
    }

    /**
     * 获取当前选中的画线工具
     */
    public FCPlot getSelectedPlot() {
        ArrayList<ChartDiv> divsCopy = getDivs();
        for (ChartDiv div : divsCopy) {
            ArrayList<FCPlot> plotsCopy = div.getPlots(SortType.NONE);
            for (FCPlot plot : plotsCopy) {
                if (plot.isVisible() && plot.isSelected()) {
                    return plot;
                }
            }
        }
        return null;
    }

    /**
     * 设置当前选中的画线工具
     */
    public void setSelectedPlot(FCPlot value) {
        ArrayList<ChartDiv> divsCopy = getDivs();
        for (ChartDiv div : divsCopy) {
            ArrayList<FCPlot> plotsCopy = div.getPlots(SortType.NONE);
            for (FCPlot plot : plotsCopy) {
                if (plot == value) {
                    plot.setSelected(true);
                } else {
                    plot.setSelected(false);
                }
            }
        }
    }

    /**
     * 获取当前选中的层
     */
    public ChartDiv getSelectedDiv() {
        ArrayList<ChartDiv> divsCopy = getDivs();
        for (ChartDiv div : divsCopy) {
            if (div.isSelected()) {
                return div;
            }
        }
        return null;
    }

    /**
     * 设置当前选中的层
     */
    public void setSelectedDiv(ChartDiv value) {
        ArrayList<ChartDiv> divsCopy = getDivs();
        for (ChartDiv div : divsCopy) {
            if (div == value) {
                div.setSelected(true);
            } else {
                div.setSelected(false);
            }
        }
    }

    protected boolean m_showCrossLine = false;

    /**
     * 获取是否显示十字线
     */
    public boolean getShowCrossLine() {
        return m_showCrossLine;
    }

    /**
     * 设置是否显示十字线
     */
    public void setShowCrossLine(boolean value) {
        m_showCrossLine = value;
    }

    protected int m_workingAreaWidth;

    /**
     * 获取层去掉坐标轴宽度后的横向宽度
     *
     * @returns 宽度
     */
    public int getWorkingAreaWidth() {
        return m_workingAreaWidth;
    }

    /**
     * 添加一个新的图层，按照所设置的比例调节纵向高度
     *
     * @param vPercent 纵向高度比例
     * @ @returns 图层
     */
    public ChartDiv addDiv(int vPercent) {
        if (vPercent <= 0) {
            return null;
        }
        // 创建层
        ChartDiv cDiv = new ChartDiv();
        cDiv.setVerticalPercent(vPercent);
        cDiv.setChart(this);
        m_divs.add(cDiv);
        update();
        return cDiv;
    }

    /**
     * 添加一个新的图层，自动调节高度
     *
     * @returns 图层ID
     */
    public ChartDiv addDiv() {
        ArrayList<ChartDiv> divsCopy = getDivs();
        int pNum = divsCopy.size() + 1;
        return addDiv(100 / pNum);
    }

    /**
     * 设置可见部分的最大值和最小值
     */
    public void adjust() {
        if (m_workingAreaWidth > 0) {
            m_lastUnEmptyIndex = -1;
            if (m_firstVisibleIndex < 0 || m_lastVisibleIndex > m_dataSource.getRowsCount() - 1) {
                return;
            }
            ArrayList<ChartDiv> divsCopy = getDivs();
            for (ChartDiv cDiv : divsCopy) {
                cDiv.setWorkingAreaHeight(cDiv.getHeight() - cDiv.getHScale().getHeight() - cDiv.getTitleBar().getHeight() - 1);
                ArrayList<BaseShape> shapesCopy = cDiv.getShapes(SortType.NONE);
                double leftMax = 0, leftMin = 0, rightMax = 0, rightMin = 0;
                boolean leftMaxInit = false, leftMinInit = false, rightMaxInit = false, rightMinInit = false;
                VScale leftVScale = cDiv.getLeftVScale();
                VScale rightVScale = cDiv.getRightVScale();
                if (m_dataSource.getRowsCount() > 0) {
                    for (BaseShape bs : shapesCopy) {
                        if (!bs.isVisible()) {
                            continue;
                        }
                        BarShape bar = (BarShape) ((bs instanceof BarShape) ? bs : null);
                        int[] fields = bs.getFields();
                        if (fields == null) {
                            continue;
                        }
                        for (int f = 0; f < fields.length; f++) {
                            int field = m_dataSource.getColumnIndex(fields[f]);
                            for (int i = m_firstVisibleIndex; i <= m_lastVisibleIndex; i++) {
                                double fieldValue = m_dataSource.get3(i, field);
                                if (!Double.isNaN(fieldValue)) {
                                    m_lastUnEmptyIndex = i;
                                    if (bs.getAttachVScale() == AttachVScale.Left) {
                                        if (fieldValue > leftMax || !leftMaxInit) {
                                            leftMaxInit = true;
                                            leftMax = fieldValue;
                                        }
                                        if (fieldValue < leftMin || !leftMinInit) {
                                            leftMinInit = true;
                                            leftMin = fieldValue;
                                        }
                                    } else {
                                        if (fieldValue > rightMax || !rightMaxInit) {
                                            rightMaxInit = true;
                                            rightMax = fieldValue;
                                        }
                                        if (fieldValue < rightMin || !rightMinInit) {
                                            rightMinInit = true;
                                            rightMin = fieldValue;
                                        }
                                    }
                                }
                            }
                        }
                        if (bar != null) {
                            if (bar.getFieldName2() == FCDataTable.NULLFIELD) {
                                double midValue = 0;
                                if (bs.getAttachVScale() == AttachVScale.Left) {
                                    if (midValue > leftMax || !leftMaxInit) {
                                        leftMaxInit = true;
                                        leftMax = midValue;
                                    }
                                    if (midValue < leftMin || !leftMinInit) {
                                        leftMinInit = true;
                                        leftMin = midValue;
                                    }
                                } else {
                                    if (midValue > rightMax || !rightMaxInit) {
                                        rightMaxInit = true;
                                        rightMax = midValue;
                                    }
                                    if (midValue < rightMin || !rightMinInit) {
                                        rightMinInit = true;
                                        rightMin = midValue;
                                    }
                                }
                            }
                        }
                    }
                    if (leftMax == leftMin) {
                        leftMax = leftMax * 1.01;
                        leftMin = leftMin * 0.99;
                    }
                    if (rightMax == rightMin) {
                        rightMax = rightMax * 1.01;
                        rightMin = rightMin * 0.99;
                    }
                }
                if (leftVScale.autoMaxMin()) {
                    leftVScale.setVisibleMax(leftMax);
                    leftVScale.setVisibleMin(leftMin);
                }
                if (rightVScale.autoMaxMin()) {
                    rightVScale.setVisibleMax(rightMax);
                    rightVScale.setVisibleMin(rightMin);
                }
                if (leftVScale.autoMaxMin() && leftVScale.getVisibleMax() == 0 && leftVScale.getVisibleMin() == 0) {
                    leftVScale.setVisibleMax(rightVScale.getVisibleMax());
                    leftVScale.setVisibleMin(rightVScale.getVisibleMin());
                }
                if (rightVScale.autoMaxMin() && rightVScale.getVisibleMax() == 0 && rightVScale.getVisibleMin() == 0) {
                    rightVScale.setVisibleMax(leftVScale.getVisibleMax());
                    rightVScale.setVisibleMin(leftVScale.getVisibleMin());
                }
            }
        }
    }

    /**
     * 在指定层的指定位置添加画线工具
     *
     * @param bpl 画线工具对象
     * @param mp 出现的位置
     * @param div 画线工具所在层
     */
    public void addPlot(FCPlot bpl, FCPoint mp, ChartDiv div) {
        if (div != null && m_dataSource.getRowsCount() >= 2) {
            // 获取索引
            int rIndex = getIndex(mp);
            if (rIndex < 0 || rIndex > m_lastVisibleIndex) {
                return;
            }
            if (bpl != null) {
                bpl.setDiv(div);
                bpl.setSelected(true);
                ArrayList<Double> zorders = new ArrayList<Double>();
                ArrayList<FCPlot> plots = div.getPlots(SortType.NONE);
                int plotSize = plots.size();
                for (int i = 0; i < plotSize; i++) {
                    zorders.add((double) plots.get(i).getZOrder());
                }
                int zordersSize = zorders.size();
                double zordersArray[] = new double[zordersSize];
                for (int i = 0; i < zordersSize; i++) {
                    zordersArray[i] = zorders.get(i);
                }
                bpl.setZOrder((int) FCScript.maxValue(zordersArray, zordersSize) + 1);
                boolean flag = bpl.onCreate(mp);
                if (flag) {
                    div.addPlot(bpl);
                    setMovingPlot(bpl);
                    getMovingPlot().onMoveStart();
                }
            }
            // 关闭选中区域
            closeSelectArea();
        }
    }

    /**
     * 对图像进行操作
     *
     * @param scrollType ScrollType枚举
     * @param limitStep 滚动幅度
     */
    public void changeChart(ScrollType scrollType, int limitStep) {
        ArrayList<ChartDiv> divsCopy = getDivs();
        if (divsCopy.isEmpty() || m_dataSource.getRowsCount() == 0) {
            return;
        }
        int fIndex = m_firstVisibleIndex;
        int lIndex = m_lastVisibleIndex;
        double axis = m_hScalePixel;
        boolean flag = false;
        boolean locateCrossHairFlag = false;
        switch (scrollType) {
            // 向左
            case Left:
                if (m_canScroll) {
                    flag = true;
                    if (m_showCrossLine) {
                        if (limitStep > m_scrollStep) {
                            scrollCrosslineLeft(limitStep);
                        } else {
                            scrollCrosslineLeft(m_scrollStep);
                        }
                        locateCrossHairFlag = true;
                    } else {
                        if (limitStep > m_scrollStep) {
                            scrollLeft(limitStep);
                        } else {
                            scrollLeft(m_scrollStep);
                        }
                    }
                }
                break;
            // 向右
            case Right:
                if (m_canScroll) {
                    flag = true;
                    if (m_showCrossLine) {
                        if (limitStep > m_scrollStep) {
                            scrollCrosslineRight(limitStep);
                        } else {
                            scrollCrosslineRight(m_scrollStep);
                        }
                        locateCrossHairFlag = true;
                    } else {
                        if (limitStep > m_scrollStep) {
                            scrollRight(limitStep);
                        } else {
                            scrollRight(m_scrollStep);
                        }
                    }
                }
                break;
            // 缩小
            case zoomIn:
                if (m_canZoom) {
                    flag = true;
                    zoomIn();
                }
                break;
            // 放大
            case zoomOut:
                if (m_canZoom) {
                    flag = true;
                    zoomOut();
                }
                break;
        }
        // 需要重新布局
        if (flag) {
            int fIndex_after = m_firstVisibleIndex;
            int lIndex_after = m_lastVisibleIndex;
            double axis_after = m_hScalePixel;
            int fi = m_firstVisibleIndex;
            int li = m_lastVisibleIndex;
            RefObject<Integer> tempRef_fi = new RefObject<Integer>(fi);
            RefObject<Integer> tempRef_li = new RefObject<Integer>(li);
            correctVisibleRecord(m_dataSource.getRowsCount(), tempRef_fi, tempRef_li);
            fi = tempRef_fi.argvalue;
            li = tempRef_li.argvalue;
            m_firstVisibleIndex = fi;
            m_lastVisibleIndex = li;
            if (fIndex != fIndex_after || lIndex != lIndex_after) {
                adjust();
            }
            resetCrossOverIndex();
            if (locateCrossHairFlag) {
                locateCrossLine();
            }
            if (fIndex == fIndex_after && lIndex == lIndex_after && axis == axis_after) {
                invalidate();
            } else {
                update();
                invalidate();
            }
        }
        // 加速滚动
        if (m_scrollAddSpeed && (scrollType == ScrollType.Left || scrollType == ScrollType.Right)) {
            if (m_scrollStep < 50) {
                m_scrollStep += 5;
            }
        } else {
            m_scrollStep = 1;
        }
    }

    /**
     * 检查并弹出提示框
     */
    public void checkToolTip() {
        Calendar calendar = Calendar.getInstance();
        if (calendar.getTimeInMillis() - m_lastTouchMoveTime.getTimeInMillis() >= m_tooltip_dely) {
            if (m_isTouchMove) {
                boolean show = true;
                // 过滤显示
                if (isOperating()) {
                    show = false;
                }
                ArrayList<ChartDiv> divsCopy = getDivs();
                for (ChartDiv div : divsCopy) {
                    if (div.getSelectArea().isVisible()) {
                        show = false;
                    }
                }
                if (show) {
                    m_showingToolTip = true;
                    // 显示提示框
                    int curRecord = getTouchOverIndex();
                    BaseShape bs = selectShape(curRecord, 0);
                    if (bs != null) {
                        invalidate();
                    }
                }
                m_isTouchMove = false;
            }
        }
    }

    /**
     * 检查最后可见索引
     */
    protected void checkLastVisibleIndex() {
        if (m_lastVisibleIndex > m_dataSource.getRowsCount() - 1) {
            m_lastVisibleIndex = m_dataSource.getRowsCount() - 1;
        }
        if (m_dataSource.getRowsCount() > 0) {
            m_lastVisibleKey = m_dataSource.getXValue(m_lastVisibleIndex);
            if (m_lastVisibleIndex == m_dataSource.getRowsCount() - 1) {
                m_lastRecordIsVisible = true;
            } else {
                m_lastRecordIsVisible = false;
            }
        } else {
            m_lastVisibleKey = 0;
            m_lastRecordIsVisible = true;
        }
    }

    /**
     * 清除图像上的数据，但不改变图形结构
     */
    public void clear() {
        ArrayList<ChartDiv> divsCopy = getDivs();
        // 清除画线工具
        for (ChartDiv cDiv : divsCopy) {
            for (FCPlot plot : cDiv.getPlots(SortType.NONE)) {
                cDiv.removePlot(plot);
                plot.delete();
            }
        }
        // 清除选中框
        closeSelectArea();
        // 清除数据源
        m_dataSource.clear();
        // 重置索引
        m_firstVisibleIndex = -1;
        m_lastVisibleIndex = -1;
        m_lastRecordIsVisible = true;
        m_lastVisibleIndex = 0;
        m_lastVisibleKey = 0;
        m_showCrossLine = false;
    }

    /**
     * 取消选中所有图形，包括K线，柱状图，线等
     */
    public void clearSelectedShape() {
        ArrayList<ChartDiv> divsCopy = getDivs();
        for (ChartDiv cDiv : divsCopy) {
            ArrayList<BaseShape> shapesCopy = cDiv.getShapes(SortType.NONE);
            for (BaseShape bs : shapesCopy) {
                bs.setSelected(false);
            }
        }
    }

    /**
     * 取消选中所有的画线工具
     */
    public void clearSelectedPlot() {
        ArrayList<ChartDiv> divsCopy = getDivs();
        for (ChartDiv cDiv : divsCopy) {
            ArrayList<FCPlot> plotsCopy = cDiv.getPlots(SortType.NONE);
            for (FCPlot bls : plotsCopy) {
                bls.setSelected(false);
            }
        }
        setMovingPlot(null);
    }

    /**
     * 取消选中所有的层
     */
    public void clearSelectedDiv() {
        ArrayList<ChartDiv> divsCopy = getDivs();
        for (ChartDiv div : divsCopy) {
            div.setSelected(false);
        }
    }

    /**
     * 隐藏选中框
     */
    protected void closeSelectArea() {
        ArrayList<ChartDiv> divsCopy = getDivs();
        // 循环遍历所有的层
        for (ChartDiv div : divsCopy) {
            div.getSelectArea().close();
        }
        m_showingSelectArea = false;
    }

    /**
     * 根据记录号获取层极值
     *
     * @param index 索引号
     * @param div 图层
     * @param flag 标识 0:最大值 1:最小值
     * @returns 极值
     */
    public double divMaxOrMin(int index, ChartDiv div, int flag) {
        if (index < 0) {
            return 0;
        }
        if (div != null) {
            ArrayList<Double> vList = new ArrayList<Double>();
            // 循环遍历所有的线条
            ArrayList<BaseShape> shapesCopy = div.getShapes(SortType.NONE);
            for (BaseShape bs : shapesCopy) {
                if (!bs.isVisible()) {
                    continue;
                }
                int[] fields = bs.getFields();
                if (fields != null) {
                    for (int i = 0; i < fields.length; i++) {
                        // 其他线条
                        double value = m_dataSource.get2(index, fields[i]);
                        if (!Double.isNaN(value)) {
                            vList.add(value);
                        }
                    }
                }
            }
            int vListSize = vList.size();
            double vListArray[] = new double[vListSize];
            for (int i = 0; i < vListSize; i++) {
                vListArray[i] = vList.get(i);
            }
            // 返回值
            if (flag == 0) {
                return FCScript.maxValue(vListArray, vListSize);
            } else {
                return FCScript.minValue(vListArray, vListSize);
            }
        }
        return 0;
    }

    /**
     * 销毁资源
     */
    @Override
    public void delete() {
        if (!isDeleted()) {
            stopTimer(m_timerID);
            removeAll();
        }
        super.delete();
    }

    /**
     * 由坐标获取层对象，返回图层对象
     *
     * @param mp 坐标
     * @returns 图层对象
     */
    public ChartDiv findDiv(FCPoint mp) {
        ArrayList<ChartDiv> divsCopy = getDivs();
        for (ChartDiv cDiv : divsCopy) {
            if (mp.y >= cDiv.getTop() && mp.y <= cDiv.getTop() + cDiv.getHeight()) {
                return cDiv;
            }
        }
        return null;
    }

    /**
     * 由图形名称获取包含它的层，返回图层对象
     *
     * @param shape 图形的名称
     * @returns 图层对象
     */
    public ChartDiv findDiv(BaseShape shape) {
        ArrayList<ChartDiv> divsCopy = getDivs();
        for (ChartDiv div : divsCopy) {
            if (div.containsShape(shape)) {
                return div;
            }
        }
        return null;
    }

    /**
     * 获取控件类型
     *
     * @returns 控件类型
     */
    @Override
    public String getControlType() {
        return "Chart";
    }

    /**
     * 获取图层集合的拷备
     *
     * @returns 图层集合
     */
    public ArrayList<ChartDiv> getDivs() {
        ArrayList<ChartDiv> divsCopy = new ArrayList<ChartDiv>();
        if (m_divs != null) {
            int divSize = m_divs.size();
            for (int i = 0; i < divSize; i++) {
                divsCopy.add(m_divs.get(i));
            }
        }
        return divsCopy;
    }

    /**
     * 获取横轴的文字
     *
     * @param date 日期
     * @param lDate 上一日期
     * @param dateType 日期类型
     * @returns 横轴的文字
     */
    public String getHScaleDateString(double date, double lDate, RefObject<DateType> dateType) {
        int tm_year = 0, tm_mon = 0, tm_mday = 0, tm_hour = 0, tm_min = 0, tm_sec = 0, tm_msec = 0;
        RefObject<Integer> tempRef_tm_year = new RefObject<Integer>(tm_year);
        RefObject<Integer> tempRef_tm_mon = new RefObject<Integer>(tm_mon);
        RefObject<Integer> tempRef_tm_mday = new RefObject<Integer>(tm_mday);
        RefObject<Integer> tempRef_tm_hour = new RefObject<Integer>(tm_hour);
        RefObject<Integer> tempRef_tm_min = new RefObject<Integer>(tm_min);
        RefObject<Integer> tempRef_tm_sec = new RefObject<Integer>(tm_sec);
        RefObject<Integer> tempRef_tm_msec = new RefObject<Integer>(tm_msec);
        FCStr.getDataByNum(date, tempRef_tm_year, tempRef_tm_mon, tempRef_tm_mday, tempRef_tm_hour, tempRef_tm_min, tempRef_tm_sec, tempRef_tm_msec);
        tm_year = tempRef_tm_year.argvalue;
        tm_mon = tempRef_tm_mon.argvalue;
        tm_mday = tempRef_tm_mday.argvalue;
        tm_hour = tempRef_tm_hour.argvalue;
        tm_min = tempRef_tm_min.argvalue;
        tm_sec = tempRef_tm_sec.argvalue;
        tm_msec = tempRef_tm_msec.argvalue;
        int l_year = 0, l_mon = 0, l_mday = 0, l_hour = 0, l_min = 0, l_sec = 0, l_msec = 0;
        if (lDate > 0) {
            RefObject<Integer> tempRef_l_year = new RefObject<Integer>(l_year);
            RefObject<Integer> tempRef_l_mon = new RefObject<Integer>(l_mon);
            RefObject<Integer> tempRef_l_mday = new RefObject<Integer>(l_mday);
            RefObject<Integer> tempRef_l_hour = new RefObject<Integer>(l_hour);
            RefObject<Integer> tempRef_l_min = new RefObject<Integer>(l_min);
            RefObject<Integer> tempRef_l_sec = new RefObject<Integer>(l_sec);
            RefObject<Integer> tempRef_l_msec = new RefObject<Integer>(l_msec);
            FCStr.getDataByNum(lDate, tempRef_l_year, tempRef_l_mon, tempRef_l_mday, tempRef_l_hour, tempRef_l_min, tempRef_l_sec, tempRef_l_msec);
            l_year = tempRef_l_year.argvalue;
            l_mon = tempRef_l_mon.argvalue;
            l_mday = tempRef_l_mday.argvalue;
            l_hour = tempRef_l_hour.argvalue;
            l_min = tempRef_l_min.argvalue;
            l_sec = tempRef_l_sec.argvalue;
            l_msec = tempRef_l_msec.argvalue;
        }
        String num = "";
        // 只显示年
        if (tm_year > l_year) {
            dateType.argvalue = DateType.Year;
            return (new Integer(tm_year)).toString();
        }
        // 只显示日
        if (tm_mon > l_mon) {
            dateType.argvalue = DateType.Month;
            String mStr = (new Integer(tm_mon)).toString();
            if (tm_mon < 10) {
                mStr = "0" + mStr;
            }
            return mStr;
        }
        // 只显示日期
        if (tm_mday > l_mday) {
            dateType.argvalue = DateType.Day;
            String dStr = (new Integer(tm_mday)).toString();
            if (tm_mday < 10) {
                dStr = "0" + dStr;
            }
            return dStr;
        }
        // 只显示小时和分钟
        if (tm_hour > l_hour || tm_min > l_min) {
            dateType.argvalue = DateType.Minute;
            String hStr = (new Integer(tm_hour)).toString();
            if (tm_hour < 10) {
                hStr = "0" + hStr;
            }
            String mStr = (new Integer(tm_min)).toString();
            if (tm_min < 10) {
                mStr = "0" + mStr;
            }
            return hStr + ":" + mStr;
        }
        // 只显示秒
        if (tm_sec > l_sec) {
            dateType.argvalue = DateType.Second;
            String sStr = (new Integer(tm_sec)).toString();
            if (tm_sec < 10) {
                sStr = "0" + sStr;
            }
            return sStr;
        }
        // 只显示毫秒
        if (tm_msec > l_msec) {
            dateType.argvalue = DateType.Millisecond;
            return (new Integer(l_msec)).toString();
        }
        return "";
    }

    /**
     * 由坐标点获取它对应的索引
     *
     * @param mp 坐标
     * @returns 索引号
     */
    public int getIndex(FCPoint mp) {
        if (m_reverseHScale) {
            mp.x = m_workingAreaWidth - (mp.x - m_leftVScaleWidth) + m_leftVScaleWidth;
        }
        double pixel = m_hScalePixel;
        int index = getChartIndex(mp.x, m_leftVScaleWidth, pixel, m_firstVisibleIndex);
        if (index < 0) {
            index = 0;
        }
        if (index > m_lastVisibleIndex) {
            index = m_lastVisibleIndex;
        }
        return index;
    }

    public ChartDiv getTouchOverDiv() {
        FCPoint mp = getTouchPoint();
        ArrayList<ChartDiv> divsCopy = getDivs();
        for (ChartDiv cDiv : divsCopy) {
            if (mp.y >= cDiv.getTop() && mp.y <= cDiv.getTop() + cDiv.getHeight()) {
                return cDiv;
            }
        }
        return null;
    }

    /**
     * 获取触摸所在横向记录索引，索引是从0开始的，最大值为显示条数-1
     *
     * @returns 索引号
     */
    public int getTouchOverIndex() {
        FCPoint mp = getTouchPoint();
        if (m_reverseHScale) {
            mp.x = m_workingAreaWidth - (mp.x - m_leftVScaleWidth) + m_leftVScaleWidth;
        }
        double pixel = m_hScalePixel;
        return getChartIndex(mp.x, m_leftVScaleWidth, pixel, m_firstVisibleIndex);
    }

    /**
     * 获取坐标轴的值
     *
     * @param div 图层
     * @param mp 坐标
     * @param attachVScale 对应坐标轴
     * @returns 指标值
     */
    public double getNumberValue(ChartDiv div, FCPoint mp, AttachVScale attachVScale) {
        VScale vScale = div.getVScale(attachVScale);
        int vHeight = div.getWorkingAreaHeight() - vScale.getPaddingTop() - vScale.getPaddingBottom();
        int cY = mp.y - div.getTop() - div.getTitleBar().getHeight() - vScale.getPaddingTop();
        if (vScale.isReverse()) {
            cY = vHeight - cY;
        }
        if (vHeight > 0) {
            double max = 0;
            double min = 0;
            boolean isLog = false;
            // 左轴
            if (attachVScale == AttachVScale.Left) {
                max = div.getLeftVScale().getVisibleMax();
                min = div.getLeftVScale().getVisibleMin();
                if (max == 0 && min == 0) {
                    max = div.getRightVScale().getVisibleMax();
                    min = div.getRightVScale().getVisibleMin();
                }
                isLog = div.getLeftVScale().getSystem() == VScaleSystem.Logarithmic;
            }
            // 右轴
            else if (attachVScale == AttachVScale.Right) {
                max = div.getRightVScale().getVisibleMax();
                min = div.getRightVScale().getVisibleMin();
                if (max == 0 && min == 0) {
                    max = div.getLeftVScale().getVisibleMax();
                    min = div.getLeftVScale().getVisibleMin();
                }
                isLog = div.getRightVScale().getSystem() == VScaleSystem.Logarithmic;
            }
            if (isLog) {
                if (max >= 0) {
                    max = Math.log10(max);
                } else {
                    max = -Math.log10(Math.abs(max));
                }
                if (min >= 0) {
                    min = Math.log10(min);
                } else {
                    min = -Math.log10(Math.abs(min));
                }
                double value = getVScaleValue(cY, max, min, vHeight);
                return Math.pow(10, value);
            } else {
                return getVScaleValue(cY, max, min, vHeight);
            }
        }
        return 0;
    }

    /**
     * 获取属性值
     *
     * @param name 属性名称
     * @param value 返回属性值
     * @param type 返回属性类型
     */
    @Override
    public void getProperty(String name, RefObject<String> value, RefObject<String> type) {
        if (name.equals("autofillhscale")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(getAutoFillHScale());
        } else if (name.equals("blankspace")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getBlankSpace());
        } else if (name.equals("canmoveshape")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(canmoveShape());
        } else if (name.equals("canresizeh")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(canResizeH());
        } else if (name.equals("canresizev")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(canResizeV());
        } else if (name.equals("canscroll")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(getCanScroll());
        } else if (name.equals("canzoom")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(canZoom());
        } else if (name.equals("crosslinemovemode")) {
            type.argvalue = "enum:CrossLineMoveMode";
            if (getCrossLineMoveMode() == CrossLineMoveMode.AfterClick) {
                value.argvalue = "AfterClick";
            } else {
                value.argvalue = "FollowTouch";
            }
        } else if (name.equals("hscalefieldtext")) {
            type.argvalue = "text";
            value.argvalue = getHScaleFieldText();
        } else if (name.equals("hscalepixel")) {
            type.argvalue = "double";
            value.argvalue = FCStr.convertDoubleToStr(getHScalePixel());
        } else if (name.equals("leftvscalewidth")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getLeftVScaleWidth());
        } else if (name.equals("reversehscale")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(getReverseHScale());
        } else if (name.equals("rightvscalewidth")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getRightVScaleWidth());
        } else if (name.equals("scrolladdspeed")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(scrollAddSpeed());
        } else if (name.equals("showcrossline")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(getShowCrossLine());
        } else {
            super.getProperty(name, value, type);
        }
    }

    /**
     * 获取属性名称列表
     *
     * @returns 属性名称列表
     */
    @Override
    public ArrayList<String> getPropertyNames() {
        ArrayList<String> propertyNames = super.getPropertyNames();
        propertyNames.addAll(Arrays.asList(new String[]{"AutoFillHScale", "BlankSpace", "CanMoveShape", "CanResizeH", "CanResizeV", "CanScroll", "CanZoom", "CrossLineMoveMode", "HScaleFieldText", "HScalePixel", "LeftVScaleWidth", "ReverseHScale", "RightVScaleWidth", "ScrollAddSpeed", "ShowCrossLine"}));
        return propertyNames;
    }

    /**
     * 由字段获取所有的图形
     *
     * @param field 字段
     * @returns 图形对象的数量
     */
    public int getShapesCount(int field) {
        int count = 0;
        ArrayList<ChartDiv> divsCopy = getDivs();
        for (ChartDiv div : divsCopy) {
            ArrayList<BaseShape> shapesCopy = div.getShapes(SortType.NONE);
            for (BaseShape bs : shapesCopy) {
                int[] fields = bs.getFields();
                if (fields != null) {
                    for (int i = 0; i < fields.length; i++) {
                        if (fields[i] == field) {
                            count++;
                        }
                    }
                }
            }
        }
        return count;
    }

    /**
     * 获取纵轴的基础字段
     *
     * @param div 图层
     * @param vScale 指定纵轴
     * @returns 基准值
     */
    protected int getVScaleBaseField(ChartDiv div, VScale vScale) {
        int baseField = vScale.getBaseField();
        if (baseField == FCDataTable.NULLFIELD) {
            ArrayList<BaseShape> baseShapes = div.getShapes(SortType.ASC);
            if (baseShapes.size() > 0) {
                baseField = baseShapes.get(0).getBaseField();
            }
        }
        return baseField;
    }

    /**
     * 获取纵轴的基准值
     *
     * @param div 图层
     * @param vScale 指定纵轴
     * @param i 索引号
     * @returns 基准值
     */
    public double getVScaleBaseValue(ChartDiv div, VScale vScale, int i) {
        double baseValue = 0;
        int baseField = getVScaleBaseField(div, vScale);
        if (baseField != FCDataTable.NULLFIELD && m_dataSource.getRowsCount() > 0) {
            if (i >= m_firstVisibleIndex && i <= m_lastVisibleIndex) {
                double value = m_dataSource.get2(i, baseField);
                if (!Double.isNaN(value)) {
                    baseValue = value;
                }
            }
        } else {
            baseValue = vScale.getMidValue();
        }
        return baseValue;
    }

    /**
     * 获取指定索引的横坐标
     *
     * @param index 指定索引
     * @returns 横坐标
     */
    public float getX(int index) {
        float x = (float) (m_leftVScaleWidth + (index - m_firstVisibleIndex) * m_hScalePixel + m_hScalePixel / 2 + 1);
        if (m_reverseHScale) {
            return m_workingAreaWidth - (x - m_leftVScaleWidth) + m_leftVScaleWidth + m_blankSpace;
        } else {
            return x;
        }
    }

    /**
     * 获取某一值对应的纵坐标
     *
     * @param div 图层
     * @param value 指定的值
     * @param attach true为左纵轴，false为右纵轴
     * @returns 纵坐标
     */
    public float getY(ChartDiv div, double value, AttachVScale attach) {
        if (div != null) {
            VScale scale = div.getVScale(attach);
            double max = scale.getVisibleMax();
            double min = scale.getVisibleMin();
            // 对数坐标系
            if (scale.getSystem() == VScaleSystem.Logarithmic) {
                if (value > 0) {
                    value = Math.log10(value);
                } else if (value < 0) {
                    value = -Math.log10(Math.abs(value));
                }
                if (max > 0) {
                    max = Math.log10(max);
                } else if (max < 0) {
                    max = -Math.log10(Math.abs(max));
                }
                if (min > 0) {
                    min = Math.log10(scale.getVisibleMin());
                } else if (min < 0) {
                    min = -Math.log10(Math.abs(min));
                }
            }
            // 计算坐标
            if (max != min) {
                int wHeight = div.getWorkingAreaHeight() - scale.getPaddingTop() - scale.getPaddingBottom();
                if (wHeight > 0) {
                    float y = (float) ((max - value) / (max - min) * wHeight);
                    if (scale.isReverse()) {
                        return div.getTitleBar().getHeight() + div.getWorkingAreaHeight() - scale.getPaddingBottom() - y;
                    } else {
                        return div.getTitleBar().getHeight() + scale.getPaddingTop() + y;
                    }
                }
            }
        }
        return 0;
    }

    public static int gridScale(double min, double max, int yLen, int maxSpan, int minSpan, int defCount, RefObject<Double> step, RefObject<Integer> digit) {
        double sub = max - min;
        int nMinCount = (int) Math.ceil((double) yLen / maxSpan);
        int nMaxCount = (int) Math.floor((double) yLen / minSpan);
        int nCount = defCount;
        double logStep = sub / nCount;
        boolean start = false;
        double divisor = 0;
        int i = 0, nTemp = 0;
        step.argvalue = 0.0;
        digit.argvalue = 0;
        nCount = Math.max(nMinCount, nCount);
        nCount = Math.min(nMaxCount, nCount);
        nCount = Math.max(nCount, 1);
        for (i = 15; i >= -6; i--) {
            divisor = Math.pow(10, i);
            if (divisor < 1) {
                digit.argvalue++;
            }
            nTemp = (int) Math.floor(logStep / divisor);
            if (start) {
                if (nTemp < 4) {
                    if (digit.argvalue > 0) {
                        digit.argvalue--;
                    }
                } else if (nTemp >= 4 && nTemp <= 6) {
                    nTemp = 5;
                    step.argvalue += nTemp * divisor;
                } else {
                    step.argvalue += 10 * divisor;
                    if (digit.argvalue > 0) {
                        digit.argvalue--;
                    }
                }
                break;
            } else if (nTemp > 0) {
                step.argvalue = nTemp * divisor + step.argvalue;
                logStep -= step.argvalue;
                start = true;
            }
        }
        return 0;
    }

    /**
     * 判断是否正在操作图形
     *
     * @returns 是否正在操作图形
     */
    public boolean isOperating() {
        if (m_movingPlot != null || m_movingShape != null || m_hResizeType != 0 || m_userResizeDiv != null) {
            return true;
        }
        return false;
    }

    /**
     * 定位十字线
     */
    public void locateCrossLine() {
        if (m_dataSource.getRowsCount() > 0) {
            // 循环遍历图层
            ArrayList<ChartDiv> divsCopy = getDivs();
            for (ChartDiv div : divsCopy) {
                if (m_cross_y >= div.getTop() && m_cross_y <= div.getTop() + div.getHeight()) {
                    if (div.getWorkingAreaHeight() > 0 && m_crossStopIndex >= 0 && m_crossStopIndex < m_dataSource.getRowsCount()) {
                        // 设置十字线沿K线移动
                        ArrayList<BaseShape> shapesCopy = div.getShapes(SortType.DESC);
                        // 设置十字线沿趋势线或成交量移动
                        for (BaseShape tls : shapesCopy) {
                            if (tls.isVisible()) {
                                double value = m_dataSource.get2(m_crossStopIndex, tls.getBaseField());
                                if (!Double.isNaN(value)) {
                                    m_cross_y = (int) getY(div, value, tls.getAttachVScale()) + div.getTop();
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /**
     * 将图形移动到另一个层中
     *
     * @param cDiv 目标层
     * @param shape 将要移动的图形
     */
    public void moveShape(ChartDiv cDiv, BaseShape shape) {
        // 循环遍历所有的层
        ArrayList<ChartDiv> divsCopy = getDivs();
        for (ChartDiv div : divsCopy) {
            div.removeShape(shape);
        }
        // 移动到新的层
        if (cDiv != null) {
            cDiv.addShape(shape);
        }
    }

    /**
     * 重置十字线穿越的记录号
     */
    public void resetCrossOverIndex() {
        if (m_showCrossLine) {
            m_crossStopIndex = resetCrossOverIndex(m_dataSource.getRowsCount(), m_maxVisibleRecord, m_crossStopIndex, m_firstVisibleIndex, m_lastVisibleIndex);
        }
        m_isScrollCross = true;
    }

    /**
     *  重置图像，删除所有的数据，层，指标和画线工具等
     */
    public void removeAll() {
        clear();
        if (m_divs != null) {
            for (ChartDiv div : m_divs) {
                div.delete();
            }
            m_divs.clear();
        }
        m_dataSource.clear();
        m_cross_y = -1;
        m_showingToolTip = false;
    }

    /**
     * 移除图层
     *
     * @param div 图层
     */
    public void removeDiv(ChartDiv div) {
        m_divs.remove(div);
        update();
    }

    /**
     * 拖动图层改变大小
     */
    public boolean resizeDiv() {
        int width = getWidth(), height = getHeight();
        // 横向改变大小
        if (m_hResizeType > 0) {
            FCPoint mp = getTouchPoint();
            // 左轴
            if (m_hResizeType == 1) {
                if (mp.x > 0 && mp.x < width - m_rightVScaleWidth - 50) {
                    m_leftVScaleWidth = mp.x;
                }
            }
            // 右轴
            else if (m_hResizeType == 2) {
                if (mp.x > m_leftVScaleWidth + 50 && mp.x < width) {
                    m_rightVScaleWidth = width - mp.x;
                }
            }
            m_hResizeType = 0;
            update();
            return true;
        }
        // 纵向改变
        if (m_userResizeDiv != null) {
            FCPoint mp = getTouchPoint();
            ChartDiv nextCP = null;
            boolean rightP = false;
            // 循环遍历
            ArrayList<ChartDiv> divsCopy = getDivs();
            for (ChartDiv cDiv : divsCopy) {
                if (rightP) {
                    nextCP = cDiv;
                    break;
                }
                if (cDiv == m_userResizeDiv) {
                    rightP = true;
                }
            }
            float sumPercent = 0;
            // 循环遍历所有层
            for (ChartDiv div : divsCopy) {
                sumPercent += div.getVerticalPercent();
            }
            float originalVP = m_userResizeDiv.getVerticalPercent();
            FCRect uRect = m_userResizeDiv.getBounds();
            // 缩小
            if (mp.x >= uRect.left && mp.x <= uRect.right && mp.y >= uRect.top && mp.y <= uRect.bottom) {
                m_userResizeDiv.setVerticalPercent(sumPercent * (mp.y - m_userResizeDiv.getTop()) / getHeight());
                if (m_userResizeDiv.getVerticalPercent() < 1) {
                    m_userResizeDiv.setVerticalPercent(1);
                }
                if (nextCP != null) {
                    nextCP.setVerticalPercent(nextCP.getVerticalPercent() + originalVP - m_userResizeDiv.getVerticalPercent());
                }
            }
            // 放大
            else {
                if (nextCP != null) {
                    FCRect nRect = nextCP.getBounds();
                    if (mp.x >= nRect.left && mp.x <= nRect.right && mp.y >= nRect.top && mp.y <= nRect.bottom) {
                        m_userResizeDiv.setVerticalPercent(sumPercent * (mp.y - m_userResizeDiv.getTop()) / getHeight());
                        if (m_userResizeDiv.getVerticalPercent() >= originalVP + nextCP.getVerticalPercent()) {
                            m_userResizeDiv.setVerticalPercent(m_userResizeDiv.getVerticalPercent() - 1);
                        }
                        nextCP.setVerticalPercent(originalVP + nextCP.getVerticalPercent() - m_userResizeDiv.getVerticalPercent());
                    }
                }
            }
            m_userResizeDiv = null;
            update();
            return true;
        }
        return false;
    }

    /**
     * 重置
     */
    public void reset() {
        if (isVisible()) {
            resetVisibleRecord();
            adjust();
            resetCrossOverIndex();
        }
    }

    /**
     * 自动设置首先可见和最后可见的记录号
     */
    public void resetVisibleRecord() {
        ArrayList<ChartDiv> divs = getDivs();
        if (divs.size() > 0) {
            int rowsCount = m_dataSource.getRowsCount();
            if (m_autoFillHScale) {
                if (m_workingAreaWidth > 0 && rowsCount > 0) {
                    m_hScalePixel = (double) m_workingAreaWidth / rowsCount;
                    m_maxVisibleRecord = rowsCount;
                    m_firstVisibleIndex = 0;
                    m_lastVisibleIndex = rowsCount - 1;
                }
            } else {
                m_maxVisibleRecord = getMaxVisibleCount(m_hScalePixel, m_workingAreaWidth);
                // 没数据时重置
                if (rowsCount == 0) {
                    m_firstVisibleIndex = -1;
                    m_lastVisibleIndex = -1;
                } else {
                    // 数据不足一屏时
                    if (rowsCount < m_maxVisibleRecord) {
                        m_lastVisibleIndex = rowsCount - 1;
                        m_firstVisibleIndex = 0;
                    }
                    // 数据超过一屏时
                    else {
                        // 显示中间的数据时
                        if (m_firstVisibleIndex != -1 && m_lastVisibleIndex != -1 && !m_lastRecordIsVisible) {
                            int index = m_dataSource.getRowIndex(m_lastVisibleKey);
                            if (index != -1) {
                                m_lastVisibleIndex = index;
                            }
                            m_firstVisibleIndex = m_lastVisibleIndex - m_maxVisibleRecord + 1;
                            if (m_firstVisibleIndex < 0) {
                                m_firstVisibleIndex = 0;
                                m_lastVisibleIndex = m_firstVisibleIndex + m_maxVisibleRecord;
                                checkLastVisibleIndex();
                            }
                        } else {
                            // 第一条或最后一条数据被显示时
                            m_lastVisibleIndex = rowsCount - 1;
                            m_firstVisibleIndex = m_lastVisibleIndex - m_maxVisibleRecord + 1;
                            if (m_firstVisibleIndex > m_lastVisibleIndex) {
                                m_firstVisibleIndex = m_lastVisibleIndex;
                            }
                        }
                    }
                }
            }
        }
    }

    /**
     * 设置属性
     *
     * @param name 属性名称
     * @param value 属性值
     */
    @Override
    public void setProperty(String name, String value) {
        if (name.equals("autofillhscale")) {
            setAutoFillHScale(FCStr.convertStrToBool(value));
        } else if (name.equals("blankspace")) {
            setBlankSpace(FCStr.convertStrToInt(value));
        } else if (name.equals("canmoveshape")) {
            setCanMoveShape(FCStr.convertStrToBool(value));
        } else if (name.equals("canresizeh")) {
            setCanResizeH(FCStr.convertStrToBool(value));
        } else if (name.equals("canresizev")) {
            setCanResizeV(FCStr.convertStrToBool(value));
        } else if (name.equals("canscroll")) {
            setCanScroll(FCStr.convertStrToBool(value));
        } else if (name.equals("canzoom")) {
            setCanZoom(FCStr.convertStrToBool(value));
        } else if (name.equals("crosslinemovemode")) {
            if (value.equals("AfterClick")) {
                setCrossLineMoveMode(CrossLineMoveMode.AfterClick);
            } else {
                setCrossLineMoveMode(CrossLineMoveMode.FollowTouch);
            }
        } else if (name.equals("hscalefieldtext")) {
            setHScaleFieldText(value);
        } else if (name.equals("hscalepixel")) {
            setHScalePixel(FCStr.convertStrToDouble(value));
        } else if (name.equals("leftvscalewidth")) {
            setLeftVScaleWidth(FCStr.convertStrToInt(value));
        } else if (name.equals("reversehscale")) {
            setReverseHScale(FCStr.convertStrToBool(value));
        } else if (name.equals("rightvscalewidth")) {
            setRightVScaleWidth(FCStr.convertStrToInt(value));
        } else if (name.equals("scrolladdspeed")) {
            setscrollAddSpeed(FCStr.convertStrToBool(value));
        } else if (name.equals("showcrossline")) {
            setShowCrossLine(FCStr.convertStrToBool(value));
        } else {
            super.setProperty(name, value);
        }
    }

    /**
     * 左滚十字线，仅在显示十字线时有效
     *
     * @param step 幅度，即滚动跨越的记录条数
     */
    protected void scrollCrosslineLeft(int step) {
        int currentIndex = m_crossStopIndex;
        m_crossStopIndex = currentIndex - step;
        if (m_crossStopIndex < 0) {
            m_crossStopIndex = 0;
        }
        if (currentIndex <= m_firstVisibleIndex) {
            scrollLeft(step);
        }
    }

    /**
     * 右滚十字线，仅在显示十字线时有效
     *
     * @param step 幅度，即滚动跨越的记录条数
     */
    public void scrollCrosslineRight(int step) {
        int currentIndex = m_crossStopIndex;
        m_crossStopIndex = currentIndex + step;
        if (m_dataSource.getRowsCount() < m_maxVisibleRecord) {
            if (m_crossStopIndex >= m_maxVisibleRecord - 1) {
                m_crossStopIndex = m_maxVisibleRecord - 1;
            }
        }
        if (currentIndex >= m_lastVisibleIndex) {
            scrollRight(step);
        }
    }

    /**
     * 左滚画面
     *
     * @param step 幅度，即滚动跨越的记录条数
     */
    public void scrollLeft(int step) {
        if (!m_autoFillHScale) {
            RefObject<Integer> tempRef_m_firstVisibleIndex = new RefObject<Integer>(m_firstVisibleIndex);
            RefObject<Integer> tempRef_m_lastVisibleIndex = new RefObject<Integer>(m_lastVisibleIndex);
            scrollLeft(step, m_dataSource.getRowsCount(), m_hScalePixel, m_workingAreaWidth, tempRef_m_firstVisibleIndex, tempRef_m_lastVisibleIndex);
            m_firstVisibleIndex = tempRef_m_firstVisibleIndex.argvalue;
            m_lastVisibleIndex = tempRef_m_lastVisibleIndex.argvalue;
            checkLastVisibleIndex();
        }
    }

    /**
     * 立即向左滚动到显示第一条数据为止
     */
    public void scrollLeftToBegin() {
        if (!m_autoFillHScale && m_dataSource.getRowsCount() > 0) {
            m_firstVisibleIndex = 0;
            m_lastVisibleIndex = m_maxVisibleRecord - 1;
            checkLastVisibleIndex();
            m_crossStopIndex = m_firstVisibleIndex;
        }
    }

    /**
     * 右滚画面
     *
     * @param step 幅度，即滚动跨越的记录条数
     */
    public void scrollRight(int step) {
        if (!m_autoFillHScale) {
            RefObject<Integer> tempRef_m_firstVisibleIndex = new RefObject<Integer>(m_firstVisibleIndex);
            RefObject<Integer> tempRef_m_lastVisibleIndex = new RefObject<Integer>(m_lastVisibleIndex);
            scrollRight(step, m_dataSource.getRowsCount(), m_hScalePixel, m_workingAreaWidth, tempRef_m_firstVisibleIndex, tempRef_m_lastVisibleIndex);
            m_firstVisibleIndex = tempRef_m_firstVisibleIndex.argvalue;
            m_lastVisibleIndex = tempRef_m_lastVisibleIndex.argvalue;
            checkLastVisibleIndex();
        }
    }

    /**
     * 立即向右滚动到显示最后一条数据为止
     */
    public void scrollRightToEnd() {
        if (!m_autoFillHScale && m_dataSource.getRowsCount() > 0) {
            m_lastVisibleIndex = m_dataSource.getRowsCount() - 1;
            checkLastVisibleIndex();
            m_firstVisibleIndex = m_lastVisibleIndex - m_maxVisibleRecord + 1;
            if (m_firstVisibleIndex < 0) {
                m_firstVisibleIndex = 0;
            }
            m_crossStopIndex = m_lastVisibleIndex;
        }
    }

    /**
     * 判断是否选中柱状图
     *
     * @param div 图层
     * @param mpY 触摸所在纵坐标
     * @param fieldName 字段名
     * @param fieldName2 字段名2
     * @param styleField 样式字段
     * @param attachVScale 依附坐标轴
     * @param curIndex 当前索引
     * @returns 是否选中
     */
    public boolean selectBar(ChartDiv div, float mpY, int fieldName, int fieldName2, int styleField, AttachVScale attachVScale, int curIndex) {
        int style = 1;
        //自定义样式
        if (styleField != FCDataTable.NULLFIELD) {
            double defineStyle = m_dataSource.get2(curIndex, styleField);
            if (!Double.isNaN(defineStyle)) {
                style = (int) defineStyle;
            }
        }
        if (style == -10000 || curIndex < 0 || curIndex > m_lastVisibleIndex || Double.isNaN(m_dataSource.get2(curIndex, fieldName))) {
            return false;
        }
        double midValue = 0;
        if (fieldName2 != FCDataTable.NULLFIELD) {
            midValue = m_dataSource.get2(curIndex, fieldName2);
        }
        double volumn = m_dataSource.get2(curIndex, fieldName);
        float y = getY(div, volumn, attachVScale);
        float midY = getY(div, midValue, attachVScale);
        float topY = y + div.getTop();
        float bottomY = midY + div.getTop();
        // 修正
        if (topY > bottomY) {
            topY = midY + div.getTop();
            bottomY = y + div.getTop();
        }
        topY -= 1;
        bottomY += 1;
        // 垂直区域判断
        if (topY >= div.getTop() && bottomY <= div.getTop() + div.getHeight() && mpY >= topY && mpY <= bottomY) {
            return true;
        }
        return false;
    }

    /**
     * 判断是否选中柱状图
     *
     * @param div 图层
     * @param mpY 触摸纵坐标
     * @param highField 最高价字段
     * @param lowField 最低价字段
     * @param styleField 样式字段
     * @param attachVScale 依附坐标轴
     * @param curIndex 当前索引
     * @returns 是否选中
     */
    public boolean selectCandle(ChartDiv div, float mpY, int highField, int lowField, int styleField, AttachVScale attachVScale, int curIndex) {
        int style = 1;
        // 自定义样式
        if (styleField != FCDataTable.NULLFIELD) {
            double defineStyle = m_dataSource.get2(curIndex, styleField);
            if (!Double.isNaN(defineStyle)) {
                style = (int) defineStyle;
            }
        }
        double highValue = 0;
        double lowValue = 0;
        if (style == -10000 || curIndex < 0 || curIndex > m_lastVisibleIndex) {
            return false;
        } else {
            highValue = m_dataSource.get2(curIndex, highField);
            lowValue = m_dataSource.get2(curIndex, lowField);
            if (Double.isNaN(highValue) || Double.isNaN(lowValue)) {
                return false;
            }
        }
        float highY = getY(div, highValue, attachVScale);
        float lowY = getY(div, lowValue, attachVScale);
        // 垂直判断
        float topY = highY + div.getTop();
        float bottomY = lowY + div.getTop();
        // 修正坐标
        if (topY > bottomY) {
            float temp = topY;
            topY = bottomY;
            bottomY = temp;
        }
        topY -= 1;
        bottomY += 1;
        if (topY >= div.getTop() && bottomY <= div.getTop() + div.getHeight() && mpY >= topY && mpY <= bottomY) {
            return true;
        }
        return false;
    }

    /**
     * 判断是否选中线
     *
     * @param div 图层
     * @param mp 触摸位置
     * @param fieldName 字段名
     * @param lineWidth 线的宽度
     * @param attachVScale 依附坐标轴
     * @param curIndex 当前索引
     * @returns 是否选中
     */
    public boolean selectPolyline(ChartDiv div, FCPoint mp, int fieldName, float lineWidth, AttachVScale attachVScale, int curIndex) {
        if (curIndex < 0 || curIndex > m_lastVisibleIndex || (Double.isNaN(m_dataSource.get2(curIndex, fieldName)))) {
            return false;
        }
        double lineValue = (m_dataSource.get2(curIndex, fieldName));
        float topY = getY(div, lineValue, attachVScale) + div.getTop();
        // 间隔较小
        if (m_hScalePixel <= 1) {
            if (topY >= div.getTop() && topY <= div.getTop() + div.getHeight() && mp.y >= topY - 8 && mp.y <= topY + 8) {
                return true;
            }
        }
        // 间隔较大
        else {
            int index = curIndex;
            int scaleX = (int) getX(index);
            float judgeTop = 0;
            float judgeScaleX = scaleX;
            // 左侧判断
            if (mp.x >= scaleX) {
                int leftIndex = curIndex + 1;
                if (curIndex < m_lastVisibleIndex && (!Double.isNaN(m_dataSource.get2(leftIndex, fieldName)))) {
                    double rightValue = m_dataSource.get2(leftIndex, fieldName);
                    judgeTop = getY(div, rightValue, attachVScale) + div.getTop();
                    if (judgeTop > div.getTop() + div.getHeight() - div.getHScale().getHeight() || judgeTop < div.getTop() + div.getTitleBar().getHeight()) {
                        return false;
                    }
                } else {
                    judgeTop = topY;
                }
            }
            // 右侧判断
            else {
                judgeScaleX = scaleX - (int) m_hScalePixel;
                int rightIndex = curIndex - 1;
                if (curIndex > 0 && (!Double.isNaN(m_dataSource.get2(rightIndex, fieldName)))) {
                    double leftValue = (m_dataSource.get2(rightIndex, fieldName));
                    judgeTop = getY(div, leftValue, attachVScale) + div.getTop();
                    if (judgeTop > div.getTop() + div.getHeight() - div.getHScale().getHeight() || judgeTop < div.getTop() + div.getTitleBar().getHeight()) {
                        return false;
                    }
                } else {
                    judgeTop = topY;
                }
            }
            float judgeX = 0, judgeY = 0, judgeW = 0, judgeH = 0;
            if (judgeTop >= topY) {
                judgeX = judgeScaleX;
                judgeY = topY - 2 - lineWidth;
                judgeW = (float) m_hScalePixel;
                judgeH = judgeTop - topY + lineWidth < 4 ? 4 : judgeTop - topY + 4 + lineWidth;
            } else {
                judgeX = judgeScaleX;
                judgeY = judgeTop - 2 - lineWidth / 2;
                judgeW = (float) m_hScalePixel;
                judgeH = topY - judgeTop + lineWidth < 4 ? 4 : topY - judgeTop + 4 + lineWidth;
            }
            if (mp.x >= judgeX && mp.x <= judgeX + judgeW && mp.y >= judgeY && mp.y <= judgeY + judgeH) {
                return true;
            }
        }
        return false;
    }

    /**
     * 选中线条的方法
     *
     * @param state 状态 0:只判断不选中 1:单选
     * @param curIndex 当前选中的索引号
     * @returns 线条
     */
    public BaseShape selectShape(int curIndex, int state) {
        BaseShape selectObj = null;
        // 获取选中点
        FCPoint mp = getTouchPoint();
        // 循环遍历所有的Div
        ArrayList<ChartDiv> divsCopy = getDivs();
        for (ChartDiv div : divsCopy) {
            // 按照顺序获取线条
            ArrayList<BaseShape> sortedBs = div.getShapes(SortType.DESC);
            // 循环遍历线条
            for (BaseShape bShape : sortedBs) {
                if (bShape.isVisible()) {
                    if (selectObj != null) {
                        if (state == 1) {
                            bShape.setSelected(false);
                        }
                    } else {
                        if (m_firstVisibleIndex == -1 || m_lastVisibleIndex == -1) {
                            if (state == 1) {
                                bShape.setSelected(false);
                            }
                            continue;
                        }
                        // 点图
                        boolean isSelect = false;
                        // 线图
                        if (bShape instanceof PolylineShape) {
                            PolylineShape tls = (PolylineShape) ((bShape instanceof PolylineShape) ? bShape : null);
                            isSelect = selectPolyline(div, mp, tls.getBaseField(), tls.getWidth(), tls.getAttachVScale(), curIndex);
                        }
                        // K线
                        else if (bShape instanceof CandleShape) {
                            CandleShape cs = (CandleShape) ((bShape instanceof CandleShape) ? bShape : null);
                            // 收盘线
                            if (cs.getStyle() == CandleStyle.CloseLine) {
                                isSelect = selectPolyline(div, mp, cs.getCloseField(), 1, cs.getAttachVScale(), curIndex);
                            }
                            // 其他类型
                            else {
                                isSelect = selectCandle(div, mp.y, cs.getHighField(), cs.getLowField(), cs.getStyleField(), cs.getAttachVScale(), curIndex);
                            }
                        }
                        // 柱状图
                        else if (bShape instanceof BarShape) {
                            BarShape barS = (BarShape) ((bShape instanceof BarShape) ? bShape : null);
                            isSelect = selectBar(div, mp.y, barS.getFieldName(), barS.getFieldName2(), barS.getStyleField(), barS.getAttachVScale(), curIndex);
                        }
                        // 保存选中
                        if (isSelect) {
                            selectObj = bShape;
                            if (state == 1) {
                                bShape.setSelected(true);
                            }
                        } else {
                            if (state == 1) {
                                bShape.setSelected(false);
                            }
                        }
                    }
                }
            }
        }
        return selectObj;
    }

    /**
     * 设置图形首先可见的索引号和最后可见的索引号
     *
     * @param firstVisibleIndex 开始记录号
     * @param lastVisibleIndex 结束记录号
     */
    public void setVisibleIndex(int firstVisibleIndex, int lastVisibleIndex) {
        double xScalePixel = (double) m_workingAreaWidth / (lastVisibleIndex - firstVisibleIndex + 1);
        if (xScalePixel < 1000000) {
            m_firstVisibleIndex = firstVisibleIndex;
            m_lastVisibleIndex = lastVisibleIndex;
            // 设置最后一条记录是否可见
            if (lastVisibleIndex != m_dataSource.getRowsCount() - 1) {
                m_lastRecordIsVisible = false;
            } else {
                m_lastRecordIsVisible = true;
            }
            setHScalePixel(xScalePixel);
            m_maxVisibleRecord = getMaxVisibleCount(m_hScalePixel, m_workingAreaWidth);
            checkLastVisibleIndex();
        }
    }

    /**
     * 重新布局
     */
    @Override
    public void update() {
        super.update();
        m_workingAreaWidth = getWidth() - m_leftVScaleWidth - m_rightVScaleWidth - m_blankSpace - 1;
        if (m_workingAreaWidth < 0) {
            m_workingAreaWidth = 0;
        }
        int locationY = 0;
        float sumPercent = 0;
        // 循环遍历所有层
        ArrayList<ChartDiv> divsCopy = getDivs();
        for (ChartDiv div : divsCopy) {
            sumPercent += div.getVerticalPercent();
        }
        if (sumPercent > 0) {
            // 调整纵向的位置
            for (ChartDiv cDiv : divsCopy) {
                cDiv.setBounds(new FCRect(0, locationY, getWidth(), locationY + (int) (getHeight() * cDiv.getVerticalPercent() / sumPercent)));
                cDiv.setWorkingAreaHeight(cDiv.getHeight() - cDiv.getHScale().getHeight() - cDiv.getTitleBar().getHeight() - 1);
                locationY += (int) (getHeight() * cDiv.getVerticalPercent() / sumPercent);
            }
        }
        reset();
    }

    /**
     * 放大
     */
    public void zoomOut() {
        if (!m_autoFillHScale) {
            double hp = m_hScalePixel;
            RefObject<Integer> tempRef_m_firstVisibleIndex = new RefObject<Integer>(m_firstVisibleIndex);
            RefObject<Integer> tempRef_m_lastVisibleIndex = new RefObject<Integer>(m_lastVisibleIndex);
            RefObject<Double> tempRef_hp = new RefObject<Double>(hp);
            zoomOut(m_workingAreaWidth, m_dataSource.getRowsCount(), tempRef_m_firstVisibleIndex, tempRef_m_lastVisibleIndex, tempRef_hp);
            m_firstVisibleIndex = tempRef_m_firstVisibleIndex.argvalue;
            m_lastVisibleIndex = tempRef_m_lastVisibleIndex.argvalue;
            hp = tempRef_hp.argvalue;
            setHScalePixel(hp);
            m_maxVisibleRecord = getMaxVisibleCount(m_hScalePixel, m_workingAreaWidth);
            checkLastVisibleIndex();
        }
    }

    /**
     * 缩小
     */
    public void zoomIn() {
        if (!m_autoFillHScale) {
            double hp = m_hScalePixel;
            RefObject<Integer> tempRef_m_firstVisibleIndex = new RefObject<Integer>(m_firstVisibleIndex);
            RefObject<Integer> tempRef_m_lastVisibleIndex = new RefObject<Integer>(m_lastVisibleIndex);
            RefObject<Double> tempRef_hp = new RefObject<Double>(hp);
            zoomIn(m_workingAreaWidth, m_dataSource.getRowsCount(), tempRef_m_firstVisibleIndex, tempRef_m_lastVisibleIndex, tempRef_hp);
            m_firstVisibleIndex = tempRef_m_firstVisibleIndex.argvalue;
            m_lastVisibleIndex = tempRef_m_lastVisibleIndex.argvalue;
            hp = tempRef_hp.argvalue;
            setHScalePixel(hp);
            m_maxVisibleRecord = getMaxVisibleCount(m_hScalePixel, m_workingAreaWidth);
            checkLastVisibleIndex();
        }
    }

    /**
     * 绘制文字
     *
     * @param paint 绘图对象
     * @param text 文字
     * @param dwPenColor 颜色
     * @param font 字体
     * @param point 坐标
     */
    public void drawText(FCPaint paint, String text, long dwPenColor, FCFont font, FCPoint FCPoint) {
        FCSize tSize = paint.textSize(text, font);
        FCRect tRect = new FCRect(FCPoint.x, FCPoint.y, FCPoint.x + tSize.cx, FCPoint.y + tSize.cy);
        paint.drawText(text, dwPenColor, font, tRect);
    }

    /**
     * 画细线，只能是水平线或垂直线
     *
     * @param paint 绘图对象
     * @param color 颜色
     * @param width 宽度
     * @param x1 第一个点的横坐标
     * @param y1 第一个点的纵坐标
     * @param x2 第二个点的横坐标
     * @param y2 第二个点的纵坐标
     */
    public void drawThinLine(FCPaint paint, long color, int width, float x1, float y1, float x2, float y2) {
        FCHost host = getNative().getHost();
        if (width > 1 || getNative().allowScaleSize()) {
            paint.drawLine(color, width, 0, (int) x1, (int) y1, (int) x2, (int) y2);
        } else {
            int x = (int) (x1 < x2 ? x1 : x2);
            int y = (int) (y1 < y2 ? y1 : y2);
            int w = (int) Math.abs(x1 - x2);
            int h = (int) Math.abs(y1 - y2);
            if (w < 1) {
                w = 1;
            }
            if (h < 1) {
                h = 1;
            }
            if ((w > 1 && h == 1) || (w == 1 && h > 1)) {
                paint.fillRect(color, x, y, x + w, y + h);
            } else {
                paint.drawLine(color, width, 0, (int) x1, (int) y1, (int) x2, (int) y2);
            }
        }
    }

    /**
     * 获取纵轴的刻度
     *
     * @param max 最大值
     * @param min 最小值
     * @param vScale 坐标轴
     * @returns 刻度集合
     */
    public ArrayList<Double> getVScaleStep(double max, double min, ChartDiv div, VScale vScale) {
        ArrayList<Double> scaleStepList = new ArrayList<Double>();
        // 等差，百分比
        if (vScale.getType() == VScaleType.EqualDiff || vScale.getType() == VScaleType.Percent) {
            double step = 0;
            int distance = div.getVGrid().getDistance();
            int digit = 0, gN = div.getWorkingAreaHeight() / distance;
            if (gN == 0) {
                gN = 1;
            }
            RefObject<Double> tempRef_step = new RefObject<Double>(step);
            RefObject<Integer> tempRef_digit = new RefObject<Integer>(digit);
            // 计算显示值
            gridScale(min, max, div.getWorkingAreaHeight(), distance, distance / 2, gN, tempRef_step, tempRef_digit);
            step = tempRef_step.argvalue;
            digit = tempRef_digit.argvalue;
            if (step > 0) {
                double start = 0;
                if (min >= 0) {
                    while (start + step < min) {
                        start += step;
                    }
                } else {
                    while (start - step > min) {
                        start -= step;
                    }
                }
                while (start <= max) {
                    scaleStepList.add(start);
                    start += step;
                }
            }
        }
        // 等比
        else if (vScale.getType() == VScaleType.EqualRatio) {
            // 获取基础字段
            int baseField = getVScaleBaseField(div, vScale);
            double bMax = Double.MIN_VALUE;
            double bMin = Double.MAX_VALUE;
            if (baseField != -1) {
                // 循环遍历数据
                for (int i = 0; i < m_dataSource.getRowsCount(); i++) {
                    double value = m_dataSource.get2(i, baseField);
                    if (!Double.isNaN(value)) {
                        if (value > bMax) {
                            bMax = value;
                        }
                        if (value < bMin) {
                            bMin = value;
                        }
                    }
                }
                // 生成坐标刻度
                if (bMax != Double.MIN_VALUE && bMin != Double.MAX_VALUE && bMin > 0 && bMax > 0 && bMin < bMax) {
                    double num = bMin;
                    while (num < bMax) {
                        num = num * 1.1;
                        if (num >= min && num <= max) {
                            scaleStepList.add(num);
                        }
                    }
                }
            }
        }
        // 等分
        else if (vScale.getType() == VScaleType.Divide) {
            scaleStepList.add(min + (max - min) * 0.25);
            scaleStepList.add(min + (max - min) * 0.5);
            scaleStepList.add(min + (max - min) * 0.75);
        }
        // 黄金分割
        else if (vScale.getType() == VScaleType.GoldenRatio) {
            scaleStepList.add(min);
            scaleStepList.add(min + (max - min) * 0.191);
            scaleStepList.add(min + (max - min) * 0.382);
            scaleStepList.add(min + (max - min) * 0.5);
            scaleStepList.add(min + (max - min) * 0.618);
            scaleStepList.add(min + (max - min) * 0.809);
        }
        if ((max != min) && scaleStepList.isEmpty()) {
            if (!Double.isNaN(min)) {
                scaleStepList.add(min);
            }
        }
        return scaleStepList;
    }

    /**
     * 绘制成交量
     *
     * @param paint 绘图对象
     * @param div 要绘制的层
     * @param bs 线条对象
     */
    public void onPaintBar(FCPaint paint, ChartDiv div, BarShape bs) {
        int ciFieldName1 = m_dataSource.getColumnIndex(bs.getFieldName());
        int ciFieldName2 = m_dataSource.getColumnIndex(bs.getFieldName2());
        int ciStyle = m_dataSource.getColumnIndex(bs.getStyleField());
        int ciClr = m_dataSource.getColumnIndex(bs.getColorField());
        int defaultLineWidth = 1;
        if (!isOperating() && m_crossStopIndex != -1) {
            if (selectBar(div, getTouchPoint().y, bs.getFieldName(), bs.getFieldName2(), bs.getStyleField(), bs.getAttachVScale(), m_crossStopIndex)) {
                defaultLineWidth = 2;
            }
        }
        for (int i = m_firstVisibleIndex; i <= m_lastVisibleIndex; i++) {
            int thinLineWidth = 1;
            if (i == m_crossStopIndex) {
                thinLineWidth = defaultLineWidth;
            }
            //样式
            int style = -10000;
            switch (bs.getStyle()) {
                case Line:
                    style = 2;
                    break;
                case Rect:
                    style = 0;
                    break;
            }
            // 自定义样式
            if (ciStyle != -1) {
                double defineStyle = m_dataSource.get3(i, ciStyle);
                if (!Double.isNaN(defineStyle)) {
                    style = (int) defineStyle;
                }
            }
            if (style == -10000) {
                continue;
            }
            double value = m_dataSource.get3(i, ciFieldName1);
            int scaleX = (int) getX(i);
            double midValue = 0;
            if (ciFieldName2 != -1) {
                midValue = m_dataSource.get3(i, ciFieldName2);
            }
            float midY = getY(div, midValue, bs.getAttachVScale());
            if (!Double.isNaN(value)) {
                float barY = getY(div, value, bs.getAttachVScale());
                int startPX = scaleX;
                int startPY = (int) midY;
                int endPX = scaleX;
                int endPY = (int) barY;
                if (bs.getStyle() == BarStyle.Rect) {
                    //修正
                    if (startPY == div.getHeight() - div.getHScale().getHeight()) {
                        startPY = div.getHeight() - div.getHScale().getHeight() + 1;
                    }
                }
                int x = 0, y = 0, width = 0, height = 0;
                width = (int) (m_hScalePixel * 2 / 3);
                if (width % 2 == 0) {
                    width += 1;
                }
                if (width < 3) {
                    width = 1;
                }
                x = scaleX - width / 2;
                // 获取阴阳柱的矩形
                if (startPY >= endPY) {
                    y = endPY;
                } else {
                    y = startPY;
                }
                height = Math.abs(startPY - endPY);
                if (height < 1) {
                    height = 1;
                }
                // 获取自定义颜色
                long barColor = FCColor.None;
                if (ciClr != -1) {
                    double defineColor = m_dataSource.get3(i, ciClr);
                    if (!Double.isNaN(defineColor)) {
                        barColor = (long) defineColor;
                    }
                }
                if (barColor == FCColor.None) {
                    if (startPY >= endPY) {
                        barColor = bs.getUpColor();
                    } else {
                        barColor = bs.getDownColor();
                    }
                }
                switch (style) {
                    // 虚线空心矩形
                    case -1:
                        if (m_hScalePixel <= 3) {
                            drawThinLine(paint, barColor, thinLineWidth, startPX, y, startPX, y + height);
                        } else {
                            FCRect rect = new FCRect(x, y, x + width, y + height);
                            paint.fillRect(div.getBackColor(), rect);
                            paint.drawRect(barColor, thinLineWidth, 2, rect);
                        }
                        break;
                    // 实心矩形
                    case 0:
                        if (m_hScalePixel <= 3) {
                            drawThinLine(paint, barColor, thinLineWidth, startPX, y, startPX, y + height);
                        } else {
                            FCRect rect = new FCRect(x, y, x + width, y + height);
                            paint.fillRect(barColor, rect);
                            if (thinLineWidth > 1) {
                                if (startPY >= endPY) {
                                    paint.drawRect(bs.getDownColor(), thinLineWidth, 0, rect);
                                } else {
                                    paint.drawRect(bs.getUpColor(), thinLineWidth, 0, rect);
                                }
                            }
                        }
                        break;
                    // 空心矩形
                    case 1:
                        if (m_hScalePixel <= 3) {
                            drawThinLine(paint, barColor, thinLineWidth, startPX, y, startPX, y + height);
                        } else {
                            FCRect rect = new FCRect(x, y, x + width, y + height);
                            paint.fillRect(div.getBackColor(), rect);
                            paint.drawRect(barColor, thinLineWidth, 0, rect);
                        }
                        break;
                    // 线
                    case 2:
                        if (startPY <= 0) {
                            startPY = 0;
                        }
                        if (startPY >= div.getHeight()) {
                            startPY = div.getHeight();
                        }
                        if (endPY <= 0) {
                            endPY = 0;
                        }
                        if (endPY >= div.getHeight()) {
                            endPY = div.getHeight();
                        }
                        // 画线
                        if (bs.getLineWidth() <= 1) {
                            drawThinLine(paint, barColor, thinLineWidth, startPX, startPY, endPX, endPY);
                        } else {
                            int lineWidth = bs.getLineWidth();
                            if (lineWidth > m_hScalePixel) {
                                lineWidth = (int) m_hScalePixel;
                            }
                            if (lineWidth < 1) {
                                lineWidth = 1;
                            }
                            paint.drawLine(barColor, lineWidth + thinLineWidth - 1, 0, startPX, startPY, endPX, endPY);
                        }
                        break;
                }
                if (bs.isSelected()) {
                    // 画选中框
                    int kPInterval = m_maxVisibleRecord / 30;
                    if (kPInterval < 2) {
                        kPInterval = 2;
                    }
                    if (i % kPInterval == 0) {
                        if (barY >= div.getTitleBar().getHeight() && barY <= div.getHeight() - div.getHScale().getHeight()) {
                            FCRect sRect = new FCRect(scaleX - 3, (int) barY - 4, scaleX + 4, (int) barY + 3);
                            paint.fillRect(bs.getSelectedColor(), sRect);
                        }
                    }
                }
            }
            // 画零线
            if (i == m_lastVisibleIndex && div.getVScale(bs.getAttachVScale()).getVisibleMin() < 0) {
                if (m_reverseHScale) {
                    float left = (float) (m_leftVScaleWidth + m_workingAreaWidth - (m_lastVisibleIndex - m_firstVisibleIndex + 1) * m_hScalePixel);
                    paint.drawLine(bs.getDownColor(), 1, 0, m_leftVScaleWidth + m_workingAreaWidth, (int) midY, (int) left, (int) midY);
                } else {
                    float right = (float) (m_leftVScaleWidth + (m_lastVisibleIndex - m_firstVisibleIndex + 1) * m_hScalePixel);
                    paint.drawLine(bs.getDownColor(), 1, 0, m_leftVScaleWidth, (int) midY, (int) right, (int) midY);
                }
            }
        }
    }

    /**
     * 绘制K线
     *
     * @param paint 绘图对象
     * @param div 要绘制的层
     * @param cs K线
     */
    public void onPaintCandle(FCPaint paint, ChartDiv div, CandleShape cs) {
        int visibleMaxIndex = -1, visibleMinIndex = -1;
        double visibleMax = 0, visibleMin = 0;
        int x = 0, y = 0;
        ArrayList<FCPoint> points = new ArrayList<FCPoint>();
        int ciHigh = m_dataSource.getColumnIndex(cs.getHighField());
        int ciLow = m_dataSource.getColumnIndex(cs.getLowField());
        int ciOpen = m_dataSource.getColumnIndex(cs.getOpenField());
        int ciClose = m_dataSource.getColumnIndex(cs.getCloseField());
        int ciStyle = m_dataSource.getColumnIndex(cs.getStyleField());
        int ciClr = m_dataSource.getColumnIndex(cs.getColorField());
        int defaultLineWidth = 1;
        if (!isOperating() && m_crossStopIndex != -1) {
            if (selectCandle(div, getTouchPoint().y, cs.getHighField(), cs.getLowField(), cs.getStyleField(), cs.getAttachVScale(), m_crossStopIndex)) {
                defaultLineWidth = 2;
            }
        }
        for (int i = m_firstVisibleIndex; i <= m_lastVisibleIndex; i++) {
            int thinLineWidth = 1;
            if (i == m_crossStopIndex) {
                thinLineWidth = defaultLineWidth;
            }
            // 样式
            int style = -10000;
            switch (cs.getStyle()) {
                case Rect:
                    style = 0;
                    break;
                case American:
                    style = 3;
                    break;
                case CloseLine:
                    style = 4;
                    break;
                case Tower:
                    style = 5;
                    break;
            }
            // 自定义样式
            if (ciStyle != -1) {
                double defineStyle = m_dataSource.get3(i, ciStyle);
                if (!Double.isNaN(defineStyle)) {
                    style = (int) defineStyle;
                }
            }
            if (style == 10000) {
                continue;
            }
            // 获取值
            double open = m_dataSource.get3(i, ciOpen);
            double high = m_dataSource.get3(i, ciHigh);
            double low = m_dataSource.get3(i, ciLow);
            double close = m_dataSource.get3(i, ciClose);
            if (Double.isNaN(open) || Double.isNaN(high) || Double.isNaN(low) || Double.isNaN(close)) {
                if (i != m_lastVisibleIndex || style != 4) {
                    continue;
                }
            }
            int scaleX = (int) getX(i);
            if (cs.showMaxMin()) {
                // 设置可见部分最大最小值及索引
                if (i == m_firstVisibleIndex) {
                    visibleMaxIndex = i;
                    visibleMinIndex = i;
                    visibleMax = high;
                    visibleMin = low;
                } else {
                    // 最大值
                    if (high > visibleMax) {
                        visibleMax = high;
                        visibleMaxIndex = i;
                    }
                    // 最小值
                    if (low < visibleMin) {
                        visibleMin = low;
                        visibleMinIndex = i;
                    }
                }
            }
            // 获取各值所在Y值
            float highY = getY(div, high, cs.getAttachVScale());
            float openY = getY(div, open, cs.getAttachVScale());
            float lowY = getY(div, low, cs.getAttachVScale());
            float closeY = getY(div, close, cs.getAttachVScale());
            int cwidth = (int) (m_hScalePixel * 2 / 3);
            if (cwidth % 2 == 0) {
                cwidth += 1;
            }
            if (cwidth < 3) {
                cwidth = 1;
            }
            int xsub = cwidth / 2;
            if (xsub < 1) {
                xsub = 1;
            }
            switch (style) {
                // 美国线
                case 3: {
                    long color = cs.getUpColor();
                    if (open > close) {
                        color = cs.getDownColor();
                    }
                    if (ciClr != -1) {
                        double defineColor = m_dataSource.get3(i, ciClr);
                        if (!Double.isNaN(defineColor)) {
                            color = (long) defineColor;
                        }
                    }
                    if ((int) highY != (int) lowY) {
                        if (m_hScalePixel <= 3) {
                            drawThinLine(paint, color, thinLineWidth, scaleX, highY, scaleX, lowY);
                        } else {
                            drawThinLine(paint, color, thinLineWidth, scaleX, highY, scaleX, lowY);
                            drawThinLine(paint, color, thinLineWidth, scaleX - xsub, openY, scaleX, openY);
                            drawThinLine(paint, color, thinLineWidth, scaleX, closeY, scaleX + xsub, closeY);
                        }
                    } else {
                        drawThinLine(paint, color, thinLineWidth, scaleX - xsub, closeY, scaleX + xsub, closeY);
                    }
                }
                break;
                // 收盘线
                case 4: {
                    RefObject<Integer> tempRef_x = new RefObject<Integer>(x);
                    RefObject<Integer> tempRef_y = new RefObject<Integer>(y);
                    onPaintPolyline(paint, div, cs.getUpColor(), FCColor.None, cs.getColorField(), defaultLineWidth, PolylineStyle.SolidLine, close, cs.getAttachVScale(), scaleX, (int) closeY, i, points, tempRef_x, tempRef_y);
                    x = tempRef_x.argvalue;
                    y = tempRef_y.argvalue;
                    break;
                }
                default: {
                    // 阳线
                    if (open <= close) {
                        //获取阳线的高度
                        float recth = getUpCandleHeight(close, open, div.getVScale(cs.getAttachVScale()).getVisibleMax(), div.getVScale(cs.getAttachVScale()).getVisibleMin(), div.getWorkingAreaHeight() - div.getVScale(cs.getAttachVScale()).getPaddingBottom() - div.getVScale(cs.getAttachVScale()).getPaddingTop());
                        if (recth < 1) {
                            recth = 1;
                        }
                        // 获取阳线的矩形
                        int rcUpX = scaleX - xsub, rcUpTop = (int) closeY, rcUpBottom = (int) openY, rcUpW = cwidth, rcUpH = (int) recth;
                        if (openY < closeY) {
                            rcUpTop = (int) openY;
                            rcUpBottom = (int) closeY;
                        }
                        long upColor = FCColor.None;
                        int colorField = cs.getColorField();
                        if (colorField != FCDataTable.NULLFIELD) {
                            double defineColor = m_dataSource.get2(i, colorField);
                            if (!Double.isNaN(defineColor)) {
                                upColor = (long) defineColor;
                            }
                        }
                        if (upColor == FCColor.None) {
                            upColor = cs.getUpColor();
                        }
                        switch (style) {
                            // 矩形
                            case 0:
                            case 1:
                            case 2:
                                if ((int) highY != (int) lowY) {
                                    drawThinLine(paint, upColor, thinLineWidth, scaleX, highY, scaleX, lowY);
                                    if (m_hScalePixel > 3) {
                                        // 描背景
                                        if ((int) openY == (int) closeY) {
                                            drawThinLine(paint, upColor, thinLineWidth, rcUpX, rcUpBottom, rcUpX + rcUpW, rcUpBottom);
                                        } else {
                                            FCRect rcUp = new FCRect(rcUpX, rcUpTop, rcUpX + rcUpW, rcUpBottom);
                                            if (style == 0 || style == 1) {
                                                if (rcUpW > 0 && rcUpH > 0 && m_hScalePixel > 3) {
                                                    paint.fillRect(div.getBackColor(), rcUp);
                                                }
                                                paint.drawRect(upColor, thinLineWidth, 0, rcUp);
                                            } else if (style == 2) {
                                                paint.fillRect(upColor, rcUp);
                                                if (thinLineWidth > 1) {
                                                    paint.drawRect(upColor, thinLineWidth, 0, rcUp);
                                                }
                                            }
                                        }
                                    }
                                } else {
                                    drawThinLine(paint, upColor, thinLineWidth, scaleX - xsub, closeY, scaleX + xsub, closeY);
                                }
                                break;
                            // 宝塔线
                            case 5: {
                                double lOpen = m_dataSource.get3(i - 1, ciOpen);
                                double lClose = m_dataSource.get3(i - 1, ciClose);
                                double lHigh = m_dataSource.get3(i - 1, ciHigh);
                                double lLow = m_dataSource.get3(i - 1, ciLow);
                                float top = highY;
                                float bottom = lowY;
                                if ((int) highY > (int) lowY) {
                                    top = lowY;
                                    bottom = highY;
                                }
                                if (i == 0 || Double.isNaN(lOpen) || Double.isNaN(lClose) || Double.isNaN(lHigh) || Double.isNaN(lLow)) {
                                    if (m_hScalePixel <= 3) {
                                        drawThinLine(paint, upColor, thinLineWidth, rcUpX, top, rcUpX, bottom);
                                    } else {
                                        int rcUpHeight = (int) Math.abs(bottom - top == 0 ? 1 : bottom - top);
                                        if (rcUpW > 0 && rcUpHeight > 0) {
                                            FCRect rcUp = new FCRect(rcUpX, top, rcUpX + rcUpW, top + rcUpHeight);
                                            paint.fillRect(upColor, rcUp);
                                            if (thinLineWidth > 1) {
                                                paint.drawRect(upColor, thinLineWidth, 0, rcUp);
                                            }
                                        }
                                    }
                                } else {
                                    if (m_hScalePixel <= 3) {
                                        drawThinLine(paint, upColor, thinLineWidth, rcUpX, top, rcUpX, bottom);
                                    } else {
                                        int rcUpHeight = (int) Math.abs(bottom - top == 0 ? 1 : bottom - top);
                                        if (rcUpW > 0 && rcUpHeight > 0) {
                                            FCRect rcUp = new FCRect(rcUpX, top, rcUpX + rcUpW, top + rcUpHeight);
                                            paint.fillRect(upColor, rcUp);
                                            if (thinLineWidth > 1) {
                                                paint.drawRect(upColor, thinLineWidth, 0, rcUp);
                                            }
                                        }
                                    }
                                    // 上一股价为下跌，画未超过最高点部分
                                    if (lClose < lOpen && low < lHigh) {
                                        // 获取矩形
                                        int tx = rcUpX;
                                        int ty = (int) getY(div, lHigh, cs.getAttachVScale());
                                        if (high < lHigh) {
                                            ty = (int) highY;
                                        }
                                        int width = rcUpW;
                                        int height = (int) lowY - ty;
                                        if (height > 0) {
                                            if (m_hScalePixel <= 3) {
                                                drawThinLine(paint, cs.getDownColor(), thinLineWidth, tx, ty, tx, ty + height);
                                            } else {
                                                if (width > 0 && height > 0) {
                                                    FCRect tRect = new FCRect(tx, ty, tx + width, ty + height);
                                                    paint.fillRect(cs.getDownColor(), tRect);
                                                    if (thinLineWidth > 1) {
                                                        paint.drawRect(cs.getDownColor(), thinLineWidth, 0, tRect);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            }
                        }
                    }
                    // 阴线
                    else {
                        // 获取阴线的高度
                        float recth = getDownCandleHeight(close, open, div.getVScale(cs.getAttachVScale()).getVisibleMax(), div.getVScale(cs.getAttachVScale()).getVisibleMin(), div.getWorkingAreaHeight() - div.getVScale(cs.getAttachVScale()).getPaddingBottom() - div.getVScale(cs.getAttachVScale()).getPaddingTop());
                        if (recth < 1) {
                            recth = 1;
                        }
                        // 获取阴线的矩形
                        int rcDownX = scaleX - xsub, rcDownTop = (int) openY, rcDownBottom = (int) closeY, rcDownW = cwidth, rcDownH = (int) recth;
                        if (closeY < openY) {
                            rcDownTop = (int) closeY;
                            rcDownBottom = (int) openY;
                        }
                        long downColor = FCColor.None;
                        if (ciClr != -1) {
                            double defineColor = m_dataSource.get3(i, ciClr);
                            if (!Double.isNaN(defineColor)) {
                                downColor = (long) defineColor;
                            }
                        }
                        if (downColor == FCColor.None) {
                            downColor = cs.getDownColor();
                        }
                        switch (style) {
                            // 标准
                            case 0:
                            case 1:
                            case 2:
                                if ((int) highY != (int) lowY) {
                                    drawThinLine(paint, downColor, thinLineWidth, scaleX, highY, scaleX, lowY);
                                    if (m_hScalePixel > 3) {
                                        FCRect rcDown = new FCRect(rcDownX, rcDownTop, rcDownX + rcDownW, rcDownBottom);
                                        if (style == 1) {
                                            if (rcDownW > 0 && rcDownH > 0 && m_hScalePixel > 3) {
                                                paint.fillRect(div.getBackColor(), rcDown);
                                            }
                                            paint.drawRect(downColor, thinLineWidth, 0, rcDown);
                                        } else if (style == 0 || style == 1) {
                                            paint.fillRect(downColor, rcDown);
                                            if (thinLineWidth > 1) {
                                                paint.drawRect(downColor, thinLineWidth, 0, rcDown);
                                            }
                                        }
                                    }
                                } else {
                                    drawThinLine(paint, downColor, thinLineWidth, scaleX - xsub, closeY, scaleX + xsub, closeY);
                                }
                                break;
                            // 宝塔线
                            case 5:
                                double lOpen = m_dataSource.get3(i - 1, ciOpen);
                                double lClose = m_dataSource.get3(i - 1, ciClose);
                                double lHigh = m_dataSource.get3(i - 1, ciHigh);
                                double lLow = m_dataSource.get3(i - 1, ciLow);
                                float top = highY;
                                float bottom = lowY;
                                if ((int) highY > (int) lowY) {
                                    top = lowY;
                                    bottom = highY;
                                }
                                if (i == 0 || Double.isNaN(lOpen) || Double.isNaN(lClose) || Double.isNaN(lHigh) || Double.isNaN(lLow)) {
                                    if (m_hScalePixel <= 3) {
                                        drawThinLine(paint, downColor, thinLineWidth, rcDownX, top, rcDownX, bottom);
                                    } else {
                                        int rcDownHeight = (int) Math.abs(bottom - top == 0 ? 1 : bottom - top);
                                        if (rcDownW > 0 && rcDownHeight > 0) {
                                            FCRect rcDown = new FCRect(rcDownX, top, rcDownX + rcDownW, rcDownBottom);
                                            paint.fillRect(downColor, rcDown);
                                            if (thinLineWidth > 1) {
                                                paint.drawRect(downColor, thinLineWidth, 0, rcDown);
                                            }
                                        }
                                    }
                                } else {
                                    // 先画阳线部分
                                    if (m_hScalePixel <= 3) {
                                        drawThinLine(paint, downColor, thinLineWidth, rcDownX, top, rcDownX, bottom);
                                    } else {
                                        int rcDownHeight = (int) Math.abs(bottom - top == 0 ? 1 : bottom - top);
                                        if (rcDownW > 0 && rcDownHeight > 0) {
                                            FCRect rcDown = new FCRect(rcDownX, top, rcDownX + rcDownW, rcDownBottom);
                                            paint.fillRect(downColor, rcDown);
                                            if (thinLineWidth > 1) {
                                                paint.drawRect(downColor, thinLineWidth, 0, rcDown);
                                            }
                                        }
                                    }
                                    // 上一股价为上涨，画未跌过最高点部分
                                    if (lClose >= lOpen && high > lLow) {
                                        // 获取矩形
                                        int tx = rcDownX;
                                        int ty = (int) highY;
                                        int width = rcDownW;
                                        int height = (int) Math.abs(getY(div, lLow, cs.getAttachVScale()) - ty);
                                        if (low > lLow) {
                                            height = (int) lowY - ty;
                                        }
                                        if (height > 0) {
                                            if (m_hScalePixel <= 3) {
                                                drawThinLine(paint, cs.getUpColor(), thinLineWidth, tx, ty, tx, ty + height);
                                            } else {
                                                if (width > 0 && height > 0) {
                                                    FCRect tRect = new FCRect(tx, ty, tx + width, ty + height);
                                                    paint.fillRect(cs.getUpColor(), new FCRect(tx, ty, tx + width, ty + height));
                                                    if (thinLineWidth > 1) {
                                                        paint.drawRect(cs.getUpColor(), thinLineWidth, 0, tRect);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    break;
                }
            }
            // 绘制选中
            if (cs.isSelected()) {
                int kPInterval = m_maxVisibleRecord / 30;
                if (kPInterval < 2) {
                    kPInterval = 3;
                }
                if (i % kPInterval == 0) {
                    if (!Double.isNaN(open) && !Double.isNaN(high) && !Double.isNaN(low) && !Double.isNaN(close)) {
                        if (closeY >= div.getTitleBar().getHeight() && closeY <= div.getHeight() - div.getHScale().getHeight()) {
                            FCRect rect = new FCRect(scaleX - 3, (int) closeY - 4, scaleX + 4, (int) closeY + 3);
                            paint.fillRect(cs.getSelectedColor(), rect);
                        }
                    }
                }
            }
        }
        onPaintCandleEx(paint, div, cs, visibleMaxIndex, visibleMax, visibleMinIndex, visibleMin);
    }

    /**
     * 绘制K线的扩展属性
     *
     * @param paint 绘图对象
     * @param div 要绘制的层
     * @param cs K线
     * @param visibleMaxIndex 最大值索引
     * @param visibleMax 最大值
     * @param visibleMinIndex 最小值索引
     * @param visibleMin 最小值
     */
    public void onPaintCandleEx(FCPaint paint, ChartDiv div, CandleShape cs, int visibleMaxIndex, double visibleMax, int visibleMinIndex, double visibleMin) {
        if (m_dataSource.getRowsCount() > 0) {
            if (visibleMaxIndex != -1 && visibleMinIndex != -1 && cs.showMaxMin()) {
                double max = visibleMax;
                double min = visibleMin;
                float scaleYMax = getY(div, max, cs.getAttachVScale());
                float scaleYMin = getY(div, min, cs.getAttachVScale());
                // 画K线的最大值
                int scaleXMax = (int) getX(visibleMaxIndex);
                int digit = div.getVScale(cs.getAttachVScale()).getDigit();
                FCSize maxSize = paint.textSize(FCStr.getValueByDigit(max, digit), div.getFont());
                float maxPX = 0, maxPY = 0;
                float strY = 0;
                if (scaleYMax > scaleYMin) {
                    RefObject<Float> tempRef_maxPX = new RefObject<Float>(maxPX);
                    RefObject<Float> tempRef_maxPY = new RefObject<Float>(maxPY);
                    getCandleMinStringPoint(scaleXMax, scaleYMax, maxSize.cx, maxSize.cy, getWidth(), m_leftVScaleWidth, m_rightVScaleWidth, tempRef_maxPX, tempRef_maxPY);
                    maxPX = tempRef_maxPX.argvalue;
                    maxPY = tempRef_maxPY.argvalue;
                    strY = maxPY + maxSize.cy;
                } else {
                    RefObject<Float> tempRef_maxPX2 = new RefObject<Float>(maxPX);
                    RefObject<Float> tempRef_maxPY2 = new RefObject<Float>(maxPY);
                    getCandleMaxStringPoint(scaleXMax, scaleYMax, maxSize.cx, maxSize.cy, getWidth(), m_leftVScaleWidth, m_rightVScaleWidth, tempRef_maxPX2, tempRef_maxPY2);
                    maxPX = tempRef_maxPX2.argvalue;
                    maxPY = tempRef_maxPY2.argvalue;
                    strY = maxPY;
                }
                FCPoint maxP = new FCPoint((int) maxPX, (int) maxPY);
                drawText(paint, FCStr.getValueByDigit(max, digit), cs.getTagColor(), div.getFont(), maxP);
                paint.drawLine(cs.getTagColor(), 1, 0, scaleXMax, (int) scaleYMax, maxP.x + maxSize.cx / 2, (int) strY);
                // 画K线的最小值
                FCSize minSize = paint.textSize(FCStr.getValueByDigit(min, digit), div.getFont());
                int scaleXMin = (int) getX(visibleMinIndex);
                float minPX = 0, minPY = 0;
                if (scaleYMax > scaleYMin) {
                    RefObject<Float> tempRef_minPX = new RefObject<Float>(minPX);
                    RefObject<Float> tempRef_minPY = new RefObject<Float>(minPY);
                    getCandleMaxStringPoint(scaleXMin, scaleYMin, minSize.cx, minSize.cy, getWidth(), m_leftVScaleWidth, m_rightVScaleWidth, tempRef_minPX, tempRef_minPY);
                    minPX = tempRef_minPX.argvalue;
                    minPY = tempRef_minPY.argvalue;
                    strY = minPY;
                } else {
                    RefObject<Float> tempRef_minPX2 = new RefObject<Float>(minPX);
                    RefObject<Float> tempRef_minPY2 = new RefObject<Float>(minPY);
                    getCandleMinStringPoint(scaleXMin, scaleYMin, minSize.cx, minSize.cy, getWidth(), m_leftVScaleWidth, m_rightVScaleWidth, tempRef_minPX2, tempRef_minPY2);
                    minPX = tempRef_minPX2.argvalue;
                    minPY = tempRef_minPY2.argvalue;
                    strY = minPY + minSize.cy;
                }
                FCPoint minP = new FCPoint((int) minPX, (int) minPY);
                drawText(paint, FCStr.getValueByDigit(min, digit), cs.getTagColor(), div.getFont(), minP);
                paint.drawLine(cs.getTagColor(), 1, 0, scaleXMin, (int) scaleYMin, minP.x + minSize.cx / 2, (int) strY);
            }
        }
    }

    /**
     * 绘制十字线
     *
     * @param paint 绘图对象
     * @param div 要绘制的层
     */
    public void onPaintCrossLine(FCPaint paint, ChartDiv div) {
        FCPoint touchPoint = getTouchPoint();
        if (m_cross_y != -1) {
            int divWidth = div.getWidth();
            int divHeight = div.getHeight();
            int titleBarHeight = div.getTitleBar().getHeight();
            int hScaleHeight = div.getHScale().getHeight();
            int mpY = m_cross_y - div.getTop();
            if (m_dataSource.getRowsCount() > 0 && m_hResizeType == 0 && m_userResizeDiv == null) {
                if (mpY >= titleBarHeight && mpY <= divHeight - hScaleHeight) {
                    // 显示左侧Y轴的提示框及文字
                    VScale leftVScale = div.getLeftVScale();
                    CrossLineTip crossLineTip = leftVScale.getCrossLineTip();
                    if (m_leftVScaleWidth != 0 && crossLineTip.isVisible()) {
                        if (crossLineTip.allowUserPaint()) {
                            FCRect clipRect = new FCRect(0, 0, m_leftVScaleWidth, divHeight - hScaleHeight);
                            crossLineTip.onPaint(paint, div, clipRect);
                        } else {
                            double lValue = getNumberValue(div, touchPoint, AttachVScale.Left);
                            String leftValue = FCStr.getValueByDigit(lValue, leftVScale.getDigit());
                            FCSize leftYTipFontSize = paint.textSize(leftValue, crossLineTip.getFont());
                            if (leftYTipFontSize.cx > 0 && leftYTipFontSize.cy > 0) {
                                int lRtX = m_leftVScaleWidth - leftYTipFontSize.cx - 1;
                                int lRtY = mpY - leftYTipFontSize.cy / 2;
                                int lRtW = leftYTipFontSize.cx;
                                int lRtH = leftYTipFontSize.cy;
                                if (lRtW > 0 && lRtH > 0) {
                                    FCRect lRtL = new FCRect(lRtX, lRtY, lRtX + lRtW, lRtY + lRtH);
                                    paint.fillRect(crossLineTip.getBackColor(), lRtL);
                                    paint.drawRect(crossLineTip.getTextColor(), 1, 0, lRtL);
                                }
                                drawText(paint, leftValue, crossLineTip.getTextColor(), crossLineTip.getFont(), new FCPoint(lRtX, lRtY));
                            }
                        }
                    }
                    // 显示右侧Y轴的提示框及文字
                    VScale rightVScale = div.getRightVScale();
                    crossLineTip = rightVScale.getCrossLineTip();
                    if (m_rightVScaleWidth != 0 && crossLineTip.isVisible()) {
                        if (crossLineTip.allowUserPaint()) {
                            FCRect clipRect = new FCRect(divWidth - m_rightVScaleWidth, 0, divWidth, divHeight - hScaleHeight);
                            crossLineTip.onPaint(paint, div, clipRect);
                        } else {
                            double rValue = getNumberValue(div, touchPoint, AttachVScale.Right);
                            String rightValue = FCStr.getValueByDigit(rValue, rightVScale.getDigit());
                            FCSize rightYTipFontSize = paint.textSize(rightValue, crossLineTip.getFont());
                            if (rightYTipFontSize.cx > 0 && rightYTipFontSize.cy > 0) {
                                int rRtX = getWidth() - m_rightVScaleWidth + 1;
                                int rRtY = mpY - rightYTipFontSize.cy / 2;
                                int rRtW = rightYTipFontSize.cx;
                                int rRtH = rightYTipFontSize.cy;
                                if (rRtW > 0 && rRtH > 0) {
                                    FCRect rRtL = new FCRect(rRtX, rRtY, rRtX + rRtW, rRtY + rRtH);
                                    paint.fillRect(crossLineTip.getBackColor(), rRtL);
                                    paint.drawRect(crossLineTip.getTextColor(), 1, 0, rRtL);
                                }
                                drawText(paint, rightValue, crossLineTip.getTextColor(), crossLineTip.getFont(), new FCPoint(rRtX, rRtY));
                            }
                        }
                    }
                }
            }
            int verticalX = 0;
            if (m_crossStopIndex >= m_firstVisibleIndex && m_crossStopIndex <= m_lastVisibleIndex) {
                verticalX = (int) getX(m_crossStopIndex);
            }
            if (!m_isScrollCross) {
                verticalX = touchPoint.x;
            }
            CrossLine crossLine = div.getCrossLine();
            if (crossLine.allowUserPaint()) {
                FCRect clRect = new FCRect(0, 0, divWidth, divHeight);
                crossLine.onPaint(paint, div, clRect);
            } else {
                if (m_showCrossLine) {
                    if (mpY >= titleBarHeight && mpY <= divHeight - hScaleHeight) {
                        // 横向的线
                        paint.drawLine(crossLine.getLineColor(), 1, 0, m_leftVScaleWidth, mpY, getWidth() - m_rightVScaleWidth, mpY);
                    }
                }
                // 超出索引时
                if (m_crossStopIndex == -1 || m_crossStopIndex < m_firstVisibleIndex || m_crossStopIndex > m_lastVisibleIndex) {
                    if (m_showCrossLine) {
                        int x = touchPoint.x;
                        if (x > m_leftVScaleWidth && x < m_leftVScaleWidth + m_workingAreaWidth) {
                            paint.drawLine(crossLine.getLineColor(), 1, 0, x, titleBarHeight, x, divHeight - hScaleHeight);
                        }
                    }
                    return;
                }
                // 纵向的线
                if (m_showCrossLine) {
                    paint.drawLine(crossLine.getLineColor(), 1, 0, verticalX, titleBarHeight, verticalX, divHeight - hScaleHeight);
                }
            }
            if (m_hResizeType == 0 && m_userResizeDiv == null) {
                HScale hScale = div.getHScale();
                CrossLineTip hScaleCrossLineTip = hScale.getCrossLineTip();
                if (hScale.isVisible() && hScaleCrossLineTip.isVisible()) {
                    if (hScaleCrossLineTip.allowUserPaint()) {
                        FCRect clipRect = new FCRect(0, divHeight - hScaleHeight, divWidth, divHeight);
                        hScaleCrossLineTip.onPaint(paint, div, clipRect);
                    } else {
                        String tip = "";
                        // 获取文字
                        if (hScale.getHScaleType() == HScaleType.Date) {
                            Calendar calendar = FCStr.convertNumToDate(m_dataSource.getXValue(m_crossStopIndex));
                            SimpleDateFormat format = null;
                            if (calendar.get(Calendar.HOUR_OF_DAY) != 0) {
                                format = new SimpleDateFormat("HH:mm");
                            } else {
                                format = new SimpleDateFormat("yyyy-MM-dd");
                            }
                            tip = format.format(calendar.getTime());
                        } else if (hScale.getHScaleType() == HScaleType.Number) {
                            tip = (new Double(m_dataSource.getXValue(m_crossStopIndex))).toString();
                        }
                        FCSize xTipFontSize = paint.textSize(tip, hScaleCrossLineTip.getFont());
                        int xRtX = verticalX - xTipFontSize.cx / 2;
                        int xRtY = div.getHeight() - hScaleHeight + 2;
                        int xRtW = xTipFontSize.cx + 2;
                        int xRtH = xTipFontSize.cy;
                        if (xRtX < m_leftVScaleWidth) {
                            xRtX = m_leftVScaleWidth;
                            xRtY = divHeight - hScaleHeight + 2;
                        }
                        if (xRtX + xRtW > divWidth - m_rightVScaleWidth) {
                            xRtX = divWidth - m_rightVScaleWidth - xTipFontSize.cx - 1;
                            xRtY = divHeight - hScaleHeight + 2;
                        }
                        if (xRtW > 0 && xRtH > 0) {
                            FCRect xRtL = new FCRect(xRtX, xRtY, xRtX + xRtW, xRtY + xRtH);
                            paint.fillRect(hScaleCrossLineTip.getBackColor(), xRtL);
                            paint.drawRect(hScaleCrossLineTip.getTextColor(), 1, 0, xRtL);
                            drawText(paint, tip, hScaleCrossLineTip.getTextColor(), hScaleCrossLineTip.getFont(), new FCPoint(xRtX, xRtY));
                        }
                    }
                }
            }
        }
    }

    /**
     * 绘制层背景
     *
     * @param paint 绘图对象
     * @param div 要绘制的层
     */
    public void onPaintDivBackGround(FCPaint paint, ChartDiv div) {
        int width = div.getWidth();
        int height = div.getHeight();
        if (width < 1) {
            width = 1;
        }
        if (height < 1) {
            height = 1;
        }
        if (width > 0 && height > 0) {
            FCRect rect = new FCRect(0, 0, width, height);
            if (div.allowUserPaint()) {
                div.onPaint(paint, rect);
            } else {
                if (div.getBackColor() != FCColor.None && div.getBackColor() != getBackColor()) {
                    paint.fillRect(div.getBackColor(), rect);
                }
            }
        }
    }

    /**
     * 绘制层边框
     *
     * @param paint 绘图对象
     * @param div 要绘制的层
     */
    public void onPaintDivBorder(FCPaint paint, ChartDiv div) {
        int y = 0;
        int width = div.getWidth();
        int height = div.getHeight();
        if (width < 1) {
            width = 1;
        }
        if (height < 1) {
            height = 1;
        }
        if (width > 0 && height > 0) {
            // 获取上一个层
            ChartDiv lDiv = null;
            ArrayList<ChartDiv> divsCopy = getDivs();
            for (ChartDiv cDiv : divsCopy) {
                if (div != cDiv) {
                    lDiv = cDiv;
                } else {
                    break;
                }
            }
            // 画线
            if (lDiv != null) {
                if (!lDiv.getHScale().isVisible()) {
                    paint.drawLine(div.getHScale().getScaleColor(), 1, 0, m_leftVScaleWidth, y, width - m_rightVScaleWidth, y);
                } else {
                    paint.drawLine(div.getHScale().getScaleColor(), 1, 0, 0, y, width, y);
                }
            }
            if (div.showSelect() && div.isSelected()) {
                // 画左轴选中框
                if (m_leftVScaleWidth > 0) {
                    FCRect leftRect = new FCRect(1, 1, m_leftVScaleWidth, div.getHeight() - div.getHScale().getHeight() - 1);
                    if (leftRect.right - leftRect.left > 0 && leftRect.bottom - leftRect.top > 0) {
                        paint.drawRect(div.getLeftVScale().getScaleColor(), 1, 0, leftRect);
                    }
                }
                // 画右轴选中框
                if (m_rightVScaleWidth > 0) {
                    FCRect rightRect = new FCRect(getWidth() - m_rightVScaleWidth + 1, 1, getWidth(), div.getHeight() - div.getHScale().getHeight() - 1);
                    if (rightRect.right - rightRect.left > 0 && rightRect.bottom - rightRect.top > 0) {
                        paint.drawRect(div.getRightVScale().getScaleColor(), 1, 0, rightRect);
                    }
                }
            }
            if (div.getBorderColor() != FCColor.None) {
                if (width > 0 && height > 0) {
                    paint.drawRect(div.getBorderColor(), 1, 0, new FCRect(0, y, width, y + height));
                }
            }
        }
    }

    /**
     * 绘制横坐标轴
     *
     * @param paint 绘图对象
     * @param div 要绘制的层
     */
    public void onPaintHScale(FCPaint paint, ChartDiv div) {
        HScale hScale = div.getHScale();
        ScaleGrid vGrid = div.getVGrid();
        int width = div.getWidth(), height = div.getHeight(), hScaleHeight = hScale.getHeight();
        // 画X轴
        if ((hScale.isVisible() || vGrid.isVisible()) && height >= hScaleHeight) {
            FCRect hRect = new FCRect(0, height - hScaleHeight, width, height);
            if (hScale.allowUserPaint()) {
                hScale.onPaint(paint, div, hRect);
            }
            if (vGrid.allowUserPaint()) {
                vGrid.onPaint(paint, div, hRect);
            }
            if (hScale.allowUserPaint() && vGrid.allowUserPaint()) {
                return;
            }
            int divBottom = div.getHeight();
            if (hScale.isVisible() && !hScale.allowUserPaint()) {
                // 画底部横线
                paint.drawLine(hScale.getScaleColor(), 1, 0, 0, divBottom - hScaleHeight + 1, width, divBottom - hScaleHeight + 1);
            }
            if (m_firstVisibleIndex >= 0) {
                double xScaleWordRight = 0;
                int pureH = m_workingAreaWidth;
                // 获取自定义的刻度
                ArrayList<Double> scaleSteps = hScale.getScaleSteps();
                int scaleStepsSize = scaleSteps.size();
                HashMap<Double, Integer> scaleStepsMap = new HashMap<Double, Integer>();
                for (int i = 0; i < scaleStepsSize; i++) {
                    scaleStepsMap.put(scaleSteps.get(i), 0);
                }
                // 数值类型
                if (hScale.getHScaleType() == HScaleType.Number) {
                    // 循环遍历要显示的数值
                    for (int i = m_firstVisibleIndex; i <= m_lastVisibleIndex; i++) {
                        double xValue = m_dataSource.getXValue(i);
                        if (scaleStepsSize > 0 && !scaleStepsMap.containsKey(xValue)) {
                            continue;
                        }
                        String xValueStr = (new Double(xValue)).toString();
                        // 画X轴的刻度
                        int scaleX = (int) getX(i);
                        FCSize xSize = paint.textSize(xValueStr, hScale.getFont());
                        if (scaleStepsSize > 0 || scaleX - xSize.cx / 2 - hScale.getInterval() > xScaleWordRight) {
                            if (hScale.isVisible() && !hScale.allowUserPaint()) {
                                drawThinLine(paint, hScale.getScaleColor(), 1, scaleX, divBottom - hScaleHeight + 1, scaleX, divBottom - hScaleHeight + 4);
                                drawText(paint, xValueStr, hScale.getTextColor(), hScale.getFont(), new FCPoint(scaleX - xSize.cx / 2, divBottom - hScaleHeight + 6));
                            }
                            xScaleWordRight = scaleX + xSize.cx / 2;
                            // 画纵向的网格
                            if (vGrid.isVisible() && !vGrid.allowUserPaint()) {
                                paint.drawLine(vGrid.getGridColor(), 1, vGrid.getLineStyle(), scaleX, div.getTitleBar().getHeight(), scaleX, div.getHeight() - hScaleHeight);
                            }
                        }
                    }
                }
                // 日期类型
                else {
                    ArrayList<Integer> xList = new ArrayList<Integer>();
                    for (int i = m_firstVisibleIndex; i <= m_lastVisibleIndex; i++) {
                        if (scaleStepsSize > 0) {
                            double date = m_dataSource.getXValue(i);
                            if (scaleStepsMap.containsKey(date)) {
                                scaleStepsMap.remove(date);
                                scaleStepsSize = scaleStepsMap.size();
                                xList.add(i);
                                if (scaleStepsSize == 0) {
                                    break;
                                }
                            } else {
                                continue;
                            }
                        }
                    }
                    int interval = hScale.getInterval();
                    ArrayList<Integer> lasts = new ArrayList<Integer>();
                    for (int p = 0; p < 7; p++) {
                        int count = 0;
                        int xListSize = xList.size();
                        for (int i = 0; i < xListSize; i++) {
                            int pos = xList.get(i);
                            double date = m_dataSource.getXValue(pos);
                            DateType dateType = DateType.Day;
                            // 上次的日期
                            double lDate = 0;
                            if (pos > 0) {
                                // 获取上次的日期
                                lDate = m_dataSource.getXValue(pos - 1);
                            }
                            RefObject<DateType> tempRef_dateType = new RefObject<DateType>(dateType);
                            String xValue = getHScaleDateString(date, lDate, tempRef_dateType);
                            dateType = tempRef_dateType.argvalue;
                            int scaleX = (int) getX(pos);
                            // 年优先显示
                            if (dateType == DateType.forValue(p)) {
                                count++;
                                boolean show = true;
                                if (scaleStepsSize == 0) {
                                    // 循环遍历集合
                                    for (int index : lasts) {
                                        int getX = (int) getX(index);
                                        // 和右边比较
                                        if (index > pos) {
                                            if (m_reverseHScale) {
                                                if (getX + interval * 2 > scaleX) {
                                                    show = false;
                                                    break;
                                                }
                                            } else {
                                                if (getX - interval * 2 < scaleX) {
                                                    show = false;
                                                    break;
                                                }
                                            }
                                        }
                                        // 和左边比较
                                        else if (index < pos) {
                                            if (m_reverseHScale) {
                                                if (getX - interval * 2 < scaleX) {
                                                    show = false;
                                                    break;
                                                }
                                            } else {
                                                if (getX + interval * 2 > scaleX) {
                                                    show = false;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                if (show) {
                                    lasts.add(pos);
                                    if (hScale.isVisible() && !hScale.allowUserPaint()) {
                                        FCSize xSize = paint.textSize(xValue, hScale.getFont());
                                        drawThinLine(paint, hScale.getScaleColor(), 1, scaleX, divBottom - hScaleHeight + 1, scaleX, divBottom - hScaleHeight + 4);
                                        long dateColor = hScale.getDateColor(DateType.forValue(p));
                                        drawText(paint, xValue, dateColor, hScale.getFont(), new FCPoint(scaleX - xSize.cx / 2, divBottom - hScaleHeight + 6));
                                    }
                                    // 画纵向的网格
                                    if (vGrid.isVisible() && !vGrid.allowUserPaint()) {
                                        paint.drawLine(vGrid.getGridColor(), 1, vGrid.getLineStyle(), scaleX, div.getTitleBar().getHeight(), scaleX, div.getHeight() - hScaleHeight);
                                    }
                                    xList.remove(i);
                                    i--;
                                    xListSize--;
                                }
                            }
                        }
                        // 跳出循环
                        if (count > pureH / 40) {
                            break;
                        }
                    }
                    lasts.clear();
                }
            }
        }
        // 画标题下方的线
        if (div.getTitleBar().showUnderLine()) {
            FCSize sizeTitle = paint.textSize(" ", div.getTitleBar().getFont());
            paint.drawLine(div.getTitleBar().getUnderLineColor(), 1, 2, m_leftVScaleWidth, 5 + sizeTitle.cy, width - m_rightVScaleWidth, 5 + sizeTitle.cy);
        }
    }

    /**
     * 绘制图形的图标
     *
     * @param paint 绘图对象
     */
    public void onPaintIcon(FCPaint paint) {
        if (m_movingShape != null) {
            ChartDiv div = findDiv(m_movingShape);
            if (div != null) {
                FCPoint actualPoint = getTouchPoint();
                int x = actualPoint.x;
                int y = actualPoint.y;
                if (m_lastTouchClickPoint.x != -1 && m_lastTouchClickPoint.y != -1 && Math.abs(x - m_lastTouchClickPoint.x) > 5 && Math.abs(y - m_lastTouchClickPoint.y) > 5) {
                    FCSize sizeK = new FCSize(15, 16);
                    int rectCsX = x - sizeK.cx;
                    int rectCsY = y - sizeK.cy;
                    int rectCsH = sizeK.cy;
                    // 柱状图
                    if (m_movingShape instanceof BarShape) {
                        BarShape bs = (BarShape) ((m_movingShape instanceof BarShape) ? m_movingShape : null);
                        paint.fillRect(bs.getUpColor(), new FCRect(rectCsX + 1, rectCsY + 10, rectCsX + 4, rectCsY + rectCsH - 1));
                        paint.fillRect(bs.getUpColor(), new FCRect(rectCsX + 6, rectCsY + 3, rectCsX + 9, rectCsY + rectCsH - 1));
                        paint.fillRect(bs.getUpColor(), new FCRect(rectCsX + 11, rectCsY + 8, rectCsX + 14, rectCsY + rectCsH - 1));
                    }
                    // K线
                    else if (m_movingShape instanceof CandleShape) {
                        CandleShape cs = (CandleShape) ((m_movingShape instanceof CandleShape) ? m_movingShape : null);
                        paint.drawLine(cs.getDownColor(), 1, 0, rectCsX + 4, rectCsY + 6, rectCsX + 4, rectCsY + rectCsH - 2);
                        paint.drawLine(cs.getUpColor(), 1, 0, rectCsX + 9, rectCsY + 2, rectCsX + 9, rectCsY + rectCsH - 4);
                        paint.fillRect(cs.getDownColor(), new FCRect(rectCsX + 3, rectCsY + 8, rectCsX + 6, rectCsY + 13));
                        paint.fillRect(cs.getUpColor(), new FCRect(rectCsX + 8, rectCsY + 4, rectCsX + 11, rectCsY + 9));
                    }
                    // 线
                    else if (m_movingShape instanceof PolylineShape) {
                        PolylineShape tls = (PolylineShape) ((m_movingShape instanceof PolylineShape) ? m_movingShape : null);
                        paint.drawLine(tls.getColor(), 1, 0, rectCsX + 2, rectCsY + 5, rectCsX + 12, rectCsY + 1);
                        paint.drawLine(tls.getColor(), 1, 0, rectCsX + 2, rectCsY + 10, rectCsX + 12, rectCsY + 6);
                        paint.drawLine(tls.getColor(), 1, 0, rectCsX + 2, rectCsY + 15, rectCsX + 12, rectCsY + 11);
                    }
                }
            }
        }
    }

    /**
     * 绘制画线工具
     *
     * @param paint 绘图对象
     * @param div 要绘制的层
     */
    public void onPaintPlots(FCPaint paint, ChartDiv div) {
        ArrayList<FCPlot> plotsCopy = div.getPlots(SortType.ASC);
        if (plotsCopy != null && plotsCopy.size() > 0) {
            // 获取横向和纵向的宽度
            int wX = m_workingAreaWidth;
            int wY = div.getWorkingAreaHeight();
            if (wX > 0 && wY > 0) {
                // 裁剪
                FCRect clipRect = new FCRect();
                clipRect.left = m_leftVScaleWidth;
                clipRect.top = (div.getTitleBar().isVisible() ? div.getTitleBar().getHeight() : 0);
                clipRect.right = clipRect.left + wX;
                clipRect.bottom = clipRect.top + wY;
                // 循环遍历所有的画线工具
                for (FCPlot pl : plotsCopy) {
                    if (pl.isVisible()) {
                        paint.setClip(clipRect);
                        pl.render(paint);
                    }
                }
            }
        }
    }

    /**
     * 绘制趋势线
     *
     * @param paint 绘图对象
     * @param div 要绘制的层
     * @param ls 线条对象
     */
    public void onPaintPolyline(FCPaint paint, ChartDiv div, PolylineShape ls) {
        int x = 0, y = 0;
        ArrayList<FCPoint> points = new ArrayList<FCPoint>();
        int ciFieldName = m_dataSource.getColumnIndex(ls.getBaseField());
        int ciClr = m_dataSource.getColumnIndex(ls.getColorField());
        float defaultLineWidth = ls.getWidth();
        if (!isOperating() && m_crossStopIndex != -1) {
            if (selectPolyline(div, getTouchPoint(), ls.getBaseField(), ls.getWidth(), ls.getAttachVScale(), m_crossStopIndex)) {
                defaultLineWidth += 1;
            }
        }
        for (int i = m_firstVisibleIndex; i <= m_lastVisibleIndex; i++) {
            int scaleX = (int) getX(i);
            double value = m_dataSource.get3(i, ciFieldName);
            if (!Double.isNaN(value)) {
                int lY = (int) getY(div, value, ls.getAttachVScale());
                // 画线
                RefObject<Integer> tempRef_x = new RefObject<Integer>(x);
                RefObject<Integer> tempRef_y = new RefObject<Integer>(y);
                onPaintPolyline(paint, div, ls.getColor(), ls.getFillColor(), ciClr, defaultLineWidth, ls.getStyle(), value, ls.getAttachVScale(), scaleX, lY, i, points, tempRef_x, tempRef_y);
                x = tempRef_x.argvalue;
                y = tempRef_y.argvalue;
                if (ls.isSelected()) {
                    // 显示选中
                    int kPInterval = m_maxVisibleRecord / 30;
                    if (kPInterval < 2) {
                        kPInterval = 3;
                    }
                    if (i % kPInterval == 0) {
                        if (lY >= div.getTitleBar().getHeight() && lY <= div.getHeight() - div.getHScale().getHeight()) {
                            int lineWidth = (int) ls.getWidth();
                            int rl = scaleX - 3 - (lineWidth - 1);
                            int rt = lY - 4 - (lineWidth - 1);
                            FCRect rect = new FCRect(rl, rt, rl + 7 + (lineWidth - 1) * 2, rt + 7 + (lineWidth - 1) * 2);
                            paint.fillRect(ls.getSelectedColor(), rect);
                        }
                    }
                }
            } else {
                // 画线
                RefObject<Integer> tempRef_x2 = new RefObject<Integer>(x);
                RefObject<Integer> tempRef_y2 = new RefObject<Integer>(y);
                onPaintPolyline(paint, div, ls.getColor(), ls.getFillColor(), ciClr, defaultLineWidth, ls.getStyle(), value, ls.getAttachVScale(), scaleX, 0, i, points, tempRef_x2, tempRef_y2);
                x = tempRef_x2.argvalue;
                y = tempRef_y2.argvalue;
            }
        }
    }

    /**
     * 绘制趋势线
     *
     * @param paint 绘图对象
     * @param div 图层
     * @param lineColor 线的颜色
     * @param fillColor 填充色
     * @param ciClr 颜色字段
     * @param lineWidth 线的宽度
     * @param lineStyle 线的样式
     * @param value 点的值
     * @param attachVScale 依附坐标轴
     * @param scaleX 横坐标
     * @param lY 纵坐标
     * @param i 索引
     * @param points 点集合
     * @param x 横坐标
     * @param y 纵坐标
     */
    public void onPaintPolyline(FCPaint paint, ChartDiv div, long lineColor, long fillColor, int ciClr, float lineWidth, PolylineStyle lineStyle, double value, AttachVScale attachVScale, int scaleX, int lY, int i, ArrayList<FCPoint> points, RefObject<Integer> x, RefObject<Integer> y) {
        if (!Double.isNaN(value)) {
            if (m_dataSource.getRowsCount() == 1) {
                int cwidth = (int) (m_hScalePixel / 4);
                points.add(new FCPoint(scaleX - cwidth, lY));
                points.add(new FCPoint(scaleX - cwidth + cwidth * 2 + 1, lY));
            } else {
                int newX = 0;
                int newY = 0;
                if (i == m_firstVisibleIndex) {
                    x.argvalue = scaleX;
                    y.argvalue = lY;
                }
                newX = scaleX;
                newY = lY;
                // 限制绘图
                if (y.argvalue <= div.getHeight() - div.getHScale().getHeight() + 1 && y.argvalue >= div.getTitleBar().getHeight() - 1 && newY < div.getHeight() - div.getHScale().getHeight() + 1 && newY >= div.getTitleBar().getHeight() - 1) {
                    if (x.argvalue != newX || y.argvalue != newY) {
                        if (points.isEmpty()) {
                            points.add(new FCPoint(x.argvalue, y.argvalue));
                            points.add(new FCPoint(newX, newY));
                        } else {
                            points.add(new FCPoint(newX, newY));
                        }
                    }
                }
                x.argvalue = newX;
                y.argvalue = newY;
            }
            if (ciClr != -1) {
                double defineColor = m_dataSource.get3(i, ciClr);
                if (!Double.isNaN(defineColor)) {
                    lineColor = (long) defineColor;
                }
            }
        }
        if (points.size() > 0) {
            // 获取上次线的颜色
            long lColor = lineColor;
            if (i > 0) {
                // 获取上一行的颜色
                if (ciClr != -1) {
                    double defineColor = m_dataSource.get3(i - 1, ciClr);
                    if (!Double.isNaN(defineColor)) {
                        lColor = (long) defineColor;
                    }
                }
            }
            // 绘制线条
            if (lineColor != lColor || i == m_lastVisibleIndex) {
                long drawColor = lineColor;
                int width = (int) (m_hScalePixel / 2);
                if (lColor != lineColor) {
                    drawColor = lColor;
                }
                switch (lineStyle) {
                    // 圆
                    case Cycle:
                        int ew = (width - 1) > 0 ? (width - 1) : 1;
                        int pointsSize = points.size();
                        for (int j = 0; j < pointsSize; j++) {
                            FCPoint FCPoint = points.get(j);
                            FCRect pRect = new FCRect(FCPoint.x - ew / 2, FCPoint.y - ew / 2, FCPoint.x + ew / 2, FCPoint.y + ew / 2);
                            paint.drawEllipse(drawColor, lineWidth, 0, pRect);
                        }
                        break;
                    case DashLine: {
                        paint.drawPolyline(drawColor, lineWidth, 1, points.toArray(new FCPoint[]{}));
                        break;
                    }
                    // 点线
                    case DotLine: {
                        paint.drawPolyline(drawColor, lineWidth, 2, points.toArray(new FCPoint[]{}));
                        break;
                    }
                    // 实线
                    case SolidLine: {
                        paint.drawPolyline(drawColor, lineWidth, 0, points.toArray(new FCPoint[]{}));
                        break;
                    }
                }
                if (fillColor != FCColor.None) {
                    int zy = (int) getY(div, 0, attachVScale);
                    int th = div.getTitleBar().isVisible() ? div.getTitleBar().getHeight() : 0;
                    int hh = div.getHScale().isVisible() ? div.getHScale().getHeight() : 0;
                    if (zy < th) {
                        zy = th;
                    } else if (zy > div.getHeight() - hh) {
                        zy = div.getHeight() - hh;
                    }
                    points.add(0, new FCPoint(points.get(0).x, zy));
                    points.add(new FCPoint(points.get(points.size() - 1).x, zy));
                    paint.fillPolygon(fillColor, points.toArray(new FCPoint[]{}));
                }
                points.clear();
            }
        }
    }

    /**
     * 绘制拖动的边线
     *
     * @param paint 绘图对象
     */
    public void onPaintResizeLine(FCPaint paint) {
        // 画横向拖动线
        if (m_hResizeType > 0) {
            FCPoint mp = getTouchPoint();
            ArrayList<ChartDiv> divsCopy = getDivs();
            for (ChartDiv div : divsCopy) {
                if (mp.x < 0) {
                    mp.x = 0;
                }
                if (mp.x > getWidth()) {
                    mp.x = getWidth();
                }
                paint.drawLine(FCColor.reverse(paint, div.getBackColor()), 1, 2, mp.x, 0, mp.x, getWidth());
            }
        }
        // 画垂直拖动线
        if (m_userResizeDiv != null) {
            FCPoint mp = getTouchPoint();
            ChartDiv nextCP = null;
            boolean rightP = false;
            //循环遍历所有的层
            ArrayList<ChartDiv> divsCopy = getDivs();
            for (ChartDiv cDiv : divsCopy) {
                if (rightP) {
                    nextCP = cDiv;
                    break;
                }
                if (cDiv == m_userResizeDiv) {
                    rightP = true;
                }
            }
            FCRect uRect = m_userResizeDiv.getBounds();
            boolean drawFlag = false;
            // 画拖动阴影
            if (mp.x >= uRect.left && mp.x <= uRect.right && mp.y >= uRect.top && mp.y <= uRect.bottom) {
                drawFlag = true;
            } else {
                if (nextCP != null) {
                    FCRect nRect = nextCP.getBounds();
                    if (mp.x >= nRect.left && mp.x <= nRect.right && mp.y >= nRect.top && mp.y <= nRect.bottom) {
                        drawFlag = true;
                    }
                }
            }
            // 画线
            if (drawFlag) {
                paint.drawLine(FCColor.reverse(paint, m_userResizeDiv.getBackColor()), 1, 2, 0, mp.y, getWidth(), mp.y);
            }
        }
    }

    /**
     * 绘制选中块
     *
     * @param paint 绘图对象
     * @param div 要绘制的层
     */
    public void onPaintSelectArea(FCPaint paint, ChartDiv div) {
        SelectArea selectArea = div.getSelectArea();
        // 判断是否显示板块
        if (selectArea.isVisible()) {
            FCRect bounds = selectArea.getBounds();
            if (selectArea.allowUserPaint()) {
                selectArea.onPaint(paint, div, bounds);
            } else {
                // 系统绘图
                if (bounds.right - bounds.left >= 5 && bounds.bottom - bounds.top >= 5) {
                    // 画选中边框
                    paint.drawRect(selectArea.getLineColor(), 1, 0, bounds);
                    paint.fillRect(selectArea.getBackColor(), bounds);
                }
            }
        }
    }

    /**
     * 绘制K线，成交量，趋势线等等
     *
     * @param paint 绘图对象
     * @param div 要绘制的层
     */
    public void onPaintShapes(FCPaint paint, ChartDiv div) {
        // 排序后的线条
        ArrayList<BaseShape> sortedBs = div.getShapes(SortType.ASC);
        for (BaseShape bShape : sortedBs) {
            if (!bShape.isVisible() || (div.getVScale(bShape.getAttachVScale()).getVisibleMax() - div.getVScale(bShape.getAttachVScale()).getVisibleMin()) == 0) {
                continue;
            }
            if (bShape.allowUserPaint()) {
                FCRect rect = new FCRect(0, 0, div.getWidth(), div.getHeight());
                bShape.onPaint(paint, div, rect);
            } else {
                BarShape bs = (BarShape) ((bShape instanceof BarShape) ? bShape : null);
                CandleShape cs = (CandleShape) ((bShape instanceof CandleShape) ? bShape : null);
                PolylineShape ls = (PolylineShape) ((bShape instanceof PolylineShape) ? bShape : null);
                TextShape ts = (TextShape) ((bShape instanceof TextShape) ? bShape : null);
                // 线条
                if (ls != null) {
                    onPaintPolyline(paint, div, ls);
                }
                // 文字
                else if (ts != null) {
                    onPaintText(paint, div, ts);
                }
                // 柱状图
                else if (bs != null) {
                    onPaintBar(paint, div, bs);
                }
                // 其他
                else if (cs != null) {
                    onPaintCandle(paint, div, cs);
                }
            }
        }
    }

    /**
     * 绘制文字
     *
     * @param paint 绘图对象
     * @param div 图层
     * @param ts 文字
     */
    public void onPaintText(FCPaint paint, ChartDiv div, TextShape ts) {
        String drawText = ts.getText();
        if (drawText == null || drawText.length() == 0) {
            return;
        }
        int ciFieldName = m_dataSource.getColumnIndex(ts.getFieldName());
        int ciStyle = m_dataSource.getColumnIndex(ts.getStyleField());
        int ciClr = m_dataSource.getColumnIndex(ts.getColorField());
        for (int i = m_firstVisibleIndex; i <= m_lastVisibleIndex; i++) {
            int style = 0;
            // 自定义样式
            if (ciStyle != -1) {
                double defineStyle = m_dataSource.get3(i, ciStyle);
                if (!Double.isNaN(defineStyle)) {
                    style = (int) defineStyle;
                }
            }
            if (style == -10000) {
                continue;
            }
            double value = m_dataSource.get3(i, ciFieldName);
            if (!Double.isNaN(value)) {
                int scaleX = (int) getX(i);
                int y = (int) getY(div, value, ts.getAttachVScale());
                FCSize tSize = paint.textSize(drawText, ts.getFont());
                FCRect tRect = new FCRect(scaleX - tSize.cx / 2, y - tSize.cy / 2, scaleX + tSize.cx / 2, y + tSize.cy / 2);
                long textColor = ts.getTextColor();
                if (ts.getColorField() != FCDataTable.NULLFIELD) {
                    double defineColor = m_dataSource.get3(i, ciClr);
                    if (!Double.isNaN(defineColor)) {
                        textColor = (long) defineColor;
                    }
                }
                // 绘制文字
                drawText(paint, drawText, textColor, ts.getFont(), new FCPoint(tRect.left, tRect.top));
            }
        }
    }

    /**
     * 绘制标题
     *
     * @param paint 绘图对象
     * @param div 要绘制的层
     */
    public void onPaintTitle(FCPaint paint, ChartDiv div) {
        ChartTitleBar titleBar = div.getTitleBar();
        int width = div.getWidth(), height = div.getHeight();
        if (titleBar.isVisible()) {
            FCRect tbRect = new FCRect(0, 0, width, height);
            if (titleBar.allowUserPaint()) {
                titleBar.onPaint(paint, div, tbRect);
            } else {
                int titleLeftPadding = m_leftVScaleWidth;
                // 创建字符串
                int rightPadding = width - m_rightVScaleWidth - 2;
                // 画主标题
                FCSize divNameSize = paint.textSize(div.getTitleBar().getText(), div.getTitleBar().getFont());
                if (titleLeftPadding + divNameSize.cx <= getWidth() - m_rightVScaleWidth) {
                    drawText(paint, titleBar.getText(), titleBar.getTextColor(), titleBar.getFont(), new FCPoint(titleLeftPadding, 2));
                }
                titleLeftPadding += divNameSize.cx + 2;
                // 画各个线条的标题
                if (m_firstVisibleIndex >= 0 && m_lastVisibleIndex >= 0) {
                    int displayIndex = m_lastVisibleIndex;
                    if (displayIndex > m_lastUnEmptyIndex) {
                        displayIndex = m_lastUnEmptyIndex;
                    }
                    if (m_showCrossLine) {
                        if (m_crossStopIndex <= m_lastVisibleIndex) {
                            displayIndex = m_crossStopIndex;
                        }
                        if (m_crossStopIndex < 0) {
                            displayIndex = 0;
                        }
                        if (m_crossStopIndex >= m_lastVisibleIndex) {
                            displayIndex = m_lastVisibleIndex;
                        }
                    }
                    int curLength = 1;
                    int tTop = 2;
                    ArrayList<ChartTitle> titles = div.getTitleBar().getTitles();
                    // 循环画标题
                    int titleSize = titles.size();
                    for (int i = 0; i < titleSize; i++) {
                        ChartTitle title = titles.get(i);
                        if (!title.isVisible() || title.getFieldTextMode() == TextMode.None) {
                            continue;
                        }
                        double value = m_dataSource.get2(displayIndex, title.getFieldName());
                        if (Double.isNaN(value)) {
                            value = 0;
                        }
                        String showTitle = "";
                        if (title.getFieldTextMode() != TextMode.Value) {
                            showTitle = title.getFieldText() + title.getFieldTextSeparator();
                        }
                        if (title.getFieldTextMode() != TextMode.Field) {
                            int digit = title.getDigit();
                            showTitle += FCStr.getValueByDigit(value, digit);
                        }
                        FCSize conditionSize = paint.textSize(showTitle, div.getTitleBar().getFont());
                        if (titleLeftPadding + conditionSize.cx + 8 > rightPadding) {
                            curLength++;
                            if (curLength <= div.getTitleBar().getMaxLine()) {
                                tTop += conditionSize.cy + 2;
                                titleLeftPadding = m_leftVScaleWidth;
                                rightPadding = getWidth() - m_rightVScaleWidth;
                            } else {
                                break;
                            }
                            if (tTop + conditionSize.cy >= div.getHeight() - div.getHScale().getHeight()) {
                                break;
                            }
                        }
                        if (titleLeftPadding <= rightPadding) {
                            drawText(paint, showTitle, title.getTextColor(), titleBar.getFont(), new FCPoint(titleLeftPadding, tTop));
                            titleLeftPadding += conditionSize.cx + 8;
                        }
                    }
                }
            }
        }
    }

    /**
     * 绘制提示框
     *
     * @param paint 绘图对象
     */
    public void onPaintToolTip(FCPaint paint) {
        if (!m_showingToolTip) {
            return;
        }
        BaseShape bs = selectShape(getTouchOverIndex(), 0);
        if (bs != null) {
            FCPoint touchP = getTouchPoint();
            // 获取触摸位置面板的digit值
            ChartDiv touchOverDiv = getTouchOverDiv();
            int digit = touchOverDiv.getVScale(bs.getAttachVScale()).getDigit();
            if (touchOverDiv == null) {
                return;
            }
            int index = getIndex(touchP);
            if (index == -1) {
                return;
            }
            ChartToolTip toolTip = touchOverDiv.getToolTip();
            if (toolTip.allowUserPaint()) {
                FCRect toolRect = new FCRect(0, 0, getWidth(), getHeight());
                toolTip.onPaint(paint, touchOverDiv, toolRect);
                return;
            }
            int pWidth = 0;
            int pHeight = 0;
            StringBuilder sbValue = new StringBuilder();
            FCFont toolTipFont = toolTip.getFont();
            double xValue = m_dataSource.getXValue(index);
            int sLeft = touchOverDiv.getLeft(), sTop = touchOverDiv.getTop();
            for (int t = 0; t < 2; t++) {
                int x = touchP.x + 10;
                int y = touchP.y + 2;
                if (t == 0) {
                    sLeft = x + 2;
                    sTop = y;
                }
                FCSize xValueSize = new FCSize();
                if (touchOverDiv.getHScale().getHScaleType() == HScaleType.Date) {
                    int tm_year = 0;
                    int tm_mon = 0;
                    int tm_mday = 0;
                    int tm_hour = 0;
                    int tm_min = 0;
                    int tm_sec = 0;
                    int tm_msec = 0;
                    RefObject<Integer> tempRef_tm_year = new RefObject<Integer>(tm_year);
                    RefObject<Integer> tempRef_tm_mon = new RefObject<Integer>(tm_mon);
                    RefObject<Integer> tempRef_tm_mday = new RefObject<Integer>(tm_mday);
                    RefObject<Integer> tempRef_tm_hour = new RefObject<Integer>(tm_hour);
                    RefObject<Integer> tempRef_tm_min = new RefObject<Integer>(tm_min);
                    RefObject<Integer> tempRef_tm_sec = new RefObject<Integer>(tm_sec);
                    RefObject<Integer> tempRef_tm_msec = new RefObject<Integer>(tm_msec);
                    FCStr.getDataByNum(xValue, tempRef_tm_year, tempRef_tm_mon, tempRef_tm_mday, tempRef_tm_hour, tempRef_tm_min, tempRef_tm_sec, tempRef_tm_msec);
                    tm_year = tempRef_tm_year.argvalue;
                    tm_mon = tempRef_tm_mon.argvalue;
                    tm_mday = tempRef_tm_mday.argvalue;
                    tm_hour = tempRef_tm_hour.argvalue;
                    tm_min = tempRef_tm_min.argvalue;
                    tm_sec = tempRef_tm_sec.argvalue;
                    tm_msec = tempRef_tm_msec.argvalue;
                    SimpleDateFormat format = new SimpleDateFormat("yyyy-MM-dd");
                    String formatDate = m_hScaleFieldText + ":" + format.format(new Date(tm_year, tm_mon, tm_mday, tm_hour, tm_min, tm_sec));
                    xValueSize = paint.textSize(formatDate, toolTipFont);
                    pHeight = xValueSize.cy;
                    if (t == 1) {
                        drawText(paint, formatDate, toolTip.getTextColor(), toolTipFont, new FCPoint(sLeft, sTop));
                    }
                } else if (touchOverDiv.getHScale().getHScaleType() == HScaleType.Number) {
                    String xValueStr = m_hScaleFieldText + ":" + (new Double(xValue)).toString();
                    xValueSize = paint.textSize(xValueStr, toolTipFont);
                    pHeight = xValueSize.cy;
                    if (t == 1) {
                        drawText(paint, xValueStr, toolTip.getTextColor(), toolTipFont, new FCPoint(sLeft, sTop));
                    }
                }
                sTop += xValueSize.cy;
                int[] fields = bs.getFields();
                int fieldsLength = fields.length;
                if (fieldsLength > 0) {
                    for (int i = 0; i < fieldsLength; i++) {
                        String fieldText = bs.getFieldText(fields[i]);
                        double value = 0;
                        if (index >= 0) {
                            value = m_dataSource.get2(index, fields[i]);
                        }
                        String valueDigit = fieldText + ":" + FCStr.getValueByDigit(value, digit);
                        if (t == 1) {
                            drawText(paint, valueDigit, toolTip.getTextColor(), toolTipFont, new FCPoint(sLeft, sTop));
                        }
                        FCSize strSize = paint.textSize(valueDigit, toolTipFont);
                        if (i == 0) {
                            pWidth = xValueSize.cx;
                        }
                        if (xValueSize.cx > pWidth) {
                            pWidth = xValueSize.cx;
                        }
                        if (strSize.cx > pWidth) {
                            pWidth = strSize.cx;
                        }
                        sTop += strSize.cy;
                        pHeight += strSize.cy;
                    }
                }
                if (t == 0) {
                    int width = getWidth(), height = getHeight();
                    pWidth += 4;
                    pHeight += 1;
                    if (x + pWidth > width) {
                        x = width - pWidth;
                        if (x < 0) {
                            x = 0;
                        }
                    }
                    if (y + pHeight > height) {
                        y = height - pHeight;
                        if (y < 0) {
                            y = 0;
                        }
                    }
                    sLeft = x;
                    sTop = y;
                    int rectPX = x, rectPY = y, rectPW = pWidth, rectPH = pHeight;
                    if (rectPW > 0 && rectPH > 0) {
                        FCRect rectP = new FCRect(rectPX, rectPY, rectPX + rectPW, rectPY + rectPH);
                        paint.fillRect(toolTip.getBackColor(), rectP);
                        paint.drawRect(toolTip.getBorderColor(), 1, 0, rectP);
                    }
                }
            }
        }
    }

    /**
     * 绘制纵坐标轴
     *
     * @param paint 绘图对象
     * @param div 要绘制的层
     */
    public void onPaintVScale(FCPaint paint, ChartDiv div) {
        int divBottom = div.getHeight();
        ArrayList<Integer> gridYList = new ArrayList<Integer>();
        boolean leftGridIsShow = false;
        int width = getWidth();
        if (m_leftVScaleWidth > 0) {
            VScale leftVScale = div.getLeftVScale();
            ScaleGrid hGrid = div.getHGrid();
            boolean paintV = true, paintG = true;
            if (leftVScale.allowUserPaint()) {
                FCRect leftVRect = new FCRect(0, 0, m_leftVScaleWidth, divBottom);
                leftVScale.onPaint(paint, div, leftVRect);
                paintV = false;
            }
            if (hGrid.allowUserPaint()) {
                FCRect hGridRect = new FCRect(0, 0, width, divBottom);
                hGrid.onPaint(paint, div, hGridRect);
                paintG = false;
            }
            if (paintV || paintG) {
                if (m_leftVScaleWidth <= width && paintV) {
                    paint.drawLine(leftVScale.getScaleColor(), 1, 0, m_leftVScaleWidth, 0, m_leftVScaleWidth, divBottom - div.getHScale().getHeight());
                }
                FCFont leftYFont = leftVScale.getFont();
                FCSize leftYSize = paint.textSize(" ", leftYFont);
                // 获取最小值和最大值
                double min = leftVScale.getVisibleMin();
                double max = leftVScale.getVisibleMax();
                if (min == 0 && max == 0) {
                    VScale rightVScale = div.getRightVScale();
                    if (rightVScale.getVisibleMin() != 0 || rightVScale.getVisibleMax() != 0) {
                        min = rightVScale.getVisibleMin();
                        max = rightVScale.getVisibleMax();
                        FCPoint point1 = new FCPoint(0, div.getTop() + div.getTitleBar().getHeight());
                        double value1 = getNumberValue(div, point1, AttachVScale.Right);
                        FCPoint point2 = new FCPoint(0, div.getBottom() - div.getHScale().getHeight());
                        double value2 = getNumberValue(div, point2, AttachVScale.Right);
                        max = Math.max(value1, value2);
                        min = Math.min(value1, value2);
                    }
                } else {
                    FCPoint point1 = new FCPoint(0, div.getTop() + div.getTitleBar().getHeight());
                    double value1 = getNumberValue(div, point1, AttachVScale.Left);
                    FCPoint point2 = new FCPoint(0, div.getBottom() - div.getHScale().getHeight());
                    double value2 = getNumberValue(div, point2, AttachVScale.Left);
                    max = Math.max(value1, value2);
                    min = Math.min(value1, value2);
                }
                ArrayList<Double> scaleStepList = leftVScale.getScaleSteps();
                if (scaleStepList.isEmpty()) {
                    scaleStepList = getVScaleStep(max, min, div, leftVScale);
                }
                // 循环遍历所有的值
                float lY = -1;
                int stepSize = scaleStepList.size();
                for (int i = 0; i < stepSize; i++) {
                    double lValue = scaleStepList.get(i) / leftVScale.getMagnitude();
                    if (lValue != 0 && leftVScale.getType() == VScaleType.Percent) {
                        double baseValue = getVScaleBaseValue(div, leftVScale, m_firstVisibleIndex) / leftVScale.getMagnitude();
                        // 计算百分比
                        lValue = 100 * (lValue - baseValue * leftVScale.getMagnitude()) / lValue;
                    }
                    String number = FCStr.getValueByDigit(lValue, leftVScale.getDigit());
                    if (div.getLeftVScale().getType() == VScaleType.Percent) {
                        number += "%";
                    }
                    int y = (int) getY(div, scaleStepList.get(i), AttachVScale.Left);
                    leftYSize = paint.textSize(number, leftYFont);
                    if (y - leftYSize.cy / 2 < 0 || y + leftYSize.cy / 2 > divBottom) {
                        continue;
                    }
                    // 网格线
                    if (hGrid.isVisible() && paintG) {
                        leftGridIsShow = true;
                        if (!gridYList.contains(y)) {
                            gridYList.add(y);
                            paint.drawLine(hGrid.getGridColor(), 1, hGrid.getLineStyle(), m_leftVScaleWidth, y, width - m_rightVScaleWidth, y);
                        }
                    }
                    if (paintV) {
                        // 画短线
                        drawThinLine(paint, leftVScale.getScaleColor(), 1, m_leftVScaleWidth - 4, y, m_leftVScaleWidth, y);
                        // 反转坐标
                        if (leftVScale.isReverse()) {
                            if (lY != -1 && y - leftYSize.cy / 2 < lY) {
                                continue;
                            }
                            lY = y + leftYSize.cy / 2;
                        } else {
                            if (lY != -1 && y + leftYSize.cy / 2 > lY) {
                                continue;
                            }
                            lY = y - leftYSize.cy / 2;
                        }
                        long scaleTextColor = leftVScale.getTextColor();
                        long scaleTextColor2 = leftVScale.getTextColor2();
                        if (leftVScale.getType() == VScaleType.Percent) {
                            if (scaleTextColor2 != FCColor.None && lValue < 0) {
                                scaleTextColor = scaleTextColor2;
                            } else {
                                if (scaleTextColor2 != FCColor.None && scaleStepList.get(i) < leftVScale.getMidValue()) {
                                    scaleTextColor = scaleTextColor2;
                                }
                            }
                        }
                        if (leftVScale.getType() != VScaleType.Percent && leftVScale.getNumberStyle() == NumberStyle.UnderLine) {
                            String[] nbs = number.split("[.]");
                            if (nbs[0].length() > 0) {
                                if (nbs.length >= 1) {
                                    drawText(paint, nbs[0], scaleTextColor, leftYFont, new FCPoint(m_leftVScaleWidth - 10 - leftYSize.cx, y - leftYSize.cy / 2));
                                }
                                if (nbs.length >= 2) {
                                    FCSize decimalSize = paint.textSize(nbs[0], leftYFont);
                                    FCSize size2 = paint.textSize(nbs[1], leftYFont);
                                    drawText(paint, nbs[1], scaleTextColor, leftYFont, new FCPoint(m_leftVScaleWidth - 10 - leftYSize.cx + decimalSize.cx, y - leftYSize.cy / 2));
                                    drawThinLine(paint, scaleTextColor, 1, m_leftVScaleWidth - 10 - leftYSize.cx + decimalSize.cx, y + leftYSize.cy / 2, m_leftVScaleWidth - 10 - leftYSize.cx + decimalSize.cx + size2.cx, y + leftYSize.cy / 2);
                                }
                            }
                        } else {
                            drawText(paint, number, scaleTextColor, leftYFont, new FCPoint(m_leftVScaleWidth - 10 - leftYSize.cx, y - leftYSize.cy / 2));
                        }
                        // 黄金分割
                        if (leftVScale.getType() == VScaleType.GoldenRatio) {
                            String goldenRatio = "";
                            if (i == 1) {
                                goldenRatio = "19.1%";
                            } else if (i == 2) {
                                goldenRatio = "38.2%";
                            } else if (i == 4) {
                                goldenRatio = "61.8%";
                            } else if (i == 5) {
                                goldenRatio = "80.9%";
                            }
                            if (goldenRatio != null && goldenRatio.length() > 0) {
                                FCSize goldenRatioSize = paint.textSize(goldenRatio, leftYFont);
                                drawText(paint, goldenRatio, scaleTextColor, leftYFont, new FCPoint(m_leftVScaleWidth - 10 - goldenRatioSize.cx, y + leftYSize.cy / 2));
                            }
                        }
                    }
                }
                if (div.getLeftVScale().getMagnitude() != 1 && paintV) {
                    // 获取字符大小
                    String str = "X" + (new Integer(leftVScale.getMagnitude())).toString();
                    FCSize sizeF = paint.textSize(str, leftYFont);
                    // 计算xy
                    int x = m_leftVScaleWidth - sizeF.cx - 6;
                    int y = div.getHeight() - div.getHScale().getHeight() - sizeF.cy - 2;
                    // 画矩形框
                    paint.drawRect(leftVScale.getScaleColor(), 1, 0, new FCRect(x - 1, y - 1, x + sizeF.cx + 1, y + sizeF.cy));
                    // 画文字
                    drawText(paint, str, leftVScale.getTextColor(), leftYFont, new FCPoint(x, y));
                }
            }
        }
        // 画右侧Y轴
        if (m_rightVScaleWidth > 0) {
            VScale rightVScale = div.getRightVScale();
            ScaleGrid hGrid = div.getHGrid();
            boolean paintV = true, paintG = true;
            if (rightVScale.allowUserPaint()) {
                FCRect rightVRect = new FCRect(width - m_rightVScaleWidth, 0, width, divBottom);
                rightVScale.onPaint(paint, div, rightVRect);
                paintV = false;
            }
            if (hGrid.allowUserPaint()) {
                FCRect hGridRect = new FCRect(0, 0, width, divBottom);
                hGrid.onPaint(paint, div, hGridRect);
                paintG = false;
            }
            if (paintV || paintG) {
                if (width - m_rightVScaleWidth >= m_leftVScaleWidth && paintV) {
                    paint.drawLine(rightVScale.getScaleColor(), 1, 0, width - m_rightVScaleWidth, 0, width - m_rightVScaleWidth, divBottom - div.getHScale().getHeight());
                }
                FCFont rightYFont = rightVScale.getFont();
                FCSize rightYSize = paint.textSize(" ", rightYFont);
                // 获取最小值和最大值
                double min = rightVScale.getVisibleMin();
                double max = rightVScale.getVisibleMax();
                if (min == 0 && max == 0) {
                    VScale leftVScale = div.getLeftVScale();
                    if (leftVScale.getVisibleMin() != 0 || leftVScale.getVisibleMax() != 0) {
                        min = leftVScale.getVisibleMin();
                        max = leftVScale.getVisibleMax();
                        FCPoint point1 = new FCPoint(0, div.getTop() + div.getTitleBar().getHeight());
                        double value1 = getNumberValue(div, point1, AttachVScale.Left);
                        FCPoint point2 = new FCPoint(0, div.getBottom() - div.getHScale().getHeight());
                        double value2 = getNumberValue(div, point2, AttachVScale.Left);
                        max = Math.max(value1, value2);
                        min = Math.min(value1, value2);
                    }
                } else {
                    FCPoint point1 = new FCPoint(0, div.getTop() + div.getTitleBar().getHeight());
                    double value1 = getNumberValue(div, point1, AttachVScale.Right);
                    FCPoint point2 = new FCPoint(0, div.getBottom() - div.getHScale().getHeight());
                    double value2 = getNumberValue(div, point2, AttachVScale.Right);
                    max = Math.max(value1, value2);
                    min = Math.min(value1, value2);
                }
                // 计算显示值
                ArrayList<Double> scaleStepList = rightVScale.getScaleSteps();
                if (scaleStepList.isEmpty()) {
                    scaleStepList = getVScaleStep(max, min, div, rightVScale);
                }
                // 循环遍历所有的值
                float lY = -1;
                int stepSize = scaleStepList.size();
                for (int i = 0; i < stepSize; i++) {
                    double rValue = scaleStepList.get(i) / rightVScale.getMagnitude();
                    // 计算百分比
                    if (rValue != 0 && rightVScale.getType() == VScaleType.Percent) {
                        double baseValue = getVScaleBaseValue(div, rightVScale, m_lastVisibleIndex) / rightVScale.getMagnitude();
                        rValue = 100 * (rValue - baseValue * rightVScale.getMagnitude()) / rValue;
                    }
                    String number = FCStr.getValueByDigit(rValue, rightVScale.getDigit());
                    if (rightVScale.getType() == VScaleType.Percent) {
                        number += "%";
                    }
                    int y = (int) getY(div, scaleStepList.get(i), AttachVScale.Right);
                    rightYSize = paint.textSize(number, rightYFont);
                    if (y - rightYSize.cy / 2 < 0 || y + rightYSize.cy / 2 > divBottom) {
                        continue;
                    }
                    // 网格
                    if (hGrid.isVisible() && paintG && !leftGridIsShow) {
                        if (!gridYList.contains(y)) {
                            gridYList.add(y);
                            paint.drawLine(hGrid.getGridColor(), 1, hGrid.getLineStyle(), m_leftVScaleWidth, y, width - m_rightVScaleWidth, y);
                        }
                    }
                    if (paintV) {
                        // 画短线
                        drawThinLine(paint, rightVScale.getScaleColor(), 1, width - m_rightVScaleWidth, y, width - m_rightVScaleWidth + 4, y);
                        // 反转坐标
                        if (rightVScale.isReverse()) {
                            if (lY != -1 && y - rightYSize.cy / 2 < lY) {
                                continue;
                            }
                            lY = y + rightYSize.cy / 2;
                        } else {
                            if (lY != -1 && y + rightYSize.cy / 2 > lY) {
                                continue;
                            }
                            lY = y - rightYSize.cy / 2;
                        }
                        long scaleTextColor = rightVScale.getTextColor();
                        long scaleTextColor2 = rightVScale.getTextColor2();
                        if (rightVScale.getType() == VScaleType.Percent) {
                            if (scaleTextColor2 != FCColor.None && rValue < 0) {
                                scaleTextColor = scaleTextColor2;
                            } else {
                                if (scaleTextColor2 != FCColor.None && scaleStepList.get(i) < rightVScale.getMidValue()) {
                                    scaleTextColor = scaleTextColor2;
                                }
                            }
                        }
                        if (rightVScale.getType() != VScaleType.Percent && rightVScale.getNumberStyle() == NumberStyle.UnderLine) {
                            String[] nbs = number.split("[.]");
                            if (nbs[0].length() > 0) {
                                if (nbs.length >= 1) {
                                    drawText(paint, nbs[0], scaleTextColor, rightYFont, new FCPoint(width - m_rightVScaleWidth + 10, y - rightYSize.cy / 2));
                                }
                                if (nbs.length >= 2) {
                                    FCSize decimalSize = paint.textSize(nbs[0], rightYFont);
                                    FCSize size2 = paint.textSize(nbs[1], rightYFont);
                                    drawText(paint, nbs[1], scaleTextColor, rightYFont, new FCPoint(width - m_rightVScaleWidth + 10 + decimalSize.cx, y - rightYSize.cy / 2));
                                    drawThinLine(paint, scaleTextColor, 1, width - m_rightVScaleWidth + 10 + decimalSize.cx, y + rightYSize.cy / 2, getWidth() - m_rightVScaleWidth + 10 + decimalSize.cx + size2.cx, y + rightYSize.cy / 2);
                                }
                            }
                        } else {
                            drawText(paint, number, scaleTextColor, rightYFont, new FCPoint(width - m_rightVScaleWidth + 10, y - rightYSize.cy / 2));
                        }
                        // 黄金分割
                        if (rightVScale.getType() == VScaleType.GoldenRatio) {
                            String goldenRatio = "";
                            if (i == 1) {
                                goldenRatio = "19.1%";
                            } else if (i == 2) {
                                goldenRatio = "38.2%";
                            } else if (i == 4) {
                                goldenRatio = "61.8%";
                            } else if (i == 5) {
                                goldenRatio = "80.9%";
                            }
                            if (goldenRatio != null && goldenRatio.length() > 0) {
                                drawText(paint, goldenRatio, scaleTextColor, rightYFont, new FCPoint(width - m_rightVScaleWidth + 10, y + rightYSize.cy / 2));
                            }
                        }
                    }
                }
                if (rightVScale.getMagnitude() != 1 && paintV) {
                    // 获取字符大小
                    String str = "X" + (new Integer(rightVScale.getMagnitude())).toString();
                    FCSize sizeF = paint.textSize(str, rightYFont);
                    // 计算xy
                    int x = width - m_rightVScaleWidth + 6;
                    int y = div.getHeight() - div.getHScale().getHeight() - sizeF.cy - 2;
                    // 画矩形框
                    paint.drawRect(rightVScale.getScaleColor(), 1, 0, new FCRect(x - 1, y - 1, x + sizeF.cx + 1, y + sizeF.cy));
                    // 画文字
                    drawText(paint, str, rightVScale.getTextColor(), rightYFont, new FCPoint(x, y));
                }
            }
        }
    }

    /**
     * 控件添加方法
     */
    @Override
    public void onLoad() {
        super.onLoad();
        startTimer(m_timerID, 10);
        if (m_dataSource == null) {
            m_dataSource = new FCDataTable();
        }
    }

    /**
     * 触摸离开的方法
     *
     * @param touchInfo 触摸信息
     */
    @Override
    public void onTouchDown(FCTouchInfo touchInfo) {
        super.onTouchDown(touchInfo.clone());
        if (touchInfo.m_firstTouch && touchInfo.m_clicks == 2) {
            clearSelectedShape();
            m_showCrossLine = !m_showCrossLine;
            invalidate();
            return;
        }
        FCPoint mp = touchInfo.m_firstPoint.clone();
        int width = getWidth();
        m_userResizeDiv = null;
        int shapeCount = getSelectedShape() == null ? 0 : 1;
        ArrayList<ChartDiv> divsCopy = getDivs();
        m_hResizeType = 0;
        if (touchInfo.m_firstTouch) {
            clearSelectedPlot();
            // 选中层
            ChartDiv touchOverDiv = getTouchOverDiv();
            for (ChartDiv div : divsCopy) {
                if (div == touchOverDiv) {
                    div.setSelected(true);
                } else {
                    div.setSelected(false);
                }
            }
            if (touchInfo.m_clicks == 1) {
                closeSelectArea();
                m_crossStopIndex = getTouchOverIndex();
                m_cross_y = mp.y;
                // 设置十字线的位置
                if (m_showCrossLine && m_crossLineMoveMode == CrossLineMoveMode.AfterClick) {
                    m_crossStopIndex = getTouchOverIndex();
                    m_cross_y = getTouchPoint().y;
                    m_isScrollCross = false;
                }
                boolean outLoop = false;
                // 横向的拖动
                if (m_canResizeH) {
                    if (mp.x >= m_leftVScaleWidth - 4 && mp.x <= m_leftVScaleWidth + 4) {
                        m_hResizeType = 1;
                        outLoop = true;
                    } else if (mp.x >= getWidth() - m_rightVScaleWidth - 4 && mp.x <= getWidth() - m_rightVScaleWidth + 4) {
                        m_hResizeType = 2;
                        outLoop = true;
                    }
                }
                // 纵向的拖动
                if (!outLoop && m_canResizeV) {
                    int pIndex = 0;
                    // 当触摸到纵向下边线上时，认为是需要调整大小
                    for (ChartDiv cDiv : divsCopy) {
                        pIndex++;
                        if (pIndex == divsCopy.size()) {
                            break;
                        }
                        FCRect resizeRect = new FCRect(0, cDiv.getBottom() - 4, cDiv.getWidth(), cDiv.getBottom() + 4);
                        if (mp.x >= resizeRect.left && mp.x <= resizeRect.right && mp.y >= resizeRect.top && mp.y <= resizeRect.bottom) {
                            m_userResizeDiv = cDiv;
                        }
                    }
                }
                // 线条的选中，以及是否显示选中框
                if ((mp.x >= m_leftVScaleWidth && mp.x <= width - m_rightVScaleWidth)) {
                    if (touchOverDiv != null) {
                        ArrayList<FCPlot> plotsCopy = touchOverDiv.getPlots(SortType.DESC);
                        for (FCPlot lsb : plotsCopy) {
                            if (lsb.isEnabled() && lsb.isVisible() && lsb.onSelect()) {
                                m_movingPlot = lsb;
                                lsb.onMoveStart();
                                // 设置图层
                                ArrayList<Double> zorders = new ArrayList<Double>();
                                ArrayList<FCPlot> plots = touchOverDiv.getPlots(SortType.DESC);
                                int plotSize = plots.size();
                                for (int i = 0; i < plotSize; i++) {
                                    zorders.add((double) plots.get(i).getZOrder());
                                }
                                int zordersSize = zorders.size();
                                double zordersArray[] = new double[zordersSize];
                                for (int i = 0; i < zordersSize; i++) {
                                    zordersArray[i] = zorders.get(i);
                                }
                                lsb.setZOrder((int) FCScript.maxValue(zordersArray, zordersSize) + 1);
                            }
                        }
                    }
                    if (m_movingPlot != null) {
                        m_movingPlot.setSelected(true);
                        // 当有选中的线条时，要清空选中
                        if (shapeCount != 0) {
                            clearSelectedShape();
                        }
                    } else {
                        BaseShape bs = selectShape(m_crossStopIndex, 1);
                        // 认为是显示选中框
                        ChartDiv div = null;
                        if (bs == null) {
                            div = touchOverDiv;
                            if (div != null && div.getSelectArea().isEnabled()) {
                                if (mp.y >= div.getTop() + 2 && mp.y <= div.getBottom() - div.getHScale().getHeight() - 2) {
                                    m_showingSelectArea = true;
                                }
                            }
                        }
                    }
                }
            }
        } else {
            // 按右键时清除
            m_isTouchMove = true;
            m_showingToolTip = false;
        }
        // 保存上次点击的位置
        m_lastTouchClickPoint = mp.clone();
        // 设置拖动线条
        if (m_canMoveShape) {
            if (getSelectedShape() != null) {
                m_movingShape = getSelectedShape();
            }
        }
        invalidate();
    }

    /**
     * 触摸抬起的方法
     *
     * @param touchInfo 触摸信息
     */
    @Override
    public void onTouchMove(FCTouchInfo touchInfo) {
        super.onTouchMove(touchInfo.clone());
        FCPoint mp = touchInfo.m_firstPoint.clone();
        if (mp.x != m_lastTouchMovePoint.x || mp.y != m_lastTouchMovePoint.y) {
            int width = getWidth();
            m_isTouchMove = true;
            m_showingToolTip = false;
            ArrayList<ChartDiv> divsCopy = getDivs();
            for (ChartDiv div : divsCopy) {
                boolean resize = false;
                if (div.getSelectArea().isVisible() && div.getSelectArea().canresize()) {
                    resize = true;
                } else {
                    // 判断是否准备画选中框
                    if (m_showingSelectArea) {
                        if (touchInfo.m_firstTouch) {
                            int subX = m_lastTouchClickPoint.x - m_lastTouchMovePoint.x;
                            int subY = m_lastTouchMovePoint.y - m_lastTouchClickPoint.y;
                            if (Math.abs(subX) > m_hScalePixel * 2 || Math.abs(subY) > m_hScalePixel * 2) {
                                m_showingSelectArea = false;
                                div.getSelectArea().setVisible(true);
                                div.getSelectArea().setcanresize(true);
                                resize = true;
                            }
                        }
                    }
                }
                if (resize && touchInfo.m_firstTouch) {
                    // 获取矩形的属性
                    int x1 = m_lastTouchClickPoint.x;
                    int y1 = m_lastTouchClickPoint.y;
                    int x2 = mp.x;
                    int y2 = mp.y;
                    // 纠正位置
                    if (x2 < m_leftVScaleWidth) {
                        x2 = m_leftVScaleWidth;
                    } else if (x2 > getWidth() - m_rightVScaleWidth) {
                        x2 = getWidth() - m_rightVScaleWidth;
                    }
                    if (y2 > div.getBottom() - div.getHScale().getHeight()) {
                        y2 = div.getBottom() - div.getHScale().getHeight();
                    } else if (y2 < div.getTop() + div.getTitleBar().getHeight()) {
                        y2 = div.getTop() + div.getTitleBar().getHeight();
                    }
                    // 生成矩形
                    int bx = 0, by = 0, bwidth = 0, bheight = 0;
                    RefObject<Integer> tempRef_bx = new RefObject<Integer>(bx);
                    RefObject<Integer> tempRef_by = new RefObject<Integer>(by);
                    RefObject<Integer> tempRef_bwidth = new RefObject<Integer>(bwidth);
                    RefObject<Integer> tempRef_bheight = new RefObject<Integer>(bheight);
                    FCPlot.rectangleXYWH(x1, y1 - div.getTop(), x2, y2 - div.getTop(), tempRef_bx, tempRef_by, tempRef_bwidth, tempRef_bheight);
                    bx = tempRef_bx.argvalue;
                    by = tempRef_by.argvalue;
                    bwidth = tempRef_bwidth.argvalue;
                    bheight = tempRef_bheight.argvalue;
                    FCRect bounds = new FCRect(bx, by, bx + bwidth, by + bheight);
                    div.getSelectArea().setBounds(bounds);
                    invalidate();
                    m_lastTouchMovePoint = mp;
                    return;
                }
                if (div.getSelectArea().isVisible()) {
                    return;
                }
            }
            m_lastTouchMoveTime = Calendar.getInstance();
            if (m_movingPlot != null && touchInfo.m_firstTouch) {
                m_movingPlot.onMoving();
            } else {
                boolean outLoop = false;
                // 横向调整
                if (m_canResizeH) {
                    if (m_hResizeType == 0) {
                        if ((mp.x >= m_leftVScaleWidth - 4 && mp.x <= m_leftVScaleWidth + 4) || (mp.x >= width - m_rightVScaleWidth - 4 && mp.x <= width - m_rightVScaleWidth + 4)) {
                            outLoop = true;
                        }
                    } else {
                        outLoop = true;
                    }
                }
                // 纵向调整
                if (!outLoop && m_canResizeV) {
                    if (m_userResizeDiv == null) {
                        int pIndex = 0;
                        // 当触摸到纵向下边线上时，认为是需要调整大小
                        for (ChartDiv cDiv : divsCopy) {
                            pIndex++;
                            if (pIndex == divsCopy.size()) {
                                break;
                            }
                            FCRect resizeRect = new FCRect(0, cDiv.getBottom() - 4, getWidth(), cDiv.getBottom() + 4);
                            if (mp.x >= resizeRect.left && mp.x <= resizeRect.right && mp.y >= resizeRect.top && mp.y <= resizeRect.bottom) {
                                outLoop = true;
                            }
                        }
                    } else {
                        outLoop = true;
                    }
                }
                // 恢复触摸
            }
            m_crossStopIndex = getTouchOverIndex();
            m_cross_y = mp.y;
            // 画十字线
            if (m_showCrossLine && m_crossLineMoveMode == CrossLineMoveMode.FollowTouch) {
                m_isScrollCross = false;
            }
            invalidate();
        }
        m_lastTouchMovePoint = mp;
    }

    /**
     * 触摸按下的方法
     *
     * @param touchInfo 触摸信息
     */
    @Override
    public void onTouchUp(FCTouchInfo touchInfo) {
        super.onTouchUp(touchInfo.clone());
        boolean needUpdate = false;
        // 移除移动的图形
        if (m_movingShape != null) {
            m_movingShape = null;
        }
        // 禁止移动选中框
        ArrayList<ChartDiv> divsCopy = getDivs();
        for (ChartDiv div : divsCopy) {
            if (div.getSelectArea().isVisible()) {
                div.getSelectArea().setcanresize(false);
                invalidate();
                return;
            }
        }
        FCPoint mp = touchInfo.m_firstPoint.clone();
        // 获取触摸按下的控件
        BaseShape selectedShape = getSelectedShape();
        if (m_hResizeType == 0 && m_userResizeDiv == null && touchInfo.m_firstTouch && m_canMoveShape && selectedShape != null) {
            ChartDiv curDiv = findDiv(selectedShape);
            // 循环遍历所有的层
            for (ChartDiv cDiv : divsCopy) {
                if (mp.y >= cDiv.getTop() && mp.y <= cDiv.getBottom()) {
                    if (cDiv == curDiv) {
                        break;
                    }
                    // K线
                    if (!cDiv.containsShape(selectedShape)) {
                        for (ChartDiv div : divsCopy) {
                            if (div.containsShape(selectedShape)) {
                                div.removeShape(selectedShape);
                            }
                        }
                        cDiv.addShape(selectedShape);
                        needUpdate = true;
                    }
                }
            }
        }
        // 清除移动中的画线工具
        if (m_movingPlot != null) {
            m_movingPlot = null;
        }
        // 重新修改层的布局
        if (resizeDiv()) {
            needUpdate = true;
        }
        // 完全重绘
        if (needUpdate) {
            update();
        }
        invalidate();
    }

    /**
     * 重绘前景方法
     *
     * @param paint 绘图对象
     * @param clipRect 裁剪区域
     */
    @Override
    public void onPaintForeground(FCPaint paint, FCRect clipRect) {
        if (isVisible()) {
            ArrayList<ChartDiv> divsCopy = getDivs();
            FCPoint offset = paint.getOffset();
            for (ChartDiv div : divsCopy) {
                int offsetX = offset.x + div.getLeft();
                int offsetY = offset.y + div.getTop();
                paint.setOffset(new FCPoint(offsetX, offsetY));
                FCRect divClipRect = new FCRect(0, 0, div.getWidth(), div.getHeight());
                paint.setClip(divClipRect);
                onPaintDivBackGround(paint, div);
                onPaintVScale(paint, div);
                onPaintHScale(paint, div);
                onPaintShapes(paint, div);
                onPaintDivBorder(paint, div);
                onPaintCrossLine(paint, div);
                onPaintTitle(paint, div);
                onPaintSelectArea(paint, div);
                onPaintPlots(paint, div);
            }
            paint.setOffset(offset);
            paint.setClip(clipRect);
            // 画拖动变线
            onPaintResizeLine(paint);
            // 画股指提示
            onPaintToolTip(paint);
            // 画拖动图标
            onPaintIcon(paint);
        }
    }

    /**
     * 秒表回调方法
     *
     * @param timerID 编号
     */
    @Override
    public void onTimer(int timerID) {
        super.onTimer(timerID);
        if (isVisible() && m_timerID == timerID) {
            FCNative inative = getNative();
            if (this == inative.getHoveredControl()) {
                checkToolTip();
            }
        }
    }

    public enum ScrollType {

        /**
        * 无
        */
        None(0),
        /**
         * 左滚
         */
        Left(1),
        /**
         * 右滚
         */
        Right(2),
        /**
         * 缩小
         */
        zoomIn(3),
        /**
         * 放大
         */
        zoomOut(4);

        private int intValue;
        private static HashMap<Integer, ScrollType> mappings;

        private synchronized static HashMap<Integer, ScrollType> getMappings() {
            if (mappings == null) {
                mappings = new HashMap<Integer, ScrollType>();
            }
            return mappings;
        }

        private ScrollType(int value) {
            intValue = value;
            ScrollType.getMappings().put(value, this);
        }

        public int getValue() {
            return intValue;
        }

        public static ScrollType forValue(int value) {
            return getMappings().get(value);
        }
    }

    /**
     * 获取最大显示条数
     *
     * @param hScalePixel 数据间隔
     * @param pureH 横向宽度
     * @returns 最大显示条数
     */
    protected int getMaxVisibleCount(double hScalePixel, int pureH) {
        return (int) (pureH / hScalePixel);
    }

    /**
     * 获取K线最大值的显示位置
     *
     * @param scaleX 横坐标
     * @param scaleY 纵坐标
     * @param stringWidth 文字的宽度
     * @param stringHeight 文字的高度
     * @param actualWidth 横向宽度
     * @param leftVScaleWidth 左侧纵轴宽度
     * @param rightVScaleWidth 右侧纵轴宽度
     * @param x 最大值的横坐标
     * @param y 最大值的纵坐标
     */
    protected void getCandleMaxStringPoint(float scaleX, float scaleY, float stringWidth, float stringHeight, int actualWidth, int leftVScaleWidth, int rightVScaleWidth, RefObject<Float> x, RefObject<Float> y) {
        if (scaleX < leftVScaleWidth + stringWidth) {
            x.argvalue = scaleX;
        } else if (scaleX > actualWidth - rightVScaleWidth - stringWidth) {
            x.argvalue = scaleX - stringWidth;
        } else {
            if (scaleX < actualWidth / 2) {
                x.argvalue = scaleX - stringWidth;
            } else {
                x.argvalue = scaleX;
            }
        }
        y.argvalue = scaleY + stringHeight / 2;
    }

    /**
     * 获取K线最小值的显示位置
     *
     * @param scaleX 横坐标
     * @param scaleY 纵坐标
     * @param stringWidth 文字的宽度
     * @param stringHeight 文字的高度
     * @param actualWidth 横向宽度
     * @param leftVScaleWidth 左侧纵轴宽度
     * @param rightVScaleWidth 右侧纵轴宽度
     * @param x 最大值的横坐标
     * @param y 最大值的纵坐标
     */
    protected void getCandleMinStringPoint(float scaleX, float scaleY, float stringWidth, float stringHeight, int actualWidth, int leftVScaleWidth, int rightVScaleWidth, RefObject<Float> x, RefObject<Float> y) {
        if (scaleX < leftVScaleWidth + stringWidth) {
            x.argvalue = scaleX;
        } else if (scaleX > actualWidth - rightVScaleWidth - stringWidth) {
            x.argvalue = scaleX - stringWidth;
        } else {
            if (scaleX < actualWidth / 2) {
                x.argvalue = scaleX - stringWidth;
            } else {
                x.argvalue = scaleX;
            }
        }
        y.argvalue = scaleY - stringHeight * 3 / 2;
    }

    /**
     * 获取某坐标对应的索引
     *
     * @param x 横坐标
     * @param leftVScaleWidth 左侧纵轴的高度
     * @param hScalePixel 数据间隔
     * @param firstVisibleIndex 首先可见索引
     * @returns 坐标
     */
    protected int getChartIndex(int x, int leftVScaleWidth, double hScalePixel, int firstVisibleIndex) {
        return (int) ((x - leftVScaleWidth) / hScalePixel + firstVisibleIndex);
    }

    /**
     * 获取阳K线的高度
     *
     * @param close 收盘价
     * @param open 开盘价
     * @param max 最高价
     * @param min 最低价
     * @param divPureV 层高度
     * @returns 高度
     */
    protected float getUpCandleHeight(double close, double open, double max, double min, float divPureV) {
        if (close - open == 0) {
            return 1;
        } else {
            return (float) ((close - open) / (max - min) * divPureV);
        }
    }

    /**
     * 获取阴K线的高度
     *
     * @param close 收盘价
     * @param open 开盘价
     * @param max 最高价
     * @param min 最低价
     * @param divPureV 层高度
     * @returns 高度
     */
    protected float getDownCandleHeight(double close, double open, double max, double min, float divPureV) {
        if (close - open == 0) {
            return 1;
        } else {
            return (float) ((open - close) / (max - min) * divPureV);
        }
    }

    /**
     * 左滚
     *
     * @param step 步长
     * @param dateCount 数据条数
     * @param hScalePixel 数据间隔
     * @param pureH 横向宽度
     * @param fIndex 首先可见索引号
     * @param lIndex 最后可见索引号
     */
    protected void scrollLeft(int step, int dateCount, double hScalePixel, int pureH, RefObject<Integer> fIndex, RefObject<Integer> lIndex) {
        int max = getMaxVisibleCount(hScalePixel, pureH);
        int right = -1;
        if (dateCount > max) {
            right = max - (lIndex.argvalue - fIndex.argvalue);
            if (right > 1) {
                fIndex.argvalue = lIndex.argvalue - max + 1;
                if (fIndex.argvalue > lIndex.argvalue) {
                    fIndex.argvalue = lIndex.argvalue;
                }
            } else {
                if (fIndex.argvalue - step >= 0) {
                    fIndex.argvalue = fIndex.argvalue - step;
                    lIndex.argvalue = lIndex.argvalue - step;
                } else {
                    if (fIndex.argvalue != 0) {
                        lIndex.argvalue = lIndex.argvalue - fIndex.argvalue;
                        fIndex.argvalue = 0;
                    }
                }
            }
        }
    }

    /**
     * 右滚
     *
     * @param step 步长
     * @param dateCount 数据条数
     * @param hScalePixel 数据间隔
     * @param pureH 横向宽度
     * @param fIndex 首先可见索引号
     * @param lIndex 最后可见索引号
     */
    protected void scrollRight(int step, int dataCount, double hScalePixel, int pureH, RefObject<Integer> fIndex, RefObject<Integer> lIndex) {
        int max = getMaxVisibleCount(hScalePixel, pureH);
        if (dataCount > max) {
            if (lIndex.argvalue < dataCount - 1) {
                if (lIndex.argvalue + step > dataCount - 1) {
                    fIndex.argvalue = fIndex.argvalue + dataCount - lIndex.argvalue;
                    lIndex.argvalue = dataCount - 1;
                } else {
                    fIndex.argvalue = fIndex.argvalue + step;
                    lIndex.argvalue = lIndex.argvalue + step;
                }
            } else {
                fIndex.argvalue = lIndex.argvalue - (int) (max * 0.9);
                if (fIndex.argvalue > lIndex.argvalue) {
                    fIndex.argvalue = lIndex.argvalue;
                }
            }
        }
    }

    /**
     * 获取纵轴某坐标的值
     *
     * @param y 纵坐标
     * @param max 最大值
     * @param min 最小值
     * @param vHeight 层高度
     * @returns 数值
     */
    protected double getVScaleValue(int y, double max, double min, float vHeight) {
        double every = (max - min) / vHeight;
        return max - y * every;
    }

    /**
     * 修正可见索引
     *
     * @param dataCount 数据条数
     * @param first 首先可见索引号
     * @param last 最后可见索引号
     */
    protected void correctVisibleRecord(int dataCount, RefObject<Integer> first, RefObject<Integer> last) {
        if (dataCount > 0) {
            if (first.argvalue == -1) {
                first.argvalue = 0;
            }
            if (last.argvalue == -1) {
                last.argvalue = 0;
            }
            if (first.argvalue > last.argvalue) {
                first.argvalue = last.argvalue;
            }
            if (last.argvalue < first.argvalue) {
                last.argvalue = first.argvalue;
            }
        } else {
            first.argvalue = -1;
            last.argvalue = -1;
        }
    }

    /**
     * 重置十字线索引
     *
     * @param dataCount 数据条数
     * @param maxVisibleRecord 最大显示记录数
     * @param crossStopIndex 十字线索引
     * @param firstL 首先可见索引号
     * @param lastL 最后可见索引号
     * @returns 修正后的十字线索引
     */
    protected int resetCrossOverIndex(int dataCount, int maxVisibleRecord, int crossStopIndex, int firstL, int lastL) {
        if (dataCount > 0 && dataCount >= maxVisibleRecord) {
            if (crossStopIndex < firstL) {
                crossStopIndex = firstL;
            }
            if (crossStopIndex > lastL) {
                crossStopIndex = lastL;
            }
        }
        return crossStopIndex;
    }

    /**
     * 缩小
     *
     * @param pureH 横向宽度
     * @param dataCount 数据条数
     * @param findex 首先可见索引号
     * @param lindex 最后可见索引号
     * @param hScalePixel 数据间隔
     */
    protected void zoomIn(int pureH, int dataCount, RefObject<Integer> findex, RefObject<Integer> lindex, RefObject<Double> hScalePixel) {
        int max = -1;
        if (hScalePixel.argvalue > 1) {
            hScalePixel.argvalue -= 2;
        } else {
            hScalePixel.argvalue = hScalePixel.argvalue * 2 / 3;
        }
        max = getMaxVisibleCount(hScalePixel.argvalue, pureH);
        if (max >= dataCount) {
            if (hScalePixel.argvalue < 1) {
                hScalePixel.argvalue = (double) pureH / max;
            }
            findex.argvalue = 0;
            lindex.argvalue = dataCount - 1;
        } else {
            findex.argvalue = lindex.argvalue - max + 1;
            if (findex.argvalue < 0) {
                findex.argvalue = 0;
            }
        }
    }

    /**
     * 放大
     *
     * @param pureH 横向宽度
     * @param dataCount 数据条数
     * @param findex 首先可见索引号
     * @param lindex 最后可见索引号
     * @param hScalePixel 数据间隔
     */
    protected void zoomOut(int pureH, int dataCount, RefObject<Integer> findex, RefObject<Integer> lindex, RefObject<Double> hScalePixel) {
        int oriMax = -1, max = -1, deal = 0;
        if (hScalePixel.argvalue < 30) {
            oriMax = getMaxVisibleCount(hScalePixel.argvalue, pureH);
            if (dataCount < oriMax) {
                deal = 1;
            }
            if (hScalePixel.argvalue >= 1) {
                hScalePixel.argvalue += 2;
            } else {
                hScalePixel.argvalue = hScalePixel.argvalue * 1.5;
                if (hScalePixel.argvalue > 1) {
                    hScalePixel.argvalue = (double) 1;
                }
            }
            max = getMaxVisibleCount(hScalePixel.argvalue, pureH);
            if (dataCount >= max) {
                if (deal == 1) {
                    lindex.argvalue = dataCount - 1;
                }
                findex.argvalue = lindex.argvalue - max + 1;
                if (findex.argvalue < 0) {
                    findex.argvalue = 0;
                }
            }
        }
    }
}
