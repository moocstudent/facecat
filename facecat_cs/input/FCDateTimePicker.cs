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
    /// 日期控件
    /// </summary>
    public class FCDateTimePicker : FCTextBox {
        /// <summary>
        /// 创建日期控件
        /// </summary>
        public FCDateTimePicker() {
            m_dropDownButtonTouchDownEvent = new FCTouchEvent(DropDownButtonTouchDown);
            m_selectedTimeChangedEvent = new FCEvent(selectedTimeChanged);
        }

        /// <summary>
        /// 下拉按钮点击函数指针
        /// </summary>
        private FCTouchEvent m_dropDownButtonTouchDownEvent;

        /// <summary>
        /// 选中日期改变函数指针
        /// </summary>
        private FCEvent m_selectedTimeChangedEvent;

        protected FCCalendar m_calendar;

        /// <summary>
        /// 获取日历
        /// </summary>
        public virtual FCCalendar Calendar {
            get { return m_calendar; }
        }

        protected String m_customFormat = "yyyy-MM-dd";

        /// <summary>
        /// 获取或设置日期格式
        /// </summary>
        public virtual String CustomFormat {
            get { return m_customFormat; }
            set { m_customFormat = value; }
        }

        protected FCButton m_dropDownButton;

        /// <summary>
        /// 获取下拉按钮
        /// </summary>
        public virtual FCButton DropDownButton {
            get { return m_dropDownButton; }
        }

        protected FCMenu m_dropDownMenu;

        /// <summary>
        /// 获取下拉菜单
        /// </summary>
        public virtual FCMenu DropDownMenu {
            get { return m_dropDownMenu; }
        }

        protected bool m_showTime = true;

        /// <summary>
        /// 获取或设置是否显示时间
        /// </summary>
        public virtual bool ShowTime {
            get { return m_showTime; }
            set { m_showTime = value; }
        }

        /// <summary>
        /// 创建日历
        /// </summary>
        /// <returns></returns>
        public virtual FCCalendar CreateCalendar() {
            FCCalendar calendar = new FCCalendar();
            calendar.Dock = FCDockStyle.Fill;
            return calendar;
        }

        /// <summary>
        /// 销毁方法
        /// </summary>
        public override void delete() {
            if (!IsDeleted) {
                if (m_calendar != null) {
                    if (m_selectedTimeChangedEvent != null) {
                        m_calendar.removeEvent(m_selectedTimeChangedEvent, FCEventID.SELECTEDTIMECHANGED);
                        m_selectedTimeChangedEvent = null;
                    }
                }
                if (m_dropDownButton != null) {
                    if (m_dropDownButtonTouchDownEvent != null) {
                        m_dropDownButton.removeEvent(m_dropDownButtonTouchDownEvent, FCEventID.TOUCHDOWN);
                        m_dropDownButtonTouchDownEvent = null;
                    }
                }
                if (m_dropDownMenu != null) {
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
        private void DropDownButtonTouchDown(object sender, FCTouchInfo touchInfo) {
            onDropDownOpening();
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <returns>控件类型</returns>
        public override String getControlType() {
            return "DateTimePicker";
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        /// <param name="type">类型</param>
        public override void getProperty(String name, ref String value, ref String type) {
            if (name == "customformat") {
                type = "string";
                value = CustomFormat;
            }
            else if (name == "showtime") {
                type = "bool";
                value = FCStr.convertBoolToStr(ShowTime);
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
            propertyNames.add("CustomFormat");
            propertyNames.add("ShowTime");
            return propertyNames;
        }

        /// <summary>
        /// 下拉菜单显示方法
        /// </summary>
        public virtual void onDropDownOpening() {
            //创建下拉菜单及日历
            if (m_dropDownMenu == null) {
                FCHost host = Native.Host;
                m_dropDownMenu = host.createInternalControl(this, "dropdownmenu") as FCMenu;
                Native.addControl(m_dropDownMenu);
                if (m_calendar == null) {
                    m_calendar = CreateCalendar();
                    m_dropDownMenu.addControl(m_calendar);
                    m_calendar.Size = m_dropDownMenu.Size;
                    m_calendar.addEvent(m_selectedTimeChangedEvent, FCEventID.SELECTEDTIMECHANGED);
                }
            }
            if (m_calendar != null && !m_showTime) {
                m_calendar.TimeDiv.Height = 0;
            }
            m_dropDownMenu.Native = Native;
            FCPoint nativePoint = pointToNative(new FCPoint(0, Height));
            m_dropDownMenu.Location = nativePoint;
            m_dropDownMenu.Visible = true;
            if (m_calendar != null) {
                m_calendar.Mode = FCCalendarMode.Day;
            }
            m_dropDownMenu.bringToFront();
            m_dropDownMenu.invalidate();
        }

        /// <summary>
        /// 添加控件方法
        /// </summary>
        public override void onLoad() {
            base.onLoad();
            if (m_dropDownButton == null) {
                FCHost host = Native.Host;
                m_dropDownButton = host.createInternalControl(this, "dropdownbutton") as FCButton;
                addControl(m_dropDownButton);
                m_dropDownButton.addEvent(m_dropDownButtonTouchDownEvent, FCEventID.TOUCHDOWN);
            }
            if (m_dropDownMenu != null) {
                m_dropDownMenu.Native = Native;
            }
        }

        /// <summary>
        /// 数值改变方法
        /// </summary>
        public virtual void onSelectedTimeChanged() {
            callEvents(FCEventID.SELECTEDTIMECHANGED);
            if (m_calendar != null) {
                CDay selectedDay = m_calendar.SelectedDay;
                if (selectedDay != null) {
                    DateTime date = new DateTime(selectedDay.Year, selectedDay.Month, selectedDay.Day, m_calendar.TimeDiv.Hour,
                        m_calendar.TimeDiv.Minute, m_calendar.TimeDiv.Second);
                    Text = date.ToString(m_customFormat);
                    invalidate();
                }
            }
        }

        /// <summary>
        /// 选中日期改变事件回调方法
        /// </summary>
        /// <param name="sender">调用者</param>
        private void selectedTimeChanged(object sender) {
            onSelectedTimeChanged();
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        public override void setProperty(String name, String value) {
            if (name == "customformat") {
                CustomFormat = value;
            }
            else if (name == "showtime") {
                ShowTime = FCStr.convertStrToBool(value);
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
                Padding = new FCPadding(0, 0, dWidth, 0);
                m_dropDownButton.Location = new FCPoint(width - dWidth, 0);
                m_dropDownButton.Size = new FCSize(dWidth, height);
            }
        }
    }
}
