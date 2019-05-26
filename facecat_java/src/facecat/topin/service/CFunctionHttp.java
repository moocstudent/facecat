/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.service;

import facecat.topin.chart.*;

/**
 * HTTP库
 */
public class CFunctionHttp extends CFunction {

    /**
     * 创建方法
     *
     * @param indicator 指标
     * @param id ID
     * @param name 名称
     * @inative XML
     */
    public CFunctionHttp(FCScript indicator, int id, String name) {
        m_indicator = indicator;
        m_ID = id;
        m_name = name;
    }

    /**
     * HTTP对象
     *
     */
    public FCHttpData m_data;

    /**
     * 指标
     *
     */
    public FCScript m_indicator;

    /// <summary>
    /// 方法
    /// </summary>
    private static String FUNCTIONS = "POSTSTRING,QUERYSTRING,WRITE,ADDPORT,GETREQUESTURL,GETREQUESTMETHOD"
            + ",GETCONTENTTYPE,SETSTATUSCODE,GETSERVICENAME,CHECKSCRIPT,CLOSE,HARDREQUEST,EASYREQUEST,GETREMOTEIP,GETREMOTEPORT";

    /// <summary>
    /// 前缀
    /// </summary>
    private static String PREFIX = "HTTP.";

    /// <summary>
    /// 开始索�?
    /// </summary>
    private static final int STARTINDEX = 4000;

    /**
     * 添加方法
     *
     * @param indicator 脚本
     * @param inative XML
     * @return 指标
     */
    public static void addFunctions(FCScript indicator) {
        String[] functions = FUNCTIONS.split("[,]");
        int functionsSize = functions.length;
        for (int i = 0; i < functionsSize; i++) {
            indicator.addFunction(new CFunctionHttp(indicator, STARTINDEX + i, PREFIX + functions[i]));
        }
    }

    /**
     * 计算
     *
     * @param var 变量
     * @return 结果
     */
    @Override
    public double onCalculate(CVariable var) {
        switch (var.m_functionID) {
            case STARTINDEX:
                return HTTP_POSTSTRING(var);
            case STARTINDEX + 1:
                return HTTP_QUERYSTRING(var);
            case STARTINDEX + 2:
                return HTTP_WRITE(var);
            case STARTINDEX + 3:
                return HTTP_ADDPORT(var);
            case STARTINDEX + 4:
                return HTTP_GETREQUESTURL(var);
            case STARTINDEX + 5:
                return HTTP_GETREQUESTMETHOD(var);
            case STARTINDEX + 6:
                return HTTP_GETCONTENTTYPE(var);
            case STARTINDEX + 7:
                return HTTP_SETSTATUSCODE(var);
            case STARTINDEX + 8:
                return HTTP_GETSERVICENAME(var);
            case STARTINDEX + 9:
                return HTTP_CHECKSCRIPT(var);
            case STARTINDEX + 10:
                return HTTP_CLOSE(var);
            case STARTINDEX + 11:
                return HTTP_HARDREQUEST(var);
            case STARTINDEX + 12:
                return HTTP_EASYREQUEST(var);
            case STARTINDEX + 13:
                return HTTP_GETREMOTEIP(var);
            case STARTINDEX + 14:
                return HTTP_GETREMOTEPORT(var);
            default:
                return 0;
        }
    }

    /**
     * 添加前缀
     *
     * @param var 变量
     * @return 状�?
     */
    private double HTTP_ADDPORT(CVariable var) {
        FCHttpMonitor.getMainMonitor().setPort((int) m_indicator.getValue(var.m_parameters[0]));
        return 0;
    }

    /**
     * 检查脚本
     *
     * @param var 变量
     * @return 状态�
     */
    private double HTTP_CHECKSCRIPT(CVariable var) {
        FCHttpMonitor.getMainMonitor().checkScript();
        return 0;
    }

    /**
     * 关闭连接
     *
     * @param var 变量
     * @return 状态
     */
    private double HTTP_CLOSE(CVariable var) {
        m_data.m_close = true;
        return 0;
    }

    /**
     * 接受简单请求
     *
     * @param var 变量
     * @return 状态
     */
    private double HTTP_EASYREQUEST(CVariable var) {
        return FCHttpMonitor.m_easyServices.get(m_indicator.getText(var.m_parameters[0])).onReceive(m_data);
    }

