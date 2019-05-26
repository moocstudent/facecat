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
import java.util.*;

/**
 * 斐波扇面
 */
public class FiboFanline extends FCPlot {

    /**
     * 创建斐波扇面
     */
    public FiboFanline() {
        setPlotType("FIBOFANLINE");
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
        // 获取点的描述
        float y1 = pY(m_marks.get(0).getValue());
        float y2 = pY(m_marks.get(1).getValue());
        int bIndex = m_dataSource.getRowIndex(m_marks.get(0).getKey());
        int eIndex = m_dataSource.getRowIndex(m_marks.get(1).getKey());
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        // 缩放
        if (selectPoint(mp, x1, y1) || m_moveTimes == 1) {
            action = ActionType.AT1;
            return action;
        }
        // 缩放
        else if (selectPoint(mp, x2, y2)) {
            action = ActionType.AT2;
            return action;
        }
        // 判断选中点
        FCPointF firstP = new FCPointF(x2, y2 - (y2 - y1) * 0.382f);
        FCPointF secondP = new FCPointF(x2, y2 - (y2 - y1) * 0.5f);
        FCPointF thirdP = new FCPointF(x2, y2 - (y2 - y1) * 0.618f);
        FCPointF startP = new FCPointF(x1, y1);
        // 获取直线参数
        boolean selected = selectSegment(mp, x1, y1, x2, y2);
        if (selected) {
            action = ActionType.MOVE;
            return action;
        }
        if ((x2 > x1 && mp.x >= x1 - 2) || (mp.x <= x1 + 2 && x2 < x1)) {
            if (selectRay(mp, startP.x, startP.y, firstP.x, firstP.y) || selectRay(mp, startP.x, startP.y, secondP.x, secondP.y) || selectRay(mp, startP.x, startP.y, thirdP.x, thirdP.y)) {
                action = ActionType.MOVE;
                return action;
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
        // 获取点阵的值及索引，然后获取内部位置
        float y1 = pY(pList.get(0).getValue());
        float y2 = pY(pList.get(1).getValue());
        int bIndex = m_dataSource.getRowIndex(pList.get(0).getKey());
        int eIndex = m_dataSource.getRowIndex(pList.get(1).getKey());
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        drawLine(paint, lineColor, m_lineWidth, 1, x1, y1, x2, y2);
        // 画选中
        if (isSelected() || (x1 == x2)) {
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x2, y2);
        }
        if (x1 != x2 && y1 != y2) {
            FCPointF firstP = new FCPointF(x2, y2 - (y2 - y1) * 0.382f);
            FCPointF secondP = new FCPointF(x2, y2 - (y2 - y1) * 0.5f);
            FCPointF thirdP = new FCPointF(x2, y2 - (y2 - y1) * 0.618f);
            FCPointF startP = new FCPointF(x1, y1);
            ArrayList<FCPointF> listP = new ArrayList<FCPointF>();
            listP.addAll(Arrays.asList(new FCPointF[]{firstP, secondP, thirdP}));
            int listSize = listP.size();
            for (int i = 0; i < listSize; i++) {
                // 获取直线参数
                float k = 0;
                float b = 0;
                RefObject<Float> tempRef_k = new RefObject<Float>(k);
                RefObject<Float> tempRef_b = new RefObject<Float>(b);
                lineXY(startP.x, startP.y, listP.get(i).x, listP.get(i).y, 0, 0, tempRef_k, tempRef_b);
                k = tempRef_k.argvalue;
                b = tempRef_b.argvalue;
                float newX = 0;
                float newY = 0;
                if (x2 > x1) {
                    newY = k * getWorkingAreaWidth() + b;
                    newX = getWorkingAreaWidth();
                } else {
                    newY = b;
                    newX = 0;
                }
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, startP.x, startP.y, newX, newY);
            }
        }
    }
}
