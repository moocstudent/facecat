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
    /// 页的布局位置
    /// </summary>
    public enum FCTabPageLayout {
        /// <summary>
        /// 下方
        /// </summary>
        Bottom,
        /// <summary>
        /// 左侧
        /// </summary>
        Left,
        /// <summary>
        /// 右侧
        /// </summary>
        Right,
        /// <summary>
        /// 顶部
        /// </summary>
        Top
    }

    /// <summary>
    /// 多页夹控件
    /// </summary>
    public class FCTabControl : FCDiv {
        /// <summary>
        /// 创建多页夹
        /// </summary>
        public FCTabControl() {
        }

        /// <summary>
        /// 动画状态
        /// </summary>
        private int m_animationState;

        /// <summary>
        /// 所有页
        /// </summary>
        public ArrayList<FCTabPage> m_tabPages = new ArrayList<FCTabPage>();

        /// <summary>
        /// 秒表ID
        /// </summary>
        private int m_timerID = getNewTimerID();

        protected FCTabPageLayout m_layout = FCTabPageLayout.Top;

        /// <summary>
        /// 获取或设置页的布局位置
        /// </summary>
        public virtual FCTabPageLayout Layout {
            get { return m_layout; }
            set { m_layout = value; }
        }

        protected int m_selectedIndex = -1;

        /// <summary>
        /// 获取或设置选中的索引
        /// </summary>
        public virtual int SelectedIndex {
            get { return m_selectedIndex; }
            set {
                int tabPageSize = m_tabPages.size();
                if (tabPageSize > 0) {
                    if (value >= 0 && value < tabPageSize) {
                        m_selectedIndex = value;
                        SelectedTabPage = m_tabPages.get(value);
                    }
                }
            }
        }

        /// <summary>
        /// 获取选中的页
        /// </summary>
        public virtual FCTabPage SelectedTabPage {
            get {
                int tabPageSize = m_tabPages.size();
                if (tabPageSize > 0) {
                    if (m_selectedIndex >= 0 && m_selectedIndex < tabPageSize) {
                        return m_tabPages.get(m_selectedIndex);
                    }
                }
                return null;
            }
            set {
                int index = -1;
                int tabPageSize = m_tabPages.size();
                if (value != null && tabPageSize > 0) {
                    FCTabPage oldSelectedTabPage = SelectedTabPage;
                    for (int i = 0; i < tabPageSize; i++) {
                        FCTabPage tabPage = m_tabPages.get(i);
                        if (tabPage == value) {
                            index = i;
                            tabPage.Visible = true;
                        }
                        else {
                            tabPage.Visible = false;
                        }
                    }
                    if (index != -1) {
                        m_selectedIndex = index;
                    }
                    else {
                        if (tabPageSize > 0) {
                            m_selectedIndex = 0;
                        }
                    }
                    FCTabPage newSelectedTabPage = SelectedTabPage;
                    if (oldSelectedTabPage != newSelectedTabPage) {
                        onSelectedTabPageChanged();
                    }
                    newSelectedTabPage.bringToFront();
                    newSelectedTabPage.HeaderButton.bringToFront();
                }
                else {
                    m_selectedIndex = -1;
                }
            }
        }

        protected bool m_useAnimation;

        /// <summary>
        /// 获取或设置是否使用动画
        /// </summary>
        public virtual bool UseAnimation {
            get { return m_useAnimation; }
            set {
                m_useAnimation = value;
                if (m_useAnimation) {
                    startTimer(m_timerID, 20);
                }
                else {
                    stopTimer(m_timerID);
                }
            }
        }

        /// <summary>
        /// 添加控件方法
        /// </summary>
        /// <param name="control">控件</param>
        public override void addControl(FCView control) {
            FCTabPage tabPage = control as FCTabPage;
            if (tabPage != null) {
                tabPage.TabControl = this;
            }
            base.addControl(control);
            if (tabPage != null) {
                m_tabPages.add(tabPage);
                SelectedTabPage = tabPage;
            }
        }

        /// <summary>
        /// 清除控件
        /// </summary>
        public override void clearControls() {
            m_tabPages.clear();
            m_selectedIndex = -1;
            base.clearControls();
        }

        /// <summary>
        /// 销毁方法
        /// </summary>
        public override void delete() {
            if (!IsDeleted) {
                stopTimer(m_timerID);
            }
            base.delete();
        }

        /// <summary>
        /// 绘制移动
        /// </summary>
        protected void drawMoving() {
            if (m_animationState > 0) {
                bool drawing = false;
                int tabPageSize = m_tabPages.size();
                FCTabPage selectedTabPage = SelectedTabPage;
                for (int i = 0; i < tabPageSize; i++) {
                    FCTabPage tabPage = m_tabPages.get(i);
                    if (tabPage == selectedTabPage && m_animationState == 1) {
                        continue;
                    }
                    FCButton headerButton = tabPage.HeaderButton;
                    if (headerButton.Visible) {
                        int moving = headerButton.Left;
                        int pos = tabPage.HeaderLocation.x;
                        if (m_layout == FCTabPageLayout.Left || m_layout == FCTabPageLayout.Right) {
                            pos = tabPage.HeaderLocation.y;
                            moving = headerButton.Top;
                        }
                        if (pos != moving) {
                            int relative = moving;
                            int sub = Math.Abs(pos - relative);
                            int step = 20;
                            if (m_useAnimation) {
                                if (tabPage == selectedTabPage) {
                                    if (sub > 200) {
                                        step = sub / 200 * 100;
                                    }
                                }
                                else {
                                    step = sub;
                                }
                            }
                            else {
                                step = sub;
                            }
                            if (relative != pos) {
                                if (pos > relative + step) relative += step;
                                else if (pos < relative - step) relative -= step;
                                else relative = pos;
                                if (m_layout == FCTabPageLayout.Left || m_layout == FCTabPageLayout.Right) {
                                    headerButton.Top = relative;
                                }
                                else {
                                    headerButton.Left = relative;
                                }
                                drawing = true;
                            }
                        }
                    }
                }
                //绘制
                if (!drawing) {
                    if (m_animationState == 2) {
                        m_animationState = 0;
                    }
                }
                update();
                invalidate();
            }
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <returns>控件类型</returns>
        public override String getControlType() {
            return "TabControl";
        }

        /// <summary>
        /// 获取事件名称列表
        /// </summary>
        /// <returns>名称列表</returns>
        public override ArrayList<String> getEventNames() {
            ArrayList<String> eventNames = base.getEventNames();
            eventNames.AddRange(new String[] { "SelectedTabPageChanged" });
            return eventNames;
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">返回属性值</param>
        /// <param name="type">返回属性类型</param>
        public override void getProperty(String name, ref String value, ref String type) {
            if (name == "layout") {
                type = "enum:FCTabPageLayout";
                if (Layout == FCTabPageLayout.Left) {
                    value = "Left";
                }
                else if (Layout == FCTabPageLayout.Right) {
                    value = "Right";
                }
                else if (Layout == FCTabPageLayout.Bottom) {
                    value = "Bottom";
                }
                else {
                    value = "Top";
                }
            }
            else if (name == "selectedindex") {
                type = "int";
                value = FCStr.convertIntToStr(SelectedIndex);
            }
            else if (name == "useanimation") {
                type = "bool";
                value = FCStr.convertBoolToStr(UseAnimation);
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
            propertyNames.AddRange(new String[] { "Layout", "SelectedIndex", "UseAnimation" });
            return propertyNames;
        }

        /// <summary>
        /// 获取多页夹列表
        /// </summary>
        /// <returns>多页夹列表</returns>
        public ArrayList<FCTabPage> getTabPages() {
            return m_tabPages;
        }

        /// <summary>
        /// 插入控件
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="control">控件</param>
        public override void insertControl(int index, FCView control) {
            FCTabPage tabPage = control as FCTabPage;
            if (tabPage != null) {
                tabPage.TabControl = this;
            }
            base.addControl(control);
            m_tabPages.Insert(index, tabPage);
            SelectedTabPage = tabPage;
        }

        /// <summary>
        /// 开始拖动页头
        /// </summary>
        /// <param name="tabPage">页</param>
        public virtual void onDragTabHeaderBegin(FCTabPage tabPage) {
            m_animationState = 1;
            tabPage.HeaderButton.bringToFront();
        }

        /// <summary>
        /// 结束拖动页头
        /// </summary>
        /// <param name="tabPage">页</param>
        public virtual void onDragTabHeaderEnd(FCTabPage tabPage) {
            if (m_animationState == 1) {
                m_animationState = 2;
                drawMoving();
            }
        }

        /// <summary>
        /// 页头拖动中
        /// </summary>
        /// <param name="tabPage">页</param>
        public virtual void onDraggingTabHeader(FCTabPage tabPage) {
            FCButton headerButton = tabPage.HeaderButton;
            int moving = headerButton.Left;
            if (m_layout == FCTabPageLayout.Left || m_layout == FCTabPageLayout.Right) {
                moving = headerButton.Top;
            }
            int tabPageSize = m_tabPages.size();
            for (int i = 0; i < tabPageSize; i++) {
                FCTabPage page = m_tabPages.get(i);
                if (page != tabPage) {
                    FCButton tpHeader = page.HeaderButton;
                    if (tpHeader.Visible) {
                        int pos = page.HeaderLocation.x;
                        int size = tpHeader.Width;
                        int sSize = headerButton.Width;
                        if (m_layout == FCTabPageLayout.Left || m_layout == FCTabPageLayout.Right) {
                            pos = page.HeaderLocation.y;
                            size = tpHeader.Height;
                            sSize = headerButton.Height;
                        }
                        bool instead = false;
                        //向左交换
                        if (moving > pos) {
                            if (moving > pos
                                && moving < pos + size / 2) {
                                instead = true;
                            }
                        }
                        if (moving < pos) {
                            if (moving + sSize > pos + size / 2
                               && moving + sSize < pos + size) {
                                instead = true;
                            }
                        }
                        //向右交换
                        if (instead) {
                            FCPoint sLocation = tabPage.HeaderLocation;
                            if (m_layout == FCTabPageLayout.Left || m_layout == FCTabPageLayout.Right) {
                                tabPage.HeaderLocation = new FCPoint(tabPage.HeaderLocation.x, pos);
                                page.HeaderLocation = new FCPoint(page.HeaderLocation.x, sLocation.y);
                            }
                            else {
                                tabPage.HeaderLocation = new FCPoint(pos, tabPage.HeaderLocation.y);
                                page.HeaderLocation = new FCPoint(sLocation.x, page.HeaderLocation.y);
                            }
                            int oIndex = m_tabPages.IndexOf(tabPage);
                            int nIndex = m_tabPages.IndexOf(page);
                            m_tabPages.set(oIndex, page);
                            m_tabPages.set(nIndex, tabPage);
                            m_selectedIndex = nIndex;
                            break;
                        }
                    }
                }
            }
            drawMoving();
        }

        /// <summary>
        /// 添加控件方法
        /// </summary>
        public override void onLoad() {
            base.onLoad();
            if (m_useAnimation) {
                startTimer(m_timerID, 20);
            }
            else {
                stopTimer(m_timerID);
            }
        }

        /// <summary>
        /// 页改变方法
        /// </summary>
        public virtual void onSelectedTabPageChanged() {
            callEvents(FCEventID.SELECTEDTABPAGECHANGED);
        }

        /// <summary>
        /// 秒表方法
        /// </summary>
        /// <param name="timerID">秒表ID</param>
        public override void onTimer(int timerID) {
            base.onTimer(timerID);
            if (m_timerID == timerID) {
                drawMoving();
            }
        }

        /// <summary>
        /// 移除控件
        /// </summary>
        /// <param name="control">控件</param>
        public override void removeControl(FCView control) {
            FCTabPage tabPage = control as FCTabPage;
            if (tabPage != null) {
                int tabPageSize = m_tabPages.size();
                if (tabPageSize > 0) {
                    FCTabPage selectedPage = SelectedTabPage;
                    if (selectedPage == tabPage) {
                        if (m_selectedIndex > 0) {
                            if (m_selectedIndex < tabPageSize - 1) {
                                selectedPage = m_tabPages.get(m_selectedIndex + 1);
                            }
                            else {
                                selectedPage = m_tabPages.get(m_selectedIndex - 1);
                            }
                        }
                        else {
                            if (tabPageSize > 1) {
                                selectedPage = m_tabPages.get(m_selectedIndex + 1);
                            }
                        }
                    }
                    m_tabPages.remove(tabPage);
                    base.removeControl(tabPage.HeaderButton);
                    base.removeControl(tabPage);
                    tabPage.TabControl = null;
                    SelectedTabPage = selectedPage;
                }
            }
            else {
                base.removeControl(control);
            }
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        public override void setProperty(String name, String value) {
            if (name == "layout") {
                value = value.ToLower();
                if (value == "left") {
                    Layout = FCTabPageLayout.Left;
                }
                else if (value == "top") {
                    Layout = FCTabPageLayout.Top;
                }
                else if (value == "right") {
                    Layout = FCTabPageLayout.Right;
                }
                else if (value == "bottom") {
                    Layout = FCTabPageLayout.Bottom;
                }
            }
            else if (name == "selectedindex") {
                SelectedIndex = FCStr.convertStrToInt(value);
            }
            else if (name == "useanimation") {
                UseAnimation = FCStr.convertStrToBool(value);
            }
            else {
                base.setProperty(name, value);
            }
        }

        /// <summary>
        /// 多页夹的样式
        /// </summary>
        /// <param name="layoutStyle"></param>
        /// <param name="bounds"></param>
        /// <param name="padding"></param>
        /// <param name="margin"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="tw"></param>
        /// <param name="th"></param>
        /// <param name="headerLocation"></param>
        public virtual void tabStyle(int layoutStyle, ref FCRect bounds, ref FCPadding padding,
        ref FCPadding margin, int left, int top, int width, int height,
         int tw, int th, ref FCPoint headerLocation) {
            switch (layoutStyle) {
                case 0:
                    bounds.left = padding.left;
                    bounds.top = padding.top;
                    bounds.right = width;
                    bounds.bottom = height - th;
                    headerLocation.x = left;
                    headerLocation.y = height - th;
                    break;
                case 1:
                    bounds.left = tw;
                    bounds.top = padding.top;
                    bounds.right = width;
                    bounds.bottom = height;
                    headerLocation.x = padding.left;
                    headerLocation.y = top;
                    break;
                case 2:
                    bounds.left = padding.left;
                    bounds.top = padding.top;
                    bounds.right = width - tw;
                    bounds.bottom = height;
                    headerLocation.x = width - tw;
                    headerLocation.y = top;
                    break;
                case 3:
                    bounds.left = padding.left;
                    bounds.top = th;
                    bounds.right = width;
                    bounds.bottom = height;
                    headerLocation.x = left;
                    headerLocation.y = padding.top;
                    break;
            }
        }

        /// <summary>
        /// 重新布局
        /// </summary>
        public override void update() {
            if (Native == null) {
                return;
            }
            base.update();
            FCPadding padding = Padding;
            int left = padding.left, top = padding.top;
            int width = Width - padding.left - padding.right;
            int height = Height - padding.top - padding.bottom;
            int tabPageSize = m_tabPages.size();
            for (int i = 0; i < tabPageSize; i++) {
                FCTabPage tabPage = m_tabPages.get(i);
                FCButton headerButton = tabPage.HeaderButton;
                if (headerButton.Visible) {
                    FCPadding margin = headerButton.Margin;
                    int tw = headerButton.Width + margin.left + margin.right;
                    int th = headerButton.Height + margin.top + margin.bottom;
                    FCRect bounds = new FCRect();
                    FCPoint headerLocation = new FCPoint();
                    int layout = 0;
                    switch (m_layout) {
                        case FCTabPageLayout.Bottom:
                            layout = 0;
                            break;
                        case FCTabPageLayout.Left:
                            layout = 1;
                            break;
                        case FCTabPageLayout.Right:
                            layout = 2;
                            break;
                        case FCTabPageLayout.Top:
                            layout = 3;
                            break;
                    }
                    tabStyle(layout, ref bounds, ref padding,
                    ref margin, left, top, width, height,
                     tw, th, ref headerLocation);
                    tabPage.Bounds = bounds;
                    tabPage.HeaderLocation = headerLocation;
                    if (!m_useAnimation) {
                        tabPage.HeaderButton.Location = headerLocation;
                    }
                    if (m_animationState > 0) {
                        if (m_layout == FCTabPageLayout.Left || m_layout == FCTabPageLayout.Right) {
                            headerLocation.y = headerButton.Top;
                        }
                        else if (m_layout == FCTabPageLayout.Bottom || m_layout == FCTabPageLayout.Top) {
                            headerLocation.x = headerButton.Left;
                        }
                    }
                    headerButton.Location = headerLocation;
                    left += tw;
                    top += th;
                }
                else {
                    tabPage.Visible = false;
                }
            }
        }
    }
}
