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
 * 拉弗回归通道
 */
public class RaffChannel extends FCPlot {

    /**
     * 创建拉弗回归通道
     */
    public RaffChannel() {
        setPlotType("RAFFCHANNEL");
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
            if (touchIndex >= chart.getFirstVisibleIndex() && touchIndex <= chart.getLastVisibleIndex()) {
                double yValue = a * ((touchIndex - bIndex) + 1) + b;
                float y = pY(yValue);
                float x = pX(touchIndex);
                if (mp.x >= x - 5 && mp.x <= x + 5 && mp.y >= y - 5 && mp.y <= y + 5) {
                    action = ActionType.MOVE;
                    return action;
                }
                double parallel = getRRCRange(m_marks, param);
                yValue = a * ((touchIndex - bIndex) + 1) + b + parallel;
                y = pY(yValue);
                x = pX(touchIndex);
                if (mp.x >= x - 5 && mp.x <= x + 5 && mp.y >= y - 5 && mp.y <= y + 5) {
                    action = ActionType.MOVE;
                    return action;
                }
                yValue = a * ((touchIndex - bIndex) + 1) + b - parallel;
                y = pY(yValue);
                x = pX(touchIndex);
                if (mp.x >= x - 5 && mp.x <= x + 5 && mp.y >= y - 5 && mp.y <= y + 5) {
                    action = ActionType.MOVE;
                    return action;
                }
            }
        }
        return action;
    }

    /**
     * 获取拉弗线性回归的高低点值
     *
     * @param pList 点阵集合
     * @param param 直线参数
     * @returns 拉弗线性回归的高低点值
     */
    private double getRRCRange(java.util.HashMap<Integer, PlotMark> pList, float[] param) {
        if (param == null || m_sourceFields == null || m_sourceFields.isEmpty() || !m_sourceFields.containsKey("HIGH") || !m_sourceFields.containsKey("LOW")) {
            return 0;
        }
        float a = param[0];
        float b = param[1];
        int bIndex = m_dataSource.getRowIndex(pList.get(0).getKey());
        int eIndex = m_dataSource.getRowIndex(pList.get(1).getKey());
        double upSubValue = 0;
        double downSubValue = 0;
        int pos = 0;
        for (int i = bIndex; i <= eIndex; i++) {
            double high = m_dataSource.get2(i, m_sourceFields.get("HIGH"));
            double low = m_dataSource.get2(i, m_sourceFields.get("LOW"));
            if (!Double.isNaN(high) && !Double.isNaN(low)) {
                double midValue = (i - bIndex + 1) * a + b;
                if (pos == 0) {
                    upSubValue = high - midValue;
                    downSubValue = midValue - low;
                } else {
                    if (high - midValue > upSubValue) {
                        upSubValue = high - midValue;
                    }
                    if (midValue - low > downSubValue) {
                        downSubValue = midValue - low;
                    }
                }
                pos++;
            }
        }
        if (upSubValue >= downSubValue) {
            return upSubValue;
        } else {
            return downSubValue;
        }
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
        // 获取直线参数
        float[] param = getLRParams(pList);
        if (param != null) {
            // 画线条和选中点
            float a = param[0];
            float b = param[1];
            int bIndex = m_dataSource.getRowIndex(pList.get(0).getKey());
            int eIndex = m_dataSource.getRowIndex(pList.get(1).getKey());
            float x1 = pX(bIndex);
            float x2 = pX(eIndex);
            double leftValue = a + b;
            double rightValue = (eIndex - bIndex + 1) * a + b;
            float y1 = pY(leftValue);
            float y2 = pY(rightValue);
            float[] param2 = getLineParams(new PlotMark(0, pList.get(0).getKey(), leftValue), new PlotMark(1, pList.get(1).getKey(), rightValue));
            if (param2 != null) {
                // 画回归线
                a = param2[0];
                b = param2[1];
                float leftX = 0;
                float leftY = leftX * a + b;
                float rightX = getWorkingAreaWidth();
                float rightY = rightX * a + b;
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, leftX, leftY, rightX, rightY);
                // 获取拉弗参数
                double parallel = getRRCRange(pList, param);
                // 画上线
                double leftTop = leftValue + parallel;
                double rightTop = rightValue + parallel;
                param2 = getLineParams(new PlotMark(0, pList.get(0).getKey(), leftTop), new PlotMark(1, pList.get(1).getKey(), rightTop));
                if (param2 != null) {
                    a = param2[0];
                    b = param2[1];
                    leftX = 0;
                    leftY = leftX * a + b;
                    rightX = getWorkingAreaWidth();
                    rightY = rightX * a + b;
                    drawLine(paint, lineColor, m_lineWidth, m_lineStyle, leftX, leftY, rightX, rightY);
                }
                //  画下线
                double leftBottom = leftValue - parallel;
                double rightBottom = rightValue - parallel;
                param2 = getLineParams(new PlotMark(0, pList.get(0).getKey(), leftBottom), new PlotMark(1, pList.get(1).getKey(), rightBottom));
                if (param2 != null) {
                    a = param2[0];
                    b = param2[1];
                    leftX = 0;
                    leftY = leftX * a + b;
                    rightX = getWorkingAreaWidth();
                    rightY = rightX * a + b;
                    drawLine(paint, lineColor, m_lineWidth, m_lineStyle, leftX, leftY, rightX, rightY);
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
