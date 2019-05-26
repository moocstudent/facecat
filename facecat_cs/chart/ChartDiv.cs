/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

using System;
using System.Collections.Generic;

namespace FaceCat {
    /// <summary>
    /// 图层，用来承载坐标轴，图形，画线工具，指标等内容。
    /// </summary>
    [Serializable()]
    public class ChartDiv : FCProperty {
        /// <summary>
        /// 创建图层
        /// </summary>
        public ChartDiv() {
            //隐藏纵向网格
            m_vGrid.Visible = false;
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~ChartDiv() {
            delete();
        }

        protected bool m_allowUserPaint;

        /// <summary>
        /// 获取或设置是否允许用户绘图
        /// </summary>
        public virtual bool AllowUserPaint {
            get { return m_allowUserPaint; }
            set { m_allowUserPaint = value; }
        }

        /// <summary>
        /// 画线工具
        /// </summary>
        protected ArrayList<FCPlot> m_plots = new ArrayList<FCPlot>();

        /// <summary>
        /// 图形
        /// </summary>
        protected ArrayList<BaseShape> m_shapes = new ArrayList<BaseShape>();

        private long m_backColor = FCColor.argb(0, 0, 0);

        /// <summary>
        /// 获取或设置背景色
        /// </summary>
        public virtual long BackColor {
            get { return m_backColor; }
            set { m_backColor = value; }
        }

        protected long m_borderColor = FCColor.None;

        /// <summary>
        /// 获取或设置边线的颜色
        /// </summary>
        public virtual long BorderColor {
            get { return m_borderColor; }
            set { m_borderColor = value; }
        }

        /// <summary>
        /// 获取距离下侧的位置
        /// </summary>
        public virtual int Bottom {
            get { return m_bounds.bottom; }
        }

        protected FCRect m_bounds = new FCRect();

        /// <summary>
        /// 获取或设置层的区域
        /// </summary>
        public virtual FCRect Bounds {
            get { return m_bounds; }
            set { m_bounds = value; }
        }

        protected FCChart m_chart;

        /// <summary>
        /// 获取或设置所在布局
        /// </summary>
        public virtual FCChart Chart {
            get { return m_chart; }
            set { m_chart = value; }
        }

        protected CrossLine m_crossLine = new CrossLine();

        /// <summary>
        /// 获取或设置十字线
        /// </summary>
        public virtual CrossLine CrossLine {
            get { return m_crossLine; }
            set {
                if (m_crossLine != null) {
                    m_crossLine.delete();
                }
                m_crossLine = value;
            }
        }

        protected FCFont m_font = new FCFont();

        /// <summary>
        /// 获取或设置层的字体
        /// </summary>
        public virtual FCFont Font {
            get { return m_font; }
            set { m_font = value; }
        }

        /// <summary>
        /// 获取高度
        /// </summary>
        public virtual int Height {
            get { return m_bounds.bottom - m_bounds.top; }
        }

        protected ScaleGrid m_hGrid = new ScaleGrid();

        /// <summary>
        /// 获取或设置横向网格线
        /// </summary>
        public virtual ScaleGrid HGrid {
            get { return m_hGrid; }
            set {
                if (m_hGrid != null) {
                    m_hGrid.delete();
                }
                m_hGrid = value;
            }
        }

        protected HScale m_hScale = new HScale();

        /// <summary>
        /// 获取或设置横轴
        /// </summary>
        public virtual HScale HScale {
            get { return m_hScale; }
            set {
                if (m_hScale != null) {
                    m_hScale.delete();
                }
                m_hScale = value;
            }
        }

        protected bool m_isDeleted;

        /// <summary>
        /// 获取或设置是否已被销毁
        /// </summary>
        public virtual bool IsDeleted {
            get { return m_isDeleted; }
        }

        /// <summary>
        /// 获取或设置距离左侧的位置
        /// </summary>
        public virtual int Left {
            get { return m_bounds.left; }
            set { m_bounds.left = value; }
        }

        protected VScale m_leftVScale = new VScale();

        /// <summary>
        /// 获取或设置左纵轴
        /// </summary>
        public virtual VScale LeftVScale {
            get { return m_leftVScale; }
            set {
                if (m_leftVScale != null) {
                    m_leftVScale.delete();
                }
                m_leftVScale = value;
            }
        }

        /// <summary>
        /// 获取距离右侧的距离
        /// </summary>
        public virtual int Right {
            get { return m_bounds.right; }
        }

        protected VScale m_rightVScale = new VScale();

        /// <summary>
        /// 获取或设置右纵轴
        /// </summary>
        public virtual VScale RightVScale {
            get { return m_rightVScale; }
            set {
                if (m_rightVScale != null) {
                    m_rightVScale.delete();
                }
                m_rightVScale = value;
            }
        }

        protected SelectArea m_selectArea = new SelectArea();

        /// <summary>
        /// 获取或设置选中框
        /// </summary>
        public virtual SelectArea SelectArea {
            get { return m_selectArea; }
            set {
                if (m_selectArea != null) {
                    m_selectArea.delete();
                }
                m_selectArea = value;
            }
        }

        protected bool m_selected;

        /// <summary>
        /// 获取或设置是否被选中
        /// </summary>
        public virtual bool Selected {
            get { return m_selected; }
            set { m_selected = value; }
        }

        protected bool m_showSelect = true;

        /// <summary>
        /// 获取或设置是否可以选中
        /// </summary>
        public virtual bool ShowSelect {
            get { return m_showSelect; }
            set { m_showSelect = value; }
        }

        protected ChartTitleBar m_titleBar = new ChartTitleBar();

        /// <summary>
        /// 获取或设置标题
        /// </summary>
        public virtual ChartTitleBar TitleBar {
            get { return m_titleBar; }
            set {
                if (m_titleBar != null) {
                    m_titleBar.delete();
                }
                m_titleBar = value;
            }
        }

        protected ChartToolTip m_toolTip = new ChartToolTip();

        /// <summary>
        /// 获取或设置提示框
        /// </summary>
        public virtual ChartToolTip ToolTip {
            get { return m_toolTip; }
            set {
                if (m_toolTip != null) {
                    m_toolTip.delete();
                }
                m_toolTip = value;
            }
        }

        /// <summary>
        /// 获取或设置距离上侧的位置
        /// </summary>
        public virtual int Top {
            get { return m_bounds.top; }
            set { m_bounds.top = value; }
        }

        protected float m_verticalPercent = 0;

        /// <summary>
        /// 获取或设置纵向所占比例
        /// </summary>
        public virtual float VerticalPercent {
            get { return m_verticalPercent; }
            set { m_verticalPercent = value; }
        }

        protected ScaleGrid m_vGrid = new ScaleGrid();

        /// <summary>
        /// 获取或设置纵向网格线
        /// </summary>
        public virtual ScaleGrid VGrid {
            get { return m_vGrid; }
            set {
                if (m_vGrid != null) {
                    m_vGrid.delete();
                }
                m_vGrid = value;
            }
        }

        /// <summary>
        /// 获取宽度
        /// </summary>
        public virtual int Width {
            get { return m_bounds.right - m_bounds.left; }
        }

        protected int m_workingAreaHeight;

        /// <summary>
        /// 获取或设置工作区域的高度
        /// </summary>
        public virtual int WorkingAreaHeight {
            get { return m_workingAreaHeight; }
            set {
                if (value >= 0) {
                    m_workingAreaHeight = value;
                }
            }
        }

        /// <summary>
        /// 添加画线工具
        /// </summary>
        /// <param name="plot">画线工具</param>
        public void addPlot(FCPlot plot) {
            m_plots.add(plot);
        }

        /// <summary>
        /// 添加图形
        /// </summary>
        /// <param name="shape">图形</param>
        public void addShape(BaseShape shape) {
            m_shapes.add(shape);
        }

        /// <summary>
        /// 是否包含图形
        /// </summary>
        /// <param name="shape">图形</param>
        /// <returns>是否包含</returns>
        public bool containsShape(BaseShape shape) {
            ArrayList<BaseShape> shapesCopy = getShapes(SortType.NONE);
            foreach (BaseShape bs in shapesCopy) {
                if (bs == shape) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 销毁资源
        /// </summary>
        public void delete() {
            if (!m_isDeleted) {
                try {
                    if (m_shapes != null) {
                        foreach (BaseShape shape in m_shapes) {
                            shape.delete();
                        }
                        m_shapes.clear();
                    }
                    if (m_plots != null) {
                        foreach (FCPlot plot in m_plots) {
                            plot.delete();
                        }
                        m_plots.clear();
                    }
                    if (m_leftVScale != null) {
                        m_leftVScale.delete();
                    }
                    if (m_rightVScale != null) {
                        m_rightVScale.delete();
                    }
                    if (m_hScale != null) {
                        m_hScale.delete();
                    }
                    if (m_hGrid != null) {
                        m_hGrid.delete();
                    }
                    if (m_vGrid != null) {
                        m_vGrid.delete();
                    }
                    if (m_crossLine != null) {
                        m_crossLine.delete();
                    }
                    if (m_titleBar != null) {
                        m_titleBar.delete();
                    }
                    if (m_selectArea != null) {
                        m_selectArea.delete();
                    }
                    if (m_toolTip != null) {
                        m_toolTip.delete();
                    }
                }
                finally {
                    m_isDeleted = true;
                }
            }
        }

        /// <summary>
        /// 获取所有的画线工具
        /// </summary>
        /// <returns>所有画线工具</returns>
        public ArrayList<FCPlot> getPlots(SortType sortType) {
            ArrayList<FCPlot> plist = new ArrayList<FCPlot>();
            plist.AddRange(m_plots.ToArray());
            if (sortType == SortType.ASC) {
                plist.Sort(new PlotZOrderCompareASC());
            }
            else if (sortType == SortType.DESC) {
                plist.Sort(new PlotZOrderCompareDESC());
            }
            return plist;
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">返回属性值</param>
        /// <param name="type">返回属性类型</param>
        public virtual void getProperty(String name, ref String value, ref String type) {
            if (name == "allowuserpaint") {
                type = "bool";
                value = FCStr.convertBoolToStr(AllowUserPaint);
            }
            else if (name == "backcolor") {
                type = "color";
                value = FCStr.convertColorToStr(BackColor);
            }
            else if (name == "bordercolor") {
                type = "color";
                value = FCStr.convertColorToStr(BorderColor);
            }
            else if (name == "showselect") {
                type = "bool";
                value = FCStr.convertBoolToStr(ShowSelect);
            }
        }

        /// <summary>
        /// 获取属性名称列表
        /// </summary>
        /// <returns></returns>
        public virtual ArrayList<String> getPropertyNames() {
            ArrayList<String> propertyNames = new ArrayList<String>();
            propertyNames.AddRange(new String[] { "AllowUserPaint", "BackColor", "BorderColor", "ShowSelect" });
            return propertyNames;
        }

        /// <summary>
        /// 获取所有的图形
        /// </summary>
        /// <param name="sortType">排序类型</param>
        /// <returns>图形集合</returns>
        public ArrayList<BaseShape> getShapes(SortType sortType) {
            ArrayList<BaseShape> slist = new ArrayList<BaseShape>();
            slist.AddRange(m_shapes.ToArray());
            if (sortType == SortType.ASC) {
                slist.Sort(new BaseShapeZOrderASC());
            }
            else if (sortType == SortType.DESC) {
                slist.Sort(new BaseShapeZOrderDESC());
            }
            return slist;
        }

        /// <summary>
        /// 获取纵轴
        /// </summary>
        /// <param name="attachVScale">纵轴类型</param>
        /// <returns>坐标轴</returns>
        public VScale getVScale(AttachVScale attachVScale) {
            if (attachVScale == AttachVScale.Left) {
                return m_leftVScale;
            }
            else {
                return m_rightVScale;
            }
        }

        /// <summary>
        /// 重绘方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="rect">区域</param>
        public virtual void onPaint(FCPaint paint, FCRect rect) {

        }

        /// <summary>
        /// 移除画线工具
        /// </summary>
        /// <param name="plot">画线工具</param>
        public void removePlot(FCPlot plot) {
            m_plots.remove(plot);
        }

        /// <summary>
        /// 移除图形
        /// </summary>
        /// <param name="shape">图形</param>
        public void removeShape(BaseShape shape) {
            if (m_shapes.Contains(shape)) {
                m_shapes.remove(shape);
            }
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        public virtual void setProperty(String name, String value) {
            if (name == "allowuserpaint") {
                AllowUserPaint = FCStr.convertStrToBool(value);
            }
            else if (name == "backcolor") {
                BackColor = FCStr.convertStrToColor(value);
            }
            else if (name == "bordercolor") {
                BorderColor = FCStr.convertStrToColor(value);
            }
            else if (name == "showselect") {
                ShowSelect = FCStr.convertStrToBool(value);
            }
        }
    }

    /// <summary>
    /// 比较线条层次的类(升序)
    /// </summary>
    public class BaseShapeZOrderASC : IComparer<BaseShape> {
        /// <summary>
        /// 比较方法
        /// </summary>
        /// <param name="x">线条X</param>
        /// <param name="y">线条Y</param>
        /// <returns>是否相等</returns>
        public int Compare(BaseShape x, BaseShape y) {
            return x.ZOrder.CompareTo(y.ZOrder);
        }
    }

    /// <summary>
    /// 比较线条层次的类(降序)
    /// </summary>
    public class BaseShapeZOrderDESC : IComparer<BaseShape> {
        /// <summary>
        /// 比较方法
        /// </summary>
        /// <param name="x">线条X</param>
        /// <param name="y">线条Y</param>
        /// <returns>是否相等</returns>
        public int Compare(BaseShape x, BaseShape y) {
            return y.ZOrder.CompareTo(x.ZOrder);
        }
    }

    /// <summary>
    /// 比较线条层次的类
    /// </summary>
    public class PlotZOrderCompareASC : IComparer<FCPlot> {
        /// <summary>
        /// 比较方法
        /// </summary>
        /// <param name="x">画线工具X</param>
        /// <param name="y">画线工具Y</param>
        /// <returns>是否相等</returns>
        public int Compare(FCPlot x, FCPlot y) {
            return x.ZOrder.CompareTo(y.ZOrder);
        }
    }

    /// <summary>
    /// 比较线条层次的类
    /// </summary>
    public class PlotZOrderCompareDESC : IComparer<FCPlot> {
        /// <summary>
        /// 比较方法
        /// </summary>
        /// <param name="x">画线工具X</param>
        /// <param name="y">画线工具Y</param>
        /// <returns>是否相等</returns>
        public int Compare(FCPlot x, FCPlot y) {
            return y.ZOrder.CompareTo(x.ZOrder);
        }
    }
}
