/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-捂脸鹿创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

using System;
using System.Collections.Generic;

namespace FaceCat {
    /// <summary>
    /// 自定义泛型集合
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    public class CList<T> {
        //数组
        public T[] m_ary;

        //当前大小
        public int m_size;

        //容量
        public int m_capacity;

        //容量增长步长
        public int m_step;

        /// <summary>
        /// 创建泛型集合
        /// </summary>
        public CList() {
            m_size = 0;
            m_ary = null;
            m_capacity = 4;
            m_step = 4;
        }

        /// <summary>
        /// 创建泛型集合
        /// </summary>
        /// <param name="capacity">容量</param>
        public CList(int capacity) {
            m_size = 0;
            m_ary = null;
            m_capacity = capacity;
            m_step = 4;
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~CList() {
            delete();
        }

        /// <summary>
        /// 创建泛型集合
        /// </summary>
        /// <param name="ary">数组</param>
        /// <param name="size">大小</param>
        public void addranges(T[] ary, int size) {
            m_ary = ary;
            m_size = size;
            m_capacity = m_size;
            m_step = 4;
        }

        /// <summary>
        /// 获取容量
        /// </summary>
        /// <returns></returns>
        public int capacity() {
            return m_capacity;
        }

        /// <summary>
        /// 清除数据
        /// </summary>
        public void clear() {
            m_step = 4;
            m_size = 0;
            m_ary = null;
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        public void delete() {
            clear();
        }

        /// <summary>
        /// 根据索引获取数据
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>数据</returns>
        public T get(int index) {
            return m_ary[index];
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="value">值</param>
        public void insert(int index, T value) {
            m_size += 1;
            if (m_ary == null) {
                m_ary = new T[m_capacity];
            }
            else {
                if (m_size > m_capacity) {
                    m_capacity += m_step;
                    T[] newAry = new T[m_capacity];
                    for (int i = 0; i < m_size - 1; i++) {
                        if (i < index) {
                            newAry[i] = m_ary[i];
                        }
                        else if (i >= index) {
                            newAry[i + 1] = m_ary[i];
                        }
                    }
                    m_ary = null;
                    m_ary = newAry;
                }
                else {
                    T last = default(T);
                    for (int i = index; i < m_size; i++) {
                        if (i == index) {
                            last = m_ary[i];
                        }
                        else if (i > index) {
                            T temp = m_ary[i];
                            m_ary[i] = last;
                            last = temp;
                        }
                    }
                }
            }
            m_ary[index] = value;
        }

        /// <summary>
        /// 向后插入值
        /// </summary>
        /// <param name="value">值</param>
        public void push_back(T value) {
            m_size += 1;
            if (m_ary == null) {
                m_ary = new T[m_capacity];
            }
            else {
                if (m_size > m_capacity) {
                    m_capacity += m_step;
                    T[] newAry = new T[m_capacity];
                    for (int i = 0; i < m_size - 1; i++) {
                        newAry[i] = m_ary[i];
                    }
                    m_ary = null;
                    m_ary = newAry;
                }
            }
            m_ary[m_size - 1] = value;
        }

        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="index">索引</param>
        public void remove_at(int index) {
            m_size -= 1;
            for (int i = index; i < m_size; i++) {
                m_ary[i] = m_ary[i + 1];
            }
            if (m_capacity - m_size > m_step) {
                m_capacity -= m_step;
                if (m_capacity > 0) {
                    T[] newAry = new T[m_capacity];
                    for (int i = 0; i < m_size; i++) {
                        newAry[i] = m_ary[i];
                    }
                    m_ary = newAry;
                }
                else {
                    m_ary = null;
                }
            }
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="value">值</param>
        public void set(int index, T value) {
            m_ary[index] = value;
        }

        /// <summary>
        /// 设置容量
        /// </summary>
        /// <param name="capacity">容量</param>
        public void set_capacity(int capacity) {
            m_capacity = capacity;
            if (m_ary != null) {
                T[] newAry = new T[m_capacity];
                for (int i = 0; i < m_size - 1; i++) {
                    newAry[i] = m_ary[i];
                }
                m_ary = null;
                m_ary = newAry;
            }
        }

        /// <summary>
        /// 设置容量增长步长
        /// </summary>
        /// <param name="step">步长</param>
        public void set_step(int step) {
            m_step = step;
        }

        /// <summary>
        /// 获取大小
        /// </summary>
        /// <returns>大小</returns>
        public int size() {
            return m_size;
        }
    }
}
