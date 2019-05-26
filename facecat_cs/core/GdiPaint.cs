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
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing.Imaging;

namespace FaceCat {
    /// <summary>
    /// Gdi绘图类
    /// </summary>
    public class GdiPaint : FCPaint {
        /// <summary>
        /// 创建绘图类
        /// </summary>
        public GdiPaint() {
        }

        /// <summary>
        /// 位图
        /// </summary>
        protected Bitmap m_bitmap;

        /// <summary>
        /// 绘图对象
        /// </summary>
        protected Graphics m_g;

        /// <summary>
        /// 绘图句柄
        /// </summary>
        protected IntPtr m_hDC;

        /// <summary>
        /// 裁剪
        /// </summary>
        protected IntPtr m_hrgn;

        /// <summary>
        /// 图像缓存
        /// </summary>
        protected HashMap<String, Bitmap> m_images = new HashMap<String, Bitmap>();

        /// <summary>
        /// 横向偏移
        /// </summary>
        protected int m_offsetX;

        /// <summary>
        /// 纵向偏移
        /// </summary>
        protected int m_offsetY;

        /// <summary>
        /// 刷新的矩形
        /// </summary>
        protected FCRect m_pRect;

        /// <summary>
        /// 资源路径
        /// </summary>
        protected String m_resourcePath;

        /// <summary>
        /// 旋转角度
        /// </summary>
        protected int m_rotateAngle;

        /// <summary>
        /// 横向缩放因子
        /// </summary>
        protected double m_scaleFactorX = 1;

        /// <summary>
        /// 纵向缩放因子
        /// </summary>
        protected double m_scaleFactorY = 1;

        /// <summary>
        /// 控件的HDC
        /// </summary>
        protected IntPtr m_wndHdc;

        private const int FW_BOLD = 700;
        private const int FW_REGULAR = 400;
        private int GB2312_CHARSET = 134;
        private int OUT_DEFAULT_PRECIS = 0;
        private int CLIP_DEFAULT_PRECIS = 0;
        private int DEFAULT_QUALITY = 0;
        private int DEFAULT_PITCH = 0;
        private int FF_SWISS = 0;
        private const int DT_SINGLELINE = 0x20;
        private const int DT_TOP = 0;
        private const int DT_LEFT = 0;
        private const int DT_CENTER = 1;
        private const int DT_RIGHT = 2;
        private const int DT_VCENTER = 4;
        private const int DT_BOTTOM = 8;
        private const int DT_NOPREFIX = 0x00000800;
        private const int DT_WORD_ELLIPSIS = 0x00040000;
        private const int HOLLOW_BRUSH = 5;
        private const int IMAGE_BITMAP = 0;
        private const int LR_LOADFROMFILE = 0x00000010;
        private const int TRANSPARENT = 1;

        [DllImport("gdi32.dll")]
        private static extern bool BitBlt(
        IntPtr hdcDest,
        int nXDest,
        int nYDest,
        int nWidth,
        int nHeight,
        IntPtr hdcSrc,
        int nXSrc,
        int nYSrc,
        System.Int32 dwRop
        );

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateFont(int H, int W, int E, int O, int FW, int I, int u, int S, int C, int OP, int CP, int Q, int PAF, String F);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreatePen(int style, int width, int dwPenColorREF);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateRectRgnIndirect(ref FCRect rect);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateSolidBrush(int dwPenColorREF);

        [DllImport("gdi32.dll")]
        private static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int drawText(IntPtr hdc, String lpStr, int nCount, ref FCRect lpRect, int wFormat);

        [DllImport("gdi32.dll")]
        private static extern int Ellipse(IntPtr hdc, int left, int top, int right, int bottom);

        [DllImport("user32.dll")]
        private static extern int fillRect(IntPtr hdc, ref FCRect lpRect, IntPtr brush);

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetTextExtentPoint32(IntPtr hdc, String text, int length, ref FCSize size);

        [DllImport("gdi32.dll")]
        private static extern IntPtr GetStockObject(int fnObject);

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
        private static extern int LineTo(IntPtr hdc, int x, int y);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr LoadImage(IntPtr hinst, String lpszName, int uType, int cxDesired, int cyDesired, int fuLoad);

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
        private static extern int MoveToEx(IntPtr hdc, int x, int y, IntPtr lpPoint);

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
        private static extern int Polygon(IntPtr hdc, FCPoint[] apt, int cpt);

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
        private static extern int Polyline(IntPtr hdc, FCPoint[] apt, int cpt);

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
        private static extern int Rectangle(IntPtr hdc, int left, int top, int right, int bottom);

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
        private static extern int RoundRect(IntPtr hdc, int left, int top, int right, int bottom, int width, int height);

        [DllImport("gdi32.dll")]
        private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hObject);

        [DllImport("gdi32.dll")]
        private static extern int SetBkMode(IntPtr hdc, int nBkMode);

        [DllImport("gdi32.dll")]
        private static extern int SelectClipRgn(IntPtr hdc, IntPtr hrgn);

        [DllImport("gdi32.dll")]
        private static extern int SetTextColor(IntPtr hdc, int dwPenColorREF);

        [DllImport("gdi32.dll")]
        private static extern bool StretchBlt(
        IntPtr hdcDest,
        int nXDest,
        int nYDest,
        int nWidth,
        int nHeight,
        IntPtr hdcSrc,
        int nXSrc,
        int nYSrc,
        int nWSrc,
        int nHSrc,
        System.Int32 dwRop
        );

        /// <summary>
        /// 添加曲线
        /// </summary>
        /// <param name="rect">矩形区域</param>
        /// <param name="startAngle">从 x 轴到弧线的起始点沿顺时针方向度量的角（以度为单位）</param>
        /// <param name="sweepAngle">从 startAngle 参数到弧线的结束点沿顺时针方向度量的角（以度为单位）</param>
        public virtual void addArc(FCRect rect, float startAngle, float sweepAngle) {
        }

