/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.core;

/**
 * 绘图类
 */
public interface FCPaint {

    /**
     * 添加曲线
     *
     * @param rect 矩形区域
     * @param startAngle 从 x 轴到弧线的起始点沿顺时针方向度量的角（以度为单位）
     * @param sweepAngle 从 startAngle 参数到弧线的结束点沿顺时针方向度量的角（以度为单位）
     */
    void addArc(FCRect rect, float startAngle, float sweepAngle);

    /**
     * 添加贝赛尔曲线
     *
     * @param point1 坐标1
     * @param point2 坐标2
     * @param point3 坐标3
     * @param point4 坐标4
     */
    void addBezier(FCPoint[] points);

    /**
     * 添加曲线
     *
     * @param points 点阵
     */
    void addCurve(FCPoint[] points);

    /**
     * 添加椭圆
     *
     * @param rect 矩形
     */
    void addEllipse(FCRect rect);

    /**
     * 添加直线
     *
     * @param x1 第一个点的横坐标
     * @param y1 第一个点的纵坐标
     * @param x2 第二个点的横坐标
     * @param y2 第二个点的纵坐标
     */
    void addLine(int x1, int y1, int x2, int y2);

    /**
     * 添加矩形
     *
     * @param rect 区域
     */
    void addRect(FCRect rect);

    /**
     * 添加扇形
     *
     * @param rect 矩形区域
     * @param startAngle 从 x 轴到弧线的起始点沿顺时针方向度量的角（以度为单位）
     * @param sweepAngle 从 startAngle 参数到弧线的结束点沿顺时针方向度量的角（以度为单位）
     */
    void addPie(FCRect rect, float startAngle, float sweepAngle);

    /**
     * 添加文字
     *
     * @param text 文字
     * @param font 字体
     * @param rect 区域
     */
    void addText(String text, FCFont font, FCRect rect);

    /**
     * 开始绘图
     *
     * @param hdc HDC
     * @param wRect 窗体区域
     * @param pRect 刷新区域
     */
    void beginPaint(int canvas, FCRect wRect, FCRect pRect);

    /**
     * 开始一段路径
     */
    void beginPath();

    /**
     * 清除缓存
     */
    void clearCaches();

    /**
     * 裁剪路径
     */
    void clipPath();

    /**
     * 闭合路径
     */
    void closeFigure();

    /**
     * 结束一段路径
     */
    void closePath();

    /**
     * 删除对象
     */
    void delete();

    /**
     * 绘制弧线
     *
     * @param dwPenColor 颜色
     * @param width 宽度
     * @param style 样式
     * @param rect 矩形区域
     * @param startAngle 从 x 轴到弧线的起始点沿顺时针方向度量的角（以度为单位）
     * @param sweepAngle 从 startAngle 参数到弧线的结束点沿顺时针方向度量的角（以度为单位）
     */
    void drawArc(long dwPenColor, float width, int style, FCRect rect, float startAngle, float sweepAngle);

    /**
     * 设置贝赛尔曲线
     *
     * @param dwPenColor 颜色
     * @param width 宽度
     * @param style 样式
     * @param points 坐标阵
     */
    void drawBezier(long dwPenColor, float width, int style, FCPoint[] points);

    /**
     * 绘制曲线
     *
     * @param dwPenColor 颜色
     * @param width 宽度
     * @param style 样式
     * @param points 坐标阵
     */
    void drawCurve(long dwPenColor, float width, int style, FCPoint[] points);

    /**
     * 绘制矩形
     *
     * @param dwPenColor 颜色
     * @param width 宽度
     * @param style 样式
     * @param rect 矩形区域
     */
    void drawEllipse(long dwPenColor, float width, int style, FCRect rect);

    /**
     * 绘制矩形
     *
     * @param dwPenColor 颜色
     * @param width 宽度
     * @param style 样式
     * @param left 左侧坐标
     * @param top 顶部左标
     * @param right 右侧坐标
     * @param bottom 底部坐标
     */
    void drawEllipse(long dwPenColor, float width, int style, int left, int top, int right, int bottom);

    /**
     * 绘制图片
     *
     * @param imagePath 图片路径
     * @param rect 绘制区域
     */
    void drawImage(String imagePath, FCRect rect);

    /**
     * 绘制直线
     *
     * @param dwPenColor 颜色
     * @param width 宽度
     * @param style 样式
     * @param x1 第一个点的横坐标
     * @param y1 第一个点的纵坐标
     * @param x2 第二个点的横坐标
     * @param y2 第二个点的纵坐标
     */
    void drawLine(long dwPenColor, float width, int style, int x1, int y1, int x2, int y2);

