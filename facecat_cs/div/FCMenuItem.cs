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
    /// 菜单项
    /// </summary>
    public class FCMenuItem : FCButton {
        /// <summary>
        /// 创建菜单项
        /// </summary>
        public FCMenuItem() {
            Font = new FCFont("宋体", 12, false, false, false);
            Size = new FCSize(200, 25);
        }

        /// <summary>
        /// 创建菜单项
        /// </summary>
        /// <param name="text">文字</param>
        public FCMenuItem(String text) {
            Font = new FCFont("宋体", 12, false, false, false);
            Size = new FCSize(200, 25);
            Text = text;
        }

        /// <summary>
        /// 所有菜单项
        /// </summary>
        public ArrayList<FCMenuItem> m_items = new ArrayList<FCMenuItem>();

        protected bool m_checked = false;

        /// <summary>
        /// 获取或设置是否选中
        /// </summary>
        public virtual bool Checked {
            get { return m_checked; }
            set { m_checked = value; }
        }

        protected FCMenu m_dropDownMenu;

        /// <summary>
        /// 获取或设置下拉表格
        /// </summary>
        public virtual FCMenu DropDownMenu {
            get { return m_dropDownMenu; }
            set { m_dropDownMenu = value; }
        }

        protected FCMenuItem m_parentItem;

        /// <summary>
        /// 获取或设置父菜单项
        /// </summary>
        public virtual FCMenuItem ParentItem {
            get { return m_parentItem; }
            set { m_parentItem = value; }
        }

        protected FCMenu m_parentMenu;

        /// <summary>
        /// 获取或设置所在菜单
        /// </summary>
        public virtual FCMenu ParentMenu {
            get { return m_parentMenu; }
            set { m_parentMenu = value; }
        }

        protected String m_value;

        /// <summary>
        /// 获取或设置值
        /// </summary>
        public virtual String Value {
            get { return m_value; }
            set { m_value = value; }
        }

        /// <summary>
        /// 添加子菜单项
        /// </summary>
        /// <param name="item">菜单项</param>
        public void addItem(FCMenuItem item) {
            item.ParentItem = this;
            item.ParentMenu = ParentMenu;
            item.onAddingItem(-1);
            m_items.add(item);
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
        /// 获取控件类型
        /// </summary>
        /// <returns>控件类型</returns>
        public override String getControlType() {
            return "MenuItem";
        }

        /// <summary>
        /// 获取所有的项
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
            if (name == "checked") {
                type = "bool";
                value = FCStr.convertBoolToStr(Checked);
            }
            else if (name == "value") {
                type = "String";
                value = Value;
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
            propertyNames.AddRange(new String[] { "Checked", "Value" });
            return propertyNames;
        }

        /// <summary>
        /// 插入项
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="item">菜单项</param>
        public void insertItem(int index, FCMenuItem item) {
            item.ParentItem = this;
            item.ParentMenu = ParentMenu;
            item.onAddingItem(index);
            m_items.Insert(index, item);
        }

        /// <summary>
        /// 添加项
        /// </summary>
        /// <param name="index">行索引</param>
        public virtual void onAddingItem(int index) {
            FCMenu dropDownMenu = null;
            if (m_parentItem != null) {
                dropDownMenu = m_parentItem.DropDownMenu;
                if (dropDownMenu == null) {
                    dropDownMenu = m_parentMenu.createDropDownMenu();
                    dropDownMenu.Visible = false;
                    m_parentItem.DropDownMenu = dropDownMenu;
                    dropDownMenu.ParentItem = m_parentItem;
                    m_parentMenu.Native.addControl(dropDownMenu);
                }
            }
            else {
                dropDownMenu = m_parentMenu;
            }
            if (dropDownMenu != null) {
                if (index != -1) {
                    //插入行
                    dropDownMenu.insertControl(index, this);
                }
                else {
                    //添加行
                    dropDownMenu.addControl(this);
                }
                //添加子节点
                int itemSize = m_items.size();
                for (int i = 0; i < itemSize; i++) {
                    m_items.get(i).onAddingItem(-1);
                }
            }
        }

        /// <summary>
        /// 点击方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onClick(FCTouchInfo touchInfo) {
            base.onClick(touchInfo);
            if (m_parentMenu != null) {
                m_parentMenu.onMenuItemClick(this, touchInfo);
            }
        }

        /// <summary>
        /// 触摸移动方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onTouchMove(FCTouchInfo touchInfo) {
            base.onTouchMove(touchInfo);
            if (m_parentMenu != null) {
                m_parentMenu.onMenuItemTouchMove(this, touchInfo);
            }
        }

        /// <summary>
        /// 重绘前景方法 
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪矩形</param>
        public override void onPaintForeground(FCPaint paint, FCRect clipRect) {
            int width = Width, height = Height;
            if (width > 0 && height > 0) {
                int right = width;
                int midY = height / 2;
                String text = Text;
                int tRight = 0;
                long textColor = getPaintingTextColor();
                if (text != null && text.Length > 0) {
                    FCFont font = Font;
                    FCSize tSize = paint.textSize(text, font);
                    FCRect tRect = new FCRect();
                    tRect.left = 10;
                    tRect.top = midY - tSize.cy / 2 + 2;
                    tRect.right = tRect.left + tSize.cx;
                    tRect.bottom = tRect.top + tSize.cy;
                    paint.drawText(text, textColor, font, tRect);
                    tRight = tRect.right + 4;
                }
                //绘制选中
                if (m_checked) {
                    FCRect eRect = new FCRect(tRight, height / 2 - 4, tRight + 8, height / 2 + 4);
                    paint.fillEllipse(textColor, eRect);
                }
                //画子菜单的提示箭头
                if (m_items.size() > 0) {
                    FCPoint point1 = new FCPoint(), point2 = new FCPoint(), point3 = new FCPoint();
                    FCMenu menu = m_parentMenu;
                    if (m_parentItem != null) {
                        menu = m_parentItem.DropDownMenu;
                    }
                    FCLayoutStyle layoutStyle = menu.LayoutStyle;
                    //横向
                    if (layoutStyle == FCLayoutStyle.LeftToRight || layoutStyle == FCLayoutStyle.RightToLeft) {
                        point1.x = right - 25;
                        point1.y = midY - 2;
                        point2.x = right - 14;
                        point2.y = midY - 2;
                        point3.x = right - 20;
                        point3.y = midY + 4;
                    }
                    //纵向
                    else {
                        point1.x = right - 15;
                        point1.y = midY;
                        point2.x = right - 25;
                        point2.y = midY - 5;
                        point3.x = right - 25;
                        point3.y = midY + 5;
                    }
                    FCPoint[] points = new FCPoint[] { point1, point2, point3 };
                    paint.fillPolygon(textColor, points);
                }
            }
        }

        /// <summary>
        /// 移除项方法
        /// </summary>
        public virtual void onRemovingItem() {
            FCMenu dropDownMenu = null;
            if (m_parentItem != null) {
                dropDownMenu = m_parentItem.DropDownMenu;
            }
            else {
                dropDownMenu = m_parentMenu;
            }
            if (dropDownMenu != null) {
                if (m_items != null && m_items.size() > 0) {
                    int itemSize = m_items.size();
                    for (int i = 0; i < itemSize; i++) {
                        m_items.get(i).onRemovingItem();
                    }
                }
                dropDownMenu.removeControl(this);
            }
            if (m_dropDownMenu != null) {
                m_parentMenu.Native.removeControl(m_dropDownMenu);
                m_dropDownMenu.delete();
                m_dropDownMenu = null;
            }
        }

        /// <summary>
        /// 移除子菜单项
        /// </summary>
        /// <param name="item">子菜单项</param>
        public void RemoveItem(FCMenuItem item) {
            item.onRemovingItem();
            m_items.remove(item);
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name">属性名</param>
        /// <param name="value">属性值</param>
        public override void setProperty(String name, String value) {
            if (name == "checked") {
                Checked = FCStr.convertStrToBool(value);
            }
            else if (name == "value") {
                Value = value;
            }
            else {
                base.setProperty(name, value);
            }
        }
    }
}
