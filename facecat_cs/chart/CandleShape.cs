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

namespace FaceCat {
    /// <summary>
    /// K线
    /// </summary>
    [Serializable()]
    public class CandleShape : BaseShape {
        /// <summary>
        /// 创建K线
        /// </summary>
        public CandleShape() {
            ZOrder = 1;
        }

        protected int m_closeField = FCDataTable.NULLFIELD;

        /// <summary>
        /// 获取或设置收盘价字段
        /// </summary>
        public virtual int CloseField {
            get { return m_closeField; }
            set { m_closeField = value; }
        }

        protected int m_colorField = FCDataTable.NULLFIELD;

        /// <summary>
        /// 获取或设置颜色的字段
        /// </summary>
        public virtual int ColorField {
            get { return m_colorField; }
            set { m_colorField = value; }
        }

        protected String m_closeFieldText;

        /// <summary>
        /// 获取或设置收盘价的显示文字
        /// </summary>
        public virtual String CloseFieldText {
            get {
                return m_closeFieldText;
            }
            set { m_closeFieldText = value; }
        }

        protected long m_downColor = FCColor.argb(82, 255, 255);

        /// <summary>
        /// 获取或设置阴线的颜色
        /// </summary>
        public virtual long DownColor {
            get { return m_downColor; }
            set { m_downColor = value; }
        }

        protected int m_highField = FCDataTable.NULLFIELD;

        /// <summary>
        /// 获取或设置最高价字段
        /// </summary>
        public virtual int HighField {
            get { return m_highField; }
            set { m_highField = value; }
        }

        protected String m_highFieldText;

        /// <summary>
        /// 获取或设置最高价的显示文字
        /// </summary>
        public virtual String HighFieldText {
            get {
                return m_highFieldText;
            }
            set { m_highFieldText = value; }
        }

        protected int m_lowField = FCDataTable.NULLFIELD;

        /// <summary>
        /// 获取或设置最低价字段
        /// </summary>
        public virtual int LowField {
            get { return m_lowField; }
            set { m_lowField = value; }
        }

        protected String m_lowFieldText;

        /// <summary>
        /// 获取或设置最低价的显示文字
        /// </summary>
        public virtual String LowFieldText {
            get {
                return m_lowFieldText;
            }
            set { m_lowFieldText = value; }
        }

        protected int m_openField = FCDataTable.NULLFIELD;

        /// <summary>
        /// 获取或设置开盘价字段
        /// </summary>
        public virtual int OpenField {
            get { return m_openField; }
            set { m_openField = value; }
        }

        private String m_openFieldText;

        /// <summary>
        /// 获取或设置开盘价的显示文字
        /// </summary>
        public virtual String OpenFieldText {
            get {
                return m_openFieldText;
            }
            set { m_openFieldText = value; }
        }

        protected bool m_showMaxMin = true;

        /// <summary>
        /// 获取或设置是否显示最大最小值
        /// </summary>
        public virtual bool ShowMaxMin {
            get { return m_showMaxMin; }
            set { m_showMaxMin = value; }
        }

        protected CandleStyle m_style = CandleStyle.Rect;

        /// <summary>
        /// 获取或设置线柱的类型
        /// </summary>
        public virtual CandleStyle Style {
            get { return m_style; }
            set { m_style = value; }
        }

        protected int m_styleField = FCDataTable.NULLFIELD;

        /// <summary>
        /// 获取或设置样式字段
        /// -10000:不画 0:阳线空心阴线实心 1:空心矩形 2:实心矩形 3:美国线 4:收盘线 5:宝塔线
        /// </summary>
        public virtual int StyleField {
            get { return m_styleField; }
            set { m_styleField = value; }
        }

        protected long m_tagColor = FCColor.argb(255, 255, 255);

        /// <summary>
        /// 获取或设置最大最小值标签的颜色
        /// </summary>
        public virtual long TagColor {
            get { return m_tagColor; }
            set { m_tagColor = value; }
        }

        protected long m_upColor = FCColor.argb(255, 82, 82);

        /// <summary>
        /// 获取或设置阳线颜色
        /// </summary>
        public virtual long UpColor {
            get { return m_upColor; }
            set { m_upColor = value; }
        }

        /// <summary>
        /// 获取基础字段
        /// </summary>
        /// <returns></returns>
        public override int getBaseField() {
            return m_closeField;
        }

        /// <summary>
        /// 由字段名称获取字段标题
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <returns>字段标题</returns>
        public override String getFieldText(int fieldName) {
            if (fieldName == m_closeField) {
                return CloseFieldText;
            }
            else if (fieldName == m_highField) {
                return HighFieldText;
            }
            else if (fieldName == m_lowField) {
                return LowFieldText;
            }
            else if (fieldName == m_openField) {
                return OpenFieldText;
            }
            else {
                return null;
            }
        }

