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
 * 泰龙水平线
 */
public class TironeLevels extends FCPlot {

    /**
     * 创建泰龙水平线
     */
    public TironeLevels() {
        setPlotType("TIRONELEVELS");
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
        double[] param = getTironelLevelsParams(m_marks);
        double nHigh = param[0];
        double nLow = param[4];
        float highY = pY(nHigh);
        float lowY = pY(nLow);
        // 获取较大的X和较小的X
        float smallX = x1 > x2 ? x2 : x1;
        float bigX = x1 > x2 ? x1 : x2;
        if ((mp.y >= highY - m_lineWidth * 2.5f && mp.y <= highY + m_lineWidth * 2.5f) || (mp.y >= lowY - m_lineWidth * 2.5f && mp.y <= lowY + m_lineWidth * 2.5f)) {
            if (mp.x >= x1 - m_lineWidth * 2.5f && mp.x <= x1 + m_lineWidth * 2.5f) {
                action = ActionType.AT1;
                return action;
            } else if (mp.x >= x2 - m_lineWidth * 2.5f && mp.x <= x2 + m_lineWidth * 2.5f) {
                action = ActionType.AT2;
                return action;
            }
        }
        // 选中上面的线
        if (mp.y >= highY - m_lineWidth * 2.5f && mp.y <= highY + m_lineWidth * 2.5f) {
            if (mp.x >= smallX - m_lineWidth * 2.5f && mp.x <= bigX + m_lineWidth * 2.5f) {
                action = ActionType.MOVE;
                return action;
            }
        }
        // 选中下面的线
        else if (mp.y >= lowY - m_lineWidth * 2.5f && mp.y <= lowY + m_lineWidth * 2.5f) {
            if (mp.x >= smallX - m_lineWidth * 2.5f && mp.x <= bigX + m_lineWidth * 2.5f) {
                action = ActionType.MOVE;
                return action;
            }
        }
        // 水平线判断
        for (int i = 1; i < param.length - 1; i++) {
            float y = pY(param[i]);
            if (mp.y >= y - m_lineWidth * 2.5f && mp.y <= y + m_lineWidth * 2.5f) {
                action = ActionType.MOVE;
                return action;
            }
        }
        return action;
    }

    /**
     * 获取泰龙水平线的参数
     *
     * @param pList 点阵集合
     * @returns 泰龙水平线的参数
     */
    private double[] getTironelLevelsParams(java.util.HashMap<Integer, PlotMark> pList) {
        double[] hl = getCandleRange(pList);
        if (hl != null) {
            double nHigh = hl[0];
            double nLow = hl[1];
            return new double[]{nHigh, nHigh - (nHigh - nLow) / 3, nHigh - (nHigh - nLow) / 2, nHigh - 2 * (nHigh - nLow) / 3, nLow};
        } else {
            return null;
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
        double[] param = getTironelLevelsParams(pList);
        double nHigh = param[0];
        double nLow = param[4];
        float highY = pY(nHigh);
        float lowY = pY(nLow);
        float mid = x1 >= x2 ? (x2 + (x1 - x2) / 2) : (x1 + (x2 - x1) / 2);
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, highY, x2, highY);
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, lowY, x2, lowY);
        drawLine(paint, lineColor, m_lineWidth, 1, mid, lowY, mid, highY);
        for (int i = 1; i < param.length - 1; i++) {
            float y = pY(param[i]);
            // 画直线
            drawLine(paint, lineColor, m_lineWidth, 1, 0, y, getWorkingAreaWidth(), y);
            String str = (new Integer(i)).toString() + "/3";
            if (i == 2) {
                str = "1/2";
            }
            FCSize sizeF = textSize(paint, str, m_font);
            drawText(paint, str, lineColor, m_font, getWorkingAreaWidth() - sizeF.cx, y - sizeF.cy);
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
