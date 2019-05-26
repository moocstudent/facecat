/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

using System;

namespace FaceCat {
    /// <summary>
    /// 切换到上个月的按钮
    /// </summary>
    public class ArrowButton : FCButton {
        /// <summary>
        /// 创建按钮
        /// </summary>
        public ArrowButton(FCCalendar calendar) {
            m_calendar = calendar;
            BorderColor = FCColor.None;
            BackColor = FCColor.None;
            Size = new FCSize(16, 16);
        }

        protected FCCalendar m_calendar;

        /// <summary>
        /// 获取或设置日历控件
        /// </summary>
        public virtual FCCalendar Calendar {
            get { return m_calendar; }
            set { m_calendar = value; }
        }

        protected bool m_toLast = true;

        /// <summary>
        /// 获取或设置是否前往上个月
        /// </summary>
        public virtual bool ToLast {
            get { return m_toLast; }
            set { m_toLast = value; }
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <returns>控件类型</returns>
        public override String getControlType() {
            return "ArrowButton";
        }

        /// <summary>
        /// 触摸点击事件
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onClick(FCTouchInfo touchInfo) {
            base.onClick(touchInfo);
            if (m_calendar != null) {
                FCCalendarMode mode = m_calendar.Mode;
                //日
                if (mode == FCCalendarMode.Day) {
                    if (m_toLast) {
                        m_calendar.goLastMonth();
                    }
                    else {
                        m_calendar.goNextMonth();
                    }
                }
                //月
                else if (mode == FCCalendarMode.Month) {
                    MonthDiv monthDiv = m_calendar.MonthDiv;
                    if (monthDiv != null) {
                        int year = monthDiv.Year;
                        if (m_toLast) {
                            monthDiv.selectYear(year - 1);
                        }
                        else {
                            monthDiv.selectYear(year + 1);
                        }
                    }
                }
                //年
                else if (mode == FCCalendarMode.Year) {
                    YearDiv yearDiv = m_calendar.YearDiv;
                    if (yearDiv != null) {
                        int year = yearDiv.StartYear;
                        if (m_toLast) {
                            yearDiv.selectStartYear(year - 12);
                        }
                        else {
                            yearDiv.selectStartYear(year + 12);
                        }
                    }
                }
                m_calendar.invalidate();
            }
        }

        /// <summary>
        /// 重绘背景方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void onPaintForeground(FCPaint paint, FCRect clipRect) {
            int width = Width, height = Height;
            FCPoint p1 = new FCPoint(), p2 = new FCPoint(), p3 = new FCPoint();
            //计算三个点的位置
            if (m_toLast) {
                p1.x = 0;
                p1.y = height / 2;
                p2.x = width;
                p2.y = 0;
                p3.x = width;
                p3.y = height;
            }
            else {
                p1.x = 0;
                p1.y = 0;
                p2.x = 0;
                p2.y = height;
                p3.x = width;
                p3.y = height / 2;
            }
            FCPoint[] points = new FCPoint[3];
            points[0] = p1;
            points[1] = p2;
            points[2] = p3;
            paint.fillPolygon(getPaintingTextColor(), points);
        }
    }
}
