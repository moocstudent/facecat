/*基于捂脸猫FaceCat框架 v1.0
 捂脸猫创始人-矿洞程序员-脉脉KOL-陶德 (微信号:suade1984);
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace FaceCat
{
    /// <summary>
    /// 数据中心
    /// </summary>
    public class DataCenter
    {
        private static UserCookieService m_userCookieService = new UserCookieService();

        /// <summary>
        /// 获取用户Cookie服务
        /// </summary>
        public static UserCookieService UserCookieService
        {
            get { return m_userCookieService; }
        }


        /// <summary>
        /// 获取程序路径
        /// </summary>
        /// <returns>程序路径</returns>
        public static String GetAppPath()
        {
            return Application.StartupPath;
        }

        /// <summary>
        /// 用户数据存储路径
        /// </summary>
        /// <returns>程序路径</returns>
        public static String GetUserPath()
        {
            String userPath = Environment.GetEnvironmentVariable("LOCALAPPDATA");
            if (!FCFile.isDirectoryExist(userPath))
            {
                userPath = GetAppPath();
            }
            else
            {
                userPath += "\\idesigner";
                if (!FCFile.isDirectoryExist(userPath))
                {
                    FCFile.createDirectory(userPath);
                }
            }
            return userPath;
        }
    }
}
