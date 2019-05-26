/*基于捂脸猫FaceCat框架 v1.0
 捂脸猫创始人-矿洞程序员-脉脉KOL-陶德 (微信号:suade1984);
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Xml;
using System.IO;
using System.Text;
using FaceCat;

namespace FaceCat
{
    /// <summary>
    /// XML界面生成类
    /// </summary>
    public class UIXmlEx:FCUIXml
    {
        /// <summary>
        /// XML对象
        /// </summary>
        private static XmlDocument m_xmlDoc2 = new XmlDocument();

        /// <summary>
        /// 文档改变事件集合
        /// </summary>
        private List<FCEvent> m_documentChangedEvents = new List<FCEvent>();

        public List<FCEvent> DocumentChangedEvents
        {
            get { return m_documentChangedEvents; }
            set { m_documentChangedEvents = value; }
        }

        protected Dictionary<FCView, XmlNode> m_nodes = new Dictionary<FCView, XmlNode>();

        /// <summary>
        /// 获取或设置节点
        /// </summary>
        public Dictionary<FCView, XmlNode> Nodes
        {
            get { return m_nodes; }
            set { m_nodes = value; }
        }

        private double m_scaleFactor = 1;

        /// <summary>
        /// 获取或设置缩放因子
        /// </summary>
        public double ScaleFactor
        {
            get { return m_scaleFactor; }
            set { m_scaleFactor = value; }
        }

        /// <summary>
        /// 获取文档的文本
        /// </summary>
        public String DocumentText
        {
            get
            {
                MemoryStream memoryStream = new MemoryStream();
                m_xmlDoc.Save(memoryStream);
                String documentText = Encoding.UTF8.GetString(memoryStream.ToArray());
                memoryStream.Dispose();
                return documentText;
            }
        }

        /// <summary>
        /// 添加控件到XML中
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="parent">父控件</param>
        public void addControl(FCView control, FCView parent)
        {
            if (m_nodes.ContainsKey(control))
            {
                XmlNode node = m_nodes[control];
                if (m_nodes.ContainsKey(parent))
                {
                    XmlNode parentNode = m_nodes[parent];
                    parentNode.AppendChild(node);
                    parent.addControl(control);
                    m_controls.Add(control);
                    onAddControl(control, node);
                }
            }
        }

        /// <summary>
        /// 复制控件
        /// </summary>
        /// <param name="newName">新的名称</param>
        /// <param name="control">控件</param>
        /// <param name="parent">父控件</param>
        public void copyControl(String newName, FCView control, FCView parent)
        {
            XmlNode parentNode = m_nodes[parent];
            XmlNode newNode = m_nodes[control].Clone();
            XmlAttribute xmlAtr = null;
            //检查是否存在属性
            foreach (XmlAttribute atr in newNode.Attributes)
            {
                if (atr.Name.ToLower() == "name")
                {
                    xmlAtr = atr;
                    break;
                }
            }
            if (xmlAtr == null)
            {
                xmlAtr = m_xmlDoc.CreateAttribute("name");
                newNode.Attributes.Append(xmlAtr);
            }
            xmlAtr.Value = newName;
            parentNode.AppendChild(newNode);
            FCView newControl = createControl(newNode, newNode.Name.ToLower());
            setAttributesBefore(newNode, newControl);
            parent.addControl(newControl);
            setAttributesAfter(newNode, newControl);
            m_controls.Add(newControl);
            onAddControl(newControl, newNode);
        }

        /// <summary>
        /// 检查XML是否合法
        /// </summary>
        /// <param name="xml">XML</param>
        /// <param name="error">错误信息</param>
        /// <returns>是否合法</returns>
        public bool checkXml(String xml, ref String error)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 创建控件
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="type">类型</param>
        /// <returns>控件</returns>
        public override FCView createControl(XmlNode node, String type)
        {
            WinHostEx host = Native.Host as WinHostEx;
            if (host.LoadingDesigner)
            {
                //事件编辑
                if (type == "eventgrid")
                {
                    EventGrid eventGrid = new EventGrid();
                    eventGrid.Xml = this;
                    return eventGrid;
                }
                //属性编辑
                else if (type == "propertygrid")
                {
                    PropertyGrid propertyGrid = new PropertyGrid();
                    propertyGrid.Xml = this;
                    return propertyGrid;
                }
                //拖动层
                else if (type == "resizediv")
                {
                    ResizeDiv resizeDiv = new ResizeDiv();
                    resizeDiv.Xml = this;
                    return resizeDiv;
                }
                //工具箱
                else if (type == "toolboxlist")
                {
                    ToolBoxList toolBoxList = new ToolBoxList();
                    toolBoxList.Xml = this;
                    return toolBoxList;
                }
                //图片按钮
                else if (type == "imagebutton")
                {
                    return new ImageButton();
                }
                //透明按钮
                else if (type == "ribbonbutton")
                {
                    return new RibbonButton();
                }
                //透明按钮2
                else if (type == "ribbonbutton2")
                {
                    return new RibbonButton2();
                }
                //窗体
                else if (type == "window")
                {
                    return new WindowEx();
                }
            }
            else
            {
                 //透明按钮
                if (type == "ribbonbutton")
                {
                    return new RibbonButton();
                }
                //透明按钮2
                else if (type == "ribbonbutton2")
                {
                    return new RibbonButton2();
                }
            }
            return base.createControl(node, type);
        }

        /// <summary>
        /// 创建控件到XML中
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="parent">父控件</param>
        public FCView createControl(String type, FCView parent)
        {
            if (m_nodes.ContainsKey(parent))
            {
                String lowerType = type.ToLower();
                String createType = "";
                if (lowerType == "grid")
                {
                    lowerType = "table";
                }
                else if (lowerType == "splitlayoutdiv")
                {
                    lowerType = "div";
                    createType = "splitlayout";
                }
                else if (lowerType == "tablelayoutdiv")
                {
                    lowerType = "div";
                    createType = "tablelayout";
                }
                else if (lowerType == "layoutdiv")
                {
                    lowerType = "div";
                    createType = "layout";
                }
                else if (lowerType == "groupbox")
                {
                    lowerType = "div";
                    createType = "groupbox";
                }
                else if (lowerType == "tabcontrol")
                {
                    lowerType = "div";
                    createType = "tabcontrol";
                }
                else if (lowerType == "tabpage")
                {
                    lowerType = "div";
                    createType = "tabpage";
                }
                else if (lowerType == "button")
                {
                    lowerType = "input";
                    createType = "button";
                }
                else if (lowerType == "checkbox")
                {
                    lowerType = "input";
                    createType = "checkbox";
                }
                else if (lowerType == "datepicker")
                {
                    lowerType = "input";
                    createType = "datetime";
                }
                else if (lowerType == "radiobutton")
                {
                    lowerType = "input";
                    createType = "radio";
                }
                else if (lowerType == "spin")
                {
                    lowerType = "input";
                    createType = "range";
                }
                else if (lowerType == "textbox")
                {
                    lowerType = "input";
                    createType = "text";
                }
                else if (lowerType == "combobox")
                {
                    lowerType = "select";
                }
                else if (lowerType == "linklabel")
                {
                    lowerType = "a";
                }
                XmlNode parentNode = m_nodes[parent];
                XmlNode node = m_xmlDoc.CreateNode(XmlNodeType.Element, lowerType, "");
                parentNode.AppendChild(node);
                if (createType != null && createType.Length > 0)
                {
                    XmlAttribute xmlAtr = m_xmlDoc.CreateAttribute("type");
                    xmlAtr.Value = createType;
                    node.Attributes.Append(xmlAtr);
                }
                FCView control = createControl(node, type);
                parent.addControl(control);
                m_nodes[control] = node;
                onAddControl(control, node);
                return control;
            }
            return null;
        }

        /// <summary>
        /// 创建用户控件
        /// </summary>
        /// <param name="node">节电</param>
        /// <returns>控件</returns>
        protected override FCView createUserControl(XmlNode node)
        {
            FCView userControl = base.createUserControl(node);
            if (userControl == null)
            {
                UserControlEx userControlEx = new UserControlEx();
                userControlEx.Native = Native;
                Dictionary<String, String> attributes = getAttributes(node);
                if (attributes.ContainsKey("cid"))
                {
                    userControlEx.Cid = attributes["cid"];
                }
                if (attributes.ContainsKey("iscontainer"))
                {
                    userControlEx.IsContainer = attributes["iscontainer"].ToLower() == "true";
                }
                userControl = userControlEx;
            }
            return userControl; 
        }

        /// <summary>
        /// 获取父节点控件
        /// </summary>
        /// <param name="control">控件</param>
        public FCView getParentNodeControl(FCView control)
        {
            FCView parent = control.Parent;
            if (parent != null)
            {
                if (m_controls.Contains(parent))
                {
                    return parent;
                }
                else
                {
                    return getParentNodeControl(parent);
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取子节点控件
        /// </summary>
        /// <param name="control">控件</param>
        public FCView getSubNodeControl(FCView control)
        {
            List<FCView> controls = control.m_controls;
            int controlsSize = controls.Count;
            for (int i = 0; i < controlsSize; i++)
            {
                if (m_controls.Contains(controls[i]))
                {
                    return controls[i];
                }
            }
            for (int i = 0; i < controlsSize; i++)
            {
                FCView subNodeControl = getSubNodeControl(controls[i]);
                if (subNodeControl != null)
                {
                    return subNodeControl;
                }
            }
            return null;
        }

        /// <summary>
        /// 判断是否容器
        /// </summary>
        /// <param name="control">控件</param>
        /// <returns>是否容器</returns>
        public bool isContainer(FCView control)
        {
            if (control is UserControlEx)
            {
                UserControlEx userControl = control as UserControlEx;
                return userControl.IsContainer;
            }
            else if (control is FCLabel)
            {
                return false;
            }
            else if (control is FCButton)
            {
                return false;
            }
            else if (control is FCCalendar)
            {
                return false;
            }
            else if (control is FCTextBox)
            {
                return false;
            }
            else if (control is FCGrid)
            {
                return false;
            }
            else if (control is FCChart)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 获取集合的交集
        /// </summary>
        /// <returns>集合</returns>
        public static List<String> getUnionProperties(List<FCView> controls)
        {
            List<String> unionProperites = new List<String>();
            Dictionary<String, int> propertyNames = new Dictionary<String, int>();
            int targetsSize = controls.Count;
            for (int i = 0; i < targetsSize; i++)
            {
                FCView target = controls[i];
                List<String> pList = target.getPropertyNames();
                int pListSize = pList.Count;
                for (int j = 0; j < pListSize; j++)
                {
                    String pName = pList[j];
                    if (!propertyNames.ContainsKey(pName))
                    {
                        propertyNames[pName] = 1;
                    }
                    else
                    {
                        propertyNames[pName] = propertyNames[pName] + 1;
                    }
                }
            }
            foreach (String key in propertyNames.Keys)
            {
                if (propertyNames[key] == targetsSize)
                {
                    unionProperites.Add(key);
                }
            }
            propertyNames.Clear();
            return unionProperites;
        }

        /// <summary>
        /// 加载到某个控件中
        /// </summary>
        /// <param name="skinPath">文件的路径</param>
        /// <param name="control">控件</param>
        public override void loadFile(String fileName, FCView control)
        {
            if (control != null)
            {
                m_nodes.Clear();
            }
            base.loadFile(fileName, control);
        }

        /// <summary>
        /// 读取字符串，加载到控件中
        /// </summary>
        /// <param name="xml">字符串</param>
        /// <param name="control">控件</param>
        public override void loadXml(String xml, FCView control)
        {
            if (control != null)
            {
                m_nodes.Clear();
            }
            base.loadXml(xml, control);
        }

        /// <summary>
        /// 添加控件方法
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="node">节点</param>
        public override void onAddControl(FCView control, XmlNode node)
        {
            base.onAddControl(control, node);
            m_nodes[control] = node;
            onDocumentChanged();
        }

        /// <summary>
        /// 文档改变方法
        /// </summary>
        public virtual void onDocumentChanged()
        {
            int eventSize = m_documentChangedEvents.Count;
            if (eventSize > 0)
            {
                for (int i = 0; i < eventSize; i++)
                {
                    m_documentChangedEvents[i](null);
                }
            }
        }

        /// <summary>
        /// 读取头部
        /// </summary>
        /// <param name="node">XML节点</param>
        /// <param name="control">控件</param>
        public override void readHead(XmlNode node, FCView control)
        {
            WinHostEx host = Native.Host as WinHostEx;
            if (host.LoadingDesigner)
            {
                base.readHead(node, control);
            }
        }

        /// <summary>
        /// 将控件从XML中移除
        /// </summary>
        /// <param name="control">控件</param>
        public void removeControl(FCView control)
        {
            if (m_nodes.ContainsKey(control))
            {
                XmlNode node = m_nodes[control];
                FCView parent = control.Parent;
                if (parent != null)
                {
                    if (m_nodes.ContainsKey(parent))
                    {
                        XmlNode parentNode = m_nodes[parent];
                        parentNode.RemoveChild(node);
                        parent.removeControl(control);
                        m_controls.Remove(control);
                        onDocumentChanged();
                    }
                }
            }
        }

        /// <summary>
        /// 移除属性
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="name">属性名称</param>
        public void removeProperty(FCView control, String name)
        {
            if (m_nodes.ContainsKey(control))
            {
                XmlNode node = m_nodes[control];
                XmlAttribute xmlAtr = null;
                //检查是否存在属性
                foreach (XmlAttribute atr in node.Attributes)
                {
                    if (atr.Name.ToLower() == name.ToLower())
                    {
                        xmlAtr = atr;
                        break;
                    }
                }
                if (xmlAtr != null)
                {
                    node.Attributes.Remove(xmlAtr);
                    onDocumentChanged();
                }
            }
        }

        /// <summary>
        /// 重置缩放尺寸
        /// </summary>
        /// <param name="clientSize">客户端大小</param>
        public void resetScaleSize(FCSize clientSize)
        {
            FCNative native = Native;
            if (native != null)
            {
                native.ScaleSize = new FCSize((int)(clientSize.cx * m_scaleFactor), (int)(clientSize.cy * m_scaleFactor));
                native.update();
            }
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        public void saveFile(String fileName)
        {
            if (fileName != null)
            {
                m_xmlDoc.Save(fileName);
            }
        }

        /// <summary>
        /// 将属性保存到XML中
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        public void setProperty(FCView control, String name, String value)
        {
            if (m_nodes.ContainsKey(control))
            {
                XmlNode node = m_nodes[control];
                XmlAttribute xmlAtr = null;
                //检查是否存在属性
                foreach (XmlAttribute atr in node.Attributes)
                {
                    if (atr.Name.ToLower() == name.ToLower())
                    {
                        xmlAtr = atr;
                        break;
                    }
                }
                if (xmlAtr == null)
                {
                    xmlAtr = m_xmlDoc.CreateAttribute(name.ToLower());
                    node.Attributes.Append(xmlAtr);
                }
                xmlAtr.Value = value;
                control.setProperty(name.ToLower(), value);
                onDocumentChanged();
            }
        }
    }

    /// <summary>
    /// Xml解析
    /// </summary>
    public class UIXmlEx2 : FCUIXml
    {
        private double m_scaleFactor = 1;

        /// <summary>
        /// 获取或设置缩放因子
        /// </summary>
        public double ScaleFactor
        {
            get { return m_scaleFactor; }
            set { m_scaleFactor = value; }
        }

        /// <summary>
        /// 创建控件
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="type">类型</param>
        /// <returns>控件</returns>
        public override FCView createControl(XmlNode node, String type)
        {
            FCNative native = Native;
            if (type == "ribbonbutton")
            {
                return new RibbonButton();
            }
            //透明按钮2
            else if (type == "ribbonbutton2")
            {
                return new RibbonButton2();
            }
            else if (type == "propertygrid")
            {
                PropertyGrid propertyGrid = new PropertyGrid();
                return propertyGrid;
            }
            else if (type == "windowex")
            {
                return new WindowEx();
            }
            else
            {
                return base.createControl(node, type);
            }
        }

        /// <summary>
        /// 重置缩放尺寸
        /// </summary>
        /// <param name="clientSize">客户端大小</param>
        public void resetScaleSize(FCSize clientSize)
        {
            FCNative native = Native;
            if (native != null)
            {
                native.ScaleSize = new FCSize((int)(clientSize.cx * m_scaleFactor), (int)(clientSize.cy * m_scaleFactor));
                native.update();
            }
        }
    }

    /// <summary>
    /// Xml解析
    /// </summary>
    public class UIXmlEx3 : FCUIXml
    {
        private double m_scaleFactor = 1;

        /// <summary>
        /// 获取或设置缩放因子
        /// </summary>
        public double ScaleFactor
        {
            get { return m_scaleFactor; }
            set { m_scaleFactor = value; }
        }

        /// <summary>
        /// 创建控件
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="type">类型</param>
        /// <returns>控件</returns>
        public override FCView createControl(XmlNode node, String type)
        {
            FCNative native = Native;
            if (type == "ribbonbutton")
            {
                return new RibbonButton();
            }
            //透明按钮2
            else if (type == "ribbonbutton2")
            {
                return new RibbonButton2();
            }
            else if (type == "propertygrid")
            {
                PropertyGrid propertyGrid = new PropertyGrid();
                return propertyGrid;
            }
            else if (type == "windowex")
            {
                FCWindow window = new FCWindow();
                window.Native = Native;
                window.show();
                return window;
            }
            else
            {
                return base.createControl(node, type);
            }
        }

        /// <summary>
        /// 重置缩放尺寸
        /// </summary>
        /// <param name="clientSize">客户端大小</param>
        public void resetScaleSize(FCSize clientSize)
        {
            FCNative native = Native;
            if (native != null)
            {
                native.ScaleSize = new FCSize((int)(clientSize.cx * m_scaleFactor), (int)(clientSize.cy * m_scaleFactor));
                native.update();
            }
        }
    }

    /// <summary>
    /// 窗体XML扩展
    /// </summary>
    public class WindowXmlEx : UIXmlEx2
    {
        /// <summary>
        /// 调用控件方法事件
        /// </summary>
        private FCInvokeEvent m_invokeEvent;

        /// <summary>
        /// Windows窗体
        /// </summary>
        protected BugHoleForm m_winForm;

        protected bool m_isWinForm;

        /// <summary>
        /// 获取或设置是否是Windows窗体
        /// </summary>
        public bool IsWinForm
        {
            get { return m_isWinForm; }
            set { m_isWinForm = value; }
        }

        protected WindowEx m_window;

        /// <summary>
        /// 获取或设置窗体
        /// </summary>
        public WindowEx Window
        {
            get { return m_window; }
        }

        /// <summary>
        /// 按钮点击事件
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
                if (m_window != null && control == m_window.CloseButton)
                {
                    close();
                }
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public virtual void close()
        {
            m_window.invoke("close");
        }

        /// <summary>
        /// 销毁方法
        /// </summary>
        public override void delete()
        {
            if (!IsDeleted)
            {
                if (m_winForm != null && m_winForm.Window != null)
                {
                    m_winForm.Window = null;
                    m_winForm.Close();
                    m_winForm = null;
                }
                if (m_window != null)
                {
                    m_window.removeEvent(m_invokeEvent, FCEventID.INVOKE);
                    m_invokeEvent = null;
                    m_window.close();
                    m_window.delete();
                    m_window = null;
                }
                base.delete();
            }
        }

        /// <summary>
        /// 加载界面
        /// </summary>
        public virtual void load(FCNative native, String xmlName, String windowName)
        {
            Native = native;
            String xmlPath = DataCenter.GetAppPath() + "\\config\\" + xmlName + ".html";
            Script = new DesignerScript(this);
            loadFile(xmlPath, null);
            m_window = findControl(windowName) as WindowEx;
            m_invokeEvent = new FCInvokeEvent(invoke);
            if (m_window != null)
            {
                m_window.addEvent(m_invokeEvent, FCEventID.INVOKE);
                //注册点击事件
                registerEvents(m_window);
            }
        }

        /// <summary>
        /// 调用控件线程方法
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="args">参数</param>
        private void invoke(object sender, object args)
        {
            onInvoke(args);
        }

        /// <summary>
        /// 调用控件线程方法
        /// </summary>
        /// <param name="args">参数</param>
        public void onInvoke(object args)
        {
            if (args != null && args.ToString() == "close")
            {
                delete();
                Native.invalidate();
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
                FCButton button = controls[i] as FCButton;
                if (button != null)
                {
                    button.addEvent(clickButtonEvent, FCEventID.CLICK);
                }
                registerEvents(controls[i]);
            }
        }

        /// <summary>
        /// 显示
        /// </summary>
        public virtual void show()
        {
            if (m_isWinForm)
            {
                FCSize windowSize = new FCSize();
                List<FCView> controls = m_window.m_controls;
                int controlsSize = controls.Count;
                m_winForm = new BugHoleForm();
                for (int i = 0; i < controlsSize; i++)
                {
                    FCView subControl = controls[i];
                    if (!(subControl is WindowButton))
                    {
                        windowSize = subControl.Size;
                        subControl.Margin = new FCPadding(0, 0, 0, 0);
                        m_winForm.addBugHole(Native, subControl);
                        break;
                    }
                }
                Native = m_winForm.Native;
                m_winForm.Text = m_window.Text;
                if (m_window.WindowState == WindowStateA.Max)
                {
                    m_winForm.WindowState = FormWindowState.Maximized;
                }
                else if (m_window.WindowState == WindowStateA.Min)
                {
                    m_winForm.WindowState = FormWindowState.Minimized;
                }
                else
                {
                    m_winForm.ClientSize = new Size(windowSize.cx, windowSize.cy);
                }
                m_winForm.Window = this;
                m_winForm.Show();
            }
            else
            {
                m_window.Location = new FCPoint(-m_window.Width, -m_window.Height);
                m_window.animateShow(false);
                m_window.invalidate();
            }
        }

        /// <summary>
        /// 显示
        /// </summary>
        public virtual void showDialog()
        {
            if (m_isWinForm)
            {
                FCSize windowSize = new FCSize();
                List<FCView> controls = m_window.m_controls;
                int controlsSize = controls.Count;
                m_winForm = new BugHoleForm();
                for (int i = 0; i < controlsSize; i++)
                {
                    FCView subControl = controls[i];
                    if (!(subControl is WindowButton))
                    {
                        windowSize = subControl.Size;
                        subControl.Margin = new FCPadding(0, 0, 0, 0);
                        m_winForm.addBugHole(Native, subControl);
                        break;
                    }
                }
                Native = m_winForm.Native;
                m_winForm.Text = m_window.Text;
                if (m_window.WindowState == WindowStateA.Max)
                {
                    m_winForm.WindowState = FormWindowState.Maximized;
                }
                else if (m_window.WindowState == WindowStateA.Min)
                {
                    m_winForm.WindowState = FormWindowState.Minimized;
                }
                else
                {
                    m_winForm.ClientSize = new Size(windowSize.cx, windowSize.cy);
                }
                m_winForm.Window = this;
                m_winForm.ShowDialog();
            }
            else
            {
                m_window.Location = new FCPoint(-m_window.Width, -m_window.Height);
                m_window.animateShow(true);
                m_window.invalidate();
            }
        }
    }
}