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

namespace FaceCat {
    /// <summary>
    /// 多页夹控件
    /// </summary>
    public class FCTabPage : FCDiv {
        /// <summary>
        /// 创建多页夹
        /// </summary>
        public FCTabPage() {
            m_dragHeaderBeginEvent = new FCEvent(DragHeaderBegin);
            m_dragHeaderEndEvent = new FCEvent(DragHeaderEnd);
            m_draggingHeaderEvent = new FCEvent(DraggingHeader);
            m_headerTouchDownEvent = new FCTouchEvent(HeaderTouchDown);
        }

        /// <summary>
        /// 开始拖动页头按钮事件
        /// </summary>
        private FCEvent m_dragHeaderBeginEvent;

        /// <summary>
        /// 结束拖动页头按钮事件
        /// </summary>
        private FCEvent m_dragHeaderEndEvent;

        /// <summary>
        /// 页头按钮拖动中事件
        /// </summary>
        private FCEvent m_draggingHeaderEvent;

        /// <summary>
        /// 页头的触摸按下事件
        /// </summary>
        private FCTouchEvent m_headerTouchDownEvent;

        protected FCButton m_headerButton;

        /// <summary>
        /// 获取或设置页头按钮
        /// </summary>
        public virtual FCButton HeaderButton {
            get { return m_headerButton; }
            set { m_headerButton = value; }
        }

        protected FCPoint m_headerLocation;

        /// <summary>
        /// 获取或设置头部的位置
        /// </summary>
        public virtual FCPoint HeaderLocation {
            get { return m_headerLocation; }
            set { m_headerLocation = value; }
        }

        /// <summary>
        /// 获取或设置头部是否可见
        /// </summary>
        public virtual bool HeaderVisible {
            get {
                if (m_headerButton != null) {
                    return m_headerButton.Visible;
                }
                else {
                    return false;
                }
            }
            set {
                if (m_headerButton != null) {
                    m_headerButton.Visible = value;
                }
            }
        }

        protected FCTabControl m_tabControl;

        /// <summary>
        /// 获取或设置多页夹控件
        /// </summary>
        public virtual FCTabControl TabControl {
            get { return m_tabControl; }
            set { m_tabControl = value; }
        }

        /// <summary>
        /// 销毁方法
        /// </summary>
        public override void delete() {
            if (!IsDeleted) {
                if (m_dragHeaderBeginEvent != null) {
                    if (m_headerButton != null) {
                        m_headerButton.removeEvent(m_dragHeaderBeginEvent, FCEventID.DRAGBEGIN);
                    }
                    m_dragHeaderBeginEvent = null;
                }
                if (m_dragHeaderEndEvent != null) {
                    if (m_headerButton != null) {
                        m_headerButton.removeEvent(m_dragHeaderEndEvent, FCEventID.DRAGEND);
                    }
                    m_dragHeaderEndEvent = null;
                }
                if (m_draggingHeaderEvent != null) {
                    if (m_headerButton != null) {
                        m_headerButton.removeEvent(m_draggingHeaderEvent, FCEventID.DRAGGING);
                    }
                    m_draggingHeaderEvent = null;
                }
                if (m_headerTouchDownEvent != null) {
                    if (m_headerButton != null) {
                        m_headerButton.removeEvent(m_headerTouchDownEvent, FCEventID.TOUCHDOWN);
                    }
                    m_headerTouchDownEvent = null;
                }
                m_headerButton = null;
            }
            base.delete();
        }

        /// <summary>
        /// 开始拖动页头按钮方法
        /// </summary>
        /// <param name="sender">调用者</param>
        protected void DragHeaderBegin(object sender) {
            if (m_tabControl != null) {
                m_tabControl.onDragTabHeaderBegin(this);
            }
        }

        /// <summary>
        /// 结束拖动页头按钮方法
        /// </summary>
        /// <param name="sender">调用者</param>
        protected void DragHeaderEnd(object sender) {
            if (m_tabControl != null) {
                m_tabControl.onDragTabHeaderEnd(this);
            }
        }

        /// <summary>
        /// 页头按钮拖动中方法
        /// </summary>
        /// <param name="sender">调用者</param>
        protected void DraggingHeader(object sender) {
            if (m_tabControl != null) {
                m_tabControl.onDraggingTabHeader(this);
            }
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <returns>控件类型</returns>
        public override String getControlType() {
            return "TabPage";
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">返回属性值</param>
        /// <param name="type">返回属性类型</param>
        public override void getProperty(String name, ref String value, ref String type) {
            if (name == "headersize") {
                type = "size";
                if (m_headerButton != null) {
                    value = FCStr.convertSizeToStr(m_headerButton.Size);
                }
                else {
                    value = "0,0";
                }
            }
            else if (name == "headervisible") {
                type = "bool";
                value = FCStr.convertBoolToStr(HeaderVisible);
            }
            else if (name.IndexOf("header-") != -1) {
                if (m_headerButton != null) {
                    m_headerButton.getProperty(name.Substring(7), ref value, ref type);
                }
            }
            else {
                base.getProperty(name, ref value, ref type);
            }
        }

        /// <summary>
        /// 获取属性名称列表
        /// </summary>
        /// <returns>属性名称列表</returns>
        public override ArrayList<String> getPropertyNames() {
            ArrayList<String> propertyNames = base.getPropertyNames();
            propertyNames.AddRange(new String[] { "Header", "HeaderSize", "HeaderVisible" });
            return propertyNames;
        }

        /// <summary>
        /// 页头触摸按下方法
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="touchInfo">触摸信息</param>
        protected void HeaderTouchDown(object sender, FCTouchInfo touchInfo) {
            if (m_tabControl != null) {
                m_tabControl.SelectedTabPage = this;
                m_tabControl.invalidate();
            }
        }

        /// <summary>
        /// 添加控件方法
        /// </summary>
        public override void onLoad() {
            base.onLoad();
            if (m_tabControl != null) {
                if (m_headerButton == null) {
                    FCHost host = Native.Host;
                    m_headerButton = host.createInternalControl(this, "headerbutton") as FCButton;
                    m_headerButton.addEvent(m_dragHeaderBeginEvent, FCEventID.DRAGBEGIN);
                    m_headerButton.addEvent(m_dragHeaderEndEvent, FCEventID.DRAGEND);
                    m_headerButton.addEvent(m_draggingHeaderEvent, FCEventID.DRAGGING);
                    m_headerButton.addEvent(m_headerTouchDownEvent, FCEventID.TOUCHDOWN);
                }
                if (!m_tabControl.containsControl(m_headerButton)) {
                    m_tabControl.addControl(m_headerButton);
                }

            }
        }

        /// <summary>
        /// 文字改变方法
        /// </summary>
        public override void onTextChanged() {
            base.onTextChanged();
            if (m_headerButton != null) {
                m_headerButton.Text = Text;
            }
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        public override void setProperty(String name, String value) {
            if (name == "headersize") {
                if (m_headerButton != null) {
                    m_headerButton.setProperty("size", value);
                }
            }
            else if (name == "headervisible") {
                HeaderVisible = FCStr.convertStrToBool(value);
            }
            else if (name.IndexOf("header-") != -1) {
                if (m_headerButton != null) {
                    m_headerButton.setProperty(name.Substring(7), value);
                }
            }
            else {
                base.setProperty(name, value);
            }
        }
    }
}