    /**
     * 绘制直线
     *
     * @param dwPenColor 颜色
     * @param width 宽度
     * @param style 样式
     * @param x 第一个点坐标
     * @param y 第二个点的坐标
     */
    void drawLine(long dwPenColor, float width, int style, FCPoint x, FCPoint y);

    /**
     * 绘制路径
     *
     * @param dwPenColor 颜色
     * @param width 宽度
     * @param style 样式
     */
    void drawPath(long dwPenColor, float width, int style);

    /**
     * 绘制弧线
     *
     * @param dwPenColor 颜色
     * @param width 宽度
     * @param style 样式
     * @param rect 矩形区域
     * @param startAngle 从 x 轴到弧线的起始点沿顺时针方向度量的角（以度为单位）
     * @param sweepAngle 从 startAngle 参数到弧线的结束点沿顺时针方向度量的角（以度为单位）
     */
    void drawPie(long dwPenColor, float width, int style, FCRect rect, float startAngle, float sweepAngle);

    /**
     * 绘制多边形
     *
     * @param dwPenColor 颜色
     * @param width 宽度
     * @param style 样式
     * @param points 点的数组
     */
    void drawPolygon(long dwPenColor, float width, int style, FCPoint[] points);

    /**
     * 绘制大量直线
     *
     * @param dwPenColor 颜色
     * @param width 宽度
     * @param style 样式
     * @param points 点集
     */
    void drawPolyline(long dwPenColor, float width, int style, FCPoint[] points);

    /**
     * 绘制矩形
     *
     * @param dwPenColor 颜色
     * @param width 宽度
     * @param style 样式
     * @param rect 矩形区域
     */
    void drawRect(long dwPenColor, float width, int style, FCRect rect);

    /**
     * 绘制矩形
     *
     * @param dwPenColor 颜色
     * @param width 宽度
     * @param style 样式
     * @param left 左侧坐标
     * @param top 顶部左标
     * @param right 右侧坐标
     * @param bottom 底部坐标
     */
    void drawRect(long dwPenColor, float width, int style, int left, int top, int right, int bottom);

    /**
     * 绘制圆角矩形
     *
     * @param dwPenColor 颜色
     * @param width 宽度
     * @param style 样式
     * @param rect 矩形区域
     * @param cornerRadius 边角半径
     */
    void drawRoundRect(long dwPenColor, float width, int style, FCRect rect, int cornerRadius);

    /**
     * 绘制矩形
     *
     * @param text 文字
     * @param dwPenColor 颜色
     * @param font 字体
     * @param rect 矩形区域
     */
    void drawText(String text, long dwPenColor, FCFont font, FCRect rect);

    /**
     * 绘制矩形
     *
     * @param text 文字
     * @param dwPenColor 颜色
     * @param font 字体
     * @param rect 矩形区域
     */
    void drawText(String text, long dwPenColor, FCFont font, FCRectF rect);

    /**
     * 绘制自动省略结尾的文字
     *
     * @param text 文字
     * @param dwPenColor 颜色
     * @param font 字体
     * @param rect 矩形区域
     */
    void drawTextAutoEllipsis(String text, long dwPenColor, FCFont font, FCRect rect);

    /**
     * 结束导出
     */
    void endPaint();

    /**
     * 结束绘图
     */
    void excludeClipPath();

    /**
     * 填充椭圆
     *
     * @param dwPenColor 颜色
     * @param rect 矩形区域
     */
    void fillEllipse(long dwPenColor, FCRect rect);

    /**
     * 填充椭圆
     *
     * @param dwPenColor 颜色
     * @param left 左侧坐标
     * @param top 顶部左标
     * @param right 右侧坐标
     * @param bottom 底部坐标
     */
    void fillEllipse(long dwPenColor, int left, int top, int right, int bottom);

    /**
     * 绘制渐变椭圆
     *
     * @param dwFirst 开始颜色
     * @param dwSecond 结束颜色
     * @param rect 矩形
     * @param angle 角度
     */
    void fillGradientEllipse(long dwFirst, long dwSecond, FCRect rect, int angle);

    /**
     * 填充渐变路径
     *
     * @param dwFirst 开始颜色
     * @param dwSecond 结束颜色
     * @param points 点的集合
     * @param angle 角度
     */
    void fillGradientPath(long dwFirst, long dwSecond, FCRect rect, int angle);

