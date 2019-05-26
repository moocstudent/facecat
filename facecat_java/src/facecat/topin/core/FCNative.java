/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.core;

import java.util.*;

/**
 * 公共类库
 */
public class FCNative {

    /**
     * 创建公共类库
     */
    public FCNative() {
    }

    protected void finalize() throws Throwable {
        delete();
    }

    /**
     * 控件
     */
    protected ArrayList<FCView> m_controls = new ArrayList<FCView>();

    /**
     * 拖动开始时的触摸位置
     */
    protected FCPoint m_dragBeginPoint = new FCPoint();

    /**
     * 拖动开始时的区域
     */
    protected FCRect m_dragBeginRect = new FCRect();

    /**
     * 可以开始拖动的偏移坐标量
     */
    protected FCPoint m_dragStartOffset = new FCPoint();

    /**
     * 正被拖动的控件
     */
    protected FCView m_draggingControl = null;

    /**
     * 正被触摸按下的控件
     */
    protected FCView m_touchDownControl = null;

    /**
     * 触摸按下时的坐标
     */
    protected FCPoint m_touchDownPoint = new FCPoint();

    /**
     * 触摸正在其上方移动的控件
     */
    protected FCView m_touchMoveControl = null;

    protected java.util.HashMap<Integer, FCView> m_timers = new java.util.HashMap<Integer, FCView>();

    protected boolean m_allowScaleSize = false;

    /**
     * 获取是否允许使用缩放尺寸
     */
    public boolean allowScaleSize() {
        return m_allowScaleSize;
    }

    /**
     * 设置是否允许使用缩放尺寸
     */
    public void setAllowScaleSize(boolean value) {
        m_allowScaleSize = value;
    }

    protected FCSize m_displaySize = new FCSize();

    /**
     * 获取显示区域
     */
    public FCSize getDisplaySize() {
        return m_displaySize;
    }

    /**
     * 设置显示区域
     */
    public void setDisplaySize(FCSize value) {
        m_displaySize = value;
    }

    protected FCView m_focusedControl = null;

    /**
     * 获取选中的控件
     */
    public FCView getFocusedControl() {
        return m_focusedControl;
    }

    /**
     * 设置选中的控件
     */
    public void setFocusedControl(FCView value) {
        if (m_focusedControl != value) {
            if (m_focusedControl != null) {
                FCView fControl = m_focusedControl;
                m_focusedControl = null;
                fControl.onLostfocus();
            }
            m_focusedControl = value;
            if (m_focusedControl != null) {
                m_focusedControl.onGotFocus();
            }
        }
    }

    protected FCHost m_host = null;

    /**
     * 获取控件管理器
     */
    public FCHost getHost() {
        return m_host;
    }

    /**
     * 设置控件管理器
     */
    public void setHost(FCHost value) {
        m_host = value;
    }

    /**
     * 获取触摸悬停的控件
     */
    public FCView getHoveredControl() {
        return m_touchMoveControl;
    }

    protected boolean m_isDeleted = false;

    /**
     * 获取或设置是否已被销毁
     */
    public boolean isDeleted() {
        return m_isDeleted;
    }

    /**
     * 获取触摸的实际位置
     */
    public FCPoint getTouchPoint() {
        if (m_host != null) {
            return m_host.getTouchPoint();
        }
        return new FCPoint(0, 0);
    }

    protected float m_opacity = 1;

    /**
     * 获取或设置透明度
     */
    public float getOpacity() {
        return m_opacity;
    }

    public void setOpacity(float value) {
        m_opacity = value;
    }

    protected FCPaint m_paint = null;

    /**
     * 获绘图类
     */
    public FCPaint getPaint() {
        return m_paint;
    }

    /**
     * 取绘图类
     */
    public void setPaint(FCPaint value) {
        m_paint = value;
    }

    /**
     * 获取触摸按住的控件
     */
    public FCView getPushedControl() {
        return m_touchDownControl;
    }

    protected String m_resourcePath;

    /**
     * 获取资源文件的路径
     */
    public String getResourcePath() {
        return m_resourcePath;
    }

    /**
     * 设置资源文件的路径
     */
    public void setResourcePath(String value) {
        m_resourcePath = value;
    }

