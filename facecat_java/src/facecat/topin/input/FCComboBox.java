/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.input;

import facecat.topin.div.*;
import facecat.topin.btn.*;
import facecat.topin.core.*;
import java.util.*;

/**
 * 下拉列表控件
 */
public class FCComboBox extends FCTextBox implements FCMenuItemTouchEvent, FCTouchEvent {

    /**
     * 创建下拉列表
     */
    public FCComboBox() {
    }

    protected FCButton m_dropDownButton = null;

    /**
     * 获取下拉按钮
     */
    public FCButton getDropDownButton() {
        return m_dropDownButton;
    }

    protected FCComboBoxMenu m_dropDownMenu = null;

    /**
     * 获取下拉菜单
     */
    public FCComboBoxMenu getDropDownMenu() {
        return m_dropDownMenu;
    }

    /**
     * 获取选中的索引
     */
    public int getSelectedIndex() {
        if (m_dropDownMenu != null) {
            ArrayList<FCMenuItem> items = m_dropDownMenu.getItems();
            int itemSize = items.size();
            for (int i = 0; i < itemSize; i++) {
                FCMenuItem item = items.get(i);
                if (item.isChecked()) {
                    return i;
                }
            }
        }
        return -1;
    }

    /**
     * 设置选中的索引
     */
    public void setSelectedIndex(int value) {
        if (m_dropDownMenu != null) {
            ArrayList<FCMenuItem> items = m_dropDownMenu.getItems();
            int itemSize = items.size();
            boolean changed = false;
            for (int i = 0; i < itemSize; i++) {
                FCMenuItem item = items.get(i);
                if (i == value) {
                    if (!item.isChecked()) {
                        // 选中并设置文字
                        item.setChecked(true);
                        changed = true;
                    }
                    setText(item.getText());
                } else {
                    item.setChecked(false);
                }
            }
            if (changed) {
                onSelectedIndexChanged();
            }
        }
    }

    /**
     * 获取选中的文字
     */
    public String getSelectedText() {
        if (m_dropDownMenu != null) {
            ArrayList<FCMenuItem> items = m_dropDownMenu.getItems();
            int itemSize = items.size();
            for (int i = 0; i < itemSize; i++) {
                FCMenuItem item = items.get(i);
                if (item.isChecked()) {
                    return item.getText();
                }
            }
        }
        return null;
    }

    /**
     * 设置选中的文字
     */
    public void setSelectedText(String value) {
        if (m_dropDownMenu != null) {
            ArrayList<FCMenuItem> items = m_dropDownMenu.getItems();
            int itemSize = items.size();
            boolean changed = false;
            for (int i = 0; i < itemSize; i++) {
                FCMenuItem item = items.get(i);
                if (item.getText().equals(value)) {
                    // 选中并设置文字
                    if (!item.isChecked()) {
                        item.setChecked(true);
                        changed = true;
                    }
                    setText(item.getText());
                } else {
                    item.setChecked(false);
                }
            }
            if (changed) {
                onSelectedIndexChanged();
            }
        }
    }

    /**
     * 获取选中的值
     */
    public String getSelectedValue() {
        if (m_dropDownMenu != null) {
            ArrayList<FCMenuItem> items = m_dropDownMenu.getItems();
            int itemSize = items.size();
            for (int i = 0; i < itemSize; i++) {
                FCMenuItem item = items.get(i);
                if (item.isChecked()) {
                    return item.getValue();
                }
            }
        }
        return null;
    }

    /**
     * 设置选中的值
     */
    public void setSelectedValue(String value) {
        if (m_dropDownMenu != null) {
            ArrayList<FCMenuItem> items = m_dropDownMenu.getItems();
            int itemSize = items.size();
            boolean changed = false;
            for (int i = 0; i < itemSize; i++) {
                FCMenuItem item = items.get(i);
                if (item.getValue().equals(value)) {
                    // 选中并设置文字
                    if (!item.isChecked()) {
                        item.setChecked(true);
                        changed = true;
                    }
                    setText(item.getText());
                } else {
                    item.setChecked(false);
                }
            }
            if (changed) {
                onSelectedIndexChanged();
            }
        }
    }

    /**
     * 添加菜单项
     *
     * @param item 菜单项
     */
    public void addItem(FCMenuItem item) {
        if (m_dropDownMenu != null) {
            m_dropDownMenu.addItem(item);
        }
    }

    @Override
    public void callControlTouchEvent(int eventID, Object sender, FCTouchInfo touchInfo) {
        if (sender == m_dropDownButton) {
            if (eventID == FCEventID.TOUCHDOWN) {
                onDropDownOpening();
            }
        }
    }

    public void callMenuItemTouchEvent(int eventID, Object sender, FCMenuItem item, FCTouchInfo touchInfo) {
        if (sender == m_dropDownMenu) {
            if (eventID == FCEventID.MENUITEMCLICK) {
                setText(item.getText());
                ArrayList<FCMenuItem> items = getItems();
                int itemSize = items.size();
                for (int i = 0; i < itemSize; i++) {
                    if (items.get(i) == item) {
                        setSelectedIndex(i);
                        break;
                    }
                }
                setSelectionStart(getText().length());
                invalidate();
            }
        }
    }

