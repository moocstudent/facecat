using System;
using System.Runtime.InteropServices;

namespace OwLib
{
    /// <summary>
    /// 数学运算方法接口
    /// </summary>
    internal class PMathLib
    {
        #region Lord  2012/9/22
        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M007(double[] list, int length, double avg, double standardDeviation);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern void M014(double[] list, int length, ref float k, ref float b);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M008(double[] list, int length);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M009(double[] list, int length);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M010(double[] list, int length);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern double M106(float x1, float y1, float x2, float y2, float oX, float oY);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern void M107(float x1, float y1, float x2, float y2, float oX, float oY, ref float k, ref float b);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern void M108(float width, float height, ref float a, ref float b);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool M109(float x, float y, float oX, float oY, float a, float b);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern void M110(float x1, float y1, float x2, float y2, float x3, float y3, ref float oX, ref float oY, ref float r);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int M112(int index);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void M124(float x1, float y1, float x2, float y2, float x3, float y3, ref float x4, ref float y4);
        #endregion
    }
}
