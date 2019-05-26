/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.input;

import facecat.topin.scroll.*;
import facecat.topin.div.*;
import facecat.topin.core.*;
import java.util.*;

/**
 * 文本框控件
 */
public class FCTextBox extends FCDiv {

    /**
     * 创建控件
     */
    public FCTextBox() {
        FCSize size = new FCSize(100, 20);
        setSize(size);
    }

    /**
     * 键盘是否按下
     */
    protected boolean m_isKeyDown = false;

    /**
     * 是否触摸按下
     */
    protected boolean m_isTouchDown = false;

    /**
     * 横向偏移量
     */
    protected int m_offsetX = 0;

    /**
     * 文字矩形范围
     */
    protected ArrayList<FCRectF> m_ranges = new ArrayList<FCRectF>();

    /**
     * 重做栈
     */
    protected java.util.Stack<String> m_redoStack = new java.util.Stack<String>();

    /**
     * 是否显示光标
     */
    protected boolean m_showCursor = false;

    /**
     * 开始移动的坐标
     */
    protected int m_startMovingIndex = -1;

    /**
     * 结束移动的坐标
     */
    protected int m_stopMovingIndex = -1;

    /**
     * 文字是否改变
     */
    protected boolean m_textChanged = false;

    /**
     * 光标闪烁频率
     */
    protected static int TICK = 500;

    /**
     * 秒表ID
     */
    private int m_timerID = getNewTimerID();

    /**
     * 撤销栈
     */
    protected java.util.Stack<String> m_undoStack = new java.util.Stack<String>();

    /**
     * 文字大小
     */
    protected ArrayList<FCSizeF> m_wordsSize = new ArrayList<FCSizeF>();

    /**
     * 获取行数
     */
    public int getLinesCount() {
        return m_lines.size();
    }

    protected int m_lineHeight = 20;

    /**
     * 获取行高
     */
    public int getLineHeight() {
        return m_lineHeight;
    }

    /**
     * 设置行高
     */
    public void setLineHeight(int value) {
        m_lineHeight = value;
    }

    protected ArrayList<FCWordLine> m_lines = new ArrayList<FCWordLine>();

    /**
     * 获取行数
     */
    public ArrayList<FCWordLine> getLines() {
        return m_lines;
    }

    protected boolean m_multiline = false;

    /**
     * 获取是否多行显示
     */
    public boolean multiline() {
        return m_multiline;
    }

    /**
     * 设置是否多行显示
     */
    public void setMultiline(boolean value) {
        if (m_multiline != value) {
            m_multiline = value;
            m_textChanged = true;
        }
        setShowVScrollBar(m_multiline);
    }

    protected char m_passwordChar;

    /**
     * 获取密码字符
     */
    public char getPasswordChar() {
        return m_passwordChar;
    }

    /**
     * 设置密码字符
     */
    public void setPasswordChar(char value) {
        m_passwordChar = value;
        m_textChanged = true;
    }

    protected boolean m_readOnly = false;

    /**
     * 获取是否只读
     */
    public boolean isReadOnly() {
        return m_readOnly;
    }

    /**
     * 设置是否只读
     */
    public void setReadOnly(boolean value) {
        m_readOnly = value;
    }

    protected boolean m_rightToLeft;

    /**
     * 获取是否从右向左绘制
     */
    public boolean rightToLeft() {
        return m_rightToLeft;
    }

    /**
     * 设置是否从右向左绘制
     */
    public void setRightToLeft(boolean value) {
        m_rightToLeft = value;
        m_textChanged = true;
    }

    /**
     * 获取选中的文字
     */
    public String getSelectionText() {
        String text = getText();
        int textLength = text.length();
        if (textLength > 0 && m_selectionStart != textLength) {
            String selectedText = text.substring(m_selectionStart, m_selectionStart + m_selectionLength);
            return selectedText;
        }
        return "";
    }

    protected long m_selectionBackColor = FCColor.argb(10, 36, 106);

    /**
     * 获取选中的背景色
     */
    public long getSelectionBackColor() {
        return m_selectionBackColor;
    }

    /**
     * 设置选中的背景色
     */
    public void setSelectionBackColor(long value) {
        m_selectionBackColor = value;
    }

    protected long m_selectionTextColor = FCColor.argb(255, 255, 255);

    /**
     * 获取选中的前景色
     */
    public long getSelectionTextColor() {
        return m_selectionTextColor;
    }

    /**
     * 设置选中的前景色
     */
    public void setSelectionTextColor(long value) {
        m_selectionTextColor = value;
    }

    protected int m_selectionLength;

    /**
     * 获取选中长度
     */
    public int getSelectionLength() {
        return m_selectionLength;
    }

