/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.chart;

import facecat.topin.core.*;
import java.util.*;

/*
 * 临时变量
 */
public class CVar {

    protected void finalize() throws Throwable {
        delete();
    }

    /**
     * 创建变量
     *
     * @param type 类型
     */
    public CVar() {

    }

    /**
     * 列表
     */
    public ArrayList<String> m_list;

    /**
     * 哈希表
     */
    public HashMap<String, String> m_map;

    /**
     * 数值
     */
    public double m_num;

    /**
     * 字符串
     */
    public String m_str;

    /**
     * 类型
     */
    public int m_type;

    /**
     * 上级变量
     */
    public CVar m_parent;

    /**
     * 销毁资源
     */
    public void delete() {
        if (m_list != null) {
            m_list.clear();
        }
        if (m_map != null) {
            m_map.clear();
        }
        m_parent = null;
    }

    /**
     * 获取文字
     *
     * @param indicator 指标
     * @param name 名称
     * @returns 数值
     */
    public String getText(FCScript indicator, CVariable name) {
        if (m_type == 1) {
            if (m_str.length() > 0 && m_str.startsWith("\'")) {
                return m_str.substring(1, m_str.length() - 1);
            } else {
                return m_str;
            }
        } else {
            return FCStr.convertDoubleToStr(m_num);
        }
    }

    /**
     * 获取值
     *
     * @param indicator 指标
     * @param name 名称
     * @returns 数值
     */
    public double getValue(FCScript indicator, CVariable name) {
        if (m_type == 1) {
            return FCStr.convertStrToDouble(m_str.replace("\'", ""));
        } else {
            return m_num;
        }
    }

    /**
     * 创建变量
     *
     * @param indicator 指标
     * @param name 名称
     * @param value 值
     */
    public double onCreate(FCScript indicator, CVariable name, CVariable value) {
        double result = 0;
        int id = name.m_field;
        if (value.m_expression.length() > 0 && value.m_expression.startsWith("\'")) {
            m_type = 1;
            m_str = value.m_expression.substring(1, value.m_expression.length() - 1);
        } else {
            if (value.m_expression.equals("LIST")) {
                m_type = 2;
                m_list = new ArrayList<String>();
            } else if (value.m_expression.equals("MAP")) {
                m_type = 3;
                m_map = new HashMap<String, String>();
            } else if (indicator.m_tempVars.containsKey(value.m_field)) {
                CVar otherCVar = indicator.m_tempVars.get(value.m_field);
                if (otherCVar.m_type == 1) {
                    m_type = 1;
                    m_str = otherCVar.m_str;
                } else {
                    m_type = 0;
                    m_num = otherCVar.m_num;
                }
            } else {
                m_type = 0;
                result = indicator.getValue(value);
                m_num = result;
            }
        }
        return result;
    }

    /**
     * 设置值
     *
     * @param indicator 指标
     * @param name 名称
     * @param value 值
     */
    public void setValue(FCScript indicator, CVariable name, CVariable value) {
        if (m_type == 1) {
            m_str = indicator.getText(value);
        } else {
            m_num = indicator.getValue(value);
        }
    }
}
