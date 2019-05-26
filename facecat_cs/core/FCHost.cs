/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-捂脸鹿创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

using System;

namespace FaceCat {
    /// <summary>
    /// 控件管理
    /// </summary>
    public abstract class FCHost {
        /// <summary>
        /// 获取或设置是否允许操作
        /// </summary>
        public abstract bool AllowOperate {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置是否允许局部绘图
        /// </summary>
        public abstract bool AllowPartialPaint {
            get;
            set;
        }

        public abstract bool IsDeleted {
            get;
        }

        /// <summary>
        /// 获取或设置方法库
        /// </summary>
        public abstract FCNative Native {
            get;
            set;
        }

        /// <summary>
        /// 激活镜像
        /// </summary>
        public abstract void activeMirror();

        /// <summary>
        /// 在控件的线程中调用方法
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="args">参数</param>
        public abstract void beginInvoke(FCView control, object args);

        /// <summary>
        /// 复制文本
        /// </summary>
        /// <param name="text">文本</param>
        public abstract void copy(String text);

        /// <summary>
        /// 销毁资源
        /// </summary>
        public abstract void delete();

        /// <summary>
        /// 创建内部控件
        /// </summary>
        /// <param name="parent">父控件</param>
        /// <param name="clsid">类型ID</param>
        /// <returns>控件</returns>
        public abstract FCView createInternalControl(FCView parent, String clsid);

        /// <summary>
        /// 获取光标
        /// </summary>
        /// <returns>光标</returns>
        public abstract FCCursors getCursor();

        /// <summary>
        /// 获取矩形相交区
        /// </summary>
        /// <param name="lpDestRect">相交矩形</param>
        /// <param name="lpSrc1Rect">矩形1</param>
        /// <param name="lpSrc2Rect">矩形2</param>
        /// <returns>是否相交</returns>
        public abstract int getIntersectRect(ref FCRect lpDestRect, ref FCRect lpSrc1Rect, ref FCRect lpSrc2Rect);

        /// <summary>
        /// 获取尺寸
        /// </summary>
        /// <returns>大小</returns>
        public abstract FCSize getSize();

        /// <summary>
        /// 获取触摸位置
        /// </summary>
        /// <returns>坐标</returns>
        public abstract FCPoint getTouchPoint();

        /// <summary>
        /// 获取矩形并集区
        /// </summary>
        /// <param name="lpDestRect">并集矩形</param>
        /// <param name="lpSrc1Rect">矩形1</param>
        /// <param name="lpSrc2Rect">矩形2</param>
        /// <returns>是否相交</returns>
        public abstract int getUnionRect(ref FCRect lpDestRect, ref FCRect lpSrc1Rect, ref FCRect lpSrc2Rect);

        /// <summary>
        /// 刷新绘图
        /// </summary>
        /// <param name="rect">区域</param>
        public abstract void invalidate(FCRect rect);

        /// <summary>
        /// 刷新绘图
        /// </summary>
        public abstract void invalidate();

        /// <summary>
        /// 在控件的线程中调用方法
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="args">参数</param>
        public abstract void invoke(FCView control, object args);

        /// <summary>
        /// 获取按键的状态
        /// </summary>
        /// <param name="key">按键</param>
        /// <returns>状态</returns>
        public abstract bool isKeyPress(int key);

        /// <summary>
        /// 获取粘贴文本
        /// </summary>
        /// <returns>文本</returns>
        public abstract String paste();

        /// <summary>
        /// 设置光标
        /// </summary>
        /// <param name="cursor">光标</param>
        public abstract void setCursor(FCCursors cursor);

        /// <summary>
        /// 显示提示框
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="mp">位置</param>
        public abstract void showToolTip(String text, FCPoint mp);

        /// <summary>
        /// 开启秒表
        /// </summary>
        /// <param name="timerID">秒表ID</param>
        /// <param name="interval">间隔</param>
        public abstract void startTimer(int timerID, int interval);

        /// <summary>
        /// 停止秒表
        /// </summary>
        /// <param name="timerID">秒表ID</param>
        public abstract void stopTimer(int timerID);
    }
}
