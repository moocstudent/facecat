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
using System.Text;

namespace FaceCat {
    /// <summary>
    /// 窗体控件边界
    /// </summary>
    public class FCWindowFrame : FCView {
        /// <summary>
        /// 创建窗体控件
        /// </summary>
        public FCWindowFrame() {
            BackColor = FCColor.None;
            BorderColor = FCColor.None;
            Dock = FCDockStyle.Fill;
        }

        /// <summary>
        /// 是否包含坐标
        /// </summary>
        /// <param name="point">坐标</param>
        /// <returns>是否包含</returns>
        public override bool containsPoint(FCPoint point) {
            ArrayList<FCView> controls = m_controls;
            int controlsSize = controls.size();
            for (int i = 0; i < controlsSize; i++) {
                FCWindow window = controls.get(i) as FCWindow;
                if (window != null && window.Frame == this) {
                    if (window.IsDialog) {
                        return true;
                    }
                    else {
                        return window.containsPoint(point);
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <returns>控件类型</returns>
        public override String getControlType() {
            return "WindowFrame";
        }

        /// <summary>
        /// 重绘方法
        /// </summary>
        public override void invalidate() {
            if (m_native != null) {
                ArrayList<FCView> controls = m_controls;
                int controlsSize = controls.size();
                for (int i = 0; i < controlsSize; i++) {
                    FCWindow window = controls.get(i) as FCWindow;
                    if (window != null) {
                        m_native.invalidate(window.getDynamicPaintRect());
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 绘制背景方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void onPaintBackground(FCPaint paint, FCRect clipRect) {
            base.onPaintBackground(paint, clipRect);
            if (paint.supportTransparent()) {
                ArrayList<FCView> controls = m_controls;
                int controlsSize = controls.size();
                for (int i = 0; i < controlsSize; i++) {
                    FCWindow window = controls.get(i) as FCWindow;
                    if (window != null) {
                        long shadowColor = window.ShadowColor;
                        int shadowSize = window.ShadowSize;
                        if (shadowColor != FCColor.None && shadowSize > 0 && window.IsDialog && window.Frame == this) {
                            FCRect bounds = window.Bounds;
                            FCRect leftShadow = new FCRect(bounds.left - shadowSize, bounds.top - shadowSize, bounds.left, bounds.bottom + shadowSize);
                            paint.fillRect(shadowColor, leftShadow);
                            FCRect rightShadow = new FCRect(bounds.right, bounds.top - shadowSize, bounds.right + shadowSize, bounds.bottom + shadowSize);
                            paint.fillRect(shadowColor, rightShadow);
                            FCRect topShadow = new FCRect(bounds.left, bounds.top - shadowSize, bounds.right, bounds.top);
                            paint.fillRect(shadowColor, topShadow);
                            FCRect bottomShadow = new FCRect(bounds.left, bounds.bottom, bounds.right, bounds.bottom + shadowSize);
                            paint.fillRect(shadowColor, bottomShadow);
                            break;
                        }
                    }
                }
            }
        }
    }
}
