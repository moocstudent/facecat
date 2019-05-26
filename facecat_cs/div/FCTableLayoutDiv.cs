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
    /// 调整大小的类型
    /// </summary>
    public enum FCSizeType {
        /// <summary>
        /// 绝对大小
        /// </summary>
        AbsoluteSize,
        /// <summary>
        /// 自动填充
        /// </summary>
        AutoFill,
        /// <summary>
        /// 百分比大小
        /// </summary>
        PercentSize
    }

    /// <summary>
    /// 列的样式
    /// </summary>
    public class FCColumnStyle : FCProperty {
        /// <summary>
        /// 创建列的样式
        /// </summary>
        /// <param name="sizeType">调整大小的类型</param>
        /// <param name="width">宽度</param>
        public FCColumnStyle(FCSizeType sizeType, float width) {
            m_sizeType = sizeType;
            m_width = width;
        }

        protected FCSizeType m_sizeType = FCSizeType.AbsoluteSize;

        /// <summary>
        /// 获取或设置调整大小的类型
        /// </summary>
        public virtual FCSizeType SizeType {
            get { return m_sizeType; }
            set { m_sizeType = value; }
        }

        protected float m_width;

        /// <summary>
        /// 获取或设置宽度
        /// </summary>
        public virtual float Width {
            get { return m_width; }
            set { m_width = value; }
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">返回属性值</param>
        /// <param name="type">返回属性类型</param>
        public virtual void getProperty(String name, ref String value, ref String type) {
            if (name == "sizetype") {
                type = "enum:FCSizeType";
                if (m_sizeType == FCSizeType.AbsoluteSize) {
                    value = "absolutesize";
                }
                else if (m_sizeType == FCSizeType.AutoFill) {
                    value = "autofill";
                }
                else if (m_sizeType == FCSizeType.PercentSize) {
                    value = "percentsize";
                }
            }
            else if (name == "width") {
                type = "float";
                value = FCStr.convertFloatToStr(Width);
            }
        }

        /// <summary>
        /// 获取属性名称列表
        /// </summary>
        /// <returns>属性名称列表</returns>
        public virtual ArrayList<String> getPropertyNames() {
            ArrayList<String> propertyNames = new ArrayList<String>();
            propertyNames.add("SizeType");
            propertyNames.add("Width");
            return propertyNames;
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        public virtual void setProperty(String name, String value) {
            if (name == "sizetype") {
                String lowerStr = value.ToLower();
                if (value == "absolutesize") {
                    m_sizeType = FCSizeType.AbsoluteSize;
                }
                else if (value == "autofill") {
                    m_sizeType = FCSizeType.AutoFill;
                }
                else if (value == "percentsize") {
                    m_sizeType = FCSizeType.PercentSize;
                }
            }
            else if (name == "width") {
                Width = FCStr.convertStrToFloat(value);
            }
        }
    }

    /// <summary>
    /// 行的样式
    /// </summary>
    public class FCRowStyle {
        /// <summary>
        /// 创建行的样式
        /// </summary>
        /// <param name="sizeType">调整大小的类型</param>
        /// <param name="height">高度</param>
        public FCRowStyle(FCSizeType sizeType, float height) {
            m_sizeType = sizeType;
            m_height = height;
        }

        protected float m_height;

        /// <summary>
        /// 获取或设置宽度
        /// </summary>
        public virtual float Height {
            get { return m_height; }
            set { m_height = value; }
        }

        protected FCSizeType m_sizeType = FCSizeType.AbsoluteSize;

        /// <summary>
        /// 获取或设置调整大小的类型
        /// </summary>
        public virtual FCSizeType SizeType {
            get { return m_sizeType; }
            set { m_sizeType = value; }
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">返回属性值</param>
        /// <param name="type">返回属性类型</param>
        public virtual void getProperty(String name, ref String value, ref String type) {
            if (name == "sizetype") {
                type = "enum:FCSizeType";
                if (m_sizeType == FCSizeType.AbsoluteSize) {
                    value = "absolutesize";
                }
                else if (m_sizeType == FCSizeType.AutoFill) {
                    value = "autofill";
                }
                else if (m_sizeType == FCSizeType.PercentSize) {
                    value = "percentsize";
                }
            }
            else if (name == "height") {
                type = "float";
                value = FCStr.convertFloatToStr(Height);
            }
        }

        /// <summary>
        /// 获取属性名称列表
        /// </summary>
        /// <returns>属性名称列表</returns>
        public virtual ArrayList<String> getPropertyNames() {
            ArrayList<String> propertyNames = new ArrayList<String>();
            propertyNames.add("SizeType");
            propertyNames.add("Height");
            return propertyNames;
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        public virtual void setProperty(String name, String value) {
            if (name == "sizetype") {
                String lowerStr = value.ToLower();
                if (value == "absolutesize") {
                    m_sizeType = FCSizeType.AbsoluteSize;
                }
                else if (value == "autofill") {
                    m_sizeType = FCSizeType.AutoFill;
                }
                else if (value == "percentsize") {
                    m_sizeType = FCSizeType.PercentSize;
                }
            }
            else if (name == "height") {
                Height = FCStr.convertStrToFloat(value);
            }
        }
    }

    /// <summary>
    /// 表格布局控件
    /// </summary>
    public class FCTableLayoutDiv : FCDiv {
        /// <summary>
        /// 创建布局控件
        /// </summary>
        public FCTableLayoutDiv() {
        }

        /// <summary>
        /// 列的集合
        /// </summary>
        protected ArrayList<int> m_columns = new ArrayList<int>();

        /// <summary>
        /// 行的集合
        /// </summary>
        protected ArrayList<int> m_rows = new ArrayList<int>();

        /// <summary>
        /// 表格控件
        /// </summary>
        protected ArrayList<FCView> m_tableControls = new ArrayList<FCView>();

        protected int m_columnsCount;

        /// <summary>
        /// 获取或设置列的数量
        /// </summary>
        public virtual int ColumnsCount {
            get { return m_columnsCount; }
            set { m_columnsCount = value; }
        }

        protected ArrayList<FCColumnStyle> m_columnStyles = new ArrayList<FCColumnStyle>();

        /// <summary>
        /// 获取或设置列的样式
        /// </summary>
        public virtual ArrayList<FCColumnStyle> ColumnStyles {
            get { return m_columnStyles; }
            set { m_columnStyles = value; }
        }

        protected int m_rowsCount;

        /// <summary>
        /// 获取或设置行的数量
        /// </summary>
        public virtual int RowsCount {
            get { return m_rowsCount; }
            set { m_rowsCount = value; }
        }

        protected ArrayList<FCRowStyle> m_rowStyles = new ArrayList<FCRowStyle>();

        /// <summary>
        /// 获取或设置行的样式
        /// </summary>
        public virtual ArrayList<FCRowStyle> RowStyles {
            get { return m_rowStyles; }
            set { m_rowStyles = value; }
        }

        /// <summary>
        /// 添加控件
        /// </summary>
        /// <param name="control">控件</param>
        public override void addControl(FCView control) {
            ArrayList<FCView> controls = m_controls;
            int controlsSize = controls.size();
            base.addControl(control);
            int column = controlsSize % m_columnsCount;
            int row = controlsSize / m_columnsCount;
            m_columns.add(column);
            m_rows.add(row);
            m_tableControls.add(control);
        }

        /// <summary>
        /// 添加控件
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="column">列</param>
        /// <param name="row">行</param>
        public virtual void addControl(FCView control, int column, int row) {
            base.addControl(control);
            m_columns.add(column);
            m_rows.add(row);
            m_tableControls.add(control);
        }

        /// <summary>
        /// 销毁资源方法
        /// </summary>
        public override void delete() {
            if (!IsDeleted) {
                m_columns.clear();
                m_columnStyles.clear();
                m_rows.clear();
                m_rowStyles.clear();
                m_tableControls.clear();
            }
            base.delete();
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <returns>控件类型</returns>
        public override String getControlType() {
            return "TableLayoutDiv";
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">返回属性值</param>
        /// <param name="type">返回属性类型</param>
        public override void getProperty(String name, ref String value, ref String type) {
            if (name == "columnscount") {
                type = "int";
                value = FCStr.convertIntToStr(ColumnsCount);
            }
            else if (name == "rowscount") {
                type = "int";
                value = FCStr.convertIntToStr(RowsCount);
            }
            else {
                base.getProperty(name, ref value, ref type);
            }
        }

        /// <summary>
        /// 获取属性名称列表
        /// </summary>
        /// <returns>属性名称列表</returns>
        public override ArrayList<String> getPropertyNames() {
            ArrayList<String> propertyNames = base.getPropertyNames();
            propertyNames.add("ColumnsCount");
            propertyNames.add("RowsCount");
            return propertyNames;
        }

        /// <summary>
        /// 重置布局
        /// </summary>
        public virtual bool OnResetLayout() {
            if (Native != null) {
                if (m_columnsCount > 0 && m_rowsCount > 0 && m_columnStyles.size() > 0 && m_rowStyles.size() > 0) {
                    int width = Width, height = Height;
                    int tabControlsSize = m_tableControls.size();
                    //获取行列的宽度
                    int[] columnWidths = new int[m_columnsCount];
                    int[] rowHeights = new int[m_rowsCount];
                    //获取列的宽度
                    int allWidth = 0, allHeight = 0;
                    for (int i = 0; i < m_columnsCount; i++) {
                        FCColumnStyle columnStyle = m_columnStyles.get(i);
                        int cWidth = 0;
                        FCSizeType sizeType = columnStyle.SizeType;
                        float sWidth = columnStyle.Width;
                        if (sizeType == FCSizeType.AbsoluteSize) {
                            cWidth = (int)(sWidth);
                        }
                        else if (sizeType == FCSizeType.AutoFill) {
                            cWidth = width - allWidth;
                        }
                        else if (sizeType == FCSizeType.PercentSize) {
                            cWidth = (int)(width * sWidth);
                        }
                        columnWidths[i] = cWidth;
                        allWidth += cWidth;
                    }
                    for (int i = 0; i < m_rowsCount; i++) {
                        FCRowStyle rowStyle = m_rowStyles.get(i);
                        //获取行的高度
                        int rHeight = 0;
                        FCSizeType sizeType = rowStyle.SizeType;
                        float sHeight = rowStyle.Height;
                        if (sizeType == FCSizeType.AbsoluteSize) {
                            rHeight = (int)(sHeight);
                        }
                        else if (sizeType == FCSizeType.AutoFill) {
                            rHeight = height - allHeight;
                        }
                        else if (sizeType == FCSizeType.PercentSize) {
                            rHeight = (int)(height * sHeight);
                        }
                        rowHeights[i] = rHeight;
                        allHeight += rHeight;
                    }
                    //控制控件的大小和位置
                    for (int i = 0; i < tabControlsSize; i++) {
                        FCView control = m_tableControls.get(i);
                        int column = m_columns[i];
                        int row = m_rows[i];
                        FCPadding margin = control.Margin;
                        //获取横坐标和纵坐标
                        int cLeft = 0, cTop = 0;
                        for (int j = 0; j < column; j++) {
                            cLeft += columnWidths[j];
                        }
                        for (int j = 0; j < row; j++) {
                            cTop += rowHeights[j];
                        }
                        int cRight = cLeft + columnWidths[column] - margin.right;
                        int cBottom = cTop + rowHeights[row] - margin.bottom;
                        cLeft += margin.left;
                        cTop += margin.top;
                        if (cRight < cLeft) {
                            cRight = cLeft;
                        }
                        if (cBottom < cTop) {
                            cBottom = cTop;
                        }
                        control.Bounds = new FCRect(cLeft, cTop, cRight, cBottom);
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 移除控件
        /// </summary>
        /// <param name="control">控件</param>
        public override void removeControl(FCView control) {
            int tabControlsSize = m_tableControls.size();
            int index = -1;
            for (int i = 0; i < tabControlsSize; i++) {
                if (control == m_tableControls.get(i)) {
                    index = i;
                    break;
                }
            }
            if (index != -1) {
                m_columns.removeAt(index);
                m_rows.removeAt(index);
                m_tableControls.removeAt(index);
            }
            base.removeControl(control);
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        public override void setProperty(String name, String value) {
            if (name == "columnscount") {
                ColumnsCount = FCStr.convertStrToInt(value);
            }
            else if (name == "rowscount") {
                RowsCount = FCStr.convertStrToInt(value);
            }
            else {
                base.setProperty(name, value);
            }
        }

        /// <summary>
        /// 布局更新方法
        /// </summary>
        public override void update() {
            OnResetLayout();
            int controlsSize = m_controls.size();
            for (int i = 0; i < controlsSize; i++) {
                m_controls.get(i).update();
            }
            updateScrollBar();
        }
    }
}
