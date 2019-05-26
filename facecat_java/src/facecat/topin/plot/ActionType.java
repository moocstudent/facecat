/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.plot;

/*
* 动作类型
*/
public enum ActionType {

    AT1(1), //第一个点的动作
    AT2(2), //第二个点的动作
    AT3(3), //第三个点的动作
    AT4(4), //第四个点的动作
    MOVE(0), //移动
    NO(-1); //不移动

    private int intValue;
    private static java.util.HashMap<Integer, ActionType> mappings;

    private synchronized static java.util.HashMap<Integer, ActionType> getMappings() {
        if (mappings == null) {
            mappings = new java.util.HashMap<Integer, ActionType>();
        }
        return mappings;
    }

    private ActionType(int value) {
        intValue = value;
        ActionType.getMappings().put(value, this);
    }

    public int getValue() {
        return intValue;
    }

    public static ActionType forValue(int value) {
        return getMappings().get(value);
    }
}
