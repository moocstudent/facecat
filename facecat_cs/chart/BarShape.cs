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
    /// 柱状图
    /// </summary>
    [Serializable()]
    public class BarShape : BaseShape {
        /// <summary>
        /// 创建柱状图
        /// </summary>
        public BarShape() {
            ZOrder = 0;
        }

        protected int m_colorField = FCDataTable.NULLFIELD;

        /// <summary>
        /// 获取或设置颜色的字段
        /// </summary>
        public virtual int ColorField {
            get { return m_colorField; }
            set { m_colorField = value; }
        }

        protected long m_downColor = FCColor.argb(82, 255, 255);

        /// <summary>
        /// 获取或设置阴线的颜色
        /// </summary>
        public virtual long DownColor {
            get { return m_downColor; }
            set { m_downColor = value; }
        }

        protected int m_fieldName = FCDataTable.NULLFIELD;

        /// <summary>
        /// 获取或设置字段名称
        /// </summary>
        public virtual int FieldName {
            get { return m_fieldName; }
            set { m_fieldName = value; }
        }

        protected int m_fieldName2 = FCDataTable.NULLFIELD;

        /// <summary>
        /// 获取或设置字段名称2
        /// </summary>
        public virtual int FieldName2 {
            get { return m_fieldName2; }
            set { m_fieldName2 = value; }
        }

        protected String m_fieldText = String.Empty;

        /// <summary>
        /// 获取或设置显示的标题名称
        /// </summary>
        public virtual String FieldText {
            get {
                return m_fieldText;
            }
            set { m_fieldText = value; }
        }

        private String m_fieldText2 = String.Empty;

        /// <summary>
        /// 获取或设置显示的标题名称2
        /// </summary>
        public virtual String FieldText2 {
            get { return m_fieldText2; }
            set { m_fieldText2 = value; }
        }

        protected float m_lineWidth = 1;

        /// <summary>
        /// 获取或设置线的宽度
        /// </summary>
        public virtual float LineWidth {
            get { return m_lineWidth; }
            set { m_lineWidth = value; }
        }

        protected BarStyle m_style = BarStyle.Rect;

        /// <summary>
        /// 获取或设置线条的样式
        /// </summary>
        public virtual BarStyle Style {
            get { return m_style; }
            set { m_style = value; }
        }

        protected int m_styleField = FCDataTable.NULLFIELD;

        /// <summary>
        /// 获取或设置样式字段
        /// -10000:不画 -1:虚线空心矩形 0:实心矩形 1:实线空心矩形  2:线
        /// </summary>
        public virtual int StyleField {
            get { return m_styleField; }
            set { m_styleField = value; }
        }

        protected long m_upColor = FCColor.argb(255, 82, 82);

        /// <summary>
        /// 获取或设置阳线的颜色
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
            return m_fieldName;
        }

        /// <summary>
        /// 由字段名称获取字段标题
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <returns>字段标题</returns>
        public override String getFieldText(int fieldName) {
            if (fieldName == m_fieldName) {
                return FieldText;
            }
            else if (fieldName == m_fieldName2) {
                return FieldText2;
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
            if (m_fieldName2 == FCDataTable.NULLFIELD) {
                return new int[] { m_fieldName };
            }
            else {
                return new int[] { m_fieldName, m_fieldName2 };
            }
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
            else if (name == "downcolor") {
                type = "color";
                value = FCStr.convertColorToStr(DownColor);
            }
            else if (name == "fieldname") {
                type = "int";
                value = FCStr.convertIntToStr(FieldName);
            }
            else if (name == "fieldname2") {
                type = "int";
                value = FCStr.convertIntToStr(FieldName2);
            }
            else if (name == "fieldtext") {
                type = "String";
                value = FieldText;
            }
            else if (name == "fieldtext2") {
                type = "String";
                value = FieldText2;
            }
            else if (name == "linewidth") {
                type = "float";
                value = FCStr.convertFloatToStr(LineWidth);
            }
            else if (name == "style") {
                type = "enum:BarStyle";
                BarStyle style = Style;
                if (style == BarStyle.Line) {
                    value = "Line";
                }
                else {
                    value = "Rect";
                }
            }
            else if (name == "stylefield") {
                type = "int";
                value = FCStr.convertIntToStr(StyleField);
            }
            else if (name == "upcolor") {
                type = "double";
                value = FCStr.convertColorToStr(UpColor);
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
            propertyNames.AddRange(new String[] { "ColorField", "DownColor", "FieldName", "FieldName2",
            "FieldText", "FieldText2", "LineWidth", "Style", "StyleField", "UpColor" });
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
            if (name == "colorfield") {
                ColorField = FCStr.convertStrToInt(value);
            }
            else if (name == "downcolor") {
                DownColor = FCStr.convertStrToColor(value);
            }
            else if (name == "fieldname") {
                FieldName = FCStr.convertStrToInt(value);
            }
            else if (name == "fieldname2") {
                FieldName2 = FCStr.convertStrToInt(value);
            }
            else if (name == "fieldtext") {
                FieldText = value;
            }
            else if (name == "fieldtext2") {
                FieldText2 = value;
            }
            else if (name == "linewidth") {
                LineWidth = FCStr.convertStrToFloat(value);
            }
            else if (name == "style") {
                value = value.ToLower();
                if (value == "line") {
                    Style = BarStyle.Line;
                }
                else {
                    Style = BarStyle.Rect;
                }
            }
            else if (name == "stylefield") {
                StyleField = FCStr.convertStrToInt(value);
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
