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
using System.Text;
using System.Drawing;

namespace FaceCat {
    /// <summary>
    /// 文字
    /// </summary>
    [Serializable()]
    public class TextShape : BaseShape {
        /// <summary>
        /// 创建文字
        /// </summary>
        public TextShape() {
            ZOrder = 4;
        }

        protected int m_colorField = FCDataTable.NULLFIELD;

        /// <summary>
        /// 获取或设置颜色字段
        /// </summary>
        public virtual int ColorField {
            get { return m_colorField; }
            set { m_colorField = value; }
        }

        protected int m_fieldName = FCDataTable.NULLFIELD;

        /// <summary>
        /// 获取或设置字段
        /// </summary>
        public virtual int FieldName {
            get { return m_fieldName; }
            set { m_fieldName = value; }
        }

        protected FCFont m_font = new FCFont();

        /// <summary>
        /// 获取或设置字体
        /// </summary>
        public virtual FCFont Font {
            get { return m_font; }
            set { m_font = value; }
        }

        protected int m_styleField = FCDataTable.NULLFIELD;

        /// <summary>
        /// 获取或设置样式字段
        /// -10000:不画
        /// </summary>
        public virtual int StyleField {
            get { return m_styleField; }
            set { m_styleField = value; }
        }

        protected String m_text;

        /// <summary>
        /// 获取或设置绘制的文字
        /// </summary>
        public virtual String Text {
            get { return m_text; }
            set { m_text = value; }
        }

        protected long m_textColor = FCColor.argb(255, 255, 255);

        /// <summary>
        /// 获取或设置前景色
        /// </summary>
        public virtual long TextColor {
            get { return m_textColor; }
            set { m_textColor = value; }
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">返回属性值</param>
        /// <param name="type">返回属性类型</param>
        public override void getProperty(String name, ref String value, ref String type) {
            if (name == "colorfield") {
                type = "int";
                value = FCStr.convertIntToStr(ColorField);
            }
            else if (name == "fieldname") {
                type = "int";
                value = FCStr.convertIntToStr(FieldName);
            }
            else if (name == "font") {
                type = "font";
                value = FCStr.convertFontToStr(Font);
            }
            else if (name == "stylefield") {
                type = "int";
                value = FCStr.convertIntToStr(StyleField);
            }
            else if (name == "text") {
                type = "String";
                value = Text;
            }
            else if (name == "textcolor") {
                type = "color";
                value = FCStr.convertColorToStr(TextColor);
            }
            else {
                base.getProperty(name, ref value, ref type);
            }
        }

        /// <summary>
        /// 获取属性名称列表
        /// </summary>
        /// <returns></returns>
        public override ArrayList<String> getPropertyNames() {
            ArrayList<String> propertyNames = new ArrayList<String>();
            propertyNames.AddRange(new String[] { "ColorField", "FieldName", "Font", "StyleField", "Text", "TextColor" });
            return propertyNames;
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        public override void setProperty(String name, String value) {
            if (name == "colorfield") {
                ColorField = FCStr.convertStrToInt(value);
            }
            else if (name == "fieldname") {
                FieldName = FCStr.convertStrToInt(value);
            }
            else if (name == "font") {
                Font = FCStr.convertStrToFont(value);
            }
            else if (name == "stylefield") {
                StyleField = FCStr.convertStrToInt(value);
            }
            else if (name == "text") {
                Text = value;
            }
            else if (name == "textcolor") {
                TextColor = FCStr.convertStrToColor(value);
            }
            else {
                base.setProperty(name, value);
            }
        }
    }
}
