/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.div;

import facecat.topin.btn.*;
import facecat.topin.core.*;
import java.util.*;

/*
 * 菜单项
 */
public class FCMenuItem extends FCButton {

    /*
     * 构造函数
     */
    public FCMenuItem() {
        setFont(new FCFont("SimSun", 12, false, false, false));
        setSize(new FCSize(200, 25));
    }

    /*
     * 构造函数
     */
    public FCMenuItem(String text) {
        setFont(new FCFont("SimSun", 12, false, false, false));
        setSize(new FCSize(200, 25));
        setText(text);
    }

    /**
     * 所有菜单项
     */
    protected ArrayList<FCMenuItem> m_items = new ArrayList<FCMenuItem>();

    protected boolean m_checked = false;

    /**
     * 获取是否选中
     */
    public boolean isChecked() {
        return m_checked;
    }

    /**
     * 设置是否选中
     */
    public void setChecked(boolean value) {
        m_checked = value;
    }

    protected FCMenu m_dropDownMenu = null;

    /**
     * 获取下拉表格
     */
    public FCMenu getDropDownMenu() {
        return m_dropDownMenu;
    }

    /**
     * 设置下拉表格
     */
    public void setDropDownMenu(FCMenu value) {
        m_dropDownMenu = value;
    }

    protected FCMenuItem m_parentItem = null;

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

    protected FCMenu m_parentMenu = null;

    /**
     * 获取置所在菜单
     */
    public FCMenu getParentMenu() {
        return m_parentMenu;
    }

    /**
     * 设置所在菜单
     */
    public void setParentMenu(FCMenu value) {
        m_parentMenu = value;
    }

    protected String m_value;

    /**
     * 获取值
     */
    public String getValue() {
        return m_value;
    }

    /**
     * 设置值
     */
    public void setValue(String value) {
        m_value = value;
    }

    /**
     * 添加子菜单项
     *
     * @param item 菜单项
     */
    public void addItem(FCMenuItem item) {
        item.setParentItem(this);
        item.setParentMenu(getParentMenu());
        item.onAddingItem(-1);
        m_items.add(item);
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
     * 获取控件类型
     */
    @Override
    public String getControlType() {
        return "MenuItem";
    }

    /**
     * 获取所有的项
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
        if (name.equals("checked")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(isChecked());
        } else if (name.equals("value")) {
            type.argvalue = "string";
            value.argvalue = getValue();
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
        propertyNames.addAll(Arrays.asList(new String[]{"Checked", "Value"}));
        return propertyNames;
    }

    /**
     * 插入项
     *
     * @param index 索引
     * @param item 菜单项
     */
    public void insertItem(int index, FCMenuItem item) {
        item.setParentItem(this);
        item.setParentMenu(getParentMenu());
        item.onAddingItem(index);
        m_items.add(index, item);
    }

    /**
     * 添加项
     *
     * @param index 行索引
     */
    public void onAddingItem(int index) {
        FCMenu dropDownMenu = null;
        if (m_parentItem != null) {
            dropDownMenu = m_parentItem.getDropDownMenu();
            if (dropDownMenu == null) {
                dropDownMenu = m_parentMenu.createDropDownMenu();
                dropDownMenu.setVisible(false);
                m_parentItem.setDropDownMenu(dropDownMenu);
                dropDownMenu.setParentItem(m_parentItem);
                m_parentMenu.getNative().addControl(dropDownMenu);
            }
        } else {
            dropDownMenu = m_parentMenu;
        }
        if (dropDownMenu != null) {
            if (index != -1) {
                dropDownMenu.insertControl(index, this);
            } else {
                dropDownMenu.addControl(this);
            }

            int itemSize = m_items.size();
            for (int i = 0; i < itemSize; i++) {
                m_items.get(i).onAddingItem(-1);
            }
        }
    }

    /**
     * 点击方法
     *
     * @param touchInfo 触摸信息
     */
    @Override
    public void onClick(FCTouchInfo touchInfo) {
        super.onClick(touchInfo.clone());
        if (m_parentMenu != null) {
            m_parentMenu.onMenuItemClick(this, touchInfo.clone());
        }
    }

    /**
     * 触摸移动方法
     *
     * @param touchInfo 触摸信息
     */
    @Override
    public void onTouchMove(FCTouchInfo touchInfo) {
        super.onTouchMove(touchInfo.clone());
        if (m_parentMenu != null) {
            m_parentMenu.onMenuItemTouchMove(this, touchInfo.clone());
        }
    }

    /**
     * 重绘前景方法
     *
     * @param paint 绘图对象
     * @param clipRect 裁剪矩形
     */
    @Override
    public void onPaintForeground(FCPaint paint, FCRect clipRect) {
        int width = getWidth(), height = getHeight();
        if (width > 0 && height > 0) {
            int right = width;
            int midY = height / 2;
            String text = getText();
            int tRight = 0;
            long textColor = getPaintingTextColor();
            if (text != null && text.length() > 0) {
                FCFont font = getFont();
                FCSize tSize = paint.textSize(text, font);
                FCRect tRect = new FCRect();
                tRect.left = 10;
                tRect.top = midY - tSize.cy / 2 + 2;
                tRect.right = tRect.left + tSize.cx;
                tRect.bottom = tRect.top + tSize.cy;
                paint.drawText(text, textColor, font, tRect);
                tRight = tRect.right + 4;
            }

            if (m_checked) {
                FCRect eRect = new FCRect(tRight, height / 2 - 4, tRight + 8, height / 2 + 4);
                paint.fillEllipse(textColor, eRect);
            }

            if (m_items.size() > 0) {
                FCPoint point1 = new FCPoint();
                FCPoint point2 = new FCPoint(), point3 = new FCPoint();
                FCMenu menu = m_parentMenu;
                if (m_parentItem != null) {
                    menu = m_parentItem.getDropDownMenu();
                }
                FCLayoutStyle layoutStyle = menu.getLayoutStyle();

                if (layoutStyle == FCLayoutStyle.LeftToRight || layoutStyle == FCLayoutStyle.RightToLeft) {
                    point1.x = right - 25;
                    point1.y = midY - 2;
                    point2.x = right - 14;
                    point2.y = midY - 2;
                    point3.x = right - 20;
                    point3.y = midY + 4;
                } else {
                    point1.x = right - 15;
                    point1.y = midY;
                    point2.x = right - 25;
                    point2.y = midY - 5;
                    point3.x = right - 25;
                    point3.y = midY + 5;
                }
                FCPoint[] points = new FCPoint[]{point1, point2, point3};
                paint.fillPolygon(textColor, points);
            }
        }
    }

    /**
     * 移除项方法
     */
    public void onRemovingItem() {
        FCMenu dropDownMenu = null;
        if (m_parentItem != null) {
            dropDownMenu = m_parentItem.getDropDownMenu();
        } else {
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
            m_parentMenu.getNative().removeControl(m_dropDownMenu);
            m_dropDownMenu.delete();
            m_dropDownMenu = null;
        }
    }

    /**
     * 移除子菜单项
     *
     * @param item 子菜单项
     */
    public void removeItem(FCMenuItem item) {
        item.onRemovingItem();
        m_items.remove(item);
    }

    /**
     * 设置属性
     *
     * @param name 属性名
     * @param value 属性值
     */
    @Override
    public void setProperty(String name, String value) {
        if (name.equals("checked")) {
            setChecked(FCStr.convertStrToBool(value));
        } else if (name.equals("value")) {
            setValue(value);
        } else {
            super.setProperty(name, value);
        }
    }
}
