/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.div;

import facecat.topin.core.*;
import java.util.*;

/**
 * 行的样式
 */
public class FCRowStyle implements FCProperty {

    /**
     * 创建行的样式
     *
     * @param sizeType 调整大小的类型
     * @param height 高度
     */
    public FCRowStyle(FCSizeType sizeType, float height) {
        m_sizeType = sizeType;
        m_height = height;
    }

    protected float m_height;

    /**
     * 获取宽度
     */
    public float getHeight() {
        return m_height;
    }

    /**
     * 设置宽度
     */
    public void setHeight(float value) {
        m_height = value;
    }

    protected FCSizeType m_sizeType = FCSizeType.AbsoluteSize;

    /**
     * 获取调整大小的类型
     */
    public FCSizeType getSizeType() {
        return m_sizeType;
    }

    /**
     * 设置调整大小的类型
     */
    public void setSizeType(FCSizeType value) {
        m_sizeType = value;
    }

    /**
     * 设置属性值
     *
     * @param name 属性名称
     * @param value 返回属性值
     * @param type 返回属性类型
     */
    public void getProperty(String name, RefObject<String> value, RefObject<String> type) {
        if (name.equals("sizetype")) {
            type.argvalue = "enum:FCSizeType";
            if (m_sizeType == FCSizeType.AbsoluteSize) {
                value.argvalue = "absolutesize";
            } else if (m_sizeType == FCSizeType.AutoFill) {
                value.argvalue = "autofill";
            } else if (m_sizeType == FCSizeType.PercentSize) {
                value.argvalue = "percentsize";
            }
        } else if (name.equals("height")) {
            type.argvalue = "float";
            value.argvalue = FCStr.convertFloatToStr(getHeight());
        }
    }

    /**
     * 获取属性名称列表
     */
    public ArrayList<String> getPropertyNames() {
        ArrayList<String> propertyNames = new ArrayList<String>();
        propertyNames.add("SizeType");
        propertyNames.add("Height");
        return propertyNames;
    }

    /**
     * 设置属性
     *
     * @param name 属性名称
     * @param value 属性值
     */
    public void setProperty(String name, String value) {
        if (name.equals("sizetype")) {
            String lowerStr = value.toLowerCase();
            if (value.equals("absolutesize")) {
                m_sizeType = FCSizeType.AbsoluteSize;
            } else if (value.equals("autofill")) {
                m_sizeType = FCSizeType.AutoFill;
            } else if (value.equals("percentsize")) {
                m_sizeType = FCSizeType.PercentSize;
            }
        } else if (name.equals("height")) {
            setHeight(FCStr.convertStrToFloat(value));
        }
    }
}
