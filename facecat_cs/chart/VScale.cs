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
    /// Y轴
    /// </summary>
    [Serializable()]
    public class VScale : FCProperty {
        /// <summary>
        /// 析构函数
        /// </summary>
        ~VScale() {
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

        protected bool m_autoMaxMin = true;

        /// <summary>
        /// 获取或设置最大值和最小值是否自动计算
        /// </summary>
        public virtual bool AutoMaxMin {
            get { return m_autoMaxMin; }
            set { m_autoMaxMin = value; }
        }

        protected int m_baseField = FCDataTable.NULLFIELD;

        /// <summary>
        /// 获取或设置基础字段
        /// </summary>
        public virtual int BaseField {
            get { return m_baseField; }
            set { m_baseField = value; }
        }

        protected CrossLineTip m_crossLineTip = new CrossLineTip();

        /// <summary>
        /// 获取或设置十字线标签
        /// </summary>
        public virtual CrossLineTip CrossLineTip {
            get { return m_crossLineTip; }
            set { m_crossLineTip = value; }
        }

        protected int m_digit = 2;

        /// <summary>
        /// 获取或设置面板显示数值保留小数的位数
        /// </summary>
        public virtual int Digit {
            get { return m_digit; }
            set { m_digit = value; }
        }

        protected FCFont m_font = new FCFont("Arial", 14, true, false, false);

        /// <summary>
        /// 获取或设置左侧Y轴文字的字体
        /// </summary>
        public virtual FCFont Font {
            get { return m_font; }
            set { m_font = value; }
        }

        protected bool m_isDeleted;

        /// <summary>
        /// 获取或设置是否已被销毁
        /// </summary>
        public virtual bool IsDeleted {
            get { return m_isDeleted; }
        }

        protected int m_magnitude = 1;

        /// <summary>
        /// 获取或设置量级
        /// </summary>
        public virtual int Magnitude {
            get { return m_magnitude; }
            set { m_magnitude = value; }
        }

        protected double m_midValue;

        /// <summary>
        /// 获取或设置区别涨贴的中间值
        /// </summary>
        public virtual double MidValue {
            get { return m_midValue; }
            set { m_midValue = value; }
        }

        protected NumberStyle m_numberStyle = NumberStyle.Standard;

        /// <summary>
        /// 获取或设置数字类型
        /// </summary>
        public virtual NumberStyle NumberStyle {
            get { return m_numberStyle; }
            set { m_numberStyle = value; }
        }

        protected int m_paddingBottom;

        /// <summary>
        /// 获取或设置最小值上方的间隙比例
        /// </summary>
        public virtual int PaddingBottom {
            get { return m_paddingBottom; }
            set { m_paddingBottom = value; }
        }

        protected int m_paddingTop;

        /// <summary>
        /// 获取或设置最大值上方的间隙比例
        /// </summary>
        public int PaddingTop {
            get { return m_paddingTop; }
            set { m_paddingTop = value; }
        }

        protected bool m_reverse;

        /// <summary>
        /// 获取或设置是否反转
        /// </summary>
        public virtual bool Reverse {
            get { return m_reverse; }
            set { m_reverse = value; }
        }

        protected long m_scaleColor = FCColor.argb(150, 0, 0);

        /// <summary>
        /// 获取或设置坐标轴的画笔
        /// </summary>
        public virtual long ScaleColor {
            get { return m_scaleColor; }
            set { m_scaleColor = value; }
        }

        protected long m_textColor = FCColor.argb(255, 82, 82);

        /// <summary>
        /// 获取或设置Y轴文字的颜色
        /// </summary>
        public virtual long TextColor {
            get { return m_textColor; }
            set { m_textColor = value; }
        }

        protected long m_textColor2 = FCColor.None;

        /// <summary>
        /// 获取或设置Y轴文字的颜色2
        /// </summary>
        public virtual long TextColor2 {
            get { return m_textColor2; }
            set { m_textColor2 = value; }
        }

        /// <summary>
        /// 刻度点
        /// </summary>
        protected ArrayList<double> m_scaleSteps = new ArrayList<double>();

        protected VScaleSystem m_system = VScaleSystem.Standard;

        /// <summary>
        /// 获取或设置坐标系的类型
        /// </summary>
        public virtual VScaleSystem System {
            get { return m_system; }
            set { m_system = value; }
        }

        protected VScaleType m_type = VScaleType.EqualDiff;

        /// <summary>
        /// 获取或设置坐标轴的类型
        /// </summary>
        public virtual VScaleType Type {
            get { return m_type; }
            set { m_type = value; }
        }

        protected double visibleMax;

        /// <summary>
        /// 获取或设置坐标值可见部分的最大值
        /// </summary>
        public virtual double VisibleMax {
            get { return visibleMax; }
            set { visibleMax = value; }
        }

        protected double m_visibleMin;

        /// <summary>
        /// 获取或设置坐标值可见部分的最小值
        /// </summary>
        public virtual double VisibleMin {
            get { return m_visibleMin; }
            set { m_visibleMin = value; }
        }

        /// <summary>
        /// 销毁资源
        /// </summary>
        public void delete() {
            if (!m_isDeleted) {
                if (m_crossLineTip != null) {
                    m_crossLineTip.delete();
                    m_crossLineTip = null;
                }
                m_scaleSteps.clear();
                m_isDeleted = true;
            }
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
            else if (name == "automaxmin") {
                type = "bool";
                value = FCStr.convertBoolToStr(AutoMaxMin);
            }
            else if (name == "basefield") {
                type = "int";
                value = FCStr.convertIntToStr(BaseField);
            }
            else if (name == "digit") {
                type = "int";
                value = FCStr.convertIntToStr(Digit);
            }
            else if (name == "font") {
                type = "font";
                value = FCStr.convertFontToStr(Font);
            }
            else if (name == "textcolor") {
                type = "color";
                value = FCStr.convertColorToStr(TextColor);
            }
            else if (name == "textcolor2") {
                type = "color";
                value = FCStr.convertColorToStr(TextColor2);
            }
            else if (name == "magnitude") {
                type = "int";
                value = FCStr.convertIntToStr(Magnitude);
            }
            else if (name == "midvalue") {
                type = "double";
                value = FCStr.convertDoubleToStr(MidValue);
            }
            else if (name == "numberstyle") {
                type = "enum:NumberStyle";
                NumberStyle style = NumberStyle;
                if (style == NumberStyle.Standard) {
                    value = "Standard";
                }
                else {
                    value = "UnderLine";
                }
            }
            else if (name == "paddingbottom") {
                type = "int";
                value = FCStr.convertIntToStr(PaddingBottom);
            }
            else if (name == "paddingtop") {
                type = "int";
                value = FCStr.convertIntToStr(PaddingTop);
            }
            else if (name == "reverse") {
                type = "bool";
                value = FCStr.convertBoolToStr(Reverse);
            }
            else if (name == "scalecolor") {
                type = "color";
                value = FCStr.convertColorToStr(ScaleColor);
            }
            else if (name == "system") {
                type = "enum:VScaleSystem";
                VScaleSystem system = System;
                if (system == VScaleSystem.Logarithmic) {
                    value = "Log";
                }
                else {
                    value = "Standard";
                }
            }
            else if (name == "type") {
                type = "enum:VScaleType";
                VScaleType vScaleType = Type;
                if (vScaleType == VScaleType.Divide) {
                    value = "Divide";
                }
                else if (vScaleType == VScaleType.EqualDiff) {
                    value = "EqualDiff";
                }
                else if (vScaleType == VScaleType.EqualRatio) {
                    value = "EqualRatio";
                }
                else if (vScaleType == VScaleType.GoldenRatio) {
                    value = "GoldenRatio";
                }
                else {
                    value = "Percent";
                }
            }
            else if (name == "visiblemax") {
                type = "double";
                value = FCStr.convertDoubleToStr(VisibleMax);
            }
            else if (name == "visiblemin") {
                type = "double";
                value = FCStr.convertDoubleToStr(VisibleMin);
            }
        }

        /// <summary>
        /// 获取属性名称列表
        /// </summary>
        /// <returns></returns>
        public virtual ArrayList<String> getPropertyNames() {
            ArrayList<String> propertyNames = new ArrayList<String>();
            propertyNames.AddRange(new String[] { "AllowUserPaint", "AutoMaxMin", "BaseField", "Digit", "Font", "Magnitude",
            "MidValue", "NumberStyle", "PaddingBottom", "PaddingTop", "Reverse", "ScaleColor", "System", "TextColor", "TextColor2", "Type", "VisibleMax", "VisibleMin"});
            return propertyNames;
        }

        /// <summary>
        /// 获取刻度点
        /// </summary>
        /// <returns>刻度点</returns>
        public ArrayList<double> getScaleSteps() {
            return m_scaleSteps;
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
            else if (name == "automaxmin") {
                AutoMaxMin = FCStr.convertStrToBool(value);
            }
            else if (name == "basefield") {
                BaseField = FCStr.convertStrToInt(value);
            }
            else if (name == "digit") {
                Digit = FCStr.convertStrToInt(value);
            }
            else if (name == "font") {
                Font = FCStr.convertStrToFont(value);
            }
            else if (name == "textcolor") {
                TextColor = FCStr.convertStrToColor(value);
            }
            else if (name == "textcolor2") {
                TextColor2 = FCStr.convertStrToColor(value);
            }
            else if (name == "magnitude") {
                Magnitude = FCStr.convertStrToInt(value);
            }
            else if (name == "midvalue") {
                MidValue = FCStr.convertStrToDouble(value);
            }
            else if (name == "numberstyle") {
                value = value.ToLower();
                if (value == "standard") {
                    NumberStyle = NumberStyle.Standard;
                }
                else {
                    NumberStyle = NumberStyle.UnderLine;
                }
            }
            else if (name == "paddingbottom") {
                PaddingBottom = FCStr.convertStrToInt(value);
            }
            else if (name == "paddingtop") {
                PaddingTop = FCStr.convertStrToInt(value);
            }
            else if (name == "reverse") {
                Reverse = FCStr.convertStrToBool(value);
            }
            else if (name == "scalecolor") {
                ScaleColor = FCStr.convertStrToColor(value);
            }
            else if (name == "system") {
                if (value == "log") {
                    System = VScaleSystem.Logarithmic;
                }
                else {
                    System = VScaleSystem.Standard;
                }
            }
            else if (name == "type") {
                if (value == "Divide") {
                    Type = VScaleType.Divide;
                }
                else if (value == "equaldiff") {
                    Type = VScaleType.EqualDiff;
                }
                else if (value == "equalratio") {
                    Type = VScaleType.EqualRatio;
                }
                else if (value == "goldenratio") {
                    Type = VScaleType.GoldenRatio;
                }
                else {
                    Type = VScaleType.Percent;
                }
            }
            else if (name == "visiblemax") {
                VisibleMax = FCStr.convertStrToDouble(value);
            }
            else if (name == "visiblemin") {
                VisibleMin = FCStr.convertStrToDouble(value);
            }
        }

        /// <summary>
        /// 设置刻度点
        /// </summary>
        /// <param name="scaleSteps">刻度点</param>
        public void setScaleSteps(ArrayList<double> scaleSteps) {
            m_scaleSteps = scaleSteps;
        }
    }
}
