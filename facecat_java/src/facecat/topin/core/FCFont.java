/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.core;

public class FCFont {
    /**
    * 创建字体
    */
    public FCFont() {
    }

    /**
     * 创建字体
     *
     * @param fontFamily 字体
     * @param fontSize 字号
     * @param bold 是否粗体
     * @param underline 是否有下划线
     * @param italic 是否斜体
     */
    public FCFont(String fontFamily, float fontSize, boolean bold, boolean underline, boolean italic) {
        m_fontFamily = fontFamily;
        m_fontSize = fontSize;
        m_bold = bold;
        m_underline = underline;
        m_italic = italic;
    }

    /**
     * 创建字体
     *
     * @param fontFamily 字体
     * @param fontSize 字号
     * @param bold 是否粗体
     * @param underline 是否有下划线
     * @param italic 是否斜体
     * @param strikeout 是否有删除线
     */
    public FCFont(String fontFamily, float fontSize, boolean bold, boolean underline, boolean italic, boolean strikeout) {
        m_fontFamily = fontFamily;
        m_fontSize = fontSize;
        m_bold = bold;
        m_underline = underline;
        m_italic = italic;
        m_strikeout = strikeout;
    }

    /**
     * 是否粗体
     */
    public boolean m_bold;

    /**
     * 字体
     */
    public String m_fontFamily = "Arial";

    /**
     * 字体大小
     */
    public float m_fontSize = 12;

    /**
     * 是否斜体
     */
    public boolean m_italic;

    /**
     * 是否有删除线
     */
    public boolean m_strikeout;

    /**
     * 是否有下划线
     */
    public boolean m_underline;
}
