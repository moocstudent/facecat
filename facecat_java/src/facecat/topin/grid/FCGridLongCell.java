/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.grid;

import facecat.topin.core.*;

/*
* 长整型单元格
*/
public class FCGridLongCell extends FCGridCell {

    public FCGridLongCell() {
    }

    public FCGridLongCell(long value) {
        m_value = value;
    }

    protected long m_value;

    @Override
    public int compareTo(FCGridCell cell) {
        long value = getLong();
        long target = cell.getLong();
        if (value > target) {
            return 1;
        } else if (value < target) {
            return -1;
        } else {
            return 0;
        }
    }

    @Override
    public boolean getBool() {
        return m_value == 0 ? false : true;
    }

    @Override
    public double getDouble() {
        return m_value;
    }

    @Override
    public float getFloat() {
        return m_value;
    }

    @Override
    public int getInt() {
        return (int) m_value;
    }

    @Override
    public long getLong() {
        return m_value;
    }

    @Override
    public String getString() {
        return (new Long(m_value)).toString();
    }

    @Override
    public void setBool(boolean value) {
        m_value = value ? 1 : 0;
    }

    @Override
    public void setDouble(double value) {
        m_value = (long) value;
    }

    @Override
    public void setFloat(float value) {
        m_value = (long) value;
    }

    @Override
    public void setInt(int value) {
        m_value = value;
    }

    @Override
    public void setLong(long value) {
        m_value = value;
    }

    @Override
    public void setString(String value) {
        m_value = (long) FCStr.convertStrToInt(value);
    }
}