    /**
     * 设置选中长度
     */
    public void setSelectionLength(int value) {
        m_selectionLength = value;
    }

    protected int m_selectionStart = -1;

    /**
     * 获取选中开始位置
     */
    public int getSelectionStart() {
        return m_selectionStart;
    }

    /**
     * 设置选中开始位置
     */
    public void setSelectionStart(int value) {
        m_selectionStart = value;
    }

    protected String m_tempText;

    /**
     * 获取临时文字
     */
    public String getTempText() {
        return m_tempText;
    }

    /**
     * 设置临时文字
     */
    public void setTempText(String tempText) {
        m_tempText = tempText;
    }

    protected long m_tempTextColor = FCColor.DisabledText;

    /**
     * 获取临时文字的颜色
     */
    public long getTempTextColor() {
        return m_tempTextColor;
    }

    /**
     * 设置临时文字的颜色
     */
    public void setTempTextColor(long tempTextColor) {
        m_tempTextColor = tempTextColor;
    }

    protected FCHorizontalAlign m_textAlign = FCHorizontalAlign.Left;

    /**
     * 获取内容的横向排列样式
     */
    public FCHorizontalAlign getTextAlign() {
        return m_textAlign;
    }

    /**
     * 设置内容的横向排列样式
     */
    public void setTextAlign(FCHorizontalAlign value) {
        m_textAlign = value;
    }

    protected boolean m_wordWrap = false;

    /**
     * 获取多行编辑控件是否启动换行
     */
    public boolean wordWrap() {
        return m_wordWrap;
    }

    /**
     * 设置多行编辑控件是否启动换行
     */
    public void setwordWrap(boolean value) {
        if (m_wordWrap != value) {
            m_wordWrap = value;
            m_textChanged = true;
        }
        setShowHScrollBar(!m_wordWrap);
    }

    /**
     * 判断是否可以重复
     */
    public boolean canRedo() {
        if (m_redoStack.size() > 0) {
            return true;
        }
        return false;
    }

    /**
     * 判断是否可以撤销
     */
    public boolean canUndo() {
        if (m_undoStack.size() > 0) {
            return true;
        }
        return false;
    }

    /**
     * 重置
     */
    public void clearRedoUndo() {
        m_undoStack.clear();
        m_redoStack.clear();
    }

    /**
     * 删除字符
     */
    protected void deleteWord() {
        String text = getText();
        if (text == null) {
            text = "";
        }
        int textLength = text.length();
        if (textLength > 0) {
            int oldLines = m_lines.size();
            String left = "", right = "";
            int rightIndex = -1;
            if (m_selectionLength > 0) {
                left = m_selectionStart > 0 ? text.substring(0, m_selectionStart) : "";
                rightIndex = m_selectionStart + m_selectionLength;
            } else {
                left = m_selectionStart > 0 ? text.substring(0, m_selectionStart - 1) : "";
                rightIndex = m_selectionStart + m_selectionLength;
                if (m_selectionStart > 0) {
                    m_selectionStart -= 1;
                }
            }
            if (rightIndex < textLength) {
                right = text.substring(rightIndex);
            }
            String newText = left + right;
            m_text = text;
            textLength = newText.length();
            if (textLength == 0) {
                m_selectionStart = 0;
            } else {
                if (m_selectionStart > textLength) {
                    m_selectionStart = textLength;
                }
            }
            m_selectionLength = 0;
        }
    }

    /**
     * 销毁方法
     */
    @Override
    public void delete() {
        if (!isDeleted()) {
            stopTimer(m_timerID);
            m_lines.clear();
            m_redoStack.clear();
            m_undoStack.clear();
            m_wordsSize.clear();
        }
        super.delete();
    }

    @Override
    public int getContentHeight() {
        int hmax = super.getContentHeight();
        int cheight = 0;
        int rangeSize = m_ranges.size();
        for (int i = 0; i < rangeSize; i++) {
            if (cheight < m_ranges.get(i).bottom) {
                cheight = (int) m_ranges.get(i).bottom;
            }
        }
        return hmax > cheight ? hmax : cheight;
    }

    /**
     * 获取内容的高度
     */
    @Override
    public int getContentWidth() {
        int wmax = super.getContentWidth();
        int cwidth = 0;
        int rangeSize = m_ranges.size();
        for (int i = 0; i < rangeSize; i++) {
            if (cwidth < m_ranges.get(i).right) {
                cwidth = (int) m_ranges.get(i).right;
            }
        }
        return wmax > cwidth ? wmax : cwidth;
    }

