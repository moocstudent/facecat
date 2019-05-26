/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-捂脸鹿创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

using System;
using System.Collections.Generic;

namespace FaceCat {
    /// <summary>
    /// 图层标题栏
    /// </summary>
    [Serializable()]
    public class ChartTitleBar : FCProperty {
        /// <summary>
        /// 析构函数
        /// </summary>
        ~ChartTitleBar() {
            delete();
        }

        protected bool m_allowUserPaint;

        /// <summary>
        /// 获取或设置是否允许用户绘图
        /// </summary>
        public virtual bool AllowUserPaint {
            get { return m_allowUserPaint; }
            set { m_allowUserPaint = value; }
        }

        protected FCFont m_font = new FCFont();

        /// <summary>
        /// 获取或设置字体
        /// </summary>
        public virtual FCFont Font {
            get { return m_font; }
            set { m_font = value; }
        }

        protected int m_height = 22;

        /// <summary>
        /// 获取或设置标题高度
        /// </summary>
        public virtual int Height {
            get { return m_height; }
            set { m_height = value; }
        }

        protected bool m_isDeleted;

        /// <summary>
        /// 获取或设置是否已被销毁
        /// </summary>
        public virtual bool IsDeleted {
            get { return m_isDeleted; }
        }

        protected int m_maxLine = 3;

        /// <summary>
        /// 获取或设置最大标题行数
        /// </summary>
        public virtual int MaxLine {
            get { return m_maxLine; }
            set { m_maxLine = value; }
        }

        protected bool m_showUnderLine = true;

        /// <summary>
        /// 获取或设置是否显示标题下面的线
        /// </summary>
        public virtual bool ShowUnderLine {
            get { return m_showUnderLine; }
            set { m_showUnderLine = value; }
        }

        protected String m_text = String.Empty;

        /// <summary>
        /// 获取或设置层的标题
        /// </summary>
        public virtual String Text {
            get { return m_text; }
            set { m_text = value; }
        }

        protected long m_textColor = FCColor.argb(255, 255, 255);

        /// <summary>
        /// 获取或设置标题的文字颜色
        /// </summary>
        public virtual long TextColor {
            get { return m_textColor; }
            set { m_textColor = value; }
        }

        protected ArrayList<CTitle> m_titles = new ArrayList<CTitle>();

        /// <summary>
        /// 获取或设置标题
        /// </summary>
        public virtual ArrayList<CTitle> Titles {
            get { return m_titles; }
            set { m_titles = value; }
        }

        protected long m_underLineColor = FCColor.argb(80, 0, 0);

        /// <summary>
        /// 获取或设置是否显示标题下面的线的颜色
        /// </summary>
        public virtual long UnderLineColor {
            get { return m_underLineColor; }
            set { m_underLineColor = value; }
        }

        protected bool m_visible = true;

        /// <summary>
        /// 获取或设置是否显示标题
        /// </summary>
        public virtual bool Visible {
            get { return m_visible; }
            set { m_visible = value; }
        }

