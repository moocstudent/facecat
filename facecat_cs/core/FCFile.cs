/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace FaceCat
{
    /// <summary>
    /// 文件操作类
    /// </summary>
    public class FCFile
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(String lpPathName, int iReadWrite);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);

        public const int OF_READWRITE = 2;
        public const int OF_SHARE_DENY_NONE = 0x40;

        /// <summary>
        /// 向文件中追加内容
        /// </summary>
        /// <param name="file">文件</param>
        /// <param name="content">内容</param>
        /// <returns>是否成功</returns>
        public static bool append(String file, String content)
        {
            try
            {
                FileStream fs = new FileStream(file, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                sw.Write(content);
                sw.Close();
                fs.Dispose();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="dir">文件夹</param>
        public static void createDirectory(String dir)
        {
            Directory.CreateDirectory(dir);
        }

        /// <summary>
        /// 获取文件夹中的文件夹
        /// </summary>
        /// <param name="dir">文件夹</param>
        /// <param name="dirs">文件夹集合</param>
        /// <returns></returns>
        public static bool getDirectories(String dir, ArrayList<String> dirs)
        {
            if (Directory.Exists(dir))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dir);
                DirectoryInfo[] lstDir = dirInfo.GetDirectories();
                int lstDirSize = lstDir.Length;
                if (lstDirSize > 0)
                {
                    for (int i = 0; i < lstDirSize; i++)
                    {
                        dirs.add(lstDir[i].FullName);
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取文件夹中的文件
        /// </summary>
        /// <param name="dir">文件夹</param>
        /// <param name="files">文件集合</param>
        /// <returns>是否成功</returns>
        public static bool getFiles(String dir, ArrayList<String> files)
        {
            if (Directory.Exists(dir))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dir);
                FileInfo[] lstFile = dirInfo.GetFiles();
                int lstFileSize = lstFile.Length;
                if (lstFileSize > 0)
                {
                    for (int i = 0; i < lstFileSize; i++)
                    {
                        files.add(lstFile[i].FullName);
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断文件夹是否存在
        /// </summary>
        /// <param name="dir">文件夹</param>
        /// <returns>是否存在</returns>
        public static bool isDirectoryExist(String dir)
        {
            return Directory.Exists(dir);
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="file">文件</param>
        /// <returns>是否存在</returns>
        public static bool isFileExist(String file)
        {
            return File.Exists(file);
        }

        /// <summary>
        /// 从文件中读取内容
        /// </summary>
        /// <param name="file">文件</param>
        /// <param name="content">返回内容</param>
        /// <returns>是否成功</returns>
        public static bool read(String file, ref String content)
        {
            try
            {
                if (File.Exists(file))
                {
                    FileStream fs = new FileStream(file, FileMode.Open);
                    StreamReader sr = new StreamReader(fs, Encoding.Default);
                    content = sr.ReadToEnd();
                    sr.Close();
                    fs.Dispose();
                    return true;
                }
            }
            catch { }
            return false;
        }

        /// <summary>
        /// 移除文件
        /// </summary>
        /// <param name="file">文件</param>
        /// <returns>是否成功</returns>
        public static bool removeFile(String file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 向文件中写入内容
        /// </summary>
        /// <param name="file">文件</param>
        /// <param name="content">内容</param>
        /// <returns>是否成功</returns>
        public static bool write(String file, String content)
        {
            try
            {
                FileStream fs = new FileStream(file, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                sw.Write(content);
                sw.Close();
                fs.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
