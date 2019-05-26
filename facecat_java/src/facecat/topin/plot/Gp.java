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
 * 黄金分割价位线
 */
public class Gp extends FCPlot {

    /**
     * 创建黄金分割价位线
     */
    public Gp() {
        setPlotType("GP");
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
        if (mp.y >= y1 - m_lineWidth * 2.5f && mp.y <= y1 + m_lineWidth * 2.5f) {
            action = ActionType.MOVE;
        } else {
            // 黄金分割位
            double[] list = new double[]{0.236, 0.382, 0.5, 0.618, 0.819, 1.191, 1.382, 1.6180, 2, 2.382, 2.618};
            for (int i = 0; i < list.length; i++) {
                // 获取坐标
                float yP = pY(list[i] * m_marks.get(0).getValue());
                if (mp.y >= yP - m_lineWidth * 2.5f && mp.y <= yP + m_lineWidth * 2.5f) {
                    action = ActionType.MOVE;
                    break;
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
        return createPoint(mp);
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
        }
    }

    /**
     * 移动线条
     */
    @Override
    public void onMoving() {
        FCPoint mp = getMovingPoint();
        switch (m_action) {
            case MOVE:
                move(mp);
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
        int bIndex = m_dataSource.getRowIndex(pList.get(0).getKey());
        float x1 = pX(bIndex);
        // 画线
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, 0, y1, getWorkingAreaWidth(), y1);
        // 黄金分割位
        double[] list = new double[]{0.236, 0.382, 0.5, 0.618, 0.819, 1.191, 1.382, 1.6180, 2, 2.382, 2.618};
        for (int i = 0; i < list.length; i++) {
           // 获取坐标
            float yP = pY(list[i] * pList.get(0).getValue());
            String str = String.format("%.2f", list[i] * 100) + "%";
            FCSize sizeF = textSize(paint, str, m_font);
            drawLine(paint, lineColor, m_lineWidth, 1, 0, yP, getWorkingAreaWidth(), yP);
            drawText(paint, str, lineColor, m_font, getWorkingAreaWidth() - sizeF.cx, yP - sizeF.cy);
        }
        // 画选中
        if (isSelected()) {
            drawSelect(paint, lineColor, x1, y1);
        }
    }
}
