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
 * 黄金分割目标线
 */
public class Ga extends Triangle {

    /**
     * 创建黄金分割目标线
     */
    public Ga() {
        setPlotType("GA");
    }

    /**
     * 获取动作类型
     */
    @Override
    public ActionType getAction() {
        ActionType action = super.getAction();
        if (action != ActionType.NO) {
            return action;
        } else {
            // 选中横线
            if (hLinesSelect(getGoldenRatioAimParams(m_marks), 6)) {
                action = ActionType.MOVE;
            }
            return action;
        }
    }

    /**
     * 获取黄金分割的直线参数
     */
    private float[] getGoldenRatioAimParams(java.util.HashMap<Integer, PlotMark> pList) {
        // 获取相对位置
        double baseValue = pList.get(0).getValue();
        if (pList.get(1).getValue() >= pList.get(2).getValue()) {
            return goldenRatioParams(baseValue, baseValue + pList.get(1).getValue() - pList.get(2).getValue());
        } else {
            return goldenRatioParams(baseValue + pList.get(1).getValue() - pList.get(2).getValue(), baseValue);
        }
    }

    /**
     * 开始移动画线工具
    @Override
    public void onMoveStart() {
        m_action = getAction();
        m_startMarks.clear();
        m_startPoint = getTouchOverPoint();
        m_startMarks.put(0, m_marks.get(0));
        m_startMarks.put(1, m_marks.get(1));
        m_startMarks.put(2, m_marks.get(2));
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
        // 获取相对位置
        float y1 = pY(pList.get(0).getValue());
        float y2 = pY(pList.get(1).getValue());
        float y3 = pY(pList.get(2).getValue());
        int aIndex = m_dataSource.getRowIndex(pList.get(0).getKey());
        int bIndex = m_dataSource.getRowIndex(pList.get(1).getKey());
        int cIndex = m_dataSource.getRowIndex(pList.get(2).getKey());
        float x1 = pX(aIndex);
        float x2 = pX(bIndex);
        float x3 = pX(cIndex);
        // 画线
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x2, y2, x3, y3);
        // 获取直线参数
        float[] lineParam = getGoldenRatioAimParams(pList);
        String[] str = new String[]{"0.00%", "23.60%", "38.20%", "50.00%", "61.80%", "100.00%"};
        // 画文字和线
        for (int i = 0; i < lineParam.length; i++) {
            FCSize sizeF = textSize(paint, str[i], m_font);
            float yP = lineParam[i];
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, 0, yP, getWorkingAreaWidth(), yP);
            drawText(paint, str[i], lineColor, m_font, getWorkingAreaWidth() - sizeF.cx, yP - sizeF.cy);
        }
        // 画选中点
        if (isSelected() || (x1 == x2 && x2 == x3 && y1 == y2 && y2 == y3)) {
            drawSelect(paint, lineColor, x1, y1);
            drawSelect(paint, lineColor, x2, y2);
            drawSelect(paint, lineColor, x3, y3);
        }
    }
}
