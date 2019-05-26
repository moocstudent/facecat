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
 * 百分比线
 */
public class Percent extends FCPlot {

    /**
     * 创建百分比线
     */
    public Percent() {
        setPlotType("PERCENT");
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
        if (hLinesSelect(getPercentParams(m_marks.get(0).getValue(), m_marks.get(1).getValue()), 5)) {
            action = ActionType.MOVE;
        }
        return action;
    }

    /**
     * 获取百分比线的参数
     *
     * @param value1 值1
     * @param value2 值2
     * @returns 百分比线的参数
     */
    protected float[] getPercentParams(double value1, double value2) {
        float y1 = pY(value1);
        float y2 = pY(value2);
        float y0 = 0, y25 = 0, y50 = 0, y75 = 0, y100 = 0;
        y0 = y1;
        y25 = y1 <= y2 ? y1 + (y2 - y1) / 4 : y2 + (y1 - y2) * 3 / 4;
        y50 = y1 <= y2 ? y1 + (y2 - y1) / 2 : y2 + (y1 - y2) / 2;
        y75 = y1 <= y2 ? y1 + (y2 - y1) * 3 / 4 : y2 + (y1 - y2) / 4;
        y100 = y2;
        return new float[]{y0, y25, y50, y75, y100};
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
        // 根据不同类型作出动作
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
        // 获取参数
        float[] lineParam = getPercentParams(pList.get(0).getValue(), pList.get(1).getValue());
        String[] str = new String[]{"0.00%", "25.00%", "50.00%", "75.00%", "100.00%"};
        // 画线和文字
        for (int i = 0; i < lineParam.length; i++) {
            FCSize sizeF = textSize(paint, str[i], m_font);
            float yP = lineParam[i];
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, 0, yP, getWorkingAreaWidth(), yP);
            drawText(paint, str[i], lineColor, m_font, getWorkingAreaWidth() - sizeF.cx, yP - sizeF.cy);
        }
        // 画选中点
        if (isSelected()) {
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x1, y2);
        }
    }
}
