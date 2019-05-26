/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.grid;

import facecat.topin.core.*;
import java.util.*;

/**
 * 表格带列
 */
public class FCBandedGridColumn extends FCGridColumn {

    /**
     * 创建表格带列
     */
    public FCBandedGridColumn() {
    }

    protected FCGridBand m_band = null;

    /**
     * 获取表格带
     */
    public FCGridBand getBand() {
        return m_band;
    }

    /**
     * 设置表格带
     */
    public void setBand(FCGridBand value) {
        m_band = value;
    }

    /**
     * 获取控件类型
     */
    @Override
    public String getControlType() {
        return "BandedGridColumn";
    }

    /**
     * 拖动开始方法
     */
    @Override
    public boolean onDragBegin() {
        return m_resizeState == 0;
    }

    /**
     * 触摸按下方法
     *
     * @param touchInfo 触摸信息
     */
    @Override
    public void onTouchDown(FCTouchInfo touchInfo) {
        callTouchEvents(FCEventID.TOUCHDOWN, touchInfo.clone());
        if (m_band != null) {
            if (touchInfo.m_firstTouch && touchInfo.m_clicks == 1) {
                FCPoint mp = touchInfo.m_firstPoint.clone();
                if (allowResize()) {
                    ArrayList<FCBandedGridColumn> bandColumns = m_band.getColumns();
                    int columnsSize = bandColumns.size();
                    int index = -1;
                    for (int i = 0; i < columnsSize; i++) {
                        if (this == bandColumns.get(i)) {
                            index = i;
                            break;
                        }
                    }
                    if (index > 0 && mp.x < 5) {
                        m_resizeState = 1;
                        m_beginWidth = bandColumns.get(index - 1).getWidth();
                    } else if (index < columnsSize - 1 && mp.x > getWidth() - 5) {
                        m_resizeState = 2;
                        m_beginWidth = getWidth();
                    }
                    m_touchDownPoint = getNative().getTouchPoint();
                }
            }
        }
        invalidate();
    }

    /**
     * 触摸移动方法
     *
     * @param touchInfo 触摸信息
     */
    @Override
    public void onTouchMove(FCTouchInfo touchInfo) {
        callTouchEvents(FCEventID.TOUCHMOVE, touchInfo.clone());
        FCGrid grid = getGrid();
        if (m_band != null && grid != null) {
            if (allowResize()) {
                ArrayList<FCBandedGridColumn> bandColumns = m_band.getColumns();
                int columnsSize = bandColumns.size();
                int index = -1;
                int width = getWidth();
                for (int i = 0; i < columnsSize; i++) {
                    if (this == bandColumns.get(i)) {
                        index = i;
                        break;
                    }
                }
                if (m_resizeState > 0) {
                    FCPoint curPoint = getNative().getTouchPoint();
                    int newWidth = m_beginWidth + (curPoint.x - m_touchDownPoint.x);
                    if (newWidth > 0) {
                        if (m_resizeState == 1) {
                            FCBandedGridColumn leftColumn = bandColumns.get(index - 1);
                            int leftWidth = leftColumn.getWidth();
                            leftColumn.setWidth(newWidth);
                            width += leftWidth - newWidth;
                            setWidth(width);
                        } else if (m_resizeState == 2) {
                            setWidth(newWidth);
                            FCBandedGridColumn rightColumn = bandColumns.get(index + 1);
                            int rightWidth = rightColumn.getWidth();
                            rightWidth += width - newWidth;
                            rightColumn.setWidth(rightWidth);
                        }
                    }
                    grid.invalidate();
                    return;
                }
            }
        }
    }
}
