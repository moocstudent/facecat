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
 * 上涨箭头
 */
public class UpArrow extends FCPlot {

    /**
     * 创建上涨箭头
     */
    public UpArrow() {
        setPlotType("UPARROW");
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
        // 获取值和索引
        double fValue = m_marks.get(0).getValue();
        int aIndex = m_dataSource.getRowIndex(m_marks.get(0).getKey());
        // 获取横坐标和纵坐标
        float x1 = pX(aIndex);
        float y1 = pY(fValue);
        // 获取点击区域
        int width = 10;
        FCRect rect = new FCRect(x1 - width / 2, y1, x1 + width / 2, y1 + width * 3 / 2);
        FCPoint mp = getTouchOverPoint();
        // 判断是否选中
        if (mp.x > rect.left && mp.x <= rect.right && mp.y >= rect.top && mp.y <= rect.bottom) {
            action = ActionType.MOVE;
        }
        return action;
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
        double fValue = pList.get(0).getValue();
        int aIndex = m_dataSource.getRowIndex(pList.get(0).getKey());
        int x1 = (int) pX(aIndex);
        int y1 = (int) pY(fValue);
        int width = 10;
        FCPoint point1 = new FCPoint(x1, y1);
        FCPoint point2 = new FCPoint(x1 + width / 2, y1 + width);
        FCPoint point3 = new FCPoint(x1 + width / 4, y1 + width);
        FCPoint point4 = new FCPoint(x1 + width / 4, y1 + width * 3 / 2);
        FCPoint point5 = new FCPoint(x1 - width / 4, y1 + width * 3 / 2);
        FCPoint point6 = new FCPoint(x1 - width / 4, y1 + width);
        FCPoint point7 = new FCPoint(x1 - width / 2, y1 + width);
        FCPoint[] points = new FCPoint[]{point1, point2, point3, point4, point5, point6, point7};
        fillPolygon(paint, lineColor, points);
        // 画选中点
        if (isSelected()) {
            drawSelect(paint, lineColor, x1 - width / 2, y1);
        }
    }
}