    /**
     * 绘制渐变的多边形
     *
     * @param dwFirst 开始颜色
     * @param dwSecond 结束颜色
     * @param points 点的集合
     * @param angle 角度
     */
    void fillGradientPolygon(long dwFirst, long dwSecond, FCPoint[] points, int angle);

    /**
     * 绘制渐变矩形
     *
     * @param dwFirst 开始颜色
     * @param dwSecond 结束颜色
     * @param rect 矩形
     * @param cornerRadius 圆角半径
     * @param angle 角度
     */
    void fillGradientRect(long dwFirst, long dwSecond, FCRect rect, int cornerRadius, int angle);

    /**
     * 填充路径
     *
     * @param dwPenColor 颜色
     */
    void fillPath(long dwPenColor);

    /**
     * 绘制扇形
     *
     * @param dwPenColor 颜色
     * @param rect 矩形区域
     * @param startAngle 从 x 轴到弧线的起始点沿顺时针方向度量的角（以度为单位）
     * @param sweepAngle 从 startAngle 参数到弧线的结束点沿顺时针方向度量的角（以度为单位）
     */
    void fillPie(long dwPenColor, FCRect rect, float startAngle, float sweepAngle);

    /**
     * 填充椭圆
     *
     * @param dwPenColor 颜色
     * @param points 点的数组
     */
    void fillPolygon(long dwPenColor, FCPoint[] points);

    /**
     * 填充矩形
     *
     * @param dwPenColor 颜色
     * @param rect 矩形区域
     */
    void fillRect(long dwPenColor, FCRect rect);

    /**
     * 填充矩形
     *
     * @param text 文字
     * @param dwPenColor 颜色
     * @param font 字体
     * @param rect 矩形区域
     */
    void fillRect(long dwPenColor, int left, int top, int right, int bottom);

    /**
     * 填充圆角矩形
     *
     * @param dwPenColor 颜色
     * @param rect 矩形区域
     * @param cornerRadius 边角半径
     */
    void fillRoundRect(long dwPenColor, FCRect rect, int cornerRadius);

    /**
     * 获取颜色
     *
     * @param dwPenColor 输入颜色
     * @returns 输出颜色
     */
    long getColor(long dwPenColor);

    /**
     * 获取要绘制的颜色
     *
     * @param dwPenColor 输入颜色
     * @returns 输出颜色
     */
    long getPaintColor(long dwPenColor);

    /**
     * 获取偏移
     */
    FCPoint getOffset();

    /**
     * 旋转角度
     *
     * @param op 圆心坐标
     * @param mp 点的坐标
     * @param angle 角度
     * @returns 结果坐标
     */
    FCPoint rotate(FCPoint op, FCPoint mp, int angle);

    /**
     * 设置裁剪区域
     *
     * @param rect 区域
     */
    void setClip(FCRect rect);

    /**
     * 设置直线两端的样式
     *
     * @param startLineCap 开始的样式
     * @param endLineCap 结束的样式
     */
    void setLineCap(int startLineCap, int endLineCap);

    /**
     * 设置偏移
     *
     * @param mp 偏移坐标
     */
    void setOffset(FCPoint mp);

    /**
     * 设置透明度
     *
     * @param opacity 透明度
     */
    void setOpacity(float opacity);

    /**
     * 设置资源的路径
     *
     * @param resourcePath 资源的路径
     */
    void setResourcePath(String resourcePath);

    /**
     * 设置旋转角度
     *
     * @param rotateAngle 旋转角度
     */
    void setRotateAngle(int rotateAngle);

    /**
     * 设置缩放因子
     *
     * @param scaleFactorX 横向因子
     * @param scaleFactorY 纵向因子
     */
    void setScaleFactor(double scaleFactorX, double scaleFactorY);

    /**
     * 设置平滑模式
     *
     * @param smoothMode 平滑模式
     */
    void setSmoothMode(int smoothMode);

    /**
     * 设置文字的质量
     *
     * @param textQuality 文字质量
     */
    void setTextQuality(int textQuality);

    /**
     * 设置是否支持透明色
     *
     * @returns 是否支持
     */
    boolean supportTransparent();

    /**
     * 获取文字大小
     *
     * @param text 文字
     * @param font 字体
     * @returns 字体大小
     */
    FCSize textSize(String text, FCFont font);

    /**
     * 获取文字大小
     *
     * @param text 文字
     * @param font 字体
     * @returns 字体大小
     */
    FCSizeF textSizeF(String text, FCFont font);
}
