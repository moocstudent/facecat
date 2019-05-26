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
    /// 字符串处理类
    /// </summary>
    public class FCStr {
        /// <summary>
        /// 锚定信息转化为字符
        /// </summary>
        /// <param name="anchor"> 锚定信息</param>
        /// <returns>字符</returns>
        public static String convertAnchorToStr(FCAnchor anchor) {
            ArrayList<String> strs = new ArrayList<String>();
            if (anchor.left) {
                strs.add("Left");
            }
            if (anchor.top) {
                strs.add("Top");
            }
            if (anchor.right) {
                strs.add("Right");
            }
            if (anchor.bottom) {
                strs.add("Bottom");
            }
            String anchorStr = "";
            int size = strs.size();
            if (size > 0) {
                for (int i = 0; i < size; i++) {
                    anchorStr += strs[i];
                    if (i != size - 1) {
                        anchorStr += ",";
                    }
                }
            }
            else {
                anchorStr = "None";
            }
            return anchorStr;
        }

        /// <summary>
        /// 布尔型数值转换为字符
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>字符</returns>
        public static String convertBoolToStr(bool value) {
            return value ? "True" : "False";
        }

        /// <summary>
        /// 颜色转换为字符
        /// </summary>
        /// <param name="color">颜色</param>
        /// <returns>字符</returns>
        public static String convertColorToStr(long color) {
            if (color == FCColor.None) {
                return "Empty";
            }
            else if (color == FCColor.Back) {
                return "Control";
            }
            else if (color == FCColor.Border) {
                return "ControlBorder";
            }
            else if (color == FCColor.Text) {
                return "ControlText";
            }
            else if (color == FCColor.DisabledBack) {
                return "DisabledControl";
            }
            else if (color == FCColor.DisabledText) {
                return "DisabledControlText";
            }
            int a = 0, r = 0, g = 0, b = 0;
            FCColor.toArgb(null, color, ref a, ref r, ref g, ref b);
            String str = String.Empty;
            if (a == 255) {
                str = String.Format("{0},{1},{2}", r, g, b);
            }
            else {
                str = String.Format("{0},{1},{2},{3}", a, r, g, b);
            }
            return str;
        }

        /// <summary>
        /// 内容布局转字符串
        /// </summary>
        /// <param name="contentAlignment">内容布局</param>
        /// <returns>字符串</returns>
        public static String convertContentAlignmentToStr(FCContentAlignment contentAlignment) {
            String str = "";
            if (contentAlignment == FCContentAlignment.BottomCenter) {
                str = "BottomCenter";
            }
            else if (contentAlignment == FCContentAlignment.BottomLeft) {
                str = "BottomLeft";
            }
            else if (contentAlignment == FCContentAlignment.BottomRight) {
                str = "BottomRight";
            }
            else if (contentAlignment == FCContentAlignment.MiddleCenter) {
                str = "MiddleCenter";
            }
            else if (contentAlignment == FCContentAlignment.MiddleLeft) {
                str = "MiddleLeft";
            }
            else if (contentAlignment == FCContentAlignment.MiddleRight) {
                str = "MiddleRight";
            }
            else if (contentAlignment == FCContentAlignment.TopCenter) {
                str = "TopCenter";
            }
            else if (contentAlignment == FCContentAlignment.TopLeft) {
                str = "TopLeft";
            }
            else if (contentAlignment == FCContentAlignment.TopRight) {
                str = "TopRight";
            }
            return str;
        }

        /// <summary>
        /// 光标转换为字符
        /// </summary>
        /// <param name="cursor">光标</param>
        /// <returns>字符</returns>
        public static String convertCursorToStr(FCCursors cursor) {
            String str = "";
            if (cursor == FCCursors.AppStarting) {
                str = "AppStarting";
            }
            else if (cursor == FCCursors.Arrow) {
                str = "Arrow";
            }
            else if (cursor == FCCursors.Cross) {
                str = "Cross";
            }
            else if (cursor == FCCursors.Hand) {
                str = "Hand";
            }
            else if (cursor == FCCursors.Help) {
                str = "Help";
            }
            else if (cursor == FCCursors.IBeam) {
                str = "IBeam";
            }
            else if (cursor == FCCursors.No) {
                str = "NO";
            }
            else if (cursor == FCCursors.SizeAll) {
                str = "SizeAll";
            }
            else if (cursor == FCCursors.SizeNESW) {
                str = "SizeNESW";
            }
            else if (cursor == FCCursors.SizeNS) {
                str = "SizeNS";
            }
            else if (cursor == FCCursors.SizeNWSE) {
                str = "SizeNWSE";
            }
            else if (cursor == FCCursors.SizeWE) {
                str = "SizeWE";
            }
            else if (cursor == FCCursors.UpArrow) {
                str = "UpArrow";
            }
            else if (cursor == FCCursors.WaitCursor) {
                str = "WaitCursor";
            }
            return str;
        }


        /// <summary>
        /// 将日期格式转换为数字格式
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>数字格式</returns>
        public static double convertDateToNum(DateTime date) {
            return FCStr.getDateNum(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond);
        }

        /// <summary>
        /// 绑定信息转换为字符
        /// </summary>
        /// <param name="dock">绑定信息</param>
        /// <returns>字符</returns>
        public static String convertDockToStr(FCDockStyle dock) {
            String str = "";
            if (dock == FCDockStyle.Bottom) {
                str = "Bottom";
            }
            else if (dock == FCDockStyle.Fill) {
                str = "Fill";
            }
            else if (dock == FCDockStyle.Left) {
                str = "Left";
            }
            else if (dock == FCDockStyle.None) {
                str = "None";
            }
            else if (dock == FCDockStyle.Right) {
                str = "Right";
            }
            else if (dock == FCDockStyle.Top) {
                str = "Top";
            }
            return str;
        }

        /// <summary>
        /// 双精度浮点型数值转换为字符
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>字符</returns>
        public static String convertDoubleToStr(double value) {
            return value.ToString();
        }

        /// <summary>
        /// 单精度浮动点型数值转换为字符
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>字符</returns>
        public static String convertFloatToStr(float value) {
            return value.ToString();
        }

        /// <summary>
        /// 字体转换为字符
        /// </summary>
        /// <param name="font">字体</param>
        /// <returns>字符</returns>
        public static String convertFontToStr(FCFont font) {
            ArrayList<String> strs = new ArrayList<String>();
            strs.add(font.m_fontFamily);
            strs.add(font.m_fontSize.ToString());
            if (font.m_bold) {
                strs.add("Bold");
            }
            if (font.m_underline) {
                strs.add("UnderLine");
            }
            if (font.m_italic) {
                strs.add("Italic");
            }
            if (font.m_strikeout) {
                strs.add("Strikeout");
            }
            String fontStr = "";
            int size = strs.size();
            if (size > 0) {
                for (int i = 0; i < size; i++) {
                    fontStr += strs[i];
                    if (i != size - 1) {
                        fontStr += ",";
                    }
                }
            }
            return fontStr;
        }

        /// <summary>
        /// 横向排列方式转换为字符
        /// </summary>
        /// <param name="horizontalAlign">横向排列方式</param>
        /// <returns>字符</returns>
        public static String convertHorizontalAlignToStr(FCHorizontalAlign horizontalAlign) {
            String str = "";
            if (horizontalAlign == FCHorizontalAlign.Center) {
                str = "Center";
            }
            else if (horizontalAlign == FCHorizontalAlign.Right) {
                str = "Right";
            }
            else if (horizontalAlign == FCHorizontalAlign.Inherit) {
                str = "Inherit";
            }
            else if (horizontalAlign == FCHorizontalAlign.Left) {
                str = "Left";
            }
            return str;
        }

        /// <summary>
        /// 整型数值转换为字符
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>字符</returns>
        public static String convertIntToStr(int value) {
            return value.ToString();
        }

        /// <summary>
        /// 布局样式转换为字符
        /// </summary>
        /// <param name="layoutStyle">布局样式转</param>
        /// <returns>布局样式</returns>
        public static String convertLayoutStyleToStr(FCLayoutStyle layoutStyle) {
            String str = "";
            if (layoutStyle == FCLayoutStyle.BottomToTop) {
                str = "BottomToTop";
            }
            else if (layoutStyle == FCLayoutStyle.LeftToRight) {
                str = "LeftToRight";
            }
            else if (layoutStyle == FCLayoutStyle.None) {
                str = "None";
            }
            else if (layoutStyle == FCLayoutStyle.RightToLeft) {
                str = "RightToLeft";
            }
            else if (layoutStyle == FCLayoutStyle.TopToBottom) {
                str = "TopToBottom";
            }
            return str;
        }

        /// <summary>
        /// 长整型数值转换为字符
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>字符串</returns>
        public static String convertLongToStr(long value) {
            return value.ToString();
        }

        /// <summary>
        /// 将数字格式转换为日期格式
        /// </summary>
        /// <param name="num">数字</param>
        /// <returns>日期格式</returns>
        public static DateTime convertNumToDate(double num) {
            int tm_year = 0;
            int tm_mon = 0;
            int tm_mday = 0;
            int tm_hour = 0;
            int tm_min = 0;
            int tm_sec = 0;
            int tm_msec = 0;
            FCStr.getDateByNum(num, ref tm_year, ref tm_mon, ref tm_mday, ref tm_hour, ref tm_min, ref tm_sec, ref tm_msec);
            return new DateTime(tm_year, tm_mon, tm_mday, tm_hour, tm_min, tm_sec, tm_msec);
        }

        /// <summary>
        /// 边距转换为字符
        /// </summary>
        /// <param name="padding">边距</param>
        /// <returns>字符</returns>
        public static String convertPaddingToStr(FCPadding padding) {
            return String.Format("{0},{1},{2},{3}", padding.left, padding.top, padding.right, padding.bottom);
        }

        /// <summary>
        /// 坐标转换为字符
        /// </summary>
        /// <param name="point">坐标</param>
        /// <returns>字符</returns>
        public static String convertPointToStr(FCPoint point) {
            return String.Format("{0},{1}", point.x, point.y);
        }

        /// <summary>
        /// 矩形转换为字符
        /// </summary>
        /// <param name="rect">矩形</param>
        /// <returns>字符</returns>
        public static String convertRectToStr(FCRect rect) {
            return String.Format("{0},{1},{2},{3}", rect.left, rect.top, rect.right, rect.bottom);
        }

        /// <summary>
        /// 大小转换为字符
        /// </summary>
        /// <param name="size">大小</param>
        /// <returns>字符</returns>
        public static String convertSizeToStr(FCSize size) {
            return String.Format("{0},{1}", size.cx, size.cy);
        }

        /// <summary>
        /// 字符转换为锚定信息
        /// </summary>
        /// <param name="str">字符</param>
        /// <returns>锚定信息</returns>
        public static FCAnchor convertStrToAnchor(String str) {
            str = str.ToLower();
            bool left = false, top = false, right = false, bottom = false;
            String[] strs = str.Split(',');
            for (int i = 0; i < strs.Length; i++) {
                String anchorStr = strs[i];
                if (anchorStr == "left") {
                    left = true;
                }
                else if (anchorStr == "top") {
                    top = true;
                }
                else if (anchorStr == "right") {
                    right = true;
                }
                else if (anchorStr == "bottom") {
                    bottom = true;
                }
            }
            FCAnchor anchor = new FCAnchor(left, top, right, bottom);
            return anchor;
        }

        /// <summary>
        /// 字符转换为布尔型
        /// </summary>
        /// <param name="str">字符</param>
        /// <returns>数值</returns>
        public static bool convertStrToBool(String str) {
            str = str.ToLower();
            return str == "true" ? true : false;
        }

        /// <summary>
        /// 字符转换为颜色
        /// </summary>
        /// <param name="str">字符</param>
        /// <returns>颜色</returns>
        public static long convertStrToColor(String str) {
            str = str.ToLower();
            if (str == "empty") {
                return FCColor.None;
            }
            else if (str == "control") {
                return FCColor.Back;
            }
            else if (str == "controlborder") {
                return FCColor.Border;
            }
            else if (str == "controltext") {
                return FCColor.Text;
            }
            else if (str == "disabledcontrol") {
                return FCColor.DisabledBack;
            }
            else if (str == "disabledcontroltext") {
                return FCColor.DisabledText;
            }
            else {
                String[] strs = str.Split(',');
                int a = 255, r = 255, g = 255, b = 255;
                if (strs.Length == 3) {
                    int.TryParse(strs[0], out r);
                    int.TryParse(strs[1], out g);
                    int.TryParse(strs[2], out b);
                    return FCColor.argb(r, g, b);
                }
                else if (strs.Length == 4) {
                    int.TryParse(strs[0], out a);
                    int.TryParse(strs[1], out r);
                    int.TryParse(strs[2], out g);
                    int.TryParse(strs[3], out b);
                    return FCColor.argb(a, r, g, b);
                }
            }
            return FCColor.None;
        }

        /// <summary>
        /// 字符串转内容布局
        /// </summary>
        /// <param name="str">字符</param>
        /// <returns>字符串</returns>
        public static FCContentAlignment convertStrToContentAlignment(String str) {
            str = str.ToLower();
            FCContentAlignment contentAlignment = FCContentAlignment.MiddleCenter;
            if (str == "bottomcenter") {
                contentAlignment = FCContentAlignment.BottomCenter;
            }
            else if (str == "bottomleft") {
                contentAlignment = FCContentAlignment.BottomLeft;
            }
            else if (str == "bottomright") {
                contentAlignment = FCContentAlignment.BottomRight;
            }
            else if (str == "middlecenter") {
                contentAlignment = FCContentAlignment.MiddleCenter;
            }
            else if (str == "middleleft") {
                contentAlignment = FCContentAlignment.MiddleLeft;
            }
            else if (str == "middleright") {
                contentAlignment = FCContentAlignment.MiddleRight;
            }
            else if (str == "topcenter") {
                contentAlignment = FCContentAlignment.TopCenter;
            }
            else if (str == "topleft") {
                contentAlignment = FCContentAlignment.TopLeft;
            }
            else if (str == "topright") {
                contentAlignment = FCContentAlignment.TopRight;
            }
            return contentAlignment;
        }

        /// <summary>
        /// 字符转换为光标
        /// </summary>
        /// <param name="str">字符</param>
        /// <returns>光标</returns>
        public static FCCursors convertStrToCursor(String str) {
            str = str.ToLower();
            FCCursors cursor = FCCursors.Arrow;
            if (str == "appstarting") {
                cursor = FCCursors.AppStarting;
            }
            else if (str == "cross") {
                cursor = FCCursors.Cross;
            }
            else if (str == "hand") {
                cursor = FCCursors.Hand;
            }
            else if (str == "help") {
                cursor = FCCursors.Help;
            }
            else if (str == "ibeam") {
                cursor = FCCursors.IBeam;
            }
            else if (str == "no") {
                cursor = FCCursors.No;
            }
            else if (str == "sizeall") {
                cursor = FCCursors.SizeAll;
            }
            else if (str == "sizenesw") {
                cursor = FCCursors.SizeNESW;
            }
            else if (str == "sizens") {
                cursor = FCCursors.SizeNS;
            }
            else if (str == "sizenwse") {
                cursor = FCCursors.SizeNWSE;
            }
            else if (str == "sizewe") {
                cursor = FCCursors.SizeWE;
            }
            else if (str == "uparrow") {
                cursor = FCCursors.UpArrow;
            }
            else if (str == "waitcursor") {
                cursor = FCCursors.WaitCursor;
            }
            return cursor;
        }

        /// <summary>
        /// 字符转换为绑定信息
        /// </summary>
        /// <param name="str">字符</param>
        /// <returns>绑定信息</returns>
        public static FCDockStyle convertStrToDock(String str) {
            str = str.ToLower();
            FCDockStyle dock = FCDockStyle.None;
            if (str == "bottom") {
                dock = FCDockStyle.Bottom;
            }
            else if (str == "fill") {
                dock = FCDockStyle.Fill;
            }
            else if (str == "left") {
                dock = FCDockStyle.Left;
            }
            else if (str == "right") {
                dock = FCDockStyle.Right;
            }
            else if (str == "top") {
                dock = FCDockStyle.Top;
            }
            return dock;
        }

        /// <summary>
        /// 字符转换为双精度浮点型数值
        /// </summary>
        /// <param name="str">字符</param>
        /// <returns>数值</returns>
        public static double convertStrToDouble(String str) {
            double value = 0;
            double.TryParse(str, out value);
            return value;
        }

        /// <summary>
        /// 字符转换为单精度浮点型数值
        /// </summary>
        /// <param name="str">字符</param>
        /// <returns>数值</returns>
        public static float convertStrToFloat(String str) {
            float value = 0;
            float.TryParse(str, out value);
            return value;
        }

        /// <summary>
        /// 字符转换为字体
        /// </summary>
        /// <param name="str">字符</param>
        /// <returns>字体</returns>
        public static FCFont convertStrToFont(String str) {
            String[] strs = str.Split(',');
            int size = strs.Length;
            String fontFamily = "SimSun";
            float fontSize = 12;
            bool bold = false;
            bool underline = false;
            bool italic = false;
            bool strikeout = false;
            if (size >= 1) {
                fontFamily = strs[0];
            }
            if (size >= 2) {
                float.TryParse(strs[1], out fontSize);
            }
            for (int i = 2; i < size; i++) {
                String subStr = strs[i].ToLower();
                if (subStr == "bold") {
                    bold = true;
                }
                else if (subStr == "underline") {
                    underline = true;
                }
                else if (subStr == "italic") {
                    italic = true;
                }
                else if (subStr == "strikeout") {
                    strikeout = true;
                }
            }
            return new FCFont(fontFamily, fontSize, bold, underline, italic, strikeout);
        }

        /// <summary>
        /// 字符转换为横向排列方式
        /// </summary>
        /// <param name="str">字符</param>
        /// <returns>字符</returns>
        public static FCHorizontalAlign convertStrToHorizontalAlign(String str) {
            str = str.ToLower();
            FCHorizontalAlign horizontalAlign = FCHorizontalAlign.Center;
            if (str == "right") {
                horizontalAlign = FCHorizontalAlign.Right;
            }
            else if (str == "inherit") {
                horizontalAlign = FCHorizontalAlign.Inherit;
            }
            else if (str == "left") {
                horizontalAlign = FCHorizontalAlign.Left;
            }
            return horizontalAlign;
        }

        /// <summary>
        /// 字符转换为整型数值
        /// </summary>
        /// <param name="str">字符</param>
        /// <returns>数值</returns>
        public static int convertStrToInt(String str) {
            int value = 0;
            int.TryParse(str, out value);
            return value;
        }

        /// <summary>
        /// 布局样式转换为字符
        /// </summary>
        /// <param name="str">字符</param>
        /// <returns>布局样式</returns>
        public static FCLayoutStyle convertStrToLayoutStyle(String str) {
            str = str.ToLower();
            FCLayoutStyle layoutStyle = FCLayoutStyle.None;
            if (str == "bottomtotop") {
                layoutStyle = FCLayoutStyle.BottomToTop;
            }
            else if (str == "lefttoright") {
                layoutStyle = FCLayoutStyle.LeftToRight;
            }
            else if (str == "righttoleft") {
                layoutStyle = FCLayoutStyle.RightToLeft;
            }
            else if (str == "toptobottom") {
                layoutStyle = FCLayoutStyle.TopToBottom;
            }
            return layoutStyle;
        }

        /// <summary>
        /// 字符转换位长整型数值
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>长整型</returns>
        public static long convertStrToLong(String str) {
            long value = 0;
            long.TryParse(str, out value);
            return value;
        }

        /// <summary>
        /// 字符转换为边距
        /// </summary>
        /// <param name="str">字符</param>
        /// <returns>边距</returns>
        public static FCPadding convertStrToPadding(String str) {
            int left = 0, top = 0, right = 0, bottom = 0;
            String[] strs = str.Split(',');
            if (strs.Length == 4) {
                int.TryParse(strs[0], out left);
                int.TryParse(strs[1], out top);
                int.TryParse(strs[2], out right);
                int.TryParse(strs[3], out bottom);
            }
            return new FCPadding(left, top, right, bottom);
        }

        /// <summary>
        /// 字符转换为坐标
        /// </summary>
        /// <param name="str">字符</param>
        /// <returns>坐标</returns>
        public static FCPoint convertStrToPoint(String str) {
            int x = 0, y = 0;
            String[] strs = str.Split(',');
            if (strs.Length == 2) {
                int.TryParse(strs[0], out x);
                int.TryParse(strs[1], out y);
            }
            return new FCPoint(x, y);
        }

        /// <summary>
        /// 字符转换为矩形
        /// </summary>
        /// <param name="str">字符</param>
        /// <returns>矩形</returns>
        public static FCRect convertStrToRect(String str) {
            int left = 0, top = 0, right = 0, bottom = 0;
            String[] strs = str.Split(',');
            if (strs.Length == 4) {
                int.TryParse(strs[0], out left);
                int.TryParse(strs[1], out top);
                int.TryParse(strs[2], out right);
                int.TryParse(strs[3], out bottom);
            }
            return new FCRect(left, top, right, bottom);
        }

        /// <summary>
        /// 字符转换为大小
        /// </summary>
        /// <param name="str">字符</param>
        /// <returns>大小</returns>
        public static FCSize convertStrToSize(String str) {
            int cx = 0, cy = 0;
            String[] strs = str.Split(',');
            if (strs.Length == 2) {
                int.TryParse(strs[0], out cx);
                int.TryParse(strs[1], out cy);
            }
            return new FCSize(cx, cy);
        }

        /// <summary>
        /// 字符转换为纵向排列方式
        /// </summary>
        /// <param name="str">字符</param>
        /// <returns>字符</returns>
        public static FCVerticalAlign convertStrToVerticalAlign(String str) {
            str = str.ToLower();
            FCVerticalAlign verticalAlign = FCVerticalAlign.Middle;
            if (str == "bottom") {
                verticalAlign = FCVerticalAlign.Bottom;
            }
            else if (str == "inherit") {
                verticalAlign = FCVerticalAlign.Inherit;
            }
            else if (str == "top") {
                verticalAlign = FCVerticalAlign.Top;
            }
            return verticalAlign;
        }

        /// <summary>
        /// 纵向排列方式转换为字符
        /// </summary>
        /// <param name="verticalAlign">横向排列方式</param>
        /// <returns>字符</returns>
        public static String convertVerticalAlignToStr(FCVerticalAlign verticalAlign) {
            String str = "";
            if (verticalAlign == FCVerticalAlign.Bottom) {
                str = "Bottom";
            }
            else if (verticalAlign == FCVerticalAlign.Inherit) {
                str = "Inherit";
            }
            else if (verticalAlign == FCVerticalAlign.Middle) {
                str = "Middle";
            }
            else if (verticalAlign == FCVerticalAlign.Top) {
                str = "Top";
            }
            return str;
        }

        public static double getDateNum(int tm_year, int tm_mon, int tm_mday, int tm_hour, int tm_min, int tm_sec, int tm_msec) {
            return (new DateTime(tm_year, tm_mon, tm_mday, tm_hour, tm_min, tm_sec) - new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public static void getDateByNum(double num, ref int tm_year, ref int tm_mon, ref int tm_mday, ref int tm_hour, ref int tm_min, ref int tm_sec, ref int tm_msec) {
            DateTime date = new DateTime(new DateTime(1970, 1, 1).Ticks + (long)num * 10000000);
            tm_year = date.Year;
            tm_mon = date.Month;
            tm_mday = date.Day;
            tm_hour = date.Hour;
            tm_min = date.Minute;
            tm_sec = date.Second;
        }

        /// <summary>
        /// 获取Guid
        /// </summary>
        /// <returns>Guid</returns>
        public static String getGuid() {
            return Guid.NewGuid().ToString();
        }

        /*格式化字符串*/
        private const String FORMATSTRING_N = "0";
        private const String FORMATSTRING_N1 = "0.0";
        private const String FORMATSTRING_N2 = "0.00";
        private const String FORMATSTRING_N3 = "0.000";
        private const String FORMATSTRING_N4 = "0.0000";

        /// <summary>
        /// 根据保留小数的位置将double型转化为String型
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="digit">保留小数点</param>
        /// <returns>数值字符</returns>
        public static String getValueByDigit(double value, int digit) {
            //保留超过4位时
            if (digit > 4) {
                StringBuilder format = new StringBuilder();
                format.Append("0");
                if (digit > 0) {
                    format.Append(".");
                    for (int i = 0; i < digit; i++) {
                        format.Append("0");
                    }
                }
                return value.ToString(format.ToString());
            }
            else {
                //4位及以下
                switch (digit) {
                    case 1:
                        return value.ToString(FORMATSTRING_N1);
                    case 2:
                        return value.ToString(FORMATSTRING_N2);
                    case 3:
                        return value.ToString(FORMATSTRING_N3);
                    case 4:
                        return value.ToString(FORMATSTRING_N4);
                    default:
                        return value.ToString(FORMATSTRING_N);
                }
            }
        }

        /// <summary>
        /// 安全的Double转Float
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="digit">保留小数位数</param>
        /// <returns>Float</returns>
        public static float safeDoubleToFloat(double value, int digit) {
            String str = getValueByDigit(value, digit);
            return convertStrToFloat(str);
        }
    }
}