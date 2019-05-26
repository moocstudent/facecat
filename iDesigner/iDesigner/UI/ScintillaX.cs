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
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Xml;
using FaceCat;
using ScintillaNet;

namespace FaceCat
{
    /// <summary>
    /// 编辑器控件
    /// </summary>
    public partial class ScintillaX : Scintilla
    {
        /// <summary>
        /// 创建控件
        /// </summary>
        public ScintillaX()
        {
            InitializeComponent();
            m_parentDivLocationChanged = new FCEvent(parentDivLocationChanged);
            m_parentDivCodeSizeChanged = new FCEvent(parentDivSizeChanged);
            m_parentDivVisibleChanged = new FCEvent(parentDivVisibleChanged);
        }

        /// <summary>
        /// 所在层位置改变事件
        /// </summary>
        private FCEvent m_parentDivLocationChanged = null;

        /// <summary>
        /// 所在层尺寸改变事件
        /// </summary>
        private FCEvent m_parentDivCodeSizeChanged = null;

        /// <summary>
        /// 所在层可见改变事件
        /// </summary>
        private FCEvent m_parentDivVisibleChanged = null;

        private FCView m_parentDiv;

        /// <summary>
        /// 获取或设置层控件
        /// </summary>
        public FCView ParentDiv
        {
            get { return m_parentDiv; }
            set
            {
                m_parentDiv = value;
                m_parentDiv.addEvent(m_parentDivLocationChanged, FCEventID.LOCATIONCHANGED);
                m_parentDiv.addEvent(m_parentDivCodeSizeChanged, FCEventID.SIZECHANGED);
                m_parentDiv.addEvent(m_parentDivVisibleChanged, FCEventID.VISIBLECHANGED);
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hwnd, int x, int y, int cx, int cy, bool repaint);

        private const int SW_HIDE = 0;
        private const int SW_SHOWNOACTIVATE = 4;
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern int ShowWindow(IntPtr hWnd, short cmdShow);

        /// <summary>
        /// 加载Xml
        /// </summary>
        /// <param name="xml">XML</param>
        public void loadXml(String xml)
        {
            Text = xml;
            UndoRedo.EmptyUndoBuffer();
        }

        /// <summary>
        /// 定位到XML节点
        /// </summary>
        /// <param name="xml">查找名称</param>
        public void locateToXml(String xml)
        {
            FindReplace.Flags = SearchFlags.WholeWord;
            Range range = null;
            range = FindReplace.FindNext(xml);
            if (range == null)
            {
                return;
            }
            byte style = Styles.GetStyleAt(range.End - 1);
            while (style == 9)
            {
                Selection.Range = range;
                range = FindReplace.FindNext(xml);
                style = Styles.GetStyleAt(range.End - 1);
            }
            Selection.Range = range;
            if (range != null)
            {
                Selection.Range = range;
            }
        }

        /// <summary>
        /// 所在层位置改变事件
        /// </summary>
        /// <param name="sender">调用者</param>
        private void parentDivLocationChanged(object sender)
        {
            updateScintilla();
        }

        /// <summary>
        /// 所在层尺寸改变事件
        /// </summary>
        /// <param name="sender">调用者</param>
        private void parentDivSizeChanged(object sender)
        {
            updateScintilla();
        }

        /// <summary>
        /// 所在层可见状态改变事件
        /// </summary>
        /// <param name="sender">调用者</param>
        private void parentDivVisibleChanged(object sender)
        {
            updateScintilla();
        }

        /// <summary>
        /// 更新编辑器
        /// </summary>
        /// <param name="parent">控件</param>
        private void updateScintilla()
        {
            FCNative native = m_parentDiv.Native;
            if (native != null)
            {
                if (m_parentDiv.isPaintVisible(m_parentDiv))
                {
                    ShowWindow(Handle, SW_SHOWNOACTIVATE);
                    float scaleFactorX = 1, scaleFactorY = 1;
                    FCSize scaleSize = native.ScaleSize;
                    WinHostEx winHost = native.Host as WinHostEx;
                    Control control = Control.FromHandle(winHost.HWnd);
                    FCSize size = new FCSize(control.ClientSize.Width, control.ClientSize.Height);
                    if (size.cx > 0 & size.cy > 0)
                    {
                        scaleFactorX = (float)scaleSize.cx / size.cx;
                        scaleFactorY = (float)scaleSize.cy / size.cy;
                    }
                    int x = (int)(native.clientX(m_parentDiv) / scaleFactorX);
                    int y = (int)(native.clientY(m_parentDiv) / scaleFactorY);
                    int cx = (int)(m_parentDiv.Width / scaleFactorX);
                    int cy = (int)(m_parentDiv.Height / scaleFactorY);
                    MoveWindow(Handle, x, y, cx, cy, true);
                }
                else
                {
                    ShowWindow(Handle, SW_HIDE);
                }
            }
        }
    }
}
