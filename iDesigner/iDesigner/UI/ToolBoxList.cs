/*基于捂脸猫FaceCat框架 v1.0
 捂脸猫创始人-矿洞程序员-脉脉KOL-陶德 (微信号:suade1984);
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using FaceCat;

namespace FaceCat
{
    /// <summary>
    /// 工具箱添加控件委托
    /// </summary>
    /// <param name="sender">调用者</param>
    /// <param name="type">控件类型</param>
    public delegate void ToolBoxListAddControlEvent(object sender, String type);

    /// <summary>
    /// 工具箱列表控件
    /// </summary>
    public class ToolBoxList : FCLayoutDiv
    {
        /// <summary>
        /// 创建网格
        /// </summary>
        public ToolBoxList()
        {
            BorderColor = FCColor.None;
            LayoutStyle = FCLayoutStyle.TopToBottom;
            Padding = new FCPadding(5, 3, 5, 0);
            ShowVScrollBar = true;
        }

        public const int EVENTID_TOOLBOXADDCONTROL = 10000;

        /// <summary>
        /// 添加控件事件集合
        /// </summary>
        protected List<ToolBoxListAddControlEvent> m_createControlEvents;

        /// <summary>
        /// 拖动项
        /// </summary>
        private ImageButton m_dragingItem;

        private UIXmlEx m_xml;

        /// <summary>
        /// 获取或设置XML解析
        /// </summary>
        public UIXmlEx Xml
        {
            get { return m_xml; }
            set { m_xml = value; }
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="func">函数指针</param>
        /// <param name="eventID">事件ID</param>
        public override void addEvent(object func, int eventID) {
            switch (eventID) {
                case EVENTID_TOOLBOXADDCONTROL:
                    ToolBoxListAddControlEvent toolBoxAddControlEvent = (ToolBoxListAddControlEvent)func;
                    if (toolBoxAddControlEvent != null) {
                        if (m_createControlEvents == null) {
                            m_createControlEvents = new List<ToolBoxListAddControlEvent>();
                        }
                        m_createControlEvents.Add(toolBoxAddControlEvent);
                    }
                    break;
                default:
                    base.addEvent(func, eventID);
                    break;
            }
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <returns>控件类型</returns>
        public override String getControlType()
        {
            return "ToolBoxList";
        }

        /// <summary>
        /// 添加控件方法
        /// </summary>
        public override void onLoad()
        {
            base.onLoad();
            //创建拖动层
            if (m_dragingItem == null)
            {
                m_dragingItem = new ImageButton();
                m_dragingItem.Size = new FCSize(20, 20);
                m_dragingItem.Visible = false;
                Native.addControl(m_dragingItem);
                String toolBoxItems = "Button;Calendar;CheckBox;ComboBox;DateTimePicker;Div;Grid;GroupBox;Label;LayoutDiv;LinkLabel;RadioButton;Spin;SplitLayoutDiv;TabControl;TableLayoutDiv;TextBox;Tree;Window";
                String toolBoxItemsText = "按钮;日历;复选框;下拉列表;日期选择;图层;表格;组合框;标签;布局层;超链接;单选按钮;数值文本框;分割层;多页夹;表格布局层;文本框;树形;窗体";
                String[] items = toolBoxItems.Split(new String[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                String[] items2 = toolBoxItemsText.Split(new String[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                int itemsSize = items.Length;
                for (int i = 0; i < itemsSize; i++)
                {
                    String item = items[i];
                    String itemText = items2[i];
                    //创建按钮控件
                    ImageButton toolBoxControlButton = new ImageButton();
                    addControl(toolBoxControlButton);
                    toolBoxControlButton.Name = item;
                    toolBoxControlButton.Text = itemText + "(" + item + ")";
                    toolBoxControlButton.Tag = item.ToLower();
                    toolBoxControlButton.BackImage = "file='\\images\\" + item + ".bmp' highcolor='255,0,255' lowcolor='255,0,255'";
                    //注册事件
                    toolBoxControlButton.addEvent(new FCTouchEvent(toolBoxItemTouchDown), FCEventID.TOUCHDOWN);
                    toolBoxControlButton.addEvent(new FCTouchEvent(toolBoxItemTouchMove), FCEventID.TOUCHMOVE);
                    toolBoxControlButton.addEvent(new FCTouchEvent(toolBoxItemTouchUp), FCEventID.TOUCHUP);
                }
            }
        }

        /// <summary>
        /// 创建控件方法
        /// </summary>
        /// <param name="type">类型</param>
        public virtual void onCreateControl(String type)
        {
            if (m_createControlEvents != null)
            {
                List<ToolBoxListAddControlEvent> eventsCopy = new List<ToolBoxListAddControlEvent>();
                int eventSize = m_createControlEvents.Count;
                for (int i = 0; i < eventSize; i++)
                {
                    eventsCopy.Add(m_createControlEvents[i]);
                }
                eventSize = eventsCopy.Count;
                for (int i = 0; i < eventSize; i++)
                {
                    eventsCopy[i](this, type);
                }
            }
        }

        /// <summary>
        /// 重绘背景方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void onPaintBackground(FCPaint paint, FCRect clipRect)
        {
            if (FCDraw.m_style == 0)
            {
                int width = Width, height = Height;
                FCRect drawRect = new FCRect(0, 0, width, height);
                paint.fillGradientRect(FCDraw.FCCOLORS_BACKCOLOR, FCDraw.FCCOLORS_BACKCOLOR2, drawRect, 0, 0);
            }
            else
            {
                base.onPaintBackground(paint, clipRect);
            }
        }

        /// <summary>
        /// 取消注册事件
        /// </summary>
        /// <param name="func">函数指针</param>
        /// <param name="eventID">事件ID</param>
        public override void removeEvent(object func, int eventID) {
            switch (eventID) {
                case EVENTID_TOOLBOXADDCONTROL:
                    ToolBoxListAddControlEvent toolBoxAddControlEvent = (ToolBoxListAddControlEvent)func;
                    if (toolBoxAddControlEvent != null) {
                        m_createControlEvents.Remove(toolBoxAddControlEvent);
                    }
                    break;
                default:
                    base.removeEvent(func, eventID);
                    break;
            }
        }

        /// <summary>
        /// 控件项鼠标按下事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚轮值</param>
        private void toolBoxItemTouchDown(object sender, FCTouchInfo touchInto)
        {
            if (touchInto.m_firstTouch && touchInto.m_clicks == 1)
            {
                if (m_xml != null)
                {
                    FCPoint mp = touchInto.m_firstPoint;
                    FCView control = sender as FCView;
                    m_dragingItem.Visible = true;
                    m_dragingItem.Text = control.Tag.ToString();
                    m_dragingItem.Location = control.pointToNative(mp);
                    m_dragingItem.BackImage = "file='\\images\\" + control.Name + ".bmp' highcolor='255,0,255' lowcolor='255,0,255'";
                    Native.invalidate();
                }
            }
        }

        /// <summary>
        /// 控件项鼠标移动事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚轮值</param>
        private void toolBoxItemTouchMove(object sender, FCTouchInfo touchInto)
        {
            if (m_dragingItem.Visible)
            {
                FCView control = sender as FCView;
                FCPoint mp = touchInto.m_firstPoint;
                FCPoint location = control.pointToNative(mp);
                location.x += 10;
                location.y -= 16;
                m_dragingItem.Location = location;
                Native.invalidate();
            }
        }

        /// <summary>
        /// 控件项鼠标抬起事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚轮值</param>
        private void toolBoxItemTouchUp(object sender, FCTouchInfo touchInto)
        {
            if (m_dragingItem.Visible)
            {
                m_dragingItem.Visible = false;
                onCreateControl(m_dragingItem.Text);
                Native.invalidate();
            }
        }
    }
}
