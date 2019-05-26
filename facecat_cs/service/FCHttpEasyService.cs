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
using System.Net;
using System.IO;
using FaceCat;
using System.IO.Compression;
using OwLib;

namespace FaceCat {
    /// <summary>
    /// HTTP的GET服务
    /// </summary>
    public interface FCHttpEasyService {
        /// <summary>
        /// 请求数据
        /// </summary>
        /// <param name="data">请求数据</param>
        /// <returns>数据</returns>
        int onReceive(FCHttpData data);
    }
}