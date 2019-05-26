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
using System.Drawing;

namespace FaceCat
{
    /// <summary>
    /// 安德鲁斯干草叉
    /// </summary>
    public sealed class AndrewsPitchfork : FCPlot
    {
        /// <summary>
        /// 创建安德鲁斯干草叉
        /// </summary>
        public AndrewsPitchfork()
        {
            PlotType = "ANDREWSPITCHFORK";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            if (m_sourceFields == null || !m_sourceFields.containsKey("CLOSE"))
            {
                return action;
            }
            FCPoint mp = getTouchOverPoint();
            //取索引
            int aIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int bIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
            int cIndex = m_dataSource.getRowIndex(m_marks.get(2).Key);
            int dIndex = m_dataSource.getRowIndex(m_marks.get(3).Key);
            float x1 = px(aIndex);
            float x2 = px(bIndex);
            float x3 = px(cIndex);
            float x4 = px(dIndex);
            float y1 = py(m_dataSource.get2(aIndex, m_sourceFields.get("CLOSE")));
            float y2 = py(m_marks.get(1).Value);
            float y3 = py(m_dataSource.get2(cIndex, m_sourceFields.get("CLOSE")));
            float y4 = py(m_dataSource.get2(dIndex, m_sourceFields.get("CLOSE")));
            //判断是否选中点
            bool selected = selectPoint(mp, x1, y1);
            if (selected)
            {
                action = ActionType.AT1;
                return action;
            }
            selected = selectPoint(mp, x2, y2);
            if (selected)
            {
                action = ActionType.AT2;
                return action;
            }
            selected = selectPoint(mp, x3, y3);
            if (selected)
            {
                action = ActionType.AT3;
                return action;
            }
            selected = selectPoint(mp, x4, y4);
            if (selected)
            {
                action = ActionType.AT4;
                return action;
            }
            //判断是否选中射线
            float k = 0, b = 0;
            selected = selectRay(mp, x1, y1, x2, y2, ref k, ref b);
            if (selected)
            {
                action = ActionType.MOVE;
                return action;
            }
            int wWidth = WorkingAreaWidth;
            //非垂直时
            if (k != 0 || b != 0)
            {
                float x3_newx = wWidth;
                if (bIndex < aIndex)
                {
                    x3_newx = 0;
                }
                b = y3 - x3 * k;
                float x3_newy = k * x3_newx + b;
                selected = selectRay(mp, x3, y3, x3_newx, x3_newy);
                if (selected)
                {
                    action = ActionType.MOVE;
                    return action;
                }
                float x4_newx = wWidth;
                if (bIndex < aIndex)
                {
                    x4_newx = 0;
                }
                b = y4 - x4 * k;
                float x4_newy = k * x4_newx + b;
                selected = selectRay(mp, x4, y4, x4_newx, x4_newy);
                if (selected)
                {
                    action = ActionType.MOVE;
                    return action;
                }
            }
            //垂直时
            else
            {
                if (y1 >= y2)
                {
                    selected = selectRay(mp, x3, y3, x3, 0);
                    if (selected)
                    {
                        action = ActionType.MOVE;
                        return action;
                    }
                    selected = selectRay(mp, x4, y4, x4, 0);
                    if (selected)
                    {
                        action = ActionType.MOVE;
                        return action;
                    }
                }
                else
                {
                    int wHeight = WorkingAreaHeight;
                    selected = selectRay(mp, x3, y3, x3, wHeight);
                    if (selected)
                    {
                        action = ActionType.MOVE;
                        return action;
                    }
                    selected = selectRay(mp, x4, y4, x4, wHeight);
                    if (selected)
                    {
                        action = ActionType.MOVE;
                        return action;
                    }
                }
            }
            return action;
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>初始化是否成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            bool create = create4CandlePoints(mp);
            if (create)
            {
                m_marks.put(1, new PlotMark(m_marks.get(1).Index, m_marks.get(1).Key, getNumberValue(mp)));
            }
            return create;
        }



        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.MOVE)
                {
                    Cursor = FCCursors.Hand;
                }
                else
                {
                    Cursor = FCCursors.SizeWE;
                }
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
                m_startMarks.put(2, m_marks.get(2));
                m_startMarks.put(3, m_marks.get(3));
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            if (m_sourceFields == null || !m_sourceFields.containsKey("CLOSE"))
            {
                return;
            }
            //取索引
            int aIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int bIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
            int cIndex = m_dataSource.getRowIndex(m_marks.get(2).Key);
            int dIndex = m_dataSource.getRowIndex(m_marks.get(3).Key);
            float x1 = px(aIndex);
            float x2 = px(bIndex);
            float x3 = px(cIndex);
            float x4 = px(dIndex);
            //取Y
            float y1 = py(m_dataSource.get2(aIndex, m_sourceFields.get("CLOSE")));
            float y2 = py(m_marks.get(1).Value);
            float y3 = py(m_dataSource.get2(cIndex, m_sourceFields.get("CLOSE")));
            float y4 = py(m_dataSource.get2(dIndex, m_sourceFields.get("CLOSE")));
            float k = 0, b = 0;
            lineXY(x1, y1, x2, y2, 0, 0, ref k, ref b);
            drawRay(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2, k, b);
            //画斜线
            if (k != 0 || b != 0)
            {
                float x3_newx = WorkingAreaWidth;
                if (bIndex < aIndex)
                {
                    x3_newx = 0;
                }
                b = y3 - x3 * k;
                float x3_newy = k * x3_newx + b;
                drawRay(paint, lineColor, m_lineWidth, m_lineStyle, x3, y3, x3_newx, x3_newy, k, b);
                float x4_newx = WorkingAreaWidth;
                if (bIndex < aIndex)
                {
                    x4_newx = 0;
                }
                b = y4 - x4 * k;
                float x4_newy = k * x4_newx + b;
                drawRay(paint, lineColor, m_lineWidth, m_lineStyle, x4, y4, x4_newx, x4_newy, k, b);
            }
            //画垂直线
            else
            {
                if (y1 >= y2)
                {
                    drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x3, y3, x3, 0);
                    drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x4, y4, x4, 0);
                }
                else
                {
                    drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x3, y3, x3, WorkingAreaHeight);
                    drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x4, y4, x4, WorkingAreaHeight);
                }
            }
            //画选中点
            if (Selected)
            {
                drawSelect(paint, lineColor, x1, y1);
                drawSelect(paint, lineColor, x2, y2);
                drawSelect(paint, lineColor, x3, y3);
                drawSelect(paint, lineColor, x4, y4);
            }
        }
    }

    /// <summary>
    /// 角度线
    /// </summary>
    public sealed class Angleline : FCPlot
    {
        /// <summary>
        /// 创建角度线
        /// </summary>
        public Angleline()
        {
            PlotType = "ANGLELINE";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            FCPoint mp = getTouchOverPoint();
            //获取参数
            ArrayList<PlotMark[]> mList = new ArrayList<PlotMark[]>();
            mList.add(new PlotMark[] { m_marks.get(0), m_marks.get(1) });
            mList.add(new PlotMark[] { m_marks.get(0), m_marks.get(2) });
            int listSize = mList.size();
            for (int i = 0; i < listSize; i++)
            {
                PlotMark markA = mList.get(i)[0];
                PlotMark markB = mList.get(i)[1];
                int bIndex = m_dataSource.getRowIndex(markA.Key);
                int eIndex = m_dataSource.getRowIndex(markB.Key);
                float x1 = px(bIndex);
                float x2 = px(eIndex);
                float y1 = py(markA.Value);
                float y2 = py(markB.Value);
                if (selectPoint(mp, x1, y1))
                {
                    action = ActionType.AT1;
                    return action;
                }
                else if (selectPoint(mp, x2, y2))
                {
                    if (i == 0)
                    {
                        action = ActionType.AT2;
                        return action;
                    }
                    else
                    {
                        action = ActionType.AT3;
                        return action;
                    }
                }
                float k = 0;
                float b = 0;
                //获取直线参数
                lineXY(x1, y1, x2, y2, 0, 0, ref k, ref b);
                if (!(k == 0 && b == 0))
                {
                    if (mp.y / (mp.x * k + b) >= 0.9 && mp.y / (mp.x * k + b) <= 1.1)
                    {
                        if (x1 >= x2)
                        {
                            if (mp.x > x1 + 5)
                            {
                                action = ActionType.NO;
                                return action;
                            }
                        }
                        else if (x1 < x2)
                        {
                            if (mp.x < x1 - 5)
                            {
                                action = ActionType.NO;
                                return action;
                            }
                        }
                        action = ActionType.MOVE;
                        return action;
                    }
                }
                else
                {
                    if (mp.x >= x1 - m_lineWidth * 5 && mp.x <= x1 + m_lineWidth * 5)
                    {
                        if (y1 >= y2)
                        {
                            if (mp.y <= y1 - 5)
                            {
                                action = ActionType.MOVE;
                                return action;
                            }
                        }
                        else
                        {
                            if (mp.y >= y1 - 5)
                            {
                                action = ActionType.MOVE;
                                return action;
                            }
                        }
                    }
                }
            }
            return action;
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return create3Points(mp);
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.MOVE)
                {
                    Cursor = FCCursors.Hand;
                }
                else
                {
                    Cursor = FCCursors.SizeWE;
                }
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
                m_startMarks.put(2, m_marks.get(2));
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取相对位置
            ArrayList<PlotMark[]> marks = new ArrayList<PlotMark[]>();
            marks.add(new PlotMark[] { pList.get(0), pList.get(1) });
            marks.add(new PlotMark[] { pList.get(0), pList.get(2) });
            int markSize = marks.size();
            for (int i = 0; i < markSize; i++)
            {
                PlotMark markA = marks.get(i)[0];
                PlotMark markB = marks.get(i)[1];
                float y1 = py(markA.Value);
                float y2 = py(markB.Value);
                int bIndex = m_dataSource.getRowIndex(markA.Key);
                int eIndex = m_dataSource.getRowIndex(markB.Key);
                //获取参数
                float[] param = getLineParams(markA, markB);
                float a = 0;
                float b = 0;
                float x1 = px(bIndex);
                float x2 = px(eIndex);
                //非垂直时
                if (param != null)
                {
                    a = param[0];
                    b = param[1];
                    float leftX = 0;
                    float leftY = leftX * a + b;
                    float rightX = WorkingAreaWidth;
                    float rightY = rightX * a + b;
                    if (x1 >= x2)
                    {
                        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, leftX, leftY, x2, y2);
                    }
                    else
                    {
                        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, rightX, rightY);
                    }
                    drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
                }
                //垂直时
                else
                {
                    if (y1 >= y2)
                    {
                        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, 0);
                    }
                    else
                    {
                        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, WorkingAreaHeight);
                    }
                }
                //画选中点
                if (Selected)
                {
                    drawSelect(paint, lineColor, x1, y1);
                    drawSelect(paint, lineColor, x2, y2);
                }
            }
        }
    }

    /// <summary>
    /// 箭头线段
    /// </summary>
    public sealed class ArrowSegment : FCPlot
    {
        /// <summary>
        /// 创建箭头线段
        /// </summary>
        public ArrowSegment()
        {
            PlotType = "ARROWSEGMENT";
        }

        private const int ARROW_SIZE = 10;

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            FCPoint mp = getTouchOverPoint();
            //获取点的位置
            float y1 = py(m_marks.get(0).Value);
            float y2 = py(m_marks.get(1).Value);
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            //获取参数
            float[] param = getLineParams(m_marks.get(0), m_marks.get(1));
            //产生动作类型
            if (param != null)
            {
                if (mp.x >= x1 - m_lineWidth * 2.5f && mp.x <= x1 + m_lineWidth * 2.5f)
                {
                    action = ActionType.AT1;
                    return action;
                }
                else if (mp.x >= x2 - m_lineWidth * 2.5f && mp.x <= x2 + m_lineWidth * 2.5f)
                {
                    action = ActionType.AT2;
                    return action;
                }
            }
            else
            {
                if (mp.y >= y1 - m_lineWidth * 5 && mp.y <= y1 + m_lineWidth * 5)
                {
                    action = ActionType.AT1;
                    return action;
                }
                else if (mp.y >= y2 - m_lineWidth * 5 && mp.y <= y2 + m_lineWidth * 5)
                {
                    action = ActionType.AT2;
                    return action;
                }
            }
            if (selectSegment(mp, x1, y1, x2, y2))
            {
                action = ActionType.MOVE;
            }
            return action;
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return create2PointsB(mp);
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.MOVE)
                {
                    Cursor = FCCursors.Hand;
                }
                else
                {
                    Cursor = FCCursors.SizeWE;
                }
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取相对位置  
            int y1 = (int)py(pList.get(0).Value);
            int y2 = (int)py(pList.get(1).Value);
            int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(pList.get(1).Key);
            int x1 = (int)px(bIndex);
            int x2 = (int)px(eIndex);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
            double slopy = 0, cosy = 0, siny = 0;
            slopy = Math.Atan2((double)(y1 - y2), (double)(x1 - x2));
            cosy = Math.Cos(slopy);
            siny = Math.Sin(slopy);
            FCPoint pt1 = new FCPoint(x2, y2);
            FCPoint pt2 = new FCPoint(pt1.x + (int)(ARROW_SIZE * cosy - (ARROW_SIZE / 2.0 * siny) + 0.5),
                pt1.y + (int)(ARROW_SIZE * siny + (ARROW_SIZE / 2.0 * cosy) + 0.5));
            FCPoint pt3 = new FCPoint(pt1.x + (int)(ARROW_SIZE * cosy + ARROW_SIZE / 2.0 * siny + 0.5),
                pt1.y - (int)(ARROW_SIZE / 2.0 * cosy - ARROW_SIZE * siny + 0.5));
            FCPoint[] points = new FCPoint[3] { pt1, pt2, pt3 };
            fillPolygon(paint, lineColor, points);
            //画选中点
            if (Selected)
            {
                drawSelect(paint, lineColor, x1, y1);
                drawSelect(paint, lineColor, x2, y2);
            }
        }
    }

    /// <summary>
    /// 箱形线
    /// </summary>
    public sealed class BoxLine : Rect
    {
        /// <summary>
        /// 创建箱形线
        /// </summary>
        public BoxLine()
        {
            PlotType = "BOXLINE";
        }

        /// <summary>
        /// 获取动作
        /// </summary>
        /// <param name="mp">点的位置</param>
        /// <returns>动作类型</returns>
        private ActionType getAction(FCPoint mp)
        {
            //获取参数
            double[] param = getCandleRange(m_marks);
            double nHigh = param[0];
            double nLow = param[1];
            if (param != null)
            {
                ActionType m_action = selectRect(mp, new PlotMark(0, m_marks.get(0).Key, nHigh), new PlotMark(1, m_marks.get(1).Key, nLow));
                return m_action;
            }
            return ActionType.NO;
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            FCPoint mp = getTouchOverPoint();
            action = getAction(mp);
            if (action == ActionType.AT4)
            {
                action = ActionType.AT2;
            }
            return action;
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return create2CandlePoints(mp);
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.MOVE)
                {
                    Cursor = FCCursors.Hand;
                }
                else
                {
                    Cursor = FCCursors.SizeWE;
                }
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
            }
        }

        /// <summary>
        /// 移动线条
        /// </summary>
        public override void onMoving()
        {
            FCPoint mp = getMovingPoint();
            //获取当前的索引和y值
            FCChart chart = Chart;
            int touchIndex = chart.getTouchOverIndex();
            if (touchIndex < 0)
            {
                touchIndex = 0;
            }
            if (touchIndex > chart.LastVisibleIndex)
            {
                touchIndex = chart.LastVisibleIndex;
            }
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
            //根据不同类型作出动作
            switch (m_action)
            {
                case ActionType.MOVE:
                    move(mp);
                    break;
                case ActionType.AT1:
                    if (touchIndex < eIndex)
                    {
                        resize(0);
                    }
                    break;
                case ActionType.AT2:
                    if (touchIndex > bIndex)
                    {
                        resize(1);
                    }
                    break;
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取点的位置
            int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(pList.get(1).Key);
            //获取参数
            double[] param = getCandleRange(pList);
            double nHigh = param[0];
            double nLow = param[1];
            if (param != null)
            {
                FCRect rect = getRectangle(new PlotMark(0, pList.get(0).Key, nHigh), new PlotMark(1, pList.get(1).Key, nLow));
                int x1 = rect.left;
                int y1 = rect.top;
                int x2 = rect.right;
                int y2 = rect.bottom;
                //画矩形
                if (rect.right-rect.left > 0 && rect.bottom-rect.top > 0)
                {
                    int rwidth = rect.right - rect.left;
                    int rheight = rect.bottom - rect.top;
                    drawRect(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x1 + rwidth, y1 + rheight);
                    //画幅度文字
                    int count = Math.Abs(bIndex - eIndex) + 1;
                    drawText(paint, "COUNT:" + count.ToString(), lineColor, m_font, x1 + 2, y1 + 2);
                    double diff = nLow - nHigh;
                    double range = 0;
                    if (nHigh != 0)
                    {
                        range = diff / nHigh;
                    }
                    String diffString = diff.ToString("0.00");
                    String rangeString = range.ToString("0.00%");
                    FCSize rangeSize = textSize(paint, rangeString, m_font);
                    drawText(paint, diffString, lineColor, m_font, x1 + rwidth + 2, y1 + 2);
                    drawText(paint, rangeString, lineColor, m_font, x1 + rwidth + 2, y1 + rheight - rangeSize.cy - 2);
                    //画平均值
                    if (m_sourceFields != null && m_sourceFields.containsKey("CLOSE"))
                    {
                        double[] array = m_dataSource.DATA_ARRAY(m_sourceFields.get("CLOSE"), eIndex, eIndex - bIndex + 1);
                        double avg = FCScript.avgValue(array, array.Length);
                        float yAvg = py(avg);
                        drawLine(paint, lineColor, m_lineWidth, 1, x1, yAvg, x2, yAvg);
                        String avgString = "AVG:" + avg.ToString("0.00");
                        FCSize avgSize = textSize(paint, avgString, m_font);
                        drawText(paint, avgString, lineColor, m_font, x1 + 2, yAvg - avgSize.cy - 2);
                    }
                }
                //画选中点
                if (Selected)
                {
                    drawSelect(paint, lineColor, x1, y1);
                    drawSelect(paint, lineColor, x2, y2);
                }
            }
        }
    }

    /// <summary>
    /// 圆
    /// </summary>
    public class Circle : FCPlot
    {
        /// <summary>
        /// 创建圆
        /// </summary>
        public Circle()
        {
            PlotType = "CIRCLE";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            FCPoint mp = getTouchOverPoint();
            //获取参数
            float y1 = py(m_marks.get(0).Value);
            float y2 = py(m_marks.get(1).Value);
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            if (selectPoint(mp, x1, y1))
            {
                action = ActionType.AT1;
            }
            else if (selectPoint(mp, x2, y2))
            {
                action = ActionType.AT2;
            }
            else
            {
                float r = (float)(Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1)));
                FCPoint p = new FCPoint(mp.x - x1, mp.y - y1);
                float round = p.x * p.x + p.y * p.y;
                if (round / (r * r) >= 0.9 && round / (r * r) <= 1.1)
                {
                    action = ActionType.MOVE;
                    return action;
                }
            }
            return action;
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return create2PointsA(mp);
        }
        
        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.MOVE)
                {
                    Cursor = FCCursors.Hand;
                }
                else
                {
                    Cursor = FCCursors.SizeWE;
                }
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取相对位置
            float y1 = py(pList.get(0).Value);
            float y2 = py(pList.get(1).Value);
            int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(pList.get(1).Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            float r = (float)Math.Sqrt(Math.Abs((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1)));
            drawEllipse(paint, lineColor, m_lineWidth, m_lineStyle, x1 - r, y1 - r, x1 + r, y1 + r);
            //画选中点
            if (Selected)
            {
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
                drawSelect(paint, lineColor, x1, y1);
                drawSelect(paint, lineColor, x2, y2);
            }
        }
    }

    /// <summary>
    /// 外接圆
    /// </summary>
    public sealed class CircumCircle : FCPlot
    {
        /// <summary>
        /// 创建外接圆
        /// </summary>
        public CircumCircle()
        {
            PlotType = "CIRCUMCIRCLE";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            FCPoint mp = getTouchOverPoint();
            //获取参数
            float y1 = py(m_marks.get(0).Value);
            float y2 = py(m_marks.get(1).Value);
            float y3 = py(m_marks.get(2).Value);
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
            int pIndex = m_dataSource.getRowIndex(m_marks.get(2).Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            float x3 = px(pIndex);
            float Ox = 0, Oy = 0, r = 0;
            //获取圆的参数
            FCPlot.ellipseOR(x1, y1, x2, y2, x3, y3, ref Ox, ref Oy, ref r);
            float clickX = mp.x - Ox;
            float clickY = mp.y - Oy;
            double ellipseValue = clickX * clickX + clickY * clickY;
            //模糊选中
            if (ellipseValue >= r * r * 0.8 && ellipseValue <= r * r * 1.2)
            {
                if (selectPoint(mp, x1, y1))
                {
                    action = ActionType.AT1;
                }
                else if (selectPoint(mp, x2, y2))
                {
                    action = ActionType.AT2;
                }
                else if (selectPoint(mp, x3, y3))
                {
                    action = ActionType.AT3;
                }
                else
                {
                    action = ActionType.MOVE;
                }
            }
            return action;
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return create3Points(mp);
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.MOVE)
                {
                    Cursor = FCCursors.Hand;
                }
                else
                {
                    Cursor = FCCursors.SizeWE;
                }
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
                m_startMarks.put(2, m_marks.get(2));
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取相对位置
            float y1 = py(pList.get(0).Value);
            float y2 = py(pList.get(1).Value);
            float y3 = py(pList.get(2).Value);
            int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(pList.get(1).Key);
            int pIndex = m_dataSource.getRowIndex(pList.get(2).Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            float x3 = px(pIndex);
            float Ox = 0, Oy = 0, r = 0;
            //获取圆的参数
            FCPlot.ellipseOR(x1, y1, x2, y2, x3, y3, ref Ox, ref Oy, ref r);
            drawEllipse(paint, lineColor, m_lineWidth, m_lineStyle, Ox - r, Oy - r, Ox + r, Oy + r);
            //画选中点
            if (Selected)
            {
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x2, y2, x3, y3);
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x3, y3, x1, y1);
                drawSelect(paint, lineColor, x1, y1);
                drawSelect(paint, lineColor, x2, y2);
                drawSelect(paint, lineColor, x3, y3);
            }
        }
    }

    /// <summary>
    /// 下跌箭头
    /// </summary>
    public sealed class DownArrow : FCPlot
    {
        /// <summary>
        /// 创建下跌箭头
        /// </summary>
        public DownArrow()
        {
            PlotType = "DOWNARROW";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            //获取值和索引
            double fValue = m_marks.get(0).Value;
            int aIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            //获取横坐标和纵坐标
            float x1 = px(aIndex);
            float y1 = py(fValue);
            //获取点击区域
            int width = 10;
            FCRect rect = new FCRect(x1 - width / 2, y1 - width * 3 / 2, x1 + width / 2, y1);
            FCPoint mp = getTouchOverPoint();
            //判断是否选中
            if (mp.x >= rect.left && mp.x <= rect.right && mp.y >= rect.top && mp.y <= rect.bottom)
            {
                action = ActionType.MOVE;
            }
            return action;
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return createPoint(mp);
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                Cursor = FCCursors.Hand;
                m_startMarks.put(0, m_marks.get(0));
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            double fValue = pList.get(0).Value;
            //获取位置和索引
            int aIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            //获取内部横坐标和纵坐标
            int x1 = (int)px(aIndex);
            int y1 = (int)py(fValue);
            int width = 10;
            FCPoint point1 = new FCPoint(x1, y1);
            FCPoint point2 = new FCPoint(x1 + width / 2, y1 - width);
            FCPoint point3 = new FCPoint(x1 + width / 4, y1 - width);
            FCPoint point4 = new FCPoint(x1 + width / 4, y1 - width * 3 / 2);
            FCPoint point5 = new FCPoint(x1 - width / 4, y1 - width * 3 / 2);
            FCPoint point6 = new FCPoint(x1 - width / 4, y1 - width);
            FCPoint point7 = new FCPoint(x1 - width / 2, y1 - width);
            FCPoint[] points = new FCPoint[7] { point1, point2, point3, point4, point5, point6, point7 };
            fillPolygon(paint, lineColor, points);
            if (Selected)
            {
                drawSelect(paint, lineColor, x1 - width / 2, y1 - width * 3 / 2);
            }
        }
    }

    /// <summary>
    /// 下降45度线
    /// </summary>
    public sealed class Dropline : FCPlot
    {
        /// <summary>
        /// 创建下降45度线
        /// </summary>
        public Dropline()
        {
            PlotType = "DROPLINE";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            FCPoint mp = getTouchOverPoint();
            //获取参数
            float[] param = getDropLineParams(m_marks);
            //判断选中
            if (param != null)
            {
                if (mp.y - param[0] * mp.x - param[1] >= m_lineWidth * -5 && mp.y - param[0] * mp.x - param[1] <= m_lineWidth * 5)
                {
                    action = ActionType.MOVE;
                }
            }
            return action;
        }

        /// <summary>
        /// 获取直线的参数
        /// </summary>
        /// <param name="pList">点阵集合</param>
        /// <returns></returns>
        private float[] getDropLineParams(HashMap<int, PlotMark> pList)
        {
            if (pList.size() == 0)
            {
                return null;
            }
            float y1 = py(pList.get(0).Value);
            int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            float x1 = px(bIndex);
            float a = 1;
            float b = y1 - x1;
            return new float[] { a, b };
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return createPoint(mp);
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                Cursor = FCCursors.Hand;
                m_startMarks.put(0, m_marks.get(0));
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取参数
            float[] param = getDropLineParams(pList);
            float a = param[0];
            float b = param[1];
            float leftX = 0;
            float leftY = leftX * a + b;
            float rightX = WorkingAreaWidth;
            float rightY = rightX * a + b;
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, leftX, leftY, rightX, rightY);
        }
    }

    /// <summary>
    /// 椭圆
    /// </summary>
    public sealed class Ellipse : FCPlot
    {
        /// <summary>
        /// 创建椭圆
        /// </summary>
        public Ellipse()
        {
            PlotType = "ELLIPSE";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            FCPoint mp = getTouchOverPoint();
            //获取线条的点阵描述
            double fValue = m_marks.get(0).Value;
            double eValue = m_marks.get(1).Value;
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
            //获取坐标
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            float y1 = py(fValue);
            float y2 = py(eValue);
            float x = 0;
            float y = 0;
            if (x1 >= x2)
            {
                x = x2;
            }
            else
            {
                x = x2 - (x2 - x1) * 2;
            }
            if (y1 >= y2)
            {
                y = y1 - (y1 - y2) * 2;
            }
            else
            {
                y = y1;
            }
            //上下缩放
            if (selectPoint(mp, x1, y1))
            {
                action = ActionType.AT1;
                return action;
            }
            //左右缩放
            else if (selectPoint(mp, x2, y2))
            {
                action = ActionType.AT2;
                return action;
            }
            //获取椭圆的几何属性
            float width = Math.Abs((x1 - x2) * 2);
            float height = Math.Abs((y1 - y2) * 2);
            float oX = x + width / 2;
            float oY = y + height / 2;
            float a = 0;
            float b = 0;
            //计算参数
            FCPlot.ellipseAB(width, height, ref a, ref b);
            //当椭圆是一个点的时候
            if (a != 0 && b != 0)
            {
                float clickX = mp.x - oX;
                float clickY = mp.y - oY;
                double ellipseValue = clickX * clickX / (a * a) + clickY * clickY / (b * b);
                //模糊选中
                if (ellipseValue >= 0.8 && ellipseValue <= 1.2)
                {
                    action = ActionType.MOVE;
                }
            }
            return action;
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return create2PointsB(mp);
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.AT1)
                {
                    Cursor = FCCursors.SizeNS;
                }
                else if (m_action == ActionType.AT2)
                {
                    Cursor = FCCursors.SizeWE;
                }
                else
                {
                    Cursor = FCCursors.Hand;
                }
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取点阵的值及索引
            double fValue = pList.get(0).Value;
            double eValue = pList.get(1).Value;
            int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(pList.get(1).Key);
            //获取内部位置
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            float y1 = py(fValue);
            float y2 = py(eValue);
            float x = x1 - (x1 - x2);
            float y = 0;
            float width = (x1 - x2) * 2;
            float height = 0;
            if (y1 >= y2)
                height = (y1 - y2) * 2;
            else
                height = (y2 - y1) * 2;
            y = y2 - height / 2;
            if (width == 0)
                width = 1;
            if (height == 0)
                height = 1;
            if (width == 1 && height == 1)
            {
                drawEllipse(paint, lineColor, m_lineWidth, m_lineStyle, x - 2, y - 2, x + 2, y + 2);
            }
            else
            {
                drawEllipse(paint, lineColor, m_lineWidth, m_lineStyle, x, y, x + width, y + height);
            }
            //画选中点
            if (Selected)
            {
                drawSelect(paint, lineColor, x1, y1);
                drawSelect(paint, lineColor, x2, y2);
            }
        }
    }

    /// <summary>
    /// 斐波圆
    /// </summary>
    public sealed class FiboEllipse : FCPlot
    {
        /// <summary>
        /// 创建斐波圆
        /// </summary>
        public FiboEllipse()
        {
            PlotType = "FIBOELLIPSE";
        }

        /// <summary>
        /// 斐波圆的参数
        /// </summary>
        /// <param name="pList">点阵描述集合</param>
        /// <returns>斐波圆的参数</returns>
        private float[] fibonacciEllipseParam(HashMap<int, PlotMark> pList)
        {
            if (pList.size() == 0)
            {
                return null;
            }
            int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(pList.get(1).Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            float y1 = py(pList.get(0).Value);
            float y2 = py(pList.get(1).Value);
            float r1 = (float)(Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1)));
            float r2 = r1 * 0.236f;
            float r3 = r1 * 0.382f;
            float r4 = r1 * 0.5f;
            float r5 = r1 * 0.618f;
            return new float[] { x1, y1, x2, y2, r1, r2, r3, r4, r5 };
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            //获取点的描述
            FCPoint mp = getTouchOverPoint();
            float[] param = fibonacciEllipseParam(m_marks);
            float x1 = param[0];
            float y1 = param[1];
            float x2 = param[2];
            float y2 = param[3];
            //外侧点
            if (selectPoint(mp, x1, y1) || m_moveTimes == 1)
            {
                action = ActionType.AT1;
                return action;
            }
            //内侧点
            else if (selectPoint(mp, x2, y2))
            {
                action = ActionType.AT2;
                return action;
            }
            if (selectSegment(mp, x1, y1, x2, y2))
            {
                action = ActionType.MOVE;
                return action;
            }
            //判断是否是移动四个椭圆
            FCPoint p = new FCPoint(mp.x - x1, mp.y - y1);
            float round = p.x * p.x + p.y * p.y;
            for (int i = 4; i < 9; i++)
            {
                float r = param[i];
                if (round / (r * r) >= 0.9 && round / (r * r) <= 1.1)
                {
                    action = ActionType.MOVE;
                    return action;
                }
            }
            return action;
        }

        /// <summary>
        /// 初始化图形
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return create2PointsB(mp);
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_moveTimes++;
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.MOVE)
                {
                    Cursor = FCCursors.Hand;
                }
                else
                {
                    Cursor = FCCursors.SizeNS;
                }
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
            }
        }

        /// <summary>
        /// 绘制图像的残影
        /// </summary>
        /// <param name="paint">绘图对象</param>
        public override void onPaintGhost(FCPaint paint)
        {
            if (m_moveTimes > 1)
            {
                onPaint(paint, m_startMarks, SelectedColor);
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取点阵的值及索引，然后获取内部位置　
            float[] param = fibonacciEllipseParam(pList);
            float x1 = param[0];
            float y1 = param[1];
            float x2 = param[2];
            float y2 = param[3];
            drawLine(paint, lineColor, m_lineWidth, 1, x1, y1, x2, y2);
            float r1 = param[4] >= 4 ? param[4] : 4;
            float r2 = param[5] >= 4 ? param[5] : 4;
            float r3 = param[6] >= 4 ? param[6] : 4;
            float r4 = param[7] >= 4 ? param[7] : 4;
            float r5 = param[8] >= 4 ? param[8] : 4;
            //画椭圆
            drawEllipse(paint, lineColor, m_lineWidth, m_lineStyle, x1 - r1, y1 - r1, x1 + r1, y1 + r1);
            drawEllipse(paint, lineColor, m_lineWidth, m_lineStyle, x1 - r2, y1 - r2, x1 + r2, y1 + r2);
            drawEllipse(paint, lineColor, m_lineWidth, m_lineStyle, x1 - r3, y1 - r3, x1 + r3, y1 + r3);
            drawEllipse(paint, lineColor, m_lineWidth, m_lineStyle, x1 - r4, y1 - r4, x1 + r4, y1 + r4);
            drawEllipse(paint, lineColor, m_lineWidth, m_lineStyle, x1 - r5, y1 - r5, x1 + r5, y1 + r5);
            //画选中点
            if (Selected)
            {
                drawSelect(paint, lineColor, x1, y1);
                drawSelect(paint, lineColor, x2, y2);
            }
            if (r5 > 20)
            {
                //绘制文字
                FCSize sizeF = textSize(paint, "23.6%", m_font);
                drawText(paint, "23.6%", lineColor, m_font, x1 - sizeF.cx / 2, y1 - r1 - sizeF.cy);
                sizeF = textSize(paint, "38.2%", m_font);
                drawText(paint, "38.2%", lineColor, m_font, x1 - sizeF.cx / 2, y1 - r2 - sizeF.cy);
                sizeF = textSize(paint, "50.0%", m_font);
                drawText(paint, "50.0%", lineColor, m_font, x1 - sizeF.cx / 2, y1 - r3 - sizeF.cy);
                sizeF = textSize(paint, "61.8%", m_font);
                drawText(paint, "61.8%", lineColor, m_font, x1 - sizeF.cx / 2, y1 - r4 - sizeF.cy);
                sizeF = textSize(paint, "100%", m_font);
                drawText(paint, "100%", lineColor, m_font, x1 - sizeF.cx / 2, y1 - r5 - sizeF.cy);
            }
        }
    }

    /// <summary>
    /// 斐波扇面
    /// </summary>
    public sealed class FiboFanline : FCPlot
    {
        /// <summary>
        /// 创建斐波扇面
        /// </summary>
        public FiboFanline()
        {
            PlotType = "FIBOFANLINE";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            FCPoint mp = getTouchOverPoint();
            //获取点的描述
            float y1 = py(m_marks.get(0).Value);
            float y2 = py(m_marks.get(1).Value);
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            //缩放
            if (selectPoint(mp, x1, y1) || m_moveTimes == 1)
            {
                action = ActionType.AT1;
                return action;
            }
            //缩放
            else if (selectPoint(mp, x2, y2))
            {
                action = ActionType.AT2;
                return action;
            }
            //判断选中点
            FCPointF firstP = new FCPointF(x2, y2 - (y2 - y1) * 0.382f);
            FCPointF secondP = new FCPointF(x2, y2 - (y2 - y1) * 0.5f);
            FCPointF thirdP = new FCPointF(x2, y2 - (y2 - y1) * 0.618f);
            FCPointF startP = new FCPointF(x1, y1);
            //获取直线参数
            bool selected = selectSegment(mp, x1, y1, x2, y2);
            if (selected)
            {
                action = ActionType.MOVE;
                return action;
            }
            if ((x2 > x1 && mp.x >= x1 - 2) || (mp.x <= x1 + 2 && x2 < x1))
            {
                if (selectRay(mp, startP.x, startP.y, firstP.x, firstP.y)
                    || selectRay(mp, startP.x, startP.y, secondP.x, secondP.y)
                    || selectRay(mp, startP.x, startP.y, thirdP.x, thirdP.y))
                {
                    action = ActionType.MOVE;
                    return action;
                }
            }
            return action;
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return create2PointsB(mp);
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.MOVE)
                {
                    Cursor = FCCursors.Hand;
                }
                else
                {
                    Cursor = FCCursors.SizeNS;
                }
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
            }
        }

        /// <summary>
        /// 绘制图像的残影
        /// </summary>
        /// <param name="paint">绘图对象</param>
        public override void onPaintGhost(FCPaint paint)
        {
            if (m_moveTimes > 1)
            {
                onPaint(paint, m_startMarks, SelectedColor);
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取点阵的值及索引，然后获取内部位置
            float y1 = py(pList.get(0).Value);
            float y2 = py(pList.get(1).Value);
            int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(pList.get(1).Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            drawLine(paint, lineColor, m_lineWidth, 1, x1, y1, x2, y2);
            //画选中
            if (Selected || (x1 == x2))
            {
                drawSelect(paint, lineColor, x1, y1);
                drawSelect(paint, lineColor, x2, y2);
            }
            if (x1 != x2 && y1 != y2)
            {
                FCPointF firstP = new FCPointF(x2, y2 - (y2 - y1) * 0.382f);
                FCPointF secondP = new FCPointF(x2, y2 - (y2 - y1) * 0.5f);
                FCPointF thirdP = new FCPointF(x2, y2 - (y2 - y1) * 0.618f);
                FCPointF startP = new FCPointF(x1, y1);
                ArrayList<FCPointF> listP = new ArrayList<FCPointF>();
                listP.AddRange(new FCPointF[] { firstP, secondP, thirdP });
                int listSize = listP.size();
                for (int i = 0; i < listSize; i++)
                {
                    //获取直线参数
                    float k = 0;
                    float b = 0;
                    lineXY(startP.x, startP.y, listP[i].x, listP[i].y, 0, 0, ref k, ref b);
                    float newX = 0;
                    float newY = 0;
                    if (x2 > x1)
                    {
                        newY = k * WorkingAreaWidth + b;
                        newX = WorkingAreaWidth;
                    }
                    else
                    {
                        newY = b;
                        newX = 0;
                    }
                    drawLine(paint, lineColor, m_lineWidth, m_lineStyle, startP.x, startP.y, newX, newY);
                }
            }
        }
    }

    /// <summary>
    /// 斐波周期线
    /// </summary>
    public sealed class FiboTimezone : FCPlot
    {
        /// <summary>
        /// 创建斐波周期线
        /// </summary>
        public FiboTimezone()
        {
            PlotType = "FIBOTIMEZONE";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            FCPoint mp = getTouchOverPoint();
            //获取参数
            int[] param = getFibonacciTimeZonesParam(m_marks);
            for (int i = 0; i < param.Length; i++)
            {
                //判断选中
                int rI = (int)param[i];
                FCChart chart = Chart;
                if (rI >= chart.FirstVisibleIndex && rI <= chart.LastVisibleIndex)
                {
                    float x1 = px(rI);
                    if (mp.x >= x1 - m_lineWidth * 2.5f && mp.x <= x1 + m_lineWidth * 2.5f)
                    {
                        action = ActionType.MOVE;
                        return action;
                    }
                }
            }
            return action;
        }

        /// <summary>
        /// 获取斐波周期线的参数
        /// </summary>
        /// <param name="pList">点阵描述</param>
        /// <returns>斐波周期线的参数</returns>
        private int[] getFibonacciTimeZonesParam(HashMap<int, PlotMark> pList)
        {
            if (pList.size() == 0)
            {
                return null;
            }
            double fValue = pList.get(0).Value;
            int aIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            int pos = 1;
            int fibonacciValue = 1;
            ArrayList<int> fValueList = new ArrayList<int>();
            fValueList.add(aIndex);
            FCChart chart = Chart;
            while (aIndex + fibonacciValue <= chart.LastVisibleIndex)
            {
                fibonacciValue = FCScript.fibonacciValue(pos);
                fValueList.add(aIndex + fibonacciValue);
                pos++;
            }
            return fValueList.ToArray();
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return createPoint(mp);
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                Cursor = FCCursors.Hand;
                m_startMarks.put(0, m_marks.get(0));
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取点阵的值及索引，然后获取内部位置
            int[] param = getFibonacciTimeZonesParam(pList);
            FCChart chart = Chart;
            for (int i = 0; i < param.Length; i++)
            {
                int rI = (int)param[i];
                if (rI >= chart.FirstVisibleIndex && rI <= chart.LastVisibleIndex)
                {
                    float x1 = px(rI);
                    //画线
                    drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, 0, x1, WorkingAreaHeight);
                    //画选中
                    if (i == 0 && Selected)
                    {
                        drawSelect(paint, lineColor, x1, WorkingAreaHeight / 2);
                    }
                    int fValue = FCScript.fibonacciValue(i);
                    //画文字
                    drawText(paint, fValue.ToString(), lineColor, m_font, x1, 0);
                }
            }
        }
    }

    /// <summary>
    /// 甘氏箱
    /// </summary>
    public sealed class GannBox : FCPlot
    {
        /// <summary>
        /// 创建甘氏箱
        /// </summary>
        public GannBox()
        {
            PlotType = "GANNBOX";
        }

        /// <summary>
        /// 对面点的坐标
        /// </summary>
        private FCPoint oppositePoint = new FCPoint();

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            //获取动作类型
            return getClickStatus();
        }

        /// <summary>
        /// 获取选中状态
        /// </summary>
        /// <returns></returns>
        private ActionType getClickStatus()
        {
            FCPoint mp = getTouchOverPoint();
            //获取各个点的位置
            FCRect rect = getRectangle(m_marks.get(0), m_marks.get(1));
            int x1 = rect.left;
            int y1 = rect.top;
            int x2 = rect.right;
            int y2 = rect.top;
            int x3 = rect.left;
            int y3 = rect.bottom;
            int x4 = rect.right;
            int y4 = rect.bottom;
            //判断是否选中点
            if (selectPoint(mp, x1, y1))
            {
                return ActionType.AT1;
            }
            else if (selectPoint(mp, x2, y2))
            {
                return ActionType.AT2;
            }
            else if (selectPoint(mp, x3, y3))
            {
                return ActionType.AT3;
            }
            else if (selectPoint(mp, x4, y4))
            {
                return ActionType.AT4;
            }
            else
            {
                int sub = (int)(m_lineWidth * 2.5);
                //判断是否移动
                FCRect bigRect = new FCRect(rect.left - sub, rect.top - sub, rect.right + sub, rect.bottom + sub);
                if (mp.x >= bigRect.left && mp.x <= bigRect.right && mp.y >= bigRect.top && mp.y <= bigRect.bottom)
                {
                    if (rect.right - rect.left <= 4 || rect.bottom - rect.top <= 4)
                    {
                        return ActionType.MOVE;
                    }
                    else
                    {
                        FCRect smallRect = new FCRect(rect.left + sub, rect.top + sub, rect.right - sub, rect.bottom - sub);
                        if (!(mp.x >= smallRect.left && mp.x <= smallRect.right && mp.y >= smallRect.top && mp.y <= smallRect.bottom))
                        {
                            return ActionType.MOVE;
                        }
                    }
                    x1 = rect.left;
                    y1 = rect.bottom;
                    x2 = rect.right;
                    y2 = rect.top;
                    FCPoint startP = new FCPoint(x1, y1);
                    FCPoint[] listP = getGannBoxPoints(x1, y1, x2, y2);
                    bool selected;
                    for (int i = 0; i < listP.Length; i++)
                    {
                        selected = selectLine(mp, startP.x, startP.y, listP[i].x, listP[i].y);
                        if (selected) return ActionType.MOVE;
                    }
                    selected = selectLine(mp, startP.x, startP.y, x2, y2);
                    if (selected) return ActionType.MOVE;
                    x1 = rect.left;
                    y1 = rect.top;
                    x2 = rect.right;
                    y2 = rect.bottom;
                    listP = getGannBoxPoints(x1, y1, x2, y2);
                    for (int i = 0; i < listP.Length; i++)
                    {
                        selected = selectLine(mp, startP.x, startP.y, listP[i].x, listP[i].y);
                        if (selected) return ActionType.MOVE;
                    }
                    startP = new FCPoint(x1, y1);
                    selected = selectLine(mp, startP.x, startP.y, x2, y2);
                    if (selected) return ActionType.MOVE;
                }
            }
            return ActionType.NO;
        }

        /// <summary>
        /// 获取江恩箱的重要点
        /// </summary>
        /// <param name="x1">第一个点的X</param>
        /// <param name="y1">第一个点的Y</param>
        /// <param name="x2">第二个点的X</param>
        /// <param name="y2">第二个点的Y</param>
        /// <returns>江恩箱的重要点</returns>
        private FCPoint[] getGannBoxPoints(float x1, float y1, float x2, float y2)
        {
            FCPoint firstP = new FCPoint(x2, y2 - (y2 - y1) * 0.875f);
            FCPoint secondP = new FCPoint(x2, y2 - (y2 - y1) * 0.75f);
            FCPoint thirdP = new FCPoint(x2, y2 - (y2 - y1) * 0.67f);
            FCPoint forthP = new FCPoint(x2, y2 - (y2 - y1) * 0.5f);
            FCPoint fifthP = new FCPoint(x2 - (x2 - x1) * 0.875f, y2);
            FCPoint sixthP = new FCPoint(x2 - (x2 - x1) * 0.75f, y2);
            FCPoint seventhP = new FCPoint(x2 - (x2 - x1) * 0.67f, y2);
            FCPoint eighthP = new FCPoint(x2 - (x2 - x1) * 0.5f, y2);
            return new FCPoint[] { firstP, secondP, thirdP, forthP, fifthP, sixthP, seventhP, eighthP };
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return create2PointsB(mp);
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_moveTimes++;
            m_action = getAction();
            FCRect rect = getRectangle(m_marks.get(0), m_marks.get(1));
            int x1 = rect.left;
            int y1 = rect.top;
            int x2 = rect.right;
            int y2 = rect.top;
            int x3 = rect.left;
            int y3 = rect.bottom;
            int x4 = rect.right;
            int y4 = rect.bottom;
            //根据不同类型初始化点阵信息
            switch (m_action)
            {
                case ActionType.AT1:
                    oppositePoint = new FCPoint(x4, y4);
                    break;
                case ActionType.AT2:
                    oppositePoint = new FCPoint(x3, y3);
                    break;
                case ActionType.AT3:
                    oppositePoint = new FCPoint(x2, y2);
                    break;
                case ActionType.AT4:
                    oppositePoint = new FCPoint(x1, y1);
                    break;
            }
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.MOVE)
                {
                    Cursor = FCCursors.Hand;
                }
                else
                {
                    Cursor = FCCursors.SizeNS;
                }
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
            }
        }

        /// <summary>
        /// 移动线条
        /// </summary>
        public override void onMoving()
        {
            FCPoint mp = getMovingPoint();
            //根据不同类型作出动作
            switch (m_action)
            {
                case ActionType.MOVE:
                    move(mp);
                    break;
                case ActionType.AT1:
                case ActionType.AT2:
                case ActionType.AT3:
                case ActionType.AT4:
                    resize(mp, oppositePoint);
                    break;
            }
        }

        /// <summary>
        /// 绘制图像的残影
        /// </summary>
        /// <param name="paint">绘图对象</param>
        public override void onPaintGhost(FCPaint paint)
        {
            if (m_moveTimes > 1)
            {
                onPaint(paint, m_startMarks, SelectedColor);
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            FCRect rect = getRectangle(pList.get(0), pList.get(1));
            int leftVScaleWidth = Chart.LeftVScaleWidth;
            int titleHeight = Div.TitleBar.Height;
            paint.setClip(new FCRect(rect.left + leftVScaleWidth, rect.top + titleHeight, rect.right + leftVScaleWidth, rect.bottom + titleHeight));
            //画矩形
            if (rect.right - rect.left > 0 && rect.bottom - rect.top > 0)
            {
                drawRect(paint, lineColor, m_lineWidth, m_lineStyle, rect.left, rect.top, rect.right, rect.bottom);
            }
            FCPoint oP = new FCPoint(rect.left, rect.top);
            int x1 = rect.left;
            int y1 = rect.bottom;
            int x2 = rect.right;
            int y2 = rect.top;
            if (x1 != x2 && y1 != y2)
            {
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
                FCPoint startP = new FCPoint(x1, y1);
                FCPoint[] listP = getGannBoxPoints(x1, y1, x2, y2);
                float k = 0;
                float b = 0;
                for (int i = 0; i < listP.Length; i++)
                {
                    //获取直线参数
                    lineXY(startP.x, startP.y, listP[i].x, listP[i].y, 0, 0, ref k, ref b);
                    float newX = 0;
                    float newY = 0;
                    if (x2 > x1)
                    {
                        newY = k * x2 + b;
                        newX = x2;
                    }
                    else
                    {
                        newY = b;
                        newX = x1;
                    }
                    drawLine(paint, lineColor, m_lineWidth, m_lineStyle, startP.x, startP.y, newX, newY);
                }
                x1 = rect.left;
                y1 = rect.top;
                x2 = rect.right;
                y2 = rect.bottom;
                //获取重要点
                listP = getGannBoxPoints(x1, y1, x2, y2);
                startP = new FCPoint(x1, y1);
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
                for (int i = 0; i < listP.Length; i++)
                {
                    //获取直线参数
                    lineXY(startP.x, startP.y, listP[i].x, listP[i].y, 0, 0, ref k, ref b);
                    float newX = 0;
                    float newY = 0;
                    if (x2 > x1)
                    {
                        newY = k * x2 + b;
                        newX = x2;
                    }
                    else
                    {
                        newY = b;
                        newX = x1;
                    }
                    drawLine(paint, lineColor, m_lineWidth, m_lineStyle, startP.x, startP.y, newX, newY);
                }
            }
            //画选中点
            if (Selected)
            {
                drawSelect(paint, lineColor, rect.left, rect.top);
                drawSelect(paint, lineColor, rect.right, rect.top);
                drawSelect(paint, lineColor, rect.left, rect.bottom);
                drawSelect(paint, lineColor, rect.right, rect.bottom);
            }
        }
    }

    /// <summary>
    /// 甘氏线
    /// </summary>
    public sealed class GannLine : FCPlot
    {
        /// <summary>
        /// 创建甘氏线
        /// </summary>
        public GannLine()
        {
            PlotType = "GANNLINE";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            FCPoint mp = getTouchOverPoint();
            float y1 = py(m_marks.get(0).Value);
            float y2 = py(m_marks.get(1).Value);
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            if (selectPoint(mp, x1, y1))
            {
                action = ActionType.AT1;
                return action;
            }
            else if (selectPoint(mp, x2, y2))
            {
                action = ActionType.AT2;
                return action;
            }
            //判断是否选中点
            bool selected = selectSegment(mp, x1, y1, x2, y2);
            if (selected)
            {
                action = ActionType.MOVE;
                return action;
            }
            FCPointF firstP = new FCPointF(x2, y2 - (y2 - y1) * 0.875f);
            FCPointF secondP = new FCPointF(x2, y2 - (y2 - y1) * 0.75f);
            FCPointF thirdP = new FCPointF(x2, y2 - (y2 - y1) * 0.67f);
            FCPointF forthP = new FCPointF(x2, y2 - (y2 - y1) * 0.5f);
            FCPointF fifthP = new FCPointF(x2 - (x2 - x1) * 0.875f, y2);
            FCPointF sixthP = new FCPointF(x2 - (x2 - x1) * 0.75f, y2);
            FCPointF seventhP = new FCPointF(x2 - (x2 - x1) * 0.67f, y2);
            FCPointF eighthP = new FCPointF(x2 - (x2 - x1) * 0.5f, y2);
            FCPointF startP = new FCPointF(x1, y1);
            ArrayList<FCPointF> listP = new ArrayList<FCPointF>();
            listP.AddRange(new FCPointF[] { firstP, secondP, thirdP, forthP, fifthP, sixthP, seventhP, eighthP });
            if ((x2 > x1 && mp.x >= x1 - 2) || (mp.x <= x1 + 2 && x2 < x1))
            {
                int listSize = listP.size();
                for (int i = 0; i < listSize; i++)
                {
                    selected = selectLine(mp, startP.x, startP.y, listP[i].x, listP[i].y);
                    if (selected)
                    {
                        action = ActionType.MOVE;
                        return action;
                    }
                }
            }
            return action;
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return create2PointsB(mp);
        }

        /// <summary>
		/// 开始移动画线工具
		/// </summary>
		public override void onMoveStart()
		{
            m_moveTimes++;
			m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.AT1)
                {
                    Cursor = FCCursors.SizeNS;
                }
                else if (m_action == ActionType.AT2)
                {
                    Cursor = FCCursors.SizeNS;
                }
                else
                {
                    Cursor = FCCursors.Hand;
                }
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
            }
		}

        /// <summary>
        /// 绘制图像的残影
        /// </summary>
        /// <param name="paint">绘图对象</param>
        public override void onPaintGhost(FCPaint paint)
        {
            if (m_moveTimes > 1)
            {
                onPaint(paint, m_startMarks, SelectedColor);
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取相对位置
            float y1 = py(pList.get(0).Value);
            float y2 = py(pList.get(1).Value);
            int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(pList.get(1).Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            drawLine(paint, lineColor, m_lineWidth, 1, x1, y1, x2, y2);
            //画选中点
            if (Selected || (x1 == x2))
            {
                drawSelect(paint, lineColor, x1, y1);
                drawSelect(paint, lineColor, x2, y2);
            }
            //画点
            if (x1 != x2 && y1 != y2)
            {
                FCPointF firstP = new FCPointF(x2, y2 - (y2 - y1) * 0.875f);
                FCPointF secondP = new FCPointF(x2, y2 - (y2 - y1) * 0.75f);
                FCPointF thirdP = new FCPointF(x2, y2 - (y2 - y1) * 0.67f);
                FCPointF forthP = new FCPointF(x2, y2 - (y2 - y1) * 0.5f);
                FCPointF fifthP = new FCPointF(x2 - (x2 - x1) * 0.875f, y2);
                FCPointF sixthP = new FCPointF(x2 - (x2 - x1) * 0.75f, y2);
                FCPointF seventhP = new FCPointF(x2 - (x2 - x1) * 0.67f, y2);
                FCPointF eighthP = new FCPointF(x2 - (x2 - x1) * 0.5f, y2);
                FCPointF startP = new FCPointF(x1, y1);
                ArrayList<FCPointF> listP = new ArrayList<FCPointF>();
                listP.AddRange(new FCPointF[] { firstP, secondP, thirdP, forthP, fifthP, sixthP, seventhP, eighthP });
                int listSize = listP.size();
                for (int i = 0; i < listSize; i++)
                {
                    float k = 0;
                    float b = 0;
                    //获取直线参数
                    lineXY(startP.x, startP.y, listP[i].x, listP[i].y, 0, 0, ref k, ref b);
                    float newX = 0;
                    float newY = 0;
                    if (x2 > x1)
                    {
                        newY = k * WorkingAreaWidth + b;
                        newX = WorkingAreaWidth;
                    }
                    else
                    {
                        newY = b;
                        newX = 0;
                    }
                    drawLine(paint, lineColor, m_lineWidth, m_lineStyle, startP.x, startP.y, newX, newY);
                }
            }
        }
    }

    /// <summary>
    /// 黄金分割线
    /// </summary>
    public class GoldenRatio : FCPlot
    {
        /// <summary>
        /// 创建黄金分割线
        /// </summary>
        public GoldenRatio()
        {
            PlotType = "GOLDENRATIO";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            FCPoint mp = getTouchOverPoint();
            //获取点阵的位置
            float y1 = py(m_marks.get(0).Value);
            float y2 = py(m_marks.get(1).Value);
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            float x1 = px(bIndex);
            if (m_moveTimes == 1)
            {
                action = ActionType.AT1;
                return action;
            }
            if (mp.x >= x1 - m_lineWidth * 2.5f && mp.x <= x1 + m_lineWidth * 2.5f)
            {
                if (mp.y >= y1 - m_lineWidth * 2.5f && mp.y <= y1 + m_lineWidth * 2.5f)
                {
                    action = ActionType.AT1;
                    return action;
                }
                else if (mp.y >= y2 - m_lineWidth * 2.5f && mp.y <= y2 + m_lineWidth * 2.5f)
                {
                    action = ActionType.AT2;
                    return action;
                }
            }
            if (hLinesSelect(GoldenRatioParams(m_marks.get(0).Value, m_marks.get(1).Value), 6))
            {
                action = ActionType.MOVE;
            }
            return action;
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return create2PointsB(mp);
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_moveTimes++;
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.MOVE)
                {
                    Cursor = FCCursors.Hand;
                }
                else
                {
                    Cursor = FCCursors.SizeWE;
                }
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
            }
        }

        /// <summary>
        /// 移动线条
        /// </summary>
        public override void onMoving()
        {
            FCPoint mp = getMovingPoint();
            switch (m_action)
            {
                case ActionType.AT1:
                    resize(0);
                    break;
                case ActionType.AT2:
                    resize(1);
                    break;
                case ActionType.MOVE:
                    double subY = mp.y - m_startPoint.y;
                    double maxValue = m_div.getVScale(m_attachVScale).VisibleMax;
                    double minValue = m_div.getVScale(m_attachVScale).VisibleMin;
                    double yAddValue = subY / WorkingAreaHeight * (minValue - maxValue);
                    m_marks.put(0, new PlotMark(0, m_startMarks.get(0).Key, m_startMarks.get(0).Value + yAddValue));
                    m_marks.put(1, new PlotMark(1, m_startMarks.get(1).Key, m_startMarks.get(1).Value + yAddValue));
                    break;
            }
        }

        /// <summary>
        /// 绘制图像的残影
        /// </summary>
        /// <param name="paint">绘图对象</param>
        public override void onPaintGhost(FCPaint paint)
        {
            if (m_moveTimes > 1)
            {
                onPaint(paint, m_startMarks, SelectedColor);
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取相对位置
            float y1 = py(pList.get(0).Value);
            float y2 = py(pList.get(1).Value);
            int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            float x1 = px(bIndex);
            //获取直线参数
            float[] lineParam = GoldenRatioParams(pList.get(0).Value, pList.get(1).Value);
            String[] str = new String[] { "0.00%", "23.60%", "38.20%", "50.00%", "61.80%", "100.00%" };
            //画文字和线
            for (int i = 0; i < lineParam.Length; i++)
            {
                FCSize sizeF = textSize(paint, str[i], m_font);
                float yP = lineParam[i];
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, 0, yP, WorkingAreaWidth, yP);
                drawText(paint, str[i], lineColor, m_font, WorkingAreaWidth - sizeF.cx, yP - sizeF.cy);
            }
            //画选中
            if (Selected)
            {
                drawSelect(paint, lineColor, x1, y1);
                drawSelect(paint, lineColor, x1, y2);
            }
        }
    }

    /// <summary>
    /// 黄金分割目标线
    /// </summary>
    public class Ga : Triangle
    {
        /// <summary>
        /// 创建黄金分割目标线
        /// </summary>
        public Ga()
        {
            PlotType = "GA";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = base.getAction();
            if (action != ActionType.NO)
            {
                return action;
            }
            else
            {
                //选中横线
                if (hLinesSelect(getGoldenRatioAimParams(m_marks), 6))
                {
                    action = ActionType.MOVE;
                }
                return action;
            }
        }

        /// <summary>
        /// 获取黄金分割的直线参数
        /// </summary>
        /// <returns></returns>
        private float[] getGoldenRatioAimParams(HashMap<int, PlotMark> pList)
        {
            //获取相对位置
            double baseValue = pList.get(0).Value;
            if (pList.get(1).Value >= pList.get(2).Value)
            {
                return GoldenRatioParams(baseValue, baseValue + pList.get(1).Value - pList.get(2).Value);
            }
            else
            {
                return GoldenRatioParams(baseValue + pList.get(1).Value - pList.get(2).Value, baseValue);
            }
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            Cursor = FCCursors.Hand;
            m_startMarks.put(0, m_marks.get(0));
            m_startMarks.put(1, m_marks.get(1));
            m_startMarks.put(2, m_marks.get(2));
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            //获取相对位置
            float y1 = py(pList.get(0).Value);
            float y2 = py(pList.get(1).Value);
            float y3 = py(pList.get(2).Value);
            int aIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            int bIndex = m_dataSource.getRowIndex(pList.get(1).Key);
            int cIndex = m_dataSource.getRowIndex(pList.get(2).Key);
            float x1 = px(aIndex);
            float x2 = px(bIndex);
            float x3 = px(cIndex);
            //画线
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x2, y2, x3, y3);
            //获取直线参数
            float[] lineParam = getGoldenRatioAimParams(pList);
            String[] str = new String[] { "0.00%", "23.60%", "38.20%", "50.00%", "61.80%", "100.00%" };
            //画文字和线
            for (int i = 0; i < lineParam.Length; i++)
            {
                FCSize sizeF = textSize(paint, str[i], m_font);
                float yP = lineParam[i];
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, 0, yP, WorkingAreaWidth, yP);
                drawText(paint, str[i], lineColor, m_font, WorkingAreaWidth - sizeF.cx, yP - sizeF.cy);
            }
            //画选中点
            if (Selected || (x1 == x2 && x2 == x3 && y1 == y2 && y2 == y3))
            {
                drawSelect(paint, lineColor, x1, y1);
                drawSelect(paint, lineColor, x2, y2);
                drawSelect(paint, lineColor, x3, y3);
            }
        }
    }

    /// <summary>
    /// 黄金分割价位线
    /// </summary>
    public class Gp : FCPlot
    {
        /// <summary>
        /// 创建黄金分割价位线
        /// </summary>
        public Gp()
        {
            PlotType = "GP";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            FCPoint mp = getTouchOverPoint();
            //获取点阵的位置
            float y1 = py(m_marks.get(0).Value);
            if (mp.y >= y1 - m_lineWidth * 2.5f && mp.y <= y1 + m_lineWidth * 2.5f)
            {
                action = ActionType.MOVE;
            }
            else
            {
                //黄金分割位
                double[] list = new double[] { 0.236, 0.382, 0.5, 0.618, 0.819, 1.191, 1.382, 1.6180, 2, 2.382, 2.618 };
                for (int i = 0; i < list.Length; i++)
                {
                    //获取坐标
                    float yP = py(list[i] * m_marks.get(0).Value);
                    if (mp.y >= yP - m_lineWidth * 2.5f && mp.y <= yP + m_lineWidth * 2.5f)
                    {
                        action = ActionType.MOVE;
                        break;
                    }
                }
            }
            return action;
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return createPoint(mp);
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.MOVE)
                {
                    Cursor = FCCursors.Hand;
                }
                else
                {
                    Cursor = FCCursors.SizeWE;
                }
                m_startMarks.put(0, m_marks.get(0));
            }
        }

        /// <summary>
        /// 移动线条
        /// </summary>
        public override void onMoving()
        {
            FCPoint mp = getMovingPoint();
            switch (m_action)
            {
                case ActionType.MOVE:
                    move(mp);
                    break;
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取相对位置
            float y1 = py(pList.get(0).Value);
            int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            float x1 = px(bIndex);
            //画线
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, 0, y1, WorkingAreaWidth, y1);
            //黄金分割位
            double[] list = new double[] { 0.236, 0.382, 0.5, 0.618, 0.819, 1.191, 1.382, 1.6180, 2, 2.382, 2.618 };
            for (int i = 0; i < list.Length; i++)
            {
                //获取坐标
                float yP = py(list[i] * pList.get(0).Value);
                String str = (list[i] * 100).ToString() + "%";
                FCSize sizeF = textSize(paint, str, m_font);
                drawLine(paint, lineColor, m_lineWidth, 1, 0, yP, WorkingAreaWidth, yP);
                drawText(paint, str, lineColor, m_font, WorkingAreaWidth - sizeF.cx, yP - sizeF.cy);
            }
            //画选中
            if (Selected)
            {
                drawSelect(paint, lineColor, x1, y1);
            }
        }
    }

    /// <summary>
    /// 水平线
    /// </summary>
    public sealed class HLine : FCPlot
    {
        /// <summary>
        /// 创建水平线
        /// </summary>
        public HLine()
        {
            PlotType = "HLINE";
        }

        /// <summary>
		/// 获取动作类型
		/// </summary>
		/// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            FCPoint mp = getTouchOverPoint();
            float y1 = py(m_marks.get(0).Value);
            if (mp.y >= y1 - m_lineWidth * 2.5f && mp.y <= y1 + m_lineWidth * 2.5f)
            {
                action = ActionType.MOVE;
            }
            return action;
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            int rIndex = m_dataSource.RowsCount;
            if (rIndex > 0)
            {
                m_marks.clear();
                double y = getNumberValue(mp);
                m_marks.put(0, new PlotMark(0, 0, y));
                return true;
            }
            return false;
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                Cursor = FCCursors.Hand;
                m_startMarks.put(0, m_marks.get(0));
            }
        }

        /// <summary>
        /// 移动
        /// </summary>
        public override void onMoving()
        {
            FCPoint mp = getMovingPoint();
            m_marks.put(0, new PlotMark(0, 0, getNumberValue(mp)));
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取直线位置
            float y1 = py(pList.get(0).Value);
            //画直线
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, 0, y1, WorkingAreaWidth, y1);
        }
    }

    /// <summary>
    /// 高低推档
    /// </summary>
    public sealed class LevelGrading : FCPlot
    {
        /// <summary>
        /// 创建高低推档
        /// </summary>
        public LevelGrading()
        {
            PlotType = "LEVELGRADING";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            FCPoint mp = getTouchOverPoint();
            //获取点阵的位置
            float y1 = py(m_marks.get(0).Value);
            float y2 = py(m_marks.get(1).Value);
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            float x1 = px(bIndex);
            if (m_moveTimes == 1)
            {
                action = ActionType.AT1;
                return action;
            }
            if (mp.x >= x1 - m_lineWidth * 2.5f && mp.x <= x1 + m_lineWidth * 2.5f)
            {
                if (mp.y >= y1 - m_lineWidth * 2.5f && mp.y <= y1 + m_lineWidth * 2.5f)
                {
                    action = ActionType.AT1;
                    return action;
                }
                else if (mp.y >= y2 - m_lineWidth * 2.5f && mp.y <= y2 + m_lineWidth * 2.5f)
                {
                    action = ActionType.AT2;
                    return action;
                }
            }
            if (hLinesSelect(levelGradingParams(m_marks.get(0).Value, m_marks.get(1).Value), 5))
            {
                action = ActionType.MOVE;
            }
            return action;
        }

        /// <summary>
        /// 获取高低推档的直线参数
        /// </summary>
        /// <param name="value1">值1</param>
        /// <param name="value2">值2</param>
        /// <returns>高低推档的直线参数</returns>
        private float[] levelGradingParams(double value1, double value2)
        {
            float y1 = py(value1);
            float y2 = py(value2);
            float yA = 0, yB = 0, yC = 0, yD = 0, yE = 0;
            yA = y1;
            yB = y2;
            yC = y1 <= y2 ? y2 + (y2 - y1) * 0.382f : y2 - (y1 - y2) * 0.382f;
            yD = y1 <= y2 ? y2 + (y2 - y1) * 0.618f : y2 - (y1 - y2) * 0.618f;
            yE = y1 <= y2 ? y2 + (y2 - y1) : y2 - (y1 - y2);
            return new float[] { yA, yB, yC, yD, yE };
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return create2PointsB(mp);
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_moveTimes++;
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.MOVE)
                {
                    Cursor = FCCursors.Hand;
                }
                else
                {
                    Cursor = FCCursors.SizeWE;
                }
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
            }
        }

        /// <summary>
        /// 移动线条
        /// </summary>
        public override void onMoving()
        {
            FCPoint mp = getMovingPoint();
            switch (m_action)
            {
                case ActionType.AT1:
                    resize(0);
                    break;
                case ActionType.AT2:
                    resize(1);
                    break;
                case ActionType.MOVE:
                    double subY = mp.y - m_startPoint.y;
                    double maxValue = m_div.getVScale(m_attachVScale).VisibleMax;
                    double minValue = m_div.getVScale(m_attachVScale).VisibleMin;
                    double yAddValue = subY / WorkingAreaHeight * (minValue - maxValue);
                    m_marks.put(0, new PlotMark(0, m_startMarks.get(0).Key, m_startMarks.get(0).Value + yAddValue));
                    m_marks.put(1, new PlotMark(1, m_startMarks.get(1).Key, m_startMarks.get(1).Value + yAddValue));
                    break;
            }
        }

        /// <summary>
        /// 绘制图像的残影
        /// </summary>
        /// <param name="paint">绘图对象</param>
        public override void onPaintGhost(FCPaint paint)
        {
            if (m_moveTimes > 1)
            {
                onPaint(paint, m_startMarks, SelectedColor);
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取相对位置
            float y1 = py(pList.get(0).Value);
            float y2 = py(pList.get(1).Value);
            int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            float x1 = px(bIndex);
            //获取直线参数
            float[] lineParam = levelGradingParams(pList.get(0).Value, pList.get(1).Value);
            String[] str = null;
            if (y1 >= y2)
            {
                str = new String[] { "-100%", "0.00%", "38.20%", "61.80%", "100%" };
            }
            else
            {
                str = new String[] { "100%", "0.00%", "-38.20%", "-61.80%", "-100%" };
            }
            //画文字和线
            for (int i = 0; i < lineParam.Length; i++)
            {
                FCSize sizeF = textSize(paint, str[i], m_font);
                float yP = lineParam[i];
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, 0, yP, WorkingAreaWidth, yP);
                drawText(paint, str[i], lineColor, m_font, WorkingAreaWidth - sizeF.cx, yP - sizeF.cy);
            }
            //画选中
            if (Selected)
            {
                drawSelect(paint, lineColor, x1, y1);
                drawSelect(paint, lineColor, x1, y2);
            }
        }
    }

    /// <summary>
    /// 直线
    /// </summary>
    public sealed class Line : FCPlot
    {
        /// <summary>
        /// 创建直线
        /// </summary>
        public Line()
        {
            PlotType = "LINE";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            FCPoint mp = getTouchOverPoint();
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
            float y1 = py(m_marks.get(0).Value);
            float y2 = py(m_marks.get(1).Value);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            if (selectPoint(mp, x1, y1))
            {
                action = ActionType.AT1;
            }
            else if (selectPoint(mp, x2, y2))
            {
                action = ActionType.AT2;
            }
            else if (selectLine(mp, x1, y1, x2, y2))
            {
                action = ActionType.MOVE;
            }
            return action;
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return create2PointsA(mp);
        }

        /// <summary>
		/// 开始移动画线工具
		/// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.MOVE)
                {
                    Cursor = FCCursors.Hand;
                }
                else
                {
                    Cursor = FCCursors.SizeWE;
                }
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取相对位置
            float y1 = py(pList.get(0).Value);
            float y2 = py(pList.get(1).Value);
            int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(pList.get(1).Key);
            //获取参数
            float[] param = getLineParams(pList.get(0), pList.get(1));
            float a = 0;
            float b = 0;
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            if (param != null)
            {
                a = param[0];
                b = param[1];
                float leftX = 0;
                float leftY = leftX * a + b;
                float rightX = WorkingAreaWidth;
                float rightY = rightX * a + b;
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, leftX, leftY, rightX, rightY);
            }
            else
            {
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, 0, x1, WorkingAreaHeight);
            }
            //画选中点
            if (Selected)
            {
                drawSelect(paint, lineColor, x1, y1);
                drawSelect(paint, lineColor, x2, y2);
            }
        }
    }

    /// <summary>
    /// 线性回归带
    /// </summary>
    public sealed class LrBand : FCPlot
    {
        /// <summary>
        /// 创建线性回归带
        /// </summary>
        public LrBand()
        {
            PlotType = "LRBAND";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            FCPoint mp = getTouchOverPoint();
            //获取点的位置
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            //获取参数
            float[] param = getLRParams(m_marks);
            if (param != null)
            {
                float a = param[0];
                float b = param[1];
                double leftValue = a + b;
                double rightValue = (eIndex - bIndex + 1) * a + b;
                float y1 = py(leftValue);
                float y2 = py(rightValue);
                //判断不同的动作类型
                if (selectPoint(mp, x1, y1))
                {
                    action = ActionType.AT1;
                    return action;
                }
                else if (selectPoint(mp, x2, y2))
                {
                    action = ActionType.AT2;
                    return action;
                }
                FCChart chart = Chart;
                int touchIndex = chart.getTouchOverIndex();
                //判断选中
                if (touchIndex >= bIndex && touchIndex <= eIndex)
                {
                    double yValue = a * ((touchIndex - bIndex) + 1) + b;
                    float y = py(yValue);
                    float x = px(touchIndex);
                    if (mp.x >= x - 5 && mp.x <= x + 5 && mp.y >= y - 5 && mp.y <= y + 5)
                    {
                        action = ActionType.MOVE;
                        return action;
                    }
                    double[] parallel = getLRBandRange(m_marks, param);
                    yValue = a * ((touchIndex - bIndex) + 1) + b + parallel[0];
                    y = py(yValue);
                    x = px(touchIndex);
                    if (mp.x >= x - 5 && mp.x <= x + 5 && mp.y >= y - 5 && mp.y <= y + 5)
                    {
                        action = ActionType.MOVE;
                        return action;
                    }
                    yValue = a * ((touchIndex - bIndex) + 1) + b - parallel[1];
                    y = py(yValue);
                    x = px(touchIndex);
                    if (mp.x >= x - 5 && mp.x <= x + 5 && mp.y >= y - 5 && mp.y <= y + 5)
                    {
                        action = ActionType.MOVE;
                        return action;
                    }
                }
            }
            return action;
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return create2CandlePoints(mp);
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.MOVE)
                {
                    Cursor = FCCursors.Hand;
                }
                else
                {
                    Cursor = FCCursors.SizeWE;
                }
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
            }
        }

        /// <summary>
        /// 移动线条
        /// </summary>
        public override void onMoving()
        {
            FCPoint mp = getMovingPoint();
            //获取当前的索引和y值
            FCChart chart = Chart;
            int touchIndex = chart.getTouchOverIndex();
            if (touchIndex < 0)
            {
                touchIndex = 0;
            }
            if (touchIndex > chart.LastVisibleIndex)
            {
                touchIndex = chart.LastVisibleIndex;
            }
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
            //根据不同类型作出动作
            switch (m_action)
            {
                case ActionType.MOVE:
                    move(mp);
                    break;
                case ActionType.AT1:
                    if (touchIndex < eIndex)
                    {
                        resize(0);
                    }
                    break;
                case ActionType.AT2:
                    if (touchIndex > bIndex)
                    {
                        resize(1);
                    }
                    break;
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取直线参数
            float[] param = getLRParams(pList);
            if (param != null)
            {
                //获取相对位置
                int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
                int eIndex = m_dataSource.getRowIndex(pList.get(1).Key);
                float x1 = px(bIndex);
                float x2 = px(eIndex);
                float a = param[0];
                float b = param[1];
                double leftValue = a + b;
                double rightValue = (eIndex - bIndex + 1) * a + b;
                //获取相对y
                float y1 = py(leftValue);
                float y2 = py(rightValue);
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
                double[] parallel = getLRBandRange(pList, param);
                double leftTop = leftValue + parallel[0];
                double rightTop = rightValue + parallel[0];
                double leftBottom = leftValue - parallel[1];
                double rightBottom = rightValue - parallel[1];
                float leftTopY = py(leftTop);
                float rightTopY = py(rightTop);
                float leftBottomY = py(leftBottom);
                float rightBottomY = py(rightBottom);
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, leftTopY, x2, rightTopY);
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, leftBottomY, x2, rightBottomY);
                //画选中点
                if (Selected)
                {
                    drawSelect(paint, lineColor, x1, y1);
                    drawSelect(paint, lineColor, x2, y2);
                }
            }
        }
    }

    /// <summary>
    /// 回归通道
    /// </summary>
    public sealed class LrChannel : FCPlot
    {
        /// <summary>
        /// 创建回归通道
        /// </summary>
        public LrChannel()
        {
            PlotType = "LRCHANNEL";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            FCPoint mp = getTouchOverPoint();
            //获取点的位置
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            //获取参数
            float[] param = getLRParams(m_marks);
            if (param != null)
            {
                float a = param[0];
                float b = param[1];
                double leftValue = a + b;
                double rightValue = (eIndex - bIndex + 1) * a + b;
                float y1 = py(leftValue);
                float y2 = py(rightValue);
                if (selectPoint(mp, x1, y1))
                {
                    action = ActionType.AT1;
                    return action;
                }
                else if (selectPoint(mp, x2, y2))
                {
                    action = ActionType.AT2;
                    return action;
                }
                FCChart chart = Chart;
                int touchIndex = chart.getTouchOverIndex();
                if (touchIndex >= bIndex && touchIndex <= chart.LastVisibleIndex)
                {
                    //判断选中
                    double yValue = a * ((touchIndex - bIndex) + 1) + b;
                    float y = py(yValue);
                    float x = px(touchIndex);
                    if (selectPoint(mp, x, y))
                    {
                        action = ActionType.MOVE;
                        return action;
                    }
                    double[] parallel = getLRBandRange(m_marks, param);
                    yValue = a * ((touchIndex - bIndex) + 1) + b + parallel[0];
                    y = py(yValue);
                    x = px(touchIndex);
                    if (selectPoint(mp, x, y))
                    {
                        action = ActionType.MOVE;
                        return action;
                    }
                    yValue = a * ((touchIndex - bIndex) + 1) + b - parallel[1];
                    y = py(yValue);
                    x = px(touchIndex);
                    if (selectPoint(mp, x, y))
                    {
                        action = ActionType.MOVE;
                        return action;
                    }
                }
            }
            return action;
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return create2CandlePoints(mp);
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.MOVE)
                {
                    Cursor = FCCursors.Hand;
                }
                else
                {
                    Cursor = FCCursors.SizeWE;
                }
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
            }
        }

        /// <summary>
        /// 移动线条
        /// </summary>
        public override void onMoving()
        {
            FCPoint mp = getMovingPoint();
            //获取当前的索引和y值
            FCChart chart = Chart;
            int touchIndex = chart.getTouchOverIndex();
            if (touchIndex < 0)
            {
                touchIndex = 0;
            }
            if (touchIndex > chart.LastVisibleIndex)
            {
                touchIndex = chart.LastVisibleIndex;
            }
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
            //根据不同类型作出动作
            switch (m_action)
            {
                case ActionType.MOVE:
                    move(mp);
                    break;
                case ActionType.AT1:
                    if (touchIndex < eIndex)
                    {
                        resize(0);
                    }
                    break;
                case ActionType.AT2:
                    if (touchIndex > bIndex)
                    {
                        resize(1);
                    }
                    break;
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取直线参数
            float[] param = getLRParams(pList);
            if (param != null)
            {
                //获取相对位置
                int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
                int eIndex = m_dataSource.getRowIndex(pList.get(1).Key);
                float x1 = px(bIndex);
                float x2 = px(eIndex);
                //画线条和选中点
                float a = param[0];
                float b = param[1];
                double leftValue = a + b;
                double rightValue = (eIndex - bIndex + 1) * a + b;
                float y1 = py(leftValue);
                float y2 = py(rightValue);
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
                double[] parallel = getLRBandRange(pList, param);
                double leftTop = leftValue + parallel[0];
                double rightTop = rightValue + parallel[0];
                double leftBottom = leftValue - parallel[1];
                double rightBottom = rightValue - parallel[1];
                float leftTopY = py(leftTop);
                float rightTopY = py(rightTop);
                float leftBottomY = py(leftBottom);
                float rightBottomY = py(rightBottom);
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, leftTopY, x2, rightTopY);
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, leftBottomY, x2, rightBottomY);
                FCChart chart = Chart;
                rightValue = (chart.LastVisibleIndex + 1 - bIndex) * a + b;
                float x3 = (float)((chart.LastVisibleIndex - chart.FirstVisibleIndex + 1) * chart.HScalePixel + chart.HScalePixel / 2);
                double dashTop = rightValue + parallel[0];
                double dashBottom = rightValue - parallel[1];
                float mValueY = py(rightValue);
                float dashTopY = py(dashTop);
                float dashBottomY = py(dashBottom);
                drawLine(paint, lineColor, m_lineWidth, 1, x2, rightTopY, x3, dashTopY);
                drawLine(paint, lineColor, m_lineWidth, 1, x2, rightBottomY, x3, dashBottomY);
                drawLine(paint, lineColor, m_lineWidth, 1, x2, y2, x3, mValueY);
                //画选中点
                if (Selected)
                {
                    drawSelect(paint, lineColor, x1, y1);
                    drawSelect(paint, lineColor, x2, y2);
                }
            }
        }
    }

    /// <summary>
    /// 线性回归
    /// </summary>
    public sealed class LrLine : FCPlot
    {
        /// <summary>
        /// 创建线性回归
        /// </summary>
        public LrLine()
        {
            PlotType = "LRLINE";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            //获取点的位置
            FCPoint mp = getTouchOverPoint();
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            //获取参数
            float[] param = getLRParams(m_marks);
            if (param != null)
            {
                //判断选中
                float a = param[0];
                float b = param[1];
                double leftValue = a + b;
                double rightValue = (eIndex - bIndex + 1) * a + b;
                float y1 = py(leftValue);
                float y2 = py(rightValue);
                if (selectPoint(mp, x1, y1))
                {
                    action = ActionType.AT1;
                    return action;
                }
                else if (selectPoint(mp, x2, y2))
                {
                    action = ActionType.AT2;
                    return action;
                }
                FCChart chart = Chart;
                int touchIndex = chart.getTouchOverIndex();
                if (touchIndex >= bIndex && touchIndex <= eIndex)
                {
                    double yValue = a * ((touchIndex - bIndex) + 1) + b;
                    float y = py(yValue);
                    float x = px(touchIndex);
                    if (selectPoint(mp, x, y))
                    {
                        action = ActionType.MOVE;
                        return action;
                    }
                }
            }
            return action;
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return create2CandlePoints(mp);
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.MOVE)
                {
                    Cursor = FCCursors.Hand;
                }
                else
                {
                    Cursor = FCCursors.SizeWE;
                }
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
            }
        }

        /// <summary>
        /// 移动线条
        /// </summary>
        public override void onMoving()
        {
            FCPoint mp = getMovingPoint();
            //获取当前的索引和y值
            FCChart chart = Chart;
            int touchIndex = chart.getTouchOverIndex();
            if (touchIndex < 0)
            {
                touchIndex = 0;
            }
            if (touchIndex > chart.LastVisibleIndex)
            {
                touchIndex = chart.LastVisibleIndex;
            }
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
            //根据不同类型作出动作
            switch (m_action)
            {
                case ActionType.MOVE:
                    move(mp);
                    break;
                case ActionType.AT1:
                    if (touchIndex < eIndex)
                    {
                        resize(0);
                    }
                    break;
                case ActionType.AT2:
                    if (touchIndex > bIndex)
                    {
                        resize(1);
                    }
                    break;
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取直线参数
            float[] param = getLRParams(pList);
            if (param != null)
            {
                int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
                int eIndex = m_dataSource.getRowIndex(pList.get(1).Key);
                float x1 = px(bIndex);
                float x2 = px(eIndex);
                //画线条和选中点
                float a = param[0];
                float b = param[1];
                double leftValue = a + b;
                double rightValue = (eIndex - bIndex + 1) * a + b;
                float y1 = py(leftValue);
                float y2 = py(rightValue);
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
                //画选中点
                if (Selected)
                {
                    drawSelect(paint, lineColor, x1, y1);
                    drawSelect(paint, lineColor, x2, y2);
                }
            }
        }

    }

    /// <summary>
    /// 解消点
    /// </summary>
    public sealed class NullPoint : FCPlot
    {
        /// <summary>
        /// 创建解消点
        /// </summary>
        public NullPoint()
        {
            PlotType = "NULLPOINT";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            FCPoint mp = getTouchOverPoint();
            //获取相对位置
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
            double[] closeParam = getNullPointParams(m_marks);
            double leftClose = closeParam[1];
            double rightClose = closeParam[0];
            float y1 = py(leftClose);
            float y2 = py(rightClose);
            float x1 = px(bIndex >= eIndex ? bIndex : eIndex);
            float x2 = px(bIndex >= eIndex ? eIndex : bIndex);
            //获取参数
            float[] param = getLineParams(m_marks.get(0), m_marks.get(1));
            //产生动作类型
            if (param != null)
            {
                if (mp.x >= x1 - m_lineWidth * 2.5f && mp.x <= x1 + m_lineWidth * 2.5f)
                {
                    action = ActionType.AT1;
                    return action;
                }
                else if (mp.x >= x2 - m_lineWidth * 2.5f && mp.x <= x2 + m_lineWidth * 2.5f)
                {
                    action = ActionType.AT2;
                    return action;
                }
            }
            else
            {
                if (mp.y >= y1 - m_lineWidth * 5 && mp.y <= y1 + m_lineWidth * 5)
                {
                    action = ActionType.AT1;
                    return action;
                }
                else if (mp.y >= y2 - m_lineWidth * 5 && mp.y <= y2 + m_lineWidth * 5)
                {
                    action = ActionType.AT2;
                    return action;
                }
            }
            float x3 = 0, y3 = 0;
            if (y1 != y2)
            {
                //第一条线段
                nullPoint(x1, y1, x2, y2, ref x3, ref y3);
            }
            if (selectTriangle(mp, x1, y1, x2, y2, x3, y3))
            {
                action = ActionType.MOVE;
                return action;
            }
            return action;
        }

        /// <summary>
        /// 获取解消点的参数
        /// </summary>
        /// <param name="pList">点阵集合</param>
        /// <returns>解消点的参数</returns>
        private double[] getNullPointParams(HashMap<int, PlotMark> pList)
        {
            if (pList.size() == 0 || m_sourceFields == null || m_sourceFields.size() == 0 || !m_sourceFields.containsKey("CLOSE"))
            {
                return null;
            }
            int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(pList.get(1).Key);
            double leftClose = 0, rightClose = 0;
            double close1 = m_dataSource.get2(bIndex, m_sourceFields.get("CLOSE"));
            double close2 = m_dataSource.get2(eIndex, m_sourceFields.get("CLOSE"));
            if (eIndex >= bIndex)
            {
                leftClose = m_dataSource.get2(bIndex, m_sourceFields.get("CLOSE"));
                rightClose = m_dataSource.get2(eIndex, m_sourceFields.get("CLOSE"));
            }
            else
            {
                leftClose = m_dataSource.get2(eIndex, m_sourceFields.get("CLOSE"));
                rightClose = m_dataSource.get2(bIndex, m_sourceFields.get("CLOSE"));
            }
            return new double[] { leftClose, rightClose };
        }

        /// <summary>
        /// 计算解消点参数
        /// </summary>
        /// <param name="x1">第一个点的横坐标</param>
        /// <param name="y1">第一个点的纵坐标</param>
        /// <param name="x2">第二个点的横坐标</param>
        /// <param name="y2">第二个点的纵坐标</param>
        /// <param name="nullX">解消点的横坐标</param>
        /// <param name="nullY">解消点的纵坐标</param>
        private void nullPoint(float x1, float y1, float x2, float y2, ref float nullX, ref float nullY)
        {
            float k1 = 0, k2 = 0, b1 = 0, b2 = 0;
            if (y1 >= y2)
            {
                k1 = -(float)Math.Tan(45);
                k2 = -(float)Math.Tan(60);
                b1 = y1 - k1 * x1;
                b2 = y2 - k2 * x2;
            }
            else
            {
                k1 = (float)Math.Tan(45);
                k2 = (float)Math.Tan(60);
                b1 = y1 - k1 * x1;
                b2 = y2 - k2 * x2;
            }
            nullX = (b2 - b1) / (k1 - k2);
            nullY = nullX * k1 + b1;
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return create2CandlePoints(mp);
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.MOVE)
                {
                    Cursor = FCCursors.Hand;
                }
                else
                {
                    Cursor = FCCursors.SizeWE;
                }
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取相对位置 
            int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            int aIndex = m_dataSource.getRowIndex(pList.get(1).Key);
            double[] closeParam = getNullPointParams(pList);
            double leftClose = closeParam[1];
            double rightClose = closeParam[0];
            float y1 = py(leftClose);
            float y2 = py(rightClose);
            float x1 = px(bIndex >= aIndex ? bIndex : aIndex);
            float x2 = px(bIndex >= aIndex ? aIndex : bIndex);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
            float nullX = 0, nullY = 0;
            if (y1 != y2)
            {
                nullPoint(x1, y1, x2, y2, ref nullX, ref nullY);
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, nullX, nullY);
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x2, y2, nullX, nullY);
            }
            //画选中点
            if (Selected)
            {
                drawSelect(paint, lineColor, x1, y1);
                drawSelect(paint, lineColor, x2, y2);
            }
        }
    }

    /// <summary>
    /// 平行线
    /// </summary>
    public class Parallel : FCPlot
    {
        /// <summary>
        /// 创建平行线
        /// </summary>
        public Parallel()
        {
            PlotType = "PARALLEL";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            FCPoint mp = getTouchOverPoint();
            float y1 = py(m_marks.get(0).Value);
            float y2 = py(m_marks.get(1).Value);
            float y3 = py(m_marks.get(2).Value);
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
            int pIndex = m_dataSource.getRowIndex(m_marks.get(2).Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            float x3 = px(pIndex);
            //获取直线参数
            float[] param = getParallelParams(m_marks);
            //非垂直
            if (param != null)
            {
                if (selectPoint(mp, x1, y1))
                {
                    action = ActionType.AT1;
                    return action;
                }
                else if (selectPoint(mp, x2, y2))
                {
                    action = ActionType.AT2;
                    return action;
                }
                else if (selectPoint(mp, x3, y3))
                {
                    action = ActionType.AT3;
                    return action;
                }
                //主直线
                if (mp.y - param[0] * mp.x - param[1] >= m_lineWidth * -5 && mp.y - param[0] * mp.x - param[1] <= m_lineWidth * 5)
                {
                    action = ActionType.MOVE;
                    return action;
                }
                //平行线
                if (mp.y - param[0] * mp.x - param[2] >= m_lineWidth * -5 && mp.y - param[0] * mp.x - param[2] <= m_lineWidth * 5)
                {
                    action = ActionType.MOVE;
                    return action;
                }
            }
            //垂直
            else
            {
                if (mp.y >= y1 - m_lineWidth * 5 && mp.y <= y1 + m_lineWidth * 5)
                {
                    action = ActionType.AT1;
                    return action;
                }
                else if (mp.y >= y2 - m_lineWidth * 5 && mp.y <= y2 + m_lineWidth * 5)
                {
                    action = ActionType.AT2;
                    return action;
                }
                else if (mp.y >= y3 - m_lineWidth * 5 && mp.y <= y3 + m_lineWidth * 5)
                {
                    action = ActionType.AT3;
                    return action;
                }
                if (mp.x >= x1 - m_lineWidth * 5 && mp.x <= x1 + m_lineWidth * 5)
                {
                    action = ActionType.MOVE;
                    return action;
                }
                if (mp.x >= x3 - m_lineWidth * 5 && mp.x <= x3 + m_lineWidth * 5)
                {
                    action = ActionType.MOVE;
                    return action;
                }
            }
            return action;
        }

        /// <summary>
        /// 获取平行直线的参数
        /// </summary>
        /// <param name="pList">点阵集合</param>
        /// <returns>平行直线的参数</returns>
        protected float[] getParallelParams(HashMap<int, PlotMark> pList)
        {
            if (pList.size() == 0)
            {
                return null;
            }
            float y1 = py(pList.get(0).Value);
            float y2 = py(pList.get(1).Value);
            float y3 = py(pList.get(2).Value);
            int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(pList.get(1).Key);
            int pIndex = m_dataSource.getRowIndex(pList.get(2).Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            float x3 = px(pIndex);
            float a = 0;
            if (x2 - x1 != 0)
            {
                a = (y2 - y1) / (x2 - x1);
                float b = y1 - a * x1;
                float c = y3 - a * x3;
                return new float[] { a, b, c };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return create3Points(mp);
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.MOVE)
                {
                    Cursor = FCCursors.Hand;
                }
                else
                {
                    Cursor = FCCursors.SizeWE;
                }
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
                m_startMarks.put(2, m_marks.get(2));
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取相对位置
            float y1 = py(pList.get(0).Value);
            float y2 = py(pList.get(1).Value);
            float y3 = py(pList.get(2).Value);
            int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(pList.get(1).Key);
            int pIndex = m_dataSource.getRowIndex(pList.get(2).Key);
            //获取参数
            float[] param = getParallelParams(pList);
            float a = 0;
            float b = 0;
            float c = 0;
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            float x3 = px(pIndex);
            //非垂直
            if (param != null)
            {
                a = param[0];
                b = param[1];
                c = param[2];
                float leftX = 0;
                float leftY = leftX * a + b;
                float rightX = WorkingAreaWidth;
                float rightY = rightX * a + b;
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, leftX, leftY, rightX, rightY);
                leftY = leftX * a + c;
                rightY = rightX * a + c;
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, leftX, leftY, rightX, rightY);
            }
            //垂直
            else
            {
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, 0, x1, WorkingAreaHeight);
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x3, 0, x3, WorkingAreaHeight);
            }
            //画选中点
            if (Selected)
            {
                drawSelect(paint, lineColor, x1, y1);
                drawSelect(paint, lineColor, x2, y2);
                drawSelect(paint, lineColor, x3, y3);
            }
        }
    }

    /// <summary>
    /// 平行四边形
    /// </summary>
    public class ParalleGram : FCPlot
    {
        /// <summary>
        /// 创建平行四边形
        /// </summary>
        public ParalleGram()
        {
            PlotType = "PARALLELOGRAM";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            FCPoint mp = getTouchOverPoint();
            if (m_moveTimes == 1)
            {
                action = ActionType.AT3;
                return action;
            }
            else
            {
                //获取点的位置
                float y1 = py(m_marks.get(0).Value);
                float y2 = py(m_marks.get(1).Value);
                float y3 = py(m_marks.get(2).Value);
                int aIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
                int bIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
                int cIndex = m_dataSource.getRowIndex(m_marks.get(2).Key);
                float x1 = px(aIndex);
                float x2 = px(bIndex);
                float x3 = px(cIndex);
                if (selectPoint(mp, x1, y1) || m_moveTimes == 1)
                {
                    action = ActionType.AT1;
                    return action;
                }
                else if (selectPoint(mp, x2, y2))
                {
                    action = ActionType.AT2;
                    return action;
                }
                else if (selectPoint(mp, x3, y3))
                {
                    action = ActionType.AT3;
                    return action;
                }
            }
            FCPoint[] points = getPLPoints(m_marks);
            for (int i = 0; i < points.Length; i++)
            {
                int start = i;
                int end = i + 1;
                if (start == 3)
                {
                    end = 0;
                }
                if (selectRay(mp, points[start].x, points[start].y, points[end].x, points[end].y))
                {
                    action = ActionType.MOVE;
                    return action;
                }
            }
            return action;
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="marks">点的集合</param>
        /// <returns>坐标数组</returns>
        private FCPoint[] getPLPoints(HashMap<int, PlotMark> marks)
        {
            FCPoint point1 = new FCPoint(px(m_dataSource.getRowIndex(marks.get(0).Key)), py(marks.get(0).Value));
            FCPoint point2 = new FCPoint(px(m_dataSource.getRowIndex(marks.get(1).Key)), py(marks.get(1).Value));
            FCPoint point3 = new FCPoint(px(m_dataSource.getRowIndex(marks.get(2).Key)), py(marks.get(2).Value));
            float x1 = 0, y1 = 0, x2 = 0, y2 = 0, x3 = 0, y3 = 0, x4 = 0, y4 = 0;
            x1 = point1.x;
            y1 = point1.y;
            x2 = point2.x;
            y2 = point2.y;
            x3 = point3.x;
            y3 = point3.y;
            parallelogram(x1, y1, x2, y2, x3, y3, ref x4, ref y4);
            FCPoint point4 = new FCPoint(x4, y4);
            return new FCPoint[] { point1, point2, point3, point4 };
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            int rIndex = m_dataSource.RowsCount;
            if (rIndex > 0)
            {
                int currentIndex = getIndex(mp);
                double y = getNumberValue(mp);
                double date = m_dataSource.getXValue(currentIndex);
                m_marks.clear();
                m_marks.put(0, new PlotMark(0, date, y));
                int si = currentIndex + 10;
                FCChart chart = Chart;
                if (si > chart.LastVisibleIndex)
                {
                    si = chart.LastVisibleIndex;
                }
                m_marks.put(1, new PlotMark(1, m_dataSource.getXValue(si), y));
                m_marks.put(2, new PlotMark(2, date, y));
                return true;
            }
            return false;
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_moveTimes++;
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.MOVE)
                {
                    Cursor = FCCursors.Hand;
                }
                else
                {
                    Cursor = FCCursors.SizeWE;
                }
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
                m_startMarks.put(2, m_marks.get(2));
            }
        }

        /// <summary>
        /// 绘制图像的残影
        /// </summary>
        /// <param name="paint">绘图对象</param>
        public override void onPaintGhost(FCPaint paint)
        {
            if (m_moveTimes > 1)
            {
                onPaint(paint, m_startMarks, SelectedColor);
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            FCPoint[] points = getPLPoints(pList);
            //画线
            for (int i = 0; i < points.Length; i++)
            {
                int start = i;
                int end = i + 1;
                if (start == 3)
                {
                    end = 0;
                }
                float x1 = points[start].x;
                float y1 = points[start].y;
                float x2 = points[end].x;
                float y2 = points[end].y;
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
                //画选中点
                if (Selected && i < 3)
                {
                    drawSelect(paint, lineColor, x1, y1);
                }
            }
        }
    }

    /// <summary>
    /// 百分比线
    /// </summary>
    public class Percent : FCPlot
    {
        /// <summary>
        /// 创建百分比线
        /// </summary>
        public Percent()
        {
            PlotType = "PERCENT";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            FCPoint mp = getTouchOverPoint();
            float y1 = py(m_marks.get(0).Value);
            float y2 = py(m_marks.get(1).Value);
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            float x1 = px(bIndex);
            if (m_moveTimes == 1)
            {
                action = ActionType.AT1;
                return action;
            }
            if (mp.x >= x1 - m_lineWidth * 2.5f && mp.x <= x1 + m_lineWidth * 2.5f)
            {
                if (mp.y >= y1 - m_lineWidth * 2.5f && mp.y <= y1 + m_lineWidth * 2.5f)
                {
                    action = ActionType.AT1;
                    return action;
                }
                else if (mp.y >= y2 - m_lineWidth * 2.5f && mp.y <= y2 + m_lineWidth * 2.5f)
                {
                    action = ActionType.AT2;
                    return action;
                }
            }
            if (hLinesSelect(getPercentParams(m_marks.get(0).Value, m_marks.get(1).Value), 5))
            {
                action = ActionType.MOVE;
            }
            return action;
        }

        /// <summary>
        /// 获取百分比线的参数
        /// </summary>
        /// <param name="value1">值1</param>
        /// <param name="value2">值2</param>
        /// <returns>百分比线的参数</returns>
        protected float[] getPercentParams(double value1, double value2)
        {
            float y1 = py(value1);
            float y2 = py(value2);
            float y0 = 0, y25 = 0, y50 = 0, y75 = 0, y100 = 0;
            y0 = y1;
            y25 = y1 <= y2 ? y1 + (y2 - y1) / 4 : y2 + (y1 - y2) * 3 / 4;
            y50 = y1 <= y2 ? y1 + (y2 - y1) / 2 : y2 + (y1 - y2) / 2;
            y75 = y1 <= y2 ? y1 + (y2 - y1) * 3 / 4 : y2 + (y1 - y2) / 4;
            y100 = y2;
            return new float[] { y0, y25, y50, y75, y100 };
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return create2PointsB(mp);
        }


        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_moveTimes++;
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.MOVE)
                {
                    Cursor = FCCursors.Hand;
                }
                else
                {
                    Cursor = FCCursors.SizeWE;
                }
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
            }
        }

        /// <summary>
        /// 移动线条
        /// </summary>
        public override void onMoving()
        {
            FCPoint mp = getMovingPoint();
            //根据不同类型作出动作
            switch (m_action)
            {
                case ActionType.AT1:
                    resize(0);
                    break;
                case ActionType.AT2:
                    resize(1);
                    break;
                case ActionType.MOVE:
                    double subY = mp.y - m_startPoint.y;
                    double maxValue = m_div.getVScale(m_attachVScale).VisibleMax;
                    double minValue = m_div.getVScale(m_attachVScale).VisibleMin;
                    double yAddValue = subY / WorkingAreaHeight * (minValue - maxValue);
                    m_marks.put(0, new PlotMark(0, m_startMarks.get(0).Key, m_startMarks.get(0).Value + yAddValue));
                    m_marks.put(1, new PlotMark(1, m_startMarks.get(1).Key, m_startMarks.get(1).Value + yAddValue));
                    break;
            }
        }

        /// <summary>
        /// 绘制图像的残影
        /// </summary>
        /// <param name="paint">绘图对象</param>
        public override void onPaintGhost(FCPaint paint)
        {
            if (m_moveTimes > 1)
            {
                onPaint(paint, m_startMarks, SelectedColor);
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取相对位置
            float y1 = py(pList.get(0).Value);
            float y2 = py(pList.get(1).Value);
            int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            float x1 = px(bIndex);
            //获取参数
            float[] lineParam = getPercentParams(pList.get(0).Value, pList.get(1).Value);
            String[] str = new String[] { "0.00%", "25.00%", "50.00%", "75.00%", "100.00%" };
            //画线和文字
            for (int i = 0; i < lineParam.Length; i++)
            {
                FCSize sizeF = textSize(paint, str[i], m_font);
                float yP = lineParam[i];
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, 0, yP, WorkingAreaWidth, yP);
                drawText(paint, str[i], lineColor, m_font, WorkingAreaWidth - sizeF.cx, yP - sizeF.cy);
            }
            //画选中点
            if (Selected)
            {
                drawSelect(paint, lineColor, x1, y1);
                drawSelect(paint, lineColor, x1, y2);
            }
        }
    }

    /// <summary>
    /// 周期线
    /// </summary>
    public sealed class Periodic : FCPlot
    {
        /// <summary>
        /// 创建周期线
        /// </summary>
        public Periodic()
        {
            PlotType = "PERIODIC";
        }

        /// <summary>
        /// 周期
        /// </summary>
        private int m_period = 5;

        /// <summary>
        /// 初始移动周期
        /// </summary>
        private int m_beginPeriod = 1;

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            //获取点的位置
            FCPoint mp = getTouchOverPoint();
            //获取参数
            double[] param = getPLParams(m_marks);
            float y = WorkingAreaHeight / 2;
            for (int i = 0; i < param.Length; i++)
            {
                //判断选中
                int rI = (int)param[i];
                float x1 = px(rI);
                if (selectPoint(mp, x1, y))
                {
                    action = ActionType.AT1;
                    m_marks.put(0, new PlotMark(0, m_dataSource.getXValue(rI), 0));
                    m_beginPeriod = m_period;
                    return action;
                }
                if (mp.x >= x1 - m_lineWidth * 2.5f && mp.x <= x1 + m_lineWidth * 2.5f)
                {
                    action = ActionType.MOVE;
                    return action;
                }
            }
            return action;
        }

        /// <summary>
        /// 获取线条参数
        /// </summary>
        /// <param name="pList">点阵的集合</param>
        /// <returns>点阵的数组描述</returns>
        private double[] getPLParams(HashMap<int, PlotMark> pList)
        {
            if (pList.size() == 0)
            {
                return null;
            }
            double fValue = pList.get(0).Value;
            int aIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            ArrayList<double> fValueList = new ArrayList<double>();
            FCChart chart = Chart;
            for (int i = chart.FirstVisibleIndex; i < chart.LastVisibleIndex; i++)
            {
                if (Math.Abs(i - aIndex) % m_period == 0)
                {
                    fValueList.add(i);
                }
            }
            return fValueList.ToArray();
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">出现位置</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            int rIndex = m_dataSource.RowsCount;
            if (rIndex > 0)
            {
                int currentIndex = getIndex(mp);
                double y = getNumberValue(mp);
                double date = m_dataSource.getXValue(currentIndex);
                m_marks.clear();
                m_marks.put(0, new PlotMark(0, date, y));
                FCChart chart = Chart;
                m_period = chart.MaxVisibleRecord / 10;
                if (m_period < 1)
                {
                    m_period = 1;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.AT1)
                {
                    Cursor = FCCursors.SizeNS;
                }
                else
                {
                    Cursor = FCCursors.Hand;
                }
                m_startMarks.put(0, m_marks.get(0));
            }
        }

        /// <summary>
        /// 移动方法
        /// </summary>
        public override void onMoving()
        {
            FCPoint mp = getMovingPoint();
            //获取当前的索引和y值
            FCChart chart = Chart;
            int touchIndex = chart.getTouchOverIndex();
            if (touchIndex < 0)
            {
                touchIndex = 0;
            }
            if (touchIndex > chart.LastVisibleIndex)
            {
                touchIndex = chart.LastVisibleIndex;
            }
            int bI = getIndex(m_startPoint);
            int eI = getIndex(mp);
            //根据不同类型作出动作
            switch (m_action)
            {
                case ActionType.MOVE:
                    move(mp);
                    break;
                case ActionType.AT1:
                    m_period = m_beginPeriod + (eI - bI);
                    if (m_period < 1)
                    {
                        m_period = 1;
                    }
                    break;
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取参数
            double[] param = getPLParams(pList);
            for (int i = 0; i < param.Length; i++)
            {
                int rI = (int)param[i];
                float x1 = px(rI);
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, 0, x1, WorkingAreaHeight);
                if (Selected)
                {
                    drawSelect(paint, lineColor, x1, WorkingAreaHeight / 2);
                }
            }
        }
    }

    /// <summary>
    /// 价格签条
    /// </summary>
    public sealed class Price : FCPlot
    {
        /// <summary>
        /// 创建价格签条
        /// </summary>
        public Price()
        {
            PlotType = "PRICE";
        }

        private FCSize m_textSize;

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            //获取点的位置
            FCPoint mp = getTouchOverPoint();
            double fValue = m_marks.get(0).Value;
            int aIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            float x1 = px(aIndex);
            float y1 = py(fValue);
            FCRect rect = new FCRect(x1, y1, x1 + m_textSize.cx, y1 + m_textSize.cy);
            if (mp.x >= rect.left && mp.x <= rect.right && mp.y >= rect.top && mp.y <= rect.bottom)
            {
                action = ActionType.MOVE;
            }
            return action;
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return createPoint(mp);
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                Cursor = FCCursors.Hand;
                m_startMarks.put(0, m_marks.get(0));
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            int wX = WorkingAreaWidth;
            int wY = WorkingAreaHeight;
            if (wX > 0 && wY > 0)
            {
                //获取相对位置
                double fValue = pList.get(0).Value;
                int aIndex = m_dataSource.getRowIndex(pList.get(0).Key);
                float x1 = px(aIndex);
                float y1 = py(fValue);
                FCChart chart = Chart;
                String word = FCStr.getValueByDigit(fValue, chart.LeftVScaleWidth > 0 ? m_div.LeftVScale.Digit : m_div.RightVScale.Digit);
                //画文字
                drawText(paint, word, lineColor, m_font, x1, y1);
                m_textSize = textSize(paint, word, m_font);
                if (Selected)
                {
                    if (m_textSize.cx > 0 && m_textSize.cy > 0)
                    {
                        drawRect(paint, lineColor, m_lineWidth, m_lineStyle, (int)x1, (int)y1, (int)x1 + m_textSize.cx, (int)y1 + m_textSize.cy);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 价格通道线
    /// </summary>
    public class PriceChannel : Parallel
    {
        /// <summary>
        /// 创建价格通道线
        /// </summary>
        public PriceChannel()
        {
            PlotType = "PRICECHANNEL";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = base.getAction();
            if (action != ActionType.NO)
            {
                return action;
            }
            FCPoint mp = getTouchOverPoint();
            float k = 0, d = 0, x4 = 0;
            getLine3Params(m_marks, ref k, ref d, ref x4);
            if (k == 0 && d == 0)
            {
                if (mp.x >= x4 - m_lineWidth * 5 && mp.x <= x4 + m_lineWidth * 5)
                {
                    action = ActionType.MOVE;
                }
            }
            else
            {
                if (selectLine(mp, px(mp.x), k, d))
                {
                    action = ActionType.MOVE;
                }
            }
            return action;
        }

        /// <summary>
        /// 获取第三条线的参数
        /// </summary>
        /// <param name="marks">点阵集合</param>
        /// <param name="k">直线参数k</param>
        /// <param name="d">直线参数d</param>
        /// <param name="x4">第四个点的横坐标</param>
        private void getLine3Params(HashMap<int, PlotMark> marks, ref float k, ref float d, ref float x4)
        {
            //获取参数
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int pIndex = m_dataSource.getRowIndex(m_marks.get(2).Key);
            float x1 = px(bIndex);
            float x3 = px(pIndex);
            float[] param = getParallelParams(m_marks);
            if (param != null)
            {
                k = param[0];
                float b = param[1];
                float c = param[2];
                d = b >= c ? b + (b - c) : b - (c - b);
            }
            else
            {
                x4 = 0;
                if (x3 > x1)
                {
                    x4 = x1 - (x3 - x1);
                }
                else
                {
                    x4 = x1 + (x1 - x3);
                }
            }
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                Cursor = FCCursors.Hand;
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
                m_startMarks.put(2, m_marks.get(2));
            }
        }

        /// <summary>
        /// 绘制图像
        /// </summary>
        /// <param name="paint">绘图对象</param>
        public override void onPaint(FCPaint paint)
        {
            paintEx(paint, m_marks, Color);
            base.onPaint(paint);
        }

        /// <summary>
        /// 绘制图像的残影
        /// </summary>
        /// <param name="paint">绘图对象</param>
        public override void onPaintGhost(FCPaint paint)
        {
            paintEx(paint, m_startMarks, SelectedColor);
            base.onPaintGhost(paint);
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        private void paintEx(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            float k = 0, d = 0, x4 = 0;
            getLine3Params(m_marks, ref k, ref d, ref x4);
            if (k == 0 && d == 0)
            {
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x4, 0, x4, WorkingAreaHeight);

            }
            else
            {
                float leftX = 0;
                float leftY = leftX * k + d;
                float rightX = WorkingAreaWidth;
                float rightY = rightX * k + d;
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, leftX, leftY, rightX, rightY);
            }
        }
    }

    /// <summary>
    /// 四等分线
    /// </summary>
    public sealed class QuadrantLines : Percent
    {
        /// <summary>
        /// 创建四等分线
        /// </summary>
        public QuadrantLines()
        {
            PlotType = "QUADRANTLINES";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            FCPoint mp = getTouchOverPoint();
            //获取点的位置
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            //获取参数
            float[] param = getLRParams(m_marks);
            if (param != null)
            {
                float a = param[0];
                float b = param[1];
                double leftValue = a + b;
                double rightValue = (eIndex - bIndex + 1) * a + b;
                float y1 = py(leftValue);
                float y2 = py(rightValue);
                if (selectPoint(mp, x1, y1))
                {
                    action = ActionType.AT1;
                    return action;
                }
                else if (selectPoint(mp, x2, y2))
                {
                    action = ActionType.AT2;
                    return action;
                }
                FCChart chart = Chart;
                int touchIndex = chart.getTouchOverIndex();
                if (touchIndex >= bIndex && touchIndex <= eIndex)
                {
                    //回归线判断
                    double yValue = a * ((touchIndex - bIndex) + 1) + b;
                    float y = py(yValue);
                    float x = px(touchIndex);
                    if (mp.x >= x - 5 && mp.x <= x + 5 && mp.y >= y - 5 && mp.y <= y + 5)
                    {
                        action = ActionType.MOVE;
                        return action;
                    }
                    //等分线判断
                    double[] candleRegion = getCandleRange(m_marks);
                    if (candleRegion != null)
                    {
                        float[] percents = getPercentParams(candleRegion[0], candleRegion[1]);
                        for (int i = 0; i < percents.Length; i++)
                        {
                            if (selectRay(mp, x1, percents[i], x2, percents[i]))
                            {
                                action = ActionType.MOVE;
                                return action;
                            }
                        }
                    }
                }
            }
            return action;
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return create2CandlePoints(mp);
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.MOVE)
                {
                    Cursor = FCCursors.Hand;
                }
                else
                {
                    Cursor = FCCursors.SizeWE;
                }
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
            }
        }

        /// <summary>
        /// 移动线条
        /// </summary>
        public override void onMoving()
        {
            FCPoint mp = getMovingPoint();
            //获取当前的索引和y值
            FCChart chart = Chart;
            int touchIndex = chart.getTouchOverIndex();
            if (touchIndex < 0)
            {
                touchIndex = 0;
            }
            if (touchIndex > chart.LastVisibleIndex)
            {
                touchIndex = chart.LastVisibleIndex;
            }
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
            //根据不同类型作出动作
            switch (m_action)
            {
                case ActionType.MOVE:
                    move(mp);
                    break;
                case ActionType.AT1:
                    if (touchIndex < eIndex)
                    {
                        resize(0);
                    }
                    break;
                case ActionType.AT2:
                    if (touchIndex > bIndex)
                    {
                        resize(1);
                    }
                    break;
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取直线参数
            float[] param = getLRParams(pList);
            if (param != null)
            {
                int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
                int eIndex = m_dataSource.getRowIndex(pList.get(1).Key);
                float x1 = px(bIndex);
                float x2 = px(eIndex);
                //画线条和选中点
                float a = param[0];
                float b = param[1];
                double leftValue = a + b;
                double rightValue = (eIndex - bIndex + 1) * a + b;
                float y1 = py(leftValue);
                float y2 = py(rightValue);
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
                //等分线判断
                double[] candleRegion = getCandleRange(pList);
                if (candleRegion != null)
                {
                    float[] percents = getPercentParams(candleRegion[0], candleRegion[1]);
                    for (int i = 0; i < percents.Length; i++)
                    {
                        float yp = percents[i];
                        if (i == 2)
                        {
                            drawLine(paint, lineColor, m_lineWidth, 1, x1, yp, x2, yp);
                        }
                        else
                        {
                            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, yp, x2, yp);
                        }
                    }
                }
                //画选中点
                if (Selected)
                {
                    drawSelect(paint, lineColor, x1, y1);
                    drawSelect(paint, lineColor, x2, y2);
                }
            }
        }
    }

    /// <summary>
    /// 拉弗回归通道
    /// </summary>
    public sealed class RaffChannel : FCPlot
    {
        /// <summary>
        /// 创建拉弗回归通道
        /// </summary>
        public RaffChannel()
        {
            PlotType = "RAFFCHANNEL";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            FCPoint mp = getTouchOverPoint();
            //获取点的位置
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            //获取参数
            float[] param = getLRParams(m_marks);
            if (param != null)
            {
                float a = param[0];
                float b = param[1];
                double leftValue = a + b;
                double rightValue = (eIndex - bIndex + 1) * a + b;
                float y1 = py(leftValue);
                float y2 = py(rightValue);
                if (selectPoint(mp, x1, y1))
                {
                    action = ActionType.AT1;
                    return action;
                }
                else if (selectPoint(mp, x2, y2))
                {
                    action = ActionType.AT2;
                    return action;
                }
                FCChart chart = Chart;
                int touchIndex = chart.getTouchOverIndex();
                if (touchIndex >= chart.FirstVisibleIndex && touchIndex <= chart.LastVisibleIndex)
                {
                    double yValue = a * ((touchIndex - bIndex) + 1) + b;
                    float y = py(yValue);
                    float x = px(touchIndex);
                    if (mp.x >= x - 5 && mp.x <= x + 5 && mp.y >= y - 5 && mp.y <= y + 5)
                    {
                        action = ActionType.MOVE;
                        return action;
                    }
                    double parallel = getRRCRange(m_marks, param);
                    yValue = a * ((touchIndex - bIndex) + 1) + b + parallel;
                    y = py(yValue);
                    x = px(touchIndex);
                    if (mp.x >= x - 5 && mp.x <= x + 5 && mp.y >= y - 5 && mp.y <= y + 5)
                    {
                        action = ActionType.MOVE;
                        return action;
                    }
                    yValue = a * ((touchIndex - bIndex) + 1) + b - parallel;
                    y = py(yValue);
                    x = px(touchIndex);
                    if (mp.x >= x - 5 && mp.x <= x + 5 && mp.y >= y - 5 && mp.y <= y + 5)
                    {
                        action = ActionType.MOVE;
                        return action;
                    }
                }
            }
            return action;
        }

        /// <summary>
        /// 获取拉弗线性回归的高低点值
        /// </summary>
        /// <param name="pList">点阵集合</param>
        /// <param name="param">直线参数</param>
        /// <returns>拉弗线性回归的高低点值</returns>
        private double getRRCRange(HashMap<int, PlotMark> pList, float[] param)
        {
            if (param == null || m_sourceFields == null || m_sourceFields.size() == 0 || !m_sourceFields.containsKey("HIGH")
            || !m_sourceFields.containsKey("LOW"))
            {
                return 0;
            }
            float a = param[0];
            float b = param[1];
            int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(pList.get(1).Key);
            double upSubValue = 0;
            double downSubValue = 0;
            int pos = 0;
            for (int i = bIndex; i <= eIndex; i++)
            {
                double high = m_dataSource.get2(i, m_sourceFields.get("HIGH"));
                double low = m_dataSource.get2(i, m_sourceFields.get("LOW"));
                if (!double.IsNaN(high) && !double.IsNaN(low))
                {
                    double midValue = (i - bIndex + 1) * a + b;
                    if (pos == 0)
                    {
                        upSubValue = high - midValue;
                        downSubValue = midValue - low;
                    }
                    else
                    {
                        if (high - midValue > upSubValue)
                        {
                            upSubValue = high - midValue;
                        }
                        if (midValue - low > downSubValue)
                        {
                            downSubValue = midValue - low;
                        }
                    }
                    pos++;
                }
            }
            if (upSubValue >= downSubValue)
            {
                return upSubValue;
            }
            else
            {
                return downSubValue;
            }
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return create2CandlePoints(mp);
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.MOVE)
                {
                    Cursor = FCCursors.Hand;
                }
                else
                {
                    Cursor = FCCursors.SizeWE;
                }
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
            }
        }

        /// <summary>
        /// 移动线条
        /// </summary>
        public override void onMoving()
        {
            FCPoint mp = getMovingPoint();
            //获取当前的索引和y值
            FCChart chart = Chart;
            int touchIndex = chart.getTouchOverIndex();
            if (touchIndex < 0)
            {
                touchIndex = 0;
            }
            if (touchIndex > chart.LastVisibleIndex)
            {
                touchIndex = chart.LastVisibleIndex;
            }
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
            //根据不同类型作出动作
            switch (m_action)
            {
                case ActionType.MOVE:
                    move(mp);
                    break;
                case ActionType.AT1:
                    if (touchIndex < eIndex)
                    {
                        resize(0);
                    }
                    break;
                case ActionType.AT2:
                    if (touchIndex > bIndex)
                    {
                        resize(1);
                    }
                    break;
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取直线参数
            float[] param = getLRParams(pList);
            if (param != null)
            {
                //画线条和选中点
                float a = param[0];
                float b = param[1];
                int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
                int eIndex = m_dataSource.getRowIndex(pList.get(1).Key);
                float x1 = px(bIndex);
                float x2 = px(eIndex);
                double leftValue = a + b;
                double rightValue = (eIndex - bIndex + 1) * a + b;
                float y1 = py(leftValue);
                float y2 = py(rightValue);
                float[] param2 = getLineParams(new PlotMark(0, pList.get(0).Key, leftValue), new PlotMark(1, pList.get(1).Key, rightValue));
                if (param2 != null)
                {
                    //画回归线
                    a = param2[0];
                    b = param2[1];
                    float leftX = 0;
                    float leftY = leftX * a + b;
                    float rightX = WorkingAreaWidth;
                    float rightY = rightX * a + b;
                    drawLine(paint, lineColor, m_lineWidth, m_lineStyle, leftX, leftY, rightX, rightY);
                    //获取拉弗参数
                    double parallel = getRRCRange(pList, param);
                    //画上线
                    double leftTop = leftValue + parallel;
                    double rightTop = rightValue + parallel;
                    param2 = getLineParams(new PlotMark(0, pList.get(0).Key, leftTop), new PlotMark(1, pList.get(1).Key, rightTop));
                    if (param2 != null)
                    {
                        a = param2[0];
                        b = param2[1];
                        leftX = 0;
                        leftY = leftX * a + b;
                        rightX = WorkingAreaWidth;
                        rightY = rightX * a + b;
                        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, leftX, leftY, rightX, rightY);
                    }
                    //画下线
                    double leftBottom = leftValue - parallel;
                    double rightBottom = rightValue - parallel;
                    param2 = getLineParams(new PlotMark(0, pList.get(0).Key, leftBottom), new PlotMark(1, pList.get(1).Key, rightBottom));
                    if (param2 != null)
                    {
                        a = param2[0];
                        b = param2[1];
                        leftX = 0;
                        leftY = leftX * a + b;
                        rightX = WorkingAreaWidth;
                        rightY = rightX * a + b;
                        drawLine(paint, lineColor, m_lineWidth, m_lineStyle, leftX, leftY, rightX, rightY);
                    }
                }
                //画选中点
                if (Selected)
                {
                    drawSelect(paint, lineColor, x1, y1);
                    drawSelect(paint, lineColor, x2, y2);
                }
            }
        }
    }

    /// <summary>
    /// 幅度尺
    /// </summary>
    public sealed class RangeRuler : FCPlot
    {
        /// <summary>
        /// 创建幅度尺
        /// </summary>
        public RangeRuler()
        {
            PlotType = "RANGERULER";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            //获取点位置
            FCPoint mp = getTouchOverPoint();
            float y1 = py(m_marks.get(0).Value);
            float y2 = py(m_marks.get(1).Value);
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            //获取参数
            double[] param = getCandleRange(m_marks);
            double nHigh = param[0];
            double nLow = param[1];
            float highY = py(nHigh);
            float lowY = py(nLow);
            //获取较大的X和较小的X
            float smallX = x1 > x2 ? x2 : x1;
            float bigX = x1 > x2 ? x1 : x2;
            if ((mp.y >= highY - m_lineWidth * 2.5f && mp.y <= highY + m_lineWidth * 2.5f)
                || (mp.y >= lowY - m_lineWidth * 2.5f && mp.y <= lowY + m_lineWidth * 2.5f))
            {
                if (mp.x >= x1 - m_lineWidth * 2.5f && mp.x <= x1 + m_lineWidth * 2.5f)
                {
                    action = ActionType.AT1;
                    return action;
                }
                else if (mp.x >= x2 - m_lineWidth * 2.5f && mp.x <= x2 + m_lineWidth * 2.5f)
                {
                    action = ActionType.AT2;
                    return action;
                }
            }
            //选中上面的线
            if (mp.y >= highY - m_lineWidth * 2.5f && mp.y <= highY + m_lineWidth * 2.5f)
            {
                if (mp.x >= smallX - m_lineWidth * 2.5f && mp.x <= bigX + m_lineWidth * 2.5f)
                {
                    action = ActionType.MOVE;
                    return action;
                }
            }
            //选中下面的线
            else if (mp.y >= lowY - m_lineWidth * 2.5f && mp.y <= lowY + m_lineWidth * 2.5f)
            {
                if (mp.x >= smallX - m_lineWidth * 2.5f && mp.x <= bigX + m_lineWidth * 2.5f)
                {
                    action = ActionType.MOVE;
                    return action;
                }
            }
            //选中中线
            float mid = x1 >= x2 ? (x2 + (x1 - x2) / 2) : (x1 + (x2 - x1) / 2);
            if (mp.x >= mid - m_lineWidth * 2.5f && mp.x <= mid + m_lineWidth * 2.5f)
            {
                if (mp.y >= highY - m_lineWidth * 2.5f && mp.y <= lowY + m_lineWidth * 2.5f)
                {
                    action = ActionType.MOVE;
                    return action;
                }
            }
            return action;
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return create2CandlePoints(mp);
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.MOVE)
                {
                    Cursor = FCCursors.Hand;
                }
                else
                {
                    Cursor = FCCursors.SizeNS;
                }
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取相对位置
            float y1 = py(pList.get(0).Value);
            float y2 = py(pList.get(1).Value);
            int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(pList.get(1).Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            float smallX = x1 > x2 ? x2 : x1;
            float bigX = x1 > x2 ? x1 : x2;
            //获取参数
            double[] param = getCandleRange(pList);
            double nHigh = param[0];
            double nLow = param[1];
            float highY = py(nHigh);
            float lowY = py(nLow);
            float mid = x1 >= x2 ? (x2 + (x1 - x2) / 2) : (x1 + (x2 - x1) / 2);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, highY, x2, highY);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, lowY, x2, lowY);
            drawLine(paint, lineColor, m_lineWidth, 1, mid, lowY, mid, highY);
            if (nHigh != nLow)
            {
                //画文字
                double diff = Math.Abs(nLow - nHigh);
                double range = 0;
                if (nHigh != 0)
                {
                    range = diff / nHigh;
                }
                FCChart chart = Chart;
                String diffString = FCStr.getValueByDigit(diff, m_div.getVScale(m_attachVScale).Digit);
                String rangeString = range.ToString("0.00%");
                FCSize diffSize = textSize(paint, diffString, m_font);
                FCSize rangeSize = textSize(paint, rangeString, m_font);
                drawText(paint, diffString, lineColor, m_font, bigX - diffSize.cx, highY + 2);
                drawText(paint, rangeString, lineColor, m_font, bigX - rangeSize.cx, lowY - rangeSize.cy);
            }
            //画选中点
            if (Selected)
            {
                drawSelect(paint, lineColor, smallX, highY);
                drawSelect(paint, lineColor, smallX, lowY);
                drawSelect(paint, lineColor, bigX, highY);
                drawSelect(paint, lineColor, bigX, lowY);
            }
        }
    }

    /// <summary>
    /// 上升45度线
    /// </summary>
    public sealed class RaseLine : FCPlot
    {
        /// <summary>
        /// 创建上升45度线
        /// </summary>
        public RaseLine()
        {
            PlotType = "RASELINE";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            FCPoint mp = getTouchOverPoint();
            //获取参数
            float[] param = getRaseLineParams(m_marks);
            //判断选中
            if (param != null)
            {
                if (mp.y - param[0] * mp.x - param[1] >= m_lineWidth * -5 && mp.y - param[0] * mp.x - param[1] <= m_lineWidth * 5)
                {
                    action = ActionType.MOVE;
                }
            }
            return action;
        }

        /// <summary>
        /// 获取直线的参数
        /// </summary>
        /// <param name="pList">点阵集合</param>
        /// <returns></returns>
        private float[] getRaseLineParams(HashMap<int, PlotMark> pList)
        {
            if (pList.size() == 0)
            {
                return null;
            }
            float y1 = py(pList.get(0).Value);
            int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            float x1 = px(bIndex);
            float a = -1;
            float b = y1 + x1;
            return new float[] { a, b };
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return createPoint(mp);
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                Cursor = FCCursors.Hand;
                m_startMarks.put(0, m_marks.get(0));
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取参数
            float[] param = getRaseLineParams(pList);
            float a = param[0];
            float b = param[1];
            float leftX = 0;
            float leftY = leftX * a + b;
            float rightX = WorkingAreaWidth;
            float rightY = rightX * a + b;
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, leftX, leftY, rightX, rightY);
        }
    }

    /// <summary>
    /// 射线
    /// </summary>
    public sealed class Ray : FCPlot
    {
        /// <summary>
        /// 创建射线
        /// </summary>
        public Ray()
        {
            PlotType = "RAY";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            FCPoint mp = getTouchOverPoint();
            //判断选中
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            float y1 = py(m_marks.get(0).Value);
            float y2 = py(m_marks.get(1).Value);
            //获取直线参数
            float[] param = getLineParams(m_marks.get(0), m_marks.get(1));
            if (param != null)
            {
                if (mp.x >= x1 - m_lineWidth * 2.5f && mp.x <= x1 + m_lineWidth * 2.5f)
                {
                    action = ActionType.AT1;
                    return action;
                }
                else if (mp.x >= x2 - m_lineWidth * 2.5f && mp.x <= x2 + m_lineWidth * 2.5f)
                {
                    action = ActionType.AT2;
                    return action;
                }
            }
            else
            {
                if (mp.y >= y1 - m_lineWidth * 5 && mp.y <= y1 + m_lineWidth * 5)
                {
                    action = ActionType.AT1;
                    return action;
                }
                else if (mp.y >= y2 - m_lineWidth * 5 && mp.y <= y2 + m_lineWidth * 5)
                {
                    action = ActionType.AT2;
                    return action;
                }
            }
            if (selectRay(mp, x1, y1, x2, y2))
            {
                action = ActionType.MOVE;
            }
            return action;
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return create2PointsA(mp);
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.MOVE)
                {
                    Cursor = FCCursors.Hand;
                }
                else
                {
                    Cursor = FCCursors.SizeWE;
                }
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取相对位置
            float y1 = py(pList.get(0).Value);
            float y2 = py(pList.get(1).Value);
            int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(pList.get(1).Key);
            float k = 0;
            float b = 0;
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            lineXY(x1, y1, x2, y2, 0, 0, ref k, ref b);
            drawRay(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2, k, b);
            //画选中点
            if (Selected)
            {
                drawSelect(paint, lineColor, x1, y1);
                drawSelect(paint, lineColor, x2, y2);
            }
        }
    }

    /// <summary>
    /// 矩形
    /// </summary>
    public class Rect : FCPlot
    {
        /// <summary>
        /// 创建矩形
        /// </summary>
        public Rect()
        {
            PlotType = "FCRect";
        }

        /// <summary>
        /// 对面点的坐标
        /// </summary>
        private FCPoint oppositePoint = new FCPoint();

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            FCPoint mp = getTouchOverPoint();
            action = selectRect(mp, m_marks.get(0), m_marks.get(1));
            return action;
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>初始化是否成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return create2PointsB(mp);
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            if (m_action != ActionType.MOVE && m_action != ActionType.NO)
            {
                //获取四个点的位置
                FCRect rect = getRectangle(m_marks.get(0), m_marks.get(1));
                int x1 = rect.left;
                int y1 = rect.top;
                int x2 = rect.right;
                int y2 = rect.top;
                int x3 = rect.left;
                int y3 = rect.bottom;
                int x4 = rect.right;
                int y4 = rect.bottom;
                //根据动作类型设置变量
                switch (m_action)
                {
                    case ActionType.AT1:
                        oppositePoint = new FCPoint(x4, y4);
                        break;
                    case ActionType.AT2:
                        oppositePoint = new FCPoint(x3, y3);
                        break;
                    case ActionType.AT3:
                        oppositePoint = new FCPoint(x2, y2);
                        break;
                    case ActionType.AT4:
                        oppositePoint = new FCPoint(x1, y1);
                        break;
                }
            }
            m_moveTimes++;
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.MOVE)
                {
                    Cursor = FCCursors.Hand;
                }
                else
                {
                    Cursor = FCCursors.SizeNS;
                }
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
            }
        }

        /// <summary>
        /// 移动线条
        /// </summary>
        public override void onMoving()
        {
            FCPoint mp = getMovingPoint();
            //根据不同类型作出动作
            switch (m_action)
            {
                case ActionType.MOVE:
                    move(mp);
                    break;
                case ActionType.AT1:
                case ActionType.AT2:
                case ActionType.AT3:
                case ActionType.AT4:
                    resize(mp, oppositePoint);
                    break;
            }
        }

        /// <summary>
        /// 绘制图像的残影
        /// </summary>
        /// <param name="paint">绘图对象</param>
        public override void onPaintGhost(FCPaint paint)
        {
            if (m_moveTimes > 1)
            {
                onPaint(paint, m_startMarks, SelectedColor);
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            FCRect rect = getRectangle(pList.get(0), pList.get(1));
            //画矩形
            if (rect.right - rect.left > 0 && rect.bottom - rect.top > 0)
            {
                drawRect(paint, lineColor, m_lineWidth, m_lineStyle, rect.left, rect.top, rect.right, rect.bottom);
            }
            //画选中的点
            if (Selected)
            {
                drawSelect(paint, lineColor, rect.left, rect.top);
                drawSelect(paint, lineColor, rect.right, rect.top);
                drawSelect(paint, lineColor, rect.left, rect.bottom);
                drawSelect(paint, lineColor, rect.right, rect.bottom);
            }
        }
    }

    /// <summary>
    /// 线段
    /// </summary>
    public sealed class Segment : FCPlot
    {
        /// <summary>
        /// 创建线段
        /// </summary>
        public Segment()
        {
            PlotType = "SEGMENT";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            FCPoint mp = getTouchOverPoint();
            //获取点的位置
            float y1 = py(m_marks.get(0).Value);
            float y2 = py(m_marks.get(1).Value);
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            //获取参数
            float[] param = getLineParams(m_marks.get(0), m_marks.get(1));
            //产生动作类型
            if (param != null)
            {
                if (mp.x >= x1 - m_lineWidth * 2.5f && mp.x <= x1 + m_lineWidth * 2.5f)
                {
                    action = ActionType.AT1;
                    return action;
                }
                else if (mp.x >= x2 - m_lineWidth * 2.5f && mp.x <= x2 + m_lineWidth * 2.5f)
                {
                    action = ActionType.AT2;
                    return action;
                }
            }
            else
            {
                if (mp.y >= y1 - m_lineWidth * 5 && mp.y <= y1 + m_lineWidth * 5)
                {
                    action = ActionType.AT1;
                    return action;
                }
                else if (mp.y >= y2 - m_lineWidth * 5 && mp.y <= y2 + m_lineWidth * 5)
                {
                    action = ActionType.AT2;
                    return action;
                }
            }
            if (selectSegment(mp, x1, y1, x2, y2))
            {
                action = ActionType.MOVE;
            }
            return action;
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return create2PointsB(mp);
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.MOVE)
                {
                    Cursor = FCCursors.Hand;
                }
                else
                {
                    Cursor = FCCursors.SizeWE;
                }
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取相对位置  
            float y1 = py(pList.get(0).Value);
            float y2 = py(pList.get(1).Value);
            int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(pList.get(1).Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
            //画选中点
            if (Selected)
            {
                drawSelect(paint, lineColor, x1, y1);
                drawSelect(paint, lineColor, x2, y2);
            }
        }
    }

    /// <summary>
    /// 正弦线
    /// </summary>
    public sealed class Sine : FCPlot
    {
        /// <summary>
        /// 创建正弦线
        /// </summary>
        public Sine()
        {
            PlotType = "SINE";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            FCPoint mp = getTouchOverPoint();
            //获取选中点
            float y1 = py(m_marks.get(0).Value);
            float y2 = py(m_marks.get(1).Value);
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            //判断选中
            if (selectPoint(mp, x1, y1))
            {
                action = ActionType.AT1;
                return action;
            }
            else if (selectPoint(mp, x2, y2))
            {
                action = ActionType.AT2;
                return action;
            }
            if (selectSine(mp, x1, y1, x2, y2))
            {
                action = ActionType.MOVE;
            }
            return action;
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            int rIndex = m_dataSource.RowsCount;
            if (rIndex > 0)
            {
                FCChart chart = Chart;
                int touchIndex = chart.getTouchOverIndex();
                if (touchIndex >= 0 && touchIndex <= chart.LastVisibleIndex)
                {
                    int eIndex = touchIndex;
                    int bIndex = eIndex - chart.MaxVisibleRecord / 10;
                    if (bIndex >= 0 && eIndex != bIndex)
                    {
                        double fDate = m_dataSource.getXValue(bIndex);
                        double sDate = m_dataSource.getXValue(eIndex);
                        m_marks.clear();
                        double y = getNumberValue(mp);
                        m_marks.put(0, new PlotMark(0, fDate, y + (m_div.getVScale(m_attachVScale).VisibleMax - m_div.getVScale(m_attachVScale).VisibleMin) / 4));
                        m_marks.put(1, new PlotMark(1, sDate, y));
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.AT1)
                {
                    Cursor = FCCursors.SizeNS;
                }
                else if (m_action == ActionType.AT2)
                {
                    Cursor = FCCursors.SizeWE;
                }
                else
                {
                    Cursor = FCCursors.Hand;
                }
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
            }
        }

        /// <summary>
        /// 移动方法
        /// </summary>
        public override void onMoving()
        {
            FCPoint mp = getMovingPoint();
            //获取当前的索引和y值
            FCChart chart = Chart;
            int touchIndex = chart.getTouchOverIndex();
            if (touchIndex < 0)
            {
                touchIndex = 0;
            }
            if (touchIndex > chart.LastVisibleIndex)
            {
                touchIndex = chart.LastVisibleIndex;
            }
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
            //根据不同类型作出动作
            switch (m_action)
            {
                case ActionType.MOVE:
                    move(mp);
                    break;
                case ActionType.AT1:
                    if (touchIndex < eIndex)
                    {
                        resize(0);
                    }
                    break;
                case ActionType.AT2:
                    if (touchIndex > bIndex)
                    {
                        resize(1);
                    }
                    break;
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取相对位置
            double fValue = pList.get(0).Value;
            double eValue = pList.get(1).Value;
            int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(pList.get(1).Key);
            int x1 = (int)px(bIndex);
            float x2 = px(eIndex);
            float y1 = py(fValue);
            float y2 = py(eValue);
            //画线
            double f = 2.0 * Math.PI / ((x2 - x1) * 4);
            if (x1 != x2)
            {
                int len = WorkingAreaWidth;
                if (len > 0)
                {
                    FCPoint[] pf = new FCPoint[len];
                    for (int i = 0; i < len; i++)
                    {
                        int x = -x1 + i;
                        float y = (float)(0.5 * (y2 - y1) * Math.Sin(x * f) * 2);
                        FCPoint pt = new FCPoint((int)(x + x1), (int)(y + y1));
                        pf[i] = pt;
                    }
                    drawPolyline(paint, lineColor, m_lineWidth, m_lineStyle, pf);
                }
            }
            //画选中点
            if (Selected)
            {
                drawSelect(paint, lineColor, x1, y1);
                drawSelect(paint, lineColor, x2, y2);
            }
        }
    }

    /// <summary>
    /// 速阻线
    /// </summary>
    public sealed class SpeedResist : FCPlot
    {
        /// <summary>
        /// 创建速阻线
        /// </summary>
        public SpeedResist()
        {
            PlotType = "SPEEDRESIST";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            //获取点的位置
            FCPoint mp = getTouchOverPoint();
            float y1 = py(m_marks.get(0).Value);
            float y2 = py(m_marks.get(1).Value);
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            if (selectPoint(mp, x1, y1) || m_moveTimes == 1)
            {
                action = ActionType.AT1;
                return action;
            }
            else if (selectPoint(mp, x2, y2))
            {
                action = ActionType.AT2;
                return action;
            }
            FCPointF firstP = new FCPointF(x2, y2 - (y2 - y1) / 3);
            FCPointF secondP = new FCPointF(x2, y2 - (y2 - y1) * 2 / 3);
            FCPointF startP = new FCPointF(x1, y1);
            float oK = 0, oB = 0, fK = 0, fB = 0, sK = 0, sB = 0;
            //获取直线参数
            lineXY(x1, y1, x2, y2, 0, 0, ref oK, ref oB);
            lineXY(startP.x, startP.y, firstP.x, firstP.y, 0, 0, ref fK, ref fB);
            lineXY(startP.x, startP.y, secondP.x, secondP.y, 0, 0, ref sK, ref sB);
            float smallX = x1 <= x2 ? x1 : x2;
            float smallY = y1 <= y2 ? y1 : y2;
            float bigX = x1 > x2 ? x1 : x2;
            float bigY = y1 > y2 ? y1 : y2;
            if (mp.x >= smallX - 2 && mp.x <= bigX + 2 && mp.y >= smallY - 2 && mp.y <= bigY + 2)
            {
                if (!(oK == 0 && oB == 0))
                {
                    if (mp.y / (mp.x * oK + oB) >= 0.9 && mp.y / (mp.x * oK + oB) <= 1.1)
                    {
                        action = ActionType.MOVE;
                        return action;
                    }
                }
                else
                {
                    action = ActionType.MOVE;
                    return action;
                }
            }
            if ((x2 > x1 && mp.x >= x1 - 2) || (mp.x <= x1 + 2 && x2 < x1))
            {
                if (!(fK == 0 && fB == 0))
                {
                    if (mp.y / (mp.x * fK + fB) >= 0.9 && mp.y / (mp.x * fK + fB) <= 1.1)
                    {
                        action = ActionType.MOVE;
                        return action;
                    }
                }
                if (!(sK == 0 && sB == 0))
                {
                    if (mp.y / (mp.x * sK + sB) >= 0.9 && mp.y / (mp.x * sK + sB) <= 1.1)
                    {
                        action = ActionType.MOVE;
                        return action;
                    }
                }
            }
            return action;
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return create2PointsA(mp);
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_moveTimes++;
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.MOVE)
                {
                    Cursor = FCCursors.Hand;
                }
                else
                {
                    Cursor = FCCursors.SizeNS;
                }
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
            }
        }

        /// <summary>
        /// 绘制图像的残影
        /// </summary>
        /// <param name="paint">绘图对象</param>
        public override void onPaintGhost(FCPaint paint)
        {
            if (m_moveTimes > 1)
            {
                onPaint(paint, m_startMarks, SelectedColor);
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取相对位置
            float y1 = py(pList.get(0).Value);
            float y2 = py(pList.get(1).Value);
            int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(pList.get(1).Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            drawLine(paint, lineColor, m_lineWidth, 1, x1, y1, x2, y2);
            //画选中点
            if (Selected || (x1 == x2))
            {
                drawSelect(paint, lineColor, x1, y1);
                drawSelect(paint, lineColor, x2, y2);
            }
            if (x1 != x2 && y1 != y2)
            {
                FCPoint firstP = new FCPoint(x2, y2 - (y2 - y1) / 3);
                FCPoint secondP = new FCPoint(x2, y2 - (y2 - y1) * 2 / 3);
                FCPoint startP = new FCPoint(x1, y1);
                float fK = 0, fB = 0, sK = 0, sB = 0;
                //获取直线参数
                lineXY(startP.x, startP.y, firstP.x, firstP.y, 0, 0, ref fK, ref fB);
                lineXY(startP.x, startP.y, secondP.x, secondP.y, 0, 0, ref sK, ref sB);
                float newYF = 0, newYS = 0;
                float newX = 0;
                if (x2 > x1)
                {
                    newYF = fK * WorkingAreaWidth + fB;
                    newYS = sK * WorkingAreaWidth + sB;
                    newX = WorkingAreaWidth;
                }
                else
                {
                    newYF = fB;
                    newYS = sB;
                    newX = 0;
                }
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, startP.x, startP.y, newX, newYF);
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, startP.x, startP.y, newX, newYS);
            }
        }
    }

    /// <summary>
    /// 标准误差通道
    /// </summary>
    public class SeChannel : FCPlot
    {
        /// <summary>
        /// 创建标准误差通道
        /// </summary>
        public SeChannel()
        {
            PlotType = "SECHANNEL";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            FCPoint mp = getTouchOverPoint();
            //获取点的位置
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            //获取参数
            float[] param = getLRParams(m_marks);
            if (param != null)
            {
                float a = param[0];
                float b = param[1];
                double leftValue = a + b;
                double rightValue = (eIndex - bIndex + 1) * a + b;
                float y1 = py(leftValue);
                float y2 = py(rightValue);
                if (selectPoint(mp, x1, y1))
                {
                    action = ActionType.AT1;
                    return action;
                }
                else if (selectPoint(mp, x2, y2))
                {
                    action = ActionType.AT2;
                    return action;
                }
                FCChart chart = Chart;
                int touchIndex = chart.getTouchOverIndex();
                if (touchIndex >= bIndex && touchIndex <= chart.LastVisibleIndex)
                {
                    //判断选中
                    double yValue = a * ((touchIndex - bIndex) + 1) + b;
                    float y = py(yValue);
                    float x = px(touchIndex);
                    if (selectPoint(mp, x, y))
                    {
                        action = ActionType.MOVE;
                        return action;
                    }
                    double sd = getSEChannelSD(m_marks);
                    yValue = a * ((touchIndex - bIndex) + 1) + b + sd;
                    y = py(yValue);
                    x = px(touchIndex);
                    if (selectPoint(mp, x, y))
                    {
                        action = ActionType.MOVE;
                        return action;
                    }
                    yValue = a * ((touchIndex - bIndex) + 1) + b - sd;
                    y = py(yValue);
                    x = px(touchIndex);
                    if (selectPoint(mp, x, y))
                    {
                        action = ActionType.MOVE;
                        return action;
                    }
                }
            }
            return action;
        }

        /// <summary>
        /// 获取标准误差通道的标准差值
        /// </summary>
        /// <param name="pList">点阵集合</param>
        /// <returns>标准误差通道的参数</returns>
        private double getSEChannelSD(HashMap<int, PlotMark> pList)
        {
            if (m_sourceFields != null && m_sourceFields.containsKey("CLOSE"))
            {
                int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
                int eIndex = m_dataSource.getRowIndex(pList.get(1).Key);
                int len = eIndex - bIndex + 1;
                if (len > 0)
                {
                    double[] ary = new double[len];
                    for (int i = 0; i < len; i++)
                    {
                        double close = m_dataSource.get2(i + bIndex, m_sourceFields.get("CLOSE"));
                        if (!double.IsNaN(close))
                        {
                            ary[i] = close;
                        }
                    }
                    double avg = FCScript.avgValue(ary, len);
                    //求标准差
                    double sd = FCScript.standardDeviation(ary, len, avg, 2);
                    return sd;
                }
            }
            return 0;
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return create2CandlePoints(mp);
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.MOVE)
                {
                    Cursor = FCCursors.Hand;
                }
                else
                {
                    Cursor = FCCursors.SizeWE;
                }
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
            }
        }

        /// <summary>
        /// 移动线条
        /// </summary>
        public override void onMoving()
        {
            FCPoint mp = getMovingPoint();
            //获取当前的索引和y值
            FCChart chart = Chart;
            int touchIndex = chart.getTouchOverIndex();
            if (touchIndex < 0)
            {
                touchIndex = 0;
            }
            if (touchIndex > chart.LastVisibleIndex)
            {
                touchIndex = chart.LastVisibleIndex;
            }
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
            //根据不同类型作出动作
            switch (m_action)
            {
                case ActionType.MOVE:
                    move(mp);
                    break;
                case ActionType.AT1:
                    if (touchIndex < eIndex)
                    {
                        resize(0);
                    }
                    break;
                case ActionType.AT2:
                    if (touchIndex > bIndex)
                    {
                        resize(1);
                    }
                    break;
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            FCChart chart = Chart;
            //获取相对位置
            int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(pList.get(1).Key); 
            float x1 = px(chart.getX(bIndex));
            float x2 = px(chart.getX(eIndex));
            //获取直线参数
            float[] param = getLRParams(pList);
            if (param != null)
            {
                //画线条和选中点
                float a = param[0];
                float b = param[1];
                double leftValue = a + b;
                double rightValue = (eIndex - bIndex + 1) * a + b;
                float y1 = py(leftValue);
                float y2 = py(rightValue);
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
                double sd = getSEChannelSD(pList);
                double leftTop = leftValue + sd;
                double rightTop = rightValue + sd;
                double leftBottom = leftValue - sd;
                double rightBottom = rightValue - sd;
                float leftTopY = py(leftTop);
                float rightTopY = py(rightTop);
                float leftBottomY = py(leftBottom);
                float rightBottomY = py(rightBottom);
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, leftTopY, x2, rightTopY);
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, leftBottomY, x2, rightBottomY);
                rightValue = (chart.LastVisibleIndex + 1 - bIndex) * a + b;
                float x3 = (float)((chart.LastVisibleIndex - chart.FirstVisibleIndex) * chart.HScalePixel + chart.HScalePixel / 2);
                double dashTop = rightValue + sd;
                double dashBottom = rightValue - sd;
                float mValueY = py(rightValue);
                float dashTopY = py(dashTop);
                float dashBottomY = py(dashBottom);
                drawLine(paint, lineColor, m_lineWidth, 1, x2, rightTopY, x3, dashTopY);
                drawLine(paint, lineColor, m_lineWidth, 1, x2, rightBottomY, x3, dashBottomY);
                drawLine(paint, lineColor, m_lineWidth, 1, x2, y2, x3, mValueY);
                //画选中点
                if (Selected)
                {
                    drawSelect(paint, lineColor, x1, y1);
                    drawSelect(paint, lineColor, x2, y2);
                }
            }
        }
    }

    /// <summary>
    /// 对称线
    /// </summary>
    public sealed class SymmetricLine : FCPlot
    {
        /// <summary>
        /// 创建对称线
        /// </summary>
        public SymmetricLine()
        {
            PlotType = "SYMMETRICLINE";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            FCPoint mp = getTouchOverPoint();
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            float y1 = py(m_marks.get(0).Value);
            float y2 = py(m_marks.get(1).Value);
            if (selectPoint(mp, x1, y1))
            {
                action = ActionType.AT1;
                return action;
            }
            else if (selectPoint(mp, x2, y2))
            {
                action = ActionType.AT2;
                return action;
            }
            //获取对称索引
            int cIndex = 0;
            if (x2 >= x1)
            {
                cIndex = bIndex - (eIndex - bIndex);
            }
            else
            {
                cIndex = bIndex + (bIndex - eIndex);
            }
            if (cIndex > m_dataSource.RowsCount - 1)
            {
                cIndex = m_dataSource.RowsCount - 1;
            }
            else if (cIndex < 0)
            {
                cIndex = 0;
            }
            float x3 = px(cIndex);
            if ((mp.x >= x1 - m_lineWidth * 5 && mp.x <= x1 + m_lineWidth * 5)
            || (mp.x >= x2 - m_lineWidth * 5 && mp.x <= x2 + m_lineWidth * 5)
            || (mp.x >= x3 - m_lineWidth * 5 && mp.x <= x3 + m_lineWidth * 5))
            {
                action = ActionType.MOVE;
            }
            return action;
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return create2PointsA(mp);
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.MOVE)
                {
                    Cursor = FCCursors.Hand;
                }
                else
                {
                    Cursor = FCCursors.SizeWE;
                }
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取相对位置
            int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(pList.get(1).Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            float y1 = py(pList.get(0).Value);
            float y2 = py(pList.get(1).Value);
            //获取对称索引
            int cIndex = -1;
            if (x2 >= x1)
            {
                cIndex = bIndex - (eIndex - bIndex);
            }
            else
            {
                cIndex = bIndex + (bIndex - eIndex);
            }
            if (cIndex > m_dataSource.RowsCount - 1)
            {
                cIndex = m_dataSource.RowsCount - 1;
            }
            else if (cIndex < 0)
            {
                cIndex = 0;
            }
            float x3 = px(cIndex);
            //画线
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, 0, x1, WorkingAreaHeight);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x2, 0, x2, WorkingAreaHeight);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x3, 0, x3, WorkingAreaHeight);
            //画选中点
            if (Selected)
            {
                drawSelect(paint, lineColor, x1, y1);
                drawSelect(paint, lineColor, x2, y2);
            }
        }
    }

    /// <summary>
    /// 对称三角形
    /// </summary>
    public class SymemetrictriAngle : FCPlot
    {
        /// <summary>
        /// 创建对称三角形
        /// </summary>
        public SymemetrictriAngle()
        {
            PlotType = "SYMMETRICTRIANGLE";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            FCPoint mp = getTouchOverPoint();
            float y1 = py(m_marks.get(0).Value);
            float y2 = py(m_marks.get(1).Value);
            float y3 = py(m_marks.get(2).Value);
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
            int pIndex = m_dataSource.getRowIndex(m_marks.get(2).Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            float x3 = px(pIndex);
            //获取参数
            float[] param = getSymmetricTriangleParams(m_marks);
            //非垂直
            if (param != null)
            {
                if (selectPoint(mp, x1, y1))
                {
                    action = ActionType.AT1;
                    return action;
                }
                else if (selectPoint(mp, x2, y2))
                {
                    action = ActionType.AT2;
                    return action;
                }
                else if (selectPoint(mp, x3, y3))
                {
                    action = ActionType.AT3;
                    return action;
                }
                //主直线
                if (mp.y - param[0] * mp.x - param[1] >= m_lineWidth * -5 && mp.y - param[0] * mp.x - param[1] <= m_lineWidth * 5)
                {
                    action = ActionType.MOVE;
                    return action;
                }
                //对称三角线
                if (mp.y - param[2] * mp.x - param[3] >= m_lineWidth * -5 && mp.y - param[2] * mp.x - param[3] <= m_lineWidth * 5)
                {
                    action = ActionType.MOVE;
                    return action;
                }
            }
            //垂直
            else
            {
                if (mp.y >= y1 - m_lineWidth * 5 && mp.y <= y1 + m_lineWidth * 5)
                {
                    action = ActionType.AT1;
                    return action;
                }
                else if (mp.y >= y2 - m_lineWidth * 5 && mp.y <= y2 + m_lineWidth * 5)
                {
                    action = ActionType.AT2;
                    return action;
                }
                else if (mp.y >= y3 - m_lineWidth * 5 && mp.y <= y3 + m_lineWidth * 5)
                {
                    action = ActionType.AT3;
                    return action;
                }
                if (mp.x >= x1 - m_lineWidth * 5 && mp.x <= x1 + m_lineWidth * 5)
                {
                    action = ActionType.MOVE;
                    return action;
                }
                if (mp.x >= x3 - m_lineWidth * 5 && mp.x <= x3 + m_lineWidth * 5)
                {
                    action = ActionType.MOVE;
                    return action;
                }
            }
            return action;
        }

        /// <summary>
        /// 获取对称三角形直线的参数
        /// </summary>
        /// <param name="pList">点阵集合</param>
        /// <returns>对称三角形直线的参数</returns>
        private float[] getSymmetricTriangleParams(HashMap<int, PlotMark> pList)
        {
            if (pList.size() == 0)
            {
                return null;
            }
            float y1 = py(pList.get(0).Value);
            float y2 = py(pList.get(1).Value);
            float y3 = py(pList.get(2).Value);
            int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(pList.get(1).Key);
            int pIndex = m_dataSource.getRowIndex(pList.get(2).Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            float x3 = px(pIndex);
            float a = 0;
            if (x2 - x1 != 0)
            {
                a = (y2 - y1) / (x2 - x1);
                float b = y1 - a * x1;
                float c = -a;
                float d = y3 - c * x3;
                return new float[] { a, b, c, d };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return create3Points(mp);
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.MOVE)
                {
                    Cursor = FCCursors.Hand;
                }
                else
                {
                    Cursor = FCCursors.SizeWE;
                }
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
                m_startMarks.put(2, m_marks.get(2));
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取相对位置
            float y1 = py(pList.get(0).Value);
            float y2 = py(pList.get(1).Value);
            float y3 = py(pList.get(2).Value);
            int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(pList.get(1).Key);
            int pIndex = m_dataSource.getRowIndex(pList.get(2).Key);
            //获取参数
            float[] param = getSymmetricTriangleParams(pList);
            float a = 0;
            float b = 0;
            float c = 0;
            float d = 0;
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            float x3 = px(pIndex);
            //非垂直
            if (param != null)
            {
                a = param[0];
                b = param[1];
                c = param[2];
                d = param[3];
                float leftX = 0;
                float leftY = leftX * a + b;
                float rightX = WorkingAreaWidth;
                float rightY = rightX * a + b;
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, leftX, leftY, rightX, rightY);
                leftY = leftX * c + d;
                rightY = rightX * c + d;
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, leftX, leftY, rightX, rightY);
            }
            //垂直
            else
            {
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, 0, x1, WorkingAreaHeight);
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x3, 0, x3, WorkingAreaHeight);
            }
            //画选中点
            if (Selected)
            {
                drawSelect(paint, lineColor, x1, y1);
                drawSelect(paint, lineColor, x2, y2);
                drawSelect(paint, lineColor, x3, y3);
            }
        }
    }

    /// <summary>
    /// 时间尺
    /// </summary>
    public sealed class TimeRuler : FCPlot
    {
        /// <summary>
        /// 创建时间尺
        /// </summary>
        public TimeRuler()
        {
            PlotType = "TIMERULER";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            //获取点位置
            FCPoint mp = getTouchOverPoint();
            float y1 = py(m_marks.get(0).Value);
            float y2 = py(m_marks.get(1).Value);
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            if (selectPoint(mp, x1, y1))
            {
                action = ActionType.AT1;
                return action;
            }
            else if (selectPoint(mp, x2, y2))
            {
                action = ActionType.AT2;
                return action;
            }
            //获取参数
            double[] param = getTimeRulerParams(m_marks);
            float yBHigh = py(param[0]);
            float yBLow = py(param[1]);
            float yEHigh = py(param[2]);
            float yELow = py(param[3]);
            //判断选中
            if (y1 < yBHigh)
            {
                yBHigh = y1;
            }
            if (y1 > yBLow)
            {
                yBLow = y1;
            }
            if (y2 < yEHigh)
            {
                yEHigh = y2;
            }
            if (y2 > yELow)
            {
                yELow = y2;
            }
            if (mp.x >= x1 - m_lineWidth * 2.5f && mp.x <= x1 + m_lineWidth * 2.5f)
            {
                if (mp.y >= yBHigh - 2 && mp.y <= yBLow + 2)
                {
                    action = ActionType.MOVE;
                    return action;
                }
            }
            if (mp.x >= x2 - m_lineWidth * 2.5f && mp.x <= x2 + m_lineWidth * 2.5f)
            {
                if (mp.y >= yEHigh - m_lineWidth * 2.5f && mp.y <= yELow + m_lineWidth * 2.5f)
                {
                    action = ActionType.MOVE;
                    return action;
                }
            }
            if (mp.y >= y1 - m_lineWidth * 2.5f && mp.y <= y1 + m_lineWidth * 2.5f)
            {
                float bigX = x1 >= x2 ? x1 : x2;
                float smallX = x1 < x2 ? x1 : x2;
                if (mp.x >= smallX - m_lineWidth * 2.5f && mp.x <= bigX + m_lineWidth * 2.5f)
                {
                    action = ActionType.MOVE;
                    return action;
                }
            }
            return action;
        }

        /// <summary>
        /// 获取时间尺的参数
        /// </summary>
        /// <param name="pList">点阵集合</param>
        /// <returns>时间尺的参数</returns>
        private double[] getTimeRulerParams(HashMap<int, PlotMark> pList)
        {
            if (pList.size() == 0)
            {
                return null;
            }
            FCChart chart = Chart;
            int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(pList.get(1).Key);
            double bHigh = chart.divMaxOrMin(bIndex, m_div, 0);
            double bLow = chart.divMaxOrMin(bIndex, m_div, 1);
            double eHigh = chart.divMaxOrMin(eIndex, m_div, 0);
            double eLow = chart.divMaxOrMin(eIndex, m_div, 1);
            return new double[] { bHigh, bLow, eHigh, eLow };
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return create2PointsB(mp);
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.MOVE)
                {
                    Cursor = FCCursors.Hand;
                }
                else
                {
                    Cursor = FCCursors.SizeNS;
                }
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
            }
        }

        /// <summary>
        /// 移动线条
        /// </summary>
        public override void onMoving()
        {
            FCPoint mp = getMovingPoint();
            //获取当前的索引和y值
            FCChart chart = Chart;
            int touchIndex = chart.getTouchOverIndex();
            double y = getNumberValue(mp);
            if (touchIndex < 0)
            {
                touchIndex = 0;
            }
            if (touchIndex > chart.LastVisibleIndex)
            {
                touchIndex = chart.LastVisibleIndex;
            }
            //根据不同类型作出动作
            switch (m_action)
            {
                case ActionType.MOVE:
                    move(mp);
                    break;
                case ActionType.AT1:
                    m_marks.put(0, new PlotMark(0, m_dataSource.getXValue(touchIndex), y));
                    m_marks.put(1, new PlotMark(1, m_marks.get(1).Key, y));
                    break;
                case ActionType.AT2:
                    m_marks.put(1, new PlotMark(1, m_dataSource.getXValue(touchIndex), y));
                    m_marks.put(0, new PlotMark(0, m_marks.get(0).Key, y));
                    break;
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取相对位置
            float y1 = py(pList.get(0).Value);
            float y2 = py(pList.get(1).Value);
            int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(pList.get(1).Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            //获取参数
            double[] param = getTimeRulerParams(pList);
            float yBHigh = py(param[0]);
            float yBLow = py(param[1]);
            float yEHigh = py(param[2]);
            float yELow = py(param[3]);
            if (y1 < yBHigh)
            {
                yBHigh = y1;
            }
            if (y1 > yBLow)
            {
                yBLow = y1;
            }
            if (y2 < yEHigh)
            {
                yEHigh = y2;
            }
            if (y2 > yELow)
            {
                yELow = y2;
            }
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, yBHigh, x1, yBLow);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x2, yEHigh, x2, yELow);
            int subRecord = Math.Abs(eIndex - bIndex) + 1;
            //画文字
            String recordStr = subRecord.ToString() + "(T)";
            FCSize sizeF = textSize(paint, recordStr, m_font);
            drawText(paint, recordStr, lineColor, m_font, (x2 + x1) / 2 - sizeF.cx / 2, y1 - sizeF.cy / 2);
            if (Math.Abs(x1 - x2) > sizeF.cx)
            {
                if (x2 >= x1)
                {
                    drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, (x2 + x1) / 2 - sizeF.cx / 2, y1);
                    drawLine(paint, lineColor, m_lineWidth, m_lineStyle, (x2 + x1) / 2 + sizeF.cx / 2, y1, x2, y1);
                }
                else
                {
                    drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x2, y1, (x2 + x1) / 2 - sizeF.cx / 2, y1);
                    drawLine(paint, lineColor, m_lineWidth, m_lineStyle, (x2 + x1) / 2 + sizeF.cx / 2, y1, x1, y1);
                }
            }
            //画选中点
            if (Selected)
            {
                drawSelect(paint, lineColor, x1, y1);
                drawSelect(paint, lineColor, x2, y2);
            }
        }
    }

    /// <summary>
    /// 泰龙水平线
    /// </summary>
    public class TironeLevels : FCPlot
    {
        /// <summary>
        /// 创建泰龙水平线
        /// </summary>
        public TironeLevels()
        {
            PlotType = "TIRONELEVELS";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            //获取点位置
            FCPoint mp = getTouchOverPoint();
            float y1 = py(m_marks.get(0).Value);
            float y2 = py(m_marks.get(1).Value);
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            //获取参数
            double[] param = getTironelLevelsParams(m_marks);
            double nHigh = param[0];
            double nLow = param[4];
            float highY = py(nHigh);
            float lowY = py(nLow);
            //获取较大的X和较小的X
            float smallX = x1 > x2 ? x2 : x1;
            float bigX = x1 > x2 ? x1 : x2;
            if ((mp.y >= highY - m_lineWidth * 2.5f && mp.y <= highY + m_lineWidth * 2.5f)
            || (mp.y >= lowY - m_lineWidth * 2.5f && mp.y <= lowY + m_lineWidth * 2.5f))
            {
                if (mp.x >= x1 - m_lineWidth * 2.5f && mp.x <= x1 + m_lineWidth * 2.5f)
                {
                    action = ActionType.AT1;
                    return action;
                }
                else if (mp.x >= x2 - m_lineWidth * 2.5f && mp.x <= x2 + m_lineWidth * 2.5f)
                {
                    action = ActionType.AT2;
                    return action;
                }
            }
            //选中上面的线
            if (mp.y >= highY - m_lineWidth * 2.5f && mp.y <= highY + m_lineWidth * 2.5f)
            {
                if (mp.x >= smallX - m_lineWidth * 2.5f && mp.x <= bigX + m_lineWidth * 2.5f)
                {
                    action = ActionType.MOVE;
                    return action;
                }
            }
            //选中下面的线
            else if (mp.y >= lowY - m_lineWidth * 2.5f && mp.y <= lowY + m_lineWidth * 2.5f)
            {
                if (mp.x >= smallX - m_lineWidth * 2.5f && mp.x <= bigX + m_lineWidth * 2.5f)
                {
                    action = ActionType.MOVE;
                    return action;
                }
            }
            //水平线判断
            for (int i = 1; i < param.Length - 1; i++)
            {
                float y = py(param[i]);
                if (mp.y >= y - m_lineWidth * 2.5f && mp.y <= y + m_lineWidth * 2.5f)
                {
                    action = ActionType.MOVE;
                    return action;
                }
            }
            return action;
        }

        /// <summary>
        /// 获取泰龙水平线的参数
        /// </summary>
        /// <param name="pList">点阵集合</param>
        /// <returns>泰龙水平线的参数</returns>
        private double[] getTironelLevelsParams(HashMap<int, PlotMark> pList)
        {
            double[] hl = getCandleRange(pList);
            if (hl != null)
            {
                double nHigh = hl[0];
                double nLow = hl[1];
                return new double[] { nHigh, nHigh - (nHigh - nLow) / 3, nHigh - (nHigh - nLow) / 2, nHigh - 2 * (nHigh - nLow) / 3, nLow };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return create2CandlePoints(mp);
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.MOVE)
                {
                    Cursor = FCCursors.Hand;
                }
                else
                {
                    Cursor = FCCursors.SizeNS;
                }
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取相对位置
            float y1 = py(pList.get(0).Value);
            float y2 = py(pList.get(1).Value);
            int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(pList.get(1).Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            float smallX = x1 > x2 ? x2 : x1;
            float bigX = x1 > x2 ? x1 : x2;
            //获取参数
            double[] param = getTironelLevelsParams(pList);
            double nHigh = param[0];
            double nLow = param[4];
            float highY = py(nHigh);
            float lowY = py(nLow);
            float mid = x1 >= x2 ? (x2 + (x1 - x2) / 2) : (x1 + (x2 - x1) / 2);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, highY, x2, highY);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, lowY, x2, lowY);
            drawLine(paint, lineColor, m_lineWidth, 1, mid, lowY, mid, highY);
            for (int i = 1; i < param.Length - 1; i++)
            {
                float y = py(param[i]);
                //画直线
                drawLine(paint, lineColor, m_lineWidth, 1, 0, y, WorkingAreaWidth, y);
                String str = i.ToString() + "/3";
                if (i == 2)
                {
                    str = "1/2";
                }
                FCSize sizeF = textSize(paint, str, m_font);
                drawText(paint, str, lineColor, m_font, WorkingAreaWidth - sizeF.cx, y - sizeF.cy);
            }
            //画选中点
            if (Selected)
            {
                drawSelect(paint, lineColor, smallX, highY);
                drawSelect(paint, lineColor, smallX, lowY);
                drawSelect(paint, lineColor, bigX, highY);
                drawSelect(paint, lineColor, bigX, lowY);
            }
        }
    }

    /// <summary>
    /// 三角形
    /// </summary>
    public class Triangle : FCPlot
    {
        /// <summary>
        /// 创建三角形
        /// </summary>
        public Triangle()
        {
            PlotType = "TRIANGLE";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            //获取选中位置
            float y1 = py(m_marks.get(0).Value);
            float y2 = py(m_marks.get(1).Value);
            float y3 = py(m_marks.get(2).Value);
            int aIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int bIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
            int cIndex = m_dataSource.getRowIndex(m_marks.get(2).Key);
            float x1 = px(aIndex);
            float x2 = px(bIndex);
            float x3 = px(cIndex);
            FCPoint mp = getTouchOverPoint();
            if (m_moveTimes == 1)
            {
                action = ActionType.AT3;
                return action;
            }
            else
            {
                if (selectPoint(mp, x1, y1) || m_moveTimes == 1)
                {
                    action = ActionType.AT1;
                    return action;
                }
                else if (selectPoint(mp, x2, y2))
                {
                    action = ActionType.AT2;
                    return action;
                }
                else if (selectPoint(mp, x3, y3))
                {
                    action = ActionType.AT3;
                    return action;
                }
            }
            if (selectTriangle(mp, x1, y1, x2, y2, x3, y3))
            {
                action = ActionType.MOVE;
            }
            return action;
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            int rIndex = m_dataSource.RowsCount;
            if (rIndex > 0)
            {
                int currentIndex = getIndex(mp);
                double y = getNumberValue(mp);
                double date = m_dataSource.getXValue(currentIndex);
                m_marks.clear();
                m_marks.put(0, new PlotMark(0, date, y));
                int si = currentIndex + 10;
                FCChart chart = Chart;
                if (si > chart.LastVisibleIndex)
                {
                    si = chart.LastVisibleIndex;
                }
                m_marks.put(1, new PlotMark(1, m_dataSource.getXValue(si), y));
                m_marks.put(2, new PlotMark(2, date, y));
                return true;
            }
            return false;
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_moveTimes++;
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.MOVE)
                {
                    Cursor = FCCursors.Hand;
                }
                else
                {
                    Cursor = FCCursors.SizeWE;
                }
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
                m_startMarks.put(2, m_marks.get(2));
            }
        }

        /// <summary>
        /// 绘制图像的残影
        /// </summary>
        /// <param name="paint">绘图对象</param>
        public override void onPaintGhost(FCPaint paint)
        {
            if (m_moveTimes > 1)
            {
                onPaint(paint, m_startMarks, SelectedColor);
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取相对位置
            float y1 = py(pList.get(0).Value);
            float y2 = py(pList.get(1).Value);
            float y3 = py(pList.get(2).Value);
            int aIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            int bIndex = m_dataSource.getRowIndex(pList.get(1).Key);
            int cIndex = m_dataSource.getRowIndex(pList.get(2).Key);
            float x1 = px(aIndex);
            float x2 = px(bIndex);
            float x3 = px(cIndex);
            //画线
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x2, y2);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, y1, x3, y3);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x2, y2, x3, y3);
            //画选中点
            if (Selected || (x1 == x2 && x2 == x3 && y1 == y2 && y2 == y3))
            {
                drawSelect(paint, lineColor, x1, y1);
                drawSelect(paint, lineColor, x2, y2);
                drawSelect(paint, lineColor, x3, y3);
            }
        }
    }

    /// <summary>
    /// 上涨箭头
    /// </summary>
    public sealed class UpArrow : FCPlot
    {
        /// <summary>
        /// 创建上涨箭头
        /// </summary>
        public UpArrow()
        {
            PlotType = "UPARROW";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            //获取值和索引
            double fValue = m_marks.get(0).Value;
            int aIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            //获取横坐标和纵坐标
            float x1 = px(aIndex);
            float y1 = py(fValue);
            //获取点击区域
            int width = 10;
            FCRect rect = new FCRect(x1 - width / 2, y1, x1 + width / 2, y1 + width * 3 / 2);
            FCPoint mp = getTouchOverPoint();
            //判断是否选中
            if (mp.x > rect.left && mp.x <= rect.right && mp.y >= rect.top && mp.y <= rect.bottom)
            {
                action = ActionType.MOVE;
            }
            return action;
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                Cursor = FCCursors.Hand;
                m_startMarks.put(0, m_marks.get(0));
            }
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return createPoint(mp);
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取相对位置
            double fValue = pList.get(0).Value;
            int aIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            int x1 = (int)px(aIndex);
            int y1 = (int)py(fValue);
            int width = 10;
            FCPoint point1 = new FCPoint(x1, y1);
            FCPoint point2 = new FCPoint(x1 + width / 2, y1 + width);
            FCPoint point3 = new FCPoint(x1 + width / 4, y1 + width);
            FCPoint point4 = new FCPoint(x1 + width / 4, y1 + width * 3 / 2);
            FCPoint point5 = new FCPoint(x1 - width / 4, y1 + width * 3 / 2);
            FCPoint point6 = new FCPoint(x1 - width / 4, y1 + width);
            FCPoint point7 = new FCPoint(x1 - width / 2, y1 + width);
            FCPoint[] points = new FCPoint[7] { point1, point2, point3, point4, point5, point6, point7 };
            fillPolygon(paint, lineColor, points);
            //画选中点
            if (Selected)
            {
                drawSelect(paint, lineColor, x1 - width / 2, y1);
            }
        }
    }

    /// <summary>
    /// 垂直线
    /// </summary>
    public sealed class VLine : FCPlot
    {
        /// <summary>
        /// 创建垂直线
        /// </summary>
        public VLine()
        {
            PlotType = "VLINE";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            //判断选中
            FCPoint mp = getTouchOverPoint();
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            float x1 = px(bIndex);
            if (mp.x >= x1 - m_lineWidth * 2.5f && mp.x <= x1 + m_lineWidth * 2.5f)
            {
                action = ActionType.MOVE;
            }
            return action;
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return createPoint(mp);
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                Cursor = FCCursors.Hand;
                m_startMarks.put(0, m_marks.get(0));
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取相对位置
            int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            float x1 = px(bIndex);
            //画线
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, 0, x1, WorkingAreaHeight);
        }
    }

    /// <summary>
    /// 波浪尺
    /// </summary>
    public sealed class WaveRuler : FCPlot
    {
        /// <summary>
        /// 创建波浪尺
        /// </summary>
        public WaveRuler()
        {
            PlotType = "WAVERULER";
        }

        /// <summary>
        /// 获取动作类型
        /// </summary>
        /// <returns>动作类型</returns>
        public override ActionType getAction()
        {
            ActionType action = ActionType.NO;
            if (m_marks.size() == 0)
            {
                return action;
            }
            FCPoint mp = getTouchOverPoint();
            //获取参数不
            float[] param = getWaveRulerParams(m_marks.get(0).Value, m_marks.get(1).Value);
            int bIndex = m_dataSource.getRowIndex(m_marks.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(m_marks.get(1).Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            float y1 = py(m_marks.get(0).Value);
            float y2 = param[param.Length - 1];
            if (selectPoint(mp, x1, y1) || m_moveTimes == 1)
            {
                action = ActionType.AT1;
                return action;
            }
            else if (selectPoint(mp, x2, y2))
            {
                action = ActionType.AT2;
                return action;
            }
            //判断选中
            float smallY = param[0] < param[param.Length - 1] ? param[0] : param[param.Length - 1];
            float bigY = param[0] >= param[param.Length - 1] ? param[0] : param[param.Length - 1];
            float mid = x1 >= x2 ? (x2 + (x1 - x2) / 2) : (x1 + (x2 - x1) / 2);
            if (mp.x >= mid - m_lineWidth * 2.5f && mp.x <= mid + m_lineWidth * 2.5f && mp.y >= smallY - m_lineWidth * 2.5f && mp.y <= bigY + m_lineWidth * 2.5f)
            {
                action = ActionType.MOVE;
                return action;
            }
            float top = 0;
            float bottom = WorkingAreaWidth;
            if (mp.y >= top && mp.y <= bottom)
            {
                foreach (float p in param)
                {
                    if (mp.x >= 0 && mp.x <= WorkingAreaWidth && mp.y >= p - m_lineWidth * 2.5f && mp.y <= p + m_lineWidth * 2.5f)
                    {
                        action = ActionType.MOVE;
                        return action;
                    }
                }
            }
            return action;
        }

        /// <summary>
        /// 获取波浪尺参数
        /// </summary>
        /// <param name="value1">值1</param>
        /// <param name="value2">值2</param>
        /// <returns>波浪尺参数</returns>
        private float[] getWaveRulerParams(double value1, double value2)
        {
            float y1 = py(value1);
            float y2 = py(value2);
            float y0 = 0, yA = 0, yB = 0, yC = 0, yD = 0, yE = 0, yF = 0, yG = 0, yH = 0, yI = 0, yMax = 0;
            y0 = y1;
            yA = y1 <= y2 ? y1 + (y2 - y1) * (0.236f / 2.618f) : y2 + (y1 - y2) * (1 - 0.236f / 2.618f);
            yB = y1 <= y2 ? y1 + (y2 - y1) * (0.362f / 2.618f) : y2 + (y1 - y2) * (1 - 0.362f / 2.618f);
            yC = y1 <= y2 ? y1 + (y2 - y1) * (0.5f / 2.618f) : y2 + (y1 - y2) * (1 - 0.5f / 2.618f);
            yD = y1 <= y2 ? y1 + (y2 - y1) * (0.618f / 2.618f) : y2 + (y1 - y2) * (1 - 0.618f / 2.618f);
            yE = y1 <= y2 ? y1 + (y2 - y1) * (1 / 2.618f) : y2 + (y1 - y2) * (1 - 1 / 2.618f);
            yF = y1 <= y2 ? y1 + (y2 - y1) * (1.382f / 2.618f) : y2 + (y1 - y2) * (1 - 1.382f / 2.618f);
            yG = y1 <= y2 ? y1 + (y2 - y1) * (1.618f / 2.618f) : y2 + (y1 - y2) * (1 - 1.618f / 2.618f);
            yH = y1 <= y2 ? y1 + (y2 - y1) * (2 / 2.618f) : y2 + (y1 - y2) * (1 - 2 / 2.618f);
            yI = y1 <= y2 ? y1 + (y2 - y1) * (2.382f / 2.618f) : y2 + (y1 - y2) * (1 - 2.382f / 2.618f);
            yMax = y2;
            return new float[] { y0, yA, yB, yC, yD, yE, yF, yG, yH, yI, yMax };
        }

        /// <summary>
        /// 初始化线条
        /// </summary>
        /// <param name="mp">坐标</param>
        /// <returns>是否初始化成功</returns>
        public override bool onCreate(FCPoint mp)
        {
            return create2PointsB(mp);
        }

        /// <summary>
        /// 开始移动画线工具
        /// </summary>
        public override void onMoveStart()
        {
            m_moveTimes++;
            m_action = getAction();
            m_startMarks.clear();
            m_startPoint = getTouchOverPoint();
            if (m_action != ActionType.NO)
            {
                if (m_action == ActionType.MOVE)
                {
                    Cursor = FCCursors.Hand;
                }
                else
                {
                    Cursor = FCCursors.SizeWE;
                }
                m_startMarks.put(0, m_marks.get(0));
                m_startMarks.put(1, m_marks.get(1));
            }
        }

        /// <summary>
        /// 绘制图像的残影
        /// </summary>
        /// <param name="paint">绘图对象</param>
        public override void onPaintGhost(FCPaint paint)
        {
            if (m_moveTimes > 1)
            {
                onPaint(paint, m_startMarks, SelectedColor);
            }
        }

        /// <summary>
        /// 绘制图像的方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="pList">横纵值描述</param>
        /// <param name="lineColor">颜色</param>
        protected override void onPaint(FCPaint paint, HashMap<int, PlotMark> pList,long lineColor)
        {
            if (pList.size() == 0)
                return;
            //获取相对位置
            float y1 = py(pList.get(0).Value);
            float y2 = py(pList.get(1).Value);
            int bIndex = m_dataSource.getRowIndex(pList.get(0).Key);
            int eIndex = m_dataSource.getRowIndex(pList.get(1).Key);
            float x1 = px(bIndex);
            float x2 = px(eIndex);
            //画文字和线条
            float[] lineParam = getWaveRulerParams(pList.get(0).Value, pList.get(1).Value);
            String[] str = new String[] { "0.00%", "23.60%", "38.20%", "50.00%", "61.80%", "100.00%", "138.20%", "161.80%", "200%", "238.20%", "261.80%" };
            float mid = x1 >= x2 ? (x2 + (x1 - x2) / 2) : (x1 + (x2 - x1) / 2);
            drawLine(paint, lineColor, m_lineWidth, m_lineStyle, mid, lineParam[0], mid, lineParam[lineParam.Length - 1]);
            for (int i = 0; i < lineParam.Length; i++)
            {
                FCSize sizeF = textSize(paint, str[i], m_font);
                float yP = lineParam[i];
                drawLine(paint, lineColor, m_lineWidth, m_lineStyle, x1, yP, x2, yP); ;
                drawText(paint, str[i], lineColor, m_font, mid, yP - sizeF.cy);
            }
            //画选中点
            if (Selected)
            {
                drawSelect(paint, lineColor, x1, y1);
                drawSelect(paint, lineColor, x2, y2);
            }
        }
    }

    /// <summary>
    /// 画线工具工厂类
    /// </summary>
    public class PFactory {
        /// <summary>
        /// 根据类型创建线条
        /// </summary>
        /// <param name="plotType">类型</param>
        /// <returns>画线工具对象</returns>
        public static FCPlot createPlot(String plotType) {
            FCPlot iplot = null;
            switch (plotType) {
                case "ANDREWSPITCHFORK":
                    iplot = new AndrewsPitchfork();
                    break;
                case "ANGLELINE":
                    iplot = new Angleline();
                    break;
                case "CIRCUMCIRCLE":
                    iplot = new CircumCircle();
                    break;
                case "ARROWSEGMENT":
                    iplot = new ArrowSegment();
                    break;
                case "DOWNARROW":
                    iplot = new DownArrow();
                    break;
                case "DROPLINE":
                    iplot = new Dropline();
                    break;
                case "ELLIPSE":
                    iplot = new Ellipse();
                    break;
                case "FIBOELLIPSE":
                    iplot = new FiboEllipse();
                    break;
                case "FIBOFANLINE":
                    iplot = new FiboFanline();
                    break;
                case "FIBOTIMEZONE":
                    iplot = new FiboTimezone();
                    break;
                case "GANNBOX":
                    iplot = new GannBox();
                    break;
                case "GANNLINE":
                    iplot = new GannLine();
                    break;
                case "GOLDENRATIO":
                    iplot = new GoldenRatio();
                    break;
                case "HLINE":
                    iplot = new HLine();
                    break;
                case "LEVELGRADING":
                    iplot = new LevelGrading();
                    break;
                case "LINE":
                    iplot = new Line();
                    break;
                case "LRBAND":
                    iplot = new LrBand();
                    break;
                case "LRCHANNEL":
                    iplot = new LrChannel();
                    break;
                case "LRLINE":
                    iplot = new LrLine();
                    break;
                case "NULLPOINT":
                    iplot = new NullPoint();
                    break;
                case "PARALLEL":
                    iplot = new Parallel();
                    break;
                case "PERCENT":
                    iplot = new Percent();
                    break;
                case "PERIODIC":
                    iplot = new Periodic();
                    break;
                case "PRICE":
                    iplot = new Price();
                    break;
                case "RANGERULER":
                    iplot = new RangeRuler();
                    break;
                case "RASELINE":
                    iplot = new RaseLine();
                    break;
                case "RAY":
                    iplot = new Ray();
                    break;
                case "FCRect":
                    iplot = new Rect();
                    break;
                case "SEGMENT":
                    iplot = new Segment();
                    break;
                case "SINE":
                    iplot = new Sine();
                    break;
                case "SPEEDRESIST":
                    iplot = new SpeedResist();
                    break;
                case "SECHANNEL":
                    iplot = new SeChannel();
                    break;
                case "SYMMETRICLINE":
                    iplot = new SymmetricLine();
                    break;
                case "SYMMETRICTRIANGLE":
                    iplot = new SymemetrictriAngle();
                    break;
                case "TIMERULER":
                    iplot = new TimeRuler();
                    break;
                case "TRIANGLE":
                    iplot = new Triangle();
                    break;
                case "UPARROW":
                    iplot = new UpArrow();
                    break;
                case "VLINE":
                    iplot = new VLine();
                    break;
                case "WAVERULER":
                    iplot = new WaveRuler();
                    break;
                case "TIRONELEVELS":
                    iplot = new TironeLevels();
                    break;
                case "RAFFCHANNEL":
                    iplot = new RaffChannel();
                    break;
                case "QUADRANTLINES":
                    iplot = new QuadrantLines();
                    break;
                case "BOXLINE":
                    iplot = new BoxLine();
                    break;
                case "PARALLELOGRAM":
                    iplot = new ParalleGram();
                    break;
                case "CIRCLE":
                    iplot = new Circle();
                    break;
                case "PRICECHANNEL":
                    iplot = new PriceChannel();
                    break;
                case "GP":
                    iplot = new Gp();
                    break;
                case "GA":
                    iplot = new Ga();
                    break;
            }
            return iplot;
        }
    }
}
