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
 * 椭圆
 */
public class Ellipse extends FCPlot {

    /**
     * 创建椭圆
     */
    public Ellipse() {
        setPlotType("ELLIPSE");
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
        double fValue = m_marks.get(0).getValue();
        double eValue = m_marks.get(1).getValue();
        int bIndex = m_dataSource.getRowIndex(m_marks.get(0).getKey());
        int eIndex = m_dataSource.getRowIndex(m_marks.get(1).getKey());
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float y1 = pY(fValue);
        float y2 = pY(eValue);
        float x = 0;
        float y = 0;
        if (x1 >= x2) {
            x = x2;
        } else {
            x = x2 - (x2 - x1) * 2;
        }
        if (y1 >= y2) {
            y = y1 - (y1 - y2) * 2;
        } else {
            y = y1;
        }
        if (selectPoint(mp, x1, y1)) {
            action = ActionType.AT1;
            return action;
        }
        else if (selectPoint(mp, x2, y2)) {
            action = ActionType.AT2;
            return action;
        }
        float width = Math.abs((x1 - x2) * 2);
        float height = Math.abs((y1 - y2) * 2);
        float oX = x + width / 2;
        float oY = y + height / 2;
        float a = 0;
        float b = 0;
        RefObject<Float> tempRef_a = new RefObject<Float>(a);
        RefObject<Float> tempRef_b = new RefObject<Float>(b);
        ellipseAB(width, height, tempRef_a, tempRef_b);
        a = tempRef_a.argvalue;
        b = tempRef_b.argvalue;
        if (a != 0 && b != 0) {
            float clickX = mp.x - oX;
            float clickY = mp.y - oY;
            double ellipseValue = clickX * clickX / (a * a) + clickY * clickY / (b * b);
            if (ellipseValue >= 0.8 && ellipseValue <= 1.2) {
                action = ActionType.MOVE;
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
        double fValue = pList.get(0).getValue();
        double eValue = pList.get(1).getValue();
        int bIndex = m_dataSource.getRowIndex(pList.get(0).getKey());
        int eIndex = m_dataSource.getRowIndex(pList.get(1).getKey());
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float y1 = pY(fValue);
        float y2 = pY(eValue);
        float x = x1 - (x1 - x2);
        float y = 0;
        float width = (x1 - x2) * 2;
        float height = 0;
        if (y1 >= y2) {
            height = (y1 - y2) * 2;
        } else {
            height = (y2 - y1) * 2;
        }
        y = y2 - height / 2;
        if (width == 0) {
            width = 1;
        }
        if (height == 0) {
            height = 1;
        }
        if (width == 1 && height == 1) {
            drawEllipse(paint, lineColor, m_lineWidth, m_lineStyle, x - 2, y - 2, x + 2, y + 2);
        } else {
            drawEllipse(paint, lineColor, m_lineWidth, m_lineStyle, x, y, x + width, y + height);
        }
        if (isSelected()) {
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x2, y2);
        }
    }
}