    protected int m_rotateAngle;

    /**
     * 获取旋转角度
     */
    public int getRotateAngle() {
        return m_rotateAngle;
    }

    /**
     * 设置旋转角度
     */
    public void setRotateAngle(int value) {
        m_rotateAngle = value;
    }

    protected FCSize m_scaleSize = new FCSize();

    /**
     * 获取使用缩放尺寸
     */
    public FCSize getScaleSize() {
        return m_scaleSize.clone();
    }

    /**
     * 设置使用缩放尺寸
     */
    public void setScaleSize(FCSize value) {
        m_scaleSize = value.clone();
    }

    /**
     * 添加控件
     *
     * @param control 控件
     */
    public void addControl(FCView control) {
        control.setNative(this);
        m_controls.add(control);
        control.onAdd();
    }

    /**
     * 将控件放到最前显示
     *
     * @param control 控件
     */
    public void bringToFront(FCView control) {
        FCView parent = control.getParent();
        if (parent != null) {
            parent.bringChildToFront(control);
        } else {
            if (m_controls != null && m_controls.size() > 0) {
                m_controls.remove(control);
                m_controls.add(control);
            }
        }
    }

    /**
     * 退出拖动
     */
    public void cancelDragging() {
        if (m_draggingControl != null) {
            m_draggingControl.setBounds(m_dragBeginRect);
            FCView draggingControl = m_draggingControl;
            m_draggingControl = null;
            draggingControl.onDragEnd();
            FCView parent = draggingControl.getParent();
            if (parent != null) {
                parent.invalidate();
            } else {
                invalidate();
            }
        }
    }

    /**
     * 清除所有的控件
     */
    public void clearControls() {
        ArrayList<FCView> controls = new ArrayList<FCView>();
        for (FCView control : m_controls) {
            controls.add(control);
        }
        for (FCView control : controls) {
            control.onRemove();
            control.delete();
        }
        m_controls.clear();
    }

    /**
     * 获取控件的绝对横坐标
     *
     * @param control 控件
     */
    public int clientX(FCView control) {
        if (control != null) {
            FCView parent = control.getParent();
            int cLeft = control.getLeft();
            if (parent != null) {
                return cLeft - (control.displayOffset() ? parent.getDisplayOffset().x : 0) + clientX(parent);
            } else {
                return cLeft;
            }
        } else {
            return 0;
        }
    }

    /**
     * 获取控件的绝对纵坐标
     *
     * @param control 控件
     * @returns 坐标
     */
    public int clientY(FCView control) {
        if (control != null) {
            FCView parent = control.getParent();
            int cTop = control.getTop();
            if (parent != null) {
                return cTop - (control.displayOffset() ? parent.getDisplayOffset().y : 0) + clientY(parent);
            } else {
                return cTop;
            }
        } else {
            return 0;
        }
    }

    /**
     * 是否包含控件
     *
     * @param control 控件
     * @returns 是否包含
     */
    public boolean containsControl(FCView control) {
        for (FCView subControl : m_controls) {
            if (subControl == control) {
                return true;
            }
        }
        return false;
    }

    /**
     * 销毁资源
     */
    public void delete() {
        if (!m_isDeleted) {
            m_focusedControl = null;
            if (m_paint != null) {
                m_paint.delete();
                m_paint = null;
            }
            m_host = null;
            m_timers.clear();
            clearControls();
            m_isDeleted = true;
        }
    }

    /**
     * 根据触摸位置获取控件
     *
     * @param mp 坐标
     * @param control 控件
     * @returns 控件对象
     */
    protected FCView findControl(FCPoint mp, ArrayList<FCView> controls) {
        int size = controls.size();
        for (int i = size - 1; i >= 0; i--) {
            FCView control = controls.get(i);
            if (control.isVisible()) {
                if (control.containsPoint(mp)) {
                    ArrayList<FCView> subControls = new ArrayList<FCView>();
                    getSortedControls(control, subControls);
                    FCView subControl = findControl(mp.clone(), subControls);
                    subControls.clear();
                    if (subControl != null) {
                        return subControl;
                    }
                    return control;
                }
            }
        }
        return null;
    }

