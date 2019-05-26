/*****************************************************************************\
*                                                                             *
* CMathLib.cs - C++ Math functions                                            *
*                                                                             *
*               Version 4.00 ★★★★★                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's OwChart. All rights reserved. *
*                                                                             *
*******************************************************************************/

using System;
using System.Runtime.InteropServices;

namespace OwLib
{
    /// <summary>
    /// 数学运算方法接口核心库专用
    /// </summary>
    internal class CMathLib
    {
        #region Lord 2016/9/22
        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M008(double[] list, int length);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M009(double[] list, int length);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int M012(double min, double max, int yLen, int maxSpan, int minSpan, int defCount, ref double step, ref int digit);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M016(double close, double open);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M017(double close);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M018(double close, double high, double low);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M019(double volume);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M020(double []list, int length);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M021(double []list, int length);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M022(double []list, int length);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M023(double []list, int length);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M024(double []list, int length);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M025(double []list, int length);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M026(double []list, int length);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M027(double []list, int length);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M028(int x, int y, double k, double b);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M029(double close, int m);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M030(double close, int m, int n);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M031(double high, double low);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M032(double close, double volume);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M033(double up, double down);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M034(double ma);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M035(double ema);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M036(double tma);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M037(double ama);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M038(double sma);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M039(double sd);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M040(int x1, int y1, int x2, int y2);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M041(int x1, int y1, int x2, int y2, int x3, int y3);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M042(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M043(int index);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M044(int index);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M045(double []list1, int length1, double []list2, int length2);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M046(double []list1, int length1, double []list2, int length2);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M047(double []list1, int length1, double []list2, int length2);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M048(double []list1, int length1, double []list2, int length2);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M049(double []list1, int length1, double []list2, int length2);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M050(double []list1, int length1, double []list2, int length2);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M051(double []list1, int length1, double []list2, int length2);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M052(double []list1, int length1, double []list2, int length2);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M053(double []list1, int length1, double []list2, int length2);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M054(int x, int y, double k, double b);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M055(int x, int y, double k, double b);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M056(int x, int y, double k, double b);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M057(double close);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M058(double close);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M059(double close);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M060(double close);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M061(double close);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M062(double close);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M063(double close, double open);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M064(double close, double open);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M065(double close, double open);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M066(double close, double open);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M067(double close, double open);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M068(double close, double open);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M069(double close, double open);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M070(double close, double open);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M071(double close, double open);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M072(double up, double down);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M073(double up, double down);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M074(double up, double down);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M075(double up, double down);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M076(double up, double down);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M077(double up, double down);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M078(double up, double down);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M079(int x1, int y1, int x2, int y2);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M080(int x1, int y1, int x2, int y2);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M081(int x1, int y1, int x2, int y2);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M082(int x1, int y1, int x2, int y2);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M083(int x, int y, double k, double b);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M084(int x, int y, double k, double b);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M085(int x, int y, double k, double b);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M086(double close, double high, double low, double open);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M087(double close, double high, double low, double open);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M088(double close, double high, double low, double open);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M089(double close, double high, double low, double open);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M090(double close, double high, double low, double open);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M091(float x1, float y1, float x2, float y2, float oX, float oY);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M092(double close, double high, double low, double open);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M093(int n, int weight, double value, double lastWMA);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M094(int n, int weight, double value, double lastWMA);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M095(int n, int weight, double value, double lastWMA);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M096(int n, double value, double lastEMA);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M097(int n, double value, double lastEMA);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M098(int n, double value, double lastEMA);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M099(int index, int n, double value);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M100(int index, int n, double value);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M101(int index, int n, double value);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M102(int index, int n, double value);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M103(int n, int weight, double value, double lastWMA);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M104(int n, int weight, double value, double lastWMA);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern void M105(int x1, int y1, int x2, int y2, ref int x, ref int y, ref int w, ref int h);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M106(float x1, float y1, float x2, float y2, float oX, float oY);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void M107(float x1, float y1, float x2, float y2, float oX, float oY, ref float k, ref float b);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void M108(float width, float height, ref float a, ref float b);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool M109(float x, float y, float oX, float oY, float a, float b);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void M110(float x1, float y1, float x2, float y2, float x3, float y3, ref float oX, ref float oY, ref float r);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M111();

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M114(int index);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M115(int index);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M116(int index);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M117(int index);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M119(int x);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M120(int y);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M121(int x, int y);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M122(int x, int y, int width);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M123(int slope);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M125(float x1, float y1, float x2, float y2);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M126(int tm_hour, int tm_min, int tm_sec);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M127(int tm_year, int tm_mon, int tm_mday);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M128(double num);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M129(int tm_year, int tm_mon, int tm_mday, int tm_hour, int tm_min, int tm_sec, int tm_msec);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern void M130(double num, ref int tm_year, ref int tm_mon, ref int tm_mday, ref int tm_hour, ref int tm_min, ref int tm_sec, ref int tm_msec);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int M131();

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void M132(ref RECT bounds, ref SIZE parentSize, ref SIZE oldSize, bool anchorLeft, bool anchorTop, bool anchorRight, bool anchorBottom);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void M133(ref RECT bounds, ref RECT spaceRect, ref SIZE cSize, int dock);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void M135(int layoutStyle, ref RECT bounds, ref PADDING padding,
        ref PADDING margin, int left, int top, int width, int height,
         int tw, int th, ref POINT headerLocation);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool M136(ref RECT bounds, int rowHeight, int scrollV, double visiblePercent, int cell, int floor);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void M137(int resizePoint, ref int left, ref int top, ref int right, ref int bottom, ref POINT nowPoint, ref POINT startMousePoint);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool M138(int indexTop, int indexBottom, int cell, int floor, int lineHeight, double visiblePercent);
        #endregion
    }
}
