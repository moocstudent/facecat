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
 * 对称三角形
 */
public class SymemetrictriAngle extends FCPlot {

    /**
     * 创建对称三角形
     */
    public SymemetrictriAngle() {
        setPlotType("SYMMETRICTRIANGLE");
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
        float y1 = pY(m_marks.get(0).getValue());
        float y2 = pY(m_marks.get(1).getValue());
        float y3 = pY(m_marks.get(2).getValue());
        int bIndex = m_dataSource.getRowIndex(m_marks.get(0).getKey());
        int eIndex = m_dataSource.getRowIndex(m_marks.get(1).getKey());
        int pIndex = m_dataSource.getRowIndex(m_marks.get(2).getKey());
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float x3 = pX(pIndex);
        // 获取参数
        float[] param = getSymmetricTriangleParams(m_marks);
        // 非垂直
        if (param != null) {
            if (selectPoint(mp, x1, y1)) {
                action = ActionType.AT1;
                return action;
            } else if (selectPoint(mp, x2, y2)) {
                action = ActionType.AT2;
                return action;
            } else if (selectPoint(mp, x3, y3)) {
                action = ActionType.AT3;
                return action;
            }
            // 主直线
            if (mp.y - param[0] * mp.x - param[1] >= m_lineWidth * -5 && mp.y - param[0] * mp.x - param[1] <= m_lineWidth * 5) {
                action = ActionType.MOVE;
                return action;
            }
            // 对称三角线
            if (mp.y - param[2] * mp.x - param[3] >= m_lineWidth * -5 && mp.y - param[2] * mp.x - param[3] <= m_lineWidth * 5) {
                action = ActionType.MOVE;
                return action;
            }
        }
        // 垂直
        else {
            if (mp.y >= y1 - m_lineWidth * 5 && mp.y <= y1 + m_lineWidth * 5) {
                action = ActionType.AT1;
                return action;
            } else if (mp.y >= y2 - m_lineWidth * 5 && mp.y <= y2 + m_lineWidth * 5) {
                action = ActionType.AT2;
                return action;
            } else if (mp.y >= y3 - m_lineWidth * 5 && mp.y <= y3 + m_lineWidth * 5) {
                action = ActionType.AT3;
                return action;
            }
            if (mp.x >= x1 - m_lineWidth * 5 && mp.x <= x1 + m_lineWidth * 5) {
                action = ActionType.MOVE;
                return action;
            }
            if (mp.x >= x3 - m_lineWidth * 5 && mp.x <= x3 + m_lineWidth * 5) {
                action = ActionType.MOVE;
                return action;
            }
        }
        return action;
    }

    /**
     * 获取对称三角形直线的参数
     *
     * @param pList 点阵集合
     * @returns 对称三角形直线的参数
     */
    private float[] getSymmetricTriangleParams(java.util.HashMap<Integer, PlotMark> pList) {
        if (pList.isEmpty()) {
            return null;
        }
        float y1 = pY(pList.get(0).getValue());
        float y2 = pY(pList.get(1).getValue());
        float y3 = pY(pList.get(2).getValue());
        int bIndex = m_dataSource.getRowIndex(pList.get(0).getKey());
        int eIndex = m_dataSource.getRowIndex(pList.get(1).getKey());
        int pIndex = m_dataSource.getRowIndex(pList.get(2).getKey());
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float x3 = pX(pIndex);
        float a = 0;
        if (x2 - x1 != 0) {
            a = (y2 - y1) / (x2 - x1);
            float b = y1 - a * x1;
            float c = -a;
            float d = y3 - c * x3;
            return new float[]{a, b, c, d};
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
        // 获取参数
        float[] param = getSymmetricTriangleParams(pList);
        float a = 0;
        float b = 0;
        float c = 0;
        float d = 0;
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        float x3 = pX(pIndex);
        // 非垂直
        if (param != null) {
            a = param[0];
            b = param[1];
            c = param[2];
            d = param[3];
            float leftX = 0;
            float leftY = leftX * a + b;
            float rightX = getWorkingAreaWidth();
            float rightY = rightX * a + b;
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, leftX, leftY, rightX, rightY);
            leftY = leftX * c + d;
            rightY = rightX * c + d;
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, leftX, leftY, rightX, rightY);
        }
       // 垂直
        else {
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, 0, x1, getWorkingAreaHeight());
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x3, 0, x3, getWorkingAreaHeight());
        }
        // 画选中点
        if (isSelected()) {
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x2, y2);
            drawSelect(paint, lineColor, x3, y3);
        }
    }
}