    @Override
    public String getControlType() {
        return "TextBox";
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
        if (name.equals("lineheight")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getLineHeight());
        } else if (name.equals("multiline")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(multiline());
        } else if (name.equals("passwordchar")) {
            type.argvalue = "text";
            value.argvalue = (new Character(getPasswordChar())).toString();
        } else if (name.equals("readonly")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(isReadOnly());
        } else if (name.equals("righttoleft")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(rightToLeft());
        } else if (name.equals("selectionbackcolor")) {
            type.argvalue = "color";
            value.argvalue = FCStr.convertColorToStr(getSelectionBackColor());
        } else if (name.equals("selectiontextcolor")) {
            type.argvalue = "color";
            value.argvalue = FCStr.convertColorToStr(getSelectionTextColor());
        } else if (name.equals("temptext")) {
            type.argvalue = "text";
            value.argvalue = getTempText();
        } else if (name.equals("temptextcolor")) {
            type.argvalue = "color";
            value.argvalue = FCStr.convertColorToStr(getTempTextColor());
        } else if (name.equals("textalign")) {
            type.argvalue = "enum:FCHorizontalAlign";
            value.argvalue = FCStr.convertHorizontalAlignToStr(getTextAlign());
        } else if (name.equals("wordwrap")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(wordWrap());
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
        propertyNames.addAll(Arrays.asList(new String[]{"LineHeight", "Multiline", "PasswordChar", "ReadOnly", "RightToLeft", "SelectionBackColor", "SelectionTextColor", "TempText", "TempTextColor", "TextAlign", "WordWrap"}));
        return propertyNames;
    }

    /**
     * 判断字符索引所在行是否可见
     *
     * @param index 字符索引
     * @param visiblePercent 可见百分比
     * @returns 是否可见
     */
    protected boolean isLineVisible(int index, double visiblePercent) {
        int rangeSize = m_ranges.size();
        if (rangeSize > 0) {
            if (index >= 0 && index < rangeSize) {
                int top = 0, scrollV = 0, sch = 0;
                FCHScrollBar hScrollBar = getHScrollBar();
                FCVScrollBar vScrollBar = getVScrollBar();
                if (hScrollBar != null && hScrollBar.isVisible()) {
                    sch = hScrollBar.getHeight();
                }
                if (vScrollBar != null && vScrollBar.isVisible()) {
                    scrollV = -vScrollBar.getPos();
                }
                top = scrollV;
                int cell = 1;
                int floor = getHeight() - cell - sch - 1;
                FCRectF indexRect = m_ranges.get(index);
                int indexTop = (int) indexRect.top + scrollV;
                int indexBottom = (int) indexRect.bottom + scrollV;
                if (indexTop < cell) {
                    indexTop = cell;
                } else if (indexTop > floor) {
                    indexTop = floor;
                }
                if (indexBottom < cell) {
                    indexBottom = cell;
                } else if (indexBottom > floor) {
                    indexBottom = floor;
                }
                if (indexBottom - indexTop > m_lineHeight * visiblePercent) {
                    return true;
                }
            }
        }
        return false;
    }

    /**
     * 插入字符
     *
     * @param str 字符串
     */
    public void insertWord(String str) {
        String text = getText();
        if (text == null) {
            text = "";
        }
        if (text.length() == 0 || m_selectionStart == text.length()) {
            text = text + str;
            m_text = text;
            m_selectionStart = text.length();
        } else {
            int sIndex = m_selectionStart > text.length() ? text.length() - 1 : m_selectionStart;
            String left = sIndex > 0 ? text.substring(0, sIndex) : "";
            String right = "";
            int rightIndex = m_selectionStart + (m_selectionLength == 0 ? 0 : m_selectionLength);
            if (rightIndex < text.length()) {
                right = text.substring(rightIndex);
            }
            text = left + str + right;
            m_text = text;
            m_selectionStart += str.length();
            if (m_selectionStart > text.length()) {
                m_selectionStart = text.length();
            }
            m_selectionLength = 0;
        }
    }

    /**
     * 复制文字
     */
    @Override
    public void onCopy() {
        String selectionText = getSelectionText();
        if (selectionText != null && selectionText.length() > 0) {
            FCHost host = getNative().getHost();
            host.copy(selectionText);
            super.onCopy();
        }
    }

    /**
     * 剪切
     */
    @Override
    public void onCut() {
        if (!m_readOnly) {
            onCopy();
            int oldLines = m_lines.size();
            String redotext = getText();
            deleteWord();
            onTextChanged();
            if (m_textChanged) {
                m_undoStack.push(redotext);
            }
            invalidate();
            int newLines = m_lines.size();
            if (newLines != oldLines) {
                FCVScrollBar vScrollBar = getVScrollBar();
                if (vScrollBar != null) {
                    vScrollBar.setPos(vScrollBar.getPos() + m_lineHeight * (newLines - oldLines));
                    invalidate();
                }
            }
            super.onCut();
        }
    }

