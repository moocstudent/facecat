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

/*
* 控件型单元格
*/
public class FCGridControlCell extends FCGridCell implements FCTouchEvent {

    public FCGridControlCell() {
    }

    protected FCView m_control = null;

    public FCView getControl() {
        return m_control;
    }

    public void setControl(FCView value) {
        m_control = value;
    }

    @Override
    public String getPaintText() {
        return "";
    }

    @Override
    public String getString() {
        if (m_control != null) {
            return m_control.getText();
        } else {
            return "";
        }
    }

    public void callControlTouchEvent(int eventID, Object sender, FCTouchInfo touchInfo) {
        if (sender == m_control) {
            if (eventID == FCEventID.TOUCHDOWN) {
                onControlTouchDown(touchInfo.clone());
            } else if (eventID == FCEventID.TOUCHMOVE) {
                onControlTouchMove(touchInfo.clone());
            } else if (eventID == FCEventID.TOUCHUP) {
                onControlTouchUp(touchInfo.clone());
            }
        }
    }

    @Override
    public void delete() {
        if (!m_isDeleted) {
            if (m_control != null) {
                m_control.delete();
                m_control = null;
            }
            m_isDeleted = true;
        }
    }

    @Override
    public void onadd() {
        FCGrid grid = getGrid();
        if (m_control != null && grid != null) {
            grid.addControl(m_control);
            m_control.addEvent(this, FCEventID.TOUCHDOWN);
            m_control.addEvent(this, FCEventID.TOUCHMOVE);
            m_control.addEvent(this, FCEventID.TOUCHUP);
        }
    }

    public void onControlTouchDown(FCTouchInfo touchInfo) {
        FCGrid grid = getGrid();
        if (m_control != null && grid != null) {
            FCTouchInfo newTouchInfo = touchInfo.clone();
            newTouchInfo.m_firstPoint = grid.pointToControl(m_control.pointToNative(touchInfo.m_firstPoint));
            newTouchInfo.m_secondPoint = grid.pointToControl(m_control.pointToNative(touchInfo.m_secondPoint));
            grid.onTouchDown(newTouchInfo);
        }
    }

    public void onControlTouchMove(FCTouchInfo touchInfo) {
        FCGrid grid = getGrid();
        if (m_control != null && grid != null) {
            FCTouchInfo newTouchInfo = touchInfo.clone();
            newTouchInfo.m_firstPoint = grid.pointToControl(m_control.pointToNative(touchInfo.m_firstPoint));
            newTouchInfo.m_secondPoint = grid.pointToControl(m_control.pointToNative(touchInfo.m_secondPoint));
            grid.onTouchMove(newTouchInfo);
        }
    }

    public void onControlTouchUp(FCTouchInfo touchInfo) {
        FCGrid grid = getGrid();
        if (m_control != null && grid != null) {
            FCTouchInfo newTouchInfo = touchInfo.clone();
            newTouchInfo.m_firstPoint = grid.pointToControl(m_control.pointToNative(touchInfo.m_firstPoint));
            newTouchInfo.m_secondPoint = grid.pointToControl(m_control.pointToNative(touchInfo.m_secondPoint));
            grid.onTouchUp(newTouchInfo);
        }
    }

    @Override
    public void onPaint(FCPaint paint, FCRect rect, FCRect clipRect, boolean isAlternate) {
        super.onPaint(paint, rect, clipRect, isAlternate);
        onPaintControl(paint, rect, clipRect);
    }

    public void onPaintControl(FCPaint paint, FCRect rect, FCRect clipRect) {
        if (m_control != null) {
            FCRect bounds = new FCRect(rect.left + 1, rect.top + 1, rect.right - 1, rect.bottom - 1);
            m_control.setBounds(bounds);
            FCRect region = clipRect.clone();
            region.left -= rect.left;
            region.top -= rect.top;
            region.right -= rect.left;
            region.bottom -= rect.top;
            m_control.setRegion(region);
        }
    }

    @Override
    public void onremove() {
        FCGrid grid = getGrid();
        if (m_control != null && grid != null) {
            m_control.removeEvent(this, FCEventID.TOUCHDOWN);
            m_control.removeEvent(this, FCEventID.TOUCHMOVE);
            m_control.removeEvent(this, FCEventID.TOUCHUP);
            grid.removeControl(m_control);
        }
    }

    @Override
    public void setString(String value) {
        if (m_control != null) {
            m_control.setText(value);
        }
    }
}
