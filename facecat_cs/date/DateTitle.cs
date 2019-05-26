/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

using System;

namespace FaceCat {
    /// <summary>
    /// 日期标题
    /// </summary>
    public class DateTitle : FCButton {
        /// <summary>
        /// 创建日期标题
        /// </summary>
        /// <param name="calendar">日历控件</param>
        public DateTitle(FCCalendar calendar) {
            m_calendar = calendar;
            BackColor = FCColor.None;
            BorderColor = FCColor.None;
            Font = new FCFont("宋体", 22, true, false, false);
            Size = new FCSize(180, 30);
        }

        /// <summary>
        /// 日历
        /// </summary>
        protected FCCalendar m_calendar;

        /// <summary>
        /// 获取或设置日历
        /// </summary>
        public virtual FCCalendar Calendar {
            get { return m_calendar; }
            set { m_calendar = value; }
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <returns>控件类型</returns>
        public override String getControlType() {
            return "DateTitle";
        }

        /// <summary>
        /// 触摸点击方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onClick(FCTouchInfo touchInfo) {
            base.onClick(touchInfo);
            if (m_calendar != null) {
                FCCalendarMode mode = m_calendar.Mode;
                //日
                if (mode == FCCalendarMode.Day) {
                    m_calendar.Mode = FCCalendarMode.Month;
                }
                //月
                else if (mode == FCCalendarMode.Month) {
                    m_calendar.Mode = FCCalendarMode.Year;
                }
                m_calendar.update();
                m_calendar.invalidate();
            }
        }

        /// <summary>
        /// 重绘前景方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void onPaintForeground(FCPaint paint, FCRect clipRect) {
            if (m_calendar != null) {
                int width = Width, height = Height;
                FCFont font = Font;
                String text = "";
                FCCalendarMode mode = m_calendar.Mode;
                //日
                if (mode == FCCalendarMode.Day) {
                    CMonth month = m_calendar.Month;
                    text = month.Year.ToString() + "年" + month.Month.ToString() + "月";
                }
                //月
                else if (mode == FCCalendarMode.Month) {
                    text = m_calendar.MonthDiv.Year.ToString() + "年";
                }
                //年
                else if (mode == FCCalendarMode.Year) {
                    int startYear = m_calendar.YearDiv.StartYear;
                    text = startYear.ToString() + "年 - " + (startYear + 12).ToString() + "年";
                }
                FCSize tSize = paint.textSize(text, font);
                FCRect tRect = new FCRect();
                tRect.left = (width - tSize.cx) / 2;
                tRect.top = (height - tSize.cy) / 2;
                tRect.right = tRect.left + tSize.cx + 1;
                tRect.bottom = tRect.top + tSize.cy + 1;
                paint.drawText(text, getPaintingTextColor(), font, tRect);
            }
        }
    }
}
