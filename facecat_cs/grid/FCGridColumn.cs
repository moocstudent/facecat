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
    /// 表格列
    /// </summary>
    public class FCGridColumn : FCButton {
        /// <summary>
        /// 创建列
        /// </summary>
        public FCGridColumn() {
            Width = 100;
        }

        /// <summary>
        /// 创建列
        /// </summary>
        /// <param name="text">标题</param>
        public FCGridColumn(String text) {
            Text = text;
            Width = 100;
        }

        /// <summary>
        /// 起始宽度
        /// </summary>
        protected int m_beginWidth = 0;

        /// <summary>
        /// 调整大小状态，1:左侧 2:右侧
        /// </summary>
        protected int m_resizeState;

        /// <summary>
        /// 触摸按下时的坐标
        /// </summary>
        protected FCPoint m_touchDownPoint;

        protected bool m_allowResize = false;

        /// <summary>
        /// 获取或设置是否可以调整大小
        /// </summary>
        public virtual bool AllowResize {
            get { return m_allowResize; }
            set { m_allowResize = value; }
        }

        protected bool m_allowSort = true;

        /// <summary>
        /// 获取或设置是否可以排序
        /// </summary>
        public virtual bool AllowSort {
            get { return m_allowSort; }
            set { m_allowSort = value; }
        }

        protected FCHorizontalAlign m_cellAlign = FCHorizontalAlign.Left;

        /// <summary>
        /// 获取或设置内容的横向排列样式
        /// </summary>
        public virtual FCHorizontalAlign CellAlign {
            get { return m_cellAlign; }
            set { m_cellAlign = value; }
        }

        protected String m_columnType = "";

        /// <summary>
        /// 获取或设置列的类型
        /// </summary>
        public virtual String ColumnType {
            get { return m_columnType; }
            set { m_columnType = value; }
        }

        protected bool m_frozen = false;

        /// <summary>
        /// 获取或设置是否冻结
        /// </summary>
        public virtual bool Frozen {
            get { return m_frozen; }
            set { m_frozen = value; }
        }

        protected FCGrid m_grid;

        /// <summary>
        /// 获取或设置表格
        /// </summary>
        public virtual FCGrid Grid {
            get { return m_grid; }
            set { m_grid = value; }
        }

        protected FCRect m_headerRect;

        /// <summary>
        /// 获取或设置头部的矩形
        /// </summary>
        public virtual FCRect HeaderRect {
            get { return m_headerRect; }
            set { m_headerRect = value; }
        }

        protected int m_index = -1;

        /// <summary>
        /// 获取或设置索引
        /// </summary>
        public virtual int Index {
            get { return m_index; }
            set { m_index = value; }
        }

        protected FCGridColumnSortMode m_sortMode = FCGridColumnSortMode.None;

        /// <summary>
        /// 获取或设置排序状态，0:不排序 1:升序 2:降序
        /// </summary>
        public virtual FCGridColumnSortMode SortMode {
            get { return m_sortMode; }
            set { m_sortMode = value; }
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <returns>控件类型</returns>
        public override String getControlType() {
            return "FCGridColumn";
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">返回属性值</param>
        /// <param name="type">返回属性类型</param>
        public override void getProperty(String name, ref String value, ref String type) {
            if (name == "allowresize") {
                type = "bool";
                value = FCStr.convertBoolToStr(AllowResize);
            }
            else if (name == "allowsort") {
                type = "bool";
                value = FCStr.convertBoolToStr(AllowSort);
            }
            else if (name == "cellalign") {
                type = "enum:FCHorizontalAlign";
                value = FCStr.convertHorizontalAlignToStr(CellAlign);
            }
            else if (name == "columntype") {
                type = "text";
                value = ColumnType;
            }
            else if (name == "frozen") {
                type = "bool";
                value = FCStr.convertBoolToStr(Frozen);
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
            propertyNames.AddRange(new String[] { "AllowResize", "AllowSort", "CellAlign", "ColumnType", "Frozen" });
            return propertyNames;
        }

        /// <summary>
        /// 点击方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onClick(FCTouchInfo touchInfo) {
            base.onClick(touchInfo);
            if (m_resizeState == 0) {
                switch (m_sortMode) {
                    case FCGridColumnSortMode.None:
                    case FCGridColumnSortMode.Desc:
                        m_grid.sortColumn(m_grid, this, FCGridColumnSortMode.Asc);
                        break;
                    case FCGridColumnSortMode.Asc:
                        m_grid.sortColumn(m_grid, this, FCGridColumnSortMode.Desc);
                        break;
                }
            }
        }

        /// <summary>
        /// 拖动开始方法
        /// </summary>
        /// <returns>是否拖动</returns>
        public override bool onDragBegin() {
            return m_resizeState == 0;
        }

        /// <summary>
        /// 拖动中方法
        /// </summary>
        public override void onDragging() {
            base.onDragging();
            if (m_grid != null) {
                ArrayList<FCGridColumn> columns = m_grid.getColumns();
                int columnSize = columns.size();
                for (int i = 0; i < columnSize; i++) {
                    FCGridColumn column = columns.get(i);
                    if (column == this) {
                        FCGridColumn lastColumn = null;
                        FCGridColumn nextColumn = null;
                        int lastIndex = i - 1;
                        int nextIndex = i + 1;
                        while (lastIndex >= 0) {
                            FCGridColumn thatColumn = columns.get(lastIndex);
                            if (thatColumn.Visible) {
                                lastColumn = thatColumn;
                                break;
                            }
                            else {
                                lastIndex--;
                            }
                        }
                        while (nextIndex < columnSize) {
                            FCGridColumn thatColumn = columns.get(nextIndex);
                            if (thatColumn.Visible) {
                                nextColumn = thatColumn;
                                break;
                            }
                            else {
                                nextIndex++;
                            }
                        }
                        //交换列
                        FCNative native = Native;
                        int clientX = native.clientX(this);
                        if (lastColumn != null) {
                            int lastClientX = native.clientX(lastColumn);
                            if (clientX < lastClientX + lastColumn.Width / 2) {
                                columns.set(lastIndex, this);
                                columns.set(i, lastColumn);
                                m_grid.update();
                                break;
                            }
                        }
                        if (nextColumn != null) {
                            int nextClientX = native.clientX(nextColumn);
                            if (clientX + column.Width > nextClientX + nextColumn.Width / 2) {
                                columns.set(nextIndex, this);
                                columns.set(i, nextColumn);
                                m_grid.update();
                                break;
                            }
                        }
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 触摸按下方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onTouchDown(FCTouchInfo touchInfo) {
            base.onTouchDown(touchInfo);
            if (touchInfo.m_firstTouch && touchInfo.m_clicks == 1) {
                FCPoint mp = touchInfo.m_firstPoint;
                if (m_allowResize) {
                    if (m_index > 0 && mp.x < 5) {
                        m_resizeState = 1;
                        m_beginWidth = Grid.getColumn(m_index - 1).Width;
                    }
                    else if (mp.x > Width - 5) {
                        m_resizeState = 2;
                        m_beginWidth = Width;
                    }
                    m_touchDownPoint = Native.TouchPoint;
                }
            }
        }

        /// <summary>
        /// 触摸移动方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onTouchMove(FCTouchInfo touchInfo) {
            base.onTouchMove(touchInfo);
            if (m_allowResize) {
                FCPoint mp = touchInfo.m_firstPoint;
                if (m_resizeState > 0) {
                    FCPoint curPoint = Native.TouchPoint;
                    int newWidth = m_beginWidth + (curPoint.x - m_touchDownPoint.x);
                    if (newWidth > 0) {
                        if (m_resizeState == 1) {
                            Grid.getColumn(m_index - 1).Width = newWidth;
                        }
                        else if (m_resizeState == 2) {
                            Width = newWidth;
                        }
                    }
                    if (m_grid != null) {
                        m_grid.update();
                        m_grid.invalidate();
                    }
                }
                else {
                    if ((m_index > 0 && mp.x < 5) || mp.x > Width - 5) {
                        Cursor = FCCursors.SizeWE;
                    }
                    else {
                        Cursor = FCCursors.Arrow;
                    }
                }
                if (IsDragging) {
                    Cursor = FCCursors.Arrow;
                }
            }
        }

        /// <summary>
        /// 触摸抬起方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onTouchUp(FCTouchInfo touchInfo) {
            base.onTouchUp(touchInfo);
            Cursor = FCCursors.Arrow;
            m_resizeState = 0;
            if (m_grid != null) {
                m_grid.invalidate();
            }
        }

        /// <summary>
        /// 重绘前景方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void onPaintForeground(FCPaint paint, FCRect clipRect) {
            base.onPaintForeground(paint, clipRect);
            if (Native != null && m_grid != null) {
                FCRect rect = new FCRect(0, 0, Width, Height);
                int tLeft = rect.right - 15;
                int midTop = rect.top + (rect.bottom - rect.top) / 2;
                long textColor = getPaintingTextColor();
                //升序
                if (m_sortMode == FCGridColumnSortMode.Asc) {
                    FCPoint[] points = new FCPoint[3] 
                    { 
                        new FCPoint(tLeft + 5, midTop - 5), 
                        new FCPoint(tLeft, midTop + 5), 
                        new FCPoint(tLeft + 10, midTop + 5) 
                    };
                    paint.fillPolygon(textColor, points);
                }
                //降序
                else if (m_sortMode == FCGridColumnSortMode.Desc) {
                    FCPoint[] points = new FCPoint[3] 
                    { 
                        new FCPoint(tLeft + 5, midTop + 5), 
                        new FCPoint(tLeft, midTop - 5), 
                        new FCPoint(tLeft + 10, midTop - 5) 
                    };
                    paint.fillPolygon(textColor, points);
                }
            }
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        public override void setProperty(String name, String value) {
            if (name == "allowresize") {
                AllowResize = FCStr.convertStrToBool(value);
            }
            else if (name == "allowsort") {
                AllowSort = FCStr.convertStrToBool(value);
            }
            else if (name == "cellalign") {
                CellAlign = FCStr.convertStrToHorizontalAlign(value);
            }
            else if (name == "columntype") {
                ColumnType = value;
            }
            else if (name == "frozen") {
                Frozen = FCStr.convertStrToBool(value);
            }
            else {
                base.setProperty(name, value);
            }
        }
    }
}
