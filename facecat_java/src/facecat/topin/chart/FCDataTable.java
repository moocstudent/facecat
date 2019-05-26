/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.chart;

/**
 * 自定义数据源
 */
public class FCDataTable {

    /**
     * 析构函数
     */
    protected void finalize() throws Throwable {
        delete();
    }

    /**
     * 列的容量
     */
    protected int m_colsCapacity = 4;

    /**
     * 列的增长步长
     */
    protected int m_colsStep = 4;

    /**
     * 数据列
     */
    protected CList<FCDataColumn> m_columns = new CList<FCDataColumn>();

    /**
     * 数值键
     */
    protected CList<Double> m_keys = new CList<Double>();

    /**
     * 数据行
     */
    protected CList<FCDataRow> m_rows = new CList<FCDataRow>();

    public static int NULLFIELD = -1;

    /**
     * 自动字段
     */
    private static int m_autoField = 10000;

    /**
     * 获取
     */
    public static int getAutoField() {
        return m_autoField++;
    }

    /**
     * 获取列数
     */
    public int getColumnsCount() {
        return m_columns.size();
    }

    private boolean m_isDeleted = false;

    /**
     * 获取或设置是否已被销毁
     */
    public boolean isDeleted() {
        return m_isDeleted;
    }

    /**
     * 获取行数
     */
    public int getRowsCount() {
        if (m_keys.size() != 0) {
            return m_keys.size();
        }
        return 0;
    }

    /**
     * 添加列
     *
     * @param colName 列名
     */
    public void addColumn(int colName) {
        FCDataColumn column = new FCDataColumn(colName, m_columns.size());
        if (m_columns.size() == 0) {
            m_columns.add(column);
        } else {
            int begin = 0;
            int end = m_columns.size() - 1;
            int sub = end - begin;
            while (sub > 1) {
                int half = begin + sub / 2;
                int hf = m_columns.get(half).m_name;
                if (hf > colName) {
                    end = half;
                } else if (hf < colName) {
                    begin = half;
                }
                sub = end - begin;
            }
            if (colName < m_columns.get(begin).m_name) {
                m_columns.insert(begin, column);
                fillEmpty();
                return;
            } else if (colName > m_columns.get(end).m_name) {
                m_columns.insert(end + 1, column);
                fillEmpty();
                return;
            } else {
                m_columns.insert(begin + 1, column);
                fillEmpty();
                return;
            }
        }
    }

    /**
     * 添加数值主键
     *
     * @param num 数值
     * @param newRow 数据行
     */
    private FCDataRow addKey(double num) {
        if (m_keys.size() == 0 || num > m_keys.get(m_keys.size() - 1)) {
            m_keys.add(num);
            FCDataRow newRow = new FCDataRow(m_colsCapacity, m_colsStep);
            m_rows.add(newRow);
            return newRow;
        } else {
            int begin = 0;
            int end = m_keys.size() - 1;
            int sub = end - begin;
            while (sub > 1) {
                int half = begin + sub / 2;
                double hf = m_keys.get(half);
                if (hf > num) {
                    end = half;
                } else if (hf < num) {
                    begin = half;
                }
                sub = end - begin;
            }
            if (num < m_keys.get(begin)) {
                m_keys.insert(begin, num);
                FCDataRow newRow = new FCDataRow(m_colsCapacity, m_colsStep);
                m_rows.insert(begin, newRow);
                return newRow;
            } else if (num > m_keys.get(end)) {
                m_keys.insert(end + 1, num);
                FCDataRow newRow = new FCDataRow(m_colsCapacity, m_colsStep);
                m_rows.insert(end + 1, newRow);
                return newRow;
            } else {
                m_keys.insert(begin + 1, num);
                FCDataRow newRow = new FCDataRow(m_colsCapacity, m_colsStep);
                m_rows.insert(begin + 1, newRow);
                return newRow;
            }
        }
    }

    /**
     * 直接插入行数据
     *
     * @param pk 主键
     * @param ary 数组
     * @param size 长度
     */
    public void addRow(double pk, double[] ary, int size) {
        m_keys.add(pk);
        FCDataRow row = new FCDataRow(ary, size);
        m_rows.add(row);
    }

    /**
     * 清除数据
     */
    public void clear() {
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

    /**
     * 从数据源中获取之前的一段数据的数组
     *
     * @param field 字段
     * @param index 结束索引
     * @param n 周期
     * @returns 数值数组
     */
    public double[] DATA_ARRAY(int field, int index, int n) {
        if (index >= 0) {
            // 获取数组长度
            int arraylength = n;
            // 数组的开始位置
            int start = 0;
            if (index < n - 1) {
                arraylength = index + 1;
            } else {
                start = index - n + 1;
            }
            if (arraylength == -1) {
                return new double[0];
            }
            double[] array = new double[arraylength];
            // 拼装数组
            int columnIndex = getColumnIndex(field);
            for (int i = start; i <= index; i++) {
                array[i - start] = get3(i, columnIndex);
            }
            return array;
        } else {
            return new double[0];
        }
    }

    /**
     * 销毁资源
     */
    public void delete() {
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

    /**
     * 填充空白
     */
    private void fillEmpty() {
        int colSize = m_columns.size();
        for (int i = 0; i < m_rows.size(); i++) {
            m_rows.get(i).fillEmpty(colSize);
        }
    }

    /**
     * 获取列的索引
     *
     * @param colName 列名
     * @returns 索引
     */
    public int getColumnIndex(int colName) {
        if (colName == FCDataTable.NULLFIELD || m_columns == null) {
            return -1;
        }
        int low = 0;
        int high = m_columns.size() - 1;
        while (low <= high) {
            int middle = (low + high) / 2;
            FCDataColumn hf = m_columns.get(middle);
            if (colName == hf.m_name) {
                return middle;
            } else if (colName > hf.m_name) {
                low = middle + 1;
            } else if (colName < hf.m_name) {
                high = middle - 1;
            }
        }
        return -1;
    }

    /**
     * 获取索引
     *
     * @param ke 键
     * @returns 索引
     */
    public int getRowIndex(double key) {
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
            } else if (key > hf) {
                low = middle + 1;
            } else if (key < hf) {
                high = middle - 1;
            }
        }
        return -1;
    }

