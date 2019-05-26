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
    /// 图片按钮
    /// </summary>
    public class ImageButton : FCButton
    {
        /// <summary>
        /// 创建工具箱按钮
        /// </summary>
        public ImageButton()
        {
            BackColor = FCColor.None;
            BorderColor = FCDraw.FCCOLORS_LINECOLOR2;
            Font = new FCFont("微软雅黑", 12, false, false, false);
        }

        /// <summary>
        /// 重绘背景
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void onPaintBackground(FCPaint paint, FCRect clipRect)
        {
            int width = Width;
            int height = Height;
            String text = Text;
            FCFont font = Font;
            FCSize tSize = paint.textSize(text, font);
            int drawWidth = tSize.cx + 20;
            FCRect drawRect = new FCRect(0, 0, width, height);
            FCNative native = Native;
            //绘制背景
            if (this == native.HoveredControl)
            {
                paint.fillGradientRect(FCDraw.FCCOLORS_BACKCOLOR5, FCDraw.FCCOLORS_BACKCOLOR6, drawRect, 2, 90);
            }
            //绘制图标
            String backImage = getPaintingBackImage();
            FCRect imageRect = new FCRect(2, (height - 16) / 2, 18, (height + 16) / 2);
            if (backImage != null && backImage.Length > 0)
            {
                paint.fillRect(getPaintingBackColor(), imageRect);
                paint.drawImage(getPaintingBackImage(), imageRect);
            }
            //绘制文字
            FCRect tRect = new FCRect();
            tRect.left = imageRect.right + 4;
            tRect.top = (height - tSize.cy) / 2;
            tRect.right = tRect.left + tSize.cx;
            tRect.bottom = tRect.top + tSize.cy;
            paint.drawText(text, getPaintingTextColor(), font, tRect);
            //绘制边线
            if (this == native.HoveredControl)
            {
                paint.drawRoundRect(getPaintingBorderColor(), 1, 0, drawRect, 2);
            }
            if (Enabled)
            {
                if (this == native.PushedControl)
                {
                    paint.fillRect(FCDraw.FCCOLORS_BACKCOLOR4, drawRect);
                }
                else if (this == native.HoveredControl)
                {
                    paint.fillRect(FCDraw.FCCOLORS_BACKCOLOR3, drawRect);
                }

            }
            else
            {
                paint.fillRect(FCDraw.FCCOLORS_BACKCOLOR7, drawRect);
            }
        }

        /// <summary>
        /// 绘制边线
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void onPaintBorder(FCPaint paint, FCRect clipRect)
        {
        }

        /// <summary>
        /// 绘制前景
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void onPaintForeground(FCPaint paint, FCRect clipRect)
        {
        }
    }
}
