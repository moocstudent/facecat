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
    public class YearDiv {
        /// <summary>
        /// 创建日期层
        /// </summary>
        public YearDiv(FCCalendar calendar) {
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
        public ArrayList<YearButton> m_yearButtons = new ArrayList<YearButton>();

        /// <summary>
        /// 月的动画按钮
        /// </summary>
        public ArrayList<YearButton> m_yearButtons_am = new ArrayList<YearButton>();

        protected FCCalendar m_calendar;

        /// <summary>
        /// 获取或设置日历控件
        /// </summary>
        public virtual FCCalendar Calendar {
            get { return m_calendar; }
            set { m_calendar = value; }
        }

        protected int m_startYear;

        /// <summary>
        /// 获取开始年份
        /// </summary>
        public virtual int StartYear {
            get { return m_startYear; }
        }

        /// <summary>
        /// 销毁方法
        /// </summary>
        public virtual void delete() {
            m_yearButtons.clear();
            m_yearButtons_am.clear();
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public virtual void hide() {
            int yearButtonSize = m_yearButtons.size();
            for (int i = 0; i < yearButtonSize; i++) {
                YearButton yearButton = m_yearButtons.get(i);
                yearButton.Visible = false;
            }
        }

        /// <summary>
        /// 触摸点击方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public void onClick(FCTouchInfo touchInfo) {
            FCPoint mp = touchInfo.m_firstPoint;
            int yearButtonsSize = m_yearButtons.size();
            for (int i = 0; i < yearButtonsSize; i++) {
                YearButton yearButton = m_yearButtons.get(i);
                if (yearButton.Visible) {
                    FCRect bounds = yearButton.Bounds;
                    if (mp.x >= bounds.left && mp.x <= bounds.right && mp.y >= bounds.top && mp.y <= bounds.bottom) {
                        yearButton.onClick(touchInfo);
                        return;
                    }
                }
            }
            int yearButtonAmSize = m_yearButtons_am.size();
            for (int i = 0; i < yearButtonAmSize; i++) {
                YearButton yearButton = m_yearButtons_am.get(i);
                if (yearButton.Visible) {
                    FCRect bounds = yearButton.Bounds;
                    if (mp.x >= bounds.left && mp.x <= bounds.right && mp.y >= bounds.top && mp.y <= bounds.bottom) {
                        yearButton.onClick(touchInfo);
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
                if (m_yearButtons.size() == 0 || m_yearButtons_am.size() == 0) {
                    m_yearButtons.clear();
                    m_yearButtons_am.clear();
                    for (int i = 0; i < 12; i++) {
                        YearButton yearButton = new YearButton(m_calendar);
                        m_yearButtons.add(yearButton);
                        YearButton yearButtonAm = new YearButton(m_calendar);
                        yearButtonAm.Visible = false;
                        m_yearButtons_am.add(yearButtonAm);
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
            int yearButtonsSize = m_yearButtons.size();
            for (int i = 0; i < yearButtonsSize; i++) {
                YearButton yearButton = m_yearButtons.get(i);
                if (yearButton.Visible) {
                    FCRect bounds = yearButton.Bounds;
                    yearButton.onPaintBackGround(paint, bounds);
                    yearButton.onPaintForeground(paint, bounds);
                    yearButton.onPaintBorder(paint, bounds);
                }
            }
            int yearButtonAmSize = m_yearButtons_am.size();
            for (int i = 0; i < yearButtonAmSize; i++) {
                YearButton yearButton = m_yearButtons_am.get(i);
                if (yearButton.Visible) {
                    FCRect bounds = yearButton.Bounds;
                    yearButton.onPaintBackGround(paint, bounds);
                    yearButton.onPaintForeground(paint, bounds);
                    yearButton.onPaintBorder(paint, bounds);
                }
            }
        }

        /// <summary>
        /// 重置日期图层
        /// </summary>
        /// <param name="state">状态</param>
        public virtual void onResetDiv(int state) {
            if (m_calendar != null) {
                int thisStartYear = m_startYear;
                int lastStartYear = m_startYear - 12;
                int nextStartYear = m_startYear + 12;
                int left = 0;
                int headHeight = m_calendar.HeadDiv.Height;
                int top = headHeight;
                int width = m_calendar.Width;
                int height = m_calendar.Height;
                height -= m_calendar.TimeDiv.Height;
                int yearButtonHeight = height - top;
                if (yearButtonHeight < 1) {
                    yearButtonHeight = 1;
                }
                int toY = 0;
                ArrayList<YearButton> yearButtons = new ArrayList<YearButton>();
                if (m_am_Direction == 1) {
                    toY = yearButtonHeight * m_am_Tick / m_am_TotalTick;
                    if (state == 1) {
                        thisStartYear = nextStartYear;
                        lastStartYear = thisStartYear - 12;
                        nextStartYear = thisStartYear + 12;
                    }
                }
                else if (m_am_Direction == 2) {
                    toY = -yearButtonHeight * m_am_Tick / m_am_TotalTick;
                    if (state == 1) {
                        thisStartYear = lastStartYear;
                        lastStartYear = thisStartYear - 12;
                        nextStartYear = thisStartYear + 12;
                    }
                }
                if (state == 0) {
                    yearButtons = m_yearButtons;
                }
                else if (state == 1) {
                    yearButtons = m_yearButtons_am;
                }
                int dheight = yearButtonHeight / 3;
                int buttonSize = yearButtons.size();
                for (int i = 0; i < buttonSize; i++) {
                    if (i == 8) {
                        dheight = height - top;
                    }
                    YearButton yearButton = yearButtons.get(i);
                    yearButton.Year = thisStartYear + i;
                    int vOffSet = 0;
                    if (state == 1) {
                        if (m_am_Tick > 0) {
                            yearButton.Visible = true;
                            if (m_am_Direction == 1) {
                                vOffSet = toY - yearButtonHeight;
                            }
                            else if (m_am_Direction == 2) {
                                vOffSet = toY + yearButtonHeight;
                            }
                        }
                        else {
                            yearButton.Visible = false;
                            continue;
                        }
                    }
                    else {
                        vOffSet = toY;
                    }
                    if ((i + 1) % 4 == 0) {
                        FCPoint dp = new FCPoint(left, top + vOffSet);
                        FCSize ds = new FCSize(width - left, dheight);
                        yearButton.Bounds = new FCRect(dp.x, dp.y, dp.x + ds.cx, dp.y + ds.cy);
                        left = 0;
                        if (i != 0 && i != buttonSize - 1) {
                            top += dheight;
                        }
                    }
                    else {
                        FCPoint dp = new FCPoint(left, top + vOffSet);
                        FCSize ds = new FCSize(width / 4 + ((i + 1) % 4) % 2, dheight);
                        yearButton.Bounds = new FCRect(dp.x, dp.y, dp.x + ds.cx, dp.y + ds.cy);
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
        /// 选择开始年份
        /// </summary>
        /// <param name="startYear">开始年份</param>
        public void selectStartYear(int startYear) {
            if (m_calendar != null) {
                if (m_startYear != startYear) {
                    if (startYear > m_startYear) {
                        m_am_Direction = 1;
                    }
                    else {
                        m_am_Direction = 2;
                    }
                    if (m_calendar.UseAnimation) {
                        m_am_Tick = m_am_TotalTick;
                    }
                    m_startYear = startYear;
                }
            }
        }

        /// <summary>
        /// 显示
        /// </summary>
        public virtual void show() {
            int yearButtonSize = m_yearButtons.size();
            for (int i = 0; i < yearButtonSize; i++) {
                YearButton yearButton = m_yearButtons.get(i);
                yearButton.Visible = true;
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
