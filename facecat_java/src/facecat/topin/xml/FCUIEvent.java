/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.xml;

import facecat.topin.div.*;
import facecat.topin.core.*;
import facecat.topin.grid.*;

/**
 * 事件
 *
 */
public class FCUIEvent implements FCEvent, FCTouchEvent, FCInvokeEvent, FCKeyEvent, FCPaintEvent, FCTimerEvent,
        FCGridCellEvent, FCGridCellTouchEvent, FCMenuItemTouchEvent, FCWindowClosingEvent {

    /**
     * 创建事件
     *
     * @param xml XML对象
     */
    public FCUIEvent(FCUIXml xml) {
        m_xml = xml;
    }

    /**
     * 事件集合
     *
     */
    protected java.util.HashMap<FCView, FCEventInfo> m_events = new java.util.HashMap<FCView, FCEventInfo>();

    private boolean m_isDeleted = false;

    /**
     * 获取是否被销毁
     *
     */
    public final boolean isDeleted() {
        return m_isDeleted;
    }

    private FCUIScript m_script;

    /**
     * 获取或设置脚本
     *
     */
    public final FCUIScript getScript() {
        return m_script;
    }

    public final void setScript(FCUIScript value) {
        m_script = value;
    }

    private String m_sender;

    /**
     * 获取或设置调用者
     *
     */
    public final String getSender() {
        return m_sender;
    }

    public final void setSender(String value) {
        m_sender = value;
    }

    private FCUIXml m_xml;

    /**
     * 获取或设置XML对象
     *
     */
    public final FCUIXml getXml() {
        return m_xml;
    }

    public final void setXml(FCUIXml value) {
        m_xml = value;
    }

    /**
     * 销毁方法
     *
     */
    public void delete() {
        if (!m_isDeleted) {
            m_isDeleted = true;
        }
    }

    /**
     * 控件事件
     *
     * @param eventID 事件ID
     * @param sender 调用者
     */
    public void callEvent(int eventID, Object sender) {
        callFunction(sender, eventID);
    }

    /**
     * 调用事件
     *
     * @param eventID 事件ID
     * @param sender 调用者
     * @param args 数据
     */
    public void callControlInvokeEvent(int eventID, Object sender, Object args) {
        callFunction(sender, eventID);
    }

    /**
     * 按键事件
     *
     * @param eventID 事件ID
     * @param sender 调用者
     * @param key 按键
     */
    public void callControlKeyEvent(int eventID, Object sender, char key) {
        callFunction(sender, eventID);
    }

    /**
     * 鼠标事件
     *
     * @param eventID 事件ID
     * @param sender 调用者
     * @param touchInfo 触摸信息
     */
    public void callControlTouchEvent(int eventID, Object sender, FCTouchInfo touchInfo) {
        callFunction(sender, eventID);
    }

    /**
     * 重绘事件
     *
     * @param eventID 事件ID
     * @param sender 调用者
     * @param paint 绘图对象
     * @param clipRect 裁剪区域
     */
    public void callControlPaintEvent(int eventID, Object sender, FCPaint paint, FCRect clipRect) {
        callFunction(sender, eventID);
    }

    /**
     * 秒表事件
     *
     * @param eventID 事件ID
     * @param sender 调用者
     * @param timerID 秒表ID
     */
    public void callControlTimerEvent(int eventID, Object sender, int timerID) {
        callFunction(sender, eventID);
    }

    /**
     * 单元格事件
     *
     * @param eventID 事件ID
     * @param sender 调用者
     * @param cell 单元格
     */
    public void callGridCellEvent(int eventID, Object sender, FCGridCell cell) {
        callFunction(sender, eventID);
    }

    /**
     * 单元格鼠标事件
     *
     * @param eventID 事件ID
     * @param sender 调用者
     * @param cell 单元格
     * @param touchInfo 触摸信息
     */
    public void callGridCellTouchEvent(int eventID, Object sender, FCGridCell cell, FCTouchInfo touchInfo) {
        callFunction(sender, eventID);
    }

    /**
     * 菜单鼠标事件
     *
     * @param eventID 事件ID
     * @param sender 调用者
     * @param item 菜单项
     * @param touchInfo 触摸信息
     */
    public void callMenuItemTouchEvent(int eventID, Object sender, FCMenuItem item, FCTouchInfo touchInfo) {
        callFunction(sender, eventID);
    }

    /**
     * 窗体关闭事件
     *
     * @param eventID 事件ID
     * @param sender 调用者
     * @param cancel 是否不处理
     */
    public void callWindowClosingEvent(int eventID, Object sender, RefObject<Boolean> cancel) {
        callFunction(sender, eventID);
    }

    /**
     * 调用方法
     *
     * @param sender 调用者
     * @param eventID 事件ID
     */
    public String callFunction(Object sender, int eventID) {
        FCView control = (FCView) ((sender instanceof FCView) ? sender : null);
        if (m_events.containsKey(control)) {
            FCEventInfo eventInfo = m_events.get(control);
            String function = eventInfo.getFunction(eventID);
            if (function != null && function.length() > 0) {
                FCUIScript script = m_xml.getScript();
                if (script != null) {
                    m_sender = control.getName();
                    String result = script.callFunction(function);
                    m_sender = "";
                    return result;
                }
            }
        }
        return null;
    }

    /**
     * 获取事件的ID
     *
     * @param eventName 事件名称
     * @return 事件的ID
     */
    public int getEventID(String eventName) {
        String lowerName = eventName.toLowerCase();
        if (lowerName.equals("onadd")) {
            return FCEventID.ADD;
        } else if (lowerName.equals("onbackcolorchanged")) {
            return FCEventID.BACKCOLORCHANGED;
        } else if (lowerName.equals("onbackimagechanged")) {
            return FCEventID.BACKIMAGECHANGED;
        } else if (lowerName.equals("onchar")) {
            return FCEventID.CHAR;
        } else if (lowerName.equals("oncheckedchanged")) {
            return FCEventID.CHECKEDCHANGED;
        } else if (lowerName.equals("onclick")) {
            return FCEventID.CLICK;
        } else if (lowerName.equals("oncopy")) {
            return FCEventID.COPY;
        } else if (lowerName.equals("oncut")) {
            return FCEventID.CUT;
        } else if (lowerName.equals("ondockchanged")) {
            return FCEventID.DOCKCHANGED;
        } else if (lowerName.equals("ondoubleclick")) {
            return FCEventID.DOUBLECLICK;
        } else if (lowerName.equals("ondragbegin")) {
            return FCEventID.DRAGBEGIN;
        } else if (lowerName.equals("ondragend")) {
            return FCEventID.DRAGEND;
        } else if (lowerName.equals("ondragging")) {
            return FCEventID.DRAGGING;
        } else if (lowerName.equals("onenablechanged")) {
            return FCEventID.ENABLECHANGED;
        } else if (lowerName.equals("onfontchanged")) {
            return FCEventID.FONTCHANGED;
        } else if (lowerName.equals("ontextcolorchanged")) {
            return FCEventID.TEXTCOLORCHANGED;
        } else if (lowerName.equals("ongotfocus")) {
            return FCEventID.GOTFOCUS;
        } else if (lowerName.equals("ongridcellclick")) {
            return FCEventID.GRIDCELLCLICK;
        } else if (lowerName.equals("ongridcelleditbegin")) {
            return FCEventID.GRIDCELLEDITBEGIN;
        } else if (lowerName.equals("ongridcelleditend")) {
            return FCEventID.GRIDCELLEDITEND;
        } else if (lowerName.equals("ongridcelltouchdown")) {
            return FCEventID.GRIDCELLTOUCHDOWN;
        } else if (lowerName.equals("ongridcelltouchmove")) {
            return FCEventID.GRIDCELLTOUCHMOVE;
        } else if (lowerName.equals("ongridcelltouchup")) {
            return FCEventID.GRIDCELLTOUCHUP;
        } else if (lowerName.equals("oninvoke")) {
            return FCEventID.INVOKE;
        } else if (lowerName.equals("onkeydown")) {
            return FCEventID.KEYDOWN;
        } else if (lowerName.equals("onkeyup")) {
            return FCEventID.KEYUP;
        } else if (lowerName.equals("onload")) {
            return FCEventID.LOAD;
        } else if (lowerName.equals("onlocationchanged")) {
            return FCEventID.LOCATIONCHANGED;
        } else if (lowerName.equals("onlostfocus")) {
            return FCEventID.LOSTFOCUS;
        } else if (lowerName.equals("onmarginchanged")) {
            return FCEventID.MARGINCHANGED;
        } else if (lowerName.equals("onmenuitemclick")) {
            return FCEventID.MENUITEMCLICK;
        } else if (lowerName.equals("ontouchdown")) {
            return FCEventID.TOUCHDOWN;
        } else if (lowerName.equals("onTouchEnter")) {
            return FCEventID.TOUCHENTER;
        } else if (lowerName.equals("ontouchleave")) {
            return FCEventID.TOUCHLEAVE;
        } else if (lowerName.equals("ontouchmove")) {
            return FCEventID.TOUCHMOVE;
        } else if (lowerName.equals("ontouchup")) {
            return FCEventID.TOUCHUP;
        } else if (lowerName.equals("ontouchwheel")) {
            return FCEventID.TOUCHWHEEL;
        } else if (lowerName.equals("onpaddingchanged")) {
            return FCEventID.PADDINGCHANGED;
        } else if (lowerName.equals("onpaint")) {
            return FCEventID.PAINT;
        } else if (lowerName.equals("onpaintborder")) {
            return FCEventID.PAINTBORDER;
        } else if (lowerName.equals("onparentchanged")) {
            return FCEventID.PARENTCHANGED;
        } else if (lowerName.equals("onPaste")) {
            return FCEventID.PASTE;
        } else if (lowerName.equals("onregionchanged")) {
            return FCEventID.REGIONCHANGED;
        } else if (lowerName.equals("onremove")) {
            return FCEventID.REMOVE;
        } else if (lowerName.equals("onselecteddaychanged")) {
            return FCEventID.SELECTEDDAYCHANGED;
        } else if (lowerName.equals("onselectedindexchanged")) {
            return FCEventID.SELECTEDINDEXCHANGED;
        } else if (lowerName.equals("onselectedtabpagechanged")) {
            return FCEventID.SELECTEDTABPAGECHANGED;
        } else if (lowerName.equals("onsizechanged")) {
            return FCEventID.SIZECHANGED;
        } else if (lowerName.equals("ontabindexchanged")) {
            return FCEventID.TABINDEXCHANGED;
        } else if (lowerName.equals("ontabstop")) {
            return FCEventID.TABSTOP;
        } else if (lowerName.equals("ontextchanged")) {
            return FCEventID.TEXTCHANGED;
        } else if (lowerName.equals("ontimer")) {
            return FCEventID.TIMER;
        } else if (lowerName.equals("onvaluechanged")) {
            return FCEventID.VALUECHANGED;
        } else if (lowerName.equals("onvisiblechanged")) {
            return FCEventID.VISIBLECHANGED;
        } else if (lowerName.equals("onscrolled")) {
            return FCEventID.SCROLLED;
        } else if (lowerName.equals("onwindowclosed")) {
            return FCEventID.WINDOWCLOSED;
        }
        return -1;
    }

    /**
     * 注册事件
     *
     * @param control 控件
     * @param eventName 事件名称
     * @param function 方法
     */
    public void registerEvent(FCView control, String eventName, String function) {
        int eventID = getEventID(eventName);
        if (eventID != -1) {
            FCEventInfo eventInfo = null;
            if (m_events.containsKey(control)) {
                eventInfo = m_events.get(control);
            } else {
                eventInfo = new FCEventInfo();
                m_events.put(control, eventInfo);
            }
            eventInfo.addEvent(eventID, function);
            control.addEvent(this, eventID);
        }
    }
}
