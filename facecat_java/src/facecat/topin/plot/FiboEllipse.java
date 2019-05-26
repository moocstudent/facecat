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
 * 斐波圆
 */
public class FiboEllipse extends FCPlot {

    /**
     * 创建斐波圆
     */
    public FiboEllipse() {
        setPlotType("FIBOELLIPSE");
    }

    /**
     * 斐波圆的参数
     *
     * @param pList 点阵描述集合
     * @returns 斐波圆的参数
     */
    private float[] fibonacciEllipseParam(java.util.HashMap<Integer, PlotMark> pList) {
        if (pList.isEmpty()) {
            return null;
        }
        int bIndex = m_dataSource.getRowIndex(pList.get(0).getKey());
        int eIndex = m_dataSource.getRowIndex(pList.get(1).getKey());
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float y1 = pY(pList.get(0).getValue());
        float y2 = pY(pList.get(1).getValue());
        float r1 = (float) (Math.sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1)));
        float r2 = r1 * 0.236f;
        float r3 = r1 * 0.382f;
        float r4 = r1 * 0.5f;
        float r5 = r1 * 0.618f;
        return new float[]{x1, y1, x2, y2, r1, r2, r3, r4, r5};
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
        // 获取点的描述
        FCPoint mp = getTouchOverPoint();
        float[] param = fibonacciEllipseParam(m_marks);
        float x1 = param[0];
        float y1 = param[1];
        float x2 = param[2];
        float y2 = param[3];
        // 外侧点
        if (selectPoint(mp, x1, y1) || m_moveTimes == 1) {
            action = ActionType.AT1;
            return action;
        }
        // 内侧点
        else if (selectPoint(mp, x2, y2)) {
            action = ActionType.AT2;
            return action;
        }
        if (selectSegment(mp, x1, y1, x2, y2)) {
            action = ActionType.MOVE;
            return action;
        }
        // 判断是否是移动四个椭圆
        FCPoint p = new FCPoint(mp.x - x1, mp.y - y1);
        float round = p.x * p.x + p.y * p.y;
        for (int i = 4; i < 9; i++) {
            float r = param[i];
            if (round / (r * r) >= 0.9 && round / (r * r) <= 1.1) {
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
        // 获取点阵的值及索引，然后获取内部位置　
        float[] param = fibonacciEllipseParam(pList);
        float x1 = param[0];
        float y1 = param[1];
        float x2 = param[2];
        float y2 = param[3];
        drawLine(paint, lineColor, m_lineWidth, 1, x1, y1, x2, y2);
        float r1 = param[4] >= 4 ? param[4] : 4;
        float r2 = param[5] >= 4 ? param[5] : 4;
        float r3 = param[6] >= 4 ? param[6] : 4;
        float r4 = param[7] >= 4 ? param[7] : 4;
        float r5 = param[8] >= 4 ? param[8] : 4;
        // 画椭圆
        drawEllipse(paint, lineColor, m_lineWidth, m_lineStyle, x1 - r1, y1 - r1, x1 + r1, y1 + r1);
        drawEllipse(paint, lineColor, m_lineWidth, m_lineStyle, x1 - r2, y1 - r2, x1 + r2, y1 + r2);
        drawEllipse(paint, lineColor, m_lineWidth, m_lineStyle, x1 - r3, y1 - r3, x1 + r3, y1 + r3);
        drawEllipse(paint, lineColor, m_lineWidth, m_lineStyle, x1 - r4, y1 - r4, x1 + r4, y1 + r4);
        drawEllipse(paint, lineColor, m_lineWidth, m_lineStyle, x1 - r5, y1 - r5, x1 + r5, y1 + r5);
        // 画选中点
        if (isSelected()) {
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x2, y2);
        }
        if (r5 > 20) {
            // 绘制文字
            FCSize sizeF = textSize(paint, "23.6%", m_font);
            drawText(paint, "23.6%", lineColor, m_font, x1 - sizeF.cx / 2, y1 - r1 - sizeF.cy);
            sizeF = textSize(paint, "38.2%", m_font);
            drawText(paint, "38.2%", lineColor, m_font, x1 - sizeF.cx / 2, y1 - r2 - sizeF.cy);
            sizeF = textSize(paint, "50.0%", m_font);
            drawText(paint, "50.0%", lineColor, m_font, x1 - sizeF.cx / 2, y1 - r3 - sizeF.cy);
            sizeF = textSize(paint, "61.8%", m_font);
            drawText(paint, "61.8%", lineColor, m_font, x1 - sizeF.cx / 2, y1 - r4 - sizeF.cy);
            sizeF = textSize(paint, "100%", m_font);
            drawText(paint, "100%", lineColor, m_font, x1 - sizeF.cx / 2, y1 - r5 - sizeF.cy);
        }
    }
}
