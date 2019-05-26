/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.service;
import java.util.*;

/**
 * 消息监听
 *
 */
public class FCMessageListener {

    /**
     * 创建消息监听
     *
     */
    public FCMessageListener() {
    }

    /**
     * 析构方法
     *
     */
    protected void finalize() throws Throwable {
        clear();
    }

    /**
     * 监听回调列表
     *
     */
    private ArrayList<FCListenerMessageCallBack> m_callBacks = new ArrayList<FCListenerMessageCallBack>();

    /**
     * 添加回调
     *
     * @param callBack 回调
     */
    public final void add(FCListenerMessageCallBack callBack) {
        m_callBacks.add(callBack);
    }

    /**
     * 回调方法
     *
     */
    public final void callBack(FCMessage message) {
        int callBackSize = m_callBacks.size();
        for (int i = 0; i < callBackSize; i++) {
            m_callBacks.get(i).callListenerMessageEvent(message);
        }
    }

    /**
     * 清除监听
     *
     */
    public final void clear() {
        m_callBacks.clear();
    }

    /**
     * 移除回调
     *
     * @param callBack 回调
     */
    public final void remove(FCListenerMessageCallBack callBack) {
        m_callBacks.remove(callBack);
    }
}