    /**
     * 根据名称查找控件
     *
     * @param name 名称
     * @param controls 控件集合
     * @returns 控件
     */
    protected FCView findControl(String name, ArrayList<FCView> controls) {
        int controlSize = controls.size();
        for (int i = 0; i < controlSize; i++) {
            FCView control = controls.get(i);
            if (control.getName().equals(name)) {
                return control;
            } else {
                ArrayList<FCView> subControls = control.getControls();
                if (subControls != null && subControls.size() > 0) {
                    FCView findControl = findControl(name, subControls);
                    if (findControl != null) {
                        return findControl;
                    }
                }
            }
        }
        return null;
    }

    /**
     * 根据坐标查找控件
     *
     * @param mp 坐标
     * @returns 控件
     */
    public FCView findControl(FCPoint mp) {
        ArrayList<FCView> subControls = new ArrayList<FCView>();
        getSortedControls(null, subControls);
        FCView control = findControl(mp.clone(), subControls);
        subControls.clear();
        return control;
    }

    /**
     * 根据坐标在控件中查找控件
     *
     * @param mp 坐标
     * @param parent 父控件
     * @returns 控件
     */
    public FCView findControl(FCPoint mp, FCView parent) {
        ArrayList<FCView> subControls = new ArrayList<FCView>();
        getSortedControls(parent, subControls);
        FCView control = findControl(mp.clone(), subControls);
        subControls.clear();
        return control;
    }

    /**
     * 根据名称查找控件
     *
     * @param name 名称
     * @returns 控件
     */
    public FCView findControl(String name) {
        return findControl(name, m_controls);
    }

    /**
     * 根据控件查找预处理事件的控件
     *
     * @param control 控件
     * @returns 控件
     */
    protected FCView findPreviewsControl(FCView control) {
        if (control.allowPreviewsEvent()) {
            return control;
        } else {
            FCView parent = control.getParent();
            if (parent != null) {
                return findPreviewsControl(parent);
            } else {
                return control;
            }
        }
    }

    /**
     * 根据控件查找窗体
     *
     * @param control 控件
     * @returns 窗体
     */
    protected FCView findWindow(FCView control) {
        if (control.isWindow()) {
            return control;
        } else {
            FCView parent = control.getParent();
            if (parent != null) {
                return findWindow(parent);
            } else {
                return control;
            }
        }
    }

    /**
     * 获取控件集合的拷贝
     */
    public ArrayList<FCView> getControls() {
        return m_controls;
    }

    /**
     * 获取焦点控件
     *
     * @param controls 控件集合
     * @returns 焦点控件
     */
    protected FCView getFocusedControl(ArrayList<FCView> controls) {
        int controlSize = controls.size();
        for (int i = 0; i < controlSize; i++) {
            FCView control = controls.get(i);
            if (control.isFocused()) {
                return control;
            } else {
                // 查找子控件
                ArrayList<FCView> subControls = control.getControls();
                if (subControls != null && subControls.size() > 0) {
                    FCView focusedControl = getFocusedControl(subControls);
                    if (focusedControl != null) {
                        return focusedControl;
                    }
                }
            }
        }
        return null;
    }

    /**
     * 获取绘制的透明度
     *
     * @param control 控件
     * @returns 透明度
     */
    protected float getPaintingOpacity(FCView control) {
        float opacity = control.getOpacity();
        FCView parent = control.getParent();
        if (parent != null) {
            opacity *= getPaintingOpacity(parent);
        } else {
            opacity *= m_opacity;
        }
        return opacity;
    }

    /**
     * 获取绘制的资源路径
     *
     * @param control 控件
     * @returns 路径
     */
    protected String getPaintingResourcePath(FCView control) {
        String resourcePath = control.getResourcePath();
        if (resourcePath != null && resourcePath.length() > 0) {
            return resourcePath;
        } else {
            FCView parent = control.getParent();
            if (parent != null) {
                return getPaintingResourcePath(parent);
            } else {
                return m_resourcePath;
            }
        }
    }

