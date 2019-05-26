/*基于捂脸猫FaceCat框架 v1.0
 捂脸猫创始人-矿洞程序员-脉脉KOL-陶德 (微信号:suade1984);
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using FaceCat;
using System.Windows.Forms;

namespace FaceCat
{
    /// <summary>
    /// 调整控件大小的图层
    /// </summary>
    public class ResizeDiv : FCView
    {
        /// <summary>
        /// 创建图层
        /// </summary>
        public ResizeDiv()
        {
            AllowDrag = true;
            BorderColor = FCColor.None;
        }

        /// <summary>
        /// 调整尺寸点的大小
        /// </summary>
        private const int RESIZEPOINT_SIZE = 6;

        /// <summary>
        /// 方向
        /// </summary>
        private int m_direction;

        /// <summary>
        /// 坐标标签
        /// </summary>
        private FCLabel m_lblXY;

        /// <summary>
        /// 大小标签
        /// </summary>
        private FCLabel m_lblWH;

        /// <summary>
        /// 横向移动步长
        /// </summary>
        private int m_offsetX = 0;

        /// <summary>
        /// 纵向移动步长
        /// </summary>
        private int m_offsetY = 0;

        /// <summary>
        /// 位置1
        /// </summary>
        private FCPoint m_point1;

        /// <summary>
        /// 位置2
        /// </summary>
        private FCPoint m_point2;

        /// <summary>
        /// 随机种子
        /// </summary>
        private Random m_random = new Random();

        /// <summary>
        /// 调整尺寸的点
        /// </summary>
        private int m_resizePoint = -1;

        /// <summary>
        /// 移动开始点
        /// </summary>
        private FCPoint m_startTouchPoint;

        /// <summary>
        /// 移动开始时的控件矩形
        /// </summary>
        private FCRect m_startRect;

        /// <summary>
        /// 移动步长
        /// </summary>
        private int m_step = 1;

        /// <summary>
        /// 目标列表
        /// </summary>
        private List<FCView> m_targets = new List<FCView>();

        /// <summary>
        /// 计数
        /// </summary>
        private int m_tick;

        private bool m_acceptTouch = true;

        /// <summary>
        /// 获取或设置是否接受鼠标动作
        /// </summary>
        public bool AcceptTouch
        {
            get { return m_acceptTouch; }
            set { m_acceptTouch = value; }
        }

        private DesignerDiv m_designerDiv;

        /// <summary>
        /// 获取或设置编辑视图
        /// </summary>
        public DesignerDiv DesignerDiv
        {
            get { return m_designerDiv; }
            set { m_designerDiv = value; }
        }

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
        /// 添加目标
        /// </summary>
        /// <param name="targets">目标列表</param>
        public void addTargets(List<FCView> targets)
        {
            int targetsSize = targets.Count;
            for (int i = 0; i < targetsSize; i++)
            {
                m_targets.Add(targets[i]);
            }
        }

        /// <summary>
        /// 清除目标
        /// </summary>
        public void clearTargets()
        {
            m_targets.Clear();
        }

        /// <summary>
        /// 退出拖动
        /// </summary>
        public void cancelDragging()
        {
            if (IsDragging)
            {
                FCNative native = Native;
                native.cancelDragging();
                invalidate();
            }
        }

        /// <summary>
        /// 是否可以移动
        /// </summary>
        /// <returns>是否可以移动</returns>
        public bool canDragTargets()
        {
            int targetsSize = m_targets.Count;
            if (targetsSize == 1)
            {
                if (m_targets[0].Parent.Parent is DesignerDiv)
                {
                    return false;
                }
                if (m_targets[0] is FCTabPage)
                {
                    return false;
                }
                if (m_targets[0].Parent is FCSplitLayoutDiv || m_targets[0].Parent is FCLayoutDiv)
                {
                    return false;
                }
            }
            if (m_resizePoint != -1 && m_resizePoint != 8)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 是否可以重置大小
        /// </summary>
        /// <returns>是否可以重置大小</returns>
        public bool canResizeTargets()
        {
            int targetsSize = m_targets.Count;
            if (targetsSize == 1)
            {
                if (m_targets[0] is FCTabPage)
                {
                    return false;
                }
                if (m_targets[0].Parent is FCSplitLayoutDiv || m_targets[0].Parent is FCLayoutDiv)
                {
                    return false;
                }
            }
            if (m_resizePoint != -1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 判断是否包含坐标
        /// </summary>
        /// <param name="point">坐标</param>
        /// <returns>否包含</returns>
        public override bool containsPoint(FCPoint point)
        {
            return m_acceptTouch;
        }

        /// <summary>
        /// 区域转换为绘制区域
        /// </summary>
        /// <param name="bounds">区域</param>
        /// <returns>绘制区域</returns>
        public FCRect convertBoundsToPRect(FCRect bounds)
        {
            FCRect pRect = bounds;
            int nSize = 3;
            if (pRect.right - pRect.left > 6)
            {
                pRect.left += nSize;
                pRect.right -= nSize;
            }
            if (pRect.bottom - pRect.top > 6)
            {
                pRect.top += nSize;
                pRect.bottom -= nSize;
            }
            return pRect;
        }

        /// <summary>
        /// 绘制区域转换为区域
        /// </summary>
        /// <param name="pRect">绘制区域</param>
        /// <returns>区域</returns>
        public FCRect convertPRectToBounds(FCRect pRect)
        {
            FCRect bounds = pRect;
            int nSize = 3;
            bounds.left -= nSize;
            bounds.top -= nSize;
            bounds.right += nSize;
            bounds.bottom += nSize;
            return bounds;
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <returns>控件类型</returns>
        public override String getControlType()
        {
            return "ResizeDiv";
        }

        /// <summary>
        /// 获取光标
        /// </summary>
        /// <param name="resizePoint">调整大小的点</param>
        /// <returns>光标</returns>
        private FCCursors getCursor(int resizePoint)
        {
            switch (resizePoint)
            {
                case 0:
                    return FCCursors.SizeWE;
                case 1:
                    return FCCursors.SizeNS;
                case 2:
                    return FCCursors.SizeWE;
                case 3:
                    return FCCursors.SizeNS;
                case 4:
                    return FCCursors.SizeNWSE;
                case 5:
                    return FCCursors.SizeNESW;
                case 6:
                    return FCCursors.SizeNESW;
                case 7:
                    return FCCursors.SizeNWSE;
                case 8:
                    return FCCursors.SizeAll;
                default:
                    return FCCursors.Arrow;
            }
        }

        /// <summary>
        /// 获取偏移坐标
        /// </summary>
        /// <returns>偏移坐标</returns>
        public FCPoint getOffsetPoint()
        {
            FCPoint offset = new FCPoint();
            FCView parent = Parent;
            if (parent != null)
            {
                FCTabPage designerDiv = parent as FCTabPage;
                offset.x += (designerDiv.HScrollBar != null ? designerDiv.HScrollBar.Pos : 0);
                offset.y += (designerDiv.VScrollBar != null ? designerDiv.VScrollBar.Pos : 0);
            }
            return offset;
        }

        /// <summary>
        /// 根据两个点获取矩形区域
        /// </summary>
        /// <param name="point1">坐标1</param>
        /// <param name="point2">坐标2</param>
        /// <returns>矩形区域</returns>
        public FCRect getRectangle(FCPoint point1, FCPoint point2)
        {
            int minX = Math.Min(point1.x, point2.x);
            int maxX = Math.Max(point1.x, point2.x);
            int minY = Math.Min(point1.y, point2.y);
            int maxY = Math.Max(point1.y, point2.y);
            return new FCRect(minX, minY, maxX, maxY);
        }

        /// <summary>
        /// 获取调整尺寸的点
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>调整尺寸的点</returns>
        private int getResizePoint(FCPoint mp)
        {
            FCRect[] pRects = getResizePoints();
            int rsize = pRects.Length;
            for (int i = rsize - 1; i >= 0; i--)
            {
                FCRect rect = pRects[i];
                if (mp.x >= rect.left && mp.x <= rect.right
                    && mp.y >= rect.top && mp.y <= rect.bottom)
                {
                    return i;
                }
            }
            int targetsSize = m_targets.Count;
            if (targetsSize > 0)
            {
                int width = Width, height = Height;
                if (mp.x >= 0 && mp.x <= width && mp.y >= 0 && mp.y <= height)
                {
                    bool inBorder = false;
                    int size = RESIZEPOINT_SIZE;
                    if (mp.x <= size
                    || mp.x >= width - size
                    || mp.y <= size
                    || mp.y >= height - size)
                    {
                        inBorder = true;
                    }
                    if (targetsSize == 1)
                    {
                        if (m_xml.isContainer(m_targets[0]))
                        {
                            if (inBorder)
                            {
                                return 8;
                            }
                        }
                        else
                        {
                            return 8;
                        }
                    }
                    else
                    {
                        if (inBorder)
                        {
                            return 8;
                        }
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// 获取调整尺寸的点
        /// </summary>
        /// <returns>矩形集合</returns>
        private FCRect[] getResizePoints()
        {
            int width = Width - 1;
            int height = Height - 1;
            FCRect[] points = new FCRect[9];
            int size = RESIZEPOINT_SIZE;
            //左
            points[0] = new FCRect(0, height / 2 - size, size, height / 2 + size);
            //上
            points[1] = new FCRect(width / 2 - size, 0, width / 2 + size, size);
            //右
            points[2] = new FCRect(width - size, height / 2 - size, width, height / 2 + size);
            //下
            points[3] = new FCRect(width / 2 - size, height - size, width / 2 + size, height);
            //左上
            points[4] = new FCRect(0, 0, size, size);
            //左下
            points[5] = new FCRect(0, height - size, size, height);
            //右上
            points[6] = new FCRect(width - size, 0, width, size);
            //右下
            points[7] = new FCRect(width - size, height - size, width, height);
            return points;
        }

        /// <summary>
        /// 获取目标列表
        /// </summary>
        /// <returns>目标列表</returns>
        public List<FCView> getTargets()
        {
            List<FCView> targets = new List<FCView>();
            int targetsSize = m_targets.Count;
            for (int i = 0; i < targetsSize; i++)
            {
                targets.Add(m_targets[i]);
            }
            return targets;
        }

        /// <summary>
        /// 是否正在调整尺寸
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>正在调整尺寸</returns>
        public bool isResizing(FCPoint mp)
        {
            FCNative native = Native;
            int clientX = native.clientX(this);
            int clientY = native.clientY(this);
            int width = Width, height = Height;
            if (mp.x >= clientX && mp.x <= clientX + width && mp.y >= clientY && mp.y <= clientY + height)
            {
                mp.x -= clientX;
                mp.y -= clientY;
                if (getResizePoint(TouchPoint) != -1)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 控件加载方法
        /// </summary>
        public override void onLoad()
        {
            base.onLoad();
            if (m_lblXY == null)
            {
                m_lblXY = m_native.findControl("lblXY") as FCLabel;
            }
            if (m_lblWH == null)
            {
                m_lblWH = m_native.findControl("lblWH") as FCLabel;
            }
            startTimer(getNewTimerID(), 100);
        }

        /// <summary>
        /// 拖动开始方法
        /// </summary>
        /// <returns>是否不处理</returns>
        public override bool onDragBegin()
        {
            int targetsSize = m_targets.Count;
            if (targetsSize == 1)
            {
                if (m_targets[0].Parent.Parent is DesignerDiv)
                {
                    return false;
                }
            }
            if (m_resizePoint == 8)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 拖动中方法
        /// </summary>
        public override void onDragging()
        {
            int targetsSize = m_targets.Count;
            base.onDragging();
            refreshStatusBar();
            Parent.update();
        }

        /// <summary>
        /// 拖动结束方法
        /// </summary>
        public override void onDragEnd()
        {
            base.onDragEnd();
            int targetsSize = m_targets.Count;
            FCView divDesigner = Parent;
            //判定区域
            FCNative native = Native;
            m_designerDiv.saveUndo();
            FCPoint mp = Native.TouchPoint;
            for (int i = 0; i < targetsSize; i++)
            {
                FCView target = m_targets[i];
                FCView parent = target.Parent;
                bool outControl = false;
                FCPoint oldNativeLocation = target.pointToNative(new FCPoint(0, 0));
                if (parent != null)
                {
                    if (divDesigner != null)
                    {
                        //查找新的控件
                        m_acceptTouch = false;
                        FCView newParent = native.findControl(mp, divDesigner);
                        m_acceptTouch = true;
                        if (newParent != null && m_xml.isContainer(newParent) && newParent != this && newParent != parent
                            && target != newParent)
                        {
                            //移除控件
                            m_xml.removeControl(target);
                            //添加控件
                            m_xml.addControl(target, newParent);
                            parent = newParent;
                            outControl = true;
                        }
                    }
                }
                if (outControl || canDragTargets())
                {
                    FCRect newRect = convertBoundsToPRect(Bounds);
                    oldNativeLocation.x += newRect.left - m_startRect.left;
                    oldNativeLocation.y += newRect.top - m_startRect.top;
                    m_xml.setProperty(target, "location", FCStr.convertPointToStr(target.Parent.pointToControl(oldNativeLocation)));
                    target.update();
                }
            }
            m_designerDiv.Designer.refreshProperties();
            refreshStatusBar();
            divDesigner.update();
        }

        /// <summary>
        /// 键盘按下方法
        /// </summary>
        /// <param name="key">按键</param>
        public override void onKeyDown(char key)
        {
            base.onKeyDown(key);
            if (!IsDragging)
            {
                if (key >= 37 && key <= 40)
                {
                    int targetsSize = m_targets.Count;
                    if (targetsSize > 0 && canDragTargets())
                    {
                        FCPoint location = Location;
                        //向左
                        if (key == 37)
                        {
                            location.x -= m_step;
                            m_offsetX -= m_step;
                        }
                        //向上
                        else if (key == 38)
                        {
                            location.y -= m_step;
                            m_offsetY -= m_step;
                        }
                        //向右
                        else if (key == 39)
                        {
                            location.x += m_step;
                            m_offsetX += m_step;
                        }
                        //向下
                        else if (key == 40)
                        {
                            location.y += m_step;
                            m_offsetY += m_step;
                        }
                        m_step += 1;
                        Location = location;
                        Parent.update();
                        refreshStatusBar();
                        Parent.invalidate();
                    }
                }
                else
                {
                    //删除控件
                    if (key == 46)
                    {
                        int targetsSize = m_targets.Count;
                        if (targetsSize > 0)
                        {
                            m_designerDiv.saveUndo();
                            for (int i = 0; i < targetsSize; i++)
                            {
                                FCView target = m_targets[i];
                                m_xml.removeControl(target);
                            }
                            m_targets.Clear();
                            Visible = false;
                            Parent.invalidate();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 按键抬起方法
        /// </summary>
        /// <param name="key">按键</param>
        public override void onKeyUp(char key)
        {
            if (!IsDragging)
            {
                m_step = 1;
                base.onKeyUp(key);
                if (m_offsetX != 0 || m_offsetY != 0)
                {
                    int targetsSize = m_targets.Count;
                    if (targetsSize > 0)
                    {
                        m_designerDiv.saveUndo();
                        for (int i = 0; i < targetsSize; i++)
                        {
                            FCView target = m_targets[i];
                            FCPoint oldLocation = target.Location;
                            oldLocation.x += m_offsetX;
                            oldLocation.y += m_offsetY;
                            m_xml.setProperty(target, "location", FCStr.convertPointToStr(oldLocation));
                            target.update();
                        }
                        m_designerDiv.Designer.refreshProperties();
                    }
                    refreshStatusBar();
                    Parent.invalidate();
                }
            }
            m_offsetX = 0;
            m_offsetY = 0;
        }

        /// <summary>
        /// 鼠标按下方法
        /// </summary>
        /// <param name="touchInto">触摸信息</param>
        public override void onTouchDown(FCTouchInfo touchInto)
        {
            base.onTouchDown(touchInto);
            m_point1 = Native.TouchPoint;
            m_point2 = Native.TouchPoint;
            //选中点
            if (touchInto.m_firstTouch && touchInto.m_clicks == 1)
            {
                FCPoint mp = touchInto.m_firstPoint;
                if (canResizeTargets())
                {
                    m_resizePoint = getResizePoint(mp);
                    if (m_resizePoint != -1)
                    {
                        Cursor = getCursor(m_resizePoint);
                        m_startTouchPoint = Native.TouchPoint;
                        m_startRect = convertBoundsToPRect(Bounds);
                    }
                }
            }
            refreshStatusBar();
            invalidate();
        }

        /// <summary>
        /// 鼠标移动方法
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚轮值</param>
        public override void onTouchMove(FCTouchInfo touchInto)
        {
            base.onTouchMove(touchInto);
            m_point2 = Native.TouchPoint;
            if (m_resizePoint != -1 && m_resizePoint != 8)
            {
                FCPoint mp = Native.TouchPoint;
                int left = m_startRect.left, top = m_startRect.top, right = m_startRect.right, bottom = m_startRect.bottom;
                switch (m_resizePoint)
                {
                    //左
                    case 0:
                        left = left + mp.x - m_startTouchPoint.x;
                        break;
                    //上
                    case 1:
                        top = top + mp.y - m_startTouchPoint.y;
                        break;
                    //右
                    case 2:
                        right = right + mp.x - m_startTouchPoint.x;
                        break;
                    //下
                    case 3:
                        bottom = bottom + mp.y - m_startTouchPoint.y;
                        break;
                    //左上
                    case 4:
                        left = left + mp.x - m_startTouchPoint.x;
                        top = top + mp.y - m_startTouchPoint.y;
                        break;
                    //左下
                    case 5:
                        left = left + mp.x - m_startTouchPoint.x;
                        bottom = bottom + mp.y - m_startTouchPoint.y;
                        break;
                    //右上
                    case 6:
                        right = right + mp.x - m_startTouchPoint.x;
                        top = top + mp.y - m_startTouchPoint.y;
                        break;
                    //右下
                    case 7:
                        right = right + mp.x - m_startTouchPoint.x;
                        bottom = bottom + mp.y - m_startTouchPoint.y;
                        break;
                }
                FCRect bounds = new FCRect(left, top, right, bottom);
                Bounds = convertPRectToBounds(bounds);
                m_designerDiv.invalidate();
            }
            else
            {
                Cursor = getCursor(getResizePoint(TouchPoint));
                invalidate();
            }
        }

        /// <summary>
        /// 鼠标抬起方法
        /// </summary>
        /// <param name="touchInto">触摸信息</param>
        public override void onTouchUp(FCTouchInfo touchInto)
        {
            m_point2 = Native.TouchPoint;
            base.onTouchUp(touchInto);
            if (!IsDragging)
            {
                FCView divDesigner = Parent;
                if (m_resizePoint != -1)
                {
                    FCRect nowRect = convertBoundsToPRect(Bounds);
                    //移动控件
                    int targetsSize = m_targets.Count;
                    if (targetsSize > 0)
                    {
                        m_designerDiv.saveUndo();
                        for (int i = 0; i < targetsSize; i++)
                        {
                            FCView target = m_targets[i];
                            FCPoint oldLocation = target.Location;
                            FCSize oldSize = target.Size;
                            oldLocation.x += nowRect.left - m_startRect.left;
                            oldLocation.y += nowRect.top - m_startRect.top;
                            oldSize.cx += (nowRect.right - nowRect.left) - (m_startRect.right - m_startRect.left);
                            oldSize.cy += (m_startRect.top - m_startRect.bottom) - (nowRect.top - nowRect.bottom);
                            if (oldSize.cx < 4)
                            {
                                oldSize.cx = 4;
                            }
                            else if (oldSize.cy < 4)
                            {
                                oldSize.cy = 4;
                            }
                            m_xml.setProperty(target, "location", FCStr.convertPointToStr(oldLocation));
                            m_xml.setProperty(target, "size", FCStr.convertSizeToStr(oldSize));
                            target.update();
                        }
                    }
                    m_designerDiv.Designer.refreshProperties();
                }
                else
                {
                    if (m_targets.Count == 1)
                    {
                        if (Math.Abs(m_point1.x - m_point2.x) > 5 && Math.Abs(m_point1.y - m_point2.y) > 5)
                        {
                            //获取选中控件
                            FCView target = m_targets[0];
                            FCRect moveRect = getRectangle(pointToControl(m_point1), pointToControl(m_point2));
                            List<FCView> newTargets = new List<FCView>();
                            List<FCView> subControls = target.getControls();
                            int left = 0, top = 0, right = 0, bottom = 0, idx = 0;
                            int subControlsSize = subControls.Count;
                            for (int i = 0; i < subControlsSize; i++)
                            {
                                FCView subControl = subControls[i];
                                FCRect bounds = subControl.Bounds;
                                FCRect tempRect = new FCRect();
                                if (Native.Host.getIntersectRect(ref tempRect, ref moveRect, ref bounds) > 0)
                                {
                                    newTargets.Add(subControl);
                                    if (idx == 0)
                                    {
                                        left = bounds.left;
                                        top = bounds.top;
                                        right = bounds.right;
                                        bottom = bounds.bottom;
                                    }
                                    else
                                    {
                                        if (left > bounds.left)
                                        {
                                            left = bounds.left;
                                        }
                                        if (top > bounds.top)
                                        {
                                            top = bounds.top;
                                        }
                                        if (right < bounds.right)
                                        {
                                            right = bounds.right;
                                        }
                                        if (bottom < bounds.bottom)
                                        {
                                            bottom = bounds.bottom;
                                        }
                                    }
                                    idx++;
                                }
                            }
                            if (newTargets.Count > 0)
                            {
                                FCPoint p1 = target.pointToNative(new FCPoint(left, top));
                                FCPoint p2 = target.pointToNative(new FCPoint(right, bottom));
                                p1 = divDesigner.pointToControl(p1);
                                p2 = divDesigner.pointToControl(p2);
                                Bounds = convertPRectToBounds(new FCRect(p1.x, p1.y, p2.x, p2.y));
                                clearTargets();
                                addTargets(newTargets);
                                m_designerDiv.Designer.refreshProperties(newTargets);
                            }
                        }
                    }
                }
            }
            refreshStatusBar();
            m_resizePoint = -1;
            Cursor = FCCursors.Arrow;
            FCNative native = Native;
            native.Host.setCursor(FCCursors.Arrow);
            native.update();
            native.invalidate();
        }
        
        /// <summary>
        /// 重绘背景方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void onPaintBackground(FCPaint paint, FCRect clipRect)
        {
            int width = Width;
            int height = Height;
            FCNative native = Native;
            FCRect drawRect = convertBoundsToPRect(new FCRect(0, 0, width, height));
            //绘制选中的点
            int resizePoint = m_resizePoint != -1 ? m_resizePoint : getResizePoint(TouchPoint);
            if (width > 4 && height > 4)
            {
                long borderColor = FCDraw.FCCOLORS_BACKCOLOR8;
                //绘制多目标
                int targetsSize = m_targets.Count;
                if (targetsSize > 1)
                {
                    for (int i = 0; i < targetsSize; i++)
                    {
                        FCView target = m_targets[i];
                        FCPoint clientLocation = pointToControl(new FCPoint(Native.clientX(target), Native.clientY(target)));
                        paint.drawRect(borderColor, 2, 0,
                            new FCRect(clientLocation.x, clientLocation.y,
                            clientLocation.x + target.Width,
                            clientLocation.y + target.Height));
                    }
                }
                if (resizePoint != -1 && resizePoint != 8)
                {
                    paint.fillGradientRect(FCDraw.FCCOLORS_BACKCOLOR5, FCDraw.FCCOLORS_BACKCOLOR6, drawRect, 0, 90);
                    paint.drawRect(borderColor, 1, 2, drawRect);
                    int thick = 4;
                    List<int> positions = new List<int>();
                    //左上右下
                    if (resizePoint < 4)
                    {
                        positions.Add(resizePoint);
                    }
                    else
                    {
                        switch (resizePoint)
                        {
                            //左上
                            case 4:
                                positions.Add(0);
                                positions.Add(1);
                                break;
                            //左下
                            case 5:
                                positions.Add(0);
                                positions.Add(3);
                                break;
                            //右上
                            case 6:
                                positions.Add(1);
                                positions.Add(2);
                                break;
                            //右下
                            case 7:
                                positions.Add(2);
                                positions.Add(3);
                                break;
                        }
                    }
                    //绘制选中点
                    int positionsSize = positions.Count;
                    for (int i = 0; i < positionsSize; i++)
                    {
                        switch (positions[i])
                        {
                            //左
                            case 0:
                                paint.fillRect(borderColor, new FCRect(0, 0, thick, height));
                                break;
                            //上
                            case 1:
                                paint.fillRect(borderColor, new FCRect(0, 0, width, thick));
                                break;
                            //右
                            case 2:
                                paint.fillRect(borderColor, new FCRect(width - thick, 0, width, height));
                                break;
                            //下
                            case 3:
                                paint.fillRect(borderColor, new FCRect(0, height - thick, width, height));
                                break;
                        }
                    }
                }
                else
                {
                    if (Native.PushedControl == this)
                    {
                        if (Math.Abs(m_point1.x - m_point2.x) > 5 && Math.Abs(m_point1.y - m_point2.y) > 5)
                        {
                            FCRect moveRect = getRectangle(pointToControl(m_point1), pointToControl(m_point2));
                            paint.drawRect(borderColor, 2, 0, moveRect);
                        }
                    }
                    paint.drawRect(borderColor, 2, 2, drawRect);
                    FCRect[] pRects = getResizePoints();
                    int pRectsSize = pRects.Length;
                    for (int p = 0; p < pRectsSize; p++)
                    {
                        paint.fillRect(borderColor, pRects[p]);
                    }
                    //绘制动画
                    if (resizePoint == -1)
                    {
                        if (m_tick < 20)
                        {
                            if (m_tick > 2 && m_tick < 18)
                            {
                                int xTick = width / 20;
                                int yTick = height / 20;
                                int tWidth = width / 3, tHeight = height / 3;
                                int nSize = RESIZEPOINT_SIZE;
                                paint.setLineCap(2, 2);
                                int a = (10 - Math.Abs(m_tick - 10)) * 20;
                                paint.drawLine(FCColor.argb(a, m_random.Next(0, 256), m_random.Next(0, 256), m_random.Next(0, 256)), m_random.Next(1, 10),
                                    0, xTick * m_tick - tWidth / 2, (m_direction == 1 ? nSize : height - nSize), xTick * m_tick + tWidth / 2, (m_direction == 1 ? nSize : height - nSize));
                                paint.drawLine(FCColor.argb(a, m_random.Next(0, 256), m_random.Next(0, 256), m_random.Next(0, 256)), m_random.Next(1, 10),
                                    0, width - xTick * m_tick - tWidth / 2, (m_direction == 0 ? nSize : height - nSize), width - (xTick * m_tick - tWidth / 2), (m_direction == 0 ? nSize : height - nSize));
                                paint.drawLine(FCColor.argb(a, m_random.Next(0, 256), m_random.Next(0, 256), m_random.Next(0, 256)), m_random.Next(1, 10),
                                    0, (m_direction == 1 ? nSize : width - nSize), height - yTick * m_tick - tHeight / 2, (m_direction == 1 ? nSize : width - nSize), height - (yTick * m_tick - tHeight / 2));
                                paint.drawLine(FCColor.argb(a, m_random.Next(0, 256), m_random.Next(0, 256), m_random.Next(0, 256)), m_random.Next(1, 10),
                                    0, (m_direction == 0 ? nSize : width - nSize), yTick * m_tick - tHeight / 2, (m_direction == 0 ? nSize : width - nSize), yTick * m_tick + tHeight / 2);
                                paint.setLineCap(0, 0);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 秒表事件
        /// </summary>
        /// <param name="timerID">秒表ID</param>
        public override void onTimer(int timerID)
        {
            base.onTimer(timerID);
            if (m_direction == 0)
            {
                m_tick++;
                if (m_tick >= 40)
                {
                    m_direction = 1;
                }
            }
            else if (m_direction == 1)
            {
                m_tick--;
                if (m_tick <= 0)
                {
                    m_direction = 0; 
                }
            }
            int resizePoint = m_resizePoint != -1 ? m_resizePoint : getResizePoint(TouchPoint);
            if (resizePoint == -1 && m_tick < 21)
            {
                invalidate();
            }
        }

        /// <summary>
        /// 刷新状态栏
        /// </summary>
        public void refreshStatusBar()
        {
            if (m_targets.Count > 0)
            {
                FCView target = m_targets[0];
                int clientX = m_native.clientX(target);
                int clientY = m_native.clientY(target);
                int thisClientX = m_native.clientX(this);
                int thisClientY = m_native.clientY(this);
                FCPoint location = target.Location;
                FCSize size = target.Size;
                m_lblXY.Text = String.Format("距左:{0} 上:{1} ", location.x + (thisClientX - clientX), location.y + (thisClientY - clientY));
                m_lblWH.Text = String.Format("长:{0} 宽:{1} ", size.cx, size.cy);
            }
            else
            {
                m_lblXY.Text = "距左:0 上:0";
                m_lblWH.Text = "长:0 宽:0";
            }
        }
    }
}
