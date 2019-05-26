/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.grid;

import facecat.topin.scroll.*;
import facecat.topin.core.*;
import java.util.*;

/**
 * 复合列头表格
 */
public class FCBandedGrid extends FCGrid {

    /**
     * 创建复合列头表格
     */
    public FCBandedGrid() {
    }

    /**
     * 表格带
     */
    protected ArrayList<FCGridBand> m_bands = new ArrayList<FCGridBand>();

    /**
     * 获取所有可见带的宽度
     */
    protected int getAllVisibleBandsWidth() {
        int allVisibleBandsWidth = 0;
        int bandsSize = m_bands.size();
        for (int i = 0; i < bandsSize; i++) {
            FCGridBand band = m_bands.get(i);
            if (band.isVisible()) {
                allVisibleBandsWidth += band.getWidth();
            }
        }
        return allVisibleBandsWidth;
    }

    /**
     * 添加表格带
     *
     * @param band 表格带
     */
    public void addBand(FCGridBand band) {
        band.setGrid(this);
        m_bands.add(band);
        int bandSize = m_bands.size();
        for (int i = 0; i < bandSize; i++) {
            m_bands.get(i).setIndex(i);
        }
        addControl(band);
    }

    /**
     * 添加列
     *
     * @param column 列
     */
    @Override
    public void addColumn(FCGridColumn column) {
        FCBandedGridColumn bandedColumn = (FCBandedGridColumn) ((column instanceof FCBandedGridColumn) ? column : null);
        if (bandedColumn != null) {
            column.setGrid(this);
            m_columns.add(column);
            addControl(column);
        }
    }

    /**
     * 清除表格带
     */
    public void clearBands() {
        int bandsSize = m_bands.size();
        for (int i = 0; i < bandsSize; i++) {
            FCGridBand band = m_bands.get(i);
            removeControl(band);
            band.delete();
        }
        m_bands.clear();
    }

    /**
     * 清除所有的列
     */
    @Override
    public void clearColumns() {
    }

    /**
     * 获取表格带列表
     */
    public ArrayList<FCGridBand> getBands() {
        return m_bands;
    }

    /**
     * 获取控件类型
     */
    @Override
    public String getControlType() {
        return "BandedGrid";
    }

    /**
     * 获取内容的宽度
     */
    @Override
    public int getContentWidth() {
        FCHScrollBar hScrollBar = getHScrollBar();
        FCVScrollBar vScrollBar = getVScrollBar();
        int wmax = 0;
        ArrayList<FCView> controls = getControls();
        int controlSize = controls.size();
        for (int i = 0; i < controlSize; i++) {
            FCView control = controls.get(i);
            if (control.isVisible() && control != hScrollBar && control != vScrollBar) {
                int right = control.getRight();
                if (right > wmax) {
                    wmax = right;
                }
            }
        }
        int allVisibleBandsWidth = getAllVisibleBandsWidth();
        return wmax > allVisibleBandsWidth ? wmax : allVisibleBandsWidth;
    }

    /**
     * 插入表格带
     *
     * @param index 索引
     * @param band 表格带
     */
    public void insertBand(int index, FCGridBand band) {
        band.setGrid(this);
        m_bands.add(index, band);
        int bandSize = m_bands.size();
        for (int i = 0; i < bandSize; i++) {
            m_bands.get(i).setIndex(i);
        }
        addControl(band);
    }

    /**
     * 设置控件不可见方法
     */
    @Override
    public void onSetEmptyClipRegion() {
        ArrayList<FCView> controls = getControls();
        int controlsSize = controls.size();
        FCRect emptyClipRect = new FCRect();
        for (int i = 0; i < controlsSize; i++) {
            FCView control = controls.get(i);
            FCScrollBar scrollBar = (FCScrollBar) ((control instanceof FCScrollBar) ? control : null);
            FCGridColumn gridColumn = (FCGridColumn) ((control instanceof FCGridColumn) ? control : null);
            FCGridBand gridBand = (FCGridBand) ((control instanceof FCGridBand) ? control : null);
            if (control != getEditTextBox() && scrollBar == null && gridColumn == null && gridBand == null) {
                control.setRegion(emptyClipRect);
            }
        }
    }

    /**
     * 移除表格带
     *
     * @param band 表格带
     */
    public void removeBand(FCGridBand band) {
        if (m_bands.contains(band)) {
            m_bands.remove(band);
            int bandSize = m_bands.size();
            for (int i = 0; i < bandSize; i++) {
                m_bands.get(i).setIndex(i);
            }
            removeControl(band);
        }
    }

    /**
     * 移除列头
     *
     * @param column 列头
     */
    @Override
    public void removeColumn(FCGridColumn column) {
        FCBandedGridColumn bandedColumn = (FCBandedGridColumn) ((column instanceof FCBandedGridColumn) ? column : null);
        if (bandedColumn != null) {
            m_columns.remove(column);
            removeControl(column);
        }
    }

    /**
     * 重置列头布局
     */
    @Override
    public void resetHeaderLayout() {
        int left = 0, top = 0, scrollH = 0;
        FCHScrollBar hScrollBar = getHScrollBar();
        if (hScrollBar != null && hScrollBar.isVisible()) {
            scrollH = -hScrollBar.getPos();
        }
        int headerHeight = isHeaderVisible() ? getHeaderHeight() : 0;
        int horizontalOffset = getHorizontalOffset();
        int verticalOffset = getVerticalOffset();
        int bandsSize = m_bands.size();
        for (int i = 0; i < bandsSize; i++) {
            FCGridBand band = m_bands.get(i);
            if (band.isVisible()) {
                int bandHeight = headerHeight < band.getHeight() ? headerHeight : band.getHeight();
                FCRect bandRect = new FCRect(left + horizontalOffset, top + verticalOffset, left + horizontalOffset + band.getWidth(), top + bandHeight + verticalOffset);
                boolean hasFrozenColumn = false;
                ArrayList<FCBandedGridColumn> childColumns = band.getAllChildColumns();
                int childColumnsSize = childColumns.size();
                for (int j = 0; j < childColumnsSize; j++) {
                    if (childColumns.get(j).isFrozen()) {
                        hasFrozenColumn = true;
                        break;
                    }
                }
                if (!hasFrozenColumn) {
                    bandRect.left += scrollH;
                    bandRect.right += scrollH;
                }
                band.setBounds(bandRect);
                band.resetHeaderLayout();
                left += band.getWidth();
            }
        }
    }

    /**
     * 重新布局
     */
    @Override
    public void update() {
        if (!m_lockUpdate) {
            int bandsSize = m_bands.size();
            // 清除所有的列
            int col = 0;
            for (int i = 0; i < bandsSize; i++) {
                FCGridBand band = m_bands.get(i);
                ArrayList<FCBandedGridColumn> childColumns = band.getAllChildColumns();
                int childColumnsSize = childColumns.size();
                for (int j = 0; j < childColumnsSize; j++) {
                    FCBandedGridColumn column = childColumns.get(j);
                    column.setIndex(col);
                    col++;
                }
            }
        }
        super.update();
    }
}