    /**
     * 获取X轴的值
     *
     * @param index 索引
     * @returns X轴的值
     */
    public double getXValue(int index) {
        try {
            return m_keys.get(index);
        } catch (Exception e) {
            return 0;
        }
    }

    /**
     * 获取数据
     *
     * @param pk 主键
     * @param colName 名称
     */
    public double get(double pk, int colName) {
        try {
            int rowIndex = getRowIndex(pk);
            int colIndex = getColumnIndex(colName);
            return m_rows.get(rowIndex).get(colIndex);
        } catch (Exception e) {
            return Double.NaN;
        }
    }

    /**
     * 根据索引和列名获取数据
     *
     * @param rowIndex 索引
     * @param colName 名称
     * @returns 数据
     */
    public double get2(int rowIndex, int colName) {
        try {
            if (rowIndex >= 0 && rowIndex < m_rows.size()) {
                int colIndex = getColumnIndex(colName);
                return m_rows.get(rowIndex).get(colIndex);
            } else {
                return Double.NaN;
            }
        } catch (Exception e) {
            return Double.NaN;
        }
    }

    /**
     * 根据行索引和列索引获取数据
     *
     * @param rowIndex 索引
     * @param colIndex 列索引
     * @returns 数据
     */
    public double get3(int rowIndex, int colIndex) {
        try {
            if (rowIndex >= 0 && rowIndex < m_rows.size()) {
                return m_rows.get(rowIndex).get(colIndex);
            } else {
                return Double.NaN;
            }
        } catch (Exception e) {
            return Double.NaN;
        }
    }

    /**
     * 移除数据
     *
     * @param pk 主键
     */
    public void remove(double pk) {
        try {
            int index = getRowIndex(pk);
            m_keys.removeAt(index);
            FCDataRow row = m_rows.get(index);
            row.clear();
            m_rows.removeAt(index);
        } catch (Exception e) {
        }
    }

    /**
     * 移除指定索引的数据
     *
     * @param rowIndex 行索引
     */
    public void removeAt(int rowIndex) {
        try {
            m_keys.removeAt(rowIndex);
            FCDataRow row = m_rows.get(rowIndex);
            row.clear();
            m_rows.removeAt(rowIndex);
        } catch (Exception e) {
        }
    }

    /**
     * 移除列
     *
     * @param colName 列名
     */
    public void removeColumn(int colName) {
        int colIndex = getColumnIndex(colName);
        if (colIndex != -1) {
            int colSize = m_columns.size();
            int removeIndex = -1;
            for (int i = 0; i < colSize; i++) {
                FCDataColumn col = m_columns.get(i);
                int name = col.m_name;
                int index = col.m_index;
                if (name == colName) {
                    removeIndex = i;
                } else {
                    if (index > colIndex) {
                        m_columns.set(i, new FCDataColumn(name, index - 1));
                    }
                }
            }
            m_columns.removeAt(removeIndex);
            for (int i = 0; i < m_rows.size(); i++) {
                m_rows.get(i).remove(colIndex);
                m_rows.get(i).fillEmpty(m_columns.size());
            }
        }
    }

    /**
     * 添加数据，主键为日期
     *
     * @param pk 主键
     * @param colName 列名称
     * @param value 值
     */
    public void set(double pk, int colName, double value) {
        FCDataRow row = null;
        int index = getRowIndex(pk);
        if (index == -1) {
            row = addKey(pk);
            row.fillEmpty(m_columns.size());
        } else {
            row = m_rows.get(index);
        }
        int colIndex = getColumnIndex(colName);
        row.set(colIndex, value);
    }

    /**
     * 根据索引添加数据
     *
     * @param rowIndex 索引
     * @param colName 列名称
     * @param value 值
     */
    public void set2(int rowIndex, int colName, double value) {
        int colIndex = getColumnIndex(colName);
        m_rows.get(rowIndex).set(colIndex, value);
    }

    /**
     * 根据行索引和列索引加数据
     *
     * @param rowIndex 索引
     * @param colName 列名称
     * @param value 值
     */
    public void set3(int rowIndex, int colIndex, double value) {
        m_rows.get(rowIndex).set(colIndex, value);
    }

    /**
     * 设置列的容量
     *
     * @param capacity 容量
     */
    public void setColsCapacity(int capacity) {
        m_colsCapacity = capacity;
    }

    /**
     * 设置列的增长步长
     *
     * @param step 容量
     */
    public void setColsGrowStep(int step) {
        m_colsStep = step;
    }

    /**
     * 设置行的容量
     *
     * @param capacity 容量
     */
    public void setRowsCapacity(int capacity) {
        m_keys.setCapacity(capacity);
        m_rows.setCapacity(capacity);
    }

    /**
     * 设置行的增长步长
     *
     * @param step 容量
     */
    public void setRowsGrowStep(int step) {
        m_keys.setStep(step);
        m_rows.setStep(step);
    }
}
