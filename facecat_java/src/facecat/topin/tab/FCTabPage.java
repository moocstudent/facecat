/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.tab;

import facecat.topin.div.*;
import facecat.topin.btn.*;
import facecat.topin.core.*;
import java.util.*;

/**
 * 多页夹控件
 */
public class FCTabPage extends FCDiv implements FCTouchEvent {

    /**
     * 创建多页夹
     */
    public FCTabPage() {
    }

    protected FCButton m_headerButton = null;

    /**
     * 获取页头按钮
     */
    public FCButton getHeaderButton() {
        return m_headerButton;
    }

    /**
     * 设置页头按钮
     */
    public void setHeaderButton(FCButton value) {
        m_headerButton = value;
    }

    protected FCPoint m_headerLocation = new FCPoint();

    /**
     * 获取头部的位置
     */
    public FCPoint getHeaderLocation() {
        return m_headerLocation.clone();
    }

    /**
     * 设置头部的位置
     */
    public void setHeaderLocation(FCPoint value) {
        m_headerLocation = value.clone();
    }

    /**
     * 获取头部是否可见
     */
    public boolean isHeaderVisible() {
        if (m_headerButton != null) {
            return m_headerButton.isVisible();
        } else {
            return false;
        }
    }

    /**
     * 设置头部是否可见
     */
    public void setHeaderVisible(boolean value) {
        if (m_headerButton != null) {
            m_headerButton.setVisible(value);
        }
    }

    protected FCTabControl m_tabControl = null;

    /**
     * 获取多页夹控件
     */
    public FCTabControl getTabControl() {
        return m_tabControl;
    }

    /**
     * 设置多页夹控件
     */
    public void setTabControl(FCTabControl value) {
        m_tabControl = value;
    }

    @Override
    public void callEvent(int eventID, Object sender) {
        super.callEvent(eventID, sender);
        if (sender == m_headerButton) {
            if (eventID == FCEventID.DRAGBEGIN) {
                if (m_tabControl != null) {
                    m_tabControl.onDragTabHeaderBegin(this);
                }
            } else if (eventID == FCEventID.DRAGEND) {
                if (m_tabControl != null) {
                    m_tabControl.onDragTabHeaderEnd(this);
                }
            } else if (eventID == FCEventID.DRAGGING) {
                if (m_tabControl != null) {
                    m_tabControl.onDraggingTabHeader(this);
                }
            }
        }
    }

    @Override
    public void callControlTouchEvent(int eventID, Object sender, FCTouchInfo touchInfo) {
        if (sender == m_headerButton) {
            if (eventID == FCEventID.TOUCHDOWN) {
                if (m_tabControl != null) {
                    m_tabControl.setSelectedTabPage(this);
                    m_tabControl.invalidate();
                }
            }
        }
    }

    /**
     * 销毁方法
     */
    @Override
    public void delete() {
        if (!isDeleted()) {
            if (m_headerButton != null) {
                m_headerButton.removeEvent(this, FCEventID.DRAGBEGIN);
                m_headerButton.removeEvent(this, FCEventID.DRAGEND);
                m_headerButton.removeEvent(this, FCEventID.DRAGGING);
                m_headerButton.removeEvent(this, FCEventID.TOUCHDOWN);
            }
            m_headerButton = null;
        }
        super.delete();
    }

    /**
     * 获取控件类型
     */
    @Override
    public String getControlType() {
        return "TabPage";
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
        if (name.equals("headersize")) {
            type.argvalue = "size";
            if (m_headerButton != null) {
                value.argvalue = FCStr.convertSizeToStr(m_headerButton.getSize());
            } else {
                value.argvalue = "0,0";
            }
        } else if (name.equals("headervisible")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(isHeaderVisible());
        } else if (name.indexOf("header-") != - 1) {
            if (m_headerButton != null) {
                m_headerButton.getProperty(name.substring(7), value, type);
            }
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
        propertyNames.addAll(Arrays.asList(new String[]{"Header", "HeaderSize", "HeaderVisible"}));
        return propertyNames;
    }

    /**
     * 添加控件方法
     */
    @Override
    public void onLoad() {
        super.onLoad();
        if (m_tabControl != null) {
            if (m_headerButton == null) {
                FCHost host = getNative().getHost();
                FCView tempVar = host.createInternalControl(this, "headerbutton");
                m_headerButton = (FCButton) ((tempVar instanceof FCButton) ? tempVar : null);
                m_headerButton.addEvent(this, FCEventID.DRAGBEGIN);
                m_headerButton.addEvent(this, FCEventID.DRAGEND);
                m_headerButton.addEvent(this, FCEventID.DRAGGING);
                m_headerButton.addEvent(this, FCEventID.TOUCHDOWN);
            }
            if (!m_tabControl.containsControl(m_headerButton)) {
                m_tabControl.addControl(m_headerButton);
            }
        }
    }

    /**
     * 文字改变方法
     */
    @Override
    public void onTextChanged() {
        super.onTextChanged();
        if (m_headerButton != null) {
            m_headerButton.setText(getText());
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
        if (name.equals("headersize")) {
            if (m_headerButton != null) {
                m_headerButton.setProperty("size", value);
            }
        } else if (name.equals("headervisible")) {
            setHeaderVisible(FCStr.convertStrToBool(value));
        } else if (name.indexOf("header-") != -1) {
            if (m_headerButton != null) {
                m_headerButton.setProperty(name.substring(7), value);
            }
        } else {
            super.setProperty(name, value);
        }
    }
}
