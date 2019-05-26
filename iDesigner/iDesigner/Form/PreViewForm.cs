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
using FaceCat;

namespace FaceCat
{
    public partial class PreViewForm : Form
    {
        /// <summary>
        /// 创建窗体
        /// </summary>
        public PreViewForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 控件管理器
        /// </summary>
        private WinHostEx m_host;

        /// <summary>
        /// 方法库
        /// </summary>
        private FCNative m_native;

        /// <summary>
        /// 布局器
        /// </summary>
        private UIXmlEx3 m_xml;

        /// <summary>
        /// 获取客户端尺寸
        /// </summary>
        /// <returns>客户端尺寸</returns>
        public FCSize getClientSize()
        {
            return new FCSize(ClientSize.Width, ClientSize.Height);
        }

        /// <summary>
        /// 加载XML
        /// </summary>
        /// <param name="fileName">文件名</param>
        public void loadFile(String fileName)
        {
            //创建XML解析器
            m_xml = new UIXmlEx3();
            //链接控件库
            m_xml.createNative();
            m_native = m_xml.Native;
            m_xml.Script = new DesignerScript(m_xml);
            m_native.Paint = new GdiPlusPaintEx();
            m_native.Host = new WinHostEx();
            m_native.Host.Native = m_native;
            m_host = m_native.Host as WinHostEx;
            m_host.HWnd = Handle;
            m_native.AllowScaleSize = true;
            m_native.DisplaySize = new FCSize(ClientSize.Width, ClientSize.Height);
            m_xml.resetScaleSize(getClientSize());
            m_native.invalidate();
            m_xml.loadFile(fileName, null);
            m_native.update();
            m_native.invalidate();
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
                m_xml.resetScaleSize(getClientSize());
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
                double scaleFactor = m_xml.ScaleFactor;
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
                m_xml.ScaleFactor = scaleFactor;
                m_xml.resetScaleSize(getClientSize());
                Invalidate();
            }
        }


        /// <summary>
        /// 消息循环
        /// </summary>
        /// <param name="m">消息</param>
        protected override void WndProc(ref Message m)
        {
            if (m_host != null)
            {
                if (m_host.onMessage(ref m) > 0)
                {
                    return;
                }
            }
            base.WndProc(ref m);
        }
    }
}