        /// <summary>
        /// 销毁资源
        /// </summary>
        public void delete() {
            m_isDeleted = true;
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">返回属性值</param>
        /// <param name="type">返回属性类型</param>
        public virtual void getProperty(String name, ref String value, ref String type) {
            if (name == "allowuserpaint") {
                type = "bool";
                value = FCStr.convertBoolToStr(AllowUserPaint);
            }
            else if (name == "font") {
                type = "font";
                value = FCStr.convertFontToStr(Font);
            }
            else if (name == "textcolor") {
                type = "color";
                value = FCStr.convertColorToStr(TextColor);
            }
            else if (name == "height") {
                type = "int";
                value = FCStr.convertIntToStr(Height);
            }
            else if (name == "maxline") {
                type = "int";
                value = FCStr.convertIntToStr(MaxLine);
            }
            else if (name == "showunderline") {
                type = "bool";
                value = FCStr.convertBoolToStr(ShowUnderLine);
            }
            else if (name == "text") {
                type = "String";
                value = Text;
            }
            else if (name == "underlinecolor") {
                type = "color";
                value = FCStr.convertColorToStr(UnderLineColor);
            }
            else if (name == "visible") {
                type = "bool";
                value = FCStr.convertBoolToStr(Visible);
            }
        }

        /// <summary>
        /// 获取属性名称列表
        /// </summary>
        /// <returns></returns>
        public virtual ArrayList<String> getPropertyNames() {
            ArrayList<String> propertyNames = new ArrayList<String>();
            propertyNames.AddRange(new String[] { "AllowUserPaint", "Font", "Height", "MaxLine", "ShowUnderLine",
            "Text", "TextColor", "UnderLineColor", "Visible"});
            return propertyNames;
        }

        /// <summary>
        /// 重绘方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="div">图层</param>
        /// <param name="rect">区域</param>
        public virtual void onPaint(FCPaint paint, ChartDiv div, FCRect rect) {

        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        public virtual void setProperty(String name, String value) {
            if (name == "allowuserpaint") {
                AllowUserPaint = FCStr.convertStrToBool(value);
            }
            else if (name == "font") {
                Font = FCStr.convertStrToFont(value);
            }
            else if (name == "height") {
                Height = FCStr.convertStrToInt(value);
            }
            else if (name == "maxline") {
                MaxLine = FCStr.convertStrToInt(value);
            }
            else if (name == "showunderline") {
                ShowUnderLine = FCStr.convertStrToBool(value);
            }
            else if (name == "text") {
                Text = value;
            }
            else if (name == "textclor") {
                TextColor = FCStr.convertStrToColor(value);
            }
            else if (name == "underlinecolor") {
                UnderLineColor = FCStr.convertStrToColor(value);
            }
            else if (name == "visible") {
                Visible = FCStr.convertStrToBool(value);
            }
        }
    }

    /// <summary>
    /// 标题
    /// </summary>
    [Serializable()]
    public class CTitle : FCProperty {
        /// <summary>
        /// 创建标题
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="fieldText">字段文字</param>
        /// <param name="textColor">文字颜色</param>
        /// <param name="digit">保留小数位数</param>
        /// <param name="visible">是否可见</param>
        public CTitle(int fieldName, String fieldText, long textColor, int digit, bool visible) {
            m_fieldName = fieldName;
            m_fieldText = fieldText;
            m_textColor = textColor;
            m_digit = digit;
            m_visible = visible;
        }

        private int m_digit = 2;

        /// <summary>
        /// 获取或设置保留小数的位数
        /// </summary>
        public int Digit {
            get { return m_digit; }
            set { m_digit = value; }
        }

        private int m_fieldName;

        /// <summary>
        /// 获取或设置字段名称
        /// </summary>
        public int FieldName {
            get { return m_fieldName; }
            set { m_fieldName = value; }
        }

        private String m_fieldText;

        /// <summary>
        /// 获取或设置字段文字
        /// </summary>
        public String FieldText {
            get { return m_fieldText; }
            set { m_fieldText = value; }
        }

        private TextMode m_fieldTextMode = TextMode.Full;

        /// <summary>
        /// 获取或设置标题显示模式
        /// </summary>
        public TextMode FieldTextMode {
            get { return m_fieldTextMode; }
            set { m_fieldTextMode = value; }
        }

        private String m_fieldTextSeparator = " ";

        /// <summary>
        /// 获取或设置标题的分隔符
        /// </summary>
        public String FieldTextSeparator {
            get { return m_fieldTextSeparator; }
            set { m_fieldTextSeparator = value; }
        }

        private long m_textColor;

        /// <summary>
        /// 获取或设置文字的颜色
        /// </summary>
        public long TextColor {
            get { return m_textColor; }
            set { m_textColor = value; }
        }

        private bool m_visible;

        /// <summary>
        /// 获取或设置是否可见
        /// </summary>
        public bool Visible {
            get { return m_visible; }
            set { m_visible = value; }
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">返回属性值</param>
        /// <param name="type">返回属性类型</param>
        public virtual void getProperty(String name, ref String value, ref String type) {
            if (name == "digit") {
                type = "int";
                value = FCStr.convertIntToStr(Digit);
            }
            else if (name == "fieldname") {
                type = "int";
                value = FCStr.convertIntToStr(FieldName);
            }
            else if (name == "fieldtext") {
                type = "text";
                value = FieldText;
            }
            else if (name == "fieldtextmode") {
                type = "enum:TextMode";
                TextMode fieldTextMode = FieldTextMode;
                if (fieldTextMode == TextMode.Field) {
                    value = "field";
                }
                else if (fieldTextMode == TextMode.Full) {
                    value = "full";
                }
                else if (fieldTextMode == TextMode.None) {
                    value = "none";
                }
                else {
                    value = "value";
                }
            }
            else if (name == "fieldtextseparator") {
                value = FieldTextSeparator;
            }
            else if (name == "textcolor") {
                type = "color";
                value = FCStr.convertColorToStr(TextColor);
            }
            else if (name == "visible") {
                type = "bool";
                value = FCStr.convertBoolToStr(Visible);
            }
        }

        /// <summary>
        /// 获取属性名称列表
        /// </summary>
        /// <returns></returns>
        public virtual ArrayList<String> getPropertyNames() {
            ArrayList<String> propertyNames = new ArrayList<String>();
            propertyNames.AddRange(new String[] { "Digit", "FieldName", "FieldText", "FieldTextMode", "FieldTextSeparator", "TextColor", "Visible" });
            return propertyNames;
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        public virtual void setProperty(String name, String value) {
            if (name == "digit") {
                Digit = FCStr.convertStrToInt(value);
            }
            else if (name == "fieldname") {
                FieldName = FCStr.convertStrToInt(value);
            }
            else if (name == "fieldtext") {
                FieldText = value;
            }
            else if (name == "fieldtextmode") {
                value = value.ToLower();
                if (value == "field") {
                    FieldTextMode = TextMode.Field;
                }
                else if (value == "full") {
                    FieldTextMode = TextMode.Full;
                }
                else if (value == "none") {
                    FieldTextMode = TextMode.None;
                }
                else {
                    FieldTextMode = TextMode.Value;
                }
            }
            else if (name == "fieldtextseparator") {
                FieldTextSeparator = value;
            }
            else if (name == "textcolor") {
                TextColor = FCStr.convertStrToColor(value);
            }
            else if (name == "visible") {
                Visible = FCStr.convertStrToBool(value);
            }
        }
    }
}
