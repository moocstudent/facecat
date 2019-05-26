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

public class RectLine extends FCPlot {

    public RectLine() {
        setPlotType("FCRect");
    }

    private FCPoint oppositePoint = new FCPoint();

    @Override
    public ActionType getAction() {
        ActionType action = ActionType.NO;
        if (m_marks.isEmpty()) {
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        action = selectRect(mp, m_marks.get(0), m_marks.get(1));
        return action;
    }

    @Override
    public boolean onCreate(FCPoint mp) {
        return create2PointsB(mp);
    }

    @Override
    public void onMoveStart() {
        m_action = getAction();
        if (m_action != ActionType.MOVE && m_action != ActionType.NO) {
            FCRect rect = getRectangle(m_marks.get(0), m_marks.get(1));
            int x1 = rect.left;
            int y1 = rect.top;
            int x2 = rect.right;
            int y2 = rect.top;
            int x3 = rect.left;
            int y3 = rect.bottom;
            int x4 = rect.right;
            int y4 = rect.bottom;
            switch (m_action) {
                case AT1:
                    oppositePoint = new FCPoint(x4, y4);
                    break;
                case AT2:
                    oppositePoint = new FCPoint(x3, y3);
                    break;
                case AT3:
                    oppositePoint = new FCPoint(x2, y2);
                    break;
                case AT4:
                    oppositePoint = new FCPoint(x1, y1);
                    break;
            }
        }
        m_moveTimes++;
        m_startMarks.clear();
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType.NO) {
            m_startMarks.put(0, m_marks.get(0));
            m_startMarks.put(1, m_marks.get(1));
        }
    }

    @Override
    public void onMoving() {
        FCPoint mp = getMovingPoint();
        switch (m_action) {
            case MOVE:
                move(mp);
                break;
            case AT1:
            case AT2:
            case AT3:
            case AT4:
                resize(mp, oppositePoint);
                break;
        }
    }

    @Override
    public void onPaintGhost(FCPaint paint) {
        if (m_moveTimes > 1) {
            paint(paint, m_startMarks, getSelectedColor());
        }
    }

    @Override
    protected void paint(FCPaint paint, java.util.HashMap<Integer, PlotMark> pList, long lineColor) {
        if (pList.isEmpty()) {
            return;
        }
        FCRect rect = getRectangle(pList.get(0), pList.get(1));
        if (rect.right - rect.left > 0 && rect.bottom - rect.top > 0) {
            drawRect(paint, lineColor, m_lineWidth, m_lineStyle, rect.left, rect.top, rect.right, rect.bottom);
        }
        if (isSelected()) {
            drawSelect(paint, lineColor, rect.left, rect.top);
            drawSelect(paint, lineColor, rect.right, rect.top);
            drawSelect(paint, lineColor, rect.left, rect.bottom);
            drawSelect(paint, lineColor, rect.right, rect.bottom);
        }
    }
}
