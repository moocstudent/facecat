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
 * 速阻线
 */
public class SpeedResist extends FCPlot {

    /**
     * 创建速阻线
     */
    public SpeedResist() {
        setPlotType("SPEEDRESIST");
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
        // 获取点的位置
        FCPoint mp = getTouchOverPoint();
        float y1 = pY(m_marks.get(0).getValue());
        float y2 = pY(m_marks.get(1).getValue());
        int bIndex = m_dataSource.getRowIndex(m_marks.get(0).getKey());
        int eIndex = m_dataSource.getRowIndex(m_marks.get(1).getKey());
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        if (selectPoint(mp, x1, y1) || m_moveTimes == 1) {
            action = ActionType.AT1;
            return action;
        } else if (selectPoint(mp, x2, y2)) {
            action = ActionType.AT2;
            return action;
        }
        FCPointF firstP = new FCPointF(x2, y2 - (y2 - y1) / 3);
        FCPointF secondP = new FCPointF(x2, y2 - (y2 - y1) * 2 / 3);
        FCPointF startP = new FCPointF(x1, y1);
        float oK = 0, oB = 0, fK = 0, fB = 0, sK = 0, sB = 0;
        RefObject<Float> tempRef_oK = new RefObject<Float>(oK);
        RefObject<Float> tempRef_oB = new RefObject<Float>(oB);
        // 获取直线参数
        lineXY(x1, y1, x2, y2, 0, 0, tempRef_oK, tempRef_oB);
        oK = tempRef_oK.argvalue;
        oB = tempRef_oB.argvalue;
        RefObject<Float> tempRef_fK = new RefObject<Float>(fK);
        RefObject<Float> tempRef_fB = new RefObject<Float>(fB);
        lineXY(startP.x, startP.y, firstP.x, firstP.y, 0, 0, tempRef_fK, tempRef_fB);
        fK = tempRef_fK.argvalue;
        fB = tempRef_fB.argvalue;
        RefObject<Float> tempRef_sK = new RefObject<Float>(sK);
        RefObject<Float> tempRef_sB = new RefObject<Float>(sB);
        lineXY(startP.x, startP.y, secondP.x, secondP.y, 0, 0, tempRef_sK, tempRef_sB);
        sK = tempRef_sK.argvalue;
        sB = tempRef_sB.argvalue;
        float smallX = x1 <= x2 ? x1 : x2;
        float smallY = y1 <= y2 ? y1 : y2;
        float bigX = x1 > x2 ? x1 : x2;
        float bigY = y1 > y2 ? y1 : y2;
        if (mp.x >= smallX - 2 && mp.x <= bigX + 2 && mp.y >= smallY - 2 && mp.y <= bigY + 2) {
            if (!(oK == 0 && oB == 0)) {
                if (mp.y / (mp.x * oK + oB) >= 0.9 && mp.y / (mp.x * oK + oB) <= 1.1) {
                    action = ActionType.MOVE;
                    return action;
                }
            } else {
                action = ActionType.MOVE;
                return action;
            }
        }
        if ((x2 > x1 && mp.x >= x1 - 2) || (mp.x <= x1 + 2 && x2 < x1)) {
            if (!(fK == 0 && fB == 0)) {
                if (mp.y / (mp.x * fK + fB) >= 0.9 && mp.y / (mp.x * fK + fB) <= 1.1) {
                    action = ActionType.MOVE;
                    return action;
                }
            }
            if (!(sK == 0 && sB == 0)) {
                if (mp.y / (mp.x * sK + sB) >= 0.9 && mp.y / (mp.x * sK + sB) <= 1.1) {
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
     * @returns 是否初始化成功
     */
    @Override
    public boolean onCreate(FCPoint mp) {
        return create2PointsA(mp);
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
        drawLine(paint, lineColor, m_lineWidth, 1, x1, y1, x2, y2);
        // 画选中点
        if (isSelected() || (x1 == x2)) {
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x2, y2);
        }
        if (x1 != x2 && y1 != y2) {
            FCPoint firstP = new FCPoint(x2, y2 - (y2 - y1) / 3);
            FCPoint secondP = new FCPoint(x2, y2 - (y2 - y1) * 2 / 3);
            FCPoint startP = new FCPoint(x1, y1);
            float fK = 0, fB = 0, sK = 0, sB = 0;
            // 获取直线参数
            RefObject<Float> tempRef_fK = new RefObject<Float>(fK);
            RefObject<Float> tempRef_fB = new RefObject<Float>(fB);
            lineXY(startP.x, startP.y, firstP.x, firstP.y, 0, 0, tempRef_fK, tempRef_fB);
            fK = tempRef_fK.argvalue;
            fB = tempRef_fB.argvalue;
            RefObject<Float> tempRef_sK = new RefObject<Float>(sK);
            RefObject<Float> tempRef_sB = new RefObject<Float>(sB);
            lineXY(startP.x, startP.y, secondP.x, secondP.y, 0, 0, tempRef_sK, tempRef_sB);
            sK = tempRef_sK.argvalue;
            sB = tempRef_sB.argvalue;
            float newYF = 0, newYS = 0;
            float newX = 0;
            if (x2 > x1) {
                newYF = fK * getWorkingAreaWidth() + fB;
                newYS = sK * getWorkingAreaWidth() + sB;
                newX = getWorkingAreaWidth();
            } else {
                newYF = fB;
                newYS = sB;
                newX = 0;
            }
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, startP.x, startP.y, newX, newYF);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, startP.x, startP.y, newX, newYS);
        }
    }
}
