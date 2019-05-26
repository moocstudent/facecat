/*基于捂脸猫FaceCat框架 v1.0
 捂脸猫创始人-矿洞程序员-脉脉KOL-陶德 (微信号:suade1984);
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace FaceCat {
    static class Program {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(String[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.Run(new MainForm());
        }

        /// <summary>
        /// 当前域的异常
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="e">参数</param>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
        }

        /// <summary>
        /// 程序异常
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="e">参数</param>
        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e) {
        }
    }
}