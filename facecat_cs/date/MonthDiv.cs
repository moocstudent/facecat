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
    /// 日期层
    /// </summary>
    public class MonthDiv {
        /// <summary>
        /// 创建日期层
        /// </summary>
        public MonthDiv(FCCalendar calendar) {
            m_calendar = calendar;
            onLoad();
        }

        /// <summary>
        /// 动画的方向
        /// </summary>
        protected int m_am_Direction;

        /// <summary>
        /// 动画当前帧数
        /// </summary>
        protected int m_am_Tick;

        /// <summary>
        /// 动画总帧数
        /// </summary>
        protected int m_am_TotalTick = 40;

        /// <summary>
        /// 月的按钮
        /// </summary>
        public ArrayList<MonthButton> m_monthButtons = new ArrayList<MonthButton>();

        /// <summary>
        /// 月的动画按钮
        /// </summary>
        public ArrayList<MonthButton> m_monthButtons_am = new ArrayList<MonthButton>();

        protected FCCalendar m_calendar;

        /// <summary>
        /// 获取或设置日历控件
        /// </summary>
        public FCCalendar Calendar {
            get { return m_calendar; }
            set { m_calendar = value; }
        }

        protected int m_year;

        /// <summary>
        /// 获取年份
        /// </summary>
        public virtual int Year {
            get { return m_year; }
        }

        /// <summary>
        /// 销毁方法
        /// </summary>
        public virtual void delete() {
            m_monthButtons.clear();
            m_monthButtons_am.clear();
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public virtual void hide() {
            int monthButtonSize = m_monthButtons.size();
            for (int i = 0; i < monthButtonSize; i++) {
                MonthButton monthButton = m_monthButtons.get(i);
                monthButton.Visible = false;
            }
        }

        /// <summary>
        /// 触摸点击方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public void onClick(FCTouchInfo touchInfo) {
            FCPoint mp = touchInfo.m_firstPoint;
            int monthButtonsSize = m_monthButtons.size();
            for (int i = 0; i < monthButtonsSize; i++) {
                MonthButton monthButton = m_monthButtons.get(i);
                if (monthButton.Visible) {
                    FCRect bounds = monthButton.Bounds;
                    if (mp.x >= bounds.left && mp.x <= bounds.right && mp.y >= bounds.top && mp.y <= bounds.bottom) {
                        monthButton.onClick(touchInfo);
                        return;
                    }
                }
            }
            int monthButtonAmSize = m_monthButtons_am.size();
            for (int i = 0; i < monthButtonAmSize; i++) {
                MonthButton monthButton = m_monthButtons_am.get(i);
                if (monthButton.Visible) {
                    FCRect bounds = monthButton.Bounds;
                    if (mp.x >= bounds.left && mp.x <= bounds.right && mp.y >= bounds.top && mp.y <= bounds.bottom) {
                        monthButton.onClick(touchInfo);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 添加控件方法
        /// </summary>
        public virtual void onLoad() {
            if (m_calendar != null) {
                FCHost host = m_calendar.Native.Host;
                if (m_monthButtons.size() == 0 || m_monthButtons_am.size() == 0) {
                    m_monthButtons.clear();
                    m_monthButtons_am.clear();
                    for (int i = 0; i < 12; i++) {
                        MonthButton monthButton = new MonthButton(m_calendar);
                        monthButton.Month = i + 1;
                        m_monthButtons.add(monthButton);
                        MonthButton monthButtonAm = new MonthButton(m_calendar);
                        monthButtonAm.Month = i + 1;
                        monthButtonAm.Visible = false;
                        m_monthButtons_am.add(monthButtonAm);
                    }
                }
            }
        }

        /// <summary>
        /// 重绘方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public virtual void onPaint(FCPaint paint, FCRect clipRect) {
            int monthButtonsSize = m_monthButtons.size();
            for (int i = 0; i < monthButtonsSize; i++) {
                MonthButton monthButton = m_monthButtons.get(i);
                if (monthButton.Visible) {
                    FCRect bounds = monthButton.Bounds;
                    monthButton.onPaintBackGround(paint, bounds);
                    monthButton.onPaintForeground(paint, bounds);
                    monthButton.onPaintBorder(paint, bounds);
                }
            }
            int monthButtonAmSize = m_monthButtons_am.size();
            for (int i = 0; i < monthButtonAmSize; i++) {
                MonthButton monthButton = m_monthButtons_am.get(i);
                if (monthButton.Visible) {
                    FCRect bounds = monthButton.Bounds;
                    monthButton.onPaintBackGround(paint, bounds);
                    monthButton.onPaintForeground(paint, bounds);
                    monthButton.onPaintBorder(paint, bounds);
                }
            }
        }

        /// <summary>
        /// 重置日期图层
        /// </summary>
        /// <param name="state">状态</param>
        public virtual void onResetDiv(int state) {
            if (m_calendar != null) {
                int thisYear = m_year;
                int lastYear = m_year - 1;
                int nextYear = m_year + 1;
                int left = 0;
                int headHeight = m_calendar.HeadDiv.Height;
                int top = headHeight;
                int width = m_calendar.Width;
                int height = m_calendar.Height;
                height -= m_calendar.TimeDiv.Height;
                int monthButtonHeight = height - top;
                if (monthButtonHeight < 1) {
                    monthButtonHeight = 1;
                }
                int toY = 0;
                ArrayList<MonthButton> monthButtons = new ArrayList<MonthButton>();
                if (m_am_Direction == 1) {
                    toY = monthButtonHeight * m_am_Tick / m_am_TotalTick;
                    if (state == 1) {
                        thisYear = nextYear;
                        lastYear = thisYear - 1;
                        nextYear = thisYear + 1;
                    }
                }
                else if (m_am_Direction == 2) {
                    toY = -monthButtonHeight * m_am_Tick / m_am_TotalTick;
                    if (state == 1) {
                        thisYear = lastYear;
                        lastYear = thisYear - 1;
                        nextYear = thisYear + 1;
                    }
                }
                if (state == 0) {
                    monthButtons = m_monthButtons;
                }
                else if (state == 1) {
                    monthButtons = m_monthButtons_am;
                }
                int dheight = monthButtonHeight / 3;
                int buttonSize = monthButtons.size();
                for (int i = 0; i < buttonSize; i++) {
                    if (i == 8) {
                        dheight = height - top;
                    }
                    MonthButton monthButton = monthButtons.get(i);
                    monthButton.Year = thisYear;
                    int vOffSet = 0;
                    if (state == 1) {
                        if (m_am_Tick > 0) {
                            monthButton.Visible = true;
                            if (m_am_Direction == 1) {
                                vOffSet = toY - monthButtonHeight;
                            }
                            else if (m_am_Direction == 2) {
                                vOffSet = toY + monthButtonHeight;
                            }
                        }
                        else {
                            monthButton.Visible = false;
                            continue;
                        }
                    }
                    else {
                        vOffSet = toY;
                    }
                    if ((i + 1) % 4 == 0) {
                        FCPoint dp = new FCPoint(left, top + vOffSet);
                        FCSize ds = new FCSize(width - left, dheight);
                        monthButton.Bounds = new FCRect(dp.x, dp.y, dp.x + ds.cx, dp.y + ds.cy);
                        left = 0;
                        if (i != 0 && i != buttonSize - 1) {
                            top += dheight;
                        }
                    }
                    else {
                        FCPoint dp = new FCPoint(left, top + vOffSet);
                        FCSize ds = new FCSize(width / 4 + ((i + 1) % 4) % 2, dheight);
                        monthButton.Bounds = new FCRect(dp.x, dp.y, dp.x + ds.cx, dp.y + ds.cy);
                        left += ds.cx;
                    }
                }
            }
        }

        /// <summary>
        /// 秒表触发方法
        /// </summary>
        public virtual void onTimer() {
            if (m_am_Tick > 0) {
                m_am_Tick = (int)((double)m_am_Tick * 2 / 3);
                if (m_calendar != null) {
                    m_calendar.update();
                    m_calendar.invalidate();
                }
            }
        }

        /// <summary>
        /// 选择年份
        /// </summary>
        /// <param name="year">年份</param>
        public void selectYear(int year) {
            if (m_calendar != null) {
                if (m_year != year) {
                    if (year > m_year) {
                        m_am_Direction = 1;
                    }
                    else {
                        m_am_Direction = 2;
                    }
                    if (m_calendar.UseAnimation) {
                        m_am_Tick = m_am_TotalTick;
                    }
                    m_year = year;
                }
            }
        }

        /// <summary>
        /// 显示
        /// </summary>
        public virtual void show() {
            int monthButtonSize = m_monthButtons.size();
            for (int i = 0; i < monthButtonSize; i++) {
                MonthButton monthButton = m_monthButtons.get(i);
                monthButton.Visible = true;
            }
        }

        /// <summary>
        /// 更新布局方法
        /// </summary>
        public virtual void update() {
            onResetDiv(0);
            onResetDiv(1);
        }
    }
}