    /**
     * 获取排序后的控件集合
     *
     * @param parent 父控件
     * @param sortedControls 排序后的控件
     * @returns 状态
     */
    protected boolean getSortedControls(FCView parent, ArrayList<FCView> sortedControls) {
        ArrayList<FCView> controls = null;
        if (parent != null) {
            controls = parent.getControls();
        } else {
            controls = m_controls;
        }
        int controlSize = controls.size();
        int index = 0;
        for (int i = 0; i < controlSize; i++) {
            FCView control = controls.get(i);
            if (control.isVisible()) {
                if (control.isTopMost()) {
                    sortedControls.add(control);
                } else {
                    sortedControls.add(index, control);
                    index++;
                }
            }
        }
        return sortedControls.size() > 0;
    }

    /**
     * 判断是否绘制可用状态
     *
     * @param control 控件
     * @returns 是否绘制可用状态
     */
    protected boolean isPaintEnabled(FCView control) {
        if (control.isEnabled()) {
            FCView parent = control.getParent();
            if (parent != null) {
                return isPaintEnabled(parent);
            } else {
                return true;
            }
        } else {
            return false;
        }
    }

    /**
     * 插入控件
     *
     * @param index 索引
     * @param control 控件
     */
    public void insertControl(int index, FCView control) {
        m_controls.add(index, control);
    }

    /**
     * 使用缓存绘制图象，不重新计算绘图结构
     */
    public void invalidate() {
        if (m_host != null) {
            m_host.invalidate();
        }
    }

    /**
     * 局部绘图
     *
     * @param control 控件
     */
    public void invalidate(FCView control) {
        if (m_host != null) {
            int clientX = clientX(control);
            int clientY = clientY(control);
            m_host.invalidate(new FCRect(clientX, clientY, clientX + control.getWidth(), clientY + control.getHeight()));
        }
    }

    /**
     * 局部绘图
     *
     * @param rect 区域
     */
    public void invalidate(FCRect rect) {
        if (m_host != null) {
            m_host.invalidate(rect);
        }
    }

    /**
     * 绘图方法
     *
     * @param clipRect 矩形区域
     */
    public void onPaint(FCRect clipRect) {
        ArrayList<FCView> subCotrols = new ArrayList<FCView>();
        getSortedControls(null, subCotrols);
        renderControls(clipRect, subCotrols, m_resourcePath, m_opacity);
        subCotrols.clear();
    }

    /**
     * 预处理触摸事件
     *
     * @param eventID 事件ID
     * @param control 控件
     * @param touchInfo 触摸信息
     */
    public boolean onPreviewTouchEvent(int eventID, FCView control, FCTouchInfo touchInfo) {
        FCView previewsControl = findPreviewsControl(control);
        if (previewsControl != null) {
            int clientX = clientX(previewsControl);
            int clientY = clientY(previewsControl);
            FCPoint mp = touchInfo.m_firstPoint;
            FCPoint wcmp = new FCPoint(mp.x - clientX, mp.y - clientY);
            FCTouchInfo newTouchInfo = touchInfo.clone();
            newTouchInfo.m_firstPoint = wcmp.clone();
            newTouchInfo.m_secondPoint = wcmp.clone();
            if (previewsControl.onPreviewsTouchEvent(eventID, newTouchInfo)) {
                return true;
            }
        }
        return false;
    }

    /**
     * 处理尺寸改变
     */
    public void onResize() {
        update();
    }

    /**
     * 处理秒表
     *
     * @param timerID 秒表ID
     */
    public void onTimer(int timerID) {
        if (m_timers.containsKey(timerID)) {
            m_timers.get(timerID).onTimer(timerID);
        }
    }

