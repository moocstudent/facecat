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
 * 提示标签
 */
public class FCToolTip extends FCView {

    /**
     * 创建标签
     */
    public FCToolTip() {
        setAutoSize(true);
        setBackColor(FCColor.argb(255, 255, 40));
        setBorderColor(FCColor.Border);
        setTopMost(true);
        setVisible(false);
    }

    /**
     * 上一次触摸的位置
     */
    private FCPoint m_lastTouchPoint;

    /**
     * 秒表ID
     */
    private int m_timerID = getNewTimerID();

    /**
     * 剩余保留显示毫秒数
     */
    protected int m_remainAutoPopDelay;

    /**
     * 剩余延迟显示毫秒数
     */
    protected int m_remainInitialDelay;

    protected int m_autoPopDelay = 5000;

    /**
     * 提示显示的时间长度
     */
    public int autoPopDelay() {
        return m_autoPopDelay;
    }

    /**
     * 保留显示的时间长度
     */
    public void setAutoPopDelay(int autoPopDelay) {
        m_autoPopDelay = autoPopDelay;
    }

    protected int m_initialDelay = 500;

    /**
     * 获取触摸静止时延迟显示的毫秒数
     */
    public int initialDelay() {
        return m_initialDelay;
    }

    /**
     * 设置触摸静止时延迟显示的毫秒数
     */
    public void setinitialDelay(int initialDelay) {
        m_initialDelay = initialDelay;
    }

    protected boolean m_showAlways;

    /**
     * 获取是否总是显示
     */
    public boolean showAlways() {
        return m_showAlways;
    }

    /**
     * 设置是否总是显示
     */
    public void setshowAlways(boolean showAlways) {
        m_showAlways = showAlways;
    }

    protected boolean m_useAnimation;

    /**
     * 获取是否在显示隐藏时使用动画
     */
    public boolean useAnimation() {
        return m_useAnimation;
    }

    /**
     * 设置是否在显示隐藏时使用动画
     */
    public void setuseAnimation(boolean useAnimation) {
        m_useAnimation = useAnimation;
    }

    /**
     * 销毁控件方法
     */
    @Override
    public void delete() {
        if (!isDeleted()) {
            stopTimer(m_timerID);
        }
        super.delete();
    }

    /**
     * 获取控件类型
     */
    @Override
    public String getControlType() {
        return "ToolTip";
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
        if (name.equals("autopopupdelay")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(autoPopDelay());
        } else if (name.equals("initialdelay")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(initialDelay());
        } else if (name.equals("showalways")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(showAlways());
        } else if (name.equals("useanimation")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(useAnimation());
        } else {
            super.getProperty(name, value, type);
        }
    }

    /**
     * 获取属性名称列表
     */
    public ArrayList<String> getPropertyNames() {
        ArrayList<String> propertyNames = super.getPropertyNames();
        propertyNames.addAll(Arrays.asList(new String[]{"AutoPopupDelay", "InitialDelay", "ShowAlways", "UseAnimation"}));
        return propertyNames;
    }


    /**
     * 隐藏控件
     */
    @Override
    public void hide() {
        setVisible(false);
    }

    /**
     * 添加控件方法
     */
    @Override
    public void onLoad() {
        super.onLoad();
        m_lastTouchPoint = getTouchPoint();
        startTimer(m_timerID, 10);
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
            FCPoint mp = getTouchPoint();
            if (!m_showAlways) {
                if (m_lastTouchPoint.x != mp.x || m_lastTouchPoint.y != mp.y) {
                    setVisible(false);
                }
            }
            m_lastTouchPoint = mp;
            if (m_remainAutoPopDelay > 0) {
                m_remainAutoPopDelay -= 10;
                if (m_remainAutoPopDelay <= 0) {
                    setVisible(false);
                }
            }
            if (m_remainInitialDelay > 0) {
                m_remainInitialDelay -= 10;
                if (m_remainInitialDelay <= 0) {
                    setVisible(true);
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
        if (m_native != null) {
            if (isVisible()) {
                m_native.addControl(this);
                m_remainAutoPopDelay = m_autoPopDelay;
                m_remainInitialDelay = 0;
            } else {
                m_native.removeControl(this);
                startTimer(m_timerID, 10);
                m_remainAutoPopDelay = 0;
                m_remainInitialDelay = 0;
            }
            getNative().invalidate();
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
        if (name.equals("autopopupdelay")) {
            setAutoPopDelay(FCStr.convertStrToInt(value));
        } else if (name.equals("initialdelay")) {
            setinitialDelay(FCStr.convertStrToInt(value));
        } else if (name.equals("showalways")) {
            setshowAlways(FCStr.convertStrToBool(value));
        } else if (name.equals("useanimation")) {
            setuseAnimation(FCStr.convertStrToBool(value));
        } else {
            super.setProperty(name, value);
        }
    }

    /**
     * 显示控件
     */
    public void show() {
        m_remainAutoPopDelay = 0;
        m_remainInitialDelay = m_initialDelay;
        setVisible(m_initialDelay == 0);
        getNative().invalidate();
    }
}
