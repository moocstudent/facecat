/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.chart;

/**
 * 变量
 */
public class CVariable {

    /**
     * 创建泛型集合
     *
     * @param capacity 容量
     */
    public CVariable(FCScript indicator) {
        m_indicator = indicator;
    }

    /**
     * 柱状图
     */
    public BarShape m_barShape;

    /**
     * K线
     */
    public CandleShape m_candleShape;

    /**
     * 表达式
     */
    public String m_expression;

    /**
     * 字段
     */
    public int m_field = FCDataTable.NULLFIELD;

    /**
     * 字段的索引
     */
    public int m_fieldIndex = -1;

    /**
     * 显示字段
     */
    public String m_fieldText;

    /**
     * 方法的编号
     */
    public int m_functionID = -1;

    /**
     * 方法名称
     */
    public String m_funcName;

    /**
     * 指标
     */
    public FCScript m_indicator;

    /**
     * 行数
     */
    public int m_line = -1;

    /**
     * 折线图
     */
    public PolylineShape m_polylineShape;

    /**
     * 键值
     */
    public String m_name;

    /**
     * 变量
     */
    public CVariable[] m_parameters;

    /**
     * 分割后的表达式
     */
    public CMathElement[] m_splitExpression;

    /**
     * 临时字段
     */
    public int[] m_tempFields;

    /**
     * 置临时字段的索引
     */
    public int[] m_tempFieldsIndex;

    /**
     * 文字
     */
    public TextShape m_textShape;

    /**
     * 类型
     */
    public int m_type = 0;

    /**
     * 常量数值
     */
    public double m_value;

    /**
     * 创建空的字段
     *
     * @param count 数量
     */
    public void createTempFields(int count) {
        m_tempFields = new int[count];
        m_tempFieldsIndex = new int[count];
        for (int i = 0; i < count; i++) {
            int field = FCDataTable.getAutoField();
            m_tempFields[i] = field;
            m_indicator.getDataSource().addColumn(field);
            m_tempFieldsIndex[i] = m_indicator.getDataSource().getColumnIndex(field);
        }
    }
}
