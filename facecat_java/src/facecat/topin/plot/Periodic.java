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
 * 周期线
 */
public class Periodic extends FCPlot {

    /**
     * 创建周期线
     */
    public Periodic() {
        setPlotType("PERIODIC");
    }

    /**
     * 周期
     */
    private int m_period = 5;

    /**
     * 初始移动周期
     */
    private int m_beginPeriod = 1;

    /**
     * 获取动作类型
     */
    @Override
    public ActionType getAction() {
        ActionType action = ActionType.NO;
        if (m_marks.isEmpty()) {
            return action;
        }
        // 获取点的位置
        FCPoint mp = getTouchOverPoint();
        // 获取参数
        double[] param = getPLParams(m_marks);
        float y = getWorkingAreaHeight() / 2;
        for (int i = 0; i < param.length; i++) {
            // 判断选中
            int rI = (int) param[i];
            float x1 = pX(rI);
            if (selectPoint(mp, x1, y)) {
                action = ActionType.AT1;
                m_marks.put(0, new PlotMark(0, m_dataSource.getXValue(rI), 0));
                m_beginPeriod = m_period;
                return action;
            }
            if (mp.x >= x1 - m_lineWidth * 2.5f && mp.x <= x1 + m_lineWidth * 2.5f) {
                action = ActionType.MOVE;
                return action;
            }
        }
        return action;
    }

    /**
     * 获取线条参数
     *
     * @param pList 点阵的集合
     * @returns 动作类型
     */
    private double[] getPLParams(java.util.HashMap<Integer, PlotMark> pList) {
        if (pList.isEmpty()) {
            return null;
        }
        double fValue = pList.get(0).getValue();
        int aIndex = m_dataSource.getRowIndex(pList.get(0).getKey());
        ArrayList<Double> fValueList = new ArrayList<Double>();
        FCChart chart = getChart();
        for (int i = chart.getFirstVisibleIndex(); i < chart.getLastVisibleIndex(); i++) {
            if (Math.abs(i - aIndex) % m_period == 0) {
                fValueList.add((double) i);
            }
        }
        int fValueListSize = fValueList.size();
        double fValueArray[] = new double[fValueListSize];
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
        int rIndex = m_dataSource.getRowsCount();
        if (rIndex > 0) {
            int currentIndex = getIndex(mp);
            double y = getNumberValue(mp);
            double date = m_dataSource.getXValue(currentIndex);
            m_marks.clear();
            m_marks.put(0, new PlotMark(0, date, y));
            FCChart chart = getChart();
            m_period = chart.getMaxVisibleRecord() / 10;
            if (m_period < 1) {
                m_period = 1;
            }
            return true;
        }
        return false;
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
     * 移动方法
     */
    @Override
    public void onMoving() {
        FCPoint mp = getMovingPoint();
        // 获取当前的索引和y值
        FCChart chart = getChart();
        int touchIndex = chart.getTouchOverIndex();
        if (touchIndex < 0) {
            touchIndex = 0;
        }
        if (touchIndex > chart.getLastVisibleIndex()) {
            touchIndex = chart.getLastVisibleIndex();
        }
        int bI = getIndex(m_startPoint);
        int eI = getIndex(mp);
        // 根据不同类型作出动作
        switch (m_action) {
            case MOVE:
                move(mp);
                break;
            case AT1:
                m_period = m_beginPeriod + (eI - bI);
                if (m_period < 1) {
                    m_period = 1;
                }
                break;
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
        // 获取参数
        double[] param = getPLParams(pList);
        for (int i = 0; i < param.length; i++) {
            int rI = (int) param[i];
            float x1 = pX(rI);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, 0, x1, getWorkingAreaHeight());
            if (isSelected()) {
                drawSelect(paint, lineColor, x1, getWorkingAreaHeight() / 2);
            }
        }
    }
}
