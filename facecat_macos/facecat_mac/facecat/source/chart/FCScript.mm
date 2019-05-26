#include "stdafx.h"
#include "FCScript.h"
#include "FCStr.h"
namespace FaceCat{
    CVariable::CVariable(){
        m_barShape = 0;
        m_candleShape = 0;
        m_field = FCDataTable::NULLFIELD();
        m_fieldIndex = -1;
        m_functionID = -1;
        m_indicator = 0;
        m_line = -1;
        m_pointShape = 0;
        m_polylineShape = 0;
        m_parameters = 0;
        m_parametersLength = 0;
        m_splitExpression = 0;
        m_splitExpressionLength = 0;
        m_tempFields = 0;
        m_tempFieldsLength = 0;
        m_tempFieldsIndex = 0;
        m_tempFieldsIndexLength = 0;
        m_textShape = 0;
        m_type = 0;
    }
    
    CVariable::~CVariable(){
        m_barShape = 0;
        m_candleShape = 0;
        m_indicator = 0;
        m_pointShape = 0;
        m_polylineShape = 0;
        m_textShape = 0;
        if(m_parameters && m_parametersLength > 0){
            delete[] m_parameters;
            m_parameters = 0;
        }
        if(m_splitExpression && m_splitExpressionLength > 0){
            for(int i = 0; i < m_splitExpressionLength; i++){
                CMathElement *math = m_splitExpression[i];
                delete math;
            }
            delete[] m_splitExpression;
            m_splitExpression = 0;
        }
        if(m_tempFields && m_tempFieldsLength > 0){
            delete[] m_tempFields;
            m_tempFields = 0;
        }
        if(m_tempFieldsIndex && m_tempFieldsIndexLength > 0){
            delete[] m_tempFieldsIndex;
            m_tempFieldsIndex = 0;
        }
    }
    
    void CVariable::createTempFields(int count){
        m_tempFields = new int[count];
        m_tempFieldsLength = count;
        m_tempFieldsIndex = new int[count];
        m_tempFieldsIndexLength = count;
        for (int i = 0; i < count; i++){
            int field = m_indicator->getDataSource()->AUTOFIELD();
            m_tempFields[i] = field;
            m_indicator->getDataSource()->addColumn(field);
            m_tempFieldsIndex[i] = m_indicator->getDataSource()->getColumnIndex(field);
        }
    }
    
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    CMathElement::CMathElement(){
        m_type = 0;
        m_value = 0;
        m_var = 0;
    }
    
    CMathElement::CMathElement(int type, double value){
        m_type = type;
        m_value = value;
    }
    
    CMathElement::~CMathElement(){
        m_var = 0;
    }
    
