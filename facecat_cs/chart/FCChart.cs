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
using System.Text;

namespace FaceCat {
    /// <summary>
    ///  股票布局控件
    /// </summary>
    public class FCChart : FCView {
        /// <summary>
        /// 创建控件
        /// </summary>
        public FCChart() {
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~FCChart() {
            delete();
        }

        /// <summary>
        /// 图层
        /// </summary>
        protected ArrayList<ChartDiv> m_divs = new ArrayList<ChartDiv>();

        /// <summary>
        /// 十字线定位y坐标
        /// </summary>
        protected int m_cross_y = -1;

        /// <summary>
        /// 横向重置大小标识
        /// </summary>
        protected int m_hResizeType = 0;

        /// <summary>
        /// 最后一条非空索引
        /// </summary>
        protected int m_lastUnEmptyIndex = -1;

        /// <summary>
        /// 当前最后一条记录是否可见
        /// </summary>
        protected bool m_lastRecordIsVisible;

        /// <summary>
        /// 最后一条可见记录的时间
        /// </summary>
        protected double m_lastVisibleKey;

        /// <summary>
        /// 滚动十字线标识
        /// </summary>
        protected bool m_isScrollCross;

        /// <summary>
        /// 触摸是否正在移动
        /// </summary>
        protected bool m_isTouchMove = true;

        /// <summary>
        /// 上次点击的位置
        /// </summary>
        protected FCPoint m_lastTouchClickPoint = new FCPoint(-1, -1);

        /// <summary>
        /// 上次移动的点位
        /// </summary>
        protected FCPoint m_lastTouchMovePoint;

        /// <summary>
        /// 触摸最后一次移动的事件
        /// </summary>
        protected DateTime m_lastTouchMoveTime = DateTime.Now;

        /// <summary>
        /// 按键滚动的幅度
        /// </summary>
        protected int m_scrollStep = 1;

        /// <summary>
        /// 显示选中区域
        /// </summary>
        protected bool m_showingSelectArea;

        /// <summary>
        /// 是否正在显示提示框
        /// </summary>
        protected bool m_showingToolTip;

        /// <summary>
        /// 秒表编号
        /// </summary>
        protected int m_timerID = getNewTimerID();

        /// <summary>
        /// 提示框显示延迟tick值
        /// </summary>
        protected const int m_tooltip_dely = 2500000;

        /// <summary>
        /// 获取或设置正在改变大小的图层
        /// </summary>
        protected ChartDiv m_userResizeDiv;

        protected bool m_autoFillHScale;

        /// <summary>
        /// 获取或设置数据是否
        /// </summary>
        public virtual bool AutoFillHScale {
            get { return m_autoFillHScale; }
            set { m_autoFillHScale = value; }
        }

        protected int m_blankSpace;

        /// <summary>
        /// 获取或设置空白空间
        /// </summary>
        public virtual int BlankSpace {
            get { return m_blankSpace; }
            set { m_blankSpace = value; }
        }

        protected bool m_canResizeV = true;

        /// <summary>
        /// 获取或设置是否可纵向改变大小
        /// </summary>
        public virtual bool CanResizeV {
            get { return m_canResizeV; }
            set { m_canResizeV = value; }
        }

        protected bool m_canResizeH = true;

        /// <summary>
        /// 获取或设置是可横向改变大小
        /// </summary>
        public virtual bool CanResizeH {
            get { return m_canResizeH; }
            set { m_canResizeH = value; }
        }

        protected bool m_canMoveShape = true;

        /// <summary>
        /// 获取或设置是否可以拖动线条
        /// </summary>
        public virtual bool CanMoveShape {
            get { return m_canMoveShape; }
            set { m_canMoveShape = value; }
        }

        protected CrossLineMoveMode m_crossLineMoveMode = CrossLineMoveMode.FollowTouch;

        /// <summary>
        /// 获取或设置十字线的移动方式
        /// </summary>
        public virtual CrossLineMoveMode CrossLineMoveMode {
            get { return m_crossLineMoveMode; }
            set { m_crossLineMoveMode = value; }
        }

        protected bool m_canScroll = true;

        /// <summary>
        /// 获取或设置是否可以执行滚动操作
        /// </summary>
        public virtual bool CanScroll {
            get { return m_canScroll; }
            set { m_canScroll = value; }
        }

        protected bool m_canZoom = true;

        /// <summary>
        /// 获取或设置是否可以执行缩放操作
        /// </summary>
        public virtual bool CanZoom {
            get { return m_canZoom; }
            set { m_canZoom = value; }
        }

        protected int m_crossStopIndex;

        /// <summary>
        /// 获取十字线当前停留的记录索引
        /// </summary>
        public virtual int CrossStopIndex {
            get { return m_crossStopIndex; }
            set { m_crossStopIndex = value; }
        }

        protected FCDataTable m_dataSource;

        /// <summary>
        /// 获取数据源
        /// </summary>
        public virtual FCDataTable DataSource {
            get { return m_dataSource; }
        }

        protected int m_firstVisibleIndex = -1;

        /// <summary>
        /// 获取首先可见记录号
        /// </summary>
        public virtual int FirstVisibleIndex {
            get { return m_firstVisibleIndex; }
        }

        protected String m_hScaleFieldText;

        /// <summary>
        /// 获取横轴字段的显示文字
        /// </summary>
        public virtual String HScaleFieldText {
            get { return m_hScaleFieldText; }
            set { m_hScaleFieldText = value; }
        }

        protected double m_hScalePixel = 7;

        /// <summary>
        /// 获取或设置每条数据在X轴上所占的空间
        /// </summary>
        public virtual double HScalePixel {
            get { return m_hScalePixel; }
            set {
                m_hScalePixel = value;
                if (m_hScalePixel > 1) m_hScalePixel = (int)m_hScalePixel;
                if (m_hScalePixel > 1 && m_hScalePixel % 2 == 0) {
                    m_hScalePixel += 1;
                }
            }
        }

        protected int m_lastVisibleIndex = -1;

        /// <summary>
        /// 获取最后可见的记录号
        /// </summary>
        public virtual int LastVisibleIndex {
            get { return m_lastVisibleIndex; }
        }

        protected int m_leftVScaleWidth = 80;

        /// <summary>
        /// 获取或设置左侧Y轴的宽度
        /// </summary>
        public virtual int LeftVScaleWidth {
            get { return m_leftVScaleWidth; }
            set { m_leftVScaleWidth = value; }
        }

        protected int m_maxVisibleRecord;

        /// <summary>
        /// 获取显示最大记录数
        /// </summary>
        /// <returns>最大记录数</returns>
        public virtual int MaxVisibleRecord {
            get { return m_maxVisibleRecord; }
        }

        protected BaseShape m_movingShape;

        /// <summary>
        /// 获取或设置正在被移动的图形
        /// </summary>
        public virtual BaseShape MovingShape {
            get { return m_movingShape; }
            set { m_movingShape = value; }
        }

        protected FCPlot m_movingPlot = null;

        /// <summary>
        /// 获取或设置正在移动的画线工具
        /// </summary>
        public virtual FCPlot MovingPlot {
            get { return m_movingPlot; }
            set { m_movingPlot = value; }
        }

        protected bool m_reverseHScale;

        /// <summary>
        /// 获取或设置是否反转横轴
        /// </summary>
        public virtual bool ReverseHScale {
            get { return m_reverseHScale; }
            set { m_reverseHScale = value; }
        }

        protected int m_rightVScaleWidth = 0;

        /// <summary>
        /// 获取或设置右侧Y轴的宽度
        /// </summary>
        public virtual int RightVScaleWidth {
            get { return m_rightVScaleWidth; }
            set { m_rightVScaleWidth = value; }
        }

        protected bool m_scrollAddSpeed;

        /// <summary>
        /// 获取或设置是否启用加速效果
        /// </summary>
        public virtual bool ScrollAddSpeed {
            get { return m_scrollAddSpeed; }
            set { m_scrollAddSpeed = value; }
        }

        /// <summary>
        /// 获取或设置当前选中的线条
        /// </summary>
        public virtual BaseShape SelectedShape {
            get {
                ArrayList<ChartDiv> divsCopy = getDivs();
                foreach (ChartDiv cDiv in divsCopy) {
                    ArrayList<BaseShape> shapesCopy = cDiv.getShapes(SortType.NONE);
                    foreach (BaseShape bs in shapesCopy) {
                        if (bs.Selected) {
                            return bs;
                        }
                    }
                }
                return null;
            }
            set {
                ArrayList<ChartDiv> divsCopy = getDivs();
                foreach (ChartDiv cDiv in divsCopy) {
                    ArrayList<BaseShape> shapesCopy = cDiv.getShapes(SortType.ASC);
                    foreach (BaseShape bs in shapesCopy) {
                        if (bs == value) {
                            bs.Selected = true;
                        }
                        else {
                            bs.Selected = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取或设置当前选中的画线工具
        /// </summary>
        public virtual FCPlot SelectedPlot {
            get {
                ArrayList<ChartDiv> divsCopy = getDivs();
                foreach (ChartDiv div in divsCopy) {
                    ArrayList<FCPlot> plotsCopy = div.getPlots(SortType.NONE);
                    foreach (FCPlot plot in plotsCopy) {
                        if (plot.Visible && plot.Selected) {
                            return plot;
                        }
                    }
                }
                return null;
            }
            set {
                ArrayList<ChartDiv> divsCopy = getDivs();
                foreach (ChartDiv div in divsCopy) {
                    ArrayList<FCPlot> plotsCopy = div.getPlots(SortType.NONE);
                    foreach (FCPlot plot in plotsCopy) {
                        if (plot == value) {
                            plot.Selected = true;
                        }
                        else {
                            plot.Selected = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取或设置当前选中的层
        /// </summary>
        public virtual ChartDiv SelectedDiv {
            get {
                ArrayList<ChartDiv> divsCopy = getDivs();
                foreach (ChartDiv div in divsCopy) {
                    if (div.Selected) {
                        return div;
                    }
                }
                return null;
            }
            set {
                ArrayList<ChartDiv> divsCopy = getDivs();
                foreach (ChartDiv div in divsCopy) {
                    if (div == value) {
                        div.Selected = true;
                    }
                    else {
                        div.Selected = false;
                    }
                }
            }
        }

        protected bool m_showCrossLine;

        /// <summary>
        /// 获取或设置是否显示十字线
        /// </summary>
        public virtual bool ShowCrossLine {
            get { return m_showCrossLine; }
            set { m_showCrossLine = value; }
        }

        protected int m_workingAreaWidth;

        /// <summary>
        /// 获取层去掉坐标轴宽度后的横向宽度
        /// </summary>
        /// <returns>宽度</returns>
        public virtual int WorkingAreaWidth {
            get { return m_workingAreaWidth; }
        }

        /// <summary>
        /// 添加一个新的图层，按照所设置的比例调节纵向高度
        /// </summary>
        /// <param name="vPercent">纵向高度比例</param>
        /// <returns>图层</returns>
        public virtual ChartDiv addDiv(int vPercent) {
            if (vPercent <= 0) return null;
            //创建层
            ChartDiv cDiv = new ChartDiv();
            cDiv.VerticalPercent = vPercent;
            cDiv.Chart = this;
            m_divs.add(cDiv);
            update();
            return cDiv;
        }

        /// <summary>
        /// 添加一个新的图层，自动调节高度
        /// </summary>
        /// <returns>图层ID</returns>
        public virtual ChartDiv addDiv() {
            ArrayList<ChartDiv> divsCopy = getDivs();
            int pNum = divsCopy.size() + 1;
            return addDiv(100 / pNum);
        }

        /// <summary>
        /// 设置可见部分的最大值和最小值
        /// </summary>
        public virtual void adjust() {
            if (m_workingAreaWidth > 0) {
                m_lastUnEmptyIndex = -1;
                if (m_firstVisibleIndex < 0 || m_lastVisibleIndex > m_dataSource.RowsCount - 1) {
                    return;
                }
                ArrayList<ChartDiv> divsCopy = getDivs();
                foreach (ChartDiv cDiv in divsCopy) {
                    //获取图层的数据区域
                    cDiv.WorkingAreaHeight = cDiv.Height - cDiv.HScale.Height - cDiv.TitleBar.Height - 1;
                    //获取数据
                    ArrayList<BaseShape> shapesCopy = cDiv.getShapes(SortType.NONE);
                    double leftMax = 0, leftMin = 0, rightMax = 0, rightMin = 0;
                    bool leftMaxInit = false, leftMinInit = false, rightMaxInit = false, rightMinInit = false;
                    VScale leftVScale = cDiv.LeftVScale;
                    VScale rightVScale = cDiv.RightVScale;
                    if (m_dataSource.RowsCount > 0) {
                        foreach (BaseShape bs in shapesCopy) {
                            if (!bs.Visible) {
                                continue;
                            }
                            BarShape bar = bs as BarShape;
                            int[] fields = bs.getFields();
                            if (fields == null) {
                                continue;
                            }
                            for (int f = 0; f < fields.Length; f++) {
                                int field = m_dataSource.getColumnIndex(fields[f]);
                                //循环遍历
                                for (int i = m_firstVisibleIndex; i <= m_lastVisibleIndex; i++) {
                                    //获取字段的值
                                    double fieldValue = m_dataSource.get3(i, field);
                                    //其他线条类型
                                    if (!double.IsNaN(fieldValue)) {
                                        m_lastUnEmptyIndex = i;
                                        if (bs.AttachVScale == AttachVScale.Left) {
                                            if (fieldValue > leftMax || !leftMaxInit) {
                                                leftMaxInit = true;
                                                leftMax = fieldValue;
                                            }
                                            if (fieldValue < leftMin || !leftMinInit) {
                                                leftMinInit = true;
                                                leftMin = fieldValue;
                                            }
                                        }
                                        else {
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
                                if (bar.FieldName2 == FCDataTable.NULLFIELD) {
                                    double midValue = 0;
                                    if (bs.AttachVScale == AttachVScale.Left) {
                                        if (midValue > leftMax || !leftMaxInit) {
                                            leftMaxInit = true;
                                            leftMax = midValue;
                                        }
                                        if (midValue < leftMin || !leftMinInit) {
                                            leftMinInit = true;
                                            leftMin = midValue;
                                        }
                                    }
                                    else {
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
                    //修正
                    if (leftVScale.AutoMaxMin) {
                        //设置主轴的最大最小值
                        leftVScale.VisibleMax = leftMax;
                        leftVScale.VisibleMin = leftMin;
                    }
                    if (rightVScale.AutoMaxMin) {
                        //设置副轴的最大最小值
                        rightVScale.VisibleMax = rightMax;
                        rightVScale.VisibleMin = rightMin;
                    }
                    //修正
                    if (leftVScale.AutoMaxMin && leftVScale.VisibleMax == 0 && leftVScale.VisibleMin == 0) {
                        leftVScale.VisibleMax = rightVScale.VisibleMax;
                        leftVScale.VisibleMin = rightVScale.VisibleMin;
                    }
                    if (rightVScale.AutoMaxMin && rightVScale.VisibleMax == 0 && rightVScale.VisibleMin == 0) {
                        rightVScale.VisibleMax = leftVScale.VisibleMax;
                        rightVScale.VisibleMin = leftVScale.VisibleMin;
                    }
                }
            }
        }

        /// <summary>
        /// 在指定层的指定位置添加画线工具
        /// </summary>
        /// <param name="bpl">画线工具对象</param>
        /// <param name="mp">出现的位置</param>
        /// <param name="div">画线工具所在层</param>
        public virtual void addPlot(FCPlot bpl, FCPoint mp, ChartDiv div) {
            if (div != null && m_dataSource.RowsCount >= 2) {
                //获取索引
                int rIndex = getIndex(mp);
                if (rIndex < 0 || rIndex > m_lastVisibleIndex) {
                    return;
                }
                if (bpl != null) {
                    bpl.Div = div;
                    bpl.Selected = true;
                    ArrayList<double> zorders = new ArrayList<double>();
                    ArrayList<FCPlot> plots = div.getPlots(SortType.NONE);
                    int plotSize = plots.size();
                    for (int i = 0; i < plotSize; i++) {
                        zorders.add(plots[i].ZOrder);
                    }
                    bpl.ZOrder = (int)FCScript.maxValue(zorders.ToArray(), zorders.size()) + 1;
                    bool flag = bpl.onCreate(mp);
                    if (flag) {
                        div.addPlot(bpl);
                        MovingPlot = bpl;
                        MovingPlot.onMoveStart();
                    }
                }
                //关闭选中区域
                closeSelectArea();
            }
        }

        /// <summary>
        /// 对图像进行操作
        /// </summary>
        /// <param name="scrollType">ScrollType枚举</param>
        /// <param name="limitStep">滚动幅度</param>
        public virtual void changeChart(ScrollType scrollType, int limitStep) {
            ArrayList<ChartDiv> divsCopy = getDivs();
            if (divsCopy.size() == 0 || m_dataSource.RowsCount == 0) {
                return;
            }
            int fIndex = m_firstVisibleIndex;
            int lIndex = m_lastVisibleIndex;
            double axis = m_hScalePixel;
            bool flag = false;
            bool locateCrossHairFlag = false;
            switch (scrollType) {
                //向左
                case ScrollType.Left:
                    if (m_canScroll) {
                        flag = true;
                        if (m_showCrossLine) {
                            if (limitStep > m_scrollStep) {
                                scrollCrossLineLeft(limitStep);
                            }
                            else {
                                scrollCrossLineLeft(m_scrollStep);
                            }
                            locateCrossHairFlag = true;
                        }
                        else {
                            if (limitStep > m_scrollStep) {
                                scrollLeft(limitStep);
                            }
                            else {
                                scrollLeft(m_scrollStep);
                            }
                        }
                    }
                    break;
                //向右
                case ScrollType.Right:
                    if (m_canScroll) {
                        flag = true;
                        if (m_showCrossLine) {
                            if (limitStep > m_scrollStep) {
                                scrollCrossLineRight(limitStep);
                            }
                            else {
                                scrollCrossLineRight(m_scrollStep);
                            }
                            locateCrossHairFlag = true;
                        }
                        else {
                            if (limitStep > m_scrollStep) {
                                scrollRight(limitStep);
                            }
                            else {
                                scrollRight(m_scrollStep);
                            }
                        }
                    }
                    break;
                //缩小
                case ScrollType.ZoomIn:
                    if (m_canZoom) {
                        flag = true;
                        zoomIn();
                    }
                    break;
                //放大
                case ScrollType.ZoomOut:
                    if (m_canZoom) {
                        flag = true;
                        zoomOut();
                    }
                    break;
            }
            //需要重新布局
            if (flag) {
                int fIndex_after = m_firstVisibleIndex;
                int lIndex_after = m_lastVisibleIndex;
                double axis_after = m_hScalePixel;
                int fi = m_firstVisibleIndex;
                int li = m_lastVisibleIndex;
                correctVisibleRecord(m_dataSource.RowsCount, ref fi, ref li);
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
                }
                else {
                    update();
                    invalidate();
                }
            }
            //加速滚动
            if (m_scrollAddSpeed && (scrollType == ScrollType.Left || scrollType == ScrollType.Right)) {
                if (m_scrollStep < 50) {
                    m_scrollStep += 5;
                }
            }
            else {
                m_scrollStep = 1;
            }
        }

        /// <summary>
        /// 检查并弹出提示框
        /// </summary>
        public virtual void checkToolTip() {
            if (m_lastTouchMoveTime.AddTicks(m_tooltip_dely) <= DateTime.Now) {
                if (m_isTouchMove) {
                    bool show = true;
                    //过滤显示
                    if (isOperating()) {
                        show = false;
                    }
                    ArrayList<ChartDiv> divsCopy = getDivs();
                    foreach (ChartDiv div in divsCopy) {
                        if (div.SelectArea.Visible) {
                            show = false;
                        }
                    }
                    if (show) {
                        m_showingToolTip = true;
                        //显示提示框
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

        /// <summary>
        /// 检查最后可见索引
        /// </summary>
        private void checkLastVisibleIndex() {
            if (m_lastVisibleIndex > m_dataSource.RowsCount - 1) {
                m_lastVisibleIndex = m_dataSource.RowsCount - 1;
            }
            if (m_dataSource.RowsCount > 0) {
                m_lastVisibleKey = m_dataSource.getXValue(m_lastVisibleIndex);
                if (m_lastVisibleIndex == m_dataSource.RowsCount - 1) {
                    m_lastRecordIsVisible = true;
                }
                else {
                    m_lastRecordIsVisible = false;
                }
            }
            else {
                m_lastVisibleKey = 0;
                m_lastRecordIsVisible = true;
            }
        }

        /// <summary>
        /// 清除图像上的数据，但不改变图形结构
        /// </summary>
        public virtual void clear() {
            ArrayList<ChartDiv> divsCopy = getDivs();
            //清除画线工具
            foreach (ChartDiv cDiv in divsCopy) {
                foreach (FCPlot plot in cDiv.getPlots(SortType.NONE)) {
                    cDiv.removePlot(plot);
                    plot.delete();
                }
            }
            //清除选中框
            closeSelectArea();
            //清除数据源
            m_dataSource.clear();
            //重置索引
            m_firstVisibleIndex = -1;
            m_lastVisibleIndex = -1;
            m_lastRecordIsVisible = true;
            m_lastVisibleIndex = 0;
            m_lastVisibleKey = 0;
            m_showCrossLine = false;
        }

        /// <summary>
        /// 取消选中所有图形，包括K线，柱状图，线等
        /// </summary>
        public virtual void clearSelectedShape() {
            ArrayList<ChartDiv> divsCopy = getDivs();
            foreach (ChartDiv cDiv in divsCopy) {
                ArrayList<BaseShape> shapesCopy = cDiv.getShapes(SortType.NONE);
                foreach (BaseShape bs in shapesCopy) {
                    bs.Selected = false;
                }
            }
        }

        /// <summary>
        /// 取消选中所有的画线工具
        /// </summary>
        public virtual void clearSelectedPlot() {
            ArrayList<ChartDiv> divsCopy = getDivs();
            foreach (ChartDiv cDiv in divsCopy) {
                ArrayList<FCPlot> plotsCopy = cDiv.getPlots(SortType.NONE);
                foreach (FCPlot bls in plotsCopy) {
                    bls.Selected = false;
                }
            }
            MovingPlot = null;
        }

        /// <summary>
        /// 取消选中所有的层
        /// </summary>
        public virtual void clearSelectedDiv() {
            ArrayList<ChartDiv> divsCopy = getDivs();
            foreach (ChartDiv div in divsCopy) {
                div.Selected = false;
            }
        }

        /// <summary>
        /// 隐藏选中框
        /// </summary>
        public virtual void closeSelectArea() {
            ArrayList<ChartDiv> divsCopy = getDivs();
            //循环遍历所有的层
            foreach (ChartDiv div in divsCopy) {
                div.SelectArea.close();
            }
            m_showingSelectArea = false;
        }

        /// <summary>
        /// 根据记录号获取层极值
        /// </summary>
        /// <param name="index">索引号</param>
        /// <param name="div">图层</param>
        /// <param name="flag">标识 0:最大值 1:最小值</param>
        /// <returns>极值</returns>
        public virtual double divMaxOrMin(int index, ChartDiv div, int flag) {
            if (index < 0) {
                return 0;
            }
            if (div != null) {
                ArrayList<double> vList = new ArrayList<double>();
                //循环遍历所有的线条
                ArrayList<BaseShape> shapesCopy = div.getShapes(SortType.NONE);
                foreach (BaseShape bs in shapesCopy) {
                    if (!bs.Visible) {
                        continue;
                    }
                    int[] fields = bs.getFields();
                    if (fields != null) {
                        for (int i = 0; i < fields.Length; i++) {
                            //其他线条
                            double value = m_dataSource.get2(index, fields[i]);
                            if (!double.IsNaN(value)) {
                                vList.add(value);
                            }
                        }
                    }
                }
                //返回值
                if (flag == 0) {
                    return FCScript.maxValue(vList.ToArray(), vList.size());
                }
                else {
                    return FCScript.minValue(vList.ToArray(), vList.size());
                }
            }
            return 0;
        }

        /// <summary>
        /// 销毁资源
        /// </summary>
        public override void delete() {
            if (!IsDeleted) {
                stopTimer(m_timerID);
                removeAll();
            }
            base.delete();
        }

        /// <summary>
        /// 由坐标获取层对象，返回图层对象
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>图层对象</returns>
        public virtual ChartDiv findDiv(FCPoint mp) {
            ArrayList<ChartDiv> divsCopy = getDivs();
            foreach (ChartDiv cDiv in divsCopy) {
                if (mp.y >= cDiv.Top && mp.y <= cDiv.Top + cDiv.Height) {
                    return cDiv;
                }
            }
            return null;
        }

        /// <summary>
        /// 由图形名称获取包含它的层，返回图层对象
        /// </summary>
        /// <param name="shape">图形的名称</param>
        /// <returns>图层对象</returns>
        public virtual ChartDiv findDiv(BaseShape shape) {
            ArrayList<ChartDiv> divsCopy = getDivs();
            foreach (ChartDiv div in divsCopy) {
                if (div.containsShape(shape)) {
                    return div;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <returns>控件类型</returns>
        public override String getControlType() {
            return "Chart";
        }

        /// <summary>
        /// 获取图层集合的拷备
        /// </summary>
        /// <returns>图层集合</returns>
        public virtual ArrayList<ChartDiv> getDivs() {
            ArrayList<ChartDiv> divsCopy = new ArrayList<ChartDiv>();
            if (m_divs != null) {
                int divSize = m_divs.size();
                for (int i = 0; i < divSize; i++) {
                    divsCopy.add(m_divs.get(i));
                }
            }
            return divsCopy;
        }

        /// <summary>
        ///  获取横轴的文字
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="lDate">上一日期</param>
        /// <param name="dateType">日期类型</param>
        /// <returns>横轴的文字</returns>
        public virtual String getHScaleDateString(double date, double lDate, ref DateType dateType) {
            int tm_year = 0, tm_mon = 0, tm_mday = 0, tm_hour = 0, tm_min = 0, tm_sec = 0, tm_msec = 0;
            FCStr.getDateByNum(date, ref tm_year, ref tm_mon, ref tm_mday, ref tm_hour, ref tm_min, ref tm_sec, ref tm_msec);
            int l_year = 0, l_mon = 0, l_mday = 0, l_hour = 0, l_min = 0, l_sec = 0, l_msec = 0;
            if (lDate > 0) {
                FCStr.getDateByNum(lDate, ref l_year, ref l_mon, ref l_mday, ref l_hour, ref l_min, ref l_sec, ref l_msec);
            }
            String num = String.Empty;
            //只显示年
            if (tm_year > l_year) {
                dateType = DateType.Year;
                return tm_year.ToString();
            }
            //只显示日
            if (tm_mon > l_mon) {
                dateType = DateType.Month;
                String mStr = tm_mon.ToString();
                if (tm_mon < 10) {
                    mStr = "0" + mStr;
                }
                return mStr;
            }
            //只显示日期
            if (tm_mday > l_mday) {
                dateType = DateType.Day;
                String dStr = tm_mday.ToString();
                if (tm_mday < 10) {
                    dStr = "0" + dStr;
                }
                return dStr;
            }
            //只显示小时和分钟
            if (tm_hour > l_hour || tm_min > l_min) {
                dateType = DateType.Minute;
                String hStr = tm_hour.ToString();
                if (tm_hour < 10) {
                    hStr = "0" + hStr;
                }
                String mStr = tm_min.ToString();
                if (tm_min < 10) {
                    mStr = "0" + mStr;
                }
                return hStr + ":" + mStr;
            }
            //只显示秒
            if (tm_sec > l_sec) {
                dateType = DateType.Second;
                String sStr = tm_sec.ToString();
                if (tm_sec < 10) {
                    sStr = "0" + sStr;
                }
                return sStr;
            }
            //只显示毫秒
            if (tm_msec > l_msec) {
                dateType = DateType.Millisecond;
                return l_msec.ToString();
            }
            return "";
        }

        /// <summary>
        /// 由坐标点获取它对应的索引
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>索引号</returns>
        public virtual int getIndex(FCPoint mp) {
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

        /// <summary>
        /// 获取坐标轴的值
        /// </summary>
        /// <param name="div">图层</param>
        /// <param name="mp">坐标</param>
        /// <param name="attachVScale">对应坐标轴</param>
        /// <returns>指标值</returns>
        public virtual double getNumberValue(ChartDiv div, FCPoint mp, AttachVScale attachVScale) {
            VScale vScale = div.getVScale(attachVScale);
            int vHeight = div.WorkingAreaHeight - vScale.PaddingTop - vScale.PaddingBottom;
            int cY = mp.y - div.Top - div.TitleBar.Height - vScale.PaddingTop;
            if (vScale.Reverse) {
                cY = vHeight - cY;
            }
            if (vHeight > 0) {
                double max = 0;
                double min = 0;
                bool isLog = false;
                //左轴
                if (attachVScale == AttachVScale.Left) {
                    max = div.LeftVScale.VisibleMax;
                    min = div.LeftVScale.VisibleMin;
                    if (max == 0 && min == 0) {
                        max = div.RightVScale.VisibleMax;
                        min = div.RightVScale.VisibleMin;
                    }
                    isLog = div.LeftVScale.System == VScaleSystem.Logarithmic;
                }
                //右轴
                else if (attachVScale == AttachVScale.Right) {
                    max = div.RightVScale.VisibleMax;
                    min = div.RightVScale.VisibleMin;
                    if (max == 0 && min == 0) {
                        max = div.LeftVScale.VisibleMax;
                        min = div.LeftVScale.VisibleMin;
                    }
                    isLog = div.RightVScale.System == VScaleSystem.Logarithmic;
                }
                if (isLog) {
                    if (max >= 0) {
                        max = Math.Log10(max);
                    }
                    else {
                        max = -Math.Log10(Math.Abs(max));
                    }
                    if (min >= 0) {
                        min = Math.Log10(min);
                    }
                    else {
                        min = -Math.Log10(Math.Abs(min));
                    }
                    double value = getVScaleValue(cY, max, min, vHeight);
                    return Math.Pow(10, value);
                }
                else {
                    return getVScaleValue(cY, max, min, vHeight);
                }
            }
            return 0;
        }

        /// <summary>
        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">返回属性值</param>
        /// <param name="type">返回属性类型</param>
        public override void getProperty(String name, ref String value, ref String type) {
            if (name == "autofillhscale") {
                type = "bool";
                value = FCStr.convertBoolToStr(AutoFillHScale);
            }
            else if (name == "blankspace") {
                type = "int";
                value = FCStr.convertIntToStr(BlankSpace);
            }
            else if (name == "canmoveshape") {
                type = "bool";
                value = FCStr.convertBoolToStr(CanMoveShape);
            }
            else if (name == "canresizeh") {
                type = "bool";
                value = FCStr.convertBoolToStr(CanResizeH);
            }
            else if (name == "canresizev") {
                type = "bool";
                value = FCStr.convertBoolToStr(CanResizeV);
            }
            else if (name == "canscroll") {
                type = "bool";
                value = FCStr.convertBoolToStr(CanScroll);
            }
            else if (name == "canzoom") {
                type = "bool";
                value = FCStr.convertBoolToStr(CanZoom);
            }
            else if (name == "crosslinemovemode") {
                type = "enum:CrossLineMoveMode";
                if (CrossLineMoveMode == CrossLineMoveMode.AfterClick) {
                    value = "AfterClick";
                }
                else {
                    value = "FollowTouch";
                }
            }
            else if (name == "hscalefieldtext") {
                type = "text";
                value = HScaleFieldText;
            }
            else if (name == "hscalepixel") {
                type = "double";
                value = FCStr.convertDoubleToStr(HScalePixel);
            }
            else if (name == "leftvscalewidth") {
                type = "int";
                value = FCStr.convertIntToStr(LeftVScaleWidth);
            }
            else if (name == "reversehscale") {
                type = "bool";
                value = FCStr.convertBoolToStr(ReverseHScale);
            }
            else if (name == "rightvscalewidth") {
                type = "int";
                value = FCStr.convertIntToStr(RightVScaleWidth);
            }
            else if (name == "scrolladdspeed") {
                type = "bool";
                value = FCStr.convertBoolToStr(ScrollAddSpeed);
            }
            else if (name == "showcrossline") {
                type = "bool";
                value = FCStr.convertBoolToStr(ShowCrossLine);
            }
            else {
                base.getProperty(name, ref value, ref type);
            }
        }

        /// <summary>
        /// 获取属性名称列表
        /// </summary>
        /// <returns>属性名称列表</returns>
        public override ArrayList<String> getPropertyNames() {
            ArrayList<String> propertyNames = base.getPropertyNames();
            propertyNames.AddRange(new String[] { "AutoFillHScale", "BlankSpace", "CanMoveShape", "CanResizeH", "CanResizeV", "CanScroll",
            "CanZoom", "CrossLineMoveMode", "HScaleFieldText", "HScalePixel", "LeftVScaleWidth", "ReverseHScale", 
                "RightVScaleWidth", "ScrollAddSpeed", "ShowCrossLine"});
            return propertyNames;
        }

        /// <summary>
        /// 由字段获取所有的图形
        /// </summary>
        /// <param name="field">字段</param>
        /// <returns>图形对象的数量</returns>
        public virtual int getShapesCount(int field) {
            int count = 0;
            ArrayList<ChartDiv> divsCopy = getDivs();
            foreach (ChartDiv div in divsCopy) {
                ArrayList<BaseShape> shapesCopy = div.getShapes(SortType.NONE);
                foreach (BaseShape bs in shapesCopy) {
                    int[] fields = bs.getFields();
                    if (fields != null) {
                        for (int i = 0; i < fields.Length; i++) {
                            if (fields[i] == field) {
                                count++;
                            }
                        }
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// 获取触摸所在横向记录索引，索引是从0开始的，最大值为显示条数-1
        /// </summary>
        /// <returns>索引号</returns>
        public virtual int getTouchOverIndex() {
            FCPoint mp = TouchPoint;
            if (m_reverseHScale) {
                mp.x = m_workingAreaWidth - (mp.x - m_leftVScaleWidth) + m_leftVScaleWidth;
            }
            double pixel = m_hScalePixel;
            return getChartIndex(mp.x, m_leftVScaleWidth, pixel, m_firstVisibleIndex);
        }

        /// <summary>
        /// 获取触摸所在的图层
        /// </summary>
        /// <returns>图层</returns>
        public virtual ChartDiv getTouchOverDiv() {
            FCPoint mp = TouchPoint;
            ArrayList<ChartDiv> divsCopy = getDivs();
            foreach (ChartDiv cDiv in divsCopy) {
                if (mp.y >= cDiv.Top && mp.y <= cDiv.Top + cDiv.Height) {
                    return cDiv;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取纵轴的基础字段
        /// </summary>
        /// <param name="div">图层</param>
        /// <param name="vScale">指定纵轴</param>
        /// <returns>基准值</returns>
        public virtual int getVScaleBaseField(ChartDiv div, VScale vScale) {
            int baseField = vScale.BaseField;
            if (baseField == FCDataTable.NULLFIELD) {
                //获取相关线条
                ArrayList<BaseShape> baseShapes = div.getShapes(SortType.ASC);
                if (baseShapes.size() > 0) {
                    baseField = baseShapes[0].getBaseField();
                }
            }
            return baseField;
        }

        /// <summary>
        /// 获取纵轴的基准值
        /// </summary>
        /// <param name="div">图层</param>
        /// <param name="vScale">指定纵轴</param>
        /// <param name="i">索引号</param>
        /// <returns>基准值</returns>
        public virtual double getVScaleBaseValue(ChartDiv div, VScale vScale, int i) {
            double baseValue = 0;
            int baseField = getVScaleBaseField(div, vScale);
            if (baseField != FCDataTable.NULLFIELD && m_dataSource.RowsCount > 0) {
                if (i >= m_firstVisibleIndex && i <= m_lastVisibleIndex) {
                    //获取值
                    double value = m_dataSource.get2(i, baseField);
                    if (!double.IsNaN(value)) {
                        baseValue = value;
                    }
                }
            }
            else {
                baseValue = vScale.MidValue;
            }
            return baseValue;
        }

        /// <summary>
        /// 获取指定索引的横坐标
        /// </summary>
        /// <param name="index">指定索引</param>
        /// <returns>横坐标</returns>
        public virtual float getX(int index) {
            float x = (float)(m_leftVScaleWidth + (index - m_firstVisibleIndex) * m_hScalePixel + m_hScalePixel / 2 + 1);
            if (m_reverseHScale) {
                return m_workingAreaWidth - (x - m_leftVScaleWidth) + m_leftVScaleWidth + m_blankSpace;
            }
            else {
                return x;
            }
        }

        /// <summary>
        /// 获取某一值对应的纵坐标
        /// </summary>
        /// <param name="div">图层</param>
        /// <param name="value">指定的值</param>
        /// <param name="attach">true为左纵轴，false为右纵轴</param>
        /// <returns>纵坐标</returns>
        public virtual float getY(ChartDiv div, double value, AttachVScale attach) {
            if (div != null) {
                VScale scale = div.getVScale(attach);
                double max = scale.VisibleMax;
                double min = scale.VisibleMin;
                //对数坐标系
                if (scale.System == VScaleSystem.Logarithmic) {
                    if (value > 0) {
                        value = Math.Log10(value);
                    }
                    else if (value < 0) {
                        value = -Math.Log10(Math.Abs(value));
                    }
                    if (max > 0) {
                        max = Math.Log10(max);
                    }
                    else if (max < 0) {
                        max = -Math.Log10(Math.Abs(max));
                    }
                    if (min > 0) {
                        min = Math.Log10(scale.VisibleMin);
                    }
                    else if (min < 0) {
                        min = -Math.Log10(Math.Abs(min));
                    }
                }
                //计算坐标
                if (max != min) {
                    int wHeight = div.WorkingAreaHeight - scale.PaddingTop - scale.PaddingBottom;
                    if (wHeight > 0) {
                        float y = (float)((max - value) / (max - min) * wHeight);
                        if (scale.Reverse) {
                            return div.TitleBar.Height + div.WorkingAreaHeight - scale.PaddingBottom - y;
                        }
                        else {
                            return div.TitleBar.Height + scale.PaddingTop + y;
                        }
                    }
                }
            }
            return 0;
        }

        public static int gridScale(double min, double max, int yLen, int maxSpan, int minSpan, int defCount, ref double step, ref int digit) {
            double sub = max - min;
            int nMinCount = (int)Math.Ceiling((double)yLen / maxSpan);
            int nMaxCount = (int)Math.Floor((double)yLen / minSpan);
            int nCount = defCount;
            double logStep = sub / nCount;
            bool start = false;
            double divisor = 0;
            int i = 0, nTemp = 0;
            step = 0;
            digit = 0;
            nCount = Math.Max(nMinCount, nCount);
            nCount = Math.Min(nMaxCount, nCount);
            nCount = Math.Max(nCount, 1);
            for (i = 15; i >= -6; i--) {
                divisor = Math.Pow(10.0, i);
                if (divisor < 1) {
                    digit++;
                }
                nTemp = (int)Math.Floor(logStep / divisor);
                if (start) {
                    if (nTemp < 4) {
                        if (digit > 0) {
                            digit--;
                        }
                    }
                    else if (nTemp >= 4 && nTemp <= 6) {
                        nTemp = 5;
                        step += nTemp * divisor;
                    }
                    else {
                        step += 10 * divisor;
                        if (digit > 0) {
                            digit--;
                        }
                    }
                    break;
                }
                else if (nTemp > 0) {
                    step = nTemp * divisor + step;
                    logStep -= step;
                    start = true;
                }
            }
            return 0;
        }

        /// <summary>
        /// 判断是否正在操作图形
        /// </summary>
        /// <returns>是否正在操作图形</returns>
        public virtual bool isOperating() {
            if (m_movingPlot != null || m_movingShape != null
            || m_hResizeType != 0 || m_userResizeDiv != null) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 定位十字线
        /// </summary>
        public virtual void locateCrossLine() {
            if (m_dataSource.RowsCount > 0) {
                //循环遍历图层
                ArrayList<ChartDiv> divsCopy = getDivs();
                foreach (ChartDiv div in divsCopy) {
                    if (m_cross_y >= div.Top && m_cross_y <= div.Top + div.Height) {
                        if (div.WorkingAreaHeight > 0 && m_crossStopIndex >= 0 && m_crossStopIndex < m_dataSource.RowsCount) {
                            //设置十字线沿K线移动
                            ArrayList<BaseShape> shapesCopy = div.getShapes(SortType.DESC);
                            //设置十字线沿趋势线或成交量移动
                            foreach (BaseShape tls in shapesCopy) {
                                if (tls.Visible) {
                                    double value = m_dataSource.get2(m_crossStopIndex, tls.getBaseField());
                                    if (!double.IsNaN(value)) {
                                        m_cross_y = (int)getY(div, value, tls.AttachVScale) + div.Top;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 将图形移动到另一个层中
        /// </summary>
        /// <param name="cDiv">目标层</param>
        /// <param name="shape">将要移动的图形</param>
        public virtual void moveShape(ChartDiv cDiv, BaseShape shape) {
            //循环遍历所有的层
            ArrayList<ChartDiv> divsCopy = getDivs();
            foreach (ChartDiv div in divsCopy) {
                div.removeShape(shape);
            }
            //移动到新的层
            if (cDiv != null) {
                cDiv.addShape(shape);
            }
        }

        /// <summary>
        /// 重置十字线穿越的记录号
        /// </summary>
        private void resetCrossOverIndex() {
            if (m_showCrossLine) {
                m_crossStopIndex = resetCrossOverIndex(m_dataSource.RowsCount, m_maxVisibleRecord,
                    m_crossStopIndex, m_firstVisibleIndex, m_lastVisibleIndex);
            }
            m_isScrollCross = true;
        }

        /// <summary>
        /// 重置图像，删除所有的数据，层，指标和画线工具等
        /// </summary>
        public virtual void removeAll() {
            clear();
            if (m_divs != null) {
                foreach (ChartDiv div in m_divs) {
                    div.delete();
                }
                m_divs.clear();
            }
            m_dataSource.clear();
            m_cross_y = -1;
            m_showingToolTip = false;
        }

        /// <summary>
        /// 移除图层
        /// </summary>
        /// <param name="div">图层</param>
        public virtual void removeDiv(ChartDiv div) {
            m_divs.remove(div);
            update();
        }

        /// <summary>
        /// 拖动图层改变大小
        /// </summary>
        public virtual bool resizeDiv() {
            int width = Width, height = Height;
            //横向改变大小
            if (m_hResizeType > 0) {
                FCPoint mp = TouchPoint;
                //左轴
                if (m_hResizeType == 1) {
                    if (mp.x > 0 && mp.x < width - m_rightVScaleWidth - 50) {
                        m_leftVScaleWidth = mp.x;
                    }
                }
                //右轴
                else if (m_hResizeType == 2) {
                    if (mp.x > m_leftVScaleWidth + 50 && mp.x < width) {
                        m_rightVScaleWidth = width - mp.x;
                    }
                }
                m_hResizeType = 0;
                update();
                return true;
            }
            //纵向改变
            if (m_userResizeDiv != null) {
                FCPoint mp = TouchPoint;
                ChartDiv nextCP = null;
                bool rightP = false;
                //循环遍历
                ArrayList<ChartDiv> divsCopy = getDivs();
                foreach (ChartDiv cDiv in divsCopy) {
                    if (rightP) {
                        nextCP = cDiv;
                        break;
                    }
                    if (cDiv == m_userResizeDiv) {
                        rightP = true;
                    }
                }
                float sumPercent = 0;
                //循环遍历所有层
                foreach (ChartDiv div in divsCopy) {
                    sumPercent += div.VerticalPercent;
                }
                float originalVP = m_userResizeDiv.VerticalPercent;
                FCRect uRect = m_userResizeDiv.Bounds;
                //缩小
                if (mp.x >= uRect.left && mp.x <= uRect.right
                    && mp.y >= uRect.top && mp.y <= uRect.bottom) {
                    m_userResizeDiv.VerticalPercent = sumPercent * (mp.y - m_userResizeDiv.Top) / Height;
                    if (m_userResizeDiv.VerticalPercent < 1) {
                        m_userResizeDiv.VerticalPercent = 1;
                    }
                    if (nextCP != null) {
                        nextCP.VerticalPercent += originalVP - m_userResizeDiv.VerticalPercent;
                    }
                }
                //放大
                else {
                    if (nextCP != null) {
                        FCRect nRect = nextCP.Bounds;
                        if (mp.x >= nRect.left && mp.x <= nRect.right
                        && mp.y >= nRect.top && mp.y <= nRect.bottom) {
                            m_userResizeDiv.VerticalPercent = sumPercent * (mp.y - m_userResizeDiv.Top) / Height;
                            if (m_userResizeDiv.VerticalPercent >= originalVP + nextCP.VerticalPercent) {
                                m_userResizeDiv.VerticalPercent -= 1;
                            }
                            nextCP.VerticalPercent = originalVP + nextCP.VerticalPercent - m_userResizeDiv.VerticalPercent;
                        }
                    }
                }
                m_userResizeDiv = null;
                update();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void reset() {
            if (Visible) {
                resetVisibleRecord();
                adjust();
                resetCrossOverIndex();
            }
        }

        /// <summary>
        /// 自动设置首先可见和最后可见的记录号
        /// </summary>
        public virtual void resetVisibleRecord() {
            ArrayList<ChartDiv> divs = getDivs();
            if (divs.size() > 0) {
                int rowsCount = m_dataSource.RowsCount;
                if (m_autoFillHScale) {
                    if (m_workingAreaWidth > 0 && rowsCount > 0) {
                        m_hScalePixel = (double)m_workingAreaWidth / rowsCount;
                        m_maxVisibleRecord = rowsCount;
                        m_firstVisibleIndex = 0;
                        m_lastVisibleIndex = rowsCount - 1;
                    }
                }
                else {
                    m_maxVisibleRecord = getMaxVisibleCount(m_hScalePixel, m_workingAreaWidth);
                    //没数据时重置
                    if (rowsCount == 0) {
                        m_firstVisibleIndex = -1;
                        m_lastVisibleIndex = -1;
                    }
                    else {
                        //数据不足一屏时
                        if (rowsCount < m_maxVisibleRecord) {
                            m_lastVisibleIndex = rowsCount - 1;
                            m_firstVisibleIndex = 0;
                        }
                        //数据超过一屏时
                        else {
                            //显示中间的数据时
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
                            }
                            else {
                                //第一条或最后一条数据被显示时
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

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        public override void setProperty(String name, String value) {
            if (name == "autofillhscale") {
                AutoFillHScale = FCStr.convertStrToBool(value);
            }
            else if (name == "blankspace") {
                BlankSpace = FCStr.convertStrToInt(value);
            }
            else if (name == "canmoveshape") {
                CanMoveShape = FCStr.convertStrToBool(value);
            }
            else if (name == "canresizeh") {
                CanResizeH = FCStr.convertStrToBool(value);
            }
            else if (name == "canresizev") {
                CanResizeV = FCStr.convertStrToBool(value);
            }
            else if (name == "canscroll") {
                CanScroll = FCStr.convertStrToBool(value);
            }
            else if (name == "canzoom") {
                CanZoom = FCStr.convertStrToBool(value);
            }
            else if (name == "crosslinemovemode") {
                if (value == "AfterClick") {
                    CrossLineMoveMode = CrossLineMoveMode.AfterClick;
                }
                else {
                    CrossLineMoveMode = CrossLineMoveMode.FollowTouch;
                }
            }
            else if (name == "hscalefieldtext") {
                HScaleFieldText = value;
            }
            else if (name == "hscalepixel") {
                HScalePixel = FCStr.convertStrToDouble(value);
            }
            else if (name == "leftvscalewidth") {
                LeftVScaleWidth = FCStr.convertStrToInt(value);
            }
            else if (name == "reversehscale") {
                ReverseHScale = FCStr.convertStrToBool(value);
            }
            else if (name == "rightvscalewidth") {
                RightVScaleWidth = FCStr.convertStrToInt(value);
            }
            else if (name == "scrolladdspeed") {
                ScrollAddSpeed = FCStr.convertStrToBool(value);
            }
            else if (name == "showcrossline") {
                ShowCrossLine = FCStr.convertStrToBool(value);
            }
            else {
                base.setProperty(name, value);
            }
        }

        /// <summary>
        /// 左滚十字线，仅在显示十字线时有效
        /// </summary>
        /// <param name="step">幅度，即滚动跨越的记录条数</param>
        private void scrollCrossLineLeft(int step) {
            int currentIndex = m_crossStopIndex;
            m_crossStopIndex = currentIndex - step;
            if (m_crossStopIndex < 0) {
                m_crossStopIndex = 0;
            }
            if (currentIndex <= m_firstVisibleIndex) {
                scrollLeft(step);
            }
        }

        /// <summary>
        /// 右滚十字线，仅在显示十字线时有效
        /// </summary>
        /// <param name="step">幅度，即滚动跨越的记录条数</param>
        public virtual void scrollCrossLineRight(int step) {
            int currentIndex = m_crossStopIndex;
            m_crossStopIndex = currentIndex + step;
            if (m_dataSource.RowsCount < m_maxVisibleRecord) {
                if (m_crossStopIndex >= m_maxVisibleRecord - 1) {
                    m_crossStopIndex = m_maxVisibleRecord - 1;
                }
            }
            if (currentIndex >= m_lastVisibleIndex) {
                scrollRight(step);
            }
        }

        /// <summary>
        /// 左滚画面
        /// </summary>
        /// <param name="step">幅度，即滚动跨越的记录条数</param>
        public virtual void scrollLeft(int step) {
            if (!m_autoFillHScale) {
                scrollLeft(step, m_dataSource.RowsCount, m_hScalePixel, m_workingAreaWidth,
                    ref m_firstVisibleIndex, ref m_lastVisibleIndex);
                checkLastVisibleIndex();
            }
        }

        /// <summary>
        /// 立即向左滚动到显示第一条数据为止
        /// </summary>
        public virtual void scrollLeftToBegin() {
            if (!m_autoFillHScale && m_dataSource.RowsCount > 0) {
                m_firstVisibleIndex = 0;
                m_lastVisibleIndex = m_maxVisibleRecord - 1;
                checkLastVisibleIndex();
                m_crossStopIndex = m_firstVisibleIndex;
            }
        }

        /// <summary>
        /// 右滚画面
        /// </summary>
        /// <param name="step">幅度，即滚动跨越的记录条数</param>
        public virtual void scrollRight(int step) {
            if (!m_autoFillHScale) {
                scrollRight(step, m_dataSource.RowsCount, m_hScalePixel, m_workingAreaWidth,
                    ref m_firstVisibleIndex, ref m_lastVisibleIndex);
                checkLastVisibleIndex();
            }
        }

        /// <summary>
        /// 立即向右滚动到显示最后一条数据为止
        /// </summary>
        public virtual void scrollRightToEnd() {
            if (!m_autoFillHScale && m_dataSource.RowsCount > 0) {
                m_lastVisibleIndex = m_dataSource.RowsCount - 1;
                checkLastVisibleIndex();
                m_firstVisibleIndex = m_lastVisibleIndex - m_maxVisibleRecord + 1;
                if (m_firstVisibleIndex < 0) {
                    m_firstVisibleIndex = 0;
                }
                m_crossStopIndex = m_lastVisibleIndex;
            }
        }

        /// <summary>
        /// 判断是否选中柱状图
        /// </summary>
        /// <param name="div">图层</param>
        /// <param name="mpY">触摸所在纵坐标</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="fieldName2">字段名2</param>
        /// <param name="styleField">样式字段</param>
        /// <param name="attachVScale">依附坐标轴</param>
        /// <param name="curIndex">当前索引</param>
        /// <returns>是否选中</returns>
        public virtual bool selectBar(ChartDiv div, float mpY, int fieldName, int fieldName2, int styleField, AttachVScale attachVScale, int curIndex) {
            int style = 1;
            //自定义样式
            if (styleField != FCDataTable.NULLFIELD) {
                double defineStyle = m_dataSource.get2(curIndex, styleField);
                if (!double.IsNaN(defineStyle)) {
                    style = (int)defineStyle;
                }
            }
            if (style == -10000 || curIndex < 0 || curIndex > m_lastVisibleIndex || double.IsNaN(m_dataSource.get2(curIndex, fieldName))) {
                return false;
            }
            double midValue = 0;
            if (fieldName2 != FCDataTable.NULLFIELD) {
                midValue = m_dataSource.get2(curIndex, fieldName2);
            }
            double volumn = m_dataSource.get2(curIndex, fieldName);
            float y = getY(div, volumn, attachVScale);
            float midY = getY(div, midValue, attachVScale);
            float topY = y + div.Top;
            float bottomY = midY + div.Top;
            //修正
            if (topY > bottomY) {
                topY = midY + div.Top;
                bottomY = y + div.Top;
            }
            topY -= 1;
            bottomY += 1;
            //垂直区域判断
            if (topY >= div.Top && bottomY <= div.Top + div.Height
                && mpY >= topY && mpY <= bottomY) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断是否选中K线
        /// </summary>
        /// <param name="div">图层</param>
        /// <param name="mpY">触摸纵坐标</param>
        /// <param name="highField">最高价字段</param>
        /// <param name="lowField">最低价字段</param>
        /// <param name="styleField">样式字段</param>
        /// <param name="attachVScale">依附坐标轴</param>
        /// <param name="curIndex">当前索引</param>
        /// <returns>是否选中</returns>
        public virtual bool selectCandle(ChartDiv div, float mpY, int highField, int lowField, int styleField, AttachVScale attachVScale, int curIndex) {
            int style = 1;
            //自定义样式
            if (styleField != FCDataTable.NULLFIELD) {
                double defineStyle = m_dataSource.get2(curIndex, styleField);
                if (!double.IsNaN(defineStyle)) {
                    style = (int)defineStyle;
                }
            }
            double highValue = 0;
            double lowValue = 0;
            if (style == -10000 || curIndex < 0 || curIndex > m_lastVisibleIndex) {
                return false;
            }
            else {
                highValue = m_dataSource.get2(curIndex, highField);
                lowValue = m_dataSource.get2(curIndex, lowField);
                if (double.IsNaN(highValue) || double.IsNaN(lowValue)) {
                    return false;
                }
            }
            float highY = getY(div, highValue, attachVScale);
            float lowY = getY(div, lowValue, attachVScale);
            //垂直判断
            float topY = highY + div.Top;
            float bottomY = lowY + div.Top;
            //修正坐标
            if (topY > bottomY) {
                float temp = topY;
                topY = bottomY;
                bottomY = temp;
            }
            topY -= 1;
            bottomY += 1;
            if (topY >= div.Top && bottomY <= div.Top + div.Height
                && mpY >= topY && mpY <= bottomY) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断是否选中线
        /// </summary>
        /// <param name="div">图层</param>
        /// <param name="mp">触摸位置</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="lineWidth">线的宽度</param>
        /// <param name="attachVScale">依附坐标轴</param>
        /// <param name="curIndex">当前索引</param>
        /// <returns>是否选中</returns>
        public virtual bool selectPolyline(ChartDiv div, FCPoint mp, int fieldName, float lineWidth, AttachVScale attachVScale, int curIndex) {
            if (curIndex < 0 || curIndex > m_lastVisibleIndex || (double.IsNaN(m_dataSource.get2(curIndex, fieldName)))) {
                return false;
            }
            double lineValue = (m_dataSource.get2(curIndex, fieldName));
            float topY = getY(div, lineValue, attachVScale) + div.Top;
            //间隔较小
            if (m_hScalePixel <= 1) {
                if (topY >= div.Top && topY <= div.Top + div.Height
                && mp.y >= topY - 8 && mp.y <= topY + 8) {
                    return true;
                }
            }
            //间隔较大
            else {
                int index = curIndex;
                int scaleX = (int)getX(index);
                float judgeTop = 0;
                float judgeScaleX = scaleX;
                //左侧判断
                if (mp.x >= scaleX) {
                    int leftIndex = curIndex + 1;
                    if (curIndex < m_lastVisibleIndex && (!double.IsNaN(m_dataSource.get2(leftIndex, fieldName)))) {
                        double rightValue = m_dataSource.get2(leftIndex, fieldName);
                        judgeTop = getY(div, rightValue, attachVScale) + div.Top;
                        if (judgeTop > div.Top + div.Height - div.HScale.Height || judgeTop < div.Top + div.TitleBar.Height) {
                            return false;
                        }
                    }
                    else {
                        judgeTop = topY;
                    }
                }
                //右侧判断
                else {
                    judgeScaleX = scaleX - (int)m_hScalePixel;
                    int rightIndex = curIndex - 1;
                    if (curIndex > 0 && (!double.IsNaN(m_dataSource.get2(rightIndex, fieldName)))) {
                        double leftValue = (m_dataSource.get2(rightIndex, fieldName));
                        judgeTop = getY(div, leftValue, attachVScale) + div.Top;
                        if (judgeTop > div.Top + div.Height - div.HScale.Height || judgeTop < div.Top + div.TitleBar.Height) {
                            return false;
                        }
                    }
                    else {
                        judgeTop = topY;
                    }
                }
                float judgeX = 0, judgeY = 0, judgeW = 0, judgeH = 0;
                if (judgeTop >= topY) {
                    judgeX = judgeScaleX;
                    judgeY = topY - 2 - lineWidth;
                    judgeW = (float)m_hScalePixel;
                    judgeH = judgeTop - topY + lineWidth < 4 ? 4 : judgeTop - topY + 4 + lineWidth;
                }
                else {
                    judgeX = judgeScaleX;
                    judgeY = judgeTop - 2 - lineWidth / 2;
                    judgeW = (float)m_hScalePixel;
                    judgeH = topY - judgeTop + lineWidth < 4 ? 4 : topY - judgeTop + 4 + lineWidth;
                }
                if (mp.x >= judgeX && mp.x <= judgeX + judgeW && mp.y >= judgeY && mp.y <= judgeY + judgeH) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 选中线条的方法
        /// </summary>
        /// <param name="curIndex">当前选中的索引号</param>
        /// <param name="state">状态 0:只判断不选中 1:单选</param>
        /// <returns>线条</returns>
        public virtual BaseShape selectShape(int curIndex, int state) {
            BaseShape selectObj = null;
            //获取选中点
            FCPoint mp = TouchPoint;
            //循环遍历所有的Div
            ArrayList<ChartDiv> divsCopy = getDivs();
            foreach (ChartDiv div in divsCopy) {
                //按照顺序获取线条
                ArrayList<BaseShape> sortedBs = div.getShapes(SortType.DESC);
                //循环遍历线条
                foreach (BaseShape bShape in sortedBs) {
                    if (bShape.Visible) {
                        if (selectObj != null) {
                            if (state == 1) {
                                bShape.Selected = false;
                            }
                        }
                        else {
                            if (m_firstVisibleIndex == -1 || m_lastVisibleIndex == -1) {
                                if (state == 1) {
                                    bShape.Selected = false;
                                }
                                continue;
                            }
                            //点图
                            bool isSelect = false;
                            //线图
                            if (bShape is PolylineShape) {
                                PolylineShape tls = bShape as PolylineShape;
                                isSelect = selectPolyline(div, mp, tls.getBaseField(), tls.Width, tls.AttachVScale, curIndex);
                            }
                            //K线
                            else if (bShape is CandleShape) {
                                CandleShape cs = bShape as CandleShape;
                                //收盘线
                                if (cs.Style == CandleStyle.CloseLine) {
                                    isSelect = selectPolyline(div, mp, cs.CloseField, 1, cs.AttachVScale, curIndex);
                                }
                                //其他类型
                                else {
                                    isSelect = selectCandle(div, mp.y, cs.HighField, cs.LowField, cs.StyleField, cs.AttachVScale, curIndex);
                                }
                            }
                            //柱状图
                            else if (bShape is BarShape) {
                                BarShape barS = bShape as BarShape;
                                isSelect = selectBar(div, mp.y, barS.FieldName, barS.FieldName2, barS.StyleField, barS.AttachVScale, curIndex);
                            }
                            //保存选中
                            if (isSelect) {
                                selectObj = bShape;
                                if (state == 1) {
                                    bShape.Selected = true;
                                }
                            }
                            else {
                                if (state == 1) {
                                    bShape.Selected = false;
                                }
                            }
                        }
                    }
                }
            }
            return selectObj;
        }

        /// <summary>
        /// 设置图形首先可见的索引号和最后可见的索引号
        /// </summary>
        /// <param name="firstVisibleIndex">开始记录号</param>
        /// <param name="lastVisibleIndex">结束记录号</param>
        public virtual void setVisibleIndex(int firstVisibleIndex, int lastVisibleIndex) {
            double xScalePixel = (double)m_workingAreaWidth / (lastVisibleIndex - firstVisibleIndex + 1);
            if (xScalePixel < 1000000) {
                m_firstVisibleIndex = firstVisibleIndex;
                m_lastVisibleIndex = lastVisibleIndex;
                //设置最后一条记录是否可见
                if (lastVisibleIndex != m_dataSource.RowsCount - 1) {
                    m_lastRecordIsVisible = false;
                }
                else {
                    m_lastRecordIsVisible = true;
                }
                HScalePixel = xScalePixel;
                m_maxVisibleRecord = getMaxVisibleCount(m_hScalePixel, m_workingAreaWidth);
                checkLastVisibleIndex();
            }
        }

        /// <summary>
        /// 重新布局 
        /// </summary>
        public override void update() {
            base.update();
            m_workingAreaWidth = Width - m_leftVScaleWidth - m_rightVScaleWidth - m_blankSpace - 1;
            if (m_workingAreaWidth < 0) {
                m_workingAreaWidth = 0;
            }
            int locationY = 0;
            float sumPercent = 0;
            //循环遍历所有层
            ArrayList<ChartDiv> divsCopy = getDivs();
            foreach (ChartDiv div in divsCopy) {
                sumPercent += div.VerticalPercent;
            }
            if (sumPercent > 0) {
                //调整纵向的位置
                foreach (ChartDiv cDiv in divsCopy) {
                    cDiv.Bounds = new FCRect(0, locationY, Width, locationY + (int)(Height * cDiv.VerticalPercent / sumPercent));
                    cDiv.WorkingAreaHeight = cDiv.Height - cDiv.HScale.Height - cDiv.TitleBar.Height - 1;
                    locationY += (int)(Height * cDiv.VerticalPercent / sumPercent);
                }
            }
            reset();
        }

        /// <summary>
        /// 放大
        /// </summary>
        public virtual void zoomOut() {
            if (!m_autoFillHScale) {
                double hp = m_hScalePixel;
                zoomOut(m_workingAreaWidth, m_dataSource.RowsCount, ref m_firstVisibleIndex, ref m_lastVisibleIndex, ref hp);
                HScalePixel = hp;
                m_maxVisibleRecord = getMaxVisibleCount(m_hScalePixel, m_workingAreaWidth);
                checkLastVisibleIndex();
            }
        }

        /// <summary>
        /// 缩小
        /// </summary>
        public virtual void zoomIn() {
            if (!m_autoFillHScale) {
                double hp = m_hScalePixel;
                zoomIn(m_workingAreaWidth, m_dataSource.RowsCount, ref m_firstVisibleIndex, ref m_lastVisibleIndex, ref hp);
                HScalePixel = hp;
                m_maxVisibleRecord = getMaxVisibleCount(m_hScalePixel, m_workingAreaWidth);
                checkLastVisibleIndex();
            }
        }

        /// <summary>
        /// 绘制文字
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="text">文字</param>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="font">字体</param>
        /// <param name="point">坐标</param>
        public virtual void drawText(FCPaint paint, String text, long dwPenColor, FCFont font, FCPoint point) {
            FCSize tSize = paint.textSize(text, font);
            FCRect tRect = new FCRect(point.x, point.y, point.x + tSize.cx, point.y + tSize.cy);
            paint.drawText(text, dwPenColor, font, tRect);
        }

        /// <summary>
        /// 画细线，只能是水平线或垂直线
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="color">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="x1">第一个点的横坐标</param>
        /// <param name="y1">第一个点的纵坐标</param>
        /// <param name="x2">第二个点的横坐标</param>
        /// <param name="y2">第二个点的纵坐标</param>
        public virtual void drawThinLine(FCPaint paint, long color, int width, float x1, float y1, float x2, float y2) {
            FCHost host = Native.Host;
            if (width > 1 || Native.AllowScaleSize) {
                paint.drawLine(color, width, 0, (int)x1, (int)y1, (int)x2, (int)y2);
            }
            else {
                int x = (int)(x1 < x2 ? x1 : x2);
                int y = (int)(y1 < y2 ? y1 : y2);
                int w = (int)Math.Abs(x1 - x2);
                int h = (int)Math.Abs(y1 - y2);
                if (w < 1) w = 1;
                if (h < 1) h = 1;
                if ((w > 1 && h == 1) || (w == 1 && h > 1)) {
                    paint.fillRect(color, x, y, x + w, y + h);
                }
                else {
                    paint.drawLine(color, width, 0, (int)x1, (int)y1, (int)x2, (int)y2);
                }
            }
        }

        /// <summary>
        /// 获取纵轴的刻度
        /// </summary>
        /// <param name="max">最大值</param>
        /// <param name="min">最小值</param>
        /// <param name="div">图层</param>
        /// <param name="vScale">坐标轴</param>
        /// <returns>刻度集合</returns>
        public virtual ArrayList<double> getVScaleStep(double max, double min, ChartDiv div, VScale vScale) {
            ArrayList<double> scaleStepList = new ArrayList<double>();
            //等差，百分比
            if (vScale.Type == VScaleType.EqualDiff || vScale.Type == VScaleType.Percent) {
                double step = 0;
                int distance = div.VGrid.Distance;
                int digit = 0, gN = div.WorkingAreaHeight / distance;
                if (gN == 0) {
                    gN = 1;
                }
                //计算显示值
                gridScale(min, max, div.WorkingAreaHeight, distance, distance / 2, gN, ref step, ref digit);
                if (step > 0) {
                    double start = 0;
                    if (min >= 0) {
                        while (start + step < min) {
                            start += step;
                        }
                    }
                    else {
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
            //等比
            else if (vScale.Type == VScaleType.EqualRatio) {
                //获取基础字段
                int baseField = getVScaleBaseField(div, vScale);
                double bMax = double.MinValue;
                double bMin = double.MaxValue;
                if (baseField != -1) {
                    //循环遍历数据
                    for (int i = 0; i < m_dataSource.RowsCount; i++) {
                        double value = m_dataSource.get2(i, baseField);
                        if (!double.IsNaN(value)) {
                            if (value > bMax) {
                                bMax = value;
                            }
                            if (value < bMin) {
                                bMin = value;
                            }
                        }
                    }
                    //生成坐标刻度
                    if (bMax != double.MinValue && bMin != double.MaxValue && bMin > 0 && bMax > 0 && bMin < bMax) {
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
            //等分
            else if (vScale.Type == VScaleType.Divide) {
                scaleStepList.add(min + (max - min) * 0.25);
                scaleStepList.add(min + (max - min) * 0.5);
                scaleStepList.add(min + (max - min) * 0.75);
            }
            //黄金分割
            else if (vScale.Type == VScaleType.GoldenRatio) {
                scaleStepList.add(min);
                scaleStepList.add(min + (max - min) * 0.191);
                scaleStepList.add(min + (max - min) * 0.382);
                scaleStepList.add(min + (max - min) * 0.5);
                scaleStepList.add(min + (max - min) * 0.618);
                scaleStepList.add(min + (max - min) * 0.809);
            }
            if ((max != min) && scaleStepList.size() == 0) {
                if (!double.IsNaN(min)) {
                    scaleStepList.add(min);
                }
            }
            return scaleStepList;
        }

        /// <summary>
        /// 绘制成交量
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="div">要绘制的层</param>
        /// <param name="bs">线条对象</param>
        public void onPaintBar(FCPaint paint, ChartDiv div, BarShape bs) {
            int ciFieldName1 = m_dataSource.getColumnIndex(bs.FieldName);
            int ciFieldName2 = m_dataSource.getColumnIndex(bs.FieldName2);
            int ciStyle = m_dataSource.getColumnIndex(bs.StyleField);
            int ciClr = m_dataSource.getColumnIndex(bs.ColorField);
            int defaultLineWidth = 1;
            if (!isOperating() && m_crossStopIndex != -1) {
                if (selectBar(div, TouchPoint.y, bs.FieldName, bs.FieldName2, bs.StyleField, bs.AttachVScale, m_crossStopIndex)) {
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
                switch (bs.Style) {
                    case BarStyle.Line:
                        style = 2;
                        break;
                    case BarStyle.Rect:
                        style = 0;
                        break;
                }
                //自定义样式
                if (ciStyle != -1) {
                    double defineStyle = m_dataSource.get3(i, ciStyle);
                    if (!double.IsNaN(defineStyle)) {
                        style = (int)defineStyle;
                    }
                }
                if (style == -10000) {
                    continue;
                }
                double value = m_dataSource.get3(i, ciFieldName1);
                int scaleX = (int)getX(i);
                double midValue = 0;
                if (ciFieldName2 != -1) {
                    midValue = m_dataSource.get3(i, ciFieldName2);
                }
                float midY = getY(div, midValue, bs.AttachVScale);
                if (!double.IsNaN(value)) {
                    float barY = getY(div, value, bs.AttachVScale);
                    int startPX = scaleX;
                    int startPY = (int)midY;
                    int endPX = scaleX;
                    int endPY = (int)barY;
                    if (bs.Style == BarStyle.Rect) {
                        //修正
                        if (startPY == div.Height - div.HScale.Height) {
                            startPY = div.Height - div.HScale.Height + 1;
                        }
                    }
                    int x = 0, y = 0, width = 0, height = 0;
                    width = (int)(m_hScalePixel * 2 / 3);
                    if (width % 2 == 0) {
                        width += 1;
                    }
                    if (width < 3) {
                        width = 1;
                    }
                    x = scaleX - width / 2;
                    //获取阴阳柱的矩形
                    if (startPY >= endPY) {
                        y = endPY;
                    }
                    else {
                        y = startPY;
                    }
                    height = Math.Abs(startPY - endPY);
                    if (height < 1) {
                        height = 1;
                    }
                    //获取自定义颜色
                    long barColor = FCColor.None;
                    if (ciClr != -1) {
                        double defineColor = m_dataSource.get3(i, ciClr);
                        if (!double.IsNaN(defineColor)) {
                            barColor = (long)defineColor;
                        }
                    }
                    if (barColor == FCColor.None) {
                        if (startPY >= endPY) {
                            barColor = bs.UpColor;
                        }
                        else {
                            barColor = bs.DownColor;
                        }
                    }
                    switch (style) {
                        //虚线空心矩形
                        case -1:
                            if (m_hScalePixel <= 3) {
                                drawThinLine(paint, barColor, thinLineWidth, startPX, y, startPX, y + height);
                            }
                            else {
                                FCRect rect = new FCRect(x, y, x + width, y + height);
                                paint.fillRect(div.BackColor, rect);
                                paint.drawRect(barColor, thinLineWidth, 2, rect);
                            }
                            break;
                        //实心矩形
                        case 0:
                            if (m_hScalePixel <= 3) {
                                drawThinLine(paint, barColor, thinLineWidth, startPX, y, startPX, y + height);
                            }
                            else {
                                FCRect rect = new FCRect(x, y, x + width, y + height);
                                paint.fillRect(barColor, rect);
                                if (thinLineWidth > 1) {
                                    if (startPY >= endPY) {
                                        paint.drawRect(bs.DownColor, thinLineWidth, 0, rect);
                                    }
                                    else {
                                        paint.drawRect(bs.UpColor, thinLineWidth, 0, rect);
                                    }
                                }
                            }
                            break;
                        //空心矩形
                        case 1:
                            if (m_hScalePixel <= 3) {
                                drawThinLine(paint, barColor, thinLineWidth, startPX, y, startPX, y + height);
                            }
                            else {
                                FCRect rect = new FCRect(x, y, x + width, y + height);
                                paint.fillRect(div.BackColor, rect);
                                paint.drawRect(barColor, thinLineWidth, 0, rect);
                            }
                            break;
                        //线
                        case 2:
                            if (startPY <= 0) {
                                startPY = 0;
                            }
                            if (startPY >= div.Height) {
                                startPY = div.Height;
                            }
                            if (endPY <= 0) {
                                endPY = 0;
                            }
                            if (endPY >= div.Height) {
                                endPY = div.Height;
                            }
                            //画线
                            if (bs.LineWidth <= 1) {
                                drawThinLine(paint, barColor, thinLineWidth, startPX, startPY, endPX, endPY);
                            }
                            else {
                                float lineWidth = bs.LineWidth;
                                if (lineWidth > m_hScalePixel) {
                                    lineWidth = (float)m_hScalePixel;
                                }
                                if (lineWidth < 1) {
                                    lineWidth = 1;
                                }
                                paint.drawLine(barColor, lineWidth + thinLineWidth - 1, 0, startPX, startPY, endPX, endPY);
                            }
                            break;
                    }
                    if (bs.Selected) {
                        //画选中框
                        int kPInterval = m_maxVisibleRecord / 30;
                        if (kPInterval < 2) {
                            kPInterval = 2;
                        }
                        if (i % kPInterval == 0) {
                            if (barY >= div.TitleBar.Height
                                && barY <= div.Height - div.HScale.Height) {
                                FCRect sRect = new FCRect(scaleX - 3, (int)barY - 4, scaleX + 4, (int)barY + 3);
                                paint.fillRect(bs.getSelectedColor(), sRect);
                            }
                        }
                    }
                }
                //画零线
                if (i == m_lastVisibleIndex && div.getVScale(bs.AttachVScale).VisibleMin < 0) {
                    if (m_reverseHScale) {
                        float left = (float)(m_leftVScaleWidth + m_workingAreaWidth - (m_lastVisibleIndex - m_firstVisibleIndex + 1) * m_hScalePixel);
                        paint.drawLine(bs.DownColor, 1, 0, m_leftVScaleWidth + m_workingAreaWidth, (int)midY, (int)left, (int)midY);
                    }
                    else {
                        float right = (float)(m_leftVScaleWidth + (m_lastVisibleIndex - m_firstVisibleIndex + 1) * m_hScalePixel);
                        paint.drawLine(bs.DownColor, 1, 0, m_leftVScaleWidth, (int)midY, (int)right, (int)midY);
                    }
                }
            }
        }

        /// <summary>
        /// 绘制K线
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="div">要绘制的层</param>
        /// <param name="cs">K线</param>
        public virtual void onPaintCandle(FCPaint paint, ChartDiv div, CandleShape cs) {
            int visibleMaxIndex = -1, visibleMinIndex = -1;
            double visibleMax = 0, visibleMin = 0;
            int x = 0, y = 0;
            ArrayList<FCPoint> points = new ArrayList<FCPoint>();
            int ciHigh = m_dataSource.getColumnIndex(cs.HighField);
            int ciLow = m_dataSource.getColumnIndex(cs.LowField);
            int ciOpen = m_dataSource.getColumnIndex(cs.OpenField);
            int ciClose = m_dataSource.getColumnIndex(cs.CloseField);
            int ciStyle = m_dataSource.getColumnIndex(cs.StyleField);
            int ciClr = m_dataSource.getColumnIndex(cs.ColorField);
            int defaultLineWidth = 1;
            if (!isOperating() && m_crossStopIndex != -1) {
                if (selectCandle(div, TouchPoint.y, cs.HighField, cs.LowField, cs.StyleField, cs.AttachVScale, m_crossStopIndex)) {
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
                switch (cs.Style) {
                    case CandleStyle.Rect:
                        style = 0;
                        break;
                    case CandleStyle.American:
                        style = 3;
                        break;
                    case CandleStyle.CloseLine:
                        style = 4;
                        break;
                    case CandleStyle.Tower:
                        style = 5;
                        break;
                }
                //自定义样式
                if (ciStyle != -1) {
                    double defineStyle = m_dataSource.get3(i, ciStyle);
                    if (!double.IsNaN(defineStyle)) {
                        style = (int)defineStyle;
                    }
                }
                if (style == 10000) {
                    continue;
                }
                //获取值
                double open = m_dataSource.get3(i, ciOpen);
                double high = m_dataSource.get3(i, ciHigh);
                double low = m_dataSource.get3(i, ciLow);
                double close = m_dataSource.get3(i, ciClose);
                if (double.IsNaN(open) || double.IsNaN(high) || double.IsNaN(low) || double.IsNaN(close)) {
                    if (i != m_lastVisibleIndex || style != 4) {
                        continue;
                    }
                }
                int scaleX = (int)getX(i);
                if (cs.ShowMaxMin) {
                    //设置可见部分最大最小值及索引
                    if (i == m_firstVisibleIndex) {
                        //初始值
                        visibleMaxIndex = i;
                        visibleMinIndex = i;
                        visibleMax = high;
                        visibleMin = low;
                    }
                    else {
                        //最大值
                        if (high > visibleMax) {
                            visibleMax = high;
                            visibleMaxIndex = i;
                        }
                        //最小值
                        if (low < visibleMin) {
                            visibleMin = low;
                            visibleMinIndex = i;
                        }
                    }
                }
                //获取各值所在Y值
                float highY = getY(div, high, cs.AttachVScale);
                float openY = getY(div, open, cs.AttachVScale);
                float lowY = getY(div, low, cs.AttachVScale);
                float closeY = getY(div, close, cs.AttachVScale);
                int cwidth = (int)(m_hScalePixel * 2 / 3);
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
                    //美国线
                    case 3: {
                            long color = cs.UpColor;
                            if (open > close) {
                                color = cs.DownColor;
                            }
                            if (ciClr != -1) {
                                double defineColor = m_dataSource.get3(i, ciClr);
                                if (!double.IsNaN(defineColor)) {
                                    color = (long)defineColor;
                                }
                            }
                            if ((int)highY != (int)lowY) {
                                if (m_hScalePixel <= 3) {
                                    drawThinLine(paint, color, thinLineWidth, scaleX, highY, scaleX, lowY);
                                }
                                else {
                                    drawThinLine(paint, color, thinLineWidth, scaleX, highY, scaleX, lowY);
                                    drawThinLine(paint, color, thinLineWidth, scaleX - xsub, openY, scaleX, openY);
                                    drawThinLine(paint, color, thinLineWidth, scaleX, closeY, scaleX + xsub, closeY);
                                }
                            }
                            else {
                                drawThinLine(paint, color, thinLineWidth, scaleX - xsub, closeY, scaleX + xsub, closeY);
                            }
                        }
                        break;
                    //收盘线
                    case 4: {
                            onPaintPolyline(paint, div, cs.UpColor, FCColor.None, cs.ColorField, defaultLineWidth, PolylineStyle.SolidLine, close, cs.AttachVScale, scaleX, (int)closeY, i, points, ref x, ref y);
                            break;
                        }
                    default: {
                            //阳线
                            if (open <= close) {
                                //获取阳线的高度
                                float recth = getUpCandleHeight(close, open, div.getVScale(cs.AttachVScale).VisibleMax, div.getVScale(cs.AttachVScale).VisibleMin, div.WorkingAreaHeight - div.getVScale(cs.AttachVScale).PaddingBottom - div.getVScale(cs.AttachVScale).PaddingTop);
                                if (recth < 1) {
                                    recth = 1;
                                }
                                //获取阳线的矩形
                                int rcUpX = scaleX - xsub, rcUpTop = (int)closeY, rcUpBottom = (int)openY, rcUpW = cwidth, rcUpH = (int)recth;
                                if (openY < closeY) {
                                    rcUpTop = (int)openY;
                                    rcUpBottom = (int)closeY;
                                }
                                long upColor = FCColor.None;
                                int colorField = cs.ColorField;
                                if (colorField != FCDataTable.NULLFIELD) {
                                    double defineColor = m_dataSource.get2(i, colorField);
                                    if (!double.IsNaN(defineColor)) {
                                        upColor = (long)defineColor;
                                    }
                                }
                                if (upColor == FCColor.None) {
                                    upColor = cs.UpColor;
                                }
                                switch (style) {
                                    //矩形
                                    case 0:
                                    case 1:
                                    case 2:
                                        if ((int)highY != (int)lowY) {
                                            drawThinLine(paint, upColor, thinLineWidth, scaleX, highY, scaleX, lowY);
                                            if (m_hScalePixel > 3) {
                                                //描背景
                                                if ((int)openY == (int)closeY) {
                                                    drawThinLine(paint, upColor, thinLineWidth, rcUpX, rcUpBottom, rcUpX + rcUpW, rcUpBottom);
                                                }
                                                else {
                                                    FCRect rcUp = new FCRect(rcUpX, rcUpTop, rcUpX + rcUpW, rcUpBottom);
                                                    if (style == 0 || style == 1) {
                                                        if (rcUpW > 0 && rcUpH > 0 && m_hScalePixel > 3) {
                                                            paint.fillRect(div.BackColor, rcUp);
                                                        }
                                                        paint.drawRect(upColor, thinLineWidth, 0, rcUp);
                                                    }
                                                    else if (style == 2) {
                                                        paint.fillRect(upColor, rcUp);
                                                        if (thinLineWidth > 1) {
                                                            paint.drawRect(upColor, thinLineWidth, 0, rcUp);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else {
                                            drawThinLine(paint, upColor, thinLineWidth, scaleX - xsub, closeY, scaleX + xsub, closeY);
                                        }
                                        break;
                                    //宝塔线
                                    case 5: {
                                            double lOpen = m_dataSource.get3(i - 1, ciOpen);
                                            double lClose = m_dataSource.get3(i - 1, ciClose);
                                            double lHigh = m_dataSource.get3(i - 1, ciHigh);
                                            double lLow = m_dataSource.get3(i - 1, ciLow);
                                            float top = highY;
                                            float bottom = lowY;
                                            if ((int)highY > (int)lowY) {
                                                top = lowY;
                                                bottom = highY;
                                            }
                                            if (i == 0 || double.IsNaN(lOpen) || double.IsNaN(lClose) || double.IsNaN(lHigh) || double.IsNaN(lLow)) {
                                                if (m_hScalePixel <= 3) {
                                                    drawThinLine(paint, upColor, thinLineWidth, rcUpX, top, rcUpX, bottom);
                                                }
                                                else {
                                                    int rcUpHeight = (int)Math.Abs(bottom - top == 0 ? 1 : bottom - top);
                                                    if (rcUpW > 0 && rcUpHeight > 0) {
                                                        FCRect rcUp = new FCRect(rcUpX, top, rcUpX + rcUpW, top + rcUpHeight);
                                                        paint.fillRect(upColor, rcUp);
                                                        if (thinLineWidth > 1) {
                                                            paint.drawRect(upColor, thinLineWidth, 0, rcUp);
                                                        }
                                                    }
                                                }
                                            }
                                            else {
                                                if (m_hScalePixel <= 3) {
                                                    drawThinLine(paint, upColor, thinLineWidth, rcUpX, top, rcUpX, bottom);
                                                }
                                                else {
                                                    int rcUpHeight = (int)Math.Abs(bottom - top == 0 ? 1 : bottom - top);
                                                    if (rcUpW > 0 && rcUpHeight > 0) {
                                                        FCRect rcUp = new FCRect(rcUpX, top, rcUpX + rcUpW, top + rcUpHeight);
                                                        paint.fillRect(upColor, rcUp);
                                                        if (thinLineWidth > 1) {
                                                            paint.drawRect(upColor, thinLineWidth, 0, rcUp);
                                                        }
                                                    }
                                                }
                                                //上一股价为下跌，画未超过最高点部分
                                                if (lClose < lOpen && low < lHigh) {
                                                    //获取矩形
                                                    int tx = rcUpX;
                                                    int ty = (int)getY(div, lHigh, cs.AttachVScale);
                                                    if (high < lHigh) {
                                                        ty = (int)highY;
                                                    }
                                                    int width = rcUpW;
                                                    int height = (int)lowY - ty;
                                                    if (height > 0) {
                                                        if (m_hScalePixel <= 3) {
                                                            drawThinLine(paint, cs.DownColor, thinLineWidth, tx, ty, tx, ty + height);
                                                        }
                                                        else {
                                                            if (width > 0 && height > 0) {
                                                                FCRect tRect = new FCRect(tx, ty, tx + width, ty + height);
                                                                paint.fillRect(cs.DownColor, tRect);
                                                                if (thinLineWidth > 1) {
                                                                    paint.drawRect(cs.DownColor, thinLineWidth, 0, tRect);
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
                            //阴线
                            else {
                                //获取阴线的高度
                                float recth = getDownCandleHeight(close, open, div.getVScale(cs.AttachVScale).VisibleMax, div.getVScale(cs.AttachVScale).VisibleMin, div.WorkingAreaHeight - div.getVScale(cs.AttachVScale).PaddingBottom - div.getVScale(cs.AttachVScale).PaddingTop);
                                if (recth < 1) {
                                    recth = 1;
                                }
                                //获取阴线的矩形
                                int rcDownX = scaleX - xsub, rcDownTop = (int)openY, rcDownBottom = (int)closeY, rcDownW = cwidth, rcDownH = (int)recth;
                                if (closeY < openY) {
                                    rcDownTop = (int)closeY;
                                    rcDownBottom = (int)openY;
                                }
                                long downColor = FCColor.None;
                                if (ciClr != -1) {
                                    double defineColor = m_dataSource.get3(i, ciClr);
                                    if (!double.IsNaN(defineColor)) {
                                        downColor = (long)defineColor;
                                    }
                                }
                                if (downColor == FCColor.None) {
                                    downColor = cs.DownColor;
                                }
                                switch (style) {
                                    //标准
                                    case 0:
                                    case 1:
                                    case 2:
                                        if ((int)highY != (int)lowY) {
                                            drawThinLine(paint, downColor, thinLineWidth, scaleX, highY, scaleX, lowY);
                                            if (m_hScalePixel > 3) {
                                                FCRect rcDown = new FCRect(rcDownX, rcDownTop, rcDownX + rcDownW, rcDownBottom);
                                                if (style == 1) {
                                                    if (rcDownW > 0 && rcDownH > 0 && m_hScalePixel > 3) {
                                                        paint.fillRect(div.BackColor, rcDown);
                                                    }
                                                    paint.drawRect(downColor, thinLineWidth, 0, rcDown);
                                                }
                                                else if (style == 0 || style == 1) {
                                                    paint.fillRect(downColor, rcDown);
                                                    if (thinLineWidth > 1) {
                                                        paint.drawRect(downColor, thinLineWidth, 0, rcDown);
                                                    }
                                                }
                                            }
                                        }
                                        else {
                                            drawThinLine(paint, downColor, thinLineWidth, scaleX - xsub, closeY, scaleX + xsub, closeY);
                                        }
                                        break;
                                    //宝塔线
                                    case 5:
                                        double lOpen = m_dataSource.get3(i - 1, ciOpen);
                                        double lClose = m_dataSource.get3(i - 1, ciClose);
                                        double lHigh = m_dataSource.get3(i - 1, ciHigh);
                                        double lLow = m_dataSource.get3(i - 1, ciLow);
                                        float top = highY;
                                        float bottom = lowY;
                                        if ((int)highY > (int)lowY) {
                                            top = lowY;
                                            bottom = highY;
                                        }
                                        if (i == 0 || double.IsNaN(lOpen) || double.IsNaN(lClose) || double.IsNaN(lHigh) || double.IsNaN(lLow)) {
                                            if (m_hScalePixel <= 3) {
                                                drawThinLine(paint, downColor, thinLineWidth, rcDownX, top, rcDownX, bottom);
                                            }
                                            else {
                                                int rcDownHeight = (int)Math.Abs(bottom - top == 0 ? 1 : bottom - top);
                                                if (rcDownW > 0 && rcDownHeight > 0) {
                                                    FCRect rcDown = new FCRect(rcDownX, top, rcDownX + rcDownW, rcDownBottom);
                                                    paint.fillRect(downColor, rcDown);
                                                    if (thinLineWidth > 1) {
                                                        paint.drawRect(downColor, thinLineWidth, 0, rcDown);
                                                    }
                                                }
                                            }
                                        }
                                        else {
                                            //先画阳线部分
                                            if (m_hScalePixel <= 3) {
                                                drawThinLine(paint, downColor, thinLineWidth, rcDownX, top, rcDownX, bottom);
                                            }
                                            else {
                                                int rcDownHeight = (int)Math.Abs(bottom - top == 0 ? 1 : bottom - top);
                                                if (rcDownW > 0 && rcDownHeight > 0) {
                                                    FCRect rcDown = new FCRect(rcDownX, top, rcDownX + rcDownW, rcDownBottom);
                                                    paint.fillRect(downColor, rcDown);
                                                    if (thinLineWidth > 1) {
                                                        paint.drawRect(downColor, thinLineWidth, 0, rcDown);
                                                    }
                                                }
                                            }
                                            //上一股价为上涨，画未跌过最高点部分
                                            if (lClose >= lOpen && high > lLow) {
                                                //获取矩形
                                                int tx = rcDownX;
                                                int ty = (int)highY;
                                                int width = rcDownW;
                                                int height = (int)Math.Abs(getY(div, lLow, cs.AttachVScale) - ty);
                                                if (low > lLow) {
                                                    height = (int)lowY - ty;
                                                }
                                                if (height > 0) {
                                                    if (m_hScalePixel <= 3) {
                                                        drawThinLine(paint, cs.UpColor, thinLineWidth, tx, ty, tx, ty + height);
                                                    }
                                                    else {
                                                        if (width > 0 && height > 0) {
                                                            FCRect tRect = new FCRect(tx, ty, tx + width, ty + height);
                                                            paint.fillRect(cs.UpColor, new FCRect(tx, ty, tx + width, ty + height));
                                                            if (thinLineWidth > 1) {
                                                                paint.drawRect(cs.UpColor, thinLineWidth, 0, tRect);
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
                //绘制选中
                if (cs.Selected) {
                    int kPInterval = m_maxVisibleRecord / 30;
                    if (kPInterval < 2) {
                        kPInterval = 3;
                    }
                    if (i % kPInterval == 0) {
                        if (!double.IsNaN(open) && !double.IsNaN(high) && !double.IsNaN(low) && !double.IsNaN(close)) {
                            if (closeY >= div.TitleBar.Height
                                && closeY <= div.Height - div.HScale.Height) {
                                FCRect rect = new FCRect(scaleX - 3, (int)closeY - 4, scaleX + 4, (int)closeY + 3);
                                paint.fillRect(cs.getSelectedColor(), rect);
                            }
                        }
                    }
                }
            }
            onPaintCandleEx(paint, div, cs, visibleMaxIndex, visibleMax, visibleMinIndex, visibleMin);
        }

        /// <summary>
        /// 绘制K线的扩展属性
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="div">要绘制的层</param>
        /// <param name="cs">K线</param>
        /// <param name="visibleMaxIndex">最大值索引</param>
        /// <param name="visibleMax">最大值</param>
        /// <param name="visibleMinIndex">最小值索引</param>
        /// <param name="visibleMin">最小值</param>
        public virtual void onPaintCandleEx(FCPaint paint, ChartDiv div, CandleShape cs, int visibleMaxIndex, double visibleMax, int visibleMinIndex, double visibleMin) {
            if (m_dataSource.RowsCount > 0) {
                if (visibleMaxIndex != -1 && visibleMinIndex != -1 && cs.ShowMaxMin) {
                    double max = visibleMax;
                    double min = visibleMin;
                    float scaleYMax = getY(div, max, cs.AttachVScale);
                    float scaleYMin = getY(div, min, cs.AttachVScale);
                    //画K线的最大值
                    int scaleXMax = (int)getX(visibleMaxIndex);
                    int digit = div.getVScale(cs.AttachVScale).Digit;
                    FCSize maxSize = paint.textSize(FCStr.getValueByDigit(max, digit), div.Font);
                    float maxPX = 0, maxPY = 0;
                    float strY = 0;
                    if (scaleYMax > scaleYMin) {
                        getCandleMinStringPoint(scaleXMax, scaleYMax, maxSize.cx, maxSize.cy, Width, m_leftVScaleWidth, m_rightVScaleWidth, ref maxPX, ref maxPY);
                        strY = maxPY + maxSize.cy;
                    }
                    else {
                        getCandleMaxStringPoint(scaleXMax, scaleYMax, maxSize.cx, maxSize.cy, Width, m_leftVScaleWidth, m_rightVScaleWidth, ref maxPX, ref maxPY);
                        strY = maxPY;
                    }
                    FCPoint maxP = new FCPoint((int)maxPX, (int)maxPY);
                    drawText(paint, FCStr.getValueByDigit(max, digit), cs.TagColor, div.Font, maxP);
                    paint.drawLine(cs.TagColor, 1, 0, scaleXMax, (int)scaleYMax, maxP.x + maxSize.cx / 2, (int)strY);
                    //画K线的最小值
                    FCSize minSize = paint.textSize(FCStr.getValueByDigit(min, digit), div.Font);
                    int scaleXMin = (int)getX(visibleMinIndex);
                    float minPX = 0, minPY = 0;
                    if (scaleYMax > scaleYMin) {
                        getCandleMaxStringPoint(scaleXMin, scaleYMin, minSize.cx, minSize.cy, Width, m_leftVScaleWidth, m_rightVScaleWidth, ref minPX, ref minPY);
                        strY = minPY;
                    }
                    else {
                        getCandleMinStringPoint(scaleXMin, scaleYMin, minSize.cx, minSize.cy, Width, m_leftVScaleWidth, m_rightVScaleWidth, ref minPX, ref minPY);
                        strY = minPY + minSize.cy;
                    }
                    FCPoint minP = new FCPoint((int)minPX, (int)minPY);
                    drawText(paint, FCStr.getValueByDigit(min, digit), cs.TagColor, div.Font, minP);
                    paint.drawLine(cs.TagColor, 1, 0, scaleXMin, (int)scaleYMin, minP.x + minSize.cx / 2, (int)strY);
                }
            }
        }

        /// <summary>
        /// 绘制十字线
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="div">要绘制的层</param>
        public virtual void onPaintCrossLine(FCPaint paint, ChartDiv div) {
            FCPoint touchPoint = TouchPoint;
            if (m_cross_y != -1) {
                int divWidth = div.Width;
                int divHeight = div.Height;
                int titleBarHeight = div.TitleBar.Height;
                int hScaleHeight = div.HScale.Height;
                int mpY = m_cross_y - div.Top;
                if (m_dataSource.RowsCount > 0 && m_hResizeType == 0 && m_userResizeDiv == null) {
                    if (mpY >= titleBarHeight && mpY <= divHeight - hScaleHeight) {
                        //显示左侧Y轴的提示框及文字
                        VScale leftVScale = div.LeftVScale;
                        CrossLineTip crossLineTip = leftVScale.CrossLineTip;
                        if (m_leftVScaleWidth != 0 && crossLineTip.Visible) {
                            if (crossLineTip.AllowUserPaint) {
                                FCRect clipRect = new FCRect(0, 0, m_leftVScaleWidth, divHeight - hScaleHeight);
                                crossLineTip.onPaint(paint, div, clipRect);
                            }
                            else {
                                double lValue = getNumberValue(div, touchPoint, AttachVScale.Left);
                                String leftValue = FCStr.getValueByDigit(lValue, leftVScale.Digit);
                                FCSize leftYTipFontSize = paint.textSize(leftValue, crossLineTip.Font);
                                if (leftYTipFontSize.cx > 0 && leftYTipFontSize.cy > 0) {
                                    int lRtX = m_leftVScaleWidth - leftYTipFontSize.cx - 1;
                                    int lRtY = mpY - leftYTipFontSize.cy / 2;
                                    int lRtW = leftYTipFontSize.cx;
                                    int lRtH = leftYTipFontSize.cy;
                                    if (lRtW > 0 && lRtH > 0) {
                                        FCRect lRtL = new FCRect(lRtX, lRtY, lRtX + lRtW, lRtY + lRtH);
                                        paint.fillRect(crossLineTip.BackColor, lRtL);
                                        paint.drawRect(crossLineTip.TextColor, 1, 0, lRtL);
                                    }
                                    drawText(paint, leftValue, crossLineTip.TextColor, crossLineTip.Font, new FCPoint(lRtX, lRtY));
                                }
                            }
                        }
                        //显示右侧Y轴的提示框及文字
                        VScale rightVScale = div.RightVScale;
                        crossLineTip = rightVScale.CrossLineTip;
                        if (m_rightVScaleWidth != 0 && crossLineTip.Visible) {
                            if (crossLineTip.AllowUserPaint) {
                                FCRect clipRect = new FCRect(divWidth - m_rightVScaleWidth, 0, divWidth, divHeight - hScaleHeight);
                                crossLineTip.onPaint(paint, div, clipRect);
                            }
                            else {
                                double rValue = getNumberValue(div, touchPoint, AttachVScale.Right);
                                String rightValue = FCStr.getValueByDigit(rValue, rightVScale.Digit);
                                FCSize rightYTipFontSize = paint.textSize(rightValue, crossLineTip.Font);
                                if (rightYTipFontSize.cx > 0 && rightYTipFontSize.cy > 0) {
                                    int rRtX = Width - m_rightVScaleWidth + 1;
                                    int rRtY = mpY - rightYTipFontSize.cy / 2;
                                    int rRtW = rightYTipFontSize.cx;
                                    int rRtH = rightYTipFontSize.cy;
                                    if (rRtW > 0 && rRtH > 0) {
                                        FCRect rRtL = new FCRect(rRtX, rRtY, rRtX + rRtW, rRtY + rRtH);
                                        paint.fillRect(crossLineTip.BackColor, rRtL);
                                        paint.drawRect(crossLineTip.TextColor, 1, 0, rRtL);
                                    }
                                    drawText(paint, rightValue, crossLineTip.TextColor, crossLineTip.Font, new FCPoint(rRtX, rRtY));
                                }
                            }
                        }
                    }
                }
                int verticalX = 0;
                if (m_crossStopIndex >= m_firstVisibleIndex && m_crossStopIndex <= m_lastVisibleIndex) {
                    verticalX = (int)getX(m_crossStopIndex);
                }
                if (!m_isScrollCross) {
                    verticalX = touchPoint.x;
                }
                CrossLine crossLine = div.CrossLine;
                if (crossLine.AllowUserPaint) {
                    FCRect clRect = new FCRect(0, 0, divWidth, divHeight);
                    crossLine.onPaint(paint, div, clRect);
                }
                else {
                    if (m_showCrossLine) {
                        if (mpY >= titleBarHeight && mpY <= divHeight - hScaleHeight) {
                            //横向的线
                            paint.drawLine(crossLine.LineColor, 1, 0, m_leftVScaleWidth, mpY, Width - m_rightVScaleWidth, mpY);
                        }
                    }
                    //超出索引时
                    if (m_crossStopIndex == -1 || m_crossStopIndex < m_firstVisibleIndex || m_crossStopIndex > m_lastVisibleIndex) {
                        if (m_showCrossLine) {
                            int x = touchPoint.x;
                            if (x > m_leftVScaleWidth && x < m_leftVScaleWidth + m_workingAreaWidth) {
                                paint.drawLine(crossLine.LineColor, 1, 0, x, titleBarHeight, x, divHeight - hScaleHeight);
                            }
                        }
                        return;
                    }
                    //纵向的线
                    if (m_showCrossLine) {
                        paint.drawLine(crossLine.LineColor, 1, 0, verticalX, titleBarHeight, verticalX, divHeight - hScaleHeight);
                    }
                }
                if (m_hResizeType == 0 && m_userResizeDiv == null) {
                    HScale hScale = div.HScale;
                    CrossLineTip hScaleCrossLineTip = hScale.CrossLineTip;
                    if (hScale.Visible && hScaleCrossLineTip.Visible) {
                        if (hScaleCrossLineTip.AllowUserPaint) {
                            FCRect clipRect = new FCRect(0, divHeight - hScaleHeight, divWidth, divHeight);
                            hScaleCrossLineTip.onPaint(paint, div, clipRect);
                        }
                        else {
                            String tip = String.Empty;
                            //获取文字
                            if (hScale.HScaleType == HScaleType.Date) {
                                DateTime date = FCStr.convertNumToDate(m_dataSource.getXValue(m_crossStopIndex));
                                if (date.Hour != 0) {
                                    tip = date.ToString("HH:mm");
                                }
                                else {
                                    tip = date.ToString("yyyy-MM-dd");
                                }
                            }
                            else if (hScale.HScaleType == HScaleType.Number) {
                                tip = m_dataSource.getXValue(m_crossStopIndex).ToString();
                            }
                            FCSize xTipFontSize = paint.textSize(tip, hScaleCrossLineTip.Font);
                            int xRtX = verticalX - xTipFontSize.cx / 2;
                            int xRtY = div.Height - hScaleHeight + 2;
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
                                paint.fillRect(hScaleCrossLineTip.BackColor, xRtL);
                                paint.drawRect(hScaleCrossLineTip.TextColor, 1, 0, xRtL);
                                drawText(paint, tip, hScaleCrossLineTip.TextColor, hScaleCrossLineTip.Font, new FCPoint(xRtX, xRtY));
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 绘制层背景
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="div">要绘制的层</param>
        public virtual void onPaintDivBackGround(FCPaint paint, ChartDiv div) {
            int width = div.Width;
            int height = div.Height;
            if (width < 1) {
                width = 1;
            }
            if (height < 1) {
                height = 1;
            }
            if (width > 0 && height > 0) {
                FCRect rect = new FCRect(0, 0, width, height);
                if (div.AllowUserPaint) {
                    div.onPaint(paint, rect);
                }
                else {
                    if (div.BackColor != FCColor.None && div.BackColor != BackColor) {
                        paint.fillRect(div.BackColor, rect);
                    }
                }
            }
        }

        /// <summary>
        /// 绘制层边框       
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="div">要绘制的层</param>
        public virtual void onPaintDivBorder(FCPaint paint, ChartDiv div) {
            int y = 0;
            int width = div.Width;
            int height = div.Height;
            if (width < 1) {
                width = 1;
            }
            if (height < 1) {
                height = 1;
            }
            if (width > 0 && height > 0) {
                //获取上一个层
                ChartDiv lDiv = null;
                ArrayList<ChartDiv> divsCopy = getDivs();
                foreach (ChartDiv cDiv in divsCopy) {
                    if (div != cDiv) {
                        lDiv = cDiv;
                    }
                    else {
                        break;
                    }
                }
                //画线
                if (lDiv != null) {
                    if (!lDiv.HScale.Visible) {
                        paint.drawLine(div.HScale.ScaleColor, 1, 0, m_leftVScaleWidth, y, width - m_rightVScaleWidth, y);
                    }
                    else {
                        paint.drawLine(div.HScale.ScaleColor, 1, 0, 0, y, width, y);
                    }
                }
                if (div.ShowSelect && div.Selected) {
                    //画左轴选中框
                    if (m_leftVScaleWidth > 0) {
                        FCRect leftRect = new FCRect(1, 1,
                            m_leftVScaleWidth, div.Height - div.HScale.Height - 1);
                        if (leftRect.right - leftRect.left > 0 && leftRect.bottom - leftRect.top > 0) {
                            paint.drawRect(div.LeftVScale.ScaleColor, 1, 0, leftRect);
                        }
                    }
                    //画右轴选中框
                    if (m_rightVScaleWidth > 0) {
                        FCRect rightRect = new FCRect(Width - m_rightVScaleWidth + 1, 1,
                        Width, div.Height - div.HScale.Height - 1);
                        if (rightRect.right - rightRect.left > 0 && rightRect.bottom - rightRect.top > 0) {
                            paint.drawRect(div.RightVScale.ScaleColor, 1, 0, rightRect);
                        }
                    }
                }
                if (div.BorderColor != FCColor.None) {
                    if (width > 0 && height > 0) {
                        paint.drawRect(div.BorderColor, 1, 0, new FCRect(0, y, width, y + height));
                    }
                }
            }
        }

        /// <summary>
        /// 绘制横坐标轴
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="div">要绘制的层</param>
        public virtual void onPaintHScale(FCPaint paint, ChartDiv div) {
            HScale hScale = div.HScale;
            ScaleGrid vGrid = div.VGrid;
            int width = div.Width, height = div.Height, hScaleHeight = hScale.Height;
            //画X轴
            if ((hScale.Visible || vGrid.Visible) && height >= hScaleHeight) {
                FCRect hRect = new FCRect(0, height - hScaleHeight, width, height);
                if (hScale.AllowUserPaint) {
                    hScale.onPaint(paint, div, hRect);
                }
                if (vGrid.AllowUserPaint) {
                    vGrid.onPaint(paint, div, hRect);
                }
                if (hScale.AllowUserPaint && vGrid.AllowUserPaint) {
                    return;
                }
                int divBottom = div.Height;
                if (hScale.Visible && !hScale.AllowUserPaint) {
                    //画底部横线
                    paint.drawLine(hScale.ScaleColor, 1, 0, 0, divBottom - hScaleHeight + 1, width, divBottom - hScaleHeight + 1);
                }
                if (m_firstVisibleIndex >= 0) {
                    double xScaleWordRight = 0;
                    int pureH = m_workingAreaWidth;
                    //获取自定义的刻度
                    ArrayList<double> scaleSteps = hScale.getScaleSteps();
                    int scaleStepsSize = scaleSteps.size();
                    HashMap<double, int> scaleStepsMap = new HashMap<double, int>();
                    for (int i = 0; i < scaleStepsSize; i++) {
                        scaleStepsMap.put(scaleSteps.get(i), 0);
                    }
                    //数值类型
                    if (hScale.HScaleType == HScaleType.Number) {
                        //循环遍历要显示的数值
                        for (int i = m_firstVisibleIndex; i <= m_lastVisibleIndex; i++) {
                            double xValue = m_dataSource.getXValue(i);
                            if (scaleStepsSize > 0 && !scaleStepsMap.containsKey(xValue)) {
                                continue;
                            }
                            String xValueStr = xValue.ToString();
                            //画X轴的刻度
                            int scaleX = (int)getX(i);
                            FCSize xSize = paint.textSize(xValueStr, hScale.Font);
                            if (scaleStepsSize > 0 || scaleX - xSize.cx / 2 - hScale.Interval > xScaleWordRight) {
                                if (hScale.Visible && !hScale.AllowUserPaint) {
                                    drawThinLine(paint, hScale.ScaleColor, 1, scaleX, divBottom - hScaleHeight + 1,
                                    scaleX, divBottom - hScaleHeight + 4);
                                    drawText(paint, xValueStr, hScale.TextColor, hScale.Font,
                                        new FCPoint(scaleX - xSize.cx / 2, divBottom - hScaleHeight + 6));
                                }
                                xScaleWordRight = scaleX + xSize.cx / 2;
                                //画纵向的网格
                                if (vGrid.Visible && !vGrid.AllowUserPaint) {
                                    paint.drawLine(vGrid.GridColor, 1, vGrid.LineStyle, scaleX, div.TitleBar.Height,
                                        scaleX, div.Height - hScaleHeight);
                                }
                            }
                        }
                    }
                    //日期类型
                    else {
                        ArrayList<int> xList = new ArrayList<int>();
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
                                }
                                else {
                                    continue;
                                }
                            }
                            xList.add(i);
                        }
                        int interval = hScale.Interval;
                        ArrayList<int> lasts = new ArrayList<int>();
                        for (int p = 0; p < 7; p++) {
                            int count = 0;
                            int xListSize = xList.size();
                            for (int i = 0; i < xListSize; i++) {
                                int pos = xList.get(i);
                                double date = m_dataSource.getXValue(pos);
                                DateType dateType = DateType.Day;
                                //上次的日期
                                double lDate = 0;
                                if (pos > 0) {
                                    //获取上次的日期
                                    lDate = m_dataSource.getXValue(pos - 1);
                                }
                                String xValue = getHScaleDateString(date, lDate, ref dateType);
                                int scaleX = (int)getX(pos);
                                //年优先显示
                                if (dateType == (DateType)p) {
                                    count++;
                                    bool show = true;
                                    if (scaleStepsSize == 0) {
                                        //循环遍历集合
                                        foreach (int index in lasts) {
                                            int getx = (int)getX(index);
                                            //和右边比较
                                            if (index > pos) {
                                                if (m_reverseHScale) {
                                                    if (getx + interval * 2 > scaleX) {
                                                        show = false;
                                                        break;
                                                    }
                                                }
                                                else {
                                                    if (getx - interval * 2 < scaleX) {
                                                        show = false;
                                                        break;
                                                    }
                                                }
                                            }
                                            //和左边比较
                                            else if (index < pos) {
                                                if (m_reverseHScale) {
                                                    if (getx - interval * 2 < scaleX) {
                                                        show = false;
                                                        break;
                                                    }
                                                }
                                                else {
                                                    if (getx + interval * 2 > scaleX) {
                                                        show = false;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (show) {
                                        lasts.add(pos);
                                        if (hScale.Visible && !hScale.AllowUserPaint) {
                                            FCSize xSize = paint.textSize(xValue, hScale.Font);
                                            drawThinLine(paint, hScale.ScaleColor, 1, scaleX, divBottom - hScaleHeight + 1,
                                           scaleX, divBottom - hScaleHeight + 4);
                                            long dateColor = hScale.getDateColor((DateType)p);
                                            drawText(paint, xValue, dateColor, hScale.Font,
                                            new FCPoint(scaleX - xSize.cx / 2, divBottom - hScaleHeight + 6));
                                        }
                                        //画纵向的网格
                                        if (vGrid.Visible && !vGrid.AllowUserPaint) {
                                            paint.drawLine(vGrid.GridColor, 1, vGrid.LineStyle, scaleX, div.TitleBar.Height,
                                                scaleX, div.Height - hScaleHeight);
                                        }
                                        xList.removeAt(i);
                                        i--;
                                        xListSize--;
                                    }
                                }
                            }
                            //跳出循环
                            if (count > pureH / 40) {
                                break;
                            }
                        }
                        lasts.clear();
                    }
                }
            }
            //画标题下方的线
            if (div.TitleBar.ShowUnderLine) {
                FCSize sizeTitle = paint.textSize(" ", div.TitleBar.Font);
                paint.drawLine(div.TitleBar.UnderLineColor, 1, 2, m_leftVScaleWidth, 5 + sizeTitle.cy,
                    width - m_rightVScaleWidth, 5 + sizeTitle.cy);
            }
        }

        /// <summary>
        /// 绘制图形的图标
        /// </summary>
        /// <param name="paint">绘图对象</param>
        public virtual void onPaintIcon(FCPaint paint) {
            if (m_movingShape != null) {
                ChartDiv div = findDiv(m_movingShape);
                if (div != null) {
                    FCPoint actualPoint = TouchPoint;
                    int x = actualPoint.x;
                    int y = actualPoint.y;
                    if (m_lastTouchClickPoint.x != -1 && m_lastTouchClickPoint.y != -1 &&
                        Math.Abs(x - m_lastTouchClickPoint.x) > 5 && Math.Abs(y - m_lastTouchClickPoint.y) > 5) {
                        FCSize sizeK = new FCSize(15, 16);
                        int rectCsX = x - sizeK.cx;
                        int rectCsY = y - sizeK.cy;
                        int rectCsH = sizeK.cy;
                        //柱状图
                        if (m_movingShape is BarShape) {
                            BarShape bs = m_movingShape as BarShape;
                            paint.fillRect(bs.UpColor, new FCRect(rectCsX + 1, rectCsY + 10, rectCsX + 4, rectCsY + rectCsH - 1));
                            paint.fillRect(bs.UpColor, new FCRect(rectCsX + 6, rectCsY + 3, rectCsX + 9, rectCsY + rectCsH - 1));
                            paint.fillRect(bs.UpColor, new FCRect(rectCsX + 11, rectCsY + 8, rectCsX + 14, rectCsY + rectCsH - 1));
                        }
                        //K线
                        else if (m_movingShape is CandleShape) {
                            CandleShape cs = m_movingShape as CandleShape;
                            paint.drawLine(cs.DownColor, 1, 0, rectCsX + 4, rectCsY + 6, rectCsX + 4, rectCsY + rectCsH - 2);
                            paint.drawLine(cs.UpColor, 1, 0, rectCsX + 9, rectCsY + 2, rectCsX + 9, rectCsY + rectCsH - 4);
                            paint.fillRect(cs.DownColor, new FCRect(rectCsX + 3, rectCsY + 8, rectCsX + 6, rectCsY + 13));
                            paint.fillRect(cs.UpColor, new FCRect(rectCsX + 8, rectCsY + 4, rectCsX + 11, rectCsY + 9));
                        }
                        //线
                        else if (m_movingShape is PolylineShape) {
                            PolylineShape tls = m_movingShape as PolylineShape;
                            paint.drawLine(tls.Color, 1, 0, rectCsX + 2, rectCsY + 5, rectCsX + 12, rectCsY + 1);
                            paint.drawLine(tls.Color, 1, 0, rectCsX + 2, rectCsY + 10, rectCsX + 12, rectCsY + 6);
                            paint.drawLine(tls.Color, 1, 0, rectCsX + 2, rectCsY + 15, rectCsX + 12, rectCsY + 11);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 绘制画线工具
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="div">要绘制的层</param>
        public virtual void onPaintPlots(FCPaint paint, ChartDiv div) {
            ArrayList<FCPlot> plotsCopy = div.getPlots(SortType.ASC);
            if (plotsCopy != null && plotsCopy.size() > 0) {
                //获取横向和纵向的宽度
                int wX = m_workingAreaWidth;
                int wY = div.WorkingAreaHeight;
                if (wX > 0 && wY > 0) {
                    //裁剪
                    FCRect clipRect = new FCRect();
                    clipRect.left = m_leftVScaleWidth;
                    clipRect.top = (div.TitleBar.Visible ? div.TitleBar.Height : 0);
                    clipRect.right = clipRect.left + wX;
                    clipRect.bottom = clipRect.top + wY;
                    //循环遍历所有的画线工具
                    foreach (FCPlot pl in plotsCopy) {
                        if (pl.Visible) {
                            paint.setClip(clipRect);
                            pl.render(paint);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 绘制趋势线
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="div">要绘制的层</param>
        /// <param name="ls">线条对象</param>
        public virtual void onPaintPolyline(FCPaint paint, ChartDiv div, PolylineShape ls) {
            int x = 0, y = 0;
            ArrayList<FCPoint> points = new ArrayList<FCPoint>();
            int ciFieldName = m_dataSource.getColumnIndex(ls.getBaseField());
            int ciClr = m_dataSource.getColumnIndex(ls.ColorField);
            float defaultLineWidth = ls.Width;
            if (!isOperating() && m_crossStopIndex != -1) {
                if (selectPolyline(div, TouchPoint, ls.getBaseField(), ls.Width, ls.AttachVScale, m_crossStopIndex)) {
                    defaultLineWidth += 1;
                }
            }
            for (int i = m_firstVisibleIndex; i <= m_lastVisibleIndex; i++) {
                int scaleX = (int)getX(i);
                double value = m_dataSource.get3(i, ciFieldName);
                if (!double.IsNaN(value)) {
                    int lY = (int)getY(div, value, ls.AttachVScale);
                    //画线
                    onPaintPolyline(paint, div, ls.Color, ls.FillColor, ciClr, defaultLineWidth, ls.Style, value, ls.AttachVScale, scaleX, lY, i, points, ref x, ref y);
                    if (ls.Selected) {
                        //显示选中
                        int kPInterval = m_maxVisibleRecord / 30;
                        if (kPInterval < 2) {
                            kPInterval = 3;
                        }
                        if (i % kPInterval == 0) {
                            if (lY >= div.TitleBar.Height
                               && lY <= div.Height - div.HScale.Height) {
                                int lineWidth = (int)ls.Width;
                                int rl = scaleX - 3 - (lineWidth - 1);
                                int rt = lY - 4 - (lineWidth - 1);
                                FCRect rect = new FCRect(rl, rt, rl + 7 + (lineWidth - 1) * 2, rt + 7 + (lineWidth - 1) * 2);
                                paint.fillRect(ls.getSelectedColor(), rect);
                            }
                        }
                    }
                }
                else {
                    //画线
                    onPaintPolyline(paint, div, ls.Color, ls.FillColor, ciClr, defaultLineWidth, ls.Style, value, ls.AttachVScale, scaleX, 0, i, points, ref x, ref y);
                }
            }
        }

        /// <summary>
        /// 绘制趋势线
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="div">图层</param>
        /// <param name="lineColor">线的颜色</param>
        /// <param name="fillColor">填充色</param>
        /// <param name="ciClr">颜色字段</param>
        /// <param name="lineWidth">线的宽度</param>
        /// <param name="lineStyle">线的样式</param>
        /// <param name="value">点的值</param>
        /// <param name="attachVScale">依附坐标轴 </param>
        /// <param name="scaleX">横坐标 </param>
        /// <param name="lY">纵坐标 </param>
        /// <param name="i">索引</param>
        /// <param name="points">点集合</param>
        /// <param name="x">横坐标</param>
        /// <param name="y">纵坐标</param>
        public virtual void onPaintPolyline(FCPaint paint, ChartDiv div, long lineColor, long fillColor, int ciClr,
            float lineWidth, PolylineStyle lineStyle, double value, AttachVScale attachVScale,
            int scaleX, int lY, int i, ArrayList<FCPoint> points, ref int x, ref int y) {
            if (!double.IsNaN(value)) {
                if (m_dataSource.RowsCount == 1) {
                    int cwidth = (int)(m_hScalePixel / 4);
                    points.add(new FCPoint(scaleX - cwidth, lY));
                    points.add(new FCPoint(scaleX - cwidth + cwidth * 2 + 1, lY));
                }
                else {
                    int newX = 0;
                    int newY = 0;
                    if (i == m_firstVisibleIndex) {
                        x = scaleX;
                        y = lY;
                    }
                    newX = scaleX;
                    newY = lY;
                    //限制绘图
                    if (y <= div.Height - div.HScale.Height + 1
                     && y >= div.TitleBar.Height - 1
                     && newY < div.Height - div.HScale.Height + 1
                     && newY >= div.TitleBar.Height - 1) {
                        if (x != newX || y != newY) {
                            if (points.size() == 0) {
                                points.add(new FCPoint(x, y));
                                points.add(new FCPoint(newX, newY));
                            }
                            else {
                                points.add(new FCPoint(newX, newY));
                            }
                        }
                    }
                    x = newX;
                    y = newY;
                }
                if (ciClr != -1) {
                    double defineColor = m_dataSource.get3(i, ciClr);
                    if (!double.IsNaN(defineColor)) {
                        lineColor = (long)defineColor;
                    }
                }
            }
            if (points.size() > 0) {
                //获取上次线的颜色
                long lColor = lineColor;
                if (i > 0) {
                    //获取上一行的颜色
                    if (ciClr != -1) {
                        double defineColor = m_dataSource.get3(i - 1, ciClr);
                        if (!double.IsNaN(defineColor)) {
                            lColor = (long)defineColor;
                        }
                    }
                }
                //绘制线条
                if (lineColor != lColor || i == m_lastVisibleIndex) {
                    long drawColor = lineColor;
                    int width = (int)(m_hScalePixel / 2);
                    if (lColor != lineColor) {
                        drawColor = lColor;
                    }
                    switch (lineStyle) {
                        //圆
                        case PolylineStyle.Cycle:
                            int ew = (width - 1) > 0 ? (width - 1) : 1;
                            int pointsSize = points.size();
                            for (int j = 0; j < pointsSize; j++) {
                                FCPoint point = points.get(j);
                                FCRect pRect = new FCRect(point.x - ew / 2,
                                   point.y - ew / 2, point.x + ew / 2, point.y + ew / 2);
                                paint.drawEllipse(drawColor, lineWidth, 0, pRect);
                            }
                            break;
                        case PolylineStyle.DashLine: {
                                paint.drawPolyline(drawColor, lineWidth, 1, points.ToArray());
                                break;
                            }
                        //点线
                        case PolylineStyle.DotLine: {
                                paint.drawPolyline(drawColor, lineWidth, 2, points.ToArray());
                                break;
                            }
                        //实线
                        case PolylineStyle.SolidLine: {
                                paint.drawPolyline(drawColor, lineWidth, 0, points.ToArray());
                                break;
                            }
                    }
                    if (fillColor != FCColor.None) {
                        int zy = (int)getY(div, 0, attachVScale);
                        int th = div.TitleBar.Visible ? div.TitleBar.Height : 0;
                        int hh = div.HScale.Visible ? div.HScale.Height : 0;
                        if (zy < th) zy = th;
                        else if (zy > div.Height - hh) zy = div.Height - hh;
                        points.Insert(0, new FCPoint(points.get(0).x, zy));
                        points.add(new FCPoint(points.get(points.size() - 1).x, zy));
                        paint.fillPolygon(fillColor, points.ToArray());
                    }
                    points.clear();
                }
            }
        }

        /// <summary>
        /// 绘制拖动的边线
        /// </summary>
        /// <param name="paint">绘图对象</param>
        public virtual void onPaintResizeLine(FCPaint paint) {
            //画横向拖动线
            if (m_hResizeType > 0) {
                FCPoint mp = TouchPoint;
                ArrayList<ChartDiv> divsCopy = getDivs();
                foreach (ChartDiv div in divsCopy) {
                    if (mp.x < 0) mp.x = 0;
                    if (mp.x > Width) mp.x = Width;
                    paint.drawLine(FCColor.reverse(paint, div.BackColor), 1, 2, mp.x, 0, mp.x, Width);
                }
            }
            //画垂直拖动线
            if (m_userResizeDiv != null) {
                FCPoint mp = TouchPoint;
                ChartDiv nextCP = null;
                bool rightP = false;
                //循环遍历所有的层
                ArrayList<ChartDiv> divsCopy = getDivs();
                foreach (ChartDiv cDiv in divsCopy) {
                    if (rightP) {
                        nextCP = cDiv;
                        break;
                    }
                    if (cDiv == m_userResizeDiv) {
                        rightP = true;
                    }
                }
                FCRect uRect = m_userResizeDiv.Bounds;
                //画拖动阴影
                bool drawFlag = false;
                if (mp.x >= uRect.left && mp.x <= uRect.right && mp.y >= uRect.top && mp.y <= uRect.bottom) {
                    drawFlag = true;
                }
                else {
                    if (nextCP != null) {
                        FCRect nRect = nextCP.Bounds;
                        if (mp.x >= nRect.left && mp.x <= nRect.right && mp.y >= nRect.top && mp.y <= nRect.bottom) {
                            drawFlag = true;
                        }
                    }
                }
                //画线
                if (drawFlag) {
                    paint.drawLine(FCColor.reverse(paint, m_userResizeDiv.BackColor), 1, 2, 0, mp.y, Width, mp.y);
                }
            }
        }

        /// <summary>
        /// 绘制选中块
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="div">要绘制的层</param>
        public virtual void onPaintSelectArea(FCPaint paint, ChartDiv div) {
            SelectArea selectArea = div.SelectArea;
            //判断是否显示板块
            if (selectArea.Visible) {
                FCRect bounds = selectArea.Bounds;
                if (selectArea.AllowUserPaint) {
                    selectArea.onPaint(paint, div, bounds);
                }
                else {
                    //系统绘图
                    if (bounds.right - bounds.left >= 5 && bounds.bottom - bounds.top >= 5) {
                        //画选中边框
                        paint.drawRect(selectArea.LineColor, 1, 0, bounds);
                        paint.fillRect(selectArea.BackColor, bounds);
                    }
                }
            }
        }

        /// <summary>
        /// 绘制K线，成交量，趋势线等等
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="div">要绘制的层</param>
        public virtual void onPaintShapes(FCPaint paint, ChartDiv div) {
            //排序后的线条
            ArrayList<BaseShape> sortedBs = div.getShapes(SortType.ASC);
            foreach (BaseShape bShape in sortedBs) {
                if (!bShape.Visible || (div.getVScale(bShape.AttachVScale).VisibleMax - div.getVScale(bShape.AttachVScale).VisibleMin) == 0) {
                    continue;
                }
                if (bShape.AllowUserPaint) {
                    FCRect rect = new FCRect(0, 0, div.Width, div.Height);
                    bShape.onPaint(paint, div, rect);
                }
                else {
                    BarShape bs = bShape as BarShape;
                    CandleShape cs = bShape as CandleShape;
                    PolylineShape ls = bShape as PolylineShape;
                    TextShape ts = bShape as TextShape;
                    //线条
                    if (ls != null) {
                        onPaintPolyline(paint, div, ls);
                    }
                    //文字
                    else if (ts != null) {
                        onPaintText(paint, div, ts);
                    }
                    //柱状图
                    else if (bs != null) {
                        onPaintBar(paint, div, bs);
                    }
                    //其他
                    else if (cs != null) {
                        onPaintCandle(paint, div, cs);
                    }
                }
            }
        }

        /// <summary>
        /// 绘制文字
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="div">图层</param>
        /// <param name="ts">文字</param>
        public virtual void onPaintText(FCPaint paint, ChartDiv div, TextShape ts) {
            String dt = ts.Text;
            if (dt == null || dt.Length == 0) {
                return;
            }
            int ciFieldName = m_dataSource.getColumnIndex(ts.FieldName);
            int ciStyle = m_dataSource.getColumnIndex(ts.StyleField);
            int ciClr = m_dataSource.getColumnIndex(ts.ColorField);
            for (int i = m_firstVisibleIndex; i <= m_lastVisibleIndex; i++) {
                int style = 0;
                //自定义样式
                if (ciStyle != -1) {
                    double defineStyle = m_dataSource.get3(i, ciStyle);
                    if (!double.IsNaN(defineStyle)) {
                        style = (int)defineStyle;
                    }
                }
                if (style == -10000) {
                    continue;
                }
                double value = m_dataSource.get3(i, ciFieldName);
                if (!double.IsNaN(value)) {
                    int scaleX = (int)getX(i);
                    int y = (int)getY(div, value, ts.AttachVScale);
                    FCSize tSize = paint.textSize(dt, ts.Font);
                    FCRect tRect = new FCRect(scaleX - tSize.cx / 2, y - tSize.cy / 2, scaleX + tSize.cx / 2, y + tSize.cy / 2);
                    long textColor = ts.TextColor;
                    if (ts.ColorField != FCDataTable.NULLFIELD) {
                        double defineColor = m_dataSource.get3(i, ciClr);
                        if (!double.IsNaN(defineColor)) {
                            textColor = (long)defineColor;
                        }
                    }
                    //绘制文字
                    drawText(paint, dt, textColor, ts.Font, new FCPoint(tRect.left, tRect.top));
                }
            }
        }

        /// <summary>
        /// 绘制标题
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="div">要绘制的层</param>
        public virtual void onPaintTitle(FCPaint paint, ChartDiv div) {
            ChartTitleBar titleBar = div.TitleBar;
            int width = div.Width, height = div.Height;
            if (titleBar.Visible) {
                FCRect tbRect = new FCRect(0, 0, width, height);
                if (titleBar.AllowUserPaint) {
                    titleBar.onPaint(paint, div, tbRect);
                }
                else {
                    int titleLeftPadding = m_leftVScaleWidth;
                    //创建字符串
                    int rightPadding = width - m_rightVScaleWidth - 2;
                    //画主标题
                    FCSize divNameSize = paint.textSize(div.TitleBar.Text, div.TitleBar.Font);
                    if (titleLeftPadding + divNameSize.cx <= Width - m_rightVScaleWidth) {
                        drawText(paint, titleBar.Text, titleBar.TextColor, titleBar.Font, new FCPoint(titleLeftPadding, 2));
                    }
                    titleLeftPadding += divNameSize.cx + 2;
                    //画各个线条的标题
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
                        ArrayList<CTitle> titles = div.TitleBar.Titles;
                        //循环画标题
                        int titleSize = titles.size();
                        for (int i = 0; i < titleSize; i++) {
                            CTitle title = titles.get(i);
                            if (!title.Visible || title.FieldTextMode == TextMode.None) {
                                continue;
                            }
                            double value = m_dataSource.get2(displayIndex, title.FieldName);
                            if (double.IsNaN(value)) {
                                value = 0;
                            }
                            String showTitle = String.Empty;
                            if (title.FieldTextMode != TextMode.Value) {
                                showTitle = title.FieldText + title.FieldTextSeparator;
                            }
                            if (title.FieldTextMode != TextMode.Field) {
                                int digit = title.Digit;
                                showTitle += FCStr.getValueByDigit(value, digit);
                            }
                            FCSize conditionSize = paint.textSize(showTitle, div.TitleBar.Font);
                            if (titleLeftPadding + conditionSize.cx + 8 > rightPadding) {
                                curLength++;
                                if (curLength <= div.TitleBar.MaxLine) {
                                    tTop += conditionSize.cy + 2;
                                    titleLeftPadding = m_leftVScaleWidth;
                                    rightPadding = Width - m_rightVScaleWidth;
                                }
                                else {
                                    break;
                                }
                                if (tTop + conditionSize.cy >= div.Height - div.HScale.Height) {
                                    break;
                                }
                            }
                            if (titleLeftPadding <= rightPadding) {
                                drawText(paint, showTitle, title.TextColor, titleBar.Font, new FCPoint(titleLeftPadding, tTop));
                                titleLeftPadding += conditionSize.cx + 8;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 绘制提示框
        /// </summary>
        /// <param name="paint">绘图对象</param>
        public virtual void onPaintToolTip(FCPaint paint) {
            if (!m_showingToolTip) {
                return;
            }
            BaseShape bs = selectShape(getTouchOverIndex(), 0);
            if (bs != null) {
                FCPoint touchP = TouchPoint;
                //获取触摸位置面板的digit值
                ChartDiv touchOverDiv = getTouchOverDiv();
                int digit = touchOverDiv.getVScale(bs.AttachVScale).Digit;
                if (touchOverDiv == null) return;
                int index = getIndex(touchP);
                if (index == -1) return;
                ChartToolTip toolTip = touchOverDiv.ToolTip;
                if (toolTip.AllowUserPaint) {
                    FCRect toolRect = new FCRect(0, 0, Width, Height);
                    toolTip.onPaint(paint, touchOverDiv, toolRect);
                    return;
                }
                int pWidth = 0;
                int pHeight = 0;
                StringBuilder sbValue = new StringBuilder();
                FCFont toolTipFont = toolTip.Font;
                double xValue = m_dataSource.getXValue(index);
                int sLeft = touchOverDiv.Left, sTop = touchOverDiv.Top;
                for (int t = 0; t < 2; t++) {
                    int x = touchP.x + 10;
                    int y = touchP.y + 2;
                    if (t == 0) {
                        sLeft = x + 2;
                        sTop = y;
                    }
                    FCSize xValueSize = new FCSize();
                    if (touchOverDiv.HScale.HScaleType == HScaleType.Date) {
                        String formatDate = m_hScaleFieldText + ":" + FCStr.convertNumToDate(xValue).ToString("yyyy-MM-dd");
                        xValueSize = paint.textSize(formatDate, toolTipFont);
                        pHeight = xValueSize.cy;
                        if (t == 1) {
                            drawText(paint, formatDate, toolTip.TextColor, toolTipFont, new FCPoint(sLeft, sTop));
                        }
                    }
                    else if (touchOverDiv.HScale.HScaleType == HScaleType.Number) {
                        String xValueStr = m_hScaleFieldText + ":" + xValue.ToString();
                        xValueSize = paint.textSize(xValueStr, toolTipFont);
                        pHeight = xValueSize.cy;
                        if (t == 1) {
                            drawText(paint, xValueStr, toolTip.TextColor, toolTipFont, new FCPoint(sLeft, sTop));
                        }
                    }
                    sTop += xValueSize.cy;
                    int[] fields = bs.getFields();
                    int fieldsLength = fields.Length;
                    if (fieldsLength > 0) {
                        for (int i = 0; i < fieldsLength; i++) {
                            String fieldText = bs.getFieldText(fields[i]);
                            double value = 0;
                            if (index >= 0) {
                                value = m_dataSource.get2(index, fields[i]);
                            }
                            String valueDigit = fieldText + ":" + FCStr.getValueByDigit(value, digit);
                            if (t == 1) {
                                drawText(paint, valueDigit, toolTip.TextColor, toolTipFont, new FCPoint(sLeft, sTop));
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
                        int width = Width, height = Height;
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
                            paint.fillRect(toolTip.BackColor, rectP);
                            paint.drawRect(toolTip.BorderColor, 1, 0, rectP);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 绘制纵坐标轴
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="div">要绘制的层</param>
        public virtual void onPaintVScale(FCPaint paint, ChartDiv div) {
            int divBottom = div.Height;
            ArrayList<int> gridYList = new ArrayList<int>();
            bool leftGridIsShow = false;
            int width = Width;
            if (m_leftVScaleWidth > 0) {
                VScale leftVScale = div.LeftVScale;
                ScaleGrid hGrid = div.HGrid;
                bool paintV = true, paintG = true;
                if (leftVScale.AllowUserPaint) {
                    FCRect leftVRect = new FCRect(0, 0, m_leftVScaleWidth, divBottom);
                    leftVScale.onPaint(paint, div, leftVRect);
                    paintV = false;
                }
                if (hGrid.AllowUserPaint) {
                    FCRect hGridRect = new FCRect(0, 0, width, divBottom);
                    hGrid.onPaint(paint, div, hGridRect);
                    paintG = false;
                }
                if (paintV || paintG) {
                    if (paintV && m_leftVScaleWidth <= width) {
                        paint.drawLine(leftVScale.ScaleColor, 1, 0, m_leftVScaleWidth, 0, m_leftVScaleWidth, divBottom - div.HScale.Height);
                    }
                    FCFont leftYFont = leftVScale.Font;
                    FCSize leftYSize = paint.textSize(" ", leftYFont);
                    //获取最小值和最大值
                    double min = leftVScale.VisibleMin;
                    double max = leftVScale.VisibleMax;
                    if (min == 0 && max == 0) {
                        VScale rightVScale = div.RightVScale;
                        if (rightVScale.VisibleMin != 0 || rightVScale.VisibleMax != 0) {
                            min = rightVScale.VisibleMin;
                            max = rightVScale.VisibleMax;
                            FCPoint point1 = new FCPoint(0, div.Top + div.TitleBar.Height);
                            double value1 = getNumberValue(div, point1, AttachVScale.Right);
                            FCPoint point2 = new FCPoint(0, div.Bottom - div.HScale.Height);
                            double value2 = getNumberValue(div, point2, AttachVScale.Right);
                            max = Math.Max(value1, value2);
                            min = Math.Min(value1, value2);
                        }
                    }
                    else {
                        FCPoint point1 = new FCPoint(0, div.Top + div.TitleBar.Height);
                        double value1 = getNumberValue(div, point1, AttachVScale.Left);
                        FCPoint point2 = new FCPoint(0, div.Bottom - div.HScale.Height);
                        double value2 = getNumberValue(div, point2, AttachVScale.Left);
                        max = Math.Max(value1, value2);
                        min = Math.Min(value1, value2);
                    }
                    ArrayList<double> scaleStepList = leftVScale.getScaleSteps();
                    if (scaleStepList.size() == 0) {
                        scaleStepList = getVScaleStep(max, min, div, leftVScale);
                    }
                    //循环遍历所有的值
                    float lY = -1;
                    int stepSize = scaleStepList.size();
                    for (int i = 0; i < stepSize; i++) {
                        double lValue = scaleStepList.get(i) / leftVScale.Magnitude;
                        //计算百分比
                        if (lValue != 0 && leftVScale.Type == VScaleType.Percent) {
                            double baseValue = getVScaleBaseValue(div, leftVScale, m_firstVisibleIndex) / leftVScale.Magnitude;
                            lValue = 100 * (lValue - baseValue * leftVScale.Magnitude) / lValue;
                        }
                        String number = FCStr.getValueByDigit(lValue, leftVScale.Digit);
                        if (div.LeftVScale.Type == VScaleType.Percent) {
                            number += "%";
                        }
                        int y = (int)getY(div, scaleStepList.get(i), AttachVScale.Left);
                        leftYSize = paint.textSize(number, leftYFont);
                        if (y - leftYSize.cy / 2 < 0 || y + leftYSize.cy / 2 > divBottom) {
                            continue;
                        }
                        //网格线
                        if (hGrid.Visible && paintG) {
                            leftGridIsShow = true;
                            if (!gridYList.Contains(y)) {
                                gridYList.add(y);
                                paint.drawLine(hGrid.GridColor, 1, hGrid.LineStyle, m_leftVScaleWidth, y, width - m_rightVScaleWidth, y);
                            }
                        }
                        if (paintV) {
                            //画短线
                            drawThinLine(paint, leftVScale.ScaleColor, 1, m_leftVScaleWidth - 4, y, m_leftVScaleWidth, y);
                            //反转坐标
                            if (leftVScale.Reverse) {
                                if (lY != -1 && y - leftYSize.cy / 2 < lY) {
                                    continue;
                                }
                                lY = y + leftYSize.cy / 2;
                            }
                            else {
                                if (lY != -1 && y + leftYSize.cy / 2 > lY) {
                                    continue;
                                }
                                lY = y - leftYSize.cy / 2;
                            }
                            long scaleTextColor = leftVScale.TextColor;
                            long scaleTextColor2 = leftVScale.TextColor2;
                            if (leftVScale.Type != VScaleType.Percent) {
                                if (scaleTextColor2 != FCColor.None && lValue < 0) {
                                    scaleTextColor = scaleTextColor2;
                                }
                            }
                            else {
                                if (scaleTextColor2 != FCColor.None && scaleStepList.get(i) < leftVScale.MidValue) {
                                    scaleTextColor = scaleTextColor2;
                                }
                            }
                            if (leftVScale.Type != VScaleType.Percent && leftVScale.NumberStyle == NumberStyle.UnderLine) {
                                String[] nbs = number.Split(new String[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                                if (nbs.Length >= 1) {
                                    drawText(paint, nbs[0], scaleTextColor, leftYFont, new FCPoint(m_leftVScaleWidth - 10 - leftYSize.cx, y - leftYSize.cy / 2));
                                }
                                if (nbs.Length >= 2) {
                                    FCSize decimalSize = paint.textSize(nbs[0], leftYFont);
                                    FCSize size2 = paint.textSize(nbs[1], leftYFont);
                                    drawText(paint, nbs[1], scaleTextColor, leftYFont, new FCPoint(m_leftVScaleWidth - 10 - leftYSize.cx
                                        + decimalSize.cx, y - leftYSize.cy / 2));
                                    drawThinLine(paint, scaleTextColor, 1, m_leftVScaleWidth - 10 - leftYSize.cx
                                    + decimalSize.cx, y + leftYSize.cy / 2,
                                    m_leftVScaleWidth - 10 - leftYSize.cx + decimalSize.cx + size2.cx, y + leftYSize.cy / 2);
                                }
                            }
                            else {
                                drawText(paint, number, scaleTextColor, leftYFont, new FCPoint(m_leftVScaleWidth - 10 - leftYSize.cx, y - leftYSize.cy / 2));
                            }
                            //黄金分割
                            if (leftVScale.Type == VScaleType.GoldenRatio) {
                                String goldenRatio = String.Empty;
                                if (i == 1) goldenRatio = "19.1%";
                                else if (i == 2) goldenRatio = "38.2%";
                                else if (i == 4) goldenRatio = "61.8%";
                                else if (i == 5) goldenRatio = "80.9%";
                                if (goldenRatio != null && goldenRatio.Length > 0) {
                                    FCSize goldenRatioSize = paint.textSize(goldenRatio, leftYFont);
                                    drawText(paint, goldenRatio, scaleTextColor, leftYFont, new FCPoint(m_leftVScaleWidth - 10 - goldenRatioSize.cx, y + leftYSize.cy / 2));
                                }
                            }
                        }
                    }
                    if (div.LeftVScale.Magnitude != 1 && paintV) {
                        //获取字符大小
                        String str = "X" + leftVScale.Magnitude.ToString();
                        FCSize sizeF = paint.textSize(str, leftYFont);
                        //计算xy
                        int x = m_leftVScaleWidth - sizeF.cx - 6;
                        int y = div.Height - div.HScale.Height - sizeF.cy - 2;
                        //画矩形框
                        paint.drawRect(leftVScale.ScaleColor, 1, 0, new FCRect(x - 1, y - 1, x + sizeF.cx + 1, y + sizeF.cy));
                        //画文字
                        drawText(paint, str, leftVScale.TextColor, leftYFont, new FCPoint(x, y));
                    }
                }
            }
            //画右侧Y轴
            if (m_rightVScaleWidth > 0) {
                VScale rightVScale = div.RightVScale;
                ScaleGrid hGrid = div.HGrid;
                bool paintV = true, paintG = true;
                if (rightVScale.AllowUserPaint) {
                    FCRect rightVRect = new FCRect(width - m_rightVScaleWidth, 0, width, divBottom);
                    rightVScale.onPaint(paint, div, rightVRect);
                    paintV = false;
                }
                if (hGrid.AllowUserPaint) {
                    FCRect hGridRect = new FCRect(0, 0, width, divBottom);
                    hGrid.onPaint(paint, div, hGridRect);
                    paintG = false;
                }
                if (paintV || paintG) {
                    if (paintV && width - m_rightVScaleWidth >= m_leftVScaleWidth) {
                        paint.drawLine(rightVScale.ScaleColor, 1, 0, width - m_rightVScaleWidth, 0, width - m_rightVScaleWidth, divBottom - div.HScale.Height);
                    }
                    FCFont rightYFont = rightVScale.Font;
                    FCSize rightYSize = paint.textSize(" ", rightYFont);
                    //获取最小值和最大值
                    double min = rightVScale.VisibleMin;
                    double max = rightVScale.VisibleMax;
                    if (min == 0 && max == 0) {
                        VScale leftVScale = div.LeftVScale;
                        if (leftVScale.VisibleMin != 0 || leftVScale.VisibleMax != 0) {
                            min = leftVScale.VisibleMin;
                            max = leftVScale.VisibleMax;
                            FCPoint point1 = new FCPoint(0, div.Top + div.TitleBar.Height);
                            double value1 = getNumberValue(div, point1, AttachVScale.Left);
                            FCPoint point2 = new FCPoint(0, div.Bottom - div.HScale.Height);
                            double value2 = getNumberValue(div, point2, AttachVScale.Left);
                            max = Math.Max(value1, value2);
                            min = Math.Min(value1, value2);
                        }
                    }
                    else {
                        FCPoint point1 = new FCPoint(0, div.Top + div.TitleBar.Height);
                        double value1 = getNumberValue(div, point1, AttachVScale.Right);
                        FCPoint point2 = new FCPoint(0, div.Bottom - div.HScale.Height);
                        double value2 = getNumberValue(div, point2, AttachVScale.Right);
                        max = Math.Max(value1, value2);
                        min = Math.Min(value1, value2);
                    }
                    //计算显示值
                    ArrayList<double> scaleStepList = rightVScale.getScaleSteps();
                    if (scaleStepList.size() == 0) {
                        scaleStepList = getVScaleStep(max, min, div, rightVScale);
                    }
                    //循环遍历所有的值
                    float lY = -1;
                    int stepSize = scaleStepList.size();
                    for (int i = 0; i < stepSize; i++) {
                        double rValue = scaleStepList.get(i) / rightVScale.Magnitude;
                        //计算百分比
                        if (rValue != 0 && rightVScale.Type == VScaleType.Percent) {
                            double baseValue = getVScaleBaseValue(div, rightVScale, m_lastVisibleIndex) / rightVScale.Magnitude;
                            rValue = 100 * (rValue - baseValue * rightVScale.Magnitude) / rValue;
                        }
                        String number = FCStr.getValueByDigit(rValue, rightVScale.Digit);
                        if (rightVScale.Type == VScaleType.Percent) {
                            number += "%";
                        }
                        int y = (int)getY(div, scaleStepList.get(i), AttachVScale.Right);
                        rightYSize = paint.textSize(number, rightYFont);
                        if (y - rightYSize.cy / 2 < 0 || y + rightYSize.cy / 2 > divBottom) {
                            continue;
                        }
                        //网格
                        if (hGrid.Visible && paintG && !leftGridIsShow) {
                            if (!gridYList.Contains(y)) {
                                gridYList.add(y);
                                paint.drawLine(hGrid.GridColor, 1, hGrid.LineStyle, m_leftVScaleWidth, y, width - m_rightVScaleWidth, y);
                            }
                        }
                        if (paintV) {
                            //画短线
                            drawThinLine(paint, rightVScale.ScaleColor, 1, width - m_rightVScaleWidth, y, width - m_rightVScaleWidth + 4, y);
                            //反转坐标
                            if (rightVScale.Reverse) {
                                if (lY != -1 && y - rightYSize.cy / 2 < lY) {
                                    continue;
                                }
                                lY = y + rightYSize.cy / 2;
                            }
                            else {
                                if (lY != -1 && y + rightYSize.cy / 2 > lY) {
                                    continue;
                                }
                                lY = y - rightYSize.cy / 2;
                            }
                            long scaleTextColor = rightVScale.TextColor;
                            long scaleTextColor2 = rightVScale.TextColor2;
                            if (rightVScale.Type != VScaleType.Percent) {
                                if (scaleTextColor2 != FCColor.None && rValue < 0) {
                                    scaleTextColor = scaleTextColor2;
                                }
                            }
                            else {
                                if (scaleTextColor2 != FCColor.None && scaleStepList.get(i) < rightVScale.MidValue) {
                                    scaleTextColor = scaleTextColor2;
                                }
                            }
                            if (rightVScale.Type != VScaleType.Percent && rightVScale.NumberStyle == NumberStyle.UnderLine) {
                                String[] nbs = number.Split(new String[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                                if (nbs.Length >= 1) {
                                    drawText(paint, nbs[0], scaleTextColor, rightYFont, new FCPoint(width - m_rightVScaleWidth + 10,
                                    y - rightYSize.cy / 2));
                                }
                                if (nbs.Length >= 2) {
                                    FCSize decimalSize = paint.textSize(nbs[0], rightYFont);
                                    FCSize size2 = paint.textSize(nbs[1], rightYFont);
                                    drawText(paint, nbs[1], scaleTextColor, rightYFont, new FCPoint(width - m_rightVScaleWidth + 10
                                        + decimalSize.cx, y - rightYSize.cy / 2));
                                    drawThinLine(paint, scaleTextColor, 1, width - m_rightVScaleWidth + 10
                                    + decimalSize.cx, y + rightYSize.cy / 2,
                                    Width - m_rightVScaleWidth + 10 + decimalSize.cx + size2.cx, y + rightYSize.cy / 2);
                                }
                            }
                            else {
                                drawText(paint, number, scaleTextColor, rightYFont, new FCPoint(width - m_rightVScaleWidth + 10,
                                y - rightYSize.cy / 2));
                            }
                            //黄金分割
                            if (rightVScale.Type == VScaleType.GoldenRatio) {
                                String goldenRatio = String.Empty;
                                if (i == 1) goldenRatio = "19.1%";
                                else if (i == 2) goldenRatio = "38.2%";
                                else if (i == 4) goldenRatio = "61.8%";
                                else if (i == 5) goldenRatio = "80.9%";
                                if (goldenRatio != null && goldenRatio.Length > 0) {
                                    drawText(paint, goldenRatio, scaleTextColor, rightYFont, new FCPoint(width - m_rightVScaleWidth + 10,
                                    y + rightYSize.cy / 2));
                                }
                            }
                        }
                    }
                    if (rightVScale.Magnitude != 1 && paintV) {
                        //获取字符大小
                        String str = "X" + rightVScale.Magnitude.ToString();
                        FCSize sizeF = paint.textSize(str, rightYFont);
                        //计算xy
                        int x = width - m_rightVScaleWidth + 6;
                        int y = div.Height - div.HScale.Height - sizeF.cy - 2;
                        //画矩形框
                        paint.drawRect(rightVScale.ScaleColor, 1, 0, new FCRect(x - 1, y - 1, x + sizeF.cx + 1, y + sizeF.cy));
                        //画文字
                        drawText(paint, str, rightVScale.TextColor, rightYFont, new FCPoint(x, y));
                    }
                }
            }
        }

        /// <summary>
        /// 触摸滚动的方法
        /// </summary>
        /// <param name="key">按键</param>
        public override void onKeyDown(char key) {
            base.onKeyDown(key);
            FCHost host = Native.Host;
            if (!(host.isKeyPress(0x10) || host.isKeyPress(0x11))) {
                m_isTouchMove = false;
                m_showingToolTip = false;
                ScrollType operatorType = ScrollType.None;
                switch ((int)key) {
                    //向左键
                    case 37:
                        if (m_reverseHScale)
                            operatorType = ScrollType.Right;
                        else
                            operatorType = ScrollType.Left;
                        break;
                    //向右键
                    case 39:
                        if (m_reverseHScale)
                            operatorType = ScrollType.Left;
                        else
                            operatorType = ScrollType.Right;
                        break;
                    //向上键
                    case 38:
                        operatorType = ScrollType.ZoomOut;
                        break;
                    //向下键
                    case 40:
                        operatorType = ScrollType.ZoomIn;
                        break;
                    //Escape键
                    case 27:
                        ArrayList<ChartDiv> divsCopy = getDivs();
                        if (divsCopy.size() > 0) {
                            clearSelectedDiv();
                            invalidate();
                        }
                        break;
                }
                if (operatorType != ScrollType.None) {
                    changeChart(operatorType, 1);
                    return;
                }
            }
        }

        /// <summary>
        /// 控件添加方法
        /// </summary>
        public override void onLoad() {
            base.onLoad();
            startTimer(m_timerID, 10);
            if (m_dataSource == null) {
                m_dataSource = new FCDataTable();
            }
        }

        /// <summary>
        /// 触摸离开的方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onTouchDown(FCTouchInfo touchInfo) {
            base.onTouchDown(touchInfo);
            if (touchInfo.m_firstTouch && touchInfo.m_clicks == 2) {
                clearSelectedShape();
                m_showCrossLine = !m_showCrossLine;
                invalidate();
                return;
            }
            FCPoint mp = touchInfo.m_firstPoint;
            int width = Width;
            m_userResizeDiv = null;
            int shapeCount = SelectedShape == null ? 0 : 1;
            ArrayList<ChartDiv> divsCopy = getDivs();
            m_hResizeType = 0;
            if (touchInfo.m_firstTouch) {
                clearSelectedPlot();
                //选中层
                ChartDiv touchOverDiv = getTouchOverDiv();
                foreach (ChartDiv div in divsCopy) {
                    if (div == touchOverDiv) {
                        div.Selected = true;
                    }
                    else {
                        div.Selected = false;
                    }
                }
                if (touchInfo.m_clicks == 1) {
                    closeSelectArea();
                    m_crossStopIndex = getTouchOverIndex();
                    m_cross_y = mp.y;
                    //设置十字线的位置
                    if (m_showCrossLine && m_crossLineMoveMode == CrossLineMoveMode.AfterClick) {
                        m_crossStopIndex = getTouchOverIndex();
                        m_cross_y = TouchPoint.y;
                        m_isScrollCross = false;
                    }
                    //横向的拖动
                    if (m_canResizeH) {
                        if (mp.x >= m_leftVScaleWidth - 4 && mp.x <= m_leftVScaleWidth + 4) {
                            m_hResizeType = 1;
                            goto OutLoop;
                        }
                        else if (mp.x >= Width - m_rightVScaleWidth - 4 && mp.x <= Width - m_rightVScaleWidth + 4) {
                            m_hResizeType = 2;
                            goto OutLoop;
                        }
                    }
                    //纵向的拖动
                    if (m_canResizeV) {
                        int pIndex = 0;
                        //当触摸到纵向下边线上时，认为是需要调整大小
                        foreach (ChartDiv cDiv in divsCopy) {
                            pIndex++;
                            if (pIndex == divsCopy.size()) {
                                break;
                            }
                            FCRect resizeRect = new FCRect(0, cDiv.Bottom - 4, cDiv.Width, cDiv.Bottom + 4);
                            if (mp.x >= resizeRect.left && mp.x <= resizeRect.right
                                && mp.y >= resizeRect.top && mp.y <= resizeRect.bottom) {
                                this.Cursor = FCCursors.SizeNS;
                                m_userResizeDiv = cDiv;
                                goto OutLoop;
                            }
                        }
                    }
                    //线条的选中，以及是否显示选中框
                    if ((mp.x >= m_leftVScaleWidth && mp.x <= width - m_rightVScaleWidth)) {
                        if (touchOverDiv != null) {
                            ArrayList<FCPlot> plotsCopy = touchOverDiv.getPlots(SortType.DESC);
                            foreach (FCPlot lsb in plotsCopy) {
                                if (lsb.Enabled && lsb.Visible && lsb.onSelect()) {
                                    m_movingPlot = lsb;
                                    lsb.onMoveStart();
                                    //设置图层
                                    ArrayList<double> zorders = new ArrayList<double>();
                                    ArrayList<FCPlot> plots = touchOverDiv.getPlots(SortType.DESC);
                                    int plotSize = plots.size();
                                    for (int i = 0; i < plotSize; i++) {
                                        zorders.add(plots.get(i).ZOrder);
                                    }
                                    lsb.ZOrder = (int)FCScript.maxValue(zorders.ToArray(), zorders.size()) + 1;
                                }
                            }
                        }
                        if (m_movingPlot != null) {
                            m_movingPlot.Selected = true;
                            //当有选中的线条时，要清空选中
                            if (shapeCount != 0) {
                                clearSelectedShape();
                            }
                        }
                        else {
                            BaseShape bs = selectShape(m_crossStopIndex, 1);
                            //认为是显示选中框
                            ChartDiv div = null;
                            if (bs == null) {
                                div = touchOverDiv;
                                if (div != null && div.SelectArea.Enabled) {
                                    if (mp.y >= div.Top + 2 && mp.y <= div.Bottom - div.HScale.Height - 2) {
                                        m_showingSelectArea = true;
                                    }
                                }
                            }
                        }
                    }
                OutLoop: ;
                }
            }
            else {
                //按右键时清除
                m_isTouchMove = true;
                m_showingToolTip = false;
            }
            //保存上次点击的位置
            m_lastTouchClickPoint = mp;
            //设置拖动线条
            if (m_canMoveShape) {
                if (SelectedShape != null) {
                    m_movingShape = SelectedShape;
                }
            }
            invalidate();
        }

        /// <summary>
        /// 触摸抬起的方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onTouchMove(FCTouchInfo touchInfo) {
            base.onTouchMove(touchInfo);
            FCPoint mp = touchInfo.m_firstPoint;
            if (mp.x != m_lastTouchMovePoint.x || mp.y != m_lastTouchMovePoint.y) {
                int width = Width;
                m_isTouchMove = true;
                m_showingToolTip = false;
                ArrayList<ChartDiv> divsCopy = getDivs();
                foreach (ChartDiv div in divsCopy) {
                    bool resize = false;
                    if (div.SelectArea.Visible && div.SelectArea.CanResize) {
                        resize = true;
                    }
                    else {
                        //判断是否准备画选中框
                        if (m_showingSelectArea) {
                            if (touchInfo.m_firstTouch) {
                                int subX = m_lastTouchClickPoint.x - m_lastTouchMovePoint.x;
                                int subY = m_lastTouchMovePoint.y - m_lastTouchClickPoint.y;
                                if (Math.Abs(subX) > m_hScalePixel * 2 || Math.Abs(subY) > m_hScalePixel * 2) {
                                    m_showingSelectArea = false;
                                    div.SelectArea.Visible = true;
                                    div.SelectArea.CanResize = true;
                                    resize = true;
                                }
                            }
                        }
                    }
                    if (resize && touchInfo.m_firstTouch) {
                        //获取矩形的属性
                        int x1 = m_lastTouchClickPoint.x;
                        int y1 = m_lastTouchClickPoint.y;
                        int x2 = mp.x;
                        int y2 = mp.y;
                        //纠正位置
                        if (x2 < m_leftVScaleWidth) {
                            x2 = m_leftVScaleWidth;
                        }
                        else if (x2 > Width - m_rightVScaleWidth) {
                            x2 = Width - m_rightVScaleWidth;
                        }
                        if (y2 > div.Bottom - div.HScale.Height) {
                            y2 = div.Bottom - div.HScale.Height;
                        }
                        else if (y2 < div.Top + div.TitleBar.Height) {
                            y2 = div.Top + div.TitleBar.Height;
                        }
                        //生成矩形
                        int bx = 0, by = 0, bwidth = 0, bheight = 0;
                        FCPlot.rectangleXYWH(x1, y1 - div.Top, x2, y2 - div.Top, ref bx, ref by, ref bwidth, ref bheight);
                        FCRect bounds = new FCRect(bx, by, bx + bwidth, by + bheight);
                        div.SelectArea.Bounds = bounds;
                        invalidate();
                        m_lastTouchMovePoint = mp;
                        return;
                    }
                    if (div.SelectArea.Visible) {
                        return;
                    }
                }
                m_lastTouchMoveTime = DateTime.Now;
                if (m_movingPlot != null && touchInfo.m_firstTouch) {
                    m_movingPlot.onMoving();
                }
                else {
                    //横向调整
                    if (m_canResizeH) {
                        if (m_hResizeType == 0) {
                            if ((mp.x >= m_leftVScaleWidth - 4 && mp.x <= m_leftVScaleWidth + 4) ||
                            (mp.x >= width - m_rightVScaleWidth - 4 && mp.x <= width - m_rightVScaleWidth + 4)) {
                                this.Cursor = FCCursors.SizeWE;
                                goto OutLoop;
                            }
                        }
                        else {
                            this.Cursor = FCCursors.SizeWE;
                            goto OutLoop;
                        }
                    }
                    //纵向调整
                    if (m_canResizeV) {
                        if (m_userResizeDiv == null) {
                            int pIndex = 0;
                            //当触摸到纵向下边线上时，认为是需要调整大小
                            foreach (ChartDiv cDiv in divsCopy) {
                                pIndex++;
                                if (pIndex == divsCopy.size()) {
                                    break;
                                }
                                FCRect resizeRect = new FCRect(0, cDiv.Bottom - 4, Width, cDiv.Bottom + 4);
                                if (mp.x >= resizeRect.left && mp.x <= resizeRect.right && mp.y >= resizeRect.top && mp.y <= resizeRect.bottom) {
                                    this.Cursor = FCCursors.SizeNS;
                                    goto OutLoop;
                                }
                            }
                        }
                        else {
                            this.Cursor = FCCursors.SizeNS;
                            goto OutLoop;
                        }
                    }
                    //恢复触摸
                    this.Cursor = FCCursors.Arrow;
                OutLoop: ;
                }
                m_crossStopIndex = getTouchOverIndex();
                m_cross_y = mp.y;
                //画十字线
                if (m_showCrossLine && m_crossLineMoveMode == CrossLineMoveMode.FollowTouch) {
                    m_isScrollCross = false;
                }
                invalidate();
            }
            m_lastTouchMovePoint = mp;
        }

        /// <summary>
        /// 触摸按下的方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onTouchUp(FCTouchInfo touchInfo) {
            base.onTouchUp(touchInfo);
            FCPoint mp = touchInfo.m_firstPoint;
            bool needUpdate = false;
            //移除移动的图形
            if (m_movingShape != null) {
                m_movingShape = null;
            }
            //禁止移动选中框
            ArrayList<ChartDiv> divsCopy = getDivs();
            foreach (ChartDiv div in divsCopy) {
                if (div.SelectArea.Visible) {
                    div.SelectArea.CanResize = false;
                    invalidate();
                    return;
                }
            }
            this.Cursor = FCCursors.Arrow;
            //获取触摸按下的控件
            BaseShape selectedShape = SelectedShape;
            if (m_hResizeType == 0 && m_userResizeDiv == null && touchInfo.m_firstTouch && m_canMoveShape && selectedShape != null) {
                ChartDiv curDiv = findDiv(selectedShape);
                //循环遍历所有的层
                foreach (ChartDiv cDiv in divsCopy) {
                    if (mp.y >= cDiv.Top && mp.y <= cDiv.Bottom) {
                        if (cDiv == curDiv) {
                            break;
                        }
                        //K线
                        if (!cDiv.containsShape(selectedShape)) {
                            foreach (ChartDiv div in divsCopy) {
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
            //清除移动中的画线工具
            if (m_movingPlot != null) {
                m_movingPlot = null;
            }
            //重新修改层的布局
            if (resizeDiv()) {
                needUpdate = true;
            }
            //完全重绘
            if (needUpdate) {
                update();
            }
            invalidate();
        }

        /// <summary>
        /// 触摸滚动方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onTouchWheel(FCTouchInfo touchInfo) {
            base.onTouchMove(touchInfo);
            if (touchInfo.m_delta > 0) {
                //放大
                changeChart(ScrollType.ZoomOut, 1);
            }
            else {
                //缩小
                changeChart(ScrollType.ZoomIn, 1);
            }
        }

        /// <summary>
        /// 按键弹起的方法
        /// </summary>
        /// <param name="key">按键</param>
        public override void onKeyUp(char key) {
            base.onKeyUp(key);
            if (m_scrollStep != 1) {
                bool redraw = false;
                if (m_scrollStep > 6) {
                    redraw = true;
                }
                m_scrollStep = 1;
                if (redraw) {
                    update();
                    invalidate();
                }
            }
        }

        /// <summary>
        /// 重绘前景方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void onPaintForeground(FCPaint paint, FCRect clipRect) {
            if (Visible) {
                ArrayList<ChartDiv> divsCopy = getDivs();
                FCPoint offset = paint.getOffset();
                foreach (ChartDiv div in divsCopy) {
                    int offsetX = offset.x + div.Left;
                    int offsetY = offset.y + div.Top;
                    paint.setOffset(new FCPoint(offsetX, offsetY));
                    FCRect divClipRect = new FCRect(0, 0, div.Width, div.Height);
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
                //画拖动变线
                onPaintResizeLine(paint);
                //画股指提示
                onPaintToolTip(paint);
                //画拖动图标
                onPaintIcon(paint);
            }
        }

        /// <summary>
        /// 秒表回调方法
        /// </summary>
        /// <param name="timerID">编号</param>
        public override void onTimer(int timerID) {
            base.onTimer(timerID);
            if (Visible && m_timerID == timerID) {
                FCNative native = Native;
                if (this == native.HoveredControl) {
                    checkToolTip();
                }
            }
        }

        /// <summary>
        /// 图像滚动类型
        /// </summary>
        public enum ScrollType {
            /// <summary>
            /// 无
            /// </summary>
            None = 0,
            /// <summary>
            /// 左滚
            /// </summary>
            Left = 1,
            /// <summary>
            /// 右滚
            /// </summary>
            Right = 2,
            /// <summary>
            /// 缩小
            /// </summary>
            ZoomIn = 3,
            /// <summary>
            /// 放大
            /// </summary>
            ZoomOut = 4
        }

        /// <summary>
        /// 获取最大显示条数
        /// </summary>
        /// <param name="hScalePixel">数据间隔</param>
        /// <param name="pureH">横向宽度</param>
        /// <returns>最大显示条数</returns>
        private int getMaxVisibleCount(double hScalePixel, int pureH) {
            return (int)(pureH / hScalePixel);
        }

        /// <summary>
        /// 获取K线最大值的显示位置
        /// </summary>
        /// <param name="scaleX">横坐标</param>
        /// <param name="scaleY">纵坐标</param>
        /// <param name="stringWidth">文字的宽度</param>
        /// <param name="stringHeight">文字的高度</param>
        /// <param name="actualWidth">横向宽度</param>
        /// <param name="leftVScaleWidth">左侧纵轴宽度</param>
        /// <param name="rightVScaleWidth">右侧纵轴宽度</param>
        /// <param name="x">最大值的横坐标</param>
        /// <param name="y">最大值的纵坐标</param>
        private void getCandleMaxStringPoint(float scaleX, float scaleY, float stringWidth, float stringHeight, int actualWidth, int leftVScaleWidth, int rightVScaleWidth, ref float x, ref float y) {
            if (scaleX < leftVScaleWidth + stringWidth) {
                x = scaleX;
            }
            else if (scaleX > actualWidth - rightVScaleWidth - stringWidth) {
                x = scaleX - stringWidth;
            }
            else {
                if (scaleX < actualWidth / 2) {
                    x = scaleX - stringWidth;
                }
                else {
                    x = scaleX;
                }
            }
            y = scaleY + stringHeight / 2;
        }

        /// <summary>
        /// 获取K线最小值的显示位置
        /// </summary>
        /// <param name="scaleX">横坐标</param>
        /// <param name="scaleY">纵坐标</param>
        /// <param name="stringWidth">文字的宽度</param>
        /// <param name="stringHeight">文字的高度</param>
        /// <param name="actualWidth">横向宽度</param>
        /// <param name="leftVScaleWidth">左侧纵轴宽度</param>
        /// <param name="rightVScaleWidth">右侧纵轴宽度</param>
        /// <param name="x">最小值的横坐标</param>
        /// <param name="y">最小值的纵坐标</param>
        private void getCandleMinStringPoint(float scaleX, float scaleY, float stringWidth, float stringHeight, int actualWidth, int leftVScaleWidth, int rightVScaleWidth, ref float x, ref float y) {
            if (scaleX < leftVScaleWidth + stringWidth) {
                x = scaleX;
            }
            else if (scaleX > actualWidth - rightVScaleWidth - stringWidth) {
                x = scaleX - stringWidth;
            }
            else {
                if (scaleX < actualWidth / 2) {
                    x = scaleX - stringWidth;
                }
                else {
                    x = scaleX;
                }
            }
            y = scaleY - stringHeight * 3 / 2;
        }

        /// <summary>
        /// 获取某坐标对应的索引
        /// </summary>
        /// <param name="x">横坐标</param>
        /// <param name="leftVScaleWidth">左侧纵轴的高度</param>
        /// <param name="hScalePixel">数据间隔</param>
        /// <param name="firstVisibleIndex">首先可见索引</param>
        /// <returns>坐标</returns>
        private int getChartIndex(int x, int leftVScaleWidth, double hScalePixel, int firstVisibleIndex) {
            return (int)((x - leftVScaleWidth) / hScalePixel + firstVisibleIndex);
        }

        /// <summary>
        /// 获取阳K线的高度
        /// </summary>
        /// <param name="close">收盘价</param>
        /// <param name="open">开盘价</param>
        /// <param name="max">最高价</param>
        /// <param name="min">最低价</param>
        /// <param name="divPureV">层高度</param>
        /// <returns>高度</returns>
        private float getUpCandleHeight(double close, double open, double max, double min, float divPureV) {
            if (close - open == 0) {
                return 1;
            }
            else {
                return (float)((close - open) / (max - min) * divPureV);
            }
        }

        /// <summary>
        /// 获取阴K线的高度
        /// </summary>
        /// <param name="close">收盘价</param>
        /// <param name="open">开盘价</param>
        /// <param name="max">最高价</param>
        /// <param name="min">最低价</param>
        /// <param name="divPureV">层高度</param>
        /// <returns>高度</returns>
        private float getDownCandleHeight(double close, double open, double max, double min, float divPureV) {
            if (close - open == 0) {
                return 1;
            }
            else {
                return (float)((open - close) / (max - min) * divPureV);
            }
        }

        /// <summary>
        /// 左滚
        /// </summary>
        /// <param name="step">步长</param>
        /// <param name="dateCount">数据条数</param>
        /// <param name="hScalePixel">数据间隔</param>
        /// <param name="pureH">横向宽度</param>
        /// <param name="fIndex">首先可见索引号</param>
        /// <param name="lIndex">最后可见索引号</param>
        private void scrollLeft(int step, int dateCount, double hScalePixel, int pureH, ref int fIndex, ref int lIndex) {
            int max = getMaxVisibleCount(hScalePixel, pureH);
            int right = -1;
            if (dateCount > max) {
                right = max - (lIndex - fIndex);
                if (right > 1) {
                    fIndex = lIndex - max + 1;
                    if (fIndex > lIndex) {
                        fIndex = lIndex;
                    }
                }
                else {
                    if (fIndex - step >= 0) {
                        fIndex = fIndex - step;
                        lIndex = lIndex - step;
                    }
                    else {
                        if (fIndex != 0) {
                            lIndex = lIndex - fIndex;
                            fIndex = 0;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 右滚
        /// </summary>
        /// <param name="step">步长</param>
        /// <param name="dataCount">数据条数</param>
        /// <param name="hScalePixel">数据间隔</param>
        /// <param name="pureH">横向宽度</param>
        /// <param name="fIndex">首先可见索引号</param>
        /// <param name="lIndex">最后可见索引号</param>
        private void scrollRight(int step, int dataCount, double hScalePixel, int pureH, ref int fIndex, ref int lIndex) {
            int max = getMaxVisibleCount(hScalePixel, pureH);
            if (dataCount > max) {
                if (lIndex < dataCount - 1) {
                    if (lIndex + step > dataCount - 1) {
                        fIndex = fIndex + dataCount - lIndex;
                        lIndex = dataCount - 1;
                    }
                    else {
                        fIndex = fIndex + step;
                        lIndex = lIndex + step;
                    }
                }
                else {
                    fIndex = lIndex - (int)(max * 0.9);
                    if (fIndex > lIndex) {
                        fIndex = lIndex;
                    }
                }
            }
        }

        /// <summary>
        /// 获取纵轴某坐标的值
        /// </summary>
        /// <param name="y">纵坐标</param>
        /// <param name="max">最大值</param>
        /// <param name="min">最小值</param>
        /// <param name="vHeight">层高度</param>
        /// <returns>数值</returns>
        private double getVScaleValue(int y, double max, double min, float vHeight) {
            double every = (max - min) / vHeight;
            return max - y * every;
        }

        /// <summary>
        /// 修正可见索引
        /// </summary>
        /// <param name="dataCount">数据条数</param>
        /// <param name="first">首先可见索引号</param>
        /// <param name="last">最后可见索引号</param>
        private void correctVisibleRecord(int dataCount, ref int first, ref int last) {
            if (dataCount > 0) {
                if (first == -1) {
                    first = 0;
                }
                if (last == -1) {
                    last = 0;
                }
                if (first > last) {
                    first = last;
                }
                if (last < first) {
                    last = first;
                }
            }
            else {
                first = -1;
                last = -1;
            }
        }

        /// <summary>
        /// 重置十字线索引
        /// </summary>
        /// <param name="dataCount">数据条数</param>
        /// <param name="maxVisibleRecord">最大显示记录数</param>
        /// <param name="crossStopIndex">十字线索引</param>
        /// <param name="firstL">首先可见索引号</param>
        /// <param name="lastL">最后可见索引号</param>
        /// <returns>修正后的十字线索引</returns>
        private int resetCrossOverIndex(int dataCount, int maxVisibleRecord, int crossStopIndex, int firstL, int lastL) {
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

        /// <summary>
        /// 缩小
        /// </summary>
        /// <param name="pureH">横向宽度</param>
        /// <param name="dataCount">数据条数</param>
        /// <param name="findex">首先可见索引号</param>
        /// <param name="lindex">最后可见索引号</param>
        /// <param name="hScalePixel">数据间隔</param>
        private void zoomIn(int pureH, int dataCount, ref int findex, ref int lindex, ref double hScalePixel) {
            int max = -1;
            if (hScalePixel > 1) {
                hScalePixel -= 2;
            }
            else {
                hScalePixel = hScalePixel * 2 / 3;
            }
            max = getMaxVisibleCount(hScalePixel, pureH);
            if (max >= dataCount) {
                if (hScalePixel < 1) {
                    hScalePixel = (double)pureH / max;
                }
                findex = 0;
                lindex = dataCount - 1;
            }
            else {
                findex = lindex - max + 1;
                if (findex < 0) {
                    findex = 0;
                }
            }
        }

        /// <summary>
        /// 放大
        /// </summary>
        /// <param name="pureH">横向宽度</param>
        /// <param name="dataCount">数据条数</param>
        /// <param name="findex">首先可见索引号</param>
        /// <param name="lindex">最后可见索引号</param>
        /// <param name="hScalePixel">数据间隔</param>
        private void zoomOut(int pureH, int dataCount, ref int findex, ref int lindex, ref double hScalePixel) {
            int oriMax = -1, max = -1, deal = 0;
            if (hScalePixel < 30) {
                oriMax = getMaxVisibleCount(hScalePixel, pureH);
                if (dataCount < oriMax) {
                    deal = 1;
                }
                if (hScalePixel >= 1) {
                    hScalePixel += 2;
                }
                else {
                    hScalePixel = hScalePixel * 1.5;
                    if (hScalePixel > 1) {
                        hScalePixel = 1;
                    }
                }
                max = getMaxVisibleCount(hScalePixel, pureH);
                if (dataCount >= max) {
                    if (deal == 1) {
                        lindex = dataCount - 1;
                    }
                    findex = lindex - max + 1;
                    if (findex < 0) {
                        findex = 0;
                    }
                }
            }
        }
    }
}
