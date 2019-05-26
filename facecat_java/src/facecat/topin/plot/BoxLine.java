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
import facecat.topin.plot.*;

/**
 * 箱形线
 */
public class BoxLine extends RectLine {

    /**
     * 创建箱形线
     */
    public BoxLine() {
        setPlotType("BOXLINE");
    }

    /**
     * 获取动作
     *
     * @param mp 点的位置
     * @returns 动作类型
     */
    private ActionType getAction(FCPoint mp) {
        // 获取参数
        double[] param = getCandleRange(m_marks);
        double nHigh = param[0];
        double nLow = param[1];
        if (param != null) {
            ActionType m_action = selectRect(mp, new PlotMark(0, m_marks.get(0).getKey(), nHigh), new PlotMark(1, m_marks.get(1).getKey(), nLow));
            return m_action;
        }
        return ActionType.NO;
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
        action = getAction(mp);
        if (action == ActionType.AT4) {
            action = ActionType.AT2;
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
        return create2CandlePoints(mp);
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
        // 获取点的位置
        int bIndex = m_dataSource.getRowIndex(pList.get(0).getKey());
        int eIndex = m_dataSource.getRowIndex(pList.get(1).getKey());
        // 获取参数
        double[] param = getCandleRange(pList);
        double nHigh = param[0];
        double nLow = param[1];
        if (param != null) {
            FCRect rect = getRectangle(new PlotMark(0, pList.get(0).getKey(), nHigh), new PlotMark(1, pList.get(1).getKey(), nLow));
            int x1 = rect.left;
            int y1 = rect.top;
            int x2 = rect.right;
            int y2 = rect.bottom;
            // 画矩形
            if (rect.right - rect.left > 0 && rect.bottom - rect.top > 0) {
                int rwidth = rect.right - rect.left;
                int rheight = rect.bottom - rect.top;
                drawRect(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x1 + rwidth, y1 + rheight);
                // 画幅度文字
                int count = Math.abs(bIndex - eIndex) + 1;
                drawText(paint, "COUNT:" + (new Integer(count)).toString(), lineColor, m_font, x1 + 2, y1 + 2);
                double diff = nLow - nHigh;
                double range = 0;
                if (nHigh != 0) {
                    range = diff / nHigh;
                }
                String diffString = String.format("%.2f", diff);
                String rangeString = String.format("%.2f", range) + "%";
                FCSize rangeSize = textSize(paint, rangeString, m_font);
                drawText(paint, diffString, lineColor, m_font, x1 + rwidth + 2, y1 + 2);
                drawText(paint, rangeString, lineColor, m_font, x1 + rwidth + 2, y1 + rheight - rangeSize.cy - 2);
                // 画平均值
                if (m_sourceFields != null && m_sourceFields.containsKey("CLOSE")) {
                    double[] array = m_dataSource.DATA_ARRAY(m_sourceFields.get("CLOSE"), eIndex, eIndex - bIndex + 1);
                    double avg = FCScript.avgValue(array, array.length);
                    float yAvg = pY(avg);
                    drawLine(paint, lineColor, m_lineWidth, 1, x1, yAvg, x2, yAvg);
                    String avgString = "AVG:" + String.format("%.2f", avg);
                    FCSize avgSize = textSize(paint, avgString, m_font);
                    drawText(paint, avgString, lineColor, m_font, x1 + 2, yAvg - avgSize.cy - 2);
                }
            }
            // 画选中点
            if (isSelected()) {
                drawSelect(paint, lineColor, x1, y1);
                drawSelect(paint, lineColor, x2, y2);
            }
        }
    }
}
