/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.chart;

import java.lang.reflect.Array;

/**
 * 自定义泛型集合
 *
 * @param T 泛型
 */
public class CList<T> {

    /**
     * 数组
     */
    public Object m_ary[];

    /**
     * 当前大小
     */
    public int m_size;

    /**
     * 容量
     */
    public int m_capacity;

    /**
     * 容量增长步长
     */
    public int m_step;

    /**
     * 创建泛型集合
     */
    public CList() {
        m_size = 0;
        m_ary = null;
        m_capacity = 4;
        m_step = 4;
    }

    /**
     * 创建泛型集合
     *
     * @param capacity 容量
     */
    public CList(int capacity) {
        m_size = 0;
        m_ary = null;
        m_capacity = capacity;
        m_step = 4;
    }

    /**
     * 析构函数
     */
    @Override
    protected void finalize() throws Throwable {
        super.finalize();
        delete();
    }
    
        /**
     * 向后插入值
     *
     * @param value 值
     */
    public void add(T value) {
        m_size += 1;
        if (m_ary == null) {
            m_ary = new Object[m_capacity];
        } else {
            if (m_size > m_capacity) {
                m_capacity += m_step;
                Object[] newAry = new Object[m_capacity];
                for (int i = 0; i < m_size - 1; i++) {
                    newAry[i] = m_ary[i];
                }
                m_ary = null;
                m_ary = newAry;
            }
        }
        m_ary[m_size - 1] = value;
    }

    /**
     * 创建泛型集合
     *
     * @param ary 数组
     * @param size 大小
     */
    public void addranges(T[] ary, int size) {
        m_ary = ary;
        m_size = size;
        m_capacity = m_size;
        m_step = 4;
    }

    /**
     * 获取容量
     */
    public int capacity() {
        return m_capacity;
    }

    /**
     * 清除数据
     */
    public void clear() {
        m_step = 4;
        m_size = 0;
        m_ary = null;
    }

    /**
     * 销毁对象
     */
    public void delete() {
        clear();
    }

    /**
     * 根据索引获取数据
     *
     * @param index 索引
     * @ returns 数据
     */
    @SuppressWarnings("unchecked")
    public T get(int index) {
        return (T) m_ary[index];
    }

    /**
     * 插入数据
     *
     * @param index 索引
     * @param value 值
     */
    public void insert(int index, T value) {
        m_size += 1;
        if (m_ary == null) {
            m_ary = new Object[m_capacity];
        } else {
            if (m_size > m_capacity) {
                m_capacity += m_step;
                Object[] newAry = new Object[m_capacity];
                for (int i = 0; i < m_size - 1; i++) {
                    if (i < index) {
                        newAry[i] = m_ary[i];
                    } else if (i >= index) {
                        newAry[i + 1] = m_ary[i];
                    }
                }
                m_ary = null;
                m_ary = newAry;
            } else {
                Object last = 0;
                for (int i = index; i < m_size; i++) {
                    if (i == index) {
                        last = m_ary[i];
                    } else if (i > index) {
                        Object temp = m_ary[i];
                        m_ary[i] = last;
                        last = temp;
                    }
                }
            }
        }
        m_ary[index] = value;
    }

    /**
     * 移除数据
     *
     * @param index 索引
     */
    public void removeAt(int index) {
        m_size -= 1;
        for (int i = index; i < m_size; i++) {
            m_ary[i] = m_ary[i + 1];
        }
        if (m_capacity - m_size > m_step) {
            m_capacity -= m_step;
            if (m_capacity > 0) {
                Object[] newAry = new Object[m_capacity];
                for (int i = 0; i < m_size; i++) {
                    newAry[i] = m_ary[i];
                }
                m_ary = newAry;
            } else {
                m_ary = null;
            }
        }
    }

    /**
     * 设置值
     *
     * @param index 索引
     * @param value 值
     */
    public void set(int index, T value) {
        m_ary[index] = value;
    }

    /**
     * 设置容量
     *
     * @param capacity 容量
     */
    public void setCapacity(int capacity) {
        m_capacity = capacity;
        if (m_ary != null) {
            Object[] newAry = new Object[m_capacity];
            for (int i = 0; i < m_size - 1; i++) {
                newAry[i] = m_ary[i];
            }
            m_ary = null;
            m_ary = newAry;
        }
    }

    /**
     * 设置容量增长步长
     *
     * @param step 步长
     */
    public void setStep(int step) {
        m_step = step;
    }

    /**
     * 获取大小
     *
     * @ returns 大小
     */
    public int size() {
        return m_size;
    }

    /**
     * 转换为字符串
     */
    @Override
    public String toString() {
        StringBuffer buf = new StringBuffer();
        buf.append("[");
        for (int i = 0; i < m_size; i++) {
            buf.append(m_ary[i]);
            if (i != m_size - 1) {
                buf.append(",");
            }
        }
        buf.append("]");
        return buf.toString();
    }

    @SuppressWarnings("unchecked")
    public T[] toArray(T[] t, int lenth, Class<? extends T[]> type) {
        T[] ret = ((Object) type == (Object) Object[].class) ? (T[]) new Object[lenth]
                : (T[]) Array.newInstance(type.getComponentType(), lenth);
        System.arraycopy(t, 0, ret, 0, Math.min(t.length, lenth));
        return ret;
    }
}
