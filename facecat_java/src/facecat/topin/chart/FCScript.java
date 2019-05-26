/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.chart;

import facecat.topin.core.*;
import java.util.*;

/*
 * 指标公式扩展
 */
public class FCScript {

    /**
     * 创建指标
     */
    public FCScript() {
        m_mainVariables = new HashMap<String, Integer>();
        m_variables = new ArrayList<CVariable>();
        m_defineParams = new HashMap<String, Double>();
        m_lines = new ArrayList<CVariable>();
        String[] functions = FUNCTIONS.split("[,]");
        String[] fieldFunctions = FUNCTIONS_FIELD.split("[,]");
        int iSize = functions.length;
        int jSize = fieldFunctions.length;
        for (int i = 0; i < iSize; i++) {
            int cType = 0;
            for (int j = 0; j < jSize; j++) {
                if (functions[i].equals(fieldFunctions[j])) {
                    cType = 1;
                    break;
                }
            }
            CFunction function = new CFunction(i, functions[i]);
            function.m_type = cType;
            m_functions.put(function.m_name, function);
        }
        m_systemColors.add(FCColor.argb(255, 255, 255));
        m_systemColors.add(FCColor.argb(255, 255, 0));
        m_systemColors.add(FCColor.argb(255, 0, 255));
        m_systemColors.add(FCColor.argb(0, 255, 0));
        m_systemColors.add(FCColor.argb(82, 255, 255));
        m_systemColors.add(FCColor.argb(255, 82, 82));
        m_varFactory = new CVarFactory();
    }

    protected void finalize() throws Throwable {
        delete();
    }

    /**
     * 参数列表
     */
    private HashMap<String, Double> m_defineParams;

    /**
     * 所有方法
     */
    private static String FUNCTIONS = "CURRBARSCOUNT,BARSCOUNT,DRAWKLINE,STICKLINE,VALUEWHEN,BARSLAST,DOWNNDAY,DRAWICON,DRAWNULL,FUNCTION,FUNCVAR"
            + ",DRAWTEXT,POLYLINE,BETWEEN,CEILING,EXPMEMA,HHVBARS,INTPART,LLVBARS,DOTIMES,DOWHILE,CONTINUE"
            + ",RETURN,REVERSE,AVEDEV,MINUTE,SQUARE,UPNDAY,DELETE"
            + ",COUNT,CROSS,EVERY,EXIST,EXPMA,FLOOR,MONTH,ROUND,TIME2,WHILE,BREAK,CHUNK"
            + ",ACOS,ASIN,ATAN,DATE,HOUR,LAST,MEMA,NDAY,RAND,SIGN,SQRT,TIME,YEAR"
            + ",ABS,AMA,COS,DAY,DMA,EMA,EXP,HHV,IFF,IFN,LLV,LOG,MAX,MIN"
            + ",MOD,NOT,POW,SIN,SMA,STD,SUM,TAN,REF,SAR,FOR,GET,SET"
            + ",TMA,VAR,WMA,ZIG,IF,MA"
            + ",STR.CONTACT,STR.EQUALS,STR.FIND,STR.FINDLAST,STR.LENGTH,STR.SUBSTR,STR.REPLACE,STR.SPLIT,STR.TOLOWER,STR.TOUPPER,LIST.ADD,LIST.CLEAR,LIST.GET,LIST.INSERT,LIST.REMOVE,LIST.SIZE,MAP.CLEAR,MAP.CONTAINSKEY,MAP.GET,MAP.GETKEYS,MAP.REMOVE,MAP.SET,MAP.SIZE";

    /**
     * 跳出标识
     */
    private int m_break;

    /**
     * 定义同步变量的ID
     */
    private static int FUNCTIONID_FUNCVAR = 10;

    /**
     * 定义方法的ID
     */
    private static int FUNCTIONID_FUNCTION = 9;

    /**
     * 定义变量的ID
     */
    private static int FUNCTIONID_VAR = 82;

    private static String FUNCTIONS_FIELD = "EXPMEMA,EXPMA,MEMA,AMA,DMA,EMA,SMA,SUM,SAR,TMA,WMA,MA";

    /**
     * 方法列表
     */
    private HashMap<String, CFunction> m_functions = new HashMap<String, CFunction>();

    /**
     * 方法的哈希表
     */
    private HashMap<Integer, CFunction> m_functionsMap = new HashMap<Integer, CFunction>();

    /**
     * 语句集合
     */
    private ArrayList<CVariable> m_lines;

    /**
     * 随机数
     */
    private Random m_random = new Random();

    /**
     * 结果
     */
    private double m_result;

    /**
     * 复合结果
     */
    private CVar m_resultVar;

    /**
     * 方法变量表
     */
    private HashMap<String, CVariable> m_tempFunctions = new HashMap<String, CVariable>();

    /**
     * 变量表
     */
    private HashMap<String, CVariable> m_tempVariables = new HashMap<String, CVariable>();

    /**
     * 临时变量
     */
    public HashMap<Integer, CVar> m_tempVars = new HashMap<Integer, CVar>();

    /**
     * 方法标识字符串
     */
    private static String VARIABLE = "~";

    /**
     * 标识字符串2
     */
    private static String VARIABLE2 = "@";

    /**
     * 标识字符串3
     */
    private static String VARIABLE3 = "?";

    /**
     * 方法缓存
     */
    protected ArrayList<CVariable> m_variables;

    protected AttachVScale m_attachVScale = AttachVScale.Left;

    /**
     * 获取在左轴或右轴
     */
    public AttachVScale getAttachVScale() {
        return m_attachVScale;
    }

    /**
     * 设置在左轴或右轴
     */
    public void setAttachVScale(AttachVScale value) {
        m_attachVScale = value;
        for (CVariable var : m_variables) {
            if (var.m_polylineShape != null) {
                var.m_barShape.setAttachVScale(value);
                var.m_candleShape.setAttachVScale(value);
                var.m_polylineShape.setAttachVScale(value);
                var.m_textShape.setAttachVScale(value);
            }
        }
    }

    protected FCDataTable m_dataSource = null;

    /**
     * 获取数据源
     */
    public FCDataTable getDataSource() {
        return m_dataSource;
    }

    /**
     * 设置数据源
     */
    public void setDataSource(FCDataTable value) {
        m_dataSource = value;
    }

    protected ChartDiv m_div = null;

    /**
     * 获取图层
     */
    public ChartDiv getDiv() {
        return m_div;
    }

    /**
     * 设置图层
     */
    public void setDiv(ChartDiv value) {
        m_div = value;
        m_dataSource = m_div.getChart().getDataSource();
    }

    protected boolean m_isDeleted = false;

    /**
     * 获取或设置是否被销毁
     */
    public boolean isDeleted() {
        return m_isDeleted;
    }

    protected int m_index = -1;

    /**
     * 获取当前正在计算的索引
     */
    public int getIndex() {
        return m_index;
    }

    protected HashMap<String, Integer> m_mainVariables;

    /**
     * 获取或设置主要变量
     */
    public HashMap<String, Integer> getMainVariables() {
        return m_mainVariables;
    }

    protected String m_name;

    /**
     * 获取名称
     */
    public String getName() {
        return m_name;
    }

    /**
     * 设置名称
     */
    public void setName(String value) {
        m_name = value;
    }

    /**
     * 获取脚本
     */
    public double getResult() {
        return m_result;
    }

    /**
     * 设置脚本
     */
    public void setScript(String value) {
        synchronized (this) {
            m_lines.clear();
            m_defineParams.clear();
            ArrayList<String> lines = new ArrayList<String>();
            getMiddleScript(value, lines);
            int linesCount = lines.size();
            for (int i = 0; i < linesCount; i++) {
                String strLine = lines.get(i);
                if (strLine.startsWith("FUNCTION ")) {
                    String funcName = strLine.substring(9, strLine.indexOf('(')).toUpperCase();
                    addFunction(new CFunction(FUNCTIONID_FUNCTION, funcName));
                } else if (strLine.startsWith("CONST ")) {
                    String[] consts = strLine.substring(6).split("[:]");
                    m_defineParams.put(consts[0], FCStr.convertStrToDouble(consts[1]));
                    lines.remove(i);
                    i--;
                    linesCount--;
                }
            }
            linesCount = lines.size();
            for (int i = 0; i < linesCount; i++) {
                analysisScriptLine(lines.get(i));
            }
            lines.clear();
        }
    }

    protected ArrayList<Long> m_systemColors = new ArrayList<Long>();

    /**
     * 获取系统颜色
     */
    public ArrayList<Long> getSystemColors() {
        return m_systemColors;
    }

    /**
     * 设置系统颜色
     */
    public void setSystemColors(ArrayList<Long> value) {
        m_systemColors = value;
    }

    protected Object m_tag = null;

    /**
     * 获取TAG值
     */
    public Object getTag() {
        return m_tag;
    }

    /**
     * 设置TAG值
     */
    public void setTag(Object value) {
        m_tag = value;
    }

    protected CVarFactory m_varFactory;

    /**
     * 获取临时变量工厂
     */
    public CVarFactory getVarFactory() {
        return m_varFactory;
    }

    /**
     * 设置临时变量工厂
     */
    public void setVarFactory(CVarFactory value) {
        m_varFactory = value;
    }

    /**
     * 添加方法
     *
     * @param function 方法
     */
    public void addFunction(CFunction function) {
        m_functions.put(function.m_name, function);
        m_functionsMap.put(function.m_ID, function);
    }

    /**
     * 调用方法
     *
     * @param fieldName 方法名称
     * @returns 返回值
     */
    public double callFunction(String funcName) {
        double result = 0;
        synchronized (this) {
            ArrayList<String> lines = new ArrayList<String>();
            getMiddleScript(funcName, lines);
            int linesSize = lines.size();
            m_result = 0;
            for (int i = 0; i < linesSize; i++) {
                String str = lines.get(i);
                int cindex = str.indexOf('(');
                String upperName = str.substring(0, cindex).toUpperCase();
                if (m_tempFunctions.containsKey(upperName)) {
                    CVariable function = m_tempFunctions.get(upperName);
                    int rindex = str.lastIndexOf(')');
                    CVariable topVar = new CVariable(this);
                    if (rindex - cindex > 1) {
                        String pStr = str.substring(cindex + 1, rindex);
                        String[] pList = pStr.split("[" + VARIABLE2 + "]");
                        String[] fieldTexts = function.m_fieldText.split("[" + VARIABLE2 + "]");
                        int pListLen = pList.length;
                        if (!(pListLen == 1 && pList[0].length() == 0)) {
                            topVar.m_parameters = new CVariable[pListLen * 2];
                            for (int j = 0; j < pListLen; j++) {
                                String pName = fieldTexts[j];
                                String pValue = pList[j];
                                CVariable varName = null;
                                if (m_tempVariables.containsKey(pName)) {
                                    varName = m_tempVariables.get(pName);
                                }
                                CVariable varValue = new CVariable(this);
                                varValue.m_expression = pValue;
                                if (pValue.startsWith("\'")) {
                                    varValue.m_type = 1;
                                } else {
                                    varValue.m_type = 3;
                                    varValue.m_value = FCStr.convertStrToDouble(pValue);
                                }
                                topVar.m_parameters[j * 2] = varName;
                                topVar.m_parameters[j * 2 + 1] = varValue;
                            }
                            FUNCVAR(topVar);
                        }
                    }
                    getValue(m_tempFunctions.get(upperName));
                    if (topVar.m_parameters != null) {
                        int variablesSize = topVar.m_parameters.length;
                        for (int j = 0; j < variablesSize; j++) {
                            if (j % 2 == 0) {
                                int id = topVar.m_parameters[j].m_field;
                                if (m_tempVars.containsKey(id)) {
                                    CVar cVar = m_tempVars.get(id);
                                    if (cVar.m_parent != null) {
                                        m_tempVars.put(id, cVar.m_parent);
                                    } else {
                                        m_tempVars.remove(id);
                                    }
                                    cVar.delete();
                                }
                            }
                        }
                    }
                }
            }
            lines.clear();
            result = m_result;
            m_result = 0;
            m_break = 0;
        }
        return result;
    }

    /**
     * 分析语句中包含的方法和变量
     */
    private void analysisVariables(RefObject<String> sentence, int line, String funcName, String fieldText, boolean isFunction) {
        ArrayList<String> wordsList = new ArrayList<String>();
        String[] splitWords = splitExpression2(sentence.argvalue);
        int splitWordsSize = splitWords.length;
        for (int s = 0; s < splitWordsSize; s++) {
            String wStr = splitWords[s];
            String[] subWStr = wStr.split(VARIABLE2 + "|:");
            int subWStrSize = subWStr.length;
            for (int u = 0; u < subWStrSize; u++) {
                if (m_functions.containsKey(subWStr[u])) {
                    wordsList.add(subWStr[u]);
                }
            }
        }
        int wordsListSize = wordsList.size();
        for (int f = 0; f < wordsListSize; f++) {
            String word = wordsList.get(f);
            CFunction func = m_functions.get(word);
            // 系统指标名
            String fName = func.m_name;
            int funcID = func.m_ID;
            int funcType = func.m_type;
            String function = fName + "(";
            // 第一个括号的索引
            int bIndex = sentence.argvalue.indexOf(function);
            while (bIndex != -1) {
                // 右括号索引
                int rightBracket = 0;
                // 字符索引
                int idx = 0;
                // 计数
                int count = 0;
                // 匹配对称括号
                char[] charArray = sentence.argvalue.toCharArray();
                for (char ch : charArray) {
                    if (idx >= bIndex) {
                        if (ch == '(') {
                            count++;
                        } else if (ch == ')') {
                            count--;
                            if (count == 0) {
                                rightBracket = idx;
                                break;
                            }
                        }
                    }
                    idx++;
                }
                if (rightBracket == 0) {
                    break;
                }
                // 方法体
                String body = sentence.argvalue.substring(bIndex, rightBracket + 1);
                // 创建缓存
                CVariable var = new CVariable(this);
                var.m_name = String.format("%s%d", VARIABLE, m_variables.size());
                var.m_expression = body.substring(0, body.indexOf('('));
                var.m_type = 0;
                var.m_functionID = funcID;
                var.m_fieldText = body;
                // 设置字段
                if (funcType == 1) {
                    int field = FCDataTable.getAutoField();
                    var.m_field = field;
                    m_dataSource.addColumn(field);
                }
                m_variables.add(var);
                if (bIndex == 0) {
                    if (isFunction) {
                        var.m_funcName = funcName;
                        var.m_line = line;
                        var.m_fieldText = fieldText;
                        m_lines.add(var);
                        m_tempFunctions.put(funcName, var);
                        isFunction = false;
                    }
                }
                var.m_splitExpression = splitExpression(var.m_expression);
                // 获取子语句
                int startIndex = bIndex + function.length();
                String subSentence = sentence.argvalue.substring(startIndex, rightBracket);
                if (funcID == FUNCTIONID_FUNCTION) {
                    if (m_tempFunctions.containsKey(fName)) {
                        if (m_tempFunctions.get(fName).m_fieldText != null) {
                            String[] fieldTexts = m_tempFunctions.get(fName).m_fieldText.split("[" + VARIABLE2 + "]");
                            String[] transferParams = subSentence.split("[" + VARIABLE2 + "]");
                            subSentence = "";
                            int transferParamsLen = transferParams.length;
                            for (int i = 0; i < transferParamsLen; i++) {
                                if (i == 0) {
                                    subSentence = "FUNCvAR(";
                                }
                                subSentence += fieldTexts[i] + VARIABLE2 + transferParams[i];
                                if (i != transferParamsLen - 1) {
                                    subSentence += VARIABLE2;
                                } else {
                                    subSentence += ")";
                                }
                            }
                        }
                    }
                }
                RefObject<String> tempRef_subSentence = new RefObject<String>(subSentence);
                // 递归解析
                analysisVariables(tempRef_subSentence, 0, "", "", false);
                subSentence = tempRef_subSentence.argvalue;
                String[] parameters = subSentence.split("[" + VARIABLE2 + "]");
                // 保存子语句
                if (parameters != null && parameters.length > 0 && parameters[0].length() > 0) {
                    var.m_parameters = new CVariable[parameters.length];
                    for (int j = 0; j < parameters.length; j++) {
                        String parameter = parameters[j];
                        // 替换参数
                        parameter = replace(parameter);
                        CVariable pVar = new CVariable(this);
                        pVar.m_expression = parameter;
                        pVar.m_name = String.format("%s%d", VARIABLE, m_variables.size());
                        pVar.m_type = 1;
                        var.m_parameters[j] = pVar;
                        // 设置类型和字段
                        for (CVariable variable : m_variables) {
                            // 替换字段
                            if (variable.m_type == 2 && variable.m_expression.equals(parameters[j]) && variable.m_field != FCDataTable.NULLFIELD) {
                                pVar.m_type = 2;
                                pVar.m_field = variable.m_field;
                                pVar.m_fieldText = parameters[j];
                                break;
                            }
                        }
                        if (pVar.m_type == 1) {
                            String varKey = parameter;
                            if (varKey.indexOf("[REF]") == 0) {
                                varKey = varKey.substring(5);
                            }
                            if (m_tempVariables.containsKey(varKey)) {
                                pVar.m_field = m_tempVariables.get(varKey).m_field;
                            } else {
                                pVar.m_field = -m_variables.size();
                                m_tempVariables.put(varKey, pVar);
                            }
                        }
                        m_variables.add(pVar);
                        pVar.m_splitExpression = splitExpression(parameter);
                        if (pVar.m_splitExpression != null && pVar.m_splitExpression.length == 2) {
                            if (pVar.m_splitExpression[0].m_var == pVar) {
                                pVar.m_splitExpression = null;
                            }
                        }
                    }
                }
                // 替换字符串
                sentence.argvalue = sentence.argvalue.substring(0, bIndex) + var.m_name + sentence.argvalue.substring(rightBracket + 1);
                bIndex = sentence.argvalue.indexOf(function, sentence.argvalue.indexOf(var.m_name));
            }
        }
        wordsList.clear();
    }

