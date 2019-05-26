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
    /// 单元格样式
    /// </summary>
    public class FCGridCellStyle {
        protected FCHorizontalAlign m_align = FCHorizontalAlign.Inherit;

        /// <summary>
        /// 获取或设置内容的横向排列样式
        /// </summary>
        public virtual FCHorizontalAlign Align {
            get { return m_align; }
            set { m_align = value; }
        }

        protected bool m_autoEllipsis;

        /// <summary>
        /// 获取或设置是否在文字超出范围时在结尾显示省略号
        /// </summary>
        public virtual bool AutoEllipsis {
            get { return m_autoEllipsis; }
            set { m_autoEllipsis = value; }
        }

        protected long m_backColor = FCColor.None;

        /// <summary>
        /// 获取
        /// </summary>
        public virtual long BackColor {
            get { return m_backColor; }
            set { m_backColor = value; }
        }

        protected FCFont m_font;

        /// <summary>
        /// 获取或设置字体
        /// </summary>
        public virtual FCFont Font {
            get { return m_font; }
            set { m_font = value; }
        }

        protected long m_textColor = FCColor.None;

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
    public class FCGridCell : FCProperty {
        /// <summary>
        /// 创建单元格
        /// </summary>
        public FCGridCell() {
        }


        /// <summary>
        /// 析构函数
        /// </summary>
        ~FCGridCell() {
            delete();
        }

        protected bool m_allowEdit;

        /// <summary>
        /// 获取或设置是否可编辑
        /// </summary>
        public virtual bool AllowEdit {
            get { return m_allowEdit; }
            set { m_allowEdit = value; }
        }

        protected int colSpan = 1;

        /// <summary>
        /// 获取或设置跨越的列数
        /// </summary>
        public virtual int ColSpan {
            get { return colSpan; }
            set { colSpan = value; }
        }

        protected FCGridColumn m_column;

        /// <summary>
        /// 获取或设置所在列
        /// </summary>
        public virtual FCGridColumn Column {
            get { return m_column; }
            set { m_column = value; }
        }

        protected FCGrid m_grid;

        /// <summary>
        /// 获取或设置表格
        /// </summary>
        public virtual FCGrid Grid {
            get { return m_grid; }
            set { m_grid = value; }
        }

        protected bool m_isDeleted;

        /// <summary>
        /// 获取或设置是否已被销毁
        /// </summary>
        public virtual bool IsDeleted {
            get { return m_isDeleted; }
        }

        protected String m_name;

        /// <summary>
        /// 获取或设置名称
        /// </summary>
        public virtual String Name {
            get { return m_name; }
            set { m_name = value; }
        }

        protected FCGridRow m_row;

        /// <summary>
        /// 获取或设置所在行
        /// </summary>
        public FCGridRow Row {
            get { return m_row; }
            set { m_row = value; }
        }

        protected int rowSpan = 1;

        /// <summary>
        /// 获取或设置跨越的行数
        /// </summary>
        public virtual int RowSpan {
            get { return rowSpan; }
            set { rowSpan = value; }
        }

        protected FCGridCellStyle m_style;

        /// <summary>
        /// 获取或设置样式
        /// </summary>
        public virtual FCGridCellStyle Style {
            get { return m_style; }
            set { m_style = value; }
        }

        protected object m_tag = null;

        /// <summary>
        /// 获取或设置TAG值
        /// </summary>
        public virtual object Tag {
            get { return m_tag; }
            set { m_tag = value; }
        }

        /// <summary>
        /// 获取或设置文字
        /// </summary>
        public virtual String Text {
            get { return getString(); }
            set { setString(value); }
        }

        /// <summary>
        /// 单元格大小比较，用于排序
        /// </summary>
        /// <param name="cell">比较单元格</param>
        /// <returns>1:较大 0:相等 -1:较小</returns>
        public virtual int compareTo(FCGridCell cell) {
            return 0;
        }

        /// <summary>
        /// 销毁资源
        /// </summary>
        public virtual void delete() {
            m_isDeleted = true;
        }

        /// <summary>
        /// 获取布尔型数值
        /// </summary>
        /// <returns>布尔型数值</returns>
        public virtual bool getBool() {
            return false;
        }

        /// <summary>
        /// 获取双精度浮点值
        /// </summary>
        /// <returns>双精度浮点值</returns>
        public virtual double getDouble() {
            return 0;
        }

        /// <summary>
        /// 获取单精度浮点值
        /// </summary>
        /// <returns>单精度浮点值</returns>
        public virtual float getFloat() {
            return 0;
        }

        /// <summary>
        /// 获取整型数值
        /// </summary>
        /// <returns>整型数值</returns>
        public virtual int getInt() {
            return 0;
        }

        /// <summary>
        /// 获取长整型数值
        /// </summary>
        /// <returns>长整型数值</returns>
        public virtual long getLong() {
            return 0;
        }

        /// <summary>
        /// 获取要绘制的文字
        /// </summary>
        /// <returns>要绘制的文字</returns>
        public virtual String getPaintText() {
            return Text;
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">返回属性值</param>
        /// <param name="type">返回属性类型</param>
        public virtual void getProperty(String name, ref String value, ref String type) {
            if (name == "align") {
                type = "enum:FCHorizontalAlign";
                FCGridCellStyle style = Style;
                if (style != null) {
                    value = FCStr.convertHorizontalAlignToStr(style.Align);
                }
            }
            else if (name == "allowedit") {
                type = "bool";
                value = FCStr.convertBoolToStr(AllowEdit);
            }
            else if (name == "autoellipsis") {
                type = "bool";
                FCGridCellStyle style = Style;
                if (style != null) {
                    value = FCStr.convertBoolToStr(style.AutoEllipsis);
                }
            }
            else if (name == "backcolor") {
                type = "color";
                FCGridCellStyle style = Style;
                if (style != null) {
                    value = FCStr.convertColorToStr(style.BackColor);
                }
            }
            else if (name == "colspan") {
                type = "int";
                value = FCStr.convertIntToStr(ColSpan);
            }
            else if (name == "font") {
                type = "font";
                FCGridCellStyle style = Style;
                if (style != null && style.Font != null) {
                    value = FCStr.convertFontToStr(style.Font);
                }
            }
            else if (name == "name") {
                type = "String";
                value = Name;
            }
            else if (name == "rowspan") {
                type = "int";
                value = FCStr.convertIntToStr(RowSpan);
            }
            else if (name == "text") {
                type = "String";
                value = Text;
            }
            else if (name == "textcolor") {
                type = "color";
                FCGridCellStyle style = Style;
                if (style != null) {
                    value = FCStr.convertColorToStr(style.TextColor);
                }
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
            propertyNames.AddRange(new String[] { "Align", "AllowEdit", "AutoEllipsis", "BackColor", "ColSpan", "Font", "Name", "RowSpan", "Text", "TextColor"});
            return propertyNames;
        }

        /// <summary>
        /// 获取字符型数值
        /// </summary>
        /// <returns>字符型数值</returns>
        public virtual String getString() {
            return "";
        }

        /// <summary>
        /// 添加单元格方法
        /// </summary>
        public virtual void onAdd() {
        }

        /// <summary>
        /// 重绘方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="rect">矩形</param>
        /// <param name="clipRect">裁剪矩形</param>
        /// <param name="isAlternate">是否交替行</param>
        public virtual void onPaint(FCPaint paint, FCRect rect, FCRect clipRect, bool isAlternate) {
            int clipW = clipRect.right - clipRect.left;
            int clipH = clipRect.bottom - clipRect.top;
            if (clipW > 0 && clipH > 0) {
                if (m_grid != null && m_row != null && m_column != null) {
                    //判断选中
                    String text = getPaintText();
                    bool selected = false;
                    if (m_grid.SelectionMode == FCGridSelectionMode.SelectCell) {
                        ArrayList<FCGridCell> selectedCells = m_grid.SelectedCells;
                        int selectedCellSize = selectedCells.size();
                        for (int i = 0; i < selectedCellSize; i++) {
                            if (selectedCells.get(i) == this) {
                                selected = true;
                                break;
                            }
                        }
                    }
                    else if (m_grid.SelectionMode == FCGridSelectionMode.SelectFullColumn) {
                        ArrayList<FCGridColumn> selectedColumns = m_grid.SelectedColumns;
                        int selectedColumnsSize = selectedColumns.size();
                        for (int i = 0; i < selectedColumnsSize; i++) {
                            if (selectedColumns.get(i) == m_column) {
                                selected = true;
                                break;
                            }
                        }
                    }
                    else if (m_grid.SelectionMode == FCGridSelectionMode.SelectFullRow) {
                        ArrayList<FCGridRow> selectedRows = m_grid.SelectedRows;
                        int selectedRowsSize = selectedRows.size();
                        for (int i = 0; i < selectedRowsSize; i++) {
                            if (selectedRows.get(i) == m_row) {
                                selected = true;
                                break;
                            }
                        }
                    }
                    //获取颜色
                    FCFont font = null;
                    long backColor = FCColor.None;
                    long textColor = FCColor.None;
                    bool autoEllipsis = m_grid.AutoEllipsis;
                    FCHorizontalAlign horizontalAlign = m_column.CellAlign;
                    if (m_style != null) {
                        if (m_style.AutoEllipsis) {
                            autoEllipsis = m_style.AutoEllipsis;
                        }
                        backColor = m_style.BackColor;
                        if (m_style.Font != null) {
                            font = m_style.Font;
                        }
                        textColor = m_style.TextColor;
                        if (m_style.Align != FCHorizontalAlign.Inherit) {
                            horizontalAlign = m_style.Align;
                        }
                    }
                    FCGridRowStyle rowStyle = m_grid.RowStyle;
                    if (isAlternate) {
                        FCGridRowStyle alternateRowStyle = m_grid.AlternateRowStyle;
                        if (alternateRowStyle != null) {
                            rowStyle = alternateRowStyle;
                        }
                    }
                    if (rowStyle != null) {
                        if (backColor == FCColor.None) {
                            if (selected) {
                                backColor = rowStyle.SelectedBackColor;
                            }
                            else if (m_row == m_grid.HoveredRow) {
                                backColor = rowStyle.HoveredBackColor;
                            }
                            else {
                                backColor = rowStyle.BackColor;
                            }
                        }
                        if (font == null) {
                            font = rowStyle.Font;
                        }
                        if (textColor == FCColor.None) {
                            if (selected) {
                                textColor = rowStyle.SelectedTextColor;
                            }
                            else if (m_row == m_grid.HoveredRow) {
                                textColor = rowStyle.HoveredTextColor;
                            }
                            else {
                                textColor = rowStyle.TextColor;
                            }
                        }
                    }
                    paint.fillRect(backColor, rect);
                    FCSize tSize = paint.textSize(text, font);
                    FCPoint tPoint = new FCPoint(rect.left + 1, rect.top + clipH / 2 - tSize.cy / 2);
                    int width = rect.right - rect.left;
                    if (tSize.cx < width) {
                        if (horizontalAlign == FCHorizontalAlign.Center) {
                            tPoint.x = rect.left + (rect.right - rect.left - tSize.cx) / 2;
                        }
                        else if (horizontalAlign == FCHorizontalAlign.Right) {
                            tPoint.x = rect.right - tSize.cx - 2;
                        }
                    }
                    FCRect tRect = new FCRect(tPoint.x, tPoint.y, tPoint.x + tSize.cx, tPoint.y + tSize.cy);
                    if (autoEllipsis && (tRect.right > clipRect.right || tRect.bottom > clipRect.bottom)) {
                        if (tRect.right > clipRect.right) {
                            tRect.right = clipRect.right;
                        }
                        if (tRect.bottom > clipRect.bottom) {
                            tRect.bottom = clipRect.bottom;
                        }
                        paint.drawTextAutoEllipsis(text, textColor, font, tRect);
                    }
                    else {
                        paint.drawText(text, textColor, font, tRect);
                    }
                }
            }
        }

        /// <summary>
        /// 移除单元格方法
        /// </summary>
        public virtual void onRemove() {
        }

        /// <summary>
        /// 设置布尔型数值
        /// </summary>
        /// <param name="value">数值</param>
        public virtual void setBool(bool value) {
        }

        /// <summary>
        /// 设置双精度浮点值
        /// </summary>
        /// <param name="value">数值</param>
        public virtual void setDouble(double value) {
        }

        /// <summary>
        /// 设置单精度浮点值
        /// </summary>
        /// <param name="value">数值</param>
        public virtual void setFloat(float value) {
        }

        /// <summary>
        /// 设置整型数值
        /// </summary>
        /// <param name="value">数值</param>
        public virtual void setInt(int value) {
        }

        /// <summary>
        /// 设置长整型数值
        /// </summary>
        /// <param name="value">数值</param>
        public virtual void setLong(long value) {
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        public virtual void setProperty(String name, String value) {
            if (name == "align") {
                if (m_style == null) {
                    m_style = new FCGridCellStyle();
                }
                m_style.Align = FCStr.convertStrToHorizontalAlign(value);
            }
            else if (name == "allowedit") {
                AllowEdit = FCStr.convertStrToBool(value);
            }
            else if (name == "autoellipsis") {
                if (m_style == null) {
                    m_style = new FCGridCellStyle();
                }
                m_style.AutoEllipsis = FCStr.convertStrToBool(value);
            }
            else if (name == "backcolor") {
                if (m_style == null) {
                    m_style = new FCGridCellStyle();
                }
                m_style.BackColor = FCStr.convertStrToColor(value);
            }
            else if (name == "colspan") {
                ColSpan = FCStr.convertStrToInt(value);
            }
            else if (name == "font") {
                if (m_style == null) {
                    m_style = new FCGridCellStyle();
                }
                m_style.Font = FCStr.convertStrToFont(value);
            }
            else if (name == "textcolor") {
                if (m_style == null) {
                    m_style = new FCGridCellStyle();
                }
                m_style.TextColor = FCStr.convertStrToColor(value);
            }
            else if (name == "name") {
                Name = value;
            }
            else if (name == "rowspan") {
                RowSpan = FCStr.convertStrToInt(value);
            }
            else if (name == "text") {
                Text = value;
            }
        }

        /// <summary>
        /// 设置字符型数值
        /// </summary>
        /// <param name="value">数值</param>
        public virtual void setString(String value) {
        }
    }

    /// <summary>
    /// 控件单元格
    /// </summary>
    public class FCGridControlCell : FCGridCell {
        /// <summary>
        /// 创建单元格
        /// </summary>
        public FCGridControlCell() {
            m_touchDownEvent = new FCTouchEvent(controlTouchDown);
            m_touchMoveEvent = new FCTouchEvent(controlTouchMove);
            m_touchUpEvent = new FCTouchEvent(controlToucheUp);
            m_touchWheelEvent = new FCTouchEvent(controlTouchWheel);
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~FCGridControlCell() {
            if (m_control != null) {
                m_control.delete();
                m_control = null;
            }
            m_touchDownEvent = null;
            m_touchMoveEvent = null;
            m_touchUpEvent = null;
            m_touchWheelEvent = null;
        }

        /// <summary>
        /// 触摸按下事件
        /// </summary>
        private FCTouchEvent m_touchDownEvent;

        /// <summary>
        /// 触摸移动事件
        /// </summary>
        private FCTouchEvent m_touchMoveEvent;

        /// <summary>
        /// 触摸抬起事件
        /// </summary>
        private FCTouchEvent m_touchUpEvent;

        /// <summary>
        /// 触摸滚轮事件
        /// </summary>
        private FCTouchEvent m_touchWheelEvent;

        protected FCView m_control;

        /// <summary>
        /// 获取或设置控件
        /// </summary>
        public virtual FCView Control {
            get { return m_control; }
            set { m_control = value; }
        }

        /// <summary>
        /// 获取要绘制的文字
        /// </summary>
        /// <returns>要绘制的文字</returns>
        public override String getPaintText() {
            return "";
        }

        /// <summary>
        /// 获取字符型数值
        /// </summary>
        /// <returns>字符型数值</returns>
        public override String getString() {
            if (m_control != null) {
                return m_control.Text;
            }
            else {
                return "";
            }
        }

        /// <summary>
        /// 控件触摸按下事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="touchInfo">触摸信息</param>
        protected void controlTouchDown(object sender, FCTouchInfo touchInfo) {
            onControlTouchDown(touchInfo);
        }

        /// <summary>
        /// 控件触摸移动事件
        /// </summary>
        /// <param name="sender">调用者</param>
        protected void controlTouchMove(object sender, FCTouchInfo touchInfo) {
            onControlTouchMove(touchInfo);
        }

        /// <summary>
        /// 控件触摸抬起事件
        /// </summary>
        /// <param name="sender">调用者</param>
        protected void controlToucheUp(object sender, FCTouchInfo touchInfo) {
            onControlTouchUp(touchInfo);
        }

        /// <summary>
        /// 控件触摸滚轮滚动事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="touchInfo">触摸信息</param>
        protected void controlTouchWheel(object sender, FCTouchInfo touchInfo) {
            onControlTouchWheel(touchInfo);
        }

        /// <summary>
        /// 添加单元格方法
        /// </summary>
        public override void onAdd() {
            FCGrid grid = Grid;
            if (m_control != null && grid != null) {
                grid.addControl(m_control);
                m_control.addEvent(m_touchDownEvent, FCEventID.TOUCHDOWN);
                m_control.addEvent(m_touchMoveEvent, FCEventID.TOUCHMOVE);
                m_control.addEvent(m_touchUpEvent, FCEventID.TOUCHUP);
                m_control.addEvent(m_touchWheelEvent, FCEventID.TOUCHWHEEL);
            }
        }

        /// <summary>
        /// 控件触摸按下方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public virtual void onControlTouchDown(FCTouchInfo touchInfo) {
            FCGrid grid = Grid;
            if (m_control != null && grid != null) {
                FCTouchInfo newTouchInfo = touchInfo.clone();
                newTouchInfo.m_firstPoint = grid.pointToControl(m_control.pointToNative(touchInfo.m_firstPoint));
                newTouchInfo.m_secondPoint = grid.pointToControl(m_control.pointToNative(touchInfo.m_secondPoint));
                grid.onTouchDown(newTouchInfo);
            }
        }

        /// <summary>
        /// 控件触摸移动方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public virtual void onControlTouchMove(FCTouchInfo touchInfo) {
            FCGrid grid = Grid;
            if (m_control != null && grid != null) {
                FCTouchInfo newTouchInfo = touchInfo.clone();
                newTouchInfo.m_firstPoint = grid.pointToControl(m_control.pointToNative(touchInfo.m_firstPoint));
                newTouchInfo.m_secondPoint = grid.pointToControl(m_control.pointToNative(touchInfo.m_secondPoint));
                grid.onTouchMove(newTouchInfo);
            }
        }

        /// <summary>
        /// 控件触摸抬起方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public virtual void onControlTouchUp(FCTouchInfo touchInfo) {
            FCGrid grid = Grid;
            if (m_control != null && grid != null) {
                FCTouchInfo newTouchInfo = touchInfo.clone();
                newTouchInfo.m_firstPoint = grid.pointToControl(m_control.pointToNative(touchInfo.m_firstPoint));
                newTouchInfo.m_secondPoint = grid.pointToControl(m_control.pointToNative(touchInfo.m_secondPoint));
                grid.onTouchUp(newTouchInfo);
            }
        }

        /// <summary>
        /// 控件触摸滚轮滚动方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public virtual void onControlTouchWheel(FCTouchInfo touchInfo) {
            FCGrid grid = Grid;
            if (m_control != null && grid != null) {
                FCTouchInfo newTouchInfo = touchInfo.clone();
                newTouchInfo.m_firstPoint = grid.pointToControl(m_control.pointToNative(touchInfo.m_firstPoint));
                newTouchInfo.m_secondPoint = grid.pointToControl(m_control.pointToNative(touchInfo.m_secondPoint));
                grid.onTouchWheel(newTouchInfo);
            }
        }

        /// <summary>
        /// 重绘方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="rect">矩形</param>
        /// <param name="clipRect">裁剪矩形</param>
        /// <param name="isAlternate">是否交替行</param>
        public override void onPaint(FCPaint paint, FCRect rect, FCRect clipRect, bool isAlternate) {
            base.onPaint(paint, rect, clipRect, isAlternate);
            onPaintControl(paint, rect, clipRect);
        }

        /// <summary>
        /// 重绘方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="rect">矩形</param>
        /// <param name="clipRect">裁剪矩形</param>
        public virtual void onPaintControl(FCPaint paint, FCRect rect, FCRect clipRect) {
            if (m_control != null) {
                FCRect bounds = new FCRect(rect.left + 1, rect.top + 1, rect.right - 1, rect.bottom - 1);
                m_control.Bounds = bounds;
                clipRect.left -= rect.left;
                clipRect.top -= rect.top;
                clipRect.right -= rect.left;
                clipRect.bottom -= rect.top;
                m_control.Region = clipRect;
            }
        }

        /// <summary>
        /// 移除单元格方法
        /// </summary>
        public override void onRemove() {
            FCGrid grid = Grid;
            if (m_control != null && grid != null) {
                m_control.removeEvent(m_touchDownEvent, FCEventID.TOUCHDOWN);
                m_control.removeEvent(m_touchMoveEvent, FCEventID.TOUCHMOVE);
                m_control.removeEvent(m_touchUpEvent, FCEventID.TOUCHUP);
                m_control.removeEvent(m_touchWheelEvent, FCEventID.TOUCHWHEEL);
                grid.removeControl(m_control);
            }
        }

        /// <summary>
        /// 设置字符型数值
        /// </summary>
        /// <param name="value">数值</param>
        public override void setString(String value) {
            if (m_control != null) {
                m_control.Text = value;
            }
        }
    }
}
