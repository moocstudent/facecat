/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.core;

import java.util.*;
import java.util.*;
import java.util.zip.*;
import java.io.*;

/**
 * 字符串处理类
 */
public class FCStr {

    public static int m_ytab[][] = {{31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31}, {31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31}
    };

    /**
     * 锚定信息转化为字符
     *
     * @param anchor 锚定信息
     * @returns 字符
     */
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
                anchorStr += strs.get(i);
                if (i != size - 1) {
                    anchorStr += ",";
                }
            }
        } else {
            anchorStr = "None";
        }
        return anchorStr;
    }

    /**
     * 布尔型数值转换为字符
     *
     * @param value 数值
     * @returns 字符
     */
    public static String convertBoolToStr(boolean value) {
        return value ? "True" : "False";
    }

    /**
     * 颜色转换为字符
     *
     * @param color 颜色
     * @returns 字符
     */
    public static String convertColorToStr(long color) {
        if (color == FCColor.None) {
            return "Empty";
        } else if (color == FCColor.Back) {
            return "Control";
        } else if (color == FCColor.Border) {
            return "ControlBorder";
        } else if (color == FCColor.Text) {
            return "ControlText";
        } else if (color == FCColor.DisabledBack) {
            return "DisabledControl";
        } else if (color == FCColor.DisabledText) {
            return "DisabledControlText";
        }
        int a = 0, r = 0, g = 0, b = 0;
        RefObject<Integer> tempRef_a = new RefObject<Integer>(a);
        RefObject<Integer> tempRef_r = new RefObject<Integer>(r);
        RefObject<Integer> tempRef_g = new RefObject<Integer>(g);
        RefObject<Integer> tempRef_b = new RefObject<Integer>(b);
        FCColor.toArgb(null, color, tempRef_a, tempRef_r, tempRef_g, tempRef_b);
        a = tempRef_a.argvalue;
        r = tempRef_r.argvalue;
        g = tempRef_g.argvalue;
        b = tempRef_b.argvalue;
        String str = "";
        if (a == 255) {
            str = String.format("%d,%d,%d", r, g, b);
        } else {
            str = String.format("%1$s,%2$s,%3$s,%4$s", a, r, g, b);
        }
        return str;
    }

    /**
     * 内容布局转字符串
     *
     * @param contentAlignment 内容布局
     * @returns 字符串
     */
    public static String convertContentAlignmentToStr(FCContentAlignment contentAlignment) {
        String str = "";
        if (contentAlignment == FCContentAlignment.BottomCenter) {
            str = "BottomCenter";
        } else if (contentAlignment == FCContentAlignment.BottomLeft) {
            str = "BottomLeft";
        } else if (contentAlignment == FCContentAlignment.BottomRight) {
            str = "BottomRight";
        } else if (contentAlignment == FCContentAlignment.MiddleCenter) {
            str = "MiddleCenter";
        } else if (contentAlignment == FCContentAlignment.MiddleLeft) {
            str = "MiddleLeft";
        } else if (contentAlignment == FCContentAlignment.MiddleRight) {
            str = "MiddleRight";
        } else if (contentAlignment == FCContentAlignment.TopCenter) {
            str = "TopCenter";
        } else if (contentAlignment == FCContentAlignment.TopLeft) {
            str = "TopLeft";
        } else if (contentAlignment == FCContentAlignment.TopRight) {
            str = "TopRight";
        }
        return str;
    }

    public static double convertDateToNum(Calendar date) {
        int year = date.get(Calendar.YEAR);
        int month = date.get(Calendar.MONTH) + 1;
        int day = date.get(Calendar.DAY_OF_MONTH);
        int hour = date.get(Calendar.HOUR_OF_DAY);
        int minute = date.get(Calendar.MINUTE);
        int second = date.get(Calendar.SECOND);
        return getDataNum(year, month, day, hour, minute, second, 0);
    }

    /**
     * 绑定信息转换为字符
     *
     * @param dock 绑定信息
     * @returns 字符
     */
    public static String convertDockToStr(FCDockStyle dock) {
        String str = "";
        if (dock == FCDockStyle.Bottom) {
            str = "Bottom";
        } else if (dock == FCDockStyle.Fill) {
            str = "Fill";
        } else if (dock == FCDockStyle.Left) {
            str = "Left";
        } else if (dock == FCDockStyle.None) {
            str = "None";
        } else if (dock == FCDockStyle.Right) {
            str = "Right";
        } else if (dock == FCDockStyle.Top) {
            str = "Top";
        }
        return str;
    }

    /**
     * 双精度浮点型数值转换为字符
     *
     * @param value 数值
     * @returns 字符
     */
    public static String convertDoubleToStr(double value) {
        try {
            return (new Double(value)).toString();
        } catch (Exception ex) {
            return "";
        }
    }

    /**
     * 单精度浮动点型数值转换为字符
     *
     * @param value 数值
     * @returns 字符
     */
    public static String convertFloatToStr(float value) {
        try {
            return (new Float(value)).toString();
        } catch (Exception ex) {
            return "";
        }
    }

    /**
     * 字体转换为字符
     *
     * @param font 字体
     * @returns 字符
     */
    public static String convertFontToStr(FCFont font) {
        ArrayList<String> strs = new ArrayList<String>();
        strs.add(font.m_fontFamily);
        strs.add((new Float(font.m_fontSize)).toString());
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
                fontStr += strs.get(i);
                if (i != size - 1) {
                    fontStr += ",";
                }
            }
        }
        return fontStr;
    }

    /**
     * 横向排列方式转换为字符
     *
     * @param horizontalAlign 横向排列方式
     * @returns 字符
     */
    public static String convertHorizontalAlignToStr(FCHorizontalAlign horizontalAlign) {
        String str = "";
        if (horizontalAlign == FCHorizontalAlign.Center) {
            str = "Center";
        } else if (horizontalAlign == FCHorizontalAlign.Right) {
            str = "Right";
        } else if (horizontalAlign == FCHorizontalAlign.Inherit) {
            str = "Inherit";
        } else if (horizontalAlign == FCHorizontalAlign.Left) {
            str = "Left";
        }
        return str;
    }

    /**
     * 整型数值转换为字符
     *
     * @param value 数值
     * @returns 字符
     */
    public static String convertIntToStr(int value) {
        try {
            return (new Integer(value)).toString();
        } catch (Exception ex) {
            return "";
        }
    }

    /**
     * 布局样式转换为字符
     *
     * @param layoutStyle 布局样式转
     * @returns 布局样式
     */
    public static String convertLayoutStyleToStr(FCLayoutStyle layoutStyle) {
        String str = "";
        if (layoutStyle == FCLayoutStyle.BottomToTop) {
            str = "BottomToTop";
        } else if (layoutStyle == FCLayoutStyle.LeftToRight) {
            str = "LeftToRight";
        } else if (layoutStyle == FCLayoutStyle.None) {
            str = "None";
        } else if (layoutStyle == FCLayoutStyle.RightToLeft) {
            str = "RightToLeft";
        } else if (layoutStyle == FCLayoutStyle.TopToBottom) {
            str = "TopToBottom";
        }
        return str;
    }

    /**
     * 长整型数值转换为字符
     *
     * @param value 数值
     * @returns 字符串
     */
    public static String convertLongToStr(long value) {
        try {
            return (new Long(value)).toString();
        } catch (Exception ex) {
            return "";
        }
    }

    /**
     * 将数字格式转换为日期格式
     *
     * @param num 数字
     * @returns 日期格式
     */
    public static Calendar convertNumToDate(double num) {
        int year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0, ms = 0;
        RefObject<Integer> tempRef_year = new RefObject<Integer>(year);
        RefObject<Integer> tempRef_month = new RefObject<Integer>(month);
        RefObject<Integer> tempRef_day = new RefObject<Integer>(day);
        RefObject<Integer> tempRef_hour = new RefObject<Integer>(hour);
        RefObject<Integer> tempRef_minute = new RefObject<Integer>(minute);
        RefObject<Integer> tempRef_second = new RefObject<Integer>(second);
        RefObject<Integer> tempRef_ms = new RefObject<Integer>(ms);
        getDataByNum(num, tempRef_year, tempRef_month, tempRef_day, tempRef_hour, tempRef_minute, tempRef_second, tempRef_ms);
        year = tempRef_year.argvalue;
        month = tempRef_month.argvalue;
        day = tempRef_day.argvalue;
        hour = tempRef_hour.argvalue;
        minute = tempRef_minute.argvalue;
        second = tempRef_second.argvalue;
        ms = tempRef_ms.argvalue;
        Calendar calendar = new GregorianCalendar(year, month - 1, day, hour, minute, second);
        return calendar;
    }

    /**
     * 边距转换为字符
     *
     * @param padding 边距
     * @returns 字符
     */
    public static String convertPaddingToStr(FCPadding padding) {
        return String.format("%1$s,%2$s,%3$s,%4$s", padding.left, padding.top, padding.right, padding.bottom);
    }

    /**
     * 坐标转换为字符
     *
     * @param point 坐标
     * @returns 字符
     */
    public static String convertPointToStr(FCPoint FCPoint) {
        return String.format("%1$s,%2$s", FCPoint.x, FCPoint.y);
    }

    /**
     * 矩形转换为字符
     *
     * @param rect 矩形
     * @returns 字符
     */
    public static String convertRectToStr(FCRect rect) {
        return String.format("%1$s,%2$s,%3$s,%4$s", rect.left, rect.top, rect.right, rect.bottom);
    }

    /**
     * 大小转换为字符
     *
     * @param size 大小
     * @returns 字符
     */
    public static String convertSizeToStr(FCSize size) {
        return String.format("%1$s,%2$s", size.cx, size.cy);
    }

    /**
     * 字符转换为锚定信息
     *
     * @param str 字符
     * @returns 锚定信息
     */
    public static FCAnchor convertStrToAnchor(String str) {
        str = str.toLowerCase();
        boolean left = false, top = false, right = false, bottom = false;
        String[] strs = str.split("[,]");
        for (int i = 0; i < strs.length; i++) {
            String anchorStr = strs[i];
            if (anchorStr.equals("left")) {
                left = true;
            } else if (anchorStr.equals("top")) {
                top = true;
            } else if (anchorStr.equals("right")) {
                right = true;
            } else if (anchorStr.equals("bottom")) {
                bottom = true;
            }
        }
        FCAnchor anchor = new FCAnchor(left, top, right, bottom);
        return anchor;
    }

    /**
     * 字符转换为布尔型
     *
     * @param str 字符
     * @returns 数值
     */
    public static boolean convertStrToBool(String str) {
        str = str.toLowerCase();
        return str.equals("true") ? true : false;
    }

    /**
     * 字符转换为颜色
     *
     * @param str 字符
     * @returns 颜色
     */
    public static long convertStrToColor(String str) {
        str = str.toLowerCase();
        if (str.equals("empty")) {
            return FCColor.None;
        } else if (str.equals("control")) {
            return FCColor.Back;
        } else if (str.equals("controlborder")) {
            return FCColor.Border;
        } else if (str.equals("controltext")) {
            return FCColor.Text;
        } else if (str.equals("disabledcontrol")) {
            return FCColor.DisabledBack;
        } else if (str.equals("disabledcontroltext")) {
            return FCColor.DisabledText;
        } else {
            String[] strs = str.split("[,]");
            int a = 255, r = 255, g = 255, b = 255;
            if (strs.length == 3) {
                r = Integer.parseInt(strs[0]);
                g = Integer.parseInt(strs[1]);
                b = Integer.parseInt(strs[2]);
                return FCColor.argb(r, g, b);
            } else if (strs.length == 4) {
                a = Integer.parseInt(strs[0]);
                r = Integer.parseInt(strs[1]);
                g = Integer.parseInt(strs[2]);
                b = Integer.parseInt(strs[3]);
                return FCColor.argb(a, r, g, b);
            }
        }
        return FCColor.None;
    }

    /**
     * 字符串转内容布局
     *
     * @param str 字符
     * @returns 字符串
     */
    public static FCContentAlignment convertStrToContentAlignment(String str) {
        str = str.toLowerCase();
        FCContentAlignment contentAlignment = FCContentAlignment.MiddleCenter;
        if (str.equals("bottomcenter")) {
            contentAlignment = FCContentAlignment.BottomCenter;
        } else if (str.equals("bottomleft")) {
            contentAlignment = FCContentAlignment.BottomLeft;
        } else if (str.equals("bottomright")) {
            contentAlignment = FCContentAlignment.BottomRight;
        } else if (str.equals("middleleft")) {
            contentAlignment = FCContentAlignment.MiddleLeft;
        } else if (str.equals("middlecenter")) {
            contentAlignment = FCContentAlignment.MiddleCenter;
        } else if (str.equals("middleright")) {
            contentAlignment = FCContentAlignment.MiddleRight;
        } else if (str.equals("topcenter")) {
            contentAlignment = FCContentAlignment.TopCenter;
        } else if (str.equals("topleft")) {
            contentAlignment = FCContentAlignment.TopLeft;
        } else if (str.equals("topright")) {
            contentAlignment = FCContentAlignment.TopRight;
        }
        return contentAlignment;
    }

    /**
     * 字符转换为绑定信息
     *
     * @param str 字符
     * @returns 绑定信息
     */
    public static FCDockStyle convertStrToDock(String str) {
        str = str.toLowerCase();
        FCDockStyle dock = FCDockStyle.None;
        if (str.equals("bottom")) {
            dock = FCDockStyle.Bottom;
        } else if (str.equals("fill")) {
            dock = FCDockStyle.Fill;
        } else if (str.equals("left")) {
            dock = FCDockStyle.Left;
        } else if (str.equals("right")) {
            dock = FCDockStyle.Right;
        } else if (str.equals("top")) {
            dock = FCDockStyle.Top;
        }
        return dock;
    }

    /**
     * 字符转换为双精度浮点型数值
     *
     * @param str 字符
     * @returns 数值
     */
    public static double convertStrToDouble(String str) {
        try {
            return Double.parseDouble(str);
        } catch (Exception ex) {
            return 0;
        }
    }

    /**
     * 字符转换为单精度浮点型数值
     *
     * @param str 字符
     * @returns 数值
     */
    public static float convertStrToFloat(String str) {
        try {
            return Float.parseFloat(str);
        } catch (Exception ex) {
            return 0;
        }
    }

    /**
     * 字符转换为字体
     *
     * @param str 字符
     * @returns 字体
     */
    public static FCFont convertStrToFont(String str) {
        String[] strs = str.split("[,]");
        int size = strs.length;
        String fontFamily = "SimSun";
        float fontSize = 12;
        boolean bold = false;
        boolean underline = false;
        boolean italic = false;
        boolean strikeout = false;
        if (size >= 1) {
            fontFamily = strs[0];
        }
        if (size >= 2) {
            fontSize = convertStrToFloat(strs[1]);
        }
        for (int i = 2; i < size; i++) {
            String subStr = strs[i].toLowerCase();
            if (subStr.equals("bold")) {
                bold = true;
            } else if (subStr.equals("underline")) {
                underline = true;
            } else if (subStr.equals("italic")) {
                italic = true;
            } else if (subStr.equals("")) {
                strikeout = true;
            }
        }
        return new FCFont(fontFamily, fontSize, bold, underline, italic, strikeout);
    }

    /**
     * 字符转换为横向排列方式
     *
     * @param str 字符
     * @returns 字符
     */
    public static FCHorizontalAlign convertStrToHorizontalAlign(String str) {
        str = str.toLowerCase();
        FCHorizontalAlign horizontalAlign = FCHorizontalAlign.Center;
        if (str.equals("right")) {
            horizontalAlign = FCHorizontalAlign.Right;
        } else if (str.equals("inherit")) {
            horizontalAlign = FCHorizontalAlign.Inherit;
        } else if (str.equals("left")) {
            horizontalAlign = FCHorizontalAlign.Left;
        }
        return horizontalAlign;
    }

    /**
     * 字符转换为整型数值
     *
     * @param str 字符
     * @returns 数值
     */
    public static int convertStrToInt(String str) {
        try {
            return Integer.parseInt(str);
        } catch (Exception ex) {
            return 0;
        }
    }

    /**
     * 布局样式转换为字符
     *
     * @param str 字符
     * @returns 布局样式
     */
    public static FCLayoutStyle convertStrToLayoutStyle(String str) {
        str = str.toLowerCase();
        FCLayoutStyle layoutStyle = FCLayoutStyle.None;
        if (str.equals("bottomtotop")) {
            layoutStyle = FCLayoutStyle.BottomToTop;
        } else if (str.equals("lefttoright")) {
            layoutStyle = FCLayoutStyle.LeftToRight;
        } else if (str.equals("righttoleft")) {
            layoutStyle = FCLayoutStyle.RightToLeft;
        } else if (str.equals("toptobottom")) {
            layoutStyle = FCLayoutStyle.TopToBottom;
        }
        return layoutStyle;
    }

    /**
     * 字符转换位长整型数值
     *
     * @param str 字符串
     * @returns 长整型
     */
    public static long convertStrToLong(String str) {
        try {
            return Long.parseLong(str);
        } catch (Exception ex) {
            return 0;
        }
    }

    /**
     * 字符转换为边距
     *
     * @param str 字符
     * @returns 边距
     */
    public static FCPadding convertStrToPadding(String str) {
        int left = 0, top = 0, right = 0, bottom = 0;
        String[] strs = str.split("[,]");
        if (strs.length == 4) {
            left = Integer.parseInt(strs[0]);
            top = Integer.parseInt(strs[1]);
            right = Integer.parseInt(strs[2]);
            bottom = Integer.parseInt(strs[3]);
        }
        return new FCPadding(left, top, right, bottom);
    }

    /**
     * 字符转换为坐标
     *
     * @param str 字符
     * @returns 坐标
     */
    public static FCPoint convertStrToPoint(String str) {
        int x = 0, y = 0;
        String[] strs = str.split("[,]");
        if (strs.length == 2) {
            x = Integer.parseInt(strs[0]);
            y = Integer.parseInt(strs[1]);
        }
        return new FCPoint(x, y);
    }

    /**
     * 字符转换为矩形
     *
     * @param str 字符
     * @returns 矩形
     */
    public static FCRect convertStrToRect(String str) {
        int left = 0, top = 0, right = 0, bottom = 0;
        String[] strs = str.split("[,]");
        if (strs.length == 4) {
            left = Integer.parseInt(strs[0]);
            top = Integer.parseInt(strs[1]);
            right = Integer.parseInt(strs[2]);
            bottom = Integer.parseInt(strs[3]);
        }
        return new FCRect(left, top, right, bottom);
    }

    /**
     * 字符转换为大小
     *
     * @param str 字符
     * @returns 大小
     */
    public static FCSize convertStrToSize(String str) {
        int cx = 0, cy = 0;
        String[] strs = str.split("[,]");
        if (strs.length == 2) {
            cx = Integer.parseInt(strs[0]);
            cy = Integer.parseInt(strs[1]);
        }
        return new FCSize(cx, cy);
    }

    /**
     * 字符转换为纵向排列方式
     *
     * @param str 字符
     * @returns 字符
     */
    public static FCVerticalAlign convertStrToVerticalAlign(String str) {
        str = str.toLowerCase();
        FCVerticalAlign verticalAlign = FCVerticalAlign.Middle;
        if (str.equals("bottom")) {
            verticalAlign = FCVerticalAlign.Bottom;
        } else if (str.equals("inherit")) {
            verticalAlign = FCVerticalAlign.Inherit;
        } else if (str.equals("top")) {
            verticalAlign = FCVerticalAlign.Top;
        }
        return verticalAlign;
    }

    /**
     * 纵向排列方式转换为字符
     *
     * @param verticalAlign 横向排列方式
     * @returns 字符
     */
    public static String convertVerticalAlignToStr(FCVerticalAlign verticalAlign) {
        String str = "";
        if (verticalAlign == FCVerticalAlign.Bottom) {
            str = "Bottom";
        } else if (verticalAlign == FCVerticalAlign.Inherit) {
            str = "Inherit";
        } else if (verticalAlign == FCVerticalAlign.Middle) {
            str = "Middle";
        } else if (verticalAlign == FCVerticalAlign.Top) {
            str = "Top";
        }
        return str;
    }

    public static String getAppPath() {
        return System.getProperty("user.dir");
    }

    public static double getDataNum(int tm_year, int tm_mon, int tm_mday, int tm_hour, int tm_min, int tm_sec, int tm_msec) {
        double tn = 0;
        if ((tm_mon -= 2) <= 0) {
            tm_mon += 12;
            tm_year -= 1;
        }
        double year = tm_year / 4 - tm_year / 100 + tm_year / 400 + 367 * tm_mon / 12 + tm_mday;
        double month = tm_year * 365 - 719499;
        tn = ((year + month) * 24 * 60 * 60) + tm_hour * 3600 + tm_min * 60 + tm_sec;
        return (long) tn + tm_msec / 1000;
    }

    public static void getDataByNum(double num, RefObject<Integer> tm_year, RefObject<Integer> tm_mon, RefObject<Integer> tm_mday, RefObject<Integer> tm_hour, RefObject<Integer> tm_min, RefObject<Integer> tm_sec, RefObject<Integer> tm_msec) {
        long tn = (long) num;
        int dayclock = (int) (tn % 86400);
        int dayno = (int) (tn / 86400);
        int year = 1970;
        tm_sec.argvalue = dayclock % 60;
        tm_min.argvalue = (dayclock % 3600) / 60;
        tm_hour.argvalue = dayclock / 3600;
        int yearSize = 0;
        while (dayno >= (yearSize = (isLeapYear(year) == 1 ? 366 : 365))) {
            dayno -= yearSize;
            year++;
        }
        tm_year.argvalue = year;
        int month = 0;
        while (dayno >= m_ytab[isLeapYear(year)][month]) {
            dayno -= m_ytab[isLeapYear(year)][month];
            month++;
        }
        tm_mon.argvalue = month + 1;
        tm_mday.argvalue = dayno + 1;
        tm_msec.argvalue = (int) ((num * 1000 - Math.floor(num) * 1000));
    }

    /**
     * 获取Guid
     *
     * @returns Guid
     */
    public static String getGuid() {
        return UUID.randomUUID().toString();
    }

    /**
     * 根据保留小数的位置将double型转化为String型
     *
     * @param value 值
     * @param digit 保留小数点
     * @returns 数值字符
     */
    public static String getValueByDigit(double value, int digit) {
        if (digit > 0) {
            String format = String.format("%d", digit);
            format = "%." + format + "f";
            String str = String.format(format, value);
            return str;
        } else {
            return String.format("%d", (int) value);
        }
    }

    public static byte[] gZip(byte[] data) {
        byte[] b = null;
        try {
            ByteArrayOutputStream bos = new ByteArrayOutputStream();
            GZIPOutputStream gzip = new GZIPOutputStream(bos);
            gzip.write(data);
            gzip.finish();
            gzip.close();
            b = bos.toByteArray();
            bos.close();
        } catch (Exception ex) {
            ex.printStackTrace();
        }
        return b;
    }

    public static int isLeapYear(int year) {
        return ((year % 4 == 0 && year % 100 != 0) || (year % 400 == 0)) ? 1 : 0;
    }

    /**
     * 安全的Double转Float
     *
     * @param value 值
     * @param digit 保留小数位数
     * @returns Float
     */
    public static float safeDoubleToFloat(double value, int digit) {
        String str = getValueByDigit(value, digit);
        return convertStrToFloat(str);
    }

    public static byte[] unGZip(byte[] data) {
        byte[] b = null;
        try {
            ByteArrayInputStream bis = new ByteArrayInputStream(data);
            GZIPInputStream gzip = new GZIPInputStream(bis);
            byte[] buf = new byte[1024];
            int num = -1;
            ByteArrayOutputStream baos = new ByteArrayOutputStream();
            while ((num = gzip.read(buf, 0, buf.length)) != -1) {
                baos.write(buf, 0, num);
            }
            b = baos.toByteArray();
            baos.flush();
            baos.close();
            gzip.close();
            bis.close();
        } catch (Exception ex) {
            ex.printStackTrace();
        }
        return b;
    }

    public static byte[] unZip(byte[] data) {
        byte[] b = null;
        try {
            ByteArrayInputStream bis = new ByteArrayInputStream(data);
            ZipInputStream zip = new ZipInputStream(bis);
            while (zip.getNextEntry() != null) {
                byte[] buf = new byte[1024];
                int num = -1;
                ByteArrayOutputStream baos = new ByteArrayOutputStream();
                while ((num = zip.read(buf, 0, buf.length)) != -1) {
                    baos.write(buf, 0, num);
                }
                b = baos.toByteArray();
                baos.flush();
                baos.close();
            }
            zip.close();
            bis.close();
        } catch (Exception ex) {
            ex.printStackTrace();
        }
        return b;
    }

    public static byte[] zip(byte[] data) {
        byte[] b = null;
        try {
            ByteArrayOutputStream bos = new ByteArrayOutputStream();
            ZipOutputStream zip = new ZipOutputStream(bos);
            ZipEntry entry = new ZipEntry("zip");
            entry.setSize(data.length);
            zip.putNextEntry(entry);
            zip.write(data);
            zip.closeEntry();
            zip.close();
            b = bos.toByteArray();
            bos.close();
        } catch (Exception ex) {
            ex.printStackTrace();
        }
        return b;
    }
}
