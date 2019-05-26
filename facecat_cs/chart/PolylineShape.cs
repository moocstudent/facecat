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

namespace FaceCat {
    /// <summary>
    /// 曲线
    /// </summary>
    [Serializable()]
    public class PolylineShape : BaseShape {
        /// <summary>
        /// 创建曲线
        /// </summary>
        public PolylineShape() {
            ZOrder = 2;
        }

        protected long m_color = FCColor.argb(255, 255, 0);

        /// <summary>
        /// 获取或设置线的颜色
        /// </summary>
        public virtual long Color {
            get { return m_color; }
            set { m_color = value; }
        }

        protected int m_colorField = FCDataTable.NULLFIELD;

        /// <summary>
        /// 获取或设置颜色的字段
        /// </summary>
        public virtual int ColorField {
            get { return m_colorField; }
            set { m_colorField = value; }
        }

        protected int m_fieldName = FCDataTable.NULLFIELD;

        /// <summary>
        /// 获取或设置字段名称
        /// </summary>
        public virtual int FieldName {
            get { return m_fieldName; }
            set { m_fieldName = value; }
        }

        protected String m_fieldText = String.Empty;

        /// <summary>
        /// 获取或设置显示的标题名称
        /// </summary>
        public virtual String FieldText {
            get { return m_fieldText; }
            set { m_fieldText = value; }
        }

        protected long m_fillColor = FCColor.None;

        /// <summary>
        /// 获取或设置填充色
        /// </summary>
        public virtual long FillColor {
            get { return m_fillColor; }
            set { m_fillColor = value; }
        }

        protected PolylineStyle m_style = PolylineStyle.SolidLine;

        /// <summary>
        /// 获取或设置样式
        /// </summary>
        public virtual PolylineStyle Style {
            get { return m_style; }
            set { m_style = value; }
        }

        protected float m_width = 1;

        /// <summary>
        /// 获取或设置线的宽度
        /// </summary>
        public virtual float Width {
            get { return m_width; }
            set { m_width = value; }
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
            else {
                return null;
            }
        }

        /// <summary>
        /// 获取所有字段
        /// </summary>
        /// <returns></returns>
        public override int[] getFields() {
            return new int[] { m_fieldName };
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">返回属性值</param>
        /// <param name="type">返回属性类型</param>
        public override void getProperty(String name, ref String value, ref String type) {
            if (name == "color") {
                type = "color";
                value = FCStr.convertColorToStr(Color);
            }
            else if (name == "colorfield") {
                type = "int";
                value = FCStr.convertIntToStr(ColorField);
            }
            else if (name == "fieldname") {
                type = "int";
                value = FCStr.convertIntToStr(FieldName);
            }
            else if (name == "fieldtext") {
                type = "String";
                value = FieldText;
            }
            else if (name == "fillcolor") {
                type = "color";
                value = FCStr.convertColorToStr(FillColor);
            }
            else if (name == "style") {
                type = "enum:PolylineStyle";
                PolylineStyle style = Style;
                if (style == PolylineStyle.Cycle) {
                    value = "Cycle";
                }
                else if (style == PolylineStyle.DashLine) {
                    value = "DashLine";
                }
                else if (style == PolylineStyle.DotLine) {
                    value = "DotLine";
                }
                else {
                    value = "SolidLine";
                }
            }
            else if (name == "width") {
                type = "float";
                value = FCStr.convertFloatToStr(Width);
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
            propertyNames.AddRange(new String[] { "Color", "ColorField", "FieldName", "FieldText",
            "FillColor", "Style", "Width"});
            return propertyNames;
        }

        /// <summary>
        /// 获取选中点的颜色
        /// </summary>
        /// <returns>颜色</returns>
        public override long getSelectedColor() {
            return m_color;
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        public override void setProperty(String name, String value) {
            if (name == "color") {
                Color = FCStr.convertStrToColor(value);
            }
            else if (name == "colorfield") {
                ColorField = FCStr.convertStrToInt(value);
            }
            else if (name == "fieldname") {
                FieldName = FCStr.convertStrToInt(value);
            }
            else if (name == "fieldtext") {
                FieldText = value;
            }
            else if (name == "fillcolor") {
                FillColor = FCStr.convertStrToColor(value);
            }
            else if (name == "style") {
                value = value.ToLower();
                if (value == "cyle") {
                    Style = PolylineStyle.Cycle;
                }
                else if (value == "dashline") {
                    Style = PolylineStyle.DashLine;
                }
                else if (value == "dotline") {
                    Style = PolylineStyle.DotLine;
                }
                else {
                    Style = PolylineStyle.SolidLine;
                }
            }
            else if (name == "width") {
                Width = FCStr.convertStrToFloat(value);
            }
            else {
                base.setProperty(name, value);
            }
        }
    }
}
