/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.service;

import java.util.Scanner;
import facecat.topin.chart.*;

/**
 * 基础方法库
 */
public class CFunctionBase extends CFunction {

    /**
     * 创建方法
     *
     * @param indicator 指标
     * @param id ID
     * @param name 名称
     */
    public CFunctionBase(FCScript indicator, int id, String name) {
        m_indicator = indicator;
        m_ID = id;
        m_name = name;
    }

    /**
     * 指标
     *
     */
    public FCScript m_indicator;

    /**
     * 方法
     */
    private static String FUNCTIONS = "IN,OUT,SLEEP";

    /**
     * 前缀
     */
    private static String PREFIX = "";

    /**
     * 开始索引
     */
    private static final int STARTINDEX = 1000;

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
            indicator.addFunction(new CFunctionBase(indicator, STARTINDEX + i, PREFIX + functions[i]));
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
                return IN(var);
            case STARTINDEX + 1:
                return OUT(var);
            case STARTINDEX + 2:
                return SLEEP(var);
            default:
                return 0;
        }
    }

    /**
     * 输入函数
     *
     * @param var 变量
     * @return 状态
     */
    private double IN(CVariable var) {
        Scanner sc = new Scanner(System.in);
        CVariable newVar = new CVariable(m_indicator);
        newVar.m_expression = "'" + sc.next() + "'";
        m_indicator.setVariable(var.m_parameters[0], newVar);
        return 0;
    }

    /**
     * 输出函数
     *
     * @param var 变量
     * @return 状态
     */
    private double OUT(CVariable var) {
        int len = var.m_parameters.length;
        for (int i = 0; i < len; i++) {
            String text = m_indicator.getText(var.m_parameters[i]);
            System.out.print(text);
        }
        System.out.println("");
        return 0;
    }

    /**
     * 睡眠
     *
     * @param var 变量
     * @return 状态
     */
    private double SLEEP(CVariable var) {
        try {
            Thread.sleep((int) m_indicator.getValue(var.m_parameters[0]));
        } catch (Exception ex) {
        }
        return 1;
    }
}
