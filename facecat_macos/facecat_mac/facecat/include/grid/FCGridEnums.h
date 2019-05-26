/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCGRIDENUMS_H__
#define __FCGRIDENUMS_H__
#pragma once
#include "stdafx.h"

namespace FaceCat{
    /**
     * 表格列排序方式
     */
    enum FCGridColumnSortMode{
        /**
         * 升序
         */
        FCGridColumnSortMode_Asc,
        /**
         * 降序
         */
        FCGridColumnSortMode_Desc,
        /**
         * 不排序
         */
        FCGridColumnSortMode_None
    };
    
    /**
     * 表格选中模式
     */
    enum FCGridSelectionMode{
        /**
         * 选中单元格
         */
        FCGridSelectionMode_SelectCell,
        /**
         * 选中整列
         */
        FCGridSelectionMode_SelectFullColumn,
        /**
         * 选中整行
         */
        FCGridSelectionMode_SelectFullRow,
        /**
         * 不选中
         */
        FCGridSelectionMode_SelectNone
    };
    
    /**
     * 单元格编辑模式
     */
    enum FCGridCellEditMode{
        /**
         * 双击
         */
        FCGridCellEditMode_DoubleClick,
        /**
         * 无效
         */
        FCGridCellEditMode_None,
        /**
         * 单击
         */
        FCGridCellEditMode_SingleClick
    };
}

#endif