    /**
     * 获取焦点方法
     */
    @Override
    public void onGotFocus() {
        super.onGotFocus();
        m_showCursor = true;
        invalidate();
        startTimer(m_timerID, TICK);
    }

    /**
     * 丢失焦点方法
     */
    @Override
    public void onLostfocus() {
        super.onLostfocus();
        stopTimer(m_timerID);
        m_isKeyDown = false;
        m_showCursor = false;
        m_selectionLength = 0;
        invalidate();
    }

    /**
     * 触摸按下方法
     *
     * @param touchInfo 触摸信息
     */
    @Override
    public void onTouchDown(FCTouchInfo touchInfo) {
        super.onTouchDown(touchInfo.clone());
        FCPoint mp = touchInfo.m_firstPoint.clone();
        if (touchInfo.m_firstTouch) {
            /**
             * 单击
             */
            if (touchInfo.m_clicks == 1) {
                int sIndex = -1;
                int linesCount = m_lines.size();
                m_selectionLength = 0;
                m_startMovingIndex = -1;
                m_stopMovingIndex = -1;
                if (linesCount > 0) {
                    FCHScrollBar hScrollBar = getHScrollBar();
                    FCVScrollBar vScrollBar = getVScrollBar();
                    int scrollH = (hScrollBar != null && hScrollBar.isVisible()) ? hScrollBar.getPos() : 0;
                    int scrollV = (vScrollBar != null && vScrollBar.isVisible()) ? vScrollBar.getPos() : 0;
                    scrollH += m_offsetX;
                    int x = mp.x + scrollH, y = mp.y + scrollV;
                    FCRectF lastRange = new FCRectF();
                    int rangeSize = m_ranges.size();
                    if (rangeSize > 0) {
                        lastRange = m_ranges.get(rangeSize - 1);
                    }
                    for (int i = 0; i < linesCount; i++) {
                        FCWordLine line = m_lines.get(i);
                        for (int j = line.m_start; j <= line.m_end; j++) {
                            FCRectF rect = m_ranges.get(j);
                            if (i == linesCount - 1) {
                                rect.bottom += 3;
                            }
                            if (y >= rect.top && y <= rect.bottom) {
                                float sub = (rect.right - rect.left) / 2;
                                if ((x >= rect.left - sub && x <= rect.right - sub) || (j == line.m_start && x <= rect.left + sub) || (j == line.m_end && x >= rect.right - sub)) {
                                    if (j == line.m_end && x >= rect.right - sub) {
                                        sIndex = j + 1;
                                    } else {
                                        sIndex = j;
                                    }
                                    break;
                                }
                            }
                        }
                        if (sIndex != -1) {
                            break;
                        }
                    }
                    if (sIndex == -1) {
                        if ((y >= lastRange.top && y <= lastRange.bottom && x > lastRange.right) || (y >= lastRange.bottom)) {
                            sIndex = rangeSize;
                        }
                    }
                }
                if (sIndex != -1) {
                    m_selectionStart = sIndex;
                } else {
                    m_selectionStart = 0;
                }
                m_startMovingIndex = m_selectionStart;
                m_stopMovingIndex = m_selectionStart;
            } else if (touchInfo.m_clicks == 2) {
                if (!m_multiline) {
                    selectAll();
                }
            }
        }
        m_isTouchDown = true;
        m_showCursor = true;
        startTimer(m_timerID, TICK);
        invalidate();
    }

