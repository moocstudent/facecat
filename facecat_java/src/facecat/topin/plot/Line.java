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
 * 直线
 */
public class Line extends FCPlot {

    /**
     * 创建直线
     */
    public Line() {
        setPlotType("LINE");
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
        int bIndex = m_dataSource.getRowIndex(m_marks.get(0).getKey());
        int eIndex = m_dataSource.getRowIndex(m_marks.get(1).getKey());
        float y1 = pY(m_marks.get(0).getValue());
        float y2 = pY(m_marks.get(1).getValue());
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        if (selectPoint(mp, x1, y1)) {
            action = ActionType.AT1;
        } else if (selectPoint(mp, x2, y2)) {
            action = ActionType.AT2;
        } else if (selectLine(mp, x1, y1, x2, y2)) {
            action = ActionType.MOVE;
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
        float y1 = pY(pList.get(0).getValue());
        float y2 = pY(pList.get(1).getValue());
        int bIndex = m_dataSource.getRowIndex(pList.get(0).getKey());
        int eIndex = m_dataSource.getRowIndex(pList.get(1).getKey());
        // 获取参数
        float[] param = getLineParams(pList.get(0), pList.get(1));
        float a = 0;
        float b = 0;
        float x1 = pX(bIndex);
        float x2 = pX(eIndex);
        if (param != null) {
            a = param[0];
            b = param[1];
            float leftX = 0;
            float leftY = leftX * a + b;
            float rightX = getWorkingAreaWidth();
            float rightY = rightX * a + b;
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, leftX, leftY, rightX, rightY);
        } else {
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, 0, x1, getWorkingAreaHeight());
        }
        // 画选中点
        if (isSelected()) {
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x2, y2);
        }
    }
}
