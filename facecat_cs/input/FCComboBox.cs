/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

using System;
using System.Collections.Generic;

namespace FaceCat {
    /// <summary>
    /// 下拉列表控件的列表
    /// </summary>
    public class FCComboBoxMenu : FCMenu {
        /// <summary>
        /// 创建列表
        /// </summary>
        public FCComboBoxMenu() {

        }

        protected FCComboBox m_comboBox;

        /// <summary>
        /// 获取或设置列表控件
        /// </summary>
        public virtual FCComboBox ComboBox {
            get { return m_comboBox; }
            set { m_comboBox = value; }
        }

        /// <summary>
        /// 是否自动隐藏
        /// </summary>
        /// <returns>是否隐藏</returns>
        public override bool onAutoHide() {
            if (m_comboBox != null && m_comboBox.Focused) {
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// 下拉列表控件
    /// </summary>
    public class FCComboBox : FCTextBox {
        /// <summary>
        /// 创建下拉列表
        /// </summary>
        public FCComboBox() {
            m_dropDownButtonTouchDownEvent = new FCTouchEvent(dropDownButtonTouchDown);
            m_menuItemClickEvent = new FCMenuItemTouchEvent(menuItemClick);
            m_menuKeyDownEvent = new FCKeyEvent(MenuKeyDown);
        }

        /// <summary>
        /// 下拉按钮的点击函数
        /// </summary>
        private FCTouchEvent m_dropDownButtonTouchDownEvent;

        /// <summary>
        /// 下拉菜单的点击函数
        /// </summary>
        private FCMenuItemTouchEvent m_menuItemClickEvent;

        /// <summary>
        /// 下拉菜单的按键函数
        /// </summary>
        private FCKeyEvent m_menuKeyDownEvent;

        protected FCButton m_dropDownButton;

        /// <summary>
        /// 获取下拉按钮
        /// </summary>
        public virtual FCButton DropDownButton {
            get { return m_dropDownButton; }
        }

        protected FCComboBoxMenu m_dropDownMenu;

        /// <summary>
        /// 获取下拉菜单
        /// </summary>
        public virtual FCComboBoxMenu DropDownMenu {
            get { return m_dropDownMenu; }
        }

        /// <summary>
        /// 获取或设置选中的索引
        /// </summary>
        public virtual int SelectedIndex {
            get {
                if (m_dropDownMenu != null) {
                    ArrayList<FCMenuItem> items = m_dropDownMenu.getItems();
                    int itemSize = items.size();
                    for (int i = 0; i < itemSize; i++) {
                        FCMenuItem item = items.get(i);
                        if (item.Checked) {
                            return i;
                        }
                    }
                }
                return -1;
            }
            set {
                if (m_dropDownMenu != null) {
                    ArrayList<FCMenuItem> items = m_dropDownMenu.getItems();
                    int itemSize = items.size();
                    bool changed = false;
                    for (int i = 0; i < itemSize; i++) {
                        FCMenuItem item = items.get(i);
                        if (i == value) {
                            if (!item.Checked) {
                                //选中并设置文字
                                item.Checked = true;
                                changed = true;
                            }
                            Text = item.Text;
                        }
                        else {
                            item.Checked = false;
                        }
                    }
                    if (changed) {
                        onSelectedIndexChanged();
                    }
                }
            }
        }

        /// <summary>
        /// 获取或设置选中的文字
        /// </summary>
        public virtual String SelectedText {
            get {
                if (m_dropDownMenu != null) {
                    ArrayList<FCMenuItem> items = m_dropDownMenu.getItems();
                    int itemSize = items.size();
                    for (int i = 0; i < itemSize; i++) {
                        FCMenuItem item = items.get(i);
                        if (item.Checked) {
                            return item.Text;
                        }
                    }
                }
                return null;
            }
            set {
                if (m_dropDownMenu != null) {
                    ArrayList<FCMenuItem> items = m_dropDownMenu.getItems();
                    int itemSize = items.size();
                    bool changed = false;
                    for (int i = 0; i < itemSize; i++) {
                        FCMenuItem item = items.get(i);
                        if (item.Text == value) {
                            //选中并设置文字
                            if (!item.Checked) {
                                item.Checked = true;
                                changed = true;
                            }
                            Text = item.Text;
                        }
                        else {
                            item.Checked = false;
                        }
                    }
                    if (changed) {
                        onSelectedIndexChanged();
                    }
                }
            }
        }

        /// <summary>
        /// 获取或设置选中的值
        /// </summary>
        public virtual String SelectedValue {
            get {
                if (m_dropDownMenu != null) {
                    ArrayList<FCMenuItem> items = m_dropDownMenu.getItems();
                    int itemSize = items.size();
                    for (int i = 0; i < itemSize; i++) {
                        FCMenuItem item = items.get(i);
                        if (item.Checked) {
                            return item.Value;
                        }
                    }
                }
                return null;
            }
            set {
                if (m_dropDownMenu != null) {
                    ArrayList<FCMenuItem> items = m_dropDownMenu.getItems();
                    int itemSize = items.size();
                    bool changed = false;
                    for (int i = 0; i < itemSize; i++) {
                        FCMenuItem item = items.get(i);
                        if (item.Value == value) {
                            //选中并设置文字
                            if (!item.Checked) {
                                item.Checked = true;
                                changed = true;
                            }
                            Text = item.Text;
                        }
                        else {
                            item.Checked = false;
                        }
                    }
                    if (changed) {
                        onSelectedIndexChanged();
                    }
                }
            }
        }

        /// <summary>
        /// 添加菜单项
        /// </summary>
        /// <param name="item">菜单项</param>
        public void addItem(FCMenuItem item) {
            if (m_dropDownMenu != null) {
                m_dropDownMenu.addItem(item);
            }
        }

        /// <summary>
        /// 销毁方法
        /// </summary>
        public override void delete() {
            if (!IsDeleted) {
                if (m_dropDownButton != null) {
                    if (m_dropDownButtonTouchDownEvent != null) {
                        m_dropDownButton.removeEvent(m_dropDownButtonTouchDownEvent, FCEventID.TOUCHDOWN);
                        m_dropDownButtonTouchDownEvent = null;
                    }
                }
                if (m_dropDownMenu != null) {
                    if (m_menuItemClickEvent != null) {
                        m_dropDownMenu.removeEvent(m_menuItemClickEvent, FCEventID.MENUITEMCLICK);
                        m_menuItemClickEvent = null;
                    }
                    if (m_menuKeyDownEvent != null) {
                        m_dropDownMenu.removeEvent(m_menuKeyDownEvent, FCEventID.KEYDOWN);
                        m_menuKeyDownEvent = null;
                    }
                    Native.removeControl(m_dropDownMenu);
                    m_dropDownMenu.delete();
                    m_dropDownMenu = null;
                }
            }
            base.delete();
        }

        /// <summary>
        /// 下拉按钮的点击方法
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="touchInfo">触摸信息</param>
        private void dropDownButtonTouchDown(object sender, FCTouchInfo touchInfo) {
            onDropDownOpening();
        }

        /// <summary>
        /// 清除所有菜单项
        /// </summary>
        public void clearItems() {
            if (m_dropDownMenu != null) {
                m_dropDownMenu.clearItems();
            }
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <returns>控件类型</returns>
        public override String getControlType() {
            return "ComboBox";
        }

        /// <summary>
        /// 获取事件名称列表
        /// </summary>
        /// <returns>名称列表</returns>
        public override ArrayList<String> getEventNames() {
            ArrayList<String> eventNames = base.getEventNames();
            eventNames.AddRange(new String[] { "SelectedIndexChanged" });
            return eventNames;
        }

        /// <summary>
        /// 获取菜单项
        /// </summary>
        /// <returns>菜单项集合</returns>
        public ArrayList<FCMenuItem> getItems() {
            if (m_dropDownMenu != null) {
                return m_dropDownMenu.getItems();
            }
            return null;
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">返回属性值</param>
        /// <param name="type">返回属性类型</param>
        public override void getProperty(String name, ref String value, ref String type) {
            if (name == "selectedindex") {
                type = "int";
                value = FCStr.convertIntToStr(SelectedIndex);
            }
            else if (name == "selectedtext") {
                type = "text";
                value = SelectedText;
            }
            else if (name == "selectedvalue") {
                type = "text";
                value = SelectedValue;
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
            propertyNames.AddRange(new String[] { "SelectedIndex", "SelectedText", "SelectedValue" });
            return propertyNames;
        }

        /// <summary>
        /// 插入菜单项
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="item">菜单项</param>
        public void insertItem(int index, FCMenuItem item) {
            if (m_dropDownMenu != null) {
                m_dropDownMenu.insertItem(index, item);
            }
        }

        /// <summary>
        /// 菜单项的点击方法
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="item">菜单项</param>
        /// <param name="touchInfo">触摸信息</param>
        private void menuItemClick(object sender, FCMenuItem item, FCTouchInfo touchInfo) {
            Text = item.Text;
            ArrayList<FCMenuItem> items = getItems();
            int itemSize = items.size();
            for (int i = 0; i < itemSize; i++) {
                if (items.get(i) == item) {
                    SelectedIndex = i;
                    break;
                }
            }
            SelectionStart = Text.Length;
            invalidate();
        }

        /// <summary>
        /// 下拉菜单的按键方法
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="key">按键</param>
        private void MenuKeyDown(object sender, char key) {
            if (key == 13) {
                onSelectedIndexChanged();
            }
        }

        /// <summary>
        /// 菜单下拉方法
        /// </summary>
        public virtual void onDropDownOpening() {
            if (m_dropDownMenu != null) {
                m_dropDownMenu.Native = Native;
                FCPoint nativePoint = pointToNative(new FCPoint(0, Height));
                m_dropDownMenu.Location = nativePoint;
                m_dropDownMenu.Size = new FCSize(Width, m_dropDownMenu.getContentHeight());
                m_dropDownMenu.Width = Width;
                m_dropDownMenu.Visible = true;
                m_dropDownMenu.bringToFront();
                m_dropDownMenu.invalidate();
            }
        }

        /// <summary>
        /// 键盘按下方法
        /// </summary>
        /// <param name="key">按键</param>
        public override void onKeyDown(char key) {
            base.onKeyDown(key);
            FCHost host = Native.Host;
            if (!host.isKeyPress(0x10) && !host.isKeyPress(0x11) && !host.isKeyPress(0x12)) {
                if (LinesCount <= 1) {
                    if (key == 13 || key == 38 || key == 40) {
                        if (m_dropDownMenu != null) {
                            m_dropDownMenu.onKeyDown(key);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 添加控件方法
        /// </summary>
        public override void onLoad() {
            base.onLoad();
            FCHost host = Native.Host;
            if (m_dropDownButton == null) {
                m_dropDownButton = host.createInternalControl(this, "dropdownbutton") as FCButton;
                addControl(m_dropDownButton);
                m_dropDownButton.addEvent(m_dropDownButtonTouchDownEvent, FCEventID.TOUCHDOWN);
            }
            if (m_dropDownMenu == null) {
                m_dropDownMenu = host.createInternalControl(this, "dropdownmenu") as FCComboBoxMenu;
                Native.addControl(m_dropDownMenu);
                m_dropDownMenu.Visible = false;
                m_dropDownMenu.addEvent(m_menuItemClickEvent, FCEventID.MENUITEMCLICK);
                m_dropDownMenu.addEvent(m_menuKeyDownEvent, FCEventID.KEYDOWN);
            }
            else {
                m_dropDownMenu.Native = Native;
            }
        }

        /// <summary>
        /// 选中索引改变方法
        /// </summary>
        public virtual void onSelectedIndexChanged() {
            callEvents(FCEventID.SELECTEDINDEXCHANGED);
        }

        /// <summary>
        /// 触摸滚轮方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onTouchWheel(FCTouchInfo touchInfo) {
            base.onTouchWheel(touchInfo);
            if (LinesCount <= 1) {
                if (m_dropDownMenu != null) {
                    FCTouchInfo newTouchInfo = touchInfo.clone();
                    newTouchInfo.m_firstPoint = m_dropDownMenu.TouchPoint;
                    newTouchInfo.m_secondPoint = m_dropDownMenu.TouchPoint;
                    m_dropDownMenu.onTouchWheel(newTouchInfo);
                }
            }
        }

        /// <summary>
        /// 移除菜单项
        /// </summary>
        /// <param name="item">菜单项</param>
        public void removeItem(FCMenuItem item) {
            if (m_dropDownMenu != null) {
                m_dropDownMenu.removeItem(item);
            }
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        public override void setProperty(String name, String value) {
            if (name == "selectedindex") {
                SelectedIndex = FCStr.convertStrToInt(value);
            }
            else if (name == "selectedtext") {
                SelectedText = value;
            }
            else if (name == "selectedvalue") {
                SelectedValue = value;
            }
            else {
                base.setProperty(name, value);
            }
        }

        /// <summary>
        /// 更新布局方法
        /// </summary>
        public override void update() {
            base.update();
            int width = Width, height = Height;
            if (m_dropDownButton != null) {
                int dWidth = m_dropDownButton.Width;
                m_dropDownButton.Location = new FCPoint(width - dWidth, 0);
                m_dropDownButton.Size = new FCSize(dWidth, height);
                Padding = new FCPadding(0, 0, dWidth, 0);
            }
        }
    }
}
