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
import facecat.topin.plot.*;
import java.util.*;

/**
 * 角度线
 */
public class Angleline extends FCPlot {

    /**
     * 创建角度线
     */
    public Angleline() {
        setPlotType("ANGLELINE");
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
        // 获取参数
        ArrayList<PlotMark[]> mList = new ArrayList<PlotMark[]>();
        mList.add(new PlotMark[]{m_marks.get(0), m_marks.get(1)});
        mList.add(new PlotMark[]{m_marks.get(0), m_marks.get(2)});
        int listSize = mList.size();
        for (int i = 0; i < listSize; i++) {
            PlotMark markA = mList.get(i)[0];
            PlotMark markB = mList.get(i)[1];
            int bIndex = m_dataSource.getRowIndex(markA.getKey());
            int eIndex = m_dataSource.getRowIndex(markB.getKey());
            float x1 = pX(bIndex);
            float x2 = pX(eIndex);
            float y1 = pY(markA.getValue());
            float y2 = pY(markB.getValue());
            if (selectPoint(mp, x1, y1)) {
                action = ActionType.AT1;
                return action;
            } else if (selectPoint(mp, x2, y2)) {
                if (i == 0) {
                    action = ActionType.AT2;
                    return action;
                } else {
                    action = ActionType.AT3;
                    return action;
                }
            }
            float k = 0;
            float b = 0;
            RefObject<Float> tempRef_k = new RefObject<Float>(k);
            RefObject<Float> tempRef_b = new RefObject<Float>(b);
            // 获取直线参数
            lineXY(x1, y1, x2, y2, 0, 0, tempRef_k, tempRef_b);
            k = tempRef_k.argvalue;
            b = tempRef_b.argvalue;
            if (!(k == 0 && b == 0)) {
                if (mp.y / (mp.x * k + b) >= 0.9 && mp.y / (mp.x * k + b) <= 1.1) {
                    if (x1 >= x2) {
                        if (mp.x > x1 + 5) {
                            action = ActionType.NO;
                            return action;
                        }
                    } else if (x1 < x2) {
                        if (mp.x < x1 - 5) {
                            action = ActionType.NO;
                            return action;
                        }
                    }
                    action = ActionType.MOVE;
                    return action;
                }
            } else {
                if (mp.x >= x1 - m_lineWidth * 5 && mp.x <= x1 + m_lineWidth * 5) {
                    if (y1 >= y2) {
                        if (mp.y <= y1 - 5) {
                            action = ActionType.MOVE;
                            return action;
                        }
                    } else {
                        if (mp.y >= y1 - 5) {
                            action = ActionType.MOVE;
                            return action;
                        }
                    }
                }
            }
        }
        return action;
    }

    @Override
    public boolean onCreate(FCPoint mp) {
        return create3Points(mp);
    }

    /**
     * 初始化线条
     *
     * @param mp 坐标
     * @returns 是否初始化成功
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
        ArrayList<PlotMark[]> marks = new ArrayList<PlotMark[]>();
        marks.add(new PlotMark[]{pList.get(0), pList.get(1)});
        marks.add(new PlotMark[]{pList.get(0), pList.get(2)});
        int markSize = marks.size();
        for (int i = 0; i < markSize; i++) {
            PlotMark markA = marks.get(i)[0];
            PlotMark markB = marks.get(i)[1];
            float y1 = pY(markA.getValue());
            float y2 = pY(markB.getValue());
            int bIndex = m_dataSource.getRowIndex(markA.getKey());
            int eIndex = m_dataSource.getRowIndex(markB.getKey());
            // 获取参数
            float[] param = getLineParams(markA, markB);
            float a = 0;
            float b = 0;
            float x1 = pX(bIndex);
            float x2 = pX(eIndex);
            // 非垂直时
            if (param != null) {
                a = param[0];
                b = param[1];
                float leftX = 0;
                float leftY = leftX * a + b;
                float rightX = getWorkingAreaWidth();
                float rightY = rightX * a + b;
                if (x1 >= x2) {
                    drawLine(paint, lineColor, m_lineWidth, m_lineStyle, leftX, leftY, x2, y2);
                } else {
                    drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, rightX, rightY);
                }
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
            }
            // 垂直时
            else {
                if (y1 >= y2) {
                    drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, 0);
                } else {
                    drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, getWorkingAreaHeight());
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
