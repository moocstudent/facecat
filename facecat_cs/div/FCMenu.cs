/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-捂脸鹿创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

using System;
using System.Collections.Generic;

namespace FaceCat {
    /// <summary>
    /// 点击菜单事件
    /// </summary>
    /// <param name="sender">调用者</param>
    /// <param name="item">菜单项</param>
    /// <param name="touchInfo">触摸信息</param>
    public delegate void FCMenuItemTouchEvent(object sender, FCMenuItem item, FCTouchInfo touchInfo);

    /// <summary>
    /// 菜单控件
    /// </summary>
    public class FCMenu : FCLayoutDiv {
        /// <summary>
        /// 创建控件
        /// </summary>
        public FCMenu() {
            AutoSize = true;
            LayoutStyle = FCLayoutStyle.TopToBottom;
            MaximumSize = new FCSize(2000, 500);
            ShowHScrollBar = true;
            ShowVScrollBar = true;
            TopMost = true;
            FCSize size = new FCSize(200, 200);
            Size = size;
        }

        /// <summary>
        /// 菜单项
        /// </summary>
        public ArrayList<FCMenuItem> m_items = new ArrayList<FCMenuItem>();

        /// <summary>
        /// 秒表编号
        /// </summary>
        private int m_timerID = getNewTimerID();

        protected bool m_autoHide = true;

        /// <summary>
        /// 获取或设置是否自动隐藏
        /// </summary>
        public virtual bool AutoHide {
            get { return m_autoHide; }
            set { m_autoHide = value; }
        }

        protected FCMenuItem m_parentItem;

        /// <summary>
        /// 获取或设置父菜单项
        /// </summary>
        public virtual FCMenuItem ParentItem {
            get { return m_parentItem; }
            set { m_parentItem = value; }
        }

        protected bool m_popup;

        /// <summary>
        /// 获取或设置是否弹出
        /// </summary>
        public virtual bool Popup {
            get { return m_popup; }
            set {
                m_popup = value;
                if (m_popup) {
                    Visible = false;
                }
            }
        }

        /// <summary>
        /// 添加项
        /// </summary>
        /// <param name="item">菜单项</param>
        public void addItem(FCMenuItem item) {
            item.ParentMenu = this;
            item.onAddingItem(-1);
            m_items.add(item);
        }

        /// <summary>
        /// 自动适应位置和大小
        /// </summary>
        /// <param name="menu">菜单</param>
        protected void adjust(FCMenu menu) {
            FCNative native = Native;
            if (AutoSize) {
                int contentHeight = menu.getContentHeight();
                int maximumHeight = MaximumSize.cy;
                menu.Height = Math.Min(contentHeight, maximumHeight);
            }
            FCPoint mPoint = menu.Location;
            FCSize mSize = menu.Size;
            FCSize nSize = native.DisplaySize;
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
            menu.Location = mPoint;
            menu.update();
        }

        /// <summary>
        /// 调用菜单的触摸事件
        /// </summary>
        /// <param name="eventID">事件ID</param>
        /// <param name="item">菜单项</param>
        /// <param name="touchInfo">触摸信息</param>
        protected void callMenuItemTouchEvent(int eventID, FCMenuItem item, FCTouchInfo touchInfo) {
            if (m_events != null && m_events.containsKey(eventID)) {
                ArrayList<object> events = m_events.get(eventID);
                int eventSize = events.size();
                for (int i = 0; i < eventSize; i++) {
                    FCMenuItemTouchEvent func = events.get(i) as FCMenuItemTouchEvent;
                    if (func != null) {
                        func(this, item, touchInfo);
                    }
                }
            }
        }

