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
    /// 顶部层
    /// </summary>
    public class HeadDiv : FCView {
        /// <summary>
        /// 创建层
        /// </summary>
        /// <param name="calendar">日历</param>
        public HeadDiv(FCCalendar calendar) {
            m_calendar = calendar;
            Font = new FCFont("宋体", 14, true, false, false);
            Height = 55;
        }

        protected String[] m_weekDays = new String[] { "周日", "周一", "周二", "周三", "周四", "周五", "周六" };

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

        protected DateTitle m_dateTitle;

        /// <summary>
        /// 获取或设置日期标题
        /// </summary>
        public virtual DateTitle DateTitle {
            get { return m_dateTitle; }
            set { m_dateTitle = value; }
        }

        protected ArrowButton m_lastBtn;

        /// <summary>
        /// 获取或设置到上个周期的按钮
        /// </summary>
        public virtual ArrowButton LastBtn {
            get { return m_lastBtn; }
            set { m_lastBtn = value; }
        }

        protected ArrowButton m_nextBtn;

        /// <summary>
        /// 获取或设置到下个周期的按钮
        /// </summary>
        public virtual ArrowButton NextBtn {
            get { return m_nextBtn; }
            set { m_nextBtn = value; }
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <returns>控件类型</returns>
        public override String getControlType() {
            return "HeadDiv";
        }

        /// <summary>
        /// 添加控件方法
        /// </summary>
        public override void onLoad() {
            base.onLoad();
            FCHost host = Native.Host;
            if (m_dateTitle == null) {
                m_dateTitle = host.createInternalControl(m_calendar, "datetitle") as DateTitle;
                addControl(m_dateTitle);
            }
            if (m_lastBtn == null) {
                m_lastBtn = host.createInternalControl(m_calendar, "lastbutton") as ArrowButton;
                addControl(m_lastBtn);
            }
            if (m_nextBtn == null) {
                m_nextBtn = host.createInternalControl(m_calendar, "nextbutton") as ArrowButton;
                addControl(m_nextBtn);
            }
        }

        /// <summary>
        /// 重绘背景方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void onPaintBackground(FCPaint paint, FCRect clipRect) {
            int width = Width, height = Height;
            FCRect rect = new FCRect(0, 0, width, height);
            paint.fillRoundRect(getPaintingBackColor(), rect, m_cornerRadius);
        }

        /// <summary>
        /// 重绘边线方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void onPaintBorder(FCPaint paint, FCRect clipRect) {
            int width = Width, height = Height;
            paint.drawLine(getPaintingBorderColor(), 1, 0, 0, height - 1, width, height - 1);
        }

        /// <summary>
        /// 重绘前景方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void onPaintForeground(FCPaint paint, FCRect clipRect) {
            int width = Width, height = Height;
            FCCalendarMode mode = m_calendar.Mode;
            //画星期标题
            if (mode == FCCalendarMode.Day) {
                float left = 0;
                FCSize weekDaySize = new FCSize();
                FCFont font = Font;
                long textColor = getPaintingTextColor();
                for (int i = 0; i < m_weekDays.Length; i++) {
                    weekDaySize = paint.textSize(m_weekDays[i], font);
                    float textX = left + (width / 7F) / 2F - weekDaySize.cx / 2F;
                    float textY = height - weekDaySize.cy;
                    FCRect tRect = new FCRect(textX, textY, textX + weekDaySize.cx, textY + weekDaySize.cy);
                    paint.drawText(m_weekDays[i], textColor, font, tRect);
                    left += Width / 7F;
                }
            }
        }

        /// <summary>
        /// 布局更新方法
        /// </summary>
        public override void update() {
            base.update();
            int width = Width, height = Height;
            if (m_dateTitle != null) {
                m_dateTitle.Location = new FCPoint((width - m_dateTitle.Width) / 2, (height - m_dateTitle.Height) / 2);
            }
            if (m_lastBtn != null) {
                m_lastBtn.Location = new FCPoint(2, (height - m_lastBtn.Height) / 2);
            }
            if (m_nextBtn != null) {
                m_nextBtn.Location = new FCPoint(width - m_nextBtn.Width - 2, (height - m_nextBtn.Height) / 2);
            }
        }
    }
}
