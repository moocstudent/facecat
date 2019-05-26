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
using System.Text;

namespace FaceCat {
    /// <summary>
    /// 表格带列
    /// </summary>
    public class FCBandedFCGridColumn : FCGridColumn {
        /// <summary>
        /// 创建表格带列
        /// </summary>
        public FCBandedFCGridColumn() {
        }

        protected FCGridBand m_band;

        /// <summary>
        /// 获取或设置表格带
        /// </summary>
        public virtual FCGridBand Band {
            get { return m_band; }
            set { m_band = value; }
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <returns>控件类型</returns>
        public override String getControlType() {
            return "BandedFCGridColumn";
        }

        /// <summary>
        /// 拖动开始方法
        /// </summary>
        /// <returns>是否拖动</returns>
        public override bool onDragBegin() {
            return m_resizeState == 0;
        }

        /// <summary>
        /// 触摸按下方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onTouchDown(FCTouchInfo touchInfo) {
            callTouchEvents(FCEventID.TOUCHDOWN, touchInfo);
            if (m_band != null) {
                if (touchInfo.m_firstTouch && touchInfo.m_clicks == 1) {
                    FCPoint mp = touchInfo.m_firstPoint;
                    if (AllowResize) {
                        ArrayList<FCBandedFCGridColumn> bandColumns = m_band.getColumns();
                        int columnsSize = bandColumns.size();
                        int index = -1;
                        for (int i = 0; i < columnsSize; i++) {
                            if (this == bandColumns.get(i)) {
                                index = i;
                                break;
                            }
                        }
                        if (index > 0 && mp.x < 5) {
                            m_resizeState = 1;
                            m_beginWidth = bandColumns.get(index - 1).Width;
                        }
                        else if (index < columnsSize - 1 && mp.x > Width - 5) {
                            m_resizeState = 2;
                            m_beginWidth = Width;
                        }
                        m_touchDownPoint = Native.TouchPoint;
                    }
                }
            }
            invalidate();
        }

        /// <summary>
        /// 触摸移动方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onTouchMove(FCTouchInfo touchInfo) {
            callTouchEvents(FCEventID.TOUCHMOVE, touchInfo);
            FCGrid grid = Grid;
            if (m_band != null && grid != null) {
                FCPoint mp = touchInfo.m_firstPoint;
                if (AllowResize) {
                    ArrayList<FCBandedFCGridColumn> bandColumns = m_band.getColumns();
                    int columnsSize = bandColumns.size();
                    int index = -1;
                    int width = Width;
                    for (int i = 0; i < columnsSize; i++) {
                        if (this == bandColumns.get(i)) {
                            index = i;
                            break;
                        }
                    }
                    if (m_resizeState > 0) {
                        FCPoint curPoint = Native.TouchPoint;
                        int newWidth = m_beginWidth + (curPoint.x - m_touchDownPoint.x);
                        if (newWidth > 0) {
                            if (m_resizeState == 1) {
                                FCBandedFCGridColumn leftColumn = bandColumns.get(index - 1);
                                int leftWidth = leftColumn.Width;
                                leftColumn.Width = newWidth;
                                width += leftWidth - newWidth;
                                Width = width;
                            }
                            else if (m_resizeState == 2) {
                                Width = newWidth;
                                FCBandedFCGridColumn rightColumn = bandColumns.get(index + 1);
                                int rightWidth = rightColumn.Width;
                                rightWidth += width - newWidth;
                                rightColumn.Width = rightWidth;
                            }
                        }
                        grid.invalidate();
                        return;
                    }
                    else {
                        FCCursors oldCursor = Cursor;
                        FCCursors newCursor = oldCursor;
                        if ((index > 0 && mp.x < 5) || (index < columnsSize - 1 && mp.x > width - 5)) {
                            newCursor = FCCursors.SizeWE;
                        }
                        else {
                            newCursor = FCCursors.Arrow;
                        }
                        if (oldCursor != newCursor) {
                            Cursor = newCursor;
                            invalidate();
                        }
                    }
                    if (!IsDragging) {
                        Cursor = FCCursors.Arrow;
                    }
                }
            }
        }
    }
}
