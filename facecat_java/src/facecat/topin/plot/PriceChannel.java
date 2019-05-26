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

/**
 * 价格通道线
 */
public class PriceChannel extends Parallel {

    /**
     * 创建价格通道线
     */
    public PriceChannel() {
        setPlotType("PRICECHANNEL");
    }

    @Override
    /**
     * 获取动作类型
     */
    public ActionType getAction() {
        ActionType action = super.getAction();
        if (action != ActionType.NO) {
            return action;
        }
        FCPoint mp = getTouchOverPoint();
        float k = 0, d = 0, x4 = 0;
        RefObject<Float> tempRef_k = new RefObject<Float>(k);
        RefObject<Float> tempRef_d = new RefObject<Float>(d);
        RefObject<Float> tempRef_x4 = new RefObject<Float>(x4);
        getLine3Params(m_marks, tempRef_k, tempRef_d, tempRef_x4);
        k = tempRef_k.argvalue;
        d = tempRef_d.argvalue;
        x4 = tempRef_x4.argvalue;
        if (k == 0 && d == 0) {
            if (mp.x >= x4 - m_lineWidth * 5 && mp.x <= x4 + m_lineWidth * 5) {
                action = ActionType.MOVE;
            }
        } else {
            if (selectLine(mp, pX(mp.x), k, d)) {
                action = ActionType.MOVE;
            }
        }
        return action;
    }

    /**
     * 获取第三条线的参数
     *
     * @param marks 点阵集合
     * @param k 直线参数k
     * @param d 直线参数d
     * @param x4 第四个点的横坐标
     */
    private void getLine3Params(java.util.HashMap<Integer, PlotMark> marks, RefObject<Float> k, RefObject<Float> d, RefObject<Float> x4) {
        int bIndex = m_dataSource.getRowIndex(m_marks.get(0).getKey());
        int pIndex = m_dataSource.getRowIndex(m_marks.get(2).getKey());
        float x1 = pX(bIndex);
        float x3 = pX(pIndex);
        float[] param = getParallelParams(m_marks);
        if (param != null) {
            k.argvalue = param[0];
            float b = param[1];
            float c = param[2];
            d.argvalue = b >= c ? b + (b - c) : b - (c - b);
        } else {
            x4.argvalue = (float) 0;
            if (x3 > x1) {
                x4.argvalue = x1 - (x3 - x1);
            } else {
                x4.argvalue = x1 + (x1 - x3);
            }
        }
    }

    @Override
    /**
     * 开始移动画线工具
     */
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

    @Override
    /**
     * 绘制图像
     *
     * @param paint 绘图对象
     */
    public void onPaint(FCPaint paint) {
        paintEx(paint, m_marks, getColor());
        super.onPaint(paint);
    }

    @Override
    /**
     * 绘制图像的残影
     *
     * @param paint 绘图对象
     */
    public void onPaintGhost(FCPaint paint) {
        paintEx(paint, m_startMarks, getSelectedColor());
        super.onPaintGhost(paint);
    }

    /**
     * 绘制图像的方法
     *
     * @param pList 横纵值描述
     * @param lineColor 颜色
     */
    private void paintEx(FCPaint paint, java.util.HashMap<Integer, PlotMark> pList, long lineColor) {
        if (pList.isEmpty()) {
            return;
        }
        float k = 0, d = 0, x4 = 0;
        RefObject<Float> tempRef_k = new RefObject<Float>(k);
        RefObject<Float> tempRef_d = new RefObject<Float>(d);
        RefObject<Float> tempRef_x4 = new RefObject<Float>(x4);
        getLine3Params(m_marks, tempRef_k, tempRef_d, tempRef_x4);
        k = tempRef_k.argvalue;
        d = tempRef_d.argvalue;
        x4 = tempRef_x4.argvalue;
        if (k == 0 && d == 0) {
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x4, 0, x4, getWorkingAreaHeight());

        } else {
            float leftX = 0;
            float leftY = leftX * k + d;
            float rightX = getWorkingAreaWidth();
            float rightY = rightX * k + d;
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, leftX, leftY, rightX, rightY);
        }
    }
}
