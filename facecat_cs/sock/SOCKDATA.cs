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
using System.Net.Sockets;

namespace FaceCat {
    public class SOCKDATA {
        public byte[] m_buffer = null;
        public int m_bufferRemain;
        public bool m_get;
        public int m_head;
        public int m_headSize = 4;
        public byte[] m_headStr = new byte[4];
        public int m_index;
        public int m_len;
        public int m_pos;
        public int m_hSocket;
        public Socket m_socket;
        public byte[] m_str = null;
        public int m_strRemain;
        public bool m_submit;
    }

    public enum ConnectStatus {
        SUCCESS,
        CONNECT_PROXY_FAIL,
        NOT_CONNECT_PROXY,
        CONNECT_SERVER_FAIL
    }
}
