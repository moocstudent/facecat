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
 * 四等分线
 */
public class QuadrantLines extends Percent {

    /**
     * 创建四等分线
     */
    public QuadrantLines() {
        setPlotType("QUADRANTLINES");
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
            if (touchIndex >= bIndex && touchIndex <= eIndex) {
                // 回归线判断
                double yValue = a * ((touchIndex - bIndex) + 1) + b;
                float y = pY(yValue);
                float x = pX(touchIndex);
                if (mp.x >= x - 5 && mp.x <= x + 5 && mp.y >= y - 5 && mp.y <= y + 5) {
                    action = ActionType.MOVE;
                    return action;
                }
                // 等分线判断
                double[] candleRegion = getCandleRange(m_marks);
                if (candleRegion != null) {
                    float[] percents = getPercentParams(candleRegion[0], candleRegion[1]);
                    for (int i = 0; i < percents.length; i++) {
                        if (selectRay(mp, x1, percents[i], x2, percents[i])) {
                            action = ActionType.MOVE;
                            return action;
                        }
                    }
                }
            }
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
        // 获取直线参数
        float[] param = getLRParams(pList);
        if (param != null) {
            int bIndex = m_dataSource.getRowIndex(pList.get(0).getKey());
            int eIndex = m_dataSource.getRowIndex(pList.get(1).getKey());
            float x1 = pX(bIndex);
            float x2 = pX(eIndex);
            // 画线条和选中点
            float a = param[0];
            float b = param[1];
            double leftValue = a + b;
            double rightValue = (eIndex - bIndex + 1) * a + b;
            float y1 = pY(leftValue);
            float y2 = pY(rightValue);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
            // 等分线判断
            double[] candleRegion = getCandleRange(pList);
            if (candleRegion != null) {
                float[] percents = getPercentParams(candleRegion[0], candleRegion[1]);
                for (int i = 0; i < percents.length; i++) {
                    float yp = percents[i];
                    if (i == 2) {
                        drawLine(paint, lineColor, m_lineWidth, 1, x1, yp, x2, yp);
                    } else {
                        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, yp, x2, yp);
                    }
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
