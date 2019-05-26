using System;
using System.Runtime.InteropServices;

namespace OwLib
{
    /// <summary>
    /// 数学运算方法接口指标专用
    /// </summary>
    internal class IMathLib
    {
        #region Lord 2016/9/22
        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern void M001(int index, int n, double s, double m, double high, double low, double hhv, double llv, int last_state,
double last_sar, double last_af, ref int state, ref double af, ref double sar);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M002(double value, double[] list, int length, double avg);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M003(int index, int n, double value, LPDATA last_MA);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M004(int index, int n, double value, LPDATA last_SUM);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M005(int n, int weight, double value, double lastSMA);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M006(int n, double value, double lastEMA);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M007(double[] list, int length, double avg, double standardDeviation);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M008(double[] list, int length);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M009(double[] list, int length);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern void M013(int index, double close, double p, ref double sxp, ref int sxi, ref double exp, ref int exi, ref int state,
		ref int cStart,ref int cEnd,ref double k,ref double b);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M015(double close, double lastSma, int n, int m);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M129(int tm_year, int tm_mon, int tm_mday, int tm_hour, int tm_min, int tm_sec, int tm_msec);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern void M130(double num, ref int tm_year, ref int tm_mon, ref int tm_mday, ref int tm_hour, ref int tm_min, ref int tm_sec, ref int tm_msec);
        #endregion
    }
}
