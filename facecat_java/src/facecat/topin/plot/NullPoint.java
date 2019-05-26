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
 * 解消点
 */
public class NullPoint extends FCPlot {

    /**
     * 创建解消点
     */
    public NullPoint() {
        setPlotType("NullPoint");
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
        // 获取相对位置
        int bIndex = m_dataSource.getRowIndex(m_marks.get(0).getKey());
        int eIndex = m_dataSource.getRowIndex(m_marks.get(1).getKey());
        double[] closeParam = getNullPointParams(m_marks);
        double leftClose = closeParam[1];
        double rightClose = closeParam[0];
        float y1 = pY(leftClose);
        float y2 = pY(rightClose);
        float x1 = pX(bIndex >= eIndex ? bIndex : eIndex);
        float x2 = pX(bIndex >= eIndex ? eIndex : bIndex);
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
        float x3 = 0, y3 = 0;
        if (y1 != y2) {
            RefObject<Float> tempRef_x3 = new RefObject<Float>(x3);
            RefObject<Float> tempRef_y3 = new RefObject<Float>(y3);
            // 第一条线段
            NullPoint(x1, y1, x2, y2, tempRef_x3, tempRef_y3);
            x3 = tempRef_x3.argvalue;
            y3 = tempRef_y3.argvalue;
        }
        if (selectTriangle(mp, x1, y1, x2, y2, x3, y3)) {
            action = ActionType.MOVE;
            return action;
        }
        return action;
    }

    /**
     * 获取解消点的参数
     *
     * @param pList 点阵集合
     * @returns 解消点的参数
     */
    private double[] getNullPointParams(java.util.HashMap<Integer, PlotMark> pList) {
        if (pList.isEmpty() || m_sourceFields == null || m_sourceFields.isEmpty() || !m_sourceFields.containsKey("CLOSE")) {
            return null;
        }
        int bIndex = m_dataSource.getRowIndex(pList.get(0).getKey());
        int eIndex = m_dataSource.getRowIndex(pList.get(1).getKey());
        double leftClose = 0, rightClose = 0;
        double close1 = m_dataSource.get2(bIndex, m_sourceFields.get("CLOSE"));
        double close2 = m_dataSource.get2(eIndex, m_sourceFields.get("CLOSE"));
        if (eIndex >= bIndex) {
            leftClose = m_dataSource.get2(bIndex, m_sourceFields.get("CLOSE"));
            rightClose = m_dataSource.get2(eIndex, m_sourceFields.get("CLOSE"));
        } else {
            leftClose = m_dataSource.get2(eIndex, m_sourceFields.get("CLOSE"));
            rightClose = m_dataSource.get2(bIndex, m_sourceFields.get("CLOSE"));
        }
        return new double[]{leftClose, rightClose};
    }

    /**
     * 计算解消点参数
     *
     * @param x1 第一个点的横坐标
     * @param y1 第一个点的纵坐标
     * @param x2 第二个点的横坐标
     * @param y2 第二个点的纵坐标
     * @param nullX 解消点的横坐标
     * @param nullY 解消点的纵坐标
     */
    private void NullPoint(float x1, float y1, float x2, float y2, RefObject<Float> nullX, RefObject<Float> nullY) {
        float k1 = 0, k2 = 0, b1 = 0, b2 = 0;
        if (y1 >= y2) {
            k1 = -(float) Math.tan(45);
            k2 = -(float) Math.tan(60);
            b1 = y1 - k1 * x1;
            b2 = y2 - k2 * x2;
        } else {
            k1 = (float) Math.tan(45);
            k2 = (float) Math.tan(60);
            b1 = y1 - k1 * x1;
            b2 = y2 - k2 * x2;
        }
        nullX.argvalue = (b2 - b1) / (k1 - k2);
        nullY.argvalue = nullX.argvalue * k1 + b1;
    }

    /**
     * 初始化线条
     *
     * @param mp 坐标
     * @returns 是否初始化成功
     */
    @Override
    public boolean onCreate(FCPoint mp) {
        return create2CandlePoints(mp);
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
        int bIndex = m_dataSource.getRowIndex(pList.get(0).getKey());
        int aIndex = m_dataSource.getRowIndex(pList.get(1).getKey());
        double[] closeParam = getNullPointParams(pList);
        double leftClose = closeParam[1];
        double rightClose = closeParam[0];
        float y1 = pY(leftClose);
        float y2 = pY(rightClose);
        float x1 = pX(bIndex >= aIndex ? bIndex : aIndex);
        float x2 = pX(bIndex >= aIndex ? aIndex : bIndex);
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
        float nullX = 0, nullY = 0;
        if (y1 != y2) {
            RefObject<Float> tempRef_nullX = new RefObject<Float>(nullX);
            RefObject<Float> tempRef_nullY = new RefObject<Float>(nullY);
            NullPoint(x1, y1, x2, y2, tempRef_nullX, tempRef_nullY);
            nullX = tempRef_nullX.argvalue;
            nullY = tempRef_nullY.argvalue;
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, nullX, nullY);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x2, y2, nullX, nullY);
        }
        // 画选中点
        if (isSelected()) {
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x2, y2);
        }
    }
}