    /**
     * 加载脚本行
     *
     * @param line 脚本行
     */
    private void analysisScriptLine(String line) {
        CVariable script = new CVariable(this);
        boolean isFunction = false;
        String strLine = line;
        String funcName = null;
        String fieldText = null;
        if (line.startsWith("FUNCTION ")) {
            int cindex = strLine.indexOf('(');
            funcName = strLine.substring(9, cindex);
            int rindex = strLine.indexOf(')');
            if (rindex - cindex > 1) {
                fieldText = strLine.substring(cindex + 1, rindex);
                String[] pList = fieldText.split("[" + VARIABLE2 + "]");
                int pListSize = pList.length;
                for (int i = 0; i < pListSize; i++) {
                    String str = pList[i];
                    if (str.indexOf("[REF]") != -1) {
                        str = str.substring(5);
                    }
                    String pCmd = "vAR(" + str + VARIABLE2 + "0)";
                    RefObject<String> refCmd = new RefObject<String>(pCmd);
                    analysisVariables(refCmd, 0, "", "", false);
                }
            }
            strLine = strLine.substring(rindex + 1);
            strLine = "CHUNK" + strLine.substring(0, strLine.length() - 1) + ")";
            isFunction = true;
        }
        RefObject<String> refStrLine = new RefObject<String>(strLine);
        analysisVariables(refStrLine, m_lines.size(), funcName, fieldText, isFunction);
        strLine = refStrLine.argvalue;
        script.m_line = m_lines.size();
        if (isFunction) {
            return;
        }
        script.m_name = "";
        // 保存语句
        String variable = null;
        String sentence = null;
        String followParameters = "";
        String op = "";
        char[] charArray = strLine.toCharArray();
        for (char ch : charArray) {
            if (ch != ':' && ch != '=') {
                if (op.length() > 0) {
                    break;
                }
            } else {
                op += (new Character(ch)).toString();
            }
        }
        // 不画线变量
        if (op.equals(":=")) {
            variable = strLine.substring(0, strLine.indexOf(":="));
            sentence = strLine.substring(strLine.indexOf(":=") + 2);
        } // 画线变量
        else if (op.equals(":")) {
            followParameters = "COLORAUTO";
            variable = strLine.substring(0, strLine.indexOf(":"));
            sentence = strLine.substring(strLine.indexOf(":") + 1);
            // 跟随参数
            if (sentence.indexOf(VARIABLE2) != -1) {
                followParameters = sentence.substring(sentence.indexOf(VARIABLE2) + 1);
                sentence = sentence.substring(0, sentence.indexOf(VARIABLE2));
            }
        } // 绘图方法
        else {
            sentence = strLine;
            String[] strs = sentence.split("[" + VARIABLE2 + "]");
            if (strs != null && strs.length > 1) {
                String strVar = strs[0];
                sentence = strVar;
                int idx = FCStr.convertStrToInt(strVar.substring(1));
                if (idx < (int) m_variables.size()) {
                    CVariable var = m_variables.get(idx);
                    // 修改参数
                    int startIndex = 0;
                    if (var.m_parameters == null) {
                        var.m_parameters = new CVariable[strs.length - 1];
                        startIndex = 0;
                    } else {
                        CVariable[] newParameters = new CVariable[var.m_parameters.length + strs.length - 1];
                        for (int i = 0; i < var.m_parameters.length; i++) {
                            newParameters[i] = var.m_parameters[i];
                        }
                        startIndex = var.m_parameters.length;
                        var.m_parameters = newParameters;
                    }
                    for (int i = 1; i < strs.length; i++) {
                        CVariable newVar = new CVariable(this);
                        newVar.m_type = 1;
                        newVar.m_expression = strs[i];
                        var.m_parameters[startIndex + i - 1] = newVar;
                    }
                }
            }
        }
        script.m_expression = replace(sentence);
        m_variables.add(script);
        m_lines.add(script);
        // 变量型语句处理
        if (variable != null) {
            script.m_type = 1;
            // 创建变量
            CVariable pfunc = new CVariable(this);
            pfunc.m_type = 2;
            pfunc.m_name = String.format("%s%d", VARIABLE, m_variables.size());
            // 判断语句是否纯变量
            int field = FCDataTable.NULLFIELD;
            if (sentence.startsWith(VARIABLE)) {
                boolean isNum = isNumeric(sentence.replace(VARIABLE, ""));
                if (isNum) {
                    for (CVariable var : m_variables) {
                        if (var.m_name.equals(sentence) && var.m_field != FCDataTable.NULLFIELD) {
                            field = var.m_field;
                            break;
                        }
                    }
                }
            }
            // 追加画线语句
            if (field == FCDataTable.NULLFIELD) {
                field = FCDataTable.getAutoField();
                m_dataSource.addColumn(field);
            } else {
                script.m_type = 0;
            }
            pfunc.m_field = field;
            pfunc.m_expression = variable;
            pfunc.m_splitExpression = splitExpression(variable);
            m_variables.add(pfunc);
            m_mainVariables.put(variable, field);
            script.m_field = field;
        }
        if (followParameters != null && followParameters.length() > 0) {
            String newLine = null;
            if (followParameters.indexOf("COLORSTICK") != -1) {
                newLine = "sTICKLINE(1" + VARIABLE2 + variable + VARIABLE2 + "0" + VARIABLE2 + "1" + VARIABLE2 + "2" + VARIABLE2 + "DRAWTITLE)";
            } else if (followParameters.indexOf("CIRCLEDOT") != -1) {
                newLine = "dRAWICON(1" + VARIABLE2 + variable + VARIABLE2 + "CIRCLEDOT" + VARIABLE2 + "DRAWTITLE)";
            } else if (followParameters.indexOf("POINTDOT") != -1) {
                newLine = "dRAWICON(1" + VARIABLE2 + variable + VARIABLE2 + "POINTDOT" + VARIABLE2 + "DRAWTITLE)";
            } else {
                newLine = "pOLYLINE(1" + VARIABLE2 + variable + VARIABLE2 + followParameters + VARIABLE2 + "DRAWTITLE)";
            }
            analysisScriptLine(newLine);
        }
        script.m_splitExpression = splitExpression(script.m_expression);
    }

    /**
     * 对表达式进行求值，求值之前会先进行语法校验
     *
     * @param expr 要求值的表达式
     * @returns 求值结果
     */
    private double calculate(CMathElement[] expr) {
        CMathElement[] optr = new CMathElement[expr.length];
        int optrLength = 1;
        CMathElement exp = new CMathElement();
        exp.m_type = 3;
        optr[0] = exp;
        CMathElement[] opnd = new CMathElement[expr.length];
        // 删
        int opndLength = 0;
        int idx = 0;
        CMathElement right = null;
        while (idx < expr.length && (expr[idx].m_type != 3 || optr[optrLength - 1].m_type != 3)) {
            CMathElement Q2 = expr[idx];
            if (Q2.m_type != 0 && Q2.m_type != 3) {
                opnd[opndLength] = Q2;
                opndLength++;
                idx++;
            } else {
                CMathElement Q1 = optr[optrLength - 1];
                int precede = -1;
                if (Q2.m_type == 3) {
                    if (Q1.m_type == 3) {
                        precede = 3;
                    } else {
                        precede = 4;
                    }
                } else {
                    int q1Value = (int) Q1.m_value;
                    int q2Value = (int) Q2.m_value;
                    switch (q2Value) {
                        case 3:
                        case 0:
                        case 13:
                        case 4:
                        case 7:
                        case 1:
                        case 11:
                        case 5:
                        case 8:
                        case 10:
                        case 14:
                            if (Q1.m_type == 3 || (Q1.m_type == 0 && q1Value == 6)) {
                                precede = 7;
                            } else {
                                precede = 4;
                            }
                            break;
                        case 9:
                        case 2:
                            if (Q1.m_type == 0 && (q1Value == 9 || q1Value == 2 || q1Value == 12)) {
                                precede = 4;
                            } else {
                                precede = 7;
                            }
                            break;
                        case 6:
                            precede = 7;
                            break;
                        case 12:
                            if (Q1.m_type == 0 && q1Value == 6) {
                                precede = 3;
                            } else {
                                precede = 4;
                            }
                            break;
                    }
                }
                switch (precede) {
                    case 7:
                        // 栈顶元素优先权低
                        optr[optrLength] = Q2;
                        optrLength++;
                        idx++;
                        break;
                    case 3:
                        // 脱括号并接收下一个字符
                        optrLength--;
                        idx++;
                        break;
                    case 4:
                        // 退栈并将运算结果入栈
                        if (opndLength == 0) {
                            return 0;
                        }
                        int op = (int) Q1.m_value;
                        optrLength--;
                        double opnd1 = 0,
                         opnd2 = 0;
                        CMathElement left = opnd[opndLength - 1];
                        if (left.m_type == 2) {
                            opnd2 = getValue(left.m_var);
                        } else {
                            opnd2 = left.m_value;
                        }
                        if (opndLength > 1) {
                            right = opnd[opndLength - 2];
                            if (right.m_type == 2) {
                                opnd1 = getValue(right.m_var);
                            } else {
                                opnd1 = right.m_value;
                            }
                            opndLength -= 2;
                        } else {
                            opndLength--;
                        }
                        // 获取左右两边的值
                        double result = 0;
                        switch (op) {
                            case 0:
                                result = opnd1 + opnd2;
                                break;
                            case 13:
                                result = opnd1 - opnd2;
                                break;
                            case 9:
                                result = opnd1 * opnd2;
                                break;
                            case 2: {
                                if (opnd2 == 0) {
                                    result = 0;
                                } else {
                                    result = opnd1 / opnd2;
                                }
                                break;
                            }
                            case 14: {
                                if (opnd2 == 0) {
                                    result = 0;
                                } else {
                                    result = opnd1 % opnd2;
                                }
                                break;
                            }
                            case 5:
                                result = (opnd1 >= opnd2 ? 1 : 0);
                                break;
                            case 8:
                                result = (opnd1 <= opnd2 ? 1 : 0);
                                break;
                            case 10: {
                                if ((left.m_var != null && left.m_var.m_functionID == -2) || (right != null && right.m_var != null && right.m_var.m_functionID == -2)) {
                                    if (right != null && left.m_var != null && right.m_var != null) {
                                        if (!getText(left.m_var).equals(getText(right.m_var))) {
                                            result = 1;
                                        }
                                    }
                                } else {
                                    result = (opnd1 != opnd2 ? 1 : 0);
                                }
                                break;
                            }
                            case 3: {

                                if ((left.m_var != null && left.m_var.m_functionID == -2) || (right != null && right.m_var != null && right.m_var.m_functionID == -2)) {
                                    if (right != null && left.m_var != null && right.m_var != null) {
                                        if (getText(left.m_var).equals(getText(right.m_var))) {
                                            result = 1;
                                        }
                                    }
                                } else {
                                    result = (opnd1 == opnd2 ? 1 : 0);
                                }
                                break;
                            }
                            case 4:
                                result = (opnd1 > opnd2 ? 1 : 0);
                                break;
                            case 7:
                                result = (opnd1 < opnd2 ? 1 : 0);
                                break;
                            case 1:
                                if (opnd1 == 1 && opnd2 == 1) {
                                    result = 1;
                                } else {
                                    result = 0;
                                }
                                break;
                            case 11:
                                if (opnd1 == 1 || opnd2 == 1) {
                                    result = 1;
                                } else {
                                    result = 0;
                                }
                                break;
                            default:
                                result = 0;
                        }
                        if (m_break > 0) {
                            return result;
                        } else {
                            CMathElement expression = new CMathElement();
                            expression.m_type = 1;
                            expression.m_value = result;
                            opnd[opndLength] = expression;
                            opndLength++;
                        }
                        break;
                }
            }
        }
        // 获取结果
        if (opndLength > 0) {
            CMathElement rlast = opnd[opndLength - 1];
            if (rlast.m_type == 2) {
                return getValue(rlast.m_var);
            } else {
                return rlast.m_value;
            }
        }
        return 0;
    }

    /**
     * 调用方法
     *
     * @param var 变量
     * @returns 结果
     */
    private double callFunction(CVariable var) {
        switch (var.m_functionID) {
            case 0:
                return CURRBARSSOUNT(var);
            case 1:
                return BARSCOUNT(var);
            case 2:
                return DRAWKLINE(var);
            case 3:
                return STICKLINE(var);
            case 4:
                return VALUEWHEN(var);
            case 5:
                return BARSlAST(var);
            case 6:
                return DOWNNdAY(var);
            case 7:
                return DRAWICON(var);
            case 8:
                return DRAWNULL(var);
            case 9:
                return FUNCTION(var);
            case 10:
                return FUNCVAR(var);
            case 11:
                return DRAWTEXT(var);
            case 12:
                return POLYLINE(var);
            case 13:
                return BETWEEN(var);
            case 14:
                return CEILING(var);
            case 15:
                return EXPMEMA(var);
            case 16:
                return HHVBARS(var);
            case 17:
                return INTPART(var);
            case 18:
                return LLVBARS(var);
            case 19:
                return DOTIMES(var);
            case 20:
                return DOWHILE(var);
            case 21:
                return CONTINUE(var);
            case 22:
                return RETURN(var);
            case 23:
                return REVERSE(var);
            case 24:
                return AVEDEV(var);
            case 25:
                return MINUTE(var);
            case 26:
                return SQUARE(var);
            case 27:
                return UPNDAY(var);
            case 28:
                return DELETE(var);
            case 29:
                return COUNT(var);
            case 30:
                return CROSS(var);
            case 31:
                return EVERY(var);
            case 32:
                return EXIST(var);
            case 33:
                return EMA(var);
            case 34:
                return FLOOR(var);
            case 35:
                return MONTH(var);
            case 36:
                return ROUND(var);
            case 37:
                return TIME2(var);
            case 38:
                return WHILE(var);
            case 39:
                return BREAK(var);
            case 40:
                return CHUNK(var);
            case 41:
                return ACOS(var);
            case 42:
                return ASIN(var);
            case 43:
                return ATAN(var);
            case 44:
                return DATE(var);
            case 45:
                return HOUR(var);
            case 46:
                return LAST(var);
            case 47:
                return MEMA(var);
            case 48:
                return NDAY(var);
            case 49:
                return RAND(var);
            case 50:
                return SIGN(var);
            case 51:
                return SQRT(var);
            case 52:
                return TIME(var);
            case 53:
                return YEAR(var);
            case 54:
                return ABS(var);
            case 55:
                return AMA(var);
            case 56:
                return COS(var);
            case 57:
                return DAY(var);
            case 58:
                return DMA(var);
            case 59:
                return EMA(var);
            case 60:
                return EXP(var);
            case 61:
                return HHV(var);
            case 62:
                return IF(var);
            case 63:
                return IFN(var);
            case 64:
                return LLV(var);
            case 65:
                return LOG(var);
            case 66:
                return MAX(var);
            case 67:
                return MIN(var);
            case 68:
                return MOD(var);
            case 69:
                return NOT(var);
            case 70:
                return POW(var);
            case 71:
                return SIN(var);
            case 72:
                return SMA(var);
            case 73:
                return STD(var);
            case 74:
                return SUM(var);
            case 75:
                return TAN(var);
            case 76:
                return REF(var);
            case 77:
                return SAR(var);
            case 78:
                return FOR(var);
            case 79:
                return GET(var);
            case 80:
                return SET(var);
            case 81:
                return TMA(var);
            case 82:
                return VAR(var);
            case 83:
                return WMA(var);
            case 84:
                return ZIG(var);
            case 85:
                return IF(var);
            case 86:
                return MA(var);
            case 87:
                return STR_CONTACT(var);
            case 88:
                return STR_EQUALS(var);
            case 89:
                return STR_FIND(var);
            case 90:
                return STR_FINDlAST(var);
            case 91:
                return STR_LENGTH(var);
            case 92:
                return STR_SUBSTR(var);
            case 93:
                return STR_REPLACE(var);
            case 94:
                return STR_SPLIT(var);
            case 95:
                return STR_TOLOWER(var);
            case 96:
                return STR_TOUPPER(var);
            case 97:
                return LIST_ADD(var);
            case 98:
                return LIST_CLEAR(var);
            case 99:
                return LIST_GET(var);
            case 100:
                return LIST_INSERT(var);
            case 101:
                return LIST_REMOVE(var);
            case 102:
                return LIST_SIZE(var);
            case 103:
                return MAP_CLEAR(var);
            case 104:
                return MAP_CONTAINSKEY(var);
            case 105:
                return MAP_GET(var);
            case 106:
                return MAP_GETKEYS(var);
            case 107:
                return MAP_REMOVE(var);
            case 108:
                return MAP_SET(var);
            case 109:
                return MAP_SIZE(var);
            default:
                if (m_functionsMap.containsKey(var.m_functionID)) {
                    return m_functionsMap.get(var.m_functionID).onCalculate(var);
                }
                return 0;
        }
    }

    /**
     * 清除元素
     */
    public void clear() {
        synchronized (this) {
            // 清除图形
            if (m_div != null) {
                ArrayList<BaseShape> shapes = getShapes();
                for (BaseShape shape : shapes) {
                    m_div.removeShape(shape);
                    m_div.getTitleBar().getTitles().clear();
                    shape.delete();
                }
                if (shapes != null) {
                    shapes.clear();
                }
            }
            // 清除变量
            for (CVariable var : m_variables) {
                if (var.m_field >= 10000) {
                    m_dataSource.removeColumn(var.m_field);
                }
                if (var.m_tempFields != null) {
                    for (int i = 0; i < var.m_tempFields.length; i++) {
                        if (var.m_tempFields[i] >= 10000) {
                            m_dataSource.removeColumn(var.m_tempFields[i]);
                        }
                    }
                }
            }
            m_lines.clear();
            m_variables.clear();
            m_mainVariables.clear();
            m_defineParams.clear();
            m_tempFunctions.clear();
            deleteTempVars();
            m_tempVariables.clear();
        }
    }

    /**
     * 拷贝临时变量
     *
     * @param var 变量
     * @returns 新的变量
     */
    public CVar copyTempVar(CVar var) {
        CVar newVar = new CVar();
        newVar.m_type = var.m_type;
        newVar.m_str = var.m_str;
        newVar.m_num = var.m_num;
        return newVar;
    }

