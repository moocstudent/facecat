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
    /// 组控件
    /// </summary>
    public class FCGroupBox : FCDiv {
        /// <summary>
        /// 创建层控件
        /// </summary>
        public FCGroupBox() {
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <returns>控件类型</returns>
        public override String getControlType() {
            return "GroupBox";
        }

        /// <summary>
        /// 重绘边线方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void onPaintBorder(FCPaint paint, FCRect clipRect) {
            FCFont font = Font;
            int width = Width, height = Height;
            String text = Text;
            FCSize tSize = new FCSize();
            if (text.Length > 0) {
                tSize = paint.textSize(text, font);
            }
            else {
                tSize = paint.textSize("0", font);
                tSize.cx = 0;
            }
            //绘制边线
            FCPoint[] points = new FCPoint[6];
            int tMid = tSize.cy / 2;
            int padding = 2;
            points[0] = new FCPoint(10, tMid);
            points[1] = new FCPoint(padding, tMid);
            points[2] = new FCPoint(padding, height - padding);
            points[3] = new FCPoint(width - padding, height - padding);
            points[4] = new FCPoint(width - padding, tMid);
            points[5] = new FCPoint(14 + tSize.cx, tMid);
            paint.drawPolyline(getPaintingBorderColor(), 1, 0, points);
            callPaintEvents(FCEventID.PAINTBORDER, paint, clipRect);
        }

        /// <summary>
        /// 重绘前景方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void onPaintForeground(FCPaint paint, FCRect clipRect) {
            String text = Text;
            if (text.Length > 0) {
                FCFont font = Font;
                FCSize tSize = paint.textSize(text, font);
                FCRect tRect = new FCRect(12, 0, 12 + tSize.cx, tSize.cy);
                paint.drawText(text, getPaintingTextColor(), font, tRect);
            }
        }
    }
}