    public void onTouchBegin(FCTouchInfo touchInfo) {
        m_draggingControl = null;
        m_touchDownControl = null;
        FCPoint mp = getTouchPoint();
        m_touchDownPoint = mp;
        ArrayList<FCView> subControls = new ArrayList<FCView>();
        if (!getSortedControls(null, subControls)) {
            subControls = m_controls;
        }
        FCView control = findControl(mp.clone(), subControls);
        if (control != null) {
            if (touchInfo.m_firstTouch && !touchInfo.m_secondTouch) {
                FCView window = findWindow(control);
                if (window != null && window.isWindow()) {
                    window.bringToFront();
                }
            }
            if (isPaintEnabled(control)) {
                int clientX = clientX(control);
                int clientY = clientY(control);
                FCPoint cmp = new FCPoint(mp.x - clientX, mp.y - clientY);
                FCView focusedControl = getFocusedControl();
                m_touchDownControl = control;
                if (touchInfo.m_firstTouch && !touchInfo.m_secondTouch) {
                    if (focusedControl == getFocusedControl()) {
                        if (control.canFocus()) {
                            setFocusedControl(control);
                        }
                    }
                    FCTouchInfo newTouchInfo = touchInfo.clone();
                    if (onPreviewTouchEvent(FCEventID.TOUCHDOWN, m_touchDownControl, newTouchInfo)) {
                        return;
                    }
                    newTouchInfo = touchInfo.clone();
                    newTouchInfo.m_firstPoint = cmp.clone();
                    newTouchInfo.m_secondPoint = cmp.clone();
                    m_touchDownControl.onTouchDown(newTouchInfo);
                    if (m_touchDownControl != null) {
                        RefObject<FCPoint> refPoint = new RefObject<FCPoint>(m_dragStartOffset);
                        m_touchDownControl.onDragReady(refPoint);
                    }
                }
            }
        }
    }

    public void onTouchCancel(FCTouchInfo touchInfo) {
        m_touchDownControl = null;
        m_draggingControl = null;
        m_touchMoveControl = null;
        invalidate();
    }

    public void onTouchEnd(FCTouchInfo touchInfo) {
        FCPoint mp = getTouchPoint();
        if (m_touchDownControl != null) {
            FCPoint cmp = new FCPoint(mp.x - clientX(m_touchDownControl), mp.y - clientY(m_touchDownControl));
            if (touchInfo.m_firstTouch && !touchInfo.m_secondTouch) {
                FCView touchDownControl = m_touchDownControl;
                FCTouchInfo newTouchInfo = touchInfo.clone();
                if (onPreviewTouchEvent(FCEventID.TOUCHUP, touchDownControl, newTouchInfo)) {
                    return;
                }
                if (m_touchDownControl != null) {
                    ArrayList<FCView> subControls = new ArrayList<FCView>();
                    if (!getSortedControls(null, subControls)) {
                        subControls = m_controls;
                    }
                    FCView control = findControl(mp.clone(), subControls);
                    if (control != null && control == m_touchDownControl) {
                        newTouchInfo = touchInfo.clone();
                        newTouchInfo.m_firstPoint = cmp.clone();
                        newTouchInfo.m_secondPoint = cmp.clone();
                        m_touchDownControl.onClick(newTouchInfo);
                    } else {
                        m_touchMoveControl = null;
                    }
                    if (m_touchDownControl != null) {
                        touchDownControl = m_touchDownControl;
                        m_touchDownControl = null;
                        newTouchInfo = touchInfo.clone();
                        newTouchInfo.m_firstPoint = cmp.clone();
                        newTouchInfo.m_secondPoint = cmp.clone();
                        touchDownControl.onTouchUp(newTouchInfo);
                    }
                }
            }
        } else if (m_draggingControl != null) {
            if (touchInfo.m_firstTouch && !touchInfo.m_secondTouch) {
                ArrayList<FCView> subControls = new ArrayList<FCView>();
                if (!getSortedControls(null, subControls)) {
                    subControls = m_controls;
                }
                FCPoint cmp = new FCPoint(mp.x - clientX(m_touchDownControl), mp.y - clientY(m_touchDownControl));
                FCView draggingControl = m_draggingControl;
                m_draggingControl = null;
                FCTouchInfo newTouchInfo = touchInfo.clone();
                if (onPreviewTouchEvent(FCEventID.TOUCHUP, draggingControl, newTouchInfo)) {
                    return;
                }
                newTouchInfo = touchInfo.clone();
                newTouchInfo.m_firstPoint = cmp.clone();
                newTouchInfo.m_secondPoint = cmp.clone();
                draggingControl.onTouchUp(newTouchInfo);
                draggingControl.onDragEnd();
                FCView parent = draggingControl.getParent();
                if (parent != null) {
                    parent.invalidate();
                } else {
                    invalidate();
                }
            }
        }
    }

