/*基于捂脸猫FaceCat框架 v1.0
 捂脸猫创始人-矿洞程序员-脉脉KOL-陶德 (微信号:suade1984);
 */

using System;
using System.Collections.Generic;
using System.Text;
using FaceCat;
using System.Xml;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace FaceCat
{
    /// <summary>
    /// 创建项目界面
    /// </summary>
    public class ProjectWindow : WindowXmlEx
    {
        /// <summary>
        /// 创建界面
        /// </summary>
        /// <param name="native">方法库</param>
        public ProjectWindow(FCNative native)
        {
            load(native, "ProjectWindow", "windowProject");
            //注册点击事件
            registerEvents(m_window);
            m_tvTypes = getTree("tvTypes");
            m_gridTemplate = getGrid("gridTemplate");
            FCTextBox txtPath = getTextBox("txtPath");
            txtPath.Text = DataCenter.GetUserPath();
            onGridSelectedRowsChanged();
        }

        /// <summary>
        /// 表格选中行改变事件
        /// </summary>
        private FCEvent m_gridSelectedRowsChangedEvent;

        /// <summary>
        /// 模板表格
        /// </summary>
        private FCGrid m_gridTemplate;

        /// <summary>
        /// 集合表
        /// </summary>
        private FCTree m_tvTypes;

        private Designer m_designer;

        /// <summary>
        /// 获取或设置编辑器
        /// </summary>
        public Designer Designer {
            get { return m_designer; }
            set { m_designer = value; }
        }

        /// <summary>
        /// 浏览文件夹
        /// </summary>
        public void browserDir()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (DialogResult.OK == folderBrowserDialog.ShowDialog())
            {
                FCTextBox txtPath = getTextBox("txtPath");
                txtPath.Text = folderBrowserDialog.SelectedPath;
                txtPath.invalidate();
            }
        }

        /// <summary>
        /// 点击按钮方法
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="click">点击次数</param>
        /// <param name="delta">滚轮滚动值</param>
        private void clickButton(object sender, FCTouchInfo touchInto)
        {
            if (touchInto.m_firstTouch && touchInto.m_clicks == 1)
            {
                FCView control = sender as FCView;
                String name = control.Name;
                if (name == "btnBrowserDir")
                {
                    browserDir();
                }
                else if (name == "btnCreate")
                {
                    createProject();
                }
            }
        }

        /// <summary>
        /// 创建项目
        /// </summary>
        public void createProject()
        {
            String name = getTextBox("txtName").Text.Trim();
            String path = getTextBox("txtPath").Text.Trim();
            if (name == null || name.Length == 0)
            {
                MessageBox.Show("请输入项目名称!", "提示");
                return;
            }
            if (path == null || path.Length == 0)
            {
                MessageBox.Show("请输入项目路径!", "提示");
                return;
            }
            List<FCGridRow> selectedRows = m_gridTemplate.SelectedRows;
            int selectedRowsSize = selectedRows.Count;
            if (selectedRowsSize > 0)
            {
                String identifier = selectedRows[0].getCell(1).getString();
                if (identifier != null && identifier.Length > 0)
                {
                    String identifier2 = selectedRows[0].getCell(2).getString();
                    String identifier3 = selectedRows[0].getCell(3).getString();
                    String identifier4 = selectedRows[0].getCell(4).getString();
                    DirectoryInfo dir = new DirectoryInfo(DataCenter.GetAppPath());
                    String codeDir = Application.StartupPath + "\\config\\projects\\" + identifier + "\\" + identifier2 + "\\" + identifier3;
                    if (identifier4 == "0") {
                        ArrayList<String> files = new ArrayList<String>();
                        FCFile.getFiles(codeDir, files);
                        int filesSize = files.Count;
                        if (filesSize > 0) {
                            String fullName = files.get(0);
                            String suffix = fullName.Substring(fullName.LastIndexOf('.') + 1);
                            String newPath = path + "\\" + name + "." + suffix;
                            File.Copy(fullName, newPath, true);
                            m_designer.openFile(newPath);

                        }
                    } else if (identifier4 == "1") {
                        String projectDir = path + "\\" + name;
                        createProject(name, projectDir, identifier);
                        Process.Start(projectDir);
                    }
                    close();
                }
            }
        }

        /// <summary>
        /// 创建项目
        /// </summary>
        /// <param name="dir">目录</param>
        private void createProject(String projectName, String dir, String identifier)
        {
            ArrayList<String> dirs = new ArrayList<String>();
            FCFile.getDirectories(dir, dirs);
            int dirsSize = dirs.Count;
            for (int i = 0; i < dirsSize; i++)
            {
                createProject(projectName, dirs.get(i), identifier);
            }
            ArrayList<String> files = new ArrayList<String>();
            FCFile.getFiles(dir, files);
            int filesSize = files.Count;
            for (int i = 0; i < filesSize; i++)
            {
                String file = files.get(i);
                String content = "";
                if (file.Substring(file.LastIndexOf('\\')).IndexOf(identifier) != -1) {
                    FCFile.read(file, ref content);
                    String newFile = file.Replace(identifier, projectName);
                    FCFile.write(newFile, content);
                    FCFile.removeFile(file);
                    file = newFile;
                }
                FCFile.read(file, ref content);
                if (content.IndexOf(identifier) != -1) {
                    content = content.Replace(identifier, projectName);
                    FCFile.write(file, content);
                }
            }
            dirs.Clear();
            files.Clear();
        }

        /// <summary>
        /// 销毁方法
        /// </summary>
        public override void delete()
        {
            if (!IsDeleted)
            {
                if (m_tvTypes != null)
                {
                    m_tvTypes.removeEvent(m_gridSelectedRowsChangedEvent, FCEventID.GRIDSELECTEDROWSCHANGED);
                    m_gridSelectedRowsChangedEvent = null;
                    m_tvTypes = null;
                }
                base.delete();
            }
        }

        /// <summary>
        /// 表格选中行改变事件
        /// </summary>
        /// <param name="sender">调用者</param>
        private void gridSelectedRowsChanged(object sender)
        {
            onGridSelectedRowsChanged();
        }

        /// <summary>
        /// 表格选中行改变方法
        /// </summary>
        public void onGridSelectedRowsChanged()
        {
            ArrayList<FCTreeNode> selectedNodes = m_tvTypes.SelectedNodes;
            int selectedNodesSize = selectedNodes.Count;
            if (selectedNodesSize > 0)
            {
                FCTreeNode node = selectedNodes[0];
                String name = node.Name.Replace("node_", "");
                ArrayList<FCGridRow> rows = m_gridTemplate.m_rows;
                int rowsSize = rows.Count;
                ArrayList<FCGridRow> selectedRows = new ArrayList<FCGridRow>();
                if (name.IndexOf('_') != -1)
                {
                    String language = name.Substring(0, name.IndexOf('_'));
                    String platform = name.Substring(name.IndexOf('_') + 1);
                    for (int i = 0; i < rowsSize; i++)
                    {
                        FCGridRow row = rows[i];
                        if (row.getCell("COLT2").getString() == language
                            && row.getCell("COLT3").getString() == platform)
                        {
                            if (selectedRows.Count == 0)
                            {
                                selectedRows.Add(row);
                            }
                            row.Visible = true;
                        }
                        else
                        {
                            row.Visible = false;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < rowsSize; i++)
                    {
                        FCGridRow row = rows[i];
                        if (row.getCell("COLT2").getString() == name)
                        {
                            if (selectedRows.Count == 0)
                            {
                                selectedRows.Add(row);
                            }
                            row.Visible = true;
                        }
                        else
                        {
                            row.Visible = false;
                        }
                    }
                }
                m_gridTemplate.VScrollBar.Pos = 0;
                m_gridTemplate.SelectedRows = selectedRows;
                m_gridTemplate.update();
                m_gridTemplate.invalidate();
            }
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="control">控件</param>
        private void registerEvents(FCView control)
        {
            FCTouchEvent clickButtonEvent = new FCTouchEvent(clickButton);
            List<FCView> controls = control.getControls();
            int controlsSize = controls.Count;
            for (int i = 0; i < controlsSize; i++)
            {
                FCView subControl = controls[i];
                FCButton button = subControl as FCButton;
                FCGrid grid = subControl as FCGrid;
                if (button != null)
                {
                    button.addEvent(clickButtonEvent, FCEventID.CLICK);
                }
                else if (grid != null)
                {
                    FCGridRowStyle rowStyle = new FCGridRowStyle();
                    grid.RowStyle = rowStyle;
                    rowStyle.BackColor = FCColor.None;
                    rowStyle.SelectedBackColor = FCDraw.FCCOLORS_SELECTEDROWCOLOR;
                    rowStyle.HoveredBackColor = FCDraw.FCCOLORS_HOVEREDROWCOLOR;
                    rowStyle.SelectedTextColor = FCDraw.FCCOLORS_TEXTCOLOR4;
                }
                registerEvents(subControl);
            }
        }

        /// <summary>
        /// 显示
        /// </summary>
        public override void showDialog()
        {
            m_gridSelectedRowsChangedEvent = new FCEvent(gridSelectedRowsChanged);
            m_tvTypes.addEvent(m_gridSelectedRowsChangedEvent, FCEventID.GRIDSELECTEDROWSCHANGED);
            base.showDialog();
        }
    }
}
