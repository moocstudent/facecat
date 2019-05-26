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

/**
 * 外接圆
 */
public class CircumCircle extends FCPlot {

    /**
     * 创建外接圆
     */
    public CircumCircle() {
        setPlotType("CIRCUMCIRCLE");
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
        float y1 = pY(m_marks.get(0).getValue());
        float y2 = pY(m_marks.get(1).getValue());
        float y3 = pY(m_marks.get(2).getValue());
        int bIndex = m_dataSource.getRowIndex(m_marks.get(0).getKey());
        int eIndex = m_dataSource.getRowIndex(m_marks.get(1).getKey());
        int pIndex = m_dataSource.getRowIndex(m_marks.get(2).getKey());
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float x3 = pX(pIndex);
        float Ox = 0, Oy = 0, r = 0;
        RefObject<Float> tempRef_Ox = new RefObject<Float>(Ox);
        RefObject<Float> tempRef_Oy = new RefObject<Float>(Oy);
        RefObject<Float> tempRef_r = new RefObject<Float>(r);
        // 获取圆的参数
        ellipseOR(x1, y1, x2, y2, x3, y3, tempRef_Ox, tempRef_Oy, tempRef_r);
        Ox = tempRef_Ox.argvalue;
        Oy = tempRef_Oy.argvalue;
        r = tempRef_r.argvalue;
        float clickX = mp.x - Ox;
        float clickY = mp.y - Oy;
        double ellipseValue = clickX * clickX + clickY * clickY;
        // 模糊选中
        if (ellipseValue >= r * r * 0.8 && ellipseValue <= r * r * 1.2) {
            if (selectPoint(mp, x1, y1)) {
                action = ActionType.AT1;
            } else if (selectPoint(mp, x2, y2)) {
                action = ActionType.AT2;
            } else if (selectPoint(mp, x3, y3)) {
                action = ActionType.AT3;
            } else {
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
        return create3Points(mp);
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
        float y3 = pY(pList.get(2).getValue());
        int bIndex = m_dataSource.getRowIndex(pList.get(0).getKey());
        int eIndex = m_dataSource.getRowIndex(pList.get(1).getKey());
        int pIndex = m_dataSource.getRowIndex(pList.get(2).getKey());
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float x3 = pX(pIndex);
        float Ox = 0, Oy = 0, r = 0;
        RefObject<Float> tempRef_Ox = new RefObject<Float>(Ox);
        RefObject<Float> tempRef_Oy = new RefObject<Float>(Oy);
        RefObject<Float> tempRef_r = new RefObject<Float>(r);
        // 获取圆的参数
        ellipseOR(x1, y1, x2, y2, x3, y3, tempRef_Ox, tempRef_Oy, tempRef_r);
        Ox = tempRef_Ox.argvalue;
        Oy = tempRef_Oy.argvalue;
        r = tempRef_r.argvalue;
        drawEllipse(paint, lineColor, m_lineWidth, m_lineStyle, Ox - r, Oy - r, Ox + r, Oy + r);
        // 画选中点
        if (isSelected()) {
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x2, y2, x3, y3);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x3, y3, x1, y1);
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x2, y2);
            drawSelect(paint, lineColor, x3, y3);
        }
    }
}
