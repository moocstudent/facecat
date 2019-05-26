/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.div;

import facecat.topin.scroll.*;
import facecat.topin.div.*;
import facecat.topin.core.*;
import java.util.*;

/**
 * 菜单控件
 */
public class FCMenu extends FCLayoutDiv {

    /**
     * 创建控件
     */
    public FCMenu() {
        setAutoSize(true);
        setLayoutStyle(FCLayoutStyle.TopToBottom);
        setMaximumSize(new FCSize(2000, 500));
        setShowHScrollBar(true);
        setShowVScrollBar(true);
        setTopMost(true);
        FCSize size = new FCSize(200, 200);
        setSize(size);
    }

    /**
     * 菜单项
     */
    public ArrayList<FCMenuItem> m_items = new ArrayList<FCMenuItem>();

    /**
     * 秒表编号
     */
    private int m_timerID = getNewTimerID();

    protected boolean m_autoHide = true;

    /**
     * 获取是否自动隐藏
     */
    public boolean autoHide() {
        return m_autoHide;
    }

    /**
     * 设置是否自动隐藏
     */
    public void setAutohHide(boolean autoHide) {
        m_autoHide = autoHide;
    }

    protected FCMenuItem m_parentItem;

    /**
     * 获取父菜单项
     */
    public FCMenuItem getParentItem() {
        return m_parentItem;
    }

    /**
     * 设置父菜单项
     */
    public void setParentItem(FCMenuItem value) {
        m_parentItem = value;
    }

    protected boolean m_popup = false;

    /**
     * 获取是否弹出
     */
    public boolean canPopup() {
        return m_popup;
    }

    /**
     * 设置是否弹出
     */
    public void setPopup(boolean value) {
        m_popup = value;
        if (m_popup) {
            setVisible(false);
        }
    }

    /**
     * 添加项
     *
     * @param item 菜单项
     */
    public void addItem(FCMenuItem item) {
        item.setParentMenu(this);
        item.onAddingItem(-1);
        m_items.add(item);
    }

    /**
     * 自动适应位置和大小
     *
     * @param menu 菜单
     */
    protected void adjust(FCMenu menu) {
        FCNative inative = getNative();
        if (autoSize()) {
            int contentHeight = menu.getContentHeight();
            int maximumHeight = getMaximumSize().cy;
            menu.setHeight(Math.min(contentHeight, maximumHeight));
        }
        FCPoint mPoint = menu.getLocation();
        FCSize mSize = menu.getSize();
        FCSize nSize = inative.getDisplaySize();
        if (mPoint.x < 0) {
            mPoint.x = 0;
        }
        if (mPoint.y < 0) {
            mPoint.y = 0;
        }
        if (mPoint.x + mSize.cx > nSize.cx) {
            mPoint.x = nSize.cx - mSize.cx;
        }
        if (mPoint.y + mSize.cy > nSize.cy) {
            mPoint.y = nSize.cy - mSize.cy;
        }
        menu.setLocation(mPoint);
        menu.update();
    }

    /**
     * 调用菜单的触摸事件
     *
     * @param eventID 事件ID
     * @param item 菜单项
     * @param touchInfo 触摸信息
     */
    protected void callMenuItemTouchEvent(int eventID, FCMenuItem item, FCTouchInfo touchInfo) {
        if (m_events != null && m_events.containsKey(eventID)) {
            ArrayList<Object> events = m_events.get(eventID);
            int eventSize = events.size();
            for (int i = 0; i < eventSize; i++) {
                FCMenuItemTouchEvent func = (FCMenuItemTouchEvent) ((events.get(i) instanceof FCMenuItemTouchEvent) ? events.get(i) : null);
                if (func != null) {
                    func.callMenuItemTouchEvent(eventID, this, item, touchInfo);
                }
            }
        }
    }