    /**
     * 触摸移动事件
     *
     * @param ouchInfo 触摸信息
     */
    public void onTouchMove(FCTouchInfo touchInfo) {
        // 按下控件时
        FCPoint mp = getTouchPoint();
        if (m_touchDownControl != null) {
            if (touchInfo.m_firstTouch && !touchInfo.m_secondTouch) {
                FCTouchInfo newTouchInfo = touchInfo.clone();
                if (onPreviewTouchEvent(FCEventID.TOUCHMOVE, m_touchDownControl, newTouchInfo)) {
                    return;
                }
                FCPoint cmp = new FCPoint(mp.x - clientX(m_touchDownControl), mp.y - clientY(m_touchDownControl));
                newTouchInfo = touchInfo.clone();
                newTouchInfo.m_firstPoint = cmp.clone();
                newTouchInfo.m_secondPoint = cmp.clone();
                m_touchDownControl.onTouchMove(newTouchInfo);
                // 拖动
                if (m_touchDownControl.allowDrag()) {
                    if (Math.abs(mp.x - m_touchDownPoint.x) > m_dragStartOffset.x
                            || Math.abs(mp.y - m_touchDownPoint.y) > m_dragStartOffset.y) {
                        // 触发事件
                        if (m_touchDownControl.onDragBegin()) {
                            m_dragBeginRect = m_touchDownControl.getBounds();
                            m_dragBeginPoint = m_touchDownPoint.clone();
                            m_draggingControl = m_touchDownControl;
                            m_touchDownControl = null;
                            FCView parent = m_draggingControl.getParent();
                            if (parent != null) {
                                parent.invalidate();
                            } else {
                                invalidate();
                            }
                        }
                    }
                }
            }
        }
        // 拖动时
        else if (m_draggingControl != null) {
            if (touchInfo.m_firstTouch && !touchInfo.m_secondTouch) {
                FCView draggingControl = m_draggingControl;
                int offsetX = mp.x - m_dragBeginPoint.x;
                int offsetY = mp.y - m_dragBeginPoint.y;
                FCRect newBounds = m_dragBeginRect.clone();
                newBounds.left += offsetX;
                newBounds.top += offsetY;
                newBounds.right += offsetX;
                newBounds.bottom += offsetY;
                draggingControl.setBounds(newBounds);
                // 触发正在拖动事件
                draggingControl.onDragging();
                FCView parent = draggingControl.getParent();
                if (parent != null) {
                    parent.invalidate();
                } else {
                    invalidate();
                }
            }
        }
    }

    /**
     * 绘制控件
     *
     * @param rect 矩形
     * @param controls 控件集合
     * @param resourcePath 资源路径
     * @param opacity 透明度
     */
    protected void renderControls(FCRect rect, ArrayList<FCView> controls, String resourcePath, float opacity) {
        int controlSize = controls.size();
        for (int i = 0; i < controlSize; i++) {
            FCView control = controls.get(i);
            control.onPrePaint(m_paint, control.getDisplayRect());
            FCRect destRect = new FCRect();
            int clientX = clientX(control);
            int clientY = clientY(control);
            FCRect bounds = new FCRect(clientX, clientY, clientX + control.getWidth(), clientY + control.getHeight());
            // 获取自定义裁剪区域
            if (control.useRegion()) {
                FCRect clipRect = control.getRegion();
                bounds.left += clipRect.left;
                bounds.top += clipRect.top;
                bounds.right = bounds.left + clipRect.right - clipRect.left;
                bounds.bottom = bounds.top + clipRect.bottom - clipRect.top;
            }
            RefObject<FCRect> tempRef_destRect = new RefObject<FCRect>(destRect);
            RefObject<FCRect> tempRef_rect = new RefObject<FCRect>(rect);
            RefObject<FCRect> tempRef_bounds = new RefObject<FCRect>(bounds);
            boolean tempVar = control.isVisible() && m_host.getIntersectRect(tempRef_destRect, tempRef_rect, tempRef_bounds) > 0;
            if (tempVar) {
                // 设置裁剪
                FCRect clipRect = new FCRect(destRect.left - clientX, destRect.top - clientY, destRect.right - clientX, destRect.bottom - clientY);
                // 设置资源路径
                String newResourcePath = control.getResourcePath();
                if (newResourcePath == null || newResourcePath.length() == 0) {
                    newResourcePath = resourcePath;
                }
                // 设置透明度
                float newOpacity = control.getOpacity() * opacity;
                setPaint(clientX, clientY, clipRect, newResourcePath, newOpacity);
                control.onPaint(m_paint, clipRect);
                // 绘制子控件
                ArrayList<FCView> subControls = new ArrayList<FCView>();
                getSortedControls(control, subControls);
                if (subControls != null && subControls.size() > 0) {
                    renderControls(destRect, subControls, newResourcePath, newOpacity);
                    subControls.clear();
                }
                setPaint(clientX, clientY, clipRect, newResourcePath, newOpacity);
                control.onPaintBorder(m_paint, clipRect);
            }
        }
    }

