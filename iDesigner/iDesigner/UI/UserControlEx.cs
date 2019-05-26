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
    /// 用户控件
    /// </summary>
    public class UserControlEx:FCDiv
    {
        private String m_cid = "";

        /// <summary>
        /// 获取或设置唯一ID
        /// </summary>
        public String Cid
        {
            get { return m_cid; }
            set { m_cid = value; }
        }

        private bool m_isContainer = true;

        /// <summary>
        /// 获取或设置是否容器
        /// </summary>
        public bool IsContainer
        {
            get { return m_isContainer; }
            set { m_isContainer = value; }
        }

        /// <summary>
        /// 重绘前景方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void onPaintForeground(FCPaint paint, FCRect clipRect)
        {
            base.onPaintForeground(paint, clipRect);
            int width = Width, height = Height;
            String cText = "UC:" + m_cid;
            int fSize = Math.Min(width, height) / 3;
            if (fSize > 40)
            {
                fSize = 40;
            }
            if (fSize > 3)
            {
                FCFont tfFont = new FCFont("SimSun", fSize, true, false, false);
                FCSize ftSize = paint.textSize(cText, tfFont);
                FCRect tfRect = new FCRect();
                tfRect.left = (width - ftSize.cx) / 2;
                tfRect.top = (height - ftSize.cy) / 2;
                tfRect.right = tfRect.left + ftSize.cx;
                tfRect.bottom = tfRect.top + ftSize.cy;
                paint.drawText(cText, FCDraw.FCCOLORS_TEXTCOLOR3, tfFont, tfRect);
            }
            String text = Text;
            FCFont font = Font;
            FCSize tSize = paint.textSize(text, font);
            FCRect tRect = new FCRect();
            if (m_cid == "windowex")
            {
                tRect.left = 2;
                tRect.top = 5;
            }
            else
            {
                tRect.left = (width - tSize.cx) / 2;
                tRect.top = (height - tSize.cy) / 2;
            }
            tRect.right = tRect.left + tSize.cx;
            tRect.bottom = tRect.top + tSize.cy;
            paint.drawText(text, getPaintingTextColor(), font, tRect);
        }
    }
}
