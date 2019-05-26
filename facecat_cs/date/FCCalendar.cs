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
    /// 日历模式
    /// </summary>
    public enum FCCalendarMode {
        /// <summary>
        /// 选择日
        /// </summary>
        Day,
        /// <summary>
        /// 选择月
        /// </summary>
        Month,
        /// <summary>
        /// 选择年
        /// </summary>
        Year
    }

    /// <summary>
    /// 日历控件
    /// </summary>
    public partial class FCCalendar : FCView {
        /// <summary>
        /// 创建日历控件
        /// </summary>
        public FCCalendar() {
            m_years = new CYears();
            FCSize size = new FCSize(200, 200);
            Size = size;
        }

        /// <summary>
        /// 当前的月份
        /// </summary>
        protected int m_month;

        /// <summary>
        /// 秒表ID
        /// </summary>
        private int m_timerID = getNewTimerID();

        /// <summary>
        /// 当前的年份
        /// </summary>
        protected int m_year;

        protected DayDiv m_dayDiv;

        /// <summary>
        /// 获取或设置日期层
        /// </summary>
        public virtual DayDiv DayDiv {
            get { return m_dayDiv; }
            set { m_dayDiv = value; }
        }

        protected HeadDiv m_headDiv;

        /// <summary>
        /// 获取或设置头部层
        /// </summary>
        public virtual HeadDiv HeadDiv {
            get { return m_headDiv; }
            set { m_headDiv = value; }
        }

        protected FCCalendarMode m_mode = FCCalendarMode.Day;

        /// <summary>
        /// 获取或设置日历的模式
        /// </summary>
        public virtual FCCalendarMode Mode {
            get { return m_mode; }
            set {
                if (m_mode != value) {
                    FCCalendarMode oldMode = m_mode;
                    m_mode = value;
                    //月
                    if (m_mode == FCCalendarMode.Month) {
                        if (m_dayDiv != null) {
                            m_dayDiv.hide();
                        }
                        if (m_yearDiv != null) {
                            m_yearDiv.hide();
                        }
                        if (m_monthDiv == null) {
                            m_monthDiv = new MonthDiv(this);
                        }
                        if (oldMode == FCCalendarMode.Day) {
                            m_monthDiv.selectYear(m_year);
                        }
                        m_monthDiv.show();
                    }
                    //年
                    else if (m_mode == FCCalendarMode.Year) {
                        if (m_dayDiv != null) {
                            m_dayDiv.hide();
                        }
                        int startYear = m_year;
                        if (m_monthDiv != null) {
                            startYear = m_monthDiv.Year;
                            m_monthDiv.hide();
                        }
                        if (m_yearDiv == null) {
                            m_yearDiv = new YearDiv(this);
                        }
                        m_yearDiv.selectStartYear(startYear);
                        m_yearDiv.show();
                    }
                    else {
                        if (m_monthDiv != null) {
                            m_monthDiv.hide();
                        }
                        if (m_yearDiv != null) {
                            m_yearDiv.hide();
                        }
                        m_dayDiv.show();
                    }
                }
            }
        }

        /// <summary>
        /// 获取或设置月份
        /// </summary>
        public virtual CMonth Month {
            get { return m_years.getYear(m_year).Months.get(m_month); }
            set {
                m_year = value.Year;
                m_month = value.Month;
                update();
            }
        }

        /// <summary>
        /// 月份层
        /// </summary>
        protected MonthDiv m_monthDiv;

        /// <summary>
        /// 获取或设置月份层
        /// </summary>
        public virtual MonthDiv MonthDiv {
            get { return m_monthDiv; }
            set { m_monthDiv = value; }
        }

        protected CDay m_selectedDay;

        /// <summary>
        /// 获取选中日期
        /// </summary>
        public virtual CDay SelectedDay {
            set {
                if (m_selectedDay != value) {
                    m_selectedDay = value;
                    if (m_dayDiv != null) {
                        m_dayDiv.selectedDay(m_selectedDay);
                    }
                    invalidate();
                    onSelectedTimeChanged();
                }
            }
            get { return m_selectedDay; }
        }

        protected TimeDiv m_timeDiv;

        /// <summary>
        /// 获取或设置时间层
        /// </summary>
        public virtual TimeDiv TimeDiv {
            get { return m_timeDiv; }
            set { m_timeDiv = value; }
        }

        protected bool m_useAnimation;

        /// <summary>
        /// 获取或设置是否使用动画
        /// </summary>
        public virtual bool UseAnimation {
            get { return m_useAnimation; }
            set {
                m_useAnimation = value;
                if (m_useAnimation) {
                    startTimer(m_timerID, 20);
                }
                else {
                    stopTimer(m_timerID);
                }
            }
        }

        protected YearDiv m_yearDiv;

        /// <summary>
        /// 获取或设置年份层
        /// </summary>
        public virtual YearDiv YearDiv {
            get { return m_yearDiv; }
            set { m_yearDiv = value; }
        }

        protected CYears m_years;

        /// <summary>
        /// 获取或设置日历
        /// </summary>
        public virtual CYears Years {
            get { return m_years; }
            set {
                m_years = value;
                update();
                SelectedDay = Month.Days.get(1);
            }
        }

        /// <summary>
        /// 销毁事件
        /// </summary>
        public override void delete() {
            if (!IsDeleted) {
                if (m_dayDiv != null) {
                    m_dayDiv.delete();
                }
                if (m_monthDiv != null) {
                    m_monthDiv.delete();
                }
                if (m_yearDiv != null) {
                    m_yearDiv.delete();
                }
                stopTimer(m_timerID);
            }
            base.delete();
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <returns>控件类型</returns>
        public override String getControlType() {
            return "Calendar";
        }

        /// <summary>
        /// 获取事件名称列表
        /// </summary>
        /// <returns>名称列表</returns>
        public override ArrayList<String> getEventNames() {
            ArrayList<String> eventNames = base.getEventNames();
            eventNames.AddRange(new String[] { "SelectedTimeChanged" });
            return eventNames;
        }

        /// <summary>
        /// 根据年月获取上个月
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <returns>上个月</returns>
        public CMonth getLastMonth(int year, int month) {
            int lastMonth = month - 1;
            int lastYear = year;
            if (lastMonth == 0) {
                lastMonth = 12;
                lastYear -= 1;
            }
            return m_years.getYear(lastYear).Months.get(lastMonth);
        }

        /// <summary>
        /// 获取下个月
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <returns>下个月</returns>
        public CMonth getNextMonth(int year, int month) {
            int nextMonth = month + 1;
            int nextYear = year;
            if (nextMonth == 13) {
                nextMonth = 1;
                nextYear += 1;
            }
            return m_years.getYear(nextYear).Months.get(nextMonth);
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        /// <param name="type">类型</param>
        public override void getProperty(String name, ref String value, ref String type) {
            if (name == "selectedday") {
                type = "string";
                value = String.Format("{0}-{1}-{2}", m_year, m_month, m_selectedDay.Day);
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
            propertyNames.add("SeletedDay");
            propertyNames.add("UseAnimation");
            return propertyNames;
        }

        /// <summary>
        /// 回到上个月
        /// </summary>
        public void goLastMonth() {
            SelectedDay = getLastMonth(m_year, m_month).Days.get(1);
        }

        /// <summary>
        /// 前往下个月
        /// </summary>
        public void goNextMonth() {
            SelectedDay = getNextMonth(m_year, m_month).Days.get(1);
        }

        /// <summary>
        /// 触摸点击方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onClick(FCTouchInfo touchInfo) {
            base.onClick(touchInfo);
            if (m_dayDiv != null) {
                m_dayDiv.onClick(touchInfo);
            }
            if (m_monthDiv != null) {
                m_monthDiv.onClick(touchInfo);
            }
            if (m_yearDiv != null) {
                m_yearDiv.onClick(touchInfo);
            }
        }

        /// <summary>
        /// 控件被添加事件
        /// </summary>
        public override void onLoad() {
            base.onLoad();
            FCHost host = Native.Host;
            if (m_dayDiv == null) {
                m_dayDiv = new DayDiv(this);
            }
            if (m_timeDiv == null) {
                m_timeDiv = new TimeDiv(this);
            }
            if (m_headDiv == null) {
                m_headDiv = host.createInternalControl(this, "headdiv") as HeadDiv;
                addControl(m_headDiv);
            }
            if (m_useAnimation) {
                startTimer(m_timerID, 20);
            }
            else {
                stopTimer(m_timerID);
            }
            if (m_years != null && m_year == 0 && m_month == 0) {
                DateTime date = DateTime.Now;
                m_year = date.Year;
                m_month = date.Month;
                SelectedDay = m_years.getYear(m_year).Months.get(m_month).Days.get(date.Day);
            }
        }

        /// <summary>
        /// 键盘方法
        /// </summary>
        /// <param name="key">按键</param>
        public override void onKeyDown(char key) {
            base.onKeyDown(key);
            FCHost host = Native.Host;
            if (!host.isKeyPress(0x10) && !host.isKeyPress(0x11) && !host.isKeyPress(0x12)) {
                CMonth thisMonth = Month;
                CMonth lastMonth = getLastMonth(m_year, m_month);
                CMonth nextMonth = getNextMonth(m_year, m_month);
                int today = m_selectedDay.Day;
                if (key >= 37 && key <= 40) {
                    switch ((int)key) {
                        case 37:
                            if (m_selectedDay == thisMonth.FirstDay) {
                                SelectedDay = lastMonth.LastDay;
                            }
                            else {
                                SelectedDay = thisMonth.Days.get(today - 1);
                            }
                            break;
                        case 38:
                            if (today <= 7) {
                                SelectedDay = lastMonth.Days.get(lastMonth.Days.size() - (7 - today));
                            }
                            else {
                                SelectedDay = thisMonth.Days.get(m_selectedDay.Day - 7);
                            }
                            break;
                        case 39:
                            if (m_selectedDay == thisMonth.LastDay) {
                                SelectedDay = nextMonth.FirstDay;
                            }
                            else {
                                SelectedDay = thisMonth.Days.get(today + 1);
                            }
                            break;
                        case 40:
                            if (today > thisMonth.Days.size() - 7) {
                                SelectedDay = nextMonth.Days.get(7 - (thisMonth.Days.size() - today));
                            }
                            else {
                                SelectedDay = thisMonth.Days.get(today + 7);
                            }
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 重绘背景方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void onPaintBackground(FCPaint paint, FCRect clipRect) {
            base.onPaintBackground(paint, clipRect);
            if (m_dayDiv != null) {
                m_dayDiv.onPaint(paint, clipRect);
            }
            if (m_monthDiv != null) {
                m_monthDiv.onPaint(paint, clipRect);
            }
            if (m_yearDiv != null) {
                m_yearDiv.onPaint(paint, clipRect);
            }
            if (m_timeDiv != null) {
                m_timeDiv.onPaint(paint, clipRect);
            }
        }

        /// <summary>
        /// 选中日期更改方法
        /// </summary>
        public virtual void onSelectedTimeChanged() {
            callEvents(FCEventID.SELECTEDTIMECHANGED);
        }

        /// <summary>
        /// 秒表事件
        /// </summary>
        /// <param name="timerID">秒表编号</param>
        public override void onTimer(int timerID) {
            base.onTimer(timerID);
            if (m_timerID == timerID) {
                if (m_dayDiv != null) {
                    m_dayDiv.onTimer();
                }
                if (m_monthDiv != null) {
                    m_monthDiv.onTimer();
                }
                if (m_yearDiv != null) {
                    m_yearDiv.onTimer();
                }
                if (m_timeDiv != null) {
                    m_timeDiv.onTimer();
                }
            }
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        public override void setProperty(String name, String value) {
            if (name == "selectedday") {
                DateTime date = Convert.ToDateTime(value);
                m_year = date.Year;
                m_month = date.Month;
                SelectedDay = m_years.getYear(m_year).Months.get(m_month).Days.get(date.Day);
            }
            else if (name == "useanimation") {
                UseAnimation = FCStr.convertStrToBool(value);
            }
            else {
                base.setProperty(name, value);
            }
        }

        /// <summary>
        /// 重新布局
        /// </summary>
        public override void update() {
            base.update();
            if (m_dayDiv != null) {
                m_dayDiv.update();
            }
            if (m_headDiv != null) {
                m_headDiv.bringToFront();
                m_headDiv.update();
            }
            if (m_monthDiv != null) {
                m_monthDiv.update();
            }
            if (m_yearDiv != null) {
                m_yearDiv.update();
            }
            if (m_timeDiv != null) {
                m_timeDiv.update();
            }
        }
    }
}
