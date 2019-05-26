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
 * 斐波周期线
 */
public class FiboTimezone extends FCPlot {

    /**
     * 创建斐波周期线
     */
    public FiboTimezone() {
        setPlotType("FIBOTIMEZONE");
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
        int[] param = getFibonacciTimeZonesParam(m_marks);
        for (int i = 0; i < param.length; i++) {
            // 判断选中
            int rI = (int) param[i];
            FCChart chart = getChart();
            if (rI >= chart.getFirstVisibleIndex() && rI <= chart.getLastVisibleIndex()) {
                float x1 = pX(rI);
                if (mp.x >= x1 - m_lineWidth * 2.5f && mp.x <= x1 + m_lineWidth * 2.5f) {
                    action = ActionType.MOVE;
                    return action;
                }
            }
        }
        return action;
    }

    /**
     * 获取斐波周期线的参数
     *
     * @param pList 点阵描述
     * @returns 斐波周期线的参数
     */
    private int[] getFibonacciTimeZonesParam(java.util.HashMap<Integer, PlotMark> pList) {
        if (pList.isEmpty()) {
            return null;
        }
        double fValue = pList.get(0).getValue();
        int aIndex = m_dataSource.getRowIndex(pList.get(0).getKey());
        int pos = 1;
        int fibonacciValue = 1;
        ArrayList<Integer> fValueList = new ArrayList<Integer>();
        fValueList.add(aIndex);
        FCChart chart = getChart();
        while (aIndex + fibonacciValue <= chart.getLastVisibleIndex()) {
            fibonacciValue = FCScript.fibonacciValue(pos);
            fValueList.add(aIndex + fibonacciValue);
            pos++;
        }
        int fValueListSize = fValueList.size();
        int fValueArray[] = new int[fValueListSize];
        for (int i = 0; i < fValueListSize; i++) {
            fValueArray[i] = fValueList.get(i);
        }
        return fValueArray;
    }

    /**
     * 初始化线条
     *
     * @param mp 坐标
     * @returns 是否初始化成功
     */
    @Override
    public boolean onCreate(FCPoint mp) {
        return createPoint(mp);
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
        int[] param = getFibonacciTimeZonesParam(pList);
        FCChart chart = getChart();
        for (int i = 0; i < param.length; i++) {
            int rI = (int) param[i];
            if (rI >= chart.getFirstVisibleIndex() && rI <= chart.getLastVisibleIndex()) {
                float x1 = pX(rI);
                // 画线
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, 0, x1, getWorkingAreaHeight());
                // 画选中
                if (i == 0 && isSelected()) {
                    drawSelect(paint, lineColor, x1, getWorkingAreaHeight() / 2);
                }
                // 画文字
                drawText(paint, (new Integer(rI)).toString(), lineColor, m_font, x1, 0);
            }
        }
    }
}
