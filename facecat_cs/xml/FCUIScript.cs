/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace FaceCat {
    /// <summary>
    /// 脚本接口类
    /// </summary>
    public interface FCUIScript {
        /// <summary>
        /// 获取是否被销毁
        /// </summary>
        bool IsDeleted {
            get;
        }

        /// <summary>
        /// 获取或设置XML对象
        /// </summary>
        FCUIXml Xml {
            get;
            set;
        }

        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="function">方法文本</param>
        /// <returns>返回值</returns>
        String callFunction(String function);

        /// <summary>
        /// 销毁对象
        /// </summary>
        void delete();

        /// <summary>
        /// 设置脚本
        /// </summary>
        /// <param name="text">脚本</param>
        void setText(String text);
    }
}