    /**
     * 触摸移动方法
     *
     * @param touchInfo 触摸信息
     */
    @Override
    public void onTouchMove(FCTouchInfo touchInfo) {
        super.onTouchMove(touchInfo.clone());
        FCPoint mp = touchInfo.m_firstPoint.clone();
        if (m_isTouchDown) {
            int linesCount = m_lines.size();
            if (linesCount > 0) {
                int eIndex = -1;
                FCHScrollBar hScrollBar = getHScrollBar();
                FCVScrollBar vScrollBar = getVScrollBar();
                int scrollH = (hScrollBar != null && hScrollBar.isVisible()) ? hScrollBar.getPos() : 0;
                int scrollV = (vScrollBar != null && vScrollBar.isVisible()) ? vScrollBar.getPos() : 0;
                scrollH += m_offsetX;
                FCPoint point = mp;
                if (point.x < 0) {
                    point.x = 0;
                }
                if (point.y < 0) {
                    point.y = 0;
                }
                int x = point.x + scrollH, y = point.y + scrollV;
                FCRectF lastRange = new FCRectF();
                int rangeSize = m_ranges.size();
                if (rangeSize > 0) {
                    lastRange = m_ranges.get(rangeSize - 1);
                }
                for (int i = 0; i < linesCount; i++) {
                    FCWordLine line = m_lines.get(i);
                    for (int j = line.m_start; j <= line.m_end; j++) {
                        FCRectF rect = m_ranges.get(j);
                        if (i == linesCount - 1) {
                            rect.bottom += 3;
                        }
                        if (eIndex == -1) {
                            if (y >= rect.top && y <= rect.bottom) {
                                float sub = (rect.right - rect.left) / 2;
                                if ((x >= rect.left - sub && x <= rect.right - sub) || (j == line.m_start && x <= rect.left + sub) || (j == line.m_end && x >= rect.right - sub)) {
                                    if (j == line.m_end && x >= rect.right - sub) {
                                        eIndex = j + 1;
                                    } else {
                                        eIndex = j;
                                    }
                                }
                            }
                        }
                    }
                    if (eIndex != -1) {
                        break;
                    }
                }
                if (eIndex != -1) {
                    m_stopMovingIndex = eIndex;
                }
                if (m_startMovingIndex == m_stopMovingIndex) {
                    m_selectionStart = m_startMovingIndex;
                    m_selectionLength = 0;
                } else {
                    m_selectionStart = m_startMovingIndex < m_stopMovingIndex ? m_startMovingIndex : m_stopMovingIndex;
                    m_selectionLength = Math.abs(m_startMovingIndex - m_stopMovingIndex);
                }
                invalidate();
            }
        }
    }

    /**
     * 触摸抬起方法
     *
     * @param touchInfo 触摸信息
     */
    @Override
    public void onTouchUp(FCTouchInfo touchInfo) {
        m_isTouchDown = false;
        super.onTouchUp(touchInfo.clone());
    }

