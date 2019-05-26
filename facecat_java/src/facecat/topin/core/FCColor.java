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
* 颜色
*/
public class FCColor {

    /**
     * 背景色
     */
    public static long Back = -200000000001L;

    /**
     * 边线颜色
     */
    public static long Border = -200000000002L;

    /**
     * 前景色
     */
    public static long Text = -200000000003L;

    /**
     * 不可用的背景色
     */
    public static long DisabledBack = -200000000004L;

    /**
     * 不可用的前景色
     */
    public static long DisabledText = -200000000005L;

    /**
     * 触摸悬停的背景色
     */
    public static long Hovered = -200000000006L;

    /**
     * 触摸被按下的背景色
     */
    public static long Pushed = -200000000007L;

    /**
     * 空颜色
     */
    public static long None = -200000000000L;

    public static long argb(int r, int g, int b) {
        return ((r | (g << 8)) | (b << 0x10));
    }

    /**
     * 获取RGB颜色
     *
     * @param r 红色值
     * @param g 绿色值
     * @param b 蓝色值
     * @returns RGB颜色
     */
    public static long argb(int a, int r, int g, int b) {
        if (a == 255) {
            return argb(r, g, b);
        } else if (a == 0) {
            return None;
        } else {
            int rgb = (int) argb(r, g, b);
            long argb = -((long) rgb * 1000 + a);
            return argb;
        }
    }

    /**
     * 获取RGB颜色
     *
     * @param a 透明值
     */
    public static void toArgb(FCPaint paint, long color, RefObject<Integer> a, RefObject<Integer> r, RefObject<Integer> g, RefObject<Integer> b) {
        if (paint != null) {
            color = paint.getColor(color);
        }
        a.argvalue = 255;
        if (color < 0) {
            color = -color;
            if (color < 1) {
                a.argvalue = 255;
            } else {
                a.argvalue = (int) (color - color / 1000 * 1000);
            }
            color /= 1000;
        }
        r.argvalue = (int) (color & 0xff);
        g.argvalue = (int) ((color >> 8) & 0xff);
        b.argvalue = (int) ((color >> 0x10) & 0xff);
    }

    /**
     * 获取比例色
     *
     * @param originalColor 原始色
     * @param ratio 比例
     */
    public static long ratioColor(FCPaint paint, long originalColor, double ratio) {
        int a = 0, r = 0, g = 0, b = 0;
        RefObject<Integer> tempRef_a = new RefObject<Integer>(a);
        RefObject<Integer> tempRef_r = new RefObject<Integer>(r);
        RefObject<Integer> tempRef_g = new RefObject<Integer>(g);
        RefObject<Integer> tempRef_b = new RefObject<Integer>(b);
        toArgb(paint, originalColor, tempRef_a, tempRef_r, tempRef_g, tempRef_b);
        a = tempRef_a.argvalue;
        r = tempRef_a.argvalue;
        g = tempRef_a.argvalue;
        b = tempRef_a.argvalue;
        r = (int) (r * ratio);
        g = (int) (g * ratio);
        b = (int) (b * ratio);
        if (r > 255) {
            r = 255;
        }
        if (g > 255) {
            g = 255;
        }
        if (b > 255) {
            b = 255;
        }
        return FCColor.argb(a, r, g, b);
    }

    /**
     * 获取反色
     */
    public static long reverse(FCPaint paint, long originalColor) {
        int a = 0, r = 0, g = 0, b = 0;
        RefObject<Integer> tempRef_a = new RefObject<Integer>(a);
        RefObject<Integer> tempRef_r = new RefObject<Integer>(r);
        RefObject<Integer> tempRef_g = new RefObject<Integer>(g);
        RefObject<Integer> tempRef_b = new RefObject<Integer>(b);
        toArgb(paint, originalColor, tempRef_a, tempRef_r, tempRef_g, tempRef_b);
        a = tempRef_a.argvalue;
        r = tempRef_a.argvalue;
        g = tempRef_a.argvalue;
        b = tempRef_a.argvalue;
        return FCColor.argb(a, 255 - r, 255 - g, 255 - b);
    }
}
