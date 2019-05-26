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
 * 价格签条
 */
public class Price extends FCPlot {

    /**
     * 创建价格签条
     */
    public Price() {
        setPlotType("PRICE");
    }

    private FCSize m_textSize;

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
        double fValue = m_marks.get(0).getValue();
        int aIndex = m_dataSource.getRowIndex(m_marks.get(0).getKey());
        float x1 = pX(aIndex);
        float y1 = pY(fValue);
        FCRect rect = new FCRect(x1, y1, x1 + m_textSize.cx, y1 + m_textSize.cy);
        if (mp.x >= rect.left && mp.x <= rect.right && mp.y >= rect.top && mp.y <= rect.bottom) {
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
        int wX = getWorkingAreaWidth();
        int wY = getWorkingAreaHeight();
        if (wX > 0 && wY > 0) {
            // 获取相对位置
            double fValue = pList.get(0).getValue();
            int aIndex = m_dataSource.getRowIndex(pList.get(0).getKey());
            float x1 = pX(aIndex);
            float y1 = pY(fValue);
            FCChart chart = getChart();
            String word = FCStr.getValueByDigit(fValue, chart.getLeftVScaleWidth() > 0 ? m_div.getLeftVScale().getDigit() : m_div.getRightVScale().getDigit());
            // 画文字
            drawText(paint, word, lineColor, m_font, x1, y1);
            m_textSize = textSize(paint, word, m_font);
            if (isSelected()) {
                if (m_textSize.cx > 0 && m_textSize.cy > 0) {
                    drawRect(paint, lineColor, m_lineWidth, m_lineStyle, (int) x1, (int) y1, (int) x1 + m_textSize.cx, (int) y1 + m_textSize.cy);
                }
            }
        }
    }
}
