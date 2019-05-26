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
    /// 横轴
    /// </summary>
    [Serializable()]
    public class HScale : FCProperty {
        /// <summary>
        /// 创建横轴
        /// </summary>
        public HScale() {
            //设置日期的自定义颜色
            m_dateColors.put(DateType.Year, FCColor.argb(255, 255, 255));
            m_dateColors.put(DateType.Month, FCColor.argb(150, 0, 0));
            m_dateColors.put(DateType.Day, FCColor.argb(100, 100, 100));
            m_dateColors.put(DateType.Hour, FCColor.argb(82, 82, 255));
            m_dateColors.put(DateType.Minute, FCColor.argb(255, 255, 0));
            m_dateColors.put(DateType.Second, FCColor.argb(255, 0, 255));
            m_dateColors.put(DateType.Millisecond, FCColor.argb(255, 0, 255));
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~HScale() {
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

        /// <summary>
        /// 日期文字的颜色
        /// </summary>
        protected HashMap<DateType, long> m_dateColors = new HashMap<DateType, long>();

        /// <summary>
        /// 刻度点
        /// </summary>
        protected ArrayList<double> m_scaleSteps = new ArrayList<double>();

        protected CrossLineTip m_crossLineTip = new CrossLineTip();

        /// <summary>
        /// 获取或设置十字线标签
        /// </summary>
        public virtual CrossLineTip CrossLineTip {
            get { return m_crossLineTip; }
            set { m_crossLineTip = value; }
        }

        protected FCFont m_font = new FCFont();

        /// <summary>
        /// 获取或设置X轴文字的字体
        /// </summary>
        public virtual FCFont Font {
            get { return m_font; }
            set { m_font = value; }
        }

        protected long m_textColor = FCColor.argb(255, 255, 255);

        /// <summary>
        /// 获取或设置X轴文字色
        /// </summary>
        public virtual long TextColor {
            get { return m_textColor; }
            set { m_textColor = value; }
        }

        protected int m_height = 25;

        /// <summary>
        /// 获取或设置X轴的高度
        /// </summary>
        public virtual int Height {
            get { return m_height; }
            set { m_height = value; }
        }

        protected HScaleType m_hScaleType = HScaleType.Date;

        /// <summary>
        /// 获取获取设置横轴的数据类型
        /// </summary>
        public virtual HScaleType HScaleType {
            get { return m_hScaleType; }
            set { m_hScaleType = value; }
        }

        protected int m_interval = 60;

        /// <summary>
        /// 获取或设置日期文字间隔
        /// </summary>
        public virtual int Interval {
            get { return m_interval; }
            set { m_interval = value; }
        }

        protected bool m_isDeleted;

        /// <summary>
        /// 获取或设置是否已被销毁
        /// </summary>
        public virtual bool IsDeleted {
            get { return m_isDeleted; }
        }

        protected long m_scaleColor = FCColor.argb(150, 0, 0);

        /// <summary>
        /// 获取或设置X轴的线条颜色
        /// </summary>
        public virtual long ScaleColor {
            get { return m_scaleColor; }
            set { m_scaleColor = value; }
        }

        protected bool m_visible = true;

        /// <summary>
        /// 获取或设置显示X轴
        /// </summary>
        public virtual bool Visible {
            get { return m_visible; }
            set { m_visible = value; }
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
                if (m_dateColors != null) {
                    m_dateColors.clear();
                    m_dateColors = null;
                }
                m_isDeleted = true;
            }
        }

        /// <summary>
        /// 获取日期文字的颜色
        /// </summary>
        /// <param name="dateType">日期类型</param>
        /// <returns>颜色</returns>
        public long getDateColor(DateType dateType) {
            return m_dateColors.get(dateType);
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
            else if (name == "height") {
                type = "int";
                value = FCStr.convertIntToStr(Height);
            }
            else if (name == "type") {
                type = "enum:HScaleType";
                HScaleType hScaleType = HScaleType;
                if (hScaleType == HScaleType.Date) {
                    value = "Date";
                }
                else {
                    value = "Number";
                }
            }
            else if (name == "interval") {
                type = "int";
                value = FCStr.convertIntToStr(Interval);
            }
            else if (name == "scalecolor") {
                type = "color";
                value = FCStr.convertColorToStr(ScaleColor);
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
            propertyNames.AddRange(new String[] { "AllowUserPaint", "Font", "Height", "Type",
            "Interval", "ScaleColor", "TextColor", "Visible"});
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
        /// 设置日期文字的颜色
        /// </summary>
        /// <param name="dateType">日期类型</param>
        /// <param name="color">颜色</param>
        public void setDateColor(DateType dateType, long color) {
            m_dateColors.put(dateType, color);
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
            else if (name == "textcolor") {
                TextColor = FCStr.convertStrToColor(value);
            }
            else if (name == "height") {
                Height = FCStr.convertStrToInt(value);
            }
            else if (name == "type") {
                value = value.ToLower();
                if (value == "date") {
                    HScaleType = HScaleType.Date;
                }
                else {
                    HScaleType = HScaleType.Number;
                }
            }
            else if (name == "interval") {
                Interval = FCStr.convertStrToInt(value);
            }
            else if (name == "scalecolor") {
                ScaleColor = FCStr.convertStrToColor(value);
            }
            else if (name == "visible") {
                Visible = FCStr.convertStrToBool(value);
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
