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
    /// 布尔型单元格
    /// </summary>
    public class FCGridBoolCell : FCGridCell {
        /// <summary>
        /// 创建布尔型单元格
        /// </summary>
        public FCGridBoolCell() {
        }

        /// <summary>
        /// 创建布尔型单元格
        /// </summary>
        /// <param name="value">数值</param>
        public FCGridBoolCell(bool value) {
            m_value = value;
        }

        /// <summary>
        /// 数值
        /// </summary>
        protected bool m_value;

        /// <summary>
        /// 单元格大小比较，用于排序
        /// </summary>
        /// <param name="cell">比较单元格</param>
        /// <returns>1:较大 0:相等 -1:较小</returns>
        public override int compareTo(FCGridCell cell) {
            bool value = getBool();
            bool target = cell.getBool();
            if (value && !target) {
                return 1;
            }
            else if (!value && target) {
                return -1;
            }
            else {
                return 0;
            }
        }

        /// <summary>
        /// 获取布尔型数值
        /// </summary>
        /// <returns>布尔型数值</returns>
        public override bool getBool() {
            return m_value;
        }

        /// <summary>
        /// 获取双精度浮点值
        /// </summary>
        /// <returns>双精度浮点值</returns>
        public override double getDouble() {
            return m_value ? 1 : 0;
        }

        /// <summary>
        /// 获取单精度浮点值
        /// </summary>
        /// <returns>单精度浮点值</returns>
        public override float getFloat() {
            return m_value ? 1 : 0;
        }

        /// <summary>
        /// 获取整型数值
        /// </summary>
        /// <returns>整型数值</returns>
        public override int getInt() {
            return m_value ? 1 : 0;
        }

        /// <summary>
        /// 获取长整型数值
        /// </summary>
        /// <returns>长整型数值</returns>
        public override long getLong() {
            return m_value ? 1 : 0;
        }

        /// <summary>
        /// 获取字符型数值
        /// </summary>
        /// <returns>字符型数值</returns>
        public override String getString() {
            return m_value ? "true" : "false";
        }

        /// <summary>
        /// 设置布尔型数值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setBool(bool value) {
            m_value = value;
        }

        /// <summary>
        /// 设置双精度浮点值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setDouble(double value) {
            m_value = value > 0 ? true : false;
        }

        /// <summary>
        /// 设置单精度浮点值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setFloat(float value) {
            m_value = value > 0 ? true : false;
        }

        /// <summary>
        /// 设置整型数值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setInt(int value) {
            m_value = value > 0 ? true : false;
        }

        /// <summary>
        /// 设置长整型数值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setLong(long value) {
            m_value = value > 0 ? true : false;
        }

        /// <summary>
        /// 设置字符型数值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setString(String value) {
            m_value = value == "true" ? true : false;
        }
    }

    /// <summary>
    /// 按钮控件单元格
    /// </summary>
    public class FCGridButtonCell : FCGridControlCell {
        /// <summary>
        /// 创建按钮控件单元格
        /// </summary>
        public FCGridButtonCell() {
            FCButton button = new FCButton();
            button.BorderColor = FCColor.None;
            button.DisplayOffset = false;
            Control = button;
        }

        /// <summary>
        /// 获取按钮
        /// </summary>
        public FCButton Button {
            get {
                if (Control != null) {
                    return Control as FCButton;
                }
                else {
                    return null;
                }
            }
        }
    }

    /// <summary>
    /// 复选框控件单元格
    /// </summary>
    public class FCGridCheckBoxCell : FCGridControlCell {
        /// <summary>
        /// 创建复选框控件单元格
        /// </summary>
        public FCGridCheckBoxCell() {
            FCCheckBox checkBox = new FCCheckBox();
            checkBox.DisplayOffset = false;
            checkBox.ButtonAlign = FCHorizontalAlign.Center;
            Control = checkBox;
        }

        /// <summary>
        /// 获取复选框控件
        /// </summary>
        public FCCheckBox CheckBox {
            get {
                if (Control != null) {
                    return Control as FCCheckBox;
                }
                else {
                    return null;
                }
            }
        }

        /// <summary>
        /// 获取布尔型数值
        /// </summary>
        /// <returns>布尔型数值</returns>
        public override bool getBool() {
            FCCheckBox checkBox = CheckBox;
            if (checkBox != null) {
                return checkBox.Checked;
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// 获取双精度浮点值
        /// </summary>
        /// <returns>双精度浮点值</returns>
        public override double getDouble() {
            FCCheckBox checkBox = CheckBox;
            if (checkBox != null) {
                return checkBox.Checked ? 1 : 0;
            }
            else {
                return 0;
            }
        }

        /// <summary>
        /// 获取单精度浮点值
        /// </summary>
        /// <returns>单精度浮点值</returns>
        public override float getFloat() {
            FCCheckBox checkBox = CheckBox;
            if (checkBox != null) {
                return checkBox.Checked ? 1 : 0;
            }
            else {
                return 0;
            }
        }

        /// <summary>
        /// 获取整型数值
        /// </summary>
        /// <returns>整型数值</returns>
        public override int getInt() {
            FCCheckBox checkBox = CheckBox;
            if (checkBox != null) {
                return checkBox.Checked ? 1 : 0;
            }
            else {
                return 0;
            }
        }

        /// <summary>
        /// 获取长整型数值
        /// </summary>
        /// <returns>长整型数值</returns>
        public override long getLong() {
            FCCheckBox checkBox = CheckBox;
            if (checkBox != null) {
                return checkBox.Checked ? 1 : 0;
            }
            else {
                return 0;
            }
        }

        /// <summary>
        /// 获取布尔型数值
        /// </summary>
        /// <returns></returns>
        public override String getString() {
            FCCheckBox checkBox = CheckBox;
            if (checkBox != null) {
                return checkBox.Checked ? "true" : "false";
            }
            else {
                return "false";
            }
        }

        /// <summary>
        /// 设置布尔型数值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setBool(bool value) {
            FCCheckBox checkBox = CheckBox;
            if (checkBox != null) {
                checkBox.Checked = value;
            }
        }

        /// <summary>
        /// 设置双精度浮点值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setDouble(double value) {
            FCCheckBox checkBox = CheckBox;
            if (checkBox != null) {
                checkBox.Checked = value > 0 ? true : false;
            }
        }

        /// <summary>
        /// 设置单精度浮点值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setFloat(float value) {
            FCCheckBox checkBox = CheckBox;
            if (checkBox != null) {
                checkBox.Checked = value > 0 ? true : false;
            }
        }

        /// <summary>
        /// 设置整型数值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setInt(int value) {
            FCCheckBox checkBox = CheckBox;
            if (checkBox != null) {
                checkBox.Checked = value > 0 ? true : false;
            }
        }

        /// <summary>
        /// 设置长整型数值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setLong(long value) {
            FCCheckBox checkBox = CheckBox;
            if (checkBox != null) {
                checkBox.Checked = value > 0 ? true : false;
            }
        }

        /// <summary>
        /// 设置字符型数值
        /// </summary>
        /// <param name="value"></param>
        public override void setString(String value) {
            FCCheckBox checkBox = CheckBox;
            if (checkBox != null) {
                checkBox.Checked = value == "true";
            }
        }
    }

    /// <summary>
    /// 按钮控件单元格
    /// </summary>
    public class FCGridComboBoxCell : FCGridControlCell {
        /// <summary>
        /// 创建按钮控件单元格
        /// </summary>
        public FCGridComboBoxCell() {
            FCComboBox comboBox = new FCComboBox();
            comboBox.BorderColor = FCColor.None;
            comboBox.DisplayOffset = false;
            Control = comboBox;
        }

        /// <summary>
        /// 获取按钮控件
        /// </summary>
        public FCComboBox ComboBox {
            get {
                if (Control != null) {
                    return Control as FCComboBox;
                }
                else {
                    return null;
                }
            }
        }

        /// <summary>
        /// 获取布尔型数值
        /// </summary>
        /// <returns>布尔型数值</returns>
        public override bool getBool() {
            FCComboBox comboBox = ComboBox;
            if (comboBox != null) {
                return comboBox.SelectedIndex > 0;
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// 获取双精度浮点值
        /// </summary>
        /// <returns>双精度浮点值</returns>
        public override double getDouble() {
            FCComboBox comboBox = ComboBox;
            if (comboBox != null) {
                return (double)comboBox.SelectedIndex;
            }
            else {
                return 0;
            }
        }

        /// <summary>
        /// 获取单精度浮点值
        /// </summary>
        /// <returns>单精度浮点值</returns>
        public override float getFloat() {
            FCComboBox comboBox = ComboBox;
            if (comboBox != null) {
                return (float)comboBox.SelectedIndex;
            }
            else {
                return 0;
            }
        }

        /// <summary>
        /// 获取整型数值
        /// </summary>
        /// <returns>整型数值</returns>
        public override int getInt() {
            FCComboBox comboBox = ComboBox;
            if (comboBox != null) {
                return comboBox.SelectedIndex;
            }
            else {
                return 0;
            }
        }

        /// <summary>
        /// 获取长整型数值
        /// </summary>
        /// <returns>长整型数值</returns>
        public override long getLong() {
            FCComboBox comboBox = ComboBox;
            if (comboBox != null) {
                return (long)comboBox.SelectedIndex;
            }
            else {
                return 0;
            }
        }

        /// <summary>
        /// 获取布尔型数值
        /// </summary>
        /// <returns>字符串</returns>
        public override String getString() {
            FCComboBox comboBox = ComboBox;
            if (comboBox != null) {
                return comboBox.SelectedValue;
            }
            else {
                return "";
            }
        }

        /// <summary>
        /// 设置布尔型数值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setBool(bool value) {
            FCComboBox comboBox = ComboBox;
            if (comboBox != null) {
                comboBox.SelectedIndex = value ? 1 : 0;
            }
        }

        /// <summary>
        /// 设置双精度浮点值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setDouble(double value) {
            FCComboBox comboBox = ComboBox;
            if (comboBox != null) {
                comboBox.SelectedIndex = (int)value;
            }
        }

        /// <summary>
        /// 设置单精度浮点值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setFloat(float value) {
            FCComboBox comboBox = ComboBox;
            if (comboBox != null) {
                comboBox.SelectedIndex = (int)value;
            }
        }

        /// <summary>
        /// 设置整型数值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setInt(int value) {
            FCComboBox comboBox = ComboBox;
            if (comboBox != null) {
                comboBox.SelectedIndex = value;
            }
        }

        /// <summary>
        /// 设置长整型数值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setLong(long value) {
            FCComboBox comboBox = ComboBox;
            if (comboBox != null) {
                comboBox.SelectedIndex = (int)value;
            }
        }

        /// <summary>
        /// 设置字符型数值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setString(String value) {
            FCComboBox comboBox = ComboBox;
            if (comboBox != null) {
                comboBox.SelectedValue = value;
            }
        }
    }

    /// <summary>
    /// 日期控件单元格
    /// </summary>
    public class FCGridDateTimePickerCell : FCGridControlCell {
        /// <summary>
        /// 创建日期控件单元格
        /// </summary>
        public FCGridDateTimePickerCell() {
            FCDateTimePicker datePicker = new FCDateTimePicker();
            datePicker.BorderColor = FCColor.None;
            datePicker.DisplayOffset = false;
            Control = datePicker;
        }

        /// <summary>
        /// 获取按钮控件
        /// </summary>
        public FCDateTimePicker DatePicker {
            get {
                if (Control != null) {
                    return Control as FCDateTimePicker;
                }
                else {
                    return null;
                }
            }
        }
    }

    /// <summary>
    /// 层控件单元格
    /// </summary>
    public class GridDivCell : FCGridControlCell {
        /// <summary>
        /// 创建层控件单元格
        /// </summary>
        public GridDivCell() {
            FCDiv div = new FCDiv();
            div.BorderColor = FCColor.None;
            div.DisplayOffset = false;
            Control = div;
        }

        /// <summary>
        /// 获取层控件
        /// </summary>
        public FCDiv Div {
            get {
                if (Control != null) {
                    return Control as FCDiv;
                }
                else {
                    return null;
                }
            }
        }
    }

    /// <summary>
    /// 双精度浮点型单元格
    /// </summary>
    public class FCGridDoubleCell : FCGridCell {
        /// <summary>
        /// 创建双精度浮点型单元格
        /// </summary>
        public FCGridDoubleCell() {
        }

        /// <summary>
        /// 创建双精度浮点型单元格
        /// </summary>
        /// <param name="value">数值</param>
        public FCGridDoubleCell(double value) {
            m_value = value;
        }

        /// <summary>
        /// 数值
        /// </summary>
        protected double m_value;

        /// <summary>
        /// 单元格大小比较，用于排序
        /// </summary>
        /// <param name="cell">比较单元格</param>
        /// <returns>1:较大 0:相等 -1:较小</returns>
        public override int compareTo(FCGridCell cell) {
            double value = getDouble();
            double target = cell.getDouble();
            if (value > target) {
                return 1;
            }
            else if (value < target) {
                return -1;
            }
            else {
                return 0;
            }
        }

        /// <summary>
        /// 获取布尔型数值
        /// </summary>
        /// <returns>布尔型数值</returns>
        public override bool getBool() {
            return m_value == 0 ? false : true;
        }

        /// <summary>
        /// 获取双精度浮点值
        /// </summary>
        /// <returns>双精度浮点值</returns>
        public override double getDouble() {
            return m_value;
        }

        /// <summary>
        /// 获取单精度浮点值
        /// </summary>
        /// <returns>单精度浮点值</returns>
        public override float getFloat() {
            return (float)m_value;
        }

        /// <summary>
        /// 获取整型数值
        /// </summary>
        /// <returns>整型数值</returns>
        public override int getInt() {
            return (int)m_value;
        }

        /// <summary>
        /// 获取长整型数值
        /// </summary>
        /// <returns>长整型数值</returns>
        public override long getLong() {
            return (long)m_value;
        }

        /// <summary>
        /// 获取字符型数值
        /// </summary>
        /// <returns>字符型数值</returns>
        public override String getString() {
            return m_value.ToString();
        }

        /// <summary>
        /// 设置布尔型数值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setBool(bool value) {
            m_value = value ? 1 : 0;
        }

        /// <summary>
        /// 设置双精度浮点值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setDouble(double value) {
            m_value = value;
        }

        /// <summary>
        /// 设置单精度浮点值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setFloat(float value) {
            m_value = value;
        }

        /// <summary>
        /// 设置整型数值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setInt(int value) {
            m_value = value;
        }

        /// <summary>
        /// 设置长整型数值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setLong(long value) {
            m_value = (double)value;
        }

        /// <summary>
        /// 设置字符型数值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setString(String value) {
            m_value = FCStr.convertStrToDouble(value);
        }
    }

    /// <summary>
    /// 单精度浮点型单元格
    /// </summary>
    public class FCGridFloatCell : FCGridCell {
        /// <summary>
        /// 创建单精度浮点型单元格
        /// </summary>
        public FCGridFloatCell() {
        }

        /// <summary>
        /// 创建单精度浮点型单元格
        /// </summary>
        /// <param name="value">数值</param>
        public FCGridFloatCell(float value) {
            m_value = value;
        }

        /// <summary>
        /// 数值
        /// </summary>
        protected float m_value;

        /// <summary>
        /// 单元格大小比较，用于排序
        /// </summary>
        /// <param name="cell">比较单元格</param>
        /// <returns>1:较大 0:相等 -1:较小</returns>
        public override int compareTo(FCGridCell cell) {
            float value = getFloat();
            float target = cell.getFloat();
            if (value > target) {
                return 1;
            }
            else if (value < target) {
                return -1;
            }
            else {
                return 0;
            }
        }

        /// <summary>
        /// 获取布尔型数值
        /// </summary>
        /// <returns>布尔型数值</returns>
        public override bool getBool() {
            return m_value == 0 ? false : true;
        }

        /// <summary>
        /// 获取双精度浮点值
        /// </summary>
        /// <returns>双精度浮点值</returns>
        public override double getDouble() {
            return m_value;
        }

        /// <summary>
        /// 获取单精度浮点值
        /// </summary>
        /// <returns>单精度浮点值</returns>
        public override float getFloat() {
            return m_value;
        }

        /// <summary>
        /// 获取整型数值
        /// </summary>
        /// <returns>整型数值</returns>
        public override int getInt() {
            return (int)m_value;
        }

        /// <summary>
        /// 获取长整型数值
        /// </summary>
        /// <returns>长整型数值</returns>
        public override long getLong() {
            return (long)m_value;
        }

        /// <summary>
        /// 获取字符型数值
        /// </summary>
        /// <returns>字符型数值</returns>
        public override String getString() {
            return m_value.ToString();
        }

        /// <summary>
        /// 设置布尔型数值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setBool(bool value) {
            m_value = value ? 1 : 0;
        }

        /// <summary>
        /// 设置双精度浮点值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setDouble(double value) {
            m_value = (float)value;
        }

        /// <summary>
        /// 设置单精度浮点值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setFloat(float value) {
            m_value = value;
        }

        /// <summary>
        /// 设置整型数值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setInt(int value) {
            m_value = value;
        }

        /// <summary>
        /// 设置长整型数值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setLong(long value) {
            m_value = (float)value;
        }

        /// <summary>
        /// 设置字符型数值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setString(String value) {
            m_value = (float)FCStr.convertStrToDouble(value);
        }
    }

    /// <summary>
    /// 整型单元格
    /// </summary>
    public class FCGridIntCell : FCGridCell {
        /// <summary>
        /// 创建整型单元格
        /// </summary>
        public FCGridIntCell() {
        }

        /// <summary>
        /// 创建整型单元格
        /// </summary>
        /// <param name="value">数值</param>
        public FCGridIntCell(int value) {
            m_value = value;
        }

        /// <summary>
        /// 数值
        /// </summary>
        protected int m_value;

        /// <summary>
        /// 单元格大小比较，用于排序
        /// </summary>
        /// <param name="cell">比较单元格</param>
        /// <returns>1:较大 0:相等 -1:较小</returns>
        public override int compareTo(FCGridCell cell) {
            int value = getInt();
            int target = cell.getInt();
            if (value > target) {
                return 1;
            }
            else if (value < target) {
                return -1;
            }
            else {
                return 0;
            }
        }

        /// <summary>
        /// 获取布尔型数值
        /// </summary>
        /// <returns>布尔型数值</returns>
        public override bool getBool() {
            return m_value == 0 ? false : true;
        }

        /// <summary>
        /// 获取双精度浮点值
        /// </summary>
        /// <returns>双精度浮点值</returns>
        public override double getDouble() {
            return m_value;
        }

        /// <summary>
        /// 获取单精度浮点值
        /// </summary>
        /// <returns>单精度浮点值</returns>
        public override float getFloat() {
            return m_value;
        }

        /// <summary>
        /// 获取整型数值
        /// </summary>
        /// <returns>整型数值</returns>
        public override int getInt() {
            return m_value;
        }

        /// <summary>
        /// 获取长整型数值
        /// </summary>
        /// <returns>长整型数值</returns>
        public override long getLong() {
            return m_value;
        }

        /// <summary>
        /// 获取字符型数值
        /// </summary>
        /// <returns>字符型数值</returns>
        public override String getString() {
            return m_value.ToString();
        }

        /// <summary>
        /// 设置布尔型数值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setBool(bool value) {
            m_value = value ? 1 : 0;
        }

        /// <summary>
        /// 设置双精度浮点值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setDouble(double value) {
            m_value = (int)value;
        }

        /// <summary>
        /// 设置单精度浮点值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setFloat(float value) {
            m_value = (int)value;
        }

        /// <summary>
        /// 设置整型数值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setInt(int value) {
            m_value = value;
        }

        /// <summary>
        /// 设置长整型数值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setLong(long value) {
            m_value = (int)value;
        }

        /// <summary>
        /// 设置字符型数值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setString(String value) {
            m_value = FCStr.convertStrToInt(value);
        }
    }

    /// <summary>
    /// 标签型单元格
    /// </summary>
    public class FCGridLabelCell : FCGridControlCell {
        /// <summary>
        /// 创建标签单元格
        /// </summary>
        public FCGridLabelCell() {
            FCLabel label = new FCLabel();
            label.BorderColor = FCColor.None;
            label.DisplayOffset = false;
            Control = label;
        }

        /// <summary>
        /// 获取标签控件
        /// </summary>
        public FCLabel Label {
            get {
                if (Control != null) {
                    return Control as FCLabel;
                }
                else {
                    return null;
                }
            }
        }
    }

    /// <summary>
    /// 长整型单元格
    /// </summary>
    public class FCGridLongCell : FCGridCell {
        /// <summary>
        /// 创建长整型单元格
        /// </summary>
        public FCGridLongCell() {
        }

        /// <summary>
        /// 创建长整型单元格
        /// </summary>
        /// <param name="value">数值</param>
        public FCGridLongCell(long value) {
            m_value = value;
        }

        /// <summary>
        /// 数值
        /// </summary>
        protected long m_value;

        /// <summary>
        /// 单元格大小比较，用于排序
        /// </summary>
        /// <param name="cell">比较单元格</param>
        /// <returns>1:较大 0:相等 -1:较小</returns>
        public override int compareTo(FCGridCell cell) {
            long value = getLong();
            long target = cell.getLong();
            if (value > target) {
                return 1;
            }
            else if (value < target) {
                return -1;
            }
            else {
                return 0;
            }
        }

        /// <summary>
        /// 获取布尔型数值
        /// </summary>
        /// <returns>布尔型数值</returns>
        public override bool getBool() {
            return m_value == 0 ? false : true;
        }

        /// <summary>
        /// 获取双精度浮点值
        /// </summary>
        /// <returns>双精度浮点值</returns>
        public override double getDouble() {
            return m_value;
        }

        /// <summary>
        /// 获取单精度浮点值
        /// </summary>
        /// <returns>单精度浮点值</returns>
        public override float getFloat() {
            return m_value;
        }

        /// <summary>
        /// 获取整型数值
        /// </summary>
        /// <returns>整型数值</returns>
        public override int getInt() {
            return (int)m_value;
        }

        /// <summary>
        /// 获取长整型数值
        /// </summary>
        /// <returns>长整型数值</returns>
        public override long getLong() {
            return m_value;
        }

        /// <summary>
        /// 获取字符型数值
        /// </summary>
        /// <returns>字符型数值</returns>
        public override String getString() {
            return m_value.ToString();
        }


        /// <summary>
        /// 设置布尔型数值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setBool(bool value) {
            m_value = value ? 1 : 0;
        }

        /// <summary>
        /// 设置双精度浮点值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setDouble(double value) {
            m_value = (long)value;
        }

        /// <summary>
        /// 设置单精度浮点值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setFloat(float value) {
            m_value = (long)value;
        }

        /// <summary>
        /// 设置整型数值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setInt(int value) {
            m_value = value;
        }

        /// <summary>
        /// 设置长整型数值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setLong(long value) {
            m_value = value;
        }

        /// <summary>
        /// 设置字符型数值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setString(String value) {
            m_value = (long)FCStr.convertStrToInt(value);
        }
    }

    /// <summary>
    /// 密码型单元格
    /// </summary>
    public class FCGridPasswordCell : FCGridStringCell {
        /// <summary>
        /// 获取要绘制的文本
        /// </summary>
        /// <returns>文本</returns>
        public override string getPaintText() {
            return "******";
        }
    }

    /// <summary>
    /// 单选按钮单元格
    /// </summary>
    public class GridRadioButtonCell : FCGridControlCell {
        /// <summary>
        /// 创建单选按钮单元格
        /// </summary>
        public GridRadioButtonCell() {
            FCRadioButton radioButton = new FCRadioButton();
            radioButton.BorderColor = FCColor.None;
            radioButton.DisplayOffset = false;
            Control = radioButton;
        }

        /// <summary>
        /// 获取按钮控件
        /// </summary>
        public FCRadioButton RadioButton {
            get {
                if (Control != null) {
                    return Control as FCRadioButton;
                }
                else {
                    return null;
                }
            }
        }

        /// <summary>
        /// 获取布尔型数值
        /// </summary>
        /// <returns>布尔型数值</returns>
        public override bool getBool() {
            FCRadioButton radioButton = RadioButton;
            if (radioButton != null) {
                return radioButton.Checked;
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// 获取双精度浮点值
        /// </summary>
        /// <returns>双精度浮点值</returns>
        public override double getDouble() {
            FCRadioButton radioButton = RadioButton;
            if (radioButton != null) {
                return radioButton.Checked ? 1 : 0;
            }
            else {
                return 0;
            }
        }

        /// <summary>
        /// 获取单精度浮点值
        /// </summary>
        /// <returns>单精度浮点值</returns>
        public override float getFloat() {
            FCRadioButton radioButton = RadioButton;
            if (radioButton != null) {
                return radioButton.Checked ? 1 : 0;
            }
            else {
                return 0;
            }
        }

        /// <summary>
        /// 获取整型数值
        /// </summary>
        /// <returns>整型数值</returns>
        public override int getInt() {
            FCRadioButton radioButton = RadioButton;
            if (radioButton != null) {
                return radioButton.Checked ? 1 : 0;
            }
            else {
                return 0;
            }
        }

        /// <summary>
        /// 获取长整型数值
        /// </summary>
        /// <returns>长整型数值</returns>
        public override long getLong() {
            FCRadioButton radioButton = RadioButton;
            if (radioButton != null) {
                return radioButton.Checked ? 1 : 0;
            }
            else {
                return 0;
            }
        }

        /// <summary>
        /// 获取布尔型数值
        /// </summary>
        /// <returns></returns>
        public override String getString() {
            FCRadioButton radioButton = RadioButton;
            if (radioButton != null) {
                return radioButton.Checked ? "true" : "false";
            }
            else {
                return "false";
            }
        }

        /// <summary>
        /// 设置布尔型数值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setBool(bool value) {
            FCRadioButton radioButton = RadioButton;
            if (radioButton != null) {
                radioButton.Checked = value;
            }
        }

        /// <summary>
        /// 设置双精度浮点值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setDouble(double value) {
            FCRadioButton radioButton = RadioButton;
            if (radioButton != null) {
                radioButton.Checked = value > 0 ? true : false;
            }
        }

        /// <summary>
        /// 设置单精度浮点值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setFloat(float value) {
            FCRadioButton radioButton = RadioButton;
            if (radioButton != null) {
                radioButton.Checked = value > 0 ? true : false;
            }
        }

        /// <summary>
        /// 设置整型数值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setInt(int value) {
            FCRadioButton radioButton = RadioButton;
            if (radioButton != null) {
                radioButton.Checked = value > 0 ? true : false;
            }
        }

        /// <summary>
        /// 设置长整型数值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setLong(long value) {
            FCRadioButton radioButton = RadioButton;
            if (radioButton != null) {
                radioButton.Checked = value > 0 ? true : false;
            }
        }

        /// <summary>
        /// 设置字符型数值
        /// </summary>
        /// <param name="value"></param>
        public override void setString(String value) {
            FCRadioButton radioButton = RadioButton;
            if (radioButton != null) {
                radioButton.Checked = value == "true";
            }
        }
    }

    /// <summary>
    /// 数值框单元格
    /// </summary>
    public class FCGridSpinCell : FCGridControlCell {
        /// <summary>
        /// 创建输入框单元格
        /// </summary>
        public FCGridSpinCell() {
            FCSpin spin = new FCSpin();
            spin.BorderColor = FCColor.None;
            spin.DisplayOffset = false;
            Control = spin;
        }

        /// <summary>
        /// 获取输入框控件
        /// </summary>
        public FCSpin Spin {
            get {
                if (Control != null) {
                    return Control as FCSpin;
                }
                else {
                    return null;
                }
            }
        }

        /// <summary>
        /// 获取布尔型数值
        /// </summary>
        /// <returns>布尔型数值</returns>
        public override bool getBool() {
            FCSpin spin = Spin;
            if (spin != null) {
                return spin.Value > 0;
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// 获取双精度浮点值
        /// </summary>
        /// <returns>双精度浮点值</returns>
        public override double getDouble() {
            FCSpin spin = Spin;
            if (spin != null) {
                return spin.Value;
            }
            else {
                return 0;
            }
        }

        /// <summary>
        /// 获取单精度浮点值
        /// </summary>
        /// <returns>单精度浮点值</returns>
        public override float getFloat() {
            FCSpin spin = Spin;
            if (spin != null) {
                return (float)spin.Value;
            }
            else {
                return 0;
            }
        }

        /// <summary>
        /// 获取整型数值
        /// </summary>
        /// <returns>整型数值</returns>
        public override int getInt() {
            FCSpin spin = Spin;
            if (spin != null) {
                return (int)spin.Value;
            }
            else {
                return 0;
            }
        }

        /// <summary>
        /// 获取长整型数值
        /// </summary>
        /// <returns>长整型数值</returns>
        public override long getLong() {
            FCSpin spin = Spin;
            if (spin != null) {
                return (long)spin.Value;
            }
            else {
                return 0;
            }
        }

        /// <summary>
        /// 设置布尔型数值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setBool(bool value) {
            FCSpin spin = Spin;
            if (spin != null) {
                spin.Value = value ? 1 : 0;
            }
        }

        /// <summary>
        /// 设置双精度浮点值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setDouble(double value) {
            FCSpin spin = Spin;
            if (spin != null) {
                spin.Value = value;
            }
        }

        /// <summary>
        /// 设置单精度浮点值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setFloat(float value) {
            FCSpin spin = Spin;
            if (spin != null) {
                spin.Value = (double)value;
            }
        }

        /// <summary>
        /// 设置整型数值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setInt(int value) {
            FCSpin spin = Spin;
            if (spin != null) {
                spin.Value = (double)value;
            }
        }

        /// <summary>
        /// 设置长整型数值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setLong(long value) {
            FCSpin spin = Spin;
            if (spin != null) {
                spin.Value = (double)value;
            }
        }
    }

    /// <summary>
    /// 字符型单元格
    /// </summary>
    public class FCGridStringCell : FCGridCell {
        /// <summary>
        /// 创建字符型单元格
        /// </summary>
        public FCGridStringCell() {
        }

        /// <summary>
        /// 创建字符型单元格
        /// </summary>
        /// <param name="value">数值</param>
        public FCGridStringCell(String value) {
            m_value = value;
        }

        /// <summary>
        /// 字符
        /// </summary>
        protected String m_value;

        /// <summary>
        /// 单元格大小比较，用于排序
        /// </summary>
        /// <param name="cell">比较单元格</param>
        /// <returns>1:较大 0:相等 -1:较小</returns>
        public override int compareTo(FCGridCell cell) {
            String target = cell.getString();
            String value = getString();
            if (value != null) {
                return value.CompareTo(target);
            }
            return -1;
        }

        /// <summary>
        /// 获取字符型数值
        /// </summary>
        /// <returns>字符型数值</returns>
        public override String getString() {
            return m_value;
        }

        /// <summary>
        /// 设置字符型数值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setString(String value) {
            m_value = value;
        }
    }

    /// <summary>
    /// 输入框单元格
    /// </summary>
    public class FCGridTextBoxCell : FCGridControlCell {
        /// <summary>
        /// 创建输入框单元格
        /// </summary>
        public FCGridTextBoxCell() {
            FCTextBox textBox = new FCTextBox();
            textBox.BorderColor = FCColor.None;
            textBox.DisplayOffset = false;
            Control = textBox;
        }

        /// <summary>
        /// 获取按钮控件
        /// </summary>
        public FCTextBox TextBox {
            get {
                if (Control != null) {
                    return Control as FCTextBox;
                }
                else {
                    return null;
                }
            }
        }
    }
}
