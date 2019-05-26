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
 * 波浪尺
 */
public class WaveRuler extends FCPlot {

    /**
     * 创建波浪尺
     */
    public WaveRuler() {
        setPlotType("WAVERULER");
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
        // 获取参数不
        float[] param = getWaveRulerParams(m_marks.get(0).getValue(), m_marks.get(1).getValue());
        int bIndex = m_dataSource.getRowIndex(m_marks.get(0).getKey());
        int eIndex = m_dataSource.getRowIndex(m_marks.get(1).getKey());
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float y1 = pY(m_marks.get(0).getValue());
        float y2 = param[param.length - 1];
        if (selectPoint(mp, x1, y1) || m_moveTimes == 1) {
            action = ActionType.AT1;
            return action;
        } else if (selectPoint(mp, x2, y2)) {
            action = ActionType.AT2;
            return action;
        }
        // 判断选中
        float smallY = param[0] < param[param.length - 1] ? param[0] : param[param.length - 1];
        float bigY = param[0] >= param[param.length - 1] ? param[0] : param[param.length - 1];
        float mid = x1 >= x2 ? (x2 + (x1 - x2) / 2) : (x1 + (x2 - x1) / 2);
        if (mp.x >= mid - m_lineWidth * 2.5f && mp.x <= mid + m_lineWidth * 2.5f && mp.y >= smallY - m_lineWidth * 2.5f && mp.y <= bigY + m_lineWidth * 2.5f) {
            action = ActionType.MOVE;
            return action;
        }
        float top = 0;
        float bottom = getWorkingAreaWidth();
        if (mp.y >= top && mp.y <= bottom) {
            for (float p : param) {
                if (mp.x >= 0 && mp.x <= getWorkingAreaWidth() && mp.y >= p - m_lineWidth * 2.5f && mp.y <= p + m_lineWidth * 2.5f) {
                    action = ActionType.MOVE;
                    return action;
                }
            }
        }
        return action;
    }

    /**
     * 获取波浪尺参数
     *
     * @param value1 值1
     * @param value2 值2
     * @returns 波浪尺参数
     */
    private float[] getWaveRulerParams(double value1, double value2) {
        float y1 = pY(value1);
        float y2 = pY(value2);
        float y0 = 0, yA = 0, yB = 0, yC = 0, yD = 0, yE = 0, yF = 0, yG = 0, yH = 0, yI = 0, yMax = 0;
        y0 = y1;
        yA = y1 <= y2 ? y1 + (y2 - y1) * (0.236f / 2.618f) : y2 + (y1 - y2) * (1 - 0.236f / 2.618f);
        yB = y1 <= y2 ? y1 + (y2 - y1) * (0.362f / 2.618f) : y2 + (y1 - y2) * (1 - 0.362f / 2.618f);
        yC = y1 <= y2 ? y1 + (y2 - y1) * (0.5f / 2.618f) : y2 + (y1 - y2) * (1 - 0.5f / 2.618f);
        yD = y1 <= y2 ? y1 + (y2 - y1) * (0.618f / 2.618f) : y2 + (y1 - y2) * (1 - 0.618f / 2.618f);
        yE = y1 <= y2 ? y1 + (y2 - y1) * (1 / 2.618f) : y2 + (y1 - y2) * (1 - 1 / 2.618f);
        yF = y1 <= y2 ? y1 + (y2 - y1) * (1.382f / 2.618f) : y2 + (y1 - y2) * (1 - 1.382f / 2.618f);
        yG = y1 <= y2 ? y1 + (y2 - y1) * (1.618f / 2.618f) : y2 + (y1 - y2) * (1 - 1.618f / 2.618f);
        yH = y1 <= y2 ? y1 + (y2 - y1) * (2 / 2.618f) : y2 + (y1 - y2) * (1 - 2 / 2.618f);
        yI = y1 <= y2 ? y1 + (y2 - y1) * (2.382f / 2.618f) : y2 + (y1 - y2) * (1 - 2.382f / 2.618f);
        yMax = y2;
        return new float[]{y0, yA, yB, yC, yD, yE, yF, yG, yH, yI, yMax};
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
        int eIndex = m_dataSource.getRowIndex(pList.get(1).getKey());
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        // 画文字和线条
        float[] lineParam = getWaveRulerParams(pList.get(0).getValue(), pList.get(1).getValue());
        String[] str = new String[]{"0.00%", "23.60%", "38.20%", "50.00%", "61.80%", "100.00%", "138.20%", "161.80%", "200%", "238.20%", "261.80%"};
        float mid = x1 >= x2 ? (x2 + (x1 - x2) / 2) : (x1 + (x2 - x1) / 2);
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, mid, lineParam[0], mid, lineParam[lineParam.length - 1]);
        for (int i = 0; i < lineParam.length; i++) {
            FCSize sizeF = textSize(paint, str[i], m_font);
            float yP = lineParam[i];
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, yP, x2, yP);
            ;
            drawText(paint, str[i], lineColor, m_font, mid, yP - sizeF.cy);
        }
        // 画选中点
        if (isSelected()) {
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x2, y2);
        }
    }
}
