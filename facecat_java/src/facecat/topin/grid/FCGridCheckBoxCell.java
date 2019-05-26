/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.grid;

import facecat.topin.btn.*;
import facecat.topin.core.*;

/*
* 复选框单元格
*/
public class FCGridCheckBoxCell extends FCGridControlCell {
    public FCGridCheckBoxCell() {
        FCCheckBox checkBox = new FCCheckBox();
        checkBox.setDisplayOffset(false);
        checkBox.setButtonAlign(FCHorizontalAlign.Center);
        setControl(checkBox);
    }

    public FCCheckBox getCheckBox() {
        if (getControl() != null) {
            FCView tempVar = getControl();
            return (FCCheckBox) ((tempVar instanceof FCCheckBox) ? tempVar : null);
        } else {
            return null;
        }
    }

    @Override
    public boolean getBool() {
        FCCheckBox checkBox = getCheckBox();
        if (checkBox != null) {
            return checkBox.isChecked();
        } else {
            return false;
        }
    }

    @Override
    public double getDouble() {
        FCCheckBox checkBox = getCheckBox();
        if (checkBox != null) {
            return checkBox.isChecked() ? 1 : 0;
        } else {
            return 0;
        }
    }

    @Override
    public float getFloat() {
        FCCheckBox checkBox = getCheckBox();
        if (checkBox != null) {
            return checkBox.isChecked() ? 1 : 0;
        } else {
            return 0;
        }
    }

    @Override
    public int getInt() {
        FCCheckBox checkBox = getCheckBox();
        if (checkBox != null) {
            return checkBox.isChecked() ? 1 : 0;
        } else {
            return 0;
        }
    }

    @Override
    public long getLong() {
        FCCheckBox checkBox = getCheckBox();
        if (checkBox != null) {
            return checkBox.isChecked() ? 1 : 0;
        } else {
            return 0;
        }
    }

    @Override
    public String getString() {
        FCCheckBox checkBox = getCheckBox();
        if (checkBox != null) {
            return checkBox.isChecked() ? "true" : "false";
        } else {
            return "false";
        }
    }

    @Override
    public void setBool(boolean value) {
        FCCheckBox checkBox = getCheckBox();
        if (checkBox != null) {
            checkBox.setChecked(value);
        }
    }

    @Override
    public void setDouble(double value) {
        FCCheckBox checkBox = getCheckBox();
        if (checkBox != null) {
            checkBox.setChecked(value > 0 ? true : false);
        }
    }

    @Override
    public void setFloat(float value) {
        FCCheckBox checkBox = getCheckBox();
        if (checkBox != null) {
            checkBox.setChecked(value > 0 ? true : false);
        }
    }

    @Override
    public void setInt(int value) {
        FCCheckBox checkBox = getCheckBox();
        if (checkBox != null) {
            checkBox.setChecked(value > 0 ? true : false);
        }
    }

    @Override
    public void setLong(long value) {
        FCCheckBox checkBox = getCheckBox();
        if (checkBox != null) {
            checkBox.setChecked(value > 0 ? true : false);
        }
    }

    @Override
    public void setString(String value) {
        FCCheckBox checkBox = getCheckBox();
        if (checkBox != null) {
            checkBox.setChecked(value.equals("true"));
        }
    }
}
