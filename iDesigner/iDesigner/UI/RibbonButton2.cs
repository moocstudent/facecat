/*基于捂脸猫FaceCat框架 v1.0
 捂脸猫创始人-矿洞程序员-脉脉KOL-陶德 (微信号:suade1984);
 */

using System;
using System.Collections.Generic;
using System.Text;
using FaceCat;

namespace FaceCat
{
    /// <summary>
    /// 透明按钮
    /// </summary>
    public class RibbonButton2 : FCButton
    {
        /// <summary>
        /// 创建按钮
        /// </summary>
        public RibbonButton2()
        {
            BorderColor = FCColor.None;
            TextColor = FCDraw.FCCOLORS_TEXTCOLOR4;
            Font = new FCFont("微软雅黑", 12, false, false, false);
        }

        /// <summary>
        /// 获取正在绘制的背景色
        /// </summary>
        /// <returns></returns>
        protected override long getPaintingBackColor()
        {
            if (Native.PushedControl == this)
            {
                return FCColor.reverse(null, FCDraw.FCCOLORS_BACKCOLOR8);
            }
            else if (Native.HoveredControl == this)
            {
                return FCColor.ratioColor(null, FCDraw.FCCOLORS_BACKCOLOR8, 0.95);
            }
            else
            {
                return FCDraw.FCCOLORS_BACKCOLOR8;
            }
        }

        /// <summary>
        /// 重绘背景方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void onPaintBackground(FCPaint paint, FCRect clipRect)
        {
            int width = Width - 1, height = Height - 1;
            FCRect drawRect = new FCRect(0, 0, width, height);
            paint.fillRoundRect(getPaintingBackColor(), drawRect, 4);
        }
    }
}
