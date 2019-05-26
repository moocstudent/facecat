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
using System.Text;

namespace FaceCat {
    /// <summary>
    /// 时间层
    /// </summary>
    public class TimeDiv {
        /// <summary>
        /// 创建时间层
        /// </summary>
        public TimeDiv(FCCalendar calendar) {
            m_calendar = calendar;
            onLoad();
        }

        /// <summary>
        /// 小时输入框
        /// </summary>
        protected FCSpin m_spinHour;

        /// <summary>
        /// 分钟输入框
        /// </summary>
        protected FCSpin m_spinMinute;

        /// <summary>
        /// 秒输入框
        /// </summary>
        protected FCSpin m_spinSecond;

        protected FCCalendar m_calendar;

        /// <summary>
        /// 获取或设置日历控件
        /// </summary>
        public virtual FCCalendar Calendar {
            get { return m_calendar; }
            set { m_calendar = value; }
        }

        protected int m_height = 40;

        /// <summary>
        /// 获取或设置高度
        /// </summary>
        public virtual int Height {
            get { return m_height; }
            set { m_height = value; }
        }

        /// <summary>
        /// 获取或设置小时
        /// </summary>
        public virtual int Hour {
            get {
                if (m_spinHour != null) {
                    return (int)m_spinHour.Value;
                }
                else {
                    return 0;
                }
            }
            set {
                if (m_spinHour != null) {
                    m_spinHour.Value = value;
                }
            }
        }

        /// <summary>
        /// 获取或设置分钟
        /// </summary>
        public virtual int Minute {
            get {
                if (m_spinMinute != null) {
                    return (int)m_spinMinute.Value;
                }
                else {
                    return 0;
                }
            }
            set {
                if (m_spinMinute != null) {
                    m_spinMinute.Value = value;
                }
            }
        }

        /// <summary>
        /// 获取或设置秒
        /// </summary>
        public virtual int Second {
            get {
                if (m_spinSecond != null) {
                    return (int)m_spinSecond.Value;
                }
                else {
                    return 0;
                }
            }
            set {
                if (m_spinSecond != null) {
                    m_spinSecond.Value = value;
                }
            }
        }

        /// <summary>
        /// 销毁方法
        /// </summary>
        public virtual void delete() {
            m_spinHour = null;
            m_spinMinute = null;
            m_spinSecond = null;
        }

        /// <summary>
        /// 获取绘制的背景色
        /// </summary>
        /// <returns>背景色</returns>
        protected virtual long getPaintingBackColor() {
            return FCColor.Back;
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
        /// 添加控件方法
        /// </summary>
        public virtual void onLoad() {
            if (m_calendar != null) {
                FCHost host = m_calendar.Native.Host;
                if (m_spinHour == null) {
                    m_spinHour = new FCSpin();
                    m_spinHour.Maximum = 23;
                    m_spinHour.TextAlign = FCHorizontalAlign.Right;
                    m_calendar.addControl(m_spinHour);
                    m_spinHour.addEvent(new FCEvent(selectedTimeChanged), FCEventID.VALUECHANGED);
                }
                if (m_spinMinute == null) {
                    m_spinMinute = new FCSpin();
                    m_spinMinute.Maximum = 59;
                    m_spinMinute.TextAlign = FCHorizontalAlign.Right;
                    m_calendar.addControl(m_spinMinute);
                    m_spinMinute.addEvent(new FCEvent(selectedTimeChanged), FCEventID.VALUECHANGED);
                }
                if (m_spinSecond == null) {
                    m_spinSecond = new FCSpin();
                    m_spinSecond.Maximum = 59;
                    m_spinSecond.TextAlign = FCHorizontalAlign.Right;
                    m_calendar.addControl(m_spinSecond);
                    m_spinSecond.addEvent(new FCEvent(selectedTimeChanged), FCEventID.VALUECHANGED);
                }
            }
        }

        /// <summary>
        /// 重绘方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public virtual void onPaint(FCPaint paint, FCRect clipRect) {
            int width = m_calendar.Width, height = m_calendar.Height;
            int top = height - m_height;
            FCRect rect = new FCRect(0, height - m_height, width, height);
            paint.fillRect(getPaintingBackColor(), rect);
            if (m_height > 0) {
                long textColor = getPaintingTextColor();
                FCFont font = m_calendar.Font;
                FCSize tSize = paint.textSize("时", font);
                FCRect tRect = new FCRect();
                tRect.left = width / 3 - tSize.cx;
                tRect.top = top + m_height / 2 - tSize.cy / 2;
                tRect.right = tRect.left + tSize.cx;
                tRect.bottom = tRect.top + tSize.cy;
                paint.drawText("时", textColor, font, tRect);
                tSize = paint.textSize("分", font);
                tRect.left = width * 2 / 3 - tSize.cx;
                tRect.top = top + m_height / 2 - tSize.cy / 2;
                tRect.right = tRect.left + tSize.cx;
                tRect.bottom = tRect.top + tSize.cy;
                paint.drawText("分", textColor, font, tRect);
                tSize = paint.textSize("秒", font);
                tRect.left = width - tSize.cx - 5;
                tRect.top = top + m_height / 2 - tSize.cy / 2;
                tRect.right = tRect.left + tSize.cx;
                tRect.bottom = tRect.top + tSize.cy;
                paint.drawText("秒", textColor, font, tRect);
            }
        }

        /// <summary>
        /// 数值修改事件
        /// </summary>
        public virtual void onSelectedTimeChanged() {
            if (m_calendar != null) {
                m_calendar.onSelectedTimeChanged();
            }
        }

        /// <summary>
        /// 秒表触发方法
        /// </summary>
        public virtual void onTimer() {
        }

        /// <summary>
        /// 数值修改事件
        /// </summary>
        /// <param name="sender">调用者</param>
        protected void selectedTimeChanged(object sender) {
            onSelectedTimeChanged();
        }

        /// <summary>
        /// 更新布局方法
        /// </summary>
        public virtual void update() {
            if (m_height > 0) {
                int width = m_calendar.Width, height = m_calendar.Height;
                int top = height - m_height;
                int left = 5;
                if (m_spinHour != null) {
                    m_spinHour.Visible = true;
                    m_spinHour.Location = new FCPoint(left, top + m_height / 2 - m_spinHour.Height / 2);
                    m_spinHour.Width = (width - 15) / 3 - 20;
                }
                if (m_spinMinute != null) {
                    m_spinMinute.Visible = true;
                    m_spinMinute.Location = new FCPoint(width / 3 + 5, top + m_height / 2 - m_spinMinute.Height / 2);
                    m_spinMinute.Width = (width - 15) / 3 - 20;
                }
                if (m_spinSecond != null) {
                    m_spinSecond.Visible = true;
                    m_spinSecond.Location = new FCPoint(width * 2 / 3 + 5, top + m_height / 2 - m_spinSecond.Height / 2);
                    m_spinSecond.Width = (width - 15) / 3 - 20;

                }
            }
            else {
                if (m_spinHour != null) {
                    m_spinHour.Visible = false;
                }
                if (m_spinMinute != null) {
                    m_spinMinute.Visible = false;
                }
                if (m_spinSecond != null) {
                    m_spinSecond.Visible = false;
                }
            }
        }
    }
}
