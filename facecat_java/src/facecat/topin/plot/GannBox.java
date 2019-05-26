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
 * 甘氏箱
 */
public class GannBox extends FCPlot {

    /**
     * 创建甘氏箱
     */
    public GannBox() {
        setPlotType("GANNBOX");
    }

    /**
     * 对面点的坐标
     */
    private FCPoint oppositePoint = new FCPoint();

    /**
     * 获取动作类型
     */
    @Override
    public ActionType getAction() {
        ActionType action = ActionType.NO;
        if (m_marks.isEmpty()) {
            return action;
        }
        // 获取动作类型
        return getClickStatus();
    }

    /**
     * 获取选中状态
     */
    private ActionType getClickStatus() {
        FCPoint mp = getTouchOverPoint();
        // 获取各个点的位置
        FCRect rect = getRectangle(m_marks.get(0), m_marks.get(1));
        int x1 = rect.left;
        int y1 = rect.top;
        int x2 = rect.right;
        int y2 = rect.top;
        int x3 = rect.left;
        int y3 = rect.bottom;
        int x4 = rect.right;
        int y4 = rect.bottom;
        // 判断是否选中点
        if (selectPoint(mp, x1, y1)) {
            return ActionType.AT1;
        } else if (selectPoint(mp, x2, y2)) {
            return ActionType.AT2;
        } else if (selectPoint(mp, x3, y3)) {
            return ActionType.AT3;
        } else if (selectPoint(mp, x4, y4)) {
            return ActionType.AT4;
        } else {
            int sub = (int) (m_lineWidth * 2.5);
            // 判断是否移动
            FCRect bigRect = new FCRect(rect.left - sub, rect.top - sub, rect.right + sub, rect.bottom + sub);
            if (mp.x >= bigRect.left && mp.x <= bigRect.right && mp.y >= bigRect.top && mp.y <= bigRect.bottom) {
                if (rect.right - rect.left <= 4 || rect.bottom - rect.top <= 4) {
                    return ActionType.MOVE;
                } else {
                    FCRect smallRect = new FCRect(rect.left + sub, rect.top + sub, rect.right - sub, rect.bottom - sub);
                    if (!(mp.x >= smallRect.left && mp.x <= smallRect.right && mp.y >= smallRect.top && mp.y <= smallRect.bottom)) {
                        return ActionType.MOVE;
                    }
                }
                x1 = rect.left;
                y1 = rect.bottom;
                x2 = rect.right;
                y2 = rect.top;
                FCPoint startP = new FCPoint(x1, y1);
                FCPoint[] listP = getGannBoxPoints(x1, y1, x2, y2);
                boolean selected;
                for (int i = 0; i < listP.length; i++) {
                    selected = selectLine(mp, startP.x, startP.y, listP[i].x, listP[i].y);
                    if (selected) {
                        return ActionType.MOVE;
                    }
                }
                selected = selectLine(mp, startP.x, startP.y, x2, y2);
                if (selected) {
                    return ActionType.MOVE;
                }
                x1 = rect.left;
                y1 = rect.top;
                x2 = rect.right;
                y2 = rect.bottom;
                listP = getGannBoxPoints(x1, y1, x2, y2);
                for (int i = 0; i < listP.length; i++) {
                    selected = selectLine(mp, startP.x, startP.y, listP[i].x, listP[i].y);
                    if (selected) {
                        return ActionType.MOVE;
                    }
                }
                startP = new FCPoint(x1, y1);
                selected = selectLine(mp, startP.x, startP.y, x2, y2);
                if (selected) {
                    return ActionType.MOVE;
                }
            }
        }
        return ActionType.NO;
    }

