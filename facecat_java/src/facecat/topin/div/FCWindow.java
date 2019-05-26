/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.div;

import facecat.topin.core.*;
import java.util.*;

/**
 * 窗体控件
 */
public class FCWindow extends FCView {

    /**
     * 创建窗体控件
     */
    public FCWindow() {
        setAllowDrag(true);
        setIsWindow(true);
        setVisible(false);
    }

    /**
     * 调整尺寸的点
     */
    protected int m_resizePoint = -1;

    /**
     * 移动开始点
     */
    protected FCPoint m_startTouchPoint = new FCPoint();

    /**
     * 移动开始时的控件矩形
     */
    protected FCRect m_startRect = new FCRect();

    protected int m_borderWidth = 2;

    /**
     * 获取边框的宽度
     */
    public int getBorderWidth() {
        return m_borderWidth;
    }

    /**
     * 设置边框的宽度
     */
    public void setBorderWidth(int value) {
        m_borderWidth = value;
    }

    protected boolean m_canResize = false;

    /**
     * 获取是否可以调整尺寸
     */
    public boolean canresize() {
        return m_canResize;
    }

    /**
     * 设置是否可以调整尺寸
     */
    public void setcanresize(boolean value) {
        m_canResize = value;
    }

    protected int m_captionHeight = 20;

    /**
     * 获取标题栏的高度
     */
    public int getCaptionHeight() {
        return m_captionHeight;
    }

    /**
     * 设置标题栏的高度
     */
    public void setCaptionHeight(int value) {
        m_captionHeight = value;
    }

    /**
     * 获取客户端的区域
     */
    public FCRect getClientSize() {
        int width = getWidth(), height = getHeight() - m_captionHeight;
        if (height < 0) {
            height = 0;
        }
        return new FCRect(0, m_captionHeight, width, height);
    }

    protected FCWindowFrame m_frame = null;

    /**
     * 获取窗体的
     */
    public FCWindowFrame getFrame() {
        return m_frame;
    }

    /**
     * 设置窗体的
     */
    public void setFrame(FCWindowFrame value) {
        m_frame = value;
    }

    protected boolean m_isDialog;

    /**
     * 获取或设置是否会话窗口
     */
    public boolean isDialog() {
        return m_isDialog;
    }

    protected long m_shadowColor = FCColor.argb(25, 255, 255, 255);

    /**
     * 获取阴影的颜色
     */
    public long getShadowColor() {
        return m_shadowColor;
    }

    /**
     * 设置阴影的颜色
     */
    public void setShadowColor(long value) {
        m_shadowColor = value;
    }

    protected int m_shadowSize = 10;

    /**
     * 获取阴影的大小
     */
    public int getShadowSize() {
        return m_shadowSize;
    }

    /**
     * 设置阴影的大小
     */
    public void setShadowSize(int value) {
        m_shadowSize = value;
    }

    /**
     * 将控件放到最前显示
     */
    @Override
    public void bringToFront() {
        super.bringToFront();
        if (m_frame != null) {
            m_frame.bringToFront();
        }
    }

    /**
     * 调用事件
     *
     * @param eventID 事件ID
     * @param cancel 是否退出
     */
    protected void callWindowClosingEvents(int eventID, RefObject<Boolean> cancel) {
        if (m_events != null && m_events.containsKey(eventID)) {
            ArrayList<Object> events = m_events.get(eventID);
            int eventSize = events.size();
            for (int i = 0; i < eventSize; i++) {
                FCWindowClosingEvent func = (FCWindowClosingEvent) ((events.get(i) instanceof FCWindowClosingEvent) ? events.get(i) : null);
                if (func != null) {
                    func.callWindowClosingEvent(eventID, this, cancel);
                }
            }
        }
    }

    /**
     * 关闭窗体
     */
    public void close() {
        boolean cancel = false;
        RefObject<Boolean> tempRef_cancel = new RefObject<Boolean>(cancel);
        onWindowClosing(tempRef_cancel);
        cancel = tempRef_cancel.argvalue;
        if (!cancel) {
            if (m_frame != null) {
                m_frame.removeControl(this);
                getNative().removeControl(m_frame);
                m_frame.delete();
                m_frame = null;
                setParent(null);
            } else {
                getNative().removeControl(this);
            }
            onWindowClosed();
        }
    }

    /**
     * 销毁控件方法
     */
    @Override
    public void delete() {
        if (!isDeleted()) {
            if (m_frame != null) {
                m_frame.removeControl(this);
                getNative().removeControl(m_frame);
                m_frame.delete();
                m_frame = null;
            }
        }
        super.delete();
    }

    /**
     * 获取控件类型
     */
    @Override
    public String getControlType() {
        return "Window";
    }

