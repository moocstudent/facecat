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
 * 数据行
 */
public class FCDataRow {

    /**
     * 创建行
     */
    public FCDataRow() {
        m_values = new CList();
    }

    /**
     * 创建行
     *
     * @param capacity 容量
     * @param step 步长
     */
    public FCDataRow(int capacity, int step) {
        m_values = new CList();
        m_values.setCapacity(capacity);
        m_values.setStep(capacity);
    }

    /**
     * 创建行
     *
     * @param ary 数组
     * @param size 长度
     */
    public FCDataRow(double[] ary, int size) {
        m_values = new CList<Double>();
        for (int i = 0; i < size; i++) {
            m_values.add(ary[i]);
        }
    }

    /**
     * 析构函数
     */
    protected void finalize() throws Throwable {
        delete();
    }

    /**
     * 值集合
     */
    private CList<Double> m_values;

    /**
     * 清除数据
     */
    public void clear() {
        if (m_values != null) {
            m_values.clear();
        }
    }

    /**
     * 销毁对象
     */
    public void delete() {
        if (m_values != null) {
            m_values.delete();
            m_values = null;
        }
    }

    /**
     * 填充空间
     *
     * @param columns 列名
     */
    public void fillEmpty(int columns) {
        if (m_values != null) {
            int size = m_values.size();
            if (size >= 0) {
                for (int i = size; i < columns; i++) {
                    m_values.insert(i, Double.NaN);
                }
            }
        }
    }

    /**
     * 获取数值
     *
     * @param index 索引
     * @returns 数值
     */
    public double get(int index) {
        if (index != -1) {
            return m_values.get(index);
        }
        return Double.NaN;
    }

    /**
     * 移除
     *
     * @param index 索引
     */
    public void remove(int index) {
        if (index != -1 && m_values != null) {
            m_values.removeAt(index);
        }
    }

    /**
     * 设置值
     *
     * @param index 索引
     * @param value 值
     */
    public void set(int index, double value) {
        m_values.set(index, value);
    }
}
