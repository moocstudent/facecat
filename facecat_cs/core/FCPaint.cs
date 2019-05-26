/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace FaceCat {
    /// <summary>
    /// 锚定信息
    /// </summary>
    public struct FCAnchor {
        /// <summary>
        /// 创建锚定信息
        /// </summary>
        /// <param name="left">左侧</param>
        /// <param name="top">顶部</param>
        /// <param name="right">右侧</param>
        /// <param name="bottom">底部</param>
        public FCAnchor(bool left, bool top, bool right, bool bottom) {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        /// <summary>
        /// 底部
        /// </summary>
        public bool bottom;

        /// <summary>
        /// 左侧
        /// </summary>
        public bool left;

        /// <summary>
        /// 右侧
        /// </summary>
        public bool right;

        /// <summary>
        /// 顶部
        /// </summary>
        public bool top;
    }

    /// <summary>
    /// 控件内容的布局
    /// </summary>
    public enum FCContentAlignment {
        /// <summary>
        /// 中部靠下居中对齐
        /// </summary>
        BottomCenter,
        /// <summary>
        /// 左下方对齐
        /// </summary>
        BottomLeft,
        /// <summary>
        /// 右下方对齐
        /// </summary>
        BottomRight,
        /// <summary>
        /// 垂直居中
        /// </summary>
        MiddleCenter,
        /// <summary>
        /// 垂直居中靠左
        /// </summary>
        MiddleLeft,
        /// <summary>
        /// 垂直居中靠右
        /// </summary>
        MiddleRight,
        /// <summary>
        /// 中部靠上居中对齐
        /// </summary>
        TopCenter,
        /// <summary>
        /// 左上方对齐
        /// </summary>
        TopLeft,
        /// <summary>
        /// 右上方对齐
        /// </summary>
        TopRight
    }

    /// <summary>
    /// 光标
    /// </summary>
    public enum FCCursors {
        /// <summary>
        /// 程序启动
        /// </summary>
        AppStarting,
        /// <summary>
        /// 箭头
        /// </summary>
        Arrow,
        /// <summary>
        /// 十字线
        /// </summary>
        Cross,
        /// <summary>
        /// 手型
        /// </summary>
        Hand,
        /// <summary>
        /// 帮助
        /// </summary>
        Help,
        /// <summary>
        /// 文本光标出现
        /// </summary>
        IBeam,
        /// <summary>
        /// 当前操作无效
        /// </summary>
        No,
        /// <summary>
        /// 四个箭头
        /// </summary>
        SizeAll,
        /// <summary>
        /// 对角线大小调整光标
        /// </summary>
        SizeNESW,
        /// <summary>
        /// 双向垂直大小调整光标
        /// </summary>
        SizeNS,
        /// <summary>
        /// 双向对角线大小调整光标
        /// </summary>
        SizeNWSE,
        /// <summary>
        /// 双向水平大小调整光标
        /// </summary>
        SizeWE,
        /// <summary>
        /// 向上箭头
        /// </summary>
        UpArrow,
        /// <summary>
        /// 等待
        /// </summary>
        WaitCursor
    }

    /// <summary>
    /// 控件绑定边缘类型
    /// </summary>
    public enum FCDockStyle {
        /// <summary>
        /// 底部
        /// </summary>
        Bottom,
        /// <summary>
        /// 填充
        /// </summary>
        Fill,
        /// <summary>
        /// 左侧
        /// </summary>
        Left,
        /// <summary>
        /// 不绑定
        /// </summary>
        None,
        /// <summary>
        /// 右侧
        /// </summary>
        Right,
        /// <summary>
        /// 顶部
        /// </summary>
        Top
    }

    /// <summary>
    /// 控件横向排列方式
    /// </summary>
    public enum FCHorizontalAlign {
        /// <summary>
        /// 居中
        /// </summary>
        Center,
        /// <summary>
        /// 远离
        /// </summary>
        Right,
        /// <summary>
        /// 继承
        /// </summary>
        Inherit,
        /// <summary>
        /// 靠近
        /// </summary>
        Left
    }

    /// <summary>
    /// 控件纵向排列方式
    /// </summary>
    public enum FCVerticalAlign {
        /// <summary>
        /// 底部
        /// </summary>
        Bottom,
        /// <summary>
        /// 中间
        /// </summary>
        Middle,
        /// <summary>
        /// 继承
        /// </summary>
        Inherit,
        /// <summary>
        /// 顶部
        /// </summary>
        Top
    }

    /// <summary>
    /// 控件布局样式
    /// </summary>
    public enum FCLayoutStyle {
        /// <summary>
        /// 自下而上
        /// </summary>
        BottomToTop,
        /// <summary>
        /// 从左向右
        /// </summary>
        LeftToRight,
        /// <summary>
        /// 无布局
        /// </summary>
        None,
        /// <summary>
        /// 从右向左
        /// </summary>
        RightToLeft,
        /// <summary>
        /// 自上而下
        /// </summary>
        TopToBottom
    }

    /// <summary>
    /// 镜像模式
    /// </summary>
    public enum FCMirrorMode {
        /// <summary>
        /// 虫洞
        /// </summary>
        BugHole,
        /// <summary>
        /// 无
        /// </summary>
        None,
        /// <summary>
        /// 影子
        /// </summary>
        Shadow
    }

    /// <summary>
    /// 颜色类
    /// </summary>
    public class FCColor {
        /// <summary>
        /// 背景色
        /// </summary>
        public static long Back = -200000000001;

        /// <summary>
        /// 边线颜色
        /// </summary>
        public static long Border = -200000000002;

        /// <summary>
        /// 前景色
        /// </summary>
        public static long Text = -200000000003;

        /// <summary>
        /// 不可用的背景色
        /// </summary>
        public static long DisabledBack = -200000000004;

        /// <summary>
        /// 不可用的前景色
        /// </summary>
        public static long DisabledText = -200000000005;

        /// <summary>
        /// 触摸悬停的背景色
        /// </summary>
        public static long Hovered = -200000000006;

        /// <summary>
        /// 触摸被按下的背景色
        /// </summary>
        public static long Pushed = -200000000007;

        /// <summary>
        /// 空颜色
        /// </summary>
        public static long None = -2000000000;

        /// <summary>
        /// 获取RGB颜色
        /// </summary>
        /// <param name="r">红色值</param>
        /// <param name="g">绿色值</param>
        /// <param name="b">蓝色值</param>
        /// <returns>RGB颜色</returns>
        public static int argb(int r, int g, int b) {
            return ((r | (g << 8)) | (b << 0x10));

        }

        /// <summary>
        /// 获取ARGB颜色
        /// </summary>
        /// <param name="a">透明值</param>
        /// <param name="r">红色</param>
        /// <param name="g">绿色</param>
        /// <param name="b">蓝色</param>
        /// <returns></returns>
        public static long argb(int a, int r, int g, int b) {
            int rgb = ((r | (g << 8)) | (b << 0x10));

            if (a == 255) {
                return rgb;
            }
            else if (a == 0) {
                return None;
            }
            else {
                return -((long)rgb * 1000 + a);
            }
        }

        /// <summary>
        /// 获取颜色的各项数值
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="color">颜色</param>
        /// <param name="a">透明值</param>
        /// <param name="r">红色</param>
        /// <param name="g">绿色</param>
        /// <param name="b">蓝色</param>
        public static void toArgb(FCPaint paint, long color, ref int a, ref int r, ref int g, ref int b) {
            if (paint != null) {
                color = paint.getColor(color);
            }
            a = 255;
            if (color < 0) {
                color = Math.Abs(color);
                a = (int)(color - color / 1000 * 1000);
                color /= 1000;
            }
            r = (int)(color & 0xff);
            g = (int)((color >> 8) & 0xff);
            b = (int)((color >> 0x10) & 0xff);
        }

        /// <summary>
        /// 获取比例色
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="originalColor">原始色</param>
        /// <param name="ratio">比例</param>
        /// <returns>比例色</returns>
        public static long ratioColor(FCPaint paint, long originalColor, double ratio) {
            int a = 0, r = 0, g = 0, b = 0;
            toArgb(paint, originalColor, ref a, ref r, ref g, ref b);
            r = (int)(r * ratio);
            g = (int)(g * ratio);
            b = (int)(b * ratio);
            if (r > 255) r = 255;
            if (g > 255) g = 255;
            if (b > 255) b = 255;
            return argb(a, r, g, b);
        }

        /// <summary>
        /// 获取反色
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="originalColor">原始色</param>
        /// <returns>反色</returns>
        public static long reverse(FCPaint paint, long originalColor) {
            int a = 0, r = 0, g = 0, b = 0;
            toArgb(paint, originalColor, ref a, ref r, ref g, ref b);
            return argb(a, 255 - r, 255 - g, 255 - b);
        }
    }

    /// <summary>
    /// 边距信息
    /// </summary>
    public struct FCPadding {
        /// <summary>
        /// 创建边距
        /// </summary>
        /// <param name="all">所有边距</param>
        public FCPadding(int all) {
            bottom = all;
            left = all;
            right = all;
            top = all;
        }

        /// <summary>
        /// 创建边距
        /// </summary>
        /// <param name="left">左边距</param>
        /// <param name="top">顶边距</param>
        /// <param name="right">右边距</param>
        /// <param name="bottom">底边距</param>
        public FCPadding(int left, int top, int right, int bottom) {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        /// <summary>
        /// 底边距
        /// </summary>
        public int bottom;

        /// <summary>
        /// 左边距
        /// </summary>
        public int left;

        /// <summary>
        /// 右边距
        /// </summary>
        public int right;

        /// <summary>
        /// 顶边距
        /// </summary>
        public int top;
    }

    /// <summary>
    /// 字体类
    /// </summary>
    public class FCFont {
        /// <summary>
        /// 创建字体
        /// </summary>
        public FCFont() {
        }

        /// <summary>
        /// 创建字体
        /// </summary>
        /// <param name="fontFamily">字体</param>
        /// <param name="fontSize">字号</param>
        /// <param name="bold">是否粗体</param>
        /// <param name="underline">是否有下划线</param>
        /// <param name="italic">是否斜体</param>
        public FCFont(String fontFamily, float fontSize, bool bold, bool underline, bool italic) {
            m_fontFamily = fontFamily;
            m_fontSize = fontSize;
            m_bold = bold;
            m_underline = underline;
            m_italic = italic;
        }

        /// <summary>
        /// 创建字体
        /// </summary>
        /// <param name="fontFamily">字体</param>
        /// <param name="fontSize">字号</param>
        /// <param name="bold">是否粗体</param>
        /// <param name="underline">是否有下划线</param>
        /// <param name="italic">是否斜体</param>
        /// <param name="strikeout">是否有删除线</param>
        public FCFont(String fontFamily, float fontSize, bool bold, bool underline, bool italic, bool strikeout) {
            m_fontFamily = fontFamily;
            m_fontSize = fontSize;
            m_bold = bold;
            m_underline = underline;
            m_italic = italic;
            m_strikeout = strikeout;
        }

        /// <summary>
        /// 是否粗体
        /// </summary>
        public bool m_bold;

        /// <summary>
        /// 字体
        /// </summary>
        public String m_fontFamily = "Arial";

        /// <summary>
        /// 字体大小
        /// </summary>
        public float m_fontSize = 12;

        /// <summary>
        /// 是否斜体
        /// </summary>
        public bool m_italic;

        /// <summary>
        /// 是否有删除线
        /// </summary>
        public bool m_strikeout;

        /// <summary>
        /// 是否有下划线
        /// </summary>
        public bool m_underline;
    }

    /// <summary>
    /// 点类
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct FCPoint {
        /// <summary>
        /// 创建点
        /// </summary>
        /// <param name="x">横坐标</param>
        /// <param name="y">纵坐标</param>
        public FCPoint(int x, int y) {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// 创建点
        /// </summary>
        /// <param name="x">横坐标</param>
        /// <param name="y">纵坐标</param>
        public FCPoint(float x, float y) {
            this.x = (int)x;
            this.y = (int)y;
        }

        /// <summary>
        /// 横坐标
        /// </summary>
        public int x;

        /// <summary>
        /// 纵坐标
        /// </summary>
        public int y;
    }

    /// <summary>
    /// 浮点类
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct FCPointF {
        /// <summary>
        /// 创建浮点
        /// </summary>
        /// <param name="x">横坐标</param>
        /// <param name="y">纵坐标</param>
        public FCPointF(float x, float y) {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// 横坐标
        /// </summary>
        public float x;

        /// <summary>
        /// 纵坐标
        /// </summary>
        public float y;
    }

    /// <summary>
    /// 浮点类
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class FCPointF2 {
        /// <summary>
        /// 创建浮点
        /// </summary>
        /// <param name="x">横坐标</param>
        /// <param name="y">纵坐标</param>
        public FCPointF2() {
        }

        /// <summary>
        /// 横坐标
        /// </summary>
        public float x;

        /// <summary>
        /// 纵坐标
        /// </summary>
        public float y;
    }

    /// <summary>
    /// 字体类
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct FCRect {
        /// <summary>
        /// 创建矩形
        /// </summary>
        /// <param name="left">左侧坐标</param>
        /// <param name="top">顶部坐标</param>
        /// <param name="right">右侧坐标</param>
        /// <param name="bottom">底部坐标</param>
        public FCRect(int left, int top, int right, int bottom) {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        /// <summary>
        /// 创建矩形
        /// </summary>
        /// <param name="left">左侧坐标</param>
        /// <param name="top">顶部坐标</param>
        /// <param name="right">右侧坐标</param>
        /// <param name="bottom">底部坐标</param>
        public FCRect(float left, float top, float right, float bottom) {
            this.left = (int)left;
            this.top = (int)top;
            this.right = (int)right;
            this.bottom = (int)bottom;
        }

        /// <summary>
        /// 左侧坐标
        /// </summary>
        public int left;

        /// <summary>
        /// 顶部坐标
        /// </summary>
        public int top;

        /// <summary>
        /// 右侧坐标
        /// </summary>
        public int right;

        /// <summary>
        /// 底部坐标
        /// </summary>
        public int bottom;
    }

    /// <summary>
    /// 浮点型矩形类
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct FCRectF {
        /// <summary>
        /// 创建矩形
        /// </summary>
        /// <param name="left">左侧坐标</param>
        /// <param name="top">顶部坐标</param>
        /// <param name="right">右侧坐标</param>
        /// <param name="bottom">底部坐标</param>
        public FCRectF(float left, float top, float right, float bottom) {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        /// <summary>
        /// 左侧坐标
        /// </summary>
        public float left;

        /// <summary>
        /// 顶部坐标
        /// </summary>
        public float top;

        /// <summary>
        /// 右侧坐标
        /// </summary>
        public float right;

        /// <summary>
        /// 底部坐标
        /// </summary>
        public float bottom;
    }

    /// <summary>
    /// 尺寸类
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct FCSize {
        /// <summary>
        /// 创建尺寸
        /// </summary>
        /// <param name="cx">宽</param>
        /// <param name="cy">高</param>
        public FCSize(int cx, int cy) {
            this.cx = cx;
            this.cy = cy;
        }

        /// <summary>
        /// 宽
        /// </summary>
        public int cx;
        /// <summary>
        /// 高
        /// </summary>
        public int cy;
    }

    /// <summary>
    /// 浮点型尺寸类
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct FCSizeF {
        /// <summary>
        /// 创建浮点型尺寸
        /// </summary>
        /// <param name="cx">宽</param>
        /// <param name="cy">高</param>
        public FCSizeF(float cx, float cy) {
            this.cx = cx;
            this.cy = cy;
        }

        /// <summary>
        /// 宽
        /// </summary>
        public float cx;
        /// <summary>
        /// 高
        /// </summary>
        public float cy;
    }

    /// <summary>
    /// 浮点型尺寸类
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class FCSizeF2 {
        /// <summary>
        /// 创建浮点型尺寸
        /// </summary>
        /// <param name="cx">宽</param>
        /// <param name="cy">高</param>
        public FCSizeF2() {
        }

        /// <summary>
        /// 宽
        /// </summary>
        public float cx;
        /// <summary>
        /// 高
        /// </summary>
        public float cy;
    }

    /// <summary>
    /// 触摸信息
    /// </summary>
    public class FCTouchInfo {
        /// <summary>
        /// 点击次数
        /// </summary>
        public int m_clicks;
        /// <summary>
        /// 滚动值
        /// </summary>
        public int m_delta;
        /// <summary>
        /// 是否第一个坐标
        /// </summary>
        public bool m_firstTouch;
        /// <summary>
        /// 第一个坐标
        /// </summary>
        public FCPoint m_firstPoint;
        /// <summary>
        /// 是否第二个坐标
        /// </summary>
        public bool m_secondTouch;
        /// <summary>
        /// 第二个坐标
        /// </summary>
        public FCPoint m_secondPoint;

        /// <summary>
        /// 拷贝信息
        /// </summary>
        /// <returns></returns>
        public FCTouchInfo clone() {
            FCTouchInfo touchInfo = new FCTouchInfo();
            touchInfo.m_firstPoint = m_firstPoint;
            touchInfo.m_firstTouch = m_firstTouch;
            touchInfo.m_secondPoint = m_secondPoint;
            touchInfo.m_secondTouch = m_secondTouch;
            touchInfo.m_clicks = m_clicks;
            touchInfo.m_delta = m_delta;
            return touchInfo;
        }
    }

    /// <summary>
    /// 绘图类
    /// </summary>
    public interface FCPaint {
        /// <summary>
        /// 添加曲线
        /// </summary>
        /// <param name="rect">矩形区域</param>
        /// <param name="startAngle">从 x 轴到弧线的起始点沿顺时针方向度量的角（以度为单位）</param>
        /// <param name="sweepAngle">从 startAngle 参数到弧线的结束点沿顺时针方向度量的角（以度为单位）</param>
        void addArc(FCRect rect, float startAngle, float sweepAngle);

        /// <summary>
        /// 添加贝赛尔曲线
        /// </summary>
        /// <param name="point1">坐标1</param>
        /// <param name="point2">坐标2</param>
        /// <param name="point3">坐标3</param>
        /// <param name="point4">坐标4</param>
        void addBezier(FCPoint[] points);

        /// <summary>
        /// 添加曲线
        /// </summary>
        /// <param name="points">点阵</param>
        void addCurve(FCPoint[] points);

        /// <summary>
        /// 添加椭圆
        /// </summary>
        /// <param name="rect">矩形</param>
        void addEllipse(FCRect rect);

        /// <summary>
        /// 添加直线
        /// </summary>
        /// <param name="x1">第一个点的横坐标</param>
        /// <param name="y1">第一个点的纵坐标</param>
        /// <param name="x2">第二个点的横坐标</param>
        /// <param name="y2">第二个点的纵坐标</param>
        void addLine(int x1, int y1, int x2, int y2);

        /// <summary>
        /// 添加矩形
        /// </summary>
        /// <param name="rect">区域</param>
        void addRect(FCRect rect);

        /// <summary>
        /// 添加扇形
        /// </summary>
        /// <param name="rect">矩形区域</param>
        /// <param name="startAngle">从 x 轴到弧线的起始点沿顺时针方向度量的角（以度为单位）</param>
        /// <param name="sweepAngle">从 startAngle 参数到弧线的结束点沿顺时针方向度量的角（以度为单位）</param>
        void addPie(FCRect rect, float startAngle, float sweepAngle);

        /// <summary>
        /// 添加文字
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="font">字体</param>
        /// <param name="rect">区域</param>
        void addText(String text, FCFont font, FCRect rect);

        /// <summary>
        /// 开始导出
        /// </summary>
        /// <param name="exportPath">路径</param>
        /// <param name="rect">区域</param>
        void beginExport(String exportPath, FCRect rect);

        /// <summary>
        /// 开始绘图
        /// </summary>
        /// <param name="hdc">HDC</param>
        /// <param name="wRect">窗体区域</param>
        /// <param name="pRect">刷新区域</param>
        void beginPaint(IntPtr hdc, FCRect wRect, FCRect pRect);

        /// <summary>
        /// 开始一段路径
        /// </summary>
        void beginPath();

        /// <summary>
        /// 清除缓存
        /// </summary>
        void clearCaches();

        /// <summary>
        /// 裁剪路径
        /// </summary>
        void clipPath();

        /// <summary>
        /// 闭合路径
        /// </summary>
        void closeFigure();

        /// <summary>
        /// 结束一段路径
        /// </summary>
        void closePath();

        /// <summary>
        /// 删除对象
        /// </summary>
        void delete();

        /// <summary>
        /// 绘制弧线
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="rect">矩形区域</param>
        /// <param name="startAngle">从 x 轴到弧线的起始点沿顺时针方向度量的角（以度为单位）</param>
        /// <param name="sweepAngle">从 startAngle 参数到弧线的结束点沿顺时针方向度量的角（以度为单位）</param>
        void drawArc(long dwPenColor, float width, int style, FCRect rect, float startAngle, float sweepAngle);

        /// <summary>
        /// 设置贝赛尔曲线
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="points">坐标阵</param>
        void drawBezier(long dwPenColor, float width, int style, FCPoint[] points);

        /// <summary>
        /// 绘制曲线
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="points">坐标阵</param>
        void drawCurve(long dwPenColor, float width, int style, FCPoint[] points);

        /// <summary>
        /// 绘制矩形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="rect">矩形区域</param>
        void drawEllipse(long dwPenColor, float width, int style, FCRect rect);

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
        void drawEllipse(long dwPenColor, float width, int style, int left, int top, int right, int bottom);

        /// <summary>
        /// 绘制图片
        /// </summary>
        /// <param name="imagePath">图片路径</param>
        /// <param name="rect">绘制区域</param>
        void drawImage(String imagePath, FCRect rect);

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
        void drawLine(long dwPenColor, float width, int style, int x1, int y1, int x2, int y2);

        /// <summary>
        /// 绘制直线
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="x">第一个点的坐标</param>
        /// <param name="y">第二个点的坐标</param>
        void drawLine(long dwPenColor, float width, int style, FCPoint x, FCPoint y);

        /// <summary>
        /// 绘制路径
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        void drawPath(long dwPenColor, float width, int style);

        /// <summary>
        /// 绘制扇形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="rect">矩形区域</param>
        /// <param name="startAngle">从 x 轴到弧线的起始点沿顺时针方向度量的角（以度为单位）</param>
        /// <param name="sweepAngle">从 startAngle 参数到弧线的结束点沿顺时针方向度量的角（以度为单位）</param>
        void drawPie(long dwPenColor, float width, int style, FCRect rect, float startAngle, float sweepAngle);

        /// <summary>
        /// 绘制多边形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="points">点的数组</param>
        void drawPolygon(long dwPenColor, float width, int style, FCPoint[] points);

        /// <summary>
        /// 绘制大量直线
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="points">点集</param>
        void drawPolyline(long dwPenColor, float width, int style, FCPoint[] points);

        /// <summary>
        /// 绘制矩形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="rect">矩形区域</param>
        void drawRect(long dwPenColor, float width, int style, FCRect rect);

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
        void drawRect(long dwPenColor, float width, int style, int left, int top, int right, int bottom);

        /// <summary>
        /// 绘制圆角矩形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="width">宽度</param>
        /// <param name="style">样式</param>
        /// <param name="rect">矩形区域</param>
        /// <param name="cornerRadius">边角半径</param>
        void drawRoundRect(long dwPenColor, float width, int style, FCRect rect, int cornerRadius);

        /// <summary>
        /// 绘制文字
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="font">字体</param>
        /// <param name="rect">矩形区域</param>
        void drawText(String text, long dwPenColor, FCFont font, FCRect rect);

        /// <summary>
        /// 绘制文字
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="font">字体</param>
        /// <param name="rect">矩形区域</param>
        void drawText(String text, long dwPenColor, FCFont font, FCRectF rect);

        /// <summary>
        /// 绘制自动省略结尾的文字
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="font">字体</param>
        /// <param name="rect">矩形区域</param>
        void drawTextAutoEllipsis(String text, long dwPenColor, FCFont font, FCRect rect);

        /// <summary>
        /// 结束导出
        /// </summary>
        void endExport();

        /// <summary>
        /// 结束绘图
        /// </summary>
        void endPaint();

        /// <summary>
        /// 反裁剪路径
        /// </summary>
        void excludeClipPath();

        /// <summary>
        /// 填充椭圆
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="rect">矩形区域</param>
        void fillEllipse(long dwPenColor, FCRect rect);

        /// <summary>
        /// 填充椭圆
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="left">左侧坐标</param>
        /// <param name="top">顶部左标</param>
        /// <param name="right">右侧坐标</param>
        /// <param name="bottom">底部坐标</param>
        void fillEllipse(long dwPenColor, int left, int top, int right, int bottom);

        /// <summary>
        /// 绘制渐变椭圆
        /// </summary>
        /// <param name="dwFirst">开始颜色</param>
        /// <param name="dwSecond">结束颜色</param>
        /// <param name="rect">矩形</param>
        /// <param name="angle">角度</param>
        void fillGradientEllipse(long dwFirst, long dwSecond, FCRect rect, int angle);

        /// <summary>
        /// 填充渐变路径
        /// </summary>
        /// <param name="dwFirst">开始颜色</param>
        /// <param name="dwSecond">结束颜色</param>
        /// <param name="points">点的集合</param>
        /// <param name="angle">角度</param>
        void fillGradientPath(long dwFirst, long dwSecond, FCRect rect, int angle);

        /// <summary>
        /// 绘制渐变的多边形
        /// </summary>
        /// <param name="dwFirst">开始颜色</param>
        /// <param name="dwSecond">结束颜色</param>
        /// <param name="points">点的集合</param>
        /// <param name="angle">角度</param>
        void fillGradientPolygon(long dwFirst, long dwSecond, FCPoint[] points, int angle);

        /// <summary>
        /// 绘制渐变矩形
        /// </summary>
        /// <param name="dwFirst">开始颜色</param>
        /// <param name="dwSecond">结束颜色</param>
        /// <param name="rect">矩形</param>
        /// <param name="cornerRadius">圆角半径</param>
        /// <param name="angle">角度</param>
        void fillGradientRect(long dwFirst, long dwSecond, FCRect rect, int cornerRadius, int angle);

        /// <summary>
        /// 填充路径
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        void fillPath(long dwPenColor);

        /// <summary>
        /// 绘制扇形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="rect">矩形区域</param>
        /// <param name="startAngle">从 x 轴到弧线的起始点沿顺时针方向度量的角（以度为单位）</param>
        /// <param name="sweepAngle">从 startAngle 参数到弧线的结束点沿顺时针方向度量的角（以度为单位）</param>
        void fillPie(long dwPenColor, FCRect rect, float startAngle, float sweepAngle);

        /// <summary>
        /// 填充多边形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="points">点的数组</param>
        void fillPolygon(long dwPenColor, FCPoint[] points);

        /// <summary>
        /// 填充矩形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="rect">矩形区域</param>
        void fillRect(long dwPenColor, FCRect rect);

        /// <summary>
        /// 填充矩形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="left">左侧坐标</param>
        /// <param name="top">顶部左标</param>
        /// <param name="right">右侧坐标</param>
        /// <param name="bottom">底部坐标</param>
        void fillRect(long dwPenColor, int left, int top, int right, int bottom);

        /// <summary>
        /// 填充圆角矩形
        /// </summary>
        /// <param name="dwPenColor">颜色</param>
        /// <param name="rect">矩形区域</param>
        /// <param name="cornerRadius">边角半径</param>
        void fillRoundRect(long dwPenColor, FCRect rect, int cornerRadius);

        /// <summary>
        /// 获取颜色
        /// </summary>
        /// <param name="dwPenColor">输入颜色</param>
        /// <returns>输出颜色</returns>
        long getColor(long dwPenColor);

        /// <summary>
        /// 获取要绘制的颜色
        /// </summary>
        /// <param name="dwPenColor">输入颜色</param>
        /// <returns>输出颜色</returns>
        long getPaintColor(long dwPenColor);

        /// <summary>
        /// 获取偏移
        /// </summary>
        /// <returns>偏移坐标</returns>
        FCPoint getOffset();

        /// <summary>
        /// 旋转角度
        /// </summary>
        /// <param name="op">圆心坐标</param>
        /// <param name="mp">点的坐标</param>
        /// <param name="angle">角度</param>
        /// <returns>结果坐标</returns>
        FCPoint rotate(FCPoint op, FCPoint mp, int angle);

        /// <summary>
        /// 设置裁剪区域
        /// </summary>
        /// <param name="rect">区域</param>
        void setClip(FCRect rect);

        /// <summary>
        /// 设置直线两端的样式
        /// </summary>
        /// <param name="startLineCap">开始的样式</param>
        /// <param name="endLineCap">结束的样式</param>
        void setLineCap(int startLineCap, int endLineCap);

        /// <summary>
        /// 设置偏移
        /// </summary>
        /// <param name="mp">偏移坐标</param>
        void setOffset(FCPoint mp);

        /// <summary>
        /// 设置透明度
        /// </summary>
        /// <param name="opacity">透明度</param>
        void setOpacity(float opacity);

        /// <summary>
        /// 设置资源的路径
        /// </summary>
        /// <param name="resourcePath">资源的路径</param>
        void setResourcePath(String resourcePath);

        /// <summary>
        /// 设置旋转角度
        /// </summary>
        /// <param name="rotateAngle">旋转角度</param>
        void setRotateAngle(int rotateAngle);

        /// <summary>
        /// 设置缩放因子
        /// </summary>
        /// <param name="scaleFactorX">横向因子</param>
        /// <param name="scaleFactorY">纵向因子</param>
        void setScaleFactor(double scaleFactorX, double scaleFactorY);

        /// <summary>
        /// 设置平滑模式
        /// </summary>
        /// <param name="smoothMode">平滑模式</param>
        void setSmoothMode(int smoothMode);

        /// <summary>
        /// 设置文字的质量
        /// </summary>
        /// <param name="textQuality">文字质量</param>
        void setTextQuality(int textQuality);

        /// <summary>
        /// 设置是否支持透明色
        /// </summary>
        /// <returns>是否支持</returns>
        bool supportTransparent();

        /// <summary>
        /// 获取文字大小
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="font">字体</param>
        /// <returns>字体大小</returns>
        FCSize textSize(String text, FCFont font);

        /// <summary>
        /// 获取文字大小
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="font">字体</param>
        /// <returns>字体大小</returns>
        FCSizeF textSizeF(String text, FCFont font);
    }
}