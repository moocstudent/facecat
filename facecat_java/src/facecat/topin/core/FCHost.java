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
 * 控件管理
 */
public abstract class FCHost {

    /**
     * 获取是否允许操作
     */
    public abstract boolean allowOperate();

    /**
     * 设置是否允许操作
     */
    public abstract void setAllowOperate(boolean value);

    /**
     * 获取是否允许局部绘图
     */
    public abstract boolean allowPartialpaint();

    /**
     * 设置是否允许局部绘图
     */
    public abstract void setAllowPartialPaint(boolean value);

    /**
     * 获取方法库
     */
    public abstract FCNative getNative();

    /**
     * 设置方法库
     */
    public abstract void setNative(FCNative value);

    /**
     * 在控件的线程中调用方法
     *
     * @param control 控件
     * @param args 参数
     */
    public abstract void beginInvoke(FCView control, Object args);

    /**
     * 复制文本
     *
     * @param text 文本
     */
    public abstract void copy(String text);

    /**
     * 创建内部控件
     *
     * @param parent 父控件
     * @param clsid 类型ID
     * @returns 控件
     */
    public abstract FCView createInternalControl(FCView parent, String clsid);

    /**
     * 获取触摸位置
     */
    public abstract FCPoint getTouchPoint();

    /**
     * 获取矩形相交区
     *
     * @param lpDestRect 相交矩形
     * @param lpSrc1Rect 矩形1
     * @param lpSrc2Rect 矩形2
     * @returns 是否相交
     */
    public abstract int getIntersectRect(RefObject<FCRect> lpDestRect, RefObject<FCRect> lpSrc1Rect, RefObject<FCRect> lpSrc2Rect);

    /**
     * 获取尺寸
     */
    public abstract FCSize getSize();

    public abstract int getUnionRect(RefObject<FCRect> lpDestRect, RefObject<FCRect> lpSrc1Rect, RefObject<FCRect> lpSrc2Rect);

    /**
     * 刷新绘图
     *
     * @param rect 区域
     */
    public abstract void invalidate(FCRect rect);

    /**
     * 刷新绘图
     */
    public abstract void invalidate();

    /**
     * 在控件的线程中调用方法
     *
     * @param control 控件
     * @param args 参数
     */
    public abstract void invoke(FCView control, Object args);

    /**
     * 获取粘贴文本
     */
    public abstract String paste();

    /**
     * 显示提示框
     *
     * @param text 文字
     * @param mp 位置
     */
    public abstract void showToolTip(String text, FCPoint FCPoint);

    /**
     * 开启秒表
     *
     * @param timerID 秒表ID
     * @param interval 间隔
     */
    public abstract void startTimer(int timerID, int interval);

    /**
     * 停止秒表
     *
     * @param timerID 秒表ID
     */
    public abstract void stopTimer(int timerID);
}