    /**
     * 获取内容类型方法
     *
     * @param var 变量
     * @return 状态�
     */
    private double HTTP_GETCONTENTTYPE(CVariable var) {
        CVariable newVar = new CVariable(m_indicator);
        newVar.m_expression = "'" + m_data.m_contentType + "'";
        m_indicator.setVariable(var.m_parameters[0], newVar);
        return 0;
    }

    /**
     * 获取IP
     *
     * @param var 变量
     * @return Port
     */
    private double HTTP_GETREMOTEIP(CVariable var) {
        CVariable newVar = new CVariable(m_indicator);
        newVar.m_expression = "'" + m_data.m_remoteIP + "'";
        m_indicator.setVariable(var.m_parameters[0], newVar);
        return 0;
    }

    /**
     * 获取Port
     *
     * @param var 变量
     * @return Port
     */
    private double HTTP_GETREMOTEPORT(CVariable var) {
        return m_data.m_remotePort;
    }

    /**
     * 获取请求方法
     *
     * @param var 变量
     * @return 状�?
     */
    private double HTTP_GETREQUESTMETHOD(CVariable var) {
        CVariable newVar = new CVariable(m_indicator);
        newVar.m_expression = "'" + m_data.m_method + "'";
        m_indicator.setVariable(var.m_parameters[0], newVar);
        return 0;
    }

    /**
     * 获取请求URL
     *
     * @param var 变量
     * @return 状态
     */
    private double HTTP_GETREQUESTURL(CVariable var) {
        CVariable newVar = new CVariable(m_indicator);
        newVar.m_expression = "'" + m_data.m_url + "'";
        m_indicator.setVariable(var.m_parameters[0], newVar);
        return 0;
    }

    /**
     * 获取服务名称
     *
     * @param var 变量
     * @return 状态
     */
    private double HTTP_GETSERVICENAME(CVariable var) {
        String url = m_data.m_url;
        int sindex = url.lastIndexOf('/');
        int eindex = url.indexOf('?');
        String text = "";
        if (eindex != -1) {
            text = url.substring(sindex + 1, sindex + 1 + eindex - sindex - 1);
        } else {
            if (sindex + 1 == url.length()) {
                url = url.substring(0, url.length() - 1);
                sindex = url.lastIndexOf('/');
            }
            text = url.substring(sindex + 1);
        }
        CVariable newVar = new CVariable(m_indicator);
        newVar.m_expression = "'" + text + "'";
        m_indicator.setVariable(var.m_parameters[0], newVar);
        return 0;
    }

    /**
     * 接受困难请求
     *
     * @param var 变量
     * @return 状态
     */
    private double HTTP_HARDREQUEST(CVariable var) {
        FCServerService.callBack(m_data.m_socketID, 0, m_data.m_body, m_data.m_body.length);
        return 0;
    }

    /**
     * HTTP获取POST请求的参数
     *
     * @param var 变量
     * @return 状态
     */
    private double HTTP_POSTSTRING(CVariable var) {
        String text = m_data.m_body.toString();
        CVariable newVar = new CVariable(m_indicator);
        newVar.m_expression = "'" + text + "'";
        m_indicator.setVariable(var.m_parameters[0], newVar);
        return 0;
    }

    /**
     * HTTP获取GET请求的参数
     *
     * @param var 变量
     * @return 状态
     */
    private double HTTP_QUERYSTRING(CVariable var) {
        String name = m_indicator.getText(var.m_parameters[1]).toLowerCase();
        String text = "";
        if (m_data.m_parameters.containsKey(name)) {
            text = m_data.m_parameters.get(name);
        }
        CVariable newVar = new CVariable(m_indicator);
        newVar.m_expression = "'" + text + "'";
        m_indicator.setVariable(var.m_parameters[0], newVar);
        return 0;
    }

    /**
     * 设置响应状态码
     *
     * @param var 变量
     * @return 状态
     */
    private double HTTP_SETSTATUSCODE(CVariable var) {
        m_data.m_statusCode = (int) m_indicator.getValue(var.m_parameters[0]);
        return 0;
    }

    /**
     * HTTP响应写流
     *
     * @param var 变量
     * @return 状态
     */
    private double HTTP_WRITE(CVariable var) {
        int len = var.m_parameters.length;
        for (int i = 0; i < len; i++) {
            String text = m_indicator.getText(var.m_parameters[i]);
            m_data.m_resStr += text;
        }
        return 0;
    }
}
