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
    /// 提示标签
    /// </summary>
    public class FCToolTip : FCLabel {
        /// <summary>
        /// 创建标签
        /// </summary>
        public FCToolTip() {
            AutoSize = true;
            BackColor = FCColor.argb(255, 255, 40);
            BorderColor = FCColor.Border;
            TopMost = true;
            Visible = false;
        }

        /// <summary>
        /// 上一次触摸的位置
        /// </summary>
        private FCPoint m_lastTouchPoint;

        /// <summary>
        /// 秒表ID
        /// </summary>
        private int m_timerID = getNewTimerID();

        /// <summary>
        /// 剩余保留显示毫秒数
        /// </summary>
        protected int m_remainAutoPopDelay;

        /// <summary>
        /// 剩余延迟显示毫秒数
        /// </summary>
        protected int m_remainInitialDelay;

        protected int m_autoPopDelay = 5000;

        /// <summary>
        /// 提示保留显示的时间长度
        /// </summary>
        public virtual int AutoPopDelay {
            get { return m_autoPopDelay; }
            set { m_autoPopDelay = value; }
        }

        protected int m_initialDelay = 500;

        /// <summary>
        /// 获取或设置触摸静止时延迟显示的毫秒数
        /// </summary>
        public virtual int InitialDelay {
            get { return m_initialDelay; }
            set { m_initialDelay = value; }
        }

        protected bool m_showAlways;

        /// <summary>
        /// 获取或设置是否总是显示
        /// </summary>
        public virtual bool ShowAlways {
            get { return m_showAlways; }
            set { m_showAlways = value; }
        }

        protected bool m_useAnimation;

        /// <summary>
        /// 获取或设置是否在显示隐藏时使用动画
        /// </summary>
        public virtual bool UseAnimation {
            get { return m_useAnimation; }
            set { m_useAnimation = value; }
        }

        /// <summary>
        /// 销毁控件方法
        /// </summary>
        public override void delete() {
            if (!IsDeleted) {
                stopTimer(m_timerID);
            }
            base.delete();
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <returns></returns>
        public override String getControlType() {
            return "ToolTip";
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">返回属性值</param>
        /// <param name="type">返回属性类型</param>
        public override void getProperty(String name, ref String value, ref String type) {
            if (name == "autopopupdelay") {
                type = "int";
                value = FCStr.convertIntToStr(AutoPopDelay);
            }
            else if (name == "initialdelay") {
                type = "int";
                value = FCStr.convertIntToStr(InitialDelay);
            }
            else if (name == "showalways") {
                type = "bool";
                value = FCStr.convertBoolToStr(ShowAlways);
            }
            else if (name == "useanimation") {
                type = "bool";
                value = FCStr.convertBoolToStr(UseAnimation);
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
            propertyNames.AddRange(new String[] { "AutoPopupDelay", "InitialDelay", "ShowAlways", "UseAnimation" });
            return propertyNames;
        }

        /// <summary>
        /// 隐藏控件
        /// </summary>
        public override void hide() {
            Visible = false;
        }

        /// <summary>
        /// 添加控件方法
        /// </summary>
        public override void onLoad() {
            base.onLoad();
            m_lastTouchPoint = TouchPoint;
            startTimer(m_timerID, 10);
        }

        /// <summary>
        /// 秒表方法
        /// </summary>
        /// <param name="timerID">秒表ID</param>
        public override void onTimer(int timerID) {
            base.onTimer(timerID);
            if (m_timerID == timerID) {
                FCPoint mp = TouchPoint;
                if (!m_showAlways) {
                    if (m_lastTouchPoint.x != mp.x || m_lastTouchPoint.y != mp.y) {
                        Visible = false;
                    }
                }
                m_lastTouchPoint = mp;
                if (m_remainAutoPopDelay > 0) {
                    m_remainAutoPopDelay -= 10;
                    if (m_remainAutoPopDelay <= 0) {
                        Visible = false;
                    }
                }
                if (m_remainInitialDelay > 0) {
                    m_remainInitialDelay -= 10;
                    if (m_remainInitialDelay <= 0) {
                        Visible = true;
                    }
                }
            }
        }

        /// <summary>
        /// 可见状态改变方法
        /// </summary>
        public override void onVisibleChanged() {
            base.onVisibleChanged();
            if (m_native != null) {
                if (Visible) {
                    m_native.addControl(this);
                    m_remainAutoPopDelay = m_autoPopDelay;
                    m_remainInitialDelay = 0;
                }
                else {
                    m_native.removeControl(this);
                    startTimer(m_timerID, 10);
                    m_remainAutoPopDelay = 0;
                    m_remainInitialDelay = 0;
                }
                Native.invalidate();
            }
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name">属性名</param>
        /// <param name="value">属性值</param>
        public override void setProperty(String name, String value) {
            if (name == "autopopupdelay") {
                AutoPopDelay = FCStr.convertStrToInt(value);
            }
            else if (name == "initialdelay") {
                InitialDelay = FCStr.convertStrToInt(value);
            }
            else if (name == "showalways") {
                ShowAlways = FCStr.convertStrToBool(value);
            }
            else if (name == "useanimation") {
                UseAnimation = FCStr.convertStrToBool(value);
            }
            else {
                base.setProperty(name, value);
            }
        }

        /// <summary>
        /// 显示控件
        /// </summary>
        public override void show() {
            m_remainAutoPopDelay = 0;
            m_remainInitialDelay = m_initialDelay;
            Visible = m_initialDelay == 0;
            Native.invalidate();
        }
    }
}