    /**
     * 获取江恩箱的重要点
     *
     * @param x1 第一个点的X
     * @param y1 第一个点的Y
     * @param x2 第二个点的X
     * @param y2 第二个点的Y
     * @return 江恩箱的重要点
     */
    private FCPoint[] getGannBoxPoints(float x1, float y1, float x2, float y2) {
        FCPoint firstP = new FCPoint(x2, y2 - (y2 - y1) * 0.875f);
        FCPoint secondP = new FCPoint(x2, y2 - (y2 - y1) * 0.75f);
        FCPoint thirdP = new FCPoint(x2, y2 - (y2 - y1) * 0.67f);
        FCPoint forthP = new FCPoint(x2, y2 - (y2 - y1) * 0.5f);
        FCPoint fifthP = new FCPoint(x2 - (x2 - x1) * 0.875f, y2);
        FCPoint sixthP = new FCPoint(x2 - (x2 - x1) * 0.75f, y2);
        FCPoint seventhP = new FCPoint(x2 - (x2 - x1) * 0.67f, y2);
        FCPoint eighthP = new FCPoint(x2 - (x2 - x1) * 0.5f, y2);
        return new FCPoint[]{firstP, secondP, thirdP, forthP, fifthP, sixthP, seventhP, eighthP};
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
        m_moveTimes++;
        m_action = getAction();
        FCRect rect = getRectangle(m_marks.get(0), m_marks.get(1));
        int x1 = rect.left;
        int y1 = rect.top;
        int x2 = rect.right;
        int y2 = rect.top;
        int x3 = rect.left;
        int y3 = rect.bottom;
        int x4 = rect.right;
        int y4 = rect.bottom;
        // 根据不同类型初始化点阵信息
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
        m_startMarks.clear();
        m_startPoint = getTouchOverPoint();
        if (m_action != ActionType.NO) {
            m_startMarks.put(0, m_marks.get(0));
            m_startMarks.put(1, m_marks.get(1));
        }
    }

    /**
     * 移动线条
     */
    @Override
    public void onMoving() {
        FCPoint mp = getMovingPoint();
        // 根据不同类型作出动作
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
        FCRect rect = getRectangle(pList.get(0), pList.get(1));
        int leftVScaleWidth = getChart().getLeftVScaleWidth();
        int titleHeight = getDiv().getTitleBar().getHeight();
        paint.setClip(new FCRect(rect.left + leftVScaleWidth, rect.top + titleHeight, rect.right + leftVScaleWidth, rect.bottom + titleHeight));
        // 画矩形
        if (rect.right - rect.left > 0 && rect.bottom - rect.top > 0) {
            drawRect(paint, lineColor, m_lineWidth, m_lineStyle, rect.left, rect.top, rect.right, rect.bottom);
        }
        FCPoint oP = new FCPoint(rect.left, rect.top);
        int x1 = rect.left;
        int y1 = rect.bottom;
        int x2 = rect.right;
        int y2 = rect.top;
        if (x1 != x2 && y1 != y2) {
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
            FCPoint startP = new FCPoint(x1, y1);
            FCPoint[] listP = getGannBoxPoints(x1, y1, x2, y2);
            float k = 0;
            float b = 0;
            for (int i = 0; i < listP.length; i++) {
                RefObject<Float> tempRef_k = new RefObject<Float>(k);
                RefObject<Float> tempRef_b = new RefObject<Float>(b);
                lineXY(startP.x, startP.y, listP[i].x, listP[i].y, 0, 0, tempRef_k, tempRef_b);
                k = tempRef_k.argvalue;
                b = tempRef_b.argvalue;
                float newX = 0;
                float newY = 0;
                if (x2 > x1) {
                    newY = k * x2 + b;
                    newX = x2;
                } else {
                    newY = b;
                    newX = x1;
                }
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, startP.x, startP.y, newX, newY);
            }
            x1 = rect.left;
            y1 = rect.top;
            x2 = rect.right;
            y2 = rect.bottom;
            // 获取重要点
            listP = getGannBoxPoints(x1, y1, x2, y2);
            startP = new FCPoint(x1, y1);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
            for (int i = 0; i < listP.length; i++) {
                RefObject<Float> tempRef_k2 = new RefObject<Float>(k);
                RefObject<Float> tempRef_b2 = new RefObject<Float>(b);
                // 获取直线参数
                lineXY(startP.x, startP.y, listP[i].x, listP[i].y, 0, 0, tempRef_k2, tempRef_b2);
                k = tempRef_k2.argvalue;
                b = tempRef_b2.argvalue;
                float newX = 0;
                float newY = 0;
                if (x2 > x1) {
                    newY = k * x2 + b;
                    newX = x2;
                } else {
                    newY = b;
                    newX = x1;
                }
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, startP.x, startP.y, newX, newY);
            }
        }
        // 画选中点
        if (isSelected()) {
            drawSelect(paint, lineColor, rect.left, rect.top);
            drawSelect(paint, lineColor, rect.right, rect.top);
            drawSelect(paint, lineColor, rect.left, rect.bottom);
            drawSelect(paint, lineColor, rect.right, rect.bottom);
        }
    }
}
