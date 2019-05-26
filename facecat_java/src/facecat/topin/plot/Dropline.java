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
 * 下降45度线
 */
public class Dropline extends FCPlot {

    /**
     * 创建下降45度线
     */
    public Dropline() {
        setPlotType("DROPLINE");
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
        /**
         * 获取参数
         */
        FCPoint mp = getTouchOverPoint();
        /**
         * 判断选中
         */
        float[] param = getDropLineParams(m_marks);
        if (param != null) {
            if (mp.y - param[0] * mp.x - param[1] >= m_lineWidth * -5 && mp.y - param[0] * mp.x - param[1] <= m_lineWidth * 5) {
                action = ActionType.MOVE;
            }
        }
        return action;
    }

    /**
     * 获取直线的参数
     *
     * @param pList 点阵集合
     */
    private float[] getDropLineParams(java.util.HashMap<Integer, PlotMark> pList) {
        if (pList.isEmpty()) {
            return null;
        }
        float y1 = pY(pList.get(0).getValue());
        int bIndex = m_dataSource.getRowIndex(pList.get(0).getKey());
        float x1 = pX(bIndex);
        float a = 1;
        float b = y1 - x1;
        return new float[]{a, b};
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
        // 获取参数
        float[] param = getDropLineParams(pList);
        float a = param[0];
        float b = param[1];
        float leftX = 0;
        float leftY = leftX * a + b;
        float rightX = getWorkingAreaWidth();
        float rightY = rightX * a + b;
        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, leftX, leftY, rightX, rightY);
    }
}
