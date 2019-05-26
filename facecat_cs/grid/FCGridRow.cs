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
    /// 表格行的样式
    /// </summary>
    public class FCGridRowStyle {
        protected long m_backColor = FCColor.Back;

        /// <summary>
        /// 获取
        /// </summary>
        public virtual long BackColor {
            get { return m_backColor; }
            set { m_backColor = value; }
        }

        protected FCFont m_font = new FCFont("Simsun", 14, false, false, false);

        /// <summary>
        /// 获取或设置字体
        /// </summary>
        public virtual FCFont Font {
            get { return m_font; }
            set { m_font = value; }
        }

        protected long m_hoveredBackColor = FCColor.argb(150, 150, 150);

        /// <summary>
        /// 获取或设置触摸悬停行的背景色
        /// </summary>
        public virtual long HoveredBackColor {
            get { return m_hoveredBackColor; }
            set { m_hoveredBackColor = value; }
        }

        protected long m_hoveredTextColor = FCColor.Text;

        /// <summary>
        /// 获取或设置触摸悬停行的前景色
        /// </summary>
        public virtual long HoveredTextColor {
            get { return m_hoveredTextColor; }
            set { m_hoveredTextColor = value; }
        }

        protected long m_selectedBackColor = FCColor.argb(100, 100, 100);

        /// <summary>
        /// 获取或设置选中行的背景色
        /// </summary>
        public virtual long SelectedBackColor {
            get { return m_selectedBackColor; }
            set { m_selectedBackColor = value; }
        }

        protected long m_selectedTextColor = FCColor.Text;

        /// <summary>
        /// 获取或设置选中行的前景色
        /// </summary>
        public virtual long SelectedTextColor {
            get { return m_selectedTextColor; }
            set { m_selectedTextColor = value; }
        }

        protected long m_textColor = FCColor.Text;

        /// <summary>
        /// 获取或设置前景色
        /// </summary>
        public virtual long TextColor {
            get { return m_textColor; }
            set { m_textColor = value; }
        }
    }

    /// <summary>
    /// 单元格
    /// </summary>
    public class FCGridRow : FCProperty {
        /// <summary>
        /// 创建行
        /// </summary>
        public FCGridRow() {
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~FCGridRow() {
            delete();
        }

        /// <summary>
        /// 所有单元格
        /// </summary>
        public ArrayList<FCGridCell> m_cells = new ArrayList<FCGridCell>();

        /// <summary>
        /// 编辑状态
        /// </summary>
        public int m_editState;

        protected bool m_allowEdit;

        /// <summary>
        /// 获取或设置是否允许编辑
        /// </summary>
        public virtual bool AllowEdit {
            get { return m_allowEdit; }
            set { m_allowEdit = value; }
        }

        protected FCRect m_bounds;

        /// <summary>
        /// 获取或设置显示区域
        /// </summary>
        public virtual FCRect Bounds {
            get { return m_bounds; }
            set { m_bounds = value; }
        }

        protected FCView m_editButton;

        /// <summary>
        /// 获取或设置编辑按钮
        /// </summary>
        public virtual FCView EditButton {
            get { return m_editButton; }
            set { m_editButton = value; }
        }

        protected FCGrid m_grid;

        /// <summary>
        /// 获取或设置所在表格
        /// </summary>
        public virtual FCGrid Grid {
            get { return m_grid; }
            set { m_grid = value; }
        }

        protected int height = 20;

        /// <summary>
        /// 获取或设置行高
        /// </summary>
        public virtual int Height {
            get { return height; }
            set { height = value; }
        }

        protected int m_horizontalOffset = 0;

        /// <summary>
        /// 获取或设置横向滚动距离
        /// </summary>
        public virtual int HorizontalOffset {
            get { return m_horizontalOffset; }
            set { m_horizontalOffset = value; }
        }

        protected int m_index = -1;

        /// <summary>
        /// 获取或设置索引
        /// </summary>
        public virtual int Index {
            get { return m_index; }
            set { m_index = value; }
        }

        protected bool m_isDeleted;

        /// <summary>
        /// 获取或设置是否已被销毁
        /// </summary>
        public bool IsDeleted {
            get { return m_isDeleted; }
        }

        protected object m_tag = null;

        /// <summary>
        /// 获取或设置TAG值
        /// </summary>
        public virtual object Tag {
            get { return m_tag; }
            set { m_tag = value; }
        }

        protected bool m_visible = true;

        /// <summary>
        /// 获取或设置是否可见
        /// </summary>
        public virtual bool Visible {
            get { return m_visible; }
            set { m_visible = value; }
        }

        protected int m_visibleIndex = -1;

        /// <summary>
        /// 获取或设置可见索引
        /// </summary>
        public virtual int VisibleIndex {
            get { return m_visibleIndex; }
            set { m_visibleIndex = value; }
        }

        /// <summary>
        /// 添加单元格
        /// </summary>
        /// <param name="column">列索引</param>
        /// <param name="cell">单元格</param>
        public void addCell(FCGridColumn column, FCGridCell cell) {
            cell.Grid = m_grid;
            cell.Column = column;
            cell.Row = this;
            m_cells.add(cell);
            cell.onAdd();
        }

        /// <summary>
        /// 添加单元格
        /// </summary>
        /// <param name="columnIndex">列索引</param>
        /// <param name="cell">单元格</param>
        public void addCell(int columnIndex, FCGridCell cell) {
            cell.Grid = m_grid;
            cell.Column = m_grid.getColumn(columnIndex);
            cell.Row = this;
            m_cells.add(cell);
            cell.onAdd();
        }

        /// <summary>
        /// 添加单元格
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <param name="cell">单元格</param>
        public void addCell(String columnName, FCGridCell cell) {
            cell.Grid = m_grid;
            cell.Column = m_grid.getColumn(columnName);
            cell.Row = this;
            m_cells.add(cell);
            cell.onAdd();
        }

        /// <summary>
        /// 清除单元格
        /// </summary>
        public void clearCells() {
            int cellSize = m_cells.size();
            for (int i = 0; i < cellSize; i++) {
                m_cells.get(i).onRemove();
                m_cells.get(i).delete();
            }
            m_cells.clear();
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        public void delete() {
            if (!m_isDeleted) {
                m_editButton = null;
                m_isDeleted = !m_isDeleted;
                int cellSize = m_cells.size();
                for (int i = 0; i < cellSize; i++) {
                    m_cells.get(i).delete();
                }
                m_cells.clear();
            }
        }

        /// <summary>
        /// 根据列获取单元格
        /// </summary>
        /// <param name="column">列</param>
        /// <returns>单元格</returns>
        public FCGridCell getCell(FCGridColumn column) {
            int cellSize = m_cells.size();
            for (int i = 0; i < cellSize; i++) {
                if (m_cells.get(i).Column == column) {
                    return m_cells.get(i);
                }
            }
            return null;
        }

        /// <summary>
        /// 根据列的索引获取单元格
        /// </summary>
        /// <returns>单元格</returns>
        public FCGridCell getCell(int columnIndex) {
            int cellsSize = m_cells.size();
            if (cellsSize > 0) {
                if (columnIndex >= 0 && columnIndex < cellsSize) {
                    if (m_cells.get(columnIndex).Column.Index == columnIndex) {
                        return m_cells.get(columnIndex);
                    }
                }
                for (int i = 0; i < cellsSize; i++) {
                    if (m_cells.get(i).Column.Index == columnIndex) {
                        return m_cells.get(i);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 根据列的名称获取单元格
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <returns>单元格</returns>
        public FCGridCell getCell(String columnName) {
            int cellSize = m_cells.size();
            for (int i = 0; i < cellSize; i++) {
                if (m_cells.get(i).Column.Name == columnName) {
                    return m_cells.get(i);
                }
            }
            return null;
        }

        /// <summary>
        /// 获取所有单元格
        /// </summary>
        /// <returns>所有单元格</returns>
        public ArrayList<FCGridCell> getCells() {
            return m_cells;
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">返回属性值</param>
        /// <param name="type">返回属性类型</param>
        public virtual void getProperty(String name, ref String value, ref String type) {
            if (name == "allowedit") {
                type = "bool";
                value = FCStr.convertBoolToStr(AllowEdit);
            }
            else if (name == "height") {
                type = "int";
                value = FCStr.convertIntToStr(Height);
            }
            else if (name == "visible") {
                type = "int";
                value = FCStr.convertBoolToStr(Visible);
            }
            else {
                type = "undefined";
                value = "";
            }
        }

        /// <summary>
        /// 获取属性名称列表
        /// </summary>
        /// <returns></returns>
        public virtual ArrayList<String> getPropertyNames() {
            ArrayList<String> propertyNames = new ArrayList<String>();
            propertyNames.AddRange(new String[] { "AllowEdit", "EditorWidth", "Height", "Visible" });
            return propertyNames;
        }

        /// <summary>
        /// 添加行方法
        /// </summary>
        public virtual void onAdd() {
            int cellSize = m_cells.size();
            for (int i = 0; i < cellSize; i++) {
                m_cells.get(i).onAdd();
            }
        }


        /// <summary>
        /// 重绘方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪矩形</param>
        /// <param name="isAlternate">是否交替行</param>
        public virtual void onPaint(FCPaint paint, FCRect clipRect, bool isAlternate) {

        }

        /// <summary>
        /// 重绘边线方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪矩形</param>
        /// <param name="isAlternate">是否交替行</param>
        public virtual void onPaintBorder(FCPaint paint, FCRect clipRect, bool isAlternate) {

        }

        /// <summary>
        /// 移除行方法
        /// </summary>
        public virtual void onRemove() {
            int cellSize = m_cells.size();
            for (int i = 0; i < cellSize; i++) {
                m_cells.get(i).onRemove();
            }
        }

        /// <summary>
        /// 移除单元格
        /// </summary>
        /// <param name="column">列</param>
        public void removeCell(FCGridColumn column) {
            int cellSize = m_cells.size();
            for (int i = 0; i < cellSize; i++) {
                FCGridCell cell = m_cells.get(i);
                if (cell.Column == column) {
                    m_cells.remove(cell);
                    cell.onRemove();
                    break;
                }
            }
        }

        /// <summary>
        /// 移除单元格
        /// </summary>
        /// <param name="columnIndex">列索引</param>
        public void removeCell(int columnIndex) {
            int cellSize = m_cells.size();
            if (columnIndex >= 0 && columnIndex < cellSize) {
                FCGridCell cell = m_cells.get(columnIndex);
                if (cell.Column.Index == columnIndex) {
                    m_cells.remove(cell);
                    cell.onRemove();
                    return;
                }
                for (int i = 0; i < cellSize; i++) {
                    cell = m_cells.get(i);
                    if (cell.Column.Index == columnIndex) {
                        m_cells.remove(cell);
                        cell.onRemove();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 移除单元格
        /// </summary>
        /// <param name="columnName">列名</param>
        public void removeCell(String columnName) {
            int cellSize = m_cells.size();
            for (int i = 0; i < cellSize; i++) {
                FCGridCell cell = m_cells.get(i);
                if (cell.Column.Name == columnName) {
                    m_cells.remove(cell);
                    cell.onRemove();
                    break;
                }
            }
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        public virtual void setProperty(String name, String value) {
            if (name == "allowedit") {
                AllowEdit = FCStr.convertStrToBool(value);
            }
            else if (name == "height") {
                Height = FCStr.convertStrToInt(value);
            }
            else if (name == "visible") {
                Visible = FCStr.convertStrToBool(value);
            }
        }
    }
}