        /// <summary>
        /// 添加贝赛尔曲线
        /// </summary>
        /// <param name="points">点阵</param>
        public virtual void addBezier(FCPoint[] points) {
        }

        /// <summary>
        /// 添加曲线
        /// </summary>
        /// <param name="points">点阵</param>
        public virtual void addCurve(FCPoint[] points) {
        }

        /// <summary>
        /// 添加椭圆
        /// </summary>
        /// <param name="rect">矩形</param>
        public virtual void addEllipse(FCRect rect) {
        }

        /// <summary>
        /// 添加直线
        /// </summary>
        /// <param name="x1">第一个点的横坐标</param>
        /// <param name="y1">第一个点的纵坐标</param>
        /// <param name="x2">第二个点的横坐标</param>
        /// <param name="y2">第二个点的纵坐标</param>
        public virtual void addLine(int x1, int y1, int x2, int y2) {
        }

        /// <summary>
        /// 添加矩形
        /// </summary>
        /// <param name="rect">区域</param>
        public virtual void addRect(FCRect rect) {
        }

        /// <summary>
        /// 添加扇形
        /// </summary>
        /// <param name="rect">矩形区域</param>
        /// <param name="startAngle">从 x 轴到弧线的起始点沿顺时针方向度量的角（以度为单位）</param>
        /// <param name="sweepAngle">从 startAngle 参数到弧线的结束点沿顺时针方向度量的角（以度为单位）</param>
        public virtual void addPie(FCRect rect, float startAngle, float sweepAngle) {
        }

        /// <summary>
        /// 添加文字
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="font">字体</param>
        /// <param name="rect">区域</param>
        public virtual void addText(String text, FCFont font, FCRect rect) {
        }

        /// <summary>
        /// 缩放因子生效
        /// </summary>
        /// <param name="gdiplusRect">矩形</param>
        protected void affectScaleFactor(ref Rectangle gdiplusRect) {
            if (m_scaleFactorX != 1 || m_scaleFactorY != 1) {
                gdiplusRect.Location = new Point((int)(gdiplusRect.Left * m_scaleFactorX),
                    (int)(gdiplusRect.Top * m_scaleFactorY));
                gdiplusRect.Width = (int)(gdiplusRect.Width * m_scaleFactorX);
                gdiplusRect.Height = (int)(gdiplusRect.Height * m_scaleFactorY);
            }
        }

        /// <summary>
        /// 缩放因子生效
        /// </summary>
        /// <param name="rect">矩形</param>
        protected void affectScaleFactor(ref FCRect rect) {
            if (m_scaleFactorX != 1 || m_scaleFactorY != 1) {
                rect.left = (int)(rect.left * m_scaleFactorX);
                rect.top = (int)(rect.top * m_scaleFactorY);
                rect.right = (int)(rect.right * m_scaleFactorX);
                rect.bottom = (int)(rect.bottom * m_scaleFactorY);
            }
        }

        /// <summary>
        /// 开始导出
        /// </summary>
        /// <param name="exportPath">路径</param>
        /// <param name="rect">区域</param>
        public virtual void beginExport(String exportPath, FCRect rect) {
        }

        /// <summary>
        /// 开始绘图
        /// </summary>
        /// <param name="hdc">HDC</param>
        /// <param name="wRect">窗体区域</param>
        /// <param name="pRect">刷新区域</param>
        public virtual void beginPaint(IntPtr hdc, FCRect wRect, FCRect pRect) {
            m_pRect = pRect;
            int imageW = wRect.right - wRect.left;
            int imageH = wRect.bottom - wRect.top;
            if (imageW == 0) imageW = 1;
            if (imageH == 0) imageH = 1;
            m_bitmap = new Bitmap(imageW, imageH);
            m_g = Graphics.FromImage(m_bitmap);
            m_hDC = m_g.GetHdc();
            m_resourcePath = null;
            m_wndHdc = hdc;
            FCRect rc = new FCRect(-1, -1, 1, 1);
            FCFont font = new FCFont();
            drawText(" ", 0, font, rc);
        }

