/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

using System;
using System.Collections.Generic;

namespace FaceCat {
    /// <summary>
    /// 分割布局控件
    /// </summary>
    public class FCSplitLayoutDiv : FCLayoutDiv {
        /// <summary>
        /// 创建分割布局控件
        /// </summary>
        public FCSplitLayoutDiv() {
            m_splitterDraggingEvent = new FCEvent(SplitterDragging);
        }

        /// <summary>
        /// 分割百分比
        /// </summary>
        protected float m_splitPercent = -1;

        protected FCEvent m_splitterDraggingEvent;

        protected FCView m_firstControl;

        /// <summary>
        /// 获取或设置第一个控件
        /// </summary>
        public virtual FCView FirstControl {
            get { return m_firstControl; }
            set { m_firstControl = value; }
        }

        protected FCView m_secondControl;

        /// <summary>
        /// 获取或设置第二个控件
        /// </summary>
        public virtual FCView SecondControl {
            get { return m_secondControl; }
            set { m_secondControl = value; }
        }

        protected FCSizeType m_splitMode = FCSizeType.AbsoluteSize;

        /// <summary>
        /// 获取或设置分割模式
        /// </summary>
        public virtual FCSizeType SplitMode {
            get { return m_splitMode; }
            set { m_splitMode = value; }
        }

        protected FCButton m_splitter;

        /// <summary>
        /// 获取分割按钮
        /// </summary>
        public virtual FCButton Splitter {
            get { return m_splitter; }
        }