    /**
     * 移除控件
     */
    public void removeControl(FCView control) {
        if (control == m_draggingControl) {
            m_draggingControl = null;
        }
        if (control == m_focusedControl) {
            m_focusedControl = null;
        }
        if (control == m_touchDownControl) {
            m_touchDownControl = null;
        }
        if (control == m_touchMoveControl) {
            m_touchMoveControl = null;
        }
        ArrayList<Integer> removeTimers = new ArrayList<Integer>();
        for (int timerID : m_timers.keySet()) {
            if (m_timers.get(timerID) == control) {
                removeTimers.add(timerID);
            }
        }
        for (int timerID : removeTimers) {
            stopTimer(timerID);
        }
        if (control.getParent() == null) {
            m_controls.remove(control);
            control.onRemove();
        }
    }

    /**
     * 将控件放到最下面显示
     */
    public void sendToBack(FCView control) {
        FCView parent = control.getParent();
        if (parent != null) {
            parent.sendChildToBack(control);
        } else {
            if (m_controls != null && m_controls.size() > 0) {
                m_controls.remove(control);
                m_controls.add(0, control);
            }
        }
    }

    /**
     * 设置排列
     */
    public void setAlign(ArrayList<FCView> controls) {
        int controlSize = controls.size();
        for (int i = 0; i < controlSize; i++) {
            FCView control = controls.get(i);
            if (control.displayOffset()) {
                FCSize parentSize = m_displaySize;
                FCView parent = control.getParent();
                if (parent != null) {
                    parentSize = parent.getSize();
                }
                FCSize size = control.getSize();
                FCPadding margin = control.getMargin();
                FCPadding padding = new FCPadding();
                if (parent != null) {
                    padding = parent.getPadding();
                }
                if (control.getAlign() == FCHorizontalAlign.Center) {
                    control.setLeft((parentSize.cx - size.cx) / 2);
                } else if (control.getAlign() == FCHorizontalAlign.Right) {
                    control.setLeft(parentSize.cx - size.cx - margin.right - padding.right);
                }
                if (control.getVerticalAlign() == FCVerticalAlign.Bottom) {
                    control.setTop(parentSize.cy - size.cy - margin.bottom - padding.bottom);
                } else if (control.getVerticalAlign() == FCVerticalAlign.Middle) {
                    control.setTop((parentSize.cy - size.cy) / 2);
                }
            }
        }
    }

    /**
     * 设置锚定信息
     *
     * @param controls 控件集合
     * @param oldSize 原尺寸
     */
    public void setAnchor(ArrayList<FCView> controls, FCSize oldSize) {
        if (oldSize.cx != 0 && oldSize.cy != 0) {
            int controlSize = controls.size();
            for (int i = 0; i < controlSize; i++) {
                FCView control = controls.get(i);
                FCSize parentSize = m_displaySize;
                FCView parent = control.getParent();
                if (parent != null) {
                    parentSize = parent.getSize();
                }
                FCAnchor anchor = control.getAnchor();
                FCRect bounds = control.getBounds();
                if (anchor.right && !anchor.left) {
                    bounds.left = bounds.left + parentSize.cx - oldSize.cx;
                }
                if (anchor.bottom && !anchor.top) {
                    bounds.top = bounds.top + parentSize.cy - oldSize.cy;
                }
                if (anchor.right) {
                    bounds.right = bounds.right + parentSize.cx - oldSize.cx;
                }
                if (anchor.bottom) {
                    bounds.bottom = bounds.bottom + parentSize.cy - oldSize.cy;
                }
                control.setBounds(bounds);
            }
        }
    }

