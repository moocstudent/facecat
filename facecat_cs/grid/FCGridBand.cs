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
    /// 表格带
    /// </summary>
    public class FCGridBand : FCButton {
        /// <summary>
        /// 创建表格带
        /// </summary>
        public FCGridBand() {
            Width = 100;
        }

        /// <summary>
        /// 子表格带
        /// </summary>
        protected ArrayList<FCGridBand> m_bands = new ArrayList<FCGridBand>();

        /// <summary>
        /// 子表格列
        /// </summary>
        protected ArrayList<FCBandedFCGridColumn> m_columns = new ArrayList<FCBandedFCGridColumn>();

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

        protected FCBandedGrid m_grid;

        /// <summary>
        /// 获取或设置表格
        /// </summary>
        public virtual FCBandedGrid Grid {
            get { return m_grid; }
            set { m_grid = value; }
        }
        protected int m_index = -1;

        /// <summary>
        /// 获取或设置索引
        /// </summary>
        public virtual int Index {
            get { return m_index; }
            set { m_index = value; }
        }

        protected FCGridBand m_parentBand;

        /// <summary>
        /// 获取或设置父表格带
        /// </summary>
        public virtual FCGridBand ParentBand {
            get { return m_parentBand; }
            set { m_parentBand = value; }
        }

        /// <summary>
        /// 添加表格带
        /// </summary>
        /// <param name="band">表格带</param>
        public void AddBand(FCGridBand band) {
            band.Grid = m_grid;
            band.ParentBand = this;
            m_bands.add(band);
            int bandSize = m_bands.size();
            for (int i = 0; i < bandSize; i++) {
                m_bands.get(i).Index = i;
            }
            m_grid.addControl(band);
        }

        /// <summary>
        /// 添加表格列
        /// </summary>
        /// <param name="column">表格列</param>
        public void AddColumn(FCBandedFCGridColumn column) {
            column.Band = this;
            m_columns.add(column);
            m_grid.addColumn(column);
        }

        /// <summary>
        /// 清除表格带
        /// </summary>
        public void ClearBands() {
            int bandsSize = m_bands.size();
            for (int i = 0; i < bandsSize; i++) {
                FCGridBand band = m_bands.get(i);
                m_grid.removeControl(band);
                band.delete();
            }
            m_bands.clear();
        }

        /// <summary>
        /// 清除列
        /// </summary>
        public void clearColumns() {
            int columnsSize = m_columns.size();
            for (int i = 0; i < columnsSize; i++) {
                FCBandedFCGridColumn column = m_columns.get(i);
                m_grid.removeColumn(column);
                column.delete();
            }
            m_columns.clear();
        }

        /// <summary>
        /// 获取所有的子表格列
        /// </summary>
        /// <returns>列头集合</returns>
        public ArrayList<FCBandedFCGridColumn> getAllChildColumns() {
            ArrayList<FCBandedFCGridColumn> columns = new ArrayList<FCBandedFCGridColumn>();
            int columnsSize = m_columns.size();
            for (int i = 0; i < columnsSize; i++) {
                FCBandedFCGridColumn column = m_columns.get(i);
                columns.add(column);
            }
            int bandsSize = m_bands.size();
            for (int i = 0; i < bandsSize; i++) {
                FCGridBand band = m_bands.get(i);
                ArrayList<FCBandedFCGridColumn> childColumns = band.getAllChildColumns();
                int childColumnsSize = childColumns.size();
                for (int j = 0; j < childColumnsSize; j++) {
                    FCBandedFCGridColumn childColumn = childColumns[j];
                    columns.add(childColumn);
                }
            }
            return columns;
        }

        /// <summary>
        /// 获取表格带列表
        /// </summary>
        /// <returns>表格带集合</returns>
        public ArrayList<FCGridBand> getBands() {
            return m_bands;
        }

        /// <summary>
        /// 获取列头
        /// </summary>
        /// <returns>列头集合</returns>
        public ArrayList<FCBandedFCGridColumn> getColumns() {
            return m_columns;
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <returns>控件类型</returns>
        public override String getControlType() {
            return "FCGridBand";
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
        }

        /// <summary>
        /// 获取属性名称列表
        /// </summary>
        /// <returns>属性名称列表</returns>
        public override ArrayList<String> getPropertyNames() {
            ArrayList<String> propertyNames = base.getPropertyNames();
            propertyNames.AddRange(new String[] { "AllowResize" });
            return propertyNames;
        }

        /// <summary>
        /// 销毁控件
        /// </summary>
        public override void delete() {
            ClearBands();
            clearColumns();
            base.delete();
        }

        /// <summary>
        /// 插入表格带
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="band">表格带</param>
        public void insertBand(int index, FCGridBand band) {
            band.Grid = m_grid;
            band.ParentBand = this;
            m_bands.Insert(index, band);
            int bandSize = m_bands.size();
            for (int i = 0; i < bandSize; i++) {
                m_bands.get(i).Index = i;
            }
            m_grid.addControl(band);
        }

        /// <summary>
        /// 插入表格列
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="column">表格列</param>
        public void insertColumn(int index, FCBandedFCGridColumn column) {
            column.Band = this;
            m_columns.Insert(index, column);
            m_grid.addColumn(column);
        }

        /// <summary>
        /// 触摸按下方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onTouchDown(FCTouchInfo touchInfo) {
            base.onTouchDown(touchInfo);
            if (touchInfo.m_firstTouch && touchInfo.m_clicks == 1) {
                if (m_allowResize) {
                    ArrayList<FCGridBand> bands = null;
                    if (m_parentBand != null) {
                        bands = m_parentBand.getBands();
                    }
                    else {
                        bands = m_grid.getBands();
                    }
                    int bandsSize = bands.size();
                    if (m_index > 0 && touchInfo.m_firstPoint.x < 5) {
                        m_resizeState = 1;
                        m_beginWidth = bands[m_index - 1].Width;
                    }
                    else if ((m_parentBand == null || m_index < bandsSize - 1) && touchInfo.m_firstPoint.x > Width - 5) {
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
                ArrayList<FCGridBand> bands = null;
                if (m_parentBand != null) {
                    bands = m_parentBand.getBands();
                }
                else {
                    bands = m_grid.getBands();
                }
                int bandsSize = bands.size();
                int width = Width;
                if (m_resizeState > 0) {
                    FCPoint curPoint = Native.TouchPoint;
                    int newWidth = m_beginWidth + (curPoint.x - m_touchDownPoint.x);
                    if (newWidth > 0) {
                        if (m_resizeState == 1) {
                            FCGridBand leftBand = bands[m_index - 1];
                            int leftWidth = leftBand.Width;
                            leftBand.Width = newWidth;
                            width += leftWidth - newWidth;
                            Width = width;
                        }
                        else if (m_resizeState == 2) {
                            Width = newWidth;
                            if (m_index < bandsSize - 1) {
                                FCGridBand rightBand = bands[m_index + 1];
                                int rightWidth = rightBand.Width;
                                rightWidth += width - newWidth;
                                rightBand.Width = rightWidth;
                            }
                            else {
                                if (m_grid != null) {
                                    m_grid.resetHeaderLayout();
                                    m_grid.update();
                                }
                            }
                        }
                    }
                    if (m_grid != null) {
                        m_grid.invalidate();
                    }
                }
                else {
                    if ((m_index > 0 && touchInfo.m_firstPoint.x < 5) || ((m_parentBand == null || m_index < bandsSize - 1) && touchInfo.m_firstPoint.x > width - 5)) {
                        Cursor = FCCursors.SizeWE;
                    }
                    else {
                        Cursor = FCCursors.Arrow;
                    }
                }
            }
        }

        /// <summary>
        /// 触摸抬起方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onTouchUp(FCTouchInfo touchInfo) {
            base.onTouchUp(touchInfo);
            if (m_resizeState != 0) {
                Cursor = FCCursors.Arrow;
                m_resizeState = 0;
                if (m_grid != null) {
                    m_grid.invalidate();
                }
            }
        }

        /// <summary>
        /// 移除表格带
        /// </summary>
        /// <param name="band">表格带</param>
        public void removeBand(FCGridBand band) {
            if (m_bands.Contains(band)) {
                m_bands.remove(band);
                int bandSize = m_bands.size();
                for (int i = 0; i < bandSize; i++) {
                    m_bands.get(i).Index = i;
                }
                m_grid.removeControl(band);
            }
        }

        /// <summary>
        /// 移除表格列
        /// </summary>
        /// <param name="column">表格列</param>
        public void removeColumn(FCBandedFCGridColumn column) {
            if (m_columns.Contains(column)) {
                m_columns.remove(column);
                m_grid.removeColumn(column);
            }
        }

        /// <summary>
        /// 重置列头布局
        /// </summary>
        public virtual void resetHeaderLayout() {
            int bandsSize = m_bands.size();
            FCRect bounds = Bounds;
            int left = bounds.left;
            int width = Width;
            if (bandsSize == 0) {
                int scrollH = 0;
                FCHScrollBar hScrollBar = Grid.HScrollBar;
                if (hScrollBar != null && hScrollBar.Visible) {
                    scrollH = -hScrollBar.Pos;
                }
                int columnsSize = m_columns.size();
                for (int i = 0; i < columnsSize; i++) {
                    FCBandedFCGridColumn column = m_columns.get(i);
                    if (column.Visible) {
                        int columnWidth = column.Width;
                        if (i == columnsSize - 1 || left + columnWidth > width + bounds.left) {
                            columnWidth = width + bounds.left - left;
                        }
                        FCRect cellRect = new FCRect(left, bounds.bottom, left + columnWidth, bounds.bottom + column.Height);
                        column.Bounds = cellRect;
                        cellRect.left -= scrollH;
                        cellRect.right -= scrollH;
                        column.HeaderRect = cellRect;
                        left += columnWidth;
                    }
                }
            }
            else {
                for (int i = 0; i < bandsSize; i++) {
                    FCGridBand band = m_bands.get(i);
                    if (band.Visible) {
                        int bandWidth = band.Width;
                        if (i == bandsSize - 1 || left + bandWidth > width + bounds.left) {
                            bandWidth = width + bounds.left - left;
                        }
                        FCRect cellRect = new FCRect(left, bounds.bottom, left + bandWidth, bounds.bottom + band.Height);
                        band.Bounds = cellRect;
                        band.resetHeaderLayout();
                        left += bandWidth;
                    }
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
            else {
                base.setProperty(name, value);
            }
        }
    }
}
