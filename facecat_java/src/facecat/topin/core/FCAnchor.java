/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.core;

/**
 * 锚定信息
 */
public class FCAnchor {
    /*
    * 构造函数
    */
    public FCAnchor() {
    }

    /**
     * 创建锚定信息
     *
     * @param left 左侧
     * @param top 顶部
     * @param right 右侧
     * @param bottom 底部
     */
    public FCAnchor(boolean left, boolean top, boolean right, boolean bottom) {
        this.left = left;
        this.top = top;
        this.right = right;
        this.bottom = bottom;
    }

    /**
     * 底部
     */
    public boolean bottom;

    /**
     * 左侧
     */
    public boolean left;

    /**
     * 右侧
     */
    public boolean right;

    /**
     * 顶部
     */
    public boolean top;

    public FCAnchor clone() {
        FCAnchor varCopy = new FCAnchor();
        varCopy.bottom = this.bottom;
        varCopy.left = this.left;
        varCopy.right = this.right;
        varCopy.top = this.top;
        return varCopy;
    }
}
