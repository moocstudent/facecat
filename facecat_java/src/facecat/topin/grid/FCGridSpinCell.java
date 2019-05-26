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
* 数值型单元格
*/
public class FCGridSpinCell extends FCGridControlCell {

    public FCGridSpinCell() {
        FCSpin spin = new FCSpin();
        spin.setBorderColor(FCColor.None);
        spin.setDisplayOffset(false);
        setControl(spin);
    }

    public final FCSpin getSpin() {
        if (getControl() != null) {
            FCView tempVar = getControl();
            return (FCSpin) ((tempVar instanceof FCSpin) ? tempVar : null);
        } else {
            return null;
        }
    }

    @Override
    public boolean getBool() {
        FCSpin spin = getSpin();
        if (spin != null) {
            return spin.getValue() > 0;
        } else {
            return false;
        }
    }

    @Override
    public double getDouble() {
        FCSpin spin = getSpin();
        if (spin != null) {
            return spin.getValue();
        } else {
            return 0;
        }
    }

    @Override
    public float getFloat() {
        FCSpin spin = getSpin();
        if (spin != null) {
            return (float) spin.getValue();
        } else {
            return 0;
        }
    }

    @Override
    public int getInt() {
        FCSpin spin = getSpin();
        if (spin != null) {
            return (int) spin.getValue();
        } else {
            return 0;
        }
    }

    @Override
    public long getLong() {
        FCSpin spin = getSpin();
        if (spin != null) {
            return (long) spin.getValue();
        } else {
            return 0;
        }
    }

    @Override
    public void setBool(boolean value) {
        FCSpin spin = getSpin();
        if (spin != null) {
            spin.setValue(value ? 1 : 0);
        }
    }

    @Override
    public void setDouble(double value) {
        FCSpin spin = getSpin();
        if (spin != null) {
            spin.setValue(value);
        }
    }

    @Override
    public void setFloat(float value) {
        FCSpin spin = getSpin();
        if (spin != null) {
            spin.setValue((double) value);
        }
    }

    @Override
    public void setInt(int value) {
        FCSpin spin = getSpin();
        if (spin != null) {
            spin.setValue((double) value);
        }
    }

    @Override
    public void setLong(long value) {
        FCSpin spin = getSpin();
        if (spin != null) {
            spin.setValue((double) value);
        }
    }
}