    /**
     * 检查图层是否具有焦点
     *
     * @param items 控件集合
     * @returns 是否有焦点
     */
    protected boolean checkDivFocused(ArrayList<FCMenuItem> items) {
        int itemSize = items.size();
        for (int i = 0; i < items.size(); i++) {
            FCMenuItem item = items.get(i);
            FCMenu dropDownMenu = item.getDropDownMenu();
            if (dropDownMenu != null) {
                if (checkFocused(dropDownMenu)) {
                    return true;
                }
            }
            ArrayList<FCMenuItem> subItems = item.getItems();
            boolean focused = checkDivFocused(subItems);
            if (focused) {
                return true;
            }
        }
        return false;
    }

    /**
     * 检查焦点
     *
     * @param control 控件
     * @returns 是否有焦点
     */
    protected boolean checkFocused(FCView control) {
        if (control.isFocused()) {
            return true;
        } else {
            ArrayList<FCView> subControls = control.getControls();
            if (subControls != null && subControls.size() > 0) {
                int subControlSize = subControls.size();
                for (int i = 0; i < subControlSize; i++) {
                    boolean focused = checkFocused(subControls.get(i));
                    if (focused) {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    /**
     * 清除所有的项
     */
    public void clearItems() {
        ArrayList<FCMenuItem> itemsCopy = new ArrayList<FCMenuItem>();
        int itemSize = m_items.size();
        for (int i = 0; i < itemSize; i++) {
            itemsCopy.add(m_items.get(i));
        }
        int copySize = itemsCopy.size();
        for (int i = 0; i < copySize; i++) {
            itemsCopy.get(i).onRemovingItem();
            itemsCopy.get(i).delete();
        }
        m_items.clear();
    }

    /**
     * 关闭网格控件
     *
     * @param items 菜单集合
     * @returns 是否关闭成功
     */
    protected boolean closeMenus(ArrayList<FCMenuItem> items) {
        int itemSize = items.size();
        boolean close = false;
        for (int i = 0; i < itemSize; i++) {
            FCMenuItem item = items.get(i);
            ArrayList<FCMenuItem> subItems = item.getItems();
            if (closeMenus(subItems)) {
                close = true;
            }
            FCMenu dropDownMenu = item.getDropDownMenu();
            if (dropDownMenu != null && dropDownMenu.isVisible()) {
                dropDownMenu.hide();
                close = true;
            }
        }
        return close;
    }

    /**
     * 创建菜单
     */
    public FCMenu createDropDownMenu() {
        FCMenu menu = new FCMenu();
        menu.setPopup(true);
        menu.setShowHScrollBar(true);
        menu.setShowVScrollBar(true);
        return menu;
    }

    /**
     * 销毁方法
     */
    @Override
    public void delete() {
        if (!isDeleted()) {
            stopTimer(m_timerID);
            clearItems();
        }
        super.delete();
    }

    /**
     * 获取控件类型
     */
    @Override
    public String getControlType() {
        return "Menu";
    }

    /**
     * 获取事件名称列表
     */
    public ArrayList<FCMenuItem> getItems() {
        return m_items;
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
        if (name.equals("popup")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(canPopup());
        } else {
            super.getProperty(name, value, type);
        }
    }

    /**
     * 获取属性名称列表
     */
    @Override
    public ArrayList<String> getPropertyNames() {
        ArrayList<String> propertyNames = super.getPropertyNames();
        propertyNames.addAll(Arrays.asList(new String[]{"Popup"}));
        return propertyNames;
    }

    /**
     * 插入项
     *
     * @param index 索引
     * @param item 菜单项
     */
    public void insertItem(int index, FCMenuItem item) {
        item.setParentMenu(this);
        item.onAddingItem(index);
        m_items.add(index, item);
    }

    /**
     * 是否不处理自动隐藏
     */
    public boolean onAutoHide() {
        return true;
    }

    @Override
    public void onLoad() {
        super.onLoad();
    }

    /**
     * 菜单点击方法
     *
     * @param item 菜单项
     * @param touchInfo 触摸信息
     */
    public void onMenuItemClick(FCMenuItem item, FCTouchInfo touchInfo) {
        if (item.getItems().isEmpty()) {
            callMenuItemTouchEvent(FCEventID.MENUITEMCLICK, item, touchInfo.clone());
            boolean close = closeMenus(m_items);
            if (m_popup) {
                hide();
            } else {
                getNative().invalidate();
            }
        } else {
            onMenuItemTouchMove(item, touchInfo.clone());
        }
    }

    /**
     * 菜单触摸移动方法
     *
     * @param item 菜单项
     * @param touchInfo 触摸信息
     */
    public void onMenuItemTouchMove(FCMenuItem item, FCTouchInfo touchInfo) {
        FCNative inative = getNative();
        ArrayList<FCMenuItem> items = null;
        FCMenuItem parentItem = item.getParentItem();
        if (parentItem != null) {
            items = parentItem.getItems();
        } else {
            items = m_items;
        }
        // 关闭其他表格
        boolean close = closeMenus(items);
        if (item.getItems().size() > 0) {
            FCMenu dropDownMenu = item.getDropDownMenu();
            // 获取位置和大小
            if (dropDownMenu != null) {
                dropDownMenu.setNative(inative);
                FCLayoutStyle layoutStyle = getLayoutStyle();
                FCPoint location = new FCPoint(inative.clientX(item) + item.getWidth(), inative.clientY(item));
                if (layoutStyle == FCLayoutStyle.LeftToRight || layoutStyle == FCLayoutStyle.RightToLeft) {
                    location.x = inative.clientX(item);
                    location.y = inative.clientY(item) + item.getHeight();
                }
                // 设置弹出位置
                dropDownMenu.setOpacity(getOpacity());
                dropDownMenu.setLocation(location);
                dropDownMenu.bringToFront();
                dropDownMenu.focus();
                dropDownMenu.show();
                adjust(dropDownMenu);
            }
        }
        inative.invalidate();
    }

    /**
     * 触摸点击方法
     *
     * @param touchInfo 触摸信息
     */
    @Override
    public void onTouchDown(FCTouchInfo touchInfo) {
        super.onTouchDown(touchInfo.clone());
        boolean close = closeMenus(m_items);
        getNative().invalidate();
    }

    /**
     * 秒表方法
     *
     * @param timerID 秒表ID
     */
    @Override
    public void onTimer(int timerID) {
        super.onTimer(timerID);
        if (m_timerID == timerID) {
            if (m_autoHide && m_parentItem == null && isVisible()) {
                if (!checkFocused(this) && !checkDivFocused(m_items) && onAutoHide()) {
                    boolean close = closeMenus(m_items);
                    if (m_popup) {
                        hide();
                    } else {
                        getNative().invalidate();
                    }
                }
            }
        }
    }

    /**
     * 可见状态改变方法
     */
    @Override
    public void onVisibleChanged() {
        super.onVisibleChanged();
        if (isVisible()) {
            if (m_popup) {
                FCHScrollBar hScrollBar = getHScrollBar();
                FCVScrollBar vScrollBar = getVScrollBar();
                if (hScrollBar != null) {
                    hScrollBar.setPos(0);
                }
                if (vScrollBar != null) {
                    vScrollBar.setPos(0);
                }
                focus();
                // 修正显示位置
                adjust(this);
            }
            startTimer(m_timerID, 10);
        } else {
            stopTimer(m_timerID);
            boolean close = closeMenus(m_items);
            FCNative inative = getNative();
            if (inative != null) {
                inative.invalidate();
            }
        }
    }

    /**
     * 移除菜单项
     *
     * @param item 菜单项
     */
    public void removeItem(FCMenuItem item) {
        item.onRemovingItem();
        m_items.remove(item);
    }

    /**
     * 设置属性值
     *
     * @param name 属性名称
     * @param value 属性值
     */
    @Override
    public void setProperty(String name, String value) {
        if (name.equals("popup")) {
            setPopup(FCStr.convertStrToBool(value));
        } else {
            super.setProperty(name, value);
        }
    }
}
