/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

using System;

namespace FaceCat {
    /// <summary>
    /// 纵轴的类型
    /// </summary>
    public enum AttachVScale {
        /// <summary>
        /// 左轴
        /// </summary>
        Left,
        /// <summary>
        /// 右轴
        /// </summary>
        Right
    }

    /// <summary>
    /// 柱状图样式
    /// </summary>
    public enum BarStyle {
        /// <summary>
        /// 线条
        /// </summary>
        Line,
        /// <summary>
        /// 矩形
        /// </summary>
        Rect,
    }

    /// <summary>
    /// K线的样式
    /// </summary>
    public enum CandleStyle {
        /// <summary>
        /// 美国线
        /// </summary>
        American,
        /// <summary>
        /// 收盘线
        /// </summary>
        CloseLine,
        /// <summary>
        /// 矩形
        /// </summary>
        Rect,
        /// <summary>
        /// 宝塔线
        /// </summary>
        Tower
    }

    /// <summary>
    /// 十字线的移动方式
    /// </summary>
    public enum CrossLineMoveMode {
        /// <summary>
        /// 触摸点击后移动
        /// </summary>
        AfterClick,
        /// <summary>
        /// 跟随触摸
        /// </summary>
        FollowTouch
    }

    /// <summary>
    /// 日期的类型
    /// </summary>
    public enum DateType {
        /// <summary>
        /// 日
        /// </summary>
        Day = 2,
        /// <summary>
        /// 小时
        /// </summary>
        Hour = 3,
        /// <summary>
        /// 毫秒
        /// </summary>
        Millisecond = 6,
        /// <summary>
        /// 分钟
        /// </summary>
        Minute = 4,
        /// <summary>
        /// 月
        /// </summary>
        Month = 1,
        /// <summary>
        /// 秒
        /// </summary>
        Second = 5,
        /// <summary>
        /// 年
        /// </summary>
        Year = 0
    }

    /// <summary>
    /// X轴的类型
    /// </summary>
    public enum HScaleType {
        /// <summary>
        /// 日期
        /// </summary>
        Date,
        /// <summary>
        /// 数字
        /// </summary>
        Number
    }

    /// <summary>
    /// 数字的样式
    /// </summary>
    public enum NumberStyle {
        /// <summary>
        /// 标准
        /// </summary>
        Standard,
        /// <summary>
        /// 加下划线数字
        /// </summary>
        UnderLine
    }

    /// <summary>
    /// 折线的样式
    /// </summary>
    public enum PolylineStyle {
        /// <summary>
        /// 圆圈
        /// </summary>
        Cycle,
        /// <summary>
        /// 虚线
        /// </summary>
        DashLine,
        /// <summary>
        /// 细点图
        /// </summary>
        DotLine,
        /// <summary>
        /// 实线
        /// </summary>
        SolidLine
    }

    /// <summary>
    /// 选中点
    /// </summary>
    public enum SelectedPoint {
        /// <summary>
        /// 圆
        /// </summary>
        Ellipse,
        /// <summary>
        /// 矩形
        /// </summary>
        Rectangle
    }

    /// <summary>
    /// 数据排序方式
    /// </summary>
    public enum SortType {
        /// <summary>
        /// 升序
        /// </summary>
        ASC,
        /// <summary>
        /// 降序
        /// </summary>
        DESC,
        /// <summary>
        /// 无排序
        /// </summary>
        NONE
    }

    /// <summary>
    /// 图形标题的模式
    /// </summary>
    public enum TextMode {
        /// <summary>
        /// 显示字段
        /// </summary>
        Field,
        /// <summary>
        /// 显示完整
        /// </summary>
        Full,
        /// <summary>
        /// 不显示
        /// </summary>
        None,
        /// <summary>
        /// 显示值
        /// </summary>
        Value
    }

    /// <summary>
    /// 纵轴坐标系
    /// </summary>
    public enum VScaleSystem {
        /// <summary>
        /// 对数坐标
        /// </summary>
        Logarithmic,
        /// <summary>
        /// 标准
        /// </summary>
        Standard
    }

    /// <summary>
    /// 纵坐标轴类型
    /// </summary>
    public enum VScaleType {
        /// <summary>
        /// 等分
        /// </summary>
        Divide,
        /// <summary>
        /// 等差
        /// </summary>
        EqualDiff,
        /// <summary>
        /// 等比
        /// </summary>
        EqualRatio,
        /// <summary>
        /// 黄金分割
        /// </summary>
        GoldenRatio,
        /// <summary>
        /// 百分比
        /// </summary>
        Percent
    }
}
