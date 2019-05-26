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
    /// 表格列排序方式
    /// </summary>
    public enum FCGridColumnSortMode {
        /// <summary>
        /// 升序
        /// </summary>
        Asc,
        /// <summary>
        /// 降序
        /// </summary>
        Desc,
        /// <summary>
        /// 不排序
        /// </summary>
        None
    }

    /// <summary>
    /// 表格选中模式
    /// </summary>
    public enum FCGridSelectionMode {
        /// <summary>
        /// 选中单元格
        /// </summary>
        SelectCell,
        /// <summary>
        /// 选中整列
        /// </summary>
        SelectFullColumn,
        /// <summary>
        /// 选中整行
        /// </summary>
        SelectFullRow,
        /// <summary>
        /// 不选中
        /// </summary>
        SelectNone
    }

    /// <summary>
    /// 单元格编辑模式
    /// </summary>
    public enum FCGridCellEditMode {
        /// <summary>
        /// 双击
        /// </summary>
        DoubleClick,
        /// <summary>
        /// 无效
        /// </summary>
        None,
        /// <summary>
        /// 单击
        /// </summary>
        SingleClick
    }
}
