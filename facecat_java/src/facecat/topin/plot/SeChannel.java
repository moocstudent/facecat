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
 * 标准误差通道
 */
public class SeChannel extends FCPlot {

    /**
     * 创建标准误差通道
     */
    public SeChannel() {
        setPlotType("SECHANNEL");
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
        // 获取点的位置
        int bIndex = m_dataSource.getRowIndex(m_marks.get(0).getKey());
        int eIndex = m_dataSource.getRowIndex(m_marks.get(1).getKey());
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        // 获取参数
        float[] param = getLRParams(m_marks);
        if (param != null) {
            float a = param[0];
            float b = param[1];
            double leftValue = a + b;
            double rightValue = (eIndex - bIndex + 1) * a + b;
            float y1 = pY(leftValue);
            float y2 = pY(rightValue);
            if (selectPoint(mp, x1, y1)) {
                action = ActionType.AT1;
                return action;
            } else if (selectPoint(mp, x2, y2)) {
                action = ActionType.AT2;
                return action;
            }
            FCChart chart = getChart();
            int touchIndex = chart.getTouchOverIndex();
            if (touchIndex >= bIndex && touchIndex <= chart.getLastVisibleIndex()) {
                //  判断选中
                double yValue = a * ((touchIndex - bIndex) + 1) + b;
                float y = pY(yValue);
                float x = pX(touchIndex);
                if (selectPoint(mp, x, y)) {
                    action = ActionType.MOVE;
                    return action;
                }
                double sd = getSEChannelSD(m_marks);
                yValue = a * ((touchIndex - bIndex) + 1) + b + sd;
                y = pY(yValue);
                x = pX(touchIndex);
                if (selectPoint(mp, x, y)) {
                    action = ActionType.MOVE;
                    return action;
                }
                yValue = a * ((touchIndex - bIndex) + 1) + b - sd;
                y = pY(yValue);
                x = pX(touchIndex);
                if (selectPoint(mp, x, y)) {
                    action = ActionType.MOVE;
                    return action;
                }
            }
        }
        return action;
    }

    /**
     * 获取标准误差通道的标准差值
     *
     * @param pList 点阵集合
     * @returns 标准误差通道的参数
     */
    private double getSEChannelSD(java.util.HashMap<Integer, PlotMark> pList) {
        if (m_sourceFields != null && m_sourceFields.containsKey("CLOSE")) {
            int bIndex = m_dataSource.getRowIndex(pList.get(0).getKey());
            int eIndex = m_dataSource.getRowIndex(pList.get(1).getKey());
            int len = eIndex - bIndex + 1;
            if (len > 0) {
                double[] ary = new double[len];
                for (int i = 0; i < len; i++) {
                    double close = m_dataSource.get2(i + bIndex, m_sourceFields.get("CLOSE"));
                    if (!Double.isNaN(close)) {
                        ary[i] = close;
                    }
                }
                double avg = FCScript.avgValue(ary, len);
                // 求标准差
                double sd = FCScript.standardDeviation(ary, len, avg, 2);
                return sd;
            }
        }
        return 0;
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
        FCChart chart = getChart();
        // 获取相对位置
        int bIndex = m_dataSource.getRowIndex(pList.get(0).getKey());
        int eIndex = m_dataSource.getRowIndex(pList.get(1).getKey());
        float x1 = pX(chart.getX(bIndex));
        float x2 = pX(chart.getX(eIndex));
        // 获取直线参数
        float[] param = getLRParams(pList);
        if (param != null) {
            // 画线条和选中点
            float a = param[0];
            float b = param[1];
            double leftValue = a + b;
            double rightValue = (eIndex - bIndex + 1) * a + b;
            float y1 = pY(leftValue);
            float y2 = pY(rightValue);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
            double sd = getSEChannelSD(pList);
            double leftTop = leftValue + sd;
            double rightTop = rightValue + sd;
            double leftBottom = leftValue - sd;
            double rightBottom = rightValue - sd;
            float leftTopY = pY(leftTop);
            float rightTopY = pY(rightTop);
            float leftBottomY = pY(leftBottom);
            float rightBottomY = pY(rightBottom);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, leftTopY, x2, rightTopY);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, leftBottomY, x2, rightBottomY);
            rightValue = (chart.getLastVisibleIndex() + 1 - bIndex) * a + b;
            float x3 = (float) ((chart.getLastVisibleIndex() - chart.getFirstVisibleIndex()) * chart.getHScalePixel() + chart.getHScalePixel() / 2);
            double dashTop = rightValue + sd;
            double dashBottom = rightValue - sd;
            float mValueY = pY(rightValue);
            float dashTopY = pY(dashTop);
            float dashBottomY = pY(dashBottom);
            drawLine(paint, lineColor, m_lineWidth, 1, x2, rightTopY, x3, dashTopY);
            drawLine(paint, lineColor, m_lineWidth, 1, x2, rightBottomY, x3, dashBottomY);
            drawLine(paint, lineColor, m_lineWidth, 1, x2, y2, x3, mValueY);
            // 画选中点
            if (isSelected()) {
                drawSelect(paint, lineColor, x1, y1);
                drawSelect(paint, lineColor, x2, y2);
            }
        }
    }
}
