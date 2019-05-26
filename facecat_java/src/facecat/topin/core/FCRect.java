/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.core;

/*
* 矩形
*/
public class FCRect {
    /**
     * 创建矩形
     */
    public FCRect() {
    }

    /**
     * 创建矩形
     */
    public FCRect(int left, int top, int right, int bottom) {
        this.left = left;
        this.top = top;
        this.right = right;
        this.bottom = bottom;
    }

    public FCRect(float left, float top, float right, float bottom) {
        this.left = (int) left;
        this.top = (int) top;
        this.right = (int) right;
        this.bottom = (int) bottom;
    }

    /**
     * 左侧坐标
     */
    public int left;

    /**
     * 顶部坐标
     */
    public int top;

    /**
     * 右侧坐标
     */
    public int right;

    /**
     * 底部坐标
     */
    public int bottom;

    public FCRect clone() {
        FCRect varCopy = new FCRect();

        varCopy.left = this.left;
        varCopy.top = this.top;
        varCopy.right = this.right;
        varCopy.bottom = this.bottom;

        return varCopy;
    }
}