    /**
     * 销毁方法
     */
    @Override
    public void delete() {
        if (!isDeleted()) {
            if (m_dropDownButton != null) {
                m_dropDownButton.removeEvent(this, FCEventID.TOUCHDOWN);
            }
            if (m_dropDownMenu != null) {
                m_dropDownMenu.removeEvent(this, FCEventID.MENUITEMCLICK);
                getNative().removeControl(m_dropDownMenu);
                m_dropDownMenu.delete();
                m_dropDownMenu = null;
            }
        }
        super.delete();
    }

    /**
     * 清除所有菜单项
     */
    public void clearItems() {
        if (m_dropDownMenu != null) {
            m_dropDownMenu.clearItems();
        }
    }

    /**
     * 获取控件类型
     */
    @Override
    public String getControlType() {
        return "ComboBox";
    }

    /**
     * 获取菜单项
     */
    public ArrayList<FCMenuItem> getItems() {
        if (m_dropDownMenu != null) {
            return m_dropDownMenu.getItems();
        }
        return null;
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
        if (name.equals("selectedindex")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getSelectedIndex());
        } else if (name.equals("selectedtext")) {
            type.argvalue = "text";
            value.argvalue = getSelectedText();
        } else if (name.equals("selectedvalue")) {
            type.argvalue = "text";
            value.argvalue = getSelectedValue();
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
        propertyNames.addAll(Arrays.asList(new String[]{"SelectedIndex", "SelectedText", "SelectedValue"}));
        return propertyNames;
    }

    /**
     * 插入菜单项
     *
     * @param index 索引
     * @param item 菜单项
     */
    public void insertItem(int index, FCMenuItem item) {
        if (m_dropDownMenu != null) {
            m_dropDownMenu.insertItem(index, item);
        }
    }

    /**
     * 菜单下拉方法
     */
    public void onDropDownOpening() {
        m_dropDownMenu.setNative(getNative());
        FCPoint nativePoint = pointToNative(new FCPoint(0, getHeight()));
        m_dropDownMenu.setLocation(nativePoint);
        m_dropDownMenu.setSize(new FCSize(getWidth(), m_dropDownMenu.getContentHeight()));
        m_dropDownMenu.setVisible(true);
        m_dropDownMenu.bringToFront();
        m_dropDownMenu.invalidate();
    }

    /**
     * 添加控件方法
     */
    @Override
    public void onLoad() {
        super.onLoad();
        FCHost host = getNative().getHost();
        if (m_dropDownButton == null) {
            FCView tempVar = host.createInternalControl(this, "dropdownbutton");
            m_dropDownButton = (FCButton) ((tempVar instanceof FCButton) ? tempVar : null);
            addControl(m_dropDownButton);
            m_dropDownButton.addEvent(this, FCEventID.TOUCHDOWN);
        }
        if (m_dropDownMenu == null) {
            FCView tempVar2 = host.createInternalControl(this, "dropdownmenu");
            m_dropDownMenu = (FCComboBoxMenu) ((tempVar2 instanceof FCComboBoxMenu) ? tempVar2 : null);
            getNative().addControl(m_dropDownMenu);
            m_dropDownMenu.setVisible(false);
            m_dropDownMenu.addEvent(this, FCEventID.MENUITEMCLICK);
        } else {
            m_dropDownMenu.setNative(getNative());
        }
    }

    /**
     * 选中索引改变方法
     */
    public void onSelectedIndexChanged() {
        callEvents(FCEventID.SELECTEDINDEXCHANGED);
    }

    /**
     * 移除菜单项
     *
     * @param item 菜单项
     */
    public void removeItem(FCMenuItem item) {
        if (m_dropDownMenu != null) {
            m_dropDownMenu.removeItem(item);
        }
    }

    /**
     * 设置属性值
     *
     * @param name 属性名称
     * @param value 属性值
     */
    @Override
    public void setProperty(String name, String value) {
        if (name.equals("selectedindex")) {
            setSelectedIndex(FCStr.convertStrToInt(value));
        } else if (name.equals("selectedtext")) {
            setSelectedText(value);
        } else if (name.equals("selectedvalue")) {
            setSelectedValue(value);
        } else {
            super.setProperty(name, value);
        }
    }

    /**
     * 更新布局方法
     */
    @Override
    public void update() {
        super.update();
        int width = getWidth(), height = getHeight();
        if (m_dropDownButton != null) {
            int dWidth = m_dropDownButton.getWidth();
            m_dropDownButton.setLocation(new FCPoint(width - dWidth, 0));
            m_dropDownButton.setSize(new FCSize(dWidth, height));
            setPadding(new FCPadding(0, 0, dWidth, 0));
        }
    }
}
