/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.plot;

import facecat.topin.core.*;
import facecat.topin.chart.*;

/**
 * 三角形
 */
public class Triangle extends FCPlot {

    /**
     * 创建三角形
     */
    public Triangle() {
        setPlotType("TRIANGLE");
    }

    /**
     * 获取动作类型
     */
    @Override
    public ActionType getAction() {
        ActionType action = ActionType.NO;
        if (m_marks.isEmpty()) {
            return action;
        }
        // 获取选中位置
        float y1 = pY(m_marks.get(0).getValue());
        float y2 = pY(m_marks.get(1).getValue());
        float y3 = pY(m_marks.get(2).getValue());
        int aIndex = m_dataSource.getRowIndex(m_marks.get(0).getKey());
        int bIndex = m_dataSource.getRowIndex(m_marks.get(1).getKey());
        int cIndex = m_dataSource.getRowIndex(m_marks.get(2).getKey());
        float x1 = pX(aIndex);
        float x2 = pX(bIndex);
        float x3 = pX(cIndex);
        FCPoint mp = getTouchOverPoint();
        if (m_moveTimes == 1) {
            action = ActionType.AT3;
            return action;
        } else {
            if (selectPoint(mp, x1, y1) || m_moveTimes == 1) {
                action = ActionType.AT1;
                return action;
            } else if (selectPoint(mp, x2, y2)) {
                action = ActionType.AT2;
                return action;
            } else if (selectPoint(mp, x3, y3)) {
                action = ActionType.AT3;
                return action;
            }
        }
        if (selectTriangle(mp, x1, y1, x2, y2, x3, y3)) {
            action = ActionType.MOVE;
        }
        return action;
    }

    /**
     * 初始化线条
     *
     * @param mp 坐标
     * @returns 是否初始化成功
     */
    @Override
    public boolean onCreate(FCPoint mp) {
        int rIndex = m_dataSource.getRowsCount();
        if (rIndex > 0) {
            int currentIndex = getIndex(mp);
            double y = getNumberValue(mp);
            double date = m_dataSource.getXValue(currentIndex);
            m_marks.clear();
            m_marks.put(0, new PlotMark(0, date, y));
            int si = currentIndex + 10;
            FCChart chart = getChart();
            if (si > chart.getLastVisibleIndex()) {
                si = chart.getLastVisibleIndex();
            }
            m_marks.put(1, new PlotMark(1, m_dataSource.getXValue(si), y));
            m_marks.put(2, new PlotMark(2, date, y));
            return true;
        }
        return false;
    }

    /**
     * 开始移动画线工具
     */
    @Override
    public void onMoveStart() {
        m_moveTimes++;
        m_action = getAction();
        m_startMarks.clear();
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType.NO) {
            m_startMarks.put(0, m_marks.get(0));
            m_startMarks.put(1, m_marks.get(1));
            m_startMarks.put(2, m_marks.get(2));
        }
    }

    /**
     * 绘制图像的残影
     *
     * @param paint 绘图对象
     */
    @Override
    public void onPaintGhost(FCPaint paint) {
        if (m_moveTimes > 1) {
            paint(paint, m_startMarks, getSelectedColor());
        }
    }

    /**
     * 绘制图像的方法
     *
     * @param paint 绘图对象
     * @param pList 横纵值描述
     * @param lineColor 颜色
     */
    @Override
    protected void paint(FCPaint paint, java.util.HashMap<Integer, PlotMark> pList, long lineColor) {
        if (pList.isEmpty()) {
            return;
        }
        // 获取相对位置
        float y1 = pY(pList.get(0).getValue());
        float y2 = pY(pList.get(1).getValue());
        float y3 = pY(pList.get(2).getValue());
        int aIndex = m_dataSource.getRowIndex(pList.get(0).getKey());
        int bIndex = m_dataSource.getRowIndex(pList.get(1).getKey());
        int cIndex = m_dataSource.getRowIndex(pList.get(2).getKey());
        float x1 = pX(aIndex);
        float x2 = pX(bIndex);
        float x3 = pX(cIndex);
        // 画线
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x3, y3);
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x2, y2, x3, y3);
        // 画选中点
        if (isSelected() || (x1 == x2 && x2 == x3 && y1 == y2 && y2 == y3)) {
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x2, y2);
            drawSelect(paint, lineColor, x3, y3);
        }
    }
}