    /**
     * 重绘前景方法
     *
     * @param paint 绘图对象
     * @param clipRect 裁剪区域
     */
    @Override
    public void onPaintForeground(FCPaint paint, FCRect clipRect) {
        int width = getWidth(), height = getHeight();
        if (width > 0 && height > 0) {
            int lineHeight = m_multiline ? m_lineHeight : height;
            FCPadding padding = getPadding();
            FCHost host = getNative().getHost();
            FCRect rect = new FCRect(0, 0, width, height);
            FCFont font = getFont();
            FCSizeF tSize = paint.textSizeF(" ", font);
            FCHScrollBar hScrollBar = getHScrollBar();
            FCVScrollBar vScrollBar = getVScrollBar();
            int vWidth = (vScrollBar != null && vScrollBar.isVisible()) ? (vScrollBar.getWidth() - padding.left - padding.right) : 0;
            int scrollH = ((hScrollBar != null && hScrollBar.isVisible()) ? hScrollBar.getPos() : 0);
            int scrollV = ((vScrollBar != null && vScrollBar.isVisible()) ? vScrollBar.getPos() : 0);
            float strX = padding.left + 1;
            // 绘制文字
            String text = getText();
            if (text == null) {
                text = "";
            }
            int length = text.length();
            FCSizeF bSize = paint.textSizeF("0", font);
            if (m_textChanged) {
                int line = 0, count = 0;
                // 获取绘制区域
                m_textChanged = !m_textChanged;
                m_lines.clear();
                m_ranges.clear();
                m_wordsSize.clear();
                for (int i = 0; i < length; i++) {
                    if (i == 0) {
                        count = 0;
                        line++;
                        strX = padding.left + 1;
                        m_lines.add(new FCWordLine(i, i));
                    }
                    char ch = text.charAt(i);
                    String dch = (new Character(ch)).toString();
                    // 制表符
                    if (ch == 9) {
                        int addCount = 4 - count % 4;
                        tSize.cx = bSize.cx * addCount;
                        tSize.cy = bSize.cy;
                        count += addCount;
                    } else {
                        // 密码替换
                        if (m_passwordChar != 0) {
                            dch = (new Character(m_passwordChar)).toString();
                        }
                        tSize = paint.textSizeF(dch, font);
                        if (ch == 10) {
                            tSize.cx = 0;
                        }
                        count++;
                    }
                    // 判断是否多行
                    if (m_multiline) {
                        boolean isNextLine = false;
                        if (ch == 13) {
                            isNextLine = true;
                            tSize.cx = 0;
                        } else {
                            // 是否自动换行
                            if (m_wordWrap) {
                                if (strX + tSize.cx > width - vWidth) {
                                    isNextLine = true;
                                }
                            }
                        }
                        // 换行
                        if (isNextLine) {
                            count = 0;
                            line++;
                            strX = padding.left + 1;
                            m_lines.add(new FCWordLine(i, i));
                        } else {
                            m_lines.set(line - 1, new FCWordLine(m_lines.get(line - 1).m_start, i));
                        }
                    } else {
                        m_lines.set(line - 1, new FCWordLine(m_lines.get(line - 1).m_start, i));
                    }
                    if (ch > 1000) {
                        tSize.cx += 1;
                    }
                    // 保存区域
                    FCRectF rangeRect = new FCRectF(strX, padding.top + (line - 1) * lineHeight, strX + tSize.cx, padding.top + line * lineHeight);
                    m_ranges.add(rangeRect);
                    m_wordsSize.add(tSize);
                    strX = rangeRect.right;
                }
                // 从右向左
                if (m_rightToLeft) {
                    int lcount = m_lines.size();
                    for (int i = 0; i < lcount; i++) {
                        FCWordLine ln = m_lines.get(i);
                        float lw = width - vWidth - (m_ranges.get(ln.m_end).right - m_ranges.get(ln.m_start).left) - 2;
                        if (lw > 0) {
                            for (int j = ln.m_start; j <= ln.m_end; j++) {
                                FCRectF rangeRect = m_ranges.get(j);
                                rangeRect.left += lw;
                                rangeRect.right += lw;
                                m_ranges.set(j, rangeRect);
                            }
                        }
                    }
                }
                update();
            }
            scrollH += m_offsetX;
            FCRect tempRect = new FCRect();
            int rangesSize = m_ranges.size();
            int offsetX = m_offsetX;
            // 判断当前索引是否可见
            if (!m_multiline) {
                FCRectF firstRange = new FCRectF();
                FCRectF lastRange = new FCRectF();
                if (rangesSize > 0) {
                    firstRange = m_ranges.get(0);
                    lastRange = m_ranges.get(rangesSize - 1);
                }
                scrollH -= offsetX;
                // 居中
                if (m_textAlign == FCHorizontalAlign.Center) {
                    offsetX = -(int) (width - padding.right - (lastRange.right - firstRange.left)) / 2;
                }
                // 远离
                else if (m_textAlign == FCHorizontalAlign.Right) {
                    offsetX = -(int) (width - padding.right - (lastRange.right - firstRange.left) - 3);
                }
                // 居左
                else {
                    // 显示超出边界
                    if (lastRange.right > width - padding.right) {
                        // 获取总是可见的索引
                        int alwaysVisibleIndex = m_selectionStart + m_selectionLength;
                        if (m_startMovingIndex != -1) {
                            alwaysVisibleIndex = m_startMovingIndex;
                        }
                        if (m_stopMovingIndex != -1) {
                            alwaysVisibleIndex = m_stopMovingIndex;
                        }
                        if (alwaysVisibleIndex > rangesSize - 1) {
                            alwaysVisibleIndex = rangesSize - 1;
                        }
                        if (alwaysVisibleIndex != -1) {
                            FCRectF alwaysVisibleRange = m_ranges.get(alwaysVisibleIndex);
                            int cw = width - padding.left - padding.right;
                            if (alwaysVisibleRange.left - offsetX > cw - 10) {
                                offsetX = (int) alwaysVisibleRange.right - cw + 3;
                                if (offsetX < 0) {
                                    offsetX = 0;
                                }
                            } else if (alwaysVisibleRange.left - offsetX < 10) {
                                offsetX -= (int) bSize.cx * 4;
                                if (offsetX < 0) {
                                    offsetX = 0;
                                }
                            }
                            if (offsetX > lastRange.right - cw) {
                                offsetX = (int) lastRange.right - cw + 3;
                            }
                        }
                    }
                    // 显示未超出边界
                    else {
                        if (m_textAlign == FCHorizontalAlign.Right) {
                            offsetX = -(int) (width - padding.right - (lastRange.right - firstRange.left) - 3);
                        } else {
                            offsetX = 0;
                        }
                    }
                }
                m_offsetX = offsetX;
                scrollH += m_offsetX;
            }
            int lineCount = m_lines.size();
            // 绘制矩形和字符
            ArrayList<FCRectF> selectedRanges = new ArrayList<FCRectF>();
            ArrayList<FCRect> selectedWordsRanges = new ArrayList<FCRect>();
            ArrayList<Character> selectedWords = new ArrayList<Character>();
            for (int i = 0; i < lineCount; i++) {
                FCWordLine line = m_lines.get(i);
                for (int j = line.m_start; j <= line.m_end; j++) {
                    char ch = text.charAt(j);
                    if (ch != 9) {
                        // 密码替换
                        if (m_passwordChar > 0) {
                            ch = m_passwordChar;
                        }
                    }
                    // 获取绘制区域
                    FCRectF rangeRect = m_ranges.get(j).clone();
                    rangeRect.left -= scrollH;
                    rangeRect.top -= scrollV;
                    rangeRect.right -= scrollH;
                    rangeRect.bottom -= scrollV;
                    FCRect rRect = new FCRect(rangeRect.left, rangeRect.top + (lineHeight - m_wordsSize.get(j).cy) / 2, rangeRect.right, rangeRect.top + (lineHeight + m_wordsSize.get(j).cy) / 2);
                    if (rRect.right == rRect.left) {
                        rRect.right = rRect.left + 1;
                    }
                    RefObject<FCRect> tempRef_tempRect = new RefObject<FCRect>(tempRect);
                    RefObject<FCRect> tempRef_rRect = new RefObject<FCRect>(rRect);
                    RefObject<FCRect> tempRef_rect = new RefObject<FCRect>(rect);
                    boolean tempVar = host.getIntersectRect(tempRef_tempRect, tempRef_rRect, tempRef_rect) > 0;
                    // 绘制文字
                    if (tempVar) {
                        if (m_selectionLength > 0) {
                            if (j >= m_selectionStart && j < m_selectionStart + m_selectionLength) {
                                selectedWordsRanges.add(rRect);
                                selectedRanges.add(rangeRect);
                                selectedWords.add(ch);
                                continue;
                            }
                        }
                        paint.drawText((new Character(ch)).toString(), getPaintingTextColor(), font, rRect);
                    }
                }
            }
            // 绘制选中区域
            int selectedRangesSize = selectedRanges.size();
            if (selectedRangesSize > 0) {
                int sIndex = 0, right = 0;
                for (int i = 0; i < selectedRangesSize; i++) {
                    FCRectF rRect = selectedRanges.get(i);
                    FCRectF sRect = selectedRanges.get(sIndex);
                    boolean newLine = rRect.top != sRect.top;
                    if (newLine || i == selectedRangesSize - 1) {
                        int eIndex = (i == selectedRangesSize - 1) ? i : i - 1;
                        FCRectF eRect = selectedRanges.get(eIndex);
                        FCRect unionRect = new FCRect(sRect.left, sRect.top, eRect.right + 1, sRect.bottom + 1);
                        if (newLine) {
                            unionRect.right = (int) right;
                        }
                        paint.fillRect(m_selectionBackColor, unionRect);
                        for (int j = sIndex; j <= eIndex; j++) {
                            paint.drawText(selectedWords.get(j).toString(), m_selectionTextColor, font, selectedWordsRanges.get(j));
                        }
                        sIndex = i;
                    }
                    right = (int) rRect.right;
                }
                selectedRanges.clear();
                selectedWords.clear();
                selectedWordsRanges.clear();
            }
            // 绘制光标
            if (isFocused() && !m_readOnly && m_selectionLength == 0 && (m_isKeyDown || m_showCursor)) {
                int index = m_selectionStart;
                if (index < 0) {
                    index = 0;
                }
                if (index > length) {
                    index = length;
                }
                // 获取光标的位置
                int cursorX = offsetX;
                int cursorY = 0;
                if (length > 0) {
                    if (index == 0) {
                        if (rangesSize > 0) {
                            cursorX = (int) m_ranges.get(0).left;
                            cursorY = (int) m_ranges.get(0).top;
                        }
                    } else {
                        cursorX = (int) Math.ceil(m_ranges.get(index - 1).right) + 2;
                        cursorY = (int) Math.ceil(m_ranges.get(index - 1).top) + 1;
                    }
                    cursorY += lineHeight / 2 - (int) tSize.cy / 2;
                } else {
                    cursorY = lineHeight / 2 - (int) tSize.cy / 2;
                }
                // 绘制闪烁光标
                if (m_isKeyDown || m_showCursor) {
                    FCRect cRect = new FCRect(cursorX - scrollH - 1, cursorY - scrollV, cursorX - scrollH + 1, cursorY + tSize.cy - scrollV);
                    paint.fillRect(getTextColor(), cRect);
                }
            } else {
                if (!isFocused() && text.length() == 0) {
                    if (m_tempText != null && m_tempText.length() > 0) {
                        FCSize pSize = paint.textSize(m_tempText, font);
                        FCRect pRect = new FCRect();
                        pRect.left = padding.left;
                        pRect.top = (lineHeight - pSize.cy) / 2;
                        pRect.right = pRect.left + pSize.cx;
                        pRect.bottom = pRect.top + pSize.cy;
                        paint.drawText(m_tempText, m_tempTextColor, font, pRect);
                    }
                }
            }
        }
    }

