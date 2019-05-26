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
    /// 复选框扩展
    /// </summary>
    public class CheckBoxM : FCCheckBox
    {
        /// <summary>
        /// 创建复选框
        /// </summary>
        public CheckBoxM()
        {
            ButtonSize = new FCSize(40, 14);
            BorderColor = FCColor.Border;
        }

        /// <summary>
        /// 重绘方法
        /// </summary>
        /// <param name="paint">绘图方法</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void onPaintCheckButton(FCPaint paint, FCRect clipRect)
        {
            bool isChecked = Checked;
            long backColor = FCDraw.FCCOLORS_BACKCOLOR4;
            if (isChecked)
            {
                backColor = FCDraw.FCCOLORS_BACKCOLOR9;
            }
            long backColor2 = FCDraw.FCCOLORS_TEXTCOLOR4;
            long borderColor = getPaintingBorderColor();
            paint.fillRoundRect(backColor, clipRect, 4);
            FCSize buttonSize = ButtonSize;
            if (isChecked)
            {
                FCRect pRect = new FCRect(clipRect.left + buttonSize.cx / 2 - 1, clipRect.top - 1, clipRect.right + 1, clipRect.bottom + 1);
                paint.fillRoundRect(backColor2, pRect, 4);
                paint.drawRoundRect(backColor, 1, 0, pRect, 4);
            }
            else
            {
                FCRect pRect = new FCRect(clipRect.left - 1, clipRect.top - 1, clipRect.left + buttonSize.cx / 2 + 1, clipRect.bottom + 1);
                paint.fillRoundRect(backColor2, pRect, 4);
                paint.drawRoundRect(backColor, 1, 0, pRect, 4);
            }
        }
    }
}