    /**
     * 获取动态绘图区域
     */
    public FCRect getDynamicPaintRect() {
        FCSize oldSize = m_oldSize;
        if (oldSize.cx == 0 && oldSize.cy == 0) {
            oldSize = getSize();
        }
        FCRect oldRect = new FCRect(m_oldLocation.x, m_oldLocation.y, m_oldLocation.x + oldSize.cx, m_oldLocation.y + oldSize.cy);
        FCRect rect = new FCRect(m_location.x, m_location.y, m_location.x + getWidth(), m_location.y + getHeight());
        FCRect paintRect = new FCRect(Math.min(oldRect.left, rect.left) - m_shadowSize - 10,
                Math.min(oldRect.top, rect.top) - m_shadowSize - 10,
                Math.max(oldRect.right, rect.right) + m_shadowSize + 10,
                Math.max(oldRect.bottom, rect.bottom) + m_shadowSize + 10);
        return paintRect;
    }

    /**
     * 获取属性值
     *
     * @param name 属性名称
     * @param value 返回属性值
     * @param type 返回属性类型
     */
    @Override
    public void getProperty(String name, RefObject<String> value, RefObject<String> type) {
        if (name.equals("borderwidth")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getBorderWidth());
        } else if (name.equals("canresize")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(canresize());
        } else if (name.equals("captionheight")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getCaptionHeight());
        } else if (name.equals("shadowcolor")) {
            type.argvalue = "color";
            value.argvalue = FCStr.convertColorToStr(getShadowColor());
        } else if (name.equals("shadowsize")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getShadowSize());
        } else {
            super.getProperty(name, value, type);
        }
    }

    /**
     * 获取属性列表
     */
    @Override
    public ArrayList<String> getPropertyNames() {
        ArrayList<String> propertyNames = super.getPropertyNames();
        propertyNames.addAll(Arrays.asList(new String[]{"BorderWidth", "CanResize", "CaptionHeight", "ShadowColor", "ShadowSize"}));
        return propertyNames;
    }

    /**
     * 获取触摸状态
     *
     * @param state 状态值
     * @returns 触摸状态
     */
    protected FCRect[] getResizePoints() {
        int width = getWidth(), height = getHeight();
        FCRect[] points = new FCRect[8];

        points[0] = new FCRect(0, 0, m_borderWidth * 2, m_borderWidth * 2);

        points[1] = new FCRect(0, height - m_borderWidth * 2, m_borderWidth * 2, height);

        points[2] = new FCRect(width - m_borderWidth * 2, 0, width, m_borderWidth * 2);

        points[3] = new FCRect(width - m_borderWidth * 2, height - m_borderWidth * 2, width, height);

        points[4] = new FCRect(0, 0, m_borderWidth, height);

        points[5] = new FCRect(0, 0, width, m_borderWidth);

        points[6] = new FCRect(width - m_borderWidth, 0, width, height);

        points[7] = new FCRect(0, height - m_borderWidth, width, height);
        return points;
    }

    /**
     * 获取调整尺寸的状态
     */
    protected int getResizeState() {
        FCPoint mp = getTouchPoint();
        FCRect[] pRects = getResizePoints();
        int rsize = pRects.length;
        for (int i = 0; i < rsize; i++) {
            FCRect rect = pRects[i];
            if (mp.x >= rect.left && mp.x <= rect.right && mp.y >= rect.top && mp.y <= rect.bottom) {
                return i;
            }
        }
        return -1;
    }

    /**
     * 滚动开始方法
     */
    @Override
    public boolean onDragBegin() {
        FCPoint mp = getTouchPoint();
        int width = getWidth(), height = getHeight();
        if (mp.y > m_captionHeight) {
            return false;
        }
        if (m_resizePoint != -1) {
            return false;
        }
        return super.onDragBegin();
    }

    /**
     * 拖动准备方法
     *
     * @param startOffset 可以拖动的偏移坐标量
     */
    @Override
    public void onDragReady(RefObject<FCPoint> startOffset) {
        startOffset.argvalue.x = 0;
        startOffset.argvalue.y = 0;
    }

    /**
     * 触摸按下方法
     *
     * @param touchInfo 触摸信息
     */
    @Override
    public void onTouchDown(FCTouchInfo touchInfo) {
        super.onTouchDown(touchInfo.clone());
        if (touchInfo.m_firstTouch && touchInfo.m_clicks == 1) {
            if (m_canResize) {
                m_resizePoint = getResizeState();
                m_startTouchPoint = getNative().getTouchPoint();
                m_startRect = getBounds();
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
        super.onTouchMove(touchInfo.clone());
        if (m_canResize) {
            FCPoint nowPoint = getNative().getTouchPoint();
            if (m_resizePoint != -1) {
                int left = m_startRect.left, top = m_startRect.top, right = m_startRect.right, bottom = m_startRect.bottom;
                switch (m_resizePoint) {
                    case 0:
                        left = left + nowPoint.x - m_startTouchPoint.x;
                        top = top + nowPoint.y - m_startTouchPoint.y;
                        break;
                    case 1:
                        left = left + nowPoint.x - m_startTouchPoint.x;
                        bottom = bottom + nowPoint.y - m_startTouchPoint.y;
                        break;
                    case 2:
                        right = right + nowPoint.x - m_startTouchPoint.x;
                        top = top + nowPoint.y - m_startTouchPoint.y;
                        break;
                    case 3:
                        right = right + nowPoint.x - m_startTouchPoint.x;
                        bottom = bottom + nowPoint.y - m_startTouchPoint.y;
                        break;
                    case 4:
                        left = left + nowPoint.x - m_startTouchPoint.x;
                        break;
                    case 5:
                        top = top + nowPoint.y - m_startTouchPoint.y;
                        break;
                    case 6:
                        right = right + nowPoint.x - m_startTouchPoint.x;
                        break;
                    case 7:
                        bottom = bottom + nowPoint.y - m_startTouchPoint.y;
                        break;
                }
                FCRect bounds = new FCRect(left, top, right, bottom);
                setBounds(bounds);
                getNative().invalidate();
            }
        }
    }

    /**
     * 触摸抬起方法
     *
     * @param touchInfo 触摸信息
     */
    @Override
    public void onTouchUp(FCTouchInfo touchInfo) {
        super.onTouchUp(touchInfo.clone());
        m_resizePoint = -1;
        invalidate();
    }

    /**
     * 绘制前景方法
     *
     * @param paint 绘图对象
     * @param clipRect 裁剪区域
     */
    @Override
    public void onPaintForeground(FCPaint paint, FCRect clipRect) {
        String text = getText();
        if (text != null && text.length() > 0) {
            int width = getWidth();
            FCFont font = getFont();
            FCSize tSize = paint.textSize(text, font);
            FCPoint strPoint = new FCPoint();
            strPoint.x = 5;
            strPoint.y = (m_captionHeight - tSize.cy) / 2;
            FCRect tRect = new FCRect(strPoint.x, strPoint.y, strPoint.x + tSize.cx, strPoint.y + tSize.cy);
            paint.drawText(text, getPaintingTextColor(), font, tRect);
        }
    }

    /**
     * 可见状态改变方法
     */
    @Override
    public void onVisibleChanged() {
        super.onVisibleChanged();
        FCNative inative = getNative();
        if (inative != null) {
            if (isVisible()) {
                if (m_frame == null) {
                    m_frame = new FCWindowFrame();
                }
                inative.removeControl(this);
                inative.addControl(m_frame);
                m_frame.setSize(inative.getDisplaySize());
                if (!m_frame.containsControl(this)) {
                    m_frame.addControl(this);
                }
            } else {
                if (m_frame != null) {
                    m_frame.removeControl(this);
                    inative.removeControl(m_frame);
                }
            }
        }
    }

    /**
     * 窗体正在关闭方法
     *
     * @param cancel 是否退出
     */
    public void onWindowClosing(RefObject<Boolean> cancel) {
        callWindowClosingEvents(FCEventID.WINDOWCLOSING, cancel);
    }

    /**
     * 窗体关闭方法
     */
    public void onWindowClosed() {
        callEvents(FCEventID.WINDOWCLOSED);
    }

    /**
     * 将控件放到最下面显示
     */
    @Override
    public void sendToBack() {
        super.sendToBack();
        if (m_frame != null) {
            m_frame.sendToBack();
        }
    }

    /**
     * 设置属性
     *
     * @param name 属性名称
     * @param value 属性值
     */
    @Override
    public void setProperty(String name, String value) {
        if (name.equals("borderwidth")) {
            setBorderWidth(FCStr.convertStrToInt(value));
        } else if (name.equals("canresize")) {
            setcanresize(FCStr.convertStrToBool(value));
        } else if (name.equals("captionheight")) {
            setCaptionHeight(FCStr.convertStrToInt(value));
        } else if (name.equals("shadowcolor")) {
            setShadowColor(FCStr.convertStrToColor(value));
        } else if (name.equals("shadowsize")) {
            setShadowSize(FCStr.convertStrToInt(value));
        } else {
            super.setProperty(name, value);
        }
    }

    /**
     * 以会话方式显示
     */
    public void showDialog() {
        m_isDialog = true;
        show();
    }
}