    /**
     * 删除临时变量
     *
     * @param var 变量
     */
    private void deleteTempVars() {
        while (m_tempVars.size() > 0) {
            ArrayList<Integer> removeIDs = new ArrayList<Integer>();
            for (Map.Entry<Integer, CVar> entry : m_tempVars.entrySet()) {
                removeIDs.add(entry.getKey());
            }
            int removeIDsSize = removeIDs.size();
            for (int i = 0; i < removeIDsSize; i++) {
                int removeID = removeIDs.get(i);
                if (m_tempVars.containsKey(removeID)) {
                    CVar cVar = m_tempVars.get(removeID);
                    if (cVar.m_parent != null) {
                        m_tempVars.put(removeID, cVar.m_parent);
                    } else {
                        m_tempVars.remove(removeID);
                    }
                    cVar.delete();
                }
            }
            removeIDs.clear();
        }
    }

    /**
     * 删除临时变量
     *
     * @param var 变量
     */
    private void deleteTempVars(CVariable var) {
        if (var.m_parameters != null) {
            int pLen = var.m_parameters.length;
            if (pLen > 0) {
                for (int i = 0; i < pLen; i++) {
                    CVariable parameter = var.m_parameters[i];
                    if (parameter.m_splitExpression != null && parameter.m_splitExpression.length > 0) {
                        CVariable subVar = parameter.m_splitExpression[0].m_var;
                        if (subVar != null && (subVar.m_functionID == FUNCTIONID_FUNCVAR || subVar.m_functionID == FUNCTIONID_VAR)) {
                            int sunLen = subVar.m_parameters.length;
                            for (int j = 0; j < sunLen; j++) {
                                if (j % 2 == 0) {
                                    CVariable sunVar = subVar.m_parameters[j];
                                    int id = sunVar.m_field;
                                    if (sunVar.m_expression.indexOf("[REF]") == 0) {
                                        int variablesSize = m_variables.size();
                                        for (int k = 0; k < variablesSize; k++) {
                                            CVariable variable = m_variables.get(k);
                                            if (variable.m_expression == sunVar.m_expression) {
                                                variable.m_field = id;
                                            }
                                        }
                                    } else {
                                        if (m_tempVars.containsKey(id)) {
                                            CVar cVar = m_tempVars.get(id);
                                            if (cVar.m_parent != null) {
                                                m_tempVars.put(id, cVar.m_parent);
                                            } else {
                                                m_tempVars.remove(id);
                                            }
                                            cVar.delete();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public static FCScript createIndicator() {
        FCScript indicator = new FCScript();
        return indicator;
    }

    /**
     * 销毁方法
     */
    public void delete() {
        if (!m_isDeleted) {
            clear();
            m_functionsMap.clear();
            m_functions.clear();
            m_isDeleted = true;
        }
    }

    /**
     * 根据字符串获取颜色
     *
     * @param strColor 字符串
     * @returns 颜色
     */
    private long getColor(String strColor) {
        if (strColor.equals("COLORRED")) {
            return FCColor.argb(255, 0, 0);
        } else if (strColor.equals("COLORGREEN")) {
            return FCColor.argb(0, 255, 0);
        } else if (strColor.equals("COLORBLUE")) {
            return FCColor.argb(0, 0, 255);
        } else if (strColor.equals("COLORMAGENTA")) {
            return FCColor.argb(255, 0, 255);
        } else if (strColor.equals("COLORYELLOW")) {
            return FCColor.argb(255, 255, 0);
        } else if (strColor.equals("COLORLIGHTGREY")) {
            return FCColor.argb(211, 211, 211);
        } else if (strColor.equals("COLORLIGHTRED")) {
            return FCColor.argb(255, 82, 82);
        } else if (strColor.equals("COLORLIGHTGREEN")) {
            return FCColor.argb(144, 238, 144);
        } else if (strColor.equals("COLORLIGHTBLUE")) {
            return FCColor.argb(173, 216, 230);
        } else if (strColor.equals("COLORBLACK")) {
            return FCColor.argb(0, 0, 0);
        } else if (strColor.equals("COLORWHITE")) {
            return FCColor.argb(255, 255, 255);
        } else if (strColor.equals("COLORCYAN")) {
            return FCColor.argb(0, 255, 255);
        } else if (strColor.equals("COLORAUTO")) {
            int lineCount = 0;
            long lineColor = FCColor.None;
            for (BaseShape shape : getShapes()) {
                if (shape instanceof PolylineShape) {
                    lineCount++;
                }
            }
            int systemColorsSize = m_systemColors.size();
            if (systemColorsSize > 0) {
                lineColor = m_systemColors.get((lineCount) % systemColorsSize);
            }
            return lineColor;
        } else {
            return 0;
        }
    }

    /**
     * 从数据源中获取计算指标需要的MATH_STRUCT结构
     *
     * @param fieldIndex 字段
     * @param mafieldIndex 保存MA值字段
     * @param index 数据索引
     * @param n 周期
     * @returns MATH_STRUCT结构
     */
    private LPDATA getDatas(int fieldIndex, int mafieldIndex, int index, int n) {
        LPDATA math_struct = new LPDATA();
        // 设置模式为1，效率较高
        math_struct.mode = 1;
        if (index >= 0) {
            // 获取当前的值
            double value = m_dataSource.get3(index, mafieldIndex);
            if (!Double.isNaN(value)) {
                math_struct.lastvalue = value;
                // 获取MA所要获取的第一个值
                if (index >= n - 1) {
                    double nValue = m_dataSource.get3(index + 1 - n, fieldIndex);
                    if (!Double.isNaN(nValue)) {
                        math_struct.first_value = nValue;
                    }
                }
            } else {
                // 设置模式为2，效率较低
                math_struct.mode = 0;
                ArrayList<Double> list = new ArrayList<Double>();
                int start = index - n + 2;
                if (start < 0) {
                    start = 0;
                }
                for (int i = start; i <= index; i++) {
                    double lValue = m_dataSource.get3(i, fieldIndex);
                    if (!Double.isNaN(lValue)) {
                        math_struct.sum += lValue;
                    }
                }
            }
        }
        return math_struct;
    }

    /**
     * 获取所有的方法
     */
    public ArrayList<CFunction> getFunctions() {
        ArrayList<CFunction> functions = new ArrayList<CFunction>();
        for (Map.Entry<String, CFunction> entry : m_functions.entrySet()) {
            functions.add(entry.getValue());
        }
        return functions;
    }

    /**
     * 获取线的宽度
     *
     * @param strLine 线的描述
     * @returns 线宽
     */
    private float getLineWidth(String strLine) {
        float lineWidth = 1;
        if (strLine.length() > 9) {
            lineWidth = FCStr.convertStrToFloat(strLine.substring(9));
        }
        return lineWidth;
    }

    /**
     * 获取操作符号
     *
     * @param op 字符串
     * @returns 操作符号
     */
    private int getOperator(String op) {
        if (op.equals(">=")) {
            return 5;
        } else if (op.equals("<=")) {
            return 8;
        } else if (op.equals("<>") || op.equals("!")) {
            return 10;
        } else if (op.equals("+")) {
            return 0;
        } else if (op.equals(VARIABLE3)) {
            return 13;
        } else if (op.equals("*")) {
            return 9;
        } else if (op.equals("/")) {
            return 2;
        } else if (op.equals("(")) {
            return 6;
        } else if (op.equals(")")) {
            return 12;
        } else if (op.equals("=")) {
            return 3;
        } else if (op.equals(">")) {
            return 4;
        } else if (op.equals("<")) {
            return 7;
        } else if (op.equals("&")) {
            return 1;
        } else if (op.equals("|")) {
            return 11;
        } else if (op.equals("%")) {
            return 14;
        }
        return -1;
    }

    /**
     * 获取中间脚本
     *
     * @param script 脚本
     * @param lines 行
     */
    private int getMiddleScript(String script, ArrayList<String> lines) {
        script = script.replace(" AND ", "&").replace(" OR ", "|");
        String line = "";
        boolean isstr = false;
        char lh = '0';
        boolean isComment = false;
        boolean functionBegin = false;
        int kh = 0;
        boolean isReturn = false, isVar = false, isNewParam = false, isSet = false;
        char[] charArray = script.toCharArray();
        for (char ch : charArray) {
            if ((int) ch == 65279) {
                continue;
            }
            if (ch == '\'') {
                isstr = !isstr;
            }
            if (!isstr) {
                if (ch == '{') {
                    int lineLength = line.length();
                    if (lineLength == 0) {
                        isComment = true;
                    } else {
                        if (!isComment) {
                            kh++;
                            if (functionBegin && kh == 1) {
                                line += "(";
                            } else {
                                if (line.lastIndexOf(")") == lineLength - 1) {
                                    line = line.substring(0, lineLength - 1) + VARIABLE2 + "cHUNK(";
                                } else if (line.lastIndexOf("))" + VARIABLE2 + "ELSE") == lineLength - 7) {
                                    line = line.substring(0, lineLength - 6) + VARIABLE2 + "cHUNK(";
                                }
                            }
                        }
                    }
                } else if (ch == '}') {
                    if (isComment) {
                        isComment = false;
                    } else {
                        kh--;
                        if (functionBegin && kh == 0) {
                            int lineLength = line.length();
                            if (lineLength > 0) {
                                if (line.substring(lineLength - 1).equals(VARIABLE2)) {
                                    line = line.substring(0, lineLength - 1);
                                }
                            }
                            line += ")";
                            lines.add(line);
                            functionBegin = false;
                            line = "";
                        } else {
                            if (kh == 0) {
                                line += "))";
                                lines.add(line);
                                line = "";
                            } else {
                                line += "))" + VARIABLE2;
                            }
                        }
                    }
                } else if (ch == ' ') {
                    int lineLength = line.length();
                    if (line.equals("CONST")) {
                        line = "CONST ";
                    } else if (line.equals("FUNCTION")) {
                        line = "FUNCTION ";
                        functionBegin = true;
                    } else if (!isReturn && (line.lastIndexOf("RETURN") == lineLength - 6)) {
                        if (lineLength == 6 || (line.lastIndexOf(")RETURN") == lineLength - 7
                                || line.lastIndexOf("(RETURN") == lineLength - 7
                                || line.lastIndexOf(VARIABLE2 + "RETURN") == lineLength - 7)) {
                            line += "(";
                            isReturn = true;
                        }
                    } else if (!isVar && line.lastIndexOf("VAR") == lineLength - 3) {
                        if (lineLength == 3 || (line.lastIndexOf(")VAR") == lineLength - 4
                                || line.lastIndexOf("(VAR") == lineLength - 4
                                || line.lastIndexOf(VARIABLE2 + "VAR") == lineLength - 4)) {
                            line += "(";
                            isVar = true;
                            isNewParam = true;
                        }
                    } else if (!isSet && line.lastIndexOf("SET") == lineLength - 3) {
                        if (lineLength == 3 || (line.lastIndexOf(")SET") == lineLength - 4
                                || line.lastIndexOf("(SET") == lineLength - 4
                                || line.lastIndexOf(VARIABLE2 + "SET") == lineLength - 4)) {
                            line = line.substring(0, lineLength - 3) + "sET(";
                            isSet = true;
                            isNewParam = true;
                        }
                    } else {
                        continue;
                    }
                } else if (ch != '\t' && ch != '\r' && ch != '\n') {
                    if (!isComment) {
                        if (ch == '&') {
                            if (lh != '&') {
                                line += ch;
                            }
                        } else if (ch == '|') {
                            if (lh != '|') {
                                line += ch;
                            }
                        } else if (ch == '=') {
                            if (isVar && isNewParam) {
                                isNewParam = false;
                                line += VARIABLE2;
                            } else if (isSet && isNewParam) {
                                isNewParam = false;
                                line += VARIABLE2;
                            } else if (lh != '=' && lh != '!') {
                                line += ch;
                            }
                        } else if (ch == '-') {
                            String strLh = String.valueOf(lh);
                            if (!strLh.equals(VARIABLE2) && getOperator(strLh) != -1
                                    && !strLh.equals(")")) {
                                line += ch;
                            } else {
                                line += VARIABLE3;
                                lh = VARIABLE3.charAt(0);
                                continue;
                            }
                        } else if (ch == ',') {
                            isNewParam = true;
                            line += VARIABLE2;
                        } else if (ch == ';') {
                            if (isReturn) {
                                line += ")";
                                isReturn = false;
                            } else if (isVar) {
                                line += ")";
                                isVar = false;
                            } else if (isSet) {
                                line += ")";
                                isSet = false;
                            } else {
                                int lineLength = line.length();
                                if (line.lastIndexOf("BREAK") == lineLength - 5) {
                                    if (line.lastIndexOf(")BREAK") == lineLength - 6
                                            || line.lastIndexOf("(BREAK") == lineLength - 6
                                            || line.lastIndexOf(VARIABLE2 + "BREAK") == lineLength - 6) {
                                        line += "()";
                                    }
                                } else if (line.lastIndexOf("CONTINUE") == lineLength - 8) {
                                    if (line.lastIndexOf(")CONTINUE") == lineLength - 9
                                            || line.lastIndexOf("(CONTINUE") == lineLength - 9
                                            || line.lastIndexOf(VARIABLE2 + "CONTINUE") == lineLength - 9) {
                                        line += "()";
                                    }
                                }
                            }
                            if (kh > 0) {
                                line += VARIABLE2;
                            } else {
                                lines.add(line);
                                line = "";
                            }
                        } else if (ch == '(') {
                            int lineLength = line.length();
                            if (kh > 0 && line.lastIndexOf("))" + VARIABLE2 + "ELSEIF") == lineLength - 9) {
                                line = line.substring(0, lineLength - 9) + ")" + VARIABLE2;
                            } else {
                                line += "(";
                            }
                        } else {
                            String newStr = String.valueOf(ch).toUpperCase();
                            line += newStr;
                        }
                    }
                }
            } else {
                line += ch;
            }
            lh = ch;
        }
        return 0;
    }

    /**
     * 获取所有的图形
     */
    public ArrayList<BaseShape> getShapes() {
        ArrayList<BaseShape> shapes = new ArrayList<BaseShape>();
        for (CVariable var : m_variables) {
            if (var.m_barShape != null) {
                shapes.add(var.m_barShape);
            }
            if (var.m_candleShape != null) {
                shapes.add(var.m_candleShape);
            }
            if (var.m_polylineShape != null) {
                shapes.add(var.m_polylineShape);
            }
            if (var.m_textShape != null) {
                shapes.add(var.m_textShape);
            }
        }
        return shapes;
    }

    /**
     * 获取文本
     *
     * @param var 变量
     * @returns 文本
     */
    public String getText(CVariable var) {
        if (var.m_expression.length() > 0 && var.m_expression.startsWith("\'")) {
            return var.m_expression.substring(1, var.m_expression.length() - 1);
        } else {
            if (m_tempVars.containsKey(var.m_field)) {
                CVar cVar = m_tempVars.get(var.m_field);
                return cVar.getText(this, var);
            } else {
                return FCStr.convertDoubleToStr(getValue(var));
            }
        }
    }

    /**
     * 获取数值
     *
     * @param var 变量
     * @returns 运算结果
     */
    public double getValue(CVariable var) {
        switch (var.m_type) {
            case 0:
                return callFunction(var);
            case 1:
                if (m_tempVars.containsKey(var.m_field)) {
                    CVar cVar = m_tempVars.get(var.m_field);
                    return cVar.getValue(this, var);
                } else {
                    if (var.m_expression.length() > 0 && var.m_expression.startsWith("\'")) {
                        return FCStr.convertStrToDouble(var.m_expression.substring(1, var.m_expression.length() - 1));
                    } else {
                        if (var.m_splitExpression != null) {
                            return calculate(var.m_splitExpression);
                        } else {
                            return 0;
                        }
                    }
                }
            case 2:
                return m_dataSource.get3(m_index, var.m_fieldIndex);
            case 3:
                return var.m_value;
            default:
                return 0;
        }
    }

    /**
     * 获取变量
     *
     * @param name 名称
     * @returns 变量
     */
    public CVariable getVariable(String name) {
        if (m_tempVariables.containsKey(name)) {
            return m_tempVariables.get(name);
        } else {
            return null;
        }
    }

    public static boolean isNumeric(String str) {
        for (int i = 0; i < str.length(); i++) {
            if (!Character.isDigit(str.charAt(i))) {
                if (str.charAt(i) != '.') {
                    return false;
                }
            }
        }
        return true;
    }

    /**
     * 计算指标
     *
     * @param index 索引
     * @returns 是否计算成功
     */
    public void onCalculate(int index) {
        synchronized (this) {
            if (m_lines != null && m_lines.size() > 0) {
                // 获取列的索引
                for (CVariable sentence : m_lines) {
                    if (sentence.m_field != FCDataTable.NULLFIELD) {
                        sentence.m_fieldIndex = m_dataSource.getColumnIndex(sentence.m_field);
                    }
                }
                for (CVariable var : m_variables) {
                    if (var.m_field != FCDataTable.NULLFIELD) {
                        var.m_fieldIndex = m_dataSource.getColumnIndex(var.m_field);
                    }
                    if (var.m_tempFields != null) {
                        for (int i = 0; i < var.m_tempFields.length; i++) {
                            var.m_tempFieldsIndex[i] = m_dataSource.getColumnIndex(var.m_tempFields[i]);
                        }
                    }
                }
                for (int i = index; i < m_dataSource.getRowsCount(); i++) {
                    m_break = 0;
                    m_index = i;
                    int lineSize = m_lines.size();
                    for (int j = 0; j < lineSize; j++) {
                        CVariable sentence = m_lines.get(j);
                        if (sentence.m_funcName == null || (sentence.m_funcName != null && sentence.m_line != j)) {
                            double value = calculate(sentence.m_splitExpression);
                            if (sentence.m_type == 1 && sentence.m_field != FCDataTable.NULLFIELD) {
                                m_dataSource.set3(i, sentence.m_fieldIndex, value);
                            }
                        }
                        if (m_break == 1) {
                            m_break = 0;
                        }
                    }
                }
            }
        }
    }

    /**
     * 移除方法
     *
     * @param function 方法
     */
    public void removeFunction(CFunction function) {
        m_functions.remove(function.m_name);
        m_functionsMap.remove(function.m_ID);
    }

    /**
     * 替换方法和变量
     *
     * @param parameter 参数
     * @returns 替换后语句
     */
    private String replace(String parameter) {
        // 替换参数
        String[] splitParameters = splitExpression2(parameter);
        for (int p = 0; p < splitParameters.length; p++) {
            String str = splitParameters[p];
            if (m_defineParams.containsKey(str)) {
                splitParameters[p] = m_defineParams.get(str).toString();
            } else {
                for (CVariable varaible : m_variables) {
                    if (varaible.m_type == 2 && varaible.m_expression.equals(str)) {
                        splitParameters[p] = varaible.m_name;
                        break;
                    }
                }
            }
        }
        String newParameter = "";
        for (int p = 0; p < splitParameters.length - 1; p++) {
            newParameter += splitParameters[p];
        }
        return newParameter;
    }

    /**
     * 设置数据源字段
     *
     * @param key 键
     * @param value 值
     */
    public void setSourceField(String key, int value) {
        CVariable pfunc = new CVariable(this);
        pfunc.m_type = 2;
        pfunc.m_name = String.format("%s%d", VARIABLE, m_variables.size());
        pfunc.m_expression = key;
        pfunc.m_splitExpression = splitExpression(key);
        pfunc.m_field = value;
        int columnIndex = m_dataSource.getColumnIndex(value);
        if (columnIndex == -1) {
            m_dataSource.addColumn(value);
        }
        m_variables.add(pfunc);
    }

    /**
     * 设置数据源值
     *
     * @param key 键
     * @param value 值
     */
    public void setSourceValue(int index, String key, double value) {
        CVariable pfunc = null;
        for (CVariable var : m_variables) {
            if (var.m_type == 3 && var.m_expression.equals(key)) {
                pfunc = var;
                break;
            }
        }
        if (pfunc != null) {
            m_dataSource.set2(index, pfunc.m_field, value);
        }
    }

    /**
     * 设置变量的值
     *
     * @param variable 变量
     * @param parameter 值
     * @returns 结果
     */
    public void setVariable(CVariable variable, CVariable parameter) {
        int type = variable.m_type;
        int id = variable.m_field;
        switch (type) {
            case 2:
                double value = getValue(parameter);
                m_dataSource.set3(m_index, variable.m_fieldIndex, value);
                break;
            default:
                if (m_tempVars.containsKey(id)) {
                    CVar cVar = m_tempVars.get(id);
                    cVar.setValue(this, variable, parameter);
                    if (m_resultVar != null) {
                        cVar.m_str = m_resultVar.m_str;
                        m_resultVar = null;
                    }
                } else {
                    variable.m_value = getValue(parameter);
                }
                break;
        }
    }

    /**
     * 把表达式拆分成字符串数组，用于后面的检查和求值
     *
     * @param expression 表达式
     * @returns 从左到右返回从下标0开始的字符串数组
     */
    private CMathElement[] splitExpression(String expression) {
        CMathElement[] exprs = null;
        ArrayList<String> lstItem = new ArrayList<String>();
        int length = expression.length();
        String item = "";
        String ch = "";
        boolean isstr = false;
        while (length != 0) {
            ch = expression.substring(expression.length() - length, expression.length() - length + 1);
            if (ch.equals("\'")) {
                isstr = !isstr;
            }
            if (isstr || getOperator(ch) == -1) {
                item += ch;
            } else {
                if (!item.equals("")) {
                    lstItem.add(item);
                }
                item = "";
                int nextIndex = expression.length() - length + 1;
                String chNext = "";
                if (nextIndex < expression.length() - 1) {
                    chNext = expression.substring(nextIndex, nextIndex + 1);
                }
                String unionText = ch + chNext;
                if (unionText.equals(">=") || unionText.equals("<=") || unionText.equals("<>")) {
                    lstItem.add(unionText);
                    length--;
                } else {
                    lstItem.add(ch);
                }
            }
            length--;
        }
        if (!item.equals("")) {
            lstItem.add(item);
        }
        exprs = new CMathElement[lstItem.size() + 1];
        int lstSize = lstItem.size();
        for (int i = 0; i < lstSize; i++) {
            CMathElement expr = new CMathElement();
            String strExpr = lstItem.get(i);
            int op = getOperator(strExpr);
            if (op != -1) {
                expr.m_type = 0;
                expr.m_value = op;
            } else {
                boolean success = isNumeric(strExpr);
                if (success) {
                    double value = Double.parseDouble(strExpr);
                    expr.m_type = 1;
                    expr.m_value = value;
                } else {
                    for (CVariable var : m_variables) {
                        if (var.m_name.equals(strExpr) || var.m_expression.equals(strExpr)) {
                            expr.m_type = 2;
                            expr.m_var = var;
                            break;
                        }
                    }
                }
            }
            exprs[i] = expr;
        }
        CMathElement lExpr = new CMathElement();
        lExpr.m_type = 3;
        exprs[lstItem.size()] = lExpr;
        return exprs;
    }

    /**
     * 把表达式拆分成字符串数组，用于后面的检查和求值
     *
     * @param expression 表达式
     * @returns 从左到右返回从下标0开始的字符串数组
     */
    private String[] splitExpression2(String expression) {
        String[] exprs = null;
        ArrayList<String> lstItem = new ArrayList<String>();
        int length = expression.length();
        String item = "";
        String ch = "";
        boolean isstr = false;
        while (length != 0) {
            ch = expression.substring(expression.length() - length, expression.length() - length + 1);
            if (ch.equals("\'")) {
                isstr = !isstr;
            }
            if (isstr || getOperator(ch) == -1) {
                item += ch;
            } else {
                if (!item.equals("")) {
                    lstItem.add(item);
                }
                item = "";
                int nextIndex = expression.length() - length + 1;
                String chNext = "";
                if (nextIndex < expression.length() - 1) {
                    chNext = expression.substring(nextIndex, nextIndex + 1);
                }
                String unionText = ch + chNext;
                if (unionText.equals(">=") || unionText.equals("<=") || unionText.equals("<>")) {
                    lstItem.add(unionText);
                    length--;
                } else {
                    lstItem.add(ch);
                }
            }
            length--;
        }
        if (!item.equals("")) {
            lstItem.add(item);
        }
        exprs = new String[lstItem.size() + 1];
        for (int i = 0; i < lstItem.size(); i++) {
            exprs[i] = lstItem.get(i);
        }
        exprs[lstItem.size()] = "#";
        return exprs;
    }

    /**
     * 计算绝对值
     *
     * @param var 变量
     * @returns 绝对值
     */
    private double ABS(CVariable var) {
        return Math.abs(getValue(var.m_parameters[0]));
    }

    /**
     * 计算自适应均线值
     *
     * @param var 变量
     * @returns 自适应均线值
     */
    private double AMA(CVariable var) {
        // 获取计算需要的值
        double close = getValue(var.m_parameters[0]);
        double lastAma = 0;
        if (m_index > 0) {
            lastAma = m_dataSource.get3(m_index - 1, var.m_fieldIndex);
        }
        double n = getValue(var.m_parameters[1]);
        double ama = lastAma + n * (close - lastAma);
        m_dataSource.set3(m_index, var.m_fieldIndex, ama);
        return ama;
    }

    /**
     * 计算反余弦值
     *
     * @param var 变量
     * @returns 反余弦值
     */
    private double ACOS(CVariable var) {
        return Math.acos(getValue(var.m_parameters[0]));
    }

    /**
     * 计算反余弦值
     *
     * @param var 变量
     * @returns 反正弦值
     */
    private double ASIN(CVariable var) {
        return Math.asin(getValue(var.m_parameters[0]));
    }

    /**
     * 计算反正切值
     *
     * @param var 变量
     * @returns 反正切值
     */
    private double ATAN(CVariable var) {
        return Math.atan(getValue(var.m_parameters[0]));
    }

    /**
     * 计算平均绝对偏差
     *
     * @param var 变量
     * @returns 平均绝对偏差
     */
    private double AVEDEV(CVariable var) {
        int p = (int) getValue(var.m_parameters[1]);
        CVariable cParam = var.m_parameters[0];
        int closeFieldIndex = cParam.m_fieldIndex;
        double close = getValue(cParam);
        int closeField = cParam.m_field;
        if (closeFieldIndex == -1) {
            if (var.m_tempFields == null) {
                var.createTempFields(1);
            }
            closeFieldIndex = var.m_tempFieldsIndex[0];
            closeField = var.m_tempFields[0];
            m_dataSource.set3(m_index, closeFieldIndex, close);
        }
        double[] list = m_dataSource.DATA_ARRAY(closeField, m_index, p);
        double avg = 0;
        if (list != null && list.length > 0) {
            double sum = 0;
            for (int i = 0; i < list.length; i++) {
                sum += list[i];
            }
            avg = sum / list.length;
        }
        return FCScript.avedev(close, list, list.length, avg);
    }

    /**
     * 计算数据条目
     *
     * @param var 变量
     * @returns 数据条目
     */
    private int BARSCOUNT(CVariable var) {
        return m_dataSource.getRowsCount();
    }

    /**
     * 计算上一次条件成立到当前的周期数
     *
     * @param var 变量
     * @returns 周期数
     */
    private int BARSlAST(CVariable var) {
        int result = 0;
        int tempIndex = m_index;
        for (int i = m_index; i >= 0; i--) {
            m_index = i;
            double value = getValue(var.m_parameters[0]);
            if (value == 1) {
                break;
            } else {
                if (i == 0) {
                    result = 0;
                } else {
                    result++;
                }
            }
        }
        m_index = tempIndex;
        return result;
    }

    /**
     * 判断表达式
     *
     * @param var 变量
     * @returns 结果
     */
    private int BETWEEN(CVariable var) {
        double value = getValue(var.m_parameters[0]);
        double min = getValue(var.m_parameters[1]);
        double max = getValue(var.m_parameters[2]);
        int result = 0;
        if (value >= min && value <= max) {
            result = 1;
        }
        return result;
    }

    /**
     * 跳出循环
     *
     * @param var 变量
     * @returns 结果
     */
    private int BREAK(CVariable var) {
        m_break = 2;
        return 0;
    }

    /**
     * 计算向上接近的整数
     *
     * @param var 变量
     * @returns 向上接近的整数
     */
    private double CEILING(CVariable var) {
        return Math.ceil(getValue(var.m_parameters[0]));
    }

    /**
     * 执行代码块
     *
     * @param var 变量
     * @returns 返回值
     */
    private double CHUNK(CVariable var) {
        int pLen = var.m_parameters.length;
        if (pLen > 0) {
            for (int i = 0; m_break == 0 && i < pLen; i++) {
                getValue(var.m_parameters[i]);
            }
        }
        deleteTempVars(var);
        return 0;
    }

    /**
     * 计算余弦值
     *
     * @param var 变量
     * @returns 余弦值
     */
    private double COS(CVariable var) {
        return Math.cos(getValue(var.m_parameters[0]));
    }

    /**
     * 继续循环
     *
     * @param var 变量
     * @returns 结果
     */
    private int CONTINUE(CVariable var) {
        m_break = 3;
        return 0;
    }

    /**
     * 统计满足条件的周期数
     *
     * @param var 变量
     * @returns 周期数
     */
    private int COUNT(CVariable var) {
        // 获取计算需要的参数
        int n = (int) getValue(var.m_parameters[1]);
        if (n < 0) {
            n = m_dataSource.getRowsCount();
        } else if (n > m_index + 1) {
            n = m_index + 1;
        }
        int tempIndex = m_index;
        int result = 0;
        for (int i = 0; i < n; i++) {
            if (getValue(var.m_parameters[0]) > 0) {
                result++;
            }
            m_index--;
        }
        m_index = tempIndex;
        return result;
    }

    /**
     * 判断是否穿越
     *
     * @param var 变量
     * @returns 穿越:1 不穿越:0
     */
    private int CROSS(CVariable var) {
        double x = getValue(var.m_parameters[0]);
        double y = getValue(var.m_parameters[1]);
        int result = 0;
        int tempIndex = m_index;
        m_index -= 1;
        if (m_index < 0) {
            m_index = 0;
        }
        double lastX = getValue(var.m_parameters[0]);
        double lastY = getValue(var.m_parameters[1]);
        m_index = tempIndex;
        if (x >= y && lastX < lastY) {
            result = 1;
        }
        return result;
    }

    /**
     * 获取当前数据索引
     *
     * @param var 变量
     * @returns 数据索引
     */
    private int CURRBARSSOUNT(CVariable var) {
        return m_index + 1;
    }

    /**
     * 取得该周期从1900以来的的年月日.
     *
     * @param var 变量
     * @returns 年月日
     */
    private int DATE(CVariable var) {
        int year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0, ms = 0;
        RefObject<Integer> tempRef_year = new RefObject<Integer>(year);
        RefObject<Integer> tempRef_month = new RefObject<Integer>(month);
        RefObject<Integer> tempRef_day = new RefObject<Integer>(day);
        RefObject<Integer> tempRef_hour = new RefObject<Integer>(hour);
        RefObject<Integer> tempRef_minute = new RefObject<Integer>(minute);
        RefObject<Integer> tempRef_second = new RefObject<Integer>(second);
        RefObject<Integer> tempRef_ms = new RefObject<Integer>(ms);
        FCStr.getDataByNum(m_dataSource.getXValue(m_index), tempRef_year, tempRef_month, tempRef_day, tempRef_hour, tempRef_minute, tempRef_second, tempRef_ms);
        year = tempRef_year.argvalue;
        month = tempRef_month.argvalue;
        day = tempRef_day.argvalue;
        hour = tempRef_hour.argvalue;
        minute = tempRef_minute.argvalue;
        second = tempRef_second.argvalue;
        ms = tempRef_ms.argvalue;
        return year * 10000 + month * 100 + day;
    }

    /**
     * 返回日期
     *
     * @param var 变量
     * @returns 日期
     */
    private int DAY(CVariable var) {
        return FCStr.convertNumToDate(m_dataSource.getXValue(m_index)).get(Calendar.DAY_OF_MONTH);
    }

    /**
     * 删除变量
     *
     * @param var 变量
     * @returns 结果
     */
    private int DELETE(CVariable var) {
        int pLen = var.m_parameters.length;
        for (int i = 0; i < pLen; i++) {
            CVariable name = var.m_parameters[i];
            int id = name.m_field;
            if (m_tempVars.containsKey(id)) {
                CVar cVar = m_tempVars.get(id);
                if (cVar.m_parent != null) {
                    m_tempVars.put(id, cVar.m_parent);
                } else {
                    m_tempVars.remove(id);
                }
                cVar.delete();
            }
        }
        return 0;
    }

    /**
     * 计算动态移动平均
     *
     * @param var 变量
     * @returns 动态移动平均
     */
    private double DMA(CVariable var) {
        double close = getValue(var.m_parameters[0]);
        double lastDma = 0;
        if (m_index > 0) {
            lastDma = m_dataSource.get3(m_index - 1, var.m_fieldIndex);
        }
        double n = getValue(var.m_parameters[1]);
        double result = n * close + (1 - n) * lastDma;
        m_dataSource.set3(m_index, var.m_fieldIndex, result);
        return result;
    }

    /**
     * 循环执行一定次数
     *
     * @param var 变量
     * @returns 状态
     */
    private int DOTIMES(CVariable var) {
        int n = (int) getValue(var.m_parameters[0]);
        int pLen = var.m_parameters.length;
        if (pLen > 1) {
            for (int i = 0; i < n; i++) {
                for (int j = 1; m_break == 0 && j < pLen; j++) {
                    getValue(var.m_parameters[j]);
                }
                if (m_break > 0) {
                    if (m_break == 3) {
                        m_break = 0;
                        deleteTempVars(var);
                        continue;
                    } else {
                        m_break = 0;
                        deleteTempVars(var);
                        break;
                    }
                } else {
                    deleteTempVars(var);
                }
            }
        }
        return 0;
    }

    /**
     * 执行DOWHILE循环
     *
     * @param var 变量
     * @returns 状态
     */
    private int DOWHILE(CVariable var) {
        int pLen = var.m_parameters.length;
        if (pLen > 1) {
            while (true) {
                for (int i = 0; m_break == 0 && i < pLen - 1; i++) {
                    getValue(var.m_parameters[i]);
                }
                if (m_break > 0) {
                    if (m_break == 3) {
                        m_break = 0;
                        deleteTempVars(var);
                        continue;
                    } else {
                        m_break = 0;
                        deleteTempVars(var);
                        break;
                    }
                }
                double inLoop = getValue(var.m_parameters[pLen - 1]);
                deleteTempVars(var);
                if (inLoop <= 0) {
                    break;
                }
            }
        }
        return 0;
    }

    /**
     * 返回是否连跌周期数
     *
     * @param var 变量
     * @returns 是否连跌周期数
     */
    private int DOWNNdAY(CVariable var) {
        int n = (int) getValue(var.m_parameters[0]);
        if (n < 0) {
            n = m_dataSource.getRowsCount();
        } else if (n > m_index + 1) {
            n = m_index + 1;
        }
        int tempIndex = m_index;
        int result = 1;
        for (int i = 0; i < n; i++) {
            double right = getValue(var.m_parameters[0]);
            m_index--;
            double left = m_index >= 0 ? getValue(var.m_parameters[0]) : 0;
            if (right >= left) {
                result = 0;
                break;
            }
        }
        m_index = tempIndex;
        return result;
    }

    /**
     * 绘制点图
     *
     * @param var 变量
     * @returns 0
     */
    private double DRAWICON(CVariable var) {
        if (m_div != null) {
            CVariable cond = var.m_parameters[0];
            CVariable price = var.m_parameters[1];
            PolylineShape polylineShape = null;
            if (var.m_polylineShape == null) {
                String strColor = "COLORAUTO";
                String strStyle = "CIRCLEDOT";
                for (int i = 2; i < var.m_parameters.length; i++) {
                    String strParam = var.m_parameters[i].m_expression;
                    if (strParam.startsWith("COLOR")) {
                        strColor = strParam;
                        break;
                    } else if (strParam.equals("CIRCLEDOT")) {
                        strStyle = strParam;
                        break;
                    } else if (strParam.equals("POINTDOT")) {
                        strStyle = strParam;
                        break;
                    }
                }
                if (var.m_expression.equals("DRAWICON")) {
                    strStyle = var.m_expression;
                }
                polylineShape = new PolylineShape();
                m_div.addShape(polylineShape);
                long lineColor = getColor(strColor);
                // 设置属性
                polylineShape.setAttachVScale(m_attachVScale);
                polylineShape.setFieldText(price.m_fieldText);
                polylineShape.setColor(lineColor);
                polylineShape.setStyle(PolylineStyle.Cycle);
                var.createTempFields(1);
                var.m_polylineShape = polylineShape;
            } else {
                polylineShape = var.m_polylineShape;
            }
            // 添加价格字段
            if (price.m_expression != null && price.m_expression.length() > 0) {
                if (polylineShape.getFieldName() == FCDataTable.NULLFIELD) {
                    if (price.m_field != FCDataTable.NULLFIELD) {
                        polylineShape.setFieldName(price.m_field);
                    } else {
                        price.createTempFields(1);
                        polylineShape.setFieldName(price.m_tempFields[0]);
                    }
                    for (int i = 2; i < var.m_parameters.length; i++) {
                        String strParam = var.m_parameters[i].m_expression;
                        if (strParam.equals("DRAWTITLE")) {
                            if (polylineShape.getFieldText() != null) {
                                m_div.getTitleBar().getTitles().add(new ChartTitle(polylineShape.getFieldName(), polylineShape.getFieldText(), polylineShape.getColor(), 2, true));
                            }
                        }
                    }
                }
                if (price.m_tempFields != null) {
                    double value = getValue(price);
                    m_dataSource.set3(m_index, price.m_tempFieldsIndex[0], value);
                }
            }
            // 设置隐藏
            double dCond = 1;
            if (cond.m_expression != null && cond.m_expression.length() > 0 && !cond.m_expression.equals("1")) {
                dCond = getValue(cond);
                if (dCond != 1) {
                    m_dataSource.set3(m_index, var.m_tempFieldsIndex[0], -10000);
                } else {
                    m_dataSource.set3(m_index, var.m_tempFieldsIndex[0], 1);
                }
            }
        }
        return 0;
    }

    /**
     * 绘制K线
     *
     * @param var 变量
     * @returns 0
     */
    private double DRAWKLINE(CVariable var) {
        if (m_div != null) {
            CVariable high = var.m_parameters[0];
            CVariable open = var.m_parameters[1];
            CVariable low = var.m_parameters[2];
            CVariable close = var.m_parameters[3];
            CandleShape candleShape = null;
            if (var.m_candleShape == null) {
                candleShape = new CandleShape();
                candleShape.setHighFieldText(high.m_fieldText);
                candleShape.setOpenFieldText(open.m_fieldText);
                candleShape.setLowFieldText(low.m_fieldText);
                candleShape.setCloseFieldText(close.m_fieldText);
                candleShape.setAttachVScale(m_attachVScale);
                candleShape.setStyle(CandleStyle.Rect);
                m_div.addShape(candleShape);
                var.m_candleShape = candleShape;
            } else {
                candleShape = var.m_candleShape;
            }
            if (high.m_expression != null && high.m_expression.length() > 0) {
                if (candleShape.getHighField() == FCDataTable.NULLFIELD) {
                    if (high.m_field != FCDataTable.NULLFIELD) {
                        candleShape.setHighField(high.m_field);
                    } else {
                        high.createTempFields(1);
                        candleShape.setHighField(high.m_tempFields[0]);
                    }
                }
                if (high.m_tempFields != null) {
                    double value = getValue(high);
                    m_dataSource.set3(m_index, high.m_tempFieldsIndex[0], value);
                }
            }
            // 设置开盘价字段
            if (open.m_expression != null && open.m_expression.length() > 0) {
                if (open.m_field != FCDataTable.NULLFIELD) {
                    candleShape.setOpenField(open.m_field);
                } else {
                    open.createTempFields(1);
                    candleShape.setOpenField(open.m_tempFields[0]);
                }
                if (open.m_tempFields != null) {
                    double value = getValue(open);
                    m_dataSource.set3(m_index, open.m_tempFieldsIndex[0], value);
                }
            }
            // 设置最低价字段
            if (low.m_expression != null && low.m_expression.length() > 0) {
                if (low.m_field != FCDataTable.NULLFIELD) {
                    candleShape.setLowField(low.m_field);
                } else {
                    low.createTempFields(1);
                    candleShape.setLowField(low.m_tempFields[0]);
                }
                if (low.m_tempFields != null) {
                    double value = getValue(low);
                    m_dataSource.set3(m_index, low.m_tempFieldsIndex[0], value);
                }
            }
            // 设置收盘价字段
            if (close.m_expression != null && close.m_expression.length() > 0) {
                if (close.m_field != FCDataTable.NULLFIELD) {
                    candleShape.setCloseField(close.m_field);
                } else {
                    close.createTempFields(1);
                    candleShape.setCloseField(close.m_tempFields[0]);
                }
                if (close.m_tempFields != null) {
                    double value = getValue(close);
                    m_dataSource.set3(m_index, close.m_tempFieldsIndex[0], value);
                }
            }
        }
        return 0;
    }

    /**
     * 返回无效数
     *
     * @param var 变量
     * @returns 无效数
     */
    private double DRAWNULL(CVariable var) {
        return Double.NaN;
    }

    /**
     * 绘制文字
     *
     * @param var 变量
     * @returns 0
     */
    private double DRAWTEXT(CVariable var) {
        if (m_div != null) {
            // 获取参数
            CVariable cond = var.m_parameters[0];
            CVariable price = var.m_parameters[1];
            CVariable text = var.m_parameters[2];
            TextShape textShape = null;
            if (var.m_textShape == null) {
                textShape = new TextShape();
                textShape.setAttachVScale(m_attachVScale);
                textShape.setText(getText(text));
                var.createTempFields(1);
                textShape.setStyleField(var.m_tempFields[0]);
                String strColor = "COLORAUTO";
                for (int i = 3; i < var.m_parameters.length; i++) {
                    String strParam = var.m_parameters[i].m_expression;
                    if (strParam.startsWith("COLOR")) {
                        strColor = strParam;
                        break;
                    }
                }
                if (!strColor.equals("COLORAUTO")) {
                    textShape.setTextColor(getColor(strColor));
                }
                m_div.addShape(textShape);
                var.m_textShape = textShape;
            } else {
                textShape = var.m_textShape;
            }
            // 添加价格字段
            if (price.m_expression != null && price.m_expression.length() > 0) {
                if (textShape.getFieldName() == FCDataTable.NULLFIELD) {
                    if (price.m_field != FCDataTable.NULLFIELD) {
                        textShape.setFieldName(price.m_field);
                    } else {
                        price.createTempFields(1);
                        textShape.setFieldName(price.m_tempFields[0]);
                    }
                }
                if (price.m_tempFields != null) {
                    double value = getValue(price);
                    m_dataSource.set3(m_index, price.m_tempFieldsIndex[0], value);
                }
            }
            // 设置隐藏
            double dCond = 1;
            if (cond.m_expression != null && cond.m_expression.length() > 0 && !cond.m_expression.equals("1")) {
                dCond = getValue(cond);
                if (dCond != 1) {
                    m_dataSource.set3(m_index, var.m_tempFieldsIndex[0], -10000);
                } else {
                    m_dataSource.set3(m_index, var.m_tempFieldsIndex[0], 0);
                }
            }
        }
        return 0;
    }

    /**
     * 判断是否存在
     *
     * @param var 是否存在
     * @returns 0
     */
    private int EXIST(CVariable var) {
        int n = (int) getValue(var.m_parameters[1]);
        if (n < 0) {
            n = m_dataSource.getRowsCount();
        } else if (n > m_index + 1) {
            n = m_index + 1;
        }
        int tempIndex = m_index;
        int result = 0;
        for (int i = 0; i < n; i++) {
            if (getValue(var.m_parameters[0]) > 0) {
                result = 1;
                break;
            }
            m_index--;
        }
        m_index = tempIndex;
        return result;
    }

    /**
     * 计算指数移动平均
     *
     * @param var 是否存在
     * @returns 指数移动平均
     */
    private double EMA(CVariable var) {
        double close = getValue(var.m_parameters[0]);
        double lastEma = 0;
        if (m_index > 0) {
            lastEma = m_dataSource.get3(m_index - 1, var.m_fieldIndex);
        }
        int n = (int) getValue(var.m_parameters[1]);
        double result = exponentialMovingAverage(n, close, lastEma);
        m_dataSource.set3(m_index, var.m_fieldIndex, result);
        return result;
    }

    /**
     * 判断是否一直存在
     *
     * @param var 是否存在
     * @returns 是否一直存在
     */
    private int EVERY(CVariable var) {
        // 获取计算需要的参数
        int n = (int) getValue(var.m_parameters[1]);
        if (n < 0) {
            n = m_dataSource.getRowsCount();
        } else if (n > m_index + 1) {
            n = m_index + 1;
        }
        int tempIndex = m_index;
        int result = 1;
        for (int i = 0; i < n; i++) {
            if (getValue(var.m_parameters[0]) <= 0) {
                result = 0;
                break;
            }
            m_index--;
        }
        m_index = tempIndex;
        return result;
    }

    /**
     * 计算指数平滑移动平均
     *
     * @param var 是否存在
     * @returns 指数平滑移动平均
     */
    private double EXPMEMA(CVariable var) {
        CVariable cParam = var.m_parameters[0];
        double close = getValue(cParam);
        int closeFieldIndex = cParam.m_fieldIndex;
        int n = (int) getValue(var.m_parameters[1]);
        if (var.m_tempFields == null) {
            if (closeFieldIndex == -1) {
                var.createTempFields(2);
            } else {
                var.createTempFields(1);
            }
        }
        if (var.m_tempFields.length == 2) {
            closeFieldIndex = var.m_tempFieldsIndex[1];
            m_dataSource.set3(m_index, closeFieldIndex, close);
        }
        // 计算指标
        int maFieldIndex = var.m_tempFieldsIndex[0];
        double ma = movingAverage(m_index, n, close, getDatas(closeFieldIndex, maFieldIndex, m_index - 1, n));
        m_dataSource.set3(m_index, maFieldIndex, ma);
        double lastEma = 0;
        if (m_index > 0) {
            lastEma = m_dataSource.get3(m_index, var.m_fieldIndex);
        }
        // 计算指标
        double result = exponentialMovingAverage(n, ma, lastEma);
        m_dataSource.set3(m_index, var.m_fieldIndex, result);
        return result;
    }

    /**
     * 计算e的X次幂
     *
     * @param fieldName 字段名称
     * @returns e的X次幂
     */
    private double EXP(CVariable var) {
        return Math.exp(getValue(var.m_parameters[0]));
    }

    /**
     * 计算向下接近的整数
     *
     * @param var 变量
     * @returns 向下接近的整数
     */
    private double FLOOR(CVariable var) {
        return Math.floor(getValue(var.m_parameters[0]));
    }

    /**
     * 执行FOR循环
     *
     * @param fieldName 字段名称
     * @returns 状态
     */
    private int FOR(CVariable var) {
        int pLen = var.m_parameters.length;
        if (pLen > 3) {
            int start = (int) getValue(var.m_parameters[0]);
            int end = (int) getValue(var.m_parameters[1]);
            int step = (int) getValue(var.m_parameters[2]);
            if (step > 0) {
                for (int i = start; i < end; i += step) {
                    for (int j = 3; j < pLen; j++) {
                        getValue(var.m_parameters[j]);
                        if (m_break != 0) {
                            break;
                        }
                    }
                    if (m_break > 0) {
                        if (m_break == 3) {
                            m_break = 0;
                            deleteTempVars(var);
                            continue;
                        } else {
                            m_break = 0;
                            deleteTempVars(var);
                            break;
                        }
                    } else {
                        deleteTempVars(var);
                    }
                }
            } else if (step < 0) {
                for (int i = start; i > end; i += step) {
                    for (int j = 3; j < pLen; j++) {
                        if (m_break != 0) {
                            break;
                        }
                    }
                    if (m_break > 0) {
                        if (m_break == 3) {
                            m_break = 0;
                            deleteTempVars(var);
                            continue;
                        } else {
                            m_break = 0;
                            deleteTempVars(var);
                            break;
                        }
                    } else {
                        deleteTempVars(var);
                    }
                }
            }
        }
        return 0;
    }

    /**
     * 执行方法
     *
     * @param var 变量
     * @returns 返回值
     */
    private double FUNCTION(CVariable var) {
        m_result = 0;
        if (var.m_parameters != null) {
            int pLen = var.m_parameters.length;
            if (pLen > 0) {
                for (int i = 0; i < pLen; i++) {
                    getValue(var.m_parameters[i]);
                }
            }
        }
        String name = var.m_expression;
        if (m_tempFunctions.containsKey(name)) {
            getValue(m_tempFunctions.get(name));
        }
        if (m_break == 1) {
            m_break = 0;
        }
        double result = m_result;
        m_result = 0;
        deleteTempVars(var);
        return result;
    }

    /**
     * 定义变量
     *
     * @param var 变量
     * @returns 数值
     */
    private double FUNCVAR(CVariable var) {
        double result = 0;
        int pLen = var.m_parameters.length;
        HashMap<CVar, Integer> cVars = new HashMap<CVar, Integer>();
        for (int i = 0; i < pLen; i++) {
            if (i % 2 == 0) {
                CVariable name = var.m_parameters[i];
                CVariable value = var.m_parameters[i + 1];
                int id = name.m_field;
                if (name.m_expression.indexOf("[REF]") == 0) {
                    int variablesSize = m_variables.size();
                    for (int j = 0; j < variablesSize; j++) {
                        CVariable variable = m_variables.get(j);
                        if (variable != name) {
                            if (variable.m_field == id) {
                                variable.m_field = value.m_field;
                            }
                        }
                    }
                    continue;
                } else {
                    CVar newCVar = m_varFactory.createVar();
                    result = newCVar.onCreate(this, name, value);
                    if (newCVar.m_type == 1) {
                        name.m_functionID = -2;
                    }
                    cVars.put(newCVar, id);
                }
            }
        }
        for (Map.Entry<CVar, Integer> entry : cVars.entrySet()) {
            int id = entry.getValue();
            CVar newCVar = entry.getKey();
            if (m_tempVars.containsKey(id)) {
                CVar cVar = m_tempVars.get(id);
                newCVar.m_parent = cVar;
            }
            m_tempVars.put(id, newCVar);
        }
        cVars.clear();
        return result;
    }

    /**
     * 计算指定字段一段区间内的最大值
     *
     * @param var 变量
     * @returns 数值
     */
    private double GET(CVariable var) {
        return getValue(var.m_parameters[0]);
    }

    /**
     * 计算指定字段一段区间内的最大值
     *
     * @param var 变量
     * @returns 最大值
     */
    private double HHV(CVariable var) {
        // 获取周期
        int n = (int) getValue(var.m_parameters[1]);
        CVariable cParam = var.m_parameters[0];
        int closeFieldIndex = cParam.m_fieldIndex;
        int closeField = cParam.m_field;
        if (closeFieldIndex == -1) {
            if (cParam.m_tempFields == null) {
                cParam.createTempFields(0);
            }
            closeFieldIndex = cParam.m_tempFieldsIndex[0];
            closeField = cParam.m_tempFields[0];
            double close = getValue(cParam);
            m_dataSource.set3(m_index, closeFieldIndex, close);
        }
        double[] higharray = m_dataSource.DATA_ARRAY(closeField, m_index, n);
        return maxValue(higharray, higharray.length);
    }

    /**
     * 计算指定字段一段区间内的最大值距今天的天数
     *
     * @param var 变量
     * @returns 天数
     */
    private double HHVBARS(CVariable var) {
        // 获取周期
        int n = (int) getValue(var.m_parameters[1]);
        CVariable cParam = var.m_parameters[0];
        int closeField = cParam.m_field;
        int closeFieldIndex = cParam.m_fieldIndex;
        if (closeFieldIndex == -1) {
            if (cParam.m_tempFields == null) {
                cParam.createTempFields(0);
            }
            closeField = cParam.m_tempFields[0];
            closeFieldIndex = cParam.m_tempFieldsIndex[0];
            double close = getValue(cParam);
            m_dataSource.set3(m_index, closeFieldIndex, close);
        }
        double[] higharray = m_dataSource.DATA_ARRAY(closeField, m_index, n);
        double result = 0;
        if (higharray.length > 0) {
            int mIndex = 0;
            double close = 0;
            for (int i = 0; i < higharray.length; i++) {
                if (i == 0) {
                    close = higharray[i];
                    mIndex = 0;
                } else {
                    if (higharray[i] > close) {
                        close = higharray[i];
                        mIndex = i;
                    }
                }
            }
            result = higharray.length - mIndex - 1;
        }
        return result;
    }

    /**
     * 返回小时
     *
     * @param var 变量
     * @returns 小时
     */
    private int HOUR(CVariable var) {
        return FCStr.convertNumToDate(m_dataSource.getXValue(m_index)).get(Calendar.HOUR_OF_DAY);
    }

    /**
     * 选择函数
     *
     * @param var 变量
     * @returns 结果
     */
    private double IF(CVariable var) {
        double result = 0;
        int pLen = var.m_parameters.length;
        for (int i = 0; i < pLen; i++) {
            result = getValue(var.m_parameters[i]);
            if (i % 2 == 0) {
                if (result == 0) {
                    i++;
                    continue;
                }
            } else {
                break;
            }
        }
        deleteTempVars(var);
        return result;
    }

    /**
     * 反选择函数
     *
     * @param var 变量
     * @returns 反选择结果
     */
    private double IFN(CVariable var) {
        double result = 0;
        int pLen = var.m_parameters.length;
        for (int i = 0; i < pLen; i++) {
            result = getValue(var.m_parameters[i]);
            if (i % 2 == 0) {
                if (result != 0) {
                    i++;
                    continue;
                }
            } else {
                break;
            }
        }
        deleteTempVars(var);
        return result;
    }

    /**
     * 计算沿X绝对值减小方向最接近的整数
     *
     * @param var 变量
     * @returns 最接近的整数
     */
    private double INTPART(CVariable var) {
        double result = getValue(var.m_parameters[0]);
        if (result != 0) {
            int intResult = (int) result;
            double sub = Math.abs(result - intResult);
            if (sub >= 0.5) {
                if (result > 0) {
                    result = intResult - 1;
                } else {
                    result = intResult + 1;
                }
            } else {
                result = intResult;
            }
        }
        return result;
    }

    /**
     * 判断是否持续存在
     *
     * @param var 变量
     * @returns 是否持续存在
     */
    private int LAST(CVariable var) {
        // 获取计算需要的参数
        int n = (int) getValue(var.m_parameters[1]);
        int m = (int) getValue(var.m_parameters[2]);
        if (n < 0) {
            n = m_dataSource.getRowsCount();
        } else if (n > m_index + 1) {
            n = m_index + 1;
        }
        if (m < 0) {
            m = m_dataSource.getRowsCount();
        } else if (m > m_index + 1) {
            m = m_index + 1;
        }
        int tempIndex = m_index;
        int result = 1;
        for (int i = m; i < n; i++) {
            m_index = tempIndex - m;
            if (getValue(var.m_parameters[0]) <= 0) {
                result = 0;
                break;
            }
        }
        m_index = tempIndex;
        return result;
    }

    /**
     * 计算指定字段一段区间内的最小值
     *
     * @param var 变量
     * @returns 最小值
     */
    private double LLV(CVariable var) {
        // 获取区间
        int n = (int) getValue(var.m_parameters[1]);
        CVariable cParam = var.m_parameters[0];
        int closeField = cParam.m_field;
        int closeFieldIndex = cParam.m_fieldIndex;
        if (closeField == FCDataTable.NULLFIELD) {
            if (cParam.m_tempFields == null) {
                cParam.createTempFields(0);
            }
            closeField = cParam.m_tempFields[0];
            closeFieldIndex = cParam.m_tempFieldsIndex[0];
            double close = getValue(cParam);
            m_dataSource.set3(m_index, closeFieldIndex, close);
        }
        double[] lowarray = m_dataSource.DATA_ARRAY(closeField, m_index, n);
        return minValue(lowarray, lowarray.length);
    }

    /**
     * 计算指定字段一段区间内的最小值距离今天的天数
     *
     * @param var 变量
     * @returns 天数
     */
    private double LLVBARS(CVariable var) {
        // 获取周期
        int n = (int) getValue(var.m_parameters[1]);
        CVariable cParam = var.m_parameters[0];
        int closeField = cParam.m_field;
        int closeFieldIndex = cParam.m_fieldIndex;
        if (closeField == FCDataTable.NULLFIELD) {
            if (cParam.m_tempFields == null) {
                cParam.createTempFields(0);
            }
            closeField = cParam.m_tempFields[0];
            closeFieldIndex = cParam.m_tempFieldsIndex[0];
            double close = getValue(cParam);
            m_dataSource.set3(m_index, closeFieldIndex, close);
        }
        // 计算天数
        double[] lowarray = m_dataSource.DATA_ARRAY(closeField, m_index, n);
        double result = 0;
        if (lowarray.length > 0) {
            int mIndex = 0;
            double close = 0;
            for (int i = 0; i < lowarray.length; i++) {
                if (i == 0) {
                    close = lowarray[i];
                    mIndex = 0;
                } else {
                    if (lowarray[i] < close) {
                        close = lowarray[i];
                        mIndex = i;
                    }
                }
            }
            result = lowarray.length - mIndex - 1;
        }
        return result;
    }

    /**
     * 计算常用对数
     *
     * @param var 变量
     * @returns 常用对数
     */
    private double LOG(CVariable var) {
        return Math.log10(getValue(var.m_parameters[0]));
    }

    /**
     * 计算简单移动平均
     *
     * @param var 变量
     * @returns 简单移动平均
     */
    private double MA(CVariable var) {
        // 获取计算需要的值
        CVariable cParam = var.m_parameters[0];
        double close = getValue(cParam);
        // 获取计算需要的参数
        int n = (int) getValue(var.m_parameters[1]);
        int closeFieldIndex = cParam.m_fieldIndex;
        if (closeFieldIndex == -1) {
            if (var.m_tempFields == null) {
                var.createTempFields(1);
            }
            closeFieldIndex = var.m_tempFieldsIndex[0];
            m_dataSource.set3(m_index, closeFieldIndex, close);
        }
        // 计算指标
        double result = movingAverage(m_index, n, close, getDatas(closeFieldIndex, var.m_fieldIndex, m_index - 1, n));
        m_dataSource.set3(m_index, var.m_fieldIndex, result);
        return result;
    }

    /**
     * 计算最大值
     *
     * @param var 变量
     * @returns 最大值
     */
    private double MAX(CVariable var) {
        // 获取左侧的值
        double left = getValue(var.m_parameters[0]);
        // 获取右侧的值
        double right = getValue(var.m_parameters[1]);
        if (left >= right) {
            return left;
        } else {
            return right;
        }
    }

    /**
     * 计算指数移动平均
     *
     * @param var 变量
     * @returns 指数移动平均
     */
    private double MEMA(CVariable var) {
        // 获取计算需要的值
        double close = getValue(var.m_parameters[0]);
        // 获取计算需要的参数
        int n = (int) getValue(var.m_parameters[1]);
        double lastMema = 0;
        if (m_index > 0) {
            lastMema = m_dataSource.get3(m_index - 1, var.m_fieldIndex);
        }
        // 计算指标
        double result = simpleMovingAverage(close, lastMema, n, 1);
        m_dataSource.set3(m_index, var.m_fieldIndex, result);
        return result;
    }

    /**
     * 计算最小值
     *
     * @param var 变量
     * @returns 最小百货值
     */
    private double MIN(CVariable var) {
        // 获取左侧的值
        double left = getValue(var.m_parameters[0]);
        // 获取右侧的值
        double right = getValue(var.m_parameters[1]);
        if (left <= right) {
            return left;
        } else {
            return right;
        }
    }

    /**
     * 返回分钟
     *
     * @param var 变量
     * @returns 分钟
     */
    private int MINUTE(CVariable var) {
        return FCStr.convertNumToDate(m_dataSource.getXValue(m_index)).get(Calendar.MINUTE);
    }

    /**
     * 计算模
     *
     * @param var 变量
     * @returns 模
     */
    private double MOD(CVariable var) {
        // 获取左侧的值
        double left = getValue(var.m_parameters[0]);
        // 获取右侧的值
        double right = getValue(var.m_parameters[1]);
        if (right != 0) {
            return left % right;
        }
        return 0;
    }

    /**
     * 返回月份
     *
     * @param var 变量
     * @returns 月份
     */
    private int MONTH(CVariable var) {
        return FCStr.convertNumToDate(m_dataSource.getXValue(m_index)).get(Calendar.MONTH) + 1;
    }

    /**
     * 返回是否持续存在X>Y
     *
     * @param var 变量
     * @returns 是否存在
     */
    private int NDAY(CVariable var) {
        // 获取计算需要的参数
        int n = (int) getValue(var.m_parameters[2]);
        if (n < 0) {
            n = m_dataSource.getRowsCount();
        } else if (n > m_index + 1) {
            n = m_index + 1;
        }
        int tempIndex = m_index;
        int result = 1;
        for (int i = 0; i < n; i++) {
            if (getValue(var.m_parameters[0]) <= getValue(var.m_parameters[1])) {
                result = 0;
                break;
            }
            m_index--;
        }
        m_index = tempIndex;
        return result;
    }

    /**
     * 当值为0时返回1,否则返回0
     *
     * @param var 变量
     * @returns 1或0
     */
    private int NOT(CVariable var) {
        double value = getValue(var.m_parameters[0]);
        if (value == 0) {
            return 1;
        } else {
            return 0;
        }
    }

    /**
     * 添加线条
     *
     * @param var 变量
     * @returns 0
     */
    private double POLYLINE(CVariable var) {
        if (m_div != null) {
            CVariable cond = var.m_parameters[0];
            CVariable price = var.m_parameters[1];
            PolylineShape polylineShape = null;
            if (var.m_polylineShape == null) {
                // 获取颜色和宽度
                String strColor = "COLORAUTO";
                String strLineWidth = "LINETHICK";
                boolean dotLine = false;
                for (int i = 2; i < var.m_parameters.length; i++) {
                    String strParam = var.m_parameters[i].m_expression;
                    if (strParam.startsWith("COLOR")) {
                        strColor = strParam;
                    } else if (strParam.startsWith("LINETHICK")) {
                        strLineWidth = strParam;
                    } else if (strParam.startsWith("DOTLINE")) {
                        dotLine = true;
                    }
                }
                polylineShape = new PolylineShape();
                m_div.addShape(polylineShape);
                // 设置属性
                polylineShape.setAttachVScale(m_attachVScale);
                polylineShape.setColor(getColor(strColor));
                polylineShape.setWidth(getLineWidth(strLineWidth));
                var.createTempFields(1);
                polylineShape.setColorField(var.m_tempFields[0]);
                polylineShape.setFieldText(price.m_fieldText);
                if (dotLine) {
                    polylineShape.setStyle(PolylineStyle.DotLine);
                }
                var.m_polylineShape = polylineShape;
            } else {
                polylineShape = var.m_polylineShape;
            }
            // 添加价格字段
            if (price.m_expression != null && price.m_expression.length() > 0) {
                if (polylineShape.getFieldName() == FCDataTable.NULLFIELD) {
                    if (price.m_field != FCDataTable.NULLFIELD) {
                        polylineShape.setFieldName(price.m_field);
                    } else {
                        price.createTempFields(1);
                        polylineShape.setFieldName(price.m_tempFields[0]);
                    }
                    for (int i = 2; i < var.m_parameters.length; i++) {
                        String strParam = var.m_parameters[i].m_expression;
                        if (strParam.equals("DRAWTITLE")) {
                            if (polylineShape.getFieldText() != null) {
                                m_div.getTitleBar().getTitles().add(new ChartTitle(polylineShape.getFieldName(), polylineShape.getFieldText(), polylineShape.getColor(), 2, true));
                            }
                        }
                    }
                }
                if (price.m_tempFieldsIndex != null) {
                    double value = getValue(price);
                    m_dataSource.set3(m_index, price.m_tempFieldsIndex[0], value);
                }
            }
            // 设置隐藏
            double dCond = 1;
            if (cond.m_expression != null && cond.m_expression.length() > 0 && !cond.m_expression.equals("1")) {
                dCond = getValue(cond);
                if (dCond != 1) {
                    m_dataSource.set3(m_index, var.m_tempFieldsIndex[0], -10000);
                } else {
                    m_dataSource.set3(m_index, var.m_tempFieldsIndex[0], 1);
                }
            }
        }
        return 0;
    }

    /**
     * 计算次幂
     *
     * @param var 变量
     * @returns 次幂
     */
    private double POW(CVariable var) {
        // 获取左侧的值
        double left = getValue(var.m_parameters[0]);
        // 获取右侧的值
        double right = getValue(var.m_parameters[1]);
        return Math.pow(left, right);
    }

    /**
     * 获取随机数
     *
     * @param var 变量
     * @returns 随机数
     */
    private int RAND(CVariable var) {
        // 获取计算需要的参数
        int n = (int) getValue(var.m_parameters[0]);
        return m_random.nextInt(n + 1);
    }

    /**
     * 获取前推周期数值
     *
     * @param var 变量
     * @returns 前推周期数值
     */
    private double REF(CVariable var) {
        // 获取周期
        int param = (int) getValue(var.m_parameters[1]);
        param = m_index - param;
        double result = 0;
        if (param >= 0) {
            int tempIndex = m_index;
            m_index = param;
            result = getValue(var.m_parameters[0]);
            m_index = tempIndex;
        }
        return result;
    }

    /**
     * 返回值
     *
     * @param var 变量
     * @returns 值
     */
    private double RETURN(CVariable var) {
        m_resultVar = null;
        m_result = getValue(var.m_parameters[0]);
        if (m_tempVars.containsKey(var.m_parameters[0].m_field)) {
            m_resultVar = copyTempVar(m_tempVars.get(var.m_parameters[0].m_field));
        } else {
            if (var.m_parameters[0].m_expression.indexOf('\'') == 0) {
                m_resultVar = new CVar();
                m_resultVar.m_type = 1;
                m_resultVar.m_str = var.m_parameters[0].m_expression;
            }
        }
        m_break = 1;
        return m_result;
    }

    /**
     * 取反
     *
     * @param var 变量
     * @returns 反值
     */
    private double REVERSE(CVariable var) {
        return -getValue(var.m_parameters[0]);
    }

    /**
     * 计算四舍五入
     *
     * @param var 变量
     * @returns 四舍五入值
     */
    private double ROUND(CVariable var) {
        return Math.round(getValue(var.m_parameters[0]));
    }

    /**
     * 计算抛物线指标
     *
     * @param var 变量
     * @returns 抛物线指标
     */
    private double SAR(CVariable var) {
        // 获取计算需要的参数
        int n = (int) getValue(var.m_parameters[2]);
        double s = getValue(var.m_parameters[3]) / 100;
        double m = getValue(var.m_parameters[4]) / 100;
        // 获取计算需要的值
        double high = 0, low = 0;
        CVariable hParam = var.m_parameters[0];
        CVariable lParam = var.m_parameters[1];
        high = getValue(hParam);
        low = getValue(lParam);
        // 保存临时变量
        if (var.m_tempFields == null) {
            if (hParam.m_field == FCDataTable.NULLFIELD || lParam.m_field == FCDataTable.NULLFIELD) {
                var.createTempFields(4);
            } else {
                var.createTempFields(2);
            }
        }
        int highField = hParam.m_field;
        int highFieldIndex = hParam.m_fieldIndex;
        if (highField == FCDataTable.NULLFIELD) {
            highField = var.m_tempFields[2];
            highFieldIndex = var.m_tempFieldsIndex[2];
            m_dataSource.set3(m_index, highFieldIndex, high);
        }
        int lowField = lParam.m_field;
        int lowFieldIndex = lParam.m_fieldIndex;
        if (lowField == FCDataTable.NULLFIELD) {
            lowField = var.m_tempFields[3];
            lowFieldIndex = var.m_tempFieldsIndex[3];
            m_dataSource.set3(m_index, lowFieldIndex, low);
        }
        double[] high_list = m_dataSource.DATA_ARRAY(highField, m_index - 1, n);
        double[] low_list = m_dataSource.DATA_ARRAY(lowField, m_index - 1, n);
        double hhv = maxValue(high_list, high_list.length);
        double llv = minValue(low_list, low_list.length);
        int lastState = 0;
        double lastSar = 0;
        double lastAf = 0;
        if (m_index > 0) {
            lastState = (int) m_dataSource.get3(m_index - 1, var.m_tempFieldsIndex[0]);
            lastSar = m_dataSource.get3(m_index - 1, var.m_fieldIndex);
            lastAf = m_dataSource.get3(m_index - 1, var.m_tempFieldsIndex[1]);
        }
        int state = 0;
        double af = 0, sar = 0;
        RefObject<Integer> tempRef_state = new RefObject<Integer>(state);
        RefObject<Double> tempRef_af = new RefObject<Double>(af);
        RefObject<Double> tempRef_sar = new RefObject<Double>(sar);
        // 计算指标
        stopAndReverse(m_index, n, s, m, high, low, hhv, llv, lastState, lastSar, lastAf, tempRef_state, tempRef_af, tempRef_sar);
        state = tempRef_state.argvalue;
        af = tempRef_af.argvalue;
        sar = tempRef_sar.argvalue;
        m_dataSource.set3(m_index, var.m_tempFieldsIndex[1], af);
        m_dataSource.set3(m_index, var.m_tempFieldsIndex[0], state);
        m_dataSource.set3(m_index, var.m_fieldIndex, sar);
        return sar;
    }

    /**
     * 设置变量的值
     *
     * @param var 变量
     * @returns 状态
     */
    private double SET(CVariable var) {
        int pLen = var.m_parameters.length;
        for (int i = 0; i < pLen; i++) {
            if (i % 2 == 0) {
                CVariable variable = var.m_parameters[i];
                CVariable parameter = var.m_parameters[i + 1];
                setVariable(variable, parameter);
            }
        }
        return 0;
    }

    /**
     * 如果大于0则返回1,如果小于0则返回－1，否则返回0
     *
     * @param var 变量
     * @returns 1,0,-1
     */
    private int SIGN(CVariable var) {
        double value = getValue(var.m_parameters[0]);
        if (value > 0) {
            return 1;
        } else if (value < 0) {
            return -1;
        }
        return 0;
    }

    /**
     * 计算正弦值
     *
     * @param var 变量
     * @returns 正弦值
     */
    private double SIN(CVariable var) {
        return Math.sin(getValue(var.m_parameters[0]));
    }

    /**
     * 计算移动平均
     *
     * @param var 变量
     * @returns 移动平均
     */
    private double SMA(CVariable var) {
        // 获取计算需要的值
        double close = getValue(var.m_parameters[0]);
        // 获取计算需要的参数
        int n = (int) getValue(var.m_parameters[1]);
        int m = (int) getValue(var.m_parameters[2]);
        double lastSma = 0;
        if (m_index > 0) {
            lastSma = m_dataSource.get3(m_index - 1, var.m_fieldIndex);
        }
        // 计算指标
        double result = simpleMovingAverage(close, lastSma, n, m);
        m_dataSource.set3(m_index, var.m_fieldIndex, result);
        return result;
    }

    /**
     * 计算平方根
     *
     * @param var 变量
     * @returns 平方根
     */
    private double SQRT(CVariable var) {
        return Math.sqrt(getValue(var.m_parameters[0]));
    }

    /**
     * 计算平方
     *
     * @param var 变量
     * @returns 平方
     */
    private double SQUARE(CVariable var) {
        double result = getValue(var.m_parameters[0]);
        result = result * result;
        return result;
    }

    /**
     * 计算标准差
     *
     * @param var 变量
     * @returns 标准差
     */
    private double STD(CVariable var) {
        // 获取计算需要的参数
        int p = (int) getValue(var.m_parameters[1]);
        CVariable cParam = var.m_parameters[0];
        double close = getValue(cParam);
        int closeField = cParam.m_field;
        int closeFieldIndex = cParam.m_fieldIndex;
        if (closeField == FCDataTable.NULLFIELD) {
            if (var.m_tempFields == null) {
                var.createTempFields(1);
            }
            closeField = var.m_tempFields[0];
            closeFieldIndex = var.m_tempFieldsIndex[0];
            m_dataSource.set3(m_index, closeFieldIndex, close);
        }
        // 获取计算需要的值
        double[] list = m_dataSource.DATA_ARRAY(closeField, m_index, p);
        double avg = 0;
        double sum = 0;
        if (list != null && list.length > 0) {
            for (int i = 0; i < list.length; i++) {
                sum += list[i];
            }
            avg = sum / list.length;
        }
        double result = standardDeviation(list, list.length, avg, 1);
        return result;
    }

    /**
     * 添加柱状图
     *
     * @param var 变量
     * @returns 0
     */
    private double STICKLINE(CVariable var) {
        if (m_div != null) {
            // 获取参数
            CVariable cond = var.m_parameters[0];
            CVariable price1 = var.m_parameters[1];
            CVariable price2 = var.m_parameters[2];
            CVariable width = var.m_parameters[3];
            CVariable empty = var.m_parameters[4];
            BarShape barShape = null;
            if (var.m_barShape == null) {
                barShape = new BarShape();
                m_div.addShape(barShape);
                barShape.setAttachVScale(m_attachVScale);
                barShape.setFieldText(price1.m_fieldText);
                barShape.setFieldText2(price2.m_fieldText);
                CVariable color = null;
                for (int i = 5; i < var.m_parameters.length; i++) {
                    String strParam = var.m_parameters[i].m_expression;
                    if (strParam.startsWith("COLOR")) {
                        color = var.m_parameters[i];
                        break;
                    }
                }
                if (color != null) {
                    barShape.setUpColor(getColor(color.m_expression));
                    barShape.setDownColor(getColor(color.m_expression));
                } else {
                    barShape.setUpColor(FCColor.argb(255, 82, 82));
                    barShape.setDownColor(FCColor.argb(82, 255, 255));
                }
                barShape.setStyle(BarStyle.Line);
                var.createTempFields(1);
                barShape.setStyleField(var.m_tempFields[0]);
                barShape.setLineWidth((int) Math.round(FCStr.convertStrToDouble(width.m_expression)));
                var.m_barShape = barShape;
            } else {
                barShape = var.m_barShape;
            }
            // 添加价格一字段
            if (price1.m_expression != null && price1.m_expression.length() > 0) {
                if (barShape.getFieldName() == FCDataTable.NULLFIELD) {
                    if (price1.m_field != FCDataTable.NULLFIELD) {
                        barShape.setFieldName(price1.m_field);
                    } else {
                        price1.createTempFields(1);
                        barShape.setFieldName(price1.m_tempFields[0]);
                    }
                    for (int i = 5; i < var.m_parameters.length; i++) {
                        String strParam = var.m_parameters[i].m_expression;
                        if (strParam.equals("DRAWTITLE")) {
                            if (barShape.getFieldText() != null) {
                                m_div.getTitleBar().getTitles().add(new ChartTitle(barShape.getFieldName(), barShape.getFieldText(), barShape.getDownColor(), 2, true));
                            }
                            break;
                        }
                    }
                }
                if (price1.m_tempFieldsIndex != null) {
                    double value = getValue(price1);
                    m_dataSource.set3(m_index, price1.m_tempFieldsIndex[0], value);
                }
            }
            // 添加价格二字段
            if (price2.m_expression != null && price2.m_expression.length() > 0 && !price2.m_expression.equals("0")) {
                if (price2.m_field != FCDataTable.NULLFIELD) {
                    barShape.setFieldName2(price2.m_field);
                } else {
                    price2.createTempFields(1);
                    barShape.setFieldName2(price2.m_tempFields[0]);
                }
                if (price2.m_tempFieldsIndex != null) {
                    double value = getValue(price2);
                    m_dataSource.set3(m_index, price2.m_tempFieldsIndex[0], value);
                }
            }
            // 设置隐藏
            double dCond = 1;
            if (cond.m_expression != null && cond.m_expression.length() > 0 && !cond.m_expression.equals("1")) {
                dCond = getValue(cond);
                if (dCond != 1) {
                    m_dataSource.set3(m_index, var.m_tempFieldsIndex[0], -10000);
                } else {
                    int dEmpty = 2;
                    if (empty.m_expression != null && empty.m_expression.length() > 0) {
                        dEmpty = (int) getValue(empty);
                        m_dataSource.set3(m_index, var.m_tempFieldsIndex[0], dEmpty);
                    }
                }
            }
        }
        return 0;
    }

    /**
     * 计算求和
     *
     * @param var 变量
     * @returns 和
     */
    private double SUM(CVariable var) {
        // 获取计算需要的值
        double close = getValue(var.m_parameters[0]);
        int closeFieldIndex = var.m_parameters[0].m_fieldIndex;
        if (closeFieldIndex == -1) {
            if (var.m_tempFields == null) {
                var.createTempFields(1);
            }
            closeFieldIndex = var.m_tempFieldsIndex[0];
            m_dataSource.set3(m_index, closeFieldIndex, close);
        }
        // 获取计算需要的参数
        int n = (int) getValue(var.m_parameters[1]);
        if (n == 0) {
            n = m_index + 1;
        }
        // 计算指标
        double result = sumilationValue(m_index, n, close, getDatas(closeFieldIndex, var.m_fieldIndex, m_index - 1, n));
        m_dataSource.set3(m_index, var.m_fieldIndex, result);
        return result;
    }

    /**
     * 计算正切值
     *
     * @param var 变量
     * @returns 正切值
     */
    private double TAN(CVariable var) {
        return Math.tan(getValue(var.m_parameters[0]));
    }

    /**
     * 取得该周期的时分,适用于日线以下周期
     *
     * @param var 变量
     * @returns 时分
     */
    private double TIME(CVariable var) {
        int year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0, ms = 0;
        RefObject<Integer> tempRef_year = new RefObject<Integer>(year);
        RefObject<Integer> tempRef_month = new RefObject<Integer>(month);
        RefObject<Integer> tempRef_day = new RefObject<Integer>(day);
        RefObject<Integer> tempRef_hour = new RefObject<Integer>(hour);
        RefObject<Integer> tempRef_minute = new RefObject<Integer>(minute);
        RefObject<Integer> tempRef_second = new RefObject<Integer>(second);
        RefObject<Integer> tempRef_ms = new RefObject<Integer>(ms);
        FCStr.getDataByNum(m_dataSource.getXValue(m_index), tempRef_year, tempRef_month, tempRef_day, tempRef_hour, tempRef_minute, tempRef_second, tempRef_ms);
        year = tempRef_year.argvalue;
        month = tempRef_month.argvalue;
        day = tempRef_day.argvalue;
        hour = tempRef_hour.argvalue;
        minute = tempRef_minute.argvalue;
        second = tempRef_second.argvalue;
        ms = tempRef_ms.argvalue;
        return hour * 100 + minute;
    }

    /**
     * 取得该周期的时分,适用于日线以下周期
     *
     * @param var 变量
     * @returns 时分秒
     */
    private double TIME2(CVariable var) {
        int year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0, ms = 0;
        RefObject<Integer> tempRef_year = new RefObject<Integer>(year);
        RefObject<Integer> tempRef_month = new RefObject<Integer>(month);
        RefObject<Integer> tempRef_day = new RefObject<Integer>(day);
        RefObject<Integer> tempRef_hour = new RefObject<Integer>(hour);
        RefObject<Integer> tempRef_minute = new RefObject<Integer>(minute);
        RefObject<Integer> tempRef_second = new RefObject<Integer>(second);
        RefObject<Integer> tempRef_ms = new RefObject<Integer>(ms);
        FCStr.getDataByNum(m_dataSource.getXValue(m_index), tempRef_year, tempRef_month, tempRef_day, tempRef_hour, tempRef_minute, tempRef_second, tempRef_ms);
        year = tempRef_year.argvalue;
        month = tempRef_month.argvalue;
        day = tempRef_day.argvalue;
        hour = tempRef_hour.argvalue;
        minute = tempRef_minute.argvalue;
        second = tempRef_second.argvalue;
        ms = tempRef_ms.argvalue;
        return hour * 10000 + minute * 100 + second;
    }

    /**
     * 计算递归移动平均
     *
     * @param var 变量
     * @returns 递归移动平均
     */
    private double TMA(CVariable var) {
        // 获取计算需要的值
        double close = getValue(var.m_parameters[0]);
        int n = (int) getValue(var.m_parameters[1]);
        int m = (int) getValue(var.m_parameters[2]);
        // 计算指标
        double lastTma = 0;
        if (m_index > 0) {
            lastTma = m_dataSource.get3(m_index - 1, var.m_fieldIndex);
        }
        double result = n * lastTma + m * close;
        m_dataSource.set3(m_index, var.m_fieldIndex, result);
        return result;
    }

    /**
     * 返回是否连涨周期数
     *
     * @param var 变量
     * @returns 是否连涨周期数
     */
    private int UPNDAY(CVariable var) {
        // 获取计算需要的参数
        int n = (int) getValue(var.m_parameters[0]);
        if (n < 0) {
            n = m_dataSource.getRowsCount();
        } else if (n > m_index + 1) {
            n = m_index + 1;
        }
        int tempIndex = m_index;
        int result = 1;
        for (int i = 0; i < n; i++) {
            double right = getValue(var.m_parameters[0]);
            m_index--;
            double left = m_index >= 0 ? getValue(var.m_parameters[0]) : 0;
            if (right <= left) {
                result = 0;
                break;
            }
        }
        m_index = tempIndex;
        return result;
    }

    /**
     * 当条件成立时,取当前值,否则取上个值
     *
     * @param var 变量
     * @returns 数值
     */
    private double VALUEWHEN(CVariable var) {
        int n = m_dataSource.getRowsCount();
        int tempIndex = m_index;
        double result = 0;
        for (int i = 0; i < n; i++) {
            double value = getValue(var.m_parameters[0]);
            if (value == 1) {
                result = getValue(var.m_parameters[1]);
                break;
            }
            m_index--;
        }
        m_index = tempIndex;
        return result;
    }

    /**
     * 定义变量
     *
     * @param var 变量
     * @returns 数值
     */
    private double VAR(CVariable var) {
        double result = 0;
        int pLen = var.m_parameters.length;
        for (int i = 0; i < pLen; i++) {
            if (i % 2 == 0) {
                CVariable name = var.m_parameters[i];
                CVariable value = var.m_parameters[i + 1];
                int id = name.m_field;
                CVar newCVar = m_varFactory.createVar();
                result = newCVar.onCreate(this, name, value);
                if (newCVar.m_type == 1) {
                    name.m_functionID = -2;
                }
                if (m_tempVars.containsKey(id)) {
                    CVar cVar = m_tempVars.get(id);
                    newCVar.m_parent = cVar;
                }
                m_tempVars.put(id, newCVar);
            }
        }
        return result;
    }

    /**
     * 执行WHILE循环
     *
     * @param var 变量
     * @returns 状态
     */
    private int WHILE(CVariable var) {
        int pLen = var.m_parameters.length;
        if (pLen > 1) {
            while (true) {
                if (getValue(var.m_parameters[0]) <= 0) {
                    break;
                }
                for (int i = 1; m_break == 0 && i < pLen; i++) {
                    getValue(var.m_parameters[i]);
                }
                if (m_break > 0) {
                    if (m_break == 3) {
                        m_break = 0;
                        deleteTempVars(var);
                        continue;
                    } else {
                        m_break = 0;
                        deleteTempVars(var);
                        break;
                    }
                } else {
                    deleteTempVars(var);
                }
            }
        }
        return 0;
    }

    /**
     * 计算加权移动平均线
     *
     * @param var 变量
     * @returns 加权移动平均
     */
    private double WMA(CVariable var) {
        // 获取计算需要的值
        double close = getValue(var.m_parameters[0]);
        // 获取计算需要的参数
        int n = (int) getValue(var.m_parameters[1]);
        int m = (int) getValue(var.m_parameters[2]);
        // 计算指标
        double lastWma = 0;
        if (m_index > 0) {
            lastWma = m_dataSource.get3(m_index - 1, var.m_fieldIndex);
        }
        double result = weightMovingAverage(n, m, close, lastWma);
        m_dataSource.set3(m_index, var.m_fieldIndex, result);
        return result;
    }

    /**
     * 计算年份
     *
     * @param var 变量
     * @returns 年份
     */
    private int YEAR(CVariable var) {
        return FCStr.convertNumToDate(m_dataSource.getXValue(m_index)).get(Calendar.YEAR);
    }

    /**
     * 计算之字反转
     *
     * @param var 变量
     * @returns 之字反转
     */
    private double ZIG(CVariable var) {
        double sxp = 0, exp = 0;
        int state = 0, sxi = 0, exi = 0;
        // 获取计算需要的参数
        double p = getValue(var.m_parameters[1]);
        // 获取计算需要的值
        CVariable cParam = var.m_parameters[0];
        double close = getValue(cParam);
        int closeFieldIndex = cParam.m_fieldIndex;
        // 创建字段
        if (var.m_tempFields == null) {
            if (closeFieldIndex == -1) {
                var.createTempFields(6);
            } else {
                var.createTempFields(5);
            }
        }
        if (closeFieldIndex == -1) {
            closeFieldIndex = var.m_tempFieldsIndex[5];
            m_dataSource.set3(m_index, closeFieldIndex, close);
        }
        // 获取上一记录的值
        if (m_index > 0) {
            state = (int) m_dataSource.get3(m_index - 1, var.m_tempFieldsIndex[0]);
            exp = m_dataSource.get3(m_index - 1, var.m_tempFieldsIndex[1]);
            sxp = m_dataSource.get3(m_index - 1, var.m_tempFieldsIndex[2]);
            sxi = (int) m_dataSource.get3(m_index - 1, var.m_tempFieldsIndex[3]);
            exi = (int) m_dataSource.get3(m_index - 1, var.m_tempFieldsIndex[4]);
        }
        int cStart = -1, cEnd = -1;
        double k = 0, b = 0;
        RefObject<Double> tempRef_sxp = new RefObject<Double>(sxp);
        RefObject<Integer> tempRef_sxi = new RefObject<Integer>(sxi);
        RefObject<Double> tempRef_exp = new RefObject<Double>(exp);
        RefObject<Integer> tempRef_exi = new RefObject<Integer>(exi);
        RefObject<Integer> tempRef_state = new RefObject<Integer>(state);
        RefObject<Integer> tempRef_cStart = new RefObject<Integer>(cStart);
        RefObject<Integer> tempRef_cEnd = new RefObject<Integer>(cEnd);
        RefObject<Double> tempRef_k = new RefObject<Double>(k);
        RefObject<Double> tempRef_b = new RefObject<Double>(b);
        zigzag(m_index, close, p, tempRef_sxp, tempRef_sxi, tempRef_exp, tempRef_exi, tempRef_state, tempRef_cStart, tempRef_cEnd, tempRef_k, tempRef_b);
        sxp = tempRef_sxp.argvalue;
        sxi = tempRef_sxi.argvalue;
        exp = tempRef_exp.argvalue;
        exi = tempRef_exi.argvalue;
        state = tempRef_state.argvalue;
        cStart = tempRef_cStart.argvalue;
        cEnd = tempRef_cEnd.argvalue;
        k = tempRef_k.argvalue;
        b = tempRef_b.argvalue;
        m_dataSource.set3(m_index, var.m_tempFieldsIndex[0], state);
        m_dataSource.set3(m_index, var.m_tempFieldsIndex[1], exp);
        m_dataSource.set3(m_index, var.m_tempFieldsIndex[2], sxp);
        m_dataSource.set3(m_index, var.m_tempFieldsIndex[3], sxi);
        m_dataSource.set3(m_index, var.m_tempFieldsIndex[4], exi);
        if (cStart != -1 && cEnd != -1) {
            return 1;
        } else {
            return 0;
        }
    }

    /**
     * 连接字符串
     *
     * @param var 变量
     * @returns 结果
     */
    private int STR_CONTACT(CVariable var) {
        int pLen = var.m_parameters.length;
        String text = "'";
        for (int i = 0; i < pLen; i++) {
            text += getText(var.m_parameters[i]);
        }
        text += "'";
        m_resultVar = new CVar();
        m_resultVar.m_type = 1;
        m_resultVar.m_str = text;
        return 0;
    }

    /**
     * 查找字符串中出现文字的位置
     *
     * @param var 变量
     * @returns 位置
     */
    private int STR_FIND(CVariable var) {
        return getText(var.m_parameters[0]).indexOf(getText(var.m_parameters[1]));
    }

    /**
     * 比较字符串是否相等
     *
     * @param var 变量
     * @returns 位置
     */
    private int STR_EQUALS(CVariable var) {
        int result = 0;
        if (getText(var.m_parameters[0]).equals(getText(var.m_parameters[1]))) {
            result = 1;
        }
        return result;
    }

    /**
     * 查找字符串中最后出现文字的位置
     *
     * @param var 变量
     * @returns 位置
     */
    private int STR_FINDlAST(CVariable var) {
        return getText(var.m_parameters[0]).lastIndexOf(getText(var.m_parameters[1]));
    }

    /**
     * 获取字符串的长度
     *
     * @param var 变量
     * @returns 长度
     */
    private int STR_LENGTH(CVariable var) {
        return getText(var.m_parameters[0]).length();
    }

    /**
     * 截取字符串
     *
     * @param var 变量
     * @returns 结果
     */
    private int STR_SUBSTR(CVariable var) {
        int pLen = var.m_parameters.length;
        if (pLen == 2) {
            m_resultVar = new CVar();
            m_resultVar.m_type = 1;
            m_resultVar.m_str = "'" + getText(var.m_parameters[0]).substring((int) getValue(var.m_parameters[1])) + "'";
        } else if (pLen >= 3) {
            m_resultVar = new CVar();
            m_resultVar.m_type = 1;
            m_resultVar.m_str = "'" + getText(var.m_parameters[0]).substring((int) getValue(var.m_parameters[1]), (int) getValue(var.m_parameters[1]) + (int) getValue(var.m_parameters[2])) + "'";
        }
        return 0;
    }

    /**
     * 替换字符串
     *
     * @param var 变量
     * @returns 结果
     */
    private int STR_REPLACE(CVariable var) {
        m_resultVar = new CVar();
        m_resultVar.m_type = 1;
        m_resultVar.m_str = "'" + getText(var.m_parameters[0]).replace(getText(var.m_parameters[1]), getText(var.m_parameters[2])) + "'";
        return 0;
    }

    /**
     * 切割字符串
     *
     * @param var 变量
     * @returns 结果
     */
    private int STR_SPLIT(CVariable var) {
        CVariable pName = var.m_parameters[0];
        int id = pName.m_field;
        if (m_tempVars.containsKey(id)) {
            ArrayList<String> list = m_tempVars.get(id).m_list;
            list.clear();
            String[] strs = getText(var.m_parameters[1]).split("[" + getText(var.m_parameters[2]) + "]");
            int strsSize = strs.length;
            for (int i = 0; i < strsSize; i++) {
                if (strs[i].length() > 0) {
                    list.add(strs[i]);
                }
            }
            return 1;
        }
        return 0;
    }

    /**
     * 字符串转化为小写
     *
     * @param var 变量
     * @returns 结果
     */
    private int STR_TOLOWER(CVariable var) {
        m_resultVar = new CVar();
        m_resultVar.m_type = 1;
        m_resultVar.m_str = getText(var.m_parameters[0]).toLowerCase();
        return 0;
    }

    /**
     * 字符串转化为大写
     *
     * @param var 变量
     * @returns 结果
     */
    private int STR_TOUPPER(CVariable var) {
        m_resultVar = new CVar();
        m_resultVar.m_type = 1;
        m_resultVar.m_str = getText(var.m_parameters[0]).toUpperCase();
        return 0;
    }

    /**
     * 添加数据到集合
     *
     * @param var 变量
     * @returns 结果
     */
    private int LIST_ADD(CVariable var) {
        CVariable pName = var.m_parameters[0];
        int listName = pName.m_field;
        if (m_tempVars.containsKey(listName)) {
            ArrayList<String> list = m_tempVars.get(listName).m_list;
            int pLen = var.m_parameters.length;
            for (int i = 1; i < pLen; i++) {
                list.add(getText(var.m_parameters[i]));
            }
            return 1;
        }
        return 0;
    }

    /**
     * 清除集合
     *
     * @param var 变量
     * @returns 结果
     */
    private int LIST_CLEAR(CVariable var) {
        CVariable pName = var.m_parameters[0];
        int listName = pName.m_field;
        if (m_tempVars.containsKey(listName)) {
            m_tempVars.get(listName).m_list.clear();
            return 1;
        }
        return 0;
    }

    /**
     * 从集合中获取数据
     *
     * @param var 变量
     * @returns 结果
     */
    private int LIST_GET(CVariable var) {
        CVariable pName = var.m_parameters[1];
        int listName = pName.m_field;
        if (m_tempVars.containsKey(listName)) {
            ArrayList<String> list = m_tempVars.get(listName).m_list;
            int index = (int) getValue(var.m_parameters[2]);
            if (index < list.size()) {
                String strValue = list.get(index);
                CVariable variable = var.m_parameters[0];
                int id = variable.m_field;
                int type = variable.m_type;
                switch (type) {
                    case 2:
                        double value = FCStr.convertStrToDouble(strValue);
                        m_dataSource.set3(m_index, variable.m_fieldIndex, value);
                        break;
                    default:
                        if (m_tempVars.containsKey(id)) {
                            CVar otherCVar = m_tempVars.get(id);
                            CVariable newVar = new CVariable(this);
                            newVar.m_type = 1;
                            newVar.m_expression = "'" + strValue + "'";
                            otherCVar.setValue(this, variable, newVar);
                        }
                        break;
                }
            }
            return 1;
        }
        return 0;
    }

    /**
     * 向集合中插入数据
     *
     * @param var 变量
     * @returns 结果
     */
    private int LIST_INSERT(CVariable var) {
        CVariable pName = var.m_parameters[0];
        int listName = pName.m_field;
        if (m_tempVars.containsKey(listName)) {
            m_tempVars.get(listName).m_list.add((int) getValue(var.m_parameters[1]), getText(var.m_parameters[2]));
            return 1;
        }
        return 0;
    }

    /**
     * 从集合中移除数据
     *
     * @param var 变量
     * @returns 结果
     */
    private int LIST_REMOVE(CVariable var) {
        CVariable pName = var.m_parameters[0];
        int listName = pName.m_field;
        if (m_tempVars.containsKey(listName)) {
            m_tempVars.get(listName).m_list.remove((int) getValue(var.m_parameters[1]));
            return 1;
        }
        return 0;
    }

    /**
     * 获取集合的大小
     *
     * @param var 变量
     * @returns 结果
     */
    private int LIST_SIZE(CVariable var) {
        int size = 0;
        CVariable pName = var.m_parameters[0];
        int listName = pName.m_field;
        if (m_tempVars.containsKey(listName)) {
            size = m_tempVars.get(listName).m_list.size();
        }
        return size;
    }

    /**
     * 清除哈希表
     *
     * @param var 变量
     * @returns 结果
     */
    private int MAP_CLEAR(CVariable var) {
        CVariable pName = var.m_parameters[0];
        int mapName = pName.m_field;
        if (m_tempVars.containsKey(mapName)) {
            m_tempVars.get(mapName).m_map.clear();
            return 1;
        }
        return 0;
    }

    /**
     * 哈希表是否包含键
     *
     * @param var 变量
     * @returns 结果
     */
    private int MAP_CONTAINSKEY(CVariable var) {
        int result = 0;
        CVariable pName = var.m_parameters[0];
        int mapName = pName.m_field;
        if (m_tempVars.containsKey(mapName)) {
            if (m_tempVars.get(mapName).m_map.containsKey(getText(var.m_parameters[1]))) {
                result = 1;
            }
        }
        return result;
    }

    /**
     * 获取哈希表的值
     *
     * @param var 变量
     * @returns 结果
     */
    private int MAP_GET(CVariable var) {
        CVariable pName = var.m_parameters[1];
        int mapName = pName.m_field;
        if (m_tempVars.containsKey(mapName)) {
            HashMap<String, String> map = m_tempVars.get(mapName).m_map;
            String key = getText(var.m_parameters[2]);
            if (map.containsKey(key)) {
                String strValue = map.get(key);
                CVariable variable = var.m_parameters[0];
                int id = variable.m_field;
                int type = variable.m_type;
                switch (type) {
                    case 2:
                        double value = FCStr.convertStrToDouble(strValue);
                        m_dataSource.set3(m_index, variable.m_fieldIndex, value);
                        break;
                    default:
                        if (m_tempVars.containsKey(id)) {
                            CVar otherCVar = m_tempVars.get(id);
                            CVariable newVar = new CVariable(this);
                            newVar.m_type = 1;
                            newVar.m_expression = "'" + strValue + "'";
                            otherCVar.setValue(this, variable, newVar);
                        }
                        break;
                }
            }
            return 1;
        }
        return 0;
    }

    /**
     * 获取哈希表的键
     *
     * @param var 变量
     * @returns 结果
     */
    private int MAP_GETKEYS(CVariable var) {
        CVariable pName = var.m_parameters[1];
        int mapName = pName.m_field;
        if (m_tempVars.containsKey(mapName)) {
            int listName = var.m_parameters[0].m_field;
            if (m_tempVars.containsKey(listName)) {
                HashMap<String, String> map = m_tempVars.get(mapName).m_map;
                ArrayList<String> list = m_tempVars.get(listName).m_list;
                list.clear();
                for (Map.Entry<String, String> entry : map.entrySet()) {
                    list.add(entry.getKey());
                }
                return 1;
            }
        }
        return 0;
    }

    /**
     * 从哈希表中移除
     *
     * @param var 变量
     * @returns 结果
     */
    private int MAP_REMOVE(CVariable var) {
        CVariable pName = var.m_parameters[0];
        int mapName = pName.m_field;
        if (m_tempVars.containsKey(mapName)) {
            m_tempVars.get(mapName).m_map.remove(getText(var.m_parameters[1]));
            return 1;
        }
        return 0;
    }

    /**
     * 向哈希表中添加数据
     *
     * @param var 变量
     * @returns 结果
     */
    private int MAP_SET(CVariable var) {
        CVariable pName = var.m_parameters[0];
        int mapName = pName.m_field;
        if (m_tempVars.containsKey(mapName)) {
            m_tempVars.get(mapName).m_map.put(getText(var.m_parameters[1]), getText(var.m_parameters[2]));
        }
        return 0;
    }

    /**
     * 获取哈希表的尺寸
     *
     * @param var 变量
     * @returns 结果
     */
    private int MAP_SIZE(CVariable var) {
        int size = 0;
        CVariable pName = var.m_parameters[0];
        int mapName = pName.m_field;
        if (m_tempVars.containsKey(mapName)) {
            size = m_tempVars.get(mapName).m_map.size();
        }
        return size;
    }

    /*
     * Avedev
     */
    public static double avedev(double value, double[] listForAvedev, int listForAvedev_length, double avg) {
        int i = 0;
        if (listForAvedev_length > 0) {
            double sum = Math.abs(value - avg);
            for (i = 0; i < listForAvedev_length; i++) {
                sum += Math.abs(listForAvedev[i] - avg);
            }
            return sum / listForAvedev_length;
        } else {
            return 0;
        }
    }

    /*
     * 平均值
     */
    public static double avgValue(double[] list, int length) {
        int i = 0;
        double sum = 0;
        if (length > 0) {
            for (i = 0; i < length; i++) {
                sum += list[i];
            }
            return sum / length;
        }
        return 0;
    }

    /*
     * 指数移动平滑平均
     */
    public static double exponentialMovingAverage(int n, double value, double lastEMA) {
        return (value * 2 + lastEMA * (n - 1)) / (n + 1);
    }

    /*
     * 斐波那契
     */
    public static int fibonacciValue(int index) {
        if (index < 1) {
            return 0;
        } else {
            int[] vList;
            int i = 0, result = 0;
            vList = new int[index];
            for (i = 0; i <= index - 1; i++) {
                if (i == 0 || i == 1) {
                    vList[i] = 1;
                } else {
                    vList[i] = vList[i - 1] + vList[i - 2];
                }
            }
            result = vList[index - 1];
            return result;
        }
    }

    /*
     * 线性回归
     */
    public static void linearRegressionEquation(double[] list, int length, RefObject<Float> k, RefObject<Float> b) {
        int i = 0;
        double sumX = 0;
        double sumY = 0;
        double sumUp = 0;
        double sumDown = 0;
        double xAvg = 0;
        double yAvg = 0;
        k.argvalue = 0.0f;
        b.argvalue = 0.0f;
        if (length > 1) {
            for (i = 0; i < length; i++) {
                sumX += i + 1;
                sumY += list[i];
            }
            xAvg = sumX / length;
            yAvg = sumY / length;
            for (i = 0; i < length; i++) {
                sumUp += (i + 1 - xAvg) * (list[i] - yAvg);
                sumDown += (i + 1 - xAvg) * (i + 1 - xAvg);
            }
            k.argvalue = (float) (sumUp / sumDown);
            b.argvalue = (float) (yAvg - k.argvalue * xAvg);
        }
    }

    /*
     * 最大值
     */
    public static double maxValue(double[] list, int length) {
        double max = 0;
        int i = 0;
        for (i = 0; i < length; i++) {
            if (i == 0) {
                max = list[i];
            } else {
                if (max < list[i]) {
                    max = list[i];
                }
            }
        }
        return max;
    }

    /*
     * 最小值
     */
    public static double minValue(double[] list, int length) {
        double min = 0;
        int i = 0;
        for (i = 0; i < length; i++) {
            if (i == 0) {
                min = list[i];
            } else {
                if (min > list[i]) {
                    min = list[i];
                }
            }
        }
        return min;
    }

    /*
     * 平均值
     */
    public static double movingAverage(int index, int n, double value, LPDATA last_MA) {
        double sum = 0;
        if (last_MA.mode == 0) {
            sum = last_MA.sum + value;
        } else {
            if (index > n - 1) {
                sum = last_MA.lastvalue * n;
                sum -= last_MA.first_value;
            } else {
                sum = last_MA.lastvalue * index;
                n = index + 1;
            }
            sum += value;
        }
        return sum / n;
    }

    /*
     * 标准差
     */
    public static double standardDeviation(double[] list, int length, double avg, double standardDeviation) {
        int i = 0;
        if (length > 0) {
            double sum = 0;
            for (i = 0; i < length; i++) {
                sum += (list[i] - avg) * (list[i] - avg);
            }
            return standardDeviation * Math.sqrt(sum / length);
        } else {
            return 0;
        }
    }

    /*
     * 简单移动平均
     */
    public static double simpleMovingAverage(double close, double lastSma, int n, int m) {
        return (close * m + lastSma * (n - m)) / n;
    }

    /*
     * 求和
     */
    public static double sumilationValue(int index, int n, double value, LPDATA last_SUM) {
        double sum = 0;
        if (last_SUM.mode == 0) {
            sum = last_SUM.sum + value;
        } else {
            sum = last_SUM.lastvalue;
            if (index > n - 1) {
                sum -= last_SUM.first_value;
            }
            sum += value;
        }
        return sum;
    }

    /*
     * 抛物线反转
     */
    public static void stopAndReverse(int index, int n, double s, double m, double high, double low, double hhv, double llv, int last_state, double last_sar, double last_af, RefObject<Integer> state, RefObject<Double> af, RefObject<Double> sar) {
        if (index >= n) {
            if (index == n) {
                af.argvalue = s;
                if (llv < low) {
                    sar.argvalue = llv;
                    state.argvalue = 1;
                }
                if (hhv > high) {
                    sar.argvalue = hhv;
                    state.argvalue = 2;
                }
            } else {
                state.argvalue = last_state;
                af.argvalue = last_af;
                if (state.argvalue == 1) {
                    if (high > hhv) {
                        af.argvalue += s;
                        if (af.argvalue > m) {
                            af.argvalue = m;
                        }
                    }
                    sar.argvalue = last_sar + af.argvalue * (hhv - last_sar);
                    if (sar.argvalue < low) {
                        state.argvalue = 1;
                    } else {
                        state.argvalue = 3;
                    }
                } else if (state.argvalue == 2) {
                    if (low < llv) {
                        af.argvalue += s;
                        if (af.argvalue > m) {
                            af.argvalue = m;
                        }
                    }
                    sar.argvalue = last_sar + af.argvalue * (llv - last_sar);
                    if (sar.argvalue > high) {
                        state.argvalue = 2;
                    } else {
                        state.argvalue = 4;
                    }
                } else if (state.argvalue == 3) {
                    sar.argvalue = hhv;
                    if (sar.argvalue > high) {
                        state.argvalue = 2;
                    } else {
                        state.argvalue = 4;
                    }
                    af.argvalue = s;
                } else if (state.argvalue == 4) {
                    sar.argvalue = llv;
                    if (sar.argvalue < low) {
                        state.argvalue = 1;
                    } else {
                        state.argvalue = 3;
                    }
                    af.argvalue = s;
                }
            }
        }
    }

    /*
     * 求和
     */
    public static double sumValue(double[] list, int length) {
        double sum = 0;
        int i = 0;
        for (i = 0; i < length; i++) {
            sum += list[i];
        }
        return sum;
    }

    /*
     * 加权移动平均
     */
    public static double weightMovingAverage(int n, int weight, double value, double lastWMA) {
        return (value * weight + (n - weight) * lastWMA) / n;
    }

    /*
     * Zigzag
     */
    public static void zigzag(int index, double close, double p, RefObject<Double> sxp, RefObject<Integer> sxi, RefObject<Double> exp, RefObject<Integer> exi, RefObject<Integer> state, RefObject<Integer> cStart, RefObject<Integer> cEnd, RefObject<Double> k, RefObject<Double> b) {
        boolean reverse = false;
        boolean ex = false;
        if (index == 0) {
            sxp.argvalue = close;
            exp.argvalue = close;
        } else if (index == 1) {
            if (close >= exp.argvalue) {
                state.argvalue = 0;
            } else {
                state.argvalue = 1;
            }
            exp.argvalue = close;
            exi.argvalue = 1;
        } else {
            if (state.argvalue == 0) {
                if (100 * (exp.argvalue - close) / (exp.argvalue) > p) {
                    reverse = true;
                } else if (close >= exp.argvalue) {
                    ex = true;
                }
            } else {
                if (100 * (close - exp.argvalue) / (exp.argvalue) > p) {
                    reverse = true;
                } else if (close <= exp.argvalue) {
                    ex = true;
                }
            }
            if (reverse == true) {
                if (state.argvalue == 1) {
                    state.argvalue = 0;
                } else {
                    state.argvalue = 1;
                }
                k.argvalue = (exp.argvalue - sxp.argvalue) / (exi.argvalue - sxi.argvalue);
                b.argvalue = exp.argvalue - (k.argvalue) * (exi.argvalue);
                cStart.argvalue = sxi.argvalue;
                cEnd.argvalue = exi.argvalue;
                sxi.argvalue = exi.argvalue;
                sxp.argvalue = exp.argvalue;
                exi.argvalue = index;
                exp.argvalue = close;
            } else if (ex == true) {
                exp.argvalue = close;
                exi.argvalue = index;
                k.argvalue = (exp.argvalue - sxp.argvalue) / (exi.argvalue - sxi.argvalue);
                b.argvalue = exp.argvalue - (k.argvalue) * (exi.argvalue);
                cStart.argvalue = sxi.argvalue;
                cEnd.argvalue = exi.argvalue;
            } else {
                k.argvalue = (close - exp.argvalue) / (index - exi.argvalue);
                b.argvalue = close - (k.argvalue) * index;
                cStart.argvalue = exi.argvalue;
                cEnd.argvalue = index;
            }
        }
    }
}
