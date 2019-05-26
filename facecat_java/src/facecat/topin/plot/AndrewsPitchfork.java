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
 * 安德鲁斯干草叉
 */
public class AndrewsPitchfork extends FCPlot {

    /**
     * 创建安德鲁斯干草叉
     */
    public AndrewsPitchfork() {
        setPlotType("ANDREWSPITCHFORK");
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
        if (m_sourceFields == null || !m_sourceFields.containsKey("CLOSE")) {
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        // 取索引
        int aIndex = m_dataSource.getRowIndex(m_marks.get(0).getKey());
        int bIndex = m_dataSource.getRowIndex(m_marks.get(1).getKey());
        int cIndex = m_dataSource.getRowIndex(m_marks.get(2).getKey());
        int dIndex = m_dataSource.getRowIndex(m_marks.get(3).getKey());
        float x1 = pX(aIndex);
        float x2 = pX(bIndex);
        float x3 = pX(cIndex);
        float x4 = pX(dIndex);
        float y1 = pY(m_dataSource.get2(aIndex, m_sourceFields.get("CLOSE")));
        float y2 = pY(m_marks.get(1).getValue());
        float y3 = pY(m_dataSource.get2(cIndex, m_sourceFields.get("CLOSE")));
        float y4 = pY(m_dataSource.get2(dIndex, m_sourceFields.get("CLOSE")));
        // 判断是否选中点
        boolean selected = selectPoint(mp, x1, y1);
        if (selected) {
            action = ActionType.AT1;
            return action;
        }
        selected = selectPoint(mp, x2, y2);
        if (selected) {
            action = ActionType.AT2;
            return action;
        }
        selected = selectPoint(mp, x3, y3);
        if (selected) {
            action = ActionType.AT3;
            return action;
        }
        selected = selectPoint(mp, x4, y4);
        if (selected) {
            action = ActionType.AT4;
            return action;
        }
        // 判断是否选中射线
        float k = 0, b = 0;
        RefObject<Float> tempRef_k = new RefObject<Float>(k);
        RefObject<Float> tempRef_b = new RefObject<Float>(b);
        selected = selectRay(mp, x1, y1, x2, y2, tempRef_k, tempRef_b);
        k = tempRef_k.argvalue;
        b = tempRef_b.argvalue;
        if (selected) {
            action = ActionType.MOVE;
            return action;
        }
        int wWidth = getWorkingAreaWidth();
        // 非垂直时
        if (k != 0 || b != 0) {
            float x3_newx = wWidth;
            if (bIndex < aIndex) {
                x3_newx = 0;
            }
            b = y3 - x3 * k;
            float x3_newy = k * x3_newx + b;
            selected = selectRay(mp, x3, y3, x3_newx, x3_newy);
            if (selected) {
                action = ActionType.MOVE;
                return action;
            }
            float x4_newx = wWidth;
            if (bIndex < aIndex) {
                x4_newx = 0;
            }
            b = y4 - x4 * k;
            float x4_newy = k * x4_newx + b;
            selected = selectRay(mp, x4, y4, x4_newx, x4_newy);
            if (selected) {
                action = ActionType.MOVE;
                return action;
            }
        }
        // 垂直时
        else {
            if (y1 >= y2) {
                selected = selectRay(mp, x3, y3, x3, 0);
                if (selected) {
                    action = ActionType.MOVE;
                    return action;
                }
                selected = selectRay(mp, x4, y4, x4, 0);
                if (selected) {
                    action = ActionType.MOVE;
                    return action;
                }
            } else {
                int wHeight = getWorkingAreaHeight();
                selected = selectRay(mp, x3, y3, x3, wHeight);
                if (selected) {
                    action = ActionType.MOVE;
                    return action;
                }
                selected = selectRay(mp, x4, y4, x4, wHeight);
                if (selected) {
                    action = ActionType.MOVE;
                    return action;
                }
            }
        }
        return action;
    }

    /**
     * 初始化线条
     *
     * @param mp 坐标
     * @returns 初始化是否成功
     */
    @Override
    public boolean onCreate(FCPoint mp) {
        boolean create = create4CandlePoints(mp);
        if (create) {
            m_marks.put(1, new PlotMark(m_marks.get(1).getIndex(), m_marks.get(1).getKey(), getNumberValue(mp)));
        }
        return create;
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
            m_startMarks.put(2, m_marks.get(2));
            m_startMarks.put(3, m_marks.get(3));
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
        if (m_sourceFields == null || !m_sourceFields.containsKey("CLOSE")) {
            return;
        }
        // 取索引
        int aIndex = m_dataSource.getRowIndex(m_marks.get(0).getKey());
        int bIndex = m_dataSource.getRowIndex(m_marks.get(1).getKey());
        int cIndex = m_dataSource.getRowIndex(m_marks.get(2).getKey());
        int dIndex = m_dataSource.getRowIndex(m_marks.get(3).getKey());
        float x1 = pX(aIndex);
        float x2 = pX(bIndex);
        float x3 = pX(cIndex);
        float x4 = pX(dIndex);
        // 取Y
        float y1 = pY(m_dataSource.get2(aIndex, m_sourceFields.get("CLOSE")));
        float y2 = pY(m_marks.get(1).getValue());
        float y3 = pY(m_dataSource.get2(cIndex, m_sourceFields.get("CLOSE")));
        float y4 = pY(m_dataSource.get2(dIndex, m_sourceFields.get("CLOSE")));
        float k = 0, b = 0;
        RefObject<Float> tempRef_k = new RefObject<Float>(k);
        RefObject<Float> tempRef_b = new RefObject<Float>(b);
        lineXY(x1, y1, x2, y2, 0, 0, tempRef_k, tempRef_b);
        k = tempRef_k.argvalue;
        b = tempRef_b.argvalue;
        drawRay(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2, k, b);
        // 画斜线
        if (k != 0 || b != 0) {
            float x3_newx = getWorkingAreaWidth();
            if (bIndex < aIndex) {
                x3_newx = 0;
            }
            b = y3 - x3 * k;
            float x3_newy = k * x3_newx + b;
            drawRay(paint, lineColor, m_lineWidth, m_lineStyle, x3, y3, x3_newx, x3_newy, k, b);
            float x4_newx = getWorkingAreaWidth();
            if (bIndex < aIndex) {
                x4_newx = 0;
            }
            b = y4 - x4 * k;
            float x4_newy = k * x4_newx + b;
            drawRay(paint, lineColor, m_lineWidth, m_lineStyle, x4, y4, x4_newx, x4_newy, k, b);
        } else {
            if (y1 >= y2) {
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x3, y3, x3, 0);
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x4, y4, x4, 0);
            } else {
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x3, y3, x3, getWorkingAreaHeight());
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x4, y4, x4, getWorkingAreaHeight());
            }
        }
        if (isSelected()) {
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x2, y2);
            drawSelect(paint, lineColor, x3, y3);
            drawSelect(paint, lineColor, x4, y4);
        }
    }
}
