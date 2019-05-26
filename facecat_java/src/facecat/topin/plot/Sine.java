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
 * 正弦线
 */
public class Sine extends FCPlot {

    /**
     * 创建正弦线
     */
    public Sine() {
        setPlotType("SINE");
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
        FCPoint mp = getTouchOverPoint();
        // 获取选中点
        float y1 = pY(m_marks.get(0).getValue());
        float y2 = pY(m_marks.get(1).getValue());
        int bIndex = m_dataSource.getRowIndex(m_marks.get(0).getKey());
        int eIndex = m_dataSource.getRowIndex(m_marks.get(1).getKey());
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        // 判断选中
        if (selectPoint(mp, x1, y1)) {
            action = ActionType.AT1;
            return action;
        } else if (selectPoint(mp, x2, y2)) {
            action = ActionType.AT2;
            return action;
        }
        if (selectSine(mp, x1, y1, x2, y2)) {
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
            FCChart chart = getChart();
            int touchIndex = chart.getTouchOverIndex();
            if (touchIndex >= 0 && touchIndex <= chart.getLastVisibleIndex()) {
                int eIndex = touchIndex;
                int bIndex = eIndex - chart.getMaxVisibleRecord() / 10;
                if (bIndex >= 0 && eIndex != bIndex) {
                    double fDate = m_dataSource.getXValue(bIndex);
                    double sDate = m_dataSource.getXValue(eIndex);
                    m_marks.clear();
                    double y = getNumberValue(mp);
                    m_marks.put(0, new PlotMark(0, fDate, y + (m_div.getVScale(m_attachVScale).getVisibleMax() - m_div.getVScale(m_attachVScale).getVisibleMin()) / 4));
                    m_marks.put(1, new PlotMark(1, sDate, y));
                    return true;
                }
            }
        }
        return false;
    }

    /**
     * 开始移动画线工具
     */
    @Override
    public void onMoveStart() {
        m_action = getAction();
        m_startMarks.clear();
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType.NO) {
            m_startMarks.put(0, m_marks.get(0));
            m_startMarks.put(1, m_marks.get(1));
        }
    }

    /**
     * 移动方法
     */
    @Override
    public void onMoving() {
        FCPoint mp = getMovingPoint();
        // 获取当前的索引和y值
        FCChart chart = getChart();
        int touchIndex = chart.getTouchOverIndex();
        if (touchIndex < 0) {
            touchIndex = 0;
        }
        if (touchIndex > chart.getLastVisibleIndex()) {
            touchIndex = chart.getLastVisibleIndex();
        }
        int bIndex = m_dataSource.getRowIndex(m_marks.get(0).getKey());
        int eIndex = m_dataSource.getRowIndex(m_marks.get(1).getKey());
        // 根据不同类型作出动作
        switch (m_action) {
            case MOVE:
                move(mp);
                break;
            case AT1:
                if (touchIndex < eIndex) {
                    resize(0);
                }
                break;
            case AT2:
                if (touchIndex > bIndex) {
                    resize(1);
                }
                break;
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
        double fValue = pList.get(0).getValue();
        double eValue = pList.get(1).getValue();
        int bIndex = m_dataSource.getRowIndex(pList.get(0).getKey());
        int eIndex = m_dataSource.getRowIndex(pList.get(1).getKey());
        int x1 = (int) pX(bIndex);
        float x2 = pX(eIndex);
        float y1 = pY(fValue);
        float y2 = pY(eValue);
        // 画线
        double f = 2.0 * Math.PI / ((x2 - x1) * 4);
        if (x1 != x2) {
            int len = getWorkingAreaWidth();
            if (len > 0) {
                FCPoint[] pf = new FCPoint[len];
                for (int i = 0; i < len; i++) {
                    int x = -x1 + i;
                    float y = (float) (0.5 * (y2 - y1) * Math.sin(x * f) * 2);
                    FCPoint pt = new FCPoint((int) (x + x1), (int) (y + y1));
                    pf[i] = pt;
                }
                drawPolyline(paint, lineColor, m_lineWidth, m_lineStyle, pf);
            }
        }
        // 画选中点
        if (isSelected()) {
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x2, y2);
        }
    }
}
