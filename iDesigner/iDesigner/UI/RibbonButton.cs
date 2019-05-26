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
    public class RibbonButton : FCButton
    {
        /// <summary>
        /// 创建透明按钮
        /// </summary>
        public RibbonButton()
        {
            BackColor = FCColor.None;
            BorderColor = FCColor.None;
            Font = new FCFont("微软雅黑", 12, false, false, false);
        }

        private int m_angle = 90;

        /// <summary>
        /// 获取或设置渐变角度
        /// </summary>
        public int Angle
        {
            get { return m_angle; }
            set { m_angle = value; }
        }

        private int m_arrowType;

        /// <summary>
        /// 获取或设置箭头类型
        /// </summary>
        public int ArrowType
        {
            get { return m_arrowType; }
            set { m_arrowType = value; }
        }

        private bool m_isClose;

        /// <summary>
        /// 获取或设置是否是关闭按钮
        /// </summary>
        public bool IsClose
        {
            get { return m_isClose; }
            set { m_isClose = value; }
        }

        /// <summary>
        /// 获取或设置是否选中
        /// </summary>
        public bool Selected
        {
            get
            {
                FCView parent = Parent;
                if (parent != null)
                {
                    FCTabControl tabControl = parent as FCTabControl;
                    if (tabControl != null)
                    {
                        FCTabPage selectedTabPage = tabControl.SelectedTabPage;
                        if (selectedTabPage != null)
                        {
                            if (this == selectedTabPage.HeaderButton)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// 获取要绘制的背景图片
        /// </summary>
        /// <returns>背景图片</returns>
        protected override String getPaintingBackImage()
        {
            return FCDraw.getCloseButtonImage();
        }

        /// <summary>
        /// 获取要绘制的前景色
        /// </summary>
        /// <returns>前景色</returns>
        protected override long getPaintingTextColor()
        {
            if (Enabled)
            {
                if (Selected)
                {
                    return FCDraw.FCCOLORS_TEXTCOLOR4;
                }
                else
                {
                    return FCDraw.FCCOLORS_TEXTCOLOR;
                }
            }
            else
            {
                return FCDraw.FCCOLORS_TEXTCOLOR2;
            }
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
            int mw = width / 2;
            int mh = height / 2;
            FCRect drawRect = new FCRect(0, 0, width, height);
            if (m_isClose)
            {
                long lineColor = FCDraw.FCCOLORS_LINECOLOR;
                FCRect ellipseRect = new FCRect(1, 1, width - 2, height - 2);
                paint.fillEllipse(FCDraw.FCCOLORS_UPCOLOR, ellipseRect);
                paint.drawLine(lineColor, 2, 0, 4, 4, width - 7, height - 7);
                paint.drawLine(lineColor, 2, 0, 4, height - 7, width - 7, 3);
            }
            else
            {
                int cornerRadius = 3;
                if (m_arrowType > 0)
                {
                    cornerRadius = 0;
                }
                FCView parent = Parent;
                if (parent != null)
                {
                    FCTabControl tabControl = parent as FCTabControl;
                    if (tabControl != null)
                    {
                        cornerRadius = 0;
                    }
                }
                paint.fillGradientRect(FCDraw.FCCOLORS_BACKCOLOR, FCDraw.FCCOLORS_BACKCOLOR2, drawRect, cornerRadius, m_angle);
                paint.drawRoundRect(FCColor.Border, 1, 0, drawRect, cornerRadius);
            }
            if (m_arrowType > 0)
            {
                FCPoint[] points = new FCPoint[3];
                int dSize = Math.Min(width, height) / 4;
                switch (m_arrowType)
                {
                    //向左
                    case 1:
                        points[0] = new FCPoint(mw - dSize, mh);
                        points[1] = new FCPoint(mw + dSize, mh - dSize);
                        points[2] = new FCPoint(mw + dSize, mh + dSize);
                        break;
                    //向右
                    case 2:
                        points[0] = new FCPoint(mw + dSize, mh);
                        points[1] = new FCPoint(mw - dSize, mh - dSize);
                        points[2] = new FCPoint(mw - dSize, mh + dSize);
                        break;
                    //向上
                    case 3:
                        points[0] = new FCPoint(mw, mh - dSize);
                        points[1] = new FCPoint(mw - dSize, mh + dSize);
                        points[2] = new FCPoint(mw + dSize, mh + dSize);
                        break;
                    //向下
                    case 4:
                        points[0] = new FCPoint(mw, mh + dSize);
                        points[1] = new FCPoint(mw - dSize, mh - dSize);
                        points[2] = new FCPoint(mw + dSize, mh - dSize);
                        break;
                }
                paint.fillPolygon(FCDraw.FCCOLORS_TEXTCOLOR, points);
            }
            //绘制选中效果
            if (paint.supportTransparent())
            {
                if (Selected)
                {
                    paint.fillRect(FCDraw.FCCOLORS_BACKCOLOR8, drawRect);
                }
                else
                {
                    FCNative native = Native;
                    if (this == native.PushedControl)
                    {
                        paint.fillRect(FCDraw.FCCOLORS_BACKCOLOR4, drawRect);
                    }
		            else if (this == native.HoveredControl)
                    {
                        paint.fillRect(FCDraw.FCCOLORS_BACKCOLOR3, drawRect);
                    }
                }
            }
        }
    }
}