        /// <summary>
        /// 获取所有字段
        /// </summary>
        /// <returns></returns>
        public override int[] getFields() {
            return new int[] { m_closeField, m_highField, m_lowField, m_openField };
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">返回属性值</param>
        /// <param name="type">返回属性类型</param>
        public override void getProperty(String name, ref String value, ref String type) {
            if (name == "closefield") {
                type = "int";
                value = FCStr.convertIntToStr(CloseField);
            }
            else if (name == "colorfield") {
                type = "int";
                value = FCStr.convertIntToStr(ColorField);
            }
            else if (name == "closefieldtext") {
                type = "String";
                value = CloseFieldText;
            }
            else if (name == "downcolor") {
                type = "color";
                value = FCStr.convertColorToStr(DownColor);
            }
            else if (name == "highfield") {
                type = "int";
                value = FCStr.convertIntToStr(HighField);
            }
            else if (name == "highfieldtext") {
                type = "String";
                value = HighFieldText;
            }
            else if (name == "lowfield") {
                type = "int";
                value = FCStr.convertIntToStr(LowField);
            }
            else if (name == "lowfieldtext") {
                type = "String";
                value = LowFieldText;
            }
            else if (name == "openfield") {
                type = "int";
                value = FCStr.convertIntToStr(OpenField);
            }
            else if (name == "openfieldtext") {
                type = "String";
                value = OpenFieldText;
            }
            else if (name == "showmaxmin") {
                type = "bool";
                value = FCStr.convertBoolToStr(ShowMaxMin);
            }
            else if (name == "style") {
                type = "enum:CandleStyle";
                CandleStyle style = Style;
                if (style == CandleStyle.American) {
                    value = "American";
                }
                else if (style == CandleStyle.CloseLine) {
                    value = "CloseLine";
                }
                else if (style == CandleStyle.Tower) {
                    value = "Tower";
                }
                else {
                    value = "Rect";
                }
            }
            else if (name == "stylefield") {
                type = "int";
                value = FCStr.convertIntToStr(StyleField);
            }
            else if (name == "tagcolor") {
                type = "double";
                value = FCStr.convertDoubleToStr(TagColor);
            }
            else if (name == "upcolor") {
                type = "color";
                value = FCStr.convertDoubleToStr(UpColor);
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
            ArrayList<String> propertyNames = base.getPropertyNames();
            propertyNames.AddRange(new String[] { "CloseField", "ColorField", "CloseFieldText", "DownColor", "DownColor",
            "HighFieldText", "LowField", "LowFieldText", "OpenField", "OpenFieldText", "ShowMaxMin", "Style", "StyleField",
            "TagColor", "UpColor"});
            return propertyNames;
        }

        /// <summary>
        /// 获取选中点的颜色
        /// </summary>
        /// <returns>颜色</returns>
        public override long getSelectedColor() {
            return m_downColor;
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        public override void setProperty(String name, String value) {
            if (name == "closefield") {
                CloseField = FCStr.convertStrToInt(value);
            }
            else if (name == "colorfield") {
                ColorField = FCStr.convertStrToInt(value);
            }
            else if (name == "closefieldtext") {
                CloseFieldText = value;
            }
            else if (name == "downcolor") {
                DownColor = FCStr.convertStrToColor(value);
            }
            else if (name == "highfield") {
                HighField = FCStr.convertStrToInt(value);
            }
            else if (name == "highfieldtext") {
                HighFieldText = value;
            }
            else if (name == "lowfield") {
                LowField = FCStr.convertStrToInt(value);
            }
            else if (name == "lowfieldtext") {
                LowFieldText = value;
            }
            else if (name == "openfield") {
                OpenField = FCStr.convertStrToInt(value);
            }
            else if (name == "openfieldtext") {
                OpenFieldText = value;
            }
            else if (name == "showmaxmin") {
                ShowMaxMin = FCStr.convertStrToBool(value);
            }
            else if (name == "style") {
                value = value.ToLower();
                if (value == "american") {
                    Style = CandleStyle.American;
                }
                else if (value == "closeline") {
                    Style = CandleStyle.CloseLine;
                }
                else if (value == "tower") {
                    Style = CandleStyle.Tower;
                }
                else {
                    Style = CandleStyle.Rect;
                }
            }
            else if (name == "stylefield") {
                StyleField = FCStr.convertStrToInt(value);
            }
            else if (name == "tagcolor") {
                TagColor = FCStr.convertStrToColor(value);
            }
            else if (name == "upcolor") {
                UpColor = FCStr.convertStrToColor(value);
            }
            else {
                base.setProperty(name, value);
            }
        }
    }
}