    //////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    CFunction::CFunction(){
        m_ID = 0;
        m_type = 0;
    }
    
    CFunction::~CFunction(){
    }
    
    double CFunction::onCalculate(CVariable *var){
        return 0;
    }
    
    //////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    String CVar::getText(FCScript *indicator, CVariable *name){
        if (m_type == 1){
            if ((int)m_str.length() > 0 && m_str[0] == '\''){
                return m_str.substr(1, m_str.length() - 2);
            }
            else{
                return m_str;
            }
        }
        else{
            return FCStr::convertDoubleToStr(m_num);
        }
    }
    
    double CVar::getValue(FCScript *indicator, CVariable *name){
        if (m_type == 1){
            return FCStr::convertStrToDouble(FCStr::replace(m_str, L"\'", L""));
        }
        else{
            return m_num;
        }
    }
    
    double CVar::onCreate(FCScript *indicator, CVariable *name, CVariable *value){
        double result = 0;
        int id = name->m_field;
        if ((int)value->m_expression.length() > 0 && value->m_expression[0] == '\''){
            m_type = 1;
            m_str = value->m_expression.substr(1, value->m_expression.length() - 2);
        }
        else{
            if (value->m_expression == L"LIST"){
                m_type = 2;
                m_list = new ArrayList<String>;
            }
            else if (value->m_expression == L"MAP"){
                m_type = 3;
                m_map = new HashMap<String, String>;
            }
            else if (indicator->m_tempVars.containsKey(value->m_field)){
                CVar *otherCVar = indicator->m_tempVars.get(value->m_field);
                if (otherCVar->m_type == 1){
                    m_type = 1;
                    m_str = otherCVar->m_str;
                }
                else{
                    m_type = 0;
                    m_num = otherCVar->m_num;
                }
            }
            else{
                m_type = 0;
                result = indicator->getValue(value);
                m_num = result;
            }
        }
        return result;
    }
    
    void CVar::setValue(FCScript *indicator, CVariable *name, CVariable *value){
        if (m_type == 1){
            m_str = indicator->getText(value);
        }
        else{
            m_num = indicator->getValue(value);
        }
    }
    
    //////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    
    void FCScript::analysisVariables(String *sentence, int line, String funcName, String fieldText, bool isFunction){
        vector<String> wordsList;
        int splitWordsSize = 0;
        String *splitWords = splitExpression2(*sentence, &splitWordsSize);
        for(int s = 0; s < splitWordsSize; s++){
            String wStr = splitWords[s];
            ArrayList<String> subWStr = FCStr::split(wStr, VARIABLE2);
            int subWStrSize = (int)subWStr.size();
            for (int u = 0; u < subWStrSize; u++){
                ArrayList<String> sunWStr = FCStr::split(subWStr.get(u), L":");
                int sunWStrSize = (int)sunWStr.size();
                for(int w = 0; w < sunWStrSize; w++){
                    if (m_functions.containsKey(sunWStr.get(w))){
                        wordsList.push_back(sunWStr.get(w));
                    }
                }
                sunWStr.clear();
            }
            subWStr.clear();
        }
        delete[] splitWords;
        splitWords = 0;
        int wordsListSize = (int)wordsList.size();
        for (int f = 0; f < wordsListSize; f++){
            String word = wordsList[f];
            CFunction *func = m_functions.get(word);
            String fName = func->m_name;
            int funcID = func->m_ID;
            int funcType = func->m_type;
            String function = fName + L"(";
            int bIndex = (int)(*sentence).find(function);
            while (bIndex != -1){
                int rightBracket = 0;
                int idx = 0;
                int count = 0;
                int stsize = (int)sentence->length();
                for(int s = 0; s < stsize; s++){
                    String ch = sentence->substr(s, 1);
                    if (idx >= bIndex){
                        if (ch == L"("){
                            count++;
                        }
                        else if (ch == L")"){
                            count--;
                            if (count == 0){
                                rightBracket = idx;
                                break;
                            }
                        }
                    }
                    idx++;
                }
                if (rightBracket == 0){
                    break;
                }
                String body = sentence->substr(bIndex, rightBracket - bIndex + 1);
                CVariable *var = new CVariable();
                var->m_indicator = this;
                wchar_t szTemp[20] ={0};
                swprintf(szTemp, 19, L"%d", m_variables.size());
                var->m_name = VARIABLE + szTemp;
                var->m_expression = body.substr(0, body.find('('));
                var->m_type = 0;
                var->m_functionID = funcID;
                var->m_fieldText = body;
                if(funcType == 1){
                    int field = m_dataSource->AUTOFIELD();
                    var->m_field = field;
                    m_dataSource->addColumn(field);
                }
                m_variables.add(var);
                if (bIndex == 0){
                    if (isFunction){
                        var->m_funcName = funcName;
                        var->m_line = line;
                        var->m_fieldText = fieldText;
                        m_lines.add(var);
                        m_tempFunctions.put(funcName, var);
                        isFunction = false;
                    }
                }
                int splitExpressionLength = 0;
                var->m_splitExpression = splitExpression(var->m_expression, &splitExpressionLength);
                var->m_splitExpressionLength = splitExpressionLength;
                int startIndex = bIndex + (int)function.length();
                String subSentence = sentence->substr(startIndex, rightBracket - startIndex);
                if (funcID == FUNCTIONID_FUNCTION){
                    if (m_tempFunctions.containsKey(fName)){
                        if ((int)m_tempFunctions.get(fName)->m_fieldText.length() > 0){
                            ArrayList<String> fieldTexts = FCStr::split(m_tempFunctions.get(fName)->m_fieldText, VARIABLE2);
                            ArrayList<String> transferParams = FCStr::split(subSentence, VARIABLE2);
                            subSentence = L"";
                            int transferParamsLen = (int)transferParams.size();
                            for (int i = 0; i < transferParamsLen; i++){
                                if (i == 0){
                                    subSentence = L"FUNCVAR(";
                                }
                                subSentence += fieldTexts.get(i) + VARIABLE2 + transferParams.get(i);
                                if (i != transferParamsLen - 1){
                                    subSentence += VARIABLE2;
                                }
                                else{
                                    subSentence += L")";
                                }
                            }
                            fieldTexts.clear();
                            transferParams.clear();
                        }
                    }
                }
                analysisVariables(&subSentence, 0, L"", L"", false);
                ArrayList<String> parameters = FCStr::split(subSentence, VARIABLE2);
                int psize = (int)parameters.size();
                if (psize > 0){
                    var->m_parameters = new CVariable*[psize];
                    var->m_parametersLength = psize;
                    for (int j = 0; j < psize; j++){
                        String parameter = parameters.get(j);
                        parameter = replace(parameter);
                        CVariable *pVar = new CVariable();
                        pVar->m_indicator = this;
                        pVar->m_expression = parameter;
                        swprintf(szTemp, 19, L"%d", m_variables.size());
                        pVar->m_name = VARIABLE + szTemp;
                        pVar->m_type = 1;
                        var->m_parameters[j] = pVar;
                        for (int v = 0; v < m_variables.size(); v++){
                            CVariable *variable = m_variables.get(v);
                            if (variable->m_type == 2 && variable->m_expression == parameters.get(j) && variable->m_field != FCDataTable::NULLFIELD()){
                                pVar->m_type = 2;
                                pVar->m_field = variable->m_field;
                                pVar->m_fieldText = parameters.get(j);
                                break;
                            }
                        }
                        if(pVar->m_type == 1){
                            String varKey = parameter;
                            if (varKey.find(L"[REF]") == 0){
                                varKey = varKey.substr(5);
                            }
                            if(m_tempVariables.containsKey(varKey)){
                                pVar->m_field = m_tempVariables.get(varKey)->m_field;
                            }
                            else{
                                pVar->m_field = -((int)m_variables.size() + 1);
                                m_tempVariables.put(varKey, pVar);
                            }
                        }
                        m_variables.add(pVar);
                        splitExpressionLength = 0;
                        pVar->m_splitExpression = splitExpression(parameter, &splitExpressionLength);
                        pVar->m_splitExpressionLength = splitExpressionLength;
                        if (pVar->m_splitExpressionLength == 2){
                            if (pVar->m_splitExpression[0]->m_var == pVar){
                                delete[] pVar->m_splitExpression;
                                pVar->m_splitExpression = 0;
                                pVar->m_splitExpressionLength = 0;
                            }
                        }
                    }
                }
                *sentence = sentence->substr(0, bIndex) + var->m_name + sentence->substr(rightBracket + 1);
                bIndex = (int)(*sentence).find(function, sentence->find(var->m_name));
            }
        }
        wordsList.clear();
    }
    
    void FCScript::analysisScriptLine(String line){
        CVariable *script = new CVariable();
        bool isFunction = false;
        String strLine = line;
        String funcName;
        String fieldText;
        script->m_indicator = this;
        if (FCStr::toUpper(strLine).find(L"FUNCTION ") == 0){
            int cindex = (int)strLine.find(L"(");
            funcName = strLine.substr(9, cindex - 9);
            int rindex = (int)strLine.find(L")");
            if (rindex - cindex > 1){
                fieldText = strLine.substr(cindex + 1, rindex - cindex - 1);
                ArrayList<String> pList = FCStr::split(fieldText, VARIABLE2);
                int pListSize = (int)pList.size();
                for (int i = 0; i < pListSize; i++){
                    String str = pList.get(i);
                    if(str.find(L"[REF]") != -1){
                        str = str.substr(5);
                    }
                    String pCmd = L"VAR(" + str + VARIABLE2 + L"0)";
                    analysisVariables(&pCmd, 0, L"", L"", false);
                }
                pList.clear();
            }
            strLine = strLine.substr(rindex + 1);
            strLine = L"CHUNK" + strLine.substr(0, strLine.length() - 1) + L")";
            isFunction = true;
        }
        analysisVariables(&strLine, (int)m_lines.size(), funcName, fieldText, isFunction);
        script->m_line = (int)m_lines.size();
        if (isFunction){
            delete script;
            script = 0;
            return;
        }
        String variable;
        String sentence;
        String followParameters;
        String op;
        int lineSize = (int)strLine.size();
        for(int i = 0; i < lineSize; i++){
            wchar_t ch = strLine[i];
            if (ch != ':' && ch != '='){
                if((int)op.length() > 0){
                    break;
                }
            }
            else{
                wchar_t szOp[2] ={0};
                szOp[0] = ch;
                op += szOp;
            }
        }
        if (op == L":="){
            int pos = (int)strLine.find(L":=");
            variable = strLine.substr(0, pos);
            sentence = strLine.substr(pos + 2);
        }
        else if(op == L":"){
            int pos = (int)strLine.find(L":");
            if(pos != -1){
                followParameters = L"FCColorAUTO";
                variable = strLine.substr(0, pos);
                sentence = strLine.substr(pos + 1);
                pos = (int)sentence.find(VARIABLE2);
                if (pos != -1){
                    followParameters = sentence.substr(pos + 1);
                    sentence = sentence.substr(0, pos);
                }
            }
        }
        else{
            sentence = strLine;
            ArrayList<String> strs = FCStr::split(sentence, VARIABLE2);
            if (strs.size() > 1){
                String strVar = strs.get(0);
                sentence = strVar;
                int idx = FCStr::convertStrToInt(strVar.substr(1).c_str());
                if(idx < (int)m_variables.size()){
                    CVariable *var = m_variables.get(idx);
                    int startIndex = 0;
                    if (!var->m_parameters){
                        var->m_parameters = new CVariable*[strs.size() - 1];
                        var->m_parametersLength = (int)strs.size() - 1;
                        startIndex = 0;
                    }
                    else{
                        int length = var->m_parametersLength + (int)strs.size() - 1;
                        CVariable **newParameters = new CVariable*[var->m_parametersLength + (int)strs.size() - 1];
                        for (int i = 0; i < var->m_parametersLength; i++){
                            newParameters[i] = var->m_parameters[i];
                        }
                        startIndex = var->m_parametersLength;
                        delete[] var->m_parameters;
                        var->m_parameters = newParameters;
                        var->m_parametersLength = length;
                    }
                    int strSize = (int)strs.size();
                    for (int i = 1; i < strSize; i++){
                        CVariable *newVar = new CVariable();
                        newVar->m_indicator = this;
                        newVar->m_type = 1;
                        newVar->m_expression = strs.get(i);
                        var->m_parameters[startIndex + i - 1] = newVar;
                    }
                }
            }
        }
        script->m_expression = replace(sentence);
        m_variables.add(script);
        m_lines.add(script);
        if (variable.length() > 0){
            script->m_type = 1;
            CVariable *pfunc = new CVariable();
            pfunc->m_indicator = this;
            pfunc->m_type = 2;
            wchar_t szTemp[20] ={0};
            swprintf(szTemp, 19, L"%d", m_variables.size());
            pfunc->m_name = VARIABLE + szTemp;
            int field = FCDataTable::NULLFIELD();
            if (sentence.length() >= 1 && sentence.substr(0, 1) == VARIABLE){
                String num = sentence.substr(1);
                if(isNumeric(num)){
                    for (int v = 0; v < m_variables.size(); v++){
                        CVariable *var = m_variables.get(v);
                        if(var->m_name == sentence && var->m_field != FCDataTable::NULLFIELD()){
                            field = var->m_field;
                        }
                    }
                }
            }
            if (field == FCDataTable::NULLFIELD()){
                field = m_dataSource->AUTOFIELD();
                m_dataSource->addColumn(field);
            }
            else{
                script->m_type = 0;
            }
            pfunc->m_field = field;
            pfunc->m_expression = variable;
            int splitExpressionLength = 0;
            pfunc->m_splitExpression = splitExpression(variable, &splitExpressionLength);
            pfunc->m_splitExpressionLength = splitExpressionLength;
            m_variables.add(pfunc);
            m_mainVariables.put(variable, field);
            script->m_field = field;
        }
        if (followParameters.length() > 0){
            String newLine;
            if (followParameters.find(L"FCColorSTICK") != -1){
                newLine = L"STICKLINE(1" + VARIABLE2 + variable + VARIABLE2 + L"0" + VARIABLE2 + L"1" + VARIABLE2 + L"2" + VARIABLE2 + L"DRAWTITLE)";
            }
            else if (followParameters.find(L"CIRCLEDOT") != -1){
                newLine = L"DRAWICON(1" + VARIABLE2 + variable + VARIABLE2 + L"CIRCLEDOT" + VARIABLE2 + L"DRAWTITLE)";
            }
            else if (followParameters.find(L"FCPointDOT") != -1){
                newLine = L"DRAWICON(1" + VARIABLE2 + variable + VARIABLE2 + L"FCPointDOT" + VARIABLE2 + L"DRAWTITLE)";
            }
            else{
                newLine = L"POLYLINE(1" + VARIABLE2 + variable + VARIABLE2 + followParameters + VARIABLE2 + L"DRAWTITLE)";
            }
            analysisScriptLine(newLine);
        }
        int splitExpressionLength = 0;
        script->m_splitExpression = splitExpression(script->m_expression, &splitExpressionLength);
        script->m_splitExpressionLength = splitExpressionLength;
    }
    
    double FCScript::calculate(CMathElement **expr, int exprLength){
        CMathElement **optr = new CMathElement*[exprLength];
        int optrLength = 1;
        CMathElementEx exp(3, 0);
        optr[0] = &exp;
        CMathElement **opnd = new CMathElement*[exprLength];
        int opndLength = 0;
        int idx = 0;
        CMathElementEx *m_last = &exp;
        CMathElement *right = 0;
        while (idx < exprLength && (expr[idx]->m_type != 3 || optr[optrLength - 1]->m_type != 3)){
            CMathElement *Q2 = expr[idx];
            if (Q2->m_type != 0 && Q2->m_type != 3){
                opnd[opndLength] = Q2;
                opndLength++;
                idx++;
            }
            else{
                CMathElement *Q1 = optr[optrLength - 1];
                int precede = -1;
                if (Q2->m_type == 3){
                    if (Q1->m_type == 3){
                        precede = 3;
                    }
                    else{
                        precede = 4;
                    }
                }
                else{
                    int q1Value = (int)Q1->m_value;
                    int q2Value = (int)Q2->m_value;
                    switch (q2Value){
                        case 0:
                        case 1:
                        case 3:
                        case 4:
                        case 5:
                        case 7:
                        case 8:
                        case 10:
                        case 11:
                        case 13:
                        case 14:
                            if (Q1->m_type == 3 || (Q1->m_type == 0 && q1Value == 6)){
                                precede = 7;
                            }
                            else{
                                precede = 4;
                            }
                            break;
                        case 2:
                        case 9:
                            
                            if (Q1->m_type == 0 && (q1Value == 9 || q1Value == 2 || q1Value == 12)){
                                precede = 4;
                            }
                            else{
                                precede = 7;
                            }
                            break;
                        case 6:
                            precede = 7;
                            break;
                        case 12:
                            if (Q1->m_type == 0 && q1Value == 6){
                                precede = 3;
                            }
                            else{
                                precede = 4;
                            }
                            break;
                    }
                }
                switch(precede){
                    case 7:
                        optr[optrLength] = expr[idx];
                        optrLength++;
                        idx++;
                        break;
                    case 3:
                        optrLength--;
                        idx++;
                        break;
                    case 4:
                        if (opndLength == 0){
                            delete[] optr;
                            optr = 0;
                            delete[] opnd;
                            opnd = 0;
                            m_last = 0;
                            right = 0;
                            return 0;
                        }
                        int op = (int)optr[optrLength - 1]->m_value;
                        optrLength--;
                        double opnd1 = 0, opnd2 = 0;
                        CMathElement *left = opnd[opndLength - 1];
                        if (left->m_type == 2){
                            opnd2 = getValue(left->m_var);
                        }
                        else{
                            opnd2 = left->m_value;
                        }
                        if (opndLength > 1){
                            right = opnd[opndLength - 2];
                            if (right->m_type == 2){
                                opnd1 = getValue(right->m_var);
                            }
                            else{
                                opnd1 = right->m_value;
                            }
                            opndLength -= 2;
                        }
                        else{
                            opndLength--;
                        }
                        double result = 0;
                        switch (op){
                            case 0: result = opnd1 + opnd2; break;
                            case 1:{
                                if (opnd1 == 1 && opnd2 == 1) result = 1;
                                else result = 0;
                                break;
                            }
                            case 2:{
                                if (opnd2 == 0){
                                    result = 0;
                                }
                                else{
                                    result = opnd1 / opnd2;
                                }
                                break;
                            }
                            case 10:{
                                if ((left->m_var && left->m_var->m_functionID == -2) || (right && right->m_var && right->m_var->m_functionID == -2)){
                                    if (right && left->m_var && right->m_var){
                                        if (getText(left->m_var) != getText(right->m_var)){
                                            result = 1;
                                        }
                                    }
                                }
                                else{
                                    result = (opnd1 != opnd2 ? 1 : 0);
                                }
                                break;
                            }
                            case 4: result = (opnd1 > opnd2 ? 1 : 0); break;
                            case 5: result = (opnd1 >= opnd2 ? 1 : 0); break;
                            case 7: result = (opnd1 < opnd2 ? 1 : 0); break;
                            case 8: result = (opnd1 <= opnd2 ? 1 : 0); break;
                            case 9: result = opnd1 * opnd2; break;
                            case 3:{
                                if ((left->m_var && left->m_var->m_functionID == -2) || (right && right->m_var && right->m_var->m_functionID == -2)){
                                    if (right && left->m_var && right->m_var){
                                        if (getText(left->m_var) == getText(right->m_var)){
                                            result = 1;
                                        }
                                    }
                                }
                                else{
                                    result = (opnd1 == opnd2 ? 1 : 0);
                                }
                                break;
                            }
                            case 11:{
                                if (opnd1 == 1 || opnd2 == 1) result = 1;
                                else result = 0;
                                break;
                            }
                            case 13: result = opnd1 - opnd2; break;
                            case 14:{
                                if (opnd2 == 0){
                                    result = 0;
                                }
                                else{
                                    result = (int)opnd1 % (int)opnd2;
                                }
                                break;
                            }
                            default: result = 0;
                        }
                        if(m_break > 0){
                            delete[] optr;
                            optr = 0;
                            delete[] opnd;
                            opnd = 0;
                            m_last = 0;
                            right = 0;
                            return result;
                        }
                        else{
                            CMathElementEx *newElement = new CMathElementEx(1, result);
                            m_last->m_next = newElement;
                            m_last = newElement;
                            opnd[opndLength] = newElement;
                            opndLength++;
                        }
                        break;
                }
            }
        }
        double value = 0;
        if (opndLength > 0){
            CMathElement *rlast = opnd[opndLength - 1];
            if (rlast->m_type == 2){
                value = getValue(rlast->m_var);
            }
            else{
                value = rlast->m_value;
            }
        }
        delete[] optr;
        optr = 0;
        delete[] opnd;
        opnd = 0;
        m_last = 0;
        right = 0;
        return value;
    }
    
    double FCScript::callFunction(CVariable *var){
        switch (var->m_functionID){
            case 0: return CURRBARSCOUNT(var);
            case 1: return BARSCOUNT(var);
            case 2: return DRAWKLINE(var);
            case 3: return STICKLINE(var);
            case 4: return VALUEWHEN(var);
            case 5: return BARSLAST(var);
            case 6: return DOWNNDAY(var);
            case 7: return DRAWICON(var);
            case 8: return DRAWNULL(var);
            case 9: return FUNCTION(var);
            case 10: return FUNCVAR(var);
            case 11: return DRAWTEXT(var);
            case 12: return POLYLINE(var);
            case 13: return BETWEEN(var);
            case 14: return CEILING(var);
            case 15: return EXPMEMA(var);
            case 16: return HHVBARS(var);
            case 17: return INTPART(var);
            case 18: return LLVBARS(var);
            case 19: return DOTIMES(var);
            case 20: return DOWHILE(var);
            case 21: return CONTINUE(var);
            case 22: return RETURN(var);
            case 23: return REVERSE(var);
            case 24: return AVEDEV(var);
            case 25: return MINUTE(var);
            case 26: return SQUARE(var);
            case 27: return UPNDAY(var);
            case 28: return DELETE2(var);
            case 29: return COUNT(var);
            case 30: return CROSS(var);
            case 31: return EVERY(var);
            case 32: return EXIST(var);
            case 33: return EMA(var);
            case 34: return FLOOR(var);
            case 35: return MONTH(var);
            case 36: return ROUND(var);
            case 37: return TIME2(var);
            case 38: return WHILE(var);
            case 39: return BREAK(var);
            case 40: return CHUNK(var);
            case 41: return ACOS(var);
            case 42: return ASIN(var);
            case 43: return ATAN(var);
            case 44: return DATE(var);
            case 45: return HOUR(var);
            case 46: return LAST(var);
            case 47: return MEMA(var);
            case 48: return NDAY(var);
            case 49: return RAND(var);
            case 50: return SIGN(var);
            case 51: return SQRT(var);
            case 52: return TIME(var);
            case 53: return YEAR(var);
            case 54: return ABS2(var);
            case 55: return AMA(var);
            case 56: return COS(var);
            case 57: return DAY(var);
            case 58: return DMA(var);
            case 59: return EMA(var);
            case 60: return EXP(var);
            case 61: return HHV(var);
            case 62: return IF(var);
            case 63: return IFN(var);
            case 64: return LLV(var);
            case 65: return LOG(var);
            case 66: return MAX2(var);
            case 67: return MIN2(var);
            case 68: return MOD(var);
            case 69: return NOT(var);
            case 70: return POW(var);
            case 71: return SIN(var);
            case 72: return SMA(var);
            case 73: return STD(var);
            case 74: return SUM(var);
            case 75: return TAN(var);
            case 76: return REF(var);
            case 77: return SAR(var);
            case 78: return FOR(var);
            case 79: return GET(var);
            case 80: return SET(var);
            case 81: return TMA(var);
            case 82: return VAR(var);
            case 83: return WMA(var);
            case 84: return ZIG(var);
            case 85: return IF(var);
            case 86: return MA(var);
            case 87: return STR_CONTACT(var);
            case 88: return STR_EQUALS(var);
            case 89: return STR_FIND(var);
            case 90: return STR_FINDLAST(var);
            case 91: return STR_LENGTH(var);
            case 92: return STR_SUBSTR(var);
            case 93: return STR_REPLACE(var);
            case 94: return STR_SPLIT(var);
            case 95: return STR_TOLOWER(var);
            case 96: return STR_TOUPPER(var);
            case 97: return LIST_ADD(var);
            case 98: return LIST_CLEAR(var);
            case 99: return LIST_GET(var);
            case 100: return LIST_INSERT(var);
            case 101: return LIST_REMOVE(var);
            case 102: return LIST_SIZE(var);
            case 103: return MAP_CLEAR(var);
            case 104: return MAP_CONTAINSKEY(var);
            case 105: return MAP_GET(var);
            case 106: return MAP_GETKEYS(var);
            case 107: return MAP_REMOVE(var);
            case 108: return MAP_SET(var);
            case 109: return MAP_SIZE(var);
            default:{
                if(m_functionsMap.containsKey(var->m_functionID)){
                    return m_functionsMap.get(var->m_functionID)->onCalculate(var);
                }
                return 0;
            }
        }
    }
    
    void FCScript::deleteTempVars(){
        while((int)m_tempVars.size() > 0){
            ArrayList<int> removeIDs;
            for(int c = 0; c < m_tempVars.size(); c++){
                removeIDs.add(m_tempVars.getKey(c));
            }
            int removeIDsSize = (int)removeIDs.size();
            for(int i = 0; i < removeIDsSize; i++){
                CVar *cVar = m_tempVars.get(removeIDs.get(i));
                if(cVar->m_parent){
                    m_tempVars.put(removeIDs.get(i), cVar->m_parent);
                }
                else{
                    m_tempVars.remove(removeIDs.get(i));
                }
                delete cVar;
                cVar = 0;
            }
            removeIDs.clear();
        }
    }
    
    void FCScript::deleteTempVars(CVariable *var){
        if (var->m_parameters){
            int pLen = var->m_parametersLength;
            if (pLen > 0){
                for (int i = 0; i < pLen; i++){
                    CVariable *parameter = var->m_parameters[i];
                    if (parameter->m_splitExpression && parameter->m_splitExpressionLength > 0){
                        CVariable *subVar = parameter->m_splitExpression[0]->m_var;
                        if (subVar && (subVar->m_functionID == FUNCTIONID_FUNCVAR
                                       || subVar->m_functionID == FUNCTIONID_VAR)){
                            int sunLen = subVar->m_parametersLength;
                            for (int j = 0; j < sunLen; j++){
                                if (j % 2 == 0){
                                    CVariable *sunVar = subVar->m_parameters[j];
                                    int id = sunVar->m_field;
                                    if (sunVar->m_expression.find(L"[REF]") == 0){
                                        int variablesSize = (int)m_variables.size();
                                        for (int k = 0; k < variablesSize; k++){
                                            CVariable *variable = m_variables.get(k);
                                            if (variable->m_expression == sunVar->m_expression){
                                                variable->m_field = id;
                                            }
                                        }
                                    }
                                    else{
                                        if (m_tempVars.containsKey(id)){
                                            CVar *cVar = m_tempVars.get(id);
                                            if (cVar->m_parent){
                                                m_tempVars.put(id, cVar->m_parent);
                                            }
                                            else{
                                                m_tempVars.remove(id);
                                            }
                                            delete cVar;
                                            cVar = 0;
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
    
    Long FCScript::getColor(const String& strColor){
        if(strColor == L"FCColorRED") return FCColor::argb(255, 0, 0);
        else if(strColor == L"FCColorGREEN") return FCColor::argb(0, 255, 0);
        else if(strColor == L"FCColorBLUE") return FCColor::argb(0, 0, 255);
        else if(strColor == L"FCColorMAGENTA") return FCColor::argb(255, 0, 255);
        else if(strColor == L"FCColorYELLOW") return FCColor::argb(255, 255, 0);
        else if(strColor == L"FCColorLIGHTGREY") return FCColor::argb(211, 211, 211);
        else if(strColor == L"FCColorLIGHTRED") return FCColor::argb(255, 82, 82);
        else if(strColor == L"FCColorLIGHTGREEN") return FCColor::argb(144, 238, 144);
        else if(strColor == L"FCColorLIGHTBLUE") return FCColor::argb(173, 216, 230);
        else if(strColor == L"FCColorBLACK") return FCColor::argb(0, 0, 0);
        else if(strColor == L"FCColorWHITE") return FCColor::argb(255, 255, 255);
        else if(strColor == L"FCColorCYAN") return FCColor::argb(0, 255, 255);
        else if(strColor == L"FCColorAUTO"){
            int lineCount = 0;
            Long lineColor = FCColor_None;
            ArrayList<BaseShape*> shapes = getShapes();
            for (int b = 0; b < shapes.size(); b++){
                BaseShape *shape = shapes.get(b);
                PolylineShape *polylineShape = dynamic_cast<PolylineShape*>(shape);
                if(polylineShape){
                    lineCount++;
                }
            }
            int systemColorsSize = (int)m_systemColors.size();
            if(systemColorsSize > 0){
                lineColor = m_systemColors.get(lineCount % systemColorsSize);
            }
            return lineColor;
        }
        else{
            String htmlColor = strColor.substr(5);
            string sHtmlColor = FCStr::wstringTostring(htmlColor);
            string strR = "0x" + sHtmlColor.substr(0, 2);
            string strG = "0x" + sHtmlColor.substr(2, 2);
            string strB = "0x" + sHtmlColor.substr(4, 2);
            int r = FCStr::hexToDec(strR.c_str());
            int g = FCStr::hexToDec(strG.c_str());
            int b = FCStr::hexToDec(strB.c_str());
            return FCColor::argb(r, g, b);
        }
    }
    
    LPDATA FCScript::getDatas(int fieldIndex, int mafieldIndex, int index, int n){
        LPDATA math_struct;
        math_struct.mode = 1;
        math_struct.lastvalue = 0;
        math_struct.first_value = 0;
        if (index >= 0){
            double value = m_dataSource->get3(index, mafieldIndex);
            if (!FCDataTable::isNaN(value)){
                math_struct.lastvalue = value;
                if (index >= n - 1){
                    double nValue = m_dataSource->get3(index + 1 - n, fieldIndex);
                    if (!FCDataTable::isNaN(nValue)){
                        math_struct.first_value = nValue;
                    }
                }
            }
            else{
                math_struct.mode = 0;
                int start = index - n + 2;
                if (start < 0) start = 0;
                for (int i = start; i <= index; i++){
                    double lValue = m_dataSource->get3(i, fieldIndex);
                    if (!FCDataTable::isNaN(lValue)){
                        math_struct.sum += lValue;
                    }
                }
            }
        }
        return math_struct;
    }
    
    float FCScript::getLineWidth(const String& strLine){
        float lineWidth = 1;
        if (strLine.length() > 9){
            lineWidth = FCStr::convertStrToFloat(strLine.substr(9).c_str());
        }
        return lineWidth;
    }
    
    int FCScript::getMiddleScript(const String& script, ArrayList<String> *lines){
        String value = script;
        value = FCStr::replace(value, L" AND ", L"&");
        value = FCStr::replace(value, L" OR ", L"|");
        String line;
        bool isstr = false;
        wchar_t lh = '0';
        bool isComment = false;
        bool functionBegin = false;
        int kh = 0;
        bool isReturn = false, isVar = false, isNewParam = false, isSet = false;
        int scriptLen = (int)value.length();
        for(int i = 0; i < scriptLen; i++){
            wchar_t ch =  value[i];
            if (ch == '\''){
                isstr = !isstr;
            }
            if (!isstr){
                if (ch == '{'){
                    int lineLength = (int)line.length();
                    if (lineLength == 0){
                        isComment = true;
                    }
                    else{
                        if (!isComment){
                            kh++;
                            if (functionBegin && kh == 1){
                                line += L"(";
                            }
                            else{
                                if (line.rfind(L")") == lineLength - 1){
                                    line = line.substr(0, lineLength - 1) + VARIABLE2 + L"CHUNK(";
                                }
                                else if (line.rfind(L"))"+ VARIABLE2 + L"ELSE") == lineLength - 7){
                                    line = line.substr(0, lineLength - 6) + VARIABLE2 + L"CHUNK(";
                                }
                            }
                        }
                    }
                }
                else if (ch == '}'){
                    if (isComment){
                        isComment = false;
                    }
                    else{
                        kh--;
                        int lineLength = (int)line.length();
                        if (functionBegin && kh == 0){
                            if (lineLength > 0){
                                if (line[lineLength - 1] == VARIABLE2[0]){
                                    line = line.substr(0, lineLength - 1);
                                }
                            }
                            line += L")";
                            lines->add(line);
                            functionBegin = false;
                            line = L"";
                        }
                        else{
                            if (kh == 0){
                                line += L"))";
                                lines->add(line);
                                line = L"";
                            }
                            else{
                                line += L"))" + VARIABLE2;
                            }
                        }
                    }
                }
                else if (ch == ' '){
                    int lineLength = (int)line.length();
                    if (line == L"CONST"){
                        line = L"CONST ";
                    }
                    else if (line == L"FUNCTION"){
                        line = L"FUNCTION ";
                        functionBegin = true;
                    }
                    else if (!isReturn && ((int)line.rfind(L"RETURN") == lineLength - 6)){
                        if (lineLength == 6 || ((int)line.rfind(L")RETURN") == lineLength - 7
                                                || (int)line.rfind(L"(RETURN") == lineLength - 7
                                                || (int)line.rfind(VARIABLE2 + L"RETURN") == lineLength - 7)){
                            line += L"(";
                            isReturn = true;
                        }
                    }
                    else if (!isVar && (int)line.rfind(L"VAR") == lineLength - 3){
                        if (lineLength == 3 || ((int)line.rfind(L")VAR") == lineLength - 4
                                                || (int)line.rfind(L"(VAR") == lineLength - 4
                                                || (int)line.rfind(VARIABLE2 + L"VAR") == lineLength - 4)){
                            line += L"(";
                            isVar = true;
                            isNewParam = true;
                        }
                    }
                    else if (!isSet && line.rfind(L"SET") == lineLength - 3){
                        if (lineLength == 3 || (line.rfind(L")SET") == lineLength - 4
                                                || line.rfind(L"(SET") == lineLength - 4
                                                || line.rfind(VARIABLE2 + L"SET") == lineLength - 4)){
                            line = line.substr(0, lineLength - 3) + L"SET(";
                            isSet = true;
                            isNewParam = true;
                        }
                    }
                    else{
                        continue;
                    }
                }
                else if (ch != '\t' && ch != '\r' && ch != '\n'){
                    if (!isComment){
                        if (ch == '&'){
                            if (lh != '&'){
                                wchar_t newCh[2] ={0};
                                newCh[0] = ch;
                                newCh[1] = '\0';
                                line += newCh;
                            }
                        }
                        else if (ch == '|'){
                            if (lh != '|'){
                                wchar_t newCh[2] ={0};
                                newCh[0] = ch;
                                newCh[1] = '\0';
                                line += newCh;
                            }
                        }
                        else if (ch == '-'){
                            wchar_t strLh[2] ={0};
                            strLh[0] = lh;
                            strLh[1] = '\0';
                            if (strLh != VARIABLE2 && getOperator(strLh) != -1 && lh != ')'){
                                wchar_t newCh[2] ={0};
                                newCh[0] = ch;
                                newCh[1] = '\0';
                                line += newCh;
                            }
                            else{
                                line += VARIABLE3;
                                lh = VARIABLE3[0];
                                continue;
                            }
                        }
                        else if (ch == '='){
                            if (isVar && isNewParam){
                                isNewParam = false;
                                line += VARIABLE2;
                            }
                            else if (isSet && isNewParam){
                                isNewParam = false;
                                line += VARIABLE2;
                            }
                            else if (lh != '=' && lh != '!'){
                                wchar_t newCh[2] ={0};
                                newCh[0] = ch;
                                newCh[1] = '\0';
                                line += newCh;
                            }
                        }
                        else if (ch == ','){
                            isNewParam = true;
                            line += VARIABLE2;
                        }
                        else if (ch == ';'){
                            if (isReturn){
                                line += L")";
                                isReturn = false;
                            }
                            else if (isVar){
                                line += L")";
                                isVar = false;
                            }
                            else if (isSet){
                                line += L")";
                                isSet = false;
                            }
                            else{
                                int lineLength = (int)line.length();
                                if ((int)line.rfind(L"BREAK") == lineLength - 5){
                                    if ((int)line.rfind(L")BREAK") == lineLength - 6
                                        || (int)line.rfind(L"(BREAK") == lineLength - 6
                                        || (int)line.rfind(VARIABLE2 + L"BREAK") == lineLength - 6){
                                        line += L"()";
                                    }
                                }
                                else if ((int)line.rfind(L"CONTINUE") == lineLength - 8){
                                    if ((int)line.rfind(L")CONTINUE") == lineLength - 9
                                        || (int)line.rfind(L"(CONTINUE") == lineLength - 9
                                        || (int)line.rfind(VARIABLE2 + L"CONTINUE") == lineLength - 9){
                                        line += L"()";
                                    }
                                }
                            }
                            if (kh > 0){
                                line += VARIABLE2;
                            }
                            else{
                                lines->add(line);
                                line = L"";
                            }
                        }
                        else if (ch == '('){
                            int lineLength = (int)line.length();
                            if (kh > 0 && line.rfind(L"))" + VARIABLE2 + L"ELSEIF") == lineLength - 9){
                                line = line.substr(0, lineLength - 9) + L")" + VARIABLE2;
                            }
                            else{
                                line += L"(";
                            }
                        }
                        else{
                            wchar_t newCh[2] ={0};
                            newCh[0] = ch;
                            newCh[1] = '\0';
                            line += FCStr::toUpper(newCh);
                        }
                    }
                }
            }
            else{
                wchar_t newCh[2] ={0};
                newCh[0] = ch;
                newCh[1] = '\0';
                line += newCh;
            }
            lh = ch;
        }
        return 0;
    }
    
    
    int FCScript::getOperator(const String& op){
        if(op == L">=")
            return 5;
        else if(op == L"<=")
            return 8;
        else if(op == L"<>" || op == L"!")
            return 10;
        else if(op == L"+")
            return 0;
        else if(op == VARIABLE3)
            return 13;
        else if(op == L"*")
            return 9;
        else if(op == L"/")
            return 2;
        else if(op == L"(")
            return 6;
        else if(op == L")")
            return 12;
        else if(op == L"=")
            return 3;
        else if(op == L">")
            return 4;
        else if(op == L"<")
            return 7;
        else if(op == L"&")
            return 1;
        else if(op == L"|")
            return 11;
        else if(op == L"%")
            return 14;
        else
            return -1;
    }
    
    bool FCScript::isNumeric(const String& str){
        int strLength = (int)str.length();
        if(str.length() > 0){
            for(int i = 0; i < strLength; i++){
                wchar_t ch = str.c_str()[i];
                if(i == 0){
                    if(strLength > 1 && ch == 45){
                        continue;
                    }
                }
                else if(i != strLength -1){
                    if(ch == 46){
                        continue;
                    }
                }
                if(ch < 48 || ch > 57){
                    return false;
                }
            }
            return true;
        }
        else{
            return false;
        }
    }
    
    String FCScript::replace(const String& parameter){
        int splitParametersLength = 0;
        String *splitParameters = splitExpression2(parameter, &splitParametersLength);
        for (int p = 0; p < splitParametersLength; p++){
            String str = splitParameters[p];
            if (m_defineParams.containsKey(str)){
                wchar_t szTemp[100] ={0};
                swprintf(szTemp, 99, L"%f", m_defineParams.get(str));
                splitParameters[p] = szTemp;
            }
            else{
                for (int v = 0; v < m_variables.size(); v++){
                    CVariable *var = m_variables.get(v);
                    if (var->m_type == 2 && var->m_expression == str){
                        splitParameters[p] = var->m_name;
                        break;
                    }
                }
            }
        }
        String newParameter;
        for (int p = 0; p < splitParametersLength - 1; p++){
            newParameter += splitParameters[p];
        }
        return newParameter;
    }
    
    CMathElement** FCScript::splitExpression(const String& expression, int *sLength){
        vector<String> lstItem;
        int length = (int)expression.length();
        String item;
        String ch;
        bool isstr = false;
        while (length != 0){
            ch = expression.substr((int)expression.length() - length, 1);
            if (ch == L"\'"){
                isstr = !isstr;
            }
            if (isstr || getOperator(ch) == -1) item += ch;
            else{
                if (item != L""){
                    lstItem.push_back(item);
                }
                item = L"";
                int nextIndex = (int)expression.length() - length + 1;
                String chNext;
                if (nextIndex < (int)expression.length() - 1){
                    chNext = expression.substr(nextIndex, 1);
                }
                String unionText = ch + chNext;
                if (unionText == L">=" || unionText == L"<=" || unionText == L"<>"){
                    lstItem.push_back(unionText);
                    length--;
                }
                else{
                    lstItem.push_back(ch);
                }
            }
            length--;
        }
        if (item != L""){
            lstItem.push_back(item);
        }
        int lstSize = (int)lstItem.size();
        CMathElement **exprs = new CMathElement*[lstSize + 1];
        for (int i = 0; i < lstSize; i++){
            CMathElement *expr = new CMathElement();
            String strExpr = lstItem[i];
            int op = getOperator(strExpr);
            if (op != -1){
                expr->m_type = 0;
                expr->m_value = op;
            }
            else{
                if(isNumeric(strExpr)){
                    expr->m_type = 1;
                    expr->m_value = FCStr::convertStrToDouble(strExpr.c_str());
                }
                else{
                    for (int v = 0; v < m_variables.size(); v++){
                        CVariable *var = m_variables.get(v);
                        if (var->m_name == strExpr || var->m_expression == strExpr){
                            expr->m_type = 2;
                            expr->m_var = var;
                            break;
                        }
                    }
                }
            }
            exprs[i] = expr;
        }
        CMathElement *lExpr = new CMathElement();
        lExpr->m_type = 3;
        *sLength = (int)lstItem.size() + 1;
        exprs[lstItem.size()] = lExpr;
        return exprs;
    }
    
    String* FCScript::splitExpression2(const String& expression, int *sLength){
        vector<String> lstItem;
        int length = (int)expression.length();
        String item;
        String ch;
        bool isstr = false;
        while (length != 0){
            ch = expression.substr(expression.length() - length, 1);
            if (ch == L"\'"){
                isstr = !isstr;
            }
            if (isstr || getOperator(ch) == -1) item += ch;
            else{
                if (item != L""){
                    lstItem.push_back(item);
                }
                item = L"";
                int nextIndex = (int)expression.length() - length + 1;
                String chNext;
                if (nextIndex < (int)expression.length() - 1){
                    chNext = expression.substr(nextIndex, 1);
                }
                String unionText = ch + chNext;
                if (unionText == L">=" || unionText == L"<=" || unionText == L"<>"){
                    lstItem.push_back(unionText);
                    length--;
                }
                else{
                    lstItem.push_back(ch);
                }
            }
            length--;
        }
        if (item != L""){
            lstItem.push_back(item);
        }
        int lstSize = (int)lstItem.size();
        String *exprs = new String[lstSize + 1];
        *sLength = lstSize + 1;
        for (int i = 0; i < lstSize; i++) exprs[i] = lstItem[i];
        exprs[lstItem.size()] = L"#";
        return exprs;
    }
    
    
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    ///////
    
    FCScript::FCScript(){
        mutex_x = PTHREAD_MUTEX_INITIALIZER;
        m_attachVScale = AttachVScale_Left;
        m_break = 0;
        m_dataSource = 0;
        m_div = 0;
        m_index = 0;
        m_result = 0;
        m_resultVar.m_type = -1;
        m_systemColors.add(FCColor::argb(255, 255, 255));
        m_systemColors.add(FCColor::argb(255, 255, 0));
        m_systemColors.add(FCColor::argb(255, 0, 255));
        m_systemColors.add(FCColor::argb(0, 255, 0));
        m_systemColors.add(FCColor::argb(82, 255, 255));
        m_systemColors.add(FCColor::argb(255, 82, 82));
        m_tag = 0;
        if(VARIABLE.length() == 0){
            VARIABLE = L"~";
        }
        if(VARIABLE2.length() == 0){
            VARIABLE2 = L"◎";
        }
        if(VARIABLE3.length() == 0){
            VARIABLE3 = L"?";
        }
        if(FUNCTIONS.length() == 0){
            FUNCTIONS = L"CURRBARSCOUNT,BARSCOUNT,DRAWKLINE,STICKLINE,VALUEWHEN,BARSLAST,DOWNNDAY,DRAWICON,DRAWNULL,FUNCTION,FUNCVAR,DRAWTEXT,POLYLINE,BETWEEN,CEILING,EXPMEMA,HHVBARS,INTPART,LLVBARS,DOTIMES,DOWHILE,CONTINUE,RETURN,REVERSE,AVEDEV,MINUTE,SQUARE,UPNDAY,DELETE,COUNT,CROSS,EVERY,EXIST,EXPMA,FLOOR,MONTH,ROUND,TIME2,WHILE,BREAK,CHUNK,ACOS,ASIN,ATAN,DATE,HOUR,LAST,MEMA,NDAY,RAND,SIGN,SQRT,TIME,YEAR,ABS,AMA,COS,DAY,DMA,EMA,EXP,HHV,IFF,IFN,LLV,LOG,MAX,MIN,MOD,NOT,POW,SIN,SMA,STD,SUM,TAN,REF,SAR,FOR,GET,SET,TMA,VAR,WMA,ZIG,IF,MA,STR.CONTACT,STR.EQUALS,STR.FIND,STR.FINDLAST,STR.LENGTH,STR.SUBSTR,STR.REPLACE,STR.SPLIT,STR.TOLOWER,STR.TOUPPER,LIST.ADD,LIST.CLEAR,LIST.GET,LIST.INSERT,LIST.REMOVE,LIST.SIZE,MAP.CLEAR,MAP.CONTAINSKEY,MAP.GET,MAP.GETKEYS,MAP.REMOVE,MAP.SET,MAP.SIZE";
        }
        if(FUNCTIONS_FIELD.length() == 0){
            FUNCTIONS_FIELD = L"EXPMEMA,EXPMA,MEMA,AMA,DMA,EMA,SMA,SUM,SAR,TMA,WMA,MA";
        }
        ArrayList<String> sysFunctions = FCStr::split(FUNCTIONS, L",");
        ArrayList<String> sysFunctionsField = FCStr::split(FUNCTIONS_FIELD, L",");
        int iSize = (int)sysFunctions.size();
        int jSize = (int)sysFunctionsField.size();
        for (int i = 0; i < iSize; i++){
            int cType = 0;
            for (int j = 0; j < jSize; j++){
                if (sysFunctions.get(i) == sysFunctionsField.get(j)){
                    cType = 1;
                    break;
                }
            }
            CFunction *func = new CFunction;
            func->m_ID = i;
            func->m_name = sysFunctions.get(i);
            func->m_type = cType;
            m_functions.put(func->m_name, func);
            m_functionsMap.put(i, func);
        }
        m_varFactory = new CVarFactory;
    }
    
    FCScript::~FCScript(){
        clear();
        m_dataSource = 0;
        m_div = 0;
        m_functionsMap.clear();
        for (int f = 0; f < m_functions.size(); f++){
            delete m_functions.getValue(f);
        }
        m_functions.clear();
        m_systemColors.clear();
        m_tag = 0;
        if(m_varFactory){
            delete m_varFactory;
            m_varFactory = 0;
        }
    }
    
    AttachVScale FCScript::getAttachVScale(){
        return m_attachVScale;
    }
    
    void FCScript::setAttachVScale(AttachVScale attachVScale){
        m_attachVScale = attachVScale;
    }
    
    FCDataTable* FCScript::getDataSource(){
        return m_dataSource;
    }
    
    void FCScript::setDataSource(FCDataTable *dataSource){
        m_dataSource = dataSource;
    }
    
    ChartDiv* FCScript::getDiv(){
        return m_div;
    }
    
    void FCScript::setDiv(ChartDiv *div){
        m_div = div;
    }
    
    int FCScript::getIndex(){
        return m_index;
    }
    
    String FCScript::getName(){
        return m_name;
    }
    
    void FCScript::setName(const String& name){
        m_name = name;
    }
    
    double FCScript::getResult(){
        return m_result;
    }
    
    void FCScript::setScript(const String& script){
        lock();
        m_lines.clear();
        m_defineParams.clear();
        ArrayList<String> lines;
        getMiddleScript(script, &lines);
        int linesCount = (int)lines.size();
        for (int i = 0; i < linesCount; i++){
            String strLine = lines.get(i);
            if (strLine.find(L"FUNCTION ") == 0){
                String funcName = FCStr::toUpper(strLine.substr(9, strLine.find(L"(") - 9));
                CFunction *function = new CFunction;
                function->m_ID = FUNCTIONID_FUNCTION;
                function->m_name = funcName;
                m_functions.put(function->m_name, function);
            }
            else if(strLine.find(L"CONST ") == 0){
                ArrayList<String> consts = FCStr::split(strLine.substr(6), L":");
                m_defineParams.put(consts.get(0), FCStr::convertStrToDouble(consts.get(1)));
                lines.removeAt(i);
                i--;
                linesCount--;
                consts.clear();
            }
        }
        linesCount = (int)lines.size();
        for(int i = 0; i < linesCount; i++){
            analysisScriptLine(lines.get(i));
        }
        lines.clear();
        unLock();
    }
    
    ArrayList<Long> FCScript::getSystemColors(){
        return m_systemColors;
    }
    
    void FCScript::setSystemColors(ArrayList<Long> systemColors){
        m_systemColors = systemColors;
    }
    
    Object FCScript::getTag(){
        return m_tag;
    }
    
    void FCScript::setTag(Object tag){
        m_tag = tag;
    }
    
    CVarFactory* FCScript::getVarFactory(){
        return m_varFactory;
    }
    
    void FCScript::setVarFactory(CVarFactory *varFactory){
        if(m_varFactory){
            delete m_varFactory;
            m_varFactory = 0;
        }
        m_varFactory = varFactory;
    }
    
    
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    ////
    
    void FCScript::addFunction(CFunction *function){
        m_functions.put(function->m_name, function);
        m_functionsMap.put(function->m_ID, function);
    }
    
    double FCScript::callFunction(String funcName){
        double result = 0;
        lock();
        ArrayList<String> lines;
        getMiddleScript(funcName, &lines);
        int linesSize = (int)lines.size();
        m_result = 0;
        for (int i = 0; i < linesSize; i++){
            String str = lines.get(i);
            int cindex = (int)str.find(L"(");
            String upperName = FCStr::toUpper(str.substr(0, cindex));
            if (m_tempFunctions.containsKey(upperName)){
                CVariable *function = m_tempFunctions.get(upperName);
                int rindex = (int)str.rfind(L")");
                CVariable *topVar = new CVariable();
                topVar->m_indicator = this;
                if (rindex - cindex > 1){
                    String pStr = str.substr(cindex + 1, rindex - cindex - 1);
                    ArrayList<String> pList = FCStr::split(pStr, VARIABLE2);
                    ArrayList<String> fieldTexts = FCStr::split(function->m_fieldText, VARIABLE2);
                    int pListLen = (int)pList.size();
                    topVar->m_parameters = new CVariable*[pListLen * 2];
                    for (int j = 0; j < pListLen; j++){
                        String pName = fieldTexts.get(j);
                        String pValue = pList.get(j);
                        CVariable *varName = 0;
                        if (m_tempVariables.containsKey(pName)){
                            varName = m_tempVariables.get(pName);
                        }
                        CVariable *varValue = new CVariable();
                        varValue->m_indicator = this;
                        varValue->m_expression = pValue;
                        if (pValue[0] == '\''){
                            varValue->m_type = 1;
                        }
                        else{
                            varValue->m_type = 3;
                            varValue->m_value = FCStr::convertStrToDouble(pValue);
                        }
                        topVar->m_parameters[j * 2] = varName;
                        topVar->m_parameters[j * 2 + 1] = varValue;
                    }
                    pList.clear();
                    fieldTexts.clear();
                    topVar->m_parametersLength = pListLen * 2;
                    FUNCVAR(topVar);
                }
                getValue(function);
                if (topVar->m_parameters){
                    int variablesSize = topVar->m_parametersLength;
                    for (int j = 0; j < variablesSize; j++){
                        if (j % 2 == 0){
                            int id = topVar->m_parameters[j]->m_field;
                            if (m_tempVars.containsKey(id)){
                                CVar *cVar = m_tempVars.get(id);
                                if (cVar->m_parent){
                                    m_tempVars.put(id, cVar->m_parent);
                                }
                                else{
                                    m_tempVars.remove(id);
                                }
                                delete cVar;
                                cVar = 0;
                            }
                        }
                    }
                }
                delete topVar;
                topVar = 0;
            }
        }
        lines.clear();
        result = m_result;
        m_result = 0;
        m_break = 0;
        unLock();
        return result;
    }
    
    void FCScript::clear(){
        lock();
        if (m_div){
            ArrayList<BaseShape*> shapes = getShapes();
            for (int b = 0; b < shapes.size(); b++){
                BaseShape *shape = shapes.get(b);
                m_div->removeShape(shape);
                ArrayList<ChartTitle*> titles = m_div->getTitleBar()->Titles;
                for (int t = 0; t < titles.size(); t++){
                    delete titles.get(t);
                }
                m_div->getTitleBar()->Titles.clear();
                delete shape;
            }
            shapes.clear();
        }
        m_lines.clear();
        for (int v = 0; v < m_variables.size(); v++){
            CVariable *var = m_variables.get(v);
            if (var->m_field >= 10000){
                m_dataSource->removeColumn(var->m_field);
            }
            if (var->m_tempFields){
                for (int i = 0; i < var->m_tempFieldsLength; i++){
                    if (var->m_tempFields[i] >= 10000){
                        m_dataSource->removeColumn(var->m_tempFields[i]);
                    }
                }
            }
            delete var;
        }
        m_variables.clear();
        m_mainVariables.clear();
        m_defineParams.clear();
        m_tempFunctions.clear();
        deleteTempVars();
        m_tempVariables.clear();
        unLock();
    }
    
    ArrayList<CFunction*> FCScript::getFunctions(){
        ArrayList<CFunction*> functions;
        for(int f = 0; f < m_functions.size(); f++){
            functions.add(m_functions.getValue(f));
        }
        return functions;
    }
    
    ArrayList<BaseShape*> FCScript::getShapes(){
        ArrayList<BaseShape*> shapes;
        for (int v = 0; v < m_variables.size(); v++){
            CVariable *var = m_variables.get(v);
            if (var->m_barShape){
                shapes.add(var->m_barShape);
            }
            if (var->m_candleShape){
                shapes.add(var->m_candleShape);
            }
            if (var->m_polylineShape){
                shapes.add(var->m_polylineShape);
            }
            if (var->m_textShape){
                shapes.add(var->m_textShape);
            }
        }
        return shapes;
    }
    
    String FCScript::getText(CVariable *var){
        if ((int)var->m_expression.length() > 0 && var->m_expression[0] == '\''){
            return var->m_expression.substr(1, (int)var->m_expression.length() - 2);
        }
        else{
            if(m_tempVars.containsKey(var->m_field)){
                CVar *cVar = m_tempVars.get(var->m_field);
                if (cVar->m_type == 1){
                    return cVar->getText(this, var);
                }
                else{
                    return FCStr::convertDoubleToStr(cVar->m_num);
                }
            }
            else{
                return FCStr::convertDoubleToStr(getValue(var));
            }
        }
    }
    
    double FCScript::getValue(CVariable *var){
        switch (var->m_type){
            case 0:
                return callFunction(var);
            case 1:{
                if(m_tempVars.containsKey(var->m_field)){
                    CVar *cVar = m_tempVars.get(var->m_field);
                    return cVar->getValue(this, var);
                    
                }
                else{
                    if ((int)var->m_expression.length() > 0 && var->m_expression[0] == '\''){
                        return FCStr::convertStrToDouble(var->m_expression.substr(1, (int)var->m_expression.length() - 2));
                    }
                    else{
                        if(var->m_splitExpressionLength > 0){
                            return calculate(var->m_splitExpression, var->m_splitExpressionLength);
                        }
                        else{
                            return 0;
                        }
                    }
                }
            }
            case 2:
                return m_dataSource->get3(m_index, var->m_fieldIndex);
            case 3:
                return var->m_value;
            default:
                return 0;
        }
    }
    
    CVariable* FCScript::getVariable(const String& name){
        if (m_tempVariables.containsKey(name)){
            return m_tempVariables.get(name);
        }
        else{
            return 0;
        }
    }
    
    void FCScript::lock(){
        pthread_mutex_lock(&mutex_x);
    }
    
    void FCScript::onCalculate(int index){
        lock();
        if (m_lines.size() > 0){
            for (int l = 0; l < m_lines.size(); l++){
                CVariable *sentence = m_lines.get(l);
                if(sentence->m_field != FCDataTable::NULLFIELD()){
                    sentence->m_fieldIndex = m_dataSource->getColumnIndex(sentence->m_field);
                }
            }
            for (int l = 0; l < m_lines.size(); l++){
                CVariable *var = m_lines.get(l);
                if(var->m_field != FCDataTable::NULLFIELD()){
                    var->m_fieldIndex = m_dataSource->getColumnIndex(var->m_field);
                }
                if(var->m_tempFields){
                    for(int i = 0; i < var->m_tempFieldsLength; i++){
                        var->m_tempFieldsIndex[i] = m_dataSource->getColumnIndex(var->m_tempFields[i]);
                    }
                }
            }
            int linesSize = (int)m_lines.size();
            for (int i = index; i < m_dataSource->rowsCount(); i++){
                m_break = 0;
                m_index = i;
                deleteTempVars();
                for(int j = 0; j < linesSize; j++){
                    CVariable *sentence = m_lines.get(j);
                    int funcLen = (int)sentence->m_funcName.length();
                    if (funcLen == 0 || (funcLen != 0 && sentence->m_line != j)){
                        double value = calculate(sentence->m_splitExpression, sentence->m_splitExpressionLength);
                        if(sentence->m_type == 1 && sentence->m_field != FCDataTable::NULLFIELD()){
                            m_dataSource->set3(i, sentence->m_fieldIndex, value);
                        }
                    }
                    if(m_break == 1){
                        break;
                    }
                }
            }
        }
        unLock();
    }
    
    
    void FCScript::removeFunction(CFunction *function){
        if(m_functions.containsKey(function->m_name)){
            m_functions.remove(function->m_name);
        }
        if(m_functionsMap.containsKey(function->m_ID)){
            m_functionsMap.remove(function->m_ID);
        }
    }
    
    void FCScript::setSourceField(const String& key, int value){
        CVariable *pfunc = new CVariable();
        pfunc->m_indicator = this;
        pfunc->m_type = 2;
        wchar_t szTemp[20] ={0};
        swprintf(szTemp, 19, L"%d", m_variables.size());
        pfunc->m_name = VARIABLE + szTemp;
        pfunc->m_expression = key;
        int splitExpressionLength = 0;
        pfunc->m_splitExpression = splitExpression(key, &splitExpressionLength);
        pfunc->m_splitExpressionLength = splitExpressionLength;
        pfunc->m_field = value;
        int columnIndex = m_dataSource->getColumnIndex(value);
        if (columnIndex == -1){
            m_dataSource->addColumn(value);
        }
        m_variables.add(pfunc);
    }
    
    void FCScript::setSourceValue(int index, const String& key, double value){
        CVariable *pfunc = 0;
        for(int v = 0; v < m_variables.size(); v++){
            CVariable *var = m_variables.get(v);
            if (var->m_type == 2 && var->m_expression == key){
                pfunc = var;
                break;
            }
        }
        if(pfunc){
            m_dataSource->set2(index, pfunc->m_field, value);
        }
    }
    
    void FCScript::setVariable(CVariable *variable, CVariable *parameter){
        int type = variable->m_type;
        int id = variable->m_field;
        switch (type){
            case 2:{
                double value = getValue(parameter);
                m_dataSource->set3(m_index, variable->m_fieldIndex, value);
                break;
            }
            default:{
                if(m_tempVars.containsKey(variable->m_field)){
                    CVar *cVar = m_tempVars.get(variable->m_field);
                    cVar->setValue(this, variable, parameter);
                    if (m_resultVar.m_type != -1){
                        cVar->m_str = m_resultVar.m_str;
                        m_resultVar.m_type = -1;
                    }
                }
                else{
                    variable->m_value = getValue(parameter);
                }
            }
        }
    }
    
    void FCScript::unLock(){
        pthread_mutex_unlock(&mutex_x);
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    
    double FCScript::ABS2(CVariable *var){
        return abs(getValue(var->m_parameters[0]));
    }
    
    double FCScript::AMA(CVariable *var){
        double close = getValue(var->m_parameters[0]);
        double lastAma = 0;
        if (m_index > 0){
            lastAma = m_dataSource->get3(m_index - 1, var->m_fieldIndex);
        }
        double n = getValue(var->m_parameters[1]);
        double ama = lastAma + n * (close - lastAma);
        m_dataSource->set3(m_index, var->m_fieldIndex, ama);
        return ama;
    }
    
    double FCScript::ACOS(CVariable *var){
        return acos(getValue(var->m_parameters[0]));
    }
    
    double FCScript::ASIN(CVariable *var){
        return asin(getValue(var->m_parameters[0]));
    }
    
    double FCScript::ATAN(CVariable *var){
        return atan(getValue(var->m_parameters[0]));
    }
    
    double m002(double value, double* listForAvedev, int listForAvedev_length, double avg){
        int i = 0;
        if(listForAvedev_length > 0){
            double sum = fabs(value - avg);
            for(i = 0;i < listForAvedev_length; i++){
                sum += fabs(listForAvedev[i]-avg);
            }
            return sum / listForAvedev_length;
        }
        else{
            return 0;
        }
    }
    
    double FCScript::AVEDEV(CVariable *var){
        int p = (int)getValue(var->m_parameters[1]);
        CVariable *cParam = var->m_parameters[0];
        int closeFieldIndex = cParam->m_fieldIndex;
        int closeField = cParam->m_field;
        double close = getValue(cParam);
        if (closeFieldIndex == -1){
            if (!var->m_tempFields){
                var->createTempFields(1);
            }
            closeFieldIndex = var->m_tempFieldsIndex[0];
            closeField = var->m_tempFields[0];
            m_dataSource->set3(m_index, closeFieldIndex, close);
        }
        int listLength = 0;
        double *list = m_dataSource->DATA_ARRAY(closeField, m_index, p, &listLength);
        double avg = 0;
        if (listLength > 0){
            double sum = 0;
            for (int i = 0; i < listLength; i++){
                sum += list[i];
            }
            avg = sum / listLength;
        }
        double value = ::m002(close, list, listLength, avg);
        if(list){
            delete[] list;
            list = 0;
        }
        return value;
    }
    
    int FCScript::BARSCOUNT(CVariable *var){
        return m_dataSource->rowsCount();
    }
    
    int FCScript::BARSLAST(CVariable *var){
        int result = 0;
        int tempIndex = m_index;
        for (int i = m_index; i >= 0; i--){
            m_index = i;
            double value = getValue(var->m_parameters[0]);
            if (value == 1){
                break;
            }
            else{
                if (i == 0){
                    result = 0;
                }
                else{
                    result++;
                }
            }
        }
        m_index = tempIndex;
        return result;
    }
    
    int FCScript::BETWEEN(CVariable *var){
        double value = getValue(var->m_parameters[0]);
        double min = getValue(var->m_parameters[1]);
        double max = getValue(var->m_parameters[2]);
        int result = 0;
        if (value >= min && value <= max){
            result = 1;
        }
        return result;
    }
    
    int FCScript::BREAK(CVariable *var){
        m_break = 2;
        return 0;
    }
    
    double FCScript::CEILING(CVariable *var){
        return ceil(getValue(var->m_parameters[0]));
    }
    
    double FCScript::CHUNK(CVariable *var){
        int pLen = var->m_parametersLength;
        if (pLen > 0){
            for (int i = 0; m_break == 0 && i < pLen; i++){
                getValue(var->m_parameters[i]);
            }
        }
        deleteTempVars(var);
        return 0;
    }
    
    int FCScript::CONTINUE(CVariable *var){
        m_break = 3;
        return 0;
    }
    
    
    double FCScript::COS(CVariable *var){
        return cos(getValue(var->m_parameters[0]));
    }
    
    int FCScript::COUNT(CVariable *var){
        int n = (int)getValue(var->m_parameters[1]);
        if (n < 0){
            n = m_dataSource->rowsCount();
        }
        else if (n > m_index + 1){
            n = m_index + 1;
        }
        int tempIndex = m_index;
        int result = 0;
        for (int i = 0; i < n; i++){
            if (getValue(var->m_parameters[0]) > 0){
                result++;
            }
            m_index--;
        }
        m_index = tempIndex;
        return result;
    }
    
    int FCScript::CROSS(CVariable *var){
        double x = getValue(var->m_parameters[0]);
        double y = getValue(var->m_parameters[1]);
        int result = 0;
        int tempIndex = m_index;
        m_index -= 1;
        if (m_index < 0){
            m_index = 0;
        }
        double lastX = getValue(var->m_parameters[0]);
        double lastY = getValue(var->m_parameters[1]);
        m_index = tempIndex;
        if (x >= y && lastX < lastY){
            result = 1;
        }
        return result;
    }
    
    int FCScript::CURRBARSCOUNT(CVariable *var){
        return m_index + 1;
    }
    
    int FCScript::DATE(CVariable *var){
        int year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0, msecond = 0;
        FCStr::getDateByNum(m_dataSource->getXValue(m_index), &year, &month, &day, &hour, &minute, &second, &msecond);
        return year * 10000 + month * 100 + day;
    }
    
    int FCScript::DAY(CVariable *var){
        int year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0, msecond = 0;
        FCStr::getDateByNum(m_dataSource->getXValue(m_index), &year, &month, &day, &hour, &minute, &second, &msecond);
        return day;
    }
    
    int FCScript::DELETE2(CVariable *var){
        CVariable *name = var->m_parameters[0];
        int id = name->m_field;
        if(m_tempVars.containsKey(id)){
            CVar *cVar = m_tempVars.get(id);
            if (cVar->m_parent){
                m_tempVars.put(id, cVar->m_parent);
            }
            else{
                m_tempVars.remove(id);
            }
            delete cVar;
        }
        return 0;
    }
    
    
    double FCScript::DMA(CVariable *var){
        double close = getValue(var->m_parameters[0]);
        double lastDma = 0;
        if (m_index > 0){
            lastDma = m_dataSource->get3(m_index - 1, var->m_fieldIndex);
        }
        double n = getValue(var->m_parameters[1]);
        double result = n * close + (1 - n) * lastDma;
        m_dataSource->set3(m_index, var->m_fieldIndex, result);
        return result;
    }
    
    int FCScript::DOTIMES(CVariable *var){
        int n = (int)getValue(var->m_parameters[0]);
        int pLen = var->m_parametersLength;
        if (pLen > 1){
            for (int i = 0; i < n; i++){
                for (int j = 1; m_break == 0 && j < pLen; j++){
                    getValue(var->m_parameters[j]);
                }
                if (m_break > 0){
                    if (m_break == 3){
                        m_break = 0;
                        deleteTempVars(var);
                        continue;
                    }
                    else{
                        m_break = 0;
                        deleteTempVars(var);
                        break;
                    }
                }
                else{
                    deleteTempVars(var);
                }
            }
        }
        return 0;
    }
    
    
    int FCScript::DOWHILE(CVariable *var){
        int pLen = var->m_parametersLength;
        if (pLen > 1){
            while (true){
                for (int i = 0; m_break == 0 && i < pLen - 1; i++){
                    getValue(var->m_parameters[i]);
                }
                if (m_break > 0){
                    if (m_break == 3){
                        m_break = 0;
                        deleteTempVars(var);
                        continue;
                    }
                    else{
                        m_break = 0;
                        deleteTempVars(var);
                        break;
                    }
                }
                double inLoop = getValue(var->m_parameters[pLen - 1]);
                deleteTempVars(var);
                if (inLoop <= 0){
                    break;
                }
            }
        }
        return 0;
    }
    
    int FCScript::DOWNNDAY(CVariable *var){
        int n = (int)getValue(var->m_parameters[0]);
        if (n < 0){
            n = m_dataSource->rowsCount();
        }
        else if (n > m_index + 1){
            n = m_index + 1;
        }
        int tempIndex = m_index;
        int result = 1;
        for (int i = 0; i < n; i++){
            double right = getValue(var->m_parameters[0]);
            m_index--;
            double left = m_index >= 0 ? getValue(var->m_parameters[0]) : 0;
            if (right >= left){
                result = 0;
                break;
            }
        }
        m_index = tempIndex;
        return result;
    }
    
    double FCScript::DRAWICON(CVariable *var){
        if (m_div){
            CVariable *cond = var->m_parameters[0];
            CVariable *price = var->m_parameters[1];
            PolylineShape *polylineShape = 0;
            if (!var->m_polylineShape){
                String strColor = L"FCColorAUTO";
                String strStyle = L"CIRCLEDOT";
                for (int i = 2; i < var->m_parametersLength; i++){
                    String strParam = var->m_parameters[i]->m_expression;
                    if (strParam.length() > 5 && strParam.substr(0, 5) == L"FCColor"){
                        strColor = strParam;
                        break;
                    }
                    else if (strParam == L"CIRCLEDOT"){
                        strStyle = strParam;
                        break;
                    }
                    else if (strParam == L"FCPointDOT"){
                        strStyle = strParam;
                        break;
                    }
                }
                if (var->m_expression == L"DRAWICON"){
                    strStyle = var->m_expression;
                }
                polylineShape = new PolylineShape();
                m_div->addShape(polylineShape);
                Long lineColor = getColor(strColor);
                polylineShape->setAttachVScale(m_attachVScale);
                polylineShape->setFieldText(price->m_fieldText);
                polylineShape->setColor(lineColor);
                var->createTempFields(1);
                if(strStyle == L"CIRCLEDOT"){
                    polylineShape->setStyle(PolylineStyle_Cycle);
                }
                var->m_polylineShape = polylineShape;
            }
            else{
                polylineShape = var->m_polylineShape;
            }
            if (price->m_expression.length() > 0){
                if (polylineShape->getFieldName() == FCDataTable::NULLFIELD()){
                    if (price->m_field != FCDataTable::NULLFIELD()){
                        polylineShape->setFieldName(price->m_field);
                    }
                    else{
                        price->createTempFields(1);
                        polylineShape->setFieldName(price->m_tempFields[0]);
                    }
                    for (int i = 2; i < var->m_parametersLength; i++){
                        String strParam = var->m_parameters[i]->m_expression;
                        if (strParam == L"DRAWTITLE"){
                            if (polylineShape->getFieldText().length() != 0){
                                m_div->getTitleBar()->Titles.add(new ChartTitle(polylineShape->getFieldName(), polylineShape->getFieldText(),
                                                                                  
                                                                                  polylineShape->getColor(), 2, true));
                            }
                        }
                    }
                }
                if (!price->m_tempFields){
                    double value = getValue(price);
                    m_dataSource->set3(m_index, price->m_tempFieldsIndex[0], value);
                }
            }
            double dCond = 1;
            if (cond->m_expression.length() > 0 && cond->m_expression != L"1"){
                dCond = getValue(cond);
                if (dCond != 1){
                    m_dataSource->set3(m_index, var->m_tempFieldsIndex[0], -10000);
                }
                else{
                    m_dataSource->set3(m_index, var->m_tempFieldsIndex[0], 1);
                }
            }
        }
        return 0;
    }
    
    double FCScript::DRAWKLINE(CVariable *var){
        if(m_div){
            CVariable *high = var->m_parameters[0];
            CVariable *open = var->m_parameters[1];
            CVariable *low = var->m_parameters[2];
            CVariable *close = var->m_parameters[3];
            CandleShape *candleShape = 0;
            if (!var->m_candleShape){
                candleShape = new CandleShape();
                candleShape->setHighFieldText(high->m_fieldText);
                candleShape->setOpenFieldText(open->m_fieldText);
                candleShape->setLowFieldText(low->m_fieldText);
                candleShape->setCloseFieldText(close->m_fieldText);
                candleShape->setAttachVScale(m_attachVScale);
                candleShape->setStyle(CandleStyle_Rect);
                m_div->addShape(candleShape);
                var->m_candleShape = candleShape;
            }
            else{
                candleShape = var->m_candleShape;
            }
            if (high->m_expression.length() > 0){
                if (candleShape->getHighField() == FCDataTable::NULLFIELD()){
                    if (high->m_field != FCDataTable::NULLFIELD()){
                        candleShape->setHighField(high->m_field);
                    }
                    else{
                        high->createTempFields(1);
                        candleShape->setHighField(high->m_tempFields[0]);
                    }
                }
                if (high->m_tempFields){
                    double value = getValue(high);
                    m_dataSource->set3(m_index, high->m_tempFieldsIndex[0], value);
                }
            }
            if (low->m_expression.length() > 0){
                if (candleShape->getLowField() == FCDataTable::NULLFIELD()){
                    if (low->m_field != FCDataTable::NULLFIELD()){
                        candleShape->setLowField(low->m_field);
                    }
                    else{
                        low->createTempFields(1);
                        candleShape->setLowField(low->m_tempFields[0]);
                    }
                }
                if (low->m_tempFields){
                    double value = getValue(low);
                    m_dataSource->set3(m_index, low->m_tempFieldsIndex[0], value);
                }
            }
            if (open->m_expression.length() > 0){
                if (candleShape->getOpenField() == FCDataTable::NULLFIELD()){
                    if (open->m_field != FCDataTable::NULLFIELD()){
                        candleShape->setOpenField(open->m_field);
                    }
                    else{
                        open->createTempFields(1);
                        candleShape->setOpenField(open->m_tempFields[0]);
                    }
                }
                if (open->m_tempFields){
                    double value = getValue(open);
                    m_dataSource->set3(m_index, open->m_tempFieldsIndex[0], value);
                }
            }
            if (close->m_expression.length() > 0){
                if (candleShape->getCloseField() == FCDataTable::NULLFIELD()){
                    if (close->m_field != FCDataTable::NULLFIELD()){
                        candleShape->setCloseField(close->m_field);
                    }
                    else{
                        close->createTempFields(1);
                        candleShape->setCloseField(close->m_tempFields[0]);
                    }
                }
                if (close->m_tempFields){
                    double value = getValue(close);
                    m_dataSource->set3(m_index, close->m_tempFieldsIndex[0], value);
                }
            }
        }
        return 0;
    }
    
    double FCScript::DRAWNULL(CVariable *var){
        return m_dataSource->NaN;
    }
    
    double FCScript::DRAWTEXT(CVariable *var){
        if (m_div){
            CVariable *cond = var->m_parameters[0];
            CVariable *price = var->m_parameters[1];
            CVariable *text = var->m_parameters[2];
            TextShape *textShape = 0;
            if (!var->m_textShape){
                textShape = new TextShape();
                textShape->setAttachVScale(m_attachVScale);
                textShape->setText(getText(text));
                var->createTempFields(1);
                textShape->setStyleField(var->m_tempFields[0]);
                String strColor = L"COLORAUTO";
                for (int i = 3; i < var->m_parametersLength; i++){
                    String strParam = var->m_parameters[i]->m_expression;
                    if (strParam.length() > 5 && strParam.substr(0, 5) == L"COLOR"){
                        strColor = strParam;
                        break;
                    }
                }
                if (strColor != L"COLORAUTO"){
                    textShape->setTextColor(getColor(strColor));
                }
                m_div->addShape(textShape);
                var->m_textShape = textShape;
            }
            else{
                textShape = var->m_textShape;
            }
            if (price->m_expression.length() > 0){
                if (textShape->getFieldName() == FCDataTable::NULLFIELD()){
                    if (price->m_field != FCDataTable::NULLFIELD()){
                        textShape->setFieldName(price->m_field);
                    }
                    else{
                        price->createTempFields(1);
                        textShape->setFieldName(price->m_tempFields[0]);
                    }
                }
                if (price->m_tempFields){
                    double value = getValue(price);
                    m_dataSource->set3(m_index, price->m_tempFieldsIndex[0], value);
                }
            }
            double dCond = 1;
            if (cond->m_expression.length() > 0 && cond->m_expression != L"1"){
                dCond = getValue(cond);
                if (dCond != 1){
                    m_dataSource->set3(m_index, var->m_tempFieldsIndex[0], -10000);
                }
                else{
                    m_dataSource->set3(m_index, var->m_tempFieldsIndex[0], 0);
                }
            }
        }
        return 0;
    }
    
    int FCScript::EXIST(CVariable *var){
        int n = (int)getValue(var->m_parameters[1]);
        if (n < 0){
            n = m_dataSource->rowsCount();
        }
        else if (n > m_index + 1){
            n = m_index + 1;
        }
        int tempIndex = m_index;
        int result = 0;
        for (int i = 0; i < n; i++){
            if (getValue(var->m_parameters[0]) > 0){
                result = 1;
                break;
            }
            m_index--;
        }
        m_index = tempIndex;
        return result;
    }
    
    double FCScript::EMA(CVariable *var){
        double close = getValue(var->m_parameters[0]);
        double lastEma = 0;
        if (m_index > 0){
            lastEma = m_dataSource->get3(m_index - 1, var->m_fieldIndex);
        }
        int n = (int)getValue(var->m_parameters[1]);
        double result = exponentialMovingAverage(n, close, lastEma);
        m_dataSource->set3(m_index, var->m_fieldIndex, result);
        return result;
    }
    
    int FCScript::EVERY(CVariable *var){
        int n = (int)getValue(var->m_parameters[1]);
        if (n < 0){
            n = m_dataSource->rowsCount();
        }
        else if (n > m_index + 1){
            n = m_index + 1;
        }
        int tempIndex = m_index;
        int result = 1;
        for (int i = 0; i < n; i++){
            if (getValue(var->m_parameters[0]) <= 0){
                result = 0;
                break;
            }
            m_index--;
        }
        m_index = tempIndex;
        return result;
    }
    
    double FCScript::EXPMEMA(CVariable *var){
        CVariable *cParam = var->m_parameters[0];
        double close = getValue(cParam);
        int closeFieldIndex = cParam->m_fieldIndex;
        int n = (int)getValue(var->m_parameters[1]);
        if (!var->m_tempFields){
            if (closeFieldIndex == -1){
                var->createTempFields(2);
            }
            else{
                var->createTempFields(1);
            }
        }
        if (var->m_tempFieldsLength == 2){
            closeFieldIndex = var->m_tempFieldsIndex[1];
            m_dataSource->set3(m_index, closeFieldIndex, close);
        }
        int maFieldIndex = var->m_tempFieldsIndex[0];
        double ma = movingAverage(m_index, n, close, getDatas(closeFieldIndex, maFieldIndex, m_index - 1, n));
        m_dataSource->set3(m_index, maFieldIndex, ma);
        double lastEma = 0;
        if (m_index > 0){
            lastEma = m_dataSource->get3(m_index, var->m_fieldIndex);
        }
        double result = exponentialMovingAverage(n, ma, lastEma);
        m_dataSource->set3(m_index, var->m_fieldIndex, result);
        return result;
    }
    
    double FCScript::EXP(CVariable *var){
        return exp(getValue(var->m_parameters[0]));
    }
    
    double FCScript::FLOOR(CVariable *var){
        return floor(getValue(var->m_parameters[0]));
    }
    
    int FCScript::FOR(CVariable *var){
        int pLen = var->m_parametersLength;
        if (pLen > 3){
            int start = (int)getValue(var->m_parameters[0]);
            int end = (int)getValue(var->m_parameters[1]);
            int step = (int)getValue(var->m_parameters[2]);
            if (step > 0){
                for (int i = start; i < end; i += step){
                    for (int j = 3; j < pLen; j++){
                        getValue(var->m_parameters[j]);
                        if (m_break != 0){
                            break;
                        }
                    }
                    if (m_break > 0){
                        if (m_break == 3){
                            m_break = 0;
                            deleteTempVars(var);
                            continue;
                        }
                        else{
                            m_break = 0;
                            deleteTempVars(var);
                            break;
                        }
                    }
                    else{
                        deleteTempVars(var);
                    }
                }
            }
            else if (step < 0){
                for (int i = start; i > end; i += step){
                    for (int j = 3; j < pLen; j++){
                        if (m_break != 0){
                            break;
                        }
                    }
                    if (m_break > 0){
                        if (m_break == 3){
                            m_break = 0;
                            deleteTempVars(var);
                            continue;
                        }
                        else{
                            m_break = 0;
                            deleteTempVars(var);
                            break;
                        }
                    }
                    else{
                        deleteTempVars(var);
                    }
                }
            }
        }
        return 0;
    }
    
    
    double FCScript::FUNCTION(CVariable *var){
        m_result = 0;
        if (var->m_parameters != 0){
            int pLen = var->m_parametersLength;
            if (pLen > 0){
                for (int i = 0; i < pLen; i++){
                    getValue(var->m_parameters[i]);
                }
            }
        }
        String name = var->m_expression;
        if (m_tempFunctions.containsKey(name)){
            getValue(m_tempFunctions.get(name));
        }
        if (m_break == 1){
            m_break = 0;
        }
        double result = m_result;
        m_result = 0;
        deleteTempVars(var);
        return result;
    }
    
    
    double FCScript::FUNCVAR(CVariable *var){
        double result = 0;
        int pLen = var->m_parametersLength;
        map<CVar*, int> cVars;
        for(int i = 0; i < pLen; i++){
            if (i % 2 == 0){
                CVariable *name = var->m_parameters[i];
                CVariable *value = var->m_parameters[i + 1];
                int id = name->m_field;
                if (name->m_expression.find(L"[REF]") == 0){
                    int variablesSize = m_variables.size();
                    for (int j = 0; j < variablesSize; j++){
                        CVariable *variable = m_variables.get(j);
                        if (variable != name){
                            if (variable->m_field == id){
                                variable->m_field = value->m_field;
                            }
                        }
                    }
                    continue;
                }
                else{
                    CVar *newCVar = m_varFactory->createVar();
                    result = newCVar->onCreate(this, name, value);
                    if(newCVar->m_type == 1){
                        name->m_functionID = -2;
                    }
                    cVars[newCVar] = id;
                }
            }
        }
        map<CVar*, int>::iterator sIter = cVars.begin();
        for(; sIter != cVars.end(); ++sIter){
            int id = sIter->second;
            CVar *newCVar = sIter->first;
            if (m_tempVars.containsKey(id)){
                CVar *cVar = m_tempVars.get(id);
                newCVar->m_parent = cVar;
            }
            m_tempVars.put(id, newCVar);
        }
        return result;
    }
    
    double FCScript::GET(CVariable *var){
        return getValue(var->m_parameters[0]);
    }
    
    double FCScript::HHV(CVariable *var){
        int n = (int)getValue(var->m_parameters[1]);
        CVariable *cParam = var->m_parameters[0];
        int closeFieldIndex = cParam->m_fieldIndex;
        int closeField = cParam->m_field;
        if (closeFieldIndex == -1){
            if (!cParam->m_tempFields){
                cParam->createTempFields(0);
            }
            closeFieldIndex = cParam->m_tempFieldsIndex[0];
            closeField = cParam->m_tempFields[0];
            double close = getValue(cParam);
            m_dataSource->set3(m_index, closeFieldIndex, close);
        }
        int higharrayLength = 0;
        double *higharray = m_dataSource->DATA_ARRAY(closeField, m_index, n, &higharrayLength);
        double result = maxValue(higharray, higharrayLength);
        if(higharray){
            delete[] higharray;
            higharray = 0;
        }
        return result;
    }
    
    double FCScript::HHVBARS(CVariable *var){
        int n = (int)getValue(var->m_parameters[1]);
        CVariable *cParam = var->m_parameters[0];
        int closeField = cParam->m_field;
        int closeFieldIndex = cParam->m_fieldIndex;
        if (closeFieldIndex == -1){
            if (!cParam->m_tempFields){
                cParam->createTempFields(0);
            }
            closeField = cParam->m_tempFields[0];
            closeFieldIndex = cParam->m_tempFieldsIndex[0];
            double close = getValue(cParam);
            m_dataSource->set3(m_index, closeFieldIndex, close);
        }
        int higharrayLength = 0;
        double *higharray = m_dataSource->DATA_ARRAY(closeField, m_index, n, &higharrayLength);
        double result = 0;
        if (higharrayLength > 0){
            int mIndex = 0;
            double close = 0;
            for (int i = 0; i < higharrayLength; i++){
                if (i == 0){
                    close = higharray[i];
                    mIndex = 0;
                }
                else{
                    if (higharray[i] > close){
                        close = higharray[i];
                        mIndex = i;
                    }
                }
            }
            result = higharrayLength - mIndex - 1;
        }
        if(higharray){
            delete[] higharray;
            higharray = 0;
        }
        return result;
    }
    
    int FCScript::HOUR(CVariable *var){
        int year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0, msecond = 0;
        FCStr::getDateByNum(m_dataSource->getXValue(m_index), &year, &month, &day, &hour, &minute, &second, &msecond);
        return hour;
    }
    
    double FCScript::IF(CVariable *var){
        double result = 0;
        int pLen = var->m_parametersLength;
        for (int i = 0; i < pLen; i++){
            result = getValue(var->m_parameters[i]);
            if (i % 2 == 0){
                if (result == 0){
                    i++;
                    continue;
                }
            }
            else{
                break;
            }
        }
        deleteTempVars(var);
        return result;
    }
    
    double FCScript::IFN(CVariable *var){
        double result = 0;
        int pLen = var->m_parametersLength;
        for (int i = 0; i < pLen; i++){
            result = getValue(var->m_parameters[i]);
            if (i % 2 == 0){
                if (result != 0){
                    i++;
                    continue;
                }
            }
            else{
                break;
            }
        }
        deleteTempVars(var);
        return result;
    }
    
    double FCScript::INTPART(CVariable *var){
        double result = getValue(var->m_parameters[0]);
        if (result != 0){
            int intResult = (int)result;
            double sub = abs(result - intResult);
            if (sub >= 0.5){
                if (result > 0){
                    result = intResult - 1;
                }
                else{
                    result = intResult + 1;
                }
            }
            else{
                result = intResult;
            }
        }
        return result;
    }
    
    int FCScript::LAST(CVariable *var){
        int n = (int)getValue(var->m_parameters[1]);
        int m = (int)getValue(var->m_parameters[2]);
        if (n < 0){
            n = m_dataSource->rowsCount();
        }
        else if (n > m_index + 1){
            n = m_index + 1;
        }
        if (m < 0){
            m = m_dataSource->rowsCount();
        }
        else if (m > m_index + 1){
            m = m_index + 1;
        }
        int tempIndex = m_index;
        int result = 1;
        for (int i = m; i < n; i++){
            m_index = tempIndex - m;
            if (getValue(var->m_parameters[0]) <= 0){
                result = 0;
                break;
            }
        }
        m_index = tempIndex;
        return result;
    }
    
    double FCScript::LLV(CVariable *var){
        int n = (int)getValue(var->m_parameters[1]);
        CVariable *cParam = var->m_parameters[0];
        int closeField = cParam->m_field;
        int closeFieldIndex = cParam->m_fieldIndex;
        if (closeField == FCDataTable::NULLFIELD()){
            if (!cParam->m_tempFields){
                cParam->createTempFields(0);
            }
            closeField = cParam->m_tempFields[0];
            closeFieldIndex = cParam->m_tempFieldsIndex[0];
            double close = getValue(cParam);
            m_dataSource->set3(m_index, closeFieldIndex, close);
        }
        int lowarrayLength = 0;
        double *lowarray = m_dataSource->DATA_ARRAY(closeField, m_index, n, &lowarrayLength);
        double result = minValue(lowarray, lowarrayLength);
        if(lowarray){
            delete[] lowarray;
            lowarray = 0;
        }
        return result;
    }
    
    double FCScript::LLVBARS(CVariable *var){
        int n = (int)getValue(var->m_parameters[1]);
        CVariable *cParam = var->m_parameters[0];
        int closeField = cParam->m_field;
        int closeFieldIndex = cParam->m_fieldIndex;
        if (closeField == FCDataTable::NULLFIELD()){
            if (!cParam->m_tempFields){
                cParam->createTempFields(0);
            }
            closeField = cParam->m_tempFields[0];
            closeFieldIndex = cParam->m_tempFieldsIndex[0];
            double close = getValue(cParam);
            m_dataSource->set3(m_index, closeFieldIndex, close);
        }
        int lowarrayLength = 0;
        double *lowarray = m_dataSource->DATA_ARRAY(closeField, m_index, n, &lowarrayLength);
        double result = 0;
        if (lowarrayLength > 0){
            int mIndex = 0;
            double close = 0;
            for (int i = 0; i < lowarrayLength; i++){
                if (i == 0){
                    close = lowarray[i];
                    mIndex = 0;
                }
                else{
                    if (lowarray[i] < close){
                        close = lowarray[i];
                        mIndex = i;
                    }
                }
            }
            result = lowarrayLength - mIndex - 1;
        }
        if(lowarray){
            delete[] lowarray;
            lowarray = 0;
        }
        return result;
    }
    
    double FCScript::LOG(CVariable *var){
        return log10(getValue(var->m_parameters[0]));
    }
    
    double FCScript::MA(CVariable *var){
        CVariable *cParam = var->m_parameters[0];
        double close = getValue(cParam);
        int n = (int)getValue(var->m_parameters[1]);
        int closeFieldIndex = cParam->m_fieldIndex;
        if (closeFieldIndex == -1){
            if (!var->m_tempFields){
                var->createTempFields(1);
            }
            closeFieldIndex = var->m_tempFieldsIndex[0];
            m_dataSource->set3(m_index, closeFieldIndex, close);
        }
        double result = movingAverage(m_index, n, close, getDatas(closeFieldIndex, var->m_fieldIndex, m_index - 1, n));
        m_dataSource->set3(m_index, var->m_fieldIndex, result);
        return result;
    }
    
    double FCScript::MAX2(CVariable *var){
        double left = getValue(var->m_parameters[0]);
        double right = getValue(var->m_parameters[1]);
        if (left >= right){
            return left;
        }
        else{
            return right;
        }
    }
    
    double FCScript::MEMA(CVariable *var){
        double close = getValue(var->m_parameters[0]);
        int n = (int)getValue(var->m_parameters[1]);
        double lastMema = 0;
        if (m_index > 0){
            lastMema = m_dataSource->get3(m_index - 1, var->m_fieldIndex);
        }
        double result = simpleMovingAverage(close, lastMema, n, 1);
        m_dataSource->set3(m_index, var->m_fieldIndex, result);
        return result;
    }
    
    double FCScript::MIN2(CVariable *var){
        double left = getValue(var->m_parameters[0]);
        double right = getValue(var->m_parameters[1]);
        if (left <= right){
            return left;
        }
        else{
            return right;
        }
    }
    
    int FCScript::MINUTE(CVariable *var){
        int year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0, msecond = 0;
        FCStr::getDateByNum(m_dataSource->getXValue(m_index), &year, &month, &day, &hour, &minute, &second, &msecond);
        return minute;
    }
    
    double FCScript::MOD(CVariable *var){
        int left = (int)getValue(var->m_parameters[0]);
        int right = (int)getValue(var->m_parameters[1]);
        if (right != 0){
            return left % right;
        }
        return 0;
    }
    
    int FCScript::MONTH(CVariable *var){
        int year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0, msecond = 0;
        FCStr::getDateByNum(m_dataSource->getXValue(m_index), &year, &month, &day, &hour, &minute, &second, &msecond);
        return month;
    }
    
    int FCScript::NDAY(CVariable *var){
        int n = (int)getValue(var->m_parameters[2]);
        if (n < 0){
            n = m_dataSource->rowsCount();
        }
        else if (n > m_index + 1){
            n = m_index + 1;
        }
        int tempIndex = m_index;
        int result = 1;
        for (int i = 0; i < n; i++){
            if (getValue(var->m_parameters[0]) <= getValue(var->m_parameters[1])){
                result = 0;
                break;
            }
            m_index--;
        }
        m_index = tempIndex;
        return result;
    }
    
    int FCScript::NOT(CVariable *var){
        double value = getValue(var->m_parameters[0]);
        if (value == 0){
            return 1;
        }
        else{
            return 0;
        }
    }
    
    double FCScript::POLYLINE(CVariable *var){
        if (m_div){
            CVariable *cond = var->m_parameters[0];
            CVariable *price = var->m_parameters[1];
            PolylineShape *polylineShape = 0;
            if (!var->m_polylineShape){
                String strColor = L"FCColorAUTO";
                String strLineWidth = L"LINETHICK";
                bool dotLine = false;
                for (int i = 2; i < var->m_parametersLength; i++){
                    String strParam = var->m_parameters[i]->m_expression;
                    if (strParam.length() > 5 && strParam.substr(0, 5) == L"FCColor"){
                        strColor = strParam;
                    }
                    else if (strParam.length() >= 9 && strParam.substr(0, 9) == L"LINETHICK"){
                        strLineWidth = strParam;
                    }
                    else if (strParam == L"DOTLINE"){
                        dotLine = true;
                    }
                }
                polylineShape = new PolylineShape();
                m_div->addShape(polylineShape);
                polylineShape->setAttachVScale(m_attachVScale);
                polylineShape->setColor(getColor(strColor));
                polylineShape->setWidth(getLineWidth(strLineWidth));
                var->createTempFields(1);
                polylineShape->setFieldText(price->m_fieldText);
                if (dotLine){
                    polylineShape->setStyle(PolylineStyle_DotLine);
                }
                var->m_polylineShape = polylineShape;
            }
            else{
                polylineShape = var->m_polylineShape;
            }
            if (price->m_expression.length() > 0){
                if (polylineShape->getFieldName() == FCDataTable::NULLFIELD()){
                    if (price->m_field != FCDataTable::NULLFIELD()){
                        polylineShape->setFieldName(price->m_field);
                    }
                    else{
                        price->createTempFields(1);
                        polylineShape->setFieldName(price->m_tempFields[0]);
                    }
                    for (int i = 2; i < var->m_parametersLength; i++){
                        String strParam = var->m_parameters[i]->m_expression;
                        if (strParam == L"DRAWTITLE"){
                            if (polylineShape->getFieldText().length() > 0){
                                m_div->getTitleBar()->Titles.add(new ChartTitle(polylineShape->getFieldName(), polylineShape->getFieldText(),
                                                                                  
                                                                                  polylineShape->getColor(), 2, true));
                            }
                        }
                    }
                }
                if (price->m_tempFieldsIndex){
                    double value = getValue(price);
                    m_dataSource->set3(m_index, price->m_tempFieldsIndex[0], value);
                }
            }
            double dCond = 1;
            if (cond->m_expression.length() > 0 && cond->m_expression != L"1"){
                dCond = getValue(cond);
                if (dCond != 1){
                    m_dataSource->set3(m_index, var->m_tempFieldsIndex[0], -10000);
                }
                else{
                    m_dataSource->set3(m_index, var->m_tempFieldsIndex[0], 1);
                }
            }
        }
        return 0;
    }
    
    double FCScript::POW(CVariable *var){
        double left = getValue(var->m_parameters[0]);
        double right = getValue(var->m_parameters[1]);
        return pow(left, right);
    }
    
    int FCScript::RAND(CVariable *var){
        int n = (int)getValue(var->m_parameters[0]);
        long timestamp = (long)time(0);
        srand(timestamp);
        return rand() % (n + 1);;
    }
    
    double FCScript::REF(CVariable *var){
        int param = (int)getValue(var->m_parameters[1]);
        param = m_index - param;
        double result = 0;
        if (param >=0){
            int tempIndex = m_index;
            m_index = param;
            result = getValue(var->m_parameters[0]);
            m_index = tempIndex;
        }
        return result;
    }
    
    double FCScript::RETURN(CVariable *var){
        m_resultVar.m_type = -1;
        m_result = getValue(var->m_parameters[0]);
        if (m_tempVars.containsKey(var->m_parameters[0]->m_field)){
            CVar *copyVar = m_tempVars.get(var->m_parameters[0]->m_field);
            m_resultVar.m_type = copyVar->m_type;
            m_resultVar.m_str = copyVar->m_str;
            m_resultVar.m_num = copyVar->m_num;
        }
        else{
            if (var->m_parameters[0]->m_expression.find(L"\'") == 0){
                m_resultVar.m_type = 1;
                m_resultVar.m_str = var->m_parameters[0]->m_expression;
            }
        }
        m_break = 1;
        return m_result;
    }
    
    double FCScript::REVERSE(CVariable *var){
        return -getValue(var->m_parameters[0]);
    }
    
    double FCScript::ROUND(CVariable *var){
        return (int)(getValue(var->m_parameters[0]));
    }
    
    double FCScript::SAR(CVariable *var){
        int n = (int)getValue(var->m_parameters[2]);
        double s = getValue(var->m_parameters[3]) / 100;
        double m = getValue(var->m_parameters[4]) / 100;
        double high = 0, low = 0;
        CVariable *hParam = var->m_parameters[0];
        CVariable *lParam = var->m_parameters[1];
        high = getValue(hParam);
        low = getValue(lParam);
        if (!var->m_tempFields){
            if (hParam->m_field == FCDataTable::NULLFIELD() || lParam->m_field == FCDataTable::NULLFIELD()){
                var->createTempFields(4);
            }
            else{
                var->createTempFields(2);
            }
        }
        int highField = hParam->m_field;
        int highFieldIndex = hParam->m_fieldIndex;
        if (highField == FCDataTable::NULLFIELD()){
            highField = var->m_tempFields[2];
            highFieldIndex = var->m_tempFieldsIndex[2];
            m_dataSource->set3(m_index, highFieldIndex, high);
        }
        int lowField = lParam->m_field;
        int lowFieldIndex = lParam->m_fieldIndex;
        if (lowField == FCDataTable::NULLFIELD()){
            lowField = var->m_tempFields[3];
            lowFieldIndex = var->m_tempFieldsIndex[3];
            m_dataSource->set3(m_index, lowFieldIndex, low);
        }
        int high_listLength = 0, low_listLength = 0;
        double *high_list = m_dataSource->DATA_ARRAY(highField, m_index - 1, n , &high_listLength);
        double *low_list = m_dataSource->DATA_ARRAY(lowField, m_index - 1, n, &low_listLength);
        double hhv = maxValue(high_list, high_listLength);
        double llv = minValue(low_list, low_listLength);
        if(high_list){
            delete[] high_list;
            high_list = 0;
        }
        if(low_list){
            delete[] low_list;
            low_list = 0;
        }
        int lastState = 0;
        double lastSar = 0;
        double lastAf = 0;
        if (m_index > 0){
            lastState = (int)m_dataSource->get3(m_index - 1, var->m_tempFieldsIndex[0]);
            lastSar = m_dataSource->get3(m_index - 1, var->m_fieldIndex);
            lastAf = m_dataSource->get3(m_index - 1, var->m_tempFieldsIndex[1]);
        }
        int state = 0;
        double af = 0, sar = 0;
        stopAndReverse(m_index, n, s, m, high, low, hhv, llv, lastState, lastSar, lastAf, &state, &af, &sar);
        m_dataSource->set3(m_index, var->m_tempFieldsIndex[1], af);
        m_dataSource->set3(m_index, var->m_tempFieldsIndex[0], state);
        m_dataSource->set3(m_index, var->m_fieldIndex, sar);
        return sar;
    }
    
    double FCScript::SET(CVariable *var){
        int pLen = var->m_parametersLength;
        for (int i = 0; i < pLen; i++){
            if (i % 2 == 0){
                CVariable *variable = var->m_parameters[i];
                CVariable *parameter = var->m_parameters[i + 1];
                setVariable(variable, parameter);
            }
        }
        return 0;
    }
    
    int FCScript::SIGN(CVariable *var){
        double value = getValue(var->m_parameters[0]);
        if (value > 0){
            return 1;
        }
        else if (value < 0){
            return -1;
        }
        return 0;
    }
    
    double FCScript::SIN(CVariable *var){
        return sin(getValue(var->m_parameters[0]));
    }
    
    double FCScript::SMA(CVariable *var){
        double close = getValue(var->m_parameters[0]);
        int n = (int)getValue(var->m_parameters[1]);
        int m = (int)getValue(var->m_parameters[2]);
        double lastSma = 0;
        if (m_index > 0){
            lastSma = m_dataSource->get3(m_index - 1, var->m_fieldIndex);
        }
        double result = simpleMovingAverage(close, lastSma, n, m);
        m_dataSource->set3(m_index, var->m_fieldIndex, result);
        return result;
    }
    
    double FCScript::SQRT(CVariable *var){
        return sqrt(getValue(var->m_parameters[0]));
    }
    
    double FCScript::SQUARE(CVariable *var){
        double result = getValue(var->m_parameters[0]);
        result = result * result;
        return result;
    }
    
    double FCScript::STD(CVariable *var){
        int p = (int)getValue(var->m_parameters[1]);
        CVariable *cParam = var->m_parameters[0];
        double close = getValue(cParam);
        int closeField = cParam->m_field;
        int closeFieldIndex = cParam->m_fieldIndex;
        if (closeField == FCDataTable::NULLFIELD()){
            if (!var->m_tempFields){
                var->createTempFields(1);
            }
            closeField = var->m_tempFields[0];
            closeFieldIndex = var->m_tempFieldsIndex[0];
            m_dataSource->set3(m_index, closeFieldIndex, close);
        }
        int listLength = 0;
        double *list = m_dataSource->DATA_ARRAY(closeField, m_index, p, &listLength);
        double avg = 0;
        if (list && listLength > 0){
            double sum = 0;
            for (int i = 0; i < listLength; i++){
                sum += list[i];
            }
            avg = sum / listLength;
        }
        double result = standardDeviation(list, listLength, avg, 1);
        if(list){
            delete[] list;
            list = 0;
        }
        return result;
    }
    
    double FCScript::STICKLINE(CVariable *var){
        if (m_div){
            CVariable *cond = var->m_parameters[0];
            CVariable *price1 = var->m_parameters[1];
            CVariable *price2 = var->m_parameters[2];
            CVariable *width = var->m_parameters[3];
            CVariable *empty = var->m_parameters[4];
            BarShape *barShape = 0;
            if (!var->m_barShape){
                barShape = new BarShape();
                m_div->addShape(barShape);
                barShape->setAttachVScale(m_attachVScale);
                barShape->setFieldText(price1->m_fieldText);
                barShape->setFieldText2(price2->m_fieldText);
                CVariable *color = 0;
                for (int i = 5; i < var->m_parametersLength; i++){
                    String strParam = var->m_parameters[i]->m_expression;
                    if (strParam.length() > 5 && strParam.substr(0, 5) == L"FCColor"){
                        color = var->m_parameters[5];
                        break;
                    }
                }
                if (color){
                    barShape->setUpColor(getColor(color->m_expression));
                    barShape->setDownColor(getColor(color->m_expression));
                }
                else{
                    barShape->setUpColor(FCColor::argb(255, 82, 82));
                    barShape->setDownColor(FCColor::argb(82, 255, 255));
                }
                barShape->setStyle(BarStyle_Line);
                var->createTempFields(1);
                barShape->setStyleField(var->m_tempFields[0]);
                barShape->setLineWidth(FCStr::convertStrToInt(width->m_expression.c_str()));
                var->m_barShape = barShape;
            }
            else{
                barShape = var->m_barShape;
            }
            if (price1->m_expression.length() > 0){
                if (barShape->getFieldName() == FCDataTable::NULLFIELD()){
                    if (price1->m_field != FCDataTable::NULLFIELD()){
                        barShape->setFieldName(price1->m_field);
                    }
                    else{
                        price1->createTempFields(1);
                        barShape->setFieldName(price1->m_tempFields[0]);
                    }
                    for (int i = 5; i < var->m_parametersLength; i++){
                        String strParam = var->m_parameters[i]->m_expression;
                        if (strParam == L"DRAWTITLE"){
                            if (barShape->getFieldText().length() > 0){
                                m_div->getTitleBar()->Titles.add(new ChartTitle(barShape->getFieldName(), barShape->getFieldText(), barShape->getDownColor(), 2, true));
                            }
                            break;
                        }
                    }
                }
                if (price1->m_tempFieldsIndex){
                    double value = getValue(price1);
                    m_dataSource->set3(m_index, price1->m_tempFieldsIndex[0], value);
                }
            }
            if (price2->m_expression.length() > 0 && price2->m_expression != L"0"){
                if (price2->m_field != FCDataTable::NULLFIELD()){
                    barShape->setFieldName2(price2->m_field);
                }
                else{
                    price2->createTempFields(1);
                    barShape->setFieldName2(price2->m_tempFields[0]);
                }
                if (price2->m_tempFieldsIndex){
                    double value = getValue(price2);
                    m_dataSource->set3(m_index, price2->m_tempFieldsIndex[0], value);
                }
            }
            double dCond = 1;
            if (cond->m_expression.length() > 0 && cond->m_expression != L"1"){
                dCond = getValue(cond);
                if (dCond != 1){
                    m_dataSource->set3(m_index, var->m_tempFieldsIndex[0], -10000);
                }
                else{
                    int dEmpty = 2;
                    if (empty->m_expression.length() > 0){
                        dEmpty = (int)getValue(empty);
                        m_dataSource->set3(m_index, var->m_tempFieldsIndex[0], dEmpty);
                    }
                }
            }
        }
        return 0;
    }
    
    double FCScript::SUM(CVariable *var){
        double close = getValue(var->m_parameters[0]);
        int closeFieldIndex = var->m_parameters[0]->m_fieldIndex;
        if (closeFieldIndex == -1){
            if (!var->m_tempFields){
                var->createTempFields(1);
            }
            closeFieldIndex = var->m_tempFieldsIndex[0];
            m_dataSource->set3(m_index, closeFieldIndex, close);
        }
        int n = (int)getValue(var->m_parameters[1]);
        if (n == 0){
            n = m_index + 1;
        }
        double result = sumilationValue(m_index, n, close, getDatas(closeFieldIndex, var->m_fieldIndex, m_index - 1, n));
        m_dataSource->set3(m_index, var->m_fieldIndex, result);
        return result;
    }
    
    double FCScript::TAN(CVariable *var){
        return tan(getValue(var->m_parameters[0]));
    }
    
    int FCScript::TIME(CVariable *var){
        int year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0, msecond = 0;
        FCStr::getDateByNum(m_dataSource->getXValue(m_index), &year, &month, &day, &hour, &minute, &second, &msecond);
        return hour * 100 + minute;
    }
    
    int FCScript::TIME2(CVariable *var){
        int year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0, msecond = 0;
        FCStr::getDateByNum(m_dataSource->getXValue(m_index), &year, &month, &day, &hour, &minute, &second, &msecond);
        return hour * 10000 + minute * 100 + second;
    }
    
    double FCScript::TMA(CVariable *var){
        double close = getValue(var->m_parameters[0]);
        int n = (int)getValue(var->m_parameters[1]);
        int m = (int)getValue(var->m_parameters[2]);
        double lastTma = 0;
        if (m_index > 0){
            lastTma = m_dataSource->get3(m_index - 1, var->m_fieldIndex);
        }
        double result = n * lastTma + m * close;
        m_dataSource->set3(m_index, var->m_fieldIndex, result);
        return result;
    }
    
    int FCScript::UPNDAY(CVariable *var){
        int n = (int)getValue(var->m_parameters[0]);
        if (n < 0){
            n = m_dataSource->rowsCount();
        }
        else if (n > m_index + 1){
            n = m_index + 1;
        }
        int tempIndex = m_index;
        int result = 1;
        for (int i = 0; i < n; i++){
            double right = getValue(var->m_parameters[0]);
            m_index--;
            double left = m_index >= 0 ? getValue(var->m_parameters[0]) : 0;
            if (right <= left){
                result = 0;
                break;
            }
        }
        m_index = tempIndex;
        return result;
    }
    
    double FCScript::VALUEWHEN(CVariable *var){
        int n = m_dataSource->rowsCount();
        int tempIndex = m_index;
        double result = 0;
        for (int i = 0; i < n; i++){
            double value = getValue(var->m_parameters[0]);
            if (value == 1){
                result = getValue(var->m_parameters[1]);
                break;
            }
            m_index--;
        }
        m_index = tempIndex;
        return result;
    }
    
    double FCScript::VAR(CVariable *var){
        double result = 0;
        int pLen = var->m_parametersLength;
        for(int i = 0; i < pLen; i++){
            if (i % 2 == 0){
                CVariable *name = var->m_parameters[i];
                CVariable *value = var->m_parameters[i + 1];
                int id = name->m_field;
                CVar *newCVar = m_varFactory->createVar();
                result = newCVar->onCreate(this, name, value);
                if(newCVar->m_type == 1){
                    name->m_functionID = -2;
                }
                if (m_tempVars.containsKey(id)){
                    CVar *cVar = m_tempVars.get(id);
                    newCVar->m_parent = cVar;
                }
                m_tempVars.put(id, newCVar);
            }
        }
        return result;
    }
    
    
    int FCScript::WHILE(CVariable *var){
        int pLen = var->m_parametersLength;
        if (pLen > 1){
            while (true){
                if (getValue(var->m_parameters[0]) <= 0){
                    break;
                }
                for (int i = 1; m_break == 0 && i < pLen; i++){
                    getValue(var->m_parameters[i]);
                }
                if (m_break > 0){
                    if (m_break == 3){
                        m_break = 0;
                        deleteTempVars(var);
                        continue;
                    }
                    else{
                        m_break = 0;
                        deleteTempVars(var);
                        break;
                    }
                }
                else{
                    deleteTempVars(var);
                }
            }
        }
        return 0;
    }
    
    double FCScript::WMA(CVariable *var){
        double close = getValue(var->m_parameters[0]);
        int n = (int)getValue(var->m_parameters[1]);
        int m = (int)getValue(var->m_parameters[2]);
        double lastWma = 0;
        if (m_index > 0){
            lastWma = m_dataSource->get3(m_index - 1, var->m_fieldIndex);
        }
        double result = weightMovingAverage(n, m, close, lastWma);
        m_dataSource->set3(m_index, var->m_fieldIndex, result);
        return result;
    }
    
    int FCScript::YEAR(CVariable *var){
        int year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0, msecond = 0;
        FCStr::getDateByNum(m_dataSource->getXValue(m_index), &year, &month, &day, &hour, &minute, &second, &msecond);
        return year;
    }
    
    double FCScript::ZIG(CVariable *var){
        double sxp = 0, exp = 0;
        int state = 0, sxi = 0, exi = 0;
        double p = getValue(var->m_parameters[1]);
        CVariable *cParam = var->m_parameters[0];
        double close = getValue(cParam);
        int closeFieldIndex = cParam->m_fieldIndex;
        if (!var->m_tempFields){
            if (closeFieldIndex == -1){
                var->createTempFields(6);
            }
            else{
                var->createTempFields(5);
            }
        }
        if (closeFieldIndex == -1){
            closeFieldIndex = var->m_tempFieldsIndex[5];
            m_dataSource->set3(m_index, closeFieldIndex, close);
        }
        if (m_index > 0){
            state = (int)m_dataSource->get3(m_index - 1, var->m_tempFieldsIndex[0]);
            exp = m_dataSource->get3(m_index - 1, var->m_tempFieldsIndex[1]);
            sxp = m_dataSource->get3(m_index - 1, var->m_tempFieldsIndex[2]);
            sxi = (int)m_dataSource->get3(m_index - 1, var->m_tempFieldsIndex[3]);
            exi = (int)m_dataSource->get3(m_index - 1, var->m_tempFieldsIndex[4]);
        }
        int cStart = -1, cEnd = -1;
        double k = 0, b = 0;
        zigZag(m_index, close, p, &sxp, &sxi, &exp, &exi, &state, &cStart, &cEnd, &k, &b);
        m_dataSource->set3(m_index, var->m_tempFieldsIndex[0], state);
        m_dataSource->set3(m_index, var->m_tempFieldsIndex[1], exp);
        m_dataSource->set3(m_index, var->m_tempFieldsIndex[2], sxp);
        m_dataSource->set3(m_index, var->m_tempFieldsIndex[3], sxi);
        m_dataSource->set3(m_index, var->m_tempFieldsIndex[4], exi);
        if (cStart != -1 && cEnd != -1){
            return 1;
        }
        else{
            return 0;
        }
    }
    
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    int FCScript::STR_CONTACT(CVariable *var){
        int pLen = var->m_parametersLength;
        String text = L"'";
        for (int i = 0; i < pLen; i++){
            text += getText(var->m_parameters[i]);
        }
        text += L"'";
        m_resultVar.m_type = 1;
        m_resultVar.m_str = text;
        return (int)text.length();
    }
    
    int FCScript::STR_FIND(CVariable *var){
        return (int)getText(var->m_parameters[0]).find(getText(var->m_parameters[1]));
    }
    
    
    int FCScript::STR_EQUALS(CVariable *var){
        int result = 0;
        if (getText(var->m_parameters[0]) == getText(var->m_parameters[1])){
            result = 1;
        }
        return result;
    }
    
    int FCScript::STR_FINDLAST(CVariable *var){
        return (int)getText(var->m_parameters[0]).rfind(getText(var->m_parameters[1]));
    }
    
    int FCScript::STR_LENGTH(CVariable *var){
        return (int)getText(var->m_parameters[0]).length();
    }
    
    int FCScript::STR_SUBSTR(CVariable *var){
        int pLen = var->m_parametersLength;
        CVariable *pName = var->m_parameters[0];
        if (pLen == 2){
            m_resultVar.m_type = 1;
            m_resultVar.m_str =  L"'" + getText(var->m_parameters[0]).substr((int)getValue(var->m_parameters[1])) + L"'";
        }
        else if (pLen >= 3){
            m_resultVar.m_type = 1;
            m_resultVar.m_str =  L"'" + getText(var->m_parameters[0]).substr((int)getValue(var->m_parameters[1]), (int)getValue(var->m_parameters[2])) +
            L"'";
        }
        return 0;
    }
    
    int FCScript::STR_REPLACE(CVariable *var){
        m_resultVar.m_type = 1;
        m_resultVar.m_str = L"'" + FCStr::replace(getText(var->m_parameters[0]), getText(var->m_parameters[1]), getText(var->m_parameters[2])) + L"'";
        return 0;
    }
    
    int FCScript::STR_SPLIT(CVariable *var){
        CVariable *pName = var->m_parameters[0];
        int id = pName->m_field;
        if (m_tempVars.containsKey(id)){
            ArrayList<String> *list = m_tempVars.get(id)->m_list;
            list->clear();
            ArrayList<String> strs = FCStr::split(getText(var->m_parameters[1]), getText(var->m_parameters[2]));
            int strsSize = (int)strs.size();
            for (int i = 0; i < strsSize; i++){
                list->add(strs.get(i));
            }
            return 1;
        }
        return 0;
    }
    
    int FCScript::STR_TOLOWER(CVariable *var){
        m_resultVar.m_type = 1;
        m_resultVar.m_str = FCStr::toLower(getText(var->m_parameters[0]));
        return 0;
    }
    
    int FCScript::STR_TOUPPER(CVariable *var){
        m_resultVar.m_type = 1;
        m_resultVar.m_str = FCStr::toUpper(getText(var->m_parameters[0]));
        return 0;
    }
    
    int FCScript::LIST_ADD(CVariable *var){
        CVariable *pName = var->m_parameters[0];
        int listName = pName->m_field;
        if (m_tempVars.containsKey(listName)){
            ArrayList<String> *list = m_tempVars.get(listName)->m_list;
            int pLen = var->m_parametersLength;
            for (int i = 1; i < pLen; i++){
                list->add(getText(var->m_parameters[i]));
            }
            return 1;
        }
        return 0;
    }
    
    int FCScript::LIST_CLEAR(CVariable *var){
        CVariable *pName = var->m_parameters[0];
        int listName = pName->m_field;
        if (m_tempVars.containsKey(listName)){
            ArrayList<String> *list = m_tempVars.get(listName)->m_list;
            list->clear();
            return 1;
        }
        return 0;
    }
    
    int FCScript::LIST_GET(CVariable *var){
        CVariable *pName = var->m_parameters[1];
        int listName = pName->m_field;
        if (m_tempVars.containsKey(listName)){
            ArrayList<String> *list = m_tempVars.get(listName)->m_list;
            int index = (int)getValue(var->m_parameters[2]);
            if (index < (int)list->size()){
                String strValue = list->get(index);
                CVariable *variable = var->m_parameters[0];
                int id = variable->m_field;
                int type = variable->m_type;
                switch (type){
                    case 2:{
                        double value = FCStr::convertStrToDouble(strValue);
                        m_dataSource->set3(m_index, variable->m_fieldIndex, value);
                        break;
                    }
                    default:
                        if(m_tempVars.containsKey(id)){
                            CVar *otherCVar = m_tempVars.get(id);
                            CVariable newVar;
                            newVar.m_indicator = this;
                            newVar.m_type = 1;
                            newVar.m_expression = L"'" + strValue + L"'";
                            otherCVar->setValue(this, variable, &newVar);
                        }
                        break;
                }
            }
            return 1;
        }
        return 0;
    }
    
    int FCScript::LIST_INSERT(CVariable *var){
        CVariable *pName = var->m_parameters[0];
        int listName = pName->m_field;
        if (m_tempVars.containsKey(listName)){
            ArrayList<String> *list = m_tempVars.get(listName)->m_list;
            list->insert((int)getValue(var->m_parameters[1]), getText(var->m_parameters[2]));
            return 1;
        }
        return 0;
    }
    
    int FCScript::LIST_REMOVE(CVariable *var){
        CVariable *pName = var->m_parameters[0];
        int listName = pName->m_field;
        if (m_tempVars.containsKey(listName)){
            ArrayList<String> *list = m_tempVars.get(listName)->m_list;
            list->removeAt((int)getValue(var->m_parameters[1]));
            return 1;
        }
        return 0;
    }
    
    int FCScript::LIST_SIZE(CVariable *var){
        int size = 0;
        CVariable *pName = var->m_parameters[0];
        int listName = pName->m_field;
        if (m_tempVars.containsKey(listName)){
            ArrayList<String> *list = m_tempVars.get(listName)->m_list;
            size = (int)list->size();
        }
        return size;
    }
    
    int FCScript::MAP_CLEAR(CVariable *var){
        CVariable *pName = var->m_parameters[0];
        int mapName = pName->m_field;
        if (m_tempVars.containsKey(mapName)){
            m_tempVars.get(mapName)->m_map->clear();
            return 1;
        }
        return 0;
    }
    
    int FCScript::MAP_CONTAINSKEY(CVariable *var){
        int result = 0;
        CVariable *pName = var->m_parameters[0];
        int mapName = pName->m_field;
        if (m_tempVars.containsKey(mapName)){
            HashMap<String, String> *map = m_tempVars.get(mapName)->m_map;
            if (map->containsKey(getText(var->m_parameters[1]))){
                result = 1;
            }
        }
        return result;
    }
    
    int FCScript::MAP_GET(CVariable *var){
        CVariable *pName = var->m_parameters[1];
        int mapName = pName->m_field;
        if (m_tempVars.containsKey(mapName)){
            HashMap<String, String> *map = m_tempVars.get(mapName)->m_map;
            String key = getText(var->m_parameters[2]);
            if (map->containsKey(key)){
                String strValue = map->get(key);
                CVariable *variable = var->m_parameters[0];
                int id = variable->m_field;
                int type = variable->m_type;
                switch (type){
                    case 2:{
                        double value = FCStr::convertStrToDouble(strValue);
                        m_dataSource->set3(m_index, variable->m_fieldIndex, value);
                        break;
                    }
                    default:
                        if (m_tempVars.containsKey(id)){
                            CVar *otherCVar = m_tempVars.get(id);
                            CVariable newVar;
                            newVar.m_indicator = this;
                            newVar.m_type = 1;
                            newVar.m_expression = L"'" + strValue + L"'";
                            otherCVar->setValue(this, variable, &newVar);
                        }
                        break;
                }
            }
            return 1;
        }
        return 0;
    }
    
    int FCScript::MAP_GETKEYS(CVariable *var){
        CVariable *pName = var->m_parameters[1];
        int mapName = pName->m_field;
        if (m_tempVars.containsKey(mapName)){
            int listName = var->m_parameters[0]->m_field;
            if (m_tempVars.containsKey(listName)){
                HashMap<String, String> *mapVar = m_tempVars.get(mapName)->m_map;
                ArrayList<String> *list = m_tempVars.get(listName)->m_list;
                list->clear();
                for(int m = 0; m < mapVar->size(); m++){
                    list->add(mapVar->getKey(m));
                }
                return 1;
            }
        }
        return 0;
    }
    
    int FCScript::MAP_REMOVE(CVariable *var){
        CVariable *pName = var->m_parameters[0];
        int mapName = pName->m_field;
        if (m_tempVars.containsKey(mapName)){
            HashMap<String, String> *map = m_tempVars.get(mapName)->m_map;
            map->remove(getText(var->m_parameters[1]));
            return 1;
        }
        return 0;
    }
    
    int FCScript::MAP_SET(CVariable *var){
        CVariable *pName = var->m_parameters[0];
        int mapName = pName->m_field;
        if (m_tempVars.containsKey(mapName)){
            HashMap<String, String> *map = m_tempVars.get(mapName)->m_map;
            map->put(getText(var->m_parameters[1]), getText(var->m_parameters[2]));
        }
        return 0;
    }
    
    int FCScript::MAP_SIZE(CVariable *var){
        int size = 0;
        CVariable *pName = var->m_parameters[0];
        int mapName = pName->m_field;
        if (m_tempVars.containsKey(mapName)){
            HashMap<String, String> *map = m_tempVars.get(mapName)->m_map;
            size = (int)map->size();
        }
        return size;
    }
    
    double FCScript::avedev(double value, double *listForAvedev, int listForAvedev_length, double avg){
        int i = 0;
        if(listForAvedev_length > 0){
            double sum = fabs(value - avg);
            for(i = 0;i < listForAvedev_length; i++){
                sum += fabs(listForAvedev[i] - avg);
            }
            return sum / listForAvedev_length;
        }
        else{
            return 0;
        }
    }
    
    double FCScript::avgValue(double *list, int length){
        int i = 0;
        double sum = 0;
        if(length > 0){
            for (i = 0; i < length; i++){
                sum += list[i];
            }
            return sum / length;
        }
        return 0;
    }
    
    double FCScript::exponentialMovingAverage(int n, double value, double lastEMA){
        return (value * 2 + lastEMA * (n - 1)) / (n + 1);
    }
    
    int FCScript::fibonacciValue(int index){
        if (index < 1){
            return 0;
        }
        else{
            int* vList;
            int i = 0,result = 0;
            vList = (int*)malloc(sizeof(int)*index);
            for (i = 0; i <= index - 1; i++){
                if (i == 0 || i == 1){
                    vList[i] = 1;
                }
                else{
                    vList[i] = vList[i - 1] + vList[i - 2];
                }
            }
            result = vList[index - 1];
            return result;
        }
    }
    
    void FCScript::linearRegressionEquation(double *list, int length, float *k, float *b){
        int i = 0;
        double sumX = 0;
        double sumY = 0;
        double sumUp = 0;
        double sumDown = 0;
        double xAvg = 0;
        double yAvg = 0;
        *k = 0;
        *b = 0;
        if(length > 1){
            for (i = 0; i < length; i++){
                sumX += i + 1;
                sumY += list[i];
            }
            xAvg = sumX / length;
            yAvg = sumY / length;
            for (i = 0; i < length; i++){
                sumUp += (i + 1 - xAvg) * (list[i] - yAvg);
                sumDown += (i + 1 - xAvg) * (i + 1 - xAvg);
            }
            *k = (float)(sumUp / sumDown);
            *b = (float)(yAvg - *k * xAvg);
        }
    }
    
    double FCScript::maxValue(double *list, int length){
        double max = 0;
        int i = 0;
        for(i = 0;i < length;i++){
            if(i == 0){
                max = list[i];
            }
            else{
                if(max < list[i]){
                    max = list[i];
                }
            }
        }
        return max;
    }
    
    double FCScript::minValue(double *list, int length){
        double min = 0;
        int i = 0;
        for(i = 0;i < length;i++){
            if(i == 0){
                min = list[i];
            }
            else{
                if(min > list[i]){
                    min = list[i];
                }
            }
        }
        return min;
    }
    
    double FCScript::movingAverage(int index, int n, double value, struct LPDATA last_MA){
        double sum = 0;
        if(last_MA.mode == 0){
            sum = last_MA.sum + value;
        }
        else{
            if(index > n-1){
                sum = last_MA.lastvalue * n;
                sum -= last_MA.first_value;
            }
            else{
                sum = last_MA.lastvalue*index;
                n = index + 1;
            }
            sum += value;
        }
        return sum / n;
    }
    
    double FCScript::simpleMovingAverage(double close, double lastSma, int n, int m){
        return (close * m + lastSma * (n - m)) / n;
    }
    
    double FCScript::standardDeviation(double *list, int length, double avg,  double standardDeviation){
        int i = 0;
        if(length > 0){
            double sum = 0;
            for (i = 0; i < length; i++){
                sum+=(list[i] - avg) * (list[i] - avg);
            }
            return standardDeviation * sqrt(sum / length);
        }
        else{
            return 0;
        }
    }
    
    void FCScript::stopAndReverse(int index, int n, double s, double m, double high, double low, double hhv, double llv, int last_state, double last_sar, double last_af, int *state, double *af, double *sar)
    {
        if (index >= n)
        {
            if (index == n)
            {
                *af = s;
                if (llv < low)
                {
                    *sar = llv;
                    *state = 1;
                }
                if (hhv > high)
                {
                    *sar = hhv;
                    *state = 2;
                }
            }
            else
            {
                *state = last_state;
                *af = last_af;
                if(*state==1)
                {
                    if (high > hhv)
                    {
                        *af += s;
                        if (*af > m)
                        {
                            *af = m;
                        }
                    }
                    *sar = last_sar + *af * (hhv - last_sar);
                    if (*sar < low)
                    {
                        *state = 1;
                    }
                    else
                    {
                        *state = 3;
                    }
                }
                else if(*state == 2)
                {
                    if (low < llv)
                    {
                        *af += s;
                        if (*af > m) *af = m;
                    }
                    *sar = last_sar + *af * (llv - last_sar);
                    if (*sar > high)
                    {
                        *state = 2;
                    }
                    else
                    {
                        *state = 4;
                    }
                }
                else if(*state==3)
                {
                    *sar = hhv;
                    if (*sar > high)
                    {
                        *state = 2;
                    }
                    else
                    {
                        *state = 4;
                    }
                    *af = s;
                }
                else if(*state==4)
                {
                    *sar = llv;
                    if (*sar < low)
                    {
                        *state = 1;
                    }
                    else
                    {
                        *state = 3;
                    }
                    *af = s;
                }
            }
        }
    }
    
    double FCScript::sumilationValue(int index, int n, double value, struct LPDATA last_SUM){
        double sum = 0;
        if(last_SUM.mode == 0){
            sum = last_SUM.sum + value;
        }
        else{
            sum=last_SUM.lastvalue;
            if(index > n-1){
                sum -= last_SUM.first_value;
            }
            sum += value;
        }
        return sum;
    }
    
    double FCScript::sumValue(double *list, int length){
        double sum = 0;
        int i = 0;
        for (i = 0; i < length; i++){
            sum += list[i];
        }
        return sum;
    }
    
    double FCScript::weightMovingAverage(int n, int weight, double value, double lastWMA){
        return (value * weight + (n - weight) * lastWMA) / n;
    }
    
    void FCScript::zigZag(int index, double close, double p, double *sxp, int *sxi, double *exp, int *exi, int *state,
                          int *cStart, int *cEnd, double *k, double *b){
        
    }
}
