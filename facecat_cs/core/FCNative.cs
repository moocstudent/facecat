/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-捂脸鹿创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FaceCat {
    /// <summary>
    /// 公共类库
    /// </summary>
    public class FCNative {
        /// <summary>
        /// 创建公共类库
        /// </summary>
        public FCNative() {
            m_host.Native = this;
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~FCNative() {
            delete();
        }

        /// <summary>
        /// 控件
        /// </summary>
        private ArrayList<FCView> m_controls = new ArrayList<FCView>();

        /// <summary>
        /// 拖动开始时的触摸位置
        /// </summary>
        private FCPoint m_dragBeginPoint;

        /// <summary>
        /// 拖动开始时的区域
        /// </summary>
        private FCRect m_dragBeginRect;

        /// <summary>
        /// 可以开始拖动的偏移坐标量
        /// </summary>
        private FCPoint m_dragStartOffset;

        /// <summary>
        /// 正被拖动的控件
        /// </summary>
        private FCView m_draggingControl;

        /// <summary>
        /// 即将导出的控件
        /// </summary>
        private FCView m_exportingControl;

        /// <summary>
        /// 秒表集合
        /// </summary>
        private HashMap<int, FCView> m_timers = new HashMap<int, FCView>();

        /// <summary>
        /// 正被触摸按下的控件
        /// </summary>
        private FCView m_touchDownControl;

        /// <summary>
        /// 触摸按下时的坐标
        /// </summary>
        private FCPoint m_touchDownPoint;

        /// <summary>
        /// 触摸正在其上方移动的控件
        /// </summary>
        private FCView m_touchMoveControl;

        private bool m_allowScaleSize = false;

        /// <summary>
        /// 获取或设置是否允许使用缩放尺寸
        /// </summary>
        public bool AllowScaleSize {
            get { return m_allowScaleSize; }
            set { m_allowScaleSize = value; }
        }

        /// <summary>
        /// 获取或设置触摸形状
        /// </summary>
        public FCCursors Cursor {
            get {
                if (m_host != null) {
                    return m_host.getCursor();
                }
                return FCCursors.Arrow;
            }
            set { m_host.setCursor(value); }
        }

        private FCSize m_displaySize;

        /// <summary>
        /// 获取或设置显示区域
        /// </summary>
        public FCSize DisplaySize {
            get { return m_displaySize; }
            set { m_displaySize = value; }
        }

        private FCView m_focusedControl;

        /// <summary>
        /// 获取或设置选中的控件
        /// </summary>
        public FCView FocusedControl {
            get { return m_focusedControl; }
            set {
                if (m_focusedControl != value) {
                    if (m_focusedControl != null) {
                        FCView fControl = m_focusedControl;
                        m_focusedControl = null;
                        fControl.onLostFocus();
                    }
                    m_focusedControl = value;
                    if (m_focusedControl != null) {
                        m_focusedControl.onGotFocus();
                    }
                }
            }
        }

        private FCHost m_host = new WinHost();

        /// <summary>
        /// 获取或设置控件管理器
        /// </summary>
        public FCHost Host {
            get { return m_host; }
            set { m_host = value; }
        }

        /// <summary>
        /// 获取触摸悬停的控件
        /// </summary>
        public FCView HoveredControl {
            get { return m_touchMoveControl; }
        }

        private bool m_isDeleted;

        /// <summary>
        /// 获取或设置是否已被销毁
        /// </summary>
        public bool IsDeleted {
            get { return m_isDeleted; }
        }

        private FCMirrorMode m_mirrorMode = FCMirrorMode.None;

        /// <summary>
        /// 获取或设置镜像模式
        /// </summary>
        public FCMirrorMode MirrorMode {
            get { return m_mirrorMode; }
            set { m_mirrorMode = value; }
        }

        private ArrayList<FCNative> m_mirrors = new ArrayList<FCNative>();

        /// <summary>
        /// 获取或设置镜像集合
        /// </summary>
        public ArrayList<FCNative> Mirrors {
            get { return m_mirrors; }
            set { m_mirrors = value; }
        }

        private FCNative m_mirrorHost;

        /// <summary>
        /// 获取宿主库
        /// </summary>
        public FCNative MirrorHost {
            get { return m_mirrorHost; }
        }

        private float m_opacity = 1;

        /// <summary>
        /// 获取或设置透明度
        /// </summary>
        public float Opacity {
            get { return m_opacity; }
            set { m_opacity = value; }
        }

        private FCPaint m_paint = new GdiPlusPaint();

        /// <summary>
        /// 获取绘图类
        /// </summary>
        public FCPaint Paint {
            get { return m_paint; }
            set { m_paint = value; }
        }

        /// <summary>
        /// 获取触摸按住的控件
        /// </summary>
        public FCView PushedControl {
            get { return m_touchDownControl; }
        }

        private String m_resourcePath;

        /// <summary>
        /// 获取或设置资源文件的路径
        /// </summary>
        public String ResourcePath {
            get { return m_resourcePath; }
            set { m_resourcePath = value; }
        }

        private int m_rotateAngle;

        /// <summary>
        /// 获取或设置旋转角度
        /// </summary>
        public int RotateAngle {
            get { return m_rotateAngle; }
            set { m_rotateAngle = value; }
        }

        private FCSize m_scaleSize;

        /// <summary>
        /// 获取或设置使用缩放尺寸
        /// </summary>
        public FCSize ScaleSize {
            get { return m_scaleSize; }
            set { m_scaleSize = value; }
        }

        /// <summary>
        /// 获取触摸的实际位置
        /// </summary>
        public FCPoint TouchPoint {
            get {
                if (m_host != null) {
                    return m_host.getTouchPoint();
                }
                return new FCPoint(0, 0);
            }
        }

        /// <summary>
        /// 添加控件
        /// </summary>
        /// <param name="control">控件</param>
        public void addControl(FCView control) {
            control.Native = this;
            m_controls.add(control);
            control.onAdd();
        }

        /// <summary>
        /// 添加镜像
        /// </summary>
        /// <param name="mirrorHost">宿主</param>
        /// <param name="control">控件</param>
        public void addMirror(FCNative mirrorHost, FCView control) {
            m_mirrorHost = mirrorHost;
            m_mirrorHost.Mirrors.add(this);
            control.Native = this;
            m_controls.add(control);
        }

        /// <summary>
        /// 将控件放到最前显示
        /// </summary>
        /// <param name="control">控件</param>
        public void bringToFront(FCView control) {
            FCView parent = control.Parent;
            if (parent != null) {
                parent.bringChildToFront(control);
            }
            else {
                if (m_controls != null && m_controls.size() > 0) {
                    m_controls.remove(control);
                    m_controls.add(control);
                }
            }
        }

        /// <summary>
        /// 退出拖动
        /// </summary>
        public void cancelDragging() {
            if (m_draggingControl != null) {
                m_draggingControl.Bounds = m_dragBeginRect;
                FCView draggingControl = m_draggingControl;
                m_draggingControl = null;
                draggingControl.onDragEnd();
                FCView parent = draggingControl.Parent;
                if (parent != null) {
                    parent.invalidate();
                }
                else {
                    invalidate();
                }
            }
        }

        /// <summary>
        /// 清除所有的控件
        /// </summary>
        public void clearControls() {
            ArrayList<FCView> controls = new ArrayList<FCView>();
            foreach (FCView control in m_controls) {
                controls.add(control);
            }
            foreach (FCView control in controls) {
                control.onRemove();
                control.delete();
            }
            m_controls.clear();
        }

        /// <summary>
        /// 获取控件的绝对横坐标
        /// </summary>
        /// <param name="control">控件</param>
        /// <returns>坐标</returns>
        public int clientX(FCView control) {
            if (control != null) {
                FCView parent = control.Parent;
                int cLeft = control.Left;
                if (control == m_exportingControl) {
                    cLeft = 0;
                }
                if (parent != null) {
                    if (m_mirrorMode != FCMirrorMode.None) {
                        int controlsSize = m_controls.size();
                        for (int i = 0; i < controlsSize; i++) {
                            if (m_controls.get(i) == control) {
                                return cLeft;
                            }
                        }
                    }
                    return cLeft - (control.DisplayOffset ? parent.getDisplayOffset().x : 0) + clientX(parent);
                }
                else {
                    return cLeft;
                }
            }
            else {
                return 0;
            }
        }

        /// <summary>
        /// 获取控件的绝对纵坐标
        /// </summary>
        /// <param name="control">控件</param>
        /// <returns>坐标</returns>
        public int clientY(FCView control) {
            if (control != null) {
                FCView parent = control.Parent;
                int cTop = control.Top;
                if (control == m_exportingControl) {
                    cTop = 0;
                }
                if (parent != null) {
                    if (m_mirrorMode != FCMirrorMode.None) {
                        int controlsSize = m_controls.size();
                        for (int i = 0; i < controlsSize; i++) {
                            if (m_controls.get(i) == control) {
                                return cTop;
                            }
                        }
                    }
                    return cTop - (control.DisplayOffset ? parent.getDisplayOffset().y : 0) + clientY(parent);
                }
                else {
                    return cTop;
                }
            }
            else {
                return 0;
            }
        }

        /// <summary>
        /// 是否包含控件
        /// </summary>
        /// <param name="control">控件</param>
        /// <returns>是否包含</returns>
        public bool containsControl(FCView control) {
            foreach (FCView subControl in m_controls) {
                if (subControl == control) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 键盘抬起事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="e">参数</param>
        private void container_KeyUp(object sender, KeyEventArgs e) {
            onKeyUp((char)e.KeyData);
        }

        /// <summary>
        /// 销毁资源
        /// </summary>
        public virtual void delete() {
            if (!m_isDeleted) {
                m_mirrors.clear();
                m_mirrorHost = null;
                m_timers.clear();
                clearControls();
                if (m_host != null) {
                    m_host.delete();
                }
                if (m_paint != null) {
                    m_paint.delete();
                }
                m_isDeleted = true;
            }
        }

        /// <summary>
        /// 导出到图片
        /// </summary>
        /// <param name="exportPath">导出路径</param>
        public void exportToImage(String exportPath) {
            exportToImage(exportPath, null);
        }

        /// <summary>
        /// 导出控件到图片
        /// </summary>
        /// <param name="exportPath">导出路径</param>
        /// <param name="control">控件</param>
        public void exportToImage(String exportPath, FCView control) {
            m_exportingControl = control;
            if (control != null) {
                ArrayList<FCView> controls = new ArrayList<FCView>();
                controls.add(control);
                FCRect rect = new FCRect(0, 0, control.Width, control.Height);
                m_paint.beginExport(exportPath, rect);
                renderControls(rect, controls, getPaintingResourcePath(control), GetPaintingOpacity(control));
                m_paint.endExport();
            }
            else {
                FCRect rect = new FCRect(0, 0, m_displaySize.cx, m_displaySize.cy);
                m_paint.beginExport(exportPath, rect);
                ArrayList<FCView> subControls = new ArrayList<FCView>();
                getSortedControls(null, subControls);
                renderControls(rect, subControls, m_resourcePath, m_opacity);
                subControls.clear();
                m_paint.endExport();
            }
            m_exportingControl = null;
        }

        /// <summary>
        /// 根据触摸位置获取控件
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <param name="controls">控件集合</param>
        /// <returns>控件对象</returns>
        private FCView findControl(FCPoint mp, ArrayList<FCView> controls) {
            int size = controls.size();
            for (int i = size - 1; i >= 0; i--) {
                FCView control = controls.get(i);
                if (control.Visible) {
                    if (control.containsPoint(mp)) {
                        ArrayList<FCView> subControls = new ArrayList<FCView>();
                        getSortedControls(control, subControls);
                        FCView subControl = findControl(mp, subControls);
                        subControls.clear();
                        if (subControl != null) {
                            return subControl;
                        }
                        return control;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 根据名称查找控件
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="controls">控件集合</param>
        /// <returns>控件</returns>
        private FCView findControl(String name, ArrayList<FCView> controls) {
            int controlSize = controls.size();
            for (int i = 0; i < controlSize; i++) {
                FCView control = controls.get(i);
                if (control.Name == name) {
                    return control;
                }
                else {
                    ArrayList<FCView> subControls = control.getControls();
                    if (subControls != null && subControls.size() > 0) {
                        FCView fControl = findControl(name, subControls);
                        if (fControl != null) {
                            return fControl;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 根据坐标查找控件
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>控件</returns>
        public FCView findControl(FCPoint mp) {
            ArrayList<FCView> subControls = new ArrayList<FCView>();
            getSortedControls(null, subControls);
            FCView control = findControl(mp, subControls);
            subControls.clear();
            return control;
        }

        /// <summary>
        /// 根据坐标在控件中查找控件
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <param name="parent">父控件</param>
        /// <returns>控件</returns>
        public FCView findControl(FCPoint mp, FCView parent) {
            ArrayList<FCView> subControls = new ArrayList<FCView>();
            getSortedControls(parent, subControls);
            FCView control = findControl(mp, subControls);
            subControls.clear();
            return control;
        }

        /// <summary>
        /// 根据名称查找控件
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>控件</returns>
        public FCView findControl(String name) {
            return findControl(name, m_controls);
        }

        /// <summary>
        /// 根据控件查找预处理事件的控件
        /// </summary>
        /// <param name="control">控件</param>
        /// <returns>控件</returns>
        private FCView findPreviewsControl(FCView control) {
            if (control.AllowPreviewsEvent) {
                return control;
            }
            else {
                FCView parent = control.Parent;
                if (parent != null) {
                    return findPreviewsControl(parent);
                }
                else {
                    return control;
                }
            }
        }

        /// <summary>
        /// 根据控件查找窗体
        /// </summary>
        /// <param name="control">控件</param>
        /// <returns>窗体</returns>
        private FCView findWindow(FCView control) {
            if (control.IsWindow) {
                return control;
            }
            else {
                FCView parent = control.Parent;
                if (parent != null) {
                    return findWindow(parent);
                }
                else {
                    return control;
                }
            }
        }

        /// <summary>
        /// 获取控件集合的拷贝
        /// </summary>
        /// <returns>控件集合</returns>
        public ArrayList<FCView> getControls() {
            return m_controls;
        }

        /// <summary>
        /// 获取焦点控件
        /// </summary>
        /// <param name="controls">控件集合</param>
        /// <returns>焦点控件</returns>
        private FCView getFocusedControl(ArrayList<FCView> controls) {
            int controlSize = controls.size();
            for (int i = 0; i < controlSize; i++) {
                FCView control = controls.get(i);
                if (control.Focused) {
                    return control;
                }
                else {
                    //查找子控件
                    ArrayList<FCView> subControls = control.getControls();
                    if (subControls != null && subControls.size() > 0) {
                        FCView focusedControl = getFocusedControl(subControls);
                        if (focusedControl != null) {
                            return focusedControl;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 获取绘制的透明度
        /// </summary>
        /// <param name="control">控件</param>
        /// <returns>透明度</returns>
        private float GetPaintingOpacity(FCView control) {
            float opacity = control.Opacity;
            FCView parent = control.Parent;
            if (parent != null) {
                opacity *= GetPaintingOpacity(parent);
            }
            else {
                opacity *= m_opacity;
            }
            return opacity;
        }

        /// <summary>
        /// 获取绘制的资源路径
        /// </summary>
        /// <param name="control">控件</param>
        /// <returns>路径</returns>
        private String getPaintingResourcePath(FCView control) {
            String resourcePath = control.ResourcePath;
            if (resourcePath != null && resourcePath.Length > 0) {
                return resourcePath;
            }
            else {
                FCView parent = control.Parent;
                if (parent != null) {
                    return getPaintingResourcePath(parent);
                }
                else {
                    return m_resourcePath;
                }
            }
        }

        /// <summary>
        /// 获取排序后的控件集合
        /// </summary>
        /// <param name="parent">父控件</param>
        /// <param name="sortedControls">排序后的控件</param>
        /// <returns>状态</returns>
        private bool getSortedControls(FCView parent, ArrayList<FCView> sortedControls) {
            ArrayList<FCView> controls = null;
            if (parent != null) {
                controls = parent.getControls();
            }
            else {
                controls = m_controls;
            }
            int controlSize = controls.size();
            int index = 0;
            for (int i = 0; i < controlSize; i++) {
                FCView control = controls.get(i);
                if (control.Visible) {
                    if (control.TopMost) {
                        sortedControls.add(control);
                    }
                    else {
                        sortedControls.Insert(index, control);
                        index++;
                    }
                }
            }
            return sortedControls.size() > 0;
        }

        /// <summary>
        /// 获取TabStop属性为TRUE的控件
        /// </summary>
        /// <param name="control"></param>
        private void getTabStopControls(FCView control, ArrayList<FCView> tabStopControls) {
            ArrayList<FCView> controls = control.getControls();
            int controlSize = controls.size();
            for (int i = 0; i < controlSize; i++) {
                FCView subControl = controls.get(i);
                if (!subControl.IsWindow) {
                    if (subControl.Enabled && subControl.TabStop) {
                        tabStopControls.add(subControl);
                    }
                    getTabStopControls(subControl, tabStopControls);
                }
            }
        }

        /// <summary>
        /// 判断是否绘制可用状态
        /// </summary>
        /// <param name="control">控件</param>
        /// <returns>是否绘制可用状态</returns>
        private bool isPaintEnabled(FCView control) {
            if (control.Enabled) {
                FCView parent = control.Parent;
                if (parent != null) {
                    return isPaintEnabled(parent);
                }
                else {
                    return true;
                }
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// 插入控件
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="control">控件</param>
        public void insertControl(int index, FCView control) {
            m_controls.Insert(index, control);
        }

        /// <summary>
        /// 使用缓存绘制图象，不重新计算绘图结构
        /// </summary>
        public virtual void invalidate() {
            if (m_host != null) {
                m_host.invalidate();
            }
        }

        /// <summary>
        /// 局部绘图
        /// </summary>
        /// <param name="control">控件</param>
        public void invalidate(FCView control) {
            if (m_host != null) {
                int clX = clientX(control);
                int clY = clientY(control);
                m_host.invalidate(new FCRect(clX, clY, clX + control.Width, clY + control.Height));
                ArrayList<FCNative> mirrors = null;
                if (m_mirrorMode == FCMirrorMode.Shadow) {
                    clX = m_mirrorHost.clientX(control);
                    clY = m_mirrorHost.clientY(control);
                    m_mirrorHost.Host.invalidate(new FCRect(clX, clY, clX + control.Width, clY + control.Height));
                    mirrors = m_mirrorHost.Mirrors;
                }
                else {
                    mirrors = m_mirrors;
                }
                int mirrorsSize = mirrors.size();
                for (int i = 0; i < mirrorsSize; i++) {
                    if (mirrors.get(i) != this && mirrors.get(i).MirrorMode != FCMirrorMode.BugHole) {
                        clX = mirrors.get(i).clientX(control);
                        clY = mirrors.get(i).clientY(control);
                        mirrors.get(i).Host.invalidate(new FCRect(clX, clY, clX + control.Width, clY + control.Height));
                    }
                }
            }
        }

        /// <summary>
        /// 局部绘图
        /// </summary>
        /// <param name="rect"> 区域</param>
        public void invalidate(FCRect rect) {
            if (m_host != null) {
                m_host.invalidate(rect);
            }
        }

        /// <summary>
        /// 文字输入方法
        /// </summary>
        /// <param name="key">按键</param>
        /// <returns>是否处理</returns>
        public bool onChar(char key) {
            FCView focusedControl = FocusedControl;
            if (focusedControl != null && isPaintEnabled(focusedControl)) {
                if (focusedControl.TabStop) {
                    FCView window = findWindow(focusedControl);
                    if (window != null) {
                        if (!(m_host.isKeyPress(0x10) || m_host.isKeyPress(0x11)) && key == 9) {
                            ArrayList<FCView> tabStopControls = new ArrayList<FCView>();
                            getTabStopControls(window, tabStopControls);
                            int size = tabStopControls.size();
                            if (size > 0) {
                                //排序
                                for (int i = 0; i < size - 1; i++) {
                                    for (int j = 0; j < size - 1 - i; j++) {
                                        FCView controlLeft = tabStopControls.get(j);
                                        FCView controlRight = tabStopControls.get(j + 1);
                                        if (controlLeft.TabIndex > controlRight.TabIndex) {
                                            FCView temp = tabStopControls.get(j + 1);
                                            tabStopControls.set(j + 1, tabStopControls.get(j));
                                            tabStopControls.set(j, temp);
                                        }
                                    }
                                }
                                //找到下一个控件
                                bool change = false;
                                FCView newFocusedControl = null;
                                for (int i = 0; i < size; i++) {
                                    FCView control = tabStopControls.get(i);
                                    if (focusedControl == control) {
                                        if (i < size - 1) {
                                            newFocusedControl = tabStopControls.get(i + 1);
                                        }
                                        else {
                                            newFocusedControl = tabStopControls.get(0);
                                        }
                                        change = true;
                                        break;
                                    }
                                }
                                if (!change) {
                                    newFocusedControl = tabStopControls.get(0);
                                }
                                //转移焦点
                                if (newFocusedControl != focusedControl) {
                                    newFocusedControl.Focused = true;
                                    focusedControl = newFocusedControl;
                                    focusedControl.onTabStop();
                                    window.invalidate();
                                    return true;
                                }
                            }
                        }
                    }
                }
                focusedControl.onChar(key);
            }
            return false;
        }

        /// <summary>
        /// 处理双击
        /// </summary>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚轮值</param>
        public void onDoubleClick(FCTouchInfo touchInfo) {
            FCView focusedControl = FocusedControl;
            if (focusedControl != null && isPaintEnabled(focusedControl)) {
                FCPoint mp = TouchPoint;
                int clx = clientX(focusedControl);
                int cly = clientY(focusedControl);
                FCPoint cmp = new FCPoint(mp.x - clx, mp.y - cly);
                FCTouchInfo newTouchInfo = touchInfo.clone();
                newTouchInfo.m_firstPoint = cmp;
                newTouchInfo.m_secondPoint = cmp;
                focusedControl.onDoubleClick(newTouchInfo);
            }
        }

        /// <summary>
        /// 处理键盘按下
        /// </summary>
        /// <param name="key">按键</param>
        public void onKeyDown(char key) {
            FCView focusedControl = FocusedControl;
            if (focusedControl != null && isPaintEnabled(focusedControl)) {
                focusedControl.onKeyDown(key);
            }
        }

        /// <summary>
        /// 处理键盘抬起
        /// </summary>
        /// <param name="key">按键</param>
        public void onKeyUp(char key) {
            FCView focusedControl = FocusedControl;
            if (focusedControl != null && isPaintEnabled(focusedControl)) {
                focusedControl.onKeyUp(key);
            }
        }

        /// <summary>
        /// 触摸单击事件
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public void onMouseDown(FCTouchInfo touchInfo) {
            m_draggingControl = null;
            m_touchDownControl = null;
            FCPoint mp = TouchPoint;
            m_touchDownPoint = mp;
            ArrayList<FCView> subControls = new ArrayList<FCView>();
            getSortedControls(null, subControls);
            FCView control = findControl(mp, subControls);
            subControls.clear();
            if (control != null) {
                //将窗体置顶
                FCView window = findWindow(control);
                if (window != null && window.IsWindow) {
                    window.bringToFront();
                }
                if (isPaintEnabled(control)) {
                    int clx = clientX(control);
                    int cly = clientY(control);
                    FCPoint cmp = new FCPoint(mp.x - clx, mp.y - cly);
                    FCView focusedControl = FocusedControl;
                    //触发事件
                    m_touchDownControl = control;
                    if (focusedControl == FocusedControl) {
                        if (control.CanFocus) {
                            if (touchInfo.m_firstTouch) {
                                //选中控件
                                FocusedControl = control;
                            }
                        }
                    }
                    if (onPreviewTouchEvent(FCEventID.TOUCHDOWN, m_touchDownControl, touchInfo)) {
                        return;
                    }
                    FCTouchInfo newTouchInfo = touchInfo.clone();
                    newTouchInfo.m_firstPoint = cmp;
                    newTouchInfo.m_secondPoint = cmp;
                    m_touchDownControl.onTouchDown(newTouchInfo);
                    if (m_touchDownControl != null) {
                        m_touchDownControl.onDragReady(ref m_dragStartOffset);
                    }
                }
            }
        }

        /// <summary>
        /// 触摸离开事件
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public void onMouseLeave(FCTouchInfo touchInfo) {
            //调用触摸离开事件
            if (m_touchMoveControl != null && isPaintEnabled(m_touchMoveControl)) {
                FCPoint mp = TouchPoint;
                FCPoint cmp = new FCPoint(mp.x - clientX(m_touchMoveControl), mp.y - clientY(m_touchMoveControl));
                FCView touchMoveControl = m_touchMoveControl;
                m_touchMoveControl = null;
                if (onPreviewTouchEvent(FCEventID.TOUCHLEAVE, m_touchDownControl, touchInfo)) {
                    return;
                }
                FCTouchInfo newTouchInfo = touchInfo.clone();
                newTouchInfo.m_firstPoint = cmp;
                newTouchInfo.m_secondPoint = cmp;
                touchMoveControl.onTouchLeave(newTouchInfo);
            }
        }

        /// <summary>
        /// 触摸移动事件
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public void onMouseMove(FCTouchInfo touchInfo) {
            FCPoint mp = TouchPoint;
            //按下控件时
            if (m_touchDownControl != null) {
                if (onPreviewTouchEvent(FCEventID.TOUCHMOVE, m_touchDownControl, touchInfo)) {
                    return;
                }
                FCPoint cmp = new FCPoint(mp.x - clientX(m_touchDownControl), mp.y - clientY(m_touchDownControl));
                FCTouchInfo newTouchInfo = touchInfo.clone();
                newTouchInfo.m_firstPoint = cmp;
                newTouchInfo.m_secondPoint = cmp;
                m_touchDownControl.onTouchMove(newTouchInfo);
                setCursor(m_touchDownControl);
                //拖动
                if (m_touchDownControl.AllowDrag && newTouchInfo.m_firstTouch && newTouchInfo.m_clicks == 1) {
                    if (Math.Abs(mp.x - m_touchDownPoint.x) > m_dragStartOffset.x
                        || Math.Abs(mp.y - m_touchDownPoint.y) > m_dragStartOffset.y) {
                        //触发事件
                        if (m_touchDownControl.onDragBegin()) {
                            m_dragBeginRect = m_touchDownControl.Bounds;
                            m_dragBeginPoint = m_touchDownPoint;
                            m_draggingControl = m_touchDownControl;
                            m_touchDownControl = null;
                            FCView parent = m_draggingControl.Parent;
                            if (parent != null) {
                                parent.invalidate();
                            }
                            else {
                                invalidate();
                            }
                        }
                    }
                }
            }
            //拖动时
            else if (m_draggingControl != null) {
                FCView draggingControl = m_draggingControl;
                int offsetX = mp.x - m_dragBeginPoint.x;
                int offsetY = mp.y - m_dragBeginPoint.y;
                FCRect newBounds = m_dragBeginRect;
                newBounds.left += offsetX;
                newBounds.top += offsetY;
                newBounds.right += offsetX;
                newBounds.bottom += offsetY;
                draggingControl.Bounds = newBounds;
                //触发正在拖动事件
                draggingControl.onDragging();
                FCView parent = draggingControl.Parent;
                if (parent != null) {
                    parent.invalidate();
                }
                else {
                    invalidate();
                }
            }
            else {
                ArrayList<FCView> subControls = new ArrayList<FCView>();
                getSortedControls(null, subControls);
                FCView control = findControl(mp, subControls);
                if (control != null) {
                    if (onPreviewTouchEvent(FCEventID.TOUCHMOVE, control, touchInfo)) {
                        return;
                    }
                }
                subControls.clear();
                if (m_touchMoveControl != control) {
                    //调用触摸离开事件
                    if (m_touchMoveControl != null && isPaintEnabled(m_touchMoveControl)) {
                        if (m_touchDownControl == null) {
                            FCPoint cmp = new FCPoint(mp.x - clientX(m_touchMoveControl), mp.y - clientY(m_touchMoveControl));
                            FCView touchMoveControl = m_touchMoveControl;
                            m_touchMoveControl = control;
                            FCTouchInfo newTouchInfo = touchInfo.clone();
                            newTouchInfo.m_firstPoint = cmp;
                            newTouchInfo.m_secondPoint = cmp;
                            touchMoveControl.onTouchLeave(newTouchInfo);
                        }
                    }
                    if (control != null && isPaintEnabled(control)) {
                        if (m_touchDownControl == null) {
                            FCPoint cmp = new FCPoint(mp.x - clientX(control), mp.y - clientY(control));
                            m_touchMoveControl = control;
                            //调用触摸进入事件
                            FCTouchInfo newTouchInfo = touchInfo.clone();
                            newTouchInfo.m_firstPoint = cmp;
                            newTouchInfo.m_secondPoint = cmp;
                            control.onTouchEnter(newTouchInfo);
                            control.onTouchMove(newTouchInfo);
                            setCursor(control);
                        }
                    }
                }
                else {
                    //调用触摸移动事件
                    if (control != null && isPaintEnabled(control)) {
                        FCPoint cmp = new FCPoint(mp.x - clientX(control), mp.y - clientY(control));
                        m_touchMoveControl = control;
                        FCTouchInfo newTouchInfo = touchInfo.clone();
                        newTouchInfo.m_firstPoint = cmp;
                        newTouchInfo.m_secondPoint = cmp;
                        control.onTouchMove(newTouchInfo);
                        setCursor(control);
                    }
                }
            }
        }

        /// <summary>
        /// 触摸弹起事件
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public void onMouseUp(FCTouchInfo touchInfo) {
            FCPoint mp = TouchPoint;
            ArrayList<FCView> subControls = new ArrayList<FCView>();
            getSortedControls(null, subControls);
            //获取触摸按下的控件
            if (m_touchDownControl != null) {
                FCView touchDownControl = m_touchDownControl;
                if (onPreviewTouchEvent(FCEventID.TOUCHUP, touchDownControl, touchInfo)) {
                    m_touchDownControl = null;
                    return;
                }
                if (m_touchDownControl != null) {
                    FCView control = findControl(mp, subControls);
                    FCPoint cmp = new FCPoint(mp.x - clientX(m_touchDownControl), mp.y - clientY(m_touchDownControl));
                    if (control != null && control == m_touchDownControl) {
                        FCTouchInfo newTouchInfo = touchInfo.clone();
                        newTouchInfo.m_firstPoint = cmp;
                        newTouchInfo.m_secondPoint = cmp;
                        m_touchDownControl.onClick(newTouchInfo);
                    }
                    else {
                        m_touchMoveControl = null;
                    }
                    //触发单击和双击事件
                    if (m_touchDownControl != null) {
                        touchDownControl = m_touchDownControl;
                        m_touchDownControl = null;
                        FCTouchInfo newTouchInfo = touchInfo.clone();
                        newTouchInfo.m_firstPoint = cmp;
                        newTouchInfo.m_secondPoint = cmp;
                        touchDownControl.onTouchUp(newTouchInfo);
                    }
                }
            }
            else if (m_draggingControl != null) {
                FCPoint cmp = new FCPoint(mp.x - clientX(m_touchDownControl), mp.y - clientY(m_touchDownControl));
                FCView draggingControl = m_draggingControl;
                m_draggingControl = null;
                if (onPreviewTouchEvent(FCEventID.TOUCHUP, draggingControl, touchInfo)) {
                    return;
                }
                //触发单击和双击事件
                FCTouchInfo newTouchInfo = touchInfo.clone();
                newTouchInfo.m_firstPoint = cmp;
                newTouchInfo.m_secondPoint = cmp;
                draggingControl.onTouchUp(newTouchInfo);
                //触发拖动结束事件
                draggingControl.onDragEnd();
                FCView parent = draggingControl.Parent;
                if (parent != null) {
                    parent.invalidate();
                }
                else {
                    invalidate();
                }
            }
            subControls.clear();
        }

        /// <summary>
        /// 触摸滚动事件
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public void onMouseWheel(FCTouchInfo touchInfo) {
            FCView focusedControl = FocusedControl;
            if (focusedControl != null && isPaintEnabled(focusedControl)) {
                FCPoint mp = TouchPoint;
                if (onPreviewTouchEvent(FCEventID.TOUCHWHEEL, focusedControl, touchInfo)) {
                    return;
                }
                mp.x -= clientX(focusedControl);
                mp.y -= clientY(focusedControl);
                touchInfo.m_firstPoint = mp;
                touchInfo.m_secondPoint = mp;
                focusedControl.onTouchWheel(touchInfo);
            }
        }

        /// <summary>
        /// 绘图方法
        /// </summary>
        /// <param name="clipRect">矩形区域</param>
        public void onPaint(FCRect clipRect) {
            ArrayList<FCView> subCotrols = new ArrayList<FCView>();
            getSortedControls(null, subCotrols);
            renderControls(clipRect, subCotrols, m_resourcePath, m_opacity);
            subCotrols.clear();
        }

        /// <summary>
        /// 预处理键盘事件
        /// </summary>
        /// <param name="eventID">事件ID</param>
        /// <param name="key">按键</param>
        /// <returns>状态</returns>
        public bool onPreviewKeyEvent(int eventID, char key) {
            FCView focusedControl = FocusedControl;
            if (focusedControl != null && isPaintEnabled(focusedControl)) {
                FCView window = findWindow(focusedControl);
                if (window != null) {
                    return window.onPreviewsKeyEvent(eventID, key);
                }
            }
            return false;
        }

        /// <summary>
        /// 预处理触摸事件
        /// </summary>
        /// <param name="eventID">事件ID</param>
        /// <param name="control">控件</param>
        /// <param name="touchInfo">触摸信息</param>
        /// <returns></returns>
        public bool onPreviewTouchEvent(int eventID, FCView control, FCTouchInfo touchInfo) {
            FCView previewsControl = findPreviewsControl(control);
            if (previewsControl != null) {
                FCPoint mp = touchInfo.m_firstPoint;
                int clx = clientX(previewsControl);
                int cly = clientY(previewsControl);
                FCPoint wcmp = new FCPoint(mp.x - clx, mp.y - cly);
                FCTouchInfo newTouchInfo = touchInfo.clone();
                newTouchInfo.m_firstPoint = wcmp;
                newTouchInfo.m_secondPoint = wcmp;
                if (previewsControl.onPreviewsTouchEvent(eventID, newTouchInfo)) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 处理尺寸改变
        /// </summary>
        public void onResize() {
            update();
        }

        /// <summary>
        /// 处理秒表
        /// </summary>
        /// <param name="timerID">秒表ID</param>
        public void onTimer(int timerID) {
            if (m_timers.containsKey(timerID)) {
                m_timers.get(timerID).onTimer(timerID);
            }
            int mirrorsSize = m_mirrors.size();
            if (mirrorsSize > 0) {
                for (int i = 0; i < mirrorsSize; i++) {
                    m_mirrors.get(i).onTimer(timerID);
                }
            }
        }

        /// <summary>
        /// 绘制控件
        /// </summary>
        /// <param name="rect">矩形</param>
        /// <param name="controls">控件集合</param>
        /// <param name="resourcePath">资源路径</param>
        /// <param name="opacity">透明度</param>
        private void renderControls(FCRect rect, ArrayList<FCView> controls, String resourcePath, float opacity) {
            int controlSize = controls.size();
            for (int i = 0; i < controlSize; i++) {
                FCView control = controls.get(i);
                control.onPrePaint(m_paint, control.DisplayRect);
                FCRect destRect = new FCRect();
                int clx = clientX(control);
                int cly = clientY(control);
                FCRect bounds = new FCRect(clx, cly, clx + control.Width, cly + control.Height);
                //获取自定义裁剪区域
                if (control.UseRegion) {
                    FCRect clipRect = control.Region;
                    bounds.left += clipRect.left;
                    bounds.top += clipRect.top;
                    bounds.right = bounds.left + clipRect.right - clipRect.left;
                    bounds.bottom = bounds.top + clipRect.bottom - clipRect.top;
                }
                if (control.Visible && m_host.getIntersectRect(ref destRect, ref rect, ref bounds) > 0) {
                    //设置裁剪
                    FCRect clipRect = new FCRect(destRect.left - clx, destRect.top - cly,
                        destRect.right - clx, destRect.bottom - cly);
                    //设置资源路径
                    String newResourcePath = control.ResourcePath;
                    if (newResourcePath == null || newResourcePath.Length == 0) {
                        newResourcePath = resourcePath;
                    }
                    //设置透明度
                    float newOpacity = control.Opacity * opacity;
                    setPaint(clx, cly, clipRect, newResourcePath, newOpacity);
                    control.onPaint(m_paint, clipRect);
                    //绘制子控件
                    ArrayList<FCView> subControls = new ArrayList<FCView>();
                    getSortedControls(control, subControls);
                    if (subControls != null && subControls.size() > 0) {
                        renderControls(destRect, subControls, newResourcePath, newOpacity);
                        subControls.clear();
                    }
                    setPaint(clx, cly, clipRect, newResourcePath, newOpacity);
                    control.onPaintBorder(m_paint, clipRect);
                }
            }
        }

        /// <summary>
        /// 移除控件
        /// </summary>
        /// <param name="control">控件</param>
        public void removeControl(FCView control) {
            if (control == m_draggingControl) {
                m_draggingControl = null;
            }
            if (control == m_focusedControl) {
                m_focusedControl = null;
            }
            if (control == m_touchDownControl) {
                m_touchDownControl = null;
            }
            if (control == m_touchMoveControl) {
                m_touchMoveControl = null;
            }
            ArrayList<int> removeTimers = new ArrayList<int>();
            foreach (int timerID in m_timers.Keys) {
                if (m_timers.get(timerID) == control) {
                    removeTimers.add(timerID);
                }
            }
            foreach (int timerID in removeTimers) {
                stopTimer(timerID);
            }
            if (control.Parent == null) {
                m_controls.remove(control);
                control.onRemove();
            }
        }

        /// <summary>
        /// 移除镜像
        /// </summary>
        /// <param name="control">控件</param>
        public void removeMirror(FCView control) {
            m_mirrorHost.Mirrors.remove(this);
            m_controls.remove(control);
            control.Native = m_mirrorHost;
        }

        /// <summary>
        /// 将控件放到最下面显示
        /// </summary>
        /// <param name="control">控件</param>
        public void sendToBack(FCView control) {
            FCView parent = control.Parent;
            if (parent != null) {
                parent.sendChildToBack(control);
            }
            else {
                if (m_controls != null && m_controls.size() > 0) {
                    m_controls.remove(control);
                    m_controls.Insert(0, control);
                }
            }
        }

        /// <summary>
        /// 设置排列
        /// </summary>
        /// <param name="controls">控件集合</param>
        public void setAlign(ArrayList<FCView> controls) {
            int controlSize = controls.size();
            for (int i = 0; i < controlSize; i++) {
                FCView control = controls.get(i);
                if (control.DisplayOffset) {
                    FCSize parentSize = m_displaySize;
                    FCView parent = control.Parent;
                    if (parent != null) {
                        if (!(m_mirrorMode == FCMirrorMode.BugHole && controls == m_controls)) {
                            parentSize = parent.Size;
                        }
                    }
                    FCSize size = control.Size;
                    FCPadding margin = control.Margin;
                    FCPadding padding = new FCPadding();
                    if (parent != null) {
                        padding = parent.Padding;
                    }
                    if (control.Align == FCHorizontalAlign.Center) {
                        control.Left = (parentSize.cx - size.cx) / 2;
                    }
                    else if (control.Align == FCHorizontalAlign.Right) {
                        control.Left = parentSize.cx - size.cx - margin.right - padding.right;
                    }
                    if (control.VerticalAlign == FCVerticalAlign.Bottom) {
                        control.Top = parentSize.cy - size.cy - margin.bottom - padding.bottom;
                    }
                    else if (control.VerticalAlign == FCVerticalAlign.Middle) {
                        control.Top = (parentSize.cy - size.cy) / 2;
                    }
                }
            }
        }

        /// <summary>
        /// 设置锚定信息
        /// </summary>
        /// <param name="controls">控件集合</param>
        /// <param name="oldSize">原尺寸</param>
        public void setAnchor(ArrayList<FCView> controls, FCSize oldSize) {
            if (oldSize.cx != 0 && oldSize.cy != 0) {
                int controlSize = controls.size();
                for (int i = 0; i < controlSize; i++) {
                    FCView control = controls.get(i);
                    FCSize parentSize = m_displaySize;
                    FCView parent = control.Parent;
                    if (parent != null) {
                        if (!(m_mirrorMode == FCMirrorMode.BugHole && controls == m_controls)) {
                            parentSize = parent.Size;
                        }
                    }
                    FCAnchor anchor = control.Anchor;
                    FCRect bounds = control.Bounds;
                    setAnchor(ref bounds, ref parentSize, ref oldSize, anchor.left, anchor.top, anchor.right, anchor.bottom);
                    control.Bounds = bounds;
                }
            }
        }

        public static void setAnchor(ref FCRect bounds, ref FCSize parentSize, ref FCSize oldSize, bool anchorLeft, bool anchorTop, bool anchorRight, bool anchorBottom) {
            if (anchorRight && !anchorLeft) {
                bounds.left = bounds.left + parentSize.cx - oldSize.cx;
            }
            if (anchorBottom && !anchorTop) {
                bounds.top = bounds.top + parentSize.cy - oldSize.cy;
            }
            if (anchorRight) {
                bounds.right = bounds.right + parentSize.cx - oldSize.cx;
            }
            if (anchorBottom) {
                bounds.bottom = bounds.bottom + parentSize.cy - oldSize.cy;
            }
        }

        /// <summary>
        /// 设置光标
        /// </summary>
        /// <param name="control">控件</param>
        public void setCursor(FCView control) {
            FCCursors cursor = control.Cursor;
            if (!isPaintEnabled(control)) {
                cursor = FCCursors.Arrow;
            }
            if (this.Cursor != cursor) {
                this.Cursor = cursor;
            }
        }

        /// <summary>
        /// 设置绑定边缘
        /// </summary>
        /// <param name="control">控件</param>
        public void setDock(ArrayList<FCView> controls) {
            int controlSize = controls.size();
            for (int i = 0; i < controlSize; i++) {
                FCView control = controls.get(i);
                FCSize parentSize = m_displaySize;
                FCView parent = control.Parent;
                if (parent != null) {
                    if (!(m_mirrorMode == FCMirrorMode.BugHole && controls == m_controls)) {
                        parentSize = parent.Size;
                    }
                }
                FCDockStyle dock = control.Dock;
                if (dock != FCDockStyle.None) {
                    FCPadding padding = new FCPadding();
                    if (parent != null) {
                        padding = parent.Padding;
                    }
                    FCPadding margin = control.Margin;
                    FCSize cSize = control.Size;
                    FCRect spaceRect = new FCRect();
                    spaceRect.left = padding.left + margin.left;
                    spaceRect.top = padding.top + margin.top;
                    spaceRect.right = parentSize.cx - padding.right - margin.right;
                    spaceRect.bottom = parentSize.cy - padding.bottom - margin.bottom;
                    FCRect bounds = new FCRect();
                    int dockStyle = -1;
                    if (dock == FCDockStyle.Bottom) {
                        dockStyle = 4;
                    }
                    else if (dock == FCDockStyle.Fill) {
                        dockStyle = 0;
                    }
                    else if (dock == FCDockStyle.Left) {
                        dockStyle = 1;
                    }
                    else if (dock == FCDockStyle.Right) {
                        dockStyle = 3;
                    }
                    else if (dock == FCDockStyle.Top) {
                        dockStyle = 2;
                    }
                    setDock(ref bounds, ref spaceRect, ref cSize, dockStyle);
                    control.Bounds = bounds;
                }
            }
        }

        public static void setDock(ref FCRect bounds, ref FCRect spaceRect, ref FCSize cSize, int dock) {
            if (dock == 0) {
                bounds.left = spaceRect.left;
                bounds.top = spaceRect.top;
                bounds.right = spaceRect.right;
                bounds.bottom = spaceRect.bottom;
            }
            else if (dock == 1) {
                bounds.left = spaceRect.left;
                bounds.top = spaceRect.top;
                bounds.right = bounds.left + cSize.cx;
                bounds.bottom = spaceRect.bottom;
            }
            else if (dock == 2) {
                bounds.left = spaceRect.left;
                bounds.top = spaceRect.top;
                bounds.right = spaceRect.right;
                bounds.bottom = bounds.top + cSize.cy;
            }
            else if (dock == 3) {
                bounds.left = spaceRect.right - cSize.cx;
                bounds.top = spaceRect.top;
                bounds.right = spaceRect.right;
                bounds.bottom = spaceRect.bottom;
            }
            else if (dock == 4) {
                bounds.left = spaceRect.left;
                bounds.top = spaceRect.bottom - cSize.cy;
                bounds.right = spaceRect.right;
                bounds.bottom = spaceRect.bottom;
            }
        }

        /// <summary>
        /// 设置绘图属性
        /// </summary>
        /// <param name="offsetX">横向偏移</param>
        /// <param name="offsetY">纵向偏移</param>
        /// <param name="clipRect">裁剪区域</param>
        /// <param name="resourcePath">资源路径</param>
        /// <param name="opacity">透明度</param>
        private void setPaint(int offsetX, int offsetY, FCRect clipRect, String resourcePath, float opacity) {
            m_paint.setOffset(new FCPoint(offsetX, offsetY));
            m_paint.setClip(clipRect);
            m_paint.setResourcePath(resourcePath);
            m_paint.setOpacity(opacity);
        }

        /// <summary>
        /// 启动秒表
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="timerID">秒表编号</param>
        /// <param name="interval">间隔</param>
        public void startTimer(FCView control, int timerID, int interval) {
            m_timers.put(timerID, control);
            if (m_host != null) {
                m_host.startTimer(timerID, interval);
            }
        }

        /// <summary>
        /// 停止秒表
        /// </summary>
        /// <param name="timerID">秒表编号</param>
        public void stopTimer(int timerID) {
            if (m_timers.containsKey(timerID)) {
                if (m_host != null) {
                    m_host.stopTimer(timerID);
                }
                m_timers.remove(timerID);
            }
        }

        /// <summary>
        /// 更新布局
        /// </summary>
        public void update() {
            if (m_host != null) {
                FCSize oldSize = m_displaySize;
                m_displaySize = m_host.getSize();
                if (m_displaySize.cx != 0 && m_displaySize.cy != 0) {
                    setAlign(m_controls);
                    setAnchor(m_controls, oldSize);
                    setDock(m_controls);
                    int controlsSize = m_controls.size();
                    for (int i = 0; i < controlsSize; i++) {
                        m_controls.get(i).update();
                    }
                }
            }
        }
    }
}