        /// <summary>
        /// 检查图层是否具有焦点
        /// </summary>
        /// <param name="items">控件集合</param>
        /// <returns>是否有焦点</returns>
        protected bool checkDivFocused(ArrayList<FCMenuItem> items) {
            int itemSize = items.size();
            for (int i = 0; i < items.size(); i++) {
                FCMenuItem item = items.get(i);
                FCMenu dropDownMenu = item.DropDownMenu;
                if (dropDownMenu != null) {
                    if (checkFocused(dropDownMenu)) {
                        return true;
                    }
                }
                ArrayList<FCMenuItem> subItems = item.getItems();
                bool focused = checkDivFocused(subItems);
                if (focused) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 检查焦点
        /// </summary>
        /// <param name="control">控件</param>
        /// <returns>是否有焦点</returns>
        protected bool checkFocused(FCView control) {
            if (control.Focused) {
                return true;
            }
            else {
                ArrayList<FCView> subControls = control.getControls();
                if (subControls != null && subControls.size() > 0) {
                    int subControlSize = subControls.size();
                    for (int i = 0; i < subControlSize; i++) {
                        bool focused = checkFocused(subControls.get(i));
                        if (focused) {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// 清除所有的项
        /// </summary>
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

        /// <summary>
        /// 关闭网格控件
        /// </summary>
        /// <param name="items">菜单集合</param>
        /// <returns>是否关闭成功</returns>
        protected bool closeMenus(ArrayList<FCMenuItem> items) {
            int itemSize = items.size();
            bool close = false;
            for (int i = 0; i < itemSize; i++) {
                FCMenuItem item = items.get(i);
                ArrayList<FCMenuItem> subItems = item.getItems();
                if (closeMenus(subItems)) {
                    close = true;
                }
                FCMenu dropDownMenu = item.DropDownMenu;
                if (dropDownMenu != null && dropDownMenu.Visible) {
                    dropDownMenu.hide();
                    close = true;
                }
            }
            return close;
        }

        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <returns>布局控件</returns>
        public virtual FCMenu createDropDownMenu() {
            FCMenu menu = new FCMenu();
            menu.Popup = true;
            menu.ShowHScrollBar = true;
            menu.ShowVScrollBar = true;
            return menu;
        }

        /// <summary>
        /// 销毁方法
        /// </summary>
        public override void delete() {
            if (!IsDeleted) {
                stopTimer(m_timerID);
                clearItems();
            }
            base.delete();
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <returns>控件类型</returns>
        public override String getControlType() {
            return "Menu";
        }

        /// <summary>
        /// 获取事件名称列表
        /// </summary>
        /// <returns>名称列表</returns>
        public override ArrayList<String> getEventNames() {
            ArrayList<String> eventNames = base.getEventNames();
            eventNames.AddRange(new String[] { "MenuItemClick" });
            return eventNames;
        }

        /// <summary>
        /// 获取所有的菜单项
        /// </summary>
        /// <returns></returns>
        public ArrayList<FCMenuItem> getItems() {
            return m_items;
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">返回属性值</param>
        /// <param name="type">返回属性类型</param>
        public override void getProperty(String name, ref String value, ref String type) {
            if (name == "popup") {
                type = "bool";
                value = FCStr.convertBoolToStr(Popup);
            }
            else {
                base.getProperty(name, ref value, ref type);
            }
        }

        /// <summary>
        /// 获取属性名称列表
        /// </summary>
        /// <returns>属性名称列表</returns>
        public override ArrayList<String> getPropertyNames() {
            ArrayList<String> propertyNames = base.getPropertyNames();
            propertyNames.AddRange(new String[] { "Popup" });
            return propertyNames;
        }

        /// <summary>
        /// 插入项
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="item">菜单项</param>
        public void insertItem(int index, FCMenuItem item) {
            item.ParentMenu = this;
            item.onAddingItem(index);
            m_items.Insert(index, item);
        }

        /// <summary>
        /// 是否不处理自动隐藏
        /// </summary>
        /// <returns>是否不处理</returns>
        public virtual bool onAutoHide() {
            return true;
        }

        /// <summary>
        /// 菜单点击方法
        /// </summary>
        /// <param name="item">菜单项</param>
        /// <param name="touchInfo">触摸信息</param>
        public virtual void onMenuItemClick(FCMenuItem item, FCTouchInfo touchInfo) {
            if (item.getItems().size() == 0) {
                callMenuItemTouchEvent(FCEventID.MENUITEMCLICK, item, touchInfo);
                bool close = closeMenus(m_items);
                if (m_popup) {
                    hide();
                }
                else {
                    Native.invalidate();
                }
            }
            else {
                onMenuItemTouchMove(item, touchInfo);
            }
        }

        /// <summary>
        /// 菜单触摸移动方法
        /// </summary>
        /// <param name="item">菜单项</param>
        /// <param name="touchInfo">触摸信息</param>
        public virtual void onMenuItemTouchMove(FCMenuItem item, FCTouchInfo touchInfo) {
            FCNative native = Native;
            ArrayList<FCMenuItem> items = null;
            FCMenuItem parentItem = item.ParentItem;
            if (parentItem != null) {
                items = parentItem.getItems();
            }
            else {
                items = m_items;
            }
            //关闭其他表格
            bool close = closeMenus(items);
            if (item.getItems().size() > 0) {
                FCMenu dropDownMenu = item.DropDownMenu;
                //获取位置和大小
                if (dropDownMenu != null) {
                    dropDownMenu.Native = native;
                    FCLayoutStyle layoutStyle = LayoutStyle;
                    FCPoint location = new FCPoint(native.clientX(item) + item.Width, native.clientY(item));
                    if (layoutStyle == FCLayoutStyle.LeftToRight || layoutStyle == FCLayoutStyle.RightToLeft) {
                        location.x = native.clientX(item);
                        location.y = native.clientY(item) + item.Height;
                    }
                    //设置弹出位置
                    dropDownMenu.Opacity = Opacity;
                    dropDownMenu.Location = location;
                    dropDownMenu.bringToFront();
                    dropDownMenu.focus();
                    dropDownMenu.show();
                    adjust(dropDownMenu);
                }
            }
            native.invalidate();
        }

        /// <summary>
        /// 触摸点击方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onTouchDown(FCTouchInfo touchInfo) {
            base.onTouchDown(touchInfo);
            bool close = closeMenus(m_items);
            Native.invalidate();
        }

        /// <summary>
        /// 秒表方法
        /// </summary>
        /// <param name="timerID">秒表ID</param>
        public override void onTimer(int timerID) {
            base.onTimer(timerID);
            if (m_timerID == timerID) {
                if (m_autoHide && m_parentItem == null && Visible) {
                    if (!checkFocused(this) && !checkDivFocused(m_items) && onAutoHide()) {
                        bool close = closeMenus(m_items);
                        if (m_popup) {
                            hide();
                        }
                        else {
                            Native.invalidate();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 可见状态改变方法
        /// </summary>
        public override void onVisibleChanged() {
            base.onVisibleChanged();
            if (Visible) {
                if (m_popup) {
                    FCHScrollBar hScrollBar = HScrollBar;
                    FCVScrollBar vScrollBar = VScrollBar;
                    if (hScrollBar != null) {
                        hScrollBar.Pos = 0;
                    }
                    if (vScrollBar != null) {
                        vScrollBar.Pos = 0;
                    }
                    focus();
                    //修正显示位置
                    adjust(this);
                }
                startTimer(m_timerID, 10);
            }
            else {
                stopTimer(m_timerID);
                bool close = closeMenus(m_items);
                FCNative native = Native;
                if (native != null) {
                    native.invalidate();
                }
            }
        }

        /// <summary>
        /// 移除菜单项
        /// </summary>
        /// <param name="item">菜单项</param>
        public void removeItem(FCMenuItem item) {
            item.onRemovingItem();
            m_items.remove(item);
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name">属性名</param>
        /// <param name="value">属性值</param>
        public override void setProperty(String name, String value) {
            if (name == "popup") {
                Popup = FCStr.convertStrToBool(value);
            }
            else {
                base.setProperty(name, value);
            }
        }
    }
}
