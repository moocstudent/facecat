/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */


#ifndef __FCDATATABLE_H__
#define __FCDATATABLE_H__
#pragma once
#include "stdafx.h"

namespace FaceCat{
    class FCDataTable;
    
    /**
     *  数据行
     */
    class FCDataRow{
    protected:
        FCDataTable *m_table;
        /**
         * 值集合
         */
        ArrayList<double> m_values;
    public:
        /**
         * 创建行
         */
        FCDataRow(FCDataTable *table);
        /**
         * 创建行
         * @param capacity  容量
         * @param step  步长
         */
        FCDataRow(FCDataTable *table, int capacity, int step);
        /**
         * 创建行
         * @param ary     数组
         * @param size    长度
         */
        FCDataRow(FCDataTable *table, double *ary, int size);
        /*
         * 销毁
         */
        virtual ~FCDataRow();
        /**
         * 填充空间
         * @param columns   列名
         */
        void fillEmpty(int columns);
        /**
         *  获取数值
         * @param index   索引
         *  @returns 数值
         */
        double get(int index);
        /**
         * 移除
         * @param index   索引
         */
        void remove(int index);
        /**
         * 设置值
         * @param index   索引
         * @param value   值
         */
        void set(int index, double value);
    };
    
    /**
     * 自定义数据源
     */
    class FCDataTable{
    protected:
        /**
         * 自动字段
         */
        int m_autoField;
        /**
         * 列的容量
         */
        int m_colsCapacity;
        /**
         * 列的增长步长
         */
        int m_colsStep;
        /**
         * 数据列
         */
        ArrayList<int*> m_columns;
        /**
         * 数值键
         */
        ArrayList<double> m_keys;
        /**
         * 数据行
         */
        ArrayList<FCDataRow*> m_rows;
        /**
         * 添加数值主键
         */
        FCDataRow* addKey(double num);
        /**
         * 填充空白
         */
        void fillEmpty();
    public:
        /*
         * 空值
         */
        double NaN;
        /*
         * 创建数据源
         */
        FCDataTable();
        /*
         * 销毁数据源
         */
        virtual ~FCDataTable();
        /*
         * 获取自动字段
         */
        int AUTOFIELD();
        /*
         * 空字段
         */
        static inline int NULLFIELD(){
            return -1;
        }
        /**
         * 添加列
         */
        void addColumn(int colName);
        /**
         * 直接插入行数据
         */
        void addRow(double pk, double *ary, int size);
        /**
         * 列数
         */
        int columnsCount();
        /**
         * 清除数据
         */
        void clear();
        /**
         * 获取列的索引
         */
        int getColumnIndex(int colName);
        /**
         * 获取索引
         */
        int getRowIndex(double key);
        /**
         * 获取X轴的值
         */
        double getXValue(int rowIndex);
        /**
         * 获取数据
         */
        double get(double pk, int colName);
        /**
         * 根据索引和列名获取数据
         */
        double get2(int rowIndex, int colName);
        /**
         * 根据行索引和列索引获取数据
         */
        double get3(int rowIndex, int colIndex);
        /*
         * 是否空值
         */
        static inline bool isNaN(double value){
            return ((*(((long long*) &value)) & 0x7fffffffffffffffL) > 0x7ff0000000000000L);
        }
        /**
         * 移除指定索引的数据
         */
        void removeAt(int rowIndex);
        /**
         * 移除列
         */
        void removeColumn(int name);
        /**
         * 获取行数
         */
        int rowsCount();
        /**
         * 添加数据，主键为日期
         */
        void set(double pk, int colName, double value);
        /**
         * 根据索引添加数据
         */
        void set2(int rowIndex, int colName, double value);
        /**
         * 根据行索引和列索引加数据
         */
        void set3(int rowIndex, int colIndex, double value);
        /**
         * 设置列的容量
         */
        void setColsCapacity(int capacity);
        /**
         * 设置列的增长步长
         */
        void setColsGrowStep(int step);
        /**
         * 设置行的容量
         */
        void setRowsCapacity(int capacity);
        /**
         * 设置行的增长步长
         */
        void setRowsGrowStep(int step);
        /**
         * 从数据源中获取之前的一段数据的数组
         */
        double* DATA_ARRAY(int colName, int rowIndex, int n, int *length);
    };
}
#endif
