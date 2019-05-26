/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.input;

import facecat.topin.btn.*;
import facecat.topin.core.*;
import java.util.*;

/**
 * 数值显示控件
 */
public class FCSpin extends FCTextBox implements FCTouchEvent {

    /**
     * 创建数值显示控件
     */
    public FCSpin() {
    }

    private int m_tick = 0;

    /**
     * 秒表ID
     */
    private int m_timerID = getNewTimerID();

    protected boolean m_autoFormat = true;

    /**
     * 获取是否自动格式化
     */
    public boolean autoFormat() {
        return m_autoFormat;
    }

    /**
     * 设置是否自动格式化
     */
    public void setautoFormat(boolean value) {
        m_autoFormat = value;
    }

    protected int m_digit = 0;

    /**
     * 获取保留小数的位数
     */
    public int getDigit() {
        return m_digit;
    }

    /**
     * 设置保留小数的位数
     */
    public void setDigit(int value) {
        m_digit = value;
        if (m_autoFormat) {
            if (m_text.equals("")) {
                m_text = "0";
            }
            double text = FCStr.convertStrToDouble(m_text);
            setText(getValueByDigit(getValue(), m_digit));
        }
    }

    protected FCButton m_downButton;

    /**
     * 获取向下按钮
     */
    public FCButton getDownButton() {
        return m_downButton;
    }

    /**
     * 设置向下按钮
     */
    public void setDownButton(FCButton value) {
        m_downButton = value;
    }

    protected boolean m_isAdding = false;

    /**
     * 获取是否正在增量
     */
    public boolean isAdding() {
        return m_isAdding;
    }

    /**
     * 设置是否正在增量
     */
    public void setisAdding(boolean value) {
        if (m_isAdding != value) {
            m_isAdding = value;
            m_tick = 0;
            if (m_isAdding) {
                startTimer(m_timerID, 10);
            } else {
                stopTimer(m_timerID);
            }
        }
    }

    protected boolean m_isReducing = false;

    /**
     * 获取是否正在减量
     */
    public boolean isReducing() {
        return m_isReducing;
    }

    /**
     * 设置是否正在减量
     */
    public void setisReducing(boolean value) {
        if (m_isReducing != value) {
            m_isReducing = value;
            m_tick = 0;
            if (m_isReducing) {
                startTimer(m_timerID, 10);
            } else {
                stopTimer(m_timerID);
            }
        }
    }

    protected double m_maximum = 100;

    /**
     * 获取最大值
     */
    public double getMaximum() {
        return m_maximum;
    }

    /**
     * 设置最大值
     */
    public void setMaximum(double value) {
        m_maximum = value;
        if (getValue() > value) {
            setValue(value);
        }
    }

    protected double m_minimum = 0;

    /**
     * 获取最小值
     */
    public double getMinimum() {
        return m_minimum;
    }

    /**
     * 设置最小值
     */
    public void setMinimum(double value) {
        m_minimum = value;
        if (getValue() < value) {
            setValue(value);
        }
    }

    protected boolean m_showThousands;

    /**
     * 获取是否显示千分位
     */
    public boolean showThousands() {
        return m_showThousands;
    }

    /**
     * 设置是否显示千分位
     */
    public void setshowThousands(boolean value) {
        m_showThousands = value;
    }

    protected double m_step = 1;

    /**
     * 获取数值增减幅度
     */
    public double getStep() {
        return m_step;
    }

    /**
     * 设置数值增减幅度
     */
    public void setStep(double value) {
        m_step = value;
    }

    /**
     * 获取文本
     */
    @Override
    public void setText(String value) {
        super.setText(formatNum(value.replace(",", "")));
    }

    protected FCButton m_upButton;

    /**
     * 获取向上按钮
     */
    public FCButton getUpButton() {
        return m_upButton;
    }

    /**
     * 设置向上按钮
     */
    public void setUpButton(FCButton value) {
        m_upButton = value;
    }

    /**
     * 获取当的数值
     */
    public double getValue() {
        return FCStr.convertStrToDouble(getText().replace(",", ""));
    }

    /**
     * 设置当的数值
     */
    public void setValue(double value) {
        if (value > m_maximum) {
            value = m_maximum;
        }
        if (value < m_minimum) {
            value = m_minimum;
        }
        double oldValue = getValue();
        setText(formatNum(getValueByDigit(value, m_digit)));
        onValueChanged();
    }

    /**
     * 增加指定幅度的数值
     */
    public void add() {
        setValue(getValue() + m_step);
    }

