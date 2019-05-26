/*基于捂脸猫FaceCat框架 v1.0
 捂脸猫创始人-矿洞程序员-脉脉KOL-陶德 (微信号:suade1984);
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Diagnostics;
using FaceCat;
using System.Drawing;
using ScintillaNet;

namespace FaceCat
{
    /// <summary>
    /// 设计器
    /// </summary>
    public class Designer : UIXmlEx
    {
        /// <summary>
        /// 创建设计器
        /// </summary>
        public Designer()
        {
            
        }

        /// <summary>
        /// 活动页按钮
        /// </summary>
        private RibbonButton m_btnActiveTab;

        /// <summary>
        /// 复制按钮
        /// </summary>
        private FCButton m_btnCopy;

        /// <summary>
        /// 剪切按钮
        /// </summary>
        private FCButton m_btnCut;

        /// <summary>
        /// 删除按钮
        /// </summary>
        private FCButton m_btnDelete;

        /// <summary>
        /// 粘贴按钮
        /// </summary>
        private FCButton m_btnPaste;

        /// <summary>
        /// 预览按钮
        /// </summary>
        private FCButton m_btnPreview;

        /// <summary>
        /// 重做按钮
        /// </summary>
        private FCButton m_btnRedo;

        /// <summary>
        /// 保存按钮
        /// </summary>
        private FCButton m_btnSave;

        /// <summary>
        /// 全部保存
        /// </summary>
        private FCButton m_btnSaveAll;

        /// <summary>
        /// 另存为按钮
        /// </summary>
        private FCButton m_btnSaveAs;

        /// <summary>
        /// 撤销按钮
        /// </summary>
        private FCButton m_btnUndo;

        /// <summary>
        /// 查看源代码按钮
        /// </summary>
        private FCButton m_btnViewSource;

        /// <summary>
        /// 控件列表
        /// </summary>
        private FCComboBox m_cbList;

        /// <summary>
        /// 控件列表选中索引改变事件
        /// </summary>
        private FCEvent m_cbListSelectedIndexChanged;

        /// <summary>
        /// 控件列表可见状态改变事件
        /// </summary>
        private FCEvent m_cbListMenuVisibleChangedEvent;

        /// <summary>
        /// 关闭页按钮事件
        /// </summary>
        private FCTouchEvent m_closeTabPageButtonClickEvent;

        /// <summary>
        /// 事件编辑
        /// </summary>
        private EventGrid m_gridEvent;

        /// <summary>
        /// 属性编辑
        /// </summary>
        private PropertyGrid m_gridProperty;

        /// <summary>
        /// 工具箱
        /// </summary>
        private ToolBoxList m_gridToolBox;

        /// <summary>
        /// 工具箱添加控件事件
        /// </summary>
        private ToolBoxListAddControlEvent m_gridToolBoxAddControlEvent;

        /// <summary>
        /// 控件管理器
        /// </summary>
        private WinHostEx m_host;

        /// <summary>
        /// 活动页菜单
        /// </summary>
        private FCMenu m_menuActiveTabs;

        /// <summary>
        /// 打开文件对话框
        /// </summary>
        private OpenFileDialog m_openFileDialog = new OpenFileDialog();

        /// <summary>
        /// 保存文件对话框
        /// </summary>
        private SaveFileDialog m_saveFileDialog = new SaveFileDialog();

        /// <summary>
        /// 设计多页夹
        /// </summary>
        private FCTabControl m_tabDesigner;

        /// <summary>
        /// 菜单多也夹
        /// </summary>
        private FCTabControl m_tabMainMenu;

        /// <summary>
        /// 编辑页
        /// </summary>
        private FCTabPage m_tabPageEdit;

        /// <summary>
        /// 文件页
        /// </summary>
        private FCTabPage m_tagPageFile;

        /// <summary>
        /// 获取设计页
        /// </summary>
        public List<DesignerDiv> DesignerDivs
        {
            get
            {
                List<DesignerDiv> designerDivs = new List<DesignerDiv>();
                List<FCTabPage> tabPages = m_tabDesigner.getTabPages();
                int tabPagesSize = tabPages.Count;
                for (int i = 0; i < tabPagesSize; i++)
                {
                    List<FCView> controls = tabPages[i].getControls();
                    int size = controls.Count;
                    for (int j = 0; j < size; j++)
                    {
                        FCView control = controls[j];
                        DesignerDiv designerDiv = control as DesignerDiv;
                        if (designerDiv != null)
                        {
                            designerDivs.Add(designerDiv);
                        }
                    }
                }
                return designerDivs;
            }
        }

        /// <summary>
        /// 获取选中的设计页
        /// </summary>
        public DesignerDiv SelectedDesignerDiv
        {
            get
            {
                FCTabPage selectedTabPage = m_tabDesigner.SelectedTabPage;
                if (selectedTabPage != null)
                {
                    List<FCView> controls = selectedTabPage.getControls();
                    int size = controls.Count;
                    for (int i = 0; i < size; i++)
                    {
                        FCView control = controls[i];
                        DesignerDiv designerDiv = control as DesignerDiv;
                        if (designerDiv != null)
                        {
                            return designerDiv;
                        }
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 获取选中的调整大小层
        /// </summary>
        public ResizeDiv SelectedResizeDiv
        {
            get
            {
                DesignerDiv designerDiv = SelectedDesignerDiv;
                if (designerDiv != null && designerDiv.DesignerTabPage.Visible)
                {
                    return designerDiv.ResizeDiv;
                }
                return null;
            }
        }

        /// <summary>
        /// 控件布局
        /// </summary>
        /// <param name="index">索引</param>
        public void align(int index)
        {
            ResizeDiv resizeDiv = SelectedResizeDiv;
            if (resizeDiv != null)
            {
                List<FCView> targets = resizeDiv.getTargets();
                int targetsSize = targets.Count;
                if (targetsSize > 1)
                {
                    UIXmlEx xml = resizeDiv.Xml;
                    resizeDiv.DesignerDiv.saveUndo();
                    FCView first = targets[0];
                    FCPoint location = first.Location;
                    FCSize size = first.Size;
                    switch (index)
                    {
                        case 0:
                            {
                                for (int i = 1; i < targetsSize; i++)
                                {
                                    FCView target = targets[i];
                                    xml.setProperty(targets[i], "location", String.Format("{0},{1}", location.x, target.Top));
                                }
                                break;
                            }
                        case 1:
                            {
                                int mx = location.x + size.cx / 2;
                                for (int i = 1; i < targetsSize; i++)
                                {
                                    FCView target = targets[i];
                                    xml.setProperty(targets[i], "location", String.Format("{0},{1}", mx - target.Width / 2, target.Top));
                                }
                                break;
                            }
                        case 2:
                            {
                                int right = location.x + size.cx;
                                for (int i = 1; i < targetsSize; i++)
                                {
                                    FCView target = targets[i];
                                    xml.setProperty(targets[i], "location", String.Format("{0},{1}", right - target.Width, target.Top));
                                }
                                break;
                            }
                        case 3:
                            {
                                for (int i = 1; i < targetsSize; i++)
                                {
                                    FCView target = targets[i];
                                    xml.setProperty(targets[i], "location",  String.Format("{0},{1}", target.Left, location.y));
                                }
                                break;
                            }
                        case 4:
                            {
                                int my = location.y + size.cy / 2;
                                for (int i = 1; i < targetsSize; i++)
                                {
                                    FCView target = targets[i];
                                    xml.setProperty(targets[i], "location",  String.Format("{0},{1}", target.Left, my - target.Height / 2));
                                }
                                break;
                            }
                        case 5:
                            {
                                int bottom = location.y + size.cy;
                                for (int i = 1; i < targetsSize; i++)
                                {
                                    FCView target = targets[i];
                                    xml.setProperty(targets[i], "location", String.Format("{0},{1}", target.Left, bottom - target.Height));
                                }
                                break;
                            }
                        case 6:
                            {
                                for (int i = 1; i < targetsSize; i++)
                                {
                                    FCView target = targets[i];
                                    xml.setProperty(targets[i], "size", String.Format("{0},{1}", size.cx, target.Height));
                                }
                                break;
                            }
                        case 7:
                            {
                                for (int i = 1; i < targetsSize; i++)
                                {
                                    FCView target = targets[i];
                                    xml.setProperty(targets[i], "size",  String.Format("{0},{1}", target.Width, size.cy));
                                }
                                break;
                            }
                        case 8:
                            {
                                for (int i = 1; i < targetsSize; i++)
                                {
                                    FCView target = targets[i];
                                    xml.setProperty(targets[i], "size", String.Format("{0},{1}", size.cx, size.cy));
                                }
                                break;
                            }
                    }
                }
                refreshProperties();
                resizeDiv.refreshStatusBar();
                Native.update();
                Native.invalidate();
            }
        }

        /// <summary>
        /// 显示虫洞
        /// </summary>
        public void bugHole()
        {
            ResizeDiv resizeDiv = SelectedResizeDiv;
            if (resizeDiv != null)
            {
                List<FCView> targets = resizeDiv.getTargets();
                int targetsSize = targets.Count;
                if (targetsSize > 0)
                {
                    FCView target = targets[0];
                    BugHoleForm bugHoleForm = new BugHoleForm();
                    bugHoleForm.addBugHole(Native, target);
                    bugHoleForm.Show();
                }
            }
        }

        /// <summary>
        /// 调用秒表事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="timerID">秒表ID</param>
        private void callTimer(object sender, int timerID)
        {
            onTimer(timerID);
        }

        /// <summary>
        /// 切换风格
        /// </summary>
        public void changeStyle(int style)
        {
            FCDraw.m_style = style;
            Native.invalidate();
        }

        /// <summary>
        /// 菜单点击事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="item">菜单项</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">鼠标滚动值</param>
        private void clickMenuItem(object sender, FCMenuItem item, FCTouchInfo touchInto)
        {
            FCTabPage tabPage = item.Tag as FCTabPage;
            if (tabPage != null)
            {
                showActiveTabPage(tabPage);
            }
        }

        /// <summary>
        /// 关闭页
        /// </summary>
        /// <param name="tabPage">页</param>
        private void closeTabPage(FCTabPage tabPage)
        {
            DesignerDiv designerDiv = SelectedDesignerDiv;
            if (designerDiv != null && designerDiv.Modified)
            {
                if (MessageBox.Show("文档未保存，是否放弃关闭？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
            }
            FCTabControl tabControl = tabPage.TabControl;
            tabPage.TabControl.removeControl(tabPage);
            tabPage.delete();
            tabControl.update();
            m_gridEvent.clearTargets();
            m_gridEvent.createProperties();
            m_gridProperty.clearTargets();
            m_gridProperty.createProperties();
            Native.invalidate();
        }

        /// <summary>
        /// 关闭页按钮点击事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚轮值</param>
        private void closeTabPageButtonClick(object sender, FCTouchInfo touchInto)
        {
            FCView control = sender as FCView;
            FCTabPage tabPage = control.Tag as FCTabPage;
            closeTabPage(tabPage);
        }

        /// <summary>
        /// 控件列表菜单可见状态改变事件
        /// </summary>
        /// <param name="sender">调用者</param>
        private void controlListMenuVisibleChanged(object sender)
        {
            FCView control = sender as FCView;
            if (control.Visible)
            {
                loadControlList();
            }
        }

        /// <summary>
        /// 控件列表选中索引改变事件
        /// </summary>
        /// <param name="sender">调用者</param>
        private void controlListSelectedIndexChanged(object sender)
        {
            int selectedIndex = m_cbList.SelectedIndex;
            if (selectedIndex != -1 && m_cbList.DropDownMenu.Visible)
            {
                List<FCMenuItem> items = m_cbList.getItems();
                FCMenuItem selectedItem = items[selectedIndex];
                FCView control = selectedItem.Tag as FCView;
                setCurrentControl(control);
            }
        }

        /// <summary>
        /// 复制
        /// </summary>
        public void Copy()
        {
            DesignerDiv selectedDiv = SelectedDesignerDiv;
            if (selectedDiv != null)
            {
                selectedDiv.copy();
            }
        }

        /// <summary>
        /// 新建
        /// </summary>
        public void create()
        {
            ProjectWindow projectWindow = new ProjectWindow(Native);
            projectWindow.Designer = this;
            projectWindow.showDialog();
        }

        /// <summary>
        /// 剪切
        /// </summary>
        public void cut()
        {
            DesignerDiv selectedDiv = SelectedDesignerDiv;
            if (selectedDiv != null)
            {
                selectedDiv.cut();
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        public void del()
        {
            DesignerDiv selectedDiv = SelectedDesignerDiv;
            if (selectedDiv != null)
            {
                selectedDiv.del();
            }
        }

        /// <summary>
        /// 退出方法
        /// </summary>
        public void exit()
        {
            String xmlPaths = "";
            List<DesignerDiv> designerDivs = DesignerDivs;
            int designerDivsSize = designerDivs.Count;
            for (int i = 0; i < designerDivsSize; i++)
            {
                DesignerDiv designerDiv = designerDivs[i];
                xmlPaths += designerDiv.XmlPath + ";";
            }
            UserCookie cookie = new UserCookie();
            cookie.m_key = "XMLPATHS";
            cookie.m_value = xmlPaths;
            DataCenter.UserCookieService.AddCookie(cookie);
            m_gridEvent.save();
            m_gridProperty.save();
        }

        /// <summary>
        /// 检查是否有修改
        /// </summary>
        /// <returns>是否有修改</returns>
        public bool hasModify()
        {
            List<DesignerDiv> designerDivs = DesignerDivs;
            int designerDivsSize = designerDivs.Count;
            for (int i = 0; i < designerDivsSize; i++)
            {
                DesignerDiv designerDiv = designerDivs[i];
                if (designerDiv.Modified)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否有窗体显示
        /// </summary>
        /// <returns>是否显示</returns>
        public bool isWindowShowing()
        {
            List<FCView> controls = Native.getControls();
            int controlsSize = controls.Count;
            for (int i = 0; i < controlsSize; i++)
            {
                FCWindowFrame frame = controls[i] as FCWindowFrame;
                if (frame != null)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 加载XML
        /// </summary>
        /// <param name="xmlPath">XML地址</param>
        public void load(String xmlPath)
        {
            //预创建控件
            Scintilla scintilla = new Scintilla();
            scintilla.Dispose();
            loadFile(xmlPath, null);
            m_host = Native.Host as WinHostEx;
            //加载事件
            loadEvents();
            m_openFileDialog.CheckFileExists = false;
            m_openFileDialog.FileName = "skin.html";
            m_openFileDialog.Filter = "All Files(*.*)|*.*|xml Files(*.xml)|*.xml|html Files(*.html)|*.html";
            m_openFileDialog.RestoreDirectory = true;
            m_saveFileDialog.FileName = "skin.html";
            m_saveFileDialog.Filter = "All Files(*.*)|*.*|xml Files(*.xml)|*.xml|html Files(*.html)|*.html";
            UserCookie cookie = new UserCookie();
            if(DataCenter.UserCookieService.GetCookie("XMLPATHS", ref cookie) > 0)
            {
                if (cookie.m_value != null && cookie.m_value.Length > 0)
                {
                    String[] xmlPaths = cookie.m_value.Split(new String[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    int xmlPathsSize = xmlPaths.Length;
                    for (int i = 0; i < xmlPathsSize; i++)
                    {
                        if (FCFile.isFileExist(xmlPaths[i]))
                        {
                            openFile(xmlPaths[i]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 加载控件列表
        /// </summary>
        public void loadControlList()
        {
            //设置控件列表
            DesignerDiv selectedDiv = SelectedDesignerDiv;
            if (selectedDiv != null)
            {
                UIXmlEx xml = selectedDiv.Xml;
                FCMenu menu = m_cbList.DropDownMenu;
                m_cbList.clearItems();
                foreach (FCView xmlControl in xml.Nodes.Keys)
                {
                    FCMenuItem item = new FCMenuItem(xmlControl.Name + " [" + xmlControl.getControlType() + "]");
                    item.Tag = xmlControl;
                    menu.addItem(item);
                }
                List<FCMenuItem> items = menu.getItems();
                int menuHeight = items.Count * 30;
                if (menuHeight > 300)
                {
                    menuHeight = 300;
                }
                menu.Width = m_cbList.Width;
                menu.Height = menuHeight;
                menu.update();
            }
        }

        /// <summary>
        /// 加载事件
        /// </summary>
        public void loadEvents()
        {
            m_btnCopy = getButton("btnCopy");
            m_btnCut = getButton("btnCut");
            m_btnDelete = getButton("btnDelete");
            m_btnPaste = getButton("btnPaste");
            m_btnPreview = getButton("btnPreview");
            m_btnRedo = getButton("btnRedo");
            m_btnSave = getButton("btnSave");
            m_btnSaveAll = getButton("btnSaveAll");
            m_btnSaveAs = getButton("btnSaveAs");
            m_btnUndo = getButton("btnUndo");
            m_btnViewSource = getButton("btnViewSource");
            //关闭页按钮点击事件
            m_closeTabPageButtonClickEvent = new FCTouchEvent(closeTabPageButtonClick);
            //下拉列表
            m_cbList = getComboBox("cbList");
            if (m_cbList != null)
            {
                m_cbListSelectedIndexChanged = new FCEvent(controlListSelectedIndexChanged);
                m_cbList.addEvent(m_cbListSelectedIndexChanged, FCEventID.SELECTEDINDEXCHANGED);
                m_cbListMenuVisibleChangedEvent = new FCEvent(controlListMenuVisibleChanged);
                m_cbList.DropDownMenu.addEvent(m_cbListMenuVisibleChangedEvent, FCEventID.VISIBLECHANGED);
            }
            //事件编辑
            m_gridEvent = findControl("gridEvent") as EventGrid;
            m_gridEvent.Xml = this;
            //属性编辑
            m_gridProperty = findControl("gridProperty") as PropertyGrid;
            m_gridProperty.Xml = this;
            //工具箱
            m_gridToolBox = findControl("listToolBox") as ToolBoxList;
            m_gridToolBoxAddControlEvent = new ToolBoxListAddControlEvent(toolBoxListAddControl);
            m_gridToolBox.addEvent(m_gridToolBoxAddControlEvent, ToolBoxList.EVENTID_TOOLBOXADDCONTROL);
            //设置页夹
            m_tabDesigner = getTabControl("tabDesigner");
            //编辑页
            m_tabPageEdit = getTabPage("tabPageEdit");
            //文件页
            m_tagPageFile = getTabPage("tabPageFile");
            Native.update();
            //活动页按钮设置
            m_btnActiveTab = findControl("btnActiveTab") as RibbonButton;
            m_btnActiveTab.ArrowType = 4;
            m_btnActiveTab.Location = new FCPoint(m_tabDesigner.Right - 22, m_tabDesigner.Top + 2);
            m_btnActiveTab.Anchor = new FCAnchor(false, true, true, false);
            FCDiv divMain = getDiv("divMain");
            divMain.addEvent(new FCTimerEvent(callTimer), FCEventID.TIMER);
            divMain.startTimer(FCView.getNewTimerID(), 100);
            m_tabMainMenu = getTabControl("tabMainMenu");
            FCPaintEvent paintLayoutEvent = new FCPaintEvent(paintLayoutMenu);
            getDiv("divLayoutMenu1").addEvent(paintLayoutEvent, FCEventID.PAINT);
            getDiv("divLayoutMenu2").addEvent(paintLayoutEvent, FCEventID.PAINT);
            getDiv("divLayoutMenu3").addEvent(paintLayoutEvent, FCEventID.PAINT);
            getDiv("divLayoutMenu4").addEvent(paintLayoutEvent, FCEventID.PAINT);
            getDiv("tabMainMenu").addEvent(paintLayoutEvent, FCEventID.PAINT);
            getDiv("divStatus").addEvent(paintLayoutEvent, FCEventID.PAINT);
        }

        /// <summary>
        /// 镜像
        /// </summary>
        public void mirror()
        {
            ResizeDiv resizeDiv = SelectedResizeDiv;
            if (resizeDiv != null)
            {
                List<FCView> targets = resizeDiv.getTargets();
                int targetsSize = targets.Count;
                if (targetsSize > 0)
                {
                    FCView target = targets[0];
                    MirroForm mirrorForm = new MirroForm();
                    mirrorForm.addMirror(Native, target);
                    mirrorForm.Show();
                }
            }
        }

        /// <summary>
        /// 秒表方法
        /// </summary>
        /// <param name="timerID">秒表ID</param>
        private void onTimer(int timerID)
        {
            bool paint = false;
            DesignerDiv selectedDiv = SelectedDesignerDiv;
            if (selectedDiv != null)
            {
                if (!m_btnSave.Enabled)
                {
                    m_btnSave.Enabled = true;
                    paint = true;
                }
                if (!m_btnSaveAll.Enabled)
                {
                    m_btnSaveAll.Enabled = true;
                    paint = true;
                }
                if (!m_btnSaveAs.Enabled)
                {
                    m_btnSaveAs.Enabled = true;
                    paint = true;
                }
                if (!m_btnViewSource.Enabled)
                {
                    m_btnViewSource.Enabled = true;
                    paint = true;
                }
                if (!m_btnActiveTab.Enabled)
                {
                    m_btnActiveTab.Enabled = true;
                    paint = true;
                }
                if (!m_btnPreview.Enabled)
                {
                    m_btnPreview.Enabled = true;
                    paint = true;
                }
                bool canRedo = selectedDiv.canRedo();
                if (m_btnRedo.Enabled != canRedo)
                {
                    m_btnRedo.Enabled = canRedo;
                    paint = true;
                }
                bool canUndo = selectedDiv.canUndo();
                if (m_btnUndo.Enabled != canUndo)
                {
                    m_btnUndo.Enabled = canUndo;
                    paint = true;
                }
                bool canCopy = selectedDiv.canCopy();
                if (m_btnCopy.Enabled != canCopy)
                {
                    m_btnCopy.Enabled = canCopy;
                    paint = true;
                }
                bool canCut = selectedDiv.canCut();
                if (m_btnCut.Enabled != canCut)
                {
                    m_btnCut.Enabled = canCut;
                    paint = true;
                }
                bool canDelete = selectedDiv.canDelete();
                if (m_btnDelete.Enabled != canDelete)
                {
                    m_btnDelete.Enabled = canDelete;
                    paint = true;
                }
                bool canPaste = selectedDiv.canPaste();
                if (m_btnPaste.Enabled != canPaste)
                {
                    m_btnPaste.Enabled = canPaste;
                    paint = true;
                }
            }
            else
            {
                if (m_btnSave.Enabled)
                {
                    m_btnCopy.Enabled = false;
                    m_btnCut.Enabled = false;
                    m_btnDelete.Enabled = false;
                    m_btnPaste.Enabled = false;
                    m_btnRedo.Enabled = false;
                    m_btnSave.Enabled = false;
                    m_btnSaveAll.Enabled = false;
                    m_btnSaveAs.Enabled = false;
                    m_btnUndo.Enabled = false;
                    m_btnViewSource.Enabled = false;
                    m_btnActiveTab.Enabled = false;
                    m_btnPreview.Enabled = false;
                    paint = true;
                }
            }
            if (paint)
            {
                m_tabMainMenu.invalidate();
            }
        }

        /// <summary>
        /// 打开
        /// </summary>
        public void open()
        {
            if (DialogResult.OK == m_openFileDialog.ShowDialog())
            {
                openFile(m_openFileDialog.FileName);
            }
        }

        /// <summary>
        /// 打开XML
        /// </summary>
        /// <param name="path">路径</param>
        public void openFile(String path)
        {
            m_host.LoadingDesigner = true;
            //创建页
            FCTabPage newTabPage = new FCTabPage();
            FileInfo fileInfo = new FileInfo(path);
            m_tabDesigner.addControl(newTabPage);
            //设置页头按钮
            FCButton headerButton = newTabPage.HeaderButton;
            FCFont headerFont = headerButton.Font;
            System.Drawing.Size textSize = TextRenderer.MeasureText(fileInfo.Name, new System.Drawing.Font(headerFont.m_fontFamily, headerFont.m_fontSize));
            int headerWidth = textSize.Width + 20;
            headerButton.Size = new FCSize(headerWidth, 25);
            headerButton.Margin = new FCPadding(0, 1, 1, 0);
            headerButton.TextAlign = FCContentAlignment.MiddleLeft;
            newTabPage.Text = fileInfo.Name;
            //添加关闭按钮
            FCButton closeButton = new FCButton();
            closeButton.Size = new FCSize(16, 16);
            headerButton.addControl(closeButton);
            closeButton.Location = new FCPoint(headerWidth - 22, 4);
            closeButton.BorderColor = FCColor.None;
            closeButton.BackImage = "file='\\images\\TabPageCloseBtn.png' source='22,0,44,19'";
            closeButton.HoveredBackImage = "file='\\images\\TabPageCloseBtn.png' source='0,0,22,19'";
            closeButton.PushedBackImage = "file='\\images\\TabPageCloseBtn.png' source='44,0,66,19'";
            closeButton.Tag = newTabPage;
            closeButton.addEvent(m_closeTabPageButtonClickEvent, FCEventID.CLICK);
            //添加设计层
            DesignerDiv designerDiv = new DesignerDiv();
            designerDiv.Dock = FCDockStyle.Fill;
            designerDiv.Designer = this;
            newTabPage.addControl(designerDiv);
            m_host.LoadingDesigner = false;
            designerDiv.openFile(path);
            m_tabDesigner.update();
            //加载控件列表
            loadControlList();
            if (designerDiv.Xml.m_controls.Count > 0)
            {
                setCurrentControl(designerDiv.Xml.m_controls[0]);
            }
            Native.invalidate();
        }

        /// <summary>
        /// 重绘菜单布局
        /// </summary>
        /// <param name="sender">调用对象</param>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        private void paintLayoutMenu(object sender, FCPaint paint, FaceCat.FCRect clipRect)
        {
            if (FCDraw.m_style == 0)
            {
                FCView control = sender as FCView;
                int width = control.Width, height = control.Height;
                FaceCat.FCRect drawRect = new FaceCat.FCRect(0, 0, width, height);
                paint.fillGradientRect(FCDraw.FCCOLORS_BACKCOLOR, FCDraw.FCCOLORS_BACKCOLOR2, drawRect, 0, 90);
            }
        }

        /// <summary>
        /// 粘贴
        /// </summary>
        public void paste()
        {
            DesignerDiv selectedDiv = SelectedDesignerDiv;
            if (selectedDiv != null)
            {
                selectedDiv.paste();
            }
        }

        /// <summary>
        /// 预览
        /// </summary>
        public void preview()
        {
            DesignerDiv selectedDiv = SelectedDesignerDiv;
            if (selectedDiv != null)
            {
                saveAll();
                PreViewForm preViewForm = new PreViewForm();
                preViewForm.loadFile(selectedDiv.XmlPath);
                preViewForm.Show();
            }
        }

        /// <summary>
        /// 重做
        /// </summary>
        public void redo()
        {
            DesignerDiv selectedDiv = SelectedDesignerDiv;
            if (selectedDiv != null)
            {
                if (selectedDiv.SelectedTabPage == selectedDiv.DesignerTabPage)
                {
                    selectedDiv.redo();
                    return;
                }
            }
            Native.update();
            Native.invalidate();
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        public void refresh()
        {
            DesignerDiv selectedDiv = SelectedDesignerDiv;
            if (selectedDiv != null)
            {
                if (selectedDiv.SelectedTabPage == selectedDiv.DesignerTabPage)
                {
                    selectedDiv.DesignerTabPage.update();
                    selectedDiv.DesignerTabPage.invalidate();
                    return;
                }
            }
            Native.update();
            Native.invalidate();
        }

        /// <summary>
        /// 刷新属性
        /// </summary>
        public void refreshProperties()
        {
            m_gridEvent.createProperties();
            m_gridProperty.createProperties();
        }

        /// <summary>
        /// 刷新属性
        /// </summary>
        /// <param name="targets"></param>
        public void refreshProperties(List<FCView> targets)
        {
            m_gridEvent.clearTargets();
            m_gridEvent.addTargets(targets);
            m_gridProperty.clearTargets();
            m_gridProperty.addTargets(targets);
            m_gridEvent.createProperties();
            m_gridProperty.createProperties();
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void save()
        {
            DesignerDiv selectedDiv = SelectedDesignerDiv;
            if (selectedDiv != null)
            {
                selectedDiv.saveFile(selectedDiv.XmlPath);
                Native.invalidate();
            }
        }

        /// <summary>
        /// 全部保存
        /// </summary>
        public void saveAll()
        {
            List<DesignerDiv> designerDivs = DesignerDivs;
            int designerDivsSize = designerDivs.Count;
            for (int i = 0; i < designerDivsSize; i++)
            {
                DesignerDiv designerDiv = designerDivs[i];
                designerDiv.saveFile(designerDiv.XmlPath);
            }
        }

        /// <summary>
        /// 另存为
        /// </summary>
        public void saveAs()
        {
            DesignerDiv selectedDiv = SelectedDesignerDiv;
            if (selectedDiv != null)
            {
                if (DialogResult.OK == m_saveFileDialog.ShowDialog())
                {
                    selectedDiv.saveFile(m_saveFileDialog.FileName);
                    Native.invalidate();
                }
            }
        }

        /// <summary>
        /// 设置当前控件
        /// </summary>
        /// <param name="control">控件</param>
        public void setCurrentControl(FCView control)
        {
            if (control != null)
            {
                DesignerDiv selectedDiv = SelectedDesignerDiv;
                if (selectedDiv != null)
                {
                    FCTabPage designerTabPage = selectedDiv.DesignerTabPage;
                    if (control != designerTabPage)
                    {
                        UIXmlEx xml = selectedDiv.Xml;
                        if (selectedDiv.Xml.containsControl(control))
                        {
                            //设置调整尺寸的层
                            FCPoint location = control.pointToNative(new FCPoint(0, 0));
                            location.x -= Native.clientX(designerTabPage);
                            location.y -= Native.clientY(designerTabPage);
                            ResizeDiv divResize = selectedDiv.ResizeDiv;
                            FCPoint offset = divResize.getOffsetPoint();
                            location.x += offset.x;
                            location.y += offset.y;
                            divResize.Visible = true;
                            List<FCView> targets = divResize.getTargets();
                            FCSize size = control.Size;
                            divResize.Xml = xml;
                            divResize.clearTargets();
                            targets.Clear();
                            targets.Add(control);
                            divResize.DesignerDiv = selectedDiv;
                            divResize.addTargets(targets);
                            divResize.Bounds = divResize.convertPRectToBounds(new FaceCat.FCRect(location.x, location.y, location.x + control.Size.cx, location.y + control.Size.cy));
                            //设置事件编辑表格
                            m_gridEvent.Xml = xml;
                            m_gridEvent.clearTargets();
                            m_gridEvent.DesignerDiv = selectedDiv;
                            m_gridEvent.addTargets(targets);
                            //设置属性编辑表格
                            m_gridProperty.Xml = xml;
                            m_gridProperty.clearTargets();
                            m_gridProperty.DesignerDiv = selectedDiv;
                            m_gridProperty.addTargets(targets);
                            //设置选中索引
                            FCMenu menu = m_cbList.DropDownMenu;
                            List<FCMenuItem> items = menu.getItems();
                            int itemsSize = items.Count;
                            for (int i = 0; i < itemsSize; i++)
                            {
                                if (items[i].Tag == control)
                                {
                                    m_cbList.SelectedIndex = i;
                                    break;
                                }
                            }
                            if (!(control.Parent is FCLayoutDiv || control.Parent is FCTableLayoutDiv))
                            {
                                control.bringToFront();
                            }
                            divResize.bringToFront();
                            divResize.focus();
                            Native.update();
                            Native.invalidate();
                        }
                        else
                        {
                            //查找父控件
                            if (control.Parent != null)
                            {
                                setCurrentControl(control.Parent);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 显示活动页
        /// </summary>
        public void showActiveTabsMenu()
        {
            if (m_menuActiveTabs == null)
            {
                m_menuActiveTabs = new FCMenu();
                m_menuActiveTabs.Popup = true;
                Native.addControl(m_menuActiveTabs);
                m_menuActiveTabs.addEvent(new FCMenuItemTouchEvent(clickMenuItem), FCEventID.MENUITEMCLICK);
            }
            m_menuActiveTabs.clearItems();
            List<FCTabPage> tabPages = m_tabDesigner.getTabPages();
            int tabPagesSize = tabPages.Count;
            if (tabPagesSize > 0)
            {
                for (int i = 0; i < tabPagesSize; i++)
                {
                    FCTabPage tabPage = tabPages[i];
                    FCMenuItem item = new FCMenuItem(tabPage.Text);
                    item.Tag = tabPage;
                    m_menuActiveTabs.addItem(item);
                }
                m_menuActiveTabs.Location = new FCPoint(Native.clientX(m_btnActiveTab) - 180, Native.clientY(m_btnActiveTab) + m_btnActiveTab.Height);
                m_menuActiveTabs.Size = new FCSize(200, tabPagesSize * 20);
                m_menuActiveTabs.show();
                m_menuActiveTabs.focus();
            }
            else
            {
                m_menuActiveTabs.hide();
            }
            m_menuActiveTabs.update();
            m_menuActiveTabs.invalidate();
        }

        /// <summary>
        /// 显示活动页
        /// </summary>
        /// <param name="tabPage">页</param>
        private void showActiveTabPage(FCTabPage tabPage)
        {
            m_tabDesigner.removeControl(tabPage);
            m_tabDesigner.insertControl(0, tabPage);
            m_tabDesigner.update();
            m_tabDesigner.invalidate();
        }

        /// <summary>
        /// 工具箱添加控件事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="type">控件类型</param>
        private void toolBoxListAddControl(object sender, String type)
        {
            DesignerDiv selectedDiv = SelectedDesignerDiv;
            if (selectedDiv != null)
            {
                FCTabPage designerTabPage = selectedDiv.DesignerTabPage;
                if (designerTabPage.Visible)
                {
                    FCPoint mp = Native.TouchPoint;
                    //获取鼠标区域
                    int clientX = Native.clientX(designerTabPage);
                    int clientY = Native.clientY(designerTabPage);
                    FCSize dSize = designerTabPage.Size;
                    if (mp.x >= clientX && mp.x <= clientX + dSize.cx
                        && mp.y >= clientY && mp.y <= clientY + dSize.cy)
                    {
                        UIXmlEx xml = selectedDiv.Xml;
                        //查找控件
                        ResizeDiv resizeDiv = selectedDiv.ResizeDiv;
                        resizeDiv.AcceptTouch = false;
                        FCView targetControl = Native.findControl(mp, designerTabPage);
                        resizeDiv.AcceptTouch = true;
                        if (targetControl != null)
                        {
                            if (!xml.isContainer(targetControl))
                            {
                                targetControl = targetControl.Parent;
                            }
                            if (targetControl != designerTabPage && xml.isContainer(targetControl) && xml.containsControl(targetControl))
                            {
                                selectedDiv.saveUndo();
                                //创建控件
                                String lowerType = type.ToLower();
                                FCView newControl = xml.createControl(lowerType, targetControl);
                                if (newControl != null)
                                {
                                    UITemplate.AttachTemplate(xml, newControl);
                                    newControl.Location = targetControl.pointToControl(mp);
                                    String controlName = UITemplate.CreateControlName(newControl, xml);
                                    xml.setProperty(newControl, "name", controlName);
                                    if (!(newControl is FCTextBox))
                                    {
                                        xml.setProperty(newControl, "text", controlName);
                                    }
                                    xml.setProperty(newControl, "location", FCStr.convertPointToStr(newControl.Location));
                                    xml.setProperty(newControl, "size", FCStr.convertSizeToStr(newControl.Size));
                                    loadControlList();
                                    setCurrentControl(newControl);
                                    Native.invalidate();
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 前往父级
        /// </summary>
        public void toParent()
        {
            DesignerDiv selectedDiv = SelectedDesignerDiv;
            if (selectedDiv != null)
            {
                List<FCView> targets = selectedDiv.ResizeDiv.getTargets();
                int targetsSize = targets.Count;
                if (targetsSize > 0)
                {
                    FCView parentNodeControl = selectedDiv.Xml.getParentNodeControl(targets[0]);
                    if (parentNodeControl != null)
                    {
                        setCurrentControl(parentNodeControl);
                    }
                }
            }
        }

        /// <summary>
        /// 前往子级
        /// </summary>
        public void toSub()
        {
            DesignerDiv selectedDiv = SelectedDesignerDiv;
            if (selectedDiv != null)
            {
                List<FCView> targets = selectedDiv.ResizeDiv.getTargets();
                int targetsSize = targets.Count;
                if (targetsSize > 0)
                {
                    FCView subNodeControl = selectedDiv.Xml.getSubNodeControl(targets[0]);
                    if (subNodeControl != null)
                    {
                        setCurrentControl(subNodeControl);
                    }
                }
            }
        }

        /// <summary>
        /// 查看代码
        /// </summary>
        public void viewSource()
        {
            DesignerDiv selectedDiv = SelectedDesignerDiv;
            if (selectedDiv != null)
            {
                ResizeDiv resizeDiv = selectedDiv.ResizeDiv;
                if (resizeDiv != null)
                {
                    List<FCView> targets = resizeDiv.getTargets();
                    int targetsSize = targets.Count;
                    if (targetsSize > 0)
                    {
                        FCView target = targets[0];
                        UIXmlEx xml = selectedDiv.Xml;
                        if (target != null && xml.containsControl(target))
                        {
                            XmlNode node = xml.Nodes[target];
                            selectedDiv.SelectedTabPage = selectedDiv.SourceCodeTabPage;
                            if (target.Name != null && target.Name.Length > 0)
                            {
                                selectedDiv.Scintilla.locateToXml("name=\"" + target.Name + "\"");
                            }
                            else
                            {
                                selectedDiv.Scintilla.locateToXml("<" + node.Name);
                            }
                            selectedDiv.Scintilla.Focus();
                            selectedDiv.invalidate();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 撤销
        /// </summary>
        public void undo()
        {
            DesignerDiv selectedDiv = SelectedDesignerDiv;
            if (selectedDiv != null)
            {
                if (selectedDiv.SelectedTabPage == selectedDiv.DesignerTabPage)
                {
                    selectedDiv.undo();
                    return;
                }
            }
            Native.update();
            Native.invalidate();
        }

        /// <summary>
        /// 消息循环
        /// </summary>
        /// <param name="m">消息</param>
        /// <returns>是否处理</returns>
        public bool wndProc(ref Message m)
        {
            FaceCat.FCRect resizeRect = new FaceCat.FCRect();
            bool selected = false;
            if (Native != null)
            {
                ResizeDiv resizeDiv = null;
                if (!isWindowShowing())
                {
                    //键盘操作
                    if (m.Msg == 0x0100)
                    {
                        char key = (char)m.WParam;
                        if (m_host.isKeyPress(0x11))
                        {
                            if (key == 67)
                            {
                                //复制
                                Copy();
                            }
                            else if (key == 88)
                            {
                                //剪切
                                cut();
                            }
                            else if (key == 86)
                            {
                                //粘贴
                                paste();
                            }
                            else if (key == 90)
                            {
                                //撤销
                                undo();
                            }
                            else if (key == 89)
                            {
                                //重做
                                redo();
                            }
                        }
                        else if (key == 27)
                        {
                            //取消
                            resizeDiv = SelectedResizeDiv;
                            if (resizeDiv != null)
                            {
                                resizeDiv.cancelDragging();
                            }
                        }
                    }
                    //鼠标点击
                    else if (m.Msg == 0x0200 || m.Msg == 0x0201)
                    {
                        DesignerDiv selectedDiv = SelectedDesignerDiv;
                        if (selectedDiv != null)
                        {
                            FCTabPage designerTabPage = selectedDiv.DesignerTabPage;
                            if (designerTabPage.Visible)
                            {
                                FCPoint mp = Native.TouchPoint;
                                int clientX = Native.clientX(designerTabPage);
                                int clientY = Native.clientY(designerTabPage);
                                int tWidth = designerTabPage.Width;
                                int tHeight = designerTabPage.Height;
                                if (mp.x >= clientX && mp.x <= clientX + tWidth && mp.y >= clientY && mp.y <= clientY + tHeight)
                                {
                                    if (m_menuActiveTabs == null || (m_menuActiveTabs != null && !m_menuActiveTabs.Visible))
                                    {
                                        resizeDiv = selectedDiv.ResizeDiv;
                                        if (!resizeDiv.isResizing(mp))
                                        {
                                            //查找控件
                                            resizeDiv.AcceptTouch = false;
                                            FCView targetControl = Native.findControl(mp, designerTabPage);
                                            if (targetControl != null)
                                            {
                                                bool canDragParent = false;
                                                if (targetControl.Parent != null)
                                                {
                                                    if (targetControl is FCTabPage)
                                                    {
                                                        canDragParent = true;
                                                    }
                                                    else
                                                    {
                                                        FCView tParent = targetControl.Parent;
                                                        if (tParent is FCTableLayoutDiv)
                                                        {
                                                            canDragParent = true;
                                                        }
                                                        else if (tParent is FCSplitLayoutDiv)
                                                        {
                                                            if (targetControl != (tParent as FCSplitLayoutDiv).Splitter)
                                                            {
                                                                canDragParent = true;
                                                            }
                                                        }
                                                    }
                                                }
                                                if (canDragParent)
                                                {
                                                    FCView tParent = targetControl.Parent;
                                                    int tClientX = Native.clientX(tParent);
                                                    int tClientY = Native.clientY(tParent);
                                                    int pSize = 5;
                                                    if (mp.x <= tClientX + pSize || mp.x >= tClientX + tParent.Width - pSize
                                                        || mp.y <= tClientY + pSize || mp.y >= tClientY + tParent.Height - pSize)
                                                    {

                                                        targetControl = targetControl.Parent;
                                                    }
                                                }
                                                FCView parent = targetControl.Parent;
                                                UIXmlEx xml = selectedDiv.Xml;
                                                //非拖动控件无效果
                                                if (xml.containsControl(targetControl)
                                                    || (!xml.containsControl(targetControl)
                                                    && (parent != null && parent is FCTextBox)))
                                                {
                                                    resizeDiv.AcceptTouch = true;
                                                    if (m.Msg == 0x0201)
                                                    {
                                                        //设置当前控件
                                                        setCurrentControl(targetControl);
                                                        resizeRect = resizeDiv.Bounds;
                                                        selected = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                //触发消息
                if (m_host != null && m_host.onMessage(ref m) > 0)
                {
                    return true;
                }
                //选中图层
                if (selected)
                {
                    resizeDiv.AcceptTouch = true;
                    resizeDiv.Bounds = resizeRect;
                    resizeDiv.Focused = true;
                }
            }
            return false;
        }
    }
}
