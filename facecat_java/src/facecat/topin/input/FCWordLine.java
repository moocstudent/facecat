/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.input;

/**
 * 字符行
 */
public class FCWordLine {

    /**
     * 字符行
     */
    public FCWordLine() {

    }

    /**
     * 创建行
     *
     * @param start 开始索引
     * @param end 结束索引
     */
    public FCWordLine(int start, int end) {
        m_start = start;
        m_end = end;
    }

    /**
     * 结束索引
     */
    public int m_end;
    /**
     * 开始索引
     */
    public int m_start;

    public FCWordLine clone() {
        FCWordLine varCopy = new FCWordLine();

        varCopy.m_end = this.m_end;
        varCopy.m_start = this.m_start;

        return varCopy;
    }
}