        /// <summary>
        /// 开始一段路径
        /// </summary>
        public virtual void beginPath() {
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        public virtual void clearCaches() {
            foreach (Image image in m_images.Values) {
                image.Dispose();
            }
            m_images.clear();
        }

        /// <summary>
        /// 裁剪路径
        /// </summary>
        public virtual void clipPath() {
        }

        /// <summary>
        /// 闭合路径
        /// </summary>
        public virtual void closeFigure() {
        }

        /// <summary>
        /// 结束一段路径
        /// </summary>
        public virtual void closePath() {
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        public virtual void delete() {
            if (m_hDC != IntPtr.Zero) {
                DeleteDC(m_hDC);
                m_hDC = IntPtr.Zero;
            }
            if (m_hrgn != IntPtr.Zero) {
                DeleteObject(m_hrgn);
                m_hrgn = IntPtr.Zero;
            }
            if (m_wndHdc != IntPtr.Zero) {
                DeleteObject(m_wndHdc);
                m_wndHdc = IntPtr.Zero;
            }
            if (m_g != null) {
                m_g.Dispose();
            }
            if (m_bitmap != null) {
                m_bitmap.Dispose();
            }
            clearCaches();
        }

        /// <summary>
        /// 绘制弧线
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="rect">矩形区域</param>
        /// <param name="startAngle">从 x 轴到弧线的起始点沿顺时针方向度量的角（以度为单位）</param>
        /// <param name="sweepAngle">从 startAngle 参数到弧线的结束点沿顺时针方向度量的角（以度为单位）</param>
        public virtual void drawArc(long dwPenColor, float width, int style, FCRect rect, float startAngle, float sweepAngle) {
        }

        /// <summary>
        /// 设置贝赛尔曲线
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="point1">坐标1</param>
        /// <param name="point2">坐标2</param>
        /// <param name="point3">坐标3</param>
        /// <param name="point4">坐标4</param>
        public virtual void drawBezier(long dwPenColor, float width, int style, FCPoint[] points) {
        }

        /// <summary>
        /// 绘制曲线
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="points">坐标阵</param>
        public virtual void drawCurve(long dwPenColor, float width, int style, FCPoint[] points) {
        }

        /// <summary>
        /// 绘制矩形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="rect">矩形区域</param>
        public virtual void drawEllipse(long dwPenColor, float width, int style, FCRect rect) {
            drawEllipse(dwPenColor, width, style, rect.left, rect.top, rect.right, rect.bottom);
        }

        /// <summary>
        /// 绘制矩形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="left">左侧坐标</param>
        /// <param name="top">顶部左标</param>
        /// <param name="right">右侧坐标</param>
        /// <param name="bottom">底部坐标</param>
        public virtual void drawEllipse(long dwPenColor, float width, int style, int left, int top, int right, int bottom) {
            if (dwPenColor == FCColor.None) return;
            dwPenColor = getPaintColor(dwPenColor);
            if (dwPenColor < 0) dwPenColor = Math.Abs(dwPenColor) / 1000;
            IntPtr hPen = CreatePen((int)dwPenColor, (int)width, style);
            IntPtr hOldPen = SelectObject(m_hDC, hPen);
            SelectObject(m_hDC, GetStockObject(HOLLOW_BRUSH));
            FCRect newRect = new FCRect(left + m_offsetX, top + m_offsetY, right + m_offsetX, bottom + m_offsetY);
            affectScaleFactor(ref newRect);
            Ellipse(m_hDC, newRect.left, newRect.top, newRect.right, newRect.bottom);
            SelectObject(m_hDC, hOldPen);
            DeleteObject(hPen);
        }

        /// <summary>
        /// 绘制图片
        /// </summary>
        /// <param name="imagePath">图片路径</param>
        /// <param name="rect">绘制区域</param>
        public virtual void drawImage(String imagePath, FCRect rect) {
            String imageKey = m_resourcePath + imagePath;
            Bitmap drawImage = null;
            int rw = rect.right - rect.left;
            if (rw < 1) rw = 1;
            int rh = rect.bottom - rect.top;
            if (rh < 1) rh = 1;
            Rectangle gdiPlusRect = new Rectangle(rect.left + m_offsetX, rect.top + m_offsetY, rw, rh);
            if (m_images.containsKey(imageKey)) {
                drawImage = m_images[imageKey];
            }
            else {
                String[] attributes = new String[] { "file", "corner", "source", "highcolor", "lowcolor" };
                String[] values = new String[5];
                values[0] = imagePath;
                if (imagePath.IndexOf("=") != -1) {
                    for (int i = 0; i < attributes.Length; i++) {
                        String attribute = attributes[i];
                        int alength = attribute.Length + 2;
                        int pos = imagePath.IndexOf(attribute + "=\'");
                        if (pos >= 0) {
                            int rpos = imagePath.IndexOf("\'", pos + alength);
                            values[i] = imagePath.Substring(pos + alength, rpos - pos - alength);
                        }
                    }
                }
                String file = values[0];
                if (!File.Exists(file)) {
                    if (m_resourcePath != null && m_resourcePath.Length > 0) {
                        if (m_resourcePath.EndsWith("\\")) {
                            file = m_resourcePath + file;
                        }
                        else {
                            file = m_resourcePath + "\\" + file;
                        }
                    }
                }
                if (File.Exists(file)) {
                    Bitmap image = null;
                    if (values[2] != null) {
                        int[] source = new int[4];
                        String[] strs = values[2].Split(',');
                        for (int i = 0; i < strs.Length; i++) {
                            source[i] = FCStr.convertStrToInt(strs[i]);
                        }
                        Rectangle gdiPlusSourceRect = new Rectangle(source[0], source[1], source[2] - source[0], source[3] - source[1]);
                        Bitmap sourceImage = (Bitmap)Bitmap.FromFile(file);
                        if (sourceImage != null) {
                            image = new Bitmap(gdiPlusSourceRect.Width, gdiPlusSourceRect.Height);
                            Graphics sg = Graphics.FromImage(image);
                            sg.DrawImage(sourceImage, new Rectangle(0, 0, gdiPlusSourceRect.Width, gdiPlusSourceRect.Height), gdiPlusSourceRect, GraphicsUnit.Pixel);
                            sg.Dispose();
                            sourceImage.Dispose();
                        }
                    }
                    else {
                        image = (Bitmap)Bitmap.FromFile(file);
                    }
                    if (image != null) {
                        long highColor = FCColor.None, lowColor = FCColor.None;
                        if (values[3] != null && values[4] != null) {
                            highColor = FCStr.convertStrToColor(values[3]);
                            lowColor = FCStr.convertStrToColor(values[4]);
                        }
                        ImageAttributes imageAttr = new ImageAttributes();
                        if (highColor != FCColor.None && lowColor != FCColor.None) {
                            int A = 0, R = 0, G = 0, B = 0;
                            FCColor.toArgb(this, highColor, ref A, ref R, ref G, ref B);
                            Color gdiPlusHighColor = Color.FromArgb(A, R, G, B);
                            FCColor.toArgb(this, lowColor, ref A, ref R, ref G, ref B);
                            Color gdiPlusLowColor = Color.FromArgb(A, R, G, B);
                            imageAttr.SetColorKey(gdiPlusLowColor, gdiPlusHighColor);
                        }
                        int iw = image.Width, ih = image.Height;
                        drawImage = new Bitmap(iw, ih);
                        Graphics g = Graphics.FromImage(drawImage);
                        if (values[1] == null) {
                            g.DrawImage(image, new Rectangle(0, 0, iw, ih),
                                0, 0, iw, ih, GraphicsUnit.Pixel, imageAttr);
                        }
                        else {
                            int[] corners = new int[4];
                            String[] strs = values[1].Split(',');
                            for (int i = 0; i < strs.Length; i++) {
                                corners[i] = FCStr.convertStrToInt(strs[i]);
                            }
                            int left = 0;
                            int top = 0;
                            int right = gdiPlusRect.Width;
                            int bottom = gdiPlusRect.Height;
                            if (corners[0] > 0) {
                                g.DrawImage(image, new Rectangle(left, top, corners[0], ih),
                                    0, 0, corners[0], ih, GraphicsUnit.Pixel, imageAttr);
                            }
                            if (corners[1] > 0) {
                                g.DrawImage(image, new Rectangle(left, top, iw, corners[1]),
                                0, 0, iw, corners[1], GraphicsUnit.Pixel, imageAttr);
                            }
                            if (corners[2] > 0) {
                                g.DrawImage(image, new Rectangle(right - corners[2], top, corners[2], ih),
                                    iw - corners[2], 0, corners[2], ih, GraphicsUnit.Pixel, imageAttr);
                            }
                            if (corners[3] > 0) {
                                g.DrawImage(image, new Rectangle(left, bottom - corners[3], iw, corners[3]),
                                    0, ih - corners[3], iw, corners[3], GraphicsUnit.Pixel, imageAttr);
                            }
                            int cwdest = iw - corners[0] - corners[2];
                            int chdest = ih - corners[1] - corners[3];
                            int cwsrc = iw - corners[0] - corners[2];
                            int chsrc = ih - corners[1] - corners[3];
                            if (cwdest > 0 && chdest > 0 && cwsrc > 0 && chsrc > 0) {
                                g.DrawImage(image, new Rectangle(left + corners[0], top + corners[1], cwdest, chdest),
                                    corners[0], corners[1], cwsrc, chsrc, GraphicsUnit.Pixel, imageAttr);
                            }
                        }
                        imageAttr.Dispose();
                        g.Dispose();
                        image.Dispose();
                        m_images.put(imageKey, drawImage);
                    }
                }
            }
            if (drawImage != null) {
                IntPtr bitmap = (drawImage as Bitmap).GetHbitmap();
                IntPtr hdcsource = CreateCompatibleDC(m_hDC);
                SelectObject(hdcsource, bitmap);
                int left = rect.left + m_offsetX;
                int top = rect.top + m_offsetY;
                int width = rw;
                int height = rh;
                if (m_scaleFactorX != 1 || m_scaleFactorY != 1) {
                    left = (int)(m_scaleFactorX * left);
                    top = (int)(m_scaleFactorY * top);
                    width = (int)(m_scaleFactorX * width);
                    height = (int)(m_scaleFactorY * height);
                }
                StretchBlt(m_hDC, left, top, width, height, hdcsource, 0, 0, drawImage.Width, drawImage.Height, 13369376);
                DeleteObject(bitmap);
                DeleteObject(hdcsource);
            }
        }

        /// <summary>
        /// 绘制直线
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="x1">第一个点的横坐标</param>
        /// <param name="y1">第一个点的纵坐标</param>
        /// <param name="x2">第二个点的横坐标</param>
        /// <param name="y2">第二个点的纵坐标</param>
        public virtual void drawLine(long dwPenColor, float width, int style, int x1, int y1, int x2, int y2) {
            if (dwPenColor == FCColor.None) return;
            dwPenColor = getPaintColor(dwPenColor);
            if (dwPenColor < 0) dwPenColor = Math.Abs(dwPenColor) / 1000;
            IntPtr hPen = CreatePen(style, (int)width, (int)dwPenColor);
            IntPtr hOldPen = SelectObject(m_hDC, hPen);
            int lx1 = x1 + m_offsetX;
            int ly1 = y1 + m_offsetY;
            int lx2 = x2 + m_offsetX;
            int ly2 = y2 + m_offsetY;
            if (m_scaleFactorX != 1 || m_scaleFactorY != 1) {
                lx1 = (int)(m_scaleFactorX * lx1);
                ly1 = (int)(m_scaleFactorY * ly1);
                lx2 = (int)(m_scaleFactorX * lx2);
                ly2 = (int)(m_scaleFactorY * ly2);
            }
            MoveToEx(m_hDC, lx1, ly1, IntPtr.Zero);
            LineTo(m_hDC, lx2, ly2);
            SelectObject(m_hDC, hOldPen);
            DeleteObject(hPen);
        }

        /// <summary>
        /// 绘制直线
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="x">第一个点的坐标</param>
        /// <param name="y">第二个点的坐标</param>
        public virtual void drawLine(long dwPenColor, float width, int style, FCPoint x, FCPoint y) {
            drawLine(dwPenColor, width, style, x.x, x.y, y.x, y.y);
        }

        /// <summary>
        /// 绘制路径
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        public virtual void drawPath(long dwPenColor, float width, int style) {
        }

        /// <summary>
        /// 绘制扇形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="rect">矩形区域</param>
        /// <param name="startAngle">从 x 轴到弧线的起始点沿顺时针方向度量的角（以度为单位）</param>
        /// <param name="sweepAngle">从 startAngle 参数到弧线的结束点沿顺时针方向度量的角（以度为单位）</param>
        public virtual void drawPie(long dwPenColor, float width, int style, FCRect rect, float startAngle, float sweepAngle) {
        }

        /// <summary>
        /// 绘制多边形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="points">点的数组</param>
        public virtual void drawPolygon(long dwPenColor, float width, int style, FCPoint[] points) {
            if (dwPenColor == FCColor.None) return;
            dwPenColor = getPaintColor(dwPenColor);
            if (dwPenColor < 0) dwPenColor = Math.Abs(dwPenColor) / 1000;
            FCPoint[] newPoints = new FCPoint[points.Length];
            for (int i = 0; i < points.Length; i++) {
                int x = points[i].x + m_offsetX;
                int y = points[i].y + m_offsetY;
                if (m_scaleFactorX != 1 || m_scaleFactorY != 1) {
                    x = (int)(m_scaleFactorX * x);
                    y = (int)(m_scaleFactorY * y);
                }
                FCPoint newPoint = new FCPoint(x, y);
                newPoints[i] = newPoint;
            }
            IntPtr hPen = CreatePen(style, (int)width, (int)dwPenColor);
            IntPtr hOldPen = SelectObject(m_hDC, hPen);
            Polygon(m_hDC, newPoints, newPoints.Length);
            SelectObject(m_hDC, hOldPen);
            DeleteObject(hPen);
        }

        /// <summary>
        /// 绘制大量直线
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="points">点集</param>
        public virtual void drawPolyline(long dwPenColor, float width, int style, FCPoint[] points) {
            if (dwPenColor == FCColor.None) return;
            dwPenColor = getPaintColor(dwPenColor);
            if (dwPenColor < 0) dwPenColor = Math.Abs(dwPenColor) / 1000;
            FCPoint[] newPoints = new FCPoint[points.Length];
            for (int i = 0; i < points.Length; i++) {
                int x = points[i].x + m_offsetX;
                int y = points[i].y + m_offsetY;
                if (m_scaleFactorX != 1 || m_scaleFactorY != 1) {
                    x = (int)(m_scaleFactorX * x);
                    y = (int)(m_scaleFactorY * y);
                }
                FCPoint newPoint = new FCPoint(x, y);
                newPoints[i] = newPoint;
            }
            IntPtr hPen = CreatePen(style, (int)width, (int)dwPenColor);
            IntPtr hOldPen = SelectObject(m_hDC, hPen);
            Polyline(m_hDC, newPoints, newPoints.Length);
            SelectObject(m_hDC, hOldPen);
            DeleteObject(hPen);
        }

        /// <summary>
        /// 绘制矩形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="rect">矩形区域</param>
        public virtual void drawRect(long dwPenColor, float width, int style, FCRect rect) {
            drawRect(dwPenColor, width, style, rect.left, rect.top, rect.right, rect.bottom);
        }

        /// <summary>
        /// 绘制矩形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="left">左侧坐标</param>
        /// <param name="top">顶部左标</param>
        /// <param name="right">右侧坐标</param>
        /// <param name="bottom">底部坐标</param>
        public virtual void drawRect(long dwPenColor, float width, int style, int left, int top, int right, int bottom) {
            if (dwPenColor == FCColor.None) return;
            dwPenColor = getPaintColor(dwPenColor);
            if (dwPenColor < 0) dwPenColor = Math.Abs(dwPenColor) / 1000;
            IntPtr hPen = CreatePen(style, (int)width, (int)dwPenColor);
            IntPtr hOldPen = SelectObject(m_hDC, hPen);
            SelectObject(m_hDC, GetStockObject(HOLLOW_BRUSH));
            FCRect newRect = new FCRect(left + m_offsetX, top + m_offsetY, right + m_offsetX, bottom + m_offsetY);
            affectScaleFactor(ref newRect);
            Rectangle(m_hDC, newRect.left, newRect.top, newRect.right, newRect.bottom);
            SelectObject(m_hDC, hOldPen);
            DeleteObject(hPen);
        }

        /// <summary>
        /// 绘制圆角矩形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="rect">矩形区域</param>
        /// <param name="cornerRadius">边角半径</param>
        public virtual void drawRoundRect(long dwPenColor, float width, int style, FCRect rect, int cornerRadius) {
            if (dwPenColor == FCColor.None) return;
            dwPenColor = getPaintColor(dwPenColor);
            if (dwPenColor < 0) dwPenColor = Math.Abs(dwPenColor) / 1000;
            IntPtr hPen = CreatePen(style, (int)width, (int)dwPenColor);
            IntPtr hOldPen = SelectObject(m_hDC, hPen);
            SelectObject(m_hDC, GetStockObject(HOLLOW_BRUSH));
            FCRect newRect = new FCRect(rect.left + m_offsetX, rect.top + m_offsetY, rect.right + m_offsetX, rect.bottom + m_offsetY);
            affectScaleFactor(ref newRect);
            if (cornerRadius != 0) {
                RoundRect(m_hDC, newRect.left, newRect.top, newRect.right, newRect.bottom, cornerRadius, cornerRadius);
            }
            else {
                Rectangle(m_hDC, newRect.left, newRect.top, newRect.right, newRect.bottom);
            }
            SelectObject(m_hDC, hOldPen);
            DeleteObject(hPen);
        }

        /// <summary>
        /// 绘制文字
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="font">字体</param>
        /// <param name="rect">矩形区域</param>
        public virtual void drawText(String text, long dwPenColor, FCFont font, FCRect rect) {
            if (dwPenColor == FCColor.None) return;
            dwPenColor = getPaintColor(dwPenColor);
            if (dwPenColor < 0) dwPenColor = Math.Abs(dwPenColor) / 1000;
            int fontSize = (int)font.m_fontSize;
            if (m_scaleFactorX != 1 || m_scaleFactorY != 1) {
                fontSize = (int)(fontSize * (m_scaleFactorX + m_scaleFactorY) / 2);
            }
            IntPtr hFont = CreateFont
            (
                fontSize, 0,
                0, 0,
                font.m_bold ? FW_BOLD : FW_REGULAR,
                font.m_italic ? 1 : 0,
                font.m_underline ? 1 : 0,
                0,
                GB2312_CHARSET,
                OUT_DEFAULT_PRECIS,
                CLIP_DEFAULT_PRECIS,
                DEFAULT_QUALITY,
                DEFAULT_PITCH | FF_SWISS,
                font.m_fontFamily
            );
            FCRect newRect = new FCRect(rect.left + m_offsetX, rect.top + m_offsetY, rect.right + m_offsetX, rect.bottom + m_offsetY);
            affectScaleFactor(ref newRect);
            SetBkMode(m_hDC, TRANSPARENT);
            SetTextColor(m_hDC, (int)dwPenColor);
            IntPtr hOldFont = SelectObject(m_hDC, hFont);
            drawText(m_hDC, text, -1, ref newRect, 0 | DT_NOPREFIX);
            SelectObject(m_hDC, hOldFont);
            DeleteObject(hFont);
        }

        /// <summary>
        /// 绘制文字
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="font">字体</param>
        /// <param name="rect">矩形区域</param>
        public virtual void drawText(String text, long dwPenColor, FCFont font, FCRectF rect) {
            drawText(text, dwPenColor, font, new FCRect(rect.left, rect.top, rect.right, rect.bottom));
        }

        /// <summary>
        /// 绘制自动省略结尾的文字
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="font">字体</param>
        /// <param name="rect">矩形区域</param>
        public virtual void drawTextAutoEllipsis(String text, long dwPenColor, FCFont font, FCRect rect) {
            if (dwPenColor == FCColor.None) return;
            dwPenColor = getPaintColor(dwPenColor);
            if (dwPenColor < 0) dwPenColor = Math.Abs(dwPenColor) / 1000;
            int fontSize = (int)font.m_fontSize;
            if (m_scaleFactorX != 1 || m_scaleFactorY != 1) {
                fontSize = (int)(fontSize * (m_scaleFactorX + m_scaleFactorY) / 2);
            }
            IntPtr hFont = CreateFont
            (
                fontSize, 0,
                0, 0,
                font.m_bold ? FW_BOLD : FW_REGULAR,
                font.m_italic ? 1 : 0,
                font.m_underline ? 1 : 0,
                0,
                GB2312_CHARSET,
                OUT_DEFAULT_PRECIS,
                CLIP_DEFAULT_PRECIS,
                DEFAULT_QUALITY,
                DEFAULT_PITCH | FF_SWISS,
                font.m_fontFamily
            );
            FCRect newRect = new FCRect(rect.left + m_offsetX, rect.top + m_offsetY, rect.right + m_offsetX, rect.bottom + m_offsetY);
            affectScaleFactor(ref newRect);
            SetBkMode(m_hDC, TRANSPARENT);
            SetTextColor(m_hDC, (int)dwPenColor);
            IntPtr hOldFont = SelectObject(m_hDC, hFont);
            drawText(m_hDC, text, -1, ref newRect, 0 | DT_NOPREFIX | DT_WORD_ELLIPSIS);
            SelectObject(m_hDC, hOldFont);
            DeleteObject(hFont);
        }

        /// <summary>
        /// 结束导出
        /// </summary>
        public virtual void endExport() {
        }

        /// <summary>
        /// 结束绘图
        /// </summary>
        public virtual void endPaint() {
            int left = m_pRect.left;
            int top = m_pRect.top;
            int width = m_pRect.right - m_pRect.left;
            int height = m_pRect.bottom - m_pRect.top;
            if (m_scaleFactorX != 1 || m_scaleFactorY != 1) {
                left = (int)(m_scaleFactorX * left);
                top = (int)(m_scaleFactorY * top);
                width = (int)(m_scaleFactorX * width);
                height = (int)(m_scaleFactorY * height);
            }
            BitBlt(m_wndHdc, left, top, width, height, m_hDC, left, top, 13369376);
            if (m_hDC != IntPtr.Zero) {
                DeleteDC(m_hDC);
                m_hDC = IntPtr.Zero;
            }
            if (m_hrgn != IntPtr.Zero) {
                DeleteObject(m_hrgn);
                m_hrgn = IntPtr.Zero;
            }
            if (m_wndHdc != IntPtr.Zero) {
                DeleteObject(m_wndHdc);
                m_wndHdc = IntPtr.Zero;
            }
            if (m_g != null) {
                m_g.Dispose();
            }
            if (m_bitmap != null) {
                m_bitmap.Dispose();
            }
            m_offsetX = 0;
            m_offsetY = 0;
            m_resourcePath = null;
        }

        /// <summary>
        /// 反裁剪路径
        /// </summary>
        public virtual void excludeClipPath() {
        }

        /// <summary>
        /// 填充椭圆
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="rect">矩形区域</param>
        public virtual void fillEllipse(long dwPenColor, FCRect rect) {
            fillEllipse(dwPenColor, rect.left, rect.top, rect.right, rect.bottom);
        }

        /// <summary>
        /// 填充椭圆
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="left">左侧坐标</param>
        /// <param name="top">顶部左标</param>
        /// <param name="right">右侧坐标</param>
        /// <param name="bottom">底部坐标</param>
        public virtual void fillEllipse(long dwPenColor, int left, int top, int right, int bottom) {
            if (dwPenColor == FCColor.None) return;
            dwPenColor = getPaintColor(dwPenColor);
            if (dwPenColor < 0) dwPenColor = Math.Abs(dwPenColor) / 1000;
            IntPtr brush = CreateSolidBrush((int)dwPenColor);
            SelectObject(m_hDC, brush);
            FCRect newRect = new FCRect(left + m_offsetX, top + m_offsetY, right + m_offsetX, bottom + m_offsetY);
            affectScaleFactor(ref newRect);
            Ellipse(m_hDC, newRect.left, newRect.top, newRect.right, newRect.bottom);
            DeleteObject(brush);
        }


        /// <summary>
        /// 绘制渐变椭圆
        /// </summary>
        /// <param name="dwFirst">开始颜色</param>
        /// <param name="dwSecond">结束颜色</param>
        /// <param name="rect">矩形</param>
        /// <param name="angle">角度</param>
        public virtual void fillGradientEllipse(long dwFirst, long dwSecond, FCRect rect, int angle) {
            fillEllipse(dwFirst, rect.left, rect.top, rect.right, rect.bottom);
        }

        /// <summary>
        /// 填充渐变路径
        /// </summary>
        /// <param name="dwFirst">开始颜色</param>
        /// <param name="dwSecond">结束颜色</param>
        /// <param name="points">点的集合</param>
        /// <param name="angle">角度</param>
        public virtual void fillGradientPath(long dwFirst, long dwSecond, FCRect rect, int angle) {
        }

        /// <summary>
        /// 绘制渐变的多边形
        /// </summary>
        /// <param name="dwFirst">开始颜色</param>
        /// <param name="dwSecond">结束颜色</param>
        /// <param name="rect">点的集合</param>
        /// <param name="angle">角度</param>
        public virtual void fillGradientPolygon(long dwFirst, long dwSecond, FCPoint[] points, int angle) {
            fillPolygon(dwFirst, points);
        }

        /// <summary>
        /// 绘制渐变矩形
        /// </summary>
        /// <param name="dwFirst">开始颜色</param>
        /// <param name="dwSecond">结束颜色</param>
        /// <param name="rect">矩形</param>
        /// <param name="cornerRadius">圆角半径</param>
        /// <param name="angle">角度</param>
        public virtual void fillGradientRect(long dwFirst, long dwSecond, FCRect rect, int cornerRadius, int angle) {
            if (cornerRadius != 0) {
                fillRoundRect(dwFirst, rect, cornerRadius);
            }
            else {
                fillRect(dwFirst, rect);
            }
        }

        /// <summary>
        /// 填充路径
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        public virtual void fillPath(long dwPenColor) {
        }

        /// <summary>
        /// 绘制扇形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="rect">矩形区域</param>
        /// <param name="startAngle">从 x 轴到弧线的起始点沿顺时针方向度量的角（以度为单位）</param>
        /// <param name="sweepAngle">从 startAngle 参数到弧线的结束点沿顺时针方向度量的角（以度为单位）</param>
        public virtual void fillPie(long dwPenColor, FCRect rect, float startAngle, float sweepAngle) {
        }

        /// <summary>
        /// 填充多边形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="points">点的数组</param>
        public virtual void fillPolygon(long dwPenColor, FCPoint[] points) {
            if (dwPenColor == FCColor.None) return;
            dwPenColor = getPaintColor(dwPenColor);
            if (dwPenColor < 0) dwPenColor = Math.Abs(dwPenColor) / 1000;
            FCPoint[] newPoints = new FCPoint[points.Length];
            for (int i = 0; i < points.Length; i++) {
                int x = points[i].x + m_offsetX;
                int y = points[i].y + m_offsetY;
                if (m_scaleFactorX != 1 || m_scaleFactorY != 1) {
                    x = (int)(m_scaleFactorX * x);
                    y = (int)(m_scaleFactorY * y);
                }
                FCPoint newPoint = new FCPoint(x, y);
                newPoints[i] = newPoint;
            }
            IntPtr brush = CreateSolidBrush((int)dwPenColor);
            SelectObject(m_hDC, brush);
            Polygon(m_hDC, newPoints, newPoints.Length);
            DeleteObject(brush);
        }

        /// <summary>
        /// 填充矩形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="rect">矩形区域</param>
        public virtual void fillRect(long dwPenColor, FCRect rect) {
            if (dwPenColor == FCColor.None) return;
            dwPenColor = getPaintColor(dwPenColor);
            if (dwPenColor < 0) dwPenColor = Math.Abs(dwPenColor) / 1000;
            FCRect newRect = new FCRect(rect.left + m_offsetX, rect.top + m_offsetY, rect.right + m_offsetX, rect.bottom + m_offsetY);
            affectScaleFactor(ref newRect);
            IntPtr brush = CreateSolidBrush((int)dwPenColor);
            SelectObject(m_hDC, brush);
            fillRect(m_hDC, ref newRect, brush);
            DeleteObject(brush);
        }

        /// <summary>
        /// 填充矩形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="left">左侧坐标</param>
        /// <param name="top">顶部左标</param>
        /// <param name="right">右侧坐标</param>
        /// <param name="bottom">底部坐标</param>
        public virtual void fillRect(long dwPenColor, int left, int top, int right, int bottom) {
            fillRect(dwPenColor, new FCRect(left, top, right, bottom));
        }

        /// <summary>
        /// 填充圆角矩形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="rect">矩形区域</param>
        /// <param name="cornerRadius">边角半径</param>
        public virtual void fillRoundRect(long dwPenColor, FCRect rect, int cornerRadius) {
            if (dwPenColor == FCColor.None) return;
            dwPenColor = getPaintColor(dwPenColor);
            if (dwPenColor < 0) dwPenColor = Math.Abs(dwPenColor) / 1000;
            FCRect newRect = new FCRect(rect.left + m_offsetX, rect.top + m_offsetY, rect.right + m_offsetX, rect.bottom + m_offsetY);
            affectScaleFactor(ref newRect);
            IntPtr brush = CreateSolidBrush((int)dwPenColor);
            SelectObject(m_hDC, brush);
            if (cornerRadius != 0) {
                RoundRect(m_hDC, newRect.left, newRect.top, newRect.right, newRect.bottom, cornerRadius, cornerRadius);
            }
            else {
                fillRect(m_hDC, ref newRect, brush);
            }
            DeleteObject(brush);
        }

        /// <summary>
        /// 获取颜色
        /// </summary>
        /// <param name="dwPenColor">输入颜色</param>
        /// <returns>输出颜色</returns>
        public virtual long getColor(long dwPenColor) {
            if (dwPenColor < FCColor.None) {
                if (dwPenColor == FCColor.Back) {
                    dwPenColor = 16777215;
                }
                else if (dwPenColor == FCColor.Border) {
                    dwPenColor = 3289650;
                }
                else if (dwPenColor == FCColor.Text) {
                    dwPenColor = 0;
                }
                else if (dwPenColor == FCColor.DisabledBack) {
                    dwPenColor = 13158600;
                }
                else if (dwPenColor == FCColor.DisabledText) {
                    dwPenColor = 3289650;
                }
                else if (dwPenColor == FCColor.Hovered) {
                    dwPenColor = 13158600;
                }
                else if (dwPenColor == FCColor.Pushed) {
                    dwPenColor = 9868950;
                }
            }
            return dwPenColor;
        }

        /// <summary>
        /// 获取要绘制的颜色
        /// </summary>
        /// <param name="dwPenColor">输入颜色</param>
        /// <returns>输出颜色</returns>
        public virtual long getPaintColor(long dwPenColor) {
            return getColor(dwPenColor);
        }

        /// <summary>
        /// 获取偏移
        /// </summary>
        /// <returns>偏移坐标</returns>
        public virtual FCPoint getOffset() {
            return new FCPoint(m_offsetX, m_offsetY);
        }

        /// <summary>
        /// 旋转角度
        /// </summary>
        /// <param name="op">圆心坐标</param>
        /// <param name="mp">点的坐标</param>
        /// <param name="angle">角度</param>
        /// <returns>结果坐标</returns>
        public virtual FCPoint rotate(FCPoint op, FCPoint mp, int angle) {
            float PI = 3.14159265f;
            FCPoint pt = new FCPoint();
            pt.x = (int)((mp.x - op.x) * Math.Cos(angle * PI / 180) - (mp.y - op.y) * Math.Sin(angle * PI / 180) + op.x);
            pt.y = (int)((mp.x - op.x) * Math.Sin(angle * PI / 180) + (mp.y - op.y) * Math.Cos(angle * PI / 180) + op.y);
            return pt;
        }

        /// <summary>
        /// 设置裁剪区域
        /// </summary>
        /// <param name="rect">区域</param>
        public virtual void setClip(FCRect rect) {
            if (m_hrgn != IntPtr.Zero) {
                DeleteObject(m_hrgn);
            }
            FCRect newRect = new FCRect(rect.left + m_offsetX, rect.top + m_offsetY, rect.right + m_offsetX, rect.bottom + m_offsetY);
            if (m_scaleFactorX != 1 || m_scaleFactorY != 1) {
                newRect.left = (int)Math.Floor(newRect.left * m_scaleFactorX);
                newRect.top = (int)Math.Floor(newRect.top * m_scaleFactorY);
                newRect.right = (int)Math.Ceiling(newRect.right * m_scaleFactorX);
                newRect.bottom = (int)Math.Ceiling(newRect.bottom * m_scaleFactorY);
            }
            m_hrgn = CreateRectRgnIndirect(ref newRect);
            SelectClipRgn(m_hDC, m_hrgn);
        }

        /// <summary>
        /// 设置直线两端的样式
        /// </summary>
        /// <param name="startLineCap">开始的样式</param>
        /// <param name="endLineCap">结束的样式</param>
        public virtual void setLineCap(int startLineCap, int endLineCap) {
        }

        /// <summary>
        /// 设置偏移
        /// </summary>
        /// <param name="mp">偏移坐标</param>
        public virtual void setOffset(FCPoint mp) {
            m_offsetX = mp.x;
            m_offsetY = mp.y;
        }

        /// <summary>
        /// 设置透明度
        /// </summary>
        /// <param name="opacity">透明度</param>
        public virtual void setOpacity(float opacity) {
        }

        /// <summary>
        /// 设置缩放因子
        /// </summary>
        /// <param name="scaleFactorX">横向因子</param>
        /// <param name="scaleFactorY">纵向因子</param>
        public virtual void setScaleFactor(double scaleFactorX, double scaleFactorY) {
            m_scaleFactorX = scaleFactorX;
            m_scaleFactorY = scaleFactorY;
        }

        /// <summary>
        /// 设置资源的路径
        /// </summary>
        /// <param name="resourcePath">资源的路径</param>
        public virtual void setResourcePath(String resourcePath) {
            m_resourcePath = resourcePath;
        }

        /// <summary>
        /// 设置旋转角度
        /// </summary>
        /// <param name="rotateAngle">旋转角度</param>
        public virtual void setRotateAngle(int rotateAngle) {
            m_rotateAngle = rotateAngle;
        }

        /// <summary>
        /// 设置平滑模式
        /// </summary>
        /// <param name="smoothMode">平滑模式</param>
        public virtual void setSmoothMode(int smoothMode) {
        }

        /// <summary>
        /// 设置文字的质量
        /// </summary>
        /// <param name="textQuality">文字质量</param>
        public virtual void setTextQuality(int textQuality) {
        }

        /// <summary>
        /// 设置是否支持透明色
        /// </summary>
        /// <returns>是否支持</returns>
        public virtual bool supportTransparent() {
            return false;
        }

        /// <summary>
        /// 获取文字大小
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="font">字体</param>
        /// <returns>字体大小</returns>
        public virtual FCSize textSize(String text, FCFont font) {
            FCSize size = new FCSize();
            if (text != null) {
                IntPtr hFont = CreateFont
                (
                    (int)font.m_fontSize, 0,
                    0, 0,
                    font.m_bold ? FW_BOLD : FW_REGULAR,
                    font.m_italic ? 1 : 0,
                    font.m_underline ? 1 : 0,
                    0,
                    GB2312_CHARSET,
                    OUT_DEFAULT_PRECIS,
                    CLIP_DEFAULT_PRECIS,
                    DEFAULT_QUALITY,
                    DEFAULT_PITCH | FF_SWISS,
                    font.m_fontFamily
                );
                IntPtr hOldFont = SelectObject(m_hDC, hFont);
                GetTextExtentPoint32(m_hDC, text, text.Length, ref size);
                SelectObject(m_hDC, hOldFont);
                DeleteObject(hFont);
            }
            return size;
        }

        /// <summary>
        /// 获取文字大小
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="font">字体</param>
        /// <returns>字体大小</returns>
        public virtual FCSizeF textSizeF(String text, FCFont font) {
            FCSize size = textSize(text, font);
            return new FCSizeF(size.cx, size.cy);
        }
    }
}