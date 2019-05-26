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
    /// 日
    /// </summary>
    [Serializable()]
    public class CDay {
        /// <summary>
        /// 创建日
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        public CDay(int year, int month, int day) {
            m_year = year;
            m_month = month;
            m_day = day;
        }

        private int m_day;

        /// <summary>
        /// 获取日
        /// </summary>
        public int Day {
            get { return m_day; }
        }

        private int m_month;

        /// <summary>
        /// 获取月
        /// </summary>
        public int Month {
            get { return m_month; }
        }

        private int m_year;

        /// <summary>
        /// 获取年
        /// </summary>
        public int Year {
            get { return m_year; }
        }

        /// <summary>
        /// 清除数据
        /// </summary>
        public void delete() {
        }
    }
}
