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
 * 幅度尺
 */
public class RangeRuler extends FCPlot {

    /**
     * 创建幅度尺
     */
    public RangeRuler() {
        setPlotType("RANGERULER");
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
        // 获取参数
        double[] param = getCandleRange(m_marks);
        double nHigh = param[0];
        double nLow = param[1];
        float highY = pY(nHigh);
        float lowY = pY(nLow);
        // 获取较大的X和较小的X
        float smallX = x1 > x2 ? x2 : x1;
        float bigX = x1 > x2 ? x1 : x2;
        // 选中上面的线
        if ((mp.y >= highY - m_lineWidth * 2.5f && mp.y <= highY + m_lineWidth * 2.5f) || (mp.y >= lowY - m_lineWidth * 2.5f && mp.y <= lowY + m_lineWidth * 2.5f)) {
            if (mp.x >= x1 - m_lineWidth * 2.5f && mp.x <= x1 + m_lineWidth * 2.5f) {
                action = ActionType.AT1;
                return action;
            } else if (mp.x >= x2 - m_lineWidth * 2.5f && mp.x <= x2 + m_lineWidth * 2.5f) {
                action = ActionType.AT2;
                return action;
            }
        }
        // 选中下面的线
        if (mp.y >= highY - m_lineWidth * 2.5f && mp.y <= highY + m_lineWidth * 2.5f) {
            if (mp.x >= smallX - m_lineWidth * 2.5f && mp.x <= bigX + m_lineWidth * 2.5f) {
                action = ActionType.MOVE;
                return action;
            }
        } else if (mp.y >= lowY - m_lineWidth * 2.5f && mp.y <= lowY + m_lineWidth * 2.5f) {
            if (mp.x >= smallX - m_lineWidth * 2.5f && mp.x <= bigX + m_lineWidth * 2.5f) {
                action = ActionType.MOVE;
                return action;
            }
        }
        // 选中中线
        float mid = x1 >= x2 ? (x2 + (x1 - x2) / 2) : (x1 + (x2 - x1) / 2);
        if (mp.x >= mid - m_lineWidth * 2.5f && mp.x <= mid + m_lineWidth * 2.5f) {
            if (mp.y >= highY - m_lineWidth * 2.5f && mp.y <= lowY + m_lineWidth * 2.5f) {
                action = ActionType.MOVE;
                return action;
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
        float smallX = x1 > x2 ? x2 : x1;
        float bigX = x1 > x2 ? x1 : x2;
        // 获取参数
        double[] param = getCandleRange(pList);
        double nHigh = param[0];
        double nLow = param[1];
        float highY = pY(nHigh);
        float lowY = pY(nLow);
        float mid = x1 >= x2 ? (x2 + (x1 - x2) / 2) : (x1 + (x2 - x1) / 2);
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, highY, x2, highY);
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, lowY, x2, lowY);
        drawLine(paint, lineColor, m_lineWidth, 1, mid, lowY, mid, highY);
        if (nHigh != nLow) {
            // 画文字
            double diff = Math.abs(nLow - nHigh);
            double range = 0;
            if (nHigh != 0) {
                range = diff / nHigh;
            }
            FCChart chart = getChart();
            String diffString = FCStr.getValueByDigit(diff, m_div.getVScale(m_attachVScale).getDigit());
            String rangeString = String.format("%.2f", range);
            FCSize diffSize = textSize(paint, diffString, m_font);
            FCSize rangeSize = textSize(paint, rangeString, m_font);
            drawText(paint, diffString, lineColor, m_font, bigX - diffSize.cx, highY + 2);
            drawText(paint, rangeString, lineColor, m_font, bigX - rangeSize.cx, lowY - rangeSize.cy);
        }
        // 画选中点
        if (isSelected()) {
            drawSelect(paint, lineColor, smallX, highY);
            drawSelect(paint, lineColor, smallX, lowY);
            drawSelect(paint, lineColor, bigX, highY);
            drawSelect(paint, lineColor, bigX, lowY);
        }
    }
}
