using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace OwLib
{
    /// <summary>
    /// 数据中心
    /// </summary>
    public class DataCenter
    {
        /// <summary>
        /// 获取程序路径
        /// </summary>
        /// <returns>程序路径</returns>
        public static String GetAppPath()
        {
            return Application.StartupPath;
        }
    }
}
