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
    /// 年
    /// </summary>
    [Serializable()]
    public class CYear {
        /// <summary>
        /// 创建年
        /// </summary>
        /// <param name="year">年份</param>
        public CYear(int year) {
            m_year = year;
            CreateMonths();
        }

        private HashMap<int, CMonth> m_months = new HashMap<int, CMonth>();

        /// <summary>
        /// 获取或设置月的集合
        /// </summary>
        public HashMap<int, CMonth> Months {
            get { return m_months; }
        }

        private int m_year;

        /// <summary>
        /// 获取年份
        /// </summary>
        public int Year {
            get { return m_year; }
        }

        /// <summary>
        /// 创建月
        /// </summary>
        private void CreateMonths() {
            for (int i = 1; i <= 12; i++) {
                m_months.put(i, new CMonth(m_year, i));
            }
        }

        /// <summary>
        /// 清除数据
        /// </summary>
        public void delete() {
            foreach (CMonth month in m_months.Values) {
                month.delete();
            }
            m_months.clear();

        }
    }
}