    @Override
    public void callControlTouchEvent(int eventID, Object sender, FCTouchInfo touchInfo) {
        if (eventID == FCEventID.TOUCHDOWN) {
            if (sender == m_downButton) {
                reduce();
                setisReducing(true);
                invalidate();
            } else if (sender == m_upButton) {
                add();
                setisAdding(true);
                invalidate();
            }
        } else if (eventID == FCEventID.TOUCHUP) {
            if (sender == m_downButton) {
                setisReducing(false);
            } else if (sender == m_upButton) {
                setisAdding(false);
            }
        }
    }

    /**
     * 销毁方法
     */
    @Override
    public void delete() {
        if (!isDeleted()) {
            if (m_downButton != null) {
                m_downButton.removeEvent(this, FCEventID.TOUCHDOWN);
                m_downButton.removeEvent(this, FCEventID.TOUCHUP);
            }
            if (m_upButton != null) {
                m_upButton.removeEvent(this, FCEventID.TOUCHDOWN);
                m_upButton.removeEvent(this, FCEventID.TOUCHUP);
            }
        }
        super.delete();
    }

    /**
     * 将文本转化为千分位显示
     *
     * @param inputText 输入文字
     * @returns 千分位文字
     */
    public String formatNum(String inputText) {
        if (m_showThousands) {
            inputText = inputText.replace(",", "");
            String theNewText = "";
            int pos = 0;
            boolean hasMinusSign = false;
            if (inputText.indexOf("-") == 0) {
                hasMinusSign = true;
                inputText = inputText.substring(1);
            }
            String textAfterDot = "";
            boolean hasDot = false;
            if (inputText.contains(".")) {
                hasDot = true;
                textAfterDot = inputText.substring(inputText.indexOf(".") + 1);
                inputText = inputText.substring(0, inputText.indexOf("."));
            }
            pos = inputText.length();
            while (pos >= 0) {
                int logicPos = inputText.length() - pos;
                if ((logicPos % 3) == 0 && logicPos > 1) {
                    if (theNewText.equals("")) {
                        theNewText = inputText.substring(pos, pos + 3);
                    } else {
                        theNewText = inputText.substring(pos, pos + 3) + "," + theNewText;
                    }
                } else {
                    if (pos == 0) {
                        if (theNewText.equals("")) {
                            theNewText = inputText.substring(pos, (logicPos % 3) + pos);
                        } else {
                            theNewText = inputText.substring(pos, (logicPos % 3) + pos) + "," + theNewText;
                        }

                    }
                }
                --pos;
            }
            if (hasMinusSign) {
                theNewText = "-" + theNewText;
            }
            if (hasDot) {
                theNewText = theNewText + "." + textAfterDot;
            }
            if (theNewText.indexOf(".") == 0) {
                theNewText = "0" + theNewText;
            }
            if (theNewText.indexOf("-.") == 0) {
                theNewText = theNewText.substring(0, 1) + "0" + theNewText.substring(1);
            }
            return theNewText;
        } else {
            return inputText;
        }
    }

    /**
     * 获取控件类型
     */
    @Override
    public String getControlType() {
        return "Spin";
    }

