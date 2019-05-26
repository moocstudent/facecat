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
    /// 月
    /// </summary>
    [Serializable()]
    public class CMonth {
        /// <summary>
        /// 创建月
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        public CMonth(int year, int month) {
            m_year = year;
            m_month = month;
            createDays();
        }

        private HashMap<int, CDay> m_days = new HashMap<int, CDay>();

        /// <summary>
        /// 获取或设置日的集合
        /// </summary>
        public HashMap<int, CDay> Days {
            get { return m_days; }
        }

        /// <summary>
        /// 获取月的日数
        /// </summary>
        public int DaysInMonth {
            get {
                return DateTime.DaysInMonth(m_year, m_month);
            }
        }

        /// <summary>
        /// 获取该月的第一日
        /// </summary>
        public CDay FirstDay {
            get {
                return m_days.get(1);
            }
        }

        /// <summary>
        /// 获取该月的最后一日
        /// </summary>
        public CDay LastDay {
            get {
                return m_days.get(m_days.size());
            }
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
        /// 创建日的集合
        /// </summary>
        private void createDays() {
            int daysInMonth = DaysInMonth;
            for (int i = 1; i <= daysInMonth; i++) {
                m_days.put(i, new CDay(m_year, m_month, i));
            }
        }

        /// <summary>
        /// 清除数据
        /// </summary>
        public void delete() {
            foreach (CDay day in m_days.Values) {
                day.delete();
            }
            m_days.clear();
        }
    }
}
