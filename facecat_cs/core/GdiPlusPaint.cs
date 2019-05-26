/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace FaceCat {
    /// <summary>
    /// Gdi+绘图类
    /// </summary>
    public class GdiPlusPaint : FCPaint {
        /// <summary>
        /// 创建绘图类
        /// </summary>
        public GdiPlusPaint() {
        }

        /// <summary>
        /// 位图
        /// </summary>
        protected Bitmap m_bitmap;

        /// <summary>
        /// 画刷
        /// </summary>
        protected SolidBrush m_brush;

        /// <summary>
        /// 画刷的颜色
        /// </summary>
        protected long m_brushColor;

        /// <summary>
        /// 空的字符串格式
        /// </summary>
        protected StringFormat m_emptyStringFormat;

        /// <summary>
        /// 直线结束样式
        /// </summary>
        protected int m_endLineCap;

        /// <summary>
        /// 导出路径
        /// </summary>
        protected String m_exportPath;

        /// <summary>
        /// 绘图画面
        /// </summary>
        protected Graphics m_g;

        /// <summary>
        /// 控件的HDC
        /// </summary>
        protected IntPtr m_hDC;

        /// <summary>
        /// 图像缓存
        /// </summary>
        protected HashMap<String, Bitmap> m_images = new HashMap<String, Bitmap>();

        /// <summary>
        /// 图像矩阵
        /// </summary>
        protected float[][] m_matrixItems = { new float[] { 1, 0, 0, 0, 0 }, new float[] { 0, 1, 0, 0, 0 }, new float[] { 0, 0, 1, 0, 0 }, new float[] { 0, 0, 0, 1, 0 }, new float[] { 0, 0, 0, 0, 1 } };

        /// <summary>
        /// 横向偏移
        /// </summary>
        protected int m_offsetX;

        /// <summary>
        /// 纵向偏移
        /// </summary>
        protected int m_offsetY;

        /// <summary>
        /// 透明度
        /// </summary>
        protected float m_opacity = 1;

        /// <summary>
        /// 绘图路径
        /// </summary>
        protected GraphicsPath m_path;

        /// <summary>
        /// 画笔
        /// </summary>
        protected Pen m_pen;

        /// <summary>
        ///  画笔的颜色 
        /// </summary>
        protected long m_penColor;

        /// <summary>
        /// 画笔的宽度 
        /// </summary>
        protected float m_penWidth;

        /// <summary>
        ///  画笔的样式 
        /// </summary>
        protected int m_penStyle;

        /// <summary>
        /// 刷新区域
        /// </summary>
        private FCRect m_pRect;

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
        /// 平滑程度
        /// </summary>
        protected int m_smoothMode = 2;

        /// <summary>
        /// 直线开始样式
        /// </summary>
        protected int m_startLineCap;

        /// <summary>
        /// 文字的质量
        /// </summary>
        protected int m_textQuality = 3;

        /// <summary>
        /// 窗口大小
        /// </summary>
        private FCRect m_wRect;

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

        /// <summary>
        /// 位图对象
        /// </summary>
        public struct BITMAP {
            public int bmType; // set to 0
            public int bmWidth; // width in pixels
            public int bmHeight; // height in pixels
            public int bmWidthBytes; // width of row in bytes
            public short bmPlanes; // number of color planes
            public short bmBitsPixel; // number of bits per pixel
            public IntPtr bmBits; // pointer to pixel bits
        }

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
        private static extern IntPtr CreateBitmapIndirect(ref BITMAP bm);

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
            int rw = rect.right - rect.left - 1;
            if (rw < 1) rw = 1;
            int rh = rect.bottom - rect.top - 1;
            if (rh < 1) rh = 1;
            Rectangle gdiPlusRect = new Rectangle(rect.left + m_offsetX, rect.top + m_offsetY, rw, rh);
            affectScaleFactor(ref gdiPlusRect);
            m_path.AddArc(gdiPlusRect, startAngle, sweepAngle);
        }

        /// <summary>
        /// 添加贝赛尔曲线
        /// </summary>
        /// <param name="points">点阵</param>
        public virtual void addBezier(FCPoint[] points) {
            Point[] gdiPlusPoints = new Point[points.Length];
            for (int i = 0; i < gdiPlusPoints.Length; i++) {
                int x = points[i].x + m_offsetX;
                int y = points[i].y + m_offsetY;
                if (m_scaleFactorX != 1 || m_scaleFactorY != 1) {
                    x = (int)(m_scaleFactorX * x);
                    y = (int)(m_scaleFactorY * y);
                }
                Point p = new Point(x, y);
                gdiPlusPoints[i] = p;
            }
            m_path.AddBezier(gdiPlusPoints[0], gdiPlusPoints[1], gdiPlusPoints[2], gdiPlusPoints[3]);
        }

        /// <summary>
        /// 添加曲线
        /// </summary>
        /// <param name="points">点阵</param>
        public virtual void addCurve(FCPoint[] points) {
            Point[] gdiPlusPoints = new Point[points.Length];
            for (int i = 0; i < gdiPlusPoints.Length; i++) {
                int x = points[i].x + m_offsetX;
                int y = points[i].y + m_offsetY;
                if (m_scaleFactorX != 1 || m_scaleFactorY != 1) {
                    x = (int)(m_scaleFactorX * x);
                    y = (int)(m_scaleFactorY * y);
                }
                Point p = new Point(x, y);
                gdiPlusPoints[i] = p;
            }
            m_path.AddCurve(gdiPlusPoints);
        }

        /// <summary>
        /// 添加椭圆
        /// </summary>
        /// <param name="rect">矩形</param>
        public virtual void addEllipse(FCRect rect) {
            int rw = rect.right - rect.left - 1;
            if (rw < 1) rw = 1;
            int rh = rect.bottom - rect.top - 1;
            if (rh < 1) rh = 1;
            Rectangle gdiPlusRect = new Rectangle(rect.left + m_offsetX, rect.top + m_offsetY, rw, rh);
            affectScaleFactor(ref gdiPlusRect);
            m_path.AddEllipse(gdiPlusRect);
        }

        /// <summary>
        /// 添加直线
        /// </summary>
        /// <param name="x1">第一个点的横坐标</param>
        /// <param name="y1">第一个点的纵坐标</param>
        /// <param name="x2">第二个点的横坐标</param>
        /// <param name="y2">第二个点的纵坐标</param>
        public virtual void addLine(int x1, int y1, int x2, int y2) {
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
            m_path.AddLine(lx1, ly1, lx2, ly2);
        }

        /// <summary>
        /// 添加矩形
        /// </summary>
        /// <param name="rect">区域</param>
        public virtual void addRect(FCRect rect) {
            int rw = rect.right - rect.left - 1;
            if (rw < 1) rw = 1;
            int rh = rect.bottom - rect.top - 1;
            if (rh < 1) rh = 1;
            Rectangle gdiPlusRect = new Rectangle(rect.left + m_offsetX, rect.top + m_offsetY, rw, rh);
            affectScaleFactor(ref gdiPlusRect);
            m_path.AddRectangle(gdiPlusRect);
        }

        /// <summary>
        /// 添加扇形
        /// </summary>
        /// <param name="rect">矩形区域</param>
        /// <param name="startAngle">从 x 轴到弧线的起始点沿顺时针方向度量的角（以度为单位）</param>
        /// <param name="sweepAngle">从 startAngle 参数到弧线的结束点沿顺时针方向度量的角（以度为单位）</param>
        public virtual void addPie(FCRect rect, float startAngle, float sweepAngle) {
            int rw = rect.right - rect.left - 1;
            if (rw < 1) rw = 1;
            int rh = rect.bottom - rect.top - 1;
            if (rh < 1) rh = 1;
            Rectangle gdiPlusRect = new Rectangle(rect.left + m_offsetX, rect.top + m_offsetY, rw, rh);
            affectScaleFactor(ref gdiPlusRect);
            m_path.AddPie(gdiPlusRect, startAngle, sweepAngle);
        }

        /// <summary>
        /// 添加文字
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="font">字体</param>
        /// <param name="rect">区域</param>
        public virtual void addText(String text, FCFont font, FCRect rect) {
            if (m_emptyStringFormat == null) {
                m_emptyStringFormat = StringFormat.GenericTypographic;
                m_emptyStringFormat.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
            }
            if (m_scaleFactorX != 1 || m_scaleFactorY != 1) {
                int strX = (int)(m_scaleFactorX * (rect.left + m_offsetX));
                int strY = (int)(m_scaleFactorY * (rect.top + m_offsetY));
                Point gdiPlusPoint = new Point(strX, strY);
                float fontSize = (float)(font.m_fontSize * (m_scaleFactorX + m_scaleFactorY) / 2);
                FCFont scaleFont = new FCFont(font.m_fontFamily, fontSize, font.m_bold, font.m_underline, font.m_italic);
                Font gdiplusFont = GetFont(scaleFont);
                m_path.AddString(text, gdiplusFont.FontFamily, (int)gdiplusFont.Style, gdiplusFont.Size, gdiPlusPoint, m_emptyStringFormat);
            }
            else {
                Point gdiPlusPoint = new Point(rect.left + m_offsetX, rect.top + m_offsetY);
                Font gdiplusFont = GetFont(font);
                m_path.AddString(text, gdiplusFont.FontFamily, (int)gdiplusFont.Style, gdiplusFont.Size, gdiPlusPoint, m_emptyStringFormat);
            }
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
        /// <param name="gdiplusRect">矩形</param>
        protected void affectScaleFactor(ref RectangleF gdiplusRect) {
            if (m_scaleFactorX != -1 || m_scaleFactorY != -1) {
                gdiplusRect.Location = new PointF((float)(gdiplusRect.Left * m_scaleFactorX),
                    (float)(gdiplusRect.Top * m_scaleFactorY));
                gdiplusRect.Width = (float)(gdiplusRect.Width * m_scaleFactorX);
                gdiplusRect.Height = (float)(gdiplusRect.Height * m_scaleFactorY);
            }
        }

        /// <summary>
        /// 开始导出
        /// </summary>
        /// <param name="exportPath">路径</param>
        /// <param name="rect">区域</param>
        public virtual void beginExport(String exportPath, FCRect rect) {
            m_exportPath = exportPath;
            int imageW = rect.right - rect.left;
            int imageH = rect.bottom - rect.top;
            if (imageW == 0) imageW = 1;
            if (imageH == 0) imageH = 1;
            if (m_g != null) {
                m_g.Dispose();
            }
            if (m_bitmap != null) {
                m_bitmap.Dispose();
            }
            m_bitmap = new Bitmap(imageW, imageH);
            m_g = Graphics.FromImage(m_bitmap);
            setSmoothMode(m_smoothMode);
            setTextQuality(m_textQuality);
            m_opacity = 1;
            m_resourcePath = null;
        }

        /// <summary>
        /// 开始绘图
        /// </summary>
        /// <param name="hdc">HDC</param>
        /// <param name="wRect">窗体区域</param>
        /// <param name="pRect">刷新区域</param>
        public virtual void beginPaint(IntPtr hdc, FCRect wRect, FCRect pRect) {
            m_pRect = pRect;
            m_wRect = wRect;
            int width = m_wRect.right - m_wRect.left;
            int height = m_wRect.bottom - m_wRect.top;
            if (m_bitmap == null || width > (int)m_bitmap.Width || height > (int)m_bitmap.Height) {
                if (m_g != null) {
                    m_g.Dispose();
                }
                int oldWidth = 0, oldHeight = 0;
                if (m_bitmap != null) {
                    oldWidth = m_bitmap.Width;
                    oldHeight = m_bitmap.Height;
                    m_bitmap.Dispose();
                }
                m_bitmap = new Bitmap(Math.Max(width, oldWidth), Math.Max(height, oldHeight));
                m_g = Graphics.FromImage(m_bitmap);
            }
            m_hDC = hdc;
            setSmoothMode(m_smoothMode);
            setTextQuality(m_textQuality);
            m_opacity = 1;
            m_resourcePath = null;
        }

        /// <summary>
        /// 开始一段路径
        /// </summary>
        public virtual void beginPath() {
            m_path = new GraphicsPath();
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        public virtual void clearCaches() {
            if (m_brush != null) {
                m_brush.Dispose();
                m_brush = null;
            }
            foreach (Image image in m_images.Values) {
                image.Dispose();
            }
            m_images.clear();
            if (m_pen != null) {
                m_pen.Dispose();
                m_pen = null;
            }
            if (m_emptyStringFormat != null) {
                m_emptyStringFormat.Dispose();
                m_emptyStringFormat = null;
            }
            if (m_path != null) {
                m_path.Dispose();
                m_path = null;
            }
        }

        /// <summary>
        /// 裁剪路径
        /// </summary>
        public virtual void clipPath() {
            m_g.SetClip(m_path);
        }

        /// <summary>
        /// 闭合路径
        /// </summary>
        public virtual void closeFigure() {
            m_path.CloseFigure();
        }

        /// <summary>
        /// 结束一段路径
        /// </summary>
        public virtual void closePath() {
            m_path.Dispose();
            m_path = null;
        }

        /// <summary>
        /// 创建位图
        /// </summary>
        /// <param name="bitmap_ptr">Bitmap图像</param>
        /// <param name="rect">区域</param>
        /// <returns>位图</returns>
        public IntPtr create_hbitmap_from_gdiplus_bitmap(Bitmap bitmap_ptr, Rectangle rect) {
            rect.Intersect(new Rectangle(0, 0, bitmap_ptr.Width, bitmap_ptr.Height));
            if (rect.Width > 0 && rect.Height > 0) {
                BITMAP bm;
                BitmapData bmpdata = bitmap_ptr.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                bm.bmType = 0;
                bm.bmWidth = bmpdata.Width;
                bm.bmHeight = bmpdata.Height;
                bm.bmWidthBytes = bmpdata.Stride;
                bm.bmPlanes = 1;
                bm.bmBitsPixel = 32;
                bm.bmBits = bmpdata.Scan0;
                IntPtr hBitmap = CreateBitmapIndirect(ref bm);
                bitmap_ptr.UnlockBits(bmpdata);
                return hBitmap;
            }
            else {
                return IntPtr.Zero;
            }
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        public virtual void delete() {
            if (m_g != null) {
                m_g.Dispose();
                m_g = null;
            }
            if (m_bitmap != null) {
                m_bitmap.Dispose();
                m_bitmap = null;
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
            if (dwPenColor == FCColor.None) return;
            int rw = rect.right - rect.left - 1;
            if (rw < 1) rw = 1;
            int rh = rect.bottom - rect.top - 1;
            if (rh < 1) rh = 1;
            Rectangle gdiPlusRect = new Rectangle(rect.left + m_offsetX, rect.top + m_offsetY, rw, rh);
            affectScaleFactor(ref gdiPlusRect);
            m_g.DrawArc(GetPen(dwPenColor, width, style), gdiPlusRect, startAngle, sweepAngle);
        }

        /// <summary>
        /// 设置贝赛尔曲线
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="points">坐标阵1</param>
        public void drawBezier(long dwPenColor, float width, int style, FCPoint[] points) {
            if (dwPenColor == FCColor.None) return;
            Point[] gdiPlusPoints = new Point[points.Length];
            for (int i = 0; i < gdiPlusPoints.Length; i++) {
                int x = points[i].x + m_offsetX;
                int y = points[i].y + m_offsetY;
                if (m_scaleFactorX != 1 || m_scaleFactorY != 1) {
                    x = (int)(m_scaleFactorX * x);
                    y = (int)(m_scaleFactorY * y);
                }
                Point p = new Point(x, y);
                gdiPlusPoints[i] = p;
            }
            m_g.DrawBezier(GetPen(dwPenColor, width, style), gdiPlusPoints[0], gdiPlusPoints[1], gdiPlusPoints[2], gdiPlusPoints[3]);
        }

        /// <summary>
        /// 绘制曲线
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="points">坐标阵</param>
        public virtual void drawCurve(long dwPenColor, float width, int style, FCPoint[] points) {
            if (dwPenColor == FCColor.None) return;
            Point[] gdiPlusPoints = new Point[points.Length];
            for (int i = 0; i < gdiPlusPoints.Length; i++) {
                int x = points[i].x + m_offsetX;
                int y = points[i].y + m_offsetY;
                if (m_scaleFactorX != 1 || m_scaleFactorY != 1) {
                    x = (int)(m_scaleFactorX * x);
                    y = (int)(m_scaleFactorY * y);
                }
                Point p = new Point(x, y);
                gdiPlusPoints[i] = p;
            }
            m_g.DrawCurve(GetPen(dwPenColor, width, style), gdiPlusPoints);
        }


        /// <summary>
        /// 绘制矩形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="rect">矩形区域</param>
        public virtual void drawEllipse(long dwPenColor, float width, int style, FCRect rect) {
            if (dwPenColor == FCColor.None) return;
            int rw = rect.right - rect.left - 1;
            if (rw < 1) rw = 1;
            int rh = rect.bottom - rect.top - 1;
            if (rh < 1) rh = 1;
            Rectangle gdiPlusRect = new Rectangle(rect.left + m_offsetX, rect.top + m_offsetY, rw, rh);
            affectScaleFactor(ref gdiPlusRect);
            m_g.DrawEllipse(GetPen(dwPenColor, width, style), gdiPlusRect);
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
            FCRect rect = new FCRect(left, top, right, bottom);
            drawEllipse(dwPenColor, width, style, rect);
        }

        /// <summary>
        /// 绘制图片
        /// </summary>
        /// <param name="imagePath">图片路径</param>
        /// <param name="rect">绘制区域</param>
        public virtual void drawImage(String imagePath, FCRect rect) {
            String imageKey = m_resourcePath + imagePath;
            int rw = rect.right - rect.left;
            if (rw < 1) rw = 1;
            int rh = rect.bottom - rect.top;
            if (rh < 1) rh = 1;
            Bitmap drawImage = null;
            Rectangle gdiPlusRect = new Rectangle(rect.left + m_offsetX, rect.top + m_offsetY, rw, rh);
            if (m_images.containsKey(imageKey)) {
                drawImage = m_images.get(imageKey);
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
                            int right = iw;
                            int bottom = ih;
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
                affectScaleFactor(ref gdiPlusRect);
                if (m_opacity < 1) {
                    m_matrixItems[3][3] = m_opacity;
                    ColorMatrix colorMatrix = new ColorMatrix(m_matrixItems);
                    ImageAttributes imageAttr = new ImageAttributes();
                    imageAttr.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                    m_g.DrawImage(drawImage, gdiPlusRect, 0, 0, drawImage.Width, drawImage.Height, GraphicsUnit.Pixel, imageAttr);
                    imageAttr.Dispose();
                }
                else {
                    m_matrixItems[3][3] = m_opacity;
                    m_g.DrawImage(drawImage, gdiPlusRect, 0, 0, drawImage.Width, drawImage.Height, GraphicsUnit.Pixel);
                }
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
            m_g.DrawLine(GetPen(dwPenColor, width, style), lx1, ly1, lx2, ly2);
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
            if (dwPenColor == FCColor.None) return;
            m_g.DrawPath(GetPen(dwPenColor, width, style), m_path);
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
            if (dwPenColor == FCColor.None) return;
            int rw = rect.right - rect.left - 1;
            if (rw < 1) rw = 1;
            int rh = rect.bottom - rect.top - 1;
            if (rh < 1) rh = 1;
            Rectangle gdiPlusRect = new Rectangle(rect.left + m_offsetX, rect.top + m_offsetY, rw, rh);
            affectScaleFactor(ref gdiPlusRect);
            m_g.DrawPie(GetPen(dwPenColor, width, style), gdiPlusRect, startAngle, sweepAngle);
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
            Point[] gdiPlusPoints = new Point[points.Length];
            for (int i = 0; i < gdiPlusPoints.Length; i++) {
                int x = points[i].x + m_offsetX;
                int y = points[i].y + m_offsetY;
                if (m_scaleFactorX != 1 || m_scaleFactorY != 1) {
                    x = (int)(m_scaleFactorX * x);
                    y = (int)(m_scaleFactorY * y);
                }
                Point p = new Point(x, y);
                gdiPlusPoints[i] = p;
            }
            m_g.DrawPolygon(GetPen(dwPenColor, width, style), gdiPlusPoints);
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
            Point[] gdiPlusPoints = new Point[points.Length];
            for (int i = 0; i < gdiPlusPoints.Length; i++) {
                int x = points[i].x + m_offsetX;
                int y = points[i].y + m_offsetY;
                if (m_scaleFactorX != 1 || m_scaleFactorY != 1) {
                    x = (int)(m_scaleFactorX * x);
                    y = (int)(m_scaleFactorY * y);
                }
                Point p = new Point(x, y);
                gdiPlusPoints[i] = p;
            }
            m_g.DrawLines(GetPen(dwPenColor, width, style), gdiPlusPoints);
        }

        /// <summary>
        /// 绘制矩形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="rect">矩形区域</param>
        public virtual void drawRect(long dwPenColor, float width, int style, FCRect rect) {
            if (dwPenColor == FCColor.None) return;
            int rw = rect.right - rect.left - 1;
            if (rw < 1) rw = 1;
            int rh = rect.bottom - rect.top - 1;
            if (rh < 1) rh = 1;
            Rectangle gdiPlusRect = new Rectangle(rect.left + m_offsetX, rect.top + m_offsetY, rw, rh);
            affectScaleFactor(ref gdiPlusRect);
            m_g.DrawRectangle(GetPen(dwPenColor, width, style), gdiPlusRect);
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
            FCRect rect = new FCRect(left, top, right, bottom);
            drawRect(dwPenColor, width, style, rect);
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
            int rw = rect.right - rect.left - 1;
            if (rw < 1) rw = 1;
            int rh = rect.bottom - rect.top - 1;
            if (rh < 1) rh = 1;
            Rectangle gdiPlusRect = new Rectangle(rect.left + m_offsetX, rect.top + m_offsetY, rw, rh);
            affectScaleFactor(ref gdiPlusRect);
            if (cornerRadius != 0) {
                GraphicsPath gdiPlusPath = GetRoundRectPath(gdiPlusRect, cornerRadius);
                m_g.DrawPath(GetPen(dwPenColor, width, style), gdiPlusPath);
                gdiPlusPath.Dispose();
            }
            else {
                m_g.DrawRectangle(GetPen(dwPenColor, width, style), gdiPlusRect);
            }
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
            if (m_emptyStringFormat == null) {
                m_emptyStringFormat = StringFormat.GenericDefault;
                m_emptyStringFormat.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
            }
            if (m_scaleFactorX != 1 || m_scaleFactorY != 1) {
                int strX = (int)(m_scaleFactorX * (rect.left + m_offsetX));
                int strY = (int)(m_scaleFactorY * (rect.top + m_offsetY));
                Point gdiPlusPoint = new Point(strX, strY);
                float fontSize = (float)(font.m_fontSize * (m_scaleFactorX + m_scaleFactorY) / 2);
                FCFont scaleFont = new FCFont(font.m_fontFamily, fontSize, font.m_bold, font.m_underline, font.m_italic);
                m_g.DrawString(text, GetFont(scaleFont), GetBrush(dwPenColor), gdiPlusPoint, m_emptyStringFormat);
            }
            else {
                Point gdiPlusPoint = new Point(rect.left + m_offsetX, rect.top + m_offsetY);
                m_g.DrawString(text, GetFont(font), GetBrush(dwPenColor), gdiPlusPoint, m_emptyStringFormat);
            }
        }

        /// <summary>
        /// 绘制文字
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="font">字体</param>
        /// <param name="rect">矩形区域</param>
        public virtual void drawText(String text, long dwPenColor, FCFont font, FCRectF rect) {
            if (dwPenColor == FCColor.None) return;
            if (m_emptyStringFormat == null) {
                m_emptyStringFormat = StringFormat.GenericDefault;
                m_emptyStringFormat.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
            }
            if (m_scaleFactorX != 1 || m_scaleFactorY != 1) {
                float strX = (float)(m_scaleFactorX * (rect.left + m_offsetX));
                float strY = (float)(m_scaleFactorY * (rect.top + m_offsetY));
                PointF gdiPlusPoint = new PointF(strX, strY);
                float fontSize = (float)(font.m_fontSize * (m_scaleFactorX + m_scaleFactorY) / 2);
                FCFont scaleFont = new FCFont(font.m_fontFamily, fontSize, font.m_bold, font.m_underline, font.m_italic);
                m_g.DrawString(text, GetFont(scaleFont), GetBrush(dwPenColor), gdiPlusPoint, m_emptyStringFormat);
            }
            else {
                PointF gdiPlusPoint = new PointF(rect.left + m_offsetX, rect.top + m_offsetY);
                m_g.DrawString(text, GetFont(font), GetBrush(dwPenColor), gdiPlusPoint, m_emptyStringFormat);
            }
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
            if (m_emptyStringFormat == null) {
                m_emptyStringFormat = StringFormat.GenericDefault;
                m_emptyStringFormat.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
            }
            int rw = rect.right - rect.left - 1;
            if (rw < 1) rw = 1;
            int rh = rect.bottom - rect.top - 1;
            if (rh < 1) rh = 1;
            Rectangle gdiPlusRect = new Rectangle(rect.left + m_offsetX, rect.top + m_offsetY, rw, rh);
            affectScaleFactor(ref gdiPlusRect);
            m_emptyStringFormat.Trimming = StringTrimming.EllipsisCharacter;
            if (m_scaleFactorX != 1 || m_scaleFactorY != 1) {
                affectScaleFactor(ref gdiPlusRect);
                float fontSize = (float)(font.m_fontSize * (m_scaleFactorX + m_scaleFactorY) / 2);
                FCFont scaleFont = new FCFont(font.m_fontFamily, fontSize, font.m_bold, font.m_underline, font.m_italic);
                m_g.DrawString(text, GetFont(scaleFont), GetBrush(dwPenColor), gdiPlusRect, m_emptyStringFormat);
            }
            else {
                m_g.DrawString(text, GetFont(font), GetBrush(dwPenColor), gdiPlusRect, m_emptyStringFormat);
            }
            m_emptyStringFormat.Trimming = StringTrimming.None;
        }

        /// <summary>
        /// 结束导出
        /// </summary>
        public virtual void endExport() {
            if (m_bitmap != null) {
                m_bitmap.Save(m_exportPath);
            }
            if (m_g != null) {
                m_g.Dispose();
                m_g = null;
            }
            if (m_bitmap != null) {
                m_bitmap.Dispose();
                m_bitmap = null;
            }
            m_offsetX = 0;
            m_offsetY = 0;
            m_opacity = 1;
            m_resourcePath = null;
        }

        /// <summary>
        /// 结束绘图
        /// </summary>
        public virtual void endPaint() {
            Rectangle clipRect = new Rectangle(m_pRect.left, m_pRect.top, m_pRect.right - m_pRect.left, m_pRect.bottom - m_pRect.top);
            affectScaleFactor(ref clipRect);
            int width = m_wRect.right - m_wRect.left;
            int height = m_wRect.bottom - m_wRect.top;
            if (clipRect.Width < width || clipRect.Height < height) {
                if (clipRect.X < m_wRect.left) {
                    clipRect.Width += clipRect.X;
                    clipRect.X = m_wRect.left;
                }
                if (clipRect.Y < m_wRect.top) {
                    clipRect.Height += clipRect.Y;
                    clipRect.Y = m_wRect.top;
                }
                if (clipRect.Right > m_wRect.right) {
                    clipRect.Width -= Math.Abs(clipRect.Right - m_wRect.right);
                }
                if (clipRect.Bottom > m_wRect.bottom) {
                    clipRect.Height -= Math.Abs(clipRect.Bottom - m_wRect.bottom);
                }
                if (clipRect.Width > 0 && clipRect.Height > 0) {
                    IntPtr bitmap = create_hbitmap_from_gdiplus_bitmap(m_bitmap, clipRect);
                    if (bitmap != IntPtr.Zero) {
                        IntPtr hdcsource = CreateCompatibleDC(m_hDC);
                        SelectObject(hdcsource, bitmap);
                        StretchBlt(m_hDC, clipRect.X, clipRect.Y, clipRect.Width, clipRect.Height, hdcsource, 0, 0, clipRect.Width, clipRect.Height, 13369376);
                        DeleteObject(bitmap);
                        DeleteObject(hdcsource);
                    }
                }

            }
            else {
                if (clipRect.Width > 0 && clipRect.Height > 0) {
                    IntPtr bitmap = create_hbitmap_from_gdiplus_bitmap(m_bitmap, clipRect);
                    if (bitmap != IntPtr.Zero) {
                        IntPtr hdcsource = CreateCompatibleDC(m_hDC);
                        SelectObject(hdcsource, bitmap);
                        StretchBlt(m_hDC, clipRect.X, clipRect.Y, clipRect.Width, clipRect.Height, hdcsource, 0, 0, width, height, 13369376);
                        DeleteObject(bitmap);
                        DeleteObject(hdcsource);
                    }
                }
            }
            m_offsetX = 0;
            m_offsetY = 0;
            m_opacity = 1;
            m_resourcePath = "";
        }

        /// <summary>
        /// 反裁剪路径
        /// </summary>
        public virtual void excludeClipPath() {
            Region region = new Region(m_path);
            m_g.ExcludeClip(region);
            region.Dispose();
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
            int rw = right - left;
            if (rw < 1) rw = 1;
            int rh = bottom - top;
            if (rh < 1) rh = 1;
            Rectangle gdiPlusRect = new Rectangle(left + m_offsetX, top + m_offsetY, rw, rh);
            affectScaleFactor(ref gdiPlusRect);
            m_g.FillEllipse(GetBrush(dwPenColor), gdiPlusRect);
        }

        /// <summary>
        /// 绘制渐变椭圆
        /// </summary>
        /// <param name="dwFirst">开始颜色</param>
        /// <param name="dwSecond">结束颜色</param>
        /// <param name="rect">矩形</param>
        /// <param name="angle">角度</param>
        public virtual void fillGradientEllipse(long dwFirst, long dwSecond, FCRect rect, int angle) {
            int rw = rect.right - rect.left;
            if (rw < 1) rw = 1;
            int rh = rect.bottom - rect.top;
            if (rh < 1) rh = 1;
            Rectangle gdiPlusRect = new Rectangle(rect.left + m_offsetX, rect.top + m_offsetY, rw, rh);
            affectScaleFactor(ref gdiPlusRect);
            LinearGradientBrush lgb = new LinearGradientBrush(gdiPlusRect, GetGdiPlusColor(dwFirst), GetGdiPlusColor(dwSecond), angle);
            m_g.FillEllipse(lgb, gdiPlusRect);
            lgb.Dispose();
        }

        /// <summary>
        /// 填充渐变路径
        /// </summary>
        /// <param name="dwFirst">开始颜色</param>
        /// <param name="dwSecond">结束颜色</param>
        /// <param name="points">点的集合</param>
        /// <param name="angle">角度</param>
        public virtual void fillGradientPath(long dwFirst, long dwSecond, FCRect rect, int angle) {
            int rw = rect.right - rect.left;
            if (rw < 1) rw = 1;
            int rh = rect.bottom - rect.top;
            if (rh < 1) rh = 1;
            Rectangle gdiPlusRect = new Rectangle(rect.left + m_offsetX, rect.top + m_offsetY, rw, rh);
            affectScaleFactor(ref gdiPlusRect);
            LinearGradientBrush lgb = new LinearGradientBrush(gdiPlusRect, GetGdiPlusColor(dwFirst), GetGdiPlusColor(dwSecond), angle);
            m_g.FillPath(lgb, m_path);
            lgb.Dispose();
        }

        /// <summary>
        /// 绘制渐变的多边形
        /// </summary>
        /// <param name="dwFirst">开始颜色</param>
        /// <param name="dwSecond">结束颜色</param>
        /// <param name="rect">点的集合</param>
        /// <param name="angle">角度</param>
        public virtual void fillGradientPolygon(long dwFirst, long dwSecond, FCPoint[] points, int angle) {
            int left = 0, top = 0, right = 0, bottom = 0;
            Point[] gdiPlusPoints = new Point[points.Length];
            for (int i = 0; i < gdiPlusPoints.Length; i++) {
                int x = points[i].x + m_offsetX;
                int y = points[i].y + m_offsetY;
                if (m_scaleFactorX != 1 || m_scaleFactorY != 1) {
                    x = (int)(m_scaleFactorX * x);
                    y = (int)(m_scaleFactorY * y);
                }
                if (i == 0) {
                    left = x;
                    top = y;
                    right = x;
                    bottom = y;
                }
                else {
                    if (x < left) {
                        left = x;
                    }
                    if (y < top) {
                        top = y;
                    }
                    if (x > right) {
                        right = x;
                    }
                    if (y > bottom) {
                        bottom = y;
                    }
                }
                Point p = new Point(x, y);
                gdiPlusPoints[i] = p;
            }
            Rectangle gdiPlusRect = new Rectangle(left, top, right - left, bottom - top);
            if (gdiPlusRect.Height > 0) {
                LinearGradientBrush lgb = new LinearGradientBrush(gdiPlusRect, GetGdiPlusColor(dwFirst), GetGdiPlusColor(dwSecond), angle);
                m_g.FillPolygon(lgb, gdiPlusPoints);
                lgb.Dispose();
            }
        }

        /// <summary>
        /// 绘制渐变矩形
        /// </summary>
        /// <param name="dwFirst">开始颜色</param>
        /// <param name="dwSecond">结束颜色</param>
        /// <param name="rect">矩形</param>
        /// <param name="cornerRadius">边角半径</param>
        /// <param name="angle">角度</param>
        public virtual void fillGradientRect(long dwFirst, long dwSecond, FCRect rect, int cornerRadius, int angle) {
            int rw = rect.right - rect.left;
            if (rw < 1) rw = 1;
            int rh = rect.bottom - rect.top;
            if (rh < 1) rh = 1;
            Rectangle gdiPlusRect = new Rectangle(rect.left + m_offsetX, rect.top + m_offsetY, rw, rh);
            affectScaleFactor(ref gdiPlusRect);
            LinearGradientBrush lgb = new LinearGradientBrush(gdiPlusRect, GetGdiPlusColor(dwFirst), GetGdiPlusColor(dwSecond), angle);
            if (cornerRadius != 0) {
                GraphicsPath gdiPlusPath = GetRoundRectPath(gdiPlusRect, cornerRadius);
                m_g.FillPath(lgb, gdiPlusPath);
                gdiPlusPath.Dispose();
            }
            else {
                m_g.FillRectangle(lgb, gdiPlusRect);
            }
            lgb.Dispose();
        }

        /// <summary>
        /// 填充路径
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        public virtual void fillPath(long dwPenColor) {
            if (dwPenColor == FCColor.None) return;
            m_g.FillPath(GetBrush(dwPenColor), m_path);
        }

        /// <summary>
        /// 绘制扇形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="rect">矩形区域</param>
        /// <param name="startAngle">从 x 轴到弧线的起始点沿顺时针方向度量的角（以度为单位）</param>
        /// <param name="sweepAngle">从 startAngle 参数到弧线的结束点沿顺时针方向度量的角（以度为单位）</param>
        public virtual void fillPie(long dwPenColor, FCRect rect, float startAngle, float sweepAngle) {
            if (dwPenColor == FCColor.None) return;
            int rw = rect.right - rect.left - 1;
            if (rw < 1) rw = 1;
            int rh = rect.bottom - rect.top - 1;
            if (rh < 1) rh = 1;
            Rectangle gdiPlusRect = new Rectangle(rect.left + m_offsetX, rect.top + m_offsetY, rw, rh);
            affectScaleFactor(ref gdiPlusRect);
            m_g.FillPie(GetBrush(dwPenColor), gdiPlusRect, startAngle, sweepAngle);
        }

        /// <summary>
        /// 填充多边形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="points">点的数组</param>
        public virtual void fillPolygon(long dwPenColor, FCPoint[] points) {
            if (dwPenColor == FCColor.None) return;
            Point[] gdiPlusPoints = new Point[points.Length];
            for (int i = 0; i < gdiPlusPoints.Length; i++) {
                int x = points[i].x + m_offsetX;
                int y = points[i].y + m_offsetY;
                if (m_scaleFactorX != 1 || m_scaleFactorY != 1) {
                    x = (int)(m_scaleFactorX * x);
                    y = (int)(m_scaleFactorY * y);
                }
                Point p = new Point(x, y);
                gdiPlusPoints[i] = p;
            }
            m_g.FillPolygon(GetBrush(dwPenColor), gdiPlusPoints);
        }

        /// <summary>
        /// 填充矩形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="rect">矩形区域</param>
        public virtual void fillRect(long dwPenColor, FCRect rect) {
            fillRect(dwPenColor, rect.left, rect.top, rect.right, rect.bottom);
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
            if (dwPenColor == FCColor.None) return;
            int rw = right - left;
            if (rw < 1) rw = 1;
            int rh = bottom - top;
            if (rh < 1) rh = 1;
            Rectangle gdiPlusRect = new Rectangle(left + m_offsetX, top + m_offsetY, rw, rh);
            affectScaleFactor(ref gdiPlusRect);
            m_g.FillRectangle(GetBrush(dwPenColor), gdiPlusRect);
        }

        /// <summary>
        /// 填充圆角矩形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="rect">矩形区域</param>
        /// <param name="cornerRadius">边角半径</param>
        public virtual void fillRoundRect(long dwPenColor, FCRect rect, int cornerRadius) {
            if (dwPenColor == FCColor.None) return;
            int rw = rect.right - rect.left - 1;
            if (rw < 1) rw = 1;
            int rh = rect.bottom - rect.top - 1;
            if (rh < 1) rh = 1;
            Rectangle gdiPlusRect = new Rectangle(rect.left + m_offsetX, rect.top + m_offsetY, rw, rh);
            affectScaleFactor(ref gdiPlusRect);
            if (cornerRadius != 0) {
                GraphicsPath gdiPlusPath = GetRoundRectPath(gdiPlusRect, cornerRadius);
                m_g.FillPath(GetBrush(dwPenColor), gdiPlusPath);
                gdiPlusPath.Dispose();
            }
            else {
                m_g.FillRectangle(GetBrush(dwPenColor), gdiPlusRect);
            }
        }

        /// <summary>
        /// 获取画刷
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <returns>画刷</returns>
        protected SolidBrush GetBrush(long dwPenColor) {
            Color gdiColor = GetGdiPlusColor(dwPenColor);
            if (m_brush == null) {
                m_brush = new SolidBrush(gdiColor);
                if (m_opacity == 1) {
                    m_brushColor = dwPenColor;
                }
                else {
                    m_brushColor = FCColor.None;
                }
            }
            else {
                if (m_brushColor == FCColor.None || m_brushColor != dwPenColor) {
                    m_brush.Color = gdiColor;
                    m_brushColor = dwPenColor;
                }
                if (m_opacity != 1) {
                    m_brushColor = FCColor.None;
                }
            }
            return m_brush;
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
        /// 获取颜色
        /// </summary>
        /// <param name="dwPenColor">整型颜色</param>
        /// <returns>Gdi颜色</returns>
        protected Color GetGdiPlusColor(long dwPenColor) {
            dwPenColor = getPaintColor(dwPenColor);
            int a = 0, r = 0, g = 0, b = 0;
            FCColor.toArgb(this, dwPenColor, ref a, ref r, ref g, ref b);
            Color gdiColor = Color.FromArgb(a, r, g, b);
            if (m_opacity < 1) {
                Color opacityColor = Color.FromArgb((int)(gdiColor.A * m_opacity), gdiColor);
                return opacityColor;
            }
            else {
                return gdiColor;
            }
        }

        /// <summary>
        /// 获取Gdi字体
        /// </summary>
        /// <param name="font">字体</param>
        /// <returns>Gdi字体</returns>
        protected Font GetFont(FCFont font) {
            if (font.m_strikeout) {
                if (font.m_bold && font.m_underline && font.m_italic) {
                    return new Font(font.m_fontFamily, font.m_fontSize, FontStyle.Bold | FontStyle.Underline | FontStyle.Italic | FontStyle.Strikeout, GraphicsUnit.Pixel);
                }
                else if (font.m_bold && font.m_underline) {
                    return new Font(font.m_fontFamily, font.m_fontSize, FontStyle.Bold | FontStyle.Underline | FontStyle.Strikeout, GraphicsUnit.Pixel);
                }
                else if (font.m_bold && font.m_italic) {
                    return new Font(font.m_fontFamily, font.m_fontSize, FontStyle.Bold | FontStyle.Italic | FontStyle.Strikeout, GraphicsUnit.Pixel);
                }
                else if (font.m_underline && font.m_italic) {
                    return new Font(font.m_fontFamily, font.m_fontSize, FontStyle.Underline | FontStyle.Italic | FontStyle.Strikeout, GraphicsUnit.Pixel);
                }
                else if (font.m_bold) {
                    return new Font(font.m_fontFamily, font.m_fontSize, FontStyle.Bold | FontStyle.Strikeout, GraphicsUnit.Pixel);
                }
                else if (font.m_underline) {
                    return new Font(font.m_fontFamily, font.m_fontSize, FontStyle.Underline | FontStyle.Strikeout, GraphicsUnit.Pixel);
                }
                else if (font.m_italic) {
                    return new Font(font.m_fontFamily, font.m_fontSize, FontStyle.Italic | FontStyle.Strikeout, GraphicsUnit.Pixel);
                }
                else {
                    return new Font(font.m_fontFamily, font.m_fontSize, FontStyle.Strikeout, GraphicsUnit.Pixel);
                }
            }
            else {
                if (font.m_bold && font.m_underline && font.m_italic) {
                    return new Font(font.m_fontFamily, font.m_fontSize, FontStyle.Bold | FontStyle.Underline | FontStyle.Italic, GraphicsUnit.Pixel);
                }
                else if (font.m_bold && font.m_underline) {
                    return new Font(font.m_fontFamily, font.m_fontSize, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Pixel);
                }
                else if (font.m_bold && font.m_italic) {
                    return new Font(font.m_fontFamily, font.m_fontSize, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Pixel);
                }
                else if (font.m_underline && font.m_italic) {
                    return new Font(font.m_fontFamily, font.m_fontSize, FontStyle.Underline | FontStyle.Italic, GraphicsUnit.Pixel);
                }
                else if (font.m_bold) {
                    return new Font(font.m_fontFamily, font.m_fontSize, FontStyle.Bold, GraphicsUnit.Pixel);
                }
                else if (font.m_underline) {
                    return new Font(font.m_fontFamily, font.m_fontSize, FontStyle.Underline, GraphicsUnit.Pixel);
                }
                else if (font.m_italic) {
                    return new Font(font.m_fontFamily, font.m_fontSize, FontStyle.Italic, GraphicsUnit.Pixel);
                }
                else {
                    return new Font(font.m_fontFamily, font.m_fontSize, GraphicsUnit.Pixel);
                }
            }
        }

        /// <summary>
        /// 获取偏移
        /// </summary>
        /// <returns>偏移坐标</returns>
        public virtual FCPoint getOffset() {
            return new FCPoint(m_offsetX, m_offsetY);
        }

        /// <summary>
        /// 获取画笔
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <returns>画笔</returns>
        protected Pen GetPen(long dwPenColor, float width, int style) {
            Color gdiColor = GetGdiPlusColor(dwPenColor);
            if (m_pen == null) {
                m_pen = new Pen(gdiColor, width);
                if (style == 0) {
                    m_pen.DashStyle = DashStyle.Solid;
                }
                else if (style == 1) {
                    m_pen.DashStyle = DashStyle.Dash;
                }
                else if (style == 2) {
                    m_pen.DashStyle = DashStyle.Dot;
                }
                if (m_opacity == 1) {
                    m_penColor = dwPenColor;
                }
                else {
                    m_penColor = FCColor.None;
                }
                m_penWidth = width;
                m_penStyle = style;
                switch (m_startLineCap) {
                    case 0:
                        m_pen.StartCap = LineCap.Flat;
                        break;
                    case 1:
                        m_pen.StartCap = LineCap.Square;
                        break;
                    case 2:
                        m_pen.StartCap = LineCap.Round;
                        break;
                    case 3:
                        m_pen.StartCap = LineCap.Triangle;
                        break;
                    case 4:
                        m_pen.StartCap = LineCap.NoAnchor;
                        break;
                    case 5:
                        m_pen.StartCap = LineCap.SquareAnchor;
                        break;
                    case 6:
                        m_pen.StartCap = LineCap.RoundAnchor;
                        break;
                    case 7:
                        m_pen.StartCap = LineCap.DiamondAnchor;
                        break;
                    case 8:
                        m_pen.StartCap = LineCap.ArrowAnchor;
                        break;
                    case 9:
                        m_pen.StartCap = LineCap.AnchorMask;
                        break;
                    case 10:
                        m_pen.StartCap = LineCap.Custom;
                        break;
                }
                switch (m_endLineCap) {
                    case 0:
                        m_pen.EndCap = LineCap.Flat;
                        break;
                    case 1:
                        m_pen.EndCap = LineCap.Square;
                        break;
                    case 2:
                        m_pen.EndCap = LineCap.Round;
                        break;
                    case 3:
                        m_pen.EndCap = LineCap.Triangle;
                        break;
                    case 4:
                        m_pen.EndCap = LineCap.NoAnchor;
                        break;
                    case 5:
                        m_pen.EndCap = LineCap.SquareAnchor;
                        break;
                    case 6:
                        m_pen.EndCap = LineCap.RoundAnchor;
                        break;
                    case 7:
                        m_pen.EndCap = LineCap.DiamondAnchor;
                        break;
                    case 8:
                        m_pen.EndCap = LineCap.ArrowAnchor;
                        break;
                    case 9:
                        m_pen.EndCap = LineCap.AnchorMask;
                        break;
                    case 10:
                        m_pen.EndCap = LineCap.Custom;
                        break;
                }
            }
            else {
                if (m_penColor == FCColor.None || m_penColor != dwPenColor) {
                    m_pen.Color = gdiColor;
                    m_penColor = dwPenColor;
                }
                if (m_opacity != 1) {
                    m_penColor = FCColor.None;
                }
                if (m_penWidth != width) {
                    m_pen.Width = width;
                    m_penWidth = width;
                }
                if (m_penStyle != style) {
                    if (style == 0) {
                        m_pen.DashStyle = DashStyle.Solid;
                    }
                    else if (style == 1) {
                        m_pen.DashStyle = DashStyle.Dash;
                    }
                    else if (style == 2) {
                        m_pen.DashStyle = DashStyle.Dot;
                    }
                    m_penStyle = style;
                }
            }
            return m_pen;
        }

        /// <summary>
        /// 获取绘制圆形矩形的路径
        /// </summary>
        /// <param name="gdiPlusRect">矩形</param>
        /// <param name="cornerRadius">边角半径</param>
        /// <returns>路径</returns>
        protected GraphicsPath GetRoundRectPath(Rectangle gdiPlusRect, int cornerRadius) {
            GraphicsPath gdiPlusPath = new GraphicsPath();
            gdiPlusPath.AddArc(gdiPlusRect.X, gdiPlusRect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
            gdiPlusPath.AddLine(gdiPlusRect.X + cornerRadius, gdiPlusRect.Y, gdiPlusRect.Right - cornerRadius * 2, gdiPlusRect.Y);
            gdiPlusPath.AddArc(gdiPlusRect.X + gdiPlusRect.Width - cornerRadius * 2, gdiPlusRect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
            gdiPlusPath.AddLine(gdiPlusRect.Right, gdiPlusRect.Y + cornerRadius * 2, gdiPlusRect.Right, gdiPlusRect.Y + gdiPlusRect.Height - cornerRadius * 2);
            gdiPlusPath.AddArc(gdiPlusRect.X + gdiPlusRect.Width - cornerRadius * 2, gdiPlusRect.Y + gdiPlusRect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
            gdiPlusPath.AddLine(gdiPlusRect.Right - cornerRadius * 2, gdiPlusRect.Bottom, gdiPlusRect.X + cornerRadius * 2, gdiPlusRect.Bottom);
            gdiPlusPath.AddArc(gdiPlusRect.X, gdiPlusRect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
            gdiPlusPath.AddLine(gdiPlusRect.X, gdiPlusRect.Bottom - cornerRadius * 2, gdiPlusRect.X, gdiPlusRect.Y + cornerRadius * 2);
            gdiPlusPath.CloseFigure();
            return gdiPlusPath;
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
            Rectangle gdiPlusRect = new Rectangle(rect.left + m_offsetX, rect.top + m_offsetY, rect.right - rect.left, rect.bottom - rect.top);
            if (m_scaleFactorX != 1 || m_scaleFactorY != 1) {
                gdiPlusRect.X = (int)Math.Floor(gdiPlusRect.X * m_scaleFactorX);
                gdiPlusRect.Y = (int)Math.Floor(gdiPlusRect.Y * m_scaleFactorY);
                gdiPlusRect.Width = (int)Math.Ceiling(gdiPlusRect.Width * m_scaleFactorX);
                gdiPlusRect.Height = (int)Math.Ceiling(gdiPlusRect.Height * m_scaleFactorY);
            }
            m_g.SetClip(gdiPlusRect);
        }

        /// <summary>
        /// 设置直线两端的样式
        /// </summary>
        /// <param name="startLineCap">开始的样式</param>
        /// <param name="endLineCap">结束的样式</param>
        public virtual void setLineCap(int startLineCap, int endLineCap) {
            m_startLineCap = startLineCap;
            m_endLineCap = endLineCap;
            if (m_pen != null) {
                switch (m_startLineCap) {
                    case 0:
                        m_pen.StartCap = LineCap.Flat;
                        break;
                    case 1:
                        m_pen.StartCap = LineCap.Square;
                        break;
                    case 2:
                        m_pen.StartCap = LineCap.Round;
                        break;
                    case 3:
                        m_pen.StartCap = LineCap.Triangle;
                        break;
                    case 4:
                        m_pen.StartCap = LineCap.NoAnchor;
                        break;
                    case 5:
                        m_pen.StartCap = LineCap.SquareAnchor;
                        break;
                    case 6:
                        m_pen.StartCap = LineCap.RoundAnchor;
                        break;
                    case 7:
                        m_pen.StartCap = LineCap.DiamondAnchor;
                        break;
                    case 8:
                        m_pen.StartCap = LineCap.ArrowAnchor;
                        break;
                    case 9:
                        m_pen.StartCap = LineCap.AnchorMask;
                        break;
                    case 10:
                        m_pen.StartCap = LineCap.Custom;
                        break;
                }
                switch (m_endLineCap) {
                    case 0:
                        m_pen.EndCap = LineCap.Flat;
                        break;
                    case 1:
                        m_pen.EndCap = LineCap.Square;
                        break;
                    case 2:
                        m_pen.EndCap = LineCap.Round;
                        break;
                    case 3:
                        m_pen.EndCap = LineCap.Triangle;
                        break;
                    case 4:
                        m_pen.EndCap = LineCap.NoAnchor;
                        break;
                    case 5:
                        m_pen.EndCap = LineCap.SquareAnchor;
                        break;
                    case 6:
                        m_pen.EndCap = LineCap.RoundAnchor;
                        break;
                    case 7:
                        m_pen.EndCap = LineCap.DiamondAnchor;
                        break;
                    case 8:
                        m_pen.EndCap = LineCap.ArrowAnchor;
                        break;
                    case 9:
                        m_pen.EndCap = LineCap.AnchorMask;
                        break;
                    case 10:
                        m_pen.EndCap = LineCap.Custom;
                        break;
                }
            }
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
            m_opacity = opacity;
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
            m_smoothMode = smoothMode;
            if (m_g != null) {
                switch (m_smoothMode) {
                    case 0:
                        m_g.SmoothingMode = SmoothingMode.Default;
                        break;
                    case 1:
                        m_g.SmoothingMode = SmoothingMode.AntiAlias;
                        break;
                    case 2:
                        m_g.SmoothingMode = SmoothingMode.HighQuality;
                        break;
                    case 3:
                        m_g.SmoothingMode = SmoothingMode.HighSpeed;
                        break;
                }
            }
        }

        /// <summary>
        /// 设置文字的质量
        /// </summary>
        /// <param name="textQuality">文字质量</param>
        public virtual void setTextQuality(int textQuality) {
            m_textQuality = textQuality;
            if (m_g != null) {
                switch (m_textQuality) {
                    case 0:
                        m_g.TextRenderingHint = TextRenderingHint.SystemDefault;
                        break;
                    case 1:
                        m_g.TextRenderingHint = TextRenderingHint.AntiAlias;
                        break;
                    case 2:
                        m_g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                        break;
                    case 3:
                        m_g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                        break;
                    case 4:
                        m_g.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
                        break;
                    case 5:
                        m_g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                        break;
                }
            }
        }

        /// <summary>
        /// 设置是否支持透明色
        /// </summary>
        /// <returns>是否支持</returns>
        public virtual bool supportTransparent() {
            return true;
        }

        /// <summary>
        /// 获取文字大小
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="font">字体</param>
        /// <returns>字体大小</returns>
        public virtual FCSize textSize(String text, FCFont font) {
            if (m_emptyStringFormat == null) {
                m_emptyStringFormat = StringFormat.GenericDefault;
                m_emptyStringFormat.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
            }
            Size gdiPlusSize = m_g.MeasureString(text, GetFont(font), Int32.MaxValue, m_emptyStringFormat).ToSize();
            return new FCSize(gdiPlusSize.Width, gdiPlusSize.Height);
        }

        /// <summary>
        /// 获取文字大小
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="font">字体</param>
        /// <returns>字体大小</returns>
        public virtual FCSizeF textSizeF(String text, FCFont font) {
            if (m_emptyStringFormat == null) {
                m_emptyStringFormat = StringFormat.GenericDefault;
                m_emptyStringFormat.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
            }
            SizeF gdiPlusSize = m_g.MeasureString(text, GetFont(font), Int32.MaxValue, m_emptyStringFormat);
            if (text.Length == 1) {
                gdiPlusSize.Width = (float)Math.Floor(gdiPlusSize.Width * 2 / 3);
            }
            return new FCSizeF(gdiPlusSize.Width, gdiPlusSize.Height);
        }
    }
}