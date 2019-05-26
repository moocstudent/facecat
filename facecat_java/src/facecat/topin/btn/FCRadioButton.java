/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.btn;

import facecat.topin.core.*;
import java.util.*;

/**
 * 复选框控件
 */
public class FCRadioButton extends FCCheckBox {

    /**
     * 创建复选框
     */
    public FCRadioButton() {
    }

    protected String groupName;

    /**
     * 获取组名
     */
    public String getGroupName() {
        return groupName;
    }

    /**
     * 设置组名
     */
    public void setGroupName(String value) {
        groupName = value;
    }

    /**
     * 获取控件类型
     */
    @Override
    public String getControlType() {
        return "RadioButton";
    }

    /**
     * 获取属性值
     *
     * @param name 属性名称
     * @param value 返回属性值
     * @param type 返回属性类型
     */
    @Override
    public void getProperty(String name, RefObject<String> value, RefObject<String> type) {
        if (name.equals("groupname")) {
            type.argvalue = "text";
            value.argvalue = getGroupName();
        } else {
            super.getProperty(name, value, type);
        }
    }

    /**
     * 获取属性名称列表
     */
    @Override
    public ArrayList<String> getPropertyNames() {
        ArrayList<String> propertyNames = super.getPropertyNames();
        propertyNames.addAll(Arrays.asList(new String[]{"GroupName"}));
        return propertyNames;
    }

    /**
     * 点击方法
     */
    @Override
    public void onClick(FCTouchInfo touchInfo) {
        if (!isChecked()) {
            setChecked(!isChecked());
        }
        callTouchEvents(FCEventID.CLICK, touchInfo.clone());
        invalidate();
    }

    /**
     * 重绘选中按钮方法
     *
     * @param paint 绘图对象
     * @param clipRect 裁剪区域
     */
    @Override
    public void onPaintCheckButton(FCPaint paint, FCRect clipRect) {
        // 绘制背景图
        String bkImage = getPaintingBackImage();
        if (bkImage != null && bkImage.length() > 0) {
            paint.drawImage(bkImage, clipRect);
        } else {
            if (isChecked()) {
                FCRect innerRect = new FCRect(clipRect.left + 2, clipRect.top + 2, clipRect.right - 3, clipRect.bottom - 3);
                if (clipRect.right - clipRect.left < 4 || clipRect.bottom - clipRect.top < 4) {
                    innerRect = clipRect.clone();
                }
                paint.fillEllipse(getPaintingButtonBackColor(), innerRect);
            }
            paint.drawEllipse(getPaintingButtonBorderColor(), 1, 0, clipRect);
        }
    }

    /**
     * 设置属性
     *
     * @param name 属性名称
     * @param value 属性值
     */
    @Override
    public void setProperty(String name, String value) {
        if (name.equals("groupname")) {
            setGroupName(value);
        } else {
            super.setProperty(name, value);
        }
    }

    /**
     * 更新布局方法
     */
    @Override
    public void update() {
        if (isChecked()) {
            ArrayList<FCView> controls = null;
            if (getParent() != null) {
                controls = getParent().getControls();
            } else {
                controls = getNative().getControls();
            }
            // 反选组别相同的项
            int controlSize = controls.size();
            for (int i = 0; i < controlSize; i++) {
                FCRadioButton radioButton = (FCRadioButton) ((controls.get(i) instanceof FCRadioButton) ? controls.get(i) : null);
                if (radioButton != null && radioButton != this) {
                    if (radioButton.getGroupName() != null && radioButton.getGroupName().equals(getGroupName()) && radioButton.isChecked() == true) {
                        radioButton.setChecked(false);
                        radioButton.invalidate();
                    }
                }
            }
        }
    }
}
