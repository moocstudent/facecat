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
 * 箭头线段
 */
public class ArrowSegment extends FCPlot {

    /**
     * 创建箭头线段
     */
    public ArrowSegment() {
        setPlotType("ARROWSEGMENT");
    }

    private static int ARROW_SIZE = 10;

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
        float y1 = pY(m_marks.get(0).getValue());
        float y2 = pY(m_marks.get(1).getValue());
        int bIndex = m_dataSource.getRowIndex(m_marks.get(0).getKey());
        int eIndex = m_dataSource.getRowIndex(m_marks.get(1).getKey());
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        // 获取参数
        float[] param = getLineParams(m_marks.get(0), m_marks.get(1));
        // 产生动作类型
        if (param != null) {
            if (mp.x >= x1 - m_lineWidth * 2.5f && mp.x <= x1 + m_lineWidth * 2.5f) {
                action = ActionType.AT1;
                return action;
            } else if (mp.x >= x2 - m_lineWidth * 2.5f && mp.x <= x2 + m_lineWidth * 2.5f) {
                action = ActionType.AT2;
                return action;
            }
        } else {
            if (mp.y >= y1 - m_lineWidth * 5 && mp.y <= y1 + m_lineWidth * 5) {
                action = ActionType.AT1;
                return action;
            } else if (mp.y >= y2 - m_lineWidth * 5 && mp.y <= y2 + m_lineWidth * 5) {
                action = ActionType.AT2;
                return action;
            }
        }
        if (selectSegment(mp, x1, y1, x2, y2)) {
            action = ActionType.MOVE;
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
        int y1 = (int) pY(pList.get(0).getValue());
        int y2 = (int) pY(pList.get(1).getValue());
        int bIndex = m_dataSource.getRowIndex(pList.get(0).getKey());
        int eIndex = m_dataSource.getRowIndex(pList.get(1).getKey());
        int x1 = (int) pX(bIndex);
        int x2 = (int) pX(eIndex);
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
        double slopy = 0, cosy = 0, siny = 0;
        slopy = Math.atan2((double) (y1 - y2), (double) (x1 - x2));
        cosy = Math.cos(slopy);
        siny = Math.sin(slopy);
        FCPoint pt1 = new FCPoint(x2, y2);
        FCPoint pt2 = new FCPoint(pt1.x + (int) (ARROW_SIZE * cosy - (ARROW_SIZE / 2.0 * siny) + 0.5), pt1.y + (int) (ARROW_SIZE * siny + (ARROW_SIZE / 2.0 * cosy) + 0.5));
        FCPoint pt3 = new FCPoint(pt1.x + (int) (ARROW_SIZE * cosy + ARROW_SIZE / 2.0 * siny + 0.5), pt1.y - (int) (ARROW_SIZE / 2.0 * cosy - ARROW_SIZE * siny + 0.5));
        FCPoint[] points = new FCPoint[]{pt1, pt2, pt3};
        fillPolygon(paint, lineColor, points);
        if (isSelected()) {
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x2, y2);
        }
    }
}
