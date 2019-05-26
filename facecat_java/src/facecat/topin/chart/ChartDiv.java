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
import java.util.*;
/*
* 图层，用来承载坐标轴，图形，画线工具，指标等内容。
*/
public class ChartDiv implements FCProperty {
    /*
    * 创建图层
    */
    public ChartDiv() {
        m_vGrid.setVisible(false);
    }

    /*
    * 析构函数
    */
    protected void finalize() throws Throwable {
        delete();
    }

    protected boolean m_allowUserPaint = false;

    /*
    * 获取是否允许用户绘图
    */
    public boolean allowUserPaint() {
        return m_allowUserPaint;
    }

    /*
    * 设置是否允许用户绘图
    */
    public void setAllowUserPaint(boolean allowUserPaint) {
        m_allowUserPaint = allowUserPaint;
    }

    protected ArrayList<FCPlot> m_plots = new ArrayList<FCPlot>();

    protected ArrayList<BaseShape> m_shapes = new ArrayList<BaseShape>();

    protected long m_backColor = FCColor.argb(0, 0, 0);
    
    /*
    * 获取背景色
    */
    public long getBackColor() {
        return m_backColor;
    }

    /*
    * 设置背景色
    */
    public void setBackColor(long value) {
        m_backColor = value;
    }

    protected long m_borderColor = FCColor.None;

    /*
    * 获取边线色
    */
    public long getBorderColor() {
        return m_borderColor;
    }

    /*
    * 设置边线色
    */
    public void setBorderColor(long value) {
        m_borderColor = value;
    }

    /*
    * 获取距离下侧的位置
    */
    public int getBottom() {
        return m_bounds.bottom;
    }

    protected FCRect m_bounds = new FCRect();

    /*
    * 获取层的区域
    */
    public FCRect getBounds() {
        return m_bounds.clone();
    }

    /*
    * 设置层的区域
    */
    public void setBounds(FCRect value) {
        m_bounds = value.clone();
    }

    protected FCChart m_chart = null;

    /*
    * 获取K线图
    */
    public FCChart getChart() {
        return m_chart;
    }

    /*
    * 设置K线图
    */
    public void setChart(FCChart value) {
        m_chart = value;
    }

    protected CrossLine m_crossLine = new CrossLine();

    /*
    * 获取十字线
    */
    public CrossLine getCrossLine() {
        return m_crossLine;
    }

    /*
    * 设置十字线
    */
    public void setCrossLine(CrossLine value) {
        if (m_crossLine != null) {
            m_crossLine.delete();
        }
        m_crossLine = value;
    }

    protected FCFont m_font = new FCFont();

    /*
    * 获取字体
    */
    public FCFont getFont() {
        return m_font;
    }

    /*
    * 设置字体
    */
    public void setFont(FCFont value) {
        m_font = value;
    }

    /*
    * 获取高度
    */
    public int getHeight() {
        return m_bounds.bottom - m_bounds.top;
    }

    protected ScaleGrid m_hGrid = new ScaleGrid();

    /*
    * 获取横向网格线
    */
    public ScaleGrid getHGrid() {
        return m_hGrid;
    }

    /*
    * 设置横向网格线
    */
    public void setHGrid(ScaleGrid value) {
        if (m_hGrid != null) {
            m_hGrid.delete();
        }
        m_hGrid = value;
    }

    protected HScale m_hScale = new HScale();

    /*
    * 获取横轴
    */
    public HScale getHScale() {
        return m_hScale;
    }

    /*
    * 设置横轴
    */
    public void setHScale(HScale value) {
        if (m_hScale != null) {
            m_hScale.delete();
        }
        m_hScale = value;
    }

    protected boolean m_isDeleted = false;

    /*
    * 是否已被删除
    */
    public boolean isDeleted() {
        return m_isDeleted;
    }

    /*
    * 获取距离左侧的距离
    */
    public int getLeft() {
        return m_bounds.left;
    }

    /*
    * 设置距离左侧的距离
    */
    public void setLeft(int value) {
        m_bounds.left = value;
    }

