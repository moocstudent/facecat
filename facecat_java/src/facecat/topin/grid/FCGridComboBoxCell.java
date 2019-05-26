/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.grid;

import facecat.topin.input.*;
import facecat.topin.core.*;

/*
* 下拉列表单元格
*/
public class FCGridComboBoxCell extends FCGridControlCell {

    public FCGridComboBoxCell() {
        FCComboBox comboBox = new FCComboBox();
        comboBox.setBorderColor(FCColor.None);
        comboBox.setDisplayOffset(false);
        setControl(comboBox);
    }

    public FCComboBox getComboBox() {
        if (getControl() != null) {
            FCView tempVar = getControl();
            return (FCComboBox) ((tempVar instanceof FCComboBox) ? tempVar : null);
        } else {
            return null;
        }
    }

    @Override
    public boolean getBool() {
        FCComboBox comboBox = getComboBox();
        if (comboBox != null) {
            return comboBox.getSelectedIndex() > 0;
        } else {
            return false;
        }
    }

    @Override
    public double getDouble() {
        FCComboBox comboBox = getComboBox();
        if (comboBox != null) {
            return (double) comboBox.getSelectedIndex();
        } else {
            return 0;
        }
    }

    @Override
    public float getFloat() {
        FCComboBox comboBox = getComboBox();
        if (comboBox != null) {
            return (float) comboBox.getSelectedIndex();
        } else {
            return 0;
        }
    }

    @Override
    public int getInt() {
        FCComboBox comboBox = getComboBox();
        if (comboBox != null) {
            return comboBox.getSelectedIndex();
        } else {
            return 0;
        }
    }

    @Override
    public long getLong() {
        FCComboBox comboBox = getComboBox();
        if (comboBox != null) {
            return (long) comboBox.getSelectedIndex();
        } else {
            return 0;
        }
    }

    @Override
    public String getString() {
        FCComboBox comboBox = getComboBox();
        if (comboBox != null) {
            return comboBox.getSelectedValue();
        } else {
            return "";
        }
    }

    @Override
    public void setBool(boolean value) {
        FCComboBox comboBox = getComboBox();
        if (comboBox != null) {
            comboBox.setSelectedIndex(value ? 1 : 0);
        }
    }

    @Override
    public void setDouble(double value) {
        FCComboBox comboBox = getComboBox();
        if (comboBox != null) {
            comboBox.setSelectedIndex((int) value);
        }
    }

    @Override
    public void setFloat(float value) {
        FCComboBox comboBox = getComboBox();
        if (comboBox != null) {
            comboBox.setSelectedIndex((int) value);
        }
    }

    @Override
    public void setInt(int value) {
        FCComboBox comboBox = getComboBox();
        if (comboBox != null) {
            comboBox.setSelectedIndex(value);
        }
    }

    @Override
    public void setLong(long value) {
        FCComboBox comboBox = getComboBox();
        if (comboBox != null) {
            comboBox.setSelectedIndex((int) value);
        }
    }

    @Override
    public void setString(String value) {
        FCComboBox comboBox = getComboBox();
        if (comboBox != null) {
            comboBox.setSelectedValue(value);
        }
    }
}
