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
 * 高低推档
 */
public class LevelGrading extends FCPlot {

    /**
     * 创建高低推档
     */
    public LevelGrading() {
        setPlotType("LEVELGRADING");
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
        // 获取点阵的位置
        float y1 = pY(m_marks.get(0).getValue());
        float y2 = pY(m_marks.get(1).getValue());
        int bIndex = m_dataSource.getRowIndex(m_marks.get(0).getKey());
        float x1 = pX(bIndex);
        if (m_moveTimes == 1) {
            action = ActionType.AT1;
            return action;
        }
        if (mp.x >= x1 - m_lineWidth * 2.5f && mp.x <= x1 + m_lineWidth * 2.5f) {
            if (mp.y >= y1 - m_lineWidth * 2.5f && mp.y <= y1 + m_lineWidth * 2.5f) {
                action = ActionType.AT1;
                return action;
            } else if (mp.y >= y2 - m_lineWidth * 2.5f && mp.y <= y2 + m_lineWidth * 2.5f) {
                action = ActionType.AT2;
                return action;
            }
        }
        if (hLinesSelect(levelGradingParams(m_marks.get(0).getValue(), m_marks.get(1).getValue()), 5)) {
            action = ActionType.MOVE;
        }
        return action;
    }

    /**
     * 获取高低推档的直线参数
     *
     * @param value1 值1
     * @param value2 值2
     * @returns 高低推档的直线参数
     */
    private float[] levelGradingParams(double value1, double value2) {
        float y1 = pY(value1);
        float y2 = pY(value2);
        float yA = 0, yB = 0, yC = 0, yD = 0, yE = 0;
        yA = y1;
        yB = y2;
        yC = y1 <= y2 ? y2 + (y2 - y1) * 0.382f : y2 - (y1 - y2) * 0.382f;
        yD = y1 <= y2 ? y2 + (y2 - y1) * 0.618f : y2 - (y1 - y2) * 0.618f;
        yE = y1 <= y2 ? y2 + (y2 - y1) : y2 - (y1 - y2);
        return new float[]{yA, yB, yC, yD, yE};
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
        m_moveTimes++;
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
        switch (m_action) {
            case AT1:
                resize(0);
                break;
            case AT2:
                resize(1);
                break;
            case MOVE:
                double subY = mp.y - m_startPoint.y;
                double maxValue = m_div.getVScale(m_attachVScale).getVisibleMax();
                double minValue = m_div.getVScale(m_attachVScale).getVisibleMin();
                double yAddValue = subY / getWorkingAreaHeight() * (minValue - maxValue);
                m_marks.put(0, new PlotMark(0, m_startMarks.get(0).getKey(), m_startMarks.get(0).getValue() + yAddValue));
                m_marks.put(1, new PlotMark(1, m_startMarks.get(1).getKey(), m_startMarks.get(1).getValue() + yAddValue));
                break;
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
        int bIndex = m_dataSource.getRowIndex(pList.get(0).getKey());
        float x1 = pX(bIndex);
        // 获取直线参数
        float[] lineParam = levelGradingParams(pList.get(0).getValue(), pList.get(1).getValue());
        String[] str = null;
        if (y1 >= y2) {
            str = new String[]{"-100%", "0.00%", "38.20%", "61.80%", "100%"};
        } else {
            str = new String[]{"100%", "0.00%", "-38.20%", "-61.80%", "-100%"};
        }
        // 画文字和线
        for (int i = 0; i < lineParam.length; i++) {
            FCSize sizeF = textSize(paint, str[i], m_font);
            float yP = lineParam[i];
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, 0, yP, getWorkingAreaWidth(), yP);
            drawText(paint, str[i], lineColor, m_font, getWorkingAreaWidth() - sizeF.cx, yP - sizeF.cy);
        }
        // 画选中
        if (isSelected()) {
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x1, y2);
        }
    }
}