        /// <summary>
        /// 销毁方法
        /// </summary>
        public override void delete() {
            if (!IsDeleted) {
                if (m_splitterDraggingEvent != null) {
                    if (m_splitter != null) {
                        m_splitter.removeEvent(m_splitterDraggingEvent, FCEventID.DRAGGING);
                        m_splitterDraggingEvent = null;
                    }
                }
            }
            base.delete();
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <returns>控件类型</returns>
        public override String getControlType() {
            return "SplitLayoutDiv";
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">返回属性值</param>
        /// <param name="type">返回属性类型</param>
        public override void getProperty(String name, ref String value, ref String type) {
            if (name == "candragsplitter") {
                type = "bool";
                if (m_splitter != null) {
                    value = FCStr.convertBoolToStr(m_splitter.AllowDrag);
                }
                else {
                    value = "False";
                }
            }
            else if (name == "splitmode") {
                type = "enum:FCSizeType";
                if (SplitMode == FCSizeType.AbsoluteSize) {
                    value = "AbsoluteSize";
                }
                else {
                    value = "PercentSize";
                }
            }
            else if (name.IndexOf("splitter-") != -1) {
                if (m_splitter != null) {
                    m_splitter.getProperty(name.Substring(9), ref value, ref type);
                }
            }
            else if (name == "splitterposition") {
                type = "str";
                if (m_splitter != null) {
                    if (m_layoutStyle == FCLayoutStyle.TopToBottom || m_layoutStyle == FCLayoutStyle.BottomToTop) {
                        value = FCStr.convertIntToStr(m_splitter.Top);
                        if (m_splitter.Height > 0) {
                            value = value + "," + FCStr.convertIntToStr(m_splitter.Height);
                        }
                    } else {
                        value = FCStr.convertIntToStr(m_splitter.Left);
                        if (m_splitter.Width > 0) {
                            value = value + "," + FCStr.convertIntToStr(m_splitter.Width);
                        }
                    }
                }
            }
            else if (name == "splittervisible") {
                type = "bool";
                if (m_splitter != null) {
                    value = FCStr.convertBoolToStr(m_splitter.Visible);
                }
                else {
                    value = "False";
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
            propertyNames.AddRange(new String[] { "CanDragSplitter", "SplitMode", "Splitter", "SplitterPosition", "SplitterVisible" });
            return propertyNames;
        }

        /// <summary>
        /// 添加控件方法
        /// </summary>
        public override void onLoad() {
            base.onLoad();
            if (m_splitter == null) {
                FCHost host = Native.Host;
                m_splitter = host.createInternalControl(this, "splitter") as FCButton;
                m_splitter.addEvent(m_splitterDraggingEvent, FCEventID.DRAGGING);
                addControl(m_splitter);
            }
            m_oldSize = Size;
        }

        /// <summary>
        /// 重置布局
        /// </summary>
        /// <returns></returns>
        public override bool onResetLayout() {
            bool reset = false;
            if (Native != null && m_splitter != null && m_firstControl != null && m_secondControl != null) {
                FCRect splitRect = new FCRect();
                int width = Width, height = Height;
                FCRect fRect = new FCRect();
                FCRect sRect = new FCRect();
                FCSize splitterSize = new FCSize(0, 0);
                if (m_splitter.Visible) {
                    splitterSize.cx = m_splitter.Width;
                    splitterSize.cy = m_splitter.Height;
                }
                FCLayoutStyle layoutStyle = LayoutStyle;
                switch (layoutStyle) {
                    //自下而上
                    case FCLayoutStyle.BottomToTop:
                        //绝对大小
                        if (m_splitMode == FCSizeType.AbsoluteSize || m_oldSize.cy == 0) {
                            splitRect.left = 0;
                            splitRect.top = height - (m_oldSize.cy - m_splitter.Top);
                            splitRect.right = width;
                            splitRect.bottom = splitRect.top + splitterSize.cy;
                        }
                        //百分比大小
                        else if (m_splitMode == FCSizeType.PercentSize) {
                            splitRect.left = 0;
                            if (m_splitPercent == -1) {
                                m_splitPercent = (float)m_splitter.Top / m_oldSize.cy;
                            }
                            splitRect.top = (int)(height * m_splitPercent);
                            splitRect.right = width;
                            splitRect.bottom = splitRect.top + splitterSize.cy;
                        }
                        fRect.left = 0;
                        fRect.top = splitRect.bottom;
                        fRect.right = width;
                        fRect.bottom = height;
                        sRect.left = 0;
                        sRect.top = 0;
                        sRect.right = width;
                        sRect.bottom = splitRect.top;
                        break;
                    //从左向右
                    case FCLayoutStyle.LeftToRight:
                        //绝对大小
                        if (m_splitMode == FCSizeType.AbsoluteSize || m_oldSize.cx == 0) {
                            splitRect.left = m_splitter.Left;
                            splitRect.top = 0;
                            splitRect.right = m_splitter.Left + splitterSize.cx;
                            splitRect.bottom = height;
                        }
                        //百分比大小
                        else if (m_splitMode == FCSizeType.PercentSize) {
                            if (m_splitPercent == -1) {
                                m_splitPercent = (float)m_splitter.Left / m_oldSize.cx;
                            }
                            splitRect.left = (int)(width * m_splitPercent);
                            splitRect.top = 0;
                            splitRect.right = splitRect.left + splitterSize.cx;
                            splitRect.bottom = height;
                        }
                        fRect.left = 0;
                        fRect.top = 0;
                        fRect.right = splitRect.left;
                        fRect.bottom = height;
                        sRect.left = splitRect.right;
                        sRect.top = 0;
                        sRect.right = width;
                        sRect.bottom = height;
                        break;
                    //从右向左
                    case FCLayoutStyle.RightToLeft:
                        //绝对大小
                        if (m_splitMode == FCSizeType.AbsoluteSize || m_oldSize.cx == 0) {
                            splitRect.left = width - (m_oldSize.cx - m_splitter.Left);
                            splitRect.top = 0;
                            splitRect.right = splitRect.left + splitterSize.cx;
                            splitRect.bottom = height;
                        }
                        //百分比大小
                        else if (m_splitMode == FCSizeType.PercentSize) {
                            if (m_splitPercent == -1) {
                                m_splitPercent = (float)m_splitter.Left / m_oldSize.cx;
                            }
                            splitRect.left = (int)(width * m_splitPercent);
                            splitRect.top = 0;
                            splitRect.right = splitRect.left + splitterSize.cx;
                            splitRect.bottom = height;
                        }
                        fRect.left = splitRect.right;
                        fRect.top = 0;
                        fRect.right = width;
                        fRect.bottom = height;
                        sRect.left = 0;
                        sRect.top = 0;
                        sRect.right = splitRect.left;
                        sRect.bottom = height;
                        break;
                    //自上而下
                    case FCLayoutStyle.TopToBottom:
                        //绝对大小
                        if (m_splitMode == FCSizeType.AbsoluteSize || m_oldSize.cy == 0) {
                            splitRect.left = 0;
                            splitRect.top = m_splitter.Top;
                            splitRect.right = width;
                            splitRect.bottom = splitRect.top + splitterSize.cy;
                        }
                        //百分比大小
                        else if (m_splitMode == FCSizeType.PercentSize) {
                            splitRect.left = 0;
                            if (m_splitPercent == -1) {
                                m_splitPercent = (float)m_splitter.Top / m_oldSize.cy;
                            }
                            splitRect.top = (int)(height * m_splitPercent);
                            splitRect.right = width;
                            splitRect.bottom = splitRect.top + splitterSize.cy;
                        }
                        fRect.left = 0;
                        fRect.top = 0;
                        fRect.right = width;
                        fRect.bottom = splitRect.top;
                        sRect.left = 0;
                        sRect.top = splitRect.bottom;
                        sRect.right = width;
                        sRect.bottom = height;
                        break;
                }
                if (m_splitter.Visible) {
                    FCRect spRect = m_splitter.Bounds;
                    if (spRect.left != splitRect.left || spRect.top != splitRect.top || spRect.right != splitRect.right || spRect.bottom != splitRect.bottom) {
                        m_splitter.Bounds = splitRect;
                        reset = true;
                    }
                    if (m_splitter.AllowDrag) {
                        if (layoutStyle == FCLayoutStyle.LeftToRight || layoutStyle == FCLayoutStyle.RightToLeft) {
                            m_splitter.Cursor = FCCursors.SizeWE;
                        }
                        else {
                            m_splitter.Cursor = FCCursors.SizeNS;
                        }
                        m_splitter.bringToFront();
                    }
                }
                FCRect fcRect = m_firstControl.Bounds;
                if (fcRect.left != fRect.left || fcRect.top != fRect.top || fcRect.right != fRect.right || fcRect.bottom != fRect.bottom) {
                    reset = true;
                    m_firstControl.Bounds = fRect;
                    m_firstControl.update();
                }
                FCRect scRect = m_secondControl.Bounds;
                if (scRect.left != sRect.left || scRect.top != sRect.top || scRect.right != sRect.right || scRect.bottom != sRect.bottom) {
                    reset = true;
                    m_secondControl.Bounds = sRect;
                    m_secondControl.update();
                }
            }
            m_oldSize = Size;
            return reset;
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        public override void setProperty(String name, String value) {
            if (name == "candragsplitter") {
                if (m_splitter != null) {
                    m_splitter.AllowDrag = FCStr.convertStrToBool(value);
                }
            }
            else if (name == "splitmode") {
                value = value.ToLower();
                if (value == "absolutesize") {
                    SplitMode = FCSizeType.AbsoluteSize;
                }
                else {
                    SplitMode = FCSizeType.PercentSize;
                }
            }
            else if (name.IndexOf("splitter-") != -1) {
                if (m_splitter != null) {
                    m_splitter.setProperty(name.Substring(9), value);
                }
            }
            else if (name == "splitterposition") {
                if (m_splitter != null) {
                    String[] strs = value.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (strs.Length == 4) {
                        m_splitter.Bounds = FCStr.convertStrToRect(value);
                    } else if (strs.Length <= 2) {
                        int pos = FCStr.convertStrToInt(strs[0]);
                        int lWidth = 0;
                        if (strs.Length == 2) {
                            lWidth = FCStr.convertStrToInt(strs[1]);
                        }
                        int width = Width, height = Height;
                        if (m_layoutStyle == FCLayoutStyle.TopToBottom || m_layoutStyle == FCLayoutStyle.BottomToTop) {
                            m_splitter.Bounds = new FCRect(0, pos, width, pos + lWidth);
                        } else {
                            m_splitter.Bounds = new FCRect(pos, 0, pos + lWidth, height);
                        }
                    }
                }
            }
            else if (name == "splittervisible") {
                if (m_splitter != null) {
                    m_splitter.Visible = FCStr.convertStrToBool(value);
                }
            }
            else {
                base.setProperty(name, value);
            }
        }

        /// <summary>
        /// 拖动滚动条
        /// </summary>
        /// <param name="sender">控件</param>
        public void SplitterDragging(object sender) {
            m_splitPercent = -1;
            update();
            invalidate();
        }

        /// <summary>
        /// 布局更新方法
        /// </summary>
        public override void update() {
            onResetLayout();
            int controlsSize = m_controls.size();
            for (int i = 0; i < controlsSize; i++) {
                m_controls.get(i).update();
            }
        }
    }
}
