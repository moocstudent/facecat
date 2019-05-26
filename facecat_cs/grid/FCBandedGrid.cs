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
using System.Text;

namespace FaceCat {
    /// <summary>
    /// 复合列头表格
    /// </summary>
    public class FCBandedGrid : FCGrid {
        /// <summary>
        /// 创建复合列头表格
        /// </summary>
        public FCBandedGrid() {
        }

        /// <summary>
        /// 表格带
        /// </summary>
        protected ArrayList<FCGridBand> m_bands = new ArrayList<FCGridBand>();

        /// <summary>
        /// 获取所有可见带的宽度
        /// </summary>
        protected virtual int AllVisibleBandsWidth {
            get {
                int allVisibleBandsWidth = 0;
                int bandsSize = m_bands.size();
                for (int i = 0; i < bandsSize; i++) {
                    FCGridBand band = m_bands.get(i);
                    if (band.Visible) {
                        allVisibleBandsWidth += band.Width;
                    }
                }
                return allVisibleBandsWidth;
            }
        }

        /// <summary>
        /// 添加表格带
        /// </summary>
        /// <param name="band">表格带</param>
        public void addBand(FCGridBand band) {
            band.Grid = this;
            m_bands.add(band);
            int bandSize = m_bands.size();
            for (int i = 0; i < bandSize; i++) {
                m_bands.get(i).Index = i;
            }
            addControl(band);
        }

        /// <summary>
        /// 添加列
        /// </summary>
        /// <param name="column">列</param>
        public override void addColumn(FCGridColumn column) {
            FCBandedFCGridColumn bandedColumn = column as FCBandedFCGridColumn;
            if (bandedColumn != null) {
                column.Grid = this;
                m_columns.add(column);
                addControl(column);
            }
        }

        /// <summary>
        /// 清除表格带
        /// </summary>
        public void clearBands() {
            int bandsSize = m_bands.size();
            for (int i = 0; i < bandsSize; i++) {
                FCGridBand band = m_bands.get(i);
                removeControl(band);
                band.delete();
            }
            m_bands.clear();
        }

        /// <summary>
        /// 清除所有的列
        /// </summary>
        public override void clearColumns() {
        }

        /// <summary>
        /// 获取表格带列表
        /// </summary>
        /// <returns>表格带集合</returns>
        public ArrayList<FCGridBand> getBands() {
            return m_bands;
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <returns>控件类型</returns>
        public override String getControlType() {
            return "BandedGrid";
        }

        /// <summary>
        /// 获取内容的宽度
        /// </summary>
        /// <returns>宽度</returns>
        public override int getContentWidth() {
            FCHScrollBar hScrollBar = HScrollBar;
            FCVScrollBar vScrollBar = VScrollBar;
            int wmax = 0;
            ArrayList<FCView> controls = m_controls;
            int controlSize = controls.size();
            for (int i = 0; i < controlSize; i++) {
                FCView control = controls.get(i);
                if (control.Visible && control != hScrollBar && control != vScrollBar) {
                    int right = control.Right;
                    if (right > wmax) {
                        wmax = right;
                    }
                }
            }
            int allVisibleBandsWidth = AllVisibleBandsWidth;
            return wmax > allVisibleBandsWidth ? wmax : allVisibleBandsWidth;
        }

        /// <summary>
        /// 插入表格带
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="band">表格带</param>
        public void insertBand(int index, FCGridBand band) {
            band.Grid = this;
            m_bands.Insert(index, band);
            int bandSize = m_bands.size();
            for (int i = 0; i < bandSize; i++) {
                m_bands.get(i).Index = i;
            }
            addControl(band);
        }

        /// <summary>
        /// 设置控件不可见方法
        /// </summary>
        public override void onSetEmptyClipRegion() {
            //隐藏控件
            ArrayList<FCView> controls = m_controls;
            int controlsSize = controls.size();
            FCRect emptyClipRect = new FCRect();
            for (int i = 0; i < controlsSize; i++) {
                FCView control = controls.get(i);
                FCScrollBar scrollBar = control as FCScrollBar;
                FCGridColumn gridColumn = control as FCGridColumn;
                FCGridBand gridBand = control as FCGridBand;
                if (control != EditTextBox && scrollBar == null && gridColumn == null && gridBand == null) {
                    control.Region = emptyClipRect;
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
                removeControl(band);
            }
        }

        /// <summary>
        /// 移除列头
        /// </summary>
        /// <param name="column">列头</param>
        public override void removeColumn(FCGridColumn column) {
            FCBandedFCGridColumn bandedColumn = column as FCBandedFCGridColumn;
            if (bandedColumn != null) {
                m_columns.remove(column);
                removeControl(column);
            }
        }

        /// <summary>
        /// 重置列头布局
        /// </summary>
        public override void resetHeaderLayout() {
            int left = 0, top = 0, scrollH = 0;
            FCHScrollBar hScrollBar = HScrollBar;
            if (hScrollBar != null && hScrollBar.Visible) {
                scrollH = -hScrollBar.Pos;
            }
            int headerHeight = HeaderVisible ? HeaderHeight : 0;
            int horizontalOffset = HorizontalOffset;
            int verticalOffset = VerticalOffset;
            int bandsSize = m_bands.size();
            for (int i = 0; i < bandsSize; i++) {
                FCGridBand band = m_bands.get(i);
                if (band.Visible) {
                    int bandHeight = headerHeight < band.Height ? headerHeight : band.Height;
                    FCRect bandRect = new FCRect(left + horizontalOffset, top + verticalOffset,
                       left + horizontalOffset + band.Width, top + bandHeight + verticalOffset);
                    bool hasFrozenColumn = false;
                    ArrayList<FCBandedFCGridColumn> childColumns = band.getAllChildColumns();
                    int childColumnsSize = childColumns.size();
                    for (int j = 0; j < childColumnsSize; j++) {
                        if (childColumns.get(j).Frozen) {
                            hasFrozenColumn = true;
                            break;
                        }
                    }
                    if (!hasFrozenColumn) {
                        bandRect.left += scrollH;
                        bandRect.right += scrollH;
                    }
                    band.Bounds = bandRect;
                    band.resetHeaderLayout();
                    left += band.Width;
                }
            }
        }

        /// <summary>
        /// 重新布局
        /// </summary>
        public override void update() {
            if (!m_lockUpdate) {
                int bandsSize = m_bands.size();
                //清除所有的列
                int col = 0;
                for (int i = 0; i < bandsSize; i++) {
                    FCGridBand band = m_bands.get(i);
                    ArrayList<FCBandedFCGridColumn> childColumns = band.getAllChildColumns();
                    int childColumnsSize = childColumns.size();
                    for (int j = 0; j < childColumnsSize; j++) {
                        FCBandedFCGridColumn column = childColumns.get(j);
                        column.Index = col;
                        col++;
                    }
                }
            }
            base.update();
        }
    }
}
