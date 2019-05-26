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
    /// 布局控件
    /// </summary>
    public class FCLayoutDiv : FCDiv {
        /// <summary>
        /// 创建布局控件
        /// </summary>
        public FCLayoutDiv() {
        }

        protected bool m_autoWrap = false;

        /// <summary>
        /// 获取或设置是否自动换行
        /// </summary>
        public virtual bool AutoWrap {
            get { return m_autoWrap; }
            set { m_autoWrap = value; }
        }

        protected FCLayoutStyle m_layoutStyle = FCLayoutStyle.LeftToRight;

        /// <summary>
        /// 获取或设置排列模式
        /// </summary>
        public virtual FCLayoutStyle LayoutStyle {
            get { return m_layoutStyle; }
            set { m_layoutStyle = value; }
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <returns>控件类型</returns>
        public override String getControlType() {
            return "LayoutDiv";
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">返回属性值</param>
        /// <param name="type">返回属性类型</param>
        public override void getProperty(String name, ref String value, ref String type) {
            if (name == "autowrap") {
                type = "bool";
                value = FCStr.convertBoolToStr(AutoWrap);
            }
            else if (name == "layoutstyle") {
                type = "enum:FCLayoutStyle";
                value = FCStr.convertLayoutStyleToStr(LayoutStyle);
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
            propertyNames.AddRange(new String[] { "AutoWrap", "LayoutStyle" });
            return propertyNames;
        }

        /// <summary>
        /// 重置布局
        /// </summary>
        public virtual bool onResetLayout() {
            bool reset = false;
            if (Native != null) {
                FCPadding padding = Padding;
                int left = padding.left, top = padding.top;
                int width = Width - padding.left - padding.right;
                int height = Height - padding.top - padding.bottom;
                int controlSize = m_controls.size();
                for (int i = 0; i < controlSize; i++) {
                    FCView control = m_controls.get(i);
                    if (control.Visible && control != HScrollBar && control != VScrollBar) {
                        FCSize size = control.Size;
                        int cLeft = control.Left, cTop = control.Top, cWidth = size.cx, cHeight = size.cy;
                        int nLeft = cLeft, nTop = cTop, nWidth = cWidth, nHeight = cHeight;
                        FCPadding margin = control.Margin;
                        switch (m_layoutStyle) {
                            //自下而上
                            case FCLayoutStyle.BottomToTop: {
                                    if (i == 0) {
                                        top = padding.top + height;
                                    }
                                    int lWidth = 0;
                                    if (m_autoWrap) {
                                        lWidth = size.cx;
                                        int lTop = top - margin.top - cHeight - margin.bottom;
                                        if (lTop < padding.top) {
                                            left += cWidth + margin.left;
                                            top = height - padding.top;
                                        }
                                    }
                                    else {
                                        lWidth = width - margin.left - margin.right;
                                    }
                                    top -= cHeight + margin.bottom;
                                    nLeft = left + margin.left;
                                    nWidth = lWidth;
                                    nTop = top;
                                    break;
                                }
                            //从左向右
                            case FCLayoutStyle.LeftToRight: {
                                    int lHeight = 0;
                                    if (m_autoWrap) {
                                        lHeight = size.cy;
                                        int lRight = left + margin.left + cWidth + margin.right;
                                        if (lRight > width) {
                                            left = padding.left;
                                            top += cHeight + margin.top;
                                        }
                                    }
                                    else {
                                        lHeight = height - margin.top - margin.bottom;
                                    }
                                    left += margin.left;
                                    nLeft = left;
                                    nTop = top + margin.top;
                                    nHeight = lHeight;
                                    left += cWidth + margin.right;
                                    break;
                                }
                            //从右向左
                            case FCLayoutStyle.RightToLeft: {
                                    if (i == 0) {
                                        left = width - padding.left;
                                    }
                                    int lHeight = 0;
                                    if (m_autoWrap) {
                                        lHeight = size.cy;
                                        int lLeft = left - margin.left - cWidth - margin.right;
                                        if (lLeft < padding.left) {
                                            left = width - padding.left;
                                            top += cHeight + margin.top;
                                        }
                                    }
                                    else {
                                        lHeight = height - margin.top - margin.bottom;
                                    }
                                    left -= cWidth + margin.left;
                                    nLeft = left;
                                    nTop = top + margin.top;
                                    nHeight = lHeight;
                                    break;
                                }
                            //自上而下
                            case FCLayoutStyle.TopToBottom: {
                                    int lWidth = 0;
                                    if (m_autoWrap) {
                                        lWidth = size.cx;
                                        int lBottom = top + margin.top + cHeight + margin.bottom;
                                        if (lBottom > height) {
                                            left += cWidth + margin.left + margin.right;
                                            top = padding.top;
                                        }
                                    }
                                    else {
                                        lWidth = width - margin.left - margin.right;
                                    }
                                    top += margin.top;
                                    nTop = top;
                                    nLeft = left + margin.left;
                                    nWidth = lWidth;
                                    top += cHeight + margin.bottom;
                                    break;
                                }
                        }
                        //设置区域
                        if (cLeft != nLeft || cTop != nTop || cWidth != nWidth || cHeight != nHeight) {
                            FCRect rect = new FCRect(nLeft, nTop, nLeft + nWidth, nTop + nHeight);
                            control.Bounds = rect;
                            reset = true;
                        }
                    }
                }
            }
            return reset;
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        public override void setProperty(String name, String value) {
            if (name == "autowrap") {
                AutoWrap = FCStr.convertStrToBool(value);
            }
            else if (name == "layoutstyle") {
                LayoutStyle = FCStr.convertStrToLayoutStyle(value);
            }
            else {
                base.setProperty(name, value);
            }
        }

        /// <summary>
        /// 布局更新方法
        /// </summary>
        public override void update() {
            onResetLayout();
            int controlsSize = m_controls.size();
            for (int i = 0; i < controlsSize; i++) {
                m_controls.get(i).update();
            }
            updateScrollBar();
        }
    }
}
