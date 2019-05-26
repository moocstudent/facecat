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
 * 时间尺
 */
public class TimeRuler extends FCPlot {

    /**
     * 创建时间尺
     */
    public TimeRuler() {
        setPlotType("TIMERULER");
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
        // 获取点位置
        FCPoint mp = getTouchOverPoint();
        float y1 = pY(m_marks.get(0).getValue());
        float y2 = pY(m_marks.get(1).getValue());
        int bIndex = m_dataSource.getRowIndex(m_marks.get(0).getKey());
        int eIndex = m_dataSource.getRowIndex(m_marks.get(1).getKey());
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        if (selectPoint(mp, x1, y1)) {
            action = ActionType.AT1;
            return action;
        } else if (selectPoint(mp, x2, y2)) {
            action = ActionType.AT2;
            return action;
        }
        // 获取参数
        double[] param = getTimeRulerParams(m_marks);
        float yBHigh = pY(param[0]);
        float yBLow = pY(param[1]);
        float yEHigh = pY(param[2]);
        float yELow = pY(param[3]);
        // 判断选中
        if (y1 < yBHigh) {
            yBHigh = y1;
        }
        if (y1 > yBLow) {
            yBLow = y1;
        }
        if (y2 < yEHigh) {
            yEHigh = y2;
        }
        if (y2 > yELow) {
            yELow = y2;
        }
        if (mp.x >= x1 - m_lineWidth * 2.5f && mp.x <= x1 + m_lineWidth * 2.5f) {
            if (mp.y >= yBHigh - 2 && mp.y <= yBLow + 2) {
                action = ActionType.MOVE;
                return action;
            }
        }
        if (mp.x >= x2 - m_lineWidth * 2.5f && mp.x <= x2 + m_lineWidth * 2.5f) {
            if (mp.y >= yEHigh - m_lineWidth * 2.5f && mp.y <= yELow + m_lineWidth * 2.5f) {
                action = ActionType.MOVE;
                return action;
            }
        }
        if (mp.y >= y1 - m_lineWidth * 2.5f && mp.y <= y1 + m_lineWidth * 2.5f) {
            float bigX = x1 >= x2 ? x1 : x2;
            float smallX = x1 < x2 ? x1 : x2;
            if (mp.x >= smallX - m_lineWidth * 2.5f && mp.x <= bigX + m_lineWidth * 2.5f) {
                action = ActionType.MOVE;
                return action;
            }
        }
        return action;
    }

    /**
     * 获取时间尺的参数
     *
     * @param pList 点阵集合
     * @returns 时间尺的参数
     */
    private double[] getTimeRulerParams(java.util.HashMap<Integer, PlotMark> pList) {
        if (pList.isEmpty()) {
            return null;
        }
        FCChart chart = getChart();
        int bIndex = m_dataSource.getRowIndex(pList.get(0).getKey());
        int eIndex = m_dataSource.getRowIndex(pList.get(1).getKey());
        double bHigh = chart.divMaxOrMin(bIndex, m_div, 0);
        double bLow = chart.divMaxOrMin(bIndex, m_div, 1);
        double eHigh = chart.divMaxOrMin(eIndex, m_div, 0);
        double eLow = chart.divMaxOrMin(eIndex, m_div, 1);
        return new double[]{bHigh, bLow, eHigh, eLow};
    }

    /**
     * 初始化线条
     *
     * @param mp 坐标
     * @returns 是否初始化成功
     */
    @Override
    public boolean onCreate(FCPoint mp) {
        return create2PointsB(mp);
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
     * 移动线条
     */
    @Override
    public void onMoving() {
        FCPoint mp = getMovingPoint();
        // 获取当前的索引和y值
        FCChart chart = getChart();
        int touchIndex = chart.getTouchOverIndex();
        double y = getNumberValue(mp);
        if (touchIndex < 0) {
            touchIndex = 0;
        }
        if (touchIndex > chart.getLastVisibleIndex()) {
            touchIndex = chart.getLastVisibleIndex();
        }
        // 根据不同类型作出动作
        switch (m_action) {
            case MOVE:
                move(mp);
                break;
            case AT1:
                m_marks.put(0, new PlotMark(0, m_dataSource.getXValue(touchIndex), y));
                m_marks.put(1, new PlotMark(1, m_marks.get(1).getKey(), y));
                break;
            case AT2:
                m_marks.put(1, new PlotMark(1, m_dataSource.getXValue(touchIndex), y));
                m_marks.put(0, new PlotMark(0, m_marks.get(0).getKey(), y));
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
        float y1 = pY(pList.get(0).getValue());
        float y2 = pY(pList.get(1).getValue());
        int bIndex = m_dataSource.getRowIndex(pList.get(0).getKey());
        int eIndex = m_dataSource.getRowIndex(pList.get(1).getKey());
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        // 获取参数
        double[] param = getTimeRulerParams(pList);
        float yBHigh = pY(param[0]);
        float yBLow = pY(param[1]);
        float yEHigh = pY(param[2]);
        float yELow = pY(param[3]);
        if (y1 < yBHigh) {
            yBHigh = y1;
        }
        if (y1 > yBLow) {
            yBLow = y1;
        }
        if (y2 < yEHigh) {
            yEHigh = y2;
        }
        if (y2 > yELow) {
            yELow = y2;
        }
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, yBHigh, x1, yBLow);
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x2, yEHigh, x2, yELow);
        int subRecord = Math.abs(eIndex - bIndex) + 1;
        // 画文字
        String recordStr = (new Integer(subRecord)).toString() + "(T)";
        FCSize sizeF = textSize(paint, recordStr, m_font);
        drawText(paint, recordStr, lineColor, m_font, (x2 + x1) / 2 - sizeF.cx / 2, y1 - sizeF.cy / 2);
        if (Math.abs(x1 - x2) > sizeF.cx) {
            if (x2 >= x1) {
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, (x2 + x1) / 2 - sizeF.cx / 2, y1);
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, (x2 + x1) / 2 + sizeF.cx / 2, y1, x2, y1);
            } else {
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x2, y1, (x2 + x1) / 2 - sizeF.cx / 2, y1);
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, (x2 + x1) / 2 + sizeF.cx / 2, y1, x1, y1);
            }
        }
        // 画选中点
        if (isSelected()) {
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x2, y2);
        }
    }
}