    /**
     * 获取属性值
     *
     * @param name 属性名称
     * @param value 返回属性值
     * @param type 返回属性类型
     */
    public void getProperty(String name, RefObject<String> value, RefObject<String> type) {
        if (name.equals("autoformat")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertBoolToStr(autoFormat());
        } else if (name.equals("digit")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertDoubleToStr(getDigit());
        } else if (name.equals("maximum")) {
            type.argvalue = "double";
            value.argvalue = FCStr.convertDoubleToStr(getMaximum());
        } else if (name.equals("minimum")) {
            type.argvalue = "double";
            value.argvalue = FCStr.convertDoubleToStr(getMinimum());
        } else if (name.equals("showthousands")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(showThousands());
        } else if (name.equals("step")) {
            type.argvalue = "double";
            value.argvalue = FCStr.convertDoubleToStr(getStep());
        } else if (name.equals("value")) {
            type.argvalue = "double";
            value.argvalue = FCStr.convertDoubleToStr(getValue());
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
        propertyNames.addAll(Arrays.asList(new String[]{"AutoFormat", "Digit", "Maximum", "Minimum", "ShowThousands", "Step"}));
        return propertyNames;
    }

    /**
     * 根据保留小数的位置将double型转化为String型
     *
     * @param value 值
     * @param digit 保留小数点
     * @returns 数值字符
     */
    protected String getValueByDigit(double value, int digit) {
        if (digit > 0) {
            String format = String.format("%d", digit);
            format = "%." + format + "f";
            String str = String.format(format, value);
            return str;
        } else {
            return String.format("%d", (int) value);
        }
    }

    /**
     * 添加控件方法
     */
    @Override
    public void onLoad() {
        super.onLoad();
        FCHost host = getNative().getHost();
        if (m_downButton == null) {
            FCView tempVar = host.createInternalControl(this, "downbutton");
            m_downButton = (FCButton) ((tempVar instanceof FCButton) ? tempVar : null);
            addControl(m_downButton);
            m_downButton.addEvent(this, FCEventID.TOUCHDOWN);
            m_downButton.addEvent(this, FCEventID.TOUCHUP);
        }
        if (m_upButton == null) {
            FCView tempVar = host.createInternalControl(this, "upbutton");
            m_upButton = (FCButton) ((tempVar instanceof FCButton) ? tempVar : null);
            addControl(m_upButton);
            m_upButton.addEvent(this, FCEventID.TOUCHDOWN);
            m_upButton.addEvent(this, FCEventID.TOUCHUP);
        }
        update();
    }

    /**
     * 粘贴方法
     */
    @Override
    public void onPaste() {
        if (m_autoFormat) {
            callEvents(FCEventID.PASTE);
            FCHost host = getNative().getHost();
            String insert = host.paste();
            if (insert != null && insert.length() > 0) {
                insertWord(insert);
                setText(formatNum(getValueByDigit(getValue(), m_digit)));
                onTextChanged();
                onValueChanged();
                invalidate();
            }
        } else {
            super.onPaste();
        }
    }

    /**
     * 秒表事件
     *
     * @param timerID 秒表ID
     */
    @Override
    public void onTimer(int timerID) {
        super.onTimer(timerID);
        if (timerID == m_timerID) {
            if (m_tick > 20) {
                if (m_tick > 50 || m_tick % 3 == 1) {
                    if (m_isAdding) {
                        add();
                        invalidate();
                    } else if (m_isReducing) {
                        reduce();
                        invalidate();
                    }
                }
            }
            m_tick++;
        }
    }

    /**
     * 数值改变方法
     */
    public void onValueChanged() {
        callEvents(FCEventID.VALUECHANGED);
    }

    /**
     * 减少指定幅度的数值
     */
    public void reduce() {
        setValue(getValue() - m_step);
    }

    /**
     * 设置属性值
     *
     * @param name 属性名称
     * @param value 属性值
     */
    @Override
    public void setProperty(String name, String value) {
        if (name.equals("autoformat")) {
            setautoFormat(FCStr.convertStrToBool(value));
        } else if (name.equals("digit")) {
            setDigit(FCStr.convertStrToInt(value));
        } else if (name.equals("maximum")) {
            setMaximum(FCStr.convertStrToDouble(value));
        } else if (name.equals("minimum")) {
            setMinimum(FCStr.convertStrToDouble(value));
        } else if (name.equals("showthousands")) {
            setshowThousands(FCStr.convertStrToBool(value));
        } else if (name.equals("step")) {
            setStep(FCStr.convertStrToDouble(value));
        } else if (name.equals("value")) {
            setValue(FCStr.convertStrToDouble(value));
        } else {
            super.setProperty(name, value);
        }
    }

    /**
     * 设置区域
     */
    public void setRegion() {
        String textValue = m_text.replace(",", "");
        if (textValue == null || textValue.equals("")) {
            textValue = "0";
        }
        if (textValue.indexOf(".") != -1 && textValue.indexOf(".") == 0) {
            textValue = "0" + textValue;
        }
        double value = FCStr.convertStrToDouble(textValue);
        if (value > getMaximum()) {
            value = getMaximum();
        }
        if (value < getMinimum()) {
            value = getMinimum();
        }
        setText(formatNum(getValueByDigit(value, m_digit)));
    }

    /**
     * 更新布局方法
     */
    @Override
    public void update() {
        super.update();
        int width = getWidth(), height = getHeight();
        int uBottom = 0;
        if (m_upButton != null) {
            int uWidth = m_upButton.getWidth();
            FCPoint location = new FCPoint(width - uWidth, 0);
            m_upButton.setLocation(location);
            FCSize size = new FCSize(uWidth, height / 2);
            m_upButton.setSize(size);
            uBottom = m_upButton.getBottom();
        }
        if (m_downButton != null) {
            int dWidth = m_downButton.getWidth();
            FCPoint location = new FCPoint(width - dWidth, uBottom);
            m_downButton.setLocation(location);
            FCSize size = new FCSize(dWidth, height - uBottom);
            m_downButton.setSize(size);
        }
    }
}