    /**
     * 粘贴方法
     */
    @Override
    public void onPaste() {
        if (!m_readOnly) {
            FCHost host = getNative().getHost();
            String insert = host.paste();
            if (insert != null && insert.length() > 0) {
                int oldLines = m_lines.size();
                String redotext = getText();
                insertWord(insert);
                onTextChanged();
                if (m_textChanged) {
                    if (redotext != null) {
                        m_undoStack.push(redotext);
                    }
                }
                invalidate();
                int newLines = m_lines.size();
                if (newLines != oldLines) {
                    FCVScrollBar vScrollBar = getVScrollBar();
                    if (vScrollBar != null) {
                        vScrollBar.setPos(vScrollBar.getPos() + m_lineHeight * (newLines - oldLines));
                        invalidate();
                    }
                }
                update();
                super.onPaste();
            }
        }
    }

    /**
     * 被使用TAB切换方法
     */
    @Override
    public void onTabStop() {
        super.onTabStop();
        if (!m_multiline) {
            if (getText() != null) {
                int textSize = getText().length();
                if (textSize > 0) {
                    m_selectionStart = 0;
                    m_selectionLength = textSize;
                    onTimer(m_timerID);
                }
            }
        }
    }

    /**
     * 文字尺寸改变事件
     */
    @Override
    public void onSizeChanged() {
        super.onSizeChanged();
        if (m_wordWrap) {
            m_textChanged = true;
            invalidate();
        }
    }