    protected VScale m_leftVScale = new VScale();

    /*
    * 获取左纵轴
    */
    public VScale getLeftVScale() {
        return m_leftVScale;
    }

    /*
    * 设置左纵轴
    */
    public void setLeftVScale(VScale value) {
        if (m_leftVScale != null) {
            m_leftVScale.delete();
        }
        m_leftVScale = value;
    }

    /*
    * 获取距离右侧的距离
    */
    public int getRight() {
        return m_bounds.right;
    }

    protected VScale m_rightVScale = new VScale();

    /*
    * 获取右纵轴
    */
    public VScale getRightVScale() {
        return m_rightVScale;
    }

    /*
    * 设置右纵轴
    */
    public void setRightVScale(VScale value) {
        if (m_rightVScale != null) {
            m_rightVScale.delete();
        }
        m_rightVScale = value;
    }

    protected SelectArea m_selectArea = new SelectArea();

    /*
    * 获取选中区域
    */
    public SelectArea getSelectArea() {
        return m_selectArea;
    }

    /*
    * 设置选中区域
    */
    public void setSelectArea(SelectArea value) {
        if (m_selectArea != null) {
            m_selectArea.delete();
        }
        m_selectArea = value;
    }

    protected boolean m_selected = false;

    /*
    * 获取是否被选中
    */
    public boolean isSelected() {
        return m_selected;
    }

    /*
    * 设置是否被选中
    */
    public void setSelected(boolean value) {
        m_selected = value;
    }

    protected boolean m_showSelect = true;

    /*
    * 获取是否要显示选中
    */
    public boolean showSelect() {
        return m_showSelect;
    }

    /*
    * 设置是否要显示选中
    */
    public void setShowSelect(boolean value) {
        m_showSelect = value;
    }

    protected ChartTitleBar m_titleBar = new ChartTitleBar();

    /*
    * 获取标题栏
    */
    public ChartTitleBar getTitleBar() {
        return m_titleBar;
    }

    /*
    * 设置标题栏
    */
    public void setTitleBar(ChartTitleBar value) {
        if (m_titleBar != null) {
            m_titleBar.delete();
        }
        m_titleBar = value;
    }

    protected ChartToolTip m_toolTip = new ChartToolTip();

    /*
    * 获取提示框
    */
    public ChartToolTip getToolTip() {
        return m_toolTip;
    }

    /*
    * 设置提示框
    */
    public void setToolTip(ChartToolTip value) {
        if (m_toolTip != null) {
            m_toolTip.delete();
        }
        m_toolTip = value;
    }

    /*
    * 获取距离上层的距离
    */
    public int getTop() {
        return m_bounds.top;
    }

    /*
    * 设置距离上侧的距离
    */
    public void setTop(int value) {
        m_bounds.top = value;
    }

    protected float m_verticalPercent = 0;

    /*
    * 获取垂直百分比
    */
    public float getVerticalPercent() {
        return m_verticalPercent;
    }

    /*
    * 设置垂直百分比
    */
    public void setVerticalPercent(float value) {
        m_verticalPercent = value;
    }

    protected ScaleGrid m_vGrid = new ScaleGrid();

    /*
    * 获取纵向网格线
    */
    public ScaleGrid getVGrid() {
        return m_vGrid;
    }

    /*
    * 设置纵向网格线
    */
    public void setVGrid(ScaleGrid value) {
        if (m_vGrid != null) {
            m_vGrid.delete();
        }
        m_vGrid = value;
    }

    /*
    * 获取宽度
    */
    public int getWidth() {
        return m_bounds.right - m_bounds.left;
    }

    protected int m_workingAreaHeight;

    /*
    * 获取工作区域的高度
    */
    public int getWorkingAreaHeight() {
        return m_workingAreaHeight;
    }

    /*
    * 设置工作区域的高度
    */
    public void setWorkingAreaHeight(int value) {
        if (value >= 0) {
            m_workingAreaHeight = value;
        }
    }

    /*
    * 添加画线
    */
    public void addPlot(FCPlot plot) {
        m_plots.add(plot);
    }