    /**
     * 设置绑定边缘
     *
     * @param control 控件
     */
    public void setDock(ArrayList<FCView> controls) {
        int controlSize = controls.size();
        for (int i = 0; i < controlSize; i++) {
            FCView control = controls.get(i);
            FCSize parentSize = m_displaySize.clone();
            FCView parent = control.getParent();
            FCDockStyle dock = control.getDock();
            if (dock != FCDockStyle.None) {
                FCPadding padding = new FCPadding();
                if (parent != null) {
                    padding = parent.getPadding();
                }
                FCPadding margin = control.getMargin();
                FCSize cSize = control.getSize();
                FCRect spaceRect = new FCRect();
                spaceRect.left = padding.left + margin.left;
                spaceRect.top = padding.top + margin.top;
                spaceRect.right = parentSize.cx - padding.right - margin.right;
                spaceRect.bottom = parentSize.cy - padding.bottom - margin.bottom;
                FCRect bounds = new FCRect();
                if (dock == FCDockStyle.Bottom) {
                    bounds.left = spaceRect.left;
                    bounds.top = spaceRect.bottom - cSize.cy;
                    bounds.right = spaceRect.right;
                    bounds.bottom = spaceRect.bottom;
                } else if (dock == FCDockStyle.Fill) {
                    bounds = spaceRect;
                } else if (dock == FCDockStyle.Left) {
                    bounds.left = spaceRect.left;
                    bounds.top = spaceRect.top;
                    bounds.right = bounds.left + cSize.cx;
                    bounds.bottom = spaceRect.bottom;
                } else if (dock == FCDockStyle.Right) {
                    bounds.left = spaceRect.right - cSize.cx;
                    bounds.top = spaceRect.top;
                    bounds.right = spaceRect.right;
                    bounds.bottom = spaceRect.bottom;
                } else if (dock == FCDockStyle.Top) {
                    bounds.left = spaceRect.left;
                    bounds.top = spaceRect.top;
                    bounds.right = spaceRect.right;
                    bounds.bottom = bounds.top + cSize.cy;
                }
                control.setBounds(bounds);
            }
        }
    }

    /**
     * 设置绘图属性
     *
     * @param offsetX 横向偏移
     * @param offsetY 纵向偏移
     * @param clipRect 裁剪区域
     * @param resourcePath 资源路径
     * @param opacity 透明度
     */
    protected void setPaint(int offsetX, int offsetY, FCRect clipRect, String resourcePath, float opacity) {
        m_paint.setOffset(new FCPoint(offsetX, offsetY));
        m_paint.setClip(clipRect);
        m_paint.setResourcePath(resourcePath);
        m_paint.setOpacity(opacity);
    }

    /**
     * 启动秒表
     *
     * @param control 控件
     * @param timerID 秒表编号
     * @param interval 间隔
     */
    public void startTimer(FCView control, int timerID, int interval) {
        m_timers.put(timerID, control);
        if (m_host != null) {
            m_host.startTimer(timerID, interval);
        }
    }

    /**
     * 停止秒表
     */
    public void stopTimer(int timerID) {
        if (m_timers.containsKey(timerID)) {
            if (m_host != null) {
                m_host.stopTimer(timerID);
            }
            m_timers.remove(timerID);
        }
    }

    /**
     * 更新布局
     */
    public void update() {
        if (m_host != null) {
            FCSize oldSize = m_displaySize.clone();
            m_displaySize = m_host.getSize();
            if (m_displaySize.cx != 0 && m_displaySize.cy != 0) {
                setAlign(m_controls);
                setAnchor(m_controls, oldSize);
                setDock(m_controls);
                int controlsSize = m_controls.size();
                for (int i = 0; i < controlsSize; i++) {
                    m_controls.get(i).update();
                }
            }
        }
    }
}
