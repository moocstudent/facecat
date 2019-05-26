/*基于捂脸猫FaceCat框架 v1.0
 捂脸猫创始人-矿洞程序员-脉脉KOL-陶德 (微信号:suade1984);
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;
using FaceCat;

namespace FaceCat
{
    /// <summary>
    /// 主窗体
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// 创建窗体
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            //创建XML解析器
            m_designer = new Designer();
            //链接控件库
            m_designer.createNative();
            m_designer.Script = new DesignerScript(m_designer);
            m_native = m_designer.Native;
            m_native.Paint = new GdiPlusPaintEx();
            m_native.Host = new WinHostEx();
            m_native.Host.Native = m_native;
            m_native.ResourcePath = WinHostEx.getAppPath() + "\\config";
            m_host = m_native.Host as WinHostEx;
            m_host.HWnd = Handle;
            //设置尺寸
            m_native.AllowScaleSize = true;
            m_native.DisplaySize = new FCSize(ClientSize.Width, ClientSize.Height);
            m_designer.resetScaleSize(getClientSize());
            m_native.invalidate();
            //加载Xml
            m_host.LoadingDesigner = true;
            m_designer.load(Path.Combine(DataCenter.GetAppPath(), "config\\MainFrame.html"));
            m_host.LoadingDesigner = false;
            
        }

        /// <summary>
        /// 设计器
        /// </summary>
        private Designer m_designer;

        private WinHostEx m_host;

        /// <summary>
        /// 获取或设置设备管理器
        /// </summary>
        public WinHostEx Host
        {
            get { return m_host; }
            set { m_host = value; }
        }

        private FCNative m_native;

        /// <summary>
        /// 获取或设置方法库
        /// </summary>
        public FCNative Native
        {
            get { return m_native; }
            set { m_native = value; }
        }

        /// <summary>
        /// 获取客户端尺寸
        /// </summary>
        /// <returns>客户端尺寸</returns>
        public FCSize getClientSize()
        {
            return new FCSize(ClientSize.Width, ClientSize.Height);
        }

        /// <summary>
        /// 文件放下方法
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnDragDrop(DragEventArgs e)
        {
            base.OnDragDrop(e);
            StringBuilder filesName = new StringBuilder("");
            Array files = (System.Array)e.Data.GetData(DataFormats.FileDrop);//将拖来的数据转化为数组存储  
            foreach (object i in files)
            {
                String str = i.ToString();
                if (FCFile.isFileExist(str))
                {
                    m_designer.openFile(str);
                }
            }
        }

        /// <summary>
        /// 文件拖入方法
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        /// <summary>
        /// 窗体关闭方法
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (m_designer.hasModify())
            {
                if (MessageBox.Show("是否保存已修改的文档？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    m_designer.saveAll();
                }
            }
            m_designer.exit();
            Environment.Exit(0);
        }

        /// <summary>
        /// 尺寸改变方法
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (m_host != null)
            {
                m_designer.resetScaleSize(getClientSize());
                Invalidate();
            }
        }

        /// <summary>
        /// 鼠标滚动方法
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (m_host.isKeyPress(0x11))
            {
                double scaleFactor = m_designer.ScaleFactor;
                if (e.Delta > 0)
                {
                    if (scaleFactor > 0.2)
                    {
                        scaleFactor -= 0.1;
                    }
                }
                else if (e.Delta < 0)
                {
                    if (scaleFactor < 10)
                    {
                        scaleFactor += 0.1;
                    }
                }
                m_designer.ScaleFactor = scaleFactor;
                m_designer.resetScaleSize(getClientSize());
                Invalidate();
            }
        }


        /// <summary>
        /// 消息循环
        /// </summary>
        /// <param name="m">消息</param>
        protected override void WndProc(ref Message m)
        {
            if (m_designer != null)
            {
                if (m_designer.wndProc(ref m))
                {
                    return;
                }
            }
            base.WndProc(ref m);
        }
    }
}