    /*
    * 添加图形
    */
    public void addShape(BaseShape shape) {
        m_shapes.add(shape);
    }

    /*
    * 是否包含图形
    */
    public boolean containsShape(BaseShape shape) {
        ArrayList<BaseShape> shapesCopy = getShapes(SortType.NONE);
        for (BaseShape bs : shapesCopy) {
            if (bs == shape) {
                return true;
            }
        }
        return false;
    }

    /*
    * 删除
    */
    public void delete() {
        if (!m_isDeleted) {
            try {
                if (m_shapes != null) {
                    for (BaseShape shape : m_shapes) {
                        shape.delete();
                    }
                    m_shapes.clear();
                }
                if (m_plots != null) {
                    for (FCPlot plot : m_plots) {
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
            } finally {
                m_isDeleted = true;
            }
        }
    }

    /*
    * 获取画线集合
    */
    public ArrayList<FCPlot> getPlots(SortType sortType) {
        ArrayList<FCPlot> plist = new ArrayList<FCPlot>();
        int plotsSize = m_plots.size();
        for (int i = 0; i < plotsSize; i++) {
            plist.add(m_plots.get(i));
        }
        if (sortType == SortType.ASC) {
            Collections.sort(plist, new PlotZOrderCompareASC());
        } else if (sortType == SortType.DESC) {
            Collections.sort(plist, new PlotZOrderCompareDESC());
        }
        return plist;
    }

    /*
    * 获取属性
    */
    public void getProperty(String name, RefObject<String> value, RefObject<String> type) {
        if (name.equals("allowUserPaint")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(allowUserPaint());
        } else if (name.equals("backcolor")) {
            type.argvalue = "color";
            value.argvalue = FCStr.convertColorToStr(getBackColor());
        } else if (name.equals("bordercolor")) {
            type.argvalue = "color";
            value.argvalue = FCStr.convertColorToStr(getBorderColor());
        } else if (name.equals("showselect")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(showSelect());
        }
    }

    /*
    * 获取属性列表
    */
    public ArrayList<String> getPropertyNames() {
        ArrayList<String> propertyNames = new ArrayList<String>();
        propertyNames.addAll(Arrays.asList(new String[]{"allowUserPaint", "BackColor", "BorderColor", "ShowSelect"}));
        return propertyNames;
    }

    /*
    * 重绘方法
    */
    public void onPaint(FCPaint paint, FCRect rect) {

    }

    /*
    * 设置属性
    */
    public void setProperty(String name, String value) {
        if (name.equals("allowUserPaint")) {
            setAllowUserPaint(FCStr.convertStrToBool(value));
        } else if (name.equals("backcolor")) {
            setBackColor(FCStr.convertStrToColor(value));
        } else if (name.equals("bordercolor")) {
            setBorderColor(FCStr.convertStrToColor(value));
        } else if (name.equals("showselect")) {
            setShowSelect(FCStr.convertStrToBool(value));
        }
    }

    /*
    * 移除画线
    */
    public void removePlot(FCPlot plot) {
        m_plots.remove(plot);
    }

    /*
    * 移除图形
    */
    public void removeShape(BaseShape shape) {
        if (m_shapes.contains(shape)) {
            m_shapes.remove(shape);
        }
    }

    public ArrayList<BaseShape> getShapes(SortType sortType) {
        ArrayList<BaseShape> slist = new ArrayList<BaseShape>();
        int shapesSize = m_shapes.size();
        for (int i = 0; i < shapesSize; i++) {
            slist.add(m_shapes.get(i));
        }
        if (sortType == SortType.ASC) {
            Collections.sort(slist, new BaseShapeZOrderASC());
        } else if (sortType == SortType.DESC) {
            Collections.sort(slist, new BaseShapeZOrderDESC());
        }
        return slist;
    }

    /*
    * 获取纵轴
    */
    public VScale getVScale(AttachVScale attachVScale) {
        if (attachVScale == AttachVScale.Left) {
            return m_leftVScale;
        } else {
            return m_rightVScale;
        }
    }
}