    /**
     * 文字改变方法
     */
    @Override
    public void onTextChanged() {
        m_textChanged = true;
        super.onTextChanged();
    }

    /**
     * 秒表回调方法
     *
     * @param timerID 秒表ID
     */
    @Override
    public void onTimer(int timerID) {
        super.onTimer(timerID);
        if (m_timerID == timerID) {
            if (isVisible() && isFocused() && !m_textChanged) {
                m_showCursor = !m_showCursor;
                invalidate();
            }
        }
    }

    /**
     * 重复
     */
    public void redo() {
        if (canRedo()) {
            if (getText() != null) {
                m_undoStack.push(getText());
            }
            setText(m_redoStack.pop());
        }
    }

    /**
     * 全选
     */
    public void selectAll() {
        m_selectionStart = 0;
        if (getText() != null) {
            m_selectionLength = getText().length();
        }
    }

    /**
     * 设置移动索引
     *
     * @param sIndex 开始索引
     * @param eIndex 结束索引
     */
    protected void setMovingIndex(int sIndex, int eIndex) {
        if (getText() != null) {
            int textSize = getText().length();
            if (textSize > 0) {
                if (sIndex < 0) {
                    sIndex = 0;
                }
                if (sIndex > textSize) {
                    sIndex = textSize;
                }
                if (eIndex < 0) {
                    eIndex = 0;
                }
                if (eIndex > textSize) {
                    eIndex = textSize;
                }
                m_startMovingIndex = sIndex;
                m_stopMovingIndex = eIndex;
                m_selectionStart = Math.min(m_startMovingIndex, m_stopMovingIndex);
                m_selectionLength = Math.abs(m_startMovingIndex - m_stopMovingIndex);
            }
        }
    }

    /**
     * 设置属性值
     *
     * @param name 属性名称
     * @param value 属性值
     */
    @Override
    public void setProperty(String name, String value) {
        if (name.equals("lineheight")) {
            setLineHeight(FCStr.convertStrToInt(value));
        } else if (name.equals("multiline")) {
            setMultiline(FCStr.convertStrToBool(value));
        } else if (name.equals("passwordchar")) {
            setPasswordChar(value.toCharArray()[0]);
        } else if (name.equals("readonly")) {
            setReadOnly(FCStr.convertStrToBool(value));
        } else if (name.equals("righttoleft")) {
            setRightToLeft(FCStr.convertStrToBool(value));
        } else if (name.equals("selectionbackcolor")) {
            setSelectionBackColor(FCStr.convertStrToColor(value));
        } else if (name.equals("selectiontextcolor")) {
            setSelectionTextColor(FCStr.convertStrToColor(value));
        } else if (name.equals("temptext")) {
            setTempText(value);
        } else if (name.equals("temptextcolor")) {
            setTempTextColor(FCStr.convertStrToColor(value));
        } else if (name.equals("textalign")) {
            setTextAlign(FCStr.convertStrToHorizontalAlign(value));
        } else if (name.equals("wordwrap")) {
            setwordWrap(FCStr.convertStrToBool(value));
        } else {
            super.setProperty(name, value);
        }
    }

    /**
     * 撤销
     */
    public void undo() {
        if (canUndo()) {
            if (getText() != null) {
                m_redoStack.push(getText());
            }
            setText(m_undoStack.pop());
        }
    }

    /**
     * 更新布局方法
     */
    @Override
    public void update() {
        FCNative inative = getNative();
        if (inative != null) {
            FCVScrollBar vScrollBar = getVScrollBar();
            if (vScrollBar != null) {
                vScrollBar.setLineSize(m_lineHeight);
            }
        }
        super.update();
    }
}
