/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

using System;
using System.Collections.Generic;

namespace FaceCat {
    /// <summary>
    /// 数据行
    /// </summary>
    public class FCDataRow {
        /// <summary>
        /// 创建行
        /// </summary>
        public FCDataRow() {
            m_values = new CList<double>();
        }

        /// <summary>
        /// 创建行
        /// <param name="capacity">容量</param>
        /// <param name="step">步长</param>
        /// </summary>
        public FCDataRow(int capacity, int step) {
            m_values = new CList<double>();
            m_values.set_capacity(capacity);
            m_values.set_step(capacity);
        }

        /// <summary>
        /// 创建行
        /// </summary>
        /// <param name="ary">数组</param>
        /// <param name="size">长度</param>
        public FCDataRow(double[] ary, int size) {
            m_values = new CList<double>();
            m_values.addranges(ary, size);
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~FCDataRow() {
            delete();
        }

        /// <summary>
        /// 值集合
        /// </summary>
        private CList<double> m_values;

        /// <summary>
        /// 清除数据
        /// </summary>
        public void clear() {
            if (m_values != null) {
                m_values.clear();
            }
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        public void delete() {
            if (m_values != null) {
                m_values.clear();
                m_values = null;
            }
        }

        /// <summary>
        /// 填充空间
        /// </summary>
        /// <param name="columns">列名</param>
        public void fillEmpty(int columns) {
            if (m_values != null) {
                int size = m_values.size();
                if (size >= 0) {
                    for (int i = size; i < columns; i++) {
                        m_values.insert(i, double.NaN);
                    }
                }
            }
        }

        /// <summary>
        /// 获取数值
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>数值</returns>
        public double get(int index) {
            if (index != -1) {
                return m_values.get(index);
            }
            return double.NaN;
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="index">索引</param>
        public void remove(int index) {
            if (index != -1 && m_values != null) {
                m_values.remove_at(index);
            }
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="value">值</param>
        public void set(int index, double value) {
            m_values.set(index, value);
        }
    }

    /// <summary>
    /// 自定义数据源
    /// </summary>
    public class FCDataTable {
        /// <summary>
        /// 析构函数
        /// </summary>
        ~FCDataTable() {
            delete();
        }

        public static int NULLFIELD = -1;

        /// <summary>
        /// 列的容量
        /// </summary>
        private int m_colsCapacity = 4;

        /// <summary>
        /// 列的增长步长
        /// </summary>
        private int m_colsStep = 4;

        /// <summary>
        /// 数据列
        /// </summary>
        private CList<int[]> m_columns = new CList<int[]>();

        /// <summary>
        /// 数值键
        /// </summary>
        private CList<double> m_keys = new CList<double>();

        /// <summary>
        /// 数据行
        /// </summary>
        private CList<FCDataRow> m_rows = new CList<FCDataRow>();

        /// <summary>
        /// 自动字段
        /// </summary>
        private static int m_autoField = 10000;

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        public static int AutoField {
            get { return m_autoField++; }
        }

        /// <summary>
        /// 获取列数
        /// </summary>
        public virtual int ColumnsCount {
            get { return m_columns.size(); }
        }

        private bool m_isDeleted;

        /// <summary>
        /// 获取或设置是否已被销毁
        /// </summary>
        public virtual bool IsDeleted {
            get { return m_isDeleted; }
        }

        /// <summary>
        /// 获取行数
        /// </summary>
        public virtual int RowsCount {
            get {
                if (m_keys.size() != 0) return m_keys.size();
                return 0;
            }
        }

        /// <summary>
        /// 添加列
        /// </summary>
        /// <param name="colName">列名</param>
        public virtual void addColumn(int colName) {
            int[] column = new int[] { colName, m_columns.size() };
            if (m_columns.size() == 0) {
                m_columns.push_back(column);
            }
            else {
                int begin = 0;
                int end = m_columns.size() - 1;
                int sub = end - begin;
                while (sub > 1) {
                    int half = begin + sub / 2;
                    int hf = m_columns.get(half)[0];
                    if (hf > colName) {
                        end = half;
                    }
                    else if (hf < colName) {
                        begin = half;
                    }
                    sub = end - begin;
                }
                if (colName < m_columns.get(begin)[0]) {
                    m_columns.insert(begin, column);
                    fillEmpty();
                    return;
                }
                else if (colName > m_columns.get(end)[0]) {
                    m_columns.insert(end + 1, column);
                    fillEmpty();
                    return;
                }
                else {
                    m_columns.insert(begin + 1, column);
                    fillEmpty();
                    return;
                }
            }
        }

        /// <summary>
        /// 添加数值主键
        /// </summary>
        /// <param name="num">数值</param>
        /// <param name="newRow">数据行</param>
        private FCDataRow addKey(double num) {
            if (m_keys.size() == 0 || num > m_keys.get(m_keys.size() - 1)) {
                m_keys.push_back(num);
                FCDataRow newRow = new FCDataRow(m_colsCapacity, m_colsStep);
                m_rows.push_back(newRow);
                return newRow;
            }
            else {
                int begin = 0;
                int end = m_keys.size() - 1;
                int sub = end - begin;
                while (sub > 1) {
                    int half = begin + sub / 2;
                    double hf = m_keys.get(half);
                    if (hf > num) {
                        end = half;
                    }
                    else if (hf < num) {
                        begin = half;
                    }
                    sub = end - begin;
                }
                if (num < m_keys.get(begin)) {
                    m_keys.insert(begin, num);
                    FCDataRow newRow = new FCDataRow(m_colsCapacity, m_colsStep);
                    m_rows.insert(begin, newRow);
                    return newRow;
                }
                else if (num > m_keys.get(end)) {
                    m_keys.insert(end + 1, num);
                    FCDataRow newRow = new FCDataRow(m_colsCapacity, m_colsStep);
                    m_rows.insert(end + 1, newRow);
                    return newRow;
                }
                else {
                    m_keys.insert(begin + 1, num);
                    FCDataRow newRow = new FCDataRow(m_colsCapacity, m_colsStep);
                    m_rows.insert(begin + 1, newRow);
                    return newRow;
                }
            }
        }

        /// <summary>
        /// 直接插入行数据
        /// </summary>
        /// <param name="pk">主键</param>
        /// <param name="ary">数组</param>
        /// <param name="size">长度</param>
        public virtual void AddRow(double pk, double[] ary, int size) {
            m_keys.push_back(pk);
            FCDataRow row = new FCDataRow(ary, size);
            m_rows.push_back(row);
        }

        /// <summary>
        /// 清除数据
        /// </summary>
        public virtual void clear() {
            if (m_keys != null) {
                m_keys.clear();
            }
            if (m_rows != null) {
                for (int i = 0; i < m_rows.size(); i++) {
                    FCDataRow row = m_rows.get(i);
                    if (row != null) {
                        row.delete();
                    }
                }
                m_rows.clear();
            }
        }

        /// <summary>
        /// 从数据源中获取之前的一段数据的数组
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="index">结束索引</param>
        /// <param name="n">周期</param>
        /// <returns>数值数组</returns>
        public virtual double[] DATA_ARRAY(int field, int index, int n) {
            if (index >= 0) {
                //获取数组长度
                int arraylength = n;
                //数组的开始位置
                int start = 0;
                if (index < n - 1) {
                    arraylength = index + 1;
                }
                else {
                    start = index - n + 1;
                }
                if (arraylength == -1) {
                    return new double[0];
                }
                double[] array = new double[arraylength];
                //拼装数组
                int columnIndex = getColumnIndex(field);
                for (int i = start; i <= index; i++) {
                    array[i - start] = get3(i, columnIndex);
                }
                return array;
            }
            else {
                return new double[0];
            }
        }

        /// <summary>
        /// 销毁资源
        /// </summary>
        public virtual void delete() {
            if (!m_isDeleted) {
                clear();
                if (m_columns != null) {
                    m_columns.delete();
                    m_columns = null;
                }
                if (m_keys != null) {
                    m_keys.delete();
                    m_keys = null;
                }
                if (m_rows != null) {
                    m_rows.delete();
                    m_rows = null;
                }
                m_isDeleted = true;
            }
        }

        /// <summary>
        /// 填充空白
        /// </summary>
        private void fillEmpty() {
            int colSize = m_columns.size();
            for (int i = 0; i < m_rows.size(); i++) {
                m_rows.get(i).fillEmpty(colSize);
            }
        }

        /// <summary>
        /// 获取列的索引
        /// </summary>
        /// <param name="colName">列名</param>
        /// <returns>索引</returns>
        public virtual int getColumnIndex(int colName) {
            if (colName == FCDataTable.NULLFIELD || m_columns == null) {
                return -1;
            }
            int low = 0;
            int high = m_columns.size() - 1;
            while (low <= high) {
                int middle = (low + high) / 2;
                int[] hf = m_columns.get(middle);
                if (colName == hf[0]) {
                    return middle;
                }
                else if (colName > hf[0]) {
                    low = middle + 1;
                }
                else if (colName < hf[0]) {
                    high = middle - 1;
                }
            }
            return -1;
        }

        /// <summary>
        /// 获取索引
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>索引</returns>
        public virtual int getRowIndex(double key) {
            if (m_keys == null) {
                return -1;
            }
            int low = 0;
            int high = m_keys.size() - 1;
            while (low <= high) {
                int middle = (low + high) / 2;
                double hf = m_keys.get(middle);
                if (key == hf) {
                    return middle;
                }
                else if (key > hf) {
                    low = middle + 1;
                }
                else if (key < hf) {
                    high = middle - 1;
                }
            }
            return -1;
        }

        /// <summary>
        /// 获取X轴的值
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>X轴的值</returns>
        public virtual double getXValue(int index) {
            try {
                return m_keys.get(index);
            }
            catch {
                return 0;
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="pk">主键</param>
        /// <param name="colName">名称</param>
        public virtual double get(double pk, int colName) {
            try {
                int rowIndex = getRowIndex(pk);
                int colIndex = getColumnIndex(colName);
                return m_rows.get(rowIndex).get(colIndex);
            }
            catch {
                return double.NaN;
            }
        }

        /// <summary>
        /// 根据索引和列名获取数据
        /// </summary>
        /// <param name="rowIndex">索引</param>
        /// <param name="colName">名称</param>
        /// <returns>数据</returns>
        public virtual double get2(int rowIndex, int colName) {
            try {
                if (rowIndex >= 0 && rowIndex < m_rows.size()) {
                    int colIndex = getColumnIndex(colName);
                    return m_rows.get(rowIndex).get(colIndex);
                }
                else {
                    return double.NaN;
                }
            }
            catch {
                return double.NaN;
            }
        }

        /// <summary>
        /// 根据行索引和列索引获取数据
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        /// <param name="colIndex">列索引</param>
        /// <returns>数据</returns>
        public virtual double get3(int rowIndex, int colIndex) {
            try {
                if (rowIndex >= 0 && rowIndex < m_rows.size()) {
                    return m_rows.get(rowIndex).get(colIndex);
                }
                else {
                    return double.NaN;
                }
            }
            catch {
                return double.NaN;
            }
        }

        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="pk">主键</param>
        public virtual void remove(double pk) {
            try {
                int index = getRowIndex(pk);
                m_keys.remove_at(index);
                FCDataRow row = m_rows.get(index);
                row.clear();
                m_rows.remove_at(index);
            }
            catch { }
        }

        /// <summary>
        /// 移除指定索引的数据
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        public virtual void removeAt(int rowIndex) {
            try {
                m_keys.remove_at(rowIndex);
                FCDataRow row = m_rows.get(rowIndex);
                row.clear();
                m_rows.remove_at(rowIndex);
            }
            catch { }
        }

        /// <summary>
        /// 移除列
        /// </summary>
        /// <param name="colName">列名</param>
        public virtual void removeColumn(int colName) {
            int colIndex = getColumnIndex(colName);
            if (colIndex != -1) {
                int colSize = m_columns.size();
                int removeIndex = -1;
                for (int i = 0; i < colSize; i++) {
                    int[] col = m_columns.get(i);
                    int name = col[0];
                    int index = col[1];
                    if (col[0] == colName) {
                        removeIndex = i;
                    }
                    else {
                        if (index > colIndex) {
                            m_columns.set(i, new int[] { name, index - 1 });
                        }
                    }
                }
                m_columns.remove_at(removeIndex);
                for (int i = 0; i < m_rows.size(); i++) {
                    m_rows.get(i).remove(colIndex);
                    m_rows.get(i).fillEmpty(m_columns.size());
                }
            }
        }

        /// <summary>
        /// 添加数据，主键为日期
        /// </summary>
        /// <param name="pk">主键</param>
        /// <param name="colName">列名称</param>
        /// <param name="value">值</param>
        public virtual void set(double pk, int colName, double value) {
            FCDataRow row = null;
            int index = getRowIndex(pk);
            if (index == -1) {
                row = addKey(pk);
                row.fillEmpty(m_columns.size());
            }
            else {
                row = m_rows.get(index);
            }
            int colIndex = getColumnIndex(colName);
            row.set(colIndex, value);
        }

        /// <summary>
        /// 根据索引添加数据
        /// </summary>
        /// <param name="rowIndex">索引</param>
        /// <param name="colName">列名称</param>
        /// <param name="value">值</param>
        public virtual void set2(int rowIndex, int colName, double value) {
            int colIndex = getColumnIndex(colName);
            m_rows.get(rowIndex).set(colIndex, value);
        }

        /// <summary>
        /// 根据行索引和列索引加数据
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        /// <param name="colIndex">列索引</param>
        /// <param name="value">值</param>
        public virtual void set3(int rowIndex, int colIndex, double value) {
            m_rows.get(rowIndex).set(colIndex, value);
        }

        /// <summary>
        /// 设置列的容量
        /// </summary>
        /// <param name="capacity">容量</param>
        public virtual void setColsCapacity(int capacity) {
            m_colsCapacity = capacity;
        }

        /// <summary>
        /// 设置列的增长步长
        /// </summary>
        /// <param name="step">容量</param>
        public virtual void setColsGrowStep(int step) {
            m_colsStep = step;
        }

        /// <summary>
        /// 设置行的容量
        /// </summary>
        /// <param name="capacity">容量</param>
        public virtual void setRowsCapacity(int capacity) {
            m_keys.set_capacity(capacity);
            m_rows.set_capacity(capacity);
        }

        /// <summary>
        /// 设置行的增长步长
        /// </summary>
        /// <param name="step">容量</param>
        public virtual void setRowsGrowStep(int step) {
            m_keys.set_step(step);
            m_rows.set_step(step);
        }
    }
}
