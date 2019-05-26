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
    /// 日期按钮
    /// </summary>
    public class DayButton {
        /// <summary>
        /// 创建日的按钮
        /// </summary>
        /// <param name="calendar">日历控件</param>
        public DayButton(FCCalendar calendar) {
            m_calendar = calendar;
        }

        protected FCRect m_bounds;

        /// <summary>
        /// 获取或设置显示区域
        /// </summary>
        public virtual FCRect Bounds {
            get { return m_bounds; }
            set { m_bounds = value; }
        }

        protected FCCalendar m_calendar;

        /// <summary>
        /// 获取或设置日历控件
        /// </summary>
        public virtual FCCalendar Calendar {
            get { return m_calendar; }
            set { m_calendar = value; }
        }

        protected CDay m_day;

        /// <summary>
        /// 获取或设置保存的日期
        /// </summary>
        public virtual CDay Day {
            get { return m_day; }
            set { m_day = value; }
        }

        protected bool m_inThisMonth;

        /// <summary>
        /// 获取或设置是否在本月
        /// </summary>
        public virtual bool InThisMonth {
            get { return m_inThisMonth; }
            set { m_inThisMonth = value; }
        }

        protected bool m_selected;

        /// <summary>
        /// 获取或设置是否被选中
        /// </summary>
        public virtual bool Selected {
            get { return m_selected; }
            set { m_selected = value; }
        }

        protected bool m_visible = true;

        /// <summary>
        /// 获取或设置是否可见
        /// </summary>
        public virtual bool Visible {
            get { return m_visible; }
            set { m_visible = value; }
        }

        /// <summary>
        /// 获取绘制的背景色
        /// </summary>
        /// <returns>背景色</returns>
        protected virtual long getPaintingBackColor() {
            if (m_selected) {
                return FCColor.Pushed;
            }
            else {
                if (m_inThisMonth) {
                    return FCColor.Hovered;
                }
                else {
                    return FCColor.Back;
                }
            }
        }

        /// <summary>
        /// 获取绘制的边线颜色
        /// </summary>
        /// <returns>边线颜色</returns>
        protected virtual long getPaintingBorderColor() {
            return FCColor.Border;
        }

        /// <summary>
        /// 获取要绘制的前景色
        /// </summary>
        /// <returns></returns>
        protected virtual long getPaintingTextColor() {
            return FCColor.Text;
        }

        /// <summary>
        /// 触摸点击事件
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public void onClick(FCTouchInfo touchInfo) {
            if (m_calendar != null && m_day != null) {
                m_calendar.SelectedDay = m_day;
            }
        }

        /// <summary>
        /// 重绘背景方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public virtual void onPaintBackGround(FCPaint paint, FCRect clipRect) {
            long backColor = getPaintingBackColor();
            paint.fillRect(backColor, m_bounds);
        }

        /// <summary>
        /// 重绘边线方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public virtual void onPaintBorder(FCPaint paint, FCRect clipRect) {
            long borderColor = getPaintingBorderColor();
            paint.drawLine(borderColor, 1, 0, m_bounds.left, m_bounds.bottom - 1, m_bounds.right - 1, m_bounds.bottom - 1);
            paint.drawLine(borderColor, 1, 0, m_bounds.right - 1, m_bounds.top, m_bounds.right - 1, m_bounds.bottom - 1);
        }

        /// <summary>
        /// 重绘前景方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public virtual void onPaintForeground(FCPaint paint, FCRect clipRect) {
            if (m_day != null) {
                int width = m_bounds.right - m_bounds.left;
                int height = m_bounds.bottom - m_bounds.top;
                String dayStr = m_day.Day.ToString();
                FCFont font = m_calendar.Font;
                FCSize textSize = paint.textSize(dayStr, font);
                FCRect tRect = new FCRect();
                tRect.left = m_bounds.left + (width - textSize.cx) / 2;
                tRect.top = m_bounds.top + (height - textSize.cy) / 2;
                tRect.right = tRect.left + textSize.cx;
                tRect.bottom = tRect.top + textSize.cy;
                paint.drawText(dayStr, getPaintingTextColor(), font, tRect);
            }
        }
    }